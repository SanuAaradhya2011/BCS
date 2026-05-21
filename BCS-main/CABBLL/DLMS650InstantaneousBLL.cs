
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
using CAB.Framework.Utility;
using System.Data;
using System.Collections.Generic;
using System;
using Utilities;
using System.Linq;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class DLMS650InstantaneousBLL : IBLL
    {
        DLMS650CommonBLL dLMS650CommonBLL;
        DLMS650InstantaneousDAL instantaneousDAL;
        public string fileName = string.Empty;
        Dictionary<string, string> instantColumns = new Dictionary<string, string>();
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        private const string CUMPOWERFAILUREFORINSTANT = "Cumulative Power-Failure Duration";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650InstantaneousBLL).ToString());
        public DLMS650InstantaneousBLL()
        {
            //Utility check added; 24th April 2012; Bug 75902   
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            else if (UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else
            {
                isPUMA = false;
                isMVVNL = false;
            }
            dLMS650CommonBLL = new DLMS650CommonBLL();
            instantaneousDAL = new DLMS650InstantaneousDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return instantaneousDAL.InsertData(entity);
        }
        public IEntity InsertData(IList<IEntity> entities)
        {
            return instantaneousDAL.InsertData(entities);
        }

        private void AddNetRows(ref DataSet ds, int MeterData_ID)
        {
            try
            {
                decimal NetKWHValue = 0;
                decimal NetKVAHValue = 0;              
                decimal CumKWHValue = 0;
                decimal CumKWHExportValue = 0;
                decimal CumKVAHValue = 0;
                decimal CumKVAHExportValue = 0;

                CumKWHExportValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kWh Export'")[0]["Value"]);
                CumKWHValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kWh'")[0]["Value"]);
                NetKWHValue = CumKWHValue - CumKWHExportValue;               

                DataRow dr = ds.Tables[0].NewRow();
                dr["Descriptions"] = "Net kWh";
                dr["OBIS Code"] = "----";
                dr["Class ID"] = "----";
                dr["Attribute"] = "----";
                dr["Value"] = NetKWHValue;
                dr["Unit"] = "kWh";
                ds.Tables[0].Rows.Add(dr);

                CumKVAHExportValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kVAh Export'")[0]["Value"]);
                CumKVAHValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kVAh'")[0]["Value"]);
                NetKVAHValue = CumKVAHValue - CumKVAHExportValue; 

                DataRow dr1 = ds.Tables[0].NewRow();
                dr1["Descriptions"] = "Net kVAh";
                dr1["OBIS Code"] = "----";
                dr1["Class ID"] = "----";
                dr1["Attribute"] = "----";
                dr1["Value"] = NetKVAHValue;
                dr1["Unit"] = "kVAh"; 
                ds.Tables[0].Rows.Add(dr1);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AddNetRows(ref DataSet ds, int MeterData_ID)", ex);
            }            
        }

        private void AddNetImportRows(ref DataSet ds, int MeterData_ID)
        {
            try
            {
                decimal NetKWHValue = 0;
                decimal NetKVAHValue = 0;
                decimal CumKWHValue = 0;
                decimal CumKWHExportValue = 0;
                decimal CumKVAHValue = 0;
                decimal CumKVAHExportValue = 0;
                               
                CumKWHExportValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kWh Export'")[0]["Value"]);
                CumKWHValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kWh'")[0]["Value"]);
                NetKWHValue = CumKWHValue - CumKWHExportValue;

                DataRow dr = ds.Tables[0].NewRow();
                dr["Descriptions"] = "Net kWh";
                dr["OBIS Code"] = "----";
                dr["Class ID"] = "----";
                dr["Attribute"] = "----";
                dr["Value"] = NetKWHValue;
                dr["Unit"] = "kWh";
                ds.Tables[0].Rows.Add(dr);

                CumKVAHExportValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kVAh Export'")[0]["Value"]);
                CumKVAHValue = Convert.ToDecimal(ds.Tables[0].Select("Descriptions = 'Cumulative Energy kVAh'")[0]["Value"]);
                NetKVAHValue = CumKVAHValue - CumKVAHExportValue;

                DataRow dr1 = ds.Tables[0].NewRow();
                dr1["Descriptions"] = "Net kVAh";
                dr1["OBIS Code"] = "----";
                dr1["Class ID"] = "----";
                dr1["Attribute"] = "----";
                dr1["Value"] = NetKVAHValue;
                dr1["Unit"] = "kVAh";
                ds.Tables[0].Rows.Add(dr1);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AddNetImportRows(ref DataSet ds, int MeterData_ID)", ex);
            }            
        }




    
        public DataSet GetMeterData(int meterDataID)
        {
            DataSet ds = null;
            try
            {
                ds = dLMS650CommonBLL.ConvertInstantRowToColumn(instantaneousDAL.GetMeterData(meterDataID), meterDataID);
                string meterVariant = dLMS650CommonBLL.GetMeterVariantByMeterDataID(meterDataID);
                switch (meterVariant)
                {
                    case CAB.Framework.MeterVariant.THREE:
                        AddNetRows(ref ds, meterDataID);
                        break;
                    case CAB.Framework.MeterVariant.FOUR:
                        AddNetImportRows(ref ds, meterDataID);
                        break;                    
                    default:
                        break;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int meterDataID)", ex);
                
            }
            return ds;
        }
        public Dictionary<string, string> CreateInstantDictionary(string selectedMeterId)
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

            instantColumns.Add("Real Time Clock - Date and Time", "InstantPowerColumnName");
            instantColumns.Add("R Phase Current", "InstantPowerColumnName");
            instantColumns.Add("Y Phase Current", "InstantPowerColumnName");
            instantColumns.Add("B Phase Current", "InstantPowerColumnName");
            instantColumns.Add("R Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("Y Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("B Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – R phase", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – Y phase", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – B phase", "InstantPowerColumnName");
            instantColumns.Add("Three Phase Power Factor - PF", "3PhasePF");
            instantColumns.Add("Frequency", "Frequency");
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conApparentPowerKVA), "AppPower");//"Apparent Power - kVA";
            //Active Power Label updated.
            if (isPUMA)
            {
                instantColumns.Add("Active Power (ABS)", "SignedActPower");
            }
            else
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conSingnedActivePowerKW), "SignedActPower");// "Signed Active Power - kW (+ Forward - Reverse)";
            }
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conSignedReactivePowerKVAR), "SignedReacPower");//"Signed Reactive Power - kvar (+ Lag - Lead)";
            //BhardwajG : TFS ID : 140774 : Corrected the sequence for Ruby
            if (!isPUMA)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKWH), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLag), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lag"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLead), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lead"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVAH), "InstantPowerColumnName");//"Cumulative Energy - kVAh"

            }
            // Changed the sequence for adding the parameteers in report for bug 83717..
            instantColumns.Add("Number of Power - Failures", "PowerFail");
            instantColumns.Add("Cumulative Power-Failure Duration", "CumPowerFail");
            instantColumns.Add("Cumulative Tamper Count", "CumTampCount");
            instantColumns.Add("Cumulative Billing Count", "CumBillCount");
            instantColumns.Add("Cumulative Programming Count", "CumProgCount");
            instantColumns.Add("Billing Date", "BillDate");
            //Corrected the sequence for PUMA
            if (isPUMA)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKWH), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLag), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lag"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLead), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lead"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVAH), "InstantPowerColumnName");//"Cumulative Energy - kVAh"

            }
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKW), "InstantPowerColumnName");//"Maximum Demand - kW"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKWDateTime), "InstantPowerColumnName");//"Maximum Demand - kW DateTime"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKVA), "InstantPowerColumnName");//"Maximum Demand - kVA"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKVADateTime), "InstantPowerColumnName");//"Maximum Demand - kVA DateTime"
            //VBM - Added cumulative export energy column if utility has this feature.
            if (UtilityDetails.ShowCumulativeExportEnergyKWH)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeExportEnergyKWH), "InstantPowerColumnName");//"Cumulative Export energy"

            }
            //BhardwajG : TFS ID : 140774
            //These two new parameters added for PUMA Load Survey Report; 24th April 2012; Bug 75902   
            if (isPUMA)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeMDKW), "InstantPowerColumnName");//"Cumulative Maximum Demand – kW"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeMDKVA), "InstantPowerColumnName");//"Cumulative Maximum Demand – kVA"
            }
            
            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.KSEB.ToString())
            {                
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conReverseKWh), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conReverseKVAH), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conReversKVArhLag), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conReversKVArhLead), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conPresentTimeZone), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT1), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT2), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT3), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT4), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT5), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT6), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT7), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeKkWhWithHighResolutionT8), "InstantPowerColumnName");
            }
            
           return instantColumns;
        }
        /// <summary>
        /// This funtion will create instant dictionary 
        /// This will be used if the utility contains Ruby and PUMA meters both.
        /// </summary>
        /// <param name="selectedMeterId"></param>
        /// <param name="isPUMAMeter"></param>
        /// <returns></returns>
        public Dictionary<string, string> CreateInstantDictionary(string selectedMeterId, bool isPUMAMeter)
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

            instantColumns.Add("Real Time Clock - Date and Time", "InstantPowerColumnName");
            instantColumns.Add("R Phase Current", "InstantPowerColumnName");
            instantColumns.Add("Y Phase Current", "InstantPowerColumnName");
            instantColumns.Add("B Phase Current", "InstantPowerColumnName");
            instantColumns.Add("R Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("Y Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("B Phase Voltage", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – R phase", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – Y phase", "InstantPowerColumnName");
            instantColumns.Add("Signed Power Factor – B phase", "InstantPowerColumnName");
            instantColumns.Add("Three Phase Power Factor - PF", "3PhasePF");
            instantColumns.Add("Frequency", "Frequency");
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conApparentPowerKVA), "AppPower");//"Apparent Power - kVA";
            //Active Power Label updated.
            if (isPUMAMeter) // VBM - To resolve defect #150011
            {
                instantColumns.Add("Active Power (ABS)", "SignedActPower");
            }
            else
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conSingnedActivePowerKW), "SignedActPower");// "Signed Active Power - kW (+ Forward - Reverse)";
            }
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conSignedReactivePowerKVAR), "SignedReacPower");//"Signed Reactive Power - kvar (+ Lag - Lead)";
            //BhardwajG : TFS ID : 140774 : Corrected the sequence for Ruby
            if (!isPUMAMeter)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKWH), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLag), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lag"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLead), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lead"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVAH), "InstantPowerColumnName");//"Cumulative Energy - kVAh"

            }
            // Changed the sequence for adding the parameteers in report for bug 83717..
            instantColumns.Add("Number of Power - Failures", "PowerFail");
            instantColumns.Add("Cumulative Power-Failure Duration", "CumPowerFail");
            instantColumns.Add("Cumulative Tamper Count", "CumTampCount");
            instantColumns.Add("Cumulative Billing Count", "CumBillCount");
            instantColumns.Add("Cumulative Programming Count", "CumProgCount");
            instantColumns.Add("Billing Date", "BillDate");
            //Corrected the sequence for PUMA
            if (isPUMAMeter)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKWH), "InstantPowerColumnName");
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLag), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lag"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVARHLead), "InstantPowerColumnName");//"Cumulative Energy - kvarh - lead"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyKVAH), "InstantPowerColumnName");//"Cumulative Energy - kVAh"

            }
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKW), "InstantPowerColumnName");//"Maximum Demand - kW"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKWDateTime), "InstantPowerColumnName");//"Maximum Demand - kW DateTime"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKVA), "InstantPowerColumnName");//"Maximum Demand - kVA"
            instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conMDKVADateTime), "InstantPowerColumnName");//"Maximum Demand - kVA DateTime"
            //VBM - Added cumulative export energy column if utility has this feature.
            if (UtilityDetails.ShowCumulativeExportEnergyKWH)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeExportEnergyKWH), "InstantPowerColumnName");//"Cumulative Export energy"

            }

            //BhardwajG : TFS ID : 140774
            //These two new parameters added for PUMA Load Survey Report; 24th April 2012; Bug 75902   
            if (isPUMAMeter)
            {
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeMDKW), "InstantPowerColumnName");//"Cumulative Maximum Demand – kW"
                instantColumns.Add(CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeMDKVA), "InstantPowerColumnName");//"Cumulative Maximum Demand – kVA"
            }
            
            
            return instantColumns;
        }

        public DataSet GetInstantDataByParameter(string value, List<string> paramList,string activeMeterDataID)
        {
            DataSet ds = new DataSet();
            DataSet instData = new DataSet();
            List<string> columns = new List<string>();
            string[] cols = new string[] { "InstantPowerColumnName", "InstantPowerColumnValue", "InstantPowerObisCode", "InstantPowerClassID", "InstantPowerAttribute", "InstantPowerDataIndex" };
            foreach (string key in cols)
            {
                columns.Add(key);
            }
            for (int index = 0; index <= paramList.Count - 1; index++)
            {
                if (paramList[index] == "R Phase Current")
                {
                    paramList[index] = "Current - IR";
                    continue;
                }
                else if (paramList[index] == "Y Phase Current")
                {
                    paramList[index] = "Current - IY";
                    continue;
                }
                else if (paramList[index] == "B Phase Current")
                {
                    paramList[index] = "Current - IB";
                    continue;
                }
                else if (paramList[index] == "R Phase Voltage")
                {
                    paramList[index] = "Voltage – VRN";
                    continue;
                }
                else if (paramList[index] == "Y Phase Voltage")
                {
                    paramList[index] = "Voltage – VYN";
                    continue;
                }
                else if (paramList[index] == "B Phase Voltage")
                {
                    paramList[index] = "Voltage – VBN";
                    continue;
                }
            }
            if (fileName == "")
            {
                ds = instantaneousDAL.GetInstantDataByMeter(value, paramList, columns, activeMeterDataID);
            }
            else
            {
                ds = instantaneousDAL.GetInstantDataByFileName(value,fileName, paramList, columns, activeMeterDataID);
            }

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            DLMS650CommonBLL dlms650CommonBLL = new DLMS650CommonBLL();
            DataTable dtTable = new DataTable();
            dtTable.Columns.Add("MeterID");
            dtTable.Columns.Add("FileName");
            dtTable.Columns.Add("ReadingDateTime");
            foreach (DataRow dRow in ds.Tables[0].Rows)
            {
                string colName = dRow[3].ToString();
                colName = dlms650CommonBLL.GetColName(colName);
                if (!(dtTable.Columns.Contains(colName)))
                    dtTable.Columns.Add(new DataColumn(colName, typeof(System.String)));
            }

            
            int rowIndex = 0;
            int noofRecords = ds.Tables[0].Rows.Count / (dtTable.Columns.Count-3);
            int maxlength=0;
            int div = noofRecords;
            for (int index = 0; index <= noofRecords-1; index++)
            {
                DataRow row = dtTable.NewRow();
                int emf = new MeterMasterBLL().GetEMF(Convert.ToInt64(ds.Tables[0].Rows[rowIndex][ds.Tables[0].Columns.Count-1].ToString())); 
                row[0] = ds.Tables[0].Rows[rowIndex][0].ToString();
                row[1] = ds.Tables[0].Rows[rowIndex][1].ToString();
                row[2] = ds.Tables[0].Rows[rowIndex][2].ToString();

                maxlength += ds.Tables[0].Rows.Count / noofRecords;
                int parmIndex = 0;
                for (int i = rowIndex; i < maxlength; i++)
                {
                    string colName = ds.Tables[0].Rows[i][3].ToString();
                    string val = ds.Tables[0].Rows[i][4].ToString();
                    if (val.IndexOf('*') > 0)
                    {
                        string[] dat = val.Split('*');
                        val = dat[0].ToString();
                    }
                    if (dLMS650CommonBLL.CheckString("	Current - IR	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Current - IY	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Current - IB	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Apparent Power - kVA	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Signed Active Power - kW (+ Forward - Reverse)	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Signed Reactive Power - kvar (+ Lag - Lead)	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Energy - kWh	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Energy - kvarh - lag	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Energy - kvarh - lead	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Energy - kVAh	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Maximum Demand - kW	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Maximum Demand - kVA	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    // added two extra parameters for bug 83717 for PUMA
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Maximum Demand – kW	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                    else if (dLMS650CommonBLL.CheckString("	Cumulative Maximum Demand – kVA	", colName))
                        row[parmIndex + 3] = (decimal.Parse(val)).ToString();
                        /*VBM show Cumulative power fail duration in dd:hh:mm */
                    else if (dLMS650CommonBLL.CheckString(CUMPOWERFAILUREFORINSTANT, colName))
                        row[parmIndex + 3] = dLMS650CommonBLL.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(Convert.ToDouble(val)));
                    /*VBM show Cumulative power fail duration in dd:hh:mm */
                    else
                        row[parmIndex + 3] = val;
                    parmIndex++;
                }

                rowIndex = maxlength;
                div -= 1;
                dtTable.Rows.Add(row);
            }

            ds = new DataSet();
            ds.Tables.Add(dtTable);
            return dlms650CommonBLL.ApplyMultiplyFactor(Convert.ToInt64(activeMeterDataID),ds);
        }
        public bool DeleteData(long meterDataID)
        {
            return instantaneousDAL.DeleteData(meterDataID);
        }

        /// <summary>
        /// Getting tamper parameters by obis code
        /// </summary>
        /// <param name="obisCode"></param>
        internal DataRow GetTamperCount(string obisCode, long meterDataID)
        {
            return instantaneousDAL.GetTamperCount(obisCode, meterDataID);
        }
    }
}

