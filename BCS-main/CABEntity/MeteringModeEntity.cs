using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
{
    public class MeteringModeEntity : EntityBase
    {
        public string MMId
        {
            get;
            set;
        }

        public string MMData
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
