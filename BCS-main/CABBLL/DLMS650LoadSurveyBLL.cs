
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System.Collections.Generic;
using CAB.Framework.Utility;
using System;
using Utilities;

namespace CAB.BLL
{
    public class DLMS650LoadSurveyBLL : IBLL
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
        private DLMS650LoadSurveyDAL loadSurveyDAL;
        Dictionary<string, string> loadSurveyColumns = new Dictionary<string, string>();
        private DLMS650CommonBLL common;
        private LoadSurveyParameterBLL loadSurveyParameterBLL;
        private const string RealTimeClockColumn = "realTimeClockDateandTime";
        private const string Date = "Date (0.0.1.0.0.255;8;2)";
        bool integrationPeriodStatus = false;
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        /* GKG 30/01/2012 TFS ID 134283 */
        bool isTNEB = false;
        /* GKG 30/01/2012 TFS ID 134283 */
        public bool IntegrationPeriodStatus
        {
            get { return integrationPeriodStatus; }
            set { integrationPeriodStatus = value; }
        }
        public DLMS650LoadSurveyBLL()
        {
            //Utility check added; 24th April 2012; Bug 75902   
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
                /* GKG 30/01/2012 TFS ID 134283 */
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.TNEB.ToString())
                {
                    isTNEB = true;
                }
                /* GKG 30/01/2012 TFS ID 134283 */
            }
            else if (UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else
            {
                /* GKG 30/01/2012 TFS ID 134283 */
                isTNEB = false;
                /* GKG 30/01/2012 TFS ID 134283 */
                isPUMA = false;
                isMVVNL = false;
            }
            /* GKG 30/01/2012 TFS ID 134283 */
            //loadSurveyDAL = new DLMS650LoadSurveyDAL(isPUMA);
            loadSurveyDAL = new DLMS650LoadSurveyDAL(isPUMA,isTNEB);
            /* GKG 30/01/2012 TFS ID 134283 */
            loadSurveyParameterBLL = new LoadSurveyParameterBLL();
            common = new DLMS650CommonBLL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return loadSurveyDAL.InsertData(entity);
        }
        public IEntity InsertData(IList<IEntity> entities)
        {
            return loadSurveyDAL.InsertData(entities);
        }
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BatchInsert(IList<IEntity> entities)
        {
             loadSurveyDAL.BatchInsert(entities);
        }
        public long GetFromDate(long meterDataID)
        {
            return loadSurveyDAL.GetFromDate(meterDataID);
        }
        public long GetToDate(long meterDataID)
        {
            return loadSurveyDAL.GetToDate(meterDataID);
        }
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        {
            return loadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate,false);// Story - 427028 - Load survey data sequence should be in descending order except graph
        }
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate, string type,bool isForGraph)
        {
            // Story - 427028 - Load survey data sequence should be in descending order except graph
            DataSet dSet = FillNullColumnsWithZero(loadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate, isForGraph)); 
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
        /// <summary>
        /// Gets the last load survey date time stored in db for a meter
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public DateTime GetLastLoadSurveyDataInDbForMeter(string meterID)
        {
           return loadSurveyDAL.GetLastLoadSurveyDataInDbForMeter(meterID);
        }
        public DataSet ListDataSetColumnWise(long meterDataId, long fromDate, long toDate, string type, bool isPadding)
        {
            DataSet dSet = loadSurveyParameterBLL.GetColumnNames(meterDataId);
            DataSet ds = null;
            if (dSet != null && dSet.Tables[0].Rows.Count != 0)
            {
                ds = common.ConvertLoadSurvey(loadSurveyDAL.ListDataSetWithColumns(meterDataId, fromDate, toDate, dSet.Tables[0].Rows[0][0].ToString()), type, meterDataId, isPadding);
                if (ds == null)
                {
                    // Added to solve bug 72902.
                    if (common.IntegrationPeriodStatus)
                    {
                        IntegrationPeriodStatus = true;
                    }
                    else
                    {
                        IntegrationPeriodStatus = false;
                    }
                }
                MDInterval = common.MDInterval;
            }
            else
            {
                ds = common.ConvertLoadSurvey(loadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate,false), type, meterDataId, isPadding); // Story - 427028 - Load survey data sequence should be in descending order except graph
                MDInterval = common.MDInterval;
            }
            return ds;
        }
        public int GetLoadSurveyInterval(long meterDataId, long fromDate, long toDate)
        {
            int minute = 0;
            DataSet dataSet = loadSurveyDAL.ListDataSet(meterDataId, fromDate, toDate,true); // Story - 427028 - Load survey data sequence should be in descending order except graph   
            TimeSpan ts;
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 1)
            {
                ts = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[0][0].ToString()));
                minute = (int)ts.TotalMinutes;                               
            }            
            return minute;
        }
        /// <summary>
        /// Get Interval period for CAB Files .
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public int GetCABLoadSurveyInterval(long meterDataId)
        {
           return loadSurveyDAL.GetIntervalPeriod(meterDataId);
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

        public DataSet GetLoadSurveyDisplayGrid(DataSet dsDisplayGrid)
        {
            DataSet dsResult = null;

            return dsResult;
        }


        public Dictionary<string, string> CreateLoadSurveyDictionary(string selectedMeterId)
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
            loadSurveyColumns.Add("Real Time Clock", "realTimeClockDateandTime");
            loadSurveyColumns.Add("Current - R Phase", "rPhaseCurrent");
            loadSurveyColumns.Add("Current - Y Phase", "yPhaseCurrent");
            loadSurveyColumns.Add("Current - B Phase", "bPhaseCurrent");
            loadSurveyColumns.Add("Voltage - R Phase", "rPhaseVoltage");
            loadSurveyColumns.Add("Voltage - Y Phase", "yPhaseVoltage");
            loadSurveyColumns.Add("Voltage - B Phase", "bPhaseVoltage");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh"), "blockEnergykWh");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh Import"), "blockEnergykWhImport");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh Export"), "blockEnergykWhExport");            
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}VAh"), "blockEnergykVAh");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}VAh Import"), "blockEnergykVAhImport");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}VAh Export"), "blockEnergykVAhExport");

            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lag)"), "blockEnergykvarhlag");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lead)"), "blockEnergykvarhlead");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lag) Q1"), "blockEnergykvarhlagQ1");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lead) Q2"), "blockEnergykvarhleadQ2");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lag) Q3"), "blockEnergykvarhlagQ3");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(lead) Q4"), "blockEnergykvarhleadQ4");

            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh R Phase"), "blockEnergykWhRPhase");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh Y Phase"), "blockEnergykWhYPhase");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}Wh B Phase"), "blockEnergykWhBPhase");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(Q12)"), "blockEnergykvarhQ12");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(Q34)"), "blockEnergykvarhQ34");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(Q14)"), "blockEnergykvarhQ14");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy - {0}varh(Q23)"), "blockEnergykvarhQ23");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Block Energy Fundamental - {0}Wh"), "blockEnergyFundamentalkWhAbsolute");
            

            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Net - {0}Wh"), "netkWh");
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Net - {0}VAh"), "netkVAh");
            
            loadSurveyColumns.Add("Active Power R Phase  - {0}W", "activePowerRPhase");
            loadSurveyColumns.Add("Active Power Y Phase  - {0}W", "activePowerYPhase");
            loadSurveyColumns.Add("Active Power B Phase  - {0}W", "activePowerBPhase");
            loadSurveyColumns.Add("Apparent Power R Phase - {0}VA", "apparentPowerRPhase");
            loadSurveyColumns.Add("Apparent Power Y Phase - {0}VA", "apparentPowerYPhase");
            loadSurveyColumns.Add("Apparent Power B Phase - {0}VA", "apparentPowerBPhase");
            loadSurveyColumns.Add("Reactive Power R Phase - {0}VAr", "reactivePowerRPhase");
            loadSurveyColumns.Add("Reactive Power Y Phase - {0}VAr", "reactivePowerYPhase");
            loadSurveyColumns.Add("Reactive Power B Phase - {0}VAr", "reactivePowerBPhase");
            loadSurveyColumns.Add("Total Power Off duration during LSIP - min", "powerOffDurationLSIP");

            //Task no: 591495- Temperature added in load survey for 1P DLMS Firmware X39.51L (DHBVN QD-755)*/
            loadSurveyColumns.Add(CommonMethods.getDisplayHeaderText("Temperature °C"), "temperature");

            //These two new parameters added for PUMA Load Survey Report; 24th April 2012; Bug 75902   
            if (isPUMA)
            {
                loadSurveyColumns.Add("Frequency - Hz", "Frequency");
                loadSurveyColumns.Add("Tamper Status", "TamperStatus");
            }
            /* VBM - Make Avg power factor configurable in LS */
            //if (UtilityDetails.ShowAvgPowerFactorInLoadSurvey)
            //{
                loadSurveyColumns.Add("Power Factor", "Power Factor");
            //}
            /* VBM - Make Avg power factor configurable in LS */
            return loadSurveyColumns;
        }
        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateLoadSurveyDictionary(string.Empty);
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (loadSurveyColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
        public DataSet GetLoadSurveyData(string value, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = common.ConvertLoadSurvey(loadSurveyDAL.GetLoadSurveyData(value, columns));
            return ds;
        }

        public DataSet GetLoadSurveyDataByFileName(string value, string fileName, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            ds = common.ConvertLoadSurvey(loadSurveyDAL.GetLoadSurveyDataByFileName(value, fileName, columns));
            return ds;
        }


        public bool DeleteData(long meterDataID)
        {
            return loadSurveyDAL.DeleteData(meterDataID);
        }

        //added PUMA Daily Consumption
        public DataSet GetDailyConsumption(string fileName,long meterDataId, long fromDate, long toDate)
        {
            DataSet dataSet = new DataSet();
            return loadSurveyDAL.GetDailyConsumption(fileName,meterDataId, fromDate, toDate);
        }

        //added PUMA MidNightData
        public DataSet GetMidNightData(string fileName,long meterDataId, long fromDate, long toDate)
        {
            DataSet dataSet = new DataSet();
            int resolution = 0;
            // Added to solve midnight data difference in fast download and direct read out issue
            if (UtilityDetails.ShowMidnight)
            {
                dataSet = loadSurveyDAL.GetMidNightData(fileName, meterDataId, fromDate, toDate, out resolution);
                dataSet = new DLMS650CommonBLL().ConvertMidnightData(meterDataId, dataSet, resolution);
            }
            
            // Added meterDataId to solve bug 73406.
            return dataSet;
        }

        public DataSet GetMidNightDataForConsumptionMeterIDWise(string meterID, long fromDate, long toDate)
        {
            DataSet dataSet = new DataSet();
            dataSet = loadSurveyDAL.GetMidNightDataForConsumptionMeterIDWise(meterID, fromDate,toDate);
            //Remove duplicate rows
            dataSet  =  common.RemoveDuplicateRows(dataSet, Date);
            return dataSet; 
        }

        public DataSet GetPUMADailyConsumption(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            if (isPUMA)
            {
                int meterModelNumber = loadSurveyDAL.GetMeterModelNoByMeterDataID(Convert.ToString(meterDataId));
                //if (meterModelNumber == 1)
                //{
                    dataSet = loadSurveyDAL.GetPUMAMidNightDataForConsumption(meterDataId);
                //}
                //else
                //{
                //    dataSet = loadSurveyDAL.GetPUMAMidNightData(meterDataId);
                //}
                dataSet = new DLMS650CommonBLL().ConvertPUMADailyConsumption(meterDataId, dataSet);
            }
            return dataSet;
        }
        public DataSet GetPUMAMidNightData(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            if (isPUMA)
            {
                dataSet = loadSurveyDAL.GetPUMAMidNightData(meterDataId);
                dataSet = new DLMS650CommonBLL().ConvertPUMAMidnightData(meterDataId, dataSet);
            }
            // Added meterDataId to solve bug 73406.
            return dataSet;
        }

        /// <summary>
        /// Get Max Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMinDateForMeterID(string meterID)
        {
            return loadSurveyDAL.GetMinDateForMeterID(meterID);
        }

        /// <summary>
        /// Get Max Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMaxDateForMeterID(string meterID)
        {
            return loadSurveyDAL.GetMaxDateForMeterID(meterID);
        }
        /// <summary>
        /// Lists DataSet Column Wise of loadsurvey For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet ListDataSetColumnWiseForMeterID(string meterID, long fromDate, long toDate, string type, bool isPadding)
        {
            DataSet dSet = loadSurveyParameterBLL.GetColumnNamesForMeterID(meterID);
          
           DataSet ds = null;
            if (dSet != null && dSet.Tables[0].Rows.Count != 0)
            {

                DataSet lpData =  loadSurveyDAL.ListDataSetWithColumnsForMeterID(meterID, fromDate, toDate, dSet.Tables[0].Rows[0][0].ToString());
                //Remove duplicate rows if any.
                ds = common.ConvertLoadSurveyForMeterID(common.RemoveDuplicateRows(lpData, RealTimeClockColumn), type, meterID, isPadding);
                if (ds == null)
                {
                    // Added to solve bug 72902.
                    if (common.IntegrationPeriodStatus)
                    {
                        IntegrationPeriodStatus = true;
                    }
                    else
                    {
                        IntegrationPeriodStatus = false;
                    }
                }
                MDInterval = common.MDInterval;
            }
            else
            {
                ds = common.ConvertLoadSurveyForMeterID(loadSurveyDAL.ListDataSetForMeterID(meterID, fromDate, toDate), type, meterID, isPadding);
                MDInterval = common.MDInterval;
            }
            return ds;
        }

        /// <summary>
        /// gets meeter type from meter serial number
        /// </summary>
        /// <param name="meterId">meter serial number</param>
        /// <returns>meter type</returns>
        public string GetActiveMeterTypeByMeterId(string meterId)
        {
            return loadSurveyDAL.GetActiveMeterTypeByMeterId(meterId);
        }

    }
}
