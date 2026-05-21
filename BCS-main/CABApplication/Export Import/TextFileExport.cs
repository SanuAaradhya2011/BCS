#region Namespaces
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using System.Collections.Generic;
using Hunt.EPIC.Logging;
using System.Linq;
#endregion

namespace CAB.UI
{
    public partial class TextFileExport : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ApplicationType apptype = ConfigInfo.GetApplicationType();
        private TextExportBLL textExportBLL = new TextExportBLL();
        private DLMS650CommonBLL dLMS650CommonBLL = new DLMS650CommonBLL();
        string meterDataID = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TextFileExport).ToString());
        enum ExportFileFormatCustomerType
        {
            CABDefault = 0,
            WB = 1,
            BSES = 2,
            Puducherry = 3,
            RelianceMumbai = 4,
            TorrentAhmedabad = 5,
            TorrentAgra = 6,
            TataPowerAdani = 7,
            CSPDCL = 8
        };
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public TextFileExport()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void TextFileExport_Load(object sender, EventArgs e)
        {
            cmbExportType.SelectedIndex = 0;
            rmcb.SelectedIndex = 0;
            cmbBillHistNo.SelectedIndex = 0;
            //DataSet mainListDataSet = meterDataBLL.FileExportListDataSet(true);
            /*DataSet mainListDataSet = meterDataBLL.PedFileExportListDataSet(true);
            if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
            {
                grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                foreach (DataGridViewColumn column in grdFileList.Columns)
                {
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
                SetTopGridEqualWidth(mainListDataSet);

                DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                checkBoxCol.Name = "Include";
                grdFileList.Columns.Add(checkBoxCol);
                grdFileList.Columns[3].HeaderText = "Include";
                grdFileList.Columns[3].ReadOnly = false;
            }
            else
            {
                lblErrorInfo.Visible = true;
                btnSave.Enabled = false;
                chkSelectAll.Enabled = false;
                return;
            }*/
        }

        private void LoadPEDMeterDetailList()
        {
            DataSet mainListDataSet = meterDataBLL.PedFileExportListDataSet(true);
            if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
            {
                lblErrorInfo.Visible = false;
                btnSave.Enabled = true;
                chkSelectAll.Enabled = true;
                
                grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                foreach (DataGridViewColumn column in grdFileList.Columns)
                {
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
                SetTopGridEqualWidth(mainListDataSet);

                DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                checkBoxCol.Name = "Include";
                grdFileList.Columns.Add(checkBoxCol);
                grdFileList.Columns[4].HeaderText = "Include";
                grdFileList.Columns[4].ReadOnly = false;
            }
            else
            {
                lblErrorInfo.Visible = true;
                btnSave.Enabled = false;
                chkSelectAll.Enabled = false;
                return;
            }
        }

        private void LoadOtherMeterDetailList()
        {
            // SB Change Start 20170915
            DataSet mainListDataSet = null;
            if (chkFilter.Checked)
            {
                long lStartDate = DateUtility.DateTimeToLong(dtpStartDate.Value.Date);
                long lEndDate = DateUtility.DateTimeToLong(dtpEndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                mainListDataSet = meterDataBLL.FileExportListFilteredDataSet(true, lStartDate, lEndDate);
            }
            else
            {
                mainListDataSet = meterDataBLL.FileExportListDataSet(true);
            }
            // Below code commented for filtering with Uploading Date Time.
            //DataSet mainListDataSet = meterDataBLL.FileExportListDataSet(true);
            // SB Change End 20170915

            //DataSet mainListDataSet = meterDataBLL.PedFileExportListDataSet(true);
            if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
            {
                lblErrorInfo.Visible = false;
                btnSave.Enabled = true;
                chkSelectAll.Enabled = true;
                
                grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                foreach (DataGridViewColumn column in grdFileList.Columns)
                {
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
                SetTopGridEqualWidth(mainListDataSet);

                DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                checkBoxCol.Name = "Include";
                grdFileList.Columns.Add(checkBoxCol);
                grdFileList.Columns[3].HeaderText = "Include";
                grdFileList.Columns[3].ReadOnly = false;
            }
            else
            {
                lblErrorInfo.Visible = true;
                btnSave.Enabled = false;
                chkSelectAll.Enabled = false;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.StatusMessage = string.Empty;
                bool flag = false;
                foreach (DataGridViewRow row in grdFileList.Rows)
                {
                    if (row.Cells["include"].Value != null)
                    {
                        flag = (bool)row.Cells["include"].Value;
                    }
                    if (flag)
                        break;
                }
                if (!flag)
                {
                    this.StatusMessage = "Please select file";
                    return;
                }

                FindAndWriteContents();
                Application.DoEvents();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.Cursor = Cursors.Default;
                logger.Log(LOGLEVELS.Error, "btnSave_Click(object sender, EventArgs e)", ex);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckedAll(chkSelectAll.Checked);
        }

        private void grdFileList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;

                    if ((bool)grdFileList.CurrentCell.EditedFormattedValue == false)
                        chkSelectAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < grdFileList.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = grdFileList["include", i] as DataGridViewCheckBoxCell;
                            if (cell.EditedFormattedValue == null || !(bool)cell.EditedFormattedValue)
                            {
                                IfAllRowsSelected = false;
                                break;
                            }
                        }
                        chkSelectAll.Checked = IfAllRowsSelected;
                    }
                    chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the grid width
        /// </summary>
        /// <param name="dataSet"></param>
        public void SetTopGridEqualWidth(DataSet dataSet)
        {
            int width = grdFileList.Width - 50;
            int totCol = grdFileList.Columns.Count;
            if (totCol == 0)
                return;
            int ExactWidth = width / totCol;
            int i = 1;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                if (i == 1)
                    ExactWidth = ExactWidth / 3;
                else if (i == 3)
                    ExactWidth = (width / totCol) - 50;
                else
                    ExactWidth = width / totCol;
                this.grdFileList.Columns[col.ColumnName].Width = ExactWidth;
                i++;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if the file is DLMS or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckDLMSFiles()
        {
            bool sep = false;
            string selectedFileName = string.Empty;
            bool isDLMSFile = false;
            foreach (DataGridViewRow row in grdFileList.Rows)
            {
                string val = Convert.ToString(grdFileList.Rows[grdFileList.Rows.IndexOf(row)].Cells["Include"].Value);
                if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val))
                {
                    selectedFileName = grdFileList.Rows[grdFileList.Rows.IndexOf(row)].Cells["File Name"].Value.ToString();
                    DataSet dataSet = textExportBLL.GetMeterDataIDByFileName(selectedFileName);
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        meterDataID = dataSet.Tables[0].Rows[0][0].ToString();
                    }
                    if (selectedFileName.Contains(".2NG"))
                    {
                        isDLMSFile = true;
                    }
                }
            }
            return isDLMSFile;
        }

        /// <summary>
        /// Used to export All available files instant and billing  data into text file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        private string ExportDataToTextFile(string fileLocation)
        {

            string fileName = string.Empty;
            string result = string.Empty;
            string noDataForExport = string.Empty;
            bool exportDataSuccess = false;
            string seperater = string.Empty;
            bool sep = false;
            StreamWriter streamWriter = new StreamWriter(fileLocation);
            DataSet dataSet = null;

            // SarkarA code change start 20171206//Reliance Mumbai Text Export
            DataSet dataSetInstant = null;
            int idCount = 1;
            // SarkarA code change end 20171206

            try
            {
                int seperatorCounter = 0;
                bool bWriteHeader = true;
                if (chkWithSeperator.Checked)
                    seperater = ",";
                foreach (DataGridViewRow gridRow in grdFileList.Rows)
                {
                    gridRow.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    string val = string.Empty;
                    try
                    {
                        val = Convert.ToString(gridRow.Cells["Include"].Value);
                        if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val))
                        {
                            #region Puducherry Format
                            if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.Puducherry)
                            {

                                string bsesFormatData = string.Empty;
                                DataSet bsesDataSet = new TextExportBLL().GetPuducherryDataForTextExport(long.Parse(gridRow.Cells[1].Value.ToString()), gridRow.Cells[2].Value.ToString());
                                if (bsesDataSet.Tables[0].Rows.Count != 0 && bsesDataSet.Tables[0].Rows.Count != null)
                                {
                                    foreach (DataRow dRow in bsesDataSet.Tables[0].Rows)
                                    {
                                        for (int colCount = 0; colCount < bsesDataSet.Tables[0].Columns.Count; colCount++)
                                        {
                                            bsesFormatData = bsesFormatData + dRow[colCount] + seperater;

                                            /*if (colCount == 1)
                                            {
                                                DateTime dateTime = DateUtility.LongToDateTime(Convert.ToInt64(dRow[colCount].ToString()));
                                                bsesFormatData = bsesFormatData + dateTime.Day.ToString("d2") + "/" + dateTime.Month.ToString("d2") + "/" + dateTime.Year.ToString("d4") + " " + dateTime.Hour.ToString("d2") + ":" + dateTime.Minute.ToString("d2") + seperater;
                                            }
                                            else if (colCount == 6)
                                            {
                                                TimeSpan ts = TimeSpan.FromMinutes(Convert.ToInt64(dRow[colCount].ToString()));
                                                Int64 temptotaldays = Convert.ToInt64(ts.Days);
                                                Int64 totalYears = temptotaldays / 365;
                                                Int64 totaldays = temptotaldays % 365;
                                                bsesFormatData = bsesFormatData + totalYears.ToString("d2").PadLeft(3, ' ') + totaldays.ToString("d3") + Convert.ToInt64(ts.Hours).ToString("d2") + Convert.ToInt64(ts.Minutes).ToString("d2") + seperater;
                                            }
                                            else
                                            {
                                                bsesFormatData = bsesFormatData + dRow[colCount] + seperater;
                                            }*/
                                        }

                                        if (bsesFormatData != string.Empty && bsesFormatData != null)
                                        {
                                            bsesFormatData = bsesFormatData.TrimEnd(seperater.ToCharArray());
                                            streamWriter.Write(bsesFormatData);
                                            bsesFormatData = string.Empty;
                                            exportDataSuccess = true;
                                        }
                                        else
                                        {
                                            noDataForExport = noDataForExport + "Data not available for Meter Readout(s) for " + gridRow.Cells[2].Value.ToString() + " file.\n";
                                        }

                                    }
                                }
                                else
                                {
                                    noDataForExport = noDataForExport + "Data not available for Meter Readout(s) for " + gridRow.Cells[2].Value.ToString() + " file.\n";
                                }


                            }
                            #endregion



                            #region lngFolrmat
                            if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.CABDefault) //(lngFormat.Checked)
                            {
                                fileName = gridRow.Cells["File Name"].Value.ToString();
                                FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                                FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                                MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                                DataSet dataForExport = new TextExportBLL().GetDataForTextExport(meterDataEntity.MeterData_ID);
                                if (dataForExport != null && dataForExport.Tables != null && dataForExport.Tables.Count > 0)
                                {
                                    if (dataForExport.Tables[0].Rows.Count > 0)
                                    {
                                        DataTable dataInTable = dataForExport.Tables[0];
                                        dataInTable = FormatData(dataInTable);
                                        result = "File exported successfully.";

                                        //write rows to text file
                                        foreach (DataRow row in dataInTable.Rows)
                                        {

                                            //Write columns values of  selected row
                                            foreach (DataColumn column in dataInTable.Columns)
                                            {
                                                if (row[column] != null)
                                                {
                                                    if (seperatorCounter != 0)
                                                        streamWriter.Write(seperater);
                                                    seperatorCounter++;

                                                    if (column.ColumnName.ToUpper() == "RTC" || column.ColumnName.ToUpper() == "READINGDATETIME"
                                                                               || column.ColumnName.ToUpper() == "BILLINGDATE")
                                                    {

                                                        string data = Convert.ToString(DateUtility.LongToStringDateTimeWithSecFormat(Int64.Parse(row[column].ToString()))).Replace("/", "");
                                                        streamWriter.Write(data.Replace(" ", seperater));
                                                        streamWriter.Write(" ");
                                                        streamWriter.Write(" ");

                                                    }
                                                    else
                                                    {
                                                        if (column.ColumnName.ToUpper() == "METER SERIAL NO")
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                            streamWriter.Write(seperater);
                                                            //Meter Make code not available so display 250.
                                                            streamWriter.Write("250");
                                                        }
                                                        else if (column.ColumnName.ToUpper() == "RN_VOLTAGE")
                                                        {

                                                            streamWriter.Write("000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                        }
                                                        else if (column.ColumnName.ToUpper() == "RN_CURRENT")
                                                        {

                                                            streamWriter.Write("0000000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("0000000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("0000000000");
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write(Convert.ToString(row[column]));

                                                        }
                                                        else if (column.ColumnName.ToUpper() == "CUMULATIVEENERGYKVAHTZ1")
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("00000000000000");
                                                        }
                                                        else if (column.ColumnName.ToUpper() == "CUMULATIVEENERGYKVAHTZ2")
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("00000000000000");
                                                        }
                                                        else if (column.ColumnName.ToUpper() == "CUMULATIVEENERGYKVAHTZ3")
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("00000000000000");
                                                        }
                                                        else if (column.ColumnName.ToUpper() == "CUMULATIVEENERGYKVAHTZ4")
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                            streamWriter.Write(seperater);
                                                            streamWriter.Write("00000000000000");
                                                        }
                                                        else
                                                        {

                                                            streamWriter.Write(Convert.ToString(row[column]));
                                                        }
                                                        exportDataSuccess = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (seperatorCounter != 0)
                                                        streamWriter.Write(seperater);
                                                    seperatorCounter++;
                                                    //streamWriter.Write("\t");
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    result = "Data Not Available for Export.";
                                }
                            }
                            #endregion

                            #region WBFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.WB)//(wbFormat.Checked)
                            {
                                fileName = gridRow.Cells["File Name"].Value.ToString();
                                FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                                FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                                MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                                string wbFormatData = string.Empty;
                                DataSet wbDataSet = new TextExportBLL().GetWBDataForTextExport(meterDataEntity.FileUpload_ID);
                                if (wbDataSet.Tables[0].Rows.Count != 0 && wbDataSet.Tables[0].Rows.Count != null)
                                {
                                    foreach (DataRow dRow in wbDataSet.Tables[0].Rows)
                                    {
                                        for (int colCount = 0; colCount < wbDataSet.Tables[0].Columns.Count; colCount++)
                                        {


                                            if (colCount == 1)
                                            {
                                                DateTime dateTime = DateUtility.LongToDateTime(Convert.ToInt64(dRow[colCount].ToString()));
                                                wbFormatData = wbFormatData + dateTime.ToString("ddMMyyyy" + seperater + "HHmmss") + seperater;
                                            }
                                            else
                                            {
                                                wbFormatData = wbFormatData + dRow[colCount] + seperater;
                                            }
                                        }

                                        if (wbFormatData != string.Empty && wbFormatData != null)
                                        {
                                            wbFormatData = wbFormatData.TrimEnd(seperater.ToCharArray());
                                            streamWriter.WriteLine("#" + wbFormatData);
                                            wbFormatData = string.Empty;
                                            exportDataSuccess = true;
                                        }
                                        else
                                        {
                                            noDataForExport = noDataForExport + "History Data not available for Meter Readout(s) for " + entity.FileName + " file.\n";
                                        }

                                    }
                                }
                                else
                                {
                                    noDataForExport = noDataForExport + "History Data not available for Meter Readout(s) for " + entity.FileName + " file.\n";
                                }


                            }
                            #endregion

                            #region BSESFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.BSES)//(BSESFormat.Checked)
                            {
                                fileName = gridRow.Cells["File Name"].Value.ToString();
                                FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                                FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                                MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                                string bsesFormatData = string.Empty;
                                string bsesFormatData1 = string.Empty;
                                DataSet bsesDataSet = new TextExportBLL().GetBSESDataForTextExport(meterDataEntity.FileUpload_ID);

                                DataSet bsesDataSet1 = new TextExportBLL().GetBSESDataForTextExport1(meterDataEntity.FileUpload_ID);

                                DataSet bsesDataSet3 = new TextExportBLL().GetBSESDataForTextInstant(meterDataEntity.FileUpload_ID);


                                for (int i = 0; i < bsesDataSet.Tables[0].Rows.Count; i++)
                                {
                                    for (int k = 0; k < bsesDataSet1.Tables[0].Rows.Count; k++)
                                    {
                                        for (int j = 0; j < bsesDataSet3.Tables[0].Rows.Count; j++)
                                        {
                                            if (bsesDataSet1.Tables[0].Columns[4].ToString() == "MDKWTZ0")
                                            {
                                                bsesDataSet.Tables[0].Rows[i][4] = bsesDataSet1.Tables[0].Rows[k][4].ToString();
                                            }
                                            if (bsesDataSet1.Tables[0].Columns[5].ToString() == "MDKVATZ0")
                                            {
                                                bsesDataSet.Tables[0].Rows[i][5] = bsesDataSet1.Tables[0].Rows[k][5].ToString();

                                            }
                                            if (bsesDataSet3.Tables[0].Columns[0].ToString() == "PowerOffDuration")
                                            {
                                                //  bsesDataSet.Tables[0].Rows[i][6] = bsesDataSet3.Tables[0].Rows[j][0].ToString();

                                                int min = int.Parse(bsesDataSet3.Tables[0].Rows[j][0].ToString().Split('*')[0]);
                                                TimeSpan span = new TimeSpan(0, 0, min, 0);
                                                bsesDataSet.Tables[0].Rows[i][6] = String.Format("{0}:{1:00}:{2:00}", span.Days, span.Hours, span.Minutes).Replace(":", "").PadLeft(9, '0');
                                            }
                                        }
                                        break;

                                    }
                                }





                                if (bsesDataSet.Tables[0].Rows.Count != 0 && bsesDataSet.Tables[0].Rows.Count != null)
                                {
                                    foreach (DataRow dRow in bsesDataSet.Tables[0].Rows)
                                    {

                                        for (int colCount = 0; colCount < bsesDataSet.Tables[0].Columns.Count; colCount++)
                                        {
                                            if (colCount == 1)
                                            {
                                                DateTime dateTime = DateUtility.LongToDateTime(Convert.ToInt64(dRow[colCount].ToString()));
                                                bsesFormatData = bsesFormatData + dateTime.Day.ToString("d2") + "/" + dateTime.Month.ToString("d2") + "/" + dateTime.Year.ToString("d4") + " " + dateTime.Hour.ToString("d2") + ":" + dateTime.Minute.ToString("d2") + seperater;
                                            }
                                            else if (colCount == 6)
                                            {


                                                bsesFormatData = bsesFormatData + " " + dRow[colCount] + seperater;
                                                //if (dRow[colCount].ToString() == "")
                                                //{
                                                //}
                                                //else
                                                //{
                                                //    ////int.Parse(bsesDataSet3.Tables[0].Rows[j][0].ToString().Split('*')[0])
                                                //    //TimeSpan ts = TimeSpan.FromMinutes(int.Parse(dRow[colCount].ToString()));
                                                //    //Int64 temptotaldays = Convert.ToInt64(ts.Days);
                                                //    //Int64 totalYears = temptotaldays / 365;
                                                //    //Int64 totaldays = temptotaldays % 365;
                                                //    //bsesFormatData = bsesFormatData + totalYears.ToString("d2").PadLeft(3, ' ') + totaldays.ToString("d3") + Convert.ToInt64(ts.Hours).ToString("d2") + Convert.ToInt64(ts.Minutes).ToString("d2") + seperater;
                                                //}
                                            }
                                            else
                                            {
                                                if (colCount == 0)
                                                {
                                                    bsesFormatData = bsesFormatData + dRow[colCount];
                                                    //  bsesFormatData = bsesFormatData.Trim();
                                                    bsesFormatData = bsesFormatData.PadLeft(8, '0') + seperater;

                                                }
                                                else
                                                {
                                                    bsesFormatData = bsesFormatData + dRow[colCount] + seperater;
                                                }


                                            }
                                        }

                                        if (bsesFormatData != string.Empty && bsesFormatData != null)
                                        {

                                            bsesFormatData = bsesFormatData.TrimEnd(seperater.ToCharArray());

                                            streamWriter.Write(bsesFormatData);

                                            bsesFormatData = string.Empty;
                                            exportDataSuccess = true;
                                            sep = true;
                                        }
                                        else
                                        {
                                            noDataForExport = noDataForExport + "History Data not available for Meter Readout(s) for " + entity.FileName + " file.\n";
                                        }

                                    }
                                }
                                else
                                {
                                    noDataForExport = noDataForExport + "History Data not available for Meter Readout(s) for " + entity.FileName + " file.\n";
                                }


                            }
                            #endregion

                            #region TorrentAhmedabadFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.TorrentAhmedabad)
                            {
                                fileName = gridRow.Cells["File Name"].Value.ToString();
                                List<string> lstMeterDataID = textExportBLL.GetMeterDataIDListByFileName(fileName);



                                string FormatData = string.Empty;

                                string sqlQuery = String.Empty;

                                foreach (string meterDataID in lstMeterDataID)
                                {
                                    dataSet = textExportBLL.GetTorrentDataForTextExport(meterDataID);
                                    if (dataSet != null && dataSet.Tables[0].Rows.Count != 0)
                                    {
                                        foreach (DataRow drow in dataSet.Tables[0].Rows)
                                        {
                                            foreach (DataColumn column in dataSet.Tables[0].Columns)
                                            {
                                                string tempdata = "";
                                                long dt;
                                                switch (column.ColumnName)
                                                {
                                                    case "meterSerialNumber":
                                                        tempdata = drow[column].ToString().Trim().PadLeft(8, '0');
                                                        break;
                                                    case "RESET":
                                                        dt = 0;
                                                        if (long.TryParse(drow[column].ToString(), out dt))
                                                            tempdata = String.Format("{0:00000000}", dt);
                                                        break;
                                                    case "ReadingDateTime":
                                                        dt = 0;
                                                        if (long.TryParse(drow[column].ToString(), out dt))
                                                            tempdata = String.Format("{0:00000000}", DateUtility.LongToDateTime(dt).ToString("ddMMyyyy:HHmmss", CultureInfo.InvariantCulture));
                                                        break;
                                                    case "BillingDate":
                                                        dt = 0;
                                                        if (long.TryParse(drow[column].ToString(), out dt))
                                                            tempdata = String.Format("{0:00000000}", DateUtility.LongToDateTime(dt).ToString("ddMMyyyy", CultureInfo.InvariantCulture));
                                                        break;
                                                    case "Manufacturer":
                                                    case "Total Reverse KWH":
                                                        tempdata = drow[column].ToString();
                                                        break;
                                                    case "CumulativeEnergykWhTZ0":
                                                    case "CumulativeEnergykvarhLag":
                                                    case "MDkWTZ0":
                                                    case "Cumulative MDKW":
                                                        Double d = 0;
                                                        if (Double.TryParse(drow[column].ToString(), out d))
                                                            tempdata = String.Format("{0:0000000000.00}", d);
                                                        break;

                                                    default:
                                                        break;
                                                }
                                                FormatData = FormatData + tempdata + seperater;
                                            }
                                            if (FormatData != string.Empty && FormatData != null)
                                            {
                                                FormatData = FormatData.TrimEnd(seperater.ToCharArray());
                                                streamWriter.WriteLine(FormatData);
                                                FormatData = string.Empty;
                                                exportDataSuccess = true;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region RelianceMumbaiFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.RelianceMumbai)
                            {
                                //rmcb.Visible = true;  // SarkarA code change start 20171206//Reliance Mumbai Text Export


                                fileName = gridRow.Cells["File Name"].Value.ToString();
                                List<string> lstMeterDataID = textExportBLL.GetMeterDataIDListByFileName(fileName);

                                //FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                                //FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                                //MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                                string FormatData = string.Empty;

                                // SarkarA code change start 20171206//Reliance Mumbai Text Export
                                string sqlQuery = String.Empty;

                                foreach (string meterDataID in lstMeterDataID)
                                {
                                    string meterVariant = dLMS650CommonBLL.GetMeterVariantByMeterDataID(Convert.ToInt32(meterDataID));

                                    if (cmbRelianceMumbaiDataType.Text.Equals("Billing"))
                                    {
                                        int historyNo = cmbBillHistNo.SelectedIndex + 1;
                                        if (rmcb.SelectedIndex == (int)RelianceBilling.NoSolar)
                                        {
                                            dataSet = textExportBLL.GetRelianceMumbaiDataForTextExport(meterDataID, meterVariant);
                                        }
                                        else if (rmcb.SelectedIndex == (int)RelianceBilling.Solar_No_ImportkVAh)
                                        {
                                            dataSet = textExportBLL.GetRelianceMumbaiDataForTextExportwithsolar(meterDataID, meterVariant, (int)RelianceBilling.Solar_No_ImportkVAh, historyNo);
                                        }
                                        else if (rmcb.SelectedIndex == (int)RelianceBilling.Solar)
                                        {
                                            dataSet = textExportBLL.GetRelianceMumbaiDataForTextExportwithsolar(meterDataID, meterVariant, (int)RelianceBilling.Solar, historyNo);
                                        }
                                    }
                                    else if (cmbRelianceMumbaiDataType.Text.Equals("LoadSurvey"))
                                    {
                                        dataSet = textExportBLL.GetRelianceLoadSurveyDataForTextExport(meterDataID);
                                    }
                                    else if (cmbRelianceMumbaiDataType.Text.Equals("Instant"))
                                    {
                                        dataSet = textExportBLL.GetRelianceInstantData1ForTextExport(meterDataID);
                                        dataSetInstant = textExportBLL.GetRelianceInstantData2ForTextExport(meterDataID);
                                    }
                                    else if (cmbRelianceMumbaiDataType.Text.Equals("Event"))
                                    {
                                        dataSet = textExportBLL.GetRelianceEventDataForTextExport(meterDataID);
                                    }
                                    // SarkarA code change end 20171206

                                    if (dataSet.Tables[0].Rows.Count != 0 && dataSet.Tables[0].Rows.Count != null)
                                    {
                                        // SarkarA code change start 20171206//Reliance Mumbai Text Export
                                        if (cmbRelianceMumbaiDataType.Text.Equals("Instant"))
                                        {
                                            Dictionary<string, string> instantDataDictionary = new Dictionary<string, string>();
                                            instantDataDictionary.Add("FileName", fileName);
                                            int angleYR = 0;
                                            int angleBR = 0;
                                            string date = String.Empty;
                                            string time = String.Empty;
                                            DateTime dateTime;
                                            double dPowerFactorR = 0.0;
                                            double dMeterCurrentR = 0.0;
                                            double dPowerFactorY = 0.0;
                                            double dMeterCurrentY = 0.0;
                                            double dPowerFactorB = 0.0;
                                            double dMeterCurrentB = 0.0;
                                            double dPowerFactor3P = 0.0;
                                            double dReactivePower = 0.0;
                                            string sTemp = String.Empty;
                                            double tempDouble = 0.0d;

                                            DataRow dRow2 = dataSet.Tables[0].Rows[0];

                                            foreach (DataColumn cl in dataSet.Tables[0].Columns)
                                            {
                                                switch (cl.ColumnName)
                                                {
                                                    case "UploadingDateTime":
                                                        dateTime = DateUtility.LongToDateTime(Int64.Parse(dRow2[cl].ToString()));
                                                        date = dateTime.Day.ToString("d2") + dateTime.Month.ToString("d2") + dateTime.Year.ToString("d4");
                                                        time = dateTime.Hour.ToString("d2") + dateTime.Minute.ToString("d2");
                                                        instantDataDictionary.Add("UploadDate", date);
                                                        instantDataDictionary.Add("UploadTime", time);
                                                        break;
                                                    case "ReadingDateTime":
                                                        dateTime = DateUtility.LongToDateTime(Int64.Parse(dRow2[cl].ToString()));
                                                        date = dateTime.Day.ToString("d2") + dateTime.Month.ToString("d2") + dateTime.Year.ToString("d4");
                                                        time = dateTime.Hour.ToString("d2") + dateTime.Minute.ToString("d2");
                                                        instantDataDictionary.Add("ReadingDate", date);
                                                        instantDataDictionary.Add("ReadingTime", time);
                                                        break;
                                                    case "Meter_InstalledCTRatio":
                                                        instantDataDictionary.Add(cl.ColumnName, dRow2[cl].ToString());
                                                        instantDataDictionary.Add("Commissioned CT", "1.0");
                                                        break;
                                                    case "Meter_InstalledPTRatio":
                                                        instantDataDictionary.Add(cl.ColumnName, dRow2[cl].ToString());
                                                        instantDataDictionary.Add("Commissioned PT", "1.0");
                                                        break;
                                                    case "AngleYR":
                                                        instantDataDictionary.Add(cl.ColumnName, dRow2[cl].ToString());
                                                        Int32.TryParse(dRow2[cl].ToString(), out angleYR);
                                                        break;
                                                    case "AngleBR":
                                                        instantDataDictionary.Add(cl.ColumnName, dRow2[cl].ToString());
                                                        Int32.TryParse(dRow2[cl].ToString(), out angleBR);
                                                        break;
                                                    default:
                                                        instantDataDictionary.Add(cl.ColumnName, dRow2[cl].ToString());
                                                        break;
                                                }
                                            }

                                            foreach (DataRow dRow in dataSetInstant.Tables[0].Rows)
                                            {
                                                if (dRow["InstantPowerColumnName"].ToString().Equals("Real Time Clock - Date and Time"))
                                                {
                                                    dateTime = DateUtility.LongToDateTime(Int64.Parse(dRow["InstantPowerColumnValue"].ToString()));
                                                    date = dateTime.Day.ToString("d2") + dateTime.Month.ToString("d2") + dateTime.Year.ToString("d4");
                                                    time = dateTime.Hour.ToString("d2") + dateTime.Minute.ToString("d2");
                                                    instantDataDictionary.Add("MeterDate", date);
                                                    instantDataDictionary.Add("MeterTime", time);
                                                }
                                                else
                                                {
                                                    instantDataDictionary.Add(dRow["InstantPowerColumnName"].ToString(), CommonBLL.RemoveUnit(dRow["InstantPowerColumnValue"].ToString()));
                                                }
                                            }

                                            instantDataDictionary.TryGetValue("Current - IR", out sTemp);
                                            Double.TryParse(sTemp, out dMeterCurrentR);
                                            instantDataDictionary.TryGetValue("Current - IY", out sTemp);
                                            Double.TryParse(sTemp, out dMeterCurrentY);
                                            instantDataDictionary.TryGetValue("Current - IB", out sTemp);
                                            Double.TryParse(sTemp, out dMeterCurrentB);

                                            instantDataDictionary.TryGetValue("Signed Power Factor - R Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out dPowerFactorR);
                                            instantDataDictionary.TryGetValue("Signed Power Factor - Y Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out dPowerFactorY);
                                            instantDataDictionary.TryGetValue("Signed Power Factor - B Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out dPowerFactorB);
                                            instantDataDictionary.TryGetValue("Signed Power Factor (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out dPowerFactor3P);
                                            instantDataDictionary.TryGetValue("Signed Reactive Power - kvar (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out dReactivePower);

                                            instantDataDictionary.Add("Active Current R", String.Format("{0:000.00000}", (dMeterCurrentR * dPowerFactorR)));
                                            instantDataDictionary.Add("Active Current Y", String.Format("{0:000.00000}", (dMeterCurrentY * dPowerFactorY)));
                                            instantDataDictionary.Add("Active Current B", String.Format("{0:000.00000}", (dMeterCurrentB * dPowerFactorB)));

                                            instantDataDictionary.Add("MeterScaling", "1.0");
                                            instantDataDictionary.Add("VoltUnit", "V");

                                            FormatData = idCount.ToString().PadLeft(3, '0') + seperater;

                                            instantDataDictionary.TryGetValue("Consumer_Number", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(12, '0') + seperater;

                                            instantDataDictionary.TryGetValue("MeterID", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(8, '0') + seperater;

                                            instantDataDictionary.TryGetValue("CMRI_Number", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(8, '0') + seperater;

                                            instantDataDictionary.TryGetValue("FileName", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("UploadDate", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("UploadTime", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("ReadingDate", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("ReadingTime", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("MeterDate", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("MeterTime", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("MeterScaling", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(9, '0') + seperater;

                                            instantDataDictionary.TryGetValue("MeterType_Name", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("Frequency", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(6, '0') + seperater;

                                            instantDataDictionary.TryGetValue("PhaseSequence", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("VoltUnit", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("Voltage - VRN", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Voltage - VYN", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Voltage - VBN", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Current - IR", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Active Current R", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Reactive Current - R", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Current - IY", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Active Current Y", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Reactive Current - Y", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Current - IB", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Active Current B", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Reactive Current - B", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Signed Power Factor - R Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0.0000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Signed Power Factor - Y Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0.0000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Signed Power Factor - B Phase (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0.0000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Signed Power Factor (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0.0000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Active Power (ABS)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0000.00000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Signed Reactive Power - kvar (+Lag;-Lead)", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:0000.00000}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("EMFApplied", out sTemp);
                                            FormatData = FormatData + sTemp + seperater;

                                            instantDataDictionary.TryGetValue("Meter_InstalledCTRatio", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(5, '0') + seperater;

                                            instantDataDictionary.TryGetValue("Meter_InstalledPTRatio", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(5, '0') + seperater;

                                            instantDataDictionary.TryGetValue("Commissioned CT", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(5, '0') + seperater;

                                            instantDataDictionary.TryGetValue("Commissioned PT", out sTemp);
                                            FormatData = FormatData + sTemp.PadLeft(5, '0') + seperater;

                                            instantDataDictionary.TryGetValue("AngleYR", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("AngleBR", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            instantDataDictionary.TryGetValue("Neutral Current", out sTemp);
                                            Double.TryParse(sTemp, out tempDouble);
                                            FormatData = FormatData + String.Format("{0:000.00}", tempDouble) + seperater;

                                            if (FormatData != string.Empty && FormatData != null)
                                            {
                                                FormatData = FormatData.TrimEnd(seperater.ToCharArray());
                                                streamWriter.WriteLine(FormatData);
                                                FormatData = string.Empty;
                                                exportDataSuccess = true;
                                            }
                                        }

                                        else if (cmbRelianceMumbaiDataType.Text.Equals("Event"))
                                        {
                                            int i = 0, j = 0, idOccur = 0, idRestore = 0;
                                            long dTime = 0;
                                            DateTime occurTime, restoreTime;
                                            DataTable dataTable = dataSet.Tables[0];
                                            TimeSpan duration;

                                            DataColumn dataCol = dataTable.Columns.Add("DateTimeEventEnd", typeof(string));
                                            dataCol.AllowDBNull = true;

                                            dataCol = new DataColumn("CumulativeCount", typeof(Int32));
                                            dataCol.DefaultValue = 1;
                                            dataCol.AllowDBNull = false;
                                            dataTable.Columns.Add(dataCol);

                                            dataCol = new DataColumn("CumulativeDuration", typeof(TimeSpan));
                                            dataCol.DefaultValue = TimeSpan.Zero;
                                            dataCol.AllowDBNull = false;
                                            dataTable.Columns.Add(dataCol);

                                            dataCol = dataTable.Columns.Add("DateTimeEventActual", typeof(DateTime));
                                            dataCol.AllowDBNull = true;

                                            foreach (DataRow dRow in dataTable.Rows)
                                            {
                                                dRow["DateTimeEventActual"] = DateUtility.LongToDateTime(Int64.Parse(dRow["DateTimeEvent"].ToString()));
                                            }

                                            for (i = 0; i < dataTable.Rows.Count; i++)
                                            {
                                                Int32.TryParse(dataTable.Rows[i]["TamperTypeID"].ToString(), out idOccur);
                                                if ((idOccur % 2) != 0)
                                                {
                                                    for (j = i + 1; j < dataTable.Rows.Count; j++)
                                                    {
                                                        Int32.TryParse(dataTable.Rows[j]["TamperTypeID"].ToString(), out idRestore);
                                                        if (idRestore == idOccur + 1)
                                                        {
                                                            dataTable.Rows[i]["DateTimeEventEnd"] = dataTable.Rows[j]["DateTimeEvent"];
                                                            occurTime = DateUtility.LongToDateTime(Int64.Parse(dataTable.Rows[i]["DateTimeEvent"].ToString()));
                                                            restoreTime = DateUtility.LongToDateTime(Int64.Parse(dataTable.Rows[i]["DateTimeEventEnd"].ToString()));
                                                            duration = restoreTime - occurTime;
                                                            dataTable.Rows[i]["CumulativeDuration"] = duration;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    dataSet.Tables[0].Rows[i]["DateTimeEventEnd"] = 0;
                                                }
                                            }
                                            foreach (DataRow dRow in dataTable.Rows)
                                            {
                                                if (dRow["TamperType"].ToString().Contains("Restoration"))
                                                {
                                                    dRow.Delete();
                                                }
                                            }
                                            dataTable.AcceptChanges();

                                            DataView viewTable = dataTable.DefaultView;
                                            viewTable.Sort = "TamperTypeID ASC";
                                            dataTable = viewTable.ToTable();

                                            for (i = 1; i < dataTable.Rows.Count; i++)
                                            {
                                                if (dataTable.Rows[i]["TamperTypeID"].ToString().Equals(dataTable.Rows[i - 1]["TamperTypeID"].ToString()))
                                                {
                                                    dataTable.Rows[i]["CumulativeCount"] = int.Parse(dataTable.Rows[i - 1]["CumulativeCount"].ToString()) + 1;
                                                    if (dataTable.Rows[i - 1]["DateTimeEventEnd"].ToString() != String.Empty)
                                                    {
                                                        dataTable.Rows[i]["CumulativeDuration"] = TimeSpan.Parse(dataTable.Rows[i - 1]["CumulativeDuration"].ToString()) + TimeSpan.Parse(dataTable.Rows[i]["CumulativeDuration"].ToString());
                                                    }
                                                }
                                            }
                                            viewTable = dataTable.DefaultView;
                                            viewTable.Sort = "DateTimeEventActual ASC";
                                            dataTable = viewTable.ToTable();
                                            foreach (DataRow dRow in dataTable.Rows)
                                            {
                                                foreach (DataColumn cl in dataTable.Columns)
                                                {
                                                    switch (cl.ColumnName)
                                                    {
                                                        case "TamperType":
                                                        case "DateTimeEventActual":
                                                            continue;
                                                        case "MeterID":
                                                            FormatData = FormatData + dRow[cl].ToString().PadLeft(8, '0') + seperater;
                                                            break;
                                                        case "TamperTypeID":
                                                            FormatData = FormatData + dRow[cl].ToString().PadLeft(50, '0') + seperater;
                                                            break;
                                                        case "DateTimeEvent":
                                                        case "DateTimeEventEnd":
                                                            if (Int64.TryParse(dRow[cl].ToString(), out dTime))
                                                            {
                                                                occurTime = DateUtility.LongToDateTime(dTime);
                                                                FormatData = FormatData + occurTime.Day.ToString("d2") + occurTime.Month.ToString("d2") + occurTime.Year.ToString("d4") + occurTime.Hour.ToString("d2") + occurTime.Minute.ToString("d2") + seperater;
                                                            }
                                                            else
                                                            {
                                                                FormatData = FormatData + seperater;
                                                            }
                                                            break;
                                                        case "CumulativeCount":
                                                            FormatData = FormatData + dRow[cl].ToString().PadLeft(4, '0') + seperater;
                                                            break;
                                                        case "CumulativeDuration":
                                                            TimeSpan.TryParse(dRow[cl].ToString(), out duration);
                                                            FormatData = FormatData + String.Format("{0:00}", duration.Days) + String.Format("{0:00}", duration.Hours) + String.Format("{0:00}", duration.Minutes) + String.Format("{0:00}", duration.Seconds) + seperater;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                if (FormatData != string.Empty && FormatData != null)
                                                {
                                                    FormatData = FormatData.TrimEnd(seperater.ToCharArray());
                                                    streamWriter.WriteLine(FormatData);
                                                    FormatData = string.Empty;
                                                }
                                            }
                                            exportDataSuccess = true;
                                        }
                                        // SarkarA code change end 20171207


                                        else if (cmbRelianceMumbaiDataType.Text.Equals("Billing"))
                                        {
                                            // SarkarA code change end 20171208
                                            seperater = "|";
                                            foreach (DataRow dRow in dataSet.Tables[0].Rows)
                                            {
                                                FormatData = string.Empty + seperater;
                                                string kvaValue = string.Empty;

                                                if (dataSet.Tables[0].Rows[0]["MDkVATZ0Import"] != null)
                                                {
                                                    kvaValue = Convert.ToString(dataSet.Tables[0].Rows[0]["MDkVATZ0Import"]);
                                                }

                                                for (int colCount = 0; colCount < dataSet.Tables[0].Columns.Count; colCount++)
                                                {
                                                    if (Convert.ToString(dRow[colCount]) == string.Empty && dataSet.Tables[0].Columns[colCount].ColumnName != "MDkVATZ3Import")
                                                    {
                                                        FormatData = FormatData + "" + seperater;
                                                    }
                                                    else
                                                    {

                                                        if (dataSet.Tables[0].Columns[colCount].ColumnName == "BillingDate" || dataSet.Tables[0].Columns[colCount].ColumnName == "MDkVADateTimeTZ0")
                                                        {
                                                            if (dRow[colCount].ToString() == "0")
                                                            {
                                                                FormatData += seperater;
                                                            }
                                                            else
                                                            {
                                                                DateTime dateTime = DateUtility.LongToDateTime(Convert.ToInt64(dRow[colCount].ToString()));
                                                                FormatData = FormatData + dateTime.Day.ToString("d2") + "/" + dateTime.Month.ToString("d2") + "/" + dateTime.Year.ToString("d4") + " " + dateTime.Hour.ToString("d2") + ":" + dateTime.Minute.ToString("d2") + seperater;
                                                            }
                                                        }
                                                        else if (dataSet.Tables[0].Columns[colCount].ColumnName == "meterSerialNumber" || dataSet.Tables[0].Columns[colCount].ColumnName == "Consumer_Number")
                                                        {
                                                            FormatData = FormatData + dRow[colCount] + seperater;
                                                        }
                                                        //else if (dataSet.Tables[0].Columns[colCount].ColumnName == "MDkVATZ0Import")
                                                        //{
                                                        //    kvaValue = Convert.ToString(dRow[colCount]);
                                                        //}
                                                        else if (dataSet.Tables[0].Columns[colCount].ColumnName == "MDkVATZ3Import" && Convert.ToString(dRow[colCount]) == string.Empty)
                                                        {
                                                            FormatData = FormatData + kvaValue.Substring(0, 5) + "." + kvaValue.Substring(5, 3) + seperater;
                                                        }
                                                        else
                                                        {
                                                            string data = Convert.ToString(dRow[colCount]);
                                                            switch (data.Length)
                                                            {
                                                                case 9:
                                                                    FormatData = FormatData + data.Substring(0, 7) + "." + data.Substring(7, 2) + seperater;
                                                                    break;
                                                                case 8:
                                                                    FormatData = FormatData + data.Substring(0, 5) + "." + data.Substring(5, 3) + seperater;
                                                                    break;
                                                                default:
                                                                    FormatData = FormatData + data + seperater;
                                                                    break;
                                                            }

                                                        }
                                                    }

                                                }

                                                if (FormatData != string.Empty && FormatData != null)
                                                {
                                                    FormatData = (cmbRelianceMumbaiDataType.Text.Equals("Billing") && cmbRelianceMumbaiDataType.Visible && rmcb.SelectedIndex != (int)RelianceBilling.NoSolar) ? FormatData : FormatData.TrimEnd(seperater.ToCharArray());
                                                    streamWriter.WriteLine(FormatData);
                                                    FormatData = string.Empty;
                                                    exportDataSuccess = true;
                                                }
                                            }
                                        }
                                        // SarkarA code change start 20171206//Reliance Mumbai Text Export
                                        //SarkarA code change start 201800227 // add net kwh,kvah
                                        else if (cmbRelianceMumbaiDataType.Text.Equals("LoadSurvey"))
                                        {
                                            DataTable dtClone = dataSet.Tables[0].Clone();
                                            dtClone.Columns["IntervalStart"].DataType = typeof(String);
                                            dtClone.Columns["IntervalEnd"].DataType = typeof(String);
                                            foreach (DataRow row in dataSet.Tables[0].Rows)
                                            {
                                                dtClone.ImportRow(row);
                                            }
                                            DateTime dateTimeStart = DateTime.MinValue;
                                            DateTime dateTimeEnd = DateTime.MaxValue;
                                            for (int i = 0; i < dtClone.Rows.Count - 1; i++)
                                            {
                                                dateTimeStart = DateUtility.LongToDateTime(Int64.Parse(dtClone.Rows[i + 1]["realTimeClockDateandTime"].ToString()));
                                                dateTimeEnd = DateUtility.LongToDateTime(Int64.Parse(dtClone.Rows[i]["realTimeClockDateandTime"].ToString()));

                                                dtClone.Rows[i]["IntervalStart"] = dateTimeStart.Day.ToString("d2") + "/" + dateTimeStart.Month.ToString("d2") + "/" + dateTimeStart.Year.ToString("d4") + " " + dateTimeStart.Hour.ToString("d2") + ":" + dateTimeStart.Minute.ToString("d2"); ;
                                                dtClone.Rows[i]["IntervalEnd"] = dateTimeEnd.Day.ToString("d2") + "/" + dateTimeEnd.Month.ToString("d2") + "/" + dateTimeEnd.Year.ToString("d4") + " " + dateTimeEnd.Hour.ToString("d2") + ":" + dateTimeEnd.Minute.ToString("d2");

                                                TimeSpan timeDiff = dateTimeEnd - dateTimeStart;
                                                dateTimeStart = DateTime.MinValue;
                                                dateTimeEnd = DateTime.MaxValue;

                                                if (i == dtClone.Rows.Count - 2)
                                                {

                                                    dateTimeEnd = DateUtility.LongToDateTime(Int64.Parse(dataSet.Tables[0].Rows[i + 1]["realTimeClockDateandTime"].ToString()));
                                                    dateTimeStart = dateTimeEnd.Subtract(timeDiff);

                                                    dtClone.Rows[i + 1]["IntervalStart"] = dateTimeStart.Day.ToString("d2") + "/" + dateTimeStart.Month.ToString("d2") + "/" + dateTimeStart.Year.ToString("d4") + " " + dateTimeStart.Hour.ToString("d2") + ":" + dateTimeStart.Minute.ToString("d2"); ;
                                                    dtClone.Rows[i + 1]["IntervalEnd"] = dateTimeEnd.Day.ToString("d2") + "/" + dateTimeEnd.Month.ToString("d2") + "/" + dateTimeEnd.Year.ToString("d4") + " " + dateTimeEnd.Hour.ToString("d2") + ":" + dateTimeEnd.Minute.ToString("d2");
                                                }
                                            }
                                            double energyKWh;
                                            double energyKVAh;
                                            double energyExportKWh;
                                            double energyExportKVAh;
                                            double powerFactor;
                                            bool isEnergyKWhDouble;
                                            bool isEnergyKVAhDouble;
                                            bool isEnergyExportKWhDouble;
                                            bool isEnergyExportKVAhDouble;
                                            string tempdata = String.Empty;

                                            foreach (DataRow dRow in dtClone.Rows)
                                            {
                                                energyKWh = 0;
                                                energyKVAh = 0;
                                                energyExportKWh = 0;
                                                energyExportKVAh = 0;
                                                powerFactor = 0;
                                                isEnergyKWhDouble = false;
                                                isEnergyKVAhDouble = false;
                                                isEnergyExportKWhDouble = false;
                                                isEnergyExportKVAhDouble = false;
                                                tempdata = String.Empty;
                                                foreach (DataColumn cl in dtClone.Columns)
                                                {
                                                    switch (cl.ColumnName)
                                                    {
                                                        case "MeterID":
                                                            tempdata = dRow[cl].ToString().PadLeft(8, '0');
                                                            break;
                                                        case "realTimeClockDateandTime":
                                                            tempdata = dRow[cl].ToString().PadLeft(50, '0');
                                                            break;
                                                        case "IntervalStart":
                                                        case "IntervalEnd":
                                                            tempdata = dRow[cl].ToString();
                                                            break;
                                                        case "blockEnergykWh":
                                                            tempdata = String.Format("{0:0000.00000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            isEnergyKWhDouble = double.TryParse(CommonBLL.RemoveUnit(dRow[cl].ToString()), out energyKWh);
                                                            break;
                                                        case "blockEnergykVAh":
                                                            tempdata = String.Format("{0:0000.00000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            isEnergyKVAhDouble = double.TryParse(CommonBLL.RemoveUnit(dRow[cl].ToString()), out energyKVAh);
                                                            break;
                                                        case "blockEnergykWhExport":
                                                            tempdata = String.Format("{0:0000.00000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            isEnergyExportKWhDouble = double.TryParse(CommonBLL.RemoveUnit(dRow[cl].ToString()), out energyExportKWh);
                                                            break;
                                                        case "blockEnergykVAhExport":
                                                            tempdata = String.Format("{0:0000.00000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            isEnergyExportKVAhDouble = double.TryParse(CommonBLL.RemoveUnit(dRow[cl].ToString()), out energyExportKVAh);
                                                            break;
                                                        case "bPhaseVoltage":
                                                        case "yPhaseVoltage":
                                                        case "rPhaseVoltage":
                                                        case "frequency":
                                                            tempdata = CommonBLL.RemoveUnit(dRow[cl].ToString()).PadLeft(6, '0');
                                                            break;
                                                        case "bPhaseCurrent":
                                                        case "yPhaseCurrent":
                                                        case "rPhaseCurrent":
                                                            tempdata = String.Format("{0:000.000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            break;
                                                        case "blockEnergykvarhlag":
                                                        case "blockEnergykvarhleadQ2":
                                                        case "blockEnergykvarhlagQ3":
                                                        case "blockEnergykvarhlead":
                                                            tempdata = String.Format("{0:0000.00000}", float.Parse(CommonBLL.RemoveUnit(dRow[cl].ToString())));
                                                            break;
                                                        case "netkWh":
                                                            if (isEnergyKWhDouble && isEnergyExportKWhDouble)
                                                                tempdata = String.Format("{0:0000.00000}", (energyKWh - energyExportKWh));
                                                            break;
                                                        case "netkVAh":
                                                            if (isEnergyKVAhDouble && isEnergyExportKVAhDouble)
                                                                tempdata = String.Format("{0:0000.00000}", (energyKVAh - energyExportKVAh));
                                                            break;
                                                        case "PowerFactor":
                                                            if (isEnergyKVAhDouble && isEnergyKWhDouble)
                                                            {
                                                                if (energyKVAh != 0)
                                                                {
                                                                    powerFactor = energyKWh / energyKVAh;
                                                                }
                                                            }
                                                            if (isEnergyExportKWhDouble && isEnergyExportKVAhDouble)
                                                            {
                                                                if (energyExportKVAh != 0 && energyExportKVAh > energyKVAh)
                                                                {
                                                                    powerFactor = energyExportKWh / energyExportKVAh;
                                                                }
                                                            }
                                                            tempdata = String.Format("{0:0.00000}", powerFactor);
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                    //SarkarA code change end 20180302
                                                    FormatData = FormatData + tempdata + seperater;
                                                }
                                                if (FormatData != string.Empty && FormatData != null)
                                                {
                                                    FormatData = FormatData.TrimEnd(seperater.ToCharArray());
                                                    streamWriter.WriteLine(FormatData);
                                                    FormatData = string.Empty;
                                                }
                                            }
                                            exportDataSuccess = true;
                                        }

                                    }
                                }
                            }
                            #endregion

                            //SarkarA-code-start-20171201
                            #region TorrentAgraFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.TorrentAgra)  //Torrent
                            {
                                try
                                {
                                    seperater = ";";
                                    fileName = gridRow.Cells["File Name"].Value.ToString();
                                    List<string> lstMeterDataID = textExportBLL.GetMeterDataIDListByFileName(fileName);

                                    string FormattedData = String.Empty;

                                    foreach (string meterDataID in lstMeterDataID)
                                    {
                                        dataSet = textExportBLL.GetTorrentAgraDataForTextExport(meterDataID);


                                        if (dataSet != null && dataSet.Tables[0].Rows.Count != 0)
                                        {
                                            foreach (DataRow dRow in dataSet.Tables[0].Rows)
                                            {
                                                foreach (DataColumn cl in dataSet.Tables[0].Columns)
                                                {
                                                    if (cl.ColumnName.ToLower().Contains("date"))
                                                    {
                                                        long temp = 0;
                                                        string date = "";
                                                        DateTime dateTime = DateTime.MinValue;
                                                        if (Int64.TryParse(CommonBLL.RemoveUnitIfExist(dRow[cl.ColumnName].ToString()), out temp))
                                                        {
                                                            dateTime = DateUtility.LongToDateTime(temp);
                                                            date = dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                                        }
                                                        else if (DateTime.TryParse(CommonBLL.RemoveUnitIfExist(dRow[cl.ColumnName].ToString()), out dateTime))
                                                        {
                                                            date = dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                                        }
                                                        date = String.IsNullOrEmpty(date) ? "00/00/0000 00:00" : date;
                                                        FormattedData = FormattedData + date + seperater;
                                                    }
                                                    //else if (String.IsNullOrEmpty((dRow[cl.ColumnName]).ToString()))
                                                    //{
                                                    //    FormattedData = FormattedData + String.Empty + seperater;
                                                    //}
                                                    else if (cl.ColumnName.Equals("Meter_Location") || cl.ColumnName.Equals("Consumer_HNumber"))
                                                    {
                                                        string tempString = dRow[cl.ColumnName].ToString();
                                                        tempString = tempString.Contains("--") ? "" : tempString;
                                                        FormattedData = FormattedData + tempString.PadLeft(2, '0') + seperater;
                                                    }
                                                    else if (cl.ColumnName.Equals("MeterID"))
                                                    {
                                                        string tempString = dRow[cl.ColumnName].ToString();
                                                        FormattedData = FormattedData + tempString.PadLeft(8, '0') + seperater;//FormattedData = FormattedData + tempString.PadLeft(8, '0') + seperater;
                                                    }
                                                    else if (cl.ColumnName.Equals("Consumer_Number"))
                                                    {
                                                        string tempString = dRow[cl.ColumnName].ToString();
                                                        FormattedData = FormattedData + tempString.PadLeft(9, '0') + "   " + seperater;
                                                    }
                                                    else
                                                    {
                                                        string tempString = dRow[cl.ColumnName].ToString();
                                                        FormattedData = FormattedData + tempString.PadLeft(11, '0') + seperater;
                                                    }
                                                }
                                            }
                                            if (!String.IsNullOrEmpty(FormattedData))
                                            {
                                                FormattedData = FormattedData.TrimEnd(seperater.ToCharArray());
                                                streamWriter.WriteLine(FormattedData);
                                                FormattedData = string.Empty;
                                                exportDataSuccess = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }




                            #endregion
                            //SarkarA-code-end-20171201

                            #region TataPowerAdaniFormat
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.TataPowerAdani)
                            {
                                try
                                {
                                    //seperater = ";";
                                    fileName = gridRow.Cells["File Name"].Value.ToString();
                                    List<string> lstMeterDataID = textExportBLL.GetMeterDataIDListByFileName(fileName);

                                    string FormattedData = String.Empty;

                                    foreach (string meterDataID in lstMeterDataID)
                                    {
                                        dataSet = textExportBLL.GetTataPowerAdaniDataForTextExport(meterDataID);


                                        if (dataSet != null && dataSet.Tables[0].Rows.Count != 0)
                                        {
                                            foreach (DataRow dRow in dataSet.Tables[0].Rows)
                                            {
                                                foreach (DataColumn cl in dataSet.Tables[0].Columns)
                                                {
                                                    string tempString = dRow[cl.ColumnName].ToString();
                                                    FormattedData = FormattedData + tempString + seperater;
                                                }
                                            }
                                            if (!String.IsNullOrEmpty(FormattedData))
                                            {
                                                FormattedData = FormattedData.TrimEnd(seperater.ToCharArray());
                                                streamWriter.WriteLine(FormattedData);
                                                FormattedData = string.Empty;
                                                exportDataSuccess = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }

                            #endregion

                            #region CSPDCL
                            //SarkarA-code-start-20200214
                            else if (cmbExportType.SelectedIndex == (int)ExportFileFormatCustomerType.CSPDCL)
                            {
                                try
                                {
                                    //seperater = ";";
                                    fileName = gridRow.Cells["File Name"].Value.ToString();
                                    List<string> lstMeterDataID = textExportBLL.GetMeterDataIDListByFileName(fileName);
                                    string tempString = string.Empty;
                                    string FormattedData = String.Empty;
                                   
                                    if (bWriteHeader)
                                    {
                                        string header = "KWHnormalcurrent(C1),KWHOnpeakcurrent(C2),KWHOffpeakcurrent(C3),KWHNormalPrevious,KWHOnpeakprevious,KWHOffpeakprevious,KWHCummlativecurrent,KWHCummlativeprevious,KVAHCummlativeCurrent,KVAHcummlativeprevious,LTRKVAHlagcurrent,LTRKVAHLagprevious,KVARHLEAD(I)History1,AvgPFcurrent,MDcurrent,CMD,ResetCount,MF,BillingDateH1,MeterSerialNo,BillingDateH2,KVAHNormalCurrent(C1),KVAHOnpeakcurrent(C2),KVAHOffpeakcurrent(C3),KVAHNormalPrevious(C1),KVAHOnpeakPrevious(C2),KVAHOffpeakPrevious(C3)";
                                        streamWriter.WriteLine(header);
                                        bWriteHeader = false;
                                    }
                                    foreach (string meterDataID in lstMeterDataID)
                                    {
                                        dataSet = textExportBLL.GetCSPDCLDataForTextExport(meterDataID);


                                        if (dataSet != null && dataSet.Tables[0].Rows.Count != 0)
                                        {
                                            foreach (DataRow dRow in dataSet.Tables[0].Rows)
                                            {
                                                foreach (DataColumn cl in dataSet.Tables[0].Columns)
                                                {
                                                    tempString = string.Empty;
                                                    if (cl.ColumnName.IndexOf("date", StringComparison.OrdinalIgnoreCase) > 0 && long.TryParse(dRow[cl.ColumnName].ToString(), out long date))
                                                    {
                                                        tempString = DateUtility.LongToDateTime(date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    }
                                                    else 
                                                    {
                                                        tempString = CommonBLL.RemoveUnitIfExist(dRow[cl.ColumnName].ToString());
                                                    }
                                                    FormattedData = FormattedData + tempString + seperater;
                                                }
                                            }
                                            if (!String.IsNullOrEmpty(FormattedData))
                                            {
                                                FormattedData = FormattedData.TrimEnd(seperater.ToCharArray());
                                                streamWriter.WriteLine(FormattedData);
                                                FormattedData = string.Empty;
                                                exportDataSuccess = true;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            //SarkarA-code-end-20200214
                            #endregion

                        }

                        idCount++;
                        // SarkarA code change end 20171208
                    }
                    catch (Exception ex)
                    {
                        logger.Log(LOGLEVELS.Error, "ExportDataToTextFile(string fileLocation)", ex);
                        gridRow.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                    if (noDataForExport != string.Empty)
                    {
                        MessageBox.Show(noDataForExport); ;
                    }

                    if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val) && exportDataSuccess)
                    {
                        result = "File exported successfully.";
                        gridRow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                result = "Error in Exporting Data.";
                logger.Log(LOGLEVELS.Error, "ExportDataToTextFile(string fileLocation)", ex);
            }
            finally
            {
                streamWriter.Close();
            }

            return result;

        }

        /// <summary>
        /// Padding of appropriate number of leading and trailing zeros
        /// </summary>
        /// <param name="record"></param>
        /// <param name="intZeros"></param>
        /// <param name="DecZeros"></param>
        /// <returns></returns>
        private string FormatDataWidth(string record, string intZeros, string DecZeros)
        {
            string value = string.Empty;
            string decimalValue = string.Empty;
            try
            {
                double data = Convert.ToDouble(record);
                value = Convert.ToUInt64(Convert.ToDecimal(record).TruncateToPrecision(0)).ToString(intZeros);

                if (record.ToString().Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                {
                    decimalValue = record.ToString().Split(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToCharArray())[1];
                    if (decimalValue.Length > 2)
                    {
                        decimalValue = decimalValue.Substring(0, 2);
                    }
                    else
                    {
                        decimalValue = Convert.ToDecimal(decimalValue).ToString(DecZeros);
                    }
                }
                else
                {
                    decimalValue = DecZeros;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FormatDataWidth(string record, string intZeros, string DecZeros)", ex);
            }
            return value + decimalValue;
        }

        /// <summary>
        /// Used to format data to be exported in text file.
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        private DataTable FormatData(DataTable inputTable)
        {
            foreach (DataRow record in inputTable.Rows)
            {
                foreach (DataColumn field in inputTable.Columns)
                {
                    if (field.ColumnName.ToUpper() == "RTC" || field.ColumnName.ToUpper() == "READINGDATETIME"
                                                            || field.ColumnName.ToUpper() == "BILLINGDATE")
                    {
                        record[field] = record[field].ToString().Split('*')[0];
                    }
                    else
                    {
                        record[field] = record[field].ToString().Split('*')[0];
                        if (record[field] != null)
                        {
                            switch (field.ColumnName.ToUpper())
                            {
                                case "METER SERIAL NO":
                                    record[field] = record[field].ToString();
                                    break;
                                case "NO.OF RESET":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000", "");
                                    break;
                                case "RN_VOLTAGE":
                                    record[field] = FormatDataWidth(record[field].ToString(), "0000", "00");
                                    break;
                                case "YN_VOLTAGE":
                                    record[field] = FormatDataWidth(record[field].ToString(), "0000", "00");
                                    break;
                                case "BN_VOLTAGE":
                                    record[field] = FormatDataWidth(record[field].ToString(), "0000", "00");
                                    break;
                                case "RN_CURRENT":
                                    record[field] = FormatDataWidth(record[field].ToString(), "00000000", "00");
                                    break;
                                case "YN_CURRENT":
                                    record[field] = FormatDataWidth(record[field].ToString(), "00000000", "00");
                                    break;
                                case "BN_CURRENT":
                                    record[field] = FormatDataWidth(record[field].ToString(), "00000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKWHTZ1":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKWHTZ2":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKWHTZ3":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKWHTZ4":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKVAHTZ1":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKVAHTZ2":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKVAHTZ3":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVEENERGYKVAHTZ4":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKWTZ1":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKWTZ2":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKWTZ3":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKWTZ4":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKVATZ1":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKVATZ2":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKVATZ3":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "MDKVATZ4":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVE KW":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVE KVA":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVE KWH":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVE KVAH":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                                case "CUMULATIVE RKVAH":
                                    record[field] = FormatDataWidth(record[field].ToString(), "000000000000", "00");
                                    break;
                            }
                        }
                    }
                }
            }

            return inputTable;
        }

        /// <summary>
        /// To write file contents at a user specific location
        /// </summary>
        /// <returns></returns>
        private void FindAndWriteContents()
        {

            string fileLocation = string.Empty;
            string result = string.Empty;

            SaveFileDialog DialogSave = new SaveFileDialog();
            DialogSave.DefaultExt = "TXT";
            DialogSave.Filter = "Text file (*.TXT)|*.TXT|CSV file (*.CSV)|*.CSV";
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".TXT";
            DialogSave.Title = "Where do you want to save the file?";
            DialogSave.RestoreDirectory = true;


            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                this.StatusMessage = "Exporting the file.";
                this.Cursor = Cursors.WaitCursor;

                fileLocation = DialogSave.FileName.Trim();
                result = ExportDataToTextFile(fileLocation);
                this.Cursor = Cursors.Default;
            }

            this.StatusMessage = result;

        }

        /// <summary>
        /// Manipulates the checkboxes of the grid on "Select All" checkbox checked changed.
        /// </summary>
        /// <param name="status"></param>
        private void CheckedAll(bool status)
        {
            if (status)
            {
                for (int i = 0; i < grdFileList.Rows.Count; i++)
                {
                    grdFileList.Rows[i].Cells["Include"].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < grdFileList.Rows.Count; i++)
                {
                    grdFileList.Rows[i].Cells["Include"].Value = false;
                }
            }
        }

        #endregion

        private void cmbExportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdFileList.Columns.Clear();

            if (cmbExportType.Text.Contains("Puducherry Format"))
            {
                LoadPEDMeterDetailList();
            }
            else
            {
                LoadOtherMeterDetailList();
            }
            // SarkarA code change start 20171206//Reliance Mumbai Text Export
            if (cmbExportType.SelectedIndex == 4)
            {
                cmbRelianceMumbaiDataType.Visible = true;
                cmbRelianceMumbaiDataType.SelectedIndex = 0;

                rmcb.Visible = true;
            }
            else
            {
                rmcb.Visible = false;
                cmbRelianceMumbaiDataType.Visible = false;
            }
            // SarkarA code change end 20171208
        }

        // SB Change Start 20170914
        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            grpFilter.Enabled = chkFilter.Checked;

            if (!chkFilter.Checked)
            {
                grdFileList.Columns.Clear();

                if (cmbExportType.Text.Contains("Puducherry Format"))
                {
                    LoadPEDMeterDetailList();
                }
                else
                {
                    LoadOtherMeterDetailList();
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            grdFileList.Columns.Clear();

            if (cmbExportType.Text.Contains("Puducherry Format"))
            {
                LoadPEDMeterDetailList();
            }
            else
            {
                LoadOtherMeterDetailList();
            }
        }
        // SB Change End 20170914

        // SarkarA code change start 20171206//Reliance Mumbai Text Export
        private void cmbRelianceMumbaiDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRelianceMumbaiDataType.Text.Equals("Billing"))
            {
                rmcb.Visible = true;
            }
            else
            {
                rmcb.Visible = false;
            }
        }

        private void rmcb_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bVisible = rmcb.SelectedIndex != (int)RelianceBilling.NoSolar;

            cmbBillHistNo.Visible = bVisible;
            lblBillHist.Visible = bVisible;
            
        }
        // SarkarA code change start 20171206
    }
}
