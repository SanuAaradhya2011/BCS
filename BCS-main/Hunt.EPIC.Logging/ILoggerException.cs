using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt.EPIC.Logging
{
    /// <summary>
    /// An interface that should be implemented by custom exception classes. Its purpose is to provide
    /// a consistent message format for all logged messages.
    /// </summary>
    public interface ILoggerException
    {
        LOGLEVELS LogLevel { get; set; }
        string GetMessage();
        string GetMessage(MessageScope scope);
        string ToString();
    }
}
