using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABCommunication.PhysicalLayer;
using CABCommunication.Common;

namespace CABCommunication.DataLinkLayer
{
    public class IrDALayer
    {
        int HDLCIndex = 0;
        //public delegate void UpdateHandler(object sender, UpdateEventArgs e);
        //public event UpdateHandler UpdatedLed;
        //UpdateEventArgs args = null;
        public int errormsgStstus = 0;
        byte[] HDLCCommand = new byte[1024];
        public int getWriteResponseCode = 0;
        public string AppDirectoryLocalPath = AppDomain.CurrentDomain.BaseDirectory + "\\Configuration";
        public string MeterInfoValue = "";
        public SerialIrDA serial;

        public IrDALayer(SerialIrDA channel)
        {
            serial = channel;
        }

        #region Enums

        public enum ProgrammingCode
        {
            Success,
            Fail,
            AccessDenied,
            DataUnavailable,
            TimeOut,
            SignOnFailed,
            CosemConnectionFailed,
            MeterIDMismatch

        }

        public enum MeterTypeInfo { Smart_Meter_1PH = 0, MicroStar_DLMS = 1, Smart_Meter_3PH = 2, DLMS_3PH = 3, SAPPHIRE = 4, DLMS_3PH_RUBY = 5, Non_DLMS_1PH = 6 };

        public enum IrDACommandType { InitiationCommand = 0x96, BillingDataCommand = 0x00, ClosingCommand = 0x9E, MeterSerialNo = 0x41 };
        #endregion

        public List<string> GetMeterTypeList()
        {
            List<string> meterTypeList = new List<string>();
            meterTypeList.Add("1Phase-Smart Meter");
            meterTypeList.Add("1Phase -DLMS");
            meterTypeList.Add("3Phase-Smart Meter");
            meterTypeList.Add("3Phase-DLMS-PUMA");
            meterTypeList.Add("3Phase-Sapphire");
            meterTypeList.Add("3Phase-RUBY");
            meterTypeList.Add("1Phase-NON-DLMS");
            return meterTypeList;
        }

        public string GetSelectedMeterType()
        {
            //AppSettings objappSettings = new AppSettings();
            List<string> meterTypelist = GetMeterTypeList();
            //return meterTypelist[objappSettings.GetMeterMode()];
            return "";
        }

        public void DisplayStatusMsg(string msgString, bool isError)
        {
            try
            {
                //args = new UpdateEventArgs(msgString, isError);
                //UpdatedLed(this, args);
            }
            catch (Exception)
            {
            }
        }

        //public bool ConnectToIrDAMeters()
        //{
        //    MeterInfoValue = string.Empty;
        //    //AppSettings objappSettings = new AppSettings();
        //    DisplayStatusMsg("  Physical Layer Communication...", false);
        //    if (!PhysicalLayerConnect()) { DisplayStatusMsg("Physical Layer Connection Failed!", true); return false; }
        //    DisplayStatusMsg("Device Is Connected, Please Wait...", false);
        //    return true;
        //}

        //public bool PhysicalLayerConnect()
        //{
        //    try
        //    {
        //        GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, SerialPortSettings.Default.CommandBaudRate, "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
        //        if (GlobalObjects.objSerialComm.OpenPort()) return true;
        //        else return false;

        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        //public bool AssociationDisconnect()
        //{
        //    try
        //    {
        //        PhysicalLayerDisconnect();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        DisplayStatusMsg("Unable To Close Current Association!", true);
        //        return false;

        //    }

        //}

        //public void PhysicalLayerDisconnect()
        //{
        //    try
        //    {
        //        GlobalObjects.objSerialComm.ClosePort();
        //        return;
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }
        //}
        public string HexToDecimalConversion(string strHex)
        {
            try
            {
                int bytecnt = 0;
                bytecnt = strHex.Length;
                string strtemp = "";
                while (bytecnt > 0)
                {
                    strtemp += strHex.Substring(bytecnt - 2, 2);
                    bytecnt -= 2;
                }
                long aa = Int64.Parse(strtemp, System.Globalization.NumberStyles.HexNumber);
                return aa.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public Result ReadIrDAByteFromMeter(byte IrDAReadCommandType, int IrDAMeterid, int IrDAhhuID, string CommandData)
        {
            string value = "";
            Result writeResponse = ReadDIrDAataCommand(IrDAReadCommandType, IrDAMeterid, IrDAhhuID, CommandData);
            //getWriteResponseCode = writeResponse;
            if (writeResponse.ErrorCode == CommunicationErrorType.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/  }
            else if (writeResponse.ErrorCode == CommunicationErrorType.AccessDenied) { value = "Access Denied!";/* MessageBox.Show("Access Denied!", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/ }
            else if (writeResponse.ErrorCode == CommunicationErrorType.Nothing) { value = "Data Not Available!"; }
            else if (writeResponse.ErrorCode == CommunicationErrorType.CosemConnectionFailed) { value = "Cosem Connection Failed!"; }
            else { value = "Communication Failed!"; }
            return writeResponse;
        }

        public bool fChecIrDAResponse(byte[] Buffer, int IrDAMeterid, int IrDAhhuID, byte IrDAReadCommandbyte)
        {
            try
            {
                if (!IrDACheckBCC(Buffer)) { DisplayStatusMsg("   BCC Not Match", false); return false; }
                if (!IrDACheckSyncWord(Buffer)) { DisplayStatusMsg("   Invalid IrDA Sync Word", false); return false; }
                //Byte Position offset is 2,3,4
                if (Convert.ToInt32(HexToDecimalConversion(Buffer[4].ToString("X2") + Buffer[3].ToString("X2") + Buffer[2].ToString("X2"))) != IrDAhhuID) { DisplayStatusMsg("  Invalid Source ID Received", false); return false; }
                //if (DLMSDataStracture.HexToDecimalConversion(Buffer[4] + Buffer[3] + Buffer[2]) !GlobalObjects.objHDLCLIB.IrDACheckHHUIP(Buffer, IrDAhhuID)) { DisplayStatusMsg("  Invalid Source ID Received", false); return false; }
                if (!IrDACheckCommandID(Buffer, IrDAReadCommandbyte)) { DisplayStatusMsg("  Invalid Command Received", false); return false; }
                //Byte Position offset is 7,8,9
                int resMeterID = Convert.ToInt32(HexToDecimalConversion(Buffer[9].ToString("X2") + Buffer[8].ToString("X2") + Buffer[7].ToString("X2")));
                if (resMeterID <= 0) { DisplayStatusMsg("  Invalid Destination ID Received", false); return false; }
                return true;
            }
            catch (Exception)
            {
                DisplayStatusMsg("   Invalid Data", false);
                return false;
            }
        }



        private Result ReadDIrDAataCommand(byte IrDAReadCommandbyte, int IrDAMeterid, int IrDAhhuID, string CommandData)
        {
            Result result = new Result();
            try
            {
                HDLCIndex = 0;
                HDLCCommand[HDLCIndex++] = 0x95;
                HDLCCommand[HDLCIndex++] = 0x95;
                //---------------------------Meter IP---------------------------------
                HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAMeterid & 0xFF0000) >> 16);
                HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAMeterid & 0xFF00) >> 8);
                HDLCCommand[HDLCIndex++] = Convert.ToByte(IrDAMeterid & 0x00FF);
                //-----------Pay load Byte, Length of command data----------------------
                HDLCCommand[HDLCIndex++] = 0x00;// Convert.ToByte(CommandData.Length);
                //-----------Command Type----------------------
                HDLCCommand[HDLCIndex++] = IrDAReadCommandbyte;
                //---------------------------HHU IP---------------------------------
                HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAhhuID & 0xFF0000) >> 16);
                HDLCCommand[HDLCIndex++] = Convert.ToByte((IrDAhhuID & 0xFF00) >> 8);
                HDLCCommand[HDLCIndex++] = Convert.ToByte(IrDAhhuID & 0x00FF);
                byte bccByte = 0x00;
                if (CommandData.Length > 0)
                {
                    int datacount = 0;
                    while (datacount < CommandData.Length)
                    {
                        HDLCCommand[HDLCIndex++] = Convert.ToByte(CommandData.Substring(datacount, 2));
                        datacount += 2;
                    }

                }
                //---------------Calculate BCC---------------
                //-----No Data Command required so no need to calculate BCC for send command
                //-------------------------------------------
                HDLCCommand[HDLCIndex++] = bccByte; //---BCC
                HDLCCommand[5] = Convert.ToByte(HDLCIndex);

                result = serial.Send(HDLCCommand, (byte)HDLCIndex);
                if (result.ErrorCode!=CommunicationErrorType.Success)
                {
                    if (IrDAReadCommandbyte == (byte)IrDALayer.IrDACommandType.ClosingCommand)
                    {
                        //return (int)ProgrammingCode.Success;//-----For Closing Command No response will come from meter 
                        result.ErrorCode = CommunicationErrorType.Success;
                    }
                    //return (int)ProgrammingCode.CosemConnectionFailed;
                }
                if (!fChecIrDAResponse(result.RecieveDataBuffer.ToArray(), IrDAMeterid, IrDAhhuID, IrDAReadCommandbyte))
                {
                    //return (int)ProgrammingCode.CosemConnectionFailed;
                    result.ErrorCode = CommunicationErrorType.CosemConnectionFailed;
                }
                else
                {
                    result.ErrorCode = CommunicationErrorType.Success;
                }
                
            }
            catch (Exception ex)
            {
                //return (int)ProgrammingCode.CosemConnectionFailed;
            }
            return result;
        }


        //-----------------------1P IrDA-------------------------------------------------

        public int ASCIIHexToDecimalConversion(byte[] recBuffer, int startdata, int enddata)
        {
            try
            {
                string hexString = "";
                while (startdata <= enddata)
                {
                    char AsciiCh = Convert.ToChar(recBuffer[startdata++]);
                    if ((AsciiCh >= 48) && AsciiCh <= 57) hexString += (Convert.ToInt16(AsciiCh) - 48).ToString();
                    else hexString += (AsciiCh).ToString();
                }
                return Convert.ToInt32(hexString, 16);

            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string Read1P_IrDAByteFromMeter(byte[] IrDAReadCommand)
        {
            string value = "";
            int writeResponse = Read1P_DIrDAataCommand(IrDAReadCommand);
            getWriteResponseCode = writeResponse;
            if (writeResponse == (int)ProgrammingCode.Success) { /*DisplayStatusMsg("Reading Succesfull.", false);*/  }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied) { value = "Access Denied!";/* MessageBox.Show("Access Denied!", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);*/  }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable) { value = "Data Not Available!"; }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed) { value = "Cosem Connection Failed!"; }
            else { value = "Communication Failed!"; }
            return value;
        }

        public bool fChec1P_IrDAResponse(byte[] Buffer)
        {
            try
            {
                int getlen = ASCIIHexToDecimalConversion(Buffer, 1, 2);
                if (!IrDACheckBCC_1P(Buffer, getlen)) { DisplayStatusMsg("   BCC Not Match", false); return false; }
                if (!IrDACheckSyncWord_1P(Buffer, getlen)) { DisplayStatusMsg("   Invalid IrDA Fram", false); return false; }
                return true;
            }
            catch (Exception)
            {
                DisplayStatusMsg("   Invalid Data", false);
                return false;
            }
        }

        private int Read1P_DIrDAataCommand(byte[] IrDAReadCommand)
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                Result result = serial.Send(IrDAReadCommand, (byte)IrDAReadCommand.Length);
                if (result.ErrorCode!=CommunicationErrorType.Success) return (int)ProgrammingCode.CosemConnectionFailed;

                if (!fChec1P_IrDAResponse(result.RecieveDataBuffer.ToArray())) return (int)ProgrammingCode.CosemConnectionFailed;
                else return (int)ProgrammingCode.Success;

            }
            catch (Exception)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        #region IrDA Methods

        //======================================================IrDA Specefic Methods===========================================
        /// <summary>
        /// Byte Position offset is 0,len
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckSyncWord_1P(byte[] Buffer, int recPayloadLen)
        {
            if (Buffer[0] == 0x3A && Buffer[recPayloadLen + 10] == 0x0A) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 0,1
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckSyncWord(byte[] Buffer)
        {
            if (Buffer[0] == 0x95 && Buffer[1] == 0x95) return true;
            else return false;
        }
        /// <summary>
        /// To Calculate BCC Make xor with reach byte.
        /// BBC of Data Part only
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckBCC(byte[] Buffer)
        {
            byte bcc = 0x00;
            int recPayloadLen = Buffer[5] - 1;
            if (Buffer[recPayloadLen] == 0x00) return true; //-----If no Payload data
            int dataIndex = 10; //------Data Start from index position 10th 
            while (dataIndex < recPayloadLen)
            {
                bcc += (byte)~Buffer[dataIndex++];
            }
            if (bcc != Buffer[recPayloadLen]) return false;
            return true;
        }
        /// <summary>
        /// To Calculate BCC Make xor with reach byte.
        /// BBC of Data Part only
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckBCC_1P(byte[] Buffer, int recPayloadLen)
        {
            byte bcc = 0x00;
            int connadTypeByteLen = 2;
            int DataLen = 2;
            recPayloadLen = recPayloadLen + connadTypeByteLen + DataLen;
            int dataIndex = 1; //------Data Start from index position 10th 
            while (dataIndex <= recPayloadLen)
            {
                bcc += (byte)Buffer[dataIndex++];
                // bcc += (byte)~Buffer[dataIndex++];
            }
            bcc = (byte)~bcc; //--2's Compliment
            byte BCC1 = (byte)(bcc + 1);//--1's Compliment of 2's Compliment
            byte BCC2 = (byte)~BCC1;  ////--2's Compliment of BCC1

            StringBuilder sb = new StringBuilder();
            StringBuilder datasb = new StringBuilder();
            string bccstr = BCC1.ToString("X2") + BCC2.ToString("X2");
            foreach (char c in bccstr)
            {
                if (Buffer[dataIndex++].ToString("X2") != ((int)c).ToString("X2")) return false;
            }


            return true;
        }
        /// <summary>
        /// Byte Position offset is 2,3,4
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns></returns>
        public bool IrDACheckHHUIP(byte[] Buffer, int hhuID)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[2]) << 16;
            compValue = (compValue | (int)Buffer[3]) << 8;
            compValue = (compValue | (int)Buffer[4]);
            if (compValue == hhuID) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 6
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public bool IrDACheckCommandID(byte[] Buffer, byte IrDAReadCommandbyte)
        {
            if (Buffer[6] == IrDAReadCommandbyte) return true;
            else return false;
        }

        /// <summary>
        /// Byte Position offset is 7,8,9
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns></returns>
        public bool IrDACheckMeterIP(byte[] Buffer, int meterID)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[7]) << 16;
            compValue = (compValue | (int)Buffer[8]) << 8;
            compValue = (compValue | (int)Buffer[9]);
            if (compValue == meterID) return true;
            else return false;
        }
        /// <summary>
        /// Byte Position offset is 7,8,9
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="hhuID"></param>
        /// <returns> Meter ID that is came from Meter response</returns>
        public int IrDAGetMeterIP(byte[] Buffer)
        {
            int compValue = 0;
            compValue = (compValue | (int)Buffer[7]) << 16;
            compValue = (compValue | (int)Buffer[8]) << 8;
            compValue = (compValue | (int)Buffer[9]);
            return compValue;
        }
        #endregion

    }
}
