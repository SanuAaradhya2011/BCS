#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;
#endregion
namespace CAB.Entity
{
    public class DIPEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public int DIPValue
        {
            get;
            set;
        }

        public long MeterDataID
        {
            get;
            set;
        }
        public long DIPId
        {
            get;
            set;
        }

    }
}
