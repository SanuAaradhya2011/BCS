using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class LSIPEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public int LSIPValue
        {
            get;
            set;
        }

        public long MeterDataID
        {
            get;
            set;
        }
        public long LSIPId
        {
            get;
            set;
        }

    }
}
