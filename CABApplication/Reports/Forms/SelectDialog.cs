using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using LTCTBLL;
using System.ComponentModel;
using CABEntity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Channel.Formatter;
using System.Linq;
using Hunt.EPIC.Logging;
using CAB.E650MeterConfiguration;

namespace CAB.UI
{
    public partial class SelectDialog : CABForm
    {
        CABApplication.Reports.DLMS_Detailed_Reports.MainReport generalReport = null;
        private List<Enum> checkList;
        private List<string> lstClmPowerFactorNET = null;
        private List<string> lstClmTODPowerFactorNET = null;
        private List<string> lstClmMainEnergyNET = null;
        private List<string> lstClmMainEnergyConsumptionNET = null;
        private List<string> lstClmTODEnergyNET = null;
        private List<string> lstClmTODConsumptionEnergyNET = null;
        private List<string> lstClmMaxDemandNET = null;
        private List<string> lstClmTODDemandNET = null;
        private string MeterId = string.Empty;
        private string MeterSerialNo = string.Empty;
        private string meterVariant = string.Empty;
        private int meterModelNumber = 0;
        private int seasonNumber = 0;
        private string CABFileName = string.Empty;
        private FileReportDataSet reportXSD = null;//new FileReportDataSet();
        private string[] MDHeadings = { "Cumulative MD1", "Cumulative MD2", "Cumulative MD3" };
        private const string dateUnavailable = "--------";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        static List<string> lsHeadings;
        string headerLoadFactor = string.Empty;

        string headerLoadFactor1 = string.Empty;
        //pradipta_start_081018
        string headerkWImportLoadFactor = string.Empty;
        string headerkWExportLoadFactor = string.Empty;
        string headerkVAImportLoadFactor = string.Empty;
        string headerkVAExportLoadFactor = string.Empty;

        string averageLoadFactor = string.Empty;

        string averageImportLoadFactor = string.Empty;
        string averageExportLoadFactor = string.Empty;

        string averagekWImportLoadFactor = string.Empty;
        string averagekWExportLoadFactor = string.Empty;
        string averagekVAExportLoadFactor = string.Empty;
        string averagekVAImportLoadFactor = string.Empty;
        //pradipta_end_081018

        string headerMainEnergyBillingResetType = string.Empty;
        string headerAverageLoad = string.Empty;
        string dummyFirmwareVersion = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        // VBM
        private const string PowerFailureTable = "PowerFailureTable";
        private const string Duration = "Duration";
        // VBM
        //Dictionary<string, string> tamperCounterDictionary = new Dictionary<string, string>();
        ApplicationType types;
        private const string OCCURENCETIME = "OccurrenceTime";

        private const string CUMPOWERFAILURECOUNT = "Cumulative Power-Failure Count (0.0.96.7.0.255;1;2)";
        private const string CUMTAMPERCOUNT = "Cumulative Tamper Count (0.0.94.91.0.255;1;2)";
        private const string DELTATAMPERCOUNT = "Tamper Count (0.0.96.2.190.255;1;2)"; // Story - 345154
        private const string CUMBILLINGMDRESETCOUNT = "Cumulative Billing Count (0.0.0.1.0.255;1;2)";
        private const string ABCCodeBilling = "ABC Code(0.0.96.2.196.255;1;2)";

        private const string RESTORATIONTIME = "RestorationTime";
        private const string MISCELLENEOUS = "Miscelleneous";
        private const string COLNAMEPOWEROFFDURATION = "CumPowerOffDuration";
        private const string COLNAMETAMPERCOUNT = "CumTamperCount";
		private const string COLNAMEPOWERfailcount = "CumPowerOffDuration";
        private const string COLNAMETAMPERCOUNTcumulative = "CumTamperCount";
        private const string COLNAMEABCodeBilling = "ABCCodeBilling";

        private const string COLNAMEBillingCount = "BillingCount";
        private const string COLNAMETamperCountdelta = "TamperCount";
        private const string POWEROFFDURATION = "Cumulative Power-Failure Duration (0.0.94.91.8.255;3;2) (YY:MM:DDD HH:MM)";
        private const string TAMPERCOUNT = "Cumulative Tamper Count (0.0.94.91.0.255;1;2)";
        private const int TOTALBILLINGCOUNT = 12;
        private const string HISTORYID = "HistoryID";
        private const string CURRENT = "Current";
        private const string HISTORY = "History";
        private const string HYPHEN = "-";
        private const string SPACE = " ";
        private const string MISCDATANOTAVAILABLE = "Miscellaneous data not  available.";
        private const string FILEEXTENSION = ".FDL";
        /* GKG 13/02/2013 JDVVNL Utility Addition */
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        /* GKG 13/02/2013 JDVVNL Utility Addition */
        bool isMPKWCL = false;
        private DLMS650CommonBLL dlms650CommonBLL = null;
        private const int MDReset = 159;
        private const int PushModeConfig = 160;
        private const int ScrollModeConfig = 161;
        private const int HRModeConfig = 162;
        private const int ScrollTimeConfig = 163;
        private const int DemandIntegrationPeriod = 152;
        private const int ProfileCapturePeriod = 153;
        private const int SingleActionScheduleForBillingDates = 154;
        private List<TabEnum> listOfprofilesWithNoData;
        private static int powerFactorCount = 0;
        private const string Incorrect = "Incorrect";
        DataRow reportRow; //user story no 505185
        string chkPowerOnOffDurationFormat = string.Empty;
        public string meter_cat;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SelectDialog).ToString());
        private enum CheckBoxEnum
        {
            [Description("General Report")]
            GeneralReport,
            [Description("Instantaneous Report")]
            InstantReport,
            [Description("Main Energy")]
            MainEnergy,
            [Description("Main Energy Consumption")]
            MainEnergyCons,
            //[Description("Miscellaneous")]
            //Miscelleneous,
            //[Description("TOU Configuration")]
            //TOUConfig,
            [Description("TOD Energy")]
            TODEnergy,
            [Description("TOD Consumption")]
            TODConsumption,
            [Description("Demand")]
            Demand,
            [Description("Power Factor")]
            PowerFactor,

            [Description("TOD Power Factor")]//story 1024441 Add TOD Export PF
            TODPowerFactor,

            [Description("Phasor")]
            Phasor,
            //[Description("Power Off Duration")]
            //PowerOffDuration,
            [Description("All Tamper")]
            Tamper,
            [Description("Transactions")]
            Transactions,
            //[Description("Midnight Consumption")]
            //DailyEnergy,
            //[Description("Midnight Energies")]
            //MidNightEnergy,
            //[Description("Load survey")]
            LoadSurvey,
            [Description("Fraud Energy")]
            FraudEnergy,
            [Description("Meter Configurations")]
            MeterConfig,
            [Description("Load Factor")]
            LoadFactor,
            [Description("Power On Off Duration")]
            PowerOnOffDuration,
            [Description("Average Load")]
            AverageLoad,
            [Description("Miscelleneous")]
            Miscelleneous,
            [Description("Cumulative MD")]
            CumulativeMD,

        }

        public SelectDialog()
        {
            InitializeComponent();
            dlms650CommonBLL = new DLMS650CommonBLL();
            meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
            checkList = new List<Enum>();
            meterVariant = dlms650CommonBLL.GetMeterVariantByMeterDataID(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));  
            chkPowerOnOffDurationFormat = ConfigSettings.GetValue("ChkPowerOnOffDurationFormat");
            ApplyModernTheme();
        }

        private void ApplyModernTheme()
        {
            try
            {
                this.BackColor = Color.White;
                foreach (Control ctrl in this.Controls)
                {
                    StyleControl(ctrl);
                }
            }
            catch { }
        }

        private void StyleControl(Control ctrl)
        {
            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(29, 70, 150);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 9.5f);
                btn.Cursor = Cursors.Hand;
            }
            else if (ctrl is DataGridView dgv)
            {
                dgv.BackgroundColor = Color.White;
                dgv.BorderStyle = BorderStyle.None;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(29, 70, 150);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 235, 255);
                dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
                dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
                dgv.GridColor = Color.FromArgb(230, 235, 245);
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 255);
                dgv.RowHeadersVisible = false;
                dgv.AllowUserToResizeRows = false;
            }
            else if (ctrl is Label lbl)
            {
                lbl.Font = new Font("Segoe UI Semibold", 9.5f);
                lbl.ForeColor = Color.FromArgb(18, 50, 115);
            }
            else if (ctrl is GroupBox gb)
            {
                gb.ForeColor = Color.FromArgb(18, 50, 115);
                gb.Font = new Font("Segoe UI Semibold", 9.5f);
                foreach (Control child in ctrl.Controls) StyleControl(child);
            }
            else if (ctrl.HasChildren)
            {
                foreach (Control child in ctrl.Controls)
                {
                    StyleControl(child);
                }
            }
        }

        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            var general = new DLMS650GeneralBLL();
            DataSet resultSet =  new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
            var generalDS = general.GetMeterData((int)activeMeterDataId);
            resultSet.Tables[0].Columns.Add("MeterType", typeof(string));
            resultSet.Tables[0].Columns.Add("MeterModel", typeof(string));
            resultSet.Tables[0].Rows[0]["MeterType"] = generalDS.Tables[0].Select("`OBIS Code` = '0.0.94.91.9.255'")[0]["Value"].ToString();
            resultSet.Tables[0].Rows[0]["MeterModel"] = generalDS.Tables[0].Select("`OBIS Code` = '0.0.96.0.166.255'")[0]["Value"].ToString();

            return resultSet;
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }
        private DataSet ListNoPowerSupplyLoadTime(long activeMeterDataId, DataSet NoSupplyLoadData)
        {
            return new MeterDataBLL().ListNoPowerSupplyLoadTime(activeMeterDataId, NoSupplyLoadData);
        }
        /// <summary>
        /// Fetch the filename
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        /// <returns></returns>
        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }
        private DataSet ListGeneralData(long activeMeterDataId)
        {
            return new ReportBLL().GetGeneralReportData(activeMeterDataId);
        }

        private DataSet ListInstantData(long activeMeterDataId)
        {
            return new ReportBLL().GetInstantReportData(activeMeterDataId);
        }

        private DataSet ListTamperData(long activeMeterDataId)
        {
            return new ReportBLL().GetTamperReportData(activeMeterDataId);
        }

        private DataSet ListPowerOnHoursData(long activeMeterDataId)
        {
            return new ReportBLL().GetPowerOnHoursReportData(activeMeterDataId);
        }

        private DataSet ListPowerFactorData(long activeMeterDataId)
        {
            return new ReportBLL().GetPowerFactorReportData(activeMeterDataId);
        }

        private DataSet ListCTRatioData(long activeMeterDataId)
        {
            return new ReportBLL().GetCTRatioReportData(activeMeterDataId);
        }

        //added for MVVNL
        private DataSet ListMidnightEnergies(long activeMeterDataId)
        {
            return new ReportBLL().GetMidnightEnergiesReportData(activeMeterDataId);
        }
        //added for MVVNL

        private DataSet ListLoadFactorData(long activeMeterDataId)
        {
            return new ReportBLL().GetLoadFactorReportData(activeMeterDataId);
        }

        private DataSet ListBillingTamperCounterData(long activeMeterDataId)
        {
            return new ReportBLL().GetBillingTamperCounterReportData(activeMeterDataId);
        }

        private DataSet ListMainEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }

        private DataSet ListConsumptionEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }
        private DataSet ListTOUEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }

        /// <summary>
        /// get fraud energy data from database
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        /// <returns></returns>
        private FraudEnergyEntity ListFraudEnergyData(long activeMeterDataId)
        {
            return new FraudEnergyBLL().GetFraudEnergy(activeMeterDataId) as FraudEnergyEntity;
        }
        /// <summary>
        /// get TOD data from database
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        /// <returns></returns>
        private string ListTODData(long activeMeterDataId)
        {
            return new TodBLL().GetData(activeMeterDataId);
        }

        private string ListRTCData(long activeMeterDataId)
        {
            return new RTCBLL().GetData(activeMeterDataId);
        }

        private int ListDIPData(long activeMeterDataId)
        {
            return new DIPBLL().GetData(activeMeterDataId);
        }

        private int ListSIPData(long activeMeterDataId)
        {
            return new LSIPBLL().GetData(activeMeterDataId);
        }

        private string ListPulseEnergyData(long activeMeterDataId)
        {
            return new PulseEnergyBLL().GetData(activeMeterDataId);
        }

        private MeterConfigurationsNFEntity ListKvahData(long activeMeterDataId)
        {
            return new kvarSelectionBLL().GetData(activeMeterDataId) as MeterConfigurationsNFEntity;

        }
        private AutoLockEntity ListAutoLockData(long activeMeterDataId)
        {
            return new AutoLockBLL().GetData(activeMeterDataId) as AutoLockEntity;

        }

        private SoftwareBillingEntity ListSoftwareBillData(long activeMeterDataId)
        {
            return new SoftwareBillingBLL().GetData(activeMeterDataId) as SoftwareBillingEntity;

        }

        private ManualMDResetEntity ListManualMDResetData(long activeMeterDataId)
        {
            return new ManualMDResetBLL().GetData(activeMeterDataId) as ManualMDResetEntity;

        }

        private BillingTypeEntity ListBillingTypeData(long activeMeterDataId)
        {
            return new BillingTypeBLL().GetData(activeMeterDataId);
        }

        private string ListDiscconectData(long activeMeterDataId)
        {
            return new DisconnectControlBLL().GetData(activeMeterDataId);
        }

        private string ListLoadControl(long activeMeterDataId)
        {
            return new LoadControlBLL().GetData(activeMeterDataId);
        }
        private string ListRS485Control(long activeMeterDataId)
        {
            return new RS485BLL().GetData(activeMeterDataId);
        }

        private string ListPaymentData(long activeMeterDataId)
        {
            return new PaymentModeBLL().GetData(activeMeterDataId);
        }

        private string ListMeteringModeData(long activeMeterDataId)
        {
            return new MeteringModeBLL().GetData(activeMeterDataId);
        }

        private string ListLoadLimitData(long activeMeterDataId)
        {
            return new LoadLimitBLL().GetData(activeMeterDataId);
        }

        private string ListSlidingDemandData(long activeMeterDataId)
        {
            return new SlidingDemandBLL().GetData(activeMeterDataId);
        }
        private string ListOpticalLockData(long activeMeterDataId)
        {
            return new OpticalLockUnlockBLL().GetData(activeMeterDataId);
        }
        private string ListRJLockData(long activeMeterDataId)
        {
            return new RJLockUnlockBLL().GetData(activeMeterDataId);
        }

        //private string ListBillingTypeData(long activeMeterDataId)
        //{
        //    return new LoadControlBLL().GetData(activeMeterDataId);
        //}

        #region "Fill The XSD with the DATASET Values"

        private void FillMeterID(DataSet meterIdDS)
        {
            //DataRow reportRow; user story no 505185
            if (meterIdDS != null && meterIdDS.Tables[0].Rows.Count > 0)
            {
                //reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in meterIdDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = dateUnavailable;

                    reportRow["ReadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));

                    if (MeterFileList.filesource == "CMRI")
                    {
                        string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                        reportRow["CMRITime"] = CMRITime.Substring(11);
                    }
                    else
                        reportRow["CMRITime"] = "";

                    reportRow["UploadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                    string FileCreationTime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                    reportRow["FileCreationTime"] = FileCreationTime.Substring(11);

                    if (!DBNull.Value.Equals(row["NoLoadDataTime"]))
                    {
                        reportRow["NoLoadDataTime"] = row["NoLoadDataTime"];
                    }
                    if (!DBNull.Value.Equals(row["NoPowerSupplyTime"]))
                    {
                        reportRow["NoPowerSupplyTime"] = row["NoPowerSupplyTime"];
                    }

                    reportRow["MeterModel"] = row["MeterModel"].ToString();
                    reportRow["MeterType"] = row["MeterType"].ToString();
                }
               //reportRow["ActiveMeter"] = "Inactive";
                reportRow["ActiveMeter"] = "";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                // reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow); user story no 505185
                
            }
        }

        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            //DataRow reportRow;
            DataTable table = new DataTable();
            if (detailsDS.Tables[0].Rows.Count > 0)
            {
                //reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in detailsDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                        reportRow["ConsumerNo"] = CommonBLL.GetFormattedData(row["Consumer_Number"].ToString());
                    else
                        reportRow["ConsumerNo"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_Location"].ToString()))
                        reportRow["Location"] = CommonBLL.GetFormattedData(row["Meter_Location"].ToString());
                    else
                        reportRow["Location"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_AllocationDate"].ToString()))
                        reportRow["InstallationDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
                    else
                        reportRow["InstallationDate"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterType_Name"].ToString()))
                        reportRow["MeterType"] = CommonBLL.GetFormattedData(row["MeterType_Name"].ToString());
                    else
                        reportRow["MeterType"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterModel_Name"].ToString()))
                        reportRow["MeterModel"] = CommonBLL.GetFormattedData(row["MeterModel_Name"].ToString());
                    else
                        reportRow["MeterModel"] = dateUnavailable;

                    /* GKG 13/02/2013 JDVVNL Utility Addition */
                    //if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                    //    reportRow["EMF"] = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                    //else
                    //    reportRow["EMF"] = dateUnavailable;
                    decimal actualEMF = 0;
                    int internalCTRatio = 0;
                    int internalPTRatio = 0;

                    String meterEMF = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                    //BhardwajG : EMF Bug
                    if (int.TryParse(CommonBLL.GetFormattedData(row["internalCTRatio"].ToString()), out internalCTRatio)
                        && int.TryParse(CommonBLL.GetFormattedData(row["internalPTRatio"].ToString()), out internalPTRatio))
                    {

                    }
                    if (internalCTRatio <= 0)
                    {
                        internalCTRatio = 1;
                    }
                    if (internalPTRatio <= 0)
                    {
                        internalPTRatio = 1;
                    }
                    actualEMF = Convert.ToDecimal(meterEMF) / (internalPTRatio * internalCTRatio);

                    /* GKG 146126 EMF resolution issue */
                    //meterEMF = actualEMF.ToString();
                    meterEMF = string.Format("{0:F3}", actualEMF);
                    /* GKG 146126 EMF resolution issue */

                    string emfApplied = CommonBLL.GetFormattedData(row["UseEMFInCalculations"].ToString());
                    if (emfApplied == "1")
                    {
                        emfApplied = APPLIED;
                    }
                    else
                    {
                        emfApplied = NOTAPPLIED;
                    }
                    meterEMF = meterEMF + " (" + emfApplied + ")";

                    if (!string.IsNullOrEmpty(meterEMF))
                        reportRow["EMF"] = meterEMF;
                    else
                        reportRow["EMF"] = dateUnavailable;
                    /* GKG 13/02/2013 JDVVNL Utility Addition */

                    if (!string.IsNullOrEmpty(row["Region_Name"].ToString()))
                        reportRow["Region"] = CommonBLL.GetFormattedData(row["Region_Name"].ToString());
                    else
                        reportRow["Region"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Circle_Name"].ToString()))
                        reportRow["Circle"] = CommonBLL.GetFormattedData(row["Circle_Name"].ToString());
                    else
                        reportRow["Circle"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Division_Name"].ToString()))
                        reportRow["Division"] = CommonBLL.GetFormattedData(row["Division_Name"].ToString());
                    else
                        reportRow["Division"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["CMRI_Number"].ToString()))
                        reportRow["CMRINumber"] = CommonBLL.GetFormattedData(row["CMRI_Number"].ToString());
                    else
                        reportRow["CMRINumber"] = dateUnavailable;

                    if (row["Status"].ToString().Equals("0"))
                        reportRow["ActiveMeter"] = "Inactive";
                    else
                        reportRow["ActiveMeter"] = "Active";

                    if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                        reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                    else
                        reportRow["ContractDemand"] = dateUnavailable;

                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportRow["ReadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                    if (MeterFileList.filesource == "CMRI")
                    {
                        string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                        reportRow["CMRITime"] = CMRITime.Substring(11);
                    }
                    else
                        reportRow["CMRITime"] = "";
                    //string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                    //reportRow["CMRITime"] = CMRITime.Substring(11); 
                    reportRow["UploadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                    string FileCreationTime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                    reportRow["FileCreationTime"] = FileCreationTime.Substring(11);

                    if (!DBNull.Value.Equals(row["NoLoadDataTime"]))
                    {
                        reportRow["NoLoadDataTime"] = row["NoLoadDataTime"];
                    }
                    if (!DBNull.Value.Equals(row["NoPowerSupplyTime"]))
                    {
                        reportRow["NoPowerSupplyTime"] = row["NoPowerSupplyTime"];
                    }

                    //reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                    break; // VBM - Take only first Row as a meter willl have one customer. .
                }
            }
        }
        /* GKG JVVNL Current TOU Read */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="touConfiguration"></param>
        private void FillTouConfigurationXSD(DataSet touConfiguration)
        {
            DataRow reportRow;
            foreach (DataRow row in touConfiguration.Tables[0].Rows)
            {
                reportRow = reportXSD.Tables["TouConfiguration"].NewRow();
                reportRow["S. No."] = row["S. No."].ToString();
                reportRow["Slot No."] = row["Slot No."].ToString();
                reportRow["Zone Start Time(HH:MM)"] = row["Zone Start Time(HH:MM)"].ToString();
                reportRow["Zone End Time(HH:MM)"] = row["Zone End Time(HH:MM)"].ToString();
                reportRow["Tariff Zone"] = row["Tariff Zone"].ToString();
                reportXSD.Tables["TouConfiguration"].Rows.Add(reportRow);
            }

        }
        /* GKG JVVNL Current TOU Read */
        private void FillGeneralXSD(DataSet generalData)
        {
            DataRow reportRow;
            dummyFirmwareVersion = string.Empty;
            if (generalData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in generalData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["NameplateDetails"].NewRow();
                        reportRow["Description"] = CommonBLL.GetFormattedData(row["Descriptions"].ToString());
                        reportRow["OBIS Code"] = CommonBLL.GetFormattedData(row["OBIS Code"].ToString());
                        reportRow["Class ID"] = CommonBLL.GetFormattedData(row["Class ID"].ToString());
                        reportRow["Attribute"] = CommonBLL.GetFormattedData(row["Attribute"].ToString());
                        if (reportRow["OBIS Code"].ToString() == "0.0.96.1.0.255")
                            reportRow["Value"] = row["Value"].ToString();
                        else
                            reportRow["Value"] = CommonBLL.GetFormattedData(row["Value"].ToString());

                        //MSEDCL Bug Fix
                        if (reportRow["Description"].ToString() == "Internal Firmware Version")
                        {
                            dummyFirmwareVersion = reportRow["Value"].ToString();
                            if ((dummyFirmwareVersion == "2.21") || (dummyFirmwareVersion == ""))
                            {
                                reportRow["Value"] = "----";
                            }
                        }

                       // if (reportRow["Description"].ToString() == "Current Rating (Ib-Imax)")
                            if (reportRow["Description"].ToString() == "Current Rating")//PGVCL
                            {
                            if (dummyFirmwareVersion == "1.66" || dummyFirmwareVersion == "4.64" || dummyFirmwareVersion == "0.00" || dummyFirmwareVersion == "4.58" || dummyFirmwareVersion == "2.21")
                            {
                                reportRow["Value"] = "----";
                            }
                        }
                        if (reportRow["Description"].ToString() == "Meter Constant")
                        {                           
                            if (ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS")
                            {
                                reportRow["OBIS Code"] = "0.0.0.0.0.255";
                            }
                        }                        

                        reportXSD.Tables["NameplateDetails"].Rows.Add(reportRow);
                    }
                }
                else
                {
                    foreach (DataRow row in generalData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["BillingGeneralTable"].NewRow();
                        reportRow["MeterID"] = row["MeterID"].ToString();
                        reportRow["MeterDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row["MeterDateTime"].ToString())).ToString(dateFormat);
                        reportRow["ErrorCode"] = CommonBLL.GetFormattedData(row["ErrorCode"].ToString());
                        reportRow["MeterConstant"] = string.Concat(CommonBLL.GetFormattedData(row["MeterConstant"].ToString()), " imp/kWh,imp/kvarh");
                        reportRow["FirmwareVersion"] = CommonBLL.GetFormattedData(row["FirmwareVersion"].ToString());
                        reportRow["CTRatio"] = CommonBLL.GetFormattedData(row["CTRatio"].ToString());
                        reportRow["VoltagePhaseSequence"] = CommonBLL.GetFormattedData(row["VoltagePhaseSequence"].ToString());
                        reportRow["TotalActiveEnergy"] = CommonBLL.GetFormattedData(row["TotalFundamentalActiveEnergy"].ToString());
                        reportRow["CumulativeMD1"] = CommonBLL.GetFormattedData(row["CMD1"].ToString());
                        reportRow["CumulativeMD2"] = CommonBLL.GetFormattedData(row["CMD2"].ToString());
                        reportRow["CumulativeMD3"] = CommonBLL.GetFormattedData(row["CMD3"].ToString());
                        reportRow["TotalPowerOnHours"] = CommonBLL.GetFormattedData(row["TotalPowerOnHours"].ToString());
                        reportRow["BatteryPowerOnHours"] = CommonBLL.GetFormattedData(row["BateryModePowerOnHour"].ToString());
                        reportRow["MD Reset Count"] = CommonBLL.GetFormattedData(row["MDResetCounter"].ToString());
                        reportRow["ReadOut Counters"] = CommonBLL.GetFormattedData(row["ReadoutCounter"].ToString());
                        reportRow["Programming Counters"] = CommonBLL.GetFormattedData(row["ProgrammingCounter"].ToString());
                        reportRow["CT Ratio Programming Counters"] = CommonBLL.GetFormattedData(row["CTRatioProgrammingCounter"].ToString());

                        string val = Convert.ToString(row["LatestTamperOccurrenceID"]);
                        if (val.ToUpper().IndexOf("PF") > 0)
                            val = val.Substring(0, val.ToUpper().IndexOf("PF") - 1);

                        if (val.ToUpper().IndexOf("LOW") > 0)
                            val = val + " PF";
                        reportRow["Latest Tamper Occurence"] = val;

                        if (row["OccurrenceTime"].ToString() != "0")
                            reportRow["Occurrence Time"] = DateUtility.LongToDateTime(Convert.ToInt64(row["OccurrenceTime"].ToString())).ToString(dateFormat);
                        else
                            reportRow["Occurrence Time"] = dateUnavailable;

                        val = CommonBLL.GetFormattedData(row["LatestTamperRestorationID"].ToString());
                        if (val.ToUpper().IndexOf("PF") > 0)
                            val = val.Substring(0, val.ToUpper().IndexOf("PF") - 1);
                        if (val.ToUpper().IndexOf("PHASE LOW") > 0)
                            val = val + " PF";
                        reportRow["Latest Tamper Restored"] = val;

                        if (row["RestorationTime"].ToString() != "0")
                            reportRow["Restoration Time"] = DateUtility.LongToDateTime(Convert.ToInt64(row["RestorationTime"].ToString())).ToString(dateFormat);
                        else
                            reportRow["Restoration Time"] = dateUnavailable;

                        reportRow["Cumulative Active Energy"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKWH"].ToString());
                        reportRow["Cumulative Apparent Energy"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVAH"].ToString());
                        reportRow["Cumulative Reactive Energy (Lag)"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVARHLag"].ToString());
                        reportRow["Cumulative Reactive Energy (Lead)"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVARHLead"].ToString());

                        reportRow["Current Month MD1"] = CommonBLL.GetFormattedData(row["CumulativeMD1"].ToString());
                        if (row["CumulativeMD1TimeStamp"].ToString() != "0")
                            reportRow["MD1 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD1TimeStamp"].ToString()) + "00")).ToString(dateFormat);
                        else
                            reportRow["MD1 TimeStamp"] = dateUnavailable;

                        reportRow["Current Month MD2"] = CommonBLL.GetFormattedData(row["CumulativeMD2"].ToString());
                        if (row["CumulativeMD2TimeStamp"].ToString() != "0")
                            reportRow["MD2 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD2TimeStamp"].ToString()) + "00")).ToString(dateFormat);
                        else
                            reportRow["MD2 TimeStamp"] = dateUnavailable;

                        reportRow["Current Month MD3"] = CommonBLL.GetFormattedData(row["CumulativeMD3"].ToString());
                        if (row["CumulativeMD3TimeStamp"].ToString() != "0")
                            reportRow["MD3 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD3TimeStamp"].ToString()) + "00")).ToString(dateFormat);
                        else
                            reportRow["MD3 TimeStamp"] = dateUnavailable;
                        reportXSD.Tables["BillingGeneralTable"].Rows.Add(reportRow);
                    }
                }
            }
        }

        /// <summary>
        /// Used to fill anomaly xsd table 
        /// </summary>
        /// <param name="objAnomalyEntity"></param>
        private void FillAnomalyXSD(AnomalyEntity objAnomalyEntity)
        {
            DataRow reportRow;
            reportRow = reportXSD.Tables["Anomaly"].NewRow();
            reportRow["Flash"] = (objAnomalyEntity.Flash == 1) ? "OK" : "NOT OK";
            reportRow["EEPROM"] = (objAnomalyEntity.EeProm == 1) ? "OK" : "NOT OK";
            reportRow["SMPS"] = (objAnomalyEntity.Smps == 1) ? "OK" : "NOT OK";
            reportRow["RTC"] = (objAnomalyEntity.Rtc == 1) ? "OK" : "NOT OK";
            reportRow["MAIN BATTERY"] = (objAnomalyEntity.MainBattery == 1) ? "OK" : "NOT OK";
            reportXSD.Tables["Anomaly"].Rows.Add(reportRow);

        }

        /// <summary>
        /// Used to fill anomaly xsd table 
        /// </summary>
        /// <param name="objAnomalyEntity"></param>
        private void FillAnomalyXSD(DataSet anomalyData)
        {
            DataRow reportRow;
            if (anomalyData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in anomalyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["AnomalyDynamic"].NewRow();
                        reportRow["Parameter"] = CommonBLL.GetFormattedData(row["Parameters"].ToString());
                        reportRow["Value"] = row["Status"].ToString();
                        reportXSD.Tables["AnomalyDynamic"].Rows.Add(reportRow);
                    }
                }
            }
        }

        private void FillABCXSD(DataSet ABCData,string abc,string meterserial)
        {
            DataRow reportRow;
            try
            {
                if (ABCData.Tables[0].Rows.Count > 0 && abc != null && meterserial != null)
                {
                    reportRow = reportXSD.Tables["AbcCode1"].NewRow();
                    reportRow["ABCCode"] = abc;
                    reportRow["MeterSerial"] = meterserial;
                   // reportXSD.Tables["AbcCode1"].Rows.Add(reportRow);

                    types = ConfigInfo.GetApplicationType();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        int i = 0;
                        foreach (DataRow row in ABCData.Tables[0].Rows)
                        {
                            if (i != 0)
                            {
                               reportRow = reportXSD.Tables["AbcCode1"].NewRow();
                            }
                                reportRow["Parameter"] = row[0].ToString();
                                reportRow["Value"] = row[1].ToString();
                                reportXSD.Tables["AbcCode1"].Rows.Add(reportRow);
                            
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillABCXSD(DataSet ABCData,string abc,string meterserial)", ex);
            }
        }

        private void FillInstantXSD(DataSet instantData)
        {
            DataRow reportRow;
            if (instantData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in instantData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650InstantTable"].NewRow();
                        // Story - 349654 - To remove the ABC code from the detailed report as its length is too high to display in the report
                        if (!(CommonBLL.GetFormattedData(row["Descriptions"].ToString()).ToUpper() == "ABC"))
                        {
                            reportRow["InstantPowerColumnName"] = CommonBLL.GetFormattedData(row["Descriptions"].ToString());
                            reportRow["InstantPowerColumnValue"] = CommonBLL.GetFormattedData(row["Value"].ToString());
                            reportRow["InstantPowerObisCode"] = CommonBLL.GetFormattedData(row["OBIS Code"].ToString());
                            reportRow["InstantPowerClassID"] = CommonBLL.GetFormattedData(row["Class ID"].ToString());
                            reportRow["InstantPowerAttribute"] = CommonBLL.GetFormattedData(row["Attribute"].ToString());
                            reportRow["Unit"] = CommonBLL.GetFormattedData(row["Unit"].ToString());
                            reportXSD.Tables["DLMS650InstantTable"].Rows.Add(reportRow);
                        }

                    }
                }
                else
                {
                    foreach (DataRow row in instantData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["InstantTable"].NewRow();
                        reportRow["voltage_RPhase"] = CommonBLL.GetFormattedData(row["VoltageRPhase"].ToString());
                        reportRow["voltage_YPhase"] = CommonBLL.GetFormattedData(row["VoltageYPhase"].ToString());
                        reportRow["voltage_BPhase"] = CommonBLL.GetFormattedData(row["VoltageBPhase"].ToString());
                        reportRow["Current_RPhase"] = CommonBLL.GetFormattedData(row["CurrentRPhase"].ToString());
                        reportRow["Current_YPhase"] = CommonBLL.GetFormattedData(row["CurrentYPhase"].ToString());
                        reportRow["Current_BPhase"] = CommonBLL.GetFormattedData(row["CurrentBPhase"].ToString());
                        reportRow["activePower"] = CommonBLL.GetFormattedData(row["InstantActivepower"].ToString());
                        reportRow["apparentPower"] = CommonBLL.GetFormattedData(row["InstantApparentPower"].ToString());
                        reportRow["reactivePowerLag"] = CommonBLL.GetFormattedData(row["InstantReactiveLagPower"].ToString());
                        reportRow["reaactivePowerLead"] = CommonBLL.GetFormattedData(row["InstantReactiveLeadPower"].ToString());
                        reportRow["totalPowerFactor"] = CommonBLL.GetFormattedData(row["TotalPowerFactor"].ToString());
                        reportRow["PowerFactor_Rphase"] = CommonBLL.GetFormattedData(row["PowerFactorRPhase"].ToString());
                        reportRow["PowerFactor_Yphase"] = CommonBLL.GetFormattedData(row["PowerFactorYPhase"].ToString());
                        reportRow["PowerFactor_Bphase"] = CommonBLL.GetFormattedData(row["PowerFactorBPhase"].ToString());
                        reportRow["AveragePowerFactor"] = CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString());
                        reportRow["Frequency"] = CommonBLL.GetFormattedData(row["Frequency"].ToString());
                        reportRow["RisingDemand_KW"] = CommonBLL.GetFormattedData(row["RisingDemandKW"].ToString());
                        reportRow["ElapsedTime_KW"] = CommonBLL.GetFormattedData(row["ElapsedTimeKW"].ToString());
                        reportRow["RisingDemand_KVA"] = CommonBLL.GetFormattedData(row["RisingDemandKVA"].ToString());
                        reportRow["ElapsedTime_KVA"] = CommonBLL.GetFormattedData(row["ElapsedTimeKVA"].ToString());
                        reportXSD.Tables["InstantTable"].Rows.Add(reportRow);
                    }
                }
            }
        }

        /// <summary>
        /// Methods fills data to Phasor table
        /// </summary>
        /// <param name="phasorData"></param>
        private void FillPhasorXSD(DataSet phasorData)
        {
            DataRow reportRow;
            if (phasorData != null && phasorData.Tables != null && phasorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PhasorTable"].NewRow();
                Dictionary<string, string> phasorColumns = new DLMS650CommonBLL().GetPhasorDisplayParameter();
                foreach (KeyValuePair<string, string> phasorItem in phasorColumns)
                {
                    if (phasorData.Tables[0].Rows[0][phasorItem.Key].ToString().ToUpper() == "INVALID")
                        reportRow[phasorItem.Key] = Incorrect;
                    else
                        reportRow[phasorItem.Key] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[0][phasorItem.Key].ToString());
                }
                reportXSD.Tables["PhasorTable"].Rows.Add(reportRow);
            }
        }

        private void FillPowerOnHoursXSD(DataSet powerOnHoursData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerOnHoursData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerOnHoursTable"].NewRow();
                foreach (DataRow row in powerOnHoursData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = CommonBLL.GetFormattedHourData(row["PowerOnHours"].ToString());
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        reportRow[colName] = CommonBLL.GetFormattedHourData(row["PowerOnHours"].ToString());
                    }
                    historyID++;
                }
                reportXSD.Tables["PowerOnHoursTable"].Rows.Add(reportRow);
            }
        }
        /// <summary>
        /// Used to fill Power off duration in report.
        /// </summary>
        /// <param name="powerOffDuration"></param>
        private void FillPowerOffDurationXSD(DataSet powerOffDuration)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerOffDuration != null && powerOffDuration.Tables.Count > 0 && powerOffDuration.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerOffDuration"].NewRow();
                foreach (DataRow powerOffDurationRow in powerOffDuration.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = powerOffDurationRow["Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm"].ToString();
                        reportRow["CumulativeCurrent"] = powerOffDurationRow["Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm"].ToString();
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        reportRow[colName] = powerOffDurationRow["Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm"].ToString();

                        colName = "CumulativeHistory -" + historyID.ToString();
                        reportRow[colName] = powerOffDurationRow["Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm"].ToString();
                    }
                    historyID++;
                }
                reportXSD.Tables["PowerOffDuration"].Rows.Add(reportRow);
            }
        }
        private void FillMiscelleneous(DataSet miscelleneousData)
        {
            if (miscelleneousData.Tables[0].Rows[0][CUMPOWERFAILURECOUNT].ToString() == "-1")
                miscelleneousData.Tables[0].Columns.Remove(CUMPOWERFAILURECOUNT);
            if (miscelleneousData.Tables[0].Rows[0][CUMTAMPERCOUNT].ToString() == "-1")
                miscelleneousData.Tables[0].Columns.Remove(CUMTAMPERCOUNT);
            if (miscelleneousData.Tables[0].Rows[0][CUMBILLINGMDRESETCOUNT].ToString() == "-1")
                miscelleneousData.Tables[0].Columns.Remove(CUMBILLINGMDRESETCOUNT);
            if (miscelleneousData.Tables[0].Rows[0][DELTATAMPERCOUNT].ToString() == "-1") // Story - 345154
                miscelleneousData.Tables[0].Columns.Remove(DELTATAMPERCOUNT);
            if (miscelleneousData.Tables[0].Rows[0][ABCCodeBilling].ToString() == null || miscelleneousData.Tables[0].Rows[0][ABCCodeBilling].ToString() == "")
                miscelleneousData.Tables[0].Columns.Remove(ABCCodeBilling);
            // Special case to give priority to Delta Tamper count in case both are coming, Story - 345154
            if (miscelleneousData.Tables[0].Columns.Contains(CUMTAMPERCOUNT) && miscelleneousData.Tables[0].Columns.Contains(DELTATAMPERCOUNT))
            {
                miscelleneousData.Tables[0].Columns.Remove(CUMTAMPERCOUNT);
            }

            DataRow reportRow = null;
            if (miscelleneousData.FirstTableHasRows() && miscelleneousData.Tables[0].Columns.Count>2 )
            {
                foreach (DataRow row in miscelleneousData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables[MISCELLENEOUS].NewRow();
                    //Added to calculate the history wise power off duration.
                    if (row[HISTORY].ToString().Contains(CURRENT))
                    {
                        reportRow[HISTORYID] = CURRENT;
                        if (miscelleneousData.Tables[0].Columns.Contains(DELTATAMPERCOUNT))
                        {
                            reportRow[COLNAMETamperCountdelta] = row[DELTATAMPERCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMETamperCountdelta))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMETamperCountdelta);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMTAMPERCOUNT))
                        {
                            reportRow[COLNAMETAMPERCOUNTcumulative] = row[CUMTAMPERCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMETAMPERCOUNTcumulative))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMETAMPERCOUNTcumulative);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMBILLINGMDRESETCOUNT))
                        {
                            reportRow[COLNAMEBillingCount] = row[CUMBILLINGMDRESETCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEBillingCount))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEBillingCount);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMPOWERFAILURECOUNT))
                        {
                            reportRow[COLNAMEPOWERfailcount] = row[CUMPOWERFAILURECOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEPOWERfailcount))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEPOWERfailcount);
                        }
                     
                        if (miscelleneousData.Tables[0].Columns.Contains(ABCCodeBilling))
                        {
                            reportRow[COLNAMEABCodeBilling] = row[ABCCodeBilling];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEABCodeBilling))
                            reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEABCodeBilling);
                        }
                       
                        reportXSD.Tables[MISCELLENEOUS].Rows.Add(reportRow);

                    }
                    else
                    {
                        reportRow[HISTORYID] = row[HISTORY].ToString();
                        if (miscelleneousData.Tables[0].Columns.Contains(DELTATAMPERCOUNT))
                        {
                            reportRow[COLNAMETamperCountdelta] = row[DELTATAMPERCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMETamperCountdelta))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMETamperCountdelta);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMTAMPERCOUNT))
                        {
                            reportRow[COLNAMETAMPERCOUNTcumulative] = row[CUMTAMPERCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMETAMPERCOUNTcumulative))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMETAMPERCOUNTcumulative);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMBILLINGMDRESETCOUNT))
                        {
                            reportRow[COLNAMEBillingCount] = row[CUMBILLINGMDRESETCOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEBillingCount))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEBillingCount);
                        }
                        if (miscelleneousData.Tables[0].Columns.Contains(CUMPOWERFAILURECOUNT))
                        {
                            reportRow[COLNAMEPOWERfailcount] = row[CUMPOWERFAILURECOUNT];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEPOWERfailcount))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEPOWERfailcount);
                        }

                        if (miscelleneousData.Tables[0].Columns.Contains(ABCCodeBilling))
                        {
                            reportRow[COLNAMEABCodeBilling] = row[ABCCodeBilling];
                        }
                        else
                        {
                            if (reportXSD.Tables[MISCELLENEOUS].Columns.Contains(COLNAMEABCodeBilling))
                                reportXSD.Tables[MISCELLENEOUS].Columns.Remove(COLNAMEABCodeBilling);
                        }

                        reportXSD.Tables[MISCELLENEOUS].Rows.Add(reportRow);
                    }
                }

            }

        }
        private void FillPowerFactorXSD(DataSet powerFactorData, string fileName)
        {
            DataRow reportRow;
            int historyID = 1;
            string colName = string.Empty;
            powerFactorCount = 0;
            if (powerFactorData.Tables[0].Rows.Count > 0)
            {
                powerFactorCount = powerFactorData.Tables[0].Rows.Count;
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["DLMS650PowerFactorTable"].NewRow();
                    foreach (DataRow row in powerFactorData.Tables[0].Rows)
                    {
                        // Added to solve bug 95899
                        if (row[HISTORY].ToString().ToUpper().Contains("CURRENT"))
                        {
                            reportRow["lblCurrent"] = row[0].ToString();
                            reportRow["Current"] = row[2].ToString();
                        }
                        else
                        {
                            colName = "History -" + historyID.ToString();
                            reportRow["lbl" + colName] = row[0].ToString();
                            reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[2].ToString().ToString()));
                            historyID++;
                        }
                    }
                    reportXSD.Tables["DLMS650PowerFactorTable"].Rows.Add(reportRow);
                }
                else
                {

                    reportRow = reportXSD.Tables["PowerFactorTable"].NewRow();
                    foreach (DataRow row in powerFactorData.Tables[0].Rows)
                    {
                        if (historyID == 0)
                        {
                            reportRow["Current"] = row[1].ToString();
                        }
                        else
                        {
                            colName = "History -" + historyID.ToString();
                            reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[0].ToString().ToString()));
                        }
                        historyID++;
                    }
                    reportXSD.Tables["PowerFactorTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTariffPowerFactorXSD(DataSet tariffPF)
        {
            DataRow reportRow;
            string colName = string.Empty;
            if (tariffPF.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in tariffPF.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["TODPowerFactorTable"].NewRow();
                    foreach (DataColumn col in tariffPF.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                            reportRow["History"] = row[col].ToString();
                        else
                            reportRow[string.Concat("Tariff", col.Ordinal.ToString())] = CommonBLL.RemoveUnit(row[col].ToString());
                    }
                    reportXSD.Tables["TODPowerFactorTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillLoadFactorXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["LoadFactorTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["LoadFactor"].ToString()));
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["LoadFactor"].ToString().ToString()));
                    }
                    historyID++;
                }
                reportXSD.Tables["LoadFactorTable"].Rows.Add(reportRow);
            }
        }

        private void FillCumulativeMDXSD(DataSet MDData)// for smart meter
        {
                 
            DataRow reportRow;
            string colName = string.Empty;
            if (MDData.Tables[0].Rows.Count > 0)
            {
                                        
             foreach (DataRow row in MDData.Tables[0].Rows)
            {
                reportRow = reportXSD.Tables["DLMS650CumulativeMDTable"].NewRow();
                reportRow["History"] = row["History"].ToString();
                reportRow["Billing Date Time (0.0.0.1.2.255;3;2)"] = row["Billing DateTime(0.0.0.1.2.255;3;2)"].ToString();
                reportRow["Cumulative MD kw(1.0.1.2.0.255;3;2)"] = row["Cumulative MD kw(1.0.1.2.0.255;3;2)"].ToString();
                reportRow["Cumulative MD kva (1.0.9.2.0.255;3;2)"] = row["Cumulative MD kva(1.0.9.2.0.255;3;2)"].ToString();
                //reportRow["Cumulative MD kw Export(1.0.2.2.0.255;3;2)"] = row["Cumulative MD kw Export(1.0.2.2.0.255;3;2)"].ToString();
                //reportRow["Cumulative MD kva Export (1.0.10.2.0.255;3;2)"] = row["Cumulative MD kva Export(1.0.10.2.0.255;3;2)"].ToString();
                    reportXSD.Tables["DLMS650CumulativeMDTable"].Rows.Add(reportRow);
                }
                
            }
        }


        private void FillCTRatioXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["CTRatioTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = CommonBLL.GetFormattedData(row["CTRatio"].ToString());
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        string val = CommonBLL.GetFormattedData(row["CTRatio"].ToString().ToString());
                        if (val.IndexOf('-') > 0)
                            val = val.Substring(0, val.IndexOf('-'));
                        try
                        {
                            val = Int32.Parse(val).ToString();
                        }
                        catch (Exception)
                        {
                            val = "0";
                        }
                        reportRow[colName] = val;
                    }
                    historyID++;
                }
                reportXSD.Tables["CTRatioTable"].Rows.Add(reportRow);
            }
        }

        //added for MVVNL

        private string[] CheckUnit(string val)
        {
            string[] unit = new string[2];
            unit[0] = unit[1] = string.Empty;
            if (string.IsNullOrEmpty(val))
                return unit;
            string[] data = val.Split('*');
            unit[0] = data[0];
            if (data.Length == 2)
                unit[1] = data[1];
            return unit;
        }

        //private void FillMidnightEnergyXSD(DataSet midnightEnergyData)
        //{
        //    DataRow reportRow;
        //    int paramID;
        //    string colName = string.Empty;

        //    if (midnightEnergyData.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in midnightEnergyData.Tables[0].Rows)
        //        {
        //            reportRow = reportXSD.Tables["MidNightEnergies"].NewRow();
        //            paramID = 0;
        //            foreach (DataColumn column in midnightEnergyData.Tables[0].Columns)
        //            {
        //                if (UtilityDetails.ShowMidnight)
        //                {
        //                    if (isPUMA)
        //                    {
        //                        reportRow[paramID] = row[paramID].ToString();
        //                    }
        //                    else
        //                    {
        //                        if (paramID == 0)
        //                        {
        //                            if (!string.IsNullOrEmpty(row[paramID].ToString()))
        //                            {
        //                                DateTime recordTimestamp = DateUtility.LongToDateTime(Int64.Parse(Convert.ToString(row[paramID])));
        //                                reportRow[paramID] = recordTimestamp.Day.ToString("00") + "/ " + recordTimestamp.Month.ToString("00") + "/" + recordTimestamp.Year;
        //                            }
        //                            else
        //                            {
        //                                reportRow[paramID] = "-------";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!string.IsNullOrEmpty(row[paramID].ToString()))
        //                            {
        //                                reportRow[paramID] = CheckUnit(row[paramID].ToString())[0];
        //                            }
        //                            else
        //                            {
        //                                reportRow[paramID] = "-------";
        //                            }
        //                        }

        //                    }
        //                    paramID++;
        //                }
        //            }
        //            reportXSD.Tables["MidNightEnergies"].Rows.Add(reportRow);
        //        }

        //    }
        //}
        //added for MVVNL

        private void FillBillingMechanismXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            string sTemp = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingMechanismTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    sTemp = row["CTRatio"].ToString();
                    if (historyID == 0)
                    {
                        if (sTemp.IndexOf('-') != -1)
                            reportRow["Current"] = sTemp.Substring(sTemp.IndexOf('-') + 1);
                        else
                            reportRow["Current"] = "-------";
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        if (sTemp.IndexOf('-') != -1)
                            reportRow[colName] = sTemp.Substring(sTemp.IndexOf('-') + 1);
                        else
                            reportRow[colName] = "-------";
                    }
                    historyID++;
                }
                reportXSD.Tables["BillingMechanismTable"].Rows.Add(reportRow);
            }
        }

        private void FillBillingTamperCounterXSD(DataSet billingTamperCounterData)
        {
            DataRow reportRow;
            int historyID = 0;
            string tamperDescription = string.Empty;
            string colName = string.Empty;
            Dictionary<string, string> tamperCounterDictionary = new TamperCounterBLL().CreateTamperCounterDictionary();
            if (billingTamperCounterData.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn col in billingTamperCounterData.Tables[0].Columns)
                {
                    if (!tamperCounterDictionary.TryGetValue(col.ColumnName, out tamperDescription))
                        tamperDescription = string.Empty;
                    else
                    {
                        historyID = 0;
                        reportRow = reportXSD.Tables["BillingTamperCounterTable"].NewRow();
                        reportRow["TamperParameters"] = tamperDescription;
                        if (tamperDescription == "Current Phase Reversal Tamper")
                            continue;
                        foreach (DataRow row in billingTamperCounterData.Tables[0].Rows)
                        {
                            if (historyID == 0)
                                reportRow["Current"] = row[col.ColumnName].ToString();
                            else
                            {
                                colName = "History" + historyID.ToString();
                                reportRow[colName] = row[col.ColumnName].ToString().ToString();
                            }
                            historyID++;
                        }
                        reportXSD.Tables["BillingTamperCounterTable"].Rows.Add(reportRow);
                    }
                }
            }
        }



        private void FillMainEnergyXSD_NET(DataTable dtMainEnergy, string XSDTableName)
        {
            DataRow reportRow;
            foreach (DataRow row in dtMainEnergy.Rows)
            {
                reportRow = reportXSD.Tables[XSDTableName].NewRow();
                for (int colCount = 0; colCount < dtMainEnergy.Columns.Count; colCount++)
                {
                    if (colCount == 0)
                    {
                        reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                    }
                    else if (colCount == 1)
                    {
                        if (row[colCount].ToString() != "0")
                            reportRow[colCount] = row[colCount].ToString();// + "00",dateFormat);
                        else
                            reportRow[colCount] = dateUnavailable;
                    }
                    else
                    {
                        reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                    }
                }
                reportXSD.Tables[XSDTableName].Rows.Add(reportRow);
            }
        }
        private void FillMainEnergyXSD(DataSet mainEnergyData)
        {
            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                FillDLMSMainEnergyXSD(mainEnergyData);
            }
            else if (types.Equals(ApplicationType.IEC_LTCT_650))
            {
                FillIECMainEnergyXSD(mainEnergyData);
            }
        }

        private void FillDLMSMainEnergyXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DLMS650MainEnergyTable"].NewRow();

                    for (int colCount = 0; colCount < mainEnergyData.Tables[0].Columns.Count; colCount++)
                    {
                        string ColumnName = mainEnergyData.Tables[0].Columns[colCount].ColumnName;
                        if (ColumnName.Contains("Billing Type"))
                        {
                            headerMainEnergyBillingResetType = ColumnName;
                        }


                        if (colCount == 0)
                        {
                            reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                        }
                        else if (colCount == 1)
                        {
                            if (row[colCount].ToString() != "0")
                                reportRow[colCount] = row[colCount].ToString();// + "00",dateFormat);
                            else
                                reportRow[colCount] = dateUnavailable;
                        }
                        else
                        {
                            reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                        }
                    }
                    reportXSD.Tables["DLMS650MainEnergyTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillIECMainEnergyXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;
            foreach (DataRow row in mainEnergyData.Tables[0].Rows)
            {
                reportRow = reportXSD.Tables["MainEnergyTable"].NewRow();
                string val = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                if (val == "0")
                    reportRow["History"] = "Current";
                else
                    reportRow["History"] = "History-" + val;
                if (row["BillingTimestamp"].ToString() != "0")
                    reportRow["BillingDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["BillingTimestamp"].ToString() + "00")).ToString(dateFormat);
                else
                    reportRow["BillingDate"] = dateUnavailable;
                reportRow["kWh"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                reportRow["kVAh"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                reportRow["KVARh (Lag)"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                reportRow["KVARh (Lead)"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                reportXSD.Tables["MainEnergyTable"].Rows.Add(reportRow);
            }
        }

        private void FillTariffEnergyXSD(DataSet mainEnergyData, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;
            DataRow reportRow;
            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650TODEnergyTable"].NewRow();
                        for (int colCount = 0; colCount < mainEnergyData.Tables[0].Columns.Count; colCount++)
                        {
                            reportRow[colCount] = row[colCount].ToString();
                        }

                        reportXSD.Tables["DLMS650TODEnergyTable"].Rows.Add(reportRow);
                    }
                }
                else
                {
                    row_kWh = reportXSD.Tables["TODKWhTable"].NewRow();
                    row_kVAh = reportXSD.Tables["TODKVAhTable"].NewRow();
                    row_kVArh_lag = reportXSD.Tables["TODKVARhLagTable"].NewRow();
                    row_kVAhrg_lead = reportXSD.Tables["TODKVARhLeadTable"].NewRow();

                    if (historyID == 0)
                        row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = "Current";
                    else
                        row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = string.Concat("History ", String.Format("{0:00}", historyID));

                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        row_kWh[string.Concat("Tariff", row["Tariff Number"])] = row["kWh"].ToString();
                        row_kVAh[string.Concat("Tariff", row["Tariff Number"])] = row["kVAh"].ToString();
                        row_kVArh_lag[string.Concat("Tariff", row["Tariff Number"])] = row["kVArh (Lag)"].ToString();
                        row_kVAhrg_lead[string.Concat("Tariff", row["Tariff Number"])] = row["kVArh (Lead)"].ToString();
                    }
                    reportXSD.Tables["TODKWhTable"].Rows.Add(row_kWh);
                    reportXSD.Tables["TODKVAhTable"].Rows.Add(row_kVAh);
                    reportXSD.Tables["TODKVARhLagTable"].Rows.Add(row_kVArh_lag);
                    reportXSD.Tables["TODKVARhLeadTable"].Rows.Add(row_kVAhrg_lead);
                }
            }
        }

        private void FillMaximumDemandXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650MaximumDemandTable"].NewRow();

                        for (int colCount = 0; colCount < mainEnergyData.Tables[0].Columns.Count; colCount++)
                        {
                            // User Story - 1000867
                            if (mainEnergyData.Tables[0].Columns[colCount].ColumnName == CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLag))
                            { reportRow[12] = row[colCount].ToString(); continue; }
                            else if (mainEnergyData.Tables[0].Columns[colCount].ColumnName == CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLagTIMESTAMP))
                            { reportRow[13] = row[colCount].ToString() == "0" || row[colCount].ToString() == "" ? dateUnavailable : row[colCount].ToString(); continue; }
                            else if (mainEnergyData.Tables[0].Columns[colCount].ColumnName == CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLead))
                            { reportRow[14] = row[colCount].ToString(); continue; }
                            else if (mainEnergyData.Tables[0].Columns[colCount].ColumnName == CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLeadTIMESTAMP))
                            { reportRow[15] = row[colCount].ToString() == "0" || row[colCount].ToString() == "" ? dateUnavailable : row[colCount].ToString(); continue; }

                            if (colCount == 1 || colCount == 3 || colCount == 5)
                            {
                                if (row[colCount].ToString() == "0" || row[colCount].ToString() == "")
                                    reportRow[colCount] = dateUnavailable;

                                else
                                    reportRow[colCount] = row[colCount].ToString();
                            }
                            else { reportRow[colCount] = row[colCount].ToString(); }
                        }
                        reportXSD.Tables["DLMS650MaximumDemandTable"].Rows.Add(reportRow);

                    }
                }
                else
                {
                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["MaximumDemandTable"].NewRow();
                        string val = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                        if (val == "0")
                            reportRow["History"] = "Current";
                        else
                            reportRow["History"] = "History-" + val;
                        if (row["BillingTimestamp"].ToString() != "0")
                            reportRow["BillingDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["BillingTimestamp"].ToString())).ToString(dateFormat);
                        else
                            reportRow["BillingDate"] = dateUnavailable;
                        reportRow["MD1"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD1"].ToString()));
                        if (row["CumulativeMD1TimeStamp"].ToString() != "0")
                            reportRow["MD1_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD1TimeStamp"].ToString())).ToString(dateFormat);
                        else
                            reportRow["MD1_TimeStamp"] = dateUnavailable;

                        reportRow["MD2"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD2"].ToString()));
                        if (row["CumulativeMD2TimeStamp"].ToString() != "0")
                            reportRow["MD2_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD2TimeStamp"].ToString())).ToString(dateFormat);
                        else
                            reportRow["MD2_TimeStamp"] = dateUnavailable;

                        reportRow["MD3"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD3"].ToString()));
                        if (row["CumulativeMD3TimeStamp"].ToString() != "0")
                            reportRow["MD3_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD3TimeStamp"].ToString())).ToString(dateFormat);
                        else
                            reportRow["MD3_TimeStamp"] = dateUnavailable;
                        reportXSD.Tables["MaximumDemandTable"].Rows.Add(reportRow);
                    }
                }
            }
        }

        private void FillTODMDXSD(DataSet TODMDData, int historyID)
        {
            DataRow reportRow;
            if (TODMDData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in TODMDData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650TODDemandTable"].NewRow();

                        for (int colCount = 0; colCount < TODMDData.Tables[0].Columns.Count; colCount++)
                        {
                            if (colCount == 3 || colCount == 5)
                            {
                                if (row[colCount].ToString() == "0" || row[colCount].ToString() == "")
                                    reportRow[colCount] = dateUnavailable;

                                else
                                    reportRow[colCount] = row[colCount].ToString();
                            }
                            else
                            { reportRow[colCount] = row[colCount].ToString(); }
                        }
                        reportXSD.Tables["DLMS650TODDemandTable"].Rows.Add(reportRow);
                    }
                }
                else
                {
                    foreach (DataRow row in TODMDData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["TODDemandTable"].NewRow();
                        reportRow["History"] = "History " + historyID;
                        reportRow["Tariff"] = row["Tariff Number"];
                        reportRow["MD1"] = row[1];
                        if (row[2].ToString() != "0")
                            reportRow["MD1_TimeStamp"] = row[2];
                        else
                            reportRow["MD1_TimeStamp"] = dateUnavailable;
                        reportRow["MD2"] = row[3];
                        if (row[4].ToString() != "0")
                            reportRow["MD2_TimeStamp"] = row[4];
                        else
                            reportRow["MD2_TimeStamp"] = dateUnavailable;
                        reportRow["MD3"] = row[5];
                        if (row[6].ToString() != "0")
                            reportRow["MD3_TimeStamp"] = row[6];
                        else
                            reportRow["MD3_TimeStamp"] = dateUnavailable;
                        reportXSD.Tables["TODDemandTable"].Rows.Add(reportRow);
                    }
                }
            }
        }

        private void FillEnergyConsumptionXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;
            int i = 0;
            string prevKWh = string.Empty;
            string prevKVAh = string.Empty;
            string prevKVARhLag = string.Empty;
            string prevKVARhLead = string.Empty;
            string prevHistory = string.Empty;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650EnergyConsumptionTable"].NewRow();

                        for (int colCount = 0; colCount < mainEnergyData.Tables[0].Columns.Count; colCount++)
                        {
                            if (colCount == 0)
                            {
                                reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                            }
                            // Removed condition colCount == 1.
                            else
                            {
                                reportRow[colCount] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[colCount].ToString()));
                            }
                        }
                        reportXSD.Tables["DLMS650EnergyConsumptionTable"].Rows.Add(reportRow);
                    }
                }
                else
                {
                    foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                    {
                        if (i == 0)
                        {
                            prevHistory = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                            prevKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                            prevKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                            prevKVARhLag = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                            prevKVARhLead = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                            i++;
                        }
                        else
                        {
                            reportRow = reportXSD.Tables["EnergyConsumptionTable"].NewRow();
                            reportRow["History"] = string.Concat("History ", prevHistory, " - ", CommonBLL.GetFormattedData(row["History_ID"].ToString()));
                            reportRow["kWh"] = (Convert.ToDecimal(prevKWh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString())))).ToString("0.000");
                            reportRow["kVAh"] = (Convert.ToDecimal(prevKVAh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString())))).ToString("0.000");
                            reportRow["KVARhLag"] = (Convert.ToDecimal(prevKVARhLag) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString())))).ToString("0.000");
                            reportRow["KVARhLead"] = (Convert.ToDecimal(prevKVARhLead) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString())))).ToString("0.000");
                            reportXSD.Tables["EnergyConsumptionTable"].Rows.Add(reportRow);
                            prevHistory = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                            prevKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                            prevKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                            prevKVARhLag = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                            prevKVARhLead = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                        }
                    }
                }
            }
        }



        private void FillTODEnergyConsumptionXSD(DataSet currentTariffEnergyDS, DataSet nextTariffEnergyDS, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;
            DataRow reportRow;
            int i = 0;
            types = ConfigInfo.GetApplicationType();
            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                foreach (DataRow row in currentTariffEnergyDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].NewRow();
                    for (int colCount = 0; colCount < currentTariffEnergyDS.Tables[0].Columns.Count; colCount++)
                    {
                        reportRow[colCount] = row[colCount].ToString();
                    }
                    reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].Rows.Add(reportRow);
                }
            }
            else
            {
                row_kWh = reportXSD.Tables["TODKWhConsumptionTable"].NewRow();
                row_kVAh = reportXSD.Tables["TODKVAhConsumptionTable"].NewRow();
                row_kVArh_lag = reportXSD.Tables["TODKVARhLagConsumptionTable"].NewRow();
                row_kVAhrg_lead = reportXSD.Tables["TODKVARhLeadConsumptionTable"].NewRow();

                row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = string.Concat(String.Format("{0:00}", historyID), " - ", String.Format("{0:00}", historyID + 1));
                foreach (DataRow row in currentTariffEnergyDS.Tables[0].Rows)
                {
                    row_kWh[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kWh"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kWh"].ToString()))).ToString("0.000");
                    row_kVAh[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVAh"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVAh"].ToString()))).ToString("0.000");
                    row_kVArh_lag[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVArh (Lag)"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVArh (Lag)"].ToString()))).ToString("0.000");
                    row_kVAhrg_lead[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVArh (Lead)"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVArh (Lead)"].ToString()))).ToString("0.000");
                    i++;
                }
                reportXSD.Tables["TODKWhConsumptionTable"].Rows.Add(row_kWh);
                reportXSD.Tables["TODKVAhConsumptionTable"].Rows.Add(row_kVAh);
                reportXSD.Tables["TODKVARhLagConsumptionTable"].Rows.Add(row_kVArh_lag);
                reportXSD.Tables["TODKVARhLeadConsumptionTable"].Rows.Add(row_kVAhrg_lead);
            }
        }

        private void FillLoadSurveyXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            int dateTimeCount = 0;
            DateTime PreviousDate = DateTime.Now;
            try
            {
                if (loadSurveyData == null || loadSurveyData.Tables[0].Rows.Count == 0)
                    return;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                        lsHeadings.Add(col.ColumnName);
                    foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                    {
                        DataTable LoadsurveyTable = new DataTable();
                        if (SMD_rbtnLoadSurveyDemand.Checked)
                        { LoadsurveyTable = reportXSD.Tables["DLMS650LoadSurvey"]; }
                        else
                        { LoadsurveyTable = reportXSD.Tables["DLMS650LoadSurveyEnergy"]; }
                        reportRow = LoadsurveyTable.NewRow();
                        //To Fill -- when values are not available
                        for (int colCount = 0; colCount < LoadsurveyTable.Columns.Count; colCount++)
                        {
                            reportRow[colCount] = "----";
                        }
                        string dateTimes = Convert.ToString(row[0]);
                        if (dateTimes.Length > 10)
                            dateTimes = dateTimes.Substring(0, 10);
                        reportRow["DateColumn"] = dateTimes;
                        for (int colCount = 0; colCount < loadSurveyData.Tables[0].Columns.Count; colCount++)
                        {
                            if (colCount == 0)
                            { reportRow["Real Time Clock - Time (0.0.1.0.0.255;8;2)"] = Convert.ToString(row[0]).Substring(11, Convert.ToString(row[0]).Length - 11); }
                            else
                            { reportRow[lsHeadings[colCount]] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[colCount].ToString())); }
                        }
                        LoadsurveyTable.Rows.Add(reportRow);
                    }
                }
                else
                {
                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                        lsHeadings.Add(col.ColumnName);
                    foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                        if (dateTimeCount == 0)
                        {
                            PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(row[0]))).Date;
                            reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitDateUnit(Convert.ToString(row[0])));
                            dateTimeCount++;
                        }
                        else
                        {
                            string dates = "";
                            DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(row[0]))).Date;
                            TimeSpan ts = currentDate - PreviousDate;
                            if (ts.Days > 0)
                            {
                                currentDate = currentDate.AddDays(-1);
                                long datesval = DateUtility.DateTimeToLong(currentDate);
                                dates = DateUtility.LongToStringDateFormat(datesval);
                                reportRow["GroupDateTime"] = dates;
                                PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(row[0]))).Date;
                            }
                            else
                            {
                                dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitDateUnit(Convert.ToString(row[0])));
                                reportRow["GroupDateTime"] = dates;
                            }
                        }
                        for (int colCount = 1; colCount < loadSurveyData.Tables[0].Columns.Count - 1; colCount++)
                        {
                            string ParameterColValue = "Parameter" + Convert.ToString(colCount);
                            reportRow[ParameterColValue] = row[colCount].ToString();
                        }
                        string dateTimes = Convert.ToString(row[0]);
                        if (dateTimes.Length > 10)
                            dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                        reportRow["TimeColumn"] = dateTimes;
                        reportXSD.Tables["LoadSurveyTable"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,System.Reflection.MethodInfo.GetCurrentMethod().Name);
                logger.Log(LOGLEVELS.Error, "FillLoadSurveyXSD(DataSet loadSurveyData)", ex);
            }
        }

        private void FillDLMSTamperXSD(DataSet tamperData)
        {
                DataRow reportRow;
                if (tamperData != null && tamperData.Tables.Count > 0 && tamperData.Tables[0].Rows.Count > 0)
                {
                    #region WB Specific to hide Tamper Parameters and set default values for magnet restore
                    if (meterModelNumber == 9)
                    {
                        DataRow[] rowDelete = tamperData.Tables[0].Select("TamperDescription = 'Over Voltage in any Phase - Occurrence' or TamperDescription = 'Over Voltage in any Phase - Restoration' or TamperDescription = 'Low Voltage in any Phase - Occurrence' or TamperDescription = 'Low Voltage in any Phase - Restoration'");
                        if (rowDelete != null && rowDelete.Length > 0)
                        {
                            for (int rowCount = 0; rowCount < rowDelete.Length; rowCount++)
                            {
                                tamperData.Tables[0].Rows.Remove(rowDelete[rowCount]);
                                tamperData.AcceptChanges();
                            }
                        }
                    }
                    #endregion
                    for (int rowCount = 0; rowCount < tamperData.Tables[0].Rows.Count; rowCount++)
                    {
                        reportRow = reportXSD.Tables["DLMSTamperTable"].NewRow();
                        reportRow["EventCode"] = tamperData.Tables[0].Rows[rowCount][0].ToString();
                        reportRow["TamperDescription"] = tamperData.Tables[0].Rows[rowCount][1].ToString();//row["EventCode"].ToString().ToString();
                        reportRow["DateTimeEvent"] = tamperData.Tables[0].Rows[rowCount][2].ToString();//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString())));
                        reportRow["CurrentIR"] = tamperData.Tables[0].Rows[rowCount][3].ToString(); //DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperRestoredTime"].ToString())));
                        reportRow["CurrentIY"] = tamperData.Tables[0].Rows[rowCount][4].ToString(); //CommonBLL.GetFormattedData(row["RVoltageRestored"].ToString());
                        reportRow["CurrentIB"] = tamperData.Tables[0].Rows[rowCount][5].ToString(); //CommonBLL.GetFormattedData(row["YVoltageRestored"].ToString());
                        reportRow["VoltageVRN"] = tamperData.Tables[0].Rows[rowCount][7].ToString(); //CommonBLL.GetFormattedData(row["BVoltageRestored"].ToString());
                        reportRow["VoltageVYN"] = tamperData.Tables[0].Rows[rowCount][8].ToString(); //CommonBLL.GetFormattedData(row["RCurrentRestored"].ToString());
                        reportRow["VoltageVBN"] = tamperData.Tables[0].Rows[rowCount][9].ToString(); //CommonBLL.GetFormattedData(row["YCurrentRestored"].ToString());
                        reportRow["PowerFactorRphase"] = tamperData.Tables[0].Rows[rowCount][11].ToString(); //CommonBLL.GetFormattedData(row["BCurrentRestored"].ToString());
                        reportRow["PowerFactorYphase"] = tamperData.Tables[0].Rows[rowCount][12].ToString(); //CommonBLL.GetFormattedData(row["RPFRestored"].ToString());
                        reportRow["PowerFactorBphase"] = tamperData.Tables[0].Rows[rowCount][13].ToString(); //CommonBLL.GetFormattedData(row["YPFRestored"].ToString());
                        reportRow["Temprature"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][14].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][14].ToString();
                        reportRow["TotalPowerFactor"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][15].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][15].ToString();
                        reportRow["CumulativeEnergykWh"] = tamperData.Tables[0].Rows[rowCount][16].ToString(); //CommonBLL.GetFormattedData(row["BPFRestored"].ToString());
                        reportRow["CumulativeEnergykVAh"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][17].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][17].ToString();

                        // reportRow["HighNeutralCurrent"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][16].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][16].ToString();//add pradipta_neu

                        //SarkarA code change start 20180110 // add new tamper parameters
                        reportRow["NeutralCurrent"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][18].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][18].ToString();
                        reportRow["HighNeutralCurrent"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][24].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][24].ToString();
                        reportRow["kWr"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][25].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][25].ToString();
                        reportRow["kWy"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][26].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][26].ToString();
                        reportRow["kWb"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][27].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][27].ToString();
                        reportRow["kVAr"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][28].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][28].ToString();
                        reportRow["kVAy"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][29].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][29].ToString();
                        reportRow["kVAb"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount][30].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount][30].ToString();

                        reportRow["CumulativeTamperCount"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["CumulativeTamperCount"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["CumulativeTamperCount"].ToString();

                        reportRow["ActiveCurrentR"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["ActiveCurrentR"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["ActiveCurrentR"].ToString();
                        reportRow["ActiveCurrentY"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["ActiveCurrentY"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["ActiveCurrentY"].ToString();
                        reportRow["ActiveCurrentB"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["ActiveCurrentB"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["ActiveCurrentB"].ToString();
                        
                        //SarkarA code change end 20180110
                        //SarkarA code change start 20180330 // add phase current instant, frequency
                        reportRow["Frequency"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["Frequency"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["Frequency"].ToString();
                        //SarkarA code change end 20180330
                        reportRow["THDVR"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDVR"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDVR"].ToString();
                        reportRow["THDVY"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDVY"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDVY"].ToString();
                        reportRow["THDVB"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDVB"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDVB"].ToString();
                        reportRow["THDIR"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDIR"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDIR"].ToString();
                        reportRow["THDIY"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDIY"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDIY"].ToString();
                        reportRow["THDIB"] = string.IsNullOrEmpty(tamperData.Tables[0].Rows[rowCount]["THDIB"].ToString()) ? "-----" : tamperData.Tables[0].Rows[rowCount]["THDIB"].ToString();
                        
                        reportXSD.Tables["DLMSTamperTable"].Rows.Add(reportRow);
                    }
                }


                DataRow newReportRow;
                TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                DataSet dataSet = new DataSet();
                string[] tamperHeadings = new string[15];
                if (tamperData != null && tamperData.Tables.Count > 0 && tamperData.Tables[0].Rows.Count > 0)
                {
                    dataSet = tamperParameterBLL.GetColumnNames(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        tamperHeadings = ("EventCode,TamperDescription,DateTimeEvent," + dataSet.Tables[0].Rows[0][0].ToString()).Split(',');
                    }

                    //SarkarA code change start 20180110 // Restore 1P Tamper Report // commented below code by PS
                    //string[] strTemp = new string[15];

                    //for (int i = 0; i < 15; i++)
                    //{
                    //    strTemp[i] = tamperHeadings[i];
                    //}
                    //tamperHeadings = null;
                    //tamperHeadings = new string[15];
                    //tamperHeadings = strTemp;
                    //tamperHeadings = strTemp;
                    //SarkarA code change end 20170110

                    DataView dtView = new DataView(tamperData.Tables[0]);
                    DataTable dtTableWithDesiredColumn = dtView.ToTable(false, tamperHeadings);
                    foreach (DataRow row in dtTableWithDesiredColumn.Rows)
                    {
                        int columnIndex = 0;
                        newReportRow = reportXSD.Tables["TamperTableDynamic"].NewRow();

                        foreach (DataColumn desiredColumns in dtTableWithDesiredColumn.Columns)
                        {

                            if (desiredColumns.ColumnName.ToLower().Contains("eventcode") || desiredColumns.ColumnName.ToLower().Contains("tamperdescription")
                                || desiredColumns.ColumnName.ToLower().Contains("datetimeevent"))
                            {
                                continue;
                            }
                            else
                            {
                                newReportRow["Parameter" + columnIndex.ToString()] = CommonBLL.GetFormattedData(row[desiredColumns].ToString());
                                columnIndex++;
                            }
                        }
                        newReportRow["EventCode"] = row["eventcode"].ToString();
                        newReportRow["TamperDescription"] = row["tamperdescription"].ToString();//row["EventCode"].ToString().ToString();
                        newReportRow["DateTimeEvent"] = row["DateTimeEvent"].ToString();//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString())));                       
                        reportXSD.Tables["TamperTableDynamic"].Rows.Add(newReportRow);
                }
            }
        }
        /// <summary>
        /// independently fill power failure tamper xsd
        /// Off the two dataset either of them may have larger rows, this functions fills report xsd in a tabuular format and show '----'
        /// when data is not available
        /// </summary>
        /// <param name="occDset"></param>
        /// <param name="resDset"></param>
        private void FillDLMSPowerFailureTamperXSD(DataSet occDset, DataSet resDset)
        {
            DataRow reportRow;
            int largerCount = 0;
            bool hasOccurenceData = false;
            bool hasRestoreData = false;
            const int dateTimeOrdinal = 2;
            string strPowerFailureDuration;
            // if has occurence tampers than set a boolean
            if ((occDset != null && occDset.Tables.Count > 0 && occDset.Tables[0].Rows.Count > 0))
            {
                largerCount = occDset.Tables[0].Rows.Count;
                hasOccurenceData = true;
            }
            else
            {
                hasOccurenceData = false;
            }
            // if has restore tampers than set a boolean
            if (resDset != null && resDset.Tables.Count > 0 && resDset.Tables[0].Rows.Count > 0)
            {
                hasRestoreData = true;
                if (largerCount < resDset.Tables[0].Rows.Count)
                {
                    largerCount = resDset.Tables[0].Rows.Count;
                }
            }
            else
            {
                hasRestoreData = false;
            }
            // largercount has count of larger dataset of occurence and retore
            // so that it can count the number of rows to be created in report xsd.
            for (int counter = 0; counter < largerCount; counter++)
            {
                strPowerFailureDuration = dateUnavailable;
                reportRow = reportXSD.Tables[PowerFailureTable].NewRow();
                //if occurence dataset has rows, show data otherwise show '-----';
                if (hasOccurenceData && occDset.Tables[0].Rows.Count - 1 >= counter)
                {
                    reportRow[OCCURENCETIME] = occDset.Tables[0].Rows[counter][dateTimeOrdinal].ToString();
                }
                else
                {
                    reportRow[OCCURENCETIME] = dateUnavailable;
                }
                if (hasRestoreData && resDset.Tables[0].Rows.Count - 1 >= counter)
                {
                    reportRow[RESTORATIONTIME] = resDset.Tables[0].Rows[counter][dateTimeOrdinal].ToString();
                }
                else
                {
                    reportRow[RESTORATIONTIME] = dateUnavailable;
                }
                /*VBM Show power failure duration in tamper report with base code */

                string strOccTime = reportRow[OCCURENCETIME].ToString();
                string strResTime = reportRow[RESTORATIONTIME].ToString();
                if (strOccTime != dateUnavailable && strResTime != dateUnavailable)
                {
                    TimeSpan timeSpan = Convert.ToDateTime(strResTime, new CultureInfo("hi-IN")) - Convert.ToDateTime(strOccTime, new CultureInfo("hi-IN"));
                    strPowerFailureDuration = dlms650CommonBLL.ConvertTimSpanToDDHHMM(timeSpan);
                }
                reportRow[Duration] = strPowerFailureDuration;

                /*VBM Show power failure duration in tamper repoer with base code*/
                reportXSD.Tables[PowerFailureTable].Rows.Add(reportRow);
            }

        }

        private void FillAllTamperXSD(DataSet tamperData, string tamperDescription, string count, string occurrenceTime, string restorationTime)
        {
            DataRow reportRow;

            if (tamperData.Tables[0].Rows.Count > 0)
            {
                if (tamperData.Tables[0].Rows[0]["TamperCode"].ToString() != "225")
                {
                    reportRow = reportXSD.Tables["TamperTable"].NewRow();
                    reportRow["Tamper Description"] = tamperDescription;//row["TamperType"].ToString();
                    reportRow["Tamper Counter"] = count;//row["TamperCounter"].ToString().ToString();
                    if (occurrenceTime.Contains("1900")) occurrenceTime = dateUnavailable;
                    else
                        reportRow["Occured DateTime"] = occurrenceTime;//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    if (restorationTime.Contains("1900")) restorationTime = dateUnavailable;
                    else
                        reportRow["Restored DateTime"] = restorationTime;//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperRestoredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    reportRow["RVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[0][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["RVoltageRestored"].ToString()), " V");
                    reportRow["YVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[1][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["YVoltageRestored"].ToString()), " V");
                    reportRow["BVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[2][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["BVoltageRestored"].ToString()), " V");
                    reportRow["RCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[3][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["RCurrentRestored"].ToString()), " A");
                    reportRow["YCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[4][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["YCurrentRestored"].ToString()), " A");
                    reportRow["BCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[5][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["BCurrentRestored"].ToString()), " A");
                    reportRow["RPF_restored"] = tamperData.Tables[0].Rows[6][2].ToString();//CommonBLL.GetFormattedData(row["RPFRestored"].ToString());
                    reportRow["YPF_restored"] = tamperData.Tables[0].Rows[7][2].ToString();//CommonBLL.GetFormattedData(row["YPFRestored"].ToString());
                    reportRow["BPF_restored"] = tamperData.Tables[0].Rows[8][2].ToString();//CommonBLL.GetFormattedData(row["BPFRestored"].ToString());
                    reportRow["TotalPF_restored"] = tamperData.Tables[0].Rows[9][2].ToString();//CommonBLL.GetFormattedData(row["TotalPFRestored"].ToString());
                    reportRow["kWh_restored"] = string.Concat(tamperData.Tables[0].Rows[10][2].ToString(), "kWh");//string.Concat(CommonBLL.GetFormattedData(row["kWhRestored"].ToString()), " kWh");
                    reportRow["kVAh_restored"] = string.Concat(tamperData.Tables[0].Rows[11][2].ToString(), "kVAh");//string.Concat(CommonBLL.GetFormattedData(row["kVAhRestored"].ToString()), " kVAh");
                    reportRow["RVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[0][1].ToString(), " V"); //string.Concat(CommonBLL.GetFormattedData(row["RVoltageOccurred"].ToString()), " V");
                    reportRow["YVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[1][1].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["YVoltageOccurred"].ToString()), " V");
                    reportRow["BVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[2][1].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["BVoltageOccurred"].ToString()), " V");
                    reportRow["RCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[3][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["RCurrentOccurred"].ToString()), " A");
                    reportRow["YCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[4][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["YCurrentOccurred"].ToString()), " A");
                    reportRow["BCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[5][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["BCurrentOccurred"].ToString()), " A");
                    reportRow["RPF_occurred"] = tamperData.Tables[0].Rows[6][1].ToString();//CommonBLL.GetFormattedData(row["RPFOccurred"].ToString());
                    reportRow["YPF_occurred"] = tamperData.Tables[0].Rows[7][1].ToString(); //CommonBLL.GetFormattedData(row["YPFOccurred"].ToString());
                    reportRow["BPF_occurred"] = tamperData.Tables[0].Rows[8][1].ToString(); //CommonBLL.GetFormattedData(row["BPFOccurred"].ToString());
                    reportRow["TotalPF_occurred"] = tamperData.Tables[0].Rows[9][1].ToString(); //CommonBLL.GetFormattedData(row["TotalPFOccurred"].ToString());
                    reportRow["kWh_occurred"] = string.Concat(tamperData.Tables[0].Rows[10][1].ToString(), "kWh"); //string.Concat(CommonBLL.GetFormattedData(row["kWhOccurred"].ToString()), " kWh");
                    reportRow["kVAh_occurred"] = string.Concat(tamperData.Tables[0].Rows[11][1].ToString(), "kVAh"); //string.Concat(CommonBLL.GetFormattedData(row["kVAhOccurred"].ToString()), " kVAh");

                    reportXSD.Tables["TamperTable"].Rows.Add(reportRow);
                }
                else
                {
                    reportRow = reportXSD.Tables["PowerOnOffTamperTable"].NewRow();
                    reportRow["counter"] = count;

                    reportRow["OccurredDateTime"] = Convert.ToString(occurrenceTime);
                    reportRow["RestoredDateTime"] = Convert.ToString(restorationTime);

                    string val1 = DateUtility.StringToLongDateTimeFormat(restorationTime);
                    string val2 = DateUtility.StringToLongDateTimeFormat(occurrenceTime);

                    restorationTime = restorationTime.Replace(" ", "");
                    restorationTime = restorationTime.Replace(":", "");
                    restorationTime = restorationTime + "00";

                    occurrenceTime = occurrenceTime.Replace(" ", "");
                    occurrenceTime = occurrenceTime.Replace(":", "");
                    occurrenceTime = occurrenceTime + "00";

                    val1 = val1.Substring(0, 8) + restorationTime.Substring(10, 6);
                    val2 = val2.Substring(0, 8) + occurrenceTime.Substring(10, 6);
                    TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(val1)) - DateUtility.LongToDateTime(Int64.Parse(val2));

                    reportRow["Duration"] = ts.Days.ToString("000") + " " + ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00");
                    reportXSD.Tables["PowerOnOffTamperTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillDTMLoadSurveyXSD(DataSet dtmLoadSurveyDS)
        {
            DataRow reportRow;
            DateTime DTMPreviousDate = DateTime.Now;
            int index = 0;
            int i = 0;
            foreach (DataRow row in dtmLoadSurveyDS.Tables[0].Rows)
            {
                System.Diagnostics.Debug.Print(i++.ToString());
                reportRow = reportXSD.Tables["DTMLoadSurveyTable"].NewRow();
                if (index == 0)
                {
                    DTMPreviousDate = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString())));
                    reportRow["GroupDate"] = DTMPreviousDate.ToString("dd/MM/yyyy");
                    index++;
                }
                else
                {
                    if (row["DTMDateTime"].ToString() != "0")
                    {
                        TimeSpan ts = Convert.ToDateTime(DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy")) - Convert.ToDateTime(DTMPreviousDate.ToString("dd/MM/yyyy"));
                        if (ts.Days > 0)
                        {
                            reportRow["GroupDate"] = Convert.ToDateTime(DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy")).AddDays(-1).ToString("dd/MM/yyyy");
                            DTMPreviousDate = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString())));
                        }
                        else
                        {
                            reportRow["GroupDate"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (row["DTMDateTime"].ToString() != "0")
                {
                    reportRow["Parameter1"] = row["KWh"];
                    reportRow["Parameter2"] = row["KVAh"];
                    reportRow["Parameter3"] = row["RPhaseKW"];
                    reportRow["Parameter4"] = row["YPhaseKW"];
                    reportRow["Parameter5"] = row["BPhaseKW"];
                    reportRow["Parameter6"] = string.Concat(row["RPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["RPhaseKVAr"]);
                    reportRow["Parameter7"] = string.Concat(row["YPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["YPhaseKVAr"]);
                    reportRow["Parameter8"] = string.Concat(row["BPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["BPhaseKVAr"]);
                    reportRow["Parameter9"] = row["RPhaseVoltage"];
                    reportRow["Parameter10"] = row["YPhaseVoltage"];
                    reportRow["Parameter11"] = row["BPhaseVoltage"];
                    reportRow["Parameter12"] = row["PowerDownTime"];
                    reportRow["TimeColumn"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("HH:mm");
                    reportXSD.Tables["DTMLoadSurveyTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTransactionXSD(DataSet transactionDS)
        {
            //DataRow reportRow;
            if (transactionDS == null)
                return;

            if (transactionDS.Tables[0].Rows.Count > 0)
            {
                //types = ConfigInfo.GetApplicationType();
                //if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //{
                //    foreach (DataRow row in transactionDS.Tables[0].Rows)
                //    {
                //        reportRow = reportXSD.Tables["DLMSProgrammingUpdatesTable"].NewRow();
                //        for (int colCount = 0; colCount < transactionDS.Tables[0].Columns.Count; colCount++)
                //        {
                //            reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                //        }
                //        reportXSD.Tables["DLMSProgrammingUpdatesTable"].Rows.Add(reportRow);
                //    }
                //}
                //else
                //{
                //    foreach (DataRow row in transactionDS.Tables[0].Rows)
                //    {
                //        reportRow = reportXSD.Tables["ProgrammingUpdatesTable"].NewRow();

                //        reportRow["LastTimeStamp"] = row["UpdateSequence"].ToString();
                //        reportRow["TimeStamp"] = row["LastTimeStamp"].ToString();
                //        reportRow["Parameter1"] = row["Description1"].ToString();
                //        reportRow["Parameter2"] = row["Description2"].ToString();
                //        reportRow["Parameter3"] = row["Description3"].ToString();
                //        reportRow["Parameter4"] = row["Description4"].ToString();
                //        reportRow["Parameter5"] = row["Description5"].ToString();
                //        reportRow["Parameter6"] = row["Description6"].ToString();
                //        reportRow["Parameter7"] = row["Description7"].ToString();
                //        reportRow["Parameter8"] = row["Description8"].ToString();
                //        reportRow["Parameter9"] = row["Description9"].ToString();
                //        reportRow["Parameter10"] = row["Description10"].ToString();
                //        reportRow["Parameter11"] = row["Description11"].ToString();
                //        reportRow["Parameter12"] = row["Description12"].ToString();
                //        reportRow["Parameter13"] = row["Description13"].ToString();
                //        reportRow["Parameter14"] = row["Description14"].ToString();
                //        reportRow["Parameter15"] = row["Description15"].ToString();
                //        reportRow["Parameter16"] = row["Description16"].ToString();
                //        reportRow["Parameter17"] = row["Description17"].ToString();

                //        reportXSD.Tables["ProgrammingUpdatesTable"].Rows.Add(reportRow);
                //    }
                //}

                DataRow newReportRow;
                TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                DataSet dataSet = new DataSet();
                string[] tamperHeadings = new string[15];
                if (transactionDS != null && transactionDS.Tables.Count > 0 && transactionDS.Tables[0].Rows.Count > 0)
                {
                    dataSet = tamperParameterBLL.GetColumnNames(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        tamperHeadings = ("TamperType,EventCode,DateTimeEvent," + dataSet.Tables[0].Rows[0][0].ToString()).Split(',');
                    }

                    DataView dtView = new DataView(transactionDS.Tables[0]);
                    DataTable dtTableWithDesiredColumn = dtView.ToTable(false, tamperHeadings);
                    foreach (DataRow row in dtTableWithDesiredColumn.Rows)
                    {
                        int columnIndex = 0;
                        newReportRow = reportXSD.Tables["TransactionDynamic"].NewRow();

                        foreach (DataColumn desiredColumns in dtTableWithDesiredColumn.Columns)
                        {
                            if (desiredColumns.ColumnName.ToLower().Contains("tampertype") || desiredColumns.ColumnName.ToLower().Contains("eventcode")
                                || desiredColumns.ColumnName.ToLower().Contains("datetimeevent"))
                            {
                                continue;
                            }
                            else
                            {
                                newReportRow["Parameter" + columnIndex.ToString()] = CommonBLL.GetFormattedData(row[desiredColumns].ToString());
                                columnIndex++;

                            }
                            newReportRow["TamperType"] = CommonBLL.GetFormattedData(row["TamperType"].ToString());
                            newReportRow["EventCode"] = CommonBLL.GetFormattedData(row["EventCode"].ToString());//row["EventCode"].ToString().ToString();
                            newReportRow["DateTimeEvent"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row["DateTimeEvent"]));
                        }
                        reportXSD.Tables["TransactionDynamic"].Rows.Add(newReportRow);
                    }
                }
            }
        }

        private void FillRTCUpdatesXSD(DataSet rtcUpdatesDS, int totalRTCUpdates)
        {
            DataRow reportRow;
            if (rtcUpdatesDS == null)
                return;

            if (rtcUpdatesDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in rtcUpdatesDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["RTCUpdatesTable"].NewRow();
                    reportRow["RTCUpdates"] = row["RTC Updates"].ToString();
                    if (!string.IsNullOrEmpty(row["Old Time Stamp"].ToString()))
                        reportRow["previousRTC"] = row["Old Time Stamp"].ToString();
                    else
                        reportRow["previousRTC"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Updated Time Stamp"].ToString()))
                        reportRow["currentRTC"] = row["Updated Time Stamp"].ToString();
                    else
                        reportRow["currentRTC"] = dateUnavailable;
                    reportRow["NumberOfUpdates"] = totalRTCUpdates.ToString();
                    reportXSD.Tables["RTCUpdatesTable"].Rows.Add(reportRow);
                }
            }
        }


        private void FillDailyEnergyConsumpXSD(DataSet dailyEnergyConsumpData)
        {
            DataRow reportRow;

            if (dailyEnergyConsumpData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dailyEnergyConsumpData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DailyConsumptionTable"].NewRow();

                    for (int colCount = 0; colCount < dailyEnergyConsumpData.Tables[0].Columns.Count; colCount++)
                    {
                        if (colCount == 0)
                        {
                            if (row[colCount].ToString() != "0")
                                reportRow[colCount] = row[colCount].ToString();
                            else
                                reportRow[colCount] = dateUnavailable;
                        }
                        else
                        {
                            reportRow[colCount] = row[colCount];
                        }
                    }
                    reportXSD.Tables["DailyConsumptionTable"].Rows.Add(reportRow);
                }
            }
        }
        /// <summary>
        /// used to fill the data for fraud energy.
        /// </summary>
        /// <param name="entity"></param>
        private void FillFraudEnergyXSD(FraudEnergyEntity entity)
        {
            DataRow reportRow;
            if (entity != null)
            {
                reportRow = reportXSD.Tables["FraudEnergyTable"].NewRow();
                reportRow["ActivekWh"] = entity.MagneticInfluenceKWh;
                reportRow["MagneticInfluenceLag"] = entity.MagneticInflueneceKVARhLag;
                reportRow["MagneticInfluenceLead"] = entity.MagneticInflueneceKVARhLead;
                reportRow["ApparentkVAh"] = entity.MagneticInflueneceKVAh;
                reportRow["CumulativekWh"] = entity.ReverseEnergyKWh;
                reportRow["CumulativekVAh"] = entity.ReverseEnergyKVAh;
                reportXSD.Tables["FraudEnergyTable"].Rows.Add(reportRow);
            }
        }

        private void FillSoftwareBillingXSD(SoftwareBillingEntity entity)
        {
            DataRow reportRow;
            if (entity != null)
            {
                reportRow = reportXSD.Tables["SoftwareBillingTable"].NewRow();
                reportRow["SoftwareBillStatus"] = entity.SoftwareBillingStatus;                
                reportXSD.Tables["SoftwareBillingTable"].Rows.Add(reportRow);
            }
        }

        private void FillAutoLockXSD(AutoLockEntity entity)
        {
            DataRow reportRow;
            if (entity != null)
            {
                reportRow = reportXSD.Tables["AutoLockTable"].NewRow();
                reportRow["AutoLockStatus"] = entity.AutoLockStatus;
                reportXSD.Tables["AutoLockTable"].Rows.Add(reportRow);
            }
        }

        private void FillKvahSelectionXSD(MeterConfigurationsNFEntity entity)
        {
            DataRow reportRow;            
             if (entity != null)
            {
                reportRow = reportXSD.Tables["KvahSelectionTable"].NewRow();
                if(entity.kvarselectionEntity.LagOnly=="1")
                    reportRow["KvahSelStatus"] = "Lag Only(Lock)";
                else
                    reportRow["KvahSelStatus"] = "Lag + Lead(Unlock)";

                reportXSD.Tables["KvahSelectionTable"].Rows.Add(reportRow);
            }
        }

        private void FillManualMDResetXSD(ManualMDResetEntity entity)
        {
            DataRow reportRow;
            if (entity != null)
            {
                reportRow = reportXSD.Tables["ManualMDResetTable"].NewRow();
                reportRow["ManualMDResetStatus"] = entity.ManualMDResetStatus;
                reportXSD.Tables["ManualMDResetTable"].Rows.Add(reportRow);
            }
        }
        /// <summary>
        /// fill configuration for sip , dip , rtc , billing type.
        /// </summary>
        /// <param name="rtc"></param>
        /// <param name="DIP"></param>
        /// <param name="SIP"></param>
        /// <param name="billingType"></param>
        private void FillDIPSmartmeterXSD(string rtc, int DIP, int SIP, BillingTypeEntity billingType)
        {
                DataRow reportRow;
                DataRow reportRowRTC;
                reportRow = reportXSD.Tables["DIPforSmartmeter"].NewRow();
                reportRowRTC = reportXSD.Tables["OtherConfigurations"].NewRow();
                if (rtc != null)
                {
                    reportRowRTC["RTC"] = rtc;
                }   
            if (DIP != 0)
                {
                    if (DIP == 0x00000384)
                    {

                        reportRow["DemandInterval"] = "15 (900)";
                                            }
                    else if (DIP == 0x00000708)
                    {

                        reportRow["DemandInterval"] = "30 (1800)";
                        
                    }
                    else if (DIP == 0x00001384)
                    {
                        
                        reportRow["DemandInterval"] = "15 (900)";
                   
                    }
                    else if (DIP == 0x00001708)
                    {

                        reportRow["DemandInterval"] = "30 (1800)";
                       
                    }
                    else if (DIP == 0x00002708)
                    {

                        reportRow["DemandInterval"] = "30 (1800)";
                      
                    }

                }
            if (SIP != 0)
            {
                reportRowRTC["SIP"] = (SIP / 60) + " (" + SIP + ")";
            }
            if (billingType != null)
            {
                if (billingType.ModeOfBilling.ToString() == "EndofMonth")
                {
                   
                    reportRowRTC["BillingDay"] = "01";
                    reportRowRTC["BillingHours"] = "00";
                    reportRowRTC["BillingMinutes"] = "00";

                }
                else if (billingType.ModeOfBilling.ToString() == "UserDefined")
                {
                   
                    reportRowRTC["BillingDay"] = billingType.Day;
                    reportRowRTC["BillingHours"] = billingType.Hours;
                    reportRowRTC["BillingMinutes"] = billingType.Minutes;
                }
                else
                {
                    reportRowRTC["BillingType"] = "";
                }
            }
            reportXSD.Tables["OtherConfigurations"].Rows.Add(reportRowRTC);
                reportXSD.Tables["DIPforSmartmeter"].Rows.Add(reportRow);
           

        }

        private void FillOtherConfigurationsXSD(string rtc, int DIP, int SIP, BillingTypeEntity billingType)
        {
            DataRow reportRow;
            if (rtc != null || billingType != null)
            {
                reportRow = reportXSD.Tables["OtherConfigurations"].NewRow();
                if (rtc != null)
                {
                    reportRow["RTC"] = rtc;
                }
                if (DIP != 0)
                {
                    if (DIP == 0x00000384)
                    {
                        reportRow["DemandType"] = "Block Demand";
                        reportRow["DemandInterval"] = "15 (900)";
                        reportRow["DemandSubInterval"] = "----";
                    }
                    else if (DIP == 0x00000708)
                    {
                        reportRow["DemandType"] = "Block Demand";
                        reportRow["DemandInterval"] = "30 (1800)";
                        reportRow["DemandSubInterval"] = "----";
                    }
                    else if (DIP == 0x00001384)
                    {
                        reportRow["DemandType"] = "Sliding Demand";
                        reportRow["DemandInterval"] = "15 (900)";
                        reportRow["DemandSubInterval"] = "05 (300)";

                    }
                    else if (DIP == 0x00001708)
                    {
                        reportRow["DemandType"] = "Sliding Demand";
                        reportRow["DemandInterval"] = "30 (1800)";
                        reportRow["DemandSubInterval"] = "05 (300)";
                    }
                    else if (DIP == 0x00002708)
                    {
                        reportRow["DemandType"] = "Sliding Demand";
                        reportRow["DemandInterval"] = "30 (1800)";
                        reportRow["DemandSubInterval"] = "10 (600)";
                    }
                    

                }
                if (SIP != 0)
                {
                    reportRow["SIP"] = (SIP / 60) + " (" + SIP + ")";
                }
                if (billingType != null)
                {
                    if (billingType.ModeOfBilling.ToString() == "EndofMonth")
                    {
                        reportRow["BillingType"] = "End of Month";
                        reportRow["BillingDay"] = "01";
                        reportRow["BillingHours"] = "00";
                        reportRow["BillingMinutes"] = "00";

                    }
                    else if (billingType.ModeOfBilling.ToString() == "UserDefined")
                    {
                        reportRow["BillingType"] = "User Defined";
                        reportRow["BillingDay"] = billingType.Day;
                        reportRow["BillingHours"] = billingType.Hours;
                        reportRow["BillingMinutes"] = billingType.Minutes;
                    }
                    else
                    {
                        reportRow["BillingType"] = "";
                    }
                }

                reportXSD.Tables["OtherConfigurations"].Rows.Add(reportRow);
            }
        }


        private void FillDisconnectControlXSD(string DisconnectData)
        {

            if (!string.IsNullOrEmpty(DisconnectData))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["DisconnectControl"].NewRow();

                if (DisconnectData == "1")
                    reportRow["Meter Status"] = "Connected";
                else
                    reportRow["Meter Status"] = "Disconnected";
                reportXSD.Tables["DisconnectControl"].Rows.Add(reportRow);
            }

        }
        // ************* Load control for smart meter 3 phase**********
        private void FillLoadControlXSD(string LoadData)
        {

            if (!string.IsNullOrEmpty(LoadData))
            {
                string[] lstData = LoadData.Split('\\');
               if (lstData.Length == 8)
                {  
                    DataRow reportRow;
                    reportRow = reportXSD.Tables["LoadControl"].NewRow();
                    if (lstData[0]== "1")
                        reportRow["OverCurrentThreshold"] = "Linking";
                    else
                        reportRow["OverCurrentThreshold"] = "Delinking";

                    if (lstData[1] == "1")
                        reportRow["LowPFThreshold"] = "Linking";
                    else
                        reportRow["LowPFThreshold"] = "Delinking";
                    if (lstData[2] == "1")
                        reportRow["OverLoadThreshold"] = "Linking";
                    else
                        reportRow["OverLoadThreshold"] = "Delinking";


                    reportRow["TimeInterval"] = lstData[3];
                    reportRow["MaxRetryCycle1To10"] = lstData[4];
                    reportRow["WaitTimeNextRetry"] = lstData[5];
                    reportRow["MaxRetry0To20"] = lstData[6];
                    reportRow["RelayReconnectionTime"] = lstData[7];
                    reportXSD.Tables["LoadControl"].Rows.Add(reportRow);
                }
            }

        }
        // ************* Load control for smart meter 1 phase**********
        private void FillLoadControl1PhaseXSD(string LoadData1P)
        {

            if (!string.IsNullOrEmpty(LoadData1P))
            {
                string[] lstData = LoadData1P.Split('\\');
                if (lstData.Length == 6)
                {
                    DataRow reportRow;
                    reportRow = reportXSD.Tables["LoadControl1Psmartmeter"].NewRow();
                    reportRow["OverLoadThreshold"] = (Convert.ToDecimal(lstData[0]) / 1000).ToString("0.000");
                    reportRow["OverCurrentThreshold"] = (Convert.ToDecimal(lstData[1]) / 100).ToString("0.00");
                    reportRow["TimeInterval120"] = lstData[2];
                    reportRow["MaxRetryCycle15"] = lstData[5];
                    reportRow["WaitTimeRetryCycle"] = lstData[3];
                    reportRow["MaxRetryCycle110"] = lstData[4];
                    reportXSD.Tables["LoadControl1Psmartmeter"].Rows.Add(reportRow);
                }
            }
        }

        // ************* RS485 HTCT meters**********
        private void RS485XSD(string Rs485)
        {
            if (!string.IsNullOrEmpty(Rs485))
            {

                DataRow rptRow = reportXSD.Tables["RS485"].NewRow();
                if (Rs485 != null)
                {
                    rptRow["RS485"] = Rs485;
                }
                reportXSD.Tables["RS485"].Rows.Add(rptRow);
            }
        }
        private void FillPaymentmodeXSD(string paymentData)
        {

            if (!string.IsNullOrEmpty(paymentData))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["PaymentMode"].NewRow();

                if (paymentData == "1")
                    reportRow["Paymentstatus"] = "Prepaid Mode";
                else
                    reportRow["Paymentstatus"] = "Postpaid Mode";
                reportXSD.Tables["PaymentMode"].Rows.Add(reportRow);
            }

        }

        private void FillMeteringModeXSD(string MeteringData)
        {

            if (!string.IsNullOrEmpty(MeteringData))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["MeteringMode"].NewRow();

                if (MeteringData == "0")
                    reportRow["Meteringstatus"] = "Forwarded";
                else
                    reportRow["Meteringstatus"] = "Import-Export";
                reportXSD.Tables["MeteringMode"].Rows.Add(reportRow);
            }

        }

        private void FillLoadLimitXSD(string Loadlimidata)
        {

            if (!string.IsNullOrEmpty(Loadlimidata))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["LoadlimitMode"].NewRow();

                if (Loadlimidata != null)
                {
                    reportRow["Loadlimitstatus"] = Loadlimidata;
                }
                reportXSD.Tables["LoadlimitMode"].Rows.Add(reportRow);
            }

        }

        private void FillPulseEnergyXSD(string pulseEnergyData)
        {

            if (!string.IsNullOrEmpty(pulseEnergyData))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["PulseEnergyType"].NewRow();

                if (pulseEnergyData != null && int.TryParse(pulseEnergyData, out int value))
                {
                    reportRow["EnergyType"] = CABEntity.EnumUtil.StringValue((PulseEnergyValues)value); ;
                }
                reportXSD.Tables["PulseEnergyType"].Rows.Add(reportRow);
            }

        }

        private void FillSlidingdemandXSD(string Slidingdemanddata)
        {

            if (!string.IsNullOrEmpty(Slidingdemanddata))
            {
                int countlen = 0;
                int EncCount = 0;
                byte[] ReceiveData = new byte[6];
                DataRow reportRow;
                reportRow = reportXSD.Tables["SlidingdemandMode"].NewRow();
                //if (Slidingdemanddata == "1")
                //    reportRow["Slidingstatus"] = "Block";
                //else
                //    reportRow["Slidingstatus"] = "Sliding";
                while (countlen < Slidingdemanddata.Length - 1)
                {
                    ReceiveData[EncCount++] = Convert.ToByte(Slidingdemanddata.Substring(countlen, 2), 16);
                    countlen += 2;

                }
               int compValueS = (int)ReceiveData[1] >> 4;
                if (ReceiveData[0] == 0x02)
                {
                    if (ReceiveData[3] <= 1 && ReceiveData[3] == 0x00)
                       reportRow["Slidingstatus"] = "Block";
                    else
                       reportRow["Slidingstatus"] = "Sliding";
                    
                    if (ReceiveData[5] == 0)
                        reportRow["SubdemandIP"] = "0";
                       
                    else if (ReceiveData[5] == 5)
                        reportRow["SubdemandIP"] = "5";
                      
                    else { /*Default Display*/ }
                }


                reportXSD.Tables["SlidingdemandMode"].Rows.Add(reportRow);
            }

        }

        private void FillOpticalPortXSD(string OpticalPortdata)
        {

            if (!string.IsNullOrEmpty(OpticalPortdata))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["OpticalRJportMode"].NewRow();
                if (OpticalPortdata == "1")
                    reportRow["Opticalstatus"] = "Lock";
                else
                    reportRow["Opticalstatus"] = "Un-Lock";
                reportXSD.Tables["OpticalRJportMode"].Rows.Add(reportRow);
            }

        }

        private void FillRJPortXSD(string RJPortdata)
        {

            if (!string.IsNullOrEmpty(RJPortdata))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["OpticalRJportMode"].NewRow();
                if (RJPortdata == "1")
                    reportRow["RJstatus"] = "Lock";
                else
                    reportRow["RJstatus"] = "Un-Lock";
                reportXSD.Tables["OpticalRJportMode"].Rows.Add(reportRow);
            }

        }


        /// <summary>
        /// Fill TOD Future Activation Date
        /// </summary>
        /// <param name="?"></param>
        private void FillTODFutureActivationDate(string TODActivationDate)
        {
            if (!string.IsNullOrEmpty(TODActivationDate))
            {
                DataRow reportRow;
                reportRow = reportXSD.Tables["TODFutureActivationDate"].NewRow();
                reportRow["TODFutureActivationDate"] = TODActivationDate;
                reportXSD.Tables["TODFutureActivationDate"].Rows.Add(reportRow);
            }
        }
        #endregion

        private void btnShow_Click(object sender, EventArgs e)
        {
            int errCount = 0;
            int showReport = 0;
            int selectedParams = 0;
            int historyIDCount = 0;
            //int meterModelNumber = 0;
            reportXSD = new FileReportDataSet();
            string errMsg = String.Empty;
            string fileName = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
                {
                    this.StatusMessage = "Please select a file.";
                    return;
                }

                if (ValidateForm() == false)
                {
                    this.StatusMessage = "Please select a parameter to view the  report.";
                    return;
                }
                // Added to get the filename.
                DataSet fileDataset = new DataSet();
                fileDataset = ListFileName(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (fileDataset != null)
                {
                    if (fileDataset.Tables[0].Rows.Count > 0)
                    {
                        fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();//user story no 505185
                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetails(detailsDS);
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                        FillMeterID(meterIDDS);
                }

                /* GKG JVVNL Current TOU Read */
                if (chkTouConfiguration.Checked == true && chkTouConfiguration.Visible == true)
                {
                    selectedParams++;
                    DataSet touConfiguration = new DataSet();
                    touConfiguration = new TOUBLL().DetailData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false);
                    if (touConfiguration != null && touConfiguration.Tables[0].Rows.Count > 0)
                    {
                        Int32.TryParse(touConfiguration.Tables[0].Rows[0]["Season Number"].ToString(), out seasonNumber);
                        FillTouConfigurationXSD(touConfiguration);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        errMsg = "TOU configuration data not available.";
                    }
                }
                /* GKG JVVNL Current TOU Read */

                if (chkGeneralReport.Checked == true)
                {
                    selectedParams++;
                    DataSet generalDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        generalDS = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        generalDS = ListGeneralData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }

                    DataRow NameplateRow = null;
                    DataSet dataSetNameplate = new DLMS650NamePlateBLL().GetMeterData(Convert.ToInt32(MeterDataID));
                    if (dataSetNameplate != null && dataSetNameplate.Tables != null && dataSetNameplate.Tables.Count > 0)
                    {
                        for (int count = 0; count < dataSetNameplate.Tables[0].Rows.Count; count++)
                        {
                            NameplateRow = dataSetNameplate.Tables[0].Rows[count];
                            if (NameplateRow["OBIS Code"].ToString() == "0.0.96.128.17.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                            {
                                int idx = generalDS.Tables[0].Rows.IndexOf(generalDS.Tables[0].Select().First(s => s["OBIS Code"].ToString().Contains("0.0.96.1.4.255")));
                                generalDS.Tables[0].Rows.InsertAt(generalDS.Tables[0].NewRow(), idx + 1);
                                generalDS.Tables[0].Rows[idx + 1].ItemArray = NameplateRow.ItemArray;
                            }
                            else if(NameplateRow["OBIS Code"].ToString() == "1.0.96.128.15.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                            {
                                generalDS.Tables[0].ImportRow(NameplateRow);
                            }
                        }
						generalDS.AcceptChanges();
                    }
								
							
                    if (generalDS != null && generalDS.Tables[0].Rows.Count > 0)
                    { FillGeneralXSD(generalDS); showReport++; }
                    else
                    {
                        errCount++; errMsg = "General data not available.";
                    }
                }
                if (chkInstantReport.Checked == true)
                {
                    selectedParams++;
                    DataSet instantDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        instantDS = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        instantDS = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (instantDS != null && instantDS.Tables[0].Rows.Count > 0)
                    {
                        FillInstantXSD(instantDS); showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "") errMsg = "Instantaneous data not available.";
                        else errMsg = errMsg + "\n" + "Instantaneous data not available.";
                    }
                    if (UtilityDetails.ShowAnamolyParameters)
                    {
                        //Code block to bind anomaly xsd                    
                        AnomalyEntity objAnomalyEntity = (AnomalyEntity)new AnomalyBLL().GetDetailData((Convert.ToInt32(ConfigInfo.ActiveMeterDataId)));
                        if (objAnomalyEntity != null && objAnomalyEntity.AnomalyId > 0)
                        {
                            FillAnomalyXSD(objAnomalyEntity);
                        }
                    }
                }

                if (chkPowerFactor.Checked == true)
                {
                    selectedParams++;
                    DataSet powerFactorDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        powerFactorDS = new DLMS650BillingBLL().GetAveragePowerFactor(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        powerFactorDS = ListPowerFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (powerFactorDS != null && powerFactorDS.Tables[0].Rows.Count > 0)
                    { FillPowerFactorXSD(powerFactorDS, fileName); showReport++; }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                        { errMsg = "Power Factor not available."; }
                        else
                        { errMsg = errMsg + "\n" + "Power Factor not available."; }

                    }
                }

                if (chkBillingTamperCounter.Checked == true)
                {
                    selectedParams++;
                    DataSet billingTamperCounterDS = new DataSet();
                    billingTamperCounterDS = ListBillingTamperCounterData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (billingTamperCounterDS != null && billingTamperCounterDS.Tables[0].Rows.Count > 0)
                    {
                        FillBillingTamperCounterXSD(billingTamperCounterDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Billing Tamper Counter not available.";
                        else errMsg = errMsg + "\n" + "Billing Tamper Counter not available.";
                    }
                }
                if (chkTamper.Checked)
                {
                    selectedParams++;
                    DLMS650TamperMasterBLL dLMS650TamperMasterBLL = new DLMS650TamperMasterBLL();
                    DataSet occurrenceDset = new DataSet();
                    DataSet restorationDset = new DataSet();
                    DataSet tamperDetailsDset = new DataSet();
                    DataTable occTimeTable = new DataTable();
                    DataTable resTimeTable = new DataTable();
                    DataTable tamperDetailsDTable = new DataTable();
                    DataRow dr;
                    DataRow occTimeRow;
                    DataRow resTimeRow;
                    DataSet tamperCounterDS = new DataSet();
                    DataSet tamperDS = new DataSet();
                    string tamperCounter = string.Empty;
                    string occurranceTime = string.Empty;
                    string restorationTime = string.Empty;
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        tamperCounterDS = dLMS650TamperMasterBLL.ListAllTamperEventCode();
                        tamperDS = dLMS650TamperMasterBLL.AllDetailData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        // meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
                    }
                    occTimeTable.Columns.Add("EventCode");
                    occTimeTable.Columns.Add("TamperDescription");
                    occTimeTable.Columns.Add("DateTimeEvent");

                    resTimeTable.Columns.Add("EventCode");
                    resTimeTable.Columns.Add("TamperDescription");
                    resTimeTable.Columns.Add("DateTimeEvent");

                    try
                    {

                        if (tamperDS != null && tamperDS.Tables[0].Rows.Count > 0)
                        {
                            showReport++;
                            for (int cIndex = 0; cIndex <= tamperDS.Tables[0].Columns.Count; cIndex++)
                            {
                                if (cIndex == 0)
                                    tamperDetailsDTable.Columns.Add(tamperDS.Tables[0].Columns[0].Caption);
                                else if (cIndex == 1)
                                    tamperDetailsDTable.Columns.Add("TamperDescription");
                                else
                                    tamperDetailsDTable.Columns.Add(tamperDS.Tables[0].Columns[cIndex - 1].Caption);
                            }

                            foreach (DataRow drow in tamperDS.Tables[0].Rows)
                            {
                                DataTable tempTable = new DataTable();
                                for (int cIndex = 0; cIndex <= tamperDS.Tables[0].Columns.Count; cIndex++)
                                {
                                    if (cIndex == 0)
                                        tempTable.Columns.Add(tamperDS.Tables[0].Columns[0].Caption);
                                    else if (cIndex == 1)
                                        tempTable.Columns.Add("TamperDescription");
                                    else
                                        tempTable.Columns.Add(tamperDS.Tables[0].Columns[cIndex - 1].Caption);
                                }
                                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                                {
                                    foreach (DataRow row in tamperCounterDS.Tables[0].Rows)
                                    {
                                        if (drow["EventCode"].ToString() == row["TamperTypeID"].ToString())
                                        {
                                            if ((drow["EventCode"].ToString() != "101") && (drow["EventCode"].ToString() != "102"))
                                            {
                                                dr = tempTable.NewRow();
                                                dr[0] = drow["EventCode"].ToString();
                                                dr[1] = row[1].ToString();
                                                dr[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                dr[3] = SplitWithOutUnit(drow["CurrentIR"].ToString());
                                                dr[4] = SplitWithOutUnit(drow["CurrentIY"].ToString());
                                                dr[5] = SplitWithOutUnit(drow["CurrentIB"].ToString());
                                                dr[6] = SplitWithOutUnit(drow["VoltageVRN"].ToString());
                                                dr[7] = SplitWithOutUnit(drow["VoltageVYN"].ToString());
                                                dr[8] = SplitWithOutUnit(drow["VoltageVBN"].ToString());
                                                dr[9] = SplitWithOutUnit(drow["PowerFactorRphase"].ToString());
                                                dr[10] = SplitWithOutUnit(drow["PowerFactorYphase"].ToString());
                                                dr[11] = SplitWithOutUnit(drow["PowerFactorBphase"].ToString());
                                                dr[12] = SplitWithOutUnit(drow["TotalPowerFactor"].ToString());
                                                dr[13] = SplitWithOutUnit(drow["CumulativeEnergykWh"].ToString());
                                                dr[14] = SplitWithOutUnit(drow["CumulativeEnergykVAh"].ToString());
                                                tempTable.Rows.Add(dr);
                                                break;
                                            }
                                            else if (drow["EventCode"].ToString() == "101")
                                            {
                                                occTimeRow = occTimeTable.NewRow();
                                                occTimeRow[0] = drow["EventCode"].ToString();
                                                occTimeRow[1] = row[1].ToString();
                                                occTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                occTimeTable.Rows.Add(occTimeRow);
                                                break;
                                            }
                                            else if (drow["EventCode"].ToString() == "102")
                                            {
                                                resTimeRow = resTimeTable.NewRow();
                                                resTimeRow[0] = drow["EventCode"].ToString();
                                                resTimeRow[1] = row[1].ToString();
                                                resTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                resTimeTable.Rows.Add(resTimeRow);
                                                break;
                                            }
                                        }
                                    }
                                    foreach (DataRow dtRow in tempTable.Rows)
                                    {
                                        //for HTCT
                                        //if (dtRow[1].ToString() == "Meter Cover Opening - Occurrence")
                                        //    continue;
                                        tamperDetailsDTable.ImportRow(dtRow);
                                    }

                                }
                            }
                            if (tamperDetailsDTable != null && tamperDetailsDTable.Rows.Count > 0)
                                tamperDetailsDset.Tables.Add(tamperDetailsDTable);

                            FillDLMSTamperXSD(tamperDetailsDset);
                            if (occTimeTable != null && occTimeTable.Rows.Count > 0)
                                occurrenceDset.Tables.Add(occTimeTable);
                            if (resTimeTable != null && resTimeTable.Rows.Count > 0)
                                restorationDset.Tables.Add(resTimeTable);


                            FillDLMSPowerFailureTamperXSD(occurrenceDset, restorationDset);

                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "Tamper data not available.";
                            else errMsg = errMsg + "\n" + "Tamper data not available.";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                    }
                }
                if (chkMainEnergy.Checked == true)
                {
                    selectedParams++;                    
                    DataSet mainEnergyDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        //if (meterModelNumber == 9)
                        //{
                        //    mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false, meterModelNumber);// Story - 365971 - 13 billing for Power ON Hours
                        //}
                        //else
                        //{
                            mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false);// Story - 365971 - 13 billing for Power ON Hours
                        //}
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                    {
                        FillMainEnergyXSD(mainEnergyDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "") errMsg = "Main Energy data not available.";
                        else errMsg = errMsg + "\n" + "Main Energy data not available.";
                    }
                }

                //if (chkEnergyConsumption.Checked == true)
                //{
                //    selectedParams++;
                //    DataSet mainEnergyDS = new DataSet();
                //    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //    {
                //        mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergyCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                //    }
                //    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                //    {
                //        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    }
                //    if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                //    { FillEnergyConsumptionXSD(mainEnergyDS); showReport++; }
                //    else
                //    {
                //        errCount++;
                //        if (errMsg == "") { errMsg = "Main Energy Consumption data not available."; } else { errMsg = errMsg + "\n" + "Main Energy Consumption data not available."; }

                //    }
                //}

                if (chkMaximumDemand.Checked == true)
                {
                    selectedParams++;
                    DataSet maximumDemandDS = new DataSet();

                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        maximumDemandDS = new DLMS650BillingBLL().GetMaximumDemand(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        maximumDemandDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (maximumDemandDS != null && maximumDemandDS.Tables[0].Rows.Count > 0)
                    { FillMaximumDemandXSD(maximumDemandDS); showReport++; }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Demand data not available.";
                        else errMsg = errMsg + "\n" + "Demand data not available.";
                    }

                    DataSet TODMDDS = new DataSet();
                    // Added to solve tod demand report issue in case of fastdownloading.
                    int counter = 0;
                    if (fileName.Contains(FILEEXTENSION))
                    {
                        counter = 1;
                    }
                    for (int historyID = counter; historyID <= 12; historyID++)
                    {
                        if (types.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            TODMDDS = new DLMS650BillingBLL().GetTODMDMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, true);
                        }
                        if (TODMDDS != null && TODMDDS.Tables[0].Rows.Count > 0)
                        { FillTODMDXSD(TODMDDS, historyID); showReport++; }
                        else
                        {
                            break;
                        }
                    }
                }

                if (chkTODEnergy.Checked == true)
                {
                    historyIDCount = 0;
                    selectedParams++;
                    DataSet tariffEnergyDS = new DataSet();

                    for (int historyID = 0; historyID <= 12; historyID++)
                    {
                        if (types.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            tariffEnergyDS = new DLMS650BillingBLL().GetMeterDataForRPT(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), Convert.ToInt32(historyID));
                        }

                        if (tariffEnergyDS != null && tariffEnergyDS.Tables[0].Rows.Count > 0)
                        {
                            FillTariffEnergyXSD(tariffEnergyDS, historyID);
                            showReport++;
                            historyIDCount++;
                        }
                    }
                    //changed on 22/02/2012
                    if (historyIDCount == 0 && tariffEnergyDS == null)
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "TOD Energy data not available.";
                        else
                            errMsg = errMsg + "\n" + "TOD Energy data not available.";
                    }
                }

                if (chkTODConsumption.Checked == true)
                {
                    historyIDCount = 0;
                    selectedParams++;
                    DataSet currentTariffEnergyDS = new DataSet();
                    DataSet nextTariffEnergyDS = new DataSet();
                    // Added to the check the extension of file. In case of fastdownloading no current data is present.
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.Contains("2NG"))
                        {
                            for (int historyID = 0; historyID <= 12; historyID++)
                            {
                                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                                {

                                    if (historyID == 0)
                                    {
                                        currentTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        historyIDCount++;
                                    }
                                    else
                                    {
                                        currentTariffEnergyDS = nextTariffEnergyDS;
                                        historyIDCount++;
                                    }
                                    nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID + 1, false);

                                    if (nextTariffEnergyDS == null)
                                    {
                                        nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        if (nextTariffEnergyDS == null) break;
                                    }
                                }
                                if (currentTariffEnergyDS != null && nextTariffEnergyDS != null && currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
                                {
                                    FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
                                    showReport++;
                                }
                            }
                        }
                        else
                        {
                            for (int historyID = 1; historyID <= 12; historyID++)
                            {
                                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                                {
                                    currentTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                    if (currentTariffEnergyDS == null)
                                        break;
                                    historyIDCount++;

                                    nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID + 1, false);

                                    if (nextTariffEnergyDS == null)
                                    {
                                        nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        if (nextTariffEnergyDS == null) break;
                                    }
                                }
                                if (currentTariffEnergyDS != null && nextTariffEnergyDS != null && currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
                                {
                                    FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
                                    showReport++;
                                }
                            }
                        }
                    }

                    if (nextTariffEnergyDS == null && currentTariffEnergyDS == null || historyIDCount == 0)
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "TOD Energy Consumption data not available.";
                        else
                            errMsg = errMsg + "\n" + "TOD Energy Consumption data not available.";
                    }
                }
                #region "Phasor Report"
                if (chkPhasor.Visible && chkPhasor.Checked)
                {
                    selectedParams++;

                    PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                    phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                    phasorDiagramForm.ShowDialog();


                    if (phasorDiagramForm.PhasorDataAvailable)
                    {

                        DataSet dsPhasor = new DLMS650PhasorBLL().GetPhasorDataSet(Convert.ToInt32(ConfigInfo.ActiveMeterDataId)) as DataSet;
                        dlms650CommonBLL.ApplyMultiplyFactor(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), dsPhasor, true);
                        FillPhasorTable();
                        FillPhasorXSD(dsPhasor);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Phasor data not available.";
                        else
                            errMsg = errMsg + "\n" + "Phasor data not available.";
                    }
                }
                #endregion

                if (chkLoadSurvey.Checked == true)
                {
                    selectedParams++;
                    string type = "Energy";
                    if (SMD_rbtnLoadSurveyDemand.Checked)
                        type = "Demand";
                    long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                    DataSet loadSurveyDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        loadSurveyDS = new DLMS650LoadSurveyBLL().ListDataSetColumnWise(id, new DLMS650LoadSurveyBLL().GetFromDate(id), new DLMS650LoadSurveyBLL().GetToDate(id), type, true);
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                    }
                    if (loadSurveyDS != null && loadSurveyDS.Tables[0].Rows.Count > 0)
                    { FillLoadSurveyXSD(loadSurveyDS); showReport++; }
                    else
                    {
                        errCount++;
                        if (errMsg == "") { errMsg = "Load Survey data not available."; }
                        else { errMsg = errMsg + "\n" + "Load Survey data not available."; }
                    }
                }

                if (chkBillingMechanism.Checked == true)
                {
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                    {
                        FillBillingMechanismXSD(ctRatioDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Billing Mechanism data not available.";
                        else
                            errMsg = errMsg + "\n" + "Billing Mechanism data not available.";
                    }
                }

                if (chkTransactions.Checked == true)
                {
                    selectedParams++;
                    DataSet transactionDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        transactionDS = new DLMS650CommonBLL().Transaction(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (transactionDS != null && transactionDS.Tables.Count > 0)
                        {
                            DataView view = new DataView(transactionDS.Tables[0]);
                            StringBuilder filterCondition = new StringBuilder();
                            if (!UtilityDetails.ShowMDResetTamper)
                            {
                                filterCondition.Append("EventCode =" + MDReset);
                            }
                            if (UtilityDetails.DisableProgrammingBillingDateTime)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + SingleActionScheduleForBillingDates);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + SingleActionScheduleForBillingDates);
                                }
                            }

                            if (UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + ProfileCapturePeriod);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + ProfileCapturePeriod);
                                }
                            }
                            if (UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + DemandIntegrationPeriod);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + DemandIntegrationPeriod);
                                }
                            }
                            if (!UtilityDetails.ShowDisplayParemeterTamper)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append("OR EventCode =" + ScrollTimeConfig + " OR EventCode =" + ScrollModeConfig
                                                      + " OR EventCode =" + PushModeConfig + " OR EventCode =" + HRModeConfig);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + ScrollTimeConfig + " OR EventCode =" + ScrollModeConfig
                                                      + " OR EventCode =" + PushModeConfig + " OR EventCode =" + HRModeConfig);
                                }

                            }
                            if (filterCondition.Length > 0)
                            {
                                view.RowFilter = filterCondition.ToString();
                                //delete rows
                                foreach (DataRowView row in view)
                                {
                                    row.Delete();
                                }
                            }
                            transactionDS.AcceptChanges();

                        }
                    }
                    if (transactionDS != null && transactionDS.Tables[0].Rows.Count > 0)
                    {
                        FillTransactionXSD(transactionDS); showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Transactions data not available.";
                        else
                            errMsg = errMsg + "\n" + "Transactions data not available.";
                    }
                }

                if (chkCTRatio.Checked == true)
                {
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                    {
                        FillCTRatioXSD(ctRatioDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "CT Ratio not available.";
                        else errMsg = errMsg + "\n" + "CT Ratio not available.";
                    }
                }

                if (chkLoadFactor.Checked == true)
                {
                    DataSet loadFactorDS = new DataSet();
                    loadFactorDS = ListLoadFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (loadFactorDS != null && loadFactorDS.Tables[0].Rows.Count > 0)
                    {
                        FillLoadFactorXSD(loadFactorDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Load Factor not available.";
                        else errMsg = errMsg + "\n" + "Load Factor not available.";
                    }
                }

                if (chkPowerOnHours.Checked == true)
                {
                    DataSet powerOnHoursDS = new DataSet();
                    powerOnHoursDS = ListPowerOnHoursData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (powerOnHoursDS != null && powerOnHoursDS.Tables[0].Rows.Count > 0)
                    {
                        FillPowerOnHoursXSD(powerOnHoursDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Power On Hours not available.";
                        else errMsg = errMsg + "\n" + "Power On Hours not available.";
                    }
                }

                if (chkPowerOffDuration.Checked && chkPowerOffDuration.Visible)
                {
                    DataSet powerOffDuration = new DataSet();
                    powerOffDuration = new DLMS650BillingBLL().GetPowerOffDuration(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (powerOffDuration != null && powerOffDuration.Tables[0].Rows.Count > 0)
                    {
                        FillPowerOffDurationXSD(powerOffDuration);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Power Off duration data not available.";
                        else errMsg = errMsg + "\n" + "Power Off duration data not available.";
                    }
                }
                //if (isMPKWCL)
                //{
                    if (chkMiscelleneous.Checked)
                    {
                        DataSet miscelleneousData = new DataSet();

                        miscelleneousData = new DLMS650BillingBLL().GetMiscellaneous(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (miscelleneousData.FirstTableHasRows())
                        {
                            FillMiscelleneous(miscelleneousData);
                            showReport++;
                        }
                        else
                        {
                            errCount++;
                            errMsg = errMsg + Symbols.NEWLINE + MISCDATANOTAVAILABLE;

                        }

                    }
                //}


                //selectedParams++;
                ////    this.Visible = false;
                //PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                //phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                //phasorDiagramForm.ShowDialog();
                ////    this.Visible = true;

                //if (phasorDiagramForm.PhasorDataAvailable)
                //{
                //    FillPhasorTable();
                //    showReport++;
                //}
                //else
                //{
                //    errCount++;
                //    if (errMsg == "")
                //        errMsg = "Phasor data is not available.";
                //    else errMsg = errMsg + "\n" + "Phasor data is not available.";

                //}


                //        if (chkDailyEnergyConsumption.Checked == true)
                //        {
                //            //DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                //            //long lsFromDateMD = loadSurveyBLL.GetFromDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //            //long lsToDateMD = loadSurveyBLL.GetToDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                //            DLMS650LoadSurveyBLL dlmsLoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //            selectedParams++;
                //            DataSet dailyEnergyConsumpDS = new DataSet();
                //            if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //            {
                //                dailyEnergyConsumpDS = dlmsLoadSurveyBLL.GetPUMADailyConsumption(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //            }
                //            if (dailyEnergyConsumpDS != null && dailyEnergyConsumpDS.Tables[0].Rows.Count > 0)
                //            {
                //                FillDailyEnergyConsumpXSD(dailyEnergyConsumpDS); showReport++;
                //            }
                //            else
                //            {
                //                errCount++;
                //                if (errMsg == "")
                //                {
                //                    errMsg = (isPUMA) ? "Mignight Consumption data not available." : "Daily Energy Consumption data not available.";

                //                }
                //                else
                //                {
                //                    string msg = (isPUMA) ? "Mignight Consumption data not available." : "Daily Energy Consumption data not available.";
                //                    errMsg = errMsg + "\n" + msg;
                //                }
                //            }
                //        }

                //        //added for MVVNL
                //        if (chkMidnightEnergy.Checked == true)
                //        {
                //            selectedParams++;
                //            DataSet midnightEnergyDS = new DataSet();
                //            DLMS650CommonBLL common = new DLMS650CommonBLL();
                //            if (isPUMA)
                //            {
                //                midnightEnergyDS = new DLMS650LoadSurveyBLL().GetPUMAMidNightData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //            }
                //            else if (UtilityDetails.ShowMidnight)
                //            {
                //                midnightEnergyDS = ListMidnightEnergies(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //                // Added to solve bug 89706.
                //                midnightEnergyDS = common.ConvertMidnightEnergy(midnightEnergyDS);
                //                // midnightEnergyDS = common.ApplyMultiplyFactor(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), midnightEnergyDS);
                //            }
                //            if (midnightEnergyDS != null && midnightEnergyDS.Tables.Count > 0 && midnightEnergyDS.Tables[0].Rows.Count > 0)
                //            {
                //                FillMidnightEnergyXSD(midnightEnergyDS);
                //                showReport++;
                //            }
                //            else
                //            {
                //                errCount++;
                //                if (errMsg == "")
                //                    errMsg = "Midnight energies not available.";
                //                else errMsg = errMsg + "\n" + "Midnight energies not available.";
                //            }
                //        }
                //        //added for MVVNL

                //        if (errCount == 0)
                //            ShowReport();
                //        else
                //        {
                //            if (string.IsNullOrEmpty(errMsg))
                //            { MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                //            else
                //            {
                //                MessageBox.Show(errMsg, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            }
                //            if (showReport > 0)
                //                ShowReport();
                //        }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
            }
        }
        private void FillPhasorTable()
        {
            //get the image file into a stream reader.
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = filePath + "PrintPage.jpg";

            FileStream FilStr = new FileStream(filePath, FileMode.Open);
            BinaryReader BinRed = new BinaryReader(FilStr);
            //PhasorDiagramTable
            DataRow reportrow = reportXSD.Tables["PhasorDiagramTable"].NewRow();
            reportrow["Image"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
            reportXSD.Tables["PhasorDiagramTable"].Rows.Add(reportrow);
            FilStr.Close();
            BinRed.Close();
        }

        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowReport()
        {
            //check file name
            string fileName = string.Empty;
            DataSet fileDataset = new DataSet();
            fileDataset = ListFileName(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (fileDataset != null)
            {
                if (fileDataset.Tables[0].Rows.Count > 0)
                {
                    fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                }
            }
            CommonMethods.MeterDataType = new DLMS650GeneralBLL().GetMeterDataType(ConfigInfo.ActiveMeterDataId);
            ReportForm ObjRptForm = new ReportForm();
            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {

                /* Add BCS Version in Report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.ReportDefinition.ReportObjects["TextBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* Add BCS Version in Report header */
                if (reportXSD.Tables["EnergyTableNET"].Rows.Count == 0)
                {
                    generalReport.secMainEnergyNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secMainEnergyNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmMainEnergyNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }



                if (reportXSD.Tables["PowerFactorNET"].Rows.Count == 0)
                {
                    generalReport.secPowerFactorNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secPowerFactorNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmPowerFactorNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }
                if (reportXSD.Tables["TODAveragePF"].Rows.Count == 0)//story 1024441
                {
                    generalReport.secTODAveragePF.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secTODAveragePF.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmTODPowerFactorNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                               iter++;
                            }
                        }
                    }
                }


                if (reportXSD.Tables["TODEnergyTableNET"].Rows.Count == 0)
                {
                    generalReport.secTODEnergyNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secTODEnergyNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmTODEnergyNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }



                if (reportXSD.Tables["TODEnergyConsumptionTableNET"].Rows.Count == 0)
                {
                    generalReport.secTODEnergyConsumptionNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secTODEnergyConsumptionNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmTODConsumptionEnergyNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }



                if (reportXSD.Tables["MaximumDemandTableNET"].Rows.Count == 0)
                {
                    generalReport.secMaximumDemandNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secMaximumDemandNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmMaxDemandNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }



                if (reportXSD.Tables["TODDemandTableNET"].Rows.Count == 0)
                {
                    generalReport.secTODDemandNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secTODDemandNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmTODDemandNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }





                if (reportXSD.Tables["EnergyConsumptionTableNET"].Rows.Count == 0)
                {
                    generalReport.secMainEnergyConsumptionNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secMainEnergyConsumptionNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int iter = 1;
                            foreach (string item in lstClmMainEnergyConsumptionNET)
                            {
                                TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + iter];
                                objText.Text = item;
                                iter++;
                            }
                        }
                    }
                }



                if (reportXSD.Tables["NameplateDetails"].Rows.Count == 0)
                {
                    generalReport.SecBillingGeneral.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["DLMS650InstantTable"].Rows.Count == 0)
                {
                    generalReport.SecMeterInstant.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["LoadFactorDT"].Rows.Count == 0)
                {
                    generalReport.secLoadFactor.SectionFormat.EnableSuppress = true;
                }
                else
                { 
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secLoadFactor.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor"];
                            TextObject objText1 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor1"];
                            //pradipta_start_081018

                            TextObject objText2 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor2"];
                            TextObject objText3 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor3"];
                            TextObject objText4 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor4"];
                            TextObject objText5 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtLoadfactor5"];
                            //pradipta_end_081018


                            // story no: 490966 - WB tender specific check implemented for Average Load factor OBIS code change
                            //if (meterModelNumber == 9)
                            //{
                            //    objText.Text = BCSConstants.LoadFactorColumn_WB;
                            //}
                            //pradipta_start_081018
                           
                            if (headerLoadFactor == "Load Factor (%) (0.0.96.1.219.255;3;2)"|| headerLoadFactor == "Load Factor (0.0.96.1.219.255;3;2)")
                            {
                                objText.Text = "Load Factor (0.0.96.1.219.255;3;2)";
                            }
                            
                                if (headerLoadFactor == "Import Load Factor (1.0.1.8.0.255;3;2)")
                                {
                                    objText.Text = "Import Load Factor (1.0.1.8.0.255;3;2)";
                                 }
                                if (headerLoadFactor1 == "Export Load Factor (1.0.2.8.0.255;3;2)")
                                {
                                    objText1.Text = "Export Load Factor (1.0.2.8.0.255;3;2)";
                                }
                                if (headerkWImportLoadFactor == "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)")
                                {
                                    headerLoadFactor1 = "";
                                    headerLoadFactor = "";
                                    objText1.Text = "";
                                    objText2.Text = "kWImport Load Factor(1.0.1.0.128.255;3;2)";
                                }
                           
                                if (headerkWExportLoadFactor == "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)")
                                {
                                    objText3.Text = "kWExport Load Factor(1.0.2.0.128.255;3;2)";
                                }

                                if (headerkVAImportLoadFactor == "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)")
                                {
                                    objText4.Text = "kVAImport Load Factor(1.0.9.0.128.255;3;2)";
                                }
                                if (headerkVAExportLoadFactor == "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)")
                                {
                                    objText5.Text = "kVAExport Load Factor(1.0.10.0.128.255;3;2)";
                                }
                            
                            //pradipta_End_081018

                           
                        }


                    }
                }

                if (reportXSD.Tables["AverageLoadDT"].Rows.Count == 0)
                {
                    generalReport.AverageLoad.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.AverageLoad.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtAverageLoad"];

                            if (!string.IsNullOrEmpty(headerAverageLoad))
                                objText.Text = headerAverageLoad;
                        }
                    }
                }

                if (reportXSD.Tables["PowerOnOffDuration"].Rows.Count == 0)
                {
                    generalReport.secPowerOnOffDuration.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.secPowerOnOffDuration.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repPowerOnOffDuration = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject txtPowerOnDuration = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtPowerOnDuration"];
                            TextObject txtPowerOffDuration = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtPowerOffDuration"];

                            // story no: 490966 - WB tender specific check implemented for Average Load factor OBIS code change
                            if (chkPowerOnOffDurationFormat == "1")
                            {
                                txtPowerOnDuration.Text = BCSConstants.PowerOnDuration_WB;
                                txtPowerOffDuration.Text = BCSConstants.PowerOffDuration_WB;
                            }
                        }
                    }
                }
                //Task_id: 579173 Self-diagnostic feature will be disabled in BCS through all the readout communication modes for f/w ver 1.66 old ruby dlms 3P meter 
                if (reportXSD.Tables["AnomalyDynamic"].Rows.Count == 0 || ConfigInfo.ActiveFirmwareVersion == "1.66")
                {
                    generalReport.AnomalySection.SectionFormat.EnableSuppress = true;
                }
                /* GKG JVVNL Current TOU Read */
                if (reportXSD.Tables["TouConfiguration"].Rows.Count == 0)
                {
                    generalReport.TouConfiguration.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.TouConfiguration.ReportObjects;
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtSeasonNumber"];
                            objText.Text = seasonNumber.ToString();
                        }
                    }
                }
                /* GKG JVVNL Current TOU Read */
                if (reportXSD.Tables["DLMS650MainEnergyTable"].Rows.Count == 0)
                {
                    generalReport.SecMainEnergy.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMainEnergy.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtBilling"];
                            // Story - 349654 - To remove the Lag, Lead and Billing Type from the detailed report (Main Energy) as it does not come from single phase non DLMS meter
                            TextObject objTextLag = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtVARHLead"];
                            TextObject objTextLead = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLag"];
                            // Hiding the Billing Type Header in cases where no corresponding value comes from the meter
                            /********Dynamic Handling at runtime depending upon data coming from meter *****/

                            DataTable dtDLMS650MainEnergyTable = reportXSD.Tables["DLMS650MainEnergyTable"];
                            bool flag = true;
                            if (dtDLMS650MainEnergyTable != null)
                            {
                                foreach (DataRow itemRow in dtDLMS650MainEnergyTable.Rows)
                                {
                                    if (Convert.ToString(itemRow["Billing Type"]).Trim() != string.Empty)
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                            }

                            if (flag)
                            {
                                objText.Width = 0;
                            }
                            else
                            {
                                // story no: 490966- Condition for Meter Model WB specific Billing Type Heading should be dynamic as mapped with Analysis View
                                if(!string.IsNullOrEmpty(headerMainEnergyBillingResetType))
                                    objText.Text = headerMainEnergyBillingResetType;
                                
                            }

                            /*if (!(meterModelNumber == 9 || meterModelNumber == 13 || ConfigInfo.ActiveFileType == BCSConstants.IEC))
                            {
                                objText.Width = 0;
                            }*/


                            /********Dynamic Handling at runtime depending upon data coming from meter *****/


                            // Story - 349654 - To remove the Lag, Lead and Billing Type from the detailed report (Main Energy) as it does not come from single phase non DLMS meter
                            //  Added new Meter model no 16 check
                            //******* Meter Model Change Required Here ***********//
                            if ((meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                            {
                                objText.Width = 0;
                                objTextLag.Width = 0;
                                objTextLead.Width = 0;
                            }
                        }
                    }
                    setReportHeaderUnitMainEnergy(generalReport);
                }

                //Main Energy Consumption 
                if (reportXSD.Tables["DLMS650EnergyConsumptionTable"].Rows.Count == 0)
                {
                    generalReport.SecMainEnergyConsumption.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderUnitMainEnergyConsumption(generalReport);
                }

                //TOD Energy 
                if ((reportXSD.Tables["DLMS650TODEnergyTable"].Rows.Count == 0))
                {
                    generalReport.SecTODEnergy.SectionFormat.EnableSuppress = true;
                }
                else { setReportHeaderTODEnergy(generalReport); }

                if ((reportXSD.Tables["TODKWhTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhTable"].Rows.Count == 0))
                {
                    generalReport.SecTODKVAhEnergy.SectionFormat.EnableSuppress = true;
                    generalReport.SecTODKWhEnergy.SectionFormat.EnableSuppress = true;
                }

                //TOD Energy Consumption
                if ((reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].Rows.Count == 0))
                {
                    generalReport.SecTODConsumption.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderTODEnergyConsumption(generalReport);
                }

                if ((reportXSD.Tables["TODKWhConsumptionTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhConsumptionTable"].Rows.Count == 0))
                {
                    generalReport.SecTODKVAhConsumption.SectionFormat.EnableSuppress = true;
                    generalReport.SecTODKWhConsumption.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["DLMS650MaximumDemandTable"].Rows.Count == 0)
                {
                    generalReport.SecMaximumDemand.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderMaximumDemand(generalReport);
                }

                //TOD Demand Section
                if (reportXSD.Tables["DLMS650TODDemandTable"].Rows.Count == 0)
                {
                    generalReport.SecTODDemand.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderTODDemand(generalReport);
                }


                if (reportXSD.Tables["DLMS650PowerFactorTable"].Rows.Count == 0)
                {
                    generalReport.SecPowerFactor.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderPowerFactor(generalReport);
                }
                if (reportXSD.Tables["DLMS650LoadSurvey"].Rows.Count == 0)
                {
                    generalReport.SecLoadSurveyDemand.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["DLMS650LoadSurveyEnergy"].Rows.Count == 0)
                {
                    generalReport.SecLoadSurveyEnergy.SectionFormat.EnableSuppress = true;
                }
                //Tamper Section
                if (reportXSD.Tables["TamperTableDynamic"].Rows.Count == 0)
                {
                    generalReport.SecTamper.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderTemper(generalReport);
                    /* VBM - Apparant energy check */
                    //if (UtilityDetails.ShowApparantEnergyInTamper)
                    //{
                    //    if (meterModelNumber == NamePlateConstants.RubyE250Value)
                    //    {
                    //        DeleteApparentEnergyColumn(generalReport.SecTamper.ReportObjects,"Text15","Text28");
                    //    }
                    //}
                    //else 
                    //{
                    //    DeleteApparentEnergyColumn(generalReport.SecTamper.ReportObjects, "Text15", "Text28");
                    //}
                    /* VBM - Apparant energy check */
                }
                if (reportXSD.Tables["PowerFailureTable"].Rows.Count == 0)
                {
                    generalReport.SecPowerFailure.SectionFormat.EnableSuppress = true;
                }
                
                if (reportXSD.Tables["TransactionDynamic"].Rows.Count == 0)
                {
                    generalReport.SecTransactions.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderTransactions(generalReport);
                    ///* VBM - Apparant energy check */
                    //if (UtilityDetails.ShowApparantEnergyInTamper)
                    //{
                    //    if (meterModelNumber == NamePlateConstants.RubyE250Value)
                    //    {
                    //        DeleteApparentEnergyColumn(generalReport.SecTransactions.ReportObjects, "txtKVAH", "txtKVAHSep");
                    //    }
                    //}
                    //else
                    //{
                    //    DeleteApparentEnergyColumn(generalReport.SecTransactions.ReportObjects, "txtKVAH", "txtKVAHSep");
                    //}
                    ///* VBM - Apparant energy check */
                }

                //Phasor Report Section
                if (reportXSD.Tables["PhasorDiagramTable"].Rows.Count == 0)
                {
                    generalReport.PhasorReportSection.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    setReportHeaderPhasor(generalReport);
                }

                ////Daily consumption Report
                //if (reportXSD.Tables["DailyConsumptionTable"].Rows.Count == 0)
                //{
                //    generalReport.SecDailyEnergyConsumption.SectionFormat.EnableSuppress = true;
                //}
                //else
                //{
                //    setReportHeaderDailyConsumption(generalReport);
                //}
                //if (reportXSD.Tables["MidNightEnergies"].Rows.Count == 0)
                //{
                //    generalReport.SecMidnightEnergies.SectionFormat.EnableSuppress = true;
                //}
                //else
                //{
                //    setReportHeaderMidNightEnergy(generalReport);
                //}
               
                if (reportXSD.Tables["PowerOffDuration"].Rows.Count == 0)
                {
                    generalReport.DetailSection1.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables[MISCELLENEOUS].Rows.Count == 0)
                {
                    generalReport.Miscelleneous.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["FraudEnergyTable"].Rows.Count == 0)
                {
                    generalReport.DetailSection2.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["TouWeekTable"].Rows.Count == 0)
                {
                    generalReport.DetailSection5.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["TouSeasonTable"].Rows.Count == 0)
                {
                    generalReport.DetailSection6.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["TouDayProfileTable"].Rows.Count == 0)
                {
                    generalReport.DetailSection7.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["TouSpecialDayTable"].Rows.Count == 0)
                {
                    generalReport.DetailSection12.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["DisconnectControl"].Rows.Count == 0)
                {
                    generalReport.DetailSection15.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["LoadControl"].Rows.Count == 0)
                {
                    generalReport.DetailSection16.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["LoadControl1Psmartmeter"].Rows.Count == 0)
                {
                    generalReport.DetailSection17.SectionFormat.EnableSuppress = true;
                }

                //for sip , rtc , dip , billing type , there is a common table named as OtherConfigurations.
                if (reportXSD.Tables["OtherConfigurations"].Rows.Count > 0)
                {
                    int rowItemCount = reportXSD.Tables["OtherConfigurations"].Rows[0].ItemArray.Length;
                    List<string> otherConfigRows = new List<string>();
                    for (int rowItem = 0; rowItem < rowItemCount; rowItem++)
                    {
                        otherConfigRows.Add(reportXSD.Tables["OtherConfigurations"].Rows[0].ItemArray[rowItem].ToString());
                    }
                    if (otherConfigRows[0] == "")//RTC
                    {
                        generalReport.DetailSection4.SectionFormat.EnableSuppress = true;
                    }
                    if (otherConfigRows[1] == "")//DIP
                    {
                        generalReport.DetailSection3.SectionFormat.EnableSuppress = true;
                    }
                    if (otherConfigRows[2] == "")//DIP
                    {
                        generalReport.DetailSection3.SectionFormat.EnableSuppress = true;
                    }
                    if (otherConfigRows[4] == "")//SIP
                    {
                        generalReport.DetailSection8.SectionFormat.EnableSuppress = true;
                    }
                }
                else
                {
                    generalReport.DetailSection4.SectionFormat.EnableSuppress = true;
                    generalReport.DetailSection3.SectionFormat.EnableSuppress = true;
                    generalReport.DetailSection8.SectionFormat.EnableSuppress = true;
                    generalReport.DetailSection9.SectionFormat.EnableSuppress = true;
                }
                //hide tod future activation date 
                if (reportXSD.Tables["TODFutureActivationDate"].Rows.Count == 0)
                {
                    generalReport.DetailSection10.SectionFormat.EnableSuppress = true;
                }
                //user story no 505185
                if (reportXSD.Tables["BillingReport_TNEB"].Rows.Count == 0)
                {
                    generalReport.secBillingReportTNEB.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["Instantaneous_TNEB"].Rows.Count == 0)
                {
                    generalReport.secInstantReportTNEB.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["RS485"].Rows.Count == 0)
                {
                    generalReport.RS485Report.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["AbcCode1"].Rows.Count == 0)
                {
                    generalReport.ABCCodeReport.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["PaymentMode"].Rows.Count == 0)
                {
                    generalReport.PaymentMode.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["MeteringMode"].Rows.Count == 0)
                {
                    generalReport.MeteringMode.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["LoadlimitMode"].Rows.Count == 0)
                {
                    generalReport.LoadLimit.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["SlidingdemandMode"].Rows.Count == 0)
                {
                    generalReport.SlidingDemand.SectionFormat.EnableSuppress = true;
                }
                if (reportXSD.Tables["OpticalRJportMode"].Rows.Count == 0)
                {
                    generalReport.PortConfiguration.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["DLMS650CumulativeMDTable"].Rows.Count == 0)
                {
                    generalReport.CumulativeMD.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["DIPforSmartmeter"].Rows.Count == 0)
                {
                    generalReport.CumulativeDIP.SectionFormat.EnableSuppress = true;
                }
               if (reportXSD.Tables["PulseEnergyType"].Rows.Count == 0)
                {
                    generalReport.PulseEnergyType.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["SoftwareBillingTable"].Rows.Count == 0)
                {
                    generalReport.SoftwareBilling.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["AutoLockTable"].Rows.Count == 0)
                {
                  generalReport.AutoLock.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["KvahSelectionTable"].Rows.Count == 0)
                {
                    generalReport.KVAHSelection.SectionFormat.EnableSuppress = true;
                }

                if (reportXSD.Tables["ManualMDResetTable"].Rows.Count == 0)
                {
                    generalReport.ManualButtonMDReset.SectionFormat.EnableSuppress = true;
                }


                // Apply modern blue theme and custom logo before rendering
                ReportThemeHelper.Apply(generalReport);

                generalReport.SetDataSource(reportXSD);
                generalReport.Refresh();
                ObjRptForm.rptViewer.ReportSource = generalReport;
                Cursor.Current = Cursors.Default;
                ObjRptForm.rptViewer.Zoom(1);
                this.Hide();
                // SB code change Start - 20180629 - Multiple Analysis View
                ObjRptForm.Show();
                // SB code change End - 20180629 - Multiple Analysis View
                reportXSD.Clear();
                this.Show();
                // SB code change Start - 20180629 - Multiple Analysis View
                ObjRptForm.BringToFront();
                // SB code change End - 20180629 - Multiple Analysis View
                Cursor.Current = Cursors.Default;
            }
        }
        /// <summary>
        /// Delete Apparent energy Column
        /// </summary>
        /// <param name="rebObjCol"></param>
        private void DeleteApparentEnergyColumn(CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol, string textColumnId, string textSeparatorid)
        {
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject objTextColumnName = (TextObject)subReportDocument.ReportDefinition.ReportObjects[textColumnId];
                    TextObject objTextSeparator = (TextObject)subReportDocument.ReportDefinition.ReportObjects[textSeparatorid];
                    objTextColumnName.Width = 0;
                    objTextSeparator.Width = 0;

                }
            }
            if (reportXSD.Tables["DLMSTamperTable"].Columns.Contains("Cumulative Energy - kVAh"))
            {
                reportXSD.Tables["DLMSTamperTable"].Columns.Remove("Cumulative Energy - kVAh");
            }
            if (reportXSD.Tables["DLMSTamperTable"].Columns.Contains("Cumulative Energy - kVAh"))
            {
                reportXSD.Tables["DLMSTamperTable"].Columns.Remove("Cumulative Energy - kVAh");
            }
        }
        private void setReportHeaderPhasor(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.PhasorReportSection.ReportObjects;
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtActivePowerKW = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtActivePowerKW"];
                    TextObject txtReactivePowerKVAR = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtReactivePowerKVAR"];
                    TextObject txtApparentPowerKVA = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtApparentPowerKVA"];
                    TextObject txtRPhaseKWDirection = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtRPhaseKWDirection"];
                    TextObject txtYPhaseKWDirection = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtYPhaseKWDirection"];
                    TextObject txtBPhaseKWDirection = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtBPhaseKWDirection"];
                    //TextObject Text7 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text7"];
                    //Text7.Width = 0;
                    //TextObject Text5 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text5"];
                    //Text5.Width = 0;
                    txtActivePowerKW.Text = CommonMethods.getDisplayHeaderText(txtActivePowerKW.Text);
                    txtReactivePowerKVAR.Text = CommonMethods.getDisplayHeaderText(txtReactivePowerKVAR.Text);
                    txtApparentPowerKVA.Text = CommonMethods.getDisplayHeaderText(txtApparentPowerKVA.Text);
                    txtRPhaseKWDirection.Text = CommonMethods.getDisplayHeaderText(txtRPhaseKWDirection.Text);
                    txtYPhaseKWDirection.Text = CommonMethods.getDisplayHeaderText(txtYPhaseKWDirection.Text);
                    txtBPhaseKWDirection.Text = CommonMethods.getDisplayHeaderText(txtBPhaseKWDirection.Text);
                }
            }
        }

        //private void setReportHeaderMidNightEnergy(MainReport generalReport)
        //{
        //    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMidnightEnergies.ReportObjects;
        //    // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
        //    foreach (ReportObject reportObject in rebObjCol)
        //    {
        //        if (reportObject.Kind == ReportObjectKind.SubreportObject)
        //        {
        //            SubreportObject subreportObject = (SubreportObject)reportObject;
        //            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
        //            TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
        //            TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
        //            TextObject txtKVARHLag = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLag"];
        //            TextObject txtVARHLead = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLead"];
        //            txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
        //            txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
        //            txtKVARHLag.Text = CommonMethods.getDisplayHeaderText(txtKVARHLag.Text);
        //            txtVARHLead.Text = CommonMethods.getDisplayHeaderText(txtVARHLead.Text);
        //        }
        //    }
        //}

        private void setReportHeaderTODEnergyConsumption(MainReport generalReport)
        {
            DataTable dtDLMS650TODEnergyConsumptionTable = reportXSD.Tables["DLMS650TODEnergyConsumptionTable"];
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecTODConsumption.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
                    TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
                    TextObject txtKVARHLAG = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKvarhLagCon"];
                    TextObject txtKVARHLEAD = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKvarhLeadCon"];
                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
                    //*****This condition for smart meter change header text ********
                    if (meterModelNumber == 24 || meterModelNumber == 25 )
                    {
                        txtKVARHLAG.Text = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart);
                        txtKVARHLEAD.Text = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart);
                    }
                    // Handle exception for HTCT Meters
                    if (meterModelNumber == 28 || meterModelNumber == 29)
                    {
                        txtKVARHLAG.Text = string.Empty;
                        txtKVARHLEAD.Text = string.Empty;
                    }

                    if (txtKVARHLAG.Text != string.Empty && reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].Rows[0][CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)].ToString() == string.Empty)
                        txtKVARHLAG.Text = string.Empty;

                    if (txtKVARHLEAD.Text != string.Empty && meterModelNumber != 28 && reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].Rows[0][CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)].ToString() == string.Empty)
                        txtKVARHLEAD.Text = string.Empty;
                    // Story - 349654 - To remove the kVAh from the detailed report(TOD Energy Consumption) as it does not come from single phase non DLMS meter
                    // This code condition is commented to show column kVAh in Detailed Report (TOD Cosnumption) for TPDDL IEC Tender requirement
                    /*if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        txtKVAH.Width = 0;
                    }*/

                    // User Story: 451613 "Set Dynamic Column kVAh visibility according to value coming or not"
                    //DataTable dtDLMS650TODEnergyConsumptionTable = reportXSD.Tables["DLMS650TODEnergyConsumptionTable"];
                    bool flag = true;
                    if (dtDLMS650TODEnergyConsumptionTable != null)
                    {
                        foreach (DataRow item in dtDLMS650TODEnergyConsumptionTable.Rows)
                        {
                            if(Convert.ToString(item["kVAh (1.0.9.8.0.255;3;2)"]).Trim() != string.Empty)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if(flag)
                    {
                        txtKVAH.Width = 0;
                    }


                }
            }
        }

        //private void setReportHeaderDailyConsumption(MainReport generalReport)
        //{
        //    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecDailyEnergyConsumption.ReportObjects;
        //    // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
        //    foreach (ReportObject reportObject in rebObjCol)
        //    {
        //        if (reportObject.Kind == ReportObjectKind.SubreportObject)
        //        {
        //            SubreportObject subreportObject = (SubreportObject)reportObject;
        //            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
        //            TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
        //            TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
        //            TextObject txtKVARHLag = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLag"];
        //            TextObject txtVARHLead = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLead"];
        //            txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
        //            txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
        //            txtKVARHLag.Text = CommonMethods.getDisplayHeaderText(txtKVARHLag.Text);
        //            txtVARHLead.Text = CommonMethods.getDisplayHeaderText(txtVARHLead.Text);

        //            if (isPUMA)
        //            {
        //                string strMidNight = "Midnight Consumption";
        //                TextObject txtHeaderText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtHeaderText"];
        //                txtHeaderText.Text = strMidNight;
        //            }
        //        }
        //    }
        //}
        private void setReportHeaderMaximumDemand(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMaximumDemand.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            // User Story - 1000867
            string unitType = CommonMethods.MeterDataType == MeterDataTypes.HTCT ? "M" : "k";
            string headerVArLag = string.Format("MD({0}VAr Lag)", unitType);
            string headerVArLead = string.Format("MD({0}VAr Lead)", unitType);
            string headerVArLagValue = "Value\n(1.0.5.6.T.255; 4; 2)";
            string headerVArLagTimeStamp = "DateTime\n(1.0.5.6.T.255; 4; 5)";
            string headerVArLeadValue = "Value\n(1.0.8.6.T.255; 4; 2)";
            string headerVArLeadTimeStamp = "DateTime\n(1.0.8.6.T.255; 4; 5)";

            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKW"];
                    TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVA"];
                    TextObject txtKWHR = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWR"];
                    TextObject txtKWHY = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWY"];
                    TextObject txtKWHB = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWB"];
                    TextObject TxtDateTimekVA = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtDateTimekVA"];
                    TextObject TxtValuekVA = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtValuekVA"];
                    TextObject TxtDateTimeBPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtDateTimeBPhase"];
                    TextObject TxtDateTimeRPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtDateTimeRPhase"];
                    TextObject TxtDateTimeYPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtDateTimeYPhase"];
                    TextObject TxtValueBPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtValueBPhase"];
                    TextObject TxtValueRPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtValueRPhase"];
                    TextObject TxtValueYPhase = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TxtValueYPhase"];
                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
                    txtKWHR.Text = CommonMethods.getDisplayHeaderText(txtKWHR.Text);
                    txtKWHY.Text = CommonMethods.getDisplayHeaderText(txtKWHY.Text);
                    txtKWHB.Text = CommonMethods.getDisplayHeaderText(txtKWHB.Text);

                    // User Story - 1000867
                    FieldObject MDkVArLag = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkVArLag"];
                    FieldObject MDkVArLagTimeStamp = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkVArLagTimeStamp"];
                    FieldObject MDkVArLead = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkVArLead"];
                    FieldObject MDkVArLeadTimeStamp = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkVArLeadTimeStamp"];
                    FieldObject MDkWRPhase = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkWRPhase"];
                    FieldObject MDkWRPhaseTimeStamp = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkWRPhaseTimeStamp"];
                    FieldObject MDkWYPhase = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkWYPhase"];
                    FieldObject MDkWYPhaseTimeStamp = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MDkWYPhaseTimeStamp"];

                    DataTable dtDLMS650MaximumDemandTable = reportXSD.Tables["DLMS650MaximumDemandTable"];

                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kVA (1.0.9.6.0.255;4;2)"))
                    {
                        if (Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kVA (1.0.9.6.0.255;4;2)"]) == string.Empty)
                        {
                            TxtDateTimekVA.Width = 0;
                            TxtValuekVA.Width = 0;
                            txtKVAH.Width = 0;
                        }
                    }
                    // User Story - 1000867
                    /* Since MD entries are hardcoded into MaximumDemand Report (MDkw, MDkva, MDkw R, Y, B phase), there is no generic parameter support.
                     * Also no space is left on Report Page. Since MD Reactive only implemented for 1PH for now so replacing R, Y Phases with MD Reactive.
                     * In future, if MD Reactive imlemented for 3PH as well, MaximumDemand Report will need to be recreated with support for generic parameter.
                     */
                    bool bContainsMDReactiveLag = false, bContainsMDReactiveLead = false;
                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kVAr Lag (1.0.5.6.0.255;4;2)"))
                    {
                        if (!(Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kVAr Lag (1.0.5.6.0.255;4;2)"]) == string.Empty || Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kVAr Lag (1.0.5.6.0.255;4;2)"]) == "------"))
                        {
                            TxtValueRPhase.Text = headerVArLagValue;
                            TxtDateTimeRPhase.Text = headerVArLagTimeStamp;
                            txtKWHR.Text = headerVArLag;

                            MDkVArLag.Left = MDkWRPhase.Left;
                            MDkVArLag.Top = MDkWRPhase.Top;
                            MDkWRPhase.Width = 0;

                            bContainsMDReactiveLag = true;
                        }
                    }
                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kW R Phase(1.0.21.6.0.255;4;2)") && !bContainsMDReactiveLag)
                    {
                        if (Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW R Phase(1.0.21.6.0.255;4;2)"]) == string.Empty || Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW R Phase(1.0.21.6.0.255;4;2)"]) == "------")
                        {
                            TxtValueRPhase.Width = 0;
                            TxtDateTimeRPhase.Width = 0;
                            txtKWHR.Width = 0;
                        }
                    }

                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kVAr Lead (1.0.8.6.0.255;4;2)"))
                    {
                        if (!(Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kVAr Lead (1.0.8.6.0.255;4;2)"]) == string.Empty || Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kVAr Lead (1.0.8.6.0.255;4;2)"]) == "------"))
                        {
                            TxtValueYPhase.Text = headerVArLeadValue;
                            TxtDateTimeYPhase.Text = headerVArLeadTimeStamp;
                            txtKWHY.Text = headerVArLead;

                            MDkVArLead.Left = MDkWYPhase.Left;
                            MDkVArLead.Top = MDkWYPhase.Top;
                            MDkWYPhase.Width = 0;

                            bContainsMDReactiveLead = true;
                        }
                    }
                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kW Y Phase(1.0.41.6.0.255;4;2)") && !bContainsMDReactiveLead)
                    {
                        if (Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW Y Phase(1.0.41.6.0.255;4;2)"]) == string.Empty || Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW Y Phase(1.0.41.6.0.255;4;2)"]) == "------")
                        {
                            TxtValueYPhase.Width = 0;
                            TxtDateTimeYPhase.Width = 0;
                            txtKWHY.Width = 0;
                        }
                    }

                    if (dtDLMS650MaximumDemandTable.Columns.Contains("MD kW B Phase(1.0.61.6.0.255;4;2)"))
                    {
                        if (Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW B Phase(1.0.61.6.0.255;4;2)"]) == string.Empty || Convert.ToString(dtDLMS650MaximumDemandTable.Rows[0]["MD kW B Phase(1.0.61.6.0.255;4;2)"]) == "------")
                        {
                            TxtValueBPhase.Width = 0;
                            TxtDateTimeBPhase.Width = 0;
                            txtKWHB.Width = 0;
                        }
                    }
                }
            }
        }

        private void setReportHeaderTODDemand(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecTODDemand.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKW"];
                    TextObject txtKVA = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVA"];
                    //Story - 349654 - To remove the kVA Values and TimeStamp from the detailed report (TOD Demand) as it does not come from single phase non DLMS meter
                    TextObject txtKVAValue = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text8"];
                    TextObject txtKVATimeStamp = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text7"];
                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVA.Text = CommonMethods.getDisplayHeaderText(txtKVA.Text);
                    // Story - 349654 - To remove the kVA Values and TimeStamp from the detailed report (TOD Demand) as it does not come from single phase non DLMS meter
                    //if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    //{
                    //    txtKVA.Width = 0;
                    //    txtKVAValue.Width = 0;
                    //    txtKVATimeStamp.Width = 0;
                    //}
                }
            }
        }
        /// <summary>
        /// Set report header for power factor
        /// </summary>
        /// <param name="generalReport"></param>
        private void setReportHeaderPowerFactor(MainReport generalReport)
        {
            string historyValue = string.Empty;
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecPowerFactor.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                    if (powerFactorCount > 0)
                    {
                        if (powerFactorCount < 8)
                        {
                            TextObject txtHistory = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text3"];
                            TextObject txtValue = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text4"];
                            txtHistory.Text = CommonMethods.getDisplayHeaderText(string.Empty);
                            txtValue.Text = CommonMethods.getDisplayHeaderText(string.Empty);
                        }
                        for (int i = powerFactorCount; i <= 12; i++)
                        {
                            historyValue = "TextH" + i;
                            TextObject txtValue = (TextObject)subReportDocument.ReportDefinition.ReportObjects[historyValue];
                            txtValue.Text = CommonMethods.getDisplayHeaderText(string.Empty);
                        }

                    }

                }
            }
        }
       
        private void setReportHeaderUnitMainEnergy(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMainEnergy.ReportObjects;
            CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
                    TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
                    TextObject txtKVARHLag = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLag"];
                    TextObject txtVARHLead = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtVARHLead"];

                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
                    txtKVARHLag.Text = CommonMethods.getDisplayHeaderText(txtKVARHLag.Text);
                    txtVARHLead.Text = CommonMethods.getDisplayHeaderText(txtVARHLead.Text);

                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter.User Story no 474879 
                    //*****kVAh visibility (yes/no) is mapped with the data coming from meter or not********
                    DataTable dtDLMS650MainEnergyTable = reportXSD.Tables["DLMS650MainEnergyTable"];
                    if (dtDLMS650MainEnergyTable != null)
                    {

                        string RemoveLagClmName = string.Empty;
                        string RemoveLeadClmName = string.Empty;
                        string RemoveKVAHClmName = string.Empty;

                        foreach (DataColumn item in dtDLMS650MainEnergyTable.Columns)
                        {
                            if (item.ColumnName.Contains("kVAh") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveKVAHClmName = item.ColumnName;
                            }
                            if (item.ColumnName.Contains("Lag") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveLagClmName = item.ColumnName;
                            }
                            if (item.ColumnName.Contains("Lead") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveLeadClmName = item.ColumnName;
                            }
                        }

                        if (RemoveKVAHClmName != string.Empty)
                        {
                            txtKVAH.Width = 0;
                        }
                        if (RemoveLagClmName != string.Empty)
                        {
                            txtKVARHLag.Width = 0;
                        }
                        if (RemoveLeadClmName != string.Empty)
                        {
                            txtVARHLead.Width = 0;
                        }
                    }
                }
            }
        }

        private void setReportHeaderUnitMainEnergyConsumption(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMainEnergyConsumption.ReportObjects;
            CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
                    TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
                    TextObject txtKVARHLag = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLag"];
                    TextObject txtVARHLead = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVARHLead"];
                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);
                    txtKVARHLag.Text = CommonMethods.getDisplayHeaderText(txtKVARHLag.Text);
                    txtVARHLead.Text = CommonMethods.getDisplayHeaderText(txtVARHLead.Text);
                    // Story - 349654 - To remove the Lag and Lead from the detailed report (Main Energy Consumption) as it does not come from single phase non DLMS meter
                    // Added new Meter model no 16 check
                    //******* Meter Model Change Required Here ***********//
                    if ((meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        txtKVARHLag.Width = 0;
                        txtVARHLead.Width = 0;
                    }

                    // Dynamic visibility is implemented for kVArhLag and kVArhLead Parameter.User Story no 474879 
                    //*****kVAh visibility (yes/no) is mapped with the data coming from meter********
                    DataTable dtDLMS650MainEnergyTable = reportXSD.Tables["DLMS650MainEnergyTable"];
                    if (dtDLMS650MainEnergyTable != null)
                    {

                        string RemoveLagClmName = string.Empty;
                        string RemoveLeadClmName = string.Empty;
                        string RemoveKVAHClmName = string.Empty;

                        foreach (DataColumn item in dtDLMS650MainEnergyTable.Columns)
                        {
                            if (item.ColumnName.Contains("kVAh") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveKVAHClmName = item.ColumnName;
                            }
                            if (item.ColumnName.Contains("Lag") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveLagClmName = item.ColumnName;
                            }
                            if (item.ColumnName.Contains("Lead") && (dtDLMS650MainEnergyTable.Rows.Count > 0) && Convert.ToString(dtDLMS650MainEnergyTable.Rows[0][item.ColumnName]) == string.Empty)
                            {
                                RemoveLeadClmName = item.ColumnName;
                            }
                        }

                        if (RemoveKVAHClmName != string.Empty)
                        {
                            txtKVAH.Width = 0;
                        }
                        if (RemoveLagClmName != string.Empty)
                        {
                            txtKVARHLag.Width = 0;
                        }
                        if (RemoveLeadClmName != string.Empty)
                        {
                            txtVARHLead.Width = 0;
                        }
                    }
                }
            }
        }

        private void setReportHeaderTODEnergy(MainReport generalReport)
        {
            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecTODEnergy.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            //reportXSD.Tables["DLMS650TODEnergyTable"].Rows[0][4].ToString()
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    TextObject txtKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKWH"];
                    TextObject txtKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtKVAH"];
                    TextObject txtKVARHLAG = (TextObject)subReportDocument.ReportDefinition.ReportObjects["kvarhLag"];
                    TextObject txtKVARHLEAD = (TextObject)subReportDocument.ReportDefinition.ReportObjects["kvarhLead"];
                    txtKWH.Text = CommonMethods.getDisplayHeaderText(txtKWH.Text);
                    txtKVAH.Text = CommonMethods.getDisplayHeaderText(txtKVAH.Text);

                    //*****This condition for smart meter change header text ********
                    if (meterModelNumber == 24 || meterModelNumber == 25)
                    {
                        txtKVARHLAG.Text = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG_smart);
                        txtKVARHLEAD.Text = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD_smart);
                    }
                    if (meterModelNumber == 28 || meterModelNumber == 29)
                    {
                        txtKVARHLAG.Text = string.Empty;
                        txtKVARHLEAD.Text = string.Empty;
                    }

                    if (txtKVARHLAG.Text != string.Empty && reportXSD.Tables["DLMS650TODEnergyTable"].Rows[0][CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)].ToString() == string.Empty)
                        txtKVARHLAG.Text = string.Empty;
                    if (txtKVARHLEAD.Text != string.Empty && reportXSD.Tables["DLMS650TODEnergyTable"].Rows[0][CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)].ToString() == string.Empty)
                        txtKVARHLEAD.Text = string.Empty;
                    // Story - 349654 - To remove the kVAh from the detailed report (TOD Energy) as it does not come from single phase non DLMS meter
                    // This code condition is commented to show column kVAh in Detailed Report (TOD Energy) for TPDDL IEC Tender requirement
                    /*if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        txtKVAH.Width = 0;
                    }*/

                    // User Story: 451613 "Set Dynamic Column kVAh visibility according to value coming or not"
                    DataTable dtDLMS650TODEnergyTable = reportXSD.Tables["DLMS650TODEnergyTable"];
                    bool flag = true;
                    if (dtDLMS650TODEnergyTable != null)
                    {
                        foreach (DataRow item in dtDLMS650TODEnergyTable.Rows)
                        {
                            if (Convert.ToString(item["kVAh (1.0.9.8.0.255;3;2)"]).Trim() != string.Empty)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        txtKVAH.Width = 0;
                    }


                }
            }
        }

        private void setReportHeaderTemper(MainReport generalReport)
        {
            try
            {
                TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                DataSet dataSet = new DataSet();
                string[] tamperHeadings = new string[15];
                dataSet = tamperParameterBLL.GetColumnNames(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                MeterDataTypes MeterDataType = new DLMS650GeneralBLL().GetMeterDataType(ConfigInfo.ActiveMeterDataId);
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    tamperHeadings = ("EventCode,TamperDescription,DateTimeEvent," + dataSet.Tables[0].Rows[0][0].ToString()).Split(',');
                }
                CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecTamper.ReportObjects;
                // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                foreach (ReportObject reportObject in rebObjCol)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        for (int selectedItemCount = 0; selectedItemCount < (tamperHeadings.Length - 3); selectedItemCount++)
                        {
                            TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount).ToString()] as TextObject;

                            if (groupHeaderTextObject != null)
                            {
                                switch (tamperHeadings[selectedItemCount + 3].ToString())
                                {
                                    case "CurrentIR":
                                        groupHeaderTextObject.Text = "Current - IR";
                                        break;
                                    case "CurrentIY":
                                        groupHeaderTextObject.Text = "Current - IY";
                                        break;
                                    case "CurrentIB":
                                        groupHeaderTextObject.Text = "Current - IB";
                                        break;
                                    case "PhaseCurrent":
                                        groupHeaderTextObject.Text = "Metering Current"; //Phase Current name change because of common nomenclature of Phase current for 1Phase meter tamper snapshot as “Metering Current”. 
                                        break;
                                    case "VoltageVRN":
                                        groupHeaderTextObject.Text = "Voltage - VRN";
                                        break;
                                    case "VoltageVYN":
                                        groupHeaderTextObject.Text = "Voltage - VYN";
                                        break;
                                    case "VoltageVBN":
                                        groupHeaderTextObject.Text = "Voltage - VBN";
                                        break;
                                    case "PhaseVoltage":
                                        groupHeaderTextObject.Text = "Phase Voltage";
                                        break;
                                    case "PowerFactorRphase":
                                        groupHeaderTextObject.Text = "PowerFactor - R Phase";
                                        break;
                                    case "PowerFactorYphase":
                                        groupHeaderTextObject.Text = "PowerFactor - Y Phase";
                                        break;
                                    case "PowerFactorBphase":
                                        groupHeaderTextObject.Text = "PowerFactor - B Phase";
                                        break;
                                    case "TotalPowerFactor":
                                        groupHeaderTextObject.Text = "Total Power Factor";
                                        break;
                                    case "CumulativeEnergykWh":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kWh";
                                        break;
                                    case "CumulativeEnergykVAh":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kVAh";
                                        break;
                                    case "NeutralCurrent":
                                        groupHeaderTextObject.Text = "Neutral Current - A"; // Story - 349654 - Neutral Current in Tamper
                                        break;                                    
                                    case "HighNeutralCurrent":
                                        groupHeaderTextObject.Text = "High Neutral Current - A"; 
                                        break;

                                    case "CumulativeEnergykvarhLag":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kvarh Lag";
                                        break;
                                    case "CumulativeEnergykvarhLead":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kvarh Lead";
                                        break;
                                    case "CumulativeEnergykWhImport":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kWh Forward";
                                        break;
                                    case "CumulativeEnergykWhExport":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kWh Export";
                                        break;
                                    case "CumulativeEnergykVAhImport":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kVAh Forward";
                                        break;
                                    case "CumulativeEnergykVAhExport":
                                        groupHeaderTextObject.Text = "Cumulative Energy - kVAh Export";
                                        break;

                                    //SarkarA code change start 20180110 // add new tamper parameters //corrected nomenclature 20180122
                                    case "kWr":
                                        groupHeaderTextObject.Text = "ActivePower R";
                                        break;
                                    case "kWy":
                                        groupHeaderTextObject.Text = "ActivePower Y";
                                        break;
                                    case "kWb":
                                        groupHeaderTextObject.Text = "ActivePower B";
                                        break;
                                    case "kVAr":
                                        groupHeaderTextObject.Text = "ApparentPower R";
                                        break;
                                    case "kVAy":
                                        groupHeaderTextObject.Text = "ApparentPower Y";
                                        break;
                                    case "kVAb":
                                        groupHeaderTextObject.Text = "ApparentPower B";
                                        break;
                                    case "ActiveCurrentR":
                                        groupHeaderTextObject.Text = "ActiveCurrent R";
                                        break;
                                    case "ActiveCurrentY":
                                        groupHeaderTextObject.Text = "ActiveCurrent Y";
                                        break;
                                    case "ActiveCurrentB":
                                        groupHeaderTextObject.Text = "ActiveCurrent B";
                                        break;
                                    //SarkarA code change end 20180110 

                                    //SarkarA code change start 20180330 // add phase current instant, frequency
                                    case "PhaseCurrentInstant":
                                        groupHeaderTextObject.Text = "Phase Current";
                                        break;
                                    case "Frequency":
                                        groupHeaderTextObject.Text = "Frequency";
                                        break;
                                    case "CumulativeTamperCount":
                                        groupHeaderTextObject.Text = "CumulativeTamperCount";
                                        break;
                                    //SarkarA code change end 20180330
                                    case "Temprature":
                                        groupHeaderTextObject.Text = "Temprature "+"("+ Convert.ToChar (0176)+ "C"+")";
                                        break;
                                    case "THDVR":
                                        groupHeaderTextObject.Text = "THDVR";
                                        break;
                                    case "THDVY":
                                        groupHeaderTextObject.Text = "THDVY";
                                        break;
                                    case "THDVB":
                                        groupHeaderTextObject.Text = "THDVB";
                                        break;
                                    case "THDIR":
                                        groupHeaderTextObject.Text = "THDIR";
                                        break;
                                    case "THDIY":
                                        groupHeaderTextObject.Text = "THDIY";
                                        break;
                                    case "THDIB":
                                        groupHeaderTextObject.Text = "THDIB";
                                        break;
                                    case "ByPassCurrent":
                                        groupHeaderTextObject.Text = "ByPass Current - A";
                                        break;
                                }
                                //For HTCT Mega Variant support
                                if(MeterDataType == MeterDataTypes.HTCT_MEGA)
                                {
                                    groupHeaderTextObject.Text = groupHeaderTextObject.Text.Replace("- k", "- M");

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "setReportHeaderTemper(MainReport generalReport)", ex);

            }
        }


        private void setReportHeaderTransactions(MainReport generalReport)
        {
            TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
            DataSet dataSet = new DataSet();
            string[] transactionHeadings = new string[15];
            dataSet = tamperParameterBLL.GetColumnNames(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                transactionHeadings = ("TamperType, EventCode, DateTimeEvent," + dataSet.Tables[0].Rows[0][0].ToString()).Split(',');
            }

            CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecTransactions.ReportObjects;
            // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
            foreach (ReportObject reportObject in rebObjCol)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                    for (int selectedItemCount = 0; selectedItemCount < (transactionHeadings.Length - 3); selectedItemCount++)
                    {
                        TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount).ToString()] as TextObject;

                        if (groupHeaderTextObject != null)
                        {
                            if (transactionHeadings[selectedItemCount + 3].ToString() == "CurrentIR")
                                groupHeaderTextObject.Text = "Current - IR";
                            else
                                if (transactionHeadings[selectedItemCount + 3].ToString() == "CurrentIY")
                                    groupHeaderTextObject.Text = "Current - IY";
                                else
                                    if (transactionHeadings[selectedItemCount + 3].ToString() == "CurrentIB")
                                        groupHeaderTextObject.Text = "Current - IB";
                                    else
                                        if (transactionHeadings[selectedItemCount + 3].ToString() == "PhaseCurrent")
                                            groupHeaderTextObject.Text = "Metering Current"; //Phase Current name change because of common nomenclature of Phase current for 1Phase meter tamper snapshot as “Metering Current”. 
                                        else
                                            if (transactionHeadings[selectedItemCount + 3].ToString() == "VoltageVRN")
                                                groupHeaderTextObject.Text = "Voltage - VRN";
                                            else
                                                if (transactionHeadings[selectedItemCount + 3].ToString() == "VoltageVYN")
                                                    groupHeaderTextObject.Text = "Voltage - VYN";
                                                else
                                                    if (transactionHeadings[selectedItemCount + 3].ToString() == "VoltageVBN")
                                                        groupHeaderTextObject.Text = "Voltage - VBN";
                                                    else
                                                        if (transactionHeadings[selectedItemCount + 3].ToString() == "PhaseVoltage")
                                                            groupHeaderTextObject.Text = "Phase Voltage";
                                                        else
                                                            if (transactionHeadings[selectedItemCount + 3].ToString() == "PowerFactorRphase")
                                                                groupHeaderTextObject.Text = "PowerFactor - R Phase";
                                                            else
                                                                if (transactionHeadings[selectedItemCount + 3].ToString() == "PowerFactorYphase")
                                                                    groupHeaderTextObject.Text = "PowerFactor - Y Phase";
                                                                else
                                                                    if (transactionHeadings[selectedItemCount + 3].ToString() == "PowerFactorBphase")
                                                                        groupHeaderTextObject.Text = "PowerFactor - B Phase";
                                                                    else
                                                                        if (transactionHeadings[selectedItemCount + 3].ToString() == "TotalPowerFactor")
                                                                            groupHeaderTextObject.Text = "Total Power Factor";
                                                                        else
                                                                            if (transactionHeadings[selectedItemCount + 3].ToString() == "CumulativeEnergykWh")
                                                                                groupHeaderTextObject.Text = "Cumulative Energy - kWh";
                                                                            else
                                                                                if (transactionHeadings[selectedItemCount + 3].ToString() == "CumulativeEnergykVAh")
                                                                                    groupHeaderTextObject.Text = "Cumulative Energy - kVAh";

                        }
                    }
                }
            }
        }
        private void SMD_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls;
            Control.ControlCollection LoadSurveyCollection = this.groupBoxLoadSurvey.Controls;
            Control.ControlCollection TamperCollection = this.groupBoxOthers.Controls;

            if (SMD_SelectAll.Checked == true)
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = true;
                }
                foreach (Control C1 in TamperCollection)
                {
                    ((CheckBox)C1).Checked = true;
                }
                foreach (Control C2 in LoadSurveyCollection)
                {
                    if (C2.Text == "Load Survey")
                    {
                        ((CheckBox)C2).Checked = true;
                    }
                }
            }
            else
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = false;
                }
                foreach (Control C1 in TamperCollection)
                {
                    ((CheckBox)C1).Checked = false;
                }
                foreach (Control C2 in LoadSurveyCollection)
                {
                    if ((C2.Text == "Load Survey") || (C2.Text == "DTM Load Survey"))
                    {
                        ((CheckBox)C2).Checked = false;
                    }
                }
            }
        }
        /// <summary>
        /// display tod data
        /// </summary>
        /// <returns></returns>
        private bool DisplayTODData()
        {
            bool isTodDataFound = false;
            try
            {
                byte[] activeSeasonProfile;
                byte[] activeWeekProfile;
                byte[] activeDayProfile;
                byte[] specialDayProfile = null;
                string TODFutureActivationDate;
                DLMS650FormatterCommon common = new DLMS650FormatterCommon();
                string todData = new TodBLL().GetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                if (!string.IsNullOrEmpty(todData) && todData.Contains("DLMS"))
                {
                    string[] touData = todData.Split('\\');
                    if (touData != null && touData.Length > 7)
                    {
                        activeSeasonProfile = SoapHexBinary.Parse(touData[4]).Value;
                        activeWeekProfile = SoapHexBinary.Parse(touData[5]).Value;
                        activeDayProfile = SoapHexBinary.Parse(touData[6]).Value;
                        if (meterModelNumber == 24 || meterModelNumber == 25)
                        {
                            specialDayProfile = SoapHexBinary.Parse(touData[8]).Value;
                        }

                        //if Active season,week,day profile not present in meter then skip all three conditions
                        if (activeSeasonProfile != null && activeSeasonProfile.Length != 0)
                        {
                            FillSeasonProfileParameters(activeSeasonProfile);
                        }
                        if (activeWeekProfile != null && activeWeekProfile.Length != 0)
                        {
                            FillWeekProfileParameters(activeWeekProfile);
                        }
                        if (activeDayProfile != null && activeDayProfile.Length != 0)
                        {
                            FillDayProfileParameters(activeDayProfile);
                        }
                        if (specialDayProfile != null && specialDayProfile.Length != 0)
                        {
                            FillSpecialDayProfileParameters(specialDayProfile);
                        }
                        isTodDataFound = true;
                    }
                    //to extract data from Tod Data
                    if (!string.IsNullOrEmpty(touData[touData.Length - 1]) && touData[touData.Length - 1].Length > 12)
                    {
                        TODFutureActivationDate = touData[7];
                        string year = common.ConvertHexToDecimal(TODFutureActivationDate.Substring(4, 4)).ToString();
                        string month = common.ConvertHexToDecimal(TODFutureActivationDate.Substring(8, 2)).ToString();
                        if (month.Length == 1)
                        {
                            month = "0" + month;
                        }
                        string day = common.ConvertHexToDecimal(TODFutureActivationDate.Substring(10, 2)).ToString();
                        if (day.Length == 1)
                        {
                            day = "0" + day;
                        }
                        //if future activation date is 00 then default 01-01-2001 for all meter
                        if (year == "0" && month == "00" && day == "00")
                        {
                            FillTODFutureActivationDate("01" + "/" + "01" + "/" + "2001");
                        }
                        else
                        FillTODFutureActivationDate(day + "/" + month + "/" + year);
                    }
                }
                // Report for TOD Data IEC Meters
                if (!string.IsNullOrEmpty(todData) && todData.Contains("\x04"))
                {
                    string[] toddataarray = todData.Split('\x04');
                    FillSeasonProfileParametersIEC(toddataarray, 0);
                    FillDayProfileParametersIEC(toddataarray, 0);
                    FillWeekProfileParametersIEC(toddataarray, 0);
                    isTodDataFound = true;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isTodDataFound = false;
                logger.Log(LOGLEVELS.Error, "DisplayTODData()", ex);
            }
            return isTodDataFound;
        }

        /// <summary>
        /// This method is used for filling session profile details in TOU grids.
        /// </summary>
        private void FillSeasonProfileParameters(byte[] buffer)
        {
            try
            {

                DataRow reportRow;
                int seasonProfileCount = buffer[1];
                int nIndex = 2;
                int StartOfData = 0;
                StartOfData = buffer[5];
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                    reportRow = reportXSD.Tables["TouSeasonTable"].NewRow();
                   // nIndex += 4;
                    nIndex += 3;
                    nIndex += StartOfData;

                     int tariff = buffer[nIndex++];
                     if (tariff > 0 && tariff < 5)
                     {
                         reportRow["WeekProfile"] = tariff.ToString("00");
                     }
                     else
                     {
                         reportRow["WeekProfile"] = "";
                     }

                    nIndex += 4;
                     tariff = buffer[nIndex++];
                     if (tariff > 0 && tariff < 13)
                     {
                         reportRow["StartMonth"] = tariff.ToString("00");
                     }
                     else
                     {
                         reportRow["StartMonth"] = "";
                     }
                     tariff = buffer[nIndex++];
                     if (tariff > 0 && tariff < 32)
                     {
                         reportRow["StartDay"] = tariff.ToString("00");
                     }
                     else
                     {
                         reportRow["StartDay"] = "";
                     }
                    nIndex += 11;
                    reportXSD.Tables["TouSeasonTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillSeasonProfileParameters(byte[] buffer)", ex);
                throw ex;
            }
        }
        /// <summary>
        ///// This method is used for filling weekly profile details in TOU grids.
        ///// </summary>
        private void FillWeekProfileParameters(byte[] buffer)
        {
            int weekProfileCount = buffer[1];
            int nIndex = 2;
            DataRow reportRow;

            int StartOfData = 0;
            StartOfData = buffer[5];

            try
            {
                for (byte weekCount = 0; weekCount < weekProfileCount; weekCount++)
                {
                   // nIndex += 4; 
                    nIndex += 3;
                    nIndex += StartOfData;
                    reportRow = reportXSD.Tables["TouWeekTable"].NewRow();
                    int tariff = buffer[nIndex++];
                    reportRow["WeekNo"] = tariff.ToString("00");

                    nIndex++;
                    tariff = buffer[nIndex++];
                    if(tariff > 0 && tariff < 5)
                        reportRow["Monday"] = tariff.ToString("00");
                    else
                        reportRow["Monday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Tuesday"] = tariff.ToString("00");
                    else
                        reportRow["Tuesday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Wednesday"] = tariff.ToString("00");
                    else
                        reportRow["Wednesday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Thursday"] = tariff.ToString("00");
                    else
                        reportRow["Thursday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Friday"] = tariff.ToString("00");
                    else
                        reportRow["Friday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Saturday"] = tariff.ToString("00");
                    else
                        reportRow["Saturday"] = "";
                    nIndex++;
                    tariff = buffer[nIndex++];
                    if (tariff > 0 && tariff < 5)
                        reportRow["Sunday"] = tariff.ToString("00");
                    else
                        reportRow["Sunday"] = "";

                    reportXSD.Tables["TouWeekTable"].Rows.Add(reportRow);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillWeekProfileParameters(byte[] buffer)", ex);
                throw ex;
            }
        }
        //******** Smart meter special day profile Report Added *********************
        private void FillSpecialDayProfileParameters(byte[] specialDayProfile)
        {
            try
            {
                byte SpecialDayProfileCount = 1;
                int nIndex = 11;
                SpecialDayProfileCount = Convert.ToByte(specialDayProfile[1]);
                DataRow reportRow;
                for (byte seasonCount = 0; seasonCount < SpecialDayProfileCount; seasonCount++)
                {
                    reportRow = reportXSD.Tables["TouSpecialDayTable"].NewRow();
                    reportRow["Days"] = (seasonCount+1).ToString("00");
                    reportRow["Month"] = specialDayProfile[nIndex++].ToString("00");
                    reportRow["Date"] = specialDayProfile[nIndex++].ToString("00");
                    nIndex += 2;
                    reportRow["DayID"] = specialDayProfile[nIndex++].ToString("00");
                    nIndex += 9;
                    reportXSD.Tables["TouSpecialDayTable"].Rows.Add(reportRow);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillSpecialDayProfileParameters(byte[] specialDayProfile)", ex);
            }
        }
        /// <summary>
        /// This method is used for filling day profile details in TOU grids.
        /// </summary>
        private void FillDayProfileParameters(byte[] buffer)
        {
            DataRow reportRow;
            try
            {
                int dayProfileCount = buffer[1];
                int nIndex = 2;
                for (byte dayCount = 1; dayCount <= dayProfileCount; dayCount++)
                {
                    nIndex += 6;
                    for (byte rowCount = 1; rowCount <= 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
                        int tariff = buffer[nIndex++];

                        string endHour = string.Empty;
                        string endMin = string.Empty;

                        if (rowCount == 10)
                        {
                            endHour = "00";
                            endMin = "00";
                        }
                        else
                        {
                            endHour = buffer[nIndex + 4].ToString("d2");
                            endMin = buffer[nIndex + 5].ToString("d2");
                        }


                        if (tariff == 0)
                        {
                            // break;
                        }
                        else
                        {
                            reportRow = reportXSD.Tables["TouDayProfileTable"].NewRow();
                            reportRow["S. No."] = "Day Profile " + dayCount.ToString("d2");
                            reportRow["Slot No."] = rowCount.ToString("d2");
                            reportRow["Zone Start Time(HH:MM)"] = startHour + ":" + startMin;
                            reportRow["Zone End Time(HH:MM)"] = endHour + ":" + endMin;
                            reportRow["Tariff Zone"] = "T" + tariff.ToString();
                            reportXSD.Tables["TouDayProfileTable"].Rows.Add(reportRow);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillDayProfileParameters(byte[] buffer)", ex);
                throw ex;
            }
        }

        bool ValidateForm()
        {
            bool boolCheck = false;

            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls; //this.ChkgroupBox.Controls;
            Control.ControlCollection LoadSurveyCollection = this.groupBoxLoadSurvey.Controls;
            Control.ControlCollection TamperCollection = this.groupBoxOthers.Controls;
            foreach (Control C in BillingCollection)
            {
                if (((CheckBox)C).Checked == true)
                {
                    boolCheck = true;
                }
                else
                {
                    foreach (Control C1 in TamperCollection)
                    {
                        if (((CheckBox)C1).Checked == true)
                        {
                            boolCheck = true;
                        }
                    }
                    foreach (Control C2 in LoadSurveyCollection)
                    {
                        if (C2.Text.Contains("Load Survey"))
                        {
                            if (((CheckBox)C2).Checked == true)
                            {
                                boolCheck = true;
                            }
                        }
                    }
                }
            }
            return boolCheck;
        }

        string GetEnergyColumnName(string ColName)
        {
            if (ColName == "Demand kW") return ColName = "Energy kWh";
            else if (ColName == "Demand kVA") return ColName = "Energy kVAh";
            else if (ColName == "Demand kVAr Lag" || ColName == "Demand kVAr(Lag)") return ColName = "Energy kVArh Lag";
            else if (ColName == "Demand kVAr Lead" || ColName == "Demand kVAr(Lead)") return ColName = "Energy kVArh Lead";
            else return null;
        }

        private DataTable GetFormatedtable(DataTable dt)
        {
            DataTable dtnew = new DataTable();
            dtnew.Columns.Add("Parameters");
            dtnew.Columns.Add("Values");
            foreach (DataColumn clm in dt.Columns)
            {
                DataRow dr = dtnew.NewRow();
                dr["Parameters"] = clm.ColumnName;
                dr["Values"] = (dt.Rows[0][clm.ColumnName]).ToString();
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }

        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;
            xy.X = (float)Math.Cos(radians) * radius + origin.X;
            xy.Y = (float)Math.Sin(radians) * radius + origin.Y; //In Reverse direction sign should be +ve
            return xy;
        }

        private void SMD_chkLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLoadSurvey.Checked == true)
            {
                SMD_chkDTMLoadSurvey.Checked = false;
                SMD_chkDTMLoadSurvey.Enabled = false;
                SMD_rbtnLoadSurveyDemand.Enabled = true;
                SMD_rbtnLoadSurveyEnergy.Enabled = true;
            }
            else if (chkLoadSurvey.Checked == false)
                SMD_chkDTMLoadSurvey.Enabled = true;
        }

        private void SMD_chkDTMLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            if (SMD_chkDTMLoadSurvey.Checked == true)
            {
                chkLoadSurvey.Checked = false;
                chkLoadSurvey.Enabled = false;
                SMD_rbtnLoadSurveyDemand.Enabled = false;
                SMD_rbtnLoadSurveyEnergy.Enabled = false;
            }
            else
            {
                chkLoadSurvey.Enabled = true;
                SMD_rbtnLoadSurveyDemand.Enabled = true;
                SMD_rbtnLoadSurveyEnergy.Enabled = true;
            }
        }

        /// <summary>
        /// This method is used to show and hide checkboxes based on the values in the database
        /// </summary>
        public void ShowCheckBox()
        {
            TabNameBLL tabNameBll = new TabNameBLL();
            TabEnum tabSwitchCheck;
            DataSet tabNameData = tabNameBll.GetNoDataTabs(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (tabNameData != null && tabNameData.Tables != null && tabNameData.Tables.Count > 0)
            {
                int dataSetLength = tabNameData.Tables[0].Rows.Count;
                foreach (DataRow dataSetRow in tabNameData.Tables[0].Rows)
                {
                    tabSwitchCheck = (TabEnum)Enum.Parse(typeof(TabEnum), dataSetRow[0].ToString().Trim(), true);

                    switch (tabSwitchCheck)
                    {
                        case TabEnum.Gen:
                            {
                                chkGeneralReport.Visible = false;

                                break;
                            }
                        case TabEnum.InsRea:
                            {

                                chkInstantReport.Visible = false;
                                break;
                            }
                        case TabEnum.InsSel:
                            {

                                break;
                            }
                        case TabEnum.InsABC:
                            {
                                break;
                            }
                        case TabEnum.BilEneMai:
                            {
                                chkMainEnergy.Visible = false;
                                break;
                            }
                        case TabEnum.BilEneCon:
                            {
                                chkEnergyConsumption.Visible = false;
                                break;
                            }
                        case TabEnum.BilEneTodEne:
                            {
                                chkTODEnergy.Visible = false;
                                break;
                            }
                        case TabEnum.BilEneTodCon:
                            {
                                chkTODConsumption.Visible = false;
                                break;
                            }
                        case TabEnum.BilDemMax:
                            {
                                chkMaximumDemand.Visible = false;
                                break;
                            }
                        case TabEnum.BilDemTod:
                            {

                                break;
                            }
                        case TabEnum.BilDemPowOff:
                            {
                                chkPowerOffDuration.Visible = false;
                                break;
                            }
                        case TabEnum.BilDemPowFac:
                            {
                                chkPowerFactor.Visible = false;
                                break;
                            }
                        case TabEnum.BilMis:
                            {
                                chkMiscelleneous.Visible = false;
                                break;
                            }
                        case TabEnum.BilTouCon:
                            {
                                chkTouConfiguration.Visible = false;
                                break;
                            }
                        case TabEnum.Tam:
                            {
                                chkTamper.Visible = false;
                                break;
                            }
                        case TabEnum.LoaSur:
                            {

                                break;
                            }
                        //case TabEnum.MidDat:
                        //    {

                        //        break;
                        //    }
                        case TabEnum.Tra:
                            {
                                chkTransactions.Visible = false;
                                break;
                            }
                        case TabEnum.Pha:
                            {
                                chkPhasor.Visible = false;
                                break;
                            }
                        //case TabEnum.MidEne:
                        //    {
                        //        chkMidnightEnergy.Visible = false;
                        //        break;
                        //    }
                        //case TabEnum.DaiEneCon:
                        //    {
                        //        chkDailyEnergyConsumption.Visible = false;
                        //        break;
                        //    }


                    }

                }
            }
        }

        /// <summary>
        /// it adds grid view control to the form , and also adds rows it , dynamically.
        /// </summary>
        public void ShowGridViewControl()
        {
            //checkList.Add(CheckBoxEnum.dem);
            TabNameBLL tabNameBll = new TabNameBLL();
            TabEnum tabSwitchCheck;
            DataSet tabNameData = tabNameBll.GetTabsToShow(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (tabNameData != null && tabNameData.Tables != null && tabNameData.Tables.Count > 0)
            {
                int dataSetLength = tabNameData.Tables[0].Rows.Count;
                foreach (DataRow dataSetRow in tabNameData.Tables[0].Rows)
                {
                    tabSwitchCheck = (TabEnum)Enum.Parse(typeof(TabEnum), dataSetRow[0].ToString().Trim(), true);

                    switch (tabSwitchCheck)
                    {
                        case TabEnum.Gen:
                            {
                                chkGeneralReport.Visible = false;
                                checkList.Add(CheckBoxEnum.GeneralReport);

                                //userControl31.addEnumList(CheckBoxEnum.gen_rep);
                                break;
                            }
                        case TabEnum.InsRea:
                            {
                                checkList.Add(CheckBoxEnum.InstantReport);

                                break;
                            }

                        case TabEnum.BilEneMai:
                            {
                                checkList.Add(CheckBoxEnum.MainEnergy);
                                break;
                            }
                        case TabEnum.BilEneCon:
                            {
                                checkList.Add(CheckBoxEnum.MainEnergyCons);
                                break;
                            }
                        case TabEnum.BilEneTodEne:
                            {
                                checkList.Add(CheckBoxEnum.TODEnergy);
                                break;
                            }
                        case TabEnum.BilEneTodCon:
                            {
                                checkList.Add(CheckBoxEnum.TODConsumption);
                                break;
                            }
                        case TabEnum.BilDemMax:
                            {
                                checkList.Add(CheckBoxEnum.Demand);
                                break;
                            }

                        //case TabEnum.BilDemPowOff:
                        //    {
                        //        checkList.Add(CheckBoxEnum.PowerOffDuration);
                        //        break;
                        //    }
                        case TabEnum.BilDemPowFac:
                            {
                                checkList.Add(CheckBoxEnum.PowerFactor);
                                break;
                            }
                        case TabEnum.BilDemAvgLoaFac:
                            {
                                checkList.Add(CheckBoxEnum.LoadFactor);
                                break;
                            }
                        case TabEnum.BilDemAvgLoad:
                            {
                                checkList.Add(CheckBoxEnum.AverageLoad);
                                break;
                            }
                        case TabEnum.BilDemPowOn:
                            {
                                checkList.Add(CheckBoxEnum.PowerOnOffDuration);
                                break;
                            }
                        case TabEnum.BilMis:
                            {
                                checkList.Add(CheckBoxEnum.Miscelleneous); ;
                                break;
                            }
                        //case TabEnum.BilTouCon:
                        //    {
                        //        checkList.Add(CheckBoxEnum.TOUConfig);
                        //        break;
                        //    }
                        case TabEnum.Tam:
                            {
                                checkList.Add(CheckBoxEnum.Tamper);
                                break;
                            }


                        //case TabEnum.Tra:
                        //    {
                        //        checkList.Add(CheckBoxEnum.Transactions);
                        //        break;
                        //    }
                        case TabEnum.Pha:
                            {
                                checkList.Add(CheckBoxEnum.Phasor);
                                break;
                            }
                        //case TabEnum.MidEne:
                        //    {
                        //        checkList.Add(CheckBoxEnum.MidNightEnergy);
                        //        break;
                        //    }
                        //case TabEnum.DaiEneCon:
                        //    {
                        //        checkList.Add(CheckBoxEnum.DailyEnergy);
                        //        break;
                        //    }
                        case TabEnum.LoaSur:
                            {
                                // checkList.Add(CheckBoxEnum.LoadSurvey);
                                break;
                            }

                        case TabEnum.MtrCfg:
                            {
                                checkList.Add(CheckBoxEnum.MeterConfig);
                                break;
                            }
                        case TabEnum.FraEne:
                            {
                                checkList.Add(CheckBoxEnum.FraudEnergy);
                                break;
                            }

                        case TabEnum.BillCumulativeMD:
                            {
                                checkList.Add(CheckBoxEnum.CumulativeMD);
                                break;
                            }
                        case TabEnum.BilTouAvgPowFac:
                            {
                                checkList.Add(CheckBoxEnum.TODPowerFactor);
                                break;
                            }
                    }

                }
            }

            if (checkList.Count > 0)
            {
                lngGridViewReadControl1.AddEnumList(checkList, true);
                lngGridViewReadControl1.DisableStatusColumn();
            }
            else
            {
                lngGridViewReadControl1.Visible = false;
                label1.Visible = false;
                lblNoData.Visible = true;
                gridShow.Visible = false;
            }



        }



        private DataTable RemoveEmptyColumnsFromDataTable(DataTable table)
        {
            DataTable dtResult = null;
            try
            {
                if (table != null && table.Rows.Count > 0)
                {
                    dtResult = table.Clone();
                    foreach (DataColumn column in table.Columns)
                    {
                        if (table.AsEnumerable().All(dr => dr.IsNull(column.ColumnName)) || table.AsEnumerable().All(dr => dr[column.ColumnName].Equals(string.Empty)))
                            dtResult.Columns.Remove(column.ColumnName);
                    }

                    foreach (DataRow dr in table.Rows)
                    {
                        DataRow drr = dtResult.NewRow();
                        foreach (DataColumn item in dtResult.Columns)
                        {
                            drr[item.ColumnName] = dr[item.ColumnName];
                        }
                        dtResult.Rows.Add(drr);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "RemoveEmptyColumnsFromDataTable(DataTable table)", ex);

            }
            return dtResult;
        }


        private DataTable GetFilterDataTable(List<string> lstColumnName, DataTable dtSource)
        {
            DataTable Selected = null;
            try
            {
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    Selected = new DataTable();
                    foreach (DataColumn itemclm in dtSource.Columns)
                    {
                        if (lstColumnName.Contains(itemclm.ColumnName))
                        {
                            Selected.Columns.Add(itemclm.ColumnName);
                        }
                    }

                    foreach (DataRow itemrow in dtSource.Rows)
                    {
                        DataRow dr = Selected.NewRow();
                        foreach (DataColumn itemclm in Selected.Columns)
                        {
                            dr[itemclm.ColumnName] = itemrow[itemclm.ColumnName];
                        }
                        Selected.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFilterDataTable(List<string> lstColumnName, DataTable dtSource)", ex);

            }
            return Selected;
        }

        private void SelectDialog_Load(object sender, EventArgs e)
        {

            //ShowCheckBox1();
            // Utility check for Daily Energy Consumption report added to solve bug 73548; 11th April 2012
            string utility = string.Empty;
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
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            types = ConfigInfo.GetApplicationType();

            ShowGridViewControl();
            //ShowCheckBox();
        }

        private string SplitWithOutUnit(string data)
        {
            string value = data;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                value = val[0] + " " + val[1];
            }
            return value;
        }

        private void chkBilling_CheckedChanged(object sender, EventArgs e)
        {
            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls;
            if (chkBilling.Checked == true)
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = true;
                }
            }
            else
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = false;
                }
            }
        }
        /// <summary>
        /// show button , for grid view , which displays the report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridShow_Click(object sender, EventArgs e)
        {
            generalReport = new MainReport();
            GetReportsToShow();
            List<Enum> CheckedBoxList = new List<Enum>();
            CheckedBoxList = lngGridViewReadControl1.GetSelectedProfilesList<Enum>(checkList);
            int errCount = 0;
            int showReport = 0;
            int selectedParams = 0;
            int historyIDCount = 0;
            //int meterModelNumber = 0;
            reportXSD = new FileReportDataSet();
            string errMsg = String.Empty;
            string fileName = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
                {
                    this.StatusMessage = "Please select a file.";
                    return;
                }
                else
                {
                    this.StatusMessage = "";
                }

                //if (ValidateForm() == false)
                //{
                //    this.StatusMessage = "Please select a parameter to view the  report.";
                //    return;
                //}

                if (CheckedBoxList.Count == 0)
                {
                    this.StatusMessage = "Please select a parameter to view the  report.";
                    return;
                }
                else
                {
                    this.StatusMessage = "";
                }
                // Added to get the filename.
                DataSet fileDataset = new DataSet();
                fileDataset = ListFileName(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (fileDataset != null)
                {
                    if (fileDataset.Tables[0].Rows.Count > 0)
                    {
                        fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                    }
                }
                Cursor.Current = Cursors.WaitCursor;

                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();

                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();//user story no 505185
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                {
                    //This function calling is required to add NoPower Supply and No Load Data column in the dataset
                    detailsDS = ListNoPowerSupplyLoadTime(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), detailsDS);
                    FillConsumerMeterDetails(detailsDS);
                }
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    {
                        //This function calling is required to add NoPower Supply and No Load Data column in the dataset
                        meterIDDS = ListNoPowerSupplyLoadTime(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), meterIDDS);
                        FillMeterID(meterIDDS);
                    }
                }

                /* GKG JVVNL Current TOU Read */
                //if (chkTouConfiguration.Checked == true && chkTouConfiguration.Visible == true)
                //if (CheckedBoxList.Contains(CheckBoxEnum.TOUConfig) == true)
                //{
                //    selectedParams++;
                //    DataSet touConfiguration = new DataSet();
                //    touConfiguration = new TOUBLL().DetailData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false);
                //    if (touConfiguration != null && touConfiguration.Tables[0].Rows.Count > 0)
                //    {
                //        Int32.TryParse(touConfiguration.Tables[0].Rows[0]["Season Number"].ToString(), out seasonNumber);
                //   FillTouConfigurationXSD(touConfiguration);
                //        showReport++;
                //    }
                //    else
                //    {
                //        errCount++;
                //        errMsg = "TOU configuration data not available.";
                //    }
                //}
                /* GKG JVVNL Current TOU Read */


                 if (CheckedBoxList.Contains(CheckBoxEnum.GeneralReport))
                {
                    selectedParams++;
                    DataSet generalDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        generalDS = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        string Voltrating = generalDS.Tables[0].Rows[10]["Value"].ToString();
                        string Currentrating = generalDS.Tables[0].Rows[11]["Value"].ToString();
                        reportRow["VoltageRating"] = Voltrating; // Add in main report TNEB
                        reportRow["CurrentRating"] = Currentrating; // Add in main report TNEB //user story no 505185

                        //Get Meter Constant from Nameplate Profile if Meter Constant is empty(----) comes form meter in General Profile   
                        DataSet dataSetNameplate = new DLMS650NamePlateBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        string GeneralMeterConstant = string.Empty;                                     
                        DataRow NameplateRow = null;
                        if (dataSetNameplate != null && dataSetNameplate.Tables != null && dataSetNameplate.Tables.Count > 0)
                        {
                            for (int count = 0; count < dataSetNameplate.Tables[0].Rows.Count; count++)
                            {
                                NameplateRow = dataSetNameplate.Tables[0].Rows[count];
                                if (NameplateRow["Descriptions"].ToString() == "Meter Constant")
                                {
                                    GeneralMeterConstant = NameplateRow["Value"].ToString();
                                }
                                else if (NameplateRow["OBIS Code"].ToString() == "0.0.96.128.17.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                                {
                                    int idx = generalDS.Tables[0].Rows.IndexOf(generalDS.Tables[0].Select().First(s => s["OBIS Code"].ToString().Contains("0.0.96.1.4.255")));
                                    generalDS.Tables[0].Rows.InsertAt(generalDS.Tables[0].NewRow(), idx + 1);
                                    generalDS.Tables[0].Rows[idx + 1].ItemArray = NameplateRow.ItemArray;
                                }
                                else if(NameplateRow["OBIS Code"].ToString() == "1.0.96.128.15.255" && !string.IsNullOrWhiteSpace(NameplateRow["Value"].ToString()))
                                {
                                    generalDS.Tables[0].ImportRow(NameplateRow);
                                }
                            }
                            generalDS.AcceptChanges();
                        }
                        DataRow generalRow = null;
                        if (generalDS != null && generalDS.Tables != null && generalDS.Tables.Count > 0)
                        {
                            for (int count = 0; count < generalDS.Tables[0].Rows.Count; count++)
                            {
                                generalRow = generalDS.Tables[0].Rows[count];
                                if (generalRow["Descriptions"].ToString() == "Meter Constant")
                                {
                                    if (generalRow["Value"].ToString() == "----")
                                    {
                                        generalRow["Value"] = GeneralMeterConstant;
                                    }
                                }
                            }
                        }
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        generalDS = ListGeneralData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        string Voltrating = generalDS.Tables[0].Rows[10]["Value"].ToString();
                        string Currentrating = generalDS.Tables[0].Rows[11]["Value"].ToString();
                        reportRow["VoltageRating"] = Voltrating; // Add in main report TNEB
                        reportRow["CurrentRating"] = Currentrating; // Add in main report TNEB //user story no 505185
                    }
                    if (generalDS != null && generalDS.Tables[0].Rows.Count > 0)
                    { FillGeneralXSD(generalDS); showReport++; }
                    else
                    {
                        errCount++; errMsg = "General data not available.";
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.InstantReport))
                {
                    selectedParams++;
                    DataSet instantDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        instantDS = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (instantDS != null && instantDS.Tables != null && instantDS.Tables.Count > 0 && instantDS.Tables[0].Rows.Count > 0)
                        {
                            string metertime = instantDS.Tables[0].Rows[0]["Value"].ToString();
                            if (metertime == "---------")
                                reportRow["MeterTime"] = "---------";
                            else
                            reportRow["MeterTime"] = metertime.Substring(11);
                        }

                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        instantDS = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (instantDS != null && instantDS.Tables[0].Rows.Count > 0)
                    {
                        string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(MeterDataID);
                        string chkValNumPowFail = ConfigSettings.GetValue("ChkNumPowFail");

                        #region WB and UGVCL Specific
                        if (chkValNumPowFail == "1")
                        {
                            DataRow[] rowPowFailCount = instantDS.Tables[0].Select("Descriptions = 'Number of Power-Failures'");
                            if (rowPowFailCount != null && rowPowFailCount.Length > 0)
                            {
                                instantDS.Tables[0].Rows.Remove(rowPowFailCount[0]);
                                instantDS.AcceptChanges();
                            }
                        }
                        #endregion

                        #region TNEB Model specific check to remove Reverse KWH
                        // [ReverseKWH_Remove]
                        int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);

                        if (meterModelNo.ToString() == "13")
                        {
                            DataRow[] reverseKWH = instantDS.Tables[0].Select("Descriptions = 'Reverse KWH'");

                            if (reverseKWH != null && reverseKWH.Length > 0)
                            {
                                instantDS.Tables[0].Rows.Remove(reverseKWH[0]);
                                instantDS.AcceptChanges();
                            }
                        }
                        #endregion

                        FillInstantXSD(instantDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "") errMsg = "Instantaneous data not available.";
                        else errMsg = errMsg + "\n" + "Instantaneous data not available.";
                    }

                    //Anomaly Report displayed through DataSet
                    // 1 => Flash ; 2 => EEPROM ; 3 => Power Supply ; 4 => RTC ; 8 => RTC Battery ; 9 => Main Battery
                    DataSet anomalyDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        anomalyDS = new AnomalyBLL().GetAnomalyDataForAnalysisDetail((Convert.ToInt32(ConfigInfo.ActiveMeterDataId)));
                    }
                    //**************if Anomaly not configure in meter then Self diagnostic data is not shown in report.user story 474452
                    if (anomalyDS != null && anomalyDS.Tables.Count> 0 && anomalyDS.Tables[0].Rows[1]["Status"].ToString() == "-" && anomalyDS.Tables[0].Rows[1]["Status"].ToString() == "-")
                    {
                        anomalyDS.Clear();
                    }
                    if (anomalyDS != null && anomalyDS.Tables.Count > 0 && anomalyDS.Tables[0].Rows.Count > 0)
                    {
                        //Remove metermodel 2 not required nvm and main battery status for puma meters.
                        //******* Meter Model Change Required Here ***********//
                        if (meterModelNumber == 9 || meterModelNumber == 8 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek || meterModelNumber == 16 || meterModelNumber == 23 || meterModelNumber == 11 || meterModelNumber == 17 || meterModelNumber == 18 || meterModelNumber == 32 || meterModelNumber == 13 || meterModelNumber == 24 || meterModelNumber == 25 || meterModelNumber == 33 || meterModelNumber == 34 || meterModelNumber == 35 || meterModelNumber == 36 || meterModelNumber == 37 || meterModelNumber == 43)//FOR THREE PH THREE SEASSION
                        {
                            DataTable anomalyWBAndSinglePhaseTable = anomalyDS.Tables[0].Copy();
                            anomalyDS.Clear();
                            if (anomalyWBAndSinglePhaseTable.Rows[0]["Status"].ToString() == "OK"
                                && anomalyWBAndSinglePhaseTable.Rows[1]["Status"].ToString() == "OK" &&
                                (meterModelNumber == 9 || meterModelNumber == 11 || meterModelNumber == 17 || meterModelNumber == 18 || meterModelNumber == 32 || meterModelNumber == 13 || meterModelNumber == 24 || meterModelNumber == 25 || meterModelNumber == 33 || meterModelNumber == 34 || meterModelNumber == 35 || meterModelNumber == 36 || meterModelNumber == 37 || meterModelNumber == 43))
                            {
                                anomalyDS.Tables[0].Rows.Add("NVM Status", "OK");
                            }
                            //******* Meter Model Change Required Here ***********//
                            else if (anomalyWBAndSinglePhaseTable.Rows[1]["Status"].ToString() == "OK" && (meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek && meterModelNumber != NamePlateConstants.ThreeTOUWCMValue && meterModelNumber != NamePlateConstants.SapphireWCM_St))//FOR THREE PH THREE TOU SEASSION
                            {
                                anomalyDS.Tables[0].Rows.Add("NVM Status", "OK");
                            }
                            else
                            {
                                anomalyDS.Tables[0].Rows.Add("NVM Status", "NOT OK");
                            }

                            if (meterModelNumber == 23) //Hardcoded for 1P Smart meter(Command is not implemented in meter)
                            {
                                if (anomalyDS.Tables[0].Rows.Count == 1)
                                    anomalyDS.Tables[0].Rows[0][1] = "OK";
                                else
                                    anomalyDS.Tables[0].Rows.Add("NVM Status", "OK");
                            }
                            ///for meterModelNumber 8 & 9
                            //******* Meter Model Change Required Here ***********//
                            //SarkarA code change start 20180122 // Meter Model 33 
                            if (meterModelNumber != 11 && meterModelNumber != 17 && meterModelNumber != 18 && meterModelNumber != 32 && meterModelNumber != 13 && meterModelNumber != 24 && meterModelNumber != 25 && meterModelNumber != 8 && meterModelNumber != 16 && meterModelNumber != 23 && meterModelNumber != NamePlateConstants.SFSP && meterModelNumber != NamePlateConstants.VFSPNoSeasonNoWeek && meterModelNumber != 33 && meterModelNumber != NamePlateConstants.SapphireS2)
                            //SarkarA code change end 20180122
                            {
                                anomalyDS.Tables[0].Rows.Add("RTC Status", anomalyWBAndSinglePhaseTable.Rows[3][1]);
                            }
                            if (meterModelNumber == 9)
                            {
                                anomalyDS.Tables[0].Rows.Add("Battery Status", "OK");
                            }
                            //******* Meter Model Change Required Here ***********//
                            else if (meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == 23 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek)
                            {
                                // anomalyDS.Tables[0].Rows.Add("RTC Status", anomalyWBAndSinglePhaseTable.Rows[4]["Status"]);// 
                                anomalyDS.Tables[0].Rows.Add("RTC Status", anomalyWBAndSinglePhaseTable.Rows[3]["Status"]);
                                if (ConfigInfo.ActiveFileType == "DLMS")
                                {
                                    if (meterModelNumber == 19 || meterModelNumber == 26) { }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("Battery Status", anomalyWBAndSinglePhaseTable.Rows[4]["Status"]);
                                    }
                                }

                                //anomalyDS.Tables[0].Rows.Add("RTC Battery Status", "OK");
                                //if (anomalyWBAndSinglePhaseTable.Rows[5]["Status"].ToString() == "OK")
                                //{
                                //    anomalyDS.Tables[0].Rows.Add("Main Battery Status", "OK");
                                //}
                                //else
                                //{
                                //    anomalyDS.Tables[0].Rows.Add("Main Battery Status", "NOT OK");
                                //}
                            }
                            //SarkarA code change start 20180122 // Meter Model 33 
                            else if (meterModelNumber == 11 || meterModelNumber == 17 || meterModelNumber == 18 || meterModelNumber == 32 || meterModelNumber == 13 || meterModelNumber == 24 || meterModelNumber == 25 || meterModelNumber == 33 || meterModelNumber == 34 || meterModelNumber == 35 || meterModelNumber == 36 || meterModelNumber == 37 || meterModelNumber == 43)/// added for sapphire . Only work for sapphire
                            //SarkarA code change end 20180122
                            {
                                if (anomalyWBAndSinglePhaseTable.Rows[2]["Status"].ToString() == "OK")
                                {
                                    anomalyDS.Tables[0].Rows.Add("Power Supply", "OK");
                                }
                                else
                                {
                                    anomalyDS.Tables[0].Rows.Add("POWER SUPPLY", "NOT OK");
                                }
                                if (meterModelNumber == 24 || meterModelNumber == 25 || meterModelNumber == 34 || meterModelNumber == 35 || meterModelNumber == 36 || meterModelNumber == 37)//For all Smart meter
                                {
                                    DataRow[] PowerSupply = anomalyDS.Tables[0].Select("Parameters = 'POWER SUPPLY'");
                                    anomalyDS.Tables[0].Rows.Remove(PowerSupply[0]);
                                    anomalyDS.AcceptChanges();
                                    if (anomalyWBAndSinglePhaseTable.Rows[3]["Status"].ToString() == "OK")
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Battery Status", "OK");
                                    }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Battery Status", "NOT OK");
                                    }
                                    if (anomalyWBAndSinglePhaseTable.Rows[5]["Status"].ToString() == "OK")
                                    {
                                        anomalyDS.Tables[0].Rows.Add("Main Battery Status", "OK");
                                    }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("Main Battery Status", "NOT OK");
                                    }
                                }
                                else
                                {

                                    if (anomalyWBAndSinglePhaseTable.Rows[3]["Status"].ToString() == "OK")
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Status", "OK");
                                    }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Status", "NOT OK");
                                    }

                                    if (anomalyWBAndSinglePhaseTable.Rows[4]["Status"].ToString() == "OK")
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Battery Status", "OK");
                                    }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("RTC Battery Status", "NOT OK");
                                    }


                                    if (anomalyWBAndSinglePhaseTable.Rows[5]["Status"].ToString() == "OK")
                                    {
                                        anomalyDS.Tables[0].Rows.Add("Main Battery Status", "OK");
                                    }
                                    else
                                    {
                                        anomalyDS.Tables[0].Rows.Add("Main Battery Status", "NOT OK");
                                    }


                                    if (anomalyWBAndSinglePhaseTable.Rows[6]["Status"].ToString() != "0" && meterModelNumber == 17)
                                    {
                                          anomalyDS.Tables[0].Rows.Add("Error Code Status", "ERROR 000" + anomalyWBAndSinglePhaseTable.Rows[6]["Status"].ToString());
                                    }                                   

                                }
                            }
                            if (meterModelNumber == 23) //Hardcoded for 1P Smart meter(Command is not implemented in meter)
                            {
                                if (anomalyDS.Tables[0].Rows.Count > 1)
                                    anomalyDS.Tables[0].Rows[1][1] = "OK";
                                else
                                    anomalyDS.Tables[0].Rows.Add("RTC Status", "OK");
                            }

                            if (meterModelNumber == 8) //For 1P IEC, FLASH is used for Error Code Display
                            {
                                if (anomalyWBAndSinglePhaseTable.Rows[0]["Status"].ToString() == "NOT OK")
                                {
                                    anomalyDS.Tables[0].Rows.Add("ERROR Code", "E005");
                                }
                            }
                            if (anomalyWBAndSinglePhaseTable.Rows[5]["Status"].ToString() == "/") // For CSPDCL
                            {
                                if (anomalyWBAndSinglePhaseTable.Rows[4]["Status"].ToString() == "OK")
                                {
                                    anomalyDS.Tables[0].Rows.Add("Meter Battery Status", "OK");
                                }
                                else
                                {
                                    anomalyDS.Tables[0].Rows.Add("Meter Battery Status", "NOT OK");
                                }
                            }
                            /// added for sapphire 
                        }
                        if (meterModelNumber != 11 && meterModelNumber != 17 && meterModelNumber != 18 && meterModelNumber != 32 && meterModelNumber != 13 && meterModelNumber != 24 && meterModelNumber != 25 && meterModelNumber != 34 && meterModelNumber != 35)
                        {
                            DataRow[] rowRTCBatteryStatus = anomalyDS.Tables[0].Select("Parameters = 'RTC Battery'");
                            if (rowRTCBatteryStatus != null && rowRTCBatteryStatus.Length > 0)
                            {
                                anomalyDS.Tables[0].Rows.Remove(rowRTCBatteryStatus[0]);
                                anomalyDS.AcceptChanges();
                            }
                        }

                        DataRow[] rowMainBatteryStatus = anomalyDS.Tables[0].Select("Parameters = 'MAIN BATTERY'");
                        if (rowMainBatteryStatus != null && rowMainBatteryStatus.Length > 0)
                        {
                            anomalyDS.Tables[0].Rows.Remove(rowMainBatteryStatus[0]);
                            anomalyDS.AcceptChanges();
                        }

                        //-----------Error code will be only for meter model 17
                        DataRow[] rowerrorStatus = anomalyDS.Tables[0].Select("Parameters = 'ErrorCodeStatus'");
                        if (rowerrorStatus != null && rowerrorStatus.Length > 0)
                        {

                            anomalyDS.Tables[0].Rows.Remove(rowerrorStatus[0]);
                            anomalyDS.AcceptChanges();
                        }


                        FillAnomalyXSD(anomalyDS);
                        showReport++;
                    }
                    // ********** Added for Vim Series2 meter ******
                    DataSet InstantDS = new DataSet();
                    DataSet GeneralDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        try
                        {
                            InstantDS = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                            GeneralDS = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                            DataRow[] Abccode = InstantDS.Tables[0].Select("Descriptions = 'ABC'");
                            DataRow[] MeterSerial = GeneralDS.Tables[0].Select("Descriptions = 'Meter Serial Number'");
                                                  
                         if (Abccode != null && Abccode.Length > 0 && MeterSerial != null && MeterSerial.Length > 0)
                         {
                          string abc = Abccode[0].ItemArray[4].ToString().Trim();
                          MeterSerialNo = MeterSerial[0].ItemArray[4].ToString().Trim();
                          DLMS650MeterDataList ReadData= new DLMS650MeterDataList ();
                          ReadData.ABCDecrypt(abc, MeterSerialNo);
                          FillABCXSD(ReadData.ABCReportDs, abc, MeterSerialNo);
                          showReport++;
                       }
                       }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "gridShow_Click(object sender, EventArgs e)", ex);
                        }
                       
                    }


                    ////Code block to bind anomaly xsd                    
                    //AnomalyEntity objAnomalyEntity = (AnomalyEntity)new AnomalyBLL().GetDetailData((Convert.ToInt32(ConfigInfo.ActiveMeterDataId)));
                    //if (objAnomalyEntity != null && objAnomalyEntity.AnomalyId > 0)
                    //{
                    //    FillAnomalyXSD(objAnomalyEntity);
                    //}
                }

                if (CheckedBoxList.Contains(CheckBoxEnum.PowerFactor))
                {
                    selectedParams++;
                    DataSet powerFactorDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        powerFactorDS = new DLMS650BillingBLL().GetAveragePowerFactor(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        powerFactorDS = ListPowerFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (powerFactorDS != null && powerFactorDS.Tables[0].Rows.Count > 0)
                    {
                        //This net varient check is removed because to support the 60 months billing and old report is having 12 static field is decleared in Report design
                        //if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO)
                        //{
                            powerFactorDS = new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), powerFactorDS, "Single");
                            if (powerFactorDS != null && powerFactorDS.Tables != null && powerFactorDS.Tables.Count > 0 && powerFactorDS.Tables[0] != null)
                            {
                                NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(powerFactorDS.Tables[0], "Power Factor Check List Selection", 13, true);
                                netReportCheckListPopUp.ShowDialog();
                                netReportCheckListPopUp.Dispose();
                                DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    FillMainEnergyXSD_NET(dtResult, "PowerFactorNET");
                                    SaveColumnList(dtResult, ref lstClmPowerFactorNET);
                                    showReport++;
                                }
                                else
                                {
                                    errCount++;
                                    if (errMsg == "") errMsg = "Power Factor data not available.";
                                    else errMsg = errMsg + "\n" + "Power Factor data not available.";
                                }
                            }
                            else
                            {
                                errCount++;
                                if (errMsg == "") errMsg = "Power Factor data not available.";
                                else errMsg = errMsg + "\n" + "Power Factor data not available.";
                            }
                        //}
                        //else
                        //{
                        //    //FillPowerFactorXSD(powerFactorDS, fileName);
                        //    FillPowerFactorXSD(new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), powerFactorDS, "Single"), fileName);
                        //    showReport++;
                        //}
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                        { errMsg = "Power Factor not available."; }
                        else
                        { errMsg = errMsg + "\n" + "Power Factor not available."; }

                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.TODPowerFactor))//story 1024441 Add TOD Export PF
                {

                    int tariffNumber = 1;
                    DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                    DataSet powerFactorDS = new DataSet();
                    DataSet AvgpowerFactorDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        powerFactorDS = new DLMS650BillingBLL().GetAveragePowerFactor(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        powerFactorDS= ShowBillingMonth(Convert.ToInt32(MeterDataID), powerFactorDS, "Single");
                    }
                    DataTable table = new DataTable();
                    DataRow row;

                    table.Columns.Add(new DataColumn("History", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
                    table.Columns.Add(new DataColumn("TOD Average PF(1.0.13.0.T.255)", typeof(System.String)));
                    table.Columns.Add(new DataColumn("TOD Average Export PF(1.0.84.0.T.255)", typeof(System.String)));

                    // DataRow dataRow = AvgpowerFactorDS.Tables[0].Rows[0];
                    int count = 0;
                    for (int counter = 0; counter <= powerFactorDS.Tables[0].Rows.Count - 1; counter++)
                    {
                        AvgpowerFactorDS = billingBLL.GetTODAvgPF(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), Convert.ToInt32(counter), false);

                        if (AvgpowerFactorDS != null)
                        {
                            //break; 
                        }
                        else
                        {
                            AvgpowerFactorDS = billingBLL.GetMeterData_PF(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), Convert.ToInt32(counter));
                        }
                        DataRow dataRow = AvgpowerFactorDS.Tables[0].Rows[0];
                        tariffNumber = 1;
                        row = table.NewRow();
                        if (count == 0)
                            row[0] = powerFactorDS.Tables[0].Rows[0].ItemArray[0].ToString();
                        else
                        {
                            row[0] =  powerFactorDS.Tables[0].Rows[1].ItemArray[0].ToString(); 
                        }
                        count++;
                        for (int counter1 = 1; counter1 < ConfigInfo.BillingTariffCount + 1; counter1++)
                        {
                            int count1 = 1;
                            if (counter == 0 && tariffNumber > 1)
                                row[0] = powerFactorDS.Tables[0].Rows[counter].ItemArray[0].ToString();
                            else
                            //if (counter == 1 && tariffNumber > 1)
                                row[0] = powerFactorDS.Tables[0].Rows[counter].ItemArray[0].ToString();

                            row[count1] = tariffNumber.ToString();
                            count1++;
                            row[count1] = AvgpowerFactorDS.Tables[0].Rows[counter1 - 1].ItemArray[1];
                            if (AvgpowerFactorDS.Tables[0].Columns.Contains("TOD Average PF Export(1.0.84.0.0.255;3;2)"))
                                row[count1 + 1] = AvgpowerFactorDS.Tables[0].Rows[counter1 - 1].ItemArray[2];

                            table.Rows.Add(row);
                            row = table.NewRow();
                            tariffNumber++;

                        }

                    }
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(table);
                    
                    if(AvgpowerFactorDS !=null && AvgpowerFactorDS.Tables !=null && AvgpowerFactorDS.Tables.Count > 0 && AvgpowerFactorDS.Tables[0] != null)
                    {
                        NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(dataSet.Tables[0], "Power Factor Check List Selection", 13, true);
                        netReportCheckListPopUp.ShowDialog();
                        netReportCheckListPopUp.Dispose();
                        DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            FillMainEnergyXSD_NET(dtResult, "TODAveragePF");
                            SaveColumnList_TODPF(dtResult, ref lstClmTODPowerFactorNET);
                            showReport++;
                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "TOD Power Factor data not available.";
                            else errMsg = errMsg + "\n" + "TOD Power Factor data not available.";
                        }
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "") errMsg = "TOD Power Factor data not available.";
                        else errMsg = errMsg + "\n" + "TOD Power Factor data not available.";
                    }

                }
                if (chkBillingTamperCounter.Checked == true)
                {
                    selectedParams++;
                    DataSet billingTamperCounterDS = new DataSet();
                    billingTamperCounterDS = ListBillingTamperCounterData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (billingTamperCounterDS != null && billingTamperCounterDS.Tables[0].Rows.Count > 0)
                    {
                        FillBillingTamperCounterXSD(billingTamperCounterDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Billing Tamper Counter not available.";
                        else errMsg = errMsg + "\n" + "Billing Tamper Counter not available.";
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.Tamper))
                {
                    selectedParams++;
                    DLMS650TamperMasterBLL dLMS650TamperMasterBLL = new DLMS650TamperMasterBLL();
                    DataSet occurrenceDset = new DataSet();
                    DataSet restorationDset = new DataSet();
                    DataSet tamperDetailsDset = new DataSet();
                    DataTable occTimeTable = new DataTable();
                    DataTable resTimeTable = new DataTable();
                    DataTable tamperDetailsDTable = new DataTable();
                    DataRow dr;
                    DataRow occTimeRow;
                    DataRow resTimeRow;
                    DataSet tamperCounterDS = new DataSet();
                    DataSet tamperDS = new DataSet();
                    string tamperCounter = string.Empty;
                    string occurranceTime = string.Empty;
                    string restorationTime = string.Empty;
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        tamperCounterDS = dLMS650TamperMasterBLL.ListAllTamperEventCode();
                        tamperDS = dLMS650TamperMasterBLL.AllDetailData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        // meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);

                        #region PSPCL specific check
                        DataRow tamperTypeRow;
                        string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
                        if (firmwareVersion == "7.01")
                        {
                            for (int count = 0; count < tamperCounterDS.Tables[0].Rows.Count; count++)
                            {
                                tamperTypeRow = tamperCounterDS.Tables[0].Rows[count];
                                if (tamperTypeRow != null && tamperTypeRow["TamperType"].ToString().Contains("Voltage Unbalance - Occurrence"))
                                {
                                    tamperTypeRow["TamperType"] = "Invalid Voltage - Occurrence";
                                }
                                if (tamperTypeRow != null && tamperTypeRow["TamperType"].ToString().Contains("Voltage Unbalance - Restoration"))
                                {
                                    tamperTypeRow["TamperType"] = "Invalid Voltage - Restoration";
                                }

                            }
                        }
                        #endregion
                    }
                    occTimeTable.Columns.Add("EventCode");
                    occTimeTable.Columns.Add("TamperDescription");
                    occTimeTable.Columns.Add("DateTimeEvent");

                    resTimeTable.Columns.Add("EventCode");
                    resTimeTable.Columns.Add("TamperDescription");
                    resTimeTable.Columns.Add("DateTimeEvent");

                    try
                    {

                        if (tamperDS != null && tamperDS.Tables[0].Rows.Count > 0)
                        {
                            showReport++;
                            for (int cIndex = 0; cIndex <= tamperDS.Tables[0].Columns.Count; cIndex++)
                            {
                                if (cIndex == 0)
                                    tamperDetailsDTable.Columns.Add(tamperDS.Tables[0].Columns[0].Caption);
                                else if (cIndex == 1)
                                    tamperDetailsDTable.Columns.Add("TamperDescription");
                                else
                                    tamperDetailsDTable.Columns.Add(tamperDS.Tables[0].Columns[cIndex - 1].Caption);
                            }

                            #region "3P DLMS LTCT 650 Tamper Snapshot"

                            if (types.Equals(ApplicationType.DLMS_LTCT_650) && ConfigInfo.ActiveMeterType != CAB.Framework.MeterType.OnePhaseTwoWire)
                            {
                                foreach (DataRow drow in tamperDS.Tables[0].Rows)
                                {
                                    DataTable tempTable = new DataTable();
                                    for (int cIndex = 0; cIndex <= tamperDS.Tables[0].Columns.Count; cIndex++)
                                    {
                                        if (cIndex == 0)
                                            tempTable.Columns.Add(tamperDS.Tables[0].Columns[0].Caption);
                                        else if (cIndex == 1)
                                            tempTable.Columns.Add("TamperDescription");
                                        else
                                            tempTable.Columns.Add(tamperDS.Tables[0].Columns[cIndex - 1].Caption);
                                    }
                                    foreach (DataRow row in tamperCounterDS.Tables[0].Rows)
                                    {
                                        if (drow["EventCode"].ToString() == row["TamperTypeID"].ToString())
                                        {

                                            if ((drow["EventCode"].ToString() != "101") && (drow["EventCode"].ToString() != "102"))
                                            {

                                                dr = tempTable.NewRow();
                                                dr[0] = drow["EventCode"].ToString();
                                                dr[1] = row[1].ToString();
                                                dr[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                dr[3] = SplitWithOutUnit(drow["CurrentIR"].ToString());
                                                dr[4] = SplitWithOutUnit(drow["CurrentIY"].ToString());
                                                dr[5] = SplitWithOutUnit(drow["CurrentIB"].ToString());
                                                dr[7] = SplitWithOutUnit(drow["VoltageVRN"].ToString());
                                                dr[8] = SplitWithOutUnit(drow["VoltageVYN"].ToString());
                                                dr[9] = SplitWithOutUnit(drow["VoltageVBN"].ToString());
                                                dr[11] = SplitWithOutUnit(drow["PowerFactorRphase"].ToString());
                                                dr[12] = SplitWithOutUnit(drow["PowerFactorYphase"].ToString());
                                                dr[13] = SplitWithOutUnit(drow["PowerFactorBphase"].ToString());
                                               // dr[14] = SplitWithOutUnit(drow["Temprature"].ToString());
                                                dr[14] = SplitWithOutUnit(drow["TotalPowerFactor"].ToString());
                                                dr[15] = SplitWithOutUnit(drow["CumulativeEnergykWh"].ToString());
                                                dr[16] = SplitWithOutUnit(drow["CumulativeEnergykVAh"].ToString());

                                                //SarkarA code change start 20170118 // neutral current
                                                dr[17] = SplitWithOutUnit(drow["NeutralCurrent"].ToString());
                                                dr[18] = SplitWithOutUnit(drow["ByPassCurrent"].ToString());
                                                dr[19] = SplitWithOutUnit(drow["CumulativeEnergykvarhLag"].ToString());
                                                dr[20] = SplitWithOutUnit(drow["CumulativeEnergykvarhLead"].ToString());
                                                dr[21] = SplitWithOutUnit(drow["CumulativeEnergykWhImport"].ToString());
                                                dr[22] = SplitWithOutUnit(drow["CumulativeEnergykWhExport"].ToString());
                                                dr[23] = SplitWithOutUnit(drow["CumulativeEnergykVAhImport"].ToString());
                                                dr[24] = SplitWithOutUnit(drow["CumulativeEnergykVAhExport"].ToString());
                                                dr[25] = SplitWithOutUnit(drow["HighNeutralCurrent"].ToString());//add pradipta_neu
                                                //SarkarA code change end 20170118

                                                //SarkarA code change start 20180110 // add new tamper parameters
                                                dr[26] = SplitWithOutUnit(drow["kWr"].ToString());
                                                dr[27] = SplitWithOutUnit(drow["kWy"].ToString());
                                                dr[28] = SplitWithOutUnit(drow["kWb"].ToString());
                                                dr[29] = SplitWithOutUnit(drow["kVAr"].ToString());
                                                dr[30] = SplitWithOutUnit(drow["kVAy"].ToString());
                                                dr[31] = SplitWithOutUnit(drow["kVAb"].ToString());
                                                dr[32] = SplitWithOutUnit(drow["CumulativeTamperCount"].ToString());//smart meter
                                                dr[33] = SplitWithOutUnit(drow["ActiveCurrentR"].ToString());
                                                dr[34] = SplitWithOutUnit(drow["ActiveCurrentY"].ToString());
                                                dr[35] = SplitWithOutUnit(drow["ActiveCurrentB"].ToString());
                                                //SarkarA code change end 20180110 

                                                //SarkarA code change start 20180330 // add phase current instant, frequency
                                                dr[36] = SplitWithOutUnit(drow["Frequency"].ToString());
                                                dr[37] = SplitWithOutUnit(drow["PhaseCurrentInstant"].ToString());
                                                //SarkarA code change end 20180330
                                                dr[38] = SplitWithOutUnit(drow["Temprature"].ToString());
                                                dr[39] = SplitWithOutUnit(drow["THDVR"].ToString());
                                                dr[40] = SplitWithOutUnit(drow["THDVY"].ToString());
                                                dr[41] = SplitWithOutUnit(drow["THDVB"].ToString());
                                                dr[42] = SplitWithOutUnit(drow["THDIR"].ToString());
                                                dr[43] = SplitWithOutUnit(drow["THDIY"].ToString());
                                                dr[44] = SplitWithOutUnit(drow["THDIB"].ToString());
                                               

                                                tempTable.Rows.Add(dr);
                                                break;
                                            }
                                            else // User Story no 474866. Power Failure tamper snapshot parameters dynamic visibility depend upon snapshot parameter coming from meter or not
                                            {
                                                if ((Convert.ToString(drow["CurrentIR"]) == "----") && (Convert.ToString(drow["VoltageVRN"]) == "----") && (Convert.ToString(drow["CumulativeEnergykWh"]) == "----"))
                                                {
                                                    if (drow["EventCode"].ToString() == "101")
                                                    {
                                                        occTimeRow = occTimeTable.NewRow();
                                                        occTimeRow[0] = drow["EventCode"].ToString();
                                                        occTimeRow[1] = row[1].ToString();
                                                        occTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                        occTimeTable.Rows.Add(occTimeRow);
                                                        break;
                                                    }
                                                    else if (drow["EventCode"].ToString() == "102")
                                                    {
                                                        resTimeRow = resTimeTable.NewRow();
                                                        resTimeRow[0] = drow["EventCode"].ToString();
                                                        resTimeRow[1] = row[1].ToString();
                                                        resTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                        resTimeTable.Rows.Add(resTimeRow);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    dr = tempTable.NewRow();
                                                    dr[0] = drow["EventCode"].ToString();
                                                    dr[1] = row[1].ToString();
                                                    dr[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                    dr[3] = SplitWithOutUnit(drow["CurrentIR"].ToString());
                                                    dr[4] = SplitWithOutUnit(drow["CurrentIY"].ToString());
                                                    dr[5] = SplitWithOutUnit(drow["CurrentIB"].ToString());
                                                    dr[7] = SplitWithOutUnit(drow["VoltageVRN"].ToString());
                                                    dr[8] = SplitWithOutUnit(drow["VoltageVYN"].ToString());
                                                    dr[9] = SplitWithOutUnit(drow["VoltageVBN"].ToString());
                                                    dr[11] = SplitWithOutUnit(drow["PowerFactorRphase"].ToString());
                                                    dr[12] = SplitWithOutUnit(drow["PowerFactorYphase"].ToString());
                                                    dr[13] = SplitWithOutUnit(drow["PowerFactorBphase"].ToString());
                                                    dr[14] = SplitWithOutUnit(drow["TotalPowerFactor"].ToString());
                                                    dr[15] = SplitWithOutUnit(drow["CumulativeEnergykWh"].ToString());
                                                    dr[16] = SplitWithOutUnit(drow["CumulativeEnergykVAh"].ToString());

                                                    tempTable.Rows.Add(dr);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    foreach (DataRow dtRow in tempTable.Rows)
                                    {
                                        //for HTCT
                                        //if (dtRow[1].ToString() == "Meter Cover Opening - Occurrence")
                                        //    continue;
                                        tamperDetailsDTable.ImportRow(dtRow);
                                    }

                                }
                            }
                            #endregion

                            #region "1P DLMS/IEC Tamper Snapshot"
                            else if (ConfigInfo.ActiveMeterType == CAB.Framework.MeterType.OnePhaseTwoWire)
                            {
                                foreach (DataRow drow in tamperDS.Tables[0].Rows)
                                {
                                          //Byte[] validColumnNumber = { 2,6, 10, 14, 15, 16, 17, 20, 21, 22, 23, 24,31, 36,37}; // Story - 349654 - Neutral Current in Tamper  //SarkarA code change start 20180330 // add phase current instant, frequency/end
                                    Byte[] validColumnNumber = { 2, 6, 10, 14, 15, 16, 17, 21, 22, 23, 24,25, 32, 37, 38 };

                                    DataTable tempTable = new DataTable();
                                    for (int cIndex = 0; cIndex <= tamperDS.Tables[0].Columns.Count; cIndex++)
                                    {
                                        if (cIndex == 0)
                                            tempTable.Columns.Add(tamperDS.Tables[0].Columns[0].Caption);
                                        else if (cIndex == 1)
                                            tempTable.Columns.Add("TamperDescription");
                                        for (int validColumnIndex = 0; validColumnIndex < validColumnNumber.Length; validColumnIndex++)
                                        {
                                            if (validColumnNumber[validColumnIndex] == cIndex)
                                            {
                                                tempTable.Columns.Add(tamperDS.Tables[0].Columns[cIndex - 1].Caption);
                                            }
                                        }

                                    }
                                    foreach (DataRow row in tamperCounterDS.Tables[0].Rows)
                                    {
                                        if (drow["EventCode"].ToString() == row["TamperTypeID"].ToString())
                                        {
                                            if ((drow["EventCode"].ToString() != "101") && (drow["EventCode"].ToString() != "102"))
                                            {
                                                dr = tempTable.NewRow();

                                                dr[0] = drow["EventCode"].ToString();
                                                dr[1] = row[1].ToString();
                                                dr[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                dr[3] = SplitWithOutUnit(drow["PhaseCurrent"].ToString());
                                                dr[4] = SplitWithOutUnit(drow["PhaseVoltage"].ToString());
                                                //dr[5] = SplitWithOutUnit(drow["Temprature"].ToString());
                                                dr[5] = SplitWithOutUnit(drow["TotalPowerFactor"].ToString());
                                                dr[6] = SplitWithOutUnit(drow["CumulativeEnergykWh"].ToString());
                                                dr[7] = SplitWithOutUnit(drow["CumulativeEnergykVAh"].ToString());
                                                dr[8] = SplitWithOutUnit(drow["NeutralCurrent"].ToString());
                                                // Story - 349654 - Neutral Current in Tamper
                                               //SarkarA code change start 20180110 // Restore 1P Tamper Report
                                                //dr[9] = SplitWithOutUnit(drow["HighNeutralCurrent"].ToString()); //pradipta_neu
                                                                                            
                                                dr[9] = SplitWithOutUnit(drow["CumulativeEnergykWhImport"].ToString());
                                                dr[10] = SplitWithOutUnit(drow["CumulativeEnergykWhExport"].ToString());
                                                dr[11] = SplitWithOutUnit(drow["CumulativeEnergykVAhImport"].ToString());
                                                dr[12] = SplitWithOutUnit(drow["CumulativeEnergykVAhExport"].ToString());


                                                 dr[13] = SplitWithOutUnit(drow["HighNeutralCurrent"].ToString());
                                                
                                                //SarkarA code change end 20180110 
                                                dr[14] = SplitWithOutUnit(drow["CumulativeTamperCount"].ToString());//for smart meter
                                                dr[15] = SplitWithOutUnit(drow["PhaseCurrentInstant"].ToString());//SarkarA code change start 20180330 // add phase current instant, frequency/end
                                                dr[16] = SplitWithOutUnit(drow["Temprature"].ToString());    //SarkarA code change start 20180330 // add phase current instant, frequency/end
                                              
                                                tempTable.Rows.Add(dr);
                                                break;
                                            }
                                            else if (drow["EventCode"].ToString() == "101")
                                            {
                                                occTimeRow = occTimeTable.NewRow();
                                                occTimeRow[0] = drow["EventCode"].ToString();
                                                occTimeRow[1] = row[1].ToString();
                                                occTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                //Get Temper occurrence DateTime for WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                                                //occTimeRow[2] = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(drow[1].ToString()));
                                                occTimeTable.Rows.Add(occTimeRow);
                                                break;
                                            }
                                            else if (drow["EventCode"].ToString() == "102")
                                            {
                                                resTimeRow = resTimeTable.NewRow();
                                                resTimeRow[0] = drow["EventCode"].ToString();
                                                resTimeRow[1] = row[1].ToString();
                                                resTimeRow[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1].ToString()));
                                                resTimeTable.Rows.Add(resTimeRow);
                                                break;
                                            }
                                        }
                                    }
                                    foreach (DataRow dtRow in tempTable.Rows)
                                    {
                                        //for HTCT
                                        //if (dtRow[1].ToString() == "Meter Cover Opening - Occurrence")
                                        //    continue;
                                        tamperDetailsDTable.ImportRow(dtRow);
                                    }
                                }
                            }
                            #endregion

                            if (tamperDetailsDTable != null && tamperDetailsDTable.Rows.Count > 0)
                            {
                                tamperDetailsDset.Tables.Add(tamperDetailsDTable);
                            }

                            FillDLMSTamperXSD(tamperDetailsDset);

                            if (occTimeTable != null && occTimeTable.Rows.Count > 0)
                                occurrenceDset.Tables.Add(occTimeTable);
                            if (resTimeTable != null && resTimeTable.Rows.Count > 0)
                                restorationDset.Tables.Add(resTimeTable);


                            FillDLMSPowerFailureTamperXSD(occurrenceDset, restorationDset);

                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "Tamper data not available.";
                            else errMsg = errMsg + "\n" + "Tamper data not available.";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "gridShow_Click(object sender, EventArgs e)", ex);
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.MainEnergy))
                {
                    selectedParams++;
                    headerMainEnergyBillingResetType = string.Empty;
                    DataSet mainEnergyDS = new DataSet();
                    int mainEnegeryOldReportColumnMaxCount = 7;
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        //if (meterModelNumber == 9)
                        //{
                        //    mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false, meterModelNumber);// Story - 365971 - 13 billing for Power ON Hours
                        //}
                        //else
                        //{
                            mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false);// Story - 365971 - 13 billing for Power ON Hours
                        //}
                        //mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId),false);// Story - 365971 - 13 billing for Power ON Hours
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO || (meterVariant == CAB.Framework.MeterVariant.ONE && meterModelNumber == NamePlateConstants.SapphireLTCT && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount) || (meterVariant == CAB.Framework.MeterVariant.ONE && meterModelNumber == NamePlateConstants.SapphireLTCT_st && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_WCM && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount|| meterModelNumber == NamePlateConstants.SmartM_Cipher_1PH && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount ) || (meterModelNumber == NamePlateConstants.Sapphire_Netmeter_LTCT && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount) || (meterModelNumber == NamePlateConstants.Sapphire_Netmeter_LTCT && mainEnergyDS.Tables[0].Columns.Count > mainEnegeryOldReportColumnMaxCount))
                    {
                        mainEnergyDS = new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), mainEnergyDS, "Single");
                        if (mainEnergyDS != null && mainEnergyDS.Tables != null && mainEnergyDS.Tables.Count > 0 && mainEnergyDS.Tables[0] != null)
                        { 
                            NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(mainEnergyDS.Tables[0], "Main Energy  Check List Selection", 13, true);
                            netReportCheckListPopUp.ShowDialog();
                            netReportCheckListPopUp.Dispose();
                            //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                            //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                            DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();

                            //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                            dlms650CommonBLL.GetReactive(dtResult, "billing");

                            if (dtResult != null && dtResult.Rows.Count > 0)
                            {
                                FillMainEnergyXSD_NET(dtResult, "EnergyTableNET");
                                SaveColumnList(dtResult, ref lstClmMainEnergyNET);
                                showReport++;
                            }
                            else
                            {
                                errCount++;
                                if (errMsg == "") errMsg = "Main Energy data not available.";
                                else errMsg = errMsg + "\n" + "Main Energy data not available.";

                            }
                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "Main Energy data not available.";
                            else errMsg = errMsg + "\n" + "Main Energy data not available.";
                        }
                    }
                    else
                    {
                        // This check is added for 1P meter, below parameters should not be visible in report coulumn of Main Energy.
                        if ((meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek || meterModelNumber == NamePlateConstants.SFSP) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                        {
                            if (mainEnergyDS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyBILLINGTYPE)))
                            {
                                mainEnergyDS.Tables[0].Columns.Remove(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyBILLINGTYPE));
                            }

                            if (mainEnergyDS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG)))
                            {
                                mainEnergyDS.Tables[0].Columns.Remove(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG));
                            }

                            if (mainEnergyDS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD)))
                            {
                                mainEnergyDS.Tables[0].Columns.Remove(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD));
                            }
                            mainEnergyDS.Tables[0].AcceptChanges();

                        } 
                        if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                        {
                            FillMainEnergyXSD(new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), mainEnergyDS, "Single"));
                            showReport++;
                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "Main Energy data not available.";
                            else errMsg = errMsg + "\n" + "Main Energy data not available.";
                        }
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.MainEnergyCons))
                {
                    selectedParams++;
                    DataSet mainEnergyDS = new DataSet();
                    int enegeryConsumptionOldReportColumnMaxCount = 5;
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergyCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }


                    if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO || (meterVariant == CAB.Framework.MeterVariant.ONE && meterModelNumber == NamePlateConstants.SapphireLTCT && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount) || (meterVariant == CAB.Framework.MeterVariant.ONE && meterModelNumber == NamePlateConstants.SapphireLTCT_st && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_WCM && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount || meterModelNumber == NamePlateConstants.SmartM_Cipher_1PH && mainEnergyDS.Tables[0].Columns.Count > enegeryConsumptionOldReportColumnMaxCount))
                    {
                        mainEnergyDS = new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), mainEnergyDS, "Double");
                        if (mainEnergyDS != null && mainEnergyDS.Tables != null && mainEnergyDS.Tables.Count > 0 && mainEnergyDS.Tables[0] != null)
                        {
                            NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(mainEnergyDS.Tables[0], "Main Energy Consumption Check List Selection", 13, true);
                            netReportCheckListPopUp.ShowDialog();
                            netReportCheckListPopUp.Dispose();
                            //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                            //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                            DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();

                            //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                            dlms650CommonBLL.GetReactive(dtResult, "billing");
                            
                            if (dtResult != null && dtResult.Rows.Count > 0)
                            {
                                FillMainEnergyXSD_NET(dtResult, "EnergyConsumptionTableNET");
                                SaveColumnList(dtResult, ref lstClmMainEnergyConsumptionNET);
                                showReport++;
                            }
                            else
                            {
                                errCount++;
                                if (errMsg == "") errMsg = "Main Energy Consumption data not available.";
                                else errMsg = errMsg + "\n" + "Main Energy Consumption data not available.";
                            }
                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") errMsg = "Main Energy Consumption data not available.";
                            else errMsg = errMsg + "\n" + "Main Energy Consumption data not available.";
                        }
                    }
                    else
                    {
                        if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                        {
                            FillEnergyConsumptionXSD(new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), mainEnergyDS, "Double")); showReport++;
                        }
                        else
                        {
                            errCount++;
                            if (errMsg == "") { errMsg = "Main Energy Consumption data not available."; } else { errMsg = errMsg + "\n" + "Main Energy Consumption data not available."; }

                        }
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.Demand))
                {
                    selectedParams++;
                    DataSet maximumDemandDS = new DataSet();

                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        maximumDemandDS = new DLMS650BillingBLL().GetMaximumDemand(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        maximumDemandDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    }
                    if (maximumDemandDS != null && maximumDemandDS.Tables != null && maximumDemandDS.Tables.Count > 0 && maximumDemandDS.Tables[0] != null && maximumDemandDS.Tables[0].Rows.Count > 0)
                    {
                        DataSet DsResult = new DLMS650MeterDataList().ShowBillingMonth(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), maximumDemandDS, "Single");
                        if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO)
                        {
                            NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(DsResult.Tables[0], "Maximum Demand Check List Selection", 13, true);
                            netReportCheckListPopUp.ShowDialog();
                            netReportCheckListPopUp.Dispose();
                            //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                            //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                            DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                            if (dtResult != null && dtResult.Rows.Count > 0)
                            {
                                FillTariffEnergyXSD_NET(dtResult, 0, "MaximumDemandTableNET");
                                SaveColumnList(dtResult, ref lstClmMaxDemandNET);
                                showReport++;
                            }
                            else
                            {
                                errCount++;
                                if (errMsg == "")
                                    errMsg = "Demand data not available.";
                                else errMsg = errMsg + "\n" + "Demand data not available.";
                            }
                        }
                        else
                        {
                            FillMaximumDemandXSD(DsResult);
                            showReport++;
                        }
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Demand data not available.";
                        else errMsg = errMsg + "\n" + "Demand data not available.";
                    }

                    DataSet TODMDDS = new DataSet();
                    // Added to solve tod demand report issue in case of fastdownloading.
                    int counter = 0;
                    if (fileName.Contains(FILEEXTENSION))
                    {
                        counter = 1;
                    }
                    //for (int historyID = counter; historyID <= 12; historyID++)
                    // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
                    for (int historyID = counter; historyID <= 60; historyID++)
                    {
                        if (types.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            TODMDDS = new DLMS650BillingBLL().GetTODMDMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, true);
                        }
                        if (TODMDDS != null && TODMDDS.Tables != null && TODMDDS.Tables.Count > 0 && TODMDDS.Tables[0].Rows.Count > 0)
                        {
                            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO)
                            {
                                if (historyID == counter)
                                {
                                    NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(TODMDDS.Tables[0], "TOD Demand Check List Selection", 13, true);
                                    netReportCheckListPopUp.ShowDialog();
                                    netReportCheckListPopUp.Dispose();
                                    //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                                    //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                                    DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                                    if (dtResult != null)
                                    {
                                        SaveColumnList(dtResult, ref lstClmTODDemandNET);
                                    }
                                    else
                                    {

                                        errCount++;
                                        if (errMsg == "")
                                            errMsg = "TOD Demand data not available.";
                                        else
                                            errMsg = errMsg + "\n" + "TOD Demand data not available.";
                                        break;
                                    }
                                }
                                if (lstClmTODDemandNET != null && lstClmTODDemandNET.Count > 0)
                                {
                                    DataTable dtResult = GetFilterDataTable(lstClmTODDemandNET, TODMDDS.Tables[0]);
                                    FillTariffEnergyXSD_NET(dtResult, historyID, "TODDemandTableNET");
                                    showReport++;
                                }
                            }
                            else
                            {
                                FillTODMDXSD(TODMDDS, historyID); showReport++;
                            }
                        }
                        else
                        {
                            //VBM - For FD readout files , as FD billing does not have Tcurrent billing.
                            if (historyID > 0)
                            {
                                break;
                            }
                        }
                    }
                }
                if (CheckedBoxList.Contains(CheckBoxEnum.CumulativeMD) == true)// for smart meter
                {
                    DataSet cumulatMDData = new DataSet();

                    cumulatMDData = new DLMS650BillingBLL().GetCumulativeMaximumDemand(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (cumulatMDData.FirstTableHasRows())
                    {
                        FillCumulativeMDXSD(cumulatMDData);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        errMsg = errMsg + Symbols.NEWLINE + MISCDATANOTAVAILABLE;

                    }

                }


                if (CheckedBoxList.Contains(CheckBoxEnum.TODEnergy))
                {
                    historyIDCount = 0;
                    selectedParams++;
                    DataSet tariffEnergyDS = new DataSet();

                    //for (int historyID = 0; historyID <= 12; historyID++)
                    // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
                    for (int historyID = 0; historyID <= 60; historyID++)
                    {
                        if (types.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            DLMS650BillingBLL objDLMS650BillingBLL = new DLMS650BillingBLL();
                            tariffEnergyDS = objDLMS650BillingBLL.GetMeterDataForRPTWithMonth(objDLMS650BillingBLL.GetMeterDataForRPT(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), Convert.ToInt32(historyID)));
                        }
                        
                        if (tariffEnergyDS != null && tariffEnergyDS.Tables != null && tariffEnergyDS.Tables.Count > 0 && tariffEnergyDS.Tables[0].Rows.Count > 0)
                        {
                            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO)
                            {
                                if (historyID == 0)
                                {
                                    NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(tariffEnergyDS.Tables[0], "TOD Energy Check List Selection", 13, true);
                                    netReportCheckListPopUp.ShowDialog();
                                    netReportCheckListPopUp.Dispose();
                                    //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                                    //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                                    DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                                    if (dtResult != null)
                                    {
                                        SaveColumnList(dtResult, ref lstClmTODEnergyNET);
                                    }
                                    else
                                    {

                                        errCount++;
                                        if (errMsg == "")
                                            errMsg = "TOD Energy data not available.";
                                        else
                                            errMsg = errMsg + "\n" + "TOD Energy data not available.";
                                        break;
                                    }
                                }
                                if (lstClmTODEnergyNET != null && lstClmTODEnergyNET.Count > 0)
                                {
                                    DataTable dtResult = GetFilterDataTable(lstClmTODEnergyNET, tariffEnergyDS.Tables[0]);
                                    FillTariffEnergyXSD_NET(dtResult, historyID, "TODEnergyTableNET");
                                    showReport++;
                                    historyIDCount++;
                                }
                            }
                            else
                            {
                                if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                                {                       
                                    tariffEnergyDS = ReplaceEmptyStringWithDash(tariffEnergyDS);
                                }
                                FillTariffEnergyXSD(tariffEnergyDS, historyID);
                                showReport++;
                                historyIDCount++;
                            }

                        }
                    }
                    //changed on 22/02/2012
                    if (historyIDCount == 0 && tariffEnergyDS == null)
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "TOD Energy data not available.";
                        else
                            errMsg = errMsg + "\n" + "TOD Energy data not available.";
                    }
                }

                if (CheckedBoxList.Contains(CheckBoxEnum.TODConsumption))
                {
                    historyIDCount = 0;
                    selectedParams++;
                    DataSet currentTariffEnergyDS = new DataSet();
                    DataSet nextTariffEnergyDS = new DataSet();
                    // Added to the check the extension of file. In case of fastdownloading no current data is present.
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.Contains("2NG"))
                        {
                            for (int historyID = 0; historyID <= 12; historyID++)
                            {
                                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                                {

                                    if (historyID == 0)
                                    {
                                        currentTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        historyIDCount++;
                                    }
                                    else
                                    {
                                        currentTariffEnergyDS = nextTariffEnergyDS;
                                        historyIDCount++;
                                    }
                                    nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID + 1, false);

                                    if (nextTariffEnergyDS == null)
                                    {
                                        nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        if (nextTariffEnergyDS == null) break;
                                    }
                                }
                                if (currentTariffEnergyDS != null && nextTariffEnergyDS != null && currentTariffEnergyDS.Tables.Count > 0 && nextTariffEnergyDS.Tables.Count > 0 && currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
                                {
                                    if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR || meterVariant == CAB.Framework.MeterVariant.TWO)
                                    {
                                        if (historyID == 0)
                                        {
                                            NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(currentTariffEnergyDS.Tables[0], "TOD Energy Consumption Check List Selection", 13, true);
                                            netReportCheckListPopUp.ShowDialog();
                                            netReportCheckListPopUp.Dispose();
                                            //DataTable dtFinal = netReportCheckListPopUp.GetSelectedDataTable();
                                            //DataTable dtResult = RemoveEmptyColumnsFromDataTable(dtFinal);
                                            DataTable dtResult = netReportCheckListPopUp.GetSelectedDataTable();
                                            if (dtResult != null)
                                            {
                                                SaveColumnList(dtResult, ref lstClmTODConsumptionEnergyNET);
                                            }
                                            else
                                            {
                                                errCount++;
                                                if (errMsg == "")
                                                    errMsg = "TOD Energy Consumption not available.";
                                                else
                                                    errMsg = errMsg + "\n" + "TOD Energy Consumption not available.";
                                                break;
                                            }
                                        }
                                        if (lstClmTODConsumptionEnergyNET != null && lstClmTODConsumptionEnergyNET.Count > 0)
                                        {
                                            DataTable dtResult = GetFilterDataTable(lstClmTODConsumptionEnergyNET, currentTariffEnergyDS.Tables[0]);
                                            FillTariffEnergyXSD_NET(dtResult, historyID, "TODEnergyConsumptionTableNET");
                                            showReport++;
                                            historyIDCount++;
                                        }
                                    }
                                    else
                                    {

                                        FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
                                        showReport++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //for (int historyID = 0; historyID <= 12; historyID++)
                            // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
                            for (int historyID = 0; historyID <= 60; historyID++)
                            {
                                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                                {
                                    currentTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                    if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                                    currentTariffEnergyDS = ReplaceEmptyStringWithDash(currentTariffEnergyDS);
                                    if (currentTariffEnergyDS == null)
                                        break;
                                    
                                    historyIDCount++;

                                    nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID + 1, false);
                                    if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                                    nextTariffEnergyDS = ReplaceEmptyStringWithDash(nextTariffEnergyDS);

                                    if (nextTariffEnergyDS == null)
                                    {
                                        nextTariffEnergyDS = new DLMS650BillingBLL().GetTODConsumption(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), historyID, false);
                                        if (meterModelNumber == 8 && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                                        nextTariffEnergyDS = ReplaceEmptyStringWithDash(nextTariffEnergyDS);
                                        if (nextTariffEnergyDS == null) break;
                                    }

                                }
                                if (currentTariffEnergyDS != null && nextTariffEnergyDS != null && currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
                                {
                                    FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
                                    showReport++;
                                }
                            }
                        }
                    }

                    if (nextTariffEnergyDS == null && currentTariffEnergyDS == null || historyIDCount == 0)
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "TOD Energy Consumption data not available.";
                        else
                            errMsg = errMsg + "\n" + "TOD Energy Consumption data not available.";
                    }
                }
                #region "Phasor Report"
                if (CheckedBoxList.Contains(CheckBoxEnum.Phasor))
                {
                    selectedParams++;

                    PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                    phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                    phasorDiagramForm.ShowDialog();


                    if (phasorDiagramForm.PhasorDataAvailable)
                    {

                        DataSet dsPhasor = new DLMS650PhasorBLL().GetPhasorDataSet(Convert.ToInt32(ConfigInfo.ActiveMeterDataId)) as DataSet;
                        dlms650CommonBLL.ApplyMultiplyFactor(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), dsPhasor, true);
                        FillPhasorTable();
                        FillPhasorXSD(dsPhasor);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Phasor data not available.";
                        else
                            errMsg = errMsg + "\n" + "Phasor data not available.";
                    }
                }
                #endregion
                if (CheckedBoxList.Contains(CheckBoxEnum.LoadSurvey))
                {
                    selectedParams++;
                    string type = "Energy";
                    if (SMD_rbtnLoadSurveyDemand.Checked)
                        type = "Demand";
                    long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                    DataSet loadSurveyDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        loadSurveyDS = new DLMS650LoadSurveyBLL().ListDataSetColumnWise(id, new DLMS650LoadSurveyBLL().GetFromDate(id), new DLMS650LoadSurveyBLL().GetToDate(id), type, true);
                    }
                    else if (types.Equals(ApplicationType.IEC_LTCT_650))
                    {
                        loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                    }
                    if (loadSurveyDS != null && loadSurveyDS.Tables[0].Rows.Count > 0)
                    { FillLoadSurveyXSD(loadSurveyDS); showReport++; }
                    else
                    {
                        errCount++;
                        if (errMsg == "") { errMsg = "Load Survey data not available."; }
                        else { errMsg = errMsg + "\n" + "Load Survey data not available."; }
                    }
                }

                if (chkBillingMechanism.Checked == true)
                {
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                    {
                        FillBillingMechanismXSD(ctRatioDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Billing Mechanism data not available.";
                        else
                            errMsg = errMsg + "\n" + "Billing Mechanism data not available.";
                    }
                }

                if (CheckedBoxList.Contains(CheckBoxEnum.Transactions))
                {
                    selectedParams++;
                    DataSet transactionDS = new DataSet();
                    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                    {
                        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
                        transactionDS = tamperBLL.DetailTransactionData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        //transactionDS = new DLMS650CommonBLL().Transaction(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (transactionDS != null && transactionDS.Tables.Count > 0)
                        {
                            foreach (DataColumn dataColumnTransaction in transactionDS.Tables[0].Columns)
                            {
                                foreach (DataRow dataRowTransaction in transactionDS.Tables[0].Rows)
                                {
                                    if (dataRowTransaction[dataColumnTransaction].ToString() == "")
                                    {
                                        dataRowTransaction[dataColumnTransaction] = dateUnavailable;
                                    }
                                }
                            }
                            DataView view = new DataView(transactionDS.Tables[0]);
                            StringBuilder filterCondition = new StringBuilder();
                            if (!UtilityDetails.ShowMDResetTamper)
                            {
                                filterCondition.Append("EventCode =" + MDReset);
                            }
                            if (UtilityDetails.DisableProgrammingBillingDateTime)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + SingleActionScheduleForBillingDates);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + SingleActionScheduleForBillingDates);
                                }
                            }

                            if (UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + ProfileCapturePeriod);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + ProfileCapturePeriod);
                                }
                            }
                            if (UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append(" OR EventCode =" + DemandIntegrationPeriod);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + DemandIntegrationPeriod);
                                }
                            }
                            if (!UtilityDetails.ShowDisplayParemeterTamper)
                            {
                                if (filterCondition.Length > 0)
                                {
                                    filterCondition.Append("OR EventCode =" + ScrollTimeConfig + " OR EventCode =" + ScrollModeConfig
                                                      + " OR EventCode =" + PushModeConfig + " OR EventCode =" + HRModeConfig);
                                }
                                else
                                {
                                    filterCondition.Append("EventCode =" + ScrollTimeConfig + " OR EventCode =" + ScrollModeConfig
                                                      + " OR EventCode =" + PushModeConfig + " OR EventCode =" + HRModeConfig);
                                }

                            }
                            if (filterCondition.Length > 0)
                            {
                                view.RowFilter = filterCondition.ToString();
                                //delete rows
                                foreach (DataRowView row in view)
                                {
                                    row.Delete();
                                }
                            }
                            transactionDS.AcceptChanges();

                        }
                    }
                    if (transactionDS != null && transactionDS.Tables[0].Rows.Count > 0)
                    {
                        FillTransactionXSD(transactionDS); showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Transactions data not available.";
                        else
                            errMsg = errMsg + "\n" + "Transactions data not available.";
                    }
                }

                if (chkCTRatio.Checked == true)
                {
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                    {
                        FillCTRatioXSD(ctRatioDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "CT Ratio not available.";
                        else errMsg = errMsg + "\n" + "CT Ratio not available.";
                    }
                }

                if (chkLoadFactor.Checked == true)
                {
                    DataSet loadFactorDS = new DataSet();
                    loadFactorDS = ListLoadFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (loadFactorDS != null && loadFactorDS.Tables[0].Rows.Count > 0)
                    {
                        FillLoadFactorXSD(loadFactorDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Load Factor not available.";
                        else errMsg = errMsg + "\n" + "Load Factor not available.";
                    }
                }

                if (chkPowerOnHours.Checked == true)
                {
                    DataSet powerOnHoursDS = new DataSet();
                    powerOnHoursDS = ListPowerOnHoursData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (powerOnHoursDS != null && powerOnHoursDS.Tables[0].Rows.Count > 0)
                    {
                        FillPowerOnHoursXSD(powerOnHoursDS);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Power On Hours not available.";
                        else errMsg = errMsg + "\n" + "Power On Hours not available.";
                    }
                }


                if (CheckedBoxList.Contains(CheckBoxEnum.PowerOnOffDuration) == true)
                {
                    DataSet powerOnOffDuration = new DataSet();
                    powerOnOffDuration = new DLMS650BillingBLL().GetPowerOnDuration(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    powerOnOffDuration = ShowBillingMonth(Convert.ToInt32(MeterDataID), powerOnOffDuration, "Single");
                    if ((meterModelNumber == 8 || meterModelNumber == 16 || meterModelNumber == NamePlateConstants.SFSP || meterModelNumber == NamePlateConstants.VFSPNoSeasonNoWeek) && ConfigInfo.ActiveFileType == BCSConstants.IEC)
                    {
                        if (powerOnOffDuration != null)
                        {
                            powerOnOffDuration.Tables[0].Rows[0][2] = "----";
                            powerOnOffDuration.Tables[0].Rows[0][3] = "----";
                        }
                    }
                    if (powerOnOffDuration != null && powerOnOffDuration.Tables[0].Rows.Count > 0)
                    {
                        FillPowerOnOffDurationXSD(powerOnOffDuration);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Power On Off duration data not available.";
                        else errMsg = errMsg + "\n" + "Power On Off duration data not available.";
                    }
                }

                if (CheckedBoxList.Contains(CheckBoxEnum.LoadFactor) == true)
                {
                    DataSet loadFactor = new DataSet();
                    headerLoadFactor = string.Empty;
                    DataSet Met_Cat = new DataSet();
                    DLMS650BillingBLL billingBLL = new DLMS650BillingBLL(); ;
                    Met_Cat = billingBLL.GetMeterCategory(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                    foreach (DataRow dataRow in Met_Cat.Tables[0].Rows)
                    {
                        meter_cat = Convert.ToString(dataRow["Category"]);
                    }
                    if (meter_cat == "B8" || meter_cat == "B2")
                    {
                        loadFactor = new DLMS650BillingBLL().GetBillingAverageLoadFactorCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        loadFactor = ShowBillingMonth(Convert.ToInt32(MeterDataID), loadFactor, "Single");
                        FillLoadFactorDTXSD(loadFactor);
                        showReport++;
                    }
                    else
                    {

                        if ((ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS" || ConfigInfo.ActiveFileType.ToUpper() == "DLMS") && ConfigInfo.ActiveMeterType.ToUpper() == "1P-2W" && loadFactor == null)
                        {
                            loadFactor = new DLMS650BillingBLL().GetBillingAverageLoadFactorCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                            loadFactor = ShowBillingMonth(Convert.ToInt32(MeterDataID), loadFactor, "Single");
                            FillLoadFactorDTXSD(loadFactor);
                            showReport++;
                        }


                        loadFactor = new DLMS650BillingBLL().GetBillingAverageLoadFactor(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));

                        if (loadFactor != null && loadFactor.Tables[0].Rows.Count > 0)
                        {
                            FillLoadFactorDTXSD(loadFactor);
                            showReport++;
                        }
                       
                        else if ((ConfigInfo.ActiveFileType.ToUpper() == "NONDLMS" || ConfigInfo.ActiveFileType.ToUpper() == "DLMS"))
                            {

                                loadFactor = new DLMS650BillingBLL().GetBillingAverageLoadFactorCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                                loadFactor = ShowBillingMonth(Convert.ToInt32(MeterDataID), loadFactor, "Single");
                                FillLoadFactorDTXSD(loadFactor);
                                showReport++;
                            }               
                     
                        
                        else
                        {
                            errCount++;
                            if (errMsg == "")
                                errMsg = "Load Factor data not available.";
                            else errMsg = errMsg + "\n" + "Load Factor data not available.";
                        }
                    }
                }

                #region "Average Load"
                if (CheckedBoxList.Contains(CheckBoxEnum.AverageLoad) == true)
                {
                    DataSet averageLoad = new DataSet();
                    headerAverageLoad = string.Empty;
                    averageLoad = new DLMS650BillingBLL().GetBillingAverageLoad(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));

                    if (averageLoad != null && averageLoad.Tables[0].Rows.Count > 0)
                    {
                        FillAverageLoadDTXSD(averageLoad);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        if (errMsg == "")
                            errMsg = "Average Load data not available.";
                        else errMsg = errMsg + "\n" + "Average Load data not available.";
                    }
                }
                #endregion


                // if (chkPowerOffDuration.Checked && chkPowerOffDuration.Visible)
                //if (CheckedBoxList.Contains(CheckBoxEnum.PowerOffDuration) == true)
                //{
                //    DataSet powerOffDuration = new DataSet();
                //    powerOffDuration = new DLMS650BillingBLL().GetPowerOffDuration(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                //    if (powerOffDuration != null && powerOffDuration.Tables[0].Rows.Count > 0)
                //    {
                //        FillPowerOffDurationXSD(powerOffDuration);
                //        showReport++;
                //    }
                //    else
                //    {
                //        errCount++;
                //        if (errMsg == "")
                //            errMsg = "Power Off duration data not available.";
                //        else errMsg = errMsg + "\n" + "Power Off duration data not available.";
                //    }
                //}
                //if (isMPKWCL)
                //{
                    //  if (chkMiscelleneous.Checked)
                if (CheckedBoxList.Contains(CheckBoxEnum.Miscelleneous) == true)
                {
                    DataSet miscelleneousData = new DataSet();

                    miscelleneousData = new DLMS650BillingBLL().GetMiscellaneous(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (miscelleneousData.FirstTableHasRows())
                    {
                        FillMiscelleneous(miscelleneousData);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        errMsg = errMsg + Symbols.NEWLINE + MISCDATANOTAVAILABLE;

                    }

                }
                //}

                //selectedParams++;
                ////    this.Visible = false;
                //PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                //phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                //phasorDiagramForm.ShowDialog();
                ////    this.Visible = true;

                //if (phasorDiagramForm.PhasorDataAvailable)
                //{
                //    FillPhasorTable();
                //    showReport++;
                //}
                //else
                //{
                //    errCount++;
                //    if (errMsg == "")
                //        errMsg = "Phasor data is not available.";
                //    else errMsg = errMsg + "\n" + "Phasor data is not available.";

                //}


                //  if (chkDailyEnergyConsumption.Checked == true)
                //if (CheckedBoxList.Contains(CheckBoxEnum.DailyEnergy) == true)
                //{
                //    //DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
                //    //long lsFromDateMD = loadSurveyBLL.GetFromDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    //long lsToDateMD = loadSurveyBLL.GetToDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                //    DLMS650LoadSurveyBLL dlmsLoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //    selectedParams++;
                //    DataSet dailyEnergyConsumpDS = new DataSet();
                //    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //    {
                //        dailyEnergyConsumpDS = dlmsLoadSurveyBLL.GetPUMADailyConsumption(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    }
                //    if (dailyEnergyConsumpDS != null && dailyEnergyConsumpDS.Tables[0].Rows.Count > 0)
                //    {
                //        FillDailyEnergyConsumpXSD(dailyEnergyConsumpDS); showReport++;
                //    }
                //    else
                //    {
                //        errCount++;
                //        if (errMsg == "")
                //        {
                //            errMsg = (isPUMA) ? "Mignight Consumption data not available." : "Daily Energy Consumption data not available.";

                //        }
                //        else
                //        {
                //            string msg = (isPUMA) ? "Mignight Consumption data not available." : "Daily Energy Consumption data not available.";
                //            errMsg = errMsg + "\n" + msg;
                //        }
                //    }
                //}

                ////added for MVVNL
                ////  if (chkMidnightEnergy.Checked == true)
                //if (CheckedBoxList.Contains(CheckBoxEnum.MidNightEnergy) == true)
                //{
                //    selectedParams++;
                //    DataSet midnightEnergyDS = new DataSet();
                //    DLMS650CommonBLL common = new DLMS650CommonBLL();
                //    if (isPUMA)
                //    {
                //        midnightEnergyDS = new DLMS650LoadSurveyBLL().GetPUMAMidNightData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    }
                //    else if (UtilityDetails.ShowMidnight)
                //    {
                //        midnightEnergyDS = ListMidnightEnergies(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //        // Added to solve bug 89706.
                //        midnightEnergyDS = common.ConvertMidnightEnergy(midnightEnergyDS);
                //        // midnightEnergyDS = common.ApplyMultiplyFactor(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), midnightEnergyDS);
                //    }
                //    if (midnightEnergyDS != null && midnightEnergyDS.Tables.Count > 0 && midnightEnergyDS.Tables[0].Rows.Count > 0)
                //    {
                //        FillMidnightEnergyXSD(midnightEnergyDS);
                //        showReport++;
                //    }
                //    else
                //    {
                //        errCount++;
                //        if (errMsg == "")
                //            errMsg = "Midnight energies not available.";
                //        else errMsg = errMsg + "\n" + "Midnight energies not available.";
                //    }
                //}

                if (CheckedBoxList.Contains(CheckBoxEnum.FraudEnergy))
                {
                    FraudEnergyEntity entity = ListFraudEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as FraudEnergyEntity;
                    if (entity != null)
                    {
                        FillFraudEnergyXSD(entity);
                        showReport++;
                    }

                }

                if (CheckedBoxList.Contains(CheckBoxEnum.MeterConfig))
                {
                    string rtc = null;
                    BillingTypeEntity billingTypeEntity = null;
                    int dip = 0;
                    string disconnect = null;
                    string Loadcontrol = null;
                    string Loadcontrol1P = null;

                    int sip = 0;
                    bool isTodDataFound = false;


                    if (!listOfprofilesWithNoData.Contains(TabEnum.RTC))
                    {
                        rtc = ListRTCData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.DIP))
                    {
                        dip = ListDIPData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                        // Add in main report TNEB //user story no 505185
                        if (dip == 0x00000384 || dip == 0x00001384)
                        {
                            reportRow["DIP"] = "15 Min";
                        }
                        else if (dip == 0x00000708 || dip == 0x00001708 || dip == 0x00002708)
                        {
                            reportRow["DIP"] = "30 Min";
                        }
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.LSIP))
                    {
                        sip = ListSIPData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.BillTyp))
                    {
                        billingTypeEntity = ListBillingTypeData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        int MeterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
                        if (MeterModelNumber == NamePlateConstants.RubyE150Value || MeterModelNumber == NamePlateConstants.SFSP)
                        {
                            if (billingTypeEntity.BillingType == "02")
                            {
                                reportRow["AutoResetDate"] = "Monthly";
                            }
                            else if (billingTypeEntity.BillingType == "01")
                            {
                                reportRow["AutoResetDate"] = "Odd Month";
                            }
                            else if (billingTypeEntity.BillingType == "00")
                            {
                                reportRow["AutoResetDate"] = "Even Month";
                            }
                        }
                        else
                        {

                            if (billingTypeEntity.BillingType == "00")
                            {
                                reportRow["AutoResetDate"] = "Monthly";
                            }
                            else if (billingTypeEntity.BillingType == "01")
                            {
                                reportRow["AutoResetDate"] = "Odd Month";
                            }
                            else if (billingTypeEntity.BillingType == "02")
                            {
                                reportRow["AutoResetDate"] = "Even Month";
                            }
                        }
                        // reportRow["AutoResetDate"] = billingTypeEntity.BillingPeriod; // Add in main report TNEB //user story no 505185
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.TOD))
                    {
                        isTodDataFound = DisplayTODData();
                        if (isTodDataFound)
                            showReport++;

                    }
                    if (!listOfprofilesWithNoData.Contains(TabEnum.DisCon))
                    {
                        disconnect = ListDiscconectData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillDisconnectControlXSD(disconnect);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.LoaCon))
                    {
                        Loadcontrol = ListLoadControl(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillLoadControlXSD(Loadcontrol);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.LoaCon1P))
                    {
                        Loadcontrol1P = ListLoadControl(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillLoadControl1PhaseXSD(Loadcontrol1P);
                        showReport++;
                    }


                    if (!listOfprofilesWithNoData.Contains(TabEnum.RS485))
                    {
                        string Rs485 = ListRS485Control(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        RS485XSD(Rs485);
                        showReport++;
                    }

                    if (rtc != null || billingTypeEntity != null || sip != 0 || dip != 0)
                    {
                        if (meterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || meterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || meterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || meterModelNumber == NamePlateConstants.SmartM_Cipher_WCM)
                            FillDIPSmartmeterXSD(rtc, dip, sip, billingTypeEntity);
                        else
                        FillOtherConfigurationsXSD(rtc, dip, sip, billingTypeEntity);
                    }

                    if (rtc == null && billingTypeEntity == null && sip == 0 && dip == 0 && isTodDataFound == false)
                    {
                        errMsg = "Meter Configuration Data Not Found.";
                        errCount++;
                    }
                    if (!listOfprofilesWithNoData.Contains(TabEnum.PaymentMode))
                    {
                        string paymentload = ListPaymentData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillPaymentmodeXSD(paymentload);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.MeteringMode))
                    {
                        string Meteringload = ListMeteringModeData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillMeteringModeXSD(Meteringload);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.LoadLimit))
                    {
                        string loadlimitload = ListLoadLimitData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillLoadLimitXSD(loadlimitload);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.SlidingDemand))
                    {
                        string slidingdemandload = ListSlidingDemandData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillSlidingdemandXSD(slidingdemandload);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.OpticalRJPortLock))
                    {
                        string Optiload = ListOpticalLockData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillOpticalPortXSD(Optiload);
                        string RJload = ListRJLockData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillRJPortXSD(RJload);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.PulseEnergy))
                    {
                        string pulseEnergyData = ListPulseEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        FillPulseEnergyXSD(pulseEnergyData);
                        showReport++;
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.KvarSel))
                    {
                        MeterConfigurationsNFEntity KvahSelEntity = ListKvahData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as MeterConfigurationsNFEntity;
                        if (KvahSelEntity != null)
                        {
                            FillKvahSelectionXSD(KvahSelEntity);
                            showReport++;
                        }
                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.AutoLck))
                    {
                        AutoLockEntity AutoLock1Entity = ListAutoLockData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as AutoLockEntity;
                        if (AutoLock1Entity != null)
                        {
                            FillAutoLockXSD(AutoLock1Entity);
                            showReport++;
                        }
                    }
                    if (!listOfprofilesWithNoData.Contains(TabEnum.SofBil))
                    {
                        SoftwareBillingEntity SoftwareBillingEntity = ListSoftwareBillData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as SoftwareBillingEntity;
                        if (SoftwareBillingEntity != null)
                        {
                            FillSoftwareBillingXSD(SoftwareBillingEntity);
                            showReport++;
                        }

                    }

                    if (!listOfprofilesWithNoData.Contains(TabEnum.ManualMDReset))
                    {
                        ManualMDResetEntity ManualMDResetEntity = ListManualMDResetData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as ManualMDResetEntity;
                        if (ManualMDResetEntity != null)
                        {
                            FillManualMDResetXSD(ManualMDResetEntity);
                            showReport++;
                        }

                    }

                }
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow); //user story no 505185
                showReport++;

                Application.DoEvents();
                Application.DoEvents();
                Application.DoEvents();

                if (errCount == 0)
                    ShowReport();
                else
                {
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    {
                        MessageBox.Show(errMsg, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (showReport > 0)
                        ShowReport();
                }

            }

            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "gridShow_Click(object sender, EventArgs e)", ex);
            }
        }


        private void FillTariffEnergyXSD_NET(DataTable dtResult, int historyID, string TableName)
        {
            try
            {
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResult.Rows)
                    {
                        DataRow repRow = reportXSD.Tables[TableName].NewRow();
                        for (int colCount = 0; colCount < dtResult.Columns.Count; colCount++)
                        {
                            repRow[colCount] = row[colCount].ToString();
                        }
                        reportXSD.Tables[TableName].Rows.Add(repRow);
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillTariffEnergyXSD_NET(DataTable dtResult, int historyID, string TableName)", ex);
            }

        }

        private void SaveColumnList(DataTable dtResult,ref List<string> lstClmNET)
        {
            try
            {
                lstClmNET = new List<string>();
                foreach (DataColumn itemclm in dtResult.Columns)
                {
                                     
                   lstClmNET.Add(itemclm.ColumnName);
                     
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SaveColumnList(DataTable dtResult,ref List<string> lstClmNET)", ex);

            }
        }
        private void SaveColumnList_TODPF(DataTable dtResult, ref List<string> lstClmNET)
        {
            try
            {
                lstClmNET = new List<string>();
                foreach (DataColumn itemclm in dtResult.Columns)
                {
                    if (itemclm.ColumnName == "History") { }
                    else
                        lstClmNET.Add(itemclm.ColumnName);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SaveColumnList_TODPF(DataTable dtResult,ref List<string> lstClmNET)", ex);

            }
        }
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
                                logger.Log(LOGLEVELS.Error, "ShowBillingMonth(int meterDataId, DataSet billingData, string mode)", ex);
                            }
                        }
                    }
                    else if (mode == "Double") //double used for the paired history value for history column
                    {
                        string currentMonth = string.Empty;
                        string previousMonth = string.Empty;
                        DataSet dataSet = new DLMS650BillingBLL().GetBillingMonths(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
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
                                if (billingData.Tables[0].Rows[i][0].ToString().Contains("History - 13")) // Story - To set 12th billing consumption history
                                    billingData.Tables[0].Rows[i][0] = "History - 12 - Initial";
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                finalDS.Tables[0].Rows[i][0] = billingData.Tables[0].Rows[i][0].ToString() + " (---)";
                                logger.Log(LOGLEVELS.Error, "ShowBillingMonth(int meterDataId, DataSet billingData, string mode)", ex);
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
         //private void FillLoadFactorDTXSD(DataSet loadFactor)

        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();
        //        DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();



        //      dataSet = billingBLL.GetBillingAverageLoadFactor(Convert.ToInt32(MeterDataID));

        //        if (loadFactor != null && loadFactor.Tables != null && loadFactor.Tables.Count > 0)
        //        {
        //            DataTable table = new DataTable();
        //            for (int i = 0; i < loadFactor.Tables[0].Columns.Count; i++)
        //            {

        //                string ClmName = loadFactor.Tables[0].Columns[i].ColumnName;
        //                foreach (DataRow dataRow in loadFactor.Tables[0].Rows)
        //                {
        //                    switch (ClmName)
        //                    {
        //                        case "Load Factor (%) (0.0.96.1.219.255;3;2)":
        //                            averageLoadFactor = Convert.ToString(dataRow["Load Factor (%) (0.0.96.1.219.255;3;2)"]);
        //                            break;
        //                        case "Import Load Factor (1.0.1.8.0.255;3;2)":
        //                            averageImportLoadFactor = Convert.ToString(dataRow["Import Load Factor (1.0.1.8.0.255;3;2)"]);
        //                            break;
        //                        case "Export Load Factor (1.0.2.8.0.255;3;2)":
        //                            averageExportLoadFactor = Convert.ToString(dataRow["Export Load Factor (1.0.2.8.0.255;3;2)"]);
        //                            break;
        //                        case "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)":
        //                            averagekWImportLoadFactor = Convert.ToString(dataRow["kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"]);
        //                            break;
        //                        case "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)":
        //                            averagekWExportLoadFactor = Convert.ToString(dataRow["kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"]);
        //                            break;
        //                        case "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)":
        //                            averagekVAImportLoadFactor = Convert.ToString(dataRow["kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"]);
        //                            break;
        //                        case "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)":
        //                            averagekVAImportLoadFactor = Convert.ToString(dataRow["kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"]);
        //                            break;
        //                    }
        //                }

        //            }




        //        }
        //        if (averagekWImportLoadFactor != string.Empty && averagekWExportLoadFactor != string.Empty && averagekVAImportLoadFactor != string.Empty && averagekVAImportLoadFactor != string.Empty)
        //        {
        //            if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
        //            {
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("LoadFactor");
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("ImportLoadFactor");
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("ExportLoadFactor");
        //            }
        //        }
        //        if (averageLoadFactor != string.Empty)
        //        {

        //            if (averageLoadFactor != "---------")
        //            {
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("ImportLoadFactor");
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("ExportLoadFactor");

        //               // reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWImportLoadFactor");
        //               // reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWExportLoadFactor");
        //                //reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAImportLoadFactor");
        //               // reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAExportLoadFactor");
        //            }
        //        }
        //        if (averageImportLoadFactor != string.Empty && averageExportLoadFactor != string.Empty)
        //        {

        //            if (averageImportLoadFactor != "---------" && averageExportLoadFactor != "---------")
        //            {
        //                reportXSD.Tables["LoadFactorDT"].Columns.Remove("LoadFactor");

        //                //reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWImportLoadFactor");
        //               // reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWExportLoadFactor");
        //                //reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAImportLoadFactor");
        //                //reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAExportLoadFactor");
        //            }
        //        }


        //        DataRow reportRow = null;
        //        if (loadFactor != null && loadFactor.Tables[0] != null && loadFactor.Tables[0].Columns != null && loadFactor.Tables[0].Rows != null && loadFactor.Tables[0].Rows.Count > 0)
        //        {  
        //            for (int j = 0; j < loadFactor.Tables[0].Rows.Count; j++)
        //            {
        //                reportRow = reportXSD.Tables["LoadFactorDT"].NewRow();
        //                for (int i = 0; i < loadFactor.Tables[0].Columns.Count; i++)
        //                {
        //                    reportRow[i] = loadFactor.Tables[0].Rows[j][i];
        //                    string ClmName = loadFactor.Tables[0].Columns[i].ColumnName;

        //                    switch (ClmName)
        //                    {
        //                        case "History":
        //                            headerLoadFactor = ClmName;
        //                            break;
        //                        case "Billing DateTime":
        //                            headerLoadFactor = ClmName;
        //                            break;
        //                        case "Load Factor (0.0.96.1.219.255;3;2)":
        //                            headerLoadFactor = ClmName;
        //                            break;
        //                        case "Load Factor (%) (0.0.96.1.219.255;3;2)":
        //                            headerLoadFactor = ClmName;
        //                            break;

        //                        case "Import Load Factor (1.0.1.8.0.255;3;2)":
        //                            headerLoadFactor = ClmName;
        //                            break;
        //                        case "Export Load Factor (1.0.2.8.0.255;3;2)":
        //                            headerLoadFactor1 = ClmName;
        //                            break;
        //                        //pradipta_start_081018

        //                        case "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)":
        //                            headerkWImportLoadFactor = ClmName;
        //                            break;
        //                        case "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)":
        //                            headerkWExportLoadFactor = ClmName;
        //                            break;
        //                        case "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)":
        //                            headerkVAImportLoadFactor = ClmName;
        //                            break;
        //                        case "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)":
        //                            headerkVAExportLoadFactor = ClmName;
        //                            break;
        //                        //pradipta_End_081018

        //                    }
        //                }


        //                reportXSD.Tables["LoadFactorDT"].Rows.Add(reportRow);
        //            }
        //        }
        //    }
        //    catch (Exception ex)    //Exception log for catch block
        //    {
        //        logger.Log(LOGLEVELS.Error, "FillLoadFactorDTXSD(DataSet loadFactor)", ex);

        //    }
        //}
        private void FillLoadFactorDTXSD(DataSet loadFactor)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
                dataSet = billingBLL.GetBillingAverageLoadFactor(Convert.ToInt32(MeterDataID));

                if (loadFactor != null && loadFactor.Tables != null && loadFactor.Tables.Count > 0)
                {
                    DataTable table = new DataTable();
                    for (int i = 0; i < loadFactor.Tables[0].Columns.Count; i++)
                    {

                        string ClmName = loadFactor.Tables[0].Columns[i].ColumnName;
                        foreach (DataRow dataRow in loadFactor.Tables[0].Rows)
                        {
                            switch (ClmName)
                            {
                                case "Load Factor (%) (0.0.96.1.219.255;3;2)":
                                    averageLoadFactor = Convert.ToString(dataRow["Load Factor (%) (0.0.96.1.219.255;3;2)"]);
                                    break;
                                case "Import Load Factor (1.0.1.8.0.255;3;2)":
                                    averageImportLoadFactor = Convert.ToString(dataRow["Import Load Factor (1.0.1.8.0.255;3;2)"]);
                                    break;
                                case "Export Load Factor (1.0.2.8.0.255;3;2)":
                                    averageExportLoadFactor = Convert.ToString(dataRow["Export Load Factor (1.0.2.8.0.255;3;2)"]);
                                    break;
                                case "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)":
                                    averagekWImportLoadFactor = Convert.ToString(dataRow["kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"]);
                                    break;
                                case "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)":
                                    averagekWExportLoadFactor = Convert.ToString(dataRow["kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"]);
                                    break;
                                case "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)":
                                    averagekVAImportLoadFactor = Convert.ToString(dataRow["kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"]);
                                    break;
                                case "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)":
                                    averagekVAImportLoadFactor = Convert.ToString(dataRow["kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"]);
                                    break;
                            }
                        }

                    }




                }
                if (averagekWImportLoadFactor != string.Empty && averagekWExportLoadFactor != string.Empty && averagekVAImportLoadFactor != string.Empty && averagekVAImportLoadFactor != string.Empty)
                {
                    if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
                    {
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("LoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ExportLoadFactor");
                    }
                }
                if (averageLoadFactor != string.Empty)
                {

                    if (averageLoadFactor != "---------")
                    {
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ExportLoadFactor");

                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWExportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAExportLoadFactor");
                    }
                }
                if (averageImportLoadFactor != string.Empty && averageExportLoadFactor != string.Empty)
                {

                    if (averageImportLoadFactor != "---------" && averageExportLoadFactor != "---------")
                    {
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("LoadFactor");

                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kWExportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("kVAExportLoadFactor");
                    }
                }


                if (averagekWImportLoadFactor != string.Empty && averagekWExportLoadFactor == string.Empty && averagekVAImportLoadFactor == string.Empty && averagekVAImportLoadFactor == string.Empty)
                {
                    if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
                    {
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("LoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ImportLoadFactor");
                        reportXSD.Tables["LoadFactorDT"].Columns.Remove("ExportLoadFactor");
                    }
                }

                // add pradipta
                if (loadFactor.Tables[0].Columns.Contains("kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"))
                {
                    if (loadFactor.Tables[0].Rows[0]["kW Import Load Factor (%) (1.0.1.0.128.255;3;2)"].ToString() == "")
                    {
                        loadFactor.Tables[0].Columns.Remove("kW Import Load Factor (%) (1.0.1.0.128.255;3;2)");
                    }

                }

                if (loadFactor.Tables[0].Columns.Contains("kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"))
                {
                    if (loadFactor.Tables[0].Rows[0]["kW Export Load Factor (%) (1.0.2.0.128.255;3;2)"].ToString() == "")
                    {
                        loadFactor.Tables[0].Columns.Remove("kW Export Load Factor (%) (1.0.2.0.128.255;3;2)");
                    }

                }

                if (loadFactor.Tables[0].Columns.Contains("kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"))
                {
                    if (loadFactor.Tables[0].Rows[0]["kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)"].ToString() == "")
                    {
                        loadFactor.Tables[0].Columns.Remove("kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)");
                    }

                }

                if (loadFactor.Tables[0].Columns.Contains("kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"))
                {
                    if (loadFactor.Tables[0].Rows[0]["kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)"].ToString() == "")
                    {
                        loadFactor.Tables[0].Columns.Remove("kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)");
                    }

                }
                loadFactor.Tables[0].AcceptChanges();
            

                //}

                //end pradipta
                DataRow reportRow = null;
                if (loadFactor != null && loadFactor.Tables[0] != null && loadFactor.Tables[0].Columns != null && loadFactor.Tables[0].Rows != null && loadFactor.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < loadFactor.Tables[0].Rows.Count; j++)
                    {
                        reportRow = reportXSD.Tables["LoadFactorDT"].NewRow();
                        for (int i = 0; i < loadFactor.Tables[0].Columns.Count; i++)
                        {
                            reportRow[i] = loadFactor.Tables[0].Rows[j][i];
                            string ClmName = loadFactor.Tables[0].Columns[i].ColumnName;
                            
                            switch (ClmName)
                            {
                                case "History":
                                    headerLoadFactor = ClmName;
                                    break;
                                case "Billing DateTime":
                                    headerLoadFactor = ClmName;
                                    break;
                                case "Load Factor (0.0.96.1.219.255;3;2)":
                                    headerLoadFactor = ClmName;
                                    break;
                                case "Load Factor (%) (0.0.96.1.219.255;3;2)":
                                    headerLoadFactor = ClmName;
                                    break;

                                case "Import Load Factor (1.0.1.8.0.255;3;2)":
                                    headerLoadFactor = ClmName;
                                    break;
                                case "Export Load Factor (1.0.2.8.0.255;3;2)":
                                    headerLoadFactor1 = ClmName;
                                    break;
                                //pradipta_start_081018

                                case "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)":
                                    headerkWImportLoadFactor = ClmName;
                                    break;
                                case "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)":
                                    headerkWExportLoadFactor = ClmName;
                                    break;
                                case "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)":
                                    headerkVAImportLoadFactor = ClmName;
                                    break;
                                case "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)":
                                    headerkVAExportLoadFactor = ClmName;
                                    break;
                                //pradipta_End_081018

                            }
                        }


                        reportXSD.Tables["LoadFactorDT"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillLoadFactorDTXSD(DataSet loadFactor)", ex);

            }
        }

        private void FillPowerOnOffDurationXSD(DataSet powerOnOffDuration)
        {
            try
            {
                DataRow reportRow = null;
                if (powerOnOffDuration != null && powerOnOffDuration.Tables[0] != null && powerOnOffDuration.Tables[0].Columns != null && powerOnOffDuration.Tables[0].Rows != null && powerOnOffDuration.Tables[0].Rows.Count > 0)
                {

                    for (int j = 0; j < powerOnOffDuration.Tables[0].Rows.Count; j++)
                    {
                        reportRow = reportXSD.Tables["PowerOnOffDuration"].NewRow();
                        for (int i = 0; i < powerOnOffDuration.Tables[0].Columns.Count; i++)
                        {
                            reportRow[i] = powerOnOffDuration.Tables[0].Rows[j][i];
                        }
                        reportXSD.Tables["PowerOnOffDuration"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillPowerOnOffDurationXSD(DataSet powerOnOffDuration)", ex);

            }
        }

        private void FillAverageLoadDTXSD(DataSet averageLoad)
        {
            try
            {
                DataRow reportRow = null;
                if (averageLoad != null && averageLoad.Tables[0] != null && averageLoad.Tables[0].Columns != null && averageLoad.Tables[0].Rows != null && averageLoad.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < averageLoad.Tables[0].Rows.Count; j++)
                    {
                        reportRow = reportXSD.Tables["AverageLoadDT"].NewRow();
                        for (int i = 0; i < averageLoad.Tables[0].Columns.Count; i++)
                        {
                            reportRow[i] = averageLoad.Tables[0].Rows[j][i];
                            string ClmName = averageLoad.Tables[0].Columns[i].ColumnName;
                            if (ClmName.Contains("Average Load"))
                            {
                                headerAverageLoad = ClmName;
                            }
                        }
                        reportXSD.Tables["AverageLoadDT"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillAverageLoadDTXSD(DataSet averageLoad)", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void GetReportsToShow()
        {
            try
            {
                DataSet tabNameData = new TabNameBLL().GetNoDataTabs(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (tabNameData != null && tabNameData.Tables != null && tabNameData.Tables.Count > 0 && tabNameData.Tables[0].Rows.Count > 0)
                {
                    listOfprofilesWithNoData = new List<TabEnum>();
                    foreach (DataRow dataSetRow in tabNameData.Tables[0].Rows)
                    {
                        listOfprofilesWithNoData.Add((TabEnum)System.Enum.Parse(typeof(TabEnum), dataSetRow[0].ToString().Trim(), true));
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetReportsToShow()", ex);
            }
        }

        /// <summary>
        /// Description: This method is used for filling day profile details in TOU grids for 1P IEC meters.
        /// Author: Mohsin Raza
        /// Date: 23-DEC-2016
        /// Remarks: Developed for torrent power limited as genric feature.
        /// </summary>
        private void FillDayProfileParametersIEC(string[] toudata, int meterModel)
        {
            DataRow reportRow;
            const int MAXROWDAYTABLE = 4;
            try
            {
                for (int idaytable = 0; idaytable < MAXROWDAYTABLE; idaytable++)
                {

                    string[] rowdataarr = toudata[idaytable].Split(')');


                    for (int irowcount = 0; irowcount < rowdataarr.Length - 1; irowcount++)
                    {
                        string rowdata = rowdataarr[irowcount].Replace("(", "");
                        rowdata = rowdata.Replace(")", "");
                        int tariff;
                        int.TryParse(rowdata.Substring(0, 1), out tariff);
                        string endHour = string.Empty;
                        string endMin = string.Empty;

                        if (irowcount == rowdataarr.Length - 2)
                        {
                            endHour = "00";
                            endMin = "00";
                        }
                        else
                        {
                            string nextrowdata = rowdataarr[irowcount + 1].Replace("(", "");
                            nextrowdata = nextrowdata.Replace(")", "");
                            endHour = nextrowdata.Substring(1, 2);
                            endMin = nextrowdata.Substring(3, 2);
                        }

                        if (tariff != 0)
                        {
                            string startHour = rowdata.Substring(1, 2);
                            string startMin = rowdata.Substring(3, 2);
                            reportRow = reportXSD.Tables["TouDayProfileTable"].NewRow();
                            reportRow["S. No."] = "Day Profile " + (idaytable + 1).ToString("d2");
                            reportRow["Slot No."] = (irowcount + 1).ToString("d2");
                            reportRow["Zone Start Time(HH:MM)"] = startHour + ":" + startMin;
                            reportRow["Zone End Time(HH:MM)"] = endHour + ":" + endMin;
                            reportRow["Tariff Zone"] = "T" + tariff.ToString();
                            reportXSD.Tables["TouDayProfileTable"].Rows.Add(reportRow);
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
                int weekProfileCount = 4;
                int nIndex = 2;
                DataRow reportRow;

                string[] rowdataarr = toudata[weekProfileCount].Split(')');

                for (int irowcount = 0; irowcount < rowdataarr.Length - 1; irowcount++)
                {
                    string rowdata = rowdataarr[irowcount].Replace("(", "");
                    rowdata = rowdata.Replace(")", "");
                    char[] coldata = rowdata.ToCharArray();

                    nIndex = 0;

                    reportRow = reportXSD.Tables["TouWeekTable"].NewRow();

                    reportRow["WeekNo"] = (irowcount + 1).ToString("00");

                    if (coldata[nIndex] == '0') break;

                    reportRow["Monday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Tuesday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Wednesday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Thursday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Friday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Saturday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportRow["Sunday"] = coldata[nIndex++].ToString().PadLeft(2, '0');

                    reportXSD.Tables["TouWeekTable"].Rows.Add(reportRow);
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
                const int weekProfileCount = 4;
                string[] rowdataarr = toudata[weekProfileCount].Split(')');
                DataRow reportRow;
                int seasonProfileCount = weekProfileCount;
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                    string rowdata = rowdataarr[seasonCount].Replace("(", "");
                    rowdata = rowdata.Replace(")", "");
                    string rowday = rowdata.Substring(MAXWEEKDAYS, 2);
                    string rowmonth = rowdata.Substring(MAXWEEKDAYS + 2, 2);
                    int tariff;
                    int.TryParse(rowday, out tariff);
                    reportRow = reportXSD.Tables["TouSeasonTable"].NewRow();

                    reportRow["WeekProfile"] = (seasonCount + 1).ToString("00");

                    if (tariff > 0 && tariff < 13)
                        reportRow["StartMonth"] = rowmonth;

                    if (tariff > 0 && tariff < 32)
                        reportRow["StartDay"] = rowday;

                    if (tariff == 0) break;

                    reportXSD.Tables["TouSeasonTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillSeasonProfileParametersIEC(string[] toudata, int meterModel)", ex);
                throw ex;
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

        
    }
}
