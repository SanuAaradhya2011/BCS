using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DLMS_Final
{
    public partial class frmModemAutoConfigure : Form
    {
        public delegate void OnConfigure(RichTextBox pRTB);
        public event OnConfigure ConfigNow;

        public frmModemAutoConfigure()
        {
            InitializeComponent();
        }

        private void frmModemAutoConfigure_Shown(object sender, EventArgs e)
        {
            if (ConfigNow != null)
            {
                ConfigNow(rtbLog);
            }
        }
    }
}
