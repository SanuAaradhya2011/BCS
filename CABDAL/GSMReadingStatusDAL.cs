using System;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GSMReadingStatusDAL : DALBase
    {
        private string GSMReadingStatus_ID = "GSMReadingStatus_ID";
        private string ReadingDateTime = "ReadingDateTime";
        private string StatusMessage = "StatusMessage";
        private string FileName = "FileName";
        private string FilePath = "FilePath";
        private string Status = "Status";
        private string GSMSchedule_ID = "GSMSchedule_ID";
        private string GSMGroupSchedule_ID = "GSMGroupSchedule_ID";
        private string Meter_ID = "Meter_ID";
        private string SchedulePeriod = "SchedulePeriod";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMReadingStatusDAL).ToString());
        public override IEntity InsertData(IEntity entity)
        {
            GSMReadingStatusEntity gSMReadingStatusEntity = null;
            if (entity == null)
                return gSMReadingStatusEntity;
            gSMReadingStatusEntity = entity as GSMReadingStatusEntity;
            bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsmreadingstatus(ReadingDateTime,StatusMessage,FileName,FilePath,GSMSchedule_ID,GSMGroupSchedule_ID,SchedulePeriod,Meter_ID,Status) values(");
                builder.Append(string.Concat(ParameterName(ReadingDateTime), ","));
				builder.Append(string.Concat(ParameterName(StatusMessage), ","));
				builder.Append(string.Concat(ParameterName(FileName), ","));
				builder.Append(string.Concat(ParameterName(FilePath), ","));
                builder.Append(string.Concat(ParameterName(GSMSchedule_ID), ","));
                builder.Append(string.Concat(ParameterName(GSMGroupSchedule_ID), ","));
                builder.Append(string.Concat(ParameterName(SchedulePeriod), ","));
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
				builder.Append(string.Concat(ParameterName(Status),")"));
				DataRequest request = new DataRequest(builder.ToString()); 
				request.AddParamter(ParameterName(ReadingDateTime), gSMReadingStatusEntity.ReadingDateTime, DbType.Int64);
				request.AddParamter(ParameterName(StatusMessage), gSMReadingStatusEntity.StatusMessage, DbType.String, 5000);
				request.AddParamter(ParameterName(FileName), gSMReadingStatusEntity.FileName, DbType.String, 150);
				request.AddParamter(ParameterName(FilePath), gSMReadingStatusEntity.FilePath, DbType.String, 500);
                request.AddParamter(ParameterName(GSMSchedule_ID), gSMReadingStatusEntity.GSMScheduleID, DbType.Int64);
                request.AddParamter(ParameterName(GSMGroupSchedule_ID), gSMReadingStatusEntity.GSMGroupScheduleID, DbType.Int64);
                request.AddParamter(ParameterName(SchedulePeriod), gSMReadingStatusEntity.SchedulePeriod, DbType.String, 5);
                request.AddParamter(ParameterName(Meter_ID), gSMReadingStatusEntity.MeterID, DbType.String, 50);
				request.AddParamter(ParameterName(Status), gSMReadingStatusEntity.Status, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status added"));
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
				Flag = false;
			} 
            return gSMReadingStatusEntity;
        }

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return false;
            try
            {
                GSMReadingStatusEntity gSMReadingStatusEntity = entity as GSMReadingStatusEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From gsmreadingstatus ");
                builder.Append(string.Concat(" Where ", GSMReadingStatus_ID, "=", ParameterName(GSMReadingStatus_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(GSMReadingStatus_ID), gSMReadingStatusEntity.GSMReadingStatus_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status deleted"));
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
            GSMReadingStatusEntity gSMReadingStatusEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select GSMReadingStatus_ID as 'ID',ReadingDateTime as 'Time Stamp',StatusMessage as 'Status Message'," +
                    "FileName as 'File Name',FilePath as 'File Path',Status from gsmreadingstatus where ");
                builder.Append(string.Concat(GSMReadingStatus_ID, "=", ParameterName(GSMReadingStatus_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(GSMReadingStatus_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    gSMReadingStatusEntity = (GSMReadingStatusEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                gSMReadingStatusEntity = null;
            }
            return gSMReadingStatusEntity;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.GSMReadingStatus_ID,A.Meter_ID as 'Meter ID',A.SchedulePeriod as 'Schedule Period', A.ReadingDateTime as 'Reading DateTime',A.StatusMessage as 'Status Message'," +
                    "A.FileName as 'File Name',A.FilePath as 'File Path',B.Group_Name as 'Group Name',C.Schedule_Name as 'Schedule Name' from gsmreadingstatus A,gsmgroupschedule B,gsmschedule C" +
                " where A.GSMGroupSchedule_ID=B.GSMGroupSchedule_ID and A.GSMSchedule_ID=C.gsmSchedule_ID ORDER BY A.ReadingDateTime Desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public bool IsRead(string meterId, string period, long sDate,long eDate)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(Meter_ID) from gsmreadingstatus ");
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                builder.Append(string.Concat(" and ", SchedulePeriod, "=", ParameterName(SchedulePeriod)));
                builder.Append(string.Concat(" and ", ReadingDateTime, ">", ParameterName("SDATE")));
                builder.Append(string.Concat(" and ", ReadingDateTime, "<", ParameterName("EDATE")));
                builder.Append(string.Concat(" and ", Status, "=", ParameterName(Status)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterId, DbType.String,50);
                request.AddParamter(ParameterName(SchedulePeriod), period, DbType.String, 5);
                request.AddParamter(ParameterName("SDATE"), sDate, DbType.Int64);
                request.AddParamter(ParameterName("EDATE"), eDate, DbType.Int64);
                request.AddParamter(ParameterName(Status), 1, DbType.Int16);
               object val= helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected Area Deleted"));
                if(Int32.Parse(val.ToString())>0)
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsRead(string meterId, string period, long sDate,long eDate)", ex);
                Flag = false;
            }
            return Flag; 
            
        }
        public DataSet ListDataSet(long fromDate, long toDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.GSMReadingStatus_ID,A.Meter_ID as 'Meter ID',A.SchedulePeriod as 'Schedule Period', A.ReadingDateTime as 'Reading DateTime',A.StatusMessage as 'Status Message'," +
                 "A.FileName as 'File Name',A.FilePath as 'File Path',B.Group_Name as 'Group Name',C.Schedule_Name as 'Schedule Name' from gsmreadingstatus A,gsmgroupschedule B,gsmschedule C" +
             " where A.GSMGroupSchedule_ID=B.GSMGroupSchedule_ID and A.GSMSchedule_ID=C.gsmSchedule_ID" +
             " and A.ReadingDateTime >" + fromDate.ToString() + " and A.ReadingDateTime <" + toDate.ToString() +
                " ORDER BY A.ReadingDateTime Desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(long fromDate, long toDate)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet ConvertData(DataSet dataSet)
        {
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(System.Int64))); 
            foreach(DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName)); 
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["ID"] = slno;
                newRow["GSMReadingStatus_ID"] = row["GSMReadingStatus_ID"];
                newRow["Reading DateTime"] = DateUtility.LongToStringDateTimeFormat(long.Parse(row["Reading DateTime"].ToString())).ToString();
                newRow["Meter ID"] = row["Meter ID"];
                newRow["Status Message"] = row["Status Message"];
                newRow["File Name"] = row["File Name"];
                if (row["File Path"].ToString().IndexOf('|') >= 0)
                    newRow["File Path"] = row["File Path"].ToString().Replace('|', '/');
                else
                    newRow["File Path"] = row["File Path"].ToString();
                newRow["Group Name"] = row["Group Name"];
                newRow["Schedule Name"] = row["Schedule Name"];
                string val =Convert.ToString(row["Schedule Period"]);
                if (val.Trim() == "D")
                    newRow["Schedule Period"] = "Daily";
                if (val.Trim() == "W")
                    newRow["Schedule Period"] = "Weekly";
                if (val.Trim() == "M")
                    newRow["Schedule Period"] = "Monthly";
                if (val.Trim() == "E")
                    newRow["Schedule Period"] = "Month End";
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
 

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GSMReadingStatusEntity gSMReadingStatusEntity = new GSMReadingStatusEntity();
            if (NotNullAndNotDBNull(row, "ID")) gSMReadingStatusEntity.GSMReadingStatus_ID = Convert.ToInt64(row["ID"]);
            if (NotNullAndNotDBNull(row, "Time Stamp")) gSMReadingStatusEntity.ReadingDateTime = Convert.ToInt64(row["Time Stamp"]);
            if (NotNullAndNotDBNull(row, "Status Message")) gSMReadingStatusEntity.StatusMessage = Convert.ToString(row["Status Message"]);
            if (NotNullAndNotDBNull(row, "File Name")) gSMReadingStatusEntity.FileName = Convert.ToString(row["File Name"]);
            if (NotNullAndNotDBNull(row, "File Path")) gSMReadingStatusEntity.FilePath = Convert.ToString(row["File Path"]);
            if (NotNullAndNotDBNull(row, "Status")) gSMReadingStatusEntity.Status = Convert.ToInt32(row["Status"]);
            return gSMReadingStatusEntity;

        }
    }
}
