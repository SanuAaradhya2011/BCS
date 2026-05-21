using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using GPRSCommunication;
using System.Threading;
using GPRSComService.Tasks;
using Hunt.EPIC.Logging;

namespace GPRSComService.Worker
{
    class ResponseProcessorWorker : WorkerBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Worker.ResponseProcessorWorker).ToString());
        private List<ResponseProcessor> newResponseProcessorList;
        private List<EventWaitHandle> waitHandlesForStop = new List<EventWaitHandle>();

        protected override void OnStart(object parameters)
        {
            logger.Log(LOGLEVELS.Info, string.Format("ResponseProcessorWorker started."));
            ProcessResponse(); 
        }


        /// <summary>
        /// Process the responser received from the M2M
        /// </summary>
        private void ProcessResponse()
        {
            EventWaitHandle waitHandleForStop = null;
            Thread processTaskThread = null;
            ResponseProcessor responseProcessor;

            int requestPickingThreadsCount = Convert.ToInt16(Constants.GetConfigValue(Constants.constResponseProcessorThread));

            newResponseProcessorList = new List<ResponseProcessor>(requestPickingThreadsCount);

            for (int counter = 0; counter < requestPickingThreadsCount; counter++)
            {
                waitHandleForStop = new EventWaitHandle(false, EventResetMode.ManualReset);

                responseProcessor = new ResponseProcessor(waitHandleForStop);

                processTaskThread = new Thread(responseProcessor.ProcessResponse);
                processTaskThread.Name = String.Format("Thread:{0}_{1}", this.GetType().Name, counter + 1);

                base.WorkerThreads.Add(processTaskThread);

                this.waitHandlesForStop.Add(waitHandleForStop);
                this.newResponseProcessorList.Add(responseProcessor);
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
            logger.Log(LOGLEVELS.Info, "Response Process Worker thread  Preparing to stop");

            EventWaitHandle[] threadStoppedSignals = new EventWaitHandle[waitHandlesForStop.Count];
            waitHandlesForStop.CopyTo(threadStoppedSignals);

            //2. request all port readers to discontinue reading
            logger.Log(LOGLEVELS.Debug, "Requesting all Response Process task Worker to discontinue execution.");

            int commandProcessThreadCount = this.newResponseProcessorList.Count;
            for (int counter = 0; counter < commandProcessThreadCount; counter++)
            {
                this.newResponseProcessorList[counter].ContinueExecution = false;
            }

            //3. wait for signal from the port readers
            logger.Log(LOGLEVELS.Debug, String.Format("waiting for signal from the Response Process Worker [{0}]", commandProcessThreadCount));

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
