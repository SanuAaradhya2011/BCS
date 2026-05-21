using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CABEntity;
using CAB.DALC.Data;
using System.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CABDAL
{
    public class OpticalLockUnlockDAL : DALBase
    {

        private string MeterDataID = "MeterData_ID";
        private string OPData = "OPData";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(OpticalLockUnlockDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            OpticalLockUnlockEntity OPEntity = entity as OpticalLockUnlockEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into Opticallock(MeterData_ID,OPData) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(OPData), ')'));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), OPEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(OPData), OPEntity.OPData, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("OpticalLockUnlock in meter readout inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return OPEntity;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public OpticalLockUnlockEntity GetData(Int64 MeterData_ID)
        {
            OpticalLockUnlockEntity OPEntity = new OpticalLockUnlockEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select IFNULL(OPData,'') from Opticallock where  MeterData_ID=" + MeterData_ID);
                object result = helper.ExecuteScalar(request);
                OPEntity.OPData = result == null ? string.Empty : result.ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("OpticalLockUnlock read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return null;
            }
            return OPEntity;
        }     

        public override CAB.Framework.Entity.IEntity InsertData(IList<CAB.Framework.Entity.IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(CAB.Framework.Entity.IEntity entity)
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
                builder.Append("Delete from Opticallock where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("OpticalLockUnlock data for a specified file deleted."));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }


        public override CAB.Framework.Entity.IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<CAB.Framework.Entity.IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override System.Data.DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity RowToEntity(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
