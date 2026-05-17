#region NameSpaces
using System;
using System.IO;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using Generic3PhaseCommunication;
#endregion
namespace CAB.UI
{
    /// <summary>
    /// Used for Gneral systme related configurations , like file naming convention , 
    /// </summary>
    public partial class GeneralConfiguration : MdiChildForm
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        #endregion

        #region Constructer
        public GeneralConfiguration()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralConfiguration_Load(object sender, EventArgs e)
        {
            //Fill load survey days .
            //SarkarA code change start 20180109 // Increase Limit from 180 to 278 for PGVCL
            for (int index = 1; index <= 278; index++)
            {
                cmbLoadSurveyDays.Items.Add(index.ToString());

            }
            //SarkarA code change end 20180109 
            //Fill no of retries .
            for (int index = 1; index <= 15; index++)
            {
                cmbNoOfRetries.Items.Add(index.ToString());

            }
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
            else if (fileNamingConvention.Trim().Equals("Default+MeterID"))
                rbtDNWM.Checked = true;

            cmbLoadSurveyDays.SelectedItem = ConfigSettings.GetValue("LoadSurveyDays");
            cmbNoOfRetries.SelectedItem = ConfigSettings.GetValue("NoOfRetries");
            if (ConfigSettings.GetValue("ChkNumPowFail") == "1")
                chkBoxNumPowFail.Checked = true;
            else
                chkBoxNumPowFail.Checked = false;


            if (ConfigSettings.GetValue("ChkHideNamePlDetails") == "1")
                chkBoxHideNameplate.Checked = true;
            else
                chkBoxHideNameplate.Checked = false;


            if (ConfigSettings.GetValue("ChkPowerOnOffDurationFormat") == "1")
                ChkPowerOnOffDurationFormat.Checked = true;
            else
                ChkPowerOnOffDurationFormat.Checked = false;
            


            tabControlSystemConfiguration.TabPages.Remove(tbDashBoard);            
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
          //SaveReadoutFile objSavefilepath= new SaveReadoutFile();
          //objSavefilepath.SaveRaoutfilePath(string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\")); 
            if(rboFNC1.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Default");
            else if (rboFNC2.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Default+System");
            else if (rboFNC3.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Custom");
            else if (rbtDNWM.Checked)
                ConfigSettings.ChangeNode("FileNamingConvention", "Default+MeterID");

            ConfigSettings.ChangeNode("DateFormat", ((System.Data.DataRowView)(cboDate.Items[cboDate.SelectedIndex])).Row.ItemArray[0].ToString());
            ConfigSettings.ChangeNode("LocationType", locationType);

            ConfigSettings.ChangeNode("LoadSurveyDays", cmbLoadSurveyDays.SelectedItem.ToString());
            ConfigSettings.ChangeNode("NoOfRetries", cmbNoOfRetries.SelectedItem.ToString());
            
            if (chkBoxNumPowFail.Checked)
                ConfigSettings.ChangeNode("ChkNumPowFail", "1");
            else
                ConfigSettings.ChangeNode("ChkNumPowFail", "0");


            if (ChkPowerOnOffDurationFormat.Checked)
                ConfigSettings.ChangeNode("ChkPowerOnOffDurationFormat", "1");
            else
                ConfigSettings.ChangeNode("ChkPowerOnOffDurationFormat", "0");


            if (chkBoxHideNameplate.Checked)
                ConfigSettings.ChangeNode("ChkHideNamePlDetails", "1");
            else
                ConfigSettings.ChangeNode("ChkHideNamePlDetails", "0");            
                     
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralConfiguration_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dlg = new FolderSelectDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo info = dlg.info;
                txtCustomCABLocation.Text = dlg.fullPath;
            }
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            txtDefaultCABLocation.Text = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), @"\CAB Readout\");
            txtCustomCABLocation.Visible = false;
            txtDefaultCABLocation.Visible = true;
            btnBrowse.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngbCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneralConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
