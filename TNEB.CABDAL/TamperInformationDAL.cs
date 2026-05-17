/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon								|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
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
    public class TamperInformationDAL : DALBase
    {  
         private string TamperInformation_ID = "TamperInformation_ID";
        private string LatestTamperOccurrenceID = "LatestTamperOccurrenceID";
        private string OccurrenceTime = "OccurrenceTime";
        private string LatestTamperRestorationID = "LatestTamperRestorationID";
           private string RestorationTime = "RestorationTime";
        public TamperInformationDAL()
            : base("MeterData_TamperInformation", "TamperInformation_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            TamperInformationEntity tamperInformationEntity = entity as TamperInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_TamperInformation(LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,RestorationTime) values(");
                builder.Append(string.Concat(ParameterName(LatestTamperOccurrenceID), ","));
                builder.Append(string.Concat(ParameterName(OccurrenceTime), ",")); 
                builder.Append(string.Concat(ParameterName(LatestTamperRestorationID), ","));
                builder.Append(string.Concat(ParameterName(RestorationTime), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(LatestTamperOccurrenceID), tamperInformationEntity.LatestTamperOccurrenceID, DbType.String, 40);
                request.AddParamter(ParameterName(OccurrenceTime), tamperInformationEntity.OccurrenceTime, DbType.Int64);
                request.AddParamter(ParameterName(LatestTamperRestorationID), tamperInformationEntity.LatestTamperRestorationID, DbType.String, 40);
                request.AddParamter(ParameterName(RestorationTime), tamperInformationEntity.RestorationTime, DbType.Int64); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Tamper Information Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tamperInformationEntity.TamperInformation_ID = long.Parse(this.GetPK());
            return tamperInformationEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            TamperInformationEntity tamperInformationEntity = entity as TamperInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TamperInformation where ");
                builder.Append(string.Concat(TamperInformation_ID, "=", ParameterName(TamperInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperInformation_ID), tamperInformationEntity.TamperInformation_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Tamper Information Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TamperInformationEntity tamperInformationEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperInformation_ID,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,RestorationTime from MeterData_TamperInformation where ");
                builder.Append(string.Concat(TamperInformation_ID, "=", ParameterName(TamperInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperInformation_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tamperInformationEntity = (TamperInformationEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Tamper Information viewed."));

            }
            catch (CABException)
            { 
            }
            return tamperInformationEntity;
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
                builder.Append("Select TamperInformation_ID,LatestTamperOccurrenceID,OccurrenceTime,LatestTamperRestorationID,RestorationTime from MeterData_TamperInformation ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Tamper Information viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            TamperInformationEntity tamperInformationEntity = new TamperInformationEntity();
            if (NotNullAndNotDBNull(row, TamperInformation_ID)) tamperInformationEntity.TamperInformation_ID = Convert.ToInt64(row[TamperInformation_ID]);
            if (NotNullAndNotDBNull(row, LatestTamperOccurrenceID)) tamperInformationEntity.LatestTamperOccurrenceID = Convert.ToString(row[LatestTamperOccurrenceID]);
            if (NotNullAndNotDBNull(row, OccurrenceTime)) tamperInformationEntity.OccurrenceTime = Convert.ToInt64(row[OccurrenceTime]);
            if (NotNullAndNotDBNull(row, LatestTamperRestorationID)) tamperInformationEntity.LatestTamperRestorationID = Convert.ToString(row[LatestTamperRestorationID]);
            if (NotNullAndNotDBNull(row, RestorationTime)) tamperInformationEntity.RestorationTime = Convert.ToInt64(row[RestorationTime]);
            return tamperInformationEntity;
        } 
    }
}
