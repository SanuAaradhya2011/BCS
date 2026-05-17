using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class MeterDataDAL : DALBase
    {
        private string MeterData_ID = "MeterData_ID";
        private string FileUpload_ID = "FileUpload_ID";
        private string SubGroup_ID = "SubGroup_ID";
        private string MeterID = "MeterID";
        private string Area_ID = "Area_ID";
        private string ReadingDateTime = "ReadingDateTime";
        private string UploadingDateTime = "UploadingDateTime";
        private string Meter_ID = "Meter_ID";
        private string CMRI_Number = "CMRI_Number";
        private bool isPUMA = false;

        private string FileName = "FileName";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterDataDAL).ToString());
        public MeterDataDAL()
            : base("MeterData", "MeterData_ID")
        {
        }
        public MeterDataDAL(bool ispuma)
            : base("MeterData", "MeterData_ID")
        {
            this.isPUMA = ispuma;
        }
        public DataSet ComboList(string value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct " + value + " from MeterData Where " + value + "!=''");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ComboList(string value)", ex);
            }
            return dataSet;
        }
        /// <summary>
        /// gets meterIDs ,where uploaded data for loadsurvey is not null for metereID.
        /// </summary>
        /// <param name="isDate"></param>
        /// <returns></returns>
        public DataSet GetMeterIDLoadSurvey(bool isDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                if (isDate)
                    builder.Append("SELECT distinct mdata.MeterId from MeterData mdata , meterdata_loadsurvey survey where mdata.meterData_ID = survey.meterData_ID ");
                else
                    builder.Append("SELECT distinct mdata.ReadingDateTime from MeterData mdata , meterdata_loadsurvey survey where mdata.meterData_ID = survey.meterData_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIDLoadSurvey(bool isDate)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// gets meterIDs ,where uploaded data for midnight is not null for metereID.
        /// </summary>
        /// <param name="isDate"></param>
        /// <returns></returns>
        public DataSet GetMeterIDMidnight(bool isDate)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                if (isDate)
                    builder.Append("SELECT distinct mdata.MeterId from MeterData mdata , meterdata_midnightdata middata where mdata.meterData_ID = middata.meterData_ID ");
                else
                    builder.Append("SELECT distinct mdata.ReadingDateTime from MeterData mdata , meterdata_midnightdata middata where mdata.meterData_ID = middata.meterData_ID");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIDMidnight(bool isDate)", ex);
            }
            return dataSet;
        }

        ///// <summary>
        ///// Updates the field CommTypes in fileupload_master
        ///// </summary>
        ///// <param name="fileUpload_ID"></param>
        ///// <param name="value"></param>
        //public void UpdateCommType(long fileUpload_ID, long value)
        //{
        //    IDataHelper helper = DatabaseFactory.GetHelper();
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("update fileupload_master set CommType=@value where FileUpload_ID=");
        //    builder.Append(fileUpload_ID);
        //    DataRequest request = new DataRequest(builder.ToString());
        //    request.AddParamter(ParameterName("@value"), value, DbType.Int64);
        //    helper.ExecuteNonQuery(request);
        //    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data Updated")); 
        //}

        public override IEntity InsertData(IEntity entity)
        {
            MeterDataEntity meterDataEntity = entity as MeterDataEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData(FileUpload_ID,MeterID,ReadingDateTime,UploadingDateTime,CMRI_Number) values(");
                builder.Append(string.Concat(ParameterName(FileUpload_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(ReadingDateTime), ","));
                builder.Append(string.Concat(ParameterName(UploadingDateTime), ","));
                builder.Append(string.Concat(ParameterName(CMRI_Number), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), meterDataEntity.FileUpload_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterID), meterDataEntity.MeterID, DbType.String, 50);
                request.AddParamter(ParameterName(ReadingDateTime), meterDataEntity.ReadingDateTime, DbType.Int64);
                request.AddParamter(ParameterName(UploadingDateTime), meterDataEntity.UploadingDateTime, DbType.Int64);
                request.AddParamter(ParameterName(CMRI_Number), meterDataEntity.CMRIID, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data added"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            if (Flag)
                meterDataEntity.MeterData_ID = long.Parse(this.GetPK());
            return meterDataEntity;
        }

        public bool DeleteDataBasedOnFileID(long fileUploadId)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteDataBasedOnFileID(long fileUploadId)", ex);
            }
            return Flag;
        }
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }
        // Added to solve bug 89140
        public bool DeleteDataFastDownload(long fileUploadID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteDataFastDownload(long fileUploadID)", ex);
            }
            return Flag;
        }
        /// <summary>
        /// Get File Name
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public string GetFileType(long fileUploadId)
        {
            string fileName = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileType from fileupload_master Where ");
                builder.Append(string.Concat("FileUpload_ID=", ParameterName("FileUpload_ID")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("FileUpload_ID"), fileUploadId, DbType.Int64);
                fileName = helper.ExecuteScalar(request).ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Name Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileType(long fileUploadId)", ex);
                fileName = string.Empty;
            }
            return fileName;
        }
        /// <summary>
        ///  Get File Type by meterID
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public string GetFileTypeByMeterId(string meterId)
        {
            string fileName = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileType from fileupload_master FM inner join meterdata M Where FM.FileUpload_ID = M.FileUpload_ID AND ");
                builder.Append(string.Concat("M.MeterId=", ParameterName("MeterId")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterId"), meterId, DbType.String);
                fileName = helper.ExecuteScalar(request).ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Type Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileTypeByMeterId(string meterId)", ex);
                fileName = string.Empty;
            }
            return fileName;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            MeterDataEntity meterDataEntity = entity as MeterDataEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataEntity.MeterData_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
            }
            return Flag;
        }

        public long GetMeterDataID(long fileUploadID)
        {
            long meterid = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID  from MeterData where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadID, DbType.Int64);
                meterid = Convert.ToInt64(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataID(long fileUploadID)", ex);
                meterid = 0;
            }
            return meterid;
        }
        public DataSet GetMeterDataSetID(long fileUploadID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID  from MeterData where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataSetID(long fileUploadID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public IEntity GetDetailData(long id)
        {
            MeterDataEntity meterDataEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID,FileUpload_ID,MeterID,ReadingDateTime,UploadingDateTime from MeterData where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            meterDataEntity = (MeterDataEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(long id)", ex);
            }
            return meterDataEntity;
        }
        public IEntity GetDetailDataUploadId(long fileUploadId)
        {
            MeterDataEntity meterDataEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID,FileUpload_ID,MeterID,ReadingDateTime,UploadingDateTime from MeterData where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadId, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            meterDataEntity = (MeterDataEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailDataUploadId(long fileUploadId)", ex);
            }
            return meterDataEntity;
        }
        public IEntity GetDetailData(string meterID, long fileUpload_ID, long readingDateTime)
        {
            MeterDataEntity meterDataEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID,FileUpload_ID,MeterID,ReadingDateTime,UploadingDateTime from MeterData where ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" and ", FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                builder.Append(string.Concat(" and ", ReadingDateTime, "=", ParameterName(ReadingDateTime)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 50);
                request.AddParamter(ParameterName(FileUpload_ID), fileUpload_ID, DbType.Int64);
                request.AddParamter(ParameterName(ReadingDateTime), readingDateTime, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            meterDataEntity = (MeterDataEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string meterID, long fileUpload_ID, long readingDateTime)", ex);
            }
            return meterDataEntity;
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
                builder.Append("Select MeterData_ID,FileUpload_ID,MeterID,ReadingDateTime from MeterData ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
            }
            return dataSet;
        }

        public DataSet GetAllMeterID()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID from Meter_Master where meter_status=1");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllMeterID()", ex);
            }
            return dataSet;
        }

        public DataSet GetAllMeterIDValues()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select meterID from meterdata union select Meter_ID from consumermeter where Meter_ID is not Null and Status <> 0");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllMeterIDValues()", ex);
            }
            return dataSet;
        }

        public DataSet GetUnAssignedMeterID(int subGroupID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Distinct MeterID as meterID from MeterData union select Meter_ID as meterID from consumermeter where status = 1 and Meter_ID is not Null and Meter_ID not in (select Meter_ID from subGroupMeter_Master where ");
                builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(SubGroup_ID), subGroupID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetUnAssignedMeterID(int subGroupID)", ex);
            }
            return dataSet;
        }

        //Piyush Singh Included on 28 April 2010 for Getting the meterDataId  for the File Name
        public bool ValidateData(long fileUploadID, string meterID, long readingDateTime)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("Select MeterData_ID from MeterData where ");
            builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
            builder.Append(string.Concat(" and ", MeterID, "=", ParameterName(MeterID)));
            builder.Append(string.Concat(" and ", ReadingDateTime, "=", ParameterName(ReadingDateTime)));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(FileUpload_ID), fileUploadID, DbType.Int64);
            request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 50);
            request.AddParamter(ParameterName(ReadingDateTime), readingDateTime, DbType.Int64);
            DataSet dataSet = new DataSet();
            dataSet = helper.FillDataSet(request, dataSet);
            if (dataSet == null)
                return false;
            if (dataSet.Tables[0].Rows.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// This method is used to Get the list of Meter data along with file name.
        /// </summary>
        /// <returns></returns>
        public DataSet GetList()
        {
            DataSet tmpDataSet = new DataSet();
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("select A.FileName,B.MeterID,B.MeterData_ID from fileupload_master A  inner join meterdata B on A.FileUpload_ID = B.FileUpload_ID");
            DataRequest request = new DataRequest(builder.ToString());
            DataSet dataSet = new DataSet();
            dataSet = helper.FillDataSet(request, dataSet);
            tmpDataSet.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File upload data viewed"));
            return tmpDataSet;
        }

        public DataSet ListDataSet(string readingType, string value, bool distinct)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                if (readingType.Equals("CABF"))
                {
                    if (!distinct)
                    {
                        builder.Append("Select A.MeterData_ID,A.MeterID as 'Meter Number',A.ReadingDateTime as 'Reading DateTime', B.FileUpload_ID, B.CommType,A.UploadingDateTime as 'Uploading DateTime',B.FileSize as 'File Size' from MeterData A,");
                        builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and B.");
                        builder.Append(string.Concat(FileName, "=", ParameterName(FileName), " order by A.ReadingDateTime asc"));
                    }
                    else
                    {
                        builder.Append("Select distinct A.MeterData_ID, A.MeterID as 'Meter Number',A.ReadingDateTime as 'Reading DateTime'. B.FileUpload_ID, B.CommType,A.UploadingDateTime as 'Uploading DateTime',B.FileSize as 'File Size' from MeterData A,");
                        builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and B.");
                        builder.Append(string.Concat(FileName, "=", ParameterName(FileName), " group by A.MeterID"));
                    }
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(FileName), value, DbType.String, 40);
                }
                if (readingType.Equals("MSN"))
                {
                    ////builder.Append("Select distinct A.MeterData_ID,B.FileName as 'File Name' from MeterData A,");
                    ////builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and A.");
                    ////builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                    ////request = new DataRequest(builder.ToString());
                    ////request.AddParamter(ParameterName(MeterID), value, DbType.String, 40);
                    if (!distinct)
                    {
                        builder.Append("Select A.MeterData_ID,B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime',B.FileUpload_ID, B.CommType,A.UploadingDateTime as 'Uploading DateTime',B.FileSize as 'File Size' from MeterData A,");
                        builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and A.");
                        builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " order by A.ReadingDateTime asc"));
                    }
                    else
                    {
                        builder.Append("Select A.MeterData_ID,B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime',B.FileUpload_ID, B.CommType,A.UploadingDateTime as 'Uploading DateTime',B.FileSize as 'File Size' from MeterData A,");
                        builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and A.");
                        builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID), " group by B.FileName"));
                    }
                    request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterID), value, DbType.String, 40);
                }
                if (readingType.Equals("RD"))
                {
                    builder.Append("Select A.MeterData_ID,A.MeterID as 'Meter Number',B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime',B.FileUpload_ID, B.CommType,A.UploadingDateTime as 'Uploading DateTime',B.FileSize as 'File Size' from MeterData A,");
                    builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID  and A.");
                    builder.Append(string.Concat(ReadingDateTime, ">=", ParameterName("ReadingDateTimeStart")));
                    builder.Append(string.Concat(" and A.", ReadingDateTime, "<=", ParameterName("ReadingDateTimeEnd"), " order by A.ReadingDateTime asc"));
                    request = new DataRequest(builder.ToString());
                    long startDate = DateUtility.DateToLong(value, true);
                    long endDate = DateUtility.DateToLong(value, false);
                    request.AddParamter(ParameterName("ReadingDateTimeStart"), startDate, DbType.Int64);
                    request.AddParamter(ParameterName("ReadingDateTimeEnd"), endDate, DbType.Int64);
                }
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                if (dataSet == null)
                    return null;
                if (dataSet.Tables.Count == 0)
                    return null;
                if (dataSet.Tables[0].Rows.Count == 0)
                    return null;
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string readingType, string value, bool distinct)", ex);
            }
            return slData;
        }

        public DataSet ListExportDataSet(string fileName)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("Select A.MeterData_ID,A.MeterID as 'Meter Number',B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime' from MeterData A,");
                builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID and B.FileName='");
                builder.Append(fileName);
                builder.Append("'");
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListExportDataSet(string fileName)", ex);
            }
            return slData;
        }


        public DataSet ListReportExportDataSet(string meterID)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("select distinct a.FileUpload_ID,a.FileName from meterdata b inner join fileupload_master a on a.FileUpload_ID = b.FileUpload_ID and ");
                builder.Append(string.Concat("b.", MeterID, " = ", ParameterName(MeterID), " order by b.uploadingdatetime desc"));
                request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListReportExportDataSet(string meterID)", ex);
            }
            return slData;
        }


        public DataSet ListExportDataSet()
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("Select A.MeterData_ID,A.MeterID as 'Meter Number',B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime' from MeterData A,");
                builder.Append("fileupload_master B where A.FileUpload_ID = B.FileUpload_ID");
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListExportDataSet()", ex);
            }
            return slData;
        }
        public DataSet AsciiPUMAListExportDataSet(string meterType)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("Select A.MeterData_ID,A.MeterID as 'Meter Number',B.FileName as 'File Name',A.ReadingDateTime as 'Reading DateTime' from MeterData A ");
                builder.Append(" inner join fileupload_master B on A.FileUpload_Id = B.FileUpload_Id");
                builder.Append(" inner Join meterdata_general C on C.MeterData_Id = A.MeterData_Id");
                builder.Append(" where C.MeterDataType = '" + meterType + "'");
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " AsciiPUMAListExportDataSet(string meterType)", ex);
            }
            return slData;
        }
        public DataSet FileExportListDataSet(bool isTextFileExport)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                if (isTextFileExport)
                {
                    //For MSEDCL , file need to have general instant and billing data .
                    builder.Append("Select distinct f.FileName as 'File Name',f.UploadingDateTime as 'Uploading DateTime' from fileupload_master f ");
                    //builder.Append("join meterdata m join meterdata_general g join meterdata_instantpower i join meterdata_billing b ");
                    builder.Append("join meterdata m join meterdata_general g  join meterdata_billing b ");
                    builder.Append("where f.FileUpload_ID= m.FileUpload_ID and m.MeterData_ID= g.MeterData_ID ");
                    // builder.Append("and m.MeterData_ID= i.MeterData_ID and m.MeterData_ID= b.MeterData_ID;");
                    builder.Append("and m.MeterData_ID= b.MeterData_ID;");
                }
                else
                {
                    builder.Append("Select distinct FileName as 'File Name',UploadingDateTime as 'Uploading DateTime' from fileupload_master");
                }
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FileExportListDataSet(bool isTextFileExport)", ex);
            }
            return slData;
        }

        // SB Change Start 20170915
        public DataSet FileExportListFilteredDataSet(bool isTextFileExport, long lStartDate, long lEndDate)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                if (isTextFileExport)
                {
                    //For MSEDCL , file need to have general instant and billing data .
                    builder.Append("Select distinct f.FileName as 'File Name',f.UploadingDateTime as 'Uploading DateTime' from fileupload_master f ");
                    //builder.Append("join meterdata m join meterdata_general g join meterdata_instantpower i join meterdata_billing b ");
                    builder.Append("join meterdata m join meterdata_general g  join meterdata_billing b ");
                    builder.Append("where f.FileUpload_ID= m.FileUpload_ID and m.MeterData_ID= g.MeterData_ID ");
                    // builder.Append("and m.MeterData_ID= i.MeterData_ID and m.MeterData_ID= b.MeterData_ID;");
                    builder.Append("and m.MeterData_ID= b.MeterData_ID ");
                    builder.Append(" and f.UploadingDateTime >= " + lStartDate.ToString() + " and f.UploadingDateTime <= " + lEndDate .ToString() + ";");
                }
                else
                {
                    builder.Append("Select distinct FileName as 'File Name',UploadingDateTime as 'Uploading DateTime' from fileupload_master");
                }
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                if (dataSet.Tables.Count > 0)
                {
                    slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FileExportListFilteredDataSet(bool isTextFileExport, long lStartDate, long lEndDate)", ex);
            }
            return slData;
        }
        // SB Change End 20170915

        // Add filter for pudducerry
        public DataSet PedFileExportListDataSet(bool isTextFileExport)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                if (isTextFileExport)
                {
                    //For MSEDCL , file need to have general instant and billing data .
                    builder.Append("Select distinct m.MeterData_ID as 'Tag ID', m.MeterID as 'Meter ID', m.UploadingDateTime as 'Uploading DateTime' from meterdata m, meterdata_instantpower b where b.MeterData_ID =m.MeterData_ID and b.InstantPowerColumnName = 'Cumulative Energy History 1';");
                    //builder.Append("join meterdata m join meterdata_general g join meterdata_instantpower i join meterdata_billing b ");
                   // builder.Append("join meterdata_instantpower b  where ");
                    //builder.Append("where  m.MeterData_ID= g.MeterData_ID ");
                    // builder.Append("and m.MeterData_ID= i.MeterData_ID and m.MeterData_ID= b.MeterData_ID;");
                   


                    /*
                     *   builder.Append("Select MeterData_ID as 'Tag ID', MeterID as 'Meter ID', UploadingDateTime as 'Uploading DateTime' from meterdata where MeterData_ID = (");
                    //builder.Append("join meterdata m join meterdata_general g join meterdata_instantpower i join meterdata_billing b ");
                    builder.Append("select MeterData_ID from  meterdata_instantpower InstantPowerColumnName = 'Cumulative Energy History 1')");
                    //builder.Append("join meterdata m join meterdata_instantpower g ");
                    //builder.Append("where f.FileUpload_ID= m.FileUpload_ID and m.MeterData_ID= g.MeterData_ID ");
                    // builder.Append("and m.MeterData_ID= i.MeterData_ID and m.MeterData_ID= b.MeterData_ID;");
                    //builder.Append("and m.MeterData_ID= b.MeterData_ID;");
                        */
                }
                else
                {
                    builder.Append("Select distinct FileName as 'File Name',UploadingDateTime as 'Uploading DateTime' from fileupload_master");
                }
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "PedFileExportListDataSet(bool isTextFileExport)", ex);
            }
            return slData;
        }


        public DataSet AsciiPUMAFileExportListDataSet(string meterType)
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                builder.Append("Select distinct A.FileName as 'File Name',A.UploadingDateTime as 'Uploading DateTime' from fileupload_master A");
                builder.Append(",meterdata M,meterdata_general G where A.FileUpload_ID = M.FileUpload_ID and M.MeterData_ID=G.MeterData_ID and MeterDataType = '" + meterType + "'");
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AsciiPUMAFileExportListDataSet(string meterType)", ex);
            }
            return slData;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            MeterDataEntity meterDataEntity = new MeterDataEntity();
            if (NotNullAndNotDBNull(row, MeterData_ID)) meterDataEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, FileUpload_ID)) meterDataEntity.FileUpload_ID = Convert.ToInt64(row[FileUpload_ID]);
            if (NotNullAndNotDBNull(row, MeterID)) meterDataEntity.MeterID = Convert.ToString(row[MeterID]);
            if (NotNullAndNotDBNull(row, ReadingDateTime)) meterDataEntity.ReadingDateTime = Convert.ToInt64(row[ReadingDateTime]);
            if (NotNullAndNotDBNull(row, UploadingDateTime)) meterDataEntity.UploadingDateTime = Convert.ToInt64(row[UploadingDateTime]);
            return meterDataEntity;
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        //added by dhirendra singh on 15 may 2010 for getting consumer,meter details when meterdata_id is given
        public DataSet GetConsumerMeterDetails(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select ");
                builder.Append("md.MeterID,c.Consumer_Number, cm.Meter_AllocationDate, cm.Meter_Location,m.Meter_ContractDemand,");
                builder.Append("m.Meter_EMF, rm.Region_Name, crm.Circle_Name,cmri.CMRI_NUMBER,dm.Division_Name, cm.Status,");
                builder.Append("m.UseEMFinCalculations,");
                //BhardwajG : Remove utility check from here will be handled in UI.
                //if (isPUMA)
                // {
                builder.Append("G.internalCTRatio,G.internalPTRatio,");
                //}
                builder.Append("mtm.MeterType_Name,mmm.MeterModel_Name,md.ReadingDateTime,md.UploadingDateTime  ");
                builder.Append("from meterdata md left outer join meter_master m on md.MeterID = m.Meter_ID ");
                builder.Append("left outer join consumermeter cm on m.Meter_ID = cm.Meter_ID ");
                builder.Append("left outer join consumer_master c on cm.Consumer_Number = c.Consumer_Number ");
                builder.Append("left outer join region_master rm on rm.Region_ID = cm.Region_ID ");
                builder.Append("left outer join division_master dm on dm.Division_ID = cm.Division_ID ");
                builder.Append("left outer join circle_master crm on crm.Circle_ID = cm.Circle_ID ");
                builder.Append("left outer join areameter_master armeter on m.Meter_ID = armeter.Meter_ID ");
                builder.Append("left outer join areamaster area on armeter.Area_ID = area.Area_ID ");
                builder.Append("left outer join cmri_master cmri on area.CMRI_ID = cmri.CMRI_ID ");
                builder.Append("left outer join metermodel_master mmm on m.MeterModel_ID = mmm.MeterModel_ID ");
                builder.Append("left outer join metertype_master mtm on m.MeterType_ID = mtm.MeterType_ID ");
                //if (isPUMA)
                //{
                builder.Append("left outer join meterdata_general G on G.MeterData_ID = md.MeterData_ID ");
                // }
                builder.Append(string.Concat("where cm.status=1 and md.", MeterData_ID, " = ", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer Meter Details retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetConsumerMeterDetails(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }

        public DataSet GetMeterIDFromMeterDataID(long activeMeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select MeterID,ReadingDateTime,UploadingDateTime from meterdata where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("MeterID Details retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIDFromMeterDataID(long activeMeterData_ID)", ex);
            }
            return dataSet;
        }
        /// <summary>
        /// This function is used to get the No Load Time and No Supply Time
        /// </summary>
        /// <param name="activeMeterData_ID"></param>
        /// <returns></returns>
        public DataSet ListNoPowerSupplyLoadTime(long activeMeterData_ID, DataSet inputDataset)
        {
            DataSet dataSet = null;
            StringBuilder builder = null;
            DataRequest request = null;
            IDataHelper helper = null;

            try
            {
                helper = DatabaseFactory.GetHelper();
                dataSet = new DataSet();
                builder = new StringBuilder();
                builder.Append("Select A.InstantPowerColumnName,A.InstantPowerColumnValue,A.InstantPowerObisCode");
                builder.Append(" from meterdata_instantpower A, MeterData B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" and (A.InstantPowerObisCode =  " + "'0.0.94.91.8.255'" + " or A.InstantPowerObisCode = " + "'0.0.96.1.201.255' ) ");
                builder.Append("order by A.InstantPowerObisCode");
                request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), activeMeterData_ID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);

                inputDataset.Tables[0].Columns.Add("NoPowerSupplyTime");
                inputDataset.Tables[0].Columns.Add("NoLoadDataTime");

                if (dataSet != null && dataSet.Tables[0].Rows.Count != 0 && inputDataset != null && inputDataset.Tables[0].Rows.Count != 0)
                {
                    for (int count = 0; count < dataSet.Tables[0].Rows.Count; count++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dataSet.Tables[0].Rows[count]["InstantPowerColumnValue"])))
                        {
                            string[] data = dataSet.Tables[0].Rows[count]["InstantPowerColumnValue"].ToString().Split('*');
                            string hours = (Convert.ToInt64(data[0]) / 60).ToString();
                            string minutes = (Convert.ToInt64(data[0]) % 60).ToString("00");
                            dataSet.Tables[0].Rows[count]["InstantPowerColumnValue"] = hours + " : " + minutes;
                        }

                        if (dataSet.Tables[0].Rows[count]["InstantPowerObisCode"].ToString() == "0.0.94.91.8.255")
                        {
                            inputDataset.Tables[0].Rows[0]["NoPowerSupplyTime"] = dataSet.Tables[0].Rows[count]["InstantPowerColumnValue"];
                        }
                        else if (dataSet.Tables[0].Rows[count]["InstantPowerObisCode"].ToString() == "0.0.96.1.201.255")
                        {
                            inputDataset.Tables[0].Rows[0]["NoLoadDataTime"] = dataSet.Tables[0].Rows[count]["InstantPowerColumnValue"];
                        }
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ListNoPowerSupplyLoadTime(long activeMeterData_ID, DataSet inputDataset)", ex);
            }
            finally
            {
                dataSet = null;
                builder = null;
                request = null;
                helper = null;
            }
            return inputDataset;
        }


        /// <summary>
        /// Get Meter data id from meter id 
        /// it can retutrn rows in table . used while emf calculation
        /// </summary>
        /// <param name="metreId"></param>
        /// <returns></returns>
        public DataSet GetMeterDataIDFromMeterID(string metreId)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select MeterData_Id from meterdata where ");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), metreId, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("MeterDataID Details retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataIDFromMeterID(string metreId)", ex);
            }
            return dataSet;
        }

        public int GetAreaIDFromMeterID(string meterID)
        {
            int areaID = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Area_ID from areaMeter_Master where ");
                builder.Append(string.Concat(Meter_ID, " = ", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt32(data) != null)
                {
                    areaID = Convert.ToInt32(data);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAreaIDFromMeterID(string meterID)", ex);
                return 0;
            }
            return areaID;
        }



        public DataSet GetAreaDetails(int areaID)
        {
            DataSet dSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Circle_Name,Region_Name,Division_Name,CMRI_Number from areamaster AM Inner Join circle_Master CM on AM.Circle_ID = CM.Circle_ID ");
                builder.Append("Inner Join region_Master RM on RM.Region_ID = AM.Region_ID ");
                builder.Append("Inner Join Division_Master DM on DM.Division_ID = AM.Divsion_ID ");
                builder.Append("Inner Join CMRI_Master CIM on CIM.CMRI_ID = AM.CMRI_ID where ");
                builder.Append(string.Concat("AM.", Area_ID, " = ", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), areaID, DbType.Int64);
                dSet = new DataSet();
                dSet = helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Area details retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAreaDetails(int areaID)", ex);
                return null;
            }
            return dSet;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
        public bool CheckSimExist(string meterId)
        {
            bool flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_Phone from Meter_Master Where ");
                builder.Append(string.Concat("Meter_ID=", ParameterName("Meter_ID")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("Meter_ID"), meterId, DbType.String, 40);
                string val = Convert.ToString(helper.ExecuteScalar(request));
                if (!string.IsNullOrEmpty(val))
                    if (Convert.ToInt64(val) > 0)
                        flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckSimExist(string meterId)", ex);
                flag = false;
            }
            return flag;
        }
        public string GetSIMNumber(string meterId)
        {
            string phoneNumber = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_Phone from Meter_Master Where ");
                builder.Append(string.Concat("Meter_ID=", ParameterName("Meter_ID")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("Meter_ID"), meterId, DbType.String, 40);
                phoneNumber = Convert.ToString(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sim number viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetSIMNumber(string meterId)", ex);
                phoneNumber = string.Empty;
            }
            return phoneNumber;
        }
        public long GetMeterData(string meterId)
        {
            long values = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.MeterData_ID from MeterData A,ConsumerMeter B Where A.MeterID=B.Meter_ID and B.");
                builder.Append(string.Concat("Meter_ID=", ParameterName("Meter_ID")));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("Meter_ID"), meterId, DbType.String, 40);
                values = Convert.ToInt64(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(string meterId)", ex);
                values = 0;
            }
            return values;
        }


        /// <summary>
        /// Returns DataSet containg the File upload status 
        /// </summary>
        /// <param name="meterDataUpReloadId"></param>
        /// <returns></returns>
        public DataSet GetFileUploadedStatus(int meterDataUploadId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GetFileUploadDetails", CommandType.StoredProcedure);
                request.AddParamter(ParameterName("meterDataId"), meterDataUploadId, DbType.Int64, 4);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileUploadedStatus(int meterDataUploadId)", ex);
            }

            return dataSet;
        }
        /// <summary>
        /// gets consumer meter Details for meter ID.
        /// </summary>
        /// <param name="activeMeterID"> A meterID can have multiple files (each having different MeterData_ID ) </param>
        /// <returns></returns>
        public DataSet GetConsumerMeterDetailsForMeterID(string activeMeterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select ");
                builder.Append("md.MeterID,c.Consumer_Number, cm.Meter_AllocationDate, cm.Meter_Location,m.Meter_ContractDemand,");
                builder.Append("m.Meter_EMF, rm.Region_Name,cmri.CMRI_Number, crm.Circle_Name,dm.Division_Name,cm.Status,");
                builder.Append("m.UseEMFinCalculations,");
                //BhardwajG : Remove utility check from here will be handled in UI.
                //if (isPUMA)
                // {
                builder.Append("G.internalCTRatio,G.internalPTRatio,");
                //}
                builder.Append("mtm.MeterType_Name,mmm.MeterModel_Name  ");
                builder.Append("from meterdata md left outer join meter_master m on md.MeterID = m.Meter_ID ");
                builder.Append("left outer join consumermeter cm on m.Meter_ID = cm.Meter_ID ");
                builder.Append("left outer join consumer_master c on cm.Consumer_Number = c.Consumer_Number ");
                builder.Append("left outer join region_master rm on rm.Region_ID = cm.Region_ID ");
                builder.Append("left outer join division_master dm on dm.Division_ID = cm.Division_ID ");
                builder.Append("left outer join circle_master crm on crm.Circle_ID = cm.Circle_ID ");
                builder.Append("left outer join areameter_master armeter on m.Meter_ID = armeter.Meter_ID ");
                builder.Append("left outer join areamaster area on armeter.Area_ID = area.Area_ID ");
                builder.Append("left outer join cmri_master cmri on area.CMRI_ID = cmri.CMRI_ID ");
                builder.Append("left outer join metermodel_master mmm on m.MeterModel_ID = mmm.MeterModel_ID ");
                builder.Append("left outer join metertype_master mtm on m.MeterType_ID = mtm.MeterType_ID ");
                //if (isPUMA)
                //{
                builder.Append("join meterdata_general G on G.MeterData_ID = md.MeterData_ID ");
                // }
                builder.Append(string.Concat("where cm.status=1 and md.", MeterID, " = ", ParameterName(MeterID)));
                builder.Append(" group by md.MeterID");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), activeMeterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer Meter Details retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetConsumerMeterDetailsForMeterID(string activeMeterID)", ex);
            }
            return dataSet;
        }


        public MeterDataEntity GetDataforCMRI(string cmriID)
        {
            MeterDataEntity meterDataEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select MeterData_ID,FileUpload_ID,MeterID,ReadingDateTime,UploadingDateTime from MeterData where ");
                builder.Append(string.Concat(CMRI_Number, "=", ParameterName(CMRI_Number)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriID, DbType.String, 50);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            meterDataEntity = (MeterDataEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataforCMRI(string cmriID)", ex);
            }
            return meterDataEntity;
        }

        public DataSet UpdateCMRIID(string meterId, string cmriID)
        {
            DataSet fUploadIDs;
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("Update meterdata Set ");
            builder.Append(string.Concat(CMRI_Number, "= ", ParameterName(CMRI_Number), ""));
            builder.Append(string.Concat(" Where ", MeterID, "=", ParameterName(MeterID)));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(CMRI_Number), cmriID, DbType.String);
            request.AddParamter(ParameterName(MeterID), meterId, DbType.String);
            helper.ExecuteNonQuery(request);

            fUploadIDs = fetchFileUploadID(meterId);

            return fUploadIDs;
        }

        private DataSet fetchFileUploadID(string meterId)
        {            
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("Select FileUpload_ID from MeterData where ");
            builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));

            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(MeterID), meterId, DbType.String, 50);
            DataSet ds = new DataSet();
            ds = helper.FillDataSet(request, ds);           
            return ds;
        }
        /// <summary>
        /// Fetch list for excel export for meterwise option
        /// </summary>
        /// <returns></returns>
        public DataSet ExcelExportListDataSet()
        {
            DataSet slData = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = null;
                
                    //For MSEDCL , file need to have general instant and billing data .
                builder.Append("Select distinct m.MeterID from fileupload_master f ");                    
                    builder.Append("join meterdata m join meterdata_general g  join meterdata_billing b ");
                    builder.Append("where f.FileUpload_ID= m.FileUpload_ID and m.MeterData_ID= g.MeterData_ID ");                   
                    builder.Append("and m.MeterData_ID= b.MeterData_ID;");
                
                request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                slData = new DataSet();
                slData.Tables.Add(AutoNumberedTable(dataSet.Tables[0]));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " ExcelExportListDataSet()", ex);
            }
            return slData;
        }

        public DataSet GetAllMeterIDLPRIDValues()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append(" select md.meterID, CASE  mm.Meter_GPRSModem_IMEI  WHEN NULL THEN ''  ELSE mm.Meter_GPRSModem_IMEI  END as 'Meter_GPRSModem_IMEI' from `dlms_ltct_650`.meterdata md join `dlms_ltct_650`.meter_master mm where md.meterID = mm.Meter_ID union select  cm.Meter_ID, CASE  mm.Meter_GPRSModem_IMEI  WHEN NULL THEN ''  ELSE mm.Meter_GPRSModem_IMEI  END as 'Meter_GPRSModem_IMEI' from `dlms_ltct_650`.consumermeter cm join `dlms_ltct_650`.meter_master mm where cm.Meter_ID = mm.Meter_ID and cm.Meter_ID is not Null and Status <> 0");
                builder.Append(" select md.meterID, mm.Meter_GPRSModem_IMEI  from meterdata md left join meter_master mm on md.meterID = mm.Meter_ID union select  cm.Meter_ID, mm.Meter_GPRSModem_IMEI  from consumermeter cm left join meter_master mm on cm.Meter_ID = mm.Meter_ID and cm.Meter_ID is not Null and Status <> 0");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllMeterIDLPRIDValues()", ex);
            }
            return dataSet;
        }

       






    }
}
