using System;
using CAB.IECFramework.Entity;
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


    public class IECMeterConfigurationsNFEntity
    {
        public MDWithIPEntity mdWithIPEntity { get; set; }  
        public IECkvarSelectionEntity kvarselectionEntity { get; set; }
        public IECDailyLogEntity dailylogentity { get; set; }
        public BillingResetEntity billingresetentity { get; set; }
        public IECRS232LockEntity RS232Entity { get; set; }
        /// <summary>
        /// To hold RTC
        /// </summary>
        public string RTC { get; set; }
        /// <summary>
        /// To hold SIP
        /// </summary>
        public int SIP { get; set; }
        /// <summary>
        /// To hold Display paremeter
        /// </summary>
        public Collection<DisplayParamatersDBEntity> DisplayParamater { get; set; }
        public IECAutoLockEntity AutoLockEntity { get; set; }
        public IECTODEntity TODEntity { get; set; }
    }
}
