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
    public class LoadControlDAL : DALBase
    {

        private string MeterDataID = "MeterData_ID";
        private string LCData = "LCData";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoadControlDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            LoadControlEntity lcEntity = entity as LoadControlEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into loadcontrol(MeterData_ID,LCData) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(LCData), ')'));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), lcEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(LCData), lcEntity.LCData, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Control in meter readout inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return lcEntity;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public LoadControlEntity GetData(Int64 MeterData_ID)
        {
            LoadControlEntity LCEntity = new LoadControlEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select IFNULL(LCData,'') from loadcontrol where  MeterData_ID=" + MeterData_ID);
                object result = helper.ExecuteScalar(request);
                LCEntity.LCData = result == null ? string.Empty : result.ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Load Control read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetData(Int64 MeterData_ID)", ex);
                return null;
            }
            return LCEntity;
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
                builder.Append("Delete from loadcontrol where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Loadcontrol data for a specified file deleted."));
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
