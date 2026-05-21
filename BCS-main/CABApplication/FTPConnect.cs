#region NameSpaces
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using SerialCommunication;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using CAB.MeterData.Upload;
using System.Threading;
using System.Reflection;
using CABFramework;
using Hunt.EPIC.Logging;
//using CABApplication;
#endregion

namespace CABApplication
{
    public partial class FTPConnect : MdiChildForm
    {
        #region PublicMembers

        static string ftpServerIP = "183.82.97.160";
        static string ftpUserID = "lng";
        static string ftpPassword = "2ng#123";
        static string remoteDir = "2nG";
        static string completeRemoteDirPath = string.Empty;
        static string selectedFileName = string.Empty;
        private static object LockUpload = new object();
        private bool IsFTPFileDeleteAllowed = false;
        private bool IsFTPListViewEnable = false;
        //--------------------Upload Variables-----------------
        Thread th = null;
        TreeView tv = null;
        static string[] AllSelectedFile = new string[1];
        static string readoutFileName = string.Empty;

        //--------------------------End---------------------------


        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FTPConnect).ToString());



        #endregion

        public FTPConnect()
        {
            try
            {
                InitializeComponent();
                IsFTPFileDeleteAllowed = Convert.ToBoolean(ConfigSettings.GetValue("IsFTPFileDeleteAllowed"));
                IsFTPListViewEnable = Convert.ToBoolean(ConfigSettings.GetValue("IsFTPListViewEnable"));
                lblDelete.Visible = IsFTPFileDeleteAllowed;
                if (!IsFTPListViewEnable && tabControl1.TabPages.Contains(tabListView))
                {
                    tabControl1.TabPages.Remove(tabListView);
                }
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "FTPConnect()", ex);
                
            }          
        }



        private void lstfileLiist_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lstfileLiist.SelectedIndex >= 0)
                {
                    UpdateSelectedFileText(lstfileLiist.SelectedItem.ToString());
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lstfileLiist_DoubleClick(object sender, EventArgs e)", ex);
            }
        }

        private void EnableDisableControl(Control cnt, bool IsEnable)
        {
            try
            {
                Action a = () =>
                    {
                        cnt.Enabled = IsEnable;
                        if (!IsEnable)
                        {
                            this.Cursor = Cursors.WaitCursor;
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "EnableDisableControl(Control cnt, bool IsEnable)", ex);
            }
        }


        private List<string> GetFileListFromServer(string Dir)
        {
            List<string> downloadFiles = new List<string>();
            try
            {
                UpdateStatusMessage("Connecting To Server...", Color.Green);
                Application.DoEvents();

                StringBuilder result = new StringBuilder();
                WebResponse response = null;
                StreamReader reader = null;

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + Dir));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;

                UpdateStatusMessage("Reading File List From Server...", Color.Green);
                Application.DoEvents();

                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                UpdateStatusMessage("Updating File List...", Color.Green);
                Application.DoEvents();

                while (!string.IsNullOrEmpty(line))
                {
                    line = line.Substring(line.IndexOf("/") + 1);
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }


                if (result.Length > 0)
                {
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    downloadFiles = result.ToString().Split('\n').ToList<string>();
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                UpdateSelectedFileText("");
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "GetFileListFromServer(string Dir)", ex);
                throw ex;
            }
            return downloadFiles;
        }


        private void UpdateStatusMessage(string Message, Color colr)
        {
            try
            {
                Action a = () =>
                    {
                        stsmsg.ForeColor = colr;
                        stsmsg.Text = Message;
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "UpdateStatusMessage(string Message, Color colr)", ex);
            }
        }


        private void DownloadFileFromServer(string path, string file)
        {
            try
            {

                UpdateStatusMessage("Connecting To Server...", Color.Green);
                Application.DoEvents();

                string localDestnDir = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");

                if (!Directory.Exists(localDestnDir))
                {
                    Directory.CreateDirectory(localDestnDir);
                }




                string uri = "ftp://" + ftpServerIP + "/" + path + "/" + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    UpdateStatusMessage("", Color.White);
                    Application.DoEvents();

                    MessageBox.Show("Server Not Found!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                #region File Downloading Source Code

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + path + "/" + file));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;

                UpdateStatusMessage("Preparing To Download...", Color.Green);
                Application.DoEvents();


                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long fileSize = response.ContentLength;
                Stream responseStream = response.GetResponseStream();


                int Length = 2048;

                string AllFileData = string.Empty;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                List<byte> ReceivedDatabytes = new List<byte>();

                FileStream localFileStream = new FileStream(localDestnDir + "\\" + file, FileMode.Create);

                while (bytesRead > 0)
                {
                    localFileStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                    UpdateStatusMessage("Downloading File From Server...", Color.Green);
                    Application.DoEvents();
                }

                //Close all Handles
                localFileStream.Close();
                response.Close();
                reqFTP = null;


                if (File.Exists(localDestnDir + "\\" + file))
                {
                    UpdateStatusMessage("File successfully Downloaded From Server...", Color.Green);
                    Application.DoEvents();
                }

                #endregion


                #region File Auto Uploading Source Code

                bool IsAutoUpload = Convert.ToBoolean(ConfigSettings.GetValue("IsAutoUpload"));

                if (IsAutoUpload && (File.Exists(localDestnDir + "\\" + file)))
                {
                    string Message = string.Empty;
                    bool IsUploaded = UploadFile(localDestnDir + "\\" + file, out Message);
                    if (IsUploaded)
                    {
                        UpdateStatusMessage("File successfully Uploaded in BCS...", Color.Green);
                        Application.DoEvents();
                    }
                    else
                    {
                        UpdateStatusMessage(Message, Color.Green);
                        Application.DoEvents();
                    }
                }

                #endregion








            }
            catch (WebException wEx)    //Exception log for catch block
            {

                MessageBox.Show(wEx.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("", Color.White);
                logger.Log(LOGLEVELS.Error, "DownloadFileFromServer(string path, string file)", wEx);
                Application.DoEvents();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("", Color.White);
                logger.Log(LOGLEVELS.Error, "DownloadFileFromServer(string path, string file)", ex);
                Application.DoEvents();
            }
            finally
            {
                SetDefaultCursor();
            }
        }



        private void DownloadFileFromServer(TreeNode tn)
        {
            try
            {

                UpdateStatusMessage("Connecting To Server...", Color.Green);
                Application.DoEvents();

                string localDestnDir = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");

                if (!Directory.Exists(localDestnDir))
                {
                    Directory.CreateDirectory(localDestnDir);
                }




                string uri = "ftp://" + ftpServerIP + "/" + tn.Tag;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    UpdateStatusMessage("", Color.White);
                    Application.DoEvents();

                    MessageBox.Show("Server Not Found!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                #region File Downloading Source Code

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + tn.Tag));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;

                UpdateStatusMessage("Preparing To Download...", Color.Green);
                Application.DoEvents();


                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long fileSize = response.ContentLength;
                Stream responseStream = response.GetResponseStream();


                int Length = 2048;

                string AllFileData = string.Empty;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                List<byte> ReceivedDatabytes = new List<byte>();

                FileStream localFileStream = new FileStream(localDestnDir + "\\" + tn.Text, FileMode.Create);

                while (bytesRead > 0)
                {
                    localFileStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                    UpdateStatusMessage("Downloading File From Server...", Color.Green);
                    Application.DoEvents();
                }

                //Close all Handles
                localFileStream.Close();
                response.Close();
                reqFTP = null;


                if (File.Exists(localDestnDir + "\\" + tn.Text))
                {
                    UpdateStatusMessage("File successfully Downloaded From Server...", Color.Green);
                    Application.DoEvents();
                }

                #endregion


                #region File Auto Uploading Source Code

                bool IsAutoUpload = Convert.ToBoolean(ConfigSettings.GetValue("IsAutoUpload"));

                if (IsAutoUpload && (File.Exists(localDestnDir + "\\" + tn.Text)))
                {
                    string Message = string.Empty;
                    bool IsUploaded = UploadFile(localDestnDir + "\\" + tn.Text, out Message);
                    if (IsUploaded)
                    {
                        UpdateStatusMessage("File successfully Uploaded in BCS...", Color.Green);
                        Application.DoEvents();
                    }
                    else
                    {
                        UpdateStatusMessage(Message, Color.Green);
                        Application.DoEvents();
                    }
                }

                #endregion








            }
            catch (WebException wEx)    //Exception log for catch block
            {

                MessageBox.Show(wEx.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "DownloadFileFromServer(TreeNode tn)", wEx);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "DownloadFileFromServer(TreeNode tn)", ex);
            }
            finally
            {
                SetDefaultCursor();
            }
        }

        private void SetDefaultCursor()
        {
            try
            {
                Action a = () =>
                    {
                        this.Cursor = Cursors.Default;
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "SetDefaultCursor()", ex);
            }
        }


        private static bool UploadFile(string filePath, out string resultMessage)
        {
            lock (LockUpload)
            {
                resultMessage = string.Empty;
                bool IsUploaded = false;
                try
                {
                    UploadFile uploadFile = new UploadFile();
                    ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.FTP).ToString());
                    IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage, null);
                }
                catch (Exception ex)    //Exception log for catch block
                {

                    //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                    MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    logger.Log(LOGLEVELS.Error, "UploadFile(string filePath, out string resultMessage)", ex);
                }
                return IsUploaded;
            }
        }




        private void DeleteFileFromServer(string path, string file)
        {
            try
            {
                UpdateStatusMessage("Connecting To Server...", Color.Green);
                Application.DoEvents();

                string uri = "ftp://" + ftpServerIP + "/" + path + "/" + file;
                Uri serverUri = new Uri(uri);

                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    UpdateStatusMessage("", Color.White);
                    Application.DoEvents();


                    MessageBox.Show("Server Not Found!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    UpdateSelectedFileText("");

                    return;
                }


                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Proxy = null;
                request.KeepAlive = false;
                request.UsePassive = false;

                UpdateStatusMessage("Deleting File From Server...", Color.Green);
                Application.DoEvents();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                UpdateStatusMessage(response.StatusDescription, Color.Green);
                Application.DoEvents();

                response.Close();

                RemoveSelectedElementFromFileList();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                UpdateSelectedFileText("");
                logger.Log(LOGLEVELS.Error, "DeleteFileFromServer(string path, string file)", ex);
                throw ex;
            }
            finally
            {
                SetDefaultCursor();
            }
        }


        private void RemoveSelectedElementFromFileList()
        {
            try
            {
                Action a = () =>
                    {
                        if (lstfileLiist.SelectedIndex >= 0)
                        {
                            lstfileLiist.Items.RemoveAt(lstfileLiist.SelectedIndex);
                        }
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "RemoveSelectedElementFromFileList()", ex);
            }
        }



        private void FTPConnect_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();

                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");
                remoteDir = ConfigSettings.GetValue("ServerDirectory");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Could not load values!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "FTPConnect_Load(object sender, EventArgs e)", ex);
            }
        }


        private void lstfileLiist_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstfileLiist.SelectedIndex >= 0)
                {
                    UpdateSelectedFileText(lstfileLiist.SelectedItem.ToString());
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lstfileLiist_Click(object sender, EventArgs e)", ex);
            }
        }



        private void lblClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lblClose_Click(object sender, EventArgs e)", ex);
            }
        }





        private void FillTreeView()
        {
            try
            {
                th = new Thread(FillFileListTreeView);
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillTreeView()", ex);
                throw ex;
            }
        }


        private void AddTreeNodeRecursively(TreeNode tnParent, List<String> FTPfiles, string ParentPath)
        {
            try
            {
                foreach (string item in FTPfiles)
                {
                    TreeNode tn = GetTreeNode(item, ParentPath);
                    if (tn != null)
                    {
                        tnParent.Nodes.Add(tn);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AddTreeNodeRecursively(TreeNode tnParent, List<String> FTPfiles, string ParentPath)", ex);
                return;
            }
        }


        private TreeNode GetTreeNode(String FTPfileorFolderName, string ParentPath)
        {
            TreeNode tn = null;
            try
            {
                tn = new TreeNode();
                tn.Checked = false;
                tn.Name = FTPfileorFolderName;
                tn.Text = FTPfileorFolderName;
                string NodePath = string.Empty;
                if (ParentPath == string.Empty)
                {
                    //parent folder
                    NodePath = FTPfileorFolderName;
                }
                else
                {
                    //child folder
                    NodePath = ParentPath + "/" + FTPfileorFolderName;
                }


                tn.Tag = NodePath;


                if (!FTPfileorFolderName.Contains("."))
                {
                    List<String> FTPChildfiles = GetFileListFromServer(NodePath);
                    if (FTPChildfiles.Count > 0)
                    {
                        AddTreeNodeRecursively(tn, FTPChildfiles, NodePath);
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTreeNode(String FTPfileorFolderName, string ParentPath)", ex);
                return null;
            }
            return tn;
        }





        private void FillFileListTreeView()
        {
            try
            {
                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");
                //root folder
                remoteDir = ConfigSettings.GetValue("ServerDirectory");
                EnableDisableControl(this, false);
                EnableDisableControl(lsttreeLiist, false);
                EnableDisableControl(menuToolTreeView, false);
                string Dir = remoteDir;
                List<String> FTPfiles = GetFileListFromServer(Dir);
                if (FTPfiles.Count == 0)
                {
                    MessageBox.Show("No File Available on FTP Server!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    Application.DoEvents();
                    return;
                }
                else
                {
                    FTPfiles.Reverse();
                    ShowTreeView(FTPfiles, Dir);
                    UpdateStatusMessage("File List Downloaded Succesfully!", Color.Green);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Could not get the list!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "FillFileListTreeView()", ex);
            }
            finally
            {
                EnableDisableControl(this, true);
                EnableDisableControl(lsttreeLiist, true);
                EnableDisableControl(menuToolTreeView, true);
            }

        }

        private void ShowTreeView(List<string> FTPfiles, string ParentDirPath)
        {
            try
            {
                TreeNode tn = new TreeNode();
                tn.Name = "root";
                tn.Text = "Cabcon Files";
                AddTreeNodeRecursively(tn, FTPfiles, ParentDirPath);
                Action a = () =>
                    {
                        //Initialize new TreeView                        
                        tv = new TreeView();
                        tv.AfterCheck += new TreeViewEventHandler(tv_AfterCheck);
                        tv.Dock = DockStyle.Fill;
                        tv.CheckBoxes = true;
                        tv.Nodes.Add(tn);
                        tn.Checked = false;
                        lsttreeLiist.Controls.Clear();
                        lsttreeLiist.Controls.Add(tv);
                        lsttreeLiist.Refresh();
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ShowTreeView(List<string> FTPfiles, string ParentDirPath)", ex);
            }
        }

        private void CheckAllChildNodesRecursive(TreeNode tn)
        {
            try
            {
                foreach (TreeNode item in tn.Nodes)
                {
                    item.Checked = tn.Checked;
                    if (item.Nodes.Count > 0)
                    {
                        CheckAllChildNodesRecursive(item);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckAllChildNodesRecursive(TreeNode tn)", ex);

            }
        }

        void tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Action != TreeViewAction.Unknown)
                {
                    TreeNode tn = e.Node;
                    if (tn.Nodes.Count > 0)
                    {
                        CheckAllChildNodesRecursive(tn);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "tv_AfterCheck(object sender, TreeViewEventArgs e)", ex);

            }
        }


        private void ClearFileList()
        {
            try
            {
                Action a = () =>
                    {
                        lstfileLiist.Items.Clear();
                        lstfileLiist.Controls.Clear();
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Could not clear the list!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "ClearFileList()", ex);
            }
        }


        private void FillFileList(List<string> FTPfiles)
        {
            try
            {
                Action a = () =>
                    {
                        foreach (string objfile in FTPfiles)
                        {
                            lstfileLiist.Items.Add(objfile);

                        }
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Could not fill the list!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "FillFileList(List<string> FTPfiles)", ex);
            }
        }


        private void GetFileList()
        {
            try
            {
                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");

                if (selectedFileName == string.Empty)
                {
                    //root folder
                    remoteDir = ConfigSettings.GetValue("ServerDirectory");
                    completeRemoteDirPath = remoteDir;
                }
                else if (completeRemoteDirPath == string.Empty)
                {
                    //parent folder
                    completeRemoteDirPath = selectedFileName;
                }
                else
                {
                    //child folder
                    completeRemoteDirPath = completeRemoteDirPath + "/" + selectedFileName;
                }



                ClearFileList();

                UpdateSelectedFileText("");


                EnableDisableControl(this, false);
                EnableDisableControl(grpFTP, false);
                EnableDisableControl(menuTool, false);

                List<String> FTPfiles = GetFileListFromServer(completeRemoteDirPath);





                if (FTPfiles.Count == 0)
                {
                    MessageBox.Show("No File Available on FTP Server!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    UpdateSelectedFileText("");
                    Application.DoEvents();
                    return;
                }
                else
                {
                    FTPfiles.Reverse();//FTPfiles.ToArray().OrderByDescending(d => d).ToArray();
                    UpdateStatusMessage("File List Downloaded Succesfully!", Color.Green);
                    Application.DoEvents();
                }


                FillFileList(FTPfiles);


            }
            catch (Exception ex)    //Exception log for catch block
            {
                completeRemoteDirPath = string.Empty;                
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Could not get the list!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "GetFileList()", ex);
            }
            finally
            {
                EnableDisableControl(this, true);
                EnableDisableControl(grpFTP, true);
                EnableDisableControl(menuTool, true);
            }

        }


        private void lblGetFileList_Click(object sender, EventArgs e)
        {
            try
            {
                selectedFileName = txtSelectedFile.Text.Trim();
                th = new Thread(GetFileList);
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Could not get the list!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "", ex);
            }

        }


        private void UpdateSelectedFileText(string text)
        {
            try
            {
                Action a = () =>
                    {
                        txtSelectedFile.Text = text;
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "UpdateSelectedFileText(string text)", ex);
            }
        }






        private void DownloadFile()
        {
            try
            {
                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");
                remoteDir = ConfigSettings.GetValue("ServerDirectory");

                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                EnableDisableControl(this, false);
                EnableDisableControl(grpFTP, false);
                EnableDisableControl(menuTool, false);

                if (selectedFileName.Length <= 0)
                {
                    MessageBox.Show("Select File From List", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);


                }
                else
                {
                    DownloadFileFromServer(completeRemoteDirPath, selectedFileName);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DownloadFile()", ex);
            }
            finally
            {
                EnableDisableControl(this, true);
                EnableDisableControl(grpFTP, true);
                EnableDisableControl(menuTool, true);
            }


        }


        private void lblDownload_Click(object sender, EventArgs e)
        {
            try
            {
                selectedFileName = txtSelectedFile.Text.Trim();
                th = new Thread(DownloadFile);
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lblDownload_Click(object sender, EventArgs e)", ex);
            }

        }


        private void DeleteFile()
        {
            try
            {
                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");
                remoteDir = ConfigSettings.GetValue("ServerDirectory");

                EnableDisableControl(this, false);
                UpdateStatusMessage("", Color.White);
                Application.DoEvents();

                if (selectedFileName.Length <= 0)
                {
                    MessageBox.Show("Select File From List", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    UpdateSelectedFileText("");
                    return;
                }
                if (MessageBox.Show("Do You Want To Delete Selected File Form Server??", "BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No) return;
                {
                    DeleteFileFromServer(completeRemoteDirPath, selectedFileName);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                UpdateSelectedFileText("");
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DeleteFile()", ex);
            }

            finally
            {
                EnableDisableControl(this, true);
            }
        }

        private void lblDelete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedFileName = txtSelectedFile.Text.Trim();
                th = new Thread(DeleteFile);
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lblDelete_Click(object sender, EventArgs e)", ex);
            }

        }


        private void lblGetTreeList_Click(object sender, EventArgs e)
        {
            try
            {
                this.FillTreeView();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lblGetTreeList_Click(object sender, EventArgs e)", ex);
            }
        }

        private void lblDownloadFileList_Click(object sender, EventArgs e)
        {
            try
            {
                selectedFileName = txtSelectedFile.Text.Trim();
                if (tv != null)
                {
                    th = new Thread(DownloadTreeViewSelectedFile);
                    th.IsBackground = true;
                    th.Start();
                }
                else
                {
                    MessageBox.Show("Files not selected in TreeView!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "lblDownloadFileList_Click(object sender, EventArgs e)", ex);
            }
        }

        private List<TreeNode> GetCheckedTreeNodeListRecursive(TreeNode tn)
        {
            List<TreeNode> lstCheckTreeNode = new List<TreeNode>();
            try
            {
                foreach (TreeNode item in tn.Nodes)
                {
                    //if item is 2NG file and is checked add it in list
                    if (item.Checked && item.Text.ToUpper().Contains(".2NG"))
                    {
                        lstCheckTreeNode.Add(item);
                    }
                    //if item contains child, add checked list recursively
                    if (item.Nodes.Count > 0)
                    {
                        lstCheckTreeNode.AddRange(GetCheckedTreeNodeListRecursive(item));
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "GetCheckedTreeNodeListRecursive(TreeNode tn)", ex);
            }
            return lstCheckTreeNode;
        }


        private void DownloadTreeViewSelectedFile()
        {
            try
            {
                ftpServerIP = ConfigSettings.GetValue("FTPIP");
                ftpUserID = ConfigSettings.GetValue("LoginID");
                ftpPassword = ConfigSettings.GetValue("LoginPassword");
                remoteDir = ConfigSettings.GetValue("ServerDirectory");

                UpdateStatusMessage("", Color.White);
                Application.DoEvents();
                EnableDisableControl(this, false);
                EnableDisableControl(lsttreeLiist, false);
                EnableDisableControl(menuToolTreeView, false);
                List<TreeNode> lstCheckTreeNode = new List<TreeNode>();
                foreach (TreeNode item in tv.Nodes)
                {
                    //if item is 2NG file and is checked add it in list
                    if (item.Checked && item.Text.ToUpper().Contains(".2NG"))
                    {
                        lstCheckTreeNode.Add(item);
                    }
                    //if item contains child, add checked list recursively
                    if (item.Nodes.Count > 0)
                    {
                        lstCheckTreeNode.AddRange(GetCheckedTreeNodeListRecursive(item));
                    }
                }
                if (lstCheckTreeNode.Count > 0)
                {
                    foreach (TreeNode item in lstCheckTreeNode)
                    {
                        DownloadFileFromServer(item);
                    }
                }
                else
                {
                    MessageBox.Show(".2nG Files not selected in TreeView!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Unable to perform operation!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DownloadTreeViewSelectedFile()", ex);
            }
            finally
            {
                EnableDisableControl(this, true);
                EnableDisableControl(lsttreeLiist, true);
                EnableDisableControl(menuToolTreeView, true);
            }
        }

        private void FTPConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (th != null && th.IsAlive)
                {
                    th.Abort();
                }               
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "FTPConnect_FormClosing(object sender, FormClosingEventArgs e)", ex);
            }
        }
    }
}
