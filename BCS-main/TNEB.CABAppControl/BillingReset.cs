using System;
using System.Windows.Forms;

namespace CABAppControl
{
    public partial class BillingReset : UserControl
    {
        public BillingReset()
        {
            InitializeComponent();
            //Default values in Billing Reset tab for Meter Configurations.
            lblBillingType.Visible = false;
            rbtnAuto.Visible = false;
            rbtnManual.Visible = false;
            //rbtnMonthly.Checked = true;
            rbtnOddMonth.Checked = true;
            rbtnAuto.Checked = true;
            
            for (int i = 0; i <= 59; i++)
            {
                cmbSelectMinutes.Items.Add(i);
            }
            for (int i = 0; i <= 255; i++)
            {
                cmbResetLockoutdays.Items.Add(i);
            }
            
        }

        private void gbManual_Enter(object sender, EventArgs e)
        {

        }

        private void rbtnAuto_CheckedChanged(object sender, EventArgs e)
        {
            //Manual mode is disabled when Automode is checked
            gbAutoMode.Enabled = true;
           // gbManual.Enabled = false;
            
        }

        private void rbtnManual_CheckedChanged(object sender, EventArgs e)
        {
            //Auto mode is disabled when manual is checked
            //gbManual.Enabled = true;
            //gbAutoMode.Enabled = false;
           
        }

        private void cmbSelectMinutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cmbModeofBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            //When Mode of Billing is selected as "End of Month"
            if (cmbModeofBilling.SelectedItem == "End of Month")
            {
                rbtnEvenMonth.Enabled = false;
                rbtnOddMonth.Enabled = false;
                rbtnMonthly.Enabled = false;
                cmbSelectDay.Enabled = false;
                cmbSelectHour.Enabled = false;
                cmbSelectMinutes.Enabled = false;
                cmbSelectDay.SelectedIndex = 0;
                cmbSelectHour.SelectedIndex = 0;
                cmbSelectMinutes.SelectedIndex = 0;
                cmbResetLockoutdays.SelectedIndex = 0;
            }
            //When Mode of Billing is selected as "User Defined"
            else if (cmbModeofBilling.SelectedItem == "User Defined")
            {
                rbtnEvenMonth.Enabled = true;
                rbtnOddMonth.Enabled = true;
                rbtnMonthly.Enabled = true;
                cmbSelectDay.Enabled = true;
                cmbSelectHour.Enabled = true;
                cmbSelectMinutes.Enabled = true;
                cmbSelectDay.SelectedIndex = 0;
                cmbSelectHour.SelectedIndex = 0;
                cmbSelectMinutes.SelectedIndex = 0;
                cmbResetLockoutdays.SelectedIndex = 0;
            }
        }
    }
}
