using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using System.ServiceProcess;

namespace CAB.UI
{
    public partial class GSMScheduleActivation : MdiChildForm
    { 
        private bool activationStatus = false;
        public GSMScheduleActivation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (activationStatus)
                System.Diagnostics.Process.Start("net", "start IISAdmin");
            else
                System.Diagnostics.Process.Start("net", "stop IISAdmin");
        }

        private void rbtnStart_CheckedChanged(object sender, EventArgs e)
        {
            activationStatus = true;
        }

        private void rbtnStop_CheckedChanged(object sender, EventArgs e)
        {
            activationStatus = false;
        }

        private void GSMScheduleActivation_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Text = "Start/Stop GSM Service";
        }
    }
}
