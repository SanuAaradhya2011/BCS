/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved toCabcon								|
 * | 																												|
 * |											Author :Piyush Singh. 	 												|
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
    public class CumulativeDemandGeneralDAL : DALBase
    {   

        private string CumulativeDemandGeneral_ID = "CumulativeDemandGeneral_ID";
        private string CumulativeMD1 = "CumulativeMD1";
        private string CumulativeMD2 = "CumulativeMD2";
        private string CumulativeMD3 = "CumulativeMD3"; 
        public CumulativeDemandGeneralDAL()
            : base("MeterData_CumulativeDemandGeneral", "CumulativeDemandGeneral_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CumulativeDemandGeneralEntity cumulativeDemandGeneralEntity = entity as CumulativeDemandGeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_CumulativeDemandGeneral(CumulativeMD1,CumulativeMD2,CumulativeMD3) values(");
                builder.Append(string.Concat(ParameterName(CumulativeMD1), ","));
                builder.Append(string.Concat(ParameterName(CumulativeMD2), ",")); 
                builder.Append(string.Concat(ParameterName(CumulativeMD3), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeMD1), cumulativeDemandGeneralEntity.CumulativeMD1, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD2), cumulativeDemandGeneralEntity.CumulativeMD2, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeMD3), cumulativeDemandGeneralEntity.CumulativeMD3, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                cumulativeDemandGeneralEntity.CumulativeDemandGeneral_ID = long.Parse(this.GetPK());
            return cumulativeDemandGeneralEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CumulativeDemandGeneralEntity cumulativeDemandGeneralEntity = entity as CumulativeDemandGeneralEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_CumulativeDemandGeneral where ");
                builder.Append(string.Concat(CumulativeDemandGeneral_ID, "=", ParameterName(CumulativeDemandGeneral_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeDemandGeneral_ID), cumulativeDemandGeneralEntity.CumulativeDemandGeneral_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CumulativeDemandGeneralEntity cumulativeDemandGeneralEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CumulativeDemandGeneral_ID,CumulativeMD1,CumulativeMD2,CumulativeMD3 from MeterData_CumulativeDemandGeneral where ");
                builder.Append(string.Concat(CumulativeDemandGeneral_ID, "=", ParameterName(CumulativeDemandGeneral_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeDemandGeneral_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    cumulativeDemandGeneralEntity = (CumulativeDemandGeneralEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand viewed."));

            }
            catch (CABException)
            { 
            }
            return cumulativeDemandGeneralEntity;
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
                builder.Append("Select CumulativeDemandGeneral_ID,CumulativeMD1,CumulativeMD2,CumulativeMD3 from MeterData_CumulativeDemandGeneral ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Demand viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            CumulativeDemandGeneralEntity cumulativeDemandGeneralEntity = new CumulativeDemandGeneralEntity();
            if (NotNullAndNotDBNull(row, CumulativeDemandGeneral_ID)) cumulativeDemandGeneralEntity.CumulativeDemandGeneral_ID = Convert.ToInt32(row[CumulativeDemandGeneral_ID]);
            if (NotNullAndNotDBNull(row, CumulativeMD1)) cumulativeDemandGeneralEntity.CumulativeMD1 = Convert.ToString(row[CumulativeMD1]);
            if (NotNullAndNotDBNull(row, CumulativeMD2)) cumulativeDemandGeneralEntity.CumulativeMD2 = Convert.ToString(row[CumulativeMD2]);
            if (NotNullAndNotDBNull(row, CumulativeMD3)) cumulativeDemandGeneralEntity.CumulativeMD3 = Convert.ToString(row[CumulativeMD3]);
            return cumulativeDemandGeneralEntity;
        } 
    }
}
