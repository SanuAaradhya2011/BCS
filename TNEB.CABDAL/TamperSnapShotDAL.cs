/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 								|
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
    public class TamperSnapShotDAL : DALBase
    {
        private string TamperSnapShot_ID = "TamperSnapShot_ID";
        private string TamperCode = "TamperCode";
        private string TamperDescription = "TamperDescription";
        private string TamperOccurredTime = "TamperOccurredTime";
        private string TamperRestoredTime = "TamperRestoredTime";
        private string RVoltageOccurred = "RVoltageOccurred";
        private string YVoltageOccurred = "YVoltageOccurred";
        private string BVoltageOccurred = "BVoltageOccurred";
        private string RCurrentOccurred = "RCurrentOccurred";
        private string YCurrentOccurred = "YCurrentOccurred";
        private string BCurrentOccurred = "BCurrentOccurred";
        private string RPFOccurred = "RPFOccurred";
        private string YPFOccurred = "YPFOccurred";
        private string BPFOccurred = "BPFOccurred";
        private string TotalPFOccurred = "TotalPFOccurred";
        private string kWhOccurred = "kWhOccurred";
        private string kVAhOccurred = "kVAhOccurred";
        private string RVoltageRestored = "RVoltageRestored";
        private string YVoltageRestored = "YVoltageRestored";
        private string BVoltageRestored = "BVoltageRestored";
        private string RCurrentRestored = "RCurrentRestored";
        private string YCurrentRestored = "YCurrentRestored";
        private string BCurrentRestored = "BCurrentRestored";
        private string RPFRestored = "RPFRestored";
        private string YPFRestored = "YPFRestored";
        private string BPFRestored = "BPFRestored";
        private string TotalPFRestored = "TotalPFRestored";
        private string kWhRestored = "kWhRestored";
        private string kVAhRestored = "kVAhRestored";
        private string MeterData_ID = "MeterData_ID";
        private string FileName = "FileName";

        private string MeterID = "MeterID";

        public TamperSnapShotDAL()
            : base("MeterData_TamperSnapShot", "TamperSnapShot_ID")
        {
        }

        private DataRequest GetRequest(IEntity entity)
        {
            TamperSnapshotEntity tamperSnapshotEntity = entity as TamperSnapshotEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into MeterData_TamperSnapShot(MeterData_ID,TamperCode,TamperDescription,TamperOccurredTime,TamperRestoredTime,RVoltageOccurred,");
            builder.Append("YVoltageOccurred,BVoltageOccurred,RCurrentOccurred,YCurrentOccurred,BCurrentOccurred,RPFOccurred,YPFOccurred,");
            builder.Append("BPFOccurred,TotalPFOccurred,kWhOccurred,kVAhOccurred,RVoltageRestored,YVoltageRestored,BVoltageRestored,RCurrentRestored,");
            builder.Append("YCurrentRestored,BCurrentRestored,RPFRestored,YPFRestored,BPFRestored,TotalPFRestored,kWhRestored,kVAhRestored) values(");
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(TamperCode), ","));
            builder.Append(string.Concat(ParameterName(TamperDescription), ","));
            builder.Append(string.Concat(ParameterName(TamperOccurredTime), ","));
            builder.Append(string.Concat(ParameterName(TamperRestoredTime), ","));
            builder.Append(string.Concat(ParameterName(RVoltageOccurred), ","));
            builder.Append(string.Concat(ParameterName(YVoltageOccurred), ","));
            builder.Append(string.Concat(ParameterName(BVoltageOccurred), ","));
            builder.Append(string.Concat(ParameterName(RCurrentOccurred), ","));
            builder.Append(string.Concat(ParameterName(YCurrentOccurred), ","));
            builder.Append(string.Concat(ParameterName(BCurrentOccurred), ","));
            builder.Append(string.Concat(ParameterName(RPFOccurred), ","));
            builder.Append(string.Concat(ParameterName(YPFOccurred), ","));
            builder.Append(string.Concat(ParameterName(BPFOccurred), ","));
            builder.Append(string.Concat(ParameterName(TotalPFOccurred), ","));
            builder.Append(string.Concat(ParameterName(kWhOccurred), ","));
            builder.Append(string.Concat(ParameterName(kVAhOccurred), ","));
            builder.Append(string.Concat(ParameterName(RVoltageRestored), ","));
            builder.Append(string.Concat(ParameterName(YVoltageRestored), ","));
            builder.Append(string.Concat(ParameterName(BVoltageRestored), ","));
            builder.Append(string.Concat(ParameterName(RCurrentRestored), ","));
            builder.Append(string.Concat(ParameterName(YCurrentRestored), ","));
            builder.Append(string.Concat(ParameterName(BCurrentRestored), ","));
            builder.Append(string.Concat(ParameterName(RPFRestored), ","));
            builder.Append(string.Concat(ParameterName(YPFRestored), ","));
            builder.Append(string.Concat(ParameterName(BPFRestored), ","));
            builder.Append(string.Concat(ParameterName(TotalPFRestored), ","));
            builder.Append(string.Concat(ParameterName(kWhRestored), ","));
            builder.Append(string.Concat(ParameterName(kVAhRestored), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(MeterData_ID), tamperSnapshotEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(TamperCode), tamperSnapshotEntity.TamperCode, DbType.Int32);
            request.AddParamter(ParameterName(TamperDescription), tamperSnapshotEntity.TamperDescription, DbType.String, 100);
            request.AddParamter(ParameterName(TamperOccurredTime), tamperSnapshotEntity.TamperOccurredTime, DbType.Int64);
            request.AddParamter(ParameterName(TamperRestoredTime), tamperSnapshotEntity.TamperRestoredTime, DbType.Int64);
            request.AddParamter(ParameterName(RVoltageOccurred), tamperSnapshotEntity.RVoltageOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(YVoltageOccurred), tamperSnapshotEntity.YVoltageOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(BVoltageOccurred), tamperSnapshotEntity.BVoltageOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(RCurrentOccurred), tamperSnapshotEntity.RCurrentOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(YCurrentOccurred), tamperSnapshotEntity.YCurrentOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(BCurrentOccurred), tamperSnapshotEntity.BCurrentOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(RPFOccurred), tamperSnapshotEntity.RPFOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(YPFOccurred), tamperSnapshotEntity.YPFOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(BPFOccurred), tamperSnapshotEntity.BPFOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(TotalPFOccurred), tamperSnapshotEntity.TotalPFOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(kWhOccurred), tamperSnapshotEntity.KWhOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(kVAhOccurred), tamperSnapshotEntity.KVAhOccurred, DbType.String, 20);
            request.AddParamter(ParameterName(RVoltageRestored), tamperSnapshotEntity.RVoltageRestored, DbType.String, 20);
            request.AddParamter(ParameterName(YVoltageRestored), tamperSnapshotEntity.YVoltageRestored, DbType.String, 20);
            request.AddParamter(ParameterName(BVoltageRestored), tamperSnapshotEntity.BVoltageRestored, DbType.String, 20);
            request.AddParamter(ParameterName(RCurrentRestored), tamperSnapshotEntity.RCurrentRestored, DbType.String, 20);
            request.AddParamter(ParameterName(YCurrentRestored), tamperSnapshotEntity.YCurrentRestored, DbType.String, 20);
            request.AddParamter(ParameterName(BCurrentRestored), tamperSnapshotEntity.BCurrentRestored, DbType.String, 20);
            request.AddParamter(ParameterName(RPFRestored), tamperSnapshotEntity.RPFRestored, DbType.String, 20);
            request.AddParamter(ParameterName(YPFRestored), tamperSnapshotEntity.YPFRestored, DbType.String, 20);
            request.AddParamter(ParameterName(BPFRestored), tamperSnapshotEntity.BPFRestored, DbType.String, 20);
            request.AddParamter(ParameterName(TotalPFRestored), tamperSnapshotEntity.TotalPFRestored, DbType.String, 20);
            request.AddParamter(ParameterName(kWhRestored), tamperSnapshotEntity.KWhRestored, DbType.String, 20);
            request.AddParamter(ParameterName(kVAhRestored), tamperSnapshotEntity.KVAhRestored, DbType.String, 20);
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Survey added"));
            }
            catch (Exception) { }
            return null;
        }
        public override IEntity InsertData(IEntity entity)
        {
            TamperSnapshotEntity tamperSnapshotEntity = entity as TamperSnapshotEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot added"));
                Flag = true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            if (Flag)
                tamperSnapshotEntity.TamperSnapShot_ID = long.Parse(this.GetPK());
            return tamperSnapshotEntity;
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
                builder.Append("Delete from MeterData_TamperSnapShot where ");
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
            TamperSnapshotEntity tamperSnapshotEntity = entity as TamperSnapshotEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TamperSnapShot where ");
                builder.Append(string.Concat(TamperSnapShot_ID, "=", ParameterName(TamperSnapShot_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperSnapShot_ID), tamperSnapshotEntity.TamperSnapShot_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }
        public DataSet DetailData(long tamperSnapShotID, long meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select RVoltageOccurred,");
                builder.Append("YVoltageOccurred,BVoltageOccurred,RCurrentOccurred,YCurrentOccurred,BCurrentOccurred,");
                builder.Append("RPFOccurred,YPFOccurred,BPFOccurred,TotalPFOccurred,kWhOccurred,kVAhOccurred,");
                builder.Append("RVoltageRestored,YVoltageRestored,BVoltageRestored,RCurrentRestored,");
                builder.Append("YCurrentRestored,BCurrentRestored,RPFRestored,YPFRestored,BPFRestored,TotalPFRestored,");
                builder.Append("kWhRestored,kVAhRestored,TamperCode from MeterData_TamperSnapShot where ");
                builder.Append(string.Concat(TamperSnapShot_ID, "=", ParameterName(TamperSnapShot_ID), " and "));
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " Order by TamperOccurredTime asc"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperSnapShot_ID), tamperSnapShotID, DbType.Int64);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
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
                builder.Append("Select TamperSnapShot_ID,MeterData_ID,TamperCode,TamperDescription,TamperOccurredTime,TamperRestoredTime,RVoltageOccurred,YVoltageOccurred,BVoltageOccurred,RCurrentOccurred,YCurrentOccurred,BCurrentOccurred,RPFOccurred,YPFOccurred,BPFOccurred,TotalPFOccurred,kWhOccurred,kVAhOccurred,RVoltageRestored,YVoltageRestored,BVoltageRestored,RCurrentRestored,YCurrentRestored,BCurrentRestored,RPFRestored,YPFRestored,BPFRestored,TotalPFRestored,kWhRestored,kVAhRestored from MeterData_TamperSnapShot ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
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
                builder.Append("Select B.TamperType as Description ,A.TamperOccurredTime as 'Occurrence Time Stamp',A.TamperRestoredTime ");
                builder.Append("as 'Restoration Time Stamp',A.TamperSnapShot_ID from meterdata_tampersnapshot A,TamperType_Master B where A.TamperCode=B.TamperTypeID and ");
                builder.Append(string.Concat("A.", MeterData_ID, "=", ParameterName(MeterData_ID), " Order by A.TamperCode"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
        public DataSet ListDataSet(long meterDataID, long tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select B.TamperType as Description ,A.TamperOccurredTime as 'Occurrence Time Stamp',A.TamperRestoredTime ");
                builder.Append("as 'Restoration Time Stamp',A.TamperSnapShot_ID from meterdata_tampersnapshot A,TamperType_Master B where A.TamperCode=B.TamperTypeID and ");
                builder.Append(string.Concat("A.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" and A.", TamperCode, "=", ParameterName(TamperCode)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                request.AddParamter(ParameterName(TamperCode), tamperCode, DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet ListDataSet(int meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.MeterID,B.TamperSnapShot_ID,B.MeterData_ID,B.TamperCode,B.TamperDescription,B.TamperOccurredTime,B.TamperRestoredTime,B.RVoltageOccurred,B.YVoltageOccurred,B.BVoltageOccurred,B.RCurrentOccurred,B.YCurrentOccurred,B.BCurrentOccurred,B.RPFOccurred,B.YPFOccurred,B.BPFOccurred,B.TotalPFOccurred,B.kWhOccurred,B.kVAhOccurred,B.RVoltageRestored,B.YVoltageRestored,B.BVoltageRestored,B.RCurrentRestored,B.YCurrentRestored,B.BCurrentRestored,B.RPFRestored,B.YPFRestored,B.BPFRestored,B.TotalPFRestored,B.kWhRestored,B.kVAhRestored from MeterData_TamperSnapShot B Inner Join meterdata A where ");
                builder.Append(string.Concat("B.", MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();
            if (NotNullAndNotDBNull(row, TamperSnapShot_ID)) tamperSnapshotEntity.TamperSnapShot_ID = Convert.ToInt32(row[TamperSnapShot_ID]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) tamperSnapshotEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, TamperCode)) tamperSnapshotEntity.TamperCode = Convert.ToInt32(row[TamperCode]);
            if (NotNullAndNotDBNull(row, TamperDescription)) tamperSnapshotEntity.TamperDescription = Convert.ToString(row[TamperDescription]);
            if (NotNullAndNotDBNull(row, TamperOccurredTime)) tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(row[TamperOccurredTime]);
            if (NotNullAndNotDBNull(row, TamperRestoredTime)) tamperSnapshotEntity.TamperRestoredTime = Convert.ToInt64(row[TamperRestoredTime]);
            if (NotNullAndNotDBNull(row, RVoltageOccurred)) tamperSnapshotEntity.RVoltageOccurred = Convert.ToString(row[RVoltageOccurred]);
            if (NotNullAndNotDBNull(row, YVoltageOccurred)) tamperSnapshotEntity.YVoltageOccurred = Convert.ToString(row[YVoltageOccurred]);
            if (NotNullAndNotDBNull(row, BVoltageOccurred)) tamperSnapshotEntity.BVoltageOccurred = Convert.ToString(row[BVoltageOccurred]);
            if (NotNullAndNotDBNull(row, RCurrentOccurred)) tamperSnapshotEntity.RCurrentOccurred = Convert.ToString(row[RCurrentOccurred]);
            if (NotNullAndNotDBNull(row, YCurrentOccurred)) tamperSnapshotEntity.YCurrentOccurred = Convert.ToString(row[YCurrentOccurred]);
            if (NotNullAndNotDBNull(row, BCurrentOccurred)) tamperSnapshotEntity.BCurrentOccurred = Convert.ToString(row[BCurrentOccurred]);
            if (NotNullAndNotDBNull(row, RPFOccurred)) tamperSnapshotEntity.RPFOccurred = Convert.ToString(row[RPFOccurred]);
            if (NotNullAndNotDBNull(row, YPFOccurred)) tamperSnapshotEntity.YPFOccurred = Convert.ToString(row[YPFOccurred]);
            if (NotNullAndNotDBNull(row, BPFOccurred)) tamperSnapshotEntity.BPFOccurred = Convert.ToString(row[BPFOccurred]);
            if (NotNullAndNotDBNull(row, TotalPFOccurred)) tamperSnapshotEntity.TotalPFOccurred = Convert.ToString(row[TotalPFOccurred]);
            if (NotNullAndNotDBNull(row, kWhOccurred)) tamperSnapshotEntity.KWhOccurred = Convert.ToString(row[kWhOccurred]);
            if (NotNullAndNotDBNull(row, kVAhOccurred)) tamperSnapshotEntity.KVAhOccurred = Convert.ToString(row[kVAhOccurred]);
            if (NotNullAndNotDBNull(row, RVoltageRestored)) tamperSnapshotEntity.RVoltageRestored = Convert.ToString(row[RVoltageRestored]);
            if (NotNullAndNotDBNull(row, YVoltageRestored)) tamperSnapshotEntity.YVoltageRestored = Convert.ToString(row[YVoltageRestored]);
            if (NotNullAndNotDBNull(row, BVoltageRestored)) tamperSnapshotEntity.BVoltageRestored = Convert.ToString(row[BVoltageRestored]);
            if (NotNullAndNotDBNull(row, RCurrentRestored)) tamperSnapshotEntity.RCurrentRestored = Convert.ToString(row[RCurrentRestored]);
            if (NotNullAndNotDBNull(row, YCurrentRestored)) tamperSnapshotEntity.YCurrentRestored = Convert.ToString(row[YCurrentRestored]);
            if (NotNullAndNotDBNull(row, BCurrentRestored)) tamperSnapshotEntity.BCurrentRestored = Convert.ToString(row[BCurrentRestored]);
            if (NotNullAndNotDBNull(row, RPFRestored)) tamperSnapshotEntity.RPFRestored = Convert.ToString(row[RPFRestored]);
            if (NotNullAndNotDBNull(row, YPFRestored)) tamperSnapshotEntity.YPFRestored = Convert.ToString(row[YPFRestored]);
            if (NotNullAndNotDBNull(row, BPFRestored)) tamperSnapshotEntity.BPFRestored = Convert.ToString(row[BPFRestored]);
            if (NotNullAndNotDBNull(row, TotalPFRestored)) tamperSnapshotEntity.TotalPFRestored = Convert.ToString(row[TotalPFRestored]);
            if (NotNullAndNotDBNull(row, kWhRestored)) tamperSnapshotEntity.KWhRestored = Convert.ToString(row[kWhRestored]);
            if (NotNullAndNotDBNull(row, kVAhRestored)) tamperSnapshotEntity.KVAhRestored = Convert.ToString(row[kVAhRestored]);

            return tamperSnapshotEntity;
        }

        public DataSet GetTamperSnapshotDataByFile(string fileName, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append("from meterdata_tampersnapshot ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", TamperCode, "=", ParameterName(TamperCode), " ", "and", " "));
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCode), tamperCode, DbType.Int32);
				request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public DataSet GetTamperSnapshotDataByMeter(string meterID, List<string> columns, int tamperCode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "ts.", column, " "));
                }
                builder.Append("from meterdata_tampersnapshot ts inner join meterdata m on ts.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where ");
                builder.Append(string.Concat("ts.", TamperCode, "=", ParameterName(TamperCode), " ", "and", " "));
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperCode), tamperCode, DbType.Int32);
				request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Tamper Snapshot viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }
    }
}