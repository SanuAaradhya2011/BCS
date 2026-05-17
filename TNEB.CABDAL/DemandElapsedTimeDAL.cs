/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon								|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{ 
    public class DemandElapsedTimeDAL : DALBase
    {
         private string  DemandElapsedTime_ID= "DemandElapsedTime_ID";
        private string RisingDemandKW = "RisingDemandKW";
        private string ElapsedTimeKW = "ElapsedTimeKW";
        private string RisingDemandKVA = "RisingDemandKVA";
         private string ElapsedTimeKVA = "ElapsedTimeKVA";
        public DemandElapsedTimeDAL()
            : base("MeterData_DemandElapsedTime", "DemandElapsedTime_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            DemandElapsedTimeEntity demandElapsedTimeEntity = entity as DemandElapsedTimeEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_DemandElapsedTime(RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA) values(");
                builder.Append(string.Concat(ParameterName(RisingDemandKW), ","));
                builder.Append(string.Concat(ParameterName(ElapsedTimeKW), ",")); 
                builder.Append(string.Concat(ParameterName(RisingDemandKVA), ","));
                builder.Append(string.Concat(ParameterName(ElapsedTimeKVA), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(RisingDemandKW), demandElapsedTimeEntity.RisingDemandKW, DbType.String, 40);
                request.AddParamter(ParameterName(ElapsedTimeKW), demandElapsedTimeEntity.ElapsedTimeKW, DbType.String, 40);
                request.AddParamter(ParameterName(RisingDemandKVA), demandElapsedTimeEntity.RisingDemandKVA, DbType.String, 40);
                request.AddParamter(ParameterName(ElapsedTimeKVA), demandElapsedTimeEntity.ElapsedTimeKVA, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Demand Elapsed Time  Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                demandElapsedTimeEntity.DemandElapsedTime_ID = long.Parse(this.GetPK());
            return demandElapsedTimeEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            DemandElapsedTimeEntity demandElapsedTimeEntity = entity as DemandElapsedTimeEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_DemandElapsedTime where ");
                builder.Append(string.Concat(DemandElapsedTime_ID, "=", ParameterName(DemandElapsedTime_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DemandElapsedTime_ID), demandElapsedTimeEntity.DemandElapsedTime_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Demand Elapsed Time  Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            DemandElapsedTimeEntity demandElapsedTimeEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DemandElapsedTime_ID,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA from MeterData_DemandElapsedTime where ");
                builder.Append(string.Concat(DemandElapsedTime_ID, "=", ParameterName(DemandElapsedTime_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DemandElapsedTime_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    demandElapsedTimeEntity = (DemandElapsedTimeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Demand Elapsed Time  viewed."));

            }
            catch (CABException)
            { 
            }
            return demandElapsedTimeEntity;
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
                builder.Append("Select DemandElapsedTime_ID,RisingDemandKW,ElapsedTimeKW,RisingDemandKVA,ElapsedTimeKVA from MeterData_DemandElapsedTime ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Demand Elapsed Time  viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            DemandElapsedTimeEntity demandElapsedTimeEntity = new DemandElapsedTimeEntity();
            if (NotNullAndNotDBNull(row, DemandElapsedTime_ID)) demandElapsedTimeEntity.DemandElapsedTime_ID = Convert.ToInt32(row[DemandElapsedTime_ID]);
            if (NotNullAndNotDBNull(row, RisingDemandKW)) demandElapsedTimeEntity.RisingDemandKW = Convert.ToString(row[RisingDemandKW]);
            if (NotNullAndNotDBNull(row, ElapsedTimeKW)) demandElapsedTimeEntity.ElapsedTimeKW = Convert.ToString(row[ElapsedTimeKW]);
            if (NotNullAndNotDBNull(row, RisingDemandKVA)) demandElapsedTimeEntity.RisingDemandKVA = Convert.ToString(row[RisingDemandKVA]);
            if (NotNullAndNotDBNull(row, ElapsedTimeKVA)) demandElapsedTimeEntity.ElapsedTimeKVA = Convert.ToString(row[ElapsedTimeKVA]);
            return demandElapsedTimeEntity;
        } 
    } 
}
