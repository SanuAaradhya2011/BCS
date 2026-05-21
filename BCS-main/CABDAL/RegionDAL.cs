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
    public class RegionDAL : DALBase
    {
        private string Region_ID = "Region_ID";
        private string Region_Name = "Region_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RegionDAL).ToString());


        public override IEntity InsertData(IEntity entity)
        {
            RegionEntity regionEntity = null;
            if (entity == null)
                return regionEntity;
            regionEntity = entity as RegionEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into Region_Master(Region_Name) values (");
                builder.Append(string.Concat(ParameterName(Region_Name), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_Name), regionEntity.RegionName, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region ", regionEntity.RegionName, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                regionEntity = null;
            }
            return regionEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                RegionEntity regionEntity = entity as RegionEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update Region_Master Set ");
                builder.Append(string.Concat( Region_Name, "=", ParameterName(Region_Name ))); 
                builder.Append(string.Concat(" Where ", Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), regionEntity.RegionID, DbType.Int32);
                request.AddParamter(ParameterName(Region_Name), regionEntity.RegionName, DbType.String,50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region ", regionEntity.RegionName, " modified"));
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
                RegionEntity regionEntity = entity as RegionEntity;
                if (regionEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From Region_Master ");
                builder.Append(string.Concat(" Where ", Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), regionEntity.RegionID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region ", regionEntity.RegionName, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public IEntity GetDetailDataForCircle(int id)
        {
            RegionEntity regionEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.Region_ID,a.Region_Name from Region_Master a, circle_master b where a.Region_ID = b.Region_ID and ");
                builder.Append(string.Concat("a.", Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    regionEntity = (RegionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataForCircle(int id)", ex);
                regionEntity = null;
            }
            return regionEntity;
        }
        public override IEntity GetDetailData(int id)
        {
            RegionEntity regionEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Region_ID,Region_Name from Region_Master where ");
                builder.Append(string.Concat(Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    regionEntity = (RegionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                regionEntity = null;
            }
            return regionEntity;
        }
        // Added by Swati
        public  IEntity GetDetailDataByConsumerMeter(int id)
        {
            RegionEntity regionEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.Region_ID,a.Region_Name from Region_Master a,consumermeter b where a.Region_ID = b.Region_ID and  ");
                builder.Append(string.Concat("a.",Region_ID, "=", ParameterName(Region_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    regionEntity = (RegionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataByConsumerMeter(int id)", ex);
                regionEntity = null;
            }
            return regionEntity;
        }

        public IEntity GetDetailData(string regionName)
        {
            RegionEntity regionEntity = new RegionEntity();
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Region_ID,Region_Name from Region_Master where ");
                builder.Append(string.Concat(Region_Name, "=", ParameterName(Region_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_Name), regionName, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    regionEntity = (RegionEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string regionName)", ex);
            }
            return regionEntity;
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
                builder.Append("Select Region_ID,Region_Name from Region_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Region viewed"));
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
            RegionEntity regionEntity = new RegionEntity();
            if (NotNullAndNotDBNull(row, Region_ID)) regionEntity.RegionID = Convert.ToInt32(row[Region_ID]);
            if (NotNullAndNotDBNull(row, Region_Name)) regionEntity.RegionName = Convert.ToString(row[Region_Name]); 
            return regionEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        //20th April 2012
        public bool ValidateRegion(IEntity entity)
        {
            bool Flag = false;
            try
            {
                RegionEntity regionEntity = entity as RegionEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from Region_Master");
                builder.Append(string.Concat(" Where ", Region_Name, "=", ParameterName(Region_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Region_Name), regionEntity.RegionName.Trim() , DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateRegion(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        /// <summary>
        /// gets all existing data from region
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegionData()
        {
            DataSet ds = new DataSet();
            RegionEntity regionEntity = new RegionEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Region_ID,Region_Name from Region_Master ");
                DataRequest request = new DataRequest(builder.ToString());
                ds = helper.FillDataSet(request, ds);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetRegionData()", ex);
            }
            return ds;
        }

    }
}
