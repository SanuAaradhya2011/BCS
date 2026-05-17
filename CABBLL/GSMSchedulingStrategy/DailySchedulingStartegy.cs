using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Framework;
namespace CAB.BLL
{
    /// <summary>
    /// The class will schedule daily task
    /// </summary>
    public class DailySchedulingStartegy : SchedulingStrategy
    {
        private GSMTaskBLL gsmTaskBLL = null;
        private DateTime startDate;
        private DateTime currentDate;
        private bool isCustom = false;
        private int intervalInDays = 0;
        private bool dateSet = false;
        private int year = DateTime.Now.Year;
        private int[] weekDayList;
        private int dayOfWeek = 0;
        private int[] dayNoListInWeek = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        private const int TASKPRIORITY = 4;
        public DailySchedulingStartegy()
        {
            gsmTaskBLL = new GSMTaskBLL();
        }
        #region ISchedulingStrategy Members

        public override DateTime ScheduleTask(GSMTaskEntity gsmTaskEntity)
        {
            currentDate = gsmTaskEntity.CalendarDate;
            isCustom = gsmTaskEntity.IsCustom;
            intervalInDays = gsmTaskEntity.IntervalInDays;
            gsmTaskEntity.taskType = GSMTasksType.Daily.ToString();
            gsmTaskEntity.taskPriority = GetPriority(GSMTasksType.Daily);
            if (!isCustom)
            {
                
                
                weekDayList = GetDayNoListInWeek(gsmTaskEntity);                
                dayOfWeek= gsmTaskBLL.GetDayNo(currentDate.DayOfWeek.ToString());
                startDate = (new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0));
                if (weekDayList[6] == 0 && dayOfWeek > 5)
                {
                    startDate = startDate.AddDays(8 - dayOfWeek);
                }
                //check if it is greater than current date
                while (DateTime.Now.AddMinutes(5) >= startDate)
                {
                    // make start date current week's monday
                    startDate = startDate.AddDays(1 - dayOfWeek);
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
                    if (!dateSet)
                        startDate = startDate.AddDays(7);
                }
            }
            else
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0);
                while (DateTime.Now.AddMinutes(5) >= startDate)
                {
                    startDate = startDate.AddDays(intervalInDays);
                }
            }
            return startDate;
        }

        #endregion


        private int[] GetDayNoListInWeek(GSMTaskEntity gsmTaskEntity)
        {
            int tryParseVar;
           
            string[] dayNameList = null;
            if (gsmTaskEntity.taskType.ToLower().Trim() == EnumUtil.stringValueOf(DailyTask.Daily).ToLower().Trim())
            {
                if (!(int.TryParse(gsmTaskEntity.tasksToBeRepeated, out tryParseVar)))
                {
                    if (gsmTaskEntity.tasksToBeRepeated.ToLower().Trim() == EnumUtil.stringValueOf(DailyTask.EveryDay).ToLower().Trim())
                    {
                        dayNoListInWeek = GetDaysList(true);
                    }
                    else if (gsmTaskEntity.tasksToBeRepeated.ToLower().Trim() == EnumUtil.stringValueOf(DailyTask.WeekDays).ToLower().Trim())
                    {
                        dayNoListInWeek = GetDaysList(false);
                    }
                    return dayNoListInWeek;
                }
            }
            else
            {
                dayNameList = gsmTaskEntity.DayNameList;
                foreach (string weekDay in dayNameList)
                {
                    if (weekDay.ToLower().Trim() == GSMSchedulingConstants.MONDAY)
                        dayNoListInWeek[0] = 1;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.TUESDAY)
                        dayNoListInWeek[1] = 2;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.WEDNESDAY)
                        dayNoListInWeek[2] = 3;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.THURSDAY)
                        dayNoListInWeek[3] = 4;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.FRIDAY)
                        dayNoListInWeek[4] = 5;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.SATURDAY)
                        dayNoListInWeek[5] = 6;
                    else if (weekDay.ToLower().Trim() == GSMSchedulingConstants.SUNDAY)
                        dayNoListInWeek[6] = 7;
                }
            }
            return dayNoListInWeek;
        }
        private int[] GetDaysList(bool isEveryDay)
        {

            for (int counter = 0; counter < dayNoListInWeek.Length; counter++)
            {
                if (!isEveryDay && counter>4)
                {
                    dayNoListInWeek[counter] = 0;
                }
                else
                {
                    dayNoListInWeek[counter] = counter+1;
                }
            }
                return dayNoListInWeek;
        }

       
    }
}
