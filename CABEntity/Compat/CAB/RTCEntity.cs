using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
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

