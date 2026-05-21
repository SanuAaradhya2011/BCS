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
    public class DLMS650NamePlateDAL : DALBase
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private string NamePlate_ID = "NamePlate_ID";
        private string meterSerialNumber = "meterSerialNumber";
        private string manufacturername = "manufacturername";
        private string firmwareVersionformeter = "firmwareVersionformeter";
        private string metertype = "metertype";
        private string category = "Category";
        private const string currentRating = "CurrentRating";
        private string internalCTratio = "internalCTratio";        
        private string internalVTratio = "internalVTratio";        
        private string meteryearofmanufacture = "meteryearofmanufacture";
        private string MeterData_ID = "MeterData_ID";
        private string FileName = "FileName";
        private const string ENERGYRESOLUTION = "energyResolution";
        private const string DEMANDRESOLUTION = "demandResolution";
        private const string MeterDataType = "MeterDataType";

        private const string PrimaryMeterConstantInfo = "PrimaryMeterConstantInfo";//PGVCL
        private const string MeterConstantInfo = "MeterConstantInfo";
        private const string MeterMonthOfManufacture = "MeterMonthOfManufacture";
        private const string AccuracyClass = "AccuracyClass";        

        bool IsPUMA = false;

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650NamePlateDAL).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DLMS650NamePlateDAL()
            : base("meterdata_nameplate", "NamePlate_ID")
        {
        }
        public DLMS650NamePlateDAL(bool isPUMA)
            : base("meterdata_nameplate", "NamePlate_ID")
        {
            IsPUMA = isPUMA;
        }
        #endregion

        #region Public Methods
        public override IEntity InsertData(IEntity entity)
        {
            if (entity == null)
                return entity;
            DLMS650NamePlateDetailsEntity namePlateEntity = entity as DLMS650NamePlateDetailsEntity;
            bool Flag = false;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_nameplate (meterSerialNumber,manufacturername,firmwareVersionformeter,metertype,Category,CurrentRating,internalCTratio,internalVTratio,meteryearofmanufacture,PrimaryMeterConstantInfo,MeterConstantInfo, MeterMonthOfManufacture, AccuracyClass MeterData_ID) values(");//PGVCL

                builder.Append(string.Concat(ParameterName(meterSerialNumber), ","));
                builder.Append(string.Concat(ParameterName(manufacturername), ","));
                builder.Append(string.Concat(ParameterName(firmwareVersionformeter), ","));
                builder.Append(string.Concat(ParameterName(metertype), ","));
                builder.Append(string.Concat(ParameterName(category), ","));
                builder.Append(string.Concat(ParameterName(currentRating), ","));
                builder.Append(string.Concat(ParameterName(internalCTratio), ","));
                builder.Append(string.Concat(ParameterName(internalVTratio), ","));
                builder.Append(string.Concat(ParameterName(meteryearofmanufacture), ","));

                builder.Append(string.Concat(ParameterName(PrimaryMeterConstantInfo), ","));//PGVCL
                builder.Append(string.Concat(ParameterName(MeterConstantInfo), ","));
                builder.Append(string.Concat(ParameterName(MeterMonthOfManufacture), ","));
                builder.Append(string.Concat(ParameterName(AccuracyClass), ","));                
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
               
                
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterSerialNumber), namePlateEntity.MeterSerialNumber, DbType.String, 40);
                request.AddParamter(ParameterName(manufacturername), namePlateEntity.Manufacturername, DbType.String, 40);
                request.AddParamter(ParameterName(firmwareVersionformeter), namePlateEntity.FirmwareVersionformeter, DbType.String, 40);
                request.AddParamter(ParameterName(metertype), namePlateEntity.Metertype, DbType.String, 40);
                request.AddParamter(ParameterName(category), namePlateEntity.Category, DbType.String, 40);
                request.AddParamter(ParameterName(currentRating), namePlateEntity.CurrentRating, DbType.String, 40);
                request.AddParamter(ParameterName(internalCTratio), namePlateEntity.InternalCTratio, DbType.String, 40);
                request.AddParamter(ParameterName(internalVTratio), namePlateEntity.InternalVTratio, DbType.String, 40);
                request.AddParamter(ParameterName(meteryearofmanufacture), namePlateEntity.Meteryearofmanufacture, DbType.String, 40);

                request.AddParamter(ParameterName(PrimaryMeterConstantInfo), namePlateEntity.PrimaryMeterConstant, DbType.String, 40);//PGVCL
                request.AddParamter(ParameterName(MeterConstantInfo), namePlateEntity.MeterConstant, DbType.String, 40);
                request.AddParamter(ParameterName(MeterMonthOfManufacture), namePlateEntity.MeterMonthOfManufacture, DbType.String, 40);
                request.AddParamter(ParameterName(AccuracyClass), namePlateEntity.AccuracyClass, DbType.String, 40);               
                request.AddParamter(ParameterName(MeterData_ID), namePlateEntity.MeterData_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " InsertData(IEntity entity)", ex);
                Flag = false;
            }
            if (Flag)
                namePlateEntity.Nameplate_ID = Convert.ToInt64(this.GetPK());
            return namePlateEntity;
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

        public DataSet GetMeterData(int meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.meterSerialNumber,A.manufacturername,A.firmwareVersionformeter,A.metertype,A.internalCTratio,A.internalVTratio,A.category,A.meteryearofmanufacture,CurrentRating,A.PrimaryMeterConstantInfo,A.MeterConstantInfo, A.MeterMonthOfManufacture, A.AccuracyClass");//PGVCL
                builder.Append(" from meterdata_nameplate A, MeterData B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Nameplate data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public bool DeleteData(long meterDataId)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_nameplate where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataId)", ex);
            }
            return Flag;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        #endregion
       
    }
}
