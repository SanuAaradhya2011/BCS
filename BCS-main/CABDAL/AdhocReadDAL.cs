#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.DALC.Data
{
    public class AdhocReadDAL : DALBase
    {
        #region Constants and Variables
        private string Descriptions = "Descriptions";
        private string OBISCODE = "OBISCODE";
        private string CLASS = "CLASS";
        private string ATTRIBUTE = "ATTRIBUTE";
        private string Value = "Value";
        private string Unit = "Unit";
        private string MeterData_ID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650NamePlateDAL).ToString());
        #endregion

       //*************** Adhoc read insert in database *************************
        public override IEntity InsertData(IEntity entity)
        {
            
            AdhocMasterEntity adhocMasterEntity = null;
            if (entity == null)
                return adhocMasterEntity;
            adhocMasterEntity = entity as AdhocMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_adhocread(AdhocColumnName,AdhocObisCode,AdhocClassID,AdhocAttribute,AdhocColumnValue) values(");
                builder.Append(string.Concat(ParameterName(Descriptions), ","));
                builder.Append(string.Concat(ParameterName(OBISCODE), ","));
                builder.Append(string.Concat(ParameterName(CLASS), ","));
                builder.Append(string.Concat(ParameterName(ATTRIBUTE), ","));
                builder.Append(string.Concat(ParameterName(Value), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Descriptions), adhocMasterEntity.Descriptions, DbType.String, 50);
                request.AddParamter(ParameterName(OBISCODE), adhocMasterEntity.OBISCODE, DbType.String, 35);
                request.AddParamter(ParameterName(CLASS), adhocMasterEntity.CLASS, DbType.String, 50);
                request.AddParamter(ParameterName(ATTRIBUTE), adhocMasterEntity.ATTRIBUTE, DbType.String, 35);
                request.AddParamter(ParameterName(Value), adhocMasterEntity.Value, DbType.String, 50);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)
            {
                adhocMasterEntity = null;
            }
            return adhocMasterEntity;
        }

        public override  IEntity InsertData(IList<IEntity> entities)
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

        public override  IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override System.Data.DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override  IEntity RowToEntity(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        public DataSet GetMeterData()
        {
            DataSet dataSet = new DataSet();          
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select AdhocColumnName,AdhocColumnValue,AdhocObisCode,AdhocClassID,AdhocAttribute from meterdata_adhocread");
              
                DataRequest request = new DataRequest(builder.ToString());                
                            
                dataSet = helper.FillDataSet(request, dataSet);               
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("adhoc data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int id)", ex);               
            }
            return dataSet;           
            
        }

        public bool DeleteData()
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_adhocread");
                //builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
               // request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataId)", ex);
            }
            return Flag;
        }
    }
}
