using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.UI.Controls;
using CAB.IECFramework.Utility;
using LTCTBLL;
using CAB.Entity;
namespace CAB.UI
{
    public partial class PortSettingForm : MdiChildForm
    {
        public PortSettingForm()
        {
            InitializeComponent();
        }

        private void PortSettingForm_Load(object sender, EventArgs e)
        {
            SettingsBLL settingsBLL = new SettingsBLL();
            this.cboPort.DataSource = settingsBLL.GetPort().Tables[0];
            this.cboPort.DisplayMember = "DisplayMember";
            this.cboPort.ValueMember = "ValueMember";

            this.cboBaudRate.DataSource = settingsBLL.GetBaudRate().Tables[0];
            this.cboBaudRate.DisplayMember = "DisplayMember";
            this.cboBaudRate.ValueMember = "ValueMember";

            this.cboCommMode.DataSource = settingsBLL.GetCommunicationMode().Tables[0];
            this.cboCommMode.DisplayMember = "DisplayMember";
            this.cboCommMode.ValueMember = "ValueMember";
            int counter = 0;
            string value = ConfigSettings.GetValue("PortName");
            for (counter = 0; counter < cboPort.Items.Count; counter++)
            {
                cboPort.SelectedIndex = counter ;
                if ((((System.Data.DataRowView)(cboPort.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            value = ConfigSettings.GetValue("BaudRate");
            for (counter = 0; counter < cboBaudRate.Items.Count; counter++)
            {
                cboBaudRate.SelectedIndex = counter ;
                if ((((System.Data.DataRowView)(cboBaudRate.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            value = ConfigSettings.GetValue("CommunicationMode");
            for (counter = 0; counter < cboCommMode.Items.Count; counter++)
            {
                cboCommMode.SelectedIndex = counter ;
                if ((((System.Data.DataRowView)(cboCommMode.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
            ConfigSettings.ChangeNode("BaudRate", ((System.Data.DataRowView)(cboBaudRate.Items[cboBaudRate.SelectedIndex])).Row.ItemArray[0].ToString());
            ConfigSettings.ChangeNode("CommunicationMode", ((System.Data.DataRowView)(cboCommMode.Items[cboCommMode.SelectedIndex])).Row.ItemArray[0].ToString());
            this.StatusMessage = "Setting's Saved Successfully.";
        }

        private void lngbCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void PortSettingForm_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        //private void cboCommMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboCommMode.SelectedIndex == 1)
        //        cboBaudRate.Text = "300";
        //}

        private void cboCommMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBaudRate.Enabled = true;
            
            if (cboCommMode.SelectedIndex == 1)
            {
                // Added for PVVNL utility 
                if (UtilityDetails.UtilityName != UtilityEntity.PVVNL)
                {
                    cboBaudRate.Text = "300";
                    cboBaudRate.Enabled = false;
                }
                else
                {
                    cboBaudRate.Text = "9600";
                    cboBaudRate.Enabled = false;
                }
            }
        }

        private void PortSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }
    }
}
