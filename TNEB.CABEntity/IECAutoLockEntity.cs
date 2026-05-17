#region nameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;
#endregion
namespace CABEntity
{
    public class IECAutoLockEntity : EntityBase
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

    }
}
