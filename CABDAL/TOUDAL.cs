using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											DAL class for meter TOU data 																				|
 * |											Author : Gopal krishna Gupta       									|
 * |											Date   : 06-Feb-2013											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

namespace CAB.DALC.Data
{
    public class TOUDAL:DALBase
    {
        private string touId = "TouId";
        private string meterDataId = "MeterDataId";
        private string startHour = "StartHour";
        private string startMin = "StartMin";
        private string tariff = "Tariff";
        private string seasonNumber = "SeasonNumber";
        private string meterId = "MeterId";
        private string FileName = "FileName";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TOUDAL).ToString());

        public TOUDAL()
            : base("meterdata_tou", "TouId")
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
        /// <summary>
        /// Insert tou data 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
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

         public DataSet DetailData(long meterDataID)
         {
             DataSet dataSet = new DataSet();
             try
             {
                 IDataHelper helper = DatabaseFactory.GetHelper();
                 StringBuilder builder = new StringBuilder();
                 builder.Append("Select Concat(LPAD(CAST(starthour as char),2,0),':',LPAD(CAST(startMin as char),2,0)) as 'Zone Start Time(HH:MM)',");
                 builder.Append("Concat('Tariff',' ',CAST(Tariff as char)) as 'Tariff Zone',SeasonNumber as 'Season Number'  from meterdata_tou where ");                                
                 builder.Append(string.Concat(meterDataId, "=", ParameterName(meterDataId)));                 
                 DataRequest request = new DataRequest(builder.ToString());
                 request.AddParamter(ParameterName(meterDataId), meterDataID, DbType.Int64);
                 dataSet = helper.FillDataSet(request, dataSet);
             }
             catch (CABException ex)    //Exception log for catch block
             {
                 logger.Log(LOGLEVELS.Error, " DetailData(long meterDataID)", ex);
             }
             return dataSet;
         }
        /// <summary>
        /// Insert anomaly data into meterdata_anomaly table and 
        /// return PK on inserted row         
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
         //public IEntity InsertData(IEntity entity, bool flag)
         //{
         //    if (entity == null)
         //        return entity;
         //    TOUEntity objTouEntity = entity as TOUEntity;  
         //    bool blnInsertSuccess = false;
         //    foreach(TOU tou in objTouEntity.tou)
         //    {             
         //        try
         //        {
         //            IDataHelper objDataHelper = DatabaseFactory.GetHelper();
         //            StringBuilder objCommandText = new StringBuilder();
         //            objCommandText.Append("Insert Into meterdata_tou(MeterDataId,StartHour,StartMin,Tariff) values(");
         //            objCommandText.Append(string.Concat(ParameterName(meterDataId), ","));
         //            objCommandText.Append(string.Concat(ParameterName(startHour), ","));
         //            objCommandText.Append(string.Concat(ParameterName(startMin), ","));                 
         //            objCommandText.Append(string.Concat(ParameterName(tariff), ")"));
         //            DataRequest objDataRequest = new DataRequest(objCommandText.ToString());
         //            objDataRequest.AddParamter(ParameterName(meterDataId), objTouEntity.MeterDataId, DbType.Int64);
         //            objDataRequest.AddParamter(ParameterName(startHour), tou.StartHour, DbType.Byte);
         //            objDataRequest.AddParamter(ParameterName(startMin), tou.StartMin, DbType.Byte);
         //            objDataRequest.AddParamter(ParameterName(tariff), tou.Tariff, DbType.Byte);                     
         //            objDataHelper.ExecuteNonQuery(objDataRequest);
         //            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New TOU data added"));
         //            blnInsertSuccess = true;
         //        }
         //        catch (CABException)
         //        {
         //            blnInsertSuccess = false;
         //        }
         //    }
         //    if (blnInsertSuccess)
         //        //objAnomalyEntity. = Convert.ToInt64(this.GetPK());
         //    return objAnomalyEntity;
         //}


         private DataRequest GetRequest(IEntity entity)
         {

             if (entity == null)
                 return null;
             TOU tou = entity as TOU;
             IDataHelper objDataHelper = DatabaseFactory.GetHelper();
             StringBuilder strCommandText = new StringBuilder();
             strCommandText.Append("Insert Into meterdata_tou(MeterDataId,StartHour,StartMin,Tariff,SeasonNumber) values(");
             strCommandText.Append(string.Concat(ParameterName(meterDataId), ","));
             strCommandText.Append(string.Concat(ParameterName(startHour), ","));
             strCommandText.Append(string.Concat(ParameterName(startMin), ","));
             strCommandText.Append(string.Concat(ParameterName(tariff), ","));
             strCommandText.Append(string.Concat(ParameterName(seasonNumber), ")"));
             DataRequest objDataRequest = new DataRequest(strCommandText.ToString());
             objDataRequest.AddParamter(ParameterName(meterDataId), tou.MeterData_ID, DbType.Int64);
             objDataRequest.AddParamter(ParameterName(startHour), tou.StartHour, DbType.Byte);
             objDataRequest.AddParamter(ParameterName(startMin), tou.StartMin, DbType.Byte);
             objDataRequest.AddParamter(ParameterName(tariff), tou.Tariff, DbType.Byte);
             objDataRequest.AddParamter(ParameterName(seasonNumber), tou.SeasonNumber, DbType.Byte);
             return objDataRequest;

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
                 builder.Append("Select AnomalyId,MeterDataId,FlashStatus,EepRamStatus,SmpsStatus,RtcStatus,ErrorCodeStatus from meterdata_anomaly where ");
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
        /// Row to entity conversion
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
         public override IEntity RowToEntity(DataRow row)
         {
             if (row == null) return null;
             AnomalyEntity objAnomalyEntity = new AnomalyEntity();
            // if (NotNullAndNotDBNull(row, anomalyId)) objAnomalyEntity.AnomalyId = Convert.ToInt32(row[anomalyId]);
             if (NotNullAndNotDBNull(row, meterDataId)) objAnomalyEntity.MeterDataId = Convert.ToInt32(row[meterDataId]);
             //if (NotNullAndNotDBNull(row, flashStatus)) objAnomalyEntity.Flash = Convert.ToInt32(row[flashStatus]);
             //if (NotNullAndNotDBNull(row, eepRamStatus)) objAnomalyEntity.EeProm = Convert.ToInt32(row[eepRamStatus]);
             //if (NotNullAndNotDBNull(row, smpsStatus)) objAnomalyEntity.Smps = Convert.ToInt32(row[smpsStatus]);
             //if (NotNullAndNotDBNull(row, rtcStatus)) objAnomalyEntity.Rtc = Convert.ToInt32(row[rtcStatus]);
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
                 logger.Log(LOGLEVELS.Error, "GetAnomalyDataByMeter(string meterID, List<string> columns)", ex);
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
         
    }
}
