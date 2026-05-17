/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class MasterDataEntity : EntityBase
    {
        public IECMeterDataEntity MeterData { get; set; }
        //public BillingFactorEntity BillingFactor { get; set; }
        //public BillingTariffInformationEntity BillingTariffInformation { get; set; }
        //public CounterEntity Counter { get; set; }
        //public CumulativeDemandBillingTimeStampEntity CumulativeDemandBillingTimeStamp { get; set; }
        //public CumulativeDemandGeneralEntity CumulativeDemandGeneral { get; set; }
        //public CumulativeEnergyEntity CumulativeEnergy { get; set; }
        //public CurrentPhaseEntity CurrentPhase { get; set; }
        //public CurrentTimeStampEntity CurrentTimeStamp { get; set; }
        //public  DemandElapsedTimeEntity DemandElapsedTime { get; set; }
        public  DTMDailyProfileEntity DTMDailyProfile { get; set; }
        public DTMLoadSurveyEntity DTMLoadSurvey { get; set; }
        //public  EnergyEntity Energy { get; set; } 
        public IECFraudEnergyEntity FraudEnergy { get; set; }
        public InstantPowerEntity InstantPower { get; set; }
        public IECLoadSurveyEntity LoadSurvey { get; set; }
        public IECPhasorEntity Phasor { get; set; }
        public PowerFactorEntity PowerFactor { get; set; }
        public ProgrammingEntity Programming { get; set; }
        public RTCUpdateEntity RTCUpdate { get; set; }
        public TamperCounterEntity TamperCounter { get; set; }
        public TamperCounterGeneralEntity TamperCounterGeneral { get; set; }
       // public TamperInformationEntity TamperInformation { get; set; }
        public TamperSnapshotEntity TamperSnapShot { get; set; }
        //public TariffDemandEntity TariffDemand { get; set; }
        //public TariffEnergyEntity TariffEnergy { get; set; }
        //public TariffInformationEntity TariffInformation { get; set; }
        //public VoltagePhaseEntity VoltagePhase { get; set; }
    }
}
