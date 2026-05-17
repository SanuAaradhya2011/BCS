using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandisGyr.AMI.Network.GPRS.Common;
using LandisGyr.AMI.Layers;

namespace GPRSCommunication.Communication
{
    class GPRSServiceCommunication:IGPRSCommunication
    {
        #region IGPRSCommunication Members

        /// <summary>
        /// Returns Dataset containing command response for passed list of command ids.
        /// </summary>
        /// <param name="commandIdList"></param>
        /// <returns></returns>
        public byte[] GetResponse(string commandId)
        {
            byte[] response = null;
            try
            {
                    ServiceFactory.Using<IEndpointDetails>(delegate(IEndpointDetails networkProtocol)
                    {
                        response = networkProtocol.GetResponseForCommand(commandId);
                    }, Constants.GPRSAdapterEndPointDetails);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        /// <summary>
        /// Return true if command delivery is successful else returns false
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RequestStatus GetCommandDeliveryStatus(LandisGyr.AMI.Layers.RequestOfbase64Binary request)
        {
            RequestStatus status = RequestStatus.NONE;
            try
            {
                do
                {
                    ServiceFactory.Using<IEndpointDetails>(delegate(IEndpointDetails networkProtocol)
                    {
                        status = networkProtocol.GetCommandStatus(request.MessageID);
                    }, Constants.GPRSAdapterEndPointDetails);

                    if (status != RequestStatus.NONE)
                    {
                        request.SLA.TimeToLive = DateTime.Now.AddSeconds(60);
                        break;
                    }

                } while (request.SLA.TimeToLive > DateTime.Now);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<Response> GetResponse(List<string> commandList)
        {
            List<Response> responseList = new List<Response>();
            try
            {
                ResponseOfbase64Binary[] responses  = null;

                ServiceFactory.Using<IEndpointDetails>(delegate(IEndpointDetails networkProtocol)
                {
                    responses = networkProtocol.GetResponseForMultipleCommands(commandList.ToArray());

                }, Constants.GPRSAdapterEndPointDetails);

                if (responses != null && responses.Length > 0)
                {
                    foreach (ResponseOfbase64Binary responseData in responses)
                    {
                        Response response = new Response();
                        response.CommandId = responseData.CorrelationID;
                        response.Data = responseData.Data[0].Payload;
                        response.IMEINumber = responseData.Data[0].SerialNumber;
                        responseList.Add(response);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseList;
        }

        /// <summary>
        /// Returns Dataset of notifications for passed notification types and eventid
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public EventCollection GetNotificationEvent(string notificationType, int eventId)
        {
            EventCollection eventList = null;
            try
            {
                ServiceFactory.Using<IEndpointDetails>(delegate(IEndpointDetails networkProtocol)
                {
                    eventList = networkProtocol.GetNotificationEvent(notificationType, eventId);

                }, Constants.GPRSAdapterEndPointDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return eventList;
        }

        /// <summary>
        /// Returns true if modem is availabale else returns false
        /// </summary>
        /// <param name="imeiNumber"></param>
        /// <returns></returns>
        public bool isModemAvailable(string imeiNumber)
        {
            bool status = false;
            try
            {
                ServiceFactory.Using<IEndpointDetails>(delegate(IEndpointDetails networkProtocol)
                {
                    status = networkProtocol.isConnectionAvailable(imeiNumber);

                }, Constants.GPRSAdapterEndPointDetails);
            }
            catch (Exception ex)
            { 
                throw ex; 
            }

            return status;
        }


        #endregion
    }
}
