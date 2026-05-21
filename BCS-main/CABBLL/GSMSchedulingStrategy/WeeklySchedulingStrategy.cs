using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Entity;
namespace CAB.BLL
{
    public class WeeklySchedulingStrategy : SchedulingStrategy
    {
        private int counter = 0;
        private bool dateSet = false;
        private StringBuilder strWeekDays = new StringBuilder();
        private int[] weekDayList;
        private DateTime startDate;
        private int year = DateTime.Now.Year;
        private int dayOfWeek=0;    
        #region ISchedulingStrategy Members

        public override DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity)
        {
            gsmTaskEntity.taskType = GSMTasksType.Weekly.ToString();
            gsmTaskEntity.taskPriority = GetPriority(GSMTasksType.Weekly);
            dayOfWeek = GetDayNo(DateTime.Now.DayOfWeek.ToString());
            weekDayList =  gsmTaskEntity.WeekDayList;
            startDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0));
            // make start date current week's monday
            startDate = startDate.AddDays(1 - dayOfWeek);
            //check if it is greater than current date
            while (DateTime.Now.AddMinutes(5) >= startDate)
            {
                if (!dateSet && counter > 0)
                    startDate = startDate.AddDays(7);
                foreach (int dayNo in weekDayList)
                {
                    if (dayNo > 0)
                    {
                        //check if the selected dayno is greater than current date
                        if (DateTime.Now.AddMinutes(5) < startDate.AddDays(dayNo - 1))
                        {
                            dateSet = true;
                            startDate = startDate.AddDays(dayNo - 1);
                            break;
                        }
                    }
                }
                //if any of the week day is not gretaer than current date increament a week.
                counter++;
            }
            return startDate;
        }

        #endregion
        private int GetDayNo(string currentDay)
        {
            int dayNo = 0;
            switch (currentDay.ToLower().Trim())
            {
                case GSMSchedulingConstants.MONDAY:
                    dayNo = 1;
                    break;
                case GSMSchedulingConstants.TUESDAY:
                    dayNo = 2;
                    break;
                case GSMSchedulingConstants.WEDNESDAY:
                    dayNo = 3;
                    break;
                case GSMSchedulingConstants.THURSDAY:
                    dayNo = 4;
                    break;
                case GSMSchedulingConstants.FRIDAY:
                    dayNo = 5;
                    break;
                case GSMSchedulingConstants.SATURDAY:
                    dayNo = 6;
                    break;
                case GSMSchedulingConstants.SUNDAY:
                    dayNo = 7;
                    break;
                default:
                    dayNo = 0;
                    break;
            }
            return dayNo;
        }
    }
}
