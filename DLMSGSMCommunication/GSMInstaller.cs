using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess; 


namespace DLMSGSMCommunication
{
    [RunInstaller(true)]
    public partial class GSMInstaller : Installer
    {
        ServiceInstaller serviceInstaller;
        public GSMInstaller()
        {
            InitializeComponent();
            ServiceProcessInstaller sp = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();
            this.AfterInstall += new InstallEventHandler(GSMInstaller_AfterInstall);
            //# Service Account Information
            sp.Account = ServiceAccount.LocalSystem;
            sp.Username = null;
            sp.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "Generic 3Phase Communication";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "Generic3PhaseCommunication";
            this.Installers.AddRange(new Installer[] { sp, serviceInstaller });

        }

        void GSMInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController(serviceInstaller.ServiceName);
            sc.Start(); 
        }
        //public override void Install(IDictionary stateSaver)
        //{
            
        //    base.Install(stateSaver);
        //} 
    }
}
