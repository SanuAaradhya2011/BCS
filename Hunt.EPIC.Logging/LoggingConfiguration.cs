using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net;

namespace Hunt.EPIC.Logging
{
    public static class LoggingConfiguration
    {
        public static void ChangeRootLogLevel(LOGLEVELS logLevel)
        {
            IGeneralLog generalLog = LogFactory.CreateGeneralLogger("LoggingConfiguration");
            Level log4NetLevel;
            switch (logLevel)
            {
                case LOGLEVELS.Off:
                    log4NetLevel = Level.Off;
                    break;
                case LOGLEVELS.Info:
                    log4NetLevel = Level.Info;
                    break;
                case LOGLEVELS.Warn:
                    log4NetLevel = Level.Warn;
                    break;
                case LOGLEVELS.Error:
                    log4NetLevel = Level.Error;
                    break;
                case LOGLEVELS.Fatal:
                    log4NetLevel = Level.Fatal;
                    break;
                case LOGLEVELS.Debug:
                    log4NetLevel = Level.Debug;
                    break;
                default:
                    log4NetLevel = Level.Debug;
                    break;
            }
            GetRootLogger().Level = log4NetLevel;
            generalLog.Log(logLevel, "Changing Root Log Level to " + logLevel.ToString());
        }

        public static LOGLEVELS GetRootLogLevel()
        {
            IGeneralLog generalLog = LogFactory.CreateGeneralLogger("LoggingConfiguration");
            Level log4NetLevel = GetRootLogger().Level;
            if (log4NetLevel == Level.Off)
            {
                return LOGLEVELS.Off;
            }
            else if (log4NetLevel == Level.Error)
            {
                return LOGLEVELS.Error;
            }
            else if (log4NetLevel == Level.Warn)
            {
                return LOGLEVELS.Warn;
            }
            else if (log4NetLevel == Level.Info)
            {
                return LOGLEVELS.Info;
            }
            else if (log4NetLevel == Level.Fatal)
            {
                return LOGLEVELS.Fatal;
            }
            else if (log4NetLevel == Level.Debug)
            {
                return LOGLEVELS.Debug; //Default to Debug if we don't match the values above. 
            }
            else //log level we do not support is present in the file, log a warning and set to debug
            {
                ChangeRootLogLevel(LOGLEVELS.Debug);
                StringBuilder logLevelError = new StringBuilder();
                logLevelError.Append("The configured log level ");
                logLevelError.Append(log4NetLevel.ToString());
                logLevelError.Append(" is not supported by this system. Modify the configuration file to use one of the following log levels:\n");
                foreach (string item in Enum.GetNames(typeof(LOGLEVELS)))
                {
                    logLevelError.Append(item + "\n");
                }
                generalLog.Log(LOGLEVELS.Debug, logLevelError);
                return LOGLEVELS.Debug;
            }
        }

        private static Logger GetRootLogger()
        {
            Hierarchy h = (Hierarchy)LogManager.GetRepository();
            return h.Root;
        }
    }
}
