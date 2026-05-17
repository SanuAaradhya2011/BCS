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
    public partial class CMRIID : CABForm
    {
        public delegate void GetSubmittedValues(string cmriID);
        public event GetSubmittedValues OnValues_Submission;

        public CMRIID(bool fileTypeSelection)
        {
            InitializeComponent();
            rdbCMRI.Visible = fileTypeSelection;
            rdbDirect.Visible = fileTypeSelection;
        }

        private void SubmitValues(string cmriID)
        {
            if (OnValues_Submission != null)
            {
                OnValues_Submission(cmriID);
            }
        }

        private bool ValidateForm()
        {
            if (txtCMRIID.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the CMRI ID", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;    
        }

        private void CMRIID_Load(object sender, EventArgs e)
        {
            if (rdbCMRI.Visible && rdbDirect.Visible)
            {
                if (rdbCMRI.Checked)
                { txtCMRIID.Enabled = true; }
                else { txtCMRIID.Enabled = false; }
            }
            btnOK.Enabled = false;
        }

        private void txtCMRIID_TextChanged(object sender, EventArgs e)
        {
            if (txtCMRIID.Text.Trim().Length < 1)
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
       }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }
            SubmitValues(txtCMRIID.Text.Trim());
            this.Close();
            Application.DoEvents();
        }

        private void txtCMRIID_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ascii = Convert.ToInt16(e.KeyChar);
            if (!((ascii >= 97 && ascii <= 122) || (ascii >= 65 && ascii <= 90) || (ascii >= 48 && ascii <= 57) || (ascii == 8) || (ascii == 45) || (ascii == 46) || (ascii == 47) || (ascii == 32)))
            { e.Handled = true; }
        }

        private void rdbDirect_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDirect.Checked)
            {
                txtCMRIID.Text = "BCS";
                txtCMRIID.Enabled = false;
            }
            else
            {
                txtCMRIID.Text = "";
                txtCMRIID.Enabled = true;
            }
        }

        private void rdbCMRI_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCMRI.Checked)
            {
                txtCMRIID.Text = "";
                txtCMRIID.Enabled = true;
            }
            else
            {
                txtCMRIID.Text = "BCS";
                txtCMRIID.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SubmitValues(string.Empty);
            this.Cursor = Cursors.Default;
            this.Close();
        }
    }
}