using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Core;

namespace Hunt.EPIC.Logging
{
    class LoggerExceptionLog : ILoggerExceptionLog
    {
        #region Attributes

        private ILog log = null;
        private string name;

        #endregion

        #region Constructors

        internal LoggerExceptionLog(string name)
        {
            log4net.Config.XmlConfigurator.Configure();

            this.name = name;
            this.log = LogManager.GetLogger(this.name);
        }

        #endregion

        #region Methods

        public void Log(ILoggerException ex)
        {
            switch (ex.LogLevel)
            {
                default:
                case LOGLEVELS.Debug:
                    this.log.Debug(ex.GetMessage(MessageScope.InternalLogging));
                    break;
                case LOGLEVELS.Info:
                    this.log.Info(ex.GetMessage(MessageScope.PublicLogging));
                    break;
                case LOGLEVELS.Warn:
                    this.log.Warn(ex.GetMessage(MessageScope.PublicLogging));
                    break;
                case LOGLEVELS.Error:
                    this.log.Error(ex.GetMessage(MessageScope.PublicLogging));
                    break;
                case LOGLEVELS.Fatal:
                    this.log.Fatal(ex.GetMessage(MessageScope.PublicLogging));
                    break;
            }
        }

        public void Log(List<ILoggerException> exceptions) 
        {
            foreach (ILoggerException ex in exceptions)
            {
                Log(ex);
            }
        }

        #endregion
    }
}
