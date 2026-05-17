/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 											|
 * | 										    Date   : 24 May 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;
using System;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
	public class SubGroupMasterDAL : DALBase
	{

		private string SubGroup_ID = "SubGroup_ID";
		private string SubGroup_Name = "SubGroup_Name";
		private string Group_Name = "Group_Name";
		private string SubGroup_Description = "SubGroup_Description";
		private string Group_ID = "Group_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SubGroupMasterDAL).ToString());

		public override IEntity InsertData(IEntity entity)
		{
			SubGroupMasterEntity subGroupMasterEntity = null;
			if (entity == null)
				return subGroupMasterEntity;
			subGroupMasterEntity = entity as SubGroupMasterEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into subgroup_master(SubGroup_Name,SubGroup_Description,Group_ID) values(");
				builder.Append(string.Concat(ParameterName(SubGroup_Name), ","));
				builder.Append(string.Concat(ParameterName(SubGroup_Description), ","));
				builder.Append(string.Concat(ParameterName(Group_ID), ")"));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_Name), subGroupMasterEntity.SubGroup_Name, DbType.String, 35);
				request.AddParamter(ParameterName(SubGroup_Description), subGroupMasterEntity.SubGroup_Description, DbType.String,50);
				request.AddParamter(ParameterName(Group_ID), subGroupMasterEntity.Group_ID, DbType.Int32);
				
				helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group inserted"));
				Flag = true;
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
				subGroupMasterEntity = null;
			}
			return subGroupMasterEntity;
		}

		public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateData(IEntity entity)
		{
			bool Flag = false;
			try
			{
				SubGroupMasterEntity subGroupMasterEntity = entity as SubGroupMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update subgroup_master Set ");
				//builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID), ","));
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name), ","));
				builder.Append(string.Concat(SubGroup_Description, "=", ParameterName(SubGroup_Description),","));
				builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID)));
				builder.Append(string.Concat(" Where ", Group_ID, "=", ParameterName(Group_ID), " and "));
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));
				DataRequest request = new DataRequest(builder.ToString());
				//request.AddParamter(ParameterName(SubGroup_ID), subGroupMasterEntity.SubGroup_ID, DbType.Int64);
				request.AddParamter(ParameterName(SubGroup_Name), subGroupMasterEntity.SubGroup_Name, DbType.String, 35);
				request.AddParamter(ParameterName(SubGroup_Description), subGroupMasterEntity.SubGroup_Description, DbType.String,50);
				request.AddParamter(ParameterName(Group_ID), subGroupMasterEntity.Group_ID, DbType.String, 20);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group updated"));
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
			bool Flag = false;
			try
			{

				SubGroupMasterEntity subGroupMasterEntity = entity as SubGroupMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete * from subgroup_master where ");
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID), " and "));
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupMasterEntity.SubGroup_ID, DbType.Int32);
				request.AddParamter(ParameterName(SubGroup_Name), subGroupMasterEntity.SubGroup_Name, DbType.String,35);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group deleted"));
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
			throw new NotImplementedException();
		}

		public bool DeleteData(int subGroupID)
		{
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete from subgroup_master where ");
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group deleted"));
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "DeleteData(int subGroupID)", ex);
				Flag = false;
			}
			return Flag;
		}

		public int GetSubGroupID(int groupID,string subGroupName)
		{
			int subGroupID = 0;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT SubGroup_ID from subgroup_master where ");
				builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID), " and "));
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Group_ID), groupID, DbType.Int32);
				request.AddParamter(ParameterName(SubGroup_Name), subGroupName, DbType.String, 35);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group ID retrieved"));
				object data = helper.ExecuteScalar(request);
				if (data == null)
				{
					return 0;
				}
				if ((Convert.ToInt64(data.ToString())) > 0)
				{
					return subGroupID = Convert.ToInt16(data.ToString());
				}
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetSubGroupID(int groupID,string subGroupName)", ex);
				return 0;
			}
			return subGroupID;
		}

		public bool GetSubGroupName(string subGroupName)
		{

			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT SubGroup_Name from subgroup_master where ");
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_Name), subGroupName, DbType.String, 35);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group name retrieved"));
				object data = helper.ExecuteScalar(request);
				if (data == null)
				{
					return false;
				}
				if (data.ToString() == subGroupName)
				{
					return true;
				}
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetSubGroupName(string subGroupName)", ex);
				return false;
			}
			return false;
		}


		public override System.Collections.Generic.IList<IEntity> ListData()
		{
			throw new NotImplementedException();
		}

		public override DataSet ListDataSet()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT * from subgroup_master");

				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group data retrieved"));
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
				ds = null;
			}
			return ds;
		}

		public DataSet GetGroupNameListValues(int groupID)
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT SubGroup_Name from subgroup_master where ");
				builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Group_ID), groupID, DbType.Int16);
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group name for the specified group retrieved"));
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetGroupNameListValues(int groupID)", ex);
				ds = null;
			}
			return ds;
		}

		public string GetDescriptionForSubGroupName(string subGroupName)
		{
			string description = string.Empty;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT SubGroup_Description from subgroup_master where ");
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_Name), subGroupName, DbType.String, 35);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Description for a specified sub group retrieved"));
				object data = helper.ExecuteScalar(request);
				if (data == null)
				{
					description = string.Empty;
				}
				if (data.ToString() != string.Empty)
				{
					description = data.ToString();
				}
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDescriptionForSubGroupName(string subGroupName)", ex);
				return "";
			}
			return description;
		}

		public bool CheckForAllInSuspectedConsumers()
		{
			bool isAvailable = true;
			string subGroupName = "All";
			string groupNameSuspected = "Suspected Consumer";
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT SubGroup_Name from subgroup_master where ");
				builder.Append(string.Concat(SubGroup_Name, "=", ParameterName(SubGroup_Name)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_Name), subGroupName, DbType.String, 35);
                string data =Convert.ToString( helper.ExecuteScalar(request));
                 
				if (string.IsNullOrEmpty(data)) 
					isAvailable = false; 

				if (isAvailable == false)
				{
					int groupID = GetGroupIDFromGroupName(groupNameSuspected);
					SubGroupMasterEntity subGroupMasterEntity = new SubGroupMasterEntity();
					subGroupMasterEntity.Group_ID = groupID;
					subGroupMasterEntity.SubGroup_Name = subGroupName;
					subGroupMasterEntity.SubGroup_Description = groupNameSuspected;
					InsertData(subGroupMasterEntity);
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub group names retrieved"));
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "CheckForAllInSuspectedConsumers()", ex);
			}
			return true;
		}

		public int GetGroupIDFromGroupName(string groupName)
		{
			IDataHelper helper = DatabaseFactory.GetHelper();
			StringBuilder builder = new StringBuilder();
			builder.Append("SELECT Group_ID,Group_Name from group_master where ");
			builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name)));
			DataRequest request = new DataRequest(builder.ToString());
			request.AddParamter(ParameterName(Group_Name), groupName, DbType.String, 35);
			object data = helper.ExecuteScalar(request);
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group ID for a specified sub group retrieved"));
			if (Convert.ToInt32(data) > 0)
			{
				return Convert.ToInt32(data);
			}
			return 0;
		}
	

		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
			SubGroupMasterEntity subGroupMasterEntity = new SubGroupMasterEntity();

			if (NotNullAndNotDBNull(row, SubGroup_ID)) subGroupMasterEntity.SubGroup_ID = Convert.ToInt32(row[SubGroup_ID]);
			if (NotNullAndNotDBNull(row, SubGroup_Name)) subGroupMasterEntity.SubGroup_Name = Convert.ToString(row[SubGroup_Name]);
			if (NotNullAndNotDBNull(row, SubGroup_Description)) subGroupMasterEntity.SubGroup_Description = Convert.ToString(row[SubGroup_Description]);
			if (NotNullAndNotDBNull(row, Group_ID)) subGroupMasterEntity.Group_ID = Convert.ToInt32(row[Group_ID]);

			return subGroupMasterEntity;
		}
	}
}
