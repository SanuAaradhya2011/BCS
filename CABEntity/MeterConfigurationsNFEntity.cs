using System;
using System.Collections;
using System.Collections.Generic;
using CABEntity;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace CAB.Entity
{
    public enum TOUType
    {
        [DescriptionAttribute("Current")]
        Current,

        [DescriptionAttribute("Future")]
        Future,
    }

    public enum DemandType
    {
        [DescriptionAttribute("Block Demand")]
        BlockDemand,

        [DescriptionAttribute("Sliding Demand")]
        SlidingDemand,
    }


    public class MeterConfigurationsNFEntity
    {
        public E650MDWithIPEntity mdWithIPEntity { get; set; }  
        public kvarSelectionEntity kvarselectionEntity { get; set; }
        public DailyLogEntity dailyLogEntity { get; set; }
        public BillingTypeEntity billingTypeEntity { get; set; }
        public RS232LockEntity RS232Entity { get; set; }
        /// <summary>
        /// To hold RTC
        /// </summary>
        public RTCEntity rtcEnity { get; set; }
        /// <summary>
        /// To hold SIP
        /// </summary>
        public int SIP { get; set; }
        /// <summary>
        /// To hold Display paremeter
        /// </summary>
        public Collection<DisplayParamatersDBEntity> DisplayParamater { get; set; }
        public AutoLockEntity AutoLockEntity { get; set; }
        public long MeterDataId { set; get; }       
        public TODEntity TODEntity { get; set; }
        public LSIPEntity LSIPEntity { get; set; }
        public DIPEntity DIPEntity { get; set; }
        public ManualBillingEntity ManualBillingEntity { get; set; }
        public SoftwareBillingEntity SoftwareBillingEntity { get; set; }
        public DisconnectControlEntity DisconnectControlEntity { get; set; }
        public LoadControlEntity LoadControlEntity { get; set; }
        public RS485Entity RS485Entity { get; set; }
        public PaymentModeEntity PaymentModeEntity { get; set; }
        public MeteringModeEntity MeteringModeEntity { get; set; }
        public LoadLimitEntity LoadLimitEntity { get; set; }
        public SlidingDemandEntity SlidingDemandEntity { get; set; }
        public OpticalLockUnlockEntity OpticalLockEntity { get; set; }
        public RJLockUnlockEntity RJLockEntity { get; set; }

        public PulseEnergyEntity PulseEnergyEntity { get; set; }
        public ManualMDResetEntity ManualButtonMDResetEntity { get; set; }
    }
}
