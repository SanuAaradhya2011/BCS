using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Entity;
using CAB.BLL;
using System.IO;
using CAB.MeterData.Upload;
using CABFramework;
using Shell32;

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
            openFile.DefaultExt = "ZIP";
            openFile.Filter = "Export File(*.ZIP)|*.ZIP";
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
            UploadFile uploadFile = new UploadFile();
            ApplicationType types = ConfigInfo.GetApplicationType();
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            if (txtBoxFileName.Text.Trim() == "")
            {
                this.StatusMessage = "File name can not be empty";
                return;
            }
            string resultMessage = string.Empty;
            this.Cursor = Cursors.WaitCursor;
            bool IsUploaded = false;
            this.StatusMessage = "Importing file.";
            Application.DoEvents();

            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                IsUploaded = false;
                //uploadFile = new DLMS650UploadFile();
                string fileContent = "";
                string filenames = "";
                foreach (string fileName in openFile.FileNames)
                {

                    Shell32.Shell Shell = new Shell32.Shell();
                    Folder output = Shell.NameSpace((tempPath));
                    Folder input = Shell.NameSpace((fileName));

                    foreach (var file in input.Items())
                    {
                        output.CopyHere(file, 4 | 16);
                    }
                    string[] files = Directory.GetFiles(tempPath);
                    ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.Import).ToString());
                    int buttonValue = 0;
                    RestoreDialogBox confirmationBox = new RestoreDialogBox();
                    foreach (string file in files)
                    {
                        fileContent = uploadFile.GetContent(file);
                        filenames = Path.GetFileName(file);

                        FileUploadMasterEntity fileEntity = new FileUploadMasterBLL().ValidateFile(filenames) as FileUploadMasterEntity;
                        if (fileEntity != null)
                        {
                            if (fileEntity.FileUpload_ID != 0)
                            {
                                if (buttonValue == 0 || buttonValue == 1 || buttonValue == 3)
                                {
                                    string message = "The database already contains a file named " + filenames + ".\r\n\n Would you like to replace the existing file?\r\n";
                                    RestoreDialogBoxResult result = confirmationBox.ShowMemoryDialog(message, "Confirm File Replace");
                                    Application.DoEvents();
                                    if (result == RestoreDialogBoxResult.Yes)
                                    {
                                        buttonValue = 1;
                                    }
                                    else if (result == RestoreDialogBoxResult.YesToAll)
                                    {
                                        buttonValue = 2;
                                    }
                                    else if (result == RestoreDialogBoxResult.No)
                                    {
                                        buttonValue = 3;
                                        if (File.Exists(file))
                                            File.Delete(file);
                                        continue;
                                    }
                                    else if (result == RestoreDialogBoxResult.NoToAll)
                                        buttonValue = 4;
                                    else if (result == RestoreDialogBoxResult.Cancel)
                                    {
                                        foreach (string filetodelete in files)
                                        {
                                            if (File.Exists(filetodelete))
                                                File.Delete(filetodelete);
                                        }
                                        this.StatusMessage = string.Empty;
                                        Application.DoEvents();
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                }
                                else if (buttonValue == 4)
                                {
                                    if (File.Exists(file))
                                        File.Delete(file);
                                    continue;
                                }

                            }
                            if (string.IsNullOrEmpty(fileContent))
                                continue;
                            if (filenames.ToUpper().Contains(".2NG"))
                                IsUploaded = uploadFile.Upload2NGFile(file, fileContent, true, out resultMessage,null);
                            else if (filenames.ToUpper().Contains(".SLG"))
                                IsUploaded = uploadFile.UploadSLGFile(file, uploadFile.GetIECFileContent(file), true, out resultMessage, null);
                            else
                            {
                                IsUploaded = uploadFile.UploadCABFile(file, uploadFile.GetIECFileContent(file), true, out resultMessage,null);
                            }

                        }
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                }

                if (IsUploaded)
                    this.StatusMessage = "File Imported successfully.";
                else
                    this.StatusMessage = resultMessage;
                Application.DoEvents();
            }
            this.ListRefresh = true;
            this.Cursor = Cursors.Default;
        }

        private void StandardImport_Load(object sender, EventArgs e)
        {
            this.Text = "Standard Import";
            this.StatusMessage = string.Empty;
        }
    }
}
