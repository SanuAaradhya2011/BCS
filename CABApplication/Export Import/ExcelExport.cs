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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Drawing;
using CABApplication.Reports.DLMS_Detailed_Reports;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.UI
{
    public partial class ExcelExport : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ApplicationType apptype = ConfigInfo.GetApplicationType();
        private TextExportBLL textExportBLL = new TextExportBLL();
        string meterDataID = string.Empty;
        TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
        DLMS650TamperMasterBLL tamperBLL = new DLMS650TamperMasterBLL();
        DLMS650BillingBLL dLMS650BillingBLL = new DLMS650BillingBLL();
        public bool IsAutomationReport = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ExcelExport).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ExcelExport()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void ExcelExport_Load(object sender, EventArgs e)
        {

            if (this.IsAutomationReport)
            {
                ((CAB.UI.ExcelExport)(sender)).meterWise.Hide();
                ((CAB.UI.ExcelExport)(sender)).fileWise.Checked = true;
                label2.Hide();
                return;
            }

            DataSet mainListDataSet = null;
            if (((CAB.UI.ExcelExport)(sender)).meterWise.Checked)
            {
                mainListDataSet = meterDataBLL.ExcelExportListDataSet();
            }
            else
            {
                mainListDataSet = meterDataBLL.FileExportListDataSet(true);
            }
            if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
            {
                grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                foreach (DataGridViewColumn column in grdFileList.Columns)
                {
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                }
                SetTopGridEqualWidth(mainListDataSet);
                if (((CAB.UI.ExcelExport)(sender)).meterWise.Checked)
                {
                    DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                    checkBoxCol.Name = "Include";
                    grdFileList.Columns.Add(checkBoxCol);
                    grdFileList.Columns[2].HeaderText = "Include";
                    grdFileList.Columns[2].ReadOnly = false;
                }
                else
                {

                    DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                    checkBoxCol.Name = "Include";
                    grdFileList.Columns.Add(checkBoxCol);
                    grdFileList.Columns[3].HeaderText = "Include";
                    grdFileList.Columns[3].ReadOnly = false;
                }
            }
            else
            {
                lblErrorInfo.Visible = true;
                btnExport.Enabled = false;
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
                    if (row.Cells["Include"].Value != null)
                    {
                        flag = (bool)row.Cells["Include"].Value;
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
                            DataGridViewCheckBoxCell cell = grdFileList[0, i] as DataGridViewCheckBoxCell;
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
        /// Used to export All available files instant and billing  data into excel file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        private string ExportDataToExcelFile(string fileLocation)
        {
            string fileName = string.Empty;
            string meterID = string.Empty;
            string result = string.Empty;
            bool exportDataSuccess = false;
            MeterDataEntity meterDataEntity = new MeterDataEntity();
            StreamWriter streamWriter = new StreamWriter(fileLocation);
            DataSet dataForExport = new DataSet();
            try
            {
                foreach (DataGridViewRow gridRow in grdFileList.Rows)
                {
                    string val = Convert.ToString(gridRow.Cells["Include"].Value);
                    if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val))
                    {
                        if (meterWise.Checked)
                        {
                            meterID = gridRow.Cells["MeterID"].Value.ToString();
                            DataTable dtMeterDataIds = meterDataBLL.GetMeterDataIDFromMeterID(meterID).Tables[0];

                            foreach (DataRow drMeterDataId in dtMeterDataIds.Rows)
                            {
                                DataTable dt = new ExcelExportBLL().GetDataForExcelExport(Convert.ToInt64(drMeterDataId["MeterData_Id"])).Tables[0];

                                //copy  is used to avoid exception 
                                //datatable already belongs to a dataset
                                DataTable dtCopy = dt.Copy();
                                dtCopy.TableName = drMeterDataId["MeterData_Id"].ToString();
                                dataForExport.Tables.Add(dtCopy);
                            }

                        }
                        else
                        {
                            fileName = gridRow.Cells["File Name"].Value.ToString();
                            FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                            FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                            meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                            DataTable dt = new ExcelExportBLL().GetDataForExcelExport(meterDataEntity.MeterData_ID).Tables[0];

                            //copy  is used to avoid exception 
                            //datatable already belongs to a dataset
                            DataTable dtCopy = dt.Copy();
                            dtCopy.TableName = entity.FileUpload_ID.ToString();

                            dataForExport.Tables.Add(dtCopy);
                        }
                    }
                }
                if (dataForExport != null && dataForExport.Tables != null && dataForExport.Tables.Count > 0)
                {
                    try
                    {
                        int count = 0;
                        foreach (DataTable dt in dataForExport.Tables)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataTable dataInTable = new DataTable();
                                dataInTable = FormatData(dt);

                                if (count == 0)
                                {
                                    foreach (DataColumn column in dataInTable.Columns)
                                    {
                                        streamWriter.Write(column.ColumnName.ToUpper() + "\t");
                                    }
                                    count++;
                                }
                                streamWriter.WriteLine();


                                //write rows to excel file
                                foreach (DataRow row in dataInTable.Rows)
                                {
                                    //Write columns values of  selected row
                                    foreach (DataColumn column in dataInTable.Columns)
                                    {
                                        if (row[column] != null)
                                        {
                                            if (column.ColumnName.ToUpper() == "RTC" || column.ColumnName.ToUpper() == "METER READ DATE & TIME"
                                                                       || column.ColumnName.ToUpper() == "BILLINGDATE")
                                            {
                                                streamWriter.Write(Convert.ToString(DateUtility.LongToDateTime(Int64.Parse(row[column].ToString()))) + "\t");
                                            }
                                            else
                                            {
                                                if (Convert.ToString(row[column]) == string.Empty)
                                                    streamWriter.Write(" -- " + "\t");
                                                else
                                                    streamWriter.Write(Convert.ToString(row[column]) + "\t");
                                            }
                                        }
                                        else
                                        {
                                            streamWriter.Write("\t");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        result = "Error in Exporting Data.";
                        logger.Log(LOGLEVELS.Error, "ExportDataToExcelFile(string fileLocation)", ex);
                    }
                    finally
                    {
                        streamWriter.Close();
                    }
                    exportDataSuccess = true;
                }
                else
                {
                    result = "Data Not Available for Export.";
                }
                if (exportDataSuccess)
                {
                    result = "File exported successfully.";
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ExportDataToExcelFile(string fileLocation)", ex);
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
                                case "METER SERIAL NO.":
                                    record[field] = record[field].ToString();
                                    break;
                                case "METERTYPE":
                                    record[field] = record[field].ToString();
                                    break;
                                case "METER READ DATE & TIME":
                                    record[field] = record[field].ToString();
                                    break;
                                case "ABS ACTIVE ENERGY (KWH)(CURRENT)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "ABS ACTIVE (MD) KW":
                                    record[field] = record[field].ToString();
                                    break;
                                case "ABS APPARENT ENERGY (KVAH)( CURRENT )":
                                    record[field] = record[field].ToString();
                                    break;
                                case "ABS APPARENT (MD) KVA":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD1)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD2)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD3)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD4)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD5)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD6)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD7)":
                                    record[field] = record[field].ToString();
                                    break;
                                case "KVAH(TOD8)":
                                    record[field] = record[field].ToString();
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
            DialogSave.DefaultExt = "xls";
            DialogSave.Filter = "Excel file (*.xls)|*.xls";
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".xls";
            DialogSave.Title = "Where do you want to save the file?";
            DialogSave.RestoreDirectory = true;


            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                this.StatusMessage = "Exporting the file.";
                this.Cursor = Cursors.WaitCursor;

                fileLocation = DialogSave.FileName.Trim();


                if (this.IsAutomationReport)
                {
                    result = ExportDataToAutomationFile(fileLocation);
                }
                else
                {
                    result = ExportDataToExcelFile(fileLocation);
                }


                this.Cursor = Cursors.Default;
            }

            this.StatusMessage = result;

        }

        private string ExportDataToAutomationFile(string fileLocation)
        {
            Excel.Application oXL;
            oXL = new Excel.Application();
            string result = string.Empty;
            string tamperColumns = string.Empty;
            DataSet dataSet = new DataSet();
            DataSet dsBilling = new DataSet();
            MeterDataEntity meterDataEntity = new MeterDataEntity();
            string fileName = string.Empty;

            try
            {


                //Fetch data and Fill excel file

                DataSet tamperData = new DataSet();
                foreach (DataGridViewRow gridRow in grdFileList.Rows)
                {
                    string val = Convert.ToString(gridRow.Cells["Include"].Value);
                    if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val))
                    {

                        #region Get Tamper data
                        fileName = gridRow.Cells["File Name"].Value.ToString();
                        FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                        FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                        meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;

                        tamperData = tamperParameterBLL.GetColumnNames(meterDataEntity.MeterData_ID);

                        //loading Abbreviations
                        CABApplication.Reports.Forms.TamperReport.GetTamperAbbreviation();
                        DataTable dtCopy = new DataTable();
                        if (tamperData != null && tamperData.Tables.Count > 0 && tamperData.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = tamperBLL.DetailDataForAutomationExport(meterDataEntity.MeterData_ID, tamperData.Tables[0].Rows[0][0].ToString(), CABApplication.Reports.Forms.TamperReport.dictTamperCodeAndAbbreviation);

                            //copy  is used to avoid exception 
                            //datatable already belongs to a dataset
                            dtCopy = dt.Copy();

                            dtCopy.TableName = fileName;

                            dataSet.Tables.Add(dtCopy);
                        }

                        #endregion

                        #region Get Billing data


                        string billingParameters = dLMS650BillingBLL.GetColumnNames(meterDataEntity.MeterData_ID);


                        DataTable dtBilling = dLMS650BillingBLL.GetDataByMeterID(meterDataEntity.MeterData_ID, billingParameters).Tables[0];

                        dtCopy = dtBilling.Copy();

                        dtCopy.TableName = fileName;

                        dsBilling.Tables.Add(dtCopy);

                        #endregion


                    }
                    else
                        continue;
                }

                PrePareExcel(dataSet, dsBilling, oXL, fileLocation);

            }
            catch (CABException ex)    //Exception log for catch block
            {
                result = "Error in Exporting Data.";
                logger.Log(LOGLEVELS.Error, "ExportDataToAutomationFile(string fileLocation)", ex);
                return result;
            }
            result = "File exported successfully.";
            return result;

        }

        private Dictionary<int, string> PrepareExcelCol()
        {
            Dictionary<int, string> dictExcelCol = new Dictionary<int, string>();

            dictExcelCol.Add(1, "A");
            dictExcelCol.Add(2, "B");
            dictExcelCol.Add(3, "C");
            dictExcelCol.Add(4, "D");
            dictExcelCol.Add(5, "E");
            dictExcelCol.Add(6, "F");
            dictExcelCol.Add(7, "G");
            dictExcelCol.Add(8, "H");
            dictExcelCol.Add(9, "I");
            dictExcelCol.Add(10, "J");
            dictExcelCol.Add(11, "K");
            dictExcelCol.Add(12, "L");
            dictExcelCol.Add(13, "M");
            dictExcelCol.Add(14, "N");
            dictExcelCol.Add(15, "O");
            dictExcelCol.Add(16, "P");
            dictExcelCol.Add(17, "Q");
            dictExcelCol.Add(18, "R");
            dictExcelCol.Add(19, "S");
            dictExcelCol.Add(20, "T");
            dictExcelCol.Add(21, "U");
            dictExcelCol.Add(22, "V");
            dictExcelCol.Add(23, "W");
            dictExcelCol.Add(24, "X");
            dictExcelCol.Add(25, "Y");
            dictExcelCol.Add(26, "Z");
            dictExcelCol.Add(27, "AA");
            dictExcelCol.Add(28, "AB");
            dictExcelCol.Add(29, "AC");
            dictExcelCol.Add(30, "AD");
            dictExcelCol.Add(31, "AE");
            dictExcelCol.Add(32, "AF");
            dictExcelCol.Add(33, "AG");
            dictExcelCol.Add(34, "AH");
            dictExcelCol.Add(35, "AI");
            dictExcelCol.Add(36, "AJ");
            dictExcelCol.Add(37, "AK");
            dictExcelCol.Add(38, "AL");
            dictExcelCol.Add(39, "AM");
            dictExcelCol.Add(40, "AN");
            dictExcelCol.Add(41, "AO");
            dictExcelCol.Add(42, "AP");
            dictExcelCol.Add(43, "AQ");
            dictExcelCol.Add(44, "AR");
            dictExcelCol.Add(45, "AS");
            dictExcelCol.Add(46, "AT");
            dictExcelCol.Add(47, "AU");
            dictExcelCol.Add(48, "AV");
            dictExcelCol.Add(49, "AW");
            dictExcelCol.Add(50, "AX");
            dictExcelCol.Add(51, "AY");
            dictExcelCol.Add(52, "AZ");
            dictExcelCol.Add(53, "BA");
            dictExcelCol.Add(54, "BB");
            dictExcelCol.Add(55, "BC");
            dictExcelCol.Add(56, "BD");
            dictExcelCol.Add(57, "BE");
            dictExcelCol.Add(58, "BF");
            dictExcelCol.Add(59, "BG");
            dictExcelCol.Add(60, "BH");
            dictExcelCol.Add(61, "BI");
            dictExcelCol.Add(62, "BJ");
            dictExcelCol.Add(63, "BK");
            dictExcelCol.Add(64, "BL");
            dictExcelCol.Add(65, "BM");
            dictExcelCol.Add(66, "BN");
            dictExcelCol.Add(67, "BO");
            dictExcelCol.Add(68, "BP");
            dictExcelCol.Add(69, "BQ");
            dictExcelCol.Add(70, "BR");
            dictExcelCol.Add(71, "BS");
            dictExcelCol.Add(72, "BT");
            dictExcelCol.Add(73, "BU");
            dictExcelCol.Add(74, "BV");
            dictExcelCol.Add(75, "BW");
            return dictExcelCol;


        }

        private void PrePareExcel(DataSet dataSet, DataSet dsBilling, Microsoft.Office.Interop.Excel.Application oXL, string fileLocation)
        {

            Excel._Workbook oWB = oXL.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);

            oXL.Visible = false;
            int i = 1;
            Dictionary<int, string> dictExcelCol = PrepareExcelCol();

            #region prepare excel for Tamper
            foreach (DataTable dt in dataSet.Tables)
            {

                Excel._Worksheet oSheet;
               // oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);


                //oSheet.Name = dt.TableName;
               // CreateFileTemplate(oXL, oSheet);

                int countrow = 2;
                int countcol = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                   // FillSheet(oSheet, dictExcelCol[countcol], countrow, Color.Black, "Arial", 10, dc.ColumnName);
                    countcol++;
                }
                countrow++;
                foreach (DataRow dr in dt.Rows)
                {
                    countcol = 1;
                    foreach (DataColumn dc in dt.Columns)
                    {

                       // FillSheet(oSheet, dictExcelCol[countcol], countrow, Color.Black, "Arial", 10, Convert.ToString(dr[dc.ColumnName]));
                        countcol++;
                    }
                    countrow++;
                }
                i++;
            }
            #endregion

            #region Prepare excel for Billing
            foreach (DataTable dt in dsBilling.Tables)
            {

                Excel._Worksheet oSheet;
                try
                {
                    //oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets[dt.TableName];
                }
                catch (CABException ex)    //Exception log for catch block
                {
                   // oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(Type.Missing, Type.Missing, 1, Type.Missing);
                   // oSheet.Name = dt.TableName;
                    logger.Log(LOGLEVELS.Error, "PrePareExcel(DataSet dataSet, DataSet dsBilling, Microsoft.Office.Interop.Excel.Application oXL, string fileLocation)", ex);
                }
                //oSheet.Select(Type.Missing);
                int countrow;//= oSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row + 2;

                int countcol = 2;
                int historyCount = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    //FillSheet(oSheet, dictExcelCol[countcol], countrow, Color.Black, "Arial", 10, dc.ColumnName);
                    countcol++;
                }
               // countrow++;
                foreach (DataRow dr in dt.Rows)
                {
                    countcol = 2;

                    //Fill history rows
                    //if (historyCount == 0)
                       // FillSheet(oSheet, dictExcelCol[1], countrow, Color.Black, "Arial", 10, "Current");
                   // else
                       // FillSheet(oSheet, dictExcelCol[1], countrow, Color.Black, "Arial", 10, "History " + historyCount.ToString());


                    foreach (DataColumn dc in dt.Columns)
                    {
                      //  FillSheet(oSheet, dictExcelCol[countcol], countrow, Color.Black, "Arial", 10, Convert.ToString(dr[dc.ColumnName]));
                        countcol++;
                    }
                  //  countrow++;
                    historyCount++;
                }
                i++;
            }
            #endregion

            oXL.UserControl = true;
            oWB.SaveCopyAs(fileLocation);
        }

        private void FillSheet(Microsoft.Office.Interop.Excel._Worksheet oSheet, string col, int row, Color color, string FontStyle, int FontSize, string value)
        {

            if (string.IsNullOrEmpty(value))
                value = "--";
            else
            {
                if (value.Contains("*"))
                    value = value.Split('*')[0];
            }
            string cell = col + Convert.ToString(row);

            oSheet.get_Range(cell, cell).NumberFormat = "@";
            oSheet.get_Range(cell, cell).Value2 = value;
            oSheet.get_Range(cell, cell).Font.Color = System.Drawing.ColorTranslator.ToOle(color);
            oSheet.get_Range(cell, cell).Font.FontStyle = FontStyle;
            oSheet.get_Range(cell, cell).Font.Size = FontSize;

            //for text auto fit in cells
            oSheet.get_Range(cell, cell).EntireColumn.AutoFit();
            oSheet.get_Range(cell, cell).EntireRow.AutoFit();
            oSheet.get_Range(cell, cell).Cells.HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

        }

        private void CreateFileTemplate(Microsoft.Office.Interop.Excel.Application oXL, Microsoft.Office.Interop.Excel._Worksheet oSheet)
        {
            oSheet.get_Range("A1", "A1").Value2 = "Detailed Tamper Report";
            oSheet.get_Range("A1", "A1").Font.Size = 10;
            oSheet.get_Range("A1", "A1").Font.FontStyle = "Arial";
            oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();
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

        private void meterWise_CheckedChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.RadioButton)(sender)).Checked)
            {
                grdFileList.DataSource = null;
                if (grdFileList.Columns.Count > 0)
                    grdFileList.Columns.Remove("Include");
                grdFileList.Refresh();
                chkSelectAll.Checked = false;
                DataSet mainListDataSet = meterDataBLL.ExcelExportListDataSet();
                if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
                {
                    grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                    foreach (DataGridViewColumn column in grdFileList.Columns)
                    {
                        column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        column.ReadOnly = false;
                    }
                    SetTopGridEqualWidth(mainListDataSet);

                    DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
                    checkBoxCol.Name = "Include";
                    grdFileList.Columns.Add(checkBoxCol);
                    grdFileList.Columns[2].HeaderText = "Include";
                    grdFileList.Columns[2].ReadOnly = false;
                }
                else
                {
                    lblErrorInfo.Visible = true;
                    btnExport.Enabled = false;
                    chkSelectAll.Enabled = false;
                    return;
                }
            }
        }

        private void fileWise_CheckedChanged(object sender, EventArgs e)
        {
            if (((System.Windows.Forms.RadioButton)(sender)).Checked)
            {
                grdFileList.DataSource = null;
                if (grdFileList.Columns.Count > 0)
                    grdFileList.Columns.Remove("Include");
                grdFileList.Refresh();
                chkSelectAll.Checked = false;

                DataSet mainListDataSet = meterDataBLL.FileExportListDataSet(true);
                if (mainListDataSet != null && mainListDataSet.Tables.Count > 0)
                {
                    grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;
                    foreach (DataGridViewColumn column in grdFileList.Columns)
                    {
                        column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        column.ReadOnly = false;
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
                    btnExport.Enabled = false;
                    chkSelectAll.Enabled = false;
                    return;
                }
            }
        }

    }
}
