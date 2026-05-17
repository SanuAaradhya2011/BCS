 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |All rights reserved to Cabcon Technologies  |
 * | |
 * |Author : Piyush Singh.  |
 * | |
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

namespace CAB.DALC.Data
{
    public class TariffDAL : DALBase
    {
        private string Tariff_ID ="Tariff_ID";
        private string HistoryID ="HistoryID";
        private string Tariff1_kWh ="Tariff1_kWh";
        private string Tariff1_kVAh ="Tariff1_kVAh";
        private string Tariff1_kVARh_lag ="Tariff1_kVARh_lag";
        private string Tariff1_kVARh_lead ="Tariff1_kVARh_lead";
        private string Tariff1_MD1 ="Tariff1_MD1";
        private string Tariff1_MD1_TimeStamp ="Tariff1_MD1_TimeStamp";
        private string Tariff1_MD2 ="Tariff1_MD2";
        private string Tariff1_MD2_TimeStamp ="Tariff1_MD2_TimeStamp"; 
        private string Tariff1_Aver_PF ="Tariff1_Aver_PF";
        private string Tariff2_kWh ="Tariff2_kWh";
        private string Tariff2_kVAh ="Tariff2_kVAh";
        private string Tariff2_kVARh_lag ="Tariff2_kVARh_lag";
        private string Tariff2_kVARh_lead ="Tariff2_kVARh_lead";
        private string Tariff2_MD1 ="Tariff2_MD1";
        private string Tariff2_MD1_TimeStamp ="Tariff2_MD1_TimeStamp";
        private string Tariff2_MD2 ="Tariff2_MD2";
        private string Tariff2_MD2_TimeStamp ="Tariff2_MD2_TimeStamp"; 
        private string Tariff2_Aver_PF ="Tariff2_Aver_PF";
        private string Tariff3_kWh ="Tariff3_kWh";
        private string Tariff3_kVAh ="Tariff3_kVAh";
        private string Tariff3_kVARh_lag ="Tariff3_kVARh_lag";
        private string Tariff3_kVARh_lead ="Tariff3_kVARh_lead";
        private string Tariff3_MD1 ="Tariff3_MD1";
        private string Tariff3_MD1_TimeStamp ="Tariff3_MD1_TimeStamp";
        private string Tariff3_MD2 ="Tariff3_MD2";
        private string Tariff3_MD2_TimeStamp ="Tariff3_MD2_TimeStamp"; 
        private string Tariff3_Aver_PF ="Tariff3_Aver_PF";
        private string Tariff4_kWh ="Tariff4_kWh";
        private string Tariff4_kVAh ="Tariff4_kVAh";
        private string Tariff4_kVARh_lag ="Tariff4_kVARh_lag";
        private string Tariff4_kVARh_lead ="Tariff4_kVARh_lead";
        private string Tariff4_MD1 ="Tariff4_MD1";
        private string Tariff4_MD1_TimeStamp ="Tariff4_MD1_TimeStamp";
        private string Tariff4_MD2 ="Tariff4_MD2";
        private string Tariff4_MD2_TimeStamp ="Tariff4_MD2_TimeStamp"; 
        private string Tariff4_Aver_PF ="Tariff4_Aver_PF";
        private string Tariff5_kWh ="Tariff5_kWh";
        private string Tariff5_kVAh ="Tariff5_kVAh";
        private string Tariff5_kVARh_lag ="Tariff5_kVARh_lag";
        private string Tariff5_kVARh_lead ="Tariff5_kVARh_lead";
        private string Tariff5_MD1 ="Tariff5_MD1";
        private string Tariff5_MD1_TimeStamp ="Tariff5_MD1_TimeStamp";
        private string Tariff5_MD2 ="Tariff5_MD2";
        private string Tariff5_MD2_TimeStamp ="Tariff5_MD2_TimeStamp"; 
        private string Tariff5_Aver_PF ="Tariff5_Aver_PF";
        private string Tariff6_kWh ="Tariff6_kWh";
        private string Tariff6_kVAh ="Tariff6_kVAh";
        private string Tariff6_kVARh_lag ="Tariff6_kVARh_lag";
        private string Tariff6_kVARh_lead ="Tariff6_kVARh_lead";
        private string Tariff6_MD1 ="Tariff6_MD1";
        private string Tariff6_MD1_TimeStamp ="Tariff6_MD1_TimeStamp";
        private string Tariff6_MD2 ="Tariff6_MD2";
        private string Tariff6_MD2_TimeStamp ="Tariff6_MD2_TimeStamp"; 
        private string Tariff6_Aver_PF ="Tariff6_Aver_PF";
        private string Tariff7_kWh ="Tariff7_kWh";
        private string Tariff7_kVAh ="Tariff7_kVAh";
        private string Tariff7_kVARh_lag ="Tariff7_kVARh_lag";
        private string Tariff7_kVARh_lead ="Tariff7_kVARh_lead";
        private string Tariff7_MD1 ="Tariff7_MD1";
        private string Tariff7_MD1_TimeStamp ="Tariff7_MD1_TimeStamp";
        private string Tariff7_MD2 ="Tariff7_MD2";
        private string Tariff7_MD2_TimeStamp ="Tariff7_MD2_TimeStamp"; 
        private string Tariff7_Aver_PF ="Tariff7_Aver_PF";
        private string Tariff8_kWh ="Tariff8_kWh";
        private string Tariff8_kVAh ="Tariff8_kVAh";
        private string Tariff8_kVARh_lag ="Tariff8_kVARh_lag";
        private string Tariff8_kVARh_lead ="Tariff8_kVARh_lead";
        private string Tariff8_MD1 ="Tariff8_MD1";
        private string Tariff8_MD1_TimeStamp ="Tariff8_MD1_TimeStamp";
        private string Tariff8_MD2 ="Tariff8_MD2";
        private string Tariff8_MD2_TimeStamp ="Tariff8_MD2_TimeStamp"; 
        private string Tariff8_Aver_PF ="Tariff8_Aver_PF";
        private string MeterData_ID = "MeterData_ID";
       

        public TariffDAL()
            : base("meterdata_tariffinformation","Tariff_ID")
        {
        }
 
        public DataSet GetTariffAveragePowerFactor(int meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Tariff1_Aver_PF,Tariff2_Aver_PF,Tariff3_Aver_PF,Tariff4_Aver_PF,Tariff5_Aver_PF,Tariff6_Aver_PF,Tariff7_Aver_PF,Tariff8_Aver_PF");
                builder.Append(" from meterdata_tariffinformation where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(HistoryID, "=", ParameterName(HistoryID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                request.AddParamter(ParameterName(HistoryID), historyID, DbType.UInt16);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff Energy data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetTODMDMeterData(long meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append("Tariff1_MD1,Tariff1_MD1_TimeStamp,Tariff1_MD2,Tariff1_MD2_TimeStamp,");
                builder.Append("Tariff2_MD1,Tariff2_MD1_TimeStamp,Tariff2_MD2,Tariff2_MD2_TimeStamp,");
                builder.Append("Tariff3_MD1,Tariff3_MD1_TimeStamp,Tariff3_MD2,Tariff3_MD2_TimeStamp,");
                builder.Append("Tariff4_MD1,Tariff4_MD1_TimeStamp,Tariff4_MD2,Tariff4_MD2_TimeStamp,");
                builder.Append("Tariff5_MD1,Tariff5_MD1_TimeStamp,Tariff5_MD2,Tariff5_MD2_TimeStamp,");
                builder.Append("Tariff6_MD1,Tariff6_MD1_TimeStamp,Tariff6_MD2,Tariff6_MD2_TimeStamp,");
                builder.Append("Tariff7_MD1,Tariff7_MD1_TimeStamp,Tariff7_MD2,Tariff7_MD2_TimeStamp,");
                builder.Append("Tariff8_MD1,Tariff8_MD1_TimeStamp,Tariff8_MD2,Tariff8_MD2_TimeStamp");
                builder.Append(" from meterdata_tariffinformation where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(HistoryID, "=", ParameterName(HistoryID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                request.AddParamter(ParameterName(HistoryID), historyID, DbType.UInt16);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff Energy data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetMeterData(long meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Tariff1_kWh,Tariff1_kVAh,Tariff1_kVARh_lag,Tariff1_kVARh_lead,");
                builder.Append("Tariff2_kWh,Tariff2_kVAh,Tariff2_kVARh_lag,Tariff2_kVARh_lead,");
                builder.Append("Tariff3_kWh,Tariff3_kVAh,Tariff3_kVARh_lag,Tariff3_kVARh_lead,");
                builder.Append("Tariff4_kWh,Tariff4_kVAh,Tariff4_kVARh_lag,Tariff4_kVARh_lead,");
                builder.Append("Tariff5_kWh,Tariff5_kVAh,Tariff5_kVARh_lag,Tariff5_kVARh_lead,");
                builder.Append("Tariff6_kWh,Tariff6_kVAh,Tariff6_kVARh_lag,Tariff6_kVARh_lead,");
                builder.Append("Tariff7_kWh,Tariff7_kVAh,Tariff7_kVARh_lag,Tariff7_kVARh_lead,");
                builder.Append("Tariff8_kWh,Tariff8_kVAh,Tariff8_kVARh_lag,Tariff8_kVARh_lead"); 
                builder.Append(" from meterdata_tariffinformation where "); 
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)," and "));
                builder.Append(string.Concat(HistoryID, "=", ParameterName(HistoryID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                request.AddParamter(ParameterName(HistoryID), historyID, DbType.UInt16);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff Energy data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        private DataRequest GetRequest(IEntity entity)
        {
            TariffEntity tariffEntity = entity as TariffEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_tariffinformation(HistoryID,Tariff1_kWh,Tariff1_kVAh,Tariff1_kVARh_lag,Tariff1_kVARh_lead,Tariff1_MD1,Tariff1_MD1_TimeStamp,Tariff1_MD2,Tariff1_MD2_TimeStamp,");
            builder.Append("Tariff1_Aver_PF,Tariff2_kWh,Tariff2_kVAh,Tariff2_kVARh_lag,Tariff2_kVARh_lead,Tariff2_MD1,Tariff2_MD1_TimeStamp,Tariff2_MD2,Tariff2_MD2_TimeStamp,");
            builder.Append("Tariff2_Aver_PF,Tariff3_kWh,Tariff3_kVAh,Tariff3_kVARh_lag,Tariff3_kVARh_lead,Tariff3_MD1,Tariff3_MD1_TimeStamp,");
            builder.Append("Tariff3_MD2,Tariff3_MD2_TimeStamp,Tariff3_Aver_PF,Tariff4_kWh,Tariff4_kVAh,Tariff4_kVARh_lag,Tariff4_kVARh_lead,Tariff4_MD1,");
            builder.Append("Tariff4_MD1_TimeStamp,Tariff4_MD2,Tariff4_MD2_TimeStamp,Tariff4_Aver_PF,Tariff5_kWh,Tariff5_kVAh,Tariff5_kVARh_lag,");
            builder.Append("Tariff5_kVARh_lead,Tariff5_MD1,Tariff5_MD1_TimeStamp,Tariff5_MD2,Tariff5_MD2_TimeStamp,Tariff5_Aver_PF,Tariff6_kWh,");
            builder.Append("Tariff6_kVAh,Tariff6_kVARh_lag,Tariff6_kVARh_lead,Tariff6_MD1,Tariff6_MD1_TimeStamp,Tariff6_MD2,Tariff6_MD2_TimeStamp,");
            builder.Append("Tariff6_Aver_PF,Tariff7_kWh,Tariff7_kVAh,Tariff7_kVARh_lag,Tariff7_kVARh_lead,Tariff7_MD1,Tariff7_MD1_TimeStamp,Tariff7_MD2,Tariff7_MD2_TimeStamp,");
            builder.Append("Tariff7_Aver_PF,Tariff8_kWh,Tariff8_kVAh,Tariff8_kVARh_lag,Tariff8_kVARh_lead,Tariff8_MD1,Tariff8_MD1_TimeStamp,Tariff8_MD2,");
            builder.Append("Tariff8_MD2_TimeStamp,Tariff8_Aver_PF,MeterData_ID) values(");
            builder.Append(string.Concat(ParameterName(HistoryID), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff1_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff1_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff1_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff2_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff2_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff2_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff3_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff3_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff3_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff4_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff4_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff4_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff5_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff5_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff5_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff6_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff6_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff6_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff7_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff7_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff7_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_kWh), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_kVAh), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_kVARh_lag), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_kVARh_lead), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_MD1), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_MD1_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_MD2), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_MD2_TimeStamp), ","));
            //builder.Append(string.Concat(ParameterName(Tariff8_MD3), ","));
            //builder.Append(string.Concat(ParameterName(Tariff8_MD3_TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(Tariff8_Aver_PF), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(HistoryID), tariffEntity.HistoryID, DbType.Int16);
            request.AddParamter(ParameterName(Tariff1_kWh), tariffEntity.Tariff1_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_kVAh), tariffEntity.Tariff1_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_kVARh_lag), tariffEntity.Tariff1_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_kVARh_lead), tariffEntity.Tariff1_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_MD1), tariffEntity.Tariff1_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_MD1_TimeStamp), tariffEntity.Tariff1_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff1_MD2), tariffEntity.Tariff1_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff1_MD2_TimeStamp), tariffEntity.Tariff1_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff1_MD3), tariffEntity.Tariff1_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff1_MD3_TimeStamp), tariffEntity.Tariff1_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff1_Aver_PF), tariffEntity.Tariff1_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_kWh), tariffEntity.Tariff2_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_kVAh), tariffEntity.Tariff2_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_kVARh_lag), tariffEntity.Tariff2_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_kVARh_lead), tariffEntity.Tariff2_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_MD1), tariffEntity.Tariff2_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_MD1_TimeStamp), tariffEntity.Tariff2_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff2_MD2), tariffEntity.Tariff2_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff2_MD2_TimeStamp), tariffEntity.Tariff2_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff2_MD3), tariffEntity.Tariff2_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff2_MD3_TimeStamp), tariffEntity.Tariff2_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff2_Aver_PF), tariffEntity.Tariff2_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_kWh), tariffEntity.Tariff3_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_kVAh), tariffEntity.Tariff3_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_kVARh_lag), tariffEntity.Tariff3_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_kVARh_lead), tariffEntity.Tariff3_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_MD1), tariffEntity.Tariff3_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_MD1_TimeStamp), tariffEntity.Tariff3_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff3_MD2), tariffEntity.Tariff3_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff3_MD2_TimeStamp), tariffEntity.Tariff3_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff3_MD3), tariffEntity.Tariff3_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff3_MD3_TimeStamp), tariffEntity.Tariff3_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff3_Aver_PF), tariffEntity.Tariff3_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_kWh), tariffEntity.Tariff4_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_kVAh), tariffEntity.Tariff4_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_kVARh_lag), tariffEntity.Tariff4_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_kVARh_lead), tariffEntity.Tariff4_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_MD1), tariffEntity.Tariff4_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_MD1_TimeStamp), tariffEntity.Tariff4_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff4_MD2), tariffEntity.Tariff4_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff4_MD2_TimeStamp), tariffEntity.Tariff4_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff4_MD3), tariffEntity.Tariff4_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff4_MD3_TimeStamp), tariffEntity.Tariff4_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff4_Aver_PF), tariffEntity.Tariff4_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_kWh), tariffEntity.Tariff5_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_kVAh), tariffEntity.Tariff5_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_kVARh_lag), tariffEntity.Tariff5_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_kVARh_lead), tariffEntity.Tariff5_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_MD1), tariffEntity.Tariff5_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_MD1_TimeStamp), tariffEntity.Tariff5_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff5_MD2), tariffEntity.Tariff5_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff5_MD2_TimeStamp), tariffEntity.Tariff5_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff5_MD3), tariffEntity.Tariff5_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff5_MD3_TimeStamp), tariffEntity.Tariff5_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff5_Aver_PF), tariffEntity.Tariff5_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_kWh), tariffEntity.Tariff6_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_kVAh), tariffEntity.Tariff6_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_kVARh_lag), tariffEntity.Tariff6_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_kVARh_lead), tariffEntity.Tariff6_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_MD1), tariffEntity.Tariff6_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_MD1_TimeStamp), tariffEntity.Tariff6_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff6_MD2), tariffEntity.Tariff6_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff6_MD2_TimeStamp), tariffEntity.Tariff6_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff6_MD3), tariffEntity.Tariff6_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff6_MD3_TimeStamp), tariffEntity.Tariff6_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff6_Aver_PF), tariffEntity.Tariff6_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_kWh), tariffEntity.Tariff7_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_kVAh), tariffEntity.Tariff7_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_kVARh_lag), tariffEntity.Tariff7_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_kVARh_lead), tariffEntity.Tariff7_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_MD1), tariffEntity.Tariff7_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_MD1_TimeStamp), tariffEntity.Tariff7_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff7_MD2), tariffEntity.Tariff7_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff7_MD2_TimeStamp), tariffEntity.Tariff7_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff7_MD3), tariffEntity.Tariff7_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff7_MD3_TimeStamp), tariffEntity.Tariff7_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff7_Aver_PF), tariffEntity.Tariff7_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_kWh), tariffEntity.Tariff8_kWh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_kVAh), tariffEntity.Tariff8_kVAh, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_kVARh_lag), tariffEntity.Tariff8_kVARh_lag, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_kVARh_lead), tariffEntity.Tariff8_kVARh_lead, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_MD1), tariffEntity.Tariff8_MD1, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_MD1_TimeStamp), tariffEntity.Tariff8_MD1_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff8_MD2), tariffEntity.Tariff8_MD2, DbType.String, 50);
            request.AddParamter(ParameterName(Tariff8_MD2_TimeStamp), tariffEntity.Tariff8_MD2_TimeStamp, DbType.Int64);
            //request.AddParamter(ParameterName(Tariff8_MD3), tariffEntity.Tariff8_MD3, DbType.String, 50);
            //request.AddParamter(ParameterName(Tariff8_MD3_TimeStamp), tariffEntity.Tariff8_MD3_TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(Tariff8_Aver_PF), tariffEntity.Tariff8_Aver_PF, DbType.String, 50);
            request.AddParamter(ParameterName(MeterData_ID), tariffEntity.MeterData_ID, DbType.Int32);
            return request;
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff Energy data added"));
            }
            catch (Exception) { }
            return null;
        }
        public override IEntity InsertData(IEntity entity)
        {
            TariffEntity tariffEntity = entity as TariffEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff Energy data added")); 
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tariffEntity.Tariff_ID = long.Parse(this.GetPK());
            return tariffEntity;
        }
        public override bool UpdateData(IEntity entity)
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
                builder.Append("Delete from meterdata_tariffinformation where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }


        public override bool DeleteData(IEntity entity)
        {
            TariffEntity tariffEntity = entity as TariffEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_tariffinformation where");
                builder.Append(string.Concat(Tariff_ID,"=", ParameterName(Tariff_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Tariff_ID), tariffEntity.Tariff_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff data deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TariffEntity tariffEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Tariff_ID,HistoryID,Tariff1_kWh,Tariff1_kVAh,Tariff1_kVARh_lag,Tariff1_kVARh_lead,Tariff1_MD1,Tariff1_MD1_TimeStamp,Tariff1_MD2,Tariff1_MD2_TimeStamp,");
                builder.Append("Tariff1_Aver_PF,Tariff2_kWh,Tariff2_kVAh,Tariff2_kVARh_lag,Tariff2_kVARh_lead,Tariff2_MD1,Tariff2_MD1_TimeStamp,Tariff2_MD2,Tariff2_MD2_TimeStamp,");
                builder.Append("Tariff2_Aver_PF,Tariff3_kWh,Tariff3_kVAh,Tariff3_kVARh_lag,Tariff3_kVARh_lead,Tariff3_MD1,Tariff3_MD1_TimeStamp,");
                builder.Append("Tariff3_MD2,Tariff3_MD2_TimeStamp,Tariff3_Aver_PF,Tariff4_kWh,Tariff4_kVAh,Tariff4_kVARh_lag,Tariff4_kVARh_lead,Tariff4_MD1,");
                builder.Append("Tariff4_MD1_TimeStamp,Tariff4_MD2,Tariff4_MD2_TimeStamp,Tariff4_Aver_PF,Tariff5_kWh,Tariff5_kVAh,Tariff5_kVARh_lag,");
                builder.Append("Tariff5_kVARh_lead,Tariff5_MD1,Tariff5_MD1_TimeStamp,Tariff5_MD2,Tariff5_MD2_TimeStamp,Tariff5_Aver_PF,Tariff6_kWh,");
                builder.Append("Tariff6_kVAh,Tariff6_kVARh_lag,Tariff6_kVARh_lead,Tariff6_MD1,Tariff6_MD1_TimeStamp,Tariff6_MD2,Tariff6_MD2_TimeStamp,");
                builder.Append("Tariff6_Aver_PF,Tariff7_kWh,Tariff7_kVAh,Tariff7_kVARh_lag,Tariff7_kVARh_lead,Tariff7_MD1,Tariff7_MD1_TimeStamp,Tariff7_MD2,Tariff7_MD2_TimeStamp,");
                builder.Append("Tariff7_Aver_PF,Tariff8_kWh,Tariff8_kVAh,Tariff8_kVARh_lag,Tariff8_kVARh_lead,Tariff8_MD1,Tariff8_MD1_TimeStamp,Tariff8_MD2,");
                builder.Append("Tariff8_MD2_TimeStamp,Tariff8_Aver_PF,MeterData_ID from meterdata_tariffinformation where ");
                builder.Append(string.Concat(Tariff_ID, "=", ParameterName(Tariff_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Tariff_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tariffEntity = (TariffEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff data viewed"));
            }
            catch (CABException)
            {
            }
            return tariffEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Tariff_ID,HistoryID,Tariff1_kWh,Tariff1_kVAh,Tariff1_kVARh_lag,Tariff1_kVARh_lead,Tariff1_MD1,Tariff1_MD1_TimeStamp,Tariff1_MD2,Tariff1_MD2_TimeStamp,");
                builder.Append("Tariff1_Aver_PF,Tariff2_kWh,Tariff2_kVAh,Tariff2_kVARh_lag,Tariff2_kVARh_lead,Tariff2_MD1,Tariff2_MD1_TimeStamp,Tariff2_MD2,Tariff2_MD2_TimeStamp,");
                builder.Append("Tariff2_Aver_PF,Tariff3_kWh,Tariff3_kVAh,Tariff3_kVARh_lag,Tariff3_kVARh_lead,Tariff3_MD1,Tariff3_MD1_TimeStamp,");
                builder.Append("Tariff3_MD2,Tariff3_MD2_TimeStamp,Tariff3_Aver_PF,Tariff4_kWh,Tariff4_kVAh,Tariff4_kVARh_lag,Tariff4_kVARh_lead,Tariff4_MD1,");
                builder.Append("Tariff4_MD1_TimeStamp,Tariff4_MD2,Tariff4_MD2_TimeStamp,Tariff4_Aver_PF,Tariff5_kWh,Tariff5_kVAh,Tariff5_kVARh_lag,");
                builder.Append("Tariff5_kVARh_lead,Tariff5_MD1,Tariff5_MD1_TimeStamp,Tariff5_MD2,Tariff5_MD2_TimeStamp,Tariff5_Aver_PF,Tariff6_kWh,");
                builder.Append("Tariff6_kVAh,Tariff6_kVARh_lag,Tariff6_kVARh_lead,Tariff6_MD1,Tariff6_MD1_TimeStamp,Tariff6_MD2,Tariff6_MD2_TimeStamp,");
                builder.Append("Tariff6_Aver_PF,Tariff7_kWh,Tariff7_kVAh,Tariff7_kVARh_lag,Tariff7_kVARh_lead,Tariff7_MD1,Tariff7_MD1_TimeStamp,Tariff7_MD2,Tariff7_MD2_TimeStamp,");
                builder.Append("Tariff7_Aver_PF,Tariff8_kWh,Tariff8_kVAh,Tariff8_kVARh_lag,Tariff8_kVARh_lead,Tariff8_MD1,Tariff8_MD1_TimeStamp,Tariff8_MD2,");
                builder.Append("Tariff8_MD2_TimeStamp,Tariff8_Aver_PF,MeterData_ID from meterdata_tariffinformation");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

		public DataSet ListDataSet(long meterDataID)
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select A.MeterID,B.Tariff_ID,B.HistoryID,B.Tariff1_kWh,B.Tariff1_kVAh,B.Tariff1_kVARh_lag,B.Tariff1_kVARh_lead,B.Tariff1_MD1,B.Tariff1_MD1_TimeStamp,B.Tariff1_MD2,B.Tariff1_MD2_TimeStamp,");
				builder.Append("B.Tariff1_Aver_PF,B.Tariff2_kWh,B.Tariff2_kVAh,B.Tariff2_kVARh_lag,B.Tariff2_kVARh_lead,B.Tariff2_MD1,B.Tariff2_MD1_TimeStamp,B.Tariff2_MD2,B.Tariff2_MD2_TimeStamp,");
				builder.Append("B.Tariff2_Aver_PF,B.Tariff3_kWh,B.Tariff3_kVAh,B.Tariff3_kVARh_lag,B.Tariff3_kVARh_lead,B.Tariff3_MD1,B.Tariff3_MD1_TimeStamp,");
				builder.Append("B.Tariff3_MD2,B.Tariff3_MD2_TimeStamp,B.Tariff3_Aver_PF,B.Tariff4_kWh,B.Tariff4_kVAh,B.Tariff4_kVARh_lag,B.Tariff4_kVARh_lead,B.Tariff4_MD1,");
				builder.Append("B.Tariff4_MD1_TimeStamp,B.Tariff4_MD2,B.Tariff4_MD2_TimeStamp,B.Tariff4_Aver_PF,B.Tariff5_kWh,B.Tariff5_kVAh,B.Tariff5_kVARh_lag,");
				builder.Append("B.Tariff5_kVARh_lead,B.Tariff5_MD1,B.Tariff5_MD1_TimeStamp,B.Tariff5_MD2,B.Tariff5_MD2_TimeStamp,B.Tariff5_Aver_PF,B.Tariff6_kWh,");
				builder.Append("B.Tariff6_kVAh,B.Tariff6_kVARh_lag,B.Tariff6_kVARh_lead,B.Tariff6_MD1,B.Tariff6_MD1_TimeStamp,B.Tariff6_MD2,B.Tariff6_MD2_TimeStamp,");
				builder.Append("B.Tariff6_Aver_PF,B.Tariff7_kWh,B.Tariff7_kVAh,B.Tariff7_kVARh_lag,B.Tariff7_kVARh_lead,B.Tariff7_MD1,B.Tariff7_MD1_TimeStamp,B.Tariff7_MD2,B.Tariff7_MD2_TimeStamp,");
				builder.Append("B.Tariff7_Aver_PF,B.Tariff8_kWh,B.Tariff8_kVAh,B.Tariff8_kVARh_lag,B.Tariff8_kVARh_lead,B.Tariff8_MD1,B.Tariff8_MD1_TimeStamp,B.Tariff8_MD2,");
				builder.Append("B.Tariff8_MD2_TimeStamp,B.Tariff8_Aver_PF,B.MeterData_ID from meterdata_tariffinformation B Inner Join meterdata A where ");
				builder.Append(string.Concat("B.",MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff data viewed"));
            }
			catch (CABException)
			{
			}
			return dataSet;
		}

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TariffEntity tariffEntity = new TariffEntity();
            if (NotNullAndNotDBNull(row, Tariff_ID)) tariffEntity.Tariff_ID = Convert.ToInt32(row[Tariff_ID]);
            if (NotNullAndNotDBNull(row, HistoryID)) tariffEntity.HistoryID = Convert.ToInt32(row[HistoryID]);
            if (NotNullAndNotDBNull(row, Tariff1_kWh)) tariffEntity.Tariff1_kWh = Convert.ToString(row[Tariff1_kWh]);
            if (NotNullAndNotDBNull(row, Tariff1_kVAh)) tariffEntity.Tariff1_kVAh = Convert.ToString(row[Tariff1_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff1_kVARh_lag)) tariffEntity.Tariff1_kVARh_lag = Convert.ToString(row[Tariff1_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff1_kVARh_lead)) tariffEntity.Tariff1_kVARh_lead = Convert.ToString(row[Tariff1_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff1_MD1)) tariffEntity.Tariff1_MD1 = Convert.ToString(row[Tariff1_MD1]);
            if (NotNullAndNotDBNull(row, Tariff1_MD1_TimeStamp)) tariffEntity.Tariff1_MD1_TimeStamp = Convert.ToInt32(row[Tariff1_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff1_MD2)) tariffEntity.Tariff1_MD2 = Convert.ToString(row[Tariff1_MD2]);
            if (NotNullAndNotDBNull(row, Tariff1_MD2_TimeStamp)) tariffEntity.Tariff1_MD2_TimeStamp = Convert.ToInt32(row[Tariff1_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff1_MD3)) tariffEntity.Tariff1_MD3 = Convert.ToString(row[Tariff1_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff1_MD3_TimeStamp)) tariffEntity.Tariff1_MD3_TimeStamp = Convert.ToInt32(row[Tariff1_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff1_Aver_PF)) tariffEntity.Tariff1_Aver_PF = Convert.ToString(row[Tariff1_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff2_kWh)) tariffEntity.Tariff2_kWh = Convert.ToString(row[Tariff2_kWh]);
            if (NotNullAndNotDBNull(row, Tariff2_kVAh)) tariffEntity.Tariff2_kVAh = Convert.ToString(row[Tariff2_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff2_kVARh_lag)) tariffEntity.Tariff2_kVARh_lag = Convert.ToString(row[Tariff2_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff2_kVARh_lead)) tariffEntity.Tariff2_kVARh_lead = Convert.ToString(row[Tariff2_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff2_MD1)) tariffEntity.Tariff2_MD1 = Convert.ToString(row[Tariff2_MD1]);
            if (NotNullAndNotDBNull(row, Tariff2_MD1_TimeStamp)) tariffEntity.Tariff2_MD1_TimeStamp = Convert.ToInt32(row[Tariff2_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff2_MD2)) tariffEntity.Tariff2_MD2 = Convert.ToString(row[Tariff2_MD2]);
            if (NotNullAndNotDBNull(row, Tariff2_MD2_TimeStamp)) tariffEntity.Tariff2_MD2_TimeStamp = Convert.ToInt32(row[Tariff2_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff2_MD3)) tariffEntity.Tariff2_MD3 = Convert.ToString(row[Tariff2_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff2_MD3_TimeStamp)) tariffEntity.Tariff2_MD3_TimeStamp = Convert.ToInt32(row[Tariff2_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff2_Aver_PF)) tariffEntity.Tariff2_Aver_PF = Convert.ToString(row[Tariff2_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff3_kWh)) tariffEntity.Tariff3_kWh = Convert.ToString(row[Tariff3_kWh]);
            if (NotNullAndNotDBNull(row, Tariff3_kVAh)) tariffEntity.Tariff3_kVAh = Convert.ToString(row[Tariff3_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff3_kVARh_lag)) tariffEntity.Tariff3_kVARh_lag = Convert.ToString(row[Tariff3_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff3_kVARh_lead)) tariffEntity.Tariff3_kVARh_lead = Convert.ToString(row[Tariff3_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff3_MD1)) tariffEntity.Tariff3_MD1 = Convert.ToString(row[Tariff3_MD1]);
            if (NotNullAndNotDBNull(row, Tariff3_MD1_TimeStamp)) tariffEntity.Tariff3_MD1_TimeStamp = Convert.ToInt32(row[Tariff3_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff3_MD2)) tariffEntity.Tariff3_MD2 = Convert.ToString(row[Tariff3_MD2]);
            if (NotNullAndNotDBNull(row, Tariff3_MD2_TimeStamp)) tariffEntity.Tariff3_MD2_TimeStamp = Convert.ToInt32(row[Tariff3_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff3_MD3)) tariffEntity.Tariff3_MD3 = Convert.ToString(row[Tariff3_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff3_MD3_TimeStamp)) tariffEntity.Tariff3_MD3_TimeStamp = Convert.ToInt32(row[Tariff3_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff3_Aver_PF)) tariffEntity.Tariff3_Aver_PF = Convert.ToString(row[Tariff3_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff4_kWh)) tariffEntity.Tariff4_kWh = Convert.ToString(row[Tariff4_kWh]);
            if (NotNullAndNotDBNull(row, Tariff4_kVAh)) tariffEntity.Tariff4_kVAh = Convert.ToString(row[Tariff4_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff4_kVARh_lag)) tariffEntity.Tariff4_kVARh_lag = Convert.ToString(row[Tariff4_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff4_kVARh_lead)) tariffEntity.Tariff4_kVARh_lead = Convert.ToString(row[Tariff4_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff4_MD1)) tariffEntity.Tariff4_MD1 = Convert.ToString(row[Tariff4_MD1]);
            if (NotNullAndNotDBNull(row, Tariff4_MD1_TimeStamp)) tariffEntity.Tariff4_MD1_TimeStamp = Convert.ToInt32(row[Tariff4_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff4_MD2)) tariffEntity.Tariff4_MD2 = Convert.ToString(row[Tariff4_MD2]);
            if (NotNullAndNotDBNull(row, Tariff4_MD2_TimeStamp)) tariffEntity.Tariff4_MD2_TimeStamp = Convert.ToInt32(row[Tariff4_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff4_MD3)) tariffEntity.Tariff4_MD3 = Convert.ToString(row[Tariff4_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff4_MD3_TimeStamp)) tariffEntity.Tariff4_MD3_TimeStamp = Convert.ToInt32(row[Tariff4_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff4_Aver_PF)) tariffEntity.Tariff4_Aver_PF = Convert.ToString(row[Tariff4_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff5_kWh)) tariffEntity.Tariff5_kWh = Convert.ToString(row[Tariff5_kWh]);
            if (NotNullAndNotDBNull(row, Tariff5_kVAh)) tariffEntity.Tariff5_kVAh = Convert.ToString(row[Tariff5_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff5_kVARh_lag)) tariffEntity.Tariff5_kVARh_lag = Convert.ToString(row[Tariff5_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff5_kVARh_lead)) tariffEntity.Tariff5_kVARh_lead = Convert.ToString(row[Tariff5_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff5_MD1)) tariffEntity.Tariff5_MD1 = Convert.ToString(row[Tariff5_MD1]);
            if (NotNullAndNotDBNull(row, Tariff5_MD1_TimeStamp)) tariffEntity.Tariff5_MD1_TimeStamp = Convert.ToInt32(row[Tariff5_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff5_MD2)) tariffEntity.Tariff5_MD2 = Convert.ToString(row[Tariff5_MD2]);
            if (NotNullAndNotDBNull(row, Tariff5_MD2_TimeStamp)) tariffEntity.Tariff5_MD2_TimeStamp = Convert.ToInt32(row[Tariff5_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff5_MD3)) tariffEntity.Tariff5_MD3 = Convert.ToString(row[Tariff5_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff5_MD3_TimeStamp)) tariffEntity.Tariff5_MD3_TimeStamp = Convert.ToInt32(row[Tariff5_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff5_Aver_PF)) tariffEntity.Tariff5_Aver_PF = Convert.ToString(row[Tariff5_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff6_kWh)) tariffEntity.Tariff6_kWh = Convert.ToString(row[Tariff6_kWh]);
            if (NotNullAndNotDBNull(row, Tariff6_kVAh)) tariffEntity.Tariff6_kVAh = Convert.ToString(row[Tariff6_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff6_kVARh_lag)) tariffEntity.Tariff6_kVARh_lag = Convert.ToString(row[Tariff6_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff6_kVARh_lead)) tariffEntity.Tariff6_kVARh_lead = Convert.ToString(row[Tariff6_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff6_MD1)) tariffEntity.Tariff6_MD1 = Convert.ToString(row[Tariff6_MD1]);
            if (NotNullAndNotDBNull(row, Tariff6_MD1_TimeStamp)) tariffEntity.Tariff6_MD1_TimeStamp = Convert.ToInt32(row[Tariff6_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff6_MD2)) tariffEntity.Tariff6_MD2 = Convert.ToString(row[Tariff6_MD2]);
            if (NotNullAndNotDBNull(row, Tariff6_MD2_TimeStamp)) tariffEntity.Tariff6_MD2_TimeStamp = Convert.ToInt32(row[Tariff6_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff6_MD3)) tariffEntity.Tariff6_MD3 = Convert.ToString(row[Tariff6_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff6_MD3_TimeStamp)) tariffEntity.Tariff6_MD3_TimeStamp = Convert.ToInt32(row[Tariff6_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff6_Aver_PF)) tariffEntity.Tariff6_Aver_PF = Convert.ToString(row[Tariff6_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff7_kWh)) tariffEntity.Tariff7_kWh = Convert.ToString(row[Tariff7_kWh]);
            if (NotNullAndNotDBNull(row, Tariff7_kVAh)) tariffEntity.Tariff7_kVAh = Convert.ToString(row[Tariff7_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff7_kVARh_lag)) tariffEntity.Tariff7_kVARh_lag = Convert.ToString(row[Tariff7_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff7_kVARh_lead)) tariffEntity.Tariff7_kVARh_lead = Convert.ToString(row[Tariff7_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff7_MD1)) tariffEntity.Tariff7_MD1 = Convert.ToString(row[Tariff7_MD1]);
            if (NotNullAndNotDBNull(row, Tariff7_MD1_TimeStamp)) tariffEntity.Tariff7_MD1_TimeStamp = Convert.ToInt32(row[Tariff7_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff7_MD2)) tariffEntity.Tariff7_MD2 = Convert.ToString(row[Tariff7_MD2]);
            if (NotNullAndNotDBNull(row, Tariff7_MD2_TimeStamp)) tariffEntity.Tariff7_MD2_TimeStamp = Convert.ToInt32(row[Tariff7_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff7_MD3)) tariffEntity.Tariff7_MD3 = Convert.ToString(row[Tariff7_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff7_MD3_TimeStamp)) tariffEntity.Tariff7_MD3_TimeStamp = Convert.ToInt32(row[Tariff7_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff7_Aver_PF)) tariffEntity.Tariff7_Aver_PF = Convert.ToString(row[Tariff7_Aver_PF]);
            if (NotNullAndNotDBNull(row, Tariff8_kWh)) tariffEntity.Tariff8_kWh = Convert.ToString(row[Tariff8_kWh]);
            if (NotNullAndNotDBNull(row, Tariff8_kVAh)) tariffEntity.Tariff8_kVAh = Convert.ToString(row[Tariff8_kVAh]);
            if (NotNullAndNotDBNull(row, Tariff8_kVARh_lag)) tariffEntity.Tariff8_kVARh_lag = Convert.ToString(row[Tariff8_kVARh_lag]);
            if (NotNullAndNotDBNull(row, Tariff8_kVARh_lead)) tariffEntity.Tariff8_kVARh_lead = Convert.ToString(row[Tariff8_kVARh_lead]);
            if (NotNullAndNotDBNull(row, Tariff8_MD1)) tariffEntity.Tariff8_MD1 = Convert.ToString(row[Tariff8_MD1]);
            if (NotNullAndNotDBNull(row, Tariff8_MD1_TimeStamp)) tariffEntity.Tariff8_MD1_TimeStamp = Convert.ToInt32(row[Tariff8_MD1_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff8_MD2)) tariffEntity.Tariff8_MD2 = Convert.ToString(row[Tariff8_MD2]);
            if (NotNullAndNotDBNull(row, Tariff8_MD2_TimeStamp)) tariffEntity.Tariff8_MD2_TimeStamp = Convert.ToInt32(row[Tariff8_MD2_TimeStamp]);
            //if (NotNullAndNotDBNull(row, Tariff8_MD3)) tariffEntity.Tariff8_MD3 = Convert.ToString(row[Tariff8_MD3]);
            //if (NotNullAndNotDBNull(row, Tariff8_MD3_TimeStamp)) tariffEntity.Tariff8_MD3_TimeStamp = Convert.ToInt32(row[Tariff8_MD3_TimeStamp]);
            if (NotNullAndNotDBNull(row, Tariff8_Aver_PF)) tariffEntity.Tariff8_Aver_PF = Convert.ToString(row[Tariff8_Aver_PF]);
                if (NotNullAndNotDBNull(row, MeterData_ID)) tariffEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]);
            return tariffEntity;
        }



        public DataSet GetTariffPF(long activeMeterDataId)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select HistoryID,Tariff1_Aver_PF,Tariff2_Aver_PF,Tariff3_Aver_PF,Tariff4_Aver_PF,Tariff5_Aver_PF,Tariff6_Aver_PF,Tariff7_Aver_PF,Tariff8_Aver_PF ");
                builder.Append("from meterdata_tariffinformation where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterDataId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tariff data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
    }
}


