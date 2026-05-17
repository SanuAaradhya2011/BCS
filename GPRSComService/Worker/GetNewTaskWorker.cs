using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using System.Threading;
using GPRSComService.Tasks;
using CAB.DALC.Data;
using System.Data;
using Hunt.EPIC.Logging;
using System.Text;
using System.Configuration;
using CAB.Framework;
using CAB.Framework.Utility;

namespace GPRSComService.Worker
{
    class GetNewTaskWorker : WorkerBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GetNewTaskWorker).ToString());
        List<TaskBase> taskList = new List<TaskBase>();
        string utilityName = string.Empty;
        //Get The constant value from app.config.
        int taskTimeOutDuration = Convert.ToInt16(ConfigurationSettings.AppSettings[Constants.constTaskExpiryTimeOutMinutes].ToString());
        private bool continueExecution = true;

        protected override void OnStart(object parameters)
        {
            logger.Log(LOGLEVELS.Info, "GetNewTaskWorker started");
            try
            {
                GSMTaskDAL taskDAL = new GSMTaskDAL();
                taskDAL.RequeueFailedTasks();
                logger.Log(LOGLEVELS.Info, "Old task updation completed.");
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while updating inprogess task during startup", ex);
            }
            ProcessNewTasks();
        }

        /// <summary>
        /// Process tje New Task. Polls the database for new task and send it to
        /// </summary>
        private void ProcessNewTasks()
        {
            while (continueExecution)
            {
                List<TaskBase> newTasks = getTasks();
                if (newTasks.Count > 0)
                {
                    taskList.AddRange(newTasks);
                }
                else
                {
                    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constNewTaskWorkerIdleTimeout)));
                }
                if (taskList.Count > 0)
                {
                    ProcessTask(taskList);
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            continueExecution = false;
        }

        /// <summary>
        /// Returns the logged in utility name 
        /// </summary>
        private string UtilityName
        {
            get
            {
                if(string.IsNullOrEmpty(utilityName))
                {
                    utilityName= UtilityDAL.GetUtility().Tables[0].Rows[0]["Utility_Name"].ToString();
                }
                return utilityName;
            }
        }
        /// <summary>
        /// Method push the newly fetched task to Task Manager. 
        /// If task can be picked then Task manager adds it to its list.
        /// </summary>
        /// <param name="newTasks"></param>
        private void ProcessTask(List<TaskBase> newTasks)
        {
            TaskManager.TryAdd(ref newTasks);
        }

        /// <summary>
        /// Returns the List of Task Base. 
        /// Polls the Database and create instance of task base and returns
        /// </summary>
        /// <returns></returns>
        private List<TaskBase> getTasks()
        {
           
            // need to move this code to BLL but that need the TaskBase and other generic classes to move to common DLL 
            List<TaskBase> lstTask = new List<TaskBase>();
            GSMTaskDAL gsmTaskDAL = new GSMTaskDAL();
            try
            {
                logger.Log(LOGLEVELS.Info, "New Task pickup started ");
                
                DataSet dsTasks = gsmTaskDAL.GetGPRSTasks();

               

                if (dsTasks != null && dsTasks.Tables != null && dsTasks.Tables.Count > 0 &&   dsTasks.Tables[0].Rows.Count > 0)
                {
                    logger.Log(LOGLEVELS.Info, string.Format("Number of sub tasks picked for execution are:", dsTasks.Tables[0].Rows.Count.ToString()));

                    foreach (DataRow dr in dsTasks.Tables[0].Rows)
                    {
                        TaskBase task = TaskFactory.getTaskInstance(dr["JOB"].ToString().ToUpper());

                        task.IMEINumber = dr["IMEI"].ToString();
                        task.TaskId = Convert.ToInt32(dr["TASK ID"].ToString());
                        task.MeterId = dr["METER ID"].ToString();
                        task.GroupId = Convert.ToInt32(dr["GROUP ID"].ToString());
                        
                        task.TaskName = dr["TASK NAME"].ToString();

                        task.MeterModel =(MeterModels) Convert.ToInt16(dr["METER MODEL ID"].ToString());

                        string[] startDate = dr["START DATE"].ToString().Split('/');
                        string[] startTime = dr["START TIME"].ToString().Split(':');

                        string fileName = string.Concat(task.MeterId, "_", 
                                    startDate[0].ToString(), "_", 
                                    startDate[1].ToString(),"_",
                                    startDate[2].ToString(), "_",
                                    startTime[0].ToString(),"_",
                                    startTime[1].ToString(),"_",
                                    task.TaskId,".2NG");

                        task.FileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, fileName);
                        task.UtilityName =this.UtilityName;
                        task.RetryCount = Int16.Parse(dr["RETRIES COUNT"].ToString());
                        task.TaskExpiryDate = DateTime.Now.AddMinutes(taskTimeOutDuration);
                        task.init();
                        task.Status = GPRSComService.Tasks.TaskStatus.None;

                        if (task is LoadSurveyTask)
                        {
                            LoadSurveyTask loadSurvey = task as LoadSurveyTask;
                            loadSurvey.FromDate = DateUtility.LongToDateTime(Convert.ToInt64(dr["FROMDATE"].ToString()));
                            loadSurvey.ToDate = DateUtility.LongToDateTime(Convert.ToInt64(dr["TODATE"].ToString()));
                        }
                        if (!TaskManager.TryAdd(task))
                        {
                            lstTask.Add(task);
                        }
                        logger.Log(LOGLEVELS.Info, string.Format("Task picked from DB for Task: {0}, Meter: {1}, Modem{2}", task.JobName, task.MeterId, task.IMEINumber)); 
                    }
                }
                logger.Log(LOGLEVELS.Info, "New Task pickup completed. ");
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while getting new task", ex);
            }
            return lstTask;
            
        }
    }
}
