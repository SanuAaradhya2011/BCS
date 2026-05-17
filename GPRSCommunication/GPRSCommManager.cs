using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandisGyr.AMI.Layers;
using LandisGyr.AMI.Network.GPRS.Common;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using GPRSCommunication.DataValidator;
using GPRSCommunication.Communication;
using System.IO;
namespace GPRSCommunication
{
    public class GPRSCommManager
    {
        private MeterCommandType commandType;
        
        private readonly bool isSerivceCallEnabled ;

        public GPRSCommManager( MeterCommandType commandType)
        {
            this.commandType = commandType;
            isSerivceCallEnabled = isServiceCall();
        }

        public GPRSCommManager()
        {
            isSerivceCallEnabled = isServiceCall();
        }

        /// <summary>
        /// Return true if Communication with M2M is service based.
        /// Retruns False if Communication with M2M is database call based.
        /// </summary>
        /// <returns></returns>
        private bool isServiceCall()
        {
            bool retValue = false;

            bool.TryParse(ConfigurationSettings.AppSettings[Constants.GPRSIsServiceCall].ToString(), out retValue);

            return retValue;
        }


        public byte[] SendDataToGPRS(string modemId, byte[] command)
        {
            byte[] result = null;
            RequestOfbase64Binary[] requests = new RequestOfbase64Binary[1];
            RequestOfbase64Binary request = new RequestOfbase64Binary();

            request.MessageID = Guid.NewGuid().ToString();
            DeviceInfo[] devices = new DeviceInfo[1];
            DeviceInfo device = new DeviceInfo();
            device.SerialNumber = modemId;
            devices[0] = device;

            request.Devices = devices;
            request.Command = command;
            SLAParameters slaParam = new SLAParameters();
            slaParam.RetryCount = 2;
            slaParam.TimeToLive = DateTime.Now.AddMinutes(3);
            request.SLA = slaParam;
            
            requests[0] = request;

            bool commandPushed = PushCommandToGPRS(requests);

            /* FOLLOWING PIECE OF CODE CAN BE UNCOMMENTED IF COMMAND REQUEST IS REQUIRED TO CHECK 
             */
            //if (commandPushed)
            //{
            //  RequestStatus status =  CheckCommandDeliveryStatus(request);
            //  if (status == RequestStatus.CONNECTIONFAILED)
            //  {
            //      throw new Exception("Modem is not available.");
            //  }
            //  else
            //  {
            //      result = GetResponseforCommand(request.MessageID);
            //  }
            //}

            result = GetResponseforCommand(request.MessageID);

            //IF send is successful then poll the database.
            return result;
        }


        /// <summary>
        /// Make a service call to M2M Gateway Adapter. 
        /// If no exception is received then Communication is successful.
        /// /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public bool PushCommandToGPRS(RequestOfbase64Binary[] requests)
        {

//#if DEBUG
//            //FileStream stream = File.Open("test.txt", FileMode.Append);
//            //StreamWriter writer = new StreamWriter(stream);
//            //writer.WriteLine(UtilityMethods.ConvertByteArrayToHex(requests[0].Command));
//            //writer.Close();
//            //stream.Close();
            
//            //Console.WriteLine(UtilityMethods.ConvertByteArrayToHex(requests[0].Command));
//#endif
       
            bool retValue = false;
            try
            {
                ServiceFactory.Using<INetworkProtocol>(delegate(INetworkProtocol networkProtocol)
                {
                    networkProtocol.Send(requests);
                }, Constants.GPRSAdapterServiceName);
                retValue = true;
            }
            catch(Exception ex) 
            {
                throw new Exception("Connection to M2M Server is not available.");
            }
            return retValue;
        }

        /// <summary>
        /// Method used to poll the response for the command sent
        /// </summary>
        /// <param name="commandRequestId"></param>
        /// <returns></returns>
        public byte[] GetResponseforCommand(string commandRequestId)
        {
            IResponseValidator responseValidator = ValidatorFactory.GetValidator(commandType);
            DateTime commandTTL = DateTime.Now.AddSeconds(60);
            byte[] responseAsHEx = null;
            int lengthByteRecieved = 0;
            //wait initially for 500 mili second.
            Thread.Sleep(500);
            do
            {
                responseAsHEx = GPRSCommunicationFactory.GetInstance(isSerivceCallEnabled).GetResponse(commandRequestId);

                //If response is complete and valdidated then break from loop.
                if (responseAsHEx != null && responseAsHEx.Length >0 && responseValidator.validateResponse(responseAsHEx))
                {
                    break;
                }
                // if response is not coming then wait for 10 mili second and try again.
                if ((responseAsHEx == null || responseAsHEx.Length ==0))
                {
                    Thread.Sleep(20);
                }
                else
                {
                    //If data is recevied but not complete i.e. response is coming in chuncks. Increase the time out to 60 seconds
                    if (lengthByteRecieved != responseAsHEx.Length)
                    {
                        //If Data is available Then Reset the timer to 0.
                        commandTTL = DateTime.Now.AddSeconds(60);
                        lengthByteRecieved = responseAsHEx.Length;
                    }
                }
                
            } while (commandTTL > DateTime.Now);

            return responseAsHEx;
        }

     /// <summary>
     /// Returns the Response List for passed comma separated command ids.
     /// </summary>
     /// <param name="commandIdList"></param>
     /// <returns></returns>
        public List<Response> GetResponseforCommand(List<string> commandIdList)
        {
            List<Response> responseList = GPRSCommunicationFactory.GetInstance(isSerivceCallEnabled).GetResponse(commandIdList);

            return responseList;
        }

        /// <summary>
        /// Returns the Event Notification from M2M Service
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="eventId"></param>
        /// <param name="maxEventId"></param>
        /// <returns></returns>
        public List<string> GetNotificationEvent(NotificationType notificationType, int eventId, out int maxEventId)
        {
            maxEventId = 0;
            EventCollection eventCollection = null;
            eventCollection = GPRSCommunicationFactory.GetInstance(isSerivceCallEnabled).GetNotificationEvent(notificationType.ToString(), eventId);
            if (eventCollection != null)
            {
                maxEventId = eventCollection.MaxEventId;
                return eventCollection.EventIDList.ToList();
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Returns true if modem is available else returns false
        /// </summary>
        /// <param name="imeiNumber"></param>
        /// <returns></returns>
        public bool IsModemAvailable(string imeiNumber)
        {
            bool isAvailable = false;
            try
            {
                isAvailable = GPRSCommunicationFactory.GetInstance(isSerivceCallEnabled).isModemAvailable(imeiNumber);
            }
            catch 
            {
                isAvailable = false;
            }
            return isAvailable;
        }
    }

    public enum MeterCommandType
    {
        DLMS=0,
        FASTDOWNLOAD,
        IEC,
        OTHER
    }

    public enum NotificationType
    {
        ADDCONNECT,
        TAMPEREVENT,
        DROPCONNECT,

    }
}
