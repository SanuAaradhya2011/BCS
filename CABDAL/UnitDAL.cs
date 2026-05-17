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
    public class UnitDAL : DALBase
    {
        private string MeterUnit_ID = "MeterUnit_ID";
        private string MeterUnit_Type = "MeterUnit_Type";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UnitDAL).ToString());


        public override IEntity InsertData(IEntity entity)
        {
            UnitEntity unitEntity = null;
            if (entity == null)
                return unitEntity;
              unitEntity = entity as UnitEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterunit_master(MeterUnit_Type) values(");
                builder.Append(string.Concat(ParameterName(MeterUnit_Type), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterUnit_Type), unitEntity.MeterUnit_Type, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit ", unitEntity.MeterUnit_Type, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                unitEntity = null;
            }
            return unitEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                UnitEntity unitEntity = entity as UnitEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meterunit_master Set ");
                builder.Append(string.Concat( MeterUnit_Type, "=", ParameterName(MeterUnit_Type ))); 
                builder.Append(string.Concat(" Where ", MeterUnit_ID, "=", ParameterName(MeterUnit_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterUnit_ID), unitEntity.MeterUnit_ID, DbType.Int32);
                request.AddParamter(ParameterName(MeterUnit_Type), unitEntity.MeterUnit_Type, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit ", unitEntity.MeterUnit_Type, " modified"));
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
                UnitEntity unitEntity = entity as UnitEntity;
                if (unitEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From meterunit_master ");
                builder.Append(string.Concat(" Where ", MeterUnit_ID, "=", ParameterName(MeterUnit_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterUnit_ID), unitEntity.MeterUnit_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit ", unitEntity.MeterUnit_Type, " deleted"));
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
            UnitEntity unitEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterUnit_ID,MeterUnit_Type from meterunit_master where ");
                builder.Append(string.Concat(MeterUnit_ID, "=", ParameterName(MeterUnit_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterUnit_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    unitEntity = (UnitEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                unitEntity = null;
            }
            return unitEntity;
        }

        public IEntity GetDetailData(string CircleName)
        {
            UnitEntity unitEntity = new UnitEntity(); 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterUnit_ID,MeterUnit_Type from meterunit_master where ");
                builder.Append(string.Concat(MeterUnit_Type, "=", ParameterName(MeterUnit_Type)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterUnit_Type), CircleName, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    unitEntity = (UnitEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string CircleName)", ex);
            }
            return unitEntity;
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
                builder.Append("Select MeterUnit_ID,MeterUnit_Type from meterunit_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Unit viewed"));
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
            UnitEntity unitEntity = new UnitEntity();
            if (NotNullAndNotDBNull(row, MeterUnit_ID)) unitEntity.MeterUnit_ID = Convert.ToInt32(row[MeterUnit_ID]);
            if (NotNullAndNotDBNull(row, MeterUnit_Type)) unitEntity.MeterUnit_Type = Convert.ToString(row[MeterUnit_Type]);
            return unitEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}



