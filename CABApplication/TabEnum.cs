#region NameSpaces
using System.ComponentModel;
#endregion

namespace CABApplication
{
    /// <summary>
    /// this enum contains all tab information used to hide and show tabs of Analysis Report.
    /// </summary>
    public enum TabEnum
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("General")]
        Gen = 0x0,
        /// <summary>
        /// 
        /// </summary>
        [Description("Instant : Reading")]
        InsRea = 0x1,
        /// <summary>
        /// 
        /// </summary>
        [Description("Instant : Self Diagnostics")]
        InsSel = 0x2,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Energy : Main Energy")]
        BilEneMai = 0x3,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Energy : Consumption")]
        BilEneCon = 0x4,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Energy : TOD Energy")]
        BilEneTodEne = 0x5,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Energy : TOD Consumption")]
        BilEneTodCon = 0x6,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Demand : Maximum Demand")]
        BilDemMax = 0x7,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Demand : TOD MD")]
        BilDemTod = 0x8,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Power Off Duration")]
        BilDemPowOff = 0x9,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Power Factor")]
        BilDemPowFac = 0x10,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Miscellaneous")]
        BilMis = 0x11,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : TOU Configuration")]
        BilTouCon = 0x12,
        /// <summary>
        /// 
        /// </summary>
        [Description("Tamper")]
        Tam = 0x13,
        /// <summary>
        /// 
        /// </summary>
        [Description("Load Survey")]
        LoaSur = 0x14,
        /// <summary>
        /// 
        /// </summary>
        [Description("MidNight Data")]
        MidDat = 0x15,
        /// <summary>
        /// 
        /// </summary>
        [Description("Transaction")]
        Tra = 0x16,
        /// <summary>
        /// 
        /// </summary>
        [Description("Phasor")]
        Pha = 0x17,
        /// <summary>
        /// 
        /// </summary>
        [Description("MidNight Energies")]
        MidEne = 0x18,
        /// <summary>
        /// 
        /// </summary>
        DaiEneCon = 0x19,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration")]
        MtrCfg = 0x20,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : MDWithIP")]
        MDWithIP = 0x21,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Kvar Selection")]
        KvarSel = 0x22,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : RS232")]
        RS232 = 0x23,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Billing Type")]
        BillTyp = 0x24,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : RTC")]
        RTC = 0x25,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Auto Lock")]
        AutoLck = 0x26,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Display Parameters")]
        DspPar = 0x27,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Daily Log")]
        DaiLog = 0x28,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : TOD")]
        TOD = 0x29,
        /// <summary>
        /// 
        /// </summary>
        [Description("Fraud Energy")]
        FraEne = 0x30,
        /// <summary>
        /// 
        /// </summary>
        [Description("CTRatio")]
        CTRatio = 0x31,
        /// <summary>
        /// 
        /// </summary>
        [Description("PTRatio")]
        PTRatio = 0x32,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : LSIP")]
        LSIP = 0x33,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : DIP")]
        DIP = 0x34,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Power On Duration")]
        BilDemPowOn = 0x35,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Average Load Factor")]
        BilDemAvgLoaFac = 0x36,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Manual Billing")]
        ManBil = 0x37,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Software Billing")]
        SofBil = 0x38,
        /// <summary>
        /// 
        /// </summary>
        [Description("NamePlate Profile")]
        Nam = 0x39,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Disconnect Control")]
        DisCon = 0x40,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Load Control")]
        LoaCon = 0x41,
        /// <summary>
        /// 
        /// </summary>
       [Description("Meter Configuration : Load Control Single Phase")]
        LoaCon1P = 0x42,
         /// <summary>
        /// 
        /// </summary>
       [Description("Meter Configuration : RS 485")]
        RS485 = 0x43,
     

        [Description("Instant : ABC Code")]
        InsABC = 0x44,

        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Average Load")]
        BilDemAvgLoad = 0x45,

        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Payment Mode")]
        PaymentMode = 0x46,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Metering Mode")]
        MeteringMode = 0x47,

        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Load Limit")]
        LoadLimit = 0x48,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : SlidingDemand")]
        SlidingDemand = 0x49,
        /// <summary>
        /// 
        /// </summary>
        [Description("Meter Configuration : Port Config")]
        OpticalRJPortLock = 0x50,
        /// <summary>
        /// 
        /// </summary>
        [Description("Billing Parameters : Demand : Cumulative MD")]
        BillCumulativeMD = 0x51,

        [Description("Load Switch")]
        LoadSwitch = 0x52,

        [Description("Meter Configuration : PulseEnergy")]
        PulseEnergy = 0x53,

		[Description("Billing Parameters :TOD Power Factor")]//1024441
        BilTouAvgPowFac = 0x54,

        [Description("Meter Configuration : Manual Button MD Reset")]
        ManualMDReset = 0x55,
    };

}
