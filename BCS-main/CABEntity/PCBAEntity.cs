using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CABEntity
{
    public enum PCBATypes
    {
        [DescriptionAttribute("RTC Failure")]
        RTCFailure,
        
        [DescriptionAttribute("NVM Failure")]
        NVMFailure
    }
    public enum PCBAStatus
    {
        [DescriptionAttribute("Fail")]
        Fail,
        [DescriptionAttribute("Working")]
        Ok
    }
    
}
