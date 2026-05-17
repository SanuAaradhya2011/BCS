
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using System.Collections.Generic;
using LTCTBLL;
using CAB.Entity;
namespace CAB.BLL
{
    public class BillingBLL : IBLL
    {
        BillingDAL billingDAL;
        
        public BillingBLL()
        {

            billingDAL = new BillingDAL(UtilityDetails.UtilityName);
        }
        public IEntity InsertData(IEntity entity)
        {
            return billingDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return billingDAL.InsertData(entities);
        }
        public DataSet GetMaximumDemand(int MeterDataId)
        {
            DataSet ds = billingDAL.GetMaximumDemand(MeterDataId);
            return CommonBLL.ConvertMaximumDemandToColumn(ds);
        }
        public DataSet GetCumulativeEnergy(int MeterDataId)
        {
            DataSet ds = billingDAL.GetCumulativeEnergy(MeterDataId);
            return CommonBLL.ConvertCumulativeEnergyToColumn(ds);
        }
        public DataSet GetCumulativeEnergyTNEB(int MeterDataId)
        {
            DataSet ds = billingDAL.GetCumulativeEnergyTNEB(MeterDataId);
            return CommonBLL.ConvertCumulativeEnergyToColumnTNEB(ds);
        }

        public DataSet GetCumulativeEnergyCalculated(int MeterDataId)
        {
            DataSet ds = GetCumulativeEnergy(MeterDataId);
            return CommonBLL.ConvertCumulativeEnergyCalculatedToColumn(ds);
        }

        public DataSet GetPowerOnHour(int MeterDataId)
        {
            DataSet ds = billingDAL.GetPowerOnHour(MeterDataId);
            return CommonBLL.ConvertPowerOnHour(ds);
        }
        public DataSet GetAveragePowerFactor(int MeterDataId)
        {
            DataSet ds = billingDAL.GetAveragePowerFactor(MeterDataId);
            return CommonBLL.ConvertHistoryWithSingleColumn(ds, "Values",true);
        }
        public DataSet GetLoadFactor(int MeterDataId)
        {
            DataSet ds = billingDAL.GetLoadFactor(MeterDataId);
            return CommonBLL.ConvertHistoryWithSingleColumn(ds, "Values (%)",true );
        }
        public DataSet GetCTRatio(int MeterDataId)
        {
            DataSet ds = billingDAL.GetCTRatio(MeterDataId);
            return CommonBLL.ConvertCTRatioColumn(ds, "Values");
        }
        public DataSet TamperOccurRestore(long MeterDataId)
        {
            return CommonBLL.ConvertTamperOccurRestore(MeterDataId);
        }
        public DataSet TamperOccurRestore(long MeterDataId,long tamperId)
        {
            return CommonBLL.ConvertTamperOccurRestore(MeterDataId, tamperId);
        }
        // Added for TNEB
        public DataSet TamperOccurRestoreTNEB(DataSet ds)
        {
            return CommonBLL.ConvertTamperOccurRestoreTNEB(ds);
        }


        public Dictionary<string, string> InitializeBillingDictionary()
        {
            Dictionary<string, string> initialBillingColumns = new Dictionary<string, string>();
            string colAlias = "b";
            initialBillingColumns.Add("Billing Mechanism", string.Concat(colAlias, ".", "CTRatio"));
            initialBillingColumns.Add("Cumulative kWh", string.Concat(colAlias, ".", "CumulativeEnergyKWH"));
            initialBillingColumns.Add("Cumulative kvarh (lag)", string.Concat(colAlias, ".", "CumulativeEnergyKVARHLag"));
            initialBillingColumns.Add("Cumulative kvarh (lead)", string.Concat(colAlias, ".", "CumulativeEnergyKVARHLead"));
            initialBillingColumns.Add("Cumulative kVAh", string.Concat(colAlias, ".", "CumulativeEnergyKVAH"));
            initialBillingColumns.Add("Current MD1 (kW)", string.Concat(colAlias, ".", "CumulativeMD1"));
            initialBillingColumns.Add("Current MD1 TimeStamp", string.Concat(colAlias, ".", "CumulativeMD1TimeStamp"));
            initialBillingColumns.Add("Current MD2 (kVA)", string.Concat(colAlias, ".", "CumulativeMD2"));
            initialBillingColumns.Add("Current MD2 TimeStamp", string.Concat(colAlias, ".", "CumulativeMD2TimeStamp"));
            //initialBillingColumns.Add("Current MD3", string.Concat(colAlias, ".", "CumulativeMD3"));
            //initialBillingColumns.Add("Current MD3 TimeStamp", string.Concat(colAlias, ".", "CumulativeMD3TimeStamp"));
            if (UtilityDetails.ShowPowerOnHoursInMinutes)
            {
                initialBillingColumns.Add("Power On Hours (HH:MM)", string.Concat(colAlias, ".", "PowerOnHours"));
            }
            else
            {
                initialBillingColumns.Add("Power On Hours", string.Concat(colAlias, ".", "PowerOnHours"));
            }
            initialBillingColumns.Add("Average Power Factor", string.Concat(colAlias, ".", "AveragePowerFactor"));
            initialBillingColumns.Add("Load Factor", string.Concat(colAlias, ".", "LoadFactor"));
            //If Utility is WBEXPORTVCL. then add two export parameters
            if (UtilityDetails.UtilityName == IECUtilityEntity.WBEXPORTVCL)
            {
                initialBillingColumns.Add("Cumulative kWh (E)", string.Concat(colAlias, ".", "CumulativeExportEnergyKWH"));
                initialBillingColumns.Add("Cumulative kVAh (E)", string.Concat(colAlias, ".", "CumulativeExportEnergyKVAH"));
            }
            return initialBillingColumns;
        }

        public Dictionary<string, string> CreateBillingDictionary()
        {
            Dictionary<string, string> billingDictionary = new Dictionary<string, string>();
            Dictionary<string, string> tariffDictionary = new Dictionary<string, string>();
            Dictionary<string, string> tamperCounterDictionary = new Dictionary<string, string>();
            billingDictionary = new BillingBLL().InitializeBillingDictionary();
            tariffDictionary = new TariffBLL().CreateTariffDictionary();
            tamperCounterDictionary = new TamperGeneralBLL().CreateTamperCounterDictionary();
            billingDictionary = new ReportBLL().MergeDictionary(billingDictionary, tariffDictionary, tamperCounterDictionary);
            return billingDictionary;
        }

        public DataSet GetBillingDataByParameter(string value, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = billingDAL.GetBillingDataByFile(value, columns);
            else if (reportType == "Meter")
                ds = billingDAL.GetBillingDataByMeter(value, columns);

            return ds;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            Dictionary<string, string> billingColumns = CreateBillingDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (billingColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }

        public bool DeleteData(long meterDataId)
        {
            return billingDAL.DeleteData(meterDataId);
        }
    }
}
