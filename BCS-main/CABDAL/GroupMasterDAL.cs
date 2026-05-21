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
	public class GroupMasterDAL : DALBase
	{
		private string Group_ID = "Group_ID";
		private string Group_Name = "Group_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GroupMasterDAL).ToString());

		public override IEntity InsertData(IEntity entity)
		{
			GroupMasterEntity groupMasterEntity = null;
			if (entity == null)
				return groupMasterEntity;
			groupMasterEntity = entity as GroupMasterEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into group_master(Group_ID,Group_Name) values(");
				builder.Append(string.Concat(ParameterName(Group_ID), ","));
				builder.Append(string.Concat(ParameterName(Group_Name), ")"));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Group_ID), groupMasterEntity.Group_ID, DbType.Int32);
				request.AddParamter(ParameterName(Group_Name), groupMasterEntity.Group_Name, DbType.String, 35);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New group data added"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
				groupMasterEntity = null;
			}
			return groupMasterEntity;
		}

        public void InsertDefaultValues(string[] defaultSubGroups)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper(); 
                int count = 0;
                for (count = 0; count < 5; count++)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("Insert Into group_master(Group_Name) values(");
                    builder.Append(string.Concat(ParameterName(Group_Name), ")")); 
                    DataRequest request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(Group_Name), defaultSubGroups[count], DbType.String, 35);
                    helper.ExecuteNonQuery(request);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New group default data added"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertDefaultValues(string[] defaultSubGroups)", ex);
            }
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
				GroupMasterEntity groupMasterEntity = entity as GroupMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update group_master Set ");
				builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID), ","));
				builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name)));
				builder.Append(string.Concat(" Where ", Group_Name, "=", ParameterName(Group_Name)));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Group_ID), groupMasterEntity.Group_ID, DbType.Int32);
				request.AddParamter(ParameterName(Group_Name), groupMasterEntity.Group_Name, DbType.String, 35);
				helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data updated"));
				Flag = true;
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

        public void DeleteAllData()
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete * from group_master");
                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteAllData()", ex);
            }
        }

		public override bool DeleteData(IEntity entity)
		{
			bool Flag = false;
			try
			{
				GroupMasterEntity groupMasterEntity = entity as GroupMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete * from group_master where ");
				builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID), " and "));
				builder.Append(string.Concat(Group_Name, "=", ParameterName(Group_Name)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Group_ID), groupMasterEntity.Group_ID, DbType.Int32);
				request.AddParamter(ParameterName(Group_Name), groupMasterEntity.Group_Name, DbType.String, 35);
				helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data deleted"));
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
			throw new NotImplementedException();
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
				builder.Append("SELECT * from group_master");

				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data viewed"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
				ds = null;
			}
			return ds;
		}

		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
			GroupMasterEntity groupMasterEntity = new GroupMasterEntity();

			if (NotNullAndNotDBNull(row, Group_ID)) groupMasterEntity.Group_ID = Convert.ToInt32(row[Group_ID]);
			if (NotNullAndNotDBNull(row, Group_Name)) groupMasterEntity.Group_Name = Convert.ToString(row[Group_Name]);

			return groupMasterEntity;
		}

        public int ListDefaultValues()
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select count(*) from  group_master");
                DataRequest request = new DataRequest(builder.ToString()); 
                object data = helper.ExecuteScalar(request);
                return Convert.ToInt32(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDefaultValues()", ex);
                return 0;
            }
        }
	}
}
