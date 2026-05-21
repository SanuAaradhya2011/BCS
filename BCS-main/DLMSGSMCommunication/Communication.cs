using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using CAB.Entity;
using Utilities;
using CAB.BLL;
using CAB.Framework;
using System.Threading;
using CAB.Framework.Utility;
namespace DLMSGSMCommunication
{
    public delegate void GSMLogEventHandler(object sender, GSMLogEventArgs e);

    public class Communication
    {
        SystemSettingsBLL systemSettingsBLL = null;
        #region Variable Declaration
        string commandResponse = string.Empty;
        byte[] HDLCCommand = new byte[200];
        byte[] MODEMCommand = new byte[20];
        GSMLogEventArgs gsmEventArgs = null;
        byte MODEMIndex = 0;
        byte HDLCIndex = 0;
        byte ShowIndex = 0;
        bool isCurrentCommandOfPTRatio = false;
        UploadFile upload = new UploadFile();
        string portName = string.Empty;
        string strPortName = string.Empty;
        bool isPUMA = false;
        #endregion

        public event GSMLogEventHandler GSMLogCreating;

        public Communication()
        {
            gsmEventArgs = new GSMLogEventArgs();
            gsmEventArgs.IsGeneralCompleted = false;
            gsmEventArgs.IsInstantCompleted = false;
            gsmEventArgs.IsBillingCompleted = false;
            gsmEventArgs.Retries = 1;
            systemSettingsBLL = new SystemSettingsBLL();
            strPortName = systemSettingsBLL.GetSettingValue("COM_PORT");
        }

        public void OnGSMLogCreating(object sender, GSMLogEventArgs e)
        {
            if (GSMLogCreating != null)
                GSMLogCreating(sender, e);
        }

        #region Private Methods for Reading Meter data.
        /// <summary>
        /// This method is used for the checksum from the file which created during reading meter data.
        /// </summary>
        /// <param name="filename">Pass the filename for get checksum.</param>
        /// <returns></returns>
        private string GetMD5ChecksumForFile(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(filename))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", filename));

            using (FileStream fstream = new FileStream(filename, FileMode.Open))
            {
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fstream);

                // Convert the byte array to a printable string.
                StringBuilder sb = new StringBuilder(32);
                foreach (byte hex in hash)
                    sb.Append(hex.ToString("X2"));

                return sb.ToString().ToUpper();
            }
        }

        /// <summary>
        /// This method is used for disconnecting DLMS communication from serial com port.
        /// </summary>
        public void DLMSDisconnect()
        {
            try
            {
                if (GlobalObjects.objGlobalFunctions.fSendDISC(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {
                    EventLogging.CallLogDetails("Disconnected with Meter Successfully.");
                    return;
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    // For bug 70006.
                    EventLogging.CallLogDetails("HDLC Connection Failed..Error occurred while disconnecting with Meter");
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails(ex.Message);
                return;
            }
        }

        /// <summary>
        /// This method is used for initializing the meter id.
        /// </summary>
        /// <param name="iIndex">Pass the index number.</param>
        /// <returns></returns>
        private int InitializeReadMeterID(int iIndex)
        {
            try
            {
                //store value from xml data set
                DataSet OBISLIST = null;
                //define xml data document object
                XmlDataDocument xmlDatadoc = null;
                xmlDatadoc = new XmlDataDocument();
                //serialize the xml data 
                string path = AppDomain.CurrentDomain.BaseDirectory + "Name Plate Details.xml";//SerialPortSettings.Default.ReadOut;//AppDomain.CurrentDomain.BaseDirectory + "DLMSReadOutList.xml";

                xmlDatadoc.DataSet.ReadXml(path);

                //assign memory to dataset object and name it "alerts"
                OBISLIST = new DataSet("OBis List Dataset");
                //deserialize xml data
                OBISLIST = xmlDatadoc.DataSet;
                GlobalObjects.objCOSEMLIB.ObisQueryDSet = OBISLIST;
                //store value from xml data set


                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();


                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQuery(HDLCCommand, HDLCIndex, iIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {

                    return 0x00;
                }
                else
                {

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type                 
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        // This is a workaround as LTCT,HTCT meters will not support PT Ratio.
                        // if access denied and current command is for PT ratio return success.
                        if (ret == 0x03 && isCurrentCommandOfPTRatio)
                        {
                            return 0x01;
                        }
                        if (ret == 0x01)
                        {
                            return 0x01; //Success
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            EventLogging.CallLogDetails("Data unavailable.");
                            return 0x0E;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            EventLogging.CallLogDetails("Access denied.");

                            return 0x03;
                        }
                        else
                        {
                            return 0x00; //Fail
                        }
                    }
                    else
                    {
                        return 0x00;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// This method is used for reading temper profile from meter.
        /// </summary>
        /// <param name="atb">Pass the attribute id.</param>
        /// <param name="tamperCompartment">Pass the temper compartment number.</param>
        /// <returns>Byte value.</returns>
        private byte ReadTamperProfile(byte atb, byte tamperCompartment)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryTamperProfile(HDLCCommand, HDLCIndex, atb, tamperCompartment);

                //added by gopal for Selective Access By Entry
                if (atb == 0x02)
                {
                    //if (rdBtnReadLastEvent.Checked == true)//TODO:Abhay
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(1));

                    }
                    // else if (rdBtnReadBetweenEvent.Checked == true)
                    {

                        //HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFromEvent.Text), Convert.ToByte(cmbBoxToEvent.Text));
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }


            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is using for reading billing profile form the meter.
        /// </summary>
        /// <param name="atb">Pass the attribute id.</param>
        /// <returns>Byte value</returns>        
        private byte ReadBillingProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryBillingProfile(HDLCCommand, HDLCIndex, atb);

                //added by gopal for Selective Access By Entry
                //if (atb == 0x02)
                ////{
                ////    if (rdBtnReadLast.Checked == true)
                ////    {
                ////        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFrom.Text));

                ////    }
                ////    else if (rdBtnReadBetween.Checked == true)
                ////    {
                ////        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFrom.Text), Convert.ToByte(cmbBoxTo.Text));
                ////    }

                //}
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {

                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used for reading load survey data from the meter.
        /// </summary>
        /// <param name="atb"></param>
        /// <returns></returns>
        private byte ReadLSProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryLoadSurveyProfile(HDLCCommand, HDLCIndex, atb);

                //added by dhirendra for Selective Access By Range
                if (atb == 0x02)
                {
                    // if (rdBtnReadBetweenLS.Checked == true)//TODO:Abhay
                    {
                        // HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, dtPickerFrom.Value, dtPickerTo.Value);
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is used for reading scalar profile from the meter.
        /// </summary>
        /// <param name="atb">Pass the atribute information to get sclar profile data.</param>
        /// <param name="nProfileindex">Pass the profile index number to get sclar profile data.</param>
        /// <returns>byte value</returns>
        private byte ReadScalarProfile(byte atb, byte nProfileindex)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (nProfileindex == 0)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 1)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 2)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 3)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 4)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 5)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, atb);
                }

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is used for reading instantaneous data from meter.
        /// </summary>
        /// <param name="atb">pass the atribute value for getting instantaneous data.</param>
        /// <returns>bye value</returns>
        private byte ReadInastantaneous(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantProfile(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This mehod is used for checking HDLC connection response.
        /// </summary>
        /// <param name="Buffer">Pass buffer to check response.</param>
        /// <returns></returns>
        private bool CheckHDLCResponse(byte[] Buffer)
        {
            if (GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails("Invalid Start or end Tag");
                return false;
            }
            else
            {
                if (GlobalObjects.objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    EventLogging.CallLogDetails("Invalid HDLC FCS");
                    return false;
                }
                else
                {
                    if (GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, SerialPortSettings.Default.ClientSAP) == false)
                    {
                        GlobalObjects.objSerialComm.ClosePort();
                        EventLogging.CallLogDetails("Invalid Destination Address");
                        return false;
                    }
                    else
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte) == false)
                        {
                            GlobalObjects.objSerialComm.ClosePort();
                            EventLogging.CallLogDetails("Invalid Response Byte");
                            return false;
                        }
                        else
                            return true;
                    }
                }
            }
        }

        /// <summary>
        /// This method is used for reading serial number.
        /// </summary>
        /// <returns></returns>
        private int ReadMeterSerialNumber()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadMeterID(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                    return 1;
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0;
                        else
                            return 1;
                    }
                    else
                        return 1;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                return 1;
            }
        }

        /// <summary>
        /// This method is used for connecting to serial port using DLMS GSM communication.
        /// </summary>
        /// <returns>A boolean value true or false.</returns>
        public bool DLMSConnect()
        {
            try
            {
 
                //GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.SetSerialPortSettings(strPortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (!GlobalObjects.objSerialComm.OpenPort())
                {
                    EventLogging.CallLogDetails(strPortName + " : Error while connecting with meter. The port is opened by other application.");
                    return false;
                }
                if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {

                    if (SerialPortSettings.Default.SecurityMechanism == 0)
                    {
                        SerialPortSettings.Default.SecurityMechanism = 1;
                    }
                    if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey) == true)
                    {
                        EventLogging.CallLogDetails("Connected to Meter Successfully");
                        return true;
                    }
                    else
                    {

                        GlobalObjects.objSerialComm.ClosePort();
                        EventLogging.CallLogDetails("Cosem Connection Failed..Unable to connect to Meter");
                        return false;
                    }
                }
                else
                {

                    GlobalObjects.objSerialComm.ClosePort();
                    EventLogging.CallLogDetails("HDLC Connection Failed..Unable to connect to Meter");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Disconnect();
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails(ex.Message);
                return false;
            }
        }
        //added PUMA
        private byte fReadCumulativeKVA(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKVA(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        //else if (ret == 0x02)//next packet
                        //{
                        //    while (true)
                        //    {
                        //        fIncrementTimer();

                        //        //Send Block tarsfer Command
                        //        HDLCIndex = 0;
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);

                        //        GlobalObjects.objHDLCLIB.fIncSend();

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                        //        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        //        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        //        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                        //        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                        //        //7EA014022321766E17E6E600C002C100000002CA8C7E
                        //        if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                        //        {
                        //            return 0x00;
                        //        }
                        //        else
                        //        {
                        //            if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                        //            {
                        //                ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //                if (ret == 0x01)
                        //                    break;
                        //                else if (ret == 0x02)
                        //                    continue;
                        //            }
                        //            else
                        //            {
                        //                return 0x00;
                        //            }
                        //        }
                        //    }

                        //    return 0x01;
                        //}
                        //else if (ret == 0x05)
                        //{
                        //    return 0x05;
                        //}
                        //else if (ret == 0x07)
                        //{
                        //    return 0x07;
                        //}
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //added PUMA
        private byte fReadCumulativeKW(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKW(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        //else if (ret == 0x02)//next packet
                        //{
                        //    while (true)
                        //    {
                        //        fIncrementTimer();

                        //        //Send Block tarsfer Command
                        //        HDLCIndex = 0;
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);

                        //        GlobalObjects.objHDLCLIB.fIncSend();

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                        //        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        //        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        //        GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                        //        GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);

                        //        HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                        //        //7EA014022321766E17E6E600C002C100000002CA8C7E
                        //        if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                        //        {
                        //            return 0x00;
                        //        }
                        //        else
                        //        {
                        //            if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                        //            {
                        //                ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //                if (ret == 0x01)
                        //                    break;
                        //                else if (ret == 0x02)
                        //                    continue;
                        //            }
                        //            else
                        //            {
                        //                return 0x00;
                        //            }
                        //        }
                        //    }

                        //    return 0x01;
                        //}
                        //else if (ret == 0x05)
                        //{
                        //    return 0x05;
                        //}
                        //else if (ret == 0x07)
                        //{
                        //    return 0x07;
                        //}
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is being used for the reading meter data.
        /// </summary>
        /// <param name="IsinstantaneousRequired">Pass the boolean value for instantaneous data is required or not to read from meter.</param>
        /// <param name="IsBillingRequired">Pass the boolean value for billing data is required or not to read from meter.</param>
        /// <param name="IsloadSurveyRequired">Pass the boolean value for load survey data is required or not to read from meter.</param>
        /// <param name="IsTamperRequired">Pass the boolean value for tampering data is required or not to read from meter.</param>
        /// <param name="IsGeneralRequired">Pass the boolean value for general data is required or not to read from meter.</param>
        /// <returns>A boolean value true or false.</returns>
        public bool GetMeterData(MeterMasterEntity meterMasterEntity, GSMTaskEntity gsmTaskEntity, int retriesUsed, int retriesCount, string message, bool newMeter, bool isModemConnected)
        {
            bool IsinstantaneousRequired = false;
            bool IsBillingRequired = false;
            bool IsloadSurveyRequired = false;
            bool IsTamperRequired = false;
            bool IsGeneralRequired = false;
            gsmEventArgs.GSMLog = new GSMLoggingEntity();
            gsmEventArgs.GSMLog.Task_ID = gsmTaskEntity.taskId;
            gsmEventArgs.GSMLog.Group_ID = gsmTaskEntity.groupId;
            gsmEventArgs.GSMLog.Meter_ID = meterMasterEntity.Meter_ID;
            IsGeneralRequired = gsmTaskEntity.isGeneralRequired;
            IsBillingRequired = gsmTaskEntity.isBillingRequired;
            IsinstantaneousRequired = gsmTaskEntity.isInstantaneousRequired;
            gsmEventArgs.GSMLog.Status = "";
            gsmEventArgs.GSMLog.Retries = retriesUsed;
            gsmEventArgs.GSMLog.ErrorMessage = message;

            string strTamperScalecapture;
            string strTamperScalebuffer;
            string strFileName;
            string FileMeterdata;
            if (UtilityEntity.PUMA == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");

            if (!Directory.Exists(strFileName))
            {
                Directory.CreateDirectory(strFileName);
            }

            if (newMeter)
            {
                gsmEventArgs.IsGeneralCompleted = false;
                gsmEventArgs.IsBillingCompleted = false;
                gsmEventArgs.IsInstantCompleted = false;

                gsmEventArgs.Log_ID = 0;
            }

            #region Meter Not Connected
            if (isModemConnected)
            {
                if (DLMSConnect() != true)
                {
                    if (IsGeneralRequired)
                    {
                        if (gsmEventArgs.IsGeneralCompleted == false)
                            gsmEventArgs.GSMLog.Status = "NC";
                    }
                    else if (IsBillingRequired)
                    {
                        if (gsmEventArgs.IsBillingCompleted == false)
                            gsmEventArgs.GSMLog.Status = "NC";
                    }
                    else if (IsinstantaneousRequired)
                    {
                        if (gsmEventArgs.IsInstantCompleted == false)
                            gsmEventArgs.GSMLog.Status = "NC";
                    }
                    else
                    {
                        gsmEventArgs.GSMLog.Status = "NS";
                    }

                    gsmEventArgs.GSMLog.ErrorMessage = "Unable to connect to Meter";

                    //raise a logging event here
                    GSMLogCreating(this, gsmEventArgs);
                    if (retriesUsed >= retriesCount)
                        gsmEventArgs.Log_ID = 0;
                    return false;
                }
                else
                    gsmEventArgs.GSMLog.ErrorMessage = "Connected to Meter Successfully";
            }
            else
            {
                if (IsGeneralRequired)
                {
                    if (gsmEventArgs.IsGeneralCompleted == false)
                        gsmEventArgs.GSMLog.Status = "NC";
                }
                else if (IsBillingRequired)
                {
                    if (gsmEventArgs.IsBillingCompleted == false)
                        gsmEventArgs.GSMLog.Status = "NC";
                }
                else if (IsinstantaneousRequired)
                {
                    if (gsmEventArgs.IsInstantCompleted == false)
                        gsmEventArgs.GSMLog.Status = "NC";
                }
                else
                {
                    gsmEventArgs.GSMLog.Status = "NS";
                }
                gsmEventArgs.GSMLog.Status = "NC";
                GSMLogCreating(this, gsmEventArgs);
                if (retriesUsed >= retriesCount)
                    gsmEventArgs.Log_ID = 0;
                return false;
            }
            #endregion

            #region Reading meter ID
            int writeResponse = ReadMeterSerialNumber();

            if (writeResponse == 0)
            {
                string data = string.Empty;

                int idLen = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                if (idLen < 7 || idLen > 16)
                {
                    EventLogging.CallLogDetails("Meter data corrupt");
                    return false;
                }
                string idLength = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                while (idLength.Length < 2) idLength = "0" + idLength;
                int index = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                for (int i = 20; i <= 20 + (index - 1); i++)
                {
                    data += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                }
                strFileName = strFileName + data;
                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                FileMeterdata = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
            }
            else
            {
                EventLogging.CallLogDetails("Cosem Connection Failed");
                return false;
            }
            #endregion

            bool bSuccess = true;
            FileStream file1 = new FileStream(strFileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);
            byte ret;

            try
            {
                wr1.WriteLine("00" + FileMeterdata);
                #region Nameplate General
                if (IsGeneralRequired)
                {
                    EventLogging.CallLogDetails("Now start reading General data from Meter");
                    int iIndex = 0;
                    int nObjectCount = 0;
                    iIndex = 0;
                    ShowIndex = 1;
                    nObjectCount = 7;//2;

                    while (iIndex < nObjectCount)
                    {
                        if (iIndex == 6)
                            isCurrentCommandOfPTRatio = true;
                        else
                            isCurrentCommandOfPTRatio = false;
                        int retn = InitializeReadMeterID(iIndex);
                        if (retn == 0x01)
                        {
                            if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                            {
                                EventLogging.CallLogDetails("Invalid Cosem FCS.");
                                gsmEventArgs.GSMLog.Status = "NC";
                                bSuccess = false;
                                break;
                            }
                            else
                            {
                                //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                                int length = 0;
                                int startIndex = 0;
                                string strTemp = "";
                                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                                {
                                    length = 2;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                                {
                                    length = 1;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                                {
                                    length = 4;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                                {
                                    length = 8;
                                    startIndex = 19;
                                }
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                if (isCurrentCommandOfPTRatio && string.IsNullOrEmpty(strTemp))
                                {
                                    wr1.WriteLine("05" + strTemp + 0x00);
                                }
                                else
                                {
                                    wr1.WriteLine("05" + strTemp);
                                }
                            }
                        }
                        else if (retn == 0x00)
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed.");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                            break;
                        }
                        else
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed.");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                            break;
                        }
                        iIndex++;
                    }

                    gsmEventArgs.IsGeneralCompleted = true;
                    EventLogging.CallLogDetails("General Data read successfully..!!");
                    gsmEventArgs.GSMLog.ErrorMessage = "General Data Read Successfully";
                    if (IsinstantaneousRequired == true || IsBillingRequired == true)
                        gsmEventArgs.GSMLog.Status = "IP";
                    else
                        gsmEventArgs.GSMLog.Status = "C";
                }
                else
                {
                    for (byte x = 0; x < 7; x++)
                        wr1.WriteLine("05");              //writing Line breaks for no data
                }
                #endregion
                //raise event here
                GSMLogCreating(this, gsmEventArgs);

                #region Instantaneous
                if (IsinstantaneousRequired)
                {
                    EventLogging.CallLogDetails("Now start reading Instantaneous data from Meter");

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadInastantaneous(3);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadInastantaneous(2);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 0);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 0);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    if (isPUMA)
                    {
                        //added PUMA
                        #region CU-MD-KW
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting Data
                        byte retval1 = fReadCumulativeKW(2);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                            }
                            else
                            {
                                // added if readout is not successful.
                                EventLogging.CallLogDetails("Cosem Connection Failed");
                                return false;
                            }
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval1 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed.");
                            bSuccess = false;
                            gsmEventArgs.GSMLog.Status = "NC";
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval1 = ReadScalarProfile(3, 4);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval1 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion

                        //added PUMA
                        #region CU-MD-KVA
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                         //for getting Data
                        byte retval2 = fReadCumulativeKVA(2);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            ///00000041_11_06_10_06_26_12
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                            }
                            else
                            {
                                // added if readout is not successful.
                                EventLogging.CallLogDetails("Cosem Connection Failed");
                                gsmEventArgs.GSMLog.Status = "NC";
                                bSuccess = false;
                                return false;
                            }
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval2 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval2 = ReadScalarProfile(3, 5);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval2 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            EventLogging.CallLogDetails("Cosem Connection Failed");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion
                    }

                    gsmEventArgs.IsInstantCompleted = true;
                    EventLogging.CallLogDetails("Instantaneous Data read successfully..!!");
                    gsmEventArgs.GSMLog.ErrorMessage = "Instantaneous Data Read Successfully";
                    if (IsBillingRequired == true && bSuccess)
                        gsmEventArgs.GSMLog.Status = "IP";
                    else
                        gsmEventArgs.GSMLog.Status = "C";
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("01");              //writing Line breaks for no data
                }
                #endregion
                //raise event here
                GSMLogCreating(this, gsmEventArgs);

                #region Billing
                if (IsBillingRequired)
                {
                    EventLogging.CallLogDetails("Now start reading Billing data from Meter");

                    ret = ReadBillingProfile(3);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        EventLogging.CallLogDetails("Access Denied.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    ret = ReadBillingProfile(2);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        EventLogging.CallLogDetails("Access Denied.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 1);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 1);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }

                    gsmEventArgs.IsBillingCompleted = true;
                    EventLogging.CallLogDetails("Billing Data read successfully..!!");
                    gsmEventArgs.GSMLog.ErrorMessage = "Billing Data Read Successfully";
                    gsmEventArgs.GSMLog.Status = "C";
                }
                else
                {
                    //writing Line breaks for no datak
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("02");
                }
                #endregion
                //raise event here
                GSMLogCreating(this, gsmEventArgs);

                #region loadSurvey
                if (IsloadSurveyRequired)
                {
                    ret = ReadLSProfile(3);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        EventLogging.CallLogDetails("Access Denied.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    //iIndex = 0;
                    ret = ReadLSProfile(2);
                    if (ret == 0x01)
                    {
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        wr1.Write("03");
                        for (int i = 0; i < length; i++)
                        {
                            //strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            wr1.Write(string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                        }
                        //wr1.WriteLine("03" + strTemp);
                        wr1.WriteLine("");
                    }
                    else if (ret == 0x05)
                    {
                        EventLogging.CallLogDetails("Access Denied.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 2);
                    if (ret == 0x01)
                    {

                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 2);
                    if (ret == 0x01)
                    {

                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("03");              //writing Line breaks for no data
                }
                #endregion

                #region EventLog Tempering
                if (IsTamperRequired)
                {

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(3, 0);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 0);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 3);
                    if (ret == 0x01)
                    {

                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalecapture = strTemp;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 3);
                    if (ret == 0x01)
                    {

                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalebuffer = strTemp;
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(3, 1);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 1);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);


                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadTamperProfile(3, 2);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 2);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(3, 3);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 3);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(3, 4);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 4);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(3, 5);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadTamperProfile(2, 5);
                    if (ret == 0x01)
                    {
                        string strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Cosem Connection Failed.");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                        return false;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);
                    strTamperScalecapture = "";
                    strTamperScalebuffer = "";

                }
                else
                {
                    for (byte x = 0; x < 24; x++)
                        wr1.WriteLine("04");              //writing Line breaks for no data
                }
                #endregion
                if (bSuccess)
                {
                    gsmEventArgs.GSMLog.Status = "C";
                    gsmEventArgs.GSMLog.ErrorMessage = "Data read successfully";
                    GSMLogCreating(this, gsmEventArgs);
                }
                else
                {
                    gsmEventArgs.GSMLog.Status = "NC";
                    gsmEventArgs.GSMLog.ErrorMessage = "Problem occurred while reading data. Most probable cause - weak signal strength.";
                    GSMLogCreating(this, gsmEventArgs);
                }
                //if the retires are completed set log_id to zero to make way for new log entry.
              
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message.ToString());
            }
            finally
            {
                if (retriesUsed >= retriesCount)
                    gsmEventArgs.Log_ID = 0;
                bool finalResult = false;
                wr1.Close();
                file1.Close();

                string strChecksum = GetMD5ChecksumForFile(strFileName);
                FileStream file2 = new FileStream(strFileName, FileMode.Append);
                StreamWriter wr2 = new StreamWriter(file2);
                wr2.WriteLine(strChecksum);
                wr2.Close();
                file2.Close();

                if (bSuccess == true)
                {
                    //This method is called for autouploding meter data into db.
                    finalResult = upload.SaveMeterData(strFileName);
                    if (finalResult)
                    {

                        EventLogging.CallLogDetails("Data saved into DB.");
                        File.Delete(strFileName);
                    }
                    else
                    {
                        EventLogging.CallLogDetails("Error occured while saving data in DB.");
                    }
                }
                else
                {

                    File.Delete(strFileName);
                }
            }
            return bSuccess;
        }
        #endregion

        #region GSM Communication
        /// <summary>
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <returns></returns>
        public string SendCommandToModem(string command)
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');

                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    return "Modem Time Out.";
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }

                    return CommandResult;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <param name="Number">Please paas the sim number to dial proper modem.</param>
        /// <returns></returns>
        private string SendCommandToModem(string command, string Number)
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                for (int i = 0; i < Number.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToByte(Number.Substring(i, 1)) + 0x30);
                }
                //MODEMCommand[MODEMIndex++] = 0X3B; //for voice call

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');



                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }

                    return CommandResult;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is used for sending description for disconnecting  to modem.
        /// </summary>
        /// <returns></returns>
        private string SendDiscription()
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    //////Application.DoEvents();
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        ///  This method is used for disconnecting from connected modem.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                GlobalObjects.objSerialComm.SetSerialPortSettings(strPortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (!GlobalObjects.objSerialComm.OpenPort())
                {
                    EventLogging.CallLogDetails(portName + " : Error while disconnecting with modem. The port is opened by other application.");
                    return;
                }
                GlobalObjects.objSerialComm.InterchatracterDelay = 5500;
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;

                //if (command == "abort")
                //{
                //    string Result = SendCommandToModem("+++");
                //    if (Result == "\r\nOK\r\n")
                //    {
                //        EventLogging.CallLogDetails("Modem disconnected successfully.");
                //    }
                //    else
                //    {
                //        GlobalObjects.objSerialComm.ClosePort();
                //        EventLogging.CallLogDetails("DLMS GSM communication stop: " + Result);
                //        return;
                //    }
                //}
                //else
                //{
                string Result = SendDiscription();

                //if (Result.Contains("OK"))
                //{
                GlobalObjects.objSerialComm.InterchatracterDelay = 5500;
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;

                Result = SendCommandToModem("ATH");

                if (Result == "\r\nOK\r\n")
                {
                    EventLogging.CallLogDetails("Modem disconnected successfully.");
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    EventLogging.CallLogDetails("DLMS GSM communication stop: " + Result);
                    return;
                }
                //}
                //else
                //{
                //GlobalObjects.objSerialComm.ClosePort();
                //EventLogging.CallLogDetails("Unable to disconnect with Modem.");
                //return;
                //}
                //}
            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails("Error while disconnecting with modem: " + ex.Message.ToString());
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                GlobalObjects.objSerialComm.flgReadFlag = false;
                GlobalObjects.objSerialComm.ClosePort();
            }
        }

        /// <summary>
        ///  This method is used for connecting to a configured dlms modem.
        /// </summary>
        /// <param name="tempsimNumber">Please paas the sim number to be dialled for communication.</param>
        public string Connect(string tempsimNumber)
        {
            string message = string.Empty;
            try
            {
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                //pick serial port from system_settings COM_PORT entry, rather than serialportsettings..
                SystemSettingsBLL objSystemSettingsBLL = new SystemSettingsBLL();
                strPortName = objSystemSettingsBLL.GetSettingValue(SystemSettings.COM_PORT);
                if (!string.IsNullOrEmpty(strPortName))
                {
                    GlobalObjects.objSerialComm.SetSerialPortSettings(strPortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                }
                else
                {
                    GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                }
                GlobalObjects.objSerialComm.OpenPort();
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;
                GlobalObjects.objSerialComm.InterchatracterDelay = 5000;
                GlobalObjects.objSerialComm.timeout = 5500;
                SerialPortSettings.Default.ModemNumber = tempsimNumber;
                SerialPortSettings.Default.Save();

                string Result = SendCommandToModem("AT");
                if (Result == "\r\nOK\r\n")
                {
                    GlobalObjects.objSerialComm.InterchatracterDelay = 35000;
                    GlobalObjects.objSerialComm.CommandTimeout = 40000;
                    GlobalObjects.objSerialComm.bCommType = 2;

                    Result = SendCommandToModem("ATD", tempsimNumber);
                    if (Result == "\r\nCONNECT 9600\r\n")
                    {
                        message = "Connected to remote modem successfully : " + tempsimNumber;
                    }
                    else if (Result == "\r\nNO CARRIER\r\n" || Result == "\r\nBUSY\r\n" || Result == "\r\nNO ANSWER\r\n")
                    {
                        message = "Unable to connect to remote modem : " + tempsimNumber;
                    }
                    else
                    {
                        message = "Unable to connect to remote modem : " + tempsimNumber;
                        GlobalObjects.objSerialComm.ClosePort();
                        return message;
                    }
                }
                else
                {
                    message = "Meter/Modem not connected";
                    GlobalObjects.objSerialComm.ClosePort();
                    return message;
                }
            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails("Not able to connect to sim number: " + tempsimNumber + ex.Message.ToString());
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                GlobalObjects.objSerialComm.ClosePort();
            }
            return message;
        }

        public bool Connect(string tempsimNumber, out string message)
        {
            message = string.Empty;
            bool status = false;
            try
            {
                strPortName = systemSettingsBLL.GetSettingValue("COM_PORT");
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                if (!string.IsNullOrEmpty(strPortName))
                {
                    GlobalObjects.objSerialComm.SetSerialPortSettings(strPortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                }
                else
                {
                    EventLogging.CallLogDetails("Please configure com port.");
                    message = "Com port is not configured.";
                    return false;
                }
                if (!GlobalObjects.objSerialComm.OpenPort())
                {
                    EventLogging.CallLogDetails(strPortName + " : Error while connecting with modem. The port is opened by some other application.");
                    message = strPortName + " : Error while connecting with modem. The port is opened by some other application.";
                    return false;

                }
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;
                GlobalObjects.objSerialComm.InterchatracterDelay = 5000;
                GlobalObjects.objSerialComm.timeout = 5500;
                SerialPortSettings.Default.ModemNumber = tempsimNumber;
                SerialPortSettings.Default.Save();

                string Result = SendCommandToModem("AT");
                if (Result == "\r\nOK\r\n")
                {
                    GlobalObjects.objSerialComm.InterchatracterDelay = 35000;
                    GlobalObjects.objSerialComm.CommandTimeout = 40000;
                    //objSerialComm.InterchatracterDelay = 60000;
                    //objSerialComm.CommandTimeout = 65000;
                    GlobalObjects.objSerialComm.bCommType = 2;

                    Result = SendCommandToModem("ATD", tempsimNumber);
                    if (Result == "\r\nCONNECT 9600\r\n")
                    {

                        message = "Connected to remote modem successfully.";
                        status = true;
                    }
                    else if (Result == "\r\nNO CARRIER\r\n" || Result == "\r\nBUSY\r\n" || Result == "\r\nNO ANSWER\r\n")
                    {
                        message = "Unable to connect to remote modem : " + tempsimNumber;
                        return false;
                    }
                    // if the error comes in modem irt needs to get rebooted.. 
                    else if (Result == "\r\nERROR\r\n")
                    {
                        message = "Error occured while connecting Remote Modem : " + tempsimNumber;
                        Disconnect();
                        //EventLogging.CallLogDetails("Rebooting local modem..");
                        //SendCommandToModem("AT+cfun=1");
                        //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                        //Thread.Sleep(10000);
                        return false;
                    }
                    else if (Result == "Modem Time Out.")
                    {
                        message = "Remote modem time out: " + tempsimNumber;
                        Disconnect();
                        //EventLogging.CallLogDetails("Rebooting local modem..");
                        //RebootModem();
                        //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                        //Thread.Sleep(10000);
                        return false;
                    }
                    else
                    {
                        message = "Unable to connect to remote modem : " + tempsimNumber;
                        GlobalObjects.objSerialComm.ClosePort();
                        return false;
                    }
                }
                else if (Result == "Modem Time Out.")
                {
                    message = "Local modem time out";
                    Disconnect();
                    //EventLogging.CallLogDetails("Rebooting local modem..");
                    //RebootModem();
                    //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                    //Thread.Sleep(10000);
                    return false;
                }
                else if (Result == "\r\nERROR\r\n")
                {
                    message = "Error occured while connecting local modem";
                    Disconnect();
                    //EventLogging.CallLogDetails("Rebooting local modem..");
                    //SendCommandToModem("AT+cfun=1");
                    //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                    //Thread.Sleep(10000);
                    return false;
                }
                else
                {
                    message = "Local modem not connected";
                    GlobalObjects.objSerialComm.ClosePort();
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
                EventLogging.CallLogDetails("Not able to connect to sim number: " + tempsimNumber + ex.Message.ToString());
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                GlobalObjects.objSerialComm.ClosePort();
            }
            return status;
        }
        public void RebootModem()
        {


            try
            {
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                //pick serial port from system_settings COM_PORT entry, rather than serialportsettings..
                if (!string.IsNullOrEmpty(strPortName))
                {
                    GlobalObjects.objSerialComm.SetSerialPortSettings(strPortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                }
                else
                {
                    EventLogging.CallLogDetails("Please configure com port");
                    return;
                }
            
                if (!GlobalObjects.objSerialComm.OpenPort())
                {
                    EventLogging.CallLogDetails(strPortName + " : Error while rebooting modem. The port is opened by some other application.");
                    return;
                }
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;
                GlobalObjects.objSerialComm.InterchatracterDelay = 5000;
                GlobalObjects.objSerialComm.timeout = 5500;
                SerialPortSettings.Default.Save();

                SendCommandToModem("AT+cfun=1");


            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                GlobalObjects.objSerialComm.ClosePort();
            }

        }
        #endregion
    }
}
