using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using MySql.Data.MySqlClient;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class MeterMasterDAL : DALBase
    {
        private string Meter_ID = "Meter_ID";
        private string MeterType_ID = "MeterType_ID";
        private string MeterModel_ID = "MeterModel_ID";
        private string Meter_EMF = "Meter_EMF";
        private string Meter_ContractDemand = "Meter_ContractDemand";
        private string MeterUnit_ID = "MeterUnit_ID";
        private string Meter_CTPrimary = "Meter_CTPrimary";
        private string Meter_CTSecondary = "Meter_CTSecondary";
        private string Meter_PTPrimary = "Meter_PTPrimary";
        private string Meter_PTSecondary = "Meter_PTSecondary";
        private string Meter_InstalledCTPrimary = "Meter_InstalledCTPrimary";
        private string Meter_InstalledCTSecondary = "Meter_InstalledCTSecondary";
        private string MeterInstalledCTRatio = "Meter_InstalledCTRatio";
        private string Meter_InstalledPTPrimary = "Meter_InstalledPTPrimary";
        private string Meter_InstalledPTSecondary = "Meter_InstalledPTSecondary";
        private string MeterInstalledPTRatio = "Meter_InstalledPTRatio";
        private string Meter_Phone = "Meter_Phone";
        private string Meter_Status = "Meter_Status";
        private string InternalPTratio = "InternalPTratio";
        private string InternalCTratio = "InternalCTratio";
        private string UpdatedOn = "UpdatedOn";
        private string UseEMFInCalculations = "UseEMFInCalculations";
        // GPRS Specific fields
        private string MeterGPRSModemIMEI = "Meter_GPRSModem_IMEI";
        private string GPRSModemConnectionType = "GPRSModemConnectionType";
        private string GPRSModemIpType = "GPRSModemIpType";
        private string CommType = "Communication_Type";
        private string MeterData_ID = "MeterData_ID";
        private string MeterSerialNumber = "meterSerialNumber";

        //BhardwajG : declare meter type
        private string MeterType = "metertype";
        private bool isPUMA = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterMasterDAL).ToString());
        public MeterMasterDAL(UtilityEntity utilityEntity)
        {
            if (utilityEntity == UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }
        public MeterMasterDAL()
        {

        }
        public override IEntity InsertData(IEntity entity)
        {
            MeterMasterEntity meterMasterEntity = null;
            if (entity == null)
                return meterMasterEntity;
            meterMasterEntity = entity as MeterMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meter_master(Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF," +
                    "Meter_ContractDemand,MeterUnit_ID,Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary," +
                    "Meter_InstalledCTPrimary,Meter_InstalledCTSecondary,Meter_InstalledCTRatio,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,Meter_InstalledPTRatio," +
                    "Meter_Phone,Meter_Status,UseEMFInCalculations,Meter_GPRSModem_IMEI,GPRSModemConnectionType,GPRSModemIpType) values(");
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterType_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterModel_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_EMF), ","));
                builder.Append(string.Concat(ParameterName(Meter_ContractDemand), ","));
                builder.Append(string.Concat(ParameterName(MeterUnit_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_CTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_CTSecondary), ","));
                builder.Append(string.Concat(ParameterName(Meter_PTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_PTSecondary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledCTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledPTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_Phone), ","));
                builder.Append(string.Concat(ParameterName(Meter_Status), ","));
                builder.Append(string.Concat(ParameterName(UseEMFInCalculations), ","));
                // GPRS specific fields
                builder.Append(string.Concat(ParameterName(MeterGPRSModemIMEI), ","));
                builder.Append(string.Concat(ParameterName(GPRSModemConnectionType), ","));
                builder.Append(string.Concat(ParameterName(GPRSModemIpType), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF.ToString(), DbType.String);
                request.AddParamter(ParameterName(Meter_ContractDemand), meterMasterEntity.Meter_ContractDemand, DbType.Double);
                request.AddParamter(ParameterName(MeterUnit_ID), meterMasterEntity.MeterUnit_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_CTPrimary), meterMasterEntity.Meter_CTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_CTSecondary), meterMasterEntity.Meter_CTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_PTPrimary), meterMasterEntity.Meter_PTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_PTSecondary), meterMasterEntity.Meter_PTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledCTRatio), meterMasterEntity.MeterInstalledCTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledPTRatio), meterMasterEntity.MeterInstalledPTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_Phone), meterMasterEntity.Meter_Phone, DbType.String, 15);
                request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
                request.AddParamter(ParameterName(UseEMFInCalculations), meterMasterEntity.UseEMFInCalculations, DbType.Int32);
                request.AddParamter(ParameterName(MeterGPRSModemIMEI), meterMasterEntity.MeterGPRSModemIMEI, DbType.String,16);
                request.AddParamter(ParameterName(GPRSModemConnectionType), meterMasterEntity.GPRSModemConnectionType, DbType.Boolean);
                request.AddParamter(ParameterName(GPRSModemIpType), meterMasterEntity.GPRSModemIpType, DbType.Int16);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data inserted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meter_master Set ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(MeterType_ID, "=", ParameterName(MeterType_ID), ","));
                builder.Append(string.Concat(MeterModel_ID, "=", ParameterName(MeterModel_ID), ","));
                builder.Append(string.Concat(Meter_EMF, "=", ParameterName(Meter_EMF), ","));
                builder.Append(string.Concat(Meter_ContractDemand, "=", ParameterName(Meter_ContractDemand), ","));
                builder.Append(string.Concat(MeterUnit_ID, "=", ParameterName(MeterUnit_ID), ","));
                builder.Append(string.Concat(Meter_CTPrimary, "=", ParameterName(Meter_CTPrimary), ","));
                builder.Append(string.Concat(Meter_CTSecondary, "=", ParameterName(Meter_CTSecondary), ","));
                builder.Append(string.Concat(Meter_PTPrimary, "=", ParameterName(Meter_PTPrimary), ","));
                builder.Append(string.Concat(Meter_PTSecondary, "=", ParameterName(Meter_PTSecondary), ","));
                builder.Append(string.Concat(Meter_InstalledCTPrimary, "=", ParameterName(Meter_InstalledCTPrimary), ","));
                builder.Append(string.Concat(Meter_InstalledCTSecondary, "=", ParameterName(Meter_InstalledCTSecondary), ","));
                builder.Append(string.Concat(MeterInstalledCTRatio, "=", ParameterName(MeterInstalledCTRatio), ","));
                builder.Append(string.Concat(Meter_InstalledPTPrimary, "=", ParameterName(Meter_InstalledPTPrimary), ","));
                builder.Append(string.Concat(Meter_InstalledPTSecondary, "=", ParameterName(Meter_InstalledPTSecondary), ","));
                builder.Append(string.Concat(MeterInstalledPTRatio, "=", ParameterName(MeterInstalledPTRatio), ","));
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone), ","));
                builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status), ","));
                builder.Append(string.Concat(UseEMFInCalculations, "=", ParameterName(UseEMFInCalculations), ","));
                builder.Append(string.Concat(MeterGPRSModemIMEI, "=", ParameterName(MeterGPRSModemIMEI), ","));
                builder.Append(string.Concat(GPRSModemConnectionType, "=", ParameterName(GPRSModemConnectionType), ","));
                builder.Append(string.Concat(GPRSModemIpType, "=", ParameterName(GPRSModemIpType)));
         

                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF.ToString(), DbType.String);
                request.AddParamter(ParameterName(Meter_ContractDemand), meterMasterEntity.Meter_ContractDemand, DbType.Double);
                request.AddParamter(ParameterName(MeterUnit_ID), meterMasterEntity.MeterUnit_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_CTPrimary), meterMasterEntity.Meter_CTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_CTSecondary), meterMasterEntity.Meter_CTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_PTPrimary), meterMasterEntity.Meter_PTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_PTSecondary), meterMasterEntity.Meter_PTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledCTRatio), meterMasterEntity.MeterInstalledCTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledPTRatio), meterMasterEntity.MeterInstalledPTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_Phone), meterMasterEntity.Meter_Phone, DbType.String, 15);
                request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
                request.AddParamter(ParameterName(UseEMFInCalculations), meterMasterEntity.UseEMFInCalculations, DbType.Int32);
                request.AddParamter(ParameterName(MeterGPRSModemIMEI), meterMasterEntity.MeterGPRSModemIMEI, DbType.String);
                request.AddParamter(ParameterName(GPRSModemConnectionType), meterMasterEntity.UseEMFInCalculations, DbType.Int16);
                request.AddParamter(ParameterName(GPRSModemIpType), meterMasterEntity.GPRSModemIpType, DbType.Boolean);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data updated"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        /// <summary>
        /// Update meter Model and meter type.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateMeterModelAndType(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meter_master Set ");               
                builder.Append(string.Concat(MeterType_ID, "=", ParameterName(MeterType_ID), ","));
                builder.Append(string.Concat(MeterModel_ID, "=", ParameterName(MeterModel_ID)));
                

                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);                
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data updated"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateMeterModelAndType(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public IEntity InsertDataIntoLog(IEntity entity)
        {
            MeterMasterEntity meterMasterEntity = null;
            if (entity == null)
                return meterMasterEntity;
            meterMasterEntity = entity as MeterMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meter_master_log(Meter_ID," +
                    "Meter_InstalledCTPrimary,Meter_InstalledCTSecondary,Meter_InstalledCTRatio,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,Meter_InstalledPTRatio," +
                    "Meter_EMF,UpdatedOn) values(");
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledCTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledPTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_EMF), ","));
                builder.Append(string.Concat(ParameterName(UpdatedOn), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledCTRatio), meterMasterEntity.MeterInstalledCTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
                request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
                request.AddParamter(ParameterName(MeterInstalledPTRatio), meterMasterEntity.MeterInstalledPTRatio, DbType.Int32);
                request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF.ToString(), DbType.String);
                request.AddParamter(ParameterName(UpdatedOn), DateUtility.DateTimeToLong(System.DateTime.Now), DbType.Int64);

                helper.ExecuteNonQuery(request);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertDataIntoLog(IEntity entity)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meter_master Set ");
                builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status)));
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public bool DeleteMeterData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meter_master ");
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteMeterData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            MeterMasterEntity meterMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF,Meter_Phone,Meter_ContractDemand,MeterUnit_ID," +
                "Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary,Meter_InstalledCTPrimary," +
                "Meter_InstalledCTSecondary,MeterInstalledCTRatio,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,MeterInstalledPTRatio,Meter_Status,UseEMFInCalculations from meter_master where ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), id, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    meterMasterEntity = (MeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }     
        public IEntity GetDetailData(string meter_ID, int status)
        {
            MeterMasterEntity meterMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //BhardwajG : Get meter type along with the general data
                builder.Append("Select distinct A.Meter_ID,A.MeterType_ID,A.MeterModel_ID,A.Meter_EMF,A.Meter_Phone,A.Meter_ContractDemand,A.MeterUnit_ID, " +
                "A.Meter_CTPrimary,A.Meter_CTSecondary,A.Meter_PTPrimary,A.Meter_PTSecondary,A.Meter_InstalledCTPrimary,A.Meter_InstalledCTSecondary,A.Meter_InstalledCTRatio, " +
                "A.Meter_InstalledPTPrimary,A.Meter_InstalledPTSecondary,A.Meter_InstalledPTRatio,A.Meter_Status,C.InternalPTratio,C.InternalCTratio,C.MeterType,A.UseEMFInCalculations,A.Meter_GPRSModem_IMEI from meter_master A left outer join meterdata B on A.Meter_ID = B.MeterID left outer join meterdata_general C on B.MeterData_ID = C.MeterData_ID where ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)) + " and ");
                builder.Append(string.Concat(Meter_Status, "=", ParameterName(Meter_Status)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_Status), status, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables[0].Rows.Count > 0)
                        meterMasterEntity = (MeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string meter_ID, int status)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }
        public IEntity GetInternalCTPT(string meter_ID)
        {
            MeterMasterEntity meterMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.internalCTratio,A.internalPTratio from meterdata_general A join meterdata B on A.MeterData_ID = B.MeterData_ID where ");
                builder.Append(string.Concat("B.MeterID", "=", ParameterName(Meter_ID)));
                builder.Append(" order by B.UploadingDateTime desc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);

                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                    if (ds.Tables[0].Rows.Count > 0)
                        meterMasterEntity = (MeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInternalCTPT(string meter_ID)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }
        public IEntity GetDetailInactiveMeterData(string meter_ID)
        {
            MeterMasterEntity meterMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select M.Meter_ID,MT.MeterType_Name,MM.MeterModel_Name,M.Meter_EMF,M.Meter_Phone,M.Meter_ContractDemand, " +
                            "M.Meter_CTPrimary,M.Meter_CTSecondary,M.Meter_PTPrimary,M.Meter_PTSecondary,M.Meter_InstalledCTPrimary,M.Meter_InstalledCTSecondary, " +
                            "M.Meter_InstalledPTPrimary,M.Meter_InstalledPTSecondary,M.Meter_Status " +
                            "from meter_master M Inner Join metertype_master MT on M.MeterType_ID = MT.MeterType_ID " +
                            "Inner Join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID where M.Meter_Status = 0 and M.");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meter_ID, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    meterMasterEntity = (MeterMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified inactive meter viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailInactiveMeterData(string meter_ID)", ex);
                meterMasterEntity = null;
            }
            return meterMasterEntity;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// To retrieve the values of active Meters 
        /// </summary>
        /// <returns></returns>

        public override DataSet ListDataSet()
        {
            DataSet ds = null;
            int internalCTRatio = 0;
            int internalPTRatio = 0;
            string meterType = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                //if meter_master.Meter_Status = 1  and consumermeter.Status = 1.then the active Meters are selected else if 0 is checked then Inactive meters are selected.
                //but the queries for selecting the active and inactive meters are different since the consumer meter and 
                //consumer_Master will not contain in the query.

                builder.Append("select CM.Consumer_Number as 'Consumer ID',CM.Meter_ID as 'Meter ID',C.Consumer_Name as 'Consumer Name',CT.ConsumerType_Name as 'Consumer Type',C.Consumer_Phone as 'Consumer Phone',C.Consumer_HNumber as 'Consumer House No.', " +
                            "C.Consumer_Street as 'Consumer Street',C.Consumer_City as 'Consumer City',CM.communication_Type as 'Communication Type',");
                if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                {
                    builder.Append("R.Region_Name as 'Region Name',CI.Circle_Name as 'Circle Name',D.division_name as 'Division Name',");
                }
                // following statement was changed to export Meter CT and PT Ratio; 12 April 2012: "G.internalCTratio as 'Internal CT Ratio', G.internalPTratio as 'Internal PT Ratio" changed
                builder.Append("C.Consumer_Email as 'Consumer Email',MT.MeterType_Name as 'Meter Type',MM.MeterModel_Name as 'Meter Model',M.Meter_EMF as 'Meter EMF',M.Meter_Phone as 'Meter SIM No.' , M.Meter_GPRSModem_IMEI 'Modem IMEI',"
               + " M.Meter_ContractDemand as 'Meter Contract Demand', " +
               "MU.MeterUnit_Type as 'Meter Unit', M.Meter_CTPrimary as 'Internal CT Ratio', M.Meter_PTPrimary as 'Internal PT Ratio', M.Meter_InstalledCTPrimary as 'Installed CT Primary', " +
               "M.Meter_InstalledCTSecondary as 'Installed CT Secondary',M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',M.Meter_Status as 'Meter Status',CM.Meter_AllocationDate as 'Meter Allocation Date',CM.Meter_Location as 'Location' " +
               "from consumer_master C inner join consumermeter CM on C.Consumer_Number = CM.Consumer_Number " +
               "inner join meter_master M on CM.Meter_ID = M.Meter_ID " +
               "left outer join meterdata MD on MD.MeterID = M.Meter_ID " +
               "left outer join meterdata_general G on G.MeterData_ID = MD.MeterData_ID " +
               "inner join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID " +
               "inner join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID " +
               "inner join metertype_master MT on M.MeterType_ID = MT.MeterType_ID " +
               "inner join consumertype_master CT on C.ConsumerType_ID = CT.ConsumerType_ID");
                if (ConfigInfo.GetTenderType() == TenderType.JUSCO)
                {
                    builder.Append(" LEFT OUTER JOIN region_master R ON CM.Region_ID = R.Region_ID ");
                    builder.Append("LEFT OUTER JOIN circle_master CI ON CM.Circle_ID = CI.Circle_ID ");
                    builder.Append("LEFT OUTER JOIN division_master D on CM.Division_ID = D.Division_ID ");
                }
                builder.Append(" where M.Meter_Status = 1 and CM.status = 1 Group by CM.Meter_ID");
                //"and MD.UploadingDateTime = (Select Max(UploadingDateTime) from meterdata)");
                DataRequest request = new DataRequest(builder.ToString());
                int rCount = 0;
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null && ds.Tables.Count > 0)
                    rCount = ds.Tables[0].Rows.Count - 1;

                //while(rCount>0)
                //{
                //    ds.Tables[0].Rows[0].Delete();
                //    ds.AcceptChanges();
                //    rCount--;
                //}
                if (rCount > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        GetLatestInternalCTPTRatio(row["Meter ID"].ToString(), out internalCTRatio, out internalPTRatio, out meterType);
                        if (internalCTRatio > 0)
                            row["Internal CT Ratio"] = internalCTRatio;
                        else
                            row["Internal CT Ratio"] = "1";
                        if (internalPTRatio > 0)
                            row["Internal PT Ratio"] = internalPTRatio;
                        else
                            row["Internal PT Ratio"] = "1";
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data viewed"));
                //if (ds.Tables[0].Rows.Count > 0)
                //    return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                ds = null;
            }
            return ds;
        }
        public void GetLatestInternalCTPTRatio(string meterID, out int internalCTRatio, out int internalPTRatio,out string meterType)
        {
            internalCTRatio = 1;
            internalPTRatio = 1;
            meterType = string.Empty;
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.internalCTratio, g.internalPTratio,g.metertype from meterdata_general g inner join meterdata d on d.MeterData_ID = g.MeterData_ID where d.");
                builder.Append(string.Concat("MeterID", "=", ParameterName("MeterID")));
                builder.Append(" order by d.UploadingDateTime desc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter("MeterID", meterID, DbType.String);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for inactive meters viewed"));
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int.TryParse(ds.Tables[0].Rows[0]["internalCTratio"].ToString(), out internalCTRatio);
                            int.TryParse(ds.Tables[0].Rows[0]["internalPTratio"].ToString(), out internalPTRatio);
                            //BhardwajG : Get meter type for displaying on consumer UI
                            meterType = ds.Tables[0].Rows[0]["metertype"].ToString();
                        }
                    }
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLatestInternalCTPTRatio(string meterID, out int internalCTRatio, out int internalPTRatio,out string meterType)", ex);
            }
        }

        /// <summary>
        /// To retrieve Inactive Meters alone
        /// </summary>
        /// <returns></returns>
        public DataSet ListInactiveMeterDataSet()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select M.Meter_ID as 'Meter ID',MT.MeterType_Name as 'Meter Type',MM.MeterModel_Name as 'Meter Model',M.Meter_EMF as 'Meter EMF',M.Meter_Phone as 'Meter SIM No.',M.Meter_GPRSModem_IMEI as 'Modem IMEI' ,M.Meter_ContractDemand as 'Meter Contract Demand', " +
                            "MU.MeterUnit_Type as 'Meter Unit',M.Meter_CTPrimary as 'Meter CT Primary',M.Meter_CTSecondary as 'Meter CT Secondary',M.Meter_PTPrimary as 'Meter PT Primary',M.Meter_PTSecondary as 'Meter PT Secondary',M.Meter_InstalledCTPrimary as 'Installed CT Primary', " +
                            "M.Meter_InstalledCTSecondary as 'Installed CT Secondary',M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',M.Meter_Status as 'Meter Status' " +
                            "from meter_master M inner join metermodel_master MM on M.MeterModel_ID = MM.MeterModel_ID " +
                            "inner join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID " +
                            "inner join metertype_master MT on M.MeterType_ID = MT.MeterType_ID where M.Meter_Status = 0");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for inactive meters viewed"));
                if (ds.Tables[0].Rows.Count > 0)
                    return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListInactiveMeterDataSet()", ex);
                ds = null;
            }
            return ds;
        }

        public DataSet ListInactiveMeterID()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from meter_master where Meter_Status = 0");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID for inactive meters retrieved"));
                if (ds.Tables[0].Rows.Count > 0)
                    return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListInactiveMeterID()", ex);
                ds = null;
            }
            return ds;
        }

        /// <summary>
        ///  endpoint for bulk migration to GPRS adapter layer
        /// </summary>
        /// <returns></returns>
        public static DataSet GetEndPointsForBulkMigrate()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                builder.Append("SELECT `meter_master`.METER_ID,METERMODEL_ID,METER_GPRSModem_IMEI FROM `meter_master`,`consumermeter` where `meter_master`.METER_ID=`consumermeter`.METER_ID and UPPER(Communication_Type)='GPRS' and IsSyncedWithGPRSAdapter=0 LIMIT 0,50");
               
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
              
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetEndPointsForBulkMigrate()", ex);
                ds = null;
            }
            
            return ds;
        }

        /// <summary>
        /// Method to set the statuses of meter id list passed
        /// </summary>
        /// <param name="syncedMeterIdXml"></param>
        /// <param name="syncedEndpointCounter"></param>
        public static DataSet BulkUpdateEndPointSyncStatus(StringBuilder syncedMeterIdXml, int syncedEndpointCounter)
        {
            DataSet dataSet = new DataSet();
            try
            {

                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("BulkUpdateEndPointSyncStatus", CommandType.StoredProcedure);
                request.AddParamter(ParameterName("meterIdsList"), syncedMeterIdXml.ToString(), DbType.String, 2000);
                request.AddParamter(ParameterName("count"), syncedEndpointCounter, DbType.Int32, 20);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BulkUpdateEndPointSyncStatus(StringBuilder syncedMeterIdXml, int syncedEndpointCounter)", ex);
            }

            return dataSet;
        }

      
        /// <summary>
        /// To retrieve Free Consumers alone
        /// </summary>
        /// <returns></returns>
        public DataSet ListFreeConsumersDataSet()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("Select distinct D.Consumer_Number as 'Consumer ID',D.Consumer_Name as 'Consumer Name',CT.ConsumerType_Name as 'Consumer Type',D.Consumer_Phone as 'Consumer Phone',D.Consumer_HNumber as 'Consumer House No.',D.Consumer_Street as 'Consumer Street', " +
                            "D.Consumer_City as 'Consumer City',D.Consumer_Email as 'Consumer Email' from Consumermeter CM " +
                            "inner join consumer_master D on CM.Consumer_Number = D.Consumer_Number " +
                            "inner join ConsumerType_master CT on D.ConsumerType_ID = CT.ConsumerType_ID where CM.Consumer_Number Not in (Select distinct Consumer_Number from Consumermeter where Status = 1)");
                //builder.Append("Select distinct D.Consumer_Number as 'Consumer ID',D.Consumer_Name as 'Consumer Name',");
                //builder.Append("CT.ConsumerType_Name as 'Consumer Type',D.Consumer_Phone as 'Consumer Phone',");
                //builder.Append("D.Consumer_HNumber as 'Consumer House No.',D.Consumer_Street as 'Consumer Street',");
                //builder.Append("D.Consumer_City as 'Consumer City',D.Consumer_Email as 'Consumer Email' from Consumermeter CM,");
                //builder.Append("consumer_master D,ConsumerType_master CT where CM.Consumer_Number = D.Consumer_Number ");
                //builder.Append("and D.ConsumerType_ID = CT.ConsumerType_ID and  CM.Status = 0");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of free consumers retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListFreeConsumersDataSet()", ex);
                ds = null;
            }
            return ds;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            int internalPTRatio = 0;
            int internalCTratio = 0;
            if (row == null) return null;
            MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
            if (NotNullAndNotDBNull(row, Meter_ID)) meterMasterEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
            if (NotNullAndNotDBNull(row, MeterType_ID)) meterMasterEntity.MeterType_ID = Convert.ToInt32(row[MeterType_ID]);
            if (NotNullAndNotDBNull(row, MeterModel_ID)) meterMasterEntity.MeterModel_ID = Convert.ToInt32(row[MeterModel_ID]);
            if (NotNullAndNotDBNull(row, Meter_EMF)) meterMasterEntity.Meter_EMF = Convert.ToDecimal(row[Meter_EMF]);
            if (NotNullAndNotDBNull(row, Meter_ContractDemand)) meterMasterEntity.Meter_ContractDemand = Convert.ToDouble(row[Meter_ContractDemand]);
            if (NotNullAndNotDBNull(row, MeterUnit_ID)) meterMasterEntity.MeterUnit_ID = Convert.ToInt32(row[MeterUnit_ID]);
            if (NotNullAndNotDBNull(row, Meter_CTPrimary)) meterMasterEntity.Meter_CTPrimary = Convert.ToInt32(row[Meter_CTPrimary]);
            if (NotNullAndNotDBNull(row, Meter_CTSecondary)) meterMasterEntity.Meter_CTSecondary = Convert.ToInt32(row[Meter_CTSecondary]);
            if (NotNullAndNotDBNull(row, Meter_PTPrimary)) meterMasterEntity.Meter_PTPrimary = Convert.ToInt32(row[Meter_PTPrimary]);
            if (NotNullAndNotDBNull(row, Meter_PTSecondary)) meterMasterEntity.Meter_PTSecondary = Convert.ToInt32(row[Meter_PTSecondary]);
            if (NotNullAndNotDBNull(row, Meter_InstalledCTPrimary)) meterMasterEntity.Meter_InstalledCTPrimary = Convert.ToInt32(row[Meter_InstalledCTPrimary]);
            if (NotNullAndNotDBNull(row, Meter_InstalledCTSecondary)) meterMasterEntity.Meter_InstalledCTSecondary = Convert.ToInt32(row[Meter_InstalledCTSecondary]);
            if (NotNullAndNotDBNull(row, MeterInstalledCTRatio)) meterMasterEntity.MeterInstalledCTRatio = Convert.ToInt32(row[MeterInstalledCTRatio]);
            if (NotNullAndNotDBNull(row, Meter_InstalledPTPrimary)) meterMasterEntity.Meter_InstalledPTPrimary = Convert.ToInt32(row[Meter_InstalledPTPrimary]);
            if (NotNullAndNotDBNull(row, Meter_InstalledPTSecondary)) meterMasterEntity.Meter_InstalledPTSecondary = Convert.ToInt32(row[Meter_InstalledPTSecondary]);
            if (NotNullAndNotDBNull(row, MeterInstalledPTRatio)) meterMasterEntity.MeterInstalledPTRatio = Convert.ToInt32(row[MeterInstalledPTRatio]);
            if (NotNullAndNotDBNull(row, Meter_Phone)) meterMasterEntity.Meter_Phone = Convert.ToString(row[Meter_Phone]);
            if (NotNullAndNotDBNull(row, Meter_Status)) meterMasterEntity.Meter_Status = Convert.ToInt32(row[Meter_Status]);
            if (NotNullAndNotDBNull(row, UseEMFInCalculations)) meterMasterEntity.UseEMFInCalculations = Convert.ToInt32(row[UseEMFInCalculations]);
            if (NotNullAndNotDBNull(row, MeterType)) meterMasterEntity.MeterType = Convert.ToString(row[MeterType]);
            if (NotNullAndNotDBNull(row, MeterGPRSModemIMEI)) meterMasterEntity.MeterGPRSModemIMEI = Convert.ToString(row[MeterGPRSModemIMEI]);

            if (int.TryParse(row[InternalPTratio].ToString(), out internalPTRatio))
                meterMasterEntity.MeterPTRatio = internalPTRatio;
            else
                meterMasterEntity.MeterPTRatio = 1;
            if (NotNullAndNotDBNull(row, InternalCTratio))
            {
                if (int.TryParse(row[InternalCTratio].ToString(), out internalCTratio))
                {
                    meterMasterEntity.MeterCTRatio = internalCTratio;
                }
            }
            return meterMasterEntity;
        }


        public bool ValidateMeterNumber(IEntity entity)
        {
            bool Flag = false;
            try
            {
                MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from meter_master");
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateMeterNumber(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Meter_ID"></param>
        /// <returns></returns>
        public bool ValidateMeterNumber(string meterID)
        {
            bool Flag = false;
            try
            {
              
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from meter_master");
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterID, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateMeterNumber(string meterID)", ex);
                Flag = false;
            }
            return Flag;
        }

        /// <summary>
        /// To check the existence of IMEI number
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsIMEIAlreadyExists(string meter_GPRSModem_IMEI)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from meter_master");
                builder.Append(string.Concat(" Where ", MeterGPRSModemIMEI, "=", ParameterName(MeterGPRSModemIMEI)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterGPRSModemIMEI), meter_GPRSModem_IMEI, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if (Convert.ToInt64(data.ToString()) > 0)
                {
                    Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified IMEI retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsIMEIAlreadyExists(string meter_GPRSModem_IMEI)", ex);
                Flag = false;
            }
            return Flag;
        }
        //added on 12 May 2010

        /// <summary>
        /// To retrieve all the existing Meter ID's
        /// </summary>
        /// <returns></returns>
        public DataSet ListMeterID()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from meter_master union select MeterID from meterdata");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of Meter ID's retrieved"));
                if (ds.Tables[0].Rows.Count > 0)
                    return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListMeterID()", ex);
                ds = null;
            }
            return ds;
        }

        public DataSet ListUnAssignedAreaMeterID()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("select Meter_ID from meter_master where Meter_ID not in (select Meter_ID from areameter_master) ");
                builder.Append("union select MeterID from meterdata where MeterID not in (select Meter_ID from areameter_master)");

                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("List of unassigned Meter ID's retrieved"));
                if (ds.Tables[0].Rows.Count > 0)
                    return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListUnAssignedAreaMeterID()", ex);
                ds = null;
            }
            return ds;
        }

        //added on 12 May 2010


        public int GetEMF(long meterDataID)
        {
            int val = 1;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.Meter_emf from meter_master A,meterdata B Where A.Meter_ID=B.MeterID and B.MeterData_ID=");
                builder.Append(ParameterName("MeterData_ID"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);
                object obj = helper.ExecuteScalar(request);
                if (obj != null)
                    val = Convert.ToInt32(obj);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetEMF(long meterDataID)", ex);
                val = 1;
            }
            return val;
        }
    
        public IEntity GetMultiplyingFactors(long meterDataID)
        {
            // set default values to 1
            MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
            meterMasterEntity.MeterInstalledCTRatio = 1;
            meterMasterEntity.MeterInstalledPTRatio = 1;
            meterMasterEntity.Meter_EMF = 1;
            meterMasterEntity.InternalCTRatio = 1;
            meterMasterEntity.InternalPTRatio = 1;
            int internalCTRatio = 1;
            int internalPTRatio = 1;
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.Meter_ID,A.Meter_InstalledCTRatio,A.Meter_InstalledPTRatio,A.Meter_emf");
                //BhardwajG : TFS ID : 117997 : EMF Enhancement required
                // Add internal ct and pt ratio
                builder.Append(",G.internalCTRatio,G.internalPTRatio");
                builder.Append(" from meter_master A,meterdata B");
                builder.Append(",meterdata_general G");
                builder.Append(" Where A.Meter_ID=B.MeterID");
                builder.Append(" and B.MeterData_ID = G.MeterData_ID");
                builder.Append(" and B.MeterData_ID=");
                builder.Append(ParameterName("MeterData_ID"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                // Get installed CT PT ratio for the meterid
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        meterMasterEntity.Meter_ID = dataSet.Tables[0].Rows[0][0].ToString();
                        meterMasterEntity.MeterInstalledCTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) : 1;
                        meterMasterEntity.MeterInstalledPTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][2]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][2]) : 1;
                        meterMasterEntity.Meter_EMF = Convert.ToDecimal(dataSet.Tables[0].Rows[0][3]) > 0 ? Convert.ToDecimal(dataSet.Tables[0].Rows[0][3]) : 1;
                        //meterMasterEntity.InternalCTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][4]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][4]) : 1;
                        //meterMasterEntity.InternalPTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][5]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][5]) : 1;
                        //BhardwajG : Try parsing the value and if parsed to int then assign value if not parsed then default to 1.
                        if (int.TryParse(dataSet.Tables[0].Rows[0][4].ToString(), out internalCTRatio))
                        {
                            meterMasterEntity.InternalCTRatio = internalCTRatio;
                        }                       
                        if (int.TryParse(dataSet.Tables[0].Rows[0][5].ToString(), out internalPTRatio))
                        {
                            meterMasterEntity.InternalPTRatio = internalPTRatio;
                        }                       
                    }
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMultiplyingFactors(long meterDataID)", ex);
                return null;
            }
            return meterMasterEntity;
        }
        
        public bool IsEMFUseinCalculation(long meterDataID)
        {
            int val = 1;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.UseEMFInCalculations from meter_master A,meterdata B Where A.Meter_ID=B.MeterID and B.MeterData_ID=");
                builder.Append(ParameterName("MeterData_ID"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);
                object obj = helper.ExecuteScalar(request);
                if (obj != null)
                    val = Convert.ToInt32(obj);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsEMFUseinCalculation(long meterDataID)", ex);
                val = 1;
            }
            if (val == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This function retrieves the consumer numbers associated with a particular phone number from the database.
        /// </summary>
        /// <param name="meterNumber"></param>
        /// <returns>Theresulting consumer number</returns>
        public string GetMeterNumber(long phoneNumber)
        {
            string returnValue = string.Empty;
            try
            {

                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_ID from meter_master where ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), phoneNumber, DbType.Int64);
                object data = helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved."));
                returnValue = Convert.ToString(data);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterNumber(long phoneNumber)", ex);
                returnValue = string.Empty;
            }

            return returnValue;
        }

        /// <summary>
        /// method to retrieve IMEI associated with IMEI
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public string GetMeterNumber(string IMEI)
        {
            string returnValue = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_ID from meter_master where ");
                builder.Append(string.Concat(MeterGPRSModemIMEI, "=", ParameterName(MeterGPRSModemIMEI)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterGPRSModemIMEI), IMEI, DbType.String);
                object data = helper.ExecuteScalar(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved."));
                returnValue = Convert.ToString(data);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterNumber(string IMEI)", ex);
                returnValue = String.Empty;
            }

            return returnValue;
        }
        /// <summary>
        /// List All Meter id and corresponding Sim Numbers
        /// </summary>
        /// <returns></returns>
        public DataSet GetMeterIdAndSimNumber(string communicationType)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // for GPRS communication,IMEI number need to be displayed
                if (communicationType == CommunicationType.GPRS.ToString())
                {
                    builder.Append("select  MM.Meter_ID as 'Meter Id',Meter_GPRSModem_IMEI as 'Modem IMEI' from meter_master MM,");
                }
                else if (communicationType == CommunicationType.TCP.ToString())
                {
                    builder.Append("select  MM.Meter_ID as 'Meter Id',Meter_GPRSModem_IMEI as 'Modem IP' from meter_master MM,");
                }
                else
                {
                    builder.Append("select MM.Meter_ID as 'Meter Id' ,Meter_Phone as 'Sim Number' from meter_master MM,");
                }

                builder.Append("consumermeter CM Where MM.Meter_ID=CM.Meter_ID and CM.Communication_Type=");
                builder.Append(ParameterName("Communication_Type"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("Communication_Type"), communicationType, DbType.String,20);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID and Sim Number retrieved"));
                if (ds!=null && ds.Tables != null && ds.Tables[0].Rows.Count == 0)
                    ds = null;
                return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterIdAndSimNumber(string communicationType)", ex);
                ds = null;
            }
            return ds;
        }

       

        /// <summary>
        /// check for EMF Use in Calculation for MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public bool IsEMFUseinCalculationforMeterID(string meterID)
        {
            int val = 1;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.UseEMFInCalculations from meter_master A,meterdata B Where A.Meter_ID=B.MeterID and B.MeterID=");
                builder.Append(ParameterName("MeterID"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterID"), meterID, DbType.String);
                object obj = helper.ExecuteScalar(request);
                if (obj != null)
                    val = Convert.ToInt32(obj);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsEMFUseinCalculationforMeterID(string meterID)", ex);
                val = 1;
            }
            if (val == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// checks for Get Multiplying Factors For MeterID
        /// </summary>
        /// <param name="meterID">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public IEntity GetMultiplyingFactorsForMeterID(string meterID)
        {
            // set default values to 1
            MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
            meterMasterEntity.MeterInstalledCTRatio = 1;
            meterMasterEntity.MeterInstalledPTRatio = 1;
            meterMasterEntity.Meter_EMF = 1;
            meterMasterEntity.InternalCTRatio = 1;
            meterMasterEntity.InternalPTRatio = 1;
            int internalCTRatio = 1;
            int internalPTRatio = 1;
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.Meter_ID,A.Meter_InstalledCTRatio,A.Meter_InstalledPTRatio,A.Meter_emf");
                //BhardwajG : TFS ID : 117997 : EMF Enhancement required
                // Add internal ct and pt ratio
                builder.Append(",G.internalCTRatio,G.internalPTRatio");
                builder.Append(" from meter_master A,meterdata B");
                builder.Append(",meterdata_general G");
                builder.Append(" Where A.Meter_ID=B.MeterID");
                builder.Append(" and B.MeterID = G.meterSerialNumber");
                builder.Append(" and B.MeterID=");
                builder.Append(ParameterName("MeterID"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterID"), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                // Get installed CT PT ratio for the meterid
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        meterMasterEntity.Meter_ID = dataSet.Tables[0].Rows[0][0].ToString();
                        meterMasterEntity.MeterInstalledCTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) : 1;
                        meterMasterEntity.MeterInstalledPTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][2]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][2]) : 1;
                        meterMasterEntity.Meter_EMF = Convert.ToDecimal(dataSet.Tables[0].Rows[0][3]) > 0 ? Convert.ToDecimal(dataSet.Tables[0].Rows[0][3]) : 1;
                        //meterMasterEntity.InternalCTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][4]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][4]) : 1;
                        //meterMasterEntity.InternalPTRatio = Convert.ToInt32(dataSet.Tables[0].Rows[0][5]) > 0 ? Convert.ToInt32(dataSet.Tables[0].Rows[0][5]) : 1;
                        //BhardwajG : Try parsing the value and if parsed to int then assign value if not parsed then default to 1.
                        if (int.TryParse(dataSet.Tables[0].Rows[0][4].ToString(), out internalCTRatio))
                        {
                            meterMasterEntity.InternalCTRatio = internalCTRatio;
                        }
                        if (int.TryParse(dataSet.Tables[0].Rows[0][5].ToString(), out internalPTRatio))
                        {
                            meterMasterEntity.InternalPTRatio = internalPTRatio;
                        }
                    }
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMultiplyingFactorsForMeterID(string meterID)", ex);
                return null;
            }
            return meterMasterEntity;
        }

        /// <summary>
        /// get dataset for meter number and sim number 
        /// </summary>
        public DataSet GetMeterDetails()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // for GPRS communication,IMEI number need to be displayed

                builder.Append("SELECT Meter_ID , Meter_Phone from meter_master");
                
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID and Sim Number retrieved"));
                
                return ds;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDetails()", ex);
                ds = null;
            }
            return ds;
        }


        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public  void BatchInsert(IList<IEntity> entities)
        {
           // List<DataRequest> requests = new List<DataRequest>();
            DataTable table = new DataTable();
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            table.Columns.Add(new DataColumn("Meter_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("MeterType_ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("MeterModel_ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_EMF", typeof(System.String)));
            table.Columns.Add(new DataColumn("Meter_ContractDemand", typeof(System.Double)));
            table.Columns.Add(new DataColumn("MeterUnit_ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_CTPrimary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_CTSecondary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_PTPrimary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_PTSecondary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_InstalledCTPrimary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_InstalledCTSecondary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_InstalledCTRatio", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Meter_InstalledPTPrimary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_InstalledPTSecondary", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_InstalledPTRatio", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Meter_Phone", typeof(System.String)));
            table.Columns.Add(new DataColumn("Meter_Status", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("UseEMFInCalculations", typeof(System.Int32)));
            table.Columns.Add(new DataColumn(MeterGPRSModemIMEI, typeof(System.String)));
            table.Columns.Add(new DataColumn("GPRSModemConnectionType", typeof(System.Boolean)));
            table.Columns.Add(new DataColumn("GPRSModemIpType", typeof(System.Int16)));
            try
            {
                foreach (IEntity entity in entities)
                {
                    MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;
                    DataRow dr = table.NewRow();
                    dr["Meter_ID"] = meterMasterEntity.Meter_ID;
                    dr["MeterType_ID"] = meterMasterEntity.MeterType_ID;
                    dr["MeterModel_ID"] = meterMasterEntity.MeterModel_ID;
                    dr["Meter_EMF"] = meterMasterEntity.Meter_EMF;
                    dr["Meter_ContractDemand"] = meterMasterEntity.Meter_ContractDemand;
                    dr["MeterUnit_ID"] = meterMasterEntity.MeterUnit_ID;
                    dr["Meter_CTPrimary"] = meterMasterEntity.Meter_CTPrimary;
                    dr["Meter_CTSecondary"] = meterMasterEntity.Meter_CTSecondary;
                    dr["Meter_PTPrimary"] = meterMasterEntity.Meter_PTPrimary;
                    dr["Meter_PTSecondary"] = meterMasterEntity.Meter_PTSecondary;
                    dr["Meter_InstalledCTPrimary"] = meterMasterEntity.Meter_InstalledCTPrimary;
                    dr["Meter_InstalledCTSecondary"] = meterMasterEntity.Meter_InstalledCTSecondary;
                    dr["Meter_InstalledCTRatio"] = meterMasterEntity.MeterInstalledCTRatio;
                    dr["Meter_InstalledPTPrimary"] = meterMasterEntity.Meter_InstalledPTPrimary;
                    dr["Meter_InstalledPTSecondary"] = meterMasterEntity.Meter_InstalledPTSecondary;
                    dr["Meter_InstalledPTRatio"] = meterMasterEntity.MeterInstalledPTRatio;
                    dr["Meter_Phone"] = meterMasterEntity.Meter_Phone;
                    dr["Meter_Status"] = meterMasterEntity.Meter_Status;
                    dr["UseEMFInCalculations"] = meterMasterEntity.UseEMFInCalculations;
                    if (meterMasterEntity.MeterGPRSModemIMEI != "")
                    {
                        dr[MeterGPRSModemIMEI] = meterMasterEntity.MeterGPRSModemIMEI;
                    }
                    if (meterMasterEntity.GPRSModemConnectionType != null)
                    {
                        dr["GPRSModemConnectionType"] = meterMasterEntity.GPRSModemConnectionType;
                    }
                    if (meterMasterEntity.GPRSModemIpType != null)
                    {
                        dr["GPRSModemIpType"] = meterMasterEntity.GPRSModemIpType;
                    }
                    table.Rows.Add(dr);
                }
                //foreach (IEntity entity in entities)
                //{
                //     MeterMasterEntity meterMasterEntity = entity as MeterMasterEntity;



                builder.Append("Insert Into meter_master(Meter_ID,MeterType_ID,MeterModel_ID,Meter_EMF," +
                   "Meter_ContractDemand,MeterUnit_ID,Meter_CTPrimary,Meter_CTSecondary,Meter_PTPrimary,Meter_PTSecondary," +
                   "Meter_InstalledCTPrimary,Meter_InstalledCTSecondary,Meter_InstalledCTRatio,Meter_InstalledPTPrimary,Meter_InstalledPTSecondary,Meter_InstalledPTRatio," +
                   "Meter_Phone,Meter_Status,UseEMFInCalculations");
                builder.Append(" ,Meter_GPRSModem_IMEI,GPRSModemConnectionType,GPRSModemIpType");
                builder.Append(") values(");
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterType_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterModel_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_EMF), ","));
                builder.Append(string.Concat(ParameterName(Meter_ContractDemand), ","));
                builder.Append(string.Concat(ParameterName(MeterUnit_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_CTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_CTSecondary), ","));
                builder.Append(string.Concat(ParameterName(Meter_PTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_PTSecondary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledCTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledCTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTPrimary), ","));
                builder.Append(string.Concat(ParameterName(Meter_InstalledPTSecondary), ","));
                builder.Append(string.Concat(ParameterName(MeterInstalledPTRatio), ","));
                builder.Append(string.Concat(ParameterName(Meter_Phone), ","));
                builder.Append(string.Concat(ParameterName(Meter_Status), ","));
                builder.Append(string.Concat(ParameterName(UseEMFInCalculations), ","));
                // GPRS specific fields
                builder.Append(string.Concat(ParameterName(MeterGPRSModemIMEI), ","));
                builder.Append(string.Concat(ParameterName(GPRSModemConnectionType), ","));
                builder.Append(string.Concat(ParameterName(GPRSModemIpType)));
                builder.Append(")");

                MySqlCommand command = new MySqlCommand(builder.ToString());
                command.CommandType = CommandType.Text;
                command.Parameters.Add("?Meter_ID", MySqlDbType.String).SourceColumn = "Meter_ID";
                command.Parameters.Add("?MeterType_ID", MySqlDbType.Int64).SourceColumn = "MeterType_ID";
                command.Parameters.Add("?MeterModel_ID", MySqlDbType.Int64).SourceColumn = "MeterModel_ID";
                command.Parameters.Add("?Meter_EMF", MySqlDbType.String).SourceColumn = "Meter_EMF";
                command.Parameters.Add("?Meter_ContractDemand", MySqlDbType.Double).SourceColumn = "Meter_ContractDemand";
                command.Parameters.Add("?MeterUnit_ID", MySqlDbType.Int64).SourceColumn = "MeterUnit_ID";
                command.Parameters.Add("?Meter_CTPrimary", MySqlDbType.Int64).SourceColumn = "Meter_CTPrimary";
                command.Parameters.Add("?Meter_CTSecondary", MySqlDbType.Int64).SourceColumn = "Meter_CTSecondary";
                command.Parameters.Add("?Meter_PTPrimary", MySqlDbType.Int64).SourceColumn = "Meter_PTPrimary";
                command.Parameters.Add("?Meter_PTSecondary", MySqlDbType.Int64).SourceColumn = "Meter_PTSecondary";
                command.Parameters.Add("?Meter_InstalledCTPrimary", MySqlDbType.Int64).SourceColumn = "Meter_InstalledCTPrimary";
                command.Parameters.Add("?Meter_InstalledCTSecondary", MySqlDbType.Int64).SourceColumn = "Meter_InstalledCTSecondary";
                command.Parameters.Add("?Meter_InstalledCTRatio", MySqlDbType.Int32).SourceColumn = "Meter_InstalledCTRatio";
                command.Parameters.Add("?Meter_InstalledPTPrimary", MySqlDbType.Int64).SourceColumn = "Meter_InstalledPTPrimary";
                command.Parameters.Add("?Meter_InstalledPTSecondary", MySqlDbType.Int64).SourceColumn = "Meter_InstalledPTSecondary";
                command.Parameters.Add("?Meter_InstalledPTRatio", MySqlDbType.Int32).SourceColumn = "Meter_InstalledPTRatio";
                command.Parameters.Add("?Meter_Phone", MySqlDbType.String).SourceColumn = "Meter_Phone";
                command.Parameters.Add("?Meter_Status", MySqlDbType.Int32).SourceColumn = "Meter_Status";
                command.Parameters.Add("?UseEMFInCalculations", MySqlDbType.Int32).SourceColumn = "UseEMFInCalculations";
                command.Parameters.Add("?Meter_GPRSModem_IMEI", MySqlDbType.String).SourceColumn = MeterGPRSModemIMEI;
                command.Parameters.Add("?GPRSModemConnectionType", MySqlDbType.Bit).SourceColumn = "GPRSModemConnectionType";
                command.Parameters.Add("?GPRSModemIpType", MySqlDbType.Int16).SourceColumn = "GPRSModemIpType";
                command.UpdatedRowSource = UpdateRowSource.None;

                //DataRequest request = new DataRequest(builder.ToString());
                //request.AddParamter(ParameterName(Meter_ID), meterMasterEntity.Meter_ID, DbType.String, 20);
                //request.AddParamter(ParameterName(MeterType_ID), meterMasterEntity.MeterType_ID, DbType.Int64);
                //request.AddParamter(ParameterName(MeterModel_ID), meterMasterEntity.MeterModel_ID, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_EMF), meterMasterEntity.Meter_EMF.ToString(), DbType.String);
                //request.AddParamter(ParameterName(Meter_ContractDemand), meterMasterEntity.Meter_ContractDemand, DbType.Double);
                //request.AddParamter(ParameterName(MeterUnit_ID), meterMasterEntity.MeterUnit_ID, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_CTPrimary), meterMasterEntity.Meter_CTPrimary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_CTSecondary), meterMasterEntity.Meter_CTSecondary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_PTPrimary), meterMasterEntity.Meter_PTPrimary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_PTSecondary), meterMasterEntity.Meter_PTSecondary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_InstalledCTPrimary), meterMasterEntity.Meter_InstalledCTPrimary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_InstalledCTSecondary), meterMasterEntity.Meter_InstalledCTSecondary, DbType.Int64);
                //request.AddParamter(ParameterName(MeterInstalledCTRatio), meterMasterEntity.MeterInstalledCTRatio, DbType.Int32);
                //request.AddParamter(ParameterName(Meter_InstalledPTPrimary), meterMasterEntity.Meter_InstalledPTPrimary, DbType.Int64);
                //request.AddParamter(ParameterName(Meter_InstalledPTSecondary), meterMasterEntity.Meter_InstalledPTSecondary, DbType.Int64);
                //request.AddParamter(ParameterName(MeterInstalledPTRatio), meterMasterEntity.MeterInstalledPTRatio, DbType.Int32);
                //request.AddParamter(ParameterName(Meter_Phone), meterMasterEntity.Meter_Phone, DbType.String, 15);
                //request.AddParamter(ParameterName(Meter_Status), meterMasterEntity.Meter_Status, DbType.Int32);
                //request.AddParamter(ParameterName(UseEMFInCalculations), meterMasterEntity.UseEMFInCalculations, DbType.Int32);
                //request.AddParamter(ParameterName(MeterGPRSModemIMEI), meterMasterEntity.MeterGPRSModemIMEI, DbType.String, 16);
                //request.AddParamter(ParameterName(GPRSModemConnectionType), meterMasterEntity.GPRSModemConnectionType, DbType.Boolean);
                //request.AddParamter(ParameterName(GPRSModemIpType), meterMasterEntity.GPRSModemIpType, DbType.Int16);

                //requests.Add(request);  


                //MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                //dataAdapter.InsertCommand = command;

                //dataAdapter.UpdateBatchSize = 1000;
                //int rec = dataAdapter.Update(table);
                helper.BatchInsert(table, command);
                //IDataHelper helper = DatabaseFactory.GetHelper();
                // helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchInsert(IList<IEntity> entities)", ex);
            }
        }

        public DataSet GetMeterVariantByMeterDataID(int meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT NetMeterVariantInfo from `meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);                
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterDataID(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }


        public DataSet GetBillingParameterColumn(int meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT ColumnsNames from `billingparameter` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetBillingParameterColumn(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetMeterVariantByMeterID(string meterID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT NetMeterVariantInfo from `meterdata_general` where ");
                builder.Append(string.Concat(MeterSerialNumber, "= '", meterID.Trim(), "'"));
                DataRequest request = new DataRequest(builder.ToString());
                //request.AddParamter(ParameterName(MeterSerialNumber), meterID.Trim(), DbType.String);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterID(string meterID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
    }
}
