using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Framework.Utility;
using CAB.Framework;
using CABApplication.Reports.Forms;
using CAB.BLL;
using CAB.UI.Controls;
using CrystalDecisions.CrystalReports.Engine;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CrystalDecisions.Shared;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class MidNightReportMeterIDWise : CABForm
    {
        DLMS650CommonBLL ds = new DLMS650CommonBLL();
        private FileReportDataSet reportXSD = null;
        static List<string> midnightHeadings;
        ApplicationType types;
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        string dateUnavailable = "--------";
        private DLMS650MidnightDataBLL midnightDataBll;
        private MidnightParameterBLL midnightParameterBLL;
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        private long changedFromDate;
        private long changedToDate;
        bool isPUMA = false;
        DataSet midnightDS = null;
        DLMS650LoadSurveyBLL dlms650LoadSurveyBLL;
        DLMS650CommonBLL common;
        string meterVariant = string.Empty;
        private const string ascendingOrder = "asc";
        private const string descendingOrder = "desc";
        private int MeterModelNumber;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MidNightReportMeterIDWise).ToString());
        public MidNightReportMeterIDWise()
        {
            try
            {
                midnightDataBll = new DLMS650MidnightDataBLL();
                reportXSD = new FileReportDataSet();
                InitializeComponent();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "MidNightReportMeterIDWise()", ex);
                
            }            
        }

        private static string ActiveMeterIdMidnight
        {
            get;
            set;
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

        private DataSet ListConsumerMeterDetailForMeterID(string activeMeterId)
        {
            return new MeterDataBLL().GetConsumerMeterDetailsForMeterID(activeMeterId);
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
                            { 
                                reportRow["DateTime"] = DateUtility.LongToStringDate(Convert.ToInt64(row[0])); 
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
            }
            catch (Exception ex)
            {
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
                        if (columnName != "MD kW (1.0.1.6.0.255;4;2)" && columnName != "MD kVA (1.0.9.6.0.255;4;2)")
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
                            if (/*midnightHeadings[colCount].ToString() != "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm" &&*/ midnightHeadings[colCount].ToString() != "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm" 
                                //&& midnightHeadings[colCount].ToString() != "Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm" && midnightHeadings[colCount].ToString() != "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"
                                )
                            {
                                if (colCount == 0)
                                { reportRow["DateTime"] = Convert.ToString(row[0])/*.Substring(11, Convert.ToString(row[0]).Length - 11)*/; }
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
                            }
                            else
                            {
                                continue;
                            }

                        }
                        MidnightTable.Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillMidnightConsumptionXSD(DataSet midnightData)", ex);
            }
        }

        private void chkListMidnightParameters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (meterVariant == MeterVariant.ONE)
                e.NewValue = CheckState.Checked;
        }


        /// <summary>
        /// event handler for radioGroupBox , when a radiobutton is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton radioButtonCheck = sender as RadioButton;
                if (radioButtonCheck != null)
                {
                    if (radioButtonCheck.Checked)
                    {
                        dateTimeStart.Enabled = true;
                        dateTimeEnd.Enabled = true;

                        btnShow.Enabled = true;
                        //selectedRadioButtonCount = groupBox.Controls.GetChildIndex(radioButtonCheck) + 1;
                        // ConfigInfo.ActiveMeterDataIdLoadSurvey = selectedRadioButtonCount;

                        //ActiveMeterIdLoadSurvey = mainForm.MeterId[selectedRadioButtonCount-1];
                        ActiveMeterIdMidnight = radioButtonCheck.Text;
                        DataSet meterDataIdList = new MeterDataBLL().GetMeterDataIDFromMeterID(ActiveMeterIdMidnight);                      
                        if (meterDataIdList != null && meterDataIdList.Tables.Count > 0 && meterDataIdList.Tables[0].Rows.Count > 0)
                        {
                            ConfigInfo.ActiveMeterDataId = meterDataIdList.Tables[0].Rows[0][0].ToString();
                        }
                        DLMS650GeneralBLL objDLMS650GeneralBLL = new DLMS650GeneralBLL();
                        meterVariant = objDLMS650GeneralBLL.GetMeterVariantByMeterDataID(ConfigInfo.ActiveMeterDataId);
                        MeterModelNumber = objDLMS650GeneralBLL.GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);                       

                        SetupDateTime();
                        lblNoDataFound.Visible = false;
                        //resetting mindate and maxvalue
                       
                        //if (meterVariant == CAB.Framework.MeterVariant.TWO || meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                       // {
                        FillMidNightParametersNET();//Now All Midnight Reports for all MeterVariant handelled by NetReport(Generic)
                       // }
                        //No of parameters are large.Can not handle in static report so net metering report is used
                        //else if (meterVariant == CAB.Framework.MeterVariant.ONE && MeterModelNumber == NamePlateConstants.SapphireLTCT)
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
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "radioBox_CheckedChanged(object sender, EventArgs e)", ex);
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
               DLMS650MidnightDataBLL dlms650MidNightDataBLL = new DLMS650MidnightDataBLL();
                TempDS = dlms650MidNightDataBLL.GetGenericMidNightConsumptionData(id, descendingOrder);                 
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
            //if (chkListMidnightParameters.CheckedItems.Count <= 0)
            //{
            //    return;
            //}
            ReportForm ObjRptForm = new ReportForm();
            MidnightReportNew midnightReportNew = new MidnightReportNew();
            try
            {
                /* Add BCS Version in Report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)midnightReportNew.ReportDefinition.ReportObjects["txtBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* Add BCS Version in Report header */



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

                                if (groupHeaderTextObject != null)
                                {
                                    groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                }

                            }
                        }
                    }
                }






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

                            for (int selectedItemCount = 0; selectedItemCount < chkListMidnightParameters.CheckedItems.Count; selectedItemCount++)
                            {
                                if (selectedItemCount != 0)
                                {
                                    // OBIS Code changed for APSPDCL : Daily Survey Requirement
                                    // Name change for APSPDCL : Daily Survey Requirement
                                    // New OBIS Code (1.0.94.91.13.255;3;2) added for JVVNL : Daily Survey Requirement
                                    //if (/*chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm" &&*/ chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm" && chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() != "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm")
                                    //{
                                        TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;

                                        //if (chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() == "MD kW (1.0.1.6.0.255;4;2)" || chkListMidnightParameters.CheckedItems[selectedItemCount].ToString() == "MD kVA (1.0.9.6.0.255;4;2)")
                                        //    groupHeaderTextObject.Text = string.Empty;
                                        //else
                                        //{
                                            if (groupHeaderTextObject != null)
                                            {
                                                groupHeaderTextObject.Text = chkListMidnightParameters.CheckedItems[selectedItemCount].ToString();
                                            }
                                        //}

                                    //}
                                    else
                                        continue;
                                }
                            }
                        }
                    }
                }

                ReportObjects rebObjCol1 = midnightReportNew.DetailSection2.ReportObjects;
                // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                if (reportXSD.Tables["MidnightEnergyDataTable"].Rows.Count == 0)
                {
                    midnightReportNew.DetailSection2.SectionFormat.EnableSuppress = true;
                }
                else
                {
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
                //ObjRptForm.ShowDialog();
                ObjRptForm.Show();
                // SB code change End - 20180629 - Multiple Analysis View
                reportXSD.Clear();
                this.Show();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "ShowReport()", ex);
            }
        }

        private void FillMidNightParameters()
        {
            try
            {
                DataSet TempDS = new DataSet();
                DataSet midnightDS = new DataSet();
                types = ConfigInfo.GetApplicationType();
                string MeterID = null;
                DataSet dSet = null;
                if (ActiveMeterIdMidnight != null)
                {
                    MeterID = ActiveMeterIdMidnight;
                }
                long id = 0;

                if (MeterID != null)
                    dSet = midnightParameterBLL.GetColumnNamesForMeterID(MeterID);

                if (types.Equals(ApplicationType.DLMS_LTCT_650) && MeterID != null)
                {
                    if (dSet.Tables[0].Rows.Count == 1)
                    {
                        long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeStart.Value.ToShortDateString() + " 00:00:00"));
                        long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeEnd.Value.ToShortDateString() + " 23:59:59"));
                        midnightDS = new DLMS650MidnightDataBLL().ListDataSetColumnWiseForMeterID(MeterID, frmDate, toDate, types.ToString());
                    }
                    else if (dSet.Tables[0].Rows.Count > 1)
                    {
                        MessageBox.Show("Parameter mismatch in readout files.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        btnShow.Enabled = false;
                    }
                }
                //if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //{
                //    midnightDS = new DLMS650MidnightDataBLL().ListDataSetColumnWise(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                //}
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeStart.Value.ToShortDateString() + " 00:00:00"));
                    long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeEnd.Value.ToShortDateString() + " 23:59:59"));                    
                    TempDS = new DLMS650MidnightDataBLL().ListDataSetColumnWiseForMeterID(MeterID, frmDate, toDate, types.ToString());
                    midnightDS = ds.ConvertMidnightEnergyDataWiseReportWithoutEMF(TempDS);// change for HTCT mega variant
                    //midnightDS = new DLMS650MidnightDataBLL().ListDataSet(id, new DLMS650MidnightDataBLL().GetFromDate(id), new DLMS650MidnightDataBLL().GetToDate(id), types.ToString());
                }
                if (midnightDS != null && midnightDS.Tables != null && midnightDS.Tables.Count > 0)
                {
                    if (midnightDS.Tables[0].Rows.Count == 0)
                    {
                        chkListMidnightParameters.Items.Clear();
                        //lblNoDataFound.Visible = true;
                        //lblNoDataFound.Text = "Report can not be shown";
                        return;
                    }
                    if (midnightDS.Tables[0].Columns.Count > 0)
                    {
                        //lblNoDataFound.Visible = false;
                        chkListMidnightParameters.Items.Clear();
                        foreach (DataColumn column in midnightDS.Tables[0].Columns)
                        {
                            chkListMidnightParameters.Items.Add(column.ColumnName);
                        }
                    }
                    else
                    {
                        //lblNoDataFound.Visible = true;
                        //lblNoDataFound.Text = "No Data Found";
                    }

                }
                else
                {
                    //lblNoDataFound.Visible = true;
                    //lblNoDataFound.Text = "No Data Found";
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

        /// <summary>
        /// check if start date and end date are same ,if yes , then add one day to end date 
        /// </summary>
        private void SetupDateTime()
        {
            long leastDate = midnightDataBll.GetMinDateForMeterID(ActiveMeterIdMidnight);
            long maxDate = midnightDataBll.GetMaxDateForMeterID(ActiveMeterIdMidnight);
            try
            {
                dateTimeStart.MinDate = DateTimePicker.MinimumDateTime;
                dateTimeStart.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimeEnd.MinDate = DateTimePicker.MinimumDateTime;
                dateTimeEnd.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimeStart.Value = DateUtility.LongToDateTime(leastDate);
                dateTimeEnd.Value = DateUtility.LongToDateTime(maxDate);

                if (dateTimeStart.Value != dateTimeEnd.Value)
                {
                    dateTimeStart.MinDate = dateTimeStart.Value;
                    dateTimeStart.MaxDate = dateTimeEnd.Value;
                    dateTimeEnd.MinDate = dateTimeStart.Value;
                    dateTimeEnd.MaxDate = dateTimeEnd.Value;

                }
                else if (dateTimeStart.Value == dateTimeEnd.Value)
                {
                    dateTimeStart.MinDate = dateTimeStart.Value;
                    dateTimeStart.MaxDate = dateTimeEnd.Value.AddDays(1);
                    dateTimeEnd.MinDate = dateTimeStart.Value;
                    dateTimeEnd.MaxDate = dateTimeEnd.Value.AddDays(1);
                }
                dateTimeStart.Value = DateUtility.LongToDateTime(leastDate);
                dateTimeEnd.Value = DateUtility.LongToDateTime(maxDate);
            }
            catch (Exception e)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SetupDateTime()", e);
                e.GetBaseException();
                
            }
        }

        private void dateTimeStart_DropDown(object sender, EventArgs e)
        {
            try
            {
                changedFromDate = DateUtility.DateTimeToLong(dateTimeStart.Value);
                changedToDate = DateUtility.DateTimeToLong(dateTimeEnd.Value);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dateTimeStart_DropDown(object sender, EventArgs e)", ex);
                
            }
         
        }

        private void dateTimeStart_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (dateTimeStart.Value > dateTimeEnd.Value)
                {
                    MessageBox.Show("StartDate cannot be greater than EndDate.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dateTimeStart.Value = DateUtility.LongToDateTime(changedFromDate);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dateTimeStart_CloseUp(object sender, EventArgs e)", ex);
                
            }
            
        }

        private void dateTimeEnd_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (dateTimeStart.Value > dateTimeEnd.Value)
                {
                    MessageBox.Show("StartDate cannot be greater than EndDate.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dateTimeStart.Value = DateUtility.LongToDateTime(changedFromDate);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "dateTimeEnd_CloseUp(object sender, EventArgs e)", ex);
            }
     

        }

        private void dateTimeEnd_DropDown(object sender, EventArgs e)
        {
            try
            {
                changedFromDate = DateUtility.DateTimeToLong(dateTimeStart.Value);
                changedToDate = DateUtility.DateTimeToLong(dateTimeEnd.Value);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dateTimeEnd_DropDown(object sender, EventArgs e)", ex);
                
            }
          
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActiveMeterIdMidnight == null)
                {
                    MessageBox.Show("No MeterID Selected.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //if (meterVariant == MeterVariant.TWO || meterVariant == MeterVariant.THREE || meterVariant == MeterVariant.FOUR)
                //{
                ShowNetReport();//Now All Midnight Reports for all MeterVariant handelled by NetReport(Generic)
                //}
                //else if (meterVariant == CAB.Framework.MeterVariant.ONE && MeterModelNumber == NamePlateConstants.SapphireLTCT)
                //{
                //    ShowNetReport();

                //}
                //else
                //{
                //    dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                //    common = new DLMS650CommonBLL();
                //    DataSet dataSet = new DataSet();
                //    //if (chkListMidnightParameters.CheckedItems.Count <= 0)
                //    //{
                //    //    return;
                //    //}
                //    DataSet TempDS = new DataSet();
                //    DataSet midnightDS = new DataSet();
                //    DataSet detailsDS = new DataSet();
                //    DataSet meterIDDS = new DataSet();
                //    this.Cursor = Cursors.WaitCursor;
                //    detailsDS = ListConsumerMeterDetailForMeterID(ActiveMeterIdMidnight);
                //    if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                //        FillConsumerMeterDetails(detailsDS);
                //    else
                //    {
                //        DataTable meterIDDT = new DataTable();
                //        meterIDDT.Columns.Add("MeterID", typeof(string));
                //        meterIDDT.Rows.Add(ActiveMeterIdMidnight);
                //        meterIDDS.Tables.Add(meterIDDT);
                //        if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                //            FillMeterID(meterIDDS);
                //    }
                //    types = ConfigInfo.GetApplicationType();
                //    if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //    {
                //        long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeStart.Value.ToShortDateString() + " 00:00:00"));
                //        long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeEnd.Value.ToShortDateString() + " 23:59:59"));
                    
                //        TempDS = new DLMS650MidnightDataBLL().ListDataSetColumnWiseForMeterID(ActiveMeterIdMidnight, frmDate, toDate, types.ToString());
                //        midnightDS = ds.ConvertMidnightEnergyDataWiseReportWithoutEMF(TempDS);// change for HTCT mega variant

                //        dataSet = common.ConvertMidnightEnergy(dlms650LoadSurveyBLL.GetMidNightDataForConsumptionMeterIDWise(ActiveMeterIdMidnight, frmDate, toDate));
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
                //            FillMidnightConsumptionXSD(dataSet);
                //        }
                //        else
                //        {
                //            MessageBox.Show("Midnight Consumption Data Not Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //        ShowReport();
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


                if (midnightDS != null && midnightDS.Tables[0].Rows.Count > 0)
                {
                    FillMidnightEnergyXSD_NET(midnightDS.Tables[0], "MidnightEnergyNET");
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        FillMidnightEnergyXSD_NET(dataSet.Tables[0], "MidnightConsumptionNET");
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
                    int iter = 0;
                    for (int colCount = 0; colCount < dtMainEnergy.Columns.Count; colCount++)
                    {
                        string clmName = dtMainEnergy.Columns[colCount].ColumnName;
                        if (CheckListContainsColumn(clmName))
                        {
                            if (colCount == 0)
                            {
                                reportRow[iter++] = CommonBLL.GetFormattedData(row[colCount].ToString());
                            }
                            else if (colCount == 1)
                            {
                                if (row[colCount].ToString() != "0")
                                    reportRow[iter++] = row[colCount].ToString();// + "00",dateFormat);
                                else
                                    reportRow[iter++] = dateUnavailable;
                            }
                            else
                            {
                                reportRow[iter++] = CommonBLL.GetFormattedData(row[colCount].ToString());
                            }
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


        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            ConfigInfo.ActiveMeterDataId = string.Empty;
            this.Close();
        }

        private void MidNightReportMeterIDWise_Load(object sender, EventArgs e)
        {
            try
            {
                //// Check utility
                ActiveMeterIdMidnight = null;
                midnightParameterBLL = new MidnightParameterBLL();
                if (UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
                {
                    isPUMA = true;
                }              
                GroupRadioBoxAdd();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "MidNightReportMeterIDWise_Load(object sender, EventArgs e)", ex);
                
            }
          
        }


        /// <summary>
        /// gets meterID from database and stores it in a list 
        /// </summary>
        /// <returns></returns>
        private List<string> SetRadioButton()
        {
            List<string> radioBoxMeterID = new List<string>();
            try
            {
               
                DataSet meterIdData = new MeterDataBLL().GetMeterIDMidnight(true);
                if (meterIdData != null && meterIdData.Tables != null && meterIdData.Tables.Count > 0)
                {
                    foreach (DataRow row in meterIdData.Tables[0].Rows)
                    {
                        radioBoxMeterID.Add(Convert.ToString(row[0]));
                    }
                }
                radioBoxMeterID.Sort();
               
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "SetRadioButton()", ex);
            }
            return radioBoxMeterID;
      
        }

        /// <summary>
        /// adds meterID's to groupRadioBox user control. 
        /// </summary>
        private void GroupRadioBoxAdd()
        {
            try
            {
                dateTimeStart.Enabled = false;
                dateTimeEnd.Enabled = false;

                List<string> radioButtonMeterID = SetRadioButton();
                if (radioButtonMeterID.Count > 0)
                    AddRadioButtons(radioButtonMeterID.Count, radioButtonMeterID);
                else
                {
                    this.Close();
                    MessageBox.Show("Midnight data not available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }

                for (int i = 0; i < radioBoxPanel.Controls.Count; i++)
                {
                    RadioButton radioBox = (RadioButton)radioBoxPanel.Controls[i];

                    radioBox.CheckedChanged += new EventHandler(radioBox_CheckedChanged);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GroupRadioBoxAdd()", ex);
            }
        

        }
        /// <summary>
        /// dynamically adds radio buttons to the panel
        /// </summary>
        /// <param name="MeterIdCount"></param>
        /// <param name="MeterID"></param>
        public void AddRadioButtons(int MeterIdCount, List<string> MeterID)
        {
            try
            {
                RadioButton radioButton;
                for (int idCounter = 0; idCounter < MeterIdCount; idCounter++)
                {
                    radioButton = new RadioButton();
                    radioButton.Name = MeterID[idCounter];
                    radioButton.Text = MeterID[idCounter];
                    radioButton.Width = 300;
                    radioButton.Location = new Point(5, 22 * idCounter);
                    radioBoxPanel.Controls.Add(radioButton);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "AddRadioButtons(int MeterIdCount, List<string> MeterID)", ex);
            }
         
        }

        
    }
}
