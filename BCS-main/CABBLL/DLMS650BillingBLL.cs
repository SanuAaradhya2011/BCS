
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
using Utilities;
using CAB.Framework.Utility;
using System;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class DLMS650BillingBLL : IBLL
    {
        DLMS650BillingDAL dLMS650BillingDAL;
        Dictionary<string, string> initialBillingColumns = new Dictionary<string, string>();
        DLMS650CommonBLL common;
        bool isMPKWCL = false;
        //private const string COLNAMEPOWEROFFDURATION = "CumPowerOffDuration";
        private const string COLNAMETAMPERCOUNT = "CumTamperCount";
        private const string TAMPERCOUNT = "Cumulative Tamper Count";
        private const string POWEROFFDURATION = "Cumulative Power-Failure Duration";
        private int meterModelNumber = 0;
        public string meter_cat;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650BillingBLL).ToString());
        // private string METERDATATYPE = string.Empty;
        public DLMS650BillingBLL()
        {
            //if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
            //{
            //    isMPKWCL = true;
            //}
            //dLMS650BillingDAL = new DLMS650BillingDAL();
            dLMS650BillingDAL = new DLMS650BillingDAL(UtilityDetails.Utility);
            common = new DLMS650CommonBLL();
            meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);

        }
        //public string MeterDataType
        //{
        //    get { return METERDATATYPE; }
        //    set { METERDATATYPE = value; }
        //}
        public IEntity InsertData(IEntity entity)
        {
            return dLMS650BillingDAL.InsertData(entity);
        }
        public IEntity InsertData(IList<IEntity> entities)
        {
            return dLMS650BillingDAL.InsertData(entities);
        }

        /// <summary>
        /// // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code and mapping change
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <param name="isConsumption"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        //public DataSet GetCumulativeEnergy(int MeterDataId, bool isConsumption, int meterModelNumber)
        //{
        //    DataSet ds = dLMS650BillingDAL.GetCumulativeEnergy(MeterDataId, isConsumption);// Story - 365971 - 13 billing for Power ON Hours
        //    return common.ConvertCumulativeEnergyToColumn(ds, MeterDataId, meterModelNumber);
        //}

        public DataSet GetCumulativeEnergy(int MeterDataId, bool isConsumption)
        {
            DataSet ds = dLMS650BillingDAL.GetCumulativeEnergy(MeterDataId, isConsumption);// Story - 365971 - 13 billing for Power ON Hours
            return common.ConvertCumulativeEnergyToColumn(ds, MeterDataId);
        }
        /// <summary>
        /// Use to get TOD details for specific meter
        /// </summary>
        /// <param name="MeterDataId">MeterId corrosponding to meter</param>
        /// <returns></returns>
        public DataSet GetTODDetails(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetTODDetails(MeterDataId,false);// Story - 365971 - 13 billing for Power ON Hours
            return common.ConvertTODDetailsToColumn(ds, MeterDataId);
        }

        /// <summary>
        /// Use to get TOD history details for specific meter
        /// </summary>
        /// <param name="MeterDataId">MeterId corrosponding to meter</param>
        /// <returns></returns>
        public DataSet GetTODHistoryDetails(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetTODDetails(MeterDataId,true);// Story - 365971 - 13 billing for Power ON Hours
            return common.ConvertTODHistoryDetailsToColumn(ds, MeterDataId);
        }
        /// <summary>
        /// Use to get billing month for specific meter
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingMonths(int MeterDataId)
        {
            return dLMS650BillingDAL.GetBillingMonths(MeterDataId);
        }
        // Added for MPKWCL
        /// <summary>
        /// This method is used to get call the method to fetch the miscellaneous data from DB.
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="historyID"></param>
        /// <returns></returns>
        public DataSet GetMiscellaneous(int meterDataId)
        {
            DataSet dataSet = dLMS650BillingDAL.GetMiscellaneous(meterDataId);
            dataSet = common.ConvertMiscellaneousToColumn(dataSet);
            return dataSet;
        }
        public DataSet GetCumulativeEnergyCalculated(int MeterDataId)
        {
            DataSet ds = GetCumulativeEnergy(MeterDataId,true); // Story - 365971 - 13 billing for Power ON Hours
            return common.ConvertCumulativeEnergyCalculatedToColumn(ds);
        }
        public DataSet GetMeterDataForRPT(long MeterDataId, int historyId)
        {
            DataSet ds = dLMS650BillingDAL.GetMeterData(MeterDataId, historyId);
            return common.ConvertTariffEnergyToColumnForRPT(ds, MeterDataId);
        }
        public DataSet GetMeterData(long MeterDataId, int historyId)
        {
            DataSet ds = dLMS650BillingDAL.GetMeterData(MeterDataId, historyId);
            return common.ConvertTariffEnergyToColumn(ds, MeterDataId);
        }
        public DataSet GetMeterData_PF(long MeterDataId, int historyId)
        {
            DataSet ds = dLMS650BillingDAL.GetMeterData(MeterDataId, historyId);
            return common.CalculateTariffPFToColumn(ds, MeterDataId);
        }
        public DataSet GetTODConsumption(int MeterDataId, int historyId, bool flag)
        {
            if (!flag)
            {
                DataSet history1 = common.ConvertTariffEnergyToColumnForRPT(dLMS650BillingDAL.GetMeterData(MeterDataId, historyId), MeterDataId);
                DataSet history2 = common.ConvertTariffEnergyToColumnForRPT(dLMS650BillingDAL.GetMeterData(MeterDataId, historyId + 1), MeterDataId);
                return common.ConvertTODConsumptionToColumnForRPT(history1, history2, MeterDataId);
            }
            else
            {
                DataSet history1 = common.ConvertTariffEnergyToColumn(dLMS650BillingDAL.GetMeterData(MeterDataId, historyId), MeterDataId);
                DataSet history2 = common.ConvertTariffEnergyToColumn(dLMS650BillingDAL.GetMeterData(MeterDataId, historyId + 1), MeterDataId);
                return common.ConvertTODConsumptionToColumn(history1, history2, MeterDataId);
            }
        }
        /// <summary>
        /// Get Meter Data For report With Month 
        /// </summary>
        /// <param name="DS"></param>
        /// <returns></returns>
        public DataSet GetCumulativeMaximumDemand(int MeterDataId)// for smart meter
        {
            DataSet ds = dLMS650BillingDAL.GetCumulMD(MeterDataId);
            return common.ConvertCumulativeMDToColumn(ds);
        }
        public DataSet GetMeterDataForRPTWithMonth(DataSet DS)
        {
            return common.ConvertMeterDataForRPTWithMonth(DS);
        }
        public DataSet GetMaximumDemand(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetMaximumDemand(MeterDataId);
            return common.ConvertMaximumDemandToColumn(ds, MeterDataId);
        }
        public DataSet GetTODMDMeterData(long MeterDataId, int historyId, bool isHistory)
        {
            DataSet ds = dLMS650BillingDAL.GetTODMDMeterData(MeterDataId, historyId);
            return common.ConvertTariffEnergyTODMDToColumnForRPT(ds, isHistory, MeterDataId);
        }
        // Added to get Billing Report
        public DataSet GetBillingReportForAllMeters()
        {
            DataSet ds = dLMS650BillingDAL.GetBillingReport();
            return common.ConvertBillingDataToColumn(ds);
        }
        // Added to get Billing Report by group
        public DataSet GetBillingReportByGroup(int GroupID)
        {
            DataSet ds = dLMS650BillingDAL.GetBillingReportByGroup(GroupID);
            return common.ConvertBillingDataToColumn(ds);
        }
        public DataSet GetAveragePowerFactor(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetAveragePowerFactor(MeterDataId);
            return common.ConvertHistoryWithSingleColumn(ds, MeterDataId);
        }

        /// <summary>
        /// To get the data for Billing Transaction
        ///  // Story no: 490966- WB tender specific check implemented for billing Rest Type OBIS code change
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingTransaction(int MeterDataId, int meterModel)
        {
            DataSet ds = dLMS650BillingDAL.GetBillingTransaction(MeterDataId);
            return common.ConvertHistoryWithSingleColumnBillingTransaction(ds, meterModel);
        }

        /// <summary>
        /// To get the data for Billing Transaction
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingTransaction(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetBillingTransaction(MeterDataId);
            return common.ConvertHistoryWithSingleColumnBillingTransaction(ds);
        }
        public Dictionary<string, string> CreateBillingDictionary(string selectedMeterId)
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
            initialBillingColumns.Add("Billing Date", "BillingDate");
            initialBillingColumns.Add("System PowerFactor for Billing Period", "SystemPowerFactorforBillingPeriod");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ0"), "CumulativeEnergykWhTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ1"), "CumulativeEnergykWhTZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ2"), "CumulativeEnergykWhTZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ3"), "CumulativeEnergykWhTZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ4"), "CumulativeEnergykWhTZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ5"), "CumulativeEnergykWhTZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ6"), "CumulativeEnergykWhTZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ7"), "CumulativeEnergykWhTZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}Wh TZ8"), "CumulativeEnergykWhTZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}varh - lag"), "CumulativeEnergykvarhLag");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}varh - lead"), "CumulativeEnergykvarhLead");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ0"), "CumulativeEnergykVAhTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ1"), "CumulativeEnergykVAhTZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ2"), "CumulativeEnergykVAhTZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ3"), "CumulativeEnergykVAhTZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ4"), "CumulativeEnergykVAhTZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ5"), "CumulativeEnergykVAhTZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ6"), "CumulativeEnergykVAhTZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ7"), "CumulativeEnergykVAhTZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy {0}VAh TZ8"), "CumulativeEnergykVAhTZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ0"), "MDkWTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ0"), "MDkWDateTimeTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ1"), "MDkWTZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ1"), "MDkWDateTimeTZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ2"), "MDkWTZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ2"), "MDkWDateTimeTZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ3"), "MDkWTZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ3"), "MDkWDateTimeTZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ4"), "MDkWTZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ4"), "MDkWDateTimeTZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ5"), "MDkWTZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ5"), "MDkWDateTimeTZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ6"), "MDkWTZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ6"), "MDkWDateTimeTZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ7"), "MDkWTZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ7"), "MDkWDateTimeTZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W TZ8"), "MDkWTZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}W DateTime TZ8"), "MDkWDateTimeTZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ0"), "MDkVATZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ0"), "MDkVADateTimeTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ1"), "MDkVATZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ1"), "MDkVADateTimeTZ1");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ2"), "MDkVATZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ2"), "MDkVADateTimeTZ2");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ3"), "MDkVATZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ3"), "MDkVADateTimeTZ3");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ4"), "MDkVATZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ4"), "MDkVADateTimeTZ4");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ5"), "MDkVATZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ5"), "MDkVADateTimeTZ5");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ6"), "MDkVATZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ6"), "MDkVADateTimeTZ6");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ7"), "MDkVATZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ7"), "MDkVADateTimeTZ7");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA TZ8"), "MDkVATZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VA DateTime TZ8"), "MDkVADateTimeTZ8");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy Fraud {0}Wh"), "CumulativeEnergyFraudkWh");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("Cumulative Energy Fraud {0}VAh"), "CumulativeEnergyFraudkVAh");

            // User Story - 1000867
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VAr Lag TZ0"), "MDkVArLagTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VAr Lag DateTime TZ0"), "MDkVArLagDateTimeTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VAr Lead TZ0"), "MDkVArLeadTZ0");
            initialBillingColumns.Add(CommonMethods.getDisplayHeaderText("MD {0}VAr Lead DateTime TZ0"), "MDkVArLeadDateTimeTZ0");

            //If Utility is PGVCL Then only show the column.
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.PGVCL)
            {
                initialBillingColumns.Add("Billing Type", "BillingResetType");
            }
            //if (isMPKWCL)
            //{
            //    initialBillingColumns.Add(POWEROFFDURATION, COLNAMEPOWEROFFDURATION);
            initialBillingColumns.Add(TAMPERCOUNT, COLNAMETAMPERCOUNT);
            //}
            if (UtilityDetails.ShowPowerOffDurationInBilling && meterModelNumber != NamePlateConstants.RubyE250Value)
            {
                initialBillingColumns.Add("Power Off Duration", "PowerOffDuration");
            }
            return initialBillingColumns;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            CreateBillingDictionary(string.Empty);
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (initialBillingColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
        public DataSet GetBillingData(string value, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            DataSet billingData = dLMS650BillingDAL.GetBillingDataByMeter(value, columns);
            if (billingData != null && billingData.Tables.Count > 0)
            {
                if (billingData.Tables[0].Columns.Contains("PowerOffDuration"))
                {
                    billingData = ConvertPowerOffDuration(billingData);
                }

                billingData = common.ApplyBillingEMF(billingData);

            }
            return billingData;
        }
        /// <summary>
        /// Convert Billing power off duration 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        private DataSet ConvertPowerOffDuration(DataSet powerOffData)
        {
            ulong powerOffValue;
            ulong previousPowerOffValue = 0;
            ulong powerOffConsumption = 0;
            ulong previousMeterDataId = 0;
            ulong currentMeterDataId = 0;
            for (int rowCount = powerOffData.Tables[0].Rows.Count - 1; rowCount >= 0; rowCount--)
            {
                //To make sure that in meter wise report calculation of power off duration will be done file wise.
                currentMeterDataId = Convert.ToUInt32(powerOffData.Tables[0].Rows[rowCount]["MeterData_ID"].ToString());
                if (currentMeterDataId != 0 && (previousMeterDataId != currentMeterDataId))
                {
                    previousPowerOffValue = 0;
                }

                powerOffValue = Convert.ToUInt32(common.CheckUnit(powerOffData.Tables[0].Rows[rowCount]["PowerOffDuration"].ToString())[0]);
                powerOffConsumption = common.GetPowerOffHoursForRollOverData(powerOffValue, previousPowerOffValue);
                powerOffData.Tables[0].Rows[rowCount]["PowerOffDuration"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(powerOffConsumption));
                previousPowerOffValue = powerOffValue;
                previousMeterDataId = currentMeterDataId;
            }
            powerOffData.Tables[0].Columns.Remove("MeterData_ID");
            powerOffData.AcceptChanges();
            return powerOffData;
        }

        public DataSet GetBillingDataByFileName(string value, string fileName, List<string> columnList)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            DataSet billingData = dLMS650BillingDAL.GetBillingDataByFileName(value, fileName, columns);
            if (billingData != null && billingData.Tables.Count > 0)
            {
                if (billingData.Tables[0].Columns.Contains("PowerOffDuration"))
                {
                    billingData = ConvertPowerOffDuration(billingData);
                }

                billingData = common.ApplyBillingEMF(billingData);

            }
            return billingData;
        }
        public bool DeleteData(long meterDataID)
        {
            return dLMS650BillingDAL.DeleteData(meterDataID);
        }
        /// <summary>
        /// Used to fetch power off duration and convert it into proper format.
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetPowerOffDuration(int MeterDataId)
        {
            DataSet ds = dLMS650BillingDAL.GetPowerOffDuration(MeterDataId);
            return common.ConvertPowerOffDuration(ds);
        }

        /// <summary>
        /// Used to fetch power off duration and convert it into proper format.
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetPowerOnDuration(int MeterDataId)
        {
            DataSet powerOnDataSet = null;
            try
            {
                DataSet powerOnDurationDataSet = dLMS650BillingDAL.GetPowerOnDuration(MeterDataId);
                powerOnDataSet = new DataSet();

                if (powerOnDurationDataSet != null && powerOnDurationDataSet.Tables.Count > 0 && powerOnDurationDataSet.Tables[0].Rows.Count > 0)
                {
                    bool isCumPowerOnDuration = false;
                    bool isCumulativePowerOffDuration = false;
                    bool isPowerOnDuration = false;
                    bool isBillingTypePowerOffDuration = false;
                    int rowCount = 0;
                    int lastRowCount = 0;
                    long totalMinutes;
                    ulong powerValue = 0;
                    ulong previousPowerValue = 0;
                    DataRow firstRow, secondRow = null;
                    byte powerOnDurationDisplay = Convert.ToByte(powerOnDurationDataSet.Tables[0].Rows[0]["PowerOnDurationDisplay"]);

                    // By Mohsin
                    // For CSPDCL : Identifyinh is cumPowerOff is available in Billing
                    if(powerOnDurationDataSet.Tables[0].Rows.Count > 1)
                        powerOnDurationDisplay = Convert.ToByte(powerOnDurationDataSet.Tables[0].Rows[1]["PowerOnDurationDisplay"]);

                    DataTable dummyTable = new DataTable();
                    isCumPowerOnDuration = Convert.ToBoolean(powerOnDurationDisplay & 1);
                    powerOnDurationDisplay = (byte)(powerOnDurationDisplay >> 1);
                    isPowerOnDuration = Convert.ToBoolean(powerOnDurationDisplay & 1);
                    powerOnDurationDisplay = (byte)(powerOnDurationDisplay >> 1);
                    isCumulativePowerOffDuration = Convert.ToBoolean(powerOnDurationDisplay & 1);
                    powerOnDurationDisplay = (byte)(powerOnDurationDisplay >> 1);
                    isBillingTypePowerOffDuration = Convert.ToBoolean(powerOnDurationDisplay & 1);


                    string chkPowerOnOffDurationFormat = ConfigSettings.GetValue("ChkPowerOnOffDurationFormat");               
                    if (isCumPowerOnDuration || isPowerOnDuration || isCumulativePowerOffDuration || isBillingTypePowerOffDuration)
                    {
                        dummyTable.Columns.Add("History", typeof(String));
                        dummyTable.Columns.Add("Billing Date Time(0.0.0.1.2.255;3;2)", typeof(String));


                        if (chkPowerOnOffDurationFormat == "1")
                        {                            
                            dummyTable.Columns.Add("Power On Duration(0.0.94.91.13.255;3;2) dddd:hh", typeof(String));
                            dummyTable.Columns.Add("Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh", typeof(String));                            
                        }
                        else
                        {                            
                            //dummyTable.Columns.Add("Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm", typeof(String));
                            //dummyTable.Columns.Add("Power Off Duration(0.0.96.1.217.255;3;2) dd:hh:mm", typeof(String));

                            dummyTable.Columns.Add("Power On Duration(0.0.94.91.13.255;3;2) dddd:hh:mm", typeof(String));
                            dummyTable.Columns.Add("Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm", typeof(String));
                        }
                        
                        byte historyValue = 0;
                        foreach (DataRow dataRow in powerOnDurationDataSet.Tables[0].Rows)
                        {
                            if (historyValue == 0)
                                dummyTable.Rows.Add("Current", dataRow["BillingTimeStamp"], 0);
                            else
                                dummyTable.Rows.Add("History - " + dataRow["History"].ToString(), dataRow["BillingTimeStamp"], 0);
                            historyValue++;
                        }
                        //If Delta Power On is present
                        if (isPowerOnDuration)
                        {
                            lastRowCount = powerOnDurationDataSet.Tables[0].Rows.Count;
                            for (rowCount = 0; rowCount < powerOnDurationDataSet.Tables[0].Rows.Count; rowCount++)
                            {
                                dummyTable.Rows[rowCount][1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][1]));
                                if (chkPowerOnOffDurationFormat == "1")
                                {
                                    dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDDDHH(
                                   TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][2]))
                                   );
                                }
                                else
                                {
                                dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDHHMM(
                                    TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][2]))
                                    );
                                }
                                //If Delta Power Off is present
                                if (isBillingTypePowerOffDuration)
                                {
                                    if (chkPowerOnOffDurationFormat == "1")
                                    {
                                        dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDDDHH(
                                       TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingWisePowerOffDuration"]))
                                       );
                                    }
                                    else
                                    {
                                        dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingWisePowerOffDuration"]))
                                        );
                                    }
                                }
                                //If Cumilative Power Off is present
                                else if (isCumulativePowerOffDuration)
                                {
                                    powerValue = Convert.ToUInt32(common.CheckUnit(powerOnDurationDataSet.Tables[0].Rows[lastRowCount - rowCount - 1]["PowerOffDuration"].ToString())[0]);
                                    if (chkPowerOnOffDurationFormat == "1")
                                    {
                                        dummyTable.Rows[lastRowCount - rowCount - 1][3] = common.ConvertTimSpanToDDDDHH(TimeSpan.FromMinutes(common.GetPowerOffHoursForRollOverData(powerValue, previousPowerValue)));
                                        previousPowerValue = powerValue;
                                    }
                                    else
                                    {

                                        dummyTable.Rows[lastRowCount - rowCount - 1][3] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(common.GetPowerOffHoursForRollOverData(powerValue, previousPowerValue)));
                                        previousPowerValue = powerValue;
                                    }
                                }
                                // Else calculate Delta Power off via Delta Power On
                                else
                                {
                                    if (rowCount < powerOnDurationDataSet.Tables[0].Rows.Count - 1)
                                    {
                                        firstRow = powerOnDurationDataSet.Tables[0].Rows[rowCount];
                                        secondRow = powerOnDurationDataSet.Tables[0].Rows[rowCount + 1];
                                        totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                                        totalMinutes = totalMinutes - (long)Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["PowerOnDuration"]);
                                        if (chkPowerOnOffDurationFormat == "1")
                                        {
                                            dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDDDHH(TimeSpan.FromMinutes(totalMinutes));
                                        }
                                        else
                                        {
                                            dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));
                                        }
                                    }
                                    else
                                    {
                                        if (chkPowerOnOffDurationFormat == "1")
                                        {
                                            dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh"] = "----";
                                        }
                                        else
                                        {
                                            dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm"] = "----";//dd
                                        }
                                    }
                                }
                            }
                        }
                        // If Delta Power On is present
                        else if (isBillingTypePowerOffDuration)
                        {
                            lastRowCount = powerOnDurationDataSet.Tables[0].Rows.Count;
                            for (rowCount = 0; rowCount < powerOnDurationDataSet.Tables[0].Rows.Count; rowCount++)
                            {
                                dummyTable.Rows[rowCount][1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][1]));
                                if (chkPowerOnOffDurationFormat == "1")
                                {
                                    dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][3]))
                                        );
                                }
                                else
                                {
                                    dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDHHMM(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][3]))
                                        );
                                }
                                // If Cumilative Power On is present
                                if (isCumPowerOnDuration)
                                {
                                    powerValue = Convert.ToUInt32(common.CheckUnit(powerOnDurationDataSet.Tables[0].Rows[lastRowCount - rowCount - 1]["CumPowerOnDuration"].ToString())[0]);

                                    if (chkPowerOnOffDurationFormat == "1")
                                    {
                                        dummyTable.Rows[lastRowCount - rowCount - 1][2] = common.ConvertTimSpanToDDDDHH(TimeSpan.FromMinutes(common.GetPowerOffHoursForRollOverData(powerValue, previousPowerValue)));
                                        previousPowerValue = powerValue;
                                    }
                                    else
                                    {
                                        dummyTable.Rows[lastRowCount - rowCount - 1][2] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(common.GetPowerOffHoursForRollOverData(powerValue, previousPowerValue)));
                                        previousPowerValue = powerValue;
                                    }
                                   
                                }
                                // Else calculate Delta Power On via Delta Power Off
                                else
                                {
                                    if (rowCount < powerOnDurationDataSet.Tables[0].Rows.Count - 1)
                                    {
                                        firstRow = powerOnDurationDataSet.Tables[0].Rows[rowCount];
                                        secondRow = powerOnDurationDataSet.Tables[0].Rows[rowCount + 1];
                                        totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                                        totalMinutes = totalMinutes - (long)Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingWisePowerOffDuration"]);
                                        if (chkPowerOnOffDurationFormat == "1")
                                        {
                                            dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDDDHH(TimeSpan.FromMinutes(totalMinutes));
                                        }
                                        else
                                        {
                                            dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));
                                        }
                                    }
                                    else
                                    {
                                        if (chkPowerOnOffDurationFormat == "1")
                                        {
                                            dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dddd:hh"] = "----";
                                        }
                                        else
                                        {
                                            dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm"] = "----";
                                        }
                                    }
                                }
                            }
                        }
                        // If Cumilative Power On/Off are present then caculate both delta via respectively 
                        else if (isCumPowerOnDuration && isCumulativePowerOffDuration)
                        {
                            powerOnDurationDataSet = GetBillingWisePowerOnFromCumulativePowerOn(powerOnDurationDataSet);
                            powerOnDurationDataSet = GetBillingWisePowerOnFromCumulativePowerOff(powerOnDurationDataSet);
                            for (rowCount = 0; rowCount < powerOnDurationDataSet.Tables[0].Rows.Count; rowCount++)
                            {
                                dummyTable.Rows[rowCount][1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][1]));
                                if (chkPowerOnOffDurationFormat == "1")
                                {
                                    dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDDDHH(
                                   TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][2]))
                                   );

                                    dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDDDHH(
                                        TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][3]))
                                        );
                                }
                                else
                                {                                 
                               
                                   dummyTable.Rows[rowCount][2] = common.ConvertTimSpanToDDHHMM(
                                    TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][2]))
                                    );
                                
									dummyTable.Rows[rowCount][3] = common.ConvertTimSpanToDDHHMM(
                                    TimeSpan.FromMinutes(Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount][3]))
                                    );
                                }
                            }
                        }
                        // If Only Cumilative Power On is present then caculate both delta via this 
                        else if (isCumPowerOnDuration)
                        {
                            powerOnDurationDataSet = GetBillingWisePowerOnFromCumulativePowerOn(powerOnDurationDataSet);
                            dummyTable = GetBillingWisePowerOnFromBillingWisePowerOn(powerOnDurationDataSet, dummyTable);
                        }
                        // If Only Cumilative Power Off is present then caculate both delta via this
                        else if (isCumulativePowerOffDuration)
                        {
                            powerOnDurationDataSet = GetBillingWisePowerOnFromCumulativePowerOff(powerOnDurationDataSet);
                            dummyTable = GetBillingWisePowerOnFromBillingWisePowerOff(powerOnDurationDataSet, dummyTable);
                        }
                        powerOnDataSet.Tables.Add(dummyTable);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerOnDuration(int MeterDataId)", ex);
            }
            return powerOnDataSet;
        }

        /// <summary>
        /// to get billing wise power duration from billing wise power off duration
        /// </summary>
        /// <param name="powerOnDurationDataSet"></param>
        /// <returns></returns>
        public DataTable GetBillingWisePowerOnFromBillingWisePowerOff(DataSet powerOnDurationDataSet, DataTable dummyTable)
        {
            long totalMinutes;
            DataRow firstRow, secondRow = null;
            int rowCount = 0;
            for (rowCount = 0; rowCount < powerOnDurationDataSet.Tables[0].Rows.Count - 1; rowCount++)
            {
                firstRow = powerOnDurationDataSet.Tables[0].Rows[rowCount];
                secondRow = powerOnDurationDataSet.Tables[0].Rows[rowCount + 1];

                dummyTable.Rows[rowCount]["Billing Date Time(0.0.0.1.2.255;3;2)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow["BillingTimeStamp"]));

                totalMinutes = (long)Convert.ToUInt64(firstRow["BillingWisePowerOffDuration"]);
                dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));//dd

                totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                totalMinutes = totalMinutes - (long)Convert.ToUInt64(firstRow["BillingWisePowerOffDuration"]);
                dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));//dd
            }
            dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dddd:hh:mm"] = "----";//
            dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes((long)Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingWisePowerOffDuration"])));//dd
            dummyTable.Rows[rowCount]["Billing Date Time(0.0.0.1.2.255;3;2)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingTimeStamp"].ToString()));
            return dummyTable;
        }

        /// <summary>
        /// to get billing wise power on/off duration from billing wise power on duration
        /// </summary>
        /// <param name="powerOnDurationDataSet"></param>
        /// <returns></returns>
        public DataTable GetBillingWisePowerOnFromBillingWisePowerOn(DataSet powerOnDurationDataSet, DataTable dummyTable)
        {
            long totalMinutes;
            DataRow firstRow, secondRow = null;
            int rowCount = 0;
            for (rowCount = 0; rowCount < powerOnDurationDataSet.Tables[0].Rows.Count - 1; rowCount++)
            {
                firstRow = powerOnDurationDataSet.Tables[0].Rows[rowCount];
                secondRow = powerOnDurationDataSet.Tables[0].Rows[rowCount + 1];

                dummyTable.Rows[rowCount]["Billing Date Time(0.0.0.1.2.255;3;2)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(firstRow["BillingTimeStamp"]));

                totalMinutes = (long)Convert.ToUInt64(firstRow["PowerOnDuration"]);
                dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));

                totalMinutes = (long)(DateUtility.LongToDateTime(Convert.ToInt64(firstRow[1])) - DateUtility.LongToDateTime(Convert.ToInt64(secondRow[1]))).TotalMinutes;
                totalMinutes = totalMinutes - (long)Convert.ToUInt64(firstRow["PowerOnDuration"]);
                dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes(totalMinutes));//dd
            }
            dummyTable.Rows[rowCount]["Power On Duration(0.0.94.91.13.255;3;2) dd:hh:mm"] = common.ConvertTimSpanToDDHHMM(TimeSpan.FromMinutes((long)Convert.ToUInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["PowerOnDuration"])));
            dummyTable.Rows[rowCount]["Power Off Duration(0.0.96.1.217.255;3;2) dddd:hh:mm"] = "----";//dd
            dummyTable.Rows[rowCount]["Billing Date Time(0.0.0.1.2.255;3;2)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingTimeStamp"].ToString()));
            return dummyTable;
        }

        /// <summary>
        /// to get billing wise power duration from cumulative power off duration
        /// </summary>
        /// <param name="powerOnDurationDataSet"></param>
        /// <returns></returns>
        public DataSet GetBillingWisePowerOnFromCumulativePowerOff(DataSet powerOnDurationDataSet)
        {
            ulong powerOffValue;
            ulong previousPowerOffValue = 0;
            for (int rowCount = powerOnDurationDataSet.Tables[0].Rows.Count - 1; rowCount >= 0; rowCount--)
            {
                powerOffValue = Convert.ToUInt32(common.CheckUnit(powerOnDurationDataSet.Tables[0].Rows[rowCount]["PowerOffDuration"].ToString())[0]);
                powerOnDurationDataSet.Tables[0].Rows[rowCount]["BillingWisePowerOffDuration"] = common.GetPowerOffHoursForRollOverData(powerOffValue, previousPowerOffValue);
                previousPowerOffValue = powerOffValue;

            }
            return powerOnDurationDataSet;
        }
        /// <summary>
        /// to get billing wise power duration from cumulative power on duration
        /// </summary>
        /// <param name="powerOnDurationDataSet"></param>
        /// <returns></returns>
        public DataSet GetBillingWisePowerOnFromCumulativePowerOn(DataSet powerOnDurationDataSet)
        {
            ulong powerOnValue;
            ulong previousPowerOnValue = 0;
            for (int rowCount = powerOnDurationDataSet.Tables[0].Rows.Count - 1; rowCount >= 0; rowCount--)
            {
                powerOnValue = Convert.ToUInt32(common.CheckUnit(powerOnDurationDataSet.Tables[0].Rows[rowCount]["CumPowerOnDuration"].ToString())[0]);
                powerOnDurationDataSet.Tables[0].Rows[rowCount]["PowerOnDuration"] = common.GetPowerOffHoursForRollOverData(powerOnValue, previousPowerOnValue);
                previousPowerOnValue = powerOnValue;
            }
            return powerOnDurationDataSet;
        }
        /// <summary>
        /// Used to Get LoadFactor
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetLoadFactor(int MeterDataId)
        {
            DataSet loadFactorInputData = dLMS650BillingDAL.GetLoadFactorInputData(MeterDataId);
            DataSet formattedLoadFactorInputData = common.GetFormattedLoadFactorInput(loadFactorInputData);
            return GetLoadFactorFromFormattedInput(formattedLoadFactorInputData);
        }
        /// <summary>
        /// Get Power On Duration 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetPowerOnDuration(long meterDataID)
        {
            DataSet billingWisePowerOffDuration = dLMS650BillingDAL.GetBillingWisePowerOffDuration(meterDataID);
            return common.GetPowerOnDuration(billingWisePowerOffDuration);
        }
        /// <summary>
        /// Used to Get Billing Average LoadFactor
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingAverageLoadFactor(long meterDataId)
        {           

            DataSet loadFactor = dLMS650BillingDAL.GetBillingAverageLoadFactor(meterDataId);
            return common.ConvertLoadFactorData(loadFactor);
        }
      
        
        /// <summary>
        /// Used to Get Billing Average Load
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingAverageLoad(long meterDataId)
        {
            DataSet averageLoad = dLMS650BillingDAL.GetBillingAverageLoad(meterDataId);
            return common.ConvertAverageLoadData(averageLoad);
        }


        /// <summary>
        /// Story - 365876 - Load Factor calculation
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        //public DataSet GetBillingAverageLoadFactorCalculated(long meterDataId)

        //{
        //    DataSet loadFactor = null;
        //    DataSet dsPowerOnHours = null;
        //    try
        //    {
        //        dsPowerOnHours = GetPowerOnDuration(Convert.ToInt32(meterDataId));// Get the Power ON Hours

        //        loadFactor = common.GetFormattedLoadFactorData(dLMS650BillingDAL.GetLoadFactorInputData(meterDataId), dsPowerOnHours, Convert.ToInt32(meterDataId));// Get the MD and KWH Consumption


        //        DLMS650BillingDAL ds = new DLMS650BillingDAL();
        //        DataSet catagory = ds.GetMeterCategory(meterDataId);

        //        foreach (DataRow dataRow in catagory.Tables[0].Rows)
        //        {
        //            meter_cat = Convert.ToString(dataRow["Category"]);
        //        }
        //        if (meter_cat == "B8")
        //        {
        //            loadFactor = GetLoadFactorFromFormattedInput_MeterCategory(loadFactor); // Calculate Load Factor
        //        }
        //        else
        //        {
        //            loadFactor = GetLoadFactorFromFormattedInput(loadFactor); // Calculate Load Factor
        //        }

                
        //    }
        //    catch (Exception ex)    //Exception log for catch block
        //    {
        //        logger.Log(LOGLEVELS.Error, "GetBillingAverageLoadFactorCalculated(long meterDataId)", ex);
        //        loadFactor = null;
        //        dsPowerOnHours = null;
        //    }
        //    finally
        //    {
        //        dsPowerOnHours = null;
        //    }
        //    return loadFactor;
        //}

        public DataSet GetBillingAverageLoadFactorCalculated(long meterDataId)
        {
            DataSet loadFactor = null;
            DataSet dsPowerOnHours = null;
            try
            {
                dsPowerOnHours = GetPowerOnDuration(Convert.ToInt32(meterDataId));// Get the Power ON Hours

                loadFactor = common.GetFormattedLoadFactorData(dLMS650BillingDAL.GetLoadFactorInputData(meterDataId), dsPowerOnHours, Convert.ToInt32(meterDataId));// Get the MD and KWH Consumption


                DLMS650BillingDAL ds = new DLMS650BillingDAL();
                DataSet catagory = ds.GetMeterCategory(meterDataId);

                foreach (DataRow dataRow in catagory.Tables[0].Rows)
                {
                    meter_cat = Convert.ToString(dataRow["Category"]);
                }
                if (meter_cat == "B8" || meter_cat == "B2")
                {
                    loadFactor = GetLoadFactorFromFormattedInput_MeterCategory(loadFactor); // Calculate Load Factor
                }
                else
                {
                    loadFactor = GetLoadFactorFromFormattedInput(loadFactor); // Calculate Load Factor
                }

                
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingAverageLoadFactorCalculated(long meterDataId)", ex);
                loadFactor = null;
                dsPowerOnHours = null;
            }
            finally
            {
                dsPowerOnHours = null;
            }
            return loadFactor;
        }     
        /// <summary>
        /// Takes input as dataset having kwhConsumption,BillingPowerOnHours,HistoryID and MDKW
        /// and returns data set having history Id and corresponsing load factor .
        /// </summary>
        /// <param name="loadFactorInput"></param>
        /// <returns></returns>
        public DataSet GetLoadFactorFromFormattedInput(DataSet loadFactorInput)
        {
            DataSet dataSet = null; // Story - 365876 - Load Factor in billing
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(BCSConstants.History, typeof(System.String)));// Story - 365876 - Load Factor calculation
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
            //pradipta_start_081018

            if (meter_cat == "C2")
            {
                table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForImportkW, typeof(System.String)));
                table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForExportkW, typeof(System.String)));
                table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForImportkVA, typeof(System.String)));
                table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForExportkVA, typeof(System.String)));
            }
            else
            {
                table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForSLG, typeof(System.String)));
            }
            //pradipta_start_081018

           // table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForSLG, typeof(System.String)));
            
            try
            {
                if (loadFactorInput != null && loadFactorInput.Tables != null && loadFactorInput.Tables.Count > 0)
                {
                    dataSet = new DataSet();
                    DataRow row;
                    //Current
                    row = table.NewRow();
                    foreach (DataRow dataRow in loadFactorInput.Tables[0].Rows)
                    {
                        row = table.NewRow();
                        if (Convert.ToInt32(dataRow[0]) == 0)
                        {
                            continue;
                            //row[0] = "Current";
                        }
                        else
                        {
                            row[0] = "History - " + Convert.ToInt32(dataRow[0]).ToString();
                        }

                        if (meter_cat == "B8" || meter_cat == "B2")
                        {
                            row[1] = dataRow[5];
                            List<string> Loadfactor = new List<string>();
                            Loadfactor = CalculateLoadFactor1(Convert.ToDouble(dataRow[1]), dataRow[2].ToString(), dataRow[3].ToString(), dataRow[4].ToString(), dataRow[5].ToString());
                            row[2] = Convert.ToString(Loadfactor[0]) + "";
                            row[3] = Convert.ToString(Loadfactor[1]) + "";
                        }
                        else
                        {
                            row[1] = dataRow[4];
                            row[2] = CalculateLoadFactor(Convert.ToDouble(dataRow[1]), dataRow[2].ToString(), dataRow[3].ToString());
                        }
                     

                        table.Rows.Add(row);
                    }
                    dataSet.Tables.Add(table);

                    
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadFactorFromFormattedInput(DataSet loadFactorInput)", ex);
				dataSet=null;
            }
            return dataSet;
        }

        public DataSet GetLoadFactorFromFormattedInput_MeterCategory(DataSet loadFactorInput)
        {
            DataSet dataSet = null; // Story - 365876 - Load Factor in billing
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(BCSConstants.History, typeof(System.String)));// Story - 365876 - Load Factor calculation
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
            

            table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForImportNet, typeof(System.String)));

            table.Columns.Add(new DataColumn(BCSConstants.LoadFactorColumnForExportNet, typeof(System.String)));

            try
            {
                if (loadFactorInput != null && loadFactorInput.Tables != null && loadFactorInput.Tables.Count > 0)
                {
                    dataSet = new DataSet();
                    DataRow row;
                    //Current
                    row = table.NewRow();
                    foreach (DataRow dataRow in loadFactorInput.Tables[0].Rows)
                    {
                        row = table.NewRow();
                        if (Convert.ToInt32(dataRow[0]) == 0)
                        {
                            continue;
                            //row[0] = "Current";
                        }
                        else
                        {
                            row[0] = "History - " + Convert.ToInt32(dataRow[0]).ToString();
                        }
                        row[1] = dataRow[6];// add pradipta  dataRow[5]
                        //  row[2] = CalculateLoadFactor(Convert.ToDouble(dataRow[1]), dataRow[2].ToString(), dataRow[3].ToString());
                        List<string> Loadfactor = new List<string>();
                        //row[2] = CalculateLoadFactor(Convert.ToDouble(dataRow[1]), dataRow[2].ToString(), dataRow[3].ToString(), dataRow[4].ToString());
                        Loadfactor = CalculateLoadFactor1(Convert.ToDouble(dataRow[1]), dataRow[2].ToString(), dataRow[3].ToString(), dataRow[4].ToString(), dataRow[5].ToString());

                        row[2] = Convert.ToString(Loadfactor[0]) + "";
                        row[3] = Convert.ToString(Loadfactor[1]) + "";



                        table.Rows.Add(row);
                    }
                    dataSet.Tables.Add(table);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadFactorFromFormattedInput_MeterCategory(DataSet loadFactorInput)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// Used to calculate Load factor from kwhConsumption,BillingPowerOnHours and MDKW.
        /// using formula 
        /// Billing load Factor = ((kwh Consumption History(1....n) / (*Power ON Hrs. History(1...n))) / MD History(1...n);
        /// </summary>
        /// <param name="kWHConsumption"></param>
        /// <param name="powerOnHours"></param>
        /// <param name="mdKW"></param>
        /// <returns></returns>
        private string CalculateLoadFactor(double powerOnHours, string kWHConsumption, string mdKW)
        {
            string loadFactor = "0.00"; // Story - 365876 - default value for load factor billing is 0
            try
            {
                if (!string.IsNullOrEmpty(kWHConsumption) && powerOnHours != 0 && !string.IsNullOrEmpty(mdKW))
                {
                    double kWhConsumption = Convert.ToDouble(kWHConsumption);
                   
                    double maximumDemandKW = Convert.ToDouble(mdKW);
                    if (kWhConsumption != 0 && maximumDemandKW != 0)
                    {
                        loadFactor = string.Format("{0:N2}", Math.Truncate(((kWhConsumption / powerOnHours) / maximumDemandKW) * 100) / 100);
                        if (Convert.ToDouble(loadFactor) <= 0) loadFactor = "0.00";
                        if (Convert.ToDouble(loadFactor) >= 1) loadFactor = "1.00";
                    }

                    
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CalculateLoadFactor(double powerOnHours, string kWHConsumption, string mdKW)", ex);
            }
            return loadFactor;
        }

        private List<string> CalculateLoadFactor1(double powerOnHours, string kWHConsumption, string ExportkWHConsumption, string mdKW, string mdKWExport)
       {
           List<string> loadFactor = new List<string>() { "0.00", "0.00" }; // Story - 365876 - default value for load factor billing is 0
          // List<string> Export_loadFactor = new List<string>() { "0.00", "0.00" }; // Story - 365876 - default value for load factor billing is 0
         


           try
           {
               if (!string.IsNullOrEmpty(kWHConsumption) && powerOnHours != 0 && !string.IsNullOrEmpty(mdKW))
               {
                   //double kWhConsumption = Convert.ToDouble(kWHConsumption);
                   double kWhConsumption = Convert.ToDouble(kWHConsumption);
                   double ExportkWhConsumption = Convert.ToDouble(ExportkWHConsumption);

                   double maximumDemandKW = Convert.ToDouble(mdKW);
                   double maximumDemandKWExport = Convert.ToDouble(mdKWExport);//add pradipta_exportloadfactor

                   if (kWhConsumption != 0 && maximumDemandKW != 0)
                   {
                       // loadFactor = string.Format("{0:N2}", Math.Truncate(((kWhConsumption / powerOnHours) / maximumDemandKW) * 100) / 100);

                       loadFactor[0]= string.Format("{0:N2}", Math.Truncate(((kWhConsumption / powerOnHours) / maximumDemandKW) * 100) / 100);


                       //if (Convert.ToDouble(loadFactor) <= 0) loadFactor = "0.00";
                       //if (Convert.ToDouble(loadFactor) >= 1) loadFactor = "1.00";

                      

                       //if (import_loadfactor <= 0) loadFactor.Add("0.00");
                       //if (import_loadfactor >= 1) loadFactor.Add("1.00");
                   }

                   if (ExportkWhConsumption != 0 && maximumDemandKWExport != 0)
                   {
                       // loadFactor = string.Format("{0:N2}", Math.Truncate(((kWhConsumption / powerOnHours) / maximumDemandKW) * 100) / 100);

                       loadFactor[1] = string.Format("{0:N2}", Math.Truncate(((ExportkWhConsumption / powerOnHours) / maximumDemandKWExport) * 100) / 100);


                       //if (Convert.ToDouble(loadFactor) <= 0) loadFactor = "0.00";
                       //if (Convert.ToDouble(loadFactor) >= 1) loadFactor = "1.00";
                      


                       //if (export_loadfactor <= 0) loadFactor.Add("0.00");
                       //if (export_loadfactor >= 1) loadFactor.Add("1.00");
                   }
               }
           }
           catch (Exception ex)    //Exception log for catch block
           {
               logger.Log(LOGLEVELS.Error, "CalculateLoadFactor1(double powerOnHours, string kWHConsumption, string ExportkWHConsumption, string mdKW)", ex);
           }
           return loadFactor;
       }



        /// <summary>
        /// Takes input as dataset having kwhConsumption,BillingPowerOnHours,HistoryID
        /// and returns data set having history Id and corresponsing Average load .
        /// Billing Average load = ((kwh Consumption History(1....n) / (*Power ON Hrs. History(1...n)))
        /// </summary>
        /// <param name="loadFactorInput"></param>
        /// <returns></returns>
        public DataSet GetAverageLoadFromFormattedInput(DataSet averageLoadInput)
        {
            DataSet dataSet = null; 
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn(BCSConstants.History, typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
            table.Columns.Add(new DataColumn(CommonMethods.getDisplayHeaderText(BCSConstants.AverageLoadColumn), typeof(System.String)));
            
            try
            {
                if (averageLoadInput != null && averageLoadInput.Tables != null && averageLoadInput.Tables.Count > 0)
                {
                    dataSet = new DataSet();
                    DataRow row;
                    //Current
                    row = table.NewRow();
                    foreach (DataRow dataRow in averageLoadInput.Tables[0].Rows)
                    {
                        row = table.NewRow();
                        if (Convert.ToInt32(dataRow[0]) == 0)
                        {
                            continue;
                            //row[0] = "Current";
                        }
                        else
                        {
                            row[0] = "History - " + Convert.ToInt32(dataRow[0]).ToString();
                        }
                        row[1] = dataRow[4];
                        //row[2] = CalculateAverageLoad(Convert.ToDouble(dataRow[1]), dataRow[2].ToString());
                        table.Rows.Add(row);
                    }
                    dataSet.Tables.Add(table);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAverageLoadFromFormattedInput(DataSet averageLoadInput)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// Used to get billing count for meterDataId.
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public int GetBillingCount(long meterDataId)
        {
            return dLMS650BillingDAL.GetBillingCount(meterDataId);
        }


        /// <summary>
        /// get TOD Avg Power Factor
        /// </summary>
        /// <param name="MeterDataId"></param>
        /// <returns></returns>
        public DataSet GetTODAvgPF(long MeterDataId, int historyId, bool isHistory)
        {
            DataSet ds = dLMS650BillingDAL.GetTODAvgPFMeterData(MeterDataId, historyId);
            return common.ConvertTODAvgPFToColumnForRPT(ds, isHistory, MeterDataId);
        }

        public DataSet GetDataByMeterID(long MeterDataId, string billingParameters)
        {
            return dLMS650BillingDAL.GetDataByMeterID(MeterDataId, billingParameters);
        }

        public string GetColumnNames(long MeterDataId)
        {
            string ColumnNames = string.Empty;
            DataSet ds = dLMS650BillingDAL.GetColumnNames(MeterDataId);
            ColumnNames = RetriveColumns(ds);

            return ColumnNames;
        }

        public DataSet  GetMeterCategory(long  MeterDataId)//add pradipta_loadfactor
        {
            string ColumnNames = string.Empty;
            DataSet ds = dLMS650BillingDAL.GetMeterCategory(MeterDataId);
            return ds;
        }

        private string RetriveColumns(DataSet ds)
        {
            string ColumnNames = string.Empty;
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[dc.ColumnName])))
                        {
                            ColumnNames = ColumnNames + "," + dc.ColumnName;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                ColumnNames = ColumnNames.TrimStart(',');
            }
            return ColumnNames;
        }
        
    }
}
