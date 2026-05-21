using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using System.Threading;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class StandardExport : MdiChildForm
    {
        private string fileName = string.Empty;
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL();
        private ApplicationType apptype = ConfigInfo.GetApplicationType();
        private const string FLAG = "False";
        private const string EXPORTERROR = "Export Error";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(StandardExport).ToString());
        public StandardExport()
        {
            InitializeComponent();
        }

        private void StandardExport_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Text = "Standard Export";
            DataSet mainListDataSet = meterDataBLL.FileExportListDataSet(false);
            if (mainListDataSet == null)
                return;
            if (mainListDataSet.Tables.Count == 0)
                return;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            bool flag = false;
            foreach (DataGridViewRow row in grdFileList.Rows)
            {
                if (row.Cells[0].Value != null)
                    flag = (bool)row.Cells[0].Value;
                if (flag)
                    break;
            }
            if (!flag)
            {
                this.StatusMessage = "Please select file";
                return;
            }
            if (CheckIECDLMSFiles())
            {
                MessageBox.Show("Please Select either .2NG or .CAB or .SLG files only to export.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); // Story - 427028 - Only one type of file should be allowed
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            bool IsSaved = false;
            
            try
            {
                IsSaved = FindAndWriteContents();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnSave_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            
            if (IsSaved)
                this.StatusMessage = "File Exported Successfully.";
            else
                this.StatusMessage = "";
            Application.DoEvents();
        }

        private bool FindAndWriteContents()
        {
            string fileLocation = string.Empty;
            SaveFileDialog DialogSave = new SaveFileDialog();
            DialogSave.DefaultExt = "ZIP";
            DialogSave.Filter = "Export file (*.ZIP)|*.ZIP";
            DialogSave.AddExtension = true;
            DialogSave.RestoreDirectory = true;
            DialogSave.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".ZIP";
            DialogSave.Title = "Where do you want to save the file?";
            DialogSave.RestoreDirectory = true;
            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                if (DialogSave.FileName == "")
                    return false;
                else
                {

                    this.StatusMessage = "Exporting the file.";
                    Application.DoEvents();
                    fileLocation = DialogSave.FileName.Trim();
                    return WriteContents(fileLocation);
                }
            }
            else
                return false;
        }

        private bool CheckDuplicate(List<MeterDataEntity> mdata, FileUploadMasterEntity entity)
        {
            if (entity == null)
                return false;
            if (mdata.Count == 0)
                return false;
            MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;
            int counter = 0;
            try
            {
                foreach (MeterDataEntity mentity in mdata)
                {
                    if ((mentity.MeterID == meterDataEntity.MeterID) && (mentity.ReadingDateTime == meterDataEntity.ReadingDateTime))
                        counter++;
                }
                if (counter == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckDuplicate(List<MeterDataEntity> mdata, FileUploadMasterEntity entity)", ex);
                throw ex;
            }

        }

        private bool WriteContents(string fileLocation)
        {
            int cols = 0;
            List<MeterDataEntity> mdata = new List<MeterDataEntity>();
            //StreamWriter file = new StreamWriter(fileLocation);
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            CreateZipFile(fileLocation);
            // Added try-catch block.
            try
            {
                foreach (DataGridViewRow row in grdFileList.Rows)
                {
                    string val = Convert.ToString(grdFileList.Rows[cols].Cells["Include"].Value);
                    cols++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    // Added to solve bug 94900
                    if (!Convert.ToBoolean(val))
                        continue;
                    fileName = grdFileList.Rows[cols - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                    FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                    MeterDataEntity meterDataEntity = new MeterDataBLL().GetDetailDataUploadId(entity.FileUpload_ID) as MeterDataEntity;
                    mdata.Add(meterDataEntity);
                }

                //StringBuilder strBuild = new StringBuilder();
                cols = 0;
                foreach (DataGridViewRow row in grdFileList.Rows)
                {
                    string val = Convert.ToString(grdFileList.Rows[cols].Cells["Include"].Value);
                    cols++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (!Convert.ToBoolean(val))
                        continue;
                    fileName = grdFileList.Rows[cols - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                    FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                    if (CheckDuplicate(mdata, entity))
                    {
                        byte[] CABFile = entity.FileContent;
                        string fileContent = Encoding.ASCII.GetString(CABFile);

                        if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            System.IO.File.WriteAllText(tempPath + fileName, fileContent);
                        }
                        else
                        {
                            string calculatedBCC = CalculateFileBcc(fileContent);  //ReadoutCommon.
                            string bcc = Convert.ToChar(calculatedBCC).ToString();
                            System.IO.File.WriteAllText(tempPath + fileName, ConfigInfo.EncryptFile(fileContent + bcc));
                        }
                        Shell32.Shell Shell = new Shell32.Shell();

                        Shell.NameSpace(fileLocation).CopyHere(tempPath + fileName, 0);
                        Thread.Sleep(1000);

                        if (File.Exists(tempPath + fileName))
                            System.IO.File.Delete(tempPath + fileName);

                        //strBuild.Append(fileContent);
                    }
                    else
                    {
                        MessageBox.Show("Duplicate file Exist (Same Meter id and Reading date).", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                //if (strBuild.ToString().Length == 0)
                //{
                //    this.StatusMessage = "No data available in file.";
                //    return false;
                //}

                //file.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(EXPORTERROR, BCSConstants.BCS);
                //file.Close();
                System.IO.File.Delete(fileLocation);
                logger.Log(LOGLEVELS.Error, "WriteContents(string fileLocation)", ex);
                return false;
            }
            return true;

        }

        private void CreateZipFile(string fileLocation)
        {
            //Create the header of the Zip File 
            System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
            string sHeader = "PK" + (char)5 + (char)6;
            sHeader = sHeader.PadRight(22, (char)0);
            //Convert to byte array
            byte[] baHeader = System.Text.Encoding.ASCII.GetBytes(sHeader);

            //Save File - Make sure your file ends with .zip!
            FileStream fs = File.Create(fileLocation);
            fs.Write(baHeader, 0, baHeader.Length);
            fs.Flush();
            fs.Close();
            fs = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckIECDLMSFiles()
        {
            string selectedFileName = string.Empty;
            bool isBothSelected = false;
            bool isDLMSFile = false;
            bool isIECFile = false;
            bool isIECSPFile = false; // Story - 427028 - Only one type of file should be allowed
            foreach (DataGridViewRow row in grdFileList.Rows)
            {
                string val = Convert.ToString(grdFileList.Rows[grdFileList.Rows.IndexOf(row)].Cells["Include"].Value);
                if (!string.IsNullOrEmpty(val) && Convert.ToBoolean(val))
                {
                    selectedFileName = grdFileList.Rows[grdFileList.Rows.IndexOf(row)].Cells["File Name"].Value.ToString();
                    if (selectedFileName.ToUpper().Contains(".2NG"))
                    {
                        isDLMSFile = true;
                    }
                    else if (selectedFileName.ToUpper().Contains(".SLG"))
                    {
                        isIECSPFile = true;
                    }
                    else
                    {
                        isIECFile = true;
                    }
                }
                if ((isDLMSFile && isIECFile && isIECSPFile) || (isDLMSFile &&isIECFile) || (isIECFile && isIECSPFile) || (isIECSPFile && isDLMSFile))
                {
                    isBothSelected = true;
                    break;
                }
            }
            return isBothSelected;
        }

        public static string CalculateFileBcc(string data)
        {
            long countbyt = 0;
            long bcc = 0;
            string checkSum = string.Empty;
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(data);
                foreach (byte b in bytes)
                {
                    if (countbyt <= bytes.Length) bcc = bcc ^ b;
                    countbyt++;
                }
                checkSum = Convert.ToChar(bcc).ToString();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                checkSum = string.Empty;
                logger.Log(LOGLEVELS.Error, "CalculateFileBcc(string data)", ex);
            }
            return checkSum;
        }

        private void StandardExport_Deactivate(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckedAll(chkSelectAll.Checked);
        }

        private void CheckedAll(bool status)
        {
            if (status == true)
            {
                for (int i = 0; i < grdFileList.Rows.Count; i++)
                {
                    grdFileList.Rows[i].Cells[0].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < grdFileList.Rows.Count; i++)
                {
                    grdFileList.Rows[i].Cells[0].Value = false;
                }
            }
        }

        private void grdFileList_CheckBoxChange(object sender, DataGridViewCellEventArgs e)
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
                            if (cell.EditedFormattedValue == null || (bool)cell.EditedFormattedValue == false)
                            { IfAllRowsSelected = false; break; }
                        }
                        chkSelectAll.Checked = IfAllRowsSelected;
                    }
                    chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
                }
            }
        }
    }
}