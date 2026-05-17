using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;

namespace CAB.UI
{
    public partial class ScheduleReportForm : CABForm
    {
        private FileReportDataSet reportXSD = null;
        GSMTaskBLL gsmTaskBLL = new GSMTaskBLL();
        GSMGroupBLL gsmGroupBLL = new GSMGroupBLL();

        List<string> lstSchedule = new List<string>();
        List<string> lstStatus = new List<string>();
        List<string> lstGroupId = new List<string>();
        List<string> lstGroupName = new List<string>();

        public ScheduleReportForm()
        {
            InitializeComponent();
            fillSchedule();
            fillStatus();
            fillGroup();
            dtpFrom.CustomFormat = "dd/MM/yyyy";
            dtpTo.CustomFormat = "dd/MM/yyyy";
        }

        private void fillSchedule()
        {
            lstSchedule.Add("All");
            lstSchedule.Add("Daily");
            lstSchedule.Add("Weekly");
            lstSchedule.Add("Monthly");
            lstSchedule.Add("One Time Only");

            cmbSchedule.DataSource = lstSchedule;
        }

        private void fillStatus()
        {
            lstStatus.Add("All");
            lstStatus.Add("Inqueue");
            lstStatus.Add("Inprogress");
            lstStatus.Add("Inactive");
            lstStatus.Add("Completed");

            cmbStatus.DataSource = lstStatus;
        }

        private void fillGroup()
        {
            DataSet dataSet = gsmGroupBLL.ListGroupData();
            string groupId = string.Empty;
            string groupName = string.Empty;

            if (dataSet != null)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count + 1; i++)
                    {
                        if (i == 0)
                        {
                            groupId = "All";
                            groupName = "All";
                        }
                        else
                        {
                            groupId = dataSet.Tables[0].Rows[i - 1]["Group ID"].ToString();
                            groupName = dataSet.Tables[0].Rows[i - 1]["Group Name"].ToString();
                        }

                        lstGroupId.Add(groupId);
                        lstGroupName.Add(groupName);
                    }

                    cmbGroup.DataSource = lstGroupName;
                }
            }
        }

        private bool validate()
        {
            bool result = false;

            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("To Date should be greater than From Date" ,"BCS",MessageBoxButtons.OK , MessageBoxIcon.Information);
                return result;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            List<GSMTaskEntity> listOfGSMTaskEntity;
            reportXSD = new FileReportDataSet();
            string groupName = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            if (validate() == true)
            {
                DateTime fromDate = DateTime.Parse(dtpFrom.Value.ToShortDateString()+ " 00:00:00");
                DateTime toDate = DateTime.Parse(dtpTo.Value.ToShortDateString() + " 23:59:59");
                string scheduleType = cmbSchedule.SelectedItem.ToString();
                string statusType = cmbStatus.SelectedItem.ToString();
                if (cmbGroup.Items.Count > 0)
                {
                    groupName = lstGroupId[cmbGroup.SelectedIndex].ToString();
                }

                //call DB
                listOfGSMTaskEntity = gsmTaskBLL.getReportSchedules(fromDate, toDate, scheduleType, groupName, statusType);
                if (listOfGSMTaskEntity.Count != 0)
                {
                    //fill schema
                    FillGsmXSD(listOfGSMTaskEntity);
                    //create report
                    ShowReport();
                }
                else
                    MessageBox.Show("There is no Schedule for selected criteria.","BCS",MessageBoxButtons.OK , MessageBoxIcon.Information);
            }
        }

        private void FillGsmXSD(List<GSMTaskEntity> listOfGSMTaskEntity)
        {
            DataRow reportRow;
            for (int i = 0; i < listOfGSMTaskEntity.Count; i++)
            {
                reportRow = reportXSD.Tables["ScheduleDetails"].NewRow();
                reportRow["ScheduleName"] = listOfGSMTaskEntity[i].taskName.ToString();
                reportRow["ScheduleType"] = listOfGSMTaskEntity[i].taskType.ToString();
                reportRow["ScheduleDate"] = listOfGSMTaskEntity[i].startDate.ToString();
                reportRow["ScheduleTime"] = listOfGSMTaskEntity[i].startTime.ToString();

                if (listOfGSMTaskEntity[i].jobNames != null)
                {
                    reportRow["DataReq"] = listOfGSMTaskEntity[i].jobNames.ToString();
                }

                reportRow["Group"] = listOfGSMTaskEntity[i].groupName.ToString();
                reportRow["ScheduleStatus"] = listOfGSMTaskEntity[i].taskStatus.ToString();
                                
                reportXSD.Tables["ScheduleDetails"].Rows.Add(reportRow);
            }
        }

        private void ShowReport()
        {
            ReportForm objReportForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.SchedulesReport scheduleReport = new CABApplication.Reports.DLMS_Detailed_Reports.SchedulesReport();

            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(scheduleReport);
            scheduleReport.SetDataSource(reportXSD);
            objReportForm.rptViewer.ReportSource = scheduleReport;

            Cursor.Current = Cursors.Default;
            objReportForm.rptViewer.Zoom(1);
            this.Hide();
            objReportForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }
    }
}