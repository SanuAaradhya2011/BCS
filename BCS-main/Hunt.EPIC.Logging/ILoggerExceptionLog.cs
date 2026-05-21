using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt.EPIC.Logging
{

    /// <summary>
    /// A logger that works with exceptions (rather than just random strings) to ensure
    /// consistency in message format for all logged messages. It relies on custom exception
    /// classes that implement the ILoggerException interface.
    /// </summary>
    public interface ILoggerExceptionLog
    {
        void Log(ILoggerException ex);
        void Log(List<ILoggerException> exceptions);
    }

}
