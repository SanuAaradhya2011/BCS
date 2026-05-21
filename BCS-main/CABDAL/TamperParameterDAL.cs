using System;
using System.Data;
using System.Text;
using ExceptionServices.Data;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class TamperParameterDAL : DALBase
    {
        private string ColumnsNames = "ColumnsNames";
        private string MeterData_ID = "MeterData_ID";
        private string MeterID = "MeterID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TamperParameterDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            TamperParameterEntity tamperParameterEntity = null;
            if (entity == null)
                return tamperParameterEntity;
            tamperParameterEntity = entity as TamperParameterEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into tamperparameter(ColumnsNames,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(ColumnsNames), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ColumnsNames), tamperParameterEntity.ColumnsNames, DbType.String, 1000);
                request.AddParamter(ParameterName(MeterData_ID), tamperParameterEntity.MeterDataId, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data added"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                tamperParameterEntity = null;
            }
            return tamperParameterEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from tamperparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }
        public IEntity GetDetailData(long id)
        {
            TamperParameterEntity tamperParameterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ColumnsNames,MeterData_ID from tamperparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    tamperParameterEntity = (TamperParameterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data RTC Update record viewed"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(long id)", ex);
            }
            return tamperParameterEntity;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new System.NotImplementedException();
        }

        public DataSet GetColumnNames(long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ColumnsNames from tamperparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetColumnNames(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// Gets Column Names of midnight parameters For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet GetColumnNamesForMeterID(string meterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct parameter.ColumnsNames  FROM tamperparameter parameter , meterdata mdata");
                builder.Append(" where parameter.MeterData_ID = mdata.MeterData_ID and mdata.");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetColumnNamesForMeterID(string meterID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperParameterEntity tamperParameterEntity = new TamperParameterEntity();
            if (NotNullAndNotDBNull(row, MeterData_ID)) tamperParameterEntity.MeterDataId = Convert.ToInt32(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, ColumnsNames)) tamperParameterEntity.ColumnsNames = Convert.ToString(row[ColumnsNames]);
            return tamperParameterEntity;
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
