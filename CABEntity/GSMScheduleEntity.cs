/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 11/06/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class GSMScheduleEntity : EntityBase
    {
        private long gsmSchedule_ID;
        private string schedule_Name;
        private string schedule_Period;
        private string period_DayName;
        private int period_DayNumber;
        private long scheduleCreationDate;
        private long scheduleActivationDate;
        private string scheduleActivationTime; 
        private string schedule_Parameter; 
        private int status; 

        public long GSMSchedule_ID
        {
            get
            {
                return gsmSchedule_ID;
            }
            set
            {
                gsmSchedule_ID = value;
            }
        }
        public string Schedule_Name
        {
            get
            {
                return schedule_Name;
            }
            set
            {
                schedule_Name = value;
            }
        }
        public string Schedule_Period
        {
            get
            {
                return schedule_Period;
            }
            set
            {
                schedule_Period = value;
            }
        }
        public string Period_DayName
        {
            get
            {
                return period_DayName;
            }
            set
            {
                period_DayName = value;
            }
        }
        public int Period_DayNumber
        {
            get
            {
                return period_DayNumber;
            }
            set
            {
                period_DayNumber = value;
            }
        }
        public long ScheduleCreationDate
        {
            get
            {
                return scheduleCreationDate;
            }
            set
            {
                scheduleCreationDate = value;
            }
        }
        public string ScheduleActivationTime
        {
            get
            {
                return scheduleActivationTime;
            }
            set
            {
                scheduleActivationTime = value;
            }
        }
        public long ScheduleActivationDate
        {
            get
            {
                return scheduleActivationDate;
            }
            set
            {
                scheduleActivationDate = value;
            }
        }
        public string Schedule_Parameter
        {
            get
            {
                return schedule_Parameter;
            }
            set
            {
                schedule_Parameter = value;
            }
        } 
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
    }
}
