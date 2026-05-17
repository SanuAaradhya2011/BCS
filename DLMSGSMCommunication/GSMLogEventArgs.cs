using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
namespace DLMSGSMCommunication
{
    public class GSMLogEventArgs : EventArgs
    {
        private int log_ID;
        private bool isGeneralCompleted;
        private bool isInstantCompleted;
        private bool isBillingCompleted;
        private int retries;
        private GSMLoggingEntity gsmLoggingEntity;
        public GSMLoggingEntity GSMLog
        {
            get
            {
                return gsmLoggingEntity;
            }
            set
            {
                gsmLoggingEntity = value;
            }
        }
        public int Log_ID
        {
            get
            {
                return log_ID;
            }
            set
            {
                log_ID = value;
            }
        }
        public bool IsGeneralCompleted
        {
            get
            {
                return isGeneralCompleted;
            }
            set
            {
                isGeneralCompleted = value;
            }
        }
        public bool IsInstantCompleted
        {
            get
            {
                return isInstantCompleted;
            }
            set
            {
                isInstantCompleted = value;
            }
        }
        public bool IsBillingCompleted
        {
            get
            {
                return isBillingCompleted;
            }
            set
            {
                isBillingCompleted = value;
            }
        }
        public int Retries
        {
            get
            {
                return retries;
            }
            set
            {
                retries = value;
            }
        }
    }
}
