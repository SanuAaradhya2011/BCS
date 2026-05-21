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
    /// This class is used to hold the value of Exception Log Table.
    /// </summary>
    public class ExceptionLogEntity : EntityBase
    {
        /// <summary>
        /// Private variable.
        /// </summary>
        private long _logID; 
        private long _logDate; 
        private string _logSource; 
        private string _logMessage; 
        private string _logMacID;
        private int _userInformationID;


        /// <summary>
        /// This property is used to get and set the value of Log ID.
        /// </summary>
        public long LogID
        {
            get
            {
                return _logID;
            }
            set
            {
                _logID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Date.
        /// </summary>
        public long LogDate
        {
            get
            {
                return _logDate;
            }
            set
            {
               _logDate = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Source.
        /// </summary>
        public string LogSource
        {
            get
            {
                return _logSource;
            }
            set
            {
                _logSource = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Message.
        /// </summary>
        public string LogMessage
        {
            get
            {
                return _logMessage;
            }
            set
            {
                _logMessage = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Log Machine ID.
        /// </summary>
        public string LogMacID
        {
            get
            {
                return _logMacID;
            }
            set
            {
                _logMacID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of UserInformation ID.
        /// </summary>
        public int UserInformationID
        {
            get
            {
                return _userInformationID;
            }
            set
            {
                _userInformationID = value;
            }
        }


    }
}