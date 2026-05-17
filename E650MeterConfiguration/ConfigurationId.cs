using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.E650MeterConfiguration
{
    /// <summary>
        /// Specifies the communication type.
        /// </summary>
        public enum ConfigurationId
        {
            RTC = 101,
            SIP,
            DIP,
            BillingType,
            TOU,
            BillingReset           

        }    
}
