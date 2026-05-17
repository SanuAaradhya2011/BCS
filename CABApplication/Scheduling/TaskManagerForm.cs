using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.UI;
using CAB.UI.Controls;
using SerialCommunication;
using System.Security;
using System.ServiceProcess;
using System.Threading;
using Hunt.EPIC.Logging;

namespace CABApplication.Scheduling
{
    public partial class TaskManagerForm : MdiChildForm
    {
        GSMTaskBLL gsmTaskBLL = new GSMTaskBLL();
        MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
        static string sSource;
        static string sLog;
        SystemSettingsBLL objSystemSettings = null;
        Dictionary<string, RichTextBox> dicRTBPorts = null;
        private bool IsMultiplePortSelected;
        private bool AreMultipleGrids = false;

        delegate void SetTextCallback(string text);
        delegate void SetRTBTextCallback(string text, RichTextBox rtb);

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TaskManagerForm).ToString());

        public TaskManagerForm()
        {
            InitializeComponent();

            sSource = "DLMS GSM Communication";
            sLog = "Application";
            try
            {

                if (EventLog.SourceExists(sSource))
                {

                    EventLog ev = new EventLog(sLog, System.Environment.MachineName, sSource);
                    ev.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
                    ev.EnableRaisingEvents = true;
                }
            }
            catch (SecurityException ex)
            {
                MessageBox.Show("Inaccessible logs: Security");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "TaskManagerForm()", ex);
            }
            if (UtilityDetails.PrimaryUtlityName == CAB.Framework.UtilityEntity.DLMS.ToString())
            {
                btnConfigureParameters.Visible = true;
                btnShowRunning.Visible = true;
            }
            else
            {
                btnConfigureParameters.Visible = false;
                btnShowRunning.Visible = false; 
            }

            ////If GPRS is not enabled for utility then hide the GPRS report button
            //if (!UtilityDetails.ShowGPRSCommunication)
            //{
            //    btnConfigureParameters.Visible = false;
            //    btnShowRunning.Visible = false;
            //}
        }

        public void OnEntryWritten(Object source, EntryWrittenEventArgs e)
        {
            if (e.Entry.InstanceId == 1250)
            {
                string name = e.Entry.Message + "." + Environment.NewLine;
                SetText(name);
            }
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (!AreMultipleGrids &&
                this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else if (AreMultipleGrids)
            {
                int separatorIndex = text.IndexOf(':');
                if (separatorIndex >= 0 && text.StartsWith("COM"))
                {
                    string portName = text.Substring(0, separatorIndex);
                    text = text.Remove(0, separatorIndex + 1);
                    if (dicRTBPorts.ContainsKey(portName))
                    {
                        if (dicRTBPorts[portName].InvokeRequired)
                        {
                            SetRTBTextCallback d = new SetRTBTextCallback(SetRTBText);
                            this.Invoke(d, new object[] { text, dicRTBPorts[portName] });
                        }
                        else
                            SetRTBText(text, dicRTBPorts[portName]);
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, RichTextBox> kvp in dicRTBPorts)
                    {
                        if (kvp.Value.InvokeRequired)
                        {
                            SetRTBTextCallback d = new SetRTBTextCallback(SetRTBText);
                            this.Invoke(d, new object[] { text, kvp.Value });
                        }
                        else
                            SetRTBText(text, kvp.Value);
                    }
                }
            }
            else
            {
                this.txtStatus.Text = "Message : " + text + txtStatus.Text;

                string[] txtLineCount = txtStatus.Lines;
                if (txtLineCount.Length > 250)
                    SetReFillText(string.Empty);
            }
        }

        private void SetReFillText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
                this.txtStatus.Text = text;
        }

        private void SetRTBText(string pText, RichTextBox pRTB)
        {
            if (pRTB.Lines.Length > 250)
            {
                pRTB.Text = string.Empty;
            }
            pRTB.AppendText(pText);
        }

        private void TaskManagerForm_Load(object sender, EventArgs e)
        {
            dgvScheduledTasks.AllowUserToAddRows = false;
            rdbInprogress.Checked = true;

            SerialComm objSerialComm = new SerialComm();
            string[] arrCOMPorts = objSerialComm.GetAvailablePorts();
            if (arrCOMPorts == null || arrCOMPorts.Length == 0)
            {
                MessageBox.Show("No COM Port detected on this system", "No COM Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            objSystemSettings = new SystemSettingsBLL();
            string strMultiplePortsFlag = objSystemSettings.GetSettingValue(SystemSettings.USE_MULTIPLE_PORTS);
            if (!string.IsNullOrEmpty(strMultiplePortsFlag))
            {
                if (strMultiplePortsFlag.Equals("1"))
                    IsMultiplePortSelected = true;
                else
                    IsMultiplePortSelected = false;
            }
            else
                IsMultiplePortSelected = false;

            if (arrCOMPorts.Length > 1 && IsMultiplePortSelected)
            {
                txtStatus.Visible = false;
                string strCOMPorts = objSystemSettings.GetSettingValue(SystemSettings.GSM_COM_PORTS);
                if (!string.IsNullOrEmpty(strCOMPorts))
                {
                    List<string> lstCOMPorts = new List<string>(strCOMPorts.Replace(" ", "").Split(','));
                    lstCOMPorts.Sort();
                    AreMultipleGrids = true;
                    dicRTBPorts = new Dictionary<string, RichTextBox>();
                    int locLblX = 20, locLblY = 15;
                    int locRtbX = locLblX, locRtbY = locLblY + 20;
                    for (int i = 0; i < lstCOMPorts.Count; i++)
                    {
                        Label lbl = new Label();
                        Font fontUnderlineBold = new Font(lbl.Font, FontStyle.Underline | FontStyle.Bold);
                        lbl.Size = new Size(100, 20);
                        lbl.Font = fontUnderlineBold;
                        lbl.Text = lstCOMPorts[i];
                        lbl.Parent = spcTaskManager.Panel2;
                        lbl.Visible = true;
                        lbl.Location = new Point(locLblX, locLblY);
                        RichTextBox rtb = new RichTextBox();
                        rtb.ReadOnly = true;
                        rtb.Parent = spcTaskManager.Panel2;
                        rtb.Size = new System.Drawing.Size(413, 80);
                        rtb.Location = new System.Drawing.Point(locRtbX, locRtbY);
                        rtb.Visible = true;
                        rtb.BackColor = System.Drawing.Color.White;
                        dicRTBPorts.Add(lstCOMPorts[i], rtb);
                        locLblX = locRtbX = rtb.Size.Width * ((i + 1) % 2) + 20 + (20 * ((i + 1) % 2));
                        //locationY = rtb.Size.Height * Convert.ToInt32(Math.Abs(i / 2)) + 20 + ((i <= 1) ? 0 : 20);
                        locLblY += (((i + 1) % 2 == 0) ? (rtb.Size.Height + lbl.Size.Height) : 0) + (((i + 1) % 2 == 0) ? 20 : 0);
                        locRtbY = locLblY + 20;
                    }
                }
            }
            else
                txtStatus.Visible = true;
        }

        //Code to populate Grid
        private void FillTaskManagerGrid(string taskStatus)
        {
            dgvScheduledTasks.Rows.Clear();
            int rowIndex = 0;
            int counter = 0;
            List<GSMTaskEntity> listOfGSMTaskEntity;

            listOfGSMTaskEntity = gsmTaskBLL.getAllSchedulesTasks(taskStatus);

            //iterate through list n create a datagridviewrow object and add to grid..
            foreach (GSMTaskEntity taskEntity in listOfGSMTaskEntity)
            {
                rowIndex = dgvScheduledTasks.Rows.Add();
                DataGridViewRow row = dgvScheduledTasks.Rows[rowIndex];
                row.Cells["taskID"].Value = taskEntity.taskId;
                row.Cells["taskName"].Value = taskEntity.taskName;
                row.Cells["taskType"].Value = taskEntity.taskType;
                if (taskEntity.taskStatus.ToLower() == "completed")
                {
                    row.Cells["startDate"].Value = taskEntity.CreationDateTime;
                    row.Cells["startTime"].Value = "-----";
                }
                else
                {
                    row.Cells["startDate"].Value = taskEntity.startDate;
                    row.Cells["startTime"].Value = taskEntity.startTime;
                }

                if (taskEntity.taskStatus.ToLower() == "inprogress")
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.LawnGreen;
                    row.DefaultCellStyle = style;
                }
                if (taskStatus.ToLower() == "all")
                {
                    if (taskEntity.taskStatus.ToLower() == "completed")
                    {
                        DataGridViewCellStyle style = new DataGridViewCellStyle();
                        style.BackColor = Color.Wheat;
                        row.DefaultCellStyle = style;
                    }
                    if (taskEntity.taskStatus.ToLower() == "inactive")
                    {
                        DataGridViewCellStyle style = new DataGridViewCellStyle();
                        style.ForeColor = Color.PaleVioletRed;
                        row.DefaultCellStyle = style;
                    }
                }
                if (taskStatus.ToLower() == "completed")
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.Wheat;
                    row.DefaultCellStyle = style;
                }
                if (taskStatus.ToLower() == "inactive")
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.ForeColor = Color.PaleVioletRed;
                    row.DefaultCellStyle = style;
                }
                row.Cells["Status"].Value = taskEntity.taskStatus;
                row.Cells["groupName"].Value = taskEntity.groupName;

                if (taskStatus == "All" || taskStatus == "Completed")
                    row.Cells["chkTaskManager"].ReadOnly = true;

                counter++;
            }
            foreach (DataGridViewColumn dalaCol in dgvScheduledTasks.Columns)
            {
                dalaCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if (taskStatus.ToLower() == "completed")
            {
                dgvScheduledTasks.Columns[5].HeaderText = "Completion Date";
                dgvScheduledTasks.Columns[6].Visible = false;
            }
            else if (taskStatus.ToLower() == "all")
            {
                dgvScheduledTasks.Columns[5].HeaderText = "Completion/Next Run Date";
                dgvScheduledTasks.Columns[6].Visible = true;
            }
            else
            {
                dgvScheduledTasks.Columns[5].HeaderText = "Next Run Date";
                dgvScheduledTasks.Columns[6].Visible = true;
            }
        }

        //Code to create new task
        private void btnNewTask_Click(object sender, EventArgs e)
        {
            SchedularForm scheduleForm = new SchedularForm();
            scheduleForm.MdiParent = mainForm;
            scheduleForm.Show();
            this.Close();
        }

        //Code to delete the existing task(s).
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool selectedTask = false;
            List<GSMTaskEntity> colGSMTaskEntity = new List<GSMTaskEntity>();

            //This will execute if there are more than Zero records in the datagridview.
            if (dgvScheduledTasks.RowCount > 0)
            {
                for (int i = 0; i < dgvScheduledTasks.RowCount; i++)
                {
                    //Execute the below code if the value of checkbox is not null.
                    if (dgvScheduledTasks.Rows[i].Cells[1].Value != null)
                    {
                        //The following code will execute if value of the checkbox is selected.
                        if ((bool)dgvScheduledTasks.Rows[i].Cells[1].Value == true)
                        {
                            selectedTask = true;
                            GSMTaskEntity gsmTaskEntity = new GSMTaskEntity();
                            gsmTaskEntity.taskStatus = dgvScheduledTasks.Rows[i].Cells["Status"].Value.ToString();

                            if (gsmTaskEntity.taskStatus.ToLower().Trim() == "inprogress")
                            {
                                MessageBox.Show(EnumUtil.stringValueOf(TaskManagerValidationMsgs.InProgressScheduleNotDeleted), "BCS");
                                continue;
                            }

                            //Get the taskId of the task selected to be deleted and assign it to the property
                            gsmTaskEntity.taskId = Convert.ToInt32(dgvScheduledTasks.Rows[i].Cells[0].Value.ToString());
                            colGSMTaskEntity.Add(gsmTaskEntity);
                        }
                    }
                }
            }

            //If the count of the collection of GSMTaskEntity entity is equal to zero, show the message that you need to select atleast one task to delete.
            if (selectedTask == false)
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskDeletion), "BCS");
            else if (colGSMTaskEntity.Count > 0)
            {
                var result = MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskDeletionQuestion), "Delete Task(s)", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    //call to DB
                    bool resultTask = gsmTaskBLL.deleteGSMTasks(colGSMTaskEntity);

                    if (resultTask)
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskDeletionSuccessful);
                    else
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskDeletionUnsuccessful);

                    FillTaskManagerGrid(string.Empty);
                }
            }
        }

        private void dgvScheduledTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //code to show GSMTaskStatus form
                GSMTaskStatus gsmTaskStatusForm = new GSMTaskStatus();
                gsmTaskStatusForm.TaskID = Convert.ToInt32(dgvScheduledTasks.Rows[e.RowIndex].Cells[0].Value.ToString());
                gsmTaskStatusForm.functionType = dgvScheduledTasks.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                gsmTaskStatusForm.MdiParent = mainForm;
                gsmTaskStatusForm.Show();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dgvScheduledTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
        }

        private void rdbInprogress_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbInprogress.Checked)
            {
                btnDelete.Enabled = true;
                btnInactive.Enabled = true;
                btnActivate.Enabled = false;
                FillTaskManagerGrid("IP/IQ");
            }
        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAll.Checked)
            {
                btnDelete.Enabled = false;
                btnActivate.Enabled = false;
                btnInactive.Enabled = false;
                FillTaskManagerGrid("All");
            }
        }

        private void rdbInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbInactive.Checked)
            {
                btnDelete.Enabled = false;
                btnInactive.Enabled = false;
                btnActivate.Enabled = true;
                FillTaskManagerGrid("Inactive");
            }
        }

        private void rdbCompleted_CheckedChanged(object sender, EventArgs e)
        {

            if (rdbCompleted.Checked)
            {
                btnDelete.Enabled = false;
                btnActivate.Enabled = false;
                btnInactive.Enabled = false;
                FillTaskManagerGrid("Completed");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rdbInprogress.Checked)
                FillTaskManagerGrid("IP/IQ");
            else if (rdbCompleted.Checked)
                FillTaskManagerGrid("Completed");
            else if (rdbAll.Checked)
                FillTaskManagerGrid("All");
        }

        //Code to inactivate the existing task(s).
        private void btnInactive_Click(object sender, EventArgs e)
        {
            List<GSMTaskEntity> colChangedGSMTaskEntity = new List<GSMTaskEntity>();
            bool selectedTask = false;

            //adding Schedule to list which will be made Inactive
            if (dgvScheduledTasks.RowCount > 0)
            {
                for (int i = 0; i < dgvScheduledTasks.RowCount; i++)
                {
                    if (dgvScheduledTasks.Rows[i].Cells["chkTaskManager"].Value != null)
                    {
                        if ((bool)dgvScheduledTasks.Rows[i].Cells["chkTaskManager"].Value == true)
                        {
                            selectedTask = true;
                            GSMTaskEntity taskEntity = new GSMTaskEntity();
                            taskEntity.taskStatus = dgvScheduledTasks.Rows[i].Cells["Status"].Value.ToString();

                            if (taskEntity.taskStatus.ToLower().Trim() == "inprogress")
                            {
                                MessageBox.Show(EnumUtil.stringValueOf(TaskManagerValidationMsgs.InProgressScheduleNotInactive), "BCS");
                                continue;
                            }
                            else
                                taskEntity.taskStatus = "Inactive";

                            taskEntity.taskId = Convert.ToInt32(dgvScheduledTasks.Rows[i].Cells["taskID"].Value);
                            colChangedGSMTaskEntity.Add(taskEntity);
                        }
                    }
                }
            }

            //If the count of the collection of GSMTaskEntity entity is equal to zero, show the message that you need to select atleast one task to inactive.
            if (selectedTask == false)
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskStatusInactive), "BCS");
            else if (colChangedGSMTaskEntity.Count > 0)
            {
                var result = MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskInactiveQuestion), "Inactive Task(s)", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    //call to DB
                    bool resultTask = gsmTaskBLL.updateGSMTasksStatus(colChangedGSMTaskEntity);

                    if (resultTask)
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskInacticeSuccessful);
                    else
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskInactiveUnsuccessful);

                    FillTaskManagerGrid(string.Empty);
                }
            }
        }

        //Code to activate the existing task(s).
        private void btnActivate_Click(object sender, EventArgs e)
        {
            bool selectedTask = false;
            List<GSMTaskEntity> colChangedGSMTaskEntity = new List<GSMTaskEntity>();

            //adding Schedule to list which will be made Inactive
            if (dgvScheduledTasks.RowCount > 0)
            {
                for (int i = 0; i < dgvScheduledTasks.RowCount; i++)
                {
                    if (dgvScheduledTasks.Rows[i].Cells["chkTaskManager"].Value != null)
                    {
                        if ((bool)dgvScheduledTasks.Rows[i].Cells["chkTaskManager"].Value == true)
                        {
                            selectedTask = true;
                            GSMTaskEntity taskEntity = new GSMTaskEntity();
                            if (dgvScheduledTasks.Rows[i].Cells["taskType"].Value.ToString() == "One Time Only")
                            {
                                MessageBox.Show("One Time only Schedule cannot be made Active again.", "BCS");
                                continue;
                            }
                            taskEntity.taskId = Convert.ToInt32(dgvScheduledTasks.Rows[i].Cells["taskID"].Value);
                            taskEntity.taskStatus = "Inqueue";
                            colChangedGSMTaskEntity.Add(taskEntity);
                        }
                    }
                }
            }

            //If the count of the collection of GSMTaskEntity entity is equal to zero, show the message that you need to select atleast one task to active.
            if (selectedTask == false)
                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskStatusActive), "BCS");
            else if (colChangedGSMTaskEntity.Count > 0)
            {
                var result = MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskActiveQuestion), "Active Task(s)", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    //call to DB
                    bool resultTask = gsmTaskBLL.updateGSMTasksStatus(colChangedGSMTaskEntity);

                    if (resultTask)
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskActiveSuccessful);
                    else
                        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskActiveUnsuccessful);

                    FillTaskManagerGrid("Inactive");
                }
            }
        }

        private void TaskManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //On Form closed event clear the Status message
            if (mainForm != null)
            {
                mainForm.toolStripStatusLabel.Text = "";
            }
            this.Hide();
            e.Cancel = true;
        }

        private void TaskManagerForm_Activated(object sender, EventArgs e)
        {
            if (rdbInprogress.Checked)
                FillTaskManagerGrid("IP/IQ");
            else if (rdbCompleted.Checked)
                FillTaskManagerGrid("Completed");
            else if (rdbAll.Checked)
                FillTaskManagerGrid("All");
            else if (rdbInactive.Checked)
                FillTaskManagerGrid("Inactive");
            //if (rdbInprogress.Checked)
            //{
            //    rdbInprogress_CheckedChanged(null, null);
            //}
            //else
            //{
            //    rdbInprogress.Checked = true;
            //}
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            if (AreMultipleGrids)
            {
                if (dicRTBPorts != null)
                {
                    if (dicRTBPorts.Count > 0)
                    {
                        foreach (RichTextBox textBox in dicRTBPorts.Values)
                        {
                            textBox.Clear();
                        }
                    }
                }
            }
            else
            {
                txtStatus.Clear();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbort_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController("Generic3PhaseCommunication");
            GSMLoggingBLL gsmLogging = new GSMLoggingBLL();
            List<GSMTaskEntity> inprogressGSMTasks = null;
            //TimeSpan timeout;
            try
            {
                inprogressGSMTasks = gsmTaskBLL.getAllSchedulesTasks("Inprogress");
                if (inprogressGSMTasks.Count > 0)
                {
                    //DialogResult dialogResult = MessageBox.Show("Do you want to resume the inprogress task ?", "BCS", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    //inprogressGSMTasks = gsmTaskBLL.getAllSchedulesTasks("Inprogress");
                    //if the task is said to not to be resumed
                    //if (dialogResult == DialogResult.No)
                    //{
                    //    service.Stop();
                    //    //delete from active tasks table
                    //    gsmTaskBLL.deleteGSMTasks(inprogressGSMTasks);
                    //    foreach (GSMTaskEntity gsmTaskEntity in inprogressGSMTasks)
                    //    {
                    //        //mark the inprogress meters/logs as aborted.
                    //        List<GSMLoggingEntity> logs = gsmLogging.GetLogsByTaskID(gsmTaskEntity.taskId);
                    //        foreach (GSMLoggingEntity entity in logs)
                    //        {
                    //            if (entity.Retries < gsmTaskEntity.taskRetries || entity.Status == "IP")
                    //            {
                    //                entity.Status = "NC";
                    //                entity.ErrorMessage = "Aborted";
                    //                gsmLogging.InsertorUpdateData(entity, true);
                    //            }
                    //        }
                    //        //move the inprogress task to completed
                    //        gsmTaskEntity.taskStatus = "NC";
                    //        gsmTaskBLL.InsertCompleteTask(gsmTaskEntity);
                    //    }
                    //}
                    //if the task is said to be resumed
                    //else if (dialogResult == DialogResult.Yes)
                    DialogResult dialogResult = MessageBox.Show("Do you want to abort the inprogress task ?", "BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        service.Stop();
                        //gsmTaskBLL.deleteGSMTasks(inprogressGSMTasks);
                        foreach (GSMTaskEntity gsmTaskEntity in inprogressGSMTasks)
                        {
                            //mark the logs as aborted.
                            List<GSMLoggingEntity> logs = gsmLogging.GetLogsByTaskID(gsmTaskEntity.taskId);
                            foreach (GSMLoggingEntity entity in logs)
                            {
                                if (entity.Retries < gsmTaskEntity.taskRetries || entity.Status == "IP")
                                {
                                    entity.Status = "NC";
                                    entity.ErrorMessage = "Aborted";
                                    entity.Retries = gsmTaskEntity.taskRetries;
                                    gsmLogging.InsertorUpdateData(entity, true);
                                }

                            }
                            //move the inprogress task to completed
                            gsmTaskEntity.taskStatus = "Aborted";
                            gsmTaskBLL.UpdateGSMTask(gsmTaskEntity);
                            this.txtStatus.Text = "Aborting..";
                        }
                        SetText("Starting service,please wait..");
                        //Wait for 10 seconds to stop the service gracefully.
                        Thread.Sleep(10000);
                        TimeSpan span = new TimeSpan(0, 0, 20);
                        service.WaitForStatus(ServiceControllerStatus.Stopped, span);
                        service.Start();
                        SetText("Service started.");
                    }
                    else
                    {
                        //do nothing
                    }
                   
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Only inprogress task(s) can be aborted!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnAbort_Click(object sender, EventArgs e)", ex);
                MessageBox.Show(ex.StackTrace);
            }

        }

        private void btnConfigureParameters_Click(object sender, EventArgs e)
        {
            SchedulingReportConfiguration config = new SchedulingReportConfiguration();
            config.MdiParent = this.Parent.Parent as Form;
            config.WindowState = FormWindowState.Maximized;
            config.Show();
        }

        private void btnShowRunning_Click(object sender, EventArgs e)
        {
            // Invoking the GPRS scheduling report form
            ReportConfigurationParameters parameters = new ReportConfigurationParameters();
            parameters.Type = ReportType.RUNNING;
            GPRSSchedulingReport report = new GPRSSchedulingReport(parameters);
            report.Show();
        }

        //private void btnAbort_Click(object sender, EventArgs e)
        //{
        //List<GSMTaskEntity> colGSMTaskEntity = new List<GSMTaskEntity>();
        //var result = DialogResult.Cancel;

        ////This will execute if there are more than Zero records in the datagridview.
        //if (dgvScheduledTasks.RowCount > 0)
        //{
        //    for (int i = 0; i < dgvScheduledTasks.RowCount; i++)
        //    {
        //        //Execute the below code if the value of checkbox is not null and if value of the checkbox is selected
        //        if (dgvScheduledTasks.Rows[i].Cells[1].Value != null && (bool)dgvScheduledTasks.Rows[i].Cells[1].Value == true)
        //        {
        //            if (dgvScheduledTasks.Rows[i].Cells[7].Value.ToString() == "Inprogress")
        //            {
        //                //send disconnect command to modem
        //                //com.DLMSDisconnect();
        //                com.Disconnect();

        //                GSMTaskEntity gsmTaskEntity = new GSMTaskEntity();
        //                gsmTaskEntity = gsmTaskBLL.GetTaskByTaskID(Convert.ToInt32(dgvScheduledTasks.Rows[i].Cells[0].Value.ToString()));
        //                gsmTaskEntity.taskStatus = "Inqueue";
        //                colGSMTaskEntity.Add(gsmTaskEntity);

        //                result = MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskAbortionQuestion), "Abort Schedule", MessageBoxButtons.OKCancel);
        //            }
        //            else
        //            {
        //                //If the status of Schedule is not IP
        //                MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.TaskInprogressAbort));
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            //If no Schedule is selected
        //            MessageBox.Show(EnumUtil.stringValueOf(ValidationMsgs.ValidateTaskAbortion));
        //            break;
        //        }
        //    }
        //}

        //if (result == DialogResult.OK)
        //{
        //    //call to DB to update Next Run Time for the Task
        //    bool resultTask = gsmTaskBLL.updateGSMTasks(colGSMTaskEntity);

        //    if (resultTask)
        //    {
        //        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskAbortionSuccessful);
        //    }
        //    else
        //    {
        //        mainForm.toolStripStatusLabel.Text = EnumUtil.stringValueOf(ValidationMsgs.TaskAbortionUnsuccessful);
        //    }

        //    FillTaskManagerGrid(string.Empty);
        //}
        //}
    }
}
