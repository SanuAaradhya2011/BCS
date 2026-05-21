using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSComService
{
    class Constants
    {
        public const string msgTaskTimedOut = "Read out failed. Connection with meter is not available.";
        public const string msgTaskCompletedSuccessfully = "{0} Profile execution is completed.";
        public const string msgTaskStatusChangedToInProgress = "{0} Profile read out is in progress.";
        public const string msgTaskFileUploading = "{0} data upload in progress.";
        public const string msgTaskFailedDuringFileUploading = "Error while uploading {0} data. Task execution failed.";


        public const string constTaskExpiryTimeOutMinutes = "TaskExpiryTimeOutMinutes";
        public const string EndPointSyncSleepDurationKey = "EndPointSyncSleepDuration";
        public const Int32  EndPointSyncSleepDuration = 60 * 10 ; // 10 minutes
        public const int nServerSAP = 1;
        public const int nClientSAP = 32;
        public const int nServerLowerMacAddress = 256;
        public const byte nSecurityMechanism = 1;
        public const string nPassword = "11111111";
        public const string HLSKey = "F26CE01A15BD16CBB03B473F65E3BFBA";

        //SET TIMEOUT CONTANTS

        public const string constCommandProcessorTimeOut = "CommandProcessorIdleTimeout";
        public const string constFileuploaderTimeOut = "FileUploaderIdleTimeout";
        public const string constNewTaskWorkerIdleTimeout = "NewTaskWorkerIdleTimeout";
        public const string constProcessTaskWorkerIdleTimeout = "ProcessTaskWorkerIdleTimeout";
        public const string constResponseProcessorIdleTimeout = "ResponseProcessorIdleTimeout";

        //Thread count configuration constants
        public const string constProcessTaskThread = "maxProcessTaskWorkerThread";
        public const string constResponseProcessorThread = "maxResponseProcessorThread";
        public const string constCommandProcessorThread = "maxCommandProcessorThread";
        public const string constFileUploadProcessorThread = "maxFileUploadProcessorThread";

        public const string constCommandExpiryTimeOut = "CommandExpiryTimeOut";

        public const string WaitIntervalBeforeAbort = "WaitIntervalBeforeAbort";
        public const string constMaxLoadSurveyDays = "MaxLoadSurveyDays";

        //pick the values from config.
        public static string GetConfigValue(string key)
        {
            try
            {
                return System.Configuration.ConfigurationSettings.AppSettings[key].ToString();
            }
            catch 
            {
                return string.Empty;
            }
        }
    }

}
