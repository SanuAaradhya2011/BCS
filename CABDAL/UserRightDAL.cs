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
using CAB.Framework;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using CAB.Entity;
using System;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
	public class UserRightDAL : DALBase
	{
		private string Right_ID = "Right_ID";
		private string Module_ID = "Module_ID";
		private string UserInformation_ID = "UserInformation_ID";
		private string Permission = "Permission";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UserRightDAL).ToString());

        public override IEntity InsertData(IEntity entity)
		{
            UserRightEntity userRightEntity = null;
            if (entity == null)
                return userRightEntity;
			bool Flag = false;
			try
			{
				  userRightEntity = entity as UserRightEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into UserRights(Module_ID,UserInformation_ID,Permission) values(");
				builder.Append(string.Concat(ParameterName(Module_ID), ","));
				builder.Append(string.Concat(ParameterName(UserInformation_ID), ","));
				builder.Append(string.Concat(ParameterName(Permission), ")"));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Module_ID), userRightEntity.Module_ID, DbType.Int32);
				request.AddParamter(ParameterName(UserInformation_ID), userRightEntity.UserInformation_ID, DbType.Int32);
				request.AddParamter(ParameterName(Permission), userRightEntity.Permission, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User rights inserted"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                userRightEntity = null;
			}
            return userRightEntity;
		}

		public override bool UpdateData(IEntity entity)
		{
			bool Flag = false;
			try
			{
				UserRightEntity userRightEntity = entity as UserRightEntity;  
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update UserRights Set ");
				builder.Append(string.Concat(Permission, "=", ParameterName(Permission)));
				builder.Append(string.Concat(" Where ", Module_ID, "=", ParameterName(Module_ID)," and "));
				builder.Append(string.Concat(UserInformation_ID, "=", ParameterName(UserInformation_ID))); 
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Permission), userRightEntity.Permission, DbType.Int16);
				request.AddParamter(ParameterName(Module_ID), userRightEntity.Module_ID, DbType.Int32);
				request.AddParamter(ParameterName(UserInformation_ID), userRightEntity.UserInformation_ID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User rights updated"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

		public override bool DeleteData(IEntity entity)
		{
			throw new NotImplementedException();
		}

		public override IEntity GetDetailData(int id)
		{
			throw new NotImplementedException();
		}
		public IEntity GetDetailData(int userId, int moduleId)
		{
			UserRightEntity userRightEntity = new UserRightEntity();
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select Right_ID,Module_ID,UserInformation_ID,Permission from UserRights where ");
				builder.Append(string.Concat(UserInformation_ID, "=", ParameterName(UserInformation_ID)," and "));
				builder.Append(string.Concat(Module_ID, "=", ParameterName(Module_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(UserInformation_ID), userId, DbType.Int32);
				request.AddParamter(ParameterName(Module_ID), moduleId, DbType.Int32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
				{
					userRightEntity = (UserRightEntity)RowToEntity(ds.Tables[0].Rows[0]);
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User rights retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(int userId, int moduleId)", ex);
			}
			return userRightEntity;
		}

		public override IList<IEntity> ListData()
		{
			throw new NotImplementedException();
		}

		public IList<UserRightEntity> ListData(int userId)
		{
			IList<UserRightEntity> userRightEntities = new List<UserRightEntity>();
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select Right_ID,Module_ID,UserInformation_ID,Permission from UserRights where ");
				builder.Append(string.Concat(UserInformation_ID, "=", ParameterName(UserInformation_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(UserInformation_ID), userId, DbType.Int32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in ds.Tables[0].Rows)
						userRightEntities.Add((UserRightEntity)RowToEntity(row));
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("User rights for a specified user retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "ListData(int userId)", ex);
			}
			return userRightEntities;
		}

		public override DataSet ListDataSet()
		{
			throw new NotImplementedException();
		}

		public override IEntity RowToEntity(DataRow row)
		{ 
			if (row == null) return null;
			UserRightEntity userRightEntity = new UserRightEntity();
			if (NotNullAndNotDBNull(row, Right_ID)) userRightEntity.UserInformation_ID = Convert.ToInt32(row[Right_ID]);
			if (NotNullAndNotDBNull(row, Module_ID)) userRightEntity.Module_ID = Convert.ToInt32(row[Module_ID]);
			if (NotNullAndNotDBNull(row, UserInformation_ID)) userRightEntity.UserInformation_ID = Convert.ToInt32(row[UserInformation_ID]);
			if (NotNullAndNotDBNull(row, Permission)) userRightEntity.Permission = Convert.ToInt16(row[Permission]);
			return userRightEntity;
		}

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public bool CheckPermission(int userID, string moduleName)
        {
            bool Flag = false;
            try
            {
                int moduleId = new ModuleMasterDAL().GetModuleIdByName(moduleName);
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Permission from userrights where ");
                builder.Append(string.Concat(Module_ID,"=",ParameterName(Module_ID)," and "));
                builder.Append(string.Concat(UserInformation_ID, "=", ParameterName(UserInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Module_ID), moduleId, DbType.Int32);
                request.AddParamter(ParameterName(UserInformation_ID), userID, DbType.Int64);   
                object data = helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Permissions for a specified user retrieved"));
                if ((Convert.ToInt64(data)) == 1)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckPermission(int userID, string moduleName)", ex);
                Flag = false;
            }
            return Flag;
        }
    }
}
