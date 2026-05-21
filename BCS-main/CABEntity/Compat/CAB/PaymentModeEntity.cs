using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class PaymentModeEntity : EntityBase
    {
        public string PMId
        {
            get;
            set;
        }

        public string PMData
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

