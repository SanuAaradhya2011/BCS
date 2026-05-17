using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System.Collections.Generic;
using CAB.Framework.Utility;
using System;
using Utilities;
using CAB.Entity;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class DLMS650MidnightDataBLL : IBLL
    {
        private int mdInterval;
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
        private DLMS650MidnightDataDAL midnightDataDAL = null;
        Dictionary<string, string> midnightEnergyColumns = new Dictionary<string, string>();
        Dictionary<string, string> columnOBISCodes = new Dictionary<string, string>();
        private DLMS650LoadSurveyDAL loadSurveyDAL = null;
        private MidnightParameterBLL midnightParameterBLL = null;
        private DLMS650CommonBLL common;
        private const string RealTimeClockColumn = "realTimeClockDateandTime";
        bool integrationPeriodStatus = false;
        string utility = string.Empty;
        bool isPUMA = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650MidnightDataBLL).ToString());
        public bool IntegrationPeriodStatus
        {
            get { return integrationPeriodStatus; }
            set { integrationPeriodStatus = value; }
        }
        public DLMS650MidnightDataBLL()
        {
            //Utility check added; 24th April 2012; Bug 75902   
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            midnightDataDAL = new DLMS650MidnightDataDAL();
            loadSurveyDAL = new DLMS650LoadSurveyDAL();
            midnightParameterBLL = new MidnightParameterBLL();
            common = new DLMS650CommonBLL();
        }

        //public IEntity InsertData(IEntity entity)
        //{
        //    return midnightDataDAL.InsertData(entity);
        //}
        public IEntity InsertData(IList<IEntity> entities)
        {
            return midnightDataDAL.InsertData(entities);
        }
        public long GetFromDate(long meterDataID)
        {
            return midnightDataDAL.GetFromDate(meterDataID);
        }
        public long GetToDate(long meterDataID)
        {
            return midnightDataDAL.GetToDate(meterDataID);
        }

        public DataSet FillNullColumnsWithZero(DataSet dataSet)
        {
            DataRow dr = null;
            DataSet dSet = new DataSet();
            DataTable dTable = new DataTable();


            dTable = dataSet.Tables[0].Clone();
            foreach (DataRow drow in dataSet.Tables[0].Rows)
            {
                dr = dTable.NewRow();
                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                {
                    if (drow[colCount].ToString() == "")
                    {

                        dr[colCount] = "0";//drow[colCount];
                    }
                    else
                    {
                        dr[colCount] = drow[colCount];
                    }
                }
                dTable.Rows.Add(dr);
            }
            dSet.Tables.Add(dTable);
            return dSet;
        }

        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate, string type)
        {
            DataSet dSet = FillNullColumnsWithZero(midnightDataDAL.ListDataSet(meterDataId, fromDate, toDate));
            //DataSet ds= common.ConvertLoadSurvey(loadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate), type,meterDataId);
            DataSet ds = common.ConvertLoadSurveyForGraph(dSet, type, meterDataId);
            MDInterval = common.MDInterval;
            // Added to solve bug 72902 for Graphical view 16th April 2012.
            if (common.IntegrationPeriodStatus)
            {
                IntegrationPeriodStatus = true;
            }
            else
            {
                IntegrationPeriodStatus = false;
            }
            return ds;
        }

        public DataSet ListDataSetColumnWise(long meterDataId, long fromDate, long toDate, string type)
        {
            DataSet dSet = midnightParameterBLL.GetColumnNames(meterDataId);
            DataSet ds = null;
            if (dSet != null && dSet.Tables[0].Rows.Count != 0)
            {
                ds = common.ConvertMidnightEnergyDataWiseReport(midnightDataDAL.ListDataSetWithColumns(meterDataId, fromDate, toDate, dSet.Tables[0].Rows[0][0].ToString()));

            }
            //    if (ds == null)
            //    {
            //        // Added to solve bug 72902.
            //        if (common.IntegrationPeriodStatus)
            //        {
            //            IntegrationPeriodStatus = true;
            //        }
            //        else
            //        {
            //            IntegrationPeriodStatus = false;
            //        }
            //    }
            //    MDInterval = common.MDInterval;
            //}
            //else
            //{
            //    ds = common.ConvertLoadSurvey(midnightDataDAL.ListDataSet(meterDataId, fromDate, toDate), type, meterDataId);
            //    MDInterval = common.MDInterval;
            //}
            return ds;
        }
        //public int GetLoadSurveyInterval(long meterDataId, long fromDate, long toDate, string type)
        //{
        //    DataSet dataSet = midnightDataDAL.ListDataSet(meterDataId, fromDate, toDate);
        //    if (dataSet == null)
        //        return 0;
        //    if (dataSet.Tables.Count <= 0)
        //        return 0;
        //    if (dataSet.Tables[0].Rows.Count <= 0)
        //        return 0; 
        //    TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[0][0].ToString()));
        //    return (int)ts.TotalMinutes;
        //}

        //public DataSet FillNullColumnsWithZero(DataSet dataSet)
        //{
        //    DataRow dr = null;
        //    DataSet dSet = new DataSet();
        //    DataTable dTable = new DataTable();
        //    dTable = dataSet.Tables[0].Clone();
        //    foreach (DataRow drow in dataSet.Tables[0].Rows)
        //    {
        //        dr = dTable.NewRow();
        //        for (int colCount = 0;colCount< dataSet.Tables[0].Columns.Count;colCount++)
        //        {
        //            if (drow[colCount].ToString() == "")
        //            {
        //                dr[colCount] = "0";//drow[colCount];
        //            }
        //            else
        //            {
        //                dr[colCount] = drow[colCount];
        //            }
        //        }
        //        dTable.Rows.Add(dr);
        //    }
        //    dSet.Tables.Add(dTable);
        //    return dSet;
        //}

        public Dictionary<string, string> CreateMidnightDataDictionary(string selectedMeterId)
        {
            //For Reports: Selected Meter can be mupltiple. i.e. selected meter id will be passed.
            //For Analysis Report Selected id will be empty. then Selected Active Meter Id will be selected Meterid
            if (!string.IsNullOrEmpty(selectedMeterId))
            {
                //Get Meter Data Type (HTCT/LTCT) and assign it to global methods
                CommonMethods.MeterDataType = new DLMS650GeneralBLL().GetMeterDataType(selectedMeterId);
            }
            else
            { //Get Meter Data Type (HTCT/LTCT) and assign it to global methods
                CommonMethods.MeterDataType = new DLMS650GeneralBLL().GetMeterDataType(ConfigInfo.ActiveMeterDataId);
            }

            //midnightEnergyColumns.Add("Date", "realTimeClockDateandTime");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh"), "cumEnergykWh");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lag"), "cumEnergykvarhlag");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lead"), "cumEnergykvarhlead");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh"), "cumEnergykVAh");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import"), "cumEnergykWhImport");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lag Q1"), "cumEnergykvarhlagQ1");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lead Q4"), "cumEnergykvarhleadQ4");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Import"), "cumEnergykVAhImport");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Export"), "cumEnergykWhExport");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lag Q3"), "cumEnergykvarhlagQ3");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh - lead Q2"), "cumEnergykvarhleadQ2");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh Import"), "cumEnergykVAhExport");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh R Phase"), "cumEnergykWhRPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Y Phase"), "cumEnergykWhYPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh B Phase"), "cumEnergykWhBPhase");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q12)"), "cumEnergykvarhQ12");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q34)"), "cumEnergykvarhQ34");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q14)"), "cumEnergykvarhQ14");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}varh (Q23)"), "cumEnergykvarhQ23");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Fundamental {0}Wh Absolute"), "fundamentalAbsolutekWH");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Net - {0}Wh"), "netkWh");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Net - {0}VAh"), "netkVAh");

            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Minimum Voltage LSIP across day-R Phase - {0}V"), "minVoltageLSIPAcrossDayRPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Minimum Voltage LSIP across day-Y Phase - {0}V"), "minVoltageLSIPAcrossDayYPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Minimum Voltage LSIP across day-B Phase - {0}V)"), "minVoltageLSIPAcrossDayBPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Highest Current LSIP across day-R Phase - {0}A"), "highestCurrentLSIPAcrossDayRPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Highest Current LSIP across day-Y Phase"), "highestCurrentLSIPAcrossDayYPhase");
            midnightEnergyColumns.Add(CommonMethods.getDisplayHeaderText("Highest Current LSIP across day-B Phase - {0}A"), "highestCurrentLSIPAcrossDayBPhase");
              
         return midnightEnergyColumns;
        }

        public DataSet ListDataSet(long meterDataId)
        {
            DTMDailyProfileParameterEntity midnightParameterEntity = new MidnightParameterBLL().GetColumn(meterDataId) as DTMDailyProfileParameterEntity;
            if (midnightParameterEntity == null)
                return null;
            return CommonBLL.ConvertDTMDailyProfileRowToColumn(midnightDataDAL.ListDataSet(meterDataId, midnightParameterEntity.ColumnsNames, GetFromDate(meterDataId), GetToDate(meterDataId)));
        }

        public List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateMidnightDataDictionary(string.Empty);
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (midnightEnergyColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
        public DataSet GetMidnightEnergies(string value, List<string> columnList)
        {
            DataSet ds = new DataSet();
            DLMS650CommonBLL dlms650CommonBLL = new DLMS650CommonBLL();
            List<string> columns = GetDatabaseColumns(columnList);
            //Added to solve bug 94907
            ds = dlms650CommonBLL.ConvertMidnightEnergy(midnightDataDAL.GetMidnightEnergy(value, columns));
            return ds;
        }

        public DataSet GetMidnightEnergiesByFileName(string value, string fileName, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = common.ConvertMidnightEnergy(midnightDataDAL.GetMidnightEnergiesByFileName(value, fileName, columns));
            return ds;
        }

        public DataSet GetMidnightEnergiesByDateWiseReport(string value, string fileName, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = common.ConvertMidnightEnergyDataWiseReport(midnightDataDAL.GetMidnightEnergiesByFileName(value, fileName, columns));
            return ds;
        }


        public void DeleteData(long meterDataID)
        {
            midnightDataDAL.DeleteData(meterDataID);
        }

        ////added PUMA Daily Consumption
        //public DataSet GetDailyConsumption(long meterDataId, long fromDate, long toDate)
        //{
        //    DataSet dataSet = new DataSet();
        //    return midnightDataDAL.GetDailyConsumption(meterDataId, fromDate, toDate);
        //}


        public DataSet GetMidNightData(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            if (!isPUMA)
            {
                dataSet = midnightDataDAL.GetMidNightData(meterDataId);
                dataSet = new DLMS650CommonBLL().ConvertMidnightEnergy(dataSet);
            }
            else
            {

                dataSet = loadSurveyDAL.GetPUMAMidNightData(meterDataId);
                dataSet = new DLMS650CommonBLL().ConvertPUMAMidnightData(meterDataId, dataSet);

            }
            return dataSet;
        }

        /// <summary>
        /// Get Max Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMinDateForMeterID(string meterID)
        {
            return midnightDataDAL.GetMinDateForMeterID(meterID);
        }

        /// <summary>
        /// Get Max Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMaxDateForMeterID(string meterID)
        {
            return midnightDataDAL.GetMaxDateForMeterID(meterID);
        }

        /// <summary>
        /// Lists DataSet Column Wise of midnight For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet ListDataSetColumnWiseForMeterID(string meterID, long fromDate, long toDate, string type)
        {
            DataSet dSet = midnightParameterBLL.GetColumnNamesForMeterID(meterID);

            DataSet ds = null;
            if (dSet != null && dSet.Tables[0].Rows.Count != 0)
            {
                DataSet midData = midnightDataDAL.ListDataSetWithColumnsForMeterID(meterID, fromDate, toDate, dSet.Tables[0].Rows[0][0].ToString());
                ds = common.ConvertMidnightEnergyDataWiseReport(common.RemoveDuplicateRows(midData, RealTimeClockColumn));
            }

            return ds;
        }

        /// <summary>
        /// getting  midnigt data by sending meter ID
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetGenericMidNightData(long meterDataId, string sortType)
        {
            DataSet dataSet = null;            
            DataSet columnDataSet = midnightParameterBLL.GetColumnNames(meterDataId);
            if (columnDataSet.Tables[0].Rows.Count > 0)
            {
                dataSet = midnightDataDAL.GetGenericMidNightData(meterDataId, columnDataSet.Tables[0].Rows[0][0].ToString(), sortType);
                dataSet = new DLMS650CommonBLL().ConvertPUMAMidnightData(meterDataId, dataSet);

                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    // OBIS Code changed for APSPDCL : Daily Survey Requirement
                    // Name change for APSPDCL : Daily Survey Requirement
                    if (ConfigSettings.GetValue("ChkPowerOnOffDurationFormat") == "1")
                    {
                        if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh"))
                            dataSet.Tables[0].Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromSeconds(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh"])));
                        if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"))
                            dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh"])));

                        if (dataSet.Tables[0].Columns.Contains("Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh"))
                            dataSet.Tables[0].Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh"])));

                        if (dataSet.Tables[0].Columns.Contains("Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"))//dd
                            dataSet.Tables[0].Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh"] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh"])));
                    }
                    else
                    {

                        if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"))//dd
                            dataSet.Tables[0].Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromSeconds(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm"])));//dd
                        if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"))//dd
                            dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm"])));//dd

                        if (dataSet.Tables[0].Columns.Contains("Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"))//dd
                            dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm"])));//dd

                        if (dataSet.Tables[0].Columns.Contains("Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"))//dd
                            dataSet.Tables[0].Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm"])));//dd

                        if (dataSet.Tables[0].Columns.Contains("Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"))//dd:hh:mm
                            dataSet.Tables[0].Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(dataSet.Tables[0].Rows[i]["Power On Duration 3 Phases (1.0.96.0.164.255;3;2) dddd:hh:mm"])));//dd:mm:hh
                    }
                } 
            }

            
            if (dataSet != null)       //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                common.GetReactive(dataSet.Tables[0], "midnight");

            return dataSet;
        }

        /// <summary>
        /// getting  midnigt Consumption data by sending meter ID
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetGenericMidNightConsumptionData(long meterDataId, string sortType)
        {
            DataSet dataSet = null;
            DataSet columnDataSet = midnightParameterBLL.GetColumnNames(meterDataId);
            if (columnDataSet.Tables[0].Rows.Count > 0)
            {
                dataSet = midnightDataDAL.GetGenericMidNightData(meterDataId, columnDataSet.Tables[0].Rows[0][0].ToString(), sortType);
                dataSet = new DLMS650CommonBLL().ConvertGenericDailyConsumption(meterDataId, dataSet);
            }

            
            if (dataSet != null)       //SarkarA code change 20180424 //add Kvarh runtime calc for billing, midnight 1Ph Net Reliance 
                common.GetReactive(dataSet.Tables[0], "midnight");

            return dataSet;
        }
    }
}
