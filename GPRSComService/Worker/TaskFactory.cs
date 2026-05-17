using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Tasks;

namespace GPRSComService.Worker
{
    public class TaskFactory
    {
       public static  TaskBase getTaskInstance(string taskType)
        {
           
            TaskBase task = null;

            switch (taskType.ToUpper())
            {
                case "GENERAL":
                    task = new GeneralTask();
                    break;
                case "INSTANTANEOUS":
                    task = new InstantaneousTask();
                    break;
                case "BILLING":
                    task= new BillingTask();
                    break;
                case "TAMPER":
                    task = new TamperTask();
                    break;
                case "LOADSURVEY":
                    task = new LoadSurveyTask();
                    break;
            }
           return task;
        }
    }

}
