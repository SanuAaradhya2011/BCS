using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Hunt.EPIC.Logging;

namespace GPRSComService
{
    public partial class GPRSSchedulerService : ServiceBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSSchedulerService).ToString());
        GPRSSchedulerProcess process;
        public GPRSSchedulerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                logger.Log(LOGLEVELS.Info, "Starting GPRSSchedulerService");
                process = new GPRSSchedulerProcess();
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnStop()
        {
            process.Stop();
            logger.Log(LOGLEVELS.Info, "Stopping GPRSSchedulerService. OnStop method getting executed.");
        }
    }
}
