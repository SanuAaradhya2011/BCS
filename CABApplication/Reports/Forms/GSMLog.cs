using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.UI;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Hunt.EPIC.Logging;





namespace CABApplication.Reports.Forms
{
    public partial class GSMLog : CABForm
    {
        private FileReportDataSet reportXSD = null;
        GPRSReportBLL gPRSReportBLL = new GPRSReportBLL();
        DataSet ds = null;
        List<string> lstClm = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMLog).ToString());
        public GSMLog()
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
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                this.StatusMessage = "To Date should be greater than From Date";
                return result;
            }
            return true;
        }


        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {                   
                if (validate() == true)
                {
                    string FromDate = dtpFrom.Value.ToString();
                    string ToDate = dtpTo.Value.ToString();
                    reportXSD = new FileReportDataSet();
                    ds = gPRSReportBLL.GetGRPSTaskLog(DateTime.Parse(dtpFrom.Value.ToShortDateString() + " 00:00:00"), DateTime.Parse(dtpTo.Value.ToShortDateString() + " 23:59:59"));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //call FillTariffEnergyXSD_NET to fill schema
                        FillTariffEnergyXSD_NET(ds, "GPRSTCPTaskLog");
                        //call fn. to create report
                        ShowReport();
                    }
                    else
                    {
                        MessageBox.Show("There is no Log present between selected dates.");
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                throw;
            }
        }



        private void ShowReport()
        {
            try
            {
                if (reportXSD.Tables["GPRSTCPTaskLog"].Rows.Count == 0)
                {
                    // No need to show report
                }
                else
                {
                    //creating object of ReportForm and GSMSuccessReport
                    ReportForm objReportForm = new ReportForm();
                    GPRSTCPTaskLogHistory gsmSuccessReport = new GPRSTCPTaskLogHistory();

                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = gsmSuccessReport.secHeader.ReportObjects;                    
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        int iter = 1;
                        foreach (string item in lstClm)
                        {
                            TextObject objText = (TextObject)rebObjCol["Text" + iter];
                            objText.Text = item;
                            iter++;
                        }
                    }

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
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "ShowReport()", ex);
            }
        }



    





        private void FillTariffEnergyXSD_NET(DataSet ds, string TableName)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstClm = new List<string>();
                    foreach (DataColumn item in ds.Tables[0].Columns)
                    {
                        lstClm.Add(item.ColumnName);
                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataRow repRow = reportXSD.Tables[TableName].NewRow();
                        for (int colCount = 0; colCount < ds.Tables[0].Columns.Count; colCount++)
                        {
                            repRow[colCount] = row[colCount].ToString();
                        }
                        reportXSD.Tables[TableName].Rows.Add(repRow);
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FillTariffEnergyXSD_NET(DataSet ds, string TableName)", ex);
            }

        }


       

        private void FillTariffEnergyXSD_NET(DataTable dtResult, string TableName)
        {
            try
            {
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResult.Rows)
                    {
                        DataRow repRow = reportXSD.Tables[TableName].NewRow();
                        for (int colCount = 0; colCount < dtResult.Columns.Count; colCount++)
                        {
                            repRow[colCount] = row[colCount].ToString();
                        }
                        reportXSD.Tables[TableName].Rows.Add(repRow);
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillTariffEnergyXSD_NET(DataTable dtResult, string TableName)", ex);

            }

        }



        private void btnCancel_Click(object sender, EventArgs e)
        {            
            this.Close();            
        }
    }
}
