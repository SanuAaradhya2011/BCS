using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using LandisGyr.AMI.Network.GPRS.Common;

namespace GPRSCommunication.Communication
{
    class DBCommunication:IGPRSCommunication
    {
        #region IGPRSCommunication Members

        /// <summary>
        /// Returns the response string from DB Poll
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public byte[] GetResponse(string commandId)
        {
           // IDataHelper 
            byte[] responseBytes = null;
            string response = string.Empty;
            using (SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings[Constants.GPRSConnectionKey].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetEndPointResponse", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("CommandRequestId", commandId));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    response = ds.Tables[0].Rows[0][0].ToString();
                }
                conn.Close();
            }

            if (!string.IsNullOrEmpty(response))
            {
                responseBytes = UtilityMethods.ConvertHexToByteArray(response);
            }
            return responseBytes;
        }

        /// <summary>
        /// Returns the command delivery status 
        /// If command is successfully delivered then it's corresponding entry will be available in commandRequest table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        RequestStatus IGPRSCommunication.GetCommandDeliveryStatus(LandisGyr.AMI.Layers.RequestOfbase64Binary request)
        {
            return RequestStatus.SUBMITTED;
        }

        /// <summary>
        /// Returns response for multiple command sent from db poll
        /// </summary>
        /// <param name="commandIdList"></param>
        /// <returns></returns>
        public List<Response> GetResponse(List<string> commandIdList)
        {
            List<Response> responseList = new List<Response>();
            string commandIds = string.Empty;
            if (commandIdList.Count > 0)
            {
                commandIds = string.Join("," , commandIdList.ToArray());
            }
            
            DataSet ds = null;
            if (!string.IsNullOrEmpty(commandIds))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings[Constants.GPRSConnectionKey].ToString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetResponseByCmdId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("CommandRequestId", commandIds));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds);
                }
            }

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Response response = new Response();
                    response.IMEINumber = row["EndPointId"].ToString();
                    response.Data = UtilityMethods.ConvertHexToByteArray(row["ResponseBytes"].ToString());
                    response.CommandId = row["CommandRequestId"].ToString();
                    responseList.Add(response);
                }
            }
            return responseList;
        }

        /// <summary>
        /// Returns Dataset for passed notification type 
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public EventCollection GetNotificationEvent(string notificationType, int eventId)
        {
            EventCollection eventCollection = null;
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings[Constants.GPRSConnectionKey].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetNewConnectionNotification", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("EventId", eventId ));
                cmd.Parameters.Add(new SqlParameter("NotificationType", notificationType.ToString()));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);
            }

            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                eventCollection = new EventCollection();
                List<string> eventIdList = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    eventIdList.Add(dr["EndPointId"].ToString());
                    eventCollection.MaxEventId = Convert.ToInt32(dr["MaxEventId"].ToString());
                }
                eventCollection.EventIDList = eventIdList.ToArray();
            }
            return eventCollection;
        }

        /// <summary>
        /// Return true if the modem is available for passed imei number.
        /// </summary>
        /// <param name="imeiNumber"></param>
        /// <returns></returns>
        public bool isModemAvailable(string imeiNumber)
        {
            bool isAvailable = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationSettings.AppSettings[Constants.GPRSRoutingServiceConnectionKey].ToString()))
            {
                conn.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("GetEndPointConnectionInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("ModemSerialNumber", imeiNumber));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    isAvailable = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUsable"].ToString());
                }
            }
            return isAvailable;
        }

        #endregion
    }

    
}
