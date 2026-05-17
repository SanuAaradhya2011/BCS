using System;
using System.ComponentModel;
using System.Reflection;

namespace CAB.Framework
{
    /// <summary>
    /// Enum for various communication types
    /// </summary>
    public enum CommunicationType
    {
        DIRECT,
        GSM,
        PSTN,
        GPRS,
        TCP,
        FTP
    }

    /// <summary>
    /// Enum for different connection mode of GPRS Modem
    /// </summary>
    public enum GPRSConnectionMode
    {   
        Periodic, 
        AlwaysOn,
        Scheduled
        
    }

    /// <summary>
    /// Enum for different types of IP types possible 
    /// </summary>
    public enum GPRSIPType
    {
       
        Static,
        Dynamic
        
    }

    /// <summary>
    /// Enum for meter models 
    /// </summary>
    public enum MeterModels
    {
     [Description("E250 - WCM")]  
      RubyE250=1,
     [Description("E650 - HT")]  
      PumaLTE650=2,
     [Description("E650 - LT")]  
      PumaHTE650=3,
      [Description("E350-SM310")]// smart meter added
     SmartWCM670 = 4,
     [Description("E670-SM405")]
     SmartLTCT670 = 5,
     [Description("E350 - SM110")]
      SM110Val = 6
    }

    /// <summary>
    /// Enum for different type of meters
    /// </summary>
    public enum MeterTypes
    {
        [Description("3P-3W")]
        ThreePhase3W = 1,
        [Description("3P-4W")]
        ThreePhase4W = 2,
        [Description("1P-2W")]
        SinglePhase2W = 3

    }


}
