using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
{
    public class RTCEntity : EntityBase
    {
        public string RTC { get; set; }

     public long MeterDataID
       {
           get;
           set;
       }
    }
}
