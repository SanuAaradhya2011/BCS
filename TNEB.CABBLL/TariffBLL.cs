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

namespace CAB.BLL
{
    public class TariffBLL : IBLL
    {
        private Dictionary<string, string> tariffColumns = new Dictionary<string, string>();
        TariffDAL tariffDAL;

        public TariffBLL()
        {
            tariffDAL = new TariffDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return tariffDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return tariffDAL.InsertData(entities);
        }
        public DataSet GetMeterData(long MeterDataId,int historyId)
        {
            DataSet ds = tariffDAL.GetMeterData(MeterDataId, historyId);
            return CommonBLL.ConvertTariffEnergyToColumn(ds);
        }
        public DataSet GetTODConsumption(int MeterDataId, int historyId)
        {
            DataSet history1 = CommonBLL.ConvertTariffEnergyToColumn(tariffDAL.GetMeterData(MeterDataId, historyId));
            DataSet history2 = CommonBLL.ConvertTariffEnergyToColumn(tariffDAL.GetMeterData(MeterDataId, historyId+1));
            return CommonBLL.ConvertTODConsumptionToColumn(history1, history2);
        }
        
        public DataSet GetTODMDMeterData(long MeterDataId, int historyId)
        {
            DataSet ds = tariffDAL.GetTODMDMeterData(MeterDataId, historyId);
            return CommonBLL.ConvertTariffEnergyTODMDToColumn(ds);
        }
        public DataSet GetTariffAveragePowerFactor(int MeterDataId, int historyId)
        {
            DataSet ds = tariffDAL.GetTariffAveragePowerFactor(MeterDataId, historyId);
            return CommonBLL.ConvertTariffAveragePowerFactorToColumn(ds);
        }
        public DataSet ListDataSet(long MeterDataId)
        {
            return tariffDAL.ListDataSet(MeterDataId);
        }

        public Dictionary<string, string> CreateTariffDictionary()
        {
            string colAlias = "ti";
            tariffColumns.Add("Tariff 1 kWh", string.Concat(colAlias, ".", "Tariff1_kWh"));
            tariffColumns.Add("Tariff 1 kVAh", string.Concat(colAlias, ".", "Tariff1_kVAh"));
            tariffColumns.Add("Tariff 1 kVArh Lag", string.Concat(colAlias, ".", "Tariff1_kVARh_lag"));
            tariffColumns.Add("Tariff 1 kVArh Lead", string.Concat(colAlias, ".", "Tariff1_kVARh_lead"));
            tariffColumns.Add("Tariff 1 MD1", string.Concat(colAlias, ".", "Tariff1_MD1"));
            tariffColumns.Add("Tariff 1 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff1_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 1 MD2", string.Concat(colAlias, ".", "Tariff1_MD2"));
            tariffColumns.Add("Tariff 1 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff1_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 1 MD3", string.Concat(colAlias, ".", "Tariff1_MD3"));
            //tariffColumns.Add("Tariff 1 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff1_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 1 Average PF", string.Concat(colAlias, ".", "Tariff1_Aver_PF"));
            tariffColumns.Add("Tariff 2 kWh", string.Concat(colAlias, ".", "Tariff2_kWh"));
            tariffColumns.Add("Tariff 2 kVAh", string.Concat(colAlias, ".", "Tariff2_kVAh"));
            tariffColumns.Add("Tariff 2 kVArh Lag", string.Concat(colAlias, ".", "Tariff2_kVARh_lag"));
            tariffColumns.Add("Tariff 2 kVArh Lead", string.Concat(colAlias, ".", "Tariff2_kVARh_lead"));
            tariffColumns.Add("Tariff 2 MD1", string.Concat(colAlias, ".", "Tariff2_MD1"));
            tariffColumns.Add("Tariff 2 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff2_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 2 MD2", string.Concat(colAlias, ".", "Tariff2_MD2"));
            tariffColumns.Add("Tariff 2 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff2_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 2 MD3", string.Concat(colAlias, ".", "Tariff2_MD3"));
            //tariffColumns.Add("Tariff 2 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff2_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 2 Average PF", string.Concat(colAlias, ".", "Tariff2_Aver_PF"));
            tariffColumns.Add("Tariff 3 kWh", string.Concat(colAlias, ".", "Tariff3_kWh"));
            tariffColumns.Add("Tariff 3 kVAh", string.Concat(colAlias, ".", "Tariff3_kVAh"));
            tariffColumns.Add("Tariff 3 kVArh Lag", string.Concat(colAlias, ".", "Tariff3_kVARh_lag"));
            tariffColumns.Add("Tariff 3 kVArh Lead", string.Concat(colAlias, ".", "Tariff3_kVARh_lead"));
            tariffColumns.Add("Tariff 3 MD1", string.Concat(colAlias, ".", "Tariff3_MD1"));
            tariffColumns.Add("Tariff 3 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff3_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 3 MD2", string.Concat(colAlias, ".", "Tariff3_MD2"));
            tariffColumns.Add("Tariff 3 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff3_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 3 MD3", string.Concat(colAlias, ".", "Tariff3_MD3"));
            //tariffColumns.Add("Tariff 3 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff3_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 3 Average PF", string.Concat(colAlias, ".", "Tariff3_Aver_PF"));
            tariffColumns.Add("Tariff 4 kWh", string.Concat(colAlias, ".", "Tariff4_kWh"));
            tariffColumns.Add("Tariff 4 kVAh", string.Concat(colAlias, ".", "Tariff4_kVAh"));
            tariffColumns.Add("Tariff 4 kVArh Lag", string.Concat(colAlias, ".", "Tariff4_kVARh_lag"));
            tariffColumns.Add("Tariff 4 kVArh Lead", string.Concat(colAlias, ".", "Tariff4_kVARh_lead"));
            tariffColumns.Add("Tariff 4 MD1", string.Concat(colAlias, ".", "Tariff4_MD1"));
            tariffColumns.Add("Tariff 4 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff4_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 4 MD2", string.Concat(colAlias, ".", "Tariff4_MD2"));
            tariffColumns.Add("Tariff 4 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff4_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 4 MD3", string.Concat(colAlias, ".", "Tariff4_MD3"));
            //tariffColumns.Add("Tariff 4 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff4_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 4 Average PF", string.Concat(colAlias, ".", "Tariff4_Aver_PF"));
            tariffColumns.Add("Tariff 5 kWh", string.Concat(colAlias, ".", "Tariff5_kWh"));
            tariffColumns.Add("Tariff 5 kVAh", string.Concat(colAlias, ".", "Tariff5_kVAh"));
            tariffColumns.Add("Tariff 5 kVArh Lag", string.Concat(colAlias, ".", "Tariff5_kVARh_lag"));
            tariffColumns.Add("Tariff 5 kVArh Lead", string.Concat(colAlias, ".", "Tariff5_kVARh_lead"));
            tariffColumns.Add("Tariff 5 MD1", string.Concat(colAlias, ".", "Tariff5_MD1"));
            tariffColumns.Add("Tariff 5 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff5_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 5 MD2", string.Concat(colAlias, ".", "Tariff5_MD2"));
            tariffColumns.Add("Tariff 5 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff5_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 5 MD3", string.Concat(colAlias, ".", "Tariff5_MD3"));
            //tariffColumns.Add("Tariff 5 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff5_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 5 Average PF", string.Concat(colAlias, ".", "Tariff5_Aver_PF"));
            tariffColumns.Add("Tariff 6 kWh", string.Concat(colAlias, ".", "Tariff6_kWh"));
            tariffColumns.Add("Tariff 6 kVAh", string.Concat(colAlias, ".", "Tariff6_kVAh"));
            tariffColumns.Add("Tariff 6 kVArh Lag", string.Concat(colAlias, ".", "Tariff6_kVARh_lag"));
            tariffColumns.Add("Tariff 6 kVArh Lead", string.Concat(colAlias, ".", "Tariff6_kVARh_lead"));
            tariffColumns.Add("Tariff 6 MD1", string.Concat(colAlias, ".", "Tariff6_MD1"));
            tariffColumns.Add("Tariff 6 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff6_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 6 MD2", string.Concat(colAlias, ".", "Tariff6_MD2"));
            tariffColumns.Add("Tariff 6 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff6_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 6 MD3", string.Concat(colAlias, ".", "Tariff6_MD3"));
            //tariffColumns.Add("Tariff 6 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff6_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 6 Average PF", string.Concat(colAlias, ".", "Tariff6_Aver_PF"));
            tariffColumns.Add("Tariff 7 kWh", string.Concat(colAlias, ".", "Tariff7_kWh"));
            tariffColumns.Add("Tariff 7 kVAh", string.Concat(colAlias, ".", "Tariff7_kVAh"));
            tariffColumns.Add("Tariff 7 kVArh Lag", string.Concat(colAlias, ".", "Tariff7_kVARh_lag"));
            tariffColumns.Add("Tariff 7 kVArh Lead", string.Concat(colAlias, ".", "Tariff7_kVARh_lead"));
            tariffColumns.Add("Tariff 7 MD1", string.Concat(colAlias, ".", "Tariff7_MD1"));
            tariffColumns.Add("Tariff 7 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff7_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 7 MD2", string.Concat(colAlias, ".", "Tariff7_MD2"));
            tariffColumns.Add("Tariff 7 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff7_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 7 MD3", string.Concat(colAlias, ".", "Tariff7_MD3"));
            //tariffColumns.Add("Tariff 7 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff7_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 7 Average PF", string.Concat(colAlias, ".", "Tariff7_Aver_PF"));
            tariffColumns.Add("Tariff 8 kWh", string.Concat(colAlias, ".", "Tariff8_kWh"));
            tariffColumns.Add("Tariff 8 kVAh", string.Concat(colAlias, ".", "Tariff8_kVAh"));
            tariffColumns.Add("Tariff 8 kVArh Lag", string.Concat(colAlias, ".", "Tariff8_kVARh_lag"));
            tariffColumns.Add("Tariff 8 kVArh Lead", string.Concat(colAlias, ".", "Tariff8_kVARh_lead"));
            tariffColumns.Add("Tariff 8 MD1", string.Concat(colAlias, ".", "Tariff8_MD1"));
            tariffColumns.Add("Tariff 8 MD1 TimeStamp", string.Concat(colAlias, ".", "Tariff8_MD1_TimeStamp"));
            tariffColumns.Add("Tariff 8 MD2", string.Concat(colAlias, ".", "Tariff8_MD2"));
            tariffColumns.Add("Tariff 8 MD2 TimeStamp", string.Concat(colAlias, ".", "Tariff8_MD2_TimeStamp"));
            //tariffColumns.Add("Tariff 8 MD3", string.Concat(colAlias, ".", "Tariff8_MD3"));
            //tariffColumns.Add("Tariff 8 MD3 Timestamp", string.Concat(colAlias, ".", "Tariff8_MD3_TimeStamp"));
            tariffColumns.Add("Tariff 8 Average PF", string.Concat(colAlias, ".", "Tariff8_Aver_PF"));
            return tariffColumns;
        }

        public bool DeleteData(long meterDataId)
        {
            return tariffDAL.DeleteData(meterDataId);
        }

        public DataSet GetTariffPF(long activeMeterDataId)
        {
            return tariffDAL.GetTariffPF(activeMeterDataId);
        }
    }
}