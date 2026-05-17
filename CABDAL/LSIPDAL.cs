#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using CAB.Entity;
using Hunt.EPIC.Logging;

#endregion

namespace CAB.DALC.Data
{
    public class LSIPDAL : DALBase
    {
        #region Constants & variables
        private string LSIPId = "LSIPId";
        private string LSIPValue = "LSIPValue";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LSIPDAL).ToString());
        #endregion

        #region Public Methods
        public override IEntity InsertData(IEntity entity)
        {
            LSIPEntity lsipEntity = entity as LSIPEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into LSIP(MeterData_ID,LSIPValue) values(");               
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(LSIPValue),")"));               
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), lsipEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(LSIPValue), lsipEntity.LSIPValue.ToString(), DbType.Int32);
                lsipEntity.LSIPId = helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("LSIP  configuration inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return lsipEntity;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public int GetData(Int64 MeterData_ID)
        {
            int loadSurveyCapturePeriod;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select  IFNULL(LSIPValue,0) as LSIPValue  from LSIP where  MeterData_ID=" + MeterData_ID);
                int.TryParse(helper.ExecuteScalar(request).ToString(),out loadSurveyCapturePeriod);               
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("LSIP read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return 0;
            }
            return loadSurveyCapturePeriod;
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
                builder.Append("Delete from LSIP where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                int i = helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("LSIP data for a specified meter deleted."));
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
        #endregion
    }
}
