using CAB.IECFramework.Entity;

namespace CAB.Entity
{
     public class MeterDataHeaderInfoEntity : EntityBase 
    {
        private long headerInfo_ID;
        private long meterData_ID;
        private string md1KWDemandType="";
        private string md1KWTimeInterval = "";
        private string md1KWSubInterval = "";
        private string md2KVADemandType = "";
        private string md2KVATimeInterval = "";
        private string md2KVASubInterval = "";
        private string pfLogic = "";
        private string powerOffDays = "";
        private string meterConstant = "";
        private string internalCTPTRatio = "";
        private string softwareVersion = "";
        private string billingType="";
        private string billingDate="";
        private string billingHour = "";
        private string billingMinute="";
        private string noLoadDuration = "";
        private string noSupplyDuration = "";
        public string meterId = null;
        public long fileUploadID;
        public long uploadDateTime;
        public long readingDateTime;

        public long HeaderInfo_ID
        {
            get
            {
                return headerInfo_ID;
            }
            set
            {
                headerInfo_ID = value;
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
        public string MD1KWDemandType
        {
            get
            {
                return md1KWDemandType;
            }
            set
            {
                md1KWDemandType = value;
            }
        }

        public string MD1KWTimeInterval
        {
            get
            {
                return md1KWTimeInterval;
            }
            set
            {
                md1KWTimeInterval = value;
            }
        }

        public string MD1KWSubInterval
        {
            get
            {
                return md1KWSubInterval;
            }
            set
            {
                md1KWSubInterval = value;
            }
        }

        public string MD2KVADemandType
        {
            get
            {
                return md2KVADemandType;
            }
            set
            {
                md2KVADemandType = value;
            }
        }

        public string MD2KVATimeInterval
        {
            get
            {
                return md2KVATimeInterval;
            }
            set
            {
                md2KVATimeInterval = value;
            }
        }

        public string MD2KVASubInterval
        {
            get
            {
                return md2KVASubInterval;
            }
            set
            {
                md2KVASubInterval = value; 
            }
        }

        public string PFLogic
        {
            get
            {
                return pfLogic;
            }
            set
            {
                pfLogic = value;
            }
        }

        public string PowerOffDays
        {
            get
            {
                return powerOffDays;
            }
            set
            {
                powerOffDays = value;
            }
        }
        public string NoLoadDuration
        {
            get
            {
                return noLoadDuration;
            }
            set
            {
                noLoadDuration = value;
            }
        }

        public string NoSupplyDuration
        {
            get
            {
                return noSupplyDuration;
            }
            set
            {
                noSupplyDuration = value;
            }
        }

        public string MeterConstant
        {
            get
            {
                return meterConstant;
            }
            set
            {
                meterConstant = value;
            }
        }

        public string InternalCTPTRatio
        {
            get
            {
                return internalCTPTRatio;
            }
            set
            {
                internalCTPTRatio = value;
            }
        }

        public string SoftwareVersion
        {
            get
            {
                return softwareVersion;
            }
            set
            {
                softwareVersion = value;
            }
        }

        public string BillingType
        {
            get
            {
                return billingType;
            }
            set
            {
                billingType = value;
            }
        }

        public string BillingDate
        {
            get
            {
                return billingDate;
            }
            set
            {
                billingDate = value;
            }
        }

        public string BillingHour
        {
            get
            {
                return billingHour;
            }
            set
            {
                billingHour = value;
            }
        }

        public string BillingMinute
        {
            get
            {
                return billingMinute;
            }
            set
            {
                billingMinute = value;
            }
        }
    }
}
