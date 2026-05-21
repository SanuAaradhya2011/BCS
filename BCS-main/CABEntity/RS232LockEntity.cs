using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework.Entity;

namespace CABEntity
{
   public class RS232LockEntity : EntityBase
    {
       public string MeterID
       {
           get;
           set;
       }
      
       public string LockStatus 
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
