using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialCommunication;

namespace DLMS_Final
{
    public partial class frmBaudRateSelector : Form
    {
        public frmBaudRateSelector()
        {
            InitializeComponent();
        }

        private DialogResult dlgResult = DialogResult.None;
        public DialogResult DlgResult
        {
            get
            {
                return dlgResult;
            }
        }
        private string strInitialBaudRate = "";
        public string StrInitialBaudRate
        {
            get
            {
                return strInitialBaudRate;
            }
        }
        private string strCOMPort = "";
        public string SelectedCOMPort
        {
            get
            {
                return strCOMPort;
            }
        }
        private bool isMultipleCOMPorts = false;
        public bool IsMultipleCOMPorts
        {
            get
            {
                return isMultipleCOMPorts;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbInitialBaudRate.Text))
            {
                MessageBox.Show("Please select a Baud Rate Value", "Select Baud Rate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (isMultipleCOMPorts && 
                string.IsNullOrEmpty(cmbCOMPorts.Text))
            {
                MessageBox.Show("Please select a COM Port", "Select COM Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                strInitialBaudRate = cmbInitialBaudRate.Text;
                if (isMultipleCOMPorts)
                {
                    strCOMPort = cmbCOMPorts.Text;
                }
                dlgResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dlgResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmBaudRateSelector_Load(object sender, EventArgs e)
        {
            if (cmbInitialBaudRate.Items.Contains("115200"))
            {
                cmbInitialBaudRate.SelectedIndex = cmbInitialBaudRate.Items.IndexOf("115200");
            }
            if (CABSerialPorts.ListOfSerialPorts.Count > 1)
            {
                isMultipleCOMPorts = true;
            }
            if (isMultipleCOMPorts)
            {
                pnlCOMPort.Visible = true;
                List<string> lstSerialPorts = new List<string>();
                for (int i = 0; i < CABSerialPorts.ListOfSerialPorts.Count; i++)
                {
                    lstSerialPorts.Add(CABSerialPorts.ListOfSerialPorts[i].PortName);
                }
                lstSerialPorts.Sort();
                cmbCOMPorts.Items.AddRange(lstSerialPorts.ToArray());
            }
            else
            {
                pnlCOMPort.Visible = false;
                btnOK.Location = new Point(btnOK.Location.X, btnOK.Location.Y - pnlCOMPort.Height);
                btnCancel.Location = new Point(btnCancel.Location.X, btnCancel.Location.Y - pnlCOMPort.Height);
                ((Form)this).Height = ((Form)this).Height - pnlCOMPort.Height;
                gbModem.Height = gbModem.Height - pnlCOMPort.Height;
            }
        }
    }
}
