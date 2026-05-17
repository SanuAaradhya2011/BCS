using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Framework;
namespace CAB.BLL
{
    public abstract class SchedulingStrategy
    {
        private const int DAILYTASKPRIORIY = 4;
        private const int WEEKLYTASKPRIORITY = 3;
        private const int MONTHLYTASKPRIORITY = 2;
        private const int ONETIMEONLYTASKPRIORITY = 1;

        public abstract DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity);

        public int GetPriority(GSMTasksType gsmTasksType)
        {
            switch (gsmTasksType)
            {
                case GSMTasksType.Daily:
                    return DAILYTASKPRIORIY;
                    break;
                case GSMTasksType.Monthly:
                    return MONTHLYTASKPRIORITY;
                    break;
                case GSMTasksType.Weekly:
                    return WEEKLYTASKPRIORITY;
                    break;
                case GSMTasksType.OneTimeOnly:
                    return ONETIMEONLYTASKPRIORITY;
                    break;
                default:
                    return ONETIMEONLYTASKPRIORITY;
            }
        }
    }
}
