#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABCommunication.PhysicalLayer;
using CABCommunication.Common;
using CABCommunication.WrapperLayer;
using CAB.Entity;
using CAB.Parser;
using System.IO;
using CAB.Framework;
using CAB.Serialization;
using System.Security.Cryptography;
using CAB.IECChannel;
using CAB.IECChannel.ReadOut;
using CAB.Framework.Utility;
using CAB.BLL;
using Utilities;
using System.Threading;
using CAB.MeterData.Upload;
using System.IO.Ports;
using System.Net;

#endregion
namespace DLMSGSMCommunication
{
    /// <summary>
    /// The class will take of remote communication used by worker thread
    /// </summary>
    public class RemoteCommunication
    {
        #region Nested Types
        #endregion

        #region Constants and Variables        
        //private static Semaphore SMPool= new Semaphore(1,3);//For thread wait
        //Mutex mu = new Mutex();
        public int securitymachanism = 0;
        private Serial objSerialComm = null;
        private string simNumber = string.Empty;
        private string ChannelType = string.Empty;
        private byte NoOfRetries = 0;
        private string modemIMEINumber = string.Empty;
        private CommunicationType communicationType;
        private Communication communication = null;
        private const string Splitter = "$";
        private static List<ProfileCommand> lstProfileCommands = null;
        private int noOfDays = 30;
        private int logID = 0;
        private GSMLoggingBLL logBLL = null;
        private UploadFile uploadFile = null;
        private string meterSerialNumber = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the port name assocated with the worker thread
        /// </summary>
        public string ComPortName
        {
            get
            {
                return objSerialComm.PortName;
            }
        }
        /// <summary>
        /// Gets the sim numbers associated with worker thread.
        /// </summary>
        public string SimNumber
        {
            get
            {
                return simNumber;
            }
        }
        #endregion       

        #region Constructor
        public RemoteCommunication(Serial serial, string phoneNumber, int loggingID)
        {
            objSerialComm = serial;
            simNumber = phoneNumber;
            ChannelType = serial.channelType.ToString();
            NoOfRetries = serial.noOfRetry;
            communication = new Communication(objSerialComm, 0x01, "11111111");
            this.logID = loggingID;
            logBLL = new GSMLoggingBLL();
            uploadFile = new UploadFile();
        }

        public RemoteCommunication(ChannelInformation channelInfo, int loggingID, string imeiNumber)
        {
            communicationType = (CommunicationType)System.Enum.Parse(typeof(CommunicationType), channelInfo.CommunicationMode);
            modemIMEINumber = imeiNumber;
            communication = new Communication(channelInfo);
            this.logID = loggingID;
            logBLL = new GSMLoggingBLL();
            uploadFile = new UploadFile();
        }
        static RemoteCommunication()
        {
            try
            {
                DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "CommandRepository.xml", typeof(DLMS));
                lstProfileCommands = new List<ProfileCommand>();
                ProfileCommand profileCommandEntity;
                foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
                {
                    profileCommandEntity = new ProfileCommand();
                    profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                    profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                    profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                    profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                    profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                    lstProfileCommands.Add(profileCommandEntity);
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("Error In Static Constructor of Remote Communication"
                 + ex.Message + " : " + ex.StackTrace + " : " + ex.Source);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Read the selected profiles from the already connected meter , used for DLMS communication
        /// </summary>
        /// <param name="selectedProfiles"></param>
        /// <param name="gsmLog"></param>
        /// <returns></returns>
        public Result Send(GSMTaskEntity gsmTask, IList<ProfileId> selectedProfiles, GSMLoggingEntity gsmLog)
        {
            string data = string.Empty;
            string idLength = string.Empty;
            int index = 0;
            string lngFilename = string.Empty;
            String fileMeterData;
            string classIdOBISCodeAttribute = string.Empty;
            int meterModelNumber = 0;
            string serialPortOrModemIMEI = string.Empty;
            switch (communicationType)
            {
                case CommunicationType.GPRS:
                    serialPortOrModemIMEI = modemIMEINumber;
                    break;
                case CommunicationType.TCP:
                    serialPortOrModemIMEI = modemIMEINumber;
                    break;

                default:
                    serialPortOrModemIMEI = objSerialComm.PortName;
                    break;
            }

            string firmwareVersion = string.Empty;
            List<byte> meterId = null;
            int meterType = 0;
            Result result = null;
            String strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");
            string ProfileTypeLog = string.Empty;
            try
            {
                if (!Directory.Exists(strFileName))
                {
                    Directory.CreateDirectory(strFileName);
                }
                // ************ Read meter ID Start****************
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                result = communication.Send(profileCommand);
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                    {
                        data = string.Empty;
                        idLength = result.RecieveDataBuffer[1].ToString("00");
                        index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        meterId = new List<byte>();
                        meterId = result.RecieveDataBuffer.GetRange(2, index);
                        for (int i = 2; i < index + 2; i++)
                        {
                            data += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();
                        }
                        //******************** Show message at communication time************
                        gsmLog.ErrorMessage = "Device ID Received";
                        GSMLogCreating(gsmLog);
                        // ************Read meter ID Close****************

                        // ************Read Invocation Counter start
                        profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
                        if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                        {
                            if (ConfigSettings.GetValue("SecurityMechanism") == "01")//Smart meter read for LLS mode
                                profileCommand = new ProfileCommand(01, "00.00.2B.01.02.FF", 02);//For MR mode LLS
                            else if (ConfigSettings.GetValue("SecurityMechanism") == "02")//Smart meter read for HLS mode
                                profileCommand = new ProfileCommand(01, "00.00.2B.01.03.FF", 02);//For US mode HLS not use in BCS
                        }

                        result = communication.Send(profileCommand);//Read here invocation counter and close session
                        communication.CloseSession();
                        if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                        {
                            index = 0;
                            long InvoCountValue = 0;
                            long InitializationCounter = 0;
                            byte[] incount = new byte[4];

                            if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                            {
                                ChannelInformation channelInfo = new ChannelInformation();
                                // channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.CommunicationMode = communicationType.ToString();
                                if (ConfigSettings.GetValue("PortName").Contains(","))
                                    channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
                                else
                                    channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                                channelInfo.ModemInfo = serialPortOrModemIMEI;
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;            
                                channelInfo.TcpPort = ConfigSettings.GetValue("TCPPORT");
                                communication = new Communication(channelInfo);
                                securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                // this.meterEntity = meterMasterEntity;
                                data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                                int adbyteindex = 0;
                                for (int i = 1; i < index + 5; i++)
                                {
                                    incount[adbyteindex++] = result.RecieveDataBuffer[i];

                                }
                                //******************** Show message at communication time************
                                gsmLog.ErrorMessage = "Invocation Counter Received.";
                                GSMLogCreating(gsmLog);
                                data = FormatData(incount, false);
                                if (data != null) InvoCountValue = Convert.ToInt64(data);
                                InitializationCounter = InvoCountValue + 1;
                                // ************Read Invocation Counter Close****************

                                result = communication.OpenSessionCipher(InitializationCounter);//Open session in ciphering mode

                            }
                        }

                        if (ConfigSettings.GetValue("ApplicationContext") != "03")
                            result = communication.OpenSession();
                        if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                        {
                            profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                            result = communication.Send(profileCommand);
                            if (result != null && result.ErrorCode == CommunicationErrorType.Success)
                            {
                                if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                                {
                                    if (result.RecieveDataBuffer[0] != 0x06)
                                    {

                                        idLength = result.RecieveDataBuffer[1].ToString("00");
                                        index = Convert.ToInt16(result.RecieveDataBuffer[1]);

                                        meterId = new List<byte>();
                                        meterId = result.RecieveDataBuffer.GetRange(2, index);
                                        for (int i = 2; i < index + 2; i++)
                                        {
                                            data += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();
                                        }

                                        //******************** Show message at communication time************
                                        gsmLog.ErrorMessage = "Device ID Received.";
                                        GSMLogCreating(gsmLog);
                                    }

                                }
                            }

                        }

                        #region CreateResultFile
                        meterSerialNumber = data;
                        //Application.DoEvents();
                        //this..Text = "Meter Serial Number : " + meterSerialNumber;
                        strFileName = strFileName + data;
                        //strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                        strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";// Story - 427028 - file format should be same for DLMS and IEC
                        fileMeterData = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);

                        FileStream initialFileStream = new FileStream(strFileName, FileMode.Create);
                        StreamWriter writeToFile = new StreamWriter(initialFileStream);
                        writeToFile.WriteLine(Splitter);
                        writeToFile.WriteLine(meterSerialNumber.Length.ToString("00"));
                        writeToFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        #endregion

                        DateTime meterTime = communication.GetMeterDateTime();
                        //******************** Show message at communication time************
                        gsmLog.ErrorMessage = "Device RTC Received.";
                        GSMLogCreating(gsmLog);
                        string signatureInfo = communication.GetSignatureData();
                        //******************** Show message at communication time************
                        gsmLog.ErrorMessage = "Signature command Received.";
                        GSMLogCreating(gsmLog);
                        if (signatureInfo.ToUpper() == "**2.21240010060WC4RSL")
                        {
                            meterType = communication.GetMeterType();
                            firmwareVersion = signatureInfo.Substring(0, 6).Trim('*');

                        }
                        else if (signatureInfo.ToUpper().IndexOf("FS") >= 0)
                        {
                            meterType = communication.GetMeterType();
                            firmwareVersion = signatureInfo.Substring(0, 7).Trim('#');  // for 1P smart meter
                        }
                        else
                        {
                            firmwareVersion = signatureInfo.Substring(0, 6).Trim('*');
                        }

                        if (signatureInfo.ToUpper().Contains("WC"))
                        {
                            if (meterType == 5)
                            {
                                meterModelNumber = NamePlateConstants.RubyE350Value;
                            }
                            else
                            {
                                meterModelNumber = NamePlateConstants.RubyE250Value;
                            }
                        }
                        else if (signatureInfo.Contains("SM0310"))
                        {
                            meterModelNumber = NamePlateConstants.SmartM_Cipher_WCM;
                            firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                        }
                        else if (signatureInfo.Contains("SM0405"))
                        {
                            meterModelNumber = NamePlateConstants.SmartM_Cipher_LTCT;
                            firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                        }
                        else if (signatureInfo.Contains("SM0110"))
                        {
                            meterModelNumber = NamePlateConstants.SmartM_Cipher_1PH;
                            firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                        }
                        else if (signatureInfo.Contains("lt"))
                        {
                            meterModelNumber = NamePlateConstants.TwoTOUltModelValue;
                        }
                        else if ((signatureInfo.Contains("LT")))
                        {
                            meterModelNumber = NamePlateConstants.PumaLTE650Value;
                        }
                        else if (signatureInfo.Contains("ST"))
                        {
                            meterModelNumber = NamePlateConstants.SapphireLTCT;
                        }

                        else if (signatureInfo.Contains("L0"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_Netmeter_LTCT;
                        }
                        else if (signatureInfo.Contains("st"))
                        {
                            meterModelNumber = NamePlateConstants.SapphireLTCT_st;
                        }
                        else if (signatureInfo.ToUpper().Contains("HT") || signatureInfo.ToUpper().Contains("HK"))
                        {
                            meterModelNumber = NamePlateConstants.PumaHTE650Value;
                        }
                        else if (signatureInfo.ToUpper().Contains("HK"))
                        {
                            meterModelNumber = NamePlateConstants.PumaHTE650Value;
                        }
                        else if (signatureInfo.ToUpper().Contains("LC"))
                        {
                            meterModelNumber = NamePlateConstants.LTCTCortexValue;
                        }
                        else if (signatureInfo.Contains("uk"))
                        {
                            meterModelNumber = NamePlateConstants.Ruby6ukModelValue;
                        }
                        else if (signatureInfo.Contains("UK"))
                        {
                            meterModelNumber = NamePlateConstants.Ruby6Value;
                        }
                        else if (signatureInfo.ToUpper().Contains("WB"))
                        {
                            meterModelNumber = NamePlateConstants.WBValue;
                        }
                        else if (signatureInfo.ToUpper().Contains("BW"))
                        {
                            meterModelNumber = NamePlateConstants.WBLTValue;
                        }
                        else if (signatureInfo.ToUpper().Contains("SK"))
                        {
                            meterModelNumber = NamePlateConstants.RubyE150Value;
                        }
                        else if (signatureInfo.ToUpper().Contains("SF"))
                        {
                            meterModelNumber = NamePlateConstants.SFSP;
                        }
                        else if (signatureInfo.ToUpper().Contains("HM"))
                        {
                            meterModelNumber = NamePlateConstants.PumaHTE650MWValue;
                        }
                        //---Rohit-----------03-March-2016------- for UPCL-----TwoSeason------
                        else if (signatureInfo.Contains("sc"))
                        {
                            meterModelNumber = NamePlateConstants.TwoTOUSapphireValue;
                        }
                        else if (signatureInfo.Contains("SC"))
                        {
                            meterModelNumber = NamePlateConstants.SapphireValue;
                        }

                        else if (signatureInfo.Contains("W0"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_Netmeter_WCM;
                        }
                        //---Rohit-----------21-March-2016------- for VB---1p DLMS--No Season-No Week-----
                        //SarkarA code change start 20180122 // Add valid meter model name for model 33 "Sc"
                        else if (signatureInfo.Contains("Sc"))
                        {
                            meterModelNumber = NamePlateConstants.ThreeTOUWCMValue;
                        }
                        //SarkarA code change end 20180122
                        else if (signatureInfo.Contains("VB"))
                        {
                            meterModelNumber = NamePlateConstants.VBSPNoSeasonNoWeek;
                        }
                        //******* Meter Model Change Required Here ***********//
                        else if (signatureInfo.ToUpper().Contains("VF"))
                        {
                            meterModelNumber = NamePlateConstants.VFSPNoSeasonNoWeek;
                        }
                        else if (signatureInfo.ToUpper().Contains("TN"))
                        {
                            meterModelNumber = NamePlateConstants.TNValue;
                        }
                        else if (signatureInfo.ToUpper().Contains("FS")) // single phase smart meter
                        {
                            meterModelNumber = NamePlateConstants.SM110value;
                        }
                        //******* Smart meter 3 phase WCM  ***********//
                        else if (signatureInfo.ToUpper().Contains("FU"))
                        {
                            meterModelNumber = NamePlateConstants.Smartmeter_WCM;
                        }
                        //******* Smart meter 3 phase LTCT Falcon ***********//
                        else if (signatureInfo.ToUpper().Contains("FL"))
                        {
                            meterModelNumber = NamePlateConstants.Smartmeter_LTCT;
                        }
                        //******* Smart meter 3 phase HTCT ***********//
                        else if (signatureInfo.ToUpper().Contains("FH"))
                        {
                            meterModelNumber = NamePlateConstants.Smartmeter_HTCT;
                        }
                        //*******Sapphire 3 phase HTCT ***********//
                        else if (signatureInfo.Contains("sm"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_sm;
                        }
                        //******* Sapphire 3 phase HTCT ***********//
                        else if (signatureInfo.Contains("SM"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_SM;
                        }

                        //*******Sapphire 3 phase HTCT ***********//
                        else if (signatureInfo.Contains("sh"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_sh;
                        }
                        //******* Sapphire 3 phase HTCT ***********//
                        else if (signatureInfo.Contains("SH"))
                        {
                            meterModelNumber = NamePlateConstants.Sapphire_SH;
                        }
                        //******* Sapphire S2 meter ***********//
                        else if (signatureInfo.Contains("SPS2"))
                        {
                            //meterModelNumber = GenericRTC.getMeterModel_S2(signatureInfo);
                            // meterModelNumber = NamePlateConstants.SapphireS2;
                        }

                        //*******Vim series 2 meter ***********//
                        else if (signatureInfo.Contains("vb"))
                        {
                            meterModelNumber = NamePlateConstants.VIM_Series2;
                        }
                        else if (signatureInfo.Contains("BF"))
                        {
                            meterModelNumber = NamePlateConstants.BYPL_7Slot;
                        }
                        else if (signatureInfo.Contains("RF"))
                        {
                            meterModelNumber = NamePlateConstants.BRPL_7Slot;
                        }
                        else if (signatureInfo.Contains("CF"))
                        {
                            meterModelNumber = NamePlateConstants.BYPL_FD;
                        }
                        else if (signatureInfo.Contains("CB"))  //user story 1016689
                        {
                            meterModelNumber = NamePlateConstants.BRPL_CBSP;
                        }
                        else
                        {
                            meterModelNumber = NamePlateConstants.InvalidModelValue;
                        }

                        writeToFile.WriteLine(GetSignatureDataInFileFormat(signatureInfo));

                        #region ReadProfileAndWriteToFile


                        foreach (ProfileId selectedProfile in selectedProfiles)
                        {
                            ProfileTypeLog = selectedProfile.ToString();
                            EventLogging.CallLogDetails(serialPortOrModemIMEI + ":" + CommonBLL.GetEnumDescription(selectedProfile) + " started.");
                            gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(selectedProfile) + " is in progress...";
                            gsmLog.Status = "IP";
                            GSMLogCreating(gsmLog);
                            List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate (ProfileCommand profileCommandEntity)
                            {
                                return profileCommandEntity.TagNumber == (int)selectedProfile
                                && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                                && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                profileCommandEntity.MeterModelNumber == 0);
                                //}
                            });

                            if (selectedProfile == ProfileId.LoadSurvey && meterTime > DateTime.MinValue)
                            {
                                profileReadCommands[1] = GetLoadSurveyProfileCommand(profileReadCommands[1], gsmTask, meterTime);
                            }
                            for (index = 0; index < profileReadCommands.Count; index++)
                            {

                                profileReadCommands[index].Action = ActionType.READ;
                                profileReadCommands[index].MeterID = meterId;
                                result = communication.Send(profileReadCommands[index]);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {

                                    classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                        + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                               + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                    if (result.RecieveDataLength <= 0)
                                    {
                                        if (selectedProfile == ProfileId.Tamper)
                                        {
                                            writeToFile.WriteLine(classIdOBISCodeAttribute + "0100");
                                        }
                                    }
                                    else
                                    {
                                        writeToFile.Write(classIdOBISCodeAttribute);
                                        for (int i = 0; i < result.RecieveDataLength; i++)
                                        {
                                            writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                        }
                                        writeToFile.WriteLine("");
                                    }

                                }
                                else
                                {
                                    break;
                                }


                            }
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                UpdateStatus(selectedProfile, gsmLog);
                                EventLogging.CallLogDetails(serialPortOrModemIMEI + ":" + selectedProfile.ToString() + " Read Completed.");
                            }
                            else
                            {
                                gsmLog.Status = "NC";
                                gsmLog.ErrorMessage = "Data read Failed.";
                                EventLogging.CallLogDetails(serialPortOrModemIMEI + ":" + selectedProfile.ToString() + " Read Failed.");
                                GSMLogCreating(gsmLog);
                                break;
                            }
                        }

                        #endregion

                        #region ResourceClosingAndCleanup
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            gsmLog.Status = "C";
                            gsmLog.ErrorMessage = "Data read successfully.";
                            GSMLogCreating(gsmLog);
                            //communication.CloseSession();
                            writeToFile.Close();
                            initialFileStream.Close();

                            String strChecksum = GetMD5ChecksumForFile(strFileName);
                            FileStream fileStream = new FileStream(strFileName, FileMode.Append);
                            StreamWriter writeStream = new StreamWriter(fileStream);
                            writeStream.WriteLine(strChecksum);
                            writeStream.Close();
                            fileStream.Close();
                            StreamReader streamReader = new StreamReader(strFileName);
                            string fileText = streamReader.ReadToEnd();
                            streamReader.Close();
                            //SMPool.WaitOne();
                            EventLogging.CallLogDetails("File Created " + strFileName);
                            //string resultMessage;
                            //if (uploadFile.Upload2NGFile(strFileName, fileText, true, out resultMessage, null))
                            //{
                            //    EventLogging.CallLogDetails("Data uploaded successfully.");                                
                            //}
                            //else
                            //{
                            //    EventLogging.CallLogDetails("Error occured while uploading data.");
                            //}

                        }
                        else
                        {
                            writeToFile.Close();
                            initialFileStream.Close();
                            File.Delete(strFileName);
                        }
                        #endregion
                    }
                    else
                    {
                        result.ErrorCode = CommunicationErrorType.Nothing;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(serialPortOrModemIMEI + ":" + ProfileTypeLog + ex.ToString());
            }
            finally
            {
                //if (result.ErrorCode == CommunicationErrorType.Success)
                //{                
                communication.CloseRemoteSession();
                //}
                if (objSerialComm != null)
                {
                    objSerialComm.CloseRemoteSession();
                }
                // SMPool.Release();
            }
            return result;
        }

        /// <summary>
        /// Reads the already connecetd according to the task saved, used for Non-DLMS meter
        /// </summary>
        /// <param name="gsmTask"></param>
        /// <returns></returns>
        public Result Send(GSMTaskEntity gsmTask, GSMLoggingEntity gsmLog, bool isSPNONDLMS)
        {
            string generalData = string.Empty;
            string transactionData = string.Empty;
            string tamperData = string.Empty;
            string phasorData = string.Empty;
            string dtmParameterData = string.Empty;
            string fraudEnergyData = string.Empty;
            string dTMDailyProfileData = string.Empty;
            string loadSurveyData = string.Empty;
            CAB.IECChannel.ReadBase readOut = null;
            Result result = new Result();
            string readingDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");

            IECChannelBase channelBase = ChannelManager.GetChannel() as IECLocalCommunication;

            //IECChannelBase channelBase = new CAB.IECChannel.GSMCommunication();

            channelBase.SetPort(objSerialComm);
            channelBase.SimNumber = this.simNumber;
            channelBase.ChannelType = this.ChannelType;
            channelBase.noOfRetry = this.NoOfRetries;
            if (isSPNONDLMS)
            {
                channelBase.Parity = System.IO.Ports.Parity.None;
                channelBase.DataBits = 8;
            }
            else
            {
                channelBase.Parity = System.IO.Ports.Parity.Even;
                channelBase.DataBits = 7;
            }


            bool success = false;
            try
            {
                #region General Data
                if (isSPNONDLMS)
                    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + "General, Billing and Instant" + " started.");
                else
                    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.NamePlate) + " started.");
                gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.NamePlate) + " is in progress...";
                gsmLog.Status = "IP";
                GSMLogCreating(gsmLog);
                if (isSPNONDLMS)
                    readOut = new ReadoutGeneralForSingllePhaseIEC();
                else
                    readOut = new CAB.IECChannel.ReadOut.ReadoutGeneral();

                //readOut = new CAB.IECChannel.ReadOut.ReadoutGeneral();
                //readOut.OnChannelStatusChanged += new ReadoutGeneral.ChannelStatusChanged(Channel_OnStatusChanged);
                readOut.Channel = channelBase;
                readOut.ReadingDateTime = readingDateTime;
                generalData = readOut.GetData();
                if (string.IsNullOrEmpty(generalData) ||
                    (!string.IsNullOrEmpty(generalData) && generalData.Length < 5) || readOut.IsCorruptedData)
                {
                    success = false;
                    generalData = string.Empty;
                    gsmLog.Status = "NC";
                    gsmLog.ErrorMessage = "Data read failure.";
                    GSMLogCreating(gsmLog);

                }
                else
                {
                    success = true;
                    gsmLog.Status = "IP";
                    gsmLog.IsGeneralCompleted = true;
                    gsmLog.IsInstantCompleted = true;
                    gsmLog.IsBillingCompleted = true;
                    gsmLog.ErrorMessage = "General data completed";
                    GSMLogCreating(gsmLog);
                    if (isSPNONDLMS)
                        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + "General, Billing and Instant" + " Read Completed.");
                    else
                        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.NamePlate) + " Read Completed.");
                }
                #endregion

                #region Daily Load

                if (success && gsmTask.IsMidnightRequired)
                {
                    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Midnight) + " started.");
                    gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.Midnight) + " is in progress...";
                    gsmLog.Status = "IP";
                    GSMLogCreating(gsmLog);
                    if (isSPNONDLMS)
                    {
                        readOut = new ReadoutDTMDailyProfileForSingllePhaseIEC(false);

                    }
                    else
                        readOut = new ReadoutDTMDailyProfile(false);
                    //readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfile.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = channelBase;
                    readOut.ReadingDateTime = readingDateTime;
                    dtmParameterData = readOut.GetDTMParameterData();
                    if (dtmParameterData.Trim() != string.Empty && !readOut.IsCorruptedData)
                    {
                        if (isSPNONDLMS)
                        {
                            if (dtmParameterData.Length >= 24)
                            {
                                readOut.ReadingDateTime = readingDateTime;
                                dTMDailyProfileData = string.Concat(Convert.ToChar(1), ReadoutConstant.DTMDAILYPROFILE, "/", ((readOut.MeterID(channelBase.ResponseSignOn)).Split('/'))[1], "/", readingDateTime, "/", dtmParameterData, dTMDailyProfileData, Convert.ToChar(4));

                                UpdateStatus(ProfileId.Midnight, gsmLog);
                                //EventLogging.CallLogDetails(objSerialComm.PortName + ":" + ProfileId.Midnight.ToString() + " Read Completed.");
                                EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Midnight) + " Read Completed.");
                            }
                            else
                            {
                                dTMDailyProfileData = string.Empty;
                                success = false;
                                gsmLog.Status = "NC";
                                gsmLog.ErrorMessage = "Data read failure.";
                                GSMLogCreating(gsmLog);
                            }
                        }
                        else
                        {
                            if (dtmParameterData.Length >= 27)
                            {
                                readOut.ReadingDateTime = readingDateTime;
                                dTMDailyProfileData = readOut.GetData();
                                if (dTMDailyProfileData.Trim() != string.Empty)
                                {
                                    if (dTMDailyProfileData.Length >= 15)
                                    {
                                        dTMDailyProfileData = string.Concat(Convert.ToChar(1),
                                            ReadoutConstant.DTMDAILYPROFILE, readOut.MeterID(channelBase.ResponseSignOn)
                                            , "/", readingDateTime, dtmParameterData, dTMDailyProfileData,
                                            Convert.ToChar(4));
                                        UpdateStatus(ProfileId.Midnight, gsmLog);
                                        //EventLogging.CallLogDetails(objSerialComm.PortName + ":" + ProfileId.Midnight.ToString() + " Read Completed.");
                                        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Midnight) + " Read Completed.");
                                    }
                                    else
                                    {
                                        dTMDailyProfileData = string.Empty;
                                        success = false;
                                        gsmLog.Status = "NC";
                                        gsmLog.ErrorMessage = "Data read failure.";
                                        GSMLogCreating(gsmLog);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Load Survey
                if (success && gsmTask.IsLoadSurveyRequired)
                {
                    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.LoadSurvey) + " started.");
                    gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.LoadSurvey) + " is in progress...";
                    gsmLog.Status = "IP";
                    GSMLogCreating(gsmLog);
                    success = false;
                    int totalDay = 0;
                    if (isSPNONDLMS)
                    {
                        totalDay = 30;
                        readOut = new ReadoutDTMLoadSurveyForSingllePhaseIEC();
                        readOut.Channel = channelBase;
                        readOut.ReadingDateTime = readingDateTime;
                        //loadSurveyData = ((ReadoutDTMLoadSurveyForSingllePhaseIEC)readOut).GetData(totalDay.ToString(), totalDay);
                        loadSurveyData = ((ReadoutDTMLoadSurveyForSingllePhaseIEC)readOut).GetData(ConfigSettings.GetValue("LoadSurveyDays"), 90);

                        if (loadSurveyData.Length >= 15)
                            loadSurveyData = Convert.ToChar(1) + ReadoutConstant.DTMPROFILE + "/" + ((readOut.MeterID(channelBase.ResponseSignOn)).Split('/'))[1] + "/" + readingDateTime + "/" + loadSurveyData + Convert.ToChar(4); //responseForLoadSurvey
                        if (!string.IsNullOrEmpty(loadSurveyData))
                        {
                            success = true;
                        }
                    }
                    else
                    {
                        CAB.IECChannel.ReadOut.ReadoutDTMLoadSurvey readoutDTMLoadSurvey = new CAB.IECChannel.ReadOut.ReadoutDTMLoadSurvey();
                        //readoutDTMLoadSurvey.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
                        readoutDTMLoadSurvey.Channel = channelBase;
                        string data = readoutDTMLoadSurvey.LoadDTMDay();
                        if (data.Length >= 5)
                        {
                            string noofdays = data.Substring(21, 2);
                            totalDay = Convert.ToInt32(noofdays, 16);
                            totalDay = totalDay > 30 ? 30 : totalDay;
                        }
                        else
                        {
                            data = string.Empty;
                        }
                        readOut = new ReadoutDTMLoadSurvey();
                        //readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
                        readOut.Channel = channelBase;
                        readOut.ReadingDateTime = readingDateTime;
                        if (totalDay > 0)
                        {

                            loadSurveyData = ((ReadoutDTMLoadSurvey)readOut).GetData(totalDay.ToString(), totalDay);
                            if (!string.IsNullOrEmpty(loadSurveyData))
                            {
                                success = true;

                            }

                        }
                    }
                    if (success)
                    {
                        UpdateStatus(ProfileId.LoadSurvey, gsmLog);
                        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.LoadSurvey) + " Read Completed.");
                    }
                    else
                    {
                        success = false;
                        gsmLog.Status = "NC";
                        gsmLog.ErrorMessage = "Data read failure.";
                        GSMLogCreating(gsmLog);
                    }
                }
                #endregion

                #region Tamper
                if (success && gsmTask.IsTamperRequired)
                {
                    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Tamper) + " started.");
                    gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.Tamper) + " is in progress...";
                    gsmLog.Status = "IP";
                    GSMLogCreating(gsmLog);
                    if (isSPNONDLMS)
                        readOut = new ReadoutTamperForSingllePhaseIEC();
                    else
                        readOut = new ReadoutTamper();
                    //readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = channelBase;
                    //readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    tamperData = readOut.GetData();
                    if (isSPNONDLMS)
                    {
                        if (tamperData.Length >= 60)
                        {
                            tamperData = Convert.ToChar(1) + "TM" + "/" + ((readOut.MeterID(channelBase.ResponseSignOn)).Split('/'))[1] + "/" + readingDateTime + "/" + tamperData + Convert.ToChar(4);
                        }
                    }
                    if (((!string.IsNullOrEmpty(tamperData)) && tamperData.Length < 5) || readOut.IsCorruptedData)
                    {
                        tamperData = string.Empty;
                        success = false;
                        gsmLog.Status = "NC";
                        gsmLog.ErrorMessage = "Data read failure.";
                        GSMLogCreating(gsmLog);
                    }
                    else
                    {
                        success = true;
                        UpdateStatus(ProfileId.Tamper, gsmLog);
                        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Tamper) + " Read Completed.");

                    }
                }
                #endregion

                #region Transaction
                ////readOut = new ReadoutTransaction();
                ////readOut.OnChannelStatusChanged += new ReadoutTransaction.ChannelStatusChanged(Channel_OnStatusChanged);
                ////readOut.Channel = channelBase;
                ////readOut.IsAborted = IsAborted;
                ////readOut.ReadingDateTime = readingDateTime;
                ////transactionData = readOut.GetData();
                ////Thread.Sleep(500);
                ////if (channelBase.ResponseSignOn != string.Empty && transactionData.Length >= 313 && !readOut.IsCorruptedData)
                ////{
                ////    string response = readOut.MeterID(channelBase.ResponseSignOn);
                ////    string strtemptr = Convert.ToChar(4).ToString() + Convert.ToChar(1).ToString() + ReadoutConstant.REGISTER + response + "/" + readingDateTime;
                ////    transactionData = transactionData.Replace(ReadoutConstant.TRANSACTION, strtemptr);
                ////    transactionData = Convert.ToChar(1) + ReadoutConstant.TAMPER + response + "/" + readingDateTime + transactionData + Convert.ToChar(4);
                ////    transactionData = transactionData.Replace("<RTC>", "");
                ////}
                ////else
                ////{
                ////    transactionData = string.Empty;
                ////}

                //#endregion
                //#region Phasor
                ////if (success)
                ////{
                ////    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.Phasor) + " started");
                ////    gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.Phasor) + " is in progress...";
                ////    gsmLog.Status = "IP";
                ////    GSMLogCreating(gsmLog);
                ////    readOut = new ReadoutPhasor();
                ////    //readOut.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
                ////    readOut.Channel = channelBase;
                ////    //readOut.IsAborted = IsAborted;
                ////    readOut.ReadingDateTime = readingDateTime;
                ////    phasorData = readOut.GetData();
                ////    if ((!string.IsNullOrEmpty(phasorData)) && phasorData.Length >= 93 && !readOut.IsCorruptedData)
                ////    {

                ////        phasorData = Convert.ToChar(1) + ReadoutConstant.PHASOR + readOut.MeterID(channelBase.ResponseSignOn) + "/" + readingDateTime + phasorData + Convert.ToChar(4);
                ////        UpdateStatus(ProfileId.Tamper, gsmLog);
                ////        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + ProfileId.Phasor.ToString() + " Read Completed.");
                ////        success = true;
                ////    }
                ////    else
                ////    {
                ////        gsmLog.Status = "NC";
                ////        gsmLog.ErrorMessage = "Data read failure.";
                ////        GSMLogCreating(gsmLog);                       
                ////        success = false;
                ////    }
                ////}
                //#endregion
                //#region Fraud Energy
                ////if (success)
                ////{
                ////    EventLogging.CallLogDetails(objSerialComm.PortName + ":" + CommonBLL.GetEnumDescription(ProfileId.FraudEnergy) + " started");
                ////    gsmLog.ErrorMessage = CommonBLL.GetEnumDescription(ProfileId.FraudEnergy) + " is in progress...";
                ////    gsmLog.Status = "IP";
                ////    GSMLogCreating(gsmLog);
                ////    readOut = new ReadoutFraudEnergy();
                ////    //readOut.OnChannelStatusChanged += new ReadoutFraudEnergy.ChannelStatusChanged(Channel_OnStatusChanged);
                ////    readOut.Channel = channelBase;
                ////    //readOut.IsAborted = IsAborted;
                ////    readOut.ReadingDateTime = readingDateTime;
                ////    fraudEnergyData = readOut.GetData();
                ////    if (fraudEnergyData != string.Empty && !readOut.IsCorruptedData)
                ////    {
                ////        UpdateStatus(ProfileId.FraudEnergy, gsmLog);
                ////        EventLogging.CallLogDetails(objSerialComm.PortName + ":" + ProfileId.FraudEnergy.ToString() + " Read Completed.");
                ////        success = true;
                ////        fraudEnergyData = string.Concat(Convert.ToChar(1), ReadoutConstant.MAGNETICINFLUENCE, readOut.MeterID(channelBase.ResponseSignOn), "/", readingDateTime, fraudEnergyData);
                ////    }
                ////    else
                ////    {
                ////        gsmLog.Status = "NC";
                ////        gsmLog.ErrorMessage = "Data read failure.";
                ////        GSMLogCreating(gsmLog);                        
                ////        success = false;
                ////    }
                ////}
                //#endregion
                //#region DTM Daily profile

                ////////readOut.IsAborted = IsAborted;

                #endregion

            }
            catch
            {

            }
            finally
            {
                channelBase.DetachEvent();
                if (success)
                {
                    gsmLog.Status = "C";
                    gsmLog.ErrorMessage = "Data read successfully.";
                    GSMLogCreating(gsmLog);
                    result.ErrorCode = CommunicationErrorType.Success;
                    string fileText = string.Concat(generalData, tamperData, loadSurveyData, transactionData, phasorData, fraudEnergyData, dTMDailyProfileData);

                    if (fileText.ToUpper().Contains("LGC"))
                    {
                        string meterID = string.Concat("/", (((readOut.MeterID(channelBase.ResponseSignOn)).Split('/'))[1]));
                        SaveDataForSPhaseIEC(fileText, readOut.MeterID(channelBase.ResponseSignOn));
                    }
                    else // Story - 349654 - Meter Id is passed as parameter to append in the file name
                        SaveData(fileText);

                }
                else
                {
                    result.ErrorCode = CommunicationErrorType.Nothing;
                }
                objSerialComm.OpenSessionWithDelay();
                objSerialComm.CloseRemoteSession();

            }


            return result;
        }


        /// <summary>
        /// Saves the file - Location of the function has to be discussed.
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveData(string fileText)
        {
            string bcc = ReadoutCommon.CalculateFileBcc(fileText);
            if (bcc != "")
            {
                string resultMessage;
                fileText = string.Concat(fileText, bcc);
                string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                if (fileName.Length >= 14)
                {
                    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                }
                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".CAB");
                filePath = filePath.Replace("\\\\", "\\");
                fileText = ConfigInfo.EncryptFile(fileText);
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                if (uploadFile.UploadCABFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null))
                {
                    EventLogging.CallLogDetails("Data uploaded successfully.");
                }
                else
                {
                    EventLogging.CallLogDetails("Error occured while uploading data.");
                }
            }
        }
        /// <summary>
        /// Saves the file - Location of the function has to be discussed.
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveDataForSPhaseIEC(string fileText, string meterId)
        {
            string bcc = ReadoutCommon.CalculateFileBcc(fileText);
            if (bcc != "")
            {
                string resultMessage;
                fileText = string.Concat(fileText, bcc);
                //string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
                //if (fileName.Length >= 14)
                //{
                //    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                //}

                string fileName = ReadoutCommon.GetFileName().Trim();
                // Story - 349654 - To get the meter id and append in the file name
                meterId = meterId.Substring(meterId.IndexOf("/") + 1);
                meterId = meterId.Substring(13, 16);
                //if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId.Trim() + "_" + fileName;

                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".SLG");
                filePath = filePath.Replace("\\\\", "\\");
                fileText = ConfigInfo.EncryptFile(fileText);
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                if (uploadFile.UploadSLGFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null))
                {
                    EventLogging.CallLogDetails("Data uploaded successfully.");
                }
                else
                {
                    EventLogging.CallLogDetails("Error occured while uploading data.");
                }
            }
        }
        /// <summary>
        /// Connect to local modem, if connected to local modem connect to remote modem 
        /// </summary>
        /// <returns></returns>
        public Result OpenSession()
        {
            return communication.OpenSession(simNumber);
        }


        /// <summary>
        /// Connect to local modem, if connected to local modem connect to remote modem 
        /// </summary>
        /// <returns></returns>
        public Result OpenSessionTCP()
        {
            return communication.OpenSession();
        }


        /// <summary>
        /// Get File List From Server
        /// </summary>
        /// <returns></returns>
        private Result Get2nGFileListFromServer(string Dir, string ftpServerIP, string ftpUserID, string ftpPassword, bool IsAutoUpload)
        {
            Result res = new Result();
            res.ErrorCode = CommunicationErrorType.Nothing;
            Dictionary<string, string> downloadFiles = new Dictionary<string,string>();
            try
            {
                string URI = "ftp://" + ftpServerIP + "/" + Dir;
                EventLogging.CallLogDetails("Connecting To Server... " + URI);                                

                WebResponse response = null;
                StreamReader reader = null;

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(URI));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                               
              


                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                EventLogging.CallLogDetails("Reading File List From Server..." + URI);      
               
                while (!string.IsNullOrEmpty(line))
                {
                    line = line.Substring(line.IndexOf("/") + 1);
                    if (line.ToLower().Contains("2ng") && !downloadFiles.ContainsKey(line))
                    {
                        downloadFiles.Add(line,URI+"/"+line);                      
                    }
                    line = reader.ReadLine();
                }



                res = DownloadFileListFromServer(downloadFiles, ftpUserID, ftpPassword, IsAutoUpload);

               

           }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("Error Get File List From Server ..."); 
                res.ErrorCode = CommunicationErrorType.AccessDenied;
            }
            return res;
        }




        /// <summary>
        /// Download File From Server
        /// </summary>
        /// <returns></returns>
        private Result DownloadFileListFromServer(Dictionary<string, string> downloadFiles, string ftpUserID, string ftpPassword, bool IsAutoUpload)
        {
            Result res = new Result();
            try
            {
                res.ErrorCode = CommunicationErrorType.Nothing;
                if (downloadFiles != null && downloadFiles.Count > 0)
                {
                    string localDestnDir = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");

                    if (!Directory.Exists(localDestnDir))
                    {
                        Directory.CreateDirectory(localDestnDir);
                    }


                    #region File Downloading Source Code

                    foreach (KeyValuePair<string,string> Item in downloadFiles)
                    {
                        FtpWebRequest reqFTP;
                        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Item.Value));
                        reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                        reqFTP.KeepAlive = false;
                        reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                        reqFTP.UseBinary = true;
                        reqFTP.Proxy = null;
                        reqFTP.UsePassive = true;

                     
                        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                        long fileSize = response.ContentLength;
                        Stream responseStream = response.GetResponseStream();


                        int Length = 2048;

                        string AllFileData = string.Empty;
                        Byte[] buffer = new Byte[Length];
                        int bytesRead = responseStream.Read(buffer, 0, Length);
                        List<byte> ReceivedDatabytes = new List<byte>();

                        FileStream localFileStream = new FileStream(localDestnDir + "\\" + Item.Key, FileMode.Create);

                        while (bytesRead > 0)
                        {
                            localFileStream.Write(buffer, 0, bytesRead);
                            bytesRead = responseStream.Read(buffer, 0, Length);
                        }

                        //Close all Handles
                        localFileStream.Close();
                        response.Close();
                        reqFTP = null;


                        if (File.Exists(localDestnDir + "\\" + Item.Key))
                        {
                            EventLogging.CallLogDetails("File successfully Downloaded From Server..." + Item.Key);                                 
                        }

                    }
                 
                    #endregion


                    #region File Auto Uploading Source Code

              

                    if (IsAutoUpload)
                    {
                        foreach (KeyValuePair<string, string> Item in downloadFiles)
                        {
                            string strFileName = localDestnDir + "\\" + Item.Key;
                            if (File.Exists(strFileName))
                            {
                                string Message = string.Empty;
                                if (uploadFile.Upload2NGFile(strFileName, uploadFile.GetContent(strFileName), true, out Message, null))
                                {
                                    EventLogging.CallLogDetails("File successfully Uploaded in BCS..." + Item.Key);
                                }
                                else
                                {
                                    EventLogging.CallLogDetails("Error occured while uploading File..." + Item.Key);
                                }
                            }
                        }
                    }
                    #endregion
                }
                res.ErrorCode = CommunicationErrorType.Success;
            }           
            catch (Exception ex)
            {
                res.ErrorCode = CommunicationErrorType.AccessDenied;
                EventLogging.CallLogDetails("Error Downloading File List From Server ...");               
            }         
            return res;
        }


        /// <summary>
        /// Connect to FTP Server
        /// </summary>
        /// <returns></returns>
        public Result DownloadFTP(GSMTaskEntity gsmtask, string FTPServerIP)
        {
            //RFTP
            Result res = new Result();
            res.ErrorCode = CommunicationErrorType.Nothing;
            try
            {
                string LoginID = ConfigSettings.GetValue("LoginID");
                string LoginPassword = ConfigSettings.GetValue("LoginPassword");
                string ServerDirectory = ConfigSettings.GetValue("ServerDirectory");
                bool IsAutoUpload = Convert.ToBoolean(ConfigSettings.GetValue("IsAutoUpload"));
                DateTime CurrentDate = DateTime.Now.AddDays(-1);
                List<string> lstDateFolder = new List<string>();
                if (gsmtask.taskType == EnumUtil.stringValueOf(GSMTasksType.OneTimeOnly))
                {
                    //One Time Current Date Folder download recursively
                    string Dir = ServerDirectory + "/" + CurrentDate.Day.ToString().PadLeft(2, '0') + "-" + CurrentDate.Month.ToString().PadLeft(2, '0') + "-" + CurrentDate.Year.ToString().PadLeft(4, '0');
                    lstDateFolder.Add(Dir);
                }
                else if (gsmtask.taskType == EnumUtil.stringValueOf(GSMTasksType.Daily))
                {
                    //Daily Current Date Folder download recursively
                    string Dir = ServerDirectory + "/" + CurrentDate.Day.ToString().PadLeft(2, '0') + "-" + CurrentDate.Month.ToString().PadLeft(2, '0') + "-" + CurrentDate.Year.ToString().PadLeft(4, '0');
                    lstDateFolder.Add(Dir);
                }
                else if (gsmtask.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                {
                    //Weekely Current Week Folders download recursively
                    string Dir = ServerDirectory + "/" + CurrentDate.Day.ToString().PadLeft(2, '0') + "-" + CurrentDate.Month.ToString().PadLeft(2, '0') + "-" + CurrentDate.Year.ToString().PadLeft(4, '0');
                    lstDateFolder.Add(Dir);
                    DateTime Targetdate = CurrentDate.AddDays(-7);
                    if (Targetdate > DateTime.MinValue)
                    {
                        for (DateTime dt = Targetdate; dt < CurrentDate; dt = dt.AddDays(1))
                        {
                            Dir = ServerDirectory + "/" + dt.Day.ToString().PadLeft(2, '0') + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Year.ToString().PadLeft(4, '0');
                            lstDateFolder.Add(Dir);
                        }
                    }
                }
                else if (gsmtask.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                {
                    //Monthly Current Monthly Folder download recursively
                    string Dir = ServerDirectory + "/" + CurrentDate.Day.ToString().PadLeft(2, '0') + "-" + CurrentDate.Month.ToString().PadLeft(2, '0') + "-" + CurrentDate.Year.ToString().PadLeft(4, '0');
                    lstDateFolder.Add(Dir);
                    DateTime Targetdate = CurrentDate.AddMonths(-1);
                    if (Targetdate > DateTime.MinValue)
                    {
                        for (DateTime dt = Targetdate; dt < CurrentDate; dt = dt.AddDays(1))
                        {
                            Dir = ServerDirectory + "/" + dt.Day.ToString().PadLeft(2, '0') + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Year.ToString().PadLeft(4, '0');
                            lstDateFolder.Add(Dir);
                        }
                    }
                }
                else
                {
                    //One Time Current Date Folder download recursively (set default)
                    string Dir = ServerDirectory + "/" + CurrentDate.Day.ToString().PadLeft(2, '0') + "-" + CurrentDate.Month.ToString().PadLeft(2, '0') + "-" + CurrentDate.Year.ToString().PadLeft(4, '0');
                    lstDateFolder.Add(Dir);
                }
                foreach (string FolderDir in lstDateFolder)
                {
                    res = Get2nGFileListFromServer(FolderDir, FTPServerIP, LoginID, LoginPassword, IsAutoUpload);                    
                }                
            }
            catch (Exception ex) 
            {
                res.ErrorCode = CommunicationErrorType.AccessDenied;
            }
            return res;
        }



      

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Result CloseSession()
        {
            return communication.CloseRemoteSession();
        }
        /// <summary>
        /// /// Saves the file - Location of the function has to be discussed.
        /// </summary>
        /// <param name="gsmLoggingEntity"></param>
        public void GSMLogCreating(GSMLoggingEntity gsmLoggingEntity)
        {

            //EventLogging.CallLogDetails("Log ID" + logID + ", Task ID" + gsmLoggingEntity.Task_ID + " , Phone number " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " Retries : " + comRetries + " , " + gsmLoggingEntity.ErrorMessage);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            gsmLoggingEntity.CreationDateTime = DateTime.Now;
            gsmLoggingEntity.Log_ID = logID;
            if (logID > 0)
            {
                logBLL.UpdateData(gsmLoggingEntity);
            }
            else
            {
                logBLL.InsertData(gsmLoggingEntity, false);
            }

            //this.LogID = gsmLoggingEntity.Log_ID;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates and returns MD5 CheckSum - /// Saves the file - Location of the function has to be discussed.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetMD5ChecksumForFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(fileName))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", fileName));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] hashValue = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                StringBuilder checkSum = new StringBuilder();
                foreach (byte hasByte in hashValue)
                {
                    checkSum.Append(hasByte.ToString("X2"));
                }
                return checkSum.ToString().ToUpper();
            }
        }
        /// <summary>
        /// Updates the status of task in database - /// Saves the file - Location of the function has to be discussed.
        /// </summary>
        /// <param name="selectedProfile"></param>
        /// <param name="gsmLog"></param>
        private void UpdateStatus(ProfileId selectedProfile, GSMLoggingEntity gsmLog)
        {
            string profile = selectedProfile.ToString();
            switch (profile.ToUpper())
            {
                case "NAMEPLATE":
                    gsmLog.IsGeneralCompleted = true;
                    break;
                case "INSTANT":
                    gsmLog.IsInstantCompleted = true;
                    break;
                case "BILLING":
                    gsmLog.IsBillingCompleted = true;
                    break;
                case "TAMPER":
                    gsmLog.IsTamperCompleted = true;
                    break;
                case "LOADSURVEY":
                    gsmLog.IsLoadSurveyCompleted = true;
                    break;
                case "MIDNIGHT":
                    gsmLog.IsMidNightCompleted = true;
                    break;
                case "MeterConfiguration":
                    gsmLog.IsMeterConfigurationCompleted = true;
                    break;
            }
            gsmLog.ErrorMessage = selectedProfile.ToString() + " is completed.";
            gsmLog.Status = "IP";
            GSMLogCreating(gsmLog);
        }
        /// <summary>
        /// Gets the signature data in file format
        /// </summary>
        /// <param name="signatureInfo"></param>
        /// <returns></returns>
        private string GetSignatureDataInFileFormat(string signatureInfo)
        {
            string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            byte[] dataInByteForm = System.Text.Encoding.ASCII.GetBytes(signatureInfo);

            for (int dataIndex = 0; dataIndex < signatureInfo.Length; dataIndex++)
            {
                outputSignatureInfo = outputSignatureInfo + String.Format("{0:X2}", dataInByteForm[dataIndex]);
            }
            return outputSignatureInfo;
        }
        public static string FormatData(byte[] buffer, bool bUnsignFlag)
        {
            StringBuilder sb = new StringBuilder();

            bool bSignFlag = false;
            Int64 tempVal = 0;
            for (int i = 0; i < buffer.Length; i++)
            {

                if (buffer[0] > 127)
                {

                    if (buffer.Length > 1)
                    {
                        if (bUnsignFlag) bSignFlag = true;

                    }
                }
                sb.Append(buffer[i].ToString("X2"));
            }

            if (bSignFlag == true)
            {
                if (buffer.Length == 4)
                {
                    tempVal = Convert.ToInt64("FFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else if (buffer.Length == 8)
                {
                    tempVal = Convert.ToInt64("FFFFFFFFFFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else
                {
                    tempVal = Convert.ToInt32("FFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }

            }
            else
            {
                return Convert.ToInt64(sb.ToString(), 16).ToString();
            }
        }
        /// <summary>
        /// Gets the load survey profile command according to the gsm task saved.
        /// </summary>
        /// <param name="profileCommand"></param>
        /// <param name="gsmTask"></param>
        /// <param name="rtc"></param>
        /// <returns></returns>
        private ProfileCommand GetLoadSurveyProfileCommand(ProfileCommand profileCommand, GSMTaskEntity gsmTask, DateTime rtc)
        {
           // profileCommand.ToTime = rtc;
            profileCommand.SelectiveAccess = true;
           // profileCommand.FromTime = rtc.AddDays(-(Convert.ToInt32(ConfigSettings.GetValue("LoadSurveyDays"))));
            // EventLogging.CallLogDetails("From Time : " + profileCommand.FromTime.ToString());
            //  EventLogging.CallLogDetails("To Time : " + profileCommand.ToTime.ToString());

            //if (gsmTask.LoadSurveyJobType == JobType.LoadSurveyPartial)
            //{
            //    DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
            //    DateTime fromTime = loadSurveyBLL.GetLastLoadSurveyDataInDbForMeter(meterSerialNumber);
            //    //  EventLogging.CallLogDetails("Data base highest time : " + fromTime.ToString());
            //    if (fromTime != null && fromTime > DateTime.MinValue && fromTime > profileCommand.FromTime)
            //    {
            //        profileCommand.FromTime = fromTime;
            //    }
            //}
            //else
            if (gsmTask.LoadSurveyJobType == JobType.LoadSurveyPartialFrom)
            {
                profileCommand.FromTime = gsmTask.LoadSurveyFromDate;
                profileCommand.ToTime = gsmTask.LoadSurveyToDate;
            }
            else if (gsmTask.LoadSurveyJobType == JobType.LoadSurveyComplete)
            {
                profileCommand.FromTime = DateTime.Now.AddDays(-30);
                profileCommand.ToTime = DateTime.Now;
            }

            return profileCommand;
        }
        #endregion

    }
}
