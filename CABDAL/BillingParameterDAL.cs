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
    public class BillingParameterDAL : DALBase
    {
        private string ColumnsNames = "ColumnsNames";
        private string MeterData_ID = "MeterData_ID";
        private string MeterID = "MeterID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingParameterDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            BillingParameterEntity billingParameterEntity = null;
            if (entity == null)
                return billingParameterEntity;
            billingParameterEntity = entity as BillingParameterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into billingparameter(ColumnsNames,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(ColumnsNames), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(ColumnsNames), billingParameterEntity.ColumnsNames, DbType.String, 1000);
                request.AddParamter(ParameterName(MeterData_ID), billingParameterEntity.MeterDataId, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                billingParameterEntity = null;
            }
            return billingParameterEntity;
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from billingparameter where ");
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

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
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


        public IEntity GetDetailData(long id)
        {
            BillingParameterEntity billingParameterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ColumnsNames,MeterData_ID from billingparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    billingParameterEntity = (BillingParameterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing Update record viewed"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(long id)", ex);
            }
            return billingParameterEntity;
        }


        public DataSet GetColumnNames(long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ColumnsNames,MeterData_ID from billingparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.UInt32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetColumnNamesForMeterID(string meterID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct parameter.ColumnsNames  FROM billingparameter parameter , meterdata mdata");
                builder.Append(" where parameter.MeterData_ID = mdata.MeterData_ID and mdata.");
                builder.Append(string.Concat(MeterID, "=", ParameterName(MeterID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data viewed"));
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
            BillingParameterEntity billingParameterEntity = new BillingParameterEntity();
            if (NotNullAndNotDBNull(row, MeterData_ID)) billingParameterEntity.MeterDataId = Convert.ToInt32(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, ColumnsNames)) billingParameterEntity.ColumnsNames = Convert.ToString(row[ColumnsNames]);
            return billingParameterEntity;
        }



        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

       
    }
}
