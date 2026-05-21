 /* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 17/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class IECMeterDataEntity : EntityBase
    {

        private long meterData_ID;
        private string meterID;
        private long fileUpload_ID;
        private long readingDateTime;
        private long uploadingDateTime;
        private string cmriID;
        private string cmriType;
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
        public string MeterID
        {
            get
            {
                return meterID;
            }
            set
            {
                meterID = value;
            }
        }
        public long FileUpload_ID
        {
            get
            {
                return fileUpload_ID;
            }
            set
            {
                fileUpload_ID = value;
            }
        }
        public long ReadingDateTime
        {
            get
            {
                return readingDateTime;
            }
            set
            {
                readingDateTime = value;
            }
        }
        public long UploadingDateTime
        {
            get
            {
                return uploadingDateTime;
            }
            set
            {
                uploadingDateTime = value;
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

        /// <summary>
        /// This property is used to get and set the value of CMRI Type
        /// </summary>
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
    }
}