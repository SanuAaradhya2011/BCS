using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using System.ComponentModel;

namespace CAB.UI
{
    public partial class GSMScheduleInfo : UserControl
    { 
        SettingsBLL settingsBLL = new SettingsBLL();
        public delegate void ChannelStatusChanged(string msg);
        public event ChannelStatusChanged OnChannelStatusChanged;
        public delegate void CancelClickHandler(object sender, EventArgs e);
        public event CancelClickHandler OnCancelClick;
        public delegate void SaveClickHandler(object sender, EventArgs e);
        public event SaveClickHandler OnSaveClick;
        private GSMScheduleBLL sgmScheduleBLL = new GSMScheduleBLL();
        private GSMScheduleEntity gsmScheduleEntity = null;
        public GSMScheduleInfo()
        {
            InitializeComponent();
        }
        private string message;
        public string StatusMessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                if (OnChannelStatusChanged != null)
                {
                    OnChannelStatusChanged(message);
                }
            }
        }
        private void GSMScheduleInfo_Load(object sender, EventArgs e)
        {
            this.cboHr.DataSource = settingsBLL.GetHours().Tables[0];
            this.cboHr.DisplayMember = "DisplayMember";
            this.cboHr.ValueMember = "ValueMember";

            this.cboMi.DataSource = settingsBLL.GetMinutes().Tables[0];
            this.cboMi.DisplayMember = "DisplayMember";
            this.cboMi.ValueMember = "ValueMember";

            lblPeriod.Enabled = false;
            cboWeekDay.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string scheduleName = txtScheduleName.Text.Trim();
            string period = "";
            string dayName = "";
            string dayNumber = "0";
            string parameter = string.Empty;
            if (rbtnDaily.Checked)
            {
                period = "D";
            }
            else if (rbtnMonthly.Checked)
            {
                period = "M";
                dayNumber = ((System.Data.DataRowView)(cboWeekDay.Items[cboWeekDay.SelectedIndex])).Row.ItemArray[1].ToString();
            }
            else if (rbtnWeekly.Checked)
            {
                period = "W";
                dayName = ((System.Data.DataRowView)(cboWeekDay.Items[cboWeekDay.SelectedIndex])).Row.ItemArray[1].ToString();
            }

            string dates = DateUtility.DateTimeToLong(dtpDate.Value).ToString();
            string times = ((System.Data.DataRowView)(cboHr.Items[cboHr.SelectedIndex])).Row.ItemArray[0].ToString() + ":" + ((System.Data.DataRowView)(cboMi.Items[cboMi.SelectedIndex])).Row.ItemArray[0].ToString(); ;
            for(int i = 0 ;i< clbParameter.Items.Count;i++)
            {
                if (clbParameter.GetItemChecked(i))
                    parameter = parameter + clbParameter.Items[i].ToString() + "|";
            }
            if (!string.IsNullOrEmpty(parameter))
                parameter = parameter.Substring(0, parameter.Length - 1);
            int status = 0;
            if (rboActive.Checked)
                status = 1;
            gsmScheduleEntity.Schedule_Name = scheduleName;
            gsmScheduleEntity.Schedule_Period = period;
            gsmScheduleEntity.Period_DayName = dayName;
            gsmScheduleEntity.Period_DayNumber = Convert.ToInt32(dayNumber);
            gsmScheduleEntity.CreationDate =  Convert.ToInt64(dates);
            gsmScheduleEntity.CreationTime = times;
            gsmScheduleEntity.Schedule_Parameter = parameter; 
            gsmScheduleEntity.Status = status;
            if (string.IsNullOrEmpty(scheduleName))
            {
                this.StatusMessage = "schedule Name can't be empty.";
                Application.DoEvents();
                txtScheduleName.Focus();
                return;
            }
             if (string.IsNullOrEmpty(parameter))
             {
                 this.StatusMessage = "Please select parameter.";
                 Application.DoEvents();
                 clbParameter.Focus();
                 return;
             }
             if (sgmScheduleBLL.ValidateSchedule(gsmScheduleEntity))
             {
                 this.StatusMessage = "Schedule Name already exist.";
                 Application.DoEvents();
                 return;
             }
            if (gsmScheduleEntity.GSMSchedule_ID == 0)
            { 
                 
                gsmScheduleEntity = sgmScheduleBLL.InsertData(gsmScheduleEntity) as GSMScheduleEntity;
                if (gsmScheduleEntity.GSMSchedule_ID != 0)
                {
                    this.StatusMessage = "Data Saved successfully";
                    Application.DoEvents();
                }
            }
            else
            { 
                if (sgmScheduleBLL.UpdateData(gsmScheduleEntity))
                {
                    this.StatusMessage = "Data modified successfully";
                    Application.DoEvents();
                }
            }
            if (OnSaveClick != null)
            {
                OnSaveClick(sender, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelClick != null)
            {
                OnCancelClick(sender, e);
            }
        }

        [Browsable(false)]
        public void ClearData()
        {
            gsmScheduleEntity = new GSMScheduleEntity();
            txtScheduleName.Text = string.Empty;
            rbtnDaily.Checked = true;
            dtpDate.Value = System.DateTime.Now;
            cboHr.SelectedIndex = 0;
            cboMi.SelectedIndex = 0;
            for(int i = 0 ;i< clbParameter.Items.Count;i++)
            {
                if (clbParameter.GetItemChecked(i))
                    clbParameter.SetItemChecked(i, false);
            }
            rboActive.Checked = true;
            this.txtScheduleName.Focus();
            txtScheduleName.Enabled = true;
            groupBox1.Text = "Create Schedule";
        }
        [Browsable(false)]
        public void EditData(IEntity entity)
        { 
            if (entity == null)
                return;
            gsmScheduleEntity = entity as GSMScheduleEntity;
             txtScheduleName.Text = gsmScheduleEntity.Schedule_Name;
             txtScheduleName.Enabled = false;
             if (gsmScheduleEntity.Schedule_Period == "D")
                 rbtnDaily.Checked=true;
             else if (gsmScheduleEntity.Schedule_Period == "M")
             {
                 rbtnMonthly.Checked = true;
                 for (int i = 0; i < cboWeekDay.Items.Count; i++)
                 {
                     if(Convert.ToInt32((((System.Data.DataRowView)(cboWeekDay.Items[i])).Row.ItemArray[1]))==Convert.ToInt32(gsmScheduleEntity.Period_DayNumber))
                     {
                         cboWeekDay.SelectedIndex = i;
                         break;
                     }
                 } 
             }
             else if (gsmScheduleEntity.Schedule_Period == "W")
             {
                 rbtnWeekly.Checked = true;
                 for (int i = 0; i < cboWeekDay.Items.Count; i++)
                 {
                     if ((((System.Data.DataRowView)(cboWeekDay.Items[i])).Row.ItemArray[0]).Equals(gsmScheduleEntity.Period_DayName))
                     {
                         cboWeekDay.SelectedIndex = i;
                         break;
                     }
                 } 
             }

             dtpDate.Value = DateUtility.LongToDateTime(gsmScheduleEntity.CreationDate);
                if (!string.IsNullOrEmpty(gsmScheduleEntity.CreationTime))
                {   
                    string[] times = gsmScheduleEntity.CreationTime.Split(':');
                    for (int i = 0; i < cboHr.Items.Count; i++)
                    {
                        if (Convert.ToInt32((((System.Data.DataRowView)(cboHr.Items[i])).Row.ItemArray[0])) == Convert.ToInt32(times[0].ToString()))
                        {
                            cboHr.SelectedIndex = i;
                            break;
                        }
                    }
                    for (int i = 0; i < cboMi.Items.Count; i++)
                    {
                        if (Convert.ToInt32((((System.Data.DataRowView)(cboMi.Items[i])).Row.ItemArray[0])) == Convert.ToInt32(times[1].ToString()))
                        {
                            cboMi.SelectedIndex = i;
                            break;
                        }
                    } 
                }
                if (!string.IsNullOrEmpty(gsmScheduleEntity.Schedule_Parameter))
                {
                    string[] param = gsmScheduleEntity.Schedule_Parameter.Split('|');
                    for (int j = 0; j < param.Length; j++)
                    {
                        int i = 0;
                        bool flag = false;
                        for (; i < clbParameter.Items.Count; i++)
                        {
                            string data=clbParameter.Items[i].ToString();
                            if (data.Equals(param[j]))
                            {
                                flag = true;
                                break;
                            } 
                        }
                        if(flag)
                        clbParameter.SetItemChecked(i, true);
                    }
                }
            if (gsmScheduleEntity.Status == 1)
                rboActive.Checked = true;
            else
                rboInactive.Checked = true;
            this.txtScheduleName.Focus();
            groupBox1.Text = "Edit Schedule"; 
        } 

        private void rbtnDaily_CheckedChanged(object sender, EventArgs e)
        {
            lblPeriod.Enabled = false;
            cboWeekDay.Enabled = false;
        }

        private void rbtnWeekly_CheckedChanged(object sender, EventArgs e)
        {
            lblPeriod.Enabled = true;
            cboWeekDay.Enabled = true;
            lblPeriod.Text = "Week Day";
            this.cboWeekDay.DataSource = settingsBLL.GetDayNames().Tables[0];
            this.cboWeekDay.DisplayMember = "DisplayMember";
            this.cboWeekDay.ValueMember = "ValueMember";

        }

        private void rbtnMonthly_CheckedChanged(object sender, EventArgs e)
        {
            lblPeriod.Enabled = true;
            cboWeekDay.Enabled = true;
            lblPeriod.Text = "Day"; 
            this.cboWeekDay.DataSource = settingsBLL.GetDays().Tables[0];
            this.cboWeekDay.DisplayMember = "DisplayMember";
            this.cboWeekDay.ValueMember = "ValueMember";
        }

         
    }
}
