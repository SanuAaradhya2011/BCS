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
    
    public class RTCDAL : DALBase
    {

        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RTCDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            
             RTCEntity rtcEntity = entity as RTCEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                builder = new StringBuilder();
                builder.Append("Insert Into RTC(MeterData_ID,RTC) values(");
                builder.Append(string.Concat(ParameterName("MeterData_ID"), ","));
                builder.Append(string.Concat(ParameterName("RTC"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterData_ID"),rtcEntity.MeterDataID , DbType.Int64);
                request.AddParamter(ParameterName("RTC"), rtcEntity.RTC, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RTC added"));
                return rtcEntity;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return rtcEntity;
        }
      
        public string GetData( Int64 MeterData_ID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from rtc where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater Read from Db"));
                if (iDataset.Tables.Count!=0 && iDataset.Tables[0].Rows.Count > 0)
                    return iDataset.Tables[0].Rows[0]["RTC"].ToString();
                else
                    return null;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
        }

        public bool DeleteAllData(IEntity entity)
        {
            bool Flag = false;
            RTCEntity rtcEntity = entity as RTCEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                DataRequest request = new DataRequest("Delete from  RTC where  MeterData_ID=" );
                builder.Append(string.Concat(ParameterName(MeterDataID), ")"));
                helper.ExecuteNonQuery(request);
                request.AddParamter(ParameterName(MeterDataID), rtcEntity.MeterDataID, DbType.Int64);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("RTC data Deleted"));
                return true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteAllData(IEntity entity)", ex);
                return false;
            }
        }

         public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from rtc where ");
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
        public override IList<IEntity> ListData()
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
