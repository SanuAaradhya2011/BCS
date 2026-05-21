using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
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
