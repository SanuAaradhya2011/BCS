/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author :Mahadavan
 *                                                      Modified by Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using System.Collections.Generic;
using System.Data;
using CAB.Entity;
using System;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class DesignationMasterDAL : DALBase
    {
        private string Designation_ID = "Designation_ID";
        private string Designation_Name = "Designation_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DesignationMasterDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            DesignationMasterEntity designationMasterEntity = null;
            if (entity == null)
                return designationMasterEntity;
              designationMasterEntity = entity as DesignationMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into Designation_Master(Designation_Name) values(");
                builder.Append(string.Concat(ParameterName(Designation_Name), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Designation_Name), designationMasterEntity.Designation_Name, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation ", designationMasterEntity.Designation_Name, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                designationMasterEntity = null;
            }
            return designationMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                DesignationMasterEntity designationMasterEntity = entity as DesignationMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update Designation_Master Set ");
                builder.Append(string.Concat(Designation_Name, "=", ParameterName(Designation_Name)));
                builder.Append(string.Concat(" Where ", Designation_ID, "=", ParameterName(Designation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Designation_ID), designationMasterEntity.Designation_ID, DbType.Int32);
                request.AddParamter(ParameterName(Designation_Name), designationMasterEntity.Designation_Name, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation ", designationMasterEntity.Designation_Name, " modified"));
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
                DesignationMasterEntity designationMasterEntity = entity as DesignationMasterEntity;
                if (designationMasterEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From Designation_Master ");
                builder.Append(string.Concat(" Where ", Designation_ID, "=", ParameterName(Designation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Designation_ID), designationMasterEntity.Designation_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation ", designationMasterEntity.Designation_Name, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        } 

        public IEntity GetDetailData(string divisionName)
        {
            DesignationMasterEntity designationMasterEntity = new DesignationMasterEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Designation_ID,Designation_Name from Designation_Master where ");
                builder.Append(string.Concat(Designation_Name, "=", ParameterName(Designation_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Designation_Name), divisionName, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    designationMasterEntity = (DesignationMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string divisionName)", ex);
            }
            return designationMasterEntity;
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
                builder.Append("Select Designation_ID,Designation_Name from Designation_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity GetDetailData(int id)
        {
            DesignationMasterEntity designationMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Designation_ID,Designation_Name from Designation_Master where ");
                builder.Append(string.Concat(Designation_ID, "=", ParameterName(Designation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Designation_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    designationMasterEntity = (DesignationMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Designation viewed."));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                designationMasterEntity = null;
            }
            return designationMasterEntity;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DesignationMasterEntity designationMasterEntity = new DesignationMasterEntity();
            if (NotNullAndNotDBNull(row, Designation_ID)) designationMasterEntity.Designation_ID = Convert.ToInt32(row[Designation_ID]);
            if (NotNullAndNotDBNull(row, Designation_Name)) designationMasterEntity.Designation_Name = Convert.ToString(row[Designation_Name]);
            return designationMasterEntity;
        }
 
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
	 
}
