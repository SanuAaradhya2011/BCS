using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class MDWithIPEntity : EntityBase
    {
        private string meterID;
        private string kwDemendType;
        private int kwInterval;
        private int kwSubInterval;
        private string kvaDemendType;
        private int kvaInterval;
        private int kvaSubInterval;


        public string MeterID
        {
            get { return meterID; }
            set { meterID = value; }
        }
        public string KWDemandType
        {
            get { return kwDemendType; }
            set { kwDemendType = value; }
        }

        public int KWInterval
        {
            get { return kwInterval; }
            set { kwInterval = value; }
        }

        public int KWSubInterval
        {
            get { return kwSubInterval; }
            set { kwSubInterval = value; }
        }

        public string KVADemandType
        {
            get { return kvaDemendType; }
            set { kvaDemendType = value; }
        }

        public int KVAInterval
        {
            get { return kvaInterval; }
            set { kvaInterval = value; }
        }

        public int KVASubInterval
        {
            get { return kvaSubInterval; }
            set { kvaSubInterval = value; }
        }
            
    }
}
