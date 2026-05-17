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
    public class DTMLoadSurveyDAL : DALBase
    {
        private string DTMLoadSurvey_ID = "DTMLoadSurvey_ID";
        private string DTMDateTime = "DTMDateTime";
        private string KWh = "KWh";
        private string KVAh = "KVAh";
        private string RPhaseKW = "RPhaseKW";
        private string YPhaseKW = "YPhaseKW";
        private string BPhaseKW = "BPhaseKW";
        private string RPhaseKVAr = "RPhaseKVAr";
        private string RPhaseType = "RPhaseType";
        private string YPhaseKVAr = "YPhaseKVAr";
        private string YPhaseType = "YPhaseType";
        private string BPhaseKVAr = "BPhaseKVAr";
        private string BPhaseType = "BPhaseType";
        private string RPhaseVoltage = "RPhaseVoltage";
        private string YPhaseVoltage = "YPhaseVoltage";
        private string BPhaseVoltage = "BPhaseVoltage";
        private string PowerDownTime = "PowerDownTime";
        private string MeterData_ID = "MeterData_ID";

        public DTMLoadSurveyDAL()
            : base("MeterData_DTMLoadSurvey", "DTMLoadSurvey_ID")
        {
        }
        public long GetToDate(long meterDataID)
        {
            long date = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Max(DTMDateTime)  from MeterData_DTMLoadSurvey where DTMDateTime!=0 and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    date = 0;
                else
                    date = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));

            }
            catch (CABException)
            {
                date = 0;
            }
            return date;
        }

        public long GetFromDate(long meterDataID)
        {
            long date = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Min(DTMDateTime)  from MeterData_DTMLoadSurvey where DTMDateTime!=0 and ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                Object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    date = 0;
                else
                    date = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey data viewed"));

            }
            catch (CABException)
            {
                date = 0;
            }
            return date;
        }
        private DataRequest GetRequest(IEntity entity)
        {
            DTMLoadSurveyEntity dTMLoadSurveyEntity = entity as DTMLoadSurveyEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into MeterData_DTMLoadSurvey(DTMDateTime,KWh,KVAh,RPhaseKW,YPhaseKW,BPhaseKW,");
            builder.Append("RPhaseKVAr,RPhaseType,YPhaseKVAr,YPhaseType,BPhaseKVAr,BPhaseType,RPhaseVoltage,");
            builder.Append("YPhaseVoltage,BPhaseVoltage,PowerDownTime,MeterData_ID) values(");
            builder.Append(string.Concat(ParameterName(DTMDateTime), ","));
            builder.Append(string.Concat(ParameterName(KWh), ","));
            builder.Append(string.Concat(ParameterName(KVAh), ","));
            builder.Append(string.Concat(ParameterName(RPhaseKW), ","));
            builder.Append(string.Concat(ParameterName(YPhaseKW), ","));
            builder.Append(string.Concat(ParameterName(BPhaseKW), ","));
            builder.Append(string.Concat(ParameterName(RPhaseKVAr), ","));
            builder.Append(string.Concat(ParameterName(RPhaseType), ","));
            builder.Append(string.Concat(ParameterName(YPhaseKVAr), ","));
            builder.Append(string.Concat(ParameterName(YPhaseType), ","));
            builder.Append(string.Concat(ParameterName(BPhaseKVAr), ","));
            builder.Append(string.Concat(ParameterName(BPhaseType), ","));
            builder.Append(string.Concat(ParameterName(RPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(YPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(BPhaseVoltage), ","));
            builder.Append(string.Concat(ParameterName(PowerDownTime), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(DTMDateTime), dTMLoadSurveyEntity.DTMDateTime, DbType.String, 20);
            request.AddParamter(ParameterName(KWh), dTMLoadSurveyEntity.KWh, DbType.String, 20);
            request.AddParamter(ParameterName(KVAh), dTMLoadSurveyEntity.KVAh, DbType.String, 20);
            request.AddParamter(ParameterName(RPhaseKW), dTMLoadSurveyEntity.RPhaseKW, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseKW), dTMLoadSurveyEntity.YPhaseKW, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseKW), dTMLoadSurveyEntity.BPhaseKW, DbType.String, 20);
            request.AddParamter(ParameterName(RPhaseKVAr), dTMLoadSurveyEntity.RPhaseKVAr, DbType.String, 20);
            request.AddParamter(ParameterName(RPhaseType), dTMLoadSurveyEntity.RPhaseType, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseKVAr), dTMLoadSurveyEntity.YPhaseKVAr, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseType), dTMLoadSurveyEntity.YPhaseType, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseKVAr), dTMLoadSurveyEntity.BPhaseKVAr, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseType), dTMLoadSurveyEntity.BPhaseType, DbType.String, 20);
            request.AddParamter(ParameterName(RPhaseVoltage), dTMLoadSurveyEntity.RPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(YPhaseVoltage), dTMLoadSurveyEntity.YPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(BPhaseVoltage), dTMLoadSurveyEntity.BPhaseVoltage, DbType.String, 20);
            request.AddParamter(ParameterName(PowerDownTime), dTMLoadSurveyEntity.PowerDownTime, DbType.String, 20);
            request.AddParamter(ParameterName(MeterData_ID), dTMLoadSurveyEntity.MeterData_ID, DbType.Int64);
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data DTM Load Survey added"));
            }
            catch (Exception) { }
            return null;
        }
        public override IEntity InsertData(IEntity entity)
        {
            DTMLoadSurveyEntity dTMLoadSurveyEntity = entity as DTMLoadSurveyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper(); 
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data DTM Load Survey added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                dTMLoadSurveyEntity.DTMLoadSurvey_ID = long.Parse(this.GetPK());
            return dTMLoadSurveyEntity;
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
                builder.Append("Delete from MeterData_DTMLoadSurvey where ");
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
            DTMLoadSurveyEntity dTMLoadSurveyEntity = entity as DTMLoadSurveyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_DTMLoadSurvey where ");
                builder.Append(string.Concat(DTMLoadSurvey_ID, "=", ParameterName(DTMLoadSurvey_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DTMLoadSurvey_ID), dTMLoadSurveyEntity.DTMLoadSurvey_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data DTM Load Survey deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            DTMLoadSurveyEntity dTMLoadSurveyEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DTMLoadSurvey_ID,DTMDateTime,KWh,KVAh,RPhaseKW,YPhaseKW,BPhaseKW,RPhaseKVAr,RPhaseType,YPhaseKVAr,YPhaseType,BPhaseKVAr,BPhaseType,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,PowerDownTim,MeterData_ID from MeterData_DTMLoadSurvey where ");
                builder.Append(string.Concat(DTMLoadSurvey_ID, "=", ParameterName(DTMLoadSurvey_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DTMLoadSurvey_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    dTMLoadSurveyEntity = (DTMLoadSurveyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("DTM Load Survey data viewed"));

            }
            catch (CABException)
            {
            }
            return dTMLoadSurveyEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DTMDateTime as 'Date Time',KWh as 'kWh',KVAh as 'kVAh',RPhaseKW as 'R Phase kW',YPhaseKW as 'Y Phase kW',BPhaseKW as 'B Phase kW',RPhaseKVAr as 'R Phase kVAr',RPhaseType as 'R Phase Type',YPhaseKVAr as 'Y Phase kVAr',YPhaseType as 'Y Phase Type',BPhaseKVAr as 'B Phase kVAr',BPhaseType as 'B Phase Type',RPhaseVoltage as 'R Phase Voltage',YPhaseVoltage as 'Y Phase Voltage',BPhaseVoltage as 'B Phase Voltage',PowerDownTime as 'Power Down Time' from MeterData_DTMLoadSurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(DTMDateTime, ">=", ParameterName("FromDate"), " and "));
                builder.Append(string.Concat(DTMDateTime, "<=", ParameterName("ToDate")));
                builder.Append(string.Concat(" order by DTMDateTime"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName("FromDate"), fromDate, DbType.Int64);
                request.AddParamter(ParameterName("ToDate"), toDate, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("DTM Load Survey data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DTMLoadSurvey_ID,DTMDateTime,KWh,KVAh,RPhaseKW,YPhaseKW,BPhaseKW,RPhaseKVAr,RPhaseType,YPhaseKVAr,YPhaseType,BPhaseKVAr,BPhaseType,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,PowerDownTime,MeterData_ID from MeterData_DTMLoadSurvey ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("DTM Load Survey data viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet ListDataSet(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DTMDateTime,KWh,KVAh,RPhaseKW,YPhaseKW,BPhaseKW,RPhaseKVAr,RPhaseType,YPhaseKVAr,YPhaseType,BPhaseKVAr,BPhaseType,RPhaseVoltage,YPhaseVoltage,BPhaseVoltage,PowerDownTime,MeterData_ID from MeterData_DTMLoadSurvey where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("DTM Load Survey data viewed"));
            }
            catch (Exception)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DTMLoadSurveyEntity dTMLoadSurveyEntity = new DTMLoadSurveyEntity();
            if (NotNullAndNotDBNull(row, DTMLoadSurvey_ID)) dTMLoadSurveyEntity.DTMLoadSurvey_ID = Convert.ToInt64(row[DTMLoadSurvey_ID]);
            if (NotNullAndNotDBNull(row, DTMDateTime)) dTMLoadSurveyEntity.DTMDateTime = Convert.ToInt64(row[DTMDateTime]);
            if (NotNullAndNotDBNull(row, KWh)) dTMLoadSurveyEntity.KWh = Convert.ToString(row[KWh]);
            if (NotNullAndNotDBNull(row, KVAh)) dTMLoadSurveyEntity.KVAh = Convert.ToString(row[KVAh]);
            if (NotNullAndNotDBNull(row, RPhaseKW)) dTMLoadSurveyEntity.RPhaseKW = Convert.ToString(row[RPhaseKW]);
            if (NotNullAndNotDBNull(row, YPhaseKW)) dTMLoadSurveyEntity.YPhaseKW = Convert.ToString(row[YPhaseKW]);
            if (NotNullAndNotDBNull(row, BPhaseKW)) dTMLoadSurveyEntity.BPhaseKW = Convert.ToString(row[BPhaseKW]);
            if (NotNullAndNotDBNull(row, RPhaseKVAr)) dTMLoadSurveyEntity.RPhaseKVAr = Convert.ToString(row[RPhaseKVAr]);
            if (NotNullAndNotDBNull(row, RPhaseType)) dTMLoadSurveyEntity.RPhaseType = Convert.ToString(row[RPhaseType]);
            if (NotNullAndNotDBNull(row, YPhaseKVAr)) dTMLoadSurveyEntity.YPhaseKVAr = Convert.ToString(row[YPhaseKVAr]);
            if (NotNullAndNotDBNull(row, YPhaseType)) dTMLoadSurveyEntity.YPhaseType = Convert.ToString(row[YPhaseType]);
            if (NotNullAndNotDBNull(row, BPhaseKVAr)) dTMLoadSurveyEntity.BPhaseKVAr = Convert.ToString(row[BPhaseKVAr]);
            if (NotNullAndNotDBNull(row, BPhaseType)) dTMLoadSurveyEntity.BPhaseType = Convert.ToString(row[BPhaseType]);
            if (NotNullAndNotDBNull(row, RPhaseVoltage)) dTMLoadSurveyEntity.RPhaseVoltage = Convert.ToString(row[RPhaseVoltage]);
            if (NotNullAndNotDBNull(row, YPhaseVoltage)) dTMLoadSurveyEntity.YPhaseVoltage = Convert.ToString(row[YPhaseVoltage]);
            if (NotNullAndNotDBNull(row, BPhaseVoltage)) dTMLoadSurveyEntity.BPhaseVoltage = Convert.ToString(row[BPhaseVoltage]);
            if (NotNullAndNotDBNull(row, PowerDownTime)) dTMLoadSurveyEntity.PowerDownTime = Convert.ToString(row[PowerDownTime]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) dTMLoadSurveyEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);

            return dTMLoadSurveyEntity;
        }
    }
}
