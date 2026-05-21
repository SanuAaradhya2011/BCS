/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
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
    public class ApplicationLogDAL : DALBase
    {
        private static ApplicationLogDAL applicationLogDAL;
        private static readonly object activityLock = new object();
        public static ApplicationLogDAL GetInstance()
		{
            lock (activityLock)
            {
                if (applicationLogDAL == null)
                    applicationLogDAL = new ApplicationLogDAL();
                return applicationLogDAL;
            }
		}
        private string LogID = "LogID";
        private string UserID = "UserID";
        private string StartDateTime = "StartDateTime";
        private string EndDateTime = "EndDateTime";

		public long GenerateAndSaveLog()
		{
            LogInformationEntity logInformationEntity = new LogInformationEntity();
            logInformationEntity.StartDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
			if(ConfigInfo.UserInformationID!=0)
                logInformationEntity.UserID = ConfigInfo.UserInformationID;
            this.InsertData(logInformationEntity);
            return logInformationEntity.LogID;
		}

        public ApplicationLogDAL()
            : base("LoginMaster", "LogID")
        {
        }

        public override IEntity InsertData(IEntity entity)
        {
            LogInformationEntity logInformationEntity = null;
            try
            {
                logInformationEntity = entity as LogInformationEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into LoginMaster(UserID,StartDateTime) values(");
                builder.Append(string.Concat(ParameterName(UserID), ","));
                builder.Append(string.Concat(ParameterName(StartDateTime), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(UserID), logInformationEntity.UserID, System.Data.DbType.Int32);
                request.AddParamter(ParameterName(StartDateTime), logInformationEntity.StartDateTime, System.Data.DbType.Int64); 
                helper.ExecuteNonQuery(request); 
            }
            catch(CABException)
            {
                logInformationEntity = null;
            }
            if (logInformationEntity!=null)
                logInformationEntity.LogID = long.Parse(this.GetPK());
            return logInformationEntity; 
        }

        public bool UpdateData(long logId)
        {
            bool flag = false; 
            try
            {  
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update LoginMaster Set ");
                builder.Append(string.Concat(EndDateTime, "=", ParameterName(EndDateTime)));
                builder.Append(string.Concat(" Where ", LogID, "=", ParameterName(LogID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(EndDateTime), DateUtility.DateTimeToLong(System.DateTime.Now), DbType.Int64);
                request.AddParamter(ParameterName(LogID), logId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                flag = true;
            }
            catch (CABException)
            { 
            }
            return flag;
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
                builder.Append("select A.Login_ID as 'User Name', B.StartDateTime ,B.EndDateTime from  userinformation A Inner Join LoginMaster B on A.UserInformation_ID = B.UserID order by B.StartDateTime desc");
				DataRequest request = new DataRequest(builder.ToString());
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet); 
			}
			catch (CABException) 
			{
				dataSet = null;
			}
			return dataSet;  
        }

		public DataSet ListDataSet(long fromDate,long toDate)
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("select A.Login_ID as 'User Name', B.StartDateTime,B.EndDateTime from "
                + "userinformation A Inner Join LoginMaster B on A.UserInformation_ID = B.UserID "
                + "where B.StartDateTime>" + fromDate.ToString() + " and B.StartDateTime<" + toDate.ToString() + " order by B.StartDateTime desc");
				DataRequest request = new DataRequest(builder.ToString());
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet); 
			}
			catch (CABException)
			{
				dataSet = null;
			}
			return dataSet;  
		}


        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new System.NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}

