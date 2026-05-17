/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon								|
 * | 																												|
 * |											Author : Dhirendra Singh.       									|
 * |											Date   : 29/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.Framework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the entities of meterdata_tamperinformation Table.
    /// </summary>
    public class TamperInformationEntity : EntityBase
    {
        /// Private variable.
        private long tamperInformation_ID;
        private string latestTamperOccurrenceID;
        private long occurrenceTime;
        private string latestTamperRestorationID;
        private long restorationTime;

        /// <summary>
        /// This property is used to get and set the value of Tamper Information ID.
        /// </summary>
        public long TamperInformation_ID
        {
            get
            {
                return tamperInformation_ID;
            }
            set
            {
                tamperInformation_ID = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Latest Tamper Occurrence ID.
        /// </summary>
        public string LatestTamperOccurrenceID
        {
            get
            {
                return latestTamperOccurrenceID;
            }
            set
            {
                latestTamperOccurrenceID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Occurrence Time.
        /// </summary>
        public long OccurrenceTime
        {
            get
            {
                return occurrenceTime;
            }
            set
            {
                occurrenceTime = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Latest Tamper Restoration ID.
        /// </summary>
        public string LatestTamperRestorationID
        {
            get
            {
                return latestTamperRestorationID;
            }
            set
            {
                latestTamperRestorationID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Restoration Time.
        /// </summary>
        public long RestorationTime
        {
            get
            {
                return restorationTime;
            }
            set
            {
                restorationTime = value;
            }
        }
    }
}