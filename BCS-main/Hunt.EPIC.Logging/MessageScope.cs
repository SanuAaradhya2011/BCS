using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt.EPIC.Logging
{
    /// <summary>
    /// An enumeration that is used by the ILoggerException interface to determine what level of detail
    /// is needed in a message that is to be logged. ClientResponse is probably a GSIS-specific option
    /// that is used to format reply values.
    /// </summary>
    public enum MessageScope
    {
        PublicLogging,
        InternalLogging,
        ClientResponse
    }
}
