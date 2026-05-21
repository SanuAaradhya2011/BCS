using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Entity;
using CAB.BLL;
using System.IO;

namespace CAB.UI
{
    public partial class StandardImport : MdiChildForm
    {
        private OpenFileDialog openFile = null;
        public StandardImport()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
              openFile = new OpenFileDialog();
            openFile.DefaultExt = "EXP"; 
            openFile.Filter = "Export File(*.EXP)|*.EXP";
            txtBoxFileName.Text = ""; 
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
                txtBoxFileName.Text = openFile.FileName;
            else
                txtBoxFileName.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtBoxFileName.Text.Trim() == "")
            {
                this.StatusMessage="Please select an export file.";
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            bool IsUploaded = false;
            this.StatusMessage = "Importing the file.";
            Application.DoEvents();
            UploadFile uploadFile = new UploadFile("BCS");
            foreach (string fileName in openFile.FileNames)
            {
                string content = uploadFile.Get2NGFileContent(fileName);
                if (content.Contains("1600000F0000FF"))
                {
                    IsUploaded = uploadFile.Upload2NGFile(fileName);
                }
                else
                {
                    string fileContent = uploadFile.GetContent(fileName);
                    FileUploadMasterEntity fileEntity = new FileUploadMasterBLL().ValidateFile(Path.GetFileName(fileName)) as FileUploadMasterEntity;
                    if (fileEntity != null)
                    {
                        if (fileEntity.FileUpload_ID != 0)
                        {
                            this.StatusMessage = "File '" + fileName + "' already exists";
                            // this.ListRefresh = true;
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }

                    IsUploaded = uploadFile.Upload(fileName, fileContent, false);
                }
                this.ListRefresh = true;
                if (IsUploaded)
                    this.StatusMessage = "File Imported successfully.";
                else
                    this.StatusMessage = "File Corrupted.";
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
        }

        private void StandardImport_Load(object sender, EventArgs e)
        {
            this.Text = "Standard Import";
            this.StatusMessage = string.Empty;
        }
    }
}
