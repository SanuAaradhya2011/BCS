using System;
using System.Windows.Forms;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class AppInfo : MdiChildForm
    {
        public AppInfo()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplicationType_Load(object sender, EventArgs e)
        {
            ApplicationType appType = ConfigInfo.GetApplicationType();
            switch (appType)
            {
                case ApplicationType.DLMS_LTCT_650:
                    rbtnDLMS.Checked = true;
                    rbtnThreePhaseLTCT.Checked = true;
                    break;
                case ApplicationType.DLMS_RUBY_250:
                    rbtnDLMS.Checked = true;
                    rbtnThreePhaseWholeCurrent.Checked = true;
                    break;
                case ApplicationType.DLMS_STARLIGHT_150:
                    rbtnDLMS.Checked = true;
                    rbtnSinglePhase.Checked = true;
                    break;
                case ApplicationType.IEC_LTCT_650:
                    rbtnIEC.Checked = true;
                    rbtnThreePhaseLTCT.Checked = true;
                    break;
                case ApplicationType.IEC_RUBY_250:
                    rbtnIEC.Checked = true;
                    rbtnThreePhaseWholeCurrent.Checked = true;
                    break;
                case ApplicationType.IEC_STARLIGHT_150:
                    rbtnIEC.Checked = true;
                    rbtnSinglePhase.Checked = true;
                    break;
            }
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
                        if (rbtnIEC.Checked && rbtnSinglePhase.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "IEC_STARLIGHT_150");
            if (rbtnIEC.Checked && rbtnThreePhaseWholeCurrent.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "IEC_RUBY_250");
            if (rbtnIEC.Checked && rbtnThreePhaseLTCT.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "IEC_LTCT_650");
            if (rbtnDLMS.Checked && rbtnSinglePhase.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "DLMS_STARLIGHT_150");
            if (rbtnDLMS.Checked && rbtnThreePhaseWholeCurrent.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "DLMS_RUBY_250");
            if (rbtnDLMS.Checked && rbtnThreePhaseLTCT.Checked)
                ConfigSettings.ChangeNode("ApplicationType", "DLMS_LTCT_650");
            MessageBox.Show("Setting's Saved Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}