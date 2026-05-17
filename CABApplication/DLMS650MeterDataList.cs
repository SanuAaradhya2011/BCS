using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO; 
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication;
using LTCTBLL;
using CAB.Parser;
using CABEntity;
using System.Collections.ObjectModel;
using CAB.Serialization;
using System.Drawing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;
using System.Linq;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class DLMS650MeterDataList : MdiChildForm
    {
        // SB code change Start - 20180629 - Multiple Analysis View
        //private ContextMenuStrip contextMenu = new ContextMenuStrip();
        // SB code change End - 20180629 - Multiple Analysis View
        public DataSet ABCReportDs = new DataSet();
        TabNameBLL tabNameBll;
        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
        TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
        private bool isDTMLoadSurvey = false;
        DLMS650CommonBLL common;
        DLMS650BillingBLL billingBLL;
        DLMS650LoadSurveyBLL loadSurveyBLL;
        bool isPUMA = false;
        string utility = string.Empty;
        DataGridView SpecialDayProfileGrid;
        bool isMVVNL = false;
        bool isMPKWCL = false;
        const int base10 = 10;
        char[] cHexa = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
        //private const string CUMPOWEROFFDURATION = "Cumulative Power-Failure Duration (0.0.94.91.8.255;3;2) (YY:MM:DDD HH:MM)";
        private const string CUMPOWERFAILURECOUNT = "Cumulative Power-Failure Count (0.0.96.7.0.255;1;2)";
        private const string CUMTAMPERCOUNT = "Cumulative Tamper Count (0.0.94.91.0.255;1;2)";
        private const string DELTATAMPERCOUNT = "Tamper Count (0.0.96.2.190.255;1;2)"; // Story - 345154
        private const string CUMBILLINGMDRESETCOUNT = "Cumulative Billing Count (0.0.0.1.0.255;1;2)";
        private const string DISPLAYMEMBER = "DisplayMember";
        private const string VALUEMEMBER = "ValueMember";
        private const string ABCCodeBilling = "ABC Code(0.0.96.2.196.255;1;2)";
        private Serializer serializer = null;
        private static object syncRoot = new object();
        public static MeterConfigSettings meterConfigSettings = null;
        private const int WIDTH = 260;
        private const int HISTORYWIDTH = 100;
        private const int ABCWidth = 200;
        private const string HISTORY = "History";
        private const int DAILYCONSUMPTIONWIDTH = 150;
        private const int LngInstantWidth = 817;
        private const int LngInstantHeight = 385;
        private const int MainFormRestoreDownWidth = 1150;
        private const int LngInstantRestoreDownWidthOffSet = 70;
        private const int LngInstantRestoreDownHeigtOffSet = 110;
        private const int GridtabHeightOffSet = 30;
        private const int GridtabWidthtOffSet = 20;
        private Dictionary<string, string> tamperMapper;
        private MeterConfigSettingsMeterConfigElement element;
        private byte[] activeSeasonProfile;
        private byte[] activeWeekProfile;
        private byte[] activeDayProfile;
        private byte[] passiveSeasonProfile;
        private byte[] passiveWeekProfile;
        private byte[] passiveDayProfile;
        private byte[] passiveActivationDate;
        private byte[] specialDayProfile;
        private byte dayProfileCount = 1;
        private byte weekProfileCount = 1;
        private byte seasonProfileCount = 1;
        private byte SpecialDayProfileCount = 1;
        private int CTRatio;
        private int PTRatio;
        private int VTRatio;
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
        private const string COLTARIFF = "colTariff";
        private const string COLSTARTHOUR = "colStartHour";
        private const string STARTHOUR = "Start Hour";
        private const string COLSTARTMIN = "colStartMin";
        private const string STARTMIN = "Start Min";
        private const string WEEK = "Week";
        private const string ONETOU = "ONE";
        private const string TWOTOU = "TWO";
        private const string FOURTOU = "FOUR";
        private const string ascendingOrder = "asc";
        private const string SEASONPROFILE = "Season";
        private const string descendingOrder = "desc";
        DataGridView[] dayProfileGrids;
        DataGridView seasonProfileGrid;
        DataGridView weekProfileGrid;
        DateTimePicker touActivationDate;
        RadioButton rdbTOUType;
        private int MeterModelNumber;
        string meter_cat;
        // SB Code change start 20161123 - Present Tamper Only
        DataSet dataSetTamper = new DataSet();
        // SB Code change end 20161123 - Present Tamper Only
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650MeterDataList).ToString());
        // SB code change Start - 20180629 - Multiple Analysis View
        string strMsg = string.Empty;
        string strActiveFileType = string.Empty;
        string strActiveFirmwareVersion = string.Empty;
        string strActiveMeterType = string.Empty;

        ContextMenu contextMenu = new ContextMenu();
        // SB code change Stop - 20180629 - Multiple Analysis View

        //DateTime FromDate;
        //DateTime ToDate;
        //DateTime tmpDTPickFromDate, tmpDTPickToDate;
        public DLMS650MeterDataList()
        {
            InitializeComponent();
            
            common = new DLMS650CommonBLL();
            billingBLL = new DLMS650BillingBLL();
            loadSurveyBLL = new DLMS650LoadSurveyBLL();
            tabNameBll = new TabNameBLL();
            if (CAB.Framework.UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (CAB.Framework.UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else if (CAB.Framework.UtilityEntity.MPKWCL == UtilityDetails.Utility)
            {
                isMPKWCL = true;
            }
            else
            {
                isPUMA = false;
                isMVVNL = false;
                isMPKWCL = false;
            }
            tabControl2.TabPages.Remove(tabPageTouConfiguration);
           int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);           
          if (meterModelNumber != 34 && meterModelNumber != 35 && meterModelNumber != 37)
                AddTamperParams();//For all meters excluding smart meter
            else
                AddTamperParamsFalcon2();//For smart meter
            serializer = new Serializer();
            lock (syncRoot)
            {
                if (meterConfigSettings == null)
                {
                    meterConfigSettings = (MeterConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterConfigSettings.xml"), typeof(MeterConfigSettings));
                }
            }
            /* AK: Tamper Resolution issue at specific resolution (1024 * 768) is fixed*/
            if (Screen.PrimaryScreen != null && Screen.PrimaryScreen.Bounds.Width <= 1024)
            {
                if (this.lngElectricalCondition != null)
                    this.lngElectricalCondition.Width = 300;
                if (this.lngTamperOccur != null)
                    this.lngTamperOccur.Width = 300;
            }

            // SB code change Start - 20180629 - Multiple Analysis View
            this.Activated += new EventHandler(DLMS650MeterDataList_Activated);
            MenuItem menuItemReport = new MenuItem("Detailed Report");
            MenuItem menuItemBillingReport = new MenuItem("Billing Report");
            MenuItem menuItemTamper = new MenuItem("Tamper");
            MenuItem menuItemLoadSurvey = new MenuItem("Load Survey");
            MenuItem menuItemLoadSurveyFileWise = new MenuItem("File Wise");
            MenuItem menuItemLoadSurveyMeterIDWise = new MenuItem("Meter ID Wise");
            MenuItem menuItemLoadSwitch = new MenuItem("Load Switch");
            MenuItem menuItemMidnight = new MenuItem("Midnight");
            MenuItem menuItemMidnightFileWise = new MenuItem("File Wise");
            MenuItem menuItemMidnightMeterIDWise = new MenuItem("Meter ID Wise");
            MenuItem menuItemSeparator1 = new MenuItem("-");
            MenuItem menuItemClose = new MenuItem("Close");

            //menuItemLoadSurvey.MenuItems.Add(menuItemLoadSurveyFileWise);
            //menuItemLoadSurvey.MenuItems.Add(menuItemLoadSurveyMeterIDWise);

            //menuItemMidnight.MenuItems.Add(menuItemMidnightFileWise);
            //menuItemMidnight.MenuItems.Add(menuItemMidnightMeterIDWise);

            contextMenu.MenuItems.Add(menuItemReport);
            contextMenu.MenuItems.Add(menuItemBillingReport);
            contextMenu.MenuItems.Add(menuItemTamper);
            contextMenu.MenuItems.Add(menuItemLoadSurvey);
            contextMenu.MenuItems.Add(menuItemLoadSwitch);
            contextMenu.MenuItems.Add(menuItemMidnight);
            contextMenu.MenuItems.Add(menuItemSeparator1);
            contextMenu.MenuItems.Add(menuItemClose);

            menuItemReport.Click += new EventHandler(menuItemReport_Click);
            menuItemBillingReport.Click += new EventHandler(menuItemBillingReport_Click);
            menuItemClose.Click += new EventHandler(menuItemClose_Click);
            menuItemTamper.Click += new EventHandler(menuItemTamper_Click);
            menuItemLoadSurvey.Click += new EventHandler(menuItemLoadSurvey_Click);
            menuItemLoadSurveyFileWise.Click += new EventHandler(menuItemLoadSurveyFileWise_Click);
            menuItemLoadSurveyMeterIDWise.Click += new EventHandler(menuItemLoadSurveyMeterIDWise_Click);
            menuItemLoadSwitch.Click += new EventHandler(menuItemLoadSwitch_Click);
            menuItemMidnight.Click += new EventHandler(menuItemMidnight_Click);
            menuItemMidnightMeterIDWise.Click += new EventHandler(menuItemMidnightMeterIDWise_Click);
            menuItemMidnightFileWise.Click += new EventHandler(menuItemMidnightFileWise_Click);
            this.ContextMenu = contextMenu;
            // SB code change End - 20180629 - Multiple Analysis View
        }

        // SB code change Start - 20180629 - Multiple Analysis View
        void menuItemMidnightFileWise_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowMidnightReportFileWise(sender, e);
            }
        }

        void menuItemMidnightMeterIDWise_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowMidnightReportMeterIDWise(sender, e);
            }
        }

        void menuItemLoadSurvey_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowLoadSurveyReport(sender, e);
            }
        }

        void menuItemLoadSurveyMeterIDWise_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowLoadSurveyReportMeterIDWise(sender, e);
            }
        }

        void menuItemBillingReport_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowBillingReportReport(sender, e);
            }
        }

        void menuItemMidnight_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowMidnightReport(sender, e);
            }
        }

        void menuItemLoadSwitch_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowLoadSwitchReport(sender, e);
            }
        }

        void menuItemLoadSurveyFileWise_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowLoadSurveyReportFileWise(sender, e);
            }
        }

        void menuItemTamper_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowTamperReport(sender, e);
            }
        }

        void menuItemReport_Click(object sender, EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.ShowReport(sender, e);
            }
        }

        void menuItemClose_Click(object sender, EventArgs e)
        {
            this.closeToolStripMenuItem_Click(sender, e);
        }

        void DLMS650MeterDataList_Activated(object sender, EventArgs e)
        {
            ConfigInfo.ActiveMeterDataId = strMsg;
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            ConfigInfo.ActiveFileType = strActiveFileType;
            ConfigInfo.ActiveFirmwareVersion = strActiveFirmwareVersion;
            ConfigInfo.ActiveMeterType = strActiveMeterType;

            SelectDialog selectDialog = (SelectDialog)Application.OpenForms["Application"];

            if (selectDialog != null)
            {
                selectDialog.Activate();
            }
        }
        // SB code change End - 20180629 - Multiple Analysis View

        private void AddTamperParams()
        {
            tamperMapper = new Dictionary<string, string>();
            tamperMapper.Add("DIP", "152|Demand Integration Period");
            tamperMapper.Add("KvahSelection", "158/188|kVAh Selection Changed"); // Sapphire LTCT Meter [ST] new DLMS code added KvahSelection
            tamperMapper.Add("PushDisplayParameter", "160|Display - Push Mode Config");// Story - Hide Display Timeout Parameter
            tamperMapper.Add("ScrollDisplyParameter", "161|Display - Scroll Mode Config");
            tamperMapper.Add("HighResolutionDisplayParameter", "162|Display - HR Mode Config");
            tamperMapper.Add("DisplayParameters", "192|Display Parameter"); // Sapphire LTCT Meter [ST] new DLMS code added
            tamperMapper.Add("TOU", "155|Activity Calendar for Time Zones etc.");
            tamperMapper.Add("RTC", "151|Real Time Clock - Date and Time");
            tamperMapper.Add("BillingType", "154|Single-Action Schedule for Billing Dates");
            tamperMapper.Add("BillingMonthType", "194|Single-Action Schedule for Billing Periods"); // Billing Period (Cycle)
            tamperMapper.Add("BillingReset", "159/189|MD Reset");
            tamperMapper.Add("SIP", "153|Profile Capture Period");
            tamperMapper.Add("CTRatio", "190|CT Ratio Changed");
            tamperMapper.Add("PTRatio", "191|PT Ratio Changed");
            tamperMapper.Add("AutoLock", "164/187|Auto Billing Lock"); // Sapphire LTCT Meter [ST] new DLMS code added
            tamperMapper.Add("RS232LockUnlock", "165/186|RS232 Lock"); // Sapphire LTCT Meter [ST] new DLMS code added
            tamperMapper.Add("DailyLog", "166|Daily Log Parameters");
            tamperMapper.Add("SoftwareBilling", "167/185|Software Billing Parameters"); // Sapphire LTCT Meter [ST] new DLMS code added
            tamperMapper.Add("ManualBilling", "168/184|Manual Billing Parameters"); // Sapphire LTCT Meter [ST] new DLMS code added
            //Temper reset 1P
            tamperMapper.Add("MagneticTamperIcon", "256|Tamper Reset");

            //Temper reset 3P
            tamperMapper.Add("MagneticTamperIcon3P", "200|Tamper Reset"); // Task ID: 569567: Tamper Reset Option in 3P DLMS Sapphire Two TOU "sc" model

            //nidhi
            tamperMapper.Add("RS485", "156|RS 485");

        }

        private void AddTamperParamsFalcon2()//For smart meter Transaction List
        {
            tamperMapper = new Dictionary<string, string>();
            tamperMapper.Add("RTC", "151|Real Time Clock - Date and Time");
            tamperMapper.Add("DIP", "152|Demand Integration Period");
            tamperMapper.Add("SIP", "153|Profile Capture Period");
            tamperMapper.Add("BillDateChange", "154|BILL Date Change");
            tamperMapper.Add("TOU", "155|Activity Calendar for Time Zones etc.");
            tamperMapper.Add("RS485", "156|RS 485");
            tamperMapper.Add("NewFirmware", "157|New Firmware Activated");
             tamperMapper.Add("LoadLimit", "158|Load Limit (Kw) set");
             tamperMapper.Add("LoadLimitEnabled", "159|Billing Reset/Load Limit Function-Enabled");
            tamperMapper.Add("LoadLimitDisabled", "160|Load Limit Function-Disabled");
             tamperMapper.Add("LLSSecret", "161|LLS Secret (MR)Change");
            tamperMapper.Add("HLSkeyChange", "162|HLS key (US)Change");
            tamperMapper.Add("HLSkeyFWChange", "163|HLS key (FW)Change");
            tamperMapper.Add("GlobalkeyChange", "164|Global key change");
            tamperMapper.Add("ESWFChange", "165|ESWF change");
            tamperMapper.Add("MDReset", "166|MD Reset");
            tamperMapper.Add("Meteringmode", "167|Metering Mode");
             tamperMapper.Add("ImageActivationSchedule", "169|Image Activation Single action schedule");
            tamperMapper.Add("ConfigChangedForwardMode", "177|ConfigChanged Forward Mode Only");
  tamperMapper.Add("ConfigChangedImportExportMode", "178|Config Changed Import and Export Mode");
            tamperMapper.Add("LastTokenAmountPrepaid", "751|Last Token Recharge Amount prepaid");
            tamperMapper.Add("LastTokenTimePrepaid", "752|Last Token Recharge Time prepaid");
          tamperMapper.Add("TotalAmountLastRecharge", "753|Total Amount Last Recharge prepaid");
            tamperMapper.Add("CurrentBalanceAmount", "754|Current Balance Amount prepaid");
            tamperMapper.Add("CurrentBalanceTime", "755|Current Balance Time prepaid");
            tamperMapper.Add("DigitalOutputOperation", "756|Digital output Operation");
            tamperMapper.Add("SIPPeriodChange", "757|Sliding Demand Period Change");
             tamperMapper.Add("EventThresholdConfig", "758|Event Threshold Config Change");
             tamperMapper.Add("EventThresholdPersistence", "759|Event Threshold Persistence time Change");
            tamperMapper.Add("DisplayParameters", "760|Event Display Parameters Change");
            tamperMapper.Add("LSParameterStoreID", "761|LS Parameter Store ID");
            tamperMapper.Add("OpticalLock", "762|Optical port Lock");
            tamperMapper.Add("OpticalLockUnlock", "763|Optical port Unlock");
            tamperMapper.Add("RJLock", "764|RJ port Lock");
            tamperMapper.Add("RJLockUnlock", "765|RJ port Unlock");
            tamperMapper.Add("SpecialDay", "766|Special Day");
            tamperMapper.Add("EventEnableDisable", "767|Event Enable/Disable Configuration");
            tamperMapper.Add("LoadControl", "768|Load control parameter");
            tamperMapper.Add("ARMButtonEnable", "769|ARM button Enable");
            tamperMapper.Add("ARMButtonDisable", "770|ARM button Disable");
            tamperMapper.Add("FSModeLock", "771|FS Mode Lock");
            tamperMapper.Add("FSModeUnlock", "772|FS Mode Unlock");

        }


        /// <summary>
        /// This method is used to fetch the general data from DB and to show it in analysis report.
        /// </summary>
        private void FillGeneral()
        {
            string dummyFirmwareVersion = string.Empty;
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                DataSet dataSet = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(MeterDataID));

                //Get Meter Constant from Nameplate Profile if Meter Constant is empty(----) comes form meter in General Profile   
                DataSet dataSetNameplate = new DLMS650NamePlateBLL().GetMeterData(Convert.ToInt32(MeterDataID));                
                string MeterConstantInfo = "----";
                string PrimaryMeterConstantInfo = "----";//PGVCL
                DataRow NameplateRow = null;
                if (dataSetNameplate != null && dataSetNameplate.Tables != null && dataSetNameplate.Tables.Count > 0)
                {
                    for (int count = 0; count < dataSetNameplate.Tables[0].Rows.Count; count++)
                    {
                        NameplateRow = dataSetNameplate.Tables[0].Rows[count];
                        if (NameplateRow["Descriptions"].ToString() == "Meter Constant")
                        {
                            MeterConstantInfo = NameplateRow["Value"].ToString();
                        }
                        else if (NameplateRow["OBIS Code"].ToString() == "0.0.96.128.17.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                        {
                            int idx = dataSet.Tables[0].Rows.IndexOf(dataSet.Tables[0].Select().First(s => s["OBIS Code"].ToString().Contains("0.0.96.1.4.255")));
                            dataSet.Tables[0].Rows.InsertAt(dataSet.Tables[0].NewRow(), idx + 1);
                            dataSet.Tables[0].Rows[idx + 1].ItemArray = NameplateRow.ItemArray;
                        }
                        else if (NameplateRow["OBIS Code"].ToString() == "1.0.96.128.15.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                        {
                            dataSet.Tables[0].ImportRow(NameplateRow);
                        }
                    }
                    dataSet.AcceptChanges();
                }

                DataRow generalRow = null;
               if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    for (int count = 0; count < dataSet.Tables[0].Rows.Count; count++)
                    {
                        generalRow = dataSet.Tables[0].Rows[count];
                        if (generalRow["Descriptions"].ToString() == "Internal CT Ratio")
                        {
                            int.TryParse(generalRow["Value"].ToString(), out CTRatio);
                        }
                        else if (generalRow["Descriptions"].ToString() == "Internal PT Ratio")
                        {
                            int.TryParse(generalRow["Value"].ToString(), out PTRatio);
                        }
                        else if (generalRow["Descriptions"].ToString() == "Internal VT Ratio")
                        {
                            int.TryParse(generalRow["Value"].ToString(), out VTRatio);
                        }

                        //MSEDCL Bug Fix
                        if (generalRow["Descriptions"].ToString() == "Internal Firmware Version")
                        {
                            dummyFirmwareVersion = generalRow["Value"].ToString();
                          
                            if (dummyFirmwareVersion == "2.21" || dummyFirmwareVersion == "")
                            {

                                generalRow["Value"] = "----";
                            }
                        }
                       // if (generalRow["Descriptions"].ToString() == "Current Rating (Ib-Imax)")
                        if (generalRow["Descriptions"].ToString() == "Current Rating ") //PGVCL
                            {

                            if (dummyFirmwareVersion == "1.66" || dummyFirmwareVersion == "4.64" || dummyFirmwareVersion == "0.00" || dummyFirmwareVersion == "4.58" || dummyFirmwareVersion == "2.21" || dummyFirmwareVersion == "")
                            {

                                generalRow["Value"] = "----";
                            }
                        }

                        if (generalRow["Descriptions"].ToString() == "Voltage Rating")
                        {
                            dummyFirmwareVersion = generalRow["Value"].ToString();
                            if (dummyFirmwareVersion == "") 
                            {
                                generalRow["Value"] = "----";
                            }
                        }
                       
                        //if (generalRow["Descriptions"].ToString() == "Communication Type")
                        //{
                        //    generalRow.Delete();
                        //    count--;
                        //    continue;

                        //}
                        //if (generalRow["Descriptions"].ToString() == "Release Type")
                        //{
                        //    generalRow.Delete();
                        //    continue;

                        //}

                        if (generalRow["Descriptions"].ToString() == " Primary Meter Constant")//PGVCL
                        {
                            if (generalRow["Value"].ToString() == "----")
                            {
                                generalRow["Value"] = PrimaryMeterConstantInfo;
                            }
                            if (ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS")
                            {
                                generalRow["OBIS Code"] = "0.0.0.0.0.255";
                            }
                        }


                        if (generalRow["Descriptions"].ToString() == "Meter Constant" || generalRow["Descriptions"].ToString() == "Secondary Meter Constant")//PGVCL
                        {
                            if (generalRow["Value"].ToString() == "----")
                            {
                                generalRow["Value"] = MeterConstantInfo;
                            }
                            if (ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS")
                            {
                                generalRow["OBIS Code"] = "0.0.0.0.0.255";
                            }                           
                        }
                    }
                    lngGeneral.Data = dataSet;
                    lngGeneral.SetWidth("Descriptions", 200);
                    lngGeneral.SetWidth("OBIS Code", 100);
                }
            }
        }
        /// <summary>
        /// /// <summary>
        /// This method is used to fetch the instant data from DB and to show it in analysis report.
        /// </summary>
        /// </summary>
        private void FillInstant()
        {
            DataSet dataSet = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(MeterDataID));
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                // Story - 427028 - instant data format in analysis view is not correct
                common.ApplyMultiplyFactorForInstant(Convert.ToInt64(MeterDataID), dataSet, "Descriptions", "Value");
            }
            string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(MeterDataID);
            string chkValNumPowFail = ConfigSettings.GetValue("ChkNumPowFail");
         
            if (dataSet != null)
            {
                #region WB and UGVCL Specific
                if (chkValNumPowFail == "1")
                {
                    DataRow[] rowPowFailCount = dataSet.Tables[0].Select("Descriptions = 'Number of Power-Failures'");
                    if (rowPowFailCount != null && rowPowFailCount.Length > 0)
                    {
                        dataSet.Tables[0].Rows.Remove(rowPowFailCount[0]);
                        dataSet.AcceptChanges();
                    }
                }  
                
                #endregion

                #region TNEB Model specific check to remove Reverse KWH
                // [ReverseKWH_Remove]
                int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);

                if (meterModelNo.ToString() == "13")
                {
                    DataRow[] reverseKWH = dataSet.Tables[0].Select("Descriptions = 'Reverse KWH'");

                    if (reverseKWH != null && reverseKWH.Length > 0)
                    {
                        dataSet.Tables[0].Rows.Remove(reverseKWH[0]);
                        dataSet.AcceptChanges();
                    }
                }
                #endregion

                #region VIM Series2 Model specific check to remove ABC Code
                // [ABC Code]
                int metermodel= new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);

                if (metermodel.ToString() == "31")
                {
                    DataRow[] Abccode = dataSet.Tables[0].Select("Descriptions = 'ABC'");

                    if (Abccode != null && Abccode.Length > 0)
                    {
                        dataSet.Tables[0].Rows.Remove(Abccode[0]);
                        dataSet.AcceptChanges();
                    }
                }
                #endregion

                lngInstant.Data = dataSet;
                lngInstant.SetWidth("Descriptions", 230);
                lngInstant.SetWidth("OBIS Code", 100);
                lngInstant.SetWidth("Class ID", 55);
                lngInstant.SetWidth("Attribute", 55);
                lngInstant.SetWidth("Value", 140);
                lngInstant.SetWidth("Unit", 140);
            }
        }
        //************** For VIM series 2 meter **************
        private void FillABCCode()
        {
            try
            {
                DataSet instantDS = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(MeterDataID));
                DataSet generalDS = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                if (tabControlInstant.TabPages.Contains(tabPageABCCode))
                {
                    tabControlInstant.TabPages.Remove(tabPageABCCode);
                }
                if (instantDS != null && generalDS != null)
                {
                  DataRow[] ABCcode = instantDS.Tables[0].Select("[OBIS Code] = '0.0.96.2.196.255'");
                  DataRow[] MeterID = generalDS.Tables[0].Select("[OBIS Code] = '0.0.96.1.0.255'");
                   if (ABCcode != null && ABCcode.Length > 0 && MeterID != null && MeterID.Length > 0)
                    {
                        tabControlInstant.TabPages.Add(tabPageABCCode);
                        string ABC = ABCcode[0].ItemArray[4].ToString().Trim();
                        string meterid = MeterID[0].ItemArray[4].ToString().Trim();
                        ABCDecrypt(ABC, meterid);
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillABCCode()", ex);
            }
        }

        public void ABCDecrypt(string digitcode20, string meterid)
        {
            string EncyCode = "";
            string BinValue = "";
            string TampValue = "";
            string DcodeByte = "";
            int DecodeValue = 0;
            DataTable DTable = new DataTable();
            DTable.Columns.Add("Parameter");
            DTable.Columns.Add("Value");
            DataRow Drow;
            txtCode.Text = digitcode20.Trim();
            txt_EnterMID.Text = meterid.Trim();
            EncyCode = digitcode20.Trim();
            int[] XorByte = new int[] { 4, 3, 7, 2, 1, 5, 6, 3, 4, 2, 5, 1, 7, 5, 2, 4, 1, 6, 3, 6 };
            if (EncyCode.Length == 20)
            {
                for (int a = 0; a < 20; a++)
                {
                    DecodeValue = Convert.ToInt16(EncyCode.Substring(a, 1));
                    if (DecodeValue <= XorByte[a])
                    {
                        DcodeByte = DcodeByte + Convert.ToString((XorByte[a] - DecodeValue));
                    }
                    else
                    {
                        DcodeByte = DcodeByte + Convert.ToString(((10 - DecodeValue) + XorByte[a]));
                    }
                }
                string mid = DcodeByte.Substring(19, 1) + DcodeByte.Substring(14, 1) + DcodeByte.Substring(6, 1) + DcodeByte.Substring(5, 1) + DcodeByte.Substring(12, 1) + DcodeByte.Substring(1, 1) + DcodeByte.Substring(13, 1) + DcodeByte.Substring(2, 1);
                string kwh = DcodeByte.Substring(4, 1) + DcodeByte.Substring(11, 1) + DcodeByte.Substring(3, 1) + DcodeByte.Substring(15, 1) + DcodeByte.Substring(10, 1) + DcodeByte.Substring(16, 1) + "." + DcodeByte.Substring(9, 1);
                string md  = DcodeByte.Substring(17, 1) + DcodeByte.Substring(8, 1) + "." + DcodeByte.Substring(18, 1);
                string MID = meterid.Trim();


                while (MID.Length < 8) MID = "0" + MID;

                if (mid.Trim() != MID)
                {
                    kwh = "";
                    md = "";
                    //MessageBox.Show("Wrong Code or Meter ID !" + "\r\n" + "Please Enter Valid 20 Digit Code or Valid Meter ID", "ABC Code", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   return;
                }

                BinValue = DecimalToBase(Int32.Parse(DcodeByte.Substring(7, 1)), 2) + DecimalToBase(Int32.Parse(DcodeByte.Substring(0, 1)), 2);

                //**************** Code added by Deep for all data in grid ***************
                Drow = DTable.NewRow();
                Drow["Parameter"] = "";
                Drow["Value"] = "---";

                Drow["Parameter"] = "Billing (kWh)";
                Drow["Value"] = kwh;
                DTable.Rows.Add(Drow);
                Drow = DTable.NewRow();
                Drow["Parameter"] = "Billing MD (kW)";
                Drow["Value"] = md;
                DTable.Rows.Add(Drow);

                for (int i = 8; i >= 1; i--)
                {
                    TampValue = String.Format("{0:Present;;Absent}", Convert.ToInt16(BinValue.Substring(i - 1, 1)));

                    Drow = DTable.NewRow();
                    Drow["Parameter"] = "";
                    Drow["Value"] = "---";



                    if (TampValue == "Present")
                    {
                        if (i == 8)
                        {
                            Drow["Parameter"] = "Earth Tamper";
                            Drow["Value"] = "Present";


                        }
                        if (i == 7)
                        {
                            Drow["Parameter"] = "Reverse Tamper";
                            Drow["Value"] = "Present";

                        }
                        if (i == 6)
                        {
                            Drow["Parameter"] = "Magnet Tamper";
                            Drow["Value"] = "Present";

                        }
                        if (i == 4)
                        {
                            Drow["Parameter"] = "ESD Tamper";
                            Drow["Value"] = "Present";
                            
                        }
                        if (i == 3)
                        {
                            Drow["Parameter"] = "Neutral Disturbance";
                            Drow["Value"] = "Present";

                        }
                        if (i == 2)
                        {
                            Drow["Parameter"] = "Single Wire";
                            Drow["Value"] = "Present";

                        }
                    }
                    else
                    {

                        if (i == 8)
                        {
                            Drow["Parameter"] = "Earth Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 7)
                        {
                            Drow["Parameter"] = "Reverse Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 6)
                        {
                            Drow["Parameter"] = "Magnet Tamper";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 4)
                        {
                            Drow["Parameter"] = "ESD Tamper";
                            Drow["Value"] = "Absent"; ;
                           
                        }
                        if (i == 3)
                        {
                            Drow["Parameter"] = "Neutral Disturbance";
                            Drow["Value"] = "Absent";

                        }
                        if (i == 2)
                        {
                            Drow["Parameter"] = "Single Wire";
                            Drow["Value"] = "Absent";

                        }
                    }

                 if (!Drow["Value"].ToString().Contains("--"))
                        DTable.Rows.Add(Drow);
                }
                
                ABCReportDs.Tables.Add(DTable);   

                string temp_Data = mid + "\r\n";
                temp_Data += kwh + "\r\n";
                temp_Data += md + "\r\n";
                for (int icount = 0; icount < DTable.Rows.Count; icount++)
                    temp_Data += DTable.Rows[icount]["Parameter"].ToString() + "," + DTable.Rows[icount]["Value"].ToString() + "\r\n";

                CreateTempFile(temp_Data);
                dgtamperstatus.Columns.Clear();
                dgtamperstatus.DataSource = DTable;
                dgtamperstatus.Columns[0].Width = 125;
                dgtamperstatus.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgtamperstatus.Columns[1].Width = 105;
                dgtamperstatus.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            else
            {
                MessageBox.Show("20 Digit code not available", MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
            }

        }
        string DecimalToBase(int iDec, int numbase)
        {
            string strBin = "";
            int[] result = new int[32];
            int MaxBit = 32;
            for (; iDec > 0; iDec /= numbase)
            {
                int rem = iDec % numbase;
                result[--MaxBit] = rem;
            }
            for (int i = 0; i < result.Length; i++)
                if ((int)result.GetValue(i) >= base10)
                    strBin += cHexa[(int)result.GetValue(i) % base10];
                else
                    strBin += result.GetValue(i);
            strBin = strBin.TrimStart(new char[] { '0' });
            if (strBin == "")
            {
                strBin = "0";
            }
            strBin = string.Format("{0:0000}", Convert.ToDouble(strBin));
            return strBin;
        }

        public void CreateTempFile(string Message)
        {
            try
            {

                String FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "tmpCrypto.txt";
                FileStream FS = new FileStream(FilePath, FileMode.Create);
                StreamWriter SW = new StreamWriter(FS);
                SW.WriteLine(Message);
                SW.Flush();
                SW.Close();
                FS.Close();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CreateTempFile(string Message)", ex);
            }

        }
        /// <summary>
        /// This method is used to fetch the general data from DB and to show it in analysis report.
        /// </summary>
        private void FillNamePlate()
        {
            string dummyFirmwareVersion = string.Empty;
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                DataSet dataSet = new DLMS650NamePlateBLL().GetMeterData(Convert.ToInt32(MeterDataID));
                DataRow generalRow;



                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {

                    //Hide nameplate tab if hide nameplate checkbox is not selected from setting
                    if (ConfigSettings.GetValue("ChkHideNamePlDetails") == "1")
                    {
                        tabControlReport.TabPages.Remove(tabNamePlateDetails);
                        return;
                    }

                    for (int count = 0; count < dataSet.Tables[0].Rows.Count; count++)
                    {
                        generalRow = dataSet.Tables[0].Rows[count];
                        if (generalRow["Descriptions"].ToString() == "Internal CT Ratio")
                        {
                            int.TryParse(generalRow["Value"].ToString(), out CTRatio);
                        }
                        else if (generalRow["Descriptions"].ToString() == "Internal PT Ratio")
                        {
                            int.TryParse(generalRow["Value"].ToString(), out PTRatio);
                        }

                        if (generalRow["Descriptions"].ToString() == "Internal Firmware Version")
                        {
                            dummyFirmwareVersion = generalRow["Value"].ToString();
                            if (dummyFirmwareVersion == "2.21")
                            {
                                generalRow["Value"] = "----";
                            }
                        }
                       // if (generalRow["Descriptions"].ToString() == "Current Rating (Ib-Imax)")
                            if (generalRow["Descriptions"].ToString() == "Current Rating") //PGVCL
                            {
                            if (dummyFirmwareVersion == "1.66" || dummyFirmwareVersion == "4.64" || dummyFirmwareVersion == "0.00" || dummyFirmwareVersion == "4.58" || dummyFirmwareVersion == "2.21")
                            {
                                generalRow["Value"] = "----";
                            }
                        }
                    }
                    lngNamePlate.Data = dataSet;
                    lngNamePlate.SetWidth("Descriptions", 200);
                    lngNamePlate.SetWidth("OBIS Code", 100);
                }
                else
                {
                    tabControlReport.TabPages.Remove(tabNamePlateDetails);
                }
            }
        }


        /// <summary>
        /// This method is used to fetch self diagnosis/anomaly data from db and show it in anomaly tab on anlysis report
        /// 1 => Flash ; 2 => EEPROM ; 3 => Power Supply ; 4 => RTC ; 8 => RTC Battery ; 9 => Main Battery
        /// </summary>
        private void FillAnamoly()
        {
            
            DataSet anomalyData = new AnomalyBLL().GetAnomalyDataForAnalysisDetail(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
            if (anomalyData != null && anomalyData.Tables != null && anomalyData.Tables.Count > 0 && anomalyData.Tables[0].Rows.Count > 0)
            {
                //******* Meter Model Change Required Here ***********//
                if (MeterModelNumber == 9 || MeterModelNumber == 8 || MeterModelNumber == 16 || MeterModelNumber == 23 || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek || MeterModelNumber == NamePlateConstants.SFSP || MeterModelNumber == 11 || MeterModelNumber == 17 || MeterModelNumber == 18 || MeterModelNumber == 13 || MeterModelNumber == 24 || MeterModelNumber == 25 || MeterModelNumber == 32 || MeterModelNumber == 33 || MeterModelNumber == 34 || MeterModelNumber == 35 || MeterModelNumber == 36 || MeterModelNumber == 37 || MeterModelNumber == 31 || MeterModelNumber == 43 || MeterModelNumber == 45 || MeterModelNumber == 46)// ADD METER MODEL 3PH THREE TOU IN 6THSEPT2017 
                {
                    DataTable anomalyWBTable = anomalyData.Tables[0].Copy();
                    anomalyData.Clear();
                    /// NVM 
                    if (anomalyWBTable.Rows[0]["Status"].ToString() == "OK" && anomalyWBTable.Rows[1]["Status"].ToString() == "OK" && (MeterModelNumber == 9 || MeterModelNumber == 11 || MeterModelNumber == 17 || MeterModelNumber == 18 || MeterModelNumber == 13 || MeterModelNumber == 24 || MeterModelNumber == 25 || MeterModelNumber == 32 || MeterModelNumber == 34 || MeterModelNumber == 35 || MeterModelNumber == 36 || MeterModelNumber == 37 || MeterModelNumber == 31 || MeterModelNumber == 43 || MeterModelNumber == 45 || MeterModelNumber == 46))
                    {
                        anomalyData.Tables[0].Rows.Add("NVM Status", "OK");
                    }
                    //******* Meter Model Change Required Here ***********//
                    else if (anomalyWBTable.Rows[1]["Status"].ToString() == "OK" && (MeterModelNumber == 8 || MeterModelNumber == 16 || MeterModelNumber == 23 || MeterModelNumber == NamePlateConstants.SFSP || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek || MeterModelNumber == NamePlateConstants.ThreeTOUWCMValue || MeterModelNumber == NamePlateConstants.SapphireWCM_St))// FOR 3PH THREE TOU
                    {
                        anomalyData.Tables[0].Rows.Add("NVM Status", "OK");
                    }
                    else
                    {
                        anomalyData.Tables[0].Rows.Add("NVM Status", "NOT OK");
                    }

                    if (MeterModelNumber == 23) //Hardcoded for 1P Smart meter(Command is not implemented in meter)
                    {
                        if (anomalyData.Tables[0].Rows.Count == 1)
                            anomalyData.Tables[0].Rows[0][1] = "OK";
                        else
                            anomalyData.Tables[0].Rows.Add("NVM Status", "OK");
                    }
                    // NVM
                    //******* Meter Model Change Required Here ***********//
                    // RTC Status
                    if (MeterModelNumber != 11 && MeterModelNumber != 17 && MeterModelNumber != 18 && MeterModelNumber != 32 && MeterModelNumber != 24 && MeterModelNumber != 25 && MeterModelNumber != 13 && MeterModelNumber != 8 && MeterModelNumber != 16 && MeterModelNumber != 23 && MeterModelNumber != NamePlateConstants.SFSP && MeterModelNumber != NamePlateConstants.VFSPNoSeasonNoWeek && MeterModelNumber != NamePlateConstants.ThreeTOUWCMValue && MeterModelNumber != NamePlateConstants.SapphireWCM_St && MeterModelNumber != NamePlateConstants.SapphireS2 && MeterModelNumber != 45 && MeterModelNumber != 46)// ADD 3PH THREE TOU 
                    {
                        anomalyData.Tables[0].Rows.Add("RTC Status", anomalyWBTable.Rows[3][1]);
                    }

                    // RTC Status

                    // Battery Status 
                    if (MeterModelNumber == 9)
                    {
                        anomalyData.Tables[0].Rows.Add("Battery Status", "OK");
                    }
                    //******* Meter Model Change Required Here ***********//
                    else if (MeterModelNumber == 8 || MeterModelNumber == 16 || MeterModelNumber == 23 || MeterModelNumber == 36 || MeterModelNumber == NamePlateConstants.SFSP || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek)//single phase
                    {
                        anomalyData.Tables[0].Rows.Add("RTC Status", anomalyWBTable.Rows[3][1]);
                        if (ConfigInfo.ActiveFileType == "DLMS")
                        {
                            if (MeterModelNumber == 19 || MeterModelNumber == 26)
                            { }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("Battery Status", anomalyWBTable.Rows[4][1]);
                            }
                        }
                    }
                    else if (MeterModelNumber == 11 || MeterModelNumber == 17 || MeterModelNumber == 18 || MeterModelNumber == 32 || MeterModelNumber == 13 || MeterModelNumber == 24 || MeterModelNumber == 25 || MeterModelNumber == 33 || MeterModelNumber == 34 || MeterModelNumber == 35 || MeterModelNumber == 36 || MeterModelNumber == 37 || MeterModelNumber == 43 || MeterModelNumber == 45 || MeterModelNumber == 46)/// added for sapphire . Only work for sapphire 3ph 
                    {
                        if (anomalyWBTable.Rows[2]["Status"].ToString() == "OK")
                        {
                            anomalyData.Tables[0].Rows.Add("Power Supply", "OK");
                        }
                        else
                        {
                            anomalyData.Tables[0].Rows.Add("POWER SUPPLY", "NOT OK");
                        }
                        
                        if (MeterModelNumber == 24 || MeterModelNumber == 25 || MeterModelNumber == 34 || MeterModelNumber == 35 || MeterModelNumber == 36 || MeterModelNumber == 37)//for all Smart meter
                        {
                            DataRow[] PowerSupply = anomalyData.Tables[0].Select("Parameters = 'POWER SUPPLY'");
                            anomalyData.Tables[0].Rows.Remove(PowerSupply[0]);
                            anomalyData.AcceptChanges();

                            if (anomalyWBTable.Rows[3]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Battery Status", "OK");
                            }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Battery Status", "NOT OK");
                            }
                            if (anomalyWBTable.Rows[5]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("Main Battery Status", "OK");
                            }
                            else
                           {
                                anomalyData.Tables[0].Rows.Add("Main Battery Status", "NOT OK");
                           }
                            

                        }
                        else
                        {
                            if (anomalyWBTable.Rows[3]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Status", "OK");
                            }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Status", "NOT OK");
                            }

                            if (anomalyWBTable.Rows[4]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Battery Status", "OK");
                            }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("RTC Battery Status", "NOT OK");
                            }

                            if (anomalyWBTable.Rows[5]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("Main Battery Status", "OK");
                            }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("Main Battery Status", "NOT OK");
                            }
                         
                            if (anomalyWBTable.Rows[6]["Status"].ToString() != "0" && MeterModelNumber == 17)
                            {
                                anomalyData.Tables[0].Rows.Add("Error Code Status", "ERROR 000" + anomalyWBTable.Rows[6]["Status"].ToString());
                            }                                   

                        }
                    }
                    if (MeterModelNumber == 23 ) //Hardcoded for 1P Smart meter(Command is not implemented in meter)
                    {
                        if (anomalyData.Tables[0].Rows.Count > 1)
                            anomalyData.Tables[0].Rows[1][1] = "OK";
                        else
                            anomalyData.Tables[0].Rows.Add("RTC Status", "OK");
                    }
                    if (MeterModelNumber == 8) //For 1P IEC, FLASH is used for Error Code Display
                    {
                        if (anomalyWBTable.Rows[0]["Status"].ToString() == "NOT OK")
                        {
                            anomalyData.Tables[0].Rows.Add("ERROR Code", "E005");
                        }
                        if (anomalyWBTable.Rows[5]["Status"].ToString() == "/")// For CSPDCL
                        {
                            if (anomalyWBTable.Rows[4]["Status"].ToString() == "OK")
                            {
                                anomalyData.Tables[0].Rows.Add("Meter Battery Status", "OK");
                            }
                            else
                            {
                                anomalyData.Tables[0].Rows.Add("Meter Battery Status", "NOT OK");
                            }
                        }
                    }
                    /// added for sapphire 

                }
                if (MeterModelNumber != 11 && MeterModelNumber != 17 && MeterModelNumber != 18 && MeterModelNumber != 32 && MeterModelNumber != 13 && MeterModelNumber != 24 && MeterModelNumber != 25 && MeterModelNumber != 34 && MeterModelNumber != 35 && MeterModelNumber != 45 && MeterModelNumber != 46)
                {
                    DataRow[] rowRTCBatteryStatus = anomalyData.Tables[0].Select("Parameters = 'RTC Battery'");
                    if (rowRTCBatteryStatus != null && rowRTCBatteryStatus.Length > 0)
                    {
                        anomalyData.Tables[0].Rows.Remove(rowRTCBatteryStatus[0]);
                        anomalyData.AcceptChanges();
                    }
                }
                DataRow[] rowMainBatteryStatus = anomalyData.Tables[0].Select("Parameters = 'MAIN BATTERY'");
                if (rowMainBatteryStatus != null && rowMainBatteryStatus.Length > 0)
                {

                    anomalyData.Tables[0].Rows.Remove(rowMainBatteryStatus[0]);
                    anomalyData.AcceptChanges();
                }   
                // for remove power supply status 1ph meter model no 31 

                if (MeterModelNumber == 31)
                {
                    DataRow[] rowPowerSupplyStatus = anomalyData.Tables[0].Select("Parameters = 'POWER SUPPLY'");
                    if (rowPowerSupplyStatus != null && rowPowerSupplyStatus.Length > 0)
                    {

                        anomalyData.Tables[0].Rows.Remove(rowPowerSupplyStatus[0]);
                        anomalyData.AcceptChanges();
                }


                }
                
                // for remove power supply status 1ph meter model no 31 

                if (MeterModelNumber == 31)
                {
                    DataRow[] rowPowerSupplyStatus = anomalyData.Tables[0].Select("Parameters = 'POWER SUPPLY'");
                    if (rowPowerSupplyStatus != null && rowPowerSupplyStatus.Length > 0)
                    {

                        anomalyData.Tables[0].Rows.Remove(rowPowerSupplyStatus[0]);
                        anomalyData.AcceptChanges();
                    }

                    
                }

                //end 
                //-----------Error code will be only for meter model 17
                    DataRow[] rowerrorStatus = anomalyData.Tables[0].Select("Parameters = 'ErrorCodeStatus'");
                    if (rowerrorStatus != null && rowerrorStatus.Length > 0)
                    {

                        anomalyData.Tables[0].Rows.Remove(rowerrorStatus[0]);
                        anomalyData.AcceptChanges();
                    }

                   
                 
                lngGridAnomaly.Data = anomalyData;
            }
            else
            {

            }
        }

        /// <summary>
        /// This method is used to fetch the billing data from DB and to show it in analysis report.
        /// </summary>
        private void FillBillingParameters()
        {
            try
            {
                DataSet dataSet = new DataSet();
                // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
				// Commented the below meter model check for WBSDCL supply demand
                //if (MeterModelNumber == 9)
                //{
                //    dataSet = billingBLL.GetCumulativeEnergy(Convert.ToInt32(MeterDataID), false, MeterModelNumber);// Story - 490966
                //}
                //else
                //{
                    dataSet = billingBLL.GetCumulativeEnergy(Convert.ToInt32(MeterDataID), false);// Story - 365971 - 13 billing for Power ON Hours
               // }
                //  dataSet = billingBLL.GetCumulativeEnergy(Convert.ToInt32(MeterDataID),false);// Story - 365971 - 13 billing for Power ON Hours
                //Append billing month with history data 
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                lngMainEnergy.Data = dataSet;
                string RemoveLagClmName = string.Empty;
                string RemoveLeadClmName = string.Empty;
                string removeBillingType = string.Empty;
                if (dataSet != null)
                {
                    lngMainEnergy.SetWidth("History", 90);
                    lngMainEnergy.SetWidth("Billing DateTime (0.0.0.1.2.255;3;2)", 190);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH), 130);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH), 130);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG), 160);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD), 160);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag), 130);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead), 130);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag), 130);
                    lngMainEnergy.SetWidth(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead), 130);

                    //Dynamic Column Hide for Billing Type
                    if (dataSet.Tables != null && dataSet.Tables.Count > 0)
                    {
                        DataTable dtDLMS650MainEnergyTable = dataSet.Tables[0];
                        if (dtDLMS650MainEnergyTable != null && dtDLMS650MainEnergyTable.Columns != null && dtDLMS650MainEnergyTable.Columns.Contains(Constants.conBillingType))
                        {
                            bool flag = true;
                            foreach (DataRow itemRow in dtDLMS650MainEnergyTable.Rows)
                            {
                                if (Convert.ToString(itemRow[Constants.conBillingType]).Trim() != string.Empty)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                lngMainEnergy.SetVisibility(Constants.conBillingType, false);
                            }
                        }
                    }

                    //Checking for WB MeterModelNumber =9 or TNEB MeterModelNumber = 13 or IEC meter or SF meterModel = 26
                    //if (!(MeterModelNumber == 9 || MeterModelNumber == 13 || ConfigInfo.ActiveFileType == BCSConstants.IEC))
                    //{
                    //    lngMainEnergy.SetVisibility(Constants.conBillingType, false);                    
                    //}

                    //******* Meter Model Change Required Here ***********//
                    // Added new Meter model no 16 check
                    if ((MeterModelNumber == 8 
                        || MeterModelNumber == 16 
                        || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek 
                        || MeterModelNumber == NamePlateConstants.SFSP 
                        || MeterModelNumber == NamePlateConstants.BYPL_FD 
                        || MeterModelNumber == NamePlateConstants.BRPL_CBSP //user story 1016689
                        ) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        lngMainEnergy.SetVisibility(Constants.kvarhLag, false);
                        lngMainEnergy.SetVisibility(Constants.kvarhLead, false);
                        lngMainEnergy.SetVisibility(Constants.conBillingType, false);
                    }  

                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter. User story no 474879
                    foreach (DataColumn item in dataSet.Tables[0].Columns)
                    {
                        if (item.ColumnName.Contains("Lag") && (dataSet.Tables[0].Rows.Count > 0) && Convert.ToString(dataSet.Tables[0].Rows[0][item.ColumnName]) == string.Empty)
                        {
                            RemoveLagClmName = item.ColumnName;
                        }
                        if (item.ColumnName.Contains("Lead") && (dataSet.Tables[0].Rows.Count > 0) && Convert.ToString(dataSet.Tables[0].Rows[0][item.ColumnName]) == string.Empty)
                        {
                            RemoveLeadClmName = item.ColumnName;
                        }
                    }
                    if (RemoveLagClmName != string.Empty)
                    {
                        dataSet.Tables[0].Columns.Remove(RemoveLagClmName);
                    }
                    if (RemoveLeadClmName != string.Empty)
                    {
                        dataSet.Tables[0].Columns.Remove(RemoveLeadClmName);
                    }

                    
                    if (dataSet != null)        //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                        common.GetReactive(dataSet.Tables[0], "billing");
                }

                //Billing Energy
                dataSet = billingBLL.GetCumulativeEnergyCalculated(Convert.ToInt32(MeterDataID));
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Double");
                lngBillingEnergyConsumption.Data = dataSet;
                if (dataSet != null)
                {
                    //Set the width of Grid Column.
                    lngBillingEnergyConsumption.SetWidth(0, 170);
                    lngBillingEnergyConsumption.SetWidth(1, 170);
                    lngBillingEnergyConsumption.SetWidth(2, 170);
                    lngBillingEnergyConsumption.SetWidth(3, 190);
                    lngBillingEnergyConsumption.SetWidth(4, 190);
                    //******* Meter Model Change Required Here ***********//
                    // Added new Meter model no 16 check
                    if ((MeterModelNumber == 8 
                        || MeterModelNumber == 16 
                        || MeterModelNumber == NamePlateConstants.SFSP 
                        || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek 
                        || MeterModelNumber == NamePlateConstants.BYPL_FD
                        || MeterModelNumber == NamePlateConstants.BRPL_CBSP //user story 1016689
                        ) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        lngBillingEnergyConsumption.SetVisibility(Constants.kvarhLag, false);
                        lngBillingEnergyConsumption.SetVisibility(Constants.kvarhLead, false);
                    }

                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter. User story no 474879 
                    RemoveLagClmName = string.Empty;
                    RemoveLeadClmName = string.Empty;
                    foreach (DataColumn item in dataSet.Tables[0].Columns)
                    {
                        if (item.ColumnName.Contains("Lag") && (dataSet.Tables[0].Rows.Count > 0) && Convert.ToString(dataSet.Tables[0].Rows[0][item.ColumnName]) == string.Empty)
                        {
                            RemoveLagClmName = item.ColumnName;
                        }
                        if (item.ColumnName.Contains("Lead") && (dataSet.Tables[0].Rows.Count > 0) && Convert.ToString(dataSet.Tables[0].Rows[0][item.ColumnName]) == string.Empty)
                        {
                            RemoveLeadClmName = item.ColumnName;
                        }
                    }
                    if (RemoveLagClmName != string.Empty)
                    {
                        dataSet.Tables[0].Columns.Remove(RemoveLagClmName);
                    }
                    if (RemoveLeadClmName != string.Empty)
                    {
                        dataSet.Tables[0].Columns.Remove(RemoveLeadClmName);
                    }

                    
                    if (dataSet != null)        //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                        common.GetReactive(dataSet.Tables[0], "billing");
                }

                //Maximum demand
                dataSet = billingBLL.GetMaximumDemand(Convert.ToInt32(MeterDataID));
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                lngMaximumDemand.Data = dataSet;

                //Cumulative MD Add for smart meter
                dataSet = billingBLL.GetCumulativeMaximumDemand(Convert.ToInt32(MeterDataID));
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");

                dataSet = ReplaceEmptyStringWithDash(dataSet);  //SarkarA code change 20180530 //Provide dash notation for zero value 

                lngCumulativeMD.Data = dataSet;
                if (dataSet != null)
                {
                    //Set the width of Grid Column.
                    lngCumulativeMD.SetWidth(0, 120);
                    lngCumulativeMD.SetWidth(1, 150);
                    lngCumulativeMD.SetWidth(2, 150);
                    lngCumulativeMD.SetWidth(3, 150);
                    
                }


                //dataSet = common.Transaction(Convert.ToInt32(MeterDataID));
                //lngTransaction.Data = dataSet;

                //VBM - Power off duration 
                dataSet = billingBLL.GetPowerOffDuration(Convert.ToInt32(MeterDataID));
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                lngGrdPowerOffDuration.Data = dataSet;
                if (dataSet != null)
                {
                    lngGrdPowerOffDuration.SetWidth("History", 90);
                    lngGrdPowerOffDuration.SetWidth("Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm", 150);
                    lngGrdPowerOffDuration.SetWidth("Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm", 130);
                }
                lngGrdPowerOffDuration.IsSorting = false;

                //power on duration
                dataSet = billingBLL.GetPowerOnDuration(Convert.ToInt32(MeterDataID));
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                // Story - 349654 - Getting exception in case data set is null, so this condition is moved indife if clause
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    //******* Meter Model Change Required Here ***********//
                    if ((MeterModelNumber == 8
                        || MeterModelNumber == 16
                        || MeterModelNumber == NamePlateConstants.SFSP
                        || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek
                        || MeterModelNumber == NamePlateConstants.BYPL_FD
                        || MeterModelNumber == NamePlateConstants.BRPL_CBSP //user story 1016689
                        ) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        dataSet.Tables[0].Rows[0][2] = "----";
                        dataSet.Tables[0].Rows[0][3] = "----";
                    }

                    lngPowerOnDuration.Data = dataSet;
                    lngPowerOnDuration.SetWidth(0, 90);
                    lngPowerOnDuration.SetWidth(1, 230);
                    lngPowerOnDuration.SetWidth(2, 300);
                    lngPowerOnDuration.SetWidth(3, 300);
                    lngPowerOnDuration.RefreshGrid();
                }
                else
                {
                    tabControl2.TabPages.Remove(tabPagePowerOnDuration);
                }
                //Power factor
                dataSet = billingBLL.GetAveragePowerFactor(Convert.ToInt32(MeterDataID));
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                lngAveragePowerFactor.Data = dataSet;
                if (lngAveragePowerFactor.Data != null)
                {
                    lngAveragePowerFactor.SetWidth(0, 90);
                    lngAveragePowerFactor.SetWidth(1, 230);
                    lngAveragePowerFactor.RefreshGrid();
                }
                else
                {
                    tabControl2.TabPages.Remove(tabPageBillingPowerFactor);
                }
                /*Load Factor */

                //Only showing load factor in case its coming from the meter, otherwise removing the tab.
                dataSet = billingBLL.GetBillingAverageLoadFactor(Convert.ToInt32(MeterDataID));
                //Story no: 490966 WB tender specific check implemented for Average Load factor OBIS code change
                //if (MeterModelNumber == 9)
                //{
                //    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null && dataSet.Tables[0].Columns != null)
                //    {
                //        DataColumn dc = dataSet.Tables[0].Columns[BCSConstants.LoadFactorColumn];
                //        // Commented the below meter model check for WBSDCL supply demand
                //        //dc.ColumnName = BCSConstants.LoadFactorColumn_WB;
                //    }
                //}

                //Story - 365876 - Load Factor calculation                
                DataSet Met_Cat = new DataSet();
                Met_Cat = billingBLL.GetMeterCategory(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                foreach (DataRow dataRow in Met_Cat.Tables[0].Rows)
                {
                    meter_cat = Convert.ToString(dataRow["Category"]);
                }
                if ((ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS" || ConfigInfo.ActiveFileType.ToUpper() == "DLMS") && ConfigInfo.ActiveMeterType.ToUpper() == "1P-2W" && dataSet == null)
                { 
                    dataSet = billingBLL.GetBillingAverageLoadFactorCalculated(Convert.ToInt32(MeterDataID));
                }
                if (dataSet != null)
                {
                }
                else
                {

                    if ((ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS" || ConfigInfo.ActiveFileType.ToUpper() == "DLMS"))
                    {

                        dataSet = billingBLL.GetBillingAverageLoadFactorCalculated(Convert.ToInt32(MeterDataID));
                        //foreach  (DataColumn item in dataSet.Tables[0].Columns)
                        //{
                        //if (item.ColumnName.Contains("kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"))
                        
                    }

                }

                if (meter_cat == "B8" || meter_cat == "B2" )
                {
                    dataSet = billingBLL.GetBillingAverageLoadFactorCalculated(Convert.ToInt32(MeterDataID));
                }

             
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 1)
                {

                    if (dataSet.Tables[0].Columns.Contains("kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"))
                    {
                        if (dataSet.Tables[0].Rows[0]["kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"].ToString() == "")
                        {
                            dataSet.Tables[0].Columns.Remove("kW Import Load Factor (%) (1.0.1.0.128.255;3;2)");
                        }

                    }

                    if (dataSet.Tables[0].Columns.Contains("kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"))
                    {
                        if (dataSet.Tables[0].Rows[0]["kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"].ToString() == "")
                        {
                            dataSet.Tables[0].Columns.Remove("kW Export Load Factor (%) (1.0.2.0.128.255;3;2)");
                        }

                    }

                    if (dataSet.Tables[0].Columns.Contains("kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"))
                    {
                        if (dataSet.Tables[0].Rows[0]["kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"].ToString() == "")
                        {
                            dataSet.Tables[0].Columns.Remove("kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)");
                        }

                    }

                    if (dataSet.Tables[0].Columns.Contains("kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"))
                    {
                        if (dataSet.Tables[0].Rows[0]["kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"].ToString() == "")
                        {
                            dataSet.Tables[0].Columns.Remove("kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)");
                        }

                    }
                    dataSet.Tables[0].AcceptChanges();
                }
                //}

               
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
               
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    lngLoadFactor.Data = dataSet;
                    lngLoadFactor.SetWidth("History", 90);
                    //Story - 365876 - Load Factor calculation
                    if (ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
                        lngLoadFactor.SetWidth(BCSConstants.LoadFactorColumnForSLG, 200);
                    else
                        lngLoadFactor.SetWidth(BCSConstants.LoadFactorColumn, 200);
                }
                else
                {
                    tabControl2.TabPages.Remove(tabPageLoadFactor);
                }

                //Billing Transaction (Auto, CMRI/BCS, Manual)
                // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                //if (MeterModelNumber == 9)
                //{
                //    dataSet = billingBLL.GetBillingTransaction(Convert.ToInt32(MeterDataID), MeterModelNumber);
                //}
                //else
                //{
                    dataSet = billingBLL.GetBillingTransaction(Convert.ToInt32(MeterDataID));
                //}
                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    lngBillingTransaction.Data = dataSet;
                    lngBillingTransaction.SetWidth(0, 90);
                    lngBillingTransaction.SetWidth(1, 150);
                    lngBillingTransaction.SetWidth(2, 150);
                    lngBillingTransaction.RefreshGrid();
                }
                else
                {
                    tabControl2.TabPages.Remove(tabPageBillingTransaction);
                }

                //To Hide TOD Power factor tab, if no data is available.
                dataSet = billingBLL.GetTODAvgPF(Convert.ToInt32(MeterDataID), 1, false);
                if (dataSet == null)
                {
                    // Block this line due to calculate TOF PF
                    //tabControl9.TabPages.Remove(tabPageTODAvgPowerFactor);
                }

                #region "Average Load calculation"
                //Story - ?? - Average Load calculation

                dataSet = billingBLL.GetBillingAverageLoad(Convert.ToInt32(MeterDataID));

                //Append billing month with history data
                dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    lngAvgLoad.Data = dataSet;
                    lngAvgLoad.SetWidth("History", 90);
                    lngAvgLoad.SetWidth(BCSConstants.AverageLoadColumn, 200);
                }
                else
                {
                    tabControl2.TabPages.Remove(tabPageAvgLoad);
                }
                #endregion


            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillBillingParameters()", ex);

            }
        }
        /// <summary>
        /// To Append billing month with history value
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="billingData"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataSet ShowBillingMonth(int meterDataId, DataSet billingData, string mode)
        {
            DataSet finalDS = null;
            try
            {
                if (billingData != null && billingData.Tables.Count > 0 && billingData.Tables[0].Rows.Count > 0)
                {
                    //Single use for the single history value shown in history column
                    if (mode == "Single")
                    {
                        string columnValue = string.Empty;
                        string billingDate = string.Empty;
                        string month = string.Empty;

                        finalDS = billingData;
                        for (int i = 0; i < finalDS.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                columnValue = finalDS.Tables[0].Rows[i][0].ToString();
                                billingDate = finalDS.Tables[0].Rows[i][1].ToString().Trim().Split(' ')[0];
                                DateTime DT = DateTime.ParseExact(billingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                month = string.Format("{0:MMM}", DT);
                                finalDS.Tables[0].Rows[i][0] = billingData.Tables[0].Rows[i][0].ToString() + " (" + month + ")";
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                finalDS.Tables[0].Rows[i][0] = billingData.Tables[0].Rows[i][0].ToString() + " (---)";
                                logger.Log(LOGLEVELS.Error, "ShowBillingMonth(int meterDataId, DataSet billingData, string mode), nullhandling", ex);
                            }
                        }
                    }
                    else if (mode == "Double") //double used for the paired history value for history column
                    {
                        string currentMonth = string.Empty;
                        string previousMonth = string.Empty;
                        DataSet dataSet = billingBLL.GetBillingMonths(Convert.ToInt32(meterDataId));
                        finalDS = billingData;
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count - 1; i++)
                        {
                            try
                            {
                                currentMonth = dataSet.Tables[0].Rows[i]["BillingMonth"].ToString();
                                previousMonth = dataSet.Tables[0].Rows[i + 1]["BillingMonth"].ToString();
                                if (currentMonth == string.Empty)
                                    currentMonth = "---";
                                if (previousMonth == string.Empty)
                                    previousMonth = "---";
                                finalDS.Tables[0].Rows[i][0] = billingData.Tables[0].Rows[i][0].ToString() + " (" + currentMonth + " - " + previousMonth + ")";
                                // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
                                if (finalDS.Tables[0].Rows.Count > 13)
                                {
                                    if (billingData.Tables[0].Rows[i][0].ToString().Contains("History - 61")) // Story - To set 60th billing consumption history
                                        billingData.Tables[0].Rows[i][0] = "History - 60 - Initial";
                                }
                                else
                                {
                                    if (billingData.Tables[0].Rows[i][0].ToString().Contains("History - 13")) // Story - To set 12th billing consumption history
                                        billingData.Tables[0].Rows[i][0] = "History - 12 - Initial";
                                }
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                finalDS.Tables[0].Rows[i][0] = billingData.Tables[0].Rows[i][0].ToString() + " (---)";
                                logger.Log(LOGLEVELS.Error, "ShowBillingMonth(int meterDataId, DataSet billingData, string mode), nullhandling", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowBillingMonth(int meterDataId, DataSet billingData, string mode)", ex);
            }
            return finalDS;
        }
        private MeterDataTypes getMeterDataType()
        {
            DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
            return generalBLL.GetMeterDataType(ConfigInfo.ActiveMeterDataId);

        }

        private string getMeterType()
        {
            DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
            return generalBLL.GetMeterType(ConfigInfo.ActiveMeterDataId);
        }

        /// <summary>
        /// This method is used to fetch the phasor data from DB and to show it in analysis report.
        /// </summary>
        private void FillPhasor()
        {
            DataSet dataSet = new DataSet();
            PhasorEntity phEntity = new DLMS650PhasorBLL().GetPhasorDataEntity(Convert.ToInt32(MeterDataID)) as PhasorEntity;
            if (phEntity != null)
            {
                if (string.IsNullOrEmpty(phEntity.PhaseSequence) || phEntity.PhaseSequence.ToUpper() == "INCORRECT")
                {
                    lbkNoDataFound.Visible = true;
                    lbkNoDataFound.Text = "Phase Sequence is not correct. Phasor can not be shown";
                    lngPhasorDiagram.Visible = false;
                    iecPhasorDiagram.Visible = false;
                }
                else
                {
                    lbkNoDataFound.Visible = false;
                    if (ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        lngPhasorDiagram.Visible = false;
                        iecPhasorDiagram.Visible = true;
                        iecPhasorDiagram.PhasorData = phEntity;
                    }
                    else
                    {
                        iecPhasorDiagram.Visible = false;
                        lngPhasorDiagram.Visible = true;
                        lngPhasorDiagram.PhasorData = phEntity;
                    }
                }
            }

            //if (UtilityDetails.ShowMeterModelNo)
            //{
            int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);
            //}
            //if (meterModelNumber == NamePlateConstants.RubyE250Value)
            //{
            //    lblPhasorNotSupported.Visible = true;
            //    lblPhasorNotSupported.Text = "* Phasor is not supported in "+NamePlateConstants.RubyE250+" meter.";
            //}
            //else
            //{
            DataSet dsPhasorData = new DLMS650PhasorBLL().GetPhasorDataSet(Convert.ToInt32(MeterDataID));

            if (dsPhasorData != null)
            {
                lngPhasorData.Data = common.GetPhasorColumnToRow(dsPhasorData, Convert.ToInt64(MeterDataID));
                lngPhasorData.SetWidth("Parameters", 150);
                lngPhasorData.SetWidth("Values", 110);
                lngPhasorData.RefreshGrid();
            }
            //}
        }
        /// <summary>
        /// This method is used to fetch the loadsurvey data from DB and to show it in analysis report.
        /// </summary>
        private void FillLoadSurvey()
        {
			DateTime errorDate = new DateTime(1900, 1, 1, 12, 0, 0);
			long lsFromDate = loadSurveyBLL.GetFromDate(Convert.ToInt64(MeterDataID));
            long lsToDate = loadSurveyBLL.GetToDate(Convert.ToInt64(MeterDataID));

            DateTime fromDate, toDate;

            if (lsFromDate != 0)
            {
                fromDate = DateUtility.LongToDateTime(lsFromDate);
                if (fromDate == DateTime.MinValue)
                    dtpFromDate.Value = errorDate;
                else
                    dtpFromDate.Value = fromDate;
            }

            if (lsToDate != 0)
            {
                toDate = DateUtility.LongToDateTime(lsToDate);
                if (toDate == DateTime.MinValue)
                    dtpToDate.Value = DateTime.MaxValue.AddYears(-1);
                else
                    dtpToDate.Value = toDate;
            }


            if (dtpToDate.Value == errorDate || dtpFromDate.Value == errorDate)
            {
                this.StatusMessage = "Load survey data corrupt";
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
                AR_btnShowLoadSurvey.Enabled = true;
                button1.Enabled = true;
                return;
            }
        }
        private void FillLoadSwitch()
        {
            try
            {
                LoadSwitchBLL loadswitchBLL = new LoadSwitchBLL();
                DataSet LoadswData = loadswitchBLL.GetMeterData(Convert.ToInt32(MeterDataID));
                lngLoadSwitch.Data = LoadswData;
                if (lngLoadSwitch.Data != null)
                {
                    lngLoadSwitch.SetWidth("Descriptions", 250);
                    lngMidnightEnergy.SetWidth(0, 450);
                    lngMidnightEnergy.SetWidth(1, 450);
                    lngMidnightEnergy.SetWidth(2, 350);
                    lngMidnightEnergy.SetWidth(3, 350);
                    lngMidnightEnergy.SetWidth(4, 350);
                    lngMidnightEnergy.SetWidth(5, 350);
                    lngMidnightEnergy.SetWidth(6, 350);
                    lngMidnightEnergy.SetWidth(7, 350);
                    lngMidnightEnergy.SetWidth(8, 350);
                    lngMidnightEnergy.SetWidth(14, 350);
                    lngMidnightEnergy.SetWidth(15, 350);
                    lngMidnightEnergy.SetWidth(16, 350);
                    lngMidnightEnergy.SetWidth(17, 350);
                    lngMidnightEnergy.SetWidth(18, 350);
                    lngMidnightEnergy.SetWidth(19, 350);
                    lngMidnightEnergy.SetWidth(20, 350);

                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "FillLoadSwitch()", ex);
            }
        }
        /// <summary>
        /// This method is used to fetch the midnight data from DB and to show it in analysis report.
        /// </summary>
        private void FillMidNightData()
        {
			DateTime errorDate = new DateTime(1900, 1, 1, 12, 0, 0);

			long lsFromDateMD = loadSurveyBLL.GetFromDate(Convert.ToInt64(MeterDataID));
            long lsToDateMD = loadSurveyBLL.GetToDate(Convert.ToInt64(MeterDataID));

            DateTime fromDate, toDate;

            if (lsFromDateMD != 0)
			{
                //dtpFromMD.Value = DateUtility.LongToDateTime(lsFromDateMD);
                fromDate = DateUtility.LongToDateTime(lsFromDateMD);
                if (fromDate == DateTime.MinValue)
                    dtpFromMD.Value = errorDate;
                else
                    dtpFromMD.Value = fromDate;
            }

            if (lsToDateMD != 0)
            {
                //dtpToMD.Value = DateUtility.LongToDateTime(lsToDateMD);
                toDate = DateUtility.LongToDateTime(lsToDateMD);
                if (toDate == DateTime.MinValue)
                    dtpToMD.Value = DateTime.MaxValue.AddYears(-1);
                else
                    dtpToMD.Value = toDate;
            }

            if (dtpToMD.Value == errorDate || dtpFromMD.Value == errorDate)
            {
                this.StatusMessage = "MidNight Data corrupt";
                dtpFromMD.Enabled = false;
                dtpToMD.Enabled = false;
                btnShow.Enabled = false;
                return;
            }
        }
        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }
        private string GetFileName(long meterDataID)
        {
            DataSet fileDataset = new DataSet();
            string fileName = string.Empty;
            fileDataset = ListFileName(meterDataID);
            if (fileDataset != null)
            {
                if (fileDataset.Tables[0].Rows.Count > 0)
                {
                    fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                }
            }
            return fileName;
        }

        /// <summary>
        /// This method is used to fetch the daily consumption data from DB and to show it in analysis report.
        /// </summary>
        private void FillDailyConsumption()
        {
            DataSet dataSet = new DataSet();
            long lsFromDateMD = loadSurveyBLL.GetFromDate(Convert.ToInt64(MeterDataID));
            long lsToDateMD = loadSurveyBLL.GetToDate(Convert.ToInt64(MeterDataID));
            // Added to solve dailu consumption data difference in fast download and direct read out issue 
            string fileName = string.Empty;
            fileName = GetFileName(Convert.ToInt64(MeterDataID));
            string strHeaderText = "Daily Consumption";

            DLMS650MidnightDataBLL dlms650MidNightDataBLL = new DLMS650MidnightDataBLL();
            dataSet = dlms650MidNightDataBLL.GetGenericMidNightConsumptionData(Convert.ToInt64(MeterDataID), descendingOrder);
            tabDailyConsmption.Text = "Midnight Consumption";
            strHeaderText = "Midnight Consumption";

            string DAILYKWH = string.Empty;
            string DAILYKVAH = string.Empty;
            string DAILYKVARHLAG = string.Empty;
            string DAILYKVARHLEAD = string.Empty;

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                // To solve bug 94243.
                if (dataSet.FirstTableHasRows())
                {
                    if (dataSet.Tables[0].Columns.Contains("Maximum Demand - kW (1.0.1.6.0.255;4;2)"))
                        dataSet.Tables[0].Columns.Remove("Maximum Demand - kW (1.0.1.6.0.255;4;2)");
                    if (dataSet.Tables[0].Columns.Contains("Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)"))
                        dataSet.Tables[0].Columns.Remove("Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)");
                    if (dataSet.Tables[0].Columns.Contains("Maximum Demand - kVA (1.0.9.6.0.255;4;2)"))
                        dataSet.Tables[0].Columns.Remove("Maximum Demand - kVA (1.0.9.6.0.255;4;2)");
                    if (dataSet.Tables[0].Columns.Contains("Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)"))
                        dataSet.Tables[0].Columns.Remove("Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)");
                    //these three parameters are only for midnight energies not for daily consumption
                    //if (dataSet.Tables[0].Columns.Contains("Power On Duration (0.0.94.91.13.255;3;2) dd:hh:mm"))
                    //    dataSet.Tables[0].Columns.Remove("Power On Duration (0.0.94.91.13.255;3;2) dd:hh:mm");
                    // Name change for APSPDCL : Daily Survey Requirement
                    //if (dataSet.Tables[0].Columns.Contains("Power On Duration 1 or 2 Phases (1.0.96.0.165.255;3;2) dd:hh:mm")) // OBIS Code changed for APSPDCL : Daily Survey Requirement
                    //    dataSet.Tables[0].Columns.Remove("Power On Duration 1 or 2 Phases (1.0.96.0.165.255;3;2) dd:hh:mm");
                    if (dataSet.Tables[0].Columns.Contains("Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm"))
                        dataSet.Tables[0].Columns.Remove("Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm");
                    if (dataSet.Tables[0].Columns.Contains("Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm"))
                        dataSet.Tables[0].Columns.Remove("Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm");
                    //if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm")) // New OBIS Code added for JVVNL : Daily Survey Requirement
                    //dataSet.Tables[0].Columns.Remove("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm");



                    lngDailyConsumption.Data = dataSet;
                    lngDailyConsumption.SetWidth(0, 160);
                    lngDailyConsumption.SetWidth(1, 125);
                    lngDailyConsumption.SetWidth(2, 125);
                    lngDailyConsumption.SetWidth(3, 125);
                    lngDailyConsumption.SetWidth(4, 125);
                    lngDailyConsumption.SetWidth(5, 125);
                    lngDailyConsumption.SetWidth(6, 125);
                }


            }
        }
        /// <summary>
        /// This method is used to fetch the midnight energies from DB and to show it in analysis report.
        /// </summary>
        private void FillMidnightEnergies()
        {
            DataSet dataSet = new DataSet();
            //if (UtilityDetails.ShowMidnight)
            //{
            if (isPUMA) //Puma Code base 
            {
                DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                dataSet = loadSurveyBLL.GetPUMAMidNightData(Convert.ToInt64(MeterDataID));

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 || dataSet.Tables[0].Rows.Count > 0)
                    {
                        lngMidnightEnergy.Data = dataSet;
                        lngMidnightEnergy.SetWidth(0, 175);
                        lngMidnightEnergy.SetWidth(1, 200);
                        lngMidnightEnergy.SetWidth(2, 175);
                        lngMidnightEnergy.SetWidth(3, 175);
                        lngMidnightEnergy.SetWidth(4, 175);
                        lngMidnightEnergy.SetWidth(5, 175);
                        lngMidnightEnergy.SetWidth(6, 175);
                        lngMidnightEnergy.SetWidth(7, 220);
                    }
                }
            }
            else // Ruby Code base 
            {
                DLMS650MidnightDataBLL midnightDataBLL = new DLMS650MidnightDataBLL();
                dataSet = midnightDataBLL.GetMidNightData(Convert.ToInt64(MeterDataID));

                //VBM - Commenting this code as it applies EMF twice .
                // dataSet = common.ApplyMultiplyFactor(Convert.ToInt64(MeterDataID), dataSet);
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 || dataSet.Tables[0].Rows.Count > 0)
                    {
                        lngMidnightEnergy.Data = dataSet;
                        lngMidnightEnergy.SetWidth("Date (0.0.1.0.0.255;8;2)", 175);
                        lngMidnightEnergy.SetWidth("Cumulative Energy - kWh (1.0.1.8.0.255;3;2)", 200);
                        lngMidnightEnergy.SetWidth("Cumulative Energy - kvarh (lag) (1.0.5.8.0.255;3;2)", 200);
                        lngMidnightEnergy.SetWidth("Cumulative Energy - kvarh (lead) (1.0.8.8.0.255;3;2)", 200);
                        lngMidnightEnergy.SetWidth("Cumulative Energy - kVAh (1.0.9.8.0.255;3;2)", 200);
                    }
                }
            }
        }
        //}
        /// <summary>
        /// This method is used to fetch the midnight energies from DB and bind with Midnight data Grid
        /// </summary>
        private void FillGenericMidnightEnergies()
        {
            DLMS650MidnightDataBLL midNightBLL = new DLMS650MidnightDataBLL();
            DataSet midNightDataSet = midNightBLL.GetGenericMidNightData(Convert.ToInt64(MeterDataID), descendingOrder);

            if (midNightDataSet != null)
            {
                if (midNightDataSet.Tables.Count > 0 || midNightDataSet.Tables[0].Rows.Count > 0)
                {
                    lngMidnightEnergy.Data = midNightDataSet;
                    lngMidnightEnergy.SetWidth(0, 130);
                    lngMidnightEnergy.SetWidth(1, 130);
                    lngMidnightEnergy.SetWidth(2, 130);
                    lngMidnightEnergy.SetWidth(3, 130);
                    lngMidnightEnergy.SetWidth(4, 130);
                    lngMidnightEnergy.SetWidth(5, 130);
                    lngMidnightEnergy.SetWidth(6, 130);
                    lngMidnightEnergy.SetWidth(7, 130); // Story - 354382 - Midnight Energies column width
                }
            }
        }

        /// <summary>
        /// This method is used to show and hide tabs in the Analysis View based on their values in the database
        /// </summary>
        private void LoadAnalysisTab()
        {
            TabEnum tabSwitchCheck;
            DataSet tabNameData = tabNameBll.GetNoDataTabs(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (tabNameData != null && tabNameData.Tables != null && tabNameData.Tables.Count > 0)
            {
                int dataSetLength = tabNameData.Tables[0].Rows.Count;
                foreach (DataRow dataSetRow in tabNameData.Tables[0].Rows)
                {
                    tabSwitchCheck = (TabEnum)System.Enum.Parse(typeof(TabEnum), dataSetRow[0].ToString().Trim(), true);

                    switch (tabSwitchCheck)
                    {
                        case TabEnum.Gen:
                            {
                                tabControlReport.TabPages.Remove(tabPageGeneral);
                                break;
                            }
                        case TabEnum.InsRea:
                            {
                                tabControlInstant.TabPages.Remove(tabPageReading);
                                break;
                            }
                        case TabEnum.InsABC:
                            {
                                tabControlInstant.TabPages.Remove(tabPageABCCode);
                                break;
                            }
                        case TabEnum.Nam:
                            {
                                tabControlReport.TabPages.Remove(tabNamePlateDetails);
                                break;
                            }
                        case TabEnum.InsSel:
                            {
                                tabControlInstant.TabPages.Remove(tabPageSelfDiagnosis);
                                break;
                            }                        
                        case TabEnum.BilEneMai:
                            {
                                tabControl5.TabPages.Remove(tabPageMainEnergy);
                                break;
                            }
                        case TabEnum.BilEneCon:
                            {
                                tabControl5.TabPages.Remove(tabPageConsumption);
                                break;
                            }
                        case TabEnum.BilEneTodEne:
                            {
                                tabControl5.TabPages.Remove(tabPageTODEnergy);
                                break;
                            }
                        case TabEnum.BilEneTodCon:
                            {
                                tabControl5.TabPages.Remove(tabPageTODConsumption);
                                break;
                            }
                        case TabEnum.BilDemMax:
                            {
                                tabControl4.TabPages.Remove(tabPageMaximumDemand);
                                break;
                            }
                        case TabEnum.BilDemTod:
                            {
                                tabControl4.TabPages.Remove(tabPageTODMD);
                                break;
                            }
                        case TabEnum.BilDemPowOff:
                            {
                                tabControl2.TabPages.Remove(tabPagePowerOffDuration);
                                break;
                            }

                        case TabEnum.BilDemPowOn:
                            {
                                tabControl2.TabPages.Remove(tabPagePowerOnDuration);
                                break;
                            }

                        case TabEnum.BilDemPowFac:
                            {
                                tabControl2.TabPages.Remove(tabPageBillingPowerFactor);
                                break;
                            }
                        case TabEnum.BilDemAvgLoaFac:
                            {
                                tabControl2.TabPages.Remove(tabPageLoadFactor);
                                break;
                            }
                        case TabEnum.BilMis:
                            {
                                tabControl2.TabPages.Remove(tabPageBillingMiscellaneous);
                                break;
                            }
                        case TabEnum.BilTouCon:
                            {
                                tabControl2.TabPages.Remove(tabPageTouConfiguration);
                                break;
                            }
                        case TabEnum.Tam:
                            {

                                tabControlReport.TabPages.Remove(tabPageTamper);
                                break;
                            }
                        case TabEnum.LoaSur:
                            {

                                tabControlReport.TabPages.Remove(tabPageLoadSurvey);
                                break;
                            }
                        case TabEnum.MidDat:
                            {
                                tabControlReport.TabPages.Remove(tabPageMidNightData);
                                break;
                            }
                        case TabEnum.Tra:
                            {
                                tabControlReport.TabPages.Remove(tabPageTransaction);
                                break;
                            }
                        case TabEnum.Pha:
                            {
                                tabControlReport.TabPages.Remove(tabPagePhasor);
                                break;
                            }
                        case TabEnum.MidEne:
                            {
                                tabControlReport.TabPages.Remove(tabPageMidnightEnergy);
                                break;
                            }
                        case TabEnum.DaiEneCon:
                            {
                                tabControlReport.TabPages.Remove(tabDailyConsmption);
                                break;
                            }
                        case TabEnum.MtrCfg:
                            {
                                tabControlReport.TabPages.Remove(tabPageMeterConfiguration);
                                break;
                            }
                        case TabEnum.MDWithIP:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabMDWithIP);
                                break;
                            }
                        case TabEnum.KvarSel:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabkvarSelection);
                                break;
                            }
                        case TabEnum.RS232:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabRS232);
                                break;
                            }
                        case TabEnum.BillTyp:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabBillingReset);
                                break;
                            }
                        case TabEnum.AutoLck:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageAutoLock);
                                break;
                            }
                        case TabEnum.RTC:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabRTC);
                                break;
                            }
                        case TabEnum.DaiLog:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabDailyLog);
                                break;
                            }
                        case TabEnum.DspPar:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabDisplayParamaters);
                                break;
                            }
                        case TabEnum.TOD:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                                break;

                            }
                        case TabEnum.LSIP:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageLSIP);
                                break;
                            }
                        case TabEnum.DIP:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageDIP);
                                break;
                            }
                        case TabEnum.FraEne:
                            {
                                tabControlReport.TabPages.Remove(tabPageFraudEnergy);
                                break;
                            }
                        case TabEnum.ManBil:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabManualBilling);
                                break;
                            }
                        case TabEnum.SofBil:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabSoftwareBilling);
                                break;
                            }
                        case TabEnum.DisCon:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabdisconnectsmart);
                                break;
                            }
                        case TabEnum.LoaCon:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabloadcontrolsmart);
                                tabCtrlMeterConfiguration.TabPages.Remove(tabLoadCtrl1PSmart);
                                break;
                            }
                        case TabEnum.RS485:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabRS485);
                                break;  
                            }
                        case TabEnum.BilDemAvgLoad:
                            {
                                tabControl2.TabPages.Remove(tabPageAvgLoad);
                                break;
                            }
                        case TabEnum.PaymentMode:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPagePaymentMode);
                                break;
                            }
                        case TabEnum.MeteringMode:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageMeteringMode);
                                break;
                            }
                        case TabEnum.LoadLimit:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageLoadLimit);
                                break;
                            }
                        case TabEnum.SlidingDemand:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageSlidingDemand);
                                break;
                            }

                        case TabEnum.OpticalRJPortLock:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPagePortConfig);
                                break;
                            }
                        case TabEnum.BillCumulativeMD:
                            {
                               tabControl4.TabPages.Remove(tabPageCumulativeMD);
                                break;
                            }
                        case TabEnum.LoadSwitch:
                            {
                                tabControlReport.TabPages.Remove(tabPageLoadSwitch);

                                break;
                            }

                        case TabEnum.ManualMDReset:
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageManualMDreset);

                                break;
                            }





                    }
                }
                //To hide the tabs if their child tabs have been removed. 
                if (tabControlInstant.TabCount == 0)
                {
                    tabControlReport.TabPages.Remove(tabPageInstant);
                }
                if (tabControl5.TabCount == 0)
                {
                    tabControl2.TabPages.Remove(tabPageBillingEnergy);
                }
                if (tabControl4.TabCount == 0)
                {
                    tabControl2.TabPages.Remove(tabPageBillingDemand);
                }
                if (tabControl5.TabCount == 0 && tabControl4.TabCount == 0)
                {
                    tabControlReport.TabPages.Remove(tabPageBilling);
                }
            }
        }

        public void MeterDataList_Load(object sender, EventArgs e)
        {
            try
            {
                GetMeterConfigSettings();
                int screenHeight = Screen.GetWorkingArea((Form)sender).Height;
                if (screenHeight < 750)
                {
                    tabControlReport.Height = 496;
                    tabControlInstant.Height = 457;
                    tabPageReading.Height = 463;
                    lngInstant.Height = 410;
                    this.Height = 546;
                }
                else
                {
                    tabControlReport.Height = 696;
                    tabControlInstant.Height = 657;
                    tabPageReading.Height = 663;
                    lngInstant.Height = 625;
                    this.Height = 746;
                }

                int diffHeight = this.Height - (this.Bounds.Height - 50);   //Calculate Bound and substract statusstrip height
                if (diffHeight>0)
                {
                    tabControlReport.Height -= diffHeight;
                    tabControlInstant.Height -= diffHeight;
                    tabPageReading.Height -= diffHeight;
                    lngInstant.Height -= diffHeight;
                    this.Height -= diffHeight;
                }


                LoadAnalysisTab();
                BindTOUGrids();
                //UtilitySpecificDesignChange();
                /* VBM #138642 Remove Midnight data tab  for all utilities bcz its redundant , for details take a tour of TFS */
                if (tabControlReport.TabPages.Contains(tabPageMidNightData))
                {
                    tabControlReport.TabPages.Remove(tabPageMidNightData);
                }
                /* VBM #138642 */

                if (!UtilityDetails.ShowPowerOffDurationInBilling)
                {
                    tabControl2.TabPages.Remove(tabPagePowerOffDuration);
                }
                //Temp code , later on need to handle it at parsing/insertion.
                MeterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);
                if (MeterModelNumber == NamePlateConstants.RubyE250Value)
                {
                    tabControl2.TabPages.Remove(tabPagePowerOffDuration);
                }




                //Temp code , later on need to handle it at parsing/insertion.
                CommonMethods.MeterDataType = getMeterDataType();


                CommonMethods.MeterType = getMeterType();




                if (CommonMethods.MeterType.Contains("LTCT"))
                {
                    if (this.displayParameters != null)
                    {
                        this.displayParameters.FillDisplayParameters(true);
                        this.displayParameters.Refresh();
                    }
                }



                /* VBM change text */
                this.Text = "Analysis View";
                /* VBM 1change text */
                //this.tabControl1.TabPages.Remove(tbGraph);    

                if (!string.IsNullOrEmpty(MeterDataID))
                {
                    //general
                    MeterDataEntity meterDataEntity = (MeterDataEntity)new MeterDataBLL().GetDetailData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    FillGeneral();
                    FillInstant();
                    FillABCCode();//For Vim series2 meter
                    FillNamePlate();
                    FillList();
                    FillAnamoly();
                    FillBillingParameters();
                    FillLoadSurvey();
                    FillMidNightData();
                    FillDailyConsumption();
                    FillGenericMidnightEnergies();
                    FillPhasor();
                    FillMiscellaneous();                
                    FillLoadSwitch();
                   
                    if (ConfigInfo.ActiveFileType == "NONDLMS")
                    {

                        if (ConfigInfo.ActiveMeterType == "1P-2W")
                        {
                            tabControlReport.TabPages.Remove(tabPageFraudEnergy);
                            tabControlReport.TabPages.Remove(tabPagePhasor);
                            tabControlReport.TabPages.Remove(tabPageLoadSwitch);
                        }
                        else
                        {
                            FillFraudEnergy();

                        }

                        // Tab not required for 1P IEC meters in meter configuration
                        tabCtrlMeterConfiguration.TabPages.Remove(tabRTC);
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPageDIP);
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPageLSIP);
                        tabCtrlMeterConfiguration.TabPages.Remove(tabBillingReset);
                        DisplayMeterConfiguration();
                        //tabControlReport.TabPages.Remove(tabPageMeterConfiguration);
                    }
                    else
                    {
                        if (tabControlReport.TabPages.Contains(tabPageFraudEnergy))
                        {
                            tabControlReport.TabPages.Remove(tabPageFraudEnergy);
                        }

                        DisplayMeterConfiguration();
                    }

                    // SB code change Start - 20180629 - Multiple Analysis View
                    this.Text = meterDataEntity.MeterID + " (" + DateUtility.LongToDateTime(meterDataEntity.ReadingDateTime).ToShortDateString()+ ")";// "Analysis View"
                    this.Tag = meterDataEntity.MeterData_ID.ToString(); ;
                    strMsg = ConfigInfo.ActiveMeterDataId;
                    strActiveFileType = ConfigInfo.ActiveFileType;
                    strActiveFirmwareVersion = ConfigInfo.ActiveFirmwareVersion;
                    strActiveMeterType = ConfigInfo.ActiveMeterType;
                    // SB code change End - 20180629 - Multiple Analysis View
                }
                int intScreeHeight = ((System.Drawing.Rectangle)Screen.GetWorkingArea((Form)sender)).Height - 250;
                //to hide tod tab , since it has no data
                //tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);  
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "MeterDataList_Load(object sender, EventArgs e)", ex);
            }
        }

        /// <summary>
        /// Display Meter Configuration tabs and data .
        /// </summary>
        private void DisplayMeterConfiguration()
        {
            if (tabControlReport.TabPages.Contains(tabPageMeterConfiguration))
            {
                //MD with IP values populated in meterconfigform                    
                //DisplayMDWithIP(new MDWithIPBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)));
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabMDWithIP))
                {
                    if (Convert.ToBoolean(element.DIPWithSliding))
                    {
                        //MD with IP values populated in meterconfigform                    
                        DisplayMDWithIP(new MDWithIPBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)));
                    }
                    else
                    {
                        tabCtrlMeterConfiguration.TabPages.Remove(tabMDWithIP);
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageLSIP))
                {
                    if (element != null)
                    {

                        if (Convert.ToBoolean(element.SIP))
                        {
                            //Display LSIP
                            DisplayLSIP();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageLSIP);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageDIP))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.DIP))
                        {
                            //DIP Display
                            DisplayDIP();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageDIP);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabkvarSelection))
                {
                    if (Convert.ToBoolean(element.KvahSelection))
                    {
                        //kvar values populated in meterconfigform
                        DisplaykvarSelection(new kvarSelectionBLL().GetData(Convert.ToInt32(MeterDataID)));
                        if (ConfigInfo.MeterModel == "10")
                        {
                            tabkvarSelection.Text = "Mvarh Selection";
                        }
                    }
                    else
                    {
                        tabCtrlMeterConfiguration.TabPages.Remove(tabkvarSelection);
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabRS232))
                {
                    if (Convert.ToBoolean(element.LockRS232))
                    {
                        //rs232 values populated in meterconfigform
                        RS232LockEntity rs232LockEntity = new RS232BLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        DisplayRS232(rs232LockEntity);
                    }
                    else
                    {
                        tabCtrlMeterConfiguration.TabPages.Remove(tabRS232);
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabBillingReset))
                {
                    if (element != null)
                    {
                        if (element.BillingType.ToUpper() == "NORMAL" || element.BillingType.ToUpper() == "OTHER")
                        //if (Convert.ToBoolean(element.BillingType))
                        {
                            //billing reset values populated in meterconfigform
                            BillingTypeEntity billingResetEntity = new BillingTypeBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            DisplayBillingType(billingResetEntity);
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabBillingReset);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageAutoLock))
                {
                    if (Convert.ToBoolean(element.AutoLock))
                    {
                        //autolock
                        AutoLockEntity autoLockEntity = new AutoLockBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        if (autoLockEntity.AutoLockStatus != null)
                        {
                            tabCtrlMeterConfiguration.TabPages["tabPageAutoLock"].Show();
                            DisplayAutoLock(autoLockEntity);
                        }
                    }
                    else
                    {
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPageAutoLock);
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabRTC))
                {
                    if (element != null)
                    {

                        if (Convert.ToBoolean(element.RTC))
                        {
                            //rtc
                            string rtcValue = new RTCBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            if (rtcValue != null)
                            {
                                DisplayRTC(rtcValue);

                            }
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabRTC);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabDailyLog))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.DailyLog))
                        {
                            //daily log
                            DailyLogEntity DailyLogEntity = new DailyLogBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            DisplayDailyLog(DailyLogEntity);
                            dailyLog1.Enabled = false;
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabDailyLog);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabTOD)
                    || tabCtrlMeterConfiguration.TabPages.Contains(tabPageTwoTOU)
                    || tabCtrlMeterConfiguration.TabPages.Contains(tabPageThreeTOU)
                    || tabCtrlMeterConfiguration.TabPages.Contains(tabPageFourTOU)
                    || tabCtrlMeterConfiguration.TabPages.Contains(tabPageFourTOUSinglePhase)
                || tabCtrlMeterConfiguration.TabPages.Contains(tabTODsmartmeter)
                    )
                    if (element != null)
                    {
                        {
                            if (element.TOD.ToUpper() == "ONE" || element.TOD.ToUpper() == "TWO" || element.TOD.ToUpper() == "FOUR" || element.TOD.ToUpper() == "FOURSP" || element.TOD.ToUpper() == "FOURSP10Z8S" || element.TOD.ToUpper() == "THREE")
                            {
                                //TOU Configuration
                                DisplayTODData();
                            }
                            else
                            {
                                tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                                tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                            }
                        }
                    }
                    else
                    {
                        // Need to remove if TOD is not available
                        //tabControlReport.TabPages.Remove(tabPageMeterConfiguration);
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageCTRatio))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.CTRatio))
                        {
                            //CT  Configuration
                            DisplayCTRatio();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageCTRatio);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPagePTRatio))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.PTRatio))
                        {
                            //CT PT Configuration
                            DisplayPTRatio();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPagePTRatio);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabDisplayParamaters))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.DisplayParameters))
                        {
                            ShowDisplayParameter();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabDisplayParamaters);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabManualBilling))
                {
                    if (element != null)
                    {

                        if (Convert.ToBoolean(element.ManualBilling))
                        {
                            //Manual Billing
                            ManualBillingEntity manualBillingEntity = new ManualBillingBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            if (manualBillingEntity.ManualBillingStatus != null)
                            {
                                tabCtrlMeterConfiguration.TabPages["tabManualBilling"].Show();
                                DisplayManualBilling(manualBillingEntity);
                            }
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabManualBilling);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabSoftwareBilling))
                {
                    if (element != null)
                    {

                        if (Convert.ToBoolean(element.SoftwareBilling))
                        {
                            //Software Billing
                            SoftwareBillingEntity softwareBillingEntity = new SoftwareBillingBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            if (softwareBillingEntity.SoftwareBillingStatus != null)
                            {
                                tabCtrlMeterConfiguration.TabPages["tabSoftwareBilling"].Show();
                                DisplaySoftwareBilling(softwareBillingEntity);
                            }
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabSoftwareBilling);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabdisconnectsmart))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.DisconnectControl))
                        {
                            //Disconnect Control                                                                          
                            DisplayDisconnectControl();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabdisconnectsmart);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabloadcontrolsmart))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.LoadControl))
                        {
                            //Load Control                                                                          
                            DisplayLoadControl();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabloadcontrolsmart);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabLoadCtrl1PSmart))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.LoadControl1PSmartMeter))
                        {
                            //Load Control                                                                          
                            DisplayLoadControl();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabLoadCtrl1PSmart);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabRS485))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.RS485))
                        {
                            //RS485                                                                         
                            DisplayReadRS485();

                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabRS485);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPagePaymentMode))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.PaymentMode))
                        {
                            //PaymentMode                                                                         
                            DisplayReadPaymentmode();

                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPagePaymentMode);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageMeteringMode))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.Meteringmode))
                        {
                            //MeteringMode                                                                         
                            DisplayReadMeteringmode();

                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageMeteringMode);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageLoadLimit))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.LoadLimit))
                        {
                            //LoadLimit                                                                         
                            DisplayReadLoadLimit();

                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageLoadLimit);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageSlidingDemand))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.SlidingDemand))
                        {
                            //SlidingDemand                                                                         
                            DisplaySlidingDemand();

                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageSlidingDemand);
                        }
                    }
                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPagePortConfig))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.OpticalLockUnlock))
                        {
                            //OpticalLockUnlock                                                                         
                            DisplayOpticalLockUnlock();

                        }

                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPagePortConfig);
                        }
                    }
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.RJLockUnlock))
                        {
                            //RJLockUnlock                                                                         
                            DisplayRJLockUnlock();

                        }

                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPagePortConfig);
                        }
                    }


                }

                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPagePulseEnergy))
                {
                    if (element != null)
                    {
                        if (Convert.ToBoolean(element.PulseEnergy))
                        {                                                                      
                            DisplayPulseEnergy();
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPagePulseEnergy);
                        }
                    }
                }
                if (tabCtrlMeterConfiguration.TabPages.Contains(tabPageManualMDreset)) 
                {
                    if (element != null)
                    {

                        if (Convert.ToBoolean(element.ManualButtonMDReset))
                        {
                            ManualMDResetEntity manualMDResetEntity = new ManualMDResetBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                            if (manualMDResetEntity.ManualMDResetStatus != null)
                            {
                                tabCtrlMeterConfiguration.TabPages["tabPageManualMDreset"].Show();
                                DisplayManualMDReset(manualMDResetEntity);
                            }
                        }
                        else
                        {
                            tabCtrlMeterConfiguration.TabPages.Remove(tabPageManualMDreset);
                        }
                    }
                }

                tabControlReport.SelectedTab = tabPageGeneral;
            }
        }

        private void DisplayLoadControl()
        {
            try
            {
                //Single Phase Smart Meter
                //if (MeterModelNumber == 23)
                //{
                //    string Data = new LoadControlBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    string[] lstData = Data.Split('\\');
                //    if (lstData.Length == 6)
                //    {
                //        txtOverLoadthreshhold.Text = (Convert.ToDecimal(lstData[0]) / 1000).ToString("0.000");
                //        txtOCthreshold.Text = (Convert.ToDecimal(lstData[1]) / 100).ToString("0.00");
                //        txtTimeInterval1P.Text = lstData[2];
                //        txtMaxRetry1P.Text = lstData[5];
                //        txtWaitTime1P.Text = lstData[3];
                //        txtMaxRinCycle.Text = lstData[4];

                //    }
                //}
                ////Three Phase Smart Meter
                //else if (MeterModelNumber == 24 || MeterModelNumber == 25)
                //{
                //    string Data = new LoadControlBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    string[] lstData = Data.Split('\\');
                //    if (lstData.Length < 8)
                //    {
                //        cmbovercurrent.SelectedIndex = int.Parse(lstData[0]);
                //        cmbLowPFLoadControl.SelectedIndex = int.Parse(lstData[1]);
                //        cmbOverLoadTH.SelectedIndex = int.Parse(lstData[2]);
                //        txttimeintervalbwRetry.Text = lstData[3];
                //        txtMaxRetryinaCycle.Text = lstData[4];
                //        txtthresoldRetryForNextConnect.Text = lstData[5];
                //        txtmaxRetryCycleCount.Text = lstData[6];

                //    }
                //}
                if (MeterModelNumber == 34 || MeterModelNumber == 35)//Three Phase Smart Meter Falcon2 ciphering
                {
                    string Data = new LoadControlBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    string[] lstData = Data.Split('\\');
                    if (lstData.Length == 8)
                    {
                        cmbovercurrent.SelectedIndex = int.Parse(lstData[0]);
                        cmbLowPFLoadControl.SelectedIndex = int.Parse(lstData[1]);
                        cmbOverLoadTH.SelectedIndex = int.Parse(lstData[2]);
                        txttimeintervalbwRetry.Text = lstData[3];
                        txtMaxRetryinaCycle.Text = lstData[4];
                        txtthresoldRetryForNextConnect.Text = lstData[5];
                        txtmaxRetryCycleCount.Text = lstData[6];
                        txtRelayReconnectiontime.Text = lstData[7];
                    }
                }
                else if (MeterModelNumber == 37)//Single Phase Smart Meter Falcon2 ciphering
                {
                    string Data = new LoadControlBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    string[] lstData = Data.Split('\\');
                    if (lstData.Length == 8)
                    {
                        cmb1phOverload.SelectedIndex = int.Parse(lstData[1]);
                        cmb1phOverloadCurrent.SelectedIndex = int.Parse(lstData[2]);
                        txt1phTimeInterval.Text = lstData[3];
                        txt1phMaxretry.Text = lstData[4];
                        txt1phWaitTime.Text = lstData[5];
                        txt1phMaxRetryCycle.Text = lstData[6];
                        txt1phRelayReconnectiontime.Text = lstData[7];
                        
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayLoadControl()", ex);
            }
        }
        //********* For HTCT meter *******************
        private void DisplayReadRS485()
        {
            try
            {
                string Data = new RS485BLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                txtRS485DeviceAddress.Text= Data.Trim();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayReadRS485()", ex);
            }
        }
        private void DisplayReadPaymentmode()
        {
            try
            {
                string Data = new PaymentModeBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (Convert.ToInt32(Data) <= 1) cmbPaymentMode.SelectedIndex = Convert.ToInt32(Data);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayReadPaymentmode()", ex);
            }
        }

        private void DisplayReadMeteringmode()
        {
            try
            {
                string Data = new MeteringModeBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (Convert.ToInt32(Data) <= 1) cmbMeteringMode.SelectedIndex = Convert.ToInt32(Data);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayReadMeteringmode()", ex);
            }
        }

        private void DisplayReadLoadLimit()
        {
            try
            {
                long objlong;
                string Data = new LoadLimitBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                string[] lstData = Data.Split('\\');
                if (lstData.Length > 0)
                {
                    //string data = string.Empty;
                    //string strtemp = lstData[0];
                    //long itotalmin = long.Parse(strtemp);
                    //decimal ihr = (decimal)(itotalmin / 1000M);
                    //data = ihr.ToString("0.000") + " kW";

                    if (long.TryParse(lstData[0], out objlong)) { txtLoadLimitValue.Text = (Convert.ToInt64(lstData[0])).ToString("0"); txtLoadLimitValue.BackColor = Color.White; }
                   
                    else { txtLoadLimitValue.Text = lstData[0]; txtLoadLimitValue.BackColor = Color.Red; }

                }


            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayReadLoadLimit()", ex);
            }
        }

        private void DisplaySlidingDemand()
        {
            try
            {
                int countlen = 0;
                int EncCount = 0;
                byte[] ReceiveData = new byte[6];
                string Data = new SlidingDemandBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                while (countlen < Data.Length - 1)
                {
                    ReceiveData[EncCount++] = Convert.ToByte(Data.Substring(countlen, 2), 16);
                    countlen += 2;

                }
                cmbIntegrationPeriodMain.SelectedIndex = -1;
                cmbIntegrationPeriodSub.SelectedIndex = -1;
                int compValueS = (int)ReceiveData[1] >> 4;
                if (ReceiveData[0] == 0x02)
                {
                    if (ReceiveData[3] <= 1) cmbIntegrationPeriodMain.SelectedIndex = ReceiveData[3];
                    if (ReceiveData[5] == 0) cmbIntegrationPeriodSub.SelectedIndex = 0;
                    else if (ReceiveData[5] == 5) cmbIntegrationPeriodSub.SelectedIndex = 1;
                    else { /*Default Display*/ }
                }


            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplaySlidingDemand()", ex);
            }



        }

        private void DisplayOpticalLockUnlock()
        {

            try
            {
                string Data = new OpticalLockUnlockBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (Convert.ToInt32(Data) <= 1) cmbopticalPortConfig.SelectedIndex = Convert.ToInt32(Data);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayOpticalLockUnlock()", ex);
            }

        }
        private void DisplayRJLockUnlock()
        {
            try
            {
                string Data = new OpticalLockUnlockBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (Convert.ToInt32(Data) <= 1) cmbRJPortConfig.SelectedIndex = Convert.ToInt32(Data);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayRJLockUnlock()", ex);
            }

        }


        private void DisplayDisconnectControl()
        {
            try
            {
                string Data = new DisconnectControlBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                bool val = Convert.ToBoolean(Convert.ToInt32(Data.Trim()));
                chkconnect.Checked = val;
                chkDisconnect.Checked = !val;

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayDisconnectControl()", ex);

            }
        }
        /// <summary>
        /// Get Settings of meter configuration for a file.
        /// </summary>            
        private void GetMeterConfigSettings()
        {
            string meterModel = "0";
            string firmwareVersion = "0.00";
            decimal fwVersion = 0.00M;
            DataSet dataMeterModelFirmware = common.GetMeterModelandFirmware(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
            if (dataMeterModelFirmware != null && dataMeterModelFirmware.Tables.Count > 0 && dataMeterModelFirmware.Tables[0].Rows.Count > 0
                && decimal.TryParse(dataMeterModelFirmware.Tables[0].Rows[0][1].ToString(), out fwVersion))
            {
                meterModel = dataMeterModelFirmware.Tables[0].Rows[0][0].ToString();
                firmwareVersion = string.Format("{0:0.00}", fwVersion);
            }
            // ***************** For Falcon2 smart meter ***************
            if (dataMeterModelFirmware != null && dataMeterModelFirmware.Tables.Count > 0 && dataMeterModelFirmware.Tables[0].Rows.Count > 0 && (dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "35" || dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "34" || dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "37" || dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "43"))
            {
                meterModel = dataMeterModelFirmware.Tables[0].Rows[0][0].ToString();
                firmwareVersion = string.Format("{0:0.00}", fwVersion);
            }
            if (ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
            {
                meterModel = "21";
                firmwareVersion = "0.00";
                // Special case to handle 1P IEC meters

            }
          
            try
            {
                if (ConfigInfo.ActiveFileType == "DLMS" && dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "38" && (ConfigInfo.ActiveMeterType == "3P-4W" || ConfigInfo.ActiveMeterType == "1P-2W"))
                {
                    meterModel = dataMeterModelFirmware.Tables[0].Rows[0][0].ToString();
                    firmwareVersion = dataMeterModelFirmware.Tables[0].Rows[0][1].ToString();
                    if (firmwareVersion == "")
                        firmwareVersion = "0.00";
                }

               else if (ConfigInfo.ActiveFileType == "DLMS" && dataMeterModelFirmware.Tables[0].Rows[0][0].ToString() == "18" && (ConfigInfo.ActiveMeterType == "3P-4W LTCT" || ConfigInfo.ActiveMeterType == "1P-2W"))
                {
                    meterModel = dataMeterModelFirmware.Tables[0].Rows[0][0].ToString();
                    firmwareVersion = dataMeterModelFirmware.Tables[0].Rows[0][1].ToString();
                    if (firmwareVersion == "b2.72")
                        firmwareVersion = "0.00";
                }
                }
            catch { }
             //Task_id: 579173 Self-diagnostic feature will be disabled in BCS through all the readout communication modes for f/w ver 1.66 old ruby dlms 3P meter 
            if (firmwareVersion == "1.66")
            {
                if(tabControlInstant.TabPages.Contains(tabPageSelfDiagnosis))
                {
                    tabControlInstant.TabPages.Remove(tabPageSelfDiagnosis);
                }
            }

            //element = GetMeterConfig(meterModel, ".065");//For TPDDL
            element = GetMeterConfig(meterModel, firmwareVersion);//Right
        }
        private void BindTOUGrids()
        {
            try
            {
                SpecialDayProfileGrid = null;
                if (element != null && element.TOD.ToUpper() == "ONE")
                {
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOUSinglePhase);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTODsmartmeter);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageThreeTOU);//add 6THSEPT 2017 3PH THREE TOU
                    dayProfileGrids = new DataGridView[] { dgvDayProfile };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 1;

                    seasonProfileGrid = dgvSeasonProfile;
                    weekProfileGrid = dgvWeekProfile;
                    touActivationDate = dtpFutureActivationDate;
                    rdbTOUType = rdbFutureTOD;
                }
                else if (element != null && element.TOD.ToUpper() == "TWO")
                {
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOUSinglePhase);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTODsmartmeter);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageThreeTOU);//add 3PH THREE TOU
                    dayProfileGrids = new DataGridView[] { dgvDayProfile1S2, dgvDayProfile2S2 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 2;

                    seasonProfileGrid = dgvSeasonProfileS2;
                    weekProfileGrid = dgvWeekProfileS2;
                    touActivationDate = dtpTOUActivationDateS2;
                    rdbTOUType = rdbFutureTOU2;
                }
                else if (element != null && element.TOD.ToUpper() == "THREE") // FOR 3PH 3TOU SEASSION
                {
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOUSinglePhase);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTODsmartmeter);


                    dayProfileGrids = new DataGridView[] { dataGridView52, dataGridView51, dataGridView50 };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 3;

                    seasonProfileCount = weekProfileCount = 4;

                    seasonProfileGrid = dataGridView53;
                    weekProfileGrid = dataGridView54;
                    touActivationDate = dateTimePicker6;
                    rdbTOUType = rdbFutureTOU3;



                }
                else if (element != null && element.TOD.ToUpper() == "FOUR" && ConfigInfo.ActiveFileType != "NONDLMS")
                {
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOUSinglePhase);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTODsmartmeter);
                   tabCtrlMeterConfiguration.TabPages.Remove(tabPageThreeTOU);//add FOR 3PH THREE TOU SEASSION
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
                else if (element != null && element.TOD.ToUpper() == "FOURSP10Z8S")
                {
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOUSinglePhase);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageThreeTOU);//add FOR 3PH THREE TOU
                    dayProfileGrids = new DataGridView[] { gridTOUDay1Smart, gridTOUDay2Smart, gridTOUDay3Smart, gridTOUDay4Smart };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfileSmart;
                    weekProfileGrid = dgvWeekProfileSmart;
                    touActivationDate = dtpTOUActivationDateSmart;
                    SpecialDayProfileGrid = dgvSpecialDayProfile;
                    rdbTOUType = rdbFutureTOUSmart;
                }
                // Handle Single Phase Non DLMS data for TOD in meter configuration 
                else if (element != null && (element.TOD.ToUpper() == "FOURSP" || ConfigInfo.ActiveFileType == "NONDLMS"))
                {
                    int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTOD);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageTwoTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageFourTOU);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabTODsmartmeter);
                    tabCtrlMeterConfiguration.TabPages.Remove(tabPageThreeTOU);//add FOR 3PH THREE TOU SEASSION
                    dayProfileGrids = new DataGridView[] { gridTOUDay1SP, gridTOUDay2SP, gridTOUDay3SP, gridTOUDay4SP };
                    seasonProfileCount = weekProfileCount = dayProfileCount = 4;
                    seasonProfileGrid = dgvSeasonProfileS4SP;
                    weekProfileGrid = dgvWeekProfileS4SP;
                    touActivationDate = dtpTOUActivationDateS4SP;
                    if (meterModelNumber != 31 && meterModelNumber != 26 && meterModelNumber != 36 && meterModelNumber != 37 && meterModelNumber != 35 && meterModelNumber != 34)
                    {
                        touActivationDate.Visible = false;
                        lbactivationdatesp.Visible = false;
                        rdbCurrentTOUS4SP.Visible = false;
                        rdbFutureTOUS4SP.Visible = false;
                    }                    
                    rdbTOUType = rdbFutureTOUS4SP;
                }

                foreach (DataGridView dgv in dayProfileGrids)
                {
                    InitializeDayProfile(dgv);
                }

                InitializeWeekProfile(weekProfileGrid);
                InitializeSeasonProfile(seasonProfileGrid);
                if (SpecialDayProfileGrid != null)
                {
                    InitializeSpecialDayProfile(SpecialDayProfileGrid);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BindTOUGrids()", ex);
                throw ex;
            }
        }

        // ************** This code is added for smart meter Special day profile ****************
        private void InitializeSpecialDayProfile(DataGridView dgvSplDayProfilesmart)
        {
            int width = 42;
            SpecialDayProfileCount = 20;
            try
            {
                dgvSplDayProfilesmart.RowHeadersVisible = false;
                dgvSplDayProfilesmart.Columns.Clear();
                dgvSplDayProfilesmart.Columns.Add(GetDataGridView(SpecialDayProfileCount, "Days", "Days", width));
                dgvSplDayProfilesmart.Columns.Add(GetDataGridView(12, "Month", "Month", width));
                dgvSplDayProfilesmart.Columns.Add(GetDataGridView(31, "Date", "Date", width));
                dgvSplDayProfilesmart.Columns.Add(GetDataGridView(4, "DayID", "DayID", width));
                dgvSplDayProfilesmart.RowCount = SpecialDayProfileCount;
                for (int i = 0; i < dgvSplDayProfilesmart.RowCount; i++)
                {
                    dgvSplDayProfilesmart.Rows[i].Cells[0].Value = (i + 1).ToString("00");
                }
                dgvSplDayProfilesmart.Columns["Days"].ReadOnly = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeSpecialDayProfile(DataGridView dgvSplDayProfilesmart)", ex);
            }

        }





        /// <summary>
        /// 
        /// </summary>
        private void DisplayMeterConfigurations()
        {
            DisplayMDWithIP();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterConfig"></param>
        /// <param name="ifMeterConfigDataFound"></param>
        private void DisplayMDWithIP()
        {
            //if (meterConfig.mdWithIPEntity != null)
            //{

            //MeterConfigurations meterConfigurations = new MeterConfigurations();
            //if (meterConfig.mdWithIPEntity.KWDemandType != null)
            //{
            //    cmbDemandType.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandType, meterConfig.mdWithIPEntity.KWDemandType);
            //    ifMeterConfigDataFound = true;
            //}

            //cmbDemandInterval.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandInterval, Convert.ToString(meterConfig.mdWithIPEntity.KWInterval));

            //if (meterConfigurations.getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval)) != -1)
            //{
            //    cmbDemandSubInterlavTime.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval));
            //    ifMeterConfigDataFound = true;
            //}
            //else
            //{
            //    cmbDemandSubInterlavTime.SelectedIndex = -1;
            //}

            //grouBoxMDWithIP.Enabled = false;
            //}
        }
        /// <summary>
        /// Fills Fraud energy data to fraud energy grid .
        /// </summary>
        private void FillFraudEnergy()
        {
            FraudEnergyBLL fraudEnergyBLL = new FraudEnergyBLL();
            DataSet fraudData = fraudEnergyBLL.GetFraudEnergyDataSet(Convert.ToInt32(MeterDataID));
            lngFraudEnergy.Data = fraudData;
            if (fraudData != null && fraudData.Tables[0].Rows.Count > 0)
            {
                if (!tabControlReport.TabPages.Contains(tabPageFraudEnergy))
                {
                    tabControlReport.TabPages.Add(tabPageFraudEnergy);
                }
                for (int rowCount = 0; rowCount < fraudData.Tables[0].Rows.Count; rowCount++)
                {
                    if (fraudData.Tables[0].Rows[rowCount]["Value"].Equals("----"))
                    {
                        fraudData.Tables[0].Rows.Remove(fraudData.Tables[0].Rows[rowCount]);
                        rowCount--;
                    }
                }
                fraudData.Tables[0].AcceptChanges();

                lngFraudEnergy.SetWidth("Descriptions", 250);
                lngFraudEnergy.SetWidth("Value", 150);
                lngFraudEnergy.SetWidth("Unit", 90);
                lngFraudEnergy.IsSorting = false;
            }
        }
        /// <summary>
        /// gets meter config data from xml where meter model number and firmware version match.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="firmware"></param>
        /// <returns></returns>
        private MeterConfigSettingsMeterConfigElement GetMeterConfig(string meterModel, string firmware)
        {
            MeterConfigSettingsMeterConfigElement result = null;
            try
            {
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
                logger.Log(LOGLEVELS.Error, "GetMeterConfig(string meterModel, string firmware)", ex);
            }
            return result;
        }
        /// <summary>
        /// used to fill tou data under touconfiguration tab under billng tab
        /// </summary>
        private void FillTouConfiguration()
        {
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                lngGrdTouConfiguration.Data = new TOUBLL().DetailData(Convert.ToInt32(MeterDataID), true);
                lngGrdTouConfiguration.SetWidth("Zone Start Time(HH:MM)", 140);
                lngGrdTouConfiguration.SetWidth("Zone End Time(HH:MM)", 140);
            }
        }
        /// <summary>
        /// This method is used to get the data based on history and display it.
        /// </summary>
        private void FillMiscellaneous()
        {
            try
            {
                bool isRemove = false;
                DataSet dataSet = new DataSet();
                if (ConfigInfo.ActiveFileType != BCSConstants.IEC)
                {
                    //if (ConfigInfo.ActiveMeterType == "1P-2W")
                    //{
                    if (!string.IsNullOrEmpty(MeterDataID))
                    {
                        dataSet = billingBLL.GetMiscellaneous(Convert.ToInt32(MeterDataID));
                        //Append billing month with history data
                        dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");

                        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            // Story - 345154
                            if (dataSet.Tables[0].Rows[0][CUMTAMPERCOUNT].ToString() == "-1" && dataSet.Tables[0].Rows[0][CUMPOWERFAILURECOUNT].ToString() == "-1" && dataSet.Tables[0].Rows[0][CUMBILLINGMDRESETCOUNT].ToString() == "-1" && dataSet.Tables[0].Rows[0][DELTATAMPERCOUNT].ToString() == "-1")
                                isRemove = true;
                            else
                            {
                                if (dataSet.Tables[0].Rows[0][CUMPOWERFAILURECOUNT].ToString() == "-1")
                                    dataSet.Tables[0].Columns.Remove(CUMPOWERFAILURECOUNT);
                                if (dataSet.Tables[0].Rows[0][CUMTAMPERCOUNT].ToString() == "-1")
                                    dataSet.Tables[0].Columns.Remove(CUMTAMPERCOUNT);
                                if (dataSet.Tables[0].Rows[0][CUMBILLINGMDRESETCOUNT].ToString() == "-1")
                                    dataSet.Tables[0].Columns.Remove(CUMBILLINGMDRESETCOUNT);
                                if (dataSet.Tables[0].Rows[0][DELTATAMPERCOUNT].ToString() == "-1") // Story - 345154
                                    dataSet.Tables[0].Columns.Remove(DELTATAMPERCOUNT);
                                if (dataSet.Tables[0].Rows[0][ABCCodeBilling].ToString() == null || dataSet.Tables[0].Rows[0][ABCCodeBilling].ToString() == "")
                                    dataSet.Tables[0].Columns.Remove(ABCCodeBilling);
                                // Special case to give priority to Delta Tamper count in case both are coming, Story - 345154
                                if (dataSet.Tables[0].Columns.Contains(CUMTAMPERCOUNT) && dataSet.Tables[0].Columns.Contains(DELTATAMPERCOUNT))
                                {
                                    dataSet.Tables[0].Columns.Remove(CUMTAMPERCOUNT);
                                }
                            }
                        }
                        lngMiscellaneous.Data = dataSet;

                        if (lngMiscellaneous.Data != null)
                        {
                            lngMiscellaneous.SetWidth(CUMPOWERFAILURECOUNT, WIDTH);
                            lngMiscellaneous.SetWidth(CUMTAMPERCOUNT, WIDTH);
                            lngMiscellaneous.SetWidth(DELTATAMPERCOUNT, WIDTH); // Story - 345154
                            lngMiscellaneous.SetWidth(CUMBILLINGMDRESETCOUNT, WIDTH);
                            lngMiscellaneous.SetWidth(HISTORY, HISTORYWIDTH);
							lngMiscellaneous.SetWidth(ABCCodeBilling, ABCWidth);//128K
                            lngMiscellaneous.RefreshGrid();
                        }
                        else
                        {
                            isRemove = true;
                        }
                    }
                    //}
                }
                else
                {
                    isRemove = true;
                }
                if (isRemove)
                {
                    tabControl2.TabPages.Remove(tabPageBillingMiscellaneous);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillMiscellaneous()", ex);
            }
        }
        //added PUMA MidNight Data
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.StatusMessage = string.Empty;
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                //getting date from UI
                // Added to solve bug 83782.
                long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtpFromMD.Value.ToShortDateString()));
                long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtpToMD.Value.ToShortDateString()));
                long diffDays = toDate - frmDate;

                if (diffDays >= 0)
                {
                    //call to MidNightData to show details
                    ActivateLSChild("MidNightData");
                    MidNightData meterDataLoadSurvey = new MidNightData();
                    meterDataLoadSurvey.MdiParent = this.MdiParent;
                    meterDataLoadSurvey.FromDate = dtpFromMD.Value;
                    meterDataLoadSurvey.ToDate = dtpToMD.Value;
                    meterDataLoadSurvey.MeterDataId = Convert.ToInt64(MeterDataID);
                    meterDataLoadSurvey.Show();
                }
                else
                {
                    this.StatusMessage = "From date should not be greater than To date.";
                    dtpFromMD.Focus();
                    Application.DoEvents();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void lstEnergy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                if (lstEnergy.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstEnergy.Items[lstEnergy.SelectedIndex])).Row.ItemArray[2].ToString();
                    DataSet ds = billingBLL.GetMeterData(Convert.ToInt32(MeterDataID), Convert.ToInt32(historyId));

                    ds = ReplaceEmptyStringWithDash(ds);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkWh)))
                    {
                        string colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkWh);
                        if (ds.Tables[0].AsEnumerable().All(dr => dr.IsNull(colName) || dr[colName] == "----"))
                            ds.Tables[0].Columns.Remove(colName);
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkVAh)))
                    {
                        string colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkVAh);
                        if (ds.Tables[0].AsEnumerable().All(dr => dr.IsNull(colName) || dr[colName] == "----"))
                            ds.Tables[0].Columns.Remove(colName);
                    }
                    lngBillingEnergy.Data = ds;

                    lngBillingEnergy.SetWidth("Tariff Number", 100);
                    lngBillingEnergy.SetWidth(1, 160);
                    lngBillingEnergy.SetWidth(2, 170);
                    lngBillingEnergy.SetWidth(3, 170);
                    lngBillingEnergy.SetWidth(4, 170);
                    lngBillingEnergy.RefreshGrid();

                    // User Story: 451613 This condition is commented to show column kVAh in TOD Energy dataGrid for 1P Non DLMS meters requirement
                    /* if (MeterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                     {
                         lngBillingEnergy.SetVisibility(Constants.conTODEnergyKVAH, false);
                     }*/

                }
            }
        }

        /// <summary>
        /// This method is used to Replace the empty string with  "----"
        /// User Story: 451613 Show Apparent Energy kVAh in TOD- Energy & Consumption Tab and Detailed Report for 1P Non DLMS meters
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>Data set of TOD Energy/Consumption table</returns>
        private DataSet ReplaceEmptyStringWithDash(DataSet ds)
        {
            if (ds != null)
            {
                foreach (DataTable item in ds.Tables)
                {
                    foreach (DataRow itemRow in item.Rows)
                    {
                        foreach (DataColumn itemColumn in item.Columns)
                        {
                            if (Convert.ToString(itemRow[itemColumn]) == string.Empty)
                            {
                                itemRow[itemColumn] = "----";
                            }
                        }
                    }
                }
            }
            return ds;
        }


        private void lstTODConsumption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                if (lstTODConsumption.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODConsumption.Items[lstTODConsumption.SelectedIndex])).Row.ItemArray[2].ToString();
                    DataSet ds = billingBLL.GetTODConsumption(Convert.ToInt32(MeterDataID), Convert.ToInt32(historyId), true);

                    ds = ReplaceEmptyStringWithDash(ds);

                    if (ds != null && ds.Tables.Count > 0 &&  ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkWh)))
                    {
                        string colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkWh);
                        if (ds.Tables[0].AsEnumerable().All(dr => dr.IsNull(colName) || dr[colName] == "----"))
                            ds.Tables[0].Columns.Remove(colName);
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkVAh)))
                    {
                        string colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkVAh);
                        if (ds.Tables[0].AsEnumerable().All(dr => dr.IsNull(colName) || dr[colName] == "----"))
                            ds.Tables[0].Columns.Remove(colName);
                    }

                    lngTODConsumption.Data = ds;
                    if (lngTODConsumption.Data != null)
                    {
                        lngTODConsumption.SetWidth("Tariff Number", 100);
                        lngTODConsumption.SetWidth(1, 150);
                        lngTODConsumption.SetWidth(2, 150);
                        lngTODConsumption.SetWidth(3, 170);
                        lngTODConsumption.SetWidth(4, 170);
                        lngTODConsumption.RefreshGrid();

                        //User Story: 451613  This condition is commented to show to show column kVAh in TOD Consumption dataGrid for 1P Non DLMS meters requirement
                        /*if (MeterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                        {
                            lngTODConsumption.SetVisibility(Constants.conTODEnergyKVAH, false);
                        }*/
                    }
                }
            }
        }

        private void lstTODMD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                if (lstTODMD.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODMD.Items[lstTODMD.SelectedIndex])).Row.ItemArray[2].ToString();
                    lngTODMD.Data = billingBLL.GetTODMDMeterData(Convert.ToInt32(MeterDataID), Convert.ToInt32(historyId), false);
                    if (lngTODMD.Data != null)
                    {
                        lngTODMD.SetWidth("Tariff Number", 120);
                        lngTODMD.SetWidth(1, 190);
                        lngTODMD.SetWidth(2, 210);
                        lngTODMD.SetWidth(3, 190);
                        lngTODMD.SetWidth(4, 210);
                        lngTODMD.RefreshGrid();
                        //******* Meter Model Change Required Here ***********//
                        if ((MeterModelNumber == 8
                            || MeterModelNumber == 16
                            || MeterModelNumber == NamePlateConstants.SFSP
                            || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek
                            || MeterModelNumber == NamePlateConstants.BYPL_FD
                            || MeterModelNumber == NamePlateConstants.BRPL_CBSP //user story 1016689
                            ) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                        {
                            lngTODMD.SetVisibility(Constants.conTODMD_KVA, false);
                            lngTODMD.SetVisibility(Constants.conTODMD_KVATimeStamp, false);
                        }
                    }
                }
            }
        }

        private void FillList()
        {
            DataSet dataSet = new DataSet();
            //  dataSet = CommonBLL.History();      
            //Get TOD details meterwise
            dataSet = billingBLL.GetTODDetails(Convert.ToInt32(MeterDataID));
            //Append billing month with history data
            dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Single");
            if (dataSet != null && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                lstEnergy.DataSource = dataSet.Tables[0];
                lstEnergy.DisplayMember = DISPLAYMEMBER;
                lstEnergy.ValueMember = VALUEMEMBER;

                lstTODMD.DataSource = dataSet.Tables[0];
                lstTODMD.DisplayMember = DISPLAYMEMBER;
                lstTODMD.ValueMember = VALUEMEMBER;

                lstTODAvgPF.DataSource = dataSet.Tables[0];
                lstTODAvgPF.DisplayMember = DISPLAYMEMBER;
                lstTODAvgPF.ValueMember = VALUEMEMBER;
            }

            //dataSet = CommonBLL.TODHistory();
            //Get TODHistory details meterwise
            dataSet = billingBLL.GetTODHistoryDetails(Convert.ToInt32(MeterDataID));
            //Append billing month with history data
            dataSet = ShowBillingMonth(Convert.ToInt32(MeterDataID), dataSet, "Double");
            if (dataSet != null && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
            {
                lstTODConsumption.DataSource = dataSet.Tables[0];
                lstTODConsumption.DisplayMember = DISPLAYMEMBER;
                lstTODConsumption.ValueMember = VALUEMEMBER;
            }

            if (tabControlReport.TabPages.Contains(tabPageTamper))
            {

                FillTamperSummary(0);
            }

            if (tabControlReport.TabPages.Contains(tabPageTransaction))
            {
                dataSet = common.TransactionCounter(Convert.ToInt32(MeterDataID), FilterData());
                //int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);
                #region HTCT Specific
                if (MeterModelNumber == 10)
                {
                    for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                    {
                        if (dataSet.Tables[0].Rows[rowCount]["Transaction"].ToString() == "kVAh Selection Changed")
                        {
                            dataSet.Tables[0].Rows[rowCount]["Transaction"] = "MVAh Selection Changed";
                            dataSet.AcceptChanges();
                        }
                    }
                }
                #endregion

                lngTamperSupportedTransaction.Data = dataSet;
                lngTamperSupportedTransaction.HiddenColumn = lngTamperSupportedTransaction.ValueColumn = "Data";
                lngTamperSupportedTransaction.SetWidth("Transaction", 235);
                lngTamperSupportedTransaction.SetWidth("Counter", 60);
                lngTamperSupportedTransaction.RefreshGrid();
            }
        }
        /// <summary>
        /// This Method is used to fill tamper Summary grid based on compartment Ids.
        /// </summary>
        /// <param name="compartmentId"></param>
        private void FillTamperSummary(int compartmentId)
        {
            decimal fwVersion = 0.00M;
            DataRow tamperTypeRow;
            if (ConfigInfo.ActiveMeterType == "1P-2W")
            {
                rdbComp1.Enabled = false;
            }
            // SB Code change start 20161123 - Present Tamper Only
            DataSet dataSet = common.TamperCounter(Convert.ToInt32(MeterDataID), compartmentId);
            dataSetTamper = dataSet;
            // SB Code change end 20161123 - Present Tamper Only

            #region PSPCL specific check
            string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
            if (firmwareVersion == "7.01")
            {
                for (int count = 0; count < dataSet.Tables[0].Rows.Count; count++)
                {
                    tamperTypeRow = dataSet.Tables[0].Rows[count];
                    if (tamperTypeRow != null && tamperTypeRow["tamper"].ToString().Contains("Voltage Unbalance - Occurrence"))
                    {
                        tamperTypeRow["tamper"] = "Invalid Voltage - Occurrence";
                    }
                    if (tamperTypeRow != null && tamperTypeRow["tamper"].ToString().Contains("Voltage Unbalance - Restoration"))
                    {
                        tamperTypeRow["tamper"] = "Invalid Voltage - Restoration";
                    }
                }
            }
            #endregion
            #region WB Specific to hide Tamper Parameters
            if (MeterModelNumber == 9)
            {
                DataRow[] rowDelete = dataSet.Tables[0].Select("Tamper = 'Over Voltage in any Phase - Occurrence' or Tamper = 'Over Voltage in any Phase - Restoration' or Tamper = 'Low Voltage in any Phase - Occurrence' or Tamper = 'Low Voltage in any Phase - Restoration'");
                if (rowDelete != null && rowDelete.Length > 0)
                {
                    for (int rowCount = 0; rowCount < rowDelete.Length; rowCount++)
                    {
                        dataSet.Tables[0].Rows.Remove(rowDelete[rowCount]);
                        dataSet.AcceptChanges();
                    }
                }
            }
            #endregion
            lngTamperSupported.Data = dataSet;
            lngTamperSupported.HiddenColumn = lngTamperSupported.ValueColumn = "Data";
            lngTamperSupported.SetWidth("Tamper", 300);
            lngTamperSupported.SetWidth("Counter", 60);
            lngTamperSupported.RefreshGrid();

        }

        /// <summary>
        /// Returns a DataRow containing TamperTypeID and TamperType
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private DataRow GetTransactionParam(ProfileId param)
        {
            
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            string value = string.Empty;
            tamperMapper.TryGetValue(Convert.ToString(param), out value);
            if (value == null)
                return null;
            string[] values = value.Split('|');
            DataRow dr = dt.NewRow();
            dr = dt.Rows.Add(values[0], values[1]);
                 
            return dr;
           
            
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataSet FilterData()
        {
            int meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID);
            DataSet data = new DataSet();
            string value = string.Empty;
            if (element != null)
            {
                data.Tables.Add();
                data.Tables[0].Columns.Add("TamperTypeID");
                data.Tables[0].Columns.Add("TamperType");

                if (Convert.ToBoolean(element.DIP))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DIP)[0], GetTransactionParam(ProfileId.DIP)[1]);
                }
                if (Convert.ToBoolean(element.KvahSelection))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.KvahSelection)[0], GetTransactionParam(ProfileId.KvahSelection)[1]);
                }
                if (Convert.ToBoolean(element.DisplayParametersIEC))
                {
                    // Story - show scroll and HR display parameters in transaction
                    //data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DisplayParameters)[0], GetTransactionParam(ProfileId.DisplayParameters)[1]);
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.PushDisplayParameter)[0], GetTransactionParam(ProfileId.PushDisplayParameter)[1]);
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ScrollDisplyParameter)[0], GetTransactionParam(ProfileId.ScrollDisplyParameter)[1]);
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.HighResolutionDisplayParameter)[0], GetTransactionParam(ProfileId.HighResolutionDisplayParameter)[1]);
                }

                if (Convert.ToBoolean(element.DisplayParameters))
                {
                    // Story - show display parameters in transaction. User Story 464096
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DisplayParameters)[0], GetTransactionParam(ProfileId.DisplayParameters)[1]);
                }

                if (element.TOD != "None")
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.TOU)[0], GetTransactionParam(ProfileId.TOU)[1]);
                }
                if (Convert.ToBoolean(element.RTC))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.RTC)[0], GetTransactionParam(ProfileId.RTC)[1]);
                }
               

                if (Convert.ToBoolean(element.MagneticTamperIcon))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.MagneticTamperIcon)[0], GetTransactionParam(ProfileId.MagneticTamperIcon)[1]);
                }

                if (Convert.ToBoolean(element.MagneticTamperIcon3P))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.MagneticTamperIcon3P)[0], GetTransactionParam(ProfileId.MagneticTamperIcon3P)[1]);
                }


                if (Convert.ToBoolean(element.AutoLock))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.AutoLock)[0], GetTransactionParam(ProfileId.AutoLock)[1]);
                }
                if (meterModelNumber != 34 && meterModelNumber != 35 && meterModelNumber != 37)
                {
                    if (Convert.ToBoolean(element.DisplayParameters))
                    {
                        // Story - show scroll and HR display parameters in transaction
                        //data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DisplayParameters)[0], GetTransactionParam(ProfileId.DisplayParameters)[1]);
                        data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.PushDisplayParameter)[0], GetTransactionParam(ProfileId.PushDisplayParameter)[1]);
                        data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ScrollDisplyParameter)[0], GetTransactionParam(ProfileId.ScrollDisplyParameter)[1]);
                        data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.HighResolutionDisplayParameter)[0], GetTransactionParam(ProfileId.HighResolutionDisplayParameter)[1]);
                    }

                if (element.BillingType.ToUpper() == "NORMAL")
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.BillingType)[0], GetTransactionParam(ProfileId.BillingType)[1]);
                }
                // Transaction Billing Period (cycle) and Billing Date time
                if (element.BillingType.ToUpper() == "OTHER")
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.BillingType)[0], GetTransactionParam(ProfileId.BillingType)[1]);
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.BillingMonthType)[0], GetTransactionParam(ProfileId.BillingMonthType)[1]);
                }
                if (Convert.ToBoolean(element.BillingReset))
                {
                    //Removing MD Reset for WB meters 
                    /* This check is removed as per WB specific OPF demand */
                    //if (MeterModelNumber != 9)
                    //{
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.BillingReset)[0], GetTransactionParam(ProfileId.BillingReset)[1]);
                    //}
                }
                if (Convert.ToBoolean(element.LockRS232))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.RS232LockUnlock)[0], GetTransactionParam(ProfileId.RS232LockUnlock)[1]);
                }
                if (Convert.ToBoolean(element.CTRatio))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.CTRatio)[0], GetTransactionParam(ProfileId.CTRatio)[1]);
                }
                if (Convert.ToBoolean(element.RS485))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.RS485)[0], GetTransactionParam(ProfileId.RS485)[1]);
                }
                if (Convert.ToBoolean(element.PTRatio))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.PTRatio)[0], GetTransactionParam(ProfileId.PTRatio)[1]);
                }
                    if (Convert.ToBoolean(element.SoftwareBilling))
                    {
                        data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.SoftwareBilling)[0], GetTransactionParam(ProfileId.SoftwareBilling)[1]);
                    }
                }
                if (Convert.ToBoolean(element.SIP))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.SIP)[0], GetTransactionParam(ProfileId.SIP)[1]);

                }
                
                if (Convert.ToBoolean(element.DailyLog))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DailyLog)[0], GetTransactionParam(ProfileId.DailyLog)[1]);
                }
               
                if (Convert.ToBoolean(element.ManualBilling))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ManualBilling)[0], GetTransactionParam(ProfileId.ManualBilling)[1]);
                }
                 //******************for smart meter start*******************
                if (Convert.ToBoolean(element.LoadControl))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LoadControl)[0], GetTransactionParam(ProfileId.LoadControl)[1]);
                }
                               
                if (Convert.ToBoolean(element.EventEnableDisable))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.EventEnableDisable)[0], GetTransactionParam(ProfileId.EventEnableDisable)[1]);
                }

                if (Convert.ToBoolean(element.ARMButtonEnable))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ARMButtonEnable)[0], GetTransactionParam(ProfileId.ARMButtonEnable)[1]);
                }

                 if (Convert.ToBoolean(element.RJLockUnlock))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.RJLockUnlock)[0], GetTransactionParam(ProfileId.RJLockUnlock)[1]);
                }


                 if (Convert.ToBoolean(element.OpticalLockUnlock))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.OpticalLockUnlock)[0], GetTransactionParam(ProfileId.OpticalLockUnlock)[1]);
                }

                 if (Convert.ToBoolean(element.ESWFChange))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ESWFChange)[0], GetTransactionParam(ProfileId.ESWFChange)[1]);
                }

                 if (Convert.ToBoolean(element.LoadLimit))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LoadLimit)[0], GetTransactionParam(ProfileId.LoadLimit)[1]);
                }

                 if (Convert.ToBoolean(element.LoadLimitDisabled))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LoadLimitDisabled)[0], GetTransactionParam(ProfileId.LoadLimitDisabled)[1]);
                }

                if (Convert.ToBoolean(element.LoadLimitEnabled))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LoadLimitEnabled)[0], GetTransactionParam(ProfileId.LoadLimitEnabled)[1]);
                }

                if (Convert.ToBoolean(element.GlobalkeyChange))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.GlobalkeyChange)[0], GetTransactionParam(ProfileId.GlobalkeyChange)[1]);
                }
                 if (Convert.ToBoolean(element.HLSkeyChange))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.HLSkeyChange)[0], GetTransactionParam(ProfileId.HLSkeyChange)[1]);
                }
                 if (Convert.ToBoolean(element.BillDateChange))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.BillDateChange)[0], GetTransactionParam(ProfileId.BillDateChange)[1]);
                }

                  if (Convert.ToBoolean(element.LLSSecret))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LLSSecret)[0], GetTransactionParam(ProfileId.LLSSecret)[1]);
                }
                 if (Convert.ToBoolean(element.ImageActivationSchedule))
                {
                    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ImageActivationSchedule)[0], GetTransactionParam(ProfileId.ImageActivationSchedule)[1]);
                }
                 if (Convert.ToBoolean(element.EventThresholdConfig))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.EventThresholdConfig)[0], GetTransactionParam(ProfileId.EventThresholdConfig)[1]);
                 }
                 if (Convert.ToBoolean(element.EventThresholdPersistence))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.EventThresholdPersistence)[0], GetTransactionParam(ProfileId.EventThresholdPersistence)[1]);
                 }
                 if (Convert.ToBoolean(element.Meteringmode))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.Meteringmode)[0], GetTransactionParam(ProfileId.Meteringmode)[1]);
                 }
                 if (Convert.ToBoolean(element.ConfigChangedForwardMode))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ConfigChangedForwardMode)[0], GetTransactionParam(ProfileId.ConfigChangedForwardMode)[1]);
                 }

                 if (Convert.ToBoolean(element.NewFirmware))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.NewFirmware)[0], GetTransactionParam(ProfileId.NewFirmware)[1]);
                 }

                 if (Convert.ToBoolean(element.HLSkeyFWChange))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.HLSkeyFWChange)[0], GetTransactionParam(ProfileId.HLSkeyFWChange)[1]);
                 }

                 if (Convert.ToBoolean(element.MDReset))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.MDReset)[0], GetTransactionParam(ProfileId.MDReset)[1]);
                 }

                 if (Convert.ToBoolean(element.ConfigChangedImportExportMode))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ConfigChangedImportExportMode)[0], GetTransactionParam(ProfileId.ConfigChangedImportExportMode)[1]);
                 }

                 if (Convert.ToBoolean(element.LastTokenAmountPrepaid))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LastTokenAmountPrepaid)[0], GetTransactionParam(ProfileId.LastTokenAmountPrepaid)[1]);
                 }

                 if (Convert.ToBoolean(element.LastTokenTimePrepaid))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LastTokenTimePrepaid)[0], GetTransactionParam(ProfileId.LastTokenTimePrepaid)[1]);
                 }

                 if (Convert.ToBoolean(element.TotalAmountLastRecharge))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.TotalAmountLastRecharge)[0], GetTransactionParam(ProfileId.TotalAmountLastRecharge)[1]);
                 }


                 if (Convert.ToBoolean(element.CurrentBalanceAmount))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.CurrentBalanceAmount)[0], GetTransactionParam(ProfileId.CurrentBalanceAmount)[1]);
                 }
                 if (Convert.ToBoolean(element.CurrentBalanceTime))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.CurrentBalanceTime)[0], GetTransactionParam(ProfileId.CurrentBalanceTime)[1]);
                 }

                 if (Convert.ToBoolean(element.DigitalOutputOperation))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.DigitalOutputOperation)[0], GetTransactionParam(ProfileId.DigitalOutputOperation)[1]);
                 }

                 if (Convert.ToBoolean(element.SIPPeriodChange))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.SIPPeriodChange)[0], GetTransactionParam(ProfileId.SIPPeriodChange)[1]);
                 }


                 if (Convert.ToBoolean(element.LSParameterStoreID))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.LSParameterStoreID)[0], GetTransactionParam(ProfileId.LSParameterStoreID)[1]);
                 }

                 if (Convert.ToBoolean(element.OpticalLock))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.OpticalLock)[0], GetTransactionParam(ProfileId.OpticalLock)[1]);
                 }

                 if (Convert.ToBoolean(element.RJLock))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.RJLock)[0], GetTransactionParam(ProfileId.RJLock)[1]);
                 }

                 if (Convert.ToBoolean(element.SpecialDay))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.SpecialDay)[0], GetTransactionParam(ProfileId.SpecialDay)[1]);
                 }
                 if (Convert.ToBoolean(element.ARMButtonDisable))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ARMButtonDisable)[0], GetTransactionParam(ProfileId.ARMButtonDisable)[1]);
                 }
                 if (Convert.ToBoolean(element.FSModeLock))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.FSModeLock)[0], GetTransactionParam(ProfileId.FSModeLock)[1]);
                 }
                 if (Convert.ToBoolean(element.FSModeUnlock))
                 {
                     data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.FSModeUnlock)[0], GetTransactionParam(ProfileId.FSModeUnlock)[1]);
                 }

                //if (Convert.ToBoolean(element.ManualButtonMDReset))
                //{
                //    data.Tables[0].Rows.Add(GetTransactionParam(ProfileId.ManualButtonMDReset)[0], GetTransactionParam(ProfileId.ManualButtonMDReset)[1]);
                //}

                //******************for smart meter start end*******************

            }
            return data;
        }

        private void ActivateLSChild(String formName)
        {
            int i;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    if (formName == "MeterDataLoadSurvey" || formName == "MidNightData")
                        this.MdiParent.MdiChildren[i].Close();
                }
            }
        }

        private void AR_btnShowLoadSurvey_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            // Added some  date validations 
            //if (dtpFromDate.Value.Date == dtpToDate.Value.Date)
            //{

            //}
            //if (dtpToDate.Value < FromDate)
            //{
            //    if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
            //        this.StatusMessage = "To Date should be greater/equal to " + FromDate.Date.Day.ToString() + "/" + FromDate.Date.Month.ToString() + "/" + FromDate.Date.Year.ToString();
            //    else
            //        this.StatusMessage = "To Date should be greater/equal to " + FromDate.Date.Month.ToString() + "/" + FromDate.Date.Day.ToString() + "/" + FromDate.Date.Year.ToString();
            //    dtpToDate.Focus();
            //    Application.DoEvents();
            //    return;
            //}
            //if (dtpFromDate.Value > ToDate)
            //{
            //    if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
            //        this.StatusMessage = "From Date should be lesser/equal to " + ToDate.Date.Day.ToString() + "/" + ToDate.Date.Month.ToString() + "/" + ToDate.Date.Year.ToString();
            //    else
            //        this.StatusMessage = "From Date should be lesser/equal to " + ToDate.Date.Month.ToString() + "/" + ToDate.Date.Day.ToString() + "/" + ToDate.Date.Year.ToString();
            //    dtpToDate.Focus();
            //    Application.DoEvents();
            //    return;
            //}

            long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtpFromDate.Value.ToShortDateString() + " 00:00:00"));
            long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtpToDate.Value.ToShortDateString() + " 23:59:59"));

            //if (FromDate.Date > dtpFromDate.Value)

            //    frmDate = DateUtility.DateTimeToLong(FromDate.Date, true);

            //else

            //    frmDate = DateUtility.DateTimeToLong(dtpFromDate.Value, true);

            //if (ToDate.Date < dtpToDate.Value)

            //    toDate = DateUtility.DateTimeToLong(ToDate.Date, false);
            //else
            //    toDate = DateUtility.DateTimeToLong(dtpToDate.Value, false);

            long diffDays = toDate - frmDate;
            if (diffDays >= 0)
            {
                ActivateLSChild("MeterDataLoadSurvey");
                MeterDataLoadSurvey meterDataLoadSurvey = new MeterDataLoadSurvey();
                meterDataLoadSurvey.MdiParent = this.MdiParent;
                meterDataLoadSurvey.FromDate = dtpFromDate.Value;
                meterDataLoadSurvey.ToDate = dtpToDate.Value;
                meterDataLoadSurvey.MeterDataId = Convert.ToInt64(MeterDataID);
                meterDataLoadSurvey.Show();
            }
            else
            {
                this.StatusMessage = "From date should not be greater than To date.";
                dtpFromDate.Focus();
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
        }

        private long ParseDate(string dateTime, bool start)
        {
            string val = dateTime.Substring(0, 8);
            if (start)
                val = val + "000000";
            else
                val = val + "235959";
            return long.Parse(val);
        }

        private void tbGraph_Enter(object sender, EventArgs e)
        {
            FrmLoadSurvey frmLoadSurvey = new FrmLoadSurvey();
            if (isDTMLoadSurvey == false)
                frmLoadSurvey.ShowDialog();
            else
                MessageBox.Show("No Load Survey record found", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabControl1.SelectedIndex = 0;
        }

        public System.Boolean ActivateThisChild(String formName)
        {
            this.StatusMessage = string.Empty;
            int i;
            System.Boolean formSetToMdi = false;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    this.MdiParent.MdiChildren[i].Activate();
                    this.MdiParent.MdiChildren[i].Visible = true;
                    formSetToMdi = true;
                }
            }
            if (i == 0 || formSetToMdi == false)
                return false;
            else
                return true;
        }

        private void lngTamperSupported_OnGridRowChanged(string msg)
        {
            string[] val = msg.Split('|');
            DataSet dataSet = tamperBLL.TamperOccurRestore(val[0], val[1]);
            lngTamperOccur.Data = dataSet;
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                lngTamperOccur.SetWidth("Description", 300);
                lngTamperOccur.SetWidth("Event Date Time (0.0.1.0.0.255;8;2)", 200); //SarkarA code change 20180405 //fix attribute
            }
            else
                lngTamperOccur_OnGridRowChanged(null);
            lngTamperOccur.HiddenColumn = lngTamperOccur.ValueColumn = "PKID|EventCode";
            lngTamperOccur.RefreshGrid();
            //* User story no: 474866. WB requiremen temporary check removed in below code is commented because Power failure tamper event have its snapshot parameter if meter support */
            if (val[0].Trim().Equals("101") || val[0].Trim().Equals("102") || val[0].Trim().Equals("301") || val[0].Trim().Equals("302"))
            {
                lblDesc.Visible = false;
                lngElectricalCondition.Visible = false;
            }
            //else
            //{
            //    lblDesc.Visible = true;
            //    lngElectricalCondition.Visible = true;
            //}
        }

        private void lngTamperOccur_OnGridRowChanged(string msg)
        {
            string tamperColumns = string.Empty;
            DataSet dataSet = null;
            //lngElectricalCondition.Data = null;
            if (!string.IsNullOrEmpty(msg))
            {
                string[] val = msg.Split('|');
                //* User story no: 474866. WB requiremen temporary check removed in below code is commented because Power failure tamper event have its snapshot parameter if meter support */
                if (val[1].Trim().Equals("101") || val[1].Trim().Equals("102"))
                {
                    lblDesc.Visible = false;
                    lngElectricalCondition.Visible = false;
                    return;
                }
                else
                {
                    lblDesc.Visible = true;
                    lngElectricalCondition.Visible = true;
                    //dataSet = common.GetTamperOccurRestoreDetail(Convert.ToInt64(val[0].Trim()), Convert.ToInt64(MeterDataID)); 
                    DataSet tamperData = new DataSet();
                    tamperData = tamperParameterBLL.GetColumnNames(Convert.ToInt32(MeterDataID));
                    if (tamperData != null && tamperData.Tables.Count > 0)
                    {
                        for (int counter = 0; counter < tamperData.Tables[0].Rows.Count; counter++)
                        {
                            tamperColumns = tamperData.Tables[0].Rows[counter][0].ToString();
                        }
                    }
                    dataSet = common.GetTamperOccurRestoreDetailColumnWise(Convert.ToInt64(val[0].Trim()), Convert.ToInt64(MeterDataID), tamperColumns);
                }

                // to hide tamper snapshot in case no data is coming from the meter
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    bool flagNotNull = false;
                    for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                    {
                        if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount]["Value"].ToString()))
                        {
                            flagNotNull = true;
                            break;
                        }
                    }
                    if (flagNotNull)
                    {
                        lblDesc.Visible = true;
                        lngElectricalCondition.Visible = true;
                        lngElectricalCondition.Data = dataSet;

                        lngElectricalCondition.SetWidth("Parameter Name", 150);
                        lngElectricalCondition.SetWidth("OBIS Code", 80);
                        lngElectricalCondition.SetWidth("Class ID", 70);
                        lngElectricalCondition.SetWidth("Attribute", 80);
                        lngElectricalCondition.SetWidth("Value", 78);
                        lngElectricalCondition.SetWidth("Unit", 60);

                        lngElectricalCondition.RefreshGrid();
                    }
                    else
                    {
                        lblDesc.Visible = false;
                        lngElectricalCondition.Visible = false;
                        return;
                    }
                }
                else
                {
                    lblDesc.Visible = false;
                    lngElectricalCondition.Visible = false;
                    return;
                }
            }
            else
            {
                lblDesc.Visible = false;
                lngElectricalCondition.Visible = false;
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            long frmDate = ParseDate(DateUtility.DateTimeToLong(dtpFromDate.Value).ToString(), true);
            long toDate = ParseDate(DateUtility.DateTimeToLong(dtpToDate.Value).ToString(), false);
            //long frmDate = DateUtility.DateTimeToLong(dtpFromDate.Value);
            //long toDate = DateUtility.DateTimeToLong(dtpToDate.Value);

            long diffDays = toDate - frmDate;
            int IntervalPeriod;
            if (diffDays >= 0)
            {
                frmLoadsurveyGraph frmloadsurveyGraph = new frmLoadsurveyGraph();
                frmloadsurveyGraph.FromDate = frmDate;
                frmloadsurveyGraph.ToDate = toDate;
                if (ConfigInfo.ActiveFileType == "DLMS")
                {
                    //IntervalPeriod = new DLMS650LoadSurveyBLL().GetLoadSurveyInterval(Convert.ToInt64(MeterDataID), frmDate, toDate, "ENERGY");                
                    IntervalPeriod = new DLMS650LoadSurveyBLL().GetLoadSurveyInterval(Convert.ToInt64(MeterDataID), ParseDate(frmDate.ToString(), true), ParseDate(toDate.ToString(), false));
                }
                else
                {
                    IntervalPeriod = new DLMS650LoadSurveyBLL().GetCABLoadSurveyInterval(Convert.ToInt64(MeterDataID));
                }
                if (IntervalPeriod != 0)
                {
                    frmloadsurveyGraph.IntervalPeriod = IntervalPeriod;
                    frmloadsurveyGraph.MeterDataId = Convert.ToInt64(MeterDataID);
                    frmloadsurveyGraph.Show();
                }
                else
                {
                    MessageBox.Show("No Data Found", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else
            {
                this.StatusMessage = "From date should not be greater than To date.";
                dtpFromDate.Focus();
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
        }

        private void lngTamperOccurTransaction_OnGridRowChanged(string msg)
        {
            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                DataSet dataSet = null;
                string[] val = null;
                if (!string.IsNullOrEmpty(msg))
                {
                    string tamperColumns = string.Empty;
                    val = msg.Split('|');
                    DataSet tamperData = new DataSet();
                    tamperData = tamperParameterBLL.GetColumnNames(Convert.ToInt32(MeterDataID));
                    if (tamperData != null && tamperData.Tables.Count > 0)
                    {
                        for (int counter = 0; counter < tamperData.Tables[0].Rows.Count; counter++)
                        {
                            tamperColumns = tamperData.Tables[0].Rows[counter][0].ToString();
                        }
                    }
                    dataSet = common.GetTamperOccurRestoreDetailColumnWise(Convert.ToInt64(val[0].Trim()), Convert.ToInt64(MeterDataID), tamperColumns);
                    //dataSet = common.GetTamperOccurRestoreDetail(Convert.ToInt64(val[0].Trim()), Convert.ToInt64(MeterDataID));
                }
                // to hide transaction snapshot in case no data is coming from the meter
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    bool flagNotNull = false;
                    for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                    {
                        if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount]["Value"].ToString()))
                        {
                            flagNotNull = true;
                            break;
                        }
                    }
                    if (flagNotNull)
                    {
                        lblTrdesc.Visible = true;
                        lngElectricalConditionTransaction.Visible = true;
                        lngElectricalConditionTransaction.Data = dataSet;

                        lngElectricalConditionTransaction.SetWidth("Parameter Name", 150);
                        lngElectricalConditionTransaction.SetWidth("OBIS Code", 80);
                        lngElectricalConditionTransaction.SetWidth("Class ID", 70);
                        lngElectricalConditionTransaction.SetWidth("Attribute", 80);
                        lngElectricalConditionTransaction.SetWidth("Value", 78);
                        lngElectricalConditionTransaction.SetWidth("Unit", 75);

                        lngElectricalConditionTransaction.RefreshGrid();
                    }
                    else
                    {
                        lblTrdesc.Visible = false;
                        lngElectricalConditionTransaction.Visible = false;
                        return;
                    }
                }
                else
                {
                    lblTrdesc.Visible = false;
                    lngElectricalConditionTransaction.Visible = false;
                    return;
                }
            }
            else
            {
                lblTrdesc.Visible = false;
                lngElectricalConditionTransaction.Visible = false;
                return;
            }
        }

        private void lngTamperSupportedTransaction_OnGridRowChanged(string msg)
        {
            string[] val = msg.Split('|');
            DataSet dataSet = tamperBLL.TamperOccurRestore(val[0], val[1]);
            #region HTCT Specific
            if (MeterModelNumber == 10)
            {
                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    if (dataSet.Tables[0].Rows[rowCount]["Description"].ToString() == "kVAh Selection Changed")
                    {
                        dataSet.Tables[0].Rows[rowCount]["Description"] = "MVAh Selection Changed";
                        dataSet.AcceptChanges();
                    }
                }
            }
            #endregion
            lngTamperOccurTransaction.Data = dataSet;
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                lngTamperOccurTransaction.SetWidth("Description", 300);
                lngTamperOccurTransaction.SetWidth("Event Date Time (0.0.1.0.0.255;8;2)", 200); //SarkarA code change 20180405 //fix attribute
            }
            else
                lngTamperOccurTransaction_OnGridRowChanged(null);
            lngTamperOccurTransaction.HiddenColumn = lngTamperOccurTransaction.ValueColumn = "PKID|EventCode";
            lngTamperOccurTransaction.RefreshGrid();
            //if (ConfigInfo.ActiveFileType == "DLMS")
            //{
            //    lblTrdesc.Visible = true;
            //    lngElectricalConditionTransaction.Visible = true;
            //}
            //if (val[0].Trim().Equals("159"))
            //{
            //    lblTrdesc.Visible = false;
            //    lngElectricalConditionTransaction.Visible = false;
            //}
        }

        private void tabPageMidNightData_Click(object sender, EventArgs e)
        {

        }

        private void tabPagePhasor_Click(object sender, EventArgs e)
        {

        }

        private void lngPhasorData_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Future TOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutureTOD_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }
        /// <summary>
        /// Handle copmpartment wise tamper display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbAllTamper_CheckedChanged(object sender, EventArgs e)
        {
            int compartmentId = 0;
            if (rdbComp1.Checked)
            {
                compartmentId = 1;
            }
            else if (rdbComp2.Checked)
            {
                compartmentId = 2;
            }
            else if (rdbComp3.Checked)
            {
                compartmentId = 3;
            }
            else if (rdbComp5.Checked)
            {
                compartmentId = 5;
            }
            //Fill Tamper summary grid according to compartment id.
            this.Cursor = Cursors.WaitCursor;
            lngTamperSupported.Data = null;
            lngTamperOccur.Data = null;
            lngElectricalCondition.Data = null;
            lngTamperSupported.RefreshGrid();
            FillTamperSummary(compartmentId);
            this.Cursor = Cursors.Default;

            // SB Code Change Start - 20171130 - Present Tamper Only
            this.chkPresents_CheckedChanged(sender, e);
            // SB Code Change End - 20171130 - Present Tamper Only
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbComp1_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbComp2_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbComp3_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbComp5_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }


        /// <summary>
        /// /* VBM bug 135756 Make sure that grid has scroll bars visible when window is resized */
        /// This method makes sure that Instant Grid and tab grows/shrink as Windows Resizes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLMS650MeterDataList_Resize(object sender, EventArgs e)
        {

            if (this.Width < MainFormRestoreDownWidth)
            {
                lngInstant.Width = this.Width - LngInstantRestoreDownWidthOffSet;
                lngInstant.Height = this.Height - LngInstantRestoreDownHeigtOffSet;

            }
            else
            {
                lngInstant.Width = LngInstantWidth;
                lngInstant.Height = LngInstantHeight;
            }
            tabControlInstant.Height = this.Height - LngInstantRestoreDownHeigtOffSet + GridtabHeightOffSet;
            tabControlInstant.Width = this.Width - LngInstantRestoreDownWidthOffSet + GridtabWidthtOffSet;
            tabPageReading.Height = this.Height - LngInstantRestoreDownHeigtOffSet + GridtabHeightOffSet;
            tabPageReading.Width = this.Width - LngInstantRestoreDownWidthOffSet + GridtabWidthtOffSet;
        }

        #region MDWithIP
        /// <summary>
        /// display meter configuration , in mdwithip  
        /// </summary>
        /// <param name="meterConfig"></param>
        /// <param name="ifMeterConfigDataFound"></param>
        /// 
        private void DisplayMDWithIP(MeterConfigurationsNFEntity meterConfig)
        {
            if (meterConfig.mdWithIPEntity != null)
            {


                if (meterConfig.mdWithIPEntity.KWDemandType != null)
                {
                    cmbDemandType.SelectedIndex = GetSelectedIndex(cmbDemandType, meterConfig.mdWithIPEntity.KWDemandType);

                }

                cmbDemandInterval.SelectedIndex = GetSelectedIndex(cmbDemandInterval, Convert.ToString(meterConfig.mdWithIPEntity.KWInterval));

                if (GetSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval)) != -1)
                {
                    cmbDemandSubInterlavTime.SelectedIndex = GetSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval));

                }
                else
                {
                    cmbDemandSubInterlavTime.SelectedIndex = -1;
                }

                grouBoxMDWithIP.Enabled = false;
            }
        }
        #endregion
        /// <summary>
        /// Used to display LSIP Value on UI
        /// </summary>
        private void DisplayLSIP()
        {
            try
            {
                int lsipData = new LSIPBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                if (lsipData != 0)
                {
                    if (lsipData == 900)
                        cmbBoxLSCapturePeriod.SelectedIndex = 0;//To set Min(Sec) value for 900 sec
                    else if (lsipData == 1800)
                        cmbBoxLSCapturePeriod.SelectedIndex = 1;//To set Min(Sec) value for 1800 sec
                    else if (lsipData == 3600)
                        cmbBoxLSCapturePeriod.SelectedIndex = 2;//To set Min(Sec) value for 3600 sec                   
                }
                groupBoxLSIP.Enabled = false;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayLSIP()", ex);
            }

        }

        /// <summary>
        /// Used to display PulseEnergy Value on UI
        /// </summary>
        private void DisplayPulseEnergy()
        {
            try
            {
                string pulseEnergyVal = new PulseEnergyBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                if (!string.IsNullOrWhiteSpace(pulseEnergyVal) && int.TryParse(pulseEnergyVal, out int value))
                {
                    if (value == (int)CAB.E650MeterConfiguration.PulseEnergyValues.Active)
                        rdbPulseActive.Checked = true;
                    else if (value == (int)CAB.E650MeterConfiguration.PulseEnergyValues.Apparent)
                        rdbPulseApparent.Checked = true;
                    else if (value == (int)CAB.E650MeterConfiguration.PulseEnergyValues.Reactive)
                        rdbPulseReactive.Checked = true;

                    grpPulseEnergy.Enabled = false;
                }
                else
                {
                    if(tabCtrlMeterConfiguration.TabPages.Contains(tabPagePulseEnergy))
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPagePulseEnergy);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayPulseEnergy()", ex);
            }

        }

        /// <summary>
        /// Used to display DIP Value on UI
        /// </summary>
        private void DisplayDIP()
        {
            try
            {
                int dipData = new DIPBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                if (dipData == 0x00000384)
                {
                    cmbDIPDemandType.SelectedIndex = 0;
                    cmbDIPDemandInterval.SelectedIndex = 0;
                    labelDIPSubDemandInterval.Visible = false;
                    cmbDIPDemandSubIntervalTime.Visible = false;
                    labelDIPSubDemandIntervalUnit.Visible = false;
                }
                else if (dipData == 0x00000708)
                {
                    cmbDIPDemandType.SelectedIndex = 0;
                    cmbDIPDemandInterval.SelectedIndex = 1;
                    labelDIPSubDemandInterval.Visible = false;
                    cmbDIPDemandSubIntervalTime.Visible = false;
                    labelDIPSubDemandIntervalUnit.Visible = false;
                }
                else if (dipData == 0x00001384)
                {
                    cmbDIPDemandType.SelectedIndex = 1;
                    cmbDIPDemandInterval.SelectedIndex = 0;
                    cmbDIPDemandSubIntervalTime.SelectedIndex = 0;
                    labelDIPSubDemandInterval.Visible = true;
                    cmbDIPDemandSubIntervalTime.Visible = true;
                    labelDIPSubDemandIntervalUnit.Visible = true;

                }
                else if (dipData == 0x00001708)
                {
                    cmbDIPDemandType.SelectedIndex = 1;
                    cmbDIPDemandInterval.SelectedIndex = 1;
                    cmbDIPDemandSubIntervalTime.SelectedIndex = 0;
                    labelDIPSubDemandInterval.Visible = true;
                    cmbDIPDemandSubIntervalTime.Visible = true;
                    labelDIPSubDemandIntervalUnit.Visible = true;
                }
                else if (dipData == 0x00002708)
                {
                    cmbDIPDemandType.SelectedIndex = 1;
                    cmbDIPDemandInterval.SelectedIndex = 1;
                    cmbDIPDemandSubIntervalTime.SelectedIndex = 1;
                    labelDIPSubDemandInterval.Visible = true;
                    cmbDIPDemandSubIntervalTime.Visible = true;
                    labelDIPSubDemandIntervalUnit.Visible = true;
                }

                if (MeterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_WCM)
                {
                    label19.Visible = false;
                    cmbDIPDemandType.Visible = false;
                }

                groupBoxDIP.Enabled = false;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " DisplayDIP()", ex);
            }
        }


        #region BillingType
        // To display Mode of billing
        private void DisplayBillingType(BillingTypeEntity billingTypeEntity)
        {
            try
            {
                Control group = this.billingResetControl.Controls["gbAutoMode"];
                ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
                cmbModeofBilling.SelectedIndex = GetSelectedIndexModeOfBilling((int)(billingTypeEntity.ModeOfBilling));
                ComboBox cmbSelectDay = (ComboBox)group.Controls["cmbSelectDay"];
                ComboBox cmbSelectHour = (ComboBox)group.Controls["cmbSelectHour"];
                ComboBox cmbSelectMinutes = (ComboBox)group.Controls["cmbSelectMinutes"];
                Label lblSelectHour = (Label)group.Controls["lblSelectHour"];
                Label lblSelectMinutes = (Label)group.Controls["lblSelectMinutes"];

                if (cmbModeofBilling.SelectedIndex != 0)
                {
                    cmbSelectDay.SelectedIndex = GetSelectedIndexBillingReset(cmbSelectDay, Convert.ToString(billingTypeEntity.Day));
                    cmbSelectHour.SelectedIndex = GetSelectedIndexBillingReset(cmbSelectHour, Convert.ToString(billingTypeEntity.Hours));
                    cmbSelectMinutes.SelectedIndex = GetSelectedIndexBillingReset(cmbSelectMinutes, Convert.ToString(billingTypeEntity.Minutes));
                }
                else
                {
                    cmbSelectDay.Items.Add("01");
                    cmbSelectHour.Items.Add("00");
                    cmbSelectMinutes.Items.Add("00");
                    cmbSelectDay.SelectedIndex = cmbSelectHour.SelectedIndex = cmbSelectMinutes.SelectedIndex = 0;
                }

                billingResetControl.Enabled = false;
                GroupBox grpResetLockOutDays = this.billingResetControl.Controls["gbManual"] as GroupBox;
                grpResetLockOutDays.Visible = false;


                RadioButton rdbMonthly = (RadioButton)group.Controls["rbtnMonthly"];
                RadioButton rdbOddMonth = (RadioButton)group.Controls["rbtnOddMonth"];
                RadioButton rdbEvenMonth = (RadioButton)group.Controls["rbtnEvenMonth"];
                Label modeof = (Label)group.Controls["lblBillingMode"];
                Label billperiod = (Label)group.Controls["lblBillingPeriod"]; 

                // [BillingType_Month]
                if (billingTypeEntity.BillingType == "255")
                {
                    rdbEvenMonth.Checked = false;
                    rdbOddMonth.Checked = false;
                    rdbMonthly.Checked = false;
                }
                else
                {
                    cmbSelectHour.Visible = false;
                    lblSelectHour.Visible = false;
                    cmbSelectMinutes.Visible = false;
                    lblSelectMinutes.Visible = false;

                    if (MeterModelNumber == NamePlateConstants.RubyE150Value || MeterModelNumber == NamePlateConstants.SFSP)
                    {
                        if (billingTypeEntity.BillingType == "02")
                        {
                            rdbMonthly.Checked = true;
                        }
                        else if (billingTypeEntity.BillingType == "01")
                        {
                            rdbOddMonth.Checked = true;
                        }
                        else if (billingTypeEntity.BillingType == "00")
                        {
                            rdbEvenMonth.Checked = true;
                        }
                    }
                    else
                    {

                        if (billingTypeEntity.BillingType == "00")
                        {
                            rdbMonthly.Checked = true;
                        }
                        else if (billingTypeEntity.BillingType == "01")
                        {
                            rdbOddMonth.Checked = true;
                        }
                        else if (billingTypeEntity.BillingType == "02")
                        {
                            rdbEvenMonth.Checked = true;
                        }
                    }
                }
                if (MeterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_WCM)
                {
                    // Hide Detail for falcon meters not required billing month and period
                    rdbMonthly.Visible = false;
                    rdbOddMonth.Visible = false;
                    rdbEvenMonth.Visible = false;
                    cmbModeofBilling.Visible = false;
                    modeof.Visible = false;
                    billperiod.Visible = false;
                }

                //if ((int)(billingTypeEntity.BillingPeriod) == 0)
                //    rdbMonthly.Checked = true;
                //if ((int)(billingTypeEntity.BillingPeriod) == 1)
                //    rdbOddMonth.Checked = true;
                //if ((int)(billingTypeEntity.BillingPeriod) == 2)
                //    rdbEvenMonth.Checked = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayBillingType(BillingTypeEntity billingTypeEntity)", ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void ShowDisplayParameter()
        {
            try
            {
                Collection<Collection<string>> collDisplayParamaters = new DisplayParameterBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (collDisplayParamaters != null && collDisplayParamaters[0] != null && collDisplayParamaters[1] != null &&
                    collDisplayParamaters[2] != null && collDisplayParamaters[3] != null)
                {
                    displayParameters.Controls[0].Controls[1].Controls["chkboxSelectAll"].Enabled = false;
                    displayParameters.Controls[0].Controls[1].Controls["btnUpScroll"].Enabled = false;
                    displayParameters.Controls[0].Controls[1].Controls["btnDownScroll"].Enabled = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (collDisplayParamaters[i].Count > 0)
                        {
                            SelectRowsInDataGridByDisplayParamaterReadOutput(collDisplayParamaters[i], i == 0 ? DisplayParameter.PushMode : i == 1 ? DisplayParameter.ScrollMode : DisplayParameter.HighResolutionMode);

                        }
                        else
                            DisableDataGrids(i == 0 ? DisplayParameter.PushMode : i == 1 ? DisplayParameter.ScrollMode : DisplayParameter.HighResolutionMode);

                    }
                    if (collDisplayParamaters[3].Count > 0)
                    {
                        SetDisplayTimeOutsByDisplayParamaterReadOutput(collDisplayParamaters[3]);


                    }
                    else
                    {
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Enabled = false;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Enabled = false;
                        ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Enabled = false;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
                    }
                    ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).TabPages.Remove((TabPage)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"]));// Story - Hide Display Timeout Parameter
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowDisplayParameter()", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayTimeOuts"></param>
        private void SetDisplayTimeOutsByDisplayParamaterReadOutput(Collection<string> displayTimeOuts)
        {
            string[] displayTimeOutValues;
            for (int i = 0; i < displayTimeOuts.Count; i++)
            {
                displayTimeOutValues = displayTimeOuts[i].Split('/');
                if (displayTimeOutValues[0] == "Scroll Time Out")
                    displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text = displayTimeOutValues[1];
                else if (displayTimeOutValues[0] == "Push Time Out")
                    displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text = displayTimeOutValues[1];
                else
                {
                    if (displayTimeOutValues[0] == "Auto Scroll Resume Time")
                    {
                        ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = true;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = displayTimeOutValues[1];
                    }
                    else
                    {
                        ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = false;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = "";
                    }
                }
            }
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Enabled = false;
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Enabled = false;
            ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Enabled = false;
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayParameter"></param>
        private void DisableDataGrids(DisplayParameter displayParameter)
        {
            DataGridView dataGridView = new DataGridView();
            tabControlReport.SelectedIndex = 9;
            tabCtrlMeterConfiguration.SelectedIndex = 2;
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
            {
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
            }
            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 1;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            }
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 2;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null)
                    ((DataGridViewCheckBoxCell)dataGridView[0, i]).Value = false;
            }
            int chkboxColIndex = 0, descColIndex = 0;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == "Description")
                    descColIndex = i;
                if (dataGridView.Columns[i].Name == "colInclude")
                    chkboxColIndex = i;

            }
            for (int n = 0; n < dataGridView.Rows.Count; n++)
            {
                dataGridView.Rows[n].ReadOnly = true;
            }

            dataGridView.EndEdit();
            tabCtrlMeterConfiguration.SelectedIndex = 0;
            ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramatersToSelect"></param>
        /// <param name="displayParameter"></param>
        private void SelectRowsInDataGridByDisplayParamaterReadOutput(Collection<string> paramatersToSelect, DisplayParameter displayParameter)
        {
            DataGridView dataGridView = new DataGridView();
            tabControlReport.SelectedTab = tabPageMeterConfiguration;
            tabCtrlMeterConfiguration.SelectedTab = tabDisplayParamaters;
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
            {
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
            }
            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 1;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            }
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 2;
                //Remove HR parameters that are only added for PUMA files.
                try
                {
                    dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
                    if (ConfigInfo.ActiveFileType == "NONDLMS")
                    {
                        DataTable highResolutionData = ((DataViewManager)dataGridView.DataSource).DataSet.Tables["HighResolution"];
                        for (int index = highResolutionData.Rows.Count - 1; index >= 0; index--)
                        {
                            DataRow row = highResolutionData.Rows[index];
                            if (Convert.ToInt32(row["SNo"]) > 4)
                            {
                                row.Delete();
                            }
                        }
                        highResolutionData.AcceptChanges();
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "SelectRowsInDataGridByDisplayParamaterReadOutput(Collection<string> paramatersToSelect, DisplayParameter displayParameter)", ex);
                }
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null)
                    ((DataGridViewCheckBoxCell)dataGridView[0, i]).Value = false;
            }
            int chkboxColIndex = 0, descColIndex = 0, idColIndex = 0; // Display Parameters sequenece in analysis view
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == "Description")
                    descColIndex = i;
                if (dataGridView.Columns[i].Name == "colInclude")
                    chkboxColIndex = i;
                if (dataGridView.Columns[i].Name.ToUpper() == "ID")
                    idColIndex = i;

            }
            #region Code to bind paramaters in data grid by Priority.
            string[] columnIDArray = new string[dataGridView.Rows.Count];

            int iCount = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                columnIDArray[iCount] = row.Cells[idColIndex].Value.ToString() + "^" + row.Cells[descColIndex].Value.ToString();
                iCount++;
            }
            for (int i = 0; i < paramatersToSelect.Count; i++)
            {//Get paramater name in Current row.
                try
                {
                    DataGridViewTextBoxCell txtcell = dataGridView[descColIndex, i] as DataGridViewTextBoxCell;
                    if (txtcell != null && txtcell.Value != null)
                    {//If paramater name in current row is same as the paramater to write then no shuffling is required.
                        //But if paramater name in current row is different to paramater to write
                        //then the paramater in the current row is shifted to the old position of paramater to be written in current row.
                        if (txtcell.Value.ToString() != paramatersToSelect[i])
                        {//if paramater name is not same as paramater name in current row then find the 
                            //row position of old occurence of paramater to be written in current row.
                            for (int j = i; j < dataGridView.Rows.Count; j++)
                            {
                                DataGridViewTextBoxCell txtcell2 = dataGridView[descColIndex, j] as DataGridViewTextBoxCell;
                                //if this condition is true then we get the old position of the paramater to be written in current row.
                                if (txtcell2 != null && txtcell2.Value != null && txtcell2.Value.ToString() == paramatersToSelect[i])
                                {//move the paramater in current row to this new position.
                                    txtcell2.Value = txtcell.Value;
                                    DataGridViewCheckBoxCell cell2 = dataGridView[chkboxColIndex, j] as DataGridViewCheckBoxCell;
                                    if (cell2 != null)
                                        ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, j]).Value = false;
                                }
                            }
                        }//Set the paramater to be written to cell in the current row.
                        txtcell.Value = paramatersToSelect[i];
                        DataGridViewCheckBoxCell cell = dataGridView[chkboxColIndex, i] as DataGridViewCheckBoxCell;
                        if (cell != null)
                            ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, i]).Value = true;
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "SelectRowsInDataGridByDisplayParamaterReadOutput(Collection<string> paramatersToSelect, DisplayParameter displayParameter)", ex);
                }
            }
            for (int n = 0; n < dataGridView.Rows.Count; n++)
            {
                // Display Parameters sequenece in analysis view
                for (int j = 0; j < columnIDArray.Length; j++)
                {
                    DataGridViewTextBoxCell txtcellID = dataGridView[idColIndex, n] as DataGridViewTextBoxCell;
                    DataGridViewTextBoxCell txtcellDesc = dataGridView[descColIndex, n] as DataGridViewTextBoxCell;
                    if (txtcellDesc.Value.ToString().ToUpper() == columnIDArray[j].Split('^')[1].ToUpper())
                    {
                        txtcellID.Value = columnIDArray[j].Split('^')[0];
                    }
                }
                dataGridView.Rows[n].ReadOnly = true;
            }
            #endregion

            if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
            {
                for (int displayParamCounter = dataGridView.Rows.Count - 1; displayParamCounter >= paramatersToSelect.Count; displayParamCounter--)
                {
                    dataGridView.Rows.RemoveAt(displayParamCounter);
                }
            }

            dataGridView.EndEdit();
            tabCtrlMeterConfiguration.SelectedIndex = 0;
            ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 0;
        }
        private int GetSelectedIndexModeOfBilling(int ModeofBilling)
        {
            if (ModeofBilling == 0)
                return ModeofBilling;
            if (ModeofBilling == 1)
                return ModeofBilling;

            return -1;
        }

        private int GetSelectedIndexBillingReset(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString().Length == 1)
                    {
                        string s = cmb.Items[i].ToString().PadLeft(2, '0');

                        if (s == strValue)
                        {
                            return i;
                        }
                    }
                    else if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }

                }
            }

            return -1;
        }

        #endregion

        #region Display RS232
        /// <summary>
        /// display RS232 data.
        /// </summary>
        /// <param name="rs232LockEntity"></param>
        private void DisplayRS232(RS232LockEntity rs232LockEntity)
        {
            if (rs232LockEntity.LockStatus == "Locked")
            {
                chkLockRS232Port.Checked = true;
            }
            else
            {
                chkLockRS232Port.Checked = false;
            }
        }
        #endregion

        #region Display Kvar
        /// <summary>
        /// display Kvar selection data.
        /// </summary>
        /// <param name="meterConfig"></param>
        /// <param name="ifMeterConfigDataFound"></param>
        private void DisplaykvarSelection(MeterConfigurationsNFEntity meterConfig)
        {
            if (meterConfig.kvarselectionEntity != null)
            {
                Control group = this.kvarSelection.Controls["grpkvarSelection"];
                RadioButton rdb = (RadioButton)group.Controls["rdbLagOnly"];

                if (meterConfig.kvarselectionEntity.LagOnly == "1")
                { rdb.Checked = true; }
                rdb = (RadioButton)group.Controls["rdbLagnLead"];

                if (meterConfig.kvarselectionEntity.LagandLead == "1")
                { rdb.Checked = true; }

                group.Enabled = false;
            }
        }
        #endregion

        #region Display AutoLock
        /// <summary>
        /// display auto lock data.
        /// </summary>
        /// <param name="autoLockEntity"></param>
        private void DisplayAutoLock(AutoLockEntity autoLockEntity)
        {
            if (autoLockEntity.AutoLockStatus == "NotLocked")
            {
                rdbAutoUnlock.Checked = true;
            }
            else if (autoLockEntity.AutoLockStatus == "Locked")
            {
                rdbAutoLock.Checked = true;
            }

        }
        #endregion

        #region DisplayDailyLog
        //To display the daily log parameters
        private void DisplayDailyLog(DailyLogEntity DailyLogEntity)
        {
            Control group = this.dailyLog1.Controls["gbDailyLog"];
            CheckBox chk1 = (CheckBox)group.Controls["chkSelectAll"];
            CheckBox chk = (CheckBox)group.Controls["chkCumulativeKwh"];
            if (DailyLogEntity.CumulativeKWh == "1")
                chk.Checked = true;
            CheckBox chk2 = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
            if (DailyLogEntity.CumulativeKVARhLag == "1")
                chk2.Checked = true;
            CheckBox chk3 = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
            if (DailyLogEntity.CumulativeKVARhLead == "1")
                chk3.Checked = true;
            CheckBox chk4 = (CheckBox)group.Controls["chkCumulativeKVAh"];
            if (DailyLogEntity.CumulativeKVAh == "1")
                chk4.Checked = true;
            CheckBox chk5 = (CheckBox)group.Controls["chkDailyMD1"];
            if (DailyLogEntity.DailyMD1 == "1")
                chk5.Checked = true;
            CheckBox chk6 = (CheckBox)group.Controls["chkDailyMD2"];
            if (DailyLogEntity.DailyMD2 == "1")
                chk6.Checked = true;
            if (chk.Checked == true && chk2.Checked == true && chk3.Checked == true && chk4.Checked == true && chk5.Checked == true && chk6.Checked == true)
            {
                chk1.Checked = true;
            }


        }

        #endregion

        #region display rtc
        /// <summary>
        /// display rtc data.
        /// </summary>
        /// <param name="rtc"></param>
        private void DisplayRTC(string rtc)
        {
            if (rtc != null)
            {
                rtcCtrl.Controls[0].Controls["txtRTC"].Text = rtc;
                rtcCtrl.Controls[0].Controls["txtRTC"].Enabled = false;
                DataGridView dataGridView = (DataGridView)rtcCtrl.Controls[0].Controls["dGridRTC"];
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewCell srlNoCell = new DataGridViewTextBoxCell();
                srlNoCell.Value = (dataGridView.Rows.Count + 1).ToString();
                dataGridViewRow.Cells.Add(srlNoCell);
                DataGridViewCell rtcDatetimeCell = new DataGridViewTextBoxCell();
                rtcDatetimeCell.Value = rtc;
                dataGridViewRow.Cells.Add(rtcDatetimeCell);
                dataGridView.Rows.Add(dataGridViewRow);
            }
        }
        #endregion
        /// <summary>
        /// Display TOD Data 
        /// </summary>
        private void DisplayTODData()
        {
            string todData = new TodBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

            if (ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS" &&  (todData == "" || todData.Length < 1))
            {
                tabControlReport.TabPages.Remove(tabPageMeterConfiguration);
                return;
            }

            if (!string.IsNullOrEmpty(todData) && todData.Contains("DLMS"))
            {
                string[] touData = todData.Split('\\');
                if (touData != null && touData.Length > 7)
                {
                    passiveSeasonProfile = SoapHexBinary.Parse(touData[1]).Value;
                    passiveWeekProfile = SoapHexBinary.Parse(touData[2]).Value;
                    passiveDayProfile = SoapHexBinary.Parse(touData[3]).Value;
                    activeSeasonProfile = SoapHexBinary.Parse(touData[4]).Value;
                    activeWeekProfile = SoapHexBinary.Parse(touData[5]).Value;
                    activeDayProfile = SoapHexBinary.Parse(touData[6]).Value;
                    passiveActivationDate = SoapHexBinary.Parse(touData[7]).Value;
                    if (MeterModelNumber == 24 || MeterModelNumber == 25)
                    {
                        specialDayProfile = SoapHexBinary.Parse(touData[8]).Value;
                        FillSpecialProfileParameters(specialDayProfile);
                    }

                    FillSeasonProfileParameters(passiveSeasonProfile);
                    FillWeekProfileParameters(passiveWeekProfile);
                    FillDayProfileParameters(passiveDayProfile, MeterModelNumber);
                    FillTOUActivationDate(passiveActivationDate);


                    //******* Meter Model Change Required Here ***********//
                    if (MeterModelNumber == NamePlateConstants.VBSPNoSeasonNoWeek
                        || MeterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek
                        || MeterModelNumber == NamePlateConstants.VIM_Series2
                        || MeterModelNumber == NamePlateConstants.ThreeTOUWCMValue 
                        || MeterModelNumber == NamePlateConstants.BRPL_7Slot
                        || MeterModelNumber == NamePlateConstants.BYPL_7Slot 
                        || MeterModelNumber == NamePlateConstants.BYPL_FD
                        || MeterModelNumber == NamePlateConstants.BRPL_CBSP //user story 1016689
                        )// ADD FOR 3PH THREE TOU SEASSION
                     
                    {
                        ShowSeasonWeekProfile(false);
                    }
                    else
                    {
                        ShowSeasonWeekProfile(true);
                    }

                }
            }
            else if (!string.IsNullOrEmpty(todData))
            {
                string[] toddataarray = todData.Split('\x04');
                FillSeasonProfileParametersIEC(toddataarray, MeterModelNumber);
                FillDayProfileParametersIEC(toddataarray, MeterModelNumber);
                FillWeekProfileParametersIEC(toddataarray, MeterModelNumber);
            }
            

            DisableTOUControls();
        }




        private void ShowSeasonWeekProfile(bool flag)
        {
            dgvWeekProfile.Visible = flag;
            dgvSeasonProfile.Visible = flag;
            label12.Visible = flag;
            label14.Visible = flag;
        }


        /// <summary>
        /// Display CT ratio from general
        /// </summary>
        private void DisplayCTRatio()
        {
            if (tabControlReport.TabPages.Contains(tabPageMeterConfiguration) && Convert.ToBoolean(element.CTRatio))
            {
                nudCTRatio.Value = CTRatio;
                nudCTRatio.Enabled = false;
            }
            else
            {
                tabCtrlMeterConfiguration.TabPages.Remove(tabPageCTRatio);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisplayPTRatio()
        {
            if (tabControlReport.TabPages.Contains(tabPageMeterConfiguration) && Convert.ToBoolean(element.PTRatio))
            {
                nudPTRatio.Value = PTRatio;
                nudPTRatio.Enabled = false;
            }
            else
            {
                tabCtrlMeterConfiguration.TabPages.Remove(tabPagePTRatio);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisableTOUControls()
        {
            foreach (DataGridView dayGrid in dayProfileGrids)
            {
                foreach (DataGridViewRow row in dayGrid.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ReadOnly = true;
                    }
                }
            }
            seasonProfileGrid.Enabled = false;
            weekProfileGrid.Enabled = false;
            touActivationDate.Enabled = false;
        }

        /// <summary>
        /// This method is used for filling session profile details in TOU grids.
        /// </summary>
        private void FillSeasonProfileParameters(byte[] buffer)
        {
            try
            {
                int nIndex = 2;
                //Bug ID 502787
                //Range Check , to avoid UI exception.
                int StartOfData = 0;
                StartOfData = buffer[5];
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                    
                  // nIndex += 4;
                    nIndex += 3;
                    nIndex += StartOfData;
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
            try
            {
                int StartOfData = 0;
                StartOfData = buffer[5];
                for (byte weekCount = 0; weekCount < weekProfileCount; weekCount++)
                {
                   //nIndex += 5;  
                   nIndex += 4;  
                    nIndex += StartOfData;
                    for (byte colCount = 1; colCount < 8; colCount++)
                    {
                        nIndex++;
                        //Bug ID 502787
                        //Range Check , to avoid UI exception.
                        int tariff = buffer[nIndex++];
                        if (tariff > 0 && tariff < 5)
                        {
                            weekProfileGrid.Rows[weekCount].Cells[colCount].Value = tariff.ToString("00");
                        }
                        else
                        {
                            weekProfileGrid.Rows[weekCount].Cells[colCount].Value = null;
                        }
                        //Top handle corrupt data case , bcz in this case UI shows exception.
                        //if (weekProfileGrid.Rows[weekCount].Cells[colCount].Value.ToString() == "00")
                        //{
                        //    weekProfileGrid.Rows[weekCount].Cells[colCount].Value = "01";
                        //}
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
                        int startHour = buffer[nIndex++];
                        int startMin = buffer[nIndex++];
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        //Max supported tariff 8
                        if (tariff > 0 && tariff < 9 &&  startHour >= 0 && startHour < 25 && startMin >= 0 && startMin < 60)
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();  
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour.ToString("00");   
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin.ToString("00");
                        }
                        else
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
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
                        int startHour = buffer[nIndex++];
                        int startMin = buffer[nIndex++];
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        //Max supported tariff 8
                        if (tariff > 0 && tariff < 9 && startHour >= 0 && startHour < 25 && startMin >= 0 && startMin < 60)
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour.ToString("00");
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin.ToString("00");
                        }
                        else
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
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
                            int startHour = buffer[nIndex++];
                            int startMin = buffer[nIndex++];
                            nIndex += 12;
                            int tariff = buffer[nIndex++];
                            //Max supported tariff 8
                            if (tariff > 0 && tariff < 9 && startHour >= 0 && startHour < 25 && startMin >= 0 && startMin < 60)
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour.ToString("00");
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin.ToString("00");
                            }
                            else
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                // throw ex;
                logger.Log(LOGLEVELS.Error, "FillDayProfileParameters(byte[] buffer, int meterModel)", ex);
            }
        }



        private void FillSpecialProfileParameters(byte[] specialDayProfile)
        {
            try
            {
                int nIndex = 11;
                SpecialDayProfileCount = Convert.ToByte(specialDayProfile[1]);
                for (byte seasonCount = 0; seasonCount < SpecialDayProfileCount; seasonCount++)
                {
                    int columnIndex = 1;
                    int Tariff = specialDayProfile[nIndex++];
                    //Bug ID 502787
                    //Month
                    if (Tariff > 0 && Tariff < 13)
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = Tariff.ToString("00");
                    }
                    else
                    {
                        SpecialDayProfileGrid.Rows[seasonCount].Cells[columnIndex++].Value = null;
                    }
                    Tariff = specialDayProfile[nIndex++];
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
                    Tariff = specialDayProfile[nIndex++];
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
                logger.Log(LOGLEVELS.Error, "FillSpecialProfileParameters(byte[] specialDayProfile)", ex);
                throw ex;
            }
        }


        /// <summary>
        /// This method is used for filling TOU activation date.
        /// </summary>
        /// <param name="buffer"></param>        
        private void FillTOUActivationDate(byte[] buffer)
        {
            int nIndex = 0x02;
            int activationYear = 0;
            try
            {
                activationYear = (activationYear | (int)buffer[nIndex++]) << 8;
                activationYear = (activationYear | (int)buffer[nIndex++]);
                int activationMonth = buffer[nIndex++];
                int activationDay = buffer[nIndex];
                //if future activation date is 00 then default 01-01-2001 for all meter
                if (activationYear == 0 && activationMonth == 0 && activationDay == 0)
                {
                    activationYear = 2001;
                    activationMonth = 01;
                    activationDay = 01;
                }
                touActivationDate.Value = Convert.ToDateTime(activationDay.ToString() + "/" + activationMonth.ToString() + "/" + activationYear.ToString(), new CultureInfo("hi-in"));


            }
            catch (Exception ex)    //Exception log for catch block
            {
                //  throw ex;
                logger.Log(LOGLEVELS.Error, "FillTOUActivationDate(byte[] buffer)", ex);
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
                else
                {
                    gridViewComboBox.Items.Add(index.ToString("00"));
                }
            }
            return gridViewComboBox;

        }

        /// <summary>
        /// Bind Input Grid with day profile attributes.
        /// </summary>
        /// <param name="dgvDayProfile"></param>
        private void InitializeDayProfile(DataGridView dgvDayProfile)
        {
            dgvDayProfile.ColumnCount = 0;
            dgvDayProfile.RowHeadersVisible = false;
            dgvDayProfile.Columns.Add(GetDataGridView(10, COLZONE, ZONE, 35));
            dgvDayProfile.Columns.Add(GetDataGridView(8, COLTARIFF, TARIFF, 39));
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
                dgvSeasonProfile.Columns.Add(GetDataGridView(31, COLDAY, DAY, width));
                dgvSeasonProfile.Columns.Add(GetDataGridView(12, COLMONTH, Month, width));
                dgvSeasonProfile.Columns.Add(GetDataGridView(12, COLSESSION, SEASONPROFILE, width));
                dgvSeasonProfile.RowCount = seasonProfileCount;
                dgvSeasonProfile.RowHeadersVisible = false;
                dgvSeasonProfile.Rows[0].Cells[COLDAY].ReadOnly = true;
                dgvSeasonProfile.Rows[0].Cells[COLMONTH].ReadOnly = true;
                //for (int index = 0; index < dgvSeasonProfile.RowCount; index++)
                //{
                //    dgvSeasonProfile.Rows[index].Cells[COLSESSION].Value = (index+1).ToString();
                //}
                dgvSeasonProfile.Columns[COLSESSION].ReadOnly = true;
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
                    FillDayProfileParameters(passiveDayProfile, MeterModelNumber);
                    FillTOUActivationDate(passiveActivationDate);
                }
            }
            else
            {
                if (activeSeasonProfile != null && activeWeekProfile != null && activeDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(activeSeasonProfile);
                    FillWeekProfileParameters(activeWeekProfile);
                    FillDayProfileParameters(activeDayProfile, MeterModelNumber);
                    FillTOUActivationDate(passiveActivationDate);
                }
            }
        }



        /// <summary>
        /// to get the selected index inside the combobox in Various forms of meter configuration.
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        internal int GetSelectedIndex(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        /// <summary>
        /// switch on button click from future to current
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbCurrentTOUS4SP_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }
        #region Manual Billing
        /// <summary>
        /// display Manual Billing data.
        /// </summary>
        /// <param name="manualBillingEntity"></param>
        private void DisplayManualBilling(ManualBillingEntity manualBillingEntity)
        {
            if (manualBillingEntity.ManualBillingStatus == "Enable")
            {
                rdbEnableManualBilling.Checked = true;
            }
            else if (manualBillingEntity.ManualBillingStatus == "Disable")
            {
                rdbDisableManualBilling.Checked = true;
            }

        }
        #endregion

        #region Software Billing
        /// <summary>
        /// display Software Billing data.
        /// </summary>
        /// <param name="softwareBillingEntity"></param>
        private void DisplaySoftwareBilling(SoftwareBillingEntity softwareBillingEntity)
        {
            if (softwareBillingEntity.SoftwareBillingStatus == "Enable")
            {
                rdbEnableSoftwareBilling.Checked = true;
            }
            else if (softwareBillingEntity.SoftwareBillingStatus == "Disable")
            {
                rdbDisableSoftwareBilling.Checked = true;
            }

        }

        private void DisplayManualMDReset(ManualMDResetEntity manualMDResetEntity)
        {
            if (manualMDResetEntity.ManualMDResetStatus == "Enable")
            {
                rdbEnablemanualmdreset.Checked = true;
            }
            else if (manualMDResetEntity.ManualMDResetStatus == "Disable")
            {
                rdbDisablemanualmdreset.Checked = true;
            }

        }
        #endregion
        private void lstTODAvgPF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterDataID))
            {
                if (lstTODMD.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODAvgPF.Items[lstTODMD.SelectedIndex])).Row.ItemArray[2].ToString();
                    lngTODAvgPF.Data = billingBLL.GetTODAvgPF(Convert.ToInt32(MeterDataID), Convert.ToInt32(historyId), false);
                    if (lngTODAvgPF.Data != null)
                    {
                        lngTODAvgPF.SetWidth("Tariff Number", 120);
                        lngTODAvgPF.SetWidth(1, 210);
                        lngTODAvgPF.RefreshGrid();
                    }
                    else
                    {

                        historyId = ((System.Data.DataRowView)(lstEnergy.Items[lstEnergy.SelectedIndex])).Row.ItemArray[2].ToString();
                        DataSet TOD_PF = billingBLL.GetMeterData_PF(Convert.ToInt32(MeterDataID), Convert.ToInt32(historyId));

                        lngTODAvgPF.Data = TOD_PF;
                        lngTODAvgPF.SetWidth("Tariff Number", 120);
                        lngTODAvgPF.SetWidth(1, 210);
                        lngTODAvgPF.RefreshGrid();

                    }

                }
            }
        }

        private void rdbCurrentTOUSmart_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }

        private void rdbFutureTOUSmart_CheckedChanged(object sender, EventArgs e)
        {
            SwitchActivePassiveTOU();
        }

        private void tabPagePTRatio_Click(object sender, EventArgs e)
        {

        }

        private void lngMainEnergy_Load(object sender, EventArgs e)
        {

        }

        /* VBM  Make sure that grid has scroll bars visible when window is resized */


        //private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Added for MPKWCL. To fill the miscellaneous columns on history selection.
        //    FillMiscellaneous();
        //}

        //private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        //{
        //    if (dtpFromDate.Value.Date == tmpDTPickFromDate.Date)
        //        dtpFromDate.Value = tmpDTPickFromDate;
        //    else if (tmpDTPickFromDate.Year == 1)
        //        return;
        //    else
        //        dtpFromDate.Value = dtpFromDate.Value.Date;
        //}

        //private void dtpToDate_ValueChanged(object sender, EventArgs e)
        //{
        //    if (dtpToDate.Value.Date == tmpDTPickToDate.Date)
        //        dtpToDate.Value = tmpDTPickToDate;
        //    else if (tmpDTPickToDate.Year == 1)
        //        return;
        //    else
        //        dtpToDate.Value = new DateTime(dtpToDate.Value.Date.Year, dtpToDate.Value.Date.Month, dtpToDate.Value.Date.Day, 23, 30, 0);
        //}

        /// <summary>
        /// Description: This method is used for filling day profile details in TOU grids for 1P IEC meters.
        /// Author: Mohsin Raza
        /// Date: 23-DEC-2016
        /// Remarks: Developed for torrent power limited as genric feature.
        /// </summary>
        private void FillDayProfileParametersIEC(string[] toudata, int meterModel)
        {
            try
            {
                byte[] buffer = new byte[255];
                const int MAXROWDAYTABLE = 4;

                for (int idaytable = 0; idaytable < MAXROWDAYTABLE; idaytable++)
                {

                    string[] rowdataarr = toudata[idaytable].Split(')');

                    for (int irowcount = 0; irowcount < rowdataarr.Length - 1; irowcount++)
                    {
                        string rowdata = rowdataarr[irowcount].Replace("(", "");
                        rowdata = rowdata.Replace(")", "");
                        int tariff;
                        int.TryParse(rowdata.Substring(0, 1), out tariff);

                        if (tariff == 0)
                        {
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            string startHour = rowdata.Substring(1, 2);
                            string startMin = rowdata.Substring(3, 2);
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLSTARTHOUR].Value = startHour;
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLSTARTMIN].Value = startMin;
                            dayProfileGrids[idaytable].Rows[irowcount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillDayProfileParametersIEC(string[] toudata, int meterModel)", ex);
            }
        }

        /// <summary>
        /// Description: This method is used for filling week profile details in TOU grids for 1P IEC meters.
        /// Author: Mohsin Raza
        /// Date: 23-DEC-2016
        /// Remarks: Developed for torrent power limited as genric feature.
        /// </summary>
        private void FillWeekProfileParametersIEC(string[] toudata, int meterModel)
        {
            try
            {
                const int MAXWEEKDAYS = 7;

                //for (int idaytable = weekProfileCount; idaytable < toudata.Length - 2; idaytable++)
                {

                    string[] rowdataarr = toudata[weekProfileCount].Split(')');

                    for (int irowcount = 0; irowcount < rowdataarr.Length - 1; irowcount++)
                    {
                        string rowdata = rowdataarr[irowcount].Replace("(", "");
                        rowdata = rowdata.Replace(")", "");
                        char[] coldata = rowdata.ToCharArray();

                        for (int icolcount = 0; icolcount < MAXWEEKDAYS; icolcount++)
                        {
                            int tariff;
                            int.TryParse(coldata[icolcount].ToString(), out tariff);

                            if (tariff > 0 && tariff < 5)
                            {
                                weekProfileGrid.Rows[irowcount].Cells[icolcount + 1].Value = tariff.ToString("00");
                            }
                            else
                            {
                                weekProfileGrid.Rows[irowcount].Cells[icolcount + 1].Value = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillWeekProfileParametersIEC(string[] toudata, int meterModel)", ex);
            }
        }

        /// <summary>
        /// Description: This method is used for filling week profile details in TOU grids for 1P IEC meters.
        /// Author: Mohsin Raza
        /// Date: 23-DEC-2016
        /// Remarks: Developed for torrent power limited as genric feature.
        /// </summary>
        private void FillSeasonProfileParametersIEC(string[] toudata, int meterModel)
        {
            try
            {
                const int MAXWEEKDAYS = 7;

                string[] rowdataarr = toudata[weekProfileCount].Split(')');

                for (byte irowcount = 0; irowcount < rowdataarr.Length - 1; irowcount++)
                {
                    string rowdata = rowdataarr[irowcount].Replace("(", "");
                    rowdata = rowdata.Replace(")", "");
                    string rowday = rowdata.Substring(MAXWEEKDAYS, 2);
                    string rowmonth = rowdata.Substring(MAXWEEKDAYS + 2, 2);
                    int tariff;
                    int.TryParse(rowday, out tariff);

                    seasonProfileGrid.Rows[irowcount].Cells[COLSESSION].Value = (irowcount + 1).ToString("00");

                    if (tariff > 0 && tariff < 13)
                        seasonProfileGrid.Rows[irowcount].Cells[COLDAY].Value = tariff.ToString("00");
                    else
                        seasonProfileGrid.Rows[irowcount].Cells[COLDAY].Value = null;

                    int.TryParse(rowmonth, out tariff);

                    if (tariff > 0 && tariff < 32)
                        seasonProfileGrid.Rows[irowcount].Cells[COLMONTH].Value = tariff.ToString("00");
                    else
                        seasonProfileGrid.Rows[irowcount].Cells[COLMONTH].Value = null;

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillSeasonProfileParametersIEC(string[] toudata, int meterModel)", ex);
            }
        }

        // SB Code change start 20161123 - Present Tamper Only
        private void chkPresents_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPresents.Checked)
            {
                DataSet dataset = new DataSet();
                dataset.Tables.Add(dataSetTamper.Tables[0].Clone());
                DataRow[] dataRow = dataSetTamper.Tables[0].Select("status='Present'");

                for (int i = 0; i < dataRow.Length; i++)
                {
                    dataset.Tables[0].Rows.Add(dataRow[i].ItemArray);
                }

                lngTamperSupported.Data = dataset;
            }
            else
            {
                lngTamperSupported.Data = dataSetTamper;
            }
        }
        // SB Code change end 20161123 - Present Tamper Only

        // SB Code change Start 20180629 - Multiple Analysis View
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        // SB Code change End 20180629 - Multiple Analysis View
    }
}
