/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class CMRIMasterDAL : DALBase
    {
        private string CMRI_ID = "CMRI_ID";
        private string CMRI_Number = "CMRI_Number";
        private string CMRI_Description = "CMRI_Description";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CMRIMasterDAL).ToString());


        public override IEntity InsertData(IEntity entity)
        {
            CMRIMasterEntity cmriMasterEntity = null;
            if (entity == null)
                return cmriMasterEntity;
            cmriMasterEntity = entity as CMRIMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into CMRI_Master(CMRI_Number,CMRI_Description) values(");
                builder.Append(string.Concat(ParameterName(CMRI_Number), ","));
                builder.Append(string.Concat(ParameterName(CMRI_Description), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriMasterEntity.CMRI_Number, DbType.String, 35);
                request.AddParamter(ParameterName(CMRI_Description), cmriMasterEntity.CMRI_Description, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity.CMRI_Number, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                cmriMasterEntity = null;
            }
            return cmriMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                CMRIMasterEntity cmriMasterEntity = entity as CMRIMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update CMRI_Master Set ");
                builder.Append(string.Concat(CMRI_Description, "=", ParameterName(CMRI_Description)));

                builder.Append(string.Concat(" Where ", CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), cmriMasterEntity.CMRI_ID, DbType.Int64);
                request.AddParamter(ParameterName(CMRI_Description), cmriMasterEntity.CMRI_Description, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity.CMRI_Number, " modified"));
                Flag = true;
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
                CMRIMasterEntity cmriMasterEntity = entity as CMRIMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From CMRI_Master ");
                builder.Append(string.Concat(" Where ", CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), cmriMasterEntity.CMRI_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI ", cmriMasterEntity.CMRI_Number, " deleted"));
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
            CMRIMasterEntity cmriMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CMRI_ID,CMRI_Number,CMRI_Description from CMRI_Master where ");
                builder.Append(string.Concat(CMRI_ID, "=", ParameterName(CMRI_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    cmriMasterEntity = (CMRIMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI list viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                cmriMasterEntity = null;
            }
            return cmriMasterEntity;
        }

        public bool GetDetailData(string cmriNumber)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CMRI_ID,CMRI_Number,CMRI_Description from CMRI_Master where ");
                builder.Append(string.Concat(CMRI_Number, "=", ParameterName(CMRI_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriNumber, DbType.String, 16);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Details of a specified CMRI viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string cmriNumber)", ex);
            }
            return Flag;
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
                builder.Append("Select CMRI_ID,CMRI_Number as 'CMRI Number',CMRI_Description as 'CMRI Description' from CMRI_Master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI list viewed"));
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
            CMRIMasterEntity cmriMasterEntity = new CMRIMasterEntity();
            if (NotNullAndNotDBNull(row, CMRI_ID)) cmriMasterEntity.CMRI_ID = Convert.ToInt64(row[CMRI_ID]);
            if (NotNullAndNotDBNull(row, CMRI_Number)) cmriMasterEntity.CMRI_Number = Convert.ToString(row[CMRI_Number]);
            if (NotNullAndNotDBNull(row, CMRI_Description)) cmriMasterEntity.CMRI_Description = Convert.ToString(row[CMRI_Description]);
            return cmriMasterEntity;
        }



        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public DataSet ComboList()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                builder.Append("SELECT distinct cmri.CMRI_Number FROM CMRI_Master cmri JOIN areamaster area ON cmri.cmri_id = area.cmri_id JOIN areameter_master armeter ON area.Area_ID = armeter.Area_ID JOIN meterdata md ON armeter.Meter_ID = md.MeterID JOIN fileupload_master fpm ON md.FileUpload_ID = fpm.FileUpload_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IList<IEntity> entities)", ex);
            }
            return dataSet;
        }

        public DataSet ListDataSet(string data)
        {
            DataSet listDataSet = new DataSet();
            try
            {
                DataSet dsAuto = autoAssociatedListDataSet(data);
                DataSet dsManual = manualAssociatedListDataSet(data);

                dsAuto.Merge(dsManual);


                DataTable temp = dsAuto.Tables[0].DefaultView.ToTable(true, "MeterData_ID", "Meter Number", "FileName", "Reading DateTime", "FileUpload_ID", "Uploading DateTime", "CommType", "File Size");
                listDataSet.Tables.Add(AutoNumberedTable(temp));

                if (listDataSet == null || listDataSet.Tables.Count == 0 || listDataSet.Tables[0].Rows.Count == 0)
                    return null;
                
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("CMRI data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string data)", ex);
            }
            return listDataSet;
        }

        /// <summary>
        /// Fetching list meter data associated with CMRI manually
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataSet manualAssociatedListDataSet(string data)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            DataRequest request = null;

            builder.Append("SELECT md.MeterData_ID,md.MeterID as 'Meter Number',fpm.FileName,md.ReadingDateTime as 'Reading DateTime',fpm.FileUpload_ID,md.UploadingDateTime as 'Uploading DateTime',fpm.CommType,fpm.FileSize as 'File Size' FROM CMRI_Master cmri JOIN areamaster area ON cmri.cmri_id = area.cmri_id JOIN areameter_master armeter ON area.Area_ID = armeter.Area_ID JOIN meterdata md ON armeter.Meter_ID = md.MeterID JOIN fileupload_master fpm ON md.FileUpload_ID = fpm.FileUpload_ID WHERE cmri.");

            builder.Append(string.Concat(CMRI_Number, "=", ParameterName(CMRI_Number)));

            request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(CMRI_Number), data, DbType.String);

            DataSet dataSet = new DataSet();

            dataSet = helper.FillDataSet(request, dataSet);

            return dataSet;
        }

        /// <summary>
        /// Fetching list meter data associated with CMRI automatically
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataSet autoAssociatedListDataSet(string data)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            DataRequest request = null;

            builder.Append("SELECT md.MeterData_ID,md.MeterID as 'Meter Number',fpm.FileName,md.ReadingDateTime as 'Reading DateTime',fpm.FileUpload_ID,md.UploadingDateTime as 'Uploading DateTime',fpm.CommType,fpm.FileSize as 'File Size' FROM  meterdata md JOIN fileupload_master fpm ON md.fileupload_ID = fpm.fileupload_ID WHERE md.");

            builder.Append(string.Concat(CMRI_Number, "=", ParameterName(CMRI_Number)));

            request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(CMRI_Number), data, DbType.String);

            DataSet dataSet = new DataSet();

            dataSet = helper.FillDataSet(request, dataSet);

            return dataSet;
        }
    }
}
