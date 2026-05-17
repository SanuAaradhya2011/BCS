using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.EntityGenerator
{
    /// <summary>
    /// Specifies  ProfileId's.
    /// </summary>
    public enum ProfileId
    {
        Instant = 1,
        Billing,
        LoadSurvey,
        Tamper,
        NamePlate,
        Midnight,
        Anomaly,
        Tou,        
        Phasor,
        FraudEnergy, 
        RTC = 101,
        SIP,
        DIP,
        BillingType,
        TOU,
        BillingReset,
        KvahSelection,
        RS232LockUnlock,
        XYZ,
        PassiveSeasonProfile,
        PassiveWeekProfile,
        PassiveDayProfile,
        ActiveSeasonProfile,
        ActiveWeekProfile,
        ActiveDayProfile,
        ActivationDate,
        PushDisplayParameter,
        ScrollDisplyParameter,
        HighResolutionDisplayParameter,
        DisplayTimeoutParameter

    }
}
