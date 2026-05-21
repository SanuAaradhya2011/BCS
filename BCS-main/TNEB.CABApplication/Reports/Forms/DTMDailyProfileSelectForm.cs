using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CAB.UI.Controls;
using CAB.BLL;
using LTCTBLL;
using CAB.IECFramework.Utility;
using CABApplication.Reports.RPTFilesNew;
using CAB.IECFramework;
using CABApplication.Reports.Forms;


namespace CAB.UI
{
    public partial class DTMDailyProfileSelectForm : CABForm
    {
        private ReportDataSet reportXSD = new ReportDataSet();
        const string dataUnavailable = "--------";
        private long activeMeterDataID = 0;
        private string meterID = string.Empty;

        public DTMDailyProfileSelectForm()
        {
            InitializeComponent();
            /* GKG 04/03/2013 137646 */
            // Old code was not working 
            //if (!UtilityDetails.ShowPowerOnHours)
            //{
                //chkBoxListDailyProfile.Items.Remove(new object[] { "Power On Hours" });
                //label1.Visible = false;
            //}
            this.chkBoxListDailyProfile.Items.Clear();
            if (!UtilityDetails.ShowPowerOnHours)
            {
                label1.Visible = false;
                this.chkBoxListDailyProfile.Items.AddRange(new object[] {
                "Cumulative kWh",
                "Cumulative kVArh Lag",
                "Cumulative kVArh Lead",
                "Cumulative kVAh",
                "Daily MD1 (kW)",
                "MD1 TimeStamp",
                "Daily MD2 (kVA)",
                "MD2 TimeStamp"});

            }
            else
            {
                this.chkBoxListDailyProfile.Items.AddRange(new object[] {
                "Cumulative kWh",
                "Cumulative kVArh Lag",
                "Cumulative kVArh Lead",
                "Cumulative kVAh",
                "Daily MD1 (kW)",
                "MD1 TimeStamp",
                "Daily MD2 (kVA)",
                "MD2 TimeStamp",
                "Power On Hours"});
            }
            /* GKG 04/03/2013 137646 */
            activeMeterDataID = Convert.ToInt64(ConfigInfo.ActiveMeterDataId);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            List<string> columnList = new List<string>();
            DataSet consumerMeterDetailsDS = new DataSet();
            DataSet dtmDailyProfileDS = new DataSet();
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (ValidateForm() == false)
                    return;

                for (int count = 0; count < chkBoxListDailyProfile.CheckedItems.Count; count++)
                    columnList.Add(chkBoxListDailyProfile.CheckedItems[count].ToString());
                 
                consumerMeterDetailsDS = ListConsumerMeterDetails();

                dtmDailyProfileDS = ListDTMDailyData(columnList);

                if (dtmDailyProfileDS.Tables[0].Rows.Count > 0)
                {
                    FillDTMDailyXSD(dtmDailyProfileDS);
                    ShowReport(columnList, consumerMeterDetailsDS);
                }
                else
                {
                    this.StatusMessage = "No data available.";
                }
            }
            catch (Exception ex)
            {
                new CABException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private DataSet ListDTMDailyData(List<string> columnList)
        {
            DTMDailyProfileBLL dtmDailyProfileBLL = new DTMDailyProfileBLL();
            return dtmDailyProfileBLL.GetDailyProfileByParameter(this.activeMeterDataID, columnList);
        }

        private void ShowReport(List<string> columnList, DataSet consumerMeterDetailsDS)
        {
            DTMDailyProfileForm dtmDailyProfileForm = new DTMDailyProfileForm();
            DTMDailyProfileNew dtmDailyProfileNew = new DTMDailyProfileNew();
            DTMDailyProfileBLL dtmDailyProfileBLL = new DTMDailyProfileBLL();
            meterID = string.Empty;

            if (consumerMeterDetailsDS != null && consumerMeterDetailsDS.Tables[0].Rows.Count > 0)
                FillConsumerMeterDetailsTextObject(consumerMeterDetailsDS, dtmDailyProfileNew);
            else
            {
                meterID = new MeterDataBLL().GetMeterIDFromMeterDataID(this.activeMeterDataID).Tables[0].Rows[0][0].ToString();
                FillMeterIDInReport(dtmDailyProfileNew);
            }

            try
            {
                if (reportXSD.Tables["DTMDailyProfileNewTable"].Rows.Count == 0)
                {
                    dtmDailyProfileNew.Section2.SectionFormat.EnableSuppress = true;
                    dtmDailyProfileNew.Section3.SectionFormat.EnableSuppress = true;
                    dtmDailyProfileNew.GroupHeaderSection1.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    dtmDailyProfileNew.Section2.SectionFormat.EnableSuppress = false;
                    dtmDailyProfileNew.Section3.SectionFormat.EnableSuppress = false;
                    dtmDailyProfileNew.GroupHeaderSection1.SectionFormat.EnableSuppress = false;
                    if (columnList.Count > 0)
                    {
                        for (int colCount = 0; colCount < columnList.Count; colCount++)
                        {
                            string TextParameterCount = "TxtParameter" + (colCount + 1);
                            CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)dtmDailyProfileNew.ReportDefinition.ReportObjects[TextParameterCount];
                            TextParam.Text = dtmDailyProfileBLL.GetReportColumnName(columnList[colCount]);
                            TextParam.ObjectFormat.EnableSuppress = false;
                        }
                    }
                }
                dtmDailyProfileNew.SetDataSource(reportXSD);
                dtmDailyProfileForm.rptViewer.ReportSource = dtmDailyProfileNew;
                Cursor.Current = Cursors.Default;
                dtmDailyProfileForm.rptViewer.Zoom(1);
                this.Hide();
                dtmDailyProfileForm.ShowDialog();
                reportXSD.Clear();
                this.Show();
            }
            catch (Exception)
            {
            }
        }

        private DataSet ListConsumerMeterDetails()
        {
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            return meterDataBLL.GetConsumerMeterDetails(this.activeMeterDataID);
        }


        private void FillConsumerMeterDetailsTextObject(DataSet detailsDset, DTMDailyProfileNew dTMDailyProfileNew)
        {
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextMeterID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextFooterMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextFooterMeterID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterType = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextMeterType"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterModel = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextMeterModel"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextEMF = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextEMF"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextContractDemand = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextContractDemand"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextInstallationDate = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextInstallationDate"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextLocation = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextLocation"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextConsumerID = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextConsumerID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCMRINumber = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextCMRINumber"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextRegion = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextRegion"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextCircle = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextCircle"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextDivision = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextDivision"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextActiveMeter"];

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterID"].ToString()))
                if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterID"].ToString()))
                {
                    TextMeterID.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterID"].ToString());
                    TextFooterMeterID.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterID"].ToString());
                }
                else
                {
                    TextMeterID.Text = dataUnavailable;
                    TextFooterMeterID.Text = dataUnavailable;
                }

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString()))
                if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString()))
                    TextMeterType.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterType_Name"].ToString());
                else
                    TextMeterType.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["MeterModel_Name"].ToString()))
                TextMeterModel.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["MeterModel_Name"].ToString());
            else
                TextMeterModel.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_EMF"].ToString()))
                TextEMF.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_EMF"].ToString());
            else
                TextEMF.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_ContractDemand"].ToString()))
                TextContractDemand.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_ContractDemand"].ToString());
            else
                TextContractDemand.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_AllocationDate"].ToString()))
                TextInstallationDate.Text = DateUtility.LongToDateTime(Convert.ToInt64(detailsDset.Tables[0].Rows[0]["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
            else
                TextInstallationDate.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Meter_Location"].ToString()))
                TextLocation.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Meter_Location"].ToString());
            else
                TextLocation.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Consumer_Number"].ToString()))
                TextConsumerID.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Consumer_Number"].ToString());
            else
                TextConsumerID.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["CMRI_Number"].ToString()))
                TextCMRINumber.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["CMRI_Number"].ToString());
            else
                TextCMRINumber.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Region_Name"].ToString()))
                TextRegion.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Region_Name"].ToString());
            else
                TextRegion.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Circle_Name"].ToString()))
                TextCircle.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Circle_Name"].ToString());
            else
                TextCircle.Text = dataUnavailable;

            if (!string.IsNullOrEmpty(detailsDset.Tables[0].Rows[0]["Division_Name"].ToString()))
                TextDivision.Text = CommonBLL.GetFormattedData(detailsDset.Tables[0].Rows[0]["Division_Name"].ToString());
            else
                TextDivision.Text = dataUnavailable;

            if (detailsDset.Tables[0].Rows[0]["Status"].ToString().Equals("0"))
                TextActiveMeter.Text = "Inactive";
            else
                TextActiveMeter.Text = "Active";
        }

        private void FillMeterIDInReport(DTMDailyProfileNew dTMDailyProfileNew)
        {
            CrystalDecisions.CrystalReports.Engine.TextObject TextMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextMeterID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextFooterMeterID = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextFooterMeterID"];
            CrystalDecisions.CrystalReports.Engine.TextObject TextActiveMeter = (CrystalDecisions.CrystalReports.Engine.TextObject)dTMDailyProfileNew.ReportDefinition.ReportObjects["TextActiveMeter"];
            TextMeterID.Text = meterID;
            TextFooterMeterID.Text = meterID;
            TextActiveMeter.Text = "Inactive";
        }

        private void FillDTMDailyXSD(DataSet dtmDailyDS)
        {
            try
            {
                DataRow reportRow;
                foreach (DataRow row in dtmDailyDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DTMDailyProfileNewTable"].NewRow();
                    foreach (DataColumn col in dtmDailyDS.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                        {
                            if (Convert.ToInt64(row["DailyProfileDate"].ToString()) != 0)
                                reportRow["DailyProfileDate"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row["DailyProfileDate"]));
                            else
                                reportRow["DailyProfileDate"] = dataUnavailable;
                        }
                        else
                        {
                            if (row[col].ToString().Equals("0") || string.IsNullOrEmpty(row[col].ToString()))
                                reportRow[string.Concat("Parameter", col.Ordinal)] = dataUnavailable;
                            else if (col.ColumnName.ToLower().Contains("time") || col.ColumnName.ToLower().Contains("date"))
                                reportRow[string.Concat("Parameter", col.Ordinal)] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row[col]));// DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
                            else if (col.ColumnName.ToLower().Contains("poweronhours") && CommonBLL.GetFormattedData(row[col].ToString()).Length < 2)
                                reportRow[string.Concat("Parameter", (col.Ordinal).ToString())] = "0" +CommonBLL.GetFormattedData(row[col].ToString());
                            else
                                reportRow[string.Concat("Parameter", (col.Ordinal).ToString())] =  CommonBLL.GetFormattedData(row[col].ToString());
                        }
                    }
                    reportXSD.Tables["DTMDailyProfileNewTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception)
            {
            }
        }

        private bool ValidateForm()
        {
            if (chkBoxListDailyProfile.CheckedItems.Count <= 0)
            {
                this.StatusMessage = "Please Select atleast one Parameter";
                return false;
            }
            else if(chkBoxListDailyProfile.CheckedItems.Count > 8)
            {             
            
                this.StatusMessage = "A maximum of 8 Parameters can be selected";
                return false;            
            }
                 
            return true;
        }

        private void chkBoxListDailyProfile_KeyUp(object sender, KeyEventArgs e)
        {
            if (chkBoxListDailyProfile.CheckedIndices.Count > 8)
            {
                chkBoxListDailyProfile.SetItemChecked(chkBoxListDailyProfile.SelectedIndex, false);
                MessageBox.Show("Maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DTMDailyProfileSelectForm_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void DTMDailyProfileSelectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void chkBoxListDailyProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkBoxListDailyProfile.CheckedIndices.Count > 8)
            {
                chkBoxListDailyProfile.SetItemChecked(chkBoxListDailyProfile.SelectedIndex, false);
                MessageBox.Show("Maximum of 8 Parameters can be selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
