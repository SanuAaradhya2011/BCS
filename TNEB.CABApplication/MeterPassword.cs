using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class MeterPassword : CABForm
    {
        public delegate void GetSubmittedValues(string meterPassword, int ctRatio);
        public event GetSubmittedValues OnValues_Submission;

        public MeterPassword(bool isCTRatioRequired)
        {
            InitializeComponent();
            lblCTRatio.Visible = isCTRatioRequired;
            txtCTRatio.Visible = isCTRatioRequired;
            lblCTRatioValidRange.Visible = isCTRatioRequired;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SubmitValues(string.Empty, 0);
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int ctRatio =0;
            if (!ValidateForm())
            {
                return;
            }

            if (!Int32.TryParse(txtCTRatio.Text.Trim(), out ctRatio))
                ctRatio = 0;
            SubmitValues(txtPassword.Text, ctRatio);
            this.Close();
            Application.DoEvents();
        }

        private void SubmitValues(string meterPassword, int ctRatio)
        {
            if (OnValues_Submission != null)
            {
                OnValues_Submission(meterPassword, ctRatio);
            }
        }

        private bool ValidateForm()
        {
            int tempVal = 0;
            if (txtCTRatio.Visible == true || txtCTRatio.Text.Length != 0)
            {
                if (!Int32.TryParse(txtCTRatio.Text.Trim(), out tempVal))
                {
                    MessageBox.Show("Incorrect CT Ratio.","BCS",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (tempVal < 1 || tempVal > 240)
                    {
                        MessageBox.Show("Incorrect CT Ratio (valid range 1-240).", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        return false;
                    }
                }
            }

            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please enter password.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;    
        }

        private void MeterPassword_Load(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length < 8)
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

    }
}