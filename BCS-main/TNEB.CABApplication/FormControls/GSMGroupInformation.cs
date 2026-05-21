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
    public partial class GSMGroupInformation : UserControl
    {
        public delegate void ChannelStatusChanged(string msg);
        public event ChannelStatusChanged OnChannelStatusChanged;
        public delegate void CancelClickHandler(object sender, EventArgs e);
        public event CancelClickHandler OnCancelClick;
        public delegate void SaveClickHandler(object sender, EventArgs e);
        public event SaveClickHandler OnSaveClick;
        private GSMScheduleBLL gsmScheduleBLL = new GSMScheduleBLL();
        GSMGroupScheduleBLL gsmGroupScheduleBLL = new GSMGroupScheduleBLL();
        private GSMGroupScheduleEntity gsmGroupScheduleEntity = null;
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        public GSMGroupInformation()
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
            this.cboSchedule.DataSource = gsmScheduleBLL.ComboListDataSet().Tables[0];
            this.cboSchedule.DisplayMember = "DisplayMember";
            this.cboSchedule.ValueMember = "ValueMember";

            DataSet ds = meterDataBLL.ListAllMeterID();
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            this.lstAllMeter.Items.Add(dr[0].ToString());
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string scheduleGroupName = txtGroupName.Text.Trim();
            string readingDates = DateUtility.DateTimeToLong(dtpDate.Value).ToString();
            string scheduleId = ((System.Data.DataRowView)(cboSchedule.Items[cboSchedule.SelectedIndex])).Row.ItemArray[0].ToString();
            string selectedMeter = string.Empty;
            for (int i = 0; i < lstSelectedMeter.Items.Count; i++)
            {
                selectedMeter = selectedMeter + lstSelectedMeter.Items[i].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(selectedMeter))
                selectedMeter = selectedMeter.Substring(0, selectedMeter.Length - 1);
            if (string.IsNullOrEmpty(scheduleId))
                scheduleId = "0";
            gsmGroupScheduleEntity.Group_Name = scheduleGroupName;
            gsmGroupScheduleEntity.GSMSchedule_ID = Convert.ToInt64(scheduleId);
            gsmGroupScheduleEntity.Meter_ID = selectedMeter;
            gsmGroupScheduleEntity.StartReadingDate = Convert.ToInt64(readingDates);
            if (string.IsNullOrEmpty(scheduleGroupName))
            {
                this.StatusMessage = "Group Name can't be empty.";
                Application.DoEvents();
                txtGroupName.Focus();
                return;
            }
            if (gsmGroupScheduleEntity.GSMSchedule_ID == 0)
            {
                this.StatusMessage = "Please select Schedule.";
                Application.DoEvents();
                cboSchedule.Focus();
                return;
            }
            if (string.IsNullOrEmpty(selectedMeter))
            {
                this.StatusMessage = "Please select Meter.";
                Application.DoEvents();
                lstSelectedMeter.Focus();
                return;
            }
            if (gsmGroupScheduleBLL.ValidateGroup(gsmGroupScheduleEntity))
            {
                this.StatusMessage = "Group Name already exist.";
                Application.DoEvents();
                return;
            }
            if (gsmGroupScheduleEntity.GSMGroupSchedule_ID == 0)
            {

                gsmGroupScheduleEntity = gsmGroupScheduleBLL.InsertData(gsmGroupScheduleEntity) as GSMGroupScheduleEntity;
                if (gsmGroupScheduleEntity.GSMGroupSchedule_ID != 0)
                {
                    this.StatusMessage = "Data Saved successfully";
                    Application.DoEvents();
                }
            }
            else
            {
                if (gsmGroupScheduleBLL.UpdateData(gsmGroupScheduleEntity))
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
            gsmGroupScheduleEntity = new GSMGroupScheduleEntity();
            txtGroupName.Text = string.Empty;
            dtpDate.Value = System.DateTime.Now;
            if(cboSchedule.Items.Count>0)
            cboSchedule.SelectedIndex = 0;
            lstSelectedMeter.Items.Clear();
            this.txtGroupName.Focus();
            txtGroupName.Enabled = true;
            grpHead.Text = "Create Schedule";
        }
        [Browsable(false)]
        public void EditData(IEntity entity)
        {
            if (entity == null)
                return;
            gsmGroupScheduleEntity = entity as GSMGroupScheduleEntity;


            txtGroupName.Text = gsmGroupScheduleEntity.Group_Name;
            txtGroupName.Enabled = false;

            for (int i = 0; i < cboSchedule.Items.Count; i++)
            {
                if (Convert.ToInt32((((System.Data.DataRowView)(cboSchedule.Items[i])).Row.ItemArray[0])) == Convert.ToInt32(gsmGroupScheduleEntity.GSMSchedule_ID))
                {
                    cboSchedule.SelectedIndex = i;
                    break;
                }
            }
            dtpDate.Value = DateUtility.LongToDateTime(gsmGroupScheduleEntity.StartReadingDate);
            lstSelectedMeter.Items.Clear();
            if (!string.IsNullOrEmpty(gsmGroupScheduleEntity.Meter_ID))
            {
                string[] metersId = gsmGroupScheduleEntity.Meter_ID.Split(',');
                for (int i = 0; i < metersId.Length; i++)
                    lstSelectedMeter.Items.Add(metersId[i]);
            }
            this.txtGroupName.Focus();
            grpHead.Text = "Edit Schedule";
        }

        private void lngButton8_Click(object sender, EventArgs e)
        {
            int idx = lstAllMeter.SelectedIndex;
            foreach (object item in lstAllMeter.SelectedItems)
            {
                if (meterDataBLL.CheckSimExist(item.ToString()))
                    lstSelectedMeter.Items.Add(item);
                else
                {
                    this.StatusMessage = "Sim Number not available for this meter";
                    Application.DoEvents();
                    return;
                }
            }
            for (int counter = 0; counter < lstSelectedMeter.Items.Count; counter++)
            {
                int i = 0;
                bool Flag = false;
                for (; i < lstAllMeter.Items.Count; i++)
                {
                    if (lstAllMeter.Items[i].ToString().Equals(lstSelectedMeter.Items[counter].ToString()))
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                    lstAllMeter.Items.RemoveAt(i);
            }
            if (idx != 0)
                idx -= 1;
            if (lstAllMeter.Items.Count >= idx && lstAllMeter.Items.Count != 0 && idx > 0)
                lstAllMeter.SelectedIndex = idx;
        }

        private void lngButton6_Click(object sender, EventArgs e)
        {
            foreach (object item in lstSelectedMeter.SelectedItems)
                lstAllMeter.Items.Add(item);
            for (int SelectedIndex = lstSelectedMeter.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--)
                lstSelectedMeter.Items.RemoveAt(lstSelectedMeter.SelectedIndex);
            if (lstSelectedMeter.Items.Count != 0)
            {
                lstSelectedMeter.SelectedIndex = 0;
                lstAllMeter.SelectedIndex = -1;
            }
        }

        private void lngButton7_Click(object sender, EventArgs e)
        {
            if (lstAllMeter.Items.Count == 0)
                return;
            foreach (object item in lstAllMeter.Items)
            {
                if (meterDataBLL.CheckSimExist(item.ToString()))
                    lstSelectedMeter.Items.Add(item);
                else
                {
                    this.StatusMessage = "Sim Number not available for this meter";
                    Application.DoEvents(); 
                }
            } 
            lstAllMeter.Items.Clear();
        }

        private void lngButton5_Click(object sender, EventArgs e)
        {
            if (lstSelectedMeter.Items.Count == 0)
                return;
            foreach (object item in lstSelectedMeter.Items)
                lstAllMeter.Items.Add(item);
            lstSelectedMeter.Items.Clear();
        }

        private void lstAllMeter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
        }
    }
}

