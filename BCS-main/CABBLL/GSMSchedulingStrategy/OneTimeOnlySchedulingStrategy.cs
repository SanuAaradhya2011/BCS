using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Entity;
namespace CAB.BLL
{
    public class OneTimeOnlySchedulingStrategy : SchedulingStrategy
    {
        #region ISchedulingStrategy Members

        public override DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity)
        {
            gsmTaskEntity.taskType = EnumUtil.stringValueOf(GSMTasksType.OneTimeOnly);
            gsmTaskEntity.taskPriority = GetPriority(GSMTasksType.OneTimeOnly);
            return gsmTaskEntity.CalendarDate;
        }

        #endregion
    }
}
