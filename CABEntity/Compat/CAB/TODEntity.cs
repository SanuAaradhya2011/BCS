#region nameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;
#endregion
namespace LNG.Entity
{
    public class TODEntity : EntityBase
    {
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
        public long MeterDataID
        {
            get;
            set;
        }

    }
}

