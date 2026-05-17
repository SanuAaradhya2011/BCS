using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GSMLoggingDAL : DALBase
    {
        private const string Log_ID = "Log_ID";
        private const string Task_ID = "tasksId";
        private const string Group_ID = "Group_ID";
        private const string Meter_ID = "Meter_ID";
        private const string IsGeneralCompleted = "isGeneralCompleted";
        private const string IsInstantCompleted = "isInstantCompleted";
        private const string IsBillingCompleted = "isBillingCompleted";
        private const string IsLoadSurveyCompleted = "isLoadSurveyCompleted";
        private const string IsTamperCompleted = "isTamperCompleted";
        private const string IsDailyLoadCompleted = "isDailyLoadCompleted";
        private const string Retries = "taskRetries";
        private const string CreationDateTime = "creationDateTime";
        private const string Status = "Status";
        private const string ErrorMessage = "errorMessage";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMLoggingDAL).ToString());

        public GSMLoggingDAL()
            : base("gsm_task_logs", "Log_ID")
        { }

        public override IEntity GetDetailData(int id)
        { return null; }

        public List<GSMLoggingEntity> GetLogsByTaskID(int id)
        {
            List<GSMLoggingEntity> listGSMLogEntity = new List<GSMLoggingEntity>();
            GSMLoggingEntity gsmLogEntity = null;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder = new StringBuilder();
                //adding load survey and tamper in select query
                builder.Append("Select Log_ID,tasksID,Group_ID,Meter_ID,isGeneralCompleted,isInstantCompleted,isBillingCompleted,isLoadSurveyCompleted,isTamperCompleted,Status,taskRetries,creationDateTime,errorMessage,isDailyLoadCompleted from gsm_task_logs where ");
                builder.Append(string.Concat(Task_ID, "=", ParameterName(Task_ID)));
                builder.Append(string.Concat(" order by creationDateTime DESC"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Task_ID), id, DbType.Int64);

                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        gsmLogEntity = (GSMLoggingEntity)RowToEntity(row);
                        listGSMLogEntity.Add(gsmLogEntity);
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Log details viewed on the selected log ID."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLogsByTaskID(int id)", ex);
                listGSMLogEntity = null;
            }
            return listGSMLogEntity;
        }

        public IEntity InsertData(IEntity entity, bool flag)
        {
            if (entity == null)
                return entity;

            GSMLoggingEntity gsmLogEntity = entity as GSMLoggingEntity;
            bool Flag = false;
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsm_task_logs(" + Task_ID + "," + Group_ID + "," + Meter_ID + "," + IsGeneralCompleted + "," + IsInstantCompleted + "," + IsBillingCompleted + "," + IsLoadSurveyCompleted + "," + IsTamperCompleted + "," + Status + "," + Retries + "," + CreationDateTime + "," + ErrorMessage + "," + IsDailyLoadCompleted + ") values(");
                builder.Append(string.Concat(ParameterName(Task_ID), ","));
                builder.Append(string.Concat(ParameterName(Group_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_ID), ","));
                builder.Append(string.Concat(ParameterName(IsGeneralCompleted), ","));
                builder.Append(string.Concat(ParameterName(IsInstantCompleted), ","));
                builder.Append(string.Concat(ParameterName(IsBillingCompleted), ","));
                //Adding load survey and tamper in logs
                builder.Append(string.Concat(ParameterName(IsLoadSurveyCompleted), ","));
                builder.Append(string.Concat(ParameterName(IsTamperCompleted), ","));
                builder.Append(string.Concat(ParameterName(Status), ","));
                builder.Append(string.Concat(ParameterName(Retries), ","));
                builder.Append(string.Concat(ParameterName(CreationDateTime), ","));
                builder.Append(string.Concat(ParameterName(ErrorMessage), ","));
                builder.Append(string.Concat(ParameterName(IsDailyLoadCompleted), ")"));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Task_ID), gsmLogEntity.Task_ID, DbType.Int32);
                request.AddParamter(ParameterName(Group_ID), gsmLogEntity.Group_ID, DbType.Int64);
                request.AddParamter(ParameterName(Meter_ID), gsmLogEntity.Meter_ID, DbType.String);
                request.AddParamter(ParameterName(IsGeneralCompleted), gsmLogEntity.IsGeneralCompleted == true ? 1 : 0, DbType.Int32);
                request.AddParamter(ParameterName(IsInstantCompleted), gsmLogEntity.IsInstantCompleted == true ? 1 : 0, DbType.Int32);
                request.AddParamter(ParameterName(IsBillingCompleted), gsmLogEntity.IsBillingCompleted == true ? 1 : 0, DbType.Int32);
                //Adding load survey and tamper in logs
                request.AddParamter(ParameterName(IsLoadSurveyCompleted), gsmLogEntity.IsLoadSurveyCompleted == true ? 1 : 0, DbType.Int32);
                request.AddParamter(ParameterName(IsTamperCompleted), gsmLogEntity.IsTamperCompleted == true ? 1 : 0, DbType.Int32);
                request.AddParamter(ParameterName(Status), gsmLogEntity.Status, DbType.String);
                request.AddParamter(ParameterName(Retries), gsmLogEntity.Retries, DbType.Int32);
                request.AddParamter(ParameterName(CreationDateTime), DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString(), DbType.String);
                request.AddParamter(ParameterName(ErrorMessage), gsmLogEntity.ErrorMessage, DbType.String);
                request.AddParamter(ParameterName(IsDailyLoadCompleted), gsmLogEntity.IsMidNightCompleted == true ? 1 : 0, DbType.Int32);
                helper.ExecuteNonQuery(request);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New GSM Log Added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity, bool flag)", ex);
                Flag = false;
            }
            if (Flag)
                gsmLogEntity.Log_ID = Convert.ToInt32(this.GetPK());
            return gsmLogEntity;
        }

        public IEntity InsertorUpdateData(IEntity entity, bool flag)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            if (entity == null)
                return entity;

            GSMLoggingEntity gsmLogEntity = entity as GSMLoggingEntity;
            bool Flag = false;

            try
            {
                if (gsmLogEntity.Log_ID > 0)
                    UpdateData(gsmLogEntity);
                else
                    InsertData(gsmLogEntity, false);

                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertorUpdateData(IEntity entity, bool flag)", ex);
                Flag = false;
            }

            if (Flag)
                gsmLogEntity.Log_ID = Convert.ToInt32(this.GetPK());

            return gsmLogEntity;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";

            GSMLoggingEntity gsmLoggingEntity = new GSMLoggingEntity();

            gsmLoggingEntity.Log_ID = NotNullAndNotDBNull(row, Log_ID) ? Convert.ToInt32(row[Log_ID]) : 0;
            gsmLoggingEntity.Task_ID = NotNullAndNotDBNull(row, Task_ID) ? Convert.ToInt32(row[Task_ID]) : 0;
            gsmLoggingEntity.Group_ID = NotNullAndNotDBNull(row, Group_ID) ? Convert.ToInt32(row[Group_ID]) : 0;
            gsmLoggingEntity.Meter_ID = NotNullAndNotDBNull(row, Meter_ID) ? row[Meter_ID].ToString() : string.Empty;
            gsmLoggingEntity.IsGeneralCompleted = NotNullAndNotDBNull(row, IsGeneralCompleted) ? (Convert.ToInt32(row[IsGeneralCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.IsInstantCompleted = NotNullAndNotDBNull(row, IsInstantCompleted) ? (Convert.ToInt32(row[IsInstantCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.IsBillingCompleted = NotNullAndNotDBNull(row, IsBillingCompleted) ? (Convert.ToInt32(row[IsBillingCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.IsTamperCompleted = NotNullAndNotDBNull(row, IsTamperCompleted) ? (Convert.ToInt32(row[IsTamperCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.IsLoadSurveyCompleted = NotNullAndNotDBNull(row, IsLoadSurveyCompleted) ? (Convert.ToInt32(row[IsLoadSurveyCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.IsMidNightCompleted = NotNullAndNotDBNull(row, IsDailyLoadCompleted) ? (Convert.ToInt32(row[IsDailyLoadCompleted]) == 1 ? true : false) : false;
            gsmLoggingEntity.Status = NotNullAndNotDBNull(row, Status) ? row[Status].ToString() : string.Empty;
            gsmLoggingEntity.Retries = NotNullAndNotDBNull(row, Retries) ? Convert.ToInt32(row[Retries]) : 0;
            gsmLoggingEntity.CreationDateTime = NotNullAndNotDBNull(row, CreationDateTime) ? Convert.ToDateTime(row[CreationDateTime], dateInfo) : DateTime.Now;
            gsmLoggingEntity.ErrorMessage = NotNullAndNotDBNull(row, ErrorMessage) ? row[ErrorMessage].ToString() : string.Empty;
            return gsmLoggingEntity;
        }

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            bool flag = false;
            GSMLoggingEntity gsmLogEntity = (GSMLoggingEntity)entity as GSMLoggingEntity;
            StringBuilder builder = new StringBuilder();

            try
            {
                if (gsmLogEntity != null)
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    builder.Append("Update gsm_task_logs set " + IsGeneralCompleted + " = ");
                    builder.Append(string.Concat(ParameterName(IsGeneralCompleted), ","));
                    builder.Append(string.Concat(IsInstantCompleted, " = ", ParameterName(IsInstantCompleted), ","));
                    builder.Append(string.Concat(IsBillingCompleted, " = ", ParameterName(IsBillingCompleted), ","));
                    //Adding load survey and tamper in logs
                    builder.Append(string.Concat(IsLoadSurveyCompleted, " = ", ParameterName(IsLoadSurveyCompleted), ","));
                    builder.Append(string.Concat(IsTamperCompleted, " = ", ParameterName(IsTamperCompleted), ","));
                    builder.Append(string.Concat(IsDailyLoadCompleted, " = ", ParameterName(IsDailyLoadCompleted), ","));
                    builder.Append(string.Concat(Status, " = ", ParameterName(Status), ","));
                    builder.Append(string.Concat(Retries, " = ", ParameterName(Retries), ","));
                    builder.Append(string.Concat(ErrorMessage, " = ", ParameterName(ErrorMessage), " where "));
                    builder.Append(string.Concat(Log_ID, " = ", ParameterName(Log_ID)));
                    

                    DataRequest request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(IsGeneralCompleted), Convert.ToInt32(gsmLogEntity.IsGeneralCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(IsInstantCompleted), Convert.ToInt32(gsmLogEntity.IsInstantCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(IsBillingCompleted), Convert.ToInt32(gsmLogEntity.IsBillingCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(IsLoadSurveyCompleted), Convert.ToInt32(gsmLogEntity.IsLoadSurveyCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(IsTamperCompleted), Convert.ToInt32(gsmLogEntity.IsTamperCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(IsDailyLoadCompleted), Convert.ToInt32(gsmLogEntity.IsMidNightCompleted), DbType.Int32);
                    request.AddParamter(ParameterName(Status), gsmLogEntity.Status, DbType.String);
                    request.AddParamter(ParameterName(Retries), Convert.ToInt32(gsmLogEntity.Retries), DbType.Int32);
                    request.AddParamter(ParameterName(ErrorMessage), gsmLogEntity.ErrorMessage, DbType.String);
                    request.AddParamter(ParameterName(Log_ID), Convert.ToInt32(gsmLogEntity.Log_ID), DbType.Int64);
                    
                    helper.ExecuteNonQuery(request);
                    flag = true;

                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Log updated"));
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                flag = false;
            }
            return flag;

        }

        //public bool UpdateRetries(int retries,long lo)
        //{
        //    bool flag = false;
        //    GSMLoggingEntity gsmLogEntity = (GSMLoggingEntity)entity as GSMLoggingEntity;
        //    StringBuilder builder = new StringBuilder();
        //    try
        //    {
        //        if (gsmLogEntity != null)
        //        {
        //            IDataHelper helper = DatabaseFactory.GetHelper();
        //            builder.Append("Update gsm_task_logs set ");                   
        //            builder.Append(string.Concat(Retries, " = ", ParameterName(Retries), " where "));
        //            builder.Append(string.Concat(Log_ID, " = ", ParameterName(Log_ID)));
        //            DataRequest request = new DataRequest(builder.ToString());
        //            request.AddParamter(ParameterName(Retries), Convert.ToInt32(gsmLogEntity.Retries), DbType.Int32);
        //            request.AddParamter(ParameterName(Log_ID), Convert.ToInt32(gsmLogEntity.Log_ID), DbType.Int64);
        //            helper.ExecuteNonQuery(request);
        //            flag = true;
        //            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Log updated"));                }
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //    }
        //    return flag;

        //}

        public override bool DeleteData(IEntity entity)
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
    }
}
