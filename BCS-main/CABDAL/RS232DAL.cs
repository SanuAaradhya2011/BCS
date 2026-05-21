using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using CAB.Framework.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace LTCTDAL
{
    public class RS232DAL : DALBase
    {
        private string MeterID = "MeterID";
        private string RS232ID = "RS232ID";
        private string FileUploadID = "FileUploadID";
        private string RS232Status = "RS232Status";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RS232DAL).ToString());
        
        public override IEntity InsertData(IEntity entity)
        {
            RS232LockEntity rs232entity = entity as RS232LockEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into RS232(MeterData_ID,RS232Status) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(RS232Status), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), rs232entity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(RS232Status), rs232entity.LockStatus.ToString(), DbType.String, 20);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RS232 lock configuration inserted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return rs232entity;

        }
        public  RS232LockEntity GetData( Int64 MeterData_ID)
        {
            RS232LockEntity rs232LockEntity = new RS232LockEntity();
            try
            {//CumulativeKWh,CumulativeKVARhLag,CumulativeKVARhLead,CumulativeKVAh,DailyMD1,FileUploadID,DailyMD2
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from RS232 where MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    //rs232LockEntity.MeterDataID = MeterDataID;
                    if (iDataset.Tables[0].Rows[0]["RS232Status"] != null)
                        rs232LockEntity.LockStatus = iDataset.Tables[0].Rows[0]["RS232Status"].ToString();
                    
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RS232 read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
            return rs232LockEntity;
        }
       
         public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from RS232 where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterData_ID)", ex);
            }
            return Flag;
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
