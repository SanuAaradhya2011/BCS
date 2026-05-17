using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using CAB.BLL;
using CAB.UI.Controls;
using CABApplication.Reports.RPTFilesNew;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CABApplication.Reports.Forms;
using LTCTBLL;

namespace CAB.UI
{
    public partial class CABFileWise : CABForm
    {
        private string FileName;
        private int selectedTamperCode;
        private string selectedTamperParameter;
        const string dateUnavailable = "--------";

        public CABFileWise()
        {
            InitializeComponent();
        }

        private void CABFileWise_Load(object sender, EventArgs e)
        { 
            this.Size = new Size(375, 410);
            this.CenterToScreen();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            FileSelectForm fileSelectForm = new FileSelectForm();
            fileSelectForm.OnGridValue_Selection += new FileSelectForm.GetValueColumn(FileSelectForm_OnGridValueSelection);
            fileSelectForm.ShowDialog();
        }

        private void FileSelectForm_OnGridValueSelection(string lngFileName)
        {
            btnFileWiseNext.Enabled = false;
            FileName = lngFileName;
            txtBoxSelectFile.Text = lngFileName;
            btnFileWiseNext.Enabled = true;
            FillListWithMeters(lngFileName);
        }

        private void FillListWithMeters(string lngFileName)
        {
            DataSet ds = new MeterDataBLL().ListDataSet("CABF", lngFileName);
            lngGridAvailableMeters.Data =ds;
            lngGridAvailableMeters.IsSorting = false;
            lngGridAvailableMeters.SetWidth("S.No", 60);
            lngGridAvailableMeters.SetWidth("Meter Number", 220);
            lngGridAvailableMeters.HiddenColumns = "MeterData_ID,Reading DateTime";
            lngGridAvailableMeters.ValueColumn = "Meter Number";
            lngGridAvailableMeters.RefreshGrid();
        }

        private void btnFileWiseNext_Click(object sender, EventArgs e)
        {
            if (!ValidateFileSelection())
                return;

            //making the relevant group to be visible
            groupBoxSelectFile.Visible = false;
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
        }

        private bool ValidateFileSelection()
        {
            if (txtBoxSelectFile.Text == "")
            {
                MessageBox.Show("Please select atleast a file.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (lngGridAvailableMeters.Data == null)
            {
                MessageBox.Show("Meters are not available for this file.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void chkListSelectParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSelectParameters.CheckedIndices.Count > 8)
            {
                chkListSelectParameters.SetItemChecked(chkListSelectParameters.SelectedIndex, false);
                MessageBox.Show("A maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            chkListSelectParameters.Items.Clear();
            foreach (string column in snapshotColumns)
                chkListSelectParameters.Items.Add(column);
            groupBoxSelectFile.Visible = false;
            groupBoxParameterCategory.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameters.Visible = true;
            groupBoxParameters.Location = new Point(9, 9);
        }

        private void btnFileWiseCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void btnTamperCategoryPrevious_Click(object sender, EventArgs e)
        {
            groupBoxSelectFile.Visible = false;
            groupBoxParameters.Visible = false;
            groupBoxTamperCategory.Visible = false;
            groupBoxParameterCategory.Visible = true;
            groupBoxParameterCategory.Location = new Point(9, 9);
        }

        private void btnTamperCategoryCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void btnParametersPrevious_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            if (radioBtnTamperParameter.Checked == false)
            {
                groupBoxSelectFile.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxParameterCategory.Visible = true;
                groupBoxParameterCategory.Location = new Point(9, 9);
            }
            else
            {
                groupBoxSelectFile.Visible = false;
                groupBoxParameterCategory.Visible = false;
                groupBoxParameters.Visible = false;
                groupBoxTamperCategory.Visible = true;
                groupBoxTamperCategory.Location = new Point(9, 9);
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
                this.StatusMessage = "Please Select atleast one Parameter";
                return false;
            }
            return true;
        }

        private void btnParametersCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void btnParameterCategoryPrevious_Click(object sender, EventArgs e)
        {
            groupBoxParameterCategory.Visible = false;
            groupBoxSelectFile.Visible = true;
            groupBoxSelectFile.Location = new Point(9, 9);
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
                    if (column != "InstantActivepowerRPhase" && column != "InstantActivepowerYPhase"
                        && column != "InstantActivepowerBPhase" && column != "InstantReactivepowerRPhase" && column != "InstantReactivepowerYPhase" && column!= "InstantReactivepowerBPhase" &&
                        column != "InstantApparentpowerRPhase" && column != "InstantApparentpowerYPhase" && column != "InstantApparentpowerBPhase" && column != "Power Off Days")
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

        public List<string> GetColumnsCollection()
        {
            List<string> selectedColumns = new List<string>();
            for (int colCount = 0; colCount < chkListSelectParameters.CheckedItems.Count; colCount++)
                selectedColumns.Add(chkListSelectParameters.CheckedItems[colCount].ToString());
            return selectedColumns;
        }

        private void ShowReport(string reportType)
        {
            try
            {
                FileReportDataSet reportXSD = new FileReportDataSet();
                DataRow reportRow;
                List<string> columnList = new List<string>();
                MeterDetails meterDetails = new MeterDetails();
                DatabaseReportForm databaseReportForm = new DatabaseReportForm();
                DataSet ds = new DataSet();
                columnList = GetColumnsCollection();

                CrystalDecisions.CrystalReports.Engine.TextObject TxtHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TextHeading"];
                if (reportType == "General")
                {
                    TxtHeading.Text = "General Details";
                    ds = new GeneralBLL().GetGeneralDataByParameter(FileName, columnList, "CAB");
                }
                else if (reportType.Equals("Instant"))
                {
                    TxtHeading.Text = "Instantaneous Details";
                    ds = new InstantPowerBLL().GetInstantDataByParameter(FileName, columnList,"CAB");
                }
                else if (reportType == "Billing")
                {
                    TxtHeading.Text = "Billing Details";
                    ds = new BillingBLL().GetBillingDataByParameter(FileName, columnList,"CAB");
                }
                else if (reportType == "LoadSurvey")
                {
                    TxtHeading.Text = "Load Survey Details";
                    ds = new LoadSurveyBLL().GetLoadSurveyDataByParameter(FileName, columnList,"CAB");
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
                    ds = new TamperSnapShotBLL().GetTamperSnapshotDataByParameter(FileName, columnList, selectedTamperCode,"CAB");

                    CrystalDecisions.CrystalReports.Engine.TextObject TxtTamperHeading = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtTamperParameter"];
                    TxtTamperHeading.Text = selectedTamperParameter;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (reportType.Equals("General"))
                        this.StatusMessage = "General values not available.";
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
                    reportRow = reportXSD.Tables["DataReportsTable"].NewRow();
                    
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                            reportRow["MeterNo"] = row[col];
                        else
                        {
                            if (CommonBLL.IsTimeColumn(col.ColumnName))
                            {
                                if (row[col].ToString().Equals("0"))
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = dateUnavailable;
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            }
                            else
                                if ((col.ColumnName != "ErrorCode") && (!(col.ColumnName.Contains("PowerFactor"))))
                                {
                                    if (string.Equals(col.ColumnName, "CumulativeExportEnergyKWH", StringComparison.OrdinalIgnoreCase) || string.Equals(col.ColumnName, "CumulativeExportEnergyKVAH", StringComparison.OrdinalIgnoreCase))
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal)] = string.IsNullOrEmpty(row[col].ToString()) ? "NA" : CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString())); //CommonBLL.GetFormattedData(row[col].ToString());
                                    }
                                    else if ((col.ColumnName == "PowerOnHours" || col.ColumnName == "TotalPowerOnHours") && UtilityDetails.ShowPowerOnHoursInMinutes)
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.FormatPowerOnHours(row[col].ToString(),true);
                                    }
                                    else
                                    {
                                        reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString())); //CommonBLL.GetFormattedData(row[col].ToString());
                                    }
                                }
                                else
                                    reportRow[string.Concat("Parameter", col.Ordinal)] = CommonBLL.RemoveUnitForReport(row[col].ToString());
                        }
                    }
                    reportXSD.Tables["DataReportsTable"].Rows.Add(reportRow);
                }

                CrystalDecisions.CrystalReports.Engine.TextObject TxtFileName = (CrystalDecisions.CrystalReports.Engine.TextObject)meterDetails.ReportDefinition.ReportObjects["TxtFileName"];
                TxtFileName.Text = txtBoxSelectFile.Text;
                
                for (int i = 0; i < chkListSelectParameters.CheckedItems.Count; i++)
                {
                    //columnList.Add(chkListSelectParameters.CheckedItems[i].ToString());
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

        private void btnParameterCategoryCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void lngGridTamper_OnGridRowChanged(string msg)
        {
            int tamperCode = 0;
            if (Int32.TryParse(msg,out tamperCode))
                selectedTamperCode = tamperCode;
        }

        private void CloseForm()
        {
            this.Close();
            this.StatusMessage = string.Empty;
        }

        private void radioBtnInstantParameter_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}