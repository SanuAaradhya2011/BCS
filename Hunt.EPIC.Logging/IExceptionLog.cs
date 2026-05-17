//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.IExceptionLog.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{

    /// <summary>
    /// Interface defines a Log method that use the WriteExceptionToDBDelegate delegate to 
    /// insert exception information into the ErrorLog table in the CentralServices database.
    /// </summary>
    public interface IExceptionLog
    {
        /// <summary>
        /// Method accepts a WriteExceptionToDBDelegate and an ExceptionInfo param.  The former is used
        /// to indirectly write to the CentralServices.ErrorLog table; the latter contains the information to write there.
        /// 
        /// </summary>
        /// <param name="writeExceptionToDBDelegate"></param>
        /// <param name="exception"></param>
        void Log(WriteExceptionToDBDelegate dbWriteDelegate, ExceptionInfo exceptionInfo);
    }

}
