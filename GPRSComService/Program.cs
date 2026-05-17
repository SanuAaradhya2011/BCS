using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace GPRSComService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if DEBUG 
            GPRSSchedulerProcess process = new GPRSSchedulerProcess();
            process.Start();
#else
            StartService();
#endif
        
        }

        private static void StartService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new GPRSSchedulerService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
