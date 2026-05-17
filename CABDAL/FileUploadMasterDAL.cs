using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Framework.Utility.DataBaseType;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class FileUploadMasterDAL : DALBase
    {
        private string FileUpload_ID = "FileUpload_ID";
        private string UploadingDateTime = "UploadingDateTime";
        private string FileContent = "FileContent";
        private string UserInformation_ID = "UserInformation_ID";
        private string FileName = "FileName";
        private string ReadingDateTime = "readingDateTime";
        private string MeterDataID = "MeterDataID";
        private string CMRI_Number = "CMRI_Number";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FileUploadMasterDAL).ToString());
        public FileUploadMasterDAL()
            : base("fileupload_master", "FileUpload_ID")
        {
        }
        public IEntity InsertData(IEntity entity, bool Flag)
        {
            FileUploadMasterEntity fileUploadMasterEntity = entity as FileUploadMasterEntity;
            try
            {
                if (ConfigInfo.GetDBType().Equals(DBType.My_SQL))
                {
                    int rowsAffected;

                    MySql.Data.MySqlClient.MySqlConnection connections = new MySql.Data.MySqlClient.MySqlConnection(ConfigInfo.GetConnectionString());
                    MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand();
                    connections.Open();
                    command.Connection = connections;
                    command.CommandText = "Insert Into fileupload_master(UploadingDateTime,FileContent,UserInformation_ID,FileName,readingDateTime,FileType,CommType,FileSize,CMRI_Number) VALUES(@UploadingDateTime,@FileContent,@UserInformation_ID,@FileName,@readingDateTime,@FileType,@CommType,@FileSize,@CMRI_Number)";
                    command.Parameters.AddWithValue("@UploadingDateTime", fileUploadMasterEntity.UploadingDateTime);
                    command.Parameters.AddWithValue("@FileContent", fileUploadMasterEntity.FileContent);
                    command.Parameters.AddWithValue("@UserInformation_ID", fileUploadMasterEntity.UserInformation_ID);
                    command.Parameters.AddWithValue("@FileName", fileUploadMasterEntity.FileName);
                    command.Parameters.AddWithValue("@readingDateTime", fileUploadMasterEntity.ReadingDateTime);
                    command.Parameters.AddWithValue("@FileType", fileUploadMasterEntity.FileType);
                    command.Parameters.AddWithValue("@CommType", fileUploadMasterEntity.CommType);
                    command.Parameters.AddWithValue("@FileSize", fileUploadMasterEntity.FileSize);
                    command.Parameters.AddWithValue("@CMRI_Number", fileUploadMasterEntity.CMRIID);
                    rowsAffected = command.ExecuteNonQuery();
                    fileUploadMasterEntity.FileUpload_ID = long.Parse(this.GetPK());
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details inserted"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity, bool Flag)", ex);
                return fileUploadMasterEntity; 
            }

            return fileUploadMasterEntity;
        }
     

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            FileUploadMasterEntity fileUploadMasterEntity = entity as FileUploadMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from fileupload_master where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), fileUploadMasterEntity.FileUpload_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File Deleted."));
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details deleted"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            FileUploadMasterEntity fileUploadMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileUpload_ID,UploadingDateTime,FileContent,UserInformation_ID,FileName,readingDateTime from fileupload_master where ");
                builder.Append(string.Concat(FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileUpload_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    fileUploadMasterEntity = (FileUploadMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
            }
            return fileUploadMasterEntity;
        }
        public IEntity GetDetailData(string fileName)
        {
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileUpload_ID,UploadingDateTime,FileContent,UserInformation_ID,FileName,CommType,readingDateTime from fileupload_master where ");
                builder.Append(string.Concat(FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    fileUploadMasterEntity = (FileUploadMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string fileName)", ex);
            }
            return fileUploadMasterEntity;
        }
        public IEntity GetDetailData(string fileName, long readingDateTime)
        {
            FileUploadMasterEntity fileUploadMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileUpload_ID,UploadingDateTime,FileContent,UserInformation_ID,FileName,readingDateTime from fileupload_master where ");
                builder.Append(string.Concat(FileName, "=", ParameterName(FileName), " and "));
                builder.Append(string.Concat(ReadingDateTime, "=", ParameterName(ReadingDateTime)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
                request.AddParamter(ParameterName(ReadingDateTime), readingDateTime, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds == null)
                    return fileUploadMasterEntity;
                if (ds.Tables.Count == 0)
                    return fileUploadMasterEntity;
                if (ds.Tables[0].Rows.Count > 0)
                    fileUploadMasterEntity = (FileUploadMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string fileName, long readingDateTime)", ex);
            }
            return fileUploadMasterEntity;
        }
        public override IList<IEntity> ListData()
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
                builder.Append("Select distinct FileName from fileupload_master order by FileUpload_ID desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ComboList()", ex);
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
                builder.Append("Select FileUpload_ID,UploadingDateTime,FileContent,UserInformation_ID,FileName from fileupload_master");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
            }
            return dataSet;
        }

        public DataSet GetCABFileNames()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileUpload_ID,FileName from fileupload_master order by FileUpload_ID desc");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCABFileNames()", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// To get the filename with meter data id
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetCABFileNamesWithMeterDataID(long MeterData_ID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.FileUpload_ID,a.FileName from meterdata b join fileupload_master a on a.FileUpload_Id=b.FileUpload_ID where b.MeterData_ID = ");
                builder.Append(string.Concat(ParameterName(MeterDataID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCABFileNamesWithMeterDataID(long MeterData_ID)", ex);
            }
            return dataSet;
        }
        //public DataSet GetCABFileNamesBetweenDates(long fromDate, long toDate)
        //{
        //    DataSet dataSet = null;
        //    try
        //    {
        //        IDataHelper helper = DatabaseFactory.GetHelper();
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append("Select FileUpload_ID,FileName from fileupload_master where UploadingDateTime between ");
        //        builder.Append(string.Concat(fromDate, " ", "and", " ", toDate, " "));
        //        builder.Append("order by FileUpload_ID desc");
        //        DataRequest request = new DataRequest(builder.ToString());
        //        dataSet = new DataSet();
        //        dataSet = helper.FillDataSet(request, dataSet);
        //        UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
        //    }
        //    catch (CABException ex)    //Exception log for catch block
        //    {
        //    }
        //    return dataSet;
        //}

        /// <summary>
        /// Get lng files between date range
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="showMeterModelNo"></param>
        /// <returns></returns>
        public DataSet GetCABFileNamesBetweenDates(long fromDate, long toDate, bool showMeterModelNo)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //BhardwajG : If meter model no is required then fetch MeterModelNo also from database
                if (showMeterModelNo)
                {
                    builder.Append("Select f.FileUpload_ID,f.FileName,mg.MeterModelNo from fileupload_master f ");
                    builder.Append("join meterdata md on md.FileUpload_ID = f.FileUpload_ID join meterdata_general mg on md.MeterData_ID = mg.MeterData_ID  ");
                    builder.Append("where f.UploadingDateTime between ");
                    builder.Append(string.Concat(fromDate, " ", "and", " ", toDate, " "));
                    builder.Append("order by f.FileUpload_ID desc");
                }
                else
                {
                    builder.Append("Select FileUpload_ID,FileName from fileupload_master where UploadingDateTime between ");
                    builder.Append(string.Concat(fromDate, " ", "and", " ", toDate, " "));
                    builder.Append("order by FileUpload_ID desc");
                }
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCABFileNamesBetweenDates(long fromDate, long toDate, bool showMeterModelNo)", ex);
            }
            return dataSet;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> ListDataSet(bool flag)
        {
            IDataReader reader = null;
            Dictionary<string, int> fileDictionary = new Dictionary<string, int>();

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select FileUpload_ID,FileName from fileupload_master");
                DataRequest request = new DataRequest(builder.ToString());
                reader = helper.ExecuteReader(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File details viewed"));

                while (reader.Read())
                {
                    fileDictionary.Add(reader.GetString(reader.GetOrdinal(FileName)), reader.GetInt32(reader.GetOrdinal(FileUpload_ID)));
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(bool flag)", ex);
            }
            return fileDictionary;
        }

        public Int32 GetFileUploadID(string fileName)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("Select FileUpload_ID from fileupload_master where FileName = ");
            builder.Append(string.Concat(ParameterName(FileName)));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(FileName), fileName, DbType.String, 40);
            object obj = helper.ExecuteScalar(request);
            if (Convert.ToInt32(obj) > 0)
            {
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File upload ID retrieved"));
                return Convert.ToInt32(obj);
            }
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("File upload ID retrieved"));
            return 0;
        }


        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            FileUploadMasterEntity fileUploadMasterEntity = new FileUploadMasterEntity();
            if (NotNullAndNotDBNull(row, FileUpload_ID)) fileUploadMasterEntity.FileUpload_ID = Convert.ToInt32(row[FileUpload_ID]);
            if (NotNullAndNotDBNull(row, UploadingDateTime)) fileUploadMasterEntity.UploadingDateTime = Convert.ToInt64(row[UploadingDateTime]);
            if (NotNullAndNotDBNull(row, FileContent)) fileUploadMasterEntity.FileContent = (byte[])row[FileContent];
            if (NotNullAndNotDBNull(row, UserInformation_ID)) fileUploadMasterEntity.UserInformation_ID = Convert.ToInt32(row[UserInformation_ID]);
            if (NotNullAndNotDBNull(row, FileName)) fileUploadMasterEntity.FileName = Convert.ToString(row[FileName]);
            if (NotNullAndNotDBNull(row, ReadingDateTime)) fileUploadMasterEntity.ReadingDateTime = Convert.ToInt64(row[ReadingDateTime]);
            return fileUploadMasterEntity;
        }


        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void UpdateCMRIID(DataSet fuploadIDs, string cmriNo)
        {
            foreach (DataRow fuploadID in fuploadIDs.Tables[0].Rows)
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update fileupload_master Set ");
                builder.Append(string.Concat(CMRI_Number, "= ", ParameterName(CMRI_Number), ""));
                builder.Append(string.Concat(" Where ", FileUpload_ID, "=", ParameterName(FileUpload_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CMRI_Number), cmriNo, DbType.String);
                request.AddParamter(ParameterName(FileUpload_ID), fuploadID["FileUpload_ID"], DbType.Int32);
                helper.ExecuteNonQuery(request);
            }
        }
    }
}