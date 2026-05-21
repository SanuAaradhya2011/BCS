using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Entity;
using CAB.BLL;
using CABApplication.Reports.Forms;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.IECFramework.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.RPTFiles;

namespace CAB.UI
{
    public partial class DaySpecificLoadSurvey : CABForm
    {
        string val = "";
        static List<string> lsHeadings;
        private ReportDataSet reportXSD = new ReportDataSet();
        public DaySpecificLoadSurvey()
        {
            InitializeComponent();
        }

        private void DaySpecificLoadSurvey_Load(object sender, EventArgs e)
        {
            rdbtnDemand.Checked = true;
            long lsFromDate = DateUtility.DateTimeToLong(System.DateTime.Now.Date);
            long lsToDate = DateUtility.DateTimeToLong(System.DateTime.Now.Date);
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            DataSet dateData = loadSurveyBLL.GetDates(Convert.ToInt64(ConfigInfo.ActiveMeterDataId ));
            if (dateData != null)
            {
                if (dateData.Tables.Count != 0)
                {
                    if (dateData.Tables[0].Rows.Count != 0)
                    {
                        lsFromDate = Convert.ToInt64(dateData.Tables[0].Rows[0][0]);
                        lsToDate = Convert.ToInt64(dateData.Tables[0].Rows[dateData.Tables[0].Rows.Count - 1][0]);
                    }
                }
            }
            if (lsFromDate == 0 && lsToDate == 0)
            {
                DTMLoadSurveyBLL dtmLoadSurveyBLL = new DTMLoadSurveyBLL();
                lsFromDate = dtmLoadSurveyBLL.GetFromDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                lsToDate = dtmLoadSurveyBLL.GetToDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            }
            if (lsFromDate != 0)
                dtpFromDate.Value = DateUtility.LongToDateTime(lsFromDate);
            if (lsToDate != 0)
                dtpToDate.Value = DateUtility.LongToDateTime(lsToDate);
        }

        //private DataSet SortDataSet(DataSet dset)
        //{
        //    if (dset == null)
        //        return null;
        //    if (dset.Tables.Count == 0)
        //        return null;
        //    if (dset.Tables[0].Rows.Count == 0)
        //        return null;
        //    DataTable table = new DataTable();
        //    foreach (DataColumn col in dset.Tables[0].Columns)
        //    {
        //        if (Convert.ToString(col.ColumnName) != "MDIntervalPeriod")
        //            table.Columns.Add(Convert.ToString(col.ColumnName));
        //    }
        //    for (int i = dset.Tables[0].Rows.Count - 1; i >= 0; i--)
        //    {
        //        DataRow dr = table.NewRow();
        //        for (int j = 0; j < dset.Tables[0].Columns.Count - 1; j++)
        //            dr[j] = dset.Tables[0].Rows[i][j];
        //        table.Rows.Add(dr);
        //    }
        //    DataSet dsetSorted = new DataSet();
        //    dsetSorted.Tables.Add(table);
        //    return dsetSorted;
        //}
        private DataSet SortDataSet(DataSet dset)
        {
            if (dset == null)
                return null;
            if (dset.Tables.Count == 0)
                return null;
            if (dset.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            foreach (DataColumn col in dset.Tables[0].Columns)
            {
                if (Convert.ToString(col.ColumnName) != "MDIntervalPeriod")
                    table.Columns.Add(Convert.ToString(col.ColumnName));
            }
            for (int i = dset.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = table.NewRow();
                for (int j = 0; j <= dset.Tables[0].Columns.Count - 1; j++)
                    dr[j] = dset.Tables[0].Rows[i][j];
                table.Rows.Add(dr);
            }
            DataSet dsetSorted = new DataSet();
            dsetSorted.Tables.Add(table);
            return dsetSorted;
        } 

        public DataSet GetDataForGrid(DataSet dSetLoadSurvey)
        {
            DateTime date;
            int year = 0;
            int day = 0;
            int month = 0;
            int yearForCheck = 0;
            int dayForCheck = 0;
            int monthforCheck = 0;
            bool isAdded = false;
            decimal maxrPhaseVoltage = 0;
            decimal minrPhaseVoltage = 0;
            decimal maxyPhaseVoltage = 0;
            decimal minyPhaseVoltage = 0;
            decimal maxrPhaseCurrent = 0;
            decimal minrPhaseCurrent = 0;
            decimal maxyPhaseCurrent = 0;
            decimal minyPhaseCurrent = 0;
            decimal maxdemandkvarlead = 0;
            decimal mindemandkvarlead = 0;
            decimal maxDemandKVA = 0;
            decimal minDemandKVA = 0;
            decimal maxDemandKW = 0;
            decimal minDemandKW = 0;
            decimal maxDemandKVARLag = 0;
            decimal minDemandKVARLag = 0;
            decimal maxPowerFactor = 0;
            decimal minPowerFactor = 0;
            object dateToDisplay = "";
            int rowIndex = 0;
            int counter = 0;
            DataTable tableForGrid = new DataTable();
            tableForGrid.Columns.Add("Date Time");
            tableForGrid.Columns.Add("Max Voltage R Phase");
            tableForGrid.Columns.Add("Min Voltage R Phase");
            tableForGrid.Columns.Add("Max Voltage Y Phase");
            tableForGrid.Columns.Add("Min Voltage Y Phase");
            tableForGrid.Columns.Add("Max Current R Phase");
            tableForGrid.Columns.Add("Min Current R Phase");
            tableForGrid.Columns.Add("Max Current Y Phase");
            tableForGrid.Columns.Add("Min Current Y Phase");
            tableForGrid.Columns.Add("Max Demand kvar(lead)");
            tableForGrid.Columns.Add("Min Demand kvar(lead)");
            tableForGrid.Columns.Add("Max Demand kVA");
            tableForGrid.Columns.Add("Min Demand kVA");
            tableForGrid.Columns.Add("Max Demand kW");
            tableForGrid.Columns.Add("Min Demand kW");
            tableForGrid.Columns.Add("Max Demand kvar(lag)");
            tableForGrid.Columns.Add("Min Demand kvar(lag)");
            tableForGrid.Columns.Add("Max Power Factor");
            tableForGrid.Columns.Add("Min Power Factor");
            foreach (DataRow row in dSetLoadSurvey.Tables[0].Rows)
            {

                string formate = ConfigInfo.DateFormat().ToUpper();
                string dateString = Convert.ToString(row["Date Time"]);
                if (formate.Equals("MM/dd/yyyy") || (formate.Equals("MM-dd-yyyy")))
                {
                    year = Convert.ToInt32(dateString.Substring(6, 4));
                    day = Convert.ToInt32(dateString.Substring(3, 2));
                    month = Convert.ToInt32(dateString.Substring(0, 2));
                }
                else
                {
                    year = Convert.ToInt32(dateString.Substring(6, 4));
                    day = Convert.ToInt32(dateString.Substring(0, 2));
                    month = Convert.ToInt32(dateString.Substring(3, 2));
                }
                //year = Convert.ToDateTime(DateUtility.StringtoFormattedString(row["Date Time"].ToString())).Year;
                //day = Convert.ToDateTime(DateUtility.StringtoFormattedString(row["Date Time"].ToString())).DayOfYear;


                // if date is not same then we know max and min value just add it to the grid
                if (rowIndex > 0)
                {
                    if (!(yearForCheck == year && dayForCheck == day && monthforCheck == month))
                    {
                        counter = 0;
                        DataRow newRow = tableForGrid.NewRow();
                        newRow["Date Time"] = dateToDisplay;
                        newRow["Max Voltage R Phase"] = maxrPhaseVoltage;
                        newRow["Min Voltage R Phase"] = minrPhaseVoltage;
                        newRow["Max Voltage Y Phase"] = maxyPhaseVoltage;
                        newRow["Min Voltage Y Phase"] = minyPhaseVoltage;
                        newRow["Max Current R Phase"] = maxrPhaseCurrent;
                        newRow["Min Current R Phase"] = minrPhaseCurrent;
                        newRow["Max Current Y Phase"] = maxyPhaseCurrent;
                        newRow["Min Current Y Phase"] = minyPhaseCurrent;
                        newRow["Max Demand kvar(lead)"] = maxdemandkvarlead;
                        newRow["Min Demand kvar(lead)"] = mindemandkvarlead;
                        newRow["Max Demand kVA"] = maxDemandKVA;
                        newRow["Min Demand kVA"] = minDemandKVA;
                        newRow["Max Demand kW"] = maxDemandKW;
                        newRow["Min Demand kW"] = minDemandKW;
                        newRow["Max Demand kvar(lag)"] = maxDemandKVARLag;
                        newRow["Min Demand kvar(lag)"] = minDemandKVARLag;
                        newRow["Max Power Factor"] = maxPowerFactor;
                        newRow["Min Power Factor"] = minPowerFactor;
                        tableForGrid.Rows.Add(newRow);
                        maxrPhaseVoltage = 0;
                        minrPhaseVoltage = 0;
                        maxyPhaseVoltage = 0;
                        minyPhaseVoltage = 0;
                        maxrPhaseCurrent = 0;
                        minrPhaseCurrent = 0;
                        maxyPhaseCurrent = 0;
                        minyPhaseCurrent = 0;
                        maxdemandkvarlead = 0;
                        mindemandkvarlead = 0;
                        maxDemandKVA = 0;
                        minDemandKVA = 0;
                        maxDemandKVARLag = 0;
                        minDemandKVARLag = 0;
                        maxPowerFactor = 0;
                        minPowerFactor = 0;

                        isAdded = true;
                    }
                }
                // if date is same or it is coming first time in a date
                if ((yearForCheck == year && dayForCheck == day && monthforCheck == month) || counter == 0)
                {
                    isAdded = false;
                    if (counter == 0)
                    {
                        maxrPhaseVoltage = Convert.ToDecimal(row[1]);
                        minrPhaseVoltage = Convert.ToDecimal(row[1]);
                        maxyPhaseVoltage = Convert.ToDecimal(row[2]);
                        minyPhaseVoltage = Convert.ToDecimal(row[2]);
                        maxrPhaseCurrent = Convert.ToDecimal(row[3]);
                        minrPhaseCurrent = Convert.ToDecimal(row[3]);
                        maxyPhaseCurrent = Convert.ToDecimal(row[4]);
                        minyPhaseCurrent = Convert.ToDecimal(row[4]);
                        maxdemandkvarlead = Convert.ToDecimal(row[5]);
                        mindemandkvarlead = Convert.ToDecimal(row[5]);
                        maxDemandKVA = Convert.ToDecimal(row[6]);
                        minDemandKVA = Convert.ToDecimal(row[6]);
                        maxDemandKW = Convert.ToDecimal(row[7]);
                        minDemandKW = Convert.ToDecimal(row[7]);
                        maxDemandKVARLag = Convert.ToDecimal(row[8]);
                        minDemandKVARLag = Convert.ToDecimal(row[8]);
                        maxPowerFactor = Convert.ToDecimal(row[9]);
                        minPowerFactor = Convert.ToDecimal(row[9]);
                    }
                    else
                    {
                        maxrPhaseVoltage = Convert.ToDecimal(row[1]) > maxrPhaseVoltage ? Convert.ToDecimal(row[1]) : maxrPhaseVoltage;
                        minrPhaseVoltage = Convert.ToDecimal(row[1]) < minrPhaseVoltage ? Convert.ToDecimal(row[1]) : minrPhaseVoltage;
                        maxyPhaseVoltage = Convert.ToDecimal(row[2]) < maxyPhaseVoltage ? Convert.ToDecimal(row[2]) : maxyPhaseVoltage;
                        minyPhaseVoltage = Convert.ToDecimal(row[2]) < minyPhaseVoltage ? Convert.ToDecimal(row[2]) : minyPhaseVoltage;
                        maxrPhaseCurrent = Convert.ToDecimal(row[3]) > maxrPhaseCurrent ? Convert.ToDecimal(row[3]) : maxrPhaseCurrent;
                        minrPhaseCurrent = Convert.ToDecimal(row[3]) < minrPhaseCurrent ? Convert.ToDecimal(row[3]) : minrPhaseCurrent;
                        maxyPhaseCurrent = Convert.ToDecimal(row[4]) < maxyPhaseCurrent ? Convert.ToDecimal(row[4]) : maxyPhaseCurrent;
                        minyPhaseCurrent = Convert.ToDecimal(row[4]) < minyPhaseCurrent ? Convert.ToDecimal(row[4]) : minyPhaseCurrent;
                        maxdemandkvarlead = Convert.ToDecimal(row[5]) > maxdemandkvarlead ? Convert.ToDecimal(row[5]) : maxdemandkvarlead;
                        mindemandkvarlead = Convert.ToDecimal(row[5]) < mindemandkvarlead ? Convert.ToDecimal(row[5]) : mindemandkvarlead;
                        maxDemandKVA = Convert.ToDecimal(row[6]) > maxDemandKVA ? Convert.ToDecimal(row[6]) : maxDemandKVA;
                        minDemandKVA = Convert.ToDecimal(row[6]) < minDemandKVA ? Convert.ToDecimal(row[6]) : minDemandKVA;
                        maxDemandKW = Convert.ToDecimal(row[7]) > maxDemandKW ? Convert.ToDecimal(row[7]) : maxDemandKW;
                        minDemandKW = Convert.ToDecimal(row[7]) < minDemandKW ? Convert.ToDecimal(row[7]) : minDemandKW;
                        maxDemandKVARLag = Convert.ToDecimal(row[8]) > maxDemandKVARLag ? Convert.ToDecimal(row[8]) : maxDemandKVARLag;
                        minDemandKVARLag = Convert.ToDecimal(row[9]) < minDemandKVARLag ? Convert.ToDecimal(row[8]) : minDemandKVARLag;
                        maxPowerFactor = Convert.ToDecimal(row[9]) > maxPowerFactor ? Convert.ToDecimal(row[9]) : maxPowerFactor;
                        minPowerFactor = Convert.ToDecimal(row[9]) < minPowerFactor ? Convert.ToDecimal(row[9]) : minPowerFactor;

                    }
                }

                dateToDisplay = row["Date Time"];
                yearForCheck = year;
                dayForCheck = day;
                monthforCheck = month;
                counter++;
                rowIndex++;
            }
            // for last row ----------  

            if (!isAdded)
            {

                DataRow newRow = tableForGrid.NewRow();
                newRow["Date Time"] = dateToDisplay;
                newRow["Max Voltage R Phase"] = maxrPhaseVoltage;
                newRow["Min Voltage R Phase"] = minrPhaseVoltage;
                newRow["Max Voltage Y Phase"] = maxyPhaseVoltage;
                newRow["Min Voltage Y Phase"] = minyPhaseVoltage;
                newRow["Max Current R Phase"] = maxrPhaseCurrent;
                newRow["Min Current R Phase"] = minrPhaseCurrent;
                newRow["Max Current Y Phase"] = maxyPhaseCurrent;
                newRow["Min Current Y Phase"] = minyPhaseCurrent;
                newRow["Max Demand kvar(lead)"] = maxdemandkvarlead;
                newRow["Min Demand kvar(lead)"] = mindemandkvarlead;
                newRow["Max Demand kVA"] = maxDemandKVA;
                newRow["Min Demand kVA"] = minDemandKVA;
                newRow["Max Demand kW"] = maxDemandKW;
                newRow["Min Demand kW"] = minDemandKW;
                newRow["Max Demand kvar(lag)"] = maxDemandKVARLag;
                newRow["Min Demand kvar(lag)"] = minDemandKVARLag;
                newRow["Max Power Factor"] = maxPowerFactor;
                newRow["Min Power Factor"] = minPowerFactor;
                tableForGrid.Rows.Add(newRow);

            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(tableForGrid);
            return dataSet;
        }
        private void ShowReport()
        {
            DaySpecificLoadSurveyReport DaySpecificLoadSurvey = new DaySpecificLoadSurveyReport();
            DatabaseReportForm ObjRptForm = new DatabaseReportForm();

            for (int ColHeadCount = 1; ColHeadCount <= lsHeadings.Count - 1; ColHeadCount++)
            {
                string TextParameterCount = "TextParameter" + ColHeadCount;
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)DaySpecificLoadSurvey.ReportDefinition.ReportObjects[TextParameterCount];
                TextParam.Text = lsHeadings[ColHeadCount].ToString();
                TextParam.ObjectFormat.EnableSuppress = false;
            }


            DaySpecificLoadSurvey.SetDataSource(reportXSD);
            ObjRptForm.drptViewer.ReportSource = DaySpecificLoadSurvey;
            Cursor.Current = Cursors.Default;
            ObjRptForm.drptViewer.Zoom(1);
            this.Hide();
            ObjRptForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    this.StatusMessage = "LoadSurvey data not available.";
            //    return;
            //}
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    reportRow = reportXSD.Tables["MDataReportsTable"].NewRow();
            //    foreach (DataColumn col in ds.Tables[0].Columns)
            //    {
            //        if (col.Ordinal == 0)
            //            reportRow["MeterNo"] = row[col];
            //        else if (col.Ordinal == 1)
            //            reportRow["FileName"] = row[col];
            //        else
            //        {
            //            if (CommonBLL.IsTimeColumn(col.ColumnName))
            //                if (row[col].ToString().Equals("0"))
            //                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = dateUnavailable;
            //                else
            //                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = DateUtility.LongToDateTime(Convert.ToInt64(row[col].ToString())).ToString(ConfigInfo.DateFormat() + " HH:mm");
            //            else
            //                if ((col.ColumnName != "ErrorCode") && (!(col.ColumnName.Contains("PowerFactor"))))
            //                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = CommonBLL.GetFormattedData(CommonBLL.RemoveUnit(row[col].ToString()));//CommonBLL.GetFormattedData(row[col].ToString());
            //                else
            //                    reportRow[string.Concat("Parameter", col.Ordinal - 1)] = CommonBLL.RemoveUnitForReport(row[col].ToString());
            //        }
            //    }
            //    reportXSD.Tables["MDataReportsTable"].Rows.Add(reportRow);
            //}
        }
        private DataSet ListLoadSurveyData(long activeMeterDataId, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = DateUtility.DateTimeToLong(dtpFromDate.Value);
            long eDate = DateUtility.DateTimeToLong(dtpToDate.Value);
            dataSet = SortDataSet(loadSurveyBLL.GetMaxMinDayLoadsurvey(activeMeterDataId, sDate, eDate, types));
            //dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId, sDate, eDate, types);
            return dataSet;
        }
        private void btnShowReport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string dataSelectionType = string.Empty;
            if (dtpFromDate.Value > dtpToDate.Value)
            {
                this.StatusMessage = "From Date shouldn't be greater than To Date";
                return;
            }
            if (rdbtnDemand.Checked)
                dataSelectionType = "Demand";
            else
                dataSelectionType = "Energy";
            DataSet loadSurveyDS = new DataSet();

            loadSurveyDS = ListLoadSurveyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), dataSelectionType);
            if (loadSurveyDS == null)
            {
                this.StatusMessage = "No Data Available";
                return;
            }
           
            //loadSurveyDS = GetDataForGrid(SortDataSet(loadSurveyDS)); 
            if (loadSurveyDS != null && loadSurveyDS.Tables[0].Rows.Count > 0)
            {
                FillLoadSurveyXSD(loadSurveyDS,val);
            }

            ShowReport();
            this.Cursor = Cursors.Default;
        }
        private void FillLoadSurveyXSD(DataSet loadSurveyData,string val)
        {
            if (val == "Demand")
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


                foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                    lsHeadings.Add(col.ColumnName);

                //loadSurveyData = SortDataSet(loadSurveyData);
                foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DaySpecificLoadSurveyTable"].NewRow();
                    reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0])));
                    for (int colCount = 1; colCount <= loadSurveyData.Tables[0].Columns.Count - 1; colCount++)
                    {
                        string ParameterColValue = "Parameter" + Convert.ToString(colCount);
                        reportRow[ParameterColValue] = row[colCount].ToString();
                    }
                    string dateTimes = Convert.ToString(row[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeColumn"] = dateTimes;
                    reportXSD.Tables["DaySpecificLoadSurveyTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                reportRow = reportXSD.Tables["DaySpecificLoadSurveyTable"].NewRow();

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
                    //if (loadSurveyData.Tables[0].Columns[ParamCount].ColumnName.Contains("Demand"))
                    //{
                    //    lsHeadings.Add(GetEnergyColumnName(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName));
                    //}
                    //else
                    //{
                        lsHeadings.Add(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName);
                    //}
                }
                //loadSurveyData = SortDataSet(loadSurveyData);
                foreach (DataRow Drow in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DaySpecificLoadSurveyTable"].NewRow();
                    //For the date Time to split at 00:15 hours
                    //if (dateTimeCount == 1)
                    //{
                    //    PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                    //    reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                    //    dateTimeCount++;
                    //}
                    //else
                    //{
                        //string dates = "";
                        //if (!string.IsNullOrEmpty(Convert.ToString(Drow[0])))
                        //{
                        //    DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                        //    TimeSpan ts = currentDate.Date - PreviousDate.Date;
                        //    if (ts.Days > 0)
                        //    {
                        //        currentDate = currentDate.AddDays(-1);
                        //        long datesval = DateUtility.DateTimeToLong(currentDate);
                        //        dates = DateUtility.LongToStringDateFormat(datesval);
                        //        reportRow["GroupDateTime"] = dates;
                        //        PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                        //    }
                        //    else
                        //    {
                        //        dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                        //        reportRow["GroupDateTime"] = dates;
                        //    }
                        //}
                    //}
                    //    
                    reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                    string dateTimes = Convert.ToString(Drow[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeColumn"] = dateTimes;
                    for (int ParamCount = 1; ParamCount <= loadSurveyData.Tables[0].Columns.Count - 1; ParamCount++)
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
                    reportXSD.Tables["DaySpecificLoadSurveyTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            this.Close();
        }
    }
}
