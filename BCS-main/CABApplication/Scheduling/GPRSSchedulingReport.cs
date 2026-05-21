using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Framework;
using CAB.UI.Controls;
using CAB.BLL;
using Hunt.EPIC.Logging;

namespace CABApplication.Scheduling
{
    public partial class GPRSSchedulingReport : Form
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSSchedulingReport).ToString());
        ReportConfigurationParameters configurationParameters;
       
        /// <summary>
        /// Parameterized constructor  
        /// </summary>
        /// <param name="parameters"></param>
        public GPRSSchedulingReport(ReportConfigurationParameters parameters)
        {
            InitializeComponent();
            this.configurationParameters = parameters;

            // Iterating through profile and creating tab and adding tab at runtime 
            foreach (string name in Enum.GetNames(typeof(Profile)))
            {
                try
                {
                    TabPage tabPage = new TabPage(((Profile)Enum.Parse(typeof(Profile), name)).GetDisplayName());
                    tabPage.Name = "tab" + name;
                    tabReports.TabPages.Add(tabPage);
                    DataGridView dataGrid = new DataGridView();
                    dataGrid.ReadOnly = true;
                    dataGrid.AllowUserToAddRows = false;
               
                    tabPage.Controls.Add(dataGrid);
                }
                catch (Exception ex)    //Exception log for catch block 
                {
                    logger.Log(LOGLEVELS.Error, "GPRSSchedulingReport(ReportConfigurationParameters parameters)", ex);
                }
            }

            // by default first tab i.e General will be selected 
            backgroundWorker.WorkerReportsProgress = true;
            tabReports.SelectedIndex = (int)Profile.GENERAL;
            this.tabReports_SelectedIndexChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event handler to be triggered on tab selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync(tabReports.SelectedIndex);
            }
        }

        /// <summary>
        /// worker task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(100, ReportBLL.GetGPRSReportData(configurationParameters, (Profile)e.Argument));
        }

        /// <summary>
        /// background worker progress change handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                TabPage tabPage = tabReports.TabPages[tabReports.SelectedIndex] as TabPage;
                //tabPage.Size = tabPage.Parent.Size;
                DataGridView view = tabPage.Controls[0] as DataGridView;
                view.Size = new Size(view.Parent.Size.Width - 10, view.Parent.Size.Height - 10);
                DataTable reportTable = (e.UserState as DataTable);

                // in case of tamper report will have 
                if (tabReports.SelectedIndex == (int)Profile.TAMPER)
                {
                    view.AllowUserToAddRows = false;
                    view.AutoGenerateColumns = false;
                    view.Columns.Clear();

                    foreach (DataColumn column in reportTable.Columns)
                    {
                        try
                        {
                            DataGridViewColumn dgvColumn = null;

                            if (column.DataType == typeof(string))
                            {
                                dgvColumn = new DataGridViewTextBoxColumn();
                                dgvColumn.Name = column.ColumnName;
                                dgvColumn.HeaderText = column.ColumnName;
                            }
                            else if (column.DataType == typeof(bool))
                            {
                                dgvColumn = new DataGridViewCheckBoxColumn();
                                dgvColumn.Name = column.ColumnName;
                                dgvColumn.HeaderText = column.ColumnName;
                                dgvColumn.ReadOnly = true;
                            }

                            view.Columns.Add(dgvColumn);
                        }
                        catch (Exception ex)    //Exception log for catch block 
                        {
                            logger.Log(LOGLEVELS.Error, "backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)", ex);
                        }
                    }

                    view.Rows.Clear();

                    foreach (DataRow row in reportTable.Rows)
                    {
                        try
                        {
                            int index = view.Rows.Add();
                            view.Rows[index].SetValues(row.ItemArray);
                        }
                        catch (Exception ex)    //Exception log for catch block 
                        {
                            logger.Log(LOGLEVELS.Error, "backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)", ex);
                        }
                    }
                }
                else
                {
                    view.DataSource = reportTable;
                }

                view.AutoResizeColumns();
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)", ex);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        /// <summary>
        /// event to be triggered on elapse of timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync(tabReports.SelectedIndex);
            }
        }

        /// <summary>
        /// event handler to be trigger on click of download button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage tabPage = tabReports.TabPages[tabReports.SelectedIndex] as TabPage;
                DataTable reportTable = new DataTable();
                DataGridView view = tabPage.Controls[0] as DataGridView;

                if ((Profile)tabReports.SelectedIndex == Profile.TAMPER)
                {
                    foreach (DataGridViewColumn column in view.Columns)
                    {
                        try { reportTable.Columns.Add(column.Name); }
                        catch (Exception ex)    //Exception log for catch block 
                        {
                            logger.Log(LOGLEVELS.Error, "buttonDownload_Click(object sender, EventArgs e)", ex);
                        }
                    }

                    foreach (DataGridViewRow row in view.Rows)
                    {
                        try
                        {
                            DataRow dr = reportTable.NewRow();

                            foreach (DataGridViewColumn column in view.Columns)
                            {
                                try { dr[column.Name] = row.Cells[column.Name].Value; }
                                catch (Exception ex)    //Exception log for catch block 
                                {
                                    logger.Log(LOGLEVELS.Error, "buttonDownload_Click(object sender, EventArgs e)", ex);
                                }
                            }

                            reportTable.Rows.Add(dr);
                        }
                        catch (Exception ex)    //Exception log for catch block 
                        {
                            logger.Log(LOGLEVELS.Error, "buttonDownload_Click(object sender, EventArgs e)", ex);
                        }
                    }
                }
                else
                {
                    reportTable = view.DataSource as DataTable;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                // TODO: show error message
                logger.Log(LOGLEVELS.Error, "buttonDownload_Click(object sender, EventArgs e)", ex);
            }
        }
    }


}
