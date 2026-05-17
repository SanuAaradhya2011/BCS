using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class OpticalLockUnlockEntity : EntityBase
    {
        public string OPId
        {
            get;
            set;
        }

        public string OPData
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

