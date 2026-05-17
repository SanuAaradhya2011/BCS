#region NameSpaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Common.EntityMapper;
using CAB.BLL;
using CAB.E650MeterConfiguration;
using CAB.E650MeterConfiguration.Entity;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.IECChannel.Programming;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using CAB.IECChannel.ReadOut;
using CAB.Framework.Utility;
using System.Net;
using Hunt.EPIC.Logging;
using System.ComponentModel;
using System.Threading;
using CABApplication;
using CAB.Entity;
using System.Collections;
using System.Linq;
#endregion

namespace CAB.UI
{
    /// <summary>
    /// Provides for Meter Programming features
    /// </summary>
    public partial class E650MeterConfigurations : MdiChildForm
    {
        Thread WriteThread = null;
       
        BackgroundWorker B_Worker;
        #region Constants and Variables
        private System.Resources.ResourceManager resourceMgr;
        private Communication communication;
        private List<int> selectedPushParams = new List<int>();
        private List<int> selectedScrollParams = new List<int>();
        private List<int> selectedHighResParams = new List<int>();
        private byte[] activeSeasonProfile;
        private byte[] activeWeekProfile;
        private byte[] activeDayProfile;
        private byte[] passiveSeasonProfile;
        private byte[] passiveWeekProfile;
        private byte[] passiveDayProfile;
        private byte[] passiveActivationDate;
        private byte[] SpecialDayProfile;
        private DataSet displayParameterRepository;
        DataGridViewCellStyle style;
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
        private bool isAborted = false;
        private bool isValidTOU = true;
        private bool isMeterConnected = false;
        int TOUZone = 0;
        int TOUSlots = 0;
        Thread thOperation = null;
        List<Thread> lstThread = null;
        int rIndex = 0;
        int count = 0;
        int rcount = 0;
        int gIndex = 0;
        List<byte> touData;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650MeterConfigurations).ToString());
        private const string INVALID = "Invalid";
        private const string ZONE = "Zone";
        private const string MONDAY = "Mon";
        private const string TUESDAY = "Tue";
        private const string WEDNESDAY = "Wed";
        private const string THURSDAY = "Thu";
        private const string FRIDAY = "Fri";
        private const string SATURDAY = "Sat";
        private const string SUNDAY = "Sun";
        private const string DAY = "Day";
        private const string Month = "Month";
        private const string TARIFF = "Tariff";
        private const string COLZONE = "colZone";
        private const string COLMONDAY = "colMon";
        private const string COLTUESDAY = "colTue";
        private const string COLWEDNESDAY = "colWed";
        private const string COLTHURSDAY = "colThu";
        private const string COLFRIDAY = "colFri";
        private const string COLSATURDAY = "colSat";
        private const string COLSUNDAY = "colSun";
        private const string COLDAY = "colDay";
        private const string COLMONTH = "colMonth";
        private const string COLSESSION = "colSeason";
        private const string WEEKPROFILE = "Week Profile";
        private string COLTARIFF = "colTariff";
        private string COLSTARTHOUR = "colStartHour";
        private const string STARTHOUR = "Start Hour";
        private string COLSTARTMIN = "colStartMin";
        private const string STARTMIN = "Start Min";
        private const string WEEK = "Week";
        private const string ColSeasonNumberIEC = "SeasonNumber";
        private const string ColSeasonActivationDateIEC = "SeasonActivationDate";
        private byte dayProfileCount = 1;
        private byte weekProfileCount = 1;
        private byte seasonProfileCount = 1;
        private byte SpecialDayProfileCount = 1;
        private const string ONETOU = "ONE";
        private const string TWOTOU = "TWO";
        private const string FOURTOU = "FOUR";
        private const string ThreeSTOU = "THREE";
        
        private const string FOURSPTOU = "FOURSP";
        private const string HOLIDAYTOU = "HOLIDAY";        
        private const string TOU1P = "TOU1P";
        private const string FourSPTOU10Z8S = "FOURSP10Z8S";    
        
        private List<ProfileId> touParameters = new List<ProfileId>();
        private List<ProfileId> displayParameters = new List<ProfileId>();
        bool IsOffline = false;
        DataGridView[] dayProfileGrids;
        public List<System.Enum> enumData;
        private List<System.Enum> listSelectedParams;
        private Serializer serializer = null;
        private static object syncRoot = new object();
        public static MeterConfigSettings meterConfigSettings = null;
        public static MeterOfflineConfigSettings meterOfflineConfigSettings = null;
        DataGridView seasonProfileGrid;
        DataGridView weekProfileGrid;
        DateTimePicker touActivationDate;
        RadioButton rdbTOUType;
        DataGridView SpecialDayProfileGrid;
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private static string simNumber = string.Empty;
        private static string MeterSerialNumber = string.Empty;
        private static string Staticip = string.Empty;
        private static string Tcpport = string.Empty;
        private CommunicationType commType;
        private MeterMasterBLL meterMasterBLL = null;
        private bool flagErrorDisplayParam = false;
        CABAppControl.DisplayParameterIEC objDisplayParameterIECConfig = null;
        string Taskname = DateTime.Now.ToString("yyyyMMddHHmmss");
        public int securitymachanism = 0;
        BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
       
        #endregion

        #region Nested Types

        public enum dgvSimColumn
        {
            /// <summary>
            /// serial Number
            /// </summary>
            SN = 0,
            /// <summary>
            /// Sim Number
            /// </summary>
            SimNo,
            /// <summary>
            /// Meter ID
            /// </summary>
            MeterID,
            /// <summary>
            /// Select
            /// </summary>
            Select,
            /// <summary>
            /// Status
            /// </summary>
            Status,
        }
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public E650MeterConfigurations(bool IsOnline)
        {
            InitializeComponent();
            
            B_Worker = new BackgroundWorker();
            B_Worker.DoWork += new DoWorkEventHandler(B_Worker_DoWork);
           // B_Worker.ProgressChanged += new ProgressChangedEventHandler(B_Worker_ProgressChanged);
            B_Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(B_Worker_RunWorkerCompleted);


            meterMasterBLL = new MeterMasterBLL();
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.E650MeterConfigurations", System.Reflection.Assembly.GetExecutingAssembly());
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.ModemInfo = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
            //to deserialize the xml file
            serializer = new Serializer();
            lock (syncRoot)
            {
                if (meterConfigSettings == null)
                {
                    meterConfigSettings = (MeterConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterConfigSettings.xml"), typeof(MeterConfigSettings));
                }
            }
            commType = GetCommuniactioType();
            if (commType == CommunicationType.DIRECT)
            {
                GsmCommPanel.Visible = false;
                tabRS232LockUnlock.Width = 847;
                tabRS232LockUnlock.Height = 426;
            }
            else
            {
                if (IsOnline)
                {
                    GsmCommPanel.Visible = true;
                    if (commType == CommunicationType.GPRS)
                    {
                        lngSimNumber.Text = "Modem IMEI Number: ";
                        grpSimNumber.Text = "IMEI Number";
                        txtBoxMeterSIM.MaxLength = 15;
                    }
                    if (commType == CommunicationType.TCP)
                    {
                        lngSimNumber.Text = "Static IP";
                        grpSimNumber.Text = "TCP/IP: ";
                        txtPort.Visible = true;
                        lblport.Visible = true;
                        txtBoxMeterSIM.MaxLength = 15;
                    }
                    FillMeterIdSerialNumber();
                }
                else
                {
                    GsmCommPanel.Visible = false;
                    tabRS232LockUnlock.Width = 847;
                    tabRS232LockUnlock.Height = 426;
                }

            }
            if (IsOnline)
            {
                //btnCancel.Location = btnCreateCfgFile.Location;
                btnCreateCfgFile.Visible = false;
                // btnCancel.Location = btnAbort.Location;//For GSM deep
                //btnAbort.Location = btnUploadFile.Location;
                //btnUploadFile.Visible = false;
               // btnAbort.Visible = false;//For GSM deep
                IsOffline = false;
                chkDisplayExtended.Checked = false;
                chkDisplayExtended.Visible = false;
                if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
                {
                    chkDisplayParamSelectAll.Visible = false;
                    chkDisplayParamSelectAll.Checked = false;
                }
            }
            else
            {
                btnCancel.Location = btnUploadFile.Location;
                btnCreateCfgFile.Location = btnWrite.Location;
                btnUploadFile.Location = btnRead.Location;
                btnRead.Visible = false;
                btnWrite.Visible = false;
                IsOffline = true;
                chkDisplayExtended.Checked = false;
                //chkDisplayExtended.Visible = true;
            }
        }
        #endregion

        #region Public Methods


        /// <summary>
        /// Used to validate data in cells of all profile grids
        /// </summary>
        /// <returns></returns>
        public string ValidateTOUGrids()
        {
            string errorMessage = string.Empty;
            errorMessage = ValidateDayTOUGrids();
            //******* Meter Model Change Required Here ***********//
            if (Convert.ToInt16(ConfigInfo.MeterModel) != NamePlateConstants.VBSPNoSeasonNoWeek && Convert.ToInt16(ConfigInfo.MeterModel) != NamePlateConstants.VFSPNoSeasonNoWeek && Convert.ToInt16(ConfigInfo.MeterModel) != NamePlateConstants.VBSPNoSeasonNoWeek)
            {
                errorMessage += ValidateWeekTOUGrids();
                errorMessage += ValidateSeasonTOUGrids();
            }

            return errorMessage;
        }

        /// <summary>
        /// gets meter config data from xml where meter model number and firmware version match.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="firmware"></param>
        /// <returns></returns>
        
        public MeterConfigSettingsMeterConfigElement GetMeterConfig(string meterModel, string firmware)
        {            
            List<byte> meterSupportedConfigData = new List<byte>();
            MeterConfigSettingsMeterConfigElement result = null;
            try
            {
                //case 1 : when we get firmware version and meter model no from the xml.
                //result = meterConfigSettings.Items[0];
                foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
                {
                    if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == firmware.ToString())
                        {
                            result = meterConfigSettingsElement;
                            break;
                        }                   
                }
                //case 2 : when we go to the meter and get the permission for showing rows.
                if (result == null)
                {
                    Result commResult = communication.OpenSession();
                    if (commResult.ErrorCode == CommunicationErrorType.Success)
                    {
                        this.StatusMessage = "Checking For Meter Type,Please Wait...";
                        meterSupportedConfigData = communication.GetPermissionEntity();
                        this.StatusMessage = "";
                        if (meterSupportedConfigData.Count > 0)
                        {
                            result = GetMeterSupportedListOfConfigParameters(meterSupportedConfigData);
                        }
                    }
                    communication.CloseSession();
                }
                //case 3 : when no meter is connected then , default parameters will be shown
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
                communication.CloseSession();
                logger.Log(LOGLEVELS.Error, "GetMeterConfig(string meterModel, string firmware)", ex);
            }
            return result;
        }


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        /// <summary>
        /// Form Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterConfigurations_Load(object sender, EventArgs e)
        {
            try
            {
                txtPort.Text = ConfigSettings.GetValue("TCPPORT");
                string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                if (mode == ReaderMode)
                {
                    btnWrite.Enabled = false;
                }
                //btnAbort.Enabled = false;
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                HideTabs();
                BindTouAndDsiplayparameterEnums();
                timer_RTC.Start();
                BindBillingTypeControls();
                LoadTabs();
                BindTOUGrids();
                if (UtilityDetails.IECSupport && IsOffline)
                {
                    rdbTOUWithHoliday.Visible = true;
                    rdbTOUWithFourSeason1Phase.Visible = true;
                    rbTOUFourSeason1P10Zone8Slots.Visible = true;
                    SetTOUGrids();
                    ResetAllGrids();
                }
                listSelectedParams = new List<System.Enum>();
                if (enumData.Contains(ProfileId.DisplayParameters) && ConfigInfo.DisplayProgrammingVariant!=DisplayProgrammingTypes.TwoByte)
                {
                    BindDisplayParameters();
                }
                MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
                DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
                Configuration = menuStrip.Items["configurationToolStripMenuItem"];
                //for DIP default values
                cmbDIPDemandInterval.Items.Clear();
                cmbDIPDemandInterval.Items.Add("15 (900)");
                cmbDIPDemandInterval.Items.Add("30 (1800)");
                Associate_SP_NDLMS_GridEvent();
                BindDisplayParametersIEC();
                if (IsOffline)
                {
                    gbMeterVariant.Visible = true;
                    rdbThreephase.Checked = true;
                    meterOfflineConfigSettings = (MeterOfflineConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterOfflineConfigSetting.xml"), typeof(MeterOfflineConfigSettings));
                    OfflineConfigurationSetControlValuesOnMeterModelBasis();
                }
                else
                {
                    gbMeterVariant.Visible = false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "E650MeterConfigurations_Load(object sender, EventArgs e)", ex);
            }
        }


      


        private void OfflineConfigurationSetControlValuesOnMeterModelBasis()
        {
            try
            {            
                //Hide All other Tabs
                HideTabs();
                //Set Tab on Meter Configuration basis
                SetTabs(rdbSinglephase.Checked);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "OfflineConfigurationSetControlValuesOnMeterModelBasis()", ex);                
            }            
        }




        private MeterOfflineConfigSettingsMeterConfigElement GetOfflineConfig(string MeterModel)
        {
            MeterOfflineConfigSettingsMeterConfigElement result = null;
            try
            {

                foreach (MeterOfflineConfigSettingsMeterConfigElement meterConfigSettingsElement in meterOfflineConfigSettings.Items)
                {
                    if (meterConfigSettingsElement.MeterModel == MeterModel)
                    {
                        result = meterConfigSettingsElement;
                        break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GetOfflineConfig(string MeterModel)", ex);
            }
            return result;
        }

        private void SetTabs(bool IsSinglePhase)
        {
            string MeterModel = string.Empty;
            MeterOfflineConfigSettingsMeterConfigElement objMeterOfflineConfigSettingsMeterConfigElement = null;
            try
            {
                switch (IsSinglePhase)
                {
                    case true:
                        MeterModel = "1";
                        break;

                    case false:
                        MeterModel = "2";
                        break;

                    default:
                        MeterModel = "0";
                        break;
                }

                objMeterOfflineConfigSettingsMeterConfigElement = GetOfflineConfig(MeterModel);
                if (objMeterOfflineConfigSettingsMeterConfigElement != null)
                {
                    enumData = new List<System.Enum>();
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.DIP))
                    {
                        enumData.Add(ProfileId.DIP);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.DIPWithSliding))
                    {
                        enumData.Add(ProfileId.DIPWithSliding);
                    }                   
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.RTC))
                    {
                        enumData.Add(ProfileId.RTC);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.KvahSelection))
                    {
                        enumData.Add(ProfileId.KvahSelection);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.DisplayParameters))
                    {
                        enumData.Add(ProfileId.DisplayParameters);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.DisplayParametersIEC))
                    {
                        enumData.Add(ProfileId.DisplayParametersIEC);
                    }
                    //-----------------TOD Handling in Offline Mode------------------//
                    if (objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper().Contains(ONETOU) ||
                        objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper().Contains(TWOTOU) ||
                        objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper().Contains(FOURTOU) ||
                        objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper().Contains(FOURSPTOU)||
                        objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper().Contains(ThreeSTOU))// ADD PRADIPTA
                    {
                        enumData.Add(ProfileId.TOU);
                        SetTOUOfflineConfiguration(objMeterOfflineConfigSettingsMeterConfigElement.TOD.ToUpper());
                    }
                    //-----------------TOD Handling in Offline Mode------------------//                  
                    if (objMeterOfflineConfigSettingsMeterConfigElement.BillingType.ToString().ToUpper() == "NORMAL")
                    {
                        enumData.Add(ProfileId.BillingType);                                           
                    }
                    if (objMeterOfflineConfigSettingsMeterConfigElement.BillingType.ToString().ToUpper() == "OTHER")
                    {
                        enumData.Add(ProfileId.BillingType);                       
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.BillingReset))
                    {
                        enumData.Add(ProfileId.BillingReset);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.AutoLock))
                    {
                        enumData.Add(ProfileId.AutoLock);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.LockRS232))
                    {
                        enumData.Add(ProfileId.RS232LockUnlock);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.SIP))
                    {
                        enumData.Add(ProfileId.SIP);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.CTRatio))
                    {
                        enumData.Add(ProfileId.CTRatio);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.PTRatio))
                    {
                        enumData.Add(ProfileId.PTRatio);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.ManualBilling))
                    {
                        enumData.Add(ProfileId.ManualBilling);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.SoftwareBilling))
                    {
                        enumData.Add(ProfileId.SoftwareBilling);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.MagneticTamperIcon))
                    {
                        enumData.Add(ProfileId.MagneticTamperIcon);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.LoadControl))
                    {
                        enumData.Add(ProfileId.LoadControl);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.DisconnectControl))
                    {
                        enumData.Add(ProfileId.DisconnectControl);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.LoadControl1PSmartMeter))
                    {
                        enumData.Add(ProfileId.LoadControl1PSmartMeter);
                    }                   
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.RS485))
                    {
                        enumData.Add(ProfileId.RS485);
                    }
                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.PulseEnergy))
                    {
                        enumData.Add(ProfileId.PulseEnergy);
                    }

                    if (Convert.ToBoolean(objMeterOfflineConfigSettingsMeterConfigElement.ManualButtonMDReset))
                    {
                        enumData.Add(ProfileId.ManualButtonMDReset);
                    }
                    lngGridViewReadControl1.RefreshList(enumData, IsOffline);
                    LoadTabs();       
                }                       
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SetTabs(bool IsSinglePhase)", ex);
            }
        }

        private void SetTOUOfflineConfiguration(string strTOD)
        {
            try
            {
                string[] lstTOD = strTOD.Split(',');               
                //------1Season---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                rdbTOUSeason1.Visible = false;
                //------2Season---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                rdbTOUSeason2.Visible = false;
                //------3Season---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//ADD PRADIPTA
                rdbTOUSession3.Visible = false;
                //------4Season---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                rdbTOUSeason4.Visible = false;
                //------Holiday---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                rdbTOUWithHoliday.Visible = false;
                //------TOUSP---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                rdbTOUSP.Visible = false;
                //------TOU1P---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                rdbTOUWithFourSeason1Phase.Visible = false;
                rbTOUFourSeason1P10Zone8Slots.Visible = false;
                //------FourSPTOU10Z8S---------------
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                rdb10Zone8SlotFutAct.Visible = false;
                foreach (string item in lstTOD)
                {
                    switch (item)
                    {
                        case ONETOU:
                            {
                                tabControlTOUOPeration.TabPages.Add(tabPageTOUSeason1);
                                rdbTOUSeason1.Visible = true;
                                rdbTOUSeason1.Checked = true;
                            }
                            break;
                        case TWOTOU:
                            {                                
                                rdbTOUSeason2.Visible = true;
                                //rdbTOUSession3.Visible = true; // ADD PRADIPTA
                            }
                            break;
                        case ThreeSTOU:// add pradipta
                            {
                                rdbTOUSession3.Visible = true;
                                //rdbTOUSession3.Visible = true; // ADD PRADIPTA
                            }
                            break;
                        case FOURTOU:
                            {                                
                                rdbTOUSeason4.Visible = true;
                            }
                            break;
                        case FOURSPTOU:
                            {                                
                                rdbTOUSP.Visible = true;
                            }
                            break;
                        case HOLIDAYTOU:
                            {                                
                                rdbTOUWithHoliday.Visible = true;
                            }
                            break;
                        case TOU1P:
                            {                                
                                rdbTOUWithFourSeason1Phase.Visible = true;
                                rbTOUFourSeason1P10Zone8Slots.Visible = true;
                            }
                            break;
                        case FourSPTOU10Z8S:
                            {
                                rdb10Zone8SlotFutAct.Visible = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SetTOUOfflineConfiguration(string strTOD)", ex);
            }            
        }   


      
         
        private void BindDisplayParametersIEC()
        {
            try
            {
                objDisplayParameterIECConfig = new CABAppControl.DisplayParameterIEC();
                objDisplayParameterIECConfig.FillDisplayParameters();
                objDisplayParameterIECConfig.Dock = DockStyle.Fill;
                tabDisplayParamIEC.Controls.Add(objDisplayParameterIECConfig);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BindDisplayParametersIEC()", ex);

            }
        }

        private void Associate_SP_NDLMS_GridEvent()
        {
            this.gridTOUDay1_1P_NDLMS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay1_1P_NDLMS_CellValidating);
            this.gridTOUDay1_1P_NDLMS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay1_1P_NDLMS_CellClick);

            this.gridTOUDay2_1P_NDLMS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay1_1P_NDLMS_CellValidating);
            this.gridTOUDay2_1P_NDLMS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay1_1P_NDLMS_CellClick);

            this.gridTOUDay3_1P_NDLMS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay1_1P_NDLMS_CellValidating);
            this.gridTOUDay3_1P_NDLMS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay1_1P_NDLMS_CellClick);

            this.gridTOUDay4_1P_NDLMS.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay1_1P_NDLMS_CellValidating);
            this.gridTOUDay4_1P_NDLMS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay1_1P_NDLMS_CellClick);



        }

        private void InitializeSinglePhaseTouGrid(int Zone, int Slots)
        {
            InitializeGrid(gridDayTables_1P_NDLMS, Zone, Slots);
            InitializeGrid(gridActivationDate_1P_NDLMS, Zone, Slots);
            InitializeGrid(gridTOUDay1_1P_NDLMS, Zone, Slots);
            InitializeGrid(gridTOUDay2_1P_NDLMS, Zone, Slots);
            InitializeGrid(gridTOUDay3_1P_NDLMS, Zone, Slots);
            InitializeGrid(gridTOUDay4_1P_NDLMS, Zone, Slots);
            ResetSinglePhaseNdlmsTouValues(Zone,Slots);
        }


        private void ResetSinglePhaseNdlmsTouValues(int Zone , int Slots)
        {
            try
            {
                FillDefaultGridValues(gridTOUDay1_1P_NDLMS, Zone, Slots);
                FillDefaultGridValues(gridTOUDay2_1P_NDLMS, Zone, Slots);
                FillDefaultGridValues(gridTOUDay3_1P_NDLMS, Zone, Slots);
                FillDefaultGridValues(gridTOUDay4_1P_NDLMS, Zone, Slots);
                FillDefaultGridValues(gridDayTables_1P_NDLMS, Zone, Slots);
                FillDefaultGridValues(gridActivationDate_1P_NDLMS, Zone, Slots);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ResetSinglePhaseNdlmsTouValues(int Zone , int Slots)", ex);
            }
        }


        private void FillDefaultGridValues(DataGridView dgv,int Zone, int Slots)
        {
            try
            {
                int Count = 0;
                if (dgv != null)
                {
                    if (dgv.Rows != null)
                    {
                        dgv.Rows.Clear();
                    }

                    if (dgv.Name.Contains("Tables") || dgv.Name.Contains("Activation"))
                    {
                        Count = 4;
                    }
                    if (dgv.Name.Contains("TOU"))
                    {
                        Count = Zone;
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        dgv.Rows.Add();
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (j == 0 && !dgv.Name.Contains("Activation"))
                            {
                                dgv.Rows[i].Cells[j].Value = Convert.ToString(i + 1);
                            }
                            else
                            {
                                dgv.Rows[i].Cells[j].Value = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillDefaultGridValues(DataGridView dgv,int Zone, int Slots)", ex);
            }
        }


        private void InitializeGrid(DataGridView dgv, int Zone, int Slots)
        {
            try
            {
                if (dgv != null)
                {
                    if (dgv.Rows != null)
                    {
                        dgv.Rows.Clear();
                    }
                    if (dgv.Columns != null)
                    {
                        dgv.Columns.Clear();
                    }

                    if (dgv.Name.Contains("TOU"))
                    {

                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 35;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Zone";
                        gridcombo.HeaderText = "Zone";
                        gridcombo.ReadOnly = true;
                        for (int i = 1; i <= Zone; i++)
                        {
                            gridcombo.Items.Add(i.ToString());
                        }



                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 39;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Tariff";
                        gridcombo1.HeaderText = "Tariff";
                        for (int i = 1; i <= Slots; i++)
                        {
                            gridcombo1.Items.Add("T" + i.ToString());
                        }


                        DataGridViewComboBoxColumn gridcombo2 = new DataGridViewComboBoxColumn();
                        gridcombo2.Width = 39;
                        gridcombo2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo2.Name = "Hours";
                        gridcombo2.HeaderText = "Hours";
                        int index = 0;
                        for (index = 0; index <= 23; index++)
                        {
                            if (index < 10) { gridcombo2.Items.Add("0" + index.ToString()); }
                            else { gridcombo2.Items.Add(index.ToString()); }
                        }

                        DataGridViewComboBoxColumn gridcombo3 = new DataGridViewComboBoxColumn();
                        gridcombo3.Width = 39;
                        gridcombo3.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        gridcombo3.Name = "Minutes";
                        gridcombo3.HeaderText = "Minutes";

                        // Updated index to resolve index expection by mohsin
                        for (index = 0; index < 4; index++)
                        {
                             gridcombo3.Items.Add((index*15).ToString("00"));
                        }



                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                        dgv.Columns.Add(gridcombo2);
                        dgv.Columns.Add(gridcombo3);
                    }
                    if (dgv.Name.Contains("Tables"))
                    {
                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 37;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Zone";
                        gridcombo.HeaderText = "Zone";
                        gridcombo.ReadOnly = true;
                        gridcombo.Items.Add("1");
                        gridcombo.Items.Add("2");
                        gridcombo.Items.Add("3");
                        gridcombo.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 37;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Mon";
                        gridcombo1.HeaderText = "Mon";
                        gridcombo1.Items.Add("1");
                        gridcombo1.Items.Add("2");
                        gridcombo1.Items.Add("3");
                        gridcombo1.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo2 = new DataGridViewComboBoxColumn();
                        gridcombo2.Width = 37;
                        gridcombo2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo2.Name = "Tue";
                        gridcombo2.HeaderText = "Tue";
                        gridcombo2.Items.Add("1");
                        gridcombo2.Items.Add("2");
                        gridcombo2.Items.Add("3");
                        gridcombo2.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo3 = new DataGridViewComboBoxColumn();
                        gridcombo3.Width = 37;
                        gridcombo3.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo3.Name = "Wed";
                        gridcombo3.HeaderText = "Wed";
                        gridcombo3.Items.Add("1");
                        gridcombo3.Items.Add("2");
                        gridcombo3.Items.Add("3");
                        gridcombo3.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo4 = new DataGridViewComboBoxColumn();
                        gridcombo4.Width = 37;
                        gridcombo4.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo4.Name = "Thu";
                        gridcombo4.HeaderText = "Thu";
                        gridcombo4.Items.Add("1");
                        gridcombo4.Items.Add("2");
                        gridcombo4.Items.Add("3");
                        gridcombo4.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo5 = new DataGridViewComboBoxColumn();
                        gridcombo5.Width = 37;
                        gridcombo5.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo5.Name = "Fri";
                        gridcombo5.HeaderText = "Fri";
                        gridcombo5.Items.Add("1");
                        gridcombo5.Items.Add("2");
                        gridcombo5.Items.Add("3");
                        gridcombo5.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo6 = new DataGridViewComboBoxColumn();
                        gridcombo6.Width = 37;
                        gridcombo6.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo6.Name = "Sat";
                        gridcombo6.HeaderText = "Sat";
                        gridcombo6.Items.Add("1");
                        gridcombo6.Items.Add("2");
                        gridcombo6.Items.Add("3");
                        gridcombo6.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo7 = new DataGridViewComboBoxColumn();
                        gridcombo7.Width = 37;
                        gridcombo7.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo7.Name = "Sun";
                        gridcombo7.HeaderText = "Sun";
                        gridcombo7.Items.Add("1");
                        gridcombo7.Items.Add("2");
                        gridcombo7.Items.Add("3");
                        gridcombo7.Items.Add("4");




                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                        dgv.Columns.Add(gridcombo2);
                        dgv.Columns.Add(gridcombo3);
                        dgv.Columns.Add(gridcombo4);
                        dgv.Columns.Add(gridcombo5);
                        dgv.Columns.Add(gridcombo6);
                        dgv.Columns.Add(gridcombo7);
                    }
                    if (dgv.Name.Contains("Activation"))
                    {

                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 40;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Day";
                        gridcombo.HeaderText = "Day";
                        //gridcombo.ReadOnly = true;
                        int index = 0;
                        for (index = 1; index <= 31; index++)
                        {
                            if (index < 10) { gridcombo.Items.Add("0" + index.ToString()); }
                            else { gridcombo.Items.Add(index.ToString()); }
                        }

                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 40;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Month";
                        gridcombo1.HeaderText = "Month";
                        for (index = 1; index <= 12; index++)
                        {
                            if (index < 10) { gridcombo1.Items.Add("0" + index.ToString()); }
                            else { gridcombo1.Items.Add(index.ToString()); }
                        }

                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeGrid(DataGridView dgv, int Zone, int Slots)", ex);
            }
        }



        private void oneToManyGSM_CheckedChanged(object sender, EventArgs e)
        {
            dgvMeterIdAndSim.Enabled = true;
            selectAll.Enabled = true;
            grpSimNumber.Enabled = false;
        }

        private void oneToOneGSM_CheckedChanged(object sender, EventArgs e)
        {
            ResetGrid(true);
            dgvMeterIdAndSim.Enabled = false;
            selectAll.Enabled = false;
            grpSimNumber.Enabled = true;
        }

        /// <summary>
        /// to enable select all button , be checked or unchecked , when a check box is clicked in column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMeterIdAndSim_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvMeterIdAndSim.IsCurrentCellDirty)
            {
                dgvMeterIdAndSim.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            }
        }

        private void dgvMeterIdAndSim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = true;
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {

                if (!(bool)dgvMeterIdAndSim.Rows[count].Cells["Select"].Value)
                {
                    flag = false;
                    break;
                }
            }

            selectAll.CheckedChanged -= selectAll_CheckedChanged;
            if (flag)
            {
                selectAll.Checked = true;
            }
            else
            {
                selectAll.Checked = false;
            }
            selectAll.CheckedChanged += selectAll_CheckedChanged;
        }

        private void selectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAll.Checked)
            {
                for (int count = 0; count < dgvMeterIdAndSim.RowCount; count++)
                {
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = true;
                }
            }
            else
            {
                for (int count = 0; count < dgvMeterIdAndSim.RowCount; count++)
                {
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void BindTouAndDsiplayparameterEnums()
        {
            touParameters.Add(ProfileId.ActiveSeasonProfile);
            touParameters.Add(ProfileId.ActiveWeekProfile);
            touParameters.Add(ProfileId.ActiveDayProfile);
            touParameters.Add(ProfileId.PassiveSeasonProfile);
            touParameters.Add(ProfileId.PassiveWeekProfile);
            touParameters.Add(ProfileId.PassiveDayProfile);
            touParameters.Add(ProfileId.ActivationDate);

            displayParameters.Add(ProfileId.PushDisplayParameter);
            displayParameters.Add(ProfileId.ScrollDisplyParameter);
            displayParameters.Add(ProfileId.HighResolutionDisplayParameter);
            //displayParameters.Add(ProfileId.DisplayTimeoutParameter);// Story - Hide Display Timeout Parameter

        }

        private void FillMeterIdSerialNumber()
        {
            noMeterFoundStatus.Visible = false;
            DataSet dsMeterIdSimNumber = meterMasterBLL.GetMeterIdAndSimNumber(this.commType.ToString());
            if (dsMeterIdSimNumber != null && dsMeterIdSimNumber.Tables != null && dsMeterIdSimNumber.Tables.Count > 0)
            {
                dgvMeterIdAndSim.AutoGenerateColumns = true;
                //dgvMeterIdAndSim.DataSource = dsMeterIdSimNumber.Tables[0];

                DataGridViewColumn dgvSnoColumn = new DataGridViewTextBoxColumn();
                dgvSnoColumn.Name = "SNo";
                dgvSnoColumn.HeaderText = "S.No";
                dgvSnoColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("Sno"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvSnoColumn);
                }

                DataGridViewColumn dgvSimNoColumn = new DataGridViewTextBoxColumn();
                dgvSimNoColumn.Name = "SimNo";
                if (commType == CommunicationType.GPRS)
                {
                    dgvSimNoColumn.HeaderText = "IMEI Number";
                }
                else
                {
                    dgvSimNoColumn.HeaderText = "Sim Number";
                }
                dgvSimNoColumn.MinimumWidth = 100;
                dgvSimNoColumn.Width = 100;
                dgvSimNoColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("SimNo"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvSimNoColumn);
                }


                DataGridViewColumn dgvMeterIDColumn = new DataGridViewTextBoxColumn();
                dgvMeterIDColumn.Name = "MeterID";
                dgvMeterIDColumn.HeaderText = "Meter ID";
                dgvMeterIDColumn.ReadOnly = true;


                if (!dgvMeterIdAndSim.Columns.Contains("MeterID"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvMeterIDColumn);
                }


                DataGridViewColumn dgvColumn = new DataGridViewCheckBoxColumn();
                dgvColumn.Name = "Select";
                dgvColumn.HeaderText = "Select";

                if (!dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvColumn);
                }

                DataGridViewColumn dgvStatusColumn = new DataGridViewTextBoxColumn();
                dgvStatusColumn.Name = "Status";
                dgvStatusColumn.HeaderText = "Meter Read Status";
                dgvStatusColumn.MinimumWidth = 200;
                dgvStatusColumn.Width = 200;
                dgvStatusColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("Status"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvStatusColumn);
                }


                for (int count = 0; count < dsMeterIdSimNumber.Tables[0].Rows.Count; count++)
                {
                    dgvMeterIdAndSim.Rows.Add();

                    dgvMeterIdAndSim.Rows[count].Cells["Sno"].Value = count + 1;
                    dgvMeterIdAndSim.Rows[count].Cells["SimNo"].Value = dsMeterIdSimNumber.Tables[0].Rows[count][1].ToString();
                    dgvMeterIdAndSim.Rows[count].Cells["MeterID"].Value = dsMeterIdSimNumber.Tables[0].Rows[count][0].ToString();
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started.";
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }


                //dgvMeterIdAndSim.Columns["Meter Id"].ReadOnly = true;
                //dgvMeterIdAndSim.Columns["Status"].ReadOnly = true;


                //dgvMeterIdAndSim.Columns["Meter Id"].Width = dgvMeterIdAndSim.Columns["Meter Id"].Width + 20;
                //dgvMeterIdAndSim.Columns["Select"].Width = dgvMeterIdAndSim.Columns["Select"].Width - 40;


                dgvMeterIdAndSim.AllowUserToAddRows = false;
            }
            else
            {

                //dgvMeterIdAndSim.Visible = false;
                if (dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Remove("Select");
                }
                dgvMeterIdAndSim.DataSource = null;
                selectAll.Visible = false;
                noMeterFoundStatus.Visible = true;
                noMeterFoundStatus.Text = "No meter is assigned for " + ConfigSettings.GetValue("ChannelType") + " Connection.";
            }


        }

        /// <summary>
        /// Sets the Style and Status of Grid
        /// </summary>
        private void SetGrid(ProfileId selectedConfigId, System.Drawing.Color color, string status)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = color;
            lngGridViewReadControl1.SetColour(selectedConfigId, style);
            lngGridViewReadControl1.SetStatus(selectedConfigId, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridTOUDay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DayGridCellClick((DataGridView)sender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
        private void gridTOUDay_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ValidateDayProfileCell(sender, e);
        }
        private void gridTOUWeek_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                WeekGridCellClick();
            }
            catch (Exception ex)    //Exception log for catch block
            {

                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "gridTOUWeek_CellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
        }
        private void gridTOUWeek_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ValidateWeekProfileCell(sender, e);
        }
        private void gridTOUSeason_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SeasonGridCellClick();
            }
            catch (Exception ex)    //Exception log for catch block
            {

                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "gridTOUSeason_CellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
        }
        private void gridTOUSeason_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ValidateSeasonProfileCell(sender, e);
        }
        private void btnAutoFill_Click(object sender, EventArgs e)
        {
            AutoFillTOU();
        }
        private void btnResetTOU_Click(object sender, EventArgs e)
        {
            ResetAllTOU();
        }
        /// <summary>
        /// RTC Timer event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_RTC_Tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("en-GB");
            //System.Threading.Thread.CurrentThread.CurrentCulture = c;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = c;
            rtcCtrl.Controls[0].Controls["txtRTC"].Text = System.DateTime.Now.ToString(c);
        }

        /// <summary>
        /// Abort Read/Write
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbort_Click(object sender, EventArgs e)
        {
            isAborted = true;
            AbortMessage();
            //Abort thread that are still alive
            AbortThread(lstThread);
            AbortThread(thOperation);
            AbortThread(WriteThread);
            btnCancel.Enabled = true;
            btnAbort.Enabled = false;
            communication.CloseSession();
            this.StatusMessage = "User Aborted.";
            Application.DoEvents();
            this.Cursor = Cursors.Default;
            SetConnectionDetail(false);
        }
        void AbortMessage()
        {
          bool WriteStatus = false;
          WriteStatus = new GSMConfigBLL().UpdateAbortTaskStatus(MeterSerialNumber, "Aborted", "User Aborted", Taskname);
        
         //for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
         //{
         //    DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
         //    if (dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value.ToString() == "Enqueue")
         //    {
         //        if (Convert.ToBoolean(chk1.Value))
         //            MeterSerialNumber = dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, rowCount].Value.ToString();
         //      dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Aborted";
         //      WriteStatus = new GSMConfigBLL().UpdateTaskStatus(MeterSerialNumber, "Aborted", "User Aborted", Taskname);
         //    }
             
         //}
         
        }
        

        /// <summary>
        /// Write Programming parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
            try
            {
                //DisableControls();
              //  this.Cursor = Cursors.WaitCursor;
                InitialiseGrid(mode);
                if (oneToOneGSM.Checked && commType != CommunicationType.TCP)
                {
                    if (!ValidateSimNumber())
                    {
                        return;
                    }
                }
                else if (oneToOneGSM.Checked && commType == CommunicationType.TCP)
                {
                    if (!ValidateIP())
                    {
                        return;
                    }
                }
                if (CheckValidations("write"))
                {
                    string validationMessage = ValidateConfiguration("write");
                    if (validationMessage.Length == 0)
                    {
                        if (oneToOneGSM.Checked || commType == CommunicationType.DIRECT)
                        {
                            WriteOneToOne();
                        }
                        else if (oneToManyGSM.Checked)
                        {
                            //WriteOneToMany();
                            if (commType == CommunicationType.TCP)
                            {
                                if (Convert.ToBoolean(ConfigSettings.GetValue("IsTCPOneToManyParallel")))
                                {
                                    DisableControls();
                                    thOperation = new Thread(WriteOneToManyParallel);
                                    thOperation.Start();

                                }
                                else
                                {
                                    
                                    WriteOneToMany();
                                }
                            }
                            else
                            {                               
                                DisableControls();
                                WriteThread = new Thread(WriteOneToMany);
                                WriteThread.Start();
                                                                
                            }
                        }
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(validationMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                
                logger.Log(LOGLEVELS.Error, "btnWrite_Click(object sender, EventArgs e)", ex);
            }
          
        }
        void B_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //WriteOneToMany();
        }
        void B_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 

        }

        private int GetDIPDemandTypeSelectedIndex()
        {
            int index = 0;
            try
            {
                index = cmbDIPDemandType.SelectedIndex;
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GetDIPDemandTypeSelectedIndex()", ex);
            }
            return index;
        }


        private string GetDIPDemandTypeSelectedItem()
        {
            string index = string.Empty;
            try
            {
                index = cmbDIPDemandType.SelectedItem.ToString();
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GetDIPDemandTypeSelectedItem()", ex);
            }
            return index;
        }



        private void WriteOneToManyParallel()
        {
            lstThread = new List<Thread>();
             int ThreadPoolSize = Convert.ToInt32(ConfigSettings.GetValue("ThreadPoolSize"));
            string TCPPort = ConfigSettings.GetValue("TCPPORT");           
            try
            {
                Result result = new Result();
                bool isConnected;
                if (ValidateCheckList(ThreadPoolSize))
                {
                    ResetGrid(false);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("write");
                    // To remove selected profile for NORMAL billing type [BillingType_Month]
                    if (normalBillingType.Checked == true)
                    {
                        selectedProfiles.Remove(ProfileId.BillingMonthType);
                    }



                    isMeterConnected = true;
                    for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                    {
                        DataGridViewRow dgvr = dgvMeterIdAndSim.Rows[rowCount];
                        DataGridViewCheckBoxCell chk1 = dgvr.Cells["Select"] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk1.Value))
                        {
                            clsParallelConfiguration objclsParallelConfiguration = null;



                            int _FillDIPData = FillDIPData();
                            int _FillSlideSubDIP = FillSlideSubDIP();
                            int _FillSIPData = FillSIPData();
                            CAB.E650MeterConfiguration.Entity.BillingDateTime _FillBillingTypeData = FillBillingTypeData();
                            byte _FillBillingMonthTypeData = FillBillingMonthTypeData();
                           
                            byte[] _GetSeasonProfileBuffer = GetSeasonProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                            byte[] _GetWeekProfileBuffer = GetWeekProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                            byte[]  _GetSplDayProfileBuffer = GetSplDayProfileBuffer();
                            byte[] _GetDayProfileBuffer = GetDayProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                            byte[] _GetActivationDateBuffer = GetActivationDateBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                            byte _cmbResetLockoutdays = 0;
                            Byte.TryParse(cmbResetLockoutdays.Text,out _cmbResetLockoutdays);

                            List<byte> _GetSelectedRowsinParameterGridPush = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                            List<byte> _GetSelectedRowsinParameterGridScroll = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                            List<byte> _GetSelectedRowsinParameterGridHigh = GetSelectedRowsinParameterGrid(dGVHighResolution);
                         
                            List<byte> _WriteLoadControl = WriteLoadControl();
                            List<byte> _WriteLoadControl1PSmartMeter = WriteLoadControl1PSmartMeter();
                            List<byte> _WriteDisconnectControl = WriteDisconnectControl();
                            List<byte> _WriteRS485 = WriteRS485();
                            int _nudCTRatio = Convert.ToInt32(nudCTRatio.Value);
                            int _nudPTRatio = Convert.ToInt32(nudPTRatio.Value);


                            int _GetDIPDemandTypeSelectedIndex = GetDIPDemandTypeSelectedIndex();
                            string _GetDIPDemandTypeSelectedItem = GetDIPDemandTypeSelectedItem();
                          
                            DateTime _rtcCtrl = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            byte _FillPulseEnergyTypeData = FillPulseEnergyTypeData();

                             objclsParallelConfiguration = new clsParallelConfiguration(rowCount, selectedProfiles, commType, dgvr, TCPPort, CommunicationMode.Normal, _FillDIPData, _FillSlideSubDIP, _FillSIPData, _FillBillingTypeData, _FillBillingMonthTypeData, _cmbResetLockoutdays, rdbKVAhLagOnly.Checked, rdbRS232Lock.Checked, rdbAutoLock.Checked, _GetSeasonProfileBuffer, _GetWeekProfileBuffer, _GetSplDayProfileBuffer, _GetDayProfileBuffer, _GetActivationDateBuffer, _GetSelectedRowsinParameterGridPush, _GetSelectedRowsinParameterGridScroll,_GetSelectedRowsinParameterGridHigh, _nudCTRatio, _nudPTRatio, rdbEnableManualBilling.Checked, rdbEnableSoftwareBilling.Checked, _WriteLoadControl, _WriteLoadControl1PSmartMeter, _WriteDisconnectControl, _WriteRS485, _GetDIPDemandTypeSelectedIndex, _GetDIPDemandTypeSelectedItem, chkDisconnect.Checked, chkconnect.Checked,
                            _rtcCtrl);



                            Thread th = new Thread(new ThreadStart(objclsParallelConfiguration.WriteThreadOne));
                            th.Start();

                            lstThread.Add(th);
                        }
                    }

                    //Wait MainThread till Child are executing
                    foreach (Thread th in lstThread)
                    {
                        th.Join();
                    }

                    //Wait MainThread till Child are executing
                    /*while (IsChildActive(lstThread))
                    {
                        Thread.Sleep(10000);
                    }*/




                }
                else
                {
                    if (commType == CommunicationType.GPRS)
                    {
                        MessageBox.Show("Select atleast 1 IMEI number to read & You can not select more than ThreadPoolSize = " + ThreadPoolSize, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (commType == CommunicationType.TCP)
                    {
                        MessageBox.Show("Select atleast 1 IP to read & You can not select more than ThreadPoolSize = " + ThreadPoolSize, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Select atleast 1 SIM number to read & You can not select more than ThreadPoolSize = " + ThreadPoolSize, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                //************************TCP/IP Log********************
                logger.Log(LOGLEVELS.Debug, "Reading Meter Data..." + ex.ToString());
            }
            finally
            {
                isMeterConnected = false;
                //Abort thread that are still alive
                AbortThread(lstThread);
                //Cleanup
                EnableControls();               
            }
        }



        private void AbortThread(Thread th)
        {
            try
            {
                if (th != null && th.IsAlive)
                {
                    th.Abort();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "AbortThread(Thread th)", ex);
            }
        }




        private void AbortThread(List<Thread> lstThread)
        {
            try
            {
                foreach (var item in lstThread)
                {
                    if (item != null && item.IsAlive)
                    {
                        try
                        {
                            item.Abort();
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {

                            logger.Log(LOGLEVELS.Error, "AbortThread(List<Thread> lstThread)", ex);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AbortThread(List<Thread> lstThread)", ex);
            }
        }




        /// <summary>
        /// 
        /// </summary>
      private void WriteOneToOne()
       {
            try
            {
                DisableControls();
                int meterModelNumber = 0;
                string MeterID = string.Empty;
                simNumber = txtBoxMeterSIM.Text;
                Staticip = txtBoxMeterSIM.Text;
                Tcpport = txtPort.Text;
                if (commType != CommunicationType.DIRECT)
                {
                    if (commType == CommunicationType.TCP)
                    {
                        this.StatusMessageAsync = "Connecting " + Staticip + " ...";
                    }
                    else
                    {
                        this.StatusMessageAsync = "Connecting " + simNumber + " ...";
                    }
                   
                }
                ChannelInformation channelInfo = new ChannelInformation();
                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                if (commType == CommunicationType.TCP)
                {
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
                if (commType == CommunicationType.DIRECT)
                {
                    channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                }
                else
                {
                    // Fixing the no of retries to 1 in case of remote communication
                    channelInfo.NoOfRetries = 1;
                }
                communication = new Communication(channelInfo);
                Result result = new Result();
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                    if (channelInfo.SecurityMechanism == 0x00)
                    {
                        result = communication.OpenSession();
                        if (commType == CommunicationType.TCP && result.ErrorCode == CommunicationErrorType.Success)
                        {
                            this.StatusMessageAsync = "Remote Modem Connected";
                        }
                        // ************Read Invocation Counter start
                        ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
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
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                communication = new Communication(channelInfo);
                                securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                                int adbyteindex = 0;
                                for (int i = 1; i < index + 5; i++)
                                {
                                    incount[adbyteindex++] = result.RecieveDataBuffer[i];

                                }
                                data = Convertstring.FormatData(incount, false);
                                if (data != null) InvoCountValue = Convert.ToInt64(data);
                                InitializationCounter = InvoCountValue + 1;

                                List<string> SecurityKeyDetails = CABApplication.Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                                //MohsinRaza : below code is added to handle dual key logic for smart meter

                                if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);
                                }
                                else //-------------Set to Default if Meter ID not found in EndDeviceSecurtityResponse file---------------
                                {
                                    logger.Log(LOGLEVELS.Error, "ReadOneToOne()--> Setting Security Keys To Default for meter ID :" + MeterID + " due to Null return from EndDeviceSecurityResponse");
                                    ConfigSettings.ChangeNode("ModePassword", ConfigSettings.GetValue("DefaultModePassword"));
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", ConfigSettings.GetValue("DefaultGlobalEncryptionKey"));
                                    ConfigSettings.ChangeNode("AuthenticationKey", ConfigSettings.GetValue("DefaultAuthenticationKey"));
                                }

                                result = communication.OpenSessionCipher(InitializationCounter);
                                //----------------CC Error Case handeling for retry with additional key received--------------------------
                                if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > 3 && SecurityKeyDetails[3].Trim().Length > 0)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[3]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[3]);
                                    result = communication.OpenSessionCipher(InitializationCounter + 1);
                                }
                            }
                        }
                    }
                if (ConfigSettings.GetValue("ApplicationContext") != "03")
                     result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                {
                    isMeterConnected = true;
                    SetConnectionDetail(true);
                    communication.GetDisplayProgrammingVariant();
                    Int32.TryParse(ConfigInfo.MeterModel, out meterModelNumber);
                    List<ProfileCommand> lstProfileCommands = GetProfileCommandEntity();
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("write");
                    // To remove selected profile for NORMAL billing type [BillingType_Month]
                    if (normalBillingType.Checked == true)
                    {
                        selectedProfiles.Remove(ProfileId.BillingMonthType);
                    }
                    WriteMeterConfigData(selectedProfiles, false, 0);
                }
                else
                {
                    if (commType == CommunicationType.DIRECT)
                    {
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteOneToOne()", ex);
            }
            finally
            {
                communication.CloseSession();
                isMeterConnected = false;               
                //Cleanup
                EnableControls();
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteOneToMany()
        {
            try
            {
               // DisableControls();
                Result result = new Result();
                EnableAbort();
                //string Taskname = DateTime.Now.ToString("yyyyMMddHHmmss");
                bool isConnected;
                if (ValidateGrid())
                {
                    ResetGrid(false);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("write");
                    // To remove selected profile for NORMAL billing type [BillingType_Month]
                    if (normalBillingType.Checked == true)
                    {
                        selectedProfiles.Remove(ProfileId.BillingMonthType);
                    }
                    byte totalRetries;
                    if (commType == CommunicationType.DIRECT)
                    {
                        totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    }
                    else
                    {
                        // Fixing the no of retries to 1 in case of remote communication
                        totalRetries = 1;
                    }

                    for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                    {
                        DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                        bool DBStatus = false;
                        simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();

                        MeterSerialNumber = dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, rowCount].Value.ToString();
                        if (Convert.ToBoolean(chk1.Value))
                        {
                         DBStatus = new GSMConfigBLL().InsertData(Convert.ToInt32(MeterSerialNumber), simNumber, "Pending...", "Reading Not start", Taskname);
                        }
                    }
                    for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                    {
                        for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                        {
                            DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                            style = new DataGridViewCellStyle();
                            bool WriteStatus = false;
                            if (Convert.ToBoolean(chk1.Value))
                            {
                                simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                                
                                MeterSerialNumber = dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, rowCount].Value.ToString();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                    : "Connecting " + simNumber + " ...";
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                                this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                                Application.DoEvents();
                                ChannelInformation channelInfo = new ChannelInformation();
                                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                                channelInfo.ModemInfo = simNumber;
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                                channelInfo.NoOfRetries = totalRetries;
                                Staticip = simNumber;//txtBoxMeterSIM.Text;
                                Tcpport = txtPort.Text;
                                if (commType == CommunicationType.TCP)
                                {
                                    channelInfo.ModemInfo = Staticip;
                                    channelInfo.TcpPort = Tcpport;
                                }
                                else
                                {
                                    channelInfo.ModemInfo = simNumber;
                                }
                                communication = new Communication(channelInfo);
                                result = communication.OpenSession();
                                SetConnectionDetail(true);
                                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    communication.GetDisplayProgrammingVariant();
                                    isMeterConnected = true;
                                    isConnected = WriteMeterConfigData(selectedProfiles, true, rowCount);
                                    if (isConnected)
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Write completed.";
                                        this.StatusMessageAsync = "Write completed.";
                                        dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;
                                        logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "" + simNumber + " Write completed.");
                                        WriteStatus = new GSMConfigBLL().UpdateTaskStatus(MeterSerialNumber, "Pass", "Write completed", Taskname);
                                       

                                    }
                                    else
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Write failed.";
                                        logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "" + simNumber + "Write failed.");
                                        this.StatusMessageAsync = "Write failed.";
                                        WriteStatus = new GSMConfigBLL().UpdateTaskStatus(MeterSerialNumber, "Fail", "Meter not responding", Taskname);
                                    }

                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.";
                                    logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.");
                                    this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                    WriteStatus = new GSMConfigBLL().UpdateTaskStatus(MeterSerialNumber,"Fail", "Modem not connected", Taskname);
                                }
                                communication.CloseSession();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast 1 SIM number to write!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteOneToMany()", ex);
            }
            finally
            {
                isMeterConnected = false;
                if (WriteThread.IsAlive)
                // WriteThread.Abort();
                //Cleanup
                EnableControls();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        private void InitialiseGrid(string mode)
        {
            this.StatusMessage = "";
            foreach (ProfileId profile in enumData)
            {
                if (profile == ProfileId.BillingReset || profile == ProfileId.MagneticTamperIcon3P)
                {
                    SetGrid(profile, System.Drawing.Color.White, "Cannot Be Read");
                }
                else
                {
                    if (mode == ReaderMode)
                        SetGrid(profile, System.Drawing.Color.White, "Reading Not Started");
                    else
                        SetGrid(profile, System.Drawing.Color.White, "Reading/Writing Not Started");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisableControls()
        {
            //if (commType != CommunicationType.DIRECT)
            //{
            //    dgvMeterIdAndSim.Enabled = false;
            //}
            selectAll.Enabled = false;
            btnCancel.Enabled = false;
            btnWrite.Enabled = false;
            btnRead.Enabled = false;
            btnUploadFile.Enabled = false;
            DataAcquisition.Enabled = false;
            Configuration.Enabled = false;
        }





        /// <summary>
        /// 
        /// </summary>
        private void EnableControls()
        {
            try
            {
                string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                if (btnCancel.InvokeRequired)
                {
                    btnCancel.BeginInvoke(new MethodInvoker(EnableControls));
                }
                else
                {
                    btnCancel.Enabled = true;
                }


                if (btnRead.InvokeRequired)
                {
                    btnRead.BeginInvoke(new MethodInvoker(EnableControls));
                }
                else
                {
                    btnRead.Enabled = true;
                }

                if (btnUploadFile.InvokeRequired)
                {
                    btnUploadFile.BeginInvoke(new MethodInvoker(EnableControls));
                }
                else
                {
                    btnUploadFile.Enabled = true;
                }


                if (btnWrite.InvokeRequired)
                {
                    btnWrite.BeginInvoke(new MethodInvoker(EnableControls));
                }
                else
                {
                    btnWrite.Enabled = (mode == ReaderMode) ? false : true;
                }

                if (selectAll.InvokeRequired)
                {
                    selectAll.BeginInvoke(new MethodInvoker(EnableControls));
                }
                else
                {
                    selectAll.Enabled = true;
                }


                if (commType != CommunicationType.DIRECT)
                {
                    if (dgvMeterIdAndSim.InvokeRequired)
                    {
                        dgvMeterIdAndSim.BeginInvoke(new MethodInvoker(EnableControls));
                    }
                    else
                    {
                        dgvMeterIdAndSim.Enabled = true;
                    }
                }


                Action a = () =>
                    {
                        this.Cursor = Cursors.Default;

                        DataAcquisition.Enabled = true;
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }



                SetConnectionDetail(false);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "EnableControls()", ex);
                throw ex;
                
            }
           
        }
        /// Read programming parameters 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
            try
            {
                DisableControls();
                this.Cursor = Cursors.WaitCursor;
                if(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData).Contains(ProfileId.DisplayParameters))
                    BindDisplayParameters();
                InitialiseGrid(mode);
                if (oneToOneGSM.Checked && commType != CommunicationType.TCP)
                {
                    if (!ValidateSimNumber())
                    {
                        return;
                    }
                }
                else if (oneToOneGSM.Checked && commType == CommunicationType.TCP)
                {
                    if (!ValidateIP())
                    {
                        return;
                    }
                }
                if (CheckValidations("read"))
                {
                    if (oneToOneGSM.Checked || commType == CommunicationType.DIRECT)
                    {
                        ReadOneToOne();
                    }
                    else if (oneToManyGSM.Checked)
                    {
                        ReadOneToMany();
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnRead_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                //Cleanup
                EnableControls();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadOneToOne()
        {
            try
            {
                string MeterID = string.Empty;
                simNumber = txtBoxMeterSIM.Text;
                Staticip = txtBoxMeterSIM.Text;
                Tcpport = txtPort.Text;
                if (commType != CommunicationType.DIRECT)
                {
                    if (commType == CommunicationType.TCP)
                    {
                        this.StatusMessageAsync = "Connecting " + Staticip + " ...";
                    }
                    else
                    {
                      this.StatusMessageAsync = "Connecting " + simNumber + " ...";
                    }
                    
                }
                ChannelInformation channelInfo = new ChannelInformation();
                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                if (commType == CommunicationType.TCP)
                {
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
                Result result = new Result();
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                    if (channelInfo.SecurityMechanism == 0x00)
                    {
                        result = communication.OpenSession();
                        if (commType == CommunicationType.TCP && result.ErrorCode == CommunicationErrorType.Success)
                        {
                            this.StatusMessageAsync = "Remote Modem Connected";
                        }                       
                        // ************Read Invocation Counter start
                        ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
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
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                communication = new Communication(channelInfo);
                                securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                                int adbyteindex = 0;
                                for (int i = 1; i < index + 5; i++)
                                {
                                    incount[adbyteindex++] = result.RecieveDataBuffer[i];

                                }
                                data = Convertstring.FormatData(incount, false);
                                if (data != null) InvoCountValue = Convert.ToInt64(data);
                                InitializationCounter = InvoCountValue + 1;

                                List<string> SecurityKeyDetails = CABApplication.Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                                //MohsinRaza : below code is added to handle dual key logic for smart meter

                                if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);
                                }
                                else //-------------Set to Default if Meter ID not found in EndDeviceSecurtityResponse file---------------
                                {
                                    logger.Log(LOGLEVELS.Error, "ReadOneToOne()--> Setting Security Keys To Default for meter ID :" + MeterID + " due to Null return from EndDeviceSecurityResponse");
                                    ConfigSettings.ChangeNode("ModePassword", ConfigSettings.GetValue("DefaultModePassword"));
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", ConfigSettings.GetValue("DefaultGlobalEncryptionKey"));
                                    ConfigSettings.ChangeNode("AuthenticationKey", ConfigSettings.GetValue("DefaultAuthenticationKey"));
                                }

                                result = communication.OpenSessionCipher(InitializationCounter);
                                //----------------CC Error Case handeling for retry with additional key received--------------------------
                                if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > 3 && SecurityKeyDetails[3].Trim().Length > 0)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[3]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[3]);
                                    result = communication.OpenSessionCipher(InitializationCounter + 1);
                                }

                            }
                        }

                    }

                if (ConfigSettings.GetValue("ApplicationContext") != "03")
                    result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                {
                    isMeterConnected = true;
                    SetConnectionDetail(true);
                    communication.GetDisplayProgrammingVariant();
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("read");
                    bool isConnected = GetMeterConfigData(selectedProfiles, false, 0);
                }
                else
                {
                    if (commType == CommunicationType.DIRECT)
                    {
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        if (commType == CommunicationType.TCP)
                        {
                            this.StatusMessageAsync = "Connection " + Staticip + " failed.";
                        }
                        else
                            this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadOneToOne()", ex);
            }
            finally
            {
                communication.CloseSession();
                isMeterConnected = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ReadOneToMany()
        {
            try
            {
                Result result;
                if (ValidateGrid())
                {
                    ResetGridonetomany(false);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("read");
                    byte totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    int rowindex = 0;
                    for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                    {
                        DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk1.Value))
                        {
                            rowindex++;
                        }
                    }
                    if (rowindex > 1)
                    {
                        MessageBox.Show("For configuration read, Select single meter only");
                        return;
                    }
                    
                    for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                    {
                        for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                        {
                            DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;                           
                            if (Convert.ToBoolean(chk1.Value))
                            {
                                simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                    : "Connecting " + simNumber + " ...";
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                                this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                                Application.DoEvents();
                                ChannelInformation channelInfo = new ChannelInformation();
                                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                                channelInfo.ModemInfo = simNumber;
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                                channelInfo.NoOfRetries = totalRetries;
                                Staticip = simNumber;//txtBoxMeterSIM.Text;
                                Tcpport = txtPort.Text;
                                if (commType == CommunicationType.TCP)
                                {
                                    channelInfo.ModemInfo = Staticip;
                                    channelInfo.TcpPort = Tcpport;
                                }
                                else
                                {
                                    channelInfo.ModemInfo = simNumber;
                                }
                                communication = new Communication(channelInfo);
                                result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    isMeterConnected = true;
                                    SetConnectionDetail(true);
                                    communication.GetDisplayProgrammingVariant();
                                    if (GetMeterConfigData(selectedProfiles, true, rowCount))
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout completed.";
                                        dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;                                       
                                    }
                                    else
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout failed.";                                       
                                    }

                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.";
                                    this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                }
                                communication.CloseSession();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast 1 SIM number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadOneToMany()", ex);
            }
            finally
            {
                isMeterConnected = false;
            }
        }
        private bool ValidateIP()
        {
            bool flag = true;
            long portNumber = 0;
            IPAddress ipAddress = null;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!IPAddress.TryParse(txtBoxMeterSIM.Text, out ipAddress))
            {
                CABMessageBox.ShowFilterMessage("M000121", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!long.TryParse(txtPort.Text, out portNumber))
            {
                CABMessageBox.ShowFilterMessage("M000122", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else
            {

            }
            return flag;
        }



        /// <summary>
        /// Billing Type Combobox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxBillingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBoxBillingHour.Items.Clear();
            cmbBoxBillingMinute.Items.Clear();
            cmbBoxBillingDate.Items.Clear();
            if (cmbBoxBillingPeriod.SelectedIndex == 1)
            {
                for (int date = 1; date <= 28; date++)
                {
                    cmbBoxBillingDate.Items.Add(date.ToString().PadLeft(2, '0'));
                }

                for (int hour = 0; hour <= 23; hour++)
                {
                    cmbBoxBillingHour.Items.Add(hour.ToString().PadLeft(2, '0'));
                }

                for (int minute = 0; minute <= 59; minute++)
                {
                    cmbBoxBillingMinute.Items.Add(minute.ToString().PadLeft(2, '0'));
                }
                //cmbBoxBillingHour.SelectedIndex = 0;
                //cmbBoxBillingMinute.SelectedIndex = 0;
                //cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = true;
                cmbBoxBillingMinute.Enabled = true;
                cmbBoxBillingDate.Enabled = true;
                // cmbResetLockoutdays.SelectedIndex = 0;
            }
            else
            {
                cmbBoxBillingDate.Items.Add("01");
                cmbBoxBillingHour.Items.Add("00");
                cmbBoxBillingMinute.Items.Add("00");
                //cmbBoxBillingHour.SelectedIndex = 0;
                //cmbBoxBillingMinute.SelectedIndex = 0;
                //cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = false;
                cmbBoxBillingMinute.Enabled = false;
                cmbBoxBillingDate.Enabled = false;
                //cmbResetLockoutdays.SelectedIndex = 0;
                Application.DoEvents();
            }
            cmbBoxBillingHour.SelectedIndex = 0;
            cmbBoxBillingMinute.SelectedIndex = 0;
            cmbBoxBillingDate.SelectedIndex = 0;
            cmbResetLockoutdays.SelectedIndex = 0;

        }

        /// <summary>
        /// Selected index changed of display paremeter tab control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlDisplayParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageDisplayTimeOut", StringComparison.OrdinalIgnoreCase))
            {
                btnUpScroll.Visible = false;
                btnDownScroll.Visible = false;
                chkDisplayParamSelectAll.Visible = false;
            }
            else
            {
                btnUpScroll.Visible = true;
                btnDownScroll.Visible = true;
                if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.OneByte)
                {
                    chkDisplayParamSelectAll.Visible = true;
                }
            }

            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPagePushButton", StringComparison.OrdinalIgnoreCase) && dGVPushDisplayParams.Columns.Count > 0)
            {
                dGVPushDisplayParams.Columns["SNO"].Width = 80;
                dGVPushDisplayParams.Columns["ID"].Width = 80;
                dGVPushDisplayParams.Columns["Description"].Width = 200;
                dGVPushDisplayParams.Columns["colInclude"].Width = 85;

            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageScrollButton", StringComparison.OrdinalIgnoreCase) && dGVScrollDisplayParams.Columns.Count > 0)
            {

                dGVScrollDisplayParams.Columns["SNO"].Width = 80;
                dGVScrollDisplayParams.Columns["ID"].Width = 80;
                dGVScrollDisplayParams.Columns["Description"].Width = 200;
                dGVScrollDisplayParams.Columns["colInclude"].Width = 85;

            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageHighResolution", StringComparison.OrdinalIgnoreCase) && dGVHighResolution.Columns.Count>0)
            {
                dGVHighResolution.Columns["ID"].Width = 80;
                dGVHighResolution.Columns["Description"].Width = 200;
                dGVHighResolution.Columns["colInclude"].Width = 85;

            }

        }
        /// <summary>
        /// Select all check box handler for disply parameters .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDisplayParamSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView dgvTemp = null;
            bool status = chkDisplayParamSelectAll.Checked ? true : false;
            if (tabControlDisplayParams.SelectedIndex == 0)
            {
                dgvTemp = dGVPushDisplayParams;
            }
            if (tabControlDisplayParams.SelectedIndex == 1)
            {
                dgvTemp = dGVScrollDisplayParams;
            }
            if (tabControlDisplayParams.SelectedIndex == 2)
            {
                dgvTemp = dGVHighResolution;
            }
            for (int i = 0; i < dgvTemp.Rows.Count; i++)
            {
                dgvTemp.Rows[i].Cells["colInclude"].Value = status;
            }
        }
        /// <summary>
        /// Cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVScrollDisplayParams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVScrollDisplayParams, e);
        }
        /// <summary>
        /// Cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVHighResolution_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVHighResolution, e);
        }
        /// <summary>
        /// Cell Content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVPushDisplayParams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVPushDisplayParams, e);
        }

        #region CheckboxEventHandler
        /// <summary>
        /// select all event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkMDWithIP.Checked = true;
                chkKVARSelcetion.Checked = true;
                chkDisplayParam.Checked = true;
                chkTOD.Checked = true;
                chkRTC.Checked = true;
                chkBillingReset.Checked = true;
                chkBilingType.Checked = true;
                chkKVARSelcetion.Checked = true;
                chkLockRS232.Checked = true;
                chkAutoLock.Checked = true;
            }
            else
            {
                chkMDWithIP.Checked = false;
                chkKVARSelcetion.Checked = false;
                chkDisplayParam.Checked = false;
                chkTOD.Checked = false;
                chkRTC.Checked = false;
                chkBillingReset.Checked = false;
                chkBilingType.Checked = false;
                chkKVARSelcetion.Checked = false;
                chkLockRS232.Checked = false;
                chkAutoLock.Checked = false;
            }
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRTC_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkMDWithIP.Checked && chkKVARSelcetion.Checked && chkDisplayParam.Checked && chkTOD.Checked
                 && chkRTC.Checked && chkBillingReset.Checked && chkBilingType.Checked && chkKVARSelcetion.Checked && chkLockRS232.Checked)
            {
                chkSelectAll.Checked = true;
            }
            else
            {
                chkSelectAll.Checked = false;
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoLock_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkMDWithIP.Checked && chkKVARSelcetion.Checked && chkDisplayParam.Checked && chkTOD.Checked
                 && chkRTC.Checked && chkAutoLock.Checked && chkBillingReset.Checked && chkBilingType.Checked && chkKVARSelcetion.Checked && chkLockRS232.Checked)
            {
                chkSelectAll.Checked = true;
            }
            else
            {
                chkSelectAll.Checked = false;
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }

        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMDWithIP_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkKVARSelcetion_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDisplayParam_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTOD_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }

        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBilingType_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBillingReset_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLSCapturePeriod_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLockRS232_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageHighResolution_Enter(object sender, EventArgs e)
        {
            CheckAndUpdateSelectAll(dGVHighResolution);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPagePushButton_Enter(object sender, EventArgs e)
        {
            CheckAndUpdateSelectAll(dGVPushDisplayParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageScrollButton_Enter(object sender, EventArgs e)
        {
            CheckAndUpdateSelectAll(dGVScrollDisplayParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpScroll_Click(object sender, EventArgs e)
        {
            DataGridView dgvDisplayParams = null;
            if (tabControlDisplayParams.SelectedIndex == 0)
            {
                dgvDisplayParams = dGVPushDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 1)
            {
                dgvDisplayParams = dGVScrollDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 2)
            {
                dgvDisplayParams = dGVHighResolution;
            }

            int SelRow = dgvDisplayParams.CurrentRow.Index;// dGVPushDisplayParams.SelectedRows[0].Index;
            if (SelRow > 0)
            {

                String tempDispID, tempDispInfo;
                bool currentColInclude = false;
                tempDispID = dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value.ToString();
                tempDispInfo = dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value.ToString();
                if (dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value != null)
                {
                    currentColInclude = Convert.ToBoolean(dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value);
                }
                dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value = dgvDisplayParams.CurrentRow.Cells["ID"].Value;
                dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value = dgvDisplayParams.CurrentRow.Cells["Description"].Value;
                dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value = dgvDisplayParams.CurrentRow.Cells["colInclude"].Value;
                dgvDisplayParams.CurrentRow.Cells["colInclude"].Value = currentColInclude.ToString();
                dgvDisplayParams.CurrentRow.Cells["ID"].Value = tempDispID;
                dgvDisplayParams.CurrentRow.Cells["Description"].Value = tempDispInfo;

                dgvDisplayParams.ClearSelection();
                dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Selected = true;
                dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownScroll_Click(object sender, EventArgs e)
        {
            String tempDispID, tempDispInfo;
            DataGridView dgvDisplayParams = null;
            bool colInclude = false;
            try
            {
                if (tabControlDisplayParams.SelectedIndex == 0)
                    dgvDisplayParams = dGVPushDisplayParams;
                else if (tabControlDisplayParams.SelectedIndex == 1)
                    dgvDisplayParams = dGVScrollDisplayParams;
                else if (tabControlDisplayParams.SelectedIndex == 2)
                    dgvDisplayParams = dGVHighResolution;

                int SelRow = dgvDisplayParams.CurrentRow.Index;// dGVPushDisplayParams.SelectedRows[0].Index;
                if (SelRow != dgvDisplayParams.Rows.Count - 1)
                {
                    if (SelRow < dgvDisplayParams.Rows.Count - 1)
                    {

                        tempDispID = dgvDisplayParams.Rows[SelRow + 1].Cells[2].Value.ToString();
                        tempDispInfo = dgvDisplayParams.Rows[SelRow + 1].Cells[3].Value.ToString();
                        if (dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value != null)
                        {
                            colInclude = Convert.ToBoolean(dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value);
                        }
                        dgvDisplayParams.Rows[SelRow + 1].Cells[2].Value = dgvDisplayParams.CurrentRow.Cells[2].Value;
                        dgvDisplayParams.Rows[SelRow + 1].Cells[3].Value = dgvDisplayParams.CurrentRow.Cells[3].Value;
                        dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value = dgvDisplayParams.CurrentRow.Cells["colInclude"].Value;
                        dgvDisplayParams.CurrentRow.Cells[2].Value = tempDispID;
                        dgvDisplayParams.CurrentRow.Cells[3].Value = tempDispInfo;
                        dgvDisplayParams.CurrentRow.Cells["colInclude"].Value = colInclude;
                        dgvDisplayParams.ClearSelection();
                        dgvDisplayParams.Rows[SelRow + 1].Cells[2].Selected = true;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnDownScroll_Click(object sender, EventArgs e)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoScrollTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoScrollTime.Checked == true)
            {
                txtScrollResumeTime.Enabled = true;
            }
            else
            {
                txtScrollResumeTime.Text = "";
                txtScrollResumeTime.Enabled = false;
            }
        }
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            DataAcquisition.Enabled = true;
            Configuration.Enabled = true;
        }
        /// <summary>
        /// Validate SIM Number 
        /// </summary>
        /// <returns></returns>
        private bool ValidateSimNumber()
        {
            bool flag = true;
            long simNumber = 0;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            else if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            if (commType == CommunicationType.GPRS)
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 15)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    flag = false;
                }
            }           
            else
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 10)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    flag = false;
                }
            }
            return flag;

        }

        /// <summary>
        /// Validate cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvDayProfile.CurrentCell.IsInEditMode == true)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (e.FormattedValue == null)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                        }
                        if (e.FormattedValue.ToString() != "")
                        {
                            if (e.RowIndex == 0)
                            {
                                dgvDayProfile.Rows[e.RowIndex].Cells[2].Value = "00";
                                dgvDayProfile.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 1; colCount <= dgvWeekProfile.ColumnCount - 1; colCount++)
                                {
                                    dgvWeekProfile.Rows[0].Cells[colCount].Value = "01";
                                }
                                dgvSeasonProfile.Rows[0].Cells[0].Value = "01";
                                dgvSeasonProfile.Rows[0].Cells[1].Value = "01";
                                dgvSeasonProfile.Rows[0].Cells[2].Value = "01";

                            }
                        }
                        rcount = dgvDayProfile.CurrentCell.RowIndex;
                        if (dgvDayProfile.Rows[rcount].Cells[1].Value == null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null
                            || dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dgvDayProfile.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dgvDayProfile.Rows[rcount].Cells[1].Value != null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null &&
                            dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                    }
                    if (e.ColumnIndex == 2)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                        }
                        if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            e.Cancel = true;
                        }
                        else if (Convert.ToInt16(e.FormattedValue) > 23)
                        {
                            e.Cancel = true;
                        }

                        else
                        {

                        }
                        if (e.RowIndex != 9 && dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < 10; count++)
                                    {
                                        dgvDayProfile.Rows[count].ReadOnly = true;
                                    }
                                }

                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "45")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < 10; count++)
                                        {
                                            dgvDayProfile.Rows[count].ReadOnly = true;
                                        }

                                    }

                                }
                            }
                        }
                        if (dgvDayProfile.Rows[rcount].Cells[1].Value != null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null
                            || dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }
                    }
                    if (e.ColumnIndex == 3)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }

                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 45)
                        {
                            e.Cancel = true;
                        }

                        if (e.RowIndex != 9 && dgvDayProfile.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))//<=
                        {
                            if (dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)    //Exception log for catch block
            {
                dgvDayProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                e.Cancel = true;
                logger.Log(LOGLEVELS.Error, "dgvDayProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)", ex);
               //throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeekProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvWeekProfile[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            string gridVal = e.FormattedValue.ToString();
                            if (gridVal == "")
                            {
                                dgvWeekProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                            }
                            else
                            {
                                if (Convert.ToInt16(gridVal) < 1 || Convert.ToInt16(gridVal) > 6)
                                {
                                    dgvWeekProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                    e.Cancel = true;
                                }
                                else
                                {
                                    dgvWeekProfile.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        if (e.RowIndex == 0 && e.ColumnIndex == 7)
                        {
                            dgvWeekProfile.Rows[e.RowIndex].Cells[0].Value = "01";
                            dgvWeekProfile.Rows[e.RowIndex].Cells[1].Value = "01";
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "dgvWeekProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSeasonProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvSeasonProfile[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }

                            if (e.FormattedValue.ToString() == "") { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 31))
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0 && e.FormattedValue.ToString() != ""
                                && e.FormattedValue.ToString() != null
                                && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value) > 29)
                            {
                                if (Convert.ToInt16(e.FormattedValue) == 2)
                                {
                                    dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (e.FormattedValue.ToString() == "")
                            { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 12))
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != 0 && e.FormattedValue != null && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value)
                                    <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }

                            if (e.RowIndex != seasonProfileCount - 1 && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value)
                                    >= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0
                                && e.RowIndex != seasonProfileCount - 1
                                && e.FormattedValue != null
                                && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.FormattedValue != null && e.FormattedValue.ToString() != "" && Convert.ToInt16(e.FormattedValue) == 2)
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value) == 29)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (e.RowIndex != 0 && e.ColumnIndex == 0)
                    {
                        if (e.RowIndex != 0 && e.FormattedValue != null
                            && e.FormattedValue.ToString() != ""
                            && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                        {
                            if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    dgvSeasonProfile[1, count].ReadOnly = true;
                                    count++;
                                }
                                return;
                            }
                            else
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    dgvSeasonProfile[1, count].ReadOnly = false;
                                    count++;
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "dgvSeasonProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DayGridCellClick((DataGridView)sender);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "dgvDayProfile_CellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
        }
        /// <summary>
        /// week grid cell click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeekProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView weekProfileGrids = dgvWeekProfile;
            try
            {
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                if (rIndex != 0 && dgvSeasonProfile.Rows[rIndex - 1].Cells[1].Value == null)
                {
                    int rowIndex = rIndex;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.Rows[rIndex].ReadOnly = true;
                        rowIndex++;
                    }
                    return;
                }
                else
                {
                    int rowIndex = rIndex + 1;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.ReadOnly = false;
                        rowIndex++;
                    }
                }

                int colIndex = weekProfileGrids.CurrentCell.ColumnIndex;
                if (colIndex != 0 && (weekProfileGrids.Rows[rIndex].Cells[colIndex - 1].Value == null))
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].Value = null;
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = true;
                    return;
                }
                else
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = false;
                }

                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    if (dgvDayProfile.Rows[0].Cells[1].Value == null
                        && dgvDayProfile.Rows[0].Cells[2].Value == null
                        && dgvDayProfile.Rows[0].Cells[3].Value == null)
                    {
                        weekProfileGrids.ReadOnly = true;
                        break;
                    }
                    else
                    {
                        weekProfileGrids.ReadOnly = false;

                    }
                }
                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rcount = 0; rcount < 9; rcount++)
                    {
                        if ((dgvDayProfile.Rows[rcount].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rcount].Cells[3].Value != null)
                            && (dgvDayProfile.Rows[rcount + 1].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rcount + 1].Cells[3].Value != null))
                        {
                            if ((dgvDayProfile.Rows[rcount].Cells[2].Value.ToString() == dgvDayProfile.Rows[rcount + 1].Cells[2].Value.ToString())
                                && (Convert.ToInt16(dgvDayProfile.Rows[rcount].Cells[3].Value) >= Convert.ToInt16(dgvDayProfile.Rows[rcount + 1].Cells[3].Value)))
                            {
                                while (rcount < 8)
                                {
                                    dgvDayProfile.Rows[rcount + 2].ReadOnly = true;
                                    rcount++;
                                }
                                weekProfileGrids.ReadOnly = true;
                                dgvSeasonProfile.ReadOnly = true;
                                return;
                            }
                        }
                    }
                }
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                count = weekProfileGrids.CurrentCell.ColumnIndex;
                if (count >= 2)
                {
                    if (weekProfileGrids.Rows[rIndex].Cells[count].Value == null)
                    {
                        if (weekProfileGrids.Rows[rIndex].Cells[count - 1].Value == null)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = true;
                            dgvSeasonProfile.Rows[rIndex].ReadOnly = true;
                        }
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = true;
                            count++;
                        }
                        return;
                    }
                    else
                    {
                        weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = false;
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = false;
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //throw ex;
                logger.Log(LOGLEVELS.Error, "dgvWeekProfile_CellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
            finally
            {
                weekProfileGrids.Columns[0].ReadOnly = true;
            }
        }
        /// <summary>
        /// Reset tou configuration data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetTOUConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rCount = 0; rCount < dgvDayProfile.RowCount; rCount++)
                    {
                        for (int cCount = 1; cCount < dgvDayProfile.ColumnCount; cCount++)
                        {
                            dgvDayProfile.Rows[rCount].Cells[cCount].Value = null;
                        }
                    }
                }
                for (int rCount = 0; rCount < weekProfileCount; rCount++)
                {
                    for (int cCount = 1; cCount < dgvWeekProfile.ColumnCount; cCount++)
                    {
                        dgvWeekProfile.Rows[rCount].Cells[cCount].Value = null;
                    }
                }

                for (int rCount = 0; rCount < seasonProfileCount; rCount++)
                {
                    for (int cCount = 0; cCount < dgvSeasonProfile.ColumnCount; cCount++)
                    {
                        dgvSeasonProfile.Rows[rCount].Cells[cCount].Value = null;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "btnResetTOUConfiguration_Click(object sender, EventArgs e)", ex);
            }
            dtpFutureActivationDate.Value = DateTime.Now;
        }
        /// <summary>
        /// write tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteTOU_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// read current tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCurrentTOU_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Read future tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFutureTOU_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Demand type change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDemandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDemandType.SelectedItem.ToString() == "Block Demand")
            {
                cmbDemandSubInterlavTime.SelectedIndex = -1;

            }
            else
            {

                if (cmbDemandInterval.Text == "15")
                {
                    cmbDemandSubInterlavTime.Text = "5";
                }
                else if (cmbDemandInterval.Text == "30")
                {
                    cmbDemandSubInterlavTime.SelectedItem = "10";
                }

            }

        }

        /// <summary>
        /// Interval type change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDemandInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDemandType.Text != "Block Demand")
            {
                if (cmbDemandInterval.Text == "15")
                {
                    cmbDemandSubInterlavTime.Text = "5";
                }
                else if (cmbDemandInterval.Text == "30")
                {
                    cmbDemandSubInterlavTime.SelectedItem = "10";
                }
            }

        }

        /// <summary>
        /// Create CFG File 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCfgFile_Click(object sender, EventArgs e)
        {
            try
            {
                 masterEntity.MeterData = new MeterDataEntity();
                string MeterModelNumber = masterEntity.General.MeterModelNo;
                this.StatusMessage = "";
                if (CheckValidations("create configuration file"))
                {
                    string validationMessage = ValidateConfiguration("Config Write");
                    if (validationMessage.Length == 0)
                    {
                        this.StatusMessage = string.Empty;
                        string fileLocation = GetFileName();
                        if (!string.IsNullOrEmpty(fileLocation))
                        {
                            FileStream file = new FileStream(fileLocation, FileMode.Create);
                            StreamWriter writer = new StreamWriter(file);
                            if (writer != null)
                            {
                                writer.WriteLine("DLMS");
                                List<System.Enum> selectedProfiles;
                                List<ProfileCommand> lstProfileCommands;
                                ProfileCommand selectedCommand;
                                int meterModelNumber = 0;
                                this.StatusMessage = "";
                                txterrorLog.Text = "";
                                Application.DoEvents();
                                this.Cursor = Cursors.WaitCursor;
                                int touTimes = 1;
                                try
                                {
                                    lstProfileCommands = GetProfileCommandEntity();
                                    selectedProfiles = GetSelectedProfileId("write");
                                    // To remove selected profile for NORMAL billing type [BillingType_Month]
                                    if (normalBillingType.Checked == true)
                                    {
                                        selectedProfiles.Remove(ProfileId.BillingMonthType);
                                    }
                                    foreach (ProfileId selectedConfigId in selectedProfiles)
                                    {

                                        //Filter one command entity
                                        List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                        {
                                            return profileCommandEntity.TagNumber == (int)selectedConfigId                                           ;
                                        });

                                        //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                                        if (selectedConfigId == ProfileId.RTC)
                                        {
                                            ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                                            rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                                            profileCommand.Add(rtcCommand);                                                                                   
                                        }

                                        if (profileCommand.Count > 0)
                                        {
                                            selectedCommand = profileCommand[0];
                                            selectedCommand.Action = ActionType.WRITE;

                                            //Fill WriteData buffer for corresponding programming parameter
                                            switch (selectedConfigId)
                                            {

                                                case ProfileId.DIP:
                                                    {
                                                        profileCommand[0].WriteDataBuffer = FillDIPData();
                                                        profileCommand[1].WriteDataBuffer = FillSlideSubDIP();
                                                    }
                                                    break;
                                                case ProfileId.SIP:
                                                    profileCommand[0].WriteDataBuffer = FillSIPData();// Convert.ToInt32(cmbBoxLSCapturePeriod.Text);
                                                    break;
                                                case ProfileId.RTC:
                                                    profileCommand[0].WriteDataBuffer = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                            "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                    break;
                                                case ProfileId.BillingReset:
                                                    //No need to send any data for MD reset
                                                    profileCommand[0].Action = ActionType.RESET;                                                   
                                                    break;
                                                case ProfileId.BillingType:
                                                    profileCommand[0].WriteDataBuffer = FillBillingTypeData();
                                                    break;
                                                case ProfileId.BillingMonthType:
                                                    profileCommand[0].WriteDataBuffer = FillBillingMonthTypeData();
                                                    break; //[BillingType_Month]
                                                case ProfileId.ResetLockOutDays:
                                                    profileCommand[0].WriteDataBuffer = Convert.ToByte(cmbResetLockoutdays.Text);
                                                    break;
                                                case ProfileId.KvahSelection:
                                                    profileCommand[0].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                                    profileCommand[1].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                                    break;
                                                case ProfileId.RS232LockUnlock:
                                                    profileCommand[0].WriteDataBuffer = rdbRS232Lock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                    break;
                                                case ProfileId.AutoLock:
                                                    profileCommand[0].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                                    profileCommand[1].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                                    break;
                                                case ProfileId.PassiveSeasonProfile:
                                                    if (touTimes == 1)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetSeasonProfileICEToDLMS()
                                                            : GetSeasonProfileBuffer(NamePlateConstants.PumaLTE650Value);
                                                    }
                                                    else if (touTimes == 2)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetSeasonProfileICEToDLMS()
                                                                : GetSeasonProfileBuffer(NamePlateConstants.RubyE250Value);
                                                    }
                                                    else
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetSeasonProfileICEToDLMS()
                                                            : GetSeasonProfileBuffer(NamePlateConstants.SapphireS2);
                                                    }

                                                    break;
                                                case ProfileId.PassiveWeekProfile:
                                                    if (touTimes == 1)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetWeekProfileBufferIECToDLMS()
                                                            : GetWeekProfileBuffer(NamePlateConstants.PumaLTE650Value);
                                                    }
                                                    else if (touTimes == 2)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetWeekProfileBufferIECToDLMS()
                                                              : GetWeekProfileBuffer(NamePlateConstants.RubyE250Value);

                                                    }
                                                    else
                                                    {
                                                        profileCommand[0].WriteDataBuffer = rdbTOUWithHoliday.Checked ? GetWeekProfileBufferIECToDLMS()
                                                           : GetWeekProfileBuffer(NamePlateConstants.SapphireS2);
                                                    }
                                                    break;
                                                case ProfileId.PassiveDayProfile:
                                                    if (touTimes == 1)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(NamePlateConstants.PumaLTE650Value);
                                                    }
                                                    else if(touTimes == 2)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(NamePlateConstants.RubyE250Value);
                                                    }
                                                    else
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(NamePlateConstants.SapphireS2);
                                                    }
                                                    break;
                                                case ProfileId.ActivationDate:
                                                    if (touTimes == 1)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(NamePlateConstants.PumaLTE650Value);
                                                    }
                                                    else if (touTimes == 2)
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(NamePlateConstants.RubyE250Value);
                                                    }
                                                    else
                                                    {
                                                        profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(NamePlateConstants.SapphireS2);
                                                    }
                                                    // profileCommand[0].WriteDataBuffer = GetActivationDateBuffer();
                                                    touTimes++;
                                                    break;
                                                case ProfileId.SpecialDayProfileSmartMeter:
                                                    profileCommand[0].WriteDataBuffer = GetSplDayProfileBuffer();
                                                    break;
                                                case ProfileId.PushDisplayParameter:
                                                    profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                                                    break;
                                                case ProfileId.ScrollDisplyParameter:
                                                    profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                                                    break;
                                                case ProfileId.HighResolutionDisplayParameter:
                                                    profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVHighResolution);
                                                    break;
                                                // Story - Hide Display Timeout Parameter
                                                //case ProfileId.DisplayTimeoutParameter:
                                                //profileCommand[0].WriteDataBuffer = GetDisplayTimeoutData();
                                                //break;
                                                case ProfileId.CTRatio:
                                                    profileCommand[0].WriteDataBuffer = Convert.ToInt32(nudCTRatio.Value);
                                                    break;
                                                case ProfileId.PTRatio:
                                                    profileCommand[0].WriteDataBuffer = Convert.ToInt32(nudPTRatio.Value);
                                                    break;
                                                case ProfileId.ManualBilling:
                                                    profileCommand[0].WriteDataBuffer = rdbEnableManualBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                    break;
                                                case ProfileId.SoftwareBilling:
                                                    profileCommand[0].WriteDataBuffer = rdbEnableSoftwareBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                    profileCommand[1].WriteDataBuffer = rdbEnableSoftwareBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                    break;
                                                case ProfileId.LoadControl:
                                                     profileCommand[0].WriteDataBuffer =  WriteLoadControl();
                                                    break;
                                                case ProfileId.DisconnectControl:
                                                    {
                                                        if (chkconnect.Checked)
                                                        {
                                                            profileCommand[0].Attribute = 0x02;
                                                        }
                                                        else if (chkDisconnect.Checked)
                                                        {
                                                            profileCommand[0].Attribute = 0x01;
                                                        }
                                                        profileCommand[0].WriteDataBuffer = WriteDisconnectControl();
                                                        profileCommand[0].Action = ActionType.ACTIONREQUEST;
                                                    }
                                                    break;                                                    
                                                case ProfileId.LoadControl1PSmartMeter:
                                                     profileCommand[0].WriteDataBuffer = WriteLoadControl1PSmartMeter();
                                                    break;
                                                case ProfileId.RS485:
                                                    profileCommand[0].WriteDataBuffer = WriteRS485();
                                                    break;
                                                case ProfileId.PulseEnergy:
                                                    profileCommand[0].WriteDataBuffer = FillPulseEnergyTypeData();
                                                    break;
                                                case ProfileId.ManualButtonMDReset:                                                    
                                                   profileCommand[0].WriteDataBuffer = rdbMDResetEnable.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                    break;

                                                default:
                                                    break;
                                                    
                                            }
                                            if (selectedConfigId == ProfileId.DIP)
                                            {
                                                if (cmbDIPDemandType.SelectedItem.ToString() == "Sliding Demand")
                                                {
                                                    //Sliding Case
                                                    //Profile[0]
                                                    writer.Write(String.Format("{0:X2}", profileCommand[0].ClassId) + profileCommand[0].ObisCode.Replace(".", "") +
                                                  String.Format("{0:X2}", profileCommand[0].Attribute));
                                                    foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[0].ClassName, profileCommand[0].WriteDataBuffer))
                                                    {
                                                        writer.Write(String.Format("{0:X2}", bufferByte));
                                                    }
                                                    writer.WriteLine();
                                                    //Profile[1]
                                                    writer.Write(String.Format("{0:X2}", profileCommand[1].ClassId) + profileCommand[1].ObisCode.Replace(".", "") +
                                                  String.Format("{0:X2}", profileCommand[1].Attribute));
                                                    foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[1].ClassName, profileCommand[1].WriteDataBuffer))
                                                    {
                                                        writer.Write(String.Format("{0:X2}", bufferByte));
                                                    }
                                                    writer.WriteLine();
                                                }
                                                else
                                                {
                                                    //Block Case
                                                    //Profile[1]
                                                    writer.Write(String.Format("{0:X2}", profileCommand[1].ClassId) + profileCommand[1].ObisCode.Replace(".", "") +
                                                  String.Format("{0:X2}", profileCommand[1].Attribute));
                                                    foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[1].ClassName, profileCommand[1].WriteDataBuffer))
                                                    {
                                                        writer.Write(String.Format("{0:X2}", bufferByte));
                                                    }
                                                    writer.WriteLine();
                                                    //Profile[0]
                                                    writer.Write(String.Format("{0:X2}", profileCommand[0].ClassId) + profileCommand[0].ObisCode.Replace(".", "") +
                                                  String.Format("{0:X2}", profileCommand[0].Attribute));
                                                    foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[0].ClassName, profileCommand[0].WriteDataBuffer))
                                                    {
                                                        writer.Write(String.Format("{0:X2}", bufferByte));
                                                    }
                                                    writer.WriteLine();
                                                }
                                            }
                                            else
                                            {
                                                if (profileCommand.Count > 1)
                                                {
                                                    for (int i = 0; i < profileCommand.Count; i++)
                                                    {
                                                        writer.Write(String.Format("{0:X2}", profileCommand[i].ClassId) + profileCommand[i].ObisCode.Replace(".", "") +
                                                               String.Format("{0:X2}", profileCommand[i].Attribute));
                                                        foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[i].ClassName, profileCommand[i].WriteDataBuffer))
                                                        {
                                                            writer.Write(String.Format("{0:X2}", bufferByte));

                                                        }
                                                        writer.WriteLine();
                                                    }

                                                }
                                                else
                                                {
                                                    writer.Write(String.Format("{0:X2}", profileCommand[0].ClassId) + profileCommand[0].ObisCode.Replace(".", "") +
                                                   String.Format("{0:X2}", profileCommand[0].Attribute));
                                                    foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[0].ClassName, profileCommand[0].WriteDataBuffer))
                                                    {
                                                        writer.Write(String.Format("{0:X2}", bufferByte));
                                                    }
                                                }
                                                writer.WriteLine();
                                            }
                                        }

                                    }

                                    writer.WriteLine("NONDLMS");
                                    WriteIECConfigDataToCFGFile(writer, selectedProfiles);

                                    this.StatusMessage = "CFG File Created Successfully.";
                                }
                                catch (Exception ex)    //Exception log for catch block
                                {
                                    this.StatusMessage = "Error occured while creating file CFG file.";
                                    logger.Log(LOGLEVELS.Error, "btnCreateCfgFile_Click(object sender, EventArgs e)", ex);
                                }
                                finally
                                {
                                    if (writer != null)
                                    {
                                        writer.Close();
                                    }
                                    this.Cursor = Cursors.Default;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(validationMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnCreateCfgFile_Click(object sender, EventArgs e)", ex);

            }
        }
        /// <summary>
        /// Upload CFG file ti fill UI controls .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            flagErrorDisplayParam = true;
            this.StatusMessage = "";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "Configuration File";
            openFile.InitialDirectory = ConfigInfo.GetLocation();
            openFile.Filter = "Configuration file(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            lngGridViewReadControl1.DeselectCheckBoxes();
            timer_RTC.Start();
             BindTOUGrids();
            BindBillingTypeControls();
            LoadTabs();
            listSelectedParams = new List<System.Enum>();
            //if (enumData.Contains(ProfileId.DisplayParameters))
            //{
            //    BindDisplayParameters();
            //}            

            try
            {
                if (result == DialogResult.OK)
                {
                    if (DisplayConfigurationFromFile(openFile.FileName))
                    {
                        this.StatusMessage = resourceMgr.GetString("Upload");
                    }
                    // Defect #219731
                    // HideTabs(listSelectedParams);
                }
            }

            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(" Invalid File ", "BCS");
                logger.Log(LOGLEVELS.Error, "btnUploadFile_Click(object sender, EventArgs e)", ex);
            }

            lngGridViewReadControl1.SelectUploadedParameters(listSelectedParams);
        }
        /// <summary>
        /// Current TOD 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbCurrentTOD_CheckedChanged(object sender, EventArgs e)
        {
            //if(
            //FillSeasonProfileParameters(activeSeasonProfile);
            //FillWeekProfileParameters(activeWeekProfile);
            //FillDayProfileParameters(activeDayProfile,NamePlateConstants.PumaLTE650Value);
            //FillTOUActivationDate(passiveActivationDate);
        }
        /// <summary>
        /// Future TOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbTOUType_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }
        /// <summary>
        /// Refresh status message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRS232LockUnlock_MouseClick(object sender, MouseEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
        private void E650MeterConfigurations_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMeterConnected)
            {
                e.Cancel = true;
            }
            else
            {
                this.StatusMessage = "";
                SetConnectionDetail(false);
                //Abort thread that are still alive
                AbortThread(lstThread);
                AbortThread(thOperation);
               // AbortThread(WriteThread);
            }
        }
        /// <summary>
        /// Check CT Ratio max value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudCTRatio_KeyUp(object sender, KeyEventArgs e)
        {
            int maxVal = 320;
            if (nudCTRatio.Value > maxVal)
            {
                nudCTRatio.Value = maxVal;
            }
        }
        /// <summary>
        /// Check PTratio Max value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPTRatio_KeyUp(object sender, KeyEventArgs e)
        {
            int maxVal = 300;
            if (nudPTRatio.Value > nudPTRatio.Maximum)
            {
                nudPTRatio.Value = maxVal;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngGridViewReadControl1_Load(object sender, EventArgs e)
        {
            enumData = new List<System.Enum>();
            MeterConfigSettingsMeterConfigElement element = GetMeterConfig(ConfigInfo.MeterModel, ConfigInfo.FirmwareVersion);
            // HTCT Specific check
            if (ConfigInfo.MeterModel == "10")
            {
                tabRS232LockUnlock.TabPages["tabkvarSelection"].Text = "Mvah Selection";
                groupBox59.Text = "Mvah Selection";
            }
            // Integrating HTCT in Offline programming
            if (IsOffline)
            {
                tabRS232LockUnlock.TabPages["tabkvarSelection"].Text = "Kvah/Mvah Selection";
                groupBox59.Text = "Kvah/Mvah Selection";
            }
            element = element == null ? GetMeterConfig("0", "0.00") : element;
            if (element != null)
            {
                // Showing the Offline TOU form in case of Remote Online as well
                if (IsOffline || commType != CommunicationType.DIRECT)
                {
                    rdbTOUSeason1.Visible = true;
                    rdbTOUSeason2.Visible = true;
                    rdbTOUSession3.Visible = true;// add pradipta
                    rdbTOUSeason4.Visible = true;
                    rdbTOUSP.Visible = true;
                    otherBillingType.Visible = true;
                    normalBillingType.Visible = true;
                }
                if (Convert.ToBoolean(element.DIP))
                {
                    enumData.Add(ProfileId.DIP);
                }
                if (Convert.ToBoolean(element.DIPWithSliding))
                {
                    enumData.Add(ProfileId.DIPWithSliding);
                }
                if (Convert.ToBoolean(element.KvahSelection))
                {
                    enumData.Add(ProfileId.KvahSelection);
                }
                if (Convert.ToBoolean(element.DisplayParameters))
                {
                    enumData.Add(ProfileId.DisplayParameters);
                }
                if (Convert.ToBoolean(element.DisplayParametersIEC))
                {
                    enumData.Add(ProfileId.DisplayParametersIEC);
                }
                if (element.TOD.ToUpper() == ONETOU)
                {
                    enumData.Add(ProfileId.TOU);
                    rdbTOUSeason1.Visible = true;
                }
                if (element.TOD.ToUpper() == TWOTOU)
                {
                    enumData.Add(ProfileId.TwoTOU);
                    rdbTOUSeason2.Visible = true;
                    if (!IsOffline)
                    {
                        rdbTOUSeason2.Location = rdbTOUSeason1.Location;
                    }
                }

                if (element.TOD.ToUpper() == "THREE")//ADD pradipta 
                {
                    enumData.Add(ProfileId.ThreeSTOU);
                    rdbTOUSession3.Visible = true;
                    if (!IsOffline)
                    {
                        rdbTOUSession3.Location = rdbTOUSeason2.Location;
                    }
                }

                if (element.TOD.ToUpper() == FOURTOU)
                {
                    enumData.Add(ProfileId.FourTOU);
                    rdbTOUSeason4.Visible = true;
                    if (!IsOffline)
                    {
                        rdbTOUSeason4.Location = rdbTOUSeason1.Location;
                    }
                }
                if (element.TOD.ToUpper() == FOURSPTOU)
                {
                    enumData.Add(ProfileId.FourSPTOU);
                    rdbTOUSP.Visible = true;
                    if (!IsOffline)
                    {
                        rdbTOUSP.Location = rdbTOUSeason1.Location;
                    }
                }
                if (element.TOD.ToUpper() == FourSPTOU10Z8S)
                {
                    enumData.Add(ProfileId.FourSPTOU10Z8S);
                    rdb10Zone8SlotFutAct.Visible = true;
                    if (!IsOffline)
                    {
                        rdb10Zone8SlotFutAct.Location = rdbTOUSeason1.Location;
                    }
                }
              
                if (Convert.ToBoolean(element.RTC))
                {
                    enumData.Add(ProfileId.RTC);
                }
                //if (Convert.ToBoolean(element.BillingType))
                //{
                //    enumData.Add(ProfileId.BillingType);
                //}
                if (element.BillingType.ToString().ToUpper() == "NORMAL")
                {
                    enumData.Add(ProfileId.BillingType);
                    normalBillingType.Visible = true;
                    normalBillingType.Checked = true;
                    billingPeriodPanel.Visible = false;
                    label23.Visible = true;
                    label24.Visible = true;
                    cmbBoxBillingHour.Visible = true;
                    cmbBoxBillingMinute.Visible = true;
                    if (!IsOffline)
                        otherBillingType.Visible = false;
                }
                if (element.BillingType.ToString().ToUpper() == "OTHER")
                {
                    enumData.Add(ProfileId.BillingType);
                    otherBillingType.Visible = true;
                    otherBillingType.Checked = true;
                    otherBillingType.Location = normalBillingType.Location;
                    billingPeriodPanel.Visible = true;
                    label23.Visible = false;
                    label24.Visible = false;
                    cmbBoxBillingHour.Visible = false;
                    cmbBoxBillingMinute.Visible = false;

                }
                if (Convert.ToBoolean(element.BillingReset))
                {
                    enumData.Add(ProfileId.BillingReset);
                }
                if (Convert.ToBoolean(element.AutoLock))
                {
                    enumData.Add(ProfileId.AutoLock);
                }
                if (Convert.ToBoolean(element.LockRS232))
                {
                    enumData.Add(ProfileId.RS232LockUnlock);
                }
                if (Convert.ToBoolean(element.SIP))
                {
                    enumData.Add(ProfileId.SIP);
                }
                if (Convert.ToBoolean(element.CTRatio))
                {
                    enumData.Add(ProfileId.CTRatio);

                }
                if (Convert.ToBoolean(element.PTRatio))
                {
                    enumData.Add(ProfileId.PTRatio);

                }
                if (Convert.ToBoolean(element.ManualBilling))
                {
                    enumData.Add(ProfileId.ManualBilling);
                }
                if (Convert.ToBoolean(element.SoftwareBilling))
                {
                    enumData.Add(ProfileId.SoftwareBilling);
                }
                if (Convert.ToBoolean(element.MagneticTamperIcon))
                {
                    enumData.Add(ProfileId.MagneticTamperIcon);
                }
                // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter having specific right authority to reset
                if (Convert.ToBoolean(element.MagneticTamperIcon3P))
                {
                    enumData.Add(ProfileId.MagneticTamperIcon3P);
                }
                if (Convert.ToBoolean(element.LoadControl))
                {
                    enumData.Add(ProfileId.LoadControl);
                }

                if (Convert.ToBoolean(element.DisconnectControl))
                {
                    enumData.Add(ProfileId.DisconnectControl);
                }
                if (Convert.ToBoolean(element.LoadControl1PSmartMeter))
                {
                    enumData.Add(ProfileId.LoadControl1PSmartMeter);
                }
                if (Convert.ToBoolean(element.RS485))
                {
                    enumData.Add(ProfileId.RS485);
                }
                if (Convert.ToBoolean(element.PulseEnergy))
                {
                    enumData.Add(ProfileId.PulseEnergy);
                }
                if (Convert.ToBoolean(element.ManualButtonMDReset))
                {
                    enumData.Add(ProfileId.ManualButtonMDReset);
                }
            }
            lngGridViewReadControl1.AddEnumList(enumData, IsOffline);
            if (!IsOffline)
            {
                foreach (System.Enum item in enumData)
                {
                    style = new DataGridViewCellStyle();
                    style.BackColor = System.Drawing.Color.White;
                    lngGridViewReadControl1.SetColour(item, style);
                    string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                    if (mode == ReaderMode)
                    {
                        if (item.GetDisplayName() == "Billing Reset"  || item.GetDisplayName() == "Tamper Reset")
                        {
                            lngGridViewReadControl1.SetStatus(item, "Cannot Be Read");                            
                        }
                        else
                        {
                            lngGridViewReadControl1.SetStatus(item, "Reading Not Started");
                        }
                       
                    }
                    else
                    {
                        if (item.GetDisplayName() == "Billing Reset" || item.GetDisplayName() == "Tamper Reset")
                        {
                            lngGridViewReadControl1.SetStatus(item, "Cannot Be Read");
                        }
                        else
                        {
                            lngGridViewReadControl1.SetStatus(item, "Reading/Writing Not Started");
                        }                        
                    }
                }
            }
            else
            {
                lngGridViewReadControl1.DisableStatusColumn();
            }
            lngGridViewReadControl1.DeselectCheckBoxes();
            lngGridViewReadControl1.SetDefaultCellStyle(true);
            string checkMode = ConfigSettings.GetValue("ModePassword");
            if (checkMode == "2222222222222222")
                selectAll.Visible = true;
            else
                selectAll.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetIECTOU_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            ResetAllGrids();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseIECTOU_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoFillIECTOU_Click(object sender, EventArgs e)
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            foreach (DataGridView sGrid in seasonGrids)
            {
                for (int rowIndex = 0; rowIndex < gridS1Day1.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < gridS1Day1.Columns.Count; colIndex++)
                    {
                        sGrid[colIndex, rowIndex].Value = gridS1Day1[colIndex, rowIndex].Value.ToString();
                    }
                }
            }

        }

        private void rdbTOUSeason1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton senderRadioButton = sender as RadioButton;
            if (rdbTOUSeason1.Checked && senderRadioButton == rdbTOUSeason1)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSeason1))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSeason1);
                }
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();

            }
            else if (rdbTOUSeason2.Checked && senderRadioButton == rdbTOUSeason2)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSeason2))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSeason2);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();

            }

            else if (rdbTOUSession3.Checked && senderRadioButton == rdbTOUSession3)//add pradipta
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSession3))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSession3);
                }
                
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();

            }

            else if (rdbTOUSeason4.Checked && senderRadioButton == rdbTOUSeason4)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSeason4))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSeason4);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();


            }
            else if (rdbTOUWithHoliday.Checked && senderRadioButton == rdbTOUWithHoliday)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUHoliday))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUHoliday);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                COLTARIFF = "Rate";
                COLSTARTHOUR = "Start Hour";
                COLSTARTMIN = "Start Minute";

                //Initialize IEC TOU Grid
                SetTOUGrids();
                ResetAllGrids();

                //IEC to DLMS mapping
                dayProfileGrids = new DataGridView[] {gridS1Day1,gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6, gridS2Day1, 
                                        gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, gridS3Day1,
                                        gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6, gridS4Day1, 
                                        gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6 };
                seasonProfileCount = weekProfileCount = 4;
                dayProfileCount = 24;
                seasonProfileGrid = gridActivation;
                weekProfileGrid = gridAssignmentS1;
                touActivationDate = dtPickerFutureActivationDate;


            }
            else if (rdbTOUSP.Checked && senderRadioButton == rdbTOUSP)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSP))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSP);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                //tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();


            }
            else if (rdbTOUWithFourSeason1Phase.Checked && senderRadioButton == rdbTOUWithFourSeason1Phase)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOU1P))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOU1P);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                TOUZone = 6;
                TOUSlots = 6;
                //Initialize TOU 1P NDLMS Grid            
                InitializeSinglePhaseTouGrid(6,6);

            }
            else if (rbTOUFourSeason1P10Zone8Slots.Checked && senderRadioButton == rbTOUFourSeason1P10Zone8Slots)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOU1P))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOU1P);
                }               
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                TOUZone = 10;
                TOUSlots = 8;
                //Initialize TOU 1P NDLMS Grid            
                InitializeSinglePhaseTouGrid(10,8);




            }

            else if (rdb10Zone8SlotFutAct.Checked && senderRadioButton == rdb10Zone8SlotFutAct)
            {
                if (!tabControlTOUOPeration.TabPages.Contains(tabPageTOUSpecialDay))
                {
                    tabControlTOUOPeration.TabPages.Add(tabPageTOUSpecialDay);
                }

                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);

                COLTARIFF = "colTariff";
                COLSTARTHOUR = "colStartHour";
                COLSTARTMIN = "colStartMin";
                //Initialize DLMS TOU grids 
                BindTOUGridsOnSeasonCheckedChange();


            }

            else
            {

            }

        }

        private void gridS1Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick((DataGridView)sender);
        }

        private void gridS1Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ValidateGridCell((DataGridView)sender, e);
        }

        private void gridAssignmentS1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick((DataGridView)sender);
        }

        private void gridAssignmentS1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate((DataGridView)sender, e);
        }

        private void gridActivation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gridActivation[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gridActivation.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                        {
                            DateTime dt = new DateTime();
                            CalendarEditingControl ctl = gridActivation.EditingControl as CalendarEditingControl;
                            if (ctl != null)
                            {
                                bool isValid = DateTime.TryParse(ctl.Value.ToString(), out dt);
                                if (!isValid)
                                {
                                    gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = true;
                                    gridActivation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ctl.Value;
                                    gridActivation.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        else
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        if (Convert.ToInt16(e.FormattedValue.ToString()) < 1 || (Convert.ToInt16(e.FormattedValue.ToString()) > 4))
                        {
                            gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            isValidTOU = true;
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                        }
                    }
                }
            }
        }

        private void gridActivation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAcCellClick();
        }

        /// <summary>
        /// on click of Demand type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDIPDemandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDIPDemandInterval.Enabled = true;
            cmbDIPDemandSubIntervalTime.Enabled = true;
            cmbDIPDemandInterval.SelectedIndex = 0;

            if (cmbDIPDemandType.SelectedIndex == 0)   // block demand
            {
                cmbDIPDemandSubIntervalTime.Visible = false;
                labelDIPSubDemandInterval.Visible = false;
                labelDIPSubDemandIntervalUnit.Visible = false;
            }
            else
            {
                cmbDIPDemandSubIntervalTime.Visible = true;
                labelDIPSubDemandInterval.Visible = true;
                labelDIPSubDemandIntervalUnit.Visible = true;
                cmbDIPDemandSubIntervalTime.SelectedIndex = 0;
            }
            //to hide the  disclaimer
            if (cmbDIPDemandType.SelectedIndex == 1 && cmbDIPDemandInterval.SelectedIndex == 1 && cmbDIPDemandSubIntervalTime.SelectedIndex == 0)
            {
                slidingDemandDisclaimer.Visible = true;
            }
            else
            {
                slidingDemandDisclaimer.Visible = false;
            }
        }
        /// <summary>
        /// on click of Demand Interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDIPDemandInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDIPDemandSubIntervalTime.Items.Clear();
            cmbDIPDemandSubIntervalTime.Items.Add("05 (300)");
            if (cmbDIPDemandType.Text != "Block Demand")
            {
                if (cmbDIPDemandInterval.SelectedIndex == 0)
                {
                    cmbDIPDemandSubIntervalTime.SelectedIndex = 0;
                }
                else if (cmbDIPDemandInterval.SelectedIndex == 1)
                {
                    cmbDIPDemandSubIntervalTime.SelectedIndex = 0;
                    cmbDIPDemandSubIntervalTime.Items.Add("10 (600)");

                }
            }
        }
        /// <summary>
        /// on click of sub demand interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDIPDemandSubIntervalTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDIPDemandType.SelectedIndex == 1 && cmbDIPDemandInterval.SelectedIndex == 1 && cmbDIPDemandSubIntervalTime.SelectedIndex == 0)
            {
                slidingDemandDisclaimer.Visible = true;
            }
            else
            {
                slidingDemandDisclaimer.Visible = false;
            }
        }
        #endregion

        /// <summary>
        /// to switch from current date to future date for single phase tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutureTOUS4SP_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }
        /// <summary>
        /// to reset all tou for single phase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetTOUS4SP_Click(object sender, EventArgs e)
        {
            ResetAllTOU();
        }
        /// <summary>
        /// to autofill all tou for single phase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoFillTOUS4Sp_Click(object sender, EventArgs e)
        {
            AutoFillTOU();
        }
        #region Private Methods
        /// <summary>
        /// get supported meter list
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private MeterConfigSettingsMeterConfigElement GetMeterSupportedListOfConfigParameters(List<byte> inputList)
        {
            MeterConfigSettingsMeterConfigElement meterSupportedListOfConfigParameters = new MeterConfigSettingsMeterConfigElement();
            List<MeterConfigParameters> configParametersList = new List<MeterConfigParameters>();
            GenerateEntity generateEntity = new GenerateEntity();
            string permissionData = string.Empty;
            List<ProfileCommand> lstProfileCommands = GetProfileCommandEntity();
            MeterConfigParameters configParameter;
            int byteIndex = 0;
            int totalArrayCount = inputList[byteIndex + 1];
            if (totalArrayCount > 0x80)
            {
                byteIndex = byteIndex + 2;
                totalArrayCount = inputList[byteIndex + 1];
            }
            byteIndex = byteIndex + 2;

            for (int objectListCount = 0; objectListCount < totalArrayCount; objectListCount++)
            {
                string obisCode = string.Empty;
                configParameter = new MeterConfigParameters();
                int totalStructCount = inputList[byteIndex + 1];
                byteIndex = byteIndex + 2;
                {
                    //total 15 bytes added
                    byteIndex = byteIndex + 2;
                    configParameter.ClassID = inputList[byteIndex];
                    byteIndex = byteIndex + 5;
                    for (int obisData = 0; obisData < 6; obisData++)
                    {
                        obisCode = obisCode + inputList[byteIndex].ToString("X2") + ".";
                        byteIndex++;
                    }
                    configParameter.OBISCode = obisCode.Substring(0, (obisCode.Length - 1));
                    byteIndex = byteIndex + 2;
                    int totalAttribCount = inputList[byteIndex + 1];
                    byteIndex = byteIndex + 2;

                    for (int objectAttribCount = 0; objectAttribCount < totalAttribCount; objectAttribCount++)
                    {
                        byteIndex = byteIndex + 3;
                        configParameter.AttributeID = inputList[byteIndex];
                        byteIndex = byteIndex + 2;

                        configParameter.Permission = inputList[byteIndex];
                        byteIndex = byteIndex + 1;
                        if (inputList[byteIndex] == 0x00)
                        {
                            byteIndex = byteIndex + 1;
                        }
                        else
                        {
                            byteIndex = byteIndex + 4;
                        }
                        configParametersList.Add(configParameter);
                    }
                    int totalMethodCount = inputList[byteIndex + 1];
                    byteIndex = byteIndex + 2;
                    for (int objectMethodCount = 0; objectMethodCount < totalMethodCount; objectMethodCount++)
                    {
                        byteIndex = byteIndex + 6;
                    }
                }
            }

            //conditions to add rows in the grid view.

            meterSupportedListOfConfigParameters.DIP = GetPermission(ProfileId.DIP, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.DIPWithSliding = GetPermission(ProfileId.DIPWithSliding, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.KvahSelection = GetPermission(ProfileId.KvahSelection, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.DisplayParameters = GetPermission(ProfileId.DisplayParameters, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.TOD = GetPermission(ProfileId.ActivationDate, configParametersList, lstProfileCommands) == "true" ? "ONE" : "false";

            meterSupportedListOfConfigParameters.RTC = GetPermission(ProfileId.RTC, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.BillingType = GetPermission(ProfileId.BillingType, configParametersList, lstProfileCommands) == "true" ? "NORMAL" : "false"; ;


            meterSupportedListOfConfigParameters.BillingReset = GetPermission(ProfileId.BillingReset, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.AutoLock = GetPermission(ProfileId.AutoLock, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.LockRS232 = GetPermission(ProfileId.RS232LockUnlock, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.SIP = GetPermission(ProfileId.SIP, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.CTRatio = GetPermission(ProfileId.CTRatio, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.PTRatio = GetPermission(ProfileId.PTRatio, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.DailyLog = GetPermission(ProfileId.DailyLog, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.DTM = GetPermission(ProfileId.DTM, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.MagneticTamperIcon = GetPermission(ProfileId.MagneticTamperIcon, configParametersList, lstProfileCommands);

            // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter having specific right authority to reset
            meterSupportedListOfConfigParameters.MagneticTamperIcon3P = GetPermission(ProfileId.MagneticTamperIcon3P, configParametersList, lstProfileCommands);

            meterSupportedListOfConfigParameters.RS485 = GetPermission(ProfileId.RS485, configParametersList, lstProfileCommands);

            return meterSupportedListOfConfigParameters;
        }

        /// <summary>
        /// return permission to show meter config parameters in the grid.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="configParametersList"></param>
        /// <param name="lstProfileCommands"></param>
        /// <returns></returns>
        private String GetPermission(ProfileId profileId, List<MeterConfigParameters> configParametersList, List<ProfileCommand> lstProfileCommands)
        {
            string mode = string.Empty;
            bool permission = false;
            mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
            List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, profileId);
            foreach (ProfileCommand profileCommand in profileReadCommands)
            {
                foreach (MeterConfigParameters configParameter in configParametersList)
                {
                    if (mode == MasterMode)
                    {
                        if (profileCommand.ObisCode == configParameter.OBISCode && profileCommand.ClassId == configParameter.ClassID
                            && profileCommand.Attribute == configParameter.AttributeID)
                        {
                            if (configParameter.Permission == 2 || configParameter.Permission == 3 || configParameter.Permission == 5 || configParameter.Permission == 6)
                            {
                                permission = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (profileCommand.ObisCode == configParameter.OBISCode && profileCommand.ClassId == configParameter.ClassID
                                && profileCommand.Attribute == configParameter.AttributeID)
                        {
                            if (configParameter.Permission == 1 || configParameter.Permission == 3 || configParameter.Permission == 4 || configParameter.Permission == 6)
                            {
                                permission = true;
                                break;
                            }
                        }
                    }
                }
                if (permission)
                {
                    break;
                }
            }
            return permission.ToString().ToLower();
        }


        /// <summary>
        /// Get Profile Commands To Read
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile)
        {
            List<ProfileCommand> profileReadCommands = null;
            //find normal commands
            profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
            {
                return profileCommandEntity.TagNumber == (int)selectedProfile
                && (profileCommandEntity.ClassId != 0xFF);
            });
            return profileReadCommands;
        }

        #region ValidateTOU
        /// <summary>
        /// This method is called while click on the season grid from the view and validating the grid.
        /// </summary>
        private void SeasonGridCellClick()
        {
            try
            {
                rIndex = seasonProfileGrid.CurrentCell.RowIndex;
                if (weekProfileGrid.Rows[rIndex].Cells[7].Value == null)
                {
                    seasonProfileGrid.ReadOnly = true;
                }
                else
                {
                    seasonProfileGrid.ReadOnly = false;
                    if (seasonProfileGrid.Rows[rIndex].Cells[0].Value == null)
                    {
                        seasonProfileGrid.Rows[rIndex].Cells[1].Value = null;
                        seasonProfileGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                    }
                    else
                    {
                        seasonProfileGrid.Rows[rIndex].Cells[1].ReadOnly = false;

                        for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                        {
                            if (dayProfileGrids[gridCount].Rows[0].Cells[1].Value == null
                                && dayProfileGrids[gridCount].Rows[0].Cells[2].Value == null
                                && dayProfileGrids[gridCount].Rows[0].Cells[3].Value == null)
                            {
                                seasonProfileGrid.ReadOnly = true;
                                break;
                            }
                            else
                            {
                                seasonProfileGrid.ReadOnly = false;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //throw ex;
                logger.Log(LOGLEVELS.Error, "SeasonGridCellClick()", ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// This method is called while click on the season grid from the view and validating the grid SP NDLMS.
        /// </summary>
        private void SP_NDLMS_DayGridCellClick(DataGridView dataGrid)
        {
            try
            {
                int dayProfCount = 4;
                DataGridView[] dayProfGrids = new DataGridView[] { gridTOUDay1_1P_NDLMS, gridTOUDay2_1P_NDLMS, gridTOUDay3_1P_NDLMS, gridTOUDay4_1P_NDLMS };


                rcount = dataGrid.CurrentCell.RowIndex;
                if (rcount != 0 && dataGrid.Rows[rcount - 1].Cells[1].Value != null)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[2].Value != null && dataGrid.Rows[rcount - 1].Cells[3].Value != null)
                    {
                        if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "55")
                        {
                            for (count = rcount; count < TOUZone; count++)
                            {
                                dataGrid.Rows[count].ReadOnly = true;
                            }
                            return;
                        }
                    }
                }

                //for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                //{
                //    for (int rowCount = 0; rowCount < 9; rowCount++)
                //    {
                //        if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value != null))
                //        {
                //            if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value.ToString())
                //                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value)))
                //            {
                //                while (rowCount < 8)
                //                {
                //                    dayProfileGrids[gIndex].Rows[rowCount + 2].ReadOnly = true;
                //                    rowCount++;
                //                }
                //                return;
                //            }
                //        }
                //    }
                //}

                if (rcount != 0)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[3].Value == null)
                    {
                        int rowCount = rcount;
                        while (rowCount < TOUZone)
                        {
                            dataGrid.Rows[rowCount].ReadOnly = true;
                            rowCount++;
                        }
                        if (dataGrid.Rows[rcount - 1].Cells[1].Value == null && dataGrid.Rows[rcount - 1].Cells[2].Value == null)
                        {
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        for (count = 1; count <= 3; count++)
                        {
                            dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                        }
                        rIndex = rcount + 1;
                        while (rIndex < TOUZone)
                        {
                            dataGrid.Rows[rIndex].ReadOnly = false;
                            rIndex++;
                        }
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value == null)
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].Value = null;
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = true;
                    }


                    count = 0;
                    while (dayProfGrids[count].Name != dataGrid.Name)
                    {
                        count++;
                    }




                    for (gIndex = 0; gIndex < dayProfCount; gIndex++)
                    {
                        for (rIndex = 0; rIndex < TOUZone; rIndex++)
                        {
                            if (dayProfGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                                && (dayProfGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                                || dayProfGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                            {
                                count = gIndex;
                                while (count < dayProfCount - 1)
                                {
                                    dayProfGrids[count + 1].ReadOnly = true;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfGrids[count - 1].ReadOnly = true;
                                        count--;
                                    }
                                }
                                gridDayTables_1P_NDLMS.ReadOnly = true;
                                gridActivationDate_1P_NDLMS.ReadOnly = true;
                                return;
                            }
                            else
                            {
                                count = gIndex;
                                while (count < dayProfCount - 1)
                                {
                                    dayProfGrids[count + 1].ReadOnly = false;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfGrids[count - 1].ReadOnly = false;
                                        count--;
                                    }
                                }
                                gridDayTables_1P_NDLMS.ReadOnly = false;
                                gridActivationDate_1P_NDLMS.ReadOnly = false;

                            }
                        }
                    }
                    rIndex = rcount + 1;
                    return;
                }
                else
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                    }
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }


                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && dataGrid.Rows[rcount].Cells[2].Value == null)
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = true;
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && (dataGrid.Rows[rcount].Cells[2].Value == null
                        && dataGrid.Rows[rcount].Cells[3].Value == null))
                {
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                for (gIndex = 0; gIndex < dayProfCount; gIndex++)
                {
                    for (rIndex = 0; rIndex < TOUZone; rIndex++)
                    {
                        if (dayProfGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                            && (dayProfGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                            || dayProfGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                        {
                            count = gIndex;
                            while (count < dayProfCount - 1)
                            {
                                dayProfGrids[count + 1].ReadOnly = true;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfGrids[count - 1].ReadOnly = true;
                                    count--;
                                }
                            }
                            gridDayTables_1P_NDLMS.ReadOnly = true;
                            gridActivationDate_1P_NDLMS.ReadOnly = true;
                            return;
                        }
                        else
                        {
                            count = gIndex;
                            while (count < dayProfCount - 1)
                            {
                                dayProfGrids[count + 1].ReadOnly = false;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfGrids[count - 1].ReadOnly = false;
                                    count--;
                                }
                            }
                            gridDayTables_1P_NDLMS.ReadOnly = false;
                            gridActivationDate_1P_NDLMS.ReadOnly = false;

                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_DayGridCellClick(DataGridView dataGrid)", ex);
            }
            finally
            {
                dataGrid.Rows[0].Cells[2].ReadOnly = true;
                dataGrid.Rows[0].Cells[3].ReadOnly = true;
                dataGrid.Columns[0].ReadOnly = true;
            }
        }



        /// <summary>
        /// This method is used for validating day profile grid value on the cell click event. 
        /// </summary>
        private void DayGridCellClick(DataGridView dataGrid)
        {
            try
            {
                rcount = dataGrid.CurrentCell.RowIndex;
                if (rcount != 0 && dataGrid.Rows[rcount - 1].Cells[1].Value != null)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[2].Value != null && dataGrid.Rows[rcount - 1].Cells[3].Value != null)
                    {
                        if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "45")
                        {
                            for (count = rcount; count < 10; count++)
                            {
                                dataGrid.Rows[count].ReadOnly = true;
                            }
                            return;
                        }
                    }
                }

                //for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                //{
                //    for (int rowCount = 0; rowCount < 9; rowCount++)
                //    {
                //        if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value != null))
                //        {
                //            if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value.ToString())
                //                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value)))
                //            {
                //                while (rowCount < 8)
                //                {
                //                    dayProfileGrids[gIndex].Rows[rowCount + 2].ReadOnly = true;
                //                    rowCount++;
                //                }
                //                return;
                //            }
                //        }
                //    }
                //}

                if (rcount != 0)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[3].Value == null)
                    {
                        int rowCount = rcount;
                        while (rowCount < 10)
                        {
                            dataGrid.Rows[rowCount].ReadOnly = true;
                            rowCount++;
                        }
                        if (dataGrid.Rows[rcount - 1].Cells[1].Value == null && dataGrid.Rows[rcount - 1].Cells[2].Value == null)
                        {
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        for (count = 1; count <= 3; count++)
                        {
                            dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                        }
                        rIndex = rcount + 1;
                        while (rIndex < 10)
                        {
                            dataGrid.Rows[rIndex].ReadOnly = false;
                            rIndex++;
                        }
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value == null)
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].Value = null;
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = true;
                    }
                    count = 0;
                    while (dayProfileGrids[count].Name != dataGrid.Name)
                    {
                        count++;
                    }

                    for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                    {
                        for (rIndex = 0; rIndex < 10; rIndex++)
                        {
                            if (dayProfileGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                                && (dayProfileGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                                || dayProfileGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                            {
                                count = gIndex;
                                while (count < dayProfileCount - 1)
                                {
                                    dayProfileGrids[count + 1].ReadOnly = true;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfileGrids[count - 1].ReadOnly = true;
                                        count--;
                                    }
                                }
                                weekProfileGrid.ReadOnly = true;
                                seasonProfileGrid.ReadOnly = true;
                                return;
                            }
                            else
                            {
                                count = gIndex;
                                while (count < dayProfileCount - 1)
                                {
                                    dayProfileGrids[count + 1].ReadOnly = false;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfileGrids[count - 1].ReadOnly = false;
                                        count--;
                                    }
                                }
                                weekProfileGrid.ReadOnly = false;
                                seasonProfileGrid.ReadOnly = false;

                            }
                        }
                    }
                    rIndex = rcount + 1;
                    return;
                }
                else
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                    }
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }


                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && dataGrid.Rows[rcount].Cells[2].Value == null)
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = true;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && (dataGrid.Rows[rcount].Cells[2].Value == null
                        && dataGrid.Rows[rcount].Cells[3].Value == null))
                {
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rIndex = 0; rIndex < 10; rIndex++)
                    {
                        if (dayProfileGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                            && (dayProfileGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                            || dayProfileGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                        {
                            count = gIndex;
                            while (count < dayProfileCount - 1)
                            {
                                dayProfileGrids[count + 1].ReadOnly = true;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfileGrids[count - 1].ReadOnly = true;
                                    count--;
                                }
                            }
                            weekProfileGrid.ReadOnly = true;
                            seasonProfileGrid.ReadOnly = true;
                            return;
                        }
                        else
                        {
                            count = gIndex;
                            while (count < dayProfileCount - 1)
                            {
                                dayProfileGrids[count + 1].ReadOnly = false;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfileGrids[count - 1].ReadOnly = false;
                                    count--;
                                }
                            }
                            weekProfileGrid.ReadOnly = false;
                            seasonProfileGrid.ReadOnly = false;

                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "DayGridCellClick(DataGridView dataGrid)", ex);
            }
            finally
            {
                dataGrid.Rows[0].Cells[2].ReadOnly = true;
                dataGrid.Rows[0].Cells[3].ReadOnly = true;
                dataGrid.Columns[0].ReadOnly = true;
            }

        }

        /// <summary>
        /// This method is used for week profile grid value  checks on the cell click event. 
        /// </summary>
        private void WeekGridCellClick()
        {
            try
            {
                rIndex = weekProfileGrid.CurrentCell.RowIndex;
                if (rIndex != 0 && seasonProfileGrid.Rows[rIndex - 1].Cells[1].Value == null)
                {
                    int rowIndex = rIndex;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrid.Rows[rIndex].ReadOnly = true;
                        rowIndex++;
                    }
                    return;
                }
                else
                {
                    int rowIndex = rIndex + 1;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrid.ReadOnly = false;
                        rowIndex++;
                    }
                }

                int colIndex = weekProfileGrid.CurrentCell.ColumnIndex;
                if (colIndex != 0 && (weekProfileGrid.Rows[rIndex].Cells[colIndex - 1].Value == null))
                {
                    weekProfileGrid.Rows[rIndex].Cells[colIndex].Value = null;
                    weekProfileGrid.Rows[rIndex].Cells[colIndex].ReadOnly = true;
                    return;
                }
                else
                {
                    weekProfileGrid.Rows[rIndex].Cells[colIndex].ReadOnly = false;
                }

                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    if (dayProfileGrids[gridCount].Rows[0].Cells[1].Value == null
                        && dayProfileGrids[gridCount].Rows[0].Cells[2].Value == null
                        && dayProfileGrids[gridCount].Rows[0].Cells[3].Value == null)
                    {
                        weekProfileGrid.ReadOnly = true;
                        break;
                    }
                    else
                    {
                        weekProfileGrid.ReadOnly = false;

                    }
                }
                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rcount = 0; rcount < 9; rcount++)
                    {
                        if ((dayProfileGrids[gIndex].Rows[rcount].Cells[2].Value != null)
                            && (dayProfileGrids[gIndex].Rows[rcount].Cells[3].Value != null)
                            && (dayProfileGrids[gIndex].Rows[rcount + 1].Cells[2].Value != null)
                            && (dayProfileGrids[gIndex].Rows[rcount + 1].Cells[3].Value != null))
                        {
                            if ((dayProfileGrids[gIndex].Rows[rcount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rcount + 1].Cells[2].Value.ToString())
                                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rcount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rcount + 1].Cells[3].Value)))
                            {
                                while (rcount < 8)
                                {
                                    dayProfileGrids[gIndex].Rows[rcount + 2].ReadOnly = true;
                                    rcount++;
                                }
                                weekProfileGrid.ReadOnly = true;
                                seasonProfileGrid.ReadOnly = true;
                                return;
                            }
                        }
                    }
                }
                rIndex = weekProfileGrid.CurrentCell.RowIndex;
                count = weekProfileGrid.CurrentCell.ColumnIndex;
                if (count >= 2)
                {
                    if (weekProfileGrid.Rows[rIndex].Cells[count].Value == null)
                    {
                        if (weekProfileGrid.Rows[rIndex].Cells[count - 1].Value == null)
                        {
                            weekProfileGrid.Rows[rIndex].Cells[count].ReadOnly = true;
                            seasonProfileGrid.Rows[rIndex].ReadOnly = true;
                        }
                        while (count < 7)
                        {
                            weekProfileGrid.Rows[rIndex].Cells[count + 1].ReadOnly = true;
                            count++;
                        }
                        return;
                    }
                    else
                    {
                        weekProfileGrid.Rows[rIndex].Cells[count].ReadOnly = false;
                        while (count < 7)
                        {
                            weekProfileGrid.Rows[rIndex].Cells[count + 1].ReadOnly = false;
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //throw ex;
                logger.Log(LOGLEVELS.Error, "WeekGridCellClick()", ex);
            }
            finally
            {
                weekProfileGrid.Columns[0].ReadOnly = true;
            }
        }

        /// <summary>
        /// This method is used for validating day profile grid value on cell click event from the view for SP NDLMS.
        /// </summary>
        private void SP_NDLMS_ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dtView = sender as DataGridView;
            try
            {
                if (dtView.CurrentCell.IsInEditMode == true)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (e.FormattedValue == null)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                        }
                        if (e.FormattedValue.ToString() != "")
                        {
                            if (e.RowIndex == 0)
                            {
                                dtView.Rows[e.RowIndex].Cells[2].Value = "00";
                                dtView.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 1; colCount <= gridDayTables_1P_NDLMS.ColumnCount - 1; colCount++)
                                {
                                    gridDayTables_1P_NDLMS.Rows[0].Cells[colCount].Value = "1";
                                }
                                gridActivationDate_1P_NDLMS.Rows[0].Cells[0].Value = "01";
                                gridActivationDate_1P_NDLMS.Rows[0].Cells[1].Value = "01";

                            }
                        }
                        rcount = dtView.CurrentCell.RowIndex;
                        if (dtView.Rows[rcount].Cells[1].Value == null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dtView.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < TOUZone)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < TOUZone)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null &&
                            dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                    }
                    if (e.ColumnIndex == 2)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                        }
                        if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            e.Cancel = true;
                        }
                        else if (Convert.ToInt16(e.FormattedValue) > 23)
                        {
                            e.Cancel = true;
                        }

                        else
                        {

                        }
                        if (e.RowIndex != (TOUZone - 1) && dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < TOUZone; count++)
                                    {
                                        dtView.Rows[count].ReadOnly = true;
                                    }
                                }

                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "55")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < TOUZone; count++)
                                        {
                                            dtView.Rows[count].ReadOnly = true;
                                        }

                                    }

                                }
                            }
                        }
                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }
                    }
                    if (e.ColumnIndex == 3)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }

                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 55)
                        {
                            e.Cancel = true;
                        }

                        if (e.RowIndex != (TOUZone - 1) && dtView.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                        {
                            if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                dtView.Rows[e.RowIndex].ErrorText = INVALID;
                e.Cancel = true;
               //throw ex;
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }


        /// <summary>
        /// This method is used for validating day profile grid value on cell click event from the view.
        /// </summary>
        private void ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dtView = sender as DataGridView;
            try
            {
                if (dtView.CurrentCell.IsInEditMode == true)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (e.FormattedValue == null)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                        }
                        if (e.FormattedValue.ToString() != "")
                        {
                            if (e.RowIndex == 0)
                            {
                                dtView.Rows[e.RowIndex].Cells[2].Value = "00";
                                dtView.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 1; colCount <= weekProfileGrid.ColumnCount - 1; colCount++)
                                {
                                    weekProfileGrid.Rows[0].Cells[colCount].Value = "01";
                                }
                                seasonProfileGrid.Rows[0].Cells[0].Value = "01";
                                seasonProfileGrid.Rows[0].Cells[1].Value = "01";
                                seasonProfileGrid.Rows[0].Cells[2].Value = "01";

                            }
                        }
                        rcount = dtView.CurrentCell.RowIndex;
                        if (dtView.Rows[rcount].Cells[1].Value == null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dtView.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null &&
                            dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                    }
                    if (e.ColumnIndex == 2)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                        }
                        if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            e.Cancel = true;
                        }
                        else if (Convert.ToInt16(e.FormattedValue) > 23)
                        {
                            e.Cancel = true;
                        }

                        else
                        {

                        }
                        if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < 10; count++)
                                    {
                                        dtView.Rows[count].ReadOnly = true;
                                    }
                                }

                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "45")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < 10; count++)
                                        {
                                            dtView.Rows[count].ReadOnly = true;
                                        }

                                    }

                                }
                            }
                        }
                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }
                    }
                    if (e.ColumnIndex == 3)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }

                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 45)
                        {
                            e.Cancel = true;
                        }

                        if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                        {
                            if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)    //Exception log for catch block
            {
                dtView.Rows[e.RowIndex].ErrorText = INVALID;
                e.Cancel = true;
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }

        /// <summary>
        /// This method is used for validating week profile grid value on the cell click event. 
        /// </summary>
        private void ValidateWeekProfileCell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView weekProfileGrids = sender as DataGridView;
            try
            {
                if (weekProfileGrids[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            string gridVal = e.FormattedValue.ToString();
                            if (gridVal == "")
                            {
                                weekProfileGrids.Rows[e.RowIndex].ErrorText = INVALID;
                                e.Cancel = true;
                            }
                            else
                            {
                                if (Convert.ToInt16(gridVal) < 1 || Convert.ToInt16(gridVal) > 6)
                                {
                                    weekProfileGrids.Rows[e.RowIndex].ErrorText = INVALID;
                                    e.Cancel = true;
                                }
                                else
                                {
                                    weekProfileGrids.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        if (e.RowIndex == 0 && e.ColumnIndex == 7)
                        {
                            seasonProfileGrid.Rows[e.RowIndex].Cells[0].Value = "01";
                            seasonProfileGrid.Rows[e.RowIndex].Cells[1].Value = "01";
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateWeekProfileCell(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }

        /// <summary>
        /// This method is used for validating the season profile grid on cell click event.
        /// </summary>
        private void ValidateSeasonProfileCell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                DataGridView seasonProfileGrids = sender as DataGridView;
                if (seasonProfileGrids[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }

                            if (e.FormattedValue.ToString() == "") { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 31))
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = INVALID;
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0 && e.FormattedValue.ToString() != ""
                                && e.FormattedValue.ToString() != null
                                && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    if (!this.rdbTOUSeason2.Checked)
                                    {
                                        e.Cancel = true;
                                        return;
                                    }
                                }
                            }
                            if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) > 29)
                            {
                                if (Convert.ToInt16(e.FormattedValue) == 2)
                                {
                                    seasonProfileGrids.Rows[e.RowIndex].ErrorText = INVALID;
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (e.FormattedValue.ToString() == "")
                            { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 12))
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = INVALID;
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != 0 && e.FormattedValue != null && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value)
                                    <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }

                            if (e.RowIndex != seasonProfileCount - 1 && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value)
                                    >= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0
                                && e.RowIndex != seasonProfileCount - 1
                                && e.FormattedValue != null
                                && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.FormattedValue != null && e.FormattedValue.ToString() != "" && Convert.ToInt16(e.FormattedValue) == 2)
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) == 29)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (e.RowIndex != 0 && e.ColumnIndex == 0)
                    {
                        if (e.RowIndex != 0 && e.FormattedValue != null
                            && e.FormattedValue.ToString() != ""
                            && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                        {
                            if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value)
                                <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    weekProfileGrid[1, count].ReadOnly = true;
                                    count++;
                                }
                                return;
                            }
                            else
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    weekProfileGrid[1, count].ReadOnly = false;
                                    count++;
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateSeasonProfileCell(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void AutoFillTOU()
        {
            if (dayProfileGrids[0].Rows[0].Cells[COLTARIFF].Value != null && dayProfileGrids[0].Rows[0].Cells[COLTARIFF].Value.ToString() != string.Empty)
            {
                for (int gridCount = 1; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rowCount = 0; rowCount < dayProfileGrids[0].Rows.Count; rowCount++)
                    {
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLTARIFF].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value;
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value;
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTMIN].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value;
                    }
                }
                for (int rowCount = 0; rowCount < weekProfileCount; rowCount++)
                {
                    for (int colCount = 1; colCount <= weekProfileGrid.ColumnCount - 1; colCount++)
                    {

                        weekProfileGrid.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");

                    }
                }
                for (int rowCount = 0; rowCount < seasonProfileCount; rowCount++)
                {
                    for (int colCount = 0; colCount <= seasonProfileGrid.ColumnCount - 1; colCount++)
                    {
                        seasonProfileGrid.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");
                    }
                }
                if (rdb10Zone8SlotFutAct.Checked)
                {
                    int monthcount = 1;
                    int dayCnt = 1;
                    //DataGridView[] gridnames;
                    //gridnames = new DataGridView[] { gridTOUDay1, gridTOUDay2, gridTOUDay3, gridTOUDay4 };
                    for (int rowCount = 0; rowCount < SpecialDayProfileCount; rowCount++)
                    {
                        if (monthcount > 12) monthcount = 1;
                        if (dayCnt > 4) dayCnt = 1;
                        SpecialDayProfileGrid.Rows[rowCount].Cells[1].Value = (monthcount++).ToString("00");
                        SpecialDayProfileGrid.Rows[rowCount].Cells[2].Value = (rowCount + 1).ToString("00");
                        SpecialDayProfileGrid.Rows[rowCount].Cells[3].Value = (dayCnt++).ToString("00");

                    }

                    
                }
            }

        }

        /// <summary>
        /// This method is used for resetting the all tou details from the grids.
        /// </summary>        
        private void ResetAllTOU()
        {
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rCount = 0; rCount < dayProfileGrids[gridCount].RowCount; rCount++)
                    {
                        for (int cCount = 1; cCount < dayProfileGrids[gridCount].ColumnCount; cCount++)
                        {
                            dayProfileGrids[gridCount].Rows[rCount].Cells[cCount].Value = null;
                        }
                    }
                }
                for (int rCount = 0; rCount < weekProfileCount; rCount++)
                {
                    for (int cCount = 1; cCount < weekProfileGrid.ColumnCount; cCount++)
                    {
                        weekProfileGrid.Rows[rCount].Cells[cCount].Value = null;
                    }
                }

                for (int rCount = 0; rCount < seasonProfileCount; rCount++)
                {
                    for (int cCount = 0; cCount < seasonProfileGrid.ColumnCount; cCount++)
                    {
                        seasonProfileGrid.Rows[rCount].Cells[cCount].Value = null;
                    }
                }
                //Special Day Profile
                if (rdb10Zone8SlotFutAct.Checked)
                {
                    for (int rCount = 0; rCount < SpecialDayProfileCount; rCount++)
                    {
                        for (int cCount = 1; cCount < SpecialDayProfileGrid.ColumnCount; cCount++)
                        {
                            SpecialDayProfileGrid.Rows[rCount].Cells[cCount].Value = null;
                        }
                    }
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ResetAllTOU()", ex);
            }
            touActivationDate.Value = DateTime.Now;
        }

        #endregion

        #region ReaddWriteTOU
        //**********smart meter special day profile************
        private byte[] GetSplDayProfileBuffer()
        {
            touData = new List<byte>();
            try
            {
                touData.Add(TOUConstants.Array);
                touData.Add(SpecialDayProfileCount);

                for (byte i = 0; i < SpecialDayProfileCount; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x03);

                    touData.Add(0x12);
                    touData.Add(0x00);
                    touData.Add(0x00);

                    touData.Add(0x09);
                    touData.Add(0x05);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(Convert.ToByte(dgvSpclDayProf8Tariff.Rows[i].Cells["Month"].Value));
                    touData.Add(Convert.ToByte(dgvSpclDayProf8Tariff.Rows[i].Cells["Date"].Value));
                    touData.Add(0xFF);
                    touData.Add(0x11);
                    touData.Add(Convert.ToByte(dgvSpclDayProf8Tariff.Rows[i].Cells["DayID"].Value));

                }

                
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GetSplDayProfileBuffer()", ex);
            }
            return touData.ToArray();
        }


        /// <summary>
        /// Used to get season profile data buffer.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetSeasonProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            try
            {
                touData.Add(TOUConstants.Array);
                if (meterModel == NamePlateConstants.RubyE250Value)// ruby
                {
                    touData.Add(TOUConstants.MaxSeason);
                }
                else
                {
                    touData.Add(seasonProfileCount);
                }

                for (byte i = 0; i < seasonProfileCount; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x03);

                    touData.Add(0x09);
                    touData.Add(0x01);
                    if (Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLSESSION].Value) == 0x00)
                    {
                        touData.Add(0x01);
                    }
                    else
                    {
                        touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLSESSION].Value));
                    }
                    touData.Add(0x09);
                    touData.Add(0x0C);
                    touData.Add(0xFF);//year low byte
                    touData.Add(0xFF);//year high
                    touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLMONTH].Value));//month
                    touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLDAY].Value));//day of month
                    touData.Add(0xFF);//day of week
                    touData.Add(0xFF);//hour
                    touData.Add(0xFF);//min
                    touData.Add(0xFF);//second
                    touData.Add(0xFF);// hundreth
                    touData.Add(0x80);//deviation high byte
                    touData.Add(0x00);//deviation low byte
                    ////***** clock status This condition for Sapphire S2,"W0","L0","BYPL_FD" send FF in place of 00(default) ********
                    touData.Add(GenericRTC.SEASONPROFILEWRITE(meterModel).clockstatus);
                    touData.Add(0x09);
                    touData.Add(0x01);
                    if (Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLSESSION].Value) == 0x00)
                    {
                        touData.Add(0x01);
                    }
                    else
                    {
                        touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[COLSESSION].Value));
                    }
                }
                if (meterModel == NamePlateConstants.RubyE250Value)// ruby
                {
                    for (int i = seasonProfileCount; i < TOUConstants.MaxSeason; i++)
                    {
                        touData.Add(TOUConstants.Structure);
                        touData.Add(0x03);
                        touData.Add(0x09);
                        touData.Add(0x01);
                        touData.Add(0x00);
                        touData.Add(0x09);
                        touData.Add(0x0C);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0xFF);
                        touData.Add(0x80);
                        touData.Add(0x00);
                        touData.Add(0x00);
                        touData.Add(0x09);
                        touData.Add(0x01);
                        touData.Add(0x00);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetSeasonProfileBuffer(int meterModel)", ex);
               //throw ex;
            }
            return touData.ToArray();
        }

        /// <summary>
        /// Used to get season profile data buffer for DLMS from IEC UI.
        /// </summary>
        /// <returns></returns>
        private byte[] GetSeasonProfileICEToDLMS()
        {
            touData = new List<byte>();
            try
            {
                touData.Add(TOUConstants.Array);
                touData.Add(TOUConstants.MaxSeason);
                DateTime SeasonActivationDate;

                for (byte i = 0; i < seasonProfileCount; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x03);

                    touData.Add(0x09);
                    touData.Add(0x01);

                    touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[ColSeasonNumberIEC].Value));

                    SeasonActivationDate = Convert.ToDateTime(seasonProfileGrid.Rows[i].Cells[ColSeasonActivationDateIEC].Value);

                    touData.Add(0x09);
                    touData.Add(0x0C);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(Convert.ToByte(SeasonActivationDate.Month));
                    touData.Add(Convert.ToByte(SeasonActivationDate.Day));
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0x80);
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x09);
                    touData.Add(0x01);
                    touData.Add(Convert.ToByte(seasonProfileGrid.Rows[i].Cells[ColSeasonNumberIEC].Value));

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetSeasonProfileICEToDLMS()", ex);
            }
            return touData.ToArray();
        }
        /// <summary>
        /// Used to get buffer data for week profile writing
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetWeekProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            try
            {
                touData.Add(TOUConstants.Array);
                if (meterModel == NamePlateConstants.RubyE250Value)// ruby
                {
                    touData.Add(TOUConstants.MaxWeek);
                }
                else
                {
                    touData.Add(weekProfileCount);
                }

                for (byte i = 0; i < weekProfileCount; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x08);

                    touData.Add(0x09);
                    touData.Add(0x01);
                    touData.Add((byte)(i + 1));

                    for (byte j = 1; j < 8; j++)
                    {
                        touData.Add(0x11);
                        touData.Add(Convert.ToByte(weekProfileGrid.Rows[i].Cells[j].Value) == 0x00 ?
                            (byte)0x01 : Convert.ToByte(weekProfileGrid.Rows[i].Cells[j].Value));
                    }
                }
                if (meterModel == NamePlateConstants.RubyE250Value)// ruby
                {
                    for (int i = weekProfileCount; i < 4; i++)
                    {
                        touData.Add(0x02);
                        touData.Add(0x08);

                        touData.Add(0x09);
                        touData.Add(0x01);
                        touData.Add(0x00);

                        for (byte j = 1; j < 8; j++)
                        {
                            touData.Add(0x11);
                            touData.Add(0x00);
                        }
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetWeekProfileBuffer(int meterModel)", ex);
            }            
            return touData.ToArray();
        }

        /// <summary>
        /// Used to get buffer data for week profile writing from IEC UI to DLMS.
        /// </summary>
        /// <returns></returns>
        private byte[] GetWeekProfileBufferIECToDLMS()
        {
            touData = new List<byte>();
            try
            {
                DataGridView[] gridDayAssignment = GetAssignmentGridCollection();
                touData.Add(TOUConstants.Array);
                touData.Add(weekProfileCount);
                for (byte i = 0; i < weekProfileCount; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x08);
                    touData.Add(0x09);
                    touData.Add(0x01);
                    touData.Add((byte)(i + 1));
                    for (byte j = 1; j < 7; j++)
                    {
                        touData.Add(0x11);
                        touData.Add(Convert.ToByte(gridDayAssignment[i].Rows[j].Cells[1].Value.ToString().Substring(10)) == 0x00 ?
                        (byte)0x01 : Convert.ToByte(gridDayAssignment[i].Rows[j].Cells[1].Value.ToString().Substring(10)));
                    }
                    //Sunday comes last
                    touData.Add(0x11);
                    touData.Add(Convert.ToByte(gridDayAssignment[i].Rows[0].Cells[1].Value.ToString().Substring(10)) == 0x00 ?
                        (byte)0x01 : Convert.ToByte(gridDayAssignment[i].Rows[0].Cells[1].Value.ToString().Substring(10)));
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetWeekProfileBufferIECToDLMS()", ex);
            }
            return touData.ToArray();
        }
        /// <summary>
        /// Used to get buffer for writing day profile data.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetDayProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            try
            {
                if (rdbTOUSeason1.Checked == true)
                {
                    dayProfileCount = 1;
                }
                //SarkarA code change start 20180503 // fix tou change via upload function
                if (rdbTOUSession3.Checked == true && 
                    !(meterModel == NamePlateConstants.RubyE250Value || meterModel == NamePlateConstants.Ruby6Value || meterModel == NamePlateConstants.Ruby6ukModelValue || meterModel == NamePlateConstants.RubyE350Value || meterModel == NamePlateConstants.RubyE150Value))
                {
                    dayProfileCount = 3;
                }
                if (rdbTOUSeason2.Checked == true && 
                    !(meterModel == NamePlateConstants.RubyE250Value || meterModel == NamePlateConstants.Ruby6Value || meterModel == NamePlateConstants.Ruby6ukModelValue || meterModel == NamePlateConstants.RubyE350Value || meterModel == NamePlateConstants.RubyE150Value))
                {
                    dayProfileCount = 2;
                }
                //SarkarA code change end 20180503
                byte tempDayProfileCount = dayProfileCount;//commet pkfoutou

                touData.Add(TOUConstants.Array);
                if (meterModel == NamePlateConstants.RubyE250Value)// ruby
                {
                    if (dayProfileCount == 1)
                    {
                        tempDayProfileCount = 1;
                    }
                    touData.Add(TOUConstants.MaxDay);
                }
                else
                {
                    touData.Add(tempDayProfileCount);
                }

                for (byte i = 0; i < tempDayProfileCount; i++)
                {
                    touData.Add(0x02);
                    touData.Add(0x02);
                    touData.Add(0x11);
                    touData.Add((byte)(i + 1)); //Day Id 
                    touData.Add(0x01);
                    touData.Add(0x0A);

                    for (byte j = 0; j < 10; j++)
                    {
                        touData.Add(0x02);
                        touData.Add(0x03);
                        touData.Add(0x09);
                        touData.Add(0x04);
                        touData.Add(Convert.ToByte(dayProfileGrids[i].Rows[j].Cells[COLSTARTHOUR].Value));      //   Slot Start Hour
                        touData.Add(Convert.ToByte(dayProfileGrids[i].Rows[j].Cells[COLSTARTMIN].Value));       //   Slot Start min
                        //*****This condition for SapphireS2 & sapphire meter amendment 5 send 00 in place of FF ********                       
                        touData.Add(GenericRTC.DAYPROFILEWRITE(meterModel).second);
                        touData.Add(GenericRTC.DAYPROFILEWRITE(meterModel).Hundreds);
                        touData.Add(0x09);
                        touData.Add(0x06);
                        touData.Add(0x00);
                        touData.Add(0x00);
                        touData.Add(0x0A);
                        touData.Add(0x00);
                        touData.Add(0x64);
                        touData.Add(0xFF);
                        touData.Add(0x12);
                        touData.Add(0x00);
                        switch (Convert.ToString(dayProfileGrids[i].Rows[j].Cells[COLTARIFF].Value))
                        {
                            case "T1":
                                touData.Add(0x01);
                                break;
                            case "T2":
                                touData.Add(0x02);
                                break;
                            case "T3":
                                touData.Add(0x03);
                                break;
                            case "T4":
                                touData.Add(0x04);
                                break;
                            case "T5":
                                touData.Add(0x05);
                                break;
                            case "T6":
                                touData.Add(0x06);
                                break;
                            case "T7":
                                touData.Add(0x07);
                                break;
                            case "T8":
                                touData.Add(0x08);
                                break;
                            default:
                                touData.Add(0x00);
                                break;
                        }
                    }
                }
                if (meterModel == NamePlateConstants.RubyE250Value)
                {
                    if (dayProfileCount == 1)
                    {
                        for (byte i = tempDayProfileCount; i <= 5; i++)
                        {
                            touData.Add(0x02);
                            touData.Add(0x02);
                            touData.Add(0x11);
                            touData.Add(0x00);             //Day Id 
                            touData.Add(0x01);
                            touData.Add(0x0A);

                            for (byte j = 0; j < 10; j++)
                            {
                                touData.Add(0x02);
                                touData.Add(0x03);
                                touData.Add(0x09);
                                touData.Add(0x04);
                                touData.Add(0x00);      //   Slot Start Hour
                                touData.Add(0x00);       //   Slot Start min
                                touData.Add(0x00);
                                touData.Add(0x00);
                                touData.Add(0x09);
                                touData.Add(0x06);
                                touData.Add(0x00);
                                touData.Add(0x00);
                                touData.Add(0x0A);
                                touData.Add(0x00);
                                touData.Add(0x64);
                                touData.Add(0xFF);
                                touData.Add(0x12);
                                touData.Add(0x00);
                                touData.Add(0x00);
                            }
                            tempDayProfileCount++;
                        }

                        touData.Add(0x02);
                        touData.Add(0x02);
                        touData.Add(0x11);
                        touData.Add((byte)(tempDayProfileCount + 1));             //Day Id 
                        touData.Add(0x01);
                        touData.Add(0x0A);

                        for (byte j = 0; j < 10; j++)
                        {
                            touData.Add(0x02);
                            touData.Add(0x03);
                            touData.Add(0x09);
                            touData.Add(0x04);
                            touData.Add(Convert.ToByte(dayProfileGrids[0].Rows[j].Cells[COLSTARTHOUR].Value));      //   Slot Start Hour
                            touData.Add(Convert.ToByte(dayProfileGrids[0].Rows[j].Cells[COLSTARTMIN].Value));       //   Slot Start min
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x09);
                            touData.Add(0x06);
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x0A);
                            touData.Add(0x00);
                            touData.Add(0x64);
                            touData.Add(0xFF);
                            touData.Add(0x12);
                            touData.Add(0x00);
                            switch (Convert.ToString(dayProfileGrids[0].Rows[j].Cells[COLTARIFF].Value))
                            {
                                case "T1":
                                    touData.Add(0x01);
                                    break;
                                case "T2":
                                    touData.Add(0x02);
                                    break;
                                case "T3":
                                    touData.Add(0x03);
                                    break;
                                case "T4":
                                    touData.Add(0x04);
                                    break;
                                case "T5":
                                    touData.Add(0x05);
                                    break;
                                case "T6":
                                    touData.Add(0x06);
                                    break;
                                case "T7":
                                    touData.Add(0x07);
                                    break;
                                case "T8":
                                    touData.Add(0x08);
                                    break;
                                default:
                                    touData.Add(0x00);
                                    break;
                            }
                        }
                        //dayProfileCount++;

                    }

                    for (byte i = tempDayProfileCount; i <= TOUConstants.MaxDay; i++)
                    {
                        touData.Add(0x02);
                        touData.Add(0x02);
                        touData.Add(0x11);
                        touData.Add(0x00);             //Day Id 
                        touData.Add(0x01);
                        touData.Add(0x0A);

                        for (byte j = 0; j < 10; j++)
                        {
                            touData.Add(0x02);
                            touData.Add(0x03);
                            touData.Add(0x09);
                            touData.Add(0x04);
                            touData.Add(0x00);      //   Slot Start Hour
                            touData.Add(0x00);       //   Slot Start min
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x09);
                            touData.Add(0x06);
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x0A);
                            touData.Add(0x00);
                            touData.Add(0x64);
                            touData.Add(0xFF);
                            touData.Add(0x12);
                            touData.Add(0x00);
                            touData.Add(0x00);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDayProfileBuffer(int meterModel)", ex);
               //throw ex;
            }
            return touData.ToArray();
        }
        /// <summary>
        /// Used to get activation date buffer data 
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetActivationDateBuffer(int meterModel)
        {
            touData = new List<byte>();
            try
            {
                touData.Add(0x09);
                touData.Add(0x0C);
                touData.Add(Convert.ToByte((touActivationDate.Value.Year & 0xFF00) >> 8));
                touData.Add(Convert.ToByte(touActivationDate.Value.Year & 0x00FF));
                touData.Add(Convert.ToByte(touActivationDate.Value.Month)); //month
                touData.Add(Convert.ToByte(touActivationDate.Value.Day));  //day of month   

                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).dayofweek);
                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).hour);
                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).minute);
                //if (meterModel == NamePlateConstants.SapphireS2 || meterModel == NamePlateConstants.Sapphire_Netmeter_LTCT || meterModel == NamePlateConstants.Sapphire_Netmeter_WCM || meterModel == NamePlateConstants.BYPL_FD || meterModel == NamePlateConstants.PumaLTE650Value)
                //{
                   //touData.Add(Convert.ToByte(touActivationDate.Value.DayOfWeek));  //day of week   
                  // touData.Add(Convert.ToByte(touActivationDate.Value.Hour));  //hour
                  //  touData.Add(Convert.ToByte(touActivationDate.Value.Minute));  //min
                //}
                //else
                //{
                //    touData.Add(0xFF);  //day of week  
                //    touData.Add(0xFF);  //hour
                //    touData.Add(0xFF);  //min

                //}                         
                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).second);
                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).Hundreds); 
                touData.Add(0x80);
                touData.Add(0x00);
                touData.Add(GenericRTC.TOUACTIVATIONWRITE(meterModel, (int)touActivationDate.Value.DayOfWeek, touActivationDate.Value.Hour, touActivationDate.Value.Minute).clockstatus);

            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetActivationDateBuffer()", ex);
            }
            return touData.ToArray();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] GetCTRatioBuffer()
        {
            touData = new List<byte>();
            try
            {
                touData.Add(0x09);
                touData.Add(0x0C);
                touData.Add(Convert.ToByte((touActivationDate.Value.Year & 0xFF00) >> 8));
                touData.Add(Convert.ToByte(touActivationDate.Value.Year & 0x00FF));
                touData.Add(Convert.ToByte(touActivationDate.Value.Month)); //month
                touData.Add(Convert.ToByte(touActivationDate.Value.Day));  //day of month   
                touData.Add(0xFF);  //day of week
                touData.Add(0xFF);  //hh
                touData.Add(0xFF);  //mm
                touData.Add(0xFF);  //ss
                touData.Add(0xFF);
                touData.Add(0x80);
                touData.Add(0x00);
                touData.Add(0x00);
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetCTRatioBuffer()", ex);
            }            
            return touData.ToArray();

        }
        /// <summary>
        /// This method is used for filling session profile details in TOU grids.
        /// </summary>
        private void FillSeasonProfileParameters(byte[] buffer)
        {
            try
            {
                int nIndex = 2;
                int StartOfData = 0;
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                   // nIndex += 4;
                   
                    nIndex += 3;
                    StartOfData = buffer[5];
                    nIndex += StartOfData;
                    //Bug ID 502787
                    int tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLSESSION].Value = tariff.ToString("00");                        
                    }
                    else
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLSESSION].Value = null;
                    }
                    nIndex += 4;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 13)
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLMONTH].Value = tariff.ToString("00");                        
                    }
                    else
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLMONTH].Value = null;
                    }
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 32)
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLDAY].Value = tariff.ToString("00");                        
                    }
                    else
                    {
                        seasonProfileGrid.Rows[seasonCount].Cells[COLDAY].Value = null;
                    }
                    nIndex += 11;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillSeasonProfileParameters(byte[] buffer)", ex);
            }
        }

        
        /// <summary>
        /// This method is used for filling weekly profile details in TOU grids.
        /// </summary>
        private void FillWeekProfileParameters(byte[] buffer)
        {
            int nIndex = 2;

             int StartOfData = 0;
                StartOfData = buffer[5];                
         
            try
            {
                for (byte weekCount = 0; weekCount < weekProfileCount; weekCount++)
                {
                        nIndex += 4;
                    
                    nIndex += StartOfData;
                    for (byte colCount = 1; colCount < 8; colCount++)
                    {
                        //Bug ID 572397
                        //nIndex += 5;  
                        nIndex++;
                        int tariff = buffer[nIndex++];
                        if (tariff > 0 && tariff < 5)
                            weekProfileGrid.Rows[weekCount].Cells[colCount].Value = tariff.ToString("00");
                        else
                            weekProfileGrid.Rows[weekCount].Cells[colCount].Value = null;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillWeekProfileParameters(byte[] buffer)", ex);
               //throw ex;
            }
        }
        /// <summary>
        /// This method is used for filling day profile details in TOU grids.
        /// </summary>
        private void FillDayProfileParameters(byte[] buffer, int meterModel)
        {
            try
            {
                if (meterModel == NamePlateConstants.RubyE250Value && dayProfileCount == 2)
                {
                    int nIndex = 2;

                    nIndex += 6;
                    for (byte rowCount = 0; rowCount < 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        if (tariff == 0)
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }

                    for (byte dayCount = 1; dayCount < 6; dayCount++)
                    {
                        nIndex += 6;
                        for (byte rowCount = 0; rowCount < 10; rowCount++)
                        {
                            nIndex += 4;
                            nIndex++;
                            nIndex++;
                            nIndex += 12;
                            nIndex++;

                        }
                    }

                    nIndex += 6;
                    for (byte rowCount = 0; rowCount < 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
						int tariff = buffer[nIndex++];
                        if (tariff == 0)
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }
                }
                else
                {
                    int nIndex = 2;
                    dayProfileCount = buffer[1];                   
                    for (byte dayCount = 0; dayCount < dayProfileCount; dayCount++)
                    {
                        if (dayCount >= dayProfileGrids.Length)
                            break;
                        nIndex += 6;
                        for (byte rowCount = 0; rowCount < 10; rowCount++)
                        {
                            nIndex += 4;
                            string startHour = buffer[nIndex++].ToString("d2");
                            string startMin = buffer[nIndex++].ToString("d2");
                            nIndex += 12;
                            int tariff = buffer[nIndex++];
                            if (tariff == 0)
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                            }
                            else
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillDayProfileParameters(byte[] buffer, int meterModel)", ex);
            }
        }
        /// <summary>
        /// This method is used for filling TOU activation date.
        /// </summary>
        /// <param name="buffer"></param>        
        private void FillTOUActivationDate(byte[] buffer, int meterModel)
        {
            int nIndex = 0x02;
            int activationYear = 0;
            try
            {
                activationYear = (activationYear | (int)buffer[nIndex++]) << 8;
                activationYear = (activationYear | (int)buffer[nIndex++]);
                int activationMonth = buffer[nIndex++];
                int activationDay = buffer[nIndex++];
                nIndex++;
                int activationHour = buffer[nIndex++];
                int activationMin = buffer[nIndex];
                //******** Add hour min in TOU activation date Amendment 5 ************
                if (meterModel == NamePlateConstants.SapphireS2 || meterModel == NamePlateConstants.Sapphire_Netmeter_LTCT || meterModel == NamePlateConstants.Sapphire_Netmeter_WCM || meterModel == NamePlateConstants.BYPL_FD || meterModel == NamePlateConstants.BRPL_CBSP)//BYPL_FD for single phase VIM
                {
                    touActivationDate.Value = Convert.ToDateTime(activationDay.ToString() + "/" + activationMonth.ToString() + "/" + activationYear.ToString() + " " + activationHour.ToString() + ":" + activationMin.ToString(), new CultureInfo("hi-in"));
                }
                else
                {
                    touActivationDate.Value = Convert.ToDateTime(activationDay.ToString() + "/" + activationMonth.ToString() + "/" + activationYear.ToString(), new CultureInfo("hi-in")); 
                }
                  

                if (!(listSelectedParams.Contains(ProfileId.TwoTOU) || listSelectedParams.Contains(ProfileId.TOU)
                    || listSelectedParams.Contains(ProfileId.FourTOU) || listSelectedParams.Contains(ProfileId.ThreeSTOU)  || listSelectedParams.Contains(ProfileId.FourSPTOU)))
                {
                    if (dayProfileCount == 24)
                    {
                        listSelectedParams.Add(ProfileId.FourTOU);
                    }
                    else if (dayProfileCount == 2)
                    {
                        listSelectedParams.Add(ProfileId.TwoTOU);
                    }
                    else if (dayProfileCount == 3)
                    {
                        listSelectedParams.Add(ProfileId.ThreeSTOU);
                    }
                    else
                    {
                        listSelectedParams.Add(ProfileId.TOU);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillTOUActivationDate(byte[] buffer)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void SwitchActivePassiveTOU()
        {
            if (rdbTOUType.Checked)
            {
                if (passiveSeasonProfile != null && passiveWeekProfile != null && passiveDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(passiveSeasonProfile);
                    FillWeekProfileParameters(passiveWeekProfile);
                    FillDayProfileParameters(passiveDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                    FillTOUActivationDate(passiveActivationDate, Convert.ToInt32(ConfigInfo.MeterModel));
                }
            }
            else
            {
                if (activeSeasonProfile != null && activeWeekProfile != null && activeDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(activeSeasonProfile);
                    FillWeekProfileParameters(activeWeekProfile);
                    FillDayProfileParameters(activeDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                    FillTOUActivationDate(passiveActivationDate, Convert.ToInt32(ConfigInfo.MeterModel));
                }
            }
        }
        // ***********This code is added for smart meter Three phase ******
        private void SwitchActivePassiveTOU_Smartmeter()
        {
            if (rdbFutureTOU_smartmeter.Checked)
            {
                if (passiveSeasonProfile != null && passiveWeekProfile != null && passiveDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(passiveSeasonProfile);
                    FillWeekProfileParameters(passiveWeekProfile);
                    FillDayProfileParameters(passiveDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                    FillTOUActivationDate(passiveActivationDate, Convert.ToInt32(ConfigInfo.MeterModel));
                }
            }
            else
            {
                if (activeSeasonProfile != null && activeWeekProfile != null && activeDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(activeSeasonProfile);
                    FillWeekProfileParameters(activeWeekProfile);
                    FillDayProfileParameters(activeDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                    FillTOUActivationDate(passiveActivationDate, Convert.ToInt32(ConfigInfo.MeterModel));
                }
            }
        }

        #endregion



        private void ParseDLMSData(string fileContentDLMS, string iecTOUData)
                            {
            string[] profileWiseData = fileContentDLMS.Split('\n');
                    ProfileCommand profileCommand;
                    foreach (string profileData in profileWiseData)
                    {
                        string actualData = profileData.TrimEnd('\r');
                        if (Array.IndexOf(profileWiseData, profileData) == 0 || Array.IndexOf(profileWiseData, profileData) == profileWiseData.Length - 1)
                            continue;
                        DLMSCOMMAND dlmsCommand = new GenerateEntity().GetCommandFromCommandRepository(actualData.Substring(0, 16));
                        byte[] receivedData = SoapHexBinary.Parse(actualData.Substring(16)).Value;
                        profileCommand = new ProfileCommand();
                        profileCommand.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                        profileCommand.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                        profileCommand.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                        profileCommand.ObisCode = dlmsCommand.OBISCODE;
                        profileCommand.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                        profileCommand.ClassName = dlmsCommand.CLASSNAME;
                        //This is exception for CT and PT ratio as commands for CT and PT ratio are also present with general profile in Commandrepository.
                        if (dlmsCommand.OBISCODE == "01.00.00.04.02.FF")
                        {
                            profileCommand.TagNumber = (int)ProfileId.CTRatio;
                            profileCommand.ClassName = "CAB.E650MeterConfiguration.CTRatio";
                        }
                        if (dlmsCommand.OBISCODE == "01.00.00.04.03.FF")
                        {
                            profileCommand.TagNumber = (int)ProfileId.PTRatio;
                            profileCommand.ClassName = "CAB.E650MeterConfiguration.PTRatio";
                        }
                        switch (profileCommand.TagNumber)
                        {

                            case (int)ProfileId.RTC:
                                DisplayMeterRTC(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.DIPWithSliding:
                                DisplayDIPWithSliding(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.DIP:
                                DisplayDIP(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.SIP:
                                DisplayLSCapturePeriod(receivedData);
                                break;
                            case (int)ProfileId.BillingReset:
                                chkMDReset.Checked = receivedData[1] == 1 ? true : false;
                                listSelectedParams.Add(ProfileId.BillingReset);
                                break;
                            case (int)ProfileId.BillingType:
                                DisplayBillingDateTime(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.BillingMonthType:
                                DisplayBillingMonthType(receivedData, profileCommand);
                                break; //[BillingType_Month]
                            case (int)ProfileId.ResetLockOutDays:
                                DisplayBillingResetLockOutDays(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.KvahSelection:
                                DisplayKVAhSelection(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.RS232LockUnlock:
                                DisplayRS232LockUnlock(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.AutoLock:
                                DisplayAutoLockUnlock(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.PassiveSeasonProfile:
                                if (string.IsNullOrEmpty(iecTOUData))
                                {
                                    FillSeasonProfileParameters(receivedData);
                                }
                                break;
                            case (int)ProfileId.PassiveWeekProfile:
                                if (string.IsNullOrEmpty(iecTOUData))
                                {
                                    FillWeekProfileParameters(receivedData);
                                }
                                break;
                            case (int)ProfileId.PassiveDayProfile:
                                if (string.IsNullOrEmpty(iecTOUData))
                                {
                                    FillDayProfileParameters(receivedData, Convert.ToInt32(ConfigInfo.MeterModel));                                   
                                }
                                break;
                            case (int)ProfileId.ActivationDate:
                                if (string.IsNullOrEmpty(iecTOUData))
                                {
                                    FillTOUActivationDate(receivedData, Convert.ToInt32(ConfigInfo.MeterModel));
                                }
                                break;
                            case (int)ProfileId.PushDisplayParameter:
                                ShowDispayParameters(receivedData, DisplayParameterType.PushMode, profileCommand);
                                break;
                            case (int)ProfileId.ScrollDisplyParameter:
                                ShowDispayParameters(receivedData, DisplayParameterType.ScrollMode, profileCommand);
                                break;
                            case (int)ProfileId.HighResolutionDisplayParameter:
                                ShowDispayParameters(receivedData, DisplayParameterType.HighResolutionMode, profileCommand);
                                break;
                            // Story - Hide Display Timeout Parameter
                            //case (int)ProfileId.DisplayTimeoutParameter:
                            //    FillDisplayParametersTimeouts(receivedData, profileCommand);
                            //    break;
                            case (int)ProfileId.CTRatio:
                                DisplayCTRatio(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.PTRatio:
                                DisplayPTRatio(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.ManualBilling:
                                DisplayManualBilling(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.SoftwareBilling:
                                DisplaySoftwareBilling(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.MagneticTamperIcon:
                                checkBoxMagneticTamperIcon.Checked = receivedData[1] == 1 ? true : false;
                                listSelectedParams.Add(ProfileId.MagneticTamperIcon);
                                break;
                            // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter model = 17 having specific right authority to reset
                            case (int)ProfileId.MagneticTamperIcon3P:
                                checkBoxMagneticTamperIcon.Checked = receivedData[1] == 1 ? true : false;
                                listSelectedParams.Add(ProfileId.MagneticTamperIcon3P);
                                break;
                            case (int)ProfileId.PulseEnergy:
                                DisplayPulseEnergySelection(receivedData, profileCommand);
                                break;
                            default:
                                break;
                        }
            }                   
        }


      

        private void ParseNonDLMSData(string fileContentNonDLMS)
        {
            try
            {
                string[] profileWiseData = fileContentNonDLMS.Split('\n');
                foreach (Parameters item in NonDLMSConfiguration.Instance.lstParameter)
                {
                    switch (item.SNo)
                    {
                        case 1:
                            {
                                if (profileWiseData.Length >= (item.StartPosition + item.Length) && !profileWiseData[item.StartPosition].Contains("$"))
                                {
                                    DisplayIECPushData(profileWiseData, item.StartPosition, item.Length);
                                }                                 
                            }
                            break;

                        case 2:
                            {
                                if (profileWiseData.Length >= (item.StartPosition + item.Length) && !profileWiseData[item.StartPosition].Contains("$"))
                                {
                                    DisplayIECScrollData(profileWiseData, item.StartPosition, item.Length);
                                }
                            }
                            break;

                        case 3:
                            {
                                if (profileWiseData.Length >= (item.StartPosition + item.Length) && !profileWiseData[item.StartPosition].Contains("$"))
                                {
                                    DisplayIECHighData(profileWiseData, item.StartPosition, item.Length);
                                }
                            }
                            break;


                            //
                        case 4:
                            {
                                if (!IsOffline)
                                {
                                    if (profileWiseData.Length >= (item.StartPosition + item.Length) && !profileWiseData[item.StartPosition].Contains("$"))
                                    {
                                        DisplaykVAhSelection(profileWiseData, item.StartPosition);
                                    }
                                }
                            }
                            break;
                         default:
                                break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ParseNonDLMSData(string fileContentNonDLMS)", ex);
            }
        }

        private void DisplaykVAhSelection(string[] profileWiseData, int startPosition)
        {
            try
            {
                
                listSelectedParams.Add(ProfileId.KvahSelection);
                bool bLagOnly = Convert.ToBoolean(Convert.ToInt32(profileWiseData[startPosition].Trim('(').Trim(')')));
                if (bLagOnly)
                    rdbKVAhLagOnly.Checked = true;
                else
                    rdbKVAhLagLead.Checked = true;
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "DisplaykVAhSelection(string[] profileWiseData, int startIndex, int length)", ex);
            }
        }

        private void DisplayIECHighData(string[] profileWiseData, int startIndex, int length)
        {
            try
            {
                if (!listSelectedParams.Contains(ProfileId.DisplayParametersIEC))
                {
                    listSelectedParams.Add(ProfileId.DisplayParametersIEC);
                }

                Dictionary<string, string> dicPushData = new Dictionary<string, string>();
                for (int i = startIndex; i < (startIndex + length); i++)
                {
                    dicPushData.Add(i.ToString(), profileWiseData[i]);
                }
                objDisplayParameterIECConfig.SetHighButtonSelectedList(dicPushData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayIECHighData(string[] profileWiseData, int startIndex, int length)", ex);

            }
        }

        private void DisplayIECScrollData(string[] profileWiseData, int startIndex, int length)
        {
            try
            {
                if (!listSelectedParams.Contains(ProfileId.DisplayParametersIEC))
                {
                    listSelectedParams.Add(ProfileId.DisplayParametersIEC);
                }

                Dictionary<string, string> dicPushData = new Dictionary<string, string>();
                for (int i = startIndex; i < (startIndex + length); i++)
                {
                    dicPushData.Add(i.ToString(), profileWiseData[i]);
                }
                objDisplayParameterIECConfig.SetScrollButtonSelectedList(dicPushData);
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "DisplayIECScrollData(string[] profileWiseData, int startIndex, int length)", ex);
            }
        }

        private void DisplayIECPushData(string[] profileWiseData, int startIndex, int length)
        {
            try
            {
                if (!listSelectedParams.Contains(ProfileId.DisplayParametersIEC))
                {
                    listSelectedParams.Add(ProfileId.DisplayParametersIEC);
                }

                Dictionary<string,string> dicPushData = new Dictionary<string,string>();
                for (int i = startIndex; i < (startIndex + length); i++)
                {
                    dicPushData.Add(i.ToString(),profileWiseData[i]);
                }
                objDisplayParameterIECConfig.SetPushButtonSelectedList(dicPushData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayIECPushData(string[] profileWiseData, int startIndex, int length)", ex);

            }
        }

      


        /// <summary>
        /// Display configuration from file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool DisplayConfigurationFromFile(string fileName)
        {
            bool result = true;
            try
            {
                string fileContent = string.Empty;
                string iecTOUData = string.Empty;
                string fileContentDLMS = string.Empty;
                string fileContentNonDLMS = string.Empty;
                if (File.Exists(fileName))
                {
                    StreamReader streamReader = new StreamReader(fileName);
                    fileContent = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (!string.IsNullOrEmpty(fileContent))
                {
                    if (fileContent.Contains(BCSConstants.IEC))
                    {
                        if (fileContent.IndexOf("<") > -1)
                        {
                            iecTOUData = fileContent.Substring(fileContent.IndexOf("<"));
                            rdbTOUWithHoliday.Checked = true;
                            iecTOUData = GetTOUFileContent(iecTOUData);
                            if (!string.IsNullOrEmpty(iecTOUData))
                            {
                                DisplayTOU(iecTOUData);
                                listSelectedParams.Add(ProfileId.FourTOUWithHoliday);
                            }
                        }
                        fileContentDLMS = fileContent.Substring(0, fileContent.IndexOf(BCSConstants.IEC));
                        fileContentNonDLMS = fileContent.Substring(fileContent.IndexOf(BCSConstants.IEC));
                    }
                    ParseDLMSData(fileContentDLMS, iecTOUData);
                    ParseNonDLMSData(fileContentNonDLMS);                  
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayConfigurationFromFile(string fileName)", ex);
                result = false;
            }
            this.StatusMessage = "";
            return result;
        }

      

        /// <summary>
        /// Writes Meter Config Data
        /// </summary>
        /// <param name="selectedProfiles"></param>
        /// <param name="isRemote"></param>
        /// <param name="simIndex"></param>
        /// <returns></returns>
        private bool WriteMeterConfigData(List<System.Enum> selectedProfiles, bool isRemote, int simIndex)
        {
            bool isSuccess = false;
            //int meterModelNumber;// = NamePlateConstants.PumaLTE650Value;
            ProfileCommand selectedCommand;
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            if (result.ErrorCode == CommunicationErrorType.Success)
            {
                List<ProfileCommand> lstProfileCommands;
                lstProfileCommands = GetProfileCommandEntity();
                //btnAbort.Enabled = true;
                foreach (ProfileId selectedConfigId in selectedProfiles)
                {
                    //ConfigInfo.MeterModel = "44";
                    //Filter one command entity
                    List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        return profileCommandEntity.TagNumber == (int)selectedConfigId
                        && (Convert.ToString(profileCommandEntity.MeterModelNumber) == ConfigInfo.MeterModel ||
                        profileCommandEntity.MeterModelNumber == 0);
                    });

                    //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                    if (selectedConfigId == ProfileId.RTC)
                    {
                        ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                        rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                        profileCommand.Add(rtcCommand);
                    }

                    if (profileCommand.Count > 0)
                    {
                        // HTCT Specific Changes
                        if (selectedConfigId == ProfileId.KvahSelection && ConfigInfo.MeterModel == "10")
                        {
                            this.StatusMessage = "Writing Mvah Selection" + " ...";
                        }
                        else
                        {
                            this.StatusMessage = "Writing " + CommonBLL.GetEnumDescription(selectedConfigId) + " ...";
                        }
                        if (selectedConfigId == ProfileId.PassiveDayProfile || selectedConfigId == ProfileId.PassiveSeasonProfile
                            || selectedConfigId == ProfileId.PassiveWeekProfile || selectedConfigId == ProfileId.ActivationDate)
                        {
                            SetGrid(ProfileId.TOU, System.Drawing.Color.LightYellow, "Writing Data...");
                        }
                        //else if (selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter
                        //    || selectedConfigId == ProfileId.DisplayTimeoutParameter)
                        else if (selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter)
                        // Story - Hide Display Timeout Parameter
                        {
                            SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.LightYellow, "Writing Data...");
                        }
                        else
                        {
                            SetGrid(selectedConfigId, System.Drawing.Color.LightYellow, "Writing Data...");
                        }

                        Application.DoEvents();
                         if ((ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString()) && (profileCommand.Count > 1) && (selectedConfigId == ProfileId.KvahSelection || selectedConfigId == ProfileId.AutoLock || selectedConfigId == ProfileId.SoftwareBilling))
                         selectedCommand = profileCommand[1];
                        else
                        selectedCommand = profileCommand[0];
                        selectedCommand.Action = ActionType.WRITE;
                        //if (selectedConfigId == ProfileId.DIP && cmbDIPDemandType.SelectedItem.ToString() == "Sliding Demand")
                        //{
                        //    profileCommand[0].ClassId = 1;
                        //    profileCommand[0].ObisCode = "00.00.60.01.99.FF";
                        //    profileCommand[0].Attribute = 2;
                        //}
                        //Fill WriteData buffer for corresponding programming parameter
                        switch (selectedConfigId)
                        {
                            case ProfileId.RTC:
                              profileCommand[0].WriteDataBuffer = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                break;
                            case ProfileId.DIP:
                                profileCommand[0].WriteDataBuffer = FillDIPData();
                                profileCommand[1].Action = ActionType.WRITE;
                                profileCommand[1].WriteDataBuffer = FillSlideSubDIP ();
                                break;
                            case ProfileId.SIP:
                                profileCommand[0].WriteDataBuffer = FillSIPData();
                                break;
                            case ProfileId.BillingReset:
                                //No need to send any data for MD reset
                                profileCommand[0].Action = ActionType.RESET;
                                profileCommand[0].MeterModelNumber = Convert.ToByte(ConfigInfo.MeterModel);                               
                                break;
                            case ProfileId.BillingType:
                                profileCommand[0].WriteDataBuffer = FillBillingTypeData();
                                break;
                            case ProfileId.BillingMonthType:
                                profileCommand[0].WriteDataBuffer = FillBillingMonthTypeData();
                                break; // [BillingType_Month]
                            case ProfileId.ResetLockOutDays:
                                profileCommand[0].WriteDataBuffer = Convert.ToByte(cmbResetLockoutdays.Text);
                                break;

                            case ProfileId.KvahSelection:

                                //**********For sapphire S2 optima**************
                                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString() && profileCommand.Count > 1)
                                    profileCommand[1].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                else
                                    profileCommand[0].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                break;
                            case ProfileId.RS232LockUnlock:
                                profileCommand[0].WriteDataBuffer = rdbRS232Lock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                break;

                            case ProfileId.AutoLock:

                                //***********For sapphire S2**************
                                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString() && profileCommand.Count > 1)
                                    profileCommand[1].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                else
                                    profileCommand[0].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                break;
                            case ProfileId.PassiveSeasonProfile:
                                //******* Meter Model Change Required Here ***********//
                                if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VBSPNoSeasonNoWeek || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VFSPNoSeasonNoWeek) continue;
                                profileCommand[0].WriteDataBuffer = GetSeasonProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                                break;
                            case ProfileId.PassiveWeekProfile:

                                //******* Meter Model Change Required Here ***********//
                                if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VBSPNoSeasonNoWeek || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VFSPNoSeasonNoWeek) continue;
                                profileCommand[0].WriteDataBuffer = GetWeekProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                                break;
                            //**********smart meter special day profile************
                            case ProfileId.SpecialDayProfileSmartMeter:
                                profileCommand[0].WriteDataBuffer = GetSplDayProfileBuffer();
                                break;
                            case ProfileId.PassiveDayProfile:
                                profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                                break;
                            case ProfileId.ActivationDate:
                                profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(Convert.ToInt32(ConfigInfo.MeterModel));
                                break;
                            case ProfileId.PushDisplayParameter:
                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                                SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Write Successful.");
                                break;
                            case ProfileId.ScrollDisplyParameter:
                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                                SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Write Successful.");
                                break;
                            case ProfileId.HighResolutionDisplayParameter:
                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVHighResolution);
                                SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Write Successful.");
                                break;
                            // Story - Hide Display Timeout Parameter
                            //case ProfileId.DisplayTimeoutParameter:
                            //    profileCommand[0].WriteDataBuffer = GetDisplayTimeoutData();
                            //    break;
                            case ProfileId.CTRatio:
                                profileCommand[0].WriteDataBuffer = Convert.ToInt32(nudCTRatio.Value);
                                break;
                            case ProfileId.PTRatio:
                                profileCommand[0].WriteDataBuffer = Convert.ToInt32(nudPTRatio.Value);
                                break;
                            case ProfileId.ManualBilling:
                                profileCommand[0].WriteDataBuffer = rdbEnableManualBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                break;

                            case ProfileId.SoftwareBilling:

                                //***********For sapphire S2**************
                                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString() && profileCommand.Count > 1)
                                    profileCommand[1].WriteDataBuffer = rdbEnableSoftwareBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                else
                                    profileCommand[0].WriteDataBuffer = rdbEnableSoftwareBilling.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                break;
                     //********* For smart meter load control and disconnect control*******
                            case ProfileId.LoadControl:
                                profileCommand[0].WriteDataBuffer = WriteLoadControl();
                                break;
                            case ProfileId.LoadControl1PSmartMeter:
                                profileCommand[0].WriteDataBuffer = WriteLoadControl1PSmartMeter();
                                break;
                                
                            case ProfileId.DisconnectControl:
                                if (chkconnect.Checked)
                                {
                                    profileCommand[0].Attribute = 0x02;
                                }
                                else if (chkDisconnect.Checked)
                                {
                                    profileCommand[0].Attribute = 0x01;
                                }
                                profileCommand[0].WriteDataBuffer = WriteDisconnectControl();
                                profileCommand[0].Action = ActionType.ACTIONREQUEST;
                                break;
                            //********* For HTCT Meter*******
                            case ProfileId.RS485:
                                profileCommand[0].WriteDataBuffer = WriteRS485();
                                break;
                            // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter model = 17 having specific right authority to reset
                            case ProfileId.MagneticTamperIcon3P:
                                profileCommand[0].Action = ActionType.RESET;
                                profileCommand[0].MeterModelNumber = Convert.ToByte(ConfigInfo.MeterModel);
                                break;
                            case ProfileId.PulseEnergy:
                                profileCommand[0].WriteDataBuffer = FillPulseEnergyTypeData();
                                break;
                            case ProfileId.ManualButtonMDReset:
                                profileCommand[0].WriteDataBuffer = profileCommand[0].WriteDataBuffer = rdbMDResetEnable.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                break;

                            default:
                                break;
                        }
                        if (selectedConfigId == ProfileId.DIP)
                        {
                            //******* Meter Model Change Required Here ***********//
                            if (ConfigInfo.SignatureInfo.ToUpperInvariant().Contains("VB") 
                                || ConfigInfo.SignatureInfo.Contains("VF") 
                                || ConfigInfo.SignatureInfo.Contains("FS") 
                                || ConfigInfo.SignatureInfo.Contains("SK") 
                                || ConfigInfo.SignatureInfo.Contains("SF") 
                                || ConfigInfo.SignatureInfo.Contains("HK") 
                                || ConfigInfo.SignatureInfo.Contains("CF") 
                                || ConfigInfo.SignatureInfo.Contains("BF") 
                                || ConfigInfo.SignatureInfo.Contains("RF") 
                                || ConfigInfo.SignatureInfo.Contains("BK")
                                || ConfigInfo.SignatureInfo.Contains("CB")  //user story 1016689
                                || ConfigInfo.SignatureInfo.Contains("SM0110")
                                || ConfigInfo.SignatureInfo.Contains("SM0310")
                                || ConfigInfo.SignatureInfo.Contains("SM0405")
                                ) //SarkarA code change 20180129 // add HK model
                            {
                                //For this Meter Model send only first Profile commands
                                result = communication.Send(profileCommand[cmbDIPDemandType.SelectedIndex]);
                            }
                            else
                            {

                                //**************Sapphire S2 DIPsupport Block demand "Not supported Sliding Demand" ***************                               

                                    if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString() && profileCommand.Count > 1 && cmbDIPDemandType.SelectedItem.ToString() != "Sliding Demand")
                                    result = communication.Send(profileCommand[0]);
                                else
                                {

                                    // For rest Meter Model send both the command
                                    if (cmbDIPDemandType.SelectedItem.ToString() == "Sliding Demand")
                                    {
                                        result = communication.Send(profileCommand[0]);
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                            result = communication.Send(profileCommand[1]);
                                    }
                                    else  //cmbDIPDemandType.SelectedItem.ToString() == "Block Demand")
                                    {
                                        result = communication.Send(profileCommand[1]);
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                            result = communication.Send(profileCommand[0]);
                                    }
                                }
                            }
                        }
                        else if ((selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter || selectedConfigId == ProfileId.HighResolutionDisplayParameter) && ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
                        {
                            result = communication.SendWriteBlock(profileCommand[0]);
                        }
                        else
                        {
                            //***********For sapphire S2 optima**************
                            if ((ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString()) && (profileCommand.Count > 1) && (selectedConfigId == ProfileId.KvahSelection || selectedConfigId == ProfileId.AutoLock || selectedConfigId == ProfileId.SoftwareBilling))
                              result = communication.Send(profileCommand[1]);
                            else
                                result = communication.Send(profileCommand[0]);
                        }
                           
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {

                            // Story - Hide Display Timeout Parameter
                            //if (selectedConfigId == ProfileId.DisplayTimeoutParameter)
                            //{
                            //    lngGridViewReadControl1.UncheckCheckBox(ProfileId.DisplayParameters);
                            //    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Write Successful.");
                            //}
                            if (selectedConfigId == ProfileId.ActivationDate)
                            {
                                lngGridViewReadControl1.UncheckCheckBox(ProfileId.TOU);
                                SetGrid(ProfileId.TOU, System.Drawing.Color.Green, "Write Successful.");
                            }
                            else
                            {
                                lngGridViewReadControl1.UncheckCheckBox(selectedConfigId);
                                SetGrid(selectedConfigId, System.Drawing.Color.Green, "Write Successful.");
                            }
                            continue;
                        }
                        else
                        {
                            if (selectedConfigId == ProfileId.PushDisplayParameter ||
                                selectedConfigId == ProfileId.HighResolutionDisplayParameter ||
                                selectedConfigId == ProfileId.ScrollDisplyParameter) //|| selectedConfigId == ProfileId.DisplayTimeoutParameter)// Story - Hide Display Timeout Parameter
                            {
                                lngGridViewReadControl1.UncheckCheckBox(ProfileId.DisplayParameters);
                                SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.LightPink, CommonBLL.GetEnumDescription(result.ErrorCode));
                            }
                            else if (selectedConfigId == ProfileId.ActivationDate ||
                                selectedConfigId == ProfileId.PassiveDayProfile || selectedConfigId == ProfileId.PassiveSeasonProfile ||
                                selectedConfigId == ProfileId.PassiveWeekProfile)
                            {
                                lngGridViewReadControl1.UncheckCheckBox(ProfileId.TOU);
                                SetGrid(ProfileId.TOU, System.Drawing.Color.LightPink, CommonBLL.GetEnumDescription(result.ErrorCode));
                            }
                            else if (selectedConfigId == ProfileId.BillingMonthType && result.ErrorCode == CommunicationErrorType.AccessDenied)
                            {
                                //Billing Month Type 
                                //If Access Denied comes from Meter then show Success message for Billing Type "Other" (if NAC(03) response come for Billing Cycle write command)                                
                                result.ErrorCode = CommunicationErrorType.Success;
                            }
                            else
                            {
                                lngGridViewReadControl1.UncheckCheckBox(selectedConfigId);
                                SetGrid(selectedConfigId, System.Drawing.Color.LightPink, CommonBLL.GetEnumDescription(result.ErrorCode));
                            }
                            //result.ErrorCode = CommunicationErrorType.PasswordInavalid;
                            this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                            isSuccess = false;
                            break;
                            Application.DoEvents();
                            //break;
                        }
                        isSuccess = true;

                    }
                    //else if (result.ErrorCode == CommunicationErrorType.AccessDenied)
                    //{
                    //    SetGrid(selectedConfigId, System.Drawing.Color.Red, "Access Denied");
                    //    isSuccess = false;
                    //}
                }
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    this.StatusMessage = "Data Written Successfully.";
                    isSuccess = true;
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
            }
            else
            {
                this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                Application.DoEvents();
            }
            return isSuccess;
        }

        /// <summary>
        /// Gets Meter Configuration Data
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private bool GetMeterConfigData(List<System.Enum> selectedProfiles, bool isRemote, int simIndex)
        {
            int objint;
            decimal objdec;
            bool isAnyReadSuccess = false;
            //int meterModelNumber = NamePlateConstants.PumaLTE650Value;
            ProfileCommand selectedCommand;
            List<ProfileCommand> lstProfileCommands = GetProfileCommandEntity();
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;
            foreach (ProfileId selectedConfigId in selectedProfiles)
            {
                //Filter one command entity
                List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {                    
                    return profileCommandEntity.TagNumber == (int)selectedConfigId
                    && (Convert.ToString(profileCommandEntity.MeterModelNumber) == ConfigInfo.MeterModel ||
                    profileCommandEntity.MeterModelNumber == 0);
                });
                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                if (selectedConfigId == ProfileId.RTC)
                {
                    ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                    profileCommand.Add(rtcCommand);
                }
                if (profileCommand.Count > 0)
                {

                    //*************For sapphire S2 KVah,MDReset,Autolock,Softwarebilling on same OBIS********
                    if ((ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString()) && (profileCommand.Count > 1) && (selectedConfigId == ProfileId.KvahSelection || selectedConfigId == ProfileId.AutoLock || selectedConfigId == ProfileId.SoftwareBilling))
                       
                        selectedCommand = profileCommand[1];
                    else
                        selectedCommand = profileCommand[0];
                    selectedCommand.Action = ActionType.READ;
                    //Bug #208382 
                    if (selectedConfigId == ProfileId.BillingReset || selectedConfigId == ProfileId.MagneticTamperIcon3P)
                    {
                        lngGridViewReadControl1.UncheckCheckBox(ProfileId.BillingReset);
                        lngGridViewReadControl1.UncheckCheckBox(ProfileId.MagneticTamperIcon3P);
                        continue;
                    }
                    else
                    {
                        // HTCT integration
                        if (selectedConfigId == ProfileId.KvahSelection && ConfigInfo.MeterModel == "10")
                        {
                            this.StatusMessage = "Reading Mvah Selection" + " ...";
                        }
                        else
                        {
                            this.StatusMessage = "Reading " + CommonBLL.GetEnumDescription(selectedConfigId) + " ...";
                        }
                        if (touParameters.Contains(selectedConfigId))
                        {
                            SetGrid(ProfileId.TOU, System.Drawing.Color.LightYellow, "Reading Data...");
                        }
                        else if (touParameters.Contains(selectedConfigId))
                        {
                        }
                        
                    }
                    Application.DoEvents();
                    result = communication.Send(selectedCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success || result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                    {
                        if ((result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0) || selectedConfigId == ProfileId.BillingReset)
                        {
                            isAnyReadSuccess = true;
                            switch (selectedConfigId)
                            {
                                case ProfileId.RTC:
                                    DisplayMeterRTC(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.RTC, System.Drawing.Color.Green, "Read Successful");
                                    break;

                                case ProfileId.DIP:
                                    DisplayDIP(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    selectedCommand.ClassId = 1;
                                    selectedCommand.ObisCode = "00.00.60.01.99.FF";
                                    selectedCommand.Attribute = 2;
                                    result = communication.Send(selectedCommand);
                                    if (result.ErrorCode == CommunicationErrorType.Success
                                        || result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                                    {
                                        DisplayDIP(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    }

                                    SetGrid(ProfileId.DIP, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.SIP:
                                    DisplayLSCapturePeriod(result.RecieveDataBuffer.ToArray());
                                    SetGrid(ProfileId.SIP, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.BillingReset:
                                    lngGridViewReadControl1.UncheckCheckBox(ProfileId.BillingReset);
                                    break;
                                case ProfileId.MagneticTamperIcon:
                                    lngGridViewReadControl1.UncheckCheckBox(ProfileId.MagneticTamperIcon);
                                    break;
                                // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter model = 17 having specific right authority to reset
                                case ProfileId.MagneticTamperIcon3P:
                                    lngGridViewReadControl1.UncheckCheckBox(ProfileId.MagneticTamperIcon3P);
                                    break;
                                case ProfileId.BillingType:
                                    DisplayBillingDateTime(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.BillingType, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.BillingMonthType:
                                    //
                                    if (otherBillingType.Checked == true)
                                    {
                                        DisplayBillingMonthType(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                        SetGrid(ProfileId.BillingType, System.Drawing.Color.Green, "Read Successful");
                                    }
                                    break; // [BillingType_Month]
                                case ProfileId.ResetLockOutDays:
                                    DisplayBillingResetLockOutDays(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.ResetLockOutDays, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.KvahSelection:
                                    DisplayKVAhSelection(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.KvahSelection, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.RS232LockUnlock:
                                    DisplayRS232LockUnlock(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.RS232LockUnlock, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.AutoLock:
                                    DisplayAutoLockUnlock(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.AutoLock, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                    //***** this code is added for smart meter *******
                                case ProfileId.SpecialDayProfileSmartMeter:
                                    SpecialDayProfile = result.RecieveDataBuffer.ToArray();
                                 
                                    if (rdbTOUType.Checked)
                                    {
                                        FillSpecialDayProfileParameters(SpecialDayProfile);
                                    }
                                    break;
                                case ProfileId.PassiveSeasonProfile:
                                    passiveSeasonProfile = result.RecieveDataBuffer.ToArray();
                                    if (rdbTOUType.Checked)
                                    {
                                        FillSeasonProfileParameters(passiveSeasonProfile);
                                    }
                                    break;
                                case ProfileId.PassiveWeekProfile:
                                    passiveWeekProfile = result.RecieveDataBuffer.ToArray();
                                    if (rdbTOUType.Checked)
                                    {
                                        FillWeekProfileParameters(passiveWeekProfile);
                                    }
                                    break;
                                case ProfileId.PassiveDayProfile:
                                    passiveDayProfile = result.RecieveDataBuffer.ToArray();
                                    if (rdbTOUType.Checked)
                                    {
                                        FillDayProfileParameters(passiveDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                                    }
                                    break;
                                case ProfileId.ActiveSeasonProfile:
                                    activeSeasonProfile = result.RecieveDataBuffer.ToArray();
                                    if (!rdbTOUType.Checked)
                                    {
                                        FillSeasonProfileParameters(activeSeasonProfile);
                                    }
                                    break;
                                case ProfileId.ActiveWeekProfile:
                                    activeWeekProfile = result.RecieveDataBuffer.ToArray();
                                    if (!rdbTOUType.Checked)
                                    {
                                          FillWeekProfileParameters(activeWeekProfile);
                                    }
                                        break;
                                case ProfileId.ActiveDayProfile:
                                    activeDayProfile = result.RecieveDataBuffer.ToArray();
                                    if (!rdbTOUType.Checked)
                                    {
                                        FillDayProfileParameters(activeDayProfile, Convert.ToInt32(ConfigInfo.MeterModel));
                                    }
                                    break;
                                case ProfileId.ActivationDate:
                                    passiveActivationDate = result.RecieveDataBuffer.ToArray();
                                    FillTOUActivationDate(passiveActivationDate, Convert.ToInt32(ConfigInfo.MeterModel));
                                    SetGrid(ProfileId.TOU, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.PushDisplayParameter:
                                    ShowDispayParameters(result.RecieveDataBuffer.ToArray(), DisplayParameterType.PushMode, selectedCommand);
                                    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Read Successful");
                                    //DisplayParameters
                                    break;
                                case ProfileId.ScrollDisplyParameter:
                                    ShowDispayParameters(result.RecieveDataBuffer.ToArray(), DisplayParameterType.ScrollMode, selectedCommand);
                                    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.HighResolutionDisplayParameter:
                                    ShowDispayParameters(result.RecieveDataBuffer.ToArray(), DisplayParameterType.HighResolutionMode, selectedCommand);
                                    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                // Story - Hide Display Timeout Parameter
                                //case ProfileId.DisplayTimeoutParameter:
                                //    FillDisplayParametersTimeouts(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                //    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Read Successful");
                                //    break;
                                case ProfileId.CTRatio:
                                    DisplayCTRatio(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.CTRatio, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.PTRatio:
                                    DisplayPTRatio(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.PTRatio, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.ManualBilling:
                                    DisplayManualBilling(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.ManualBilling, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.SoftwareBilling:
                                    DisplaySoftwareBilling(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.SoftwareBilling, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.LoadControl:
                                    DisplayLoadControl(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.LoadControl, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.DisconnectControl:
                                    DisplayConnectDisconnectStatus(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.DisconnectControl, System.Drawing.Color.Green, "Read Successful");
                                    break;  
                                case ProfileId.LoadControl1PSmartMeter:
                                    DisplayLoadControl1PSmartMeter(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    //if (int.TryParse(txtOverLoadthreshhold.Text, out objint))
                                    //    txtOverLoadthreshhold.Text = (Convert.ToDecimal(txtOverLoadthreshhold.Text.Trim()) / 1000).ToString("0.000");
                                    //if (decimal.TryParse(txtOCthreshold.Text, out objdec)) txtOCthreshold.Text = (Convert.ToDecimal(txtOCthreshold.Text.Trim()) / 100).ToString("0.00");
                                    SetGrid(ProfileId.LoadControl, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.RS485:
                                    DisplayRS485(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.RS485, System.Drawing.Color.Green, "Read Successful");
                                    break;

                                case ProfileId.PulseEnergy:
                                    DisplayPulseEnergySelection(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.PulseEnergy, System.Drawing.Color.Green, "Read Successful");
                                    break;
                                case ProfileId.ManualButtonMDReset:
                                    DisplayManualButtonMDReset(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                    SetGrid(ProfileId.ManualButtonMDReset, System.Drawing.Color.Green, "Read Successful");
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            UpdateGridViewAndStatusBar(selectedConfigId);
                        }
                    }
                    else
                    {
                        UpdateGridViewAndStatusBar(selectedConfigId);
                    }
                }
            }
            if (isAnyReadSuccess)
                this.StatusMessage = "Readout Successful.";



            return isAnyReadSuccess;
        }

        private void FillSpecialDayProfileParameters(byte[] SpecialDayProfile)
        {
            try
            {
                int nIndex = 11;
                SpecialDayProfileCount =Convert.ToByte(SpecialDayProfile[1]);
                for (byte seasonCount = 0; seasonCount < SpecialDayProfileCount; seasonCount++)
                {
                    int columnIndex = 1;
                    //Bug ID 502787
                    int Tariff = SpecialDayProfile[nIndex++];
                    //month
                    if (Tariff > 0 && Tariff < 13)
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = Tariff.ToString("00");
                    }
                    else
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = null;
                    }
                    Tariff = SpecialDayProfile[nIndex++];
                    //Date
                    if (Tariff > 0 && Tariff < 32)
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = Tariff.ToString("00");
                    }
                    else
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = null;
                    }
                    nIndex += 2;
                    Tariff = SpecialDayProfile[nIndex++];
                    //DayID
                    if (Tariff > 0 && Tariff < 5)
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = Tariff.ToString("00");
                    }
                    else
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = null;
                    }
                    nIndex += 9;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillSpecialDayProfileParameters(byte[] SpecialDayProfile)", ex);
            }
        }

        /// <summary>
        /// Used to update gridview  rows and status bar in case of unsuccessfull read/write
        /// </summary>
        /// <param name="selectedConfigId"></param>
        private void UpdateGridViewAndStatusBar(ProfileId selectedConfigId)
        {
            if (selectedConfigId == ProfileId.PushDisplayParameter ||
                                selectedConfigId == ProfileId.HighResolutionDisplayParameter ||
                                selectedConfigId == ProfileId.ScrollDisplyParameter)//|| selectedConfigId == ProfileId.DisplayTimeoutParameter)// Story - Hide Display Timeout Parameter
            {
                lngGridViewReadControl1.UncheckCheckBox(ProfileId.DisplayParameters);
                SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Red, CommonBLL.GetEnumDescription(CommunicationErrorType.AccessDenied));
            }
            else if (selectedConfigId == ProfileId.ActivationDate || selectedConfigId == ProfileId.ActiveDayProfile ||
                                selectedConfigId == ProfileId.ActiveSeasonProfile || selectedConfigId == ProfileId.ActiveWeekProfile ||
                                selectedConfigId == ProfileId.PassiveDayProfile || selectedConfigId == ProfileId.PassiveSeasonProfile ||
                                selectedConfigId == ProfileId.PassiveWeekProfile)
            {
                lngGridViewReadControl1.UncheckCheckBox(ProfileId.TOU);
                SetGrid(ProfileId.TOU, System.Drawing.Color.Red, CommonBLL.GetEnumDescription(CommunicationErrorType.AccessDenied));
            }
            else
            {
                lngGridViewReadControl1.UncheckCheckBox(selectedConfigId);
                SetGrid(selectedConfigId, System.Drawing.Color.Red, CommonBLL.GetEnumDescription(CommunicationErrorType.AccessDenied));
            }
            this.StatusMessage = CommonBLL.GetEnumDescription(CommunicationErrorType.AccessDenied);
            Application.DoEvents();

        }

        /// <summary>
        /// 
        /// </summary>
        private void HideTabs()
        {
            tabRS232LockUnlock.TabPages.Remove(tabMDWithIP);
            tabRS232LockUnlock.TabPages.Remove(tabkvarSelection);
            tabRS232LockUnlock.TabPages.Remove(tabDisplayParam);
            tabRS232LockUnlock.TabPages.Remove(tabDisplayParamIEC);
            tabRS232LockUnlock.TabPages.Remove(tabPageTOUOperation);
            tabRS232LockUnlock.TabPages.Remove(tabRTC);
            tabRS232LockUnlock.TabPages.Remove(tabBillingReset);
            tabRS232LockUnlock.TabPages.Remove(tabReset);
            tabRS232LockUnlock.TabPages.Remove(tabPageAutoLock);
            tabRS232LockUnlock.TabPages.Remove(tabRS232);
            tabRS232LockUnlock.TabPages.Remove(tabPageCTRatio);
            tabRS232LockUnlock.TabPages.Remove(tabPagePTRatio);
            tabRS232LockUnlock.TabPages.Remove(tabPageLSCapturePeriod);
            tabRS232LockUnlock.TabPages.Remove(tabMDIPSliding);
            tabRS232LockUnlock.TabPages.Remove(tabManualBilling);
            tabRS232LockUnlock.TabPages.Remove(tabSoftwareBilling);
            tabRS232LockUnlock.TabPages.Remove(tabMgntIconReset);
            tabRS232LockUnlock.TabPages.Remove(tabloadcontrolsmart);
            tabRS232LockUnlock.TabPages.Remove(tabdisconnectsmart);
            tabRS232LockUnlock.TabPages.Remove(tabLoadCtrl1PSmart);
            tabRS232LockUnlock.TabPages.Remove(tabRS485);
            tabRS232LockUnlock.TabPages.Remove(tabPulseEnergy);
            tabRS232LockUnlock.TabPages.Remove(tabMDReset);
            //Todo      tabRS232LockUnlock.TabPages.Remove(tabLoadCtrl1PSmart);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void HideTabs(List<System.Enum> list)
        {
            if (!list.Contains(ProfileId.DIP))
            {
                tabRS232LockUnlock.TabPages.Remove(tabMDWithIP);
            }
            if (!list.Contains(ProfileId.KvahSelection))
            {
                tabRS232LockUnlock.TabPages.Remove(tabkvarSelection);
            }
            if (!list.Contains(ProfileId.DisplayParameters))
            {
                tabRS232LockUnlock.TabPages.Remove(tabDisplayParam);
            }
            if (!list.Contains(ProfileId.TOU) && !list.Contains(ProfileId.TwoTOU) && !list.Contains(ProfileId.FourTOU) && !list.Contains(ProfileId.FourTOUWithHoliday) && !list.Contains(ProfileId.FourSPTOU))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPageTOUOperation);
            }
            if (!list.Contains(ProfileId.RTC))
            {
                tabRS232LockUnlock.TabPages.Remove(tabRTC);
            }
            if (!list.Contains(ProfileId.BillingType))
            {
                tabRS232LockUnlock.TabPages.Remove(tabBillingReset);
            }
            if (!list.Contains(ProfileId.BillingReset))
            {
                tabRS232LockUnlock.TabPages.Remove(tabReset);
            }
            if (!list.Contains(ProfileId.AutoLock))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPageAutoLock);
            }
            if (!list.Contains(ProfileId.RS232LockUnlock))
            {
                tabRS232LockUnlock.TabPages.Remove(tabRS232);
            }
            if (!list.Contains(ProfileId.CTRatio))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPageCTRatio);
            }
            if (!list.Contains(ProfileId.PTRatio))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPagePTRatio);
            }
            if (!list.Contains(ProfileId.SIP))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPageLSCapturePeriod);
            }
            if (!list.Contains(ProfileId.DIPWithSliding))
            {
                tabRS232LockUnlock.TabPages.Remove(tabMDIPSliding);
            }
            if (!list.Contains(ProfileId.LoadControl))
            {
                tabRS232LockUnlock.TabPages.Remove(tabloadcontrolsmart);
        }
            if (!list.Contains(ProfileId.DisconnectControl))
            {
                tabRS232LockUnlock.TabPages.Remove(tabdisconnectsmart);
            }
            if (!list.Contains(ProfileId.LoadControl1PSmartMeter))
            {
                tabRS232LockUnlock.TabPages.Remove(tabLoadCtrl1PSmart);
            }

            if (!list.Contains(ProfileId.RS485))
            {
                tabRS232LockUnlock.TabPages.Remove(tabRS485);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadTabs()
        {
            foreach (ProfileId param in enumData)
            {
                switch (param)
                {
                    case ProfileId.DIP:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabMDWithIP))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabMDWithIP);
                            }
                            break;
                        }
                    case ProfileId.KvahSelection:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabkvarSelection))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabkvarSelection);
                            }
                            break;
                        }
                    case ProfileId.DisplayParameters:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabDisplayParam))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabDisplayParam);
                            }
                            break;
                        }
                    case ProfileId.DisplayParametersIEC:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabDisplayParamIEC))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabDisplayParamIEC);
                            }
                            break;
                        }
                    case ProfileId.TOU:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                        
                                    tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                    rdbTOUSeason1.Checked = true;
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                    tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                                
                            }
                            break;
                        }
                    case ProfileId.TwoTOU:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdbTOUSeason2.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }

                    case ProfileId.ThreeSTOU:// ADD PRADIPTA
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdbTOUSession3.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }


                    case ProfileId.FourTOU:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdbTOUSeason4.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta                              
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }
                    case ProfileId.FourTOUWithHoliday:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdbTOUWithHoliday.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }
                    case ProfileId.FourSPTOU:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdbTOUSP.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason1);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta_tou
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                /// tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }
                    case ProfileId.FourSPTOU10Z8S:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageTOUOperation))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageTOUOperation);
                                rdb10Zone8SlotFutAct.Checked = true;
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason2);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSession3);//add pradipta
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSeason4);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUHoliday);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSP);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOU1P);
                                tabControlTOUOPeration.TabPages.Remove(tabPageTOUSpecialDay);
                            }
                            break;
                        }
                   
                    case ProfileId.LoadControl:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabloadcontrolsmart))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabloadcontrolsmart);
                                
                            }
                            break;
                        }

                    case ProfileId.DisconnectControl:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabdisconnectsmart))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabdisconnectsmart);                               
                            }
                            break;
                        }
                    
                    case ProfileId.RTC:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabRTC))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabRTC);
                            }
                            break;
                        }
                    case ProfileId.BillingType:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabBillingReset))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabBillingReset);
                            }
                            break;
                        }
                    case ProfileId.BillingReset:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabReset))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabReset);
                            }
                            break;
                        }
                    case ProfileId.AutoLock:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageAutoLock))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageAutoLock);
                            }
                            break;
                        }
                    case ProfileId.RS232LockUnlock:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabRS232))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabRS232);
                            }
                            break;
                        }
                    case ProfileId.CTRatio:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageCTRatio))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageCTRatio);
                            }
                            break;
                        }
                    case ProfileId.PTRatio:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPagePTRatio))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPagePTRatio);
                            }
                            break;
                        }
                    case ProfileId.SIP:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPageLSCapturePeriod))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPageLSCapturePeriod);
                            }
                            break;
                        }
                    case ProfileId.DIPWithSliding:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabMDIPSliding))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabMDIPSliding);
                            }
                            break;
                        }
                    case ProfileId.ManualBilling:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabManualBilling))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabManualBilling);
                            }
                            break;
                        }
                    case ProfileId.SoftwareBilling:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabSoftwareBilling))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabSoftwareBilling);
                            }
                            break;
                        }
                    case ProfileId.MagneticTamperIcon:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabMgntIconReset))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabMgntIconReset);
                            }
                            break;
                        }
                    // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter model = 17 having specific right authority to reset
                    case ProfileId.MagneticTamperIcon3P:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabMgntIconReset))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabMgntIconReset);
                            }
                            break;
                        }
                    case ProfileId.LoadControl1PSmartMeter:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabLoadCtrl1PSmart))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabLoadCtrl1PSmart);
                            }
                            break;
                        }
                    case ProfileId.RS485:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabRS485))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabRS485);
                            }
                            break;
                        }
                    case ProfileId.PulseEnergy:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabPulseEnergy))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabPulseEnergy);
                            }
                            break;
                        }

                    case ProfileId.ManualButtonMDReset:
                        {
                            if (!tabRS232LockUnlock.TabPages.Contains(tabMDReset))
                            {
                                tabRS232LockUnlock.TabPages.Add(tabMDReset);
                            }
                            break;
                        }

                    default: break;
                }
            }
        }


        /// <summary>
        /// Validate display timeout tab
        /// </summary>
        /// <param name="scrollTime"></param>
        /// <param name="pushTimeOut"></param>
        /// <param name="autoScrollTime"></param>
        /// <returns></returns>
        private string ValidateDisplayTimeout(string scrollTime, string pushTimeOut, string autoScrollTime)
        {

            string validationMessage = string.Empty;
            if (scrollTime == string.Empty)
            {

                validationMessage += "Scroll time can not be left blank." + Symbols.NEWLINE;
            }
            if (pushTimeOut == string.Empty)
            {

                validationMessage += "Push button timeout can not be left blank." + Symbols.NEWLINE;

            }
            if (chkAutoScrollTime.Checked && autoScrollTime == string.Empty)
            {

                validationMessage += "Auto scroll resume time can not be left blank." + Symbols.NEWLINE;

            }

            if (!string.IsNullOrEmpty(scrollTime) && (ValidateRegEx(scrollTime, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid scroll time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(scrollTime) && (Convert.ToInt32(scrollTime) < 1 || Convert.ToInt32(scrollTime) > 300))
            {

                validationMessage += "Scroll time can contain value between 1 and 300." + Symbols.NEWLINE;
            }


            if (!string.IsNullOrEmpty(pushTimeOut) && (ValidateRegEx(pushTimeOut, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid push button timeout." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(pushTimeOut) && (Convert.ToInt32(pushTimeOut) < 1 || Convert.ToInt32(pushTimeOut) > 600))
            {

                validationMessage += "Push button timeout can contain value between 1 and 600." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (ValidateRegEx(autoScrollTime, @"^([0-9]{0,3})$") == false))
            {

                validationMessage += "Invalid auto scroll resume time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (Convert.ToInt32(autoScrollTime) < 3 || Convert.ToInt32(autoScrollTime) > 300))
            {

                validationMessage += "Auto scroll resume time can contain value between 3 and 300." + Symbols.NEWLINE;

            }


            return validationMessage;
        }

        /// <summary>
        /// Validate a specified string against given regular expression 
        /// </summary>
        /// <param name="toValidate">String to be validated</param>
        /// <param name="regEx">Regular expression for validation</param>
        /// <returns>Returns true if validation is performed successfully otherwise false.</returns>
        private bool ValidateRegEx(string toValidate, string regEx)
        {
            if (Regex.Match(toValidate, regEx).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Tou grid binding
        /// </summary>
        private void BindTOUGrids()
        {
            try
            {
                int noOfTariffs = 8;
                if (IsOffline)
                {
                    rdbCurrentTOD.Visible = false;
                    rdbCurrentTOU2.Visible = false;
                    radioButton4.Visible = false;
                    radioButton5.Visible = false;
                }
                if (enumData != null && enumData.Contains(ProfileId.TOU))
                {
                    dayProfileGrids = new DataGridView[] { dgvDayProfile };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 1;

                    seasonProfileGrid = dgvSeasonProfile;
                    weekProfileGrid = dgvWeekProfile;
                    touActivationDate = dtpFutureActivationDate;
                    rdbTOUType = rdbFutureTOD;
                    //******* Meter Model Change Required Here ***********//
                    if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VBSPNoSeasonNoWeek 
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VFSPNoSeasonNoWeek 
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VIM_Series2 
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.BRPL_7Slot 
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.BYPL_7Slot 
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.BYPL_FD
                        || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.BRPL_CBSP //user story 1016689
                        )
                    {
                        dgvSeasonProfile.Visible = false;
                        dgvWeekProfile.Visible = false;
                        label12.Visible = false;
                        
                        noOfTariffs = 6;    //SarkarA code change 20180528 //Limit TOD to 6 for 1PH 1Season
                    }
                    else
                    {
                        dgvSeasonProfile.Visible = true;
                        dgvWeekProfile.Visible = true;
                        label12.Visible = true;
                    }

                }
                else if (enumData != null && enumData.Contains(ProfileId.TwoTOU))
                {
                    dayProfileGrids = new DataGridView[] { dgvDayProfile1S2, dgvDayProfile2S2 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 2;

                    seasonProfileGrid = dgvSeasonProfileS2;
                    weekProfileGrid = dgvWeekProfileS2;
                    touActivationDate = dtpTOUActivationDateS2;
                    rdbTOUType = rdbFutureTOU2;

                }
                else if (enumData != null && enumData.Contains(ProfileId.ThreeSTOU))
                {
                    dayProfileGrids = new DataGridView[] { dataGridView97, dataGridView98, dataGridView99 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 3;

                    seasonProfileGrid = dataGridView100;
                    weekProfileGrid = dataGridView101;
                    touActivationDate = dateTimePicker19;
                    rdbTOUType = radioButton3;

                }


                else if (enumData != null && enumData.Contains(ProfileId.FourTOU))
                {
                    dayProfileGrids = new DataGridView[] {gridTOUDay1,gridTOUDay2, gridTOUDay3, gridTOUDay4, gridTOUDay5, gridTOUDay6, gridTOUDay7, 
                                        gridTOUDay8, gridTOUDay9, gridTOUDay10, gridTOUDay11, gridTOUDay12, gridTOUDay13,
                                        gridTOUDay14, gridTOUDay15, gridTOUDay16, gridTOUDay17, gridTOUDay18, gridTOUDay19, 
                                        gridTOUDay20, gridTOUDay21, gridTOUDay22, gridTOUDay23, gridTOUDay24 };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 24;
                    seasonProfileGrid = dgvSeasonProfileS4;
                    weekProfileGrid = dgvWeekProfileS4;
                    touActivationDate = dtpTOUActivationDateS4;
                    rdbTOUType = rdbFutureTOUS4;


                }
                else if (enumData != null && enumData.Contains(ProfileId.FourSPTOU10Z8S))
                {
                    dayProfileGrids = new DataGridView[] { grdTOUDay1SP8Tariff, grdTOUDay2SP8Tariff, grdTOUDay3SP8Tariff, grdTOUDay4SP8Tariff };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfile4SP8Tariff;
                    weekProfileGrid = dgvWeekProfileSP8tariff;
                    SpecialDayProfileGrid = dgvSpclDayProf8Tariff;
                    touActivationDate = dtpActtivation8Tariff;
                    rdbTOUType = rdbFutureTOUS4SP;
                    noOfTariffs = 8;
                    //****** this code is add for smart meter special day profile
                    InitializeSpecialDayProfile(SpecialDayProfileGrid);

                }
                else if (enumData != null && enumData.Contains(ProfileId.FourSPTOU))
                {
                    dayProfileGrids = new DataGridView[] { grdTOUDay1SP8Tariff, grdTOUDay2SP8Tariff, grdTOUDay3SP8Tariff, grdTOUDay4SP8Tariff };
                    seasonProfileCount = weekProfileCount = dayProfileCount=4;
                    //dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfile4SP8Tariff;
                    weekProfileGrid = dgvWeekProfileSP8tariff;
                   // SpecialDayProfileGrid = dgvSpclDayProf8Tariff;
                    touActivationDate = dtpActtivation8Tariff;
                    rdbTOUType = rdbFutureTOUS4SP;
                    dgvSpclDayProf8Tariff.Visible = false;
                    label104.Visible = false;
                    noOfTariffs = 8;
                    rdbTOUSP.Text = "TOU With Four Season (10Zone - 8Slots)";

                }

                else
                {
                    dayProfileGrids = new DataGridView[] { gridTOUDay1SP, gridTOUDay2SP, gridTOUDay3SP, gridTOUDay4SP };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfileS4SP;
                    weekProfileGrid = dgvWeekProfileS4SP;
                    touActivationDate = dtpTOUActivationDateS4SP;
                    rdbTOUType = rdbFutureTOUS4SP;

                    if (ConfigInfo.MeterModel.Equals(NamePlateConstants.BYPL_7Slot.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.BRPL_7Slot.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.BYPL_FD.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.BRPL_CBSP.ToString())    //user story 1016689
                        )
                    {
                        noOfTariffs = 7;//For BRPL 1P 7 slot
                    }
                    else if(!ConfigInfo.MeterModel.Equals(NamePlateConstants.InvalidModelValue.ToString()))
                    {
                        noOfTariffs = 6;
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 6Slots)";
                    }
                    else
                    {
                        noOfTariffs = 7;
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 6/7Slots)";
                    }


                }

                foreach (DataGridView dgv in dayProfileGrids)
                {
                    InitializeDayProfile(dgv, noOfTariffs);
                }
                InitializeWeekProfile(weekProfileGrid);
                InitializeSeasonProfile(seasonProfileGrid);
               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //throw ex;
                logger.Log(LOGLEVELS.Error, "BindTOUGrids()", ex);
            }
        }

        /// <summary>
        /// Tou grid binding
        /// </summary>
        private void BindTOUGridsOnSeasonCheckedChange()
        {
            try
            {
                int noOfTariffs = 8;
                if (IsOffline)
                {
                    rdbCurrentTOD.Visible = false;
                    rdbCurrentTOU2.Visible = false;
                    radioButton4.Visible = false;
                    radioButton5.Visible = false;
                }
                if (rdbTOUSeason1.Checked)
                {
                    dayProfileGrids = new DataGridView[] { dgvDayProfile };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 1;

                    seasonProfileGrid = dgvSeasonProfile;
                    weekProfileGrid = dgvWeekProfile;
                    touActivationDate = dtpFutureActivationDate;
                    rdbTOUType = rdbFutureTOD;


                }
                else if (rdbTOUSeason2.Checked)
                {
                    dayProfileGrids = new DataGridView[] { dgvDayProfile1S2, dgvDayProfile2S2 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 2;

                    seasonProfileGrid = dgvSeasonProfileS2;
                    weekProfileGrid = dgvWeekProfileS2;
                    touActivationDate = dtpTOUActivationDateS2;
                    rdbTOUType = rdbFutureTOU2;


                }

                else if (rdbTOUSession3.Checked)// ADD PRADIPTA
                {
                    dayProfileGrids = new DataGridView[] { dataGridView97, dataGridView98, dataGridView99 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 3;

                    //seasonProfileGrid = dgvSeasonProfileS3;
                    //weekProfileGrid = dgvWeekProfileS3;
                    //touActivationDate = dtpFutureAct3;
                    //rdbTOUType = rdbFutureTOU2;
                    seasonProfileGrid = dataGridView100;
                    weekProfileGrid = dataGridView101;
                    touActivationDate = dateTimePicker6;
                    rdbTOUType = radioButton3;

                }

                else if (rdbTOUSeason4.Checked)
                {
                    dayProfileGrids = new DataGridView[] {gridTOUDay1,gridTOUDay2, gridTOUDay3, gridTOUDay4, gridTOUDay5, gridTOUDay6, gridTOUDay7, 
                                        gridTOUDay8, gridTOUDay9, gridTOUDay10, gridTOUDay11, gridTOUDay12, gridTOUDay13,
                                        gridTOUDay14, gridTOUDay15, gridTOUDay16, gridTOUDay17, gridTOUDay18, gridTOUDay19, 
                                        gridTOUDay20, gridTOUDay21, gridTOUDay22, gridTOUDay23, gridTOUDay24 };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 24;
                    seasonProfileGrid = dgvSeasonProfileS4;
                    weekProfileGrid = dgvWeekProfileS4;
                    touActivationDate = dtpTOUActivationDateS4;
                    rdbTOUType = rdbFutureTOUS4;



                }
                else if (rdbTOUSP.Checked)
                {
                    dayProfileGrids = new DataGridView[] { gridTOUDay1SP, gridTOUDay2SP, gridTOUDay3SP, gridTOUDay4SP };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfileS4SP;
                    weekProfileGrid = dgvWeekProfileS4SP;
                    touActivationDate = dtpTOUActivationDateS4SP;
                    rdbTOUType = rdbFutureTOUS4SP;

                    if (ConfigInfo.MeterModel.Equals(NamePlateConstants.BYPL_7Slot.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.BRPL_7Slot.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.BYPL_FD.ToString())
                         || ConfigInfo.MeterModel.Equals(NamePlateConstants.BRPL_CBSP.ToString())    //user story 1016689
                        )
                    {
                        noOfTariffs = 7;//For BRPL 1P 7 slot
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 7Slots)";
                    }
                    else if (!ConfigInfo.MeterModel.Equals(NamePlateConstants.SmartM_Cipher_LTCT.ToString()) || ConfigInfo.MeterModel.Equals(NamePlateConstants.SmartM_Cipher_WCM.ToString())
                        || ConfigInfo.MeterModel.Equals(NamePlateConstants.SmartM_Cipher_HTCT.ToString())
                         || ConfigInfo.MeterModel.Equals(NamePlateConstants.SmartM_Cipher_1PH.ToString()))
                    {
                        noOfTariffs = 8;
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 8Slots)";
                    }
                    else if(!ConfigInfo.MeterModel.Equals(NamePlateConstants.InvalidModelValue.ToString()))
                    {
                        noOfTariffs = 6;
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 6Slots)";
                    }
                    else
                    {
                        noOfTariffs = 7;
                        rdbTOUSP.Text = "TOU With Four Season (10Zone - 6/7Slots)";
                    }
                }                
                // add for smart meter special day profile
                else if (rdb10Zone8SlotFutAct.Checked)
                {
                    dayProfileGrids = new DataGridView[] { grdTOUDay1SP8Tariff, grdTOUDay2SP8Tariff, grdTOUDay3SP8Tariff, grdTOUDay4SP8Tariff };
                    seasonProfileCount = weekProfileCount = 4;
                    dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfile4SP8Tariff;
                    weekProfileGrid = dgvWeekProfileSP8tariff;
                    SpecialDayProfileGrid = dgvSpclDayProf8Tariff;
                    touActivationDate = dtpActtivation8Tariff;
                    rdbTOUType = rdbFutureTOUS4SP;

                    noOfTariffs = 8;
                    InitializeSpecialDayProfile(SpecialDayProfileGrid);

                }
                foreach (DataGridView dgv in dayProfileGrids)
                {
                    InitializeDayProfile(dgv, noOfTariffs);
                }
                InitializeWeekProfile(weekProfileGrid);
                InitializeSeasonProfile(seasonProfileGrid);
              
            }
            catch (Exception ex)    //Exception log for catch block
            {
              //throw ex;
                logger.Log(LOGLEVELS.Error, "BindTOUGridsOnSeasonCheckedChange()", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void BindDisplayParameters()
        {
            FillDisplayParameters(dGVPushDisplayParams, "PUSH");
            dGVPushDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVPushDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVPushDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedPushParams, dGVPushDisplayParams);
            dGVPushDisplayParams.Columns["SNO"].Width = 80;
            dGVPushDisplayParams.Columns["ID"].Width = 80;
            dGVPushDisplayParams.Columns["Description"].Width = 200;
            dGVPushDisplayParams.Columns["colInclude"].Width = 85;
            dGVPushDisplayParams.Refresh();

            FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
            dGVScrollDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVScrollDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVScrollDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedScrollParams, dGVScrollDisplayParams);

            dGVScrollDisplayParams.Columns["SNO"].Width = 80;
            dGVScrollDisplayParams.Columns["ID"].Width = 80;
            dGVScrollDisplayParams.Columns["Description"].Width = 200;
            dGVScrollDisplayParams.Columns["colInclude"].Width = 85;
            dGVScrollDisplayParams.Refresh();

            FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");
            dGVHighResolution.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVHighResolution.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVHighResolution.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedHighResParams, dGVHighResolution);

            dGVHighResolution.Columns["SNO"].Width = 80;
            dGVHighResolution.Columns["ID"].Width = 80;
            dGVHighResolution.Columns["Description"].Width = 200;
            dGVHighResolution.Columns["colInclude"].Width = 85;
            dGVHighResolution.Refresh();

            if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
            {
                dGVPushDisplayParams.Columns["colInclude"].ReadOnly = true;
                dGVScrollDisplayParams.Columns["colInclude"].ReadOnly = true;
                dGVHighResolution.Columns["colInclude"].ReadOnly = true;
            }
        }

        /// <summary>
        /// Bind Input Grid with day profile attributes.
        /// </summary>
        /// <param name="dgvDayProfile"></param>
        /// <param name="noOfTariffs"></param>
        private void InitializeDayProfile(DataGridView dgvDayProfile, int noOfTariffs)
        {
            dgvDayProfile.ColumnCount = 0;
            dgvDayProfile.RowHeadersVisible = false;
            dgvDayProfile.Columns.Add(GetDataGridView(10, COLZONE, ZONE, 35));
            dgvDayProfile.Columns.Add(GetDataGridView(noOfTariffs, COLTARIFF, TARIFF, 39));
            dgvDayProfile.Columns.Add(GetDataGridView(23, COLSTARTHOUR, STARTHOUR, 39));
            dgvDayProfile.Columns.Add(GetDataGridView(3, COLSTARTMIN, STARTMIN, 39));
            dgvDayProfile.RowCount = 10;
            for (int index = 0; index < dgvDayProfile.RowCount; index++)
            {
                dgvDayProfile.Rows[index].Cells[0].Value = (index + 1).ToString();
            }
            if (dayProfileCount == 2)
            {
                dgvDayProfile.Columns[0].Width = 36;
                dgvDayProfile.Columns[1].Width = 62;
                dgvDayProfile.Columns[2].Width = 62;
                dgvDayProfile.Columns[3].Width = 62;
            }
            if (dayProfileCount == 1)
            {
                dgvDayProfile.Columns[0].Width = 58;
                dgvDayProfile.Columns[1].Width = 77;
                dgvDayProfile.Columns[2].Width = 77;
                dgvDayProfile.Columns[3].Width = 77;
            }
        }




        /// <summary>
        /// Used to bind season profile grid on view load method
        /// </summary>        
        private void InitializeSeasonProfile(DataGridView dgvSeasonProfile)
        {
            int width = 56;
            if (dayProfileCount == 24 || dayProfileCount == 4)
            {
                width = 51;
            }
            try
            {
                dgvSeasonProfile.Columns.Clear();
                dgvSeasonProfile.Columns.Add(GetDataGridView(31, COLDAY, DAY, width));
                dgvSeasonProfile.Columns.Add(GetDataGridView(12, COLMONTH, Month, width));
                dgvSeasonProfile.Columns.Add(GetDataGridView(seasonProfileCount, COLSESSION, WEEKPROFILE, width));
                dgvSeasonProfile.RowCount = seasonProfileCount;
                dgvSeasonProfile.RowHeadersVisible = false;
                dgvSeasonProfile.Rows[0].Cells[COLDAY].ReadOnly = true;
                dgvSeasonProfile.Rows[0].Cells[COLMONTH].ReadOnly = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeSeasonProfile(DataGridView dgvSeasonProfile)", ex);
                throw;
            }
        }
        /// <summary>
        /// Initialize day profile Grid 
        /// </summary>
        /// <param name="dgvWeekProfile"></param>
        private void InitializeWeekProfile(DataGridView dgvWeekProfile)
        {
            int width = 37;
            int widthWeek = 42;
            if (dayProfileCount == 24 || dayProfileCount == 4)
            {
                width = 31;
                widthWeek = 38;
            }
            try
            {
                dgvWeekProfile.RowHeadersVisible = false;
                dgvWeekProfile.Columns.Clear();
                dgvWeekProfile.Columns.Add(GetDataGridView(weekProfileCount, COLZONE, WEEK, widthWeek));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLMONDAY, MONDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLTUESDAY, TUESDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLWEDNESDAY, WEDNESDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLTHURSDAY, THURSDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLFRIDAY, FRIDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLSATURDAY, SATURDAY, width));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLSUNDAY, SUNDAY, width));

                dgvWeekProfile.RowCount = weekProfileCount;
                for (int index = 0; index < dgvWeekProfile.RowCount; index++)
                {
                    dgvWeekProfile.Rows[index].Cells[0].Value = (index + 1).ToString();
                }
                dgvWeekProfile.Columns[COLZONE].ReadOnly = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeWeekProfile(DataGridView dgvWeekProfile)", ex);
                throw;
            }

        }


        /// <summary>
        /// Used to create columns for various profile data grids
        /// This method is called while adding columns to various data grids
        /// </summary>
        /// <param name="numberOfItems"></param>
        /// <param name="columnName"></param>
        /// <param name="headerText"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        /// 
        // ************** This code is added for smart meter Special day profile ****************
        private void InitializeSpecialDayProfile(DataGridView dgvSplDayProfile)
        {
            int width = 42;
            SpecialDayProfileCount = 20;
            try
            {
                dgvSplDayProfile.RowHeadersVisible = false;
                dgvSplDayProfile.Columns.Clear();
                dgvSplDayProfile.Columns.Add(GetDataGridView(SpecialDayProfileCount, "Days", "Days", width));
                dgvSplDayProfile.Columns.Add(GetDataGridView(12, "Month", "Month", width));
                dgvSplDayProfile.Columns.Add(GetDataGridView(31, "Date", "Date", width));
                dgvSplDayProfile.Columns.Add(GetDataGridView(4, "DayID", "DayID", width));
                dgvSplDayProfile.RowCount = SpecialDayProfileCount;
                for (int index = 0; index < dgvSplDayProfile.RowCount; index++)
                {
                    dgvSplDayProfile.Rows[index].Cells[0].Value = (index + 1).ToString();
                }
                dgvSplDayProfile.Columns["Days"].ReadOnly = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeSpecialDayProfile(DataGridView dgvSplDayProfile)", ex);
            }

        }
        DataGridViewComboBoxColumn GetDataGridView(int numberOfItems, string columnName, string headerText, int width)
        {
            int index = 1;
            DataGridViewComboBoxColumn gridViewComboBox = new DataGridViewComboBoxColumn();
            gridViewComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            gridViewComboBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewComboBox.Width = width;
            gridViewComboBox.Name = columnName;
            gridViewComboBox.HeaderText = headerText;
            if (headerText == STARTHOUR || headerText == STARTMIN)
            {
                index = 0;
            }
            for (; index <= numberOfItems; index++)
            {
                if (headerText == TARIFF)
                {
                    gridViewComboBox.Items.Add("T" + index.ToString());
                }
                else if (headerText == STARTMIN)
                {
                    gridViewComboBox.Items.Add((index * 15).ToString("00"));
                }
                else if (headerText == WEEK || headerText == ZONE)
                {
                    gridViewComboBox.Items.Add(index.ToString());
                }
                //****This code is added for smart meter spl day profile
                else if (columnName == "Days" && headerText == "Days")
                {
                    gridViewComboBox.Items.Add(index.ToString());
                }

                else
                {
                    gridViewComboBox.Items.Add(index.ToString("00"));
                }
            }
            return gridViewComboBox;

        }

        /// <summary>
        /// All check box of dgrid should be checked
        /// </summary>
        /// <param name="dgvTemp"></param>
        private void CheckAllTheElementsInGrid(DataGridView dgvTemp)
        {
            if (chkDisplayParamSelectAll.Checked == true)
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = true;
                }

            }
            else
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = false;
                }
            }

        }
        /// <summary>
        /// Check uncheck according to display paremeter grid selected.
        /// </summary>
        /// <param name="dgvTemp"></param>
        private void CheckUncheckAll(DataGridView dgvTemp)
        {
            List<int> selectedParemetrs = new List<int>();
            if (chkDisplayParamSelectAll.Checked == true)
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = true;
                    selectedParemetrs.Add(Convert.ToInt32(row.Cells["ID"].Value));
                }

            }
            else
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = false;
                }
            }
            if (dgvTemp.Name == "dGVPushDisplayParams")
            {
                selectedPushParams = selectedParemetrs;
            }
            else if (dgvTemp.Name == "dGVScrollDisplayParams")
            {
                selectedScrollParams = selectedParemetrs;
            }
            else if (dgvTemp.Name == "dGVHighResolution")
            {
                selectedHighResParams = selectedParemetrs;
            }
            dgvTemp.EndEdit();
        }
        /// <summary>
        /// Gets id's of selected rows in parameter grid.
        /// </summary>
        /// <param name="dgvDisplayParams"></param>
        /// <returns></returns>
        private List<byte> GetSelectedRowsinParameterGrid(DataGridView dgvDisplayParams)
        {
            List<byte> displayParamsBytes = new List<byte>();
            List<int> displayParams = new List<int>();
            try
            {
                if (dgvDisplayParams != null)
                {
                    foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                    {
                        if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                        {
                            displayParams.Add(Convert.ToInt32(row.Cells["ID"].Value));
                        }
                    }
                }
            
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "GetSelectedRowsinParameterGrid(DataGridView dgvDisplayParams)", ex);
            }

            if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte || chkDisplayExtended.Checked)
            {
                foreach (int val in displayParams)
                {
                    var byteLo = (byte)(val & 0xFF);
                    var byteHi = (byte)((val >> 8) & 0xFF);
                    displayParamsBytes.Add(byteHi);
                    displayParamsBytes.Add(byteLo);
                }
            }
            else
            {
                foreach (int val in displayParams)
                    displayParamsBytes.Add((byte)val);
            }
            return displayParamsBytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dGVHighResolution"></param>
        private void CheckAndUpdateSelectAll(DataGridView dGVHighResolution)
        {
            bool isSelected = false;
            foreach (DataGridViewRow row in dGVHighResolution.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["colInclude"].Value))
                {
                    isSelected = false;
                    break;
                }
                else if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                {
                    isSelected = true;
                }
            }
            chkDisplayParamSelectAll.CheckedChanged -= new EventHandler(chkDisplayParamSelectAll_CheckedChanged);
            chkDisplayParamSelectAll.Checked = isSelected;
            chkDisplayParamSelectAll.CheckedChanged += new EventHandler(chkDisplayParamSelectAll_CheckedChanged);
        }
        /// <summary>
        /// Fill selected disply parameters
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="dgvDisplayParams"></param>
        private void FillDisplayParameters(List<int> receivedData, DataGridView dgvDisplayParams)
        {
            int parameterCount = receivedData.Count;
            int displayParameters = 0;
            foreach (DataGridViewRow row in dgvDisplayParams.Rows)
            {
                row.Cells["colInclude"].Value = false;
            }
            Application.DoEvents();

            for (int paramCounter = 0; paramCounter < parameterCount; paramCounter++)
            {
                int rowCounter = 0;
                displayParameters = receivedData[paramCounter];
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    rowCounter++;
                    if (Convert.ToInt32(row.Cells["ID"].Value) == displayParameters)
                    {
                        //row.Cells["colInclude"].Value = true;
                        for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                        {
                            MoveDisplayRow(tempRowCounter, dgvDisplayParams);
                        }

                    }
                }
            }

            dgvDisplayParams.ClearSelection();
            dgvDisplayParams.Rows[0].Cells[2].Selected = true;


            for (int displayParamCounter = 0; displayParamCounter < parameterCount; displayParamCounter++)
            {
                dgvDisplayParams.Rows[displayParamCounter].Cells["colInclude"].Value = true;
            }
            Application.DoEvents();
        }
        /// <summary>
        /// Show display paremeter after display readout.
        /// </summary>
        /// <param name="receviedData"></param>
        /// <param name="parameterType"></param>
        /// <param name="profileCommand"></param>
        private void ShowDispayParameters(byte[] receviedData, DisplayParameterType parameterType, ProfileCommand profileCommand)
        {
            try
            {

                List<int> selectedParameters = new List<int>();
                DataGridView targetGridView = null;
                tabRS232LockUnlock.SelectedTab = tabDisplayParam;
                if (parameterType == DisplayParameterType.PushMode)
                {
                    targetGridView = dGVPushDisplayParams;
                }
                else if (parameterType == DisplayParameterType.ScrollMode)
                {
                    targetGridView = dGVScrollDisplayParams;
                    tabControlDisplayParams.SelectedIndex = 1;
                }
                else if (parameterType == DisplayParameterType.HighResolutionMode)
                {
                    targetGridView = dGVHighResolution;
                    tabControlDisplayParams.SelectedIndex = 2;
                }
                targetGridView.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                targetGridView.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                targetGridView.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;

                // BugID: 501277 - Uploading Issue for 3P DLMS meter Display_Parameters code corrected(Previously due to Roolback of code wrong class (LoadControl) is used for parsing.
                //ProfileData profileData = new CAB.E650MeterConfiguration.DisplayParameter(true).ParseData(receviedData, GetDLMSCommandFromProfileCommand(profileCommand));
                ProfileData profileData = new CAB.E650MeterConfiguration.DisplayParameter(true).ParseData(receviedData, GetDLMSCommandFromProfileCommand(profileCommand));
                List<ProfileData> profileList = new List<ProfileData>();
                profileList.Add(profileData);
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    Collection<CAB.Entity.DisplayParamatersDBEntity> collDisplayParamatersDBEntity = null;
                    if (ConfigInfo.SignatureInfo.Contains("ST"))
                    {
                        collDisplayParamatersDBEntity = new DisplayParameterAndTimeout("ST").GetData(profileList, parameterType); //null
                    }
                    else if (ConfigInfo.SignatureInfo.Contains("st"))
                    {
                        collDisplayParamatersDBEntity = new DisplayParameterAndTimeout("st").GetData(profileList, parameterType); //null
                    }
                    else
                    {
                        collDisplayParamatersDBEntity = new DisplayParameterAndTimeout("").GetData(profileList, parameterType); //null
                    }


                    foreach (DataGridViewRow row in targetGridView.Rows)
                    {
                        row.Cells["colInclude"].Value = false;
                    }
                    Application.DoEvents();
                    int rowCounter = 0;
                    int selectedParameterId = -1;
                    for (int paramCounter = 0; paramCounter < collDisplayParamatersDBEntity.Count; paramCounter++)
                    {
                        rowCounter = 0;

                        selectedParameterId = GetParameterIdFromName(collDisplayParamatersDBEntity[paramCounter].paramaterName, parameterType);
                        if (selectedParameterId > -1)
                        {
                            selectedParameters.Add(Convert.ToInt32(selectedParameterId));
                            foreach (DataGridViewRow row in targetGridView.Rows)
                            {
                                rowCounter++;
                                if (Convert.ToInt32(row.Cells["ID"].Value) == selectedParameterId)
                                {
                                    for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                                    {
                                        MoveDisplayRow(tempRowCounter, targetGridView);
                                    }

                                }
                            }
                        }
                    }
                    targetGridView.ClearSelection();
                    targetGridView.Rows[0].Cells[2].Selected = true;


                    for (int displayParamCounter = 0; displayParamCounter < collDisplayParamatersDBEntity.Count; displayParamCounter++)
                    {
                        targetGridView.Rows[displayParamCounter].Cells["colInclude"].Value = true;

                    }

                    if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte && !IsOffline)
                    {
                        for (int displayParamCounter = targetGridView.Rows.Count - 1; displayParamCounter >= collDisplayParamatersDBEntity.Count; displayParamCounter--)
                        {
                            targetGridView.Rows.RemoveAt(displayParamCounter);
                        }
                        targetGridView.Columns["colInclude"].ReadOnly = true;
                    }

                    if (!listSelectedParams.Contains(ProfileId.DisplayParameters)) // Story - 427028 - check box was not checked while cfg file uploaded
                        listSelectedParams.Add(ProfileId.DisplayParameters);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                // MessageBox.Show("Error in showing dsiplay parameter !");
                if (flagErrorDisplayParam)
                {
                    MessageBox.Show("Please select the correct Display Parameter (3P WC / 3P LTCT) Option!"); //Bug id: 469083 Exception is coming while uploading CFG File in CMRI Scheduling.
                    flagErrorDisplayParam = false;
                }
                logger.Log(LOGLEVELS.Error, "ShowDispayParameters(byte[] receviedData, DisplayParameterType parameterType, ProfileCommand profileCommand)", ex);
            }
            tabRS232LockUnlock.SelectedIndex = 0;
            tabControlDisplayParams.SelectedIndex = 0;
            this.StatusMessage = "Display Parameters " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Used to get parameter id from parameter name 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        private int GetParameterIdFromName(string parameterName, DisplayParameterType parameterType)
        {
            DataTable masterTable = new DataTable();
            int parameterId = -1;
            if (parameterType == DisplayParameterType.PushMode)
            {
                masterTable = displayParameterRepository.Tables["PushDisplayParams"];
            }
            else if (parameterType == DisplayParameterType.ScrollMode)
            {
                masterTable = displayParameterRepository.Tables["ScrollDisplayParams"];
            }
            else if (parameterType == DisplayParameterType.HighResolutionMode)
            {
                masterTable = displayParameterRepository.Tables["HighResolution"];
            }

            foreach (DataRow row in masterTable.Rows)
            {
                if (row["Description"].ToString() == parameterName)
                {
                    parameterId = Convert.ToInt32(row["ID"]);
                    break;
                }
            }
            return parameterId;
        }

        /// <summary>
        /// Move selected display row upward
        /// </summary>
        /// <param name="nRowIndex"></param>
        /// <param name="dgvDisplayParams"></param>
        private void MoveDisplayRow(int rowIndex, DataGridView dgvDisplayParams)
        {

            try
            {           
            int selectedRow = rowIndex;// 
            if (selectedRow > 0)
            {

                String tempDispID, tempDispInfo;
                tempDispID = dgvDisplayParams.Rows[selectedRow - 1].Cells["ID"].Value.ToString();
                tempDispInfo = dgvDisplayParams.Rows[selectedRow - 1].Cells["Description"].Value.ToString();

                dgvDisplayParams.Rows[selectedRow - 1].Cells["ID"].Value = dgvDisplayParams.Rows[selectedRow - 2].Cells["ID"].Value;
                dgvDisplayParams.Rows[selectedRow - 1].Cells["Description"].Value = dgvDisplayParams.Rows[selectedRow - 2].Cells["Description"].Value;

                dgvDisplayParams.Rows[selectedRow - 2].Cells["ID"].Value = tempDispID;
                dgvDisplayParams.Rows[selectedRow - 2].Cells["Description"].Value = tempDispInfo;
                dgvDisplayParams.ClearSelection();

            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "MoveDisplayRow(int rowIndex, DataGridView dgvDisplayParams)", ex);
                throw;
            }

        }

        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);                
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
               // profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                profileCommandEntity.ClassName = dlmsCommand.CLASSNAME;
                lstProfileCommands.Add(profileCommandEntity);
                
            }
            return lstProfileCommands;
        }

        /// <summary>
        /// Gets Selected profile Ids to be programmed in meter
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private List<System.Enum> GetSelectedProfileId(string action)
        {
            List<System.Enum> selectedElements = new List<System.Enum>();
            selectedElements.Clear();
            selectedElements.AddRange(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData));
            //if (chkRTC.Checked)
            //{
            //    selectedElements.Add(ProfileId.RTC);

            //}
            //if (chkBillingReset.Checked)
            //{
            //    selectedElements.Add(ProfileId.BillingReset);
            //}
            if (selectedElements.Contains(ProfileId.TOU) || selectedElements.Contains(ProfileId.TwoTOU) || selectedElements.Contains(ProfileId.ThreeSTOU)
                || selectedElements.Contains(ProfileId.FourTOU) || selectedElements.Contains(ProfileId.FourSPTOU) ||
                selectedElements.Contains(ProfileId.FourSPTOU10Z8S))// chkTOD.Checked)
            {
                if (action == "read")
                {
                    selectedElements.Add(ProfileId.PassiveSeasonProfile);
                    selectedElements.Add(ProfileId.PassiveWeekProfile);
                    selectedElements.Add(ProfileId.PassiveDayProfile);
                    selectedElements.Add(ProfileId.SpecialDayProfileSmartMeter);
                    selectedElements.Add(ProfileId.ActiveSeasonProfile);
                    selectedElements.Add(ProfileId.ActiveWeekProfile);
                    selectedElements.Add(ProfileId.ActiveDayProfile);
                    selectedElements.Add(ProfileId.ActivationDate);
                }
                else
                {
                    if (rdb10Zone8SlotFutAct.Checked)
                    {
                           selectedElements.Add(ProfileId.SpecialDayProfileSmartMeter);
                    }


                    if (rdbFutureTOD.Checked)
                    {
                        
                        selectedElements.Add(ProfileId.PassiveSeasonProfile);
                        selectedElements.Add(ProfileId.PassiveWeekProfile);
                        selectedElements.Add(ProfileId.PassiveDayProfile);
                        selectedElements.Add(ProfileId.ActivationDate);                     
                        //selectedElements.Add(ProfileId.SpecialDayProfileSmartMeter);
                        //To make sure that for DLMS meters tou send two times
                         if (IsOffline && !rdbTOUWithHoliday.Checked && !rdbTOUSeason4.Checked)
                        {
                            selectedElements.Add(ProfileId.PassiveSeasonProfile);
                            selectedElements.Add(ProfileId.PassiveWeekProfile);
                            selectedElements.Add(ProfileId.PassiveDayProfile);
                            selectedElements.Add(ProfileId.ActivationDate);
                            //*********** Third time TOU Profile ID as per IS CLock related changes specific to S2
                            selectedElements.Add(ProfileId.PassiveSeasonProfile);
                            selectedElements.Add(ProfileId.PassiveWeekProfile);
                            selectedElements.Add(ProfileId.PassiveDayProfile);
                            selectedElements.Add(ProfileId.ActivationDate);
                        }

                        
                    }
                    else
                    {
                        selectedElements.Add(ProfileId.ActiveSeasonProfile);
                        selectedElements.Add(ProfileId.ActiveWeekProfile);
                        selectedElements.Add(ProfileId.ActiveDayProfile);
                    }
                }

            }
            if (selectedElements.Contains(ProfileId.DisplayParameters))// chkDisplayParam.Checked)
            {
                selectedElements.Add(ProfileId.PushDisplayParameter);
                selectedElements.Add(ProfileId.ScrollDisplyParameter);
                selectedElements.Add(ProfileId.HighResolutionDisplayParameter);
                // selectedElements.Add(ProfileId.DisplayTimeoutParameter); // Story - Hide Display Timeout Parameter
            }
            //[BillingType_Month]
            if (selectedElements.Contains(ProfileId.BillingType))
            {
                selectedElements.Add(ProfileId.BillingMonthType);                
            }
            //if (chkKVARSelcetion.Checked)
            //{
            //    selectedElements.Add(ProfileId.KvahSelection);
            //}
            //if (chkLockRS232.Checked)
            //{
            //    selectedElements.Add(ProfileId.RS232LockUnlock);
            //}
            //if (chkMDWithIP.Checked)
            //{
            //    selectedElements.Add(ProfileId.DIP);
            //    selectedElements.Add(ProfileId.SIP);
            //}
            //if (chkAutoLock.Checked)
            //{
            //    selectedElements.Add(ProfileId.AutoLock);
            //}
            return selectedElements;
        }

       


        /// <summary>
        /// Used to fill Billing month type entity from values of Billing type controls [BillingType_Month]
        /// </summary>
        /// <returns></returns>
        private byte FillBillingMonthTypeData()
        {
            byte bilingMonthType = 0xFF;
            try
            {
                if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.RubyE150Value || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SFSP)
                {
                    if (otherBillingType.Checked == true)
                    {
                        if (monthlyBilling.Checked)
                        {
                            bilingMonthType = 02;
                        }
                        else if (oddMonthBilling.Checked)
                        {
                            bilingMonthType = 01;
                        }
                        else if (evenMonthBilling.Checked)
                        {
                            bilingMonthType = 00;
                        }
                    }
                }
                else
                {
                    if (otherBillingType.Checked == true)
                    {
                        if (monthlyBilling.Checked)
                        {
                            bilingMonthType = 00;
                        }
                        else if (oddMonthBilling.Checked)
                        {
                            bilingMonthType = 01;
                        }
                        else if (evenMonthBilling.Checked)
                        {
                            bilingMonthType = 02;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillBillingMonthTypeData()", ex);
            }            
            return bilingMonthType;
        }

        /// <summary>
        /// Used to fill Meter Pulse Energy Type 
        /// </summary>
        /// <returns></returns>
        private byte FillPulseEnergyTypeData()
        {
            byte pulseByte = 0xFF;
            try
            {
                if (rdbPulseActive.Checked) pulseByte = (int)PulseEnergyValues.Active;
                else if (rdbPulseApparent.Checked) pulseByte = (int)PulseEnergyValues.Apparent;
                else if (rdbPulseReactive.Checked) pulseByte = (int)PulseEnergyValues.Reactive;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //throw ex;
                logger.Log(LOGLEVELS.Error, "FillPulseEnergyTypeData()", ex);
            }
            return pulseByte;
        }

        /// <summary>
        /// Used to fill Billing type entity from values of Billing type controls
        /// </summary>
        /// <returns></returns>
        private CAB.E650MeterConfiguration.Entity.BillingDateTime FillBillingTypeData()
        {
            CAB.E650MeterConfiguration.Entity.BillingDateTime billingTypeData = new CAB.E650MeterConfiguration.Entity.BillingDateTime();
            try
            {
                if (cmbBoxBillingPeriod.SelectedIndex == 1)
                {
                    // [BillingType_Month]
                    //if (normalBillingType.Checked == true)
                    //{
                    billingTypeData.Hour = Convert.ToByte(cmbBoxBillingHour.Text);
                    billingTypeData.Date = Convert.ToByte(cmbBoxBillingDate.Text);
                    billingTypeData.Minute = Convert.ToByte(cmbBoxBillingMinute.Text);
                    //}
                    //else if (otherBillingType.Checked == true)
                    //{
                    //    billingTypeData.Date = Convert.ToByte(cmbBoxBillingDate.Text);
                    //    billingTypeData.Hour = 0x00;
                    //    billingTypeData.Minute = 0x00;
                    //}
                }
                else
                {
                    billingTypeData.Hour = 0xFF;
                    billingTypeData.Date = 0xFE;
                    billingTypeData.Minute = 0xFF;
                }
                if (normalBillingType.Checked)
                {
                    billingTypeData.BillingType = 0xff;
                }
                else
                {
                    if (rdbSinglephase.Checked)
                    {
                        if (evenMonthBilling.Checked)
                        {
                            billingTypeData.BillingType = 0x00;
                        }
                        else if (oddMonthBilling.Checked)
                        {
                            billingTypeData.BillingType = 0x01;
                        }
                        else
                        {
                            billingTypeData.BillingType = 0x02;
                        }
                    }
                    else
                    {
                        if (evenMonthBilling.Checked)
                        {
                            billingTypeData.BillingType = 0x02;
                        }
                        else if (oddMonthBilling.Checked)
                        {
                            billingTypeData.BillingType = 0x01;
                        }
                        else
                        {
                            billingTypeData.BillingType = 0x00;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillBillingTypeData()", ex);
            }        
            return billingTypeData;
        }

        /// <summary>
        /// Used to fill DIP with Sliding Demand
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int FillMDWithIPData(bool isSlidingDemand)
        {
            int demandIPData = 0;
            try
            {
                if (isSlidingDemand)
                {
                    if (cmbDemandInterval.Text == "15")
                    {
                        demandIPData = 4996;
                    }
                    else
                    {
                        demandIPData = 9992;
                    }
                }
                else
                {
                    demandIPData = Convert.ToInt32(cmbDemandInterval.SelectedItem) * 60;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillMDWithIPData(bool isSlidingDemand)", ex);
            }            
            return demandIPData;
        }

        /// <summary>
        /// Used to fill Demand integration period
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        /// 
        private List<byte> WriteRS485()
        {
            List<byte> RS485DeviceAddressbyte = new List<byte>();
            try
            {
                int objint;
                int RS485DeviceAddress = 0;
                if (int.TryParse(txtRS485DeviceAddress.Text, out objint)) RS485DeviceAddress = Convert.ToInt32(txtRS485DeviceAddress.Text);

                RS485DeviceAddressbyte.Add(Convert.ToByte((RS485DeviceAddress & 0xFF00) >> 8));
                RS485DeviceAddressbyte.Add(Convert.ToByte(RS485DeviceAddress & 0x00FF));
            }
            catch (Exception ex)    //Exception log for catch block
            {                   
               //throw ex;
                logger.Log(LOGLEVELS.Error, "WriteRS485()", ex);
            }            
            return RS485DeviceAddressbyte;

        }
        private List<byte> WriteDisconnectControl()
        {
            List<byte> DCbyte = new List<byte>();
            try
            {
                DCbyte.Add(0x0F);
                DCbyte.Add(0x00);
            
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "WriteDisconnectControl()", ex);
            }
            return DCbyte;
        
        }

        public enum LOADCONTROLBIT
        {
            // BIT 7: Over Current BIT 12: Low PF     BIT 13: OverLoad
            Over_Current = 2,
            OverLoad = 3,
        }
        private List<byte> WriteLoadControl()
        {
            List<byte> LCbyte = new List<byte>();
            try
            {     
                BitArray myarra = new BitArray(32);
                int priority = 0;
                myarra[priority + (int)LOADCONTROLBIT.Over_Current] = (cmbOverload.SelectedIndex != 0);
                myarra[priority + (int)LOADCONTROLBIT.OverLoad] = (cmbOverloadCurrent.SelectedIndex != 0);
                byte[] ret = new byte[myarra.Length / 8];
                myarra.CopyTo(ret, 0);
                List<byte> convertedByteList = ReverseBitsofByteList(ret);
                //return (ret.ToList());
                LCbyte.Add(0x04);//--Bit String
                LCbyte.Add(0x20);//--32 Bit String len
                LCbyte.Add(convertedByteList[0]);
                LCbyte.Add(convertedByteList[1]);
                LCbyte.Add(convertedByteList[2]);
                LCbyte.Add(convertedByteList[3]);

                //--------------Struct 2nd onward-----------------------
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txtTimeInterval.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txtMaxretry.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txtWaitTime.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txtMaxRetryCycle.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txtRelayReconnectiontime.Text));
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "WriteLoadControl()", ex);
            }            
            return LCbyte;
        }

        private List<byte> WriteLoadControl1PSmartMeter()
        {
            List<byte> LCbyte = new List<byte>();
            try
            {
                BitArray myarra = new BitArray(32);
                int priority = 0;
                myarra[priority + (int)LOADCONTROLBIT.Over_Current] = (cmb1phOverloadCurrent.SelectedIndex != 0);
                myarra[priority + (int)LOADCONTROLBIT.OverLoad] = (cmb1phOverload.SelectedIndex != 0);
                byte[] ret = new byte[myarra.Length / 8];
                myarra.CopyTo(ret, 0);
                List<byte> convertedByteList = ReverseBitsofByteList(ret);
                //return (ret.ToList());
                LCbyte.Add(0x04);//--Bit String
                LCbyte.Add(0x20);//--32 Bit String len
                LCbyte.Add(convertedByteList[0]);
                LCbyte.Add(convertedByteList[1]);
                LCbyte.Add(convertedByteList[2]);
                LCbyte.Add(convertedByteList[3]);

                //--------------Struct 2nd onward-----------------------
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txt1phTimeInterval.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txt1phMaxretry.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txt1phWaitTime.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txt1phMaxRetryCycle.Text));
                LCbyte.Add(0x11);
                LCbyte.Add(Convert.ToByte(txt1phRelayReconnectiontime.Text));
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "WriteLoadControl1PSmartMeter()", ex);
            }
            
            return LCbyte;

        }


        private int FillDIPData()
        {
            int dipValue = 0;
            try
            {                
                //if (cmbDIPDemandType.SelectedIndex == 0)
                //{
                if (cmbDIPDemandInterval.SelectedIndex == 1)                 //block 30 min
                {
                    dipValue = 0x0708;

                }
                else if (cmbDIPDemandInterval.SelectedIndex == 0)            //block 15 min
                {
                    dipValue = 0x0384;
                }
                //}
                //else
                //{

                //    if (cmbDIPDemandInterval.SelectedIndex == 0)                                                 //sliding 15/5 min
                //    {
                //        dipValue = 0x1384;
                //    }
                //    else
                //    {
                //        if (cmbDIPDemandSubIntervalTime.SelectedIndex == 0)                                           // sliding 30/5 min
                //        {
                //            dipValue = 0x1708;
                //        }
                //        else
                //        {
                //            dipValue = 0x2708;                                                            // sliding 30/10 min
                //        }
                //    }
                //}
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillDIPData()", ex);
            }           
            return dipValue;
        }

        private int FillSlideSubDIP()
        {
            int SlideValue = 0x0;
            try
            {
                if (cmbDIPDemandType.SelectedIndex == 1)
                {

                    if (cmbDIPDemandInterval.SelectedIndex == 0)                                                 //sliding 15/5 min
                    {
                        SlideValue = 0x1384;
                    }
                    else
                    {
                        if (cmbDIPDemandSubIntervalTime.SelectedIndex == 0)                                           // sliding 30/5 min
                        {
                            SlideValue = 0x1708;
                        }
                        else
                        {
                            SlideValue = 0x2708;                                                            // sliding 30/10 min
                        }
                    }
                }
                else
                {
                    if (cmbDIPDemandInterval.SelectedIndex == 0)                                                 //sliding 15/0 or 30/0 min
                    {
                        SlideValue = 0x0384;
                    }
                    else
                    {
                        SlideValue = 0x0708;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillSlideSubDIP()", ex);
            }
            return SlideValue;
        }


        /// <summary>
        ///  Used to fill LSIP
        /// </summary>
        /// <returns></returns>
        private int FillSIPData()
        {
            int lodaSurveyCapturePeriod = 0;
            try
            {
                if (cmbBoxLSCapturePeriod.SelectedItem.ToString().Contains("15"))
                {
                    lodaSurveyCapturePeriod = 900;//To set (Sec) value for Min(Sec)
                }
                else if (cmbBoxLSCapturePeriod.SelectedItem.ToString().Contains("30"))
                {
                    lodaSurveyCapturePeriod = 1800;//To set (Sec) value for Min(Sec)
                }
                else if (cmbBoxLSCapturePeriod.SelectedItem.ToString().Contains("60"))
                {
                    lodaSurveyCapturePeriod = 3600;//To set (Sec) value for Min(Sec)
                }
                // int lodaSurveyCapturePeriod = Convert.ToInt32(cmbBoxLSCapturePeriod.SelectedItem);
            }
            catch (Exception ex)    //Exception log for catch block
            {                
               //throw ex;
                logger.Log(LOGLEVELS.Error, "FillSIPData()", ex);
            }
            return lodaSurveyCapturePeriod;
        }


        /// <summary>
        /// get display timeout data from UI  into entity
        /// </summary>
        /// <returns></returns>
        private DisplayTimeout GetDisplayTimeoutData()
        {
            DisplayTimeout displyTimeout = new DisplayTimeout();
            try
            {
                displyTimeout.PushTimeout = Convert.ToInt32(txtPushButtonTimeout.Text);
                displyTimeout.ScrollTime = Convert.ToInt32(txtScrollTime.Text);


                displyTimeout.AutoScrollTime = string.IsNullOrEmpty(txtScrollResumeTime.Text.Trim()) ? 0 : Convert.ToInt32(txtScrollResumeTime.Text);
                displyTimeout.AutoScrollModeSelected = (chkAutoScrollTime.Checked) ? 1 : 0;

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDisplayTimeoutData()", ex);
               //throw ex;
            }            
            return displyTimeout;
        }

        /// <summary>
        /// This function checks whether user has made selctions before reading or writing meetr configurations
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True if proper selections have neen made, else False </returns>
        private bool CheckValidations(string action)
        {
            bool result = true;
            try
            {
                List<System.Enum> listSelected = new List<System.Enum>();
                listSelected.AddRange(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData));
                if (listSelected.Count == 0)
                {
                    MessageBox.Show("Please select at least one option.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    result = false;
                }
                if (listSelected.Contains(ProfileId.BillingReset) && action == "read")
                {
                    //Bug #208382 
                    listSelected.Remove(ProfileId.BillingReset);
                    if (!listSelected.Contains(ProfileId.DIP) && !listSelected.Contains(ProfileId.KvahSelection) && !listSelected.Contains(ProfileId.DisplayParameters) && !listSelected.Contains(ProfileId.TOU)
                       && !listSelected.Contains(ProfileId.FourTOU) && !listSelected.Contains(ProfileId.TwoTOU) && !listSelected.Contains(ProfileId.ThreeSTOU) && !listSelected.Contains(ProfileId.DIPWithSliding) && !listSelected.Contains(ProfileId.RTC) && !listSelected.Contains(ProfileId.BillingType) && !listSelected.Contains(ProfileId.RS232LockUnlock))
                    //if (!chkMDWithIP.Checked && !chkKVARSelcetion.Checked && !chkDisplayParam.Checked && !chkTOD.Checked && !chkRTC.Checked && !chkBilingType.Checked  && !chkLockRS232.Checked)
                    {
                        MessageBox.Show("Selected option(s) cannot be read", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result = false;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                result = false;
                logger.Log(LOGLEVELS.Error, "CheckValidations(string action)", ex);
            }
            return result;
        }

        /// <summary>
        /// Display meter RTC on  UI
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayMeterRTC(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new RTC(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    DataGridView dgvRTC = rtcCtrl.Controls[0].Controls["dGridRTC"] as DataGridView;
                    dgvRTC.Rows.Clear();
                    dgvRTC.Rows.Add();
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn1"].Value = dgvRTC.RowCount;
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn2"].Value =
                                                                    profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (!listSelectedParams.Contains(ProfileId.RTC))
                    {
                        listSelectedParams.Add(ProfileId.RTC);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in Displaying RTC", "BCS");
                logger.Log(LOGLEVELS.Error, "DisplayMeterRTC(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "RTC" + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Sets DIP with Sliding on UI control
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayDIPWithSliding(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                ProfileData profileData = new E650MeterConfiguration.DemandIntegrationPeriod(true).ParseData(receivedData,
                                                                                              GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    cmbDemandType.SelectedItem = Convert.ToInt32(acutalData.Substring(0, 1)) == 0 ? "Block Demand" : "Sliding Demand";
                    cmbDemandInterval.SelectedItem = (Int32.Parse(acutalData.Substring(1, 3), System.Globalization.NumberStyles.HexNumber) / 60).ToString();
                    cmbDemandSubInterlavTime.SelectedItem = (int.Parse(acutalData.Substring(0, 1), System.Globalization.NumberStyles.HexNumber) * 5).ToString();
                    if (!listSelectedParams.Contains(ProfileId.DIPWithSliding))
                    {
                        listSelectedParams.Add(ProfileId.DIPWithSliding);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayDIPWithSliding(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            this.StatusMessage = "MDIP and LSIP" + resourceMgr.GetString("ReadSuccess");

        }
        /// <summary>
        /// Sets DIP on UI control
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayDIP(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                ProfileData profileData = new E650MeterConfiguration.DemandIntegrationPeriod(true).ParseData(receivedData,
                                                                                              GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (acutalData == "0384")
                    {
                        cmbDIPDemandType.SelectedIndex = 0;
                        cmbDIPDemandInterval.SelectedIndex = 0;
                        labelDIPSubDemandInterval.Visible = false;
                        cmbDIPDemandSubIntervalTime.Visible = false;
                        labelDIPSubDemandIntervalUnit.Visible = false;
                    }
                    else if (acutalData == "0708")
                    {
                        cmbDIPDemandType.SelectedIndex = 0;
                        cmbDIPDemandInterval.SelectedIndex = 1;
                        labelDIPSubDemandInterval.Visible = false;
                        cmbDIPDemandSubIntervalTime.Visible = false;
                        labelDIPSubDemandIntervalUnit.Visible = false;
                    }
                    else if (acutalData == "1384")
                    {
                        cmbDIPDemandType.SelectedIndex = 1;
                        cmbDIPDemandInterval.SelectedIndex = 0;
                        cmbDIPDemandSubIntervalTime.Text = "05 (300)";
                        labelDIPSubDemandInterval.Visible = true;
                        cmbDIPDemandSubIntervalTime.Visible = true;
                        labelDIPSubDemandIntervalUnit.Visible = true;

                    }
                    else if (acutalData == "1708")
                    {
                        cmbDIPDemandType.SelectedIndex = 1;
                        cmbDIPDemandInterval.SelectedIndex = 1;
                        cmbDIPDemandSubIntervalTime.Text = "05 (300)";
                        labelDIPSubDemandInterval.Visible = true;
                        cmbDIPDemandSubIntervalTime.Visible = true;
                        labelDIPSubDemandIntervalUnit.Visible = true;
                    }
                    else if (acutalData == "2708")
                    {
                        cmbDIPDemandType.SelectedIndex = 1;
                        cmbDIPDemandInterval.SelectedIndex = 1;
                        cmbDIPDemandSubIntervalTime.Text = "10 (600)";
                        labelDIPSubDemandInterval.Visible = true;
                        cmbDIPDemandSubIntervalTime.Visible = true;
                        labelDIPSubDemandIntervalUnit.Visible = true;
                    }

                    if (!listSelectedParams.Contains(ProfileId.DIP))
                    {
                        listSelectedParams.Add(ProfileId.DIP);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayDIP(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            this.StatusMessage = "DIP " + resourceMgr.GetString("ReadSuccess");

        }
        /// <summary>
        ///  Converts ProfileCommand instance to DLMSCOMMAND instance .
        /// </summary>
        /// <param name="profileCommand"></param>
        /// <returns></returns>
        private DLMSCOMMAND GetDLMSCommandFromProfileCommand(ProfileCommand profileCommand)
        {
            DLMSCOMMAND dlmsCommand = new DLMSCOMMAND();
            dlmsCommand.ATTRIBUTE = profileCommand.Attribute.ToString();
            dlmsCommand.OBISCODE = profileCommand.ObisCode;
            dlmsCommand.CLASSNAME = profileCommand.ClassName;
            dlmsCommand.CLASS = profileCommand.ClassId.ToString();
            return dlmsCommand;

        }

        /// <summary>
        /// Sets Kvah selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayKVAhSelection(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ProfileData();

                //************Sapphire S2 optima *******************
                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString())
                    profileData = new CAB.E650MeterConfiguration.KVAHSelectionS2(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                else
                    profileData = new CAB.E650MeterConfiguration.KVAHSelection(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbKVAhLagOnly.Checked = true; //Disable
                    }
                    else
                    {
                        rdbKVAhLagLead.Checked = true; //Enable
                    }
                    if (!listSelectedParams.Contains(ProfileId.KvahSelection))
                    {
                        listSelectedParams.Add(ProfileId.KvahSelection);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying KvahSelection!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayKVAhSelection(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            this.StatusMessage = "kVah" + resourceMgr.GetString("ReadSuccess");

        }

        /// <summary>
        /// Sets Meter Pulse Energy Type  option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayPulseEnergySelection(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new CAB.E650MeterConfiguration.KVAHSelection(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;

                    if(byte.TryParse(resultData, out byte value))
                    {
                        if (value == (int)PulseEnergyValues.Active) rdbPulseActive.Checked = true;
                        else if (value == (int)PulseEnergyValues.Apparent) rdbPulseApparent.Checked = true;
                        else if (value == (int)PulseEnergyValues.Reactive) rdbPulseReactive.Checked = true;

                        if (!listSelectedParams.Contains(ProfileId.PulseEnergy))
                        {
                            listSelectedParams.Add(ProfileId.PulseEnergy);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Pulse Energy Selection!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayPulseEnergySelection(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            this.StatusMessage = "PulseEnergy" + resourceMgr.GetString("ReadSuccess");

        }

        /// <summary>
        /// Sets RS232 selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayRS232LockUnlock(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new RS232Lock(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbRS232Unlock.Checked = true;
                    }
                    else
                    {
                        rdbRS232Lock.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.RS232LockUnlock))
                    {
                        listSelectedParams.Add(ProfileId.RS232LockUnlock);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying RS232LockUnlock !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayRS232LockUnlock(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Lock Unlock RS232" + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Sets Auto Lock selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayAutoLockUnlock(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ProfileData();
                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString())
                     profileData = new AutoLockUnlockS2(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                else
                     profileData = new AutoLockUnlock(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "1")
                    {
                        rdbAutoUnlock.Checked = true;
                    }
                    else
                    {
                        rdbAutoLock.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.AutoLock))
                    {
                        listSelectedParams.Add(ProfileId.AutoLock);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Auto Lock !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayAutoLockUnlock(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Auto Lock Unlock" + resourceMgr.GetString("ReadSuccess");
        }


        /// <summary>
        /// Display Billing Month Type Data on UI, This will be called in case Other Type is selected [BillingType_Month]
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayBillingMonthType(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {

                ProfileData profileData = new BillingMonthType(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData.Length == 1)
                    {
                        resultData = resultData.ToString().PadLeft(2, '0');
                    }
                    ////sets 25 here , origionally it is 255 but BillingPeriod is a string and it accepts : 2 digit number , so it cuts 255 to 25 
                    if (resultData == "255")
                    {
                        normalBillingType.Checked = true;
                    }
                    else
                    {
                        otherBillingType.Checked = true;
                    }
                    if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.RubyE150Value || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SFSP)
                    {
                        if (resultData == "02")
                        {
                            monthlyBilling.Checked = true;
                        }
                        else if (resultData == "01")
                        {
                            oddMonthBilling.Checked = true;
                        }
                        else if (resultData == "00")
                        {
                            evenMonthBilling.Checked = true;
                        }
                    }
                    else
                    {
                    if (resultData == "00")
                    {
                        monthlyBilling.Checked = true;
                    }
                    else if (resultData == "01")
                    {
                        oddMonthBilling.Checked = true;
                    }
                    else if (resultData == "02")
                    {
                        evenMonthBilling.Checked = true;
                    }
                }
                }

                if (!listSelectedParams.Contains(ProfileId.BillingType))
                {
                    listSelectedParams.Add(ProfileId.BillingType);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayBillingMonthType(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Billing Mode " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Display Billing Type Data on UI
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayBillingDateTime(byte[] receivedData, ProfileCommand profileCommand)
        {
            string billingPeriod = string.Empty;
            try
            {
                cmbBoxBillingPeriod.Items.Clear();
                cmbBoxBillingPeriod.Items.Add("End of Month");
                cmbBoxBillingPeriod.Items.Add("User Defined");
                ProfileData profileData = new BillingType(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string actualData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    string billingMode = actualData.Substring(0, 2);
                    if (billingMode == "00")
                    {
                        cmbBoxBillingPeriod.SelectedIndex = 0;
                        cmbBoxBillingDate.Text = "";
                        cmbBoxBillingHour.Text = "";
                        cmbBoxBillingMinute.Text = "";

                        cmbBoxBillingDate.Enabled = false;
                        cmbBoxBillingHour.Enabled = false;
                        cmbBoxBillingMinute.Enabled = false;
                        billingPeriod = actualData.Substring(6, 2).ToString();
                    }
                    else
                    {
                        cmbBoxBillingPeriod.SelectedIndex = 1;

                        cmbBoxBillingDate.Text = actualData.Substring(0, 2).ToString();
                        cmbBoxBillingHour.Text = actualData.Substring(2, 2).ToString();
                        cmbBoxBillingMinute.Text = actualData.Substring(4, 2).ToString();
                        billingPeriod = actualData.Substring(6, 2).ToString();


                    }
                    //[RisingDemand_ReverseKWH_BillingType]
                    ////sets 25 here , origionally it is 255 but BillingPeriod is a string and it accepts : 2 digit number , so it cuts 255 to 25 
                    //if (billingPeriod == "25")
                    //{
                    //    normalBillingType.Checked = true;
                    //}
                    //else
                    //{
                    //    otherBillingType.Checked = true;
                    //	[BillingType_Month]
                    if (billingPeriod == "00")
                    {
                        monthlyBilling.Checked = true;
                    }
                    else if (billingPeriod == "01")
                    {
                        oddMonthBilling.Checked = true;
                    }
                    else if (billingPeriod == "02")
                    {
                        evenMonthBilling.Checked = true;
                    }
                    else
                    {
                        monthlyBilling.Checked = true;
                    }
                    //}
                    if (!listSelectedParams.Contains(ProfileId.BillingType))
                    {
                        listSelectedParams.Add(ProfileId.BillingType);
                    }
                    //Lockout days 
                    //cmbResetLockoutdays.Text = Convert.ToString(Convert.ToInt32(actualData.Substring(10, 2), 16));
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayBillingDateTime(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Billing Mode " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Display LS Capture period value on UI
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayLSCapturePeriod(byte[] receivedData)
        {
            int compValue = 0;
            compValue = (compValue | (int)receivedData[01]) << 8;
            compValue = (compValue | (int)receivedData[02]);
            if (compValue == 900)
            {
                cmbBoxLSCapturePeriod.SelectedIndex = 0;//To set Min(Sec) value for 900 sec  
            }
            else if (compValue == 1800)
            {
                cmbBoxLSCapturePeriod.SelectedIndex = 1;//To set Min(Sec) value for 1800 sec  
            }
            else if (compValue == 3600)
            {
                cmbBoxLSCapturePeriod.SelectedIndex = 2;//To set Min(Sec) value for 3600 sec  
            }
            //cmbBoxLSCapturePeriod.Text = Convert.ToString(compValue);
            if (!listSelectedParams.Contains(ProfileId.SIP))
            {
                listSelectedParams.Add(ProfileId.SIP);
            }
        }

        /// <summary>
        /// Display CTRatio Value on UI
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayCTRatio(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new CTRatio(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    nudCTRatio.Value = Convert.ToInt16(profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value);
                }
                if (!listSelectedParams.Contains(ProfileId.CTRatio))
                {
                    listSelectedParams.Add(ProfileId.CTRatio);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayCTRatio(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
        }
        /// <summary>
        /// Display PTRatio Value on UI
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayPTRatio(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new PTRatio(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    nudPTRatio.Value = Convert.ToInt16(profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value);
                }
                if (!listSelectedParams.Contains(ProfileId.PTRatio))
                {
                    listSelectedParams.Add(ProfileId.PTRatio);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayPTRatio(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
        }
        //*******************Dispaly HTCT RS485 **************************
        private void DisplayRS485(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                int compValue = 0;
                compValue = (compValue | (int)receivedData[01]) << 8;
                compValue = (compValue | (int)receivedData[02]);
                txtRS485DeviceAddress.Text = Convert.ToString(compValue);
            }
            catch (Exception ex)    //Exception log for catch block 
            {

                logger.Log(LOGLEVELS.Error, "DisplayRS485(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            //try
            //{
            //    ProfileData profileData = new PTRatio(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
            //    if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
            //    {
            //        nudPTRatio.Value = Convert.ToInt16(profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value);
            //    }
                
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// Display Billing reset lock out days
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayBillingResetLockOutDays(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ResetLockOutDays(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string actualData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    cmbResetLockoutdays.Text = ((int.Parse(actualData, System.Globalization.NumberStyles.HexNumber)) / (24 * 4)).ToString();
                    if (!listSelectedParams.Contains(ProfileId.BillingReset))
                    {
                        listSelectedParams.Add(ProfileId.BillingReset);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in dsiplaying Reset Lock out days !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayBillingResetLockOutDays(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }

            this.StatusMessage = "Reset Lockout Days  " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Fill Display parameters
        /// </summary>
        /// <param name="dGVPushDisplayParams"></param>
        /// <param name="paramType"></param>
        private void FillDisplayParameters(DataGridView dGVPushDisplayParams, string paramType)
        {

            rbLTCT.Enabled = !ConfigInfo.IsOnline;
            rbWCM.Enabled = !ConfigInfo.IsOnline;

            //DataSet displayParameters = null;
            XmlDataDocument xmlDataDocument = null;
            try
            {
                xmlDataDocument = new XmlDataDocument();
                //xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DisplayParameters.xml");

                if (ConfigInfo.SignatureInfo.Contains("ST"))
                {

                    xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "LTCT_DisplayParameters.xml");
                    rbLTCT.Enabled = true;
                    rbLTCT.Checked = true;
                }
                else if (ConfigInfo.SignatureInfo.Contains("st"))
                {

                    xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "LTCT_DisplayParameters.xml");
                    rbLTCT.Enabled = true;
                    rbLTCT.Checked = true;
                }
                else if (ConfigInfo.SignatureInfo.Contains("SM0110"))
                {

                    xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DisplayParameters_Falcon1PH.xml");
                    rbLTCT.Enabled =false;
                    rbLTCT.Checked = false;
                }

                else if (ConfigInfo.SignatureInfo.Contains("SM0310")&& ConfigInfo.SignatureInfo.Contains("SM0405"))
                {

                    xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DisplayParameters_Falcon3PH.xml");
                    rbLTCT.Enabled = false;
                    rbLTCT.Checked = false;
                }

                else
                {
                    xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DisplayParameters.xml");
                    rbWCM.Enabled = true;
                    rbWCM.Checked = true;
                }

                if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
                {
                    xmlDataDocument = null;
                    xmlDataDocument = new XmlDataDocument();
                    xmlDataDocument.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "\\" + "DisplayParameters_Extended.xml"));
                }

                //displayParameters = new DataSet();
                displayParameterRepository = xmlDataDocument.DataSet;
                dGVPushDisplayParams.DataSource = displayParameterRepository.DefaultViewManager;

                //specify grdiview datamember
                if (paramType == "PUSH")
                {
                    dGVPushDisplayParams.DataMember = "PushDisplayParams";
                }
                else if (paramType == "SCROLL")
                {
                    dGVPushDisplayParams.DataMember = "ScrollDisplayParams";
                }
                else if (paramType == "HIGHRESOLUTION")
                {
                    dGVPushDisplayParams.DataMember = "HighResolution";
                }

                DataGridViewColumn gridViewColumn = new DataGridViewCheckBoxColumn();
                gridViewColumn.Name = "colInclude";
                gridViewColumn.HeaderText = "Include";

                if (!dGVPushDisplayParams.Columns.Contains("colInclude"))
                {
                    dGVPushDisplayParams.Columns.Insert(dGVPushDisplayParams.Columns.Count, gridViewColumn);
                }
                dGVPushDisplayParams.Columns["SNO"].Width = 80;
                dGVPushDisplayParams.Columns["ID"].Width = 80;
                dGVPushDisplayParams.Columns["Description"].Width = 200;
                dGVPushDisplayParams.Columns["colInclude"].Width = 85;

                dGVPushDisplayParams.Columns["SNO"].ReadOnly = true;
                dGVPushDisplayParams.Columns["ID"].ReadOnly = true;
                dGVPushDisplayParams.Columns["Description"].ReadOnly = true;

                if (!listSelectedParams.Contains(ProfileId.DisplayParameters))
                {
                    listSelectedParams.Add(ProfileId.DisplayParameters);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillDisplayParameters(DataGridView dGVPushDisplayParams, string paramType)", ex);
            }
            finally
            {
                //dispose and free memory occupied by objects
                displayParameterRepository.Dispose();
            }

        }

        /// <summary>
        /// fill display timeout dat on ui
        /// </summary>
        /// <param name="receivedData"></param>
        private void FillDisplayParametersTimeouts(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                ProfileData profileData = new DisplayTimeoutParameter(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    foreach (DataElement dataElement in profileData.ListMeterDataPacket[0].ListDataElementValue)
                    {
                        if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 0)
                        {
                            txtPushButtonTimeout.Text = dataElement.Value;
                        }
                        else if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 1)
                        {
                            txtScrollTime.Text = dataElement.Value;
                        }
                        else if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 2)
                        {
                            txtScrollResumeTime.Text = dataElement.Value;
                            chkAutoScrollTime.Checked = true;
                        }

                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in showing Display Tiemout data !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "FillDisplayParametersTimeouts(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
        }

        /// <summary>
        /// Sets Manual Billing option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayManualBilling(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new CAB.E650MeterConfiguration.ManualBilling(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbDisableManualBilling.Checked = true;
                    }
                    else
                    {
                        rdbEnableManualBilling.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.ManualBilling))
                    {
                        listSelectedParams.Add(ProfileId.ManualBilling);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Manual Billing !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayManualBilling(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Manual Billing" + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Sets Software Billing option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        /// 
        
            //****************** Smart Meter Connect Disconnect Status Control ***************
        private void DisplayConnectDisconnectStatus(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new CAB.E650MeterConfiguration.ManualBilling(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                      chkDisconnect.Checked = true;
                    }
                    else
                    {
                      chkconnect.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.DisconnectControl))
                    {
                        listSelectedParams.Add(ProfileId.DisconnectControl);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Connect Disconnect Status !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayConnectDisconnectStatus(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Connect Disconnect Status" + resourceMgr.GetString("ReadSuccess");
        }

       //*********************** Smart Meter Load Control ***************
        
        private void DisplayLoadControl(byte[] receivedData, ProfileCommand profileCommand)
        {
            Control[] mycontrol = new Control[] { cmbOverload, cmbOverloadCurrent, txtTimeInterval, txtMaxretry, txtWaitTime, txtMaxRetryCycle, txtRelayReconnectiontime };
            try
            {
                int startDataindx = 0;                
                if (receivedData[startDataindx++] == 0x02) 
                {
                    int stractcount = 0;

                    int lengthodstruct = receivedData[startDataindx++];
                    while (stractcount < mycontrol.Length)
                    {
                        if (receivedData[startDataindx] == 0x11)
                        {
                            startDataindx++;
                            if (mycontrol[stractcount].GetType() == typeof(System.Windows.Forms.ComboBox))
                            {
                                ComboBox cb = (ComboBox)mycontrol[stractcount];
                                cb.SelectedIndex = receivedData[startDataindx++];
                            }
                            else
                            {
                                TextBox txtbx = (TextBox)mycontrol[stractcount];
                                txtbx.Text = receivedData[startDataindx++].ToString();
                            }
                        }                      
                            else if (receivedData[startDataindx] == 0x04)//unsigned 2 byte
                            {
                            startDataindx++;
                            int recBytelen = receivedData[startDataindx];
                            startDataindx += 1;
                            byte[] lCByteData = new byte[recBytelen / 8];
                            Array.Copy(receivedData, startDataindx, lCByteData, 0, lCByteData.Length);
                            List<byte> convertedByteList = ReverseBitsofByteList(lCByteData);
                            BitArray myarra = new BitArray(convertedByteList.ToArray());
                            // dataindexByte = 0;
                            startDataindx += lCByteData.Length;
                            cmbOverload.SelectedIndex = Convert.ToInt32(myarra[0]);//int.Parse(bitlist[15].ToString());
                            cmbOverloadCurrent.SelectedIndex = Convert.ToInt32(myarra[1]); //int.Parse(bitlist[14].ToString());
                            stractcount += 1;
                            
                        }
                        stractcount++;
                    }
                }
            }

            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Load Control !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayLoadControl(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Load Control" + resourceMgr.GetString("ReadSuccess");
        }

        public static List<byte> ReverseBitsofByteList(byte[] recByteList)
        {
            List<byte> convertedlist = new List<byte>();
            try
            {
                foreach (byte item in recByteList)
                {
                    char[] bitarr = Convert.ToString(item, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(bitarr);
                    convertedlist.Add((byte)Convert.ToInt32(new string(bitarr), 2));
                }
                return convertedlist;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //***********************1P Smart Meter Load Control ***************
        private void DisplayLoadControl1PSmartMeter(byte[] receivedData, ProfileCommand profileCommand)
        {
            Control[] mycontrol = new Control[] { cmb1phOverload, cmb1phOverloadCurrent, txt1phTimeInterval, txt1phMaxretry, txt1phWaitTime, txt1phMaxRetryCycle, txt1phRelayReconnectiontime };
            try
            {
                int startDataindx = 0;
                if (receivedData[startDataindx++] == 0x02)
                {
                    int stractcount = 0;

                    int lengthodstruct = receivedData[startDataindx++];
                    while (stractcount < mycontrol.Length)
                    {
                        if (receivedData[startDataindx] == 0x11)
                        {
                            startDataindx++;
                            if (mycontrol[stractcount].GetType() == typeof(System.Windows.Forms.ComboBox))
                            {
                                ComboBox cb = (ComboBox)mycontrol[stractcount];
                                cb.SelectedIndex = receivedData[startDataindx++];
                            }
                            else
                            {
                                TextBox txtbx = (TextBox)mycontrol[stractcount];
                                txtbx.Text = receivedData[startDataindx++].ToString();
                            }
                        }
                        else if (receivedData[startDataindx] == 0x04)//unsigned 2 byte
                        {
                            startDataindx++;
                            int recBytelen = receivedData[startDataindx];
                            startDataindx += 1;
                            byte[] lCByteData = new byte[recBytelen / 8];
                            Array.Copy(receivedData, startDataindx, lCByteData, 0, lCByteData.Length);
                            List<byte> convertedByteList = ReverseBitsofByteList(lCByteData);
                            BitArray myarra = new BitArray(convertedByteList.ToArray());                           
                            startDataindx += lCByteData.Length;
                            // cmb1phOverloadCurrent.SelectedIndex = Convert.ToInt32(myarra[0]);
                            // cmb1phOverload.SelectedIndex = Convert.ToInt32(myarra[1]);
                            cmb1phOverload.SelectedIndex = Convert.ToInt32(myarra[(int)LOADCONTROLBIT.Over_Current]);
                            cmb1phOverloadCurrent.SelectedIndex = Convert.ToInt32(myarra[(int)LOADCONTROLBIT.OverLoad]);
                            stractcount += 1;
                        }
                        stractcount++;
                    }
                }
            }

            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Load Control !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayLoadControl(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Load Control" + resourceMgr.GetString("ReadSuccess");
        }

        private void DisplayDatainControl(byte[] receivedData, TextBox[] txtboxobject, string displayFormat, decimal emf)
        {
            try
            {
                int startDataindx = 18;
                string[] TamperpersistanceTime = new string[8];

                if (receivedData[startDataindx++] == 0x02) //srtact
                {
                    int stractcount = 0;
                    int lengthodstruct = receivedData[startDataindx++];//length of stract
                    while (stractcount < lengthodstruct)
                    {
                        if (receivedData[startDataindx] == 18)//unsigned 2 byte
                        {
                            startDataindx++;
                            TamperpersistanceTime[0] = receivedData[startDataindx++].ToString("X").PadLeft(2, '0');
                            TamperpersistanceTime[1] = receivedData[startDataindx++].ToString("X").PadLeft(2, '0');
                            txtboxobject[stractcount].Text = (Convert.ToInt32((TamperpersistanceTime[0] + TamperpersistanceTime[1]), 16) / emf).ToString(displayFormat);


                        }
                        else if (receivedData[startDataindx] == 17)//unsigned long 1 byte
                        {
                            startDataindx++;
                            TamperpersistanceTime[0] = receivedData[startDataindx++].ToString("X");
                            txtboxobject[stractcount].Text = (Convert.ToInt32((TamperpersistanceTime[0]), 16) / emf).ToString(displayFormat);

                        }
                        stractcount++;
                    }

                }


            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Load Control !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayDatainControl(byte[] receivedData, TextBox[] txtboxobject, string displayFormat, decimal emf)", ex);
            }
            this.StatusMessage = "Load Control" + resourceMgr.GetString("ReadSuccess");


        }



        private void DisplaySoftwareBilling(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ProfileData();
                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString())
                     profileData = new CAB.E650MeterConfiguration.SoftwareBillingS2(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                else
                     profileData = new CAB.E650MeterConfiguration.ManualBilling(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbDisableSoftwareBilling.Checked = true;
                    }
                    else
                    {
                        rdbEnableSoftwareBilling.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.SoftwareBilling))
                    {
                        listSelectedParams.Add(ProfileId.SoftwareBilling);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Software Billing !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplaySoftwareBilling(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Software Billing" + resourceMgr.GetString("ReadSuccess");
        }

        private void DisplayManualButtonMDReset(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ProfileData();
                if (ConfigInfo.MeterModel == NamePlateConstants.SapphireS2.ToString())
                    profileData = new CAB.E650MeterConfiguration.ManualButtonMDResetS2(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
               
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbMDResetDisable.Checked = true;
                    }
                    else
                    {
                        rdbMDResetEnable.Checked = true;
                    }
                    if (!listSelectedParams.Contains(ProfileId.ManualButtonMDReset))
                    {
                        listSelectedParams.Add(ProfileId.ManualButtonMDReset);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Error in displaying Manual button MD reset!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "DisplayManualButtonMDReset(byte[] receivedData, ProfileCommand profileCommand)", ex);
            }
            this.StatusMessage = "Manual Button MD Reset" + resourceMgr.GetString("ReadSuccess");
        }


        /// <summary>
        /// Used to validate programming data before writting configuration to meter
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string ValidateConfiguration(string action)
        {
            StringBuilder errorMessage = new StringBuilder();
            List<System.Enum> listSelected = new List<System.Enum>();
            listSelected.AddRange(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData));
            if (listSelected.Contains(ProfileId.DIPWithSliding))
            {
                if (cmbDemandType.Text == "")
                {
                    errorMessage.Append("Demand type can't be left blank." + Symbols.NEWLINE);

                }
                if (cmbDemandInterval.Text == "")
                {
                    errorMessage.Append("Demand interval can't be left blank." + Symbols.NEWLINE);

                }
                if (cmbDemandType.Text != "Block Demand" && cmbDemandInterval.SelectedIndex == 1 && cmbDemandSubInterlavTime.Text == "")
                {
                    errorMessage.Append("Demand sub interval can't be left blank." + Symbols.NEWLINE);

                }
            }
            if (listSelected.Contains(ProfileId.DIP))
            {
                if (cmbDIPDemandType.Text == "")
                {
                    errorMessage.Append("Demand interval can't be left blank." + Symbols.NEWLINE);
                }
            }
            if (listSelected.Contains(ProfileId.BillingReset))
            {
                if (!chkMDReset.Checked)
                {
                    errorMessage.Append("Please select billing reset checkbox." + Symbols.NEWLINE);
                }
            }
            if (listSelected.Contains(ProfileId.BillingType))
            {
                if (cmbBoxBillingPeriod.Text == "")
                {
                    errorMessage.Append("Billing Period can't be left blank." + Symbols.NEWLINE);

                }
                //Currently this code is not needed
                ////To support Single Phase DLMS meter for only BillingDateTime write
                ////Read Configuration support Billing Cycle but Write Configuration does not support Billing Cycle
                //if (otherBillingType.Visible == false && normalBillingType.Checked == false)
                //{
                //    errorMessage.Append("Please select normal mode in Billing Type." + Symbols.NEWLINE);
                //}
            }
            if (listSelected.Contains(ProfileId.SIP))
            {
                if (cmbBoxLSCapturePeriod.Text == "")
                {
                    errorMessage.Append("Load survey capture period can't be left blank." + Symbols.NEWLINE);

                }
            }
            if ((listSelected.Contains(ProfileId.TOU) || listSelected.Contains(ProfileId.TwoTOU) || listSelected.Contains(ProfileId.ThreeSTOU)
                 || listSelected.Contains(ProfileId.FourTOU)) && !rdbTOUWithHoliday.Checked && !rdbTOUWithFourSeason1Phase.Checked && !rbTOUFourSeason1P10Zone8Slots.Checked  )
            {
                if (!rdbTOUType.Checked)
                {
                    errorMessage.Append("Current TOU cannot be programmed in meter please select future TOU." + Symbols.NEWLINE);
                }
                string touMessage = ValidateTOUGrids();
                if (touMessage.Length > 0)
                {
                    errorMessage.Append(ValidateTOUGrids());
                }
            }
            if ((listSelected.Contains(ProfileId.TOU) || listSelected.Contains(ProfileId.TwoTOU)
                 || listSelected.Contains(ProfileId.FourTOU)) && rdbTOUWithHoliday.Checked && !rdbTOUWithFourSeason1Phase.Checked)
            {
                if (!isValidTOU)
                {
                    errorMessage.Append("Invalid TOU Entry." + Symbols.NEWLINE);

                }
                string touMessage = ValidateTOUData();
                if (touMessage != string.Empty)
                {
                    errorMessage.Append(touMessage);
                }
            }

            //if ((listSelected.Contains(ProfileId.TOU) || listSelected.Contains(ProfileId.FourTOU))  )// add pradipta
            //{
            //    if (!isValidTOU)
            //    {
            //        errorMessage.Append("Invalid TOU Entry." + Symbols.NEWLINE);

            //    }
            //    string touMessage = ValidateTOUData();
            //    if (touMessage != string.Empty)
            //    {
            //        errorMessage.Append(touMessage);
            //    }
            //}



            if (listSelected.Contains(ProfileId.RS232LockUnlock))
            {
                if (!rdbRS232Lock.Checked && !rdbRS232Unlock.Checked)
                {
                    errorMessage.Append("Please select Lock/Unlock RS232 option." + Symbols.NEWLINE);
                }
            }
            if (listSelected.Contains(ProfileId.KvahSelection))
            {
                if (!rdbKVAhLagOnly.Checked && !rdbKVAhLagLead.Checked)
                {
                    errorMessage.Append("Please select kvah Selection mode." + Symbols.NEWLINE);

                }
            }
            if (listSelected.Contains(ProfileId.ManualButtonMDReset))
            {
                if (!rdbMDResetEnable.Checked && !rdbMDResetDisable.Checked)
                {
                    errorMessage.Append("Please select Manual Button MD reset Enable/Disable" + Symbols.NEWLINE);

                }
            }

            if (listSelected.Contains(ProfileId.AutoLock))
            {
                if (!rdbAutoLock.Checked && !rdbAutoUnlock.Checked)
                {
                    errorMessage.Append("Please select auto lock/unlock option." + Symbols.NEWLINE);

                }
            }
            if (listSelected.Contains(ProfileId.DisplayParameters))
            {
                errorMessage.Append(ValidateDisplayParameters());
            }

            if (listSelected.Contains(ProfileId.PTRatio))
            {
                if (nudPTRatio.Value > 320 && nudPTRatio.Value < 1)
                {
                    errorMessage.Append("Inavlid PTRatio." + Symbols.NEWLINE);
                }
            }
            if (listSelected.Contains(ProfileId.CTRatio))
            {
                if (nudPTRatio.Value > 300 && nudCTRatio.Value < 1)
                {
                    errorMessage.Append("Inavlid CTRatio." + Symbols.NEWLINE);
                }
            }
            if (listSelected.Contains(ProfileId.ManualBilling))
            {
                if (!rdbEnableManualBilling.Checked && !rdbDisableManualBilling.Checked)
                {
                    errorMessage.Append("Please select Manual Billing Programmability option." + Symbols.NEWLINE);

                }
            }

            if (listSelected.Contains(ProfileId.SoftwareBilling))
            {
                if (!rdbEnableSoftwareBilling.Checked && !rdbDisableSoftwareBilling.Checked)
                {
                    errorMessage.Append("Please select Software Billing Programmability option." + Symbols.NEWLINE);

                }
            }
            // Task ID: 569567 added Tamper Reset option for Torrent Power 3P 10-60 WCM meter model = 17 having specific right authority to reset
            if (listSelected.Contains(ProfileId.MagneticTamperIcon) || listSelected.Contains(ProfileId.MagneticTamperIcon3P))
            {
                if (!checkBoxMagneticTamperIcon.Checked)
                {
                    errorMessage.Append("Please select Tamper Reset checkbox." + Symbols.NEWLINE);
                }
            }            
           
           
            //******* This validation is added for smart meter Disconnect control *****
            if (listSelected.Contains(ProfileId.DisconnectControl ))
            {
                if (!chkconnect.Checked && !chkDisconnect.Checked)
                {
                    errorMessage.Append("Please select Connect/Disconnect checkbox." + Symbols.NEWLINE);
                }
                
            }
            //******* This validation is added for smart meter 1 phase load control *****
            if (listSelected.Contains(ProfileId.LoadControl1PSmartMeter))
            {
                if (txt1phTimeInterval.Text == string.Empty)
                    errorMessage.Append("Time interval cant'be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txt1phTimeInterval.Text) > 10 || Convert.ToInt32(txt1phTimeInterval.Text) < 1)
                        errorMessage.Append("Invalid Time Interval range 1-10" + Symbols.NEWLINE);
                }

                if (txt1phMaxretry.Text == string.Empty)
                    errorMessage.Append("Max Retry Cycle cant'be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txt1phMaxretry.Text) > 10 || Convert.ToInt32(txt1phMaxretry.Text) < 1)
                        errorMessage.Append("Invalid Max Retry Cycle range 1-10" + Symbols.NEWLINE);
                }
                if (txt1phWaitTime.Text == string.Empty)
                    errorMessage.Append("Wait Time For Next Retry Cycle can't be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txt1phWaitTime.Text) > 60 || Convert.ToInt32(txt1phWaitTime.Text) < 15)
                        errorMessage.Append("Invalid Wait Time For Next Retry Cycle 15-60" + Symbols.NEWLINE);
                }

                if (txt1phMaxRetryCycle.Text == string.Empty)
                    errorMessage.Append("Max Retry in a Cycle can't be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txt1phMaxRetryCycle.Text) > 5 || Convert.ToInt32(txt1phMaxRetryCycle.Text) < 1)
                        errorMessage.Append("Invalid Max Retry in a Cycle 1-5" + Symbols.NEWLINE);
                }
            }
            //******* This validation is added for smart meter 3 phase load control *****
            if (listSelected.Contains(ProfileId.LoadControl))
            {
                if (txtTimeInterval.Text == string.Empty)
                    errorMessage.Append("Time interval cant'be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txtTimeInterval.Text) > 10 || Convert.ToInt32(txtTimeInterval.Text) < 1)
                        errorMessage.Append("Invalid Time Interval range 1-10" + Symbols.NEWLINE);
                }

                if (txtMaxretry.Text == string.Empty)
                    errorMessage.Append("Max Retry Cycle cant'be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txtMaxretry.Text) > 10 || Convert.ToInt32(txtMaxretry.Text) < 1)
                        errorMessage.Append("Invalid Max Retry Cycle range 1-10" + Symbols.NEWLINE);
                }
                if (txtWaitTime.Text == string.Empty)
                    errorMessage.Append("Wait Time For Next Retry Cycle can't be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txtWaitTime.Text) > 60 || Convert.ToInt32(txtWaitTime.Text) < 15)
                        errorMessage.Append("Invalid Wait Time For Next Retry Cycle 15-60" + Symbols.NEWLINE);
                }

                if (txtMaxRetryCycle.Text == string.Empty)
                    errorMessage.Append("Max Retry in a Cycle can't be blank" + Symbols.NEWLINE);
                else
                {
                    if (Convert.ToInt32(txtMaxRetryCycle.Text) > 5 || Convert.ToInt32(txtMaxRetryCycle.Text) < 1)
                        errorMessage.Append("Invalid Max Retry in a Cycle 1-5" + Symbols.NEWLINE);
                }
            }

            if (listSelected.Contains(ProfileId.FourSPTOU10Z8S))
            { 
           
                int checkfornull = 0;
                if (dgvSpclDayProf8Tariff.Rows[0].Cells[1].Value != null && dgvSpclDayProf8Tariff.Rows[0].Cells[2].Value != null && dgvSpclDayProf8Tariff.Rows[0].Cells[3].Value != null) checkfornull++;
                if (checkfornull <= 0)
                {
                    MessageBox.Show("Special Day's Profile table can not be left blank.", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                   // return false;
                }

                if (!isValidSplDayData())
                {
                    MessageBox.Show("Special Day's Profile table could not contain same data ", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                   // return false;
                }
           }

            if (listSelected.Contains(ProfileId.RS485))
            {
                if (txtRS485DeviceAddress.Text == string.Empty)
                    errorMessage.Append("RS485 Device Address  cant'be blank" + Symbols.NEWLINE);
                    int RS485DeviceAddressMin = 16;
                    int RS485DeviceAddressMax = 16381;
                if (!ValueInBetween(txtRS485DeviceAddress.Text.Trim(), RS485DeviceAddressMin, RS485DeviceAddressMax))
                {
                    errorMessage.Append("Please Enter Valid RS485 Device Address " + Symbols.NEWLINE);
                    Application.DoEvents();
                    txtRS485DeviceAddress.Focus();
                   
                }
                
            }
            if (listSelected.Contains(ProfileId.PulseEnergy))
            {
                if (grpPulseEnergy.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked) == null)
                {
                    errorMessage.Append("Invalid Pulse Energy Type." + Symbols.NEWLINE);
                }
            }

            return errorMessage.ToString();
        }
        private bool ValueInBetween(string checkValue, Single minValue, Single maxValue)
        {
            Single temp = 0;

            if (Single.TryParse(checkValue, out temp) == false)
            {
                return false;
            }

            if (temp >= minValue && temp <= maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isValidSplDayData()
        {
            int irowdatacount = 0;
            List<string> mylist = new List<string>();

            for (int icount = 0; icount < dgvSpclDayProf8Tariff.RowCount; icount++)
            {
                irowdatacount = icount;
                if (dgvSpclDayProf8Tariff.Rows[icount].Cells[1].Value == null || dgvSpclDayProf8Tariff.Rows[icount].Cells[2].Value == null) break;
                string strval = dgvSpclDayProf8Tariff.Rows[icount].Cells[1].Value.ToString() + "-" + dgvSpclDayProf8Tariff.Rows[icount].Cells[2].Value.ToString();
                int ival = mylist.IndexOf(strval);

                if (ival >= 0)
                {
                    dgvSpclDayProf8Tariff.ClearSelection();
                    dgvSpclDayProf8Tariff.Rows[icount].Cells[1].Selected = true;
                    return false;
                }

                mylist.Add(dgvSpclDayProf8Tariff.Rows[icount].Cells[1].Value.ToString() + "-" + dgvSpclDayProf8Tariff.Rows[icount].Cells[2].Value.ToString());
            }

            return true;
        }
        /// <summary>
        /// Code Copied from IEC
        /// </summary>
        /// <returns></returns>
        private string ValidateTOUData()
        {
            DataGridView[] gridSeason = GetSeasonGridCollection();
            DataGridView[] gridHoliday = GetHolidayGridCollection();
            StringBuilder errorMessage = new StringBuilder();
            foreach (DataGridView gridTOU in gridSeason)
            {
                errorMessage.Append(CheckTOUSlots(gridTOU));

                if (errorMessage.ToString() != string.Empty)
                {
                    errorMessage.Append(Symbols.NEWLINE);
                    break;
                }
            }
            if (!CheckActivationDate())
            {
                errorMessage.Append(string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy")));
                errorMessage.Append(Symbols.NEWLINE);
            }

            return errorMessage.ToString();
        }
        /// <summary>
        /// Code copied form IEC
        /// </summary>
        /// <param name="gridTOU"></param>
        /// <returns></returns>
        private string CheckTOUSlots(DataGridView gridTOU)
        {
            if (gridTOU.Columns.Count == 0 || gridTOU.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return "Season slots empty!";
            }


            if (gridTOU.Rows[0].Cells[1].Value.ToString() == "00")
            {

                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return "Season slots not complete!";
            }
            if (!isValidTOU)
            {
                //this.StatusMessage = "Invalid Entry!";
                Application.DoEvents();
                return "Invalid Entry!";
            }
            int grdRowIndex = 0;
            for (grdRowIndex = 1; grdRowIndex <= 9; grdRowIndex++)
            {
                if (Convert.ToString(gridTOU.Rows[grdRowIndex].Cells[1].Value) != "00")
                {
                    if (Convert.ToString(gridTOU.Rows[grdRowIndex].Cells[2].Value) == "00")
                    {
                        if ((Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) != "00") || (Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(gridTOU.Rows[grdRowIndex - 1].Cells[3].Value) == 45) || (Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(gridTOU.Rows[grdRowIndex - 1].Cells[3].Value) >= Convert.ToInt16(gridTOU.Rows[grdRowIndex].Cells[3].Value)))
                        {
                            return "Invalid Entry!";
                        }
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Code Copied from IEC
        /// </summary>
        /// <returns></returns>
        private bool CheckActivationDate()
        {
            DateTime prevDate = new DateTime();
            DateTime currentDate = new DateTime();
            DateTime futureActivationDate = DateTime.Now.AddDays(1).Date;
            TimeSpan ts = futureActivationDate.Date.Subtract(dtPickerFutureActivationDate.Value.Date);

            if (ts.Days > 0)
            {
                this.StatusMessage = string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                return false;
            }

            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                //DateTime dt;
                if (row.Index == 0)
                {
                    prevDate = ProgrammingCommon.GetDate(Convert.ToDateTime(row.Cells[0].Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), false);
                }
                else
                {
                    currentDate = ProgrammingCommon.GetDate(Convert.ToDateTime(row.Cells[0].Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), true);
                    if (prevDate.Day == currentDate.Day && prevDate.Month == currentDate.Month)
                    {
                        this.StatusMessage = "Season activation dates should be unique";
                        return false;
                    }
                    prevDate = currentDate.Date;
                }
                if (prevDate.Day == 29 && prevDate.Month == 2)
                {
                    this.StatusMessage = "  29 Feb cannot be selected as a Season Activation Date";
                    return false;
                }

                //if (prevDate < dtPickerFutureActivationDate.Value.Date)
                //{
                //    this.StatusMessage = "Season Activation date cannot be less than the Future TOU Activation Date";
                //    return false;
                //}
            }
            return true;
        }

        /// <summary>
        ///  used to validate data of dat profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateDayTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {

                    if (dayProfileGrids[gridCount].Rows[0].Cells[COLTARIFF].Value == null)
                    {

                        if (dayProfileCount == 24)
                        {
                            errorMessage = "Please fill TOU day table." + Symbols.NEWLINE;
                        }
                        else
                        {
                            errorMessage = "Please fill TOD Detail." + Symbols.NEWLINE;
                        }
                        break;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateDayTOUGrids()", ex);
            }
            return errorMessage;
        }


        /// <summary>
        ///  used to validate data of week profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateWeekTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int rowCount = 0; rowCount < weekProfileGrid.RowCount; rowCount++)
                {
                    for (int colCount = 1; colCount < weekProfileGrid.ColumnCount; colCount++)
                    {
                        if (weekProfileGrid.Rows[rowCount].Cells[colCount].Value == null)
                        {
                            errorMessage = "Please fill TOU week table." + Symbols.NEWLINE;
                            break;
                        }
                    }
                    if (errorMessage.Length > 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateWeekTOUGrids()", ex);
            }
            return errorMessage;
        }

        /// <summary>
        /// used to validate data of season profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateSeasonTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int rowCount = 0; rowCount < seasonProfileGrid.RowCount; rowCount++)
                {
                    if (seasonProfileGrid.Rows[rowCount].Cells[COLMONTH].Value == null)
                    {
                        errorMessage = "Please fill TOU season table." + Symbols.NEWLINE;
                        break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
               //throw ex;
                logger.Log(LOGLEVELS.Error, "ValidateSeasonTOUGrids()", ex);
            }
            return errorMessage;
        }
        /// <summary>
        /// Validate display parameters 
        /// </summary>
        /// <returns></returns>
        private string ValidateDisplayParameters()
        {
            StringBuilder validationMessage = new StringBuilder();
            if (GetSelectedRowsinParameterGrid(dGVPushDisplayParams).Count == 0 && selectedPushParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 push button display parameter." + Symbols.NEWLINE);
            }
            if (GetSelectedRowsinParameterGrid(dGVScrollDisplayParams).Count == 0 && selectedScrollParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 scroll button display parameter." + Symbols.NEWLINE);
            }
            if (GetSelectedRowsinParameterGrid(dGVHighResolution).Count == 0 && selectedHighResParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 high resolution display parameter" + Symbols.NEWLINE);
            }
            // Temporary commented it to run the display parameters 
            //validationMessage.Append(ValidateDisplayTimeout(txtScrollTime.Text, txtPushButtonTimeout.Text, txtScrollResumeTime.Text));

            return validationMessage.ToString();
        }
        /// <summary>
        /// Bind default data to billing type controls
        /// </summary>
        private void BindBillingTypeControls()
        {
            for (int i = 0; i <= 255; i++)
            {
                cmbResetLockoutdays.Items.Add(i);
            }
        }
        /// <summary>
        /// This function returns the file name of cfg file chosen by user
        /// </summary>
        /// <param name="fileText"></param>
        private string GetFileName()
        {
            string fileLocation = string.Empty;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "cfg";
                saveFileDialog.Filter = "Export file (*.cfg)|*.cfg";
                saveFileDialog.AddExtension = true;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".cfg";
                saveFileDialog.Title = "Save As";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName == "")
                    {
                        CABMessageBox.ShowFilterMessage("File name cannot be blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    }
                    else
                    {
                        fileLocation = saveFileDialog.FileName.Trim();
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileName()", ex);
            }
            finally
            {

            }
            return fileLocation;
        }
        /// <summary>
        /// Update Select All check box on each cell check box click .
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void UpdateSelectAllCheckBoxForDisplayParameters(DataGridView dataGridView, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dataGridView.CurrentCell.Value == null) return;
                if (e.ColumnIndex == 0)
                {
                    dataGridView.EndEdit();
                    chkDisplayParamSelectAll.CheckedChanged -= chkDisplayParamSelectAll_CheckedChanged;
                    //if (!(bool)dataGridView.CurrentCell.Value)
                    if (!Convert.ToBoolean(dataGridView.CurrentCell.Value))
                        chkDisplayParamSelectAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                            //if (cell.Value == null || (bool)(cell.Value) == false)
                            if (cell.Value == null || Convert.ToBoolean(cell.Value) == false) // Story - 427028 - not able to check the cell in display parameter

                            { IfAllRowsSelected = false; break; }
                        }
                        chkDisplayParamSelectAll.Checked = IfAllRowsSelected;
                    }
                    this.chkDisplayParamSelectAll.CheckedChanged += chkDisplayParamSelectAll_CheckedChanged;
                }
            }
        }

        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CommunicationType GetCommuniactioType()
        {
            CommunicationType commType = CommunicationType.DIRECT;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (channelType == CABCommunication.PhysicalLayer.ChannelType.GSM.ToString())
            {
                commType = CommunicationType.GSM;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString())
            {
                commType = CommunicationType.PSTN;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString())
            {
                commType = CommunicationType.GPRS;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.TCP.ToString())
            {
                commType = CommunicationType.TCP;
            }
            return commType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateGrid()
        {
            bool isSuccess = false;

            if (dgvMeterIdAndSim != null)
            {
                for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                {
                    DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(chk1.Value) == true)
                    {
                        isSuccess = true;
                    }

                }
            }

            return isSuccess;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateCheckList(int ThreadPoolSize)
        {
            bool isSuccess = false;
            int CheckListCounter = 0;
            if (dgvMeterIdAndSim != null)
            {
                for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                {
                    DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(chk1.Value) == true)
                    {
                        CheckListCounter++;
                    }
                }
                //Lower and Upper Bound for Checked Rows in DataGridView
                if (CheckListCounter > 0 && CheckListCounter <= ThreadPoolSize)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }


        /// <summary>
        /// Change grid status while click on read button .
        /// </summary>
        private void ResetGrid(bool resetToInitialMode)
        {
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {
                DataGridViewCheckBoxCell selectedCheckBox = dgvMeterIdAndSim.Rows[count].Cells["Select"] as DataGridViewCheckBoxCell;
                if (resetToInitialMode)
                {
                    selectedCheckBox.Value = false;
                    selectAll.Checked = false;
                }
                if (Convert.ToBoolean(selectedCheckBox.Value))
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = System.Drawing.Color.LightGray;
                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style = style;
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Enqueue";
                }
                else
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = System.Drawing.Color.White;
                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style = style;
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started";
                }
            }
        }

        private void ResetGridonetomany(bool resetToInitialMode)
        {
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {
                DataGridViewCheckBoxCell selectedCheckBox = dgvMeterIdAndSim.Rows[count].Cells["Select"] as DataGridViewCheckBoxCell;
                if (resetToInitialMode)
                {
                    selectedCheckBox.Value = false;
                    selectAll.Checked = false;
                }
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.BackColor = System.Drawing.Color.White;
                dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style = style;
                dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started";
               
            }
        }
        /// <summary>
        /// Enables abort button.
        /// </summary>
        private void EnableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(EnableAbort));
            }
            else
            {
                btnAbort.Enabled = true;
            }
        }
        /// <summary>
        /// Disable abort button.
        /// </summary>
        private void DisableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(DisableAbort));
            }
            else
            {
                btnAbort.Enabled = false;
            }
        }

        /// <summary>
        /// Gets Flags for CFG file.
        /// </summary>
        /// <param name="selectedProfiles"></param>
        /// <returns></returns>
        private string GetAuthenticationFlag(List<System.Enum> selectedProfiles)
        {
            string authFlag = string.Empty;
            if (selectedProfiles.Contains(ProfileId.RTC))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.MagneticTamperIcon))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            //-------------Rohit----------1phaseNDLMS TOU selected then only bit on--------------//
            if (selectedProfiles.Contains(ProfileId.FourTOU)
                || selectedProfiles.Contains(ProfileId.TwoTOU)
                || selectedProfiles.Contains(ProfileId.TOU) || selectedProfiles.Contains(ProfileId.FourSPTOU))
            {
                if (rdbTOUWithFourSeason1Phase.Checked || rbTOUFourSeason1P10Zone8Slots.Checked)
                {
                    authFlag += "1"; //NDLMS selected
                }
                else
                {
                    authFlag += "0"; //DLMS selected
                }
            }
            else
            {
                authFlag += "0"; //No TOU selected
            }




            if (selectedProfiles.Contains(ProfileId.BillingReset))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            //if (chk_CTRatio.Checked) authFlag += "1";
            //else authFlag += "0";
            // authFlag += "0";
            if (selectedProfiles.Contains(ProfileId.DIP))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.DisplayParametersIEC))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.BillingType))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }

            return authFlag;
        }

        public string GetWishListData()
        {
            try
            {
                string data1 = "";
                data1 = "00000";

                if (cmbBoxBillingPeriod.SelectedIndex == 0) { data1 = "0" + data1; }
                else if (cmbBoxBillingPeriod.SelectedIndex == 1) { data1 = "1" + data1; }

                if (evenMonthBilling.Checked) data1 = "00" + data1; //0
                else if (oddMonthBilling.Checked) data1 = "01" + data1; //1
                else if (monthlyBilling.Checked) data1 = "10" + data1; //2

                while (data1.Length < 8) data1 = "0" + data1;
                data1 = BinarytoHex(data1);

                string data2 = (Convert.ToInt32(cmbBoxBillingDate.Text)).ToString("X");
                if (data2.Length < 2) { data2 = "0" + data2; }
                //data2 = ReadoutCommon.StrToHexCmd(data2);

                string data3 = (Convert.ToInt32(cmbBoxBillingHour.Text)).ToString("X");
                if (data3.Length < 2) { data3 = "0" + data3; }
                //data3 = ReadoutCommon.StrToHexCmd(data3);

                string data4 = (Convert.ToInt32(cmbBoxBillingMinute.Text)).ToString("X");
                if (data4.Length < 2) { data4 = "0" + data4; }
                //data4 = ReadoutCommon.StrToHexCmd(data4);

                return data1 + data2 + data3 + data4;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetWishListData()", ex);
                return "";
            }
        }
        private string BinarytoHex(string binData)
        {
            try
            {
                string data = "";
                data = Convert.ToInt64(binData, 2).ToString("X");//binary to hex
                while (data.Length < 2) data = "0" + data;
                //data = ReadoutCommon.DTMStringToHex(data);
                return data;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BinarytoHex(string binData)", ex);
                return "";
            }
        }

        /// <summary>
        /// Used to write IEC Config data to CFG file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="selectedProfiles"></param>
        private void WriteIECConfigDataToCFGFile(StreamWriter writer, List<System.Enum> selectedProfiles)
        {

            try
            {
                string authFlag = string.Empty;
                this.Cursor = Cursors.WaitCursor;
                //selectedProfiles = GetSelectedProfileId("write");
                string AuthenticationFlag = GetAuthenticationFlag(selectedProfiles);
                writer.Write("(" + AuthenticationFlag + ")");

                //For CTratio
                if (selectedProfiles.Contains(ProfileId.DIP))
                {
                    if (cmbDIPDemandInterval.SelectedIndex == 0)
                    {
                        writer.Write("\r\n(0C)");
                    }
                    else if (cmbDIPDemandInterval.SelectedIndex == 1)
                    {
                        writer.Write("\r\n(2C)");
                    }
                    else
                    {
                        writer.Write("\r\n(00)");
                    }
                }
                else
                {
                    writer.Write("\r\n(00)");
                }


                if (selectedProfiles.Contains(ProfileId.BillingType))
                {
                    string data = GetWishListData();
                    writer.Write("\r\n(" + data + ")");
                }
                else
                {
                    writer.Write("\r\n(00)");
                }

                writer.Write("\r\n(0000)");


                //writer.Write("\r\n(00)");

                //For DTM Daily Profile

                // writer.Write("\r\n(00000000)");

                //For Daily Log
                // writer.Write("\r\n(0000)");



                //For TOU Log
                //if (rdbTOUWithHoliday.Checked)
                //{
                //    writer.Write("\r\n" + CreateTOUCommand());
                //}
                //else 



                if(AuthenticationFlag[2] == '1')
                {
                    List<string> tou1PNDLMS = GetTOUCommands_1P_NDLMS();
                    foreach (string item in tou1PNDLMS)
                    {
                        writer.Write("\r\n(" + item + ")");
                    }
                }
                else
                {
                    List<string> tou1PNDLMS = GetDummy_TOU_28Lines();
                    foreach (string item in tou1PNDLMS)
                    {
                        writer.Write("\r\n(" + item + ")");
                    }
                }

                //Display Parameter Non DLMS Present or Not
                if (selectedProfiles.Contains(ProfileId.DisplayParametersIEC))
                {
                    List<string> lstDisplayParameter = GetSelectedDisplayParameter_NDLMS();
                    foreach (string item in lstDisplayParameter)
                    {
                        writer.Write("\r\n(" + item + ")");
                    }
                }
                else
                {
                    List<string> lstDisplayParameter = new List<string>();
                    //Push Dummy 4 Lines
                    lstDisplayParameter.AddRange(GetDummy_DisplayParameter_4Lines());

                    //Scroll Dummy 4 Lines
                    lstDisplayParameter.AddRange(GetDummy_DisplayParameter_4Lines());

                    //High Dummy 4 Lines
                    lstDisplayParameter.AddRange(GetDummy_DisplayParameter_4Lines());

                    foreach (string item in lstDisplayParameter)
                    {
                        writer.Write("\r\n(" + item + ")");
                    }
                }

                if (!IsOffline)
                {
                    if (selectedProfiles.Contains(ProfileId.KvahSelection))
                    {
                        string lstKVAHSelection = GetKVAHSelection_NDLMS();
                        writer.Write("\r\n(" + lstKVAHSelection + ")");

                    }
                    else
                    {
                        //Push Dummy 1 Line
                        string lstKVAHSelection = GetDummy_kVAhSelection_1Line();
                        writer.Write("\r\n(" + lstKVAHSelection + ")");
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "Error occured while creating CFG file";
                logger.Log(LOGLEVELS.Error, "WriteIECConfigDataToCFGFile(StreamWriter writer, List<System.Enum> selectedProfiles)", ex);
            }
        }

        private List<string> GetDummy_DisplayParameter_4Lines()
        {
            List<string> dummy = new List<string>();
            dummy.Add("$");
            dummy.Add("$");
            dummy.Add("$");
            dummy.Add("$");
            return dummy;
        }


        private string GetDummy_kVAhSelection_1Line()
        {
            return "$";
        }

        private string GetKVAHSelection_NDLMS()
        {
            string result = GetKVAHSelection();
            return result;
        }

        private string GetKVAHSelection()
        {
            return Convert.ToInt32(rdbKVAhLagOnly.Checked).ToString();
        }

        private List<string> GetSelectedDisplayParameter_NDLMS()
        {
            List<String> result = new List<String>();
            List<String> push = objDisplayParameterIECConfig.GetPushButtonSelectedList();
            List<String> scroll = objDisplayParameterIECConfig.GetScrollButtonSelectedList();
            List<String> high = objDisplayParameterIECConfig.GetHighButtonSelectedList();

            //Add Push Parameters if selected
            if (push.Count == 4 && objDisplayParameterIECConfig.TotalPushCount > 0)
            {
                result.AddRange(push);
            }
            else
            {
                //not selected place dummy 4 lines
                result.AddRange(GetDummy_DisplayParameter_4Lines());
            }

            //Add Scroll Parameters if selected
            if (scroll.Count == 4 && objDisplayParameterIECConfig.TotalScrollCount > 0)
            {
                result.AddRange(scroll);
            }
            else
            {
                //not selected place dummy 4 lines
                result.AddRange(GetDummy_DisplayParameter_4Lines());
            }

            //Add high Parameters if selected
            if (high.Count == 4 && objDisplayParameterIECConfig.TotalHighCount > 0)
            {
                result.AddRange(high);
            }
            else
            {
                //not selected place dummy 4 lines
                result.AddRange(GetDummy_DisplayParameter_4Lines());
            }
            return result;
        }


        private List<string> GetDummy_TOU_28Lines()
        {
            List<string> lstDummy = new List<string>();
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            //Start Add Empty 4Lines
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            //End Add Empty 4Lines
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            //Start Add Empty 4Lines
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            //End Add Empty 4Lines
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            //Start Add Empty 4Lines
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            //End Add Empty 4Lines
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            lstDummy.Add("00000");
            //Start Add Empty 4Lines
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            lstDummy.Add("$");
            //End Add Empty 4Lines
            lstDummy.Add("00000000000");
            lstDummy.Add("00000000000");
            lstDummy.Add("00000000000");
            lstDummy.Add("00000000000");
            return lstDummy;

        }

        private List<string> GetValuesFromDayTableGrid(string GridName)
        {
            List<string> lstTOUTable = new List<string>();
            Control[] arrControl = this.Controls.Find(GridName, true);
            if (arrControl != null)
            {
                if (arrControl.Length > 0)
                {
                    DataGridView dgv = arrControl[0] as DataGridView;
                    if (dgv != null)
                    {
                        int indexRow = 0;
                        foreach (DataGridViewRow itemRow in dgv.Rows)
                        {
                            string Data = string.Empty;
                            string CellData = Convert.ToString(itemRow.Cells["Tariff"].Value).Trim();

                            if (CellData == string.Empty)
                            {
                                Data = "00000";
                            }
                            else
                            {
                                Data = CellData.Substring(1, 1) + Convert.ToString(itemRow.Cells["Hours"].Value).Trim() + Convert.ToString(itemRow.Cells["Minutes"].Value).Trim();
                            }
                            lstTOUTable.Add(Data);
                            indexRow++;
                        }
                    }
                }
            }
            return lstTOUTable;
        }

        private List<string> AddRangeValue(List<string> TempLstCommands)
        {
            for (int i = TempLstCommands.Count; i < 10; i++)
            {
                TempLstCommands.Add("$");
            }
            return TempLstCommands;
        }

        private List<string> GetTOUCommands_1P_NDLMS()
        {
            List<string> touCommands = new List<string>();
            try
            {

                //DayTable Entry
                for (int index = 1; index < 6; index++)
                {
                    switch (index)
                    {
                        case 1:
                            {
                                List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay1_1P_NDLMS.Name);
                                TempLstCommands = AddRangeValue(TempLstCommands);
                                touCommands.AddRange(TempLstCommands);
                            }
                            break;
                        case 2:
                            {
                                List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay2_1P_NDLMS.Name);
                                TempLstCommands = AddRangeValue(TempLstCommands);
                                touCommands.AddRange(TempLstCommands);
                            }
                            break;
                        case 3:
                            {
                                List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay3_1P_NDLMS.Name);
                                TempLstCommands = AddRangeValue(TempLstCommands);
                                touCommands.AddRange(TempLstCommands);
                            }
                            break;
                        case 4:
                            {
                                List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay4_1P_NDLMS.Name);
                                TempLstCommands = AddRangeValue(TempLstCommands);
                                touCommands.AddRange(TempLstCommands);
                            }
                            break;
                        case 5:
                            {
                                //Week Table Entry
                                List<string> TempLstCommands = GetValuesFromWeekTableGrid();
                                touCommands.AddRange(TempLstCommands);
                            }
                            break;
                        default:
                            {

                            }
                            break;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTOUCommands_1P_NDLMS()", ex);
            }
            return touCommands;
        }

        private List<string> GetTOUCommands_SP_NDLMS()
        {
            //
            /* 0x01   W   4   0x02  D     4    0   INDEX  (   DATA   )  {0x03 OR 0x04} BCC */
            //CHAR DISPLAY
            /* 0x01 0x57 0x34 0x02 0x44  0x34 0x30 INDEX 0x28 DATA 0x29 {0x03 OR 0x04} BCC */
            //HEX DISPLAY

            /* 0x01   W   4   0x02  (   DATA   )  {0x03 OR 0x04} BCC */
            //CHAR DISPLAY
            /* 0x01 0x57 0x34 0x02 0x28 DATA 0x29 {0x03 OR 0x04} BCC */
            //HEX DISPLAY

            List<string> touCommands = new List<string>();
            string HeaderText1 = "01573402";
            string HeaderText2 = "443430";
            string Header31 = "31";
            string Header32 = "32";
            string Header33 = "33";
            string Header34 = "34";
            string Header30 = "30";
            string HeaderStartBracket = "28";
            string HeaderEndBracket = "29";
            string HeaderResponseComplete = "03";
            string HeaderResponseRemaining = "04";



            //DayTable Entry
            for (int index = 1; index < 6; index++)
            {
                switch (index)
                {
                    case 1:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay1_1P_NDLMS.Name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header31;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;
                    case 2:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay2_1P_NDLMS.Name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header32;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 3:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay3_1P_NDLMS.Name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header33;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 4:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay4_1P_NDLMS.Name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header34;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 5:
                        {
                            //Week Table Entry
                            List<string> TempLstCommands = GetValuesFromWeekTableGrid();
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header30;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    default:
                        {

                        }
                        break;
                }
            }
            return touCommands;
        }



        private List<string> GetValuesFromWeekTableGrid()
        {
            List<string> lstTOUTable = new List<string>();
            int indexRow = 0;
            foreach (DataGridViewRow itemRow in gridDayTables_1P_NDLMS.Rows)
            {
                string Data = string.Empty;
                if (Convert.ToString(itemRow.Cells["Mon"].Value) == string.Empty)
                {
                    Data = "00000000000";
                }
                else
                {
                    Data = Convert.ToString(itemRow.Cells["Mon"].Value) +
                            Convert.ToString(itemRow.Cells["Tue"].Value) +
                            Convert.ToString(itemRow.Cells["Wed"].Value) +
                            Convert.ToString(itemRow.Cells["Thu"].Value) +
                            Convert.ToString(itemRow.Cells["Fri"].Value) +
                            Convert.ToString(itemRow.Cells["Sat"].Value) +
                            Convert.ToString(itemRow.Cells["Sun"].Value) +
                            Convert.ToString(gridActivationDate_1P_NDLMS.Rows[indexRow].Cells[0].Value) +
                            Convert.ToString(gridActivationDate_1P_NDLMS.Rows[indexRow].Cells[1].Value);
                }
                lstTOUTable.Add(Data);
                indexRow++;
            }
            return lstTOUTable;
        }




        /// <summary>
        /// Display IEc TOU from CFG File .
        /// Code copied from IEC
        /// </summary>
        /// <param name="touData"></param>
        private void DisplayTOU(string touData)
        {
            int tableIndex = 0;
            int rowIndex = 0;
            DataSet ds = new DataSet();

            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();
            DataGridView[] dayAssignmentGrids = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();

            ds = ProgrammingCommon.DisplayTOUData(touData, "Current");//2 march 2012: tag changed from "Future" to "Current" to display current TOU 
            if (ds == null)
            {
                this.StatusMessage = "Invalid TOU";
                return;
            }

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Rows.Clear();
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    seasonGrid.Rows.Add();
                    seasonGrid.Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                tableIndex++;
            }

            for (int holidayIndex = 0; holidayIndex <= holidayGrids.GetUpperBound(0); holidayIndex++)
            {
                holidayGrids[holidayIndex].Rows.Clear();
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    holidayGrids[holidayIndex].Rows.Add();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                {
                    DateTime dt;
                    if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                        if (DateTime.TryParse(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString(), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                        {
                            dtPickerCollection[holidayIndex].Value = dt;
                            dtPickerCollection[holidayIndex].CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                        }
                }
                tableIndex++;
            }
            if (!string.IsNullOrEmpty(ProgrammingCommon.futureActivationDate))
            {
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                dtPickerFutureActivationDate.Value = ProgrammingCommon.GetDate(ProgrammingCommon.futureActivationDate, true);
            }
            for (int dayAssignment = 0; dayAssignment <= dayAssignmentGrids.GetUpperBound(0); dayAssignment++)
            {
                rowIndex = 0;
                dayAssignmentGrids[dayAssignment].Rows.Clear();
                foreach (DataRow row in ds.Tables[tableIndex].Rows)
                {
                    dayAssignmentGrids[dayAssignment].Rows.Add();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[0].Value = row["Day"].ToString();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[1].Value = row["Day Table"].ToString();
                    rowIndex++;
                }
                tableIndex++;
            }

            rowIndex = 0;
            gridActivation.Rows.Clear();
            dtPickerFutureActivationDate.Value = Convert.ToDateTime(ds.Tables[tableIndex].Rows[0]["Season Activation Date"].ToString(), new CultureInfo("en-GB"));

            foreach (DataRow row in ds.Tables[tableIndex].Rows)
            {
                gridActivation.Rows.Add();
                gridActivation.Rows[rowIndex].Cells[0].Value = row["Season Activation Date"].ToString();
                gridActivation.Rows[rowIndex].Cells[1].Value = Convert.ToInt32(row["Season Number"].ToString()).ToString();
                rowIndex++;
            }

        }

        /// <summary>
        /// Code Copied from IEC.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetTOUFileContent(string inputFileContent)
        {
            string fileContent = string.Empty;
            int index = 0;
            int intsec = 34;
            int intb = 0;
            string strTemp = string.Empty;
            string[] readData = new string[40];
            string[] finalData = new string[40];

            if (inputFileContent.Length <= 0)
                return string.Empty;

            using (StringReader sr = new StringReader(inputFileContent))
            {
                while (sr.Peek() >= 0)
                {
                    readData[index++] = sr.ReadLine();
                }
            }
            index = 0;

            while (intb < 38)
            {
                if (intb == 6 || intb == 13 || intb == 20 || intb == 27)
                {
                    finalData[intb] = readData[intsec];
                    intsec++;
                }
                else
                {
                    finalData[intb] = readData[index];
                    index++;
                }
                intb++;
                if (intb == 38)
                    finalData[intb] = readData[intb];

            }
            index = 0;
            while (index < 39)
            {
                strTemp = finalData[index];
                strTemp = strTemp.Replace("28", "(");
                strTemp = strTemp.Replace("29", ")");
                strTemp = strTemp.Replace(" ", "");
                strTemp = strTemp.Substring(strTemp.IndexOf("("), ((strTemp.IndexOf(")") + 1) - strTemp.IndexOf("(")));
                intb = 1;
                fileContent += "(";
                while (intb < strTemp.Length - 1)
                {
                    string tempcontent = (Convert.ToInt16(strTemp.Substring(intb, 2)) - 30).ToString();
                    fileContent += tempcontent;
                    intb += 2;
                }
                fileContent += ")";
                index++;
            }
            return fileContent;
        }
        ///// <summary>
        ///// Create IEC TOU Command . COde ported form IEC Code base
        ///// </summary>
        ///// <returns></returns>
        private string CreateTOUCommand()
        {
            int touIndex = 0, touSeason = 0, touSeasonDay = 0, touHoliday = 0, touDateActvation = 0;
            List<string> touFileContent = new List<string>();
            string fileContent = string.Empty;

            touFileContent = GetTOUCommands();
            if (touFileContent[0].Trim().Length == 0)
                return string.Empty;

            touSeason = 1;
            while (touSeason <= 4)  //Season Day Table
            {
                touSeasonDay = 1;
                while (touSeasonDay <= 6)
                {
                    fileContent += "<S" + touSeason.ToString() + "D" + touSeasonDay.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</S" + touSeason.ToString() + "D" + touSeasonDay.ToString() + ">" + "\r\n";
                    touSeasonDay++;
                    touIndex++;
                }
                touSeason++;
            }

            touHoliday = 1;
            while (touHoliday <= 10)    //Holiday
            {
                fileContent += "<HD" + touHoliday.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</HD" + touHoliday.ToString() + ">" + "\r\n";
                touHoliday++;
                touIndex++;

            }

            touDateActvation = 1;
            while (touDateActvation <= 4)   //Activation Date
            {
                fileContent += "<SD" + touDateActvation.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</SD" + touDateActvation.ToString() + ">" + "\r\n";
                touDateActvation++;
                touIndex++;

            }

            /*Future Activation date*/
            fileContent += "<FSAD>" + CreateTOUString(touFileContent[touIndex]) + "</FSAD>";
            return fileContent;
        }
        ///// <summary>
        ///// Code ported from IEC
        ///// </summary>
        ///// <param name="cmdTou"></param>
        ///// <returns></returns>
        private string CreateTOUString(string cmdTou)
        {
            int cmdLength = 0;
            string strRes = string.Empty;
            while (cmdLength < cmdTou.Length)
            {
                if (cmdLength == cmdTou.Length - 2)
                    strRes += cmdTou.Substring(cmdLength, 2);
                else
                    strRes += cmdTou.Substring(cmdLength, 2) + "\x20";
                cmdLength += 2;
            }
            return strRes;
        }
        ///// <summary>
        ///// Used to get TOU commands .Code ported from IEC
        ///// </summary>
        ///// <returns></returns>
        private List<string> GetTOUCommands()
        {
            List<string> touCommands = new List<string>();
            string touAddress = string.Empty;
            string touCommand = string.Empty;
            int slots = 0, gridIndex = 0;
            string dayTable = string.Empty;
            string holidayActivationDate = string.Empty;

            int[] touMemoryAddress = new int[] 
            {
                784, 785, 786, 787, 788, 789, 800, 801, 802, 803, 804, 805, 816,
                817, 818, 819, 820, 821, 832, 833, 834, 835, 836, 837, 848, 849,    //TOU address in meter memory 
                850, 851, 852, 853, 854, 855, 856, 857, 790, 806, 822, 838, 864 
            };

            DataGridView[] gridSeason = GetSeasonGridCollection();
            DataGridView[] gridHoliday = GetHolidayGridCollection();
            DataGridView[] gridDayAssignment = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();


            for (int seasonIndex = 0; seasonIndex <= gridSeason.GetUpperBound(0); seasonIndex++)
            {
                touAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                touCommand = string.Empty;

                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridSeason[seasonIndex].Rows)
                {
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((seasonIndex % 6 + 1 % 7).ToString("d2"));
                touCommands.Add("01573102" + touAddress + "28" + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + touCommand + "29" + "03");
            }

            for (int holidayIndex = 0; holidayIndex <= gridHoliday.GetUpperBound(0); holidayIndex++)
            {
                touAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                touCommand = string.Empty;

                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridHoliday[holidayIndex].Rows)
                {
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((holidayIndex % 6 + 1 % 7).ToString("d2"));
                //holidayActivationDate = ProgrammingCommon.GetASCIIValue(dtPickerCollection[holidayIndex].Value.Date.ToShortDateString().Replace(ConfigInfo.DateFormat().Substring(2, 1), "")); 
                ////
                //holidayActivationDate = DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString();
                holidayActivationDate = ProgrammingCommon.GetASCIIValue(DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString().Substring(2, 6));
                /////
                touCommands.Add("01573102" + touAddress + "28" + holidayActivationDate + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + touCommand + "29" + "03");
            }

            for (int dayAssignment = 0; dayAssignment <= gridDayAssignment.GetUpperBound(0); dayAssignment++)
            {
                touCommand = string.Empty;
                touAddress = string.Empty;
                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));

                foreach (DataGridViewRow row in gridDayAssignment[dayAssignment].Rows)
                {
                    string tempStr = row.Cells[1].Value.ToString();
                    touCommand += ProgrammingCommon.GetASCIIValue(tempStr.Replace("Day Table ", "0").Trim());
                }
                touCommands.Add("01573102" + touAddress + "28" + touCommand + "29" + "03");
            }

            touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
            touCommand = string.Empty;
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Day.ToString("d2"));
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Month.ToString("d2"));
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Year.ToString().Substring(2));

            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string tempCommand = "";
                DateTime dateTime = ProgrammingCommon.GetDate(Convert.ToDateTime(row.Cells[0].Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), true);
                tempCommand = String.Format("{0:00}", dateTime.Day.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", dateTime.Month.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", Convert.ToInt16(row.Cells["SeasonNumber"].Value.ToString()));
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
            }
            touCommands.Add("01573102" + touAddress + "28" + touCommand + "29" + "03");
            return touCommands;

        }
        ///// <summary>
        ///// Code ported from IEC
        ///// </summary>
        ///// <returns></returns>
        private DataGridView[] GetSeasonGridCollection()
        {
            DataGridView[] seasonGrids = new DataGridView[] 
            {
                gridS1Day1, gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6,
                gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, 
                gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6,
                gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6                
            };
            return seasonGrids;
        }
        ///// <summary>
        ///// Code ported from IEC
        ///// </summary>
        ///// <returns></returns>
        private DataGridView[] GetHolidayGridCollection()
        {
            DataGridView[] holidayGrids = new DataGridView[] 
            {
                gridHoliday1,gridHoliday2, gridHoliday3, gridHoliday4, gridHoliday5,
                gridHoliday6, gridHoliday7, gridHoliday8, gridHoliday9, gridHoliday10
            };
            return holidayGrids;
        }
        ///// <summary>
        ///// Code ported from IEC
        ///// </summary>
        ///// <returns></returns>
        private DataGridView[] GetAssignmentGridCollection()
        {
            DataGridView[] dayAssignmentGrids = new DataGridView[] 
            {
                gridAssignmentS1, gridAssignmentS2, gridAssignmentS3, gridAssignmentS4
            };
            return dayAssignmentGrids;
        }
        ///// <summary>
        ///// Code ported from IEC
        ///// </summary>
        ///// <returns></returns>
        private DateTimePicker[] GetActivationDateCollection()
        {
            DateTimePicker[] holidayActivationDates = new DateTimePicker[]
            {
                dtPicAcDate1, dtPicAcDate2, dtPicAcDate3, dtPicAcDate4, dtPicAcDate5,
                dtPicAcDate6, dtPicAcDate7, dtPicAcDate8, dtPicAcDate9, dtPicAcDate10 
            };
            return holidayActivationDates;
        }
        /// <summary>
        /// Used to initialize TOU grids. Code ported from IEC Code base.
        /// </summary>
        private void SetTOUGrids()
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Columns.Clear();
                seasonGrid.Columns.Add(GetSNo());
                seasonGrid.Columns.Add(GetRates());
                seasonGrid.Columns.Add(GetStartHour());
                seasonGrid.Columns.Add(GetStartMinute());
                seasonGrid.Columns[0].ReadOnly = true;
            }
            foreach (DataGridView holidayGrid in holidayGrids)
            {
                holidayGrid.Columns.Clear();
                holidayGrid.Columns.Add(GetSNo());
                holidayGrid.Columns.Add(GetRates());
                holidayGrid.Columns.Add(GetStartHour());
                holidayGrid.Columns.Add(GetStartMinute());
                holidayGrid.Columns[0].ReadOnly = true;
            }
        }
        /// <summary>
        /// Code ported form IEC
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetSNo()
        {
            DataGridViewComboBoxColumn colSNo = new DataGridViewComboBoxColumn();
            colSNo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colSNo.Name = "SNo.";
            colSNo.HeaderText = "SNo.";
            colSNo.Items.Add("1");
            colSNo.Items.Add("2");
            colSNo.Items.Add("3");
            colSNo.Items.Add("4");
            colSNo.Items.Add("5");
            colSNo.Items.Add("6");
            colSNo.Items.Add("7");
            colSNo.Items.Add("8");
            colSNo.Items.Add("9");
            colSNo.Items.Add("10");
            return colSNo;
        }
        /// <summary>
        /// Code ported from IEC
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetRates()
        {
            DataGridViewComboBoxColumn colRate = new DataGridViewComboBoxColumn();
            colRate.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colRate.Name = "Rate";
            colRate.HeaderText = "Rate";
            colRate.Items.Add("T1");
            colRate.Items.Add("T2");
            colRate.Items.Add("T3");
            colRate.Items.Add("T4");
            colRate.Items.Add("T5");
            colRate.Items.Add("T6");
            colRate.Items.Add("T7");
            colRate.Items.Add("T8");
            colRate.Items.Add("00");
            return colRate;
        }
        /// <summary>
        /// Code Ported from IEC
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetStartHour()
        {
            DataGridViewComboBoxColumn colStartHour = new DataGridViewComboBoxColumn();
            colStartHour.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartHour.Name = "Start Hour";
            colStartHour.HeaderText = "Start Hour";
            colStartHour.Items.Add("00");
            colStartHour.Items.Add("01");
            colStartHour.Items.Add("02");
            colStartHour.Items.Add("03");
            colStartHour.Items.Add("04");
            colStartHour.Items.Add("05");
            colStartHour.Items.Add("06");
            colStartHour.Items.Add("07");
            colStartHour.Items.Add("08");
            colStartHour.Items.Add("09");
            colStartHour.Items.Add("10");
            colStartHour.Items.Add("11");
            colStartHour.Items.Add("12");
            colStartHour.Items.Add("13");
            colStartHour.Items.Add("14");
            colStartHour.Items.Add("15");
            colStartHour.Items.Add("16");
            colStartHour.Items.Add("17");
            colStartHour.Items.Add("18");
            colStartHour.Items.Add("19");
            colStartHour.Items.Add("20");
            colStartHour.Items.Add("21");
            colStartHour.Items.Add("22");
            colStartHour.Items.Add("23");
            return colStartHour;
        }
        /// <summary>
        /// Code ported from IEC
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn GetStartMinute()
        {
            DataGridViewComboBoxColumn colStartMinute = new DataGridViewComboBoxColumn();
            colStartMinute.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartMinute.Name = "Start Minute";
            colStartMinute.HeaderText = "Start Minute";
            colStartMinute.Items.Add("00");
            colStartMinute.Items.Add("15");
            colStartMinute.Items.Add("30");
            colStartMinute.Items.Add("45");
            return colStartMinute;
        }
        /// <summary>
        /// Initialize all grids . Code Copied from IEc
        /// </summary>
        private void ResetAllGrids()
        {
            ResetSeasonGrids();
            ResetHolidayGrids();
            ResetDayAssignmentGrids();
            ResetFutureActivationGrid();
        }
        /// <summary>
        /// Initialize Season Grids . Code Copied from IEc
        /// </summary>
        private void ResetSeasonGrids()
        {
            int rowCount = 0;
            foreach (DataGridView seasonGrid in GetSeasonGridCollection())
            {
                rowCount = 0;
                if (seasonGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        seasonGrid.Rows.Add();
                        seasonGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }

                foreach (DataGridViewRow row in seasonGrid.Rows)
                {
                    row.Cells["Start Hour"].Value = "00";
                    row.Cells["Start Minute"].Value = "00";
                    row.Cells["Rate"].Value = "00";
                }
            }
        }
        /// <summary>
        /// Initialize Holiday Grids . Code Copied from IEC
        /// </summary>
        private void ResetHolidayGrids()
        {
            int rowCount = 0;
            foreach (DataGridView holidayGrid in GetHolidayGridCollection())
            {
                rowCount = 0;
                if (holidayGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        holidayGrid.Rows.Add();
                        holidayGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }
                foreach (DataGridViewRow row in holidayGrid.Rows)
                {
                    row.Cells["Start Hour"].Value = "00";
                    row.Cells["Start Minute"].Value = "00";
                    row.Cells["Rate"].Value = "00";
                }
            }

            foreach (DateTimePicker dtp in GetActivationDateCollection())
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                dtp.Value = System.DateTime.Now;
            }
        }
        /// <summary>
        /// Initialize DayAssignment Grids . Code Copied from IEC
        /// </summary>
        private void ResetDayAssignmentGrids()
        {
            foreach (DataGridView assignmentGrid in GetAssignmentGridCollection())
            {
                if (assignmentGrid.Rows.Count == 0)
                {
                    assignmentGrid.Rows.Add(7);
                    assignmentGrid.Rows[0].Cells[0].Value = "Sunday";
                    assignmentGrid.Rows[1].Cells[0].Value = "Monday";
                    assignmentGrid.Rows[2].Cells[0].Value = "Tuesday";
                    assignmentGrid.Rows[3].Cells[0].Value = "Wednesday";
                    assignmentGrid.Rows[4].Cells[0].Value = "Thursday";
                    assignmentGrid.Rows[5].Cells[0].Value = "Friday";
                    assignmentGrid.Rows[6].Cells[0].Value = "Saturday";
                }
                foreach (DataGridViewRow row in assignmentGrid.Rows)
                {
                    row.Cells[1].Value = "Day Table 1";
                    row.Cells[1].Value = "Day Table 1";
                }
            }
        }
        /// <summary>
        /// Initialize FutureActivation Grids . Code Copied from IEC
        /// </summary>
        private void ResetFutureActivationGrid()
        {
            double dayIndex = 0;
            dtPickerFutureActivationDate.Format = DateTimePickerFormat.Custom;
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
            dtPickerFutureActivationDate.Value = System.DateTime.Now.AddDays(++dayIndex);

            if (gridActivation.Rows.Count == 0)
                gridActivation.Rows.Add(4);
            int rIndex = 0;
            int startDay = 1;
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string sDay = startDay.ToString();
                if (sDay.Length < 2) { sDay = "0" + sDay; }
                DateTime dt = new DateTime(DateTime.Now.Year, 1, Int32.Parse(sDay));
                row.Cells[0].Value = dt;// Convert.ToDateTime(sDay + "/01/" + DateTime.Now.Year.ToString());
                startDay++;
                //row.Cells[0].Value = System.DateTime.Now.AddDays(++dayIndex);
                row.Cells[1].Value = "1";
            }
        }
        /// <summary>
        /// Calls when cell click of TOU grids 
        /// Code Copied from IEC Code Base .
        /// </summary>
        /// <param name="dataGrid"></param>
        private void GridCellClick(DataGridView dataGrid)
        {
            if (dataGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                int colIndex = dataGrid.CurrentCell.ColumnIndex;
                int rowIndex = dataGrid.CurrentCell.RowIndex;
                dataGrid.Rows[rowIndex].Cells[colIndex] = comboCell;

                if (dataGrid.CurrentCell.ColumnIndex == 1)
                {
                    comboCell.Items.Add("T1");
                    comboCell.Items.Add("T2");
                    comboCell.Items.Add("T3");
                    comboCell.Items.Add("T4");
                    comboCell.Items.Add("T5");
                    comboCell.Items.Add("T6");
                    comboCell.Items.Add("T7");
                    comboCell.Items.Add("T8");
                    comboCell.Items.Add("00");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 2)
                {
                    comboCell.Items.Add("00");
                    comboCell.Items.Add("01");
                    comboCell.Items.Add("02");
                    comboCell.Items.Add("03");
                    comboCell.Items.Add("04");
                    comboCell.Items.Add("05");
                    comboCell.Items.Add("06");
                    comboCell.Items.Add("07");
                    comboCell.Items.Add("08");
                    comboCell.Items.Add("09");
                    comboCell.Items.Add("10");
                    comboCell.Items.Add("11");
                    comboCell.Items.Add("12");
                    comboCell.Items.Add("13");
                    comboCell.Items.Add("14");
                    comboCell.Items.Add("15");
                    comboCell.Items.Add("16");
                    comboCell.Items.Add("17");
                    comboCell.Items.Add("18");
                    comboCell.Items.Add("19");
                    comboCell.Items.Add("20");
                    comboCell.Items.Add("21");
                    comboCell.Items.Add("22");
                    comboCell.Items.Add("23");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 3)
                {
                    comboCell.Items.Add("00");
                    comboCell.Items.Add("15");
                    comboCell.Items.Add("30");
                    comboCell.Items.Add("45");
                }
            }
            int rIndex = dataGrid.CurrentCell.RowIndex;
            int count = 0;
            if (rIndex != 0)
            {

                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "23" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "45")
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = true;
                        dataGrid.Rows[count].Cells[2].ReadOnly = true;
                        dataGrid.Rows[count].Cells[3].ReadOnly = true;
                    }
                    this.StatusMessage = "No more entries allowed as the day is complete";
                    isValidTOU = true;
                    return;
                }
                else
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = false;
                        dataGrid.Rows[count].Cells[2].ReadOnly = false;
                        dataGrid.Rows[count].Cells[3].ReadOnly = false;
                    }
                    this.StatusMessage = " ";
                    isValidTOU = true;
                }

                int grdRowIndex = 0;
                for (grdRowIndex = 1; grdRowIndex < 9; grdRowIndex++) //changed on 11/01/2011 grdRowIndex <= 9 changed to grdRowIndex < 9 
                {
                    if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[2].Value) == "00")
                        {
                            if ((Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) != "00") || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == 45) || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[grdRowIndex].Cells[3].Value)))
                            {
                                dataGrid.Rows[grdRowIndex].Cells[3].ReadOnly = true;
                                do
                                {
                                    dataGrid.Rows[grdRowIndex + 1].Cells[1].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[2].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[3].ReadOnly = true;
                                    grdRowIndex++;
                                } while (grdRowIndex != 9);
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 7; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true; // this line added on  11/01/2011
                            do
                            {
                                dataGrid.Rows[rIndex + 2].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 8);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 8; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex - 1].Cells[3].ReadOnly = true;
                            do
                            {
                                dataGrid.Rows[rIndex + 1].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 9);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex > 1)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                    {
                        string val = Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value);
                        string val1 = Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[1].Value);
                        if (val1 != "00" && val != "00")
                        {
                            do
                            {
                                dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 10);
                            isValidTOU = false;
                            return;
                        }
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    //isValidTOU = false;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    if (rIndex != 1 && (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "00"))
                    {
                        do
                        {
                            dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                            rIndex++;
                        } while (rIndex != 10);
                        isValidTOU = false;
                        return;
                    }
                    else
                    {
                        dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = true;
                    }
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    //isValidTOU = false;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00" && Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == "00")
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) != "00" || (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "45"))// && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == "00")//added on june 10
                    {
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = false;
                        return;
                    }
                }
                //else
                //{ 
                //   dataGrid.Rows[rIndex].Cells[3].ReadOnly = false ;
                //    isValidTOU = true;
                //}

                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 9)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value))
                        {
                            if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                            {
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                    if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 0)
                {
                    if (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }

            }
        }

        /// <summary>
        /// Validates TOU Data
        /// Code Copied from IEC CodeBase 
        /// </summary>
        /// <param name="dtView"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool ValidateGridCell(DataGridView dtView, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "T1") && (e.FormattedValue.ToString() != "T2") && (e.FormattedValue.ToString() != "T3") && (e.FormattedValue.ToString() != "T4") && (e.FormattedValue.ToString() != "T5") && (e.FormattedValue.ToString() != "T6") && (e.FormattedValue.ToString() != "T7") && (e.FormattedValue.ToString() != "T8"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";

                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                            dtView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                            dtView.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        }
                        //if (e.RowIndex != 0)
                        //{
                        //    if (e.FormattedValue.ToString() != "00" && (Convert.ToString(dtView.Rows[e.RowIndex].Cells[2].Value) == "00" && Convert.ToString(dtView.Rows[e.RowIndex].Cells[3].Value) == "00"))
                        //    {
                        //        e.Cancel = true;
                        //        isValidTOU = false;
                        //        return false;
                        //    }
                        //}
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else isValidTOU = true;
                        if (e.FormattedValue.ToString() != "" && (Convert.ToInt16(e.FormattedValue.ToString()) > 23))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }
                        if (e.RowIndex != 0)//added on june 10
                        {
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "45")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }

                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                        }
                        else isValidTOU = true;

                        //Shivangy included the condition on 26 May 2009
                        if (e.RowIndex != 0 && e.RowIndex != 9)
                        {

                            if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString() != "00")
                            {
                                if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;


                                if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value) || Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                        e.Cancel = true;
                                        isValidTOU = false;
                                    }
                                    else { isValidTOU = true; }
                                }
                            }

                            //*********

                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else isValidTOU = true;

                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() == "00" && dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "45")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = false;
                                    return false;
                                }
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    int currIndex = e.RowIndex;
                                    int rIndex = e.RowIndex + 1;
                                    if (rIndex < 10)
                                    {
                                        do
                                        {
                                            dtView.Rows[rIndex].Cells[1].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[2].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[3].ReadOnly = true;
                                            rIndex++;
                                        } while (rIndex != 10);
                                        isValidTOU = false;
                                        return false;
                                    }
                                }
                                else { isValidTOU = true; }
                            }
                        }
                        if (e.RowIndex == 9)
                        {
                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[2].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[2].Value) && Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[3].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[3].Value))
                            {
                                isValidTOU = false;
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "00") && (e.FormattedValue.ToString() != "15") && (e.FormattedValue.ToString() != "30") && (e.FormattedValue.ToString() != "45"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }

                        if (e.FormattedValue.ToString() != "" && e.RowIndex > 0)
                        {
                            int index = e.RowIndex;
                            if (index != 9)
                            {
                                while (index != 10)//Added on March 13
                                {
                                    if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 2].Value.ToString() != "00")
                                    {
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        else
                                        {
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                        }
                                    }
                                    index++;
                                }
                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                int rIndex = e.RowIndex + 1;
                                do
                                {
                                    dtView.Rows[rIndex].Cells[0].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[1].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[2].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[3].ReadOnly = false;
                                    rIndex++;
                                } while (rIndex != 10);
                                rIndex--;
                                this.StatusMessage = "";
                                isValidTOU = false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                dtView.Rows[e.RowIndex].ErrorText = "Invalid Value ";
                e.Cancel = true;
                isValidTOU = false;
                logger.Log(LOGLEVELS.Error, "ValidateGridCell(DataGridView dtView, DataGridViewCellValidatingEventArgs e)", ex);
                return false;
            }
        }
        /// <summary>
        /// Cpde Copied from IEC
        /// </summary>
        /// <param name="AssignGrid"></param>
        private void GridAssignClick(DataGridView AssignGrid)
        {
            if (AssignGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                if (AssignGrid.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell dtComboCell = new DataGridViewComboBoxCell();
                    dtComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    dtComboCell.Items.Add("Day Table 1");
                    dtComboCell.Items.Add("Day Table 2");
                    dtComboCell.Items.Add("Day Table 3");
                    dtComboCell.Items.Add("Day Table 4");
                    dtComboCell.Items.Add("Day Table 5");
                    dtComboCell.Items.Add("Day Table 6");
                    int rIndex = AssignGrid.CurrentCell.RowIndex;
                    int cIndex = AssignGrid.CurrentCell.ColumnIndex;
                    AssignGrid.Rows[rIndex].Cells[cIndex] = dtComboCell;
                }
            }
        }
        /// <summary>
        /// Code copied from IEC code base 
        /// </summary>
        /// <param name="AssignGrid"></param>
        /// <param name="e"></param>
        private void GridAssignValidate(DataGridView AssignGrid, DataGridViewCellValidatingEventArgs e)
        {
            if (AssignGrid[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string gridVal = e.FormattedValue.ToString();
                        if (gridVal == "")
                        {
                            AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            if (gridVal != "Day Table 1" && gridVal != "Day Table 2" && gridVal != "Day Table 3" && gridVal != "Day Table 4" && gridVal != "Day Table 5" && gridVal != "Day Table 6")
                            {
                                AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else
                            {
                                isValidTOU = true;
                                AssignGrid.Rows[e.RowIndex].ErrorText = "";
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Code copied from ICE Code Base.
        /// </summary>
        private void GridAcCellClick()
        {
            if (gridActivation.CurrentCell.Style.ForeColor == Color.Red)
            {
                gridActivation.CurrentCell.Style.ForeColor = Color.Black;
                if (gridActivation.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                    comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    comboCell.Items.Add("1");
                    comboCell.Items.Add("2");
                    comboCell.Items.Add("3");
                    comboCell.Items.Add("4");
                    int rIndex = gridActivation.CurrentCell.RowIndex;
                    gridActivation.Rows[rIndex].Cells[1] = comboCell;
                }
            }
        }


        #endregion
        /// <summary>
        /// choosing Normal or er radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalBillingType_CheckedChanged(object sender, EventArgs e)
        {
            if (normalBillingType.Checked)
            {
                billingPeriodPanel.Visible = false;
                label23.Visible = true;
                label24.Visible = true;
                cmbBoxBillingHour.Visible = true;
                cmbBoxBillingMinute.Visible = true;
            }
            if (otherBillingType.Checked)
            {
                billingPeriodPanel.Visible = true;
                label23.Visible = false;
                label24.Visible = false;
                cmbBoxBillingHour.Visible = false;
                cmbBoxBillingMinute.Visible = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSinglePhaseNdlmsTouValues(TOUZone,TOUSlots);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnReset_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnFillAuto_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridTOUDay1_1P_NDLMS != null && gridTOUDay2_1P_NDLMS != null && gridTOUDay3_1P_NDLMS != null && gridTOUDay4_1P_NDLMS != null && gridDayTables_1P_NDLMS != null && gridActivationDate_1P_NDLMS != null)
                {
                    FillDefaultGridValues(gridTOUDay2_1P_NDLMS,TOUZone,TOUSlots);
                    FillDefaultGridValues(gridTOUDay3_1P_NDLMS, TOUZone, TOUSlots);
                    FillDefaultGridValues(gridTOUDay4_1P_NDLMS, TOUZone, TOUSlots);
                    FillDefaultGridValues(gridDayTables_1P_NDLMS, TOUZone, TOUSlots);
                    FillDefaultGridValues(gridActivationDate_1P_NDLMS, TOUZone, TOUSlots);


                    if (Convert.ToString(gridTOUDay1_1P_NDLMS.Rows[0].Cells[1].Value) != string.Empty)
                    {
                        int index = 0;
                        foreach (DataGridViewRow itemRow in gridTOUDay1_1P_NDLMS.Rows)
                        {
                            int indexClm = 0;
                            foreach (DataGridViewColumn itemColumn in gridTOUDay1_1P_NDLMS.Columns)
                            {
                                if (indexClm != 0)
                                {
                                    gridTOUDay2_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value;
                                    gridTOUDay3_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value;
                                    gridTOUDay4_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1_1P_NDLMS.Rows[index].Cells[itemColumn.Name].Value;
                                }
                                indexClm++;
                            }

                            index++;
                        }

                        //gridDayTables_1P_NDLMS.Rows.Add();
                        int indexColumn = 0;
                        foreach (DataGridViewColumn itemColumn in gridDayTables_1P_NDLMS.Columns)
                        {
                            if (indexColumn != 0)
                            {
                                gridDayTables_1P_NDLMS.Rows[0].Cells[itemColumn.Name].Value = "1";
                                gridDayTables_1P_NDLMS.Rows[1].Cells[itemColumn.Name].Value = "2";
                                gridDayTables_1P_NDLMS.Rows[2].Cells[itemColumn.Name].Value = "3";
                                gridDayTables_1P_NDLMS.Rows[3].Cells[itemColumn.Name].Value = "4";
                            }
                            indexColumn++;
                        }


                        //gridActivationDate_1P_NDLMS.Rows.Add();
                        foreach (DataGridViewColumn itemColumn in gridActivationDate_1P_NDLMS.Columns)
                        {
                            gridActivationDate_1P_NDLMS.Rows[0].Cells[itemColumn.Name].Value = "01";
                            gridActivationDate_1P_NDLMS.Rows[1].Cells[itemColumn.Name].Value = "02";
                            gridActivationDate_1P_NDLMS.Rows[2].Cells[itemColumn.Name].Value = "03";
                            gridActivationDate_1P_NDLMS.Rows[3].Cells[itemColumn.Name].Value = "04";
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnFillAuto_Click(object sender, EventArgs e)", ex);
            }
        }

        void getLTCT_WCMDisplayParameters()
        {
            if (rbWCM.Checked)
            {
                ConfigInfo.SignatureInfo = "";
            }
            else if(rbLTCT.Checked)
            {
                ConfigInfo.SignatureInfo = "ST";
            }
            
            FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
            FillDisplayParameters(dGVPushDisplayParams, "PUSH");
            FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");

        }

        private void rbWCM_CheckedChanged(object sender, EventArgs e)
        {

            getLTCT_WCMDisplayParameters();
        }

        private void rbLTCT_CheckedChanged(object sender, EventArgs e)
        {

            getLTCT_WCMDisplayParameters();
        }


        private void SP_NDLMS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SP_NDLMS_DayGridCellClick((DataGridView)sender);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_CellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
        }


        private void SP_NDLMS_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                SP_NDLMS_ValidateDayProfileCell(sender, e);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }





        private void gridTOUDay1_1P_NDLMS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SP_NDLMS_CellClick(sender, e);
        }

        private void gridTOUDay1_1P_NDLMS_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            SP_NDLMS_CellValidating(sender, e);
        }

        private void rdbSinglephase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                OfflineConfigurationSetControlValuesOnMeterModelBasis();
                lngGridViewReadControl1.DeselectCheckBoxes();
                 
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "rdbSinglephase_CheckedChanged(object sender, EventArgs e)", ex);
            }
        }



        private void chkconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkconnect.Checked == true)
            {
                chkDisconnect.Checked = false;
            }
        }

        private void chkDisconnect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisconnect.Checked == true)
            {
                chkconnect.Checked = false;
            }
        }

        private void btnResetAll_Click(object sender, EventArgs e)
        {
            try
            {
                ResetAllTOU();
              
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnResetAll_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnAutoFillSpecial_Click(object sender, EventArgs e)
        {
            try
            {
                AutoFillTOU();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnAutoFillSpecial_Click(object sender, EventArgs e)", ex);
                
        }
            
        }

        private void txtOverLoadthreshhold_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '.') { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtOCthreshold_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '.') { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtTimeInterval1P_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtMaxRetry1P_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtWaitTime1P_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtMaxRinCycle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar)) { }
            else e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void rdbFutureTOU_smartmeter_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU_Smartmeter();
        }

        private void GsmCommPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView98_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                AutoFillTOU();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "button7_Click(object sender, EventArgs e)", ex);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                ResetAllTOU();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "button8_Click(object sender, EventArgs e)", ex);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                ResetAllTOU();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "button7_Click_1(object sender, EventArgs e)", ex);
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                AutoFillTOU();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "button8_Click_1(object sender, EventArgs e)", ex);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

        private void chkDisplayExtended_CheckedChanged(object sender, EventArgs e)
        {
            ConfigInfo.DisplayProgrammingVariant = chkDisplayExtended.Checked ? DisplayProgrammingTypes.TwoByte : DisplayProgrammingTypes.OneByte;
            BindDisplayParameters();
        }

        private void btnReadDisplayFromMeter_Click(object sender, EventArgs e)
        {
            try
            {
               
                ChannelInformation channelInfo = new ChannelInformation();
                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                if (commType == CommunicationType.TCP)
                {
                    channelInfo.ModemInfo = Staticip;
                    channelInfo.TcpPort = Tcpport;
                }
                else
                {
                    channelInfo.ModemInfo = simNumber;
                }

                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communication = new Communication(channelInfo);
                Result result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                {
                    isMeterConnected = true;
                    SetConnectionDetail(true);
                    communication.GetDisplayProgrammingVariant();
                    BindDisplayParameters();
                    List<System.Enum> selectedProfiles = new List<System.Enum>
                    {
                        ProfileId.PushDisplayParameter,
                        ProfileId.ScrollDisplyParameter,
                        ProfileId.HighResolutionDisplayParameter
                    };
                    bool isConnected = GetMeterConfigData(selectedProfiles, false, 0);
                }
                else
                {
                    if (commType == CommunicationType.DIRECT)
                    {
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        if (commType == CommunicationType.TCP)
                        {
                            this.StatusMessageAsync = "Connection " + Staticip + " failed.";
                        }
                        else
                            this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "btnReadDisplayFromMeter_Click", ex);
            }
            finally
            {
                communication.CloseSession();
                isMeterConnected = false;
            }
        }
   }

    public class Parameters
    {
        public int SNo = 0;
        public string Name = string.Empty;
        public int StartPosition = 0;
        public int Length = 0;
        public Parameters(int sno, string name, int startPos, int len)
        {
            this.SNo = sno;
            this.Name = name;
            this.StartPosition = startPos;
            this.Length = len;
        }
    }
       
    public class NonDLMSConfiguration
    {
        public List<Parameters> lstParameter = null;
        private static NonDLMSConfiguration instance = null;

        private NonDLMSConfiguration()
        {
            lstParameter = new List<Parameters>();
            lstParameter.Add(new Parameters(1,"PushDisplayParameter",49,4));
            lstParameter.Add(new Parameters(2, "ScrollDisplayParameter", 53, 4));
            lstParameter.Add(new Parameters(3, "PushDisplayParameter", 57, 4));

            lstParameter.Add(new Parameters(4, "KVAHSelection", 61, 1));
        }

        public static NonDLMSConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NonDLMSConfiguration();
                }
                return instance;
            }           
        }
    }
}
