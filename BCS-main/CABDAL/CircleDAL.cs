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
    public class CircleDAL : DALBase
    {
        private string Circle_ID = "Circle_ID";
        private string Circle_Name = "Circle_Name";
        private string Region_ID = "Region_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CircleDAL).ToString());


        public override IEntity InsertData(IEntity entity)
        {
            CircleEntity circleEntity = null;
            if (entity == null)
                return circleEntity;
              circleEntity = entity as CircleEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into Circle_Master(Circle_Name,Region_ID) values(");
                builder.Append(string.Concat(ParameterName(Circle_Name), ","));
                builder.Append(string.Concat(ParameterName(Region_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_Name), circleEntity.CircleName, DbType.String, 50);
                request.AddParamter(ParameterName(Region_ID), circleEntity.RegionID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Circle ", circleEntity.CircleName, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                circleEntity = null;
            }
            return circleEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                CircleEntity circleEntity = entity as CircleEntity;
                CircleEntity objDBCircleEntity = GetDetailData(circleEntity.CircleID) as CircleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update Circle_Master Set ");
                builder.Append(string.Concat( Circle_Name, "=", ParameterName(Circle_Name ),","));
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)));
                builder.Append(string.Concat(" Where ", Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), circleEntity.CircleID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_Name), circleEntity.CircleName, DbType.String, 50);
                request.AddParamter(ParameterName(Region_ID), circleEntity.RegionID, DbType.Int64);
                if((helper.ExecuteNonQuery(request) > 0) &&
                    objDBCircleEntity.RegionID != circleEntity.RegionID)
                {
                    builder.Remove(0, builder.Length);
                    builder.Append("Update division_master set ");
                    builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)));
                    builder.Append(string.Concat(" where ", Circle_ID, "=", ParameterName(Circle_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Region_ID), circleEntity.RegionID, DbType.Int64);
                    request.AddParamter(ParameterName(Circle_ID), circleEntity.CircleID, DbType.Int32);
                    helper.ExecuteNonQuery(request);

                    builder.Remove(0, builder.Length);
                    builder.Append("Update consumermeter set ");
                    builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)));
                    builder.Append(string.Concat(" where ", Circle_ID, "=", ParameterName(Circle_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Region_ID), circleEntity.RegionID, DbType.Int64);
                    request.AddParamter(ParameterName(Circle_ID), circleEntity.CircleID, DbType.Int64);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Circle ", circleEntity.CircleName, " modified"));
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
                CircleEntity circleEntity = entity as CircleEntity;
                if (circleEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From Circle_Master ");
                builder.Append(string.Concat(" Where ", Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), circleEntity.CircleID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Circle ", circleEntity.CircleName, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        // Added by Swati. To check if division is existing.
        public IEntity GetDetailDataByDivision(int id)
        {
            CircleEntity circleEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.Circle_ID,a.Circle_Name,a.Region_ID from Circle_Master a, division_master b where a.Circle_ID = b.Circle_ID and ");
                builder.Append("a.Region_ID = b.Region_ID and ");
                builder.Append(string.Concat("a.",Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    circleEntity = (CircleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified circle viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataByDivision(int id)", ex);
                circleEntity = null;
            }
            return circleEntity;
        }
        public override IEntity GetDetailData(int id)
        {
            CircleEntity circleEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Circle_ID,Circle_Name,Region_ID from Circle_Master where ");
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    circleEntity = (CircleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified circle viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                circleEntity = null;
            }
            return circleEntity;
        }
        // Added by Swati
        public IEntity GetDetailDataConsumer(int id)
        {
            CircleEntity circleEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.Circle_ID,a.Circle_Name,a.Region_ID from Circle_Master a,consumermeter b where a.Circle_ID=b.Circle_ID and  ");
                builder.Append(string.Concat("a.",Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    circleEntity = (CircleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified circle viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataConsumer(int id)", ex);
                circleEntity = null;
            }
            return circleEntity;
        }
        public DataSet GetCircleDetailData(int regionID)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Circle_ID,Circle_Name,Region_ID from Circle_Master where ");
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), regionID, DbType.Int64);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for circle dropdown retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCircleDetailData(int regionID)", ex);
                ds = null;
            }
            return ds;
        }
        public IEntity GetDetailData(string CircleName)
        {
            CircleEntity circleEntity = new CircleEntity(); 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Circle_ID,Circle_Name,Region_ID from Circle_Master where ");
                builder.Append(string.Concat(Circle_Name, "=", ParameterName(Circle_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_Name), CircleName, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    circleEntity = (CircleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified circle viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string CircleName)", ex);
            }
            return circleEntity;
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
                builder.Append("Select c.Circle_ID ,c.Circle_Name,r.Region_Name from Circle_Master c join Region_Master r on c.Region_ID = r.Region_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a all the circles viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        // added by swati
        public DataSet ListDataSetCircle(int regionid)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select c.Circle_ID ,c.Circle_Name,r.Region_Name from Circle_Master c join Region_Master r on c.Region_ID = r.Region_ID and ");
                builder.Append(string.Concat("c.",Region_ID, "=",ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), regionid, DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a all the circles viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetCircle(int regionid)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            CircleEntity circleEntity = new CircleEntity();
            if (NotNullAndNotDBNull(row, Circle_ID)) circleEntity.CircleID = Convert.ToInt32(row[Circle_ID]);
            if (NotNullAndNotDBNull(row, Circle_Name)) circleEntity.CircleName = Convert.ToString(row[Circle_Name]);
            if (NotNullAndNotDBNull(row, Region_ID)) circleEntity.RegionID= Convert.ToInt32(row[Region_ID]);
            return circleEntity;
        }
 
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        //20th April 2012
        public bool ValidateCircle(IEntity entity)
        {
            bool Flag = false;
            try
            {
                CircleEntity circleEntity = entity  as CircleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from Circle_Master c, Region_Master r");
                // Added to solve bug 83735.
                builder.Append(string.Concat(" Where ", Circle_Name, "=", ParameterName(Circle_Name)));
                builder.Append(" and c.Region_ID = r.Region_ID");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_Name), circleEntity.CircleName.Trim(), DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateCircle(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        /// <summary>
        /// get all data of circle
        /// </summary>
        /// <returns></returns>
        public DataSet GetCircleData()
        {
            DataSet ds = new DataSet();
            CircleEntity circleEntity = new CircleEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Circle_ID,Circle_Name,Region_ID from Circle_Master ");
                DataRequest request = new DataRequest(builder.ToString());
                ds = helper.FillDataSet(request, ds);
               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCircleData()", ex);
            }
            return ds;
        }
    }
}


