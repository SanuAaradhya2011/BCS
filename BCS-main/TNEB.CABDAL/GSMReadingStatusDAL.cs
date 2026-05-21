using System;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;

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
				builder.Append("Insert Into gsmreadingstatus(GSMReadingStatus_ID,ReadingDateTime,StatusMessage,FileName,FilePath,Status) values(");
				builder.Append(string.Concat(ParameterName(GSMReadingStatus_ID), ","));
				builder.Append(string.Concat(ParameterName(ReadingDateTime), ","));
				builder.Append(string.Concat(ParameterName(StatusMessage), ","));
				builder.Append(string.Concat(ParameterName(FileName), ")"));
				builder.Append(string.Concat(ParameterName(FilePath), ","));
				builder.Append(string.Concat(ParameterName(Status), ","));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(GSMReadingStatus_ID), gSMReadingStatusEntity.GSMReadingStatus_ID, DbType.Int64);
				request.AddParamter(ParameterName(ReadingDateTime), gSMReadingStatusEntity.ReadingDateTime, DbType.Int64);
				request.AddParamter(ParameterName(StatusMessage), gSMReadingStatusEntity.StatusMessage, DbType.String, 100);
				request.AddParamter(ParameterName(FileName), gSMReadingStatusEntity.FileName, DbType.String, 100);
				request.AddParamter(ParameterName(FilePath), gSMReadingStatusEntity.FilePath, DbType.String, 150);
				request.AddParamter(ParameterName(Status), gSMReadingStatusEntity.Status, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status added"));
			}
			catch (CABException)
			{
				Flag = false;
			}
            if (Flag)
                gSMReadingStatusEntity.GSMReadingStatus_ID = long.Parse(this.GetPK());
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
            catch (CABException)
            {
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
            catch (CABException)
            {
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
                builder.Append("Select GSMReadingStatus_ID as 'ID',ReadingDateTime as 'Time Stamp',StatusMessage as 'Status Message'," +
                    "FileName as 'File Name',FilePath as 'File Path',Status from gsmreadingstatus ORDER BY ReadingDateTime Desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
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
                builder.Append("Select GSMReadingStatus_ID as 'ID',ReadingDateTime as 'Time Stamp',StatusMessage as 'Status Message'," +
                    "FileName as 'File Name',FilePath as 'File Path',Status from gsmreadingstatus " + 
                    "where ReadingDateTime >" + fromDate.ToString()+ " and ReadingDateTime <" + toDate.ToString()+ " ORDER BY ReadingDateTime Desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
            }
            catch (CABException)
            {
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
            table.Columns.Add(new DataColumn("Time Stamp", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status Message", typeof(System.String)));
            table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("File Path", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.Int32)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["ID"] = slno;
                newRow["Time Stamp"] = DateUtility.LongToStringDateTimeFormat(long.Parse(row["Time Stamp"].ToString())).ToString();
                newRow["Status Message"] = row["Status Message"];
                newRow["File Name"] = row["File Name"];
                newRow["File Path"] = row["File Path"];
                newRow["Status"] = row["Status"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public DataSet ListDataSet(string columnName, int value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select GSMReadingStatus_ID as 'ID',ReadingDateTime as 'Time Stamp',StatusMessage as 'Status Message'," +
                    "FileName as 'File Name',FilePath as 'File Path',Status from gsmreadingstatus where ");
                builder.Append(string.Concat(columnName, "=", value));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                dataSet = ConvertData(dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
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
                builder.Append("Select GSMReadingStatus_ID as 'ID',ReadingDateTime as 'Time Stamp',StatusMessage as 'Status Message'," +
                    "FileName as 'File Name',FilePath as 'File Path',Status from gsmreadingstatus where ");
                builder.Append(string.Concat(columnName, " like '%", value, "%'"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                dataSet = ConvertData(dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Reading Status viewed"));
            }
            catch (CABException)
            {
                dataSet = null;
            }
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
