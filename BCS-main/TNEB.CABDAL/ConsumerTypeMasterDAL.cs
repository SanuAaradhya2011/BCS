/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class ConsumerTypeMasterDAL : DALBase
    { 
        private string ConsumerType_ID = "ConsumerType_ID";
        private string ConsumerType_Name = "ConsumerType_Name";

        public void InsertDefaultConsumerType()
        { 
            string[] qry = new string[8];
            qry[0] = "Insert Into consumertype_master(ConsumerType_Name) values('Feeder')";
            qry[1] = "Insert Into consumertype_master(ConsumerType_Name) values('Substation')";
            qry[2] = "Insert Into consumertype_master(ConsumerType_Name) values('Industrial')";
            qry[3] = "Insert Into consumertype_master(ConsumerType_Name) values('DT')";
            qry[4] = "Insert Into consumertype_master(ConsumerType_Name) values('Commercial')";
            qry[5] = "Insert Into consumertype_master(ConsumerType_Name) values('Residential')";
            qry[6] = "Insert Into consumertype_master(ConsumerType_Name) values('Agriculture')";
            qry[7] = "Insert Into consumertype_master(ConsumerType_Name) values('Others')"; 
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 8; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }

        public override IEntity InsertData(IEntity entity)
        {
            ConsumerTypeMasterEntity consumerTypeMasterEntity = null;
            if (entity == null)
                return consumerTypeMasterEntity;
              consumerTypeMasterEntity = entity as ConsumerTypeMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into consumertype_master(ConsumerType_Name) values(");
                builder.Append(string.Concat(ParameterName(ConsumerType_Name), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerType_Name), consumerTypeMasterEntity.ConsumerType_Name, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", consumerTypeMasterEntity.ConsumerType_Name, " added"));
                Flag = true;
            }
            catch (CABException)
            {
                consumerTypeMasterEntity = null;
            }
            return consumerTypeMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerTypeMasterEntity consumerTypeMasterEntity = entity as ConsumerTypeMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update consumertype_master Set ");
                builder.Append(string.Concat( ConsumerType_Name, "=", ParameterName(ConsumerType_Name ))); 
                builder.Append(string.Concat(" Where ", ConsumerType_ID, "=", ParameterName(ConsumerType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerType_ID), consumerTypeMasterEntity.ConsumerType_ID, DbType.Int32);
                request.AddParamter(ParameterName(ConsumerType_Name), consumerTypeMasterEntity.ConsumerType_Name, DbType.String,50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", consumerTypeMasterEntity.ConsumerType_Name, " modified"));
                Flag = true;
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerTypeMasterEntity consumerTypeMasterEntity = entity as ConsumerTypeMasterEntity;
                if (consumerTypeMasterEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From consumertype_master ");
                builder.Append(string.Concat(" Where ", ConsumerType_ID, "=", ParameterName(ConsumerType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerType_ID), consumerTypeMasterEntity.ConsumerType_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", consumerTypeMasterEntity.ConsumerType_Name, " deleted"));
                Flag = true;
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            ConsumerTypeMasterEntity consumerTypeMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ConsumerType_ID,ConsumerType_Name from consumertype_master where ");
                builder.Append(string.Concat(ConsumerType_ID, "=", ParameterName(ConsumerType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerType_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    consumerTypeMasterEntity = (ConsumerTypeMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException)
            {
                consumerTypeMasterEntity = null;
            }
            return consumerTypeMasterEntity;
        }

        public IEntity GetDetailData(string typeName)
        {
            ConsumerTypeMasterEntity consumerTypeMasterEntity = new ConsumerTypeMasterEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ConsumerType_ID,ConsumerType_Name from consumertype_master where ");
                builder.Append(string.Concat(ConsumerType_Name, "=", ParameterName(ConsumerType_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerType_Name), typeName, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    consumerTypeMasterEntity = (ConsumerTypeMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException)
            {
            }
            return consumerTypeMasterEntity;
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
                builder.Append("Select ConsumerType_ID,ConsumerType_Name from consumertype_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            ConsumerTypeMasterEntity consumerTypeMasterEntity = new ConsumerTypeMasterEntity();
            if (NotNullAndNotDBNull(row, ConsumerType_ID)) consumerTypeMasterEntity.ConsumerType_ID = Convert.ToInt32(row[ConsumerType_ID]);
            if (NotNullAndNotDBNull(row, ConsumerType_Name)) consumerTypeMasterEntity.ConsumerType_Name = Convert.ToString(row[ConsumerType_Name]); 
            return consumerTypeMasterEntity;
        }
 

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}


