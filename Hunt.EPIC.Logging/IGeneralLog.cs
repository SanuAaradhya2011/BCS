//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.IGeneralLog.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using System;

    /// <summary>
    /// Public interface to the general logging system.  This is implemented by GeneralLog which in turn
    /// delegates all calls to log4net.
    /// </summary>
    public interface IGeneralLog
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        void Log(LOGLEVELS logLevel, object message);
        void Log(LOGLEVELS logLevel, object message, Exception exception);
    }

}