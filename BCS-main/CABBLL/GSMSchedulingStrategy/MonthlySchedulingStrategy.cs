using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Entity;
namespace CAB.BLL
{
    public class MonthlySchedulingStrategy : SchedulingStrategy 
    {
        private StringBuilder strMonths = new StringBuilder();
        private string strMonth = string.Empty;
        private int dayOfMonth = 0;
        private int currentMonthNo = 0;
        private int[] monthNoList;
        private DateTime startDate;
        private int year = DateTime.Now.Year;

        #region ISchedulingStrategy Members
        public override DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity)
        {
            gsmTaskEntity.taskPriority = GetPriority(GSMTasksType.Monthly);
            //Set the Task Type to Monthly
            gsmTaskEntity.taskType = GSMTasksType.Monthly.ToString();
            dayOfMonth = Convert.ToInt32(gsmTaskEntity.IntervalInDays);
            currentMonthNo = DateTime.Now.Month;
            monthNoList = gsmTaskEntity.MonthNoList;
            startDate = (new DateTime(year, 1, dayOfMonth, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0));
            //increament with a month and if months are not enough increament a year and assign startdate untill it is greater than current date
            while (DateTime.Now.AddMinutes(5) >= startDate)
            {
                startDate = (new DateTime(year, 1, dayOfMonth, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0));
                foreach (int monthNo in monthNoList)
                {
                    if (monthNo > 0)
                    {
                        //compare by adding months with the current date
                        if (DateTime.Now.AddMinutes(5) < startDate.AddMonths(monthNo - 1))
                        {
                            //if ok, then actually add months and return
                            startDate = startDate.AddMonths(monthNo - 1);
                            break;
                        }
                    }
                }
                year = year + 1;
            }
            return startDate;
        }

        #endregion
    }
}
