/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh.       									|
 * |											Date   : 31/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the entities of meterdata_tampersnapshot Table.
    /// </summary>
    public class TamperSnapshotEntity : EntityBase
    {
        /// Private variable.
        private long tamperSnapShot_ID;
        private long meterData_ID;
        private int tamperCode;
        private string tamperDescription;
        private long tamperOccurredTime;
        private long tamperRestoredTime;
        private string rVoltageOccurred;
        private string yVoltageOccurred;
        private string bVoltageOccurred;
        private string rCurrentOccurred;
        private string yCurrentOccurred;
        private string bCurrentOccurred;
        private string rPFOccurred;
        private string yPFOccurred;
        private string bPFOccurred;
        private string totalPFOccurred;
        private string kWhOccurred;
        private string kVAhOccurred;
        private string rVoltageRestored;
        private string yVoltageRestored;
        private string bVoltageRestored;
        private string rCurrentRestored;
        private string yCurrentRestored;
        private string bCurrentRestored;
        private string rPFRestored;
        private string yPFRestored;
        private string bPFRestored;
        private string totalPFRestored;
        private string kWhRestored;
        private string kVAhRestored;
        private string cmriType;
        private string cmriID;
        /// <summary>
        /// This property is used to get and set the value of Tamper Snapshot ID.
        /// </summary>
        public long TamperSnapShot_ID
        {
            get
            {
                return tamperSnapShot_ID;
            }
            set
            {
                tamperSnapShot_ID = value;
            }
        }
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
        public int TamperCode
        {
            get
            {
                return tamperCode;
            }
            set
            {
                tamperCode = value;
            }
        }
        public string TamperDescription
        {
            get
            {
                return tamperDescription;
            }
            set
            {
                tamperDescription = value;
            }
        }
        public long TamperOccurredTime
        {
            get
            {
                return tamperOccurredTime;
            }
            set
            {
                tamperOccurredTime = value;
            }
        }
        public long TamperRestoredTime
        {
            get
            {
                return tamperRestoredTime;
            }
            set
            {
                tamperRestoredTime = value;
            }
        }
        public string RVoltageOccurred
        {
            get
            {
                return rVoltageOccurred;
            }
            set
            {
                rVoltageOccurred = value;
            }
        }
        public string YVoltageOccurred
        {
            get
            {
                return yVoltageOccurred;
            }
            set
            {
                yVoltageOccurred = value;
            }
        }
        public string BVoltageOccurred
        {
            get
            {
                return bVoltageOccurred;
            }
            set
            {
                bVoltageOccurred = value;
            }
        }
        public string RCurrentOccurred
        {
            get
            {
                return rCurrentOccurred;
            }
            set
            {
                rCurrentOccurred = value;
            }
        }
        public string YCurrentOccurred
        {
            get
            {
                return yCurrentOccurred;
            }
            set
            {
                yCurrentOccurred = value;
            }
        }
        public string BCurrentOccurred
        {
            get
            {
                return bCurrentOccurred;
            }
            set
            {
                bCurrentOccurred = value;
            }
        }
        public string RPFOccurred
        {
            get
            {
                return rPFOccurred;
            }
            set
            {
                rPFOccurred = value;
            }
        }
        public string YPFOccurred
        {
            get
            {
                return yPFOccurred;
            }
            set
            {
                yPFOccurred = value;
            }
        }
        public string BPFOccurred
        {
            get
            {
                return bPFOccurred;
            }
            set
            {
                bPFOccurred = value;
            }
        }
        public string TotalPFOccurred
        {
            get
            {
                return totalPFOccurred;
            }
            set
            {
                totalPFOccurred = value;
            }
        }
        public string KWhOccurred
        {
            get
            {
                return kWhOccurred;
            }
            set
            {
                kWhOccurred = value;
            }
        }
        public string KVAhOccurred
        {
            get
            {
                return kVAhOccurred;
            }
            set
            {
                kVAhOccurred = value;
            }
        }
        public string RVoltageRestored
        {
            get
            {
                return rVoltageRestored;
            }
            set
            {
                rVoltageRestored = value;
            }
        }
        public string YVoltageRestored
        {
            get
            {
                return yVoltageRestored;
            }
            set
            {
                yVoltageRestored = value;
            }
        }
        public string BVoltageRestored
        {
            get
            {
                return bVoltageRestored;
            }
            set
            {
                bVoltageRestored = value;
            }
        }
        public string RCurrentRestored
        {
            get
            {
                return rCurrentRestored;
            }
            set
            {
                rCurrentRestored = value;
            }
        }
        public string YCurrentRestored
        {
            get
            {
                return yCurrentRestored;
            }
            set
            {
                yCurrentRestored = value;
            }
        }
        public string BCurrentRestored
        {
            get
            {
                return bCurrentRestored;
            }
            set
            {
                bCurrentRestored = value;
            }
        }
        public string RPFRestored
        {
            get
            {
                return rPFRestored;
            }
            set
            {
                rPFRestored = value;
            }
        }
        public string YPFRestored
        {
            get
            {
                return yPFRestored;
            }
            set
            {
                yPFRestored = value;
            }
        }
        public string BPFRestored
        {
            get
            {
                return bPFRestored;
            }
            set
            {
                bPFRestored = value;
            }
        }
        public string TotalPFRestored
        {
            get
            {
                return totalPFRestored;
            }
            set
            {
                totalPFRestored = value;
            }
        }
        public string KWhRestored
        {
            get
            {
                return kWhRestored;
            }
            set
            {
                kWhRestored = value;
            }
        }
        public string KVAhRestored
        {
            get
            {
                return kVAhRestored;
            }
            set
            {
                kVAhRestored = value;
            }
        }
        
        public string CMRIID
        {

            get
            {
                return cmriID;
            }
            set
            {
                cmriID = value;
            }
        }
     
        public string CMRIType
        {
            get
            {
                return cmriType;
            }
            set
            {
                cmriType = value;
            }
        }
        public string  RestoreNeutralCurrent { get; set; }
        public string OccuredNeutralCurrent { get; set; }
        public string PhaseVoltageOccured { get; set; }
        public string PhaseCurrentOccured { get; set; }
        public string PhaseVoltageRestore { get; set; }
        public string PhaseCurrentRestore { get; set; }
        public string TempratureOccured { get; set; }
    }
}