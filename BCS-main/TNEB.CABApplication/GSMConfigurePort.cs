using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using System.Management;
using System.Collections;

namespace CAB.UI
{
    public partial class GSMConfigurePort : MdiChildForm
    {
        public GSMConfigurePort()
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
             
        }
    }
}
