using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using System.Threading;
using Hunt.EPIC.Logging;
using GPRSCommunication;
using GPRSComService;

namespace GPRSComService.Worker
{
    class ProcessTaskWorker:WorkerBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GetNewTaskWorker).ToString());
        private List<EventWaitHandle> waitHandlesForStop = new List<EventWaitHandle>();
        private List<NewTaskProcessor> newTaskProcessorList;
        protected override void OnStart(object parameters)
        {
            logger.Log(LOGLEVELS.Info, "ProcessTaskWorker started.");
            ProcessTasks();
        }

        private void ProcessTasks()
        {
            EventWaitHandle waitHandleForStop = null;
            Thread processTaskThread = null;
            NewTaskProcessor taskProcessor;

            int requestPickingThreadsCount = Convert.ToInt16(Constants.GetConfigValue(Constants.constProcessTaskThread));

            newTaskProcessorList = new List<NewTaskProcessor>(requestPickingThreadsCount);

            for (int counter = 0; counter < requestPickingThreadsCount; counter++)
            {
                waitHandleForStop = new EventWaitHandle(false, EventResetMode.ManualReset);

                taskProcessor = new NewTaskProcessor(waitHandleForStop);

                processTaskThread = new Thread(taskProcessor.ProcessNewTask);
                processTaskThread.Name = String.Format("Thread:{0}_{1}", this.GetType().Name, counter + 1);

                base.WorkerThreads.Add(processTaskThread);

                this.waitHandlesForStop.Add(waitHandleForStop);
                this.newTaskProcessorList.Add(taskProcessor);
            }

            for (int counter = 0; counter < requestPickingThreadsCount; counter++)
            {
                Thread workerThread = base.WorkerThreads[counter];
                workerThread.Start();
            }

             waitHandleForStop = new EventWaitHandle(false, EventResetMode.ManualReset);
             NewConnectionProcessor newConnectionProcess = new NewConnectionProcessor(waitHandleForStop);
             Thread newConnectionProcessThread = new Thread(newConnectionProcess.ProcessNewConnection);
             base.WorkerThreads.Add(newConnectionProcessThread);
             this.waitHandlesForStop.Add(waitHandleForStop);
             newConnectionProcessThread.Start();

            //while (true)
            //{
            //    //Start Communication for newly added Tasks
            //    TaskManager.ProcessTask();

            //    //Get New Connection Request
            //    List<string> imeiNumber = GetNewConnectionNotification();

            //    //Find if any task is available for execution and start communication
            //    TaskManager.ProcessTaskForNewConnection(imeiNumber);

            //    //Process Exipred Task.
            //    TaskManager.ProcessExpiredTasks();

            //    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constProcessTaskWorkerIdleTimeout)));
               
            //}
        }

        /// <summary>
        /// Overridden to request PortMessageReader to stop
        /// </summary>
        /// <remarks>This method will block the current executing thread
        /// till all port readers signals back or the thread time out <see cref="ThreadTimeOut"/> expires</remarks>
        protected override void OnStop()
        {
            base.OnStop();

            //1. prepare to stop
            logger.Log(LOGLEVELS.Info, "Process Task Worker thread  Preparing to stop");

            EventWaitHandle[] threadStoppedSignals = new EventWaitHandle[waitHandlesForStop.Count];
            waitHandlesForStop.CopyTo(threadStoppedSignals);

            //2. request all port readers to discontinue reading
            logger.Log(LOGLEVELS.Debug, "Requesting all Process Task Worker to discontinue execution.");

            int commandProcessThreadCount = this.newTaskProcessorList.Count;
            for (int counter = 0; counter < commandProcessThreadCount; counter++)
            {
                this.newTaskProcessorList[counter].ContinueExecution = false;
            }

            //3. wait for signal from the port readers
            logger.Log(LOGLEVELS.Debug, String.Format("waiting for signal from the Process Task Worker [{0}]", newTaskProcessorList));

            try
            {
                WaitHandle.WaitAll(threadStoppedSignals, base.ThreadTimeOut * 1000, false);
            }
            catch (System.Threading.ThreadAbortException threadAbortException)
            {
                //this is not an error, no need to log the exception
                logger.Log(LOGLEVELS.Debug, "Wait-over, threads Aborted", threadAbortException);
            }

        }

        /// <summary>
        /// Overridden to clean up the wait handles
        /// </summary>
        protected override void AfterStop()
        {
            base.AfterStop();

            logger.Log(LOGLEVELS.Debug, "Closing - wait handles");

            int waitHandlesCount = this.waitHandlesForStop.Count;
            for (int counter = 0; counter < waitHandlesCount; counter++)
            {
                this.waitHandlesForStop[counter].Close();
            }

            logger.Log(LOGLEVELS.Debug, "Closed - wait handles");
        }
    }
}
