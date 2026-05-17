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
    public class PulseEnergyDAL : DALBase
    {
        #region Constants & variables
        private string PulseEnergyId = "pulseEnergyId";
        private string PulseEnergyValue = "pulseEnergyValue";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(PulseEnergyDAL).ToString());
        #endregion

        #region Public Methods
        public override IEntity InsertData(IEntity entity)
        {
            PulseEnergyEntity pulseEnergyEntity = entity as PulseEnergyEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into pulseEnergy(MeterData_ID,pulseEnergyValue) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(PulseEnergyValue), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), pulseEnergyEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(PulseEnergyValue), pulseEnergyEntity.PulseEnergyValue, DbType.String);
                pulseEnergyEntity.PulseEnergyId = helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("PulseEnergy  configuration inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return pulseEnergyEntity;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public string GetData(Int64 MeterData_ID)
        {
            string pulseEnergyValue;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select  pulseEnergyValue  from dlms_ltct_650.pulseEnergy where  MeterData_ID=" + MeterData_ID);
                pulseEnergyValue = Convert.ToString(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("PulseEnergy read from DB"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return string.Empty;
            }
            return pulseEnergyValue;
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
                builder.Append("Delete from pulseEnergy where ");
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

