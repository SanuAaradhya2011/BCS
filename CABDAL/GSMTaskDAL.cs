using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using Utilities;
using CAB.Framework.Utility;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;
namespace CAB.DALC.Data
{
    public class GSMTaskDAL : DALBase
    {
        private string groupId = "groupId";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMTaskDAL).ToString());
        
        /// <summary>
        /// Function to get list of tasks to be executed
        /// </summary>
        /// <returns></returns>
        public DataSet GetGPRSTasks()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GetTasks", CommandType.StoredProcedure);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGPRSTasks()", ex);
            }
            
            return dataSet;
        }


        /// <summary>
        /// Method to update database for GPRS schedule
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="taskId"></param>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="taskRetries"></param>
        /// <param name="errorMessage"></param>
        /// <param name="Job"></param>
        /// <returns></returns>
        public GSMTaskEntity UpdateGPRSTask(string meterId, int taskId,
          int groupId, string status, int taskRetries, string errorMessage, string Job)
        {

            GSMTaskEntity gsmTaskEntity = null;
            string strGroupName = string.Empty;
            string[] separator = null;
            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("UpdateTask");
                DataRequest request = new DataRequest(builder.ToString(), CommandType.StoredProcedure);
                request.AddParamter(ParameterName("meterId"), meterId, DbType.String, 20);
                request.AddParamter(ParameterName("taskId"), taskId, DbType.Int32, 20);
                request.AddParamter(ParameterName("groupId"), groupId, DbType.Int64, 20);
                request.AddParamter(ParameterName("status"), status, DbType.String, 15);
                request.AddParamter(ParameterName("taskretries"), taskRetries, DbType.Int32, 20);
                request.AddParamter(ParameterName("errorMessage"), errorMessage, DbType.String, 200);
                request.AddParamter(ParameterName("Job"), Job, DbType.String, 50);
                helper.FillDataSet(request, ds);


                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(ds.Tables[0].Rows[i]["tasksId"].ToString());
                        gsmTaskEntity.taskName = ds.Tables[0].Rows[i]["taskName"].ToString();

                        strGroupName = new GSMTaskDAL().getGroupNameAgainstGroupId(ds.Tables[0].Rows[i]["groupId"].ToString());

                        if (strGroupName != "")
                        {
                            gsmTaskEntity.groupName = strGroupName;
                        }

                        gsmTaskEntity.groupId = Convert.ToInt32(ds.Tables[0].Rows[i]["groupId"].ToString());
                        gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[0]["creationDateTime"].ToString();
                        gsmTaskEntity.startDate = ds.Tables[0].Rows[i]["startDate"].ToString();
                        gsmTaskEntity.startTime = ds.Tables[0].Rows[i]["startTime"].ToString();
                        gsmTaskEntity.taskType = ds.Tables[0].Rows[i]["taskType"].ToString();
                        gsmTaskEntity.tasksToBeRepeated = ds.Tables[0].Rows[i]["repeatTask"].ToString();
                     //   gsmTaskEntity.JobDetails = ds.Tables[0].Rows[i]["jobdetails"].ToString();

                        separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();

                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                        {
                            if (separator.Length > 0)
                            {
                                int dayOfMonth = 0;
                                if (int.TryParse(separator[0], out dayOfMonth))
                                    gsmTaskEntity.dayOfMonth = dayOfMonth;
                                gsmTaskEntity.MonthList = new string[separator.Length - 1];
                                Array.Copy(separator, 1, gsmTaskEntity.MonthList, 0, separator.Length - 1);
                            }
                        }
                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                        {
                            if (separator.Length > 0)
                            {
                                gsmTaskEntity.DayNameList = new string[separator.Length];
                                gsmTaskEntity.DayNameList = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                            }
                        }
                        gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();

                        string[] arrJobs = gsmTaskEntity.jobNames.Split(',');
                        int jobCount = gsmTaskEntity.jobNames.Split(',').Length;

                        gsmTaskEntity.isGeneralRequired = false;
                        gsmTaskEntity.isBillingRequired = false;
                        gsmTaskEntity.isInstantaneousRequired = false;

                        for (int j = 0; j < jobCount; j++)
                        {
                            if (arrJobs[j].ToLower().Trim() == "general")
                            {
                                gsmTaskEntity.isGeneralRequired = true;
                            }
                            else if (arrJobs[j].ToLower().Trim() == "billing")
                            {
                                gsmTaskEntity.isBillingRequired = true;
                            }
                            else if (arrJobs[j].ToLower().Trim() == "instantaneous")
                            {
                                gsmTaskEntity.isInstantaneousRequired = true;
                            }
                        }

                        gsmTaskEntity.taskStatus = ds.Tables[0].Rows[i]["taskStatus"].ToString();
                        gsmTaskEntity.taskRetries = Convert.ToInt32(ds.Tables[0].Rows[i]["taskRetries"].ToString());
                        gsmTaskEntity.taskPriority = Convert.ToInt32(ds.Tables[0].Rows[i]["taskPriority"].ToString());
                    }
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateGPRSTask(string meterId, int taskId,int groupId, string status, int taskRetries, string errorMessage, string Job)", ex);
            }

            return gsmTaskEntity;
        }


        public DataSet RequeueFailedTasks()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("ReQueueFailedTasks", CommandType.StoredProcedure);
                helper.FillDataSet(request,dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "RequeueFailedTasks()", ex);
            }

            return dataSet;
        }

        public string ParameterName(string parameterName)
        {
            return string.Concat(DatabaseFactory.GetPlaceholder(), parameterName);
        }
        
        //Declare the default constructor
        public GSMTaskDAL()
            : base("gsm_tasks", "tasksID")
        {
            // Do nothing
        }
        public bool InsertGSMTask(GSMTaskEntity gsmTaskEntity)
        {
            bool Flag = false;
            string taskID = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsm_tasks(taskName,groupId,creationDateTime,startDate,startTime,taskType,repeatTask,jobs,taskRetries,taskPriority,taskStatus) values('");
                builder.Append(gsmTaskEntity.taskName + "',");
                builder.Append(gsmTaskEntity.groupId + ",'");
                builder.Append(gsmTaskEntity.CreationDateTime + "','");
                builder.Append(gsmTaskEntity.startDate + "','");
                builder.Append(gsmTaskEntity.startTime + "','");
                builder.Append(gsmTaskEntity.taskType + "','");
                builder.Append(gsmTaskEntity.tasksToBeRepeated + "','");
                builder.Append(gsmTaskEntity.jobNames + "',");
           //     builder.Append(gsmTaskEntity.JobDetails + "',");
                builder.Append(gsmTaskEntity.taskRetries + ",");
                builder.Append(gsmTaskEntity.taskPriority + ",");
                if (gsmTaskEntity.taskStatus == "Inactive")
                    builder.Append("'Inactive')");
                else
                    builder.Append("'Inqueue')");

                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                //Get the max id from the table and update the gsm task entity
                if (gsmTaskEntity.jobNames.Contains(JobType.LoadSurveyPartialFrom.ToString()))
                {
                    taskID = GetPK();
                    if (!string.IsNullOrEmpty(taskID))
                    {
                        gsmTaskEntity.taskId = int.Parse(taskID);
                        InsertGSMLoadSurveyTask(gsmTaskEntity);
                    }
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data added"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertGSMTask(GSMTaskEntity gsmTaskEntity)", ex);
                throw ex.InnerException;
            }
            return Flag;
        }
        /// <summary>
        /// Inserts the load survey task information in gsm_tasks_loadsurvey table
        /// 
        /// </summary>
        /// <param name="gsmTaskEntity"></param>
        public void InsertGSMLoadSurveyTask(GSMTaskEntity gsmTaskEntity)
        {
            bool Flag = false;
            string taskID = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsm_tasks_loadsurvey(tasksID,LoadSurvey_FromDate,LoadSurvey_ToDate) values(");
                builder.Append(gsmTaskEntity.taskId + ",");
                builder.Append(DateUtility.DateTimeToLong(gsmTaskEntity.LoadSurveyFromDate) + ",");
                builder.Append(DateUtility.DateTimeToLong(gsmTaskEntity.LoadSurveyToDate) + ")");

                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task Load Survey data added"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertGSMLoadSurveyTask(GSMTaskEntity gsmTaskEntity)", ex);
                throw ex.InnerException;
            }
        }

        //ashish
        public bool InsertCompleteTask(GSMTaskEntity gsmTaskEntity)
        {
            bool Flag = false;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsm_tasks_completed(tasksId,taskName,creationDateTime,groupId,startDate,startTime,taskType,repeatTask,jobs,taskRetries,taskPriority,taskStatus) values(");
                builder.Append(gsmTaskEntity.taskId + ",'");
                builder.Append(gsmTaskEntity.taskName + "','");
                builder.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "',");
                builder.Append(gsmTaskEntity.groupId + ",'");
                builder.Append(gsmTaskEntity.startDate + "','");
                builder.Append(gsmTaskEntity.startTime + "','");
                builder.Append(gsmTaskEntity.taskType + "','");
                builder.Append(gsmTaskEntity.tasksToBeRepeated + "','");
                builder.Append(gsmTaskEntity.jobNames + "',");
                builder.Append(gsmTaskEntity.taskRetries + ",");
                builder.Append(gsmTaskEntity.taskPriority + ",");
                builder.Append("'Completed')");

                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data added"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertCompleteTask(GSMTaskEntity gsmTaskEntity)", ex);
                throw ex.InnerException;
            }
            return Flag;
        }

        //ashish
        public GSMTaskEntity GetCompletedTaskByID(int taskID)
        {
            GSMTaskEntity gsmTaskEntity = null;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from gsm_tasks_completed where tasksId = " + taskID.ToString());

                DataRequest request = new DataRequest(builder.ToString());
                DataSet ds = new DataSet();
                helper.FillDataSet(request, ds);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data added"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(ds.Tables[0].Rows[i]["tasksId"].ToString());
                        gsmTaskEntity.taskName = ds.Tables[0].Rows[i]["taskName"].ToString();
                        gsmTaskEntity.groupName = getGroupNameAgainstGroupId(ds.Tables[0].Rows[i]["groupId"].ToString());
                        gsmTaskEntity.groupId = Convert.ToInt32(ds.Tables[0].Rows[i]["groupId"].ToString());
                        gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[i]["creationDateTime"].ToString();
                        gsmTaskEntity.startDate = ds.Tables[0].Rows[i]["startDate"].ToString();
                        gsmTaskEntity.startTime = ds.Tables[0].Rows[i]["startTime"].ToString();
                        gsmTaskEntity.taskType = ds.Tables[0].Rows[i]["taskType"].ToString();
                        gsmTaskEntity.tasksToBeRepeated = ds.Tables[0].Rows[i]["repeatTask"].ToString();
                        gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();
                        gsmTaskEntity.taskStatus = ds.Tables[0].Rows[i]["taskStatus"].ToString();
                        gsmTaskEntity.taskRetries = Convert.ToInt32(ds.Tables[0].Rows[i]["taskRetries"].ToString());
                        gsmTaskEntity.taskPriority = Convert.ToInt32(ds.Tables[0].Rows[i]["taskPriority"].ToString());
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetCompletedTaskByID(int taskID)", ex);
                throw ex.InnerException;
            }
            return gsmTaskEntity;
        }

        public GSMTaskEntity GetTaskByTaskID(int taskID)
        {
            DataSet ds = null;
            GSMTaskEntity gsmTaskEntity = null;
            string strGroupName = string.Empty;
            string[] separator = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select * from gsm_tasks where tasksId = " + taskID.ToString());
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data added"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(ds.Tables[0].Rows[i]["tasksId"].ToString());
                        gsmTaskEntity.taskName = ds.Tables[0].Rows[i]["taskName"].ToString();

                        strGroupName = getGroupNameAgainstGroupId(ds.Tables[0].Rows[i]["groupId"].ToString());

                        if (strGroupName != "")
                        {
                            gsmTaskEntity.groupName = strGroupName;
                        }

                        gsmTaskEntity.groupId = Convert.ToInt32(ds.Tables[0].Rows[i]["groupId"].ToString());
                        gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[0]["creationDateTime"].ToString();
                        gsmTaskEntity.startDate = ds.Tables[0].Rows[i]["startDate"].ToString();
                        gsmTaskEntity.startTime = ds.Tables[0].Rows[i]["startTime"].ToString();
                        gsmTaskEntity.taskType = ds.Tables[0].Rows[i]["taskType"].ToString();
                        gsmTaskEntity.tasksToBeRepeated = ds.Tables[0].Rows[i]["repeatTask"].ToString();

                        separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();

                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                        {
                            if (separator.Length > 0)
                            {
                                int dayOfMonth = 0;
                                if (int.TryParse(separator[0], out dayOfMonth))
                                    gsmTaskEntity.dayOfMonth = dayOfMonth;
                                gsmTaskEntity.MonthList = new string[separator.Length - 1];
                                Array.Copy(separator, 1, gsmTaskEntity.MonthList, 0, separator.Length - 1);
                            }
                        }
                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                        {
                            if (separator.Length > 0)
                            {
                                gsmTaskEntity.DayNameList = new string[separator.Length];
                                gsmTaskEntity.DayNameList = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                            }
                        }
                        gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();

                        string[] arrJobs = gsmTaskEntity.jobNames.Split(',');
                        int jobCount = gsmTaskEntity.jobNames.Split(',').Length;

                        gsmTaskEntity.isGeneralRequired = false;
                        gsmTaskEntity.isBillingRequired = false;
                        gsmTaskEntity.isInstantaneousRequired = false;

                        for (int jobsCounter = 0; jobsCounter < jobCount; jobsCounter++)
                        {
                            if (arrJobs[jobsCounter].ToLower().Trim() == "general")
                            {
                                gsmTaskEntity.isGeneralRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "billing")
                            {
                                gsmTaskEntity.isBillingRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "instantaneous")
                            {
                                gsmTaskEntity.isInstantaneousRequired = true;
                            }
                            // set loadsurvey and tamper if these are present in task
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "loadsurvey")
                            {
                                gsmTaskEntity.IsLoadSurveyRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "tamper")
                            {
                                gsmTaskEntity.IsTamperRequired = true;
                            }
                        }

                        gsmTaskEntity.taskStatus = ds.Tables[0].Rows[i]["taskStatus"].ToString();
                        gsmTaskEntity.taskRetries = Convert.ToInt32(ds.Tables[0].Rows[i]["taskRetries"].ToString());
                        gsmTaskEntity.taskPriority = Convert.ToInt32(ds.Tables[0].Rows[i]["taskPriority"].ToString());
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTaskByTaskID(int taskID)", ex);
                throw ex.InnerException;
            }
            return gsmTaskEntity;
        }

        //ashish
        public List<GSMTaskEntity> getReportSchedules(DateTime fromDate, DateTime toDate, string scheduleType, string groupId, string taskStatus)
        {
            List<GSMTaskEntity> colGSMTaskEntity = new List<GSMTaskEntity>();
            GSMTaskEntity gsmTaskEntity;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                //task status
                builder.Append("Select * from ");
                if (taskStatus == "Completed")
                    builder.Append("gsm_tasks_completed where taskStatus = 'Completed'");
                else if (taskStatus == "All")
                    builder.Append("gsm_tasks_completed");
                else if (taskStatus == "Inqueue")
                    builder.Append("gsm_tasks where taskStatus = 'Inqueue'");
                else if (taskStatus == "Inprogress")
                    builder.Append("gsm_tasks where taskStatus = 'Inprogress'");
                else if (taskStatus == "Inactive")
                    builder.Append("gsm_tasks where taskStatus = 'Inactive'");

                if (taskStatus == "All")
                {
                    //group
                    if (groupId != "All")
                        builder.Append(" where groupId = " + groupId);
                    //schedule
                    if (scheduleType != "All" && groupId != "All")
                        builder.Append(" and taskType = '" + scheduleType + "'");
                    else if (scheduleType != "All")
                        builder.Append(" where taskType = '" + scheduleType + "'");

                    builder.Append(" UNION Select * from gsm_tasks");
                    //group
                    if (groupId != "All")
                        builder.Append(" where groupId = " + groupId);
                    //schedule
                    if (scheduleType != "All" && groupId != "All")
                        builder.Append(" and taskType = '" + scheduleType + "'");
                    else if (scheduleType != "All")
                        builder.Append(" where taskType = '" + scheduleType + "'");
                }
                else
                {
                    //group
                    if (groupId != "All")
                        builder.Append(" and groupId = " + groupId);
                    //schedule
                    if (scheduleType != "All")
                        builder.Append(" and taskType = '" + scheduleType + "'");
                }

                DataSet ds = new DataSet();
                DataRequest request = new DataRequest(builder.ToString());
                helper.FillDataSet(request, ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime date = DateTime.MinValue;
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();

                        date = DateTime.Parse(ds.Tables[0].Rows[i]["startDate"].ToString());

                        if (date >= fromDate && date <= toDate)
                        {
                            gsmTaskEntity.taskName = ds.Tables[0].Rows[i]["taskName"].ToString();
                            gsmTaskEntity.groupName = getGroupNameAgainstGroupId(ds.Tables[0].Rows[i]["groupId"].ToString());
                            gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[i]["creationDateTime"].ToString();
                            gsmTaskEntity.startDate = ds.Tables[0].Rows[i]["startDate"].ToString();
                            gsmTaskEntity.startTime = ds.Tables[0].Rows[i]["startTime"].ToString();
                            gsmTaskEntity.taskType = ds.Tables[0].Rows[i]["taskType"].ToString();
                            gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();
                            gsmTaskEntity.taskStatus = ds.Tables[0].Rows[i]["taskStatus"].ToString();

                            colGSMTaskEntity.Add(gsmTaskEntity);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "getReportSchedules(DateTime fromDate, DateTime toDate, string scheduleType, string groupId, string taskStatus)", ex);
            }

            return colGSMTaskEntity;
        }

        //ashish
        public List<GSMTaskEntity> GetFilteredScheduledTasks(string taskStatus)
        {
            List<GSMTaskEntity> lstGSMTaskEntity = new List<GSMTaskEntity>();
            GSMTaskEntity gsmTaskEntity;
            string[] separator = null;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
              builder.Append("Select g.*,gl.* from gsm_groups,gsm_tasks g left join gsm_tasks_loadsurvey gl on g.tasksID = gl.tasksID where  g.groupID=gsm_groups.group_ID  and g.taskStatus = '" + taskStatus + "' order by STR_TO_DATE(startDate, '%d/%m/%Y'), startTime LIMIT 0,1");
              
                DataSet ds = new DataSet();
                DataRequest request = new DataRequest(builder.ToString());
                helper.FillDataSet(request, ds);

                if (ds !=null && ds.Tables!=null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int rowCounter = 0; rowCounter < ds.Tables[0].Rows.Count; rowCounter++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(ds.Tables[0].Rows[rowCounter]["tasksId"].ToString());
                        gsmTaskEntity.taskName = ds.Tables[0].Rows[rowCounter]["taskName"].ToString();
                        gsmTaskEntity.groupName = getGroupNameAgainstGroupId(ds.Tables[0].Rows[rowCounter]["groupId"].ToString());
                        gsmTaskEntity.groupId = Convert.ToInt32(ds.Tables[0].Rows[rowCounter]["groupId"].ToString());
                        gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[rowCounter]["creationDateTime"].ToString();
                        gsmTaskEntity.startDate = ds.Tables[0].Rows[rowCounter]["startDate"].ToString();
                        gsmTaskEntity.startTime = ds.Tables[0].Rows[rowCounter]["startTime"].ToString();
                        gsmTaskEntity.taskType = ds.Tables[0].Rows[rowCounter]["taskType"].ToString();
                        gsmTaskEntity.tasksToBeRepeated = ds.Tables[0].Rows[rowCounter]["repeatTask"].ToString();                      

                        //condition for monthly task
                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                        {
                            separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                            if (separator.Length > 0)
                            {
                                int dayOfMonth = 0;
                                if (int.TryParse(separator[0], out dayOfMonth))
                                    gsmTaskEntity.dayOfMonth = dayOfMonth;
                                gsmTaskEntity.MonthList = new string[separator.Length - 1];
                                Array.Copy(separator, 1, gsmTaskEntity.MonthList, 0, separator.Length - 1);
                            }
                        }
                        //condition for weekely task
                        if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                        {
                            separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                            gsmTaskEntity.DayNameList = new string[separator.Length];
                            gsmTaskEntity.DayNameList = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                        }

                        gsmTaskEntity.jobNames = ds.Tables[0].Rows[rowCounter]["jobs"].ToString();

                        string[] arrJobs = gsmTaskEntity.jobNames.Split(',');
                        int jobCount = gsmTaskEntity.jobNames.Split(',').Length;

                        gsmTaskEntity.isGeneralRequired = false;
                        gsmTaskEntity.isBillingRequired = false;
                        gsmTaskEntity.isInstantaneousRequired = false;
                        gsmTaskEntity.IsMidnightRequired = false;
                        gsmTaskEntity.IsMeterConfigRequired = false;

                        for (int jobsCounter = 0; jobsCounter < jobCount; jobsCounter++)
                        {
                            if (arrJobs[jobsCounter].ToLower().Trim() == JobType.General.ToString().ToLower())
                            {
                                gsmTaskEntity.isGeneralRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == JobType.Billing.ToString().ToLower())
                            {
                                gsmTaskEntity.isBillingRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == JobType.Instantaneous.ToString().ToLower())
                            {
                                gsmTaskEntity.isInstantaneousRequired = true;
                            }
                            // fill load survey and tamper data
                            else if ((arrJobs[jobsCounter].ToLower().Trim().StartsWith("loadsurvey")))
                            {
                                gsmTaskEntity.IsLoadSurveyRequired = true;
                                gsmTaskEntity.LoadSurveyJobType = (JobType)Enum.Parse(typeof(JobType), arrJobs[jobsCounter]);
                                if (gsmTaskEntity.LoadSurveyJobType == JobType.LoadSurveyPartialFrom)
                                {
                                    gsmTaskEntity.LoadSurveyFromDate = NotNullAndNotDBNull(ds.Tables[0].Rows[rowCounter], "loadsurvey_fromdate") ? DateUtility.LongToDateTime(Convert.ToInt64(ds.Tables[0].Rows[rowCounter]["loadsurvey_fromdate"])) : DateTime.MinValue;
                                    gsmTaskEntity.LoadSurveyToDate = NotNullAndNotDBNull(ds.Tables[0].Rows[rowCounter], "loadsurvey_todate") ? DateUtility.LongToDateTime(Convert.ToInt64(ds.Tables[0].Rows[rowCounter]["loadsurvey_todate"])) : DateTime.MinValue;
                                }
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "tamper")
                            {
                                gsmTaskEntity.IsTamperRequired = true;
                            }
                  
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "midnight")
                            {
                                gsmTaskEntity.IsMidnightRequired = true;
                            }
                            else if (arrJobs[jobsCounter].ToLower().Trim() == "meterconfiguration")
                            {
                                gsmTaskEntity.IsMeterConfigRequired = true;
                            }
                        }
                        
                        gsmTaskEntity.taskStatus = ds.Tables[0].Rows[rowCounter]["taskStatus"].ToString();
                        gsmTaskEntity.taskRetries = Convert.ToInt32(ds.Tables[0].Rows[rowCounter]["taskRetries"].ToString());
                        gsmTaskEntity.taskPriority = Convert.ToInt32(ds.Tables[0].Rows[rowCounter]["taskPriority"].ToString());

                        lstGSMTaskEntity.Add(gsmTaskEntity);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFilteredScheduledTasks(string taskStatus)", ex);
                throw ex.InnerException;
            }
            return lstGSMTaskEntity;
        }

        public List<GSMTaskEntity> GetScheduledTasks(string taskStatus)
        {
            List<GSMTaskEntity> colGSMTaskEntity = new List<GSMTaskEntity>();
            GSMTaskEntity gsmTaskEntity;
            string[] separator = null;

            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                string strGroupName = "";

                StringBuilder builder = new StringBuilder();
                if (taskStatus == "Completed")
                    builder.Append("Select * from gsm_tasks_completed order by STR_TO_DATE(creationDateTime,'%d/%m/%Y %h:%m:%s %t')");// start date converted to datetime type to implment correct sorting
                else if (taskStatus == "All")
                    builder.Append("Select * from gsm_tasks_completed UNION Select * from gsm_tasks order by taskName");// order by startDate DESC clause removed as the startDate is of varchar type; 23 march 2012
                else if (taskStatus == "Inqueue")
                    builder.Append("Select * from gsm_tasks where taskStatus = 'Inqueue' order by STR_TO_DATE(startDate, '%d/%m/%Y'), startTime");
                else if (taskStatus == "Inprogress")
                    builder.Append("Select * from gsm_tasks where taskStatus = 'Inprogress' order by STR_TO_DATE(startDate, '%d/%m/%Y'), startTime");
                else if (taskStatus == "Inactive")
                    builder.Append("Select * from gsm_tasks where taskStatus = 'Inactive' order by STR_TO_DATE(startDate, '%d/%m/%Y'), startTime");
                else if (taskStatus == "validate")
                    builder.Append("Select * from gsm_tasks");
                else
                    builder.Append("Select * from gsm_tasks where taskStatus = 'Inprogress' OR  taskStatus = 'Inqueue' order by STR_TO_DATE(startDate, '%d/%m/%Y'), startTime");

                DataSet ds = new DataSet();
                DataRequest request = new DataRequest(builder.ToString());
                helper.FillDataSet(request, ds);

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data is retrieved"));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(ds.Tables[0].Rows[i]["tasksId"].ToString());
                        gsmTaskEntity.taskName = ds.Tables[0].Rows[i]["taskName"].ToString();
                        strGroupName = getGroupNameAgainstGroupId(ds.Tables[0].Rows[i]["groupId"].ToString());
                        if (strGroupName != "")
                            gsmTaskEntity.groupName = strGroupName;
                        gsmTaskEntity.groupId = Convert.ToInt32(ds.Tables[0].Rows[i]["groupId"].ToString());
                        gsmTaskEntity.CreationDateTime = ds.Tables[0].Rows[i]["creationDateTime"].ToString();
                        gsmTaskEntity.startDate = ds.Tables[0].Rows[i]["startDate"].ToString();
                        gsmTaskEntity.startTime = ds.Tables[0].Rows[i]["startTime"].ToString();
                        gsmTaskEntity.taskType = ds.Tables[0].Rows[i]["taskType"].ToString();
                        gsmTaskEntity.tasksToBeRepeated = ds.Tables[0].Rows[i]["repeatTask"].ToString();

                        if (taskStatus != "Completed" && taskStatus != "All")
                        {
                            //condition for monthly task
                            if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                            {
                                separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                                if (separator.Length > 0)
                                {
                                    int dayOfMonth = 0;
                                    if (int.TryParse(separator[0], out dayOfMonth))
                                        gsmTaskEntity.dayOfMonth = dayOfMonth;
                                    gsmTaskEntity.MonthList = new string[separator.Length - 1];
                                    Array.Copy(separator, 1, gsmTaskEntity.MonthList, 0, separator.Length - 1);
                                }
                            }
                            //condition for weekely task
                            if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                            {
                                separator = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                                gsmTaskEntity.DayNameList = new string[separator.Length];
                                gsmTaskEntity.DayNameList = gsmTaskEntity.tasksToBeRepeated.CommaSplit();
                            }

                            gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();

                            string[] arrJobs = gsmTaskEntity.jobNames.Split(',');
                            int jobCount = gsmTaskEntity.jobNames.Split(',').Length;

                            gsmTaskEntity.isGeneralRequired = false;
                            gsmTaskEntity.isBillingRequired = false;
                            gsmTaskEntity.isInstantaneousRequired = false;

                            for (int jobCounter = 0; jobCounter < jobCount; jobCounter++)
                            {
                                if (arrJobs[jobCounter].ToLower().Trim() == "general")
                                {
                                    gsmTaskEntity.isGeneralRequired = true;
                                }
                                else if (arrJobs[jobCounter].ToLower().Trim() == "billing")
                                {
                                    gsmTaskEntity.isBillingRequired = true;
                                }
                                else if (arrJobs[jobCounter].ToLower().Trim() == "instantaneous")
                                {
                                    gsmTaskEntity.isInstantaneousRequired = true;
                                }

                                else if ((arrJobs[jobCounter].ToLower().Trim().StartsWith("loadsurvey")))
                                {
                                    gsmTaskEntity.IsLoadSurveyRequired = true;
                                    gsmTaskEntity.LoadSurveyJobType = (JobType)Enum.Parse(typeof(JobType), arrJobs[jobCounter]);
                                }
                                else if (arrJobs[jobCounter].ToLower().Trim() == "tamper")
                                {
                                    gsmTaskEntity.IsTamperRequired = true;
                                }
                            }
                        }
                        else
                        {
                            gsmTaskEntity.jobNames = ds.Tables[0].Rows[i]["jobs"].ToString();
                        }

                        gsmTaskEntity.taskStatus = ds.Tables[0].Rows[i]["taskStatus"].ToString();
                        gsmTaskEntity.taskRetries = Convert.ToInt32(ds.Tables[0].Rows[i]["taskRetries"].ToString());
                        gsmTaskEntity.taskPriority = Convert.ToInt32(ds.Tables[0].Rows[i]["taskPriority"].ToString());

                        colGSMTaskEntity.Add(gsmTaskEntity);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetScheduledTasks(string taskStatus)", ex);
                throw ex.InnerException;
            }
            return colGSMTaskEntity;
        }

        public string getGroupNameAgainstGroupId(string GroupId)
        {
            GSMGroupDAL grpDAL = new GSMGroupDAL();
            DataSet ds = grpDAL.ListGroupData();

            string strGroupName = "";

            if (ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["Group ID"].ToString() == GroupId)
                        {
                            strGroupName = ds.Tables[0].Rows[i]["Group Name"].ToString();
                            return strGroupName;
                        }
                    }
                }
            }
            return strGroupName;
        }

        public bool deleteGSMTasks(List<GSMTaskEntity> colTaskEntity)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                string strTaskIds = "";

                foreach (GSMTaskEntity gsmTaskEntity in colTaskEntity)
                {
                    strTaskIds = strTaskIds + gsmTaskEntity.taskId + ",";
                }

                if (strTaskIds.EndsWith(","))
                {
                    strTaskIds = strTaskIds.Substring(0, strTaskIds.Length - 1);
                }

                builder.Append("Delete from gsm_tasks where tasksId in (" + strTaskIds + ")");
                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                //Delete load survey tasks is available
                DeleteGSMLoadSurveyTasks(strTaskIds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "deleteGSMTasks(List<GSMTaskEntity> colTaskEntity)", ex);
                throw ex.InnerException;
            }

            return Flag;
        }
        /// <summary>
        /// Funtion is for deleting load survey tasks, if available
        /// </summary>
        /// <param name="taskIDs"></param>
        private void DeleteGSMLoadSurveyTasks(string taskIDs)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from gsm_tasks_loadsurvey where tasksId in (" + taskIDs + ")");
                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task Load survey data deleted"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteGSMLoadSurveyTasks(string taskIDs)", ex);
                throw ex.InnerException;
            }
        }
        
        public bool updateGSMTasks(List<GSMTaskEntity> colTaskEntity)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = null;

                foreach (GSMTaskEntity gsmTaskEntity in colTaskEntity)
                {
                    builder = new StringBuilder();
                    //updatedTaskEntity =  UpdateStartTime(gsmTaskEntity);
                    builder.Append("Update gsm_tasks set taskStatus = '" + gsmTaskEntity.taskStatus + "',startTime = '" + gsmTaskEntity.startTime + "',startDate = '" + gsmTaskEntity.startDate + "' where tasksId = " + gsmTaskEntity.taskId);
                    DataRequest request = new DataRequest(builder.ToString());
                    helper.ExecuteNonQuery(request);
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data updated"));
                    Flag = true;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "updateGSMTasks(List<GSMTaskEntity> colTaskEntity)", ex);
                throw ex.InnerException;
            }
            return Flag;
        }

        public bool UpdateGSMTask(GSMTaskEntity gsmTaskEntity)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = null;
                builder = new StringBuilder();
                builder.Append("Update gsm_tasks set taskStatus = '" + gsmTaskEntity.taskStatus + "',startTime = '" + gsmTaskEntity.startTime + "',startDate = '" + gsmTaskEntity.startDate + "' where tasksId = " + gsmTaskEntity.taskId);

                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data updated"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateGSMTask(GSMTaskEntity gsmTaskEntity)", ex);
                throw ex.InnerException;
            }
            return Flag;
        }

        public bool updateGSMTasksStatus(List<GSMTaskEntity> colTaskEntity)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = null;

                foreach (GSMTaskEntity gsmTaskEntity in colTaskEntity)
                {
                    builder = new StringBuilder();
                    builder.Append("Update gsm_tasks set taskStatus = '" + gsmTaskEntity.taskStatus + "' where tasksId = " + gsmTaskEntity.taskId.ToString());
                    DataRequest request = new DataRequest(builder.ToString());
                    helper.ExecuteNonQuery(request);
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Task data updated"));
                    Flag = true;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "updateGSMTasksStatus(List<GSMTaskEntity> colTaskEntity)", ex);
                throw ex.InnerException;
            }
            return Flag;
        }

        public GSMTaskEntity UpdateStartTime(GSMTaskEntity gsmTaskEntity)
        {
            int hour = 0;
            int minutes = 0;
            int repeatInHours = 0;
            int repeatInDays = 0;
            if (gsmTaskEntity != null)
            {
                hour = Convert.ToInt32(gsmTaskEntity.StartHour);
                minutes = Convert.ToInt32(gsmTaskEntity.StartMinute);
                //if task type is daily
                if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Daily))
                {
                    if (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(DailyTask.EveryDay))
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(1).ToShortDateTimeCABFormat();
                    }
                    else if (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(DailyTask.WeekDays))
                    {
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                        {
                            gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(3).ToShortDateTimeCABFormat();
                        }
                        else
                        {
                            gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(1).ToShortDateTimeCABFormat();
                        }
                    }
                    else if (int.TryParse(gsmTaskEntity.tasksToBeRepeated, out repeatInDays))
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(repeatInDays).ToShortDateTimeCABFormat();
                    }
                }
                //if task type is weekly
                else if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                {
                    int dayDiff = 0;
                    if (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.AllWeekDays))
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(1).ToShortDateTimeCABFormat();
                    }
                    else if ((gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Monday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Tuesday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Wednesday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Thursday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Friday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Saturday)) || (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(WeeklyTask.Sunday)))
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(7).ToShortDateTimeCABFormat();
                    }
                    else if (gsmTaskEntity.DayNameList.Length > 0)
                    {


                        for (int counter = 0; counter < gsmTaskEntity.DayNameList.Length; counter++)
                        {
                            if (Convert.ToDateTime(gsmTaskEntity.startDate).DayOfWeek.ToString() == gsmTaskEntity.DayNameList[counter])
                            {
                                if (counter == gsmTaskEntity.DayNameList.Length - 1)
                                    dayDiff = DayDiff(gsmTaskEntity.DayNameList[counter], gsmTaskEntity.DayNameList[0]);
                                else
                                    dayDiff = DayDiff(gsmTaskEntity.DayNameList[counter], gsmTaskEntity.DayNameList[counter + 1]);

                                break;
                            }
                        }
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddDays(dayDiff).ToShortDateTimeCABFormat();
                    }
                }
                //if task type is monthly
                else if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                {
                    int monthDiff = 0;
                    if (gsmTaskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(MonthlyTask.AllMonths))
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddMonths(1).ToShortDateTimeCABFormat();
                    }
                    else if (gsmTaskEntity.MonthList.Length > 1)
                    {
                        for (int counter = 0; counter < gsmTaskEntity.MonthList.Length; counter++)
                        {
                            if (Convert.ToDateTime(gsmTaskEntity.startDate).Month.ToString() == gsmTaskEntity.MonthList[counter])
                            {
                                if (counter == gsmTaskEntity.MonthList.Length - 1)
                                    monthDiff = MonthDiff(gsmTaskEntity.MonthList[counter], gsmTaskEntity.MonthList[0]);
                                else
                                    monthDiff = MonthDiff(gsmTaskEntity.MonthList[counter], gsmTaskEntity.MonthList[counter + 1]);
                                break;
                            }
                        }
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddMonths(monthDiff).ToShortDateTimeCABFormat();
                    }
                    else
                    {
                        gsmTaskEntity.startDate = Convert.ToDateTime(gsmTaskEntity.startDate).AddYears(1).ToShortDateTimeCABFormat();
                    }
                }
            }
            return gsmTaskEntity;
        }
        /// <summary>
        /// Get all task ids associated with a group.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public DataSet GetTasksByGroupId(int inputGroupId)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TasksID as 'TaskId' from gsm_tasks where ");
                builder.Append(string.Concat(groupId, "=", ParameterName(groupId)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(groupId), inputGroupId, DbType.Int32);                                          
                ds = new DataSet();
                helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tasks ID(s) sretrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetTasksByGroupId(int inputGroupId)", ex);
                ds = null;
            }
            return ds;
        }
       /// <summary>
        /// Used to delete tasks that are already completed and moved to gsm_tasks_completed table .
        /// Here we need both taskid's and group id's bcz tables gsm_tasks_completed and gsm_tasks_loadsurvey have different taskId's for same task
        /// and gsm_tasks_loadsurvey does not have groupid.
       /// </summary>
       /// <param name="colTaskEntity"></param>
       /// <param name="inputGroupId"></param>
       /// <returns></returns>
        public bool DeleteCompletedTasks(List<GSMTaskEntity> colTaskEntity, int inputGroupId)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                string strTaskIds = "";

                foreach (GSMTaskEntity gsmTaskEntity in colTaskEntity)
                {
                    strTaskIds = strTaskIds + gsmTaskEntity.taskId + ",";
                }

                if (strTaskIds.EndsWith(","))
                {
                    strTaskIds = strTaskIds.Substring(0, strTaskIds.Length - 1);
                }
                
                builder.Append("Delete from gsm_tasks_completed where ");
                builder.Append(string.Concat(groupId, "=", ParameterName(groupId)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(groupId), inputGroupId, DbType.Int32);
                helper.ExecuteNonQuery(request);
                //Delete load survey tasks if available
                DeleteGSMLoadSurveyTasks(strTaskIds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Completed GSM Task data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteCompletedTasks(List<GSMTaskEntity> colTaskEntity, int inputGroupId)", ex);
                throw ex.InnerException;
            }

            return Flag;
        }

        private int DayDiff(string currentDay, string futureDay)
        {
            int currentDayNo = 0;
            int futureDayNo = 0;
            currentDayNo = GetDayNo(currentDay);
            futureDayNo = GetDayNo(futureDay);
            if (futureDayNo > currentDayNo)
                return futureDayNo - currentDayNo;
            else
                return 7 + futureDayNo - currentDayNo;
        }

        private int MonthDiff(string currentMonth, string futureMonth)
        {
            int currentMonthNo = 0;
            int futureMonthNo = 0;
            currentMonthNo = GetMonthNo(currentMonth);
            futureMonthNo = GetMonthNo(futureMonth);
            if (futureMonthNo > currentMonthNo)
                return futureMonthNo - currentMonthNo;
            else
                return 12 + futureMonthNo - currentMonthNo;
        }

        private int GetDayNo(string currentDay)
        {
            int dayNo = 0;
            switch (currentDay.ToLower())
            {
                case "monday":
                    dayNo = 1;
                    break;
                case "tuesday":
                    dayNo = 2;
                    break;
                case "wednesday":
                    dayNo = 3;
                    break;
                case "thursday":
                    dayNo = 4;
                    break;
                case "friday":
                    dayNo = 5;
                    break;
                case "saturday":
                    dayNo = 6;
                    break;
                case "sunday":
                    dayNo = 7;
                    break;
                default:
                    dayNo = 0;
                    break;
            }
            return dayNo;
        }

        private int GetMonthNo(string month)
        {
            int monthNo = 0;
            switch (month.ToLower())
            {
                case "jan":
                    monthNo = 1;
                    break;
                case "feb":
                    monthNo = 2;
                    break;
                case "mar":
                    monthNo = 3;
                    break;
                case "apr":
                    monthNo = 4;
                    break;
                case "may":
                    monthNo = 5;
                    break;
                case "jun":
                    monthNo = 6;
                    break;
                case "jul":
                    monthNo = 7;
                    break;
                case "aug":
                    monthNo = 8;
                    break;
                case "sep":
                    monthNo = 9;
                    break;
                case "oct":
                    monthNo = 10;
                    break;
                case "nov":
                    monthNo = 11;
                    break;
                case "dec":
                    monthNo = 12;
                    break;
                default:
                    monthNo = 0;
                    break;
            }
            return monthNo;
        }
        //Yatin 13-Jan-2012 Returns true when any active task for the meterid exists in the database (Inqueue, Inprogress, and Completed tasks are checked for).
        public bool DoesActiveTaskExistsForMeterID(string pMeterID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select count(distinct gt.tasksid) from gsm_tasks gt inner join gsm_group_meters ggm on gt.groupid = ggm.group_id ");
                builder.Append(" where ggm.meter_id = '" + pMeterID + "' and gt.taskstatus <> 'Inactive'");

                DataSet ds = new DataSet();
                DataRequest request = new DataRequest(builder.ToString());
                if (Convert.ToInt32(helper.ExecuteScalar(request)) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DoesActiveTaskExistsForMeterID(string pMeterID)", ex);
                throw ex.InnerException;
            }
            return false;
        }
        //Functions required for implementing abstract base class.
    
        public override IEntity InsertData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<CAB.Framework.Entity.IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
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

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
