
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
using System;
using Utilities;
using CAB.Framework.Utility;

namespace CAB.BLL
{
    public class DLMS650TamperMasterBLL : IBLL
    {
        DLMS650TamperMasterDAL dLMS650TamperMasterDAL;
        Dictionary<string, string> tamperColumns = new Dictionary<string, string>();
        DLMS650CommonBLL common;
        DLMS650GeneralBLL dlms650General;
        public Dictionary<int, string> tamperCount = new Dictionary<int, string>();
        DLMS650InstantaneousBLL dLMS650InstantaneousBLL;
        
        public DLMS650TamperMasterBLL()
        {
            dLMS650TamperMasterDAL = new DLMS650TamperMasterDAL();
            common = new DLMS650CommonBLL();
            dlms650General = new DLMS650GeneralBLL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return dLMS650TamperMasterDAL.InsertData(entity);
        }
        public IEntity InsertData(IList<IEntity> entities)
        {
            return dLMS650TamperMasterDAL.InsertData(entities);
        }
        public DataSet TamperOccurRestore(string eventCode, string meterDataID)
        {
            return common.ConvertTamperOccurRestore(eventCode, meterDataID);
        }
        public DataSet ListData(string eventCode, string meterDataID)
        {
            return dLMS650TamperMasterDAL.ListDataSet(eventCode, meterDataID);
        }
        public DataSet ListEventCodeORData(string eventCode1, string eventCode2, string meterDataID)
        {
            return dLMS650TamperMasterDAL.ListEventCodeORData(eventCode1, eventCode2,meterDataID);
        }
        public DataSet ListAllTamperEventCode()
        {
            return dLMS650TamperMasterDAL.ListAllTamperEventCode();
        }
        public DataSet DetailData(long tamperId, long meterDataID)
        {
            return dLMS650TamperMasterDAL.DetailData(tamperId, meterDataID);
        }

        public DataSet DetailDataColumnWise(long tamperId, long meterDataId, string tamperColumnParameters)
        {
            return dLMS650TamperMasterDAL.ListDataSetWithColumns(tamperId, meterDataId, tamperColumnParameters);
        }

        public DataSet DetailData(long meterDataID)
        {
            return dLMS650TamperMasterDAL.DetailData(meterDataID);
        }
        public DataSet DetailTransactionData(long meterDataID)
        {
            return dLMS650TamperMasterDAL.DetailTransactionData(meterDataID);
        }
        /* VBM Added for New tamper Report */
        public DataSet GetTamperStartEndDate(long meterDataID)
        {
            return dLMS650TamperMasterDAL.GetTamperStartEndDate(meterDataID);
        }

        /// <summary>
        /// This method give the Detail of Tamper added
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetTamperCountDetail(long meterDataID)
        {
            DataSet dataSet = dLMS650TamperMasterDAL.GetTampersAndCount(meterDataID);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataSet = ProcessTamperDataSet(dataSet);
            }           

            //Make dictionary of obiscode and event code of tamper counter
            GetObisCodes();

            //Get tamper count from Instantaneous            
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                string obisCode = string.Empty;
                dLMS650InstantaneousBLL = new DLMS650InstantaneousBLL();

                if (tamperCount.ContainsKey(Convert.ToInt32(dr["TamperId"])))
                {

                    obisCode = tamperCount[Convert.ToInt32(dr["TamperId"])];
                }

                DataRow drInstantTamperCount = dLMS650InstantaneousBLL.GetTamperCount(obisCode, meterDataID);

                if (drInstantTamperCount == null)
                {
                    if (obisCode == "0.0.12.80.3.255")
                        obisCode = "0.0.96.2.177.255"; // Magnet Fraud Tamper Count obis code is "replace" with Magnet Tamper Counter obis code
                    else if (obisCode == "0.0.12.80.5.255")
                        obisCode = "0.0.96.2.178.255";// Neutral Disturbance Tamper Count obis code is replace with Neutral Disturbance Tamper Counter obis code

                    drInstantTamperCount = dLMS650InstantaneousBLL.GetTamperCount(obisCode, meterDataID);
                }
                if (drInstantTamperCount != null)
                {
                    if (drInstantTamperCount["InstantPowerColumnValue"].ToString().Contains("*"))
                        drInstantTamperCount["InstantPowerColumnValue"] = drInstantTamperCount["InstantPowerColumnValue"].ToString().Split('*')[0];

                    dr["Count"] = (drInstantTamperCount["InstantPowerColumnValue"]);
                }

            }
            return dataSet;
        }

        /// <summary>
        /// Add a tamper "obis code" and "event code" for total no of tamper's count
        /// </summary>
        private void GetObisCodes()
        {

            tamperCount.Add(1, "0.0.96.2.162.255");// R-Phase Missing Potential Tamper Counter
            tamperCount.Add(3, "0.0.96.2.163.255");// Y-Phase Missing Potential Tamper Counter
            tamperCount.Add(5, "0.0.96.2.164.255");// B-Phase Missing Potential Tamper Counter
            tamperCount.Add(7, "0.0.96.2.165.255");// Over Voltage Tamper Counter
            tamperCount.Add(9, "0.0.96.2.166.255");// Low Voltage Tamper Counter
            tamperCount.Add(11, "0.0.96.2.167.255");// Voltage Unbalance Tamper Counter
            tamperCount.Add(47, "0.0.96.2.193.255");// Invalid Phase Association Tamper Counter
            tamperCount.Add(49, "0.0.96.2.192.255");// Invalid Voltage Tamper Counter
            tamperCount.Add(51, "0.0.96.2.168.255");// R-Phase CT Reversal Tamper Counter
            tamperCount.Add(53, "0.0.96.2.169.255");// Y-Phase CT Reversal Tamper Counter
            tamperCount.Add(55, "0.0.96.2.170.255");// B-Phase CT Reversal Tamper Counter
            tamperCount.Add(57, "0.0.96.2.171.255");// R-Phase CT Open Tamper Counter
            tamperCount.Add(59, "0.0.96.2.172.255");// Y-Phase CT Open Tamper Counter
            tamperCount.Add(61, "0.0.96.2.173.255");// B-Phase CT Open Tamper Counter
            tamperCount.Add(63, "0.0.96.2.174.255");// Current Unbalance Tamper Counter
            tamperCount.Add(65, "0.0.96.2.175.255");// CT Bypass Tamper Counter
            tamperCount.Add(67, "0.0.96.2.176.255");// Over Current Tamper Counter
            tamperCount.Add(69, "0.0.12.80.2.255");// Earthed Load Tamper Count
            tamperCount.Add(91, "0.0.96.2.194.255");// Over Load Tamper Counter
            tamperCount.Add(101, "0.0.96.2.180.255");// Power On-Off Tamper Counter
            tamperCount.Add(201, "0.0.12.80.3.255");// Magnet Tamper Counter
            tamperCount.Add(203, "0.0.12.80.5.255");// Neutral Disturbance Tamper Counter
            tamperCount.Add(205, "0.0.96.2.179.255");// Very Low PF Tamper Counter
            tamperCount.Add(207, "0.0.12.80.7.255");// Single Wire Tamper Counter
            tamperCount.Add(247, "0.0.96.2.195.255");// 2PN Tamper Counter
            tamperCount.Add(251, "0.0.96.2.181.255");// Front Cover Open Tamper Counter
            tamperCount.Add(249, "0.0.12.81.3.255");// ESD Tamper Count 


        }

        /// <summary>
        /// Process Tamper data set to remove occurence from tamper types
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private DataSet ProcessTamperDataSet(DataSet dataSet)
        {
            // Warning : If this string is changed in data base logic will not work.
            string strOccurence = "- Occurrence";
            // Warning : If this string is changed in data base logic will not work.
            string strDescription = string.Empty;
            DataColumn dataColumn = new DataColumn("S.No.", typeof(string));
            dataSet.Tables[0].Columns.Add(dataColumn);
            dataColumn.SetOrdinal(0);
            int intCounter = 1;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                dataRow[0] = intCounter;
                strDescription = dataRow[2].ToString();
                if (strDescription.Contains(strOccurence))
                {
                    strDescription = strDescription.Remove(strDescription.IndexOf(strOccurence));
                    dataRow[2] = strDescription;
                }
                intCounter++;
            }
            return dataSet;

        }

        public DataSet GetTamperDetailByTamperId(long meterDataID, int tamperId, long fromDate, long toDate)
        {
            DataSet tamperDataSet = dLMS650TamperMasterDAL.GetTamperDetailByTamperId(meterDataID, tamperId, fromDate, toDate);
            if (tamperDataSet != null && tamperDataSet.Tables != null && tamperDataSet.Tables[0].Rows.Count > 0)
            {
                if (ConfigInfo.ActiveFileType == "DLMS" && ConfigInfo.ActiveMeterType != CAB.Framework.MeterType.OnePhaseTwoWire)
                {
                    tamperDataSet = common.ApplyEMF(tamperDataSet, meterDataID);
                }
            }
            return tamperDataSet;
        }
        /* VBM Added for New tamper Report */

        /// <summary>
        /// BhardwajG : get tamper details from date range
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetTamperDetailByDateRange(int meterDataID, long fromDate, long toDate)
        {
            DataSet dataSet = dLMS650TamperMasterDAL.GetTamperDetailByDateRange(meterDataID, fromDate, toDate);
            
            //Get tamper count from Instantaneous            
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                string obisCode = string.Empty;
                dLMS650InstantaneousBLL = new DLMS650InstantaneousBLL();

                if (tamperCount.ContainsKey(Convert.ToInt32(dr["TamperId"])))
                    obisCode = tamperCount[Convert.ToInt32(dr["TamperId"])];

                DataRow drInstantTamperCount = dLMS650InstantaneousBLL.GetTamperCount(obisCode, meterDataID);

                //Replace values 
                if (drInstantTamperCount != null)
                {
                    if (drInstantTamperCount["InstantPowerColumnValue"].ToString().Contains("*"))
                        drInstantTamperCount["InstantPowerColumnValue"] = drInstantTamperCount["InstantPowerColumnValue"].ToString().Split('*')[0];

                    dr["Count"] = drInstantTamperCount["InstantPowerColumnValue"];
                }

            }

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataSet = ProcessTamperDataSet(dataSet);
            }
            return dataSet;

        }
        /// <summary>
        /// get tamper details from date range based on compartment ID
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetTamperDetailByDateRangeWithCompartmentID(int meterDataID, long fromDate, long toDate, string compartmentNo)
        {
            DataSet dataSet = dLMS650TamperMasterDAL.GetTamperDetailByDateRangeWithCompartmentID(meterDataID, fromDate, toDate, compartmentNo);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dataSet = ProcessTamperDataSet(dataSet);
            }
            return dataSet;
        }

        public DataSet AllDetailData(long meterDataID)
        {
            DataSet dataset = dLMS650TamperMasterDAL.AllDetailData(meterDataID);
            if (ConfigInfo.ActiveFileType == "DLMS" && ConfigInfo.ActiveMeterType != CAB.Framework.MeterType.OnePhaseTwoWire)
            {
                dataset = common.ApplyEMF(dataset, meterDataID);
            }
            return dataset;
        }
        public DataSet Transaction(long meterDataID)
        {
            return common.Transaction(meterDataID);
        }
        public Dictionary<string, string> CreateTamperDictionary(string selectedMeterId)
        {
            //For Reports: Selected Meter can be mupltiple. i.e. selected meter id will be passed.
            //For Analysis Report Selected id will be empty. then Selected Active Meter Id will be selected Meterid
            if (!string.IsNullOrEmpty(selectedMeterId))
            {
                //Get Meter Data Type (HTCT/LTCT) and assign it to global methods
                CommonMethods.MeterDataType = dlms650General.GetMeterDataType(selectedMeterId);
                CommonMethods.MeterModelNumber = dlms650General.GetMeterModelNoByMeterDataID(selectedMeterId);
            }
            else
            { //Get Meter Data Type (HTCT/LTCT) and assign it to global methods
                CommonMethods.MeterDataType = dlms650General.GetMeterDataType(ConfigInfo.ActiveMeterDataId);
                //meterModelNo = dlms650General.GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
            }
            tamperColumns.Add("Time Stamp", "DateTimeEvent");
            tamperColumns.Add("Event Code", "EventCode");
            tamperColumns.Add("R Phase Current", "CurrentIR");
            tamperColumns.Add("Y Phase Current", "CurrentIY");
            tamperColumns.Add("B Phase Current", "CurrentIB");
            tamperColumns.Add("R Phase Voltage", "VoltageVRN");
            tamperColumns.Add("Y Phase Voltage", "VoltageVYN");
            tamperColumns.Add("B Phase Voltage", "VoltageVBN");
            tamperColumns.Add("Signed Power Factor – R phase", "PowerFactorRphase");
            tamperColumns.Add("Signed Power Factor – Y phase", "PowerFactorYphase");
            tamperColumns.Add("Signed Power Factor – B phase", "PowerFactorBphase");
            tamperColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh"), "CumulativeEnergykWh");

            tamperColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Import"), "CumulativeEnergykWhImport");
            tamperColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}Wh Export"), "CumulativeEnergykWh Export");

            if (UtilityDetails.ShowApparantEnergyInTamper)
            {
                if (CommonMethods.MeterModelNumber == NamePlateConstants.PumaLTE650Value
                    || CommonMethods.MeterModelNumber == NamePlateConstants.PumaHTE650Value)
                {
                    tamperColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy - {0}VAh"), "CumulativeEnergykVAh");
                }
            }
            //tamperColumns.Add("Compartment Number", "CompartmentNumber");
            return tamperColumns;
        }
        private List<string> GetDatabaseColumns(List<string> columnList)
        {

            CreateTamperDictionary(string.Empty);
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (tamperColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }


        public DataSet GetTamperSnapshotData(string value, List<string> columnList, int tamperCode)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            return common.ApplyEMF(dLMS650TamperMasterDAL.GetTamperSnapshotData(value, columns, tamperCode));
        }

        public DataSet GetTamperSnapshotDataByFileName(string value, string fileName, List<string> columnList, int tamperCode)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            return common.ApplyEMF(dLMS650TamperMasterDAL.GetTamperSnapshotDataByFileName(value, fileName, columns, tamperCode));
        }

        public DataSet GetTransactionSnapShotData(string value, List<string> columnList, int tamperCode)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            return common.ApplyEMF(dLMS650TamperMasterDAL.GetTransactionSnapshotData(value, columns, tamperCode));
        }

        public DataSet GetTransactionSnapShotDataByFileName(string value, string fileName, List<string> columnList, int tamperCode)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            return common.ApplyEMF(dLMS650TamperMasterDAL.GetTransactionSnapshotDataByFileName(value, fileName, columns, tamperCode));
        }
        public bool DeleteData(long meterDataID)
        {
            return dLMS650TamperMasterDAL.DeleteData(meterDataID);
        }

        public DataTable DetailDataForAutomationExport(long meterDataID, string parameters, Dictionary<string, string> dictTamperCodeAndAbbreviation)
        {
            return dLMS650TamperMasterDAL.DetailDataForAutomationExport(meterDataID, parameters, dictTamperCodeAndAbbreviation);
        }
    }
}
