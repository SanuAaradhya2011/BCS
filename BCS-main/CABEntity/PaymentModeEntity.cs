using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
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
