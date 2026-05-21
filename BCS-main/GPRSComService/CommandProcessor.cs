using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hunt.EPIC.Logging;
using LandisGyr.AMI.Layers;
using GPRSCommunication;

namespace GPRSComService
{
    class CommandProcessor
    {

        private EventWaitHandle signalReadyToStop;
        private static object LockObj = new object();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CommandProcessor).ToString());
        private bool continueExecution;
        private ReaderWriterLock flagLock;

        public CommandProcessor(EventWaitHandle signalReadyToStop)
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

        public void ProcessCommand()
        {
            while (true)
            {
                List<RequestOfbase64Binary> commands = RequestCommandQueue.GetCommandToPush();
               
                if (commands.Count > 0)
                {
                    PushCommandToM2M(commands);
                }
                else
                {
                    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constCommandProcessorTimeOut)));
                    System.Diagnostics.Trace.WriteLine("Thread sleep called from Command Processor");

                }

                flagLock.AcquireReaderLock(100);
                if (!continueExecution)
                {
                    signalReadyToStop.Set();
                }
                flagLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Method Push the data to M2M by making service call.
        /// </summary>
        /// <param name="commands"></param>
        private void PushCommandToM2M(List<RequestOfbase64Binary> commands)
        {
            try
            {
                GPRSCommManager comMgr = new GPRSCommManager(MeterCommandType.DLMS);
                comMgr.PushCommandToGPRS(commands.ToArray());
                logger.Log(LOGLEVELS.Info, "PushCommandToM2M method execution completed.");
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while call PushCommandToM2M method", ex);
            }
        }
    }
}
