using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using LNG.Framework.Utility;
using LNG.Framework;
namespace LNG.Entity
{
    public class GSMTaskEntity
    {
        private int[] monthNoList;
        private string creationDateTime;
        private string StartTime;
        public int taskId { get; set; }
        public string taskName { get; set; }
        public int groupId { get; set; }
        private bool isCustom = false;
        private int intervalInDays = 0;
        private DateTime calendarDate;
        private int[] weekDayList;
        public string CreationDateTime
        {
            get
            {
                return creationDateTime;
            }
            set
            {
                creationDateTime = value;
            }
        }
        public int[] WeekDayList
        {
            get
            {
                return weekDayList;
            }
            set
            {
                weekDayList = value;
            }
        }
        public int[] MonthNoList
        {
            get
            {
                return monthNoList;
            }
            set
            {
                monthNoList = value;
            }
        }
        [Browsable(false)]
        public bool IsCustom
        {
            get
            {
                return isCustom;
            }
            set
            {
                isCustom = value;
            }
        }
        [Browsable(false)]
        public int IntervalInDays
        {
            get
            {
                return intervalInDays;
            }
            set
            {
                intervalInDays = value;
            }
        }
        [Browsable(false)]
        public DateTime CalendarDate
        {
            get
            {
                return calendarDate;
            }
            set
            {
                calendarDate = value;
            }
        }
        public string groupName { get; set; }
        public string taskType { get; set; }
        public string startDate { get; set; }
        //return String.Format("{0:dd/MM/yyyy}", localStartDate);
        //localStartDate = Convert.ToDateTime(value);
        public string startTime
        {
            get
            {
                return StartTime;
            }
            set
            {
                StartTime = value;
            }
        }
        public string tasksToBeRepeated { get; set; }
        [Browsable(false)]
        public string jobNames { get; set; }
        [Browsable(false)]
        public int taskRetries { get; set; }
        public int taskPriority { get; set; }
        [Browsable(false)]
        public string taskStatus { get; set; }
        public bool isGeneralRequired { get; set; }
        public bool isBillingRequired { get; set; }
        public bool isInstantaneousRequired { get; set; }
        //gets or sets if load survey is required
        public bool IsLoadSurveyRequired { get; set; }
        //gets or sets if tamper is required
        public bool IsTamperRequired { get; set; }
        //gets or sets if midnight is required
        public bool IsMidnightRequired { get; set; }
        public bool IsMeterConfigRequired { get; set; }
        [Browsable(false)]
        public int dayOfMonth { get; set; }
        public string[] MonthList { get; set; }
        public string[] DayNameList { get; set; }
        [Browsable(false)]
        public string StartHour
        {
            get
            {
                if (!String.IsNullOrEmpty(StartTime))
                    return StartTime.Substring(0, 2);
                else
                    return string.Empty;
            }
        }
        [Browsable(false)]
        public string StartMinute
        {
            get
            {
                if (!String.IsNullOrEmpty(StartTime))
                    return StartTime.Substring(3, 2);
                else
                    return string.Empty;
            }
        }
        /// <summary>
        /// gets or sets the load survey to data
        /// </summary>
        [Browsable(false)]
        public DateTime LoadSurveyToDate { get; set; }
         /// <summary>
         /// gets or sets the load survey from date
         /// </summary>
         [Browsable(false)]
        public DateTime LoadSurveyFromDate { get; set; }
        /// <summary>
        /// gets or sets the load survey job type
        /// </summary>
         [Browsable(false)]
         public JobType LoadSurveyJobType { get; set; }
    }
}

