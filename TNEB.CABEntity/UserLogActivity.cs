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
    /// This class is used to hold the value of User Log Activity Table.
    /// </summary>
    public class UserLogActivityEntity : EntityBase
    {
        /// <summary>
        /// Private variable.
        /// </summary>
        private ulong activityID;
        private int userID;
        private long activityDateTime;
        private string activity;
 
        /// <summary>
        /// This property is used to get and set the value of Log ID.
        /// </summary>
        public ulong Activity_ID
        {
            get
            {
                return activityID;
            }
            set
            {
                activityID = value;
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

        /// <summary>
        /// This property is used to get and set the value of Log Date.
        /// </summary>
        public long Activity_DateTime
        {
            get
            {
                return activityDateTime;
            }
            set
            {
                activityDateTime = value;
            }        
        }

        /// <summary>
        /// This property is used to get and set the value of Log Source.
        /// </summary>
        public string Activity
        {
            get
            {
                return activity;
            }
            set
            {
                activity = value;
            }
        }
    }
}
