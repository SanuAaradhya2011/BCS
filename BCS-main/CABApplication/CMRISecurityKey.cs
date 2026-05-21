using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Framework;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class CMRISecurityKey : MdiChildForm
    {
        public CMRISecurityKey()
        {
            InitializeComponent();
        }

        MeterDataBLL meterDataBLL = new MeterDataBLL();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CMRISecurityKey).ToString());

        Dictionary<string, string> dicMeterLPR = null;
        private void btnPushMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAvailableMeters.SelectedItems.Count == 0) { return; }
                foreach (object item in listBoxAvailableMeters.SelectedItems)
                {
                    listBoxSelectedMeters.Items.Add(item);
                }
                foreach (object item in listBoxSelectedMeters.Items)
                {
                    listBoxAvailableMeters.Items.Remove(item);
                }
                if (listBoxAvailableMeters.Items.Count >= 1)
                { listBoxAvailableMeters.SelectedIndex = 0; }
                listBoxAvailableMeters.Focus();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushMove_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushMoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                while (count < listBoxAvailableMeters.Items.Count)
                {
                    listBoxSelectedMeters.Items.Add(listBoxAvailableMeters.Items[count]);
                    count++;
                }
                listBoxAvailableMeters.Items.Clear();
                if (listBoxSelectedMeters.Items.Count > 0) listBoxSelectedMeters.SelectedIndex = 0;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " btnPushMoveAll_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxSelectedMeters.SelectedItems.Count == 0) { return; }
                foreach (object item in listBoxSelectedMeters.SelectedItems)
                {
                    listBoxAvailableMeters.Items.Add(item);
                }
                foreach (object item in listBoxAvailableMeters.Items)
                {
                    listBoxSelectedMeters.Items.Remove(item);
                    if (listBoxAvailableMeters.Items.Count == 0) { return; }
                }
                if (listBoxSelectedMeters.Items.Count >= 1)
                { listBoxSelectedMeters.SelectedIndex = 0; }
                listBoxSelectedMeters.Focus();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushRemove_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnPushRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                while (count < listBoxSelectedMeters.Items.Count)
                {
                    listBoxAvailableMeters.Items.Add(listBoxSelectedMeters.Items[count]);
                    count++;
                }
                listBoxSelectedMeters.Items.Clear();
                if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPushRemoveAll_Click(object sender, EventArgs e)", ex);
            }
        }

        private string GetScheduleFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;

            openFile.DefaultExt = "cfg";
            openFile.Filter = "Config(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            string AllSelectedFile = openFile.FileName;

            if (result == DialogResult.OK)
            {
                //if (AllSelectedFile.Length > 0) lblcfgfile.Text = AllSelectedFile;
                //else
                //{
                //    this.StatusMessage = "File is corrupted";
                //    Application.DoEvents();
                //    return "";
                //}
            }
            else
            {
                //do nothing
            }
            return AllSelectedFile;
        }

        private string VerifyTouFile(string fileName)
        {
            try
            {
                string tmpCount = string.Empty;
                string fileContent = string.Empty;
                string touSendCommand = string.Empty;
                string touFile = string.Empty;
                if (!File.Exists(fileName)) return "";
                StringBuilder fileData = new StringBuilder();
                StreamReader streamReader = File.OpenText(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                while (((tmpCount = streamReader.ReadLine()) != null))
                {
                    fileContent += tmpCount;
                }

                streamReader.Close();
                if (fileContent.Length > 0)
                {
                    if (fileContent.Length < 237) return "1";
                    else return "0";
                }
                return "1";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return "";
                logger.Log(LOGLEVELS.Error, "VerifyTouFile(string fileName)", ex);
            }
        }

        public string CalDataBcc(string RecInpData)
        {
            try
            {
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                int Bcc = 0;
                int countbyt = 0;
                string TempStr = "";
                while (countbyt < RecInpData.Length)
                {
                    TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                    countbyt += 2;
                }
                countbyt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(TempStr);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }
                string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();

                return retval;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return "False";
                logger.Log(LOGLEVELS.Error, "CalDataBcc(string RecInpData)", ex);
            }
        }

        private string StrToHexForDTM(string GetStr)
        {
            int indecount = 0;
            int intmod = 0;
            int intDiv = 0;
            string strval = "";
            char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            indecount = Convert.ToInt16(GetStr);

            intmod = (indecount % 16);
            intDiv = (indecount / 16);
            if (intDiv <= 9)
            {
                strval = (intDiv + 30).ToString();
            }
            else
            {
                int index1 = Convert.ToUInt16(_hexChars[intDiv]);
                strval = (_hexChars[index1 / 16]).ToString() + (_hexChars[index1 % 16]).ToString();
            }
            if (intmod <= 9)
            {
                strval += (intmod + 30).ToString();
            }
            else
            {
                int index2 = Convert.ToUInt16(_hexChars[intmod]);
                strval += (_hexChars[index2 / 16]).ToString() + (_hexChars[index2 % 16]).ToString();
            }
            return strval;
        }

        public string StrToHex(string GetStr)
        {
            string tempstr = "";
            try
            {
                int indecount = 0;
                char AsciiCh;
                int chrascii;
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                while (indecount < GetStr.Length)
                {
                    AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                    if ((AsciiCh >= 48) && AsciiCh <= 57)
                    {
                        chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                        tempstr += chrascii.ToString();
                    }
                    else
                    {
                        if (AsciiCh != 32)
                        {
                            chrascii = Convert.ToInt16(AsciiCh);
                            AsciiCh = _hexChars[chrascii / 16];
                            tempstr += (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                        }
                        else
                        {
                            tempstr += "20";       //Space
                        }
                    }
                    indecount++;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return "";
                logger.Log(LOGLEVELS.Error, "StrToHex(string GetStr)", ex);
            }
            return tempstr;
        }

      

        private string GetMeterIDList()
        {
            string MeterIdList = string.Empty;
            DataTable DTable = new DataTable();
            try
            {
                int itmcnt = 0;
                while (itmcnt < listBoxSelectedMeters.Items.Count)
                {
                    MeterIdList += "\r\n" + listBoxSelectedMeters.Items[itmcnt++].ToString().Trim();
                }
                return MeterIdList;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return "";
                logger.Log(LOGLEVELS.Error, "GetMeterIDList()", ex);
            }
        }



        /// <summary>
        /// Create The Schedule File On Selected Path
        /// </summary>
        /// <param name="FileContent"></param>
        /// <returns></returns>
        private bool CreateScheduleFile(string fileContent)
        {
            try
            {
                SaveFileDialog FileDialog = new SaveFileDialog();
                string fileName = string.Empty;
                FileDialog.Filter = "Schedule Files (*.Lcd)|*.Lcd";
                FileDialog.FilterIndex = 1;
                if (FileDialog.ShowDialog() == DialogResult.Cancel)
                    return false;
                else
                    fileName = FileDialog.FileName;
                if (fileName.Trim().Length > 0)
                {
                    this.StatusMessage = "File Saving in  " + fileName + "...";
                    Application.DoEvents();
                    FileStream file1 = new FileStream(fileName, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file1);
                    /*-------------------Calculate File BCC and Append-------------*/
                    string strdata = fileContent.Replace("\r\n", "");
                    //fileName = "cfgFilePath:" + lblcfgfile.Text;
                    strdata = StrToHex(strdata);
                    string fileBcc = CalBcc(strdata);
                    int BccVal = Convert.ToInt32(fileBcc, 16);
                    char mchar;
                    if (BccVal == 9 || BccVal == 10 || BccVal == 32)
                        mchar = '\\';
                    else
                        mchar = Convert.ToChar(BccVal);
                    /*--------------------End Of Bcc Appending---------------------*/
                    if (fileName == "") wr1.Write(fileContent + "\r\n" + mchar.ToString());
                    else wr1.Write(fileContent + "\r\n" + mchar.ToString());

                    wr1.Close();
                    file1.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return false;
                logger.Log(LOGLEVELS.Error, "CreateScheduleFile(string fileContent)", ex);
            }
        }
        public string CalBcc(string RecInpData)
        {
            try
            {
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                int Bcc = 0;
                int countbyt = 0;
                string TempStr = "";
                while (countbyt < RecInpData.Length)
                {
                    TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                    countbyt += 2;
                }
                countbyt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(TempStr);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }
                string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();

                return retval;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return "False";
                logger.Log(LOGLEVELS.Error, "CalBcc(string RecInpData)", ex);
            }
        }

        private void CMRIScheduleFile_Load(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            DataSet dSet = new DataSet();
            //dSet = meterDataBLL.ListAllMeterIDValues();
            dSet = meterDataBLL.ListAllMeterIDLPRIDValues();
            dicMeterLPR = new Dictionary<string, string>();
            foreach (DataRow drow in dSet.Tables[0].Rows)
            {
                if (!dicMeterLPR.ContainsKey(Convert.ToString(drow["MeterID"])))
                {
                    dicMeterLPR.Add(Convert.ToString(drow["MeterID"]), Convert.ToString(drow["Meter_GPRSModem_IMEI"]));
                    listBoxAvailableMeters.Items.Add(drow["MeterID"].ToString());
                }
            }
        }
        
        private void CMRIScheduleFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void btnCMRISchedule_Click(object sender, EventArgs e)
        {
            string MeterIDList = string.Empty;
            string oneLineText = string.Empty;
          //  StringBuilder scheduleFileData = new StringBuilder();
           // StringBuilder nonDLMSData = new StringBuilder();
           // StreamReader reader = null;

            if (listBoxSelectedMeters.Items.Count <= 0)
            {
                MessageBox.Show("Please select a Meter ID", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (listBoxAvailableMeters.Items.Count > 0) listBoxAvailableMeters.SelectedIndex = 0;
                return;
            }
        List<string> MatcheddataList = new List<string>();
        foreach (var item in listBoxSelectedMeters.Items)
                {
                    MatcheddataList.Add(item.ToString());
                } 
            if (CreateXml(MatcheddataList))
            {
                this.StatusMessage = "Security File Created Successfully";
                Application.DoEvents();
            }
            else
                this.StatusMessage = "Security File Creation Failed";
          
        }

        private  string ToHex(string input)
        {
            //foreach (char ch in meterID)
            //{
            //    Buffer[nBufferIndex++] = Convert.ToByte(ch);
            //}

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("{0:X2}", (int)c);
            return sb.ToString().Trim();
        }


        private void btnScheduleCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            this.Close();
        }

        private bool CreateXml(List<string> configparaList)
        {
            return false;
        }
        //private bool CreateXml(List<string> configparaList)
        //{
        //    string xmlfilename = "EndDeviceSecurityConfig.xml";
        //    bool isSuccess = true;
        //    string StrPgmDesc = string.Empty;
        //    try
        //    {


        //        RequestMessage objxsd = new RequestMessage();
        //        RequestMessageHeader objheader = new RequestMessageHeader();
        //        objheader.Verb = "get";
        //        objheader.Noun = "GetEndDeviceSecurityConfig";
        //        objheader.Revision = 2.0M;
        //        objheader.Timestamp = System.DateTime.Now;
        //        objheader.Source = "HHU";
        //        objheader.MessageID = Guid.NewGuid().ToString();// "7427076d-4dde-47ad-a8f9-83880bcca114";
        //        objheader.CorrelationID = "D9F521CB-406F-4F29-AEC3-2F17C8755B2D";
        //        objxsd.Header = objheader;

        //        List<GetEndDeviceSecurityConfigEndDeviceSecurityNames> objNameCollection = new List<GetEndDeviceSecurityConfigEndDeviceSecurityNames>();
        //        GetEndDeviceSecurityConfigEndDeviceSecurityNames ops = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();
        //        GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType objNameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();
        //        int commdCount = 0;
        //        while (commdCount < configparaList.Count)
        //        {
        //            string paraContents = configparaList[commdCount];

        //            ops = new GetEndDeviceSecurityConfigEndDeviceSecurityNames();
        //            objNameType = new GetEndDeviceSecurityConfigEndDeviceSecurityNamesNameType();
        //            ops.name = configparaList[commdCount];
        //            objNameType.description = ops.name + "description";
        //            objNameType.name = ops.name + "name";
        //            ops.NameType = objNameType;
        //            objNameCollection.Add(ops);
        //            commdCount++;
        //        }
        //        GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey objKeyInfo = new GetEndDeviceSecurityConfigEndDeviceSecurityKeyEncryptionKey();
        //        RequestMessageRequest oblPayload = new RequestMessageRequest();
        //        //-----------------------RSA to Get Packet Encryption Exponent & Modulus-------------------------------------
        //        string publicKey = "a383a2916281721498ff28226f851613bab6f89eb0536e9f237e158596d3b012e5707eba9f2a2963faca63fcb10f5f246c1f587ee6e8f895fd848f2da5aba9d71af4dd8d06e99ff3729631626ed3f3202e56962957c0110a99d2b3893feb148291e09b54fe7df121751fb8bb589576542321b4f548be06b9845ebc6bbef1427741c00b632c05854146b597fdef5a89ace1556a769c5eaff8fc0589e7ad4adb2e2a929969c77f395b2f5a276a9389d1f43c061c9459a65b77bcd581c107aa8424223a0b44ee52582362cc96b90eea071a0dda5e9cb8fd5c31240dc1c9169a629ecec31751069f0c7ccc1c17523038fd5c31240dc1c9169a629ecec31751069f0c";
        //        string exponant = "098765";
        //        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //        {
        //            RSAParameters rsap = new RSAParameters
        //            {
        //                Modulus = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey))),
        //                Exponent = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(exponant)))
        //            };
        //            objKeyInfo.exponent = Convert.ToBase64String(Encoding.UTF8.GetBytes(publicKey));// rsap.Exponent;
        //            objKeyInfo.modulus = Convert.ToBase64String(Encoding.UTF8.GetBytes(exponant));// rsap.Modulus;
        //        }
        //        //-----------------------------------------------------------------------------------------------------------
        //        GetEndDeviceSecurityConfigEndDeviceSecurity objeds = new GetEndDeviceSecurityConfigEndDeviceSecurity();
        //        objeds.Names = objNameCollection.ToArray();
        //        objeds.KeyEncryptionKey = objKeyInfo;

        //        GetEndDeviceSecurityConfig objCong = new GetEndDeviceSecurityConfig();
        //        objCong.EndDeviceSecurity = objeds;

        //        oblPayload.GetEndDeviceSecurityConfig = objCong;
        //        objxsd.Request = oblPayload;

        //        XmlSerializer xsSubmit = new XmlSerializer(typeof(RequestMessage));
        //        StringWriter sww = new StringWriter();
        //        XmlWriter writer = XmlWriter.Create(sww);
        //        xsSubmit.Serialize(writer, objxsd);
        //        var xml = sww.ToString();
        //        //localhost.Service1 objserviceRequest = new App_1Phase.localhost.Service1();
        //        //string getresp = objserviceRequest.GetSecurityKey(xml);
        //        //MessageBox.Show(getresp);
        //        var xmldocument = XDocument.Parse(xml);
        //        xmldocument.Save(xmlfilename);


        //    }
        //    catch (Exception ex)
        //    {
        //        isSuccess = false;
        //    }
        //    return isSuccess;

        //}

        private string VerifycfgFile(string fileName)
        {
            try
            {
                string tmpCount = string.Empty;
                string fileContent = string.Empty;
                if (!File.Exists(fileName)) return "";
                StringBuilder fileData = new StringBuilder();
                StreamReader streamReader = File.OpenText(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                while (((tmpCount = streamReader.ReadLine()) != null))
                {
                    fileContent += tmpCount;
                }

                streamReader.Close();
                if (fileContent.Length > 0)
                {
                    if (fileContent.Length <= 0) return "1";
                    else return "0";
                }
                return "1";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "VerifycfgFile(string fileName)", ex);

                return "";
                
            }
        }

        //private void btnSelectcfgfile_Click(object sender, EventArgs e)
        //{
        //    this.StatusMessage = string.Empty;
        //    lblcfgfile.Text = "";
        //    string cfgCommand = GetScheduleFile();
        //    if (cfgCommand == "") return;
        //    lblcfgfile.Text = cfgCommand;
        //    string fileExt = string.Empty;
        //    if (lblcfgfile.Text.IndexOf(".") > 0)
        //    {
        //        if (!(lblcfgfile.Text.Contains(".cfg")) && (!(lblcfgfile.Text.Contains(".CFG"))))
        //        {
        //            this.StatusMessage = "Please select the configuration file";
        //            Application.DoEvents();
        //          //  btnSelectcfgfile.Focus();
        //            lblcfgfile.Text = "";
        //            return;
        //        }
        //    }

        //    if (lblcfgfile.Text.Trim().Length > 0) cfgCommand = VerifycfgFile(lblcfgfile.Text);

        //    if (cfgCommand.Length <= 0)
        //    {
        //        this.StatusMessage = "Please select the configuration file";
        //        Application.DoEvents();
        //        lblcfgfile.Text = "";
        //        return;
        //    }
        //    if (cfgCommand == "1")
        //    {
        //        this.StatusMessage = "File corrupted!";
        //        Application.DoEvents();
        //        lblcfgfile.Text = "";
        //        return;
        //    }
        //}

        private void txtClearCMRI_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ascii = Convert.ToInt16(e.KeyChar);
            if (!((ascii >= 97 && ascii <= 122) || (ascii >= 65 && ascii <= 90) || (ascii >= 48 && ascii <= 57) || (ascii == 8)))
            { e.Handled = true; }
        }

        private void txtLSDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ascii = Convert.ToInt16(e.KeyChar);
            if (!((ascii >= 48 && ascii <= 57) || (ascii == 8)))
            { e.Handled = true; }
        }

        private void CMRIScheduleFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }

        //private void chkisLPRSchedule_CheckedChanged(object sender, EventArgs e)
        //{
        //    btnSelectcfgfile.Enabled = !chkisLPRSchedule.Checked;
        //}
    }
}
