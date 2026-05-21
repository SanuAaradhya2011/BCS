using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.Linq;

namespace CAB.UI
{
    public partial class LoadSwitchReport : CABForm
    {
        private FileReportDataSet reportXSD = null;
        static List<string> lsHeadings;
        ApplicationType types;
        private const string TAMPERSTATUS = "Tamper Status (0.0.96.1.152.255;1;2)";
        private const string POWERFACTOR = "Power Factor";
        private const string POWERFACTORLITERAL = "Power Factor";
        private const string EMPTYPARAMETER = "Empty Parameter";
        bool isPUMA = false;
        private bool hasData = false;
        string utility = string.Empty;
        DataSet loadSwitchDS = null;
       // DataRow reportRow;

        public LoadSwitchReport()
        {
            reportXSD = new FileReportDataSet();
            InitializeComponent();
            long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
            loadSwitchDS = new LoadSwitchBLL().GetMeterData(Convert.ToInt32(id));
            if (loadSwitchDS == null)
            {
                MessageBox.Show("No Data Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                hasData = false;
            }
            else
                hasData = true;
        }

        public bool HasData
        {
            get
            {
                return hasData;
            }
            set
            {
                hasData = value;
            }
        }
        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private void ShowReport()
        {
           ReportForm ObjRptForm = new ReportForm();
           LoadSwitchReportSM LoadSwitchReportView = new LoadSwitchReportSM(); 
            
            try
            {
                if (chkListLoadSwitchParameters.CheckedItems.Count <= 0)
                {
                    return;
                }
               
                if (!isPUMA)
                {
                    LoadSwitchReportView.Section4.SectionFormat.EnableSuppress = true;
                }
                
                if (!chkListLoadSwitchParameters.CheckedItems.Contains(TAMPERSTATUS))
                {
                    LoadSwitchReportView.Section4.SectionFormat.EnableSuppress = true;
                  
                }
                else
                {
                    if (ConfigInfo.ActiveMeterType == "1P-2W")
                    {
                        LoadSwitchReportView.Section4.SectionFormat.EnableSuppress = true;
                    }
                    
                }
               
                if (chkListLoadSwitchParameters.CheckedItems.Count > 0)
                {
                    for (int selectedItemCount = 0; selectedItemCount < chkListLoadSwitchParameters.CheckedItems.Count; selectedItemCount++)
                    {
                        TextObject groupHeaderTextObject = (TextObject)LoadSwitchReportView.GroupHeaderSection1.ReportObjects["GroupHeader" + (selectedItemCount ).ToString()] as TextObject;
                           
                            if (groupHeaderTextObject != null)
                            {
                                groupHeaderTextObject.Text = chkListLoadSwitchParameters.CheckedItems[selectedItemCount].ToString();
                               
                            }
                     }
                    // Apply modern blue theme and custom logo before rendering
                    ReportThemeHelper.Apply(LoadSwitchReportView);
                    LoadSwitchReportView.SetDataSource(reportXSD);
                    ObjRptForm.rptViewer.ReportSource = LoadSwitchReportView;
                    Cursor.Current = Cursors.Default;
                    ObjRptForm.rptViewer.Zoom(1);
                    this.Hide();
                    // SB code change Start - 20180629 - Multiple Analysis View
                    ObjRptForm.Show();
                    //ObjRptForm.ShowDialog();
                    // SB code change End - 20180629 - Multiple Analysis View
                    reportXSD.Clear();
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                int maxColumns = 14;
                if (chkListLoadSwitchParameters.CheckedItems.Count <= 0)
                {
                    MessageBox.Show("Please select atleast one parameter for report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (chkListLoadSwitchParameters.CheckedItems.Count > maxColumns)
                {
                    MessageBox.Show("Selected parameters can not exceed maxLimit (" + maxColumns + ")", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
               
                long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                loadSwitchDS = new DataSet();
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    loadSwitchDS = new LoadSwitchBLL().GetMeterData(Convert.ToInt32(id));
                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    loadSwitchDS = new LoadSwitchBLL().GetMeterData(Convert.ToInt32(id));
                }

                if (loadSwitchDS != null && loadSwitchDS.Tables[0].Rows.Count > 0)
                {
                    if ((loadSwitchDS.Tables[0].Rows.Count == 1 && ConfigInfo.ActiveFileType == "DLMS"))
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    FillLoadSwitchXSD(loadSwitchDS);
                    ShowReport();
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("No data available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception)
            {
                
                
            }          
        }
       
        private void FillLoadSwitchXSD(DataSet loadSwitchData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            DateTime PreviousDate = DateTime.Now;

            try
            {
                reportXSD.Tables["LoadSwitchTable"].Rows.Clear();
                if (loadSwitchData == null || loadSwitchData.Tables[0].Rows.Count == 0)
                    return;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["LoadSwitchTable"].NewRow();
                   
                    foreach (string columnName in chkListLoadSwitchParameters.CheckedItems)
                    {
                        if (columnName == POWERFACTORLITERAL)
                        {
                            lsHeadings.Add(POWERFACTOR);
                        }
                        else
                        {
                            lsHeadings.Add(columnName);
                        }
                    }

                    foreach (DataRow row in loadSwitchData.Tables[0].Rows)
                    {
                        DataTable LoadswitchTable = new DataTable();
                        
                        LoadswitchTable = reportXSD.Tables["LoadSwitchTable"];
                        reportRow = LoadswitchTable.NewRow();
                       
                        string dateTimes = Convert.ToString(row[0]);
                      
                        
                        for (int colCount = 0; colCount < lsHeadings.Count; colCount++)
                        {
                            
                                reportRow["Parameter" + colCount.ToString()] = CommonBLL.GetFormattedData(row[lsHeadings[colCount]].ToString());
                        
                        }
                        LoadswitchTable.Rows.Add(reportRow);
                    }
                 
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        

      private bool ConvertStringToDouble(string value)
        {
            bool isDouble = false;

            try
            {
                if (!string.IsNullOrEmpty(value))
                    {
                        Convert.ToDecimal(value);
                        isDouble = true;
                    }
            }
            catch (Exception ex)
            {
                isDouble = false;
            }
            return isDouble ;
        }
       
        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSurveyReport_Load(object sender, EventArgs e)
        {
            try
            {
               
                if (UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
                {
                    isPUMA = true;
                }
                FillLoadSwitchParameters();
                 if (chkListLoadSwitchParameters.Items.Count > 0)
                {
                    chkListLoadSwitchParameters.SetItemCheckState(0, CheckState.Checked);
                 
                }
            }
            catch (Exception)
            {
                
                
            }
        }

                
       

        private void FillLoadSwitchParameters()
        {
            try
            {
                
                types = ConfigInfo.GetApplicationType();
                long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    loadSwitchDS = new LoadSwitchBLL().GetMeterData(Convert.ToInt32(id));

                   
                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    loadSwitchDS = new LoadSwitchBLL().GetMeterData(Convert.ToInt32(id));
                  
                }
                if (loadSwitchDS != null)
                {
                    if (loadSwitchDS.Tables != null)
                    {
                        if (loadSwitchDS.Tables[0].Rows.Count == 1 && ConfigInfo.ActiveFileType == "DLMS")
                        {
                            chkListLoadSwitchParameters.Items.Clear();
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "Report can not be shown";
                            return;
                        }
                        if (loadSwitchDS.Tables[0].Columns.Count > 0)
                        {
                            lblNoDataFound.Visible = false;
                            chkListLoadSwitchParameters.Items.Clear();
                            foreach (DataColumn column in loadSwitchDS.Tables[0].Columns)
                            {
                                if (column.ColumnName == POWERFACTOR)
                                {
                                    chkListLoadSwitchParameters.Items.Add(POWERFACTORLITERAL);
                                }
                                //SarkarA code change start 20180209 // Disallow DateTime being disabled
                                else if (column.ColumnName.Contains("0.0.1.0.0.255;8;2") || column.ColumnName.Contains("0.0.96.11.6.255;1;2"))
                                {
                                    chkListLoadSwitchParameters.Items.Add(column.ColumnName, CheckState.Indeterminate);
                                    chkListLoadSwitchParameters.ItemCheck += (s, e) =>
                                    {
                                        if (e.CurrentValue == CheckState.Indeterminate) e.NewValue = CheckState.Indeterminate;
                                    };
                                }
                                //SarkarA code change end 20180209
                                else
                                {
                                    chkListLoadSwitchParameters.Items.Add(column.ColumnName);
                                }
                            }
                        }
                        else
                        {
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "No Data Found";
                            btnShow.Enabled = false;
                        }
                    }
                    else
                    {
                        lblNoDataFound.Visible = true;
                        lblNoDataFound.Text = "No Data Found";
                        btnShow.Enabled = false;
                    }
                }
                else
                {
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = "No Data Found";
                    btnShow.Enabled = false;
                }
                for (int itemCounter = 0; itemCounter < chkListLoadSwitchParameters.Items.Count; itemCounter++)
                {
                    chkListLoadSwitchParameters.SetItemCheckState(itemCounter, CheckState.Checked);
                }
            }
            catch (Exception)
            {


            }          
        }

        private void chkListLoadSwitchParameters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            e.NewValue = CheckState.Checked ;
        }

       
              
       
    }
}
