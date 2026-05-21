/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Parvinder Singh										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using CAB.DALC.Data;
using CAB.Entity;
using CAB.Framework;
using Utilities;
using System.Data;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class GSMTaskBLL
    {
        GSMTaskDAL gsmTaskDAL;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMTaskBLL).ToString());

        public GSMTaskBLL()
        {
            gsmTaskDAL = new GSMTaskDAL();
        }

        public bool InsertGSMTask(GSMTaskEntity gsmTaskEntity)
        {
            return gsmTaskDAL.InsertGSMTask(gsmTaskEntity);
        }

        //ashish
        public bool InsertCompleteTask(GSMTaskEntity gsmTaskEntity)
        {
            return gsmTaskDAL.InsertCompleteTask(gsmTaskEntity);
        }
        //ashish
        public GSMTaskEntity GetCompletedTaskByID(int taskID)
        {
            return gsmTaskDAL.GetCompletedTaskByID(taskID);
        }
        //ashish
        public List<GSMTaskEntity> getAllSchedulesTasks(string taskStatus)
        {
            return gsmTaskDAL.GetScheduledTasks(taskStatus);
        }
        //ashish
        public List<GSMTaskEntity> getReportSchedules(DateTime fromDate, DateTime toDate, string scheduleType, string groupName, string taskStatus)
        {
            return gsmTaskDAL.getReportSchedules(fromDate, toDate, scheduleType, groupName, taskStatus);
        }

        public List<GSMTaskEntity> GetFilteredScheduledTasks(string taskStatus)
        {
            return gsmTaskDAL.GetFilteredScheduledTasks(taskStatus);
        }

        public bool deleteGSMTasks(List<GSMTaskEntity> colTaskEntity)
        {
            return gsmTaskDAL.deleteGSMTasks(colTaskEntity);
        }

        public bool updateGSMTasks(List<GSMTaskEntity> colTaskEntity)
        {
            foreach (GSMTaskEntity entity in colTaskEntity)
            {
                UpdateStartTime(entity);
            }
            return gsmTaskDAL.updateGSMTasks(colTaskEntity);
        }

        public bool UpdateGSMTask(GSMTaskEntity gsmTaskEntity)
        {

            return gsmTaskDAL.UpdateGSMTask(gsmTaskEntity);
        }
        /// <summary>
        /// Used to delete completed tasks.
        /// </summary>
        /// <param name="colTaskEntity"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool DeleteCompletedTasks(List<GSMTaskEntity> colTaskEntity,int groupId)
        {
            return gsmTaskDAL.DeleteCompletedTasks(colTaskEntity, groupId);
        }
        /// <summary>
        /// Get all tasks Associated with a group and creates GSMTaskEntity from each tsakid(s).
        /// </summary>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public List<GSMTaskEntity> GetTasksByGroupId(int groupId)
        {       
            //Get All taskids for a group
            DataSet gsmTasks = gsmTaskDAL.GetTasksByGroupId(groupId);
            GSMTaskEntity gsmTaskEntity  ;
            List<GSMTaskEntity> gsmTaskEntityList = new List<GSMTaskEntity>();
            try
            {
                //Create TaskEntity from TsakId's
                if (gsmTasks != null && gsmTasks.Tables != null && gsmTasks.Tables.Count > 0)
                {
                    foreach (DataRow taskRow in gsmTasks.Tables[0].Rows)
                    {
                        gsmTaskEntity = new GSMTaskEntity();
                        gsmTaskEntity.taskId = Convert.ToInt32(taskRow["TaskId"]);
                        gsmTaskEntityList.Add(gsmTaskEntity);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, " GetTasksByGroupId(int groupId)", ex);
            }
            return gsmTaskEntityList;
        }                              

        public bool UpdateGSMTask(GSMTaskEntity gsmTaskEntity, bool isUpdateStartTime)
        {
            if (isUpdateStartTime)
                gsmTaskEntity = UpdateStartTime(gsmTaskEntity);
            return gsmTaskDAL.UpdateGSMTask(gsmTaskEntity);
        }

        public bool updateGSMTasksStatus(List<GSMTaskEntity> colTaskEntity)
        {
            return gsmTaskDAL.updateGSMTasksStatus(colTaskEntity);
        }

        public GSMTaskEntity GetTaskByTaskID(int taskID)
        {
            return gsmTaskDAL.GetTaskByTaskID(taskID);
       
        }



        /// <summary>
        /// new method added for GPRS scheduling support
        /// </summary>
        /// <param name="gsmTaskEntity"></param>
        public void UpdateStartTimeGPRS(GSMTaskEntity gsmTaskEntity)
        {
            DateTime startDate = Convert.ToDateTime(gsmTaskEntity.startDate, new System.Globalization.CultureInfo("en-GB"));

            switch (gsmTaskEntity.taskType)
            {
                case "Daily" :
                   
                   switch (gsmTaskEntity.tasksToBeRepeated)
                    {
                        case "Every day":
                            startDate = startDate.AddDays(1);
                            break;
                        case "Weekdays":
                          if(startDate.DayOfWeek.Equals(DayOfWeek.Friday))
                              startDate = startDate.AddDays(3);
                          else
                              startDate = startDate.AddDays(1);
                            break;
                    }

                    // nth day scenario
                   int tryParseVal = 0;
                   
                   if (int.TryParse(gsmTaskEntity.tasksToBeRepeated, out tryParseVal))
                   {
                       startDate = startDate.AddDays(Convert.ToInt32(gsmTaskEntity.tasksToBeRepeated));
                   }

                    break;
                case "Weekly":
                    startDate = startDate.AddDays(7);
                    break;
                case "Monthly":

                    string[] monthList = gsmTaskEntity.tasksToBeRepeated.Split(',');
     
                    int currentMonthIndex = Array.FindIndex<string>(monthList, new Predicate<string>((string a) => { if (a.Trim() == Enum.GetName(typeof(Months), startDate.Month).Trim()) return true; return false; }));

                    int nextMonthIndex = monthList.Count() - 1== 1 ? 1: currentMonthIndex == monthList.Count() - 1 ? 1 : currentMonthIndex + 1;

                    int diff = (Int16)((Months)Enum.Parse(typeof(Months), monthList[nextMonthIndex].Trim())) - (Int16)((Months)Enum.Parse(typeof(Months), monthList[currentMonthIndex].Trim()));
                    
                    diff = diff <= 0 ? diff + 12 : diff;

                    startDate = startDate.AddMonths(diff);
                    
                    break;
            }

            gsmTaskEntity.startDate = startDate.ToShortDateTimeCABFormat();
        }



        public GSMTaskEntity UpdateStartTime(GSMTaskEntity gsmTaskEntity)
        {
            int hour = 0;
            int minutes = 0;
            int repeatInHours = 0;

            try
            {
                if (gsmTaskEntity != null)
                {
                    hour = Convert.ToInt32(gsmTaskEntity.StartHour);
                    minutes = Convert.ToInt32(gsmTaskEntity.StartMinute);
                    #region Daily
                    if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Daily))
                    {
                        int counter = 0;
                        bool isCustom = false;
                        int tryParseVal;
                        int year;
                        int dayOfWeek;
                        
                        DateTime checkDate = Convert.ToDateTime(gsmTaskEntity.startDate, new System.Globalization.CultureInfo("en-GB"));
                        DateTime startDate = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0);

                        if (int.TryParse(gsmTaskEntity.tasksToBeRepeated, out tryParseVal))
                            isCustom = true;
                        if (!isCustom)
                        {
                            bool dateSet = false;
                            int[] weekDayList = GetWeekNoList(gsmTaskEntity);

                            year = DateTime.Now.Year;
                            dayOfWeek = GetDayNo(DateTime.Now.DayOfWeek.ToString());

                            //fix Ashish - make start date start Date week's monday
                         
                             startDate = DateTime.Now.AddDays(1 - dayOfWeek);
                         
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
                        }
                        else
                        {
                            startDate = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0);
                            while (DateTime.Now.AddMinutes(5) >= startDate)
                            {
                                startDate = startDate.AddDays(Convert.ToInt32(gsmTaskEntity.tasksToBeRepeated));
                            }
                        }
                        gsmTaskEntity.startDate = startDate.ToShortDateTimeCABFormat();
                    }
                    #endregion
                    #region Weekely
                    else if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                    {
                        int counter = 0;
                        bool dateSet = false;
                        int currentDayNo;
                        int[] weekNoList = GetWeekNoList(gsmTaskEntity);
                        int lowestDayNo = GetMinDayNo(weekNoList);
                        int highestDayNo = weekNoList.Max();
                        currentDayNo = GetDayNo(DateTime.Now.DayOfWeek.ToString());

                        DateTime checkDate = Convert.ToDateTime(gsmTaskEntity.startDate, new System.Globalization.CultureInfo("en-GB"));
                        DateTime startDate = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0);

                        //fix Ashish - make startdate current weeks monday
                        startDate = DateTime.Now.AddDays(1 - GetDayNo(DateTime.Now.DayOfWeek.ToString()));

                        while (DateTime.Now.AddMinutes(5) >= startDate)
                        {
                            if (!dateSet && counter > 0)
                                startDate = startDate.AddDays(7);
                            foreach (int dayNo in weekNoList)
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
                        gsmTaskEntity.startDate = startDate.ToShortDateTimeCABFormat();
                    }
                    #endregion
                    #region Monthly
                    else if (gsmTaskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                    {
                        int[] monthNoList = GetMonthNoList(gsmTaskEntity);
                        DateTime startDate = Convert.ToDateTime(gsmTaskEntity.startDate, new System.Globalization.CultureInfo("en-GB"));
                        int year = DateTime.Now.Year;

                        //increament and assign startdate untill it is greater than current date
                        while (DateTime.Now.AddMinutes(5) >= startDate)
                        {
                            startDate = (new DateTime(year, 1, startDate.Day, Convert.ToInt32(gsmTaskEntity.StartHour), Convert.ToInt32(gsmTaskEntity.StartMinute), 0));
                            foreach (int monthNo in monthNoList)
                            {
                                if (monthNo > 0)
                                {
                                    if (DateTime.Now.AddMinutes(5) < startDate.AddMonths(monthNo - 1))
                                    {
                                        startDate = startDate.AddMonths(monthNo - 1);
                                        break;
                                    }
                                }
                            }
                            year = year + 1;
                        }
                        gsmTaskEntity.startDate = startDate.ToShortDateTimeCABFormat();
                    }
                    #endregion
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateStartTime(GSMTaskEntity gsmTaskEntity)", ex);
                EventLogging.CallLogDetails(ex.Message);
            }
            return gsmTaskEntity;
        }

        public int DayDiff(string currentDay, string futureDay)
        {
            int currentDayNo = 0;
            int futureDayNo = 0;
            currentDayNo = GetDayNo(currentDay);
            futureDayNo = GetDayNo(futureDay);

            if (futureDayNo >= currentDayNo)
                return futureDayNo - currentDayNo;
            else
                return 7 + futureDayNo - currentDayNo;
        }

        public int MonthDiff(string currentMonth, string futureMonth)
        {
            int currentMonthNo = 0;
            int futureMonthNo = 0;
            currentMonthNo = GetMonthNo(currentMonth);
            futureMonthNo = GetMonthNo(futureMonth);
            if (futureMonthNo >= currentMonthNo)
                return futureMonthNo - currentMonthNo;
            else
                return 12 + futureMonthNo - currentMonthNo;
        }

        public int GetDayNo(string currentDay)
        {
            int dayNo = 0;
            switch (currentDay.ToLower().Trim())
            {
                case "monday":
                    dayNo = 1;
                    break;
                case "tuesday":
                    dayNo = 2;
                    break;
                case "wednesday":
                    dayNo = 3;
                    break;
                case "thursday":
                    dayNo = 4;
                    break;
                case "friday":
                    dayNo = 5;
                    break;
                case "saturday":
                    dayNo = 6;
                    break;
                case "sunday":
                    dayNo = 7;
                    break;
                default:
                    dayNo = 0;
                    break;
            }
            return dayNo;
        }

        private int GetMonthNo(string month)
        {
            int monthNo = 0;
            switch (month.ToLower())
            {
                case "jan":
                    monthNo = 1;
                    break;
                case "feb":
                    monthNo = 2;
                    break;
                case "mar":
                    monthNo = 3;
                    break;
                case "apr":
                    monthNo = 4;
                    break;
                case "may":
                    monthNo = 5;
                    break;
                case "jun":
                    monthNo = 6;
                    break;
                case "jul":
                    monthNo = 7;
                    break;
                case "aug":
                    monthNo = 8;
                    break;
                case "sep":
                    monthNo = 9;
                    break;
                case "oct":
                    monthNo = 10;
                    break;
                case "nov":
                    monthNo = 11;
                    break;
                case "dec":
                    monthNo = 12;
                    break;
                default:
                    monthNo = 0;
                    break;
            }
            return monthNo;
        }

        public int[] GetWeekNoList(GSMTaskEntity gsmTaskEntity)
        {
            int tryParseVar;
            int counter = 0;
            int[] weekNoList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string[] dayNameList = null;
            if (gsmTaskEntity.taskType.ToLower().Trim() == "daily")
            {
                if (!(int.TryParse(gsmTaskEntity.tasksToBeRepeated, out tryParseVar)))
                {
                    if (gsmTaskEntity.tasksToBeRepeated.ToLower().Trim() == "every day")
                    {
                        weekNoList[0] = 1;
                        weekNoList[1] = 2;
                        weekNoList[2] = 3;
                        weekNoList[3] = 4;
                        weekNoList[4] = 5;
                        weekNoList[5] = 6;
                        weekNoList[6] = 7;
                    }
                    else if (gsmTaskEntity.tasksToBeRepeated.ToLower().Trim() == "weekdays")
                    {
                        weekNoList[0] = 1;
                        weekNoList[1] = 2;
                        weekNoList[2] = 3;
                        weekNoList[3] = 4;
                        weekNoList[4] = 5;
                        weekNoList[5] = 0;
                        weekNoList[6] = 0;
                    }
                    return weekNoList;
                }
            }
            else
                dayNameList = gsmTaskEntity.DayNameList;

            foreach (string weekDay in dayNameList)
            {
                if (weekDay.ToLower().Trim() == "monday")
                    weekNoList[0] = 1;
                else if (weekDay.ToLower().Trim() == "tuesday")
                    weekNoList[1] = 2;
                else if (weekDay.ToLower().Trim() == "wednesday")
                    weekNoList[2] = 3;
                else if (weekDay.ToLower().Trim() == "thursday")
                    weekNoList[3] = 4;
                else if (weekDay.ToLower().Trim() == "friday")
                    weekNoList[4] = 5;
                else if (weekDay.ToLower().Trim() == "saturday")
                    weekNoList[5] = 6;
                else if (weekDay.ToLower().Trim() == "sunday")
                    weekNoList[6] = 7;
            }
            return weekNoList;
        }

        private int[] GetMonthNoList(GSMTaskEntity gsmTaskEntity)
        {
            int counter = 0;
            int[] monthNoList = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (string month in gsmTaskEntity.MonthList)
            {
                switch (month.ToLower().Trim())
                {
                    case "jan":
                        monthNoList[0] = 1;
                        break;
                    case "feb":
                        monthNoList[1] = 2;
                        break;
                    case "mar":
                        monthNoList[2] = 3;
                        break;
                    case "apr":
                        monthNoList[3] = 4;
                        break;
                    case "may":
                        monthNoList[4] = 5;
                        break;
                    case "jun":
                        monthNoList[5] = 6;
                        break;
                    case "jul":
                        monthNoList[6] = 7;
                        break;
                    case "aug":
                        monthNoList[7] = 8;
                        break;
                    case "sep":
                        monthNoList[8] = 9;
                        break;
                    case "oct":
                        monthNoList[9] = 10;
                        break;
                    case "nov":
                        monthNoList[10] = 11;
                        break;
                    case "dec":
                        monthNoList[11] = 12;
                        break;
                }
            }
            return monthNoList;
        }

        private int GetMinNo(int[] highestMonthNo)
        {
            int minNo = 0;
            for (int counter = 0; counter < highestMonthNo.Length; counter++)
            {
                if (counter == 0 && highestMonthNo[counter] > 0)
                {
                    minNo = highestMonthNo[counter];
                }
                if (counter > 0)
                {
                    if (minNo == 0 && highestMonthNo[counter] > 0)
                    {
                        minNo = highestMonthNo[counter];

                    }
                    if (highestMonthNo[counter] < minNo && highestMonthNo[counter] > 0)
                    {
                        minNo = highestMonthNo[counter];
                    }
                }
            }
            return minNo;
        }

        private int GetMinDayNo(int[] weekDayList)
        {
            int minDayNo = 0;
            for (int counter = 0; counter < weekDayList.Length; counter++)
            {
                if (counter == 0)
                {
                    minDayNo = weekDayList[counter];
                }
                if (counter > 0)
                {
                    if (minDayNo == 0 && weekDayList[counter] > 0)
                    {
                        minDayNo = weekDayList[counter];
                    }
                    if (weekDayList[counter] < minDayNo && weekDayList[counter] > 0)
                    {
                        minDayNo = weekDayList[counter];
                    }
                }
            }
            return minDayNo;
        }
        //Yatin 13-Jan-2012 Returns true when any active task for the meterid exists in the database (Inqueue, Inprogress, and Completed tasks are checked for).
        public bool DoesActiveTaskExistsForMeterID(string pMeterID)
        {
            return gsmTaskDAL.DoesActiveTaskExistsForMeterID(pMeterID);
        }
    }
}