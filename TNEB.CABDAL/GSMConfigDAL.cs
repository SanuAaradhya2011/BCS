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
using CAB.IECFramework.Utility;

namespace CAB.DALC.Data
{
    public class GSMConfigDAL : DALBase
    {
        private string Meter_ID = "Meter_ID"; 
        private string Meter_Phone = "Meter_Phone";

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return false;
            try
            {
                GSMConfigEntity gSMConfigEntity = entity as GSMConfigEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meter_master Set ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), gSMConfigEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_Phone), gSMConfigEntity.SIM_Number, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule ", gSMConfigEntity.Meter_ID, " deleted"));
                Flag = true;
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet =new DataSet();
            try
            { 
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_ID as 'Meter ID',Meter_Phone as 'Meter Phone' from Meter_Master where Meter_Status=1");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet); 
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        } 
        public   IEntity GetDetailData(string id)
        {
            GSMConfigEntity configEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), id, DbType.String,10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    if(ds.Tables.Count>0)
                    if (ds.Tables[0].Rows.Count > 0)
                        configEntity = (GSMConfigEntity)RowToEntity(ds.Tables[0].Rows[0]);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException)
            {
                configEntity = null;
            }
            return configEntity;
        }

        


        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GSMConfigEntity configEntity = new GSMConfigEntity();
            if (NotNullAndNotDBNull(row, Meter_ID)) configEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
            if (NotNullAndNotDBNull(row, Meter_Phone)) configEntity.SIM_Number = Convert.ToInt64(row[Meter_Phone]);
            return configEntity;
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEntity GetDetailData(long id)
        {
            GSMConfigEntity configEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    configEntity = (GSMConfigEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException)
            {
                configEntity = null;
            }
            return configEntity;
        }
        public int GetCount(long id)
        {
            int counter = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                        counter = ds.Tables[0].Rows.Count;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException)
            {
                counter = 0;
            }
            return counter;
        }
        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
