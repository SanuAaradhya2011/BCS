/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;
using System.Collections;
using System.Collections.Generic;
using LNG.Framework.Utility;


namespace LNG.Entity
{
    public class BillingGeneralNFEntity
    {
        //Master 
        //public MeterDataEntity MeterData { get; set; }
        //public InstantPowerEntity CurrentInstant{ get; set; }
        //public GeneralEntity CurrentGeneral { get; set; }
        //public BillingEntity CurrentBilling { get; set; }
        //public TariffEntity CurrentTariff { get; set; }
        //public TamperCounterGeneralEntity CurrentTamper { get; set; }
        //public List<BillingEntity> HistoryBilling { get; set; }
        //public List<TariffEntity> HistoryTariff { get; set; }
        //public List<TamperCounterGeneralEntity> HistoryTamper { get; set; }
        //public FraudEnergyEntity FraudEnergy { get; set; }
        //public List<ProgrammingEntity> Programming { get; set; }
        //public RTCUpdateEntity RTCUpdate { get; set; }
        //public PhasorEntity Phasor { get; set; }
        //public List<LoadSurveyEntity> LoadSurvey { get; set; }
        //public TamperData Tamper { get; set; }
        //public List<DTMLoadSurveyEntity> DTMLoadSurvey { get; set; }
        //public List<DTMDailyProfileEntity> DTMDailyProfile { get; set; }
        //public MeterDataEntity DTMDailyProfileMeterData { get; set; }
        //public MeterDataEntity DTMLoadSurveyMeterData { get; set; }
        //public MeterDataEntity LoadSurveyMeterData { get; set; }

    }
    public class BillingGeneralNFDLMSEntity
    {
        public MeterDataEntity MeterData { get; set; }
        public DLMS650NamePlateDetailsEntity General { get; set; }
        public DLMS650NamePlateDetailsEntity NamePlateProfile { get; set; }
        public List<DLMS650InstantaneousEntity> Instant { get; set; }
        public List<DLMS650LoadSurveyEntity> LoadSurvey{ get; set; }
        //added for MVVNL
        public List<DLMS650MidnightDataEntity> MidnightData { get; set; }
        //added for MVVNL
        public List<DLMS650BillingEntity> Billing { get; set; }
        public List<DLMS650TamperEntity> Tamper { get; set; }
        public BillingParameterEntity BillingParameterColumns { get; set; }
        public LoadSurveyParameterEntity LSParameterColumns { get; set; }
        public DTMDailyProfileParameterEntity MidnightParameterColumns { get; set; }
        public TamperParameterEntity TamperParameterColumns { get; set; }
        public MeterDataTypes MeterDataType { get; set; }
        public AnomalyEntity Anomaly { get; set; }
        public PhasorEntity Phasor { get; set; }
        public FraudEnergyEntity FraudEnergy { get; set; }
        public List<LoadSwitchEntity> LoadSwitch { get; set; }
        public LoadSwitchParameterEntity LoadSwitchParameterColumns { get; set; }
        /* GKG JVVNL Current TOU Read */
        public List<TOU> TOU { get; set; }
        //BhardwajG For holding demand integration period
        public int DemandIntegrationPeriod { get; set; }
        public string FileType { get; set; }
        public MeterConfigurationsNFEntity MeterConfigurations { get; set; }
        /* GKG JVVNL Current TOU Read */
        public long MeterDataID { get; set; }
        public long ReadoutCounter { get; set; } // Story - 358810 - Readout counter value for single phase non DLMS meter integration


    }
    //public class TamperData
    //{
    //    public TamperCounterGeneralEntity General { get; set; }
    //    public TamperCounterEntity Counter { get; set; }
    //    public List<TamperSnapshotEntity> Snapshot { get; set; }
    //}
}

