using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.UI;
using CAB.UI.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CABApplication.Scheduling
{
    public partial class SchedularForm : MdiChildForm
    {
        MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
        SchedulingStrategyContext schedulingStrategy = null;
        GSMTaskBLL gsmTaskBLL = new GSMTaskBLL();
        GSMGroupBLL gsmGroupBLL = new GSMGroupBLL();
        List<GSMTaskEntity> colGSMTaskEntity = null;
        private string mode = string.Empty;
        private GSMTaskEntity taskEntity;
        private const string DATEFORMAT = "dd/MM/yyyy hh:mm:ss tt";
        private const string COLON = ":";
        private const string COMMA = ",";
        private const string TASK = "Task ";
        private const string SAVEDSUCCESSFULLY = " saved successfully";
        private const string COULDNOTBESAVED = " could not be saved";
        private const string GROUPNAME = "Group Name";
        private const string GROUPID = "Group ID";
        private const string VALIDATE = "validate";
        DataSet dataSet;
        
        public SchedularForm()
        {
            InitializeComponent();

            calStartDate.Value = System.DateTime.Now;
            SetGroupDropDown();
            
        }
        public class Panel3D : Panel
    {
        public int BorderRadius { get; set; } = 20;
        public Color ShadowColor { get; set; } = Color.FromArgb(163, 177, 198);
        public Color HighlightColor { get; set; } = Color.White;

        public Panel3D()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(224, 229, 236); // Soft 3D Grey
            this.Padding = new Padding(10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(5, 5, this.Width - 15, this.Height - 15);
            GraphicsPath path = GetRoundedRect(rect, BorderRadius);

            // Draw Light Shadow (Top-Left)
            using (Pen pen = new Pen(HighlightColor, 5))
            {
                g.DrawPath(pen, path);
            }

            // Draw Dark Shadow (Bottom-Right)
            using (Pen pen = new Pen(ShadowColor, 2))
            {
                rect.Offset(3, 3);
                GraphicsPath shadowPath = GetRoundedRect(rect, BorderRadius);
                g.DrawPath(pen, shadowPath);
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
        private void SetGroupDropDown()
        {
            dataSet = gsmGroupBLL.ListGroupData();
            if (dataSet != null)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    ddGroup.DataSource = dataSet.Tables[0];
                    ddGroup.DisplayMember = GROUPNAME;
                    ddGroup.ValueMember = GROUPID;
                }
            }
        }

        public string Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        public GSMTaskEntity TaskEntity
        {
            get
            {
                return taskEntity;
            }
            set
            {
                taskEntity = value;
            }
        }

        private void rdbEveryDay_Click(object sender, EventArgs e)
        {
            numericUpDownDaily.Enabled = false;
            lblDailyHour.Enabled = false;
        }

        private void rdbDailyWeekdays_Click(object sender, EventArgs e)
        {
            numericUpDownDaily.Enabled = false;
            lblDailyHour.Enabled = false;
        }

        private void rdbDailyCustom_Click(object sender, EventArgs e)
        {
            numericUpDownDaily.Enabled = true;
            lblDailyHour.Enabled = true;
        }

        private void numericUpDownDaily_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownDaily.Value == 1)
                lblDailyHour.Text = "day";
            else
                lblDailyHour.Text = "days";
        }

        private void chkMonthlySelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonthlySelectAll.Checked == true)
            {
                chkJan.Checked = true;
                chkFeb.Checked = true;
                chkMar.Checked = true;
                chkApr.Checked = true;
                chkMay.Checked = true;
                chkJun.Checked = true;
                chkJul.Checked = true;
                chkAug.Checked = true;
                chkSep.Checked = true;
                chkOct.Checked = true;
                chkNov.Checked = true;
                chkDec.Checked = true;

                //chkJan.Enabled = false;
                //chkFeb.Enabled = false;
                //chkMar.Enabled = false;
                //chkApr.Enabled = false;
                //chkMay.Enabled = false;
                //chkJun.Enabled = false;
                //chkJul.Enabled = false;
                //chkAug.Enabled = false;
                //chkSep.Enabled = false;
                //chkOct.Enabled = false;
                //chkNov.Enabled = false;
                //chkDec.Enabled = false;
            }
            else
            {
                chkJan.Checked = false;
                chkFeb.Checked = false;
                chkMar.Checked = false;
                chkApr.Checked = false;
                chkMay.Checked = false;
                chkJun.Checked = false;
                chkJul.Checked = false;
                chkAug.Checked = false;
                chkSep.Checked = false;
                chkOct.Checked = false;
                chkNov.Checked = false;
                chkDec.Checked = false;

                chkJan.Enabled = true;
                chkFeb.Enabled = true;
                chkMar.Enabled = true;
                chkApr.Enabled = true;
                chkMay.Enabled = true;
                chkJun.Enabled = true;
                chkJul.Enabled = true;
                chkAug.Enabled = true;
                chkSep.Enabled = true;
                chkOct.Enabled = true;
                chkNov.Enabled = true;
                chkDec.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FillUIDetailsInEntity(GSMTaskEntity gsmTaskEntity)
        {
            gsmTaskEntity.taskName = txtTaskName.Text;
            gsmTaskEntity.groupId = Convert.ToInt32(ddGroup.SelectedValue.ToString());
            gsmTaskEntity.CreationDateTime = DateTime.Now.ToString(DATEFORMAT);
            gsmTaskEntity.startDate = calStartDate.Value.ToShortDateTimeCABFormat();
            gsmTaskEntity.CalendarDate = calStartDate.Value;
            gsmTaskEntity.startTime = ddHour.SelectedItem.ToString() + COLON + ddMinutes.SelectedItem.ToString();
            gsmTaskEntity.IntervalInDays = Convert.ToInt32(numericUpDownDaily.Value);
            gsmTaskEntity.taskRetries = Convert.ToInt32(numericUpDownRetry.Value);
        }
        private void FillDailyTaskDetails(GSMTaskEntity gsmTaskEntity)
        {
            FillDailyTasksToBeRepeated(gsmTaskEntity);
            schedulingStrategy = new SchedulingStrategyContext(new DailySchedulingStartegy());
            gsmTaskEntity.startDate = schedulingStrategy.ScheduleTask(gsmTaskEntity).ToShortDateTimeCABFormat();
        }
        private void FillWeeklyTaskDetails(GSMTaskEntity gsmTaskEntity)
        {
            int[] weekDayList;
            FillWeeklyTasksToBeRepeated(gsmTaskEntity);
            FillDaysInWeek(out weekDayList);
            gsmTaskEntity.WeekDayList = weekDayList;
            schedulingStrategy = new SchedulingStrategyContext(new WeeklySchedulingStrategy());
            gsmTaskEntity.startDate = schedulingStrategy.ScheduleTask(gsmTaskEntity).ToShortDateTimeCABFormat();
        }
        private void FillMonthlyTaskDetails(GSMTaskEntity gsmTaskEntity)
        {
            int[] monthNoList;
            string strMonth = string.Empty;
            FillMonthsInYear(out monthNoList);
            gsmTaskEntity.IntervalInDays = Convert.ToInt32(numericUpDownMonthly.Value);
            gsmTaskEntity.MonthNoList = monthNoList;
            schedulingStrategy = new SchedulingStrategyContext(new MonthlySchedulingStrategy());
            gsmTaskEntity.startDate = schedulingStrategy.ScheduleTask(gsmTaskEntity).ToShortDateTimeCABFormat();
            strMonth = FillMonthlyTasksToBeRepeated(gsmTaskEntity);
            PromptUserForDaysMismatchInMonth(strMonth, gsmTaskEntity.IntervalInDays);
        }
        private void FillOneTimeOnlyTaskDetails(GSMTaskEntity gsmTaskEntity)
        {
            schedulingStrategy = new SchedulingStrategyContext(new OneTimeOnlySchedulingStrategy());
            gsmTaskEntity.tasksToBeRepeated = string.Empty;
            gsmTaskEntity.startDate = schedulingStrategy.ScheduleTask(gsmTaskEntity).ToShortDateTimeCABFormat();
        }
        private bool CheckIfAnotherTaskScheduled(GSMTaskEntity gsmTaskEntity)
        {
            bool isAnotherTaskScheduled = false;
            string startTime = ddHour.Text + COLON + ddMinutes.Text;
            if (colGSMTaskEntity.Count(x => x.startDate == gsmTaskEntity.startDate && x.startTime == startTime) > 0)
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskScheduling) + gsmTaskEntity.startDate + " " + startTime, "BCS");
                ddHour.Focus();
                isAnotherTaskScheduled = true;
            }
            return isAnotherTaskScheduled;
        }
        private void FillJobNames(GSMTaskEntity gsmTaskEntity)
        {
            StringBuilder strJobs = new StringBuilder();
            strJobs.Append(JobType.General.ToString());

            //If Instantaneous is selected.
            if (chkInstantaneous.Checked)
            {
                strJobs.Append("," + JobType.Instantaneous.ToString());
            }

            if (chkBilling.Checked)
            {
                strJobs.Append("," + JobType.Billing.ToString());
            }

            //If load survey is checked, append the load survey literal to the jobs
            if (chkLoadSurvey.Checked)
            {
                if (rdLoadSurveyComplete.Checked)
                {
                    strJobs.Append("," + JobType.LoadSurveyComplete.ToString());
                }
                else if (rdLoadSurveyPartialFrom.Checked)
                {
                    strJobs.Append("," + JobType.LoadSurveyPartialFrom.ToString());
                    gsmTaskEntity.LoadSurveyFromDate = dpCalendarFromDate.Value;
                    gsmTaskEntity.LoadSurveyToDate = dpCalendarToDate.Value;
                }
                else
                {
                    strJobs.Append("," + JobType.LoadSurveyPartial.ToString());
                }
            }

            // If Tamper is checked, append the tamper literal to the jobs
            if (chkTamper.Checked)
            {
                strJobs.Append("," + JobType.Tamper.ToString());
            }

            if (chkMidnight.Checked)
            {
                strJobs.Append("," + JobType.Midnight.ToString());
            }
            if (chkMeterConfig.Checked)
            {
                strJobs.Append("," + JobType.MeterConfiguration.ToString());
            }
            
           
            //if (strJobs.ToString().EndsWith(COMMA))
            //{
            //    gsmTaskEntity.jobNames = strJobs.ToString().Substring(0, strJobs.ToString().Length - 1);
            //}
            //else
            //{
               gsmTaskEntity.jobNames = strJobs.ToString();
       //     }
       }


        private void UpdateToolTip(bool result,GSMTaskEntity gsmTaskEntity)
        {
            if (result)
                mainForm.toolStripStatusLabel.Text = TASK + gsmTaskEntity.taskName + SAVEDSUCCESSFULLY;
            else
                mainForm.toolStripStatusLabel.Text = TASK + gsmTaskEntity.taskName + COULDNOTBESAVED;
        }
        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            CreateTask();
        }

        private void CreateTask()
        {
            GSMTaskEntity gsmTaskEntity = new GSMTaskEntity();
            if (validate())
            {
                FillUIDetailsInEntity(gsmTaskEntity);
                if (rdbDaily.Checked)
                {
                    FillDailyTaskDetails(gsmTaskEntity);
                }
                else if (rdbWeekly.Checked)
                {
                    FillWeeklyTaskDetails(gsmTaskEntity);
                }
                else if (rdbMonthly.Checked)
                {
                    FillMonthlyTaskDetails(gsmTaskEntity);
                }
                else if (rdbOneTime.Checked)
                {
                    FillOneTimeOnlyTaskDetails(gsmTaskEntity);
                }
                if (CheckIfAnotherTaskScheduled(gsmTaskEntity))
                {
                    return;
                }
                FillJobNames(gsmTaskEntity);
                UpdateToolTip(gsmTaskBLL.InsertGSMTask(gsmTaskEntity), gsmTaskEntity);
            }
        }

        private bool validate()
        {
            bool result = false;
            colGSMTaskEntity = gsmTaskBLL.getAllSchedulesTasks(VALIDATE);
            // Validates the Task Name
            if (txtTaskName.Text == "")
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskNameEmpty), BCSConstants.BCS);
                txtTaskName.Focus();
                return result;
            }

            if (ddGroup.SelectedIndex <= -1)
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.GroupSelected), BCSConstants.BCS);
                ddGroup.Focus();
                return result;
            }

            //Check for the Task Name Duplicacy
            if (colGSMTaskEntity.Count(x => x.taskName == txtTaskName.Text) > 0)
            {
                MessageBox.Show("Task Name: " + txtTaskName.Text + EnumUtil.stringValueOf(ValidationMsgs.TaskAlreadyPresent), "BCS");
                txtTaskName.Focus();
                return result;
            }

            // Validates Tasks to be performed
            if (rdbDaily.Checked == false && rdbWeekly.Checked == false && rdbMonthly.Checked == false && rdbOneTime.Checked == false)
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskPerformed), BCSConstants.BCS);
                return result;
            }

            if (ddHour.Text == "")
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateHour), BCSConstants.BCS);
                ddHour.Focus();
                return result;
            }

            if (ddMinutes.Text == "")
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateMinutes), BCSConstants.BCS);
                ddMinutes.Focus();
                return result;
            }

            if ((calStartDate.Value.Date < System.DateTime.Now.Date) && (rdbDaily.Checked || rdbOneTime.Checked))
            {
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateStartDate), BCSConstants.BCS);
                calStartDate.Focus();
                return result;
            }

            if (rdbOneTime.Checked || rdbDaily.Checked)
            {
                if (calStartDate.Value.Date == DateTime.Now.Date)
                {
                    if (Convert.ToInt32(ddHour.SelectedItem.ToString()) >= DateTime.Now.Hour)
                    {
                        if (Convert.ToInt32(ddHour.SelectedItem.ToString()) == DateTime.Now.Hour)
                            if (Convert.ToInt32(ddMinutes.SelectedItem.ToString()) < DateTime.Now.Minute)
                            {
                                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.StartTimeLessCurrentTime), BCSConstants.BCS);
                                return result;
                            }
                    }
                    else
                    {
                        MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.StartTimeLessCurrentTime), BCSConstants.BCS);
                        return result;
                    }
                }
            }

            if (rdbDaily.Checked == true)
            {
                if (rdbEveryDay.Checked == false && rdbDailyCustom.Checked == false && rdbDailyWeekdays.Checked == false)
                {
                    MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateDailyTask), BCSConstants.BCS);
                    return result;
                }
            }

            if (rdbWeekly.Checked == true)
            {
                if (chkMonday.Checked == false && chkTuesday.Checked == false && chkWednesday.Checked == false && chkThursday.Checked == false && chkFriday.Checked == false && chkSaturday.Checked == false && chkSunday.Checked == false)
                {
                    MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateWeeklyTask), BCSConstants.BCS);
                    return result;
                }
            }

            if (rdbMonthly.Checked == true)
            {
                if (chkMonthlySelectAll.Checked == false && chkJan.Checked == false && chkFeb.Checked == false && chkMar.Checked == false && chkApr.Checked == false && chkMay.Checked == false && chkJun.Checked == false && chkJul.Checked == false && chkAug.Checked == false && chkSep.Checked == false && chkOct.Checked == false && chkNov.Checked == false && chkDec.Checked == false)
                {
                    MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateMonthlyTask), BCSConstants.BCS);
                    return result;
                }
            }
            //validation for date range of load survey
            if (chkLoadSurvey.Checked && rdLoadSurveyPartialFrom.Checked)
            {
                if (dpCalendarFromDate.Value >= dpCalendarToDate.Value)
                {
                    MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.FromDateCanNotBeGreatherThanToDate), BCSConstants.BCS);
                    return result;
                }
            }
            return true;
        }

        private void SchedularForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.toolStripStatusLabel.Text = "";

            if (mainForm.ActivateThisChild("TaskManagerForm") == false)
            {
                TaskManagerForm taskManager = new TaskManagerForm();
                taskManager.MdiParent = mainForm;
                taskManager.WindowState = FormWindowState.Maximized;
                taskManager.Show();
            }
        }

        private void SchedularForm_Load(object sender, EventArgs e)
        {
            rdbEveryDay.Checked = true;
            decimal repeatInDays = 0;
            GSMTaskEntity gsmTaskEntity = null;
            if (!UtilityDetails.IECSupport)
            {
                chkInstantaneous.Checked = false;
                chkInstantaneous.Enabled = true;
                chkBilling.Checked = false;
                chkBilling.Enabled = true;
            }
            if (!chkLoadSurvey.Checked)
            {
                gbLoadSurveyDateRange.Visible = false;
                rdLoadSurveyPartial.Checked = true;
                //set the date to today's date
                dpCalendarFromDate.Value = DateTime.Now;
                dpCalendarToDate.Value = DateTime.Now;
                dpCalendarFromDate.Value = Convert.ToDateTime(dpCalendarFromDate.Value.ToShortDateString() + " 00:00:00");
                dpCalendarToDate.Value = Convert.ToDateTime(dpCalendarToDate.Value.ToShortDateString() + " 23:59:59");
            }
            if (Mode == "Edit")
            {
                if (taskEntity != null)
                {
                    if (!string.IsNullOrEmpty(taskEntity.StartHour))
                    {
                        ddHour.SelectedItem = taskEntity.StartHour;
                        ddMinutes.SelectedItem = taskEntity.StartMinute;
                    }
                    txtTaskName.Text = taskEntity.taskName;
                    SetGroupDropDown();
                    ddGroup.SelectedValue = taskEntity.groupId;

                    if (taskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Daily))
                    {
                        rdbDaily.Checked = true;
                        if (taskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(DailyTask.EveryDay))
                        {
                            rdbEveryDay.Checked = true;
                        }
                        else if (taskEntity.tasksToBeRepeated == EnumUtil.stringValueOf(DailyTask.WeekDays))
                        {
                            rdbDailyWeekdays.Checked = true;
                        }
                        else if (decimal.TryParse(taskEntity.tasksToBeRepeated, out repeatInDays))
                        {
                            rdbDailyCustom.Checked = true;
                            //Selection of Custom days
                            numericUpDownDaily.Value = repeatInDays;
                        }
                    }
                    if (taskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Weekly))
                    {
                        rdbWeekly.Checked = true;
                    }
                    if (taskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.Monthly))
                    {
                        rdbMonthly.Checked = true;
                    }

                    if (taskEntity.taskType == EnumUtil.stringValueOf(GSMTasksType.OneTimeOnly))
                        rdbOneTime.Checked = true;
                }
            }
        }

        private void rdbDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDaily.Checked)
            {
                lblStartDate.Visible = true;
                calStartDate.Visible = true;
                pnlDaily.Visible = true;
                pnlWeekly.Visible = false;
                pnlMonthly.Visible = false;
                // Disable the partial from functionality for one time only task
                //EnablePartialFrom(false);
            }
        }

        private void rdbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbWeekly.Checked)
            {
                lblStartDate.Visible = false;
                calStartDate.Visible = false;
                pnlDaily.Visible = false;
                pnlWeekly.Visible = true;
                pnlMonthly.Visible = false;
                // Disable the partial from functionality for one time only task
                //EnablePartialFrom(false);
            }
        }

        private void rdbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMonthly.Checked)
            {
                lblStartDate.Visible = false;
                calStartDate.Visible = false;
                pnlDaily.Visible = false;
                pnlWeekly.Visible = false;
                pnlMonthly.Visible = true;
                // Disable the partial from functionality for one time only task
                //EnablePartialFrom(false);
            }
        }

        private void rdbOneTime_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOneTime.Checked)
            {
                lblStartDate.Visible = true;
                calStartDate.Visible = true;
                pnlDaily.Visible = false;
                pnlWeekly.Visible = false;
                pnlMonthly.Visible = false;
                // Enable the partial from functionality for one time only task
                //EnablePartialFrom(true);
            }
        }
        ///// <summary>
        ///// The function enables or disable the partial from radio button and calendars
        ///// according to the argument passed in the function
        ///// </summary>
        ///// <param name="enable"></param>
        //private void EnablePartialFrom(bool enable)
        //{            
        //    rdLoadSurveyPartialFrom.Enabled = enable;
        //    dpCalendarToDate.Enabled = enable;
        //    dpCalendarFromDate.Enabled = enable;
        //    // if other than one time only is checked then uncheck the load survey partial from radio button as well as 
        //    //the Task save button will interpret that load durvey is enabled and checked.
        //    if (!enable)
        //    {
        //        rdLoadSurveyPartialFrom.Checked = false;
        //     }
        //}
        private bool FillDaysInWeek(out int[] weekNoList)
        {
            int counter = 0;
            weekNoList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            if (chkMonday.Checked)
            {
                counter++;
                weekNoList[0] = 1;
            }
            if (chkTuesday.Checked)
            {
                counter++;
                weekNoList[1] = 2;
            }
            if (chkWednesday.Checked)
            {
                counter++;
                weekNoList[2] = 3;
            }
            if (chkThursday.Checked)
            {
                counter++;
                weekNoList[3] = 4;
            }
            if (chkFriday.Checked)
            {
                counter++;
                weekNoList[4] = 5;
            }
            if (chkSaturday.Checked)
            {
                counter++;
                weekNoList[5] = 6;
            }
            if (chkSunday.Checked)
            {
                counter++;
                weekNoList[6] = 7;
            }
            if (counter > 1)
                return false;
            else
                return true;
        }

        private bool FillMonthsInYear(out int[] highestMonthNo)
        {
            int counter = 0;
            highestMonthNo = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (chkJan.Checked)
            {
                counter++;
                highestMonthNo[0] = 1;
            }
            if (chkFeb.Checked)
            {
                counter++;
                highestMonthNo[1] = 2;
            }
            if (chkMar.Checked)
            {
                counter++;
                highestMonthNo[2] = 3;
            }
            if (chkApr.Checked)
            {
                counter++;
                highestMonthNo[3] = 4;
            }
            if (chkMay.Checked)
            {
                counter++;
                highestMonthNo[4] = 5;
            }
            if (chkJun.Checked)
            {
                counter++;
                highestMonthNo[5] = 6;
            }
            if (chkJul.Checked)
            {
                counter++;
                highestMonthNo[6] = 7;
            }
            if (chkAug.Checked)
            {
                counter++;
                highestMonthNo[7] = 8;
            }
            if (chkSep.Checked)
            {
                counter++;
                highestMonthNo[8] = 9;
            }
            if (chkOct.Checked)
            {
                counter++;
                highestMonthNo[9] = 10;
            }
            if (chkNov.Checked)
            {
                counter++;
                highestMonthNo[10] = 11;
            }
            if (chkDec.Checked)
            {
                counter++;
                highestMonthNo[11] = 12;
            }
            if (counter > 1)
                return false;
            else
                return true;
        }

        private int GetMinNo(int[] highestMonthNo)
        {
            int minNo = 0;
            for (int counter = 0; counter < highestMonthNo.Length; counter++)
            {
                if (counter == 0 && highestMonthNo[counter] > 0)
                {
                    minNo = highestMonthNo[counter];
                }
                if (counter > 0)
                {
                    if (minNo == 0 && highestMonthNo[counter] > 0)
                    {
                        minNo = highestMonthNo[counter];

                    }
                    if (highestMonthNo[counter] < minNo && highestMonthNo[counter] > 0)
                    {
                        minNo = highestMonthNo[counter];
                    }
                }
            }
            return minNo;
        }

        private int GetMinDayNo(int[] weekDayList)
        {
            int minDayNo = 0;
            for (int counter = 0; counter < weekDayList.Length; counter++)
            {
                if (counter == 0)
                {
                    minDayNo = weekDayList[counter];
                }
                if (counter > 0)
                {
                    if (minDayNo == 0 && weekDayList[counter] > 0)
                    {
                        minDayNo = weekDayList[counter];
                    }
                    if (weekDayList[counter] < minDayNo && weekDayList[counter] > 0)
                    {
                        minDayNo = weekDayList[counter];
                    }
                }
            }
            return minDayNo;
        }
        private void PromptUserForDaysMismatchInMonth(String strMonth,int dayOfMonth)
        {
            bool isMonthWithLessDaysSelected = false;
            string[] arrSelectedMonthNames = strMonth.Replace(" ", "").Split(',');
            for (int i = 1; i <= 12; i++)
            {
                if ((DateTime.DaysInMonth(DateTime.Now.Year, i) < dayOfMonth) &&
                    arrSelectedMonthNames.Contains((new DateTime(DateTime.Now.Year, i, 1)).ToString("MMM")))
                {
                    isMonthWithLessDaysSelected = true;
                    break;
                }
            }
            if (isMonthWithLessDaysSelected)
            {
                if (arrSelectedMonthNames.Length == 1)
                {
                    MessageBox.Show("The selected month contains less number of days than the date value selected. The scheduled task will be rescheduled to the last day of the month.", "Task Rescheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("One or more of the selected months contain less number of days than the date value selected. For those month(s) the scheduled task will be rescheduled to the last day of the month.", "Task Rescheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void FillDailyTasksToBeRepeated(GSMTaskEntity gsmTaskEntity)
        {
            if (rdbEveryDay.Checked == true)
            {
                //Selection of Every day option.
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(DailyTask.EveryDay); //"Every day";

            }
            else if (rdbDailyWeekdays.Checked == true)
            {

                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(DailyTask.WeekDays); //"Weekdays";
            }
            else if (rdbDailyCustom.Checked == true)
            {
                gsmTaskEntity.IsCustom = true;
                //Selection of Custom days
                gsmTaskEntity.tasksToBeRepeated = numericUpDownDaily.Value.ToString();
            }
        }
        private void FillWeeklyTasksToBeRepeated(GSMTaskEntity gsmTaskEntity)
        {
            //If 'Weekly' is selected as taskType, then the user can select the individual days

            // Selecting the individual day, by selecting the radio buttons against the given days.
            // Whenever a checkbox is selected for a day, the value of that checkbox is appended to the string strWeekDays
            if (chkMonday.Checked)
            {
                // Appending Monday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Monday);
            }

            else if (chkTuesday.Checked)
            {
                // Appending Tuesday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Tuesday);
            }

            else if (chkWednesday.Checked)
            {
                // Appending Wednesday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Wednesday);
            }

            else if (chkThursday.Checked)
            {
                // Appending Thursday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Thursday);
            }

            else if (chkFriday.Checked)
            {
                // Appending Friday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Friday);
            }

            else if (chkSaturday.Checked)
            {
                // Appending Saturday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Saturday);
            }

            else if (chkSunday.Checked)
            {
                // Appending Sunday
                gsmTaskEntity.tasksToBeRepeated = EnumUtil.stringValueOf(WeeklyTask.Sunday);
            }
            if (gsmTaskEntity.tasksToBeRepeated.EndsWith(","))
            {
                // Removing the comma from strWeekDays and assigning its value to strDays
                gsmTaskEntity.tasksToBeRepeated = gsmTaskEntity.tasksToBeRepeated.Substring(0, gsmTaskEntity.tasksToBeRepeated.Length - 1);
            }

            // Appending 'Every' with the value of strDays and assigning it to the property tasksToBeRepeated

        }
        private string FillMonthlyTasksToBeRepeated(GSMTaskEntity gsmTaskEntity)
        {
            StringBuilder strMonths = new StringBuilder();
            string strMonth = string.Empty;
            if (chkMonthlySelectAll.Checked)
            {
                gsmTaskEntity.tasksToBeRepeated = numericUpDownMonthly.Value.ToString() + "," + EnumUtil.stringValueOf(MonthlyTask.AllMonths);
                strMonth = EnumUtil.stringValueOf(MonthlyTask.AllMonths);
            
            }
            else
            {
                // Selection the individual months, by selecting the checkboxes against the given months.
                // Whenever a checkbox is selected for a month, the value of that checkbox is appended to the string strMonths
                if (chkJan.Checked)
                {
                    // Appending Jan
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Jan));
                }

                if (chkFeb.Checked)
                {
                    // Appending Feb
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Feb));
                }

                if (chkMar.Checked)
                {
                    //// Appending Mar
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Mar));
                }

                if (chkApr.Checked)
                {
                    //// Appending Apr
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Apr));
                }

                if (chkMay.Checked)
                {
                    // Appending May
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.May));
                }

                if (chkJun.Checked)
                {
                    // Appending Jun
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Jun));
                }

                if (chkJul.Checked)
                {
                    // Appending Jul
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Jul));
                }

                if (chkAug.Checked)
                {
                    // Appending Aug
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Aug));
                }

                if (chkSep.Checked)
                {
                    // Appending Sep
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Sep));
                }

                if (chkOct.Checked)
                {
                    // Appending Oct
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Oct));
                }

                if (chkNov.Checked)
                {
                    // Appending Nov
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Nov));
                }

                if (chkDec.Checked)
                {
                    // Appending Dec
                    strMonths.Append(EnumUtil.stringValueOf(MonthlyTask.Dec));
                }
                if (strMonths.ToString().EndsWith(","))
                {
                    // Removing the comma from strMonths and assigning its value to strMonth
                    strMonth = strMonths.ToString().Substring(0, strMonths.ToString().Length - 1);
                }
                else
                {
                    strMonth = strMonths.ToString();
                }
               gsmTaskEntity.tasksToBeRepeated = numericUpDownMonthly.Value.ToString() + "," + strMonth;
   
            }
            return strMonth;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void chkLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLoadSurvey.Checked)
            {
                gbLoadSurveyDateRange.Visible = true;
            }
            else
            {
                gbLoadSurveyDateRange.Visible = false;
            }
        }

        private void rdLoadSurveyComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLoadSurveyComplete.Checked)
            {
                dpCalendarFromDate.Enabled = false;
                dpCalendarToDate.Enabled = false;
                rdbMonthly.Enabled = true;
                rdbDaily.Enabled = true;
                rdbWeekly.Enabled = true;
                rdbOneTime.Enabled = true;
            }
        }

        private void rdLoadSurveyPartial_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLoadSurveyPartialFrom.Checked)
            {
                dpCalendarFromDate.Enabled = true;
                dpCalendarToDate.Enabled = true;
                rdbMonthly.Enabled = false;
                rdbDaily.Enabled = false;
                rdbWeekly.Enabled = false;
                rdbOneTime.Enabled = true;
                rdbOneTime.Checked = true;
            }
        }

        private void rdLoadSurveyPartial_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdLoadSurveyPartial.Checked)
            {
                dpCalendarFromDate.Enabled = false;
                dpCalendarToDate.Enabled = false;
                rdbMonthly.Enabled = true;
                rdbDaily.Enabled = true;
                rdbWeekly.Enabled = true;
                rdbOneTime.Enabled = true;
            }
        }


        private void ddGroup_SelectedIndexChanged(object sender, EventArgs e)
        {         
            // Uncommenting the below statement : Bug #260389
            textBoxType.Text = dataSet.Tables[0].Rows[ddGroup.SelectedIndex]["CommunicationType"].ToString();


            //set default values
            grpJobSelection.Visible = true;
            gbLoadSurveyDateRange.Visible = true;
            //specific check for FTP 
            if (dataSet.Tables[0].Rows[ddGroup.SelectedIndex]["CommunicationType"].ToString().ToUpper() == "FTP")
            {
                grpJobSelection.Visible = false;
                gbLoadSurveyDateRange.Visible = false;
            }
           
                
            //// BCS for GPRS first release does not support Tamper and LoadSurvey
            //if (dataSet.Tables[0].Rows[ddGroup.SelectedIndex]["CommunicationType"].ToString().ToUpper() == "GPRS")
            //{
            //    chkLoadSurvey.Checked = false;
            //    chkLoadSurvey.Visible = false;
            //    //gbLoadSurveyDateRange.Visible = false;
            //    chkTamper.Checked = false;
            //    chkTamper.Visible = false;
            //    rdLoadSurveyPartial.Checked = true;
            //    dpCalendarFromDate.Value = DateTime.Now;
            //    dpCalendarToDate.Value = DateTime.Now;
            //}
            //else
            //{
            //    chkLoadSurvey.Visible = true;
            //    chkTamper.Visible = true;
            //    //gbLoadSurveyDateRange.Visible = true;
              
            //}

        }

        private void txtTaskName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblStartDate_Click(object sender, EventArgs e)
        {

        }
    }

}