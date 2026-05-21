using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using Hunt.EPIC.Logging;

/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											DAL class for meter anomaly data 																				|
 * |											Author : Vidya BHooshan Mishra       									|
 * |											Date   : 18-dec-2012											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

namespace CAB.DALC.Data
{
    public class AnomalyDAL:DALBase
    {
        private string anomalyId = "AnomalyId";
        private string meterDataId = "MeterDataId";
        private string flashStatus = "FlashStatus";
        private string eepRamStatus = "EepRamStatus";
        private string smpsStatus = "SmpsStatus";
        private string rtcStatus = "RtcStatus";
        private string rtcBatteryStatus = "RTCBatteryStatus";
        private string mainBatteryStatus = "MainBatteryStatus";
        private string Error_code = "ErrorCodeStatus";
        private string meterId = "MeterId";
        private string FileName = "FileName";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AnomalyDAL).ToString());

        public AnomalyDAL()
            : base("meterdata_anomaly", "AnomalyId")
        {
        }

         public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
         public override bool DeleteData(IEntity entity)
         {
             throw new NotImplementedException();
         }
         public override IList<IEntity> ListData()
         {
             throw new NotImplementedException();
         }

         public override IEntity InsertData(IList<IEntity> entities)
         {
             throw new NotImplementedException();
         }

        /// <summary>
        /// Insert anomaly data into meterdata_anomaly table and 
        /// return PK on inserted row         
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
         public IEntity InsertData(IEntity entity, bool flag)
         {
             if (entity == null)
                 return entity;
             AnomalyEntity objAnomalyEntity = entity as AnomalyEntity;             
             bool blnInsertSuccess = false;
             try
             {
                 IDataHelper objDataHelper = DatabaseFactory.GetHelper();
                 StringBuilder objCommandText = new StringBuilder();
                 objCommandText.Append("Insert Into meterdata_anomaly(MeterDataId,FlashStatus,EepRamStatus,SmpsStatus,RtcStatus,RTCBatteryStatus,MainBatteryStatus,ErrorCodeStatus) values(");
                 objCommandText.Append(string.Concat(ParameterName(meterDataId), ","));
                 objCommandText.Append(string.Concat(ParameterName(flashStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(eepRamStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(smpsStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(rtcStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(rtcBatteryStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(mainBatteryStatus), ","));
                 objCommandText.Append(string.Concat(ParameterName(Error_code), ")"));
                 DataRequest objDataRequest = new DataRequest(objCommandText.ToString());
                 objDataRequest.AddParamter(ParameterName(meterDataId), objAnomalyEntity.MeterDataId, DbType.Int64);
                 objDataRequest.AddParamter(ParameterName(flashStatus), objAnomalyEntity.Flash, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(eepRamStatus), objAnomalyEntity.EeProm, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(smpsStatus), objAnomalyEntity.Smps, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(rtcStatus), objAnomalyEntity.Rtc, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(rtcBatteryStatus), objAnomalyEntity.RTCBattery, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(mainBatteryStatus), objAnomalyEntity.MainBattery, DbType.Int32);
                 objDataRequest.AddParamter(ParameterName(Error_code), objAnomalyEntity.Error, DbType.Int32);
                 objDataHelper.ExecuteNonQuery(objDataRequest);
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New anomaly value added"));
                 blnInsertSuccess = true;
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity, bool flag)", ex);
                 blnInsertSuccess = false;
             }
             if (blnInsertSuccess)
                 objAnomalyEntity.AnomalyId = Convert.ToInt64(this.GetPK());
             return objAnomalyEntity;
         }
        /// <summary>
        /// Retrive data from anomaly table using meter data id 
        /// </summary>
        /// <param name="intMeterDataId"></param>
        /// <returns></returns>
         public override IEntity GetDetailData(int intMeterDataId)
         {
             AnomalyEntity objAnomalyEntity = null;
             try
             {
                 IDataHelper helper = DatabaseFactory.GetHelper();
                 StringBuilder builder = new StringBuilder();
                 builder.Append("Select AnomalyId,MeterDataId,FlashStatus,EepRamStatus,SmpsStatus,RtcStatus, MainBatteryStatus, ErrorCodeStatus from meterdata_anomaly where ");
                 builder.Append(string.Concat(meterDataId, "=", ParameterName(meterDataId)));
                 DataRequest request = new DataRequest(builder.ToString());
                 request.AddParamter(ParameterName(meterDataId), intMeterDataId, DbType.Int64);
                 DataSet ds = new DataSet();
                 ds = helper.FillDataSet(request, ds);
                 if (ds.Tables[0].Rows.Count > 0)
                     objAnomalyEntity = (AnomalyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Anomaly  Description Viewed Based on The Selected Anomaly tab"));
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "GetDetailData(int intMeterDataId)", ex);
                 objAnomalyEntity = null;
             }
             return objAnomalyEntity;
         }

         /// <summary>
         /// Retrive Anomaly Data to be diplayed in Analysis Detail.
         /// </summary>
         /// <param name="intMeterDataId"></param>
         /// <returns></returns>
         public  DataSet GetAnomalyDataForAnalysisDetail(int intMeterDataId)
         {

             DataSet anomalyData = new DataSet();
             try
             {
                 IDataHelper helper = DatabaseFactory.GetHelper();
                 StringBuilder builder = new StringBuilder();
                 //**************if Anomaly not configure in meter then eprom and RTC is -1.user story 474452   
                 builder.Append("Select AnomalyId,MeterDataId, (CASE FlashStatus  WHEN 1 THEN 'OK' ELSE 'NOT OK' END) as 'FLASH'"+
                 ",(CASE EepRamStatus  WHEN 1 THEN 'OK' WHEN -1 THEN '-' ELSE 'NOT OK' END) as 'EEPROM',(CASE SmpsStatus  WHEN 1 THEN 'OK' ELSE 'NOT OK' END) as 'POWER SUPPLY'" +
                 ",(CASE RtcStatus  WHEN 1 THEN 'OK' WHEN -1 THEN '-' ELSE 'NOT OK' END) as 'RTC',(CASE RTCBatteryStatus  WHEN 1 THEN 'OK' ELSE 'NOT OK' END) as 'RTC BATTERY', (CASE MainBatteryStatus  WHEN 1 THEN 'OK' WHEN -2 THEN '/' ELSE 'NOT OK' END) as 'MAIN BATTERY',  ErrorCodeStatus from meterdata_anomaly where ");
                 builder.Append(string.Concat(meterDataId, "=", ParameterName(meterDataId)));
                 DataRequest request = new DataRequest(builder.ToString());
                 request.AddParamter(ParameterName(meterDataId), intMeterDataId, DbType.Int64);
                 anomalyData = helper.FillDataSet(request, anomalyData);                 
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Anomaly  Description Viewed Based on The Selected Anomaly tab"));
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "GetAnomalyDataForAnalysisDetail(int intMeterDataId)", ex);
                 anomalyData = null;
             }
             return anomalyData;
         }

        /// <summary>
        /// Row to entity conversion
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
         public override IEntity RowToEntity(DataRow row)
         {
             if (row == null) return null;
             AnomalyEntity objAnomalyEntity = new AnomalyEntity();
             if (NotNullAndNotDBNull(row, anomalyId)) objAnomalyEntity.AnomalyId = Convert.ToInt32(row[anomalyId]);
             if (NotNullAndNotDBNull(row, meterDataId)) objAnomalyEntity.MeterDataId = Convert.ToInt32(row[meterDataId]);
             if (NotNullAndNotDBNull(row, flashStatus)) objAnomalyEntity.Flash = Convert.ToInt32(row[flashStatus]);
             if (NotNullAndNotDBNull(row, eepRamStatus)) objAnomalyEntity.EeProm = Convert.ToInt32(row[eepRamStatus]);
             if (NotNullAndNotDBNull(row, smpsStatus)) objAnomalyEntity.Smps = Convert.ToInt32(row[smpsStatus]);
             if (NotNullAndNotDBNull(row, rtcStatus)) objAnomalyEntity.Rtc = Convert.ToInt32(row[rtcStatus]);
             if (NotNullAndNotDBNull(row, Error_code)) objAnomalyEntity.Error = Convert.ToInt32(row[Error_code]);
             if (NotNullAndNotDBNull(row, mainBatteryStatus)) objAnomalyEntity.MainBattery = Convert.ToInt32(row[mainBatteryStatus]);
             return objAnomalyEntity;
         }
        /// <summary>
        /// Used to get anomlay data by meter for displaying in meter wise anomlay reports
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
         public DataSet GetAnomalyDataByMeter(string meterID, List<string> columns)
         {
             DataSet dataSet = null;
             try
             {
                 IDataHelper helper = DatabaseFactory.GetHelper();
                 StringBuilder builder = new StringBuilder();
                 builder.Append("Select m.MeterId, f.FileName,m.ReadingDateTime ");
                 foreach (string column in columns)
                 {
                     builder.Append(string.Concat(",", "a.", column, " "));
                 }
                 builder.Append("from meterdata_anomaly a inner join meterdata m on a.MeterDataID = m.MeterData_ID ");
                 builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                 builder.Append(string.Concat("m.", meterId, "=", ParameterName(meterId)));                
                 DataRequest request = new DataRequest(builder.ToString());                 
                 request.AddParamter(ParameterName(meterId), meterID, DbType.String);
                 dataSet = new DataSet();
                 dataSet = helper.FillDataSet(request, dataSet);
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for self diagnosis meter wise report"));
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "", ex);
             }
             return dataSet;
         }
        /// <summary>
        /// Gets anomaly data by file name to display in date wise reports
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
         public DataSet GetAnomalyDataByFileName(string strMeterID, string strfileName, List<string> columns)
         {
             DataSet dataSet = null;
             try
             {
                 IDataHelper helper = DatabaseFactory.GetHelper();
                 StringBuilder builder = new StringBuilder();
                 builder.Append("Select m.MeterId, f.FileName,m.ReadingDateTime ");
                 foreach (string column in columns)
                 {
                     builder.Append(string.Concat(",", "a.", column, " "));
                 }
                 builder.Append("from meterdata_anomaly a inner join meterdata m on a.MeterDataID = m.MeterData_ID ");
                 builder.Append("inner join fileupload_master f on m.fileUpload_ID = f.fileUpload_ID where ");
                 builder.Append(string.Concat("m.", meterId, "=", ParameterName(meterId)));
                 builder.Append(string.Concat(" ", "AND", " ", "f.", FileName, "=", ParameterName(FileName)));
                 DataRequest request = new DataRequest(builder.ToString());
                 request.AddParamter(ParameterName(meterId), strMeterID, DbType.String);
                 request.AddParamter(ParameterName(FileName), strfileName, DbType.String);
                 dataSet = new DataSet();
                 dataSet = helper.FillDataSet(request, dataSet);
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for self diagnosis date wise report"));
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "GetAnomalyDataByFileName(string strMeterID, string strfileName, List<string> columns)", ex);
             }
             return dataSet;
         }


         public override bool UpdateData(IEntity entity)
         {
             throw new NotImplementedException();
         }
        

         public override DataSet ListDataSet()
         {
             throw new NotImplementedException();
         }

         /// <summary>
         /// Deletes Anomaly Data 
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
                 builder.Append("Delete from meterdata_anomaly where ");
                 builder.Append(string.Concat(meterDataId, "=", ParameterName(meterDataId.ToString())));
                 DataRequest request = new DataRequest(builder.ToString());
                 request.AddParamter(ParameterName(meterDataId), meterDataID, DbType.Int32);
                 int i = helper.ExecuteNonQuery(request);
                 UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Anomaly data for a specified meter deleted."));
                 Flag = true;
             }
             catch (Exception ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
             }
             return Flag;
         }
         
    }
}
