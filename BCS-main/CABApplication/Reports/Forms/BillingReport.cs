using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CAB.BLL;
using CAB.DALC.Data.DataServices;
using CAB.UI;
using CAB.UI.Controls;
using CABApplication.Reports.DLMS_Detailed_Reports;
using Hunt.EPIC.Logging;
namespace CABApplication.Reports.Forms
{
    public partial class BillingReport : CABForm
    {
        private FileReportDataSet reportXSD = new FileReportDataSet();
        private string dateUnavailable = "-----------";
        static List<string> blHeadings = new List<string>();
        ReportDocument reportDocument;
        ParameterFields paramFields;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingReport).ToString());
        ParameterField paramField;
        ParameterDiscreteValue paramDiscreteValue;
        
        public BillingReport()
        {
            InitializeComponent();
            cmbHistory.SelectedIndex = 0;
        }

        private void BillingReport_Load(object sender, EventArgs e)
        {
            rdbAllMeters.Checked = true;
            cmbGroup.Visible = false;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            DataSet dsBillingData = new DataSet();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dsBillingData = GetData();
                DataSet ds = null;
                if (dsBillingData != null)
                {
                     ds = GetBillingDatatoColumn(dsBillingData);

                }
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    FillBillingReport(ds);
                    ShowReport(ds);
                }
                else
                {
                    if (rdbAllMeters.Checked == true)
                    {
                        MessageBox.Show("There is either no Billing data or Meter is not assigned to Consumer.");
                    }
                    else
                    {
                        MessageBox.Show("There is either no Billing data or Meter is not assigned to Group.");
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // Added to showreport
        private void ShowReport(DataSet ds)
        {
            ReportForm objReportForm = new ReportForm();

            CABApplication.Reports.DLMS_Detailed_Reports.BillingReportNew billingrpt = new BillingReportNew();

            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(billingrpt);
            billingrpt.SetDataSource(reportXSD);
            objReportForm.rptViewer.ReportSource = billingrpt;
            objReportForm.rptViewer.ParameterFieldInfo = paramFields;
            Cursor.Current = Cursors.Default;
            objReportForm.rptViewer.Zoom(1);
            this.Hide();
            objReportForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }
        
        // Added to fill the report fields.
        private void FillBillingReport(DataSet ds)
        {
            DataRow reportrow;

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        reportrow = reportXSD.Tables["BillingReportNew"].NewRow();
                        reportrow["History"] = "History : " + cmbHistory.SelectedItem.ToString();
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            string val = ds.Tables[0].Columns[i].ToString();
                                                        
                            if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                                reportrow["Consumer_Number"] = row["Consumer_Number"].ToString();
                            else
                                reportrow["Consumer_Number"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["Consumer_Name"].ToString()))
                                reportrow["Consumer_Name"] = row["Consumer_Name"].ToString();
                            else
                                reportrow["Consumer_Name"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["Meter_ID"].ToString()))
                                reportrow["Meter_ID"] = row["Meter_ID"].ToString();
                            else
                                reportrow["Meter_ID"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["BillingDate"].ToString()))
                                reportrow["BillingDate&Time"] = row["BillingDate"].ToString();
                            else
                                reportrow["BillingDate&Time"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["CumulativeEnergykWhTZ0"].ToString()))
                                reportrow["CumkWh"] = row["CumulativeEnergykWhTZ0"].ToString();
                            else
                                reportrow["CumkWh"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["CumulativeEnergykVAhTZ0"].ToString()))
                                reportrow["CumkVAh"] = row["CumulativeEnergykVAhTZ0"].ToString();
                            else
                                reportrow["CumkVAh"] = dateUnavailable;
                            // Added to check the optional columns.
                            if (val == "Column1")
                            {
                                if (!string.IsNullOrEmpty(row["Column1"].ToString()))
                                    reportrow["Column1"] = row["Column1"].ToString();
                                else
                                    reportrow["Column1"] = dateUnavailable;
                            }
                            if (val == "Column2")
                            {
                                if (!string.IsNullOrEmpty(row["Column2"].ToString()))
                                    reportrow["Column2"] = row["Column2"].ToString();
                                else
                                    reportrow["Column2"] = dateUnavailable;
                            }
                            if (val == "Column3")
                            {
                                if (!string.IsNullOrEmpty(row["Column3"].ToString()))
                                    reportrow["Column3"] = row["Column3"].ToString();
                                else
                                    reportrow["Column3"] = dateUnavailable;
                            }
                            if (val == "Column4")
                            {
                                if (!string.IsNullOrEmpty(row["Column4"].ToString()))
                                    reportrow["Column4"] = row["Column4"].ToString();
                                else
                                    reportrow["Column4"] = dateUnavailable;
                            }
                            if (!string.IsNullOrEmpty(row["MDkVATZ0"].ToString()))
                                reportrow["MDkVA"] = row["MDkVATZ0"].ToString();
                            else
                                reportrow["MDkVA"] = dateUnavailable;
                            if (!string.IsNullOrEmpty(row["MDkVADateTimeTZ0"].ToString()))
                                reportrow["MDkVATime"] = row["MDkVADateTimeTZ0"].ToString();
                            else
                                reportrow["MDkVATime"] = dateUnavailable;
                            if (val == "Column5")
                            {
                                if (!string.IsNullOrEmpty(row["Column5"].ToString()))
                                    reportrow["Column5"] = row["Column5"].ToString();
                                else
                                    reportrow["Column5"] = dateUnavailable;
                            }

                        }
                        reportXSD.Tables["BillingReportNew"].Rows.Add(reportrow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillBillingReport(DataSet ds)", ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdbAllMeters_CheckedChanged(object sender, EventArgs e)
        {
            cmbGroup.Visible = false;
        }

        private void rdbGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbGroup.Checked)
            {
                DataSet dsGroup = new DataSet();
                dsGroup = ListGroupData();
                if (dsGroup != null && dsGroup.Tables[0].Rows.Count > 0)
                {

                    cmbGroup.DataSource = dsGroup.Tables[0];
                    cmbGroup.ValueMember = dsGroup.Tables[0].Columns[0].ToString();
                    cmbGroup.DisplayMember = dsGroup.Tables[0].Columns[1].ToString();
                    cmbGroup.Visible = true;
                }
                else
                {
                    MessageBox.Show("Currently there are no groups available");
                    cmbGroup.Visible = false;
                }
            }
        }

        private DataSet ListGroupData()
        {
            return new GSMGroupBLL().ListGroupData();
        }

        private string QueryGroup(out ParameterFields paramFields)
        {
            reportDocument = new ReportDocument();
            paramFields = new ParameterFields();
            string query = "select a. Consumer_Number  ,a.Consumer_Name,a.Meter_ID ,b.BillingDate,b.CumulativeEnergykWhTZ0,b.CumulativeEnergykVAhTZ0";
            query += ",b.MDkVATZ0,b.MDkVADateTimeTZ0 ";
            string str = "(select a.Consumer_Number,a.Consumer_Name,b.Meter_ID from consumer_master a inner join consumermeter b ";
            str += "on a.Consumer_Number = b.Consumer_Number) as a,";
            str += " (select d.*,c.* from (SELECT b.MeterId, a.Billing_ID,a.BillingDate,a.SystemPowerFactorforBillingPeriod,a.CumulativeEnergykWhTZ0,";
            str += "a.CumulativeEnergykvarhLag,a.CumulativeEnergykvarhLead,";
            str += "a.CumulativeEnergykVAhTZ0,a.MDkWTZ0,a.MDkWDateTimeTZ0,a.MDkVATZ0,a.MDkVADateTimeTZ0,a.MeterData_ID,a.DataIndex";
            str += " FROM meterdata_billing a, meterdata b where a.meterdata_id = b.meterdata_id and a.DataIndex = " + cmbHistory.SelectedItem.ToString() + ") as c";
            str += ",gsm_group_meters d where group_id = " + cmbGroup.SelectedValue.ToString() + " and c.MeterId = d.Meter_ID and d.Status = 'S') as b";
            str += " where a.Meter_ID = b.MeterId";

            int columnNo = 8;
            paramField = new ParameterField();
            paramField.Name = "col1";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Consumer_Number";
            paramField.CurrentValues.Add(paramDiscreteValue);
            //Add the paramField to paramFields
            paramFields.Add(paramField);
            paramField = new ParameterField();
            paramField.Name = "col2";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Consumer_Name";
            paramField.CurrentValues.Add(paramDiscreteValue);
            //Add the paramField to paramFields
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col3";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Meter_ID";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col4";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "BillingDate";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col5";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "CumulativeEnergykWhTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col6";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "CumulativeEnergykVAhTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col7";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "MDkVATZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col8";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "MDkVADateTimeTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);
            int ColumnData = 0;
            if (chkBillAveragePF.Checked)
            {
                columnNo++; ColumnData++;
                query = query.Insert(query.Length, ",b.SystemPowerFactorforBillingPeriod as  Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Bill Avg PF";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkCumkvarhLag.Checked)
            {
                columnNo++;
                
                ColumnData++;
                query = query.Insert(query.Length, ",b.CumulativeEnergykvarhLag as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Cum kvarh(Lag)";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkCumkvarhLead.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.CumulativeEnergykvarhLead as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Cum kvarh(Lead)";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkMDkW.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.MDkWTZ0 as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "MD kW";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkMDkWDateTime.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.MDkWDateTimeTZ0 as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "MD kW DateTime";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }

            for (int i = columnNo; i < 13; i++)
            {
                columnNo++;
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            query += " from ";
            query = query + str;
            return query;
        }
        private string Query(out ParameterFields paramFields)
        {
            reportDocument = new ReportDocument();
            paramFields = new ParameterFields();
            // Query to retrieve data for all meters irrespective of group.
            string query = "select a. Consumer_Number  ,a.Consumer_Name,a.Meter_ID ,b.BillingDate,b.CumulativeEnergykWhTZ0,b.CumulativeEnergykVAhTZ0";
            query += ",b.MDkVATZ0,b.MDkVADateTimeTZ0 ";
            string str = "(select a.Consumer_Number,a.Consumer_Name,b.Meter_ID from consumer_master a inner join consumermeter b ";
            str += "on a.Consumer_Number = b.Consumer_Number) as a,";
            str += "(SELECT b.MeterId, a.Billing_ID,a.BillingDate,a.SystemPowerFactorforBillingPeriod,a.CumulativeEnergykWhTZ0,";
            str += "a.CumulativeEnergykvarhLag,a.CumulativeEnergykvarhLead,";
            str += "a.CumulativeEnergykVAhTZ0,a.MDkWTZ0,a.MDkWDateTimeTZ0,a.MDkVATZ0,a.MDkVADateTimeTZ0,a.MeterData_ID,a.DataIndex";
            str += " FROM meterdata_billing a, meterdata b where a.meterdata_id = b.meterdata_id and a.DataIndex = " + cmbHistory.SelectedItem.ToString() + " ) as b where a.Meter_ID = b.MeterId";
            int columnNo = 8;
            //Parameter fields created at runtime.
            paramField = new ParameterField();
            paramField.Name = "col1";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Consumer_Number";
            paramField.CurrentValues.Add(paramDiscreteValue);
            //Add the paramField to paramFields
            paramFields.Add(paramField);
            paramField = new ParameterField();
            paramField.Name = "col2";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Consumer_Name";
            paramField.CurrentValues.Add(paramDiscreteValue);
            //Add the paramField to paramFields
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col3";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "Meter_ID";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col4";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "BillingDate";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col5";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "CumulativeEnergykWhTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col6";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "CumulativeEnergykVAhTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col7";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "MDkVATZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            paramField = new ParameterField();
            paramField.Name = "col8";
            paramDiscreteValue = new ParameterDiscreteValue();
            paramDiscreteValue.Value = "MDkVADateTimeTZ0";
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);
            int ColumnData = 0;
            if (chkBillAveragePF.Checked)
            {
                columnNo++; ColumnData++;
                query = query.Insert(query.Length, ",b.SystemPowerFactorforBillingPeriod as  Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Bill Avg PF";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkCumkvarhLag.Checked)
            {
                columnNo++;
                
                ColumnData++;
                query = query.Insert(query.Length, ",b.CumulativeEnergykvarhLag as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Cum kvarh(Lag)";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkCumkvarhLead.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.CumulativeEnergykvarhLead as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "Cum kvarh(Lead)";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkMDkW.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.MDkWTZ0 as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "MD kW";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            if (chkMDkWDateTime.Checked)
            {
                columnNo++; ColumnData++;
                
                query = query.Insert(query.Length, ",b.MDkWDateTimeTZ0 as Column" + ColumnData.ToString());
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "MD kW DateTime";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);

            }
            //To make parameter value empty when all parameters are not selected.
            for (int i = columnNo; i < 13; i++)
            {
                columnNo++;
                paramField = new ParameterField();
                paramField.Name = "col" + columnNo.ToString();
                paramDiscreteValue = new ParameterDiscreteValue();
                paramDiscreteValue.Value = "";
                paramField.CurrentValues.Add(paramDiscreteValue);
                //Add the paramField to paramFields
                paramFields.Add(paramField);
            }

            query += " from ";
            query = query + str;
            return query;
        }

        // Added to get the data
        private DataSet GetData()
        {
            string query = "";
            if (rdbAllMeters.Checked)
            {
                query = Query(out paramFields);
            }
            if (rdbGroup.Checked && cmbGroup.SelectedValue == null)
            {
                return null;
            }
            if (rdbGroup.Checked && Convert.ToInt32(cmbGroup.SelectedValue) != -1)
                query = QueryGroup(out paramFields);

            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest(query);
                helper.FillDataSet(request, ds);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData()", ex);
            }
            return ds;
        }
        private DataSet GetBillingDatatoColumn(DataSet ds)
        {
            DLMS650CommonBLL common = new DLMS650CommonBLL();
            DataSet dataset = common.BillingDataToColumn(ds);
            return dataset;
        }
    }
}