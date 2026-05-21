using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandisGyr.AMI.Network.GPRS.Common;
using LandisGyr.AMI.Layers;
using System.Data;

namespace GPRSCommunication.Communication
{
    interface IGPRSCommunication
    {
        /// <summary>
        /// Returns response bytes for passed commandId
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        byte[] GetResponse(string commandId);
        
        /// <summary>
        /// Returns Dataset containing command response for passed list of command ids.
        /// </summary>
        /// <param name="commandIdList"></param>
        /// <returns></returns>
        List<Response> GetResponse(List<string> commandIdList);

        /// <summary>
        /// Return true if command delivery is successful else returns false
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RequestStatus GetCommandDeliveryStatus(RequestOfbase64Binary request);

        /// <summary>
        /// Returns Dataset of notifications for passed notification types and eventid
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        EventCollection GetNotificationEvent(string notificationType, int eventId);

        /// <summary>
        /// Returns true if modem is availabale else returns false
        /// </summary>
        /// <param name="imeiNumber"></param>
        /// <returns></returns>
        bool isModemAvailable(string imeiNumber);
    }
}
