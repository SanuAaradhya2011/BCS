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
using System.Data;
using CAB.Entity;
using System;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
	public class CategoryMasterDAL:DALBase
	{
		private string Category_ID = "Category_ID";
		private string Category_Name = "Category_Name";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CategoryMasterDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            CategoryMasterEntity categoryMasterEntity=null;
            if (entity == null)
                return categoryMasterEntity;
              categoryMasterEntity = entity as CategoryMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into Category_Master(Category_Name) values(");
                builder.Append(string.Concat(ParameterName(Category_Name), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_Name), categoryMasterEntity.Category_Name, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category ", categoryMasterEntity.Category_Name, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                categoryMasterEntity = null;
            }
            return categoryMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                CategoryMasterEntity categoryMasterEntity = entity as CategoryMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update Category_Master Set ");
                builder.Append(string.Concat(Category_Name, "=", ParameterName(Category_Name)));
                builder.Append(string.Concat(" Where ", Category_ID, "=", ParameterName(Category_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_ID), categoryMasterEntity.Category_ID, DbType.Int64);
                request.AddParamter(ParameterName(Category_Name),categoryMasterEntity.Category_Name, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category ", categoryMasterEntity.Category_Name, " modified"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
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
                CategoryMasterEntity categoryMasterEntity = entity as CategoryMasterEntity;
                if (categoryMasterEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From Category_Master ");
                builder.Append(string.Concat(" Where ", Category_ID, "=", ParameterName(Category_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_ID), categoryMasterEntity.Category_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category ", categoryMasterEntity.Category_Name, " deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

		public override IEntity GetDetailData(int id)
		{
			CategoryMasterEntity categoryMasterEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select Category_ID,Category_Name from Category_Master where ");
				builder.Append(string.Concat(Category_ID, "=", ParameterName(Category_ID)));
				DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_ID), id, DbType.Int64);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					categoryMasterEntity = (CategoryMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);

			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
				categoryMasterEntity = null;
			}
			return categoryMasterEntity;
		}

		public override IList<IEntity> ListData()
		{
			throw new NotImplementedException();
		}

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Category_ID,Category_Name from Category_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public IEntity GetDetailData(string categoryName)
        {
            CategoryMasterEntity categoryMasterEntity = new CategoryMasterEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Category_ID,Category_Name from Category_Master where ");
                builder.Append(string.Concat(Category_Name, "=", ParameterName(Category_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_Name), categoryName, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    categoryMasterEntity = (CategoryMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string categoryName)", ex);
            }
            return categoryMasterEntity;
        }

		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
			CategoryMasterEntity categoryMasterEntity = new CategoryMasterEntity();
			if (NotNullAndNotDBNull(row, Category_ID)) categoryMasterEntity.Category_ID = Convert.ToInt32(row[Category_ID]);
			if (NotNullAndNotDBNull(row, Category_Name)) categoryMasterEntity.Category_Name = Convert.ToString(row[Category_Name]);
			return categoryMasterEntity;
		}

        public DataSet GetAllData()
        {
            DataSet dSet = null;
            try
            {
                dSet = new DataSet();
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from category_master");
                DataRequest request = new DataRequest(builder.ToString());
                helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllData()", ex);
                dSet = null;
            }
            return dSet;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void InsertDefaultCategory()
        {
            string[] qry = new string[5];
            qry[0] = "Insert Into category_master(Category_Name) values('Administrator')";
            qry[1] = "Insert Into category_master(Category_Name) values('Master')";
            qry[2] = "Insert Into category_master(Category_Name) values('Utility')";
            qry[3] = "Insert Into category_master(Category_Name) values('Reader')";
            qry[4] = "Insert Into category_master(Category_Name) values('Data Store Administrator')";
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 5; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Default categories inserted"));
        }

        public int GetCategoryIDByName(string categoryName)
        {
            int categoryId = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Category_ID from category_master where ");
                builder.Append(string.Concat(Category_Name, "=", ParameterName(Category_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Category_Name), categoryName, DbType.String, 35);
                object obj = helper.ExecuteScalar(request);
                categoryId = Convert.ToInt32(obj);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Category ID for a specified category retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCategoryIDByName(string categoryName)", ex);
                categoryId = 0;
            }
            return categoryId;
        }
    }
} 