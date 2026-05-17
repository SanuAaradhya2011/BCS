using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;

namespace CABEntity
{
   public class IECRS232LockEntity : EntityBase
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

    }
}
