using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
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
