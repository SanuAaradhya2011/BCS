using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class DisconnectControlEntity : EntityBase
    {
        public string DCId
        {
            get;
            set;
        }

        public string DCData
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

