using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.Entity.Base;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

namespace ExceptionServices.Data
{
    public class ApplicationLogDAL : DALBase
    {
        private string Log_ID = "Log_ID";
        private string Log_Date = "Log_Date";
        private string Log_Source = "Log_Source";
        private string Log_Message = "Log_Message";
        private string Log_MacID = "Log_MacID";
        private string UserInformation_ID = "UserInformation_ID";
        private string Log_Exception = "Log_Exception";

        public override IEntity InsertData(IEntity entity)
        {
            ApplicationLogEntity applicationLog = null;
            if (entity == null)
                return applicationLog;
            applicationLog = entity as ApplicationLogEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                MySQLHelper mySQLHelper = helper as MySQLHelper;
                MySql.Data.MySqlClient.MySqlConnection connections = new MySql.Data.MySqlClient.MySqlConnection(mySQLHelper.ConnectionString);
                MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand();
                connections.Open();
                command.Connection = connections;
                command.CommandText = "Insert Into exceptionlog(Log_Date,Log_Source,Log_Message,Log_MacID,UserInformation_ID) values(@Log_Date,@Log_Source,@Log_Message,@Log_MacID,@UserInformation_ID)";
                command.Parameters.AddWithValue("@Log_Date", applicationLog.LogDate);
                command.Parameters.AddWithValue("@Log_Source", applicationLog.LogSource);
                command.Parameters.AddWithValue("@Log_Message", applicationLog.LogMessage);
                command.Parameters.AddWithValue("@Log_MacID", applicationLog.LogMacID);
                command.Parameters.AddWithValue("@UserInformation_ID", applicationLog.UserID); 
                command.ExecuteNonQuery();
            }
            catch (Exception ex )
            {
                applicationLog = null;
            }
            return applicationLog;
        }


        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
