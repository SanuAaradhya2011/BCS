using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.Framework.Utility;
using CAB.MeterData.Upload;
using CAB.UI.Controls;
using CABFramework;
using LTCTBLL;


namespace CAB.UI
{
    public partial class DLMS650UploadFile : MdiChildForm 
    {
        private OpenFileDialog openFileDialog;
        MeterDataBLL meterDataBLL = null;
        string lsColumnNames = string.Empty;
        private System.Resources.ResourceManager resourceMgr;
        private GenerateEntity entityGenerator = null;
        private UploadFile uploadFile = null;
        bool isPUMA = false;
        private string meterDataType = string.Empty;
        private string meterid = ConfigInfo.ActiveMeterDataId;
        TabNameBLL tabNameBll;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650UploadFile).ToString());
        private Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
        public DLMS650UploadFile()
        { 
            InitializeComponent();            
            meterDataBLL = new MeterDataBLL();
            tabNameBll = new TabNameBLL();
            entityGenerator = new GenerateEntity();
            uploadFile = new UploadFile();
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.DLMS650UploadFile", System.Reflection.Assembly.GetExecutingAssembly());
            if (CAB.Framework.UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
            {
                isPUMA = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetCommType()
        {
            CommTypes commType = CommTypes.Direct;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (channelType == CABCommunication.PhysicalLayer.ChannelType.GSM.ToString())
            {
                commType = CommTypes.GSM;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString())
            {
                commType = CommTypes.PSTN;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString())
            {
                commType = CommTypes.GPRS;
            }
            return (int)commType;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtBoxFileName.Text.Trim() == "")
            {
                MessageBox.Show("Please Select File", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtBoxFileName.Text.Length == 0)
            {
                MessageBox.Show("Please Select File", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }           
            int FileCount = 0;
            foreach (string fileName in openFileDialog.FileNames)
            {
                string[] strSplit = fileName.Split('\\');
                if (strSplit != null)
                {
                    string RelativeFileName = strSplit[strSplit.Length - 1];
                    if (RelativeFileName.Trim().Length > 40)
                    {
                        MessageBox.Show("File name should not exceed 40 characters." + fileName, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxFileName.Focus();
                        return;
                    }
                }
                FileCount++;
            }
            if (FileCount == 0)
            {
                MessageBox.Show("Please Select File", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxFileName.Text = "";
                return;
            }            
            //it checks whether a file with the same name exists or not
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            FileUploadMasterEntity fileUploadEntity = fileUploadMasterBLL.ValidateFile(txtBoxFileName.Text) as FileUploadMasterEntity;
            if (fileUploadEntity != null && fileUploadEntity.FileUpload_ID != 0)
            {
                DialogResult dialogResult = MessageBox.Show(fileUploadEntity.FileName + " already exists.\nDo you want to replace it?", "BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
			this.Cursor = Cursors.WaitCursor;
            UploadFiles();
			this.Cursor = Cursors.Default;
        }
        bool isFDLfile = false;
        private void formatterBilling_OnChannelStatusChanged(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }                                                                   
        /// <summary>
        /// Upload Readout file
        /// </summary>
        private void UploadFiles()
        {
            string dtpath = string.Empty; ;
            try
            {
                string NONDLMSText = string.Empty;
               
                bool IsUploaded = false;
                string resultMessage = string.Empty;
                this.Cursor = Cursors.WaitCursor;
                ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.Upload).ToString());
                foreach (string fileName in openFileDialog.FileNames)
                {
                    this.StatusMessage = "Uploading " + Path.GetFileName(fileName) + "..";
                    Application.DoEvents();
                    if (Path.GetExtension(fileName).ToUpper() == ".ZIP")
                    {
                        //this.StatusMessage = "Uploading " + Path.GetFileName(fileName)+ "..";
                        
                        CMRICommunication CmriCommunicationEntity = new CMRICommunication();
                        CmriCommunicationEntity.On_StatusChanged += new IsStatusChanged(Upload_OnStatusChanged);
                        string fullpath = Path.GetFullPath(fileName);
                        dtpath = CmriCommunicationEntity.UnZipFiles(fullpath);
                        string[] fileCollection = Directory.GetFiles(dtpath, "*.LGD");
                        string[] fileCollection1 = Directory.GetFiles(dtpath, "*.FTP");
                        string[] fileCollection2 = Directory.GetFiles(dtpath, "*.CAB");

                        //string[] fileCollection3 = Directory.GetFiles(dtpath, "*.SLG");
                        //string[] fileCollection4 = Directory.GetFiles(dtpath, "*.2NG");


                        if ((fileCollection.Length <= 0 || fileCollection1.Length <= 0) && fileCollection2.Length <= 0)
                        {

                            this.Cursor = Cursors.Default;
                            this.ListRefresh = true;
                            resultMessage = "File Not Found.";
                            Application.DoEvents();

                        }
                        else
                        {
                            if (!dtpath.Contains(".2NG"))
                                NONDLMSText = CmriCommunicationEntity.GetIECCMRIData(dtpath);

                            if (dtpath == String.Empty && NONDLMSText == String.Empty)
                            {
                                this.Cursor = Cursors.Default;
                                this.ListRefresh = true;
                                this.StatusMessage = "No data available for readout.";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                                //EnableStopTimer();
                                // this.StatusMessage = "ZIP readout successful";
                                //  Application.DoEvents();


                                if (dtpath.Contains(".2NG"))
                                {
                                    CmriCommunicationEntity.ReadDataAndCreateInstantBillingCMRIFiles(dtpath, "Default CMRI");
                                }
                                else if (dtpath != String.Empty)
                                {
                                    CmriCommunicationEntity.ReadDataAndCreateDLMSFileFromNativeCMRIFiles(dtpath, "Default CMRI");

                                    //region Uplaod NON DLMS
                                    if (UtilityDetails.IECSupport && NONDLMSText != String.Empty)
                                    {
                                        CmriCommunicationEntity.UploadNonDLMSCMRIFile(NONDLMSText, "Default CMRI");
                                    }
                                }

                                if (dtpath != String.Empty || (UtilityDetails.IECSupport && NONDLMSText != String.Empty))
                                {
                                    this.Cursor = Cursors.Default;
                                    this.ListRefresh = true;
                                    IsUploaded = true;
                                    //this.StatusMessage = "File Uploaded Successfully";
                                    //Application.DoEvents();

                                }
                            }
                        }
                    }
                    else if (Path.GetExtension(fileName).ToUpper() == ".2NG")
                    {

                        IsUploaded = uploadFile.Upload2NGFile(fileName, uploadFile.GetContent(fileName),true,out resultMessage,null);

                    }
                    else if (Path.GetExtension(fileName).ToUpper() == ".SLG") // Story - 347720 
                    {
                        string filedata= uploadFile.GetIECFileContent(fileName);
                        if (filedata.Contains("NonSupportedFileError")) 
                        {
                            this.Cursor = Cursors.Default;
                            this.ListRefresh = true;
                            resultMessage = "Non-Generic BCS Supported Readout File"; 
                            Application.DoEvents();
                           
                        }
                        else
                        {
                            IsUploaded = uploadFile.UploadSLGFile(fileName, filedata, true, out resultMessage, null);
                        }
                    }
                    else 
                    {

                        IsUploaded = uploadFile.UploadCABFile(fileName, uploadFile.GetIECFileContent(fileName),true,out resultMessage,null);
                    }
                }

                if (IsUploaded)
                {
                    this.StatusMessage = resourceMgr.GetString("Fileuploadedsuccessfully");
                }
                else
                {
                    this.StatusMessage = resultMessage;
                }
                this.Cursor = Cursors.Default;
                this.ListRefresh = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Uploading ZIP file", ex);
            }
           
            
        }
       

        private void Upload_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();

            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExt = "2NG";
            /* GKG 13/02/2013 JDVVNL Utility Addition */
            //openFileDialog.Filter = "Readout(*.2NG)|*.2NG|Readout(*.FDL)|*.FDL";
            if (UtilityDetails.PrimaryUtlityName == "JDVVNL")
            {
                openFileDialog.Filter = "Readout(*.2NG)|*.2NG";
            }
            else
            {
                openFileDialog.Filter = "Readout(*.CAB;*.2NG;*.SLG;*.ZIP)|*.CAB;*.2NG;*.SLG;*.ZIP"; // Story - 347720
            }
            /* GKG 13/02/2013 JDVVNL Utility Addition */
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtBoxFileName.Text = string.Empty;
                int counter = 0;
                foreach (string fileName in openFileDialog.SafeFileNames)
                    counter++;

                foreach (string fileName in openFileDialog.SafeFileNames)
                { 
                    txtBoxFileName.Text += fileName;
                    break;
                }
            }
        }

        private void UploadFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void DLMS650UploadFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }
    } 
}
