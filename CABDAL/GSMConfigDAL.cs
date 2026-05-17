/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GSMConfigDAL : DALBase
    {
        private string Meter_ID = "Meter_ID"; 
        private string Meter_Phone = "Meter_Phone";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMConfigDAL).ToString());

        
        private string meterDataId = "MeterDataId";
        private string SimNo = "SimNumber";
        private string Status = "Status";
        private string FP = "Reason";
        private string TaskID = "Task";
        private string CreationDateTime = "creationDateTime";

        public bool InsertDataconfig(int MeterID, string SimNum, string WriteStatus, string ReasonPassFail,string Taskname)
        {
            bool blnInsertSuccess = false;
            try
            {
               
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsmconfigstatus(MeterID,SimNo,Status,Reason,TaskID,creationDateTime) values(");
                builder.Append(string.Concat(ParameterName(meterDataId), ","));
                builder.Append(string.Concat(ParameterName(SimNo), ","));
                builder.Append(string.Concat(ParameterName(Status), ","));
                builder.Append(string.Concat(ParameterName(FP), ","));
                builder.Append(string.Concat(ParameterName(TaskID), ","));
                builder.Append(string.Concat(ParameterName(CreationDateTime), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterDataId), MeterID, System.Data.DbType.Int32, 8);
                request.AddParamter(ParameterName(SimNo), SimNum, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(Status), WriteStatus, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(FP), ReasonPassFail, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(TaskID), Taskname, System.Data.DbType.String);
                request.AddParamter(ParameterName(CreationDateTime), DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString(), DbType.String);
                helper.ExecuteNonQuery(request);
                blnInsertSuccess = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertDataconfig(IEntity entity)", ex);
                blnInsertSuccess = false;
            }
            return blnInsertSuccess;
          
        }
        public bool InsertDataRead(int MeterID, string SimNum, string WriteStatus, string ReasonPassFail, string Taskname)
        {
            bool blnInsertSuccess = false;
            try
            {
               
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsmreadstatus(MeterID,SimNo,Status,Reason,TaskID,creationDateTime) values(");
                builder.Append(string.Concat(ParameterName(meterDataId), ","));
                builder.Append(string.Concat(ParameterName(SimNo), ","));
                builder.Append(string.Concat(ParameterName(Status), ","));
                builder.Append(string.Concat(ParameterName(FP), ","));
                builder.Append(string.Concat(ParameterName(TaskID), ","));
                builder.Append(string.Concat(ParameterName(CreationDateTime), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(meterDataId), MeterID, System.Data.DbType.Int32, 8);
                request.AddParamter(ParameterName(SimNo), SimNum, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(Status), WriteStatus, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(FP), ReasonPassFail, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(TaskID), Taskname, System.Data.DbType.String);
                request.AddParamter(ParameterName(CreationDateTime), DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString(), DbType.String);
                helper.ExecuteNonQuery(request);
                blnInsertSuccess = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertDataconfig(IEntity entity)", ex);
                blnInsertSuccess = false;
            }
            return blnInsertSuccess;
          
        }
        public bool UpdateStatus(string MeterSerailNo, string WriteStatus, string Reason, string Taskname)
        {
            bool Flag = false;
            try
            {
               IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update gsmconfigstatus Set ");
                builder.Append(string.Concat("Status", "=", ParameterName(Status), ","));
                builder.Append(string.Concat("Reason", "=", ParameterName(FP), ","));
                builder.Append(string.Concat("creationDateTime", "=", ParameterName(CreationDateTime), " "));
                builder.Append(string.Concat(" Where ", "TaskID", "=", ParameterName(TaskID), " "));
                builder.Append(string.Concat(" and ", "MeterID", "=", ParameterName(meterDataId)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Status), WriteStatus, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(FP), Reason, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(CreationDateTime), DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString(), DbType.String);
                request.AddParamter(ParameterName(TaskID), Taskname, System.Data.DbType.String);
                request.AddParamter(ParameterName(meterDataId), MeterSerailNo, System.Data.DbType.String);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateStatus(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        public bool UpdateRead(string MeterSerailNo, string WriteStatus, string Reason, string Taskname)
        {
            bool Flag = false;
            try
            {
               IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update gsmreadstatus Set ");
                builder.Append(string.Concat("Status", "=", ParameterName(Status), ","));
                builder.Append(string.Concat("Reason", "=", ParameterName(FP), ","));
                builder.Append(string.Concat("creationDateTime", "=", ParameterName(CreationDateTime), " "));
                builder.Append(string.Concat(" Where ", "TaskID", "=", ParameterName(TaskID), " "));
                builder.Append(string.Concat(" and ", "MeterID", "=", ParameterName(meterDataId)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Status), WriteStatus, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(FP), Reason, System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(CreationDateTime), DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString(), DbType.String);
                request.AddParamter(ParameterName(TaskID), Taskname, System.Data.DbType.String);
                request.AddParamter(ParameterName(meterDataId), MeterSerailNo, System.Data.DbType.String);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateStatus(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }
        
        
        public bool UpdateAbortStatus(string MeterSerailNo, string WriteStatus, string Reason, string Taskname)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
              //  builder.Append("Update gsmconfigstatus Set Status='abort',Reason='user abort' Where Status='Pending...' and TaskID='20180813165726'");
                builder.Append("Update gsmconfigstatus Set ");
                builder.Append(string.Concat("Status", "=", ParameterName(Status), ","));
                builder.Append(string.Concat("Reason", "=", ParameterName(FP), " "));
                builder.Append(string.Concat(" Where ", "Status", "=", "'Pending...'", " "));
                builder.Append(string.Concat(" and ", "TaskID", "=", ParameterName(TaskID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Status), "Abort", System.Data.DbType.String, 150);
                request.AddParamter(ParameterName(FP), "User Aborted", System.Data.DbType.String, 150);             
               request.AddParamter(ParameterName(TaskID), Taskname, System.Data.DbType.String);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateStatus(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public List<GSMConfigReportEntity> GetReportData(DateTime fromDate, DateTime toDate,string taskname)
        {
            string meterIDCheck = string.Empty;
            string meterIDSwap = string.Empty;
           List<GSMConfigReportEntity> lstGsmReport = new List<GSMConfigReportEntity>();
            GSMConfigReportEntity gsmReportEntity;
            IDataHelper helper = DatabaseFactory.GetHelper();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            StringBuilder builder = new StringBuilder();
           // builder.Append("select MeterID,SimNo,Status,Reason,TaskID,creationDateTime from gsmconfigstatus where STR_TO_DATE(creationDateTime, '%d/%m/%Y %H:%i:%s') between STR_TO_DATE('" + fromDate + "', '%d/%m/%Y %H:%i:%s') and STR_TO_DATE('" + toDate + "'" +
           //  " ,'%d/%m/%Y %H:%i:%s') and TaskID ='" + taskname + "'");

            builder.Append("select MeterID,SimNo,Status,Reason,TaskID,creationDateTime from gsmconfigstatus where TaskID ='" + taskname + "'");
            DataRequest request = new DataRequest(builder.ToString());
            DataSet ds = new DataSet();
            ds = helper.FillDataSet(request, ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count + 1; i++)
                {
                    //extracting meter ID
                    if (i < ds.Tables[0].Rows.Count)
                        meterIDCheck = ds.Tables[0].Rows[i]["MeterID"].ToString();

                    //enter if it is different meter id or always enter if it is last row
                    if ((meterIDCheck != meterIDSwap && i != 0) || i == ds.Tables[0].Rows.Count)
                    {
                        try
                        {
                            gsmReportEntity = new GSMConfigReportEntity();
                            gsmReportEntity.Meter_ID = Convert.ToInt32(ds.Tables[0].Rows[i - 1]["MeterID"]);
                            gsmReportEntity.Sim_No = ds.Tables[0].Rows[i - 1]["SimNo"].ToString();
                            gsmReportEntity.Status = ds.Tables[0].Rows[i - 1]["Status"].ToString();
                            gsmReportEntity.Reason = ds.Tables[0].Rows[i - 1]["Reason"].ToString();
                            gsmReportEntity.TaskName = ds.Tables[0].Rows[i - 1]["TaskID"].ToString();
                            gsmReportEntity.Reading_DateTime = (Convert.ToDateTime(ds.Tables[0].Rows[i - 1]["creationDateTime"].ToString(), new System.Globalization.CultureInfo("en-GB"))).ToString();
                            lstGsmReport.Add(gsmReportEntity);
                        }
                        catch (DivideByZeroException ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "GetReportData(DateTime fromDate, DateTime toDate, out decimal saTotal, out decimal faTotal, out s", ex);
                        }
                    }
                    meterIDSwap = meterIDCheck;
                }
            }
            else if (ds.Tables[0].Rows.Count==0)
            {
                logger.Log(LOGLEVELS.Error, "Record Not found Between these dates");
            }
            return lstGsmReport;
        }

        public List<GSMConfigReportEntity> GetReadReportData(DateTime fromDate, DateTime toDate, string taskname)
        {
            string meterIDCheck = string.Empty;
            string meterIDSwap = string.Empty;
            List<GSMConfigReportEntity> lstGsmReport = new List<GSMConfigReportEntity>();
            GSMConfigReportEntity gsmReportEntity;
            IDataHelper helper = DatabaseFactory.GetHelper();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            StringBuilder builder = new StringBuilder();
            // builder.Append("select MeterID,SimNo,Status,Reason,TaskID,creationDateTime from gsmconfigstatus where STR_TO_DATE(creationDateTime, '%d/%m/%Y %H:%i:%s') between STR_TO_DATE('" + fromDate + "', '%d/%m/%Y %H:%i:%s') and STR_TO_DATE('" + toDate + "'" +
            //  " ,'%d/%m/%Y %H:%i:%s') and TaskID ='" + taskname + "'");

            builder.Append("select MeterID,SimNo,Status,Reason,TaskID,creationDateTime from gsmreadstatus where TaskID ='" + taskname + "'");
            DataRequest request = new DataRequest(builder.ToString());
            DataSet ds = new DataSet();
            ds = helper.FillDataSet(request, ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count + 1; i++)
                {
                    //extracting meter ID
                    if (i < ds.Tables[0].Rows.Count)
                        meterIDCheck = ds.Tables[0].Rows[i]["MeterID"].ToString();

                    //enter if it is different meter id or always enter if it is last row
                    if ((meterIDCheck != meterIDSwap && i != 0) || i == ds.Tables[0].Rows.Count)
                    {
                        try
                        {
                            gsmReportEntity = new GSMConfigReportEntity();
                            gsmReportEntity.Meter_ID = Convert.ToInt32(ds.Tables[0].Rows[i - 1]["MeterID"]);
                            gsmReportEntity.Sim_No = ds.Tables[0].Rows[i - 1]["SimNo"].ToString();
                            gsmReportEntity.Status = ds.Tables[0].Rows[i - 1]["Status"].ToString();
                            gsmReportEntity.Reason = ds.Tables[0].Rows[i - 1]["Reason"].ToString();
                            gsmReportEntity.TaskName = ds.Tables[0].Rows[i - 1]["TaskID"].ToString();
                            gsmReportEntity.Reading_DateTime = (Convert.ToDateTime(ds.Tables[0].Rows[i - 1]["creationDateTime"].ToString(), new System.Globalization.CultureInfo("en-GB"))).ToString();
                            lstGsmReport.Add(gsmReportEntity);
                        }
                        catch (DivideByZeroException ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "GetReportData(DateTime fromDate, DateTime toDate, out decimal saTotal, out decimal faTotal, out s", ex);
                        }
                    }
                    meterIDSwap = meterIDCheck;
                }
            }
            else if (ds.Tables[0].Rows.Count == 0)
            {
                logger.Log(LOGLEVELS.Error, "Record Not found Between these dates");
            }
            return lstGsmReport;
        }

        public DataSet GetTaskName(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();
            //DateTime dt1 = new DateTime(fromDate.Day, ).
            //DateTime dt1 = fromDate.to
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            string FromDate = fromDate.Day.ToString() + "/" + fromDate.Month.ToString() + "/" + fromDate.Year + " 00:00:00 ";
            string ToDate = toDate.Day.ToString() + "/" + toDate.Month.ToString() + "/" + toDate.Year + " 23:59:59 ";
           // string startdate = String.Format("{0}", fromDate);
           // string fromdate = String.Format("{0:dd/MM/yy 23:59:59}", toDate);
            builder.Append("select distinct TaskID from gsmconfigstatus where STR_TO_DATE( creationDateTime, '%d/%m/%Y %H:%i:%s') between STR_TO_DATE('" + FromDate + "', '%d/%m/%Y %H:%i:%s') and STR_TO_DATE('" + ToDate + "'" +
               " ,'%d/%m/%Y %H:%i:%s')order by TaskID desc ");
           // builder.Append("select distinct TaskID from gsmconfigstatus order by TaskID desc");
            DataRequest request = new DataRequest(builder.ToString());
            ds = helper.FillDataSet(request, ds);
            return ds;
        }

        public DataSet ReadTaskName(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = new DataSet();           
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            string FromDate = fromDate.Day.ToString() + "/" + fromDate.Month.ToString() + "/" + fromDate.Year + " 00:00:00 ";
            string ToDate = toDate.Day.ToString() + "/" + toDate.Month.ToString() + "/" + toDate.Year + " 23:59:59 ";
           builder.Append("select distinct TaskID from gsmreadstatus where STR_TO_DATE( creationDateTime, '%d/%m/%Y %H:%i:%s') between STR_TO_DATE('" + FromDate + "', '%d/%m/%Y %H:%i:%s') and STR_TO_DATE('" + ToDate + "'" +
           " ,'%d/%m/%Y %H:%i:%s')order by TaskID desc ");           
            DataRequest request = new DataRequest(builder.ToString());
            ds = helper.FillDataSet(request, ds);
            return ds;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return false;
            try
            {
                GSMConfigEntity gSMConfigEntity = entity as GSMConfigEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update meter_master Set ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                builder.Append(string.Concat(" Where ", Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), gSMConfigEntity.Meter_ID, DbType.String, 20);
                request.AddParamter(ParameterName(Meter_Phone), gSMConfigEntity.SIM_Number, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule ", gSMConfigEntity.Meter_ID, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Meter_ID as 'Meter ID',Meter_Phone as 'Meter Phone' from Meter_Master where Meter_Status=1");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        } 
        public IEntity GetDetailData(string id)
        {
            GSMConfigEntity configEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), id, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            configEntity = (GSMConfigEntity)RowToEntity(ds.Tables[0].Rows[0]);
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string id)", ex);
                configEntity = null;
            }
            return configEntity;
        }

        


        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GSMConfigEntity configEntity = new GSMConfigEntity();
            if (NotNullAndNotDBNull(row, Meter_ID)) configEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
            if (NotNullAndNotDBNull(row, Meter_Phone)) configEntity.SIM_Number = Convert.ToInt64(row[Meter_Phone]);
            return configEntity;
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEntity GetDetailData(long id)
        {
            GSMConfigEntity configEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    configEntity = (GSMConfigEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(long id)", ex);
                configEntity = null;
            }
            return configEntity;
        }
        public int GetCount(long id)
        {
            int counter = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Meter_ID,Meter_Phone from Meter_Master where ");
                builder.Append(string.Concat(Meter_Phone, "=", ParameterName(Meter_Phone)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_Phone), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                        counter = ds.Tables[0].Rows.Count;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SIM Number viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCount(long id)", ex);
                counter = 0;
            }
            return counter;
        }
        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

    }
}
