using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CAB.BLL;
using CABApplication.Reports.RPTFilesNew;
using CAB.IECFramework;
using CAB.UI.Controls;
using CAB.IECFramework.Utility;
using CABApplication.Reports.Forms;
using LTCTBLL;

namespace CAB.UI
{
    public partial class DateWiseBetween : CABForm
    {
        private string FileName;
        private int selectedTamperCode;
        private string selectedTamperParameter;
        const string dateUnavailable = "--------";
        const string NOTAVAILABLE = "NA";

        public DateWiseBetween()
        {
            InitializeComponent();
        }

        private void btnSelectDateCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAvailableMeterCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTamperCategoryCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnParametersCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            this.Close();
        }

        private void btnParameterCategoryCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectDateNext_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (ValidateDates())
            {
                if (lngGridSelectFiles.Data == null || lngGridSelectFiles.Data.Tables[0].Rows.Count == 0)
                {
                    this.StatusMessage = "No files present.";
                    return;
                }
                FileName = lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["FileName"].ToString();
                FillAvailableMeters(FileName);
                groupBoxAvailableMeters.Visible = true;
                groupBoxAvailableMeters.Location = new Point(9, 9);
                groupBoxSelectFile.Visible = false;
                groupBoxTamperCategory.Visible = false;
                groupBoxParameterCategory.Visible = false;
                groupBoxParameters.Visible = false;
            }
        }

        private void FillAvailableMeters(string fileName)
        {
            lngGridAvailableMeters.Data = new MeterDataBLL().ListDataSet("CABF", fileName);
            lngGridAvailableMeters.HiddenColumns = "MeterData_ID,Reading DateTime,File Uploading Date Time,FileUpload_ID";
            
            lngGridAvailableMeters.ValueColumn="Meter Number";
            lngGridAvailableMeters.SetWidth("S.No", 55);
            lngGridAvailableMeters.SetWidth("Meter Number", 230);
            lngGridAvailableMeters.RefreshGrid();
        }


        private void btnView_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            lngGridSelectFiles.Data = new DataSet();
            lngGridSelectFiles.RefreshGrid();

            if (ValidateDates() == true)
                GetCABFilesBetweenDates(dtPickerFromDate.Value, dtPickerToDate.Value);
            if (lngGridSelectFiles.Data != null)
            {
                if (lngGridSelectFiles.Data.Tables.Count >0 && lngGridSelectFiles.Data.Tables[0].Rows.Count > 0)
                    btnSelectDateNext.Enabled = true;
                else
                    btnSelectDateNext.Enabled = false;
            }
        }

        private bool ValidateDates()
        {
            if (dtPickerFromDate.Value.Date > dtPickerToDate.Value.Date)
            {
                this.StatusMessage = "To Date should be greater than From Date";
                return false;
            }
            else
                return true;
        }

        private void GetCABFilesBetweenDates(DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Subtract(fromDate.TimeOfDay);
            toDate = toDate.Subtract(toDate.TimeOfDay);
            toDate = toDate.Add(new TimeSpan(23, 59, 59));
            lngGridSelectFiles.Data = new FileUploadMasterBLL().GetCABFileNamesBetweenDates(fromDate, toDate);
            lngGridSelectFiles.SetWidth("S.No", 55);
            lngGridSelectFiles.SetWidth("FileName", 235);
            lngGridSelectFiles.HiddenColumn = "FileUpload_ID";
            lngGridSelectFiles.ValueColumn = "FileName";
            lngGridSelectFiles.IsSorting = false;
            lngGridSelectFiles.RefreshGrid();
        }

        private void btnAvailableMeterNext_Click(object sender, EventArgs e)
        {
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
            List<string> snapshotColumns = new List<string>();
            selectedTamperParameter = lngGridTamper.Data.Tables[0].Rows[lngGridTamper.SelectedIndex]["TamperType"].ToString();
            selectedTamperCode = Convert.ToInt32(lngGridTamper.Data.Tables[0].Rows[lngGridTamper.SelectedIndex]["TamperTypeID"].ToString());
            if (selectedTamperCode == 225)
                snapshotColumns = CommonBLL.GetColumns("PowerOnOffSnapShot");
            else
                snapshotColumns = CommonBLL.GetColumns("TamperSnapshot");
            //snapshotColumns = CommonBLL.GetColumns("TamperSnapshot");
            chkListSelectParameters.Items.Clear();
            foreach (string column in snapshotColumns)
                chkListSelectParameters.Items.Add(column);
            groupBoxSelectFile.Visible = false;
            groupBoxAvailableMeters.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = true;
            groupBoxParameters.Location = new Point(9, 9);
        }

        private void btnParameterCategoryNext_Click(object sender, EventArgs e)
        {
            List<string> columnList = new List<string>();
            DataSet ds = new DataSet();

            groupBoxSelectFile.Visible = false;
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
                columnList =  CommonBLL.GetColumns("Instant");
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
            this.StatusMessage = string.Empty;
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
        }

        private void chkListSelectParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSelectParameters.CheckedIndices.Count > 8)
            {
                chkListSelectParameters.SetItemChecked(chkListSelectParameters.SelectedIndex, false);
                MessageBox.Show("A maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowParameters_Click(object sender, EventArgs e)
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
            return true;
        }

        private void ShowReport(string reportType)
        {
            try
            {
                FileReportDataSet reportXSD = new FileReportDataSet();
                DataRow reportRow;
                List<string> columnList = new List<string>();
                DWBetMeterDetails meterDetails = new DWBetMeterDetails();
                DatabaseReportForm databaseReportForm = new DatabaseReportForm();
                DataSet ds = new DataSet();

                columnList = GetColumnsCollection();

                CrystalDecisions.CrystalReports.Engine.TextObject TxtFromDate = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtFromDate"];
                TxtFromDate.Text =dtPickerFromDate.Value.ToString(ConfigInfo.DateFormat());

                CrystalDecisions.CrystalReports.Engine.TextObject TxtToDate = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtToDate"];
                TxtToDate.Text = dtPickerToDate.Value.ToString(ConfigInfo.DateFormat());

                CrystalDecisions.CrystalReports.Engine.TextObject TxtHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TextHeading"];
                if (reportType == "General")
                {
                    TxtHeading.Text = "General Details";
                    ds = new GeneralBLL().GetGeneralDataByParameter(FileName, columnList, "CAB");
                }
                else if (reportType.Equals("Instant"))
                {
                    TxtHeading.Text = "Instantaneous Details";
                    ds = new InstantPowerBLL().GetInstantDataByParameter(FileName, columnList, "CAB");
                }
                else if (reportType == "Billing")
                {
                    TxtHeading.Text = "Billing Details";
                    ds = new BillingBLL().GetBillingDataByParameter(FileName, columnList, "CAB");
                }
                else if (reportType == "LoadSurvey")
                {
                    TxtHeading.Text = "Load Survey Details";
                    ds = new LoadSurveyBLL().GetLoadSurveyDataByParameter(FileName, columnList, "CAB");
                    int value = 0;
                    int rIndex = 0;
                    for (rIndex = 0; rIndex <= ds.Tables[0].Rows.Count - 1; rIndex++)
                    {
                        for (int colIndex = 1; colIndex <= ds.Tables[0].Columns.Count - 1; colIndex++)
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
                    ds = new TamperSnapShotBLL().GetTamperSnapshotDataByParameter(FileName, columnList, selectedTamperCode, "CAB");

                    CrystalDecisions.CrystalReports.Engine.TextObject TxtTamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtTamperParameter"];
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
                        this.StatusMessage = "Load survey data not available.";
                    else if (reportType.Equals("Tamper"))
                        this.StatusMessage = "Tamper data not available.";
                    
                    return;
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DWBetDataReportsTable"].NewRow();

                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                        {
                            reportRow["MeterNo"] = row[col];
                            reportRow["FileName"] = FileName;
                        }
                        else
                        {
                            if (CommonBLL.IsTimeColumn(col.ColumnName))
                                if (row[col].ToString().Equals("0"))
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = dateUnavailable;
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            else
                            {
                                if ((col.ColumnName != "ErrorCode") && (!(col.ColumnName.Contains("PowerFactor"))))
                                {
                                    if (string.Equals(col.ColumnName, "CumulativeExportEnergyKWH", StringComparison.OrdinalIgnoreCase) || string.Equals(col.ColumnName, "CumulativeExportEnergyKVAH", StringComparison.OrdinalIgnoreCase))
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal)] = string.IsNullOrEmpty(row[col].ToString()) ? NOTAVAILABLE : CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString()));//CommonBLL.GetFormattedData(row[col].ToString());
                                    }
                                    else if ((col.ColumnName == "PowerOnHours" || col.ColumnName == "TotalPowerOnHours") && UtilityDetails.ShowPowerOnHoursInMinutes)
                                    {
                                        
                                      reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.FormatPowerOnHours(row[col].ToString(), true);                                       
                                    }
                                    else
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString()));
                                    }
                                }
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.RemoveUnitForReport(row[col].ToString());
                            }
                        }
                    }
                    reportXSD.Tables["DWBetDataReportsTable"].Rows.Add(reportRow);
                }

                //CrystalDecisions.CrystalReports.Engine.TextObject TxtFileName = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtFileName"];
                //if (reportType != "Tamper")
                //    TxtFileName.Text = txtBoxSelectFile.Text;
                //else
                //    TxtFileName.Text = selectedTamperParameter;

                for (int i = 0; i < chkListSelectParameters.CheckedItems.Count; i++)
                {
                    columnList.Add(chkListSelectParameters.CheckedItems[i].ToString());
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["Parameter" + (i + 1)];
                    TextParam.Text = chkListSelectParameters.CheckedItems[i].ToString();
                    TextParam.ObjectFormat.EnableSuppress = false;
                }

                meterDetails.SetDataSource(reportXSD);
                databaseReportForm.drptViewer.ReportSource = meterDetails;
                databaseReportForm.drptViewer.Zoom(1);
                this.Hide();
                databaseReportForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                new CABException(ex);
            }
        }

        
        public List<string> GetColumnsCollection()
        {
            List<string> selectedColumns = new List<string>();
            for (int colCount = 0; colCount < chkListSelectParameters.CheckedItems.Count; colCount++)
                selectedColumns.Add(chkListSelectParameters.CheckedItems[colCount].ToString());
            return selectedColumns;
        }

        private void lngGridSelectFiles_OnGridRowChanged(string msg)
        {
            FileName = lngGridSelectFiles.Data.Tables[0].Rows[lngGridSelectFiles.SelectedIndex]["FileName"].ToString();
        }

        private void DateWiseBetween_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
        }

        private void dtPickerFromDate_ValueChanged(object sender, EventArgs e)
        {
            btnSelectDateNext.Enabled = false;
        }

        private void dtPickerToDate_ValueChanged(object sender, EventArgs e)
        {
            btnSelectDateNext.Enabled = false;
        }
    }
}
