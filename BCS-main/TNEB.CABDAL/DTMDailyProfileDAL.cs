/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class DTMDailyProfileDAL : DALBase
    {
        private string DTMDailyProfile_ID = "DTMDailyProfile_ID";
        private string DailyProfileDate = "DailyProfileDate";
        private string CumulativeFundamentalKwh = "CumulativeFundamentalKwh";
        private string CumulativekWh = "CumulativekWh";
        private string CumulativekVArh_lag = "CumulativekVArh_lag";
        private string CumulativekVArh_lead = "CumulativekVArh_lead";
        private string CumulativekVAh = "CumulativekVAh";
        private string DailyMD1 = "DailyMD1";
        private string MD1TimeStamp = "MD1TimeStamp";
        private string DailyMD2 = "DailyMD2";
        private string MD2TimeStamp = "MD2TimeStamp";
        private string DailyMD3 = "DailyMD3"; 
        private string MD3TimeStamp = "MD3TimeStamp";
        private string MaxAvgVoltage = "MaxAvgVoltage";
        private string MinAvgVoltage = "MinAvgVoltage";
        private string MaxAvgCurrent = "MaxAvgCurrent";
        private string MinAvgCurrent = "MinAvgCurrent";
        private string AvailableDays = "AvailableDays";
        private string MaximumDays = "MaximumDays";
        private string MeterData_ID = "MeterData_ID";
        private string PowerOnHours = "PowerOnHours";

        public  DTMDailyProfileDAL()
            : base("MeterData_DTMDailyProfile", "DTMDailyProfile_ID")
        {
        }
        private DataRequest GetRequest(IEntity entity)
        {
            DTMDailyProfileEntity dTMDailyProfileEntity = entity as DTMDailyProfileEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into MeterData_DTMDailyProfile(DailyProfileDate,CumulativeFundamentalKwh,CumulativekWh,CumulativekVArh_lag,CumulativekVArh_lead,CumulativekVAh,DailyMD1,MD1TimeStamp,DailyMD2,MD2TimeStamp,DailyMD3,MD3TimeStamp,MaxAvgVoltage,MinAvgVoltage,MaxAvgCurrent,MinAvgCurrent,AvailableDays,MaximumDays,MeterData_ID,PowerOnHours) values(");
            builder.Append(string.Concat(ParameterName(DailyProfileDate), ","));
            builder.Append(string.Concat(ParameterName(CumulativeFundamentalKwh), ","));
            builder.Append(string.Concat(ParameterName(CumulativekWh), ","));
            builder.Append(string.Concat(ParameterName(CumulativekVArh_lag), ","));
            builder.Append(string.Concat(ParameterName(CumulativekVArh_lead), ","));
            builder.Append(string.Concat(ParameterName(CumulativekVAh), ","));
            builder.Append(string.Concat(ParameterName(DailyMD1), ","));
            builder.Append(string.Concat(ParameterName(MD1TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(DailyMD2), ","));
            builder.Append(string.Concat(ParameterName(MD2TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(DailyMD3), ","));
            builder.Append(string.Concat(ParameterName(MD3TimeStamp), ","));
            builder.Append(string.Concat(ParameterName(MaxAvgVoltage), ","));
            builder.Append(string.Concat(ParameterName(MinAvgVoltage), ","));
            builder.Append(string.Concat(ParameterName(MaxAvgCurrent), ","));
            builder.Append(string.Concat(ParameterName(MinAvgCurrent), ","));
            builder.Append(string.Concat(ParameterName(AvailableDays), ","));
            builder.Append(string.Concat(ParameterName(MaximumDays), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(PowerOnHours), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(DailyProfileDate), dTMDailyProfileEntity.DailyProfileDate, DbType.Int64);
            request.AddParamter(ParameterName(CumulativeFundamentalKwh), dTMDailyProfileEntity.CumulativeFundamentalkWh, DbType.String, 20);
            request.AddParamter(ParameterName(CumulativekWh), dTMDailyProfileEntity.CumulativekWh, DbType.String, 20);
            request.AddParamter(ParameterName(CumulativekVArh_lag), dTMDailyProfileEntity.CumulativekVArh_lag, DbType.String, 20);
            request.AddParamter(ParameterName(CumulativekVArh_lead), dTMDailyProfileEntity.CumulativekVArh_lead, DbType.String, 20);
            request.AddParamter(ParameterName(CumulativekVAh), dTMDailyProfileEntity.CumulativekVAh, DbType.String, 20);
            request.AddParamter(ParameterName(DailyMD1), dTMDailyProfileEntity.DailyMD1, DbType.String, 20);
            request.AddParamter(ParameterName(MD1TimeStamp), dTMDailyProfileEntity.MD1TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(DailyMD2), dTMDailyProfileEntity.DailyMD2, DbType.String, 20);
            request.AddParamter(ParameterName(MD2TimeStamp), dTMDailyProfileEntity.MD2TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(DailyMD3), dTMDailyProfileEntity.DailyMD3, DbType.String, 20);
            request.AddParamter(ParameterName(MD3TimeStamp), dTMDailyProfileEntity.MD3TimeStamp, DbType.Int64);
            request.AddParamter(ParameterName(MaxAvgVoltage), dTMDailyProfileEntity.MaxAvgVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(MinAvgVoltage), dTMDailyProfileEntity.MinAvgVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(MaxAvgCurrent), dTMDailyProfileEntity.MaxAvgCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(MinAvgCurrent), dTMDailyProfileEntity.MinAvgCurrent, DbType.String, 20);
            request.AddParamter(ParameterName(AvailableDays), dTMDailyProfileEntity.AvailableDays, DbType.String, 20);
            request.AddParamter(ParameterName(MaximumDays), dTMDailyProfileEntity.MaximumDays, DbType.String, 20);
            request.AddParamter(ParameterName(MeterData_ID), dTMDailyProfileEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(PowerOnHours), dTMDailyProfileEntity.PowerOnHours, DbType.String,20);
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Load Survey added"));
            }
            catch (Exception) { }
            return null;
        }
        
        public override  IEntity InsertData(IEntity entity)
        {
            DTMDailyProfileEntity dTMDailyProfileEntity = entity as DTMDailyProfileEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Daily Profile added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                dTMDailyProfileEntity.DTMDailyProfile_ID = long.Parse(this.GetPK());
            return dTMDailyProfileEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

         

        public override IEntity RowToEntity(DataRow row)
        {  
            if (row == null) return null;
            DTMDailyProfileEntity dTMDailyProfileEntity = new DTMDailyProfileEntity();
            if (NotNullAndNotDBNull(row, DTMDailyProfile_ID)) dTMDailyProfileEntity.DTMDailyProfile_ID = Convert.ToInt64(row[DTMDailyProfile_ID]);
            if (NotNullAndNotDBNull(row, DailyProfileDate)) dTMDailyProfileEntity.DailyProfileDate = Convert.ToInt64(row[DailyProfileDate]);
            if (NotNullAndNotDBNull(row, CumulativeFundamentalKwh)) dTMDailyProfileEntity.CumulativeFundamentalkWh = Convert.ToString(row[CumulativeFundamentalKwh]);
            if (NotNullAndNotDBNull(row, CumulativekWh)) dTMDailyProfileEntity.CumulativekWh = Convert.ToString(row[CumulativekWh]);
            if (NotNullAndNotDBNull(row, CumulativekVArh_lag)) dTMDailyProfileEntity.CumulativekVArh_lag = Convert.ToString(row[CumulativekVArh_lag]);
            if (NotNullAndNotDBNull(row, CumulativekVArh_lead)) dTMDailyProfileEntity.CumulativekVArh_lead = Convert.ToString(row[CumulativekVArh_lead]);
            if (NotNullAndNotDBNull(row, CumulativekVAh)) dTMDailyProfileEntity.CumulativekVAh = Convert.ToString(row[CumulativekVAh]);
            if (NotNullAndNotDBNull(row, DailyMD1)) dTMDailyProfileEntity.DailyMD1 = Convert.ToString(row[DailyMD1]); 
            if (NotNullAndNotDBNull(row, MD1TimeStamp)) dTMDailyProfileEntity.MD1TimeStamp = Convert.ToInt64(row[MD1TimeStamp]);
            if (NotNullAndNotDBNull(row, DailyMD2)) dTMDailyProfileEntity.DailyMD2 = Convert.ToString(row[DailyMD2]);
            if (NotNullAndNotDBNull(row, MD2TimeStamp)) dTMDailyProfileEntity.MD2TimeStamp = Convert.ToInt64(row[MD2TimeStamp]);
            if (NotNullAndNotDBNull(row, DailyMD3)) dTMDailyProfileEntity.DailyMD3 = Convert.ToString(row[DailyMD3]);
            if (NotNullAndNotDBNull(row, MD3TimeStamp)) dTMDailyProfileEntity.MD3TimeStamp = Convert.ToInt64(row[MD3TimeStamp]); 
            if (NotNullAndNotDBNull(row, MaxAvgVoltage)) dTMDailyProfileEntity.MaxAvgVoltage = Convert.ToString(row[MaxAvgVoltage]);
            if (NotNullAndNotDBNull(row, MinAvgVoltage)) dTMDailyProfileEntity.MinAvgVoltage = Convert.ToString(row[MinAvgVoltage]);
            if (NotNullAndNotDBNull(row, MaxAvgCurrent)) dTMDailyProfileEntity.MaxAvgCurrent = Convert.ToString(row[MaxAvgCurrent]); 
            if (NotNullAndNotDBNull(row, MinAvgCurrent)) dTMDailyProfileEntity.MinAvgCurrent = Convert.ToString(row[MinAvgCurrent]);
            if (NotNullAndNotDBNull(row, AvailableDays)) dTMDailyProfileEntity.AvailableDays = Convert.ToString(row[AvailableDays]);
            if (NotNullAndNotDBNull(row, MaximumDays)) dTMDailyProfileEntity.MaximumDays = Convert.ToString(row[MaximumDays]); 
            if (NotNullAndNotDBNull(row, MeterData_ID)) dTMDailyProfileEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, PowerOnHours)) dTMDailyProfileEntity.PowerOnHours = Convert.ToString(row[PowerOnHours]); 
            return dTMDailyProfileEntity;
        }

        public DataSet GetDailyProfileByParameter(long activeMeterData_ID, List<string> columnList)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select DailyProfileDate");
                foreach (string column in columnList)
                {
                    builder.Append(string.Concat(",",column," "));
                }
                builder.Append("from meterdata_dtmdailyprofile where ");
                builder.Append(string.Concat(MeterData_ID, " = ", ParameterName(MeterData_ID)));
                builder.Append(" and DailyProfileDate !=0 ");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Daily Profile viewed"));
            }
            catch (Exception)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_dtmdailyprofile where ");
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
            throw new NotImplementedException();
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

        public DataSet ListData(IECDTMDailyProfileParameterEntity entity)
        {
            if (entity == null)
                return null;
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select DailyProfileDate,");
                builder.Append(entity.ColumnsNames);
                builder.Append(",AvailableDays from meterdata_dtmdailyprofile where ");
                builder.Append(string.Concat(MeterData_ID, " = ", ParameterName(MeterData_ID)));
                builder.Append(" order by DailyProfileDate asc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), entity.MeterDataId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Daily Profile viewed"));
            }
            catch (Exception)
            {
                dataSet = null;
            }
            return dataSet;
        } 
    }
}
