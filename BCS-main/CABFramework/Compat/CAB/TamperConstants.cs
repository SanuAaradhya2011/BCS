using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNG.Framework
{
   public class TamperConstants
    {
       public const string DATETIME= "DateTimeEvent";
        public const string CURRENTIR = "currentir";
        public const string CURRENTIY = "currentiy";
        public const string CURRENTIB = "currentib";
        public const string VOLTAGEVRN = "voltagevrn";
        public const string VOLTAGEVYN = "voltagevyn";
        public const string VOLTAGEVBN = "voltagevbn";
        public const string PFRPHASE = "powerfactorrphase";
        public const string PFYPHASE = "powerfactoryphase";
        public const string PFBPHASE = "powerfactorbphase";
        public const string CUMULATIVEENERGYKWH = "cumulativeenergykwh";
        public const string CUMULATIVEENERGYKVAH = "cumulativeenergykvah";
        public const string NeutralCurrent = "neutralcurrent"; // Story - 365865 - Neutral current for single phase non DLMS meter integration
        public const string ByPassCurrent = "bypasscurrent"; 
        public const string HighNeutralCurrent = "highneutralcurrent";

        public const string kWr = "kwr"; 
        public const string kWy = "kwy"; 
        public const string kWb = "kwb"; 

        public const string kVAr = "kvar";
        public const string kVAy = "kvay";
        public const string kVAb = "kvab"; 


    }
}

