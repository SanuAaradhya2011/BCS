/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 18/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity.Base
{
    /// <summary>
    /// This class is used to hold the value of Application Log Table.
    /// </summary>
    public class ApplicationLogEntity : EntityBase
    {
        /// <summary>
        /// Private variable.
        /// </summary>
        private ulong logID; 
        private long logDate; 
        private string logSource; 
        private string logMessage; 
        private string logMacID; 
        private int userID; 


        /// <summary>
        /// This property is used to get and set the value of Log ID.
        /// </summary>
        public ulong LogID
        {
            get
            {
                return logID;
            }
            set
            {
                logID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Date.
        /// </summary>
        public long LogDate
        {
            get
            {
                return logDate;
            }
            set
            {
               logDate = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Source.
        /// </summary>
        public string LogSource
        {
            get
            {
                return logSource;
            }
            set
            {
                logSource = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Message.
        /// </summary>
        public string LogMessage
        {
            get
            {
                return logMessage;
            }
            set
            {
                logMessage = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Machine ID.
        /// </summary>
        public string LogMacID
        {
            get
            {
                return logMacID;
            }
            set
            {
                logMacID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of User ID.
        /// </summary>
        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }
 
    }
}









