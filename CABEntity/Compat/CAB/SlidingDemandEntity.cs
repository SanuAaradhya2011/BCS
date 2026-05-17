using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class SlidingDemandEntity : EntityBase
    {
        public string SDId
        {
            get;
            set;
        }

        public string SDData
        {
            get;
            set;
        }
        public long MeterDataID
        {
            get;
            set;
        }
    }
}

