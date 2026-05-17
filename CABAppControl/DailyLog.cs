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
    public partial class DailyLog : UserControl
    {
        public DailyLog()
        {
            InitializeComponent();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeKwh_CheckedChanged(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeKwh_CheckedChanged(sender, e);
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked == true)
            {
                chkCumulativeKVAh.Checked = true;
                chkCumulativeKVARhLag.Checked = true;
                chkCumulativeKVARhLead.Checked = true;
                chkCumulativeKwh.Checked = true;
                chkDailyMD1.Checked = true;
                chkDailyMD2.Checked = true;
            }
            else
            {
                chkCumulativeKVAh.Checked = false;
                chkCumulativeKVARhLag.Checked = false;
                chkCumulativeKVARhLead.Checked = false;
                chkCumulativeKwh.Checked = false;
                chkDailyMD1.Checked = false;
                chkDailyMD2.Checked = false;
            }
        }

        private void chkCumulativeKwh_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkDailyMD1.Checked == true && chkDailyMD2.Checked && chkCumulativeKwh.Checked == true && chkCumulativeKVAh.Checked == true
                && chkCumulativeKVARhLag.Checked == true && chkCumulativeKVARhLead.Checked == true)
                chkSelectAll.Checked = true;
            else
                chkSelectAll.Checked = false;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }

        private void chkCumulativeKVAh_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeKwh_CheckedChanged(sender, e);
        }

        private void chkDailyMD1_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeKwh_CheckedChanged(sender, e);
        }

        private void chkDailyMD2_CheckedChanged(object sender, EventArgs e)
        {
            chkCumulativeKwh_CheckedChanged(sender, e);
        }
    }
}
