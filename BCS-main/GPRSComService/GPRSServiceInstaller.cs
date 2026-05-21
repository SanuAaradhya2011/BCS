using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading;


namespace GPRSComService
{
    [RunInstaller(true)]
    public partial class GPRSServiceInstaller : Installer
    {
        public GPRSServiceInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            Thread.Sleep(20000);
            base.Install(stateSaver);
            //StartWindowService();
        }

        /// <summary>
        /// Start the Communication Manager window Service
        /// </summary>
        private static void StartWindowService()
        {
            ServiceController controller = new ServiceController(GPRSServiceInstaller.SERVICENAME);
            switch (controller.Status)
            {
                case ServiceControllerStatus.ContinuePending:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.StopPending:
                    break;
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.Stopped:
                    controller.Start();
                    controller.WaitForStatus(ServiceControllerStatus.Running, System.TimeSpan.FromSeconds(30));
                    break;
            }
        }
    }
}
