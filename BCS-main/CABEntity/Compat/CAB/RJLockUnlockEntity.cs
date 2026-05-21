using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class RJLockUnlockEntity : EntityBase
    {
        public string RJId
        {
            get;
            set;
        }

        public string RJData
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

