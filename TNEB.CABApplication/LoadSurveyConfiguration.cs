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
using CAB.Entity;
using CAB.Framework.Utility;

namespace CAB.UI
{
    public partial class LoadSurveyConfiguration : MdiChildForm 
    {
        public LoadSurveyConfiguration()
        {
            InitializeComponent();
        }

        private void LoadSurveyConfiguration_Load(object sender, EventArgs e)
        {
            SettingsBLL settingsBLL = new SettingsBLL();
            this.cmbLSConfiguration.DataSource = settingsBLL.GetLSConfig().Tables[0];
            this.cmbLSConfiguration.DisplayMember = "DisplayMember";
            this.cmbLSConfiguration.ValueMember = "ValueMember";

            string value = ConfigSettings.GetValue("LoadSurveyConfiguration");
            for (int counter = 0; counter < cmbLSConfiguration.Items.Count; counter++)
            {
                cmbLSConfiguration.SelectedIndex = counter;
                if ((((System.Data.DataRowView)(cmbLSConfiguration.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
        }

        private void btnLSConfigSave_Click(object sender, EventArgs e)
        {
            ConfigSettings.ChangeNode("LoadSurveyConfiguration", ((System.Data.DataRowView)(cmbLSConfiguration.Items[cmbLSConfiguration.SelectedIndex])).Row.ItemArray[0].ToString());
            this.StatusMessage = "Setting's Saved Successfully.";
        }

        private void btnLSConfigCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
