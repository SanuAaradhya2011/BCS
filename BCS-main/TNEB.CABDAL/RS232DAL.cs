using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CABEntity;

namespace LTCTDAL
{
    public class RS232DAL : DALBase
    {
        private string MeterID = "MeterID";
        private string RS232ID = "RS232ID";
        private string FileUploadID = "FileUploadID";
        private string RS232Status = "RS232Status";
        private string MeterDataID = "MeterData_ID";
        
        public IEntity InsertData(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECRS232LockEntity rs232entity = entity as IECRS232LockEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into RS232(MeterID,MeterData_ID,RS232Status,FileUploadID) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(RS232Status), ","));
                builder.Append(string.Concat(ParameterName(FileUploadID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), rs232entity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(FileUploadID), fileUploadID, DbType.Int64);
                request.AddParamter(ParameterName(RS232Status), rs232entity.LockStatus.ToString(), DbType.String, 20);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RS232 lock configuration inserted"));
                Flag = true;
            }
            catch (Exception)
            {
            }
            return rs232entity;

        }
        public IECRS232LockEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            IECRS232LockEntity rs232LockEntity = new IECRS232LockEntity();
            try
            {//CumulativeKWh,CumulativeKVARhLag,CumulativeKVARhLead,CumulativeKVAh,DailyMD1,FileUploadID,DailyMD2
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from RS232 where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    rs232LockEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["RS232Status"] != null)
                        rs232LockEntity.LockStatus = iDataset.Tables[0].Rows[0]["RS232Status"].ToString();
                    
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RS232 read from DB"));
            }
            catch (CABException)
            {
                return null;
            }
            return rs232LockEntity;
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
