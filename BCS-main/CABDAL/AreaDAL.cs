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
    public class AreaDAL : DALBase
    {
        private string Area_ID = "Area_ID";
        private string Region_ID = "Region_ID";
        private string Circle_ID = "Circle_ID";
        private string Divsion_ID = "Divsion_ID";
        private string CMRI_ID = "CMRI_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AreaDAL).ToString());

        public AreaDAL()
            : base("AreaMaster", "Area_ID")
        {
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public IEntity InsertData(IEntity entity,bool flag)
        {
            if (entity == null)
                return entity;
            AreaEntity areaEntity = entity as AreaEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into AreaMaster(Region_ID,Circle_ID,Divsion_ID,CMRI_ID) values(");
                builder.Append(string.Concat(ParameterName(Region_ID), ","));
                builder.Append(string.Concat(ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(ParameterName(Divsion_ID), ","));
                builder.Append(string.Concat(ParameterName(CMRI_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), areaEntity.Region_ID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), areaEntity.Circle_ID, DbType.Int32);
                request.AddParamter(ParameterName(Divsion_ID), areaEntity.Divsion_ID, DbType.Int32);
                request.AddParamter(ParameterName(CMRI_ID), areaEntity.CMRI_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New Area Added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity,bool flag)", ex);
                Flag = false;
            }
            if (Flag)
                areaEntity.Area_ID = Convert.ToInt64(this.GetPK());
            return areaEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                AreaEntity areaEntity = entity as AreaEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update AreaMaster Set "); 
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID),","));
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(Divsion_ID, "=", ParameterName(Divsion_ID), ","));
                builder.Append(string.Concat(CMRI_ID, "=", ParameterName(CMRI_ID)));
                builder.Append(string.Concat(" Where ", Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), areaEntity.Area_ID, DbType.Int32);
                request.AddParamter(ParameterName(Region_ID), areaEntity.Region_ID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), areaEntity.Circle_ID, DbType.Int32);
                request.AddParamter(ParameterName(Divsion_ID), areaEntity.Divsion_ID, DbType.Int32);
                request.AddParamter(ParameterName(CMRI_ID), areaEntity.CMRI_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected Area Modified"));
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
            throw new NotImplementedException();
        }
        //public override bool DeleteData(IEntity entity)
        //{
        //    bool Flag = false;
        //    try
        //    {
        //        AreaEntity areaEntity = entity as AreaEntity;
        //        if (areaEntity == null)
        //            return false;
        //        IDataHelper helper = DatabaseFactory.GetHelper();
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Delete From AreaMaster ");
        //        builder.Append(string.Concat(" Where ", Area_ID, "=", ParameterName(Area_ID)));
        //        DataRequest request = new DataRequest(builder.ToString());
        //        request.AddParamter(ParameterName(Area_ID), areaEntity.Area_ID, DbType.Int64);
        //        helper.ExecuteNonQuery(request);
        //        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area Deleted"));
        //        Flag = true;
        //    }
        //    catch (CABException)
        //    {
        //        Flag = false;
        //    }
        //    return Flag;
        //}

        public bool DeleteData(long id)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From AreaMaster ");
                builder.Append(string.Concat(" Where ", Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), id, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected Area Deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long id)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            AreaEntity areaEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Area_ID,Region_ID,Circle_ID,Divsion_ID,CMRI_ID from AreaMaster where ");
                builder.Append(string.Concat(Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    areaEntity = (AreaEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area Description Viewed Based on The Selected Area ID"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                areaEntity = null;
            }
            return areaEntity;
        }

        public IEntity GetDataforCMRI(int cmriID)
        {
            AreaEntity areaEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from areamaster where ");
                builder.Append(string.Concat(CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), cmriID, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    areaEntity = (AreaEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area Description Viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataforCMRI(int cmriID)", ex);
                areaEntity = null;
            }
            return areaEntity;
        }

        public IEntity GetDetailData(int divisionId, int circleId, int regionId)
        {
            AreaEntity areaEntity = new AreaEntity();
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Area_ID,Region_ID,Circle_ID,Divsion_ID,CMRI_ID from AreaMaster where ");
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)," and "));
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID), " and "));
                builder.Append(string.Concat(Divsion_ID, "=", ParameterName(Divsion_ID) ));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), regionId, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), circleId, DbType.Int32);
                request.AddParamter(ParameterName(Divsion_ID), divisionId, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    areaEntity = (AreaEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area description viewed based on the selected region, circle and division ID"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int divisionId, int circleId, int regionId)", ex);
            }
            return areaEntity;
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
                builder.Append("Select Area_ID,Region_ID,Circle_ID,Divsion_ID,CMRI_ID from AreaMaster");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area description viewed"));
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
            AreaEntity areaEntity = new AreaEntity();
            if (NotNullAndNotDBNull(row, Area_ID)) areaEntity.Area_ID = Convert.ToInt32(row[Area_ID]);
            if (NotNullAndNotDBNull(row, Region_ID)) areaEntity.Region_ID = Convert.ToInt32(row[Region_ID]);
            if (NotNullAndNotDBNull(row, Circle_ID)) areaEntity.Circle_ID = Convert.ToInt32(row[Circle_ID]);
            if (NotNullAndNotDBNull(row, Divsion_ID)) areaEntity.Divsion_ID = Convert.ToInt32(row[Divsion_ID]);
            if (NotNullAndNotDBNull(row, CMRI_ID)) areaEntity.CMRI_ID = Convert.ToInt32(row[CMRI_ID]); 
            return areaEntity;
        }

     

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DataSet ListDetails()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select a.Area_ID, r.Region_Name as 'Region Name', c.Circle_Name as 'Circle Name', d.Division_Name as 'Division Name', "
                + "no.CMRI_Number as 'CMRI Number' from  region_master r inner join areamaster a on a.Region_ID = r.Region_ID inner join circle_master c "
                + "on c.Circle_ID = a.Circle_ID inner join division_master d on a.Divsion_ID = d.Division_ID inner join cmri_Master no on "
                + "no.CMRI_ID = a.CMRI_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area definition viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDetails()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public bool ValidateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                AreaEntity areaEntity = entity as AreaEntity;
                if (areaEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select count(*) from  AreaMaster");
                builder.Append(string.Concat(" Where ", Region_ID, "=", areaEntity.Region_ID, " and "));
                builder.Append(string.Concat(Circle_ID, "=", areaEntity.Circle_ID, " and "));
                builder.Append(string.Concat(Divsion_ID, "=", areaEntity.Divsion_ID ));

                DataRequest request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt32(data)>0)
                    Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified region viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        /// <summary>
        /// gets all the data in area table
        /// </summary>
        /// <returns></returns>
        public DataSet GetAreaData()
        {
            DataSet ds = new DataSet();
            AreaEntity areaEntity = new AreaEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Area_ID,Region_ID,Circle_ID,Divsion_ID,CMRI_ID from AreaMaster ");
                DataRequest request = new DataRequest(builder.ToString());
                ds = helper.FillDataSet(request, ds);
                }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAreaData()", ex);
            }
            return ds;
        }
    }
}
