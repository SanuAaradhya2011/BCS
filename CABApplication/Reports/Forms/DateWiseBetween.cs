using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CAB.BLL;
//using CABApplication.Reports.DaterwiseReports;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using CAB.Framework;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using System.Text;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class DateWiseBetween : CABForm
    {
        private string FileName;
        private string meterID;
        private int selectedTamperCode;
        private string selectedTamperParameter;
        const string dateUnavailable = "--------";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        FileReportDataSet reportXSD;
        private Dictionary<string, string> reportColumns = new Dictionary<string, string>();
        DLMS650CommonBLL dlms650CommonBLL;
        DateTime fromDate = DateTime.MinValue;
        DateTime toDate = DateTime.MinValue;
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        string  SelectedMeterDataId;
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
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DateWiseBetween).ToString());
        public DateWiseBetween()
        {
            //Utility check added; 24th April 2012; Bug 75902   
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (UtilityEntity.MVVNL == UtilityDetails.Utility)
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

        private void btnSelectDateCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
            
        }

        private void btnAvailableMeterCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
          
        }

        private void btnTamperCategoryCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnParametersCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnParameterCategoryCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnSelectDateNext_Click(object sender, EventArgs e)
        {
            // Added validationdates() check.
            if (ValidateDates())
            {
                if (lngGridSelectFiles.Data == null || lngGridSelectFiles.Data.Tables[0].Rows.Count == 0)
                {
                    this.StatusMessage = "No files present.";
                    return;
                }
                FileName = lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["FileName"].ToString();
                //BhardwajG : If show meter model no then get the meter model no value if the value is not equal to dbnull
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    if (lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["MeterModelNo"] != DBNull.Value)
                    {
                        int.TryParse(lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["MeterModelNo"].ToString(), out meterModelNo);
                    }
               // }
                FillAvailableMeters(FileName);
                meterID = new MeterDataBLL().ListDataSet("CABF", FileName, false).Tables[0].Rows[0][2].ToString();
                groupBoxAvailableMeters.Visible = true;
                groupBoxAvailableMeters.Location = new Point(9, 9);
                groupBoxSelectFile.Visible = false;
                groupBoxTamperCategory.Visible = false;
                groupBoxParameterCategory.Visible = false;
                groupBoxParameters.Visible = false;
                if (!UtilityDetails.ShowAnamolyParameters)
                {
                    rdbSelfDiagnosis.Visible = false;
                }
            }
            else
                return;
        }

        private void FillAvailableMeters(string fileName)
        {
            lngGridAvailableMeters.Data = new MeterDataBLL().ListDataSet("CABF", fileName,false);
            lngGridAvailableMeters.HiddenColumns =  "MeterData_ID,Reading DateTime";
            lngGridAvailableMeters.ValueColumn="Meter Number";
            lngGridAvailableMeters.SetWidth("S.No", 55);
            lngGridAvailableMeters.SetWidth("Meter Number", 230);
            lngGridAvailableMeters.RefreshGrid();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ValidateDates() == true)
                GetCABFilesBetweenDates(dtPickerFromDate.Value, dtPickerToDate.Value);
            else
            {
                lngGridSelectFiles.Data = null;
                return;
            }

            if (lngGridSelectFiles.Data.Tables[0].Rows.Count > 0)
                btnSelectDateNext.Enabled = true;
            else
                btnSelectDateNext.Enabled = false;
        }

        private bool ValidateDates()
        {
            if (dtPickerFromDate.Value.Date > dtPickerToDate.Value.Date)
            {
                this.StatusMessage = "To Date should be greater than From Date";
                return false;
            }
            else
            {
                this.StatusMessage = "";
                return true;
            }
        }

        private void GetCABFilesBetweenDates(DateTime dtFromDate, DateTime dtToDate)
        {
            fromDate = dtFromDate;
            fromDate = dtFromDate.Subtract(fromDate.TimeOfDay);
            toDate = dtToDate.Subtract(toDate.TimeOfDay);
            toDate = dtToDate.Add(new TimeSpan(0, 59, 59));
            lngGridSelectFiles.Data = new FileUploadMasterBLL().GetCABFileNamesBetweenDates(fromDate, toDate);
            lngGridSelectFiles.SetWidth("S.No", 55);
            lngGridSelectFiles.SetWidth("FileName", 235);
            // Hide Meter Model No as well if meter model number is required
            //if (UtilityDetails.ShowMeterModelNo)
            //{
                lngGridSelectFiles.HiddenColumn = "FileUpload_ID,MeterModelNo";
            //}
            //else
            //{
            //    lngGridSelectFiles.HiddenColumn = "FileUpload_ID";
            //}
            lngGridSelectFiles.ValueColumn = "FileName";
            lngGridSelectFiles.IsSorting = false;
            lngGridSelectFiles.RefreshGrid();
        }

        private void btnAvailableMeterNext_Click(object sender, EventArgs e)
        {
            //***To get consumer details in header****************//
            DataSet tmpData = null;
            tmpData = new MeterDataBLL().ListDataSet("CABF", FileName, false);
            //tmpData = new MeterDataBLL().ListDataSet("MSN", FileName, false);
            SelectedMeterDataId = tmpData.Tables[0].Rows[0].ItemArray[1].ToString();


            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
            groupBoxSelectFile.Visible = false;
            groupBoxAvailableMeters.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = false;
        }

        private void btnAvailableMeterPrevious_Click(object sender, EventArgs e)
        {
            groupBoxSelectFile.Visible = true;
            groupBoxSelectFile.Location = new Point(9, 9);
            groupBoxAvailableMeters.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxParameters.Visible = false;
        }

        private void btnTamperCategoryPrevious_Click(object sender, EventArgs e)
        {
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
            groupBoxSelectFile.Visible = false;
            groupBoxAvailableMeters.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = false;
        }

        private void btnTamperCategoryNext_Click(object sender, EventArgs e)
        {
            //***To get consumer details in header****************//
            DataSet tmpData = null;
            tmpData = new MeterDataBLL().ListDataSet("CABF", FileName, false);
            //tmpData = new MeterDataBLL().ListDataSet("MSN", FileName, false);
            SelectedMeterDataId = tmpData.Tables[0].Rows[0].ItemArray[1].ToString();


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

            groupBoxSelectFile.Visible = false;
            groupBoxAvailableMeters.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = true;
            groupBoxParameters.Location = new Point(9, 9);
        }

        private List<string> GetColumns(string paramType)
        {
            bool isPUMAMeter = false;
            Dictionary<string, string> columnDictionary = new Dictionary<string, string>();
            List<string> columns = new List<string>();

            if (paramType == "General")
            {
                //columnDictionary = new GeneralBLL().CreateGeneralDictionary();
                columnDictionary = new DLMS650GeneralBLL().CreateGeneralDictionary();
            }
            else if (paramType == "Anomaly")
            {
                //columnDictionary = new InstantPowerBLL().CreateInstantDictionary();
                columnDictionary = new AnomalyBLL().CreateAnomalyDictionary();
            }
            else if (paramType == "Instant")
            {
                //BhardwajG : If meter model no is enabled , fetch the instant dictionary accordingly as Ruby and PUMA meters both can be present 
                //columnDictionary = new InstantPowerBLL().CreateInstantDictionary();
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    if (meterModelNo == NamePlateConstants.PumaLTE650Value || meterModelNo == NamePlateConstants.PumaHTE650Value)
                    {
                        isPUMAMeter = true;
                    }
                    columnDictionary = new DLMS650InstantaneousBLL().CreateInstantDictionary(SelectedMeterDataId, isPUMAMeter);
                //}
                //else
                //{
                //    columnDictionary = new DLMS650InstantaneousBLL().CreateInstantDictionary(SelectedMeterDataId);
                //}
            }
            else if (paramType == "Billing")
            {
                //columnDictionary = new BillingBLL().CreateBillingDictionary();
                columnDictionary = new DLMS650BillingBLL().CreateBillingDictionary(SelectedMeterDataId);

            }
            else if (paramType == "LoadSurvey")
            {   //columnDictionary = new LoadSurveyBLL().CreateLoadSurveyDictionary();
                columnDictionary = new DLMS650LoadSurveyBLL().CreateLoadSurveyDictionary(SelectedMeterDataId);
            }
            else if (paramType == "TamperSnapshot")
            {
                columnDictionary = new DLMS650TamperMasterBLL().CreateTamperDictionary(SelectedMeterDataId);
            }
                //added for MVVNL
            else if (paramType == "MidnightEnergies")
            {
                lblNotification.Visible = false;
                columnDictionary = new DLMS650MidnightDataBLL().CreateMidnightDataDictionary(SelectedMeterDataId);
            }
            //added for MVVNL

            foreach (KeyValuePair<string, string> kvp in columnDictionary)
                columns.Add(kvp.Key);

            return columns;
        }

        private void btnParameterCategoryNext_Click(object sender, EventArgs e)
        {
            List<string> columnList = new List<string>();
            DataSet ds = new DataSet();
             groupBoxSelectFile.Visible = false;
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

            if (radioBtnGeneralParameter.Checked == true)
                columnList = GetColumns("General");//CommonBLL.GetColumns("General");
            else if (rdbSelfDiagnosis.Checked == true)
                columnList = GetColumns("Anomaly");//CommonBLL.GetColumns("Instant");
            else if (radioBtnInstantParameter.Checked == true)
                columnList = GetColumns("Instant");//CommonBLL.GetColumns("Instant");
            else if (radioBtnBillingParameter.Checked == true)
                columnList = GetColumns("Billing");//CommonBLL.GetColumns("Billing");
            else if (radioBtnLoadSurveyParameter.Checked == true)
                columnList = GetColumns("LoadSurvey"); //CommonBLL.GetColumns("LoadSurvey");
            //added for MVVNL
            else if (radioBtnMidnightEnergy.Checked == true)
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
                lngGridTamper.Data = new TamperTypeBLL().ListDataSet(3);
                /* VBM - Display voltagphasesequencereversal tamper only when utility has this fature */
                DataSet tamperData = new TamperTypeBLL().ListDataSet(3);
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
                lngGridTamper.SetWidth("TamperType", 230);
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

        private void btnParameterCategoryPrevious_Click(object sender, EventArgs e)
        {
            groupBoxAvailableMeters.Visible = true;
            groupBoxAvailableMeters.Location = new Point(9, 9);
            groupBoxSelectFile.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxParameters.Visible = false;
        }

        private void btnParametersPrevious_Click(object sender, EventArgs e)
        {
            groupBoxSelectFile.Visible = false;
            groupBoxAvailableMeters.Visible = false;
            if (radioBtnTamperParameter.Checked == false)
            {
                groupBoxTamperCategory.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxParameterCategory.Visible = true;
                groupBoxParameterCategory.Location = new Point(9, 9);
            }
            else
            {
                groupBoxParameterCategory.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxTamperCategory.Visible = true;
                groupBoxTamperCategory.Location = new Point(9, 9);
            }
        }

        private void DateWiseBetween_Load(object sender, EventArgs e)
        {
            this.Size = new Size(375, 410);
            this.CenterToScreen();
            btnSelectDateNext.Enabled = false;
            /* VBM - Midnight energy chnages */
            if (UtilityDetails.ShowMidnight)
            {
                radioBtnMidnightEnergy.Visible = true;
            }
            else
            {
                radioBtnMidnightEnergy.Checked = false;
                radioBtnMidnightEnergy.Visible = false;
            }
            /* VBM - Midnight energy chnages */
        }

        private void chkListSelectParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSelectParameters.CheckedIndices.Count > 8)
            {
                chkListSelectParameters.SetItemChecked(chkListSelectParameters.SelectedIndex, false);
                MessageBox.Show("Maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowParameters_Click(object sender, EventArgs e)
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
            else if (radioBtnMidnightEnergy.Checked == true)
                ShowReport("MidnightEnergies");
            //added for MVVNL

            Cursor.Current = Cursors.Default;
        }

        private bool ValidateParameters()
        {
            if (chkListSelectParameters.CheckedItems.Count > 10)
            {
                this.StatusMessage = "Please Select 10 Parameters only";
                return false;
            }
            else if (chkListSelectParameters.CheckedItems.Count == 0)
            {
                this.StatusMessage = "Please Select atleast one Parameter";
                return false;
            }
            return true;
        }

        private void FillMeterID(string meterId)
        {
            DataRow reportRow;
            if (meterId != null)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                //foreach (DataRow row in meterIdDS.Tables[0].Rows)
                //{
                //    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                //        reportRow["MeterNo"] = row["MeterID"].ToString();
                //    else
                //        reportRow["MeterNo"] = dateUnavailable;
                //}
                reportRow["MeterNo"] = meterId;
                reportRow["ActiveMeter"] = "Inactive";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }

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

        private void FillConsumerMeterDetailsTextObject(DataSet detailsDset, DWDateWiseReport dWReport)
        {
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterType = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextMeterType"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterModel = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextMeterModel"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextEMF = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextEMF"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextContractDemand = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextContractDemand"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextInstallationDate = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextInstallationDate"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextLocation = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextLocation"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextConsumerID = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextConsumerID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCMRINumber = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextCMRINumber"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextRegion = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextRegion"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCircle = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextCircle"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextDivision = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextDivision"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)dWReport.ReportDefinition.ReportObjects["TextActiveMeter"];

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString()))
                if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString()))
                    TextMeterType.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString());
                else
                    TextMeterType.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterModel_Name"].ToString()))
                TextMeterModel.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterModel_Name"].ToString());
            else
                TextMeterModel.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_EMF"].ToString()))
            {
                /* VBM - EMF Bug Fixed */
                string meterEMF = CommonBLL.CalculateActualEMF(Convert.ToDecimal(detailsDset.Tables[0].Rows[0]["Meter_EMF"].ToString()),
                                                                 detailsDset.Tables[0].Rows[0]["internalCTRatio"].ToString(),
                                                                 detailsDset.Tables[0].Rows[0]["internalPTRatio"].ToString());
                /* VBM - EMF Bug Fixed */
                string emfApplied = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["UseEMFInCalculations"].ToString());
                if (emfApplied == "1")
                {
                    emfApplied = APPLIED;
                }
                else
                {
                    emfApplied = NOTAPPLIED;
                }
                meterEMF = meterEMF + " (" + emfApplied + ")";
                TextEMF.Text = meterEMF;
            }
            else
                TextEMF.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_ContractDemand"].ToString()))
                TextContractDemand.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_ContractDemand"].ToString());
            else
                TextContractDemand.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_AllocationDate"].ToString()))
                TextInstallationDate.Text = DateUtility.LongToDateTime(Convert.ToInt64(detailsDset.Tables[0].Rows[0]["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
            else
                TextInstallationDate.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_Location"].ToString()))
                TextLocation.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_Location"].ToString());
            else
                TextLocation.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Consumer_Number"].ToString()))
                TextConsumerID.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Consumer_Number"].ToString());
            else
                TextConsumerID.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["CMRI_Number"].ToString()))
                TextCMRINumber.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["CMRI_Number"].ToString());
            else
                TextCMRINumber.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Region_Name"].ToString()))
                TextRegion.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Region_Name"].ToString());
            else
                TextRegion.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Circle_Name"].ToString()))
                TextCircle.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Circle_Name"].ToString());
            else
                TextCircle.Text = dateUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Division_Name"].ToString()))
                TextDivision.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Division_Name"].ToString());
            else
                TextDivision.Text = dateUnavailable;

            if (detailsDset.Tables[0].Rows[0]["Status"].ToString().Equals("0"))
                TextActiveMeter.Text = "Inactive";
            else
                TextActiveMeter.Text = "Active";

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
                        reportRow["EMF"] = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
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
                    //reportXSD.AcceptChanges();
                }
            }
        }

        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }

        private void ShowReport(string reportType)
        {
            DWDateWiseReport dWDateWiseReport = new DWDateWiseReport();
            List<string> columnList = new List<string>();
            DataRow reportRow;
            reportXSD = new FileReportDataSet();

            try
            {
                DatabaseReportForm databaseReportForm = new DatabaseReportForm();
                DataSet ds = new DataSet();

                columnList = GetColumnsCollection();

                //***To get consumer details in header****************//
                DataSet tmpData = null;
                tmpData = new MeterDataBLL().ListDataSet("CABF", FileName, false);
                //tmpData = new MeterDataBLL().ListDataSet("MSN", FileName, false);
                string activeMeterDataId = tmpData.Tables[0].Rows[0].ItemArray[1].ToString();

                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(activeMeterDataId));
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    //FillConsumerMeterDetails(detailsDS);
                    FillConsumerMeterDetailsTextObject(detailsDS, dWDateWiseReport);
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(activeMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    {
                        FillMeterID(meterIDDS);
                        // Added to solve Inactive not updated in status field of report. 
                        CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["TextActiveMeter"];
                        TextActiveMeter.Text = "Inactive";
                     

                    }
                }
                //***To get consumer details in header****************//
                string selectedID = lngGridAvailableMeters.Data.Tables[0].Rows[lngGridAvailableMeters.SelectedIndex][2].ToString();
                if (selectedID != null) FillMeterID(selectedID);
                CrystalDecisions.CrystalReports.Engine.TextObject TextFromDate = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["TextFromDate"];
                //TextFromDate.Text = dtPickerFromDate.Value.ToString(ConfigInfo.DateFormat());
                TextFromDate.Text = fromDate.ToString("dd/MM/yyyy");

                CrystalDecisions.CrystalReports.Engine.TextObject TextToDate = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["TextToDate"];
                //TextToDate.Text = dtPickerToDate.Value.ToString(ConfigInfo.DateFormat());
                //toDate = toDate.Subtract(new TimeSpan(23, 59, 59));
                TextToDate.Text = toDate.ToString("dd/MM/yyyy");
                /* VBM - Add BCS Version number in report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["txtBCSVersion"];                
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* VBM - Add BCS Version number in report header */
                CrystalDecisions.CrystalReports.Engine.TextObject TextFileName = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["TextFileName"];
                TextFileName.Text = FileName;

                CrystalDecisions.CrystalReports.Engine.TextObject TxtHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["txtReportType"];

                //Find static text object txtMidNight. Displaying static text "Midnight value are of 00:00hrs". 
                CrystalDecisions.CrystalReports.Engine.TextObject txtMidNight = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["txtMidNight"];
                //Statis text will be visible only for Midnight report.
                txtMidNight.Width = 0;  

                if (reportType == "General")
                {
                    TxtHeading.Text = "General Details";
                    //ds = new GeneralBLL().GetGeneralDataByFileName(selectedID, FileName, columnList, "CAB");
                    //if (UtilityDetails.ShowMeterModelNo)
                    //{
                        ds = new DLMS650GeneralBLL().GetGeneralDataByFileName(meterID, FileName, columnList,meterModelNo);
                    //}
                    //else
                    //{
                    //    ds = new DLMS650GeneralBLL().GetGeneralDataByFileName(meterID, FileName, columnList);
                    //}
        
                }
                else if (reportType == "Anomaly")
                {
                    TxtHeading.Text = "Self Diagnostics Report";
                    AnomalyBLL objAnomalyBLL = new AnomalyBLL();
                    objAnomalyBLL.fileName = FileName;
                    ds = objAnomalyBLL.GetAnomalyDataByParameter(meterID, columnList);
                }     
                else if (reportType.Equals("Instant"))
                {
                    TxtHeading.Text = "Instantaneous Details";                    
                    DLMS650InstantaneousBLL dLMS650InstantaneousBLL = new DLMS650InstantaneousBLL();
                    dLMS650InstantaneousBLL.fileName = FileName;
                    //ds = new InstantPowerBLL().GetInstantDataByFileName(selectedID, FileName, columnList, "CAB");
                    ds = dLMS650InstantaneousBLL.GetInstantDataByParameter(meterID, columnList, activeMeterDataId);
                    dLMS650InstantaneousBLL.fileName = string.Empty;
                }
                else if (reportType == "Billing")
                {
                    TxtHeading.Text = "Billing Details";
                    //ds = new BillingBLL().GetBillingDataByFileName(selectedID, FileName, columnList, "CAB");
                    ds = new DLMS650BillingBLL().GetBillingDataByFileName(meterID, FileName, columnList);
                }
                else if (reportType == "LoadSurvey")
                {
                    TxtHeading.Text = "Load Survey Details";
                    //   ds = new LoadSurveyBLL().GetLoadSurveyDataByFileName(selectedID, FileName, columnList, "CAB");
                    ds = new DLMS650LoadSurveyBLL().GetLoadSurveyDataByFileName(meterID, FileName, columnList);
                }
                else if (reportType == "Tamper")
                {
                    TxtHeading.Text = "Tamper Details";
                    //ds = new TamperSnapShotBLL().GetTamperSnapshotDataByFileName(selectedID, FileName, columnList, selectedTamperCode, "CAB");
                    ds = new DLMS650TamperMasterBLL().GetTamperSnapshotDataByFileName(meterID, FileName, columnList, selectedTamperCode);

                    CrystalDecisions.CrystalReports.Engine.TextObject TamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["tamperName"];
                    TamperHeading.Text = selectedTamperParameter;
                    CrystalDecisions.CrystalReports.Engine.TextObject TamperCode = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["tamperCode"];
                    TamperCode.Text = selectedTamperCode.ToString();

                }
                else if (reportType == "Transaction")
                {
                    TxtHeading.Text = "Transaction Details";
                    ds = new DLMS650TamperMasterBLL().GetTransactionSnapShotDataByFileName(meterID, FileName, columnList, selectedTamperCode);

                    CrystalDecisions.CrystalReports.Engine.TextObject TamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["tamperName"];
                    TamperHeading.Text = selectedTamperParameter;
                    CrystalDecisions.CrystalReports.Engine.TextObject TamperCode = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["tamperCode"];
                    TamperCode.Text = selectedTamperCode.ToString();

                }
                //added for MVVNL    
                else if (reportType == "MidnightEnergies")
                {
                    TxtHeading.Text = "Midnight Energies";

                    //Set the width from 0 to 2280 to display the static text object.
                    txtMidNight.Width = 2280;

                    ds = new DLMS650MidnightDataBLL().GetMidnightEnergiesByDateWiseReport(meterID, FileName, columnList);
                }
                //added for MVVNL

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    if (reportType.Equals("General"))
                        this.StatusMessage = "General data not available.";
                    else if (reportType.Equals("Anomaly"))
                        this.StatusMessage = "Self Diagnostics data not available.";
                    else if (reportType.Equals("Instant"))
                        this.StatusMessage = "Instant data not available.";
                    else if (reportType.Equals("Billing"))
                        this.StatusMessage = "Billing data not available.";
                    else if (reportType.Equals("LoadSurvey"))
                        this.StatusMessage = "Load survey data not available.";
                    else if (reportType.Equals("Tamper"))
                        this.StatusMessage = "Tamper data not available.";
                    else if (reportType.Equals("Transaction"))
                        this.StatusMessage = "Transaction data not available.";
                    //added for MVVNL
                    else if (reportType.Equals("MidnightEnergies"))
                        this.StatusMessage = "Midnight Energies not available.";
                    //added for MVVNL

                    return;
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataTable dtx = reportXSD.Tables["MDataReportsTable"];
                    DataRow drx = dtx.NewRow();
                    reportXSD.Tables.Remove("MDataReportsTable");
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                            drx["MeterNo"] = row[col];
                        else if (col.Ordinal == 1)
                            drx["FileName"] = row[col];
                        else if (col.Ordinal == 2)
                            drx["ReadingDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                        else
                        {
                            if (CommonBLL.IsTimeColumn(col.ColumnName))
                            {
                                if (row[col].ToString().Equals("0"))
                                {
                                    drx[string.Concat("Parameter", col.Ordinal - 2)] = dateUnavailable;
                                }
                                else
                                {
                                    if (reportType.Equals("MidnightEnergies"))
                                    {
                                        try
                                        {
                                            drx[string.Concat("Parameter", col.Ordinal - 2)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat());
                                        }
                                        catch (Exception ex)    //Exception log for catch block
                                        {
                                           drx[string.Concat("Parameter", col.Ordinal - 2)]= "-------";
                                           logger.Log(LOGLEVELS.Error, "ShowReport(string reportType)", ex);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            drx[string.Concat("Parameter", col.Ordinal - 2)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                                        }
                                        catch (Exception ex)    //Exception log for catch block
                                        {
                                            drx[string.Concat("Parameter", col.Ordinal - 2)] = "-------";
                                            logger.Log(LOGLEVELS.Error, "ShowReport(string reportType)", ex);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string val = Convert.ToString(row[col]);
                                if (val.Trim().ToUpper().Equals("LGZ"))
                                    drx[string.Concat("Parameter", col.Ordinal - 2)] = "Cabcon";
                                else if (reportType.Equals("Anomaly"))
                                {
                                    if (Convert.ToInt32(row[col]) > 1 || Convert.ToInt32(row[col]) < 0)
                                    {
                                        drx[string.Concat("Parameter", col.Ordinal - 2)] = "";
                                    }
                                    else
                                    {
                                        drx[string.Concat("Parameter", col.Ordinal - 2)] = Convert.ToInt32(row[col]) == 1 ? "OK" : "NOT OK";
                                    }
                                }                                
                                else
                                    drx[string.Concat("Parameter", col.Ordinal - 2)] = CommonBLL.GetFormattedData(val);
                            }
                        }
                    }
                    dtx.Rows.Add(drx);
                    reportXSD.Tables.Add(dtx);
                }
                if (!(reportType.Equals("Tamper") || reportType.Equals("Transaction")))
                {
                    dWDateWiseReport.Section2.SectionFormat.EnableSuppress = true;
                }
                int startIndex = 0;
                if (reportType.Equals("MidnightEnergies"))
                {
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["Parameter1"];
                    TextParam.Text = "Date \n (0.0.1.0.0.255;8;2)";
                    TextParam.ObjectFormat.EnableSuppress = false;
                    startIndex++;
                }               
                for (int i = 0; i < chkListSelectParameters.CheckedItems.Count; i++)
                {
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)dWDateWiseReport.ReportDefinition.ReportObjects["Parameter" + (startIndex++ + 1)];
                    bool isBillingPowerOffDuration = false;
                    bool isSignedActivePower = false;
                    foreach (KeyValuePair<string, string> pair in reportColumns)
                    {
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
                    TextParam.Text = columnList[i];
                    //TextParam.Text = chkListSelectParameters.CheckedItems[i].ToString();
                    TextParam.ObjectFormat.EnableSuppress = false;
                }

                dWDateWiseReport.SetDataSource(reportXSD);
                //This condition added to show the abbreviations for tamper at the report footer; 24th April 2012; Bug 75902   
                if (!isPUMA)
                {
                    dWDateWiseReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    if (!(reportType.Equals("LoadSurvey")))
                    {
                        dWDateWiseReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                    }
                    else
                    {
                        if(!ds.Tables[0].Columns.Contains("TamperStatus"))
                        {
                            dWDateWiseReport.SecReportFooter.SectionFormat.EnableSuppress = true;
                        }
                    }
                }
                databaseReportForm.drptViewer.ReportSource = dWDateWiseReport;
                databaseReportForm.drptViewer.Zoom(1);
                this.Hide();
                databaseReportForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowReport(string reportType)", ex);
                new CABException(ex);
            }
        }

        public List<string> GetColumnsCollection()
        {
            List<string> selectedColumns = new List<string>();
            for (int colCount = 0; colCount < chkListSelectParameters.CheckedItems.Count; colCount++)
            {
             
                    selectedColumns.Add(chkListSelectParameters.CheckedItems[colCount].ToString());
           
            }
            return selectedColumns;
        }

        private void lngGridSelectFiles_OnGridRowChanged(string msg)
        {
            FileName = lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["FileName"].ToString();
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
            reportColumns.Add("(0.0.94.91.11.255;1;2)", "Category");
            reportColumns.Add("(0.0.96.1.4.255;1;2)", "Year of Manuf");
            //reportColumns.Add("0.0.96.0.166.255;1;2", "Meter Model No.");
            reportColumns.Add("0.0.96.0.166.255;1;2", "Model - Type");


            return reportColumns;
        }

        private void dtPickerFromDate_ValueChanged(object sender, EventArgs e)
        {
            btnSelectDateNext.Enabled = false;
        }

        private void dtPickerToDate_ValueChanged(object sender, EventArgs e)
        {
            btnSelectDateNext.Enabled = false;
        }

        private void DateWiseBetween_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
        }
    }
}
