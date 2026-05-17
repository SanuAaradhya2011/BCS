using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CABFramework
{
    /// <summary>
    /// this enum contains various communication types.
    /// </summary>
    public enum CommTypes
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Unknown")]
        Unknown = 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("Direct")]
        Direct = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("GSM")]
        GSM = 2,
        /// <summary>
        /// 
        /// </summary>
        [Description("PSTN")]
        PSTN = 3,
        /// <summary>
        /// 
        /// </summary>
        [Description("GPRS")]
        GPRS = 4,
        /// <summary>
        /// 
        /// </summary>
        [Description("Upload")]
        Upload = 5,
        /// <summary>
        /// 
        /// </summary>
        [Description("Restore")]
        Restore = 6,
        /// <summary>
        /// 
        /// </summary>
        [Description("Scheduling")]
        Scheduling = 7,
        /// <summary>
        /// 
        /// </summary>
        [Description("Import")]
        Import = 8,
        /// <summary>
        /// 
        /// </summary>
        [Description("CMRI")]
        CMRI = 9,
        /// <summary>
        /// 
        /// </summary>
        [Description("Previous Version Upload")]
        PreviousVersionUpload = 10,


        [Description("TCP")]
        TCP = 11,


        [Description("FTP")]
        FTP = 12

    };    
}
