using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using GPRSComService.Tasks;
using Hunt.EPIC.Logging;
using GPRSCommunication;
using System.Threading;

namespace GPRSComService
{
    class TaskManager
    {
        #region "Private Members"
        
        private static Dictionary<string, TaskBase> taskList = new Dictionary<string, TaskBase>();
        private static Queue<TaskBase> commandRequestQueue = new Queue<TaskBase>();
        private static Dictionary<string, TaskBase> modemNACommandQueue = new Dictionary<string, TaskBase>();
        private static Queue<TaskBase> fileUploadingQueue = new Queue<TaskBase>();

        private static object LockObj = new object();
        private static object FileUploadingLocker = new object();
        private static object CommandRequestLocker = new object();
        private static object ModemNALocker = new object();

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TaskManager).ToString());
        private static GPRSCommManager objComMgr = new GPRSCommManager();

        private static int itemCounter = 0;

        #endregion

        /// <summary>
        /// Returns next TasBase object in the Queue. 
        /// If item does not exists then returns null.
        /// </summary>
        public static TaskBase GetNextTask
        {
            get 
            {
                TaskBase task =null;
                lock (CommandRequestLocker)
                {
                    if (commandRequestQueue.Count() > 0)
                    {
                        task= commandRequestQueue.Dequeue();
                    }
                    return task;
                }
            }
        }

        public static void AddTaskToCommandProcessingQueue(TaskBase task)
        {
            lock (CommandRequestLocker)
            {
                commandRequestQueue.Enqueue(task);
            }
        }

        /// <summary>
        /// Adds a task to Modem not Available queue. 
        /// Task for whom modem is not available will be put in the waiting queue.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool AddTaskToModemWaitQueue(TaskBase task)
        {
            lock (ModemNALocker)
            {
                bool retValue = false;
                if (modemNACommandQueue.ContainsKey(task.IMEINumber))
                {
                    modemNACommandQueue.Add(task.IMEINumber, task);
                    retValue = true;
                }
                return retValue;
            }
        }

        /// <summary>
        /// Returns true if there are tasks waiting for modem.
        /// </summary>
        public static bool HasTaskInModemWaitQueue
        {
            get
            {
                lock (ModemNALocker)
                {
                    return (modemNACommandQueue.Count >0 )?true:false;
                }
            }
        }

        /// <summary>
        /// Adds the task to File uploading Queue.
        /// </summary>
        /// <param name="task"></param>
        public static void AddTaskToFileUploadQueue(TaskBase task)
        {
            lock (FileUploadingLocker)
            {
                fileUploadingQueue.Enqueue(task);
            }
        }

        /// <summary>
        /// Dequeue the task from FileUploadingQueue and returns
        /// </summary>
        /// <returns></returns>
        public static TaskBase GetNextFileUploadTask()
        {
            lock (FileUploadingLocker)
            {
                TaskBase task = null;

                if (fileUploadingQueue.Count() > 0)
                {
                    task = fileUploadingQueue.Dequeue();
                }
                return task;
            }
        }

        /// <summary>
        /// Try to Add the Task in main execution Task List.
        /// If Task does not exist then add and if exists then check it's status
        /// only status with complete can be replaced with new task
        /// </summary>
        /// <param name="newTaskList"></param>
        public static void TryAdd(ref List<TaskBase> newTaskList)
        {
            try
            {
                TaskBase[] copyTask = null;
                lock (LockObj)
                {
                    //Take local version of Task list from main list.
                    copyTask = new TaskBase[newTaskList.Count];
                    newTaskList.CopyTo(copyTask);
                }

                foreach (TaskBase task in copyTask)
                {
                    //If not exists then add to main list and remove it from passed task list.
                    if (!taskList.ContainsKey(task.IMEINumber) && !modemNACommandQueue.ContainsKey(task.IMEINumber))
                    {
                        taskList.Add(task.IMEINumber, task);
                        lock (CommandRequestLocker)
                        {
                            commandRequestQueue.Enqueue(task);
                        }
                        newTaskList.Remove(task);
                        logger.Log(LOGLEVELS.Info, string.Format("Task {0} picked for execution for Meter: {1}, Modem{2}", task.JobName, task.MeterId, task.IMEINumber));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, string.Format("Error while adding task to task processing queue", ex));
            }
        }

        public static bool TryAdd(TaskBase task)
        {
            bool retValue = false;
            lock (LockObj)
            {
                try
                {
                    //If not exists then add to main list and remove it from passed task list.
                    if (!taskList.ContainsKey(task.IMEINumber) && !modemNACommandQueue.ContainsKey(task.IMEINumber))
                    {
                        taskList.Add(task.IMEINumber, task);
                        lock (CommandRequestLocker)
                        {
                            commandRequestQueue.Enqueue(task);
                        }
                        retValue = true;
                        logger.Log(LOGLEVELS.Info, string.Format("Task {0} picked for execution for Meter: {1}, Modem{2}", task.JobName, task.MeterId, task.IMEINumber));
                    }
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, string.Format("Error while adding task to task processing queue", ex));
                }
            }
            return retValue;
        }

        /// <summary>
        /// Retruns the Task based on imeiNumber form master task list.
        /// </summary>
        /// <param name="imeiNumber"></param>
        /// <returns></returns>
        public static TaskBase GetTask(string imeiNumber)
        {
            TaskBase task = null;
            if (taskList.ContainsKey(imeiNumber))
            {
                task = taskList[imeiNumber];
            }
            return task;
        }


        /// <summary>
        /// Poll the new connection list from M2M Server. 
        /// If any new connection is availble then check if any task is pending for that modem/meter.
        /// </summary>
        /// <param name="imeiList"></param>
        public static void ProcessTaskForNewConnection(List<string> imeiList)
        {
            try
            {
                if (imeiList != null && imeiList.Count > 0)
                {
                    for (int i = 0; i < imeiList.Count; i++)
                    {
                        if (modemNACommandQueue.ContainsKey(imeiList[i]))
                        {
                            TaskBase task = modemNACommandQueue[imeiList[i]] as TaskBase;

                            lock (CommandRequestLocker)
                            {
                                commandRequestQueue.Enqueue(task);
                            }

                            //Remove the item from modemNaQueue. It is no longer required, since modem is available
                            modemNACommandQueue.Remove(imeiList[i]);


                            ////If Status is none that start communication 
                            //if (task.Status == TaskStatus.None)
                            //{
                            //    task.Status = TaskStatus.StartComm;
                            //    task.StartCommunication();
                            //}
                            ////Add the task to main Task queue
                            //taskList.Add(task.IMEINumber, task);


                        }

                        //No longer required.

                        //else if (taskList.ContainsKey(imeiList[i]))
                        //{
                        //    TaskBase task = taskList[imeiList[i]] as TaskBase;

                        //    //If Status is none that start communication 
                        //    if (task.Status == TaskStatus.None)
                        //    {
                        //        task.Status = TaskStatus.StartComm;
                        //        task.StartCommunication();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, string.Format("Error while Processing new connection.", ex));
            }
        }


        /// <summary>
        /// Check the status and Expired Time out for the task
        /// If time out is reached and task has not started or stuck then make it fail and remove from master list.
        /// </summary>
        public static void ProcessExpiredTasks()
        {
            List<TaskBase> copytaskList = null;
            lock (LockObj)
            {
                copytaskList = new List<TaskBase>();
                copytaskList.AddRange(taskList.Values.ToList());
                //copytaskList.AddRange(modemNACommandQueue.Values.ToList());
            }
            foreach (TaskBase task in copytaskList)
            {
                //If Completed Task
                if (task.Status == TaskStatus.Complete)
                {
                    lock (LockObj)
                    {
                        if (taskList.ContainsKey(task.IMEINumber))
                        {
                            taskList.Remove(task.IMEINumber);
                        }
                        else if (modemNACommandQueue.ContainsKey(task.IMEINumber))
                        {
                            modemNACommandQueue.Remove(task.IMEINumber);
                        }

                        logger.Log(LOGLEVELS.Info, string.Format("Execution is completed for Task id:{0}, TaskName: {1}, modemIMEI: {2}, ProfileName:{3} ", task.TaskId.ToString(), task.TaskName, task.IMEINumber, task.JobName));
                    }
                }

                //If inProgress Task and time out reached
                if (DateTime.Compare(DateTime.Now, task.TaskExpiryDate.Value) > 0)
                {
                    //If Retry is left then start communication again.
                    if (task.RetryCount > 0 && task.Status == TaskStatus.InProgress)
                    {
                        RequestCommandQueue.RemoveCommandFromNewCommandList(task.CommandId);
                        logger.Log(LOGLEVELS.Info, string.Format("Retry started for Task with id {0}.", task.IMEINumber));
                        task.RetryCount = task.RetryCount - 1;
                        task.RetryExecuted = task.RetryExecuted + 1;
                        task.StartCommunication();
                    }
                    else
                    {
                        //Mark the task as failed and removed from parent list
                        task.StatusMessage = Constants.msgTaskTimedOut;
                        task.Status = TaskStatus.Failed;
                        lock (LockObj)
                        {
                            RequestCommandQueue.RemoveCommandFromNewCommandList(task.CommandId);
                            if (taskList.ContainsKey(task.IMEINumber))
                            {
                                taskList.Remove(task.IMEINumber);
                            }
                            else if (modemNACommandQueue.ContainsKey(task.IMEINumber))
                            {
                                modemNACommandQueue.Remove(task.IMEINumber);
                            }
                        }
                        logger.Log(LOGLEVELS.Info, string.Format("Task Failed. Task id:{0}, TaskName: {1}, modemIMEI: {2}, ProfileName:{3} ", task.TaskId.ToString(), task.TaskName, task.IMEINumber, task.JobName));
                    }
                }
            }
        }

        /// <summary>
        /// Returns the list of commands that are in Progress state
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCommandIds()
        {
            List<string> commandIdList = new List<string>();

            lock (LockObj)
            {
                Dictionary<string, TaskBase> copyTaskList = new Dictionary<string, TaskBase>(taskList); 
                foreach (KeyValuePair<string, TaskBase> item in copyTaskList)
                {
                    TaskBase task = item.Value as TaskBase;
                    if (task.Status == TaskStatus.InProgress && !string.IsNullOrEmpty(task.CommandId))
                    {
                        commandIdList.Add(task.CommandId);
                    }
                }
            }
            return commandIdList;
        }

        /// <summary>
        /// Method returns those task which are pending for File Uploading.
        /// </summary>
        /// <returns></returns>
        public static List<TaskBase> GetTasksForFileUploading()
        {
            List<TaskBase> fileUploadingTasks = new List<TaskBase>();
            Dictionary<string, TaskBase> copyTaskList = null;
            lock (LockObj)
            {
                copyTaskList = new Dictionary<string, TaskBase>(taskList);
            }

            foreach (KeyValuePair<string, TaskBase> item in copyTaskList)
            {
                if (item.Value.Status == TaskStatus.DataUploading)
                {
                    item.Value.StatusMessage = string.Format(Constants.msgTaskFileUploading, item.Value.JobName);
                    item.Value.Status = TaskStatus.UploadInProcess;
                    fileUploadingTasks.Add(item.Value);
                }
            }

            return fileUploadingTasks;
        }
    }
}
