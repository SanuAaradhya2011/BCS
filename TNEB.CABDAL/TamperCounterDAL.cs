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
    public class TamperCounterDAL : DALBase
    {
		private string MeterData_ID = "MeterData_ID";
        private string TamperCounter_ID = "TamperCounter_ID";
        private string TotalTamperCounter = "TotalTamperCounter";
        private string PowerOnOffCounter = "PowerOnOffCounter";
        private string LowLoadCounter = "LowLoadCounter";
        private string OverLoadCounter = "OverLoadCounter";
        private string TamperCounterGeneral_ID = "TamperCounterGeneral_ID"; 
        public TamperCounterDAL()
            : base("MeterData_TamperCounter", "TamperCounter_ID")
        {
        }

       
        public object GetTamperCount(int meterDataId, string tamperName)
        {
            object obj = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append(tamperName);
                builder.Append(" from MeterData_TamperCounter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int32);
                obj = helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper counter viewed"));
            }
            catch (Exception) { obj = null; }
            return obj;
        }
        public override IEntity InsertData(IEntity entity)
        {
            TamperCounterEntity tamperCounterEntity = entity as TamperCounterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_TamperCounter(TotalTamperCounter,PowerOnOffCounter,LowLoadCounter,OverLoadCounter,TamperCounterGeneral_ID,meterData_ID) values(");
                builder.Append(string.Concat(ParameterName(TotalTamperCounter), ","));
                builder.Append(string.Concat(ParameterName(PowerOnOffCounter), ","));
                builder.Append(string.Concat(ParameterName(LowLoadCounter), ","));
                builder.Append(string.Concat(ParameterName(OverLoadCounter), ","));
                builder.Append(string.Concat(ParameterName(TamperCounterGeneral_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TotalTamperCounter), tamperCounterEntity.TotalTamperCounter, DbType.Int32);
                request.AddParamter(ParameterName(PowerOnOffCounter), tamperCounterEntity.PowerOnOffCounter, DbType.Int32);
                request.AddParamter(ParameterName(LowLoadCounter), tamperCounterEntity.LowLoadCounter, DbType.Int32);
                request.AddParamter(ParameterName(OverLoadCounter), tamperCounterEntity.OverLoadCounter, DbType.Int32);
                request.AddParamter(ParameterName(TamperCounterGeneral_ID), tamperCounterEntity.TamperCounterGeneral_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterData_ID), tamperCounterEntity.MeterData_ID, DbType.Int64); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tamperCounterEntity.TamperCounter_ID = long.Parse(this.GetPK());
            return tamperCounterEntity;
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
                builder.Append("Delete from MeterData_TamperCounter where ");
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
            TamperCounterEntity tamperCounterEntity = entity as TamperCounterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TamperCounter where ");
                builder.Append(string.Concat(TamperCounter_ID, "=", ParameterName(TamperCounter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCounter_ID), tamperCounterEntity.TamperCounter_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TamperCounterEntity tamperCounterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperCounter_ID,TotalTamperCounter,PowerOnOffCounter,LowLoadCounter,OverLoadCounter,TamperCounterGeneral_ID from MeterData_TamperCounter where ");
                builder.Append(string.Concat(TamperCounter_ID, "=", ParameterName(TamperCounter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCounter_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tamperCounterEntity = (TamperCounterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter retrieved"));

            }
            catch (CABException)
            {
            }
            return tamperCounterEntity;
        }

		public DataSet ListDataSet(int meterDataID)
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select A.MeterID,B.TamperCounter_ID,B.TotalTamperCounter,B.PowerOnOffCounter,B.LowLoadCounter,B.OverLoadCounter,B.TamperCounterGeneral_ID from MeterData_TamperCounter B Inner Join meterdata A on A.MeterData_ID = B.MeterData_ID where ");
				builder.Append(string.Concat("B.",MeterData_ID, "=", ParameterName(MeterData_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(TamperCounter_ID), meterDataID, DbType.UInt32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					return dataSet;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter viewed"));
            }
			catch (CABException)
			{
			}
			return dataSet;
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
                builder.Append("Select TamperCounter_ID,TotalTamperCounter,PowerOnOffCounter,LowLoadCounter,OverLoadCounter,TamperCounterGeneral_ID from MeterData_TamperCounter ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Counter viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperCounterEntity tamperCounterEntity = new TamperCounterEntity();
            if (NotNullAndNotDBNull(row, TamperCounter_ID)) tamperCounterEntity.TamperCounter_ID = Convert.ToInt64(row[TamperCounter_ID]);
            if (NotNullAndNotDBNull(row, TotalTamperCounter)) tamperCounterEntity.TotalTamperCounter = Convert.ToInt32(row[TotalTamperCounter]);
            if (NotNullAndNotDBNull(row, PowerOnOffCounter)) tamperCounterEntity.PowerOnOffCounter = Convert.ToInt32(row[PowerOnOffCounter]);
            if (NotNullAndNotDBNull(row, LowLoadCounter)) tamperCounterEntity.LowLoadCounter = Convert.ToInt32(row[LowLoadCounter]);
            if (NotNullAndNotDBNull(row, OverLoadCounter)) tamperCounterEntity.OverLoadCounter = Convert.ToInt32(row[OverLoadCounter]);
            if (NotNullAndNotDBNull(row, TamperCounterGeneral_ID)) tamperCounterEntity.TamperCounterGeneral_ID = Convert.ToInt64(row[TamperCounterGeneral_ID]);
            return tamperCounterEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
