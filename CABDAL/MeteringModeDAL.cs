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
    public class MeteringModeDAL : DALBase
    {

        private string MeterDataID = "MeterData_ID";
        private string MMData = "MMData";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeteringModeDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            MeteringModeEntity MMEntity = entity as MeteringModeEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into meteringmode(MeterData_ID,MMData) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(MMData), ')'));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), MMEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(MMData), MMEntity.MMData, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("meteringmode in meter readout inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return MMEntity;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MeterData_ID"></param>
        /// <returns></returns>
        public MeteringModeEntity GetData(Int64 MeterData_ID)
        {
            MeteringModeEntity MMEntity = new MeteringModeEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select IFNULL(MMData,'') from meteringmode where  MeterData_ID=" + MeterData_ID);
                object result = helper.ExecuteScalar(request);
                MMEntity.MMData = result == null ? string.Empty : result.ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("meteringmode read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return null;
            }
            return MMEntity;
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
                builder.Append("Delete from meteringmode where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("meteringmode for a specified file deleted."));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " DeleteData(long meterDataID)", ex);
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
