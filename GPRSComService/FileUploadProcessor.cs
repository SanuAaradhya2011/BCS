using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hunt.EPIC.Logging;
using GPRSComService.Tasks;

namespace GPRSComService
{
    class FileUploadProcessor
    {
        private EventWaitHandle signalReadyToStop;
        private static object LockObj = new object();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FileUploadProcessor).ToString());
        private bool continueExecution;
        private ReaderWriterLock flagLock;

        public FileUploadProcessor(EventWaitHandle signalReadyToStop)
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

        /// <summary>
        /// Picks the task from fileUploading queue and  uploads to database
        /// </summary>
        public void ProcessFileUpload()
        {
            while (true)
            {
                TaskBase task = TaskManager.GetNextFileUploadTask();
                if (task != null)
                {
                    logger.Log(LOGLEVELS.Debug, string.Format("File uploading started for IMEI:{0}, Meter: {1}, Job Name: {2}", task.IMEINumber, task.MeterId, task.JobName));

                    task.WriteToFile();
                    
                    logger.Log(LOGLEVELS.Debug, string.Format("File uploading completed for IMEI:{0}, Meter: {1}, Job Name: {2}", task.IMEINumber, task.MeterId, task.JobName));
                }
                else
                {
                    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constFileuploaderTimeOut)));
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
