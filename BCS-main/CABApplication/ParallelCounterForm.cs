using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.IECFramework.Utility;
using Hunt.EPIC.Logging;

namespace CABApplication
{
    public partial class ParallelCounterForm : Form
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ParallelCounterForm).ToString());

        public ParallelCounterForm()
        {
            InitializeComponent();
        }

        private bool ValidateControl()
        {
            bool result = false;
            try
            {                
                Int16 iVar;
                if (Int16.TryParse(txtThreadCount.Text.Trim(), out iVar))
                {
                    result = true;
                }
                else
                {
                    txtThreadCount.Text = ConfigSettings.GetValue("ThreadPoolSize");
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ValidateControl()", ex);
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateControl();
                ConfigSettings.ChangeNode("IsTCPOneToManyParallel", rbParallel.Checked.ToString());
                ConfigSettings.ChangeNode("ThreadPoolSize", txtThreadCount.Text);
                this.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnSave_Click(object sender, EventArgs e)", ex);
            }
        }

        private void ParallelCounterForm_Load(object sender, EventArgs e)
        {
            try
            {
                rbParallel.Checked = Convert.ToBoolean(ConfigSettings.GetValue("IsTCPOneToManyParallel"));
                txtThreadCount.Text = ConfigSettings.GetValue("ThreadPoolSize");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ParallelCounterForm_Load(object sender, EventArgs e)", ex);
            }
        }

        private void rbSerial_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonCheckedChanged();
        }

        private void rbParallel_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonCheckedChanged();
        }

        private void RadioButtonCheckedChanged()
        {
            if (rbParallel.Checked)
            {
                txtThreadCount.Visible = true;
                lblThreadCount.Visible = true;
            }
            else
            {
                txtThreadCount.Visible = false;
                lblThreadCount.Visible = false;
            }

        }

    }
}
