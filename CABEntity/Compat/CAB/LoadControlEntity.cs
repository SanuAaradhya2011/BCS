using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class LoadControlEntity : EntityBase
    {
        public string LCId
        {
            get;
            set;
        }

        public string LCData
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

