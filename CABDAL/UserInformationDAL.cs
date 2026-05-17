using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using System.Collections.Generic;
using System.Data;
using CAB.Entity;
using System;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class UserInformationDAL : DALBase
    {
        public UserInformationDAL() : base("UserInformation","UserInformation_ID")
        {
        }
        private string UserInformation_ID = "UserInformation_ID";
		private string User_Name = "Users_Name";
        private string User_Password = "User_Password";
        private string Category_ID = "Category_ID";
        private string Designation_ID = "Designation_ID";
		private string Login_ID = "Login_ID";
        private string IsActive = "IsActive";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UserInformationDAL).ToString());

        public override IEntity ValidateUser(IEntity entity)
        {
            UserInformationEntity userInformation = null;
            try
            {
                userInformation = entity as UserInformationEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select UserInformation_ID,Users_Name,User_Password,Category_ID,Login_ID,Designation_ID,IsActive from UserInformation where IsActive=0 and ");
				builder.Append(string.Concat(Login_ID, "=", ParameterName(Login_ID), " and "));
                builder.Append(string.Concat(User_Password,"=", ParameterName(User_Password)));
                DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Login_ID), userInformation.Login_ID, DbType.String, 20);
                request.AddParamter(ParameterName(User_Password), userInformation.User_Password, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if(ds==null)
                    return null;
                if (ds.Tables.Count == 0)
                    return null;
				if (ds.Tables[0].Rows.Count > 0)
                {
                    userInformation = (UserInformationEntity)RowToEntity(ds.Tables[0].Rows[0]);
                    ConfigInfo.UserInformationID = userInformation.UserInformation_ID;
					UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", userInformation.User_Name, " logged in system"));
				}
                
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateUser(IEntity entity)", ex);
                userInformation = null;
            }
            return userInformation;
        }

        public override IEntity InsertData(IEntity entity)
        {
            UserInformationEntity objUserInformationEntity = null;
			if (entity == null)
                return objUserInformationEntity;
			  objUserInformationEntity = entity as UserInformationEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into userinformation(Users_Name,User_Password,Category_ID,Login_ID,Designation_ID,IsActive) values(");
				builder.Append(string.Concat(ParameterName(User_Name), ","));
				builder.Append(string.Concat(ParameterName(User_Password), ","));
				builder.Append(string.Concat(ParameterName(Category_ID), ","));
				builder.Append(string.Concat(ParameterName(Login_ID), ","));
				builder.Append(string.Concat(ParameterName(Designation_ID), ","));
				builder.Append(string.Concat(ParameterName(IsActive), ")")); 
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(User_Name), objUserInformationEntity.User_Name, DbType.String, 35);
				request.AddParamter(ParameterName(User_Password), objUserInformationEntity.User_Password, DbType.UInt64, 10);
				if(objUserInformationEntity.Category_ID!=0)
				request.AddParamter(ParameterName(Category_ID), objUserInformationEntity.Category_ID, DbType.Int32);
				else
					request.AddParamter(ParameterName(Category_ID), null, DbType.Int32);
				request.AddParamter(ParameterName(Login_ID), objUserInformationEntity.Login_ID, DbType.String, 20);
				if (objUserInformationEntity.Designation_ID != 0)
					request.AddParamter(ParameterName(Designation_ID), objUserInformationEntity.Designation_ID, DbType.Int32);
				else
					request.AddParamter(ParameterName(Designation_ID), null, DbType.Int32);
				request.AddParamter(ParameterName(IsActive), objUserInformationEntity.IsActive, DbType.Int32);
				helper.ExecuteNonQuery(request);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", objUserInformationEntity.User_Name, " created"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                objUserInformationEntity = null;
			}
            return objUserInformationEntity;
        }
        public bool ValidateLoginID(string logingId)
        {
            bool Flag=false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Login_ID from userinformation where IsActive=0 and ");
                builder.Append(string.Concat(Login_ID, "=", ParameterName(Login_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Login_ID), logingId, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User verified"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateLoginID(string logingId)", ex);
                Flag = true;
            }
            return Flag;
        }
        public override bool UpdateData(IEntity entity)
        {
			bool Flag = false;
			try
			{
				UserInformationEntity objUserInformationEntity = entity as UserInformationEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update userinformation Set ");
				builder.Append(string.Concat(User_Name, "=", ParameterName(User_Name), ","));
				builder.Append(string.Concat(User_Password, "=", ParameterName(User_Password), ","));
				builder.Append(string.Concat(Category_ID, "=", ParameterName(Category_ID), ","));
				builder.Append(string.Concat(Designation_ID, "=", ParameterName(Designation_ID)));
				builder.Append(string.Concat(" Where ", UserInformation_ID, "=", ParameterName(UserInformation_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(User_Name), objUserInformationEntity.User_Name, DbType.String, 35);
				request.AddParamter(ParameterName(User_Password), objUserInformationEntity.User_Password, DbType.UInt64, 10);
				request.AddParamter(ParameterName(Category_ID), objUserInformationEntity.Category_ID, DbType.Int32);
				request.AddParamter(ParameterName(Designation_ID), objUserInformationEntity.Designation_ID, DbType.Int32);
				request.AddParamter(ParameterName(UserInformation_ID), objUserInformationEntity.UserInformation_ID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", objUserInformationEntity.User_Name, " modified"));
				Flag = true;
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
        }

		public bool UpdatePassword(IEntity entity)
		{
			bool Flag = false;
			try
			{
				UserInformationEntity objUserInformationEntity = entity as UserInformationEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update userinformation Set ");
				builder.Append(string.Concat(User_Password, "=", ParameterName(User_Password)));
				builder.Append(string.Concat(" Where ", UserInformation_ID, "=", ParameterName(UserInformation_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(User_Password), objUserInformationEntity.User_Password, DbType.String, 10);
				request.AddParamter(ParameterName(UserInformation_ID), objUserInformationEntity.UserInformation_ID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", objUserInformationEntity.User_Name, " password changed"));
				Flag = true;
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "UpdatePassword(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}


        public override bool DeleteData(IEntity entity)
        {
			bool Flag = false;
			try
			{
				UserInformationEntity objUserInformationEntity = entity as UserInformationEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update userinformation Set ");
				builder.Append(string.Concat(IsActive, "=", ParameterName(IsActive)));
				builder.Append(string.Concat(" Where ", UserInformation_ID, "=", ParameterName(UserInformation_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(IsActive), objUserInformationEntity.IsActive, DbType.Int32);
				request.AddParamter(ParameterName(UserInformation_ID), objUserInformationEntity.UserInformation_ID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", objUserInformationEntity.User_Name, " deleted"));
				Flag = true;
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
        }

		public DataSet GetModuleMasterData()
		{
			DataSet dataSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select Module_ID,Module_Name from module_master");
				DataRequest request = new DataRequest(builder.ToString()); 
				dataSet = helper.FillDataSet(request, dataSet);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Module list viewed"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetModuleMasterData()", ex);
				dataSet=null;
			}
			return dataSet;
		}


        public override IEntity GetDetailData(int id)
        {
			UserInformationEntity userInformation = null;
			try
			{ 
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select UserInformation_ID,Users_Name,User_Password,Category_ID,Login_ID,Designation_ID,IsActive from UserInformation where ");
				builder.Append(string.Concat(UserInformation_ID, "=", ParameterName(UserInformation_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(UserInformation_ID), id, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					userInformation = (UserInformationEntity)RowToEntity(ds.Tables[0].Rows[0]);

				if(userInformation!=null)
					UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User ", userInformation.User_Name, " Viewed."));
				else
					UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User viewed"));

			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
				userInformation = null;
			}
			return userInformation;
        }

        public override IList<IEntity> ListData()
        {
            return null;
        }
        public DataSet ListDataSet(string columnName, string value)
        { 
            DataSet dataSet = null;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Select UserInformation_ID, Users_Name, Category_ID, Login_ID, Designation_ID from UserInformation where IsActive=0 and ");
                DataRequest request = new DataRequest(builder.ToString());
                if (columnName.Equals(User_Name))
                {
                    builder.Append(string.Concat(User_Name, " like '", value, "%'"));
                    request = new DataRequest(builder.ToString());
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on user name"));
                }
                else if (columnName.Equals(Login_ID))
                {
                    builder.Append(string.Concat(Login_ID, " like '", value, "%'"));
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on login ID"));
                    request = new DataRequest(builder.ToString()); 
                }
                else if (columnName.Equals(Category_ID))
                {
                    builder.Append(string.Concat("Category_ID=", value));
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on category ID"));
                    request = new DataRequest(builder.ToString());
                }
                else if (columnName.Equals(Designation_ID))
                {
                    builder.Append(string.Concat("Designation_ID=", value));
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on designation ID"));
                    request = new DataRequest(builder.ToString());
                }
                else
                {
                    return null;
                }
                IDataHelper helper = DatabaseFactory.GetHelper();
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string columnName, string value)", ex);
            }
            return dataSet;
        }

        public DataSet ListDataSet(string columnName, int value)
        {
            DataSet dataSet = null;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Select UserInformation_ID, Users_Name, Category_ID, Login_ID, Designation_ID from UserInformation where IsActive=0 and ");
                DataRequest request = new DataRequest(builder.ToString());
                if (columnName.Equals(Designation_ID))
                {
                    builder.Append(string.Concat(Designation_ID, "=", ParameterName(Designation_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Designation_ID), value, DbType.Int32);
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on designation ID"));
                }
                else if (columnName.Equals(Category_ID))
                {
                    builder.Append(string.Concat(Category_ID, "=", ParameterName(Category_ID)));
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Category_ID), value, DbType.Int32);
					UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User searched based on category ID"));
                }
                else
                {
                    return null;
                }
                IDataHelper helper = DatabaseFactory.GetHelper();
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string columnName, int value)", ex);
            }
            return dataSet;
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
				builder.Append("Select  UserInformation_ID, Users_Name, Category_ID, Login_ID, Designation_ID from UserInformation where IsActive=0");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
				UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User information retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            UserInformationEntity userInformation = new UserInformationEntity();
            if (NotNullAndNotDBNull(row, UserInformation_ID)) userInformation.UserInformation_ID = Convert.ToInt32(row[UserInformation_ID]);
            if (NotNullAndNotDBNull(row, User_Name)) userInformation.User_Name = Convert.ToString(row[User_Name]);
            if (NotNullAndNotDBNull(row, User_Password)) userInformation.User_Password = Convert.ToString(row[User_Password]);
            if (NotNullAndNotDBNull(row, Category_ID)) userInformation.Category_ID = Convert.ToInt32(row[Category_ID]);
            if (NotNullAndNotDBNull(row, Designation_ID)) userInformation.Designation_ID = Convert.ToInt32(row[Designation_ID]);
			if (NotNullAndNotDBNull(row, User_Password)) userInformation.User_Confirm_Password = Convert.ToString(row[User_Password]);
			if (NotNullAndNotDBNull(row, Login_ID)) userInformation.Login_ID = Convert.ToString(row[Login_ID]);
            if (NotNullAndNotDBNull(row, IsActive)) userInformation.IsActive = Convert.ToInt32(row[IsActive]);
            return userInformation;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
