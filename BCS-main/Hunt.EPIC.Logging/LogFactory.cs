//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.LogFactory.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using System;

    /// <summary>
    /// LogFactory provides static methods that return IGeneralLog or IExceptionLog implementations.
    /// </summary>
    public class LogFactory
    {

        #region Constructor

        /// <summary>
        /// Private constructor to prevent object creation
        /// </summary>
        private LogFactory() { }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Method returns an instance of the custom Exception logger.  That logger is a wrapper for a custom log4net appender
        /// the inserts error messages into the ErrorLog table in the CentralServices database.
        /// </summary>
        /// <returns></returns>
        public static IExceptionLog CreateExceptionLogger()
        {
            return new ExceptionLog();
        }

        /// <summary>
        /// Method returns an instance of GeneralLog which is a wrapper for a named log4net logger.
        /// </summary>
        /// <param name="name">The name of the logger to be returned</param>
        /// <returns>Instance of GeneralLog</returns>
        public static IGeneralLog CreateGeneralLogger(string name)
        {
            return new GeneralLog(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILoggerExceptionLog CreateLoggerExceptionLogger(string name)
        {
            return new LoggerExceptionLog(name);
        }

        #endregion

    }
}
