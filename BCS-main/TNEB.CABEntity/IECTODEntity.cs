#region nameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;
#endregion
namespace CAB.Entity
{
    public class IECTODEntity : EntityBase
    {
        public long MeterData_ID
        {
            get;
            set;
        }

        public string TODId
        {
            get;
            set;
        }

        public string TODData
        {
            get;
            set;
        }

        public long ReadingDateTime
        {
            get;
            set;
        }
        public string MeterDataID
        {
            get;
            set;
        }

    }
}
