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
	 public class ProgrammingDAL : DALBase
    {
        private string Programming_ID = "Programming_ID";
        private string TotalProgrammingUpdates = "TotalProgrammingUpdates";
        private string UpdateSequence = "UpdateSequence"; 
        private string LastTimestamp = "LastTimestamp";
        private string Description1 = "Description1";
        private string Description2 = "Description2"; 
        private string Description3 = "Description3";
        private string Description4 = "Description4";
        private string Description5 = "Description5";
        private string Description6 = "Description6";
        private string Description7 = "Description7";
        private string Description8 = "Description8";
        private string Description9 = "Description9";
        private string Description10 = "Description10";
        private string Description11 = "Description11";
        private string Description12 = "Description12";
        private string Description13 = "Description13";
        private string Description14 = "Description14";
        private string Description15 = "Description15";
        private string Description16 = "Description16";
        private string Description17 = "Description17";
        private string Description18 = "Description18";
        private string Description19 = "Description19";

        private string MeterData_ID = "MeterData_ID";

        public ProgrammingDAL()
            : base("MeterData_Programming", "Programming_ID")
        {
        }

        private DataRequest GetRequest(IEntity entity)
        {
            ProgrammingEntity programmingEntity = entity as ProgrammingEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into MeterData_Programming(TotalProgrammingUpdates,UpdateSequence,LastTimestamp,Description1,Description2,Description3,");
            builder.Append("Description4,Description5,Description6,Description7,Description8,Description9,Description10,");
            builder.Append("Description11,Description12,Description13,Description14,Description15,Description16,Description17,Description18,Description19,MeterData_ID) values(");
            builder.Append(string.Concat(ParameterName(TotalProgrammingUpdates), ","));
            builder.Append(string.Concat(ParameterName(UpdateSequence), ","));
            builder.Append(string.Concat(ParameterName(LastTimestamp), ","));
            builder.Append(string.Concat(ParameterName(Description1), ","));
            builder.Append(string.Concat(ParameterName(Description2), ","));
            builder.Append(string.Concat(ParameterName(Description3), ","));
            builder.Append(string.Concat(ParameterName(Description4), ","));
            builder.Append(string.Concat(ParameterName(Description5), ","));
            builder.Append(string.Concat(ParameterName(Description6), ","));
            builder.Append(string.Concat(ParameterName(Description7), ","));
            builder.Append(string.Concat(ParameterName(Description8), ","));
            builder.Append(string.Concat(ParameterName(Description9), ","));
            builder.Append(string.Concat(ParameterName(Description10), ","));
            builder.Append(string.Concat(ParameterName(Description11), ","));
            builder.Append(string.Concat(ParameterName(Description12), ","));
            builder.Append(string.Concat(ParameterName(Description13), ","));
            builder.Append(string.Concat(ParameterName(Description14), ","));
            builder.Append(string.Concat(ParameterName(Description15), ","));
            builder.Append(string.Concat(ParameterName(Description16), ","));
            builder.Append(string.Concat(ParameterName(Description17), ","));
            builder.Append(string.Concat(ParameterName(Description18), ","));
            builder.Append(string.Concat(ParameterName(Description19), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(TotalProgrammingUpdates), programmingEntity.TotalProgrammingUpdates, DbType.String, 20);
            request.AddParamter(ParameterName(UpdateSequence), programmingEntity.UpdateSequence, DbType.String, 20);
            request.AddParamter(ParameterName(LastTimestamp), programmingEntity.LastTimestamp, DbType.String, 20);
            request.AddParamter(ParameterName(Description1), programmingEntity.Description1, DbType.String, 50);
            request.AddParamter(ParameterName(Description2), programmingEntity.Description2, DbType.String, 50);
            request.AddParamter(ParameterName(Description3), programmingEntity.Description3, DbType.String, 50);
            request.AddParamter(ParameterName(Description4), programmingEntity.Description4, DbType.String, 50);
            request.AddParamter(ParameterName(Description5), programmingEntity.Description5, DbType.String, 50);
            request.AddParamter(ParameterName(Description6), programmingEntity.Description6, DbType.String, 50);
            request.AddParamter(ParameterName(Description7), programmingEntity.Description7, DbType.String, 50);
            request.AddParamter(ParameterName(Description8), programmingEntity.Description8, DbType.String, 50);
            request.AddParamter(ParameterName(Description9), programmingEntity.Description9, DbType.String, 50);
            request.AddParamter(ParameterName(Description10), programmingEntity.Description10, DbType.String, 50);
            request.AddParamter(ParameterName(Description11), programmingEntity.Description11, DbType.String, 50);
            request.AddParamter(ParameterName(Description12), programmingEntity.Description12, DbType.String, 50);
            request.AddParamter(ParameterName(Description13), programmingEntity.Description13, DbType.String, 50);
            request.AddParamter(ParameterName(Description14), programmingEntity.Description14, DbType.String, 50);
            request.AddParamter(ParameterName(Description15), programmingEntity.Description15, DbType.String, 50);
            request.AddParamter(ParameterName(Description16), programmingEntity.Description16, DbType.String, 50);
            request.AddParamter(ParameterName(Description17), programmingEntity.Description17, DbType.String, 50);
            request.AddParamter(ParameterName(Description18), programmingEntity.Description18, DbType.String, 50);
            request.AddParamter(ParameterName(Description19), programmingEntity.Description19, DbType.String, 50);
            request.AddParamter(ParameterName(MeterData_ID), programmingEntity.MeterData_ID, DbType.Int64);
            return request;
        }

        public override IEntity InsertData(IEntity entity)
        {     
            ProgrammingEntity programmingEntity = entity as ProgrammingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper(); 
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                programmingEntity.Programming_ID = long.Parse(this.GetPK());
            return programmingEntity;
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records added"));
            }
            catch (Exception) { }
            return null;
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
                builder.Append("Delete from MeterData_Programming where ");
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
            ProgrammingEntity programmingEntity = entity as ProgrammingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_Programming where ");
                builder.Append(string.Concat(Programming_ID, "=", ParameterName(Programming_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Programming_ID), programmingEntity.Programming_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }
        public int GetTotalProgrammingUpdates(int meterDataId)
        {
            int total = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Max(TotalProgrammingUpdates) from MeterData_Programming where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                object obj = helper.ExecuteScalar(request);
                if(!(obj == DBNull.Value))
                {
                  total = Convert.ToInt32(obj);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records viewed"));
            }
            catch (Exception)
            {
                total = 0;
            }
            return total;
        }
        public DataSet GetProgrammingList(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TotalProgrammingUpdates,UpdateSequence,LastTimestamp,Description1,Description2,Description3,Description4,Description5,Description6,Description7,Description8,Description9,Description10,Description11,Description12,Description13,Description14,Description15,Description16,Description17,Description18,Description19 from MeterData_Programming where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID))); //Programming_ID desc
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public override IEntity GetDetailData(int id)
        {
            ProgrammingEntity programmingEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Programming_ID,TotalProgrammingUpdates,UpdateSequence,LastTimestamp,Description1,Description2,Description3,Description4,Description5,Description6,Description7,Description8,Description9,Description10,Description11,Description12,Description13,Description14,Description15,Description16,Description17,Description18,Description19,MeterData_ID from MeterData_Programming where ");
                builder.Append(string.Concat(Programming_ID, "=", ParameterName(Programming_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Programming_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    programmingEntity = (ProgrammingEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records viewed"));
            }
            catch (CABException)
            {
            }
            return programmingEntity;
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
                builder.Append("Select Programming_ID,TotalProgrammingUpdates,UpdateSequence,LastTimestamp,Description1,Description2,Description3,Description4,Description5,Description6,Description7,Description8,Description9,Description10,Description11,Description12,Description13,Description14,Description15,Description16,Description17,Description18,Description19,MeterData_ID from MeterData_Programming ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Programming Records viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            ProgrammingEntity programmingEntity = new ProgrammingEntity();
            if (NotNullAndNotDBNull(row, Programming_ID)) programmingEntity.Programming_ID = Convert.ToInt64(row[Programming_ID]);
            if (NotNullAndNotDBNull(row, TotalProgrammingUpdates)) programmingEntity.TotalProgrammingUpdates = Convert.ToString(row[TotalProgrammingUpdates]);
            if (NotNullAndNotDBNull(row, UpdateSequence)) programmingEntity.UpdateSequence = Convert.ToString(row[UpdateSequence]);
            if (NotNullAndNotDBNull(row, LastTimestamp)) programmingEntity.LastTimestamp = Convert.ToString(row[LastTimestamp]);
            if (NotNullAndNotDBNull(row, Description1)) programmingEntity.Description1 = Convert.ToString(row[Description1]);
            if (NotNullAndNotDBNull(row, Description2)) programmingEntity.Description2 = Convert.ToString(row[Description2]);
            if (NotNullAndNotDBNull(row, Description3)) programmingEntity.Description3 = Convert.ToString(row[Description3]);
            if (NotNullAndNotDBNull(row, Description4)) programmingEntity.Description4 = Convert.ToString(row[Description4]);
            if (NotNullAndNotDBNull(row, Description5)) programmingEntity.Description5 = Convert.ToString(row[Description5]);
            if (NotNullAndNotDBNull(row, Description6)) programmingEntity.Description6 = Convert.ToString(row[Description6]);
            if (NotNullAndNotDBNull(row, Description7)) programmingEntity.Description7 = Convert.ToString(row[Description7]);
            if (NotNullAndNotDBNull(row, Description8)) programmingEntity.Description8 = Convert.ToString(row[Description8]);
            if (NotNullAndNotDBNull(row, Description9)) programmingEntity.Description9 = Convert.ToString(row[Description9]);
            if (NotNullAndNotDBNull(row, Description10)) programmingEntity.Description10 = Convert.ToString(row[Description10]);
            if (NotNullAndNotDBNull(row, Description11)) programmingEntity.Description11 = Convert.ToString(row[Description11]);
            if (NotNullAndNotDBNull(row, Description12)) programmingEntity.Description12 = Convert.ToString(row[Description12]);
            if (NotNullAndNotDBNull(row, Description13)) programmingEntity.Description13 = Convert.ToString(row[Description13]);
            if (NotNullAndNotDBNull(row, Description14)) programmingEntity.Description14 = Convert.ToString(row[Description14]);
            if (NotNullAndNotDBNull(row, Description15)) programmingEntity.Description15 = Convert.ToString(row[Description15]);
            if (NotNullAndNotDBNull(row, Description16)) programmingEntity.Description16 = Convert.ToString(row[Description16]);
            if (NotNullAndNotDBNull(row, Description17)) programmingEntity.Description17 = Convert.ToString(row[Description17]);
            if (NotNullAndNotDBNull(row, Description18)) programmingEntity.Description17 = Convert.ToString(row[Description18]);
            if (NotNullAndNotDBNull(row, Description19)) programmingEntity.Description17 = Convert.ToString(row[Description19]); 
            if (NotNullAndNotDBNull(row, MeterData_ID)) programmingEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]); 

            return programmingEntity;
        }
    }
}
