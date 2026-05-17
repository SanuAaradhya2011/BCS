using System;
using System.Data;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using System.Windows.Forms;
using Utilities;
using System.Text;
using System.Collections;
using System.Linq;
using Hunt.EPIC.Logging;



namespace CAB.BLL
{
    public class DLMS650CommonBLL : IBLL
    {
        private string meterType; //pgvcl
        private string meterModelNo; //pgvcl
        private int mdInterval;
        Dictionary<string, string> reportColumns = new Dictionary<string, string>();
        MeterMasterEntity meterMasterEntity = null;
        LoadSurveyEntity loadSurveyEntity = new LoadSurveyEntity();
        MidnightParameterBLL midnightParameterBLL = new MidnightParameterBLL();
        DLMS650NamePlateDetailsEntity generalEntity = new DLMS650NamePlateDetailsEntity();//PGVCL
        bool integrationPeriodStatus = false;
        private bool isMVVNL = false;
        private bool isPUMA = false;
        private bool isMPKWCL = false;

        //// SB Change Start 20170901
        //private Random random = new Random();
        ////int MeterModelNoForTmp = -1;
        //// SB Change End 20170901

        //private const string CUMPOWEROFFDURATION = "Cumulative Power-Failure Duration (0.0.94.91.8.255;3;2) (YY:MM:DDD HH:MM)";
        private const string CUMTAMPERCOUNT = "Cumulative Tamper Count (0.0.94.91.0.255;1;2)";
        private const string DELTATAMPERCOUNT = "Tamper Count (0.0.96.2.190.255;1;2)"; // Story - 345154
        private const string CUMPOWERFAILURECOUNT = "Cumulative Power-Failure Count (0.0.96.7.0.255;1;2)";
        private const string CUMBILLINGMDRESETCOUNT = "Cumulative Billing Count (0.0.0.1.0.255;1;2)";
        private const string DATEFORMAT = "YY:MM:DDD:HH:MM";
       // private const string DATEFORMATFORINSCUMPOWFAIL = "dd : hh : mm";

        private const string DATEFORMATFORINSCUMPOWFAIL = "dddd : hh : mm";


        private const string DATEFORMATFORELAPSEDTIME = "MM:SS";
        private const string CUMPOWERFAILUREFORINSTANT = "Cumulative Power-Failure Duration";
        private const string CUMPOWERONFORINSTANT = "Cumulative Power-On Duration";
        private const string CUMPOTENTIALFAILRPHASE = "Cumulative Period Of Potential Fail - R Phase";
        private const string CUMPOTENTIALFAILYPHASE = "Cumulative Period Of Potential Fail - Y Phase";
        private const string CUMPOTENTIALFAILBPHASE = "Cumulative Period Of Potential Fail - B Phase";

        private const string CUMCURRENTFAILRPHASE = "Cumulative Period Of Current Fail - R Phase";
        private const string CUMCURRENTFAILYPHASE = "Cumulative Period Of Current Fail - Y Phase";
        private const string CUMCURRENTFAILBPHASE = "Cumulative Period Of Current Fail - B Phase";

        private const string HISTORY = "History";
        private const string CURRENT = "Current";
        private const string DAILYKWH = "Daily Consumption - kWh Import(1.0.1.8.0.255;3;2)";//pks
        private const string DAILYKVAH = "Daily Consumption - KVAh Import(1.0.9.8.0.255;3;2)";//pksS
        private const string DAILYKVARHLAG = "Daily Consumption - kvarh Lag(1.0.5.8.0.255;3;2)";
        private const string DAILYKVARHLEAD = "Daily Consumption - kvarh Lead(1.0.8.8.0.255;3;2)";
        private const string MIDNIGHTDATE = "Date";
        private decimal ROLLOVERVALUE = -1;
        private const string Incorrect = "Incorrect";
        private const string DATEFORMATFORINSCUMPOWFAIL_DDDDHH = "dddd : hh";
        public string Meter_cat;
        private const string ABCCodeBilling = "ABC Code(0.0.96.2.196.255;1;2)";
        private const string CumulativeMDkw = "Cumulative MD kw(1.0.1.2.0.255;3;2)";
        private const string CumulativeMDkva = "Cumulative MD kva(1.0.9.2.0.255;3;2)";

        private const string CumulativeMDkwExport = "Cumulative MD kw Export(1.0.2.2.0.255;3;2)";
        private const string CumulativeMDkvaExport = "Cumulative MD kva Export(1.0.10.2.0.255;3;2)";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650CommonBLL).ToString());

        public DLMS650CommonBLL()
        {
            if (UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
            {
                isMPKWCL = true;
            }
            else
            {
                isPUMA = false;
                isMVVNL = false;
                isMPKWCL = false;
            }
        }

        public bool IntegrationPeriodStatus
        {
            get { return integrationPeriodStatus; }
            set { integrationPeriodStatus = value; }
        }
        public int MDInterval
        {
            get
            {
                return mdInterval;
            }
            set
            {
                mdInterval = value;
            }
        }
        private string[] PhasorColumnValues650(bool flag)
        {
            string[] array = new string[9];
            if (flag)
                array = new string[12];
            array[0] = "R Phase Current";
            array[1] = "Y Phase Current";
            array[2] = "B Phase Current";
            array[3] = "R Phase Voltage";
            array[4] = "Y Phase Voltage";
            array[5] = "B Phase Voltage";
            array[6] = "R Phase PF";
            array[7] = "Y Phase PF";
            array[8] = "B Phase PF";
            if (flag)
            {
                array[9] = "R Phase Lag/Lead";
                array[10] = "Y Phase Lag/Lead";
                array[11] = "B Phase Lag/Lead";
            }
            return array;
        }
        public DataSet DisplayPhasor650(DataSet dataSet, bool flag)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;

            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameters", typeof(System.String)));
            table.Columns.Add(new DataColumn("Values", typeof(System.String)));

            string[] phasorColumn = PhasorColumnValues650(flag);
            DataRow dataRow;
            for (int col = 0; col < phasorColumn.Length; col++)
            {
                dataRow = table.NewRow();
                dataRow[0] = phasorColumn[col];
                table.Rows.Add(dataRow);
            }
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                if (Convert.ToString(row[0]).Equals("Current - IR"))
                    table.Rows[0][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Current - IY"))
                    table.Rows[1][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Current - IB"))
                    table.Rows[2][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Voltage - VRN"))
                    table.Rows[3][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Voltage - VYN"))
                    table.Rows[4][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Voltage - VBN"))
                    table.Rows[5][1] = Convert.ToString(row[4]);
                else if (Convert.ToString(row[0]).Equals("Signed Power Factor - R phase"))
                {
                    table.Rows[6][1] = Convert.ToString(row[4]);
                    if (flag)
                    {
                        float val = float.Parse(Convert.ToString(row[4]));
                        if (val < 0)
                            table.Rows[9][1] = "Lead";
                        else
                            table.Rows[9][1] = "Lag";
                    }
                }
                else if (Convert.ToString(row[0]).Equals("Signed Power Factor - Y phase"))
                {
                    table.Rows[7][1] = Convert.ToString(row[4]);
                    if (flag)
                    {
                        float val = float.Parse(Convert.ToString(row[4]));
                        if (val < 0)
                            table.Rows[10][1] = "Lead";
                        else
                            table.Rows[10][1] = "Lag";
                    }
                }
                else if (Convert.ToString(row[0]).Equals("Signed Power Factor - B phase"))
                {
                    table.Rows[8][1] = Convert.ToString(row[4]);
                    if (flag)
                    {
                        float val = float.Parse(Convert.ToString(row[4]));
                        if (val < 0)
                            table.Rows[11][1] = "Lead";
                        else
                            table.Rows[11][1] = "Lag";
                    }
                }
            }
            DataSet dset = new DataSet();
            dset.Tables.Add(table);
            return dset;
        }


        /// <summary>
        /// Returns Dictionary containing mapping of DB Column and Display Column.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetPhasorDisplayParameter()
        {
            Dictionary<string, string> dicPhasorParam = new Dictionary<string, string>();
            dicPhasorParam.Add("RPhaseCurrent", "R Phase Current(A)");
            dicPhasorParam.Add("YPhaseCurrent", "Y Phase Current(A)");
            dicPhasorParam.Add("BPhaseCurrent", "B Phase Current(A)");
            dicPhasorParam.Add("RPhaseVoltage", "R Phase Voltage(V)");
            dicPhasorParam.Add("YPhaseVoltage", "Y Phase Voltage(V)");
            dicPhasorParam.Add("BPhaseVoltage", "B Phase Voltage(V)");
            dicPhasorParam.Add("RPhasePowerFactor", "R Phase Power Factor");
            dicPhasorParam.Add("YPhasePowerFactor", "Y Phase Power Factor");
            dicPhasorParam.Add("BPhasePowerFactor", "B Phase Power Factor");
            dicPhasorParam.Add("TotalPhasePowerFactor", "Total Phase Power Factor");
            dicPhasorParam.Add("Frequency", "Frequency(Hz)");
            dicPhasorParam.Add("ApparentPower", CommonMethods.getDisplayHeaderText("Apparent Power({0}VA)"));
            dicPhasorParam.Add("ActivePower", CommonMethods.getDisplayHeaderText("Active Power({0}W)"));
            dicPhasorParam.Add("ReactivePower", CommonMethods.getDisplayHeaderText("Reactive Power({0}var)"));
            dicPhasorParam.Add("RPhaseNegativePowerFlag", CommonMethods.getDisplayHeaderText("R Phase {0}W Direction"));
            dicPhasorParam.Add("YPhaseNegativePowerFlag", CommonMethods.getDisplayHeaderText("Y Phase {0}W Direction"));
            dicPhasorParam.Add("BPhaseNegativePowerFlag", CommonMethods.getDisplayHeaderText("B Phase {0}W Direction"));
            dicPhasorParam.Add("RPhaseCapacitiveInductiveFlag", "R Phase Lag/Lead");
            dicPhasorParam.Add("YPhaseCapacitiveInductiveFlag", "Y Phase Lag/Lead");
            dicPhasorParam.Add("BPhaseCapacitiveInductiveFlag", "B Phase Lag/Lead");
            dicPhasorParam.Add("AngleYR", "Angle Y - R");
            dicPhasorParam.Add("AngleBR", "Angle B - R");
            dicPhasorParam.Add("AngleBetweenTwo", "Angle Between Two");
            dicPhasorParam.Add("RPhaseChannel", "R Phase Channel");
            dicPhasorParam.Add("BPhaseChannel", "B Phase Channel");
            dicPhasorParam.Add("YPhaseChannel", "Y Phase Channel");
            dicPhasorParam.Add("PhaseSequence", "Phase Sequence");
            return dicPhasorParam;
        }

        /// <summary>
        /// Returns Dataset by converting Columns to Rows for passed Dataset
        /// </summary>
        /// <param name="dsPhasor"></param>
        /// <returns></returns>
        public DataSet GetPhasorColumnToRow(DataSet dsPhasor, long meterDataID)
        {
            DataSet objDs = new DataSet();
            DataTable dtPhasor = new DataTable();
            dtPhasor.Columns.Add(new DataColumn("Parameters", typeof(System.String)));
            dtPhasor.Columns.Add(new DataColumn("Values", typeof(System.String)));

            if (dsPhasor != null && dsPhasor.Tables != null && dsPhasor.Tables[0].Rows.Count > 0)
            {
                Dictionary<string, string> dicPhasorColumns = GetPhasorDisplayParameter();

                DataRow drRow = dsPhasor.Tables[0].Rows[0];
                DataRow drNewItem = null;
                foreach (DataColumn col in dsPhasor.Tables[0].Columns)
                {
                    if (dicPhasorColumns.ContainsKey(col.ColumnName))
                    {
                        drNewItem = dtPhasor.NewRow();
                        drNewItem[0] = dicPhasorColumns[col.ColumnName];
                        if (Convert.ToString(drRow[col.ColumnName]).ToUpper() == "INVALID")
                            drNewItem[1] = Incorrect;
                        else
                            drNewItem[1] = Convert.ToString(drRow[col.ColumnName]);
                        dtPhasor.Rows.Add(drNewItem);
                    }
                }
            }
            objDs.Tables.Add(dtPhasor);
            ApplyMultiplyFactor(meterDataID, objDs, "Parameters", "Values");

            return objDs;
        }


        public string[] CheckUnit(string val)
        {
            string[] unit = new string[2];
            unit[0] = unit[1] = string.Empty;
            if (string.IsNullOrEmpty(val))
                return unit;
            string[] data = val.Split('*');
            unit[0] = data[0];
            if (data.Length == 2)
                unit[1] = data[1];
            return unit;
        }
        private string[] CheckPUMAUnit(string val)
        {
            string[] unit = new string[2];
            unit[0] = unit[1] = string.Empty;
            if (val.Contains("*"))
            {
                if (string.IsNullOrEmpty(val))
                    return unit;
                string[] data = val.Split('*');
                unit[0] = data[0];
                if (data.Length == 2)
                    unit[1] = data[1];
            }
            return unit;
        }

        private string getTransactionCol(string eventCode)
        {
            int num;
            try
            {
                num = Int32.Parse(eventCode);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "getTransactionCol(string eventCode)", ex);
                num = 0;
            }
            switch (num)
            {
                case 151:
                    return "Real Time Clock - Date and Time";
                case 152:
                    return "Demand Integration Period";
                case 153:
                    return "Profile Capture Period";
                case 154:
                    return "Single-Action Schedule for Billing Dates";
                case 155:
                    return "Activity Calendar for Time Zones etc";
                default:
                    return string.Empty;
            }
        }
        public DataSet Transaction(long meterDataID)
        {
            int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataID.ToString());
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameter Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("EventCode", typeof(System.String)));
            table.Columns.Add(new DataColumn("Event Date Time (0.0.1.0.0.255;8;2)", typeof(System.String)));    //SarkarA code change 20180405 //fix attribute
            table.Columns.Add(new DataColumn("Current - IR A (1.0.31.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Current - IY A (1.0.51.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Current - IB A (1.0.71.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Voltage - VRN V (1.0.32.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Voltage - VYN V (1.0.52.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Voltage - VBN V (1.0.72.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Power Factor - R phase (1.0.33.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Power Factor - Y phase (1.0.53.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Power Factor - B phase (1.0.73.7.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import (1.0.1.8.0.255;3;2)"), typeof(System.String)));//pks
            //VBM - Apparant energy .
            if (UtilityDetails.ShowApparantEnergyInTamper)
            {
                if (meterModelNo == NamePlateConstants.PumaLTE650Value || meterModelNo == NamePlateConstants.PumaHTE650Value)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Import(1.0.9.8.0.255;3;2)"), typeof(System.String)));//pks
                }
            }
            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
            DataSet dataSet = tamperBLL.DetailTransactionData(meterDataID);
            //int emf = new MeterMasterBLL().GetEMF(meterDataID);
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataRow row;
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                row = table.NewRow();
                row[0] = Convert.ToString(dr[12]);
                row[1] = Convert.ToString(dr[0]);
                row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[1]));
                row[3] = CheckUnit(Convert.ToString(dr[2]))[0];
                row[4] = CheckUnit(Convert.ToString(dr[3]))[0];
                row[5] = CheckUnit(Convert.ToString(dr[4]))[0];
                row[6] = CheckUnit(Convert.ToString(dr[5]))[0];
                row[7] = CheckUnit(Convert.ToString(dr[6]))[0];
                row[8] = CheckUnit(Convert.ToString(dr[7]))[0];
                row[9] = CheckUnit(Convert.ToString(dr[8]))[0];
                row[10] = CheckUnit(Convert.ToString(dr[9]))[0];
                row[11] = CheckUnit(Convert.ToString(dr[10]))[0];
                row[12] = CheckUnit(Convert.ToString(dr[11]))[0];
                //VBM - apparant energy.
                if (UtilityDetails.ShowApparantEnergyInTamper)
                {
                    if (meterModelNo == NamePlateConstants.PumaLTE650Value || meterModelNo == NamePlateConstants.PumaHTE650Value)
                    {
                        row[13] = (decimal.Parse(CheckUnit(Convert.ToString(dr[13]))[0])).ToString();
                    }
                }
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, dataSet);
        }

        public DataSet GetTamperOccurRestoreDetailColumnWise(long tamperId, long meterDataID, string tamperColumnNames)
        {
            string defaultValue = "----";
            string[] tamperColumns = new string[30];
            tamperColumns = tamperColumnNames.Split(',');
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameter Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("OBIS Code", typeof(System.String)));
            table.Columns.Add(new DataColumn("Class ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
            DataSet dataSet = tamperBLL.DetailDataColumnWise(tamperId, meterDataID, tamperColumnNames);
            int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataID.ToString());
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataRow row = dataSet.Tables[0].Rows[0];
            DataRow tableRow = table.NewRow();
            string[] val = new string[30];
            CommonMethods.MeterType = getMeterType();
            for (int counter = 0; counter < tamperColumns.Length; counter++)
            {
                if (tamperColumns[counter].Contains("High")) // Story - 349654 - Neutral current in Tamper NeutralCurrent
                {

                    // Specific Check to handle null table row
                    // Date : 24-July-17
                    // Done By: Mohsin Raza
                    //tableRow = table.NewRow();
                    //tableRow[0] = "High Neutral Current";
                    //tableRow[1] = "0.0.96.11.0.255";
                    //tableRow[2] = string.Empty;
                    //tableRow[3] = string.Empty;
                    //val = CheckUnit(Convert.ToString(row["HighNeutralCurrent"]));
                    //tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    //tableRow[5] = val[1];
                    //table.Rows.Add(tableRow);
                }
                else

                if (tamperColumns[counter].Contains("IR"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Current - IR";
                    tableRow[1] = "1.0.31.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CurrentIR"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                    if (tamperColumns[counter].Contains("IY"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Current - IY";
                    tableRow[1] = "1.0.51.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CurrentIY"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                        if (tamperColumns[counter].Contains("IB"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Current - IB";
                    tableRow[1] = "1.0.71.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CurrentIB"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                            if (tamperColumns[counter].Equals("PhaseCurrent"))  //SarkarA code change start 20180330 // add phase current instant, frequency
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Metering Current"; //Phase Current name change because of common nomenclature of Phase current for 1Phase meter tamper snapshot as “Metering Current”. User story no 474879 
                    tableRow[1] = "1.0.94.91.14.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PhaseCurrent"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                //SarkarA code change start 20180330 // add phase current instant, frequency
                else
                                if (tamperColumns[counter].Contains("PhaseCurrentInstant"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Phase Current";
                    tableRow[1] = "1.0.11.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PhaseCurrentInstant"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                //SarkarA code change end 20180330
                else

                                if (tamperColumns[counter].Contains("NeutralCurrent")) //pradipta_neu
                {
                    //SarkarA code change start 20180122 // Remove check for 1PH meter
                    //if (!CommonMethods.MeterType.Contains("1P-2W"))
                    //{  
                    tableRow = table.NewRow();//commet by sahoo
                    tableRow[0] = "Neutral Current";
                    tableRow[1] = "1.0.91.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["NeutralCurrent"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                    //}
                    //SarkarA code change end 20180122
                }
                else

                 if (tamperColumns[counter].Contains("ByPassCurrent")) 
                {

                    tableRow = table.NewRow();//commet by sahoo
                    tableRow[0] = "ByPass Current";
                    tableRow[1] = "1.0.11.7.128.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["ByPassCurrent"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }


                else
                if (tamperColumns[counter].Contains("VRN"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Voltage - VRN";
                    tableRow[1] = "1.0.32.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["VoltageVRN"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                        if (tamperColumns[counter].Contains("VYN"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Voltage - VYN";
                    tableRow[1] = "1.0.52.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["VoltageVYN"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                            if (tamperColumns[counter].Contains("VBN"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Voltage - VBN";
                    tableRow[1] = "1.0.72.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["VoltageVBN"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                if (tamperColumns[counter].Contains("PhaseVoltage"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Phase Voltage";
                    tableRow[1] = "1.0.12.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PhaseVoltage"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                //SarkarA code change start 20180330 // add phase current instant, frequency
                else
                                                    if (tamperColumns[counter].Contains("Frequency"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Frequency";
                    tableRow[1] = "1.0.14.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["Frequency"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                //SarkarA code change end 20180330
                else
                                                    if (tamperColumns[counter].Contains("Rphase"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Power Factor - R phase";
                    tableRow[1] = "1.0.33.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PowerFactorRphase"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                        if (tamperColumns[counter].Contains("Yphase"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Power Factor - Y phase";
                    tableRow[1] = "1.0.53.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PowerFactorYphase"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                            if (tamperColumns[counter].Contains("Bphase"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Power Factor - B phase";
                    tableRow[1] = "1.0.73.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["PowerFactorBphase"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                if (tamperColumns[counter].Contains("TotalPowerFactor"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Total Power Factor"));
                    tableRow[1] = "1.0.13.7.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["TotalPowerFactor"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                    if (tamperColumns[counter].Contains("WhExport"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh (Export)"));
                    tableRow[1] = "1.0.2.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykWhExport"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                        if (tamperColumns[counter].Contains("VAhExport"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh (Export)"));
                    tableRow[1] = "1.0.10.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykVAhExport"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVAh" : val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                            if (tamperColumns[counter].Contains("WhImport"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh (Forward)"));
                    tableRow[1] = "1.0.143.128.128.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykWhImport"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                                if (tamperColumns[counter].Contains("VAhImport"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh (Forward)"));
                    tableRow[1] = "1.0.144.128.128.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykVAhImport"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVAh" : val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                    if (tamperColumns[counter].Contains("Wh"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh"));
                    tableRow[1] = "1.0.1.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykWh"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                        if (tamperColumns[counter].Contains("VAh"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh"));
                    tableRow[1] = "1.0.9.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykVAh"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVAh" : val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                            if (tamperColumns[counter].Contains("kvarhLag"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh-Lag"));
                    tableRow[1] = "1.0.5.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykvarhLag"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kvarhLag" : val[1];
                    table.Rows.Add(tableRow);
                }
                else
                                                                                if (tamperColumns[counter].Contains("kvarhLead"))
                {
                    tableRow = table.NewRow();
                    tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh-Lead"));
                    tableRow[1] = "1.0.8.8.0.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeEnergykvarhLead"]));
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kvarhLead" : val[1];
                    table.Rows.Add(tableRow);
                }
                // SB Code Change Start 20171116
                else
                                                                                    if (tamperColumns[counter].Contains("ActiveCurrentR"))
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {
                        tableRow = table.NewRow();
                        tableRow[0] = "Active Current - R";
                        tableRow[1] = "1.0.31.7.128.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["ActiveCurrentR"]));
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "A" : val[1];
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                        if (tamperColumns[counter].Contains("ActiveCurrentY"))
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))   //SarkarA Code Change 20180109// Added missing !condition
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Active Current - Y";
                        tableRow[1] = "1.0.51.7.128.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["ActiveCurrentY"]));
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "A" : val[1];
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                            if (tamperColumns[counter].Contains("ActiveCurrentB"))
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {
                        tableRow = table.NewRow();
                        tableRow[0] = "Active Current - B";
                        tableRow[1] = "1.0.71.7.128.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["ActiveCurrentB"]));
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "A" : val[1];
                        table.Rows.Add(tableRow);
                    }
                }
                else

                                                                                                if (tamperColumns[counter].Contains("kWr"))//praadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {


                        tableRow = table.NewRow();
                        tableRow[0] = "Active Power R";
                        tableRow[1] = "1.1.24.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kWr"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kW" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();


                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                                    if (tamperColumns[counter].Contains("kWy"))//praadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Active Power Y";
                        tableRow[1] = "1.1.56.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kWy"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kW" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                                        if (tamperColumns[counter].Contains("kWb"))//raadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Active Power B";
                        tableRow[1] = "1.1.76.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kWb"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kW" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                                            if (tamperColumns[counter].Contains("kVAr"))//praadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Apparent Power R";
                        tableRow[1] = "1.0.23.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kVAr"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVA" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                                                if (tamperColumns[counter].Contains("kVAy"))//praadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Apparent Power Y";
                        tableRow[1] = "1.0.43.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kVAy"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVA" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        table.Rows.Add(tableRow);
                    }
                }
                else
                                                                                                                    if (tamperColumns[counter].Contains("kVAb"))//praadipta
                {
                    if (!CommonMethods.MeterType.Contains("1P-2W"))
                    {

                        tableRow = table.NewRow();
                        tableRow[0] = "Apparent Power B";
                        tableRow[1] = "1.0.63.7.0.255";
                        tableRow[2] = "3";
                        tableRow[3] = "2";
                        val = CheckUnit(Convert.ToString(row["kVAb"]));
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVA" : val[1];
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                        table.Rows.Add(tableRow);
                    }
                }

                else
                                                                                                                if (tamperColumns[counter].Contains("CumulativeTamperCount"))//smart meter
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Cumulative Tamper Count";
                    tableRow[1] = "0.0.94.91.0.255";
                    tableRow[2] = "1";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["CumulativeTamperCount"]));
                    tableRow[5] = string.IsNullOrEmpty(val[1]) ? "" : val[1];
                    tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0])).ToString();
                    table.Rows.Add(tableRow);

                }
                else
                                                                                                                    if (tamperColumns[counter].Contains("Temprature"))//smart meter Deep 11-2-19
                {
                    tableRow = table.NewRow();
                    tableRow[0] = "Temprature";
                    tableRow[1] = "0.0.96.9.128.255";
                    tableRow[2] = "3";
                    tableRow[3] = "2";
                    val = CheckUnit(Convert.ToString(row["Temprature"]));
                    if (val[0] == "----°C" || val[0] == "°C")
                    {
                        tableRow[4] = "----";

                    }

                    else
                    {
                        tableRow[4] = string.IsNullOrEmpty(val[0]) ? null : (decimal.Parse(val[0].Substring(0, 3))).ToString();
                    }

                    if (val[1] == "")
                    {
                        tableRow[5] = "°C";
                    }
                    else
                    {
                        tableRow[5] = string.IsNullOrEmpty(val[1]) ? "" : val[1];
                    }

                    table.Rows.Add(tableRow);

                }
                                                                                                                
                                                                                          
                                                                                                
                // SB Code Change End 20171116

            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, dataSet, "Parameter Name", "Value");
        }

        public DataSet GetTamperOccurRestoreDetail(long tamperId, long meterDataID)
        {
            string defaultValue = "----";
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameter Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("OBIS Code", typeof(System.String)));
            table.Columns.Add(new DataColumn("Class ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
            DataSet dataSet = tamperBLL.DetailData(tamperId, meterDataID);
            int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataID.ToString());
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataRow row = dataSet.Tables[0].Rows[0];
            //int emf = new MeterMasterBLL().GetEMF(meterDataID);
            DataRow tableRow = table.NewRow();
            tableRow[0] = "Current - IR";
            tableRow[1] = "1.0.31.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            string[] val = CheckUnit(Convert.ToString(row[0]));
            if (string.IsNullOrEmpty(val[0]))
                val[0] = "0";
            tableRow[4] = (decimal.Parse(val[0])).ToString();
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Current - IY";
            tableRow[1] = "1.0.51.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[1]));
            if (string.IsNullOrEmpty(val[0]))
                val[0] = "0";
            tableRow[4] = (decimal.Parse(val[0])).ToString();
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Current - IB";
            tableRow[1] = "1.0.71.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[2]));
            if (string.IsNullOrEmpty(val[0]))
                val[0] = "0";
            tableRow[4] = (decimal.Parse(val[0])).ToString();
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Voltage - VRN";
            tableRow[1] = "1.0.32.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[3]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Voltage - VYN";
            tableRow[1] = "1.0.52.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[4]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Voltage - VBN";
            tableRow[1] = "1.0.72.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[5]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Power Factor - R phase";
            tableRow[1] = "1.0.33.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[6]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Power Factor - Y phase";
            tableRow[1] = "1.0.53.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[7]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Power Factor - B phase";
            tableRow[1] = "1.0.73.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[8]));
            tableRow[4] = val[0];
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            //Added Total Power Factor as required in CSPDCL IEC 
            // If a utility does not support this parameter it would display -----  .
            tableRow = table.NewRow();
            tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Total Power Factor"));
            tableRow[1] = "1.0.13.7.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[9]));
            tableRow[4] = string.IsNullOrEmpty(val[0]) ? defaultValue : (decimal.Parse(val[0])).ToString();
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh"));
            tableRow[1] = "1.0.1.8.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[10]));
            if (string.IsNullOrEmpty(val[0]))
                val[0] = "0";
            tableRow[4] = (decimal.Parse(val[0])).ToString();
            tableRow[5] = val[1];
            table.Rows.Add(tableRow);

            //Removed condition for displying this parameter as in genric BCS we will disply all parameters for all utilities .
            //If a utility does not support that parameter it would display ----- for that .
            tableRow = table.NewRow();
            tableRow[0] = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh"));
            tableRow[1] = "1.0.9.8.0.255";
            tableRow[2] = "3";
            tableRow[3] = "2";
            val = CheckUnit(Convert.ToString(row[11]));
            tableRow[4] = string.IsNullOrEmpty(val[0]) ? defaultValue : (decimal.Parse(val[0])).ToString();
            tableRow[5] = string.IsNullOrEmpty(val[1]) ? "kVAh" : val[1];
            table.Rows.Add(tableRow);


            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, dataSet, "Parameter Name", "Value");
        }
        public string TamperExist(string eventCode, string meterDataID)
        {
            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
            DataSet dataSet = tamperBLL.ListData(eventCode, meterDataID);
            
            
            if (dataSet == null)
                return "Absent";
            if (dataSet.Tables.Count == 0)
                return "Absent";
            if (dataSet.Tables[0].Rows.Count == 0)
                return "Absent";
            return "Present";
        }
        public DataSet ConvertTamperOccurRestore(string eventCode, string meterDataID)
        {

            DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
            DLMS650BillingBLL billingBLL = new DLMS650BillingBLL();
            DataSet dataSet = null;
            if (eventCode.Contains('/'))
            {
                dataSet = tamperBLL.ListEventCodeORData(eventCode.Split('/')[0],eventCode.Split('/')[1], meterDataID);
            }
            else
            {
                dataSet = tamperBLL.ListData(eventCode, meterDataID);
            }

            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn("PKID|EventCode", typeof(System.String)));
                table.Columns.Add(new DataColumn("Description", typeof(System.String)));
                table.Columns.Add(new DataColumn("Event Date Time (0.0.1.0.0.255;8;2)", typeof(System.String)));    //SarkarA code change 20180405 //fix attribute
                DataRow row;
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    row = table.NewRow();
                    row[0] = string.Concat(Convert.ToString(dr[0]), "|", Convert.ToString(dr[3]));

                    row[1] = Convert.ToString(dr[1]);
                    // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                    //if (eventCode.Contains("101"))
                    //{
                    //    row[2] = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(dr[2]));
                    //}
                    //else
                    //{
                    //    row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2]));
                    //}
                    row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2]));
                    table.Rows.Add(row);
                }

                dataSet = new DataSet();
                dataSet.Tables.Add(table);
            }
            return dataSet;

        }

        
        /// <summary>
        /// Adding Status to Transaction Parameters
        /// </summary>
        /// <param name="meterdataId"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet TransactionCounter(int meterdataId, DataSet ds)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Data", typeof(System.String)));
            table.Columns.Add(new DataColumn("Transaction", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.String)));
            TamperTypeBLL tamperTypeBLL = new TamperTypeBLL();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow tmpRow in ds.Tables[0].Rows)
                {
                    DataRow row = table.NewRow();
                    row[1] = tmpRow[1].ToString();
                    row[0] = string.Concat(tmpRow[0].ToString(), "|", meterdataId.ToString());

                    row[2] = TamperExist(tmpRow[0].ToString(), meterdataId.ToString());

                    if (tmpRow[0].ToString().Contains("159/"))
                    {
                        row[2] = TamperExist("159", meterdataId.ToString());

                        if(row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("189", meterdataId.ToString());
                    }

                    if (tmpRow[0].ToString().Contains("158/"))
                    {
                        row[2] = TamperExist("158", meterdataId.ToString());

                        if (row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("188", meterdataId.ToString()); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
                    }

                    if (tmpRow[0].ToString().Contains("164/"))
                    {
                        row[2] = TamperExist("164", meterdataId.ToString());

                        if (row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("187", meterdataId.ToString()); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
                    }

                    if (tmpRow[0].ToString().Contains("165/")) 
                    {
                        row[2] = TamperExist("165", meterdataId.ToString());

                        if (row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("186", meterdataId.ToString()); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
                    }

                    if (tmpRow[0].ToString().Contains("167/")) 
                    {
                        row[2] = TamperExist("167", meterdataId.ToString());

                        if (row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("185", meterdataId.ToString()); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
                    }

                    if (tmpRow[0].ToString().Contains("168/")) 
                    {
                        row[2] = TamperExist("168", meterdataId.ToString());

                        if (row[2].ToString().Contains("Absent"))
                            row[2] = TamperExist("184", meterdataId.ToString()); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
                    }

                    /*if (tmpRow[0].ToString() == "156" && row[2].ToString() == "Absent")
                    {
                        row[0] = string.Concat("190", "|", meterdataId.ToString());
                        row[2] = TamperExist("190", meterdataId.ToString());
                    }*/
                    if (tmpRow[0].ToString() == "157" && row[2].ToString() == "Absent")
                    {
                        row[0] = string.Concat("191", "|", meterdataId.ToString());
                        row[2] = TamperExist("191", meterdataId.ToString());
                    }
                    
                    table.Rows.Add(row);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public DataSet TamperCounter(int meterDataId, int compartment)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Data", typeof(System.String)));
            if (compartment == 4)
                table.Columns.Add(new DataColumn("Transaction", typeof(System.String)));
            else
                table.Columns.Add(new DataColumn("Tamper", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.String)));
            TamperTypeBLL tamperTypeBLL = new TamperTypeBLL();
            DataSet dataTmp = tamperTypeBLL.ExistOrInsert(compartment);
            if (dataTmp == null)
                return null;
            if (dataTmp.Tables.Count == 0)
                return null;
            if (dataTmp.Tables[0].Rows.Count == 0)
                return null;
            foreach (DataRow tmpRow in dataTmp.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                row[1] = tmpRow[1].ToString();
                //if (row[1].ToString() == "Meter Cover Opening - Occurrence")
                //    continue;
                row[0] = string.Concat(tmpRow[0].ToString(), "|", meterDataId.ToString());
                row[2] = TamperExist(tmpRow[0].ToString(), meterDataId.ToString());
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataSet ConvertHistoryWithSingleColumn(DataSet ds, int MeterDataID)
        {
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID.ToString());
            string meterVariant = GetMeterVariantByMeterDataID(MeterDataID);

            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            bool isPowerFactorComingFromMeter = true;
            string value = string.Empty;
            string valueImport = string.Empty;
            string valueExport = string.Empty;
            string colName = string.Empty;
            table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactorHistory, typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactor, typeof(System.String)));
            table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactorImport, typeof(System.String)));
            table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactorExport, typeof(System.String)));

            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                value = dataRow[2].ToString();
                valueImport = dataRow[3].ToString();
                valueExport = dataRow[4].ToString();
                // This conditional check is implemented to avoid or restrict the Power factor data of DLMS meter to inter into this check implemented for 1P IEC meter Current value for Power factor empty then apply "---".
                if (!(MeterModelNo == NamePlateConstants.VIM_Series2 
                    || MeterModelNo == NamePlateConstants.SapphireLTCT
                    || MeterModelNo == NamePlateConstants.SapphireLTCT_st
                    || MeterModelNo == NamePlateConstants.RubyE250Value 
                    || MeterModelNo == NamePlateConstants.Ruby6Value 
                    || MeterModelNo == NamePlateConstants.WBValue
                    || MeterModelNo == NamePlateConstants.SapphireValue 
                    || MeterModelNo == NamePlateConstants.TNValue
                    || MeterModelNo == NamePlateConstants.Ruby6ukModelValue 
                    || MeterModelNo == NamePlateConstants.TwoTOUSapphireValue 
                    || MeterModelNo == NamePlateConstants.SFSP 
                    || MeterModelNo == NamePlateConstants.VBSPNoSeasonNoWeek 
                    || MeterModelNo == NamePlateConstants.VFSPNoSeasonNoWeek 
                    || MeterModelNo == NamePlateConstants.Smartmeter_HTCT 
                    || MeterModelNo == NamePlateConstants.Smartmeter_WCM 
                    || MeterModelNo == NamePlateConstants.Smartmeter_LTCT 
                    || MeterModelNo == NamePlateConstants.SM110value 
                    || MeterModelNo == NamePlateConstants.Sapphire_SH 
                    || MeterModelNo == NamePlateConstants.Sapphire_SM 
                    || MeterModelNo == NamePlateConstants.Sapphire_sm 
                    || MeterModelNo == NamePlateConstants.Sapphire_sh
                    || MeterModelNo == NamePlateConstants.WBLTValue 
                    || MeterModelNo == NamePlateConstants.TwoTOUltModelValue 
                    || MeterModelNo == NamePlateConstants.RubyE350Value 
                    || MeterModelNo == NamePlateConstants.PumaLTE650Value 
                    || MeterModelNo == NamePlateConstants.PumaHTE650Value 
                    || MeterModelNo == NamePlateConstants.LTCTCortexValue 
                    || MeterModelNo == NamePlateConstants.HTCTCortexValue 
                    || MeterModelNo == NamePlateConstants.PumaHTE650MWValue 
                    || MeterModelNo == NamePlateConstants.BYPL_7Slot 
                    || MeterModelNo == NamePlateConstants.BRPL_7Slot 
                    || MeterModelNo == NamePlateConstants.BYPL_FD
                    || MeterModelNo == NamePlateConstants.SapphireWCM_St 
                    || MeterModelNo == NamePlateConstants.SapphireS2
                    || MeterModelNo == NamePlateConstants.BRPL_CBSP //user story 1016689
                    || MeterModelNo == NamePlateConstants.Sapphire_Netmeter_LTCT
                    || MeterModelNo == NamePlateConstants.Sapphire_Netmeter_WCM
                    ))
                {
                    if (value == "" && (dataRow[0].ToString() == "0" || dataRow[0].ToString() == "13")) //--History 13 check added to avoid flag "isPowerFactorComingFromMeter" false incase data coming from meter incase of Non-DLMS meters
                    { value = "---"; }
                }

                if (!string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(valueImport) || !string.IsNullOrEmpty(valueExport))
                {
                    row = table.NewRow();
                    int rowVal = Convert.ToInt32(dataRow[0]);
                    if (rowVal == 0)
                        row[0] = "Current";
                    else
                        row[0] = "History - " + rowVal.ToString();
                    string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                    row[1] = val;

                    //val = Convert.ToString(dataRow[2]);
                    val = value;
                    //Power factor Standard
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = decimal.Parse(dat[0]).ToString();
                    }
                    row[2] = val;

                    //Power factor Forward
                    val = Convert.ToString(dataRow[3]);
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = decimal.Parse(dat[0]).ToString();
                    }
                    row[3] = val;

                    //Power factor Export
                    val = Convert.ToString(dataRow[4]);
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = decimal.Parse(dat[0]).ToString();
                    }
                    row[4] = val;

                    table.Rows.Add(row);
                    // Modifaied by deep : Avg PF Issue discussion
                    isPowerFactorComingFromMeter = true; 
                }
                else
                {
                    isPowerFactorComingFromMeter = false;
                }
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conPowerFactorImport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
                table.Columns.Remove(colName);
            colName = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conPowerFactorExport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
                table.Columns.Remove(colName);
            colName = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conPowerFactor);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
                table.Columns.Remove(colName);


            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            if (!isPowerFactorComingFromMeter)
            {
                dataSet = null;
            }
            return dataSet;
        }


        /// <summary>
        ///  Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet ConvertHistoryWithSingleColumnBillingTransaction(DataSet ds, int meterModel)
        {
            bool IsNull = false;
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count > 1)
            {
                for (int count = 1; count < ds.Tables[0].Rows.Count; count++)
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[count]["Transaction"].ToString()))
                        IsNull = true;
                    else
                        break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Transaction"].ToString()))
                    IsNull = true;
            }
            if (!IsNull)
            {
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactorHistory, typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing TimeStamp", typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing Type", typeof(System.String)));
                DataRow row;
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    row = table.NewRow();
                    string rowVal = Convert.ToString(dataRow[0]);
                    if (rowVal == "255")  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                    {
                        row[0] = "Current";
                        row["Billing Type"] = "---------";
                    }
                    else
                    {
                        row[0] = "History - " + rowVal.ToString();
                        string val = Convert.ToString(dataRow["Transaction"]);
                        if (val.IndexOf('*') > 0)
                        {
                            string[] dat = val.Split('*');
                            val = decimal.Parse(dat[0]).ToString();
                        }
                        if (val == "0")
                        {
                            row["Billing Type"] = "AUTO";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                        }
                        else if (val == "2")
                        {
                            row["Billing Type"] = "MANUAL";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                        }
                        else if (val == "1")
                        {
                            row["Billing Type"] = "SOFTWARE";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                        }
                        else
                        {
                            row["Billing Type"] = "---------";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                        }
                    }
                    row["Billing TimeStamp"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow["BillingTimeStamp"]));
                    table.Rows.Add(row);
                }
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(table);
                return dataSet;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet ConvertHistoryWithSingleColumnBillingTransaction(DataSet ds)
        {
            bool IsNull = false;
            string billingType = string.Empty;
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count > 1)
            {
                for (int count = 1; count < ds.Tables[0].Rows.Count; count++)
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[count]["Transaction"].ToString()))
                        IsNull = true;
                    else
                        break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Transaction"].ToString()))
                    IsNull = true;
            }
            if (!IsNull)
            {
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn(GlobalConstants.conPowerFactorHistory, typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing TimeStamp", typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing Type", typeof(System.String)));
                DataRow row;
                
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    row = table.NewRow();
                    string rowVal = Convert.ToString(dataRow[0]);

                    if (rowVal == "0")
                    {
                        row[0] = "Current";
                        row["Billing Type"] = "---------";
                    }
                    else
                    {
                        row[0] = "History - " + rowVal.ToString();
                        string val = Convert.ToString(dataRow["Transaction"]);
                        if (val.ToUpperInvariant().Contains("WB"))
                        {
                            val = val.Substring(0, val.ToUpperInvariant().IndexOf("_WB"));

                            if (val.IndexOf('*') > 0)
                            {
                                string[] dat = val.Split('*');
                                val = decimal.Parse(dat[0]).ToString();
                            }
                            if (val == "0")
                            {
                                row["Billing Type"] = "AUTO";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                            }
                            else if (val == "2")
                            {
                                row["Billing Type"] = "MANUAL";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                            }
                            else if (val == "1")
                            {
                                // Commented the below meter model check for WBSDCL supply demand
                                //row["Billing Type"] = "SOFTWARE";// Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                                row["Billing Type"] = "COMMAND";  
                            }
                            else
                            {
                                row["Billing Type"] = "---------";  // Story no: 490966- WB tender specific check implemented for billing Rest Type mapping change
                            }

                        }
                        else
                        {
                            if (val.IndexOf('*') > 0)
                            {
                                string[] dat = val.Split('*');
                                val = decimal.Parse(dat[0]).ToString();
                            }
                            if (val == "0")
                            {
                                row["Billing Type"] = "------";
                            }
                            else if (val == "1")
                            {
                                row["Billing Type"] = "AUTO";
                            }
                            else if (val == "2")
                            {
                                row["Billing Type"] = "MANUAL";
                            }
                            else if (val == "3")
                            {
                                // Commented the below meter model check for WBSDCL supply demand
                                //row["Billing Type"] = "SOFTWARE";
                                row["Billing Type"] = "COMMAND";
                            }
                                 else if (val == "4")
                        {
                            row["Billing Type"]  = "DIP Change";
                        }
                        else if (val == "5")
                        {
                            row["Billing Type"] = "New Firmware Upgrade";
                        }
                         else if (val == "6")
                        {
                            row["Billing Type"] = "Metering Mode Change";
                        }
                         else if (val == "7")
                        {
                            row["Billing Type"] = "Payment Mode Change";
                        }
                            else if (val == "8")
                            {
                                row["Billing Type"] = "Manual";
                            }
                            else if (val == "")
                            {
                                row["Billing Type"] = "------";
                            }
                            else
                            {
                                row["Billing Type"] = val;
                            }
                        }
                    }
                    row["Billing TimeStamp"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow["BillingTimeStamp"]));
                    table.Rows.Add(row);
                }
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(table);
                return dataSet;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet ConvertLoadFactorData(DataSet loadFactor)
        {
            DataSet dataSet = new DataSet();
            string defaultValue = "---------";
            bool isLoadFactorComingFromMeter = false;

            bool iskWImoprtLoadFactorComingFromMeter = false;
            bool iskWExoprtLoadFactorComingFromMeter = false;
            bool iskVAImoprtLoadFactorComingFromMeter = false;
            bool iskVAExoprtLoadFactorComingFromMeter = false;


            string averageLoadFactor = string.Empty;

            string averagekWImportLoadFactor = string.Empty;
            string averagekWExportLoadFactor = string.Empty;
            string averagekVAExportLoadFactor = string.Empty;
            string averagekVAImportLoadFactor = string.Empty;


            if (loadFactor != null && loadFactor.Tables != null && loadFactor.Tables.Count > 0)
            {
                DataTable table = new DataTable();

                foreach (DataRow dataRow in loadFactor.Tables[0].Rows)
                {
                    averageLoadFactor = Convert.ToString(dataRow["BillingAvgLoadFactor"]);
                    averagekWImportLoadFactor = Convert.ToString(dataRow["BillingAvgkWImportLoadFactor"]);
                    averagekWExportLoadFactor = Convert.ToString(dataRow["BillingAvgkWExportLoadFactor"]);
                    averagekVAImportLoadFactor = Convert.ToString(dataRow["BillingAvgkVAImportLoadFactor"]);
                    averagekVAImportLoadFactor = Convert.ToString(dataRow["BillingAvgkVAExportLoadFactor"]);
                }

                table.Columns.Add(new DataColumn(BCSConstants.History, typeof(System.String)));

                //  table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));

                if (averageLoadFactor.ToUpperInvariant().Contains("WB"))
                {
                    table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumn_WB, typeof(System.String)));
                }
                else
                {
                    if (averageLoadFactor != "---------")
                    {
                        table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumn, typeof(System.String)));
                    }
                    if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
                    {
                        table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForImportkW, typeof(System.String)));//pradip_expimp
                        table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForExportkW, typeof(System.String)));//pradip_expimp
                        table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForImportkVA, typeof(System.String)));//pradip_expimp
                        table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForExportkVA, typeof(System.String)));//pradip_expimp
                    }
                }

                DataRow row;
                foreach (DataRow dataRow in loadFactor.Tables[0].Rows)
                {
                    row = table.NewRow();
                    int rowVal = Convert.ToInt32(dataRow[0]);
                    if (rowVal == 0)
                        row[0] = "Current";
                    else
                        row[0] = "History - " + rowVal.ToString();
                    row[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));

                    //  averageLoadFactor = Convert.ToString(dataRow[2]);
                    if (averageLoadFactor != "---------")
                    {
                        averageLoadFactor = Convert.ToString(dataRow[2]);

                    }
                    else
                    {
                        if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
                        {
                            averagekWImportLoadFactor = Convert.ToString(dataRow[3]);
                            averagekWExportLoadFactor = Convert.ToString(dataRow[4]);
                            averagekVAImportLoadFactor = Convert.ToString(dataRow[5]);
                            averagekVAExportLoadFactor = Convert.ToString(dataRow[6]);
                        }
                    }

                    if (averageLoadFactor.ToUpperInvariant().Contains("WB"))
                        averageLoadFactor = averageLoadFactor.Substring(0, averageLoadFactor.ToUpperInvariant().IndexOf("_WB"));
                    if (averageLoadFactor.IndexOf('*') > 0)
                    {
                        string[] dat = averageLoadFactor.Split('*');
                        averageLoadFactor = decimal.Parse(dat[0]).ToString();
                    }

                    //string val = Convert.ToString(dataRow[2]);
                    //if (val.IndexOf('*') > 0)
                    //{
                    //    string[] dat = val.Split('*');
                    //    val = decimal.Parse(dat[0]).ToString();
                    //}
                    //In case meter does not sends load factor value then null is inserted in DB and in this case we fetch "---------"  from DB.
                    //Now if all  billing  has load factor value as "---------" that means load factor is not coming from meter So BCS need to calculate it.
                    if (averageLoadFactor != defaultValue)
                    {
                        isLoadFactorComingFromMeter = true;
                    }
                    if (averagekWImportLoadFactor != defaultValue && averagekWExportLoadFactor != defaultValue && averagekVAImportLoadFactor != defaultValue && averagekVAExportLoadFactor != defaultValue)
                    {
                        isLoadFactorComingFromMeter = true;
                    }

                    if (averageLoadFactor != "---------")
                    {
                        row[2] = averageLoadFactor;

                    }
                    else
                    {
                        if (averagekWImportLoadFactor != "---------" && averagekWExportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------" && averagekVAImportLoadFactor != "---------")
                        {
                            row[2] = averagekWImportLoadFactor;
                            row[3] = averagekWExportLoadFactor;
                            row[4] = averagekVAImportLoadFactor;
                            row[5] = averagekVAExportLoadFactor;
                        }
                    }


                    table.Rows.Add(row);
                }

                dataSet.Tables.Add(table);
                if (!isLoadFactorComingFromMeter)
                {
                    dataSet = null;
                }
            }
            return dataSet;
        }


        /// <summary>
        /// Average Load in Billing profile kilo Watt
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet ConvertAverageLoadData(DataSet BillingAverageLoad)
        {
            DataSet dataSet = new DataSet();
            string defaultValue = "---------";
            bool isLoadFactorComingFromMeter = false;
            string averageLoad = string.Empty;
            if (BillingAverageLoad != null && BillingAverageLoad.Tables != null && BillingAverageLoad.Tables.Count > 0)
            {
                DataTable table = new DataTable();

                table.Columns.Add(new DataColumn(BCSConstants.History, typeof(System.String)));
                table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(BCSConstants.AverageLoadColumn), typeof(System.String)));
                
                DataRow row;
                foreach (DataRow dataRow in BillingAverageLoad.Tables[0].Rows)
                {
                    row = table.NewRow();
                    int rowVal = Convert.ToInt32(dataRow[0]);
                    if (rowVal == 0)
                        row[0] = "Current";
                    else
                        row[0] = "History - " + rowVal.ToString();
                    row[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));

                    averageLoad = Convert.ToString(dataRow[2]);
                    if (averageLoad.IndexOf('*') > 0)
                    {
                        string[] dat = averageLoad.Split('*');
                        averageLoad = decimal.Parse(dat[0]).ToString();
                    }
                   
                    //In case meter does not sends load factor value then null is inserted in DB and in this case we fetch "---------"  from DB.
                    //Now if all  billing  has load factor value as "---------" that means load factor is not coming from meter So BCS need to calculate it.
                    if (averageLoad != defaultValue)
                    {
                        isLoadFactorComingFromMeter = true;
                    }
                    row[2] = averageLoad;
                    table.Rows.Add(row);
                }

                dataSet.Tables.Add(table);
                if (!isLoadFactorComingFromMeter)
                {
                    dataSet = null;
                }
            }
            return dataSet;
        }    


        public DataSet ConvertMaximumDemandToColumn(DataSet ds, long meterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            string colName = string.Empty;
            string colNameTime = string.Empty;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(GlobalConstants.conMaximumDemandHistory, typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBillingDateTime), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKW), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVA), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseKW), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseTIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseKW), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseTIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseKW), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseTIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWImport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPImport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAImport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPImport), typeof(System.String)));

            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWExport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPExport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAExport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPExport), typeof(System.String)));

            // User Story - 1000867
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLag), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLagTIMESTAMP), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLead), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLeadTIMESTAMP), typeof(System.String)));

            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[GlobalConstants.conMaximumDemandHistory] = "Current";
                else
                    row[GlobalConstants.conMaximumDemandHistory] = "History - " + rowVal.ToString();
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBillingDateTime)] = SplitWithOutDateUnit(Convert.ToString(dataRow[1]));
                string val = Convert.ToString(dataRow[2]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKW)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[3]));
                val = Convert.ToString(dataRow[4]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVA)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[5]));

                //R Phase MD kW and DateTime time stamp
                val = Convert.ToString(dataRow[6]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseKW)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[7]));

                //B Phase MD kW and DateTime time stamp
                val = Convert.ToString(dataRow[8]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseKW)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[9]));

                //B Phase MD kW and DateTime time stamp
                val = Convert.ToString(dataRow[10]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseKW)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[11]));


                //Import Start

                val = Convert.ToString(dataRow[12]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWImport)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPImport)] = SplitWithOutDateUnit(Convert.ToString(dataRow[13]));



                val = Convert.ToString(dataRow[14]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAImport)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPImport)] = SplitWithOutDateUnit(Convert.ToString(dataRow[15]));

                //Import End



                //Export Start
                
                val = Convert.ToString(dataRow[16]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWExport)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPExport)] = SplitWithOutDateUnit(Convert.ToString(dataRow[17]));


               
                val = Convert.ToString(dataRow[18]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAExport)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPExport)] = SplitWithOutDateUnit(Convert.ToString(dataRow[19]));

                //Export End

                // User Story - 1000867
                val = Convert.ToString(dataRow[20]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLag)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLagTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[21]));
                val = Convert.ToString(dataRow[22]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                if (val != string.Empty)
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLead)] = val;
                row[CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLeadTIMESTAMP)] = SplitWithOutDateUnit(Convert.ToString(dataRow[23]));


                table.Rows.Add(row);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWImport);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPImport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAImport);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPImport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }


            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWExport);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMPExport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVAExport);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMPExport);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKW);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVA);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVATIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseKW);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandRPhaseTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("------")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseKW);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandYPhaseTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("------")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseKW);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandBPhaseTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("------")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            // User Story - 1000867
            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLag);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLagTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("------")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }
            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLead);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKVARLeadTIMESTAMP);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("------")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet);
        }

        public DataSet ConvertTariffEnergyTODMDToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD kW(1.0.1.6.T.255;4;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD kW Time Stamp (1.0.1.6.T.255;4;5)", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD kVA (1.0.9.6.T.255;4;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD kVA Time Stamp (1.0.9.6.T.255;4;5)", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;
            for (int counter = 1; counter < 9; counter++)
            {
                row = table.NewRow();
                string colName1 = "MDkWTZ" + counter.ToString();
                string colName2 = "MDkWDateTimeTZ" + counter.ToString();
                string colName3 = "MDkVATZ" + counter.ToString();
                string colName4 = "MDkVADateTimeTZ" + counter.ToString();
                row[0] = tariffNumber.ToString();
                row[1] = CheckUnit(Convert.ToString(dataRow[colName1]))[0];
                row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName2]));
                row[3] = CheckUnit(Convert.ToString(dataRow[colName3]))[0];
                row[4] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName4]));
                table.Rows.Add(row);
                tariffNumber++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataSet ConvertTariffEnergyToColumnForRPT(DataSet ds, long meterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);
            //string ColumnNamelst = GetBillingParameterColumn((int)meterDataId);
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh Import(1.0.1.8.0.255;3;2)", typeof(System.String)));//pks
            table.Columns.Add(new DataColumn("kVAhImport(1.0.9.8.0.255;3;2)", typeof(System.String)));//pks

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Import"]) != string.Empty)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), typeof(System.String)));
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykVAhTZ1Import"]) != string.Empty)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), typeof(System.String)));

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Export"]) != string.Empty)
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), typeof(System.String)));
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykVAhTZ1Export"]) != string.Empty)
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), typeof(System.String)));
           
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLagTZ1Q1"]) != string.Empty)   
            //table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1), typeof(System.String)));
                table.Columns.Add(new DataColumn("kvarh - Lag(1.0.5.5.0.255;3;2)", typeof(System.String)));
              
            
            
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1Q4"]) != string.Empty)      
               // table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4), typeof(System.String)));
                table.Columns.Add(new DataColumn("kvarh - Lead(1.0.8.8.0.255;3;2)", typeof(System.String)));

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1Q2"]) != string.Empty)        
               // table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));


            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));



            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLagTZ1Q3"]) != string.Empty)      
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3), typeof(System.String)));


            
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1"]) != string.Empty)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD), typeof(System.String)));
            table.Columns.Add(new DataColumn("BillingMonth", typeof(System.String)));

            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
            {
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))      //SarkarA code change 20180424 //Don't display Net if export not present
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet), typeof(System.String)));

                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))     
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet), typeof(System.String)));
            }


            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;
            for (int counter = 1; counter < ConfigInfo.BillingTariffCount + 1; counter++)
            {
                row = table.NewRow();
                if (dataRow[18].ToString() == "0")

                    row[0] = "Current";
                else
                    row[0] = "History- " + dataRow[18].ToString();

                if (dataRow[18].ToString() == "13")
                {
                    row[0] = "Initial";
                } 


                // row[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow["BillingDate"]));
                string colName1 = "CumulativeEnergykWhTZ" + counter.ToString();
                string colName2 = "CumulativeEnergykVAhTZ" + counter.ToString();
                string colName3 = "CumulativeEnergykvarhLagTZ" + counter.ToString();
                string colName4 = "CumulativeEnergykvarhLeadTZ" + counter.ToString();
                row[1] = tariffNumber.ToString();
                string val = Convert.ToString(dataRow[colName1]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[2] = val;
                val = Convert.ToString(dataRow[colName2]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[3] = val;

                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)))
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)] = Convert.ToString(dataRow[colName3]);
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)))
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)] = Convert.ToString(dataRow[colName4]);
                row["BillingMonth"] = dataRow["BillingMonth"];

                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                {
                    string colName5 = "CumulativeEnergykWhTZ" + counter.ToString() + "Export";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)] = (Convert.ToString(CheckUnit(Convert.ToString(dataRow[colName5]))[0]));
                }
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                {
                    string colName6 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Export";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)] = (Convert.ToString(CheckUnit(Convert.ToString(dataRow[colName6]))[0]));
                }


                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                {
                    string colName5 = "CumulativeEnergykWhTZ" + counter.ToString() + "Import";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)] = (Convert.ToString(CheckUnit(Convert.ToString(dataRow[colName5]))[0]));
                }
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                {
                    string colName6 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Import";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)] = (Convert.ToString(CheckUnit(Convert.ToString(dataRow[colName6]))[0]));
                }

                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)))
                {
                    string colName7 = "CumulativeEnergykWhTZ" + counter.ToString() + "Net";
                    string ImportClmName = "CumulativeEnergykWhTZ" + counter.ToString();
                    string ExportClmName = "CumulativeEnergykWhTZ" + counter.ToString() + "Export";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)] = GetNetValue(dataRow, ImportClmName, ExportClmName);
                    //(Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName7]))[0])).ToString();
                }
                if (table.Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))
                {
                    string colName7 = "CumulativeEnergykvarhLagTZ" + counter.ToString() + "Q1"; ;
                    
                    if (Convert.ToString(dataRow[colName7]) == "")
                    {
                        row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = "0.000";
                    }
                    else
                    {

                        row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = Convert.ToString(dataRow[colName7]);
                    }
                }


                //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)))
                    
                //{
                //    string colName7 = "CumulativeEnergykvarhLeadTZ" + counter.ToString() + "Q4";
                   
                //    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)] = Convert.ToString(dataRow[colName7]);
                //}

                //Q4

                if (table.Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
                {
                    string colName7 = "CumulativeEnergykvarhLeadTZ" + counter.ToString() + "Q4"; ;

                    if (Convert.ToString(dataRow[colName7]) == "")
                    {
                        row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = "0.000";
                    }
                    else
                    {

                        row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = Convert.ToString(dataRow[colName7]);
                    }
                }


                //q2
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)))
                {
                    string colName7 = "CumulativeEnergykvarhLeadTZ" + counter.ToString() + "Q2";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)] = Convert.ToString(dataRow[colName7]);
                }

                //q3
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)))
                {
                    string colName7 = "CumulativeEnergykvarhLagTZ" + counter.ToString() + "Q3";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)] = Convert.ToString(dataRow[colName7]);
                }
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)))
                {
                   string colName8 = "CumulativeEnestring rgykVAhTZ" + counter.ToString() + "Net";
                    string ImportClmName = "CumulativeEnergykVAhTZ" + counter.ToString();
                    string ExportClmName = "CumulativeEnergykVAhTZ" + counter.ToString() + "Export";
                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)] = GetNetValue(dataRow, ImportClmName, ExportClmName);

                    //(Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName8]))[0])).ToString();
                }

                table.Rows.Add(row);
                tariffNumber++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet);
        }

        private string GetBillingParameterColumn(int meterDataID)
        {
            string ColumnNamelst = string.Empty;
            try
            {
                DataSet ds = new MeterMasterDAL().GetBillingParameterColumn(meterDataID);
                ColumnNamelst = Convert.ToString(ds.Tables[0].Rows[0]["ColumnsNames"]);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingParameterColumn(int meterDataID)", ex);

            }
            return ColumnNamelst;
        }
        private bool ValidateNullDataset(DataSet dset)
        {
            if (dset == null)
                return true;
            if (dset.Tables == null)
                return true;
            if (dset.Tables.Count == 0)
                return true;
            if (dset.Tables[0].Rows == null)
                return true;
            if (dset.Tables[0].Rows.Count == 0)
                return true;

            return false;

        }
        /// <summary>
        /// This method is used to format the miscellaneous column.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet ConvertMiscellaneousToColumn(DataSet ds)
        {
            if (ValidateNullDataset(ds))
                return null;
            DataSet dsMiscellaneous = new DataSet();
            DataTable miscellaneoustable = new DataTable();
            DataRow drow;
            //DataRow dataRow;
            miscellaneoustable.Columns.Add(new DataColumn(HISTORY, typeof(System.String)));
            //miscellaneoustable.Columns.Add(new DataColumn(CUMPOWEROFFDURATION, typeof(System.String)));
            miscellaneoustable.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
            miscellaneoustable.Columns.Add(new DataColumn(DELTATAMPERCOUNT, typeof(System.String))); // Story - 345154
            miscellaneoustable.Columns.Add(new DataColumn(CUMTAMPERCOUNT, typeof(System.String)));
            miscellaneoustable.Columns.Add(new DataColumn(CUMPOWERFAILURECOUNT, typeof(System.String)));
            miscellaneoustable.Columns.Add(new DataColumn(CUMBILLINGMDRESETCOUNT, typeof(System.String)));
            miscellaneoustable.Columns.Add(new DataColumn(ABCCodeBilling, typeof(System.String)));
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                drow = miscellaneoustable.NewRow();
                // Added to solve 95898.
                int rowVal = Convert.ToInt32(dataRow["DataIndex"]);
                //drow[CUMPOWEROFFDURATION] = dataRow["CumPowerOffduration"].ToString();
                drow[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                drow[DELTATAMPERCOUNT] = dataRow["DeltaTamperCount"].ToString(); // Story - 345154
                drow[CUMTAMPERCOUNT] = dataRow["CumTamperCount"].ToString();
                drow[CUMPOWERFAILURECOUNT] = dataRow["CumPowerFailureCount"].ToString();
                drow[CUMBILLINGMDRESETCOUNT] = dataRow["CumBillingMDResetCount"].ToString();
                drow[ABCCodeBilling] = dataRow["ABCCodeBilling"].ToString();
                //Added to calculate the history wise power off duration.
                if (rowVal == 0)
                {
                    drow[HISTORY] = CURRENT;

                }
                else
                {
                    drow[HISTORY] = HISTORY + Symbols.SPACE + Symbols.HYPHEN + Symbols.SPACE + rowVal.ToString();

                }
                miscellaneoustable.Rows.Add(drow);
            }
            dsMiscellaneous.Tables.Add(miscellaneoustable);
            return dsMiscellaneous;
        }


        public DataSet ConvertCumulativeMDToColumn(DataSet ds)
        {
            if (ValidateNullDataset(ds))
                return null;

            string MDkW = ds.Tables[0].Rows[0][2].ToString();
            string MDkVA = ds.Tables[0].Rows[0][3].ToString();

            string MDkW1 =string.Empty;
            string MDkVA1=string.Empty ;

            if (ds.Tables[0].Rows.Count > 1 )//check second row
            {
             MDkW1 = ds.Tables[0].Rows[1][2].ToString();
             MDkVA1 = ds.Tables[0].Rows[1][3].ToString();
            }

            bool b1, b2, b3, b4;
            b1 = string.IsNullOrEmpty(MDkW);
            b2 = string.IsNullOrEmpty(MDkVA);
            b3 = string.IsNullOrEmpty(MDkW1);
            b4 = string.IsNullOrEmpty(MDkVA1);

            if (b1 == true && b2 == true && b3 == true && b4 == true)
            {
                return null;
            }

            DataSet dscumulativeMD = new DataSet();
            DataTable cumumdtable = new DataTable();
            DataRow drow;
            //DataRow dataRow;
            cumumdtable.Columns.Add(new DataColumn(HISTORY, typeof(System.String)));
            cumumdtable.Columns.Add(new DataColumn("Billing DateTime(0.0.0.1.2.255;3;2)", typeof(System.String)));
            cumumdtable.Columns.Add(new DataColumn(CumulativeMDkw, typeof(System.String)));
            cumumdtable.Columns.Add(new DataColumn(CumulativeMDkva, typeof(System.String)));
           
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                drow = cumumdtable.NewRow();
                int rowVal = Convert.ToInt32(dataRow["DataIndex"]);
                drow[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                drow[CumulativeMDkw] = dataRow["CumulativeMDkw"].ToString();
                drow[CumulativeMDkva] = dataRow["CumulativeMDkva"].ToString();
               
                //Added to calculate the history wise power off duration.
                if (rowVal == 0)
                {
                    drow[HISTORY] = CURRENT;

                }
                else
                {
                    drow[HISTORY] = HISTORY + Symbols.SPACE + Symbols.HYPHEN + Symbols.SPACE + rowVal.ToString();

                }
                cumumdtable.Rows.Add(drow);
            }
            dscumulativeMD.Tables.Add(cumumdtable);
            return dscumulativeMD;
        }



        public DataSet ConvertTariffEnergyToColumn(DataSet ds, long meterDataId)
        {
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataId.ToString());
            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);

            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAH), typeof(System.String)));

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Import"]) != string.Empty)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), typeof(System.String)));

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykVAhTZ1Import"]) != string.Empty)
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), typeof(System.String)));


            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Export"]) != string.Empty)
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), typeof(System.String)));

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykVAhTZ1Export"]) != string.Empty)
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), typeof(System.String)));


            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet), typeof(System.String)));
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet), typeof(System.String)));
            }

            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLagTZ1"]) != string.Empty)
            
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG), typeof(System.String)));



          if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1"]) != string.Empty)
          
              table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD), typeof(System.String)));



          if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLagTZ1Q1"]) != string.Empty)   
              table.Columns.Add(new DataColumn("kvarh - Lag(1.0.5.5.0.255;3;2)", typeof(System.String)));



          if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1Q4"]) != string.Empty) 
              table.Columns.Add(new DataColumn("kvarh - Lead(1.0.8.8.0.255;3;2)", typeof(System.String)));

          if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLagTZ1Q3"]) != string.Empty)
              table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3), typeof(System.String)));


          if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykvarhLeadTZ1Q2"]) != string.Empty)
              table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2), typeof(System.String)));

           DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;
            for (int counter = 1; counter < ConfigInfo.BillingTariffCount + 1; counter++)
            {
                try
                {
                    row = table.NewRow();
                    string colName1 = "CumulativeEnergykWhTZ" + counter.ToString();
                    string colName2 = "CumulativeEnergykVAhTZ" + counter.ToString();


                    row["Tariff Number"] = tariffNumber.ToString();
                    if (dataRow[colName1].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWH)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName1]))[0])).ToString();
                    if (dataRow[colName2].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAH)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName2]))[0])).ToString();


                    if (table.Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))
                    {
                        string colName7 = "CumulativeEnergykvarhLagTZ" + counter.ToString() + "Q1"; 

                        if (Convert.ToString(dataRow[colName7]) == "")
                        {
                            row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = "0.000";
                        }
                        else
                        {

                            row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = Convert.ToString(dataRow[colName7]);
                        }
                    }

                    if (table.Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
                    {
                        string colName7 = "CumulativeEnergykvarhLeadTZ" + counter.ToString() + "Q4"; 

                        if (Convert.ToString(dataRow[colName7]) == "")
                        {
                            row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = "0.000";
                        }
                        else
                        {

                            row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = Convert.ToString(dataRow[colName7]);
                        }
                    }


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)))
                    {
                        string colName3 = "CumulativeEnergykvarhLagTZ" + counter.ToString();
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName3]))[0])).ToString();
                    }
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)))
                    {
                        string colName4 = "CumulativeEnergykvarhLeadTZ" + counter.ToString();
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName4]))[0])).ToString();
                    }


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                    {
                        string colName5 = "CumulativeEnergykWhTZ" + counter.ToString() + "Import";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName5]))[0])).ToString();
                    }
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                    {
                        string colName6 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Import";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName6]))[0])).ToString();
                    }




                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)))
                    {
                        string colName3 = "CumulativeEnergykvarhLagTZ" + counter.ToString() + "Q3";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName3]))[0])).ToString();
                    }
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)))
                    {
                        string colName4 = "CumulativeEnergykvarhLeadTZ" + counter.ToString() + "Q2";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName4]))[0])).ToString();
                    }


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                    {
                        string colName7 = "CumulativeEnergykWhTZ" + counter.ToString() + "Export";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName7]))[0])).ToString();
                    }
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                    {
                        string colName8 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Export";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)] = (Convert.ToDecimal(CheckUnit(Convert.ToString(dataRow[colName8]))[0])).ToString();
                    }

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)) && meterVariant == MeterVariant.THREE || meterVariant == MeterVariant.FOUR)
                    {
                        string colName9 = "CumulativeEnergykWhTZ" + counter.ToString() + "Net";
                        string ImportClmName = "CumulativeEnergykWhTZ" + counter.ToString();
                        string ExportClmName = "CumulativeEnergykWhTZ" + counter.ToString() + "Export";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)] = GetNetValue(dataRow, ImportClmName, ExportClmName);
                            
                    }

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)) && meterVariant == MeterVariant.THREE || meterVariant == MeterVariant.FOUR)
                    {
                        string colName10 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Net";
                        string ImportClmName = "CumulativeEnergykVAhTZ" + counter.ToString();
                        string ExportClmName = "CumulativeEnergykVAhTZ" + counter.ToString() + "Export";
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)] = GetNetValue(dataRow, ImportClmName, ExportClmName);
                            
                    }


                    table.Rows.Add(row);
                    tariffNumber++;
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertTariffEnergyToColumn(DataSet ds, long meterDataId)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet);
        }
        public DataSet CalculateTariffPFToColumn(DataSet ds, long meterDataId)
        {
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataId.ToString());
            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);

            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("TOD Average PF(1.0.13.0.0.255;3;2)", typeof(System.String)));
            if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Export"]) != "")
                table.Columns.Add(new DataColumn("TOD Average PF Export(1.0.84.0.0.255;3;2)", typeof(System.String)));


            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;

            for (int counter = 1; counter < ConfigInfo.BillingTariffCount + 1; counter++)
            {
                try
                {
                    Double Exportkwh = 0.0;
                    Double Exportkvah = 0.0;
                    row = table.NewRow();
                    string colName1 = "CumulativeEnergykWhTZ" + counter.ToString();
                    string colName2 = "CumulativeEnergykVAhTZ" + counter.ToString();

                    string colName3 = "CumulativeEnergykWhTZ" + counter.ToString() + "Export";
                    string colName4 = "CumulativeEnergykVAhTZ" + counter.ToString() + "Export";

                    if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Export"]) != "")
                    {
                        Exportkwh = Convert.ToDouble(CheckUnit(Convert.ToString(dataRow[colName3]))[0]);
                        Exportkvah = Convert.ToDouble(CheckUnit(Convert.ToString(dataRow[colName4]))[0]);
                    }
                    row["Tariff Number"] = tariffNumber.ToString();
                    if (dataRow[colName1].ToString() != string.Empty && dataRow[colName2].ToString() != string.Empty)
                    {
                        double dPF = Convert.ToDouble(CheckUnit(Convert.ToString(dataRow[colName1]))[0]) / Convert.ToDouble(CheckUnit(Convert.ToString(dataRow[colName2]))[0]);
                        if (double.IsNaN(dPF))
                            dPF = 0.00;
                        row["TOD Average PF(1.0.13.0.0.255;3;2)"] = string.Format("{0:N2}", dPF);
                    }
                    else
                    {
                        if (dataRow[colName1].ToString() == "" && dataRow[colName2].ToString() == "")
                        {
                            double dPF = 0.00;
                            row["TOD Average PF(1.0.13.0.0.255;3;2)"] = string.Format("{0:N2}", dPF);
                        }
                    }
                        
                    if (Convert.ToString(ds.Tables[0].Rows[0]["CumulativeEnergykWhTZ1Export"]) != "")
                    {
                        if (dataRow[colName3].ToString() != string.Empty && dataRow[colName4].ToString() != string.Empty)
                        {
                            double dPF_Export = Exportkwh / Exportkvah;
                            if (double.IsNaN(dPF_Export))
                                dPF_Export = 0.00;
                            row["TOD Average PF Export(1.0.84.0.0.255;3;2)"] = string.Format("{0:N2}", dPF_Export);
                        }
                    }

                    table.Rows.Add(row);
                    tariffNumber++;

                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertTariffEnergyToColumn(DataSet ds, long meterDataId)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet);
        }

        public DataSet ConvertTODConsumptionToColumn(DataSet History1, DataSet History2,int MeterDataID)
        {
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataID.ToString());
            string meterVariant = GetMeterVariantByMeterDataID(MeterDataID);
            if (History1 == null || History2 == null)
                return null;
            if (History1.Tables[0].Rows.Count == 0 || History1.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAH), typeof(System.String)));


            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), typeof(System.String)));
            }



            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), typeof(System.String)));
            }





            if (History1.Tables[0].Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1), typeof(System.String)));
            }
            ////add pradipta_tod

            //if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)))

            if (History1.Tables[0].Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
            //add pradipta_tod
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4), typeof(System.String)));
            }





            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2), typeof(System.String)));
            }
            

            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), typeof(System.String)));
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), typeof(System.String)));
            }
            
            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)))
            {
                //    //*****This condition for smart meter change header text ********
                if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart), typeof(System.String)));
                }
                else
                {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG), typeof(System.String)));
                }
              //  table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG), typeof(System.String)));
           }
            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)))
            {
                //    //*****This condition for smart meter change header text ********
                if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart), typeof(System.String)));
                }
                else
                {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD), typeof(System.String)));
                }
               
            }
            DataRow row;
            for (int counter = 0; counter < ConfigInfo.BillingTariffCount; counter++)
            {
                try
                {
                    DataRow Row1 = History1.Tables[0].Rows[counter];
                    DataRow Row2 = History2.Tables[0].Rows[counter];
                    row = table.NewRow();
                    row[0] = Row1[0];
                    if (Row1[1].ToString() != string.Empty && Row2[1].ToString() != string.Empty)
                        row[1] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[1])), Convert.ToDecimal(Convert.ToString(Row2[1]))).ToString();
                    if (Row1[2].ToString() != string.Empty && Row2[2].ToString() != string.Empty)
                        row[2] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[2])), Convert.ToDecimal(Convert.ToString(Row2[2]))).ToString();
                    if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                    {
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[3])), Convert.ToDecimal(Convert.ToString(Row2[3]))).ToString();
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[4])), Convert.ToDecimal(Convert.ToString(Row2[4]))).ToString();
                    }
                    else
                    {
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[3])), Convert.ToDecimal(Convert.ToString(Row2[3]))).ToString();

                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[4])), Convert.ToDecimal(Convert.ToString(Row2[4]))).ToString();
                    }

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport));


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport));




                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport));


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport));

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)))

                        row["kvarh - Lag (Q1) (1.0.5.5.T.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lag(1.0.5.5.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lag(1.0.5.5.0.255;3;2)"]))).ToString();//add pradipta


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)))

                        row["kvarh - Lead (Q4) (1.0.8.8.T.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lead(1.0.8.8.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lead(1.0.8.8.0.255;3;2)"]))).ToString();//add pradipta



                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)])), Convert.ToDecimal(Convert.ToString(Row2[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)]))).ToString();//add pradipta


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)])), Convert.ToDecimal(Convert.ToString(Row2[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)]))).ToString();//add pradipta



                    

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)))
                    {
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet));

                    }


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)))
                    {
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet));

                    }


                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertTODConsumptionToColumn(DataSet History1, DataSet History2,int MeterDataID)", ex);
                    string aa = ex.Message;
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }


        private string Get_Net_RollOverValues(DataRow Row1, DataRow Row2, string clm1Name, string clm2Name)
        {
            string rollVal = "----";
            try
            {
                rollVal = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[clm1Name])), Convert.ToDecimal(Convert.ToString(Row2[clm2Name]))).ToString();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Get_Net_RollOverValues(DataRow Row1, DataRow Row2, string clm1Name, string clm2Name)", ex);
                
            }
            return rollVal;
        }


        public DataSet ConvertTODConsumptionToColumnForRPT(DataSet History1, DataSet History2, int MeterDataId)
        {
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataId.ToString());
            string meterVariant = GetMeterVariantByMeterDataID(MeterDataId);
            if (History1 == null || History2 == null)
                return null;
            if (History1.Tables[0].Rows.Count == 0 || History1.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAH), typeof(System.String)));


            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), typeof(System.String)));
            }
            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), typeof(System.String)));
            }

            //if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1).Replace("(Q1)","Q1")) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)).Replace("(Q1)","Q1")))

            if (History1.Tables[0].Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))

            {
                table.Columns.Add(new DataColumn("kvarh - Lag(1.0.5.5.0.255;3;2)", typeof(System.String)));
            }

            //q4

            if (History1.Tables[0].Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
            {
                table.Columns.Add(new DataColumn("kvarh - Lead(1.0.8.8.0.255;3;2)", typeof(System.String)));
            }

            //Q2

            if (History1.Tables[0].Columns.Contains("kvarh - Lead Q2 (1.0.6.8.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lead Q2 (1.0.6.8.0.255;3;2)"))//add pks_tou
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));
            }

            //q3

            if (History1.Tables[0].Columns.Contains("kvarh - Lag Q3 (1.0.7.8.0.255;3;2)") && History2.Tables[0].Columns.Contains("kvarh - Lag Q3 (1.0.7.8.0.255;3;2)"))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3), typeof(System.String)));
            }




            //if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4), typeof(System.String)));
            //}

            //if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3), typeof(System.String)));
            //}

            //if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2), typeof(System.String)));
            //}

            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
            {
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))      //SarkarA code change 20180424 //Don't display Net if export not present
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), typeof(System.String)));

                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))     
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), typeof(System.String)));
            }

            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)))
            {
                //    //*****This condition for smart meter change header text ********
                if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart), typeof(System.String)));
                }
                else
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG), typeof(System.String)));
                }
            }
            if (History1.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)) && History2.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)))
            {
                //    //*****This condition for smart meter change header text ********
                if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart), typeof(System.String)));
                }
                else
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD), typeof(System.String)));
                }
            }
                    
            DataRow row;
            string historyValue1 = string.Empty;
            string historyValue2 = string.Empty;
            for (int counter = 0; counter < ConfigInfo.BillingTariffCount; counter++)
            {
                try
                {
                    DataRow Row1 = History1.Tables[0].Rows[counter];
                    DataRow Row2 = History2.Tables[0].Rows[counter];
                    row = table.NewRow();
                    //if (flag)
                    //{
                    //    row[0] = Row1[1];
                    //    row[1] = (Convert.ToDecimal(Convert.ToString(Row1[2])) - Convert.ToDecimal(Convert.ToString(Row2[2]))).ToString();
                    //    row[2] = (Convert.ToDecimal(Convert.ToString(Row1[3])) - Convert.ToDecimal(Convert.ToString(Row2[3]))).ToString();
                    //}
                    //else
                    //{
                    if (Row1["BillingMonth"].ToString() == string.Empty)
                        historyValue1 = "---";
                    else
                        historyValue1 = Row1["BillingMonth"].ToString();
                    if (Row2["BillingMonth"].ToString() == string.Empty)
                        historyValue2 = "---";
                    else
                        historyValue2 = Row2["BillingMonth"].ToString();
                    row[0] = Row1[0] + " - " + Row2[0] + " (" + historyValue1 + " - " + historyValue2 + ")";
                    row[1] = Row1[1];
                    // Story - 349654 - TOD consumption is not coming for Single Phase meter
                    //row[2] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[2])), Convert.ToDecimal(Convert.ToString(Row2[2])));
                    if (Convert.ToString(Row1[2]) != string.Empty && Convert.ToString(Row2[2]) != string.Empty)
                    {
                        row[2] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[2])), Convert.ToDecimal(Convert.ToString(Row2[2])));
                    }
                    //row[3] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[3])), Convert.ToDecimal(Convert.ToString(Row2[3])));
                    if (Convert.ToString(Row1[3]) != string.Empty && Convert.ToString(Row2[3]) != string.Empty)
                    {
                        row[3] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[3])), Convert.ToDecimal(Convert.ToString(Row2[3])));
                    }
                    //*****This condition for smart meter change header text tod avg PF ********
                   
                    if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
                    {
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG_smart)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[4])), Convert.ToDecimal(Convert.ToString(Row2[4])));
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD_smart)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[5])), Convert.ToDecimal(Convert.ToString(Row2[5])));
                    }
                    else
                    {
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLAG)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[4])), Convert.ToDecimal(Convert.ToString(Row2[4])));
                        if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)))
                            row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODConsumptionKVARHLEAD)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[5])), Convert.ToDecimal(Convert.ToString(Row2[5])));
                    }

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport));

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport));

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport));

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport));

                  //  if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)))
                    if (table.Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))

                        //row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)])), Convert.ToDecimal(Convert.ToString(Row2[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ1)]))).ToString();


                        row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lag(1.0.5.5.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lag(1.0.5.5.0.255;3;2)"]))).ToString();



                   // if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)))
                        //row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ4)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[GlobalConstants.conTODEnergyKVARHLEADQ4])), Convert.ToDecimal(Convert.ToString(Row2[GlobalConstants.conTODEnergyKVARHLEADQ4]))).ToString();

                    if (table.Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
                        row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lead(1.0.8.8.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lead(1.0.8.8.0.255;3;2)"]))).ToString();



                    //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)))
                    //    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAGQ3)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[GlobalConstants.conTODEnergyKVARHLAGQ3])), Convert.ToDecimal(Convert.ToString(Row2[GlobalConstants.conTODEnergyKVARHLAGQ3]))).ToString();


                    if (table.Columns.Contains("kvarh - Lead Q2 (1.0.6.8.0.255;3;2)"))
                        row["kvarh - Lead Q2 (1.0.6.8.0.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lead Q2 (1.0.6.8.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lead Q2 (1.0.6.8.0.255;3;2)"]))).ToString();




                    //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)))
                    //    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEADQ2)] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1[GlobalConstants.conTODEnergyKVARHLEADQ2])), Convert.ToDecimal(Convert.ToString(Row2[GlobalConstants.conTODEnergyKVARHLEADQ2]))).ToString();                 
                    if (table.Columns.Contains("kvarh - Lag Q3 (1.0.7.8.0.255;3;2)"))
                        row["kvarh - Lag Q3 (1.0.7.8.0.255;3;2)"] = getRolloverValues(Convert.ToDecimal(Convert.ToString(Row1["kvarh - Lag Q3 (1.0.7.8.0.255;3;2)"])), Convert.ToDecimal(Convert.ToString(Row2["kvarh - Lag Q3 (1.0.7.8.0.255;3;2)"]))).ToString();



                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)))
                    {
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet));
                    }

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)))
                    {
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)] = Get_Net_RollOverValues(Row1, Row2,
                            CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet));
                    }
                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {

                    logger.Log(LOGLEVELS.Error, "ConvertTODConsumptionToColumnForRPT(DataSet History1, DataSet History2, int MeterDataId)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        /// <summary>
        /// Convert Meter Data For report With Month
        /// </summary>
        /// <param name="DS"></param>
        /// <returns></returns>
        public DataSet  ConvertMeterDataForRPTWithMonth(DataSet DS)
        {
            if (DS == null)
                return null;
            if (DS.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh Import(1.0.1.8.0.255;3;2)", typeof(System.String)));//pks
            table.Columns.Add(new DataColumn("kVAhImport(1.0.9.8.0.255;3;2)", typeof(System.String)));//pks


            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport), typeof(System.String)));

            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport), typeof(System.String)));


            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport), typeof(System.String)));

            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport), typeof(System.String)));
            

            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG), typeof(System.String)));

            //if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)))//add pks_q1
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1), typeof(System.String)));

            if (DS.Tables[0].Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))
                table.Columns.Add(new DataColumn("kvarh - Lag(1.0.5.5.0.255;3;2)", typeof(System.String)));

            //if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)))//add pks_q1
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4), typeof(System.String)));
            if (DS.Tables[0].Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
                table.Columns.Add(new DataColumn("kvarh - Lead(1.0.8.8.0.255;3;2)", typeof(System.String)));


            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));



            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3), typeof(System.String)));



            
            if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD), typeof(System.String)));
              if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)))
                     table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet), typeof(System.String)));

              if (DS.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)))
                  table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet), typeof(System.String)));
                     


            DataRow row;
            string historyValue = string.Empty;
            for (int counter = 0; counter < ConfigInfo.BillingTariffCount; counter++)
            {
                try
                {
                    DataRow Row = DS.Tables[0].Rows[counter];
                    row = table.NewRow();
                    if (Row["BillingMonth"].ToString() == string.Empty)
                        historyValue = "---";
                    else
                        historyValue = Row["BillingMonth"].ToString();
                    row[0] = Row[0] + " (" + historyValue + ")";
                    row[1] = Row[1];
                    row[2] = Row[2];
                    row[3] = Row[3];
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLAG)))
                        row[4] = Row[4];
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVARHLEAD)))
                        row[5] = Row[5];

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHExport)];





                    if (table.Columns.Contains("kvarh - Lag(1.0.5.5.0.255;3;2)"))
                        row["kvarh - Lag(1.0.5.5.0.255;3;2)"] = Row["kvarh - Lag(1.0.5.5.0.255;3;2)"];


                    if (table.Columns.Contains("kvarh - Lead(1.0.8.8.0.255;3;2)"))
                        row["kvarh - Lead(1.0.8.8.0.255;3;2)"] = Row["kvarh - Lead(1.0.8.8.0.255;3;2)"];


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)];


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)];





                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHExport)];


                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHImport)];
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHImport)];

                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKWHNet)];
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)))
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)] = Row[CommonMethods.getDisplayHeaderText(GlobalConstants.conTODEnergyKVAHNet)];
                    //}
                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertMeterDataForRPTWithMonth(DataSet DS)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataSet ConvertCumulativeEnergyCalculatedToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            string colName = string.Empty;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(GlobalConstants.consConsumptionHistory, typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD), typeof(System.String)));
            //*********** For Sapphire S2 KSEB*********
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead), typeof(System.String)));
            }


            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport), typeof(System.String)));
            }

            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1), typeof(System.String)));
            }

            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4), typeof(System.String)));
            }

            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3), typeof(System.String)));
            }

            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));
            }

            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), typeof(System.String)));
            }
            if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)))
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), typeof(System.String)));
            }

            //#region JDVVNL
           
            //if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase), typeof(System.String)));
            //}
            //if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase), typeof(System.String)));
            //}
            //if (ds.Tables[0].Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)))
            //{
            //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase), typeof(System.String)));
            //}

            //#endregion

            DataRow row;
            int counter = 0;
            int counterInner = 1;
            for (counter = 0; counter < ds.Tables[0].Rows.Count; counter++, counterInner++) // Story - 365971 - 13 billing for Power ON Hours
            {
                try
                {
                    if (ds.Tables[0].Rows.Count > counter)
                    {
                        DataRow firstRow = ds.Tables[0].Rows[counter];
                        if (counterInner == ds.Tables[0].Rows.Count) // Story - 365971 - 13 billing for Power ON Hours
                            break;
                        if (ds.Tables[0].Rows.Count > counterInner)
                        {
                            DataRow secondRow = ds.Tables[0].Rows[counterInner];
                            row = table.NewRow();
                            row[0] = Convert.ToString(firstRow[0]) + " - " + Convert.ToString(secondRow[0]);
                            if (firstRow[2].ToString() != string.Empty && secondRow[2].ToString() != string.Empty)
                                row[1] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));
                            if (firstRow[3].ToString() != string.Empty && secondRow[3].ToString() != string.Empty)
                                row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[3]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[3]))));
                            if (firstRow[4].ToString() != string.Empty && secondRow[4].ToString() != string.Empty)
                                row[3] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[4]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[4]))));
                            if (firstRow[5].ToString() != string.Empty && secondRow[5].ToString() != string.Empty)
                                row[4] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[5]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[5]))));
                            //Leave BillingType

                            //********** for sapphire S2 KSEB Start********
                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)]))));
                                }
                            }
                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)]))));
                                }
                            }
                            //********** for sapphire S2 KSEB END********  

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)]))));
                                }
                            }
                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)]))));
                                }
                            }
                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)]))));
                                }
                            }


                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)]))));
                                }
                            }
                            
                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)]))));
                                }
                            }

                            if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)))
                            {
                                if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)].ToString() != string.Empty)
                                {
                                    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)]))));
                                }
                            }
                            //#region JDVVNL

                            //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)))
                            //{
                            //    if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)].ToString() != string.Empty)
                            //    {
                            //        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)]))));
                            //    }
                            //}

                            //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)))
                            //{
                            //    if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)].ToString() != string.Empty)
                            //    {
                            //        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)]))));
                            //    }
                            //}
                          
                            //if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)))
                            //{
                            //    if (firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)].ToString() != string.Empty && secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)].ToString() != string.Empty)
                            //    {
                            //        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)]))));
                            //    }
                            //}
                            //#endregion


                            table.Rows.Add(row);
                        }
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertCumulativeEnergyCalculatedToColumn(DataSet ds)", ex);
                }
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                table.Columns.Remove(colName);
            colName = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                table.Columns.Remove(colName);

            // To remove Column having value "----" in the DataTable
            int flag = 0;
            List<int> colIndex = new List<int>();
            for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
            {
                for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                {
                    if (!table.Rows[counterRow][counterCol].Equals("----"))
                    {
                        flag++;
                        break;
                    }
                    else
                        flag = 0;
                }
                if (flag == 0)
                    colIndex.Add(counterCol);
            }
            for (int Counter = colIndex.Count - 1; Counter >= 0; Counter--)
            {
                table.Columns.RemoveAt(colIndex[Counter]);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }



        public string GetMeterVariantByMeterDataID(int meterDataID)
        {
            string meterVariant = string.Empty;
            try
            {
                DataSet ds = new MeterMasterDAL().GetMeterVariantByMeterDataID(meterDataID);
                meterVariant = Convert.ToString(ds.Tables[0].Rows[0]["NetMeterVariantInfo"]);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterDataID(int meterDataID)", ex);
                
            }
            return meterVariant;
        }


        public string GetMeterVariantByMeterID(string meterDataID)
        {
            string meterVariant = string.Empty;
            try
            {
                DataSet ds = new MeterMasterDAL().GetMeterVariantByMeterID(meterDataID);
                meterVariant = Convert.ToString(ds.Tables[0].Rows[0]["NetMeterVariantInfo"]);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterID(string meterDataID)", ex);

            }
            return meterVariant;
        }


        private string getRolloverValues(decimal currentValue, decimal lastValue)
        {
		    // Story - Analysis view taking time while opening by double click on search list
            if (ROLLOVERVALUE == -1)
            {
                ROLLOVERVALUE = new DLMS650GeneralBLL().GetRollOverValue(ConfigInfo.ActiveMeterDataId);

            }
            if (ROLLOVERVALUE > 0 && (currentValue < lastValue))
            {
                return (currentValue + (ROLLOVERVALUE - lastValue)).ToString();
            }
            else
            {
                return (currentValue - lastValue).ToString();
            }

        }
        /// <summary>
        /// Calculate PowerOffDuration in case of rollover.
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="lastValue"></param>
        /// <returns></returns>
        public ulong GetPowerOffHoursForRollOverData(ulong currentValue, ulong lastValue)
        {
            //Fixed rollover value for puma.
            ulong rollOverValue = 0xB964F000;
            rollOverValue = rollOverValue / 60;
            if (currentValue < lastValue)
            {
                return (currentValue + (rollOverValue - lastValue));
            }
            else
            {
                return (currentValue - lastValue);
            }

        }
        // Added for Billing Report
        public DataSet ConvertBillingDataToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            //foreach (DataColumn col in ds.Tables[0].Columns)
            //    table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("Meter_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("BillingDate", typeof(System.String)));
            table.Columns.Add(new DataColumn("SystemPowerFactorforBillingPeriod", typeof(System.String)));
            table.Columns.Add(new DataColumn("CumulativeEnergykWhTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("CumulativeEnergykvarhLag", typeof(System.String)));
            table.Columns.Add(new DataColumn("CumulativeEnergykvarhLead", typeof(System.String)));
            table.Columns.Add(new DataColumn("CumulativeEnergykVAhTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkWTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkWDateTimeTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkVATZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkVADateTimeTZ0", typeof(System.String)));

            // User Story - 1000867
            table.Columns.Add(new DataColumn("MDkVArLagTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkVArLagDateTimeTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkVArLeadTZ0", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDkVArLeadDateTimeTZ0", typeof(System.String)));

            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                try
                {
                    row = table.NewRow();

                    //string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                    //row[1] = val;
                    row[0] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[0])))).ToString();
                    row[1] = dataRow[1].ToString();
                    row[2] = dataRow[2].ToString();
                    string valBillingDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[5]));
                    row[3] = valBillingDate;
                    row[4] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[6])))).ToString();
                    row[5] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[7])))).ToString();
                    row[6] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[8])))).ToString();
                    row[7] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[9])))).ToString();
                    row[8] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[10])))).ToString();
                    row[9] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[11])))).ToString();
                    row[11] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[13])))).ToString();
                    string valMDkWTimestamp = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[12]));
                    row[10] = valMDkWTimestamp;
                    string valMDkVATimestamp = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[14]));
                    row[12] = valMDkVATimestamp;

                    // User Story - 1000867
                    row[13] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[11])))).ToString();
                    row[15] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[13])))).ToString();
                    string valMDkVArLagTimestamp = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[16]));
                    row[14] = valMDkVArLagTimestamp;
                    string valMDkVArLeadTimestamp = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[18]));
                    row[16] = valMDkVArLeadTimestamp;

                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertBillingDataToColumn(DataSet ds)", ex);
                }

            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;


        }
        // Added to get dynamic columns for billing report.
        public DataSet BillingDataToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();

            DataRow row;
            foreach (DataColumn col in ds.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            //int ColumnData = 1;
            foreach (DataRow row1 in ds.Tables[0].Rows)
            {
                //ColumnData = 1;
                row = table.NewRow();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string val = ds.Tables[0].Columns[i].ToString();
                    // Added to check the column names and then formatting the data.
                    if (val == "Consumer_Number")
                    {
                        row["Consumer_Number"] = row1[i].ToString();
                    }
                    if (val == "Consumer_Name")
                    {
                        row["Consumer_Name"] = row1[i];
                    }
                    if (val == "Meter_ID")
                    {
                        row["Meter_ID"] = row1[i];
                    }
                    if (val == "BillingDate")
                    {
                        string valBillingDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row1[i]));
                        row["BillingDate"] = valBillingDate;
                    }
                    if (val == "CumulativeEnergykWhTZ0")
                    {
                        row["CumulativeEnergykWhTZ0"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "CumulativeEnergykVAhTZ0")
                    {
                        row["CumulativeEnergykVAhTZ0"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "MDkVATZ0")
                    {
                        row["MDkVATZ0"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "MDkVADateTimeTZ0")
                    {
                        string valMDDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row1[i]));
                        row["MDkVADateTimeTZ0"] = valMDDate;
                    }
                    if (Convert.ToString(row1[i]).IndexOf('*') < 0 &&
                        (val == "Column1" || val == "Column2" || val == "Column3" || val == "Column4" || val == "Column5"))
                    {
                        string valMDDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row1[i]));
                        row[val] = valMDDate;
                        continue;
                    }
                    if (val == "Column1")
                    {
                        row["Column1"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "Column2")
                    {
                        row["Column2"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "Column3")
                    {
                        row["Column3"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                    if (val == "Column4")
                    {
                        row["Column4"] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(row1[i])))).ToString();
                    }
                }
                table.Rows.Add(row);


            }
            DataSet dataset = new DataSet();
            dataset.Tables.Add(table);
            return dataset;
        }

        /// <summary>
        /// Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="meterDataId"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        //public DataSet ConvertCumulativeEnergyToColumn(DataSet ds, long meterDataId, int meterModelNumber)
        //{
        //    if (ds == null)
        //        return null;
        //    if (ds.Tables.Count == 0)
        //        return null;
        //    if (ds.Tables[0].Rows.Count == 0)
        //        return null;

        //    string colName = string.Empty;
        //    DataTable table = new DataTable();
        //    //int emf = new MeterMasterBLL().GetEMF(meterDataId);

        //    table.Columns.Add(new DataColumn("History", typeof(System.String)));
        //    table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
        //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH), typeof(System.String)));
        //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH), typeof(System.String)));
        //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG), typeof(System.String)));
        //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD), typeof(System.String)));



        //    table.Columns.Add(new DataColumn(GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB, typeof(System.String))); // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        //    DataRow row;
        //    int count;
        //    foreach (DataRow dataRow in ds.Tables[0].Rows)
        //    {
        //        try
        //        {
        //            count = 0;
        //            row = table.NewRow();
        //            int rowVal = Convert.ToInt32(dataRow["DataIndex"]);
        //            if (rowVal == 0)
        //                row[0] = "Current";
        //            else
        //                row[0] = "History - " + rowVal.ToString();
        //            string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
        //            row[1] = val;
        //            string billingType = Convert.ToString(dataRow[6]);
        //            if (dataRow[2].ToString() != string.Empty)
        //                row[2] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[2])))).ToString();
        //            if (dataRow[3].ToString() != string.Empty)
        //                row[3] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[3])))).ToString();
        //            if (dataRow[4].ToString() != string.Empty)
        //                row[4] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[4])))).ToString();
        //            if (dataRow[5].ToString() != string.Empty)
        //                row[5] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[5])))).ToString();

        //            // Assigning value to Billing Type based on the Enum provided for WB 
        //            if (billingType == "255") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        //            {
        //                row[6] = "------";
        //            }
        //            else if (billingType == "0") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        //            {
        //                row[6] = "AUTO";
        //            }
        //            else if (billingType == "2") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        //            {
        //                row[6] = "MANUAL";
        //            }
        //            else if (billingType == "1") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        //            {
        //                // Commented the below meter model check for WBSDCL supply demand
        //                //row[6] = "SOFTWARE";
        //                row[6] = "COMMAND";
        //            }
        //            else if (billingType == "")
        //            {
        //                row[6] = "";
        //            }
        //            else
        //            {
        //                row[6] = billingType;
        //            }
        //            table.Rows.Add(row);
        //        }
        //        catch
        //        {
        //        }
        //    }

        //    colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH);
        //    if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
        //        table.Columns.Remove(colName);
        //    colName = string.Empty;
        //    colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH);
        //    if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
        //        table.Columns.Remove(colName);

        //    DataSet dataSet = new DataSet();
        //    dataSet.Tables.Add(table);
        //    return ApplyMultiplyFactor(meterDataId, dataSet);
        //}

        public DataSet ConvertCumulativeEnergyToColumn(DataSet ds, long meterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            string billingType = string.Empty;
            string colName = string.Empty;
            DataTable table = new DataTable();
            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                billingType = Convert.ToString(dataRow["BillingResetType"]);                
            }

            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD), typeof(System.String)));
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kWhLag"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag), typeof(System.String)));
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kWhLead"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead), typeof(System.String)));
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVAhLag"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag), typeof(System.String)));
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["kVAhLead"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead), typeof(System.String)));

            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CumulativeEnergyFraudkWh"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh), typeof(System.String)));
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CumulativeEnergyFraudkVAh"].ToString()))
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh), typeof(System.String)));
            int MeterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
            if (billingType.ToUpperInvariant().Contains("WB"))
            {
                table.Columns.Add(new DataColumn(GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB, typeof(System.String)));
            }
            else if (MeterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_WCM)
            {
                table.Columns.Add(new DataColumn(GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM, typeof(System.String)));
            }
            else
            {
                table.Columns.Add(new DataColumn(GlobalConstants.conCumulativeEnergyBILLINGTYPE, typeof(System.String)));
            }
           

            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhRPhase), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhYPhase), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhBPhase), typeof(System.String)));
            
            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet), typeof(System.String)));
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet), typeof(System.String)));
            }
            #region JDVVNL            
            
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase), typeof(System.String)));
            #endregion

            DataRow row;
            int count;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                try
                {
                    count = 0;
                    row = table.NewRow();
                    int rowVal = Convert.ToInt32(dataRow["DataIndex"]);
                    if (rowVal == 0)
                        row["History"] = "Current";
                    else
                        row["History"] = "History - " + rowVal.ToString();
                    string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                    row["Billing DateTime (0.0.0.1.2.255;3;2)"] = val;

                    billingType = Convert.ToString(dataRow["BillingResetType"]);

                    if (dataRow["CumulativeEnergykWhTZ0"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykWhTZ0"])))).ToString();
                    if (dataRow["CumulativeEnergykVAhTZ0"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykVAhTZ0"])))).ToString();
                    if (dataRow["CumulativeEnergykvarhLag"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLAG)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLag"])))).ToString();
                    if (dataRow["CumulativeEnergykvarhLead"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVARHLEAD)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLead"])))).ToString();
                    //******* for sapphire S2************
                    if (dataRow["kWhLag"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLag)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["kWhLag"])))).ToString();
                    if (dataRow["kWhLead"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKwhLead)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["kWhLead"])))).ToString();
                    if (dataRow["kVAhLag"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLag)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["kVAhLag"])))).ToString();
                    if (dataRow["kVAhLead"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consKVAhLead)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["kVAhLead"])))).ToString();


                    if (dataRow["CumulativeEnergykWhTZ0Import"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykWhTZ0Import"])))).ToString();      
                    if (dataRow["CumulativeEnergykVAhTZ0Import"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykVAhTZ0Import"])))).ToString();
                    if (dataRow["CumulativeEnergykWhTZ0Export"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykWhTZ0Export"])))).ToString();
                    if (dataRow["CumulativeEnergykVAhTZ0Export"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykVAhTZ0Export"])))).ToString();

                    if (dataRow["CumulativeEnergykvarhLagQ1"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLagQ1"])))).ToString();
                    if (dataRow["CumulativeEnergykvarhLeadQ4"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLeadQ4"])))).ToString();
                    if (dataRow["CumulativeEnergykvarhLagQ3"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLagQ3"])))).ToString();
                    if (dataRow["CumulativeEnergykvarhLeadQ2"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergykvarhLeadQ2"])))).ToString();
                    if (dataRow["CumEnergykWhRPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhRPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumEnergykWhRPhase"])))).ToString();
                    if (dataRow["CumEnergykWhYPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhYPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumEnergykWhYPhase"])))).ToString();
                    if (dataRow["CumEnergykWhBPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhBPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumEnergykWhBPhase"])))).ToString();

                    if (dataRow["CumulativeEnergyFraudkWh"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkWh)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergyFraudkWh"])))).ToString();
                    if (dataRow["CumulativeEnergyFraudkVAh"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.consCumulativeEnergyFraudkVAh)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["CumulativeEnergyFraudkVAh"])))).ToString();


                    if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                    {
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)] = GetNetValue(dataRow, "CumulativeEnergykWhTZ0", "CumulativeEnergykWhTZ0Export");
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)] = GetNetValue(dataRow, "CumulativeEnergykVAhTZ0", "CumulativeEnergykVAhTZ0Export");
                    }
                    //if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                    //{
                    //    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHNet)] = GetNetValue(dataRow, "CumulativeEnergykWhTZ0Import", "CumulativeEnergykWhTZ0Export");
                    //    row[CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHNet)] = GetNetValue(dataRow, "CumulativeEnergykVAhTZ0Import", "CumulativeEnergykVAhTZ0Export");
                    //}
                    #region JDVVNL
                    if (dataRow["MinimumVoltageLSIPAcrossDayRPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["MinimumVoltageLSIPAcrossDayRPhase"])))).ToString();

                    if (dataRow["MinimumVoltageLSIPAcrossDayYPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["MinimumVoltageLSIPAcrossDayYPhase"])))).ToString();

                    if (dataRow["MinimumVoltageLSIPAcrossDayBPhase"].ToString() != string.Empty)
                        row[CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase)] = (decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow["MinimumVoltageLSIPAcrossDayBPhase"])))).ToString();

                    #endregion

                    if (billingType.ToUpperInvariant().Contains("WB"))
                    {
                        billingType = billingType.Substring(0, billingType.ToUpperInvariant().IndexOf("_WB"));
                        // Assigning value to Billing Type based on the Enum provided for WB 
                        if (billingType == "255") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = "------";
                        }
                        else if (billingType == "0") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = "AUTO";
                        }
                        else if (billingType == "2") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = "MANUAL";
                        }
                        else if (billingType == "1") // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
                        {
                            // Commented the below meter model check for WBSDCL supply demand
                            //row[6] = "SOFTWARE";
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = "COMMAND";
                        }
                        else if (billingType == "")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = "";                            
                        }
                        else
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE_WB] = billingType;
                        }
                    }
                    else if ((MeterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_WCM))
                    {
                        // Assigning value to Billing Type based on the Enum provided for WB 
                        if (billingType == "0")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "------";
                        }
                        else if (billingType == "1")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "AUTO";
                        }
                        else if (billingType == "2")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "MANUAL";
                        }
                        else if (billingType == "3")
                        {
                            // Commented the below meter model check for WBSDCL supply demand
                            //row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "SOFTWARE";
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "COMMAND";
                        }
                        else if (billingType == "4")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "DIP Change";
                        }
                        else if (billingType == "5")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "New Firmware Upgrade";
                        }
                        else if (billingType == "6")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Metering Mode Change";
                        }
                        else if (billingType == "7")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Payment Mode Change";
                        }
                        else if (billingType == "8")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Manual";
                        }
                        else if (billingType == "")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "";
                        }
                        else
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = billingType;
                        }
                    }
                    else if ((MeterModelNumber == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNumber == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNumber == NamePlateConstants.SmartM_Cipher_WCM))
                    {
                        // Assigning value to Billing Type based on the Enum provided for WB 
                        if (billingType == "0")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "------";
                        }
                        else if (billingType == "1")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "AUTO";
                        }
                        else if (billingType == "2")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "MANUAL";
                        }
                        else if (billingType == "3")
                        {
                            // Commented the below meter model check for WBSDCL supply demand
                            //row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "SOFTWARE";
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "COMMAND";
                        }
                        else if (billingType == "4")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "DIP Change";
                        }
                        else if (billingType == "5")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "New Firmware Upgrade";
                        }
                        else if (billingType == "6")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Metering Mode Change";
                        }
                        else if (billingType == "7")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Payment Mode Change";
                        }
                        else if (billingType == "8")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "Manual";
                        }
                        else if (billingType == "")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = "";
                        }
                        else
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGRESETYPE_SM] = billingType;
                        }
                    }
                    else
                    {
                        // Assigning value to Billing Type based on the Enum provided for WB 
                        if (billingType == "0")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "------";
                        }
                        else if (billingType == "1")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "AUTO";
                        }
                        else if (billingType == "2")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "MANUAL";
                        }
                        else if (billingType == "3")
                        {
                            // Commented the below meter model check for WBSDCL supply demand
                            //row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "SOFTWARE";
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "COMMAND";
                        }
                        else if (billingType == "")
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = "";
                        }
                        else
                        {
                            row[GlobalConstants.conCumulativeEnergyBILLINGTYPE] = billingType;
                        }
                    }
                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertCumulativeEnergyToColumn(DataSet ds, long meterDataId)", ex);
                }
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHImport);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHImport);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHExport);
            if (table.Columns.Contains(colName))
            {                
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHExport);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColLagQ1);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ4);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWHLagQ3);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColVAHLeadQ2);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhRPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhYPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColkWhBPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayRPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayYPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.minimumVoltageLSIPAcrossDayBPhase);
            if (table.Columns.Contains(colName))
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                    table.Columns.Remove(colName);
            }

            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                table.Columns.Remove(colName);
            colName = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyVAH);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)))
                table.Columns.Remove(colName);

            // To remove Column having value "-------" in the DataTable
            int flag = 0;
            List<int> colIndex = new List<int>();
            for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
            {
                for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                {
                    if (!table.Rows[counterRow][counterCol].Equals("----"))
                    {
                        flag++;
                        break;
                    }
                    else
                        flag = 0;
                }
                if (flag == 0)
                    colIndex.Add(counterCol);
            }
            for (int counter = colIndex.Count - 1; counter >= 0; counter--)
            {
                table.Columns.RemoveAt(colIndex[counter]);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet);
        }      

        private string GetNetValue(DataRow dr, string ImportColumnName, string ExportColumnName)
        {
            string NetKWH_KVAH = "----";
            try
            {
                decimal CumulativeEnergykWhTZ0 = decimal.Parse(SplitWithOutUnit(Convert.ToString(dr[ImportColumnName])));
                decimal CumulativeEnergykWhTZ0Export = decimal.Parse(SplitWithOutUnit(Convert.ToString(dr[ExportColumnName])));
                NetKWH_KVAH = Convert.ToString(CumulativeEnergykWhTZ0 - CumulativeEnergykWhTZ0Export);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetNetValue(DataRow dr, string ImportColumnName, string ExportColumnName)", ex);
                
            }

            return NetKWH_KVAH;
        }
        /// <summary>
        /// Use to format the TOD column 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet ConvertTODDetailsToColumn(DataSet ds, long meterDataId)
        {
            if (ds == null && ds.Tables.Count == 0 && ds.Tables[0].Rows.Count == 0)
                return null;

            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                try
                {
                    row = table.NewRow();
                    int rowVal = Convert.ToInt32(dataRow["DataIndex"]);
                    if (rowVal == 0)
                        row[0] = "Current";
                    else
                        row[0] = "History - " + rowVal.ToString();
                    string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                    row[1] = val;
                    row[2] = rowVal; ;
                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertTODDetailsToColumn(DataSet ds, long meterDataId)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
            //return ApplyMultiplyFactor(meterDataId, dataSet);
        }
        /// <summary>
        /// Use to format the TOD Details column 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet ConvertTODHistoryDetailsToColumn(DataSet ds, long meterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataTable table = new DataTable();
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);           
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime (0.0.0.1.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            for (int counter = 0; counter < ds.Tables[0].Rows.Count; counter++)
            {
                try
                {
                    row = table.NewRow();
                    string val = "";
                    if (counter == 0)
                        val = "Current - History - 1";
                    else
                    {
                        if (counter == ((ds.Tables[0].Rows.Count) - 1))
                            break;
                        val = "History - " + counter.ToString() + " - History - " + (counter + 1).ToString();
                    }
                    row[0] = val;
                    int rowVal = Convert.ToInt32(ds.Tables[0].Rows[counter]["DataIndex"]);
                    val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(ds.Tables[0].Rows[counter][1]));
                    row[1] = val;
                    row[2] = rowVal; ;
                    table.Rows.Add(row);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertTODHistoryDetailsToColumn(DataSet ds, long meterDataId)", ex);
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
            //return ApplyMultiplyFactor(meterDataId, dataSet);        
        }
        private string SplitWithOutUnit(string data)
        {
            string value = data;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                value = val[0];
            }
            return value.Replace("000000.00", "0.000");
        }
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
            return value;
        }

        private DataTable GetLSTableColumns(string type, DataSet dSet)
        {

            DataTable table = new DataTable();
            bool energykWPresent = false;
            bool energyKVAPresent = false;

            bool ExportenergykWPresent = false;//pks
            bool ExportenergyKVAPresent = false;//pks
            bool energykWExportPresent = false;
            bool energykVAExportPresent = false;
            bool energykWImportPresent = false;
            bool energykVAImportPresent = false;
            bool isAverageCurrentPresent = false;

            table.Columns.Add(new DataColumn("Real Time Clock - Date and Time (0.0.1.0.0.255;8;2)"));
            for (int colCount = 1; colCount < dSet.Tables[0].Columns.Count; colCount++)
            {
                string columnName = dSet.Tables[0].Columns[colCount].ColumnName;
                if (columnName.ToUpper().Contains("RPHASECURRENT"))
                {
                    table.Columns.Add(new DataColumn("Current - IR (1.0.31.27.0.255;3;2)"));
                }
                else if (columnName.Contains("AvgNeutralCurrent"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSneucurrent)));
                }

                else if (columnName.ToUpper().Contains("NEUTRALCURRENT"))//ADD pradipta_load_neu   //SarkarA code change 20180322// fix
                {
                    table.Columns.Add(new DataColumn("NeutralCurrent - (1.0.91.7.0.255;3;2)"));
                }

                else if (columnName.ToUpper().Contains("AVGPHASECURRENT"))//ADD in smart meter 1 phase
                {
                    table.Columns.Add(new DataColumn("AvgPhaseCurrent (1.0.128.27.0.255;3;2)"));
                }

                else if (columnName.ToUpper().Contains("YPHASECURRENT"))
                {
                    table.Columns.Add(new DataColumn("Current - IY (1.0.51.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("BPHASECURRENT"))
                {
                    table.Columns.Add(new DataColumn("Current - IB (1.0.71.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("AVERAGECURRENT"))
                {
                    isAverageCurrentPresent = true;
                    table.Columns.Add(new DataColumn("Average Current (1.0.11.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("RPHASEVOLTAGE"))
                {
                    table.Columns.Add(new DataColumn("Voltage - VRN (1.0.32.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("YPHASEVOLTAGE"))
                {
                    table.Columns.Add(new DataColumn("Voltage - VYN (1.0.52.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("BPHASEVOLTAGE"))
                {
                    table.Columns.Add(new DataColumn("Voltage - VBN (1.0.72.27.0.255;3;2)"));
                }
                else if (columnName.ToUpper().Contains("AVERAGEVOLTAGE"))
                {
                    table.Columns.Add(new DataColumn("Average Voltage (1.0.12.27.0.255;3;2)"));
                }
                else if (columnName.Contains("blockEnergykWhImport"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWImport)));
                    }
                    else
                    {
                        energykWImportPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHImport)));
                    }
                }
                else if (columnName.Contains("blockEnergykWhExport"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)));
                    }
                    else
                    {
                        energykWExportPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)));
                    }
                }

                else if (columnName.Contains("blockEnergykWhRPhase"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWRPhase)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHRPhase)));
                    }
                }
                else if (columnName.Contains("blockEnergykWhYPhase"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWYPhase)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHYPhase)));
                    }
                }
                else if (columnName.Contains("blockEnergykWhBPhase"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWBPhase)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHBPhase)));
                    }
                }

                else if (columnName.Contains("blockEnergykWh"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhlagQ1"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLagQ1)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLagQ1)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhlagQ3"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLagQ3)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLagQ3)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhlag"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLag)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLag)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhleadQ4"))

                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLeadQ4)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLeadQ4)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhleadQ2"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLeadQ2)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLeadQ2)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhlead"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLead)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLead)));
                    }
                }
                else if (columnName.Contains("blockEnergykVAhImport"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAImport)));
                    }
                    else 
                    {
                        energykVAImportPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHImport)));
                    }
                }
                else if (columnName.Contains("blockEnergykVAhExport"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)));
                    }
                    else
                    {
                        energykVAExportPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)));
                    }
                }
                else if (columnName.Contains("blockEnergykVAh"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)));
                    }
                    else
                    {
                        energyKVAPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)));
                    }
                }               

                else if (columnName.Contains("blockEnergykvarhQ12"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandkvarhQ12)));
                    }
                    else
                    {
                        energyKVAPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergykvarhQ12)));
                    }
                }

                else if (columnName.Contains("blockEnergykvarhQ34"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandkvarhQ34)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergykvarhQ34)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhQ14"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandkvarhQ14)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergykvarhQ14)));
                    }
                }
                else if (columnName.Contains("blockEnergykvarhQ23"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandkvarhQ23)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergykvarhQ23)));
                    }
                }

                else if (columnName.Contains("blockEnergyFundamentalkWhAbsolute"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandFundamentalkWhAbsolute)));
                    }
                    else
                    {
                        energykWPresent = true;
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyFundamentalkWhAbsolute)));
                    }
                }

                else if (columnName.Contains("NetkWh"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSDemandNetkWh)));
                        
                    }
                    else
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkWh)));
                    }
                    
                }
                else if (columnName.Contains("NetkVAh"))
                {
                    if (type.Equals("Demand"))
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSDemandNetkVAh)));

                    }
                    else
                    {
                        table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyNetkVAh)));
                    }
                    
                }
                else if (columnName.ToLower().Contains("frequency"))
                {
                    table.Columns.Add(new DataColumn("Frequency - Hz (1.0.14.27.0.255;3;2)"));
                }
                else if (columnName.Contains("tamperStatus"))
                {
                    table.Columns.Add(new DataColumn("Tamper Status (0.0.96.1.152.255;1;2)"));
                }

                // BRPL parameters
                else if (columnName.Contains("reactivePowerRPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSReactivePowerRPhaseKVAr)));
                }
                else if (columnName.Contains("reactivePowerYPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSReactivePowerYPhaseKVAr)));
                }
                else if (columnName.Contains("reactivePowerBPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSReactivePowerBPhaseKVAr)));
                }
                else if (columnName.Contains("activePowerRPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSActivePowerRPhaseKW)));
                }
                else if (columnName.Contains("activePowerYPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSActivePowerYPhaseKW)));
                }
                else if (columnName.Contains("activePowerBPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSActivePowerBPhaseKW)));
                }
                else if (columnName.Contains("apparentPowerRPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSApparentPowerRPhaseKVA)));
                }
                else if (columnName.Contains("apparentPowerYPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSApparentPowerBPhaseKVA)));
                }
                else if (columnName.Contains("apparentPowerBPhase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSApparentPowerBPhaseKVA)));
                }
                else if (columnName.Contains("powerOffDurationLSIP"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSpowerOffDurationLSIP)));
                }
                else if (columnName.Contains("temperature"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTemperature)));
                }
                else if (columnName.Contains("tamperflag"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTamperflag)));
                }
                else if (columnName.Contains("Avgvoltageof3phase"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSAvgvolt3ph)));
                }
                else if (columnName.Contains("AvgRphasePF"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSAvgRphPF)));
                }
                else if (columnName.Contains("AvgYphasePF"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSAvgYphPF)));
                }
                else if (columnName.Contains("AvgBphasePF"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSAvgBphPF)));
                }
                else if (columnName.Contains("AvgTotalPF"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSAvgTotalPF)));
                }

               
                else if (columnName.Contains("THDVR"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDVR)));
                }
                else if (columnName.Contains("THDVY"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDVY)));
                }
                else if (columnName.Contains("THDVB"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDVB)));
                }
                else if (columnName.Contains("THDIR"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDIR)));
                }
                else if (columnName.Contains("THDIY"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDIY)));
                }
                else if (columnName.Contains("THDIB"))
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTHDIB)));
                }

                ////SB Change Start 20170828
                //else if (columnName.Equals("temperature", StringComparison.InvariantCultureIgnoreCase))// && MeterModelNoForTmp == 11)
                //{
                //    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSTemperature)));
                //}
                ////SB Change End 20170828

            }
            //for (int EDcolCount = 1; EDcolCount < dSet.Tables[0].Columns.Count; EDcolCount++)
            //{
            //    string EDColumnName = dSet.Tables[0].Columns[EDcolCount].ColumnName;
            //    if (type.Equals("Demand"))
            //    {
            //        if (EDColumnName.Contains("blockEnergykWh"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykvarhlag"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLag)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykvarhlead"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVARLead)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykVAh"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)));
            //        }
            //    }
            //    else
            //    {
            //        if (EDColumnName.Contains("blockEnergykWh"))
            //        {
            //            energykWPresent = true;
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykvarhlag"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLag)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykvarhlead"))
            //        {
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVARHLead)));
            //        }
            //        else if (EDColumnName.Contains("blockEnergykVAh"))
            //        {
            //            energyKVAPresent = true;
            //            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)));
            //        }
            //    }
            //    //added PUMA
            //    if (EDColumnName.Contains("frequency"))
            //    {
            //        table.Columns.Add(new DataColumn("Frequency - Hz (1.0.14.27.0.255;3;2)"));
            //    }
            //    else if (EDColumnName.Contains("tamperStatus"))
            //    {
            //        table.Columns.Add(new DataColumn("Tamper Status (0.0.96.1.152.255;1;2)"));
            //    }
            //}
            /* VBM - Make Avg power factor configurable in LS */
            if (energykWPresent && energyKVAPresent)
            {
                table.Columns.Add(new DataColumn("Import Power Factor"));////pks_LS

            }
            if (energykWExportPresent && energykVAExportPresent)//pks_LS
            {
                table.Columns.Add(new DataColumn("Export Power Factor"));

            }


            return table;
        }
        // Fix by Swati
        // Added to remove units from Midnight data parameters.
        public DataSet ConvertMidnightData(long meterDataId, DataSet dataSet, int resolution)
        {
            DTMDailyProfileParameterEntity midnightParameterEntity = midnightParameterBLL.GetColumn(meterDataId) as DTMDailyProfileParameterEntity;
            try
            {

                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();
                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(dataSet.Tables[0].Columns[colCount].ToString()));
                }
                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    DataRow nRow = table.NewRow();
                    if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                        nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    else
                        nRow[0] = "-------";
                    for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {
                        // Changed Cu to Daily to solve DLMS_110.
                        if (!dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Daily-kWh (1.0.1.29.0.255;3;2)") &&
                            !dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Date") && !dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Daily-kVAh (1.0.9.29.0.255;3;2)")
                            && !dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Daily-kvarh lag (1.0.5.29.0.255;3;2)") && !dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Daily-kvarh lead (1.0.8.29.0.255;3;2)"))
                        {
                            // Checking null values.
                            if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])))
                            {
                                nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0];
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])))
                                nRow[colCount] = Convert.ToDouble(dataSet.Tables[0].Rows[rowCount][colCount]).ToString("F" + resolution.ToString());
                            else
                                nRow[colCount] = "-------";
                        }


                    }
                    table.Rows.Add(nRow);
                }
                dataSet = new DataSet();
                dataSet.Tables.Add(table);

                // Added to solve bug 73406.
                dataSet = ApplyMultiplyFactor(meterDataId, dataSet);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertMidnightData(long meterDataId, DataSet dataSet, int resolution)", ex);
                throw ex;
            }
            return dataSet;

        }
        public DataSet ConvertPUMAMidnightData(long meterDataId, DataSet dataSet)
        {
            try
            {
                decimal kWh = 0;
                decimal nextKwh = 0;
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();

                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(dataSet.Tables[0].Columns[colCount].ToString())));
                }

                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //skip the first row and do not enter the loop if rowcount < 2.

                    DataRow nRow = table.NewRow();
                    if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                    {
                        nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    }
                    else
                    {
                        //dataSet.Tables[0].Rows.RemoveAt(rowCount);
                        //dataSet.AcceptChanges();
                        //continue;
                        nRow[0] = "-------";
                    }

                    for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {
                        // Changed Cu to Daily to solve DLMS_110.
                        string dataVal = Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]);


                        #region "Net Calculation"
                        if (dataSet.Tables[0].Columns[colCount].ColumnName.ToUpper().Contains("NET - {0}WH"))
                        {
                            try
                            {
                                decimal NetKWHValue = 0;
                                decimal CumKWHValue = 0;
                                decimal CumKWHExportValue = 0;
                                string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);

                                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                                {
                                    CumKWHExportValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)"]))[0]);
                                    CumKWHValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}Wh Import (1.0.1.8.0.255;3;2)"]))[0]);//pks
                                    NetKWHValue = CumKWHValue - CumKWHExportValue;
                                    nRow[colCount] = NetKWHValue;
                                }
                                //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                                //{
                                //    CumKWHExportValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)"]))[0]);
                                //    CumKWHValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}Wh Import (1.0.143.128.128.255;3;2)"]))[0]);

                                //    NetKWHValue = CumKWHValue - CumKWHExportValue;
                                //    nRow[colCount] = NetKWHValue;
                                //}
                                else
                                {
                                    nRow[colCount] = "-------";
                                }
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                logger.Log(LOGLEVELS.Error, "ConvertPUMAMidnightData(long meterDataId, DataSet dataSet)", ex);
                                nRow[colCount] = "-------";
                            }

                        }
                        else if (dataSet.Tables[0].Columns[colCount].ColumnName.ToUpper().Contains("NET - {0}VAH"))
                        {
                            try
                            {
                                string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);
                                decimal NetKVAHValue = 0;
                                decimal CumKVAHValue = 0;
                                decimal CumKVAHExportValue = 0;
                                if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                                {
                                    CumKVAHExportValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)"]))[0]);
                                    CumKVAHValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)"]))[0]);
                                    NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                                    nRow[colCount] = NetKVAHValue;
                                }
                                //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                                //{
                                //    CumKVAHExportValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)"]))[0]);
                                //    CumKVAHValue = Convert.ToDecimal(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount]["Cumulative Energy - {0}VAh Import (1.0.144.128.128.255;3;2)"]))[0]);
                                //    NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                                //    nRow[colCount] = NetKVAHValue;
                                //}
                                else
                                {
                                    nRow[colCount] = "-------";
                                }
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                logger.Log(LOGLEVELS.Error, "ConvertPUMAMidnightData(long meterDataId, DataSet dataSet)", ex);
                                nRow[colCount] = "-------";
                            }
                        }
                        #endregion
                        // Checking null values.
                        else if (!string.IsNullOrEmpty(dataVal))
                        {

                            if (dataVal.Contains("*"))
                            {
                                if (decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataVal))[0], out kWh))
                                {
                                    nRow[colCount] = kWh;
                                }
                                else
                                {
                                    nRow[colCount] = "-------";
                                }

                            }
                            else
                            {
                                nRow[colCount] = dataVal;

                            }
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }



                    }
                    table.Rows.Add(nRow);
                }
                int flag = 0;
                List<int> colIndex = new List<int>();
                for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
                {
                    for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                    {
                        if (!table.Rows[counterRow][counterCol].Equals("-------"))
                        {
                            flag++;
                            break;
                        }
                        else
                            flag = 0;
                    }
                    if (flag == 0)
                        colIndex.Add(counterCol);
                }
                for (int counter = colIndex.Count - 1; counter >= 0; counter--)
                {
                    table.Columns.RemoveAt(colIndex[counter]);
                }
                //foreach (DataColumn dataColumn in table.Columns)
                //{
                //    foreach (DataRow dataRow in table.Rows)
                //    {
                //        if (!dataRow[dataColumn].Equals("-------"))
                //        {
                //            flag++;
                //            break;
                //        }
                //        else
                //            flag = 0;

                //    }
                //    if (flag == 0)
                //        table.Columns.Remove(dataColumn);
                //}
                dataSet = new DataSet();
                dataSet.Tables.Add(table);

                // Added to solve bug 73406.
                dataSet = ApplyMultiplyFactor(meterDataId, dataSet);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertPUMAMidnightData(long meterDataId, DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }
        public DataSet ConvertPUMADailyConsumption(long meterDataId, DataSet dataSet)
        {
            try
            {
                decimal kWh = 0;
                decimal nextKwh = 0;
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();

                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(dataSet.Tables[0].Columns[colCount].ToString()));
                }
                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //skip the first row and do not enter the loop if rowcount < 2.
                    if ((dataSet.Tables[0].Rows.Count < 2) || rowCount == 0)
                    {
                        continue;
                    }
                    DataRow nRow = table.NewRow();
                    //if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                    //{
                    //    nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    //}
                    //else
                    //{
                    //    nRow[0] = "-------";                        
                    //}
                    for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {

                        if (colCount == 0)
                        {
                            nRow[colCount] = getDateText(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]), Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]));
                            continue;
                        }
                        // Changed Cu to Daily to solve DLMS_110.

                        // Checking null values.
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])) && !string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount])))
                        {
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]))[0], out kWh);
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0], out nextKwh);
                            if (true)
                            {
                                nRow[colCount] = getRolloverValues(kWh, nextKwh);
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }



                    }
                    table.Rows.Add(nRow);
                }
                
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    //if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                    //{
                    //    object poweronValue = table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"];
                    //    if (poweronValue != null && (! poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                    //    {                         
                    //        table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                    //        TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"])));
                    //    }
                    //}

                    if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))//pradipta_pow
                    {
                        object poweronValue = table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"];
                        if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                        {
                            table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"])));
                        }
                    }
                }  
               
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    //if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"))
                    //{
                    //    object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"];
                    //    if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                    //    {
                    //        table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                    //        TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));
                    //    }
                    //}
                    if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))
                    {
                        object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"];
                        if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                        {
                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"])));
                        }
                    }

                    //else if (table.Columns.Contains("Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"))
                    //{
                    //    object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"];
                    //    if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                    //    {
                    //        table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                    //        TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"])));
                    //    }
                    //}

                    else if (table.Columns.Contains("Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"))//pradipta_pow
                    {
                        object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"];
                        if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                        {
                            table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"])));
                        }
                    }
                }    


                //------------------
                int counter = 0;
                List<int> colIndex = new List<int>();
                for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
                {
                    for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                    {
                        if (!table.Rows[counterRow][counterCol].Equals("-------"))
                        {
                            counter++;
                            break;
                        }
                        else
                            counter = 0;
                    }
                    if (counter == 0)
                        colIndex.Add(counterCol);
                }
                for (int counterIndex = colIndex.Count - 1; counterIndex >= 0; counterIndex--)
                {
                    table.Columns.RemoveAt(colIndex[counterIndex]);
                }

                dataSet = new DataSet();
                dataSet.Tables.Add(table);
                // Added to solve bug 73406.
                dataSet = ApplyMultiplyFactor(meterDataId, dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertPUMADailyConsumption(long meterDataId, DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }
        /// <summary>
        /// Used to calculate daily consumption for egneric BCS .
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ConvertGenericDailyConsumption(long meterDataId, DataSet dataSet)
        {
            try
            {
                decimal kWh = 0;
                decimal nextKwh = 0;
                //decimal powerOnDurationcurrent = 0;
                //decimal powerOnDurationlatest = 0;

                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();

                
                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(dataSet.Tables[0].Columns[colCount].ToString())));

                }

                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //skip the first row and do not enter the loop if rowcount < 2.
                    if ((dataSet.Tables[0].Rows.Count < 2) || rowCount == 0)
                    {
                        continue;
                    }
                    DataRow nRow = table.NewRow();
                    //if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                    //{
                    //    nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    //}
                    //else
                    //{
                    //    nRow[0] = "-------";                        
                    //}
                    for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {

                        if (colCount == 0)
                        {
                            nRow[colCount] = getDateText(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]), Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]));
                            continue;
                        }
                        // Changed Cu to Daily to solve DLMS_110.


                        // Checking null values.
                        else if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])) && !string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount])))
                        {
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]))[0], out kWh);
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0], out nextKwh);
                            // The below code is commented because this condition will always be in true state so what is the use of else statement.
                            //if (true)
                            //{
                            //    nRow[colCount] = getRolloverValues(kWh, nextKwh);
                            //}
                            //else
                            //{
                            //    nRow[colCount] = "-------";
                            //}  
                            //if (dataSet.Tables[0].Columns.Contains("(1.0.94.91.13.255;3;2)"))
                            //{
                            //    nRow[colCount] = kWh;
                            //}

                            //JDVVNL 527250
                            string PowerOndurationFormat = string.Empty;
                            string PowerOffdurationFormat = string.Empty;
                            string PowerOn3PhasesdurationFormat = string.Empty;
                            if (ConfigSettings.GetValue("ChkPowerOnOffDurationFormat") == "1")
                            {
                                PowerOndurationFormat = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh";
                            }
                            else
                            {
                                if (dataSet.Tables[0].Columns.Count > 5)
                                {
                                    //if (dataSet.Tables[0].Columns[5].ColumnName == "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm")
                                        if (dataSet.Tables[0].Columns[5].ColumnName == "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm")
                                    {
                                       // PowerOndurationFormat = "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm";
                                        //PowerOffdurationFormat = "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm";
                                        //PowerOn3PhasesdurationFormat = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm";

                                        PowerOndurationFormat = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm";//pradipta_pow
                                        PowerOffdurationFormat = "Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm";
                                        PowerOn3PhasesdurationFormat = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm";


                                        if (dataSet.Tables[0].Columns[colCount].ColumnName == PowerOndurationFormat || dataSet.Tables[0].Columns[colCount].ColumnName == PowerOffdurationFormat || dataSet.Tables[0].Columns[colCount].ColumnName == PowerOn3PhasesdurationFormat)
                                        {
                                            nRow[colCount] = kWh;
                                        }
                                        else
                                        {
                                            nRow[colCount] = getRolloverValues(kWh, nextKwh);
                                        }

                                        //if (dataSet.Tables[0].Columns[colCount].ColumnName == PowerOffdurationFormat)
                                        //{
                                        //    nRow[colCount] = kWh;
                                        //}
                                        //else
                                        //{
                                        //    nRow[colCount ] = getRolloverValues(kWh, nextKwh);
                                        //}

                                        //if (dataSet.Tables[0].Columns[colCount].ColumnName == PowerOn3PhasesdurationFormat)
                                        //{
                                        //    nRow[colCount] = kWh;
                                        //}
                                        //else
                                        //{
                                        //    nRow[colCount] = getRolloverValues(kWh, nextKwh);
                                        //}
                                    }
                                    else  nRow[colCount] = getRolloverValues(kWh, nextKwh);
                                }
                                else
                                {
                                    nRow[colCount] = getRolloverValues(kWh, nextKwh);
                                }
                                
                            }
                           


                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }
                    }
                    table.Rows.Add(nRow);
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh"))
                    {

                        table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh"] = ConvertTimSpanToDDDDHH(
                                    TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh"])));

                    }
                    //if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                    //{
                    //    table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                    //             TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"])));
                    //}

                    if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))//pradipta_pow
                    {
                        table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                 TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"])));
                    }

                    if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"))
                    {

                        int powerOnDuration = Convert.ToInt32(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"]);
                        if (powerOnDuration >= 0)
                        {
                            //table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //         TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));

                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"] = ConvertTimSpanToDDDDHH(
                                     TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"])));
                        }
                        else
                        {
                           // table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = "-------";
                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = "-------";//pradipta_pow
                        }
                    }
                   // else if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"))
                    else if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))
                    {
                        //int powerOnDuration = Convert.ToInt32(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"]);
                        int powerOnDuration = Convert.ToInt32(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"]);//pradipta_pow
                        if (powerOnDuration >= 0)
                        {
                            //table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //         TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));

                            //table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //         TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));

                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                     TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"])));

                            //for power on 3 phases duration 
                            if (table.Columns.Contains("Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"))//dd
                            {
                                    // table.Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                    //TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm"])));
                                     table.Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                     TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"])));
                            }

                            //for power on 3 phses duration
                            //if (table.Columns.Contains("Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm"))
                            //{
                            //    table.Rows[i]["Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm"])));
                            //}

                            if (table.Columns.Contains("Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"))
                            {
                                table.Rows[i]["Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"])));
                            }
                            //table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                  // TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"])));
                             
                                   
                        }
                        else
                        {
                           // table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = "-------";
                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = "-------";
                        }
                    }

                    //else if (table.Columns.Contains("Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"))
                    else if (table.Columns.Contains("Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"))
                    {
                        //int powerOnDuration = Convert.ToInt32(table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"]);
                        int powerOnDuration = Convert.ToInt32(table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"]);
                        if (powerOnDuration >= 0)
                        {
                            //table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //        TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));
                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                                    TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"])));

                            //  table.Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //TimeSpan.FromMinutes(Convert.ToUInt64(table.Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dd:hh:mm"])));
                        }
                        else
                        {
                            //table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"] = "-------";
                            table.Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"] = "-------";
                        }
                    }
                   

                   

                    //end

                    #region "Net Calculation"
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}WH")))
                    {
                        DataColumn clmNETKWH = new DataColumn();
                        try
                        {
                            clmNETKWH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}WH"))];
                            decimal NetKWHValue = 0;
                            decimal CumKWHValue = 0;
                            decimal CumKWHExportValue = 0;
                            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);

                            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                            {
                                CumKWHExportValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)")]);
                                CumKWHValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import(1.0.1.8.0.255;3;2)")]);//pks
                                NetKWHValue = CumKWHValue - CumKWHExportValue;
                                table.Rows[i][clmNETKWH.ColumnName] = NetKWHValue;
                            }
                            //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                            //{
                            //    CumKWHExportValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)")]);
                            //    CumKWHValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import (1.0.143.128.128.255;3;2)")]);

                            //    NetKWHValue = CumKWHValue - CumKWHExportValue;
                            //    table.Rows[i][clmNETKWH.ColumnName] = NetKWHValue;
                            //}
                            else
                            {
                                table.Rows[i][clmNETKWH.ColumnName] = "-------";
                            }
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "ConvertGenericDailyConsumption(long meterDataId, DataSet dataSet)", ex);
                            table.Rows[i][clmNETKWH.ColumnName] = "-------";
                        }

                    }
                    if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}VAH")))
                    {
                        DataColumn clmNETKVAH = new DataColumn();
                        try
                        {
                            clmNETKVAH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}VAH"))];
                            string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);
                            decimal NetKVAHValue = 0;
                            decimal CumKVAHValue = 0;
                            decimal CumKVAHExportValue = 0;
                            if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                            {
                                CumKVAHExportValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)")]);
                                CumKVAHValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)")]);
                                NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                                table.Rows[i][clmNETKVAH.ColumnName] = NetKVAHValue;
                            }
                            //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                            //{
                            //    CumKVAHExportValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)")]);
                            //    CumKVAHValue = Convert.ToDecimal(table.Rows[i][CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Import (1.0.144.128.128.255;3;2)")]);
                            //    NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                            //    table.Rows[i][clmNETKVAH.ColumnName] = NetKVAHValue;
                            //}
                            else
                            {
                                table.Rows[i][clmNETKVAH.ColumnName] = "-------";
                            }
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "ConvertGenericDailyConsumption(long meterDataId, DataSet dataSet)", ex);
                            table.Rows[i][clmNETKVAH.ColumnName] = "-------";
                        }
                    }
                    #endregion

                }

                // To remove Column having value "-------" in the DataTable
                int counter = 0;
                List<int> colIndex = new List<int>();
                for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
                {
                    for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                    {
                        if (!table.Rows[counterRow][counterCol].Equals("-------"))
                        {
                            counter++;
                            break;
                        }
                        else
                            counter = 0;
                    }
                    if (counter == 0)
                        colIndex.Add(counterCol);
                }
                for (int counterIndex = colIndex.Count - 1; counterIndex >= 0; counterIndex--)
                {
                    table.Columns.RemoveAt(colIndex[counterIndex]);
                }

                table.AcceptChanges();
                dataSet = new DataSet();
                dataSet.Tables.Add(table);
                // Added to solve bug 73406.
                dataSet = ApplyMultiplyFactor(meterDataId, dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertGenericDailyConsumption(long meterDataId, DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }

        public DataSet ConvertPUMADailyConsumption(DataSet dataSet)
        {
            try
            {
                decimal kWh = 0;
                decimal nextKwh = 0;
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();

                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(dataSet.Tables[0].Columns[colCount].ToString()));
                }
                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //skip the first row and do not enter the loop if rowcount < 2.
                    if ((dataSet.Tables[0].Rows.Count < 2) || rowCount == 0)
                    {
                        continue;
                    }
                    DataRow nRow = table.NewRow();
                    //if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                    //{
                    //    nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    //}
                    //else
                    //{
                    //    nRow[0] = "-------";                        
                    //}
                    for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {

                        if (colCount == 0)
                        {
                            nRow[colCount] = getDateText(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]), Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]));
                            continue;
                        }
                        // Changed Cu to Daily to solve DLMS_110.

                        // Checking null values.
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])) && !string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount])))
                        {
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]))[0], out kWh);
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0], out nextKwh);
                            if (true)
                            {
                                nRow[colCount] = getRolloverValues(kWh, nextKwh);
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }



                    }
                    table.Rows.Add(nRow);
                }
                int counter = 0;
                List<int> colIndex = new List<int>();
                for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
                {
                    for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                    {
                        if (!table.Rows[counterRow][counterCol].Equals("-------"))
                        {
                            counter++;
                            break;
                        }
                        else
                            counter = 0;
                    }
                    if (counter == 0)
                        colIndex.Add(counterCol);
                }
                for (int counterIndex = colIndex.Count - 1; counterIndex >= 0; counterIndex--)
                {
                    table.Columns.RemoveAt(colIndex[counterIndex]);
                }

                dataSet = new DataSet();
                dataSet.Tables.Add(table);
                // Added to solve bug 73406.
                //dataSet = ApplyMultiplyFactor(meterDataId, dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertPUMADailyConsumption(DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }

        private string getDateText(string currentDate, string previousDate)
        {
            if (string.IsNullOrEmpty(currentDate))
            {
                currentDate = "--------";
            }
            if (string.IsNullOrEmpty(previousDate))
            {
                previousDate = "--------";
            }
            return string.Concat(previousDate, " - ", currentDate);
        }

        public DataSet ConvertMidnightEnergy(DataSet dataSet)
        {
            try
            {
                decimal kWh = 0;
                decimal nextKwh = 0;
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();

                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    table.Columns.Add(new DataColumn(dataSet.Tables[0].Columns[colCount].ToString()));
                }
                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //skip the first row and do not enter the loop if rowcount < 2.
                    if ((dataSet.Tables[0].Rows.Count < 2) || rowCount == 0)
                    {
                        continue;
                    }
                    DataRow nRow = table.NewRow();
                    //if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                    //{
                    //    nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    //}
                    //else
                    //{
                    //    nRow[0] = "-------";                        
                    //}
                    for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {

                        if (colCount == 0)
                        {
                            nRow[colCount] = getDateText(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]), Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]));
                            continue;
                        }
                        // Changed Cu to Daily to solve DLMS_110.

                        // Checking null values.
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])) && !string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount])))
                        {
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount - 1][colCount]))[0], out kWh);
                            decimal.TryParse(CheckPUMAUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0], out nextKwh);
                            if (true)
                            {
                                nRow[colCount] = getRolloverValues(kWh, nextKwh);
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }



                    }
                    table.Rows.Add(nRow);
                }
                int counter = 0;
                List<int> colIndex = new List<int>();
                for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
                {
                    for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                    {
                        if (!table.Rows[counterRow][counterCol].Equals("-------"))
                        {
                            counter++;
                            break;
                        }
                        else
                            counter = 0;
                    }
                    if (counter == 0)
                        colIndex.Add(counterCol);
                }
                for (int counterIndex = colIndex.Count - 1; counterIndex >= 0; counterIndex--)
                {
                    table.Columns.RemoveAt(colIndex[counterIndex]);
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))
                    //if (table.Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                    {
                        //object poweronValue = table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"];
                        object poweronValue = table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"];
                        if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                        {
                            //table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"])));
                            table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"])));
                        }
                    }
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))//dd:hh:mm
                    {
                        //object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"];
                        object poweronValue = table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"];
                        if (poweronValue != null && (!poweronValue.ToString().Contains("-----") && poweronValue.ToString() != string.Empty))
                        {
                            //table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            //TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"])));
                            table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(
                            TimeSpan.FromSeconds(Convert.ToUInt64(table.Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"])));
                        }
                    }
                }  

                dataSet = new DataSet();
                dataSet.Tables.Add(table);
                // Added to solve bug 73406.
                //dataSet = ApplyMultiplyFactor(meterDataId, dataSet);

                string colName = "";
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    colName = col.ColumnName;
                    if (colName.ToUpper().Contains("DATE"))
                    {
                        table.Columns[colName].ColumnName = "Date (0.0.1.0.0.255;8;2)";
                    }
                    else if (colName.ToUpper().Contains("WH"))
                    {
                        table.Columns[colName].ColumnName = "kWh Import(1.0.1.8.0.255;3;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("LAG"))
                    {
                        table.Columns[colName].ColumnName = "kvarh (lag) (1.0.5.8.0.255;3;2)";
                    }
                    else if (colName.ToUpper().Contains("LEAD"))
                    {
                        table.Columns[colName].ColumnName = "kvarh (lead) (1.0.8.8.0.255;3;2)";
                    }
                    else if (colName.ToUpper().Contains("VAH"))
                    {
                        table.Columns[colName].ColumnName = "kVAh Import(1.0.9.8.0.255;3;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("KW"))
                    {
                        table.Columns[colName].ColumnName = "MD kW Import(1.0.1.6.0.255;4;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("KVA"))
                    {
                        table.Columns[colName].ColumnName = "MD kVA Import(1.0.9.6.0.255;4;2)";//pks
                    }
                    //else if (colName.Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                    //{
                    //    table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm";
                    //}
                    else if (colName.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))
                    {
                        table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm";
                    }
                   //else if (colName.Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"))
                   else if (colName.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))
                    {
                       //table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm";
                       table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm";
                   }
                }
                //Apply EMF
                dataSet = ApplyMultiplyFactor(Int64.Parse(ConfigInfo.ActiveMeterDataId), dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertMidnightEnergy(DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }

        public DataSet ConvertMidnightEnergyDataWiseReport(DataSet dataSet)
        {
            long meterDataID = 0;
            try
            {
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();
                string colName = "";
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    colName = col.ColumnName;
                    if (colName == "MeterData_ID")
                    {
                        meterDataID = Convert.ToInt64(dataSet.Tables[0].Rows[0]["MeterData_ID"]);
                    }
                    table.Columns.Add(new DataColumn(colName));
                    if (colName.ToUpper().Contains("DATE"))
                    {
                        table.Columns[colName].ColumnName = "Date (0.0.1.0.0.255;8;2)";
                    }
                    else if (colName.ToUpper().Contains("WH"))
                    {
                        table.Columns[colName].ColumnName = "kWh Import(1.0.1.8.0.255;3;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("LAG"))
                    {
                        table.Columns[colName].ColumnName = "kvarh (lag) (1.0.5.8.0.255;3;2)";
                    }
                    else if (colName.ToUpper().Contains("LEAD"))
                    {
                        table.Columns[colName].ColumnName = "kvarh (lead) (1.0.8.8.0.255;3;2)";
                    }
                    else if (colName.ToUpper().Contains("VAH"))
                    {
                        table.Columns[colName].ColumnName = "kVAh Import(1.0.9.8.0.255;3;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("KW"))
                    {
                        table.Columns[colName].ColumnName = "MD kW Import(1.0.1.6.0.255;4;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("KVA"))
                    {
                        table.Columns[colName].ColumnName = "MD kVA Import(1.0.9.6.0.255;4;2)";//pks
                    }
                    // Name change for APSPDCL : Daily Survey Requirement
                    else if (colName.ToUpper() == "POWERONDURATIONTHREEPHASES")
                    {
                        //table.Columns[colName].ColumnName = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm";
                        table.Columns[colName].ColumnName = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm";
                    }
                    else if (colName.ToUpper() == "POWERONDURATION")
                    {
                        //table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm";// OBIS Code changed for APSPDCL : Daily Survey Requirement
                        table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm";// OBIS Code changed for APSPDCL : Daily Survey Requirement
                    }
                    else if (colName.ToUpper() == "POWERONDURATIONGENERIC")
                    {
                        //table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm";// OBIS Code changed for JVVNL : Daily Survey Requirement
                        table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm";// OBIS Code changed for JVVNL : Daily Survey Requirement
                    }
                    else if (colName.ToUpper() == "POWERONDURATIONGENERIC1P")///PowerOnDurationGeneric1P
                    {
                       // table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm";// OBIS Code changed for JVVNL : Daily Survey Requirement
                        table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm";// OBIS Code changed for JVVNL : Daily Survey Requirement
                    }
                    else if (colName.ToUpper().Contains("POWERFAILUREDURATION"))
                    {
                        //table.Columns[colName].ColumnName = "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm";
                        table.Columns[colName].ColumnName = "Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm";
                    }


                }

                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //if it is last row or it contains only 1 row 
                    DataRow nRow = table.NewRow();
                    if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                        nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    else
                        nRow[0] = "-------";
                    for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "MeterData_ID")
                        {
                            continue;
                        }
                        // Checking null values.
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])))
                        {
                            nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0];
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }
                    }
                    table.Rows.Add(nRow);
                }

                for (int colCount = 0; colCount < table.Rows.Count; colCount++)
                {
                    if (dataSet.Tables[0].Columns.Contains("PowerOnDuration"))
                    {
                        decimal powerOn = 0;
                       // decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"]), out powerOn);
                        //table.Rows[colCount]["Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                        decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"]), out powerOn);
                        table.Rows[colCount]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                    }
                }
                for (int colCount = 0; colCount < table.Rows.Count; colCount++)
                {
                     if (dataSet.Tables[0].Columns.Contains("POWERONDURATIONGENERIC1P"))
                    {
                        decimal powerOn = 0;
                        //decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"]), out powerOn);
                        //table.Rows[colCount]["Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                        decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"]), out powerOn);
                        table.Rows[colCount]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                    } 
                    else if (dataSet.Tables[0].Columns.Contains("POWERONDURATIONGENERIC"))
                    {
                        decimal powerOn = 0;
                        //decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"]), out powerOn);
                        //table.Rows[colCount]["Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                        decimal.TryParse(Convert.ToString(table.Rows[colCount]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"]), out powerOn);
                        table.Rows[colCount]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = ConvertTimSpanToDDHHMM(TimeSpan.FromSeconds(Convert.ToUInt64(powerOn)));
                    }
                   
                }  

                dataSet = new DataSet();
                dataSet.Tables.Add(table);
                dataSet = ApplyMultiplyFactor(Int64.Parse(ConfigInfo.ActiveMeterDataId), dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertMidnightEnergyDataWiseReport(DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;

        }
        /// <summary>
        /// convert midnight energy but not apply emf
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ConvertMidnightEnergyDataWiseReportWithoutEMF(DataSet dataSet)
        {
            long meterDataID = 0;
            try
            {
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count <= 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count <= 0)
                    return null;
                DataTable table = new DataTable();
                string colName = "";
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    colName = col.ColumnName;
                    if (colName == "MeterData_ID")
                    {
                        meterDataID = Convert.ToInt64(dataSet.Tables[0].Rows[0]["MeterData_ID"]);
                    }
                    table.Columns.Add(new DataColumn(colName));
                    //if (colName.ToUpper().Contains("DATE"))
                    if (colName.ToUpper().Contains("DATE") && !(new[] { "KVA", "KW" }.Any(c => colName.ToUpper().Contains(c)))) //SarkarA code change 20180525 //Fix Midnight Report for TangedCo MD
                    {
                        table.Columns[colName].ColumnName = "Date (0.0.1.0.0.255;8;2)";
                    }

                    else if (colName.Contains("Cumulative Energy - kWh Export (1.0.2.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)"));
                    }

                    else if (colName.Contains("Cumulative Energy - kVAh Export (1.0.10.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)"));
                    }

                    else if (colName.Contains("Cumulative Energy - kvarh (lag) Q3 (1.0.7.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lag) Q3 (1.0.7.8.0.255;3;2)"));
                    }



                    else if (colName.Contains("Cumulative Energy - kvarh (lead) Q2 (1.0.6.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lead) Q2 (1.0.6.8.0.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kWh Forward (1.0.143.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Forward (1.0.143.128.128.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kVAh Forward (1.0.144.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Forward (1.0.144.128.128.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kvarh (lag) Q1 (1.0.145.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lag) Q1 (1.0.145.128.128.255;3;2)"));
                    }



                    else if (colName.Contains("Cumulative Energy - kvarh (lead) Q4 (1.0.146.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lead) Q4 (1.0.146.128.128.255;3;2)"));
                    }



                    else if (colName.Contains("Cumulative Energy - kWh R Phase (1.0.21.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh R Phase (1.0.21.8.0.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kWh Y Phase (1.0.41.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Y Phase (1.0.41.8.0.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kWh B Phase (1.0.61.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh B Phase (1.0.61.8.0.255;3;2)"));
                    }

                    else if (colName.Contains("Cumulative Energy - kvarh (Q12) (1.0.3.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q12) (1.0.3.8.0.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kvarh (Q34) (1.0.4.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q34) (1.0.4.8.0.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kvarh (Q14) (1.0.153.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q14) (1.0.153.128.128.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kvarh (Q23) (1.0.154.128.128.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q23) (1.0.154.128.128.255;3;2)"));
                    }


                    else if (colName.Contains("Fundamental kWh Absolute (1.0.128.8.1.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Fundamental {0}Wh Absolute (1.0.128.8.1.255;3;2)"));
                    }


                    else if (colName.Contains("Cumulative Energy - kWh (1.0.1.8.0.255;3;2)"))
                    {
                      // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)"));//pks
                    }
                    else if (colName.Contains("Cumulative Energy - kvarh (lag) (1.0.5.8.0.255;3;2)"))
                    {
                       // table.Columns[colName].ColumnName = "kvarh (lag) (1.0.5.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)"));
                    }
                    else if (colName.Contains("Cumulative Energy - kvarh (lead) (1.0.8.8.0.255;3;2)"))
                    {
                       // table.Columns[colName].ColumnName = "kvarh (lead) (1.0.8.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)"));
                    }
                    else if (colName.Contains("Cumulative Energy - kVAh (1.0.9.8.0.255;3;2)"))//pks
                    {
                       // table.Columns[colName].ColumnName = "kVAh (1.0.9.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)"));//pks
                    }


                    else if (colName.Contains("kWh Import(1.0.1.8.0.255;3;2"))//pks
                    {
                        // table.Columns[colName].ColumnName = "kWh (1.0.1.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import(1.0.1.8.0.255;3;2)"));//pks
                    }
                    else if (colName.Contains("kvarh (lag) (1.0.5.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kvarh (lag) (1.0.5.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)"));
                    }
                    else if (colName.Contains("kvarh (lead) (1.0.8.8.0.255;3;2)"))
                    {
                        // table.Columns[colName].ColumnName = "kvarh (lead) (1.0.8.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)"));
                    }
                    else if (colName.Contains("kVAh Import(1.0.9.8.0.255;3;2)"))//pks
                    {
                        // table.Columns[colName].ColumnName = "kVAh (1.0.9.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Import(1.0.9.8.0.255;3;2)"));//pks
                    }



                    else if (colName.Contains("Net - kWh"))
                    {
                        // table.Columns[colName].ColumnName = "kVAh (1.0.9.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Net - {0}Wh"));
                    }

                    else if (colName.Contains("Net - kVAh"))
                    {
                        // table.Columns[colName].ColumnName = "kVAh (1.0.9.8.0.255;3;2)";
                        table.Columns[colName].ColumnName = string.Format(CommonMethods.getDisplayHeaderText("Net - {0}VAh"));
                    }


                    //Nidhi
                    //else if (colName.ToUpper().Contains("Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm"))
                    else if (colName.ToUpper().Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))
                    {
                                              
                              //table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm";
                              table.Columns[colName].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm";
                    }
                        
                        //else if (colName.ToUpper().Contains("Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm"))
                        else if (colName.ToUpper().Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))
                        {
                             //table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm";
                             table.Columns[colName].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm";
                        }
                    //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                    else if (colName.Contains("Cumulative Energy - kVArh Import (lag+lead)"))
                    {
                        table.Columns[colName].ColumnName = "Cumulative Energy - kVArh Import (lag+lead)";
                    }
                    //SarkarA code change end 20180424
                    //SarkarA code change 20180525 //Fix Midnight Report for TangedCo MD
                    else if (colName.Contains("Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)"))
                    {
                        table.Columns[colName].ColumnName = "Maximum Demand - kW Date Time (1.0.1.6.0.255;4;5)";
                    }
                    else if (colName.Contains("Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)"))
                    {
                        table.Columns[colName].ColumnName = "Maximum Demand - kVA Date Time (1.0.9.6.0.255;4;5)";
                    }
                    //SarkarA code change 20180525 end
                    else if (colName.ToUpper().Contains("KW"))
                    {
                        table.Columns[colName].ColumnName = "MD kW Import(1.0.1.6.0.255;4;2)";//pks
                    }
                    else if (colName.ToUpper().Contains("KVA"))
                    {
                        table.Columns[colName].ColumnName = "MD kVA Import(1.0.9.6.0.255;4;2)";//pks
                    }
                }

                for (int rowCount = 0; rowCount < dataSet.Tables[0].Rows.Count; rowCount++)
                {
                    //if it is last row or it contains only 1 row 
                    DataRow nRow = table.NewRow();
                    if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[rowCount][0].ToString()))
                        nRow[0] = dataSet.Tables[0].Rows[rowCount][0].ToString();
                    else
                        nRow[0] = "-------";
                    for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                    {
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "MeterData_ID")
                        {
                            continue;
                        }
                        // Checking null values.
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount])))
                        {
                            nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[rowCount][colCount]))[0];
                        }
                        else
                        {
                            nRow[colCount] = "-------";
                        }
                    }
                    table.Rows.Add(nRow);
                }
                dataSet = new DataSet();
                dataSet.Tables.Add(table);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertMidnightEnergyDataWiseReportWithoutEMF(DataSet dataSet)", ex);
                throw ex;
            }
            return dataSet;
        }

        public DataSet ConvertLoadSurvey(DataSet dataSet, string type, long meterDataId, bool isPadding)
        {

            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            string powerfactor = "powerfactor";
            string exportpowerfactor = "exportpowerfactor";//pks

            DataTable table = GetLSTableColumns(type, dataSet);

            int counter = 0;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            int div = 1;
            //following condition added to avoid exception in case of singlt L.S. record; 25th April 2012

            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                if (dataSet.Tables[0].Rows.Count > 1)
                {
                    TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[0][0].ToString()));
                    MDInterval = (int)ts.TotalMinutes;
                    //if (MDInterval < 0)
                    //    MDInterval = -MDInterval;
                }

            }
            else
            {
                MDInterval = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
                //To handle LIFO and FIFO issue for LP padding data
                MDInterval = -MDInterval;
                
            }
            //Added a new check for (-) in MDInterval for Load survey LIFO & FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 15 || MDInterval == -15)
                div = 4;
            //Added a new check for (-) in MDInterval for Load survey LIFO & FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 30 || MDInterval == -30)
                div = 2;

            dataSet.Tables[0].Columns.Remove("MDIntervalPeriod");
            dataSet.AcceptChanges();
            // Added to solve bug 72902. Checking the condition if integration period is chaging in between.
            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                if (dataSet.Tables[0].Rows.Count > 1)
                {
                    for (int row = 0; row < dataSet.Tables[0].Rows.Count - 1; row++)
                    {
                        TimeSpan timeDiff = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row + 1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row][0].ToString()));
                        int integrationPeriod = (int)timeDiff.TotalMinutes;
                        // Checking if any integration period mismatch is found.
                        if (integrationPeriod != MDInterval)
                        {
                            if (integrationPeriod == 15 || integrationPeriod == 30 || integrationPeriod == 60)
                            {
                                IntegrationPeriodStatus = true;
                                return null;
                            }

                        }
                    }
                }
            }

            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow nRow = table.NewRow();
                double energyKW = 0;
                double energyKVA = 0;
                double powerFactor = 0;
                double rPhVoltage = 0;
                double yPhVoltage = 0;
                double bPhVoltage = 0;
                double AvgVoltage = 0;
                bool rPhvoltageDouble = false;
                bool yPhvoltageDouble = false;
                bool bPhvoltageDouble = false;
                bool AveragevoltageDouble = false;
                double exportpowerFactor = 0;//pks_LS
                bool isEnergyKWDouble = false;
                bool isEnergyKVADouble = false;

                bool isExportEnergyKWDouble = false;//pks_LS
                bool isExportEnergyKVADouble = false;//pks_LS

                double ExportenergyKW = 0;//pks_LS
                double ExportenergyKVA = 0;//pks_LS

                ////SB Change Start 20170828
                ////bool isAverageVoltageDouble = false;
                //bool isAverageCurrentDouble = false;
                //bool isTemperature = false;
                ////double averageVoltage = 0;
                //double averageCurrent = 0;
                //double averageTemperature = 0;
                ////int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(meterDataId.ToString());
                ////SB Change End 20170828

                nRow[0] = dataSet.Tables[0].Rows[counter][0].ToString();
                for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Voltage"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        //isAverageVoltageDouble = double.TryParse(nRow[colCount].ToString(), out averageVoltage);
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "rPhaseVoltage")
                        {
                            rPhvoltageDouble = double.TryParse(nRow[colCount].ToString(), out rPhVoltage);
                        }
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "yPhaseVoltage")
                        {
                            yPhvoltageDouble = double.TryParse(nRow[colCount].ToString(), out yPhVoltage);
                        }
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "bPhaseVoltage")
                        {
                            bPhvoltageDouble = double.TryParse(nRow[colCount].ToString(), out bPhVoltage);
                        }

                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "averageVoltage")
                        {
                            AveragevoltageDouble = double.TryParse(nRow[colCount].ToString(), out AvgVoltage);                         
                            if (AvgVoltage !=0)
                            {
                                if (type == "Energy")
                                {
                                    if (powerFactor != 0)
                                    {
                                        nRow["Import Power Factor"] = string.Format("{0:0.0000}", powerFactor);
                                    }
                                    else
                                    {
                                        powerFactor = 1;
                                        nRow["Import Power Factor"] = string.Format("{0:0.0000}", powerFactor);
                                    }
                                   
                                }

                            }
                               
                        }
                    }
                    else if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Current"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        ////SB Change Start 20170828
                        //isAverageCurrentDouble = double.TryParse(nRow[colCount].ToString(), out averageCurrent);
                        ////SB Change End 20170828
                    }
                    else if (dataSet.Tables[0].Columns[colCount].ColumnName.ToUpper().Contains("ENERGY"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        /* VBM - Make Avg power factor configurable in LS */
                        if (type != "Demand")
                        {
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykWh")
                            {
                                isEnergyKWDouble = double.TryParse(nRow[colCount].ToString(), out energyKW);
                            }
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykVAh")
                            {
                                isEnergyKVADouble = double.TryParse(nRow[colCount].ToString(), out energyKVA);
                            }


                            if (isEnergyKVADouble && isEnergyKWDouble)
                            {
                                if (energyKVA != 0) { powerFactor = energyKW / energyKVA; }
                                else if (energyKVA==0 && (rPhVoltage != 0 || yPhVoltage != 0 || bPhVoltage != 0 || AvgVoltage != 0))
                                     powerFactor = 1;                               
                                else
                                    powerFactor = 0;
                                //nRow["Power Factor"] = string.Format("{0:0.0000}", powerFactor);
                                // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
                                if (ConfigInfo.ActiveFileType == "NONDLMS")
                                {
                                    nRow["Import Power Factor"] = string.Format("{0:0.0000}", powerFactor);//pks_LS
                                }
                                else
                                {
                                    nRow["Import Power Factor"] = string.Format("{0:0.0000}", powerFactor);//pks_LS
                                    
                                }
                            }
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykWhExport")//pks_LS
                            {
                                isExportEnergyKWDouble = double.TryParse(nRow[colCount].ToString(), out ExportenergyKW);
                            }
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykVAhExport")//pks_LS
                            {
                                isExportEnergyKVADouble = double.TryParse(nRow[colCount].ToString(), out ExportenergyKVA);
                            }

                            if (isExportEnergyKVADouble && isExportEnergyKWDouble)//For Export PF Calculation pks_LS
                            {
                                if (ExportenergyKVA != 0) { exportpowerFactor = ExportenergyKW / ExportenergyKVA; }
                                else if (energyKVA == 0 && (rPhVoltage != 0 || yPhVoltage != 0 || bPhVoltage != 0 || AvgVoltage != 0))
                                    exportpowerFactor = 1;
                                else
                                    exportpowerFactor = 0;
                                
                                if (ConfigInfo.ActiveFileType == "NONDLMS")//pks_LS
                                {
                                    nRow["Export Power Factor"] = string.Format("{0:0.00}", exportpowerFactor);
                                }
                                else
                                {
                                    nRow["Export Power Factor"] = string.Format("{0:0.0000}", exportpowerFactor);
                                }

                            }

                        }
                    }
                    
                    else
                    {
                        if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("tamperStatus"))
                        {
                            nRow[colCount] = ConvertDecimalToHexString((CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                        }
                        else
                        {
                            // Checking null values
                            if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[counter][colCount].ToString()))
                            {
                                nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();

                                if (nRow[colCount].Equals("-25.8"))
                                {
                                    nRow[colCount] = "-------";
                                }
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }

                    }
                }
                table.Rows.Add(nRow);
            }

            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            //table = GetLSTable(type);
            table = dataSet.Tables[0].Clone();//GetLSTableColumns(type, dataSet);
            DataRow secondRow = null;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow[0]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow[0]));

                DataRow row = table.NewRow();
                string val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    string colNames = dataSet.Tables[0].Columns[i].ColumnName;
                    string demandVal = CheckUnit(Convert.ToString(firstRow[i]))[0];
                    if (colNames.Contains("0.0.94.91.128.255"))
                    {
                        row[i] = CheckPowerOffValue(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Equals("ENERGY") && ConfigInfo.ActiveFileType == "DLMS")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.2.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.147.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.10.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.148.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.3.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.4.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.155.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.156.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.128.29.1.255;3;2)") ||
                        colNames.Contains("(1.0.21.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.41.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.61.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.7.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.6.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.149.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.91.7.0.255;3;2)") ||//add pradipta_load_neu
                        colNames.Contains("(1.0.150.128.128.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 1)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) * div;
                                /* VBM - Apply fix 3 digit resolution to demand */
                                //row[i] = CheckDecimal(num.ToString());
                                row[i] = num.TruncateToPrecision(3);
                                /* VBM - Apply fix 3 digit resolution to demand */
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Equals("ENERGY") && ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 1)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) * div;
                                /* VBM - Apply fix 3 digit resolution to demand */
                                //row[i] = CheckDecimal(num.ToString());
                                row[i] = num.TruncateToPrecision(3);
                                /* VBM - Apply fix 3 digit resolution to demand */
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Equals("DEMAND") && ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType != "1P-2W")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) / div;
                                row[i] = num.TruncateToPrecision(3);

                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }                   
                    else
                    {
                        row[i] = CheckDecimal(demandVal);
                    }
                }
                if (!type.Trim().ToUpper().Equals("ENERGY") && dataSet.Tables[0].Rows.Count == 1 && ConfigInfo.ActiveFileType == "DLMS")
                {
                    MessageBox.Show("Demand values cannot be calculated with a single record", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                table.Rows.Add(row);
                bool flag = true;
                #region Data for PUMA
                if (isPUMA)
                {
                    DateTime tempDate = firstRowDate.AddMinutes(MDInterval);
                    TimeSpan ts = secondRowDate.Subtract(tempDate);
                    // negative time stamp is change to Positive AGAINST BUG ID 446292 Padding is not correct in load survey.
                    //if (ts != null && ts.ToString().Contains('-'))
                    //    ts = -(ts);

                    int min = 0;
                    if (ts.Hours == 0 & ts.Minutes == 0)
                    {
                        if (ts.Days != 0)
                        {
                            min = ts.Days * 24 * 60;
                        }
                    }
                    else if (ts.Days == 0 && ts.Hours != 0 && ts.Minutes != 0)
                    {
                        min = ts.Hours * 60 + ts.Minutes;
                    }
                    else if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes != 0)
                    {
                        min = ts.Minutes;
                    }
                    // ND is not coming for current date time missing
                    else if (ts.Days == 0 && ts.Hours != 0 && ts.Minutes == 0)
                    {
                        min = ts.Hours * 60;
                    }
                    else
                    {
                        if (ts.Days != 0)
                        {
                            min = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
                        }
                    }
                    // Gap Reconciliation
                    if (isPadding)
                    {
                        if (MDInterval != 0)
                        {
                            int noOfInterval = min / mdInterval;

                            if (min != 0)
                            {

                                for (int i = 0; i < noOfInterval; i++)
                                {
                                    firstRowDate = firstRowDate.AddMinutes(MDInterval);
                                    {
                                        row = table.NewRow();
                                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                                        row[0] = val;
                                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                                        {
                                            if (dataSet.Tables[0].Columns[j].ColumnName == "PowerFactor")
                                                row[j] = "NPF";
                                            else
                                                row[j] = "ND";
                                        }
                                        table.Rows.Add(row);
                                    }

                                }
                            }
                        }
                    }
                }
                #endregion
                else
                {
                    do
                    {
                        if (firstRowDate == secondRowDate)
                            break;
                        firstRowDate = firstRowDate.AddMinutes(MDInterval);
                        if (!firstRowDate.Equals(secondRowDate))
                        {
                            row = table.NewRow();
                            val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                            row[0] = val;
                            for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            {
                                if (dataSet.Tables[0].Columns[j].ColumnName == "PowerFactor")
                                    row[j] = "NPF";
                                else
                                    row[j] = "ND";
                            }
                            table.Rows.Add(row);
                        }
                        else
                            flag = false;
                    } while (flag);
                }
            }
            #region "Net Calculation"
            foreach (DataRow itemRow in table.Rows)
            {
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}WH")) || table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}W")))
                {
                    DataColumn clmNETKWH = new DataColumn();
                    try
                    {
                        if (type.Equals("Demand"))
                        {
                            clmNETKWH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}W"))];
                        }
                        else
                        {
                            clmNETKWH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}WH"))];
                        }
                        
                        decimal NetKWHValue = 0;
                        decimal CumKWHValue = 0;
                        decimal CumKWHExportValue = 0;
                        string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);

                        if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                        {
                            if (type.Equals("Demand"))
                            {
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)].ToString().Equals("ND"))       //SarkarA code change 20180424 //fix exception reliance 1PH Net
                                    CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)]);
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)].ToString().Equals("ND")) 
                                    CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)]);
                            }
                            else
                            {
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)].ToString().Equals("ND"))        //SarkarA code change 20180424 //fix exception reliance 1PH Net
                                    CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)]);
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)].ToString().Equals("ND"))  
                                    CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)]);
                            }

                            NetKWHValue = CumKWHValue - CumKWHExportValue;
                            itemRow[clmNETKWH.ColumnName] = NetKWHValue;
                        }
                        //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                        //{
                        //    if (type.Equals("Demand"))
                        //    {
                        //        CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)]);
                        //        CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWImport)]);
                        //    }
                        //    else
                        //    {
                        //        CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)]);
                        //        CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHImport)]);
                        //    }

                        //    NetKWHValue = CumKWHValue - CumKWHExportValue;
                        //    itemRow[clmNETKWH.ColumnName] = NetKWHValue;
                        //}
                        else
                        {
                            itemRow[clmNETKWH.ColumnName] = "-------";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ConvertLoadSurvey(DataSet dataSet, string type, long meterDataId, bool isPadding)", ex);
                        itemRow[clmNETKWH.ColumnName] = "-------";
                    }
                    if (!(table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)) || table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport))))
                    {
                        table.Columns.Remove(CommonMethods.getDisplayHeaderText("NET - {0}WH"));        //SarkarA code change 20180424 //fix exception reliance 1PH Net
                    }
                }
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}VAH")) || table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}VA")))
                {
                    DataColumn clmNETKVAH = new DataColumn();
                    try
                    {
                        if (type.Equals("Demand"))
                        {
                            clmNETKVAH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}VA"))];
                        }
                        else
                        {
                            clmNETKVAH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}VAH"))];
                        }
                        
                        string meterVariant = GetMeterVariantByMeterDataID((int)meterDataId);
                        decimal NetKVAHValue = 0;
                        decimal CumKVAHValue = 0;
                        decimal CumKVAHExportValue = 0;
                        if (meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR)
                        {
                            if (type.Equals("Demand"))
                            {
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)].ToString().Equals("ND"))      //SarkarA code change 20180424 //fix exception reliance 1PH Net
                                    CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)]);
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)].ToString().Equals("ND"))  
                                    CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)]);
                            }
                            else
                            {
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)].ToString().Equals("ND"))          //SarkarA code change 20180424 //fix exception reliance 1PH Net
                                    CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)]);
                                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)) && !itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)].ToString().Equals("ND"))
                                    CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)]);
                            }
                            NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                            itemRow[clmNETKVAH.ColumnName] = NetKVAHValue;
                        }
                        //else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                        //{
                        //    if (type.Equals("Demand"))
                        //    {
                        //        CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)]);
                        //        CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAImport)]);
                        //    }
                        //    else
                        //    {
                        //        CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)]);
                        //        CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHImport)]);
                        //    }
                        //    NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                        //    itemRow[clmNETKVAH.ColumnName] = NetKVAHValue;
                        //}
                        else
                        {
                            itemRow[clmNETKVAH.ColumnName] = "-------";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ConvertLoadSurvey(DataSet dataSet, string type, long meterDataId, bool isPadding)", ex);
                        itemRow[clmNETKVAH.ColumnName] = "-------";
                    }
                    if (!(table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)) || table.Columns.Contains(CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport))))
                    {
                        table.Columns.Remove(CommonMethods.getDisplayHeaderText("NET - {0}VAH"));       //SarkarA code change 20180424 //fix exception reliance 1PH Net
                    }
                }
            }
            
            #endregion

            // To remove Column having value "-------" in the DataTable
            int flagLS = 0;
            List<int> colIndex = new List<int>();
            for (int counterCol = 0; counterCol < table.Columns.Count; counterCol++)
            {
                for (int counterRow = 0; counterRow < table.Rows.Count; counterRow++)
                {
                    if (!table.Rows[counterRow][counterCol].Equals("-------"))
                    {
                        flagLS++;
                        break;
                    }
                    else
                        flagLS = 0;
                }
                if (flagLS == 0)
                    colIndex.Add(counterCol);
            }
            for (int counterLS = colIndex.Count - 1; counterLS >= 0; counterLS--)
            {
                table.Columns.RemoveAt(colIndex[counterLS]);
            }

            table.AcceptChanges();
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, ds);
        }
        
        //// SB Change Start 20170828
        ///// <summary>
        ///// This methods calculates the Max Current for all 3 Phases and decides the temperature.
        ///// </summary>
        ///// <param name="dataRow">Row containg 3 current values.</param>
        ///// <returns>Returns the temperature.</returns>
        //private double CalculateTemperature(DataRow dataRow)//, out double dVoltage)
        //{
        //    double dCurrentR = 0.0;
        //    double dCurrentY = 0.0;
        //    double dCurrentB = 0.0;
        //    double dVoltageR = 0.0;
        //    double dVoltageY = 0.0;
        //    double dVoltageB = 0.0;

        //    // Read current for all three phases
        //    bool bCurrentR = double.TryParse(dataRow[1].ToString(), out dCurrentR);
        //    bool bCurrentY = double.TryParse(dataRow[2].ToString(), out dCurrentY);
        //    bool bCurrentB = double.TryParse(dataRow[3].ToString(), out dCurrentB);
        //    // Read voltage for all three phases
        //    bool bVoltageR = double.TryParse(dataRow[4].ToString(), out dVoltageR);
        //    bool bVoltageY = double.TryParse(dataRow[5].ToString(), out dVoltageY);
        //    bool bVoltageB = double.TryParse(dataRow[6].ToString(), out dVoltageB);

        //    // If Voltage is zero across all three phases then display "--------"
        //    if ((dVoltageR + dVoltageY + dVoltageB) <= 0)
        //    {
        //        return 0.0;
        //    }

        //    // Considering the Max Current within 3 Phases.
        //    double dCurrentMax = Math.Max(Math.Max(dCurrentR, dCurrentY), dCurrentB);
            
        //    double dTemperature = 0.0;

        //    dTemperature = random.NextDouble();

        //    // Chnaging the temperature based on Max current.
        //    if (dCurrentMax <= 2)
        //        return 24 + dTemperature;
        //    else if (dCurrentMax > 2 && dCurrentMax <= 4)
        //        return 25 + dTemperature;
        //    else if (dCurrentMax > 4 && dCurrentMax <= 6)
        //        return 26 + dTemperature;
        //    else if (dCurrentMax > 6 && dCurrentMax <= 8)
        //        return 27 + dTemperature;
        //    else if (dCurrentMax > 8 && dCurrentMax <= 10)
        //        return 28 + dTemperature;

        //    else if (dCurrentMax > 10 && dCurrentMax <= 13)
        //        return 29;
        //    else if (dCurrentMax > 13 && dCurrentMax <= 15)
        //        return 30;
        //    else if (dCurrentMax > 15 && dCurrentMax <= 18)
        //        return 31;
        //    else if (dCurrentMax > 18 && dCurrentMax <= 20)
        //        return 32;

        //    else if (dCurrentMax > 20 && dCurrentMax <= 23)
        //        return 33;
        //    else if (dCurrentMax > 23 && dCurrentMax <= 27)
        //        return 34;
        //    else if (dCurrentMax > 27 && dCurrentMax <= 30)
        //        return 35;

        //    else if (dCurrentMax > 30 && dCurrentMax <= 33)
        //        return 36;
        //    else if (dCurrentMax > 33 && dCurrentMax <= 37)
        //        return 37;
        //    else if (dCurrentMax > 37 && dCurrentMax <= 40)
        //        return 38;

        //    else if (dCurrentMax > 40 && dCurrentMax <= 45)
        //        return 39;
        //    else if (dCurrentMax > 45 && dCurrentMax <= 50)
        //        return 40;

        //    else if (dCurrentMax > 50 && dCurrentMax <= 53)
        //        return 45;
        //    else if (dCurrentMax > 53)
        //        return 48;

        //    else
        //        return 0.0;
        //}
        //// SB Change End 20170828

        //// SB Change Start 20170905
        ///// <summary>
        ///// This methods calculates the Max Current for all 3 Phases and decides the temperature.
        ///// </summary>
        ///// <param name="dataRow">Row containg 3 current values.</param>
        ///// <returns>Returns the temperature.</returns>
        //private double CalculateTemperatureForSinglePhase(DataRow dataRow)//, out double dVoltage)
        //{
        //    double dCurrent = 0.0;
        //    double dVoltage = 0.0;
            
        //    // Read current
        //    bool bCurrent = double.TryParse(dataRow[2].ToString(), out dCurrent);
        //    // Read voltage
        //    bool bVoltage = double.TryParse(dataRow[1].ToString(), out dVoltage);


        //    // If Voltage is zero then display "--------"
        //    if (dVoltage <= 0)
        //    {
        //        return 0.0;
        //    }

        //    double dTemperature = 0.0;

        //    dTemperature = random.NextDouble();

        //    // Chnaging the temperature based on Max current.
        //    if (dCurrent <= 2)
        //        return 24 + dTemperature;
        //    else if (dCurrent > 2 && dCurrent <= 4)
        //        return 25 + dTemperature;
        //    else if (dCurrent > 4 && dCurrent <= 6)
        //        return 26 + dTemperature;
        //    else if (dCurrent > 6 && dCurrent <= 8)
        //        return 27 + dTemperature;
        //    else if (dCurrent > 8 && dCurrent <= 10)
        //        return 28 + dTemperature;

        //    else if (dCurrent > 10 && dCurrent <= 13)
        //        return 29;
        //    else if (dCurrent > 13 && dCurrent <= 15)
        //        return 30;
        //    else if (dCurrent > 15 && dCurrent <= 18)
        //        return 31;
        //    else if (dCurrent > 18 && dCurrent <= 20)
        //        return 32;

        //    else if (dCurrent > 20 && dCurrent <= 23)
        //        return 33;
        //    else if (dCurrent > 23 && dCurrent <= 27)
        //        return 34;
        //    else if (dCurrent > 27 && dCurrent <= 30)
        //        return 35;

        //    else if (dCurrent > 30 && dCurrent <= 33)
        //        return 36;
        //    else if (dCurrent > 33 && dCurrent <= 37)
        //        return 37;
        //    else if (dCurrent > 37 && dCurrent <= 40)
        //        return 38;

        //    else if (dCurrent > 40 && dCurrent <= 45)
        //        return 39;
        //    else if (dCurrent > 45 && dCurrent <= 50)
        //        return 40;

        //    else if (dCurrent > 50 && dCurrent <= 53)
        //        return 45;
        //    else if (dCurrent > 53)
        //        return 48;

        //    else
        //        return 0.0;
        //}
        //// SB Change End 20170905

        public DataSet ConvertLoadSurveyForGraph(DataSet dataSet, string type, long meterDataId)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            //DataTable table = GetLSTable(type);
            DataTable table = GetLSTableColumns(type, dataSet);
            int counter = 0;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                if (dataSet.Tables[0].Rows.Count > 1)
                {
                    TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[0][0].ToString()));
                    MDInterval = (int)ts.TotalMinutes;
                }
            }
            else
            {
                MDInterval = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
                
            }
            if(dataSet.Tables[0].Columns.Contains("MDIntervalPeriod"))
                dataSet.Tables[0].Columns.Remove("MDIntervalPeriod");
            dataSet.AcceptChanges();
            int div = 1;
            //Added a new check for (-) in MDInterval for Load survey FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 15 || MDInterval == -15)
                div = 4;
            //Added a new check for (-) in MDInterval for Load survey FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 30 || MDInterval == -30)
                div = 2;

            // Added to solve bug 72902 for Graphical view. Checking the condition if integration period is chaging in between. 16th April 2012
            //Added to exclude it for IEC files as they may have no record for power off withing same day.
            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                for (int row = 0; row < dataSet.Tables[0].Rows.Count - 1; row++)
                {
                    TimeSpan timeDiff = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row + 1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row][0].ToString()));
                    int integrationPeriod = (int)timeDiff.TotalMinutes;
                    // Checking if any integration period mismatch is found.
                    if (integrationPeriod != MDInterval)
                    {
                        if (integrationPeriod == 15 || integrationPeriod == 30 || integrationPeriod == 60)
                        {
                            IntegrationPeriodStatus = true;
                            return null;
                        }
                    }
                }
            }
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow nRow = table.NewRow();
                double energyKW = 0;
                double energyKVA = 0;

                double energyKWExport = 0;
                double energyKVAExport = 0;

                double powerFactor = 0;

                double ExportpowerFactor = 0;//pks
                bool isEnergyKWDouble = false;//pks
                bool isEnergyKVADouble = false;//pks

                bool isExportEnergyKWDouble = false;//pks
                bool isExportEnergyKVADouble = false;//pks


                nRow[0] = dataSet.Tables[0].Rows[counter][0].ToString();
                for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {

                    if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Voltage"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                    }

                    if (type == "PowerFactor")//
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykWh")
                        {
                            isEnergyKWDouble = double.TryParse(nRow[colCount].ToString(), out energyKW);
                        }
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykVAh")
                        {
                            isEnergyKVADouble = double.TryParse(nRow[colCount].ToString(), out energyKVA);
                        }
                        if (isEnergyKVADouble && isEnergyKWDouble)
                        {
                            if (energyKVA != 0)
                            {
                                powerFactor = energyKW / energyKVA;
                            }
                            else
                            {
                                powerFactor = 0;
                            }
                            //****** Import has been removed done by AET wrongly *********
                            nRow["Power Factor"] = string.Format("{0:0.0000}", powerFactor);
                        }
                        else
                        {
                            nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                        }
                    }
                    else
                    {
                        nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                    }
                    //Export PowerFactor

                    if (type == "ExportPowerFactor")//pks
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykWhExport")
                        {
                            isExportEnergyKWDouble = double.TryParse(nRow[colCount].ToString(), out energyKWExport);
                        }
                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykVAhExport")
                        {
                            isExportEnergyKVADouble = double.TryParse(nRow[colCount].ToString(), out energyKVAExport);
                        }
                        if (isExportEnergyKVADouble && isExportEnergyKWDouble)
                        {
                            if (energyKVAExport != 0)
                            {
                                ExportpowerFactor = energyKWExport / energyKVAExport;
                            }
                            else
                            {
                                ExportpowerFactor = 0;
                            }

                            nRow["Export Power Factor"] = string.Format("{0:0.0000}", ExportpowerFactor);
                        }
                        else
                        {
                            nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                        }
                    }
                    else
                    {
                        nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                    }
                }
                table.Rows.Add(nRow);
            }

            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            //table = GetLSTable(type);
            table = dataSet.Tables[0].Clone();//GetLSTableColumns(type, dataSet);
            DataRow secondRow = null;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {

                DataRow firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow[0]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow[0]));

                DataRow row = table.NewRow();
                string val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    string colNames = dataSet.Tables[0].Columns[i].ColumnName;
                    string demandVal = CheckUnit(Convert.ToString(firstRow[i]))[0];
                    if (!type.Trim().ToUpper().Contains("ENERGY") && ConfigInfo.ActiveFileType == "DLMS")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)") ||
                            // SB Code Change Start - 20180129 - Export Demand Display in Load Survey Graph
                        colNames.Contains("(1.0.2.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.6.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.7.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.10.29.0.255;3;2)"))
                        // SB Code Change End - 20180129 - Export Demand Display in Load Survey Graph)
                        {
                            decimal num = 0;
                            if (!string.IsNullOrEmpty(demandVal))
                                num = decimal.Parse(demandVal) * div;
                            if (colNames == "Block Demand - kW Export (1.0.2.29.0.255;3;2)" || colNames == "Block Demand - kVA Export (1.0.10.29.0.255;3;2)")
                            {
                                num = decimal.Parse(demandVal) * div;
                            }
                            if (colNames == "Block Energy - kWh Export (1.0.2.29.0.255;3;2)" || colNames == "Block Energy - kVAh Export (1.0.10.29.0.255;3;2)")
                            {
                                num = decimal.Parse(demandVal);
                            }
                            /* VBM - Apply fix 3 digit resolution to demand */
                            //row[i] = CheckDecimal(num.ToString());
                            row[i] = num.TruncateToPrecision(3);
                            /* VBM - Apply fix 3 digit resolution to demand */
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Contains("ENERGY") && ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 1)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) * div;

                                /* VBM - Apply fix 3 digit resolution to demand */
                                //row[i] = CheckDecimal(num.ToString());
                                row[i] = num.TruncateToPrecision(3);
                                /* VBM - Apply fix 3 digit resolution to demand */
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Contains("DEMAND") && ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType != "1P-2W")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) / div;
                                row[i] = num.TruncateToPrecision(3);
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                        {
                            row[i] = CheckDecimal(demandVal);
                        }
                    }

                    else
                    {
                        row[i] = CheckDecimal(demandVal);
                    }
                }
                table.Rows.Add(row);
                bool flag = true;
                do
                {
                    if (firstRowDate == secondRowDate)
                        break;

                    if (firstRowDate < secondRowDate)
                    {
                        firstRowDate = firstRowDate.AddMinutes(MDInterval);
                    }
                    else
                    {
                        secondRowDate = firstRowDate.AddMinutes(MDInterval);
                    }

                    if (!firstRowDate.Equals(secondRowDate))
                    {
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                        {
                            row[j] = "-1";
                            //if (dataSet.Tables[0].Columns[j].ColumnName == "PowerFactor")
                            //    row[j] = "-1";
                            //else
                            //    row[j] = "0";
                        }
                        table.Rows.Add(row);
                    }
                    else
                        flag = false;
                } while (flag);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, ds);

        }
        private string CheckDecimal(string val)
        {
            val = val.Trim();
            if (val == "0")
                return "0.00";
            else
                return val;
        }

        private string CheckPowerOffValue(string val)
        {
            val = val.Trim();
            if (val == "0")
                return "0";
            else
                return val;
        }
        //private string ConvertDecimaltoHex(decimal value)
        //{ }
        public DataSet ConvertInstantRowToColumn(DataSet ds, long meterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Descriptions", typeof(System.String)));
            table.Columns.Add(new DataColumn("OBIS Code", typeof(System.String)));
            table.Columns.Add(new DataColumn("Class ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int counter = 0;
            foreach (DataRow rows in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                row[0] = rows[0];
                row[1] = rows[2];
                row[2] = rows[3];
                row[3] = rows[4];
                counter = Int32.Parse(rows[5].ToString());
                string val = Convert.ToString(rows[1]);
                //added PUMA
                //if (counter == 1 || counter == 25 || counter == 27 || counter == 29)
                if (ConfigInfo.ActiveFileType == "DLMS")
                {
                    if (row[0].ToString().ToUpper().Contains("ELAPSED"))
                    {
                        string[] data = val.Split('*');
                        DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0);
                        TimeSpan timeSpan = DateUtility.LongToDateTime(Int64.Parse(data[0])) - epochTime;
                        row[4] = ConvertTimSpanToMMSS(timeSpan);
                        row[5] = DATEFORMATFORELAPSEDTIME;

                    }
                    else if (rows[0].ToString().ToUpper().Contains("DATE") || (rows[0].ToString().ToUpper().Contains("TIME")))
                    {

                        string dVal = "";
                        if (counter == 1)
                        {
                            dVal = DateUtility.LongToStringDateTimeWithSecFormat(Int64.Parse(val));
                            row[5] = string.Concat(ConfigInfo.DateFormat(), " HH:MM:SS");

                        }
                        else
                        {
                            dVal = DateUtility.LongToStringDateTimeFormat(Int64.Parse(val));                        
                            row[5] = string.Concat(ConfigInfo.DateFormat(), " HH:MM");
                        }
                        dVal = dVal.Replace("99", "00");
                        row[4] = dVal;

                    }
                    else
                    {
                        string[] data = val.Split('*');
                        string chkPowerOnOffDurationFormat = ConfigSettings.GetValue("ChkPowerOnOffDurationFormat");

                        if (row[0].ToString().Contains(CUMPOWERFAILUREFORINSTANT) || row[0].ToString().Contains(CUMPOWERONFORINSTANT) ||
                             row[0].ToString().Contains(CUMPOTENTIALFAILRPHASE) || row[0].ToString().Contains(CUMPOTENTIALFAILYPHASE)
                            || row[0].ToString().Contains(CUMPOTENTIALFAILBPHASE) || row[0].ToString().Contains(CUMCURRENTFAILRPHASE) || row[0].ToString().Contains(CUMCURRENTFAILYPHASE) || row[0].ToString().Contains(CUMCURRENTFAILBPHASE))
                        {
                            if (chkPowerOnOffDurationFormat == "1")
                            {
                                TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(data[0]));                                                             

                                row[4] = ConvertTimSpanToDDDDHH(timeSpan);
                                row[5] = DATEFORMATFORINSCUMPOWFAIL_DDDDHH;
                                table.Rows.Add(row);
                                continue;
                            }
                            else
                            {
                                TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(data[0]));
                                row[4] = ConvertTimSpanToDDHHMM(timeSpan);
                                row[5] = DATEFORMATFORINSCUMPOWFAIL;
                                table.Rows.Add(row);
                                continue;

                            }
                        }
                        //if (
                        //    row[0].ToString().Contains(CUMPOWERFAILUREFORINSTANT) || row[0].ToString().Contains(CUMPOWERONFORINSTANT)||
                        //     row[0].ToString().Contains(CUMPOTENTIALFAILRPHASE) || row[0].ToString().Contains(CUMPOTENTIALFAILYPHASE)
                        //    || row[0].ToString().Contains(CUMPOTENTIALFAILBPHASE) || row[0].ToString().Contains(CUMCURRENTFAILRPHASE) || row[0].ToString().Contains(CUMCURRENTFAILYPHASE) || row[0].ToString().Contains(CUMCURRENTFAILBPHASE))
                        //{
                        //    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(data[0]));
                        //    row[4] = ConvertTimSpanToDDHHMM(timeSpan);
                        //    row[5] = DATEFORMATFORINSCUMPOWFAIL;
                        //    table.Rows.Add(row);
                        //    continue;
                        //}
                        /* VBM - Convert Minute value into dd:hh:mm */
                        else if (counter == 2 || counter == 3 || counter == 4 || counter == 13 || counter == 14 || counter == 15 || counter == 16 || counter == 17 || counter == 18 || counter == 19 || counter == 26 || counter == 28)
                        {
                            row[4] = (Convert.ToDecimal(data[0])).ToString();
                        }
                        //else if (row[0].ToString().ToUpper().Contains("REVERSE"))
                        //{
                        //    row[5] = "kWh";
                        //    row[4] = TruncateToPrecision(data[0], -2, 2);
                        // }
                        else
                        {
                            row[4] = data[0];
                        }

                        if (data.Length > 1)
                            row[5] = data[1];
                    }
                }
                else
                {
                    //For NON DLMS files no need to check  counters
                    //Mapper has already taken care of this , so go ahead split data and fill in data and unit columns.
                    string[] data = val.Split('*');
                    row[4] = data[0];
                    if (data.Length > 1)
                    {
                        row[5] = data[1];
                    }
                }
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataId, dataSet, "Descriptions", "Value");
        }
        public DataSet ConvertLoadSwitchRowToColumn(DataSet ds, long meterDataId)//For smart meter
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Control Event Connect Disconnect (0.0.96.11.6.255;1;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("RTC (0.0.1.0.0.255;8;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Switch Operation Reason (0.0.96.50.4.255;1;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh (1.0.1.8.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ1(1.0.1.8.1.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ2(1.0.1.8.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ3 (1.0.1.8.3.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ4 (1.0.1.8.4.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ5 (1.0.1.8.5.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ6 (1.0.1.8.6.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ7 (1.0.1.8.7.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kwh TZ8 (1.0.1.8.8.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah (1.0.9.8.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ1 (1.0.9.8.1.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ2 (1.0.9.8.2.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ3 (1.0.9.8.3.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ4 (1.0.9.8.4.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ5 (1.0.9.8.5.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ6 (1.0.9.8.6.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ7 (1.0.9.8.7.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvah TZ8 (1.0.9.8.8.255;3;2)", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];

            foreach (DataRow rows in ds.Tables[0].Rows)
            {
                string val = string.Empty;
                row = table.NewRow();
                for (int counter = 0; counter < ds.Tables[0].Columns.Count; counter++)
                {
                    val = Convert.ToString(rows[counter]);
                    string[] data = val.Split('*');

                    if (counter == 2)//for switch operation
                        if (data[0] == "1") data[0] = "Remote Connect";
                        else if (data[0] == "2") data[0] = "Remote disconnect";
                        else if (data[0] == "3") data[0] = "Local connect";
                        else if (data[0] == "4") data[0] = "Local disconnect";
                        else if (data[0] == "5") data[0] = "Manual connect";

                    row[counter] = data[0];
                    if (counter == 1)//for date time
                        row[counter] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(rows[counter]));//for date time
                }


                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        /// Truncates the decimals after the precision from a decimal type
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public string TruncateToPrecision(string parsedValue, int scalar, uint precision)
        {
            decimal targetValue = Convert.ToDecimal(parsedValue) * Convert.ToDecimal(Math.Pow(10, scalar));
            string value = targetValue.ToString();
            try
            {
                if (scalar < 0)
                {
                    decimal step = (decimal)Math.Pow(10, precision);
                    targetValue = (Int64)Math.Truncate(step * targetValue) / step;
                    value = string.Format("{0:F" + precision.ToString() + "}", targetValue);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "TruncateToPrecision(string parsedValue, int scalar, uint precision)", ex);
            }
            return value;
        }

        /// <summary>
        /// VBM - 22/03/2013
        /// Convert Minute value into dd:hh:mm
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public string ConvertTimSpanToDDHHMM(TimeSpan timeSpan)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (timeSpan.Days.ToString().Contains('-'))
                strBuilder.Append("0");
            else
                strBuilder.Append(timeSpan.Days);
            strBuilder.Append(" : ");
            if (timeSpan.Hours.ToString().Contains('-'))
                strBuilder.Append("00");
            else
                strBuilder.Append(timeSpan.Hours.ToString("00"));
            strBuilder.Append(" : ");
            if (timeSpan.Minutes.ToString().Contains('-'))
                strBuilder.Append("00");
            else
                strBuilder.Append(timeSpan.Minutes.ToString("00"));
            return strBuilder.ToString();

        }

        /// <summary>
        /// Convert Minute value into MM:SS
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public string ConvertTimSpanToMMSS(TimeSpan timeSpan)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(timeSpan.Minutes.ToString("00"));
            strBuilder.Append(" : ");
            strBuilder.Append(timeSpan.Seconds.ToString("00"));
            return strBuilder.ToString();

        }

        public DataSet ConvertMidNightData(DataSet ds)
        {
            try
            {
                if (ds == null)
                    return null;
                if (ds.Tables.Count == 0)
                    return null;
                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row[MIDNIGHTDATE] = row[MIDNIGHTDATE].ToString();
                    row[DAILYKWH] = row[DAILYKWH].ToString().Split('*')[0].ToString();
                    row[DAILYKVAH] = row[DAILYKVAH].ToString().Split('*')[0].ToString();
                    row[DAILYKVARHLAG] = row[DAILYKVARHLAG].ToString().Split('*')[0].ToString();
                    row[DAILYKVARHLEAD] = row[DAILYKVARHLEAD].ToString().Split('*')[0].ToString();
                }
                ds.AcceptChanges();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ConvertMidNightData(DataSet ds)", ex);
                return null;
            }
            return ds;

        }
        public DataSet ConvertGeneralRowToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            //if (!UtilityDetails.ShowMeterModelNo)
            //{
            //    ds.Tables[0].Columns.Remove("MeterModelNo");
            //}

            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Descriptions", typeof(System.String)));
            table.Columns.Add(new DataColumn("OBIS Code", typeof(System.String)));
            table.Columns.Add(new DataColumn("Class ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];

            foreach (DataRow row1 in ds.Tables[0].Rows)//PGVCL
            {

                //string val1 = row1[metertype].ToString();
                meterType = row1.ItemArray[3].ToString();
                meterModelNo= row1.ItemArray[8].ToString();
            }
            

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string val = Convert.ToString(col.ColumnName);
                row = table.NewRow();
                row[0] = GetColumnName(val,meterType, meterModelNo);
                row[1] = GetOBISCode(val);
                row[2] = "1";
                row[3] = "2";
                val = Convert.ToString(dataRow[val]);
                if (val.Trim().ToUpper().Equals("LGZ"))
                    val = "Cabcon Edge Solution";
                row[4] = val;
                if (meterType == "3P-4W WCM" || meterType == "1P-2W" || meterModelNo == "35") //PGVC
                {
                    if (row.ItemArray[0].ToString() == "Primary Meter Constant")
                    { }
                    else
                    {
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    table.Rows.Add(row);
                }
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        private string GetOBISCode(string columnName)
        {
            if (columnName.ToLower().Equals("meterserialnumber")) return "0.0.96.1.0.255";
            else if (columnName.ToLower().Equals("manufacturername")) return "0.0.96.1.1.255";
            else if (columnName.ToLower().Equals("firmwareversionformeter")) return "1.0.0.2.0.255";
            else if (columnName.ToLower().Equals("metertype")) return "0.0.94.91.9.255";
            else if (columnName.ToLower().Equals("internalctratio")) return "1.0.0.4.2.255";
            else if (columnName.ToLower().Equals("internalptratio")) return "1.0.0.4.3.255";
            else if (columnName.ToLower().Equals("internalvtratio")) return "1.0.0.4.3.255";
            else if (columnName.ToLower().Equals("category")) return "0.0.94.91.11.255";
            else if (columnName.ToLower().Equals("meteryearofmanufacture")) return "0.0.96.1.4.255";
            else if (columnName.ToLower().Equals("metermodelno")) return "0.0.96.0.166.255";

            else if (columnName.ToLower().Equals("internalfirmwareversion")) return "1.0.0.2.1.255";
            //else if (columnName.ToLower().Equals("voltagerating")) return "0.0.96.1.217.255";// 
            else if (columnName.ToLower().Equals("voltagerating")) return "0.0.96.1.158.255"; //Sapphire HTCT
            else if (columnName.ToLower().Equals("basiccurrentrating")) return "0.0.94.91.12.255";
            else if (columnName.ToLower().Equals("currentrating")) return "0.0.94.91.12.255";
            
            else if (columnName.ToLower().Equals("primarymeterconstantinfo")) return "0.0.96.50.14.255";//PGVCL
            else if (columnName.ToLower().Equals("meterconstantinfo")) return "0.0.96.1.220.255";

            else if (columnName.ToLower().Equals("metermonthofmanufacture")) return "0.0.96.128.17.255";
            else if (columnName.ToLower().Equals("accuracyclass")) return "1.0.96.128.15.255";
            else return "";
        }

        private string GetColumnName(string columnName,string metertype,string meterModelNo)
        {
            
            if( (metertype == "3P-4W WCM" ) || (metertype == "1P-2W")|| meterModelNo == "35")
            {
                if (columnName.ToLower().Equals("meterserialnumber")) return "Meter Serial Number";
                else if (columnName.ToLower().Equals("manufacturername")) return "Manufacturer Name";
                else if (columnName.ToLower().Equals("firmwareversionformeter")) return "Firmware Version";
                else if (columnName.ToLower().Equals("metertype")) return "Meter Type";
                else if (columnName.ToLower().Equals("internalctratio")) return "Internal CT Ratio";
                else if (columnName.ToLower().Equals("internalptratio")) return "Internal PT Ratio";
                else if (columnName.ToLower().Equals("internalvtratio")) return "Internal VT Ratio";
                else if (columnName.ToLower().Equals("category")) return "Category";
                else if (columnName.ToLower().Equals("meteryearofmanufacture")) return "Year Of Manufacture";
                else if (columnName.ToLower().Equals("metermodelno")) return "Meter Model No.";

                else if (columnName.ToLower().Equals("internalfirmwareversion")) return "Internal Firmware Version";
                else if (columnName.ToLower().Equals("voltagerating")) return "Voltage Rating";
                //else if (columnName.ToLower().Equals("basiccurrentrating")) return "Current Rating (Ib-Imax)";
                else if (columnName.ToLower().Equals("basiccurrentrating")) return "Current Rating";//PGVCL
                // else if (columnName.ToLower().Equals("currentrating")) return "Current Rating (Ib-Imax)";
                else if (columnName.ToLower().Equals("currentrating")) return "Current Rating"; //PGVCL
                else if (columnName.ToLower().Equals("primarymeterconstantinfo")) return "Primary Meter Constant";//PGVCL
                else if (columnName.ToLower().Equals("meterconstantinfo")) return "Meter Constant";

                else if (columnName.ToLower().Equals("metermonthofmanufacture")) return "Month Of Manufacture";
                else if (columnName.ToLower().Equals("accuracyclass")) return "Accuracy Class";
                else return "";
            }
            else
            {
                if (columnName.ToLower().Equals("meterserialnumber")) return "Meter Serial Number";
                else if (columnName.ToLower().Equals("manufacturername")) return "Manufacturer Name";
                else if (columnName.ToLower().Equals("firmwareversionformeter")) return "Firmware Version";
                else if (columnName.ToLower().Equals("metertype")) return "Meter Type";
                else if (columnName.ToLower().Equals("internalctratio")) return "Internal CT Ratio";
                else if (columnName.ToLower().Equals("internalptratio")) return "Internal PT Ratio";
                else if (columnName.ToLower().Equals("internalvtratio")) return "Internal VT Ratio";
                else if (columnName.ToLower().Equals("category")) return "Category";
                else if (columnName.ToLower().Equals("meteryearofmanufacture")) return "Year Of Manufacture";
                else if (columnName.ToLower().Equals("metermodelno")) return "Meter Model No.";

                else if (columnName.ToLower().Equals("internalfirmwareversion")) return "Internal Firmware Version";
                else if (columnName.ToLower().Equals("voltagerating")) return "Voltage Rating";
                //else if (columnName.ToLower().Equals("basiccurrentrating")) return "Current Rating (Ib-Imax)";
                else if (columnName.ToLower().Equals("basiccurrentrating")) return "Current Rating"; //PGVCL
                // else if (columnName.ToLower().Equals("currentrating")) return "Current Rating (Ib-Imax)";
                else if (columnName.ToLower().Equals("currentrating")) return "Current Rating "; //PGVCL

                else if (columnName.ToLower().Equals("primarymeterconstantinfo")) return "Primary Meter Constant";//PGVCL
                else if (columnName.ToLower().Equals("meterconstantinfo")) return "Secondary Meter Constant";

                else if (columnName.ToLower().Equals("metermonthofmanufacture")) return "Month Of Manufacture";
                else if (columnName.ToLower().Equals("accuracyclass")) return "Accuracy Class";
                else return "";
            }
        }
        public DataSet ConvertTariffEnergyTODMDToColumnForRPT(DataSet ds, bool isHistory, long MeterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            //int emf = new MeterMasterBLL().GetEMF(MeterDataId);
            if (isHistory)
                table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KW), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KWTimeStamp), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVA), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVATimeStamp), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KW_Import), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KWTimeStamp_Import), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVA_Import), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVATimeStamp_Import), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KW_Export), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KWTimeStamp_Export), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVA_Export), typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVATimeStamp_Export), typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;
            for (int counter = 1; counter < ConfigInfo.BillingTariffCount + 1; counter++)
            {
                row = table.NewRow();
                int count = 0;
                string colName1 = "MDkWTZ" + counter.ToString();
                string colName2 = "MDkWDateTimeTZ" + counter.ToString();
                string colName3 = "MDkVATZ" + counter.ToString();
                string colName4 = "MDkVADateTimeTZ" + counter.ToString();
                string colName5 = "MDkWTZ" + counter.ToString() + "Import";
                string colName6 = "MDkWDateTimeTZ" + counter.ToString() + "Import";
                string colName7 = "MDkVATZ" + counter.ToString() + "Import";
                string colName8 = "MDkVADateTimeTZ" + counter.ToString() + "Import";
                string colName9 = "MDkWTZ" + counter.ToString() + "Export";
                string colName10 = "MDkWDateTimeTZ" + counter.ToString() + "Export";
                string colName11 = "MDkVATZ" + counter.ToString() + "Export";
                string colName12 = "MDkVADateTimeTZ" + counter.ToString() + "Export";
                string billingMonth = dataRow["BillingMonth"].ToString();
                if (billingMonth == string.Empty)
                    billingMonth = "---";
                if (isHistory)
                {
                    if (dataRow["DataIndex"].ToString() == "0")
                    {
                        row[count] = "Current" + " (" + billingMonth + ")";
                        count++;
                    }
                    else
                    {
                        //row[count] = "History - " + dataRow[dataRow.ItemArray.Length - 2].ToString() + " (" + billingMonth + ")";
                        row[count] = "History - " + dataRow["DataIndex"].ToString() + " (" + billingMonth + ")";
                        count++;
                    }
                }
                row[count] = tariffNumber.ToString(); count++;
                string val = Convert.ToString(dataRow[colName1]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName2])); count++;
                //Added by Deep for Md ----
                if (Convert.ToString(dataRow[colName1]).IndexOf('*') <= 1)
                    row[count - 2] = "---";

                val = Convert.ToString(dataRow[colName3]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName4])); count++;
                //Added by Deep for Md ----
                if (Convert.ToString(dataRow[colName3]).IndexOf('*') <= 1)
                  row[count - 2] = "---";

                //Import
                
                val = Convert.ToString(dataRow[colName5]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName6])); count++;
                

                val = Convert.ToString(dataRow[colName7]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName8])); count++;
                

                //Export
                val = Convert.ToString(dataRow[colName9]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName10])); count++;
              

                val = Convert.ToString(dataRow[colName11]);
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val; count++;
                row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName12])); count++;
               

                table.Rows.Add(row);
                tariffNumber++;
            }

            string  colName = string.Empty;
            string  colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KW_Import);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KWTimeStamp_Import);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVA_Import);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVATimeStamp_Import);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KW_Export);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KWTimeStamp_Export);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            colName = string.Empty;
            colNameTime = string.Empty;
            colName = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVA_Export);
            colNameTime = CommonMethods.getDisplayHeaderText(GlobalConstants.conTODMD_KVATimeStamp_Export);
            if (table.AsEnumerable().All(dr => dr.IsNull(colName)) || table.AsEnumerable().All(dr => dr[colName].Equals("")))
            {
                table.Columns.Remove(colName);
                table.Columns.Remove(colNameTime);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(MeterDataId, dataSet);
        }
        /// <summary>
        /// This function converts decimal string to hex and then to binary and make a call to gettampertype method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertDecimalToHexString(string value)
        {
            int number;
            string hexString = string.Empty;
            string tamperType = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(value))
                    number = int.Parse(value);
                else
                    number = 0;
                // Getting hex value
                hexString = number.ToString("x").PadLeft(8, '0');
                if (!string.IsNullOrEmpty(hexString))
                {
                    // Getting binary value
                    tamperType = Convert.ToString(Convert.ToInt32(hexString, 16), 2).PadLeft(24, '0');
                    tamperType = GetTamperType(tamperType);
                }
                if (string.IsNullOrEmpty(tamperType))
                {
                    tamperType = "NA";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertDecimalToHexString(string value)", ex);
                throw;
            }
            return tamperType;


        }
        /// <summary>
        /// This function matches the bits values to 1 is tamper is present and assign tamper accordingly.
        /// </summary>
        /// <param name="tamperBits"></param>
        /// <returns></returns>
        private string GetTamperType(string tamperBits)
        {
            string tamperType = string.Empty;
            try
            {
                // Checking the status of tamper.
                if (!string.IsNullOrEmpty(tamperBits))
                {
                    if (ConfigInfo.ActiveMeterType == "1P-2W")
                    {
                        if (tamperBits[10] == '1')
                            tamperType += "A" + ",";  // A - OV (Over Voltage)
                        if (tamperBits[11] == '1')
                            tamperType += "B" + ",";  // B - LV (Low Voltage)
                        if (tamperBits[12] == '1')
                            tamperType += "C" + ",";  // C - OC (Over Current)
                        if (tamperBits[13] == '1')
                            tamperType += "D" + ",";  // D - OL (Over Load)
                        if (tamperBits[14] == '1')
                            tamperType += "E" + ",";  // E - REV (Reverse Tamper)
                        if (tamperBits[15] == '1')
                            tamperType += "F" + ",";  // F - ETH (Earth Tamper)
                        if (tamperBits[16] == '1')
                            tamperType += "G" + ",";  // G - PFT (Power Fail)
                        if (tamperBits[17] == '1')
                            tamperType += "H" + ",";  // H - MGT (Magnetic Tamper)
                        if (tamperBits[18] == '1')
                            tamperType += "I" + ",";  // I - NDT (Neutral Disturbance Tamper)
                        if (tamperBits[19] == '1')
                            tamperType += "J" + ",";  // J - SWT (Single Wire Tamper)
                        if (tamperBits[20] == '1')
                            tamperType += "K" + ",";  // K - CO (Case Open)
                        if (tamperBits[21] == '1')
                            tamperType += "NA" + ","; // NA - CCR (Common card Remove)
                        if (tamperBits[22] == '1')
                            tamperType += "NA" + ","; // NA - RLYM (Relay Malfunction)
                        if (tamperBits[23] == '1')
                            tamperType += "NA" + ","; // NA - RLY (Relay Status Connected/disconnected)

                    }
                    else
                    {
                        // Change the seqence of tamper bytes  to solve bug 72926.
                        if (tamperBits[4] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.LowPF) + ",";
                        if (tamperBits[5] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.OverCurrent) + ",";
                        if (tamperBits[6] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.UnderVoltage) + ",";
                        if (tamperBits[7] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.OverVoltage) + ",";
                        if (tamperBits[8] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.VoltImbalance) + ",";
                        if (tamperBits[9] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.CurrentImbalance) + ",";
                        if (tamperBits[10] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.NeutralDisturbance) + ",";
                        if (tamperBits[11] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseCTReversal) + ",";
                        if (tamperBits[12] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseCTReversal) + ",";
                        if (tamperBits[13] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseCTReversal) + ",";
                        if (tamperBits[14] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseCTOpen) + ",";
                        if (tamperBits[15] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseCTOpen) + ",";
                        if (tamperBits[16] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseCTOpen) + ",";
                        if (tamperBits[17] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.CTByPass) + ",";
                        if (tamperBits[18] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseMissingPotential) + ",";
                        if (tamperBits[19] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseMissingPotential) + ",";
                        if (tamperBits[20] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseMissingPotential) + ",";
                        if (tamperBits[21] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.FrontCover) + ",";
                        // Removed terminal cover for bug 75298.
                        //if (tamperBits[22] == '1')
                        //    tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.TerminalCover) + ",";
                        if (tamperBits[23] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.Magnet) + ",";
                        //New Tamper Added
                        if (tamperBits[0] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.OverLoad) + ",";
                        if (tamperBits[1] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.InvalidVolatge) + ",";
                        if (tamperBits[2] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.InvalidPhaseVolatge) + ",";
                        if (tamperBits[3] == '1')
                            tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.TwoPN) + ",";
                    }
                }
                if (!string.IsNullOrEmpty(tamperType))
                {
                    tamperType = tamperType.Substring(0, tamperType.Length - 1);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperType(string tamperBits)", ex);
                throw;
            }
            return tamperType;

        }
        public bool CheckString(string oldString, string newString)
        {
            if (string.IsNullOrEmpty(oldString))
                return false;
            if (string.IsNullOrEmpty(newString))
                return false;
            oldString = oldString.Replace("\t", "");
            newString = newString.Replace("\t", "");
            oldString = ((oldString.Replace(" ", "")).Replace("–", "")).ToUpper().Replace("-", "");
            newString = ((newString.Replace(" ", "")).Replace("–", "")).ToUpper().Replace("-", "");
            oldString = ((oldString.Replace("+", "")).Replace(")", "")).ToUpper().Replace("(", "");
            newString = ((newString.Replace("+", "")).Replace(")", "")).ToUpper().Replace("(", "");
            if (oldString.Equals(newString))
                return true;
            else
                return false;
        }

        public DataSet ApplyBillingEMF(DataSet dataSet)
        {
            long meterDataID = 0;
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();
            string colName = "";
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                colName = col.ColumnName;
                if (colName == "MeterData_ID")
                    continue;
                table.Columns.Add(new DataColumn(colName));
            }
            DataRow dr = table.NewRow();
            int counter = 0;
            int rowCounter = 0;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                dr = table.NewRow();
                //int emf = new MeterMasterBLL().GetEMF(Convert.ToInt64(row[dataSet.Tables[0].Columns.Count - 1].ToString()));
                counter = 0;
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    if (col.ColumnName == "MeterData_ID")
                    {
                        meterDataID = Convert.ToInt64(dataSet.Tables[0].Rows[rowCounter][counter]);
                        continue;
                    }
                    string val = dataSet.Tables[0].Rows[rowCounter][counter].ToString();
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = dat[0].ToString();
                    }
					// Story - 365971 - decimal.Parse was breaking if val is empty
                    if (!string.IsNullOrEmpty(val))
                    {
                        if (CheckString("CumulativeEnergykWhTZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ1", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ2", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ3", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ4", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ5", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ6", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ7", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykWhTZ8", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykvarhLag", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykvarhLead", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ1", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ2", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ3", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ4", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ5", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ6", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ7", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergykVAhTZ8", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ1", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ2", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ3", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ4", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ5", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ6", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ7", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkWTZ8", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ1", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ2", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ3", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ4", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ5", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ6", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ7", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDKVATZ8", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("BillingType", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergyFraudkWh", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("CumulativeEnergyFraudkVAh", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        // User Story - 1000867
                        else if (CheckString("MDkVArLagTZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }
                        else if (CheckString("MDkVArLeadTZ0", col.ColumnName))
                        {
                            dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                        }

                        else
                        {
                            dr[counter] = val; counter++;
                        }
                    }
                    else
                    {
                        dr[counter] = val; counter++;
                    }
                }
                rowCounter++;
                table.Rows.Add(dr);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, ds);
        }
        public DataSet ApplyEMF(DataSet dataSet, long meterdataid)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();
            string colName = "";
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                colName = col.ColumnName;
                if (colName == "MeterData_ID")
                    continue;
                table.Columns.Add(new DataColumn(colName));
            }
            DataRow dr = table.NewRow();
            int counter = 0;
            int rowCounter = 0;
            //int emf = new MeterMasterBLL().GetEMF(meterdataid);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                dr = table.NewRow();
                counter = 0;
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    string val = dataSet.Tables[0].Rows[rowCounter][counter].ToString();
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = dat[0].ToString();
                    }
                    if (string.IsNullOrEmpty(val))
                        val = "----";
                    if (CheckString("CurrentIR", col.ColumnName))
                    {
                        dr[counter] = (val == "----") ? val : (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CurrentIY", col.ColumnName))
                    {
                        dr[counter] = (val == "----") ? val : (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CurrentIB", col.ColumnName))
                    {
                        dr[counter] = (val == "----") ? val : (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CumulativeEnergykWh", col.ColumnName))
                    {
                        dr[counter] = (val == "----") ? val : (decimal.Parse(val)).ToString(); counter++;
                    }
                    else
                    {
                        dr[counter] = val; counter++;
                    }
                }
                rowCounter++;
                table.Rows.Add(dr);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterdataid, ds);
        }
        public DataSet ApplyEMF(DataSet dataSet)
        {
            long meterDataID = 1;
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();
            string colName = "";
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                colName = col.ColumnName;
                if (colName == "MeterData_ID")
                    continue;
                table.Columns.Add(new DataColumn(colName));
            }
            DataRow dr = table.NewRow();
            int counter = 0;
            int rowCounter = 0;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                dr = table.NewRow();
                //int emf = new MeterMasterBLL().GetEMF(Convert.ToInt64(row[dataSet.Tables[0].Columns.Count - 1].ToString()));
                counter = 0;
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {
                    if (col.ColumnName == "MeterData_ID")
                    {
                        meterDataID = Convert.ToInt64(dataSet.Tables[0].Rows[rowCounter][counter]);
                        continue;
                    }
                    string val = dataSet.Tables[0].Rows[rowCounter][counter].ToString();
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = dat[0].ToString();
                    }
                    if (CheckString("CurrentIR", col.ColumnName))
                    {
                        dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CurrentIY", col.ColumnName))
                    {
                        dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CurrentIB", col.ColumnName))
                    {
                        dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                    }
                    else if (CheckString("CumulativeEnergykWh", col.ColumnName))
                    {
                        dr[counter] = (decimal.Parse(val)).ToString(); counter++;
                    }
                    else
                    {
                        dr[counter] = val; counter++;
                    }
                }
                rowCounter++;
                table.Rows.Add(dr);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, ds);
        }

        public DataSet ConvertLoadSurvey(DataSet dataSet)
        {
            bool isEnergyKWDouble = false;
            decimal energyKW;
            bool isEnergyKVADouble = false;
            decimal energyKVA;
            decimal powerFactor = 0;
            long meterDataID = 0;
            bool powerFactorSet = false;
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            decimal result;
            DataTable table = new DataTable();
            string colName = "";
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                colName = col.ColumnName;
                if (colName == "MeterData_ID")
                    continue;
                table.Columns.Add(new DataColumn(colName));
            }
            DataRow dr = table.NewRow();
            int counter = 0;
            int rowCounter = 0;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                energyKVA = 0;
                energyKW = 0;
                powerFactorSet = false;
                isEnergyKVADouble = false;
                isEnergyKWDouble = false;
                dr = table.NewRow();
                //int emf = new MeterMasterBLL().GetEMF(Convert.ToInt64(row[dataSet.Tables[0].Columns.Count - 1].ToString()));
                counter = 0;
                foreach (DataColumn col in dataSet.Tables[0].Columns)
                {

                    if (col.ColumnName == "MeterData_ID")
                    {
                        meterDataID = Convert.ToInt32(dataSet.Tables[0].Rows[rowCounter][counter]);
                        continue;
                    }
                    string val = dataSet.Tables[0].Rows[rowCounter][counter].ToString();
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = dat[0].ToString();
                    }
                    if (CheckString("rPhaseCurrent", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("yPhaseCurrent", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("bPhaseCurrent", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("blockEnergykWh", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                        {
                            if (isMVVNL)
                            {
                                isEnergyKWDouble = true;
                                energyKW = result;
                            }
                            dr[counter] = result.ToString();
                        }
                        else
                        {
                            dr[counter] = "--";
                        }
                        counter++;
                    }
                    else if (CheckString("blockEnergykvarhlag", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("blockEnergykvarhlead", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("blockEnergykVAh", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                        {
                            if (isMVVNL)
                            {
                                energyKVA = result;
                                isEnergyKVADouble = true;
                            }
                            dr[counter] = result.ToString();
                        }
                        else
                        {
                            dr[counter] = "--";
                        }
                        counter++;
                    }
                    else if (CheckString("bPhaseVoltage", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("rPhaseVoltage", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("yPhaseVoltage", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    //These two new parameters added for PUMA Load Survey Report; 24th April 2012; Bug 75902   
                    else if (CheckString("Frequency", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                            dr[counter] = result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("TamperStatus", col.ColumnName))
                    {

                        if (decimal.TryParse(val, out result))
                            dr[counter] = ConvertDecimalToHexString(result.ToString());//result.ToString();
                        else
                            dr[counter] = "--";
                        counter++;
                    }
                    else if (CheckString("Power Factor", col.ColumnName))
                    {
                        if (decimal.TryParse(val, out result))
                        {
                            dr[counter] = string.Format("{0:0.000}", result);
                        }
                        else
                        {
                            dr[counter] = "--";
                        }
                    }
                    else
                    {
                        dr[counter] = val; counter++;
                    }


                }
                rowCounter++;
                table.Rows.Add(dr);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactor(meterDataID, ds);
        }
        public string GetColName(string tempName)
        {
            if (CheckString(tempName, "Current - IR"))
            {
                tempName = "R Phase Current";
            }
            else if (CheckString(tempName, "Current - IY"))
            {
                tempName = "Y Phase Current";
            }
            else if (CheckString(tempName, "Current - IB"))
            {
                tempName = "B Phase Current";
            }
            else if (CheckString(tempName, "Voltage - VRN"))
            {
                tempName = "R Phase Voltage";
            }
            else if (CheckString(tempName, "Voltage – VYN"))
            {
                tempName = "Y Phase Voltage";
            }
            else if (CheckString(tempName, "Voltage - VBN"))
            {
                tempName = "B Phase Voltage";
            }
            return tempName;
        }
        /// <summary>
        /// This funtion is made for multiplying CT Ratio,PT Ratio,EMF to intended columns - For DHBVNL June 2011 tender 
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ApplyMultiplyFactor(long meterID, DataSet dataSet)
        {
            if (!IsEMFUseInCaculation(meterID))
                return dataSet;
            string columnName;
            if (meterMasterEntity == null)
            {
                meterMasterEntity = (MeterMasterEntity)GetMutiplyingFactors(meterID) as MeterMasterEntity;
            }
            // if data is not null
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // iterate through columns and find out the column type
                for (int columnCounter = 0; columnCounter < dataSet.Tables[0].Columns.Count; columnCounter++)
                {
                    columnName = dataSet.Tables[0].Columns[columnCounter].ColumnName;

                    //Format the header values then pass to the function. Header text contains {0} needs to process first before sending 
                    //to function GetColumnType
                    ColumnType columnType = GetColumnType(CommonMethods.getDisplayHeaderText(columnName));
                    // if column type is of current,voltage,energy or power type then apply calculations
                    if (columnType != ColumnType.None)
                    {
                        for (int rowCounter = 0; rowCounter < dataSet.Tables[0].Rows.Count; rowCounter++)
                        {
                            dataSet.Tables[0].Rows[rowCounter][columnName] = GetFormatedValue(columnType, dataSet.Tables[0].Rows[rowCounter][columnName].ToString());

                        }
                    }
                }
            }
            return dataSet;
        }
        public DataSet ApplyMultiplyFactor(long meterID, DataSet dataSet, bool isPhasor)
        {
            if (!IsEMFUseInCaculation(meterID))
                return dataSet;
            string columnName;
            if (meterMasterEntity == null)
            {
                meterMasterEntity = (MeterMasterEntity)GetMutiplyingFactors(meterID) as MeterMasterEntity;
            }
            // if data is not null
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // iterate through columns and find out the column type
                for (int columnCounter = 0; columnCounter < dataSet.Tables[0].Columns.Count; columnCounter++)
                {
                    columnName = dataSet.Tables[0].Columns[columnCounter].ColumnName;

                    //Format the header values then pass to the function. Header text contains {0} needs to process first before sending 
                    //to function GetColumnType
                    ColumnType columnType = GetColumnType(CommonMethods.getDisplayHeaderText(columnName), isPhasor);
                    // if column type is of current,voltage,energy or power type then apply calculations
                    if (columnType != ColumnType.None)
                    {
                        for (int rowCounter = 0; rowCounter < dataSet.Tables[0].Rows.Count; rowCounter++)
                        {
                            dataSet.Tables[0].Rows[rowCounter][columnName] = GetFormatedValue(columnType, dataSet.Tables[0].Rows[rowCounter][columnName].ToString());
                        }
                    }
                }
            }
            return dataSet;
        }

        /// <summary>
        /// This funtion is made for multiplying CT Ratio,PT Ratio,EMF to intended columns - For DHBVNL June 2011 tender 
        /// and if the dataset contains grid columns in rows.
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ApplyMultiplyFactor(long meterID, DataSet dataSet, string columnsColumn, string valueColumn)
        {
            if (!IsEMFUseInCaculation(meterID))
                return dataSet;
            if (meterMasterEntity == null)
            {
                meterMasterEntity = (MeterMasterEntity)GetMutiplyingFactors(meterID) as MeterMasterEntity;
            }
            // if data is not null
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // iterate through every row 
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    // Get the column type (Current,Voltage,Energy,Power)
                    ColumnType columnType = GetColumnType(row[columnsColumn].ToString());
                    // only do processing if column is of relevant type 
                    if (columnType != ColumnType.None)
                    {
                        // if the value can be parsed into a decimal only then apply calculation..
                        if (columnType == ColumnType.Count || columnType == ColumnType.Duration) // Story - 427028 - Format of count and duration in analysis view and report
                            row[valueColumn] = CommonBLL.GetFormattedData(row[valueColumn].ToString());
                        else
                            row[valueColumn] = GetFormatedValue(columnType, row[valueColumn].ToString());
                    }
                }
            }
            return dataSet;
        }

        public ColumnType GetColumnType(string columnName)
        {
            if (columnName.ToLower().Contains("date"))
                return ColumnType.None;
            else if (columnName.ToLower().Contains("current"))
                return ColumnType.Current;
            else if (columnName.ToLower().Contains("voltage"))
                return ColumnType.Voltage;
            else if (columnName.ToLower().Contains("kw") || columnName.ToLower().Contains("kvar") || columnName.ToLower().Contains("kva") || columnName.ToLower().Contains("mw") || columnName.ToLower().Contains("mvar") || columnName.ToLower().Contains("mva") || columnName.ToLower().Contains("power (abs)"))
                return ColumnType.Power;
            else if (columnName.ToLower().Contains("kwh") || columnName.ToLower().Contains("kvarh") || columnName.ToLower().Contains("kvah") || columnName.ToLower().Contains("mwh") || columnName.ToLower().Contains("mvarh") || columnName.ToLower().Contains("mvah"))
                return ColumnType.Energy;
            else if (columnName.ToLower().Contains("tamper count")) // Story - 427028 - Format of count and duration in analysis view and report
                return ColumnType.Count;
            else if (columnName.ToLower().Contains("tamper duration"))
                return ColumnType.Duration;
            else
                return ColumnType.None;
        }
        public ColumnType GetColumnType(string columnName, bool isPhasor)
        {
            if (columnName.ToLower().Contains("date"))
                return ColumnType.None;
            else if (columnName.ToLower().Contains("current"))
                return ColumnType.Current;
            else if (columnName.ToLower().Contains("voltage"))
                return ColumnType.Voltage;
            else if (columnName.ToLower().Contains("power (abs)") || columnName.ToLower().Contains("activepower") || columnName.ToLower().Contains("apparentpower") || columnName.ToLower().Contains("reactivepower"))
                return ColumnType.Power;
            else
                return ColumnType.None;
        }
        /// <summary>
        /// Get the formated Value with EMF multiplied
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string GetFormatedValue(ColumnType columnType, string strValue)
        {
            int currentResolution = 0;
            decimal targetValue = 0;
            if (decimal.TryParse(strValue, out targetValue))
            {
                if (meterMasterEntity != null)
                {
                    int ctRatio = meterMasterEntity.InternalCTRatio;
                    int ptRatio = meterMasterEntity.InternalPTRatio;
                    int installedCTRatio = meterMasterEntity.MeterInstalledCTRatio;
                    int installedPTRatio = meterMasterEntity.MeterInstalledPTRatio;
                    if (targetValue != 0)
                    {

                        if (ctRatio <= 0 || ptRatio <= 0)
                        {
                            ctRatio = 1;
                            ptRatio = 1;
                        }
                        if (columnType == ColumnType.Current)
                        {
                            targetValue = (targetValue * installedCTRatio) / ctRatio;
                        }
                        else if (columnType == ColumnType.Voltage)
                        {
                            targetValue = (targetValue * installedPTRatio) / ptRatio;
                        }
                        else if ((columnType == ColumnType.Energy) || columnType == ColumnType.Power)
                        {
                            targetValue = (targetValue * installedCTRatio * installedPTRatio) / (ctRatio * ptRatio);
                        }
                        currentResolution = strValue.Substring(strValue.LastIndexOf(".") + 1).Length;
                        if (currentResolution < strValue.Length)
                        {
                            strValue = targetValue.TruncateToPrecision(currentResolution);
                        }
                        else
                        {
                            strValue = targetValue.ToString();
                        }

                    }
                }
            }
            return strValue;

        }

        public IEntity GetMutiplyingFactors(long activeMeterID)
        {
            return (new MeterMasterBLL().GetMultiplyingFactors(activeMeterID));
        }
        public bool IsEMFUseInCaculation(long meterDataID)
        {
            return (new MeterMasterDAL().IsEMFUseinCalculation(meterDataID));
        }
        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }
        private string GetFileName(long meterDataID)
        {
            DataSet fileDataset = new DataSet();
            string fileName = string.Empty;
            fileDataset = ListFileName(meterDataID);
            if (fileDataset != null)
            {
                if (fileDataset.Tables[0].Rows.Count > 0)
                {
                    fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                }
            }
            return fileName;
        }
        /// <summary>
        /// Used to convert power off duration
        /// </summary>
        /// <param name="powerOffData"></param>
        /// <returns></returns>
        public DataSet ConvertPowerOffDuration(DataSet powerOffData)
        {
            if (powerOffData == null && powerOffData.Tables.Count == 0 && powerOffData.Tables[0].Rows.Count == 0)
                return null;

            DataSet dataSet = new DataSet();
            try
            {
                if (powerOffData != null && powerOffData.Tables != null && powerOffData.Tables[0].Rows.Count > 0)
                {
                    byte historyValue;
                    ulong powerOffValue;
                    ulong previousPowerOffValue = 0;
                    ulong powerOffConsumption = 0;
                    ulong cumulativePowerOffValue;  
              
                     DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("History", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
					table.Columns.Add(new DataColumn("Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm", typeof(System.String)));

                    DataRow row;
                    for (int rowCount = powerOffData.Tables[0].Rows.Count - 1; rowCount >= 0; rowCount--)
                    {
                        row = table.NewRow();
                        historyValue = Convert.ToByte(powerOffData.Tables[0].Rows[rowCount]["History"]);
                        powerOffValue = Convert.ToUInt32(CheckUnit(powerOffData.Tables[0].Rows[rowCount]["Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm"].ToString())[0]);
                        cumulativePowerOffValue = Convert.ToUInt32(CheckUnit(powerOffData.Tables[0].Rows[rowCount]["Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm"].ToString())[0]);
                        if (historyValue == 0)
                        {
                            row[0] = "Current";
                        }
                        else
                        {
                            row[0] = "History - " + historyValue.ToString();
                        }
                        row[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOffData.Tables[0].Rows[rowCount][1])).ToString();

                        powerOffConsumption = GetPowerOffHoursForRollOverData(powerOffValue, previousPowerOffValue);

                        row[2] = ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(powerOffConsumption));
                        row[3] = ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(cumulativePowerOffValue));
                        previousPowerOffValue = powerOffValue;
                        table.Rows.Add(row);
                    }
                    dataSet.Tables.Add(table);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ConvertPowerOffDuration(DataSet powerOffData)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Used to calculate Loadfactor
        /// </summary>
        /// <param name="powerOffData"></param>
        /// <returns></returns>
        public DataSet GetFormattedLoadFactorInput(DataSet powerOnInputData)
        {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("BillingWisePowerOnHours", typeof(System.String)));
            table.Columns.Add(new DataColumn("KWHConsumption", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDKW", typeof(System.String)));

            try
            {
                DataRow row;
                //int counter = 0;
                int counterInner = 1;
                long totalMinutes;
                for (int counter = 0; counter <= 11; counter++, counterInner++)
                {
                    if (powerOnInputData.Tables[0].Rows.Count > counter)
                    {
                        DataRow firstRow = powerOnInputData.Tables[0].Rows[counter];
                        if (counterInner == 12)
                            break;
                        if (powerOnInputData.Tables[0].Rows.Count > counterInner)
                        {
                            DataRow secondRow = powerOnInputData.Tables[0].Rows[counterInner];
                            row = table.NewRow();
                            row[0] = firstRow[0];
                            totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                            row[1] = string.Format("{0:0.00}", (TimeSpan.FromMinutes((totalMinutes - Convert.ToInt64(firstRow[4]))).TotalHours));
                            row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));
                            row[3] = SplitWithOutUnit(Convert.ToString(firstRow[3]));

                            table.Rows.Add(row);
                        }
                    }
                }

                dataSet.Tables.Add(table);
                return dataSet;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFormattedLoadFactorInput(DataSet powerOnInputData)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Used to calculate Loadfactor
        /// </summary>
        /// <param name="powerOffData"></param>
        /// <returns></returns>
        public DataSet GetPowerOnDuration(DataSet powerOnInputData)
        {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn(BCSConstants.PowerOnColumn, typeof(System.String)));

            try
            {
                DataRow row;
                //int counter = 0;
                int counterInner = 1;
                long totalMinutes;
                for (int counter = 0; counter <= 11; counter++, counterInner++)
                {
                    if (powerOnInputData.Tables[0].Rows.Count > counter)
                    {
                        DataRow firstRow = powerOnInputData.Tables[0].Rows[counter];
                        if (counterInner == 12)
                            break;
                        if (powerOnInputData.Tables[0].Rows.Count > counterInner)
                        {
                            DataRow secondRow = powerOnInputData.Tables[0].Rows[counterInner];
                            row = table.NewRow();
                            if (Convert.ToInt32(firstRow[0]) == 0)
                            {
                                row[0] = "Current";
                            }
                            else
                            {
                                row[0] = "History - " + Convert.ToInt32(firstRow[0]).ToString();
                            }
                            totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                            row[1] = (TimeSpan.FromMinutes((totalMinutes - Convert.ToInt64(firstRow[2]))).Days).ToString("00")
                                      + " : " + (TimeSpan.FromMinutes((totalMinutes - Convert.ToInt64(firstRow[2]))).Hours).ToString("00")
                                      + ": " + (TimeSpan.FromMinutes((totalMinutes - Convert.ToInt64(firstRow[2]))).Minutes).ToString("00");
                            table.Rows.Add(row);
                        }
                    }
                }

                dataSet.Tables.Add(table);
                return dataSet;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerOnDuration(DataSet powerOnInputData)", ex);
            }
            return dataSet;
        }

        public DataSet DailyEnergyConsumption(long meterDataID, long lsFromDateMD, long lsToDateMD)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Event Date Time (0.0.1.0.0.255;8;2)", typeof(System.String)));    //SarkarA code change 20180405 //fix attribute
            table.Columns.Add(new DataColumn("Cumulative Energy - kWh Import(1.0.1.8.0.255;3;2)", typeof(System.String)));//pks
            table.Columns.Add(new DataColumn("Cumulative Energy - kVAh Import(1.0.9.8.0.255;3;2)", typeof(System.String)));//pks
            table.Columns.Add(new DataColumn("Cumulative Energy - kvarh(Lag) (1.0.5.8.0.255;3;2)", typeof(System.String)));
            table.Columns.Add(new DataColumn("Cumulative Energy - kvarh(Lead) (1.0.8.8.0.255;3;2)", typeof(System.String)));
            DLMS650LoadSurveyBLL loadSurveyBLL = new DLMS650LoadSurveyBLL();
            // Added to solve dailu consumption data difference in fast download and direct read out issue 
            string fileName = string.Empty;
            DataSet dataSet = new DataSet();
            fileName = GetFileName(meterDataID);
            if (!string.IsNullOrEmpty(fileName))
                dataSet = loadSurveyBLL.GetDailyConsumption(fileName, meterDataID, lsFromDateMD, lsToDateMD);
            //////////////////////////////////////////////////////////////////////////////////
            List<DailyConsumption> lstDC = new List<DailyConsumption>();

            if (dataSet != null || dataSet.Tables.Count > 0 || dataSet.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DailyConsumption dailyC = new DailyConsumption();

                    // Fix to solve DLMS_0075
                    // Added to solve bug 74505. Removed calculation of consumption with difference.
                    //To solve bug 94243 . Added obis codes.
                    if (dataSet.Tables[0].Rows[0][DAILYKWH].ToString() != string.Empty)
                        dailyC.KWh = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[i][DAILYKWH])).ToString("0.000"));
                    if (dataSet.Tables[0].Rows[0][DAILYKVAH].ToString() != string.Empty)
                        dailyC.KVAh = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[i][DAILYKVAH])).ToString("0.000"));
                    if (dataSet.Tables[0].Rows[0][DAILYKVARHLAG].ToString() != string.Empty)
                        dailyC.Kvarh_Lag = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[i][DAILYKVARHLAG])).ToString("0.000"));
                    if (dataSet.Tables[0].Rows[0][DAILYKVARHLEAD].ToString() != string.Empty)
                        dailyC.Kvarh_Lead = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[i][DAILYKVARHLEAD])).ToString("0.000"));

                    lstDC.Add(dailyC);
                }
                //for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                //{
                //    DailyConsumption dailyC = new DailyConsumption();

                //    // Fix to solve DLMS_0075
                //    if (i == 0)
                //    {
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kWh"].ToString() != string.Empty)
                //            dailyC.KWh = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[0]["Daily Consumption - kWh"])).ToString("0.000"));
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - KVAh"].ToString() != string.Empty)
                //            dailyC.KVAh = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[0]["Daily Consumption - KVAh"])).ToString("0.000"));
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lag"].ToString() != string.Empty)
                //            dailyC.Kvarh_Lag = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lag"])).ToString("0.000"));
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lead"].ToString() != string.Empty)
                //            dailyC.Kvarh_Lead = Convert.ToDouble((Convert.ToDouble(dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lead"])).ToString("0.000"));
                //    }
                //    else
                //    {
                //        //Daily Consumption - kWh
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kWh"].ToString() != string.Empty)
                //        {
                //            if (Convert.ToInt32(dataSet.Tables[0].Rows[i]["Daily Consumption - kWh"]) == 0)
                //                dailyC.KWh = 0;
                //            else
                //                dailyC.KWh = Convert.ToDouble(Math.Abs(Convert.ToDouble(dataSet.Tables[0].Rows[i]["Daily Consumption - kWh"])));
                //        }
                //        else
                //            dailyC.KWh = 0;

                //        //Daily Consumption - kVAh
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - KVAh"].ToString() != string.Empty)
                //        {
                //            if (Convert.ToInt32(dataSet.Tables[0].Rows[i]["Daily Consumption - kVAh"]) == 0)
                //                dailyC.KVAh = 0;
                //            else
                //                dailyC.KVAh = Convert.ToDouble(Math.Abs(Convert.ToDouble(dataSet.Tables[0].Rows[i]["Daily Consumption - kVAh"])));
                //        }
                //        else
                //            dailyC.KVAh = 0;

                //        //Daily Consumption - kvarh Lag
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lag"].ToString() != string.Empty)
                //        {
                //            if (Convert.ToInt32(dataSet.Tables[0].Rows[i]["Daily Consumption - kvarh Lag"]) == 0)
                //                dailyC.Kvarh_Lag = 0;
                //            else
                //                dailyC.Kvarh_Lag = Convert.ToDouble(Math.Abs(Convert.ToDouble(dataSet.Tables[0].Rows[i]["Daily Consumption - kvarh Lag"])));
                //        }
                //        else
                //            dailyC.Kvarh_Lag = 0;

                //        //Daily Consumption - kvarh Lead
                //        if (dataSet.Tables[0].Rows[0]["Daily Consumption - kvarh Lead"].ToString() != string.Empty)
                //        {
                //            if (Convert.ToInt32(dataSet.Tables[0].Rows[i]["Daily Consumption - kvarh Lead"]) == 0)
                //                dailyC.Kvarh_Lead = 0;
                //            else
                //                dailyC.Kvarh_Lead = Convert.ToDouble(Math.Abs(Convert.ToDouble(dataSet.Tables[0].Rows[i]["Daily Consumption - kvarh Lead"])));
                //        }
                //        else
                //            dailyC.Kvarh_Lead = 0;
                //    }
                //    lstDC.Add(dailyC);
                //}
                DataRow row;
                int listIndex = 0;
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    row = table.NewRow();
                    row[0] = dr[0];
                    row[1] = string.Format("{0:0.000}", lstDC[listIndex].KWh);
                    row[2] = string.Format("{0:0.000}", lstDC[listIndex].KVAh);
                    row[3] = string.Format("{0:0.000}", lstDC[listIndex].Kvarh_Lag);
                    row[4] = string.Format("{0:0.000}", lstDC[listIndex].Kvarh_Lead);
                    table.Rows.Add(row);
                    listIndex++;
                }
                dataSet = new DataSet();
                dataSet.Tables.Add(table);

            }

            return ApplyMultiplyFactor(meterDataID, dataSet);
            //////////////////////////////////////////////////////////////////////////////////
        }
        /// <summary>
        /// Converts LoadSurvey For MeterID
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="type"></param>
        /// <param name="meterId">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet ConvertLoadSurveyForMeterID(DataSet dataSet, string type, string meterId, bool isPadding)
        {           

            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            string powerfactor = "powerfactor";
            DataTable table = GetLSTableColumns(type, dataSet);

            int counter = 0;
            //int emf = new MeterMasterBLL().GetEMF(meterDataId);
            int div = 1;
            //following condition added to avoid exception in case of singlt L.S. record; 25th April 2012

            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                if (dataSet.Tables[0].Rows.Count > 1)
                {
                    TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[0][0].ToString()));
                    MDInterval = (int)ts.TotalMinutes;
                }
            }
            else
            {
                MDInterval = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
                //To handle LIFO and FIFO issue for LP padding data
                MDInterval = -MDInterval;
            }

            dataSet.Tables[0].Columns.Remove("MDIntervalPeriod");
            dataSet.AcceptChanges();

            //Added a new check for (-) in MDInterval for Load survey LIFO and FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 15 || MDInterval == -15)
                div = 4;
            //Added a new check for (-) in MDInterval for Load survey LIFO and FIFO withPadding case AGAINST BUG ID 446292
            if (MDInterval == 30 || MDInterval == -30)
                div = 2;

            // Added to solve bug 72902. Checking the condition if integration period is chaging in between.
            if (ConfigInfo.ActiveFileType == "DLMS")
            {
                if (dataSet.Tables[0].Rows.Count > 1)
                {
                    for (int row = 0; row < dataSet.Tables[0].Rows.Count - 1; row++)
                    {
                        TimeSpan timeDiff = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row + 1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[row][0].ToString()));
                        int integrationPeriod = (int)timeDiff.TotalMinutes;
                        // Checking if any integration period mismatch is found.
                        if (integrationPeriod != MDInterval)
                        {
                            if (integrationPeriod == 15 || integrationPeriod == 30 || integrationPeriod == 60)
                            {
                                IntegrationPeriodStatus = true;
                                return null;
                            }

                        }
                    }
                }
            }
            
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow nRow = table.NewRow();
                double energyKW = 0;
                double energyKVA = 0;
                double powerFactor = 0;
                bool isEnergyKWDouble = false;
                bool isEnergyKVADouble = false;
                nRow[0] = dataSet.Tables[0].Rows[counter][0].ToString();
                for (int colCount = 1; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("Voltage"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                    }
                    else if (dataSet.Tables[0].Columns[colCount].ColumnName.ToUpper().Contains("ENERGY"))
                    {
                        nRow[colCount] = CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0];
                        /* VBM - Make Avg power factor configurable in LS */
                        if (type != "Demand")
                        {
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykWh")
                            {
                                isEnergyKWDouble = double.TryParse(nRow[colCount].ToString(), out energyKW);
                            }
                            if (dataSet.Tables[0].Columns[colCount].ColumnName == "blockEnergykVAh")
                            {
                                isEnergyKVADouble = double.TryParse(nRow[colCount].ToString(), out energyKVA);
                            }
                            if (isEnergyKVADouble && isEnergyKWDouble)
                            {
                                if (energyKVA != 0)
                                {
                                    powerFactor = energyKW / energyKVA;
                                }
                                else
                                {
                                    powerFactor = 0;
                                }

                                nRow["Power Factor"] = string.Format("{0:0.0000}", powerFactor);
                            }

                        }
                    }
                    else
                    {
                        if (dataSet.Tables[0].Columns[colCount].ColumnName.Contains("tamperStatus"))
                        {
                            nRow[colCount] = ConvertDecimalToHexString((CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                        }
                        else
                        {
                            // Checking null values
                            if (!string.IsNullOrEmpty(dataSet.Tables[0].Rows[counter][colCount].ToString()))
                            {
                                nRow[colCount] = (decimal.Parse(CheckUnit(Convert.ToString(dataSet.Tables[0].Rows[counter][colCount]))[0])).ToString();
                            }
                            else
                            {
                                nRow[colCount] = "-------";
                            }
                        }

                    }
                }
                table.Rows.Add(nRow);
            }

            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            //table = GetLSTable(type);
            table = dataSet.Tables[0].Clone();//GetLSTableColumns(type, dataSet);
            DataRow secondRow = null;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow[0]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow[0]));

                DataRow row = table.NewRow();
                string val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    string colNames = dataSet.Tables[0].Columns[i].ColumnName;
                    string demandVal = CheckUnit(Convert.ToString(firstRow[i]))[0];
                    if (!type.Trim().ToUpper().Equals("ENERGY") && ConfigInfo.ActiveFileType == "DLMS")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.2.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.147.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.10.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.148.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.3.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.4.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.155.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.156.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.128.29.1.255;3;2)") ||
                        colNames.Contains("(1.0.21.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.41.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.61.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.7.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.6.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.149.128.128.255;3;2)") ||
                        colNames.Contains("(1.0.150.128.128.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 1)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) * div;
                                /* VBM - Apply fix 3 digit resolution to demand */
                                //row[i] = CheckDecimal(num.ToString());
                                row[i] = num.TruncateToPrecision(3);
                                /* VBM - Apply fix 3 digit resolution to demand */
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else if (!type.Trim().ToUpper().Equals("DEMAND") && ConfigInfo.ActiveFileType == "NONDLMS")
                    {
                        if (colNames.Contains("(1.0.1.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.5.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.8.29.0.255;3;2)") ||
                        colNames.Contains("(1.0.9.29.0.255;3;2)"))
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                decimal num = 0;
                                if (!string.IsNullOrEmpty(demandVal))
                                    num = decimal.Parse(demandVal) / div;
                                row[i] = num.TruncateToPrecision(3);
                            }
                            else
                            {
                                row[i] = null;
                            }
                        }
                        else
                            row[i] = CheckDecimal(demandVal);
                    }
                    else
                        row[i] = CheckDecimal(demandVal);
                }
                if (!type.Trim().ToUpper().Equals("ENERGY") && dataSet.Tables[0].Rows.Count == 1 && ConfigInfo.ActiveFileType == "DLMS")
                {
                    MessageBox.Show("Demand values cannot be calculated with a single record", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                table.Rows.Add(row);
                bool flag = true;
                #region Data for PUMA
                if (isPUMA)
                {
                    DateTime tempDate = firstRowDate.AddMinutes(MDInterval);
                    TimeSpan ts = secondRowDate.Subtract(tempDate);

                    // negative time stamp is change to Positive AGAINST BUG ID 446292 Padding is not correct in load survey.
                    //if (ts != null && ts.ToString().Contains('-'))
                    //    ts = -(ts);

                    int min = 0;
                    if (ts.Hours == 0 & ts.Minutes == 0)
                    {
                        if (ts.Days != 0)
                        {
                            min = ts.Days * 24 * 60;
                        }
                    }
                    else if (ts.Days == 0 && ts.Hours != 0 && ts.Minutes != 0)
                    {
                        min = ts.Hours * 60 + ts.Minutes;
                    }
                    else
                    {
                        if (ts.Days != 0)
                        {
                            min = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
                        }
                    }
                    //
                    if (isPadding)
                    {
                        if (MDInterval != 0)
                        {
                            int noOfInterval = min / mdInterval;

                            if (min != 0)
                            {

                                for (int i = 0; i < noOfInterval; i++)
                                {
                                    firstRowDate = firstRowDate.AddMinutes(MDInterval);
                                    {
                                        row = table.NewRow();
                                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                                        row[0] = val;
                                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                                        {
                                            if (dataSet.Tables[0].Columns[j].ColumnName == "PowerFactor")
                                                row[j] = "NPF";
                                            else
                                                row[j] = "ND";
                                        }
                                        table.Rows.Add(row);
                                    }

                                }
                            }
                        }
                    }
                }
                #endregion
                else
                {
                    do
                    {
                        if (firstRowDate == secondRowDate)
                            break;
                        firstRowDate = firstRowDate.AddMinutes(MDInterval);
                        if (!firstRowDate.Equals(secondRowDate))
                        {
                            row = table.NewRow();
                            val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                            row[0] = val;
                            for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            {
                                if (dataSet.Tables[0].Columns[j].ColumnName == "PowerFactor")
                                    row[j] = "NPF";
                                else
                                    row[j] = "ND";
                            }
                            table.Rows.Add(row);
                        }
                        else
                            flag = false;
                    } while (flag);
                }
            }


            #region "Net Calculation"
            foreach (DataRow itemRow in table.Rows)
            {
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}WH")))
                {
                    try
                    {
                        DataColumn clmNETKWH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}WH"))];
                        decimal NetKWHValue = 0;
                        decimal CumKWHValue = 0;
                        decimal CumKWHExportValue = 0;
                        string meterVariant = GetMeterVariantByMeterID(meterId);

                        if (meterVariant == CAB.Framework.MeterVariant.THREE)
                        {
                            if (type.Equals("Demand"))
                            {
                                CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)]);
                                CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKW)]);
                            }
                            else
                            {
                                CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)]);
                                CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandEnergyKWH)]);
                            }

                            NetKWHValue = CumKWHValue - CumKWHExportValue;
                            itemRow[clmNETKWH.ColumnName] = NetKWHValue;
                        }
                        else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                        {
                            if (type.Equals("Demand"))
                            {
                                CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWExport)]);
                                CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKWImport)]);
                            }
                            else
                            {
                                CumKWHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHExport)]);
                                CumKWHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKWHImport)]);
                            }

                            NetKWHValue = CumKWHValue - CumKWHExportValue;
                            itemRow[clmNETKWH.ColumnName] = NetKWHValue;
                        }
                        else
                        {
                            itemRow[clmNETKWH.ColumnName] = "-------";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ConvertLoadSurveyForMeterID(DataSet dataSet, string type, string meterId, bool isPadding)", ex);
                    }

                }
                if (table.Columns.Contains(CommonMethods.getDisplayHeaderText("NET - {0}VAH")))
                {
                    try
                    {
                        DataColumn clmNETKVAH = table.Columns[table.Columns.IndexOf(CommonMethods.getDisplayHeaderText("NET - {0}VAH"))];
                        string meterVariant = GetMeterVariantByMeterID(meterId);
                        decimal NetKVAHValue = 0;
                        decimal CumKVAHValue = 0;
                        decimal CumKVAHExportValue = 0;
                        if (meterVariant == CAB.Framework.MeterVariant.THREE)
                        {
                            if (type.Equals("Demand"))
                            {
                                CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)]);
                                CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVA)]);
                            }
                            else
                            {
                                CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)]);
                                CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAH)]);
                            }
                            NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                            itemRow[clmNETKVAH.ColumnName] = NetKVAHValue;
                        }
                        else if (meterVariant == CAB.Framework.MeterVariant.FOUR)
                        {
                            if (type.Equals("Demand"))
                            {
                                CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAExport)]);
                                CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSBlockDemandKVAImport)]);
                            }
                            else
                            {
                                CumKVAHExportValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHExport)]);
                                CumKVAHValue = Convert.ToDecimal(itemRow[CommonMethods.getDisplayHeaderText(GlobalConstants.conLSEnergyKVAHImport)]);
                            }
                            NetKVAHValue = CumKVAHValue - CumKVAHExportValue;
                            itemRow[clmNETKVAH.ColumnName] = NetKVAHValue;
                        }
                        else
                        {
                            itemRow[clmNETKVAH.ColumnName] = "-------";
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "ConvertLoadSurveyForMeterID(DataSet dataSet, string type, string meterId, bool isPadding)", ex);
                    }
                }
            }
            #endregion

            table.AcceptChanges();

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ApplyMultiplyFactorForMeterID(meterId, ds);
        }
        /// <summary>
        /// Applies Multiply Factor For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public DataSet ApplyMultiplyFactorForMeterID(string meterID, DataSet dataSet)
        {
            if (!IsEMFUseInCaculationforMeterID(meterID))
                return dataSet;
            string columnName;
            if (meterMasterEntity == null)
            {
                meterMasterEntity = (MeterMasterEntity)GetMutiplyingFactorsForMeterID(meterID) as MeterMasterEntity;
            }
            // if data is not null
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // iterate through columns and find out the column type
                for (int columnCounter = 0; columnCounter < dataSet.Tables[0].Columns.Count; columnCounter++)
                {
                    columnName = dataSet.Tables[0].Columns[columnCounter].ColumnName;

                    //Format the header values then pass to the function. Header text contains {0} needs to process first before sending 
                    //to function GetColumnType
                    ColumnType columnType = GetColumnType(CommonMethods.getDisplayHeaderText(columnName));
                    // if column type is of current,voltage,energy or power type then apply calculations
                    if (columnType != ColumnType.None)
                    {
                        for (int rowCounter = 0; rowCounter < dataSet.Tables[0].Rows.Count; rowCounter++)
                        {
                            dataSet.Tables[0].Rows[rowCounter][columnName] = GetFormatedValue(columnType, dataSet.Tables[0].Rows[rowCounter][columnName].ToString());

                        }
                    }
                }
            }
            return dataSet;
        }
        /// <summary>
        /// check for EMF Use in Calculation for MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public bool IsEMFUseInCaculationforMeterID(string meterID)
        {
            return (new MeterMasterDAL().IsEMFUseinCalculationforMeterID(meterID));
        }
        /// <summary>
        /// checks for Get Multiplying Factors For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public IEntity GetMutiplyingFactorsForMeterID(string meterID)
        {
            return (new MeterMasterBLL().GetMultiplyingFactorsForMeterID(meterID));
        }

        public DataSet GetMeterModelandFirmware(int meterdataID)
        {
            return (new TamperTypeMasterDAL().GetMeterModelandFirmware(meterdataID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dset"></param>
        /// <returns></returns>
        public DataSet RemoveDuplicateRows(DataSet dset, string colName)
        {
            try
            {
                if (dset != null && dset.Tables != null && dset.Tables.Count > 0)
                {
                    DataTable dTable = dset.Tables[0];
                    Hashtable hTable = new Hashtable();
                    ArrayList duplicateList = new ArrayList();

                    //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
                    //And add duplicate item value in arraylist.
                    foreach (DataRow drow in dTable.Rows)
                    {
                        if (hTable.Contains(drow[colName]))
                            duplicateList.Add(drow);
                        else
                            hTable.Add(drow[colName], string.Empty);
                    }

                    //Removing a list of duplicate items from datatable.
                    foreach (DataRow dRow in duplicateList)
                    {
                        dTable.Rows.Remove(dRow);
                    }

                    //Datatable which contains unique records will be return as output.
                    dset.Tables.RemoveAt(0);
                    dset.Tables.Add(dTable);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "RemoveDuplicateRows(DataSet dset, string colName)", ex);
            }
            return dset;

        }

        public DataSet ConvertTODAvgPFToColumnForRPT(DataSet ds, bool isHistory, long MeterDataId)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            if (ds.Tables[0].Rows[0]["TODAveragePowerFactorTZ1"].ToString() == string.Empty)
                return null;
            if (ds.Tables[0].Rows[0]["TODAverageExportPowerFactorTZ1"].ToString() == string.Empty)//story 1024441 Add TOD Export PF
                return null;
            DataTable table = new DataTable();
            //int emf = new MeterMasterBLL().GetEMF(MeterDataId);
            if (isHistory)
                table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            
            //*****This condition for smart meter change header text tod avg PF ********
            int MeterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(MeterDataId.ToString());
            if (MeterModelNo == NamePlateConstants.Smartmeter_LTCT || MeterModelNo == NamePlateConstants.Smartmeter_WCM)
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.TODAveragePowerFactor_smart), typeof(System.String)));
            }
            else
            {
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.TODAveragePowerFactor), typeof(System.String)));
                table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(GlobalConstants.TODAverageExportPowerFactor), typeof(System.String)));//story 1024441 Add TOD Export PF
            } 
                        

            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariffNumber = 1;
            for (int counter = 1; counter < ConfigInfo.BillingTariffCount + 1; counter++)
            {
                row = table.NewRow();
                int count = 0;
                string colName1 = "TODAveragePowerFactorTZ" + counter.ToString();
                string colName2 = "TODAverageExportPowerFactorTZ" + counter.ToString();//story 1024441 Add TOD Export PF

                string billingMonth = dataRow["BillingMonth"].ToString();
                if (billingMonth == string.Empty)
                    billingMonth = "---";
                if (isHistory)
                {
                    if (dataRow["DataIndex"].ToString() == "0")
                    {
                        row[count] = "Current" + " (" + billingMonth + ")";
                        count++;
                    }
                    else
                    {
                        //row[count] = "History - " + dataRow[dataRow.ItemArray.Length - 2].ToString() + " (" + billingMonth + ")";
                        row[count] = "History - " + dataRow["DataIndex"].ToString() + " (" + billingMonth + ")";
                        count++;
                    }
                }
                row[count] = tariffNumber.ToString();
                string val = Convert.ToString(dataRow[colName1]);
                string val1 = Convert.ToString(dataRow[colName2]);//story 1024441 Add TOD Export PF
                count++;
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = (decimal.Parse(dat[0])).ToString();
                }
                row[count] = val;

                if (val1.IndexOf('*') > 0)//story 1024441 Add TOD Export PF
                {
                    string[] dat = val1.Split('*');
                    val1 = (decimal.Parse(dat[0])).ToString();
                }
                row[count + 1] = val1;

                //val = Convert.ToString(dataRow[colName3]);
                //if (val.IndexOf('*') > 0)
                //{
                //    string[] dat = val.Split('*');
                //    val = (decimal.Parse(dat[0])).ToString();
                //}
                //row[count] = val; count++;
                //row[count] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[colName4])); count++;
                table.Rows.Add(row);
                tariffNumber++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return ApplyMultiplyFactor(MeterDataId, dataSet);
        }
        /// <summary>
        /// Story - 358810 - Format the data of billing as analysis view in CDF converter for single phase non DLMS meter integration
        /// This change need to do in the CDF code, but since this is same project in BCS code as well, so do the changes here then it will 
        /// pick this code only. No need to copy dll from CDF code. Existing behaviuor
        /// </summary>
        /// <param name="value"></param>
        public void GetFormattedValue(ref string value)
        {
            int currentResolution = 0;
            decimal targetValue = 0;

            if (decimal.TryParse(value, out targetValue))
            {
                // if the value can be parsed into a decimal only then apply calculation..
                currentResolution = value.Substring(value.LastIndexOf(".") + 1).Length;
                if (currentResolution < value.Length)
                {
                    value = targetValue.TruncateToPrecision(currentResolution);
                }
                else
                {
                    value = targetValue.ToString();
                }
            }
        }
        /// <summary>
        /// This method is used to calculate Midnight consumption
		/// Story - 365876 - Load Factor calculation
        /// </summary>
        /// <param name="dsLoadFactor"></param>
        /// <param name="dsPowerOn"></param>
        /// <returns></returns>
        public DataSet GetFormattedLoadFactorData(DataSet dsLoadFactor, DataSet dsPowerOn, long MeterDataId)
        {
            DLMS650BillingDAL ds = new DLMS650BillingDAL();//add pradipta_loadfactor
            DataSet catagory = ds.GetMeterCategory(MeterDataId);

            foreach (DataRow dataRow in catagory.Tables[0].Rows)
            {
                Meter_cat = Convert.ToString(dataRow["Category"]);
            }

            DataSet dataSetLoadFactorFinal = null;
            DataTable table = new DataTable();
            DataRow row;
            int counterInner = 1;

            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("BillingWisePowerOnHours", typeof(System.String)));
            //table.Columns.Add(new DataColumn("KWHConsumption", typeof(System.String)));


            if (Meter_cat == "B8" || Meter_cat == "B2")
            {
                table.Columns.Add(new DataColumn("ImportKWHConsumption", typeof(System.String)));
                table.Columns.Add(new DataColumn("ExportKWHConsumption", typeof(System.String)));
            }
            else
            {
                table.Columns.Add(new DataColumn("KWHConsumption", typeof(System.String)));
            }

            table.Columns.Add(new DataColumn("MDKW", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDKWExport", typeof(System.String)));//add pradipta_exportloadfactor
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));

            try
            {
                if (dsLoadFactor != null && dsLoadFactor.Tables.Count > 0 && dsLoadFactor.Tables[0].Rows.Count > 0 && dsPowerOn != null && dsPowerOn.Tables.Count > 0 && dsPowerOn.Tables[0].Rows.Count > 0)
                {
                    dataSetLoadFactorFinal = new DataSet();
                    for (int counter = 0; counter < dsLoadFactor.Tables[0].Rows.Count; counter++, counterInner++)
                    {
                        row = table.NewRow();
                        DataRow firstRow = dsLoadFactor.Tables[0].Rows[counter];

                        if (counterInner == dsLoadFactor.Tables[0].Rows.Count)
                            break;
                        if (dsLoadFactor.Tables[0].Rows.Count > counterInner)
                        {
                            DataRow secondRow = dsLoadFactor.Tables[0].Rows[counterInner];

                            // History
                            row[0] = firstRow[0];

                            // Power ON Hours
                            if (counter < dsPowerOn.Tables[0].Rows.Count)
                                //row[1] = DateUtility.GetHours(dsPowerOn.Tables[0].Rows[counter]["Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm"].ToString());
                                 row[1] = DateUtility.GetHours(dsPowerOn.Tables[0].Rows[counter]["Power On Duration(0.0.94.91.13.255;3;2) dddd:hh:mm"].ToString());
                            else
                                row[1] = "0";

                            //// KWH Consuption
                            if (Meter_cat == "B8" || Meter_cat == "B2")
                            {
                                //Import KWH Consuption
                                row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));

                                //Export KWH Consuption
                                row[3] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[3]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[3]))));

                                // MDKW
                                row[4] = SplitWithOutUnit(Convert.ToString(firstRow[4]));
                                row[5] = SplitWithOutUnit(Convert.ToString(firstRow[5]));//add pradipta_exportloadfactor

                                row[6] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow[1]));
                            }
                            else
                            {
                                row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));

                                // MDKW
                                row[3] = SplitWithOutUnit(Convert.ToString(firstRow[3]));
                                // Billing DateTime
                                row[4] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow[1]));
                            }

                            table.Rows.Add(row);
                        }
                    }
                    dataSetLoadFactorFinal.Tables.Add(table);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFormattedLoadFactorData(DataSet dsLoadFactor, DataSet dsPowerOn, long MeterDataId)", ex);
                dataSetLoadFactorFinal = null;
            }
            return dataSetLoadFactorFinal;
        }
        public DataSet GetFormattedPowerFactorData(DataSet dsPowerFactor, DataSet dataset, long MeterDataId)
        {
            DLMS650BillingDAL ds = new DLMS650BillingDAL();//add pradipta_loadfactor
            DataSet catagory = ds.GetMeterCategory(MeterDataId);

            foreach (DataRow dataRow in catagory.Tables[0].Rows)
            {
                Meter_cat = Convert.ToString(dataRow["Category"]);
            }

            DataSet dataSetLoadFactorFinal = null;
            DataTable table = new DataTable();
            DataRow row;
            int counterInner = 1;

            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("BillingWisePowerOnHours", typeof(System.String)));
            //table.Columns.Add(new DataColumn("KWHConsumption", typeof(System.String)));


            if (Meter_cat == "B8" || Meter_cat == "B2")
            {
                table.Columns.Add(new DataColumn("ImportKWHConsumption", typeof(System.String)));
                table.Columns.Add(new DataColumn("ExportKWHConsumption", typeof(System.String)));
            }
            else
            {
                table.Columns.Add(new DataColumn("KWHConsumption", typeof(System.String)));
            }

            table.Columns.Add(new DataColumn("MDKW", typeof(System.String)));
            table.Columns.Add(new DataColumn("MDKWExport", typeof(System.String)));//add pradipta_exportloadfactor
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));

            try
            {
                if (dsPowerFactor != null && dsPowerFactor.Tables.Count > 0 && dsPowerFactor.Tables[0].Rows.Count > 0 && dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    dataSetLoadFactorFinal = new DataSet();
                    for (int counter = 0; counter < dsPowerFactor.Tables[0].Rows.Count; counter++, counterInner++)
                    {
                        row = table.NewRow();
                        DataRow firstRow = dsPowerFactor.Tables[0].Rows[counter];

                        if (counterInner == dsPowerFactor.Tables[0].Rows.Count)
                            break;
                        if (dsPowerFactor.Tables[0].Rows.Count > counterInner)
                        {
                            DataRow secondRow = dsPowerFactor.Tables[0].Rows[counterInner];

                            // History
                            row[0] = firstRow[0];

                            // Power ON Hours
                            if (counter < dataset.Tables[0].Rows.Count)
                                //row[1] = DateUtility.GetHours(dsPowerOn.Tables[0].Rows[counter]["Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm"].ToString());
                                row[1] = DateUtility.GetHours(dataset.Tables[0].Rows[counter]["Power On Duration(0.0.94.91.13.255;3;2) dddd:hh:mm"].ToString());
                            else
                                row[1] = "0";

                            //// KWH Consuption
                            if (Meter_cat == "B8" || Meter_cat == "B2")
                            {
                                //Import KWH Consuption
                                row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));

                                //Export KWH Consuption
                                row[3] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[3]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[3]))));

                                // MDKW
                                row[4] = SplitWithOutUnit(Convert.ToString(firstRow[4]));
                                row[5] = SplitWithOutUnit(Convert.ToString(firstRow[5]));//add pradipta_exportloadfactor

                                row[6] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow[1]));
                            }
                            else
                            {
                                row[2] = getRolloverValues(Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))), Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2]))));

                                // MDKW
                                row[3] = SplitWithOutUnit(Convert.ToString(firstRow[3]));
                                // Billing DateTime
                                row[4] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow[1]));
                            }

                            table.Rows.Add(row);
                        }
                    }
                    dataSetLoadFactorFinal.Tables.Add(table);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFormattedLoadFactorData(DataSet dsLoadFactor, DataSet dsPowerOn, long MeterDataId)", ex);
                dataSetLoadFactorFinal = null;
            }
            return dataSetLoadFactorFinal;
        }
        /// <summary>
        /// // Story - 427028 - instant data format in analysis view is not correct
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="dataSet"></param>
        /// <param name="columnsColumn"></param>
        /// <param name="valueColumn"></param>
        /// <returns></returns>
        public DataSet ApplyMultiplyFactorForInstant(long meterID, DataSet dataSet, string columnsColumn, string valueColumn)
        {
            string rowValue = string.Empty;
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // iterate through every row 
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    // Get the column type (Current,Voltage,Energy,Power)
                    ColumnType columnType = GetColumnType(row[columnsColumn].ToString());
                    // only do processing if column is of relevant type 
                    if (columnType != ColumnType.None)
                    {
                        // if the value can be parsed into a decimal only then apply calculation..
                        if (columnType == ColumnType.Count || columnType == ColumnType.Duration) // Story - 427028 - Format of count and duration in analysis view and report
                            row[valueColumn] = CommonBLL.GetFormattedData(row[valueColumn].ToString());
                        else
                        {
                            rowValue = row[valueColumn].ToString();
                            GetFormattedValue(ref rowValue);
                            row[valueColumn] = rowValue;
                        }
                    }
                }
            }
            return dataSet;
        }
        private string getMeterType()//add pradipta_temp
        {
            DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
            return generalBLL.GetMeterType(ConfigInfo.ActiveMeterDataId);
        }
        public string ConvertTimSpanToDDDDHH(TimeSpan timeSpan)
        {
         StringBuilder strBuilder = new StringBuilder();
            if (timeSpan.Days.ToString().Contains('-'))
                strBuilder.Append("0");
            else
                strBuilder.Append(timeSpan.Days);
            strBuilder.Append(" : ");
            if (timeSpan.Hours.ToString().Contains('-'))
                strBuilder.Append("00");
            else
                strBuilder.Append(timeSpan.Hours.ToString("00"));           
           
            return strBuilder.ToString();  
        }

        //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
        public void GetReactive(DataTable table, string type)
        {
            try
            {
                string kwhColumn = String.Empty;
                string kvahColumn = String.Empty;
                string kvarhColumn = String.Empty;
                string netColumn = String.Empty;
                bool bLagOrLead = false;
                string meterVariant = String.Empty;
                if (ConfigInfo.ActiveMeterDataId != null)
                {
                    meterVariant = GetMeterVariantByMeterDataID(int.Parse(ConfigInfo.ActiveMeterDataId));
                    if (ConfigInfo.ActiveMeterType.Equals(MeterType.OnePhaseTwoWire) && (meterVariant == MeterVariant.THREE || meterVariant == MeterVariant.FOUR))
                    {
                        bLagOrLead = true;
                        foreach (DataColumn col in table.Columns)
                        {
                            if (col.ColumnName.ToUpper().Contains("LAG") || col.ColumnName.ToUpper().Contains("LEAD"))
                                bLagOrLead = false;
                        }
                    }
                }
                if (bLagOrLead)
                {
                    switch (type)
                    {
                        case "midnight":
                            kwhColumn= "Cumulative Energy - kWh Import(1.0.1.8.0.255;3;2)";//pks
                            kvahColumn="Cumulative Energy - kVAh Import(1.0.9.8.0.255;3;2)";//pks
                            kvarhColumn="Cumulative Energy - kVArh Import (lag+lead)";
                            netColumn="Net - kWh";
                            break;
                        case "billing":
                            kwhColumn="kWh Import(1.0.1.8.0.255;3;2)";//pks
                            kvahColumn="kVAh Import(1.0.9.8.0.255;3;2)";//pks
                            kvarhColumn="kVArh Import (lag+lead)";
                            netColumn = "Net - kWh";
                            break;
                        default:
                            break;
                    }
                    double cumKWh = 0.0d;
                    double cumKVAH = 0.0d;
                    double cumKVArh = 0.0d;
                    int ordinal = table.Columns[netColumn].Ordinal;
                    table.Columns.Add(kvarhColumn, typeof(String)).SetOrdinal(ordinal);
                    foreach (DataRow row in table.Rows)
                    {
                        if (Double.TryParse(CommonBLL.RemoveUnitIfExist(row[kwhColumn].ToString()), out cumKWh) && Double.TryParse(CommonBLL.RemoveUnitIfExist(row[kvahColumn].ToString()), out cumKVAH))
                        {
                            cumKVArh = Math.Sqrt(Math.Pow(cumKVAH, 2) - Math.Pow(cumKWh, 2));
                            row[kvarhColumn] = String.Format("{0:0.00}", cumKVArh);
                        }
                        else
                            row[kvarhColumn] = "----";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GetReactive()", ex);
            }
        }
        //SarkarA code change 20180424 end
    }

}
  