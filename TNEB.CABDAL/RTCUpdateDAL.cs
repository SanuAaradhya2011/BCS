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
	 public class RTCUpdateDAL : DALBase
    {
        private string RTCUpdate_ID = "RTCUpdate_ID";
        private string TotalRTCUpdates = "TotalRTCUpdates";
        private string CurrentRTC1 = "CurrentRTC1"; 
        private string PreviousRTC1 = "PreviousRTC1";
        private string CurrentRTC2 = "CurrentRTC2";
        private string PreviousRTC2 = "PreviousRTC2"; 
        private string CurrentRTC3 = "CurrentRTC3";
        private string PreviousRTC3 = "PreviousRTC3";
        private string CurrentRTC4 = "CurrentRTC4";
        private string PreviousRTC4 = "PreviousRTC4";
        private string CurrentRTC5 = "CurrentRTC5";
        private string PreviousRTC5 = "PreviousRTC5";
        private string CurrentRTC6 = "CurrentRTC6";
        private string PreviousRTC6 = "PreviousRTC6";
        private string CurrentRTC7 = "CurrentRTC7";
        private string PreviousRTC7 = "PreviousRTC7";
        private string CurrentRTC8 = "CurrentRTC8";
        private string PreviousRTC8 = "PreviousRTC8";
        private string CurrentRTC9 = "CurrentRTC9";
        private string PreviousRTC9 = "PreviousRTC9";
        private string CurrentRTC10 = "CurrentRTC10";
        private string PreviousRTC10 = "PreviousRTC10";
        private string MeterData_ID = "MeterData_ID";

        public RTCUpdateDAL()
            : base("MeterData_RTCUpdate", "RTCUpdate_ID")
        {
        }
 
        public DataSet GetRTCUpdateList(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TotalRTCUpdates, CurrentRTC1,PreviousRTC1,CurrentRTC2,PreviousRTC2,CurrentRTC3,");
                builder.Append("PreviousRTC3,CurrentRTC4,PreviousRTC4,CurrentRTC5,PreviousRTC5,CurrentRTC6,PreviousRTC6,");
                builder.Append("CurrentRTC7,PreviousRTC7,CurrentRTC8,PreviousRTC8,CurrentRTC9,PreviousRTC9,CurrentRTC10,PreviousRTC10");
                builder.Append(" from MeterData_RTCUpdate where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public int GetTotalRTCUpdates(long meterDataId)
        {
            int total = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Max(TotalRTCUpdates) from MeterData_RTCUpdate where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                object obj = helper.ExecuteScalar(request);
				if(!string.IsNullOrEmpty(Convert.ToString(obj)))
                total = Convert.ToInt32(obj);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));
            }
            catch (CABException)
            {
                total = 0;
            }
            return total;
        }
        public override IEntity InsertData(IEntity entity)
        {
            RTCUpdateEntity rTCUpdateEntity = entity as RTCUpdateEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_RTCUpdate(TotalRTCUpdates,CurrentRTC1,PreviousRTC1,CurrentRTC2,PreviousRTC2,CurrentRTC3,");
                builder.Append("PreviousRTC3,CurrentRTC4,PreviousRTC4,CurrentRTC5,PreviousRTC5,CurrentRTC6,PreviousRTC6,");
                builder.Append("CurrentRTC7,PreviousRTC7,CurrentRTC8,PreviousRTC8,CurrentRTC9,PreviousRTC9,CurrentRTC10,PreviousRTC10,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(TotalRTCUpdates), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC1), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC1), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC2), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC2), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC3), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC3), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC4), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC4), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC5), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC5), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC6), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC6), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC7), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC7), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC8), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC8), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC9), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC9), ","));
                builder.Append(string.Concat(ParameterName(CurrentRTC10), ","));
                builder.Append(string.Concat(ParameterName(PreviousRTC10), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TotalRTCUpdates), rTCUpdateEntity.TotalRTCUpdates, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC1), rTCUpdateEntity.CurrentRTC1, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC1), rTCUpdateEntity.PreviousRTC1, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC2), rTCUpdateEntity.CurrentRTC2, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC2), rTCUpdateEntity.PreviousRTC2, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC3), rTCUpdateEntity.CurrentRTC3, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC3), rTCUpdateEntity.PreviousRTC3, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC4), rTCUpdateEntity.CurrentRTC4, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC4), rTCUpdateEntity.PreviousRTC4, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC5), rTCUpdateEntity.CurrentRTC5, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC5), rTCUpdateEntity.PreviousRTC5, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC6), rTCUpdateEntity.CurrentRTC6, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC6), rTCUpdateEntity.PreviousRTC6, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC7), rTCUpdateEntity.CurrentRTC7, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC7), rTCUpdateEntity.PreviousRTC7, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC8), rTCUpdateEntity.CurrentRTC8, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC8), rTCUpdateEntity.PreviousRTC8, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC9), rTCUpdateEntity.CurrentRTC9, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC9), rTCUpdateEntity.PreviousRTC9, DbType.String, 20);
                request.AddParamter(ParameterName(CurrentRTC10), rTCUpdateEntity.CurrentRTC10, DbType.String, 20);
                request.AddParamter(ParameterName(PreviousRTC10), rTCUpdateEntity.PreviousRTC10, DbType.String, 20);
                request.AddParamter(ParameterName(MeterData_ID), rTCUpdateEntity.MeterData_ID, DbType.Int64); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                rTCUpdateEntity.RTCUpdate_ID = long.Parse(this.GetPK());
            return rTCUpdateEntity;
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
                builder.Append("Delete from MeterData_RTCUpdate where ");
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
            RTCUpdateEntity rTCUpdateEntity = entity as RTCUpdateEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_RTCUpdate where ");
                builder.Append(string.Concat(RTCUpdate_ID, "=", ParameterName(RTCUpdate_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(RTCUpdate_ID), rTCUpdateEntity.RTCUpdate_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record deleted")); 
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            RTCUpdateEntity rTCUpdateEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select RTCUpdate_ID,TotalRTCUpdates,CurrentRTC1,PreviousRTC1,CurrentRTC2,PreviousRTC2,CurrentRTC3,PreviousRTC3,CurrentRTC4,PreviousRTC4,CurrentRTC5,PreviousRTC5,CurrentRTC6,PreviousRTC6,CurrentRTC7,PreviousRTC7,CurrentRTC8,PreviousRTC8,CurrentRTC9,PreviousRTC9,CurrentRTC10,PreviousRTC10,MeterData_ID from MeterData_RTCUpdate where ");
                builder.Append(string.Concat(RTCUpdate_ID, "=", ParameterName(RTCUpdate_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(RTCUpdate_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    rTCUpdateEntity = (RTCUpdateEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));
            }
            catch (CABException)
            {
            }
            return rTCUpdateEntity;
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
                builder.Append("Select RTCUpdate_ID,TotalRTCUpdates,CurrentRTC1,PreviousRTC1,CurrentRTC2,PreviousRTC2,CurrentRTC3,PreviousRTC3,CurrentRTC4,PreviousRTC4,CurrentRTC5,PreviousRTC5,CurrentRTC6,PreviousRTC6,CurrentRTC7,PreviousRTC7,CurrentRTC8,PreviousRTC8,CurrentRTC9,PreviousRTC9,CurrentRTC10,PreviousRTC10,MeterData_ID from MeterData_RTCUpdate ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            RTCUpdateEntity rTCUpdateEntity = new RTCUpdateEntity();
            if (NotNullAndNotDBNull(row, RTCUpdate_ID)) rTCUpdateEntity.RTCUpdate_ID = Convert.ToInt64(row[RTCUpdate_ID]);
            if (NotNullAndNotDBNull(row, TotalRTCUpdates)) rTCUpdateEntity.TotalRTCUpdates = Convert.ToString(row[TotalRTCUpdates]);
            if (NotNullAndNotDBNull(row, CurrentRTC1)) rTCUpdateEntity.CurrentRTC1 = Convert.ToString(row[CurrentRTC1]);
            if (NotNullAndNotDBNull(row, PreviousRTC1)) rTCUpdateEntity.PreviousRTC1 = Convert.ToString(row[PreviousRTC1]);
            if (NotNullAndNotDBNull(row, CurrentRTC2)) rTCUpdateEntity.CurrentRTC2 = Convert.ToString(row[CurrentRTC2]);
            if (NotNullAndNotDBNull(row, PreviousRTC2)) rTCUpdateEntity.PreviousRTC2 = Convert.ToString(row[PreviousRTC2]);
            if (NotNullAndNotDBNull(row, CurrentRTC3)) rTCUpdateEntity.CurrentRTC3 = Convert.ToString(row[CurrentRTC3]);
            if (NotNullAndNotDBNull(row, PreviousRTC3)) rTCUpdateEntity.PreviousRTC3 = Convert.ToString(row[PreviousRTC3]);
            if (NotNullAndNotDBNull(row, CurrentRTC4)) rTCUpdateEntity.CurrentRTC4 = Convert.ToString(row[CurrentRTC4]);
            if (NotNullAndNotDBNull(row, PreviousRTC4)) rTCUpdateEntity.PreviousRTC4 = Convert.ToString(row[PreviousRTC4]);
            if (NotNullAndNotDBNull(row, CurrentRTC5)) rTCUpdateEntity.CurrentRTC5 = Convert.ToString(row[CurrentRTC5]);
            if (NotNullAndNotDBNull(row, PreviousRTC5)) rTCUpdateEntity.PreviousRTC5 = Convert.ToString(row[PreviousRTC5]);
            if (NotNullAndNotDBNull(row, CurrentRTC6)) rTCUpdateEntity.CurrentRTC6 = Convert.ToString(row[CurrentRTC6]);
            if (NotNullAndNotDBNull(row, PreviousRTC6)) rTCUpdateEntity.PreviousRTC6 = Convert.ToString(row[PreviousRTC6]);
            if (NotNullAndNotDBNull(row, CurrentRTC7)) rTCUpdateEntity.CurrentRTC7 = Convert.ToString(row[CurrentRTC7]);
            if (NotNullAndNotDBNull(row, PreviousRTC7)) rTCUpdateEntity.PreviousRTC7 = Convert.ToString(row[PreviousRTC7]);
            if (NotNullAndNotDBNull(row, CurrentRTC8)) rTCUpdateEntity.CurrentRTC8 = Convert.ToString(row[CurrentRTC8]);
            if (NotNullAndNotDBNull(row, PreviousRTC8)) rTCUpdateEntity.PreviousRTC8 = Convert.ToString(row[PreviousRTC8]);
            if (NotNullAndNotDBNull(row, CurrentRTC9)) rTCUpdateEntity.CurrentRTC9 = Convert.ToString(row[CurrentRTC9]);
            if (NotNullAndNotDBNull(row, PreviousRTC9)) rTCUpdateEntity.PreviousRTC9 = Convert.ToString(row[PreviousRTC9]);
            if (NotNullAndNotDBNull(row, CurrentRTC10)) rTCUpdateEntity.CurrentRTC10 = Convert.ToString(row[CurrentRTC10]);
            if (NotNullAndNotDBNull(row, PreviousRTC10)) rTCUpdateEntity.PreviousRTC10 = Convert.ToString(row[PreviousRTC10]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) rTCUpdateEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]); 

            return rTCUpdateEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
