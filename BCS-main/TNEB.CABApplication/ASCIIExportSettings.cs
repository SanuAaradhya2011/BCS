using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECFramework;
using CAB.Entity;
using CAB.BLL;

namespace CAB.UI
{
    public partial class ASCIIExportSettings : MdiChildForm
    { 
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL();
        private ASCIIExportItemSettings aSCIIExportItemSettings = null;
        private ASCIIExportSettingsEntity entity =null; 
        public ASCIIExportSettings()
        {
            InitializeComponent();
        } 
        private void LoadParameterForm(SettingTypes sTypes,bool flags)
        {
            aSCIIExportItemSettings = new ASCIIExportItemSettings();
            aSCIIExportItemSettings.StartPosition = FormStartPosition.CenterScreen;
            if (sTypes.Equals(SettingTypes.General))
                aSCIIExportItemSettings.SelectedColumn=entity.GeneralColumn; 
            else if (sTypes.Equals(SettingTypes.Billing))
                aSCIIExportItemSettings.SelectedColumn=entity.BillingColumn; 
            else if (sTypes.Equals(SettingTypes.Instant))
                aSCIIExportItemSettings.SelectedColumn = entity.InstantColumn; 
            else if (sTypes.Equals(SettingTypes.Tamper))
                aSCIIExportItemSettings.SelectedColumn = entity.TamperColumn;
            else if (sTypes.Equals(SettingTypes.LoadSurvey))
                aSCIIExportItemSettings.SelectedColumn = entity.LoadSurveyColumn;   
            aSCIIExportItemSettings.ParameterType = sTypes;
            aSCIIExportItemSettings.ShowDialog();
            if (sTypes.Equals(SettingTypes.General))
            {
                if (flags)
                {
                    entity.GeneralColumn = aSCIIExportItemSettings.SelectedColumn;
                    entity.GeneralDBColumn = aSCIIExportItemSettings.SelectedDBColumn;
                }
                else
                {
                    entity.GeneralColumn = "";
                    entity.GeneralDBColumn = "";
                }
            }
            else if (sTypes.Equals(SettingTypes.Billing))
            {
                if (flags)
                {
                    entity.BillingColumn = aSCIIExportItemSettings.SelectedColumn;
                    entity.BillingDBColumn = aSCIIExportItemSettings.SelectedDBColumn;
                }
                else
                {
                    entity.BillingColumn = "";
                    entity.BillingDBColumn = "";
                }  
            }
            else if (sTypes.Equals(SettingTypes.Instant))
            {
                if (flags)
                {
                    entity.InstantColumn = aSCIIExportItemSettings.SelectedColumn;
                    entity.InstantDBColum = aSCIIExportItemSettings.SelectedDBColumn;
                }
                else
                {
                    entity.InstantColumn = "";
                    entity.InstantDBColum = "";
                }   
            }
            else if (sTypes.Equals(SettingTypes.Tamper))
            {
                if (flags)
                {
                    entity.TamperColumn = aSCIIExportItemSettings.SelectedColumn;
                    entity.TamberDBColumn = aSCIIExportItemSettings.SelectedDBColumn;
                }
                else
                {
                    entity.TamperColumn = "";
                    entity.TamberDBColumn = "";
                } 
            }
            else if (sTypes.Equals(SettingTypes.LoadSurvey))
            {
                if (flags)
                {
                    entity.LoadSurveyColumn = aSCIIExportItemSettings.SelectedColumn;
                    entity.LoadSurveyDBColumn = aSCIIExportItemSettings.SelectedDBColumn;
                }
                else
                {
                    entity.LoadSurveyColumn = "";
                    entity.LoadSurveyDBColumn = "";
                }  
            }
        }

        private void btnGeneralDataParam_Click_1(object sender, EventArgs e)
        {
            LoadParameterForm(SettingTypes.General, chkBoxGeneralData.Checked);
        }

        private void btnBillingDataParam_Click(object sender, EventArgs e)
        {
            LoadParameterForm(SettingTypes.Billing, chkBoxBillingData.Checked);
        }

        private void btnInstantDataParam_Click(object sender, EventArgs e)
        {
            LoadParameterForm(SettingTypes.Instant, chkBoxInstantData.Checked);
        }

        private void btnTamperDataParam_Click(object sender, EventArgs e)
        {
            LoadParameterForm(SettingTypes.Tamper, chkBoxTamperData.Checked);
        }

        private void btnLoadSurveyDataParam_Click(object sender, EventArgs e)
        {
            LoadParameterForm(SettingTypes.LoadSurvey, chkBoxLoadSurveyData.Checked);
        }

        private void chkBoxGeneralData_CheckedChanged(object sender, EventArgs e)
        {
            btnGeneralDataParam.Enabled = chkBoxGeneralData.Checked;
            if (chkBoxGeneralData.Checked == false)
            {
                entity.GeneralColumn = "";
                entity.GeneralDBColumn = "";
            }
        }

        private void chkBoxBillingData_CheckedChanged(object sender, EventArgs e)
        {
            btnBillingDataParam.Enabled = chkBoxBillingData.Checked;
            if (chkBoxBillingData.Checked == false)
            {
                entity.BillingColumn = "";
                entity.BillingDBColumn = "";
            }
        }

        private void chkBoxInstantData_CheckedChanged(object sender, EventArgs e)
        {
            btnInstantDataParam.Enabled = chkBoxInstantData.Checked;
            if (chkBoxInstantData.Checked == false)
            {
                entity.InstantColumn = "";
                entity.InstantDBColum = "";
            }
        }

        private void chkBoxTamperData_CheckedChanged(object sender, EventArgs e)
        {
            btnTamperDataParam.Enabled = chkBoxTamperData.Checked;
            if (chkBoxTamperData.Checked == false)
            {
                entity.TamperColumn = "";
                entity.TamberDBColumn = "";
            }
        }

        private void chkBoxLoadSurveyData_CheckedChanged(object sender, EventArgs e)
        {
            btnLoadSurveyDataParam.Enabled = chkBoxLoadSurveyData.Checked;
            if (chkBoxLoadSurveyData.Checked == false)
            {
                entity.LoadSurveyColumn = "";
                entity.LoadSurveyDBColumn = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            gbFormate.Text = "View export settings";
            btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
        }

        private void ASCIIExportSettings_Load(object sender, EventArgs e)
        { 
            this.Text = "ASCII Export Settings";
            this.StatusMessage = string.Empty;
            DataSet dataSet = asciiExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "Asciiexportsettings_ID";
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        { 
            string fileName = txtFileName.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                this.StatusMessage = "Format name can not be left blank.";
                txtFileName.Focus();
                return;
            }
            string delimeter = cmbDataSeparator.Text;
            if (string.IsNullOrEmpty(delimeter))
            {
                this.StatusMessage = "Please select delimeter.";
                cmbDataSeparator.Focus();
                return;
            }
            if (string.IsNullOrEmpty(entity.GeneralColumn) && string.IsNullOrEmpty(entity.BillingColumn) && string.IsNullOrEmpty(entity.InstantColumn) && string.IsNullOrEmpty(entity.TamperColumn) && string.IsNullOrEmpty(entity.LoadSurveyColumn) ) 
            {
                this.StatusMessage = "Please select Parameter"; 
                return;
            }
            entity.FileName = fileName;
            entity.Delimeter = delimeter;
            if (asciiExportSettingsBLL.IsValidFile(fileName) && entity.Asciiexportsettings_ID == 0)
            {
                this.StatusMessage = "File already exist.";
                txtFileName.Focus();
                return;
            }

            if (entity.Asciiexportsettings_ID == 0)
            {
                asciiExportSettingsBLL.InsertData(entity);
                this.StatusMessage = "File saved successfully.";
            }
            else
            {
                asciiExportSettingsBLL.UpdateData(entity);
                this.StatusMessage = "File modified successfully.";
            }
            LoadList();
            gbFormate.Text = "View export settings";
            btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
        }
        private void LoadList()
        {
            DataSet dataSet = asciiExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "Asciiexportsettings_ID";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            entity = new ASCIIExportSettingsEntity();
            gbFormate.Text = "New export settings";
            gbFormate.Enabled = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            chkBoxBillingData.Checked = false;
            chkBoxGeneralData.Checked = false;
            chkBoxInstantData.Checked = false;
            chkBoxTamperData.Checked = false;
            chkBoxLoadSurveyData.Checked = false;
            txtFileName.Text = "";
            this.StatusMessage = string.Empty;
            cmbDataSeparator.SelectedIndex = 0;
            txtFileName.Enabled = true;
            txtFileName.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        { 
                string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
                entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;
                if (entity != null)
                {
                    chkBoxBillingData.Checked = (!string.IsNullOrEmpty(entity.BillingColumn)) ? true : false;
                    chkBoxGeneralData.Checked = (!string.IsNullOrEmpty(entity.GeneralColumn)) ? true : false;
                    chkBoxInstantData.Checked = (!string.IsNullOrEmpty(entity.InstantColumn)) ? true : false;
                    chkBoxTamperData.Checked = (!string.IsNullOrEmpty(entity.TamperColumn)) ? true : false;
                    chkBoxLoadSurveyData.Checked = (!string.IsNullOrEmpty(entity.LoadSurveyColumn)) ? true : false;
                    txtFileName.Text = entity.FileName;
                    for (int i = 0; i < cmbDataSeparator.Items.Count; i++)
                    {
                        cmbDataSeparator.SelectedIndex = i;
                        if (cmbDataSeparator.Text.Trim().Equals(entity.Delimeter.Trim()))
                            break;
                    }
                }
                gbFormate.Text = "View export settings";
                btnSave.Visible = btnCancel.Visible = gbFormate.Enabled = false;
            } 

        private void btnDelete_Click(object sender, EventArgs e)
        { 
            this.StatusMessage = string.Empty; 
            if (lstFile.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are you sure to delete this file", "Delete Customer Settings", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
                int index = lstFile.SelectedIndex;
                
                string fileId = ((System.Data.DataRowView)(lstFile.Items[index])).Row.ItemArray[0].ToString();
                asciiExportSettingsBLL.DeleteSettings(Convert.ToInt32(fileId));
                this.StatusMessage = "File deleted successfully.";
                if (index != 0)
                    index--;
                LoadList();
                if (lstFile.Items.Count > 0)
                    lstFile.SelectedIndex = index;
            } 
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstFile.Items.Count == 0)
                return;
            string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
            entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;
            if (entity != null)
            {
                chkBoxBillingData.Checked = (!string.IsNullOrEmpty(entity.BillingColumn)) ? true : false;
                chkBoxGeneralData.Checked = (!string.IsNullOrEmpty(entity.GeneralColumn)) ? true : false;
                chkBoxInstantData.Checked = (!string.IsNullOrEmpty(entity.InstantColumn)) ? true : false;
                chkBoxTamperData.Checked = (!string.IsNullOrEmpty(entity.TamperColumn)) ? true : false;
                chkBoxLoadSurveyData.Checked = (!string.IsNullOrEmpty(entity.LoadSurveyColumn)) ? true : false;
                txtFileName.Text = entity.FileName;
                for (int i = 0; i < cmbDataSeparator.Items.Count; i++)
                {
                    cmbDataSeparator.SelectedIndex = i;
                    if (cmbDataSeparator.Text.Trim().Equals(entity.Delimeter.Trim()))
                        break;
                }
            }
            this.StatusMessage = string.Empty;
            gbFormate.Text = "Edit export settings";
            gbFormate.Enabled = true;
            btnSave.Visible = btnCancel.Visible = true;
            txtFileName.Enabled = false;
        }
    }
}
