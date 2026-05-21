using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CABApplication;
namespace CAB.UI
{
    public partial class CMRICommunicationPrepareSmartHHU : Form
    {
        public byte SmartHHUSelectionType =0;
        public CMRICommunicationPrepareSmartHHU()
        {
            InitializeComponent();
        }

        private void BthSave_Click(object sender, EventArgs e)
        {
            SmartHHUSelectionType = (byte)cmbSelectionType.SelectedIndex;
            this.Close();
        }

        private void CMRICommunicationPrepareSmartHHU_Load(object sender, EventArgs e)
        {
            cmbSelectionType.SelectedIndex = 0;
        }
    }
}
