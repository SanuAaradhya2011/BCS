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
    public class UserLogActivityDAL : DALBase
    {
		private static UserLogActivityDAL _UserLogActivityDAL;
        private static readonly object activityLock = new object();
		public static UserLogActivityDAL GetInstance()
		{
            lock (activityLock)
            {
                if (_UserLogActivityDAL == null)
                    _UserLogActivityDAL = new UserLogActivityDAL();
                return _UserLogActivityDAL;
            }
		}

		private string UserInformation_ID = "UserInformation_ID";
        private string Activity_DateTime = "Activity_DateTime";
        private string Activity = "Activity";

		public void GenerateAndSaveLog(string Activity)
		{
			UserLogActivityEntity logActivity = new UserLogActivityEntity();
			logActivity.Activity = Activity;
			logActivity.Activity_DateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
			if(ConfigInfo.UserInformationID!=0)
			  logActivity.UserID = ConfigInfo.UserInformationID; 
			this.InsertData(logActivity);
		}

        public override IEntity InsertData(IEntity entity)
        {
            UserLogActivityEntity activity = null;
            try
            {
                  activity = entity as UserLogActivityEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into UserLogActivity(UserInformation_ID,Activity_DateTime,Activity) values(");
				builder.Append(string.Concat(ParameterName(UserInformation_ID), ","));
                builder.Append(string.Concat(ParameterName(Activity_DateTime), ","));
                builder.Append(string.Concat(ParameterName(Activity), ")"));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(UserInformation_ID), activity.UserID, System.Data.DbType.Int32, 8);
                request.AddParamter(ParameterName(Activity_DateTime), activity.Activity_DateTime, System.Data.DbType.UInt64, 14);
                request.AddParamter(ParameterName(Activity), activity.Activity, System.Data.DbType.String, 150);
                helper.ExecuteNonQuery(request); 
            }
            catch(CABException)
            {
                activity = null;
            }
            return activity;
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
                builder.Append("select A.Users_Name as 'User Name', B.Activity_DateTime ,B.Activity 'Activities' from  userinformation A Inner Join userlogactivity B on A.UserInformation_ID = B.UserInformation_ID order by Activity_DateTime desc");
				DataRequest request = new DataRequest(builder.ToString());
				dataSet = new DataSet();
				dataSet = helper.FillDataSet(request, dataSet); 
			}
			catch (CABException)//
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
				builder.Append("select A.Users_Name as 'User Name', B.Activity_DateTime,B.Activity 'Activities' from "
				+ "userinformation A Inner Join userlogactivity B on A.UserInformation_ID = B.UserInformation_ID "
				+ "where B.Activity_DateTime>"+fromDate.ToString()+ " and B.Activity_DateTime<"+toDate.ToString()+ " order by Activity_DateTime desc");
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
    }
}
