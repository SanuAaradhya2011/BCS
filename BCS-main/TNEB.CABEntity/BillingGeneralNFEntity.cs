/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;
using System.Collections;
using System.Collections.Generic;
using CAB.E650MeterConfiguration.Entity;


namespace CAB.Entity
{
    public class IECBillingGeneralNFEntity
    {
        //Master 
        public List<GeneralData> listGeneralData { get; set; }

        public List<IECFraudEnergyEntity> listFraudEnergy { get; set; }
        public List<RTCUpdateEntity> listRTCUpdate { get; set; }
        public List<IECPhasorEntity> listPhasor { get; set; }
        public List<LoadSurveyMeterDataList> LoadSurveyMeterDataList { get; set; }
        public List<DTMLoadSurveyEntity> DTMLoadSurvey { get; set; }

        public List<TransactionData> listTransactionData { get; set; }
        
        //public MeterDataEntity DTMLoadSurveyMeterData { get; set; }

        public List<LoadSurveyData> listLoadSurveyData { get; set; }
        public List<TamperData> listTamper { get; set; }
        public List<DTMDailyProfileData> listDTMDailyProfileData { get; set; }

        public List<MeterDataHeaderInfoEntity> listHeaderInfo { get; set; }
        public List<NamePlateDetailEntity> listNamePlateDetail { get; set; }
        public List<IECMeterConfigurationsNFEntity> meterConfigurationDetail { get; set; }
        public long MeterDataID { get; set; }

       
       
    }

    public class GeneralData
    {
        public IECMeterDataEntity MeterData { get; set; }

        public InstantPowerEntity CurrentInstant { get; set; }
        public GeneralEntity CurrentGeneral { get; set; }
        public IECBillingEntity CurrentBilling { get; set; }
        public TariffEntity CurrentTariff { get; set; }
        public TamperCounterGeneralEntity CurrentTamper { get; set; }
        public List<IECBillingEntity> listHistoryBilling { get; set; }
        public List<TariffEntity> listHistoryTariff { get; set; }
        public List<TamperCounterGeneralEntity> listHistoryTamper { get; set; }
        public AnomalyEntityForSP Anomaly { get; set; }

    }

    public class TransactionData
    {
        public List<ProgrammingEntity> programmingData=new List<ProgrammingEntity>();
        public IECMeterDataEntity meterDataEntity { get; set; }
    }

    public class TamperData
    {
        public TamperCounterGeneralEntity General { get; set; }
        public TamperCounterEntity Counter { get; set; }
        public List<TamperSnapshotEntity> Snapshot { get; set; }
    }
    public class LoadSurveyData
    {
        public IECMeterDataEntity LoadSurveyMeterData { get; set; }
        public List<IECLoadSurveyEntity> LoadSurvey { get; set; }
    }
    public class DTMDailyProfileData
    {
        public List<DTMDailyProfileEntity> DTMDailyProfile { get; set; }
        public IECMeterDataEntity DTMDailyProfileMeterData { get; set; }
    }
}
