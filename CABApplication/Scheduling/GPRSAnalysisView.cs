using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;

namespace CABApplication.Scheduling
{
    public partial class GPRSAnalysisView : Form
    {
        public GPRSAnalysisView()
        {
            InitializeComponent();
            this.Text = "GPRS Analysis Reports";
        }

        #region "Column Constants"
        private const string colID = "Id";
        private const string colScheduleName = "Schedule Name";
        private const string colRunDate = "Completion Date";
        private const string colScheduleDate = "Schedule Date";
        private const string colType = "Type";
        private const string colGroupName = "Group Name";
        private const string colCommType = "Communication Type";
        private const string colMeterId = "Meter Id";
        private const string colFileName = "File Name";
        private const string colFileUploadId = "File Upload Id";
        private const string colProfile = "Profiles";
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ValidateSearchControls())
            {
                GPRSReportBLL objReport = new GPRSReportBLL();
                DataSet ds = objReport.GetGRPSSearchResult(cmbSearchType.Text, dtFrom.Value, dtTo.Value);
                if (ds != null && ds.Tables != null && ds.Tables.Count >0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (string.Equals(cmbSearchType.Text, "Schedule", StringComparison.OrdinalIgnoreCase))
                    {
                        PopulateScheduleWiseData(ds.Tables[0]);
                    }
                    else
                    {
                        PopulateMeterWiseData(ds.Tables[0]);
                    }
                    btnShow.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No record found for passed search criteria.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        /// <summary>
        /// Populates Meter wise data on gridview 
        /// </summary>
        /// <param name="dtTable"></param>
        private void PopulateMeterWiseData(DataTable dtTable)
        {
            gvSearchResult.Columns.Clear();
            gvSearchResult.DataSource = dtTable;
            gvSearchResult.Columns[colMeterId].Width = 100;
            gvSearchResult.Columns[colScheduleName].Width = 200;
            gvSearchResult.Columns[colRunDate].Width = 120;
            gvSearchResult.Columns[colScheduleDate].Width = 120;
            gvSearchResult.Columns[colType].Width = 120;
            gvSearchResult.Columns[colGroupName].Width = 120;
            gvSearchResult.Columns[colCommType].Width = 150;
            gvSearchResult.Columns[colFileName].Width = 200;
            gvSearchResult.Columns[colProfile].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            gvSearchResult.Columns[colFileUploadId].Visible = false;
            gvSearchResult.Columns[colID].Visible = false;

            if (gvSearchResult.Columns["Select"] == null)
            {
                gvSearchResult.Columns.Insert(0, new DataGridViewCheckBoxColumn(false));
                gvSearchResult.Columns[0].HeaderText = "Select";
                gvSearchResult.Columns[0].Width = 50;
                gvSearchResult.Columns[0].Name = "Select";
            }
            gvSearchResult.AllowUserToAddRows = false;
            //gvSearchResult.AutoSize = true;
            // gvSearchResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Populate Schedule wise data in the gridview 
        /// </summary>
        /// <param name="dtTable"></param>
        private void PopulateScheduleWiseData(DataTable dtTable)
        {
            // gvSearchResult.Rows.Clear();
            gvSearchResult.DataSource = dtTable;
            gvSearchResult.Columns[colID].Width = 75;
            gvSearchResult.Columns[colScheduleName].Width = 200;
            gvSearchResult.Columns[colRunDate].Width = 120;
            gvSearchResult.Columns[colScheduleDate].Width = 120;
            gvSearchResult.Columns[colType].Width = 120;
            gvSearchResult.Columns[colGroupName].Width = 120;
            gvSearchResult.Columns[colProfile].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            gvSearchResult.Columns[colCommType].Width = 150;
            if (gvSearchResult.Columns["Select"] == null)
            {
                gvSearchResult.Columns.Insert(0, new DataGridViewCheckBoxColumn(false));
                gvSearchResult.Columns[0].HeaderText = "Select";
                gvSearchResult.Columns[0].Name = "Select";
                gvSearchResult.Columns[0].Width = 50;
            }
            gvSearchResult.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Performs screen validation. If all validation are passed returns true
        /// </summary>
        /// <returns></returns>
        private bool ValidateSearchControls()
        {
            bool retValue = true;
            DateTime dtFromDate = dtFrom.Value;
            DateTime dtToDate = dtTo.Value;
            TimeSpan timespan = dtToDate - dtFromDate;

            if (cmbSearchType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select search by option.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSearchType.Focus();
                retValue = false;
            }
            else if (DateTime.Now.CompareTo(dtToDate) < 0)
            {
                MessageBox.Show("TO date cannot be future date.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtTo.Focus();
                retValue = false;
            }
            else if (timespan.Seconds < 0 || timespan.Minutes < 0 || timespan.Hours < 0 || timespan.Days < 0)
            {
                MessageBox.Show("FROM date can not be greater than TO date.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtFrom.Focus();
                retValue = false;
            }
            return retValue;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            List<string> idList = getSelectedIds();
            if (idList == null || idList.Count == 0)
            {
                MessageBox.Show("Please select at least one check box to generate the report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ReportConfigurationParameters parameters = new ReportConfigurationParameters();
            if (string.Equals(cmbSearchType.Text, "SCHEDULE", StringComparison.OrdinalIgnoreCase))
            {
                parameters.Type = ReportType.SCHEDULEWISE;
                parameters.ScheduleIds = idList.ToArray();
            }
            else
            {
                parameters.Type = ReportType.METERWISE;
                parameters.Meters = getSelectedMeterSerialNum().ToArray();
                parameters.ScheduleIds = getSelectedIds().ToArray();
                parameters.StartDate = dtFrom.Value;
                parameters.EndDate = dtTo.Value;
            }

            GPRSSchedulingReport report = new GPRSSchedulingReport(parameters);
            report.Show();
        }

        /// <summary>
        /// Retruns the list of selected task/meter id from gridview control
        /// </summary>
        /// <returns></returns>
        private List<string> getSelectedIds()
        {
            List<string> idList = new List<string>();
            foreach (DataGridViewRow row in gvSearchResult.Rows)
            {
                DataGridViewCheckBoxCell chkBox = row.Cells["Select"] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(chkBox.Value))
                {
                    idList.Add(Convert.ToString(row.Cells[colID].Value));
                }
            }
            return idList;
        }

        private List<string> getSelectedMeterSerialNum()
        {
            List<string> idList = new List<string>();
            foreach (DataGridViewRow row in gvSearchResult.Rows)
            {
                DataGridViewCheckBoxCell chkBox = row.Cells["Select"] as DataGridViewCheckBoxCell;
                DataGridViewCell chkCell = row.Cells["File Upload Id"] as DataGridViewCell;
                if (Convert.ToBoolean(chkBox.Value))
                {
                    idList.Add(Convert.ToString(chkCell.Value));
                }
            }
            return idList;
        }

        private void btnConfigReportParam_Click(object sender, EventArgs e)
        {
            SchedulingReportConfiguration config = new SchedulingReportConfiguration();
            config.MdiParent = this.Parent.Parent as Form;
            config.WindowState = FormWindowState.Maximized;
            config.Show();
        }

        private void cmbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnShow.Enabled = false;
        }       
    }
}
