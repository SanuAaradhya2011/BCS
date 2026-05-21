using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CAB.UI;
using CABApplication;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using LTCTBLL;
using System.ComponentModel;
using CABEntity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Channel.Formatter;
using CAB.DALC.Data;
using Hunt.EPIC.Logging;

namespace CABApplication.Reports.Forms
{
    public partial class AdhocReport : Form
    {
        private FileReportDataSet reportXSD = null;
        DataSet AdhocDS = null;
        bool isPUMA = false;
        ApplicationType types;
        private const string AdhocColumnName = "AdhocColumnName";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingReportnew_TNEB).ToString());
        int meterModelNumber=0; 
        
        public AdhocReport() 
        {
            InitializeComponent();
        }

        private void AdhocReport_Load(object sender, EventArgs e)
        {
            string signaturedata = ConfigInfo.SignatureInfo;

            if (signaturedata =="")
            {
                MessageBox.Show("This feature is only for dynamic readout.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnShow.Enabled = false;
                return;
            }
            else if (signaturedata.Contains("SPS201"))
            {
                meterModelNumber = NamePlateConstants.SapphireS2;
            }
            //******* Sapphire S2 Three phase Low cost meter ***********//
            else if (signaturedata.Contains("SPS202"))
            {
                meterModelNumber = NamePlateConstants.SapphireS2;
            }

            if (meterModelNumber == NamePlateConstants.SapphireS2)
            {
                FillAdhocParameters();               
            }
            else
            {
                MessageBox.Show("Ad hoc report is only for specific utlity", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShow.Enabled = false;
                return;

            }
            
           
        }
        private void FillAdhocParameters()
        {
            try
            {
                types = ConfigInfo.GetApplicationType();
                AdhocDS = new AdhocReadDAL().GetMeterData();                
                if (AdhocDS != null)
                {                    
                        if (AdhocDS.Tables[0].Rows.Count > 0)
                        {
                            lblNoDataFound.Visible = false;
                            chkListadhocParameters.Items.Clear();
                            foreach (DataRow row in AdhocDS.Tables[0].Rows)
                            {
                                chkListadhocParameters.Items.Add(row["AdhocColumnName"].ToString());                                
                            }
                        }
                        else
                        {
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "No Data Found";
                            btnShow.Enabled = false;
                        }
                   
                    
                }
                else
                {
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = "No Data Found";
                    btnShow.Enabled = false;
                }
                for (int itemCounter = 0; itemCounter < chkListadhocParameters.Items.Count; itemCounter++)
                {
                    chkListadhocParameters.SetItemCheckState(itemCounter, CheckState.Indeterminate);
                    chkListadhocParameters.ItemCheck += (s, e) =>
                    {
                        if (e.CurrentValue == CheckState.Indeterminate) e.NewValue = CheckState.Indeterminate;
                    };
                }
            }
               
            catch (Exception)
            {
            }
           
        }

        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                
                List<AdhocMasterEntity> AdhocReportEntity = new List<AdhocMasterEntity>();
                AdhocDS = new DataSet();
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    AdhocDS = new AdhocReadDAL().GetMeterData();
                    FillAdhocXSD(AdhocDS);
                    ShowReport();
                    this.Cursor = Cursors.Default;
                }   
                else
                {
                    MessageBox.Show("No data available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void FillAdhocXSD(DataSet AdhocData)
        {
            DataRow reportRow;
            reportXSD = new FileReportDataSet();
            if (AdhocData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in AdhocData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["DLMS650AdhocTable"].NewRow();
                        reportRow["AdhocColumnName"] = row["AdhocColumnName"].ToString();
                        reportRow["AdhocColumnValue"] = row["AdhocColumnValue"].ToString();
                        reportRow["AdhocObisCode"] = row["AdhocObisCode"].ToString();
                        reportRow["AdhocClassID"] = row["AdhocClassID"].ToString();
                        reportRow["AdhocAttribute"] = row["AdhocAttribute"].ToString();
                        reportXSD.Tables["DLMS650AdhocTable"].Rows.Add(reportRow);
                    }
                }

            }
        }
        private void ShowReport()
        {
            //creating object adhocreport
            ReportForm objReportForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.AdhocDetails AdhocReport = new AdhocDetails();
            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(AdhocReport);
            AdhocReport.SetDataSource(reportXSD);
            objReportForm.rptViewer.ReportSource = AdhocReport;
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
            this.Close();
        }
        
    }
}
