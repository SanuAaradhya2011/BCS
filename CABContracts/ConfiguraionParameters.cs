using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAB.Contracts
{
    public enum ConfigurationParameter
    {
        [DescriptionAttribute("MD With IP")]
        MDWithIP,

        [DescriptionAttribute("kVAh Selection")]
        KVAHSelection,

        [DescriptionAttribute("Display Parameters")]
        DisplayParameters,

        [DescriptionAttribute("TOD")]
        TOD,

        [DescriptionAttribute("RTC")]
        RTC,

        [DescriptionAttribute("Billing Reset")]
        BillingReset,

        [DescriptionAttribute("Resets")]
        Resets,

        [DescriptionAttribute("Daily Log")]
        DailyLog,

        [DescriptionAttribute("Mode Of Billing")]
        ModeOfBilling,

        [DescriptionAttribute("Billing Period")]
        BillingPeriod,

        [DescriptionAttribute("Reset Lockout Days")]
        ResetLockOutDays,

        [DescriptionAttribute("Lock Unlock RS232")]
        LockUnlockRS232,

        [DescriptionAttribute("TODSP")]
        TODSP,

        [DescriptionAttribute("None")]
        None
    }
}
