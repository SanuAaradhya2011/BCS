/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 01/09/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity.Base
{ 
    public class LogInformationEntity : EntityBase
    { 
        private long logID;
        private int userID;
        private long startDateTime;
        private long endDateTime;
  
        public long LogID
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
        public long StartDateTime
        {
            get
            {
                return startDateTime;
            }
            set
            {
                startDateTime = value;
            }        
        }
        public long EndDateTime
        {
            get
            {
                return endDateTime;
            }
            set
            {
                endDateTime = value;
            }
        }
    }
} 