
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 05/10/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using CAB.Framework.Utility;
using MySql.Data.MySqlClient;
using Hunt.EPIC.Logging;
namespace CAB.DALC.Data
{
    public class DLMS650LoadSurveyDAL : DALBase
    {
        private string LoadSurvey_ID = "LoadSurvey_ID";
        private string realTimeClockDateandTime = "realTimeClockDateandTime";
        private string rPhaseCurrent = "rPhaseCurrent";
        private string yPhaseCurrent = "yPhaseCurrent";
        private string bPhaseCurrent = "bPhaseCurrent";
        private string averageCurrent = "averageCurrent";
        private string rPhaseVoltage = "rPhaseVoltage";
        private string yPhaseVoltage = "yPhaseVoltage";
        private string bPhaseVoltage = "bPhaseVoltage";
        private string averageVoltage = "averageVoltage";
        private string blockEnergykWh = "blockEnergykWh";
        private string blockEnergykvarhlag = "blockEnergykvarhlag";
        private string blockEnergykvarhlead = "blockEnergykvarhlead";
        private string blockEnergykVAh = "blockEnergykVAh";

        private string blockEnergykWhExport = "blockEnergykWhExport";
        private string blockEnergykVAhExport = "blockEnergykVAhExport";
        private string blockEnergykvarhlagQ3 = "blockEnergykvarhlagQ3";
        private string blockEnergykvarhleadQ2 = "blockEnergykvarhleadQ2";
        private string blockEnergykWhImport = "blockEnergykWhImport";
        private string blockEnergykVAhImport = "blockEnergykVAhImport";
        private string blockEnergykvarhlagQ1 = "blockEnergykvarhlagQ1";
        private string blockEnergykvarhleadQ4 = "blockEnergykvarhleadQ4";

        private string blockEnergykWhRPhase = "blockEnergykWhRPhase";
        private string blockEnergykWhYPhase = "blockEnergykWhYPhase";
        private string blockEnergykWhBPhase = "blockEnergykWhBPhase";
        private string blockEnergykvarhQ12 = "blockEnergykvarhQ12";
        private string blockEnergykvarhQ34 = "blockEnergykvarhQ34";
        private string blockEnergykvarhQ14 = "blockEnergykvarhQ14";
        private string blockEnergykvarhQ23 = "blockEnergykvarhQ23";
        private string blockEnergyFundamentalkWhAbsolute = "blockEnergyFundamentalkWhAbsolute";

        private string temperature = "temperature";
        private string neutralcurrent = "neutralcurrent";//add pradipta_load_neu
        private string avgphasecurrent = "avgphasecurrent";

        private string activePowerRPhase = "activePowerRPhase";
        private string activePowerYPhase = "activePowerYPhase";
        private string activePowerBPhase = "activePowerBPhase";
        private string apparentPowerRPhase = "apparentPowerRPhase";
        private string apparentPowerYPhase = "apparentPowerYPhase";
        private string apparentPowerBPhase = "apparentPowerBPhase";
        private string reactivePowerRPhase = "reactivePowerRPhase";
        private string reactivePowerYPhase = "reactivePowerYPhase";
        private string reactivePowerBPhase = "reactivePowerBPhase";
        private string powerOffDurationLSIP = "powerOffDurationLSIP";

        private string netkWh = "netkWh";
        private string netkVAh = "netkVAh";

        private string MeterData_ID = "MeterData_ID";
        private string MeterID = "MeterID";
        private string FileName = "FileName";
        private string MDIntervalPeriod = "MDIntervalPeriod";

        private string neutralCurrent = "NeutralCurrent";


        private string IsDLMS = "IsDLMS";
        //added PUMA
        private string Frequency = "frequency";
        private string Tamper = "tamperStatus";
        private string TamperFlag = "tamperflag";
        private string AvgVolta3ph = "Avgvoltageof3phase";
        private string AvgRPHPF = "AvgRphasePF";
        private string AvgYPHPF = "AvgYphasePF";
        private string AvgBPHPF = "AvgBphasePF";
        private string AvgTotalPF = "AvgTotalPF";
        private string AvgNeutralCurrent = "AvgNeutralCurrent";
        private string ThdVr = "ThdVr";
        private string ThdVy = "ThdVy";
        private string ThdVb = "ThdVb";
        private string ThdIr = "ThdIr";
        private string ThdIy = "ThdIy";
        private string ThdIb = "ThdIb";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoadSurveyDAL).ToString());

        bool isPUMA = false;
        /* GKG 30/01/2012 TFS ID 134283 */
        bool isTNEB = false;
        /* GKG 30/01/2012 TFS ID 134283 */
        public DLMS650LoadSurveyDAL()
            : base("meterdata_loadsurvey", "LoadSurvey_ID")
        {


        }
        /* GKG 30/01/2012 TFS ID 134283 */
        //public DLMS650LoadSurveyDAL(bool IsPUMA)
        //    : base("meterdata_loadsurvey", "LoadSurvey_ID")
        //{
        //    isPUMA = IsPUMA;
        //}
        public DLMS650LoadSurveyDAL(bool IsPUMA, bool IsTNEB)
            : base("meterdata_loadsurvey", "LoadSurvey_ID")
        {
            isPUMA = IsPUMA;
            isTNEB = IsTNEB;

        }
        /* GKG 30/01/2012 TFS ID 134283 */
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate, bool isForGraph)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,averageCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,averageVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,blockEnergykWhExport,blockEnergykvarhlagQ3,blockEnergykvarhleadQ2,blockEnergykVAhExport,MDIntervalPeriod ,NeutralCurrent,Frequency,AvgNeutralCurrent,AvgPhaseCurrent "); // SB Code Change Start/End - Export Energy Display in Load Survey Graph - Added "Energy (Export)" Columns pradipta_load_neu
                builder.Append(" from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                /* GKG 30/01/2012 TFS ID 134283 */
                // builder.Append(string.Concat(" order by LoadSurvey_ID"));
				// Story - 427028 - Load survey data should be in ascending order
                //if (isPUMA && isTNEB)
                //{
                //    builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                //}
                //// Story - 349654 - In case of single phase non DLMS meter, data is coming in reverse order
                //else if (ConfigInfo.ActiveFileType=="NONDLMS" && ConfigInfo.ActiveMeterType=="1P-2W") 
                //{
                if (isForGraph) // Story - 427028 - Load survey data sequence should be in descending order except graph
                    builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                else
                    builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                //}
                //else
                //{
                //    builder.Append(string.Concat(" order by LoadSurvey_ID"));
                //}
                /* GKG 30/01/2012 TFS ID 134283 */
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ListDataSet(long meterDataId, long fromDate, long toDate, bool isForGraph)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        //added PUMA Daily Consumption
        public DataSet GetDailyConsumption(string fileName, long meterDataId, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            bool result = false;
            int resolution = 0;
            // Added to solve dailu consumption data difference in fast download and direct read out issue 
            resolution = GetScale(meterDataId);

            try
            {
                //Added convert function.Solved bug 73406.
                // added substring the energy value as per the resolution of energy in instant and then calculating sum.
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date', ");
                // To solve issue in daily consumption if resolution is zero 

                //To solve bug 94243.
                builder.Append(string.Concat("sum(Convert(blockenergykwh,decimal(10," + resolution + "))) as 'Daily Consumption - kWh(1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykVAh,decimal(10," + resolution + "))) as 'Daily Consumption - KVAh(1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykvarhlag,decimal(10," + resolution + "))) as 'Daily Consumption - kvarh Lag(1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykvarhlead,decimal(10," + resolution + "))) as 'Daily Consumption - kvarh Lead(1.0.8.8.0.255;3;2)' "));

                builder.Append(string.Concat("from meterdata_loadsurvey where MeterData_ID = " + meterDataId + " and "));
                builder.Append(string.Concat("realTimeClockDateandTime >= '" + fromDate + "' and realTimeClockDateandTime <= '" + toDate));
                builder.Append(string.Concat("' group by convert(realTimeClockDateandTime,char(8))"));

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDailyConsumption(string fileName, long meterDataId, long fromDate, long toDate)", ex);
                throw;
            }

            return dataSet;
        }
        /// <summary>
        /// Get the last load survey date time stored in DB
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public DateTime GetLastLoadSurveyDataInDbForMeter(string meterID)
        {
            DateTime lastLoadSurveyDateTime = DateTime.MinValue;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            object lastLoadSurveyDate;
            try
            {
                builder.Append("Select ml.realTimeClockDateandTime from meterdata m join meterdata_loadsurvey ml on m.MeterData_ID = ml.MeterData_ID where m.MeterID = '");
                builder.Append(meterID + "' order by ml.realTimeClockDateandTime desc Limit 1");
                DataRequest request = new DataRequest(builder.ToString());
                lastLoadSurveyDate = helper.ExecuteScalar(request);
                if (lastLoadSurveyDate != null)
                {
                    lastLoadSurveyDateTime = DateUtility.LongToDateTime(Convert.ToInt64(lastLoadSurveyDate));
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("last load survey date fetched"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLastLoadSurveyDataInDbForMeter(string meterID)", ex);
                lastLoadSurveyDateTime = DateTime.MinValue;
            }
            return lastLoadSurveyDateTime;
        }
        // Added to solve known issue--Difference in the value of daily consumption data in fast downloading and normal downloading data.
        public int GetScaleForDailyConsumption(long id)
        {
            int decimalCounter = 0;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select InstantPowerColumnValue from meterdata_instantpower where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" And InstantPowerColumnName in ('Cumulative Energy - kWh')");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    // Added rows count condition.
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string tempVal = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                            if (!string.IsNullOrEmpty(tempVal))
                            {
                                if (tempVal.IndexOf('.') < 0)
                                { decimalCounter = 0; }
                                else
                                { decimalCounter = Convert.ToInt16(tempVal.IndexOf('*') - (tempVal.IndexOf('.') + 1)); }
                            }
                        }
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Resolution viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetScaleForDailyConsumption(long id)", ex);
                decimalCounter = 0;
            }
            return decimalCounter;
        }

        public int GetScale(long id)
        {
            int decimalCounter = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select blockEnergykWh from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    // Added rows count condition.
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            string tempVal = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                            if (tempVal.IndexOf('.') < 0)
                            { decimalCounter = 0; }
                            else
                            { decimalCounter = Convert.ToInt16(tempVal.IndexOf('*') - (tempVal.IndexOf('.') + 1)); }
                        }
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetScale(long id)", ex);
                decimalCounter = 0;
            }
            return decimalCounter;
        }
        public DataSet GetPUMAMidNightData(long meterDataID)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)', ");
                // To solve issue in midnight data if resolution is zero 

                builder.Append(string.Concat("cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)' "));
                builder.Append(string.Concat("from meterdata_midnightdata where MeterData_ID = " + meterDataID));
                //builder.Append(string.Concat("from meterdata_midnightdata where MeterData_ID = " + meterDataID + " and "));
                // builder.Append(string.Concat("realTimeClockDateandTime >= '" + fromDate + "' and realTimeClockDateandTime <= '" + toDate + "'"));
                //  builder.Append(string.Concat(" order by convert(realTimeClockDateandTime,char(8)) desc"));
                builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));// Story - 427028 - Load survey data sequence should be in descending order except graph
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPUMAMidNightData(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        ///VBM : Function for fetching meter model number by meterid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int GetMeterModelNoByMeterDataID(string meterDataID)
        {
            int meterModelNo = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(MeterModelNo,0) as MeterModelNo FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                meterModelNo = Convert.ToInt32(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterModelNoByMeterDataID(string meterDataID)", ex);
            }
            return meterModelNo;
        }

        /// <summary>
        ///VBM : Used to Get LOad survey interlav period for CAB files.
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int GetIntervalPeriod(long meterDataID)
        {
            int meterModelNo = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(MDIntervalPeriod,0) as MDIntervalPeriod FROM `dlms_ltct_650`.`meterdata_loadsurvey` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                meterModelNo = Convert.ToInt32(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetIntervalPeriod(long meterDataID)", ex);
            }
            return meterModelNo;
        }

        /// <summary>
        /// Midnight data MeterID Wise
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetMidNightDataForConsumptionMeterIDWise(string meterID, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            try
            {
                DLMS650GeneralDAL generalDAL = new DLMS650GeneralDAL();
                int meterModel = generalDAL.GetMeterModelNoByMeterID(meterID);
                builder.Append("Select distinct DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)', ");
                builder.Append(string.Concat("cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)', "));
                builder.Append(string.Concat("mDKW as 'Maximum Demand - kW (1.0.1.6.0.255;4;2)', "));
                builder.Append(string.Concat("mDKVA as 'Maximum Demand - kVA (1.0.9.6.0.255;4;2)', "));

                //Nidhi
                if (meterModel == NamePlateConstants.RubyE150Value)
                {
                    builder.Append(string.Concat("PowerOnDuration as 'Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm'")); // Removed because of OBIS conflict
                }
                else
                {
                    builder.Append(string.Concat("PowerOnDuration as 'Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm'"));
                }
               // OBIS code removed because two OBIS code are there for different meter
               

                //builder.Append(string.Concat("PowerOnDuration as 'Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm'"));
                builder.Append(string.Concat("from `meterdata_midnightdata` middata inner join `meterdata` mdata on middata.MeterData_ID = mdata.MeterData_ID and mdata."));
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " and "));
                builder.Append(string.Concat("middata.", realTimeClockDateandTime, ">= ", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat("middata.", realTimeClockDateandTime, "<= ", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidNightDataForConsumptionMeterIDWise(string meterID, long fromDate, long toDate)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// This function is temporary.Need to be corrected when we plan to write midnight queries again.
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetPUMAMidNightDataForConsumption(long meterDataID)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
           
            try
            {
                int meterModel = GetMeterModelNoByMeterDataID(meterDataID.ToString());
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)', ");
                builder.Append(string.Concat("cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)', "));
                builder.Append(string.Concat("mDKW as 'Maximum Demand - kW (1.0.1.6.0.255;4;2)', "));
                builder.Append(string.Concat("mDKVA as 'Maximum Demand - kVA (1.0.9.6.0.255;4;2)', "));
                //Nidhi
                if (meterModel == NamePlateConstants.RubyE150Value)
                {
                    builder.Append(string.Concat("PowerOnDuration as 'Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm'")); // Removed because of OBIS conflict
                }
                else
                {
                    builder.Append(string.Concat("PowerOnDuration as 'Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm'"));
                }
                builder.Append(string.Concat("from meterdata_midnightdata where MeterData_ID = " + meterDataID));
                builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPUMAMidNightDataForConsumption(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetSinglePhaseIECMidNightDataForConsumption(long meterDataID)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)', ");
                builder.Append(string.Concat("cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)', "));
                builder.Append(string.Concat("mDKW as 'Daily Power ON Duration (1.0.1.6.0.255;4;2)', "));
                builder.Append(string.Concat("mDKVA as 'Maximum Demand - kVA (1.0.9.6.0.255;4;2)', "));
                builder.Append(string.Concat("PowerOnDuration as 'PowerOnDuration'"));
                builder.Append(string.Concat("from meterdata_midnightdata where MeterData_ID = " + meterDataID));
                builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetSinglePhaseIECMidNightDataForConsumption(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        //added PUMA MidNightData
        public DataSet GetMidNightData(string fileName, long meterDataId, long fromDate, long toDate, out int resolution)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            bool result = false;
            resolution = 0;
            // Added to solve midnight data difference in fast download and direct read out issue    
            resolution = GetScale(meterDataId);
            result = true;
            try
            {

                // Changed Cu to Daily to solve DLMS_110.
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date', ");
                // To solve issue in midnight data if resolution is zero 

                builder.Append(string.Concat("sum(Convert(blockenergykwh,decimal(10," + resolution + "))) as 'Daily-kWh (1.0.1.29.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykVAh,decimal(10," + resolution + "))) as 'Daily-kVAh (1.0.9.29.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykvarhlag,decimal(10," + resolution + "))) as 'Daily-kvarh lag (1.0.5.29.0.255;3;2)', "));
                builder.Append(string.Concat("sum(Convert(blockenergykvarhlead,decimal(10," + resolution + "))) as 'Daily-kvarh lead (1.0.8.29.0.255;3;2)', "));

                // Added convert function to change varchar column values to decimal for bug 73008.
                builder.Append(string.Concat("max(Convert(rPhaseCurrent,decimal(10,3))) as 'Max-IRCurrent (1.0.31.27.0.255;3;2)',"));
                builder.Append(string.Concat("min(Convert(rPhaseCurrent,decimal(10,3))) as 'Min-IRCurrent (1.0.31.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(rPhaseVoltage,decimal(10,3))) as 'Max-VRVoltage (1.0.32.27.0.255;3;2)', "));
                builder.Append(string.Concat("min(Convert(rPhaseVoltage,decimal(10,3))) as 'Min-VRVoltage (1.0.32.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(yPhaseCurrent,decimal(10,3))) as 'Max-IYCurrent (1.0.51.27.0.255;3;2)', "));
                builder.Append(string.Concat("min(Convert(yPhaseCurrent,decimal(10,3))) as 'Min-IYCurrent (1.0.51.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(yPhaseVoltage,decimal(10,3))) as 'Max-VYVoltage (1.0.52.27.0.255;3;2)', "));
                builder.Append(string.Concat("min(Convert(yPhaseVoltage,decimal(10,3))) as 'Min-VYVoltage (1.0.52.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(bPhaseCurrent,decimal(10,3))) as 'Max-IBCurrent (1.0.71.27.0.255;3;2)', "));
                builder.Append(string.Concat("min(Convert(bPhaseCurrent,decimal(10,3))) as 'Min-IBCurrent (1.0.71.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(bPhaseVoltage,decimal(10,3))) as 'Max-VBVoltage (1.0.72.27.0.255;3;2)', "));
                builder.Append(string.Concat("min(Convert(bPhaseVoltage,decimal(10,3))) as 'Min-VBVoltage (1.0.72.27.0.255;3;2)', "));
                builder.Append(string.Concat("max(Convert(substring(blockenergykwh,1,instr(blockenergykwh,'.')+ " + resolution + "),decimal(10," + resolution + "))) as 'MD - kW (1.0.1.29.0.255;3;2)',  "));
                builder.Append(string.Concat("max(Convert(substring(blockenergykVAh,1,instr(blockenergykVAh,'.')+ " + resolution + "),decimal(10," + resolution + "))) as 'MD - kVA (1.0.9.29.0.255;3;2)' "));
                builder.Append(string.Concat("from meterdata_loadsurvey where MeterData_ID = " + meterDataId + " and "));
                builder.Append(string.Concat("realTimeClockDateandTime >= '" + fromDate + "' and realTimeClockDateandTime <= '" + toDate));
                builder.Append(string.Concat("' group by convert(realTimeClockDateandTime,char(8))"));
                builder.Append(string.Concat(" order by LoadSurvey_ID desc"));

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidNightData(string fileName, long meterDataId, long fromDate, long toDate, out int resolution)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListDataSetWithColumns(long meterDataId, long fromDate, long toDate, string lsColumnParameters)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // Added PUMa condition
                if (!isPUMA)
                {
                    builder.Append("Select distinct ");
                }
                else
                {
                    builder.Append("Select ");
                }
                builder.Append(lsColumnParameters);
                builder.Append(",MDIntervalPeriod from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                if (isPUMA)
                {
                    /* GKG 30/01/2012 TFS ID 134283 */
                    //  builder.Append(string.Concat(" order by LoadSurvey_ID"));
        			// Story - 427028 - Load survey data sequence should be in descending order except graph
                    //if (isTNEB)
                    //{
                    //    builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                    //}
                    //// Story - 349654 - In case of single phase non DLMS meter, data is coming in reverse order
                    //else if (ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
                    //{
                        builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                    //}
                    //else
                    //{
                    //    builder.Append(string.Concat(" order by LoadSurvey_ID"));
                    //}
                    /* GKG 30/01/2012 TFS ID 134283 */

                }
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetWithColumns(long meterDataId, long fromDate, long toDate, string lsColumnParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public string IsNet(int meterDataID)
        {

            string IsNet = string.Empty;
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT MeterModelNo,MeterData_ID,case NetMeterVariantInfo when '3' then 'Net' when '4' then 'NetImport' else                                  'NonNet' end as 'IsNet' from `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                IsNet = Convert.ToString(dataSet.Tables[0].Rows[0]["IsNet"]);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsNet(int meterDataID)", ex);
                dataSet = null;
                IsNet = string.Empty;
            }
            return IsNet;
        }

        //public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        //{
        //    DataSet dataSet = null;
        //    try
        //    {
        //        IDataHelper helper = DatabaseFactory.GetHelper();
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Select * ");
        //        builder.Append(" from meterdata_loadsurvey where ");
        //        builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
        //        builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
        //        builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
        //        builder.Append(string.Concat(" order by realTimeClockDateandTime"));
        //        DataRequest request = new DataRequest(builder.ToString());
        //        request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
        //        request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
        //        request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
        //        dataSet = new DataSet();
        //        dataSet = helper.FillDataSet(request, dataSet);
        //        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
        //    }
        //    catch (CABException ex)    //Exception log for catch block
        //    {
        //        dataSet = null;
        //    }
        //    return dataSet;
        //}

        private DataRequest GetRequest(IEntity entity)
        {

            if (entity == null)
                return null;
            DLMS650LoadSurveyEntity loadSurveyEntity = entity as DLMS650LoadSurveyEntity;

            StringBuilder builder = new StringBuilder();
            if (isPUMA)
                builder.Append("Insert Into meterdata_loadsurvey(realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,averageCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,averageVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,blockEnergykWhExport, blockEnergykvarhlagQ3,blockEnergykvarhleadQ2,blockEnergykVAhExport,blockEnergykWhImport, blockEnergykvarhlagQ1,blockEnergykvarhleadQ4,blockEnergykVAhImport,MeterData_ID,MDIntervalPeriod,IsDLMS,frequency,tamperStatus,NeutralCurrent,AvgPhaseCurrent) values(");//add pradipta_load_neu
            else
                builder.Append("Insert Into meterdata_loadsurvey(realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,averageCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,averageVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,MeterData_ID,MDIntervalPeriod,IsDLMS) values(");

            builder.Append(string.Concat(ParameterName(realTimeClockDateandTime), ","));
            builder.Append(string.Concat(ParameterName(rPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(yPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(bPhaseCurrent), ","));
            builder.Append(string.Concat(ParameterName(averageCurrent), ","));
            builder.Append(string.Concat(ParameterName(rPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(yPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(bPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(averageVoltage), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykWh), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhlag), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhlead), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykVAh), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykWhExport), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhlagQ3), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhleadQ2), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykVAhExport), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykWhImport), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhlagQ1), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykvarhleadQ4), ","));
            builder.Append(string.Concat(ParameterName(blockEnergykVAhImport), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(MDIntervalPeriod), ","));
            builder.Append(string.Concat(ParameterName(neutralcurrent), ","));//add pradipta_load_neu
            builder.Append(string.Concat(ParameterName(avgphasecurrent), ","));


            builder.Append(ParameterName(IsDLMS));
            //added PUMA
            if (isPUMA)
            {
                builder.Append(",");
                builder.Append(string.Concat(ParameterName(Frequency), ","));
                builder.Append(string.Concat(ParameterName(Tamper), ")"));
            }
            else
            {
                builder.Append(")");
            }
            DataRequest request = new DataRequest(builder.ToString());

            // Story - 354382 - For a day Time 240000 is coming which would be treated as 000000 for next day for Single phase meter
            if (!string.IsNullOrEmpty(loadSurveyEntity.RealTimeClockDateandTime.ToString()))
            {
                if (loadSurveyEntity.RealTimeClockDateandTime.ToString().Substring(8, 2) == "24" && loadSurveyEntity.RealTimeClockDateandTime.ToString().Substring(10, 2) == "00" && loadSurveyEntity.RealTimeClockDateandTime.ToString().Substring(12, 2) == "00")
                {
                    DateTime realTimeClockDateTime = DateUtility.LongToDateTime(Convert.ToInt64(loadSurveyEntity.RealTimeClockDateandTime.ToString()));
                    loadSurveyEntity.RealTimeClockDateandTime = DateUtility.DateTimeToLong(realTimeClockDateTime);
                }
            }

            request.AddParamter(ParameterName(realTimeClockDateandTime), loadSurveyEntity.RealTimeClockDateandTime, DbType.Int64);
            request.AddParamter(ParameterName(rPhaseCurrent), loadSurveyEntity.RPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(yPhaseCurrent), loadSurveyEntity.YPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(bPhaseCurrent), loadSurveyEntity.BPhaseCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(averageCurrent), loadSurveyEntity.AverageCurrent, DbType.String, 40);
            request.AddParamter(ParameterName(rPhaseVoltage), loadSurveyEntity.RPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(yPhaseVoltage), loadSurveyEntity.YPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(bPhaseVoltage), loadSurveyEntity.BPhaseVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(averageVoltage), loadSurveyEntity.AverageVoltage, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykWh), loadSurveyEntity.BlockEnergykWh, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhlag), loadSurveyEntity.BlockEnergykvarhlag, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhlead), loadSurveyEntity.BlockEnergykvarhlead, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykVAh), loadSurveyEntity.BlockEnergykVAh, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykWhExport), loadSurveyEntity.BlockEnergykWhExport, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhlagQ3), loadSurveyEntity.BlockEnergykvarhlagQ3, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhleadQ2), loadSurveyEntity.BlockEnergykvarhleadQ2, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykVAhExport), loadSurveyEntity.BlockEnergykVAhExport, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykWhImport), loadSurveyEntity.BlockEnergykWhImport, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhlagQ1), loadSurveyEntity.BlockEnergykvarhlagQ1, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykvarhleadQ4), loadSurveyEntity.BlockEnergykvarhleadQ4, DbType.String, 40);
            request.AddParamter(ParameterName(blockEnergykVAhImport), loadSurveyEntity.BlockEnergykVAhImport, DbType.String, 40);
            request.AddParamter(ParameterName(MeterData_ID), loadSurveyEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(MDIntervalPeriod), loadSurveyEntity.MDIntervalPeriod, DbType.Int32);

            request.AddParamter(ParameterName(neutralCurrent), loadSurveyEntity.NeuCurrent, DbType.String, 40);//add pradipta_load_neu

            request.AddParamter(ParameterName(IsDLMS), loadSurveyEntity.IsDLMS, DbType.Int32);
            //added
            if (isPUMA)
            {
                request.AddParamter(ParameterName(Frequency), loadSurveyEntity.Frequency, DbType.String, 40);
                request.AddParamter(ParameterName(Tamper), loadSurveyEntity.TamperStatus, DbType.String, 40);
            }
            return request;
        }



        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DLMS650LoadSurveyEntity loadSurveyEntity = new DLMS650LoadSurveyEntity();
            if (NotNullAndNotDBNull(row, LoadSurvey_ID)) loadSurveyEntity.LoadSurvey_ID = Convert.ToInt64(row[LoadSurvey_ID]);
            if (NotNullAndNotDBNull(row, realTimeClockDateandTime)) loadSurveyEntity.RealTimeClockDateandTime = Convert.ToInt64(row[realTimeClockDateandTime]);
            if (NotNullAndNotDBNull(row, rPhaseCurrent)) loadSurveyEntity.RPhaseCurrent = Convert.ToString(row[rPhaseCurrent]);
            if (NotNullAndNotDBNull(row, yPhaseCurrent)) loadSurveyEntity.YPhaseCurrent = Convert.ToString(row[yPhaseCurrent]);
            if (NotNullAndNotDBNull(row, bPhaseCurrent)) loadSurveyEntity.BPhaseCurrent = Convert.ToString(row[bPhaseCurrent]);
            if (NotNullAndNotDBNull(row, rPhaseVoltage)) loadSurveyEntity.RPhaseVoltage = Convert.ToString(row[rPhaseVoltage]);
            if (NotNullAndNotDBNull(row, yPhaseVoltage)) loadSurveyEntity.YPhaseVoltage = Convert.ToString(row[yPhaseVoltage]);
            if (NotNullAndNotDBNull(row, bPhaseVoltage)) loadSurveyEntity.BPhaseVoltage = Convert.ToString(row[bPhaseVoltage]);
            if (NotNullAndNotDBNull(row, blockEnergykWh)) loadSurveyEntity.BlockEnergykWh = Convert.ToString(row[blockEnergykWh]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhlag)) loadSurveyEntity.BlockEnergykvarhlag = Convert.ToString(row[blockEnergykvarhlag]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhlead)) loadSurveyEntity.BlockEnergykvarhlead = Convert.ToString(row[blockEnergykvarhlead]);
            if (NotNullAndNotDBNull(row, blockEnergykVAh)) loadSurveyEntity.BlockEnergykVAh = Convert.ToString(row[blockEnergykVAh]);
            if (NotNullAndNotDBNull(row, blockEnergykWhExport)) loadSurveyEntity.BlockEnergykWhExport = Convert.ToString(row[blockEnergykWhExport]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhlagQ3)) loadSurveyEntity.BlockEnergykvarhlagQ3 = Convert.ToString(row[blockEnergykvarhlagQ3]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhleadQ2)) loadSurveyEntity.BlockEnergykvarhleadQ2 = Convert.ToString(row[blockEnergykvarhleadQ2]);
            if (NotNullAndNotDBNull(row, blockEnergykVAhExport)) loadSurveyEntity.BlockEnergykVAhExport = Convert.ToString(row[blockEnergykVAhExport]);
            if (NotNullAndNotDBNull(row, blockEnergykWhImport)) loadSurveyEntity.BlockEnergykWhImport = Convert.ToString(row[blockEnergykWhImport]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhlagQ1)) loadSurveyEntity.BlockEnergykvarhlagQ1 = Convert.ToString(row[blockEnergykvarhlagQ1]);
            if (NotNullAndNotDBNull(row, blockEnergykvarhleadQ4)) loadSurveyEntity.BlockEnergykvarhleadQ4 = Convert.ToString(row[blockEnergykvarhleadQ4]);
            if (NotNullAndNotDBNull(row, blockEnergykVAhImport)) loadSurveyEntity.BlockEnergykVAhImport = Convert.ToString(row[blockEnergykVAhImport]);


            if (NotNullAndNotDBNull(row, neutralcurrent)) loadSurveyEntity.NeuCurrent = Convert.ToString(row[neutralcurrent]);//add pradipta_neu


            if (NotNullAndNotDBNull(row, MeterData_ID)) loadSurveyEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            return loadSurveyEntity;
        }

        public override IEntity InsertData(IEntity entity)
        {
            BillingEntity billingEntity = entity as BillingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = this.GetRequest(entity);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            if (Flag)
                billingEntity.Billing_ID = long.Parse(this.GetPK());
            return billingEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();
            foreach (IEntity entity in entities)
                requests.Add(this.GetRequest(entity));
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IList<IEntity> entities)", ex);
            }
            return null; 
        }
          /// <summary>
        /// bulk insert
          /// </summary>
          /// <param name="entities"></param>
          /// <returns></returns>
        public  void BatchInsert(IList<IEntity> entities)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("realTimeClockDateandTime", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("rPhaseCurrent", typeof(System.String)));
            table.Columns.Add(new DataColumn("yPhaseCurrent", typeof(System.String)));
            table.Columns.Add(new DataColumn("bPhaseCurrent", typeof(System.String)));
            table.Columns.Add(new DataColumn("averageCurrent", typeof(System.String)));
            table.Columns.Add(new DataColumn("rPhaseVoltage", typeof(System.String)));
            table.Columns.Add(new DataColumn("yPhaseVoltage", typeof(System.String)));
            table.Columns.Add(new DataColumn("bPhaseVoltage", typeof(System.String)));
            table.Columns.Add(new DataColumn("averageVoltage", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykWh", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhlag", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhlead", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykVAh", typeof(System.String)));

            table.Columns.Add(new DataColumn("blockEnergykWhExport", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhlagQ3", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhleadQ2", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykVAhExport", typeof(System.String)));

            table.Columns.Add(new DataColumn("blockEnergykWhImport", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhlagQ1", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhleadQ4", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykVAhImport", typeof(System.String)));

            table.Columns.Add(new DataColumn("blockEnergykWhRPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykWhYPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykWhBPhase", typeof(System.String)));

            table.Columns.Add(new DataColumn("blockEnergykvarhQ12", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhQ34", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhQ14", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergykvarhQ23", typeof(System.String)));
            table.Columns.Add(new DataColumn("blockEnergyFundamentalkWhAbsolute", typeof(System.String)));

            table.Columns.Add(new DataColumn("netkWh", typeof(System.String)));
            table.Columns.Add(new DataColumn("netkVAh", typeof(System.String)));

            table.Columns.Add(new DataColumn("activePowerRPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("activePowerYPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("activePowerBPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("apparentPowerRPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("apparentPowerYPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("apparentPowerBPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("reactivePowerRPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("reactivePowerYPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("reactivePowerBPhase", typeof(System.String)));
            table.Columns.Add(new DataColumn("powerOffDurationLSIP", typeof(System.String)));

            table.Columns.Add(new DataColumn("temperature", typeof(System.String)));


            table.Columns.Add(new DataColumn("MeterData_ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("MDIntervalPeriod", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("IsDLMS", typeof(System.Int32)));
            if (isPUMA)
            {
                table.Columns.Add(new DataColumn("Frequency", typeof(System.String)));
                table.Columns.Add(new DataColumn("Tamper", typeof(System.String)));
                table.Columns.Add(new DataColumn("TamperFlag", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgVolta3ph", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgRPHPF", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgYPHPF", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgBPHPF", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgTotalPF", typeof(System.String)));
                table.Columns.Add(new DataColumn("AvgNeutralCurrent", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdVr", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdVy", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdVb", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdIr", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdIy", typeof(System.String)));
                table.Columns.Add(new DataColumn("ThdIb", typeof(System.String)));
                table.Columns.Add(new DataColumn("NeutralCurrent", typeof(System.String)));//add pradipta_load_neu
                table.Columns.Add(new DataColumn("AvgPhaseCurrent", typeof(System.String)));

            }
            
           

            try
            {
                foreach (IEntity entity in entities)
                {
                    DLMS650LoadSurveyEntity loadSurveyEntity = entity as DLMS650LoadSurveyEntity;
                    DataRow dr = table.NewRow();
                    dr["realTimeClockDateandTime"] = loadSurveyEntity.RealTimeClockDateandTime;
                    dr["rPhaseCurrent"] = loadSurveyEntity.RPhaseCurrent;
                    dr["yPhaseCurrent"] = loadSurveyEntity.YPhaseCurrent;
                    dr["bPhaseCurrent"] = loadSurveyEntity.BPhaseCurrent;
                    dr["averageCurrent"] = loadSurveyEntity.AverageCurrent;
                    dr["rPhaseVoltage"] = loadSurveyEntity.RPhaseVoltage;
                    dr["yPhaseVoltage"] = loadSurveyEntity.YPhaseVoltage;
                    dr["bPhaseVoltage"] = loadSurveyEntity.BPhaseVoltage;
                    dr["averageVoltage"] = loadSurveyEntity.AverageVoltage;
                    dr["blockEnergykWh"] = loadSurveyEntity.BlockEnergykWh;
                    dr["blockEnergykvarhlag"] = loadSurveyEntity.BlockEnergykvarhlag;
                    dr["blockEnergykvarhlead"] = loadSurveyEntity.BlockEnergykvarhlead;
                    dr["blockEnergykVAh"] = loadSurveyEntity.BlockEnergykVAh;

                    dr["blockEnergykWhExport"] = loadSurveyEntity.BlockEnergykWhExport;
                    dr["blockEnergykvarhlagQ3"] = loadSurveyEntity.BlockEnergykvarhlagQ3;
                    dr["blockEnergykvarhleadQ2"] = loadSurveyEntity.BlockEnergykvarhleadQ2;
                    dr["blockEnergykVAhExport"] = loadSurveyEntity.BlockEnergykVAhExport;
                    dr["blockEnergykWhImport"] = loadSurveyEntity.BlockEnergykWhImport;
                    dr["blockEnergykvarhlagQ1"] = loadSurveyEntity.BlockEnergykvarhlagQ1;
                    dr["blockEnergykvarhleadQ4"] = loadSurveyEntity.BlockEnergykvarhleadQ4;
                    dr["blockEnergykVAhImport"] = loadSurveyEntity.BlockEnergykVAhImport;

                    dr["blockEnergykWhRPhase"] = loadSurveyEntity.BlockEnergykWhRPhase;
                    dr["blockEnergykWhYPhase"] = loadSurveyEntity.BlockEnergykWhYPhase;
                    dr["blockEnergykWhBPhase"] = loadSurveyEntity.BlockEnergykWhBPhase;
                    dr["blockEnergykvarhQ12"] = loadSurveyEntity.BlockEnergykvarhQ12;
                    dr["blockEnergykvarhQ34"] = loadSurveyEntity.BlockEnergykvarhQ34;
                    dr["blockEnergykvarhQ14"] = loadSurveyEntity.BlockEnergykvarhQ14;
                    dr["blockEnergykvarhQ23"] = loadSurveyEntity.BlockEnergykvarhQ23;
                    dr["blockEnergyFundamentalkWhAbsolute"] = loadSurveyEntity.BlockEnergyFundamentalkWhAbsolute;

                    dr["netkWh"] = loadSurveyEntity.NetkWh;
                    dr["netkVAh"] = loadSurveyEntity.NetkVAh;

                    dr["activePowerRPhase"] = loadSurveyEntity.ActivePowerRPhase;
                    dr["activePowerYPhase"] = loadSurveyEntity.ActivePowerYPhase;
                    dr["activePowerBPhase"] = loadSurveyEntity.ActivePowerBPhase;
                    dr["apparentPowerRPhase"] = loadSurveyEntity.ApparentPowerRPhase;
                    dr["apparentPowerYPhase"] = loadSurveyEntity.ApparentPowerYPhase;
                    dr["apparentPowerBPhase"] = loadSurveyEntity.ApparentPowerBPhase;
                    dr["reactivePowerRPhase"] = loadSurveyEntity.ReactivePowerRPhase;
                    dr["reactivePowerYPhase"] = loadSurveyEntity.ReactivePowerYPhase;
                    dr["reactivePowerBPhase"] = loadSurveyEntity.ReactivePowerBPhase;
                    dr["powerOffDurationLSIP"] = loadSurveyEntity.PowerOffDurationLSIP;

                    dr["temperature"] = loadSurveyEntity.Temperature;

                    dr["MeterData_ID"] = loadSurveyEntity.MeterData_ID;
                    dr["MDIntervalPeriod"] = loadSurveyEntity.MDIntervalPeriod;
                    dr["IsDLMS"] = loadSurveyEntity.IsDLMS;
                    if (isPUMA)
                    {
                        dr["Frequency"] = loadSurveyEntity.Frequency;
                        dr["Tamper"] = loadSurveyEntity.TamperStatus;
                        dr["TamperFlag"] = loadSurveyEntity.TemperFlag;
                        dr["AvgVolta3ph"] = loadSurveyEntity.AVgVolt3phase;
                        dr["AvgRPHPF"] = loadSurveyEntity.AvgRphPF;
                        dr["AvgYPHPF"] = loadSurveyEntity.AvgYphPF;
                        dr["AvgBPHPF"] = loadSurveyEntity.AvgBphPF;
                        dr["AvgTotalPF"] = loadSurveyEntity.AvgTotalPF;
                        dr["AvgNeutralCurrent"] = loadSurveyEntity.AvgNeuCurrent;
                        dr["ThdVr"] = loadSurveyEntity.THDVR;
                        dr["ThdVy"] = loadSurveyEntity.THDVY;
                        dr["ThdVb"] = loadSurveyEntity.THDVB;
                        dr["ThdIr"] = loadSurveyEntity.THDIR;
                        dr["ThdIy"] = loadSurveyEntity.THDIY;
                        dr["ThdIb"] = loadSurveyEntity.THDIB;                        
                        dr["NeutralCurrent"] = loadSurveyEntity.NeuCurrent;//add pradipta_load_neu
                        dr["avgphasecurrent"] = loadSurveyEntity.AvgPhaseCurrent;

                    }
                    table.Rows.Add(dr);
                }
                StringBuilder builder = new StringBuilder();
                if (isPUMA)
                    builder.Append("Insert Into meterdata_loadsurvey(realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,averageCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,averageVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,blockEnergykWhExport, blockEnergykvarhlagQ3,blockEnergykvarhleadQ2,blockEnergykVAhExport,blockEnergykWhImport,blockEnergykvarhlagQ1,blockEnergykvarhleadQ4,blockEnergykVAhImport,blockEnergykWhRPhase,blockEnergykWhYPhase,blockEnergykWhBPhase,blockEnergykvarhQ12,blockEnergykvarhQ34,blockEnergykvarhQ14,blockEnergykvarhQ23,blockEnergyFundamentalkWhAbsolute,netkWh,netkVAh,activePowerRPhase,activePowerYPhase,activePowerBPhase,apparentPowerRPhase,apparentPowerYPhase,apparentPowerBPhase,reactivePowerRPhase,reactivePowerYPhase,reactivePowerBPhase,powerOffDurationLSIP,temperature,MeterData_ID,MDIntervalPeriod,IsDLMS,frequency,tamperStatus,tamperflag,Avgvoltageof3phase,AvgRphasePF,AvgYphasePF,AvgBphasePF,AvgTotalPF,AvgNeutralCurrent,ThdVr,ThdVy,ThdVb,ThdIr,ThdIy,ThdIb,NeutralCurrent,AvgPhaseCurrent) values(");//add pradipta_load_neu
                else
                    builder.Append("Insert Into meterdata_loadsurvey(realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,averageCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,averageVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,temperature,MeterData_ID,MDIntervalPeriod,IsDLMS) values(");

                builder.Append(string.Concat(ParameterName(realTimeClockDateandTime), ","));
                builder.Append(string.Concat(ParameterName(rPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(yPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(bPhaseCurrent), ","));
                builder.Append(string.Concat(ParameterName(averageCurrent), ","));
                builder.Append(string.Concat(ParameterName(rPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(yPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(bPhaseVoltage), ","));
                builder.Append(string.Concat(ParameterName(averageVoltage), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykWh), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhlag), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhlead), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykVAh), ","));

                builder.Append(string.Concat(ParameterName(blockEnergykWhExport), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhlagQ3), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhleadQ2), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykVAhExport), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykWhImport), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhlagQ1), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhleadQ4), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykVAhImport), ","));

                builder.Append(string.Concat(ParameterName(blockEnergykWhRPhase), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykWhYPhase), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykWhBPhase), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhQ12), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhQ34), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhQ14), ","));
                builder.Append(string.Concat(ParameterName(blockEnergykvarhQ23), ","));
                builder.Append(string.Concat(ParameterName(blockEnergyFundamentalkWhAbsolute), ","));

                builder.Append(string.Concat(ParameterName(netkWh), ","));
                builder.Append(string.Concat(ParameterName(netkVAh), ","));

                builder.Append(string.Concat(ParameterName(activePowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(activePowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(activePowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(apparentPowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(apparentPowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(apparentPowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(reactivePowerRPhase), ","));
                builder.Append(string.Concat(ParameterName(reactivePowerYPhase), ","));
                builder.Append(string.Concat(ParameterName(reactivePowerBPhase), ","));
                builder.Append(string.Concat(ParameterName(powerOffDurationLSIP), ","));

                builder.Append(String.Concat(ParameterName(temperature), ","));

                builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
                builder.Append(string.Concat(ParameterName(MDIntervalPeriod), ","));
                builder.Append(ParameterName(IsDLMS));
                //added PUMA
                if (isPUMA)
                {
                    builder.Append(",");
                    builder.Append(string.Concat(ParameterName(Frequency), ","));
                    builder.Append(string.Concat(ParameterName(Tamper), ","));
                    builder.Append(string.Concat(ParameterName(TamperFlag), ","));
                    builder.Append(string.Concat(ParameterName(AvgVolta3ph), ","));
                    builder.Append(string.Concat(ParameterName(AvgRPHPF), ","));//for smart meter
                    builder.Append(string.Concat(ParameterName(AvgYPHPF), ","));
                    builder.Append(string.Concat(ParameterName(AvgBPHPF), ","));
                    builder.Append(string.Concat(ParameterName(AvgTotalPF), ","));
                    builder.Append(string.Concat(ParameterName(AvgNeutralCurrent), ","));
                    builder.Append(string.Concat(ParameterName(ThdVr), ","));
                    builder.Append(string.Concat(ParameterName(ThdVy), ","));
                    builder.Append(string.Concat(ParameterName(ThdVb), ","));
                    builder.Append(string.Concat(ParameterName(ThdIr), ","));
                    builder.Append(string.Concat(ParameterName(ThdIy), ","));
                    builder.Append(string.Concat(ParameterName(ThdIb), ","));
                    builder.Append(string.Concat(ParameterName(neutralcurrent), ","));//add pradipta_load_neu 
                   builder.Append(String.Concat(ParameterName(avgphasecurrent), ")"));
                }
                else
                {
                    builder.Append(String.Concat(ParameterName(neutralcurrent), ","));//add pradipta_load_neu
                    builder.Append(")");
                }
                //List<DataRequest> requests = new List<DataRequest>();
                //foreach (IEntity entity in entities)
                //    requests.Add(this.GetRequest(entity));


                MySqlCommand command = new MySqlCommand(builder.ToString());
                command.CommandType = CommandType.Text;
                command.Parameters.Add("?realTimeClockDateandTime", MySqlDbType.Int64).SourceColumn = "realTimeClockDateandTime";
                command.Parameters.Add("?rPhaseCurrent", MySqlDbType.String, 40).SourceColumn = "rPhaseCurrent";
                command.Parameters.Add("?yPhaseCurrent", MySqlDbType.String, 40).SourceColumn = "yPhaseCurrent";
                command.Parameters.Add("?bPhaseCurrent", MySqlDbType.String, 40).SourceColumn = "bPhaseCurrent";
                command.Parameters.Add("?averageCurrent", MySqlDbType.String, 40).SourceColumn = "averageCurrent";
                command.Parameters.Add("?rPhaseVoltage", MySqlDbType.String, 40).SourceColumn = "rPhaseVoltage";
                command.Parameters.Add("?yPhaseVoltage", MySqlDbType.String, 40).SourceColumn = "yPhaseVoltage";
                command.Parameters.Add("?bPhaseVoltage", MySqlDbType.String, 40).SourceColumn = "bPhaseVoltage";
                command.Parameters.Add("?averageVoltage", MySqlDbType.String, 40).SourceColumn = "averageVoltage";
                command.Parameters.Add("?blockEnergykWh", MySqlDbType.String, 40).SourceColumn = "blockEnergykWh";
                command.Parameters.Add("?blockEnergykvarhlag", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhlag";
                command.Parameters.Add("?blockEnergykvarhlead", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhlead";
                command.Parameters.Add("?blockEnergykVAh", MySqlDbType.String, 40).SourceColumn = "blockEnergykVAh";
                command.Parameters.Add("?blockEnergykWhExport", MySqlDbType.String, 40).SourceColumn = "blockEnergykWhExport";
                command.Parameters.Add("?blockEnergykvarhlagQ3", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhlagQ3";
                command.Parameters.Add("?blockEnergykvarhleadQ2", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhleadQ2";
                command.Parameters.Add("?blockEnergykVAhExport", MySqlDbType.String, 40).SourceColumn = "blockEnergykVAhExport";
                command.Parameters.Add("?blockEnergykWhImport", MySqlDbType.String, 40).SourceColumn = "blockEnergykWhImport";
                command.Parameters.Add("?blockEnergykvarhlagQ1", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhlagQ1";
                command.Parameters.Add("?blockEnergykvarhleadQ4", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhleadQ4";
                command.Parameters.Add("?blockEnergykVAhImport", MySqlDbType.String, 40).SourceColumn = "blockEnergykVAhImport";
                command.Parameters.Add("?blockEnergykWhRPhase", MySqlDbType.String, 40).SourceColumn = "blockEnergykWhRPhase";
                command.Parameters.Add("?blockEnergykWhYPhase", MySqlDbType.String, 40).SourceColumn = "blockEnergykWhYPhase";
                command.Parameters.Add("?blockEnergykWhBPhase", MySqlDbType.String, 40).SourceColumn = "blockEnergykWhBPhase";
                command.Parameters.Add("?blockEnergykvarhQ12", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhQ12";
                command.Parameters.Add("?blockEnergykvarhQ34", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhQ34";
                command.Parameters.Add("?blockEnergykvarhQ14", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhQ14";
                command.Parameters.Add("?blockEnergykvarhQ23", MySqlDbType.String, 40).SourceColumn = "blockEnergykvarhQ23";
                command.Parameters.Add("?blockEnergyFundamentalkWhAbsolute", MySqlDbType.String, 40).SourceColumn = "blockEnergyFundamentalkWhAbsolute";

                command.Parameters.Add("?netkWh", MySqlDbType.String, 40).SourceColumn = "netkWh";
                command.Parameters.Add("?netkVAh", MySqlDbType.String, 40).SourceColumn = "netkVAh";
                command.Parameters.Add("?activePowerRPhase", MySqlDbType.String, 40).SourceColumn = "activePowerRPhase";
                command.Parameters.Add("?activePowerYPhase", MySqlDbType.String, 40).SourceColumn = "activePowerYPhase";
                command.Parameters.Add("?activePowerBPhase", MySqlDbType.String, 40).SourceColumn = "activePowerBPhase";
                command.Parameters.Add("?apparentPowerRPhase", MySqlDbType.String, 40).SourceColumn = "apparentPowerRPhase";
                command.Parameters.Add("?apparentPowerYPhase", MySqlDbType.String, 40).SourceColumn = "apparentPowerYPhase";
                command.Parameters.Add("?apparentPowerBPhase", MySqlDbType.String, 40).SourceColumn = "apparentPowerBPhase";
                command.Parameters.Add("?reactivePowerRPhase", MySqlDbType.String, 40).SourceColumn = "reactivePowerRPhase";
                command.Parameters.Add("?reactivePowerYPhase", MySqlDbType.String, 40).SourceColumn = "reactivePowerYPhase";
                command.Parameters.Add("?reactivePowerBPhase", MySqlDbType.String, 40).SourceColumn = "reactivePowerBPhase";
                command.Parameters.Add("?powerOffDurationLSIP", MySqlDbType.String, 40).SourceColumn = "powerOffDurationLSIP";
                command.Parameters.Add("?temperature", MySqlDbType.String, 40).SourceColumn = "temperature";
                command.Parameters.Add("?MeterData_ID", MySqlDbType.Int64).SourceColumn = "MeterData_ID";
                command.Parameters.Add("?MDIntervalPeriod", MySqlDbType.Int32).SourceColumn = "MDIntervalPeriod";
                command.Parameters.Add("?IsDLMS", MySqlDbType.Int32).SourceColumn = "IsDLMS";

                if (isPUMA)
                {
                    command.Parameters.Add("?frequency", MySqlDbType.String, 40).SourceColumn = "Frequency";
                    command.Parameters.Add("?tamperStatus", MySqlDbType.String, 40).SourceColumn = "Tamper";
                    command.Parameters.Add("?tamperflag", MySqlDbType.String, 40).SourceColumn = "TamperFlag";
                    command.Parameters.Add("?Avgvoltageof3phase", MySqlDbType.String, 40).SourceColumn = "AvgVolta3ph";
                    command.Parameters.Add("?AvgRphasePF", MySqlDbType.String, 40).SourceColumn = "AvgRPHPF";
                    command.Parameters.Add("?AvgYphasePF", MySqlDbType.String, 40).SourceColumn = "AvgYPHPF";
                    command.Parameters.Add("?AvgBphasePF", MySqlDbType.String, 40).SourceColumn = "AvgBPHPF";
                    command.Parameters.Add("?AvgTotalPF", MySqlDbType.String, 40).SourceColumn = "AvgTotalPF";
                    command.Parameters.Add("?AvgNeutralCurrent", MySqlDbType.String, 40).SourceColumn = "AvgNeutralCurrent";
                    command.Parameters.Add("?ThdVr", MySqlDbType.String, 40).SourceColumn = "ThdVr";
                    command.Parameters.Add("?ThdVy", MySqlDbType.String, 40).SourceColumn = "ThdVy";
                    command.Parameters.Add("?ThdVb", MySqlDbType.String, 40).SourceColumn = "ThdVb";
                    command.Parameters.Add("?ThdIr", MySqlDbType.String, 40).SourceColumn = "ThdIr";
                    command.Parameters.Add("?ThdIy", MySqlDbType.String, 40).SourceColumn = "ThdIy";
                    command.Parameters.Add("?ThdIb", MySqlDbType.String, 40).SourceColumn = "ThdIb";
                    command.Parameters.Add("?neutralcurrent", MySqlDbType.String, 40).SourceColumn = "neutralcurrent";//add pradipta_load_neu
                    command.Parameters.Add("?avgphasecurrent", MySqlDbType.String, 40).SourceColumn = "avgphasecurrent";
                }

                command.UpdatedRowSource = UpdateRowSource.None;
                helper.BatchInsert(table, command);
                //IDataHelper helper = DatabaseFactory.GetHelper();
                //helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchInsert(IList<IEntity> entities)", ex);
                string aa = ex.Message;
            }
           
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }
        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }
        public long GetToDate(long meterDataID)
        {
            long meterid = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Max(realTimeClockDateandTime)  from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                    meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetToDate(long meterDataID)", ex);
                meterid = 0;
            }
            return meterid;
        }

        public long GetFromDate(long meterDataID)
        {
            long meterid = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Min(realTimeClockDateandTime)  from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                    meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFromDate(long meterDataID)", ex);
                meterid = 0;
            }
            return meterid;
        }

        public DataSet GetLoadSurveyData(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    if (column == "Power Factor")
                    {
                        builder.Append(string.Concat(",", "IFNULL(substring(ls.blockenergykwh,1,instr(ls.blockenergykwh,'*')-1)/substring(ls.blockenergykVAh,1,instr(ls.blockenergykVAh,'*')-1),0) as '", column, "' "));
                        // (blockEnergykWh/blockEnergykVAh) as 'power factor'
                    }
                    else
                    {
                        builder.Append(string.Concat(",", "ls.", column, " "));
                    }
                }
                builder.Append(",m.MeterData_ID from meterdata_loadsurvey ls inner join meterdata m on ls.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                if (isPUMA)
                {
                    builder.Append(string.Concat(" order by ls.LoadSurvey_ID"));
                }
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyData(string meterID, List<string> columns)", ex);
            }
            return dataSet;
        }

        public DataSet GetLoadSurveyDataByFileName(string meterID, string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    if (column == "Power Factor")
                    {
                        builder.Append(string.Concat(",", "IFNULL(substring(ls.blockenergykwh,1,instr(ls.blockenergykwh,'*')-1)/substring(ls.blockenergykVAh,1,instr(ls.blockenergykVAh,'*')-1),0) as '", column, "' "));
                        // (blockEnergykWh/blockEnergykVAh) as 'power factor'
                    }
                    else
                    {
                        builder.Append(string.Concat(",", "ls.", column, " "));
                    }
                }
                builder.Append(",m.MeterData_ID from meterdata_loadsurvey ls inner join meterdata m on ls.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                if (isPUMA)
                {
                    builder.Append(string.Concat(" order by ls.LoadSurvey_ID"));
                }
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyDataByFileName(string meterID, string fileName, List<string> columns)", ex);
            }
            return dataSet;
        }
        public DataSet GetLoadSurveyByFileName(string meterID, string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    if (!(column == "Power Factor"))
                    {
                        builder.Append(string.Concat(",", "ls.", column, " "));
                    }
                }
                builder.Append(",m.MeterData_ID from meterdata_loadsurvey ls inner join meterdata m on ls.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                if (isPUMA)
                {
                    builder.Append(string.Concat(" order by ls.LoadSurvey_ID"));
                }
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyByFileName(string meterID, string fileName, List<string> columns)", ex);
            }
            return dataSet;
        }

        public DataSet GetLoadSurveyParameters(string meterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT parameter.MeterData_ID FROM loadsurveyparameter parameter , meterdata mdata where mdata.MeterData_ID = parameter.MeterData_ID and mdata.MeterID='11111111'; ");

                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.Int64);

                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyParameters(string meterID)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Lists DataSet With Columns of load survey For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="lsColumnParameters"></param>
        /// <returns></returns>
        public DataSet ListDataSetWithColumnsForMeterID(string meterID, long fromDate, long toDate, string lsColumnParameters)
        {
            string strAdder = lsColumnParameters;
            string[] strArray = strAdder.Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = "survey." + strArray[i];
            }
            lsColumnParameters = String.Join(",", strArray);
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // Added PUMa condition

                builder.Append("Select distinct ");
                builder.Append(lsColumnParameters);


                builder.Append(" ,MDIntervalPeriod from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_loadsurvey` survey ");
                builder.Append("on mdata.meterData_ID = survey.meterData_ID and mdata.");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " and "));
                builder.Append(string.Concat("survey.", realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat("survey.", realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by survey.realTimeClockDateandTime desc")); // Story - 427028 - Load survey data sequence should be in descending order except graph

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetWithColumnsForMeterID(string meterID, long fromDate, long toDate, string lsColumnParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// Lists DataSet With Columns of load survey For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet ListDataSetForMeterID(string meterID, long fromDate, long toDate)
        {

            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh,MDIntervalPeriod ,NeutralCurrent");//add pradipta_load_neu
                builder.Append(" from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_loadsurvey` survey ");
                builder.Append("on mdata.meterData_ID = survey.meterData_ID and mdata. ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                /* GKG 30/01/2012 TFS ID 134283 */
                // builder.Append(string.Concat(" order by LoadSurvey_ID"));
				// Story - 427028 - Load survey data sequence should be in descending order except graph
                //if (isPUMA && isTNEB)
                //{
                //    builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                //}
                //// Story - 349654 - In case of single phase non DLMS meter, data is coming in reverse order
                //else if (ConfigInfo.ActiveFileType == "NONDLMS" && ConfigInfo.ActiveMeterType == "1P-2W")
                //{
                    builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                //}
                //else
                //{
                //    builder.Append(string.Concat(" order by LoadSurvey_ID"));
                //}
                ///* GKG 30/01/2012 TFS ID 134283 */
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetForMeterID(string meterID, long fromDate, long toDate)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// Get Min Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMinDateForMeterID(string meterID)
        {
            long leastDate = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select min(");
                builder.Append(realTimeClockDateandTime);
                builder.Append(")from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_loadsurvey` survey ");
                builder.Append("on mdata.meterData_ID = survey.meterData_ID and mdata. ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));



                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    leastDate = 0;
                else
                    leastDate = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMinDateForMeterID(string meterID)", ex);
                leastDate = 0;

            }
            return leastDate;


        }
        /// <summary>
        /// Get Max Date for mutilple meterdata_id's  of a  MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public long GetMaxDateForMeterID(string meterID)
        {
            long maxDate = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select max(");
                builder.Append(realTimeClockDateandTime);
                builder.Append(")from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_loadsurvey` survey ");
                builder.Append("on mdata.meterData_ID = survey.meterData_ID and mdata. ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    maxDate = 0;
                else
                    maxDate = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMaxDateForMeterID(string meterID)", ex);
                maxDate = 0;

            }
            return maxDate;


        }

        /// <summary>
        /// gets meeter type from meter serial number
        /// </summary>
        /// <param name="meterId">meter serial number</param>
        /// <returns>meter type</returns>
        public string GetActiveMeterTypeByMeterId(string meterId)
        {
            string activeMeterType = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT DISTINCT metertype FROM meterdata_general where ");
                builder.Append(string.Concat("meterSerialNumber = ", ParameterName("MeterId")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterId"), meterId, DbType.String);
                activeMeterType = helper.ExecuteScalar(request).ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Type Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetActiveMeterTypeByMeterId(string meterId)", ex);
                activeMeterType = string.Empty;
            }
            return activeMeterType;
        }
    }
}
