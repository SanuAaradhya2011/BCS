using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using System.Data;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class ExceptionLogDetailsDAL : DALBase
    {
        private static ExceptionLogDetailsDAL exceptionLogDeatilsDAL;
        private static readonly object exceptionLock = new object();
        public static ExceptionLogDetailsDAL GetInstance()
        {
            lock (exceptionLock)
            {
                if (exceptionLogDeatilsDAL == null)
                    exceptionLogDeatilsDAL = new ExceptionLogDetailsDAL();
                return exceptionLogDeatilsDAL;
            }
        }

        private string  LogID = "LogID";
        private string LogDate = "LogDate";
        private string LogSource = "LogSource";
        private string LogMessage = "LogMessage";
        private string LogMacID = "LogMacID";
        private string  UserInformationID = "UserInformationID";

        public override IEntity InsertData(IEntity entity)
        {
            ExceptionLogEntity logEntity = null;
            try
            {
                logEntity = entity as ExceptionLogEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into exceptionlog values(");
                builder.Append(string.Concat(ParameterName(LogID.ToString()), ","));
                builder.Append(string.Concat(ParameterName(LogDate), ","));
                builder.Append(string.Concat(ParameterName(LogSource), ")"));
                builder.Append(string.Concat(ParameterName(LogMessage), ","));
                builder.Append(string.Concat(ParameterName(LogMacID), ","));
                builder.Append(string.Concat(ParameterName(UserInformationID.ToString()), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(LogID.ToString()), logEntity.LogID, System.Data.DbType.Int64);
                request.AddParamter(ParameterName(LogDate), logEntity.LogDate, System.Data.DbType.UInt64);
                request.AddParamter(ParameterName(LogSource), logEntity.LogSource, System.Data.DbType.String);
                request.AddParamter(ParameterName(LogMessage), logEntity.LogMessage, System.Data.DbType.String);
                request.AddParamter(ParameterName(LogMacID), logEntity.LogMacID, System.Data.DbType.UInt32);
                request.AddParamter(ParameterName(UserInformationID.ToString()), logEntity.UserInformationID, System.Data.DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Exception log inserted"));
            }
            catch (CABException)
            {
                logEntity = null;
            }
            return logEntity;
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
            throw new System.NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new System.NotImplementedException();
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
                builder.Append("select Log_ID as 'Exception ID', Log_Date as 'Date', Log_Source as 'Source', Log_Message as 'Message', "
                + "Log_MacID as 'Machine ID', UserInformation_ID as 'User ID', Log_Exception as 'Exception' from exceptionlog");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Exception log viewed"));
                return dataSet;
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
                builder.Append("select Log_ID as 'Exception ID', Log_Date as 'Date', Log_Source as 'Source', Log_Message as 'Message', "
                + "Log_MacID as 'Machine ID', UserInformation_ID as 'User ID', Log_Exception as 'Exception' from exceptionlog "
                + "where Log_Date>" + fromDate.ToString() + " and Log_Date<" + toDate.ToString());
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Exception log viewed"));

            }
            catch (CABException)
            {
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new System.NotImplementedException();
        }
    }
}
