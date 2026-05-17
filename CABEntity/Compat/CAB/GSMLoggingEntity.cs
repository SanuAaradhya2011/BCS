using LNG.Framework.Entity;
using System;

namespace LNG.Entity
{
    public class GSMLoggingEntity:EntityBase
    {
        private int log_ID;
        private int task_ID;
        private int group_ID;
        private string meter_ID;
        private int isGeneralCompleted;
        private int isInstantCompleted;
        private int isBillingCompleted;
        private int isLoadSurveyCompleted;
        private int isTamperCompleted;
        private int isMidNightCompleted;
        private int isMeterConfigCompleted;
        private string status;
        private int taskRetries;
        private DateTime creationDateTime;
        private string errorMessage;
        //holds the value of anomaly profile completion status
        public bool IsAnomalyCompleted { get; set; }
        //holds the value of tou completion status
        public bool IsTOUCompleted { get; set; }
        //holds the value of DIP completion status
        public bool IsDIPCompleted { get; set; }
        //holds the value of meter ID completed
        public bool IsMeterIDCompleted { get; set; }
        
        public  int Log_ID 
        {
            get { return log_ID; }
            set { log_ID = value; }
        }
        public int Task_ID
        {
            get { return task_ID; }
            set { task_ID = value; }
        }
        public int Group_ID
        {
            get { return group_ID; }
            set { group_ID = value; }
        }
        public string Meter_ID
        {
            get
            {
                return meter_ID;
            }
            set
            {
                meter_ID = value;
            }
        }
        public bool IsGeneralCompleted
        {
            get
            {
                if (isGeneralCompleted == 1)
                    return true;
                else
                   return false;
            }
            set
            {
                if (value == true)
                    isGeneralCompleted = 1;
                else
                    isGeneralCompleted = 0;
            }
        }
        public bool IsInstantCompleted
        {
            get
            {
                if (isInstantCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isInstantCompleted = 1;
                else
                    isInstantCompleted = 0;
            }
        }
        public bool IsBillingCompleted
        {
            get
            {
                if (isBillingCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isBillingCompleted = 1;
                else
                    isBillingCompleted = 0;
            }
        }
        /// <summary>
        /// gets or sets the load survey completed.
        /// </summary>
        public bool IsLoadSurveyCompleted
        {
            get
            {
                if (isLoadSurveyCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isLoadSurveyCompleted = 1;
                else
                    isLoadSurveyCompleted = 0;
            }
        }
        /// <summary>
        /// gets or sets the tamper completed boolean
        /// </summary>
        public bool IsTamperCompleted
        {
            get
            {
                if (isTamperCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isTamperCompleted = 1;
                else
                    isTamperCompleted = 0;
            }
        }
        /// <summary>
        /// gets or sets the midnight completed boolean
        /// </summary>
        public bool IsMidNightCompleted
        {
            get
            {
                if (isMidNightCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isMidNightCompleted = 1;
                else
                    isMidNightCompleted = 0;
            }
        }

        public bool IsMeterConfigurationCompleted
        {
            get
            {
                if (isMeterConfigCompleted == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    isMeterConfigCompleted = 1;
                else
                    isMeterConfigCompleted = 0;
            }
        }


        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// This property contains retries used while communication
        /// </summary>
        public int Retries
        {
            get { return taskRetries; }
            set { taskRetries = value; }
        }
        public DateTime CreationDateTime
        {
            get { return creationDateTime; }
            set { creationDateTime = value; }
        }
        
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }
}
