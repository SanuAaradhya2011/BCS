/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to  Cabcon							|
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
    public class CurrentTimeStampDAL : DALBase
    { 
        private string CurrentTimeStamp_ID = "CurrentTimeStamp_ID";
        private string CurrentMD1 = "CurrentMD1";
        private string CurrentMD1TimeStamp = "CurrentMD1TimeStamp";
        private string CurrentMD2 = "CurrentMD2";
        private string CurrentMD2TimeStamp = "CurrentMD2TimeStamp";
        private string CurrentMD3 = "CurrentMD3";
        private string CurrentMD3TimeStamp = "CurrentMD3TimeStamp";

        public CurrentTimeStampDAL() : base("MeterData_CurrentTimeStamp", "CurrentTimeStamp_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CurrentTimeStampEntity currentTimeStampEntity = entity as CurrentTimeStampEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_CurrentTimeStamp(CurrentMD1,CurrentMD1TimeStamp,CurrentMD2,CurrentMD2TimeStamp,CurrentMD3,CurrentMD3TimeStamp) values(");
                builder.Append(string.Concat(ParameterName(CurrentMD1), ","));
                builder.Append(string.Concat(ParameterName(CurrentMD1TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(CurrentMD2), ","));
                builder.Append(string.Concat(ParameterName(CurrentMD2TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(CurrentMD3), ","));
                builder.Append(string.Concat(ParameterName(CurrentMD3TimeStamp), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentMD1), currentTimeStampEntity.CurrentMD1, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMD1TimeStamp), currentTimeStampEntity.CurrentMD1TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMD2), currentTimeStampEntity.CurrentMD2, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMD2TimeStamp), currentTimeStampEntity.CurrentMD2TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMD3), currentTimeStampEntity.CurrentMD3, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentMD3TimeStamp), currentTimeStampEntity.CurrentMD3TimeStamp, DbType.String, 40);
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Time Stamp Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                currentTimeStampEntity.CurrentTimeStamp_ID = long.Parse(this.GetPK());
            return currentTimeStampEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CurrentTimeStampEntity currentTimeStampEntity = entity as CurrentTimeStampEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_CurrentTimeStamp where ");
                builder.Append(string.Concat(CurrentTimeStamp_ID, "=", ParameterName(CurrentTimeStamp_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentTimeStamp_ID), currentTimeStampEntity.CurrentTimeStamp_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Time Stamp Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CurrentTimeStampEntity currentTimeStampEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CurrentTimeStamp_ID,CurrentMD1,CurrentMD1TimeStamp,CurrentMD2,CurrentMD2TimeStamp,CurrentMD3,CurrentMD3TimeStamp from MeterData_CurrentTimeStamp where ");
                builder.Append(string.Concat(CurrentTimeStamp_ID, "=", ParameterName(CurrentTimeStamp_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentTimeStamp_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    currentTimeStampEntity = (CurrentTimeStampEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Time Stamp viewed."));

            }
            catch (CABException)
            { 
            }
            return currentTimeStampEntity;
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
                builder.Append("Select CurrentTimeStamp_ID,CurrentMD1,CurrentMD1TimeStamp,CurrentMD2,CurrentMD2TimeStamp,CurrentMD3,CurrentMD3TimeStamp from MeterData_CurrentTimeStamp");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Time Stamp viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            CurrentTimeStampEntity currentTimeStampEntity = new CurrentTimeStampEntity();
            if (NotNullAndNotDBNull(row, CurrentTimeStamp_ID)) currentTimeStampEntity.CurrentTimeStamp_ID = Convert.ToInt32(row[CurrentTimeStamp_ID]);
            if (NotNullAndNotDBNull(row, CurrentMD1)) currentTimeStampEntity.CurrentMD1 = Convert.ToString(row[CurrentMD1]);
            if (NotNullAndNotDBNull(row, CurrentMD1TimeStamp)) currentTimeStampEntity.CurrentMD1TimeStamp = Convert.ToString(row[CurrentMD1TimeStamp]);
            if (NotNullAndNotDBNull(row, CurrentMD2)) currentTimeStampEntity.CurrentMD2 = Convert.ToString(row[CurrentMD2]);
            if (NotNullAndNotDBNull(row, CurrentMD2TimeStamp)) currentTimeStampEntity.CurrentMD2TimeStamp = Convert.ToString(row[CurrentMD2TimeStamp]);
            if (NotNullAndNotDBNull(row, CurrentMD3)) currentTimeStampEntity.CurrentMD3 = Convert.ToString(row[CurrentMD3]);
            if (NotNullAndNotDBNull(row, CurrentMD3TimeStamp)) currentTimeStampEntity.CurrentMD3TimeStamp = Convert.ToString(row[CurrentMD3TimeStamp]);
            return currentTimeStampEntity;
        } 
    }
}
