
using System;
using System.Collections.Generic;
using System.Text;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class DLMS650MidnightDataEntity : EntityBase
    {
        private string netkWh;
        private string netkVAh;

        public long MidnightData_ID
        { get; set; }

        public long RealTimeClockDateandTime
        { get; set; }

        public string CumEnergykWh
        { get; set; }

        public string CumEnergykvarhlag
        { get; set; }

        public string CumEnergykvarhlead
        { get; set; }

        public string CumEnergykVAh
        { get; set; }

        public string MDKW
        { get; set; }

        public long MDKWDateTime
        { get; set; }

        public string MDKVA
        { get; set; }

        public long MDKVADateTime
        { get; set; }

        public long MeterData_ID
        { get; set; }

        public string PowerOnDuration 
        { get; set; }

        public string PowerFailureDuration
        { get; set; }

        public string PowerOnDurationThreePhases
        { get; set; }

        public string PowerOnDurationGeneric // 
        { get; set; }

        public string PowerOnDurationGeneric1P // PowerOnDurationGeneric1P
        { get; set; }

        public string CumEnergykWhExport
        { get; set; }
             
        public string CumEnergykVAhExport
        { get; set; }

        public string CumEnergykWhImport
        { get; set; }

        public string CumEnergykVAhImport
        { get; set; }

        public string CumEnergykvarhlagQ1
        { get; set; }

        public string CumEnergykvarhlagQ3
        { get; set; }

        public string CumEnergykvarhleadQ2
        { get; set; }

        public string CumEnergykvarhleadQ4
        { get; set; }

        public string CumEnergykWhRPhase
        { get; set; }

        public string CumEnergykWhYPhase
        { get; set; }

        public string CumEnergykWhBPhase
        { get; set; }

        public string CumEnergykvarhQ12
        { get; set; }

        public string CumEnergykvarhQ34
        { get; set; }

        public string CumEnergykvarhQ14
        { get; set; }

        public string CumEnergykvarhQ23
        { get; set; }

        public string FundamentalAbsolutekWH
        { get; set; }

        public string NetkWh
        {
            get { return netkWh; }
            set { netkWh = value; }
        }

        public string NetkVAh
        {
            get { return netkVAh; }
            set { netkVAh = value; }
        }
        
        public string MinVoltageLSIPAcrossDayRPhase 
        { get; set; }
        public string MinVoltageLSIPAcrossDayYPhase
        { get; set; }
        public string MinVoltageLSIPAcrossDayBPhase
        { get; set; }
        public string HighestCurrentLSIPAcrossDayRPhase 
        { get; set; }
        public string HighestCurrentLSIPAcrossDayYPhase
        { get; set; }
        public string HighestCurrentLSIPAcrossDayBPhase
        { get; set; }


    }
}
