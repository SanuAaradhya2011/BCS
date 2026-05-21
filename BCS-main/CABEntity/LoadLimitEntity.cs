using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
{
    public class LoadLimitEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public int LLValue
        {
            get;
            set;
        }
        public string LLId
        {
            get;
            set;
        }

        public string LLData
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
