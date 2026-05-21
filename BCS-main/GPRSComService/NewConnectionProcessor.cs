using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hunt.EPIC.Logging;
using GPRSCommunication;

namespace GPRSComService
{
    class NewConnectionProcessor
    {

        private EventWaitHandle signalReadyToStop;
        private static object LockObj = new object();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(NewConnectionProcessor).ToString());
        private static int addConnectEventId = 0;
        private bool continueExecution;
        private ReaderWriterLock flagLock;

        public NewConnectionProcessor(EventWaitHandle signalReadyToStop)
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

        public void ProcessNewConnection()
        {
            while (true)
            {
                if (TaskManager.HasTaskInModemWaitQueue)
                {
                    //Get New Connection Request
                    List<string> imeiNumber = GetNewConnectionNotification();

                    //Find if any task is available for execution and start communication
                    TaskManager.ProcessTaskForNewConnection(imeiNumber);
                }

                //Process Exipred Task.
                TaskManager.ProcessExpiredTasks();


                Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constProcessTaskWorkerIdleTimeout)));
                
                flagLock.AcquireReaderLock(100);
                if (!continueExecution)
                {
                    signalReadyToStop.Set();
                }
                flagLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Gets the New connection add notification from M2M Database
        /// </summary>
        /// <returns></returns>
        private List<string> GetNewConnectionNotification()
        {
            List<string> newConnection = new List<string>();
            try
            {
                GPRSCommManager objCommMgr = new GPRSCommManager();
                int tempEventId = 0;
                newConnection = objCommMgr.GetNotificationEvent(NotificationType.ADDCONNECT, addConnectEventId, out tempEventId);

                if (addConnectEventId != tempEventId && tempEventId != 0)
                {
                    addConnectEventId = tempEventId;
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while getting new connection notifications", ex);
            }
            return newConnection;
        }
    }
}
