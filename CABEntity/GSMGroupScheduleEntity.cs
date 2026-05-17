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
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class GSMGroupScheduleEntity : EntityBase
    {
        private long gsmGroupSchedule_ID;
        public long GSMGroupSchedule_ID
        {
            get { return gsmGroupSchedule_ID; }
            set { gsmGroupSchedule_ID = value; }
        }
        private string group_Name;
        public string Group_Name
        {
            get { return group_Name; }
            set { group_Name = value; }
        }
        private long startReadingDate;
        public long StartReadingDate
        {
            get { return startReadingDate; }
            set { startReadingDate = value; }
        }
        private long gsmSchedule_ID;
        public long GSMSchedule_ID
        {
            get { return gsmSchedule_ID; }
            set { gsmSchedule_ID = value; }
        }
        private string meter_ID;
        public string Meter_ID
        {
            get { return meter_ID; }
            set { meter_ID = value; }
        }
    }
}
