using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace CAB.Framework
{
    public enum GSMTasksType
    {   
        Daily,
        Monthly,
        Weekly,
        [DescriptionAttribute("One Time Only")]
        OneTimeOnly
    }

    public enum DailyTask
    {
        [DescriptionAttribute("Daily")]
        Daily,
        [DescriptionAttribute("Every day")]
        EveryDay,
        [DescriptionAttribute("Weekdays")]
        WeekDays
    }

    public enum WeeklyTask
    {
        [DescriptionAttribute("Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday")]
        AllWeekDays,
        [DescriptionAttribute(" Monday,")]
        Monday,
        [DescriptionAttribute(" Tuesday,")]
        Tuesday,
        [DescriptionAttribute(" Wednesday,")]
        Wednesday,
        [DescriptionAttribute(" Thursday,")]
        Thursday,
        [DescriptionAttribute(" Friday,")]
        Friday,
        [DescriptionAttribute(" Saturday,")]
        Saturday,
        [DescriptionAttribute(" Sunday,")]
        Sunday
    }
    [Flags]
    public enum MonthlyTask
    {
        [DescriptionAttribute("Every day ")]
        EveryDayOf,
        [DescriptionAttribute(" Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec")]
        AllMonths,
        [DescriptionAttribute(" Jan,")]
        Jan,
        [DescriptionAttribute(" Feb,")]
        Feb,
        [DescriptionAttribute(" Mar,")]
        Mar,
        [DescriptionAttribute(" Apr,")]
        Apr,
        [DescriptionAttribute(" May,")]
        May,
        [DescriptionAttribute(" Jun,")]
        Jun,
        [DescriptionAttribute(" Jul,")]
        Jul,
        [DescriptionAttribute(" Aug,")]
        Aug,
        [DescriptionAttribute(" Sep,")]
        Sep,
        [DescriptionAttribute(" Oct,")]
        Oct,
        [DescriptionAttribute(" Nov,")]
        Nov,
        [DescriptionAttribute(" Dec,")]
        Dec
    }

    /// <summary>
    /// Enum for Months
    /// </summary>
    public enum Months
    {
        Jan=1,
        Feb=2,
        Mar=3,
        Apr=4,
        May=5,
        Jun=6,
        Jul=7,
        Aug=8,
        Sep=9,
        Oct=10,
        Nov=11,
        Dec=12
    }


    public enum JobType
    {
        [DescriptionAttribute("General, Billing, Instantaneous")]
        AllJobs,
        [DescriptionAttribute("Billing,")]
        Billing,
        [DescriptionAttribute("Instantaneous,")]
        Instantaneous,
        Anomaly,//added by ravi
        [DescriptionAttribute("General")]
        General,
        //adding load survey types and tamper as these are added in scheudling
        [DescriptionAttribute(" LoadSurveyComplete,")]
        LoadSurveyComplete,
        [DescriptionAttribute(" LoadSurveyPartial,")]
        LoadSurveyPartial,
        [DescriptionAttribute(" LoadSurveyPartialFrom,")]
        LoadSurveyPartialFrom,
        [DescriptionAttribute(" Tamper,")]
        Tamper,
        [DescriptionAttribute(" Midnight,")]
        Midnight,
        [DescriptionAttribute(" Meter Configuration,")]
        MeterConfiguration
        
    }

    public enum TaskManagerValidationMsgs
    {
        [DescriptionAttribute("Status can not be changed to InProgress")]
        StatusNotChangedToInProgress,
        [DescriptionAttribute("Status can not be changed to Completed")]
        StatusNotChangedToCompleted,
        [DescriptionAttribute("Schedule in Inprogress Status can not be Deleted")]
        InProgressScheduleNotDeleted,
        [DescriptionAttribute("Schedule in Inprogress Status can not be made Inactive")]
        InProgressScheduleNotInactive
    }

    public enum ValidationMsgs
    {
        [DescriptionAttribute("Task Name can not be empty")]
        TaskNameEmpty,
        [DescriptionAttribute(" already exists, enter some other Task Name.")]
        TaskAlreadyPresent,
        [DescriptionAttribute("Select one of the Tasks to perform: Daily, Weekly, Monthly or One Time Only")]
        ValidateTaskPerformed,
        [DescriptionAttribute("Please select any group")]
        GroupSelected,
        [DescriptionAttribute("Select the value for Hour in Start Time")]
        ValidateHour,
        [DescriptionAttribute("Select the value for Minutes in Start Time")]
        ValidateMinutes,
        [DescriptionAttribute("Start date can not be less than Today's date")]
        ValidateStartDate,
        [DescriptionAttribute("Select one of the Tasks: Every day, Weekdays or every custom day")]
        ValidateDailyTask,
        [DescriptionAttribute("Select one of the week days")]
        ValidateWeeklyTask,
        [DescriptionAttribute("Select atleast one of the Months, or select 'Select All' checkbox")]
        ValidateMonthlyTask,
        [DescriptionAttribute("There is already another Task scheduled at ")]
        ValidateTaskScheduling,
        [DescriptionAttribute("Please select alteast one task to Delete")]
        ValidateTaskDeletion,
        [DescriptionAttribute("Are you sure, you want to Delete the selected Schedule(s)?")]
        TaskDeletionQuestion,
        [DescriptionAttribute("Are you sure, you want to make selected Schedule(s) Inactive?")]
        TaskInactiveQuestion,
        [DescriptionAttribute("Are you sure, you want to make selected Schedule(s) Active?")]
        TaskActiveQuestion,
        [DescriptionAttribute("Schedule(s) deleted successfully")]
        TaskDeletionSuccessful,
        [DescriptionAttribute("Schedule(s) were not deleted successfully")]
        TaskDeletionUnsuccessful,
        [DescriptionAttribute("Change the status to Active or Inactive for Schedule(s): ")]
        ValidateActiveInactiveStatusUpdation,
        [DescriptionAttribute("Please select atleast one Schedule to made Inactive.")]
        ValidateTaskStatusInactive,
        [DescriptionAttribute("Please select atleast one Schedule to made Active.")]
        ValidateTaskStatusActive,
        [DescriptionAttribute("Schedule(s) InActivated successfully")]
        TaskInacticeSuccessful,
        [DescriptionAttribute("Schedule(s) were not InActivated successfully")]
        TaskInactiveUnsuccessful,
        [DescriptionAttribute("Please select a Schedule to Abort")]
        ValidateTaskAbortion,
        [DescriptionAttribute("Are you sure, you want to abort the selected Schedule?")]
        TaskAbortionQuestion,
        [DescriptionAttribute("Schedule aborted successfully")]
        TaskAbortionSuccessful,
        [DescriptionAttribute("Schedule was not aborted successfully")]
        TaskAbortionUnsuccessful,
        [DescriptionAttribute("Schedule only in Inprogress status can be aborted")]
        TaskInprogressAbort,
        [DescriptionAttribute("Schedule(s) Activated successfully")]
        TaskActiveSuccessful,
        [DescriptionAttribute("Schedule(s) were not Activated successfully")]
        TaskActiveUnsuccessful,
        [DescriptionAttribute("Start time can not be less/equal than Current time.")]
        StartTimeLessCurrentTime,
        [DescriptionAttribute("Task Rescheduled")]
        TaskRescheduled,
        [DescriptionAttribute("Load Survey From date can not be greater than To Date")]
        FromDateCanNotBeGreatherThanToDate
    }


    public enum TaskStatus
    {
        [DescriptionAttribute("Inqueue")]
        Active,
        [DescriptionAttribute("Inactive")]
        Inactive,
        [DescriptionAttribute("Inprogress")]
        InProgress,
        [DescriptionAttribute("Remote modem connected.")]
        Remotemodemconnected,
        [DescriptionAttribute("Trying to connect TCP Modem again...")]
        Tryingtoconnectmodem
    }

    public enum BCSStatus
    {
        [DescriptionAttribute("Running")]
        Running,
        [DescriptionAttribute("Restart")]
        Restart,
        [DescriptionAttribute("Resume")]
        Resume
    }
}
