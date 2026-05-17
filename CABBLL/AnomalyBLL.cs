using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using System.Data;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											BLL calss for anamoly data  																						|
 * |											Author : Vidya BHooshan Mishra       									|
 * |											Date   : 18-dec-2012											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

namespace CAB.BLL
{
    public class AnomalyBLL:IBLL
    {
        Dictionary<string, string> anomalyDataColumns = new Dictionary<string, string>();
        private AnomalyDAL objAnomalyDAL = null;
        public string fileName = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AnomalyBLL).ToString());
        public AnomalyBLL()
        {
            objAnomalyDAL = new AnomalyDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return objAnomalyDAL.InsertData(entity, true);
        }
        public IEntity GetDetailData(int meterDataId)
        {
            return objAnomalyDAL.GetDetailData(meterDataId);
        }

        public DataSet GetAnomalyDataForAnalysisDetail(int meterDataId)
        {           
            DataSet anomalyData = GetAnomalyTranspose(objAnomalyDAL.GetAnomalyDataForAnalysisDetail(meterDataId));            
            return anomalyData;
        }
       
        /// <summary>
        /// Used to convert Columns to rows
        /// </summary>
        /// <returns></returns>
        private DataSet GetAnomalyTranspose(DataSet anomalyData)
        {
            DataSet outputDataSet = new DataSet();
            if (anomalyData != null && anomalyData.Tables != null && anomalyData.Tables.Count > 0 && anomalyData.Tables[0].Rows.Count > 0)
            {                
                DataTable outputTable = new DataTable();
                DataTable inputTable = anomalyData.Tables[0];
                try
                {

                    inputTable.Columns.Remove("AnomalyId");
                    inputTable.Columns.Remove("MeterDataId");
                    // Add Header Column
                    inputTable.Columns.Add("Parameters", typeof(string)).SetOrdinal(0);
                    inputTable.Rows[0]["Parameters"] = "Status";

                    // Header row's first column is same as in inputTable
                    outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

                    // Header row's second column onwards, 'inputTable's first column taken
                    foreach (DataRow inRow in inputTable.Rows)
                    {
                        string newColName = inRow[0].ToString();
                        outputTable.Columns.Add(newColName);
                    }

                    // Add rows by looping columns        
                    for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
                    {
                        DataRow newRow = outputTable.NewRow();

                        // First column is inputTable's Header row's second column
                        newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                        for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                        {
                            string colValue = inputTable.Rows[cCount][rCount].ToString();
                            newRow[cCount + 1] = colValue;
                        }
                        outputTable.Rows.Add(newRow);
                    }
                    outputDataSet.Tables.Add(outputTable);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "GetAnomalyTranspose(DataSet anomalyData)", ex);
                    return null;
                }
                
            }
            return outputDataSet;


        }
        
        /// <summary>
        /// returns databse column names corresponding to supplied parameter names
        /// </summary>
        /// <param name="columnList"></param>
        /// <returns></returns>
        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateAnomalyDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (anomalyDataColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
        /// <summary>
        /// Maps database column names and column names displayed as parameters for user to select 
        /// in Self Diagnosis reports
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> CreateAnomalyDictionary()
        {
            //generalDataColumns.Add("Meter Serial Number", "meterSerialNumber");
            anomalyDataColumns.Add("FLASH", "FlashStatus");
            anomalyDataColumns.Add("EEPROM", "EepRamStatus");
            anomalyDataColumns.Add("POWER SUPPLY", "SmpsStatus");
            anomalyDataColumns.Add("RTC", "RTcStatus");
            anomalyDataColumns.Add("Error_Code", "ErrorCodeStatus");
            return anomalyDataColumns;
        }
        /// <summary>
        /// Retrives value of specified parameters from meterdata_anomaly table
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnList"></param>
        /// <returns></returns>
        public DataSet GetAnomalyDataByParameter(string value, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (fileName.Length > 0)
            {
                ds = objAnomalyDAL.GetAnomalyDataByFileName(value,fileName,columns);
            }
            else
            {
                ds = objAnomalyDAL.GetAnomalyDataByMeter(value, columns);
            }
            return ds;
        }

        /// <summary>
        /// Deletes data from Anomlay table
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public bool DeleteData(long meterDataId)
        {
            return objAnomalyDAL.DeleteData(meterDataId);
        }

    }
}
