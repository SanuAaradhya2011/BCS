//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.WriteExceptionToDBDelegate.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using System;

    /// <summary>
    /// A delegate that is accepted as a param by IExceptionLog.Log and used by ExceptionAppender to write 
    /// to the CentralServicess.ErrorLog table.
    /// </summary>
    /// <param name="connectString"></param>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="collectorId"></param>
    /// <param name="endpointId"></param>
    /// <param name="errorTypeId"></param>
    /// <param name="organizationId"></param>
    /// <param name="userId"></param>
    public delegate void WriteExceptionToDBDelegate(ExceptionInfo exceptionInfo);

}
