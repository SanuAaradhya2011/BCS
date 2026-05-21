/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 05/10/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class DLMS650InstantaneousDAL : DALBase
    {
       
        private string InstantPower_ID = "InstantPower_ID";
        private string InstantPowerColumnName = "InstantPowerColumnName";
        private string InstantPowerColumnValue = "InstantPowerColumnValue";
        private string InstantPowerObisCode = "InstantPowerObisCode";
        private string InstantPowerClassID = "InstantPowerClassID";
        private string InstantPowerAttribute = "InstantPowerAttribute";
        private string InstantPowerDataIndex = "InstantPowerDataIndex";
        private string MeterData_ID = "MeterData_ID";
        private string meterSerialNumber = "meterSerialNumber";
        private string MeterID = "MeterID";
        private string FileName = "FileName";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650InstantaneousDAL).ToString());

        public DLMS650InstantaneousDAL()
            : base("meterdata_instantpower", "instantPower_ID")
        {
        }
        public DataSet GetMeterData(int meterDataID)
        {
           
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.InstantPowerColumnName,A.InstantPowerColumnValue,A.InstantPowerObisCode,A.InstantPowerClassID,A.InstantPowerAttribute,A.InstantPowerDataIndex");
                builder.Append(" from meterdata_instantpower A, MeterData B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        private DataRequest GetRequest(IEntity entity)
        {
            if (entity == null)
                return null;
            DLMS650InstantaneousEntity instantaneousEntity = entity as DLMS650InstantaneousEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_instantpower(InstantPowerColumnName,InstantPowerColumnValue,InstantPowerObisCode,InstantPowerClassID,InstantPowerAttribute,InstantPowerDataIndex,MeterData_ID) values(");
            builder.Append(string.Concat(ParameterName(InstantPowerColumnName), ","));
            builder.Append(string.Concat(ParameterName(InstantPowerColumnValue), ","));
            builder.Append(string.Concat(ParameterName(InstantPowerObisCode), ","));
            builder.Append(string.Concat(ParameterName(InstantPowerClassID), ","));
            builder.Append(string.Concat(ParameterName(InstantPowerAttribute), ","));
            builder.Append(string.Concat(ParameterName(InstantPowerDataIndex), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));

            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(InstantPowerColumnName), instantaneousEntity.InstantPowerColumnName, DbType.String, 60);
            request.AddParamter(ParameterName(InstantPowerColumnValue), instantaneousEntity.InstantPowerColumnValue, DbType.String, 40);
            request.AddParamter(ParameterName(InstantPowerObisCode), instantaneousEntity.InstantPowerObisCode, DbType.String, 40);
            request.AddParamter(ParameterName(InstantPowerClassID), instantaneousEntity.InstantPowerClassID, DbType.String, 40);
            request.AddParamter(ParameterName(InstantPowerAttribute), instantaneousEntity.InstantPowerAttribute, DbType.String, 40);
            request.AddParamter(ParameterName(InstantPowerDataIndex), instantaneousEntity.InstantPowerDataIndex, DbType.Int64);
            request.AddParamter(ParameterName(MeterData_ID), instantaneousEntity.MeterDataID, DbType.Int64);
            return request;
        }
        
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DLMS650InstantaneousEntity instantaneousEntity = new DLMS650InstantaneousEntity();
            if (NotNullAndNotDBNull(row, InstantPower_ID)) instantaneousEntity.InstantPower_ID = Convert.ToInt64(row[InstantPower_ID]);
            if (NotNullAndNotDBNull(row, InstantPowerColumnName)) instantaneousEntity.InstantPowerColumnName = Convert.ToString(row[InstantPowerColumnName]);
            if (NotNullAndNotDBNull(row, InstantPowerColumnValue)) instantaneousEntity.InstantPowerColumnValue = Convert.ToString(row[InstantPowerColumnValue]);
            if (NotNullAndNotDBNull(row, InstantPowerObisCode)) instantaneousEntity.InstantPowerObisCode = Convert.ToString(row[InstantPowerObisCode]);
            if (NotNullAndNotDBNull(row, InstantPowerClassID)) instantaneousEntity.InstantPowerClassID = Convert.ToString(row[InstantPowerClassID]);
            if (NotNullAndNotDBNull(row, InstantPowerAttribute)) instantaneousEntity.InstantPowerAttribute = Convert.ToString(row[InstantPowerAttribute]);
            if (NotNullAndNotDBNull(row, InstantPowerDataIndex)) instantaneousEntity.InstantPowerDataIndex = Convert.ToInt64(row[InstantPowerDataIndex]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) instantaneousEntity.MeterDataID = Convert.ToInt64(row[MeterData_ID]);
         
            return instantaneousEntity;
        }

        public override IEntity InsertData(IEntity entity)
        {
            BillingEntity billingEntity = entity as BillingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = this.GetRequest(entity);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
           catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
           }
            if (Flag)
                billingEntity.Billing_ID = long.Parse(this.GetPK());
            return billingEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();
            foreach (IEntity entity in entities)
                requests.Add(this.GetRequest(entity));
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IList<IEntity> entities)", ex);
            }
            return null;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_instantpower where ");
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
        public DataSet GetInstantDataByMeter(string meterID, List<string> parameters,List<string>columns,string activeMeterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "i.", column, " "));
                }
                builder.Append(",m.MeterData_ID from meterdata_instantpower i inner join meterdata m on i.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                //builder.Append(string.Concat(" AND ", "i.",MeterData_ID ,"=", ParameterName(MeterData_ID)));
                builder.Append(" AND InstantPowerColumnName in (");
                for (int i = 0; i < parameters.Count; i++)
                {
                    builder.Append(string.Concat("'", parameters[i].ToString(), "'", ","));
                }
               
                builder = builder.Remove(builder.Length - 1, 1);
                builder.Append(")");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                //request.AddParamter(ParameterName(MeterData_ID), activeMeterDataID, DbType.String, 20);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantDataByMeter(string meterID, List<string> parameters,List<string>columns,string activeMeterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet GetInstantDataByFileName(string meterID,string fileName, List<string> parameters, List<string> columns, string activeMeterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select  m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "i.", column, " "));
                }
                builder.Append(",m.MeterData_ID from meterdata_instantpower i inner join meterdata m on i.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "AND", " ", "f.", FileName, "=", ParameterName(FileName)));
                builder.Append(" AND InstantPowerColumnName in (");
                for (int i = 0; i < parameters.Count; i++)
                {
                    builder.Append(string.Concat("'", parameters[i].ToString(), "'", ","));
                }
                builder = builder.Remove(builder.Length - 1, 1);
                builder.Append(")");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Instant Power data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantDataByFileName(string meterID,string fileName, List<string> parameters, List<string> columns, string activeMeterDataID)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// /// Getting tamper parameters by obis code
        /// </summary>
        /// <param name="obisCode"></param>
        /// <returns></returns>
        public DataRow GetTamperCount(string obisCode, long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select  InstantPowerColumnValue from meterdata_instantpower where ");
                builder.Append(string.Concat(InstantPowerObisCode + "=" + ParameterName(InstantPowerObisCode)));
                builder.Append(" AND ");
                builder.Append(string.Concat(MeterData_ID + "=" + ParameterName(MeterData_ID))); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(InstantPowerObisCode), obisCode, DbType.String, 40); 
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper count data Fetched"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperCount(string obisCode, long meterDataID)", ex);
            }
            if (dataSet != null && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count != 0)
                return dataSet.Tables[0].Rows[0];
            else
                return null;




        }
    }
}
