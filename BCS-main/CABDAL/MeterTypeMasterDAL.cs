
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
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class MeterTypeMasterDAL : DALBase
    {
        private string MeterType_ID = "MeterType_ID";
        private string MeterType_Name = "MeterType_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterTypeMasterDAL).ToString());

        public void InsertDefaultMeterType()
        {  
            string[] qry = new string[3]; 
            //qry[0] = "Insert Into metertype_master(MeterType_Name) values('1P3W')";
            //BhardwajG : Changing the value of meter type according to the general profile
            qry[0] = "Insert Into metertype_master(MeterType_Name) values('3P-3W')";
            qry[1] = "Insert Into metertype_master(MeterType_Name) values('3P-4W')";
            qry[2] = "Insert Into metertype_master(MeterType_Name) values('1P-2W')";
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 3; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
        }


        public override IEntity InsertData(IEntity entity)
        {
            MeterTypeMasterEntity meterTypeMasterEntity = null;
            if (entity == null)
                return meterTypeMasterEntity;
            meterTypeMasterEntity = entity as MeterTypeMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into metertype_master(MeterType_Name) values(");
                builder.Append(string.Concat(ParameterName(MeterType_Name), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterType_Name), meterTypeMasterEntity.MeterType_Name, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type  ", meterTypeMasterEntity.MeterType_Name, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                meterTypeMasterEntity = null;
            }
            return meterTypeMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterTypeMasterEntity meterTypeMasterEntity = entity as MeterTypeMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update metertype_master Set ");
                builder.Append(string.Concat( MeterType_Name, "=", ParameterName(MeterType_Name ))); 
                builder.Append(string.Concat(" Where ", MeterType_ID, "=", ParameterName(MeterType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterType_ID), meterTypeMasterEntity.MeterType_ID, DbType.Int32);
                request.AddParamter(ParameterName(MeterType_Name), meterTypeMasterEntity.MeterType_Name, DbType.String,50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type  ", meterTypeMasterEntity.MeterType_Name, " modified"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterTypeMasterEntity meterTypeMasterEntity = entity as MeterTypeMasterEntity;
                if (meterTypeMasterEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From metertype_master ");
                builder.Append(string.Concat(" Where ", MeterType_ID, "=", ParameterName(MeterType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterType_ID), meterTypeMasterEntity.MeterType_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type  ", meterTypeMasterEntity.MeterType_Name, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            MeterTypeMasterEntity meterTypeMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterType_ID,MeterType_Name from metertype_master where ");
                builder.Append(string.Concat(MeterType_ID, "=", ParameterName(MeterType_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterType_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    meterTypeMasterEntity = (MeterTypeMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type viewe."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                meterTypeMasterEntity = null;
            }
            return meterTypeMasterEntity;
        }

        public IEntity GetDetailData(string typeName)
        {
            MeterTypeMasterEntity meterTypeMasterEntity = new MeterTypeMasterEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterType_ID,MeterType_Name from metertype_master where ");
                builder.Append(string.Concat(MeterType_Name, "=", ParameterName(MeterType_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterType_Name), typeName, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    meterTypeMasterEntity = (MeterTypeMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string typeName)", ex);
            }
            return meterTypeMasterEntity;
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
                builder.Append("Select MeterType_ID,MeterType_Name from metertype_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            MeterTypeMasterEntity meterTypeMasterEntity = new MeterTypeMasterEntity();
            if (NotNullAndNotDBNull(row, MeterType_ID)) meterTypeMasterEntity.MeterType_ID = Convert.ToInt32(row[MeterType_ID]);
            if (NotNullAndNotDBNull(row, MeterType_Name)) meterTypeMasterEntity.MeterType_Name = Convert.ToString(row[MeterType_Name]); 
            return meterTypeMasterEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}


