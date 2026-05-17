using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Hunt.EPIC.Logging;
using GPRSCommunication;
using GPRSComService.Tasks;

namespace GPRSComService
{
    class ResponseProcessor
    {

        private EventWaitHandle signalReadyToStop;
        private static object LockObj = new object();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ResponseProcessor).ToString());
        GPRSCommManager comMgr = new GPRSCommManager();
        private bool continueExecution;
        private ReaderWriterLock flagLock; 

        public ResponseProcessor(EventWaitHandle signalReadyToStop)
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

        public void ProcessResponse()
        {
            while (true)
            {

                List<Response> responseList =  GetResponse();

                //If response is available else sleep the thread for configured time
                if (responseList != null && responseList.Count > 0)
                {
                    logger.Log(LOGLEVELS.Info, "Response pick started.");

                    PushResponseToTask(responseList);
                }
                else
                {
                    Thread.Sleep(Convert.ToInt16(Constants.GetConfigValue(Constants.constResponseProcessorIdleTimeout)));
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
        /// Get the response from M2M for comma separated command ids.
        /// </summary>
        /// <returns></returns>
        private List<Response> GetResponse()
        {
            List<Response> responseList = new List<Response>();
            try
            {
                List<string> tempList = RequestCommandQueue.GetCommandId();
                
                if (tempList != null && tempList.Count > 0)
                {
                    responseList = comMgr.GetResponseforCommand(tempList);
                    
                    //Remove the commands whose response is receivied in database poll
                    foreach (Response response in responseList)
                    {
                        tempList.Remove(response.CommandId);
                    }

                    //We are left with commands whose response is not receivied from database poll
                    //this commands has to be sent again for database poll
                    foreach (string commandId in tempList)
                    {
                        //Remove commands from old list and add it to new command list. so that it can be picked again by the thread
                        //RequestCommandQueue.RemoveCommandIdFromOld(commandId);
                        RequestCommandQueue.AddCommandIdToNewCommandList(commandId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while fetching response from database. Details:", ex);

            }
            return responseList;
        }

        /// <summary>
        /// Push the response to the corresponding Task
        /// </summary>
        /// <param name="responseList"></param>
        private void PushResponseToTask(List<Response> responseList)
        {
            foreach (Response response in responseList)
            {
                //Get the Task for response based on imei number. 
                TaskBase task = TaskManager.GetTask(response.IMEINumber);

                //If task is available then push it.
                if (task != null)
                {
                    logger.Log(LOGLEVELS.Info, string.Format("Response Pushed for MeterId:{0}, ImeiNumber: {1}", response.CommandId, response.IMEINumber));
                    task.PushResponse(response.Data);
                }
            }
        }

    }
}
