using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class DailyLogEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }
        public string CumulativeKWh
        {
            get;set;
            
        }
        public string CumulativeKVARhLag
        {
            get;set;
            
        }
         public string CumulativeKVARhLead
        {
            get;set;
           
        }
         public string CumulativeKVAh
         {
             get;
             set;
         }
         public string DailyMD1
         {
             get;
             set;
         }
         public string DailyMD2
         {
             get;
             set;
         }
    }

}    

