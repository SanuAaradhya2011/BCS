using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAB.Parser
{
    /// <summary>
    /// Specifies  ProfileId's.
    /// </summary>
    public enum ProfileId
    {
        [Description("Instantaneous")]
        Instant = 1,
        Billing,//2
        [Description("Load Survey")]
        LoadSurvey,//3
        Tamper,//4
        [Description("Name Plate")]
        NamePlate,//5
        [Description("Daily Load Profile")]
        Midnight,//6
        Anomaly,//7
        Tou,//8
        Phasor,//9
        [Description("Fraud Energy")]
        FraudEnergy,//10
        [Description("Meter Accuracy Check")]
        MeterAccuracyCheck,
        Transaction,
        [Description("General and Billing")]
        GeneralBilling,
        [Description("Name Plate")]
        NamePlateProfile,
        // Do not use Any Profile ID greater than 100 for Readout Will impact CMRI
        [Description("RTC")]
        RTC = 101,
        [Description("LS Capture Period")]//102
        SIP,
        [Description("Demand Integration Period")]//103
        DIP,
        [Description("Billing Type")]//104
        BillingType,
        [Description("TOD")]//105
        TOU,
        [Description("Billing Reset")]//106
        BillingReset,
        [Description("Kvah/Mvah Selection")]//107
        KvahSelection,
        [Description("Lock/Unlock RS232")]//108
        RS232LockUnlock,
        [Description("Reset LockOut Days")]//109
        ResetLockOutDays,
        [Description("Passive Season Profile")]//110
        PassiveSeasonProfile,
        [Description("Passive Week Profile")]//111
        PassiveWeekProfile,
        [Description("Passive Day Profile")]//112
        PassiveDayProfile,
        [Description("Active Season Profile")]//113
        ActiveSeasonProfile,
        [Description("Active Week Profile")]//114
        ActiveWeekProfile,
        [Description("Active Day Profile")]//115
        ActiveDayProfile,
        [Description("Activation Date")]//116
        ActivationDate,
        [Description("Push Display Parameter")]//117
        PushDisplayParameter,
        [Description("Scroll Disply Parameter")]//118
        ScrollDisplyParameter,
        [Description("High Resolution Display Parameter")]//119
        HighResolutionDisplayParameter,
        [Description("Display Timeout Parameter")]//120
        DisplayTimeoutParameter,
        [Description("Auto Lock")]//121
        AutoLock,
        [Description("Meter Configuration")]//122
        MeterConfiguration,
        [Description("Display Parameters")]//123
        DisplayParameters,
        [Description("CT Ratio")]//124
        CTRatio,
        [Description("PT Ratio")]//125
        PTRatio,
        [Description("TOD")]//126
        TwoTOU,
        [Description("TOD")]//127
        FourTOU,
        [Description("TOD")]//128
        FourSPTOU,
        [Description("Daily Log")]//129
        DailyLog,
        //[Description("TOD")] // 3PH THREE TOU
        //ThreeSTOU,
        [Description("DTM Programming")]//130
        DTM,
        [Description("Tamper Reset")]//131
        MagneticTamperIcon,
        [Description("MD with IP")]//132
        DIPWithSliding,
        [Description("TOD")]//133
        FourTOUWithHoliday,
        [Description("Manual Billing")]//134
        ManualBilling,
        [Description("Software Billing")]//135
        SoftwareBilling,
        [Description("Billing Month Type")] //[BillingType_Month]//136
        BillingMonthType,
        [Description("TOD")]//137
        FourSPTOU10Z8S,
        [Description("Special day profile for smart meters")]//138
        SpecialDayProfileSmartMeter,
        [Description("Load Control")]//139
        LoadControl,
        [Description("Disconnect Control")]//140 connect/disconnect and status
        DisconnectControl,
        [Description("NON-DLMS Display Parameters")]//141
        DisplayParametersIEC,
        [Description("Load Control")]//142
        LoadControl1PSmartMeter,
        // Do not use Any Profile ID greater than 100 for Readout Will impact CMRI
        [Description("RS 485 Device Address")]//143
        RS485,
        [Description("ABC CODE")]//144
        ABCCode,
        // Task ID: 569567 Tamper Reset option for Torrent Power 3P 10-60 WCM meter having specific right authority to reset
        [Description("Tamper Reset")] //145
        MagneticTamperIcon3P,
        [Description("TOD")] //146 3PH THREE TOU 
        ThreeSTOU,
        [Description("PaymentMode")] //147 For smart meter 
        Paymentmode,
        [Description("MeteringMode")] //148 For smart meter 
        Meteringmode,
        [Description("LoadLimit")] //149 For smart meter 
        LoadLimit,
        [Description("SlidingDemand")] //150 For smart meter 
        Slidingdemand,
        [Description("Optical Port Lock Unlock")] //151 For smart meter 
        OpticalLockUnlock,
        [Description("RJ Port Lock Unlock")] //152 For smart meter 
        RJLockUnlock,
        [Description("Event Enable Disable Configuration")] //153 For smart meter 
        EventEnableDisable,
        [Description("ARM button Enable")] //154 For smart meter 
        ARMButtonEnable,
        [Description("ESWF change")] //155 For smart meter 
        ESWFChange,
        [Description("LoadLimit Function Disabled")] //156 For smart meter 
        LoadLimitDisabled,

        [Description("BillingReset LoadLimit Function Enabled")] //157 For smart meter 
        LoadLimitEnabled,

        [Description("Globalkey Encryption Authentication change")] //158 For smart meter 
        GlobalkeyChange,

        [Description("HLSkey US Change")] //159 For smart meter 
        HLSkeyChange,

        [Description("BILL Date Change")] //160 For smart meter 
        BillDateChange,

        [Description("LLSSecret MR Change")] //161 For smart meter 
        LLSSecret,

        [Description("Image Activation Single Action Schedule")] //162 For smart meter 
        ImageActivationSchedule,

        [Description("Load Switch Profile")] //163 For smart meter 
        LoadSwitch,
        [Description("Event Threshold config change")] //164 For smart meter 
        EventThresholdConfig,
        [Description("Event Threshold Persistence time change ")] //165 For smart meter 
        EventThresholdPersistence,
        [Description("Event Display Parameters Change")] //166 For smart meter 
        EventDisplayPara,
       [Description("ConfigChanged Forward Mode Only")] //167 For smart meter 
        ConfigChangedForwardMode,
       [Description("New Firmware Activated")] //168 For smart meter 
       NewFirmware,
       [Description("HLS key(FW)Change")] //169 For smart meter 
       HLSkeyFWChange,
       [Description("MD Reset")] //170 For smart meter 
       MDReset,
       [Description("Config Changed Import and Export Mode")] //171 For smart meter 
       ConfigChangedImportExportMode,
       [Description("Last Token Recharge Amount prepaid")] //172 For smart meter 
       LastTokenAmountPrepaid,
       [Description("Last Token Recharge Time prepaid")] //173 For smart meter 
       LastTokenTimePrepaid,
       [Description("Total Amount Last Recharge prepaid")] //174 For smart meter 
       TotalAmountLastRecharge,
       [Description("Current Balance Amount prepaid")] //175 For smart meter 
       CurrentBalanceAmount,
       [Description("Current Balance Time prepaid")] //176 For smart meter 
       CurrentBalanceTime,
       [Description("Digital output Operation")] //177 For smart meter 
       DigitalOutputOperation,
       [Description("Sliding Demand Period Change")] //178 For smart meter 
       SIPPeriodChange,
       [Description("LS Parameter Store ID")] //179 For smart meter 
       LSParameterStoreID,
       [Description("Optical port Lock")] //180 For smart meter 
       OpticalLock,
       [Description("RJ port Lock")] //181 For smart meter 
       RJLock,
       [Description("Special Day")] //182 For smart meter 
       SpecialDay,
       [Description("ARM button Disable")] //183 For smart meter 
       ARMButtonDisable,
       [Description("FS Mode Lock")] //184 For smart meter 
       FSModeLock,
       [Description("FS Mode Unlock")] //185 For smart meter 
       FSModeUnlock,
       [Description("MR password")] //186 MR password write 
       MRPasswordWrite,
       [Description("HR Profile")] //187 High resolution Profile 
       HRProfile,
       [Description("Pulse Energy Type")] //188 Meter Pulse Energy Type 
        PulseEnergy,
       [Description("Manual Button MD Reset")] //189 Meter Pulse Energy Type 
        ManualButtonMDReset,
    };
}

