using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;
using CABApplication.Reports.RPTFilesNew;
using LTCTBLL;
namespace CAB.UI
{
    public partial class MeterWise : CABForm
    {
        private string MeterID = string.Empty;
        private int selectedTamperCode;
        private string selectedTamperParameter;
        private long activeMeterDataID = 0;
        const string dateUnavailable = "--------";
        const string NOTAVAILABLE = "NA";
        private string meterID = string.Empty;

        public MeterWise()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                activeMeterDataID = Convert.ToInt64(ConfigInfo.ActiveMeterDataId);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            MeterSelectForm meterSelectForm = new MeterSelectForm();
            meterSelectForm.OnGridValue_Selection += new MeterSelectForm.GetValueColumn(meterSelectForm_OnGridValueSelection);
            meterSelectForm.ShowDialog();
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
            DataSet ds = new MeterDataBLL().ListDataSet("MSN", meterID);
            lngGridAvailableFiles.Data =ds;
            lngGridAvailableFiles.IsSorting = false;
            lngGridAvailableFiles.SetWidth("S.No", 60);
            lngGridAvailableFiles.SetWidth("File Name", 220);
            lngGridAvailableFiles.HiddenColumns = "MeterData_ID,Reading DateTime";
            lngGridAvailableFiles.ValueColumn = "File Name";
            lngGridAvailableFiles.RefreshGrid();
        }

        private void btnMeterCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMeterNext_Click(object sender, EventArgs e)
        {

            if (!ValidateMeterSelection())
                return;

            groupBoxSelectMeter.Visible = false;
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
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
            if (radioBtnTamperParameter.Checked == false)
            {
                groupBoxParameters.Visible = true;
                groupBoxParameters.Location = new Point(9, 9);
            }
            else
            {
                groupBoxTamperCategory.Visible = true;
                groupBoxTamperCategory.Location = new Point(9, 9);
            }

            if (radioBtnInstantParameter.Checked == true)
                columnList = CommonBLL.GetColumns("Instant");
            else if (radioBtnGeneralParameter.Checked == true)
                columnList = CommonBLL.GetColumns("General");
            else if (radioBtnBillingParameter.Checked == true)
                columnList = CommonBLL.GetColumns("Billing");
            else if (radioBtnLoadSurveyParameter.Checked == true)
                columnList = CommonBLL.GetColumns("LoadSurvey");

            chkListSelectParameters.Items.Clear();

            if (radioBtnTamperParameter.Checked == false)
            {
                foreach (string column in columnList)
                    chkListSelectParameters.Items.Add(column);
            }
            else
            {
                lngGridTamper.Data = new TamperTypeBLL().ListDataSetForReports();
                lngGridTamper.SetWidth("SNo", 40);
                lngGridTamper.SetWidth("TamperType", 225);
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
            if (selectedTamperCode == 225)
                snapshotColumns = CommonBLL.GetColumns("PowerOnOffSnapShot");
            else
                snapshotColumns = CommonBLL.GetColumns("TamperSnapshot");
            //snapshotColumns = GetColumns("TamperSnapshot");
            chkListSelectParameters.Items.Clear();
            foreach (string column in snapshotColumns)
                chkListSelectParameters.Items.Add(column);
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
            this.StatusMessage = string.Empty;
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

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!ValidateParameters())
                return;

            if (radioBtnGeneralParameter.Checked == true)
                ShowReport("General");
            else if (radioBtnInstantParameter.Checked == true)
                ShowReport("Instant");
            else if (radioBtnBillingParameter.Checked == true)
                ShowReport("Billing");
            else if (radioBtnLoadSurveyParameter.Checked == true)
                ShowReport("LoadSurvey");
            else if (radioBtnTamperParameter.Checked == true)
                ShowReport("Tamper");

            Cursor.Current = Cursors.Default;
        }

        private bool ValidateParameters()
        {
            if (chkListSelectParameters.CheckedItems.Count > 8)
            {
                this.StatusMessage = "A maximum of 8 Parameters can be selected";
                return false;
            }
            else if (chkListSelectParameters.CheckedItems.Count == 0)
            {
                this.StatusMessage = "Please select a parameter";
                return false;
            }
            else
            {
                this.StatusMessage = "";
                return true;
            }
        }

        private void btnReportParamsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MeterWiseReport_Load(object sender, EventArgs e)
        {
            this.Size = new Size(375, 410);
            this.CenterToScreen();
        }

        private void chklistBoxSelectedParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSelectParameters.CheckedIndices.Count > 8)
            {
                chkListSelectParameters.SetItemChecked(chkListSelectParameters.SelectedIndex, false);
                MessageBox.Show("A maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowReport(string reportType)
        {
            try
            {
                FileReportDataSet reportXSD = new FileReportDataSet();
                DataSet consumerDetailsDS = new DataSet();
                DataRow reportRow;
                List<string> columnList = new List<string>();
                MMeterDetailsReport mMeterDetailsReport = new MMeterDetailsReport();
                DatabaseReportForm databaseReportForm = new DatabaseReportForm();
                DataSet ds = new DataSet();
                columnList = GetColumnsCollection();
                meterID = string.Empty;

                //Fill the Consumer Meter Details
                consumerDetailsDS = ListConsumerMeterDetailsByMeterID(this.MeterID);
                if (consumerDetailsDS != null && consumerDetailsDS.Tables.Count > 0 && consumerDetailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetailsTextObject(consumerDetailsDS, mMeterDetailsReport);
                else
                {
                    //meterID = new MeterDataBLL().GetMeterIDFromMeterDataID(this.activeMeterDataID).Tables[0].Rows[0][0].ToString();
                    FillMeterIDInReport(mMeterDetailsReport);
                }

                CrystalDecisions.CrystalReports.Engine.TextObject TxtHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextHeading"];
                if (reportType == "General")
                {
                    TxtHeading.Text = "General Details";
                    ds = new GeneralBLL().GetGeneralDataByParameter(MeterID , columnList, "Meter");
                }
                else if (reportType.Equals("Instant"))
                {
                    TxtHeading.Text = "Instantaneous Details";
                    ds = new InstantPowerBLL().GetInstantDataByParameter(MeterID, columnList, "Meter");
                }
                else if (reportType == "Billing")
                {
                    TxtHeading.Text = "Billing Details";
                    ds = new BillingBLL().GetBillingDataByParameter(MeterID, columnList, "Meter");
                }
                else if (reportType == "LoadSurvey")
                {
                    TxtHeading.Text = "Load Survey Details";
                    ds = new LoadSurveyBLL().GetLoadSurveyDataByParameter(MeterID, columnList, "Meter");
                    int value = 0;
                    int rIndex = 0;
                    for (rIndex = 0; rIndex <= ds.Tables[0].Rows.Count - 1; rIndex++)
                    {
                        for (int colIndex = 2; colIndex <= ds.Tables[0].Columns.Count - 1; colIndex++)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[rIndex][colIndex]) != "")
                            {
                                value++;
                            }
                            else
                            {
                                ds.Tables[0].Rows[rIndex][colIndex] = "---------";
                            }
                        }
                    }
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (value == 0)
                        {
                            MessageBox.Show("Selected paramters are not configured in the meter", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
                else if (reportType == "Tamper")
                {
                    TxtHeading.Text = "Tamper Details";
                    ds = new TamperSnapShotBLL().GetTamperSnapshotDataByParameter(MeterID, columnList, selectedTamperCode, "Meter");

                    CrystalDecisions.CrystalReports.Engine.TextObject TxtTamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TxtTamperParameter"];
                    TxtTamperHeading.Text = selectedTamperParameter;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (reportType.Equals("General"))
                        this.StatusMessage = "General data not available.";
                    else if (reportType.Equals("Instant"))
                        this.StatusMessage = "Instant data not available.";
                    else if (reportType.Equals("Billing"))
                        this.StatusMessage = "Billing data not available.";
                    else if (reportType.Equals("LoadSurvey"))
                        this.StatusMessage = "LoadSurvey data not available.";
                    else if (reportType.Equals("Tamper"))
                        this.StatusMessage = "Tamper data not available.";

                    return;
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["MDataReportsTable"].NewRow();
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                            reportRow["MeterNo"] = row[col];
                        else if (col.Ordinal == 1)
                            reportRow["FileName"] = row[col];
                        else
                        {
                            if (CommonBLL.IsTimeColumn(col.ColumnName))
                                if (row[col].ToString().Equals("0"))
                                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = dateUnavailable;
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            else
                            {
                                if ((col.ColumnName != "ErrorCode") && (!(col.ColumnName.Contains("PowerFactor"))))
                                {
                                    if (string.Equals(col.ColumnName, "CumulativeExportEnergyKWH", StringComparison.OrdinalIgnoreCase) || string.Equals(col.ColumnName, "CumulativeExportEnergyKVAH", StringComparison.OrdinalIgnoreCase))
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal - 1)] = string.IsNullOrEmpty(row[col].ToString())? NOTAVAILABLE : CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString()));
                                    }
                                    else if ((col.ColumnName == "PowerOnHours" || col.ColumnName == "TotalPowerOnHours") && UtilityDetails.ShowPowerOnHoursInMinutes)
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal - 1)] = CommonBLL.FormatPowerOnHours(row[col].ToString(),true);
                                    }
                                    else
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal - 1)] = CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString()));//CommonBLL.GetFormattedData(row[col].ToString());
                                    }
                                }
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = CommonBLL.RemoveUnitForReport(row[col].ToString());
                            }
                        }
                    }
                    reportXSD.Tables["MDataReportsTable"].Rows.Add(reportRow);
                }

                for (int i = 0; i < chkListSelectParameters.CheckedItems.Count; i++)
                {
                    //columnList.Add(chkListSelectParameters.CheckedItems[i].ToString());
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["Parameter" + (i + 1)];
                    TextParam.Text = chkListSelectParameters.CheckedItems[i].ToString();
                    TextParam.ObjectFormat.EnableSuppress = false;
                }

                mMeterDetailsReport.SetDataSource(reportXSD);
                databaseReportForm.drptViewer.ReportSource = mMeterDetailsReport;
                databaseReportForm.drptViewer.Zoom(1);
                this.Hide();
                databaseReportForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                throw new CABException(ex);
            }
        }

        private void FillConsumerMeterDetailsTextObject(DataSet detailsDset, MMeterDetailsReport mMeterDetailsReport)
        {
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterType = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextMeterType"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterModel = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextMeterModel"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextEMF = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextEMF"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextContractDemand = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextContractDemand"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextInstallationDate = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextInstallationDate"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextLocation = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextLocation"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextConsumerID = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextConsumerID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCMRINumber = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextCMRINumber"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextRegion = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextRegion"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCircle = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextCircle"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextDivision = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextDivision"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextActiveMeter"];

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
                TextEMF.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_EMF"].ToString());
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

        private void FillMeterIDInReport(MMeterDetailsReport mMeterDetailsReport)
        {
            //CrystalDecisions.CrystalReports.Engine.TextObject TextMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextMeterNo1"];
            //CrystalDecisions.CrystalReports.Engine.TextObject TextFooterMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextFooterMeterID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)mMeterDetailsReport.ReportDefinition.ReportObjects["TextActiveMeter"];
            //TextMeterID.Text = meterID;
            //TextFooterMeterID.Text = meterID;
            TextActiveMeter.Text = "Inactive";
        }

        private DataSet ListConsumerMeterDetailsByMeterID(string MeterID)
        {
            return new MeterDataBLL().GetConsumerMeterDetailsByMeterID(MeterID);
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
    }
}