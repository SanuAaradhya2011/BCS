using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.UI;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using CAB.Framework;
using CrystalDecisions.CrystalReports.Engine;
using CAB.BLL;
using CrystalDecisions.Shared;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class MidNightReportFileWise : CABForm
    {
        private FileReportDataSet reportXSD = null;
        static List<string> midnightHeadings;
        ApplicationType types;
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        string dateUnavailable = "--------";
        bool isPUMA = false;
        private const string KVAR = "kvar";
        private const string ENERGYKVARH = "kvarh";
        private const string DEMANDKVAR = "kvar";
        private const string KVA = "kva";
        private const string ENERGYKVAH = "kVAh";
        private const string DEMANDKVA = "kVA";
        private const string KW = "kw";
        private const string ENERGYKWH = "kWh";
        private const string DEMANDKW = "kW";
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        private const string ascendingOrder = "asc";
        private const string descendingOrder = "desc";

        public string MeterVariant = string.Empty;
        public int MeterModelNumber;
        DLMS650LoadSurveyBLL dlms650LoadSurveyBLL;
        List<string> lstClmMidnightEnergyNET = null;
        DLMS650CommonBLL common;

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MidNightReportFileWise).ToString());

        public MidNightReportFileWise()
        {
            try
            {
            reportXSD = new FileReportDataSet();
            InitializeComponent();
        }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "MidNightReportFileWise()", ex);
                
            }
           
        }

        private void FillMeterID(DataSet meterIdDS)
        {
            try
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
                reportRow["ActiveMeter"] = "No";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillMeterID(DataSet meterIdDS)", ex);
            }

        }

        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            try
            {
            DataRow reportRow;
            DataTable table = new DataTable();
            if (detailsDS.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
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

                    if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                    {

                        /* VBM - EMF Bug Fixed */
                        string meterEMF = CommonBLL.CalculateActualEMF(Convert.ToDecimal(row["Meter_EMF"].ToString()),
                                                                          row["internalCTRatio"].ToString(),
                                                                          row["internalPTRatio"].ToString());
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
                        /* VBM - EMF Bug Fixed */
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
                        reportRow["ActiveMeter"] = "No";
                    else
                        reportRow["ActiveMeter"] = "Yes";

                    if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                        reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                    else
                        reportRow["ContractDemand"] = dateUnavailable;

                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                }
            }
        }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillConsumerMeterDetails(DataSet detailsDS)", ex);
                
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

        private string GetUnit(string columnName)
        {
            if (columnName.ToLower().Contains("kwh"))
            {
                return "kWh";
            }
            else if (columnName.ToLower().Contains("kvah"))
            {
                return "kVAh";
            }
            else if (columnName.ToLower().Contains("kvarh (lag)"))
            {
                return "kVArh (Lag)";
            }
            else if (columnName.ToLower().Contains("kvarh (lead)"))
            {
                return "kVArh (Lead)";
            }
            else
            {
                return string.Empty;
            }
        }

        private void ShowReport()
        {
            ReportForm ObjRptForm = new ReportForm();
            MidnightReportNew midnightReportNew = new MidnightReportNew();
            try
            {
                if (chkListMidnightParameters.CheckedItems.Count <= 0)
                {
                    return;
                }
                /* Add BCS Version in Report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)midnightReportNew.ReportDefinition.ReportObjects["txtBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* Add BCS Version in Report header */
                //ReportObject mainReportObject = midnightEnergyNew.DetailSection2.ReportObjects[0];

                if (reportXSD.Tables["MidnightEnergyNET"].Rows.Count == 0)
                {
                    midnightReportNew.secMidnightValueNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjColu = midnightReportNew.secMidnightValueNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjColu[0];
                    foreach (ReportObject reportObject in rebObjColu)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)
                            {

                                TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + (selectedItemCount + 1).ToString()] as TextObject;

                                if (groupHeaderTextObject != null)
                                {
                                    groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                }

                            }
                        }
                    }
                }


                if (reportXSD.Tables["MidnightConsumptionNET"].Rows.Count == 0)
                {
                    midnightReportNew.secMidNightConsumptionNET.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjColu = midnightReportNew.secMidNightConsumptionNET.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repLoadFactor = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjColu[0];
                    foreach (ReportObject reportObject in rebObjColu)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)
                            {

                                TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text" + (selectedItemCount + 1).ToString()] as TextObject;

                                //SarkarA code change 20180525 //Fix Midnight Report for TangedCo MD
                                switch (chkListMidnightParameters.CheckedItems[selectedItemCount].ToString())
                                {
                                    case "MD kW (1.0.1.6.0.255;4;2)":
                                    case "Maximum Demand - kW (1.0.1.6.0.255;4;2)":
                                    case "Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)":
                                    case "MD kW Date Time (1.0.1.6.0.255;4;5)":
                                    case "MD kVA (1.0.9.6.0.255;4;2)":
                                    case "Maximum Demand - kVA (1.0.9.6.0.255;4;2)":
                                    case "Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)":
                                    case "MD kVA Date Time (1.0.9.6.0.255;4;5)":
                                        continue;
                                    default: 
                                        break;  
                                }
                                //SarkarA code change 20180525 

                                if (groupHeaderTextObject != null)
                                {
                                    groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                }

                            }
                        }
                    }
                }




                ReportObjects rebObjCol = midnightReportNew.Section3.ReportObjects;
                // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                if (reportXSD.Tables["MidnightConsumptionDataTable"].Rows.Count == 0)
                {
                    midnightReportNew.Section3.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                           
                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)                           
                            {
                               if (selectedItemCount != 0)
                                {
                                    //we don't need these three columns for Midnight Consumption
                                    // OBIS Code changed for APSPDCL : Daily Survey Requirement
                                    // Name change for APSPDCL : Daily Survey Requirement
                                    //if (chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration 1 or 2 Phases (1.0.96.0.165.255;3;2) dd:hh:mm" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm")
                                    //{

                                        TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;
                                        // if (chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "MD kW (1.0.1.6.0.255;4;2)" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "MD kVA (1.0.9.6.0.255;4;2)")
                                        //if (chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() == "MD kW (1.0.1.6.0.255;4;2)" || chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() == "MD kVA (1.0.9.6.0.255;4;2)")
                                        //    groupHeaderTextObject.Text = string.Empty;
                                        //else
                                        //{
                                       if (groupHeaderTextObject != null)
                                        {
                                            groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                        }
                                       // }

                                    //}
                                    else
                                        continue;

                                }
                            }
                        }
                    }
                }
                if (reportXSD.Tables["MidnightEnergyDataTable"].Rows.Count == 0)
                {
                    midnightReportNew.DetailSection2.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    ReportObjects rebObjCol1 = midnightReportNew.DetailSection2.ReportObjects;
                    // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol1)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)
                            {
                                if (selectedItemCount != 0)
                                {
                                    TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;

                                    if (groupHeaderTextObject != null)
                                    {
                                        groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                // Apply modern blue theme and custom logo before rendering
                ReportThemeHelper.Apply(midnightReportNew);
                midnightReportNew.SetDataSource(reportXSD);
                ObjRptForm.rptViewer.ReportSource = midnightReportNew;
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
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "ShowReport()", ex);
            }
        }




        private void ShowReportTNEB()// For TNEB
        {
            ReportForm ObjRptForm = new ReportForm();
            MidnightReportNew midnightReportNew = new MidnightReportNew();
            try
            {
            if (chkListMidnightParameters.CheckedItems.Count <= 0)
            {
                return;
            }
               CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)midnightReportNew.ReportDefinition.ReportObjects["txtBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                ReportObjects rebObjCol = midnightReportNew.Section3.ReportObjects;
               if (reportXSD.Tables["MidnightConsumptionDataTable"].Rows.Count == 0)
                {
                    midnightReportNew.Section3.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            int itemcounter = 0;
                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count*2; selectedItemCount++)
                            {
                                if (selectedItemCount != 0)
                                {
                                    TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;
                                      itemcounter = selectedItemCount % 3;
                                    if (groupHeaderTextObject != null)
                                    {
                                        groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[itemcounter].ToString();
                                    }
                                  else
                                        continue;

                                }
                            }
                        }
                    }
                }
                if (reportXSD.Tables["MidnightEnergyDataTable"].Rows.Count == 0)
                {
                    midnightReportNew.DetailSection2.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    ReportObjects rebObjCol1 = midnightReportNew.DetailSection2.ReportObjects;
                   foreach (ReportObject reportObject in rebObjCol1)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)
                            {
                                if (selectedItemCount != 0)
                                {
                                    TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;
                                   
                                    if (groupHeaderTextObject != null)
                                    {
                                        groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                // Apply modern blue theme and custom logo before rendering
                ReportThemeHelper.Apply(midnightReportNew);
                midnightReportNew.SetDataSource(reportXSD);
                ObjRptForm.rptViewer.ReportSource = midnightReportNew;
                Cursor.Current = Cursors.Default;
                ObjRptForm.rptViewer.Zoom(1);
                this.Hide();
                ObjRptForm.ShowDialog();
                reportXSD.Clear();
                this.Show();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "ShowReportTNEB()", ex);
            }
        }


        private void ShowNetReport()
        {
            try
            {
                int maxColumns = 13;
                if (chkListMidnightParameters.CheckedItems.Count <= 0)
                {
                    MessageBox.Show("Please select atleast one parameter for report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (chkListMidnightParameters.CheckedItems.Count > maxColumns)
                {
                    MessageBox.Show("Selected parameters can not exceed maxLimit (" + maxColumns + ")", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                //dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //common = new DLMS650CommonBLL();
                DataSet dataSet = new DataSet();
                MidnightReportNew midnightReportNew = new MidnightReportNew();

                DataSet midnightDS = new DataSet();
                long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                this.Cursor = Cursors.WaitCursor;
                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetails(detailsDS);
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                        FillMeterID(meterIDDS);
                }

                DLMS650MidnightDataBLL midNightBLL = new DLMS650MidnightDataBLL();

                midnightDS = midNightBLL.GetGenericMidNightData(id, descendingOrder);



                dataSet = midNightBLL.GetGenericMidNightConsumptionData(id, descendingOrder);

                //SarkarA code change 20180525 //Fix Midnight Report for TangedCo MD
                for (int i = dataSet.Tables[0].Columns.Count - 1; i >= 0; i-- )
                {
                    switch (dataSet.Tables[0].Columns[i].ColumnName)
                    {
                        case "Maximum Demand - kW (1.0.1.6.0.255;4;2)":
                        case "MD kW (1.0.1.6.0.255;4;2)":
                        case "Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)":
                        case "MD kW Date Time (1.0.1.6.0.255;4;5)":
                        case "Maximum Demand - kVA (1.0.9.6.0.255;4;2)":
                        case "MD kVA (1.0.9.6.0.255;4;2)":
                        case "Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)":
                        case "MD kVA Date Time (1.0.9.6.0.255;4;5)":
                            dataSet.Tables[0].Columns.RemoveAt(i);
                            break;
                        default:
                            break;
                    }
                }
                //SarkarA code change 20180525 

                if (midnightDS != null && midnightDS.Tables[0].Rows.Count > 0)
                {
                    FillMidnightEnergyXSD_NET(midnightDS.Tables[0],"MidnightEnergyNET");
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        FillMidnightEnergyXSD_NET(dataSet.Tables[0],"MidnightConsumptionNET");
                    }
                    else
                    {
                        MessageBox.Show("Midnight Consumption Data Not Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                    
                    ShowReport();                   
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    MessageBox.Show("No data available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowNetReport()", ex);
                
            }
        }

        private bool CheckListContainsColumn(string ColumnName)
        {
            bool flag = false;
            try
            {
                foreach (string columnName in chkListMidnightParameters.CheckedItems)
                {
                    if (columnName.Contains(ColumnName))
                    {
                        flag = true;
                        break;
                    }
                    //SarkarA code change 20180525 //Fix Midnight Report for TangedCo MD
                    else if (columnName.Contains("(1.0.1.6.0.255;4;2)") && ColumnName.Contains("(1.0.1.6.0.255;4;2)"))
                    {
                        flag = true;
                        break;
                    }
                    else if (columnName.Contains("(1.0.9.6.0.255;4;2)") && ColumnName.Contains("(1.0.9.6.0.255;4;2)"))
                    {
                        flag = true;
                        break;
                    }
                    //SarkarA code change 20180525 
                    else if (columnName.Contains("(1.0.9.6.0.255;4;2)") && ColumnName.Contains("(1.0.9.6.0.255;4;2)"))
                    {
                        flag = true;
                        break;
                    }
                    else if (columnName.Contains("(1.0.1.8.0.255;3;2)") && ColumnName.Contains("(1.0.1.8.0.255;3;2)"))
                    {
                        flag = true;
                        break;
                    }
                    else if (columnName.Contains("(1.0.9.8.0.255;3;2)") && ColumnName.Contains("(1.0.9.8.0.255;3;2)"))
                    {
                        flag = true;
                        break;
                    }
                }
        }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckListContainsColumn(string ColumnName)", ex);
                
            }
            return flag;
        }


        private void FillMidnightEnergyXSD_NET(DataTable dtMainEnergy, string XSDTableName)
        {
            try
            {
                DataRow reportRow;
                foreach (DataRow row in dtMainEnergy.Rows)
                {
                    reportRow = reportXSD.Tables[XSDTableName].NewRow();
                    //int iter = 0;
                    for (int colCount = 0; colCount < dtMainEnergy.Columns.Count; colCount++)
                    {
                        string clmName = dtMainEnergy.Columns[colCount].ColumnName;
                        if (CheckListContainsColumn(clmName))
                        {
                            //column 1 Check if 0 is coming, make it "-----" not required as per earlier logic implementation.
                            //if (colCount == 0)
                            //{
                            reportRow[colCount] = CommonBLL.GetFormattedData(row[colCount].ToString());
                            //}
                            //else if (colCount == 1)
                            //{
                            //    if (row[colCount].ToString() != "0")
                            //        reportRow[iter++] = row[colCount].ToString();// + "00",dateFormat);
                            //    else
                            //        reportRow[iter++] = dateUnavailable;
                            //}
                            //else
                            //{
                            //    reportRow[iter++] = CommonBLL.GetFormattedData(row[colCount].ToString());
                            //}
        }
                    }
                    reportXSD.Tables[XSDTableName].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillMidnightEnergyXSD_NET(DataTable dtMainEnergy, string XSDTableName)", ex);
            }  
        }

     


        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {


                //if (MeterVariant == CAB.Framework.MeterVariant.TWO || MeterVariant == CAB.Framework.MeterVariant.THREE || MeterVariant == CAB.Framework.MeterVariant.FOUR)
                //if(true)
                //{

                ShowNetReport();//Now All Midnight Reports for all MeterVariant handelled by NetReport(Generic)
                //}
                //else if (MeterVariant == CAB.Framework.MeterVariant.ONE && MeterModelNumber == NamePlateConstants.SapphireLTCT)
                //{
                //    ShowNetReport();

                //}
                //else
                //{
                //    common = new DLMS650CommonBLL();
                //    dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //    DataSet dataSet = new DataSet();
                //    MidnightReportNew midnightReportNew = new MidnightReportNew();
                //    DataSet midnightDS = new DataSet();
                //    long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                //    DataSet detailsDS = new DataSet();
                //    DataSet meterIDDS = new DataSet();
                //    this.Cursor = Cursors.WaitCursor;
                //    detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //    if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                //        FillConsumerMeterDetails(detailsDS);
                //    else
                //    {
                //        meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                //        if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                //            FillMeterID(meterIDDS);
                //    }
                //    types = ConfigInfo.GetApplicationType();


                //    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //    {
                //        DataSet TempDS = new DataSet();
                //        DLMS650CommonBLL ds = new DLMS650CommonBLL();
                //        TempDS = new DLMS650MidnightDataBLL().ListDataSetColumnWise(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                //        midnightDS = ds.ConvertMidnightEnergyDataWiseReportWithoutEMF(TempDS);// change for HTCT mega variant


                //        dataSet = common.ConvertMidnightEnergyDataWiseReportWithoutEMF(dlms650LoadSurveyBLL.GetPUMADailyConsumption(Convert.ToInt64(id)));
                //    }



                //    //else if (types.Equals(ApplicationType.IEC_LTCT_650))
                //    //{
                //    //    midnightDS = new DLMS650MidnightDataBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), types.ToString());
                //    //}

                //    if (midnightDS != null && midnightDS.Tables[0].Rows.Count > 0)
                //    {
                //        FillMidnightEnergyXSD(midnightDS);
                //        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                //        {

                //            //if (chkListMidnightParameters.CheckedItems.Count <= 4)// For TNEB
                //            //{
                //            //    FillMidnightConsumptionXSD_TNEB(dataSet);
                //            //}
                //            //else
                //            //{
                //            FillMidnightConsumptionXSD(dataSet);
                //            //}
                //        }
                //        else
                //        {
                //            MessageBox.Show("Midnight Consumption Data Not Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //        //TNEB code commented to support single phase JVVNL
                //        //if (chkListMidnightParameters.CheckedItems.Count <= 4)// For TNEB
                //        //{
                //        //    ShowReportTNEB();// For TNEB
                //        //}
                //        //else
                //        //{
                //        ShowReport();
                //        //}
                //        this.Cursor = Cursors.Default;
                //    }
                //    else
                //    {
                //        MessageBox.Show("No data available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        this.Cursor = Cursors.Default;
                //    }
                //}
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                
            }          
        }

        private void FillMidnightEnergyXSD(DataSet midnightData)
        {
            midnightHeadings = new List<string>();
            DataRow reportRow;
            DateTime PreviousDate = DateTime.Now;

            try
            {
                reportXSD.Tables["MidnightEnergyDataTable"].Rows.Clear();
                if (midnightData == null || midnightData.Tables[0].Rows.Count == 0)
                    return;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["MidnightEnergyDataTable"].NewRow();

                    foreach (string columnName in chkListMidnightParameters.CheckedItems)
                    {
                        midnightHeadings.Add(columnName);
                    }

                    foreach (DataRow row in midnightData.Tables[0].Rows)
                    {
                        DataTable MidnightTable = new DataTable();
                        MidnightTable = reportXSD.Tables["MidnightEnergyDataTable"];
                        reportRow = MidnightTable.NewRow();
                        string dateTimes = Convert.ToString(row[0]);
                        if (dateTimes.Length > 10)
                            dateTimes = dateTimes.Substring(0, 10);
                        for (int colCount = 0; colCount < midnightHeadings.Count; colCount++)
                        {
                            if (colCount == 0)
                            { reportRow["DateTime"] = DateUtility.LongToStringDate(Convert.ToInt64(row[0]));
                            }
                            else if (midnightHeadings[colCount].Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                            {
                                reportRow["Parameter" + colCount.ToString()] = row[midnightHeadings[colCount]].ToString();
                            }
                            else
                            {
                                reportRow["Parameter" + colCount.ToString()] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[midnightHeadings[colCount]].ToString()));
                            }
                        }
                        MidnightTable.Rows.Add(reportRow);
                    }
                }

                for (int i = 0; i < reportXSD.Tables["MidnightEnergyDataTable"].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter5"])))
                        reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter5"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter5"])));

                    if (!string.IsNullOrEmpty(Convert.ToString(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter6"])))
                        reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter6"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter6"])));

                    if (!string.IsNullOrEmpty(Convert.ToString(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter7"])))
                        reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter7"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(reportXSD.Tables["MidnightEnergyDataTable"].Rows[i]["Parameter7"])));
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "FillMidnightEnergyXSD(DataSet midnightData)", ex);
                //MessageBox.Show(ex.Message);
            }
        }

        private void FillMidnightConsumptionXSD(DataSet midnightData)
        {
            midnightHeadings = new List<string>();
            DataRow reportRow;
            DateTime PreviousDate = DateTime.Now;

            try
            {
                reportXSD.Tables["MidnightConsumptionDataTable"].Rows.Clear();
                if (midnightData == null || midnightData.Tables[0].Rows.Count == 0)
                    return;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["MidnightConsumptionDataTable"].NewRow();

                    foreach (string columnName in chkListMidnightParameters.CheckedItems)
                    {
                        //if (columnName != "MD kW (1.0.1.6.0.255;4;2)" && columnName != "MD kVA (1.0.9.6.0.255;4;2)")
                        midnightHeadings.Add(columnName);
                    }
                    foreach (DataRow row in midnightData.Tables[0].Rows)
                    {
                        DataTable MidnightTable = new DataTable();
                        MidnightTable = reportXSD.Tables["MidnightConsumptionDataTable"];
                        reportRow = MidnightTable.NewRow();
                        string dateTimes = Convert.ToString(row[0]);
                        if (dateTimes.Length > 10)
                            dateTimes = dateTimes.Substring(0, 10);
                        for (int colCount = 0; colCount < midnightHeadings.Count; colCount++)
                        {
                            // OBIS Code changed for APSPDCL : Daily Survey Requirement
                            // Name change for APSPDCL : Daily Survey Requirement
                            //if (midnightHeadings[colCount].ToString() != "Power On Duration 1 or 2 Phases (1.0.96.0.165.255;3;2) dd:hh:mm" && midnightHeadings[colCount].ToString() != "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm" && midnightHeadings[colCount].ToString() != "Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm" && midnightHeadings[colCount].ToString() != "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm")
                            //{
                            if (colCount == 0)
                            { reportRow["DateTime"] = Convert.ToString(row[0])/*.Substring(11, Convert.ToString(row[0]).Length - 11)*/;
                            }
                            else if (midnightHeadings[colCount].Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                            {
                                reportRow["Parameter" + colCount.ToString()] = row[midnightHeadings[colCount]].ToString();
                            }
                            else if (midnightHeadings[colCount].Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"))
                            {
                                reportRow["Parameter" + colCount.ToString()] = row[midnightHeadings[colCount]].ToString();
                            }
                            
                            
                            else
                            {
                                reportRow["Parameter" + colCount.ToString()] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[midnightHeadings[colCount]].ToString()));
                            }
                            //}
                            //else
                            //{
                            //    continue;
                            //}
                        }
                        MidnightTable.Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "FillMidnightConsumptionXSD(DataSet midnightData)", ex);
                //MessageBox.Show(ex.Message);
            }
        }

        private void FillMidnightConsumptionXSD_TNEB(DataSet midnightData)//For TNEB
        {
            midnightHeadings = new List<string>();
            DataRow reportRow;
            DateTime PreviousDate = DateTime.Now;

            try
            {
                reportXSD.Tables["MidnightConsumptionDataTable"].Rows.Clear();
                if (midnightData == null || midnightData.Tables[0].Rows.Count == 0)
                    return;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    reportRow = reportXSD.Tables["MidnightConsumptionDataTable"].NewRow();

                    foreach (string columnName in chkListMidnightParameters.CheckedItems)
                    {
                        //if (columnName != "MD kW (1.0.1.6.0.255;4;2)" && columnName != "MD kVA (1.0.9.6.0.255;4;2)")
                        midnightHeadings.Add(columnName);
                    }
                    DataTable MidnightTable = new DataTable();
                    MidnightTable = reportXSD.Tables["MidnightConsumptionDataTable"];
                    reportRow = MidnightTable.NewRow();
                    // In case kwh,kvah headings 2 times add for all meters including TNEB
                    if (midnightHeadings.Count < 4)
                    {
                        foreach (string columnName in chkListMidnightParameters.CheckedItems)
                        {
                            midnightHeadings.Add(columnName);
                        }
                        // for TNEB
                        int itercount = 0;
                        
                        for (int rowcount = 0; rowcount < midnightData.Tables[0].Rows.Count; rowcount++)
                        {
                            for (int colCount = 0; colCount < 3; colCount++)
                            {
                                if (itercount == 0)
                                {
                                    reportRow["DateTime"] = midnightData.Tables[0].Rows[rowcount][colCount];
                                }
                                else
                                {
                                    reportRow["Parameter" + itercount.ToString()] = midnightData.Tables[0].Rows[rowcount][colCount];
                                }
                                itercount++;
                            }

                        }

                    }
                        MidnightTable.Rows.Add(reportRow);
                                    
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillMidnightConsumptionXSD_TNEB(DataSet midnightData)", ex);
            }
        }
       
        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MidNightReportFileWise_Load(object sender, EventArgs e)
        {
            try
            {
                DLMS650GeneralBLL objDLMS650GeneralBLL = new DLMS650GeneralBLL();
                MeterVariant = objDLMS650GeneralBLL.GetMeterVariantByMeterDataID(ConfigInfo.ActiveMeterDataId);
                MeterModelNumber = objDLMS650GeneralBLL.GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
                // Check utility
                if (UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
                {
                    isPUMA = true;
                }

             //   if (MeterVariant == CAB.Framework.MeterVariant.TWO || MeterVariant == CAB.Framework.MeterVariant.THREE || MeterVariant == CAB.Framework.MeterVariant.FOUR)
                //if(true)
                //{
                FillMidNightParametersNET();//Now All Midnight Reports for all MeterVariant handelled by NetReport(Generic)
                //}
                    //No of parameters are large.Can not handle in static report so net metering report is used
                //else if (MeterVariant == CAB.Framework.MeterVariant.ONE && MeterModelNumber == NamePlateConstants.SapphireLTCT)
                //{
                //    FillMidNightParametersNET();
                //}
                //else
                //{
                //    FillMidNightParameters();
                //}

                if (chkListMidnightParameters.Items.Count > 0)
                {
                    chkListMidnightParameters.SetItemCheckState(0, CheckState.Checked);
                    chkListMidnightParameters.ItemCheck += new ItemCheckEventHandler(chkListMidnightParameters_ItemCheck);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "MidNightReportFileWise_Load(object sender, EventArgs e)", ex);

            }

        }   


        private void FillMidNightParametersNET()
        {
            try
            {
                DataSet midnightDS = new DataSet();
                DataSet TempDS = new DataSet();
                types = ConfigInfo.GetApplicationType();
                long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                //if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //{
                //    midnightDS = new DLMS650MidnightDataBLL().ListDataSetColumnWise(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                //}
                //else if (types.Equals(ApplicationType.IEC_LTCT_650))
                //{
                //    midnightDS = new DLMS650MidnightDataBLL().ListDataSet(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                //}
                DLMS650CommonBLL ds = new DLMS650CommonBLL();
                DLMS650MidnightDataBLL dlms650MidNightDataBLL = new DLMS650MidnightDataBLL();
                TempDS = dlms650MidNightDataBLL.GetGenericMidNightData(id, descendingOrder);
                midnightDS = ds.ConvertMidnightEnergyDataWiseReportWithoutEMF(TempDS);// change for HTCT mega variant
                if (midnightDS != null)
                {
                    if (midnightDS.Tables != null)
                    {
                        if (midnightDS.Tables[0].Rows.Count == 0)
                        {
                            chkListMidnightParameters.Items.Clear();
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "Report can not be shown";
                            return;
                        }
                        if (midnightDS.Tables[0].Columns.Count > 0)
                        {
                            lblNoDataFound.Visible = false;
                            chkListMidnightParameters.Items.Clear();
                            foreach (DataColumn column in midnightDS.Tables[0].Columns)
                            {
                                chkListMidnightParameters.Items.Add(column.ColumnName);
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
                for (int itemCounter = 0; itemCounter < chkListMidnightParameters.Items.Count; itemCounter++)
                {
                    chkListMidnightParameters.SetItemCheckState(itemCounter, CheckState.Checked);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillMidNightParametersNET()", ex);
            }         
        }

        private void chkListMidnightParameters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (MeterVariant == CAB.Framework.MeterVariant.ONE)
                {
            e.NewValue = CheckState.Checked;
        }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "chkListMidnightParameters_ItemCheck(object sender, ItemCheckEventArgs e)", ex);
            }         
        }

        private void FillMidNightParameters()
        {
            try
            {
            DataSet midnightDS = new DataSet();
            DataSet TempDS = new DataSet();
            DLMS650CommonBLL ds = new DLMS650CommonBLL();
            types = ConfigInfo.GetApplicationType();
            long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                TempDS = new DLMS650MidnightDataBLL().ListDataSetColumnWise(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                midnightDS = ds.ConvertMidnightEnergyDataWiseReportWithoutEMF(TempDS);// change for HTCT mega variant
            }
            else if (types.Equals(ApplicationType.IEC_LTCT_650))
            {
                midnightDS = new DLMS650MidnightDataBLL().ListDataSet(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
            }
            if (midnightDS != null)
            {
                if (midnightDS.Tables != null)
                {
                    if (midnightDS.Tables[0].Rows.Count == 0)
                    {
                        chkListMidnightParameters.Items.Clear();
                        lblNoDataFound.Visible = true;
                        lblNoDataFound.Text = "Report can not be shown";
                        return;
                    }
                    if (midnightDS.Tables[0].Columns.Count > 0)
                    {
                        lblNoDataFound.Visible = false;
                        chkListMidnightParameters.Items.Clear();
                        foreach (DataColumn column in midnightDS.Tables[0].Columns)
                        {
                            chkListMidnightParameters.Items.Add(column.ColumnName);
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
            for (int itemCounter = 0; itemCounter < chkListMidnightParameters.Items.Count; itemCounter++)
            {
                chkListMidnightParameters.SetItemCheckState(itemCounter, CheckState.Checked);
            }
        }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillMidNightParameters()", ex);
                
            }         
        }
    }
}
