#region NameSpaces
using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CABEntity;
using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
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

        public IEntity InsertData(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECAutoLockEntity autoLockEntity = entity as IECAutoLockEntity;            
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into autolock(MeterID,MeterData_ID,AutoLockStatus,FileUploadID) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(AutoLockStatus), ","));
                builder.Append(string.Concat(ParameterName(FileUploadID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), autoLockEntity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(AutoLockStatus), autoLockEntity.AutoLockStatus.ToString(), DbType.String, 20);
                request.AddParamter(ParameterName(FileUploadID), fileUploadID, DbType.Int64);                
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("AutoLock lock configuration inserted"));
               
            }
            catch (Exception)
            {
            }
            return autoLockEntity;

        }
        public IECAutoLockEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECAutoLockEntity autoLockEntity = new IECAutoLockEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from AutoLock where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());                
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    autoLockEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["AutoLockStatus"] != null)
                        autoLockEntity.AutoLockStatus = iDataset.Tables[0].Rows[0]["AutoLockStatus"].ToString();

                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Auto Lock read from DB"));
            }
            catch (CABException)
            {
                return null;
            }
            return autoLockEntity;
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
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
            catch (Exception) { }
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
