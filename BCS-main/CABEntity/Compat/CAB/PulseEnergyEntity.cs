using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LNG.Framework.Entity;

namespace LNG.Entity
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

