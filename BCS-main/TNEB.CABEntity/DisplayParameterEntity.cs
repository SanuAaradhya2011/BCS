using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using CAB.IECFramework.Entity;

namespace CABEntity
{
    #region Enum - Display Parameter Type Data Object
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :DisplayParameter is enumeration in DisplayParametertype
    /// 
    /// </summary>
    public enum DisplayParameter
    {
        [DescriptionAttribute("Push Mode")]
        PushMode=1,
        [DescriptionAttribute("Scroll Mode")]
        ScrollMode=2,
        [DescriptionAttribute("High Resolution Mode")]
        HighResolutionMode=3,
        [DescriptionAttribute("Display Timeouts")]
        DisplayTimeouts=4
    }
    #endregion

    #region Display Paramater object to write in DB.

    public class DisplayParamatersDBEntity : EntityBase
    {
        public DisplayParameter displayParamaterType;
        public string paramaterName;
        public int paramaterValue;

    }
    #endregion

    #region Enum - Push Mode Display Parameter Type Data Object
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :PushModeParameters is enumeration in PushModeParameter type
    /// 
    /// </summary>
    /// 
    public enum PushModeParameters
    {
        [DescriptionAttribute("LCD Test")]
        [ParamaterCodeAttribute("3031")]
        LCDTest,

        [DescriptionAttribute("Anomaly Indicator")]
        [ParamaterCodeAttribute("3032")]
        AnomalyIndicator,
        [DescriptionAttribute("Meter Serial Number")]
        [ParamaterCodeAttribute("3033")]
        MeterSerialNumber,

        [DescriptionAttribute("Meter Constant")]
        [ParamaterCodeAttribute("3034")]
        MeterConstant,
        [DescriptionAttribute("Firmware Version")]
        [ParamaterCodeAttribute("3035")]
        FirmwareVersion,

        [DescriptionAttribute("Voltage Phase Sequence")]
        [ParamaterCodeAttribute("3037")]
        VoltagePhaseSequence,

        [DescriptionAttribute("Meter Reading Count Register")]
        [ParamaterCodeAttribute("3039")]
        MeterReadingCountRegister,

        [DescriptionAttribute("Total Power ON Hours")]
        [ParamaterCodeAttribute("3530")]
        TotalPowerONHours,

        [DescriptionAttribute("Current Power ON Hours")]
        [ParamaterCodeAttribute("3531")]
        CurrentPowerONHours,

        [DescriptionAttribute("Power on Hours in Battery")]
        [ParamaterCodeAttribute("3743")]
        PoweronHoursinBattery,

        [DescriptionAttribute("Current Month Load Factor")]
        [ParamaterCodeAttribute("3541")]
        CurrentMonthLoadFactor,

        [DescriptionAttribute("Current Month APF")]
        [ParamaterCodeAttribute("3745")]
        CurrentMonthAPF,

        [DescriptionAttribute("Magnetic Tamper Status")]
        [ParamaterCodeAttribute("3746")]
        MagneticTamperStatus,

        [DescriptionAttribute("R Phase THD")]
        [ParamaterCodeAttribute("3830")]
        RPhaseTHD,

        [DescriptionAttribute("Y Phase THD")]
        [ParamaterCodeAttribute("3831")]
        YPhaseTHD,


        [DescriptionAttribute("B Phase THD")]
        [ParamaterCodeAttribute("3832")]
        BPhaseTHD,

        [DescriptionAttribute("Real Date (DD-MM-YY)")]
        [ParamaterCodeAttribute("3041")]
        RealDate,

        [DescriptionAttribute("Real Time (HH:MM:SS)")]
        [ParamaterCodeAttribute("3042")]
        RealTime,

        [DescriptionAttribute("Cumulative KWh (Fundamental only)")]
        [ParamaterCodeAttribute("3043")]
        CumulativeKWhFundamentalonly,

        [DescriptionAttribute("Cumulative KWh")]
        [ParamaterCodeAttribute("3044")]
        CumulativeKWh,

        [DescriptionAttribute("Cumulative KVARh (lag)")]
        [ParamaterCodeAttribute("3045")]
        CumulativeKVARh_lag,

        [DescriptionAttribute("Cumulative KVARh (lead)")]
        [ParamaterCodeAttribute("3046")]
        CumulativeKVARh_lead,

        [DescriptionAttribute("Cumulative KVAh")]
        [ParamaterCodeAttribute("3130")]
        CumulativeKVAh,

        [DescriptionAttribute("Rising KW With Elapsed Time (in MM:SS)")]
        [ParamaterCodeAttribute("3131")]
        RisingKWWithElapsedTimeInMMSS,

        [DescriptionAttribute("Rising KVA With Elapsed Time (in MM:SS)")]
        [ParamaterCodeAttribute("3132")]
        RisingKVAWithElapsedTimeInMMSS,

        [DescriptionAttribute("Current Demand KW")]
        [ParamaterCodeAttribute("3133")]
        CurrentDemandKW,

        [DescriptionAttribute("Current Demand KW Date")]
        [ParamaterCodeAttribute("3134")]
        CurrentDemandKWDate,

        [DescriptionAttribute("Current Demand KW Time")]
        [ParamaterCodeAttribute("3135")]
        CurrentDemandKWTime,

        [DescriptionAttribute("Current Demand KVA")]
        [ParamaterCodeAttribute("3136")]
        CurrentDemandKVA,

        [DescriptionAttribute("Current Demand KVA Date")]
        [ParamaterCodeAttribute("3137")]
        CurrentDemandKVADate,

        [DescriptionAttribute("Current Demand KVA Time")]
        [ParamaterCodeAttribute("3138")]
        CurrentDemandKVATime,

        [DescriptionAttribute("Cumulative Demand KW")]
        [ParamaterCodeAttribute("3143")]
        CumulativeDemandKW,

        [DescriptionAttribute("Cumulative Demand KVA")]
        [ParamaterCodeAttribute("3144")]
        CumulativeDemandKVA,

        [DescriptionAttribute("Instantaneous Active Power")]
        [ParamaterCodeAttribute("3146")]
        InstantaneousActivePower,

        [DescriptionAttribute("Instantaneous Apparent Power")]
        [ParamaterCodeAttribute("3230")]
        InstantaneousApparentPower,

        [DescriptionAttribute("Instantaneous Reactive Power  (Lag)")]
        [ParamaterCodeAttribute("3231")]
        InstantaneousReactivePowerLag,

        [DescriptionAttribute("Instantaneous Reactive Power  (Lead)")]
        [ParamaterCodeAttribute("3145")]
        InstantaneousReactivePowerLead,

        [DescriptionAttribute("Instantaneous R Phase Voltage")]
        [ParamaterCodeAttribute("3232")]
        InstantaneousRPhaseVoltage,

        [DescriptionAttribute("Instantaneous Y Phase Voltage")]
        [ParamaterCodeAttribute("3233")]
        InstantaneousYPhaseVoltage,

        [DescriptionAttribute("Instantaneous B Phase Voltage")]
        [ParamaterCodeAttribute("3234")]
        InstantaneousBPhaseVoltage,

        [DescriptionAttribute("Instantaneous R Phase Current")]
        [ParamaterCodeAttribute("3235")]
        InstantaneousRPhaseCurrent,

        [DescriptionAttribute("Instantaneous Y Phase Current")]
        [ParamaterCodeAttribute("3236")]
        InstantaneousYPhaseCurrent,

        [DescriptionAttribute("Instantaneous B Phase Current")]
        [ParamaterCodeAttribute("3237")]
        InstantaneousBPhaseCurrent,

        [DescriptionAttribute("Instantaneous Total Power Factor (with lag and lead sign)")]
        [ParamaterCodeAttribute("3238")]
        InstantaneousTotalPowerFactorwithlagandleadsign,

        [DescriptionAttribute("Instantaneous R Phase Power Factor")]
        [ParamaterCodeAttribute("3239")]
        InstantaneousRPhasePowerFactor,

        [DescriptionAttribute("Instantaneous Y Phase Power Factor")]
        [ParamaterCodeAttribute("3241")]
        InstantaneousYPhasePowerFactor,

        [DescriptionAttribute("Instantaneous B Phase Power Factor")]
        [ParamaterCodeAttribute("3242")]
        InstantaneousBPhasePowerFactor,

        [DescriptionAttribute("Frequency")]
        [ParamaterCodeAttribute("3243")]
        Frequency,

        [DescriptionAttribute("ABC Encrypted Display")]
        [ParamaterCodeAttribute("3139")]
        ABCEncryptedDisplay,

        [DescriptionAttribute("Billing Reset Counter")]
        [ParamaterCodeAttribute("3244")]
        BillingResetCounter,

        [DescriptionAttribute("Billing Date and Time Stamp")]
        [ParamaterCodeAttribute("3245")]
        BillingDateandTimeStamp,

        [DescriptionAttribute("Billing KWh")]
        [ParamaterCodeAttribute("3246")]
        BillingKWh,

        [DescriptionAttribute("Billing KVARh (lag)")]
        [ParamaterCodeAttribute("3330")]
        BillingKVARhLag,

        [DescriptionAttribute("Billing KVARh (lead)")]
        [ParamaterCodeAttribute("3331")]
        BillingKVARhLead,

        [DescriptionAttribute("Billing KVAh")]
        [ParamaterCodeAttribute("3332")]
        BillingKVAh,

        [DescriptionAttribute("Billing Demand KW Date and Time")]
        [ParamaterCodeAttribute("3334")]
        BillingDemandKWDateandTime,

        [DescriptionAttribute("Billing Demand KVA Date and Time")]
        [ParamaterCodeAttribute("3336")]
        BillingDemandKVADateandTime,

        [DescriptionAttribute("Billing Period Average Power Factor")]
        [ParamaterCodeAttribute("3339")]
        BillingPeriodAveragePowerFactor,

        [DescriptionAttribute("Billing Power On Hours")]
        [ParamaterCodeAttribute("3744")]
        BillingPowerOnHours,

        [DescriptionAttribute("Billing Load Factor")]
        [ParamaterCodeAttribute("3542")]
        BillingLoadFactor,

        [DescriptionAttribute("TOU KWh; Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3341")]
        TOUKWhRate1toRate8,

        [DescriptionAttribute("TOU KVARh (lag) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3342")]
        TOUKVARhLagRate1toRate8,

        [DescriptionAttribute("TOU KVARh (lead) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3343")]
        TOUKVARhLeadRate1toRate8,

        [DescriptionAttribute("TOU KVAh : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3344")]
        TOUKVAhRate1toRate8,

        [DescriptionAttribute("TOU Demand KW : Date and Time : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3346")]
        TOUDemandKWDateandTimeRate1toRate8,

        [DescriptionAttribute("TOU Demand KVA : Date and Time : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3431")]
        TOUDemandKVADateandTimeRate1toRate8,

        [DescriptionAttribute("TOU Average Power Factor : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3434")]
        TOUAveragePowerFactorRate1toRate8,

        [DescriptionAttribute("Billing TOU KWh : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3435")]
        BillingTOUKWhRate1toRate8,

        [DescriptionAttribute("Billing TOU KVARh (lag) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3436")]
        BillingTOUKVARhLagRate1toRate8,

        [DescriptionAttribute("Billing TOU KVARh (lead) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3437")]
        BillingTOUKVARhLeadRate1toRate8,

        [DescriptionAttribute("Billing TOU KVAh: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3438")]
        BillingTOUKVAhRate1toRate8,

        [DescriptionAttribute("Billing TOU Demand KW Date and Time: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3441")]
        BillingTOUDemandKWDateandTimeRate1toRate8,

        [DescriptionAttribute("Billing TOU Demand KVA Date and Time:  Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3443")]
        BillingTOUDemandKVADateandTimeRate1toRate8,

        [DescriptionAttribute("Billing TOU Average Power Factor: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3446")]
        BillingTOUAveragePowerFactorRate1toRate8,

        [DescriptionAttribute("Tamper Status")]
        [ParamaterCodeAttribute("3532")]
        TamperStatus,

        [DescriptionAttribute("Tamper Counter Cumulative")]
        [ParamaterCodeAttribute("3533")]
        TamperCounterCumulative,

        [DescriptionAttribute("Latest Tamper Occurrence - Tamper ID")]
        [ParamaterCodeAttribute("3534")]
        LatestTamperOccurrenceTamperID,

        [DescriptionAttribute("Latest Tamper Occurrence Time Stamp")]
        [ParamaterCodeAttribute("3535")]
        LatestTamperOccurrenceTimeStamp,

        [DescriptionAttribute("Latest Tamper Restoration - Tamper ID")]
        [ParamaterCodeAttribute("3536")]
        LatestTamperRestorationTamperID,

        [DescriptionAttribute("Latest Tamper Restoration Time Stamp")]
        [ParamaterCodeAttribute("3537")]
        LatestTamperRestorationTimeStamp,

        [DescriptionAttribute("Front Cover Tamper Occurance - Tamper ID")]
        [ParamaterCodeAttribute("3141")]
        FrontCoverTamperOccuranceTamperID,

        [DescriptionAttribute("Front Cover Tamper Occurance - Time Stamp")]
        [ParamaterCodeAttribute("3142")]
        FrontCoverTamperOccuranceTimeStamp,

        [DescriptionAttribute("Reverse Cumulative KWh")]
        [ParamaterCodeAttribute("3538")]
        ReverseCumulativeKWh,

        [DescriptionAttribute("Reverse Cumulative KVAh")]
        [ParamaterCodeAttribute("3539")]
        ReverseCumulativeKVAh,

        [DescriptionAttribute("R Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3543")]
        RPhaseMissingPotentialCounter,

        [DescriptionAttribute("Y Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3544")]
        YPhaseMissingPotentialCounter,

        [DescriptionAttribute("B Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3545")]
        BPhaseMissingPotentialCounter,

        [DescriptionAttribute("R Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3546")]
        RPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("Y Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3630")]
        YPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("B Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3631")]
        BPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("R Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3632")]
        RPhaseCurrentReversalCounter,

        [DescriptionAttribute("Y Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3633")]
        YPhaseCurrentReversalCounter,

        [DescriptionAttribute("B Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3634")]
        BPhaseCurrentReversalCounter,

        [DescriptionAttribute("Current Short/ Bypass Counter")]
        [ParamaterCodeAttribute("3635")]
        CurrentShort_BypassCounter,

        [DescriptionAttribute("R Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3636")]
        RPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("Y Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3637")]
        YPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("B Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3638")]
        BPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("R Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3639")]
        RPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("Y Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3641")]
        YPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("B Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3642")]
        BPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("Magnetic Tamper Counter")]
        [ParamaterCodeAttribute("3643")]
        MagneticTamperCounter,

        [DescriptionAttribute("Neutral Disturbance Counter")]
        [ParamaterCodeAttribute("3644")]
        NeutralDisturbanceCounter,

        [DescriptionAttribute("Voltage Phase Sequence Reversal Counter")]
        [ParamaterCodeAttribute("3734")]
        VoltagePhaseSequenceReversalCounter,

        [DescriptionAttribute("Front Cover Open Counter")]
        [ParamaterCodeAttribute("3736")]
        FrontCoverOpenCounter,

        [DescriptionAttribute("Terminal Cover Open Counter")]
        [ParamaterCodeAttribute("3737")]
        TerminalCoverOpenCounter,

        [DescriptionAttribute("Two Phase Operation Counter")]
        [ParamaterCodeAttribute("3645")]
        TwoPhaseOperationCounter,

        [DescriptionAttribute("Instantaneous Signed Power in KW in R Phase")]
        [ParamaterCodeAttribute("3838")]
        InstantaneousSignedPowerinKWinRPhase,

        [DescriptionAttribute("Instantaneous Signed Power in KW in Y Phase")]
        [ParamaterCodeAttribute("3839")]
        InstantaneousSignedPowerinKWinYPhase,

        [DescriptionAttribute("Instantaneous Signed Power in KW in B Phase")]
        [ParamaterCodeAttribute("3841")]
        InstantaneousSignedPowerinKWinBPhase,

        [DescriptionAttribute("Maximum Demand in KW for Last Reset")]
        [ParamaterCodeAttribute("3333")]
        MaximumDemandinKWforLastReset,

        [DescriptionAttribute("Power Off hours since last reset billing period.")]
        [ParamaterCodeAttribute("3836")]
        PowerOffhourssincelastresetbillingperiod,

        [DescriptionAttribute("Cumulative Power Off Hours in R Phase")]
        [ParamaterCodeAttribute("3833")]
        CumulativePowerOffHoursinRPhase,

        [DescriptionAttribute("Cumulative Power Off Hours in Y Phase")]
        [ParamaterCodeAttribute("3834")]
        CumulativePowerOffHoursinYPhase,

        [DescriptionAttribute("Cumulative Power Off Hours in B Phase")]
        [ParamaterCodeAttribute("3835")]
        CumulativePowerOffHoursinBPhase,

        [DescriptionAttribute("LBP-Cumulative KWh reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3842")]
        LBPCumulativeKWhreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP-Cumulative KVARh reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3843")]
        LBPCumulativeKVARhreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP APF reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3845")]
        LBPAPFreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP Maximum demand in KW  at the time of prior to reset")]
        [ParamaterCodeAttribute("3844")]
        LBPMaximumdemandinKWatthetimeofpriortoreset,

        [DescriptionAttribute("Power Off hours for the last billing period")]
        [ParamaterCodeAttribute("3837")]
        PowerOffhoursforthelastbillingperiod,

        /* GKG 21/01/2013 TANGEDCO ISSUE*/
        [DescriptionAttribute("Magnetic Interference Date And Time stamp")]
        [ParamaterCodeAttribute("3846")]
        MagneticInterferenceDateTime
        /* GKG 21/01/2013 TANGEDCO ISSUE*/




    }

    #endregion

    #region Enum - Scroll Mode Display Parameter Type Data Object
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :ScrollModeParameters is enumeration in ScrollModeParameter type
    /// 
    /// </summary>
    /// 
    public enum ScrollModeParameters
    {
        [DescriptionAttribute("LCD Test")]
        [ParamaterCodeAttribute("3031")]
        LCDTest,

        [DescriptionAttribute("Anomaly Indicator")]
        [ParamaterCodeAttribute("3032")]
        AnomalyIndicator,
        [DescriptionAttribute("Meter Serial Number")]
        [ParamaterCodeAttribute("3033")]
        MeterSerialNumber,

        [DescriptionAttribute("Meter Constant")]
        [ParamaterCodeAttribute("3034")]
        MeterConstant,
        [DescriptionAttribute("Firmware Version")]
        [ParamaterCodeAttribute("3035")]
        FirmwareVersion,

        [DescriptionAttribute("Voltage Phase Sequence")]
        [ParamaterCodeAttribute("3037")]
        VoltagePhaseSequence,

        [DescriptionAttribute("Meter Reading Count Register")]
        [ParamaterCodeAttribute("3039")]
        MeterReadingCountRegister,

        [DescriptionAttribute("Total Power ON Hours")]
        [ParamaterCodeAttribute("3530")]
        TotalPowerONHours,

        [DescriptionAttribute("Current Power ON Hours")]
        [ParamaterCodeAttribute("3531")]
        CurrentPowerONHours,

        [DescriptionAttribute("Power on Hours in Battery")]
        [ParamaterCodeAttribute("3743")]
        PoweronHoursinBattery,

        [DescriptionAttribute("Current Month Load Factor")]
        [ParamaterCodeAttribute("3541")]
        CurrentMonthLoadFactor,

        [DescriptionAttribute("Current Month APF")]
        [ParamaterCodeAttribute("3745")]
        CurrentMonthAPF,

        [DescriptionAttribute("Magnetic Tamper Status")]
        [ParamaterCodeAttribute("3746")]
        MagneticTamperStatus,

        [DescriptionAttribute("R Phase THD")]
        [ParamaterCodeAttribute("3830")]
        RPhaseTHD,

        [DescriptionAttribute("Y Phase THD")]
        [ParamaterCodeAttribute("3831")]
        YPhaseTHD,


        [DescriptionAttribute("B Phase THD")]
        [ParamaterCodeAttribute("3832")]
        BPhaseTHD,

        [DescriptionAttribute("Real Date (DD-MM-YY)")]
        [ParamaterCodeAttribute("3041")]
        RealDate,

        [DescriptionAttribute("Real Time (HH:MM:SS)")]
        [ParamaterCodeAttribute("3042")]
        RealTime,

        [DescriptionAttribute("Cumulative KWh (Fundamental only)")]
        [ParamaterCodeAttribute("3043")]
        CumulativeKWhFundamentalonly,

        [DescriptionAttribute("Cumulative KWh")]
        [ParamaterCodeAttribute("3044")]
        CumulativeKWh,

        [DescriptionAttribute("Cumulative KVARh (lag)")]
        [ParamaterCodeAttribute("3045")]
        CumulativeKVARh_lag,

        [DescriptionAttribute("Cumulative KVARh (lead)")]
        [ParamaterCodeAttribute("3046")]
        CumulativeKVARh_lead,

        [DescriptionAttribute("Cumulative KVAh")]
        [ParamaterCodeAttribute("3130")]
        CumulativeKVAh,

        [DescriptionAttribute("Rising KW With Elapsed Time (in MM:SS)")]
        [ParamaterCodeAttribute("3131")]
        RisingKWWithElapsedTimeInMMSS,

        [DescriptionAttribute("Rising KVA With Elapsed Time (in MM:SS)")]
        [ParamaterCodeAttribute("3132")]
        RisingKVAWithElapsedTimeInMMSS,

        [DescriptionAttribute("Current Demand KW")]
        [ParamaterCodeAttribute("3133")]
        CurrentDemandKW,

        [DescriptionAttribute("Current Demand KW Date")]
        [ParamaterCodeAttribute("3134")]
        CurrentDemandKWDate,

        [DescriptionAttribute("Current Demand KW Time")]
        [ParamaterCodeAttribute("3135")]
        CurrentDemandKWTime,

        [DescriptionAttribute("Current Demand KVA")]
        [ParamaterCodeAttribute("3136")]
        CurrentDemandKVA,

        [DescriptionAttribute("Current Demand KVA Date")]
        [ParamaterCodeAttribute("3137")]
        CurrentDemandKVADate,

        [DescriptionAttribute("Current Demand KVA Time")]
        [ParamaterCodeAttribute("3138")]
        CurrentDemandKVATime,

        [DescriptionAttribute("Cumulative Demand KW")]
        [ParamaterCodeAttribute("3143")]
        CumulativeDemandKW,

        [DescriptionAttribute("Cumulative Demand KVA")]
        [ParamaterCodeAttribute("3144")]
        CumulativeDemandKVA,

        [DescriptionAttribute("Instantaneous Active Power")]
        [ParamaterCodeAttribute("3146")]
        InstantaneousActivePower,

        [DescriptionAttribute("Instantaneous Apparent Power")]
        [ParamaterCodeAttribute("3230")]
        InstantaneousApparentPower,

        [DescriptionAttribute("Instantaneous Reactive Power  (Lag)")]
        [ParamaterCodeAttribute("3231")]
        InstantaneousReactivePowerLag,

        [DescriptionAttribute("Instantaneous Reactive Power  (Lead)")]
        [ParamaterCodeAttribute("3145")]
        InstantaneousReactivePowerLead,

        [DescriptionAttribute("Instantaneous R Phase Voltage")]
        [ParamaterCodeAttribute("3232")]
        InstantaneousRPhaseVoltage,

        [DescriptionAttribute("Instantaneous Y Phase Voltage")]
        [ParamaterCodeAttribute("3233")]
        InstantaneousYPhaseVoltage,

        [DescriptionAttribute("Instantaneous B Phase Voltage")]
        [ParamaterCodeAttribute("3234")]
        InstantaneousBPhaseVoltage,

        [DescriptionAttribute("Instantaneous R Phase Current")]
        [ParamaterCodeAttribute("3235")]
        InstantaneousRPhaseCurrent,

        [DescriptionAttribute("Instantaneous Y Phase Current")]
        [ParamaterCodeAttribute("3236")]
        InstantaneousYPhaseCurrent,

        [DescriptionAttribute("Instantaneous B Phase Current")]
        [ParamaterCodeAttribute("3237")]
        InstantaneousBPhaseCurrent,

        [DescriptionAttribute("Instantaneous Total Power Factor (with lag and lead sign)")]
        [ParamaterCodeAttribute("3238")]
        InstantaneousTotalPowerFactorwithlagandleadsign,

        [DescriptionAttribute("Instantaneous R Phase Power Factor")]
        [ParamaterCodeAttribute("3239")]
        InstantaneousRPhasePowerFactor,

        [DescriptionAttribute("Instantaneous Y Phase Power Factor")]
        [ParamaterCodeAttribute("3241")]
        InstantaneousYPhasePowerFactor,

        [DescriptionAttribute("Instantaneous B Phase Power Factor")]
        [ParamaterCodeAttribute("3242")]
        InstantaneousBPhasePowerFactor,

        [DescriptionAttribute("Frequency")]
        [ParamaterCodeAttribute("3243")]
        Frequency,

        [DescriptionAttribute("ABC Encrypted Display")]
        [ParamaterCodeAttribute("3139")]
        ABCEncryptedDisplay,

        [DescriptionAttribute("Billing Reset Counter")]
        [ParamaterCodeAttribute("3244")]
        BillingResetCounter,

        [DescriptionAttribute("Billing Date and Time Stamp")]
        [ParamaterCodeAttribute("3245")]
        BillingDateandTimeStamp,

        [DescriptionAttribute("Billing KWh")]
        [ParamaterCodeAttribute("3246")]
        BillingKWh,

        [DescriptionAttribute("Billing KVARh (lag)")]
        [ParamaterCodeAttribute("3330")]
        BillingKVARhLag,

        [DescriptionAttribute("Billing KVARh (lead)")]
        [ParamaterCodeAttribute("3331")]
        BillingKVARhLead,

        [DescriptionAttribute("Billing KVAh")]
        [ParamaterCodeAttribute("3332")]
        BillingKVAh,

        [DescriptionAttribute("Billing Demand KW Date and Time")]
        [ParamaterCodeAttribute("3334")]
        BillingDemandKWDateandTime,

        [DescriptionAttribute("Billing Demand KVA Date and Time")]
        [ParamaterCodeAttribute("3336")]
        BillingDemandKVADateandTime,

        [DescriptionAttribute("Billing Period Average Power Factor")]
        [ParamaterCodeAttribute("3339")]
        BillingPeriodAveragePowerFactor,

        [DescriptionAttribute("Billing Power On Hours")]
        [ParamaterCodeAttribute("3744")]
        BillingPowerOnHours,

        [DescriptionAttribute("Billing Load Factor")]
        [ParamaterCodeAttribute("3542")]
        BillingLoadFactor,

        [DescriptionAttribute("TOU KWh; Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3341")]
        TOUKWhRate1toRate8,

        [DescriptionAttribute("TOU KVARh (lag) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3342")]
        TOUKVARhLagRate1toRate8,

        [DescriptionAttribute("TOU KVARh (lead) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3343")]
        TOUKVARhLeadRate1toRate8,

        [DescriptionAttribute("TOU KVAh : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3344")]
        TOUKVAhRate1toRate8,

        [DescriptionAttribute("TOU Demand KW : Date and Time : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3346")]
        TOUDemandKWDateandTimeRate1toRate8,

        [DescriptionAttribute("TOU Demand KVA : Date and Time : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3431")]
        TOUDemandKVADateandTimeRate1toRate8,

        [DescriptionAttribute("TOU Average Power Factor : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3434")]
        TOUAveragePowerFactorRate1toRate8,

        [DescriptionAttribute("Billing TOU KWh : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3435")]
        BillingTOUKWhRate1toRate8,

        [DescriptionAttribute("Billing TOU KVARh (lag) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3436")]
        BillingTOUKVARhLagRate1toRate8,

        [DescriptionAttribute("Billing TOU KVARh (lead) : Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3437")]
        BillingTOUKVARhLeadRate1toRate8,

        [DescriptionAttribute("Billing TOU KVAh: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3438")]
        BillingTOUKVAhRate1toRate8,

        [DescriptionAttribute("Billing TOU Demand KW Date and Time: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3441")]
        BillingTOUDemandKWDateandTimeRate1toRate8,

        [DescriptionAttribute("Billing TOU Demand KVA Date and Time:  Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3443")]
        BillingTOUDemandKVADateandTimeRate1toRate8,

        [DescriptionAttribute("Billing TOU Average Power Factor: Rate 1 to Rate 8")]
        [ParamaterCodeAttribute("3446")]
        BillingTOUAveragePowerFactorRate1toRate8,

        [DescriptionAttribute("Tamper Status")]
        [ParamaterCodeAttribute("3532")]
        TamperStatus,

        [DescriptionAttribute("Tamper Counter Cumulative")]
        [ParamaterCodeAttribute("3533")]
        TamperCounterCumulative,

        [DescriptionAttribute("Latest Tamper Occurrence - Tamper ID")]
        [ParamaterCodeAttribute("3534")]
        LatestTamperOccurrenceTamperID,

        [DescriptionAttribute("Latest Tamper Occurrence Time Stamp")]
        [ParamaterCodeAttribute("3535")]
        LatestTamperOccurrenceTimeStamp,

        [DescriptionAttribute("Latest Tamper Restoration - Tamper ID")]
        [ParamaterCodeAttribute("3536")]
        LatestTamperRestorationTamperID,

        [DescriptionAttribute("Latest Tamper Restoration Time Stamp")]
        [ParamaterCodeAttribute("3537")]
        LatestTamperRestorationTimeStamp,

        [DescriptionAttribute("Front Cover Tamper Occurance - Tamper ID")]
        [ParamaterCodeAttribute("3141")]
        FrontCoverTamperOccuranceTamperID,

        [DescriptionAttribute("Front Cover Tamper Occurance - Time Stamp")]
        [ParamaterCodeAttribute("3142")]
        FrontCoverTamperOccuranceTimeStamp,

        [DescriptionAttribute("Reverse Cumulative KWh")]
        [ParamaterCodeAttribute("3538")]
        ReverseCumulativeKWh,

        [DescriptionAttribute("Reverse Cumulative KVAh")]
        [ParamaterCodeAttribute("3539")]
        ReverseCumulativeKVAh,

        [DescriptionAttribute("R Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3543")]
        RPhaseMissingPotentialCounter,

        [DescriptionAttribute("Y Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3544")]
        YPhaseMissingPotentialCounter,

        [DescriptionAttribute("B Phase Missing Potential Counter")]
        [ParamaterCodeAttribute("3545")]
        BPhaseMissingPotentialCounter,

        [DescriptionAttribute("R Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3546")]
        RPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("Y Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3630")]
        YPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("B Phase Missing Current (Open) Counter")]
        [ParamaterCodeAttribute("3631")]
        BPhaseMissingCurrentOpenCounter,

        [DescriptionAttribute("R Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3632")]
        RPhaseCurrentReversalCounter,

        [DescriptionAttribute("Y Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3633")]
        YPhaseCurrentReversalCounter,

        [DescriptionAttribute("B Phase Current Reversal Counter")]
        [ParamaterCodeAttribute("3634")]
        BPhaseCurrentReversalCounter,

        [DescriptionAttribute("Current Short/ Bypass Counter")]
        [ParamaterCodeAttribute("3635")]
        CurrentShort_BypassCounter,

        [DescriptionAttribute("R Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3636")]
        RPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("Y Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3637")]
        YPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("B Phase Voltage Unbalance Counter")]
        [ParamaterCodeAttribute("3638")]
        BPhaseVoltageUnbalanceCounter,

        [DescriptionAttribute("R Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3639")]
        RPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("Y Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3641")]
        YPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("B Phase Current Unbalance Counter")]
        [ParamaterCodeAttribute("3642")]
        BPhaseCurrentUnbalanceCounter,

        [DescriptionAttribute("Magnetic Tamper Counter")]
        [ParamaterCodeAttribute("3643")]
        MagneticTamperCounter,

        [DescriptionAttribute("Neutral Disturbance Counter")]
        [ParamaterCodeAttribute("3644")]
        NeutralDisturbanceCounter,

        [DescriptionAttribute("Voltage Phase Sequence Reversal Counter")]
        [ParamaterCodeAttribute("3734")]
        VoltagePhaseSequenceReversalCounter,

        [DescriptionAttribute("Front Cover Open Counter")]
        [ParamaterCodeAttribute("3736")]
        FrontCoverOpenCounter,

        [DescriptionAttribute("Terminal Cover Open Counter")]
        [ParamaterCodeAttribute("3737")]
        TerminalCoverOpenCounter,

        [DescriptionAttribute("Two Phase Operation Counter")]
        [ParamaterCodeAttribute("3645")]
        TwoPhaseOperationCounter,

        [DescriptionAttribute("Instantaneous Signed Power in KW in R Phase")]
        [ParamaterCodeAttribute("3838")]
        InstantaneousSignedPowerinKWinRPhase,

        [DescriptionAttribute("Instantaneous Signed Power in KW in Y Phase")]
        [ParamaterCodeAttribute("3839")]
        InstantaneousSignedPowerinKWinYPhase,

        [DescriptionAttribute("Instantaneous Signed Power in KW in B Phase")]
        [ParamaterCodeAttribute("3841")]
        InstantaneousSignedPowerinKWinBPhase,

        [DescriptionAttribute("Maximum Demand in KW for Last Reset")]
        [ParamaterCodeAttribute("3333")]
        MaximumDemandinKWforLastReset,

        [DescriptionAttribute("Power Off hours since last reset billing period.")]
        [ParamaterCodeAttribute("3836")]
        PowerOffhourssincelastresetbillingperiod,

        [DescriptionAttribute("Cumulative Power Off Hours in R Phase")]
        [ParamaterCodeAttribute("3833")]
        CumulativePowerOffHoursinRPhase,

        [DescriptionAttribute("Cumulative Power Off Hours in Y Phase")]
        [ParamaterCodeAttribute("3834")]
        CumulativePowerOffHoursinYPhase,

        [DescriptionAttribute("Cumulative Power Off Hours in B Phase")]
        [ParamaterCodeAttribute("3835")]
        CumulativePowerOffHoursinBPhase,

        [DescriptionAttribute("LBP-Cumulative KWh reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3842")]
        LBPCumulativeKWhreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP-Cumulative KVARh reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3843")]
        LBPCumulativeKVARhreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP APF reading at the time of prior to reset")]
        [ParamaterCodeAttribute("3845")]
        LBPAPFreadingatthetimeofpriortoreset,

        [DescriptionAttribute("LBP Maximum demand in KW  at the time of prior to reset")]
        [ParamaterCodeAttribute("3844")]
        LBPMaximumdemandinKWatthetimeofpriortoreset,

        [DescriptionAttribute("Power Off hours for the last billing period")]
        [ParamaterCodeAttribute("3837")]
        PowerOffhoursforthelastbillingperiod,

        /* GKG 21/01/2013 TANGEDCO ISSUE*/
        [DescriptionAttribute("Magnetic Interference Date And Time stamp")]
        [ParamaterCodeAttribute("3846")]
        MagneticInterferenceDateTime
        /* GKG 21/01/2013 TANGEDCO ISSUE*/

    }
    #endregion

    #region Enum - High Resolution Mode Display Parameter Type Data Object
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :HighResolutionModeParameters is enumeration in HighResolutionModeParameter type
    /// 
    /// </summary>
    public enum HighResolutionModeParameters
    {
        [DescriptionAttribute("kWh")]
        [ParamaterCodeAttribute("3738")]
        kWh,
        [DescriptionAttribute("kVArh lag")]
        [ParamaterCodeAttribute("3739")]
        kVArhlag,
        [DescriptionAttribute("kVArh lead")]
        [ParamaterCodeAttribute("3741")]
        kVArhlead,
        [DescriptionAttribute("kVAh")]
        [ParamaterCodeAttribute("3742")]
        kVAh
    }
    #endregion
    
    #region Enum - DisplayTimeOuts Display Parameter Type Data Object
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :DisplayTimeOutsParameters is enumeration in DisplayTimeOutsParameter type
    /// 
    /// </summary>
    public enum DisplayTimeOutsParameters
    {
        [DescriptionAttribute("ScrollTimePerItem")]
        [ParamaterCodeAttribute("")]
        ScrollTimePerItem,
        [DescriptionAttribute("PushButtonTimeOut")]
        [ParamaterCodeAttribute("")]
        PushButtonTimeOut,
        [DescriptionAttribute("AutoScrollResumeTime")]
        [ParamaterCodeAttribute("")]
        AutoScrollResumeTime
    }
    #endregion

   
    public class ParamaterCodeAttribute : System.Attribute 
    {
        private string _value;
        public ParamaterCodeAttribute(string value) 
        {
            _value = value; 
        }
        public string ParamaterCode 
        { 
            get 
            { return _value; } 
        } 
    }
 
    public class EnumUtil
    {
        public static string StringValue(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string ParameterCode(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            ParamaterCodeAttribute[] attributes = (ParamaterCodeAttribute[])fi.GetCustomAttributes(typeof(ParamaterCodeAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].ParamaterCode;
            }
            else
            {
                return value.ToString();
            }
        }
        public static object GetValueByParamCode(string paramCode,Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (ParameterCode((Enum)Enum.Parse(enumType, name)).Equals(paramCode))
                {
                    return Enum.Parse(enumType, name);
                }
            }
            return null;

        }
    }

}
