#region nameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;
#endregion
namespace CABEntity
{
    public class AutoLockEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public string AutoLockStatus
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
