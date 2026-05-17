/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using CAB.Entity;
using CAB.BLL;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Collections;
using CABApplication;
using LTCTBLL;
using CABCommunication.Common;
using CABCommunication.WrapperLayer;

namespace CAB.UI
{
	public partial class MainForm : CABForm
	{
		IList<UserRightEntity> userRightEntities = null;
		private int oldIndex;
		private MeterFileList meterFileList;
		private MeterConsumerFileList meterConsumerFileList;
		private UserRightBLL userRightBLL = new UserRightBLL();
		string data = string.Empty;
        int originalButtonPosition = 0;
        string meterID;
        DataSet tmpData = null;
        Dictionary<string, string> monthList = new Dictionary<string, string>();
        LoginForm loginform = new LoginForm();
        private Communication communication;
        /* Commented by dhirendra on 15 march 10
		MyCrypro objCrypto = new MyCrypro();
		//private MainMenu mainMenu;
		public static long UserId = 0;
		LogFileData StoreLog;
		int recordCount = 10;
		bool isKeyDown_TxtSearch = false;
		public static string UserIDforPassword = string.Empty;
		public static bool isConnected = false;
        
		//this variable is used to check when user has clicked on the explorer treeview while it was previoulsy selected
		int ExplorerClickCount = 0; 

		//public delegate void GetDataGridValues();
		*/
        private bool isTNEB = false;

		public MainForm()
		{
            
			InitializeComponent();
			this.IsMdiContainer = true;
            if ((UtilityDetails.UtilityName == UtilityEntity.TNEB) || (UtilityDetails.UtilityName == UtilityEntity.TNEB1))
            {
                isTNEB = true;
            }
            //Added to show changed form according to the application
            if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL || UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL)
            {
                toolStripMenuItem1.Visible = allDataToolStripMenuItem.Visible = generalToolStripMenuItem.Visible = true;
                instantToolStripMenuItem.Visible = true;
                toolStripBtnMeterProgramming.Visible = true;
                MeterProgrammingToolStripMenuItem.Visible = true;
                toolStripBtnMeterProgramming.Enabled = true;
                MeterProgrammingToolStripMenuItem.Enabled = true;
                meterConfigurationsToolStripMenuItem.Visible = false;
                meterConfigurationsToolStripMenuItem.Enabled = false;
                
            }
            MeterFileList meterfilelist = new MeterFileList();
			/* Commented by dhirendra on 15 march 10
			UpdateUI();
			*/
		}

		private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("UserInformations") == false)
			{
				UserInformations userInformations = new UserInformations();
				userInformations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				userInformations.MdiParent = this;
				userInformations.Show();
			}
		}

		private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ChangePassword") == false)
			{
				ChangePassword changePassword = new ChangePassword();
				changePassword.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				changePassword.MdiParent = this;
				changePassword.Show();
			}
			/* Commented by dhirendra on 15 march 10
			if (ActivateThisChild("ChangePassword") == false)
			{
				PresentationLayer.WindowsForms.ChangePassword objChangePassword = new CAB.UI.ChangePassword();
				objChangePassword.MdiParent = this;
				//splitContainer1.Panel2.Controls.Add(objChangePassword);
				objChangePassword.userid = 3;
				objChangePassword.Show();
			}
			*/
		}        
        public void PendingFilesLabel()
        {
            string fileCount = string.Empty;
            fileCount = CsvFileReader.ReadPendingFiles();
            if (fileCount != string.Empty && (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1))
            {
                lnklblFiles.Visible = true;
                lnklblFiles.Text = fileCount;
            }
            else
            {
                lnklblFiles.Visible = false;
            }
        }
        public string SetPendingFiles
        {
            set
            {
                lnklblFiles.Text = value;   
            }
        }
        public bool SetPendingFilesVisible
        {
            set
            {
                if (!isTNEB)
                {
                    lnklblFiles.Visible = false;
                }
                else
                {
                    lnklblFiles.Visible = value;
                }
            }
        }
        private string ConnectionMode
        {
            set {
                
                this.toolStripStatusLabel2.Text = string.Concat("Connection: ", value.ToString()); 
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Temp code to release TNEB IEC for validation to test pending IEC bugs
           // e650LTMeterCommunicationToolStripMenuItem.Visible = false;
            //systemSettingsToolStripMenuItem.Visible = false;
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? "Reader(MR)" : "Master(US)";
            ConnectionMode = (ConfigInfo.ChannelType == CAB.IECFramework.ChannelType.GSM) ? CAB.IECFramework.ChannelType.GSM.ToString() : "Normal"
                             + "  Port: " + ConfigSettings.GetValue("PortName") + "  Mode: " + mode;                                       

            originalButtonPosition = lngButton.Location.X;
            LoginForm obj = (LoginForm)Application.OpenForms["LoginForm"];
            if (obj.lbl_ShowDemo.Text == "label1")
            {
                registerProductToolStripMenuItem.Visible = false;
            }
            this.Text = "emSTAR";
            /*GKG 26/02/2013 135725 Feture Disabled as per discussion with Amitesh*/
            //PendingFilesLabel();
            lnklblFiles.Visible = false;
            /*GKG 26/02/2013 135725 Feture Disabled as per discussion with Amitesh*/
            ModuleMasterBLL moduleBLL = new ModuleMasterBLL();
            if (ConfigInfo.UserInformationID == 0)
            {
                //string rootId = ConfigInfo.RootLog();
                //if (rootId == "0")
                //    ConfigSettings.ChangeNode("RootLog", "1");
                //else
                //{
                //    MessageBox.Show("Please contact Vendor. Your Session has Expired", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Application.Exit();
                //}
                if (ConfigInfo.UserInformationID == 0)

                    UpdateINI();
                //moduleBLL.DeleteAllData();
            }
            int subGroupCount = 0;
            GroupDefinitionBLL groupDefinitionBLL = new GroupDefinitionBLL();
            subGroupCount = groupDefinitionBLL.ListDefaultValues();
            if (subGroupCount == 0)
            {
                string[] defaultSubGroups = new string[] { "Suspected Consumer", "Temporary Connection", "Monthly Reading", "Bi-Monthly Reading", "Tri-Monthly Reading" };
                groupDefinitionBLL.InsertDefaultValues(defaultSubGroups);
            }
            DataSet dataSet = moduleBLL.GetAllData();
            bool Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                moduleBLL.InsertDefaultData();

            CategoryMasterBLL categoryMasterBLL = new CategoryMasterBLL();
            dataSet = categoryMasterBLL.GetAllData();
            Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                categoryMasterBLL.InsertDefaultData();


            ConsumerTypeBLL consumerTypeBLL = new ConsumerTypeBLL();
            dataSet = consumerTypeBLL.ListDataSet();
            Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                consumerTypeBLL.InsertDefaultData();


            MeterTypeBLL meterTypeBLL = new MeterTypeBLL();
            dataSet = meterTypeBLL.ListDataSet();
            Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                meterTypeBLL.InsertDefaultData();

            MeterModelBLL meterModelBLL = new MeterModelBLL();
            dataSet = meterModelBLL.ListDataSet();
            Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                meterModelBLL.InsertDefaultData();

            MeterUnitBLL meterUnitBLL = new MeterUnitBLL();
            dataSet = meterUnitBLL.ListDataSet();
            Flag = false;
            if (dataSet == null)
                Flag = true;
            else
                if (dataSet.Tables.Count == 0)
                    Flag = true;
                else
                    if (dataSet.Tables[0].Rows.Count == 0)
                        Flag = true;
            if (Flag)
                meterUnitBLL.InsertDefaultData();
            if (ConfigInfo.UserInformationID == 0)
            {

                userGroupToolStripMenuItem.Enabled = toolStripBtnUser.Enabled = true;
                definitionToolStripMenuItem.Enabled = false;
                dataAcquisitionToolStripMenuItem.Enabled = false;
                reportsToolStripMenuItem.Enabled = false;
                helpToolStripMenuItem.Enabled = false;
                exitToolStripMenuItem.Enabled = true;
                toolStripBtnCMRICommunication.Enabled = false;
                toolStripBtnMeterCommunication.Enabled = false;
                toolStripBtnMeterProgramming.Enabled = false;
                toolStripBtnConsumerMeter.Enabled = false;
                toolStripBtnSystemConfig.Enabled = false;
                toolStripBtnReport.Enabled = false;
                toolStripBtnUploadCABFile.Enabled = false;
                changePasswordToolStripMenuItem.Enabled = false;
                applicationLogDetailsToolStripMenuItem.Enabled = false;
                exceptionLogDetailsToolStripMenuItem.Enabled = false;
                //toolStripBtnConsumerMeter.Enabled =
                //toolStripBtnCMRICommunication.Enabled =
                //toolStripBtnMeterCommunication.Enabled =
                //toolStripBtnMeterProgramming.Enabled =
                //toolStripBtnUploadCABFile.Enabled =
                //toolStripBtnSystemConfig.Enabled =
                //toolStripBtnReport.Enabled =

                //changePasswordToolStripMenuItem.Visible =
                //applicationLogDetailsToolStripMenuItem.Enabled =
                //exceptionLogDetailsToolStripMenuItem.Enabled =
                //definitionToolStripMenuItem.Enabled =
                //dataAcquisitionToolStripMenuItem.Enabled =
                //configurationToolStripMenuItem.Enabled =
                //dataArchiveToolStripMenuItem.Enabled =
                //exportImportToolStripMenuItem.Enabled =
                //reportsToolStripMenuItem.Enabled = true;

                //userGroupToolStripMenuItem.Enabled = true;
                //dataAcquisitionToolStripMenuItem.Enabled =  
                //toolStripBtnUser.Enabled = 
                //toolStripBtnCMRICommunication.Enabled =  
                //toolStripBtnMeterCommunication.Enabled =  
                //toolStripBtnMeterProgramming.Enabled =  
                //mRICommunicationToolStripMenuItem.Enabled =  
                //localCommunicationToolStripMenuItem.Enabled =  
                //readDataToolStripMenuItem.Enabled =  
                //MeterProgrammingToolStripMenuItem.Enabled =  
                //dataAcquisitionToolStripMenuItem.Enabled =  
                //cMRISchedulingToolStripMenuItem.Enabled =  
                //dataArchiveToolStripMenuItem.Enabled =  
                //exportImportToolStripMenuItem.Enabled =  
                //definitionToolStripMenuItem.Enabled =  
                //reportsToolStripMenuItem.Enabled =  
                //changePasswordToolStripMenuItem.Visible = false;
            }
            else
            {
                ////changePasswordToolStripMenuItem.Visible = true;
                userRightBLL = new UserRightBLL();

                bool userAdministrator = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "User Administrator");
                userManagementToolStripMenuItem.Enabled = applicationLogDetailsToolStripMenuItem.Enabled = exceptionLogDetailsToolStripMenuItem.Enabled = toolStripBtnUser.Enabled = userAdministrator;
                bool definition = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Definition");
                definitionToolStripMenuItem.Enabled = toolStripBtnConsumerMeter.Enabled = definition;
                bool cmriDownload = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "CMRI Download");
                mRICommunicationToolStripMenuItem.Enabled = cMRISchedulingToolStripMenuItem.Enabled = configurationToolStripMenuItem.Enabled = toolStripBtnSystemConfig.Enabled = toolStripBtnUploadCABFile.Enabled = toolStripBtnCMRICommunication.Enabled = cmriDownload;
                bool dataReadout = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Readout");
                configurationToolStripMenuItem.Enabled = toolStripBtnSystemConfig.Enabled = toolStripBtnUploadCABFile.Enabled = readDataToolStripMenuItem.Enabled = GSMCommToolStripMenuItem.Enabled = scheduleReadingStatusToolStripMenuItem.Enabled = toolStripBtnMeterCommunication.Enabled = dataReadout;
                bool programming = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Programming");
                configurationToolStripMenuItem.Enabled = toolStripBtnSystemConfig.Enabled = toolStripBtnUploadCABFile.Enabled = MeterProgrammingToolStripMenuItem.Enabled = configurePortToolStripMenuItem.Enabled = gSMSchedulingToolStripMenuItem.Enabled = assignGSMScheduleToolStripMenuItem.Enabled = setGSMScheduleToolStripMenuItem.Enabled = toolStripBtnMeterProgramming.Enabled = programming;
                exportImportToolStripMenuItem.Enabled = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Export/Import");
                bool reportsView = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Reports View");
                reportsToolStripMenuItem.Enabled = toolStripBtnReport.Enabled = listViewExplorer.Enabled = cboSearch.Enabled = reportsView;
                dataArchiveToolStripMenuItem.Enabled = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Archive");

                if (!cmriDownload && !programming)
                {
                    toolStripBtnUploadCABFile.Enabled = false;
                    //e650LTMeterCommunicationToolStripMenuItem.Enabled = false;
                }
                else
                {
                    toolStripBtnUploadCABFile.Enabled = true;
                    //e650LTMeterCommunicationToolStripMenuItem.Enabled = true;
                }
                if (!setGSMScheduleToolStripMenuItem.Enabled && !scheduleReadingStatusToolStripMenuItem.Enabled && !scheduleReadingStatusToolStripMenuItem.Enabled && !assignGSMScheduleToolStripMenuItem.Enabled && !gSMSchedulingToolStripMenuItem.Enabled && !configurePortToolStripMenuItem.Enabled && !GSMCommToolStripMenuItem.Enabled && !MeterProgrammingToolStripMenuItem.Enabled && !readDataToolStripMenuItem.Enabled)
                    localCommunicationToolStripMenuItem.Enabled = false;
                else
                    localCommunicationToolStripMenuItem.Enabled = true;
                if ( !localCommunicationToolStripMenuItem.Enabled && !cMRISchedulingToolStripMenuItem.Enabled && !mRICommunicationToolStripMenuItem.Enabled)
                    dataAcquisitionToolStripMenuItem.Enabled = false;
                else
                {
                    dataAcquisitionToolStripMenuItem.Enabled = true;
                    
                }
            }
            LoadSearchData();

            gSMSchedulingToolStripMenuItem.Visible = false;
            assignGSMScheduleToolStripMenuItem.Visible = false;
            scheduleReadingStatusToolStripMenuItem.Visible = false;
            setGSMScheduleToolStripMenuItem.Visible = false;
            //reportsToolStripMenuItem.Enabled = true;
            //toolStripBtnReport.Enabled = true;
            //toolStripBtnMeterProgramming.Enabled = false;
            communication = new Communication(ConfigSettings.GetValue("PortName"),
                                              Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")),
                                              ConfigSettings.GetValue("ModePassword"));
        }



		private void listViewExplorer_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (oldIndex != e.ItemIndex)
			{
                if (oldIndex > e.ItemIndex)
                {
                    listViewExplorer.Items[e.ItemIndex].ImageIndex = 1;
                    listViewExplorer.Items[oldIndex].ImageIndex = 0;
                    oldIndex = e.ItemIndex;
                    return;
                }
				listViewExplorer.Items[oldIndex].ImageIndex = 0;
				oldIndex = e.ItemIndex;
			}
			listViewExplorer.Items[e.ItemIndex].ImageIndex = 1;
		}



		private void applicationLogDetailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (ActivateThisChild("LogDetails") == false)
            {
                LogDetails applicationLogDetails = new LogDetails();
                applicationLogDetails.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                applicationLogDetails.MdiParent = this;
                applicationLogDetails.Show();
            }
        }

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBCS aboutBCS = new AboutBCS();
			aboutBCS.ShowDialog();
			/*Commented by dhirendra on 15 march 10
		   PresentationLayer.WindowsForms.AboutBCS objAboutBCS = new CAB.UI.AboutBCS();
		   //objAboutBCS.MdiParent = this;
		   objAboutBCS.ShowDialog();
		   */
		}

		private void cOMPortSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("PortSettingForm") == false)
			{
				PortSettingForm portSettingForm = new PortSettingForm();
				portSettingForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				portSettingForm.MdiParent = this;
				portSettingForm.Show();
				this.statusBar.Text = this.StatusMessage;
			}
		}


		private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/* Commented by dhirendra on 15 march 10
			if (ActivateThisChild("ExportASCIIData") == false)
			{
				PresentationLayer.WindowsForms.ExportASCIIData objExportData = new CAB.UI.ExportASCIIData();
				objExportData.MdiParent = this;
				objExportData.Show();
			}
			*/
		}

		private void systemConfigurationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("GeneralConfiguration") == false)
			{
				GeneralConfiguration generalConfiguration = new GeneralConfiguration();
				generalConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				generalConfiguration.MdiParent = this;
				generalConfiguration.Show();
				this.statusBar.Text = this.StatusMessage;
			}
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/* Commented by dhirendra on 15 march 10
			if (ActivateThisChild("ExportSettings") == false)
			{
				PresentationLayer.WindowsForms.ExportSettings objExportSettings = new CAB.UI.ExportSettings();
				objExportSettings.MdiParent = this;
				objExportSettings.Show();
			}
			*/
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (ConfigInfo.UserInformationID == 0)
            {
                UpdateINI();
                MessageBox.Show("System will automatically restart now.","E-250 BCS",MessageBoxButtons.OK,MessageBoxIcon.Information);
              System.Diagnostics.Process.Start("ShutDown", "-r");
            }
            new ApplicationLogBLL().UpdateData();                     
			Application.Exit();
		}

		//private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
		//{
		//    if (ActivateThisChild("ImportSettings") == false)
		//    {
		//        PresentationLayer.WindowsForms.ImportSettings objImportSettings = new CAB.UI.ImportSettings();
		//        objImportSettings.MdiParent = this;
		//        objImportSettings.Show();
		//    }
		//}

		private void areaDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("CMRIMasterForm") == false)
			{
				CMRIMasterForm cmriMasterForm = new CMRIMasterForm();
				cmriMasterForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				cmriMasterForm.MdiParent = this;
				cmriMasterForm.Show();
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{ 
			Application.Exit();  
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            new ApplicationLogBLL().UpdateData();
            Application.Exit();
        }

		private void backupDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (ActivateThisChild("BackupFile") == false)
				{
					BackupFile backupFile = new BackupFile();
					backupFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
					backupFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
					backupFile.MdiParent = this;
					backupFile.Show();
				}
			}
			catch (Exception)
			{
			}
		}

		private void deleteDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("DeleteFile") == false)
			{
				DeleteFile deleteFile = new DeleteFile();
				deleteFile.MdiParent = this;
				deleteFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				deleteFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
				deleteFile.Show();
			}
		}

		private void restoreDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("RestoreFile") == false)
			{
				RestoreFile restoreFile = new RestoreFile();
				restoreFile.MdiParent = this;
				restoreFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				restoreFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
				restoreFile.Show();
			}
		}

		private void registerProductToolStripMenuItem_Click(object sender, EventArgs e)
		{
            //if (ActivateThisChild("RegisterProduct") == false)
            //{
				RegisterProduct registerProduct = new RegisterProduct();
				//registerProduct.MdiParent = this;
				//registerProduct.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				//registerProduct.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
				registerProduct.ShowDialog();
			//}
		}

		private void instantToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateWiseBetween datewisebetween = new DateWiseBetween();
			datewisebetween.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
			datewisebetween.ShowDialog();
		}

		private void generalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MeterWise meterWise = new MeterWise();
			meterWise.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
			meterWise.ShowDialog();
		}

		private void tamperToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/* Commented by dhirendra on 15 march 10
			if (ActivateThisChild("AnalysisReport") == false)
			{
				PresentationLayer.WindowsForms.ReportForms.AnalysisReport objAnalysisReport = new CAB.UI.ReportForms.AnalysisReport();
				objAnalysisReport.MdiParent = this;
				objAnalysisReport.Show();
			}
			*/
		}

		private void allDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CABFileWise lngFileWise = new CABFileWise();
			lngFileWise.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
			lngFileWise.ShowDialog();
		}

		private void groupDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("AreaMaster") == false)
			{
				AreaMaster areaMaster = new AreaMaster();
				areaMaster.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				areaMaster.MdiParent = this;
				areaMaster.Show();
			}
		}

		private void mRICommunicationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("CMRICommunication") == false)
			{
				CMRICommunication cmriCommunication = new CMRICommunication();
				cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                cmriCommunication.MdiParent = this;
				cmriCommunication.Show();
			}
            
		}

		private void readDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("MeterDataReadoutForm") == false)
			{
				MeterDataReadoutForm meterDataReadoutForm = new MeterDataReadoutForm();
				meterDataReadoutForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				meterDataReadoutForm.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                meterDataReadoutForm.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
				meterDataReadoutForm.MdiParent = this;
				meterDataReadoutForm.Show();
			}

            //if (ActivateThisChild("UploadFile") == false)
            //{
            //    UploadFile uploadFile = new UploadFile();
            //    uploadFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //    uploadFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
            //    uploadFile.MdiParent = this;
            //    uploadFile.Show();
            //}
		}
		
		private void toolStripBtnUser_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("UserInformations") == false)
			{
				UserInformations userInformations = new UserInformations();
				userInformations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				userInformations.MdiParent = this;
				userInformations.Show();
			}
		}

		private void toolStripBtnCMRICommunication_Click(object sender, EventArgs e)
		{
            toolStripStatusLabel.Text = "Checking For CMRI Type,Please Wait....";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), "300");
            this.Cursor = Cursors.Default;
            if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                if (ActivateThisChild("CMRICommunication") == false)
                {
                    toolStripStatusLabel.Text = "";
                    CMRICommunication cmriCommunication = new CMRICommunication();
                    cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    cmriCommunication.MdiParent = this;
                    cmriCommunication.Show();
                }
            }
            else
            {
                if (ActivateThisChild("E650CMRICommunication") == false)
                {
                    toolStripStatusLabel.Text = "";
                    E650CMRICommunication E650CMRICommunication = new E650CMRICommunication();
                    E650CMRICommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    E650CMRICommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    E650CMRICommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    E650CMRICommunication.MdiParent = this;
                    E650CMRICommunication.Show();
                }
            }
		}

		private void toolStripBtnConsumerMeter_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ConsumerMeterDefinition") == false)
			{
				ConsumerMeterDefinition consumerMeterDefinition = new ConsumerMeterDefinition();
				consumerMeterDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				consumerMeterDefinition.MdiParent = this;
				consumerMeterDefinition.Show();
			}
		}

		private void toolStripBtnSystemConfig_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("GeneralConfiguration") == false)
			{
				GeneralConfiguration generalConfiguration = new GeneralConfiguration();
				generalConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				generalConfiguration.MdiParent = this;
				generalConfiguration.Show();
				this.statusBar.Text = this.StatusMessage;
			}
		}

		private void toolStripBtnUploadCABFile_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("UploadFile") == false)
			{
				UploadFile uploadFile = new UploadFile();
				uploadFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				uploadFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
				uploadFile.MdiParent = this;
				uploadFile.Show();
			}
		}

		private void toolStripBtnReport_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
			{
				MessageBox.Show("Please select a CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
            //SelectDialog selectDialog = new SelectDialog();
          //  PhasorDiagram selectDialog = new PhasorDiagram();

            if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
            {
                SelectDialogTNEB selectDialog = new SelectDialogTNEB();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.ShowDialog();
            }
            if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            {
                SelectDialog selectDialog = new SelectDialog();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.ShowDialog();
            }

		}

		private void toolStripBtnMeterCommunication_Click(object sender, EventArgs e)
		{

            toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), "300");
            this.Cursor = Cursors.Default;
            if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                if (ActivateThisChild("MeterDataReadoutForm") == false)
                {
                    toolStripStatusLabel.Text = "";
                    MeterDataReadoutForm meterDataReadout = new MeterDataReadoutForm();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.MdiParent = this;
                    meterDataReadout.WindowState = FormWindowState.Maximized;
                    meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (ActivateThisChild("E650MeterDataReadout") == false)
                {
                    toolStripStatusLabel.Text = "";
                    E650MeterDataReadout meterDataReadout = new E650MeterDataReadout();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.MdiParent = this;
                    meterDataReadout.WindowState = FormWindowState.Maximized;
                    meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }


		}

		public Boolean ActivateThisChild(String formName)
		{
			this.StatusMessage = string.Empty;
			int i;
			Boolean formSetToMdi = false;
			for (i = 0; i < this.MdiChildren.Length; i++)
			{
				if (this.MdiChildren[i].Name == formName)
				{
					this.MdiChildren[i].Activate();
					this.MdiChildren[i].Visible = true;
					formSetToMdi = true;
				}
			}

			if (i == 0 || formSetToMdi == false)
				return false;
			else
				return true;
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
                Help.ShowHelp(this, "TNEB_BCS.chm");
			}
			catch (Exception Ex)
			{
			}
		}

		private void toolStripBtnMeterProgramming_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("MeterDataProgramming") == false)
			{
				MeterDataProgramming meterDataProgramming = new MeterDataProgramming();
				meterDataProgramming.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				meterDataProgramming.MdiParent = this;
				meterDataProgramming.Show();
			}
		}

		private void detailedReportToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select a CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
            {
                SelectDialogTNEB selectDialog = new SelectDialogTNEB();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.ShowDialog();
            }
            if (UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL || UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            {
                SelectDialog selectDialog = new SelectDialog();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.ShowDialog();
            }


		}

		private void MeterProgrammingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (ActivateThisChild("MeterConfiguration") == false)
            //{
            //    MeterProgramming meterProgramming = new MeterProgramming();
            //    meterProgramming.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //    meterProgramming.MdiParent = this;
            //    meterProgramming.Show();
            //}

            if (ActivateThisChild("MeterDataProgramming") == false)
            {
                MeterDataProgramming meterDataProgramming = new MeterDataProgramming();
                meterDataProgramming.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterDataProgramming.MdiParent = this;
                meterDataProgramming.Show();
            }
            //if (ActivateThisChild("MeterProgrammingConfigSettings") == false)
            //{
            //    MeterProgrammingConfigSettings meterProgrammingConfigSettings = new MeterProgrammingConfigSettings();
            //    meterProgrammingConfigSettings.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //    meterProgrammingConfigSettings.MdiParent = this;
            //    meterProgrammingConfigSettings.Show();
            //}
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
			{
				MessageBox.Show("Please select a CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DTMDailyProfileSelectForm dtmDailyProfileSelectForm = new DTMDailyProfileSelectForm();
			dtmDailyProfileSelectForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
			dtmDailyProfileSelectForm.ShowDialog();
		}

		private void GSMCommToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("GSMStatus") == false)
			{
				GSMStatus gSMStatus = new GSMStatus();
				gSMStatus.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gSMStatus.On_ConnectionTypeChanged += new IsConnectionTypeChanged(MainForm_OnconnectionTypeChanged);
				gSMStatus.MdiParent = this;
				gSMStatus.Show();
			}
		}

		private void ConsMeterDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ConsumerMeterDefinition") == false)
			{
				ConsumerMeterDefinition consumerMeterDefinition = new ConsumerMeterDefinition();
				consumerMeterDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				consumerMeterDefinition.MdiParent = this;
				consumerMeterDefinition.Show();
			}
		}

		public void MainForm_OnStatusChanged(string msg)
		{            
			this.toolStripStatusLabel.Text = msg;
		}
        public void MainForm_OnconnectionTypeChanged(string msg)
        {
            this.toolStripStatusLabel2.Text = msg;
        }
		private void MainForm_On_RightStatusChanged(string msg)
		{
			if (msg.Trim() == string.Empty)
				this.toolStripStatusLabel1.Text = MessageConstant.GetText("M000069");
			else
				this.toolStripStatusLabel1.Text = msg;
		}

        
        private void LoadSearchData()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            row = table.NewRow();
            row["DisplayMember"] = "Meter ID"; //"Meter Serial Number";
            row["ValueMember"] = "MSN";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Consumer ID"; //"Consumer Number";
            row["ValueMember"] = "CN";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "CMRI ID"; //"CMRI IF";
            row["ValueMember"] = "CMRIID";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Location";
            row["ValueMember"] = "L";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Reading Dates";
            row["ValueMember"] = "RD";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Date Range";
            row["ValueMember"] = "DR";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "ID & Date Range";
            row["ValueMember"] = "DRMID";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "CAB File";
            row["ValueMember"] = "CABF";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Week Number";
            row["ValueMember"] = "WN";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Month";
            row["ValueMember"] = "MN";
            table.Rows.Add(row);

            cboSearch.DataSource = table;
            cboSearch.DisplayMember = "DisplayMember";
            cboSearch.ValueMember = "ValueMember";
        }
        private void InitializeSearchControls(bool isDateRange,bool isDRMeterID,bool isWeekNumber,bool isMonthNumber)
        {
      
                frmCalendar.Visible =
                toCalendar.Visible =
                lblFrom.Visible = 
                lblTo.Visible =isDateRange;
                lblSMeterID.Visible =
                ddlMeterID.Visible = isDRMeterID;
                spnWeek.Visible = isWeekNumber;
                ddlMonth.Visible = isMonthNumber;        
                frmCalendar.Format = DateTimePickerFormat.Custom;
                toCalendar.Format = DateTimePickerFormat.Custom;
                frmCalendar.CustomFormat = ConfigInfo.DateFormat();
                toCalendar.CustomFormat = ConfigInfo.DateFormat();
           
                lblFrom.Text = "From : ";
                if (isDateRange)
                {
                    lngButton.Visible = true;
                    lngButton.Location = new Point(284, lngButton.Location.Y);
                    //searchControlPanel.Width = 400;
                    
                }
                if (isDRMeterID)
                {
                    lngButton.Location = new Point(originalButtonPosition, lngButton.Location.Y);
                    ddlMeterID.DataSource = new MeterDataBLL().GetAllReadMeters().Tables[0];
                    ddlMeterID.ValueMember = "MeterID";
                    ddlMeterID.DisplayMember = "MeterID";
                    //searchControlPanel.Width = 550;
                }
                if (isWeekNumber)
                { 
                  lblFrom.Text = "Week Number : ";
                  lngButton.Location = new Point(toCalendar.Location.X -29,lngButton.Location.Y);
                  lngButton.Visible = true;
                  lblFrom.Visible = true;
                  //searchControlPanel.Width = 245;
                }
                if (isMonthNumber)
                {
                    lblFrom.Text = "Month : ";
                    lngButton.Location = new Point(toCalendar.Location.X -67, lngButton.Location.Y);
                    lngButton.Visible = true;
                    lblFrom.Visible = true;
                    ddlMonth.DataSource = PrepareMonthTable();
                    ddlMonth.DisplayMember = "Month";
                    ddlMonth.ValueMember = "Value";
                    //searchControlPanel.Width = 204;
                    
                }
                 
                if (isMonthNumber || isDateRange || isDRMeterID || isWeekNumber)
                    searchControlPanel.Visible = true;
                else
                    searchControlPanel.Visible = false;
            
                
        }
        private DataTable PrepareMonthTable()
        {
               DataTable monthTable = new DataTable();
               monthTable.Columns.Add("Month");
               monthTable.Columns.Add("Value");
               DataRow row;
                row=monthTable.NewRow();
                row[0] = "Jan";
                row[1] = "1";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Feb";
                row[1] = "2";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Mar";
                row[1] = "3";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Apr";
                row[1] = "4";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "May";
                row[1] = "5";
                monthTable.Rows.Add(row);
                 row = monthTable.NewRow();
                row[0] = "Jun";
                row[1] = "6";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Jul";
                row[1] = "7";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Aug";
                row[1] = "8";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Sep";
                row[1] = "9";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Oct";
                row[1] = "10";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Nov";
                row[1] = "11";
                monthTable.Rows.Add(row);
                row = monthTable.NewRow();
                row[0] = "Dec";
                row[1] = "12";
                monthTable.Rows.Add(row);
                return monthTable;
        }
        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            oldIndex = 0;
            this.toolStripStatusLabel.Text = string.Empty;
            InitializeSearchControls(false, false,false,false);
            listViewExplorer.Items.Clear();
            string data = ((System.Data.DataRowView)(cboSearch.Items[cboSearch.SelectedIndex])).Row.ItemArray[1].ToString();
            listViewExplorer.ComboDataType = data;
            if (data.Equals("CABF"))
                listViewExplorer.ListData = new FileUploadMasterBLL().ComboList();
            else if (data.Equals("CN"))
                listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(true);
            else if (data.Equals("CMRIID"))
                listViewExplorer.ListData = new CMRIMasterBLL().ComboList();
            else if (data.Equals("L"))
                listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(false);
            else if (data.Equals("MSN"))
                listViewExplorer.ListData = new MeterDataBLL().ComboList(true);
            else if (data.Equals("DR"))
                InitializeSearchControls(true, false, false,false);
            else if (data.Equals("DRMID"))
                InitializeSearchControls(true, true, false, false);
            else if (data.Equals("WN"))
                InitializeSearchControls(false, false, true, false);
            else if (data.Equals("MN"))
                InitializeSearchControls(false, false, false, true);
            else if (data.Equals("RD"))
            {
                DataSet dataSet = new MeterDataBLL().ComboList(false);
                DataTable table = new DataTable();
                table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
                DataRow row;
                string dataDate1 = string.Empty;
                foreach (DataRow drow in dataSet.Tables[0].Rows)
                {
                    row = table.NewRow();
                    string dataDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[0]));
                    if (dataDate != "---------")
                    {
                        dataDate = dataDate.Substring(0, 10);
                        row["DisplayMember"] = dataDate;
                        if (dataDate1 == dataDate)
                            continue;
                        dataDate1 = dataDate;
                        bool exist = false;
                        foreach (DataRow tmpRow in table.Rows)
                        {
                            if (tmpRow["DisplayMember"].ToString().Equals(dataDate))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist)
                            table.Rows.Add(row);
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                listViewExplorer.ListData = ds;
            }
            else
                listViewExplorer.ListData = null;
        }

        //private void MainForm_OnListRefresh(bool flag)
        //{
        //    if (flag)
        //    {
        //        int index = 0;
        //        for (int i = 0; i < listViewExplorer.Items.Count; i++)
        //        {
        //            if (listViewExplorer.Items[i].Selected == true)
        //            {
        //                index = i;
        //                break;
        //            }
        //        }
        //        this.cboSearch_SelectedIndexChanged(this, null);
        //        if (listViewExplorer.Items.Count != 0)
        //            listViewExplorer.Items[index].Selected = true;
        //    }
        //}       
        private void MainForm_OnListRefresh(bool flag)
        {
            if (flag)
            {
                //If the thread is different from main thread, invoke required will be true
                if (listViewExplorer.InvokeRequired)
                {
                    //Call on UI thread
                    this.listViewExplorer.Invoke(new MethodInvoker(UpdateList));
                }
                else
                {
                    UpdateList();
                }
            }
        }
        /// <summary>
        /// Updates the list view
        /// </summary>
        private void UpdateList()
        {
            int index = 0;
            for (int i = 0; i < listViewExplorer.Items.Count; i++)
            {
                if (listViewExplorer.Items[i].Selected == true)
                {
                    index = i;
                    break;
                }
            }
            this.cboSearch_SelectedIndexChanged(this, null);
            if (listViewExplorer.Items.Count != 0)
                listViewExplorer.Items[index].Selected = true;
        }
        private void listViewExplorer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            long meterDataID = 0;
            int index = 0;
            for (int i = 0; i < listViewExplorer.Items.Count; i++)
            {
                if (listViewExplorer.Items[i].Selected == true)
                {
                    index = i;
                    break;
                }
            }
            data = listViewExplorer.Items[index].Text;
            string cboData = ((System.Data.DataRowView)(cboSearch.Items[cboSearch.SelectedIndex])).Row.ItemArray[1].ToString();
           

            if (cboData.Equals("CABF") || cboData.Equals("RD") || cboData.Equals("MSN")||cboData.Equals("CMRIID")||cboData.Equals("DR")||cboData.Equals("DRMID")||cboData.Equals("WN")||cboData.Equals("MN"))
            {
                if (cboData.Equals("CMRIID"))
                    //data = data.Substring(0,data.LastIndexOf("("));
                if(cboData.Equals("DRMID"))
                    data=data.Replace("(" + meterID + ")","");
                tmpData = new MeterDataBLL().ListDataSet(cboData, data);
               
                if (cboData.Equals("MSN")) { lblMeterIDVal.Text = listViewExplorer.SelectedItems[0].Text; }
                else { }
                if (meterConsumerFileList != null)
                    meterConsumerFileList.Close();
                if (ActivateThisChild("MeterFileList") == false)
                {
                    meterFileList = new MeterFileList();
                }
                else
                {
                    meterFileList.Close();
                    meterFileList = new MeterFileList();
                }
                meterFileList.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                CABGridControl gridControl = (CABGridControl)meterFileList.Controls["lngFileLists"] as CABGridControl;
                if(gridControl!=null)
                    gridControl.OnGridMouseDoubleClick += new CABGridControl.CABGridMouseDoubleClick(gridControl_OnGridMouseDoubleClick);
                meterFileList.MdiParent = this;
                meterFileList.ListData = data;
                meterFileList.ComboData = cboData;
                meterFileList.Show();
            }
            if (cboData.Equals("CN") || cboData.Equals("L"))
            {
                tmpData = new ConsumerMeterBLL().ListDataSet(cboData, data);
               
                if (meterFileList != null)
                    meterFileList.Close();
                if (ActivateThisChild("MeterConsumerFileList") == false)
                    meterConsumerFileList = new MeterConsumerFileList();
                else
                {
                    meterConsumerFileList.Close();
                    meterConsumerFileList = new MeterConsumerFileList();
                }
                meterConsumerFileList.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                CABGridControl consumerGridControl = (CABGridControl)meterConsumerFileList.Controls["lngFileLists"] as CABGridControl;
                if(consumerGridControl!=null)
                    consumerGridControl.OnGridMouseDoubleClick += new CABGridControl.CABGridMouseDoubleClick(consumerGridControl_OnGridMouseDoubleClick);
                meterConsumerFileList.MdiParent = this;
                meterConsumerFileList.ListData = data;
                meterConsumerFileList.ComboData = cboData;
                meterConsumerFileList.Show();
            }
            if (tmpData != null)
            {
                try
                {
                    if (tmpData.Tables[0].Rows.Count > 0)
                        meterDataID = Convert.ToInt64(tmpData.Tables[0].Rows[0]["MeterData_ID"]);
                }
                catch (Exception)
                {
                    string meterId = "0";
                    if (tmpData.Tables[0].Rows.Count > 0)
                        meterId = Convert.ToString(tmpData.Tables[0].Rows[0]["Meter_ID"]);
                    meterDataID = meterDataBLL.GetMeterData(meterId);
                }
            }
            FillGrid(meterDataID);
            this.statusBar.Text = this.StatusMessage;
        }

        void consumerGridControl_OnGridMouseDoubleClick(string KeyValue)
        {
            long meterDataID;
            if(long.TryParse(KeyValue,out meterDataID))
                 FillGrid(Convert.ToInt64(meterDataID));
        }

        void gridControl_OnGridMouseDoubleClick(string KeyValue)
        {

            long meterDataID;
            if (long.TryParse(KeyValue, out meterDataID))
                FillGrid(Convert.ToInt64(meterDataID));
        }

        private void FillGrid(long meterDataId)
        {
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            //addded on 29 feb 2012 as per the bug report
            int recordNo = meterDataBLL.FetchRecords(meterDataId);
            if (recordNo <= 0)
            {
                this.RightStatusMessage = "";
                return;
            }
            //long meterDataId = 0;

            //if (tmpData == null)
            //{
            //    meterDataId = 0;
            //    return;
            //}
            //if (tmpData.Tables.Count == 0)
            //{
            //    meterDataId = 0;
            //    return;
            //}
            //try
            //{
            //    if (tmpData.Tables[0].Rows.Count > 0)
            //        meterDataId = Convert.ToInt64(tmpData.Tables[0].Rows[0]["MeterData_ID"]);
            //}
            //catch (Exception)
            //{
            //    string meterId = "0";
            //    if (tmpData.Tables[0].Rows.Count > 0)
            //        meterId = Convert.ToString(tmpData.Tables[0].Rows[0]["Meter_ID"]);
            //    meterDataId = meterDataBLL.GetMeterData(meterId);
            //}
            // fetch cmri id ...
            DataSet cmriDataSet = meterDataBLL.GetCMRIDetails(meterDataId);
            // assign it to the label
            string CMRIType="";
            if ((cmriDataSet != null && cmriDataSet.Tables.Count != 0 && cmriDataSet.Tables[0].Rows.Count != 0))
                CMRIType=CheckValue(cmriDataSet.Tables[0].Rows[0]["CMRIType"].ToString());
            if (CMRIType != "--------")
                lblMRINumberVal.Text = CheckValue(cmriDataSet.Tables[0].Rows[0]["CMRIID"].ToString()) + " " + CheckValue(cmriDataSet.Tables[0].Rows[0]["CMRIType"].ToString());
            else
                lblMRINumberVal.Text = CheckValue(cmriDataSet.Tables[0].Rows[0]["CMRIID"].ToString());
            DataSet dataSet = meterDataBLL.GetConsumerMeterDetails(meterDataId);
            bool flag = false;
            if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                flag = true;

            if (flag)
            {
                lblMeterIDVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["MeterID"].ToString());
                lblConsumerIDVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Consumer_Number"].ToString());
                string val = Convert.ToString(dataSet.Tables[0].Rows[0]["Meter_AllocationDate"]);
                if (string.IsNullOrEmpty(val))
                    val = "0";
                val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(val));
                if (val != "---------")
                    val = val.Substring(0, 10);
                lblInstalledDateVal.Text = val;
                lblMeterTypeVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["MeterType_Name"].ToString());
                lblMeterModelVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["MeterModel_Name"].ToString());

                //lblMRINumberVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["CMRI_Number"].ToString());
              
                lblRegionVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Region_Name"].ToString());
                lblCircleVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Circle_Name"].ToString());
                lblDivisionVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Division_Name"].ToString());
                lblEMFVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Meter_EMF"].ToString());
                lblContractDemandVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Meter_ContractDemand"].ToString());
                val = Convert.ToString(dataSet.Tables[0].Rows[0]["Status"]);
                if (string.IsNullOrEmpty(val))
                    val = "0";
                lblActiveMeterVal.Text = (Convert.ToInt16(val) == 0) ? "No" : "Yes";

                int areaID = meterDataBLL.GetAreaIDFromMeterID(data);
                DataSet dSet = meterDataBLL.GetAreaDetails(areaID);

                if ((dSet != null) && (dSet.Tables.Count != 0) && (dSet.Tables[0].Rows.Count != 0))
                {
                    lblRegionVal.Text = CheckValue(dSet.Tables[0].Rows[0]["Region_Name"].ToString());
                    lblCircleVal.Text = CheckValue(dSet.Tables[0].Rows[0]["Circle_Name"].ToString());
                    lblDivisionVal.Text = CheckValue(dSet.Tables[0].Rows[0]["Division_Name"].ToString());
                }
            }
            else
            {
                DataSet meterIDDS = meterDataBLL.GetMeterIDFromMeterDataID(meterDataId);
                if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    lblMeterIDVal.Text = meterIDDS.Tables[0].Rows[0][0].ToString();

                lblActiveMeterVal.Text = "Inactive";
                lblConsumerIDVal.Text =
                lblInstalledDateVal.Text =
                lblMeterTypeVal.Text =
                lblMeterModelVal.Text =
               // lblMRINumberVal.Text =
                lblRegionVal.Text =
                lblCircleVal.Text =
                lblDivisionVal.Text =
                lblEMFVal.Text =
                lblContractDemandVal.Text = "--------";
            }
        }

        private string CheckValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "--------";
            else
                return value;
        }
	 
		private void exportDataToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ExportConsumerData") == false)
			{
				ExportConsumerData exportConsumerData = new ExportConsumerData();
				exportConsumerData.MdiParent = this;
				exportConsumerData.Show();
			}
		}
 
		private void cMRISchedulingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("CMRIScheduleFile") == false)
			{

                if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                {
                    CMRIScheduleFile cMRIScheduleFile = new CMRIScheduleFile();
                    cMRIScheduleFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    cMRIScheduleFile.MdiParent = this;
                    cMRIScheduleFile.Show();
                }
                else if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                {
                    CMRIScheduleFileNonTneb cMRIScheduleFile = new CMRIScheduleFileNonTneb();
                    cMRIScheduleFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    cMRIScheduleFile.MdiParent = this;
                    cMRIScheduleFile.Show();
                }
                else
                {
                    CMRIScheduleFileNonTneb cMRIScheduleFile = new CMRIScheduleFileNonTneb();
                    cMRIScheduleFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    cMRIScheduleFile.MdiParent = this;
                    cMRIScheduleFile.Show();
                }

			}
		}

		private void GroupDefinitionToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (ActivateThisChild("GroupDefinition") == false)
			{
				GroupDefinition groupDefinition = new GroupDefinition();
				groupDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				groupDefinition.MdiParent = this;
				groupDefinition.Show();
			}
		}

		private void exceptionLogDetailsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ExceptionLogDetails") == false)
			{
				ExceptionLogDetails exceptionLogDetails = new ExceptionLogDetails();
				exceptionLogDetails.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				exceptionLogDetails.MdiParent = this;
				exceptionLogDetails.Show();
			}
		}

		private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (ActivateThisChild("ConsumExportSettings") == false)
			{
				ConsumerExportSettings objExportSettings = new ConsumerExportSettings();
				objExportSettings.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				objExportSettings.MdiParent = this;
				objExportSettings.Show();
			}
		}

		private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ConsumerExport") == false)
			{
				ConsumerExport consumerExport = new ConsumerExport();
				consumerExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				consumerExport.MdiParent = this;
				consumerExport.Show();
			}
		}

		private void importToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (ActivateThisChild("ConsumerImport") == false)
			{
				ConsumerImport consumerImport = new ConsumerImport();
				consumerImport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				consumerImport.MdiParent = this;
				consumerImport.Show();
			}
		}

		private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (ActivateThisChild("ASCIIExportSettings") == false)
			{
				ASCIIExportSettings aSCIIExportSettings = new ASCIIExportSettings();
				aSCIIExportSettings.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
				aSCIIExportSettings.MdiParent = this;
				aSCIIExportSettings.Show();
			}
		}

        private void aSCIISettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ASCIIExportSettings") == false)
            {
                ASCIIExportSettings aSCIIExportSettings = new ASCIIExportSettings();
                aSCIIExportSettings.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                aSCIIExportSettings.MdiParent = this;
                aSCIIExportSettings.Show();
            }
        }

        private void aSCIIExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ASCIIExportFileWise") == false)
            {
                ASCIIExportFileWise aSCIIExportFileWise = new ASCIIExportFileWise();
                aSCIIExportFileWise.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                aSCIIExportFileWise.MdiParent = this;
                aSCIIExportFileWise.Show();
            }
        }

        private void aSCIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("StandardExport") == false)
            {
                StandardExport standardExport = new StandardExport();
                standardExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                standardExport.MdiParent = this;
                standardExport.Show();
            }
        }

        private void standaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("StandardImport") == false)
            {
                StandardImport standardImport = new StandardImport();
                standardImport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                standardImport.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                standardImport.MdiParent = this;
                standardImport.Show();
            }
        }

        private void gSMSchedulingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMScheduling") == false)
            {
                GSMScheduling gsmScheduling = new GSMScheduling();
                gsmScheduling.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gsmScheduling.MdiParent = this;
                gsmScheduling.Show();
            } 
        }
      
        private void assignGSMScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMGrouping") == false)
            {
                GSMGrouping gsmGrouping = new GSMGrouping();
                gsmGrouping.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gsmGrouping.MdiParent = this;
                gsmGrouping.Show();
            }  
        }

        private void configurePortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMConfigurePort") == false)
            {
                GSMConfigurePort gsmConfigurePort = new GSMConfigurePort();
                gsmConfigurePort.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gsmConfigurePort.MdiParent = this;
                gsmConfigurePort.Show();
            } 
        }

        public void DisplayMainStatus()
        {
            try
            {
                string Communication = string.Empty;
                string Company_Profile = string.Empty;
                string filename = AppDomain.CurrentDomain.BaseDirectory + @"\CABApplication.exe";
                string strbuilton = "Built On: " + System.IO.File.GetCreationTime(filename).ToShortDateString().ToString(); 
                MyCrypro objcrypto = new MyCrypro();
                Communication = "Communication Settings : " + ConfigInfo.GetPortName() + ", " + ConfigInfo.GetBaudRate();
                string strpath = AppDomain.CurrentDomain.BaseDirectory + "CABApplication.exe";
                Company_Profile = objcrypto.CopyRightsDetail() + "   " + Application.ProductName.ToString() + " Version  " + objcrypto.ProductVersion();
                //lblHeader.Text = Company_Profile+ "  "+ Communication;  
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            DisplayMainStatus();
        }

        private void setGSMScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMScheduleActivation") == false)
            {
                GSMScheduleActivation gsmScheduleActivation = new GSMScheduleActivation();
                gsmScheduleActivation.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gsmScheduleActivation.MdiParent = this;
                gsmScheduleActivation.Show();
            } 
        }

        private void scheduleReadingStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMReadingStatus") == false)
            {
                GSMReadingStatus gSMReadingStatus = new GSMReadingStatus();
                gSMReadingStatus.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gSMReadingStatus.MdiParent = this;
                gSMReadingStatus.Show();
            } 
        }

        public void UpdateINI()
        {
            try
            {
                string filePath = "";
                string fileContent="";
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    filePath = drive.Name + @"Program Files\MySQL\MySQL Server 5.1\my.ini";
                    try
                    {
                        StreamReader streamReader = new StreamReader(filePath);
                        fileContent = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                    catch (Exception)
                    { }
                    if (string.IsNullOrEmpty(fileContent))
                        continue;
                    else
                        break;
                }
                fileContent = fileContent.Replace("max_allowed_packet = 2G","");
                fileContent = fileContent + "\n" + "max_allowed_packet = 2G";
                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.Write(fileContent);
                streamWriter.Close();
            }
            catch (IOException)
            {
            }
        }

        private void loadSuveyConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (ActivateThisChild("LoadSurveyConfiguration") == false)
            //{
            //    LoadSurveyConfiguration loadSurveyConfiguration = new LoadSurveyConfiguration();
            //    loadSurveyConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //    loadSurveyConfiguration.MdiParent = this;
            //    loadSurveyConfiguration.Show();
            //} 
        }

        //private void dateSpecificLoadSurveyToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
        //    {
        //        MessageBox.Show("Please select a CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    DaySpecificLoadSurvey daySpecificLoadSurvey = new DaySpecificLoadSurvey();
        //    daySpecificLoadSurvey.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
        //    daySpecificLoadSurvey.ShowDialog();
        //}

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmCalendar_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lngButton_Click(object sender, EventArgs e)
        {
            DateTime systemDate;
            DateTime lastDateofYear;
            int weekNo=0;
            int monthNo=0;
            systemDate = DateTime.Now;
            int year = systemDate.Year;
            systemDate = Convert.ToDateTime(string.Concat("01/01/", year.ToString()));
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            lastDateofYear = systemDate.AddDays((ciCurr.Calendar.GetDaysInYear(year)-1));
            weekNo = (int)spnWeek.Value;
            monthNo = Convert.ToInt32(ddlMonth.SelectedValue);      
            DateTime FromDateTime = DateTime.Now;
            DateTime toDateTime = DateTime.Now;
             CABGridControl consumerGridControl =null;
            if (cboSearch.SelectedValue.Equals("DR")||cboSearch.SelectedValue.Equals("DRMID"))
            { 
              FromDateTime = frmCalendar.Value;
              toDateTime = toCalendar.Value;
            }
            else if (cboSearch.SelectedValue.Equals("WN"))
            {
                FromDateTime = ciCurr.Calendar.AddWeeks(systemDate, weekNo - 1);
                if (FromDateTime > lastDateofYear)
                    FromDateTime = lastDateofYear;
                toDateTime = FromDateTime.AddDays(6);
                if (toDateTime > lastDateofYear)
                    toDateTime = lastDateofYear;
                
            }
            else if (cboSearch.SelectedValue.Equals("MN"))
            {
                FromDateTime = ciCurr.Calendar.AddMonths(systemDate, monthNo - 1);
                toDateTime = FromDateTime.AddDays(ciCurr.Calendar.GetDaysInMonth(year, monthNo)-1);
            }


            if (cboSearch.SelectedValue.ToString().Equals("DRMID") && ddlMeterID.Items.Count > 0) 
                 meterID = ddlMeterID.SelectedValue.ToString();
            //Fix defct #163208
            if (!ValidateDates(FromDateTime, toDateTime)) 
            {
                return;
            }
             DataSet dataSet = null;
             if (cboSearch.SelectedValue.Equals("DR") || cboSearch.SelectedValue.Equals("WN") || cboSearch.SelectedValue.Equals("MN"))
            {
                dataSet = new MeterDataBLL().ComboListForDateRange(FromDateTime, toDateTime);
            }
            else if (cboSearch.SelectedValue.Equals("DRMID"))
            {
                dataSet = new MeterDataBLL().ComboListForDateRangeandMeterID(FromDateTime,toDateTime, meterID);
            }

             this.toolStripStatusLabel.Text = string.Empty;
             if (dataSet != null)
             {

                 if (dataSet.Tables[0].Rows.Count <= 0)
                 {
                     this.toolStripStatusLabel.Text = "No data found";
                    
                     
                     if (meterFileList != null)
                     {
                         consumerGridControl = (CABGridControl)meterFileList.Controls["lngFileLists"] as CABGridControl;
                         meterFileList.lblText.Text = "";
                     }
                     if (meterConsumerFileList != null)
                     {
                         consumerGridControl = (CABGridControl)meterConsumerFileList.Controls["lngFileLists"] as CABGridControl;
                     }
                     if (consumerGridControl != null)
                     {
                         consumerGridControl.Data = null;
                     }
                 }
                 else
                 {
                     if (meterConsumerFileList != null)
                         meterConsumerFileList.Hide();
                     if (meterFileList != null)
                         meterFileList.Hide();
                  
                 }
             }
             else
             {
                 this.toolStripStatusLabel.Text = "No data found";
                 meterFileList.lblText.Text = "";
                 if (meterFileList != null)
                 {
                     if (meterFileList != null)
                     {
                         consumerGridControl = (CABGridControl)meterFileList.Controls["lngFileLists"] as CABGridControl;
                     }
                     if (meterConsumerFileList != null)
                     {
                         consumerGridControl = (CABGridControl)meterConsumerFileList.Controls["lngFileLists"] as CABGridControl;
                     }
                     if (consumerGridControl != null)
                     {
                         consumerGridControl.Data = null;
                     }
                 }
             }
            listViewExplorer.ListData = dataSet;

           
        }
        //Fix defct #163208
        private bool ValidateDates(DateTime frmDateTime, DateTime toDateTime)
        {
            if (frmDateTime > toDateTime)
            {
                this.toolStripStatusLabel.Text = "From Date can not be greater than To Date";
                return false;
            }
            return true;
        }

      

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
           // ConnectionMode = (ConfigInfo.ChannelType == CAB.IECFramework.ChannelType.GSM) ? CAB.IECFramework.ChannelType.GSM.ToString() : "Normal"; 
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? "Reader(MR)" : "Master(US)";
            ConnectionMode = (ConfigInfo.ChannelType == CAB.IECFramework.ChannelType.GSM) ? CAB.IECFramework.ChannelType.GSM.ToString() : "Normal"
                             + "  Port: " + ConfigSettings.GetValue("PortName") + "  Mode: " + mode;  
        }

        private void spnWeek_KeyUp(object sender, KeyEventArgs e)
        {
            if (spnWeek.Value.ToString().Length >= 0)
            {
                if (spnWeek.Value > 53)
                    spnWeek.Value = 53;
            }
        }

        private void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void searchControlPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localCommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void spnWeek_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblFrom_Click(object sender, EventArgs e)
        {

        }

        private void lblDivisionVal_Click(object sender, EventArgs e)
        {

        }

        private void lblEMFVal_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblContractDemandVal_Click(object sender, EventArgs e)
        {

        }

        private void meterConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("MeterConfigurations") == false)
            {
                MeterConfigurations meterConfigurations = new MeterConfigurations();
                meterConfigurations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterConfigurations.MdiParent = this;
                meterConfigurations.Show();
            }
        }

        private void dataAcquisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GSMCommToolStripMenuItem.Enabled = true;
            //MeterProgrammingToolStripMenuItem.Enabled = false;
        }

        // For Meter Accuracy Check 
        private void MeterAccuracyChecktoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("MeterAccuracyCheck") == false)
            {
                MeterAccuracyCheck MeterAccuracyCheck = new MeterAccuracyCheck();
                MeterAccuracyCheck.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                MeterAccuracyCheck.MdiParent = this;
                MeterAccuracyCheck.Show();

               
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }
        private void CreateLogFile()
        {
            string fileName = string.Empty;
            string[] parameters = new string[]{"FileUpload_ID","UploadingDateTime","FileName","Status"};
            FileStream fileStream;
            try
            {
                if (File.Exists(fileName))
                {
                    fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                }


                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    foreach(string parameter in parameters)
                    sw.WriteLine(parameter.ToString() + ",") ;
                    sw.Close();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("FileImport") == false)
            {
                FileImport fileImport = new FileImport();
                fileImport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                fileImport.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                fileImport.MdiParent = this;
                fileImport.Show();
            }
        }

        private void lnklblFiles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void meterConfigurationtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            toolStripStatusLabel1.Text = string.Empty;
            if (ActivateThisChild("E650MeterDataReadout") == false)
            {
                E650MeterDataReadout meterDataReadout = new E650MeterDataReadout();
                meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterDataReadout.MdiParent = this;
                meterDataReadout.WindowState = FormWindowState.Maximized;
                meterDataReadout.Show();
            }
        }

        /// <summary>
        /// Open WCM meter configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCMMeterConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("MeterConfigurations") == false)
            {
                MeterConfigurations meterConfigurations = new MeterConfigurations();
                meterConfigurations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterConfigurations.MdiParent = this;
                meterConfigurations.Show();
            }
        }
        /// <summary>
        /// Open LT Meter Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LTMeterConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("E650MeterConfigurations") == false)
            {
                E650MeterConfigurations meterConfigurations = new E650MeterConfigurations();
                meterConfigurations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterConfigurations.MdiParent = this;
                meterConfigurations.Show();
            }
        }

        private void meterConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void E650MeterAccuracyCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("MeterAccuracyCheck") == false)
            {
                MeterAccuracyCheck MeterAccuracyCheck = new MeterAccuracyCheck();
                MeterAccuracyCheck.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                MeterAccuracyCheck.MdiParent = this;
                MeterAccuracyCheck.Show();


            }
        }

        private void E650GSMConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("GSMStatus") == false)
            {
                GSMStatus gSMStatus = new GSMStatus();
                gSMStatus.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gSMStatus.On_ConnectionTypeChanged += new IsConnectionTypeChanged(MainForm_OnconnectionTypeChanged);
                gSMStatus.MdiParent = this;
                gSMStatus.Show();
            }
        }

        private void E650CMRICommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("E650CMRICommunication") == false)
            {
                E650CMRICommunication cmriCommunication = new E650CMRICommunication();
                cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                cmriCommunication.MdiParent = this;
                cmriCommunication.Show();
            }
        }

        private void cMRICommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("CMRICommunication") == false)
            {
                CMRICommunication cmriCommunication = new CMRICommunication();
                cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                cmriCommunication.MdiParent = this;
                cmriCommunication.Show();
            }
        }

        private void systemSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("SystemSettings") == false)
            {
                E650Settings cmriCommunication = new E650Settings();
                cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                cmriCommunication.MdiParent = this;
                cmriCommunication.Show();
            }
        }

        private void e650LTMeterCommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// OPen Upload file window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadCABFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("UploadFile") == false)
            {
                UploadFile uploadFile = new UploadFile();
                uploadFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                uploadFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                uploadFile.MdiParent = this;
                uploadFile.Show();
            }
        }

        /// <summary>
        /// Meter readout menu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void e650MeterReadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            toolStripStatusLabel1.Text = string.Empty;
            if (ActivateThisChild("E650MeterDataReadout") == false)
            {
                E650MeterDataReadout meterDataReadout = new E650MeterDataReadout();
                meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterDataReadout.MdiParent = this;
                meterDataReadout.WindowState = FormWindowState.Maximized;
                meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                meterDataReadout.Show();
            }

        }
        /// <summary>
        /// Check for meter type 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemMeterReadout_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), "300");
            this.Cursor = Cursors.Default;
            if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                if (ActivateThisChild("MeterDataReadoutForm") == false)
                {
                    toolStripStatusLabel.Text = "";
                    MeterDataReadoutForm meterDataReadout = new MeterDataReadoutForm();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.MdiParent = this;
                    meterDataReadout.WindowState = FormWindowState.Maximized;
                    meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (ActivateThisChild("E650MeterDataReadout") == false)
                {
                    toolStripStatusLabel.Text = "";
                    E650MeterDataReadout meterDataReadout = new E650MeterDataReadout();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.MdiParent = this;
                    meterDataReadout.WindowState = FormWindowState.Maximized;
                    meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            
        }
        /// <summary>
        /// Open CMRI Communication Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCMRICommunication_Click(object sender, EventArgs e)
        {            

            toolStripStatusLabel.Text = "Checking For CMRI Type,Please Wait....";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), "300");
            this.Cursor = Cursors.Default;
            if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                if (ActivateThisChild("CMRICommunication") == false)
                {
                    toolStripStatusLabel.Text = "";
                    CMRICommunication cmriCommunication = new CMRICommunication();
                    cmriCommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    cmriCommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    cmriCommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    cmriCommunication.MdiParent = this;
                    cmriCommunication.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (ActivateThisChild("E650CMRICommunication") == false)
                {
                    toolStripStatusLabel.Text = "";
                    E650CMRICommunication E650CMRICommunication = new E650CMRICommunication();
                    E650CMRICommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    E650CMRICommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    E650CMRICommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    E650CMRICommunication.MdiParent = this;
                    E650CMRICommunication.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
           }
        }

        private void toolStripMenuItemMeterAccuracy_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), "300");
            this.Cursor = Cursors.Default;
            if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                if (ActivateThisChild("MeterAccuracyCheck") == false)
                {
                    toolStripStatusLabel.Text = "";
                    MeterAccuracyCheck MeterAccuracyCheck = new MeterAccuracyCheck();
                    MeterAccuracyCheck.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    MeterAccuracyCheck.MdiParent = this;
                    MeterAccuracyCheck.Show(); 


                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (ActivateThisChild("E650MeterAccuracyCheck") == false)
                {
                    toolStripStatusLabel.Text = "";
                    E650MeterAccuracyCheck e650MeterAccuracyCheck = new E650MeterAccuracyCheck();
                    e650MeterAccuracyCheck.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    e650MeterAccuracyCheck.MdiParent = this;
                    e650MeterAccuracyCheck.Show(); 


                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
        }               

        
        //private void phasorReportToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
        //    {
        //        MessageBox.Show("Please select a CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    PhasorDiagramForm selectDialog = new PhasorDiagramForm();
        //    selectDialog.MeterDataID = ConfigInfo.ActiveMeterDataId;
        //    selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
        //    selectDialog.ShowDialog();
        //}
        
        
	}
}
