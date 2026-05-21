//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.ExceptionInfo.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using System;

    /// <summary>
    /// A class to hold information related to exceptions in a form that can be passed to the ExceptionLog.Log method.
    /// The class requires an Exception and a string param, along with five int pararms that are allowed to be null.
    /// Any of the values may be set either in a constructor or via properties.
    /// </summary>
    public class ExceptionInfo
    {
        #region Data Members and Properties



        private int errorTypeId;
        public int ErrorTypeId
        {
            get { return this.errorTypeId; }
            set { this.errorTypeId = value; }
        }

        private Exception exception = null;
        public Exception Exception
        {
            get { return this.exception; }
            set { this.exception = value; }
        }

        private string description = null;
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        private int organizationId;
        public int OrganizationId
        {
            get { return this.organizationId; }
            set { this.organizationId = value; }
        }

        private int userId;
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        private DateTime logTime;
        public DateTime LogTime
        {
            get { return this.logTime; }
            set { this.logTime = value; }
        }

        #endregion

        #region optional attributes
        private int? collectorId = null;
        public int? CollectorId
        {
            get { return this.collectorId; }
            set { this.collectorId = value; }
        }

        private int? endpointId = null;
        public int? EndpointId
        {
            get { return this.endpointId; }
            set { this.endpointId = value; }
        }
        private string serializedMessage = null;
        public string SerializedMessage
        {
            get { return this.serializedMessage; }
            set { this.serializedMessage = value; }
        }
        private string source = null;
        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }
        private string target = null;
        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }
        #endregion

        #region Constructors

        public ExceptionInfo(
            Exception exception,
            int errorTypeId,
            string description,
            int organizationId,
            int userId,
            DateTime logTime)
            : this(exception, errorTypeId, description, organizationId, userId, logTime, null, null, null, null, null) { }

        public ExceptionInfo(
            Exception exception,
            int errorTypeId,
            string description,
            int organizationId,
            int userId,
            DateTime logTime,
            int? collectorId,
            int? endpointId,
            string serializedMessage,
            string source,
            string target)
        {
            this.exception = exception;
            this.errorTypeId = errorTypeId;
            this.description = description;
            this.organizationId = organizationId;
            this.userId = userId;
            this.logTime = logTime;
            this.collectorId = collectorId;
            this.endpointId = endpointId;
            this.serializedMessage = serializedMessage;
            this.source = source;
            this.target = target;
        }

        #endregion

    }
}
