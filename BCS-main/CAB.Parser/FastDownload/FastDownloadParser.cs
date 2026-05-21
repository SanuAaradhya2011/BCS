#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Parser.Entity;
using System.Text;
using System.Collections;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Class contains the functionality for parsing the fast download data
    /// </summary>
    public class FastDownloadParser : BaseParser
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private List<int> DataDefIdForEneregyParameters;
        private List<int> DataDefIdForDemandParameters;
        //FD Data Match string
        bool IsFDData = false;
        int FDDataConfigurationBitLength = 0;
        int FDDataLPInterval = 0;
        int FDDataMaxDaysConfigured = 0;
        int FDCurrentEntries = 0;
        int FDCheckSum = 0;
        string hexFDDATA = "464444415441";
        string NetMeterVariantInfo = string.Empty;
        int meterModel = 2;

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public FastDownloadParser(bool isLittleEndian)
            : base(isLittleEndian)
        {
            DataDefIdForEneregyParameters = new List<int>() { 31, 32, 33, 34, 48, 49, 50, 51, 304,305,306,307,
                                                              308,309,310,311,315,316,317,318,319,320,321,322,
                                                              362,363,364,365,366,367,368,369,370,371,372,373,
                                                              374,375,376,377};
            DataDefIdForDemandParameters = new List<int>() { 35,37,39,40,71,72,325,326,327,328,329,330,331,332,
                                                               343,344,345,346,347,348,349,350 };
        }
        #endregion

        #region Public Methods
        /// Parse the Hexadecimal string containing  meter data as per the data configuration supplied in the DataPacketConfiguration parameter
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="dataPacket"></param>
        /// <returns></returns>
        public List<MeterDataPacket> ParseProfile(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            #region test code
            bool testCode = false;
            if (testCode)
            {
                FileStream initialFileStream = null;
                StreamWriter writeToFile = null;
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"DLMSCommunication\" + DateTime.Now.Year + DateTime.Now.Month +
                    DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ".2NG";
                initialFileStream = new FileStream(fileName, FileMode.Create);
                writeToFile = new StreamWriter(initialFileStream);
                List<DataElementConfiguration> elementConfigs = dataPacket.DataElements;
                // Length of Array
                long arrayLength = (long)((profile.DataBuffer.Length) / (dataPacket.PacketLength * 2));
                long counter = 0;
                long tCounter = 0;
                int elementCounter = 0;
                List<string> data = new List<String>();
                if (dataPacket.PacketLength == 29)
                {
                    counter = 4;
                }

                while (tCounter < arrayLength)
                {
                    elementCounter = 0;
                    foreach (DataElementConfiguration elementConfig in elementConfigs)
                    {
                        int length = 0;
                        if (elementConfig.DataTypeID == 1001)
                        {
                            length = 6;
                        }
                        if (elementConfig.DataTypeID == 1006 || elementConfig.DataTypeID == 1003)
                        {
                            length = 4;
                        }
                        else if (elementConfig.DataTypeID == 1005)
                        {
                            length = 8;
                        }
                        else if (elementConfig.DataTypeID == 1004)
                        {
                            length = 6;
                        }
                        else if (elementConfig.DataTypeID == 1002)
                        {
                            length = 5;
                        }
                        else if (elementConfig.DataTypeID == 1001)
                        {
                            length = 6;
                        }
                        else if (elementConfig.DataTypeID == 16)
                        {
                            length = 2;
                        }
                        else if (elementConfig.DataTypeID == 6)
                        {
                            length = 4;
                        }
                        else if (elementConfig.DataTypeID == 5)
                        {
                            length = 4;
                        }

                        else if (elementConfig.DataTypeID == 18)
                        {
                            length = 2;
                        }
                        else if (elementConfig.DataTypeID == 17)
                        {
                            length = 1;
                        }
                        else if (elementConfig.DataTypeID == 21)
                        {
                            length = 8;
                        }
                        else if (elementConfig.DataTypeID == 1009)
                        {
                            length = 3;
                        }
                        else if (elementConfig.DataTypeID == 1007)
                        {
                            length = 4;
                        }

                        else if (elementConfig.DataTypeID == 1007)//add pradipta_fastdownload
                        {
                            length = 2;
                        }

                        elementCounter++;
                        data.Add(profile.DataBuffer.Substring((int)counter, (int)length * 2));
                        writeToFile.WriteLine(profile.DataBuffer.Substring((int)counter, (int)length * 2) + " :  " + elementCounter.ToString());
                        counter = counter + (int)length * 2;
                    }

                    if (dataPacket.PacketLength == 457)
                    {
                        long unusedBytes = 164 * 2;
                        counter = counter + unusedBytes;
                    }
                    writeToFile.WriteLine("new Record : " + (tCounter + 2));
                    tCounter++;

                }
                writeToFile.Close();
                initialFileStream.Close();
            }
            #endregion

            return Parse(SoapHexBinary.Parse(profile.DataBuffer).Value, dataPacket);

        }


        #region CommonParsingFuntion

        public bool IsBCCMatch(Byte[] bytes)
        {
            //**** <4 Byte Packet Length> + <4 Byte Configuration> + <Data> + <1 Byte CRC> ******// 
            bool IsBCCPass = false;
            try
            {
                int Bcc = 0;
                for (int i = 0; i < (bytes.Length - 1); i++)
                {
                    Bcc = Bcc ^ bytes[i];
                }               
                if (Bcc == bytes[bytes.Length - 1])
                {
                    IsBCCPass = true;
                }
            }
            catch (Exception ex)
            {
                
            }

            return IsBCCPass;
        }

        private bool CheckLength(byte[] fileStream, int StartIndex, int PacketByteLength)
        {
            //**** <4 Byte Packet Length> + <4 Byte Configuration> + <Data> + <1 Byte CRC> ******//       
            bool IslenghtMatch = false;
            try
            {
                UInt32 Length = BitConverter.ToUInt32(ReverseByte(fileStream, StartIndex, PacketByteLength), 0);
                if (Length == (UInt32)fileStream.Length)
                {
                    IslenghtMatch = true;
                }
            }
            catch (Exception ex)
            {                
                
            }           
            return IslenghtMatch;
        }

        private byte[] ReverseByte(byte[] source, int startindex, int length)
        {
            //**********Reverse Byte Array for Endian Support**************//
            List<byte> lstByte = new List<byte>();
            try
            {
                for (int i = startindex; i < (startindex + length); i++)
                {
                    lstByte.Add(source[i]);
                }
                lstByte.Reverse();            
            }
            catch (Exception ex)
            {                
                
            }
            return lstByte.ToArray();
        }

        private byte[] OriginalByte(byte[] source, int startindex, int length)
        {
            //**********Original Byte Array for Endian Support**************//
            List<byte> lstByte = new List<byte>();
            try
            {
                for (int i = startindex; i < (startindex + length); i++)
                {
                    lstByte.Add(source[i]);
                }
            }
            catch (Exception ex)
            {                
                
            }            
            return lstByte.ToArray();
        }

        private string ReverseString(string source)
        {
            //**********Reverse String Value for Endian Support**************//           
            System.Text.StringBuilder strRes = new System.Text.StringBuilder();
            try
            {
                int sourceLen = source.Length;
                while (sourceLen > 0)
                {
                    strRes.Append(source[--sourceLen]);
                }
            }
            catch (Exception ex)
            {                
                
            }            
            return strRes.ToString();
        }

        private string ByteTOBinary(byte[] source)
        {
            //***********Convert  Byte Array to Binary Equivalent ***********//
            string Result = string.Empty;
            try
            {
                for (int i = 0; i < source.Length; i++)
                {
                    Result = Convert.ToString(source[i], 2).PadLeft(8, '0') + Result;
                }
            }
            catch (Exception ex)
            {                
                
            }            
            return Result;
        }

        private string GetPacketConfigurationBit(byte[] fileStream, int StartIndex, int ConfigurationByteLength)
        {
            //**** <4 Byte Packet Length> + <4 Byte Configuration> + <Data> + <1 Byte CRC> ******//             
            string ConfigurationBit = string.Empty;
            try
            {
                ConfigurationBit = ByteTOBinary(ReverseByte(fileStream, StartIndex, ConfigurationByteLength));
            }
            catch (Exception ex)
            {                
                
            }            
            return ConfigurationBit;
        }

        public byte[] FromHex(string hex)
        {
            //***********Convert from Hex Byte String to equivalent Byte Array*****//
            byte[] raw = new byte[hex.Length / 2];
            try
            {
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
            }
            catch (Exception ex)
            {                
                
            }            
            return raw;
        }

        public static string GetParseValue(byte[] sourceBuffer, int ByteLength)
        {
            try
            {
                switch (ByteLength)
                {
                    case 2:
                        {
                            return Convert.ToString(BitConverter.ToUInt16(sourceBuffer, 0));
                        }

                    case 3:
                        {
                            byte[] btsrc = new byte[4];
                            btsrc[0] = sourceBuffer[0];
                            btsrc[1] = sourceBuffer[1];
                            btsrc[2] = sourceBuffer[2];
                            btsrc[3] = 0x00;
                            return Convert.ToString(BitConverter.ToUInt32(btsrc, 0));
                        }

                    case 4:
                        {
                            return Convert.ToString(BitConverter.ToUInt32(sourceBuffer, 0));
                        }

                    case 8:
                        {
                            return Convert.ToString(BitConverter.ToUInt64(sourceBuffer, 0));
                        }

                    default:
                        return string.Empty;

                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }       
        }

        private DateTime GetDateFromBase(double value)
        {
            //**************Convert Seconds To DateTime considering 01-01-2000 00:00:00 as base ***********//
            DateTime baseDt = new DateTime(2000, 01, 01, 00, 00, 00);
            TimeSpan tp = TimeSpan.FromSeconds(value);
            DateTime dtNew = baseDt.Add(tp);
            return dtNew;
        }



        public static DateTime GetProfileTimestamp(string data)
        {
            DateTime dat = new DateTime(2000,1,1,0,0,0);
            try
            {
                string date = Convert.ToString(Convert.ToInt32(data.Substring(0, 2), 16), 2);
                string year = Convert.ToString(Convert.ToInt32(data.Substring(2, 2), 16));
                if (year.Length < 2) { year = "0" + year; }
                while (date.Length < 8) { date = "0" + date; }
                date = "0" + date.Substring(1);
                date = Convert.ToInt32(date, 2).ToString();
                if (date.Length < 2) { date = "0" + date; }
                date += "-" + year;
                string time = Convert.ToString(Convert.ToInt32(data.Substring(6, 2) + data.Substring(4, 2), 16), 2);
                while (time.Length < 16) { time = "0" + time; }
                string day = time.Substring(0, 5);
                while (day.Length < 8) { day = "0" + day; }
                string tempDay = Convert.ToInt32(day, 2).ToString();
                if (tempDay.Length < 2) { tempDay = "0" + tempDay; }
                date = tempDay + "-" + date; ;// Convert.ToString(Convert.ToInt32(time.Substring(0, 5), 16), 2) + "-" + date;

                string hour = time.Substring(5, 5);
                while (hour.Length < 8) { hour = "0" + hour; }
                string min = time.Substring(10, 6);
                while (min.Length < 8) { min = "0" + min; }
                string tempHour = Convert.ToInt32(hour, 2).ToString();
                if (tempHour.Length < 2) { tempHour = "0" + tempHour; }
                string tempMin = Convert.ToInt32(min, 2).ToString();
                if (tempMin.Length < 2) { tempMin = "0" + tempMin; }
                time = tempHour + ":" + tempMin;


                int iHour = Convert.ToInt32(tempHour);
                int iMinutes = Convert.ToInt32(tempMin);
                int iSeconds = 0;

                string[] lstDate = date.Split('-');

                if(lstDate.Length == 3)
                {
                    int iDay = Convert.ToInt32(lstDate[0]);
                    int iMonth = Convert.ToInt32(lstDate[1]);
                    int iYear = Convert.ToInt32(lstDate[2]);
                    iYear = 2000 + iYear;
                    dat = new DateTime(iYear, iMonth, iDay, iHour, iMinutes, iSeconds);
                }
            }
            catch (Exception ex)
            {
                
                
            }

            return dat;
        }


        private string ByteToHexString(byte[] Buffer, int startPosition, int BufferLength)
        {
            string Data = string.Empty;
            try
            {
                for (int i = startPosition; i < (startPosition + BufferLength); i++)
                {
                    Data = Data + Buffer[i].ToString("X").PadLeft(2, '0');
                }
            }
            catch (Exception ex)
            {


            }
            return Data;
        }


        private string GetHexString(byte[] Buffer, int BytePosition, int byteLength)
        {
            string Data = string.Empty;
            try
            {
                for (int i = BytePosition; i < (BytePosition + byteLength); i++)
                {
                    if (i == BytePosition)
                    {
                        Data = Buffer[i].ToString("X").PadLeft(2, '0');
                    }
                    else if(i < (BytePosition + 3))
                    {
                        Data = Data + "-" + Buffer[i].ToString("X").PadLeft(2, '0');
                    }
                    else if (i == (BytePosition + 3))
                    {
                        Data = Data + " " + Buffer[i].ToString("X").PadLeft(2, '0');
                    }
                    else
                    {
                        Data = Data + ":" + Buffer[i].ToString("X").PadLeft(2, '0');
                    }
                }
            }
            catch (Exception ex)
            {                
                
            }            
            return Data;
        }


        #endregion             
        

        #region TestParsingCode             

        //class TestForm : System.Windows.Forms.Form
        //{
        //    public System.Windows.Forms.Button button1;
        //    public System.Windows.Forms.Panel panel1;
        //    public System.Windows.Forms.DataGridView dataGridView1;
        //    public System.Windows.Forms.Label label1;
        //    public System.Windows.Forms.Label label2;
        //    public System.Windows.Forms.Label label3;
        //    public System.Windows.Forms.Label label4;
        //    public System.Windows.Forms.Label label5;
        //    public System.Windows.Forms.Label label6;
        //    public System.Windows.Forms.Label label7;
        //    public System.Windows.Forms.Label label8;


        //    private void InitializeComponent()
        //    {
        //        this.button1 = new System.Windows.Forms.Button();
        //        this.panel1 = new System.Windows.Forms.Panel();
        //        this.dataGridView1 = new System.Windows.Forms.DataGridView();
        //        this.label1 = new System.Windows.Forms.Label();
        //        this.label2 = new System.Windows.Forms.Label();
        //        this.label3 = new System.Windows.Forms.Label();
        //        this.label4 = new System.Windows.Forms.Label();
        //        this.label5 = new System.Windows.Forms.Label();
        //        this.label6 = new System.Windows.Forms.Label();
        //        this.label7 = new System.Windows.Forms.Label();
        //        this.label8 = new System.Windows.Forms.Label();
        //        this.panel1.SuspendLayout();
        //        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        //        this.SuspendLayout();
        //        // 
        //        // button1
        //        // 
        //        this.button1.Location = new System.Drawing.Point(1098, 12);
        //        this.button1.Name = "button1";
        //        this.button1.Size = new System.Drawing.Size(75, 23);
        //        this.button1.TabIndex = 0;
        //        this.button1.Text = "Close";
        //        this.button1.UseVisualStyleBackColor = true;
        //        // 
        //        // panel1
        //        // 
        //        this.panel1.Controls.Add(this.dataGridView1);
        //        this.panel1.Location = new System.Drawing.Point(12, 61);
        //        this.panel1.Name = "panel1";
        //        this.panel1.Size = new System.Drawing.Size(1161, 448);
        //        this.panel1.TabIndex = 1;
        //        // 
        //        // dataGridView1
        //        // 
        //        this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //        this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
        //        this.dataGridView1.Location = new System.Drawing.Point(0, 0);
        //        this.dataGridView1.Name = "dataGridView1";
        //        this.dataGridView1.Size = new System.Drawing.Size(1161, 448);
        //        this.dataGridView1.TabIndex = 0;
        //        // 
        //        // label1
        //        // 
        //        this.label1.AutoSize = true;
        //        this.label1.Location = new System.Drawing.Point(12, 9);
        //        this.label1.Name = "label1";
        //        this.label1.Size = new System.Drawing.Size(66, 13);
        //        this.label1.TabIndex = 1;
        //        this.label1.Text = "IsBCCMatch";
        //        // 
        //        // label2
        //        // 
        //        this.label2.AutoSize = true;
        //        this.label2.Location = new System.Drawing.Point(25, 31);
        //        this.label2.Name = "label2";
        //        this.label2.Size = new System.Drawing.Size(35, 13);
        //        this.label2.TabIndex = 2;
        //        this.label2.Text = "label2";
        //        // 
        //        // label3
        //        // 
        //        this.label3.AutoSize = true;
        //        this.label3.Location = new System.Drawing.Point(243, 9);
        //        this.label3.Name = "label3";
        //        this.label3.Size = new System.Drawing.Size(78, 13);
        //        this.label3.TabIndex = 3;
        //        this.label3.Text = "IsLengthMatch";
        //        // 
        //        // label4
        //        // 
        //        this.label4.AutoSize = true;
        //        this.label4.Location = new System.Drawing.Point(261, 31);
        //        this.label4.Name = "label4";
        //        this.label4.Size = new System.Drawing.Size(35, 13);
        //        this.label4.TabIndex = 4;
        //        this.label4.Text = "label4";
        //        // 
        //        // label5
        //        // 
        //        this.label5.AutoSize = true;
        //        this.label5.Location = new System.Drawing.Point(465, 9);
        //        this.label5.Name = "label5";
        //        this.label5.Size = new System.Drawing.Size(74, 13);
        //        this.label5.TabIndex = 5;
        //        this.label5.Text = "PacketLength";
        //        // 
        //        // label6
        //        // 
        //        this.label6.AutoSize = true;
        //        this.label6.Location = new System.Drawing.Point(488, 31);
        //        this.label6.Name = "label6";
        //        this.label6.Size = new System.Drawing.Size(35, 13);
        //        this.label6.TabIndex = 6;
        //        this.label6.Text = "label6";
        //        // 
        //        // label7
        //        // 
        //        this.label7.AutoSize = true;
        //        this.label7.Location = new System.Drawing.Point(719, 9);
        //        this.label7.Name = "label7";
        //        this.label7.Size = new System.Drawing.Size(69, 13);
        //        this.label7.TabIndex = 7;
        //        this.label7.Text = "Configuration";
        //        // 
        //        // label8
        //        // 
        //        this.label8.AutoSize = true;
        //        this.label8.Location = new System.Drawing.Point(737, 31);
        //        this.label8.Name = "label8";
        //        this.label8.Size = new System.Drawing.Size(35, 13);
        //        this.label8.TabIndex = 8;
        //        this.label8.Text = "label8";
        //        // 
        //        // TestParse
        //        // 
        //        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //        this.ClientSize = new System.Drawing.Size(1196, 521);
        //        this.Controls.Add(this.label8);
        //        this.Controls.Add(this.label7);
        //        this.Controls.Add(this.label6);
        //        this.Controls.Add(this.label5);
        //        this.Controls.Add(this.label4);
        //        this.Controls.Add(this.label3);
        //        this.Controls.Add(this.label2);
        //        this.Controls.Add(this.label1);
        //        this.Controls.Add(this.panel1);
        //        this.Controls.Add(this.button1);
        //        this.Name = "TestParse";
        //        this.Text = "TestParse";
        //        this.Load += new System.EventHandler(this.TestForm_Load);
        //        this.panel1.ResumeLayout(false);
        //        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        //        this.ResumeLayout(false);
        //        this.PerformLayout();

        //    }

        //    public TestForm()
        //    {
        //        InitializeComponent();
        //    }

        //    private void TestForm_Load(object sender, EventArgs e)
        //    {
        //        bool flag = BitConverter.IsLittleEndian;
        //    }
        //}

        //TestForm formobj = null;

        //private void TestParsingSP(int PacketStartBytePosition, bool isBCCMatch, bool isPacketLengthMatch, int PacketLength, string PacketConfiguration, Byte[] Buffer, List<DataElementConfiguration> elementConfigs)
        //{

        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {
        //        formobj = new TestForm();



        //        //***********Columns For DataTable*************//
        //        dt.Columns.Add("SNo", typeof(int));
        //        foreach (DataElementConfiguration item in elementConfigs)
        //        {
        //            dt.Columns.Add(item.DataDefID + "_" + item.Unit, typeof(string));
        //        }
        //        dt.Columns.Add("FormatDate", typeof(string));


        //        formobj.label2.Text = isBCCMatch.ToString();
        //        formobj.label4.Text = isPacketLengthMatch.ToString();
        //        formobj.label6.Text = PacketLength.ToString();
        //        formobj.label8.Text = PacketConfiguration.ToString();


        //        int BytePosition = PacketStartBytePosition;
        //        int indexRow = 0;
        //        while (BytePosition < (Buffer.Length - 1))
        //        {
        //            int ClmIterator = 0;
        //            System.Data.DataRow dr = dt.NewRow();
        //            dr[ClmIterator++] = ++indexRow;
        //            for (int iteratorIndex = 0; iteratorIndex < elementConfigs.Count; iteratorIndex++)
        //            {
        //                string Val = "Absent";
        //                if (PacketConfiguration[iteratorIndex] == '1')
        //                {
        //                    int byteLength = elementConfigs[iteratorIndex].LengthOfDataType;
        //                    Byte[] TempBt = OriginalByte(Buffer, BytePosition, byteLength);
        //                    Val = GetParseValue(TempBt, byteLength);
        //                    BytePosition = BytePosition + byteLength;
        //                    if (elementConfigs[iteratorIndex].DataDefID == 9 || elementConfigs[iteratorIndex].DataDefID == 41 || elementConfigs[iteratorIndex].DataDefID == 9 || elementConfigs[iteratorIndex].DataDefID == 30)
        //                    {
        //                        UInt64 DatVal;
        //                        if (UInt64.TryParse(Val, out DatVal))
        //                        {
        //                            dr["FormatDate"] = GetDateFromBase(DatVal).ToString("dd-MM-yyyy HH:mm:ss");
        //                        }
        //                    }
        //                }
        //                dr[ClmIterator++] = Val;
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //System.Windows.Forms.MessageBox.Show(ex.ToString());
        //    }

        //    formobj.dataGridView1.DataSource = dt;
        //    formobj.button1.Click += new EventHandler(button1_Click);
        //    formobj.Show();
        //}

        //private void TestParsing3P(int PacketStartBytePosition, bool isBCCMatch, bool isPacketLengthMatch, int PacketLength, string PacketConfiguration, Byte[] Buffer, List<DataElementConfiguration> elementConfigs)
        //{

        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {
        //        formobj = new TestForm();



        //        //***********Columns For DataTable*************//
        //        dt.Columns.Add("SNo", typeof(int));
        //        foreach (DataElementConfiguration item in elementConfigs)
        //        {
        //            dt.Columns.Add(item.DataDefID + "_" + item.Unit, typeof(string));
        //        }
        //        dt.Columns.Add("FormatDate", typeof(string));


        //        formobj.label2.Text = isBCCMatch.ToString();
        //        formobj.label4.Text = isPacketLengthMatch.ToString();
        //        formobj.label6.Text = PacketLength.ToString();
        //        formobj.label8.Text = PacketConfiguration.ToString();


        //        int BytePosition = PacketStartBytePosition;
        //        int indexRow = 0;
        //        while (BytePosition < (Buffer.Length - 1))
        //        {
        //            int ClmIterator = 0;
        //            System.Data.DataRow dr = dt.NewRow();
        //            dr[ClmIterator++] = ++indexRow;
        //            for (int iteratorIndex = 0; iteratorIndex < elementConfigs.Count; iteratorIndex++)
        //            {
        //                string Val = "Absent";
        //                if (PacketConfiguration[iteratorIndex] == '1')
        //                {
        //                    int byteLength = elementConfigs[iteratorIndex].LengthOfDataType;

        //                    if (elementConfigs[iteratorIndex].DataDefID == 30)
        //                    {
        //                        Val = GetHexString(Buffer, BytePosition, byteLength);
        //                        dr["FormatDate"] = Val;
        //                    }
        //                    else
        //                    {
        //                        Byte[] TempBt = OriginalByte(Buffer, BytePosition, byteLength);
        //                        Val = GetParseValue(TempBt, byteLength);
        //                    }
        //                    BytePosition = BytePosition + byteLength;
        //                }
        //                dr[ClmIterator++] = Val;
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //System.Windows.Forms.MessageBox.Show(ex.ToString());
        //    }

        //    formobj.dataGridView1.DataSource = dt;
        //    formobj.button1.Click += new EventHandler(button1_Click);
        //    formobj.Show();
        //}

        //void button1_Click(object sender, EventArgs e)
        //{
        //    formobj.Close();
        //}

        #endregion


        #region SinglePhaseFastDownloadParser

        private List<MeterDataPacket> ParseSP(int PacketStartBytePosition, string PacketConfiguration, byte[] DataBuffer, List<DataElementConfiguration> elementConfigs)
        {
            List<MeterDataPacket> lstMeterDataPacket = new List<MeterDataPacket>();
            try
            {
                int BytePosition = PacketStartBytePosition;                
                while (BytePosition < (DataBuffer.Length - 1))
                {                     
                    MeterDataPacket meterDataPacket = new MeterDataPacket();
                    meterDataPacket.ReadingDate = DateTime.Now;
                    meterDataPacket.ListDataElementValue = new List<DataElement>();
                    for (int iteratorIndex = 0; iteratorIndex < elementConfigs.Count; iteratorIndex++)
                    {        
                        if (PacketConfiguration[iteratorIndex] == '1')
                        {
                            DataElement dataElement = new DataElement();
                            dataElement.DataDefinitionID = elementConfigs[iteratorIndex].DataDefID;
                            dataElement.Unit = elementConfigs[iteratorIndex].Unit;                   
                            int byteLength = elementConfigs[iteratorIndex].LengthOfDataType;
                            Byte[] TempBt = OriginalByte(DataBuffer, BytePosition, byteLength);
                            string Val = GetParseValue(TempBt, byteLength);
                            BytePosition = BytePosition + byteLength;

                            #region DateTimeSpecificCheck
                            //****************DateTime specific Check******************//
                            if (elementConfigs[iteratorIndex].DataDefID == 9 || elementConfigs[iteratorIndex].DataDefID == 41 ||  elementConfigs[iteratorIndex].DataDefID == 30)
                            {
                                UInt64 DatVal;
                                if (UInt64.TryParse(Val, out DatVal))
                                {
                                    Val = GetDateFromBase(DatVal).ToString("dd-MM-yyyy HH:mm:ss");
                                }
                                else
                                {
                                    Val = GetDateFromBase(0).ToString("dd-MM-yyyy HH:mm:ss");
                                }                              
                            }
                            //****************DateTime specific Check******************//
                            #endregion

                            dataElement.Value = Val;

                            #region UnitConversionPrecision
                            //******************Unit Conversion Precision
                            if (elementConfigs[iteratorIndex].Scalar == 0 && string.IsNullOrEmpty(elementConfigs[iteratorIndex].Unit)) //scalar can be zero for RTC or counters
                            {
                            }
                            else
                            {
                                DataElement newElement = GetUnitConvertedWithPrecision(dataElement, elementConfigs[iteratorIndex].Scalar, elementConfigs[iteratorIndex].Precision);
                                dataElement.Value = newElement.Value;
                                dataElement.Unit = newElement.Unit;
                            }
                            //******************Unit Conversion Precision
                            #endregion

                            meterDataPacket.ListDataElementValue.Add(dataElement);
                        } 
                    }                  
                     lstMeterDataPacket.Add(meterDataPacket);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return lstMeterDataPacket;
        }

        private List<MeterDataPacket> ParseProfileSP(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            //******Packet Format*******//
            //**** <4 Byte Packet Length> + <4 Byte Configuration> + <Data> + <1 Byte CRC> ******//            
            List<MeterDataPacket> ListMeterDataPacket = null;
            try
            {
                const int StartIndex = 0;
                const int PacketByteLength = 4;
                const int ConfigurationByteLength = 4;
                const int PacketStartBytePosition = StartIndex + PacketByteLength + ConfigurationByteLength;
                List<DataElementConfiguration> elementConfigs = dataPacket.DataElements;
                Byte[] DataBuffer = FromHex(profile.DataBuffer);
                bool isBCCMatch = IsBCCMatch(DataBuffer);
                bool isPacketLengthMatch = CheckLength(DataBuffer, StartIndex, PacketByteLength);
                string PacketConfigurationBit = GetPacketConfigurationBit(DataBuffer, (StartIndex + PacketByteLength), ConfigurationByteLength);
                string ReversePacketConfigurationBit = ReverseString(PacketConfigurationBit);
                //************To Test Parsing******************//
                //TestParsingSP(PacketStartBytePosition, isBCCMatch, isPacketLengthMatch, DataBuffer.Length, ReversePacketConfigurationBit, DataBuffer, elementConfigs);
                //Parse the Packet
                ListMeterDataPacket = ParseSP(PacketStartBytePosition,ReversePacketConfigurationBit, DataBuffer, elementConfigs);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return ListMeterDataPacket;
        }

        #endregion

        #region SFMeterFastDownloadParser

        //Meter Model SF Fast Download Parsing (DateTime coming in string instead of Seconds different from 3Phase and 1Phase) so seperate Parser for the same.

        private List<MeterDataPacket> ParseProfileSF(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            //******Packet Format*******//
            //**** <4 Byte Packet Length> + <4 Byte Configuration> + <Data> + <1 Byte CRC> ******//            
            List<MeterDataPacket> ListMeterDataPacket = null;
            try
            {
                const int StartIndex = 0;
                const int PacketByteLength = 4;
                const int ConfigurationByteLength = 4;
                const int PacketStartBytePosition = StartIndex + PacketByteLength + ConfigurationByteLength;
                List<DataElementConfiguration> elementConfigs = dataPacket.DataElements;
                Byte[] DataBuffer = FromHex(profile.DataBuffer);
                bool isBCCMatch = IsBCCMatch(DataBuffer);
                bool isPacketLengthMatch = CheckLength(DataBuffer, StartIndex, PacketByteLength);
                string PacketConfigurationBit = GetPacketConfigurationBit(DataBuffer, (StartIndex + PacketByteLength), ConfigurationByteLength);
                string ReversePacketConfigurationBit = ReverseString(PacketConfigurationBit);
                //************To Test Parsing******************//
                //TestParsingSP(PacketStartBytePosition, isBCCMatch, isPacketLengthMatch, DataBuffer.Length, ReversePacketConfigurationBit, DataBuffer, elementConfigs);
                //Parse the Packet
                ListMeterDataPacket = ParseSF(PacketStartBytePosition, ReversePacketConfigurationBit, DataBuffer, elementConfigs);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return ListMeterDataPacket;
        }


        private List<MeterDataPacket> ParseSF(int PacketStartBytePosition, string PacketConfiguration, byte[] DataBuffer, List<DataElementConfiguration> elementConfigs)
        {
            List<MeterDataPacket> lstMeterDataPacket = new List<MeterDataPacket>();
            try
            {
                int BytePosition = PacketStartBytePosition;
                while (BytePosition < (DataBuffer.Length - 1))
                {
                    MeterDataPacket meterDataPacket = new MeterDataPacket();
                    meterDataPacket.ReadingDate = DateTime.Now;
                    meterDataPacket.ListDataElementValue = new List<DataElement>();
                    for (int iteratorIndex = 0; iteratorIndex < elementConfigs.Count; iteratorIndex++)
                    {
                        if (PacketConfiguration[iteratorIndex] == '1')
                        {
                            DataElement dataElement = new DataElement();
                            dataElement.DataDefinitionID = elementConfigs[iteratorIndex].DataDefID;
                            dataElement.Unit = elementConfigs[iteratorIndex].Unit;
                            int byteLength = elementConfigs[iteratorIndex].LengthOfDataType;
                            Byte[] TempBt = OriginalByte(DataBuffer, BytePosition, byteLength);
                            string Val = GetParseValue(TempBt, byteLength);
                            BytePosition = BytePosition + byteLength;

                            #region DateTimeSpecificCheck
                            //****************DateTime specific Check******************//
                            if (elementConfigs[iteratorIndex].DataDefID == 9 || elementConfigs[iteratorIndex].DataDefID == 41 || elementConfigs[iteratorIndex].DataDefID == 30)
                            {
                                DateTime dt = GetProfileTimestamp(ByteToHexString(TempBt, 0, TempBt.Length));                               
                                Val = dt.ToString("dd-MM-yyyy HH:mm:ss");                                
                            }
                            //****************DateTime specific Check******************//
                            #endregion

                            dataElement.Value = Val;

                            #region UnitConversionPrecision
                            //******************Unit Conversion Precision
                            if (elementConfigs[iteratorIndex].Scalar == 0 && string.IsNullOrEmpty(elementConfigs[iteratorIndex].Unit)) //scalar can be zero for RTC or counters
                            {
                            }
                            else
                            {
                                DataElement newElement = GetUnitConvertedWithPrecision(dataElement, elementConfigs[iteratorIndex].Scalar, elementConfigs[iteratorIndex].Precision);
                                dataElement.Value = newElement.Value;
                                dataElement.Unit = newElement.Unit;
                            }
                            //******************Unit Conversion Precision
                            #endregion

                            meterDataPacket.ListDataElementValue.Add(dataElement);
                        }
                    }
                    lstMeterDataPacket.Add(meterDataPacket);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return lstMeterDataPacket;
        }


        #endregion

        #region ThreePhaseFastDownloadParser

        private List<MeterDataPacket> ParseProfile3P(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            //******Packet Format*******//
            //**** <2 Byte Configuration> + <Data> + <1 Byte CRC> ******//            
            List<MeterDataPacket> ListMeterDataPacket = null;
            try
            {
                const int StartIndex = 0;
                const int ConfigurationByteLength = 2;
                const int PacketStartBytePosition = StartIndex + ConfigurationByteLength;
                List<DataElementConfiguration> elementConfigs = dataPacket.DataElements;
                Byte[] DataBuffer = FromHex(profile.DataBuffer);
                bool isBCCMatch = IsBCCMatch(DataBuffer);
                bool isPacketLengthMatch = false;
                string PacketConfigurationBit = GetPacketConfigurationBit(DataBuffer, StartIndex, ConfigurationByteLength);
                string ReversePacketConfigurationBit = ReverseString(PacketConfigurationBit);
                string ModifiedReversePacketConfigurationBit = "1" + ReversePacketConfigurationBit;//1 Bit Compulsory Bit for DateTime
                //************To Test Parsing******************//
                //TestParsing3P(PacketStartBytePosition, isBCCMatch, isPacketLengthMatch, DataBuffer.Length, ModifiedReversePacketConfigurationBit, DataBuffer, elementConfigs);
                //Parse the Packet
                ListMeterDataPacket = Parse3P(PacketStartBytePosition, ModifiedReversePacketConfigurationBit, DataBuffer, elementConfigs);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return ListMeterDataPacket;
        }

        private List<MeterDataPacket> Parse3P(int PacketStartBytePosition, string PacketConfiguration, byte[] DataBuffer, List<DataElementConfiguration> elementConfigs)
        {
            List<MeterDataPacket> lstMeterDataPacket = new List<MeterDataPacket>();
            try
            {
                int BytePosition = PacketStartBytePosition;
                while (BytePosition < (DataBuffer.Length - 1))
                {
                    MeterDataPacket meterDataPacket = new MeterDataPacket();
                    meterDataPacket.ReadingDate = DateTime.Now;
                    meterDataPacket.ListDataElementValue = new List<DataElement>();
                    for (int iteratorIndex = 0; iteratorIndex < elementConfigs.Count; iteratorIndex++)
                    {
                        if (PacketConfiguration[iteratorIndex] == '1')
                        {
                            DataElement dataElement = new DataElement();
                            dataElement.DataDefinitionID = elementConfigs[iteratorIndex].DataDefID;
                            dataElement.Unit = elementConfigs[iteratorIndex].Unit;
                            int byteLength = elementConfigs[iteratorIndex].LengthOfDataType;
                            Byte[] TempBt = OriginalByte(DataBuffer, BytePosition, byteLength);
                            string Val = GetParseValue(TempBt, byteLength);
                            BytePosition = BytePosition + byteLength;

                            #region DateTimeSpecificCheck
                            //****************DateTime specific Check******************//
                            if (elementConfigs[iteratorIndex].DataDefID == 9 || elementConfigs[iteratorIndex].DataDefID == 41 || elementConfigs[iteratorIndex].DataDefID == 30)
                            {
                                DateTime dt = new DateTime(2000,01,01,0,0,0);
                                if(DateTime.TryParse(GetHexString(TempBt, 0, TempBt.Length),out dt))
                                {
                                    Val = dt.ToString("dd-MM-yyyy HH:mm:ss");
                                }
                                else
                                {
                                    Val = dt.ToString("dd-MM-yyyy HH:mm:ss");
                                }                               
                            }
                            //****************DateTime specific Check******************//
                            #endregion

                            dataElement.Value = Val;

                            #region UnitConversionPrecision
                            //******************Unit Conversion Precision
                            if (elementConfigs[iteratorIndex].Scalar == 0 && string.IsNullOrEmpty(elementConfigs[iteratorIndex].Unit)) //scalar can be zero for RTC or counters
                            {
                            }
                            else
                            {
                                DataElement newElement = GetUnitConvertedWithPrecision(dataElement, elementConfigs[iteratorIndex].Scalar, elementConfigs[iteratorIndex].Precision);
                                dataElement.Value = newElement.Value;
                                dataElement.Unit = newElement.Unit;
                            }
                            //******************Unit Conversion Precision
                            #endregion

                            meterDataPacket.ListDataElementValue.Add(dataElement);
                        }
                    }
                    lstMeterDataPacket.Add(meterDataPacket);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return lstMeterDataPacket;
        }


        #endregion
      

     


        /// <summary>
        /// Parses the data in allprofile data, fills the profiledata with parsed data and returns the current pointer in allprofiledata array
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="profileData"></param>
        /// <returns></returns>
        public override ProfileData ParseProfile(string[] allProfileData, DLMSCOMMAND dlmsCommand, ref int commandIndex)
        {
            IsFDData = false;
            ProfileData individualProfile = new ProfileData();
            CAB.Parser.Entity.Profile profile = new CAB.Parser.Entity.Profile();
            int obisIndex = 0;
            for (int index = 0; index < allProfileData.Length; index++)
            {
                if (allProfileData[index].Substring(0, 16) == "0100006001BCFF02")
                {
                    obisIndex = index;
                    break;
                }
            }
            int meterModel = GetMeterModelFromSignatureData(allProfileData[obisIndex].Substring(16, allProfileData[obisIndex].Length - 16));
            /**
            Added INI config for meter models with multiple sets of configurations of loadsurveyelements.
            **/
            DataPacketConfiguration dataPacketConfig =null;
            switch (meterModel)
            {
                case 11:
                    if (ConfigSettings.GetValue("FDUseAlternateModelWBNET") == "1")
                        dataPacketConfig = meterConfigParser.GetConfiguration(Convert.ToInt32(dlmsCommand.TAGNO), (int)FDModelEnum.WBSEDCL_NET_FD_EXT_PARSING);
                    else
                        goto default;
                    break;
                default:
                    dataPacketConfig = meterConfigParser.GetConfiguration(Convert.ToInt32(dlmsCommand.TAGNO), meterModel);
                    break;
            }
            //DataPacketConfiguration dataPacketConfig = meterConfigParser.GetConfiguration(Convert.ToInt32(dlmsCommand.TAGNO), meterModel);
            //Update Resolution for energy and demand in config packet.
            //Also responsible for updating meterID length in dataelement to support dynamic meterID for FD redaout.

            //******* Meter Model Change Required Here ***********//
            if (meterModel == NamePlateConstants.VFSPNoSeasonNoWeek)
            {
                profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
                individualProfile.ListMeterDataPacket = ParseProfileSP(profile, dataPacketConfig);
            }
            else if (meterModel == NamePlateConstants.SFSP)
            {
                profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
                individualProfile.ListMeterDataPacket = ParseProfileSF(profile, dataPacketConfig);
            }
            else if (meterModel == NamePlateConstants.BYPL_FD)// For 1P VIM 64K DLMS with FD
            {
                profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
                individualProfile.ListMeterDataPacket = ParseProfileSinglePhaseCF(profile, dataPacketConfig);
            }
            else if (meterModel == NamePlateConstants.BRPL_7Slot)
            {
                profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
                individualProfile.ListMeterDataPacket = ParseProfileSF(profile, dataPacketConfig);
            }


            //else if (meterModel == NamePlateConstants.SapphireValue)
            //{
            //    profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
            //    individualProfile.ListMeterDataPacket = ParseProfile3P(profile, dataPacketConfig);
            //}   
            //else if (meterModel == NamePlateConstants.TwoTOUSapphireValue)
            //{
            //profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;
            //individualProfile.ListMeterDataPacket = ParseProfile3P(profile, dataPacketConfig);
            //}   
            else
            {

                UpdateDataElementConfiguration(dataPacketConfig);
                profile.DataBuffer = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16); commandIndex++;

                // For net meter
                if (profile.DataBuffer.Substring(0, 12) == hexFDDATA)
                {
                    IsFDData = true;
                    dataPacketConfig = meterConfigParser.GetConfiguration(Convert.ToInt32(dlmsCommand.TAGNO), meterModel, "DataElementsNetMeters.xml");
                    //Remove hexFDDATA from buffer
                    profile.DataBuffer = profile.DataBuffer.Substring(14,(profile.DataBuffer.Length - 14));
                    //Get Configuration Bit length
                    FDDataConfigurationBitLength = Convert.ToInt16(FromHex(profile.DataBuffer.Substring(0, 2))[0]);
                    //Remove Configuration Length from buffer
                    profile.DataBuffer = profile.DataBuffer.Substring(2, (profile.DataBuffer.Length - 2));
                    //Get LP Interval                    
                    FDDataLPInterval = Convert.ToInt16(FromHex(profile.DataBuffer.Substring((profile.DataBuffer.Length - 13), 2))[0]);
                    //Get Max no of Days
                    FDDataMaxDaysConfigured = BitConverter.ToInt16(ReverseByte(FromHex(profile.DataBuffer.Substring((profile.DataBuffer.Length - 11), 4)),0,2),0);
                    //Get Current Entries
                    FDCurrentEntries = BitConverter.ToInt16(ReverseByte(FromHex(profile.DataBuffer.Substring((profile.DataBuffer.Length - 7), 4)),0,2), 0);
                    //Get CheckSum 
                    FDCheckSum = Convert.ToInt16(FromHex(profile.DataBuffer.Substring((profile.DataBuffer.Length - 3), 2))[0]);
                    //Get Resultant buffer
                    profile.DataBuffer = profile.DataBuffer.Substring(0, (profile.DataBuffer.Length - 13));
                }
               
                individualProfile.ListMeterDataPacket = ParseProfile(profile, dataPacketConfig);

                //If Net meter variant 3
                if (NetMeterVariantInfo == "3")
                {
                    SwapDataDefIdForMetervariantThree(individualProfile.ListMeterDataPacket);
                }
            }

            
            //Get Resolution for energy and demand form NamePlate data.
            if (Convert.ToInt32(dlmsCommand.TAGNO) == (int)ProfileId.NamePlate)
            {
                GetEnergyAndDemandResolution(individualProfile.ListMeterDataPacket[0]);
            }
            return individualProfile;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Used to get Energy and demand resolution values from NamePlate profile.
        /// </summary>
        /// <param name="meterDataPacket"></param>
        private void GetEnergyAndDemandResolution(MeterDataPacket meterDataPacket)
        {
            foreach (DataElement dataElement in meterDataPacket.ListDataElementValue)
            {
                if (dataElement.DataDefinitionID == 1001)
                {
                    ConfigInfo.EnergyResolution = Convert.ToInt32(dataElement.Value);
                }
                else if (dataElement.DataDefinitionID == 1002)
                {
                    ConfigInfo.DemandResolution = Convert.ToInt32(dataElement.Value);
                }
            }
        }

        /// <summary>
        /// Used to update dataelement configuration eneregy and demand precesion values 
        /// From values of energy and demand resolution got from Nameplate profile .
        /// It also updates meter serial number lenth with meterid length fetched from DLMS meter ID readout,to support dynamic meterid for FD.        /// 
        /// </summary>
        /// <param name="dataElementConfigurationList"></param>
        /// <returns></returns>
        private List<DataElementConfiguration> UpdateDataElementConfiguration(DataPacketConfiguration dataPacketConfiguration)
        {

            #region UpadateDemandEnergyPrecision
            foreach (DataElementConfiguration dataConfigElement in dataPacketConfiguration.DataElements)
            {
                //Update Meter Serial Number legth
                if (dataConfigElement.DataDefID == 1)
                {
                    dataConfigElement.LengthInBits = ConfigInfo.MeterIdLength * 8;
                    //Update Data Packet Length , due to change in serial number length
                    dataPacketConfiguration.PacketLength = dataPacketConfiguration.PacketLength - 8 + ConfigInfo.MeterIdLength;
                }
                foreach (int dataDefId in DataDefIdForDemandParameters)
                {
                    if (dataDefId == dataConfigElement.DataDefID)
                    {
                        dataConfigElement.Precision = ConfigInfo.DemandResolution;
                        break;
                    }
                }

                foreach (int dataDefId in DataDefIdForEneregyParameters)
                {
                    // This condition is not applicable for Sapphire HTCT meter
                    if (meterModel != 27 && meterModel != 28)
                    {
                        if (dataDefId == dataConfigElement.DataDefID)
                        {
                            dataConfigElement.Precision = ConfigInfo.EnergyResolution;
                            break;
                        }
                    }
                }

            }
            #endregion

            return dataPacketConfiguration.DataElements;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signatureData"></param>
        /// <returns></returns>
        /// 



        private int GetMeterModelFromSignatureData(string strSignatureInfo)
        {
            string InternalFirmwareVersion = string.Empty;
            string Metertype = string.Empty;
            string VoltageRating = string.Empty;
            string BasicCurrentRating = string.Empty;
            //string NetMeterVariantInfo = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(strSignatureInfo))
                {
                    byte[] signatureInfo = SoapHexBinary.Parse(strSignatureInfo).Value;
                    string signatureData = Encoding.ASCII.GetString(signatureInfo);
                    int InitialStartupIndex = 0;


                    if(signatureData.Contains("*"))
                    {
                        signatureData = signatureData.Substring(signatureData.IndexOf('*'));
                    }
                    else if (signatureData.Contains("#"))
                    {
                        signatureData = signatureData.Substring(signatureData.IndexOf('#'));
                    }
                    else
                    {

                    }


                    if (signatureData == "**2.21240010060WC4RS")
                    {
                        signatureData = "**2.21240010060SK4RS";
                        InternalFirmwareVersion = "----";
                    }
                    else if (signatureData.Contains("#"))
                    {
                        InternalFirmwareVersion = signatureData.Substring(signatureData.IndexOf('#') + 1, 6).TrimStart('0');
                        InitialStartupIndex = 8;
                    }
                    else
                    {
                        InternalFirmwareVersion = string.Format("{0:0.00}", Convert.ToDecimal((signatureData.Substring(0, 6).Trim('*'))));
                        InitialStartupIndex = 6;
                    }

                    //User Story 478245. Voltage Rating change to 63.5 V for HK meter model
                    int LenghtVoltageRating = 3;
                    if (signatureData.Length >= 30 && signatureData.Length < 60)
                    {
                        LenghtVoltageRating = 4;
                    }
                    //NET Metering Meter SignatureInfo Length >= 60
                    if (signatureData.Length >= 60)
                    {
                        LenghtVoltageRating = 5;
                    }

                    VoltageRating = signatureData.Substring(InitialStartupIndex, LenghtVoltageRating) + " V";


                    InitialStartupIndex += LenghtVoltageRating;


                    InitialStartupIndex += 6;

                    string meterType = signatureData.Substring(InitialStartupIndex, 2);

                    //NET Metering Meter SignatureInfo Length >= 60
                    NetMeterVariantInfo = CAB.Framework.MeterVariant.ONE;
                    if (signatureData.Length >= 60)
                    {
                        InitialStartupIndex += 5;
                        try
                        {
                            byte MeterVariant = Convert.ToByte(signatureData[InitialStartupIndex]);
                            NetMeterVariantInfo = MeterVariant.ToString("X");                        
                        }
                        catch
                        {


                        }
                    }

                    if (meterType == "UK")
                    {
                        meterModel = NamePlateConstants.Ruby6Value;
                    }
                    else if ((meterType == "LT"))
                    {
                        meterModel = NamePlateConstants.PumaLTE650Value;
                    }
                    else if ((meterType == "ST"))
                    {
                        meterModel = NamePlateConstants.SapphireLTCT;
                    }

                    else if ((meterType == "L0"))
                    {
                        meterModel = NamePlateConstants.Sapphire_Netmeter_LTCT;
                    }
                    else if ((meterType == "st"))
                    {
                        meterModel = NamePlateConstants.SapphireLTCT_st;
                    }
                    else if ((meterType == "St"))
                    {
                        meterModel = NamePlateConstants.SapphireWCM_St;
                    }
                    else if (meterType == "WB")
                    {
                        meterModel = NamePlateConstants.WBValue;
                    }
                    else if (meterType == "SC")
                    {
                        meterModel = NamePlateConstants.SapphireValue;
                    }
                    else if (meterType == "Sc")//FOR 3PH THREE TOU SEASSION
                    {
                        meterModel = NamePlateConstants.ThreeTOUWCMValue;
                    }

                    else if (meterType == "W0")//SapphireS2WCM
                    {
                        meterModel = NamePlateConstants.Sapphire_Netmeter_WCM;
                    }
                    else if (meterType == "TN")
                    {
                        meterModel = NamePlateConstants.TNValue;
                    }
                    //******* If Fast Download support for this meter model *******//
                    //******* Meter Model Change Required Here ***********//
                    else if (meterType == "VF")
                    {
                        meterModel = NamePlateConstants.VFSPNoSeasonNoWeek;
                    }
                    else if (meterType == "sc")
                    {
                        meterModel = NamePlateConstants.TwoTOUSapphireValue;
                    }
                    else if (meterType == "SF")
                    {
                        meterModel = NamePlateConstants.SFSP;
                    }
                    else if (meterType == "SH")
                    {
                        meterModel = NamePlateConstants.Sapphire_SH;
                    }
                    else if (meterType == "SM")
                    {
                        meterModel = NamePlateConstants.Sapphire_SM;
                    }
                    else if (meterType == "sm")
                    {
                        meterModel = NamePlateConstants.Sapphire_sm;
                    }
                    else if (meterType == "sh")
                    {
                        meterModel = NamePlateConstants.Sapphire_sh;
                    }
                    else if (meterType == "vb")
                    {
                        meterModel = NamePlateConstants.VIM_Series2;
                    }
                    else if (meterType == "CF")
                    {
                        meterModel = NamePlateConstants.BYPL_FD;// For 1P VIM 64K DLMS with FD
                    }
                    else if (meterType == "BF")//FOR BYPL 7 SLOT
                    {
                        meterModel = NamePlateConstants.BYPL_7Slot;
                    }
                    else if (meterType == "RF")//FOR BRPL 7 SLOT
                    {
                        meterModel = NamePlateConstants.BRPL_7Slot;
                    }


                    else
                    {

                    }

                   
                 
                    //  generalEntity.Metertype = signatureData.Substring(17, 1) == "3" ? MeterType.ThreePhaseThreeWire : MeterType.ThreePhaseFourWire;
                }
            }
            catch (Exception)
            {


            }
            return meterModel;
        }



        //private int GetMeterModelFromSignatureData(string signatureData)
        //{
        //    int meterModel = 2;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(signatureData))
        //        {
        //            string meterType = string.Empty;
        //            byte[] signatureInfo = SoapHexBinary.Parse(signatureData).Value;
        //            string strSignatureInfo = Encoding.ASCII.GetString(signatureInfo);
        //            if (signatureInfo != null && signatureInfo.Length > 18)
        //            {
        //                meterType = Convert.ToChar(signatureInfo[17]).ToString() + Convert.ToChar(signatureInfo[18]).ToString();
        //                if (meterType == "UK")
        //                {
        //                    meterModel = NamePlateConstants.Ruby6Value;
        //                }
        //                else if ((meterType == "LT"))
        //                {
        //                    meterModel = NamePlateConstants.PumaLTE650Value;
        //                }
        //                else if ((meterType == "ST"))
        //                {
        //                    meterModel = NamePlateConstants.SapphireLTCT;
        //                }
        //                else if (meterType == "WB")
        //                {
        //                    meterModel = NamePlateConstants.WBValue;
        //                }
        //                else if (meterType == "SC")
        //                {
        //                    meterModel = NamePlateConstants.SapphireValue;
        //                }
        //                else if (meterType == "TN")
        //                {
        //                    meterModel = NamePlateConstants.TNValue;
        //                }
        //                //******* If Fast Download support for this meter model *******//
        //                //******* Meter Model Change Required Here ***********//
        //                else if (meterType == "VF")
        //                {
        //                    meterModel = NamePlateConstants.VFSPNoSeasonNoWeek;
        //                }
        //                else if (meterType == "sc")
        //                {
        //                    meterModel = NamePlateConstants.TwoTOUSapphireValue;
        //                }
        //                else if (meterType == "SF")
        //                {
        //                    meterModel = NamePlateConstants.SFSP;
        //                }                        
        //                else if (meterType == "SF")
        //                {
        //                    meterModel = NamePlateConstants.SFSP;
        //                }
        //                else if (meterType == "SH")
        //                {
        //                    meterModel = NamePlateConstants.Sapphire_SH;
        //                }
        //                else if (meterType == "SM")
        //                {
        //                    meterModel = NamePlateConstants.Sapphire_SM;
        //                }
        //                else if (meterType == "sm")
        //                {
        //                    meterModel = NamePlateConstants.Sapphire_sm;
        //                }
        //                else if (meterType == "sh")
        //                {
        //                    meterModel = NamePlateConstants.Sapphire_sh;
        //                }
        //                else if (meterType == "vb")
        //                {
        //                    meterModel = NamePlateConstants.VIM_Series2;
        //                }
        //            }
        //        }

        //    }
        //    catch
        //    {
        //    }
        //    return meterModel;
        //}





        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="scale"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        protected DataElement GetUnitConvertedWithPrecision(DataElement element, int scale, int precision)
        {
            string unit = element.Unit;
            string value = string.Empty;
            string additionalUnit = "k";
            if (unit.ToUpper() == "W" || unit.ToUpper() == "VA" || unit.ToUpper() == "VAR" || unit.ToUpper() == "WH" || unit.ToUpper() == "VAH" || unit.ToUpper() == "VARH")
            {
                if (unit.ToUpper() == "VAR")
                {
                    unit = "VAr";
                }
                else if (unit.ToUpper() == "W")
                {
                    unit = unit.ToUpper();
                }
                else if (unit.ToUpper() == "VA")
                {
                    unit = unit.ToUpper();
                }
                if (scale > 3)
                {
                    additionalUnit = "M";
                    scale = scale - 6;
                }
                else
                {
                    scale = scale - 3;
                }

                element.Value = TruncateToPrecision(element.Value, scale, (uint)precision);


                element.Unit = additionalUnit + unit;
            }
            else
            {
                element.Value = TruncateToPrecision(element.Value, scale, (uint)precision);
            }

            return element;
        }
		
		protected DataElement GetUnitConvertedWithPrecisionCF(DataElement element, int scale, int precision)
        {
            string unit = element.Unit;
            string value = string.Empty;
            string additionalUnit = "k";
            if (unit.ToUpper() == "W" || unit.ToUpper() == "VA" || unit.ToUpper() == "VAR" || unit.ToUpper() == "WH" || unit.ToUpper() == "VAH" || unit.ToUpper() == "VARH")
            {
                if (unit.ToUpper() == "VAR")
                {
                    unit = "VAr";
                }
                else if (unit.ToUpper() == "W")
                {
                    unit = unit.ToUpper();
                }
                else if (unit.ToUpper() == "VA")
                {
                    unit = unit.ToUpper();
                }               
               

                if (scale > 3)
                {
                    additionalUnit = "M";
                    scale = scale - 6;
                }
                else
                {
                    scale = scale - 3;
                }

                element.Value = TruncateToPrecisionCF(element.Value, scale, (uint)precision);


                element.Unit = additionalUnit + unit;
            }
            else
            {
                if (unit.Contains("k"))
                {
                    scale = (Byte)(scale - 3); 
                }
                element.Value = TruncateToPrecisionCF(element.Value, scale, (uint)precision);
            }

            return element;
        }

        /// <summary>
        /// Gets the list of meter data elements according to DLMS standard from extended binary reader 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<MeterDataPacket> GetMeterDataPackets(DataPacketConfiguration dataPacketConfig, ExtendedBinaryReader reader)
        {
            bool isBilling = false;
            int currentPointer = 0;
            long counter = 0;
            long arrayLength = 0;
            string data = string.Empty;
            MeterDataPacket dataRow = null;
            List<MeterDataPacket> meterDataPackets = new List<MeterDataPacket>();
            List<MeterDataPacket> sortedMeterDataPackets = null;
            List<DataElementConfiguration> elementConfigs = dataPacketConfig.DataElements;
            try
            {
                

                // Ghatiya code hai.But majboori hai.FW design ke wajah se.Sorry for myself.
                if (dataPacketConfig.PacketLength == 556 || dataPacketConfig.PacketLength == 457
                    || dataPacketConfig.PacketLength == 453 || dataPacketConfig.PacketLength == 512)
                {
                    isBilling = true;

                }
                // Ghatiya code hai.But majboori hai.FW design ke wajah se.Sorry for myself.
                #region LP Dyanamic parameter Handling

                if (IsFDData)
                {
                    #region Fd Data Parsing For Net Meters
                    
                    //List All the Configuration Byte
                    List<byte> lstByte = new List<byte>();
                    for (int i = 0; i < FDDataConfigurationBitLength; i++)
                    {
                        lstByte.Add(reader.ReadByte());
                    }
                    //Reverse the List for Endian Support
                    //lstByte.Reverse();

                    //Get Bit Configuration String Wise
                    string ConfigurationBit = ByteTOBinary(lstByte.ToArray());
                    string ReverseConfigurationBit = ReverseString(ConfigurationBit);

                    ReverseConfigurationBit = "1" + ReverseConfigurationBit; //DateTime is present by default at 0 Position
                   
                    //Get List Element Present in Configuration 
                    List<DataElementConfiguration> lstDataElementPresent = new List<DataElementConfiguration>();
                    int iterator = 0; int lpPacketLength = 0; 
                    foreach (DataElementConfiguration elementConfig in elementConfigs)
                    {
                        if (ReverseConfigurationBit[iterator++] == '1')
                        {
                            lstDataElementPresent.Add(elementConfig);
                            lpPacketLength += elementConfig.LengthOfDataType;
                        }
                    }


                    //Now Refresh Configuration List
                    elementConfigs.Clear();
                    elementConfigs.AddRange(lstDataElementPresent);
                    
                   
                    dataPacketConfig.PacketLength = lpPacketLength;  

                    #endregion
                }
                else
                {
                    //Previous Existing Parsing
                    if (dataPacketConfig.PacketLength == 34 || dataPacketConfig.PacketLength == 37)
                    {

                        byte value1 = reader.ReadByte();
                        byte value2 = reader.ReadByte();

                        for (byte bitIndex = 42; bitIndex < 50; bitIndex++)
                        {
                            if (!Convert.ToBoolean(value2 & 1))
                            {
                                elementConfigs.RemoveAll(x => x.DataDefID == bitIndex);
                            }
                            value2 = (byte)(value2 >> 1);
                        }
                        for (byte bitIndex = 50; bitIndex < 54; bitIndex++)
                        {
                            if (!Convert.ToBoolean(value1 & 1))
                                elementConfigs.RemoveAll(x => x.DataDefID == bitIndex);
                            value1 = (byte)(value1 >> 1);
                        }
                        int lpPacketLength = 0;
                        foreach (DataElementConfiguration elementConfig in elementConfigs)
                        {
                            lpPacketLength = lpPacketLength + elementConfig.LengthOfDataType;
                        }
                        dataPacketConfig.PacketLength = lpPacketLength;
                    }

                }

                #endregion

                // Length of Array
                arrayLength = (long)((reader.BaseStream.Length) / (dataPacketConfig.PacketLength));
                while (counter < arrayLength)
                {
                    dataRow = new MeterDataPacket();
                    //structure Length                   
                    dataRow.ListDataElementValue = new List<DataElement>();
                    EatUnusedBytes(counter, dataPacketConfig, reader);

					//For Generic Meters
                    if (!IsFDData)
                    {
                        if (meterModel == 27 || meterModel == 28) // For HTCT meter
                        {
                            foreach (DataElementConfiguration elementConfig in elementConfigs)
                            {
                                dataRow.ListDataElementValue.Add(GetDataElementNetMeters(elementConfig, reader));
                            }
                        }
                        else
                        {                        
                            foreach (DataElementConfiguration elementConfig in elementConfigs)
                            {
                                dataRow.ListDataElementValue.Add(GetDataElement(elementConfig, reader));
                            }
                        }                   
                    }
					// For Net Meters
                    else
                    {
                        foreach (DataElementConfiguration elementConfig in elementConfigs)
                        {
                            dataRow.ListDataElementValue.Add(GetDataElementNetMeters(elementConfig, reader));
                        }
                    }
                   
                    counter++;

                    meterDataPackets.Add(dataRow);

                }
                // Ghatiya code hai.But majboori hai.FW design ke wajah se.Sorry for myself.
                if (isBilling)
                {
                    reader.BaseStream.Position = 0;
                    byte[] billingData = reader.ReadBytes((int)reader.BaseStream.Length);
                    currentPointer = billingData[1];
                    sortedMeterDataPackets = new List<MeterDataPacket>();
               
                    for (int i = currentPointer; i < arrayLength; i++)
                    {
                        sortedMeterDataPackets.Add(meterDataPackets[i]);
                    }
                    for (int i = 0; i < currentPointer; i++)
                    {
                        sortedMeterDataPackets.Add(meterDataPackets[i]);
                    }
                    meterDataPackets = sortedMeterDataPackets;
                    meterDataPackets.Reverse();
                    isBilling = false;
                }
                // Ghatiya code hai.But majboori hai.FW design ke wajah se.Sorry for myself.
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return meterDataPackets;
        }
        /// <summary>
        /// Parses the raw bytes as per the data configuration supplied in the DataPacketConfiguration parameter  
        /// </summary>
        /// <param name="data"> Byte array coming from the NPL layer without AMR and Generic Header</param>
        /// <param name="dataPacket">Data packet containing sorted set of data element information</param>
        /// 
        /// <returns>Parsed objet to metrology parser</returns>
        private List<MeterDataPacket> Parse(byte[] data, DataPacketConfiguration dataPacketConfig)
        {
            List<MeterDataPacket> meterDataPackets = GetMeterDataPackets(dataPacketConfig, GetReader(data));
            return meterDataPackets;
        }
        /// <summary>
        /// Fills the element with element configuration and data contained in reader
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementConfig"></param>
        /// <param name="binaryReader"></param>
        private DataElement GetDataElement(DataElementConfiguration elementConfig, ExtendedBinaryReader reader)
        {
            DataElement element = new DataElement();
            try
            {
                DataType dataType = dataTypeFactory.GetDataType(infoManager.GetUnitInfo(elementConfig.DataTypeID));
                element.Value = dataType.GetValue(reader, elementConfig);
                element.Unit = elementConfig.Unit;
                element.DataDefinitionID = elementConfig.DataDefID;
                if (elementConfig.Scalar != 0) //scalar can be zero for RTC or counters
                {
                    DataElement newElement = null;
                    // Added to make Lp parameetrs always 3 decimal place
                    if (element.DataDefinitionID == 48 || element.DataDefinitionID == 49 || element.DataDefinitionID == 50 || element.DataDefinitionID == 51)
                    {
                        newElement = GetUnitConvertedWithPrecision(element, -5, 3);
                    }
                    // Added to make Lp parameetrs always 3 decimal place
                    else
                    {
                        newElement = GetUnitConvertedWithPrecision(element, elementConfig.Scalar, elementConfig.Precision);
                    }
                    element.Value = newElement.Value;
                    element.Unit = newElement.Unit;
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return element;
        }
        /// <summary>
        /// Eats unused bytes in a row of profile
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="dataPacketConfig"></param>
        /// <param name="reader"></param>
        private void EatUnusedBytes(long counter, DataPacketConfiguration dataPacketConfig, ExtendedBinaryReader reader)
        {
            if (reader.BaseStream.Position > 0)
            {
                long diff = (counter * dataPacketConfig.PacketLength) - reader.BaseStream.Position;
                while (diff > 0)
                {
                    reader.ReadByte();
                    diff--;
                }
            }
        }
        private DataElement GetDataElementNetMeters(DataElementConfiguration elementConfig, ExtendedBinaryReader reader)
        {
            DataElement element = new DataElement();
            try
            {
                DataType dataType = dataTypeFactory.GetDataType(infoManager.GetUnitInfo(elementConfig.DataTypeID));
                element.Value = dataType.GetValue(reader, elementConfig);
                element.Unit = elementConfig.Unit;
                element.DataDefinitionID = elementConfig.DataDefID;
                if (elementConfig.Scalar != 0) //scalar can be zero for RTC or counters
                {
                    DataElement newElement = null;
                    // Added to make Lp parameetrs always 3 decimal place
                    
                        newElement = GetUnitConvertedWithPrecision(element, elementConfig.Scalar, elementConfig.Precision);
                    
                    element.Value = newElement.Value;
                    element.Unit = newElement.Unit;
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return element;
        }

        ////If Net meter variant 3 then display generic obiscode and header 
        private void SwapDataDefIdForMetervariantThree(List<MeterDataPacket> loadsurveyRecords)
        {
            try
            {           
                foreach (MeterDataPacket record in loadsurveyRecords)
                {
                    foreach (DataElement item in record.ListDataElementValue)
                    {
                        if (item.DataDefinitionID == 2053)
                        {
                            item.DataDefinitionID = 48;
                        }
                        if (item.DataDefinitionID == 2054)
                        {
                            item.DataDefinitionID = 51;
                        }
                        if (item.DataDefinitionID == 2079)
                        {
                            item.DataDefinitionID = 49;
                        }
                        if (item.DataDefinitionID == 2080)
                        {
                            item.DataDefinitionID = 50;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
       }
        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="dataPacket"></param>
        /// <returns></returns>
        private List<MeterDataPacket> ParseProfileSinglePhaseCF(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            //******************* By Mohsin *********************
            const int SECONDS = 60;
            const int LENGTHINDEX = 4;
            const int RESERVEDBYTE = 2;
            const int RECORDINDEX = 1;
            const int RECORDTIMESTAMP = 4;
            const int RECORDCONFIG = 2;
            const int CHECKSUMLENGTH = 1;
            uint recievedDataCount = 0;
            byte[] reserverdbyte = new byte[RESERVEDBYTE];
            byte recordIndex;
            uint recordDatetime;
            byte recordsperday = 0;
            byte ipvalue = 0;
            List<string> MasterParamList = new List<string>();
            int recordsizeinbytes=0;
            List<MeterDataPacket> lstMeterDataPacket = new List<MeterDataPacket>();
            string filename = "TempDataRecord.txt";
            List<string> filetempdata = new List<string>();
            try
            {
                byte[] TempData = new byte[4];
                const int StartIndex = 0;
                uint uiDataCunter = 0;
                List<DataElementConfiguration> elementConfigs = dataPacket.DataElements;
                Byte[] DataBuffer = FromHex(profile.DataBuffer);
                bool isBCCMatch = IsBCCMatch(DataBuffer);
                bool isPacketLengthMatch = CheckLength(DataBuffer, StartIndex, LENGTHINDEX);
                bool[] boolArray = new bool[8];

                // Get Total data length including checksum
                Array.Copy(DataBuffer, uiDataCunter, TempData, 0, LENGTHINDEX);
                Array.Reverse(TempData);
                recievedDataCount = BitConverter.ToUInt32(TempData, 0);
                uiDataCunter += LENGTHINDEX;
                
                // Get reserverd bytes
                uiDataCunter += RESERVEDBYTE;

                //Get IP
                recordsperday = DataBuffer[uiDataCunter]; 
                if (recordsperday == 0x30) ipvalue = 30;
                else if (recordsperday == 96) ipvalue = 15;
                else
                    ipvalue = 60;

                //Log
                filetempdata.Add("NumberOfRecords- " + recordsperday);
                filetempdata.Add("IP- " + ipvalue.ToString());


                uiDataCunter += RECORDINDEX;

                //Get RecordStartPointer
                recordIndex = DataBuffer[uiDataCunter];
                uiDataCunter += RECORDINDEX;

                // Log
                filetempdata.Add("RecordIndex- " + recordIndex.ToString());

                // Get First Record Date
                Array.Copy(DataBuffer, uiDataCunter, TempData, 0, RECORDTIMESTAMP);
                recordDatetime = BitConverter.ToUInt32(TempData, 0);
                uiDataCunter += RECORDTIMESTAMP;

                // Get Config Data 2 Bytes
                TempData = new byte[2];
                Array.Copy(DataBuffer, uiDataCunter, TempData, 0, RECORDCONFIG);
                BitArray lsConfigBitData = new BitArray(new byte[] { TempData[0], TempData[1] });
                elementConfigs = GetLSConfigData(lsConfigBitData, elementConfigs);
                uiDataCunter += RECORDCONFIG;

                // Log
                filetempdata.Add("LSConfigbyte0, byte1- " + TempData[0].ToString("X2") + TempData[1].ToString("X2"));


                // Get Param size and scalar
                elementConfigs = GetLSParamSizeScalar(elementConfigs, ref uiDataCunter, DataBuffer, ref recordsizeinbytes);

                // Log
                filetempdata.Add("BytesinOneRecord- " + recordsizeinbytes.ToString());

                
                // Get Days 
                // uiDataCunter here is start pointer from which data starts
                // RECORDTIMESTAMP each day has 4 bytes time stamp
                int noofdays = (int)(recievedDataCount - CHECKSUMLENGTH - uiDataCunter + RECORDTIMESTAMP) / (recordsizeinbytes * recordsperday + RECORDTIMESTAMP);
                // Log
                filetempdata.Add("NoOfDay- " + noofdays.ToString());


                // Get remaining records
                int noofrecord = (int)(recievedDataCount - CHECKSUMLENGTH - uiDataCunter + RECORDTIMESTAMP) % (recordsizeinbytes * recordsperday + RECORDTIMESTAMP);
                noofrecord = (noofrecord - RECORDTIMESTAMP) / recordsizeinbytes;
                // Log
                filetempdata.Add("RemainingRecordsinday- " + noofrecord.ToString());

                filetempdata.Add("**************data Start************");

                string fileelements = "";
                for (int ielementcount = 0; ielementcount < elementConfigs.Count; ielementcount++)
                {
                    fileelements += elementConfigs[ielementcount].Unit + ", ";
                }

                filetempdata.Add(fileelements);

                // Fill Data record wise
                for (int irecordcount = recordIndex, irecordsinday = recordIndex, idatapointer = 0; irecordcount < (noofdays * recordsperday) + noofrecord + recordIndex; irecordcount++, irecordsinday++, idatapointer++)
                {
                    List<bool> booldatalist = new List<bool>();
                    MeterDataPacket meterDataPacket = new MeterDataPacket();
                    meterDataPacket.ReadingDate = DateTime.Now;
                    meterDataPacket.ListDataElementValue = new List<DataElement>();

                    // Check when one day records completed and switch to next date to get time stamp of 4 bytes
                    if (irecordcount != 0 && irecordcount % recordsperday == 0)
                    {
                        TempData = new byte[4];
                        uint itestval = (uint)(uiDataCunter + idatapointer * recordsizeinbytes);
                        
                        Array.Copy(DataBuffer, uiDataCunter + idatapointer * recordsizeinbytes, TempData, 0, RECORDTIMESTAMP);
                        recordDatetime = BitConverter.ToUInt32(TempData, 0);
                        uiDataCunter += RECORDTIMESTAMP;
                        string temptime = GetDateFromBase(recordDatetime).ToString("dd-MM-yyyy HH:mm:ss");
                        irecordsinday = 0;
                    }

                    filetempdata.Add("Record*** " + irecordsinday.ToString());
                    
                    byte[] bdata = new byte[recordsizeinbytes];
                    Array.Copy(DataBuffer, uiDataCunter + idatapointer * recordsizeinbytes, bdata, 0, recordsizeinbytes);
                    // Reverse array for little indian
                    Array.Reverse(bdata);

                    // Convert to bit array for bit manipulation
                    for (int irecordbits = 0; irecordbits < recordsizeinbytes; irecordbits++)
                    {
                        BitArray bitarray = new BitArray(new byte[] { bdata[irecordbits] });
                        bitarray.CopyTo(boolArray, 0);
                        Array.Reverse(boolArray);
                        booldatalist.AddRange(boolArray);
                    }

                    booldatalist.Reverse();
                    string recorddata = "";
                    // Fill value in parameters of each record
                    for (int iparam = 0; iparam < elementConfigs.Count; iparam++)
                    {
                        DataElement dataElement = new DataElement();
                        dataElement.DataDefinitionID = elementConfigs[iparam].DataDefID;
                        dataElement.Unit = elementConfigs[iparam].Unit;

                        int byteLength = elementConfigs[iparam].LengthOfDataType;

                        // Checks used in SF
                        if (elementConfigs[iparam].DataDefID == 9 || elementConfigs[iparam].DataDefID == 41 || elementConfigs[iparam].DataDefID == 30)
                        {
                            // Get 4 bytes for Date Time
                            UInt64 DatVal = (UInt64)(recordDatetime + (irecordsinday+1) * ipvalue * SECONDS);
                            dataElement.Value = GetDateFromBase(DatVal).ToString("dd-MM-yyyy HH:mm:ss");
                            meterDataPacket.ListDataElementValue.Add(dataElement);
                            //Log
                            recorddata += dataElement.Value;
                            continue;
                        }


                        bool[] paramdatabits = new bool[elementConfigs[iparam].LengthInBits];
                        Array.Copy(booldatalist.ToArray(), iparam * elementConfigs[iparam].LengthInBits, paramdatabits, 0, elementConfigs[iparam].LengthInBits
);
                        BitArray paramdataArray = new BitArray(paramdatabits);
                        int[] uiarray = new int[1];
                        paramdataArray.CopyTo(uiarray, 0);

                        dataElement.Value = uiarray[0].ToString();
						 #region UnitConversionPrecision
                        //******************Unit Conversion Precision
                        if (elementConfigs[iparam].Scalar == 0 && string.IsNullOrEmpty(elementConfigs[iparam].Unit)) //scalar can be zero for RTC or counters
                        {
                        }
                        else
                        {
                            DataElement newElement = GetUnitConvertedWithPrecisionCF(dataElement, elementConfigs[iparam].Scalar, elementConfigs[iparam].Precision);
                            dataElement.Value = newElement.Value;
                            dataElement.Unit = newElement.Unit;
                        }
                        //******************Unit Conversion Precision
                        #endregion
                        // Add records param in one record                      
                        meterDataPacket.ListDataElementValue.Add(dataElement);
                        //Log
                        recorddata += dataElement.Value + ",";
                    }
                    // Add records
                    filetempdata.Add(recorddata);
                    lstMeterDataPacket.Add(meterDataPacket);
                }

                File.WriteAllLines(filename, filetempdata.ToArray());
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

           
            return lstMeterDataPacket;
        }

        private List<DataElementConfiguration> GetLSConfigData(BitArray bitsdata, List<DataElementConfiguration> elementConfigs)
        {
            try
            {
                const int RTCINDEXFORRECORD = 6;
                List<DataElementConfiguration> lsconfiglist = new List<DataElementConfiguration>();
                if (bitsdata == null) return null;
                if (elementConfigs == null || elementConfigs.Count == 0) return null;

                for (int idata = 0; idata < bitsdata.Count; idata++)
                {
                    if (bitsdata[idata] == true)
                        lsconfiglist.Add(elementConfigs[idata]);
                }

                // For Date Time must be added for date and time
                lsconfiglist.Add(elementConfigs[RTCINDEXFORRECORD]);

                return lsconfiglist;
            }
            catch (Exception)
            {
                // Add Logger Here
            }

            return null;
        }

        private List<DataElementConfiguration> GetLSParamSizeScalar(List<DataElementConfiguration> lsfddata, ref uint startindex, byte[] DataBuffer, ref int recordsizeinbytes)
        {
            try
            {
                const byte BYTESIZEINBITS = 8;

                //if (lsParamCount < 1) return null;
                if (lsfddata == null || lsfddata.Count == 0) return null;
                byte bcount = 0;
                int lsParamCount = lsfddata.Count - 1;

                for (int icount = 0; icount < lsfddata.Count; icount++)
                {
                    lsfddata[icount].LengthInBits = DataBuffer[startindex + bcount]; bcount++;
                    lsfddata[icount].Scalar = DataBuffer[startindex + bcount]; bcount++;
                    recordsizeinbytes += lsfddata[icount].LengthInBits;

                }
                recordsizeinbytes = recordsizeinbytes / BYTESIZEINBITS;
                startindex += (uint)(lsParamCount * 2);

                return lsfddata;

            }
            catch (Exception ex)
            {
            }

            return null;
        }


    }
}
