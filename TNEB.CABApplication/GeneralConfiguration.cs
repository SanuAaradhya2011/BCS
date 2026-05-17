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
using System.IO;
using LTCTBLL;
using CAB.Entity;
namespace CAB.UI
{
    public partial class GeneralConfiguration : MdiChildForm
    {
        public GeneralConfiguration()
        {
            InitializeComponent();
        }

        private void PortSettingForm_Load(object sender, EventArgs e)
        {
            SettingsBLL settingsBLL = new SettingsBLL();
            this.cboDate.DataSource = settingsBLL.GetDateTypes().Tables[0];
            this.cboDate.DisplayMember = "DisplayMember";
            this.cboDate.ValueMember = "ValueMember";
            this.rbtnDefault.Checked = true;
            string value = ConfigSettings.GetValue("DateFormat");
            string locType = ConfigSettings.GetValue("LocationType");
            string fileNamingConvention = ConfigSettings.GetValue("FileNamingConvention");
            if (locType.Equals("Default"))
            {
                rbtnDefault.Checked = true;
                btnBrowse.Visible = false;
                rbtnCustom.Checked = false;
                txtDefaultCABLocation.Text = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\");
                txtCustomCABLocation.Visible = false;
            }
            else
            {
                txtCustomCABLocation.Text = ConfigInfo.GetLocation();
                rbtnCustom.Checked = true;
                btnBrowse.Visible = true;
                rbtnDefault.Checked = false;
                txtDefaultCABLocation.Visible = false;
            }
            for (int counter = 0; counter < cboDate.Items.Count; counter++)
            {
                cboDate.SelectedIndex = counter ;
                if ((((System.Data.DataRowView)(cboDate.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            if (fileNamingConvention.Trim().Equals("Default"))
              rboFNC1.Checked=true;
            else if (fileNamingConvention.Trim().Equals("Default+System"))
                rboFNC2.Checked = true;
            else if (fileNamingConvention.Trim().Equals("Custom"))
                rboFNC3.Checked = true;
            locType = ConfigSettings.GetValue("TOULocationType");
            if (locType.Equals("Default"))
            {
                rbtnTOUDefault.Checked = true;
                btnTOUBrowse.Visible = false;
                rbtnTOUCustom.Checked = false;
                txtDefaultTOULocation.Text = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\TOU\");
                txtCustomTOULocation.Visible = false;
            }
            else
            {
                txtCustomTOULocation.Text = ConfigInfo.GetTOULocation();
                rbtnTOUCustom.Checked = true;
                btnTOUBrowse.Visible = true;
                rbtnTOUDefault.Checked = false;
                txtDefaultTOULocation.Visible = false;
            }
            tabControl1.TabPages.Remove(tbDashBoard);
            if (UtilityDetails.UtilityName != UtilityEntity.UGVCL && UtilityDetails.UtilityName != UtilityEntity.PVVNL && UtilityDetails.UtilityName != UtilityEntity.JDVVNL)
            {
                groupBox5.Enabled = false;
            }
            
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string locationType = string.Empty;
            if (this.rbtnDefault.Checked)
            {
                locationType = "Default";
                ConfigSettings.ChangeNode("FileLocation", string.Empty); 
            }
            else
            {
                locationType = "Custom"; 
                if (txtCustomCABLocation.Text.Trim() == "")
                {
                    this.StatusMessage = "CAB file location can not be empty";
                    Application.DoEvents();
                    return;
                }
                ConfigSettings.ChangeNode("FileLocation", txtCustomCABLocation.Text);
            }
            if(rboFNC1.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Default");
            else if (rboFNC2.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Default+System");
            else if (rboFNC3.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Custom");

            ConfigSettings.ChangeNode("DateFormat", ((System.Data.DataRowView)(cboDate.Items[cboDate.SelectedIndex])).Row.ItemArray[0].ToString());
            ConfigSettings.ChangeNode("LocationType", locationType);

            if (this.rbtnTOUDefault.Checked)
            {
                locationType = "Default";
                ConfigSettings.ChangeNode("TOUFileLocation", string.Empty); 
            }
            else
            {
                locationType = "Custom"; 
                if (txtCustomTOULocation.Text.Trim() == "")
                {
                    this.StatusMessage = "TOU file location can not be empty";
                    txtCustomTOULocation.Focus();
                    Application.DoEvents();
                    return;
                }
                ConfigSettings.ChangeNode("TOUFileLocation", txtCustomTOULocation.Text);
            }
            ConfigSettings.ChangeNode("TOULocationType", locationType);
            //for Dash Board Settings
            string strDashBoard = "";
            for (int index = 0; index <= chklstDashBoard.Items.Count - 1; index++)
            {
                if (chklstDashBoard.GetItemCheckState(index) == CheckState.Checked)
                    strDashBoard += "1";
                else
                    strDashBoard += "0";
            }
            ConfigSettings.ChangeNode("DashBoardParameters", strDashBoard);   
            //for Dash Board Settings
            this.StatusMessage = "Setting's Saved Successfully.";
            Application.DoEvents();
        }

        private void GeneralConfiguration_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

         private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                for (int index = 0; index <= chklstDashBoard.Items.Count - 1; index++)
                    chklstDashBoard.SetItemCheckState(index, CheckState.Checked);
            }
            else
            {
                for (int index = 0; index <= chklstDashBoard.Items.Count - 1; index++)
                    chklstDashBoard.SetItemCheckState(index, CheckState.Unchecked);
            }
        }

        private void btnTOUBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dlg = new FolderSelectDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo info = dlg.info;
                txtCustomTOULocation.Text = dlg.fullPath;
            }
        }

        private void rbtnTOUCustom_CheckedChanged(object sender, EventArgs e)
        {
            string locType = ConfigSettings.GetValue("TOULocationType");
            if (locType.Equals("Default"))
                txtCustomTOULocation.Text = string.Empty;
            else
                txtCustomTOULocation.Text = ConfigInfo.GetLocation();
            txtCustomTOULocation.Visible = true;
            btnTOUBrowse.Visible = true;
            txtDefaultTOULocation.Visible = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dlg = new FolderSelectDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo info = dlg.info;
                txtCustomCABLocation.Text = dlg.fullPath;
            }
        }

        private void rbtnTOUDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtDefaultTOULocation.Text = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\TOU\");
            txtCustomTOULocation.Visible = false;
            txtDefaultTOULocation.Visible = true;
            btnTOUBrowse.Visible = false;
        }

        private void rbtnCustom_CheckedChanged(object sender, EventArgs e)
        {
            string locType = ConfigSettings.GetValue("LocationType");
            if (locType.Equals("Default"))
                txtCustomCABLocation.Text = string.Empty;
            else
                txtCustomCABLocation.Text = ConfigInfo.GetLocation();
            txtCustomCABLocation.Visible = true;
            btnBrowse.Visible = true;
            txtDefaultCABLocation.Visible = false;
        }

        private void rbtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtDefaultCABLocation.Text = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\");
            txtCustomCABLocation.Visible = false;
            txtDefaultCABLocation.Visible = true;
            btnBrowse.Visible = false;
        }

        private void lngbCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void cboDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GeneralConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }
    }
}
