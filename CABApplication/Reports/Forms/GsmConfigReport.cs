using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;

namespace CAB.UI
{
    public partial class GsmConfigReport : CABForm
    {
        GSMGroupBLL gsmGroupBLL = new GSMGroupBLL();
        GSMConfigBLL gsmConfigBLL = new GSMConfigBLL();
        private FileReportDataSet reportXSD = null;

        public GsmConfigReport()
        {
            InitializeComponent();
            dtpFrom.CustomFormat = "dd/MM/yyyy";
            dtpTo.CustomFormat = "dd/MM/yyyy";
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
        }

        private bool validate()
        {
            bool result = false;

            //check for correct date
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                this.StatusMessage = "To Date should be greater than From Date";
                return result;
            }
            return true;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            List<GSMConfigReportEntity> lstReportEntity = new List<GSMConfigReportEntity>();
            reportXSD = new FileReportDataSet();

            Cursor.Current = Cursors.WaitCursor;
           
            //calling validate fn. to validate business rule
            if (validate() == true)
            {
                string FromDate = dtpFrom.Value.ToString();
                string ToDate = dtpTo.Value.ToString();
                decimal saTotal = 0;
                decimal faTotal = 0;
                string totalSuccess = string.Empty;

                //call to DB
                lstReportEntity = gsmConfigBLL.GetReportData(DateTime.Parse(dtpFrom.Value.ToShortDateString() + " 00:00:00"), DateTime.Parse(dtpTo.Value.ToShortDateString() + " 23:59:59"), lstTaskList.Text);
                if (lstReportEntity.Count != 0)
                {
                    FillGsmXSD(lstReportEntity);
                    //call fn. to create report
                    ShowReport();
                }
                else
                    MessageBox.Show("There is no Schedule between selected dates.");
            }
        }

        private void FillGsmXSD(List<GSMConfigReportEntity> lstReportEntity)
        {
            //filling the FileReportDataSet
            DataRow reportRow;
            for (int i = 0; i < lstReportEntity.Count; i++)
            {
                reportRow = reportXSD.Tables["GSMConfigDetails"].NewRow();
                reportRow["MeterID"] = lstReportEntity[i].Meter_ID.ToString();
                reportRow["Sim No."] = lstReportEntity[i].Sim_No.ToString();
                reportRow["Status"] = lstReportEntity[i].Status.ToString();
                reportRow["Reason"] = lstReportEntity[i].Reason.ToString();
                reportRow["TaskName"] = lstReportEntity[i].TaskName.ToString();
                reportRow["CreationDateTime"] = lstReportEntity[i].Reading_DateTime.ToString();
                reportXSD.Tables["GSMConfigDetails"].Rows.Add(reportRow);
            }
        }

        private void ShowReport()
        {
            //creating object of ReportForm and GSMSuccessReport
            ReportForm objReportForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.GSMConfigurationReport gsmConfigReport = new GSMConfigurationReport();

            //assinging data source to GSMSuccessReport
            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(gsmConfigReport);
            gsmConfigReport.SetDataSource(reportXSD);

            //assinging data source to ReportForm
            objReportForm.rptViewer.ReportSource = gsmConfigReport;
            
            Cursor.Current = Cursors.Default;
            objReportForm.rptViewer.Zoom(1);
            this.Hide();
            objReportForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //form will get closed
            this.Close();
            this.StatusMessage = string.Empty;
        }

        private void btnTaskID_Click(object sender, EventArgs e)
        {
           DataSet Tds = new DataSet();
          // Tds = gsmConfigBLL.GetTaskIDName();
           Tds = gsmConfigBLL.GetTaskIDName(dtpFrom.Value, dtpTo.Value);
            lstTaskList.DataSource =Tds.Tables[0];
            lstTaskList.DisplayMember = "TaskID"; 
          
        }
    }
}