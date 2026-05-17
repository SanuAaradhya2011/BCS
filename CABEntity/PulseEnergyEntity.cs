using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class PulseEnergyEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public string PulseEnergyValue
        {
            get;
            set;
        }

        public long MeterDataID
        {
            get;
            set;
        }
        public long PulseEnergyId
        {
            get;
            set;
        }

    }
}
