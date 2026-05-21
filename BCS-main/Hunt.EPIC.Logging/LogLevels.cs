using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt.EPIC.Logging
{
    /// <summary>
    /// An enumeration aligned with the values given in log4net.Core.Level
    /// </summary>
    public enum LOGLEVELS
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        Fatal = 4,
        Off = 5
    };

}
