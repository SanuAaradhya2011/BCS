using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using Microsoft.Win32;
using System.Diagnostics;

namespace CAB.UI
{
    public partial class SystemInfo : MdiChildForm
    {
        public SystemInfo()
        {
            InitializeComponent();
        }

        private void SystemInfo_Load(object sender, EventArgs e)
        {
            try
            {
                lblsystemname.Text = "Computer Name       :    " + System.Environment.MachineName;
                lbldomainname.Text = "User Name               :    " + System.Environment.UserName.ToString();
                lblsystemdirectory.Text = "System Directory      :    " + System.Environment.SystemDirectory;
                RegistryKey Rkey = Registry.LocalMachine;
                Rkey = Rkey.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                lblcpu.Text = "CPU and RAM         :    " + (string)Rkey.GetValue("ProcessorNameString");
                lblmonitorsize.Text = "Monitor Size             :    " + SystemInformation.PrimaryMonitorSize.ToString();
                lblOS.Text = "Operating system      :    " + System.Environment.OSVersion.ToString();


                Process[] processlist = Process.GetProcesses();

                int rcount = 0;
                lstinfo.Items.Add("     Process ID                                                              " + "Process Name");
                foreach (Process theprocess in processlist)
                {
                    rcount++;
                    lstinfo.Items.Add(theprocess.Id.ToString("       000000") + "                                                                  " + theprocess.ProcessName.ToString());


                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("BCS Error");
            }

 
        }
    }
}
