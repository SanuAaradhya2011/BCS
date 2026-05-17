using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using LandisGyr.AMI.Layers;
using GPRSCommunication;
using System.Threading;
using Hunt.EPIC.Logging;

namespace GPRSComService.Worker
{
    class CommandProcessorWorker : WorkerBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GetNewTaskWorker).ToString());
        private List<CommandProcessor> commandProcessorList;
        private List<EventWaitHandle> waitHandlesForStop = new List<EventWaitHandle>();


        protected override void OnStart(object parameters)
        {
            logger.Log(LOGLEVELS.Info, "CommandProcessorWorker thread started.");
            ExecutingCommandProcessingThread();
        }

        /// <summary>
        /// Process the commands and push the commands to M2M Server.
        /// </summary>
        private void ExecutingCommandProcessingThread()
        {
            EventWaitHandle waitHandleForStop = null;
            Thread processTaskThread = null;
            CommandProcessor commandProcessor;

            int requestPickingThreadsCount = Convert.ToInt16(Constants.GetConfigValue(Constants.constCommandProcessorThread));

            commandProcessorList = new List<CommandProcessor>(requestPickingThreadsCount);

            for (int counter = 0; counter < requestPickingThreadsCount; counter++)
            {
                waitHandleForStop = new EventWaitHandle(false, EventResetMode.ManualReset);

                commandProcessor = new CommandProcessor(waitHandleForStop);

                processTaskThread = new Thread(commandProcessor.ProcessCommand);
                processTaskThread.Name = String.Format("Thread:{0}_{1}", this.GetType().Name, counter + 1);

                base.WorkerThreads.Add(processTaskThread);

                this.waitHandlesForStop.Add(waitHandleForStop);
                this.commandProcessorList.Add(commandProcessor);
            }

            for (int counter = 0; counter < requestPickingThreadsCount; counter++)
            {
                Thread workerThread = base.WorkerThreads[counter];
                workerThread.Start();
            }
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
            logger.Log(LOGLEVELS.Info,"CommandProcessWorker thread  Preparing to stop");

            EventWaitHandle[] threadStoppedSignals = new EventWaitHandle[waitHandlesForStop.Count];
            waitHandlesForStop.CopyTo(threadStoppedSignals);

            //2. request all port readers to discontinue reading
            logger.Log(LOGLEVELS.Debug,"Requesting all Command Reader process thread to discontinue execution.");

            int commandProcessThreadCount = this.commandProcessorList.Count;
            for (int counter = 0; counter < commandProcessThreadCount; counter++)
            {
                this.commandProcessorList[counter].ContinueExecution = false;
            }

            //3. wait for signal from the port readers
            logger.Log(LOGLEVELS.Debug,String.Format("waiting for signal from the Message Reader port readers [{0}]", commandProcessThreadCount));

            try
            {
                WaitHandle.WaitAll(threadStoppedSignals, base.ThreadTimeOut * 1000, false);
            }
            catch (System.Threading.ThreadAbortException threadAbortException)
            {
                //this is not an error, no need to log the exception
                logger.Log(LOGLEVELS.Debug, "Wait-over, threads Aborted",threadAbortException);
            }

        }

        /// <summary>
        /// Overridden to clean up the wait handles
        /// </summary>
        protected override void AfterStop()
        {
            base.AfterStop();

            logger.Log(LOGLEVELS.Debug,"Closing - wait handles");

            int waitHandlesCount = this.waitHandlesForStop.Count;
            for (int counter = 0; counter < waitHandlesCount; counter++)
            {
                this.waitHandlesForStop[counter].Close();
            }

            logger.Log(LOGLEVELS.Debug, "Closed - wait handles");
        }
    }
}
