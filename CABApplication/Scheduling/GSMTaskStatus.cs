using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;

namespace CABApplication
{
    public partial class GSMTaskStatus : MdiChildForm
    {
        GSMLoggingBLL gsmLoggingBLL = new GSMLoggingBLL();
        GSMTaskBLL gsmTaskBLL = new GSMTaskBLL();

        private int taskID = 0;
        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        public string functionType { get; set; }

        public GSMTaskStatus()
        {
            InitializeComponent();
        }

        private void GSMTaskStatus_Load(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            if (taskID > 0)
            {
                FillDataGrid(taskID);
                FillTaskDetails(taskID);
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillDataGrid(taskID);
            FillTaskDetails(taskID);
        }

        //fill Labels
        public void FillTaskDetails(int taskID)
        {
            if (taskID > 0)
            {
                GSMTaskEntity gsmTaskEntity = new GSMTaskEntity();
                if (functionType == "Completed")
                    gsmTaskEntity = gsmTaskBLL.GetCompletedTaskByID(taskID);
                else
                    gsmTaskEntity = gsmTaskBLL.GetTaskByTaskID(taskID);

                if (gsmTaskEntity != null)
                {
                    lblTaskName.Text = "Schedule Name : " + gsmTaskEntity.taskName;
                    lblGroupName.Text = "Group Name      : " + gsmTaskEntity.groupName;
                    lblTaskType.Text = "Schedule Type  : " + gsmTaskEntity.taskType;
                    lblNextRunDate.Text = "Next Run Date : " + gsmTaskEntity.startDate;
                    lblNextRunTime.Text = "Next Run Time : " + gsmTaskEntity.startTime;
                    //lblStatus.Text = "Status : " + gsmTaskEntity.taskStatus;
                }
            }
        }

        //fill Grid
        public void FillDataGrid(int taskID)
        {
            //call to DB
            List<GSMLoggingEntity> lstGSMLogging = gsmLoggingBLL.GetLogsByTaskID(taskID);

            dataGridView1.Rows.Clear();
            int rowIndex = 0;
            int counter = 0;

            if (lstGSMLogging != null && lstGSMLogging.Count > 0)
                foreach (GSMLoggingEntity gsmLoggingEntity in lstGSMLogging)
                {
                    rowIndex = dataGridView1.Rows.Add();
                    DataGridViewRow row = dataGridView1.Rows[rowIndex];

                    row.Cells["MeterID"].Value = gsmLoggingEntity.Meter_ID;
                    DataGridViewCheckBoxCell dgvchkGeneral = (DataGridViewCheckBoxCell)row.Cells["MeterGeneralData"];
                    dgvchkGeneral.Value = gsmLoggingEntity.IsGeneralCompleted;
                    DataGridViewCheckBoxCell dgvchkInst = (DataGridViewCheckBoxCell)row.Cells["MeterInstData"];
                    dgvchkInst.Value = gsmLoggingEntity.IsInstantCompleted;
                    DataGridViewCheckBoxCell dgvchkBilling = (DataGridViewCheckBoxCell)row.Cells["MeterBillingData"];
                    dgvchkBilling.Value = gsmLoggingEntity.IsBillingCompleted;
                    //fill the values in load survey and tamper columns
                    DataGridViewCheckBoxCell dgvchkLoadSurvey = (DataGridViewCheckBoxCell)row.Cells["LoadSurveyCompleted"];
                    dgvchkLoadSurvey.Value = gsmLoggingEntity.IsLoadSurveyCompleted;
                    DataGridViewCheckBoxCell dgvchkTamper = (DataGridViewCheckBoxCell)row.Cells["TamperCompleted"];
                    dgvchkTamper.Value = gsmLoggingEntity.IsTamperCompleted;


                    DataGridViewCheckBoxCell dgvchkMidNight = (DataGridViewCheckBoxCell)row.Cells["MidnightCompleted"];
                    dgvchkMidNight.Value = gsmLoggingEntity.IsMidNightCompleted;

                    row.Cells["MeterDate"].Value = gsmLoggingEntity.CreationDateTime.ToString();
                    row.Cells["MeterStatus"].Value = gsmLoggingEntity.Status;
                    row.Cells["MeterRetries"].Value = gsmLoggingEntity.Retries;
                    row.Cells["errorMessage"].Value = gsmLoggingEntity.ErrorMessage;

                    counter++;
                }
        }
    }
}