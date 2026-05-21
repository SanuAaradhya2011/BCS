/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to  Cabcon							|
 * | 																												|
 * |											Author : Piyush Singh.	 												|
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
    public class CumulativeEnergyDAL : DALBase
    {   
        private string CumulativeEnergy_ID = "CumulativeEnergy_ID";
        private string CumulativeEnergyKWH = "CumulativeEnergyKWH";
        private string CumulativeEnergyKVARHLag = "CumulativeEnergyKVARHLag";
        private string CumulativeEnergyKVARHLead = "CumulativeEnergyKVARHLead";
         private string CumulativeEnergyKVAH = "CumulativeEnergyKVAH";
        public CumulativeEnergyDAL()
            : base("MeterData_CumulativeEnergy", "CumulativeEnergy_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CumulativeEnergyEntity cumulativeEnergyEntity = entity as CumulativeEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_CumulativeEnergy(CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH) values(");
                builder.Append(string.Concat(ParameterName(CumulativeEnergyKWH), ","));
                builder.Append(string.Concat(ParameterName(CumulativeEnergyKVARHLag), ",")); 
                builder.Append(string.Concat(ParameterName(CumulativeEnergyKVARHLead), ","));
                builder.Append(string.Concat(ParameterName(CumulativeEnergyKVAH), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeEnergyKWH), cumulativeEnergyEntity.CumulativeEnergyKWH, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeEnergyKVARHLag), cumulativeEnergyEntity.CumulativeEnergyKVARHLag, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeEnergyKVARHLead), cumulativeEnergyEntity.CumulativeEnergyKVARHLead, DbType.String, 40);
                request.AddParamter(ParameterName(CumulativeEnergyKVAH), cumulativeEnergyEntity.CumulativeEnergyKVAH, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Energy Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                cumulativeEnergyEntity.CumulativeEnergy_ID = long.Parse(this.GetPK());
            return cumulativeEnergyEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CumulativeEnergyEntity cumulativeEnergyEntity = entity as CumulativeEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_CumulativeEnergy where ");
                builder.Append(string.Concat(CumulativeEnergy_ID, "=", ParameterName(CumulativeEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeEnergy_ID), cumulativeEnergyEntity.CumulativeEnergy_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Energy Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CumulativeEnergyEntity cumulativeEnergyEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CumulativeEnergy_ID,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH from MeterData_CumulativeEnergy where ");
                builder.Append(string.Concat(CumulativeEnergy_ID, "=", ParameterName(CumulativeEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeEnergy_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    cumulativeEnergyEntity = (CumulativeEnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Energy viewed."));

            }
            catch (CABException)
            { 
            }
            return cumulativeEnergyEntity;
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
                builder.Append("Select CumulativeEnergy_ID,CumulativeEnergyKWH,CumulativeEnergyKVARHLag,CumulativeEnergyKVARHLead,CumulativeEnergyKVAH from MeterData_CumulativeEnergy ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Cumulative Energy viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            CumulativeEnergyEntity cumulativeEnergyEntity = new CumulativeEnergyEntity();
            if (NotNullAndNotDBNull(row, CumulativeEnergy_ID)) cumulativeEnergyEntity.CumulativeEnergy_ID = Convert.ToInt32(row[CumulativeEnergy_ID]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyKWH)) cumulativeEnergyEntity.CumulativeEnergyKWH = Convert.ToString(row[CumulativeEnergyKWH]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyKVARHLag)) cumulativeEnergyEntity.CumulativeEnergyKVARHLag = Convert.ToString(row[CumulativeEnergyKVARHLag]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyKVARHLead)) cumulativeEnergyEntity.CumulativeEnergyKVARHLead = Convert.ToString(row[CumulativeEnergyKVARHLead]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyKVAH)) cumulativeEnergyEntity.CumulativeEnergyKVAH = Convert.ToString(row[CumulativeEnergyKVAH]);
            
            return cumulativeEnergyEntity;
        } 
    }
}
