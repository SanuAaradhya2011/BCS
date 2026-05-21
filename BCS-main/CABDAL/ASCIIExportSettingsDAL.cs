

using System;
using System.Collections.Generic;
using System.Data;
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
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class ASCIIExportSettingsDAL : DALBase
	{
        private string Asciiexportsettings_ID = "Asciiexportsettings_ID";
        private string FileName = "FileName";
        private string Delimeter = "Delimeter";
        private string GeneralColumn = "GeneralColumn";
        private string GeneralDBColumn = "GeneralDBColumn";
        private string BillingColumn = "BillingColumn";
        private string BillingDBColumn = "BillingDBColumn";
        private string TamperColumn = "TamperColumn";
        private string TamberDBColumn = "TamberDBColumn";
        private string InstantColumn = "InstantColumn";
        private string InstantDBColum = "InstantDBColum";
        private string LoadSurveyColumn = "LoadSurveyColumn";
        private string LoadSurveyDBColumn = "LoadSurveyDBColumn";
        private string MidnightEnergiesColumn="MidnightEnergiesColumn";
        private string MidnightEnergiesDBColumn="MidnightEnergiesDBColumn";
        private string SelfDiagnosisColumn = "SelfDiagnosticsColumn";
        private string SelfDiagnosisDBColumn = "SelfDiagnosticsDBColumn";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ASCIIExportSettingsDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            ASCIIExportSettingsEntity asciiExportSettingsEntity = null;
            if (entity == null)
                return asciiExportSettingsEntity;
            try
            {
                asciiExportSettingsEntity = entity as ASCIIExportSettingsEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into asciiexportsettings(FileName,Delimeter,GeneralColumn,GeneralDBColumn,BillingColumn,BillingDBColumn,TamperColumn,TamberDBColumn,InstantColumn,InstantDBColum,LoadSurveyColumn,LoadSurveyDBColumn,MidnightEnergiesColumn,MidnightEnergiesDBColumn,SelfDiagnosticsColumn,SelfDiagnosticsDBColumn) values(");
                builder.Append(string.Concat(ParameterName(FileName), ","));
                builder.Append(string.Concat(ParameterName(Delimeter), ","));
                builder.Append(string.Concat(ParameterName(GeneralColumn), ","));
                builder.Append(string.Concat(ParameterName(GeneralDBColumn), ","));
                builder.Append(string.Concat(ParameterName(BillingColumn), ","));
                builder.Append(string.Concat(ParameterName(BillingDBColumn), ","));
                builder.Append(string.Concat(ParameterName(TamperColumn), ","));
                builder.Append(string.Concat(ParameterName(TamberDBColumn), ","));
                builder.Append(string.Concat(ParameterName(InstantColumn), ","));
                builder.Append(string.Concat(ParameterName(InstantDBColum), ","));
                builder.Append(string.Concat(ParameterName(LoadSurveyColumn), ","));
                builder.Append(string.Concat(ParameterName(LoadSurveyDBColumn), ","));
                builder.Append(string.Concat(ParameterName(MidnightEnergiesColumn), ","));
                builder.Append(string.Concat(ParameterName(MidnightEnergiesDBColumn), ","));
                builder.Append(string.Concat(ParameterName(SelfDiagnosisColumn), ","));
                builder.Append(string.Concat(ParameterName(SelfDiagnosisDBColumn), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), asciiExportSettingsEntity.FileName, DbType.String, 50);
                request.AddParamter(ParameterName(Delimeter), asciiExportSettingsEntity.Delimeter, DbType.String, 2);
                request.AddParamter(ParameterName(GeneralColumn), asciiExportSettingsEntity.GeneralColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(GeneralDBColumn), asciiExportSettingsEntity.GeneralDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(BillingColumn), asciiExportSettingsEntity.BillingColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(BillingDBColumn), asciiExportSettingsEntity.BillingDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(TamperColumn), asciiExportSettingsEntity.TamperColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(TamberDBColumn), asciiExportSettingsEntity.TamberDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(InstantColumn), asciiExportSettingsEntity.InstantColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(InstantDBColum), asciiExportSettingsEntity.InstantDBColum, DbType.String, 50000);
                request.AddParamter(ParameterName(LoadSurveyColumn), asciiExportSettingsEntity.LoadSurveyColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(LoadSurveyDBColumn), asciiExportSettingsEntity.LoadSurveyDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(MidnightEnergiesColumn), asciiExportSettingsEntity.MidnightEnergiesColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(MidnightEnergiesDBColumn), asciiExportSettingsEntity.MidnightEnergiesDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(SelfDiagnosisColumn), asciiExportSettingsEntity.SelfDiagnosisColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(SelfDiagnosisDBColumn), asciiExportSettingsEntity.SelfDiagnosisDBColumn, DbType.String, 50000);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII export settings added");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                asciiExportSettingsEntity = null;
            }
            return asciiExportSettingsEntity;
        }
        public bool DeleteData(long settingsId)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from asciiexportsettings where ");
                builder.Append(string.Concat(Asciiexportsettings_ID, "=", ParameterName(Asciiexportsettings_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Asciiexportsettings_ID), settingsId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("ASCII export settings deleted"));
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
                builder.Append("Select count(*) from asciiexportsettings where ");
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII export settings viewed");
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
			ASCIIExportSettingsEntity asciiExportSettingsEntity = new ASCIIExportSettingsEntity();
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
                builder.Append("Select Asciiexportsettings_ID,FileName,Delimeter,GeneralColumn,GeneralDBColumn,BillingColumn,BillingDBColumn,TamperColumn,TamberDBColumn,InstantColumn,InstantDBColum,LoadSurveyColumn,LoadSurveyDBColumn,MidnightEnergiesColumn,MidnightEnergiesDBColumn, SelfDiagnosticsColumn, SelfDiagnosticsDBColumn from asciiexportsettings where ");
                builder.Append(string.Concat(Asciiexportsettings_ID, "=", ParameterName(Asciiexportsettings_ID)));
				DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Asciiexportsettings_ID), id, DbType.Int32);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0) 
					asciiExportSettingsEntity = (ASCIIExportSettingsEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII export settings viewed");
			}
            catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                asciiExportSettingsEntity = null;
			}
			return asciiExportSettingsEntity;
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
                builder.Append("Select Asciiexportsettings_ID,FileName,Delimeter,GeneralColumn,GeneralDBColumn,BillingColumn,BillingDBColumn,TamperColumn,TamberDBColumn,InstantColumn,InstantDBColum,LoadSurveyColumn,LoadSurveyDBColumn,MidnightEnergiesColumn,MidnightEnergiesDBColumn from asciiexportsettings");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII export settings viewed");
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

		public override IEntity RowToEntity(DataRow row)
		{ 
			if (row == null) return null;
			ASCIIExportSettingsEntity asciiExportSettingsEntity = new ASCIIExportSettingsEntity();
            if (NotNullAndNotDBNull(row,Asciiexportsettings_ID )) asciiExportSettingsEntity.Asciiexportsettings_ID = Convert.ToInt32(row[Asciiexportsettings_ID]);
			if (NotNullAndNotDBNull(row,FileName )) asciiExportSettingsEntity.FileName = Convert.ToString(row[FileName]);
            if (NotNullAndNotDBNull(row,Delimeter )) asciiExportSettingsEntity.Delimeter = Convert.ToString(row[Delimeter]);
            if (NotNullAndNotDBNull(row,GeneralColumn )) asciiExportSettingsEntity.GeneralColumn = Convert.ToString(row[GeneralColumn]);
            if (NotNullAndNotDBNull(row,GeneralDBColumn )) asciiExportSettingsEntity.GeneralDBColumn = Convert.ToString(row[GeneralDBColumn]);
            if (NotNullAndNotDBNull(row,BillingColumn )) asciiExportSettingsEntity.BillingColumn = Convert.ToString(row[BillingColumn]);
            if (NotNullAndNotDBNull(row, BillingDBColumn)) asciiExportSettingsEntity.BillingDBColumn = Convert.ToString(row[BillingDBColumn]);
            if (NotNullAndNotDBNull(row,TamperColumn )) asciiExportSettingsEntity.TamperColumn = Convert.ToString(row[TamperColumn]);
            if (NotNullAndNotDBNull(row,TamberDBColumn )) asciiExportSettingsEntity.TamberDBColumn = Convert.ToString(row[TamberDBColumn]);
            if (NotNullAndNotDBNull(row,InstantColumn )) asciiExportSettingsEntity.InstantColumn = Convert.ToString(row[InstantColumn]);
            if (NotNullAndNotDBNull(row,InstantDBColum )) asciiExportSettingsEntity.InstantDBColum = Convert.ToString(row[InstantDBColum]);
            if (NotNullAndNotDBNull(row, LoadSurveyColumn)) asciiExportSettingsEntity.LoadSurveyColumn = Convert.ToString(row[LoadSurveyColumn]);
            if (NotNullAndNotDBNull(row,LoadSurveyDBColumn )) asciiExportSettingsEntity.LoadSurveyDBColumn = Convert.ToString(row[LoadSurveyDBColumn]);
            if (NotNullAndNotDBNull(row, MidnightEnergiesColumn)) asciiExportSettingsEntity.MidnightEnergiesColumn = Convert.ToString(row[MidnightEnergiesColumn]);
            if (NotNullAndNotDBNull(row, MidnightEnergiesDBColumn)) asciiExportSettingsEntity.MidnightEnergiesDBColumn = Convert.ToString(row[MidnightEnergiesDBColumn]);
            if (NotNullAndNotDBNull(row, SelfDiagnosisColumn)) asciiExportSettingsEntity.SelfDiagnosisColumn = Convert.ToString(row[SelfDiagnosisColumn]);
            if (NotNullAndNotDBNull(row, SelfDiagnosisDBColumn)) asciiExportSettingsEntity.SelfDiagnosisDBColumn = Convert.ToString(row[SelfDiagnosisDBColumn]); 
			return asciiExportSettingsEntity;
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
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII data exported");
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
                ASCIIExportSettingsEntity asciiExportSettingsEntity = entity as ASCIIExportSettingsEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update asciiexportsettings set ");
                builder.Append(string.Concat(FileName,"=",ParameterName(FileName), ","));
                builder.Append(string.Concat( Delimeter,"=",ParameterName(Delimeter), ","));
                builder.Append(string.Concat(GeneralColumn, "=", ParameterName(GeneralColumn), ","));
                builder.Append(string.Concat(GeneralDBColumn, "=", ParameterName(GeneralDBColumn), ","));
                builder.Append(string.Concat(BillingColumn, "=", ParameterName(BillingColumn), ","));
                builder.Append(string.Concat(BillingDBColumn, "=", ParameterName(BillingDBColumn), ","));
                builder.Append(string.Concat(TamperColumn, "=", ParameterName(TamperColumn), ","));
                builder.Append(string.Concat(TamberDBColumn, "=", ParameterName(TamberDBColumn), ","));
                builder.Append(string.Concat(InstantColumn, "=", ParameterName(InstantColumn), ","));
                builder.Append(string.Concat(InstantDBColum, "=", ParameterName(InstantDBColum), ","));
                builder.Append(string.Concat(LoadSurveyColumn, "=", ParameterName(LoadSurveyColumn), ","));
                builder.Append(string.Concat(LoadSurveyDBColumn, "=", ParameterName(LoadSurveyDBColumn), ","));
                builder.Append(string.Concat(MidnightEnergiesColumn, "=", ParameterName(MidnightEnergiesColumn), ","));
                builder.Append(string.Concat(MidnightEnergiesDBColumn, "=", ParameterName(MidnightEnergiesDBColumn), ","));
                builder.Append(string.Concat(SelfDiagnosisColumn, "=", ParameterName(SelfDiagnosisColumn), ","));
                builder.Append(string.Concat(SelfDiagnosisDBColumn, "=", ParameterName(SelfDiagnosisDBColumn), " where "));

                builder.Append(string.Concat(Asciiexportsettings_ID, "=", ParameterName(Asciiexportsettings_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), asciiExportSettingsEntity.FileName, DbType.String, 50);
                request.AddParamter(ParameterName(Delimeter), asciiExportSettingsEntity.Delimeter, DbType.String, 2);
                request.AddParamter(ParameterName(GeneralColumn), asciiExportSettingsEntity.GeneralColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(GeneralDBColumn), asciiExportSettingsEntity.GeneralDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(BillingColumn), asciiExportSettingsEntity.BillingColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(BillingDBColumn), asciiExportSettingsEntity.BillingDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(TamperColumn), asciiExportSettingsEntity.TamperColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(TamberDBColumn), asciiExportSettingsEntity.TamberDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(InstantColumn), asciiExportSettingsEntity.InstantColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(InstantDBColum), asciiExportSettingsEntity.InstantDBColum, DbType.String, 50000);
                request.AddParamter(ParameterName(LoadSurveyColumn), asciiExportSettingsEntity.LoadSurveyColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(LoadSurveyDBColumn), asciiExportSettingsEntity.LoadSurveyDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(MidnightEnergiesColumn), asciiExportSettingsEntity.MidnightEnergiesColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(MidnightEnergiesDBColumn), asciiExportSettingsEntity.MidnightEnergiesDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(SelfDiagnosisColumn), asciiExportSettingsEntity.SelfDiagnosisColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(SelfDiagnosisDBColumn), asciiExportSettingsEntity.SelfDiagnosisDBColumn, DbType.String, 50000);
                request.AddParamter(ParameterName(Asciiexportsettings_ID), asciiExportSettingsEntity.Asciiexportsettings_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog("ASCII export settings modified");
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

