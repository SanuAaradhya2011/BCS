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
   public class BillingFactorDAL : DALBase
    { 
      

        private string BillingFactor_ID = "BillingFactor_ID";
        private string CTRatio = "CTRatio";
        private string AveragePowerFactor = "AveragePowerFactor";
        private string PowerOnHours = "PowerOnHours";
        private string LoadFactor = "LoadFactor";
        private string History_ID = "History_ID"; 

        public BillingFactorDAL() : base("MeterData_BillingFactor", "BillingFactor_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            BillingFactorEntity billingFactorEntity = entity as BillingFactorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_BillingFactor(CTRatio,AveragePowerFactor,PowerOnHours,LoadFactor,History_ID) values(");
                builder.Append(string.Concat(ParameterName(CTRatio), ","));
                builder.Append(string.Concat(ParameterName(AveragePowerFactor), ","));
                builder.Append(string.Concat(ParameterName(PowerOnHours), ",")); 
                builder.Append(string.Concat(ParameterName(LoadFactor), ","));
                builder.Append(string.Concat(ParameterName(History_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CTRatio), billingFactorEntity.CTRatio, DbType.String, 40);
                request.AddParamter(ParameterName(AveragePowerFactor), billingFactorEntity.AveragePowerFactor, DbType.String, 40);
                request.AddParamter(ParameterName(PowerOnHours), billingFactorEntity.PowerOnHours, DbType.String, 40);
                request.AddParamter(ParameterName(LoadFactor), billingFactorEntity.LoadFactor, DbType.String, 40);
                request.AddParamter(ParameterName(History_ID), billingFactorEntity.History_ID, DbType.Int32); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Factor Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                billingFactorEntity.BillingFactor_ID = long.Parse(this.GetPK());
            return billingFactorEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            BillingFactorEntity billingFactorEntity = entity as BillingFactorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_BillingFactor where ");
                builder.Append(string.Concat(BillingFactor_ID, "=", ParameterName(BillingFactor_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(BillingFactor_ID), billingFactorEntity.BillingFactor_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Factor Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            BillingFactorEntity billingFactorEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select BillingFactor_ID,CTRatio,AveragePowerFactor,PowerOnHours,LoadFactor,History_ID from MeterData_BillingFactor where ");
                builder.Append(string.Concat(BillingFactor_ID, "=", ParameterName(BillingFactor_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(BillingFactor_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    billingFactorEntity = (BillingFactorEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Factor viewed."));

            }
            catch (CABException)
            { 
            }
            return billingFactorEntity;
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
                builder.Append("Select BillingFactor_ID,CTRatio,AveragePowerFactor,PowerOnHours,LoadFactor,History_ID from MeterData_BillingFactor");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Factor viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            BillingFactorEntity billingFactorEntity = new BillingFactorEntity();
            if (NotNullAndNotDBNull(row, BillingFactor_ID)) billingFactorEntity.BillingFactor_ID = Convert.ToInt32(row[BillingFactor_ID]);
            if (NotNullAndNotDBNull(row, CTRatio)) billingFactorEntity.CTRatio = Convert.ToString(row[CTRatio]);
            if (NotNullAndNotDBNull(row, AveragePowerFactor)) billingFactorEntity.AveragePowerFactor = Convert.ToString(row[AveragePowerFactor]);
            if (NotNullAndNotDBNull(row, PowerOnHours)) billingFactorEntity.PowerOnHours = Convert.ToString(row[PowerOnHours]);
            if (NotNullAndNotDBNull(row, LoadFactor)) billingFactorEntity.LoadFactor = Convert.ToString(row[LoadFactor]);
            if (NotNullAndNotDBNull(row, History_ID)) billingFactorEntity.History_ID = Convert.ToInt32(row[History_ID]); 
            return billingFactorEntity;
        } 
    }
}
