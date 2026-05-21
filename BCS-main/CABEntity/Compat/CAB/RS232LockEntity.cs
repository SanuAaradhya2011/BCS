using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
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

