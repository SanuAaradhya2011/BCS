using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.Framework;
using GPRSComService.Worker;
using System.Threading;

namespace GPRSComService
{
    class GPRSSchedulerProcess : ProcessBase
    {
        protected override short WaitIntervalBeforeAbort
        {
            get { return Int16.Parse(Constants.GetConfigValue(Constants.WaitIntervalBeforeAbort)); }
        }

        protected override void CreateWorkers()
        {
            base.AddWorker(new GetNewTaskWorker());
            base.AddWorker(new CommandProcessorWorker());
            base.AddWorker(new ResponseProcessorWorker());
            base.AddWorker(new ProcessTaskWorker());
            base.AddWorker(new FileUploaderWorker());
            base.AddWorker(new EndpointOperationsWorker());
        }
    }
}
