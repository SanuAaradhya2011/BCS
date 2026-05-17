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
    public class DLMS650GeneralDAL : DALBase
    {
        private string General_ID = "General_ID";
        private string meterSerialNumber = "meterSerialNumber";
        private string manufacturername = "manufacturername";
        private string firmwareVersionformeter = "firmwareVersionformeter";
        private string metertype = "metertype";
        private string internalCTratio = "internalCTratio";
        private string internalPTratio = "internalPTratio";
        private string category = "Category";
        private string meteryearofmanufacture = "meteryearofmanufacture";
        private string MeterData_ID = "MeterData_ID";
        private string FileName = "FileName";
        private const string ENERGYRESOLUTION = "energyResolution";
        private const string DEMANDRESOLUTION = "demandResolution";
        private const string MeterDataType = "MeterDataType";
        private const string MeterModelNo = "MeterModelNo";

        private const string InternalFirmwareVersion = "InternalFirmwareVersion";
        private const string VoltageRating = "VoltageRating";
        private const string BasicCurrentRating = "BasicCurrentRating";

        private const string NetMeterVariantInfo = "NetMeterVariantInfo";

        private const string PrimaryMeterConstantInfo = "PrimaryMeterConstantInfo";//PGVCL
        private const string MeterConstantInfo = "MeterConstantInfo";

        private const string DisplayProgrammingType = "DisplayProgrammingType";


        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650GeneralDAL).ToString());
        
        bool IsPUMA = false;
        public DLMS650GeneralDAL()
            : base("meterdata_general", "General_ID")
        {
        }
        public DLMS650GeneralDAL(bool isPUMA)
            : base("meterdata_general", "General_ID")
        {
            IsPUMA = isPUMA;
        }
        public DataSet GetMeterData(int meterDataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.meterSerialNumber,A.manufacturername,A.firmwareVersionformeter,A.metertype,A.internalCTratio,A.internalPTratio,A.category,A.meteryearofmanufacture,A.MeterModelNo,A.InternalFirmwareVersion,A.VoltageRating,A.BasicCurrentRating,A.PrimaryMeterConstantInfo,A.MeterConstantInfo");//PGVCL
                builder.Append(" from meterdata_general A, MeterData B where ");
                builder.Append("A.MeterData_ID=B.MeterData_ID and A.");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("General data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(int meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public DataSet dGetGeneralDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.meterSerialNumber, f.FileName,m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "g.", column, " "));
                }
                builder.Append("from meterdata_general g inner join meterdata m on g.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("g.", meterSerialNumber, "=", ParameterName(meterSerialNumber)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterSerialNumber), meterID, DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for General viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dGetGeneralDataByMeter(string meterID, List<string> columns)", ex);
            }
            return dataSet;
        } 
        /// <summary>
        /// BhardwajG : Function for fetching meter model number by meterid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int GetMeterModelNoByMeterID(string meterID)
        {
            int meterModelNo = 0;
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct MeterModelNo FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(meterSerialNumber, "=", ParameterName(meterSerialNumber)));
                builder.Append(" and MeterModelNo is not null");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterSerialNumber), meterID, DbType.String);
                dataSet = new DataSet();
                helper.FillDataSet(request, dataSet);
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 1)
                    {
                        meterModelNo = -1;  /// Invalid case as same meter ID should not have Different Meter Model Number
                    }
                    else if (dataSet.Tables[0].Rows.Count == 1)
                    {
                        meterModelNo = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MeterModelNo"]);
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Meter Model ID viewed"));

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterModelNoByMeterID(string meterID)", ex);
            }
            return meterModelNo;
        }
        /// <summary>
        ///VBM : Function for fetching meter model number by meterid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int GetMeterModelNoByMeterDataID(string meterDataID)
        {
            int meterModelNo = 0;           
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(MeterModelNo,0) as MeterModelNo FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                meterModelNo = Convert.ToInt32(helper.ExecuteScalar(request));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterModelNoByMeterDataID(string meterDataID)", ex);
            }
            return meterModelNo;
        }

        /// <summary>
        ///VBM : Function for fetching Firmware verison  by meterdataid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public string GetFirmwareVersionByMeterDataID(string meterDataID)
        {
            string firmWareVersion = "0";
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(InternalFirmwareVersion,0) as MeterModFirmwareVersionelNo FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                object fwVersion = helper.ExecuteScalar(request);
                firmWareVersion = fwVersion == null ? firmWareVersion : fwVersion.ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Firmware Version number fetched"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFirmwareVersionByMeterDataID(string meterDataID)", ex);
            }
            return firmWareVersion;
        }

        /// <summary>
        ///HM : Function for fetching Active meter type  by meterdataid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public string GetActiveMeterTypeByMeterDataID(string meterDataID)
        {
            string meterType = String.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(metertype,0) as MeterType FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                object mtType = helper.ExecuteScalar(request);
                meterType = mtType == null ? meterType : mtType.ToString();
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter type fetched"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetActiveMeterTypeByMeterDataID(string meterDataID)", ex);
            }
            return meterType;
        }
        /// <summary>
        ///VBM : Function for fetching meter type by meterid
        /// </summary>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public int GetMeterTypeNoByMeterID(string meterID)
        {
            int meterType =0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(MeterType_ID,0) as MeterTypeID FROM meterdata_general g inner join metertype_master m ");
                builder.Append("on g.metertype = m.MeterType_Name  where ");
                builder.Append(string.Concat(meterSerialNumber, "=", ParameterName(meterSerialNumber)));
                DataRequest request = new DataRequest(builder.ToString());
                DataSet dataSet = new DataSet();
                helper.FillDataSet(request, dataSet);
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {                    
                    if (dataSet.Tables[0].Rows.Count == 1)
                    {
                        meterType = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MeterTypeID"]);
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Meter Type"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterTypeNoByMeterID(string meterID)", ex);
            }
            return meterType;
        }
        public DataSet GetMeterDataType(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select meterSerialNumber, MeterDataType ");
                builder.Append("from meterdata_general ");
                builder.Append("where ");
                builder.Append(string.Concat(MeterData_ID, " = ", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), Convert.ToInt32(meterDataID), DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Detail"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataType(string meterDataID)", ex);
            }
            return dataSet;
        }

        public DataSet GetMeterType(string meterDataId)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select meterSerialNumber, MeterDataType, metertype ");
                builder.Append("from meterdata_general ");
                builder.Append("where ");
                builder.Append(string.Concat(MeterData_ID, " = ", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), Convert.ToInt32(meterDataId), DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Detail"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetMeterType(string meterDataId)", ex);
            }
            return dataSet;
        }



        public DataSet GetEnergyResolution(string meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select EnergyResolution,DemandResolution ");
                builder.Append("from meterdata_general ");
                builder.Append("where ");
                builder.Append(string.Concat(MeterData_ID, " = ", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), Convert.ToInt32(meterDataID), DbType.Int32);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Detail"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetEnergyResolution(string meterDataID)", ex);
            }
            return dataSet;
        }
        public DataSet dGetGeneralDataByFileName(string meterID, string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.meterSerialNumber, f.FileName,m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    builder.Append(string.Concat(",", "g.", column, " "));
                }
                builder.Append("from meterdata_general g inner join meterdata m on g.MeterData_ID = m.MeterData_ID ");
                builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                builder.Append(string.Concat("g.", meterSerialNumber, "=", ParameterName(meterSerialNumber), " ", "and", " "));
                builder.Append(string.Concat("f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterSerialNumber), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for Power viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dGetGeneralDataByFileName(string meterID, string fileName, List<string> columns)", ex);
            }
            return dataSet;
        }

        public override IEntity InsertData(IEntity entity)
        {
            if (entity == null)
                return entity;
            DLMS650NamePlateDetailsEntity generalEntity = entity as DLMS650NamePlateDetailsEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_general(meterSerialNumber,manufacturername,firmwareVersionformeter,metertype,internalCTratio,internalPTratio,MeterModelNo,");
                builder.Append("InternalFirmwareVersion,VoltageRating,BasicCurrentRating,NetMeterVariantInfo,PrimaryMeterConstantInfo,MeterConstantInfo,DisplayProgrammingType,");//PGVCL
               
                if (IsPUMA)
                {
                    builder.Append("energyResolution,demandResolution,MeterDataType,");
                }
                builder.Append("Category,meteryearofmanufacture,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(meterSerialNumber), ","));
                builder.Append(string.Concat(ParameterName(manufacturername), ","));
                builder.Append(string.Concat(ParameterName(firmwareVersionformeter), ","));
                builder.Append(string.Concat(ParameterName(metertype), ","));
                builder.Append(string.Concat(ParameterName(internalCTratio), ","));
                builder.Append(string.Concat(ParameterName(internalPTratio), ","));
                builder.Append(string.Concat(ParameterName(MeterModelNo), ","));
                builder.Append(string.Concat(ParameterName(InternalFirmwareVersion), ","));
                builder.Append(string.Concat(ParameterName(VoltageRating), ","));
                builder.Append(string.Concat(ParameterName(BasicCurrentRating), ","));
                builder.Append(string.Concat(ParameterName(NetMeterVariantInfo), ","));

                builder.Append(string.Concat(ParameterName(PrimaryMeterConstantInfo), ","));//PGVCL
                builder.Append(string.Concat(ParameterName(MeterConstantInfo), ","));
                builder.Append(string.Concat(ParameterName(DisplayProgrammingType), ","));
                if (IsPUMA)
                {
                    builder.Append(string.Concat(ParameterName(ENERGYRESOLUTION), ","));
                    builder.Append(string.Concat(ParameterName(DEMANDRESOLUTION), ","));
                    builder.Append(string.Concat(ParameterName(MeterDataType), ","));
                }
                builder.Append(string.Concat(ParameterName(category), ","));
                builder.Append(string.Concat(ParameterName(meteryearofmanufacture), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterSerialNumber), generalEntity.MeterSerialNumber, DbType.String, 40);
                request.AddParamter(ParameterName(manufacturername), generalEntity.Manufacturername, DbType.String, 40);
                request.AddParamter(ParameterName(firmwareVersionformeter), generalEntity.FirmwareVersionformeter, DbType.String, 40);
                request.AddParamter(ParameterName(metertype), generalEntity.Metertype, DbType.String, 40);
                request.AddParamter(ParameterName(internalCTratio), generalEntity.InternalCTratio, DbType.String, 40);
                request.AddParamter(ParameterName(internalPTratio), generalEntity.InternalPTratio, DbType.String, 40);
                request.AddParamter(ParameterName(MeterModelNo), generalEntity.MeterModelNo, DbType.String, 40);
                request.AddParamter(ParameterName(InternalFirmwareVersion), generalEntity.InternalFirmwareVersion, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageRating), generalEntity.VoltageRating, DbType.String, 40); 
                request.AddParamter(ParameterName(BasicCurrentRating),generalEntity.BasicCurrentRating , DbType.String, 40);
                request.AddParamter(ParameterName(NetMeterVariantInfo), generalEntity.NetMeterVariantInfo, DbType.String, 40);

                request.AddParamter(ParameterName(PrimaryMeterConstantInfo), generalEntity.PrimaryMeterConstant, DbType.String, 40);//PGVCL
                request.AddParamter(ParameterName(MeterConstantInfo), generalEntity.MeterConstant, DbType.String, 40);
                request.AddParamter(ParameterName(DisplayProgrammingType), generalEntity.DisplayProgrammingType, DbType.String, 40);
                if (IsPUMA)
                {
                    request.AddParamter(ParameterName(ENERGYRESOLUTION), generalEntity.EnergyResolution, DbType.String, 40);
                    request.AddParamter(ParameterName(DEMANDRESOLUTION), generalEntity.DemandResolution, DbType.String, 40);
                    request.AddParamter(ParameterName(MeterDataType), generalEntity.MeterDataType, DbType.String, 40);
                }
                request.AddParamter(ParameterName(category), generalEntity.Category, DbType.String, 40);
                request.AddParamter(ParameterName(meteryearofmanufacture), generalEntity.Meteryearofmanufacture, DbType.String, 40);
                request.AddParamter(ParameterName(MeterData_ID), generalEntity.MeterData_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                Flag = false;
            }
            if (Flag)
                generalEntity.General_ID = Convert.ToInt64(this.GetPK());
            return generalEntity;
        }



        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DLMS650NamePlateDetailsEntity generalEntity = new DLMS650NamePlateDetailsEntity();
            if (NotNullAndNotDBNull(row, General_ID)) generalEntity.General_ID = Convert.ToInt64(row[General_ID]);
            if (NotNullAndNotDBNull(row, meterSerialNumber)) generalEntity.MeterSerialNumber = Convert.ToString(row[meterSerialNumber]);
            if (NotNullAndNotDBNull(row, manufacturername)) generalEntity.Manufacturername = Convert.ToString(row[manufacturername]);
            if (NotNullAndNotDBNull(row, firmwareVersionformeter)) generalEntity.FirmwareVersionformeter = Convert.ToString(row[firmwareVersionformeter]);
            if (NotNullAndNotDBNull(row, metertype)) generalEntity.Metertype = Convert.ToString(row[metertype]);
            if (NotNullAndNotDBNull(row, internalCTratio)) generalEntity.InternalCTratio = Convert.ToString(row[internalCTratio]);
            if (NotNullAndNotDBNull(row, category)) generalEntity.Category = Convert.ToString(row[category]);
            if (NotNullAndNotDBNull(row, meteryearofmanufacture)) generalEntity.Meteryearofmanufacture = Convert.ToString(row[meteryearofmanufacture]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) generalEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            return generalEntity;
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
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_general where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block { }
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
                return Flag;
        }

        public bool UpdateMeterDataType(string meterDataID, string meterType)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("update meterdata_general set ");
                builder.Append(string.Concat(MeterDataType, "=", ParameterName(MeterDataType)));
                builder.Append(string.Concat(" Where ", MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(string.Concat(" and " + MeterDataType, "=", "'LTCT'"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataType), meterType, DbType.String);
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data updated"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block { }
            {
                logger.Log(LOGLEVELS.Error, "UpdateMeterDataType(string meterDataID, string meterType)", ex);
            }
                return Flag;
        }

        /// <summary>
        /// get dataset for meter number and sim number 
        /// </summary>
        public DataSet GetAllMeterType()
        {
            DataSet ds;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // for GPRS communication,IMEI number need to be displayed
                builder.Append("SELECT g.meterSerialNumber  , m.MeterType_ID FROM meterdata_general g , metertype_master m where g.metertype = m.MeterType_Name");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);     
                return ds;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllMeterType()", ex);
                ds = null;
            }
            return ds;
        }
        /// <summary>
        /// get dataset of all meterid and meter model no.
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMeterModel()
        {
            DataSet ds ;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                // for GPRS communication,IMEI number need to be displayed
                builder.Append("SELECT meterSerialNumber ,metermodelno FROM meterdata_general where metermodelno is not null");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);                
                return ds;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllMeterModel()", ex);
                ds = null;
            }
            return ds;
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

        public DataSet GetMeterVariantByMeterDataID(string meterDataId)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
               
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT NetMeterVariantInfo FROM meterdata_general where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int32);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);   
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterVariantByMeterDataID(string meterDataId)", ex);
                ds = null;
            }
            return ds;
        }

        public int GetMeterDisplayProgrammingVariantByMeterDataID(string meterDataId)
        {
            int variant = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT distinct IFNULL(DisplayProgrammingType, NULL) as DisplayProgrammingType FROM `dlms_ltct_650`.`meterdata_general` where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.String);
                object result = helper.ExecuteScalar(request);

                if (Int32.TryParse(result?.ToString(), out variant))
                {
                    variant = variant == 2 ? (int)DisplayProgrammingTypes.TwoByte : (int)DisplayProgrammingTypes.OneByte;
                }
                else
                {
                    variant = 0;
                }

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter type fetched"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetActiveMeterTypeByMeterDataID(string meterDataID)", ex);
            }
            return variant;
        }
    }
}

