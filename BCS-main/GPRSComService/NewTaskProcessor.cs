using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GPRSComService.Tasks;
using Hunt.EPIC.Logging;
using GPRSCommunication;

namespace GPRSComService
{
    class NewTaskProcessor
    {
        /// <remarks>The consumer of the class instance will set this property when initialising the
        /// instance. Consumer of the instance of this class will set the ContinueReading to false
        /// when there is a need to stop and will wait for this event wait handle to be set. The set state 
        /// indicate that the Port Message Reader has read and processed the current message successfully and 
        /// have no work in hand</remarks>
        private EventWaitHandle signalReadyToStop;
        private static object LockObj = new object();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(NewTaskProcessor).ToString());
        private bool continueExecution;
        private ReaderWriterLock flagLock; 

        public NewTaskProcessor(EventWaitHandle signalReadyToStop)
        {
            this.signalReadyToStop = signalReadyToStop;
            this.flagLock = new ReaderWriterLock();
            continueExecution = true;
        }

        public bool ContinueExecution
        {
            set
            {
                flagLock.AcquireWriterLock(10);
                continueExecution = value;
                flagLock.ReleaseWriterLock();
            }
        }
        public void ProcessNewTask()
        {
           GPRSCommManager objComMgr = new GPRSCommManager();

            while (true)
            {
                TaskBase task = TaskManager.GetNextTask;
                if (task != null)
                {
                    if (task.Status == TaskStatus.None)
                    {
                        //Check if modem is availablr for task. If yes then start communication on modem
                        if (objComMgr.IsModemAvailable(task.IMEINumber))
                        {
                            task.Status = TaskStatus.StartComm;
                            task.StartCommunication();
                        }
                        else
                        {
                            //Try to add modem in modem not available list.
                            if (!TaskManager.AddTaskToModemWaitQueue(task))
                            {
                                //If add to modem not available list fails then add the task again to task queue.
                                TaskManager.AddTaskToCommandProcessingQueue(task);
                            }

                            logger.Log(LOGLEVELS.Info, string.Format("Modem not available for Task id:{0}, TaskName: {1}, modemIMEI: {2} ", task.TaskId.ToString(), task.TaskName, task.IMEINumber));
                        }
                    }
                }
                else
                {
                    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constNewTaskWorkerIdleTimeout)));

                }

                flagLock.AcquireReaderLock(100);
                if (!continueExecution)
                {
                    signalReadyToStop.Set();
                }
                flagLock.ReleaseReaderLock();
            }
        }
    }
}
