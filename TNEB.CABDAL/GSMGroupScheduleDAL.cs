/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 18/06/2010 												|
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
    public class GSMGroupScheduleDAL : DALBase
    {
        private string GSMGroupSchedule_ID = "GSMGroupSchedule_ID";
        private string Group_Name = "Group_Name";
        private string StartReadingDate = "StartReadingDate";
        private string GSMSchedule_ID = "GSMSchedule_ID";
        private string Meter_ID = "Meter_ID";
        public GSMGroupScheduleDAL()
            : base("GSMGroupSchedule", "GSMGroupSchedule_ID")
        {
        }
        public override IEntity InsertData(IEntity entity)
        {
            GSMGroupScheduleEntity gsmGroupScheduleEntity = null;
            if (entity == null)
                return gsmGroupScheduleEntity;
              gsmGroupScheduleEntity = entity as GSMGroupScheduleEntity; 
              bool Flag=false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into GSMGroupSchedule(Group_Name,StartReadingDate,GSMSchedule_ID,Meter_ID) values(");
                builder.Append(string.Concat(ParameterName(Group_Name), ","));
                builder.Append(string.Concat(ParameterName(StartReadingDate), ","));
                builder.Append(string.Concat(ParameterName(GSMSchedule_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_ID), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), gsmGroupScheduleEntity.Group_Name, DbType.String, 80);
                request.AddParamter(ParameterName(StartReadingDate), gsmGroupScheduleEntity.StartReadingDate, DbType.Int64);
                request.AddParamter(ParameterName(GSMSchedule_ID), gsmGroupScheduleEntity.GSMSchedule_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_ID), gsmGroupScheduleEntity.Meter_ID, DbType.String,5000);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule ", gsmGroupScheduleEntity.Group_Name, " added"));
            }
            catch (CABException)
            {
                Flag=false; 
            }
            if (Flag)
                gsmGroupScheduleEntity.GSMGroupSchedule_ID = long.Parse(this.GetPK());
            return gsmGroupScheduleEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                GSMGroupScheduleEntity gsmGroupScheduleEntity = entity as GSMGroupScheduleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update GSMGroupSchedule Set ");
                builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name),","));
                builder.Append(string.Concat(StartReadingDate, "=", ParameterName(StartReadingDate), ","));
                builder.Append(string.Concat(GSMSchedule_ID, "=", ParameterName(GSMSchedule_ID), ","));
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID))); 
                builder.Append(string.Concat(" Where ", GSMGroupSchedule_ID, "=", ParameterName(GSMGroupSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), gsmGroupScheduleEntity.Group_Name, DbType.String, 80);
                request.AddParamter(ParameterName(StartReadingDate), gsmGroupScheduleEntity.StartReadingDate, DbType.Int64);
                request.AddParamter(ParameterName(GSMSchedule_ID), gsmGroupScheduleEntity.GSMSchedule_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_ID), gsmGroupScheduleEntity.Meter_ID, DbType.String, 5000);
                request.AddParamter(ParameterName(GSMGroupSchedule_ID), gsmGroupScheduleEntity.GSMGroupSchedule_ID, DbType.Int64); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule ", gsmGroupScheduleEntity.Group_Name, " modified"));
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
            if (entity == null)
                return false;
            try
            {
                GSMGroupScheduleEntity gsmGroupScheduleEntity = entity as GSMGroupScheduleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From GSMGroupSchedule ");
                builder.Append(string.Concat(" Where ", GSMGroupSchedule_ID, "=", ParameterName(GSMGroupSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(GSMGroupSchedule_ID), gsmGroupScheduleEntity.GSMGroupSchedule_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule ", gsmGroupScheduleEntity.Group_Name, " deleted"));
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
            GSMGroupScheduleEntity gsmGroupScheduleEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select GSMGroupSchedule_ID,Group_Name,StartReadingDate,GSMSchedule_ID,Meter_ID from GSMGroupSchedule where ");
                builder.Append(string.Concat(GSMGroupSchedule_ID, "=", ParameterName(GSMGroupSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(GSMGroupSchedule_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    gsmGroupScheduleEntity = (GSMGroupScheduleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule detail data viewed"));
    
            }
            catch (CABException)
            {
                gsmGroupScheduleEntity = null;
            }
            return gsmGroupScheduleEntity;
        }

        public bool GetDetailData(string groupName)
        {
            bool Flag =false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select GSMGroupSchedule_ID,Group_Name,StartReadingDate,GSMSchedule_ID,Meter_ID from GSMGroupSchedule where ");
                builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), groupName, DbType.String, 80);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    Flag= true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule " + groupName + " data viewed"));
            }
            catch (CABException)
            { 
            }
            return Flag;
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
                builder.Append("Select A.GSMGroupSchedule_ID,A.Group_Name as 'Group Name',A.StartReadingDate AS 'Reading Start Date',B.Schedule_Name AS 'Schedule Name',A.Meter_ID as 'Meter Number' from GSMGroupSchedule A,gsmschedule B where A.GSMSchedule_ID=B.gsmSchedule_ID order by A.Group_Name");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group Schedule all data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListDataSet(string columnName, string value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.GSMGroupSchedule_ID,A.Group_Name as 'Group Name',A.StartReadingDate AS 'Reading Start Date',B.Schedule_Name AS 'Schedule Name',A.Meter_ID as 'Meter Number' from GSMGroupSchedule A,gsmschedule B where A.GSMSchedule_ID=B.gsmSchedule_ID and A.");
                builder.Append(string.Concat(columnName, " like '%", value, "%'"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListDataSet(long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.GSMGroupSchedule_ID,A.Group_Name as 'Group Name',A.StartReadingDate AS 'Reading Start Date',B.Schedule_Name AS 'Schedule Name',A.Meter_ID as 'Meter Number' from GSMGroupSchedule A,gsmschedule B where A.GSMSchedule_ID=B.gsmSchedule_ID and A.");
                builder.Append(string.Concat(StartReadingDate, ">", fromDate, " and ", StartReadingDate, "<=", toDate));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListDataSet(string columnName, int value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.GSMGroupSchedule_ID,A.Group_Name as 'Group Name',A.StartReadingDate AS 'Reading Start Date',B.Schedule_Name AS 'Schedule Name',A.Meter_ID as 'Meter Number' from GSMGroupSchedule A,gsmschedule B where A.GSMSchedule_ID=B.gsmSchedule_ID and A.");
                builder.Append(string.Concat(columnName, "=", value));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }
        public bool ValidateGroup(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return false;
            try
            {
                GSMGroupScheduleEntity gsmGroupScheduleEntity = entity as GSMGroupScheduleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from GSMGroupSchedule where ");
                builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name)));
                if(gsmGroupScheduleEntity.GSMGroupSchedule_ID!=0)
                    builder.Append(string.Concat(GSMGroupSchedule_ID, "<>", ParameterName(GSMGroupSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), gsmGroupScheduleEntity.Group_Name, DbType.String, 80);
                if (gsmGroupScheduleEntity.GSMGroupSchedule_ID != 0)
                    request.AddParamter(ParameterName(GSMGroupSchedule_ID), gsmGroupScheduleEntity.GSMGroupSchedule_ID, DbType.Int64);
                if (Convert.ToInt32(helper.ExecuteScalar(request)) > 0)
                    Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data validated"));
            }
            catch (CABException)
            {
                Flag = false;
            }
            return Flag;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GSMGroupScheduleEntity gsmGroupScheduleEntity = new GSMGroupScheduleEntity();
            if (NotNullAndNotDBNull(row, GSMGroupSchedule_ID)) gsmGroupScheduleEntity.GSMGroupSchedule_ID = Convert.ToInt64(row[GSMGroupSchedule_ID]);
            if (NotNullAndNotDBNull(row, Group_Name)) gsmGroupScheduleEntity.Group_Name = Convert.ToString(row[Group_Name]);
            if (NotNullAndNotDBNull(row, StartReadingDate)) gsmGroupScheduleEntity.StartReadingDate = Convert.ToInt64(row[StartReadingDate]);
            if (NotNullAndNotDBNull(row, GSMSchedule_ID)) gsmGroupScheduleEntity.GSMSchedule_ID = Convert.ToInt64(row[GSMSchedule_ID]);
            if (NotNullAndNotDBNull(row, Meter_ID)) gsmGroupScheduleEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
            return gsmGroupScheduleEntity;  
        }
  
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}

