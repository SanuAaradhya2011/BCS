using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.UI;
using CAB.Entity;
using CAB.BLL;
using System.IO;

namespace CABApplication
{
    public partial class FileImport : MdiChildForm
    {  
        public FileImport()
        {
            InitializeComponent();
        }

        private void FileImport_Load(object sender, EventArgs e)
        {
            OnLoadCall();            
        }
        /// <summary>
        /// Create the list of the pending files to be imported.
        /// </summary>
        private void OnLoadCall()
        {
            DataTable table = new DataTable();
            try
            {
                table = ReadPendingFiles();
                grdFileList.DataSource = table;
                foreach (DataGridViewColumn column in grdFileList.Columns)
                {
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    column.ReadOnly = true;
                    column.Width = 200;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Read pending files to be imported from the CSV file.
        /// </summary>
        /// <returns></returns>
        private DataTable ReadPendingFiles()
        {
            DataTable table = new DataTable();
            DataRow nrow;
            table.Columns.Add(new DataColumn("File Name"));         
            string filName = Application.StartupPath + "\\" + "Log" + "\\" + "FileStatus.csv";
            if (File.Exists(filName))
            {
            StreamReader sw = new StreamReader(filName); 
            try
            {
                CsvFileReader csvFileReader = new CsvFileReader(sw);
                CsvRow row = new CsvRow();
                string statusData = string.Empty;
                while (csvFileReader.ReadRow(row))
                {
                    nrow = table.NewRow();
                    if (row[3].Equals("2") || row[3].Equals("3") || row[3].Equals("4"))
                    {
                        nrow["File Name"] = row[2];                      
                    }
                    else
                    {
                        continue;
                    }

                    table.Rows.Add(nrow);
                }
                sw.Close(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            }
            return table;
        }
       
        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportFiles();
        }
        /// <summary>
        /// Import pending files into db.
        /// </summary>
        private void ImportFiles()
        {
            this.Cursor = Cursors.WaitCursor;
            bool IsUploaded = false;
            this.StatusMessage = "Importing the file.";
            Application.DoEvents();
            UploadFile uploadFile = new UploadFile("BCS");

            try
            {
                foreach (DataGridViewRow drow in grdFileList.Rows)
                {

                    
                    string fileName = Application.StartupPath + "\\" + "CAB Readout" + "\\" + drow.Cells["File Name"].Value.ToString();

                    this.StatusMessage = String.Format("Importing file {0}", Path.GetFileName(fileName));
                    Application.DoEvents();

                    string fileContent = uploadFile.GetContent(fileName);
                    FileUploadMasterEntity fileEntity = new FileUploadMasterBLL().ValidateFile(Path.GetFileName(fileName)) as FileUploadMasterEntity;

                    IsUploaded = uploadFile.Upload(fileName, fileContent, true);
                    this.ListRefresh = true;
                    if (IsUploaded)
                    {

                        this.StatusMessage = "File Imported successfully.";
                    }
                    else
                    {
                        this.StatusMessage = string.Format("Unable to import file {0}.", Path.GetFileName(fileName));
                        Application.DoEvents();
                    }
                    uploadFile.DeleteFile();
                    Application.DoEvents();
                }
                this.Cursor = Cursors.Default;
               
                OnLoadCall();

                MainForm mainForm = (MainForm)this.ParentForm;
                if (mainForm != null)
                {
                    if (CsvFileReader.ReadPendingFiles() != string.Empty)
                    {
                        mainForm.SetPendingFiles = CsvFileReader.ReadPendingFiles();
                        mainForm.SetPendingFilesVisible = true;
                    }
                    else
                    {
                        mainForm.SetPendingFilesVisible = false;
                        this.Close(); 
                    }
                    mainForm.Show();
                }
            }
            catch (Exception ex)
            {
                this.StatusMessage = "File Corrupted.";
                this.Cursor = Cursors.Default;   
            }
        }
    }
}
