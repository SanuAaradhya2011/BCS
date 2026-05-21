/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;
namespace LNG.Entity
{
    public class DLMS650TamperEntity : EntityBase
    { 
        public long Tamper_ID { get; set; }
        public long DateTimeEvent { get; set; }
        public long EventCode { get; set; }
        public string CurrentIR { get; set; }
        public string CurrentIY { get; set; }
        public string CurrentIB { get; set; }
        public string PhaseCurrent { get; set; }
        public string VoltageVRN { get; set; }
        public string VoltageVYN { get; set; }
        public string VoltageVBN { get; set; }
        public string PhaseVoltage { get; set; }
        public string PowerFactorRphase { get; set; }
        public string PowerFactorYphase { get; set; }
        public string PowerFactorBphase { get; set; }
        public string TotalPowerFactor { get; set; }
        public string CumulativeEnergykWh { get; set; }
        public string CumulativeEnergykVAh { get; set; }
        public string CumulativeTampercount { get; set; }
        public string CumulativeEnergykvarhLag { get; set; }
        public string CumulativeEnergykvarhLead { get; set; }
        public long CompartmentNumber { get; set; }
        public long MeterData_ID { get; set; }
        public string NeutralCurrent { get; set; }
        public string ByPassCurrent { get; set; }
        // Net Metering new Parameters
        public string CumulativeEnergykWhImport { get; set; }
        public string CumulativeEnergykVAhImport { get; set; }
        public string CumulativeEnergykWhExport { get; set; }
        public string CumulativeEnergykVAhExport { get; set; }
        //public string kWhAbsolute { get; set; }
        //public string kVAhAbsolute { get; set; }

        // SB Code Change Start 20171116
        public string ActiveCurrentR { get; set; }
        public string ActiveCurrentY { get; set; }
        public string ActiveCurrentB { get; set; }

        public string HighNeutralCurrent { get; set; }//add pradipta_neu

        public string kWr { get; set; } // add pradipta_neu
        public string kWy { get; set; } // add pradipta_neu
        public string kWb { get; set; } // add pradipta_neu

        public string kVAr { get; set; } // add pradipta_neu
        public string kVAy { get; set; } // add pradipta_neu
        public string kVAb { get; set; } // add pradipta_neu
        // SB Code Change End 20171116

        //SarkarA code change start 20180330 // add phase current instant, frequency
        public string Frequency { get; set; } 
        public string PhaseCurrentInstant { get; set; }
        //SarkarA code change end 20180330
        public string Temprature { get; set; }
        public string THDVR { get; set; }
        public string THDVY { get; set; }
        public string THDVB { get; set; }
        public string THDIR { get; set; }
        public string THDIY { get; set; }
        public string THDIB { get; set; }
        
    }
}

