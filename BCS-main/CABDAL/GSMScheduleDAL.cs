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
    public class GSMScheduleDAL : DALBase 
    {
        private string gsmSchedule_ID = "gsmSchedule_ID";
        private string Schedule_Name = "Schedule_Name";
        private string Schedule_Period = "Schedule_Period";
        private string Period_DayName = "Period_DayName";
        private string Period_DayNumber = "Period_DayNumber";
        private string ActivationDate = "ActivationDate";
        private string ActivationTime = "ActivationTime";
        private string CreationDate = "CreationDate";
        private string Schedule_Parameter = "Schedule_Parameter"; 
        private string Status = "Status";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMScheduleDAL).ToString());
        public GSMScheduleDAL()
            : base("gsmSchedule", "gsmSchedule_ID")
        {
        }

        public override IEntity InsertData(IEntity entity)
        {
            if (entity == null)
                return null;
            GSMScheduleEntity gsmScheduleEntity = entity as GSMScheduleEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsmSchedule(Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status) values(");
                builder.Append(string.Concat(ParameterName(Schedule_Name), ","));
                builder.Append(string.Concat(ParameterName(Schedule_Period), ","));
                builder.Append(string.Concat(ParameterName(Period_DayName), ","));
                builder.Append(string.Concat(ParameterName(Period_DayNumber), ","));
                builder.Append(string.Concat(ParameterName(ActivationDate), ","));
                builder.Append(string.Concat(ParameterName(ActivationTime), ","));
                builder.Append(string.Concat(ParameterName(CreationDate), ","));
                builder.Append(string.Concat(ParameterName(Schedule_Parameter), ",")); 
                builder.Append(string.Concat(ParameterName(Status), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Schedule_Name), gsmScheduleEntity.Schedule_Name, DbType.String, 80);
                request.AddParamter(ParameterName(Schedule_Period), gsmScheduleEntity.Schedule_Period, DbType.String, 2);
                request.AddParamter(ParameterName(Period_DayName), gsmScheduleEntity.Period_DayName, DbType.String, 40);
                request.AddParamter(ParameterName(Period_DayNumber), gsmScheduleEntity.Period_DayNumber, DbType.Int32);
                request.AddParamter(ParameterName(ActivationDate), gsmScheduleEntity.ScheduleActivationDate, DbType.Int64);
                request.AddParamter(ParameterName(ActivationTime), gsmScheduleEntity.ScheduleActivationTime, DbType.String, 10);
                request.AddParamter(ParameterName(CreationDate), gsmScheduleEntity.ScheduleCreationDate, DbType.Int64);
                request.AddParamter(ParameterName(Schedule_Parameter), gsmScheduleEntity.Schedule_Parameter, DbType.String, 200); 
                request.AddParamter(ParameterName(Status), gsmScheduleEntity.Status, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data added")); 
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            if (Flag)
                gsmScheduleEntity.GSMSchedule_ID = long.Parse(this.GetPK());
            return gsmScheduleEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        { 
            bool Flag = false;
            if (entity == null)
                return Flag;
            GSMScheduleEntity gsmScheduleEntity = entity as GSMScheduleEntity;
            try
            { 
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();  
                builder.Append("Update gsmSchedule Set ");
                builder.Append(string.Concat(Schedule_Name, "=", ParameterName(Schedule_Name),","));
                builder.Append(string.Concat(Schedule_Period, "=", ParameterName(Schedule_Period), ","));
                builder.Append(string.Concat(Period_DayName, "=", ParameterName(Period_DayName), ","));
                builder.Append(string.Concat(Period_DayNumber, "=", ParameterName(Period_DayNumber), ","));
                builder.Append(string.Concat(ActivationDate, "=", ParameterName(ActivationDate), ","));
                builder.Append(string.Concat(ActivationTime, "=", ParameterName(ActivationTime), ","));
                builder.Append(string.Concat(CreationDate, "=", ParameterName(CreationDate), ","));
                builder.Append(string.Concat(Schedule_Parameter, "=", ParameterName(Schedule_Parameter), ",")); 
                builder.Append(string.Concat(Status, "=", ParameterName(Status)));
                builder.Append(string.Concat(" Where ", gsmSchedule_ID, "=", ParameterName(gsmSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Schedule_Name), gsmScheduleEntity.Schedule_Name, DbType.String, 80);
                request.AddParamter(ParameterName(Schedule_Period), gsmScheduleEntity.Schedule_Period, DbType.String, 2);
                request.AddParamter(ParameterName(Period_DayName), gsmScheduleEntity.Period_DayName, DbType.String, 40);
                request.AddParamter(ParameterName(Period_DayNumber), gsmScheduleEntity.Period_DayNumber, DbType.Int32);
                request.AddParamter(ParameterName(ActivationDate), gsmScheduleEntity.ScheduleActivationDate, DbType.Int64);
                request.AddParamter(ParameterName(ActivationTime), gsmScheduleEntity.ScheduleActivationTime, DbType.String, 10);
                request.AddParamter(ParameterName(CreationDate), gsmScheduleEntity.ScheduleCreationDate, DbType.Int64);
                request.AddParamter(ParameterName(Schedule_Parameter), gsmScheduleEntity.Schedule_Parameter, DbType.String, 200);
                request.AddParamter(ParameterName(Status), gsmScheduleEntity.Status, DbType.Int32);
                request.AddParamter(ParameterName(gsmSchedule_ID), gsmScheduleEntity.GSMSchedule_ID, DbType.Int64); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule of ", Schedule_Period, " modified"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return Flag;
            GSMScheduleEntity gsmScheduleEntity = entity as GSMScheduleEntity; 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from gsmSchedule where ");
                builder.Append(string.Concat(gsmSchedule_ID, "=", ParameterName(gsmSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(gsmSchedule_ID), gsmScheduleEntity.GSMSchedule_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            GSMScheduleEntity gsmScheduleEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule where ");
                builder.Append(string.Concat(gsmSchedule_ID, "=", ParameterName(gsmSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(gsmSchedule_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    gsmScheduleEntity = (GSMScheduleEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                gsmScheduleEntity = null;
            }
            return gsmScheduleEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
        public DataSet ComboListDataSet()
        {
            DataSet dataSet = null; 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID as ValueMember,Schedule_Name as DisplayMember from gsmSchedule");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ComboListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule");
                DataRequest request = new DataRequest(builder.ToString());
                 dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet); 
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public   DataSet ListDataSet(string columnName, string value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule where ");
                builder.Append(string.Concat(columnName, " like '%", value, "%'"));
                DataRequest request = new DataRequest(builder.ToString()); 
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string columnName, string value)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListDataSet(long fromDate, long toDate,string mode)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule where ");
                if (mode == "Activation")
                    builder.Append(string.Concat(ActivationDate, ">", fromDate, " and ", ActivationDate, "<=", toDate));
                else
                    builder.Append(string.Concat(CreationDate, ">", fromDate, " and ", CreationDate, "<=", toDate));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(long fromDate, long toDate,string mode)", ex);
                dataSet = null;
            }
            return dataSet;
        } 
        public DataSet ListDataSet(string columnName, int value)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule where ");
                builder.Append(string.Concat(columnName, "=", value));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(string columnName, int value)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet ListScheduleDataSet()
        {
            DataSet dataSet = null;
            try
            {
                string dates = DateUtility.DateTimeToLong(System.DateTime.Now).ToString();
                long fromDate = Convert.ToInt64(dates.Substring(0, 8) + "000000");
                long toDate = Convert.ToInt64(dates.Substring(0, 8) + "235959");
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select gsmSchedule_ID,Schedule_Name,Schedule_Period,Period_DayName,Period_DayNumber,ActivationDate,ActivationTime,CreationDate,Schedule_Parameter,Status from gsmSchedule where Status=1 and ");
                builder.Append(string.Concat(CreationDate, " between ", fromDate," and ",toDate));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListScheduleDataSet()", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            GSMScheduleEntity gsmScheduleEntity = new GSMScheduleEntity();
            if (NotNullAndNotDBNull(row, gsmSchedule_ID)) gsmScheduleEntity.GSMSchedule_ID = Convert.ToInt64(row[gsmSchedule_ID]);
            if (NotNullAndNotDBNull(row, Schedule_Name)) gsmScheduleEntity.Schedule_Name = Convert.ToString(row[Schedule_Name]);
            if (NotNullAndNotDBNull(row, Schedule_Period)) gsmScheduleEntity.Schedule_Period = Convert.ToString(row[Schedule_Period]);
            if (NotNullAndNotDBNull(row, Period_DayName)) gsmScheduleEntity.Period_DayName = Convert.ToString(row[Period_DayName]);
            if (NotNullAndNotDBNull(row, Period_DayNumber)) gsmScheduleEntity.Period_DayNumber = Convert.ToInt32(row[Period_DayNumber]);
            if (NotNullAndNotDBNull(row, ActivationDate)) gsmScheduleEntity.ScheduleActivationDate = Convert.ToInt64(row[ActivationDate]);
            if (NotNullAndNotDBNull(row, ActivationTime)) gsmScheduleEntity.ScheduleActivationTime = Convert.ToString(row[ActivationTime]);
            if (NotNullAndNotDBNull(row, ActivationDate)) gsmScheduleEntity.ScheduleCreationDate = Convert.ToInt64(row[CreationDate]);
            if (NotNullAndNotDBNull(row, Schedule_Parameter)) gsmScheduleEntity.Schedule_Parameter = Convert.ToString(row[Schedule_Parameter]); 
            if (NotNullAndNotDBNull(row, Status)) gsmScheduleEntity.Status = Convert.ToInt32(row[Status]);
            return gsmScheduleEntity;
        }

        public bool IsPortFree(long scheduleID, long startDateTime,long endDateTime,string portName)
        {
            bool flag=false;
            try
            {
                //ActivationDate,ActivationTime,CreationDate
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from gsmSchedule where ");
                builder.Append(string.Concat(ActivationDate, " between ", ParameterName("Start"), " and "));
                builder.Append(string.Concat( ParameterName("End"), " and "));
                builder.Append(string.Concat(ActivationTime, "=", ParameterName(ActivationTime))); 
                if (scheduleID != 0)
                    builder.Append(string.Concat(" and ", gsmSchedule_ID, "<>", ParameterName(gsmSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("Start"), startDateTime, DbType.Int64);
                request.AddParamter(ParameterName("End"), endDateTime, DbType.Int64);
                request.AddParamter(ParameterName(ActivationTime), portName, DbType.String, 50);
                if (scheduleID != 0)
                    request.AddParamter(ParameterName(gsmSchedule_ID), scheduleID, DbType.Int64);
                object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    flag = false;
                else
                {
                    if (data.ToString().Equals("0"))
                        flag = false;
                    else
                        flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsPortFree(long scheduleID, long startDateTime,long endDateTime,string portName)", ex);
                flag = false;
            }
            return flag;
        }

        public long ValidateData(long scheduleID,string consumerNumber, string meterNumber, string meterSimNumber)
        {
            long num = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from gsmSchedule where ");
                builder.Append(string.Concat(Schedule_Name, "=", ParameterName(Schedule_Name) ," and "));
                builder.Append(string.Concat(Period_DayName, "=", ParameterName(Period_DayName), " and "));
                builder.Append(string.Concat(Period_DayNumber, "=", ParameterName(Period_DayNumber))); 
                if(scheduleID!=0)
                    builder.Append(string.Concat(" and ",gsmSchedule_ID, "<>", ParameterName(gsmSchedule_ID))); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Schedule_Name), consumerNumber, DbType.String, 80);
                request.AddParamter(ParameterName(Period_DayName), meterNumber, DbType.String, 80);
                request.AddParamter(ParameterName(Period_DayNumber), meterSimNumber, DbType.String, 80);
                if (scheduleID != 0)
                    request.AddParamter(ParameterName(gsmSchedule_ID), scheduleID, DbType.Int64);
                object data = helper.ExecuteScalar(request);
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    num = 0;
                else
                    num = Convert.ToInt64(data);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Schedule data viewed."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateData(long scheduleID,string consumerNumber, string meterNumber, string meterSimNumber)", ex);
                num = 0;
            }
            return num;
        }
        public DataSet GetCustomerMeterInformationList()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select A.Schedule_Name as 'Consumer Number',A.Schedule_Period as 'Consumer Name',B.Meter_ID as 'Meter Number',D.MeterType_Name as 'Meter Type',E.metermodel_Name as 'Meter Model' from consumer_master A,consumermeter B,meter_master C,metertype_master D,metermodel_master E where A.Schedule_Name=B.Schedule_Name and C.Meter_ID=B.Meter_ID and C.MeterType_ID=D.MeterType_ID and E.MeterModel_ID=C.MeterModel_ID");
                DataRequest request = new DataRequest(builder.ToString()); 
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer Meter data"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCustomerMeterInformationList()", ex);
                dataSet = null;
            }
            return dataSet;
           
        }
        public bool ValidateSchedule(IEntity entity)
        {
            bool Flag = false;
            if (entity == null)
                return false;
            try
            {
                GSMScheduleEntity gsmScheduleEntity = entity as GSMScheduleEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from gsmSchedule where ");
                builder.Append(string.Concat(Schedule_Name, "=", ParameterName(Schedule_Name)));
                if (gsmScheduleEntity.GSMSchedule_ID != 0)
                    builder.Append(string.Concat(gsmSchedule_ID, "<>", ParameterName(gsmSchedule_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Schedule_Name), gsmScheduleEntity.Schedule_Name, DbType.String, 80);
                if (gsmScheduleEntity.GSMSchedule_ID != 0)
                    request.AddParamter(ParameterName(gsmSchedule_ID), gsmScheduleEntity.GSMSchedule_ID, DbType.Int64);
                if (Convert.ToInt32(helper.ExecuteScalar(request)) > 0)
                    Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer Meter data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateSchedule(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public DataSet GroupScheduleDataList()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select A.Schedule_Name,A.Schedule_Period,A.Period_DayName,A.Period_DayNumber,A.ActivationDate,A.ActivationTime,A.Schedule_Parameter,B.GSMSchedule_ID,B.Meter_ID,B.GSMGroupSchedule_ID ");
                builder.Append("From gsmschedule A,gsmgroupschedule B where A.GSMSchedule_ID=B.GSMSchedule_ID and A.Status=1");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group schedule data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GroupScheduleDataList()", ex);
                dataSet = null;
            }
            return dataSet;
        }
    }
}
