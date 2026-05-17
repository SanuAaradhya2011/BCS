//-------------------------------------------------------------------------------------------
// <copyright file="Hunt.EPIC.Logging.GeneralLog.cs" company="Hunt Technologies, LLC.">
//     Copyright (c) Hunt Technologies, LLC.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------
namespace Hunt.EPIC.Logging
{
    using log4net;
    using System.Text;
    using log4net.Core;

    /// <summary>
    /// An implementation of IGeneralLog that delegates all calls to log4net.
    /// </summary>
    public class GeneralLog : IGeneralLog
    {
        #region Data Members

        private ILog log = null;
        private string name;

        #endregion

        #region Is...Enabled Properties

        public bool IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        #endregion

        #region Constructors

        internal GeneralLog(string name)
        {
            this.name = name;
            this.log = LogManager.GetLogger(this.name);
        }

        #endregion

        #region IGeneralLog Members

        public void Log(LOGLEVELS logLevel, object message)
        {
            switch(logLevel)
            {
                case LOGLEVELS.Info:
                    this.log.Info(message);
                    break;
                case LOGLEVELS.Warn:
                    this.log.Warn(message);
                    break;
                case LOGLEVELS.Error:
                    this.log.Error(message);
                    break;
                case LOGLEVELS.Fatal:
                    this.log.Fatal(message);
                    break;
                // default is LOGLEVELS.Debug
                default:
                    this.log.Debug(message);
                    break;
            }
        }

        public void Log(LOGLEVELS logLevel, object message, System.Exception exception)
        {
            switch(logLevel)
            {
                case LOGLEVELS.Info:
                    this.log.Info(message, exception);
                    break;
                case LOGLEVELS.Warn:
                    this.log.Warn(message, exception);
                    break;
                case LOGLEVELS.Error:
                    this.log.Error(message, exception);
                    break;
                case LOGLEVELS.Fatal:
                    this.log.Fatal(message, exception);
                    break;
                // default is LOGLEVELS.Debug
                default:
                    this.log.Debug(message, exception);
                    break;
            }
        }
        #endregion
    
    }

}
