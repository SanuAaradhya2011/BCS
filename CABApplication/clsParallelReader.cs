using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.Channel.ReadOut;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.MeterData.Upload;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Drawing;
using System.Security.Cryptography;
using System.Reflection;

namespace CABApplication
{
    public class clsParallelReader
    {
        
        #region MemberVariable&Constants
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        public Communication communication;
        private List<byte> meterId = null;
        public int securitymachanism = 0;
        DateTime readoutDateTime;
        private const string ReadoutFailure = "Readout Failure.";
        private ProfileId currentProfile = 0;
        private string firmwareVersion = null;
        public int RowCount;
        public string StrFileName;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(clsParallelReader).ToString());
        private const string Splitter = "$";
        private MeterConfigSettings meterConfigSettings = null;
        private Serializer serializer = null;
        DataGridViewRow dgvr = null;
        private CommunicationType commType;
        List<System.Enum> selectedMeterConfigProfile = null;
        string StatusMessageAsync = string.Empty;
        List<System.Enum> lstProfiles = null;
        private bool readSuccess = false;
        private CommunicationMode commMode = CommunicationMode.Normal;
        private int countProfile;
        private string Staticip = string.Empty;
        private string Tcpport = string.Empty;
        private bool IsDebugLog = false;
        private string DebugLogFileName = string.Empty;
        private static object LockUpload = new object();
        #endregion


        #region Constructor

        public clsParallelReader(int rowCount, string strFileName, DataGridViewRow DataGridRow, List<System.Enum> LstProfiles, CommunicationMode CommMode, string Port, CommunicationType CommType)
        {
            this.RowCount = rowCount;
            this.StrFileName = strFileName;
            this.dgvr = DataGridRow;
            this.serializer = new Serializer();
            this.lstProfiles = LstProfiles;
            this.commMode = CommMode;
            this.Tcpport = Port;
            this.commType = CommType;
            IsDebugLog = Convert.ToBoolean(ConfigSettings.GetValue("IsDebugLog"));
            DebugLogFileName = dgvr.Cells["SimNo"].Value.ToString().Replace('.','_')  + "_"+ DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (meterConfigSettings == null)
                meterConfigSettings = (MeterConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterConfigSettings.xml"), typeof(MeterConfigSettings));

            WriteDebugLog("clsParallelReader");
        }

        #endregion


        #region MemberFunctions

        private void WriteDebugLog(string Message)
        {
            try
            {
                if (IsDebugLog)
                {
                    File.AppendAllText(DebugLogFileName, Message + "\n");
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteDebugLog(string Message)", ex);
                MessageBox.Show(ex.ToString());
            }
        }



        /// <summary>        
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="firmware"></param>
        /// <returns></returns>
        private MeterConfigSettingsMeterConfigElement GetMeterConfig(string meterModel, string firmware)
        {
            MeterConfigSettingsMeterConfigElement result = null;
            try
            {
                if (meterModel == "6" || meterModel == "9" || meterModel == "8" || meterModel == "10" || meterModel == "11"
                    || meterModel == "12" || meterModel == "13" || meterModel == "14" || meterModel == "18" || ConfigInfo.MeterModel == ((int)NamePlateConstants.SFSP).ToString())
                {
                    firmware = "0.00";
                }
                //result = meterConfigSettings.Items[0];
                foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
                {
                    if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == firmware.ToString())
                    {
                        result = meterConfigSettingsElement;
                        break;
                    }
                }
                if (result == null)
                {
                    foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
                    {
                        if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == "0.00")
                        {
                            result = meterConfigSettingsElement;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "GetMeterConfig(string meterModel, string firmware)", ex);
            }
            return result;
        }


        /// <summary>
        /// checks what parameters to show for meter configuration
        /// </summary>
        /// <param name="meterModelNumber"></param>
        /// <param name="firmwareVersion"></param>
        /// <returns></returns>
        private List<System.Enum> CheckMeterConfiguration(string meterModelNumber, string firmwareVersion)
        {

            List<System.Enum> selectedProfiles = new List<System.Enum>();
            try
            {
                MeterConfigSettingsMeterConfigElement element = GetMeterConfig(meterModelNumber, firmwareVersion);
                if (element != null)
                {

                    if (Convert.ToBoolean(element.DIP))
                    {
                        selectedProfiles.Add(ProfileId.DIP);
                    }
                    if (Convert.ToBoolean(element.KvahSelection))
                    {
                        selectedProfiles.Add(ProfileId.KvahSelection);
                    }
                    if (Convert.ToBoolean(element.DisplayParameters))
                    {
                        selectedProfiles.Add(ProfileId.PushDisplayParameter);
                        selectedProfiles.Add(ProfileId.ScrollDisplyParameter);
                        selectedProfiles.Add(ProfileId.HighResolutionDisplayParameter);
                        //selectedProfiles.Add(ProfileId.DisplayTimeoutParameter); // Story - Hide Display Timeout Parameter
                    }
                    if (element.TOD.ToString().ToUpper() == "ONE"
                        || element.TOD.ToString().ToUpper() == "TWO"
                        || element.TOD.ToString().ToUpper() == "FOUR"
                        || element.TOD.ToString().ToUpper() == "FOURSP"
                        || element.TOD.ToString().ToUpper() == "THREE")     //SarkarA code change 20180223 // added 3TOU
                    {
                        selectedProfiles.Add(ProfileId.ActiveDayProfile);
                        selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                        selectedProfiles.Add(ProfileId.ActiveSeasonProfile);

                        selectedProfiles.Add(ProfileId.PassiveDayProfile);
                        selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                        selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                        selectedProfiles.Add(ProfileId.ActivationDate);
                    }

                    if (element.TOD.ToString().ToUpper() == "FOURSP10Z8S")
                    {
                        selectedProfiles.Add(ProfileId.ActiveDayProfile);
                        selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                        selectedProfiles.Add(ProfileId.ActiveSeasonProfile);

                        selectedProfiles.Add(ProfileId.PassiveDayProfile);
                        selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                        selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                        selectedProfiles.Add(ProfileId.ActivationDate);
                        selectedProfiles.Add(ProfileId.SpecialDayProfileSmartMeter);
                    }


                    if (Convert.ToBoolean(element.RTC))
                    {
                        selectedProfiles.Add(ProfileId.RTC);
                    }
                    // [BillingType_Month]
                    if (element.BillingType.ToString().ToUpper() == "NORMAL")
                    {
                        selectedProfiles.Add(ProfileId.BillingType);
                    }
                    else if (element.BillingType.ToString().ToUpper() == "OTHER")
                    {
                        selectedProfiles.Add(ProfileId.BillingType);
                        selectedProfiles.Add(ProfileId.BillingMonthType);
                    }
                    if (Convert.ToBoolean(element.AutoLock))
                    {
                        selectedProfiles.Add(ProfileId.AutoLock);
                    }
                    if (Convert.ToBoolean(element.ManualBilling))
                    {
                        selectedProfiles.Add(ProfileId.ManualBilling);
                    }
                    if (Convert.ToBoolean(element.SoftwareBilling))
                    {
                        selectedProfiles.Add(ProfileId.SoftwareBilling);
                    }
                    if (Convert.ToBoolean(element.LockRS232))
                    {
                        selectedProfiles.Add(ProfileId.RS232LockUnlock);
                    }
                    if (Convert.ToBoolean(element.SIP))
                    {
                        selectedProfiles.Add(ProfileId.SIP);
                    }
                    if (Convert.ToBoolean(element.CTRatio))
                    {
                        selectedProfiles.Add(ProfileId.CTRatio);
                    }
                    if (Convert.ToBoolean(element.PTRatio))
                    {
                        selectedProfiles.Add(ProfileId.PTRatio);
                    }
                    if (Convert.ToBoolean(element.DIPWithSliding))
                    {
                        selectedProfiles.Add(ProfileId.DIPWithSliding);
                    }
                    if (Convert.ToBoolean(element.DisconnectControl))
                    {
                        selectedProfiles.Add(ProfileId.DisconnectControl);
                    }
                    if (Convert.ToBoolean(element.LoadControl))
                    {
                        selectedProfiles.Add(ProfileId.LoadControl);
                    }
                    if (Convert.ToBoolean(element.LoadControl1PSmartMeter))
                    {
                        selectedProfiles.Add(ProfileId.LoadControl1PSmartMeter);
                    }
                    if (Convert.ToBoolean(element.RS485))
                    {
                        selectedProfiles.Add(ProfileId.RS485);
                    }
                    if (Convert.ToBoolean(element.PaymentMode))
                    {
                        selectedProfiles.Add(ProfileId.Paymentmode);
                    }
                    if (Convert.ToBoolean(element.Meteringmode))
                    {
                        selectedProfiles.Add(ProfileId.Meteringmode);
                    }
                    if (Convert.ToBoolean(element.LoadLimit))
                    {
                        selectedProfiles.Add(ProfileId.LoadLimit);
                    }
                    if (Convert.ToBoolean(element.SlidingDemand))
                    {
                        selectedProfiles.Add(ProfileId.Slidingdemand);
                    }
                    if (Convert.ToBoolean(element.OpticalLockUnlock))
                    {
                        selectedProfiles.Add(ProfileId.OpticalLockUnlock);
                    }
                    if (Convert.ToBoolean(element.RJLockUnlock))
                    {
                        selectedProfiles.Add(ProfileId.RJLockUnlock);
                    }
                }
                if (selectedProfiles.Contains(ProfileId.Instant))
                {
                    selectedProfiles.Add(ProfileId.Anomaly);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "CheckMeterConfiguration(string meterModelNumber, string firmwareVersion)", ex);
            }
           
            return selectedProfiles;
        }


        /// <summary>
        /// GetProfileCommandsToRead
        /// </summary>     
        /// <param name="lstProfileCommands" Type= "List<ProfileCommand>"></param>    
        /// <param name="selectedProfile" Type= "ProfileId"></param>    
        /// <param name="meterModelNumber" Type= "int"></param>    
        /// <param name="commMode" Type= "CommunicationMode"></param>    
        /// <returns "List<ProfileCommand>"></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber, CommunicationMode commMode)
        {
            List<ProfileCommand> profileReadCommands = null;
            try
            {
                if (commMode == CommunicationMode.Normal)
                {
                    //find normal commands
                    profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        return profileCommandEntity.TagNumber == (int)selectedProfile
                        && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                        && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                        profileCommandEntity.MeterModelNumber == 0);
                    });
                }
                else
                {
                    //find fast download commands
                    profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        if (profileCommandEntity.TagNumber == 3)
                        {
                            return (profileCommandEntity.TagNumber == (int)selectedProfile)
                             && (profileCommandEntity.ClassId == 0xFD)
                             && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                             profileCommandEntity.MeterModelNumber == 0);
                        }
                        //else if (profileCommandEntity.TagNumber == 4 && meterModelNumber == 2)
                        //{
                        //    return (profileCommandEntity.TagNumber == (int)selectedProfile)
                        //     && (profileCommandEntity.ClassId == 0xFD)
                        //     && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                        //     profileCommandEntity.MeterModelNumber == 0);
                        //}
                        else
                        {
                            return (profileCommandEntity.TagNumber == (int)selectedProfile)
                            && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                            && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                            profileCommandEntity.MeterModelNumber == 0);
                        }
                    });

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber, CommunicationMode commMode)", ex);
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
            }
            return profileReadCommands;
        }


        /// <summary>
        /// GetProfileCommandEntity
        /// </summary>           
        /// <returns "List<ProfileCommand>"></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            try
            {
                DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
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
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "GetProfileCommandEntity()", ex);
            }            
            return lstProfileCommands;
        }

        private bool GetMeterData(string strFileName, bool isRemote, int simIndex)
        {
            FileStream initialFileStream = null;
            StreamWriter writeToFile = null;
            string data = string.Empty;
            string lngFilename = string.Empty;
            string classIdOBISCodeAttribute = string.Empty;
            string meterSerialNumber = string.Empty;
            string idLength = string.Empty;
            int index = 0;
            String fileMeterData;
            int meterModelNumber = 0;
            data = string.Empty;
            List<System.Enum> selectedProfiles;
            List<ProfileCommand> lstProfileCommands;
            lstProfileCommands = GetProfileCommandEntity();
            // selectedProfiles = GetSelectedProfilesToRead();

            bool isConnected = true;
            ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
            Result result = communication.Send(profileCommand);
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
                        SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading Device ID", dgvr.Cells["Status"]);
                    }

                    else // To handle hpl meter id datatype 0x06 --> 4 byte integer
                    {

                        int adbyteindex = 0;
                        byte[] incount = new byte[4];
                        for (int i = 1; i <= 4; i++)
                        {
                            incount[adbyteindex++] = result.RecieveDataBuffer[i];

                        }
                        data = FormatData(incount, false);
                    }
                    if (isRemote && commType == CommunicationType.GSM)
                    {
                        //dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, simIndex].Value = data;
                    }

                    #region CreateResultFile
                    try
                    {
                        meterSerialNumber = data.TrimEnd();
                        Application.DoEvents();
                        strFileName = strFileName + data;
                        readoutDateTime = DateTime.Now;
                        strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                        fileMeterData = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);

                        initialFileStream = new FileStream(strFileName, FileMode.Create);
                        writeToFile = new StreamWriter(initialFileStream);
                        writeToFile.WriteLine(Splitter);
                        writeToFile.WriteLine(index.ToString("00"));
                        writeToFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        result.ErrorCode = CommunicationErrorType.InvalidMeterIDName;
                        logger.Log(LOGLEVELS.Error, "GetMeterData(string strFileName, bool isRemote, int simIndex)", ex);
                    }
                    #endregion

                    Application.DoEvents();
                    DateTime meterTime;
                    string signatureInfo = "";
                    int meterType = 0;
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        meterTime = communication.GetMeterDateTime();
                        SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading Device RTC.", dgvr.Cells["Status"]);
                        signatureInfo = communication.GetSignatureData();
                        SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading Signature command.", dgvr.Cells["Status"]);
                        if (signatureInfo == "")
                        {
                            SetGridRowAttributes(System.Drawing.Color.Red, "Meter Reading Failed", dgvr.Cells["Status"]);
                            //this.StatusMessageAsync = "Meter Reading Failed";
                            return false;
                        }

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
                            if (signatureInfo != "")
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
                        
                        else if (ConfigSettings.GetValue("OtherManufacture") == "TRUE" && signatureInfo != "Non-Landis+GyrMeter")
                        {
                            MessageBox.Show("Setting saved as Non Cabcon Meter " + "\n" + "Connected Meter is Cabcon." + "\n" + "Please change the settings or change the meter.");
                            this.StatusMessageAsync = "";
                            return isConnected = false;

                         
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

                        else if (signatureInfo.Contains("L0"))//Amendment 5 changes
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

                        else if (signatureInfo.Contains("W0")) //Amendment 5 changes meter models
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
                        else if (signatureInfo.Contains("SPS2"))
                        {
                            meterModelNumber = NamePlateConstants.SapphireS2;
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
                        else if (signatureInfo.Contains("Non-Landis+GyrMeter"))
                        {
                            meterModelNumber = NamePlateConstants.NonLandisMeter;
                        }


                        else
                        {
                            meterModelNumber = NamePlateConstants.InvalidModelValue;
                        }
                        if (meterModelNumber != NamePlateConstants.NonLandisMeter && meterModelNumber != 0)
                        {
                            selectedMeterConfigProfile = CheckMeterConfiguration(meterModelNumber.ToString(), firmwareVersion.TrimStart('0'));
                        }
                        else
                        {
                            //SetGridRowAttributes(Color.LightGray, ProfileId.MeterConfiguration, "Readout Not Supported.");
                            SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.MeterConfiguration + "Readout Not Supported.", dgvr.Cells["Status"]);
                        }
                        selectedProfiles = GetSelectedProfilesToRead();
                        writeToFile.WriteLine(GetSignatureDataInFileFormat(signatureInfo));
                        if ((signatureInfo.ToUpper().Contains("W0") || signatureInfo.ToUpper().Contains("L0") || signatureInfo.ToUpper().Contains("SC") || signatureInfo.ToUpper().Contains("ST") || signatureInfo.ToUpper().Contains("st")) && (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1) && (commType == CommunicationType.DIRECT))
                            communication.SetBaudRate(6);

                        #region ReadProfileAndWriteToFile
                        Application.DoEvents();
                        //NOt a good practice , but forced ro apply check as for NDPL Ruby meter with version 1.66
                        //a strange thing encountered that if BCS sends command for anomaly and then reads midnight data it returnes no data even if meter has midnight data.
                        if (firmwareVersion == "1.66")
                        {
                            selectedProfiles.Remove(ProfileId.Anomaly);
                        }
                        //MSEDCL Check for Daily Load Profile
                        if (firmwareVersion == "2.21" && meterModelNumber == NamePlateConstants.RubyE250Value)
                        {
                            if (selectedProfiles.Contains(ProfileId.Midnight))
                            {
                                selectedProfiles.Remove(ProfileId.Midnight);
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Midnight + "Readout Not Supported.", dgvr.Cells["Status"]);
                                //SetGridRowAttributes(Color.LightGray, ProfileId.Midnight, "Readout Not Supported.");
                            }
                        }
                        if (meterModelNumber == NamePlateConstants.SM110value || meterModelNumber == NamePlateConstants.NonLandisMeter)
                        {
                            if (selectedProfiles.Contains(ProfileId.Phasor))
                            {
                                selectedProfiles.Remove(ProfileId.Phasor);
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Phasor + "Readout Not Supported.", dgvr.Cells["Status"]);
                                //SetGridRowAttributes(Color.LightGray, ProfileId.Phasor, "Readout Not Supported.");
                            }
                        }
                        if (selectedProfiles.Contains(ProfileId.LoadSwitch) && meterModelNumber == NamePlateConstants.NonLandisMeter)
                        {
                            selectedProfiles.Remove(ProfileId.LoadSwitch);
                            SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.LoadSwitch + "Readout Not Supported.", dgvr.Cells["Status"]);
                            //SetGridRowAttributes(Color.LightGray, ProfileId.LoadSwitch, "Readout Not Supported.");
                        }
                        //Fast Download check for PVVNL  
                        if (commMode == CommunicationMode.FastDownload && (firmwareVersion == "2.21" || firmwareVersion == "06.03"
                            || firmwareVersion == "06.09"))
                        {                           
                            SetGridRowAttributes(System.Drawing.Color.Red, "Fast Download mode not supported.", dgvr.Cells["Status"]);
                            return false;
                        }
                        foreach (ProfileId selectedProfile in selectedProfiles)
                        {
                            readSuccess = false;
                            currentProfile = selectedProfile;
                            // HTCT Specific Changes
                            if (selectedProfile == ProfileId.KvahSelection && meterModelNumber == 10)
                            {
                               // this.StatusMessageAsync = "Reading Mvah Selection";
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading Mvah Selection Data...", dgvr.Cells["Status"]);
                            }
                            else
                            {
                                //this.StatusMessageAsync = "Reading " + CommonBLL.GetEnumDescription(selectedProfile) + " Data...";
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading " + CommonBLL.GetEnumDescription(selectedProfile) + " Data...", dgvr.Cells["Status"]);
                            }


                            if (selectedMeterConfigProfile.Contains(selectedProfile))
                            {
                                //SetGridRowAttributes(Color.LightYellow, ProfileId.MeterConfiguration, "Reading Data...");
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading" + selectedProfile + "Data...", dgvr.Cells["Status"]);
                            }
                            else if (selectedProfile == ProfileId.Anomaly)
                            {
                               // SetGridRowAttributes(Color.LightYellow, ProfileId.Instant, "Reading Data...");
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading" + ProfileId.Instant + "Data...", dgvr.Cells["Status"]);
                            }
                            else
                            {
                               // SetGridRowAttributes(Color.LightYellow, selectedProfile, "Reading Data...");
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Reading" + selectedProfile + "Data...", dgvr.Cells["Status"]);
                            }

                            if (isRemote)
                            {
                                //dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                //Application.DoEvents();
                            }
                            //******* Meter Model Change Required Here ***********//
                            if (((meterModelNumber == NamePlateConstants.RubyE250Value) || (meterModelNumber == NamePlateConstants.RubyE150Value || meterModelNumber == NamePlateConstants.VBSPNoSeasonNoWeek || meterModelNumber == NamePlateConstants.SM110value || meterModelNumber == NamePlateConstants.SM110value) || meterModelNumber == NamePlateConstants.Smartmeter_LTCT || meterModelNumber == NamePlateConstants.Smartmeter_WCM || meterModelNumber == NamePlateConstants.SmartM_Cipher_WCM || meterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT)
                                && (commMode == CommunicationMode.FastDownload))
                            {
                                this.StatusMessageAsync = "Fast download mode not supported.";
                                result.ErrorCode = CommunicationErrorType.FastDownloadNotSupported;
                                break;
                            }
                            else
                            {

                                //Get profile commands according to communication mode saved in other settings
                                List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, selectedProfile, meterModelNumber);

                                if (selectedProfile == ProfileId.LoadSurvey)
                                {
                                    int commandIndex = commMode == CommunicationMode.FastDownload ? 0 : 1;
                                    profileReadCommands[commandIndex].SelectiveAccess = true;
                                    profileReadCommands[commandIndex].ToTime = meterTime;
                                    profileReadCommands[commandIndex].FromTime = meterTime.AddDays(-(Convert.ToInt32(ConfigSettings.GetValue("LoadSurveyDays"))));
                                }
                                for (index = 0; index < profileReadCommands.Count; index++)
                                {
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        profileReadCommands[index].Action = ActionType.READ;
                                        profileReadCommands[index].MeterID = meterId;

                                        result = communication.Send(profileReadCommands[index]);

                                        classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                            + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                   + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                        if (((result.RecieveDataLength <= 0) || (result.RecieveDataLength < 33)) && selectedProfile == ProfileId.Phasor && profileReadCommands[index].ClassId == 0xFE)
                                        {
                                            result.ErrorCode = CommunicationErrorType.Success;
                                        }
                                        else if ((result.RecieveDataLength <= 0) && selectedProfile == ProfileId.Tamper)
                                        {
                                            result.ErrorCode = CommunicationErrorType.Success;
                                            writeToFile.WriteLine(classIdOBISCodeAttribute + "0100");
                                            // *********** For smart meter Falcon 2 ******
                                            if (index == profileReadCommands.Count - 1)
                                            {
                                               // SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                SetGridRowAttributes(System.Drawing.Color.LightGreen, selectedProfile + "Readout Successful.", dgvr.Cells["Status"]);
                                            }
                                        }
                                        else if ((result.RecieveDataLength <= 0) || (result.RecieveDataLength < 33 && selectedProfile == ProfileId.Phasor))
                                        {
                                            if (selectedProfile == ProfileId.NamePlate)
                                            {
                                                readSuccess = true;
                                            }
                                            else
                                            {
                                                if (readSuccess)
                                                {
                                                   // SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                    SetGridRowAttributes(System.Drawing.Color.LightGreen, selectedProfile + "Readout completed.", dgvr.Cells["Status"]);
                                                  // SetGridRowAttributes(false,dgvr.Cells["Select"] as DataGridViewCheckBoxCell);
                                                    break;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            readSuccess = true;
                                            writeToFile.Write(classIdOBISCodeAttribute);
                                            for (int i = 0; i < result.RecieveDataBuffer.Count; i++)
                                            {
                                                writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                            }
                                            writeToFile.WriteLine("");
                                            if (index == profileReadCommands.Count - 1)
                                            {
                                                if (selectedProfile == ProfileId.Anomaly)
                                                {
                                                    //SetGridRowAttributes(Color.LightGreen, ProfileId.Instant, "Readout Successful.");
                                                    SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Instant + "Readout Successful.", dgvr.Cells["Status"]);
                                                }
                                                else
                                                {
                                                    //SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                    SetGridRowAttributes(System.Drawing.Color.LightGreen, selectedProfile + "Readout Successful.", dgvr.Cells["Status"]);
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        break;
                                    }


                                }
                                if (!readSuccess)
                                {
                                    if (result.ErrorCode == CommunicationErrorType.ResponseTimeout)
                                    {
                                       // SetGridRowAttributes(Color.Red, selectedProfile, "Sign On Failure/Timeout.");
                                        SetGridRowAttributes(System.Drawing.Color.Red, selectedProfile + "Sign On Failure/Timeout.", dgvr.Cells["Status"]);
                                    }
                                    else
                                    {
                                        //SetGridRowAttributes(Color.LightGray, selectedProfile, "Readout Not Supported.");
                                        SetGridRowAttributes(System.Drawing.Color.LightYellow, selectedProfile + "Readout Not Supported.", dgvr.Cells["Status"]);
                                    }
                                }

                                if (result.ErrorCode != CommunicationErrorType.Success)
                                {
                                    if (result.ErrorCode == CommunicationErrorType.ResponseTimeout)
                                    {
                                        if (selectedProfile == ProfileId.Anomaly)
                                        {
                                           // SetGridRowAttributes(Color.Red, ProfileId.Instant, "Sign On Failure/Timeout.");
                                            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Instant + "Sign On Failure/Timeout.", dgvr.Cells["Status"]);
                                        }
                                        else if (selectedMeterConfigProfile.Contains(selectedProfile))
                                        {
                                            //SetGridRowAttributes(Color.Red, ProfileId.MeterConfiguration, "Sign On Failure/Timeout.");
                                            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.MeterConfiguration + "Sign On Failure/Timeout.", dgvr.Cells["Status"]);
                                        }
                                        else
                                        {
                                            //SetGridRowAttributes(Color.Red, selectedProfile, "Sign On Failure/Timeout.");
                                            SetGridRowAttributes(System.Drawing.Color.Red, selectedProfile + "Sign On Failure/Timeout.", dgvr.Cells["Status"]);
                                        }
                                    }
                                    break;
                                }
                            }
                            //Last element of meter config so make meter config row as successfull.
                            if (selectedMeterConfigProfile.Count > 0 && selectedProfile == (ProfileId)selectedMeterConfigProfile[selectedMeterConfigProfile.Count - 1])
                            {
                               // SetGridRowAttributes(Color.LightGreen, ProfileId.MeterConfiguration, "Readout Successful.");
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.MeterConfiguration + "Readout Successful.", dgvr.Cells["Status"]);
                            }


                        }
                        #endregion

                        if ((signatureInfo.ToUpper().Contains("W0") || signatureInfo.ToUpper().Contains("L0") || signatureInfo.ToUpper().Contains("SC") || signatureInfo.ToUpper().Contains("ST") || signatureInfo.ToUpper().Contains("st")) && (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1) && (commType == CommunicationType.DIRECT))
                            communication.SetBaudRate(5);
                    }


                    #region ResourceClosingAndCleanup
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {

                        communication.CloseSession();
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
                        File.Delete(strFileName);
                        if (fileText != "")
                        {

                            if (commType == CommunicationType.DIRECT)
                            {                                
                            }
                            else
                            {
                                SaveRemoteData(fileText, data.TrimEnd());
                            }
                        }

                    }
                    else if (result.ErrorCode == CommunicationErrorType.InvalidMeterIDName)
                    {
                        communication.CloseSession();
                       // this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                        SetGridRowAttributes(System.Drawing.Color.Red, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);

                    }
                    else
                    {
                        isConnected = false;
                        writeToFile.Close();
                        initialFileStream.Close();
                        File.Delete(strFileName);
                        //this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                        SetGridRowAttributes(System.Drawing.Color.Red, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                    }
                    #endregion

                }
                else
                {
                    isConnected = false;
                    //this.StatusMessageAsync = ReadoutFailure;
                    SetGridRowAttributes(System.Drawing.Color.Red, "Readout Failure", dgvr.Cells["Status"]);
                }
            }
            else
            {
                isConnected = false;
                try
                {
                    //this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    SetGridRowAttributes(System.Drawing.Color.Red, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                }
                catch (Exception ex)    //Exception log for catch block 
                {
                    logger.Log(LOGLEVELS.Error, "GetMeterData(string strFileName, bool isRemote, int simIndex)", ex);
                }
            }
            return isConnected;

        }

        private void SetGridRowAttributes(Color color, ProfileId profileId, string p)
        {
            throw new NotImplementedException();
        }            


        /// <summary>
        /// GetMD5ChecksumForFile
        /// </summary>     
        /// <param name="fileName" Type= "string"></param>
        /// <returns "string"></returns>
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
        /// GetProfileCommandsToRead
        /// </summary>     
        /// <param name="lstProfileCommands" Type= "List<ProfileCommand>"></param>    
        /// <param name="selectedProfile" Type= "ProfileId"></param>    
        /// <param name="meterModelNumber" Type= "int"></param>    
        /// <returns "List<ProfileCommand>"></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber)
        {
            List<ProfileCommand> profileReadCommands = null;
            try
            {
                if (commMode == CommunicationMode.Normal)
                {
                    //find normal commands
                    profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        return profileCommandEntity.TagNumber == (int)selectedProfile
                        && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                        && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                        profileCommandEntity.MeterModelNumber == 0);
                    });
                }
                else
                {
                    //find fast download commands
                    profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        if (profileCommandEntity.TagNumber == 3)
                        {
                            return (profileCommandEntity.TagNumber == (int)selectedProfile)
                             && (profileCommandEntity.ClassId == 0xFD)
                             && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                             profileCommandEntity.MeterModelNumber == 0);
                        }
                        //else if (profileCommandEntity.TagNumber == 4 && meterModelNumber == 2)
                        //{
                        //    return (profileCommandEntity.TagNumber == (int)selectedProfile)
                        //     && (profileCommandEntity.ClassId == 0xFD)
                        //     && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                        //     profileCommandEntity.MeterModelNumber == 0);
                        //}
                        else
                        {
                            return (profileCommandEntity.TagNumber == (int)selectedProfile)
                            && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                            && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                            profileCommandEntity.MeterModelNumber == 0);
                        }
                    });

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber)", ex);
            }
            return profileReadCommands;
        }


        /// <summary>
        /// GetSignatureDataInFileFormat
        /// </summary>     
        /// <param name="signatureInfo" Type= "string"></param>        
        /// <returns "string"></returns>
        private string GetSignatureDataInFileFormat(string signatureInfo)
        {
            //string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            string outputSignatureInfo = "0100006001BCFF02";
            try
            {
                // Combining the signature Data length and Octet string with the outputSignatureInfo //User Story 478245. Voltage Rating change to 63.5 V for HK meter
                if (ConfigInfo.SignatureDataLength != string.Empty)
                {
                    outputSignatureInfo += ConfigInfo.SignatureDataLength;
                }
                else
                {
                    outputSignatureInfo += "0914";
                }
                byte[] dataInByteForm = System.Text.Encoding.ASCII.GetBytes(signatureInfo);

                for (int dataIndex = 0; dataIndex < signatureInfo.Length; dataIndex++)
                {
                    outputSignatureInfo = outputSignatureInfo + String.Format("{0:X2}", dataInByteForm[dataIndex]);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "GetSignatureDataInFileFormat(string signatureInfo)", ex);
            }
        
            return outputSignatureInfo;
        }

        /// <summary>
        /// GetSelectedProfilesToRead
        /// </summary>     
        /// <returns "List<System.Enum>"></returns>
        private List<System.Enum> GetSelectedProfilesToRead()
        {
            List<System.Enum> selectedProfiles = new List<System.Enum>();
            try
            {
                selectedProfiles.Clear();
                selectedProfiles.Add(ProfileId.NamePlate);
                selectedProfiles.Add(ProfileId.NamePlateProfile);
                selectedProfiles.AddRange(lstProfiles);
                if (selectedProfiles.Contains(ProfileId.Instant))
                {
                    selectedProfiles.Insert(selectedProfiles.IndexOf(ProfileId.NamePlate) + 1, ProfileId.Anomaly);
                }
                if (selectedProfiles.Contains(ProfileId.MeterConfiguration))
                {
                    if (selectedMeterConfigProfile.Count != 0)
                    {
                        // Keeping Load Survey as the last parameter to be read
                        if (selectedProfiles.Contains(ProfileId.LoadSurvey))
                        {
                            selectedProfiles.InsertRange(selectedProfiles.IndexOf(ProfileId.LoadSurvey) - 1, selectedMeterConfigProfile);
                        }
                        else
                        {
                            selectedProfiles.AddRange(selectedMeterConfigProfile);
                        }
                    }
                    selectedProfiles.Remove(ProfileId.MeterConfiguration);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                
                 this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                 logger.Log(LOGLEVELS.Error, "GetSelectedProfilesToRead()", ex);
            }
           
            return selectedProfiles;
        }



        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="selectedProfile" Type= "System.Enum"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color, System.Enum selectedProfile, string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                    {
                        
                        dgvc.Style.BackColor = color;
                        dgvc.Value = CommonBLL.GetEnumDescription(selectedProfile) + Message;

                        if (dgvc.Style.BackColor == Color.Red)
                        {
                            dgvc.Style.ForeColor = Color.White;
                        }
                    };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SetGridRowAttributes(System.Drawing.Color color, System.Enum selectedProfile, string Message, DataGridViewCell dgvc)", ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="Message" Type= "string"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color,  string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                    {
                        dgvc.Style.BackColor = color;
                        dgvc.Value = Message;
                        //Application.DoEvents();
                    };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SetGridRowAttributes(System.Drawing.Color color,  string Message, DataGridViewCell dgvc)", ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="Message" Type= "string"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                    {
                        dgvc.Value = Message;
                        //Application.DoEvents();
                    };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SetGridRowAttributes(string Message, DataGridViewCell dgvc)", ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set CheckValue for DataGridViewCheckBoxCell
        /// </summary>
        /// <param name="Value" Type= "bool"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(bool Value, DataGridViewCheckBoxCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                    {
                        dgvc.Value = Value;
                        //Application.DoEvents();
                    };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SetGridRowAttributes(bool Value, DataGridViewCheckBoxCell dgvc)", ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set backcolor for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color , DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                    {
                        dgvc.Style.BackColor = color;
                        //Application.DoEvents();
                    };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SetGridRowAttributes(System.Drawing.Color color , DataGridViewCell dgvc)", ex);
            }
        }



        /// <summary>
        /// To make sure that file upload window will same as IEC.
        /// </summary>
        /// <param name="fileText"></param>
        /// <returns></returns>
        private void SaveRemoteData(string fileText, string meterId)
        {
            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
            if (fileName.Trim().Equals(string.Empty))
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");
                //SetGridRowAttributes(MessageConstant.GetText("M000047"),dgvr.Cells["Status"]);
                SetGridRowAttributes(System.Drawing.Color.LightGreen, MessageConstant.GetText("M000047"), dgvr.Cells["Status"]);
            }
            else
            {
                // To make sure that both direct and remote communication read file naming convention will be same.
                if (fileName.Length >= 14)
                {
                    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                }
                fileName = meterId + "_" + fileName;
                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
                filePath = filePath.Replace("\\\\", "\\");

                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file);
                    wr1.Write(fileText);
                    wr1.Close();
                    file.Close();
                    this.StatusMessageAsync = MessageConstant.GetText("M000048");
                    //SetGridRowAttributes(MessageConstant.GetText("M000048"),dgvr.Cells["Status"]);
                    SetGridRowAttributes(System.Drawing.Color.LightGreen, MessageConstant.GetText("M000048"), dgvr.Cells["Status"]);
                    if (commType != CommunicationType.TCP)
                    FileUplaod(filePath);
                }
                catch (Exception Ex)    //Exception log for catch block
                {
                    //MessageBox.Show(Ex.Message, "BCS");
                    this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + Ex.ToString());
                    logger.Log(LOGLEVELS.Error, "SaveRemoteData(string fileText, string meterId)", Ex);
                }

            }

        }


        /// <summary>
        /// SaveData
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="meterId"></param>
        /// <returns></returns>
        private void SaveData(string fileText, string meterId)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
            if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId + "_" + fileName;
            bool Flag = false;
        ReConf:
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessageAsync = "File name can't be empty.";
                        SetGridRowAttributes("File name can't be empty.",dgvr.Cells["Status"]);
                        //this.RightStatusMessageAsync = "";
                        //Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessageAsync = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");
                //SetGridRowAttributes(MessageConstant.GetText("M000047"), dgvr.Cells["Status"]);
                SetGridRowAttributes(System.Drawing.Color.LightGreen, MessageConstant.GetText("M000047"), dgvr.Cells["Status"]);
                return;
            }
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
            filePath = filePath.Replace("\\\\", "\\");
            if (File.Exists(filePath))
            {
                DialogResult dr = CABMessageBox.ShowFilterMessage("M000046", "A000001", MessageBoxButtons.YesNo);
                if (dr.Equals(DialogResult.No))
                    goto ReConf;
            }
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                FileUplaod(filePath);
                //this.StatusMessageAsync = MessageConstant.GetText("M000048");
                //SetGridRowAttributes(MessageConstant.GetText("M000048"), dgvr.Cells["Status"]);
                SetGridRowAttributes(System.Drawing.Color.LightGreen, MessageConstant.GetText("M000048"), dgvr.Cells["Status"]);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(Ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "SaveData(string fileText, string meterId)", ex);
            }



        }


        private static bool UploadFile(string filePath, out string resultMessage)
        {
            lock (LockUpload)
            {
                resultMessage = string.Empty;
                bool IsUploaded = false;
                try
                {
                    UploadFile uploadFile = new UploadFile();
                    IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage, null);
                }
                catch (Exception ex)    //Exception log for catch block
                {

                    MessageBox.Show(ex.ToString());
                    logger.Log(LOGLEVELS.Error, "UploadFile(string filePath, out string resultMessage)", ex);
                }
                return IsUploaded;
            }
        }


        /// <summary>
        /// To upload File
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void FileUplaod(string filePath)
        {
            try
            {
                bool IsUploaded = false;

                this.StatusMessageAsync = "Uploading readout file...";
               // SetGridRowAttributes("Uploading readout file...", dgvr.Cells["Status"]);
                SetGridRowAttributes(System.Drawing.Color.LightYellow, "Uploading readout file...", dgvr.Cells["Status"]);
                string resultMessage = string.Empty;
                ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
                //this.Cursor = Cursors.WaitCursor;

                IsUploaded = UploadFile(filePath, out resultMessage);
                if (IsUploaded)
                {
                    //this.ListRefreshAsync = true;
                    //this.RightStatusMessageAsync = String.Empty;
                    //this.StatusMessageAsync = "File Uploaded successfully.";
                    SetGridRowAttributes(System.Drawing.Color.LightGreen, "File Uploaded successfully.", dgvr.Cells["Status"]);
                   // SetGridRowAttributes("File Uploaded successfully", dgvr.Cells["Status"]);
                    //Application.DoEvents();

                }
                else
                {
                    //this.RightStatusMessageAsync = String.Empty;
                    this.StatusMessageAsync = resultMessage;
                    SetGridRowAttributes(resultMessage, dgvr.Cells["Status"]);
                }
                //this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {

                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "FileUplaod(string filePath)", ex);
            }
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

        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                //this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
               // this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
      
      


        /// <summary>
        /// Start Point for Single Thread
        /// </summary>        
        /// <returns></returns>
        public void ReadThreadOne()
        {          
            //Result result = new Result();
            bool isConnected = false;
            string MeterID = string.Empty;
            countProfile = -1;
            try
            {

                int rowCount = this.RowCount;
                string strFileName = this.StrFileName;
                countProfile = rowCount;

                byte totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                {
                    DataGridViewCheckBoxCell chk1 = dgvr.Cells["Select"] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(chk1.Value))
                    {                        
                        string simNumber = dgvr.Cells["SimNo"].Value.ToString();
                        string Message = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                        this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";

                        SetGridRowAttributes(System.Drawing.Color.LightYellow, Message, dgvr.Cells["Status"]);
                        //Application.DoEvents();
                        ChannelInformation channelInfo = new ChannelInformation();
                        channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                        if (ConfigSettings.GetValue("PortName").Contains(","))
                            channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
                        else
                            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                        channelInfo.ModemInfo = simNumber;
                        channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                        channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                        channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                        channelInfo.NoOfRetries = totalRetries;
                        Staticip = simNumber;//txtBoxMeterSIM.Text;                        
                        if (commType == CommunicationType.TCP)
                        {
                            //************************TCP/IP Log********************
                            //logger.Log(LOGLEVELS.Debug, "Reading Meter Data... CommunicationType.TCP" + simNumber);
                            channelInfo.ModemInfo = Staticip;
                            channelInfo.TcpPort = Tcpport;
                        }
                        else
                        {
                            channelInfo.ModemInfo = simNumber;
                        }                       
                        if (ConfigSettings.GetValue("ApplicationContext") == "03")
                            channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter
                        else
                            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                        channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                        channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                        channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                        communication = new Communication(channelInfo);
                       // EnableAbort();
                        Result result = new Result();
                        if (ConfigSettings.GetValue("ApplicationContext") == "03")
                            if (channelInfo.SecurityMechanism == 0x00)
                            {
                                result = communication.OpenSession();
                                if (commType == CommunicationType.TCP && result.ErrorCode == CommunicationErrorType.Success)
                                    SetGridRowAttributes(System.Drawing.Color.LightYellow, "Remote Modem Connected.", dgvr.Cells["Status"]);
                                else
                                {
                                    SetGridRowAttributes(System.Drawing.Color.LightYellow, "Remote Modem not Connected/Timeout", dgvr.Cells["Status"]);
                                    break;
                                }
                                // ************ Read meter ID Start
                                ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x60.0x01.0x00.0xFF", 02);
                                result = communication.Send(profileCommand);
                                //communication.CloseSession();
                                if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    int index = 0;

                                    index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                                    for (int i = 2; i < index + 2; i++)
                                    {
                                        MeterID += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                                    }
                                    SetGridRowAttributes(System.Drawing.Color.LightYellow, "Device ID Received.", dgvr.Cells["Status"]);
                                }
                                // ************Read meter ID Close
                                // result = communication.OpenSession();
                                // ************Read Invocation Counter start
                                profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
                                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    if (ConfigSettings.GetValue("SecurityMechanism") == "01")
                                        profileCommand = new ProfileCommand(01, "00.00.2B.01.02.FF", 02);
                                    else if (ConfigSettings.GetValue("SecurityMechanism") == "02")
                                        profileCommand = new ProfileCommand(01, "00.00.2B.01.03.FF", 02);
                                }

                                result = communication.Send(profileCommand);
                                communication.CloseSession();
                                if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    int index = 0;
                                    long InvoCountValue = 0;
                                    long InitializationCounter = 0;
                                    byte[] incount = new byte[4];

                                    if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                                    {
                                        SetGridRowAttributes(System.Drawing.Color.LightYellow, "Invocation Counter Received.", dgvr.Cells["Status"]);
                                        channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                        communication = new Communication(channelInfo);
                                        securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                        string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                                        int adbyteindex = 0;
                                        for (int i = 1; i < index + 5; i++)
                                        {
                                            incount[adbyteindex++] = result.RecieveDataBuffer[i];

                                        }
                                        
                                        data = FormatData(incount, false);
                                        if (data != null) InvoCountValue = Convert.ToInt64(data);
                                        InitializationCounter = InvoCountValue + 1;

                                        List<string> SecurityKeyDetails = Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                                        //MohsinRaza : below code is added to handle dual key logic for smart meter

                                        if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                                        {
                                            ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                            ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                            ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);
                                        }

                                        result = communication.OpenSessionCipher(InitializationCounter);

                                        if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > 3)
                                        {
                                            ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                            ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[3]);
                                            ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[3]);
                                            result = communication.OpenSessionCipher(InitializationCounter + 1);
                                        }

                                    }
                                }

                            }
                        // ************Read Invocation Counter Close
                        if (ConfigSettings.GetValue("ApplicationContext") != "03")
                            result = communication.OpenSession();
                        if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                        {
                            SetConnectionDetail(true);
                            isConnected = GetMeterData(strFileName, false, 0);
                            if (isConnected == false)
                                communication.CloseSession();
                        }
                        else
                        {
                            if (commType == CommunicationType.DIRECT)
                            {
                                this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                            }
                            else
                            {
                                communication.CloseSession();
                                if (commType == CommunicationType.TCP)
                                {
                                    this.StatusMessageAsync = "Connection " + Staticip + " failed.";
                                }
                                else
                                {
                                    this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                //logger.Log(LOGLEVELS.Error, "Reading Meter Data... CommunicationType.TCP" + RowCount + ex.ToString());
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadThreadOne()", ex);
            }

        }

        #endregion 

    }
}
