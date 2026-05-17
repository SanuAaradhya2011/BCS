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
    public class DivisionDAL : DALBase
    {
        private string Division_ID = "Division_ID";
        private string Division_Name = "Division_Name";
        private string Region_ID = "Region_ID";
        private string Circle_ID = "Circle_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DivisionDAL).ToString());
        public override IEntity InsertData(IEntity entity)
        {
            DivisionEntity divisionEntity = null;
            if (entity == null)
                return divisionEntity;
            divisionEntity = entity as DivisionEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into Division_Master(Division_Name,Region_ID,Circle_ID) values(");
                builder.Append(string.Concat(ParameterName(Division_Name), ","));
                builder.Append(string.Concat(ParameterName(Region_ID), ","));
                builder.Append(string.Concat(ParameterName(Circle_ID), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_Name), divisionEntity.DivisionName, DbType.String, 50);
                request.AddParamter(ParameterName(Region_ID), divisionEntity.RegionID, DbType.Int64);
                request.AddParamter(ParameterName(Circle_ID), divisionEntity.CircleID, DbType.Int64); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", divisionEntity.DivisionName, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                divisionEntity = null;
            }
            return divisionEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                DivisionEntity divisionEntity = entity as DivisionEntity;
                DivisionEntity objDBDivisionEntity = GetDetailData(divisionEntity.DivisionID) as DivisionEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update Division_Master Set ");
                builder.Append(string.Concat( Division_Name, "=", ParameterName(Division_Name ),","));
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID), ","));
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID)));
                builder.Append(string.Concat(" Where ", Division_ID, "=", ParameterName(Division_ID)));
                DataRequest request = new DataRequest(builder.ToString());

                request.AddParamter(ParameterName(Division_Name), divisionEntity.DivisionName, DbType.String,50);
                request.AddParamter(ParameterName(Region_ID), divisionEntity.RegionID, DbType.Int64);
                request.AddParamter(ParameterName(Circle_ID), divisionEntity.CircleID, DbType.Int64);
                request.AddParamter(ParameterName(Division_ID), divisionEntity.DivisionID, DbType.Int32);
                if ((helper.ExecuteNonQuery(request) > 0) &&
                    (objDBDivisionEntity.CircleID != divisionEntity.CircleID || 
                    objDBDivisionEntity.RegionID != divisionEntity.RegionID))
                {
                    builder.Remove(0, builder.Length);
                    builder.Append("Update consumermeter set ");
                    builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID), ","));
                    builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID)));
                    builder.Append(string.Concat(" where ", Division_ID, "=", ParameterName(Division_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Region_ID), divisionEntity.RegionID, DbType.Int64);
                    request.AddParamter(ParameterName(Circle_ID), divisionEntity.CircleID, DbType.Int64);
                    request.AddParamter(ParameterName(Division_ID), divisionEntity.DivisionID, DbType.Int64);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", divisionEntity.DivisionName, " modified"));
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
                DivisionEntity divisionEntity = entity as DivisionEntity;
                if (divisionEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From Division_Master ");
                builder.Append(string.Concat(" Where ", Division_ID, "=", ParameterName(Division_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_ID), divisionEntity.DivisionID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division ", divisionEntity.DivisionName, " deleted"));
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
            DivisionEntity divisionEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Division_ID,Division_Name,Region_ID,Circle_ID from Division_Master where ");
                builder.Append(string.Concat(Division_ID, "=", ParameterName(Division_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    divisionEntity = (DivisionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                divisionEntity = null;
            }
            return divisionEntity;
        }
        //Added by Swati
        public IEntity GetDetailDataConsumer(int id)
        {
            DivisionEntity divisionEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.Division_ID,a.Division_Name,a.Region_ID,a.Circle_ID from Division_Master a,consumermeter b where a. Division_ID = b.Division_ID and ");
                builder.Append(string.Concat("a.",Division_ID, "=", ParameterName(Division_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    divisionEntity = (DivisionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataConsumer(int id)", ex);
                divisionEntity = null;
            }
            return divisionEntity;
        }
        public IEntity GetDetailData(string divisionName)
        {
            DivisionEntity divisionEntity = new DivisionEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Division_ID,Division_Name,Region_ID,Circle_ID from Division_Master where ");
                builder.Append(string.Concat(Division_Name, "=", ParameterName(Division_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_Name), divisionName, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    divisionEntity = (DivisionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string divisionName)", ex);
            }
            return divisionEntity;
        }
        public DataSet GetDivisionDataByCircleID(int circleID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Division_ID,Division_Name,Region_ID,Circle_ID from Division_Master where ");
                builder.Append(string.Concat(Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Circle_ID), circleID, DbType.Int64);
                dataSet= new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDivisionDataByCircleID(int circleID)", ex);
                dataSet = null;
            }
            return dataSet;
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
                builder.Append("Select d.Division_ID,d.Division_Name,r.Region_Name,c.Circle_Name from Division_Master d join region_master r on d.Region_ID = r.Region_ID join circle_master c on c.Circle_ID = d.Circle_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        // Added by Swati
        public DataSet ListDataSetDivision(int regionid, int circleid)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select d.Division_ID,d.Division_Name,r.Region_Name,c.Circle_Name from Division_Master d join region_master r on d.Region_ID = r.Region_ID join circle_master c on c.Circle_ID = d.Circle_ID and ");
                builder.Append(string.Concat("d.",Region_ID ,"=",ParameterName(Region_ID)));
                builder.Append(string.Concat(" and d.", Circle_ID, "=", ParameterName(Circle_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID),regionid,DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), circleid, DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSetDivision(int regionid, int circleid)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DivisionEntity divisionEntity = new DivisionEntity();
            if (NotNullAndNotDBNull(row, Division_ID)) divisionEntity.DivisionID = Convert.ToInt32(row[Division_ID]);
            if (NotNullAndNotDBNull(row, Division_Name)) divisionEntity.DivisionName = Convert.ToString(row[Division_Name]);
            if (NotNullAndNotDBNull(row, Region_ID)) divisionEntity.RegionID = Convert.ToInt32(row[Region_ID]);
            if (NotNullAndNotDBNull(row, Circle_ID)) divisionEntity.CircleID= Convert.ToInt32(row[Circle_ID]); 
            return divisionEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        //20th April 2012
        public bool ValidateDivision(IEntity entity)
        {
            bool Flag = false;
            try
            {
                DivisionEntity divisionEntity = entity  as DivisionEntity; 
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // Added to solve bug 83735.
                builder.Append("Select count(*) from Division_Master d, Region_Master r");
                builder.Append(string.Concat(" Where ", Division_Name, "=", ParameterName(Division_Name)));
                builder.Append(" and d.Region_ID = r.Region_ID");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Division_Name), divisionEntity.DivisionName.Trim(), DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateDivision(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        /// <summary>
/// get all division data
/// </summary>
/// <returns></returns>
        public DataSet GetDivisionData()
        {
            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Division_ID,Division_Name,Region_ID,Circle_ID from Division_Master ");
                DataRequest request = new DataRequest(builder.ToString());
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Division viewed."));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDivisionData()", ex);
            }
            return ds;
        }
    }
}

