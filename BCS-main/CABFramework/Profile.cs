using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAB.Framework
{
    
    /// <summary>
    /// enum for different reading profiles
    /// </summary>
    public enum Profile
        {
        [Description("General")] 
        GENERAL=0,
        [Description("Instantaneous")]
        INSTANTANEOUS=1,
        [Description("Billing")]
        BILLING=2,
        //[Description("Load Survey")]
        //LOADSURVEY=3,
        [Description("Tamper")]
        TAMPER=3

        }


    /// <summary>
    /// Classs to contain report configuration parameters
    /// </summary>
    public class ReportConfigurationParameters
    {
        public string[] Meters { get; set; }
        public string[] ScheduleIds { get; set; }
        public ReportType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    /// <summary>
    /// Enum for different type of reports
    /// </summary>
    public enum ReportType
    {
        RUNNING,
        METERWISE,
        SCHEDULEWISE
    }

    
}
