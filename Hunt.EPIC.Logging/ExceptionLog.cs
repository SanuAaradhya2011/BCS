//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.ExceptionLog.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{

    using System;
    using log4net;
    using log4net.Core;

    /// <summary>
    /// An implementation of IExceptionLog that delegates to log4net.
    /// </summary>
    public class ExceptionLog : IExceptionLog
    {

        #region General Logger

        /// <summary>
        /// An instance of IGeneralLog that is used to write to a log file before writing to the DB.
        /// </summary>
        private static readonly IGeneralLog generalLog = LogFactory.CreateGeneralLogger("ExceptionLog");

        #endregion

        #region Data Members

        /// <summary>
        /// Required as param to log4net.LoggingEvent contructor.
        /// </summary>
        private readonly static Type declaringType = typeof(ExceptionLog);

        /// <summary>
        /// A log4net logger
        /// </summary>
        private ILog log = null;

        /// <summary>
        /// The name of the logger
        /// </summary>
        private const string LOG_NAME = "ExceptionLogger";

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor gets instance of log4net logger.
        /// </summary>
        internal ExceptionLog()
        {
            this.log = LogManager.GetLogger(LOG_NAME);
        }

        #endregion

        #region IExceptionLog Members

        /// <summary>
        /// Method logs the information in the Exception contained in the param ExceptionInfo object then parks the two params
        /// into a LoggingEvent object that log4net passes to ExceptionAppender.
        /// </summary>
        /// <param name="exception"></param>
        public void Log(WriteExceptionToDBDelegate writeExceptionToDBDelegate, ExceptionInfo exceptionInfo)
        {
            generalLog.Log(LOGLEVELS.Error, exceptionInfo.Exception);
            LoggingEvent loggingEvent = new LoggingEvent(declaringType, log.Logger.Repository, log.Logger.Name, Level.Error, "", null);
            loggingEvent.Properties["exceptionInfo"] = exceptionInfo;
            loggingEvent.Properties["writeExceptionToDBDelegate"] = writeExceptionToDBDelegate;
            this.log.Logger.Log(loggingEvent);
        }

        #endregion

    }

}
