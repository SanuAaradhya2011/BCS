 

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
    public class ConsumerExportSettingsDAL : DALBase
	{
		private string ConsumerExportSettings_ID = "ConsumerExportSettings_ID";
		private string FileName = "FileName";
		private string ParametersName = "ParametersName";
		private string ParameterColumn = "ParameterColumn";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ConsumerExportSettingsDAL).ToString());

        public override IEntity InsertData(IEntity entity)
		{
            ConsumerExportSettingsEntity consumerExportSettingsEntity = null;
            if (entity == null)
                return consumerExportSettingsEntity; 
			try
			{
				  consumerExportSettingsEntity = entity as ConsumerExportSettingsEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into ConsumerExportSettings(FileName,ParametersName,ParameterColumn) values(");
				builder.Append(string.Concat(ParameterName(FileName), ","));
				builder.Append(string.Concat(ParameterName(ParametersName), ","));
				builder.Append(string.Concat(ParameterName(ParameterColumn), ")"));
				DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), consumerExportSettingsEntity.FileName, DbType.String, 50);
				request.AddParamter(ParameterName(ParametersName), consumerExportSettingsEntity.ParametersName, DbType.String,1000);
                request.AddParamter(ParameterName(ParameterColumn), consumerExportSettingsEntity.ParameterColumn, DbType.String, 1500);
				helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer export settings added");
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                consumerExportSettingsEntity = null;
			}
            return consumerExportSettingsEntity;
		}
        public bool DeleteData(long settingsId)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from ConsumerExportSettings where ");
                builder.Append(string.Concat(ConsumerExportSettings_ID, "=", ParameterName(ConsumerExportSettings_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerExportSettings_ID), settingsId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer export settings deleted."));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long settingsId)", ex);
            }
            return Flag;
        }
		public override bool DeleteData(IEntity entity)
		{
			throw new NotImplementedException();
		}
        public bool ValidateFile(string fileName)
        { 
            bool flag=false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from ConsumerExportSettings where ");
                builder.Append(string.Concat(FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String,50);
                string data =Convert.ToString( helper.ExecuteScalar(request));
                if (string.IsNullOrEmpty(data))
                    flag = false;
                else
                {
                    int val = Convert.ToInt32(data);
                    if (val != 0)
                        flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer export settings viewed");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateFile(string fileName)", ex);
                flag = false;
            }
            return flag;
        }
		public override IEntity GetDetailData(int id)
		{ 
			ConsumerExportSettingsEntity consumerExportSettingsEntity = new ConsumerExportSettingsEntity();
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select ConsumerExportSettings_ID,FileName,ParametersName,ParameterColumn from ConsumerExportSettings where ");
                builder.Append(string.Concat(ConsumerExportSettings_ID, "=", ParameterName(ConsumerExportSettings_ID)));
				DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ConsumerExportSettings_ID), id, DbType.Int32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0) 
					consumerExportSettingsEntity = (ConsumerExportSettingsEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer export settings viewed");
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                consumerExportSettingsEntity = null;
			}
			return consumerExportSettingsEntity;
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
                builder.Append("Select ConsumerExportSettings_ID,FileName,ParametersName,ParameterColumn from ConsumerExportSettings");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer export settings viewed");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

		public override IEntity RowToEntity(DataRow row)
		{ 
			if (row == null) return null;
			ConsumerExportSettingsEntity consumerExportSettingsEntity = new ConsumerExportSettingsEntity();
            if (NotNullAndNotDBNull(row, ConsumerExportSettings_ID)) consumerExportSettingsEntity.ConsumerExportSettings_ID = Convert.ToInt32(row[ConsumerExportSettings_ID]);
			if (NotNullAndNotDBNull(row, FileName)) consumerExportSettingsEntity.FileName = Convert.ToString(row[FileName]);
            if (NotNullAndNotDBNull(row, ParametersName)) consumerExportSettingsEntity.ParametersName = Convert.ToString(row[ParametersName]);
            if (NotNullAndNotDBNull(row, ParameterColumn)) consumerExportSettingsEntity.ParameterColumn = Convert.ToString(row[ParameterColumn]);
			return consumerExportSettingsEntity;
		}
 
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DataSet GetParameterData(string qry)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest(qry);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer data exported");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetParameterData(string qry)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override bool UpdateData(IEntity entity)
        {
             bool flag=false;
            if (entity == null)
                return flag;
            try
            {
                ConsumerExportSettingsEntity consumerExportSettingsEntity = entity as ConsumerExportSettingsEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update ConsumerExportSettings set ");
                builder.Append(string.Concat(FileName,"=",ParameterName(FileName), ","));
                builder.Append(string.Concat(ParametersName ,"=",ParameterName(ParametersName), ","));
                builder.Append(string.Concat(ParameterColumn ,"=", ParameterName(ParameterColumn), " where "));
                builder.Append(string.Concat(ConsumerExportSettings_ID, "=", ParameterName(ConsumerExportSettings_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), consumerExportSettingsEntity.FileName, DbType.String, 50);
                request.AddParamter(ParameterName(ParametersName), consumerExportSettingsEntity.ParametersName, DbType.String, 1000);
                request.AddParamter(ParameterName(ParameterColumn), consumerExportSettingsEntity.ParameterColumn, DbType.String, 1500);
                request.AddParamter(ParameterName(ConsumerExportSettings_ID), consumerExportSettingsEntity.ConsumerExportSettings_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("Consumer export settings modified");
                flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                flag = false;
            }
            return flag;
        }
    }
}
