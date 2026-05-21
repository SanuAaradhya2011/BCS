using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using CAB.Entity;
using CAB.BLL;
using CABApplication.Reports.RPTFiles;
using CAB.IECFramework.Utility;
using CAB.IECFramework.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;
using CABEntity;
using CrystalDecisions.CrystalReports.Engine;
using System.Globalization;

namespace CAB.UI
{
    public partial class SelectDialogTNEB : CABForm
    {
        public ReportDataSet reportXSD = new ReportDataSet();
        private string MeterId = string.Empty;
        private string CABFileName = string.Empty;
        const string dateUnavailable = "---------";
        static List<string> lsHeadings;
        System.Globalization.CultureInfo britishDateTimeFormat = new System.Globalization.CultureInfo("en-GB");
        System.Globalization.CultureInfo usDateTimeFormat = new System.Globalization.CultureInfo("en-US");
        DateTime fromDateCS, toDateCS;
        ArrayList filteredColumsn;
        private System.Resources.ResourceManager resourceMgr;
        CultureInfo cultureInfo;
        public TamperType TamperType1
        {
            get;
            set;
        }
        public enum TamperType

        {
            [DescriptionAttribute("Voltage Imbalance R Phase Tamper")]
            VoltageImbalanceRPhaseTamper,
            [DescriptionAttribute("Voltage Imbalance Y Phase Tamper")]
            VoltageImbalanceYPhaseTamper,
            [DescriptionAttribute("Voltage Imbalance B Phase Tamper")]
            VoltageImbalanceBPhaseTamper,
            [DescriptionAttribute("Current Imbalance R Phase Tamper")]
            CurrentImbalanceRPhaseTamper,
            [DescriptionAttribute("Current Imbalance Y Phase Tamper")]
            CurrentImbalanceYPhaseTamper,
            [DescriptionAttribute("Current Imbalance B Phase Tamper")]
            CurrentImbalanceBPhaseTamper,
            [DescriptionAttribute("Missing Potential R Phase Tamper")]
            MissingPotentialRPhaseTamper,
            [DescriptionAttribute("Missing Potential Y Phase Tamper")]
            MissingPotentialYPhaseTamper,
            [DescriptionAttribute("Missing Potential B Phase Tamper")]
            MissingPotentialBPhaseTamper,
            [DescriptionAttribute("CT Short Tamper")]
            CTShortTamper,
            [DescriptionAttribute("CT Open R Phase Tamper")]
            CTOpenRPhaseTamper,
            [DescriptionAttribute("CT Open Y Phase Tamper")]
            CTOpenYPhaseTamper,
            [DescriptionAttribute("CT Open B Phase Tamper")]
            CTOpenBPhaseTamper,
            [DescriptionAttribute("One Phase Neutral Absent Tamper")]
            OnePhaseNeutralAbsentTamper,
            [DescriptionAttribute("Voltage Phase Reversal Tamper")]
            VoltagePhaseReversalTamper,
            [DescriptionAttribute("Current Reversal R Phase Tamper")]
            CurrentReversalRPhaseTamper,
            [DescriptionAttribute("Current Reversal Y Phase Tamper")]
            CurrentReversalYPhaseTamper,
            [DescriptionAttribute("Current Reversal B Phase Tamper")]
            CurrentReversalBPhaseTamper,
            [DescriptionAttribute("Magnetic Influence Tamper")]
            MagneticInfluenceTamper,
            [DescriptionAttribute("Neutral Disturbance Tamper")]
            NeutralDisturbanceTamper,
            [DescriptionAttribute("Front Cover Opening Tamper")]
            FrontCoverOpeningTamper,
            [DescriptionAttribute("Total Tamper")]
            TotalTamper,
            [DescriptionAttribute("Power On/Off")]
            PowerOnOff

        }
        public SelectDialogTNEB()
        {
            InitializeComponent();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = loadSurveyBLL.GetFromDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            long eDate = loadSurveyBLL.GetToDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            DateTime tmpDateTime;
            
            
            string dateformatSetting = ConfigSettings.GetValue("DateFormat");
            //dd/MM/yyyy
            fromDate.CustomFormat = dateformatSetting;
            toDate.CustomFormat = dateformatSetting;

            
            cultureInfo = new System.Globalization.CultureInfo("en-GB");
            britishDateTimeFormat.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            usDateTimeFormat.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            if (!(dateformatSetting == "dd/MM/yyyy" || dateformatSetting == "dd-MM-yyyy"))
            {
                cultureInfo = new System.Globalization.CultureInfo("en-US");
                cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            }
            else
                cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            resourceMgr = new System.Resources.ResourceManager("CAB.UI.SelectDialogTNEB", System.Reflection.Assembly.GetExecutingAssembly());

            if (sDate != 0)
            {
                tmpDateTime = DateUtility.LongToDateTime(sDate);
                if (tmpDateTime.Minute == 0 && tmpDateTime.Hour == 0)
                    fromDate.Value = tmpDateTime.Subtract(new TimeSpan(24, 0, 0));
                else
                    fromDate.Value=tmpDateTime;

                 //if (cultureInfo.Name == "en-GB")
                 //       {
                 //           if (CultureInfo.CurrentCulture.Name != "en-GB")
                 //           {
                 //               fromDateCS = Convert.ToDateTime(fromDate.Value.ToString(), britishDateTimeFormat);
                 //               toDateCS = Convert.ToDateTime(fromDate.Value.ToString(), britishDateTimeFormat);
                 //           }
                 //           else
                 //           {
                 //               fromDateCS = Convert.ToDateTime(fromDate.Value.ToString());
                 //               toDateCS = Convert.ToDateTime(fromDate.Value.ToString());
                 //           }
                 //       }
                 //       else if (cultureInfo.Name == "en-US")
                 //       {
                 //           if (CultureInfo.CurrentCulture.Name != "en-US")
                 //           {
                 //               fromDateCS = Convert.ToDateTime(fromDate.Value.ToString(), usDateTimeFormat);
                 //               toDateCS = Convert.ToDateTime(fromDate.Value.ToString());
                 //           }
                 //           else
                 //           {
                 //               fromDateCS = Convert.ToDateTime(fromDate.Value.ToString());
                 //               toDateCS = Convert.ToDateTime(fromDate.Value.ToString());
                 //           }
                 //       }

            }
            if (eDate != 0)
            {
                tmpDateTime = DateUtility.LongToDateTime(eDate);
                if (tmpDateTime.Minute == 0 && tmpDateTime.Hour == 0)
                    toDate.Value = tmpDateTime.Subtract(new TimeSpan(24, 0, 0));
                else
                    toDate.Value = tmpDateTime;
            }
           
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.SelectDialogTNEB", System.Reflection.Assembly.GetExecutingAssembly());
        }

        private void groupBoxOthers_Enter(object sender, EventArgs e)
        {

        }
        private void SelectDialogTNEB_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
        }

        // Added to list the Load Survey Data.
        private DataSet ListLoadSurveyData(long activeMeterDataId, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = loadSurveyBLL.GetFromDate(activeMeterDataId);
            long eDate = loadSurveyBLL.GetToDate(activeMeterDataId);
            dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId, sDate, eDate, types);
            return dataSet;
        }
        // Added to list the Daily Maximum Demand Data.
        private DataSet ListDailyMDData(long activeMeterDataId, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = loadSurveyBLL.GetFromDate(activeMeterDataId);
            long eDate = loadSurveyBLL.GetToDate(activeMeterDataId);
            dataSet = loadSurveyBLL.GetMaxMinDayLoadsurvey(activeMeterDataId, sDate, eDate,"Demand");
            return dataSet;
        }
        // Added to list the Instantaneous Data.
        private DataSet ListInstantData(long activeMeterDataId)
        {
            return new InstantPowerBLL().GetMeterData(Convert.ToInt32(activeMeterDataId));
        }
        // Added to list the Daily Energy Consumption Data.
        private DataSet ListDailyEnergyConsumption(long activeMeterDataId)//, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            dataSet = new DTMDailyProfileBLL().ListData(Convert.ToInt64(activeMeterDataId));
            return CommonBLL.RemoveGarbageColumns(dataSet);
        }
        // Added to list the Tamper Data.
        private DataSet ListTamperData(long activeMeterDataId)//, string types)
        {
            DataSet dsTampers = CommonBLL.TamperCounter(Convert.ToInt32(activeMeterDataId));
            TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
            DataSet dsTamperDuration=new DataSet();
            
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("TamperName", typeof(System.String)));
            table.Columns.Add(new DataColumn("TamperDuration", typeof(System.String)));
            table.Columns.Add(new DataColumn("TmprCount", typeof(System.String)));

            DataRow tamperRow;
            DateTime fromDateTime, toDateTime;
            TimeSpan ts_tmp=new TimeSpan(),tsTmprDuration = new TimeSpan();
            string days, hour, minute;
            foreach (DataRow tmprDescRow in dsTampers.Tables[0].Rows)
            {
                if (String.IsNullOrEmpty(tmprDescRow[2].ToString()) || Convert.ToInt32(tmprDescRow[2]) == 0)
                    continue;
                tamperRow = table.NewRow();
                tamperRow[0] = tmprDescRow[1];//Tamper Event.
                tamperRow[2] = tmprDescRow[2];//Tamper Count.
                dsTamperDuration = tamperSnapShotBLL.ListData(activeMeterDataId, Convert.ToInt64(tmprDescRow[0]));
                tsTmprDuration = new TimeSpan();
                if (dsTamperDuration.Tables[0].Rows.Count == 0)
                    continue;
                foreach (DataRow dr in dsTamperDuration.Tables[0].Rows)
                {
                    fromDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[1]));
                    toDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[2]));
                    ts_tmp = toDateTime - fromDateTime;
                    if(ts_tmp.TotalSeconds >0 )
                    tsTmprDuration=tsTmprDuration.Add(ts_tmp);
                }

                days = tsTmprDuration.Days.ToString();
                hour = tsTmprDuration.Hours.ToString();
                minute = tsTmprDuration.Minutes.ToString();
                if (Convert.ToInt32(hour) <= 0)
                    hour = "00";
                if (Convert.ToInt32(minute) <= 0)
                    minute = "00";
                if (Convert.ToInt32(days) <= 0)
                    days = "00";
                if (hour.Length == 1)
                    hour = "0" + hour;
                if (minute.Length == 1)
                    minute = "0" + minute;
                if (days.Length == 1)
                    days = "0" + days;
                tamperRow[1] = string.Concat(days, ":", hour, ":", minute);

                table.Rows.Add(tamperRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ds;
        }
        // Added to list the VI Profile Data.
        private DataSet ListVIProfileData(long activeMeterDataId)//, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = loadSurveyBLL.GetFromDate(activeMeterDataId);
            long eDate = loadSurveyBLL.GetToDate(activeMeterDataId);
            dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId, sDate, eDate, "Demand");
            return dataSet;
        }
        // Added to list the Demand Profile and Energy Profile Data.
        private DataSet ListProfileData(long activeMeterDataId,string type)//, string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate = loadSurveyBLL.GetFromDate(activeMeterDataId);
            long eDate = loadSurveyBLL.GetToDate(activeMeterDataId);
            dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId, sDate, eDate, type);
            return dataSet;
        }
        // Added to fill the report dataset table with Cumulative Taper Events Data.
        private void FillCumulativeTamperEventsDataXSD(DataSet dsCumulativeTamperData)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Event", typeof(System.String)));
            table.Columns.Add(new DataColumn("TamperDuration", typeof(System.String)));
            table.Columns.Add(new DataColumn("TamperCount", typeof(System.String)));
            
            ////Get Data from The DB dataset to a temporary dataset.
            DataRow row1;
            foreach (DataRow datarow in dsCumulativeTamperData.Tables[0].Rows)
            {
                row1 = table.NewRow();
                table.Rows.Add(row1);//"Date Time"
                foreach (DataColumn col in dsCumulativeTamperData.Tables[0].Columns)
                {
                    string val = Convert.ToString(datarow[col.ColumnName]); 
                    if (col.ColumnName.Equals("TamperName") && val != "")
                        row1[0] = val;
                    if (col.ColumnName.Equals("TamperDuration") && val != "")
                        row1[1] = val;
                    if (col.ColumnName.Equals("TmprCount") && val != "")
                        row1[2] = val;
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            DataRow reportrow;
            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    reportrow = reportXSD.Tables["CumulativeTamperEventTable"].NewRow();

                    if (!string.IsNullOrEmpty(row["Event"].ToString()))
                        reportrow["Event"] = row["Event"].ToString();
                    else
                        reportrow["Event"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["TamperDuration"].ToString()))
                        reportrow["TamperDuration"] = row["TamperDuration"].ToString();
                    else
                        reportrow["TamperDuration"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["TamperCount"].ToString()))
                        reportrow["TamperCount"] = row["TamperCount"].ToString();
                    else
                        reportrow["TamperCount"] = dateUnavailable;

                    reportXSD.Tables["CumulativeTamperEventTable"].Rows.Add(reportrow);
                }
            }
        }
        // Added to fill the report dataset table with Instantaneous Data.
        private void FillInstantDatXSD(DataSet instantData)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("VoltageR", typeof(System.String)));
            table.Columns.Add(new DataColumn("VoltageY", typeof(System.String)));
            table.Columns.Add(new DataColumn("VoltageB", typeof(System.String)));

            table.Columns.Add(new DataColumn("CurrentR", typeof(System.String)));
            table.Columns.Add(new DataColumn("CurrentY", typeof(System.String)));
            table.Columns.Add(new DataColumn("CurrentB", typeof(System.String)));

            table.Columns.Add(new DataColumn("kWR", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWY", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWB", typeof(System.String)));

            table.Columns.Add(new DataColumn("kVArR", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVArY", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVArB", typeof(System.String)));

            table.Columns.Add(new DataColumn("PF_R", typeof(System.String)));
            table.Columns.Add(new DataColumn("PF_Y", typeof(System.String)));
            table.Columns.Add(new DataColumn("PF_B", typeof(System.String)));

            table.Columns.Add(new DataColumn("Total_kW", typeof(System.String)));
            table.Columns.Add(new DataColumn("Total_kVAr", typeof(System.String)));//--reactive power lag/lead
            table.Columns.Add(new DataColumn("Total_PF", typeof(System.String)));

            table.Columns.Add(new DataColumn("Total_kVA", typeof(System.String)));
            table.Columns.Add(new DataColumn("Avg_PF", typeof(System.String)));            

            DataRow row1 = table.NewRow();
            table.Rows.Add(row1);
            foreach (DataRow datarow in instantData.Tables[0].Rows)
            {
                  switch (datarow[0].ToString().Trim())
                  {
                      case "Voltage R Phase" :
                          row1["VoltageR"] = datarow[1].ToString().Trim();
                          break;
                      case "Voltage Y Phase":
                          row1["VoltageY"] = datarow[1].ToString().Trim();
                          break;
                      case "Voltage B Phase":
                          row1["VoltageB"] = datarow[1].ToString().Trim();
                          break;
                      case "Current R Phase" :
                          row1["CurrentR"] = datarow[1].ToString().Trim();
                          break;
                      case "Current Y Phase":
                          row1["CurrentY"] = datarow[1].ToString().Trim();
                          break;
                      case "Current B Phase":
                          row1["CurrentB"] = datarow[1].ToString().Trim();
                          break;//

                      case "Instant Active power R Phase":
                          row1["kWR"] = datarow[1].ToString().Trim();
                          break;
                      case "Instant Active power Y Phase":
                          row1["kWY"] = datarow[1].ToString().Trim();
                          break;
                      case "Instant Active power B Phase":
                          row1["kWB"] = datarow[1].ToString().Trim();
                          break;

                      case "Instant Reactive power R Phase":
                          row1["kVArR"] = datarow[1].ToString().Trim();
                          break;
                      case "Instant Reactive power Y Phase":
                          row1["kVArY"] = datarow[1].ToString().Trim();
                          break;
                      case "Instant Reactive power B Phase":
                          row1["kVArB"] = datarow[1].ToString().Trim();
                          break;


                      case "Power Factor R Phase":
                          row1["PF_R"] = datarow[1].ToString().Trim();
                          break;
                      case "Power Factor Y Phase":
                          row1["PF_Y"] = datarow[1].ToString().Trim();
                          break;
                      case "Power Factor B Phase":
                          row1["PF_B"] = datarow[1].ToString().Trim();
                          break;

                      case "Active power":
                          row1["Total_kW"] = datarow[1].ToString().Trim();
                          break;
                      //case "Reactive power (Lag)":
                      //    row1["Total_kVAr"] = datarow[1].ToString().Trim();
                      //    break;
                      case "Total Power Factor":
                          row1["Total_PF"] = datarow[1].ToString().Trim();
                          break;

                      case "Apparent power":
                          row1["Total_kVA"] = datarow[1].ToString().Trim();
                          break;
                    
                      
                          
                  }
                  
              
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            DataRow reportrow;
            if (ds.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{//kVADemand,kWDemand
                double tmpdbl; 
                reportrow = reportXSD.Tables["InstantaneousDataTable"].NewRow();

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["VoltageR"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["VoltageR"].ToString(),out tmpdbl))
                    reportrow["VoltageR"] = Math.Round(tmpdbl,2);
                else
                    reportrow["VoltageR"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["VoltageY"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["VoltageY"].ToString(), out tmpdbl))
                    reportrow["VoltageY"] = Math.Round(tmpdbl, 2);
                else
                    reportrow["VoltageY"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["VoltageB"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["VoltageB"].ToString(), out tmpdbl))
                    reportrow["VoltageB"] = Math.Round(tmpdbl, 2);
                else
                    reportrow["VoltageB"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CurrentR"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["CurrentR"].ToString(), out tmpdbl))
                    reportrow["CurrentR"] = Math.Round(tmpdbl, 3);
                else
                    reportrow["CurrentR"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CurrentY"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["CurrentY"].ToString(), out tmpdbl))
                    reportrow["CurrentY"] = Math.Round(tmpdbl, 3);
                else
                    reportrow["CurrentY"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CurrentB"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["CurrentB"].ToString(), out tmpdbl))
                    reportrow["CurrentB"] = Math.Round(tmpdbl, 3);
                else
                    reportrow["CurrentB"] = dateUnavailable;


                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kWR"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kWR"].ToString(), out tmpdbl))
                    reportrow["kWR"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kWR"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kWY"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kWY"].ToString(), out tmpdbl))
                    reportrow["kWY"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kWY"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kWB"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kWB"].ToString(), out tmpdbl))
                    reportrow["kWB"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kWB"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArR"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kVArR"].ToString(), out tmpdbl))
                    reportrow["kVArR"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kVArR"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArY"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kVArY"].ToString(), out tmpdbl))
                    reportrow["kVArY"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kVArY"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArB"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["kVArB"].ToString(), out tmpdbl))
                    reportrow["kVArB"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["kVArB"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PF_R"].ToString()))// && double.TryParse(ds.Tables[0].Rows[0]["PF_R"].ToString(), out tmpdbl))
                    reportrow["PF_R"] = ds.Tables[0].Rows[0]["PF_R"].ToString();//Math.Round(tmpdbl, 4);
                else
                    reportrow["PF_R"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PF_Y"].ToString()))// && double.TryParse(ds.Tables[0].Rows[0]["PF_Y"].ToString(), out tmpdbl))
                    reportrow["PF_Y"] = ds.Tables[0].Rows[0]["PF_Y"].ToString();//Math.Round(tmpdbl, 4);
                else
                    reportrow["PF_Y"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PF_B"].ToString()))// && double.TryParse(ds.Tables[0].Rows[0]["PF_B"].ToString(), out tmpdbl))
                    reportrow["PF_B"] = ds.Tables[0].Rows[0]["PF_B"].ToString();//Math.Round(tmpdbl, 4);
                else
                    reportrow["PF_B"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Total_kW"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["Total_kW"].ToString(), out tmpdbl))
                    reportrow["Total_kW"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["Total_kW"] = dateUnavailable;
                //ds.Tables[0].Rows[0]["kVArR"]

                if (
                    ((!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArR"].ToString()))
                    ||
                    (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArY"].ToString()))
                    ||
                    (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVArB"].ToString())))
                    )
                {

                    double? a, b, c;
                    try { a = Convert.ToDouble(ds.Tables[0].Rows[0]["kVArR"]); }catch(Exception r){a=null;}
                    try { b = Convert.ToDouble(ds.Tables[0].Rows[0]["kVArY"]); }catch (Exception r) { b = null; }
                    try { c = Convert.ToDouble(ds.Tables[0].Rows[0]["kVArB"]); }catch (Exception r) { c = null; }
                    if(a!=null && b!=null && c!=null)
                        reportrow["Total_kVAr"] = Math.Round(a.Value + b.Value + c.Value,1);
                    else
                        reportrow["Total_kVAr"] = dateUnavailable;
                }
                else
                {
                    reportrow["Total_kVAr"] = dateUnavailable;
                }

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Total_PF"].ToString()))// && double.TryParse(ds.Tables[0].Rows[0]["Total_PF"].ToString(), out tmpdbl))
                    reportrow["Total_PF"] = ds.Tables[0].Rows[0]["Total_PF"].ToString();
                else
                    reportrow["Total_PF"] = dateUnavailable;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Total_kVA"].ToString()) && double.TryParse(ds.Tables[0].Rows[0]["Total_kVA"].ToString(), out tmpdbl))
                    reportrow["Total_kVA"] = Math.Round(tmpdbl, 1);
                else
                    reportrow["Total_kVA"] = dateUnavailable;

                DataSet dsAvgPF = new GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                
                if (dsAvgPF != null && dsAvgPF.Tables.Count > 0 && dsAvgPF.Tables[0].Rows.Count > 0)
                {
                    double? Cum_kWH = null, Cum_kVAH = null, mdResetcounter=null;
                   
                    foreach (DataRow drow in dsAvgPF.Tables[0].Rows)
                    {
                        if (drow["Descriptions"].ToString() == "Cumulative Active Energy")
                        {
                            if (drow["Value"] != null
                                && drow["Value"].ToString().Trim() != null
                                   && Double.TryParse(drow["Value"].ToString().Trim(), out tmpdbl))
                            {

                                Cum_kWH = tmpdbl;
                            }
                            else
                                Cum_kWH = null;
                        }
                        if (drow["Descriptions"].ToString() == "Cumulative Apparent Energy")
                        {
                            if (drow["Value"] != null
                                && drow["Value"].ToString().Trim() != null
                                   && Double.TryParse(drow["Value"].ToString().Trim(), out tmpdbl))
                            {

                                Cum_kVAH = tmpdbl;
                            }
                            else
                                Cum_kVAH = null;
                        }
                        if (drow["Descriptions"].ToString() == "MD Reset Counter")
                        {
                            if (drow["Value"] != null
                                && drow["Value"].ToString().Trim() != null
                                   && Double.TryParse(drow["Value"].ToString().Trim(), out tmpdbl))
                            {

                                mdResetcounter = tmpdbl;
                            }
                            else
                                mdResetcounter = null;
                        }
                        
                      }

                    if (mdResetcounter != null && mdResetcounter ==0.0)
                    {
                        if (Cum_kVAH.GetValueOrDefault() != 0.0 && Cum_kWH != null)
                        {
                            reportrow["Avg_PF"] = Math.Round(Convert.ToDecimal(Cum_kWH / Cum_kVAH), 4);
                        }
                        else
                            reportrow["Avg_PF"] = dateUnavailable;
                    }
                    else if (mdResetcounter != null && mdResetcounter > 0.0)
                    {
                        DataSet dsConsumption = new BillingBLL().GetCumulativeEnergyCalculated(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (dsConsumption != null && dsConsumption.Tables.Count > 0 && dsConsumption.Tables[0].Rows.Count > 0)
                        {
                            DataRow ddrow = dsConsumption.Tables[0].Rows[0];
                            double? Cur_kWH = null, Cur_kVAH = null;
                            if (ddrow[1] != null
                               && ddrow[1].ToString().Trim() != null
                                  && Double.TryParse(ddrow[1].ToString().Trim(), out tmpdbl))
                            {

                                Cur_kWH = tmpdbl;
                            }
                            else
                                Cur_kWH = null;

                            if (ddrow[2] != null
                               && ddrow[2].ToString().Trim() != null
                                  && Double.TryParse(ddrow[2].ToString().Trim(), out tmpdbl))
                            {

                                Cur_kVAH = tmpdbl;
                            }
                            else
                                Cur_kVAH = null;

                            if (Cur_kVAH.GetValueOrDefault() != 0.0 && Cur_kWH != null)
                            {
                                reportrow["Avg_PF"] = Math.Round(Convert.ToDecimal(Cur_kWH / Cur_kVAH), 4);
                            }
                            else
                                reportrow["Avg_PF"] = dateUnavailable;

                        }
                        else
                            reportrow["Avg_PF"] = dateUnavailable;
                    }
                    else
                        reportrow["Avg_PF"] = dateUnavailable;
                }
                else
                    reportrow["Avg_PF"] = dateUnavailable;

                reportrow["ReadingDateTime"] = reportXSD.Tables["FileUploadDetails"].Rows[0]["ReadingDateandTime"];
                
                reportXSD.Tables["InstantaneousDataTable"].Rows.Add(reportrow);

               
            }
        }
        // Added to fill the report dataset table with Maximum Demand Data.
        private void FillMDDataXSD(DataSet dailyMDData)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("MDDate", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVADemand", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWDemand", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDDate2", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVADemand2", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWDemand2", typeof(System.String)));

            DataRow row1=table.NewRow();
            int i = 0;
            bool isSecondTable = false;
            foreach (DataRow datarow in dailyMDData.Tables[0].Rows)
            {
                if (!isSecondTable)
                {
                    row1 = table.NewRow();
                    table.Rows.Add(row1);
                }
                foreach (DataColumn col in dailyMDData.Tables[0].Columns)
                {
                    string val = Convert.ToString(datarow[col.ColumnName]);
                    if (col.ColumnName.Equals("Date") && val != "")
                    {
                        if (!isSecondTable)
                            row1[0] = val;
                        else
                            row1[3] = val;
                    }
                    if (col.ColumnName.Equals("Max Demand kVA") && val != "")
                    {
                        if (!isSecondTable) 
                            row1[1] = val;
                        else
                            row1[4] = val;
                    }
                    if (col.ColumnName.Equals("Max Demand kW") && val != "")
                    {
                        if (!isSecondTable) 
                            row1[2] = val;
                        else
                            row1[5] = val;
                    }
                }
                isSecondTable = !isSecondTable;
                i++;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            DataRow reportrow;
            if (ds.Tables[0].Rows.Count > 0)
            {
                double tmpdl;
                foreach (DataRow row in ds.Tables[0].Rows)
                {//kVADemand,kWDemand
                    reportrow = reportXSD.Tables["DailyMaxDemandDataTable"].NewRow();
                    if (!string.IsNullOrEmpty(row["MDDate"].ToString()))
                        reportrow["MDDate"] = row["MDDate"].ToString();
                    else
                        reportrow["MDDate"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kVADemand"].ToString()) && double.TryParse(row["kVADemand"].ToString(), out tmpdl))
                        reportrow["kVADemand"] = Math.Round(tmpdl, 3);
                    else
                        reportrow["kVADemand"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kWDemand"].ToString()) && double.TryParse(row["kWDemand"].ToString(), out tmpdl))
                        reportrow["kWDemand"] = Math.Round(tmpdl, 3);
                    else
                        reportrow["kWDemand"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["MDDate2"].ToString()))
                        reportrow["MDDate2"] = row["MDDate2"].ToString();
                    else
                        reportrow["MDDate2"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kVADemand2"].ToString()) && double.TryParse(row["kVADemand2"].ToString(), out tmpdl))
                        reportrow["kVADemand2"] = Math.Round(tmpdl, 3);
                    else
                        reportrow["kVADemand2"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kWDemand2"].ToString()) && double.TryParse(row["kWDemand2"].ToString(), out tmpdl))
                        reportrow["kWDemand2"] = Math.Round(tmpdl, 3);
                    else
                        reportrow["kWDemand2"] = dateUnavailable;

                    reportXSD.Tables["DailyMaxDemandDataTable"].Rows.Add(reportrow);

                }
            }
        }
        // Added to fill the report dataset table with Daily Energy Consumption Data.
        private void FillDailyenergyConsumptionDataXSD(DataSet dsdailyEnergyConsumption)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ConsumptionDate", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAHr", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWHr", typeof(System.String)));
            table.Columns.Add(new DataColumn("ConsumptionDate2", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAHr2", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWHr2", typeof(System.String)));
            //Get Data from The DB dataset to a temporary dataset.
            DataRow row1 = table.NewRow();
            int i = 0;
            string last_kVAh = string.Empty, last_kWh = string.Empty;
            double tmp;
            DateTime tmpDate;
            bool isSecondTable = false;
            int rowCounter = 0;
            foreach (DataRow datarow in dsdailyEnergyConsumption.Tables[0].Rows)
            {
                           
               
                if (!isSecondTable)
                {
                    row1 = table.NewRow();
                    table.Rows.Add(row1);
                }
                foreach (DataColumn col in dsdailyEnergyConsumption.Tables[0].Columns)
                {
                    
                    string val = Convert.ToString(datarow[col.ColumnName]);
                    if (col.ColumnName.Equals("DailyProfileDate") && val != "")
                    {
                        tmpDate=DateUtility.LongToDateTime(Convert.ToInt64(val));
                        if (!isSecondTable)
                        {
                            if (cultureInfo.Name == "en-US")
                            {
                                row1[0] = tmpDate.Month.ToString() + "/" + tmpDate.Day.ToString() + "/" + tmpDate.Year.ToString();
                            }
                            else
                            {//en-GB
                                row1[0] = tmpDate.Day.ToString() + "/" + tmpDate.Month.ToString() + "/" + tmpDate.Year.ToString();
                            }
                        }
                        else
                            if (cultureInfo.Name == "en-US")
                            {
                                row1[3] = tmpDate.Month.ToString() + "/" + tmpDate.Day.ToString() + "/" + tmpDate.Year.ToString();
                            }
                            else
                            {//en-GB
                                row1[3] = tmpDate.Day.ToString() + "/" + tmpDate.Month.ToString() + "/" + tmpDate.Year.ToString();
                            }
                       
                        //if (!isSecondTable)
                        //    row1[0] = tmpDate.Day.ToString() + "/" + tmpDate.Month.ToString() + "/" + tmpDate.Year.ToString();
                        //else
                        //    row1[3] = tmpDate.Day.ToString() + "/" + tmpDate.Month.ToString() + "/" + tmpDate.Year.ToString();
                    }
                    if (col.ColumnName.Equals("Cumulative kVAh") && val != "")
                    {
                        if (i == 0)
                        {
                            if (!isSecondTable)
                                row1[1] = "------";//Math.Round(Convert.ToDouble(val), 3).ToString();
                            else
                                row1[4] = "------";// Math.Round(Convert.ToDouble(val), 3).ToString();
                        }
                        else
                        {
                            tmp = Convert.ToDouble(val) - Convert.ToDouble(last_kVAh);
                            if (!isSecondTable)
                                row1[1] = Math.Round(tmp, 3).ToString();
                            else
                                row1[4] = Math.Round(tmp, 3).ToString();
                        }
                        last_kVAh = val;
                    }
                    if (col.ColumnName.Equals("Cumulative kWh") && val != "")
                    {
                        if (i == 0)
                        {
                            if (!isSecondTable)
                                row1[2] = "------";//Math.Round(Convert.ToDouble(val), 3).ToString();
                            else
                                row1[5] = "------";// Math.Round(Convert.ToDouble(val), 3).ToString();
                        }
                        else
                        {
                            tmp = Convert.ToDouble(val) - Convert.ToDouble(last_kWh);
                            if (!isSecondTable)
                                row1[2] = Math.Round(tmp, 3).ToString();
                            else
                                row1[5] = Math.Round(tmp, 3).ToString();
                        }
                        last_kWh = val;
                    }
                }
                i++;
                isSecondTable = !isSecondTable;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            DataRow reportrow;
            //Set data from temp dataset to Report dataset.
            if (ds.Tables[0].Rows.Count > 0)
            {
                double tmpdl;
                foreach (DataRow row in ds.Tables[0].Rows)
                {//kVADemand,kWDemand
                    reportrow = reportXSD.Tables["DailyEnergyConsumptionTable"].NewRow();
                    if (!string.IsNullOrEmpty(row["ConsumptionDate"].ToString()))
                        reportrow["ConsumptionDate"] = row["ConsumptionDate"].ToString();
                    else
                        reportrow["ConsumptionDate"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kVAHr"].ToString()) && double.TryParse(row["kVAHr"].ToString(), out tmpdl))
                        reportrow["kVAHr"] = Math.Round(tmpdl, 1);
                    else
                        reportrow["kVAHr"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kWHr"].ToString()) && double.TryParse(row["kWHr"].ToString(), out tmpdl))
                        reportrow["kWHr"] = Math.Round(tmpdl, 1);
                    else
                        reportrow["kWHr"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["ConsumptionDate2"].ToString()))
                        reportrow["ConsumptionDate2"] = row["ConsumptionDate2"].ToString();
                    else
                        reportrow["ConsumptionDate2"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kVAHr2"].ToString()) && double.TryParse(row["kVAHr2"].ToString(), out tmpdl))
                        reportrow["kVAHr2"] = Math.Round(tmpdl, 1);
                    else
                        reportrow["kVAHr2"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["kWHr2"].ToString()) && double.TryParse(row["kWHr2"].ToString(), out tmpdl))
                        reportrow["kWHr2"] = Math.Round(tmpdl, 1);
                    else
                        reportrow["kWHr2"] = dateUnavailable;


                    reportXSD.Tables["DailyEnergyConsumptionTable"].Rows.Add(reportrow);

                }
            }
        }
        // This function is used to compare date picker values and Load survey date values.
        private bool ChkDateRange(string dateValue, TimeSpan tmpTimeSpan)
        {
            if (dateValue != null)
            {
               
                DateTime dt = new DateTime();
                    
                //if (cultureInfo.Name != CultureInfo.CurrentCulture.Name)
                //    dt = Convert.ToDateTime(dateValue, new CultureInfo(CultureInfo.CurrentCulture.Name)).Subtract(tmpTimeSpan);
                //else
                    dt=Convert.ToDateTime(dateValue, cultureInfo).Subtract(tmpTimeSpan);

                
                if (DateTime.Compare(dt.Date, fromDate.Value.Date) >= 0 && DateTime.Compare(dt.Date, toDate.Value.Date) <= 0)
                    return true;

            }
            return false;
        }
        // Added to validate From date should not be greater than to date.
        private bool ChkValidations()
        {
            if (fromDate.Value.Date > toDate.Value)
            {
                

                return false;
            }
            return true;
        }
        #region Check No Load
        /// <summary>
        /// This Function checks if the 
        /// current IP can be marked as NL.
        /// If all the Demand/Energy Columns have 0 value
        /// then the IP can be marked as NL.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private LSIPType ChkNoLoad(DataRow dr)
        {
            for (int i = 0; i < filteredColumsn.Count; i++)
            {// If all the Demand/Energy Columns have 0 value then the IP can be marked as NL.
                if (!(dr[filteredColumsn[i].ToString()] != null && Convert.ToDouble(dr[filteredColumsn[i].ToString()]) == 0.0))
                    return LSIPType.None;
            }
            return LSIPType.NL;
        }
        #endregion

        #region Check No Power
        /// <summary>
        /// This Function checks if the 
        /// current IP can be marked as NP.
        /// If all the LS Columns are not null & not equal to -1
        /// then the IP can be marked as NP.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private LSIPType ChkNoPower(DataRow dr)
        {
            LSIPType PowerOffInIP = LSIPType.NP;
            double tempColValue=double.MinValue;
            for (int j = 1; j < dr.Table.Columns.Count; j++)
            {// If all the LS Columns are not null & not equal to -1  then the IP can be marked as NP.
                if (dr[j] == null || Double.TryParse(dr[j].ToString(), out tempColValue))
                {
                    if (tempColValue == double.MinValue || tempColValue != -1)
                    {
                        PowerOffInIP = LSIPType.None;
                        break;
                    }
                }
            }
           
            return PowerOffInIP;
        }
        #endregion

        #region Check if IP is NL/NP/Normal
        /// <summary>
        /// Check if IP is NL/NP/Normal
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private LSIPType ChkIPStatus(DataRow dr)
        {
            if (ChkNoPower(dr) == LSIPType.NP)
                return LSIPType.NP;
            if (ChkNoLoad(dr) == LSIPType.NL)
                return LSIPType.NL;
            return LSIPType.None;
        }
        #endregion

        #region Create Temporary Data Table for VI Profile Report
        /// <summary>
        /// CreateVIProfileTempDataTable
        /// </summary>
        /// <returns></returns>
        private DataTable CreateVIProfileTempDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Interval", typeof(System.String)));

            table.Columns.Add(new DataColumn("Vr", typeof(System.Double)));
            table.Columns.Add(new DataColumn("Vy", typeof(System.Double)));
            table.Columns.Add(new DataColumn("Vb", typeof(System.Double)));

            table.Columns.Add(new DataColumn("Ir", typeof(System.Double)));
            table.Columns.Add(new DataColumn("Iy", typeof(System.Double)));
            table.Columns.Add(new DataColumn("Ib", typeof(System.Double)));

            table.Columns.Add(new DataColumn("DateValue", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxVr", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinVr", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxVy", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinVy", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxVb", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinVb", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxIr", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinIr", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxIy", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinIy", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxIb", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinIb", typeof(System.String)));

            table.Columns.Add(new DataColumn("kW", typeof(System.Double)));
            table.Columns.Add(new DataColumn("kVA", typeof(System.Double)));

            return table;
        }
        #endregion

        #region Set Values in Data Table for VI Profile Report
        /// <summary>
        /// Set Values in Data Table for VI Profile Report
        /// </summary>
        /// <param name="dsDemandProfile"></param>
        /// <returns></returns>
        private DataTable SetVIProfileDataTable(DataSet dsVIProfile)
        {//Create Temp Table for report.
            DataTable table = CreateVIProfileTempDataTable();
            int i = 0, next_Hr = 0, next_Minute = 0, currentHr = 0, currentMinute = 0;
            DateTime dtTmp;
            int IP = Convert.ToInt32(dsVIProfile.Tables[0].Rows[0]["MDIntervalPeriod"]);//Get IP value.
            TimeSpan tmpTimeSpan = new TimeSpan(0, IP, 0);//Time Span between consecutive IPs.
            string columnVal = string.Empty;
            foreach (DataRow datarow in dsVIProfile.Tables[0].Rows)
            {
                if (!ChkDateRange(datarow["Date Time"].ToString(), tmpTimeSpan))
                    continue;
                table.Rows.Add(table.NewRow());//"Date Time"
                foreach (DataColumn col in dsVIProfile.Tables[0].Columns)
                {
                    columnVal = Convert.ToString(datarow[col.ColumnName]);
                    if (columnVal != "")
                    {
                        switch (col.ColumnName)
                        {
                            case "Date Time":

                                #region Create Interval Value for Report.
                                dtTmp = Convert.ToDateTime(columnVal, cultureInfo).Subtract(tmpTimeSpan);
                                if (cultureInfo.Name == "en-US")
                                    table.Rows[i][7] = dtTmp.Month.ToString() + "/" + dtTmp.Day.ToString() + "/" + dtTmp.Year.ToString();
                                else
                                    table.Rows[i][7] = dtTmp.Day.ToString() + "/" + dtTmp.Month.ToString() + "/" + dtTmp.Year.ToString();
                              
                                    currentMinute = dtTmp.Minute;
                                currentHr = dtTmp.Hour;

                                next_Hr = currentHr;
                                next_Minute = currentMinute + IP;
                                if (next_Minute == 60)
                                {
                                    next_Hr++;
                                    next_Minute = 0;
                                }
                                table.Rows[i][0] = currentHr.ToString().PadLeft(2, '0') + ":" + dtTmp.Minute.ToString().PadLeft(2, '0')
                                                    + "-" +
                                                    next_Hr.ToString().PadLeft(2, '0') + ":" + next_Minute.ToString().PadLeft(2, '0');
                                #endregion

                                break;
                            case "Voltage R Phase":
                                table.Rows[i][1] = Math.Round(Convert.ToDouble(columnVal), 2); 
                                break;
                            case "Voltage Y Phase":
                                table.Rows[i][2] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                            case "Voltage B Phase":
                                table.Rows[i][3] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                            case "Current R Phase":
                                table.Rows[i][4] = Math.Round(Convert.ToDouble(columnVal), 3);
                                break;
                            case "Current Y Phase":
                                table.Rows[i][5] = Math.Round(Convert.ToDouble(columnVal), 3);
                                break;
                            case "Current B Phase":
                                table.Rows[i][6] = Math.Round(Convert.ToDouble(columnVal), 3);
                                break;
                            case "Demand kW":
                                table.Rows[i][20] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                            case "Demand kVA":
                                table.Rows[i][21] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                        }
                    }
                }
                i++;
            }
            return table;
        }
        #endregion

        #region Fill VI prowwwfile Data Table in Report XSD.
        // Added to fill the report dataset table with VI Profile Data.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDemandProfile"></param>
        private void FillVwwIProfileDataXSD(DataSet dsDemandProfile)
        {
            DataSet ds = new DataSet();//Set Demand Profile Data 
            ds.Tables.Add(SetDemandProfileDataTable(dsDemandProfile));

            if (ds.Tables[0].Rows.Count > 0)
            {//Initalize Max Min and Sum Values for each Demand Column.
                double[] maxValues = null, minValues = null, sumValues = null;
                InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                int fromrow = 0, torow = 0;
                string prevdate = string.Empty;
                DataRow reportrow;
                DateTime currentDateTime;
                LSIPType lsIPType = LSIPType.None;
                //Set Demand Columns.
                filteredColumsn = new ArrayList();
                filteredColumsn.Add("kW");
                filteredColumsn.Add("kVA");
                foreach (DataRow row in ds.Tables[0].Rows)
                {//Create a row w.r.t each LS row from DB.
                    reportrow = reportXSD.Tables["DemandProfileTable"].NewRow();
                    if (!string.IsNullOrEmpty(row["DPDateValue"].ToString()))
                        reportrow["DPDateValue"] = row["DPDateValue"].ToString();
                    lsIPType = ChkIPStatus(row);//Check for NP/NL.
                    if (lsIPType != LSIPType.None)//Set NP/NL Status if apply.
                    { reportrow["kW"] = reportrow["kVA"] = lsIPType.ToString(); }

                    if (torow == 0)
                    {
                        if (lsIPType == LSIPType.None)
                        {//Set Values in Min/Max array for the first time.
                            if (row["kW"] != null && Convert.ToDouble(row["kW"]) >= 0.0)
                            { minValues[0] = maxValues[0] = Convert.ToDouble(row["kW"]); }
                            if (row["kVA"] != null && Convert.ToDouble(row["kVA"]) >= 0.0)
                            { minValues[1] = maxValues[1] = Convert.ToDouble(row["kVA"]); }
                        }
                        prevdate = row["DPDateValue"].ToString();
                    }
                    currentDateTime = Convert.ToDateTime(row["DPDateValue"], cultureInfo);
                    if (currentDateTime < fromDate.Value.Date || currentDateTime > toDate.Value.Date)
                        continue;
                    //Set Interval value.
                    if (!string.IsNullOrEmpty(row["DPInterval"].ToString()))
                        reportrow["DPInterval"] = row["DPInterval"].ToString();
                    else
                        reportrow["DPInterval"] = dateUnavailable;
                    if (lsIPType != LSIPType.NP)
                    {
                        if (!string.IsNullOrEmpty(row["Event"].ToString()))
                            reportrow["Event"] = row["Event"].ToString();
                        else
                            reportrow["Event"] = dateUnavailable;
                    }
                    else
                        reportrow["Event"] = LSIPType.NP.ToString();

                    if (lsIPType == LSIPType.None)
                    {
                        if (!string.IsNullOrEmpty(row["kW"].ToString()))
                            reportrow["kW"] = row["kW"].ToString();
                        else
                            reportrow["kW"] = dateUnavailable;

                        if (!string.IsNullOrEmpty(row["kVA"].ToString()))
                            reportrow["kVA"] = row["kVA"].ToString();
                        else
                            reportrow["kVA"] = dateUnavailable;
                    }
                    if (prevdate != reportrow["DPDateValue"].ToString())
                    {
                        SetMaxMinSumValuesDemandProfileTable(maxValues, minValues, sumValues, fromrow, torow);
                        InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                        fromrow = torow;
                    }

                    torow++;
                    SetMaxMinSumValuesInArrays(row, ref minValues, ref maxValues, ref sumValues);

                    prevdate = reportrow["DPDateValue"].ToString();
                    reportXSD.Tables["DemandProfileTable"].Rows.Add(reportrow);
                }
                SetMaxMinSumValuesDemandProfileTable(maxValues, minValues, sumValues, fromrow, torow);
            }
        }
        #endregion


        // Added to fill the report dataset table with VI Profile Data.
        private void FillVIProfileDataXSD(DataSet dsVIProfile)
        {
                DataSet ds = new DataSet();//Set VI Profile Data 
                ds.Tables.Add(SetVIProfileDataTable(dsVIProfile));
                if (ds.Tables[0].Rows.Count > 0)
                {//Initalize Max Min Values for each VI Column.
                    double[] maxValues = new double[6], minValues = new double[6], sumValues = new double[0];
                    InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                    int fromrow = 0, torow = 0;
                    string prevdate = string.Empty;
                    DataRow reportrow;
                    DateTime currentDateTime;
                    LSIPType lsIPType = LSIPType.None;
                    //Set VI Columns.
                    filteredColumsn = new ArrayList();
                    filteredColumsn.Add("kW");
                    filteredColumsn.Add("kVA");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {//Create a row w.r.t each LS row from DB.
                        reportrow = reportXSD.Tables["VIProfileTable"].NewRow();
                        if (!string.IsNullOrEmpty(row["DateValue"].ToString()))
                            reportrow["DateValue"] = row["DateValue"].ToString();
                        lsIPType = ChkIPStatus(row);//Check for NP/NL.
                        if (lsIPType == LSIPType.NP)//Set NP/NL Status if apply.
                        { reportrow["Vr"] = reportrow["Vy"] = reportrow["Vb"] = reportrow["Ir"] = reportrow["Iy"] = reportrow["Ib"] = lsIPType.ToString(); }
                        if (torow == 0)
                        {
                            if (lsIPType != LSIPType.NP)
                            {//Set Values in Min/Max array for the first time.
                                if (row["Vr"] != null && Convert.ToDouble(row["Vr"]) >= 0.0) { minValues[0] = maxValues[0] = Convert.ToDouble(row["Vr"]); }
                                if (row["Vy"] != null && Convert.ToDouble(row["Vy"]) >= 0.0) { minValues[1] = maxValues[1] = Convert.ToDouble(row["Vy"]); }
                                if (row["Vb"] != null && Convert.ToDouble(row["Vb"]) >= 0.0) { minValues[2] = maxValues[2] = Convert.ToDouble(row["Vb"]); }

                                if (row["Ir"] != null && Convert.ToDouble(row["Ir"]) >= 0.0) { minValues[3] = maxValues[3] = Convert.ToDouble(row["Ir"]); }
                                if (row["Iy"] != null && Convert.ToDouble(row["Iy"]) >= 0.0) { minValues[4] = maxValues[4] = Convert.ToDouble(row["Iy"]); }
                                if (row["Ib"] != null && Convert.ToDouble(row["Ib"]) >= 0.0) { minValues[5] = maxValues[5] = Convert.ToDouble(row["Ib"]); }
                            }
                            prevdate = row["DateValue"].ToString();
                        }

                        currentDateTime = Convert.ToDateTime(row["DateValue"], cultureInfo);
                        if (currentDateTime < fromDate.Value.Date || currentDateTime > toDate.Value.Date)
                            continue;

                        if (!string.IsNullOrEmpty(row["Interval"].ToString()))
                            reportrow["Interval"] = row["Interval"].ToString();
                        else
                            reportrow["Interval"] = dateUnavailable;

                        if (lsIPType != LSIPType.NP)
                        {
                            if (!string.IsNullOrEmpty(row["Vr"].ToString()))
                                reportrow["Vr"] = row["Vr"];//.ToString();
                            if (!string.IsNullOrEmpty(row["Vy"].ToString()))
                                reportrow["Vy"] = row["Vy"];
                            if (!string.IsNullOrEmpty(row["Vb"].ToString()))
                                reportrow["Vb"] = row["Vb"];//.ToString();
                            if (!string.IsNullOrEmpty(row["Ir"].ToString()))
                                reportrow["Ir"] = row["Ir"];//.ToString();
                            if (!string.IsNullOrEmpty(row["Iy"].ToString()))
                                reportrow["Iy"] = row["Iy"];//.ToString();
                            if (!string.IsNullOrEmpty(row["Ib"].ToString()))
                                reportrow["Ib"] = row["Ib"];//.ToString();
                        }
                        if (prevdate != reportrow["DateValue"].ToString())
                        {
                            SetMaxMinValuesVIProfileTable(maxValues, minValues, fromrow, torow);
                            InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                            fromrow = torow;
                        }
                        torow++;
                        for (int c = 0; c < 6; c++)
                        {
                            if (Convert.ToDouble(row[c + 1]) >= 0.0)
                            {
                                maxValues[c] = Math.Max(maxValues[c], Convert.ToDouble(row[c + 1]));
                                minValues[c] = Math.Min(minValues[c], Convert.ToDouble(row[c + 1]));
                            }

                        }
                        reportXSD.Tables["VIProfileTable"].Rows.Add(reportrow);
                        prevdate = reportrow["DateValue"].ToString();
                    }
                    SetMaxMinValuesVIProfileTable(maxValues, minValues, fromrow, torow);
                }
        }

        #region Set Max Min Sum Values w.r.t to columns in the Data Row
        /// <summary>
        /// Set Max Min Sum Values w.r.t to columns in the Data Row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="minValues"></param>
        /// <param name="maxValues"></param>
        /// <param name="sumValues"></param>
        private void SetMaxMinSumValuesInArrays(DataRow row, ref double[] minValues, ref  double[] maxValues, ref  double[] sumValues)
        {
            for (int c = 0; c < 2; c++)
            {//Filter -1 valued columns.
                if (Convert.ToDouble(row[c + 2]) >= 0.0)
                {//Set Max/min in max/min array w.r.t corresponding column in Row 
                    maxValues[c] = Math.Max(maxValues[c], Convert.ToDouble(row[c + 2]));
                    minValues[c] = Math.Min(minValues[c], Convert.ToDouble(row[c + 2]));
                    //Set Sum in Sum array w.r.t corresponding column in Row 
                    if (sumValues[c] != double.MinValue)
                        sumValues[c] += Convert.ToDouble(row[c + 2]);
                    else
                        sumValues[c] = Convert.ToDouble(row[c + 2]);
                }
            }
        }
        #endregion

        #region InitalizeArrays
        /// <summary>
        /// InitalizeArrays
        /// </summary>
        /// <param name="minValues"></param>
        /// <param name="maxValues"></param>
        /// <param name="sumValues"></param>
        private void InitalizeArrays(ref double[] minValues, ref  double[] maxValues, ref  double[] sumValues)
        {
            for (int i = 0; i < maxValues.Length; i++)
            {
               maxValues[i] = double.MinValue;
               if (sumValues.Length != 0)
                   sumValues[i] = double.MinValue;
               minValues[i] = double.MaxValue;
            }
           
        }
        #endregion

        #region Create Temporary Data Table for DemandProfile Report
        /// <summary>
        /// CreateDemandProfileTempDataTable
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDemandProfileTempDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DPInterval", typeof(System.String)));

            table.Columns.Add(new DataColumn("Event", typeof(System.String)));
            table.Columns.Add(new DataColumn("kW", typeof(System.Double)));
            table.Columns.Add(new DataColumn("kVA", typeof(System.Double)));
            table.Columns.Add(new DataColumn("DPDateValue", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxkW", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinkW", typeof(System.String)));
            table.Columns.Add(new DataColumn("SumkW", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxkVA", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinkVA", typeof(System.String)));
            table.Columns.Add(new DataColumn("SumkVA", typeof(System.String)));
            return table;
        }
#endregion

        #region Set Values in Data Table for DemandProfile Report
        /// <summary>
        /// Set Values in Data Table for DemandProfile Report
        /// </summary>
        /// <param name="dsDemandProfile"></param>
        /// <returns></returns>
        private DataTable SetDemandProfileDataTable(DataSet dsDemandProfile)
        {//Create Temp Table for report.
            DataTable table = CreateDemandProfileTempDataTable();
            int i = 0, next_Hr = 0, next_Minute = 0, currentHr = 0, currentMinute = 0;
            DateTime dtTmp;
            int IP = Convert.ToInt32(dsDemandProfile.Tables[0].Rows[0]["MDIntervalPeriod"]);//Get IP value.
            TimeSpan tmpTimeSpan = new TimeSpan(0, IP, 0);//Time Span between consecutive IPs.
            string columnVal = string.Empty;
            foreach (DataRow datarow in dsDemandProfile.Tables[0].Rows)
            {
                if (!ChkDateRange(datarow["Date Time"].ToString(), tmpTimeSpan))
                    continue;
                table.Rows.Add(table.NewRow());//"Date Time"
                foreach (DataColumn col in dsDemandProfile.Tables[0].Columns)
                {
                    columnVal = Convert.ToString(datarow[col.ColumnName]);
                    if (columnVal != "")
                    {
                        switch (col.ColumnName)
                        {
                            case "Date Time":
                                
                                #region Create Interval Value for Report.
                                dtTmp = Convert.ToDateTime(columnVal, cultureInfo).Subtract(tmpTimeSpan);
                                if (cultureInfo.Name == "en-US")
                                    table.Rows[i][4] = dtTmp.Month.ToString() + "/" + dtTmp.Day.ToString() + "/" + dtTmp.Year.ToString();
                                else
                                    table.Rows[i][4] = dtTmp.Day.ToString() + "/" + dtTmp.Month.ToString() + "/" + dtTmp.Year.ToString();

                                currentMinute = dtTmp.Minute;
                                currentHr = dtTmp.Hour;

                                next_Hr = currentHr;
                                next_Minute = currentMinute + IP;
                                if (next_Minute == 60)
                                {
                                    next_Hr++;
                                    next_Minute = 0;
                                }
                                table.Rows[i][0] = currentHr.ToString().PadLeft(2, '0') + ":" + dtTmp.Minute.ToString().PadLeft(2, '0')
                                                    + "-" +
                                                    next_Hr.ToString().PadLeft(2, '0') + ":" + next_Minute.ToString().PadLeft(2, '0');
                                #endregion

                                break;
                            case "TamperStatus":
                                table.Rows[i][1] = columnVal;
                                break;
                            case "Demand kW":
                                table.Rows[i][2] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                            case "Demand kVA":
                                table.Rows[i][3] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                        }
                    }
                }
                i++;
            }
            return table;
        }
#endregion

        #region Fill Demand profile Data Table in Report XSD.
        // Added to fill the report dataset table with Demand Profile Data.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDemandProfile"></param>
        private void FillDemandProfileDataXSD(DataSet dsDemandProfile)
        {
            DataSet ds = new DataSet();//Set Demand Profile Data 
            ds.Tables.Add(SetDemandProfileDataTable(dsDemandProfile));

            if (ds.Tables[0].Rows.Count > 0)
            {//Initalize Max Min and Sum Values for each Demand Column.
                double[] maxValues = new double[2], minValues = new double[2], sumValues = new double[2];
                InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                int fromrow = 0, torow = 0;
                string prevdate = string.Empty;
                DataRow reportrow;
                DateTime currentDateTime;
                LSIPType lsIPType=LSIPType.None;
                //Set Demand Columns.
                filteredColumsn = new ArrayList();
                filteredColumsn.Add("kW");
                filteredColumsn.Add("kVA");
                foreach (DataRow row in ds.Tables[0].Rows)
                {//Create a row w.r.t each LS row from DB.
                    reportrow = reportXSD.Tables["DemandProfileTable"].NewRow();
                    if (!string.IsNullOrEmpty(row["DPDateValue"].ToString()))
                        reportrow["DPDateValue"] = row["DPDateValue"].ToString();
                    lsIPType = ChkIPStatus(row);//Check for NP/NL.
                    if (lsIPType != LSIPType.None)//Set NP/NL Status if apply.
                    { reportrow["kW"] = reportrow["kVA"] = lsIPType.ToString(); }
                    
                    if (torow == 0)
                    {
                        if (lsIPType == LSIPType.None)
                        {//Set Values in Min/Max array for the first time.
                            if (row["kW"] != null && Convert.ToDouble(row["kW"]) >= 0.0) 
                                    { minValues[0] = maxValues[0] = Convert.ToDouble(row["kW"]); }
                            if (row["kVA"] != null && Convert.ToDouble(row["kVA"]) >= 0.0)
                                    { minValues[1] = maxValues[1] = Convert.ToDouble(row["kVA"]); }
                        }
                        prevdate = row["DPDateValue"].ToString();
                    }
                    currentDateTime = Convert.ToDateTime(row["DPDateValue"], cultureInfo);
                    if (currentDateTime < fromDate.Value.Date || currentDateTime > toDate.Value.Date)
                        continue;
                    //Set Interval value.
                    if (!string.IsNullOrEmpty(row["DPInterval"].ToString()))
                        reportrow["DPInterval"] = row["DPInterval"].ToString();
                    else
                        reportrow["DPInterval"] = dateUnavailable;
                    if (lsIPType != LSIPType.NP)
                    {
                        if (!string.IsNullOrEmpty(row["Event"].ToString()))
                            reportrow["Event"] = row["Event"].ToString();
                        else
                            reportrow["Event"] = dateUnavailable;
                    }
                    else
                        reportrow["Event"] = LSIPType.NP.ToString();

                    if (lsIPType == LSIPType.None)
                    {
                        if (!string.IsNullOrEmpty(row["kW"].ToString()))
                            reportrow["kW"] = row["kW"].ToString();
                        else
                            reportrow["kW"] = dateUnavailable;

                        if (!string.IsNullOrEmpty(row["kVA"].ToString()))
                            reportrow["kVA"] = row["kVA"].ToString();
                        else
                            reportrow["kVA"] = dateUnavailable;
                    }
                    if (prevdate != reportrow["DPDateValue"].ToString())
                    {
                        SetMaxMinSumValuesDemandProfileTable(maxValues, minValues, sumValues, fromrow, torow);
                        InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                        fromrow = torow;
                    }
                  
                    torow++;
                    SetMaxMinSumValuesInArrays(row, ref minValues, ref maxValues, ref sumValues);

                    prevdate = reportrow["DPDateValue"].ToString();
                    reportXSD.Tables["DemandProfileTable"].Rows.Add(reportrow);
                }
                SetMaxMinSumValuesDemandProfileTable(maxValues, minValues, sumValues, fromrow, torow);
            }
        }
        #endregion

        #region Create Temporary Data Table for Energy Profile Report
        private DataTable CreateEnergyProfileTempDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("EPInterval", typeof(System.String)));

            table.Columns.Add(new DataColumn("Event", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWH", typeof(System.Double)));
            table.Columns.Add(new DataColumn("kVAH", typeof(System.Double)));
            table.Columns.Add(new DataColumn("EPDateValue", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxkWH", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinkWH", typeof(System.String)));
            table.Columns.Add(new DataColumn("SumkWH", typeof(System.String)));

            table.Columns.Add(new DataColumn("MaxkVAH", typeof(System.String)));
            table.Columns.Add(new DataColumn("MinkVAH", typeof(System.String)));
            table.Columns.Add(new DataColumn("SumkVAH", typeof(System.String)));
            return table;
        }
        #endregion

        #region Set Values in Data Table for Energy Profile Report
        private DataTable SetEnergyProfileDataTable(DataSet dsEnergyProfile)
        {//Create Temp Table for report.
            DataTable table = CreateEnergyProfileTempDataTable();
            int i = 0, next_Hr = 0, next_Minute = 0, currentHr = 0, currentMinute = 0;
            DateTime dtTmp;
            int IP = Convert.ToInt32(dsEnergyProfile.Tables[0].Rows[0]["MDIntervalPeriod"]);//Get IP value.
            TimeSpan tmpTimeSpan = new TimeSpan(0, IP, 0);//Time Span between consecutive IPs.
            string columnVal = string.Empty;
            foreach (DataRow datarow in dsEnergyProfile.Tables[0].Rows)
            {
                if (!ChkDateRange(datarow["Date Time"].ToString(), tmpTimeSpan))
                    continue;
                table.Rows.Add(table.NewRow());//"Date Time"
                foreach (DataColumn col in dsEnergyProfile.Tables[0].Columns)
                {
                    columnVal = Convert.ToString(datarow[col.ColumnName]);
                    if (columnVal != "")
                    {
                        switch (col.ColumnName)
                        {
                            case "Date Time":
                                #region Create Interval Value for Report.
                                dtTmp = Convert.ToDateTime(columnVal, cultureInfo).Subtract(tmpTimeSpan);
                                if (cultureInfo.Name == "en-US")
                                    table.Rows[i][4] = dtTmp.Month.ToString() + "/" + dtTmp.Day.ToString() + "/" + dtTmp.Year.ToString();
                                else
                                    table.Rows[i][4] = dtTmp.Day.ToString() + "/" + dtTmp.Month.ToString() + "/" + dtTmp.Year.ToString();

                                currentMinute = dtTmp.Minute;
                                currentHr = dtTmp.Hour;

                                next_Hr = currentHr;
                                next_Minute = currentMinute + IP;
                                if (next_Minute == 60)
                                {
                                    next_Hr++;
                                    next_Minute = 0;
                                }
                                table.Rows[i][0] = currentHr.ToString().PadLeft(2, '0') + ":" + dtTmp.Minute.ToString().PadLeft(2, '0')
                                                    + "-" +
                                                    next_Hr.ToString().PadLeft(2, '0') + ":" + next_Minute.ToString().PadLeft(2, '0');
                                #endregion
                                break;
                            case "TamperStatus":
                                table.Rows[i][1] = columnVal;
                                break;
                            case "Energy kWh":
                                table.Rows[i][2] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                            case "Energy kVAh":
                                table.Rows[i][3] = Math.Round(Convert.ToDouble(columnVal), 2);
                                break;
                        }
                    }
                }
                i++;
            }
            return table;
        }
        #endregion

        #region Fill Energy profile Data Table in Report XSD.
        // Added to fill the report dataset table with Energy Profile Data.
        private void FillEnergyProfileDataXSD(DataSet dsEnergyProfile)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(SetEnergyProfileDataTable(dsEnergyProfile));
           
            if (ds.Tables[0].Rows.Count > 0)
            {
                double[] maxValues = new double[2], minValues = new double[2], sumValues = new double[2];
                InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                int fromrow = 0, torow = 0;
                string prevdate = string.Empty;
                DataRow reportrow;
                DateTime currentDateTime;
                LSIPType lsIPType = LSIPType.None;
                //Set Demand Columns.
                filteredColumsn = new ArrayList();
                filteredColumsn.Add("kWH");
                filteredColumsn.Add("kVAH");
                foreach (DataRow row in ds.Tables[0].Rows)
                {//Create a row w.r.t each LS row from DB.
                    reportrow = reportXSD.Tables["EnergyProfileTable"].NewRow();
                    if (!string.IsNullOrEmpty(row["EPDateValue"].ToString()))
                        reportrow["EPDateValue"] = row["EPDateValue"].ToString();
                    lsIPType = ChkIPStatus(row);//Check for NP/NL.
                    if (lsIPType != LSIPType.None)//Set NP/NL Status if apply.
                    { reportrow["kWH"] = reportrow["kVAH"] = lsIPType.ToString(); }

                    if (torow == 0)
                    {
                        if (lsIPType == LSIPType.None)
                        {//Set Values in Min/Max array for the first time.
                            if (row["kWH"] != null && Convert.ToDouble(row["kWH"]) >= 0.0)
                            { minValues[0] = maxValues[0] = Convert.ToDouble(row["kWH"]); }
                            if (row["kVAH"] != null && Convert.ToDouble(row["kVAH"]) >= 0.0)
                            { minValues[1] = maxValues[1] = Convert.ToDouble(row["kVAH"]); }
                        }
                        prevdate = row["EPDateValue"].ToString();
                    }
                    currentDateTime = Convert.ToDateTime(row["EPDateValue"], cultureInfo);
                    if (currentDateTime < fromDate.Value.Date || currentDateTime > toDate.Value.Date)
                        continue;
                    //Set Interval value.
                    if (!string.IsNullOrEmpty(row["EPInterval"].ToString()))
                        reportrow["EPInterval"] = row["EPInterval"].ToString();
                    else
                        reportrow["EPInterval"] = dateUnavailable;

                    if (lsIPType != LSIPType.NP)
                    {
                        if (!string.IsNullOrEmpty(row["Event"].ToString()))
                            reportrow["Event"] = row["Event"].ToString();
                        else
                            reportrow["Event"] = dateUnavailable;
                    }
                    else
                        reportrow["Event"] = LSIPType.NP.ToString();

                    if (lsIPType == LSIPType.None)
                    {
                        if (!string.IsNullOrEmpty(row["kWH"].ToString()))
                            reportrow["kWH"] = row["kWH"].ToString();
                        else
                            reportrow["kWH"] = dateUnavailable;

                        if (!string.IsNullOrEmpty(row["kVAH"].ToString()))
                            reportrow["kVAH"] = row["kVAH"].ToString();
                        else
                            reportrow["kVAH"] = dateUnavailable;
                    }
                    if (prevdate != reportrow["EPDateValue"].ToString())
                    {
                        SetMaxMinSumValuesEnergyProfileTable(maxValues, minValues, sumValues, fromrow, torow);
                        InitalizeArrays(ref minValues, ref maxValues, ref sumValues);
                        fromrow = torow;
                    }
                    torow++;
                    SetMaxMinSumValuesInArrays(row, ref minValues, ref maxValues, ref sumValues);
                    prevdate = reportrow["EPDateValue"].ToString();
                    reportXSD.Tables["EnergyProfileTable"].Rows.Add(reportrow);
                }
                SetMaxMinSumValuesEnergyProfileTable(maxValues, minValues, sumValues, fromrow, torow);
            }
        }
        #endregion

        // This function is used to set maximum values , minimum values and sum values in Energy Profile Report.
        private void SetMaxMinSumValuesEnergyProfileTable(double[] maxValues, double[] minValues, double[] sumValues, int fromrow, int torow)
        {
            for (int j = fromrow; j < torow; j++)
            {
                if (maxValues[0] != double.MinValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MaxkWH"] = Math.Round(maxValues[0],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MaxkWH"] = "-----";

                if (minValues[0] != double.MaxValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MinkWH"] = Math.Round(minValues[0],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MinkWH"] = "-----";

                if (sumValues[0] != double.MinValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["SumkWH"] = Math.Round(sumValues[0],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["SumkWH"] = "-----";

                if (maxValues[1] != double.MinValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MaxkVAH"] = Math.Round(maxValues[1],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MaxkVAH"] = "-----";

                if (minValues[1] != double.MaxValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MinkVAH"] = Math.Round(minValues[1],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["MinkVAH"] = "-----";

                if (sumValues[1] != double.MinValue)
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["SumkVAH"] = Math.Round(sumValues[1],1).ToString();
                else
                    reportXSD.Tables["EnergyProfileTable"].Rows[j]["SumkVAH"] = "-----";
            }
        }
        // This function is used to set maximum values , minimum values and sum values in Demand Profile Report.
        private void SetMaxMinSumValuesDemandProfileTable(double[] maxValues, double[] minValues, double[] sumValues, int fromrow, int torow)
        {
            for (int j = fromrow; j < torow; j++)
            {
                if (maxValues[0] != double.MinValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MaxkW"] = Math.Round(maxValues[0], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MaxkW"] = "-----";

                if (minValues[0] != double.MaxValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MinkW"] = Math.Round(minValues[0], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MinkW"] = "-----";

                if (sumValues[0] != double.MinValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["SumkW"] = Math.Round(sumValues[0], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["SumkW"] = "-----";

                if (maxValues[1] != double.MinValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MaxkVA"] = Math.Round(maxValues[1], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MaxkVA"] = "-----";

                if (minValues[1] != double.MaxValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MinkVA"] = Math.Round(minValues[1], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["MinkVA"] = "-----";

                if (sumValues[1] != double.MinValue)
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["SumkVA"] = Math.Round(sumValues[1], 2).ToString();
                else
                    reportXSD.Tables["DemandProfileTable"].Rows[j]["SumkVA"] = "-----";
            }
        }
        // This function is used to set maximum values and minimum values in VI Profile Report.
        private void SetMaxMinValuesVIProfileTable(double[] maxValues, double[] minValues, int fromrow, int torow)
        {
            for (int j = fromrow; j < torow; j++)
            {
                if (maxValues[0] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVr"] = Math.Round(maxValues[0], 2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVr"] = "-----";

                if (minValues[0] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVr"] = Math.Round(minValues[0], 2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVr"] = "-----";

                if (maxValues[1] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVy"] = Math.Round(maxValues[1], 2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVy"] = "-----";

                if (minValues[1] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVy"] = Math.Round(minValues[1], 2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVy"] = "-----";

                if (maxValues[2] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVb"] = Math.Round(maxValues[2],2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxVb"] = "-----";

                if (minValues[2] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVb"] = Math.Round(minValues[2],2).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinVb"] = "-----";

                if (maxValues[3] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIr"] = Math.Round(maxValues[3],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIr"] = "-----";

                if (minValues[3] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIr"] =  Math.Round(minValues[3],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIr"] = "-----";

                if (maxValues[4] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIy"] =  Math.Round(maxValues[4],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIy"] = "-----";

                if (minValues[4] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIy"] =  Math.Round(minValues[4],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIy"] = "-----";

                if (maxValues[5] != double.MinValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIb"] =  Math.Round(maxValues[5],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MaxIb"] = "-----";

                if (minValues[5] != double.MaxValue)
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIb"] =  Math.Round(minValues[5],3).ToString();
                else
                    reportXSD.Tables["VIProfileTable"].Rows[j]["MinIb"] = "-----";
            }
        }
        // Added to fill the report dataset table with Load survey Demand data.
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

                reportRow = reportXSD.Tables["DemandProfile"].NewRow();

                //foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                //    lsHeadings.Add(col.ColumnName);

                foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["DemandProfile"].NewRow();
                    //if (dateTimeCount == 0)
                    //{
                    //    PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                    //    reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0])));
                    //    dateTimeCount++;
                    //}
                    //else
                    //{
                    string dates = "";
                    DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                    TimeSpan ts = currentDate - PreviousDate;
                    if (ts.Days > 0)
                    {
                        currentDate = currentDate.AddDays(-1);
                        long datesval = DateUtility.DateTimeToLong(currentDate);
                        dates = DateUtility.LongToStringDateFormat(datesval);
                        reportRow["Date"] = dates;
                        PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                    }
                    else
                    {
                        dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0])));
                        reportRow["Date"] = dates;
                    }
                    //}
                    //for (int colCount = 1; colCount < loadSurveyData.Tables[0].Columns.Count - 2; colCount++)
                    //{
                    //    string ParameterColValue = "Parameter" + Convert.ToString(colCount);
                    //    reportRow[ParameterColValue] = row[colCount].ToString();
                    //}
                    reportRow["kwDelivered"] = Convert.ToString(row["Demand kW"]);
                    reportRow["kVADelivered"] = Convert.ToString(row["Demand kVA"]);
                    //reportRow["Tampers"] = Convert.ToString(row["TamperStatus"]);

                    reportRow["Tampers"] = Convert.ToString(row["TamperStatus"]);
                    if (reportRow["Tampers"].ToString() == "000000")
                        reportRow["Tampers"] = "-----------------------------------";
                    else
                    {
                        reportRow["Tampers"] = reportRow["Tampers"].ToString();// new LoadSurveyBLL().GetTamperNames(reportRow["Tampers"].ToString()); 
                    }
                    string dateTimes = Convert.ToString(row[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeInterval"] = dateTimes;
                    reportXSD.Tables["DemandProfile"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,resourceMgr.GetString("BCS"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Added for detailed report1(Portrait)
        private void btnShow_Click_1(object sender, EventArgs e)
        {
            reportXSD.Clear();
            //Fix defect #163498
            this.StatusMessage = string.Empty;

            int errCount = 0;
            int selectedParams = 0;
            int showReport = 0;
            string[] message = new string[9];
            
                if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
                {
                    this.StatusMessage = resourceMgr.GetString("CAB File");
                    return;
                }

                if (ValidateForm())
                {
                    this.StatusMessage = resourceMgr.GetString("Select");
                    return;
                }
                
                else
                    FillHeaderDetails();
                
                if (!ChkValidations())
                {
                    MessageBox.Show(resourceMgr.GetString("ChkValidations"), resourceMgr.GetString("BCS"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
 
                Cursor.Current = Cursors.WaitCursor;
                
                try
                {
                // Cumulative Energies Report
                if (chkCumulativeEnergies.Checked)
                {
                    selectedParams++;
                    DataSet CumulativeEnergiesds = new DataSet();
                    CumulativeEnergiesds = ListCumulativeEnergies(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (CumulativeEnergiesds != null && CumulativeEnergiesds.Tables.Count > 0 && CumulativeEnergiesds.Tables[0].Rows.Count > 0)
                    {
                        reportXSD.Tables["CumulativeEnergiesReport"].Rows.Clear();
                        FillCumulativeEnergy(CumulativeEnergiesds);
                        showReport++;
                        
                    }
                    else
                    {
                        message[0] = resourceMgr.GetString("Cumulative Energy");
                        errCount++;
                    }

                }
                //CumulativeTamperEventSection
                if (chkCumulativeTamperEvents.Checked)
                {
                    selectedParams++;
                    DataSet dsTamperData = new DataSet();
                    dsTamperData = ListTamperData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                    //  CommonBLL.ConvertTamperOccurRestore(MeterDataId, tamperId);

                    if (dsTamperData != null && dsTamperData.Tables.Count > 0 && dsTamperData.Tables[0].Rows.Count > 0)
                    {
                        FillCumulativeTamperEventsDataXSD(dsTamperData);
                        showReport++;
                        
                    }
                    else
                    {
                        message[1] = resourceMgr.GetString("Cumulative Tamper Events");
                        errCount++;
                    }
                }
                if (chkDailyEnergyConsumption.Checked)
                {
                    selectedParams++;
                    DataSet dsdailyEnergyConsumtion = new DataSet();
                    dsdailyEnergyConsumtion = ListDailyEnergyConsumption(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (dsdailyEnergyConsumtion != null && dsdailyEnergyConsumtion.Tables.Count > 0 && dsdailyEnergyConsumtion.Tables[0].Rows.Count > 0)
                    {
                        FillDailyenergyConsumptionDataXSD(dsdailyEnergyConsumtion);

                        showReport++;
                       
                    }
                    else
                    {
                        message[2] = resourceMgr.GetString("Daily Energy");
                        errCount++;
                    }
                }
                if (chkDailyMaximumData.Checked)
                {
                    selectedParams++;
                    DataSet dailyMDData = new DataSet();
                    dailyMDData = ListDailyMDData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), "Demand");
                    if (dailyMDData != null && dailyMDData.Tables.Count > 0 && dailyMDData.Tables[0].Rows.Count > 0)
                    {
                        FillMDDataXSD(dailyMDData);

                        
                        showReport++;
                    }
                    else
                    {
                        message[3] = resourceMgr.GetString("Daily Maximum");
                        errCount++;
                    }
                }
                if (chkInstantaneousData.Checked)
                {
                    selectedParams++;
                    DataSet instantaneousData = new DataSet();
                    instantaneousData = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (instantaneousData != null && instantaneousData.Tables.Count > 0 && instantaneousData.Tables[0].Rows.Count > 0)
                    {
                        FillInstantDatXSD(instantaneousData);

                        showReport++;
                        
                    }
                    else
                    {
                        message[4] = resourceMgr.GetString("Instantaneous");
                        errCount++;
                    }
                }
                if (chkVIProfile.Checked)
                {
                   
                        selectedParams++;
                        DataSet dsVIProfile = new DataSet();
                        dsVIProfile = ListVIProfileData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                        if (dsVIProfile != null && dsVIProfile.Tables.Count > 0 && dsVIProfile.Tables[0].Rows.Count > 0)
                        {
                            FillVIProfileDataXSD(dsVIProfile);

                           
                            
                        }
                        if (reportXSD.Tables["VIProfileTable"].Rows.Count <= 0)
                        {
                            message[5] = resourceMgr.GetString("VI Profile");
                            errCount++;
                        }
                        else
                            showReport++;
                    
                }

                if (chkProfileDemand.Checked)
                {
                   
                        selectedParams++;
                        DataSet dsDemandProfile = new DataSet();
                        dsDemandProfile = ListProfileData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), "Demand");
                        if (dsDemandProfile != null && dsDemandProfile.Tables.Count > 0 && dsDemandProfile.Tables[0].Rows.Count > 0)
                        {
                            FillDemandProfileDataXSD(dsDemandProfile);
                        }
                       if(reportXSD.Tables["DemandProfileTable"].Rows.Count <=0)
                        {
                            message[6] = resourceMgr.GetString("Demand Profile");
                            errCount++;
                        }
                       else
                           showReport++;
                   
                }
                if (chkProfileEnergy.Checked)
                {
                    
                        selectedParams++;
                        DataSet dsEnergyProfile = new DataSet();
                        dsEnergyProfile = ListProfileData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), "Energy");
                        if (dsEnergyProfile != null && dsEnergyProfile.Tables.Count > 0 && dsEnergyProfile.Tables[0].Rows.Count > 0)
                        {
                            FillEnergyProfileDataXSD(dsEnergyProfile);

                           
                        }
                        if (reportXSD.Tables["EnergyProfileTable"].Rows.Count <= 0)
                        {
                            message[7] = resourceMgr.GetString("Energy Profile");
                            errCount++;
                        }
                        else
                            showReport++;
                }
                if (chkPhasorReport.Checked)
                {

                    selectedParams++;
                //    this.Visible = false;
                    PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                    phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                    phasorDiagramForm.ShowDialog();
                //    this.Visible = true;

                    if (phasorDiagramForm.PhasorDataAvailable)
                    {
                        FillPhasorTable();
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        message[8] = resourceMgr.GetString("PhasorDataMsg");
                    }

                }
               
                if (errCount == 0)
                    ShowReport(message);
                else
                {
                    if (errCount == selectedParams)
                    { MessageBox.Show(resourceMgr.GetString("No Data"), resourceMgr.GetString("BCS"), MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    if (showReport > 0)
                    { ShowReport(message); }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(resourceMgr.GetString("Error"),resourceMgr.GetString("BCS"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.StatusMessage = "";
            }
        }

        private void FillPhasorTable()
        {
          //get the image file into a stream reader.
          string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
          filePath = filePath + "PrintPage.jpg";

          FileStream FilStr = new FileStream(filePath, FileMode.Open);
          BinaryReader BinRed = new BinaryReader(FilStr);
               //PhasorDiagramTable
          DataRow reportrow = reportXSD.Tables["PhasorDiagramTable"].NewRow();
            reportrow["Image"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
            reportXSD.Tables["PhasorDiagramTable"].Rows.Add(reportrow);
          FilStr.Close();
          BinRed.Close();

        
          //create the report object
       //   DynamicImageExample DyImg = new DynamicImageExample();
          // feed the dataset to the report.
         // DyImg.SetDataSource(this.DsImages);
      //    this.crystalReportViewer1.ReportSource = DyImg;

    }





       // This function is used to validate check conditions of all the reports checkboxes.
        private bool ValidateForm()
        {
            if (chkCumulativeEnergies.Checked == false && chkCumulativeTamperEvents.Checked == false && chkDailyEnergyConsumption.Checked == false
                && chkDailyMaximumData.Checked == false && chkProfileDemand.Checked == false && chkProfileEnergy.Checked ==false 
                 && chkInstantaneousData.Checked == false && chkVIProfile.Checked == false && chkPhasorReport.Checked==false)

                return true;
            return false;
                
        }
        private void chkGeneralReport_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SMD_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (SMD_SelectAll.Checked == true)
            {
                chkCumulativeEnergies.Checked = true;
                chkCumulativeTamperEvents.Checked = true;
                chkDailyEnergyConsumption.Checked = true;
                chkDailyMaximumData.Checked = true;
                chkInstantaneousData.Checked = true;
                chkProfileDemand.Checked = true;
                chkProfileEnergy.Checked = true;
                chkVIProfile.Checked = true;
                chkPhasorReport.Checked = true;

            }
            else
            {
                chkCumulativeEnergies.Checked = false;
                chkCumulativeTamperEvents.Checked = false;
                chkDailyEnergyConsumption.Checked = false;
                chkDailyMaximumData.Checked = false;
                chkInstantaneousData.Checked = false;
                chkProfileDemand.Checked = false;
                chkProfileEnergy.Checked = false;
                chkVIProfile.Checked = false;
                chkPhasorReport.Checked = false;
            }
        }

        // Added for getting all the values for Header
        #region
        // Added to list consumer meter details
        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }
        // Added to list file upload details and meter reading details
        private DataSet ListFileUploadAndMeterRead(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchFileUploadandMeterRead(activeMeterDataId);
        }
        // Added to get meterId
        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }
        // Added to list Nameplate details
        private DataSet ListNamePlateDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchNamePlateDetails(activeMeterDataId);
        }
        // Added to list header information details like auto reset date and time,power off days
        private DataSet ListHeaderInfo1(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchHeaderInfo1(activeMeterDataId);
        }
        // Added to list header information details like BCS software version.
        private DataSet ListHeaderInfo2(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchHeaderInfo2(activeMeterDataId);
        }
        // Added to list Kvah computation logic
        private DataSet ListKvahComputationLogic(long activeMeterDataId, long FileUploadID)
        {
            return new MeterDataBLL().FetchKvahComputation(activeMeterDataId, FileUploadID);
        }
        // Added to list Integration Period
        private DataSet ListIntegrationPeriod(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchIntegrationPeriod(activeMeterDataId);
        }
        // Added to list Last reset date and time
        private DataSet ListLastResetDateAndTime(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchLastResetDateAndTime(activeMeterDataId);
        }
        // Added to list Last reset date and time
        private DataSet ListNoLoadNoSupplyDuration(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchNoLOadNoSupplyDuration(activeMeterDataId);
        }
        // Added to list Detailed Tamper Data 
        private DataSet ListTamperDataTNEB(long activeMeterDataId)
        {
            return new ReportBLL().GetTamperReportDataTNEB(activeMeterDataId);
        }
        // Added to list Detailed Tamper Data in correct format
        private DataSet ListTamperOccResDataTNEB(DataSet ds)
        {
            return new BillingBLL().TamperOccurRestoreTNEB(ds);
        }
        // Added to list cumulative Energies data
        private DataSet ListCumulativeEnergies(long activeMeterDataId)
        {
            return new MeterDataBLL().FetchCumulativeEnergies(activeMeterDataId);
        }
        // Added to list billing data
        private DataSet ListBillingData(int activeMeterDataId)
        {
            return new BillingBLL().GetCumulativeEnergyTNEB(activeMeterDataId);
        }
        #endregion

        // Added for filling the report dataset tables columns.
        #region
        
        // Added to fill the report dataset table with consumer meter details.
        private void FillConsumerMeterDetails(DataSet Detailds)
        {
            DataRow reportrow;
            reportrow = reportXSD.Tables["ConsumerMeterDetails"].NewRow();
            if (Detailds.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow row in Detailds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportrow["Meter_Serial_No"] = row["MeterID"].ToString();
                    else
                        reportrow["Meter_Serial_No"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["Consumer_Name"].ToString()))
                        reportrow["Consumer_Name"] = row["Consumer_Name"].ToString();
                    else
                        reportrow["Consumer_Name"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                        reportrow["Consumer_Code"] = row["Consumer_Number"].ToString();
                    else
                        reportrow["Consumer_Code"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["Meter_Location"].ToString()))
                        reportrow["Location_Name"] = row["Meter_Location"].ToString();
                    else
                        reportrow["Location_Name"] = dateUnavailable;
                    reportXSD.Tables["ConsumerMeterDetails"].Rows.Add(reportrow);
                }

            }
           
        }
        // Added to fill the report dataset table with file uploading details.
        private void FillFileUpload(DataSet Detailds)
        {
            DataRow reportrow;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("MeterID", typeof(System.String)));
            table.Columns.Add(new DataColumn("UploadingDateTime", typeof(System.String)));
            table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("ReadingDateTime", typeof(System.String)));
            DataRow row1;
            row1 = table.NewRow();
            table.Rows.Add(row1);
            DataRow dataRow = Detailds.Tables[0].Rows[0];
            if (Detailds.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn col in Detailds.Tables[0].Columns)
                {

                    string val = Convert.ToString(dataRow[col.ColumnName]);
                    if (col.ColumnName.Equals("MeterID"))
                    {
                        table.Rows[0][0] = val;
                    }
                    if (col.ColumnName.Equals("UploadingDateTime"))
                    {
                        string s = "";
                        s = SplitWithOutDateUnit(val);

                        table.Rows[0][1] = s;

                    }
                    if (col.ColumnName.Equals("FileUpload_ID"))
                    {

                        table.Rows[0][2] = val;

                    }
                    if (col.ColumnName.Equals("ReadingDateTime"))
                    {
                        string s = "";
                        s = SplitWithOutDateUnit(val);
                        table.Rows[0][3] = s;

                    }

                }

            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);

            if (ds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["FileUploadDetails"].NewRow();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["UploadingDateTime"].ToString()))
                    {

                        reportrow["UploadingTime"] = row["UploadingDateTime"].ToString();
                    }
                    else
                        reportrow["UploadingTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["ReadingDateTime"].ToString()))
                        reportrow["ReadingDateandTime"] = row["ReadingDateTime"].ToString();
                    else
                        reportrow["ReadingDateandTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["FileUpload_ID"].ToString()))
                        reportrow["FileUpload_ID"] = row["FileUpload_ID"].ToString();
                    else
                        reportrow["FileUpload_ID"] = dateUnavailable;
                    reportXSD.Tables["FileUploadDetails"].Rows.Add(reportrow);
                }
            }



        }

        // Added to fill the report dataset table with Meter details.
        private void GetMeterIDFromMeterDataID(DataSet MeterIDds)
        {
            DataRow reportrow;
            if (MeterIDds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["ConsumerMeterDetails"].NewRow();
                foreach (DataRow row in MeterIDds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportrow["Meter_Serial_No"] = row["MeterID"].ToString();
                    else
                        reportrow["Meter_Serial_No"] = dateUnavailable;
                    reportrow["Consumer_Name"] = dateUnavailable;
                    reportrow["Consumer_Code"] = dateUnavailable;
                    reportrow["Location_Name"] = dateUnavailable;
                    reportXSD.Tables["ConsumerMeterDetails"].Rows.Add(reportrow);
                }
            }
        }
        // Added to fill the report dataset table with Nameplate details.
        private void FillNamePlateDetails(DataSet Detailds)
        {
            DataRow reportrow;
            if (Detailds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["NamePlateDetails"].NewRow();
                foreach (DataRow row in Detailds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterType"].ToString()))
                        reportrow["MeterType"] = row["MeterType"].ToString();
                    else
                        reportrow["MeterType"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["MeterConstant"].ToString()))
                        reportrow["MeterConstant"] = row["MeterConstant"].ToString();
                    else
                        reportrow["MeterConstant"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["CurrentRating"].ToString()))
                        reportrow["CurrentRating"] = row["CurrentRating"].ToString();
                    else
                        reportrow["CurrentRating"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VoltageRating"].ToString()))
                        reportrow["VoltageRating"] = row["VoltageRating"].ToString();
                    else
                        reportrow["VoltageRating"] = dateUnavailable;
                    reportXSD.Tables["NamePlateDetails"].Rows.Add(reportrow);
                }

            }


        }
        // Added to fill the report dataset table with Header information details like auto reset date and time,power off days.
        private void FillHeaderInfo1(DataSet detailds)
        {
            DataRow reportrow;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("MeterID", typeof(System.String)));
            table.Columns.Add(new DataColumn("PowerOffDays", typeof(System.String)));
            table.Columns.Add(new DataColumn("SoftwareVersion", typeof(System.String)));
            table.Columns.Add(new DataColumn("AutoResetDateAndTime", typeof(System.String)));
            table.Columns.Add(new DataColumn("DemandIntegrationPeriod(KW)", typeof(System.String)));
            table.Columns.Add(new DataColumn("DemandIntegrationPeriod(KVA)", typeof(System.String)));
            DataRow row1;
            row1 = table.NewRow();
            table.Rows.Add(row1);
            string s = "";
            DataRow dataRow = detailds.Tables[0].Rows[0];
            if (detailds.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn col in detailds.Tables[0].Columns)
                {

                    string val = Convert.ToString(dataRow[col.ColumnName]);
                    if (col.ColumnName.Equals("MeterID"))
                        table.Rows[0][0] = val;
                    if (col.ColumnName.Equals("PowerOffDays"))
                        table.Rows[0][1] = val;
                    if (col.ColumnName.Equals("SoftwareVersion"))

                        table.Rows[0][2] = val;
                    if (col.ColumnName.Equals("BillingDate") || col.ColumnName.Equals("BillingHour") || col.ColumnName.Equals("BillingMinute"))
                    {
                        s += val;

                    }
                    if (col.ColumnName.Equals("MD1KwTimeInterval"))
                        table.Rows[0][4] = val;
                    if (col.ColumnName.Equals("MD2KVATimeInterval"))
                        table.Rows[0][5] = val;

                }
                if (s != "")
                {
                    string s1 = "";
                    s1 = s.Substring(0, 2) + " ";
                    s1 += s.Substring(2, 2) + ":" + s.Substring(4, 2);
                    
                    table.Rows[0][3] = s1;
                }
                else table.Rows[0][3] = s;

            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);


            if (ds.Tables[0].Rows.Count >= 0)
            {
                reportrow = reportXSD.Tables["HeaderInfo1"].NewRow();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportrow["MeterID"] = row["MeterID"].ToString();
                    else
                        reportrow["MeterID"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["AutoResetDateAndTime"].ToString()))
                        reportrow["AutoResetDateAndTime"] = row["AutoResetDateAndTime"].ToString();
                    else
                        reportrow["AutoResetDateAndTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["PowerOffDays"].ToString()))
                        reportrow["PowerOffDays"] = row["PowerOffDays"].ToString();
                    else
                        reportrow["PowerOffDays"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["SoftwareVersion"].ToString()))
                        reportrow["SoftwareVersion"] = row["SoftwareVersion"].ToString();
                    else
                        reportrow["SoftwareVersion"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["DemandIntegrationPeriod(KW)"].ToString()))
                        reportrow["DemandIntegrationPeriod1(KW)"] = row["DemandIntegrationPeriod(KW)"].ToString();
                    else
                        reportrow["DemandIntegrationPeriod1(KW)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["DemandIntegrationPeriod(KVA)"].ToString()))
                        reportrow["DemandIntegrationPeriod(KVA)"] = row["DemandIntegrationPeriod(KVA)"].ToString();
                    else
                        reportrow["DemandIntegrationPeriod(KVA)"] = dateUnavailable;
                    reportXSD.Tables["HeaderInfo1"].Rows.Add(reportrow);
                }
            }

        }
        // Added to fill the report dataset table with Header information details like BCS software version.
        private void FillHeaderInfo2(DataSet detailds)
        {
            DataRow reportrow;
            if (detailds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["HeaderInfo2"].NewRow();
                foreach (DataRow row in detailds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MDResetCounter"].ToString()))
                        reportrow["NumberOfResets"] = row["MDResetCounter"].ToString().Trim('0');
                    else
                        reportrow["NumberOfResets"] = dateUnavailable;

                    reportrow["InternalCTRatio"] = dateUnavailable;
                    reportrow["InternalPTRatio"] = dateUnavailable;
                    reportrow["BCSSoftwareVersion"] = ConfigInfo.GetBCSVersion();
                    reportXSD.Tables["HeaderInfo2"].Rows.Add(reportrow);
                }


            }
            else
            {
                reportrow = reportXSD.Tables["HeaderInfo2"].NewRow();
                reportrow["NumberOfResets"] = dateUnavailable;

                reportrow["InternalCTRatio"] = dateUnavailable;
                reportrow["InternalPTRatio"] = dateUnavailable;
                reportrow["BCSSoftwareVersion"] = ConfigInfo.GetBCSVersion();
                reportXSD.Tables["HeaderInfo2"].Rows.Add(reportrow);
            }

        }

        // Added to fill the report dataset table with Header information details: NoLoad and NoSupply Duration.
        private void FillNoLoadNoSupplyDuration(DataSet detailds)
        {             
             DataRow reportrow = reportXSD.Tables["NoLoadNoSupplyDuration"].NewRow();
            if (detailds.Tables[0].Rows.Count > 0)
            {                
                foreach (DataRow row in detailds.Tables[0].Rows)
                {
                    reportrow["NoLoadDuration"] = row["NoLoadDuration"].ToString();   
                    reportrow["NoSupplyDuration"] = row["NoSupplyDuration"].ToString();                                             
                }
            }
            else
            {
                reportrow["NoLoadDuration"] =dateUnavailable;
                reportrow["NoSupplyDuration"] = dateUnavailable;                 
            }
            reportXSD.Tables["NoLoadNoSupplyDuration"].Rows.Add(reportrow);

        }
        // Added to fill the report dataset table with Kvah computation.
        private void FillKvahComputationLogic(DataSet Detailds)
        {
            DataRow reportrow;

            if (Detailds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["KvahComputatiolLogic"].NewRow();
                foreach (DataRow row in Detailds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["PFLogic"].ToString()))
                        reportrow["LAL"] = row["PFLogic"].ToString();
                    else
                        reportrow["LAL"] = dateUnavailable;
                    reportXSD.Tables["KvahComputatiolLogic"].Rows.Add(reportrow);
                }
            }
            else
            {
                reportrow = reportXSD.Tables["KvahComputatiolLogic"].NewRow();
                reportrow["LAL"] = dateUnavailable;
                reportXSD.Tables["KvahComputatiolLogic"].Rows.Add(reportrow);
            }
   
        }
        // Added to fill the report dataset table with Integration period.
        private void FillIntegrationPeriod(DataSet Detailsds)
        {
            DataRow reportrow;
            DataRow row1;
            DataRow datarow = Detailsds.Tables[0].Rows[0];
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("IntegrationPeriod", typeof(System.String)));
            row1 = table.NewRow();
            table.Rows.Add(row1);
            if (Detailsds.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn col in Detailsds.Tables[0].Columns)
                {
                    string val = Convert.ToString(datarow[col.ColumnName]);
                    if (col.ColumnName.Equals("MDIntervalPeriod") && val != "")
                    {

                        table.Rows[0][0] = val;

                    }
                    else
                        table.Rows[0][0] = val;

                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            if (ds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["IntegrationTable"].NewRow();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["IntegrationPeriod"].ToString()))
                        reportrow["IntegrationPeriod1"] = row["IntegrationPeriod"].ToString();
                    else
                        reportrow["IntegrationPeriod1"] = dateUnavailable;
                        reportXSD.Tables["IntegrationTable"].Rows.Add(reportrow);
                        
                }

                
            }

        }
        // Added to fill the report dataset table with Last reset date and time.
        private void FillLastResetDateAndTime(DataSet detailsds)
        {
            DataRow reportrow;
            DataRow row1;
            
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("LastResetDateandTime", typeof(System.String)));
            row1 = table.NewRow();
            table.Rows.Add(row1);
            if (detailsds.Tables[0].Rows.Count > 0)
            {
                DataRow datarow = detailsds.Tables[0].Rows[0];
                foreach (DataColumn col in detailsds.Tables[0].Columns)
                {
                    string val = Convert.ToString(datarow[col.ColumnName]);
                    if (col.ColumnName.Equals("BillingTimeStamp") && val != "")
                    {
                        string s = "";
                        s = SplitWithOutDateUnit(val);
                        table.Rows[0][0] = s;
                    }
                    else
                        table.Rows[0][0] = val;
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            if (ds.Tables[0].Rows.Count > 0)
            {
                reportrow = reportXSD.Tables["ForResetDateAndTime"].NewRow();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["LastResetDateandTime"].ToString()))
                        reportrow["LastResetDateAndTime"] = row["LastResetDateandTime"].ToString();
                    else
                        reportrow["LastResetDateAndTime"] = dateUnavailable;
                    reportXSD.Tables["ForResetDateAndTime"].Rows.Add(reportrow);

                }
            }

        }
        // Added to fill the report dataset table with Detailed Tamper details.
         private void FillDetailedTamperReport(DataSet detailsds)
        {
            DataRow reportrow;
            if (detailsds.Tables[0].Rows.Count > 0)
            {
                double tmpdl;
                foreach (DataRow row in detailsds.Tables[0].Rows)
                {
                    reportrow = reportXSD.Tables["DetailedTamperReport"].NewRow();
                    if (!string.IsNullOrEmpty(row["TamperType"].ToString()))
                        reportrow["TamperType"] = row["TamperType"].ToString();
                    else
                        reportrow["TamperType"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["EventOcc"].ToString()))
                        reportrow["EventOcc"] = row["EventOcc"].ToString();
                    else
                        reportrow["EventOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["EventRes"].ToString()))
                        reportrow["EventRes"] = row["EventRes"].ToString();
                    else
                        reportrow["EventRes"] = dateUnavailable;
                    DateTime temp=new DateTime();
                    if (!string.IsNullOrEmpty(row["OccTime"].ToString()) && row["OccTime"].ToString() != dateUnavailable)
                    {

                        if (cultureInfo.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "dd/MM/yyyy")
                                temp = Convert.ToDateTime(row["OccTime"].ToString(), britishDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row["OccTime"].ToString());
                        }
                        else if (cultureInfo.DateTimeFormat.ShortDatePattern == "MM/dd/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "MM/dd/yyyy")
                                temp = Convert.ToDateTime(row["OccTime"].ToString(), usDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row["OccTime"].ToString());
                        }
                        try
                        {
                            reportrow["OccTime"] = temp.Day.ToString() + "/" + temp.Month.ToString() + "/" + temp.Year.ToString() + " " + temp.ToString("H:mm");//Convert.ToDateTime(row["OccTime"].ToString(), britishDateTimeFormat).ToString("H:mm");
                        }
                        catch (Exception eeee)
                        {
                        }

                    }
                    else
                        reportrow["OccTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["ResTime"].ToString()) && row["ResTime"].ToString() != dateUnavailable)
                    {
                        if (cultureInfo.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "dd/MM/yyyy")
                                temp = Convert.ToDateTime(row["ResTime"].ToString(), britishDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row["ResTime"].ToString());
                        }
                        else if (cultureInfo.DateTimeFormat.ShortDatePattern == "MM/dd/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "MM/dd/yyyy")
                                temp = Convert.ToDateTime(row["ResTime"].ToString(), usDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row["ResTime"].ToString());
                        }
                        reportrow["ResTime"] = temp.Day.ToString() + "/" + temp.Month.ToString() + "/" + temp.Year.ToString() + " " + temp.ToString("H:mm");// Convert.ToDateTime(row["ResTime"].ToString(), britishDateTimeFormat).ToString("H:mm");

                    }
                    else
                        reportrow["ResTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["Duration"].ToString()))
                        reportrow["Duration"] = row["Duration"].ToString();
                    else
                        reportrow["Duration"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["VrOcc"].ToString()) && double.TryParse(row["VrOcc"].ToString(),out tmpdl))
                    {
                        if (row["VrOcc"].ToString() != dateUnavailable)
                        {
                            reportrow["VrOcc"] = Math.Round(tmpdl, 2);
                        }
                        else
                            reportrow["VrOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["VrOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VyOcc"].ToString()) && double.TryParse(row["VyOcc"].ToString(), out tmpdl))
                    {
                        if (row["VyOcc"].ToString() != dateUnavailable)
                        {
                            reportrow["VyOcc"] = Math.Round(tmpdl, 2);
                        }
                        else
                            reportrow["VyOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["VyOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VbOcc"].ToString()) && double.TryParse(row["VbOcc"].ToString(), out tmpdl))
                    {
                        if (row["VbOcc"].ToString() != dateUnavailable)
                            reportrow["VbOcc"] = Math.Round(tmpdl, 2);
                        else
                            reportrow["VbOcc"] = dateUnavailable;

                    }
                    else
                        reportrow["VbOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IrOcc"].ToString()) && double.TryParse(row["IrOcc"].ToString(), out tmpdl))
                    {
                        if (row["IrOcc"].ToString() != dateUnavailable)
                            reportrow["IrOcc"] = Math.Round(tmpdl, 3);
                        else
                            reportrow["IrOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["IrOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IyOcc"].ToString()) && double.TryParse(row["IyOcc"].ToString(), out tmpdl))
                    {
                        if (row["IyOcc"].ToString() != dateUnavailable)
                            reportrow["IyOcc"] = Math.Round(tmpdl, 3);
                        else
                            reportrow["IyOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["IyOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IbOcc"].ToString()) && double.TryParse(row["IbOcc"].ToString(), out tmpdl))
                    {
                        if (row["IbOcc"].ToString() != dateUnavailable)
                            reportrow["IbOcc"] = Math.Round(tmpdl, 3);
                        else
                            reportrow["IbOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["IbOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VrRes"].ToString()) && double.TryParse(row["VrRes"].ToString(), out tmpdl))
                    {
                        if (row["VrRes"].ToString() != dateUnavailable)
                        {
                            reportrow["VrRes"] = Math.Round(tmpdl, 2);
                        }
                        else
                            reportrow["VrRes"] = dateUnavailable;
                    }
                    else
                        reportrow["VrRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VyRes"].ToString()) && double.TryParse(row["VyRes"].ToString(), out tmpdl))
                    {
                        if (row["VyRes"].ToString() != dateUnavailable)
                        {
                            reportrow["VyRes"] = Math.Round(tmpdl, 2);
                        }
                        else
                            reportrow["VyRes"] = dateUnavailable;
                    }
                    else
                        reportrow["VyRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["VbRes"].ToString()) && double.TryParse(row["VbRes"].ToString(), out tmpdl))
                    {
                        if (row["VbRes"].ToString() != dateUnavailable)
                        {
                            reportrow["VbRes"] = Math.Round(tmpdl, 2);
                        }
                        else
                            reportrow["VbRes"] = dateUnavailable;
                    }
                    else
                        reportrow["VbRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IrRes"].ToString()) && double.TryParse(row["IrRes"].ToString(), out tmpdl))
                    {
                        if (row["IrRes"].ToString() != dateUnavailable)
                        {
                            reportrow["IrRes"] = Math.Round(tmpdl, 3);
                        }
                        else
                            reportrow["IrRes"] = dateUnavailable;
                    }
                    else
                        reportrow["IrRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IyRes"].ToString()) && double.TryParse(row["IyRes"].ToString(), out tmpdl))
                    {
                        if (row["IyRes"].ToString() != dateUnavailable)
                        {
                            reportrow["IyRes"] = Math.Round(tmpdl, 3);
                        }
                        else
                            reportrow["IyRes"] = dateUnavailable;

                    }
                    else
                        reportrow["IyRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["IbRes"].ToString()) && double.TryParse(row["IbRes"].ToString(), out tmpdl))
                    {
                        if (row["IbRes"].ToString() != dateUnavailable)
                        {
                            reportrow["IbRes"] = Math.Round(tmpdl, 3);
                        }
                        else
                            reportrow["IbRes"] = dateUnavailable;
                    }
                    else
                        reportrow["IbRes"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["KWHOcc"].ToString()) && double.TryParse(row["KWHOcc"].ToString(), out tmpdl))
                    {
                        if (row["KWHOcc"].ToString() != dateUnavailable)
                        {
                            reportrow["KWHOcc"] = Math.Round(tmpdl, 1);
                        }
                        else
                            reportrow["KWHOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["KWHOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["KVAHOcc"].ToString()) && double.TryParse(row["KVAHOcc"].ToString(), out tmpdl))
                    {
                        if (row["KVAHOcc"].ToString() != dateUnavailable)
                        {
                            reportrow["KVAHOcc"] = Math.Round(tmpdl, 1);
                        }
                        else
                            reportrow["KVAHOcc"] = dateUnavailable;
                    }
                    else
                        reportrow["KVAHOcc"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["KWRes"].ToString()) && double.TryParse(row["KWRes"].ToString(), out tmpdl))
                    {
                        if (row["KWRes"].ToString() != dateUnavailable)
                        {
                            reportrow["KWRes"] = Math.Round(tmpdl, 1);
                        }
                        else
                            reportrow["KWRes"] = dateUnavailable;
                    }
                    else
                        reportrow["KWRes"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row["KVAHRes"].ToString()) && double.TryParse(row["KVAHRes"].ToString(), out tmpdl))
                    {
                        if (row["KVAHRes"].ToString() != dateUnavailable)
                        {
                            reportrow["KVAHRes"] = Math.Round(tmpdl, 1);
                        }
                        else
                            reportrow["KVAHRes"] = dateUnavailable;
                    }
                    else
                        reportrow["KVAHRes"] = dateUnavailable;

                    reportXSD.Tables["DetailedTamperReport"].Rows.Add(reportrow);



                }
            }

        }
         // Added to fill the report dataset table with Reset Backups(Billing details).
        private void FillBillingReport(DataSet detailds)
         {
             DataRow reportrow;
             reportrow = reportXSD.Tables["BillingReportTNEB"].NewRow();
             double tmpdl;
            if (detailds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in detailds.Tables[0].Rows)
                {
                    reportrow = reportXSD.Tables["BillingReportTNEB"].NewRow();
                    if(!string.IsNullOrEmpty(row[0].ToString()))
                    {
                        reportrow["ResetNo"] = row[0].ToString();
                    }
                    else 
                    reportrow["ResetNo"] = dateUnavailable;
                    DateTime temp = new DateTime();  
                    if (!string.IsNullOrEmpty(row[1].ToString()))
                    {

                        if (cultureInfo.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "dd/MM/yyyy")
                                temp = Convert.ToDateTime(row[1].ToString(), britishDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[1].ToString());
                        }
                        else if (cultureInfo.DateTimeFormat.ShortDatePattern == "MM/dd/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "MM/dd/yyyy")
                                temp = Convert.ToDateTime(row[1].ToString(), usDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[1].ToString());
                        }
                        reportrow["ResetDateandTime"] = temp.Day.ToString() + "/" + temp.Month.ToString() + "/" + temp.Year.ToString() + " " + temp.ToString("H:mm");// Convert.ToDateTime(row[1].ToString(), britishDateTimeFormat).ToString("H:mm:ss");

                        //Convert.ToDateTime(row[1].ToString(),britishDateTimeFormat);
                    }
                    else
                        reportrow["ResetDateandTime"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[2].ToString()) && double.TryParse(row[2].ToString(), out tmpdl))
                    {
                        reportrow["CumkWh"] = Math.Round(tmpdl, 1);
                    }
                    else
                        reportrow["CumkWh"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[3].ToString()) && double.TryParse(row[3].ToString(), out tmpdl))
                    {
                        reportrow["CumkVAh"] = Math.Round(tmpdl, 1);
                    }
                    else
                        reportrow["CumkVAh"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[4].ToString()) && double.TryParse(row[4].ToString(), out tmpdl))
                    {
                        reportrow["CumkVARh(Lag)"] = Math.Round(tmpdl, 1);
                    }
                    else
                        reportrow["CumkVARh(Lag)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[5].ToString()) && double.TryParse(row[5].ToString(), out tmpdl))
                    {
                        reportrow["CumkVARh(Lead)"] = Math.Round(tmpdl, 1);
                    }
                    else
                        reportrow["CumkVARh(Lead)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[6].ToString()) && double.TryParse(row[6].ToString(), out tmpdl))
                    {
                        reportrow["KW(MD)"] = Math.Round(tmpdl, 2);
                    }
                    else
                        reportrow["KW(MD)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[7].ToString()) && row[7].ToString()!=dateUnavailable)
                    {

                        if (cultureInfo.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "dd/MM/yyyy")
                                temp = Convert.ToDateTime(row[7].ToString(), britishDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[7].ToString());
                        }
                        else if (cultureInfo.DateTimeFormat.ShortDatePattern == "MM/dd/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "MM/dd/yyyy")
                                temp = Convert.ToDateTime(row[7].ToString(), usDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[7].ToString());
                        }

                        reportrow["OccTime(KW)"] = temp.Day.ToString() + "/" + temp.Month.ToString() + "/" + temp.Year.ToString() + " " + temp.ToString("H:mm:ss"); //Convert.ToDateTime(row[7].ToString(), britishDateTimeFormat).ToString("H:mm:ss");
                        
                    }
                    else
                        reportrow["OccTime(KW)"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row[8].ToString()) && double.TryParse(row[8].ToString(), out tmpdl))
                    {
                        reportrow["KVA(MD)"] = Math.Round(tmpdl, 2);
                    }
                    else
                        reportrow["KVA(MD)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[9].ToString()) && row[9].ToString() != dateUnavailable)
                    {
                        if (cultureInfo.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "dd/MM/yyyy")
                                temp = Convert.ToDateTime(row[9].ToString(), britishDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[9].ToString());
                        }
                        else if (cultureInfo.DateTimeFormat.ShortDatePattern == "MM/dd/yyyy")
                        {
                            if (CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern != "MM/dd/yyyy")
                                temp = Convert.ToDateTime(row[9].ToString(), usDateTimeFormat);
                            else
                                temp = Convert.ToDateTime(row[9].ToString());
                        }

                        reportrow["OccTime(KVA)"] = temp.Day.ToString() + "/" + temp.Month.ToString() + "/" + temp.Year.ToString() + " " + temp.ToString("H:mm:ss");// Convert.ToDateTime(row[9].ToString(), britishDateTimeFormat).ToString("H:mm:ss");
                    }
                    else
                        reportrow["OccTime(KVA)"] = dateUnavailable;
                    if (!string.IsNullOrEmpty(row[10].ToString()) && double.TryParse(row[10].ToString(), out tmpdl))
                    {
                        reportrow["AveragePF"] = Math.Round(tmpdl, 4);
                    }
                    else
                        reportrow["AveragePF"] = dateUnavailable;
                    reportXSD.Tables["BillingReportTNEB"].Rows.Add(reportrow);

                }
            }
            
         }
        // Added to fill the report dataset table with Cumulative energies.
         private void FillCumulativeEnergy(DataSet detailds)
         {
             DataTable table = new DataTable();
             table.Columns.Add(new DataColumn("Date", typeof(System.String)));
             table.Columns.Add(new DataColumn("ActiveEnergy", typeof(System.String)));
             table.Columns.Add(new DataColumn("ApparentEnergy", typeof(System.String)));
             DataRow[] row1 = new DataRow[detailds.Tables[0].Rows.Count];
             if (detailds.Tables[0].Rows.Count > 0)
             {
                 for (int i = 0; i < detailds.Tables[0].Rows.Count; )
                 {

                     foreach (DataRow row in detailds.Tables[0].Rows)
                     {
                         row1[i] = table.NewRow();
                         foreach (DataColumn col in detailds.Tables[0].Columns)
                         {
                             string val = Convert.ToString(row[col.ColumnName]);
                             if (col.ColumnName.Equals("DailyProfileDate"))
                             {
                                 string s = "";
                                 s = SplitWithOutDateUnit(val);
                                 row1[i][0] = s;
                             }
                             if (col.ColumnName.Equals("Cumulativekwh"))
                             {
                                 row1[i][1] = val;
                             }
                             if (col.ColumnName.Equals("CumulativekVAh"))
                             {
                                 row1[i][2] = val;
                             }


                         }

                         table.Rows.Add(row1[i]);
                         i++;
                     }
                 }
             }
             DataSet ds = new DataSet();
             ds.Tables.Add(table);
             DataRow[] Reportrow = new DataRow[ds.Tables[0].Rows.Count]; ;
             if (ds.Tables[0].Rows.Count > 0)
             {
                 double tmpdbl; 
                 for (int i = 0; i < ds.Tables[0].Rows.Count; )
                 {
                     foreach (DataRow row in ds.Tables[0].Rows)
                     {
                         Reportrow[i] = reportXSD.Tables["CumulativeEnergiesReport"].NewRow();

                         if (!string.IsNullOrEmpty(row["Date"].ToString()))
                         {
                             Reportrow[i]["Date"] = row["Date"].ToString();
                             
                         }
                         else
                             Reportrow[i]["Date"] = dateUnavailable;
                         if (!string.IsNullOrEmpty(row["ActiveEnergy"].ToString()) && double.TryParse(row["ActiveEnergy"].ToString(),out tmpdbl))
                         {
                             Reportrow[i]["ActiveEnergy"] = Math.Round(tmpdbl, 1);
                         }
                         else
                             Reportrow[i]["ActiveEnergy"] = dateUnavailable;
                         if (!string.IsNullOrEmpty(row["ApparentEnergy"].ToString()) && double.TryParse(row["ApparentEnergy"].ToString(), out tmpdbl))
                         {
                             Reportrow[i]["ApparentEnergy"] = Math.Round(tmpdbl, 1);
                         }
                         else
                             Reportrow[i]["ApparentEnergy"] = dateUnavailable;

                         reportXSD.Tables["CumulativeEnergiesReport"].Rows.Add(Reportrow[i]);
                         i++;
                     }

                 }
             }

         }



        #endregion

        // This function is used to convert Date and time in proper format.
        public string SplitWithOutDateUnit(string data)
        {
            string value = "------";
            if (data == "0")
                return value;
            if (data == "")
                return value;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                if (val[1] == "YYYY/MM/DD :MM:SS")
                {
                    value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(val[0]));
                }
            }
            else if (data.IndexOf('-') > 0)
            {
                long dateval = Convert.ToInt64(DateUtility.StringToLongDateTimeFormat(data));
                if (dateval != 0)
                    value = DateUtility.LongToStringDateTimeFormat(dateval);
            }
            else
                value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(data));

            string[] temp = value.Split('/');
            value = "";
          
            if (cultureInfo.Name == "en-US")
            {

                value = temp[1].ToString() + "/" + temp[0].ToString() + "/" + temp[2].ToString();
            }
            else
            {//en-GB
                value = temp[0].ToString() + "/" + temp[1].ToString() + "/" + temp[2].ToString();
            }


            return value;
        }
        // This function is used to fill Header Details for the report
        private void FillHeaderDetails()
        {
            // Added for Consumer Meter Details
            DataSet detailsDS = new DataSet();
            DataSet meterIDDS = new DataSet();
            detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillConsumerMeterDetails(detailsDS);
                }
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    GetMeterIDFromMeterDataID(meterIDDS);

                }
            }
            //Added for file upload time and meter read date and time.
            detailsDS = new DataSet();
            detailsDS = ListFileUploadAndMeterRead(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillFileUpload(detailsDS);

                }
            }

            // Added for Name Plate Details.
            detailsDS = new DataSet();
            detailsDS = ListNamePlateDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillNamePlateDetails(detailsDS);

                }
            }

            // Added for Header Info 1.
            detailsDS = new DataSet();
            detailsDS = ListHeaderInfo1(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillHeaderInfo1(detailsDS);

                }
            }

            // Added for Header Info 2
            detailsDS = new DataSet();
            detailsDS = ListHeaderInfo2(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillHeaderInfo2(detailsDS);

                }
            }
            // Added for KvahCompuatationlogic
            detailsDS = new DataSet();
            detailsDS = ListKvahComputationLogic(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), Convert.ToInt64(ConfigInfo.FileUpload_ID));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {

                FillKvahComputationLogic(detailsDS);


            }
            // Added for Integration Period
            detailsDS = new DataSet();
            detailsDS = ListIntegrationPeriod(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                if (detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillIntegrationPeriod(detailsDS);
                }
            }
            // Added for Last Reset Date and Time
            detailsDS = new DataSet();
            detailsDS = ListLastResetDateAndTime(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                //    if (detailsDS.Tables[0].Rows.Count > 0)
                //    {
                FillLastResetDateAndTime(detailsDS);
                //}
            }

            // Added for NoLoad NoSupply Duration
            detailsDS = new DataSet();
            detailsDS = ListNoLoadNoSupplyDuration(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables.Count > 0)
            {
                FillNoLoadNoSupplyDuration(detailsDS);               
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void SelectDialogTNEB_Load(object sender, EventArgs e)
        {

        }
        // Added to show Landscape(detailed Tamper Report and Reset Backups) Reports
        public void ShowReportLandscape(string[] message)
        {
            ReportForm ObjRptForm = new ReportForm();
            DetailedTamperReport report = new DetailedTamperReport();
            string s = "";
            for (int i = 0; i < 2; i++)
            {
                if (!string.IsNullOrEmpty(message[i]))
                {
                    s += message[i] + "\n";
                }
            }
            if (!string.IsNullOrEmpty(s))
            {
                MessageBox.Show(s,resourceMgr.GetString("BCS"),MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (reportXSD.Tables["DetailedTamperReport"].Rows.Count == 0)
            {
                report.DetailedTamperReportTNEB.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["BillingReportTNEB"].Rows.Count == 0)
            {
                report.BillingReport.SectionFormat.EnableSuppress = true;
            }

            report.SetDataSource(reportXSD);
            report.Refresh();
            ObjRptForm.rptViewer.ReportSource = report;

            Cursor.Current = Cursors.Default;
            ObjRptForm.rptViewer.Zoom(1);
            this.Hide();
            ObjRptForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;

 
        }
        // Added to show detailed report1(Portrait)
        public void ShowReport(string[] message)
        {

            ReportForm ObjRptForm = new ReportForm();
            TNEBMasterReport masterreport = new TNEBMasterReport();
            string s = "";
            for (int i = 0; i < 9; i++)
            {
                if(!string.IsNullOrEmpty(message[i]))
                {
                    s += message[i] + "\n";
                }
            }
            if(!string.IsNullOrEmpty(s))
            {
                MessageBox.Show(s, resourceMgr.GetString("BCS"),MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            

            if (reportXSD.Tables["CumulativeEnergiesReport"].Rows.Count == 0)
            {
                masterreport.CumulativeEnergiesSection.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DailyMaxDemandDataTable"].Rows.Count == 0)
            {
                masterreport.DailyMDDataSection.SectionFormat.EnableSuppress = true;
            }
            //CumulativeTamperEventSection
            if (reportXSD.Tables["CumulativeTamperEventTable"].Rows.Count == 0)
            {
                masterreport.CumulativeTamperEventSection.SectionFormat.EnableSuppress = true;
            }
            //DailyEnergyConsumptionSection
            if (reportXSD.Tables["DailyEnergyConsumptionTable"].Rows.Count == 0)
            {
                masterreport.DailyEnergyConsumptionSection.SectionFormat.EnableSuppress = true;
            }
            //InstantDataSection
            if (reportXSD.Tables["InstantaneousDataTable"].Rows.Count == 0)
            {
                masterreport.InstantDataSection.SectionFormat.EnableSuppress = true;
            }
            
            //VIProfileSection
            if (reportXSD.Tables["VIProfileTable"].Rows.Count == 0)
            {
                masterreport.VIProfileSection.SectionFormat.EnableSuppress = true;
            }
            //DemandProfileSection
            if (reportXSD.Tables["DemandProfileTable"].Rows.Count == 0)
            {
                masterreport.DemandProfileSection.SectionFormat.EnableSuppress = true;
            }
            //EnergyProfileSection
            if (reportXSD.Tables["EnergyProfileTable"].Rows.Count == 0)
            {
                masterreport.EnergyProfileSection.SectionFormat.EnableSuppress = true;
            }
            if ( reportXSD.Tables["PhasorDiagramTable"].Rows.Count == 0)
            {
                masterreport.PhasorReportSection.SectionFormat.EnableSuppress = true;

            }
          
            masterreport.SetDataSource(reportXSD);
            masterreport.Refresh();
            ObjRptForm.rptViewer.ReportSource = masterreport;

            Cursor.Current = Cursors.Default;
            ObjRptForm.rptViewer.Zoom(1);
            this.Hide();
            ObjRptForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }

        private void SMD_btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.StatusMessage = "";
        }

        // Added for Landscape(detailed Tamper Report and Reset Backups) Reports
        private void btnDetailedShow_Click(object sender, EventArgs e)
        {
            reportXSD.Clear();
            int errCount = 0;
            int selectedParams = 0;
            int showReport = 0;
            string[] message = new string[2];
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                this.StatusMessage = resourceMgr.GetString("CAB File");
                return;
            }
            if (!chkBillingReport.Checked && !chkDetailedTamper.Checked)
            {

                this.StatusMessage = resourceMgr.GetString("Select");
                return;
            }
            else
                FillHeaderDetails();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (chkDetailedTamper.Checked)
                {
                    selectedParams++;
                    DataSet tamperDS = new DataSet();
                    DataSet tamperCounterDS = new DataSet();
                    DataSet tamperDetailsDset = new DataSet();
                    tamperCounterDS = ListTamperDataTNEB(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    tamperDS = ListTamperOccResDataTNEB(tamperCounterDS);
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("TamperType", typeof(System.String)));
                    table.Columns.Add(new DataColumn("EventOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("EventRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("OccTime", typeof(System.String)));
                    table.Columns.Add(new DataColumn("ResTime", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Duration", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VrOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VyOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VbOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IrOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IyOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IbOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VrRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VyRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("VbRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IrRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IyRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("IbRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("KWHOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("KVAHOcc", typeof(System.String)));
                    table.Columns.Add(new DataColumn("KWRes", typeof(System.String)));
                    table.Columns.Add(new DataColumn("KVAHRes", typeof(System.String)));
                    DataRow tablerow;
                    string tamperCounter = string.Empty;
                    string occurranceTime = string.Empty;
                    string restorationTime = string.Empty;
                    string tamperType = string.Empty;
                    if (tamperDS != null && tamperDS.Tables.Count > 0 && tamperDS.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow drow in tamperDS.Tables[0].Rows)
                        {
                            tablerow = table.NewRow();
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.VoltageImbalanceRPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("VIRP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.VoltageImbalanceYPhaseTamper)) 
                                tablerow[0] = resourceMgr.GetString("VIYP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.VoltageImbalanceBPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("VIBP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.MissingPotentialRPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("MPRP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.MissingPotentialYPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("MPYP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.MissingPotentialBPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("MPBP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CTShortTamper))
                                tablerow[0] = resourceMgr.GetString("CTST");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CTOpenRPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CTOR");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CTOpenYPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CTOY");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CTOpenBPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CTOB");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.OnePhaseNeutralAbsentTamper))
                                tablerow[0] = resourceMgr.GetString("OPNA");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.VoltagePhaseReversalTamper))
                                tablerow[0] = resourceMgr.GetString("VPRT");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentImbalanceRPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CIRP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentImbalanceYPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CIYP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentImbalanceBPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CIBP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentReversalRPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CRRP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentReversalYPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CRYP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.CurrentReversalBPhaseTamper))
                                tablerow[0] = resourceMgr.GetString("CRBP");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.MagneticInfluenceTamper))
                                tablerow[0] = resourceMgr.GetString("MGIT");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.NeutralDisturbanceTamper))
                                tablerow[0] = resourceMgr.GetString("NEDT");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.FrontCoverOpeningTamper))
                                tablerow[0] = resourceMgr.GetString("FCOT");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.TotalTamper))
                                tablerow[0] = resourceMgr.GetString("TOTT");
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.PowerOnOff))
                                tablerow[0] = resourceMgr.GetString("POOF");
                            tablerow[1] = resourceMgr.GetString("OCC");
                            tablerow[2] = resourceMgr.GetString("RES");
                            tablerow[3] = drow["TamperOccurredTime"];
                            tablerow[4] = drow["TamperRestoredTime"];
                            tablerow[5] = drow["Duration (Days:HH:MM)"];
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.PowerOnOff))
                            {
                                tablerow[6] = dateUnavailable;
                                tablerow[7] = dateUnavailable;
                                tablerow[8] = dateUnavailable;
                                tablerow[9] = dateUnavailable;
                                tablerow[10] = dateUnavailable;
                                tablerow[11] = dateUnavailable;
                                tablerow[12] = dateUnavailable;
                                tablerow[13] = dateUnavailable;
                                tablerow[14] = dateUnavailable;
                                tablerow[15] = dateUnavailable;
                                tablerow[16] = dateUnavailable;
                                tablerow[17] = dateUnavailable;
                                tablerow[18] = dateUnavailable;
                                tablerow[19] = dateUnavailable;
                                tablerow[20] = dateUnavailable;
                                tablerow[21] = dateUnavailable;
                            }
                            if (drow["TamperType"].ToString() == EnumUtil.StringValue(TamperType.FrontCoverOpeningTamper))
                            {
                                /* VBM - Display data unavailable in case of front cover open tamper */
                                tablerow[6] = dateUnavailable;
                                tablerow[7] = dateUnavailable;
                                tablerow[8] = dateUnavailable;
                                tablerow[9] = dateUnavailable;
                                tablerow[10] = dateUnavailable;
                                tablerow[11] = dateUnavailable;
                                tablerow[12] = dateUnavailable;
                                tablerow[13] = dateUnavailable;
                                tablerow[14] = dateUnavailable;
                                tablerow[15] = dateUnavailable;
                                tablerow[16] = dateUnavailable;
                                tablerow[17] = dateUnavailable;
                                tablerow[18] = dateUnavailable;
                                tablerow[19] = dateUnavailable;
                                tablerow[20] = dateUnavailable;
                                tablerow[21] = dateUnavailable;
                                /* VBM -  Display data unavailable in case of front cover open tamper */

                            }
                            if (drow["TamperType"].ToString() != EnumUtil.StringValue(TamperType.PowerOnOff) && drow["TamperType"].ToString() != EnumUtil.StringValue(TamperType.FrontCoverOpeningTamper))
                            {

                                tablerow[6] = drow["RVoltageOccurred"];
                                tablerow[7] = drow["YVoltageOccurred"];
                                tablerow[8] = drow["BVoltageOccurred"];
                                tablerow[9] = drow["RCurrentOccurred"];
                                tablerow[10] = drow["YCurrentOccurred"];
                                tablerow[11] = drow["BCurrentOccurred"];
                                tablerow[12] = drow["RVoltageRestored"];
                                tablerow[13] = drow["YVoltageRestored"];
                                tablerow[14] = drow["BVoltageRestored"];
                                tablerow[15] = drow["RCurrentRestored"];
                                tablerow[16] = drow["YCurrentRestored"];
                                tablerow[17] = drow["BCurrentRestored"];
                                tablerow[18] = drow["kWhOccurred"];
                                tablerow[19] = drow["kVAhOccurred"];
                                tablerow[20] = drow["kWhRestored"];
                                tablerow[21] = drow["kVAhRestored"];
                            }


                            table.Rows.Add(tablerow);

                        }
                        DataSet ds = new DataSet();
                        ds.Tables.Add(table);
                        FillDetailedTamperReport(ds);
                        showReport++;
                    }
                    else
                    {
                        errCount++;
                        message[0] = resourceMgr.GetString("Detailed Tamper");
                    }

                }  

                if (chkBillingReport.Checked)
                {
                    DataSet Billingds = new DataSet();
                    selectedParams++;
                    
                        Billingds = ListBillingData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                        if (Billingds != null && Billingds.Tables.Count > 0 && Billingds.Tables[0].Rows.Count > 0)
                        {
                        FillBillingReport(Billingds);
                        showReport++;
                        }
                    else
                    {
                        errCount++;
                        message[1] = resourceMgr.GetString("Reset Backups");
                    }

                }
                
                if (errCount == 0)
                    ShowReportLandscape(message);
                else
                {
                    if (errCount == selectedParams)
                    { MessageBox.Show(resourceMgr.GetString("No Data"), resourceMgr.GetString("BCS"), MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    if (showReport > 0)
                    { ShowReportLandscape(message); }
                }

            }
            catch
            {
                MessageBox.Show(resourceMgr.GetString("Error"),resourceMgr.GetString("BCS"),MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.StatusMessage = "";
            }
        }

        private void btnDetailedCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.StatusMessage = "";
        }
        // Added for validations.
        private void chkCumulativeEnergies_CheckedChanged(object sender, EventArgs e)
        {
            SMD_SelectAll.CheckedChanged -= SMD_SelectAll_CheckedChanged;
            if (chkVIProfile.Checked == true && chkProfileEnergy.Checked == true && chkProfileDemand.Checked == true
                && chkInstantaneousData.Checked == true && chkDailyMaximumData.Checked == true && chkDailyEnergyConsumption.Checked == true
                && chkCumulativeTamperEvents.Checked == true && chkCumulativeEnergies.Checked == true && chkPhasorReport.Checked ==true)
                SMD_SelectAll.Checked = true;
            else
                SMD_SelectAll.Checked = false;
            SMD_SelectAll.CheckedChanged += SMD_SelectAll_CheckedChanged;
        }

        private void chkCumulativeTamperEvents_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkDailyEnergyConsumption_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkDailyMaximumData_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkInstantaneousData_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkVIProfile_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkProfileDemand_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkProfileEnergy_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

        private void chkPhasorReport_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeEnergies_CheckedChanged(sender, e);
        }

  
    }
}
