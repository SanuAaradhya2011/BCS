/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 30/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using LNG.Framework.Entity;

namespace LNG.Entity
{
    /// <summary>
    /// This class is used to hold the value of meterdata_tampercounter Table.
    /// </summary>
    public class TamperCounterEntity : EntityBase
    {
        /// Private variables.
        private long tamperCounter_ID ;
        private int totalTamperCounter;
        private int powerOnOffCounter;
        private int lowLoadCounter;
        private int overLoadCounter;
        private long tamperCounterGeneral_ID;
        private long meterData_ID;
        
        /// <summary>
        /// This property is used to get and set the value of Tamper Counter ID.
        /// </summary>
        public long MeterData_ID
        {
            get
            {
                return meterData_ID;
            }
            set
            {
                meterData_ID = value;
            }
        }
        public long TamperCounter_ID
        {
            get
            {
                return tamperCounter_ID;
            }
            set
            {
                tamperCounter_ID = value;
            }
        }
        /// <summary>
        /// This property is used to get and set the value of Total Tamper Counter.
        /// </summary>
        public int TotalTamperCounter
        {
            get
            {
                return totalTamperCounter;
            }
            set
            {
                totalTamperCounter = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Power On/Off Counter.
        /// </summary>
        public int PowerOnOffCounter
        {
            get
            {
                return powerOnOffCounter;
            }
            set
            {
                powerOnOffCounter = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Low Load Counter.
        /// </summary>
        public int LowLoadCounter
        {
            get
            {
                return lowLoadCounter;
            }
            set
            {
                lowLoadCounter = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Over Load Counter.
        /// </summary>
        public int OverLoadCounter
        {
            get
            {
                return overLoadCounter;
            }
            set
            {
                overLoadCounter = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of TamperCounterGeneral ID.
        /// </summary>
        public long TamperCounterGeneral_ID
        {
            get
            {
                return tamperCounterGeneral_ID;
            }
            set
            {
                tamperCounterGeneral_ID = value;
            }
        }
    }
}
