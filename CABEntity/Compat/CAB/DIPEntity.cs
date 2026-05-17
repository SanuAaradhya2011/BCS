#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;
#endregion
namespace LNG.Entity
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

