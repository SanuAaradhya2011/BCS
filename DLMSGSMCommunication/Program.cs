using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Utilities;

namespace DLMSGSMCommunication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			{ 
				new GSMCommunication() 
			};
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("Message : " + ex.Message + "Source : " + ex.Source);
            }
        }
    }
}
