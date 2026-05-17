
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
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class DLMS650MidnightDataDAL : DALBase
    {
        private string MidnightData_ID = "MidnightData_ID";
        private string realTimeClockDateandTime = "realTimeClockDateandTime";
        private string cumEnergykWh = "cumEnergykWh";
        private string cumEnergykvarhlag = "cumEnergykvarhlag";
        private string cumEnergykvarhlead = "cumEnergykvarhlead";
        private string cumEnergykVAh = "cumEnergykVAh";
        private string mDKW = "mDKW";
        private string mDKWDateTime = "mDKWDateTime";
        private string mDKVA = "mDKVA";
        private string mDKVADateTime = "mDKVADateTime";
        private string MeterData_ID = "MeterData_ID";
        private string MeterID = "MeterID";
        private string FileName = "FileName";

        private string PowerOnDuration = "PowerOnDuration";
        private string PowerFailureDuration = "PowerFailureDuration";
        private string PowerOnDurationThreePhases = "PowerOnDurationThreePhases";
        private string PowerOnDurationGeneric = "PowerOnDurationGeneric";
        private string PowerOnDurationGeneric1P = "PowerOnDurationGeneric1P";

        private string cumEnergykWhExport = "cumEnergykWhExport";
        private string cumEnergykvarhlagQ3 = "cumEnergykvarhlagQ3";
        private string cumEnergykvarhleadQ2 = "cumEnergykvarhleadQ2";
        private string cumEnergykVAhExport = "cumEnergykVAhExport";
        private string cumEnergykWhImport = "cumEnergykWhImport";
        private string cumEnergykvarhlagQ1 = "cumEnergykvarhlagQ1";
        private string cumEnergykvarhleadQ4 = "cumEnergykvarhleadQ4";
        private string cumEnergykVAhImport = "cumEnergykVAhImport";

        private string cumEnergykWhRPhase = "cumEnergykWhRPhase";
        private string cumEnergykWhYPhase = "cumEnergykWhYPhase";
        private string cumEnergykWhBPhase = "cumEnergykWhBPhase";
        private string cumEnergykvarhQ12 = "cumEnergykvarhQ12";
        private string cumEnergykvarhQ34 = "cumEnergykvarhQ34";
        private string cumEnergykvarhQ14 = "cumEnergykvarhQ14";
        private string cumEnergykvarhQ23 = "cumEnergykvarhQ23";
        private string fundamentalAbsolutekWH = "fundamentalAbsolutekWH";

        private string netkWh = "netkWh";
        private string netkVAh = "netkVAh";
        private string minVoltageLSIPAcrossDayRPhase = "minVoltageLSIPAcrossDayRPhase";
        private string minVoltageLSIPAcrossDayYPhase = "minVoltageLSIPAcrossDayYPhase";
        private string minVoltageLSIPAcrossDayBPhase = "minVoltageLSIPAcrossDayBPhase";
        private string highestCurrentLSIPAcrossDayRPhase = "highestCurrentLSIPAcrossDayRPhase";
        private string highestCurrentLSIPAcrossDayYPhase = "highestCurrentLSIPAcrossDayYPhase";
        private string highestCurrentLSIPAcrossDayBPhase = "highestCurrentLSIPAcrossDayBPhase";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650MidnightDataDAL).ToString());

        public DLMS650MidnightDataDAL()
            : base("meterdata_midnightdata", "MidnightData_ID")
        {
		}
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct realTimeClockDateandTime,rPhaseCurrent,yPhaseCurrent,bPhaseCurrent,rPhaseVoltage,yPhaseVoltage,bPhaseVoltage,blockEnergykWh,blockEnergykvarhlag,blockEnergykvarhlead,blockEnergykVAh ,NeutralCurrent");//add pradipta_load_neu
                builder.Append(" from meterdata_loadsurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by realTimeClockDateandTime"));
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
                logger.Log(LOGLEVELS.Error, "ListDataSet(long meterDataId, long fromDate, long toDate)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        ////added PUMA Daily Consumption
        //public DataSet GetDailyConsumption(long meterDataId, long fromDate, long toDate)
        //{
        //    DataSet dataSet = null;
        //    IDataHelper helper = DatabaseFactory.GetHelper();
        //    StringBuilder builder = new StringBuilder();

        //    try
        //    {
        //        //Added convert function.Solved bug 73406.
        //        builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date', ");
        //        builder.Append(string.Concat("sum(Convert(blockEnergykWh,decimal(10,3))) as 'Daily Consumption - kWh', "));
        //        builder.Append(string.Concat("sum(Convert(blockEnergykVAh,decimal(10,3))) as 'Daily Consumption - kVAh', "));
        //        builder.Append(string.Concat("sum(Convert(blockEnergykvarhlag,decimal(10,3))) as 'Daily Consumption - kvarh Lag', "));
        //        builder.Append(string.Concat("sum(Convert(blockEnergykvarhlead,decimal(10,3))) as 'Daily Consumption - kvarh Lead' "));
        //        builder.Append(string.Concat("from meterdata_loadsurvey where MeterData_ID = " + meterDataId + " and "));
        //        builder.Append(string.Concat("realTimeClockDateandTime >= '" + fromDate + "' and realTimeClockDateandTime <= '" + toDate));
        //        builder.Append(string.Concat("' group by convert(realTimeClockDateandTime,char(8))"));

        //        DataRequest request = new DataRequest(builder.ToString());
        //        dataSet = new DataSet();
        //        dataSet = helper.FillDataSet(request, dataSet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return dataSet;
        //}

        //public int GetScale(long id)
        //{
        //    int decimalCounter = 0;
        //    try
        //    {
        //        IDataHelper helper = DatabaseFactory.GetHelper();
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Select blockEnergykWh from meterdata_loadsurvey where ");
        //        builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
        //        DataRequest request = new DataRequest(builder.ToString());
        //        request.AddParamter(ParameterName(MeterData_ID), id, DbType.Int64);
        //        DataSet ds = new DataSet();
        //        ds = helper.FillDataSet(request, ds);
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                string tempVal = ds.Tables[0].Rows[0].ItemArray[0].ToString();
        //                if (tempVal.IndexOf('.') < 0)
        //                {decimalCounter=0; }
        //                else
        //                { decimalCounter = Convert.ToInt16(tempVal.IndexOf('*') - (tempVal.IndexOf('.') + 1)); }
        //            }
        //        }
        //        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
        //    }
        //    catch (CABException ex)    //Exception log for catch block
        //    {
        //        decimalCounter = 0;
        //    }
        //    return decimalCounter;
        //}

        public DataSet ListDataSet(long meterDataId, string columnsNames, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct realTimeClockDateandTime, ");
                builder.Append(columnsNames);
                builder.Append(" from meterdata_midnightdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(long meterDataId, string columnsNames, long fromDate, long toDate)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetMidNightData(long meterDataId)
        {
            DataSet dataSet = null;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            try
            {

                // Changed Cu to Daily to solve DLMS_110.
                builder.Append("Select DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)', ");
                builder.Append(string.Concat("cumEnergykWh as 'Cumulative Energy - kWh (1.0.1.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAh as 'Cumulative Energy - kVAh (1.0.9.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlag as 'Cumulative Energy - kvarh (lag) (1.0.5.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlead as 'Cumulative Energy - kvarh (lead) (1.0.8.8.0.255;3;2)' "));

                builder.Append(string.Concat("cumEnergykWhExport as 'Cumulative Energy - kWh Export (1.0.2.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAhExport as 'Cumulative Energy - kVAh Export (1.0.10.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlagQ3 as 'Cumulative Energy - kvarh (lag) Q3 (1.0.7.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhleadQ2 as 'Cumulative Energy - kvarh (lead) Q2 (1.0.6.8.0.255;3;2)' "));

                builder.Append(string.Concat("cumEnergykWhImport as 'Cumulative Energy - kWh Import (1.0.143.128.128.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykVAhImport as 'Cumulative Energy - kVAh Import (1.0.144.128.128.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhlagQ1 as 'Cumulative Energy - kvarh (lag) Q1 (1.0.145.128.128.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhleadQ4 as 'Cumulative Energy - kvarh (lead) Q4 (1.0.146.128.128.255;3;2)' "));

                builder.Append(string.Concat("cumEnergykWhRPhase as 'Cumulative Energy - kWh R Phase (1.0.21.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykWhYPhase as 'Cumulative Energy - kWh Y Phase (1.0.41.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykWhBPhase as 'Cumulative Energy - kWh B Phase (1.0.61.8.0.255;3;2)' "));

                builder.Append(string.Concat("cumEnergykvarhQ12 as 'Cumulative Energy kvarh (Q12) (1.0.3.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhQ34 as 'Cumulative Energy kvarh (Q34) (1.0.4.8.0.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhQ14 as 'Cumulative Energy kvarh (Q14) (1.0.153.128.128.255;3;2)', "));
                builder.Append(string.Concat("cumEnergykvarhQ23 as 'Cumulative Energy kvarh (Q23) (1.0.154.128.128.255;3;2)' "));
                builder.Append(string.Concat("fundamentalAbsolutekWH as 'Fundamental kWh Absolute   (1.0.128.8.1.255;3;2)' ")); 

                builder.Append(string.Concat("netkWh as 'Net - kWh', "));
                builder.Append(string.Concat("netkVAh as 'Net - kVAh', "));

                builder.Append(string.Concat("mDKW as 'Maximum Demand - kW (1.0.1.6.0.255;4;2)', "));
                builder.Append(string.Concat("mDKWDateTime as 'Maximum Demand – kW DateTime (1.0.1.6.0.255;4;5)', "));
                builder.Append(string.Concat("mDKVA as 'Maximum Demand - kVA (1.0.9.6.0.255;4;2)', "));
                builder.Append(string.Concat("mDKVADateTime as 'Maximum Demand – kVA DateTime (1.0.9.6.0.255;4;5)' "));

                #region JDVVNL  
        
            builder.Append(string.Concat("minVoltageLSIPAcrossDayRPhase as 'Minimum Voltage R Phase (1.0.32.51.128.255;3;2)' "));
            builder.Append(string.Concat("minVoltageLSIPAcrossDayYPhase as 'Minimum Voltage Y Phase (1.0.52.51.128.255;3;2)' "));
            builder.Append(string.Concat("minVoltageLSIPAcrossDayBPhase as 'Minimum Voltage B Phase (1.0.72.51.128.255;3;2)' "));
            builder.Append(string.Concat("highestCurrentLSIPAcrossDayRPhase as 'Highest Current R Phase (1.0.31.53.128.255;3;2)' "));
            builder.Append(string.Concat("highestCurrentLSIPAcrossDayYPhase as 'Highest Current Y Phase (1.0.51.53.128.255;3;2)' "));
            builder.Append(string.Concat("highestCurrentLSIPAcrossDayBPhase as 'Highest Current B Phase (1.0.71.53.128.255;3;2)' "));

             #endregion




                builder.Append(string.Concat("from meterdata_midnightdata where MeterData_ID = " + meterDataId ));
                builder.Append(string.Concat(" group by convert(realTimeClockDateandTime,char(8))"));
                builder.Append(string.Concat(" order by realTimeClockDateandTime"));

                //builder.Append(string.Concat(" order by convert(realTimeClockDateandTime,char(8)) desc"));

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetMidNightData(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet ListDataSetWithColumns(long meterDataId, long fromDate, long toDate, string midnightColumnParameters)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct ");
                builder.Append(midnightColumnParameters);
                builder.Append(" from meterdata_midnightdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by realTimeClockDateandTime desc"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetWithColumns(long meterDataId, long fromDate, long toDate, string midnightColumnParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// get dynamic mid night data
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        /// <sortType>use "asc" for ascending order and use "desc" for decending order </sortType>
        public DataSet GetGenericMidNightData(long meterDataID, string midnightColumnParameters , string sortType)
        {
            DataSet dataSet = null;
            try
            {
                midnightColumnParameters = UpdateMidNightColumnNames(midnightColumnParameters);
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select distinct ");
                builder.Append(midnightColumnParameters);
                builder.Append(" from meterdata_midnightdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" order by realTimeClockDateandTime"));
                if (sortType == "desc")
                {
                    builder.Append(string.Concat(" desc"));
                }
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
                ChangeColumnName(dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGenericMidNightData(long meterDataID, string midnightColumnParameters , string sortType)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        private void ChangeColumnName(DataSet dataSet)
        {
            for (int i = 0; i < dataSet.Tables[0].Columns.Count;i++)
            {

                switch (dataSet.Tables[0].Columns[i].ColumnName)
                {
                    //case "realTimeClockDateandTime":                        
                    //        dataSet.Tables[0].Columns[i].ColumnName = "Date (0.0.1.0.0.255;8;2)";
                    //        break;                        
                    //case "cumEnergykWh":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)";
                    //    break;
                    //case "cumEnergykVAh":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)";
                    //    break;
                    //case "cumEnergykvarhlag":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)";
                    //    break;
                    //case "cumEnergykvarhlead":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)";
                    //    break;
                    //case "mDKW":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Maximum Demand - {0}W (1.0.1.6.0.255;4;2)";
                    //    break;
                    //case "mDKWDateTime":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Maximum Demand - {0}W Date Time (1.0.1.6.0.255;4;5)";
                    //    break;
                    //case "mDKVA":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Maximum Demand - {0}VA (1.0.9.6.0.255;4;2)";
                    //    break;

                    //case "mDKVADateTime":
                    //    dataSet.Tables[0].Columns[i].ColumnName = "Maximum Demand - {0}VA Date Time (1.0.9.6.0.255;4;5)";
                    //    break;
                    // Name change for APSPDCL : Daily Survey Requirement
                    case "PowerOnDuration":
                        //dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm";// OBIS Code changed for APSPDCL : Daily Survey Requirement
                        dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.96.0.165.255;3;2) dddd:hh:mm";// OBIS Code changed for APSPDCL : Daily Survey Requirement  
                        break;
                    case "PowerOnDurationGeneric":
                        //dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm";// OBIS Code added for JVVNL : Daily Survey Requirement
                        dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm";// OBIS Code added for JVVNL : Daily Survey Requirement
                        break;
                    case "PowerOnDurationGeneric1P":
                        //dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm";// OBIS Code added for JVVNL : Daily Survey Requirement
                        dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm";// OBIS Code added for JVVNL : Daily Survey Requirement
                        break;
                    case "PowerFailureDuration":
                        //dataSet.Tables[0].Columns[i].ColumnName = "Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm";
                        dataSet.Tables[0].Columns[i].ColumnName = "Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm";
                        break;
                    case "PowerOnDurationThreePhases":
                        //dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm";
                        dataSet.Tables[0].Columns[i].ColumnName = "Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm";
                        break;
                }

                
              
            }
        }

        private string GetMappedValue(string Value)
        {
            Dictionary<string, string> dicMatchString = new Dictionary<string, string>();
            dicMatchString.Add("realTimeClockDateandTime", "DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)'");
            dicMatchString.Add("cumEnergykWh", "cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykVAh", "cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhlag", "cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhlead", "cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykWhExport", "cumEnergykWhExport as 'Cumulative Energy - {0}Wh Export (1.0.2.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykVAhExport", "cumEnergykVAhExport as 'Cumulative Energy - {0}VAh Export (1.0.10.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhlagQ3", "cumEnergykvarhlagQ3 as 'Cumulative Energy - {0}varh (lag) Q3 (1.0.7.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhleadQ2", "cumEnergykvarhleadQ2 as 'Cumulative Energy - {0}varh (lead) Q2 (1.0.6.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykWhImport", "cumEnergykWhImport as 'Cumulative Energy - {0}Wh Forward (1.0.143.128.128.255;3;2)'");
            dicMatchString.Add("cumEnergykVAhImport", "cumEnergykVAhImport as 'Cumulative Energy - {0}VAh Forward (1.0.144.128.128.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhlagQ1", "cumEnergykvarhlagQ1 as 'Cumulative Energy - {0}varh (lag) Q1 (1.0.145.128.128.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhleadQ4", "cumEnergykvarhleadQ4 as 'Cumulative Energy - {0}varh (lead) Q4 (1.0.146.128.128.255;3;2)'");

            dicMatchString.Add("cumEnergykWhRPhase", "cumEnergykWhRPhase as 'Cumulative Energy - {0}Wh R Phase (1.0.21.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykWhYPhase", "cumEnergykWhYPhase as 'Cumulative Energy - {0}Wh Y Phase (1.0.41.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykWhBPhase", "cumEnergykWhBPhase as 'Cumulative Energy - {0}Wh B Phase (1.0.61.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhQ12", "cumEnergykvarhQ12 as 'Cumulative Energy - {0}varh (Q12) (1.0.3.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhQ34", "cumEnergykvarhQ34 as 'Cumulative Energy - {0}varh (Q34) (1.0.4.8.0.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhQ14", "cumEnergykvarhQ14 as 'Cumulative Energy - {0}varh (Q14) (1.0.153.128.128.255;3;2)'");
            dicMatchString.Add("cumEnergykvarhQ23", "cumEnergykvarhQ23 as 'Cumulative Energy - {0}varh (Q23) (1.0.154.128.128.255;3;2)'");
            dicMatchString.Add("fundamentalAbsolutekWH", "fundamentalAbsolutekWH as 'Fundamental {0}Wh Absolute (1.0.128.8.1.255;3;2)'"); 

            dicMatchString.Add("mDKW", "mDKW as 'Maximum Demand - {0}W (1.0.1.6.0.255;4;2)' , DATE_FORMAT(mDKWDateTime,'%d/%m/%Y %H:%i') as 'Maximum Demand - {0}W Date Time (1.0.1.6.0.255;4;5)'");
            dicMatchString.Add("mDKVA", "mDKVA as 'Maximum Demand - {0}VA (1.0.9.6.0.255;4;2)' ,DATE_FORMAT(mDKVADateTime,'%d/%m/%Y %H:%i') as 'Maximum Demand - {0}VA Date Time (1.0.9.6.0.255;4;5)'");

            dicMatchString.Add("netkWh", "case when netkWh is null then 0 end as 'Net - {0}Wh'");
            dicMatchString.Add("netkVAh", "case when netkVAh is null then 0 end as 'Net - {0}VAh'");

            #region JDVVNL

            dicMatchString.Add("minVoltageLSIPAcrossDayRPhase", "minVoltageLSIPAcrossDayRPhase as 'Minimum Voltage R Phase (1.0.32.51.128.255;3;2)'");
            dicMatchString.Add("minVoltageLSIPAcrossDayYPhase", "minVoltageLSIPAcrossDayYPhase as 'Minimum Voltage Y Phase (1.0.52.51.128.255;3;2)'");
            dicMatchString.Add("minVoltageLSIPAcrossDayBPhase", "minVoltageLSIPAcrossDayBPhase as 'Minimum Voltage B Phase  (1.0.72.51.128.255;3;2)'");
            dicMatchString.Add("highestCurrentLSIPAcrossDayRPhase", "highestCurrentLSIPAcrossDayRPhase as 'Highest Current R Phase (1.0.31.53.128.255;3;2)'");
            dicMatchString.Add("highestCurrentLSIPAcrossDayYPhase", "highestCurrentLSIPAcrossDayYPhase as 'Highest Current Y Phase (1.0.51.53.128.255;3;2)'");
            dicMatchString.Add("highestCurrentLSIPAcrossDayBPhase", "highestCurrentLSIPAcrossDayBPhase as 'Highest Current B Phase (1.0.71.53.128.255;3;2)'");

            if (ConfigSettings.GetValue("ChkPowerOnOffDurationFormat") == "1")
            {
                dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.94.91.13.255;3;2) dddd:hh'");
                dicMatchString.Add("PowerOffDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (0.0.96.1.217.255;3;2) dddd:hh'");

                //dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.96.0.165.255;3;2) dddd:hh'");
                //dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.96.0.164.255;3;2) dddd:hh'");               
            }
            else
            {
                //dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.94.91.13.255;3;2) dd:hh:mm'");
                dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.94.91.13.255;3;2) dddd:hh:mm'");
                //dicMatchString.Add("PoweroffDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (0.0.96.1.217.255;3;2) dd:hh:mm'");

                //dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.96.0.165.255;3;2) dd:hh:mm'");
                //dicMatchString.Add("PowerOnDurationGeneric", "PowerOnDurationGeneric as 'Power On Duration (1.0.96.0.164.255;3;2) dd:hh:mm'");  
            }
            //dicMatchString.Add("PowerFailureDuration", "PowerFailureDuration as 'Power Off Duration (0.0.96.1.217.255;3;2) dd:hh:mm'");
            //dicMatchString.Add("PowerOnDurationThreePhases", "PowerOnDurationThreePhases as 'Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dd:hh:mm'");

            //dicMatchString.Add("PowerOnDurationGeneric1P", "PowerOnDurationGeneric1P as 'Power On Duration (1.0.94.91.14.255;3;2) dd:hh:mm'");
            dicMatchString.Add("PowerFailureDuration", "PowerFailureDuration as 'Power Off Duration (0.0.96.1.217.255;3;2) dddd:hh:mm'");
            dicMatchString.Add("PowerOnDurationThreePhases", "PowerOnDurationThreePhases as 'Power On Duration 1 or 2  Phases (1.0.96.0.164.255;3;2) dddd:hh:mm'");

            dicMatchString.Add("PowerOnDurationGeneric1P", "PowerOnDurationGeneric1P as 'Power On Duration (1.0.94.91.14.255;3;2) dddd:hh:mm'");
            #endregion

            if (dicMatchString.ContainsKey(Value))
            {
                return dicMatchString[Value];
            }
            else
            {
                return string.Empty;
            }
        }



        /// <summary>
        /// update the query to replace 
        /// </summary>
        /// <param name="midNightColumns"></param>
        /// <returns></returns>
        private string UpdateMidNightColumnNames(string midNightColumns)
        {
            string convertedColumns = "*";
            try
            {               
                string[] lstColumnNames = midNightColumns.Split(',');
                if (lstColumnNames != null)
                {
                    convertedColumns = "";
                    for (int i = 0; i < lstColumnNames.Length; i++)
                    {
                        string MappedValue = GetMappedValue(lstColumnNames[i]);
                        if (MappedValue != string.Empty)
                        {
                            convertedColumns += MappedValue;
                            if (i != (lstColumnNames.Length - 1))
                            {
                                convertedColumns += ",";
                            }
                        }
                    }
                }

                //midNightColumns = midNightColumns.Replace("realTimeClockDateandTime", "DATE_FORMAT(DATE(CONVERT(realTimeClockDateandTime,char(8))),'%d/%m/%Y') as 'Date (0.0.1.0.0.255;8;2)'");
                //midNightColumns = midNightColumns.Replace("cumEnergykWh", "cumEnergykWh as 'Cumulative Energy - {0}Wh (1.0.1.8.0.255;3;2)'");
                //midNightColumns = midNightColumns.Replace("cumEnergykVAh", "cumEnergykVAh as 'Cumulative Energy - {0}VAh (1.0.9.8.0.255;3;2)'");
                //midNightColumns = midNightColumns.Replace("cumEnergykvarhlag", "cumEnergykvarhlag as 'Cumulative Energy - {0}varh (lag) (1.0.5.8.0.255;3;2)'");
                //midNightColumns = midNightColumns.Replace("cumEnergykvarhlead", "cumEnergykvarhlead as 'Cumulative Energy - {0}varh (lead) (1.0.8.8.0.255;3;2)'");
                //midNightColumns = midNightColumns.Replace("mDKW", "mDKW as 'Maximum Demand - {0}W (1.0.1.6.0.255;4;2)' , DATE_FORMAT(mDKWDateTime,'%d/%m/%Y %H:%i') as 'Maximum Demand - {0}W Date Time (1.0.1.6.0.255;4;5)'");
                //midNightColumns = midNightColumns.Replace("mDKVA", "mDKVA as 'Maximum Demand - {0}VA (1.0.9.6.0.255;4;2)' ,DATE_FORMAT(mDKVADateTime,'%d/%m/%Y %H:%i') as 'Maximum Demand - {0}VA Date Time (1.0.9.6.0.255;4;5)'");

                //convertedColumns = midNightColumns;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateMidNightColumnNames(string midNightColumns)", ex);
            }
            return convertedColumns;
        }
        ////public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        ////{
        ////    DataSet dataSet = null;
        ////    try
        ////    {
        ////        IDataHelper helper = DatabaseFactory.GetHelper();
        ////        StringBuilder builder = new StringBuilder();
        ////        builder.Append("Select * ");
        ////        builder.Append(" from meterdata_loadsurvey where ");
        ////        builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
        ////        builder.Append(string.Concat(realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
        ////        builder.Append(string.Concat(realTimeClockDateandTime, "<=", ParameterName("ToDate")));
        ////        builder.Append(string.Concat(" order by realTimeClockDateandTime"));
        ////        DataRequest request = new DataRequest(builder.ToString());
        ////        request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
        ////        request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
        ////        request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
        ////        dataSet = new DataSet();
        ////        dataSet = helper.FillDataSet(request, dataSet);
        ////        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));
        ////    }
        ////    catch (CABException ex)    //Exception log for catch block
        ////    {
        ////        dataSet = null;
        ////    }
        ////    return dataSet;
        ////}

        private DataRequest GetRequest(IEntity entity)
        {

            if (entity == null)
                return null;
            DLMS650MidnightDataEntity midnightDataEntity = entity as DLMS650MidnightDataEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_midnightdata(realTimeClockDateandTime,cumEnergykWh,cumEnergykvarhlag,cumEnergykvarhlead,cumEnergykVAh,cumEnergykWhExport,cumEnergykvarhlagQ3,cumEnergykvarhleadQ2,cumEnergykVAhExport,cumEnergykWhImport,cumEnergykvarhlagQ1,cumEnergykvarhleadQ4,cumEnergykVAhImport,cumEnergykWhRPhase,cumEnergykWhYPhase,cumEnergykWhBPhase,cumEnergykvarhQ12,cumEnergykvarhQ34,cumEnergykvarhQ14,cumEnergykvarhQ23,fundamentalAbsolutekWH,netkWh,netkVAh,minVoltageLSIPAcrossDayRPhase,minVoltageLSIPAcrossDayYPhase,minVoltageLSIPAcrossDayBPhase,highestCurrentLSIPAcrossDayRPhase,highestCurrentLSIPAcrossDayYPhase,highestCurrentLSIPAcrossDayBPhase,mDKW,mDKWDateTime,mDKVA,mDKVADateTime,MeterData_ID,PowerOnDuration,PowerFailureDuration,PowerOnDurationThreePhases,PowerOnDurationGeneric,PowerOnDurationGeneric1P) values(");    

            builder.Append(string.Concat(ParameterName(realTimeClockDateandTime), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykWh), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhlag), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhlead), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykVAh), ","));

            builder.Append(string.Concat(ParameterName(cumEnergykWhExport), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhlagQ3), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhleadQ2), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykVAhExport), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykWhImport), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhlagQ1), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhleadQ4), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykVAhImport), ","));

            builder.Append(string.Concat(ParameterName(cumEnergykWhRPhase), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykWhYPhase), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykWhBPhase), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhQ12), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhQ34), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhQ14), ","));
            builder.Append(string.Concat(ParameterName(cumEnergykvarhQ23), ","));
            builder.Append(string.Concat(ParameterName(fundamentalAbsolutekWH), ","));

            builder.Append(string.Concat(ParameterName(netkWh), ","));
            builder.Append(string.Concat(ParameterName(netkVAh), ","));

            builder.Append(string.Concat(ParameterName(minVoltageLSIPAcrossDayRPhase), ","));
            builder.Append(string.Concat(ParameterName(minVoltageLSIPAcrossDayYPhase), ","));
            builder.Append(string.Concat(ParameterName(minVoltageLSIPAcrossDayBPhase), ","));
            builder.Append(string.Concat(ParameterName(highestCurrentLSIPAcrossDayRPhase), ","));
            builder.Append(string.Concat(ParameterName(highestCurrentLSIPAcrossDayYPhase), ","));
            builder.Append(string.Concat(ParameterName(highestCurrentLSIPAcrossDayBPhase), ","));

            builder.Append(string.Concat(ParameterName(mDKW), ","));
            builder.Append(string.Concat(ParameterName(mDKWDateTime), ","));
            builder.Append(string.Concat(ParameterName(mDKVA), ","));
            builder.Append(string.Concat(ParameterName(mDKVADateTime), ","));

            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(PowerOnDuration), ","));
            builder.Append(string.Concat(ParameterName(PowerFailureDuration), ","));
            builder.Append(string.Concat(ParameterName(PowerOnDurationThreePhases), ","));
            builder.Append(string.Concat(ParameterName(PowerOnDurationGeneric), ","));
            builder.Append(ParameterName(PowerOnDurationGeneric1P));
            builder.Append(")");
      
            DataRequest request = new DataRequest(builder.ToString());
            // Story - 354382 - For a day Time 240000 is coming which would be treated as 000000 for next day for Single phase meter
            if (!string.IsNullOrEmpty(midnightDataEntity.RealTimeClockDateandTime.ToString()))
            {
                if (midnightDataEntity.RealTimeClockDateandTime.ToString().Substring(8, 2) == "24" && midnightDataEntity.RealTimeClockDateandTime.ToString().Substring(10, 2) == "00" && midnightDataEntity.RealTimeClockDateandTime.ToString().Substring(12, 2) == "00")
                {
                    DateTime realTimeClockDateTime = DateUtility.LongToDateTime(Convert.ToInt64(midnightDataEntity.RealTimeClockDateandTime.ToString()));
                    midnightDataEntity.RealTimeClockDateandTime = DateUtility.DateTimeToLong(realTimeClockDateTime);
                }
            }
            request.AddParamter(ParameterName(realTimeClockDateandTime), midnightDataEntity.RealTimeClockDateandTime, DbType.Int64);
            request.AddParamter(ParameterName(cumEnergykWh), midnightDataEntity.CumEnergykWh, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhlag), midnightDataEntity.CumEnergykvarhlag, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhlead), midnightDataEntity.CumEnergykvarhlead, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykVAh), midnightDataEntity.CumEnergykVAh , DbType.String, 40);
            request.AddParamter(ParameterName(mDKW), midnightDataEntity.MDKW, DbType.String, 40);
            request.AddParamter(ParameterName(mDKWDateTime), midnightDataEntity.MDKWDateTime, DbType.String, 40);
            request.AddParamter(ParameterName(mDKVA), midnightDataEntity.MDKVA, DbType.String, 40);
            request.AddParamter(ParameterName(mDKVADateTime), midnightDataEntity.MDKVADateTime, DbType.String, 40);
            request.AddParamter(ParameterName(MeterData_ID), midnightDataEntity.MeterData_ID, DbType.Int64);
   
            request.AddParamter(ParameterName(PowerOnDuration), midnightDataEntity.PowerOnDuration, DbType.String, 40);
            request.AddParamter(ParameterName(PowerFailureDuration), midnightDataEntity.PowerFailureDuration, DbType.String, 40);
            request.AddParamter(ParameterName(PowerOnDurationThreePhases), midnightDataEntity.PowerOnDurationThreePhases, DbType.String, 40);
            request.AddParamter(ParameterName(PowerOnDurationGeneric), midnightDataEntity.PowerOnDurationGeneric, DbType.String, 40);
            request.AddParamter(ParameterName(PowerOnDurationGeneric1P), midnightDataEntity.PowerOnDurationGeneric1P, DbType.String, 40);

            request.AddParamter(ParameterName(cumEnergykWhExport), midnightDataEntity.CumEnergykWhExport, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhlagQ3), midnightDataEntity.CumEnergykvarhlagQ3, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhleadQ2), midnightDataEntity.CumEnergykvarhleadQ2, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykVAhExport), midnightDataEntity.CumEnergykVAhExport, DbType.String, 40);

            request.AddParamter(ParameterName(cumEnergykWhImport), midnightDataEntity.CumEnergykWhImport, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhlagQ1), midnightDataEntity.CumEnergykvarhlagQ1, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhleadQ4), midnightDataEntity.CumEnergykvarhleadQ4, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykVAhImport), midnightDataEntity.CumEnergykVAhImport, DbType.String, 40);

            request.AddParamter(ParameterName(cumEnergykWhRPhase), midnightDataEntity.CumEnergykWhRPhase, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykWhYPhase), midnightDataEntity.CumEnergykWhYPhase, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykWhBPhase), midnightDataEntity.CumEnergykWhBPhase, DbType.String, 40);

            request.AddParamter(ParameterName(cumEnergykvarhQ12), midnightDataEntity.CumEnergykvarhQ12, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhQ34), midnightDataEntity.CumEnergykvarhQ34, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhQ14), midnightDataEntity.CumEnergykvarhQ14, DbType.String, 40);
            request.AddParamter(ParameterName(cumEnergykvarhQ23), midnightDataEntity.CumEnergykvarhQ23, DbType.String, 40);
            request.AddParamter(ParameterName(fundamentalAbsolutekWH), midnightDataEntity.FundamentalAbsolutekWH, DbType.String, 40); 

            request.AddParamter(ParameterName(netkWh), midnightDataEntity.NetkWh, DbType.String, 40);
            request.AddParamter(ParameterName(netkVAh), midnightDataEntity.NetkVAh, DbType.String, 40);            

            #region JDVVNL
            request.AddParamter(ParameterName(minVoltageLSIPAcrossDayRPhase), midnightDataEntity.MinVoltageLSIPAcrossDayRPhase, DbType.String, 40);
            request.AddParamter(ParameterName(minVoltageLSIPAcrossDayYPhase), midnightDataEntity.MinVoltageLSIPAcrossDayYPhase, DbType.String, 40);
            request.AddParamter(ParameterName(minVoltageLSIPAcrossDayBPhase), midnightDataEntity.MinVoltageLSIPAcrossDayBPhase, DbType.String, 40);
            request.AddParamter(ParameterName(highestCurrentLSIPAcrossDayRPhase), midnightDataEntity.HighestCurrentLSIPAcrossDayRPhase, DbType.String, 40);
            request.AddParamter(ParameterName(highestCurrentLSIPAcrossDayYPhase), midnightDataEntity.HighestCurrentLSIPAcrossDayYPhase, DbType.String, 40);
            request.AddParamter(ParameterName(highestCurrentLSIPAcrossDayBPhase), midnightDataEntity.HighestCurrentLSIPAcrossDayBPhase, DbType.String, 40);

            #endregion
            return request;
        }



        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DLMS650MidnightDataEntity midnightDataEntity = new DLMS650MidnightDataEntity();
            if (NotNullAndNotDBNull(row, MidnightData_ID)) midnightDataEntity.MidnightData_ID = Convert.ToInt64(row[MidnightData_ID]);
            if (NotNullAndNotDBNull(row, realTimeClockDateandTime)) midnightDataEntity.RealTimeClockDateandTime = Convert.ToInt64(row[realTimeClockDateandTime]);
            if (NotNullAndNotDBNull(row, cumEnergykWh)) midnightDataEntity.CumEnergykWh = Convert.ToString(row[cumEnergykWh]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhlag)) midnightDataEntity.CumEnergykvarhlag = Convert.ToString(row[cumEnergykvarhlag]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhlead)) midnightDataEntity.CumEnergykvarhlead = Convert.ToString(row[cumEnergykvarhlead]);
            if (NotNullAndNotDBNull(row, cumEnergykVAh)) midnightDataEntity.CumEnergykVAh = Convert.ToString(row[cumEnergykVAh]);

            if (NotNullAndNotDBNull(row, cumEnergykWhExport)) midnightDataEntity.CumEnergykWhExport = Convert.ToString(row[cumEnergykWhExport]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhlagQ3)) midnightDataEntity.CumEnergykvarhlagQ3 = Convert.ToString(row[cumEnergykvarhlagQ3]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhleadQ2)) midnightDataEntity.CumEnergykvarhleadQ2 = Convert.ToString(row[cumEnergykvarhleadQ2]);
            if (NotNullAndNotDBNull(row, cumEnergykVAhExport)) midnightDataEntity.CumEnergykVAhExport = Convert.ToString(row[cumEnergykVAhExport]);
            if (NotNullAndNotDBNull(row, cumEnergykWhImport)) midnightDataEntity.CumEnergykWhImport = Convert.ToString(row[cumEnergykWhImport]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhlagQ1)) midnightDataEntity.CumEnergykvarhlagQ1 = Convert.ToString(row[cumEnergykvarhlagQ1]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhleadQ4)) midnightDataEntity.CumEnergykvarhleadQ4 = Convert.ToString(row[cumEnergykvarhleadQ4]);
            if (NotNullAndNotDBNull(row, cumEnergykVAhImport)) midnightDataEntity.CumEnergykVAhImport = Convert.ToString(row[cumEnergykVAhImport]);

            if (NotNullAndNotDBNull(row, cumEnergykWhRPhase)) midnightDataEntity.CumEnergykWhRPhase = Convert.ToString(row[cumEnergykWhRPhase]);
            if (NotNullAndNotDBNull(row, cumEnergykWhYPhase)) midnightDataEntity.CumEnergykWhYPhase = Convert.ToString(row[cumEnergykWhYPhase]);
            if (NotNullAndNotDBNull(row, cumEnergykWhBPhase)) midnightDataEntity.CumEnergykWhBPhase = Convert.ToString(row[cumEnergykWhBPhase]);

            if (NotNullAndNotDBNull(row, cumEnergykvarhQ12)) midnightDataEntity.CumEnergykvarhQ12 = Convert.ToString(row[cumEnergykvarhQ12]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhQ34)) midnightDataEntity.CumEnergykvarhQ34 = Convert.ToString(row[cumEnergykvarhQ34]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhQ14)) midnightDataEntity.CumEnergykvarhQ14 = Convert.ToString(row[cumEnergykvarhQ14]);
            if (NotNullAndNotDBNull(row, cumEnergykvarhQ23)) midnightDataEntity.CumEnergykvarhQ23 = Convert.ToString(row[cumEnergykvarhQ23]);

            if (NotNullAndNotDBNull(row, netkWh)) midnightDataEntity.NetkWh = Convert.ToString(row[netkWh]);
            if (NotNullAndNotDBNull(row, netkVAh)) midnightDataEntity.NetkVAh = Convert.ToString(row[netkVAh]);

            if (NotNullAndNotDBNull(row, minVoltageLSIPAcrossDayRPhase)) midnightDataEntity.MinVoltageLSIPAcrossDayRPhase = Convert.ToString(row[minVoltageLSIPAcrossDayRPhase]);
            if (NotNullAndNotDBNull(row, minVoltageLSIPAcrossDayYPhase)) midnightDataEntity.MinVoltageLSIPAcrossDayYPhase = Convert.ToString(row[minVoltageLSIPAcrossDayYPhase]);
            if (NotNullAndNotDBNull(row, minVoltageLSIPAcrossDayBPhase)) midnightDataEntity.MinVoltageLSIPAcrossDayBPhase = Convert.ToString(row[minVoltageLSIPAcrossDayBPhase]);
            if (NotNullAndNotDBNull(row, highestCurrentLSIPAcrossDayRPhase)) midnightDataEntity.HighestCurrentLSIPAcrossDayRPhase = Convert.ToString(row[highestCurrentLSIPAcrossDayRPhase]);
            if (NotNullAndNotDBNull(row, highestCurrentLSIPAcrossDayYPhase)) midnightDataEntity.HighestCurrentLSIPAcrossDayYPhase = Convert.ToString(row[highestCurrentLSIPAcrossDayYPhase]);
            if (NotNullAndNotDBNull(row, highestCurrentLSIPAcrossDayBPhase)) midnightDataEntity.HighestCurrentLSIPAcrossDayBPhase = Convert.ToString(row[highestCurrentLSIPAcrossDayBPhase]);

            if (NotNullAndNotDBNull(row, MeterData_ID)) midnightDataEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            return midnightDataEntity;
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

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        //public bool DeleteData(long meterDataID)
        //{
        //    bool Flag = false;
        //    try
        //    {
        //        IDataHelper helper = DatabaseFactory.GetHelper();
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Delete from meterdata_loadsurvey where ");
        //        builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
        //        DataRequest request = new DataRequest(builder.ToString());
        //        request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
        //        helper.ExecuteNonQuery(request);
        //        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
        //        Flag = true;
        //    }
        //    catch (Exception) { }
        //    return Flag;
        //}
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
                builder.Append("Select Max(realTimeClockDateandTime)  from meterdata_midnightdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                    meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetToDate(long meterDataID)", ex);
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
                builder.Append("Select Min(realTimeClockDateandTime)  from meterdata_midnightdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    meterid = 0;
                else
                    meterid = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFromDate(long meterDataID)", ex);
                meterid = 0;
            }
            return meterid;
        }

        public DataSet GetMidnightEnergy(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterData_ID,m.MeterID, f.FileName, m.ReadingDateTime, md.realTimeClockDateandTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "md.", column, " "));
                }
                builder.Append("from meterdata_midnightdata md inner join meterdata m on md.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight Energies viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidnightEnergy(string meterID, List<string> columns)", ex);
            }
            return dataSet;
        }

        public DataSet GetMidnightEnergiesByFileName(string meterID, string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime, md.realTimeClockDateandTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "md.", column, " "));
                }
                builder.Append(",m.MeterData_ID from meterdata_midnightdata md inner join meterdata m on md.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight Energies viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidnightEnergiesByFileName(string meterID, string fileName, List<string> columns)", ex);
            }
            return dataSet;
        }
        /// <summary>
        /// Deletes data of meterdata_midnightdata table
        /// </summary>
        /// <param name="meterid"></param>
        public void DeleteData(long meterDataID)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from meterdata_midnightdata where MeterData_ID= @meterDataID");
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName("@meterDataID"), meterDataID, DbType.Int64);
            helper.ExecuteNonQuery(request);
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
                builder.Append(")from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_midnightdata` middata ");
                builder.Append("on mdata.meterData_ID = middata.meterData_ID and mdata. ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    leastDate = 0;
                else
                    leastDate = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));

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
                builder.Append(")from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_midnightdata` middata ");
                builder.Append("on mdata.meterData_ID = middata.meterData_ID and mdata. ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    maxDate = 0;
                else
                    maxDate = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMaxDateForMeterID(string meterID)", ex);
                maxDate = 0;

            }
            return maxDate;
        }

        /// <summary>
        /// Lists DataSet With Columns of load survey For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="lsColumnParameters"></param>
        /// <returns></returns>
        public DataSet ListDataSetWithColumnsForMeterID(string meterID, long fromDate, long toDate, string midnightColumnParameters)
        {
            string strAdder = midnightColumnParameters;
            string[] strArray = strAdder.Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = "middata." + strArray[i];
            }
            midnightColumnParameters = String.Join(",", strArray);
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // Added PUMa condition

                builder.Append("Select distinct ");
                builder.Append(midnightColumnParameters);
                builder.Append(" from `dlms_ltct_650`.`meterdata`  mdata inner join  `dlms_ltct_650`.`meterdata_midnightdata` middata ");
                builder.Append("on mdata.meterData_ID = middata.meterData_ID and mdata.");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " and "));
                builder.Append(string.Concat("middata.", realTimeClockDateandTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat("middata.", realTimeClockDateandTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by middata.realTimeClockDateandTime desc"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Midnight data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetWithColumnsForMeterID(string meterID, long fromDate, long toDate, string midnightColumnParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public string ConvertTimSpanToDDHHMM(TimeSpan timeSpan)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (timeSpan.Days.ToString().Contains("-"))
                strBuilder.Append("0");
            else
                strBuilder.Append(timeSpan.Days);
            strBuilder.Append(" : ");
            if (timeSpan.Hours.ToString().Contains("-"))
                strBuilder.Append("00");
            else
                strBuilder.Append(timeSpan.Hours.ToString("00"));
            strBuilder.Append(" : ");
            if (timeSpan.Minutes.ToString().Contains("-"))
                strBuilder.Append("00");
            else
                strBuilder.Append(timeSpan.Minutes.ToString("00"));
            return strBuilder.ToString();

        }

    }
}
