using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using CAB.Entity;
using CAB.BLL;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Framework.Entity;
using CAB.UI.Controls;
using System.Linq;
using System.Text;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class MeterWise : CABForm
    {
        DLMS650CommonBLL dlms650CommonBLL;
        ApplicationType types;
        private string MeterID = string.Empty;
        private string selectedTamperParameter;
        private int selectedTamperCode;
        private string MeterId = string.Empty;
        private string CABFileName = string.Empty;
        private FileReportDataSet reportXSD;
        private string[] MDHeadings = { "Cumulative MD1", "Cumulative MD2", "Cumulative MD3" };
        const string dateUnavailable = "--------";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        static List<string> lsHeadings;
        private Dictionary<string, string> reportColumns = new Dictionary<string, string>();
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        string selectedMeterId = string.Empty;
        private const string MIDNIGHTENERGIES = "MidnightEnergies";
        private int meterModelNo = 0;
        private int kVAhSelectionTamperId = 158;
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        private const int VoltagePhaseReversalOccurenceId = 13;
        private const int VoltagePhaseReversalRestorationId = 14;
        private const int MDReset = 159;
        private const int PushModeConfig = 160;
        private const int ScrollModeConfig = 161;
        private const int HRModeConfig = 162;
        private const int ScrollTimeConfig = 163;
        private const int DemandIntegrationPeriod = 152;
        private const int ProfileCapturePeriod = 153;
        private const int SingleActionScheduleForBillingDates = 154;
        private const string PowerOffDuration = "Power Off Duration";
        private const string PowerOffDurationOfColumnList = "Power-Failure Duration";
        private const string ActivePower = "Active Power (ABS)";
        private const string ActivePowerOfColumnList = "Signed Active";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterWise).ToString());
        public MeterWise()
        {
            //Utility Check added; 24th April 2012; Bug 75902   
            if (CAB.Framework.UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (CAB.Framework.UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else
            {
                isPUMA = false;
                isMVVNL = false;
            }
            InitializeComponent();
            CreateReportColumns();
        }

        private void meterSelectForm_OnGridValueSelection(string meterID)
        {
            MeterID = meterID;
            txtBoxSelectMeter.Text = meterID;
            btnMeterNext.Enabled = true;
            FillListWithFiles(meterID);
        }

        private void FillListWithFiles(string meterID)
        {
            DataSet ds = new MeterDataBLL().ListDataSet("MSN", meterID, true);
            lngGridAvailableFiles.ValueColumn = "MeterData_ID";
            lngGridAvailableFiles.Data = ds;
            lngGridAvailableFiles.IsSorting = false;
            lngGridAvailableFiles.SetWidth("S.No", 60);
            lngGridAvailableFiles.SetWidth("File Name", 220);
            lngGridAvailableFiles.HiddenColumns = "MeterData_ID,Reading DateTime";
            lngGridAvailableFiles.RefreshGrid();

        }



        private bool ValidateMeterSelection()
        {
            if (txtBoxSelectMeter.Text.Length == 0)
            {
                MessageBox.Show("Please select a meter.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (lngGridAvailableFiles.Data.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Files are not available for this meter.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private List<string> GetColumns(string paramType)
        {
            Dictionary<string, string> columnDictionary = new Dictionary<string, string>();
            bool isPUMAMeter = false;
            List<string> columns = new List<string>();

            if (paramType == "General")
            {
                //columnDictionary = new GeneralBLL().CreateGeneralDictionary();
                columnDictionary = new DLMS650GeneralBLL().CreateGeneralDictionary();
            }
            else if (paramType == "Anomaly")
            {
                columnDictionary = new AnomalyBLL().CreateAnomalyDictionary();
            }
            else if (paramType == "Instant")
            {
                //BhardwajG : If meter model no is enables fetch the instant dictionary accordingly as Ruby and PUMA meters both available for this utililty
                //columnDictionary = new InstantPowerBLL().CreateInstantDictionary();
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    if (meterModelNo == NamePlateConstants.PumaLTE650Value || meterModelNo == NamePlateConstants.PumaHTE650Value)
                    {
                        isPUMAMeter = true;
                    }
                    else
                    {
                       isPUMAMeter = false;
                    }
                    columnDictionary = new DLMS650InstantaneousBLL().CreateInstantDictionary(selectedMeterId,isPUMAMeter);
                //}
                //else
                //{
                //    columnDictionary = new DLMS650InstantaneousBLL().CreateInstantDictionary(selectedMeterId);
                //}
            }
            else if (paramType == "Billing")
            {
                //columnDictionary = new BillingBLL().CreateBillingDictionary();
                columnDictionary = new DLMS650BillingBLL().CreateBillingDictionary(selectedMeterId);
            }
            else if (paramType == "LoadSurvey")
            {
                //columnDictionary = new LoadSurveyBLL().CreateLoadSurveyDictionary();
                columnDictionary = new DLMS650LoadSurveyBLL().CreateLoadSurveyDictionary(selectedMeterId);
            }
            else if (paramType == "TamperSnapshot")
            {
                //columnDictionary = new TamperSnapShotBLL().CreateSnapshotDictionary();
                columnDictionary = new DLMS650TamperMasterBLL().CreateTamperDictionary(selectedMeterId);
            }
            //added for MVVNL
            else if (paramType == "MidnightEnergies")
            {
                lblNotification.Visible = false;
                columnDictionary = new DLMS650MidnightDataBLL().CreateMidnightDataDictionary(selectedMeterId);
            }
            //added for MVVNL


            foreach (KeyValuePair<string, string> kvp in columnDictionary)
                columns.Add(kvp.Key);

            return columns;
        }

        private bool ValidateParameters()
        {
            if (chkListSelectParameters.CheckedItems.Count > 8)
            {
                this.StatusMessage = "Please Select 8 Parameters only";
                return false;
            }
            else if (chkListSelectParameters.CheckedItems.Count == 0)
            {
                this.StatusMessage = "Please select a parameter";
                return false;
            }
            return true;
        }


        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }

        //private DataSet ListGeneralData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetGeneralReportData(activeMeterDataId);
        //}

        //private DataSet ListInstantData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetInstantReportData(activeMeterDataId);
        //}

        //private DataSet ListPhasorData(long activeMeterDataId)
        //{
        //    return new PhasorBLL().ListDataSet(activeMeterDataId);
        //}

        //private DataSet ListPhasorData(string activeMeterDataId)
        //{
        //    return new PhasorBLL().ListDataSet(activeMeterDataId);
        //}

        //private DataSet ListTamperData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetTamperReportData(activeMeterDataId);
        //}

        //private DataSet ListPowerOnHoursData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetPowerOnHoursReportData(activeMeterDataId);
        //}

        //private DataSet ListPowerFactorData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetPowerFactorReportData(activeMeterDataId);
        //}

        //private DataSet GetTariffPF(long activeMeterDataId)
        //{
        //    return new TariffBLL().GetTariffPF(activeMeterDataId);
        //}

        //private DataSet ListCTRatioData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetCTRatioReportData(activeMeterDataId);
        //}

        //private DataSet ListLoadFactorData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetLoadFactorReportData(activeMeterDataId);
        //}

        //private DataSet ListBillingTamperCounterData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetBillingTamperCounterReportData(activeMeterDataId);
        //}

        //private DataSet ListTamperOccResData(long activeMeterDataId)
        //{
        //    return new BillingBLL().TamperOccurRestore(activeMeterDataId);
        //}

        //public DataSet GetTamperOccurRestoreDetail(int tamperSnapShotID, long activeMeterDataId)
        //{
        //    return CommonBLL.GetTamperOccurRestoreDetail(tamperSnapShotID, activeMeterDataId);
        //}

        //private DataSet ListMainEnergyData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetMainEnergyReportData(activeMeterDataId);
        //}

        //private DataSet ListTariffEnergyData(long activeMeterDataId, int historyID)
        //{
        //    return new TariffBLL().GetMeterData(activeMeterDataId, historyID);
        //}

        //private DataSet ListTODMDData(long activeMeterDataId, int historyID)
        //{
        //    return new TariffBLL().GetTODMDMeterData(Convert.ToInt16(activeMeterDataId), historyID);
        //}

        //private DataSet ListConsumptionEnergyData(long activeMeterDataId)
        //{
        //    return new MeterwiseReportBLL().GetMainEnergyReportData(activeMeterDataId);
        //}

        //private DataSet ListLoadSurveyData(long activeMeterDataId)
        //{
        //    DataSet dataSet = new DataSet();
        //    LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
        //    dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId, loadSurveyBLL.GetFromDate(activeMeterDataId), loadSurveyBLL.GetToDate(activeMeterDataId));
        //    return dataSet;
        //}

        //private DataSet ListDTMLoadSurveyData(long activeMeterDataId)
        //{
        //    DTMLoadSurveyBLL dtmLoadSurveyBLL = new DTMLoadSurveyBLL();
        //    return dtmLoadSurveyBLL.ListDataSet(activeMeterDataId);
        //}

        //private DataSet ListTransactionData(long activeMeterDataId)
        //{
        //    ProgrammingBLL programmingBLL = new ProgrammingBLL();
        //    return programmingBLL.GetProgrammingList(activeMeterDataId);
        //}

        //private DataSet ListRTCUpdatesData(long activeMeterDataId)
        //{
        //    return new RTCUpdateBLL().GetRTCUpdateList(activeMeterDataId);
        //}

        //private IEntity ListFraudEnergyData(long activeMeterDataId)
        //{
        //    return new FraudEnergyBLL().GetFraudEnergy(activeMeterDataId);
        //}

        private void FillMeterID(DataSet meterIdDS)
        {
            DataRow reportRow;
            if (meterIdDS != null && meterIdDS.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in meterIdDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = dateUnavailable;
                }
                reportRow["ActiveMeter"] = "Inactive";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }

        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            DataRow reportRow;

            if (detailsDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in detailsDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
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

                    if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                    {
                        /* VBM - EMF Bug Fixed */
                        string meterEMF = CommonBLL.CalculateActualEMF(Convert.ToDecimal(row["Meter_EMF"].ToString()),
                                                                         row["internalCTRatio"].ToString(),
                                                                         row["internalPTRatio"].ToString());
                        /* VBM - EMF Bug Fixed */
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
                        reportRow["EMF"] = meterEMF;
                    }
                    else
                        reportRow["EMF"] = dateUnavailable;

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
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                    reportXSD.AcceptChanges();
                }
            }
        }

        private void FillGeneralXSD(DataSet generalData)
        {
            DataRow reportRow;

            if (generalData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingGeneralTable"].NewRow();
                foreach (DataRow row in generalData.Tables[0].Rows)
                {
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

                    //reportRow["Occurrence Time"] = CommonBLL.GetFormattedData(row["OccurrenceTime"].ToString());
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

                    //reportRow["Restoration Time"] = CommonBLL.GetFormattedData(row["RestorationTime"].ToString());
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

        private void FillInstantXSD(DataSet instantData)
        {

            DataRow reportRow;
            if (instantData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in instantData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["InstantTable_MW"].NewRow();
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
                    reportRow["ReadingDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row["ReadingDateTime"].ToString())).ToString(ConfigInfo.DateFormat());
                    reportRow["FileName"] = CommonBLL.GetFormattedData(row["FileName"].ToString());
                    reportXSD.Tables["InstantTable_MW"].Rows.Add(reportRow);
                }
            }
        }

        private void FillPhasorXSD(DataSet phasorData)
        {
            DataRow reportRow;
            if (phasorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PhasorTable_MW"].NewRow();
                reportRow["RVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[0][1].ToString());
                reportRow["YVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[1][1].ToString());
                reportRow["BVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[2][1].ToString());
                reportRow["RCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[3][1].ToString());
                reportRow["YCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[4][1].ToString());
                reportRow["BCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[5][1].ToString());
                reportRow["RPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[6][1].ToString());
                reportRow["YPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[7][1].ToString());
                reportRow["BPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[8][1].ToString());
                reportRow["TotalPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[9][1].ToString());
                reportRow["Frequency"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[10][1].ToString());
                reportRow["PhaseSequence"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[11][1].ToString());
                reportRow["TotalImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[15][1].ToString());
                reportRow["RImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[12][1].ToString());
                reportRow["YImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[13][1].ToString());
                reportRow["BImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[14][1].ToString());
                reportRow["RLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[19][1].ToString());
                reportRow["YLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[20][1].ToString());
                reportRow["BLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[21][1].ToString());
                reportRow["TotalLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[22][1].ToString());
                reportRow["YPhaseAngle"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[23][1].ToString());
                reportRow["BPhaseAngle"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[24][1].ToString());
                reportRow["PhaseAngleDifference"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[25][1].ToString());
                reportRow["FileName"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[26][1].ToString());
                reportRow["ReadingDateTime"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[27][1].ToString());
                reportXSD.Tables["PhasorTable_MW"].Rows.Add(reportRow);
            }
        }

        public static byte[] ImageTable(string ImageFile)
        {
            FileStream fs = new FileStream(ImageFile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] imageData = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));

            br = null;
            fs.Close();
            fs = null;
            return imageData;
        }

        private void FillPhasorDigram(DataSet phasorData)
        {
            try
            {
                PhasorDiagram phasorGraph = new PhasorDiagram();
                phasorGraph.PhasorDataset = phasorData;
                DataRow reportRow;
                Bitmap graphBitmap = new Bitmap(phasorGraph.Width, phasorGraph.Width);
                phasorGraph.DrawToBitmap(graphBitmap, phasorGraph.ClientRectangle);
                using (Graphics g = phasorGraph.CreateGraphics())
                {
                    g.DrawImageUnscaled(graphBitmap, new Point(0, 0));
                }
                graphBitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "PhasorDiagram.jpg");

                byte[] DiagramByte = ImageTable(AppDomain.CurrentDomain.BaseDirectory + "PhasorDiagram.jpg");

                reportRow = reportXSD.Tables["PhasorDiagramTable"].NewRow();
                reportRow["Image"] = DiagramByte;
                reportXSD.Tables["PhasorDiagramTable"].Rows.Add(reportRow);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillPhasorDigram(DataSet phasorData)", ex);
            }
        }

        private void FillPowerOnHoursXSD(DataSet powerOnHoursData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerOnHoursData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerFactorTable"].NewRow();
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
                reportXSD.Tables["PowerFactorTable"].Rows.Add(reportRow);
            }
        }

        private void FillPowerFactorXSD(DataSet powerFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerOnHoursTable"].NewRow();
                foreach (DataRow row in powerFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = row["AveragePowerFactor"].ToString();//CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString()).Remove(row["AveragePowerFactor"].ToString().Length);
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString().ToString()));
                    }
                    historyID++;
                }
                reportXSD.Tables["PowerOnHoursTable"].Rows.Add(reportRow);
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
                        catch (Exception ex)    //Exception log for catch block
                        {
                            val = "0";
                            logger.Log(LOGLEVELS.Error, "FillCTRatioXSD(DataSet loadFactorData)", ex);
                        }
                        reportRow[colName] = val;
                    }
                    historyID++;
                }
                reportXSD.Tables["CTRatioTable"].Rows.Add(reportRow);
            }
        }

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

        private void FillMainEnergyXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["MainEnergyTable_MW"].NewRow();
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
                    reportXSD.Tables["MainEnergyTable_MW"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTariffEnergyXSD(DataSet mainEnergyData, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
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

        private void FillMaximumDemandXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
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

        private void FillTODMDXSD(DataSet TODMDData, int historyID)
        {
            DataRow reportRow;
            if (TODMDData.Tables[0].Rows.Count > 0)
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

        private void FillTODEnergyConsumptionXSD(DataSet currentTariffEnergyDS, DataSet nextTariffEnergyDS, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;

            int i = 0;

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

        private void FillLoadSurveyXSD(DataSet loadSurveyData)
        {
            if (radioBtnLoadSurveyParameter.Checked == true)
                FillLoadSurveyDemandXSD(loadSurveyData);
            else
                FillLoadSurveyEnergyXSD(loadSurveyData);
        }

        private void FillLoadSurveyDemandXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            int dateTimeCount = 0;
            DateTime PreviousDate = DateTime.Now;
            try
            {
                if (loadSurveyData == null || loadSurveyData.Tables[0].Rows.Count == 0)
                    return;

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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillLoadSurveyDemandXSD(DataSet loadSurveyData)", ex);
            }
        }

        string GetEnergyColumnName(string ColName)
        {
            if (ColName == "Demand kW") return ColName = "Energy kWh";
            else if (ColName == "Demand kVA") return ColName = "Energy kVAh";
            else if (ColName == "Demand kVAr Lag" || ColName == "Demand kVAr(Lag)") return ColName = "Energy kVArh Lag";
            else if (ColName == "Demand kVAr Lead" || ColName == "Demand kVAr(Lead)") return ColName = "Energy kVArh Lead";
            else return null;

        }

        private void FillLoadSurveyEnergyXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            int dateTimeCount = 1;
            int IntervalValue = 4;
            DateTime PreviousDate = DateTime.Now;
            try
            {
                reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();

                if (loadSurveyData == null)
                {
                    return;
                }
                if (loadSurveyData.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                for (int ParamCount = 0; ParamCount < loadSurveyData.Tables[0].Columns.Count; ParamCount++)
                {
                    if (loadSurveyData.Tables[0].Columns[ParamCount].ColumnName.Contains("Demand"))
                    {
                        lsHeadings.Add(GetEnergyColumnName(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName));
                    }
                    else
                    {
                        lsHeadings.Add(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName);
                    }
                }
                foreach (DataRow Drow in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    //For the date Time to split at 00:15 hours
                    if (dateTimeCount == 1)
                    {
                        PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(Drow[0])));
                        reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitDateUnit(Convert.ToString(Drow[0])));
                        dateTimeCount++;
                    }
                    else
                    {
                        string dates = "";
                        if (!string.IsNullOrEmpty(Convert.ToString(Drow[0])))
                        {
                            DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(Drow[0])));
                            TimeSpan ts = currentDate - PreviousDate;
                            if (ts.Days > 0)
                            {
                                currentDate = currentDate.AddDays(-1);
                                long datesval = DateUtility.DateTimeToLong(currentDate);
                                dates = DateUtility.LongToStringDateFormat(datesval);
                                reportRow["GroupDateTime"] = dates;
                                PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitDateUnit(Convert.ToString(Drow[0])));
                            }
                            else
                            {
                                dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitDateUnit(Convert.ToString(Drow[0])));
                                reportRow["GroupDateTime"] = dates;
                            }
                        }
                    }
                    string dateTimes = Convert.ToString(Drow[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeColumn"] = dateTimes;
                    for (int ParamCount = 1; ParamCount < loadSurveyData.Tables[0].Columns.Count - 1; ParamCount++)
                    {
                        string ParameterColValue = "Parameter" + Convert.ToString(ParamCount);
                        if (loadSurveyData.Tables[0].Columns[ParamCount].ColumnName.Contains("Demand"))
                        {
                            if (Convert.ToDouble(Drow[ParamCount].ToString()) != -1)
                            {
                                string str = string.Format("{0:0.0000}", (Convert.ToDouble(Drow[ParamCount].ToString()) / IntervalValue)).ToString();
                                str = str.Substring(0, str.Length - 1);
                                reportRow[ParameterColValue] = str;
                            }
                            else
                            {
                                reportRow[ParameterColValue] = "-1";
                            }
                        }
                        else
                            reportRow[ParameterColValue] = Drow[ParamCount].ToString();
                    }
                    reportXSD.Tables["LoadSurveyTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillLoadSurveyEnergyXSD(DataSet loadSurveyData)", ex);
            }
        }
        private void FillTamperXSD(DataSet tamperData)
        {
            DataRow reportRow;

            if (tamperData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in tamperData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["TamperTable"].NewRow();

                    reportRow["Tamper Description"] = row["TamperType"].ToString();
                    reportRow["Tamper Counter"] = row["TamperCounter"].ToString().ToString();

                    reportRow["Occured DateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    reportRow["Restored DateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperRestoredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    reportRow["RVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["RVoltageRestored"].ToString()), " V");
                    reportRow["YVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["YVoltageRestored"].ToString()), " V");
                    reportRow["BVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["BVoltageRestored"].ToString()), " V");
                    reportRow["RCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["RCurrentRestored"].ToString()), " A");
                    reportRow["YCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["YCurrentRestored"].ToString()), " A");
                    reportRow["BCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["BCurrentRestored"].ToString()), " A");
                    reportRow["RPF_restored"] = CommonBLL.GetFormattedData(row["RPFRestored"].ToString());
                    reportRow["YPF_restored"] = CommonBLL.GetFormattedData(row["YPFRestored"].ToString());
                    reportRow["BPF_restored"] = CommonBLL.GetFormattedData(row["BPFRestored"].ToString());
                    reportRow["TotalPF_restored"] = CommonBLL.GetFormattedData(row["TotalPFRestored"].ToString());
                    reportRow["kWh_restored"] = string.Concat(CommonBLL.GetFormattedData(row["kWhRestored"].ToString()), " kWh");
                    reportRow["kVAh_restored"] = string.Concat(CommonBLL.GetFormattedData(row["kVAhRestored"].ToString()), " kVAh");
                    reportRow["RVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["RVoltageOccurred"].ToString()), " V");
                    reportRow["YVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["YVoltageOccurred"].ToString()), " V");
                    reportRow["BVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["BVoltageOccurred"].ToString()), " V");
                    reportRow["RCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["RCurrentOccurred"].ToString()), " A");
                    reportRow["YCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["YCurrentOccurred"].ToString()), " A");
                    reportRow["BCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["BCurrentOccurred"].ToString()), " A");
                    reportRow["RPF_occurred"] = CommonBLL.GetFormattedData(row["RPFOccurred"].ToString());
                    reportRow["YPF_occurred"] = CommonBLL.GetFormattedData(row["YPFOccurred"].ToString());
                    reportRow["BPF_occurred"] = CommonBLL.GetFormattedData(row["BPFOccurred"].ToString());
                    reportRow["TotalPF_occurred"] = CommonBLL.GetFormattedData(row["TotalPFOccurred"].ToString());
                    reportRow["kWh_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["kWhOccurred"].ToString()), " kWh");
                    reportRow["kVAh_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["kVAhOccurred"].ToString()), " kVAh");

                    reportXSD.Tables["TamperTable"].Rows.Add(reportRow);
                }
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

                    reportRow["OccurredDateTime"] = occurrenceTime;
                    reportRow["RestoredDateTime"] = restorationTime;
                    TimeSpan ts = Convert.ToDateTime(restorationTime) - Convert.ToDateTime(occurrenceTime);

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
            DataRow reportRow;
            if (transactionDS == null)
                return;

            if (transactionDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in transactionDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["ProgrammingUpdatesTable"].NewRow();

                    reportRow["LastTimeStamp"] = row["UpdateSequence"].ToString();
                    reportRow["TimeStamp"] = row["LastTimeStamp"].ToString();
                    reportRow["Parameter1"] = row["Description1"].ToString();
                    reportRow["Parameter2"] = row["Description2"].ToString();
                    reportRow["Parameter3"] = row["Description3"].ToString();
                    reportRow["Parameter4"] = row["Description4"].ToString();
                    reportRow["Parameter5"] = row["Description5"].ToString();
                    reportRow["Parameter6"] = row["Description6"].ToString();
                    reportRow["Parameter7"] = row["Description7"].ToString();
                    reportRow["Parameter8"] = row["Description8"].ToString();
                    reportRow["Parameter9"] = row["Description9"].ToString();
                    reportRow["Parameter10"] = row["Description10"].ToString();
                    reportRow["Parameter11"] = row["Description11"].ToString();
                    reportRow["Parameter12"] = row["Description12"].ToString();
                    reportRow["Parameter13"] = row["Description13"].ToString();
                    reportRow["Parameter14"] = row["Description14"].ToString();
                    reportRow["Parameter15"] = row["Description15"].ToString();
                    reportRow["Parameter16"] = row["Description16"].ToString();
                    reportRow["Parameter17"] = row["Description17"].ToString();

                    reportXSD.Tables["ProgrammingUpdatesTable"].Rows.Add(reportRow);
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

        //private void FillFraudEnergyXSD(FraudEnergyEntity entity)
        //{
        //    DataRow reportRow;
        //    if (entity != null)
        //    {
        //        reportRow = reportXSD.Tables["FraudEnergyTable"].NewRow();
        //        reportRow["ActivekWh"] = entity.MagneticInfluenceKWh;
        //        reportRow["ApparentkVAh"] = entity.MagneticInflueneceKVAh;
        //        reportRow["CumulativekWh"] = entity.ReverseEnergyKWh;
        //        reportRow["CumulativekVAh"] = entity.ReverseEnergyKVAh;
        //        reportXSD.Tables["FraudEnergyTable"].Rows.Add(reportRow);
        //    }
        //}
        /// <summary>
        /// Used to fill anomaly xsd table 
        /// </summary>
        /// <param name="objAnomalyEntity"></param>
        private void FillAnomalyXSD(AnomalyEntity objAnomalyEntity)
        {
            DataRow reportRow;
            reportRow = reportXSD.Tables["Anomaly"].NewRow();
            reportRow["Flash"] = (objAnomalyEntity.Flash == 1) ? "OK" : "NOT OK";
            // GKG: 04/08/13 142860 
            //reportRow["EepRomStatus"] = (objAnomalyEntity.EeProm == 1) ? "OK" : "NOT OK";
            //reportRow["SmpsStatus"] = (objAnomalyEntity.Smps == 1) ? "OK" : "NOT OK";
            //reportRow["RtcStatus"] = (objAnomalyEntity.Rtc == 1) ? "OK" : "NOT OK";

            reportRow["EepRom"] = (objAnomalyEntity.EeProm == 1) ? "OK" : "NOT OK";
            reportRow["Smps"] = (objAnomalyEntity.Smps == 1) ? "OK" : "NOT OK";
            reportRow["Rtc"] = (objAnomalyEntity.Rtc == 1) ? "OK" : "NOT OK";

            // GKG: 04/08/13 142860 

            reportXSD.Tables["Anomaly"].Rows.Add(reportRow);
        }

        private void ShowReport(string reportType)
        {
            try
            {
                reportXSD = new FileReportDataSet();
                DataRow reportRow;
                List<string> columnList = new List<string>();
                MWMeterDetailsReport mMeterDetailsReport = new MWMeterDetailsReport();
                DatabaseReportForm databaseReportForm = new DatabaseReportForm();
                DataSet ds = new DataSet();
                columnList = GetColumnsCollection();


                //***To get consumer details in header****************//
                DataSet tmpData = null;
                tmpData = new MeterDataBLL().ListDataSet("MSN", MeterID, false);
                //int index = lngGridAvailableMeters.SelectedIndex;
                string activeMeterDataId = tmpData.Tables[0].Rows[0].ItemArray[1].ToString();

                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(activeMeterDataId));
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetails(detailsDS);
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(activeMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                        FillMeterID(meterIDDS);
                }
                //***To get consumer details in header****************//

                CrystalDecisions.CrystalReports.Engine.TextObject TxtHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["txtReportType"];

                //Find static text object txtMidNight. Displaying static text "Midnight value are of 00:00hrs". 
                CrystalDecisions.CrystalReports.Engine.TextObject txtMidNight = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["txtMidNight"];
                //Statis text will be visible only for Midnight report.
                txtMidNight.Width = 0;
                /* VBM - Add BCS Version number in report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["txtBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* VBM - Add BCS Version number in report header */
                if (reportType == "General")
                {
                    TxtHeading.Text = "General Details";
                    //ds = new GeneralBLL().GetGeneralDataByParameter(MeterID, columnList, "Meter");
                    //BhardwajG : Pass the boolean to the function for identifying whether model no is required or not
                    ds = new DLMS650GeneralBLL().GetGeneralDataByParameter(MeterID, columnList,true);
                }
                else if (reportType == "Anomaly")
                {
                    TxtHeading.Text = "Self Diagnostics Report";
                    //ds = new GeneralBLL().GetGeneralDataByParameter(MeterID, columnList, "Meter");
                    ds = new AnomalyBLL().GetAnomalyDataByParameter(MeterID, columnList);
                }
                else if (reportType.Equals("Instant"))
                {
                    TxtHeading.Text = "Instantaneous Details";
                    //ds = new InstantPowerBLL().GetInstantDataByParameter(MeterID, columnList, "Meter");                   
                    ds = new DLMS650InstantaneousBLL().GetInstantDataByParameter(MeterID, columnList, activeMeterDataId);
                    //Bind anomlay report 
                    AnomalyEntity objAnomalyEntity = (AnomalyEntity)new AnomalyBLL().GetDetailData((Convert.ToInt32(ConfigInfo.ActiveMeterDataId)));
                    if (objAnomalyEntity != null && objAnomalyEntity.AnomalyId > 0)
                    {
                        FillAnomalyXSD(objAnomalyEntity);
                    }
                }
                else if (reportType == "Billing")
                {
                    TxtHeading.Text = "Billing Details";
                    //ds = new BillingBLL().GetBillingDataByParameter(MeterID, columnList, "Meter");
                    ds = new DLMS650BillingBLL().GetBillingData(MeterID, columnList);
                }
                else if (reportType == "LoadSurvey")
                {
                    TxtHeading.Text = "Load Survey Details";
                    //ds = new LoadSurveyBLL().GetLoadSurveyDataByParameter(MeterID, columnList, "Meter");
                    ds = new DLMS650LoadSurveyBLL().GetLoadSurveyData(MeterID, columnList);
                }
                else if (reportType == "Tamper")
                {
                    TxtHeading.Text = "Tamper Details";
                    //ds = new TamperSnapShotBLL().GetTamperSnapshotDataByParameter(MeterID, columnList, selectedTamperCode, "Meter");
                    ds = new DLMS650TamperMasterBLL().GetTamperSnapshotData(MeterID, columnList, selectedTamperCode);
                    //CrystalDecisions.CrystalReports.Engine.TextObject TxtTamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TxtTamperParameter"];
                    //TxtTamperHeading.Text = selectedTamperParameter;
                    CrystalDecisions.CrystalReports.Engine.TextObject TamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["tamperName"];
                    TamperHeading.Text = selectedTamperParameter;
                    CrystalDecisions.CrystalReports.Engine.TextObject TamperCode = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["tamperCode"];
                    TamperCode.Text = selectedTamperCode.ToString();
                }
                else if (reportType == "Transaction")
                {
                    TxtHeading.Text = "Transaction Details";
                    ds = new DLMS650TamperMasterBLL().GetTransactionSnapShotData(MeterID, columnList, selectedTamperCode);

                    CrystalDecisions.CrystalReports.Engine.TextObject TamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["tamperName"];
                    TamperHeading.Text = selectedTamperParameter;
                    CrystalDecisions.CrystalReports.Engine.TextObject TamperCode = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["tamperCode"];
                    TamperCode.Text = selectedTamperCode.ToString();
                }
                //added for MVVNL
                else if (reportType == MIDNIGHTENERGIES)
                {
                    //Set the width from 0 to 2280 to display the static text object.
                    txtMidNight.Width = 2280;
                    TxtHeading.Text = "Midnight Energies";
                    ds = new DLMS650MidnightDataBLL().GetMidnightEnergies(MeterID, columnList);
                }
                //added for MVVNL

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (reportType.Equals("General"))
                        this.StatusMessage = "General data not available.";
                    else if (reportType.Equals("Anomaly"))
                        this.StatusMessage = "Self Diagnostics data not available.";
                    else if (reportType.Equals("Instant"))
                        this.StatusMessage = "Instantaneous data not available.";
                    else if (reportType.Equals("Billing"))
                        this.StatusMessage = "Billing data not available.";
                    else if (reportType.Equals("LoadSurvey"))
                        this.StatusMessage = "Load Survey data not available.";
                    else if (reportType.Equals("Tamper"))
                        this.StatusMessage = "Tamper data not available.";
                    else if (reportType.Equals("Transaction"))
                        this.StatusMessage = "Transaction data not available.";
                    //added for MVVNL
                    else if (reportType.Equals(MIDNIGHTENERGIES))
                    {
                        this.StatusMessage = "Midnight Energies not available.";
                    }
                    //added for MVVNL
                    return;
                }
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["MDataReportsTable"].NewRow();
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {

                            if (col.Ordinal == 0)
                            {
                                reportRow["MeterNo"] = row[col];
                            }
                            else if (col.Ordinal == 1)
                            {
                                reportRow["FileName"] = row[col];
                            }
                            //else if (col.Ordinal == 0)
                            //{
                            //}
                            else if (col.Ordinal == 2)
                                reportRow["ReadingDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            else
                            {
                                if (CommonBLL.IsTimeColumn(col.ColumnName))
                                    if (row[col].ToString().Equals("0"))
                                        reportRow[string.Concat("Parameter", col.Ordinal - 2)] = dateUnavailable;
                                    else
                                    {
                                        DateTime dtx = new DateTime();
                                        string dateString = "";
                                        try
                                        {
                                            dtx = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString()));
                                        }
                                        catch (Exception ex)    //Exception log for catch block
                                        {
                                            dateString = "----";
                                            logger.Log(LOGLEVELS.Error, "ShowReport(string reportType)", ex);
                                        }
                                        if (!string.IsNullOrEmpty(dateString))
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = "----";
                                        }
                                        else
                                        {
                                            if (reportType.Equals(MIDNIGHTENERGIES))
                                            {
                                                reportRow[string.Concat("Parameter", col.Ordinal - 2)] = dtx.ToString(ConfigInfo.DateFormat());
                                            }
                                            else
                                            {
                                                reportRow[string.Concat("Parameter", col.Ordinal - 2)] = dtx.ToString(ConfigInfo.DateFormat() + " HH:mm");
                                            }
                                        }
                                    }
                                else
                                    if (row[col].ToString().IndexOf('*') >= 0)//(row[col].ToString().IndexOf("(Lead)") < 0 && row[col].ToString().IndexOf("(Lag)") < 0 && row[col].ToString().IndexOf("(kVA)") < 0))
                                    {
                                        if (!(col.ColumnName == "CumulativeMD1" || col.ColumnName == "CumulativeMD2" || col.ColumnName == "CumulativeMD3"))
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = row[col].ToString().Substring(0, row[col].ToString().IndexOf('*'));
                                        }
                                        else
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = row[col].ToString();
                                        }
                                    }
                                    else
                                    {
                                        string val = Convert.ToString(row[col]);
                                        if (val.Trim().ToUpper().Equals("LGZ"))
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = "Cabcon";
                                        else
                                            // Added to solve bug 94907.
                                            if (reportType.Equals(MIDNIGHTENERGIES))
                                            {
                                                reportRow[string.Concat("Parameter", col.Ordinal - 2)] = CommonBLL.GetFormattedData(val);
                                            }
                                            else if (reportType.Equals("Anomaly"))
                                            {
                                                reportRow[string.Concat("Parameter", col.Ordinal - 2)] = (Boolean)row[col] ? "OK" : "NOT OK";
                                            }
                                            else
                                            {
                                                reportRow[string.Concat("Parameter", col.Ordinal - 2)] = val;
                                            }
                                    }
                            }
                        }
                        reportXSD.Tables["MDataReportsTable"].Rows.Add(reportRow);
                    }
                }
                else
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["MDataReportsTable"].NewRow();
                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {

                            if (col.Ordinal == 0)
                                reportRow["MeterNo"] = row[col];
                            else if (col.Ordinal == 1)
                                reportRow["FileName"] = row[col];
                            else if (col.Ordinal == 2)
                                reportRow["ReadingDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            else
                            {
                                if (CommonBLL.IsTimeColumn(col.ColumnName))
                                    if (row[col].ToString().Equals("0"))
                                        reportRow[string.Concat("Parameter", col.Ordinal - 2)] = dateUnavailable;
                                    else
                                    {
                                        DateTime dtx = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString()));
                                        if (dtx.Day == System.DateTime.Now.Day && dtx.Month == System.DateTime.Now.Month && dtx.Year == System.DateTime.Now.Year)
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = "----";
                                        }
                                        else
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = dtx.ToString(ConfigInfo.DateFormat() + " HH:mm");
                                        }
                                    }
                                else
                                    if (row[col].ToString().IndexOf('*') >= 0)//(row[col].ToString().IndexOf("(Lead)") < 0 && row[col].ToString().IndexOf("(Lag)") < 0 && row[col].ToString().IndexOf("(kVA)") < 0))
                                    {
                                        if (!(col.ColumnName == "CumulativeMD1" || col.ColumnName == "CumulativeMD2" || col.ColumnName == "CumulativeMD3"))
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = CommonBLL.GetFormattedData(row[col].ToString().Substring(0, row[col].ToString().IndexOf('*')));
                                        }
                                        else
                                        {
                                            reportRow[string.Concat("Parameter", col.Ordinal - 2)] = CommonBLL.GetFormattedData(row[col].ToString());
                                        }
                                    }
                                    else
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal - 2)] = CommonBLL.GetFormattedData(row[col].ToString());
                                    }
                            }
                        }
                        reportXSD.Tables["MDataReportsTable"].Rows.Add(reportRow);
                    }
                }
                if (!(reportType.Equals("Tamper") || (reportType == "Transaction")))
                {
                    mMeterDetailsReport.Section2.SectionFormat.EnableSuppress = true;
                }
                int startIndex = 0;
                if (reportType.Equals("MidnightEnergies"))
                {
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["Parameter1"];
                    TextParam.Text = "Date \n (0.0.1.0.0.255;8;2)";
                    TextParam.ObjectFormat.EnableSuppress = false;
                    startIndex++;
                }
               
                for (int i = 0; i < chkListSelectParameters.CheckedItems.Count; i++)
                {
                    bool isBillingPowerOffDuration = false;
                    bool isSignedActivePower = false;
                    //columnList.Add(chkListSelectParameters.CheckedItems[i].ToString());
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["Parameter" + (startIndex + 1)]; startIndex++;
                    foreach (KeyValuePair<string, string> pair in reportColumns)
                    {
                        dlms650CommonBLL = new DLMS650CommonBLL();
                        columnList[i] = dlms650CommonBLL.GetColName(columnList[i]);
                        // To get obis code for power off hours used in tnstant but with different display name .
                        //Forced to do so as dictionary does not allow duplicate key(obiscode)
                        if (columnList[i] == PowerOffDuration)
                        {
                            columnList[i] = PowerOffDurationOfColumnList;
                            isBillingPowerOffDuration = true;
                        }
                        if (columnList[i] == ActivePower)
                        {
                            columnList[i] = ActivePowerOfColumnList;
                            isSignedActivePower = true;
                        }
                           
                        if (columnList[i].Contains(pair.Value))
                        {
                            if (isSignedActivePower)
                            {
                                columnList[i] = ActivePower + " " + pair.Key;
                            }
                            else if (isBillingPowerOffDuration)
                            {
                                columnList[i] = PowerOffDuration + " " + pair.Key;
                            }
                            else
                            {
                               columnList[i] = columnList[i] + " " + pair.Key;
                            }
                            
                            break;
                        }
                    }

                    TextParam.Text = columnList[i];//chkListSelectParameters.CheckedItems[i].ToString();
                    TextParam.ObjectFormat.EnableSuppress = false;
                }

                // Apply modern blue theme and custom logo before rendering
                ReportThemeHelper.Apply(mMeterDetailsReport);
                mMeterDetailsReport.SetDataSource(reportXSD);
                //This condition added to show the abbreviations for tampers at the report footer; 24th April 2012; Bug 75902   
                if (!isPUMA)
                {
                    mMeterDetailsReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    if (!(reportType.Equals("LoadSurvey")))
                    {
                        mMeterDetailsReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                    }
                    else
                    {
                        if (!ds.Tables[0].Columns.Contains("TamperStatus"))
                        {
                            mMeterDetailsReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                        }
                    }
                }
                databaseReportForm.drptViewer.ReportSource = mMeterDetailsReport;
                databaseReportForm.drptViewer.Zoom(1);
                this.Hide();
                databaseReportForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowReport(string reportType)", ex);
                throw new CABException(ex);
            }
        }

        public Dictionary<string, string> CreateReportColumns()
        {
            //Instant
            //reportColumns.Add("(0.0.1.0.0.255;8;2)", "Real Time Clock - Date and Time"); //same as Load Survey

            //reportColumns.Add("(1.0.31.7.0.255;3;2)", "CurrentIR");//same as Tamper
            //reportColumns.Add("(1.0.51.7.0.255;3;2)", "CurrentIY");
            //reportColumns.Add("(1.0.71.7.0.255;3;2)", "CurrentIB");
            //reportColumns.Add("(1.0.32.7.0.255;3;2)", "VoltageVRN");
            //reportColumns.Add("(1.0.52.7.0.255;3;2)", "VoltageVYN");
            //reportColumns.Add("(1.0.72.7.0.255;3;2)", "VoltageVBN");

            //reportColumns.Add("(1.0.33.7.0.255;3;2)", "Power Factor - R phase");
            //reportColumns.Add("(1.0.53.7.0.255;3;2)", "Power Factor - Y phase");
            //reportColumns.Add("(1.0.73.7.0.255;3;2)", "Power Factor - B phase");

            reportColumns.Add("(1.0.13.7.0.255;3;2)", "Power Factor - PF");
            reportColumns.Add("(1.0.14.7.0.255;3;2)", "Frequency");
            reportColumns.Add("(1.0.9.7.0.255;3;2)", "Apparent");
            reportColumns.Add("(1.0.1.7.0.255;3;2)", "Signed Active");
            reportColumns.Add("(1.0.3.7.0.255;3;2)", "Signed Reactive");

            //reportColumns.Add("(1.0.1.8.0.255;3;2)", "kWh");       //same as Billing
            //reportColumns.Add("(1.0.5.8.0.255;3;2)", "kvarhLag");
            //reportColumns.Add("(1.0.8.8.0.255;3;2)", "kvarhLead");
            //reportColumns.Add("(1.0.9.8.0.255;3;2)", "kVAh");

            reportColumns.Add("(0.0.96.7.0.255;1;2)", "Power - Failures");
            reportColumns.Add("(0.0.94.91.8.255;3;2)", "Power-Failure Duration");
            reportColumns.Add("(0.0.94.91.0.255;1;2)", "Cumulative Tamper");
            reportColumns.Add("(0.0.0.1.0.255;1;2)", "Cumulative Billing");
            reportColumns.Add("(0.0.96.2.0.255;1;2)", "Cumulative Programming");
            //reportColumns.Add("(0.0.0.1.2.255;3;2)", "BillDate"); //same as Billing

            //reportColumns.Add("(0.0.1.6.0.255;4;2)", "Demand - kW");
            //reportColumns.Add("(0.0.1.6.0.255;4;5)", "Demand - kW DateTime");
            //reportColumns.Add("(0.0.9.6.0.255;4;2)", "Demand - kVA");
            //reportColumns.Add("(0.0.9.6.0.255;4;5)", "Demand - kVA DateTime");

            //Load survey
            reportColumns.Add("(0.0.1.0.0.255;8;2)", "Real Time Clock");
            reportColumns.Add("(1.0.31.27.0.255;3;2)", "Current - R Phase");
            reportColumns.Add("(1.0.51.27.0.255;3;2)", "Current - Y Phase");
            reportColumns.Add("(1.0.71.27.0.255;3;2)", "Current - B Phase");
            reportColumns.Add("(1.0.32.27.0.255;3;2)", "Voltage - R Phase");
            reportColumns.Add("(1.0.52.27.0.255;3;2)", "Voltage - Y Phase");
            reportColumns.Add("(1.0.72.27.0.255;3;2)", "Voltage - B Phase");
            reportColumns.Add("(1.0.1.29.0.255;3;2)", "Block Energy - kWh");
            reportColumns.Add("(1.0.5.29.0.255;3;2)", "Block Energy - kvarh(lag)");
            reportColumns.Add("(1.0.8.29.0.255;3;2)", "Block Energy - kvarh(lead)");
            reportColumns.Add("(1.0.9.29.0.255;3;2)", "Block Energy - kVAh");

            //Billing
            reportColumns.Add("(0.0.0.1.2.255;3;2)", "Billing Date");
            reportColumns.Add("(1.0.13.0.0.255;3;2)", "PowerFactor");
            reportColumns.Add("(1.0.1.8.0.255;3;2)", "kWh");
            reportColumns.Add("(1.0.5.8.0.255;3;2)", "kvarh - lag");   //Cumulative Energy - kvarh - lag
            reportColumns.Add("(1.0.8.8.0.255;3;2)", "kvarh - lead");
            reportColumns.Add("(1.0.9.8.0.255;3;2)", "kVAh");
            reportColumns.Add("(1.0.1.6.0.255;4;5)", "kW DateTime");
            reportColumns.Add("(1.0.1.6.0.255;4;2)", "kW");
            reportColumns.Add("(1.0.9.6.0.255;4;5)", "kVA DateTime");
            reportColumns.Add("(1.0.9.6.0.255;4;2)", "kVA");           

            //Tamper
            reportColumns.Add("(0.0.1.0.0.255;8;3)", "Time");
            reportColumns.Add("(1.0.31.7.0.255;3;2)", "R Phase Current");
            reportColumns.Add("(1.0.51.7.0.255;3;2)", "Y Phase Current");
            reportColumns.Add("(1.0.71.7.0.255;3;2)", "B Phase Current");
            reportColumns.Add("(1.0.32.7.0.255;3;2)", "R Phase Voltage");
            reportColumns.Add("(1.0.52.7.0.255;3;2)", "Y Phase Voltage");
            reportColumns.Add("(1.0.72.7.0.255;3;2)", "B Phase Voltage");
            reportColumns.Add("(1.0.33.7.0.255;3;2)", "Signed Power Factor – R phase");
            reportColumns.Add("(1.0.53.7.0.255;3;2)", "Signed Power Factor – Y phase");
            reportColumns.Add("(1.0.73.7.0.255;3;2)", "Signed Power Factor – B phase");
            //reportColumns.Add("(1.0.1.8.0.255;3;2)", "EnergykWh");  //same as 3rd row in Billing Parameters

            //General
            reportColumns.Add("(0.0.96.1.1.255;1;2)", "Manufacturer");
            reportColumns.Add("(1.0.0.2.0.255;1;2)", "Version");
            reportColumns.Add("(0.0.94.91.9.255;1;2)", "Type");
            reportColumns.Add("(1.0.0.4.2.255;1;2)", "CT Ratio");
            reportColumns.Add("(1.0.0.4.3.255;1;2)", "PT Ratio");
            reportColumns.Add("(0.0.96.1.4.255;1;2)", "Year of Manuf");
            //reportColumns.Add("0.0.96.0.166.255;1;2", "Meter Model No.");
            reportColumns.Add("0.0.96.0.166.255;1;2", "Model - Type");

            return reportColumns;
        }

        public List<string> GetColumnsCollection()
        {
            List<string> selectedColumns = new List<string>();
            for (int colCount = 0; colCount < chkListSelectParameters.CheckedItems.Count; colCount++)
                selectedColumns.Add(chkListSelectParameters.CheckedItems[colCount].ToString());
            return selectedColumns;
        }

        private void MeterWise_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void MeterWise_Load(object sender, EventArgs e)
        {
            this.Size = new Size(375, 410);
            this.CenterToScreen();
            /* Midnight Energy changes */
            if (UtilityDetails.ShowMidnight)
            {
                radioBtnMidnightEnergies.Visible = true;
            }
            else
            {
                radioBtnMidnightEnergies.Checked = false;
                radioBtnMidnightEnergies.Visible = false;
            }
            /* Midnight Energy changes */
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            MeterSelectForm meterSelectForm = new MeterSelectForm();
            meterSelectForm.OnGridValue_Selection += new MeterSelectForm.GetValueColumn(meterSelectForm_OnGridValueSelection);
            meterSelectForm.ShowDialog();
        }

        private void btnMeterNext_Click_1(object sender, EventArgs e)
        {
            ////***To get consumer details in header****************//
            //DataSet tmpData = null;
            //tmpData = new MeterDataBLL().ListDataSet("MSN", MeterID, false);
            ////int index = lngGridAvailableMeters.SelectedIndex;
            // selectedMeterId = tmpData.Tables[0].Rows[0].ItemArray[1].ToString();

            //Get the selected row's file id from row id field of grid.
            selectedMeterId = lngGridAvailableFiles.SelectedRowId;
            //if (UtilityDetails.ShowMeterModelNo)
            //{
                DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
                meterModelNo = generalBLL.GetMeterModelNoByMeterID(MeterID);
            //}
            
            if (!ValidateMeterSelection())
                return;

            groupBoxSelectMeter.Visible = false;
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
            if (!UtilityDetails.ShowAnamolyParameters)
            {
                rdbSelfDiagnosis.Visible = false;
            }
        }

        private void btnMeterCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnParameterPrevious_Click(object sender, EventArgs e)
        {
            groupBoxSelectMeter.Visible = true;
            groupBoxParameterCategory.Visible = false;
            groupBoxSelectMeter.Location = new Point(9, 9);
        }

        private void btnParameterNext_Click(object sender, EventArgs e)
        {

            List<string> columnList = new List<string>();
            DataSet ds = new DataSet();

            groupBoxSelectMeter.Visible = false;
            groupBoxParameterCategory.Visible = false;
            if (radioBtnTamperParameter.Checked == false && radioBtnTransactionParameter.Checked == false)
            {
                groupBoxParameters.Visible = true;
                groupBoxParameters.Location = new Point(9, 9);
            }
            else
            {
                groupBoxTamperCategory.Visible = true;
                groupBoxTamperCategory.Location = new Point(9, 9);
            }

            if (radioBtnGeneralParameter.Checked || rdbSelfDiagnosis.Checked)
                lblNotification.Visible = false;
            else
                lblNotification.Visible = true;

            if (radioBtnInstantParameter.Checked == true)
            {
                columnList = GetColumns("Instant");
            }
            else if (radioBtnGeneralParameter.Checked == true)
                columnList = GetColumns("General");
            else if (radioBtnBillingParameter.Checked == true)
                columnList = GetColumns("Billing");
            else if (rdbSelfDiagnosis.Checked)
                columnList = GetColumns("Anomaly");
            else if (radioBtnLoadSurveyParameter.Checked == true)
                columnList = GetColumns("LoadSurvey");
            //added for MVVNL
            else if (radioBtnMidnightEnergies.Checked == true)
            {
                columnList = GetColumns("MidnightEnergies");
            }
            //added for MVVNL
            chkListSelectParameters.Items.Clear();

            if (radioBtnTamperParameter.Checked == false && radioBtnTransactionParameter.Checked == false)
            {
                foreach (string column in columnList)
                    chkListSelectParameters.Items.Add(column);
            }
            else if (radioBtnTamperParameter.Checked == true)
            {                
              /* VBM - Display voltagphasesequencereversal tamper only when utility has this fature */
              DataSet tamperData =   new TamperTypeBLL().ListDataSet(3);
              if (tamperData != null && tamperData.Tables.Count > 0)
                {
                    if (!UtilityDetails.ShowVoltagePhaseReversalTamper)
                    {                       
                        foreach (DataRow tamperRow in tamperData.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(tamperRow["TamperTypeID"]) == VoltagePhaseReversalOccurenceId)
                            {
                                tamperData.Tables[0].Rows.Remove(tamperRow);
                                tamperData.AcceptChanges();
                                break;
                            }                                                     
                        }
                        foreach (DataRow tamperRow in tamperData.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(tamperRow["TamperTypeID"]) == VoltagePhaseReversalRestorationId)
                            {
                                tamperData.Tables[0].Rows.Remove(tamperRow);
                                tamperData.AcceptChanges();
                                break;
                            }
                        }
                    }
                }
              /* VBM - Display voltagphasesequencereversal tamper only when utility has this fature */
                lngGridTamper.Data = tamperData;
                lngGridTamper.SetWidth("SNo", 40);
                lngGridTamper.SetWidth("TamperType", 225);
                lngGridTamper.HiddenColumn = "TamperTypeID";
                lngGridTamper.ValueColumn = "TamperTypeID";
                lngGridTamper.IsSorting = false;
                lngGridTamper.RefreshGrid();
                lngGridTamper.SelectedIndex = 0;
            }
            else if (radioBtnTransactionParameter.Checked == true)
            {
                DataSet trnsData = new TamperTypeBLL().ListDataSet(4);
                /* VBM - Display kvahSelectionTamper only when utility has this fature */
                if (trnsData != null && trnsData.Tables.Count > 0)
                {
                    if (!UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                    {
                        foreach (DataRow trnsRow in trnsData.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(trnsRow["TamperTypeID"]) == kVAhSelectionTamperId)
                            {
                                trnsData.Tables[0].Rows.Remove(trnsRow);
                                trnsData.AcceptChanges();
                                break;
                            }
                        }
                    }

                    DataView view = new DataView(trnsData.Tables[0]);
                    StringBuilder filterCondition = new StringBuilder();
                    if (!UtilityDetails.ShowMDResetTamper)
                    {
                        filterCondition.Append("TamperTypeID =" + MDReset);
                    }
                    if (UtilityDetails.DisableProgrammingBillingDateTime)
                    {
                        if (filterCondition.Length > 0)
                        {
                            filterCondition.Append(" OR TamperTypeID =" + SingleActionScheduleForBillingDates);
                        }
                        else
                        {
                            filterCondition.Append("TamperTypeID =" + SingleActionScheduleForBillingDates);
                        }
                    }

                    if (UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                    {
                        if (filterCondition.Length > 0)
                        {
                            filterCondition.Append(" OR TamperTypeID =" + ProfileCapturePeriod);
                        }
                        else
                        {
                            filterCondition.Append("TamperTypeID =" + ProfileCapturePeriod);
                        }
                    }
                    if (UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                    {
                        if (filterCondition.Length > 0)
                        {
                            filterCondition.Append(" OR TamperTypeID =" + DemandIntegrationPeriod);
                        }
                        else
                        {
                            filterCondition.Append("TamperTypeID =" + DemandIntegrationPeriod);
                        }
                    }
                    if (!UtilityDetails.ShowDisplayParemeterTamper)
                    {
                        if (filterCondition.Length > 0)
                        {
                            filterCondition.Append("OR TamperTypeID =" + ScrollTimeConfig + " OR TamperTypeID =" + ScrollModeConfig
                                              + " OR TamperTypeID =" + PushModeConfig + " OR TamperTypeID =" + HRModeConfig);
                        }
                        else
                        {
                            filterCondition.Append("TamperTypeID =" + ScrollTimeConfig + " OR TamperTypeID =" + ScrollModeConfig
                                              + " OR TamperTypeID =" + PushModeConfig + " OR TamperTypeID =" + HRModeConfig);
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
                    trnsData.AcceptChanges(); 
                }
            /* VBM - Display kvahSelectionTamper only when utility has this fature */
                lngGridTamper.Data = trnsData;
                lngGridTamper.SetWidth("SNo", 40);
                lngGridTamper.SetWidth("TamperType", 230);
                lngGridTamper.HiddenColumn = "TamperTypeID";
                lngGridTamper.ValueColumn = "TamperTypeID";
                lngGridTamper.IsSorting = false;
                lngGridTamper.RefreshGrid();
                lngGridTamper.SelectedIndex = 0;
            }
        }

        private void btnParameterCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTamperPrevious_Click(object sender, EventArgs e)
        {
            groupBoxSelectMeter.Visible = false;
            groupBoxParameters.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
        }

        private void btnTamperCategoryNext_Click(object sender, EventArgs e)
        {
            List<string> snapshotColumns = new List<string>();
            selectedTamperParameter = lngGridTamper.Data.Tables[0].Rows[lngGridTamper.SelectedIndex]["TamperType"].ToString();
            selectedTamperCode = Convert.ToInt32(lngGridTamper.Data.Tables[0].Rows[lngGridTamper.SelectedIndex]["TamperTypeID"].ToString());
            snapshotColumns = GetColumns("TamperSnapshot");
            chkListSelectParameters.Items.Clear();
            if (selectedTamperCode == 101 || selectedTamperCode == 102)
            {
                chkListSelectParameters.Items.Add(snapshotColumns[0]);
                lblNotification.Visible = false;
            }
            else
            {
                foreach (string column in snapshotColumns)
                    chkListSelectParameters.Items.Add(column);
                lblNotification.Visible = true;

            }



            groupBoxSelectMeter.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = true;
            groupBoxParameters.Location = new Point(9, 9);
        }

        private void btnTamperCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReportParamsPrevious_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (radioBtnTamperParameter.Checked == false)
            {
                //making the relevant group to be visible
                groupBoxSelectMeter.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxParameterCategory.Visible = true;
                groupBoxParameterCategory.Location = new Point(9, 9);
            }
            else
            {
                //making the relevant group to be visible
                groupBoxSelectMeter.Visible = false;
                groupBoxParameterCategory.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxTamperCategory.Visible = true;
                groupBoxTamperCategory.Location = new Point(9, 9);

            }
        }

        private void btnReportParamsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!ValidateParameters())
                return;

            if (radioBtnGeneralParameter.Checked == true)
                ShowReport("General");
            else if (rdbSelfDiagnosis.Checked == true)
                ShowReport("Anomaly");
            else if (radioBtnInstantParameter.Checked == true)
                ShowReport("Instant");
            else if (radioBtnBillingParameter.Checked == true)
                ShowReport("Billing");
            else if (radioBtnLoadSurveyParameter.Checked == true)
                ShowReport("LoadSurvey");
            else if (radioBtnTamperParameter.Checked == true)
                ShowReport("Tamper");
            else if (radioBtnTransactionParameter.Checked == true)
                ShowReport("Transaction");
            //added for MVVNL
            else if (radioBtnMidnightEnergies.Checked == true)
            {
                ShowReport("MidnightEnergies");
            }
            //added for MVVNL

            Cursor.Current = Cursors.Default;
        }

        private void chkListSelectParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSelectParameters.CheckedIndices.Count > 8)
            {
                chkListSelectParameters.SetItemChecked(chkListSelectParameters.SelectedIndex, false);
                MessageBox.Show("A maximum of 8 parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MeterWise_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = String.Empty;
        }


        //private void btnShowReport_Click(object sender, EventArgs e)
        //{
        //    DataSet detailsDS = new DataSet();
        //    DataSet meterIDDS = new DataSet();

        //    detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //    if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
        //        FillConsumerMeterDetails(detailsDS);
        //    else
        //    {
        //        meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
        //            FillMeterID(meterIDDS);
        //    }

        //    if (radioBtnGeneralParameter.Checked == true)
        //    {
        //        DataSet generalDS = new DataSet();
        //        generalDS = ListGeneralData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (generalDS != null && generalDS.Tables[0].Rows.Count > 0)
        //            FillGeneralXSD(generalDS);
        //    }
        //    if (radioBtnInstantParameter.Checked == true)
        //    {
        //        DataSet instantDS = new DataSet();
        //        instantDS = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (instantDS != null && instantDS.Tables[0].Rows.Count > 0)
        //            FillInstantXSD(instantDS);

        //        DataSet phasorDS = new DataSet();
        //        phasorDS = ListPhasorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (phasorDS != null && phasorDS.Tables[0].Rows.Count > 0)
        //        {
        //            FillPhasorXSD(phasorDS);
        //            FillPhasorDigram(phasorDS);
        //        }
        //    }
        //    if (radioBtnBillingParameter.Checked == true)
        //    {
        //        //Power Factor
        //        DataSet powerFactorDS = new DataSet();
        //        powerFactorDS = ListPowerFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (powerFactorDS != null && powerFactorDS.Tables[0].Rows.Count > 0)
        //            FillPowerFactorXSD(powerFactorDS);

        //        DataSet tariffPowerFactorDS = new DataSet();
        //        tariffPowerFactorDS = GetTariffPF(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (tariffPowerFactorDS != null && tariffPowerFactorDS.Tables[0].Rows.Count > 0)
        //            FillTariffPowerFactorXSD(tariffPowerFactorDS);

        //        //Power On Hours
        //        DataSet powerOnHoursDS = new DataSet();
        //        powerOnHoursDS = ListPowerOnHoursData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (powerOnHoursDS != null && powerOnHoursDS.Tables[0].Rows.Count > 0)
        //            FillPowerOnHoursXSD(powerOnHoursDS);

        //        //Billing Tamper Counter
        //        DataSet billingTamperCounterDS = new DataSet();
        //        billingTamperCounterDS = ListBillingTamperCounterData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (billingTamperCounterDS != null && billingTamperCounterDS.Tables[0].Rows.Count > 0)
        //            FillBillingTamperCounterXSD(billingTamperCounterDS);

        //        //Main Energy
        //        DataSet mainEnergyDS = new DataSet();
        //        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
        //            FillMainEnergyXSD(mainEnergyDS);

        //        //Energy Consumption
        //        mainEnergyDS = new DataSet();
        //        mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
        //            FillEnergyConsumptionXSD(mainEnergyDS);

        //        //CT Ratio
        //        DataSet ctRatioDS = new DataSet();
        //        ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
        //            FillCTRatioXSD(ctRatioDS);

        //        // MD
        //        DataSet maximumDemandDS = new DataSet();
        //        maximumDemandDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (maximumDemandDS != null && maximumDemandDS.Tables[0].Rows.Count > 0)
        //            FillMaximumDemandXSD(maximumDemandDS);

        //        DataSet TODMDDS = new DataSet();
        //        for (int historyID = 0; historyID <= 12; historyID++)
        //        {
        //            TODMDDS = ListTODMDData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
        //            if (TODMDDS != null && TODMDDS.Tables[0].Rows.Count > 0)
        //                FillTODMDXSD(TODMDDS, historyID);
        //        }

        //        //TOD Energy
        //        DataSet tariffEnergyDS = new DataSet();
        //        for (int historyID = 0; historyID <= 12; historyID++)
        //        {
        //            tariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
        //            if (tariffEnergyDS != null && tariffEnergyDS.Tables[0].Rows.Count > 0)
        //                FillTariffEnergyXSD(tariffEnergyDS, historyID);
        //        }

        //        //Billing Mechanism
        //        ctRatioDS = new DataSet();
        //        ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
        //            FillBillingMechanismXSD(ctRatioDS);

        //        //TOD Consumption
        //        DataSet currentTariffEnergyDS = new DataSet();
        //        DataSet nextTariffEnergyDS = new DataSet();
        //        for (int historyID = 0; historyID < 12; historyID++)
        //        {
        //            if (historyID == 0)
        //                currentTariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
        //            else
        //                currentTariffEnergyDS = nextTariffEnergyDS;

        //            nextTariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID + 1);
        //            if (nextTariffEnergyDS == null)
        //                break;
        //            if (currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
        //                FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
        //        }

        //        //Transactions
        //        DataSet transactionDS = new DataSet();
        //        transactionDS = ListTransactionData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (transactionDS != null && transactionDS.Tables[0].Rows.Count > 0)
        //            FillTransactionXSD(transactionDS);

        //        DataSet rtcUpdatesDS;

        //        int totalRTCUpdates = new RTCUpdateBLL().GetTotalRTCUpdates(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        rtcUpdatesDS = ListRTCUpdatesData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (rtcUpdatesDS != null && rtcUpdatesDS.Tables[0].Rows.Count > 0)
        //            FillRTCUpdatesXSD(rtcUpdatesDS, totalRTCUpdates);

        //        FraudEnergyEntity entity = ListFraudEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as FraudEnergyEntity;
        //        FillFraudEnergyXSD(entity);
        //    }
        //    if (radioBtnLoadSurveyParameter.Checked == true)
        //    {
        //        DataSet dtmLoadSurveyDS = new DataSet();
        //        dtmLoadSurveyDS = ListDTMLoadSurveyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        if (dtmLoadSurveyDS != null && dtmLoadSurveyDS.Tables[0].Rows.Count > 0)
        //            FillDTMLoadSurveyXSD(dtmLoadSurveyDS);
        //    }
        //    if (radioBtnTamperParameter.Checked == true)
        //    {
        //        DataSet tamperDS = new DataSet();
        //        DataSet tamperCounterDS = new DataSet();
        //        DataSet tamperDetailsDset = new DataSet();
        //        tamperCounterDS = ListTamperData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        tamperDS = ListTamperOccResData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
        //        string tamperCounter = string.Empty;
        //        string occurranceTime = string.Empty;
        //        string restorationTime = string.Empty;
        //        if (tamperDS != null && tamperDS.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow drow in tamperDS.Tables[0].Rows)
        //            {
        //                foreach (DataRow row in tamperCounterDS.Tables[0].Rows)
        //                {
        //                    if (row["TamperType"].ToString() == drow["Description"].ToString())
        //                    {
        //                        tamperCounter = row["TamperCounter"].ToString();
        //                        break;
        //                    }
        //                }
        //                occurranceTime = drow["Occurrence Time Stamp"].ToString();
        //                restorationTime = drow["Restoration Time Stamp"].ToString();
        //                tamperDetailsDset = GetTamperOccurRestoreDetail(Convert.ToInt16(drow["TamperSnapShot_ID"].ToString()), Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
        //                FillAllTamperXSD(tamperDetailsDset, drow["Description"].ToString(), tamperCounter, occurranceTime, restorationTime);
        //            }
        //        }
        //    }
        //    ShowReport();
        //}
    }
}
