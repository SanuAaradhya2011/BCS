using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
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
