using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CABAppControl
{
    public partial class MDWithIP : UserControl
    {
        public MDWithIP()
        {
            InitializeComponent();
        }

        private void cmbkWDemandInt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbkWDemandType.SelectedIndex == 0)
            {
                cmbkWDemandSubInt.Items.Clear();
                cmbkWDemandSubInt.Text = "0";
                cmbkWDemandSubInt.Enabled = false;
            }
            else
            {
                if (cmbkWDemandInt.SelectedIndex == 0)
                {
                    cmbkWDemandSubInt.Items.Clear();
                    cmbkWDemandSubInt.Items.Add("5");
                    cmbkWDemandSubInt.SelectedIndex = 0;
                    cmbkWDemandSubInt.Enabled = false;
                }
                else if (cmbkWDemandInt.SelectedIndex == 1 || cmbkWDemandInt.SelectedIndex == 2)
                {
                    cmbkWDemandSubInt.Items.Clear();
                    cmbkWDemandSubInt.Items.Add("5");
                    cmbkWDemandSubInt.Items.Add("10");
                    cmbkWDemandSubInt.Items.Add("15");
                    cmbkWDemandSubInt.Enabled = true;
                }
            }

        }

        private void cmbkVADemandInt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbkVADemandType.SelectedIndex == 0)
            {
                cmbkVADemandSubInt.Items.Clear();
                cmbkVADemandSubInt.Text = "0";
                cmbkVADemandSubInt.Enabled = false;
            }
            else
            {
                if (cmbkVADemandInt.SelectedIndex == 0)
                {
                    cmbkVADemandSubInt.Items.Clear();
                    cmbkVADemandSubInt.Items.Add("5");
                    cmbkVADemandSubInt.SelectedIndex = 0;
                    cmbkVADemandSubInt.Enabled = false;
                }
                else if (cmbkVADemandInt.SelectedIndex == 1 || cmbkVADemandInt.SelectedIndex == 2)
                {
                    cmbkVADemandSubInt.Items.Clear();
                    cmbkVADemandSubInt.Items.Add("5");
                    cmbkVADemandSubInt.Items.Add("10");
                    cmbkVADemandSubInt.Items.Add("15");
                    cmbkVADemandSubInt.Enabled = true;
                }
            }
        }

        private void cmbkWDemandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbkWDemandType.SelectedIndex == 0)
            {
                cmbkWDemandSubInt.Items.Clear();
                cmbkWDemandSubInt.Text = "0";
                cmbkWDemandSubInt.Enabled = false;
            }
            else
            {
                cmbkWDemandSubInt.Enabled = true;
                cmbkWDemandSubInt.Items.Clear();
                cmbkWDemandSubInt.Items.Add("5");
                cmbkWDemandSubInt.Items.Add("10");
                cmbkWDemandSubInt.Items.Add("15");
                if (cmbkWDemandInt.SelectedIndex == 0)
                {
                    cmbkWDemandSubInt.Items.Clear();
                    cmbkWDemandSubInt.Items.Add("5");
                    cmbkWDemandSubInt.SelectedIndex = 0;
                    cmbkWDemandSubInt.Enabled = false;
                }
                else if (cmbkWDemandInt.SelectedIndex == 1 || cmbkWDemandInt.SelectedIndex == 2)
                {
                    cmbkWDemandSubInt.Items.Clear();
                    cmbkWDemandSubInt.Items.Add("5");
                    cmbkWDemandSubInt.Items.Add("10");
                    cmbkWDemandSubInt.Items.Add("15");
                }
            }

        }

        private void cmbkVADemandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbkVADemandType.SelectedIndex == 0)
            {
                cmbkVADemandSubInt.Items.Clear();
                cmbkVADemandSubInt.Text = "0";
                cmbkVADemandSubInt.Enabled = false;
            }
            else
            {
                cmbkVADemandSubInt.Enabled = true;
                cmbkVADemandSubInt.Items.Clear();
                cmbkVADemandSubInt.Items.Add("5");
                cmbkVADemandSubInt.Items.Add("10");
                cmbkVADemandSubInt.Items.Add("15");
                if (cmbkVADemandInt.SelectedIndex == 0)
                {
                    cmbkVADemandSubInt.Items.Clear();
                    cmbkVADemandSubInt.Items.Add("5");
                    cmbkVADemandSubInt.SelectedIndex = 0;
                    cmbkVADemandSubInt.Enabled = false;
                }
                else if (cmbkVADemandInt.SelectedIndex == 1 || cmbkVADemandInt.SelectedIndex == 2)
                {
                    cmbkVADemandSubInt.Items.Clear();
                    cmbkVADemandSubInt.Items.Add("5");
                    cmbkVADemandSubInt.Items.Add("10");
                    cmbkVADemandSubInt.Items.Add("15");
                }
            }
        }
    }
}
