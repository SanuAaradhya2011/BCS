//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.ExceptionAppender.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// A custom appender that uses an instance of WriteExceptionToDBDelegate to insert exception reports into the 
    /// ErrorLog table in the CentralServices database.  The information to be written is held in an instance of ExceptionInfo.
    /// </summary>
    public class ExceptionAppender : AppenderSkeleton
    {
         
        #region Constructor

        /// <summary>
        /// An internal constructor
        /// </summary>
        public ExceptionAppender() { }

        #endregion

        #region Overrides

        /// <summary>
        /// Override of method in AppenderSkeleton that is called by by log4net when something hits ExceptionLog.Log.  
        /// The data held in the instance of ExceptionInfo held in the LoggingEvent.Properties is passed to the 
        /// delegate that is also held in the LoggingEvent.Properties.
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            WriteExceptionToDBDelegate writeExceptionToDBDelegate = 
                (WriteExceptionToDBDelegate)loggingEvent.Properties["writeExceptionToDBDelegate"];
            ExceptionInfo exceptionInfo = loggingEvent.Properties["exceptionInfo"] as ExceptionInfo;
            if (writeExceptionToDBDelegate != null)
            {
                writeExceptionToDBDelegate(exceptionInfo);
            }
        }

        #endregion

    }

}
