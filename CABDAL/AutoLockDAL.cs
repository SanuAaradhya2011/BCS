#region NameSpaces
using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CABEntity;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;
#endregion

namespace LTCTDAL
{
    public class AutoLockDAL :DALBase
    {

        private string MeterID = "MeterID";
        private string AutoLockId = "AutoLockId";
        private string FileUploadID = "FileUploadID";
        private string AutoLockStatus = "AutoLockStatus";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AutoLockDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            AutoLockEntity autoLockEntity = entity as AutoLockEntity;            
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into autolock(MeterData_ID,AutoLockStatus) values(");                
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(AutoLockStatus), ")"));                
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), autoLockEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(AutoLockStatus), autoLockEntity.AutoLockStatus.ToString(), DbType.String, 20);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("AutoLock lock configuration inserted"));
               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return autoLockEntity;

        }
        public AutoLockEntity GetData( Int64 MeterData_ID)
        {
            AutoLockEntity autoLockEntity = new AutoLockEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from AutoLock where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());                
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                   // autoLockEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["AutoLockStatus"] != null)
                        autoLockEntity.AutoLockStatus = iDataset.Tables[0].Rows[0]["AutoLockStatus"].ToString();

                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Auto Lock read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
            return autoLockEntity;
        }
      
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Data 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from autolock where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                int i = helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Auto lock data for a specified meter deleted."));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }
        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
