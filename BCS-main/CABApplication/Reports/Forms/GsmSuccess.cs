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
    public partial class GsmSuccess : CABForm
    {
        GSMGroupBLL gsmGroupBLL = new GSMGroupBLL();
        GSMReportingBLL gsmReportBLL = new GSMReportingBLL();
        private FileReportDataSet reportXSD = null;

        public GsmSuccess()
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
            List<GSMReportEntity> lstReportEntity = new List<GSMReportEntity>();
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
                lstReportEntity = gsmReportBLL.GetReportData(DateTime.Parse(dtpFrom.Value.ToShortDateString()+ " 00:00:00"), DateTime.Parse(dtpTo.Value.ToShortDateString()+ " 23:59:59"), out saTotal, out faTotal, out totalSuccess);
                if (lstReportEntity.Count != 0)
                {
                    //call FillGsmXSD to fill schema
                    FillGsmXSD(lstReportEntity, FromDate, ToDate, saTotal, faTotal, totalSuccess);
                    //call fn. to create report
                    ShowReport();
                }
                else
                    MessageBox.Show("There is no Schedule between selected dates.");
            }
        }

        private void FillGsmXSD(List<GSMReportEntity> lstReportEntity, string fromDate, string toDate, decimal saTotal, decimal faTotal, string totalSuccess)
        {
            //filling the FileReportDataSet
            DataRow reportRow;
            for (int i = 0; i < lstReportEntity.Count; i++)
            {
                reportRow = reportXSD.Tables["GSMMeterDetails"].NewRow();
                reportRow["Consumer ID"] = lstReportEntity[i].Consumer_ID.ToString();
                reportRow["Consumer Name"] = lstReportEntity[i].Consumer_Name.ToString();
                reportRow["Meter No."] = lstReportEntity[i].Meter_ID.ToString();
                reportRow["Sim No."] = "+91" + lstReportEntity[i].Sim_ID.ToString().Substring(1, 10);
                reportRow["Success Attempt"] = lstReportEntity[i].Success_Attempt.ToString();
                reportRow["Failure Attempt"] = lstReportEntity[i].Failure_Attempt.ToString();
                reportRow["Success %"] = lstReportEntity[i].Success_Rate.ToString() + " %";
                reportRow["From Date"] = fromDate.Substring(0,10);
                reportRow["To Date"] = toDate.Substring(0, 10);
                reportRow["Last reading date"] = lstReportEntity[i].Reading_DateTime.ToString();
                reportRow["saTotal"] = saTotal;
                reportRow["faTotal"] = faTotal;
                reportRow["totalSuccess%"] = totalSuccess + " %";

                reportXSD.Tables["GSMMeterDetails"].Rows.Add(reportRow);
            }
        }

        private void ShowReport()
        {
            //creating object of ReportForm and GSMSuccessReport
            ReportForm objReportForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.GSMSuccessReport gsmSuccessReport = new GSMSuccessReport();

            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(gsmSuccessReport);

            //assinging data source to GSMSuccessReport
            gsmSuccessReport.SetDataSource(reportXSD);

            //assinging data source to ReportForm
            objReportForm.rptViewer.ReportSource = gsmSuccessReport;
            
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
    }
}