/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
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
   public class CumulativeDemandBillingTimeStampDAL : DALBase
    {   

        private string CumulativeDemandBillingTimeStamp_ID = "CumulativeDemandBillingTimeStamp_ID";
        private string CumulativeMD1 = "CumulativeMD1";
        private string CumulativeMD1TimeStamp = "CumulativeMD1TimeStamp";
        private string CumulativeMD2 = "CumulativeMD2"; 
        private string CumulativeMD2TimeStamp = "CumulativeMD2TimeStamp"; 
        private string CumulativeMD3 = "CumulativeMD3"; 
        private string CumulativeMD3TimeStamp = "CumulativeMD3TimeStamp";
        public CumulativeDemandBillingTimeStampDAL()
            : base("MeterData_CumulativeDemandBillingTimeStamp", "CumulativeDemandBillingTimeStamp_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CumulativeDemandBillingTimeStampEntity cumulativeDemandBillingTimeStampEntity = entity as CumulativeDemandBillingTimeStampEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_CumulativeDemandBillingTimeStamp(CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,CumulativeMD3,CumulativeMD3TimeStamp) values(");
                builder.Append(string.Concat(ParameterName(CumulativeMD1), ","));
                builder.Append(string.Concat(ParameterName(CumulativeMD1TimeStamp), ",")); 
                builder.Append(string.Concat(ParameterName(CumulativeMD2), ","));
                builder.Append(string.Concat(ParameterName(CumulativeMD2TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(CumulativeMD3), ","));  
                builder.Append(string.Concat(ParameterName(CumulativeMD3TimeStamp), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeMD1), cumulativeDemandBillingTimeStampEntity.CumulativeMD1, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD1TimeStamp), cumulativeDemandBillingTimeStampEntity.CumulativeMD1TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD2), cumulativeDemandBillingTimeStampEntity.CumulativeMD2, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD2TimeStamp), cumulativeDemandBillingTimeStampEntity.CumulativeMD2TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD3), cumulativeDemandBillingTimeStampEntity.CumulativeMD3, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD3TimeStamp), cumulativeDemandBillingTimeStampEntity.CumulativeMD3TimeStamp, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Billing Time Stamp Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                cumulativeDemandBillingTimeStampEntity.CumulativeDemandBillingTimeStamp_ID = long.Parse(this.GetPK());
            return cumulativeDemandBillingTimeStampEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CumulativeDemandBillingTimeStampEntity cumulativeDemandBillingTimeStampEntity = entity as CumulativeDemandBillingTimeStampEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_CumulativeDemandBillingTimeStamp where ");
                builder.Append(string.Concat(CumulativeDemandBillingTimeStamp_ID, "=", ParameterName(CumulativeDemandBillingTimeStamp_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeDemandBillingTimeStamp_ID), cumulativeDemandBillingTimeStampEntity.CumulativeDemandBillingTimeStamp_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Billing Time Stamp Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CumulativeDemandBillingTimeStampEntity CumulativeDemandBillingTimeStampEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CumulativeDemandBillingTimeStamp_ID,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,CumulativeMD3,CumulativeMD3TimeStamp from MeterData_CumulativeDemandBillingTimeStamp where ");
                builder.Append(string.Concat(CumulativeDemandBillingTimeStamp_ID, "=", ParameterName(CumulativeDemandBillingTimeStamp_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeDemandBillingTimeStamp_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    CumulativeDemandBillingTimeStampEntity = (CumulativeDemandBillingTimeStampEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Billing Time Stamp viewed."));

            }
            catch (CABException)
            { 
            }
            return CumulativeDemandBillingTimeStampEntity;
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
                builder.Append("Select CumulativeDemandBillingTimeStamp_ID,CumulativeMD1,CumulativeMD1TimeStamp,CumulativeMD2,CumulativeMD2TimeStamp,CumulativeMD3,CumulativeMD3TimeStamp from MeterData_CumulativeDemandBillingTimeStamp");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Billing Time Stamp viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            CumulativeDemandBillingTimeStampEntity cumulativeDemandBillingTimeStampEntity = new CumulativeDemandBillingTimeStampEntity();
            if (NotNullAndNotDBNull(row, CumulativeDemandBillingTimeStamp_ID)) cumulativeDemandBillingTimeStampEntity.CumulativeDemandBillingTimeStamp_ID = Convert.ToInt32(row[CumulativeDemandBillingTimeStamp_ID]);
            if (NotNullAndNotDBNull(row, CumulativeMD1)) cumulativeDemandBillingTimeStampEntity.CumulativeMD1 = Convert.ToString(row[CumulativeMD1]);
            if (NotNullAndNotDBNull(row, CumulativeMD1TimeStamp)) cumulativeDemandBillingTimeStampEntity.CumulativeMD1TimeStamp = Convert.ToString(row[CumulativeMD1TimeStamp]);
            if (NotNullAndNotDBNull(row, CumulativeMD2)) cumulativeDemandBillingTimeStampEntity.CumulativeMD2 = Convert.ToString(row[CumulativeMD2]);
            if (NotNullAndNotDBNull(row, CumulativeMD2TimeStamp)) cumulativeDemandBillingTimeStampEntity.CumulativeMD2TimeStamp = Convert.ToString(row[CumulativeMD2TimeStamp]);
            if (NotNullAndNotDBNull(row, CumulativeMD3)) cumulativeDemandBillingTimeStampEntity.CumulativeMD3 = Convert.ToString(row[CumulativeMD3]);
            if (NotNullAndNotDBNull(row, CumulativeMD3TimeStamp)) cumulativeDemandBillingTimeStampEntity.CumulativeMD3TimeStamp = Convert.ToString(row[CumulativeMD3TimeStamp]);
            return cumulativeDemandBillingTimeStampEntity;
        } 
    }
}
