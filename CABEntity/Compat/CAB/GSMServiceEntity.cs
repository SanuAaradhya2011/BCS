/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class GSMServiceEntity : EntityBase
    {
        private string scheduleName;
        private string schedulePeriod;
        private string weekDayName;
        private string monthDayNumber;
        private string activationDate;
        private string activationTime;
        private string parameters;
        private string meterId;
        private string gSMScheduleID;
        private string gSMGroupScheduleID;
        private string gsmConnectionStatus;
        private int numberofTimeTried;

        public string ScheduleName
        {
            get { return scheduleName; }
            set { scheduleName=value;}
        }
        public string SchedulePeriod
        {
            get { return schedulePeriod; }
            set { schedulePeriod = value; }
        }
        public string WeekDayName
        {
            get { return weekDayName; }
            set { weekDayName = value; }
        }
        public string MonthDayNumber
        {
            get { return monthDayNumber; }
            set { monthDayNumber = value; }
        }
        public string ActivationDate
        {
            get { return activationDate; }
            set { activationDate = value; }
        }
        public string ActivationTime
        {
            get { return activationTime; }
            set { activationTime = value; }
        }
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        public string MeterId
        {
            get { return meterId; }
            set { meterId = value; }
        }
        public string GSMScheduleID
        {
            get { return gSMScheduleID; }
            set { gSMScheduleID = value; }
        }
        public string GSMGroupScheduleID
        {
            get { return gSMGroupScheduleID; }
            set { gSMGroupScheduleID = value; }
        } 
        private string GSMConnectionStatus
        {
            get { return gsmConnectionStatus; }
            set { gsmConnectionStatus = value; }
        }
        private int NumberOfTimeTried
        {
            get { return numberofTimeTried; }
            set { numberofTimeTried = value; }
        }

    }
}

