using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Entity;
namespace CAB.BLL
{
    public class SchedulingStrategyContext
    {
        private SchedulingStrategy schedulingStrategy;
        public SchedulingStrategyContext(SchedulingStrategy strategy)
        {
            this.schedulingStrategy = strategy;
        }
        public DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity)
        {
            return schedulingStrategy.ScheduleTask(gsmTaskEntity);
        }
    }
}
