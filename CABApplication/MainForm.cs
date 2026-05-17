using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using Common.EntityMapper;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication;
using System.Text;
using CABApplication.Reports.Forms;
using CABApplication.Scheduling;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Security.Principal;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class MainForm : CABForm
    {
        System.Timers.Timer _timer;
        IList<UserRightEntity> userRightEntities = null;
        private int oldIndex;
        private MeterFileList meterFileList;
        private MeterConsumerFileList meterConsumerFileList;
        private UserRightBLL userRightBLL = new UserRightBLL();
        string data = string.Empty;
        bool isPUMA = false;
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        private Communication communication;
        private const string ReadoutFailure = "Readout Failure.";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MainForm).ToString());
        private CommunicationType commType;
        public int securitymachanism = 0;
        public MainForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
        }
        /// <summary>
        /// set connected/not connected , protocol and mode , for the operations performed in this form.
        /// </summary>
        public string ConnectionMode
        {
            set
            {

                this.toolStripStatusLabel2.Text = string.Concat("Connection: ", value.ToString());
            }
        }


        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("UserInformations") == false)
            {
                UserInformations userInformations = new UserInformations();
                userInformations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                userInformations.MdiParent = this;
                userInformations.WindowState = FormWindowState.Maximized;
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
                changePassword.WindowState = FormWindowState.Maximized;
                changePassword.Show();
            }
        }

        private void ShowIcon(bool flag)
        {
            try
            {
                tsbGSM.ToolTipText = "";
                ServiceController controller = new ServiceController();
                controller.MachineName = ".";
                controller.ServiceName = "GSMService";
                tsbGSM.ToolTipText = "Service " + controller.Status.ToString();
                if (tsbGSM.ToolTipText.Trim().Contains("Stopped"))
                    tsbGSM.Visible = false;
                else
                    tsbGSM.Visible = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                tsbGSM.Visible = false;
                logger.Log(LOGLEVELS.Error, "ShowIcon(bool flag)", ex);
            }
        }
        [Conditional("DEBUG")]
        private void ShowDebugOptions()
        {
            // tsmDebug.Visible = true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowDebugOptions();
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? "Reader(MR)" : "Master(US)";
            //ConnectionMode = (ConfigInfo.ChannelType == CAB.Framework.ChannelType.GSM) ? CAB.Framework.ChannelType.GSM.ToString() : "Normal"
            //                 + "  Port: " + ConfigSettings.GetValue("PortName") + "  Mode: " + mode;   

            ConnectionMode = "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

            //if (UtilityDetails.PrimaryUtlityName == UtilityEntity.Generic.ToString())
            //{
            isPUMA = true;
            //snapReadToolStripMenuItem.Visible = false;
            //}
            //if (UtilityDetails.IECSupport)
            //{
            //    iECToolStripMenuItem.Visible = true;
            //}
            /* VBM - Disable Midnighjt data tab as requested by amitesh date 4/5/2013 for all utilities */
            //if (!(UtilityDetails.GetUtilityDetails() == UtilityEntity.MVVNL))
            //{
            //midNightDataToolStripMenuItem.Visible = false;

            //}
            /* VBM - Disable Midnighjt data tab as requested by amitesh date 4/5/2013 for all utilities */
            //if (!UtilityDetails.ShowEnhancedTamperReportMenu)
            //{
            //    tamperToolStripMenuItem.Visible = false;
            //}
            //BhardwajG : No need to show cdf links if it is not enabled for utility
            //if (!UtilityDetails.ShowCDFConverter)
            //{
            //    cDFToolStripMenuItem.Visible = false;
            //}

            //If GPRS Service is enabled for Utility then start the GPRS Communication Service
            //if (UtilityDetails.PrimaryUtlityName == UtilityEntity.DLMS.ToString())
            //{

            StartGPRSService();
            //}
            //else
            //{
            //    StopGPRSService();
            //    gPRSStatusReportToolStripMenuItem.Visible = false;
            //}
            this.ShowIcon(true);

            LoginForm obj = (LoginForm)Application.OpenForms["LoginForm"];
            if (obj != null)
            {
                if (obj.lbl_ShowDemo.Text == "label1")
                {
                    //registerProductToolStripMenuItem.Visible = false;
                }
            }
            this.Text = "Cabcon Edge Solution";
            ModuleMasterBLL moduleBLL = new ModuleMasterBLL();
            if (ConfigInfo.UserInformationID == 0)
            {
                string rootId = ConfigInfo.RootLog();
                if (rootId == "0")
                    ConfigSettings.ChangeNode("RootLog", "1");
                else
                    //{
                    //    MessageBox.Show("Please contact Vendor. Your Session has Expired", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Application.Exit();
                    //}
                    UpdateINI();
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
            {
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                meterModelBLL.InsertMeterModelNumber();
                //}
                //else
                //{
                //    meterModelBLL.InsertDefaultData();
                //}
            }

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
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.DLMS_LTCT_650))
            {
                toolStripBtnCMRICommunication.Visible =
                toolStripBtnMeterCommunication.Visible =
                toolStripBtnMeterProgramming.Visible =
                toolStripBtnSystemConfig.Visible =
                exceptionLogDetailsToolStripMenuItem.Visible =
                activityLogDetailsToolStripMenuItem.Visible =
                    // mRICommunicationToolStripMenuItem.Visible =
                    //cMRISchedulingToolStripMenuItem.Visible =
                mnuMeterCommunication.Visible = false;
                //configurationToolStripMenuItem.Visible = false;
                //DailyProfiletoolStripMenuItem.Visible = allDataToolStripMenuItem.Visible = false;
            }
            else
            {
                dLMSCommunicationToolStripMenuItem.Visible = false;
            }
            if (ConfigInfo.UserInformationID == 0)
            {
                userGroupToolStripMenuItem.Enabled = toolStripBtnUser.Enabled = true;
                dataAcquisitionToolStripMenuItem.Enabled =
                toolStripBtnConsumerMeter.Enabled =
                toolStripBtnCMRICommunication.Enabled =
                toolStripBtnMeterCommunication.Enabled =
                toolStripBtnMeterProgramming.Enabled =
                toolStripBtnUploadCABFile.Enabled =
                toolStripBtnSystemConfig.Enabled =
                toolStripBtnReport.Enabled =

                changePasswordToolStripMenuItem.Visible =
                applicationLogDetailsToolStripMenuItem.Enabled =
                exceptionLogDetailsToolStripMenuItem.Enabled =
                activityLogDetailsToolStripMenuItem.Enabled =
                definitionToolStripMenuItem.Enabled =
                dataAcquisitionToolStripMenuItem.Enabled =
                scheduleToolStripMenuItem.Enabled =
                configurationToolStripMenuItem.Enabled =
                dataArchiveToolStripMenuItem.Enabled =
                exportImportToolStripMenuItem.Enabled =
                reportsToolStripMenuItem.Enabled = false;


            }
            else
            {
                ////changePasswordToolStripMenuItem.Visible = true;
                userRightBLL = new UserRightBLL();

                bool userAdministrator = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "User Administrator");
                userManagementToolStripMenuItem.Enabled = applicationLogDetailsToolStripMenuItem.Enabled = exceptionLogDetailsToolStripMenuItem.Enabled =
                toolStripBtnUser.Enabled = userAdministrator;
                bool definition = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Definition");
                definitionToolStripMenuItem.Enabled = toolStripBtnConsumerMeter.Enabled = definition;
                bool cmriDownload = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Schedule");
                mRICommunicationToolStripMenuItem.Enabled = scheduleToolStripMenuItem.Enabled = cMRISchedulingToolStripMenuItem.Enabled =
                configurationToolStripMenuItem.Enabled = toolStripBtnSystemConfig.Enabled = toolStripBtnUploadCABFile.Enabled =
                toolStripBtnCMRICommunication.Enabled = cmriDownload;

                bool programming = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Programming");
                MeterProgrammingToolStripMenuItem.Enabled = mnuMeterCommunication.Enabled = meterConfigurationsToolStripMenuItem.Enabled =
                toolStripBtnMeterProgramming.Enabled = programming;

                bool dataReadout = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Readout");
                //mnuMeterCommunication.Enabled = mnuGSMScheduling.Enabled = readDataToolStripMenuItem.Enabled = GSMCommToolStripMenuItem.Enabled =
                //gSMSchedulingToolStripMenuItem.Enabled = groupWiseGSMSchedulingToolStripMenuItem.Enabled = readingGSMTaskManagerToolStripMenuItem.Enabled
                //= activateDeactivateGSMScheduleToolStripMenuItem.Enabled = toolStripBtnMeterCommunication.Enabled = mRICommunicationToolStripMenuItem.Enabled = dataReadout;

                readToolStripMenuItem.Enabled = dynamicReadoutToolStripMenuItem.Enabled = dataReadout;


                exportImportToolStripMenuItem.Enabled = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Export/Import");

                bool reportsView = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Reports View");
                reportsToolStripMenuItem.Enabled = toolStripBtnReport.Enabled = listViewExplorer.Enabled = cboSearch.Enabled = reportsView;
                dataArchiveToolStripMenuItem.Enabled = userRightBLL.CheckPermission(ConfigInfo.UserInformationID, "Data Archive");

                if (!cmriDownload && !programming && !dataReadout)
                    uploadCABFileToolStripMenuItem.Enabled = toolStripBtnUploadCABFile.Enabled = false;
                else
                    uploadCABFileToolStripMenuItem.Enabled = toolStripBtnUploadCABFile.Enabled = true;
                if (!MeterProgrammingToolStripMenuItem.Enabled && !readDataToolStripMenuItem.Enabled && !GSMCommToolStripMenuItem.Enabled)
                    mnuMeterCommunication.Enabled = false;
                else
                    mnuMeterCommunication.Enabled = true;
                if (!gSMSchedulingToolStripMenuItem.Enabled && !groupWiseGSMSchedulingToolStripMenuItem.Enabled && !readingGSMTaskManagerToolStripMenuItem.Enabled && !activateDeactivateGSMScheduleToolStripMenuItem.Enabled)
                    mnuGSMScheduling.Enabled = false;
                else
                    mnuGSMScheduling.Enabled = true;


                if (!uploadCABFileToolStripMenuItem.Enabled && !mnuGSMScheduling.Enabled && !mnuMeterCommunication.Enabled && !cMRISchedulingToolStripMenuItem.Enabled && !mRICommunicationToolStripMenuItem.Enabled)
                {
                    dataAcquisitionToolStripMenuItem.Enabled = false;
                }
                else
                    dataAcquisitionToolStripMenuItem.Enabled = true;

                if (programming && !userAdministrator && !definition && !cmriDownload && !dataReadout && !reportsView && !dataArchiveToolStripMenuItem.Enabled)
                {
                    dataAcquisitionToolStripMenuItem.Enabled = true;
                    dynamicReadoutToolStripMenuItem.Enabled = false;
                    readToolStripMenuItem.Enabled = false;
                    uploadCABFileToolStripMenuItem.Enabled = false;
                    settingsToolStripMenuItem1.Enabled = true;
                }

                if (cmriDownload && !userAdministrator && !programming && !dataReadout && !reportsView && !dataArchiveToolStripMenuItem.Enabled)
                {
                    scheduleToolStripMenuItem.Enabled = true;
                    cMRISchedulerToolStripMenuItem.Enabled = true;
                    meterDataSchedulerToolStripMenuItem.Enabled = true;
                    toolStripMenuItem1.Enabled = true;
                    groupWiseGSMSchedulingToolStripMenuItem.Enabled = true;
                    readingGSMTaskManagerToolStripMenuItem.Enabled = true;
                    if (definition)
                    {
                        definitionToolStripMenuItem.Enabled = true;
                    }

                    dataAcquisitionToolStripMenuItem.Enabled = false;
                    dataArchiveToolStripMenuItem.Enabled = false;
                    reportsToolStripMenuItem.Enabled = false;
                    exportImportToolStripMenuItem.Enabled = false;
                }
            }
            meterTypeConfigurationToolStripMenuItem.Visible = false;
            LoadSearchData();
            ApplyModernTheme();
            //StartService();
            //_timer = new System.Timers.Timer(5 * 60 * 1000);
            //_timer.Enabled = true;
            //_timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // MODERN 2026 UI THEME
        // ═══════════════════════════════════════════════════════════════════════

        private static readonly Color BrandBlue      = Color.FromArgb( 29,  70, 150);
        private static readonly Color BrandBlueDark  = Color.FromArgb( 18,  50, 115);
        private static readonly Color BrandBlueLight = Color.FromArgb( 50,  95, 175);
        private static readonly Color MenuHover      = Color.FromArgb( 50, 100, 190);
        private static readonly Color PanelBg        = Color.FromArgb(245, 248, 255);
        private static readonly Color StatusBg       = Color.FromArgb( 22,  42,  90);

        private void ApplyModernTheme()
        {
            try
            {
                // ── Menu bar ─────────────────────────────────────────────────
                menuStripMainForm.BackColor = BrandBlue;
                menuStripMainForm.ForeColor = Color.White;
                menuStripMainForm.Renderer  = new ModernMenuRenderer();
                menuStripMainForm.Font      = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                menuStripMainForm.Padding   = new Padding(4, 0, 0, 0);

                foreach (ToolStripMenuItem item in menuStripMainForm.Items.OfType<ToolStripMenuItem>())
                {
                    // Top-level items: white text on brand blue
                    item.ForeColor = Color.White;
                    item.BackColor = BrandBlue;
                    item.Padding   = new Padding(10, 6, 10, 6);

                    // Make the dropdown panel itself white
                    item.DropDown.BackColor = Color.White;

                    StyleDropDown(item);
                }

                // ── Toolbar ──────────────────────────────────────────────────
                toolStripMainForm.BackColor  = BrandBlueDark;
                toolStripMainForm.ForeColor  = Color.White;
                toolStripMainForm.Renderer   = new ModernToolStripRenderer();
                toolStripMainForm.GripStyle  = ToolStripGripStyle.Hidden;
                toolStripMainForm.Padding    = new Padding(6, 2, 6, 2);

                // ── Left side panel ──────────────────────────────────────────
                if (panelListViewContainer != null)
                {
                    panelListViewContainer.BackColor = PanelBg;
                    panelListViewContainer.Padding   = new Padding(4);
                }
                if (panelSearchBoxContainer != null)
                {
                    panelSearchBoxContainer.BackColor = PanelBg;
                    panelSearchBoxContainer.Padding   = new Padding(4, 4, 4, 4);
                }

                // ── Search & Dropdown Controls ────────────────────────────────
                if (cboSearch != null)
                {
                    cboSearch.FlatStyle = FlatStyle.Flat;
                    cboSearch.Font = new Font("Segoe UI", 9.5f);
                    cboSearch.BackColor = Color.White;
                    cboSearch.ForeColor = Color.FromArgb(40, 40, 40);
                }
                if (txtSearch != null)
                {
                    txtSearch.BorderStyle = BorderStyle.FixedSingle;
                    txtSearch.Font = new Font("Segoe UI", 9.5f);
                    txtSearch.ForeColor = txtSearch.Text == "Search..." ? Color.Gray : Color.FromArgb(40, 40, 40);
                }
                if (lngLabel1 != null)
                {
                    lngLabel1.Font = new Font("Segoe UI Semibold", 9.5f);
                    lngLabel1.ForeColor = BrandBlueDark;
                }

                // ── Status bar ───────────────────────────────────────────────
                statusBar.BackColor  = StatusBg;
                statusBar.ForeColor  = Color.FromArgb(200, 225, 255);
                statusBar.SizingGrip = false;
                statusBar.Font       = new Font("Segoe UI", 9f);
                foreach (ToolStripStatusLabel lbl in statusBar.Items.OfType<ToolStripStatusLabel>())
                    lbl.ForeColor = Color.FromArgb(200, 225, 255);
            }
            catch { }
        }

        private static void StyleDropDown(ToolStripMenuItem parent)
        {
            foreach (ToolStripItem sub in parent.DropDownItems)
            {
                if (sub is ToolStripSeparator sep)
                {
                    sep.BackColor = Color.White;
                    continue;
                }

                if (sub is ToolStripMenuItem mi)
                {
                    // Dropdown items: white background, dark readable text
                    mi.BackColor = Color.White;
                    mi.ForeColor = Color.FromArgb(25, 50, 120);
                    mi.Font      = new Font("Segoe UI", 9.25f);

                    // Recurse: set sub-panel background too
                    mi.DropDown.BackColor = Color.White;
                    StyleDropDown(mi);
                }
            }
        }

        // ── Custom renderer: removes all gradients from the menu bar ──────────
        private class ModernMenuRenderer : ToolStripProfessionalRenderer
        {
            public ModernMenuRenderer() : base(new ModernColorTable()) { RoundedEdges = false; }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.IsOnDropDown)
                {
                    // Dropdown items: white bg, highlight on hover
                    Color fill = e.Item.Selected
                        ? Color.FromArgb(229, 239, 255)
                        : Color.White;
                    using (var brush = new System.Drawing.SolidBrush(fill))
                        e.Graphics.FillRectangle(brush,
                            new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                }
                else
                {
                    // Top-level items on the menu bar
                    Color fill = e.Item.Selected || (e.Item as ToolStripMenuItem)?.Pressed == true
                        ? Color.FromArgb(50, 100, 190)
                        : Color.FromArgb(29, 70, 150);
                    using (var brush = new System.Drawing.SolidBrush(fill))
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                }
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                Color bg = e.ToolStrip.IsDropDown
                    ? Color.White
                    : Color.FromArgb(29, 70, 150);
                using (var brush = new System.Drawing.SolidBrush(bg))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (e.ToolStrip.IsDropDown)
                {
                    // Draw a thin brand-blue border around the dropdown
                    using (var pen = new System.Drawing.Pen(Color.FromArgb(180, 205, 245)))
                        e.Graphics.DrawRectangle(pen,
                            new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1));
                }
            }
        }

        // ── Custom renderer for the icon toolbar ──────────────────────────────
        private class ModernToolStripRenderer : ToolStripProfessionalRenderer
        {
            public ModernToolStripRenderer() : base(new ModernColorTable()) { RoundedEdges = false; }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                using (var brush = new System.Drawing.SolidBrush(Color.FromArgb(18, 50, 115)))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }

            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected || e.Item.Pressed)
                {
                    using (var brush = new System.Drawing.SolidBrush(Color.FromArgb(50, 100, 190)))
                        e.Graphics.FillRectangle(brush,
                            new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                }
            }
        }

        // ── Shared color table (no gradients anywhere) ────────────────────────
        private class ModernColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin        => Color.FromArgb( 29,  70, 150);
            public override Color MenuStripGradientEnd          => Color.FromArgb( 29,  70, 150);
            public override Color MenuItemSelected              => Color.FromArgb(229, 239, 255);
            public override Color MenuItemBorder                => Color.FromArgb(180, 210, 245);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(229, 239, 255);
            public override Color MenuItemSelectedGradientEnd   => Color.FromArgb(229, 239, 255);
            public override Color MenuItemPressedGradientBegin  => Color.FromArgb( 18,  50, 115);
            public override Color MenuItemPressedGradientEnd    => Color.FromArgb( 18,  50, 115);
            public override Color ToolStripDropDownBackground   => Color.White;
            public override Color ImageMarginGradientBegin      => Color.FromArgb(235, 242, 255);
            public override Color ImageMarginGradientMiddle     => Color.FromArgb(235, 242, 255);
            public override Color ImageMarginGradientEnd        => Color.FromArgb(235, 242, 255);
            public override Color SeparatorDark                 => Color.FromArgb(200, 215, 245);
            public override Color SeparatorLight                => Color.FromArgb(240, 245, 255);
        }



        /// <summary>
        /// Method starts the GPRS service if GPRS is enabled for utility
        /// </summary>
        private void StartGPRSService()
        {
            try
            {
                ServiceController controller = new ServiceController("LandisGyr.BCS.GPRSCommService", ".");
                switch (controller.Status)
                {
                    case ServiceControllerStatus.ContinuePending:
                    case ServiceControllerStatus.PausePending:
                    case ServiceControllerStatus.Running:
                    case ServiceControllerStatus.StartPending:
                    case ServiceControllerStatus.StopPending:
                        break;
                    case ServiceControllerStatus.Paused:
                    case ServiceControllerStatus.Stopped:
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, System.TimeSpan.FromSeconds(30));
                        break;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "StartGPRSService()", ex);
            }
        }

        /// <summary>
        /// Method stops the service if GPRS is not enabled for utility.
        /// </summary>
        private void StopGPRSService()
        {
            try
            {
                ServiceController controller = new ServiceController("LandisGyr.BCS.GPRSCommService", ".");
                switch (controller.Status)
                {
                    case ServiceControllerStatus.Running:
                        controller.Stop();
                        break;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "StopGPRSService()", ex);
            }
        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            StartService();
            _timer.Start();
        }
        private void StartService()
        {
            try
            {
                ServiceController controller = new ServiceController("GSM Communication");
                if (controller != null)
                {
                    if (controller.Status == ServiceControllerStatus.Stopped || controller.Status == ServiceControllerStatus.StopPending)
                    {
                        toolStripButton1.Visible = true;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //toolStripButton1.Visible = true;
                logger.Log(LOGLEVELS.Error, "StartService()", ex);
            }
        }
        private void listViewExplorer_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            for (int i = 0; i < listViewExplorer.Length; i++)
            {
                if (i == e.ItemIndex)
                {
                    listViewExplorer.Items[e.ItemIndex].ImageIndex = 1;
                    oldIndex = e.ItemIndex;
                }
                else
                    listViewExplorer.Items[i].ImageIndex = 0;
            }
        }

        private void applicationLogDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("LogDetails") == false)
            {
                LogDetails applicationLogDetails = new LogDetails();
                applicationLogDetails.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                applicationLogDetails.MdiParent = this;
                applicationLogDetails.WindowState = FormWindowState.Maximized;
                applicationLogDetails.Show();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBCS aboutBCS = new AboutBCS();
            aboutBCS.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfigInfo.UserInformationID == 0)
            {
                MessageBox.Show("System will automatically restart now.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("ShutDown", "-r");
            }
            new ApplicationLogBLL().UpdateData();
            Process[] processes = Process.GetProcessesByName("DLMS_Final");
            foreach (Process p in processes)
            {
                p.CloseMainWindow();
            }
            Application.Exit();
            Application.Exit();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new ApplicationLogBLL().UpdateData();
            Process[] processes = Process.GetProcessesByName("DLMS_Final");
            foreach (Process p in processes)
            {
                p.CloseMainWindow();
            }
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            new ApplicationLogBLL().UpdateData();
            Process[] processes = Process.GetProcessesByName("DLMS_Final");
            foreach (Process p in processes)
            {
                p.CloseMainWindow();
            }
            Application.Exit();
        }

        private void uploadCABFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CABForm uploadFile = new CABForm();
            ApplicationType types = ConfigInfo.GetApplicationType();

            if ((ActivateThisChild("DLMS650UploadFile") == false) && (types.Equals(ApplicationType.DLMS_LTCT_650)))
            {
                uploadFile = new DLMS650UploadFile();
                uploadFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                uploadFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                uploadFile.MdiParent = this;
                uploadFile.WindowState = FormWindowState.Maximized;
                uploadFile.Show();
            }
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
                    backupFile.WindowState = FormWindowState.Maximized;
                    backupFile.Show();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "backupDataToolStripMenuItem_Click(object sender, EventArgs e)", ex);
            }
        }

        private void deleteDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("DeleteFile") == false)
            {
                DeleteFile deleteFile = new DeleteFile();
                deleteFile.MdiParent = this;
                deleteFile.WindowState = FormWindowState.Maximized;
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
                restoreFile.WindowState = FormWindowState.Maximized;
                restoreFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                restoreFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                restoreFile.Show();
            }
        }

        public bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin = false;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "IsUserAdministrator()", ex);
            }
            return isAdmin;
        }


        private void registerProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //To check User is Administrator or Not
            if (!IsUserAdministrator())
            {
                //No Administrator Rights
                MessageBox.Show(CABApplication.Properties.Resources.RUNASADMIN, CABApplication.Properties.Resources.BCS);
                return;
            }

            RegisterProduct registerProduct = new RegisterProduct();
            registerProduct.ImposeProceedFunctionality();            
        }

        private void instantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateWiseBetween datewisebetween = new DateWiseBetween();
            datewisebetween.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            datewisebetween.ShowDialog();
        }
        //ashish
        private void successReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GsmSuccess gsmSuccess = new GsmSuccess();
            gsmSuccess.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            gsmSuccess.ShowDialog();
        }
        //ashish
        private void schedulesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleReportForm scheduleReport = new ScheduleReportForm();
            scheduleReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            scheduleReport.ShowDialog();
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeterWise meterWise = new MeterWise();
            meterWise.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            meterWise.ShowDialog();
        }

        private void toolStripBtnUser_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("UserInformations") == false)
            {
                UserInformations userInformations = new UserInformations();
                userInformations.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                userInformations.MdiParent = this;
                userInformations.WindowState = FormWindowState.Maximized;
                userInformations.Show();
            }
        }

        private void toolStripBtnConsumerMeter_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ConsumerMeterDefinition") == false)
            {
                ConsumerMeterDefinition consumerMeterDefinition = new ConsumerMeterDefinition();
                consumerMeterDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                consumerMeterDefinition.MdiParent = this;
                consumerMeterDefinition.WindowState = FormWindowState.Maximized;
                consumerMeterDefinition.Show();
            }
        }

        private void toolStripBtnUploadCABFile_Click(object sender, EventArgs e)
        {
            CABForm uploadFile = new CABForm();
            ApplicationType types = ConfigInfo.GetApplicationType();

            if ((ActivateThisChild("DLMS650UploadFile") == false) && (types.Equals(ApplicationType.DLMS_LTCT_650)))
            {
                uploadFile = new DLMS650UploadFile();
                uploadFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                uploadFile.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                uploadFile.MdiParent = this;
                uploadFile.WindowState = FormWindowState.Maximized;
                uploadFile.Show();
            }
        }

        private void toolStripBtnReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select CAB file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SelectDialog selectDialog = new SelectDialog();
            selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            selectDialog.ShowDialog();
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
                    this.MdiChildren[i].WindowState = FormWindowState.Maximized;
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
                Help.ShowHelp(this, "E650_DLMS_BCS.chm");
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "contentsToolStripMenuItem_Click(object sender, EventArgs e)", Ex);
            }
        }

        // SB code change Start - 20180629 - Multiple Analysis View
        public void ShowReport(object sender, EventArgs e)
        {
            detailedReportToolStripMenuItem_Click(sender, e);
        }

        public void ShowBillingReportReport(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }

        public void ShowTamperReport(object sender, EventArgs e)
        {
            tamperToolStripMenuItem_Click(sender, e);
        }

        public void ShowLoadSurveyReportFileWise(object sender, EventArgs e)
        {
            fileWiseReportToolStripMenuItem_Click(sender, e);
        }

        public void ShowLoadSurveyReportMeterIDWise(object sender, EventArgs e)
        {
            meterIDWiseReportToolStripMenuItem_Click(sender, e);
        }

        public void ShowLoadSurveyReport(object sender, EventArgs e)
        {
            loadSurveyToolStripMenuItem_Click(sender, e);
        }
        
        public void ShowLoadSwitchReport(object sender, EventArgs e)
        {
            toolStripMenuItem3_Click(sender, e);
        }

        public void ShowMidnightReportFileWise(object sender, EventArgs e)
        {
            toolStripFileWise_Click(sender, e);
        }

        public void ShowMidnightReportMeterIDWise(object sender, EventArgs e)
        {
            toolStripMeterIDWise_Click(sender, e);
        }

        public void ShowMidnightReport(object sender, EventArgs e)
        {
            midNightDataToolStripMenuItem_Click(sender, e);
        }
        
        // SB code change End - 20180629 - Multiple Analysis View

        private void detailedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // SB code change Start - 20180629 - Multiple Analysis View
            //SelectDialog selectDialog = new SelectDialog();
            //selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //selectDialog.ShowDialog();

            SelectDialog selectDialog = (SelectDialog)Application.OpenForms["SelectDialog"];

            if (selectDialog == null)
            {
                selectDialog = new SelectDialog();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.Show();
            }
            else
            {
                selectDialog.Close();
                selectDialog = new SelectDialog();
                selectDialog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                selectDialog.Show();
                //selectDialog.Activate();
            }
            
            // SB code change End - 20180629 - Multiple Analysis View
        }

        private void ConsMeterDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ConsumerMeterDefinition") == false)
            {
                ConsumerMeterDefinition consumerMeterDefinition = new ConsumerMeterDefinition();
                consumerMeterDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                consumerMeterDefinition.MdiParent = this;
                consumerMeterDefinition.WindowState = FormWindowState.Maximized;
                consumerMeterDefinition.Show();
            }
        }

        private void MainForm_OnStatusChanged(string msg)
        {
            this.toolStripStatusLabel.Text = msg;
        }

        private void MainForm_OnStatusChangedLabel2(string msg)
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
            row["DisplayMember"] = "Location";
            row["ValueMember"] = "L";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "Reading Dates";
            row["ValueMember"] = "RD";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "File Wise";
            row["ValueMember"] = "CABF";
            table.Rows.Add(row);
            row = table.NewRow();
            row["DisplayMember"] = "CMRI ID";
            row["ValueMember"] = "CMRI";
            table.Rows.Add(row);
            cboSearch.DataSource = table;
            cboSearch.DisplayMember = "DisplayMember";
            cboSearch.ValueMember = "ValueMember";

        }

        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewExplorer.Items.Clear();
            string data = ((System.Data.DataRowView)(cboSearch.Items[cboSearch.SelectedIndex])).Row.ItemArray[1].ToString();
            listViewExplorer.ComboDataType = data;
            // SB Code Change Start 20171121 - Search
            if (data.Equals("CABF"))
            {
                ShowDTPicker(data);             //SarkarA code change 20180220 // implement to and from dates search
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    listViewExplorer.ListData = new FileUploadMasterBLL().ComboList();
                }
                else
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else if (data.Equals("CN"))
            {
                ShowDTPicker(data);             //SarkarA code change 20180220 // implement to and from dates search
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(true);
                }
                else
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else if (data.Equals("L"))
            {
                ShowDTPicker(data);             //SarkarA code change 20180220 // implement to and from dates search
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(false);
                }
                else
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else if (data.Equals("MSN"))
            {
                ShowDTPicker(data);             //SarkarA code change 20180220 // implement to and from dates search
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    listViewExplorer.ListData = new MeterDataBLL().ComboList("MeterId");
                }
                else
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else if (data.Equals("CMRI"))
            {
                ShowDTPicker(data);             //SarkarA code change 20180220 // implement to and from dates search
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    listViewExplorer.ListData = GetCMRIListData();
                }
                else
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else if (data.Equals("RD"))
            {

                ShowDTPicker(data);             //SarkarA code change start 20180220 // implement to and from dates search
               
                if (txtSearch.Text.Trim().Equals(string.Empty) || txtSearch.Text.Trim().ToLower().Equals("search..."))
                {
                    DataSet dataSet = new MeterDataBLL().ComboList("ReadingDateTime");
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
                {
                    txtMeterId_TextChanged(sender, e);
                }
            }
            else
            {
                listViewExplorer.ListData = null;
            }
            // SB Code Change End 20171121 - Search
            // To enable disable control of meterfilelist
            EnableDisableSearchControls(data);

            //else if (data.Equals("W"))
            //{
            //    int wCount = 1;

            //    DataSet dataSet = new DataSet();
            //    DataTable table = new DataTable();
            //    table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            //    while (wCount <= 53)
            //    {
            //        DataRow row;
            //        string dataDate1 = string.Empty;
            //        row = table.NewRow();

            //        row["DisplayMember"] = "Week" + (wCount++).ToString("00");
            //        table.Rows.Add(row);
            //    }
            //    dataSet.Tables.Add(table);
            //    listViewExplorer.ListData = dataSet;
            //}
            //else if (data.Equals("M"))
            //{
            //    DataSet dataSet = new DataSet();
            //    DataTable table = new DataTable();
            //    table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            //    DataRow row1 = table.NewRow();
            //    row1["DisplayMember"] = "JAN";
            //    table.Rows.Add(row1);
            //    DataRow row2 = table.NewRow();
            //    row2["DisplayMember"] = "FEB";
            //    table.Rows.Add(row2);
            //    DataRow row3 = table.NewRow();
            //    row3["DisplayMember"] = "MAR";
            //    table.Rows.Add(row3);
            //    DataRow row4 = table.NewRow();
            //    row4["DisplayMember"] = "APR";
            //    table.Rows.Add(row4);
            //    DataRow row5 = table.NewRow();
            //    row5["DisplayMember"] = "MAY";
            //    table.Rows.Add(row5);
            //    DataRow row6 = table.NewRow();
            //    row6["DisplayMember"] = "JUN";
            //    table.Rows.Add(row6);
            //    DataRow row7 = table.NewRow();
            //    row7["DisplayMember"] = "JAN";
            //    table.Rows.Add(row7);
            //    DataRow row8 = table.NewRow();
            //    row8["DisplayMember"] = "JAN";
            //    table.Rows.Add(row8);
            //    DataRow row9 = table.NewRow();
            //    row9["DisplayMember"] = "JAN";
            //    table.Rows.Add(row9);
            //    DataRow row10 = table.NewRow();
            //    row10["DisplayMember"] = "JAN";
            //    table.Rows.Add(row10);
            //    DataRow row11 = table.NewRow();
            //    row11["DisplayMember"] = "JAN";
            //    table.Rows.Add(row11);
            //    dataSet.Tables.Add(table);
            //    listViewExplorer.ListData = dataSet;
            //}
        


        }

        private DataSet GetCMRIListData()
        {
            DataSet dsAuto = new MeterDataBLL().ComboList("CMRI_Number");
            DataSet dsManual = new CMRIMasterBLL().ComboList();
            DataSet dsCMRIList = new DataSet();

            dsAuto.Merge(dsManual);

            DataTable temp = dsAuto.Tables[0].DefaultView.ToTable(true, "CMRI_Number");

            dsCMRIList.Tables.Add(temp);

            return dsCMRIList;
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
            DataSet tmpData = null;

            if (cboData.Equals("CABF") || cboData.Equals("RD") || cboData.Equals("MSN") || cboData.Equals("CMRI"))
            {
                if (cboData.Equals("CMRI"))
                    tmpData = new CMRIMasterBLL().ListDataSet(data);
                else
                    tmpData = new MeterDataBLL().ListDataSet(cboData, data, false);
                FillGrid(tmpData);
                if (cboData.Equals("MSN")) { lblMeterIDVal.Text = listViewExplorer.SelectedItems[0].Text; }

                if (meterConsumerFileList != null)
                    meterConsumerFileList.Close();
                if (ActivateThisChild("MeterFileList") == false)
                {
                    meterFileList = new MeterFileList();
                }
                else
                {
                    if (meterFileList != null)
                        meterFileList.Close();
                    meterFileList = new MeterFileList();
                }
                meterFileList.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterFileList.MdiParent = this;
                meterFileList.WindowState = FormWindowState.Maximized;
                meterFileList.ListData = data;
                meterFileList.ComboData = cboData;

                EnableDisableSearchControls(cboData);
                meterFileList.Show();
            }
            if (cboData.Equals("CN") || cboData.Equals("L"))
            {
                tmpData = new ConsumerMeterBLL().ListDataSet(cboData, data);
                FillGrid(tmpData);
                if (meterFileList != null)
                    meterFileList.Close();
                if (ActivateThisChild("MeterConsumerFileList") == false)
                    meterConsumerFileList = new MeterConsumerFileList();
                else
                {
                    if (meterConsumerFileList != null)
                        meterConsumerFileList.Close();
                    meterConsumerFileList = new MeterConsumerFileList();
                }
                meterConsumerFileList.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                meterConsumerFileList.MdiParent = this;
                meterConsumerFileList.WindowState = FormWindowState.Maximized;
                meterConsumerFileList.ListData = data;
                meterConsumerFileList.ComboData = cboData;
                meterConsumerFileList.Show();
            }
            this.statusBar.Text = this.StatusMessage;
        }

        public void FillGrid(DataSet tmpData)
        {
            string emfApplied = string.Empty;
            //decimal actualEMF = 0;
            //int internalCTRatio = 0;
            //int internalPTRatio = 0;
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            long meterDataId = 0;

            if (tmpData == null)
            {
                meterDataId = 0;
                return;
            }
            if (tmpData.Tables.Count == 0)
            {
                meterDataId = 0;
                return;
            }
            try
            {
                if (tmpData.Tables[0].Rows.Count > 0)
                    meterDataId = Convert.ToInt64(tmpData.Tables[0].Rows[0]["MeterData_ID"]);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                string meterId = "0";
                if (tmpData.Tables[0].Rows.Count > 0)
                    meterId = Convert.ToString(tmpData.Tables[0].Rows[0]["Meter_ID"]);
                meterDataId = meterDataBLL.GetMeterData(meterId);
                logger.Log(LOGLEVELS.Error, "FillGrid(DataSet tmpData)", ex);
            }
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
                lblMRINumberVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["CMRI_Number"].ToString());
                lblRegionVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Region_Name"].ToString());
                lblCircleVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Circle_Name"].ToString());
                lblDivisionVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Division_Name"].ToString());
                lblEMFVal.Text = CheckValue(dataSet.Tables[0].Rows[0]["Meter_EMF"].ToString());
                /*GKG EMF Bug */
                //if (int.TryParse(dataSet.Tables[0].Rows[0]["internalCTRatio"].ToString(), out internalCTRatio) && int.TryParse(dataSet.Tables[0].Rows[0]["internalPTRatio"].ToString(), out internalPTRatio))
                //{
                //    //if (internalCTRatio <= 0)
                //    //{
                //    //    internalCTRatio = 1;
                //    //}
                //    //if (internalPTRatio <= 0)
                //    //{
                //    //    internalPTRatio = 1;
                //    //}
                //}
                //if (internalCTRatio <= 0)
                //{
                //    internalCTRatio = 1;
                //}
                //if (internalPTRatio <= 0)
                //{
                //    internalPTRatio = 1;
                //}
                /*GKG EMF Bug */

                /*VBM EMF Bug */

                lblEMFVal.Text = CommonBLL.CalculateActualEMF(Convert.ToDecimal(lblEMFVal.Text),
                                                               dataSet.Tables[0].Rows[0]["internalCTRatio"].ToString(),
                                                               dataSet.Tables[0].Rows[0]["internalPTRatio"].ToString());
                /*VBM EMF Bug */

                emfApplied = dataSet.Tables[0].Rows[0]["UseEMFInCalculations"].ToString();
                if (emfApplied.ToString() == "1")
                {
                    emfApplied = APPLIED;
                }
                else
                {
                    emfApplied = NOTAPPLIED;
                }
                lblEMFVal.Text = lblEMFVal.Text + " (" + emfApplied + ")";

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

                lblActiveMeterVal.Text = "InActive";
                lblConsumerIDVal.Text =
                lblInstalledDateVal.Text =
                lblMeterTypeVal.Text =
                lblMeterModelVal.Text =
                lblMRINumberVal.Text =
                lblRegionVal.Text =
                lblCircleVal.Text =
                lblDivisionVal.Text =
                lblEMFVal.Text =
                lblContractDemandVal.Text = "--------";
            }
        }
        public void ClearGrid()
        {
            lblMeterIDVal.Text =
            lblActiveMeterVal.Text =
            lblConsumerIDVal.Text =
            lblInstalledDateVal.Text =
            lblMeterTypeVal.Text =
            lblMeterModelVal.Text =
            lblMRINumberVal.Text =
            lblRegionVal.Text =
            lblCircleVal.Text =
            lblDivisionVal.Text =
            lblEMFVal.Text =
            lblContractDemandVal.Text = "--------";
        }
        private string CheckValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "--------";
            else
                return value;
        }

        private void GroupDefinitionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (ActivateThisChild("GroupDefinition") == false)
            {
                GroupDefinition groupDefinition = new GroupDefinition();
                groupDefinition.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                groupDefinition.MdiParent = this;
                groupDefinition.WindowState = FormWindowState.Maximized;
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
                exceptionLogDetails.WindowState = FormWindowState.Maximized;
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
                objExportSettings.WindowState = FormWindowState.Maximized;
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
                consumerExport.WindowState = FormWindowState.Maximized;
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
                consumerImport.WindowState = FormWindowState.Maximized;
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
                aSCIIExportSettings.WindowState = FormWindowState.Maximized;
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
                aSCIIExportSettings.WindowState = FormWindowState.Maximized;
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
                aSCIIExportFileWise.WindowState = FormWindowState.Maximized;
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
                standardExport.WindowState = FormWindowState.Maximized;
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
                standardImport.WindowState = FormWindowState.Maximized;
                standardImport.Show();
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
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show("Setting Main Form Status Failed !" + "\r\n" + "\r\n" + Ex.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DisplayMainStatus()", Ex);
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            DisplayMainStatus();
            if (ConfigInfo.GetTenderType() != TenderType.JUSCO)
                mnuGSMScheduling.Visible = false;
        }

        public void UpdateINI()
        {
            try
            {
                string filePath = "";
                string fileContent = "";
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    filePath = drive.Name + @"Program Files\MySQL\MySQL Server 5.1\my.ini";
                    try
                    {
                        StreamReader streamReader = new StreamReader(filePath);
                        fileContent = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {
                        logger.Log(LOGLEVELS.Error, "UpdateINI()", ex);
                    }
                    if (string.IsNullOrEmpty(fileContent))
                        continue;
                    else
                        break;
                }
                fileContent = fileContent + "\n" + "max_allowed_packet = 1G";
                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.Write(fileContent);
                streamWriter.Close();
            }
            catch (IOException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateINI()", ex);
            }
        }

        private void activityLogDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ApplicationLogDetails") == false)
            {
                ApplicationLogDetails applicationLogDetails = new ApplicationLogDetails();
                applicationLogDetails.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                applicationLogDetails.MdiParent = this;
                applicationLogDetails.WindowState = FormWindowState.Maximized;
                applicationLogDetails.Show();
            }
        }

        private void meterTypeConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("AppInfo") == false)
            {
                AppInfo appInfo = new AppInfo();
                appInfo.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                appInfo.MdiParent = this;
                appInfo.WindowState = FormWindowState.Maximized;
                appInfo.Show();
            }
        }

        private void dLMSCommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            // toolStripStatusLabel1.Text = string.Empty;
            if (ActivateThisChild("DLMSMain") == false)
            {
                //DLMSMain dlmsComMainForm = new DLMSMain();
                //dlmsComMainForm.MdiParent = this;
                //dlmsComMainForm.WindowState = FormWindowState.Maximized;
                //dlmsComMainForm.Show();
            }
        }

        //DS report is now open in File wise mode only as per decision taken by Balgovind because before in ID wise and File wise both the case same report is opening
        private void midNightDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["MidNightReportFileWise"] != null)
            {
                Application.OpenForms["MidNightReportFileWise"].Close();
            }

            if (ActivateThisChild("MidNightReportFileWise") == false)
            {
                MidNightReportFileWise midNightReport = new MidNightReportFileWise();
                midNightReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                midNightReport.ShowDialog();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }

        //LS report is now open in File wise mode only as per decision taken by Balgovind because before in ID wise and File wise both the case same report is opening
        private void loadSurveyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["LoadSurveyReportFileWise"] != null)
            {
                Application.OpenForms["LoadSurveyReportFileWise"].Close();
            }

            if (ActivateThisChild("LoadSurveyReportFileWise") == false)
            {
                LoadSurveyReportFileWise loadsurveyReport = new LoadSurveyReportFileWise();
                loadsurveyReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                loadsurveyReport.Show();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }
        /// <summary>
        /// Used to show new tamper report and cumulative tamper counter report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tamperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ActivateThisChild("TamperReport") == false)
            {
                // SB code change Start - 20180629 - Multiple Analysis View
                TamperReport tamperReport = null;

                if (Application.OpenForms["TamperReport"] != null)
                {
                    tamperReport = (TamperReport)Application.OpenForms["TamperReport"];
                    tamperReport.Close();
                    tamperReport = new TamperReport();
                    if (tamperReport.HasData)
                    {
                        tamperReport.Show();
                    }
                }
                else
                {
                    tamperReport = new TamperReport();
                    if (tamperReport.HasData)
                    {
                        tamperReport.Show();
                    }
                }

                //TamperReport tamperReport = new TamperReport();
                //if (tamperReport.HasData)
                //{
                //    tamperReport.ShowDialog();
                //}
                // SB code change End - 20180629 - Multiple Analysis View
            }
        }

        private void CMRIDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("CMRIMasterForm") == false)
            {
                CMRIMasterForm cmriMasterForm = new CMRIMasterForm();
                cmriMasterForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cmriMasterForm.MdiParent = this;
                cmriMasterForm.WindowState = FormWindowState.Maximized;
                cmriMasterForm.Show();
            }
        }

        private void MainForm_MdiChildActivate_1(object sender, EventArgs e)
        {
            this.toolStripStatusLabel.Text = string.Empty;
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? "Reader(MR)" : "Master(US)";
            ConnectionMode = "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

        }

        private void mnuGSMScheduling_Click(object sender, EventArgs e)
        {

        }

        private void gSMSchedulingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (ActivateThisChild("CreateGroups") == false)
            {
                frmGSMGrouping createGroups = new frmGSMGrouping();
                createGroups.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                createGroups.MdiParent = this;
                createGroups.WindowState = FormWindowState.Maximized;
                createGroups.Show();
            }
        }

        private void groupWiseGSMSchedulingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            // toolStripStatusLabel1.Text = string.Empty;
            if (ActivateThisChild("SchedularForm") == false)
            {
                SchedularForm taskSchedular = new SchedularForm();
                taskSchedular.MdiParent = this;
                taskSchedular.WindowState = FormWindowState.Maximized;
                taskSchedular.Show();
            }
        }

        private void readingGSMTaskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            // toolStripStatusLabel1.Text = string.Empty;
            if (ActivateThisChild("TaskManagerForm") == false)
            {
                TaskManagerForm taskManager = new TaskManagerForm();
                taskManager.MdiParent = this;
                taskManager.WindowState = FormWindowState.Maximized;
                taskManager.Show();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("ManageGroups") == false)
            {
                ManageGroups manageGroups = new ManageGroups();
                manageGroups.MdiParent = this;
                manageGroups.WindowState = FormWindowState.Maximized;
                manageGroups.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                manageGroups.Show();
            }
        }

        private void manageRegionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("RegionManager") == false)
            {
                RegionManager regionMaster = new RegionManager();
                regionMaster.MdiParent = this;
                regionMaster.WindowState = FormWindowState.Maximized;
                regionMaster.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                regionMaster.ShowPickButton = false;
                regionMaster.Show();
            }
        }

        private void manageCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("CircleManager") == false)
            {
                CircleManager circleMaster = new CircleManager();
                circleMaster.MdiParent = this;
                circleMaster.WindowState = FormWindowState.Maximized;
                circleMaster.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                circleMaster.ShowPickButton = false;
                circleMaster.Show();
            }
        }

        private void manageDivisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("DivisionManager") == false)
            {
                DivisionManager divisionMaster = new DivisionManager();
                divisionMaster.MdiParent = this;
                divisionMaster.WindowState = FormWindowState.Maximized;
                divisionMaster.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                divisionMaster.ShowPickButton = false;
                divisionMaster.Show();
            }
        }

        private void areaDefinitionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("AreaMaster") == false)
            {
                AreaMaster areaMaster = new AreaMaster();
                areaMaster.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                areaMaster.MdiParent = this;
                areaMaster.WindowState = FormWindowState.Maximized;
                areaMaster.Show();
            }
        }

        private void billingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("BillingReport") == false)
            {
                BillingReport billingReport = new BillingReport();
                billingReport.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController controller = new ServiceController("GSM Communication");
                if (controller != null)
                {
                    if (controller.Status == ServiceControllerStatus.Stopped || controller.Status == ServiceControllerStatus.StopPending)
                    {
                        controller.Start();
                        toolStripButton1.Visible = false;
                        this.StatusMessage = "Service started successfully.";
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("Exception occured while starting the service : " + ex.Message);
                logger.Log(LOGLEVELS.Error, "toolStripButton1_Click(object sender, EventArgs e)", ex);
            }
        }

        private void mdiTabStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            toolStripStatusLabel.Text = string.Empty;
            //toolStripStatusLabel1.Text = string.Empty;
        }



        private void cDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("CDFConverter") == false)
            {
                CDFConverter cdfConverter = new CDFConverter();
                cdfConverter.MdiParent = this;
                cdfConverter.WindowState = FormWindowState.Maximized;
                cdfConverter.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cdfConverter.Show();
            }
        }
        /// <summary>
        /// Opens IEC/NON DLMS Meter Configuration Window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wcmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("E650MeterConfigurations"))
            {
                E650MeterConfigurations e650MeterConfiguration = new E650MeterConfigurations(true);
                e650MeterConfiguration.MdiParent = this;
                e650MeterConfiguration.WindowState = FormWindowState.Maximized;
                e650MeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                e650MeterConfiguration.Show();
            }
        }
        /// <summary>
        /// Opens DLMS Meter Configuration Window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ltMeterConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("E650MeterConfigurations") == false)
            {
                E650MeterConfigurations e650MeterConfiguration = new E650MeterConfigurations(true);
                e650MeterConfiguration.MdiParent = this;
                e650MeterConfiguration.WindowState = FormWindowState.Maximized;
                e650MeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                e650MeterConfiguration.Show();
            }
        }
        /// <summary>
        /// Opens DLMS Meter Readout window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meterReadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //No need to check Meter type as remote one to one is only implmented for DLMS.
            if ((ConfigSettings.GetValue("ChannelType") != CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
            {
                if ((ConfigSettings.GetValue("MeterFirmwareType")).ToString() == "1" || (ConfigSettings.GetValue("MeterFirmwareType")).ToString() == "2")
                    OpenMeterDataReadoutForm(true, 0); // Story - 347720
                else if ((ConfigSettings.GetValue("MeterFirmwareType")).ToString() == "3")
                    OpenMeterDataReadoutForm(false, 2);
            }
            else
            {
                if (UtilityDetails.IECSupport)
                {
                    toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";

                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"),
                        ConfigSettings.GetValue("BaudRate"), Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                    this.Cursor = Cursors.Default;
                    if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
                    {
                        // Story - 347720 - To compare LGC in ASCII format
                        if (result.RecieveDataBuffer[1].ToString() == "76" && result.RecieveDataBuffer[2].ToString() == "71" && result.RecieveDataBuffer[3].ToString() == "67")
                        {
                            OpenMeterDataReadoutForm(false, 1);
                        }
                        else if (result.RecieveDataBuffer[1].ToString() == "88" && result.RecieveDataBuffer[2].ToString() == "88" && result.RecieveDataBuffer[3].ToString() == "88")
                        {
                            OpenMeterDataReadoutForm(false, 3);
                        }
                        else
                        {
                            toolStripStatusLabel.Text = "Sign-On failure.";
                        }
                    }
                    else if (result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                    {
                        OpenMeterDataReadoutForm(false, 2);
                    }
                    else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                    {
                        OpenMeterDataReadoutForm(true, 0); // Story - 347720
                    }
                    else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                    {
                        toolStripStatusLabel.Text = "Port Not Available.";
                    }
                    else
                    {
                        toolStripStatusLabel.Text = "Sign-On failure.";
                    }
                }
                else
                {
                    OpenMeterDataReadoutForm(true, 0); // Story - 347720
                }
            }
        }
        /// <summary>
        ///  Story - 347720 - Method signature changed to set the is Single Phase Non DLMS meter
        /// </summary>
        /// <param name="isDLMS"></param>
        private void OpenMeterDataReadoutForm(bool isDLMS, int isMeterType)
        {
            if (isDLMS)
            {
                if (!ActivateThisChild("E650MeterDataReadout"))
                {
                    E650MeterDataReadout e650MeterDataReadout = new E650MeterDataReadout();
                    e650MeterDataReadout.MdiParent = this;
                    e650MeterDataReadout.WindowState = FormWindowState.Maximized;
                    e650MeterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    e650MeterDataReadout.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    e650MeterDataReadout.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    e650MeterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    e650MeterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (!ActivateThisChild("MeterDataReadoutForm"))
                {
                    MeterDataReadoutForm e650MeterDataReadout = new MeterDataReadoutForm();
                    e650MeterDataReadout.MdiParent = this;
                    e650MeterDataReadout.WindowState = FormWindowState.Maximized;
                    e650MeterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    e650MeterDataReadout.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                    e650MeterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    e650MeterDataReadout.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    e650MeterDataReadout.IsMeterType = isMeterType; // Story - 347720
                    e650MeterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
        }
        /// <summary>
        /// Com port and Mode paswsord setting 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cOMPortSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("PortSettingFormNew"))
            {
                PortSettingFormNew portSettingForm = new PortSettingFormNew();
                portSettingForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                portSettingForm.MdiParent = this;
                portSettingForm.Show();
                this.statusBar.Text = this.StatusMessage;
            }
            //if (!ActivateThisChild("PortSettingForm"))
            //{
            //    PortSettingForm portSettingForm = new PortSettingForm();
            //    portSettingForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            //    portSettingForm.MdiParent = this;
            //    portSettingForm.Show();
            //    this.statusBar.Text = this.StatusMessage;
            //}
        }
        /// <summary>
        /// Set Mode and password for DLMS Communication .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dLMSSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("E650Settings") == false)
            {
                E650Settings portSettingForm = new E650Settings();
                portSettingForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                portSettingForm.MdiParent = this;
                portSettingForm.Show();
                this.statusBar.Text = this.StatusMessage;
            }
        }
        /// <summary>
        /// Check for meter type for phasor readout 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dynamicPhasorReadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UtilityDetails.IECSupport)
            {
                toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"),
                    Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                this.Cursor = Cursors.Default;
                if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
                {
                    if (result.RecieveDataBuffer[1].ToString() == "76" && result.RecieveDataBuffer[2].ToString() == "71" && result.RecieveDataBuffer[3].ToString() == "67")
                    {
                        ShowPhasorForm(false, 1);
                    }
                    else
                        ShowPhasorForm(false, 3);
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    ShowPhasorForm(true, 0);
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                {
                    ShowPhasorForm(false, 2);
                }
                else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                {

                    toolStripStatusLabel.Text = "Port Not Available.";
                }
                else
                {
                    toolStripStatusLabel.Text = "Sign-On failure.";
                }
            }
            else
            {
                ShowPhasorForm(true, 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDLMS"></param>
        private void ShowPhasorForm(bool isDLMS, int isMeterType)
        {
            if (isDLMS)
            {
                if (!ActivateThisChild("E650PhasorReadout"))
                {
                    toolStripStatusLabel.Text = "";
                    E650PhasorReadout meterDataReadout = new E650PhasorReadout();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);

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
                if (!ActivateThisChild("IECPhasorReadout"))
                {
                    toolStripStatusLabel.Text = "";
                    IECPhasorReadout meterDataReadout = new IECPhasorReadout();
                    meterDataReadout.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterDataReadout.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    meterDataReadout.MdiParent = this;
                    meterDataReadout.WindowState = FormWindowState.Maximized;
                    meterDataReadout.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterDataReadout.IsMeterType = isMeterType; // Story - 347720
                    meterDataReadout.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
        }
        /// <summary>
        /// sub menu for loadsurvey , display load survey report for MeterDataID (for every seperate file)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["LoadSurveyReportFileWise"] != null)
            {
                Application.OpenForms["LoadSurveyReportFileWise"].Close();
            }

            if (ActivateThisChild("LoadSurveyReportFileWise") == false)
            {
                LoadSurveyReportFileWise loadsurveyReport = new LoadSurveyReportFileWise();
                loadsurveyReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                loadsurveyReport.Show();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }
        /// <summary>
        /// sub menu for loadsurvey , display load survey report for MeterID 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meterIDWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["LoadSurveyReportMeterIDWise"]!= null)
            {
                Application.OpenForms["LoadSurveyReportMeterIDWise"].Close();
            }

            if (ActivateThisChild("LoadSurveyReportMeterIDWise") == false)
            {
                LoadSurveyReportMeterIDWise loadsurveyReport = new LoadSurveyReportMeterIDWise();
                loadsurveyReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                loadsurveyReport.Show();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }
        /// <summary>
        /// General configuration window .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemConfigurationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("GeneralConfiguration"))
            {
                GeneralConfiguration generalConfiguration = new GeneralConfiguration();
                generalConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                generalConfiguration.MdiParent = this;
                generalConfiguration.Show();
                this.statusBar.Text = this.StatusMessage;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mRICommunicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("CMRICommunication"))
            {
                CMRICommunication CMRICommunication = new CMRICommunication();
                CMRICommunication.MdiParent = this;
                CMRICommunication.WindowState = FormWindowState.Maximized;
                CMRICommunication.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                CMRICommunication.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                CMRICommunication.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                CMRICommunication.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                CMRICommunication.Show();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cMRISchedulingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("CMRIScheduleFile"))
            {

                CMRIScheduleFile cMRIScheduleFile = new CMRIScheduleFile();
                cMRIScheduleFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cMRIScheduleFile.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                cMRIScheduleFile.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cMRIScheduleFile.MdiParent = this;
                cMRIScheduleFile.Show();

            }
        }
        /// <summary>
        /// on meter accuracy click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meterAccuracyCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UtilityDetails.IECSupport)
            {
                toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"),
                    Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                this.Cursor = Cursors.Default;
                if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
                {
                    //ShowAccuracyForm(false);
                    if (result.RecieveDataBuffer[1].ToString() == "76" && result.RecieveDataBuffer[2].ToString() == "71" && result.RecieveDataBuffer[3].ToString() == "67")
                    {
                        ShowAccuracyForm(false, 1);
                    }
                    else if (result.RecieveDataBuffer[1].ToString() == "88" && result.RecieveDataBuffer[2].ToString() == "88" && result.RecieveDataBuffer[3].ToString() == "88")
                    {
                        ShowAccuracyForm(false, 3);
                    }
                    else
                    {
                        toolStripStatusLabel.Text = "Sign-On failure.";
                    }

                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                {
                    ShowAccuracyForm(false, 2); // Story - 369686 - Accuracy check for single phase IEC meter
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    //ShowAccuracyForm(true);
                    ShowAccuracyForm(true, 0);
                }
                else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                {
                    toolStripStatusLabel.Text = "Port Not Available.";
                }

                else
                {
                    toolStripStatusLabel.Text = "Sign-On failure/Not Supported.";
                }
            }
            else
            {
                //ShowAccuracyForm(true);
                ShowAccuracyForm(true,0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDLMS"></param>
        private void ShowAccuracyForm(bool isDLMS, int MeterType)
        {
            if (isDLMS)
            {
                if (!ActivateThisChild("E650MeterAccuracyCheck"))
                {
                    toolStripStatusLabel.Text = "";
                    E650MeterAccuracyCheck meterAccuracy = new E650MeterAccuracyCheck();
                    meterAccuracy.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterAccuracy.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);

                    meterAccuracy.MdiParent = this;
                    meterAccuracy.WindowState = FormWindowState.Maximized;
                    meterAccuracy.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterAccuracy.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
            else
            {
                if (!ActivateThisChild("IECMeterAccuracyCheck"))
                {
                    toolStripStatusLabel.Text = "";
                    IECMeterAccuracyCheck meterAccuracy = new IECMeterAccuracyCheck();
                    meterAccuracy.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    meterAccuracy.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    meterAccuracy.MdiParent = this;
                    meterAccuracy.WindowState = FormWindowState.Maximized;
                    meterAccuracy.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    meterAccuracy.IsMeterType = MeterType;
                    meterAccuracy.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
        }
        /// <summary>
        /// on snap read click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void snapReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("SnapRead"))
            {
                toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"),
                    ConfigSettings.GetValue("BaudRate"), Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                this.Cursor = Cursors.Default;
                if (result.ErrorCode == CommunicationErrorType.SuccessForIEC || result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                {
                    toolStripStatusLabel.Text = "Snap Read is not supported for this meter";
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    SnapRead snapRead = new SnapRead();
                    snapRead.MdiParent = this;
                    snapRead.WindowState = FormWindowState.Maximized;
                    snapRead.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    snapRead.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    snapRead.Show();
                }
                else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                {
                    toolStripStatusLabel.Text = "Port Not Available.";
                }
                else
                {
                    toolStripStatusLabel.Text = "Sign-On failure.";
                }
            }
        }



        /// <summary>
        /// on dlms click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dLMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("E650MeterConfigurations"))
            {
                ConfigInfo.MeterModel = "0";
                ConfigInfo.FirmwareVersion = "0.00";
                //ConfigInfo.IsOffLineMode = true;
                E650MeterConfigurations e650MeterConfiguration = new E650MeterConfigurations(false);
                e650MeterConfiguration.Text = "CMRI Configuration";
                e650MeterConfiguration.MdiParent = this;
                e650MeterConfiguration.WindowState = FormWindowState.Maximized;
                e650MeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                e650MeterConfiguration.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                e650MeterConfiguration.Show();
            }
            else
            {

                if (!CheckIfFormIsOpened("CMRI Configuration", "E650MeterConfigurations"))
                {
                    MessageBox.Show("To open CMRI configuration, please close meter programming", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripStatusLabel.Text = "";
                    Application.DoEvents();
                }
            }
        }
        /// <summary>
        /// on iec click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iECToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("IECMeterConfiguration"))
            {
                ConfigInfo.MeterModel = "0";
                ConfigInfo.FirmwareVersion = "0.00";
                toolStripStatusLabel.Text = "";
                IECMeterConfiguration IECMeterConfiguration = new IECMeterConfiguration(false);
                IECMeterConfiguration.Text = "CMRI Configuration";
                IECMeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                IECMeterConfiguration.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                IECMeterConfiguration.MdiParent = this;
                IECMeterConfiguration.WindowState = FormWindowState.Maximized;
                IECMeterConfiguration.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                IECMeterConfiguration.Show();
            }
            else
            {
                toolStripStatusLabel.Text = "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meterConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigInfo.IsOnline = true;
            string metertypeforconfig = "";
            CAB.IECFramework.Utility.ConfigInfo.SignatureInfo = string.Empty;
            metertypeforconfig = ConfigSettings.GetValue("MeterFirmwareType");
            //No need to check Meter type as remote one to one is only implmented for DLMS.            
            if ((ConfigSettings.GetValue("ChannelType") == CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
            {
                if (UtilityDetails.IECSupport)
                {
                    toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"), ConfigSettings.GetValue("BaudRate"),Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));

                   

                    this.Cursor = Cursors.Default;
                    //if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
                    //{
                    //    ShowMeterConfigurationForm(false);
                    //}
                    //else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                    //{
                    //    ShowMeterConfigurationForm(true);
                    //}


                    if (result.ErrorCode == CommunicationErrorType.SuccessForIEC)
                    {
                        // Story - 347720 - To compare LGC in ASCII format
                        if (result.RecieveDataBuffer[1].ToString() == "76" && result.RecieveDataBuffer[2].ToString() == "71" && result.RecieveDataBuffer[3].ToString() == "67")
                        {
                            ShowMeterConfigurationForm(false, 1);
                        }
                        else if (result.RecieveDataBuffer[1].ToString() == "88" && result.RecieveDataBuffer[2].ToString() == "88" && result.RecieveDataBuffer[3].ToString() == "88")
                        {
                            ShowMeterConfigurationForm(false, 3);
                        }
                        else
                        {
                            toolStripStatusLabel.Text = "Sign-On failure.";
                        }
                    }
                    else if (result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                    {
                        ShowMeterConfigurationForm(false, 2);
                    }
                    else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                    {
                        ShowMeterConfigurationForm(true, 0);
                    }
                    else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                    {
                        toolStripStatusLabel.Text = "Port Not Available.";
                    }
                    else
                    {
                        toolStripStatusLabel.Text = "Meter not connected.";
                    }
                }
                else
                {
                    ShowMeterConfigurationForm(true, 0);
                }
            }
            else
            {

                if (ConfigSettings.GetValue("ChannelType") == "GSM" && metertypeforconfig == "3")
                {
                    ShowMeterConfigurationForm(false, 2);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    ShowMeterConfigurationForm(true, 0);
                    this.Cursor = Cursors.Default;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDLMS"></param>
        private void ShowMeterConfigurationForm(bool isDLMS, int isMeterType)
        {
            if (isDLMS)
            {

                if (!ActivateThisChild("E650MeterConfigurations"))
                {
                    toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();

                    if ((ConfigSettings.GetValue("ChannelType") == CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
                    {
                        GetMeterModelAndFirmwareVersion();
                    }
                    else
                    {
                        // Setting the default values (6, 0.00)
                        ConfigInfo.MeterModel = "6";
                        ConfigInfo.FirmwareVersion = "0.00";
                    }
                    if (ConfigInfo.MeterModel == "0" && ConfigInfo.FirmwareVersion == "0.00")
                    {
                        toolStripStatusLabel.Text = "Meter not supported / not connected.";
                    }
                    else
                    {
                        toolStripStatusLabel.Text = "";
                        Application.DoEvents();
                        E650MeterConfigurations e650MeterConfiguration = new E650MeterConfigurations(true);
                        e650MeterConfiguration.MdiParent = this;
                        e650MeterConfiguration.WindowState = FormWindowState.Maximized;
                        e650MeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                        e650MeterConfiguration.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                        e650MeterConfiguration.Show();
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                    if (!CheckIfFormIsOpened("Meter Programming", "E650MeterConfigurations"))
                    {
                        MessageBox.Show("To open meter programming, please close CMRI configuration", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.DoEvents();
                    }
                }
            }
            else
            {
                if (!ActivateThisChild("IECMeterConfiguration"))
                {
                    toolStripStatusLabel.Text = "";
                    if (isMeterType == 1 || isMeterType == 2)
                    {
                        ConfigInfo.MeterModel = "21";
                        ConfigInfo.FirmwareVersion = "0.00";
                    }
                    else
                    {
                        ConfigInfo.MeterModel = "1";
                        ConfigInfo.FirmwareVersion = "1.63";
                    }
                    IECMeterConfiguration IECMeterConfiguration = new IECMeterConfiguration(true);
                    IECMeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    IECMeterConfiguration.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    IECMeterConfiguration.MdiParent = this;
                    IECMeterConfiguration.WindowState = FormWindowState.Maximized;
                    IECMeterConfiguration.OnListRefresh += new IsListRefresh(MainForm_OnListRefresh);
                    IECMeterConfiguration.IsMeterType = isMeterType;
                    IECMeterConfiguration.Show();
                }
                else
                {
                    toolStripStatusLabel.Text = "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formText"></param>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool CheckIfFormIsOpened(string formText, string formName)
        {
            bool isformOpen = false;
            for (int count = 0; count < this.MdiChildren.Length; count++)
            {
                if (this.MdiChildren[count].Name == formName)
                {
                    if (this.MdiChildren[count].Text == formText)
                    {
                        isformOpen = true;
                    }
                }
            }
            return isformOpen;
        }
        /// <summary>
        /// Gets Meter Model and Firmware veriosn 
        /// </summary>
        private void GetMeterModelAndFirmwareVersion()
        {
            try
            {
                string MeterID = string.Empty;
                //-----------------------
                ChannelInformation channelInfo = new ChannelInformation();
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                   channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter
                else
                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");                
                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
                channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communication = new Communication(channelInfo);
                //-----------------------
                ConfigInfo.MeterModel = "0";
                ConfigInfo.FirmwareVersion = "0.00";
                ConfigInfo.SignatureInfo = "";
                Result result = new Result();
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                    if (channelInfo.SecurityMechanism == 0x00)
                    {
                        result = communication.OpenSession();
                        if (commType == CommunicationType.TCP && result.ErrorCode == CommunicationErrorType.Success)
                        {
                            this.StatusMessageAsync = "Remote Modem Connected";
                        }
                        // ************Read Invocation Counter start
                        ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
                        if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                        {
                            if (ConfigSettings.GetValue("SecurityMechanism") == "01")
                                profileCommand = new ProfileCommand(01, "00.00.2B.01.02.FF", 02);
                            else if (ConfigSettings.GetValue("SecurityMechanism") == "02")
                                profileCommand = new ProfileCommand(01, "00.00.2B.01.03.FF", 02);
                        }
                        result = communication.Send(profileCommand);
                        communication.CloseSession();
                        if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                        {
                            int index = 0;
                            long InvoCountValue = 0;
                            long InitializationCounter = 0;
                            byte[] incount = new byte[4];

                            if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                            {
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                communication = new Communication(channelInfo);
                                securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                                int adbyteindex = 0;
                                for (int i = 1; i < index + 5; i++)
                                {
                                    incount[adbyteindex++] = result.RecieveDataBuffer[i];

                                }
                                data =Convertstring.FormatData(incount, false);
                                if (data != null) InvoCountValue = Convert.ToInt64(data);
                                InitializationCounter = InvoCountValue + 1;

                                List<string> SecurityKeyDetails = CABApplication.Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                                //MohsinRaza : below code is added to handle dual key logic for smart meter

                                if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);
                                }
                                else //-------------Set to Default if Meter ID not found in EndDeviceSecurtityResponse file---------------
                                {
                                    logger.Log(LOGLEVELS.Error, "ReadOneToOne()--> Setting Security Keys To Default for meter ID :" + MeterID + " due to Null return from EndDeviceSecurityResponse");
                                    ConfigSettings.ChangeNode("ModePassword", ConfigSettings.GetValue("DefaultModePassword"));
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", ConfigSettings.GetValue("DefaultGlobalEncryptionKey"));
                                    ConfigSettings.ChangeNode("AuthenticationKey", ConfigSettings.GetValue("DefaultAuthenticationKey"));
                                }

                                result = communication.OpenSessionCipher(InitializationCounter);
                                //----------------CC Error Case handeling for retry with additional key received--------------------------
                                if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > 3 && SecurityKeyDetails[3].Trim().Length > 0)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[3]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[3]);
                                    result = communication.OpenSessionCipher(InitializationCounter + 1);
                                }
                            }
                        }
                    }

                if (ConfigSettings.GetValue("ApplicationContext") != "03")
                    result = communication.OpenSession();

                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    string signatureData = communication.GetSignatureData();
                    ConfigInfo.SignatureInfo = signatureData;
                    communication.CloseSession();
                    DLMS650NamePlateDetailsEntity generalEntity = new DLMS650NamePlateDetailsEntity();
                    new General().MapSignatureData(signatureData, generalEntity);
                    ConfigInfo.MeterModel = generalEntity.MeterModelNo;
                    decimal fwVrsion;
                    string intenalFWVersion = string.Format("{0:0.00}", decimal.TryParse(generalEntity.InternalFirmwareVersion,out fwVrsion));
                    if (fwVrsion <= 0) intenalFWVersion = generalEntity.InternalFirmwareVersion;
                    //string intenalFWVersion = string.Format("{0:0.00}", Convert.ToDecimal(generalEntity.InternalFirmwareVersion));
                    //******* Meter Model Change Required Here ***********//
                    if (ConfigInfo.MeterModel == "9" 
                        || ConfigInfo.MeterModel == "6" 
                        || ConfigInfo.MeterModel == "8" 
                        || ConfigInfo.MeterModel == ((int)NamePlateConstants.SFSP).ToString() 
                        || ConfigInfo.MeterModel == ((int)NamePlateConstants.VBSPNoSeasonNoWeek).ToString() 
                        || ConfigInfo.MeterModel == ((int)NamePlateConstants.VIM_Series2).ToString() 
                        || ConfigInfo.MeterModel == ((int)NamePlateConstants.VFSPNoSeasonNoWeek).ToString() 
                        || ConfigInfo.MeterModel == "10" 
                        || ConfigInfo.MeterModel == "11" 
                        || ConfigInfo.MeterModel == "12" 
                        || ConfigInfo.MeterModel == "13" 
                        || ConfigInfo.MeterModel == "14" 
                        || ConfigInfo.MeterModel == "15" 
                        || ConfigInfo.MeterModel == "17" 
                        || ConfigInfo.MeterModel == "18" 
                        || ConfigInfo.MeterModel == "23" 
                        || ConfigInfo.MeterModel == "24" 
                        || ConfigInfo.MeterModel == "25" 
                        || ConfigInfo.MeterModel == "27" 
                        || ConfigInfo.MeterModel == "28"
                        || ConfigInfo.MeterModel == "29" 
                        || ConfigInfo.MeterModel == "30" 
                        || ConfigInfo.MeterModel == "32" 
                        || ConfigInfo.MeterModel == "33" 
                        || ConfigInfo.MeterModel == "34" 
                        || ConfigInfo.MeterModel == "35"
                        || ConfigInfo.MeterModel == "36"
                        || ConfigInfo.MeterModel == "37"
                        || ConfigInfo.MeterModel == "39" 
                        || ConfigInfo.MeterModel == "40" 
                        || ConfigInfo.MeterModel == "41"
                        || ConfigInfo.MeterModel == "42"
                        || ConfigInfo.MeterModel == "43"
                        || ConfigInfo.MeterModel == ((int)NamePlateConstants.BRPL_CBSP).ToString()  //user story 1016689
                        || ConfigInfo.MeterModel == "45"
                        || ConfigInfo.MeterModel == "46"
                        )
                    {
                        ConfigInfo.FirmwareVersion = "0.00";
                    }
                    else if (ConfigInfo.MeterModel == "2" && (intenalFWVersion != "4.64" && intenalFWVersion != "7.16" && intenalFWVersion != "7.22"
                        && intenalFWVersion != "7.25" && intenalFWVersion != "7.26" && intenalFWVersion != "7.27" && intenalFWVersion != "7.28"))
                    {
                        ConfigInfo.FirmwareVersion = "0.00";
                    }
                    else
                    {
                        ConfigInfo.FirmwareVersion = intenalFWVersion;
                    }

                    CommonMethods.GetDisplayProgrammingVariantFromSignature(signatureData);
                }
                 else communication.CloseSession();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterModelAndFirmwareVersion()", ex);
            }
        }

      
        private void toolStripFileWise_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["MidNightReportFileWise"] != null)
            {
                Application.OpenForms["MidNightReportFileWise"].Close();
            }

            if (ActivateThisChild("MidNightReportFileWise") == false)
            {
                MidNightReportFileWise midNightReport = new MidNightReportFileWise();
                midNightReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                midNightReport.Show();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }

        private void toolStripMeterIDWise_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            //{
            //    MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["MidNightReportMeterIDWise"] != null)
            {
                Application.OpenForms["MidNightReportMeterIDWise"].Close();
            }

            if (ActivateThisChild("MidNightReportMeterIDWise") == false)
            {
                MidNightReportMeterIDWise midNightReport = new MidNightReportMeterIDWise();
                // midNightReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                midNightReport.ShowDialog();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }
        /// <summary>
        /// Open Offline programming window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigInfo.IsOnline = false;
            if (!ActivateThisChild("E650MeterConfigurations"))
            {
                ConfigInfo.MeterModel = "0";
                ConfigInfo.FirmwareVersion = "0.00";
                //ConfigInfo.IsOffLineMode = true;
                E650MeterConfigurations e650MeterConfiguration = new E650MeterConfigurations(false);
                e650MeterConfiguration.Text = "CMRI Configuration";
                e650MeterConfiguration.MdiParent = this;
                e650MeterConfiguration.WindowState = FormWindowState.Maximized;
                e650MeterConfiguration.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                e650MeterConfiguration.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                e650MeterConfiguration.Show();
            }
            else
            {
                if (!CheckIfFormIsOpened("CMRI Configuration", "E650MeterConfigurations"))
                {
                    MessageBox.Show("To open CMRI configuration, please close meter programming", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripStatusLabel.Text = "";
                    Application.DoEvents();
                }
            }
        }

        private void gPRSStatusReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPRSAnalysisView analysisView = new GPRSAnalysisView();
            analysisView.MdiParent = this;
            analysisView.WindowState = FormWindowState.Maximized;
            analysisView.Show();
        }

        private void excelExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("ExcelExport"))
            {
                ExcelExport excelExport = new ExcelExport();
                excelExport.IsAutomationReport = false;
                excelExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                excelExport.MdiParent = this;
                excelExport.WindowState = FormWindowState.Maximized;
                excelExport.Show();
            }


        }


        /// <summary>
        /// Used to format data to be exported inn excelsheet.
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
                        //NO action
                    }
                    else
                    {
                        record[field] = record[field].ToString().Split('*')[0];
                    }
                }
            }

            return inputTable;
        }

        private void customTextFileExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("TextFileExport"))
            {
                TextFileExport textFileExport = new TextFileExport();
                textFileExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                textFileExport.MdiParent = this;
                textFileExport.WindowState = FormWindowState.Maximized;
                textFileExport.Show();
            }
        }

        private void automationExToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelExport excelExport = new ExcelExport();
            excelExport.IsAutomationReport = true;
            excelExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            excelExport.MdiParent = this;
            excelExport.WindowState = FormWindowState.Maximized;
            excelExport.Show();
        }

        private void aBCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ABCDecryption abcDecryption = new ABCDecryption();

            abcDecryption.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            abcDecryption.MdiParent = this;
            abcDecryption.WindowState = FormWindowState.Maximized;
            abcDecryption.Show();

        }

        private void schedularSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("SchedularSettingForm"))
            {
                SchedularSettingForm schedularSettingForm = new SchedularSettingForm();
                schedularSettingForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                schedularSettingForm.MdiParent = this;
                schedularSettingForm.Show();
                this.statusBar.Text = this.StatusMessage;
            }
        }

        private void aBC1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ABC2Decrypt abc2Decryption = new ABC2Decrypt();

            abc2Decryption.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            abc2Decryption.MdiParent = this;
            abc2Decryption.WindowState = FormWindowState.Maximized;
            abc2Decryption.Show();
        }
        // userstory 505186
        private void EnableDisableSearchControls(string SearchBy)
        {
            try
            {
                if (meterFileList != null)
                {
                    string data = "MSN";
                    if (data.Equals(SearchBy))
                    {
                        meterFileList.lblFromdate.Visible = true;
                        meterFileList.DtpickerFrom.Visible = true;
                        meterFileList.lblTodate.Visible = true;
                        meterFileList.DtpickerTo.Visible = true;
                        meterFileList.btnShow.Visible = true;
                    }
                    else
                    {
                        meterFileList.lblFromdate.Visible = false;
                        meterFileList.DtpickerFrom.Visible = false;
                        meterFileList.lblTodate.Visible = false;
                        meterFileList.DtpickerTo.Visible = false;
                        meterFileList.btnShow.Visible = false;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "EnableDisableSearchControls(string SearchBy)", ex);
            }
           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["BillingReportnew_TNEB"] != null)
            {
                Application.OpenForms["BillingReportnew_TNEB"].Close();
            }

            if (ActivateThisChild("LoadSurveyReportFileWise") == false)
            {

                BillingReportnew_TNEB BillingReport = new BillingReportnew_TNEB();
                // BillingReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChangedBilling);
                BillingReport.Show();
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }

        private void mnuMeterCommunication_Click(object sender, EventArgs e)
        {

        }

        private void fTPSeetingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ActivateThisChild("FTPSettings"))
                {
                    FTPSettings fTPSettingsForm = new FTPSettings();                    
                    fTPSettingsForm.MdiParent = this;
                    fTPSettingsForm.WindowState = FormWindowState.Maximized;
                    fTPSettingsForm.Show();
                    this.statusBar.Text = this.StatusMessage;
                }
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "fTPSeetingsToolStripMenuItem_Click(object sender, EventArgs e)", ex);
                return;                
            }
        }      

        private void fTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ActivateThisChild("FTPConnect"))
                {
                    FTPConnect fTPConnectForm = new FTPConnect();
                    fTPConnectForm.MdiParent = this;
                    fTPConnectForm.WindowState = FormWindowState.Maximized;
                    fTPConnectForm.Show();
                    this.statusBar.Text = this.StatusMessage;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "fTPToolStripMenuItem_Click(object sender, EventArgs e)", ex);
                return;
            }
        }

        private void gPRSLogReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GSMLog gsmLog = new GSMLog();
                gsmLog.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                gsmLog.StartPosition = FormStartPosition.CenterScreen;
                gsmLog.ShowDialog();
            }
            catch (Exception ex)    //Exception log for catch block 
            {
                logger.Log(LOGLEVELS.Error, "gPRSLogReportToolStripMenuItem_Click(object sender, EventArgs e)", ex);
                return;
            }           
        }

        private void hVDSExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ActivateThisChild("HVDSExport"))
                {
                    HVDSExport textFileExport = new HVDSExport();
                    textFileExport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    textFileExport.MdiParent = this;
                    textFileExport.WindowState = FormWindowState.Maximized;
                    textFileExport.Show();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "hVDSExportToolStripMenuItem_Click(object sender, EventArgs e)", ex);
                return;
            }            
        }

        // SB Code Change Start 20171120 - Search
        
        private void cboSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtMeterId_TextChanged(object sender, EventArgs e)
        {
            if (listViewExplorer.ListData == null || listViewExplorer.ListData.Tables.Count == 0 || txtSearch.Visible == false)
            {
                return;
            }

            listViewExplorer.Items.Clear();

            string data = ((System.Data.DataRowView)(cboSearch.Items[cboSearch.SelectedIndex])).Row.ItemArray[1].ToString();

            if (!txtSearch.Text.Trim().Equals(string.Empty) && !txtSearch.Text.Trim().ToLower().Equals("search..."))
            {
                DataSet daMeter = new DataSet();
                daMeter.Tables.Add(listViewExplorer.ListData.Tables[0].Clone());

                DataRow[] rows = null;

                if (data.Equals("CABF"))
                //listViewExplorer.ListData = new FileUploadMasterBLL().ComboList();
                    rows = new FileUploadMasterBLL().ComboList().Tables[0].Select("FileName like '%" + txtSearch.Text.Trim() + "%'");
                else if (data.Equals("CN"))
                    //listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(true);
                    rows = new ConsumerMeterBLL().ComboList(true).Tables[0].Select("Consumer_Number like '%" + txtSearch.Text.Trim() + "%'");
                else if (data.Equals("L"))
                        //listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(false);
                    rows = new ConsumerMeterBLL().ComboList(false).Tables[0].Select("Meter_Location like '%" + txtSearch.Text.Trim() + "%'");
                else if (data.Equals("MSN"))
                        //listViewExplorer.ListData = new MeterDataBLL().ComboList("MeterId");
                        rows = new MeterDataBLL().ComboList("MeterId").Tables[0].Select("MeterId like '%" + txtSearch.Text.Trim() + "%'");
                else if (data.Equals("CMRI"))
                    //listViewExplorer.ListData = GetCMRIListData();
                    rows = GetCMRIListData().Tables[0].Select("CMRI_Number like '%" + txtSearch.Text.Trim() + "%'");
                else if (data.Equals("RD"))
                {
                    DataSet dataSet = new MeterDataBLL().ComboList("ReadingDateTime");
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
                    rows = ds.Tables[0].Select("DisplayMember like '%" + txtSearch.Text.Trim() + "%'");
                }

                for (int i = 0; i < rows.Length; i++)
                {
                    daMeter.Tables[0].Rows.Add(rows[i].ItemArray);
                }

                listViewExplorer.ListData = daMeter;
            }
            else
            {
                if (data.Equals("CABF"))
                    listViewExplorer.ListData = new FileUploadMasterBLL().ComboList();
                else if (data.Equals("CN"))
                    listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(true);
                else if (data.Equals("L"))
                    listViewExplorer.ListData = new ConsumerMeterBLL().ComboList(false);
                else if (data.Equals("MSN"))
                    listViewExplorer.ListData = new MeterDataBLL().ComboList("MeterId");
                else if (data.Equals("CMRI"))
                    listViewExplorer.ListData = GetCMRIListData();
                else if (data.Equals("RD"))
                {
                    DataSet dataSet = new MeterDataBLL().ComboList("ReadingDateTime");
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
            }
        }

        private void txtMeterId_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void txtMeterId_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().Equals(string.Empty))
            {
                txtSearch.Text = "Search...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtMeterId_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().ToLower().Equals("search..."))
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = Color.FromArgb(40, 40, 40);
            }
        }

        // SB Code Change End 20171120 - Search
        private void securityKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("CMRISecurityKey"))
            {

                CMRISecurityKey cMRISecurityFile = new CMRISecurityKey();
                cMRISecurityFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cMRISecurityFile.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                cMRISecurityFile.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cMRISecurityFile.MdiParent = this;
                cMRISecurityFile.Show();

            }
        }

        private void securityFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("CMRIScheduleFile"))
            {

                CMRIScheduleFile cMRIScheduleFile = new CMRIScheduleFile();
                cMRIScheduleFile.SecurityFileFlag = true;
                cMRIScheduleFile.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                cMRIScheduleFile.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                cMRIScheduleFile.On_RightStatusChanged += new IsRightStatusChanged(MainForm_On_RightStatusChanged);
                cMRIScheduleFile.MdiParent = this;
                cMRIScheduleFile.Show();

            }
        }     

//SarkarA code change start 20180220 // implement to and from dates search
        private void ShowDTPicker(string data)
        {
            if (data.Equals("RD"))
            {
                panelSearchBoxContainer.Height = 68;
                lblTo.Visible = true;
                lblFrom.Visible = true;
                dateTimePickerTo.Visible = true;
                dateTimePickerFrom.Visible = true;
            }
            else
            {
                panelSearchBoxContainer.Height = 25;
                lblTo.Visible = false;
                lblFrom.Visible = false;
                dateTimePickerTo.Visible = false;
                dateTimePickerFrom.Visible = false;
            }
        }

        private void dateTimePickerSearch_ValueChanged(object sender, EventArgs e)
        {
            long dateTimeFrom = DateUtility.DateTimeToLong(dateTimePickerFrom.Value);
            long dateTimeTo = DateUtility.DateTimeToLong(dateTimePickerTo.Value);
            if (!(dateTimeFrom <= dateTimeTo))
            {
                MessageBox.Show("'From' Date cannot be greater than 'To' Date", "Incorrect Format", MessageBoxButtons.OK);
                dateTimePickerFrom.Value = dateTimePickerTo.Value;
            }
            DataRow[] rows = null;
            DataSet dataSet = new MeterDataBLL().ComboList("ReadingDateTime");
            DataSet daMeter = new DataSet();
            DataSet listMeter = new DataSet();
            daMeter.Tables.Add(listViewExplorer.ListData.Tables[0].Clone());

            rows = dataSet.Tables[0].Select("ReadingDateTime >= " + dateTimeFrom + " and ReadingDateTime <= " + dateTimeTo);

            for (int i = 0; i < rows.Length; i++)
            {

                DataRow row = daMeter.Tables[0].NewRow();
                string dataDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(rows[i][0]));
                if (dataDate != "---------")
                {
                    dataDate = dataDate.Substring(0, 10);
                    row["DisplayMember"] = dataDate;

                    daMeter.Tables[0].Rows.Add(row);
                }
            }

            listMeter.Tables.Add(daMeter.Tables[0].DefaultView.ToTable(true, "DisplayMember"));
            listViewExplorer.ListData = listMeter;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            {
                MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // SB code change Start - 20180629 - Multiple Analysis View
            if (Application.OpenForms["LoadSwitchReport"] != null)
            {
                Application.OpenForms["LoadSwitchReport"].Close();
            }
            
            if (ActivateThisChild("LoadSwitchReport") == false)
            {
                LoadSwitchReport loadswitchReport = new LoadSwitchReport();
                //  loadswitchReport.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                if (loadswitchReport.HasData)
                {
                    loadswitchReport.Show();
                }
            }
            // SB code change End - 20180629 - Multiple Analysis View
        }

        private void gSMConfigurationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GsmConfigReport gsmConfig = new GsmConfigReport();
            gsmConfig.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            gsmConfig.ShowDialog();
        }

        private void gSMReadReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GsmReadReport gsmRead = new GsmReadReport();
            gsmRead.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
            gsmRead.ShowDialog();
        }

        private void meterPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActivateThisChild("E650MeterPassword") == false)
            {
                E650MeterPassword MRPasswordWriteForm = new E650MeterPassword();
                MRPasswordWriteForm.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                MRPasswordWriteForm.MdiParent = this;
                MRPasswordWriteForm.Show();
                this.statusBar.Text = this.StatusMessage;
            }
        }

        private void uploadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUploadXmlToCC _frmuploadtocc = new frmUploadXmlToCC();

            _frmuploadtocc.ShowDialog();
        }

        private void dynamicHighResolutionReadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("E650HRReadout"))
            {
                toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"),
                    ConfigSettings.GetValue("BaudRate"), Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                this.Cursor = Cursors.Default;
                if (result.ErrorCode == CommunicationErrorType.SuccessForIEC || result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                {
                    toolStripStatusLabel.Text = "High Resolution Read is not supported for this meter";
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    E650HRReadout snapRead = new E650HRReadout();
                    snapRead.MdiParent = this;
                    snapRead.WindowState = FormWindowState.Maximized;
                    snapRead.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    snapRead.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    snapRead.Show();
                }
                else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                {
                    toolStripStatusLabel.Text = "Port Not Available.";
                }
                else
                {
                    toolStripStatusLabel.Text = "Sign-On failure.";
                }
            }
        }

        private void irDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("IrDAReadout"))
            {
                IrDAReadout form = new IrDAReadout();
                form.MdiParent = this;
                form.WindowState = FormWindowState.Maximized;
                form.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                form.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                form.Show();
            }
        }
        private void TsmDebug_decryptSecurityKeys_Click(object sender, EventArgs e)
        {
            DebugSecurityKey objdebugsecKey = new DebugSecurityKey();
            objdebugsecKey.ShowDialog();
        }

        private void adhocReadoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ActivateThisChild("AdhocRead"))
            {
                toolStripStatusLabel.Text = "Checking For Meter Type,Please Wait....";
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Result result = communication.CheckSession(ConfigSettings.GetValue("PortName"),
                    ConfigSettings.GetValue("BaudRate"), Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")));
                this.Cursor = Cursors.Default;
                if (result.ErrorCode == CommunicationErrorType.SuccessForIEC || result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                {
                    toolStripStatusLabel.Text = "Adhoc Read is not supported for this meter";
                }
                else if (result.ErrorCode == CommunicationErrorType.SuccessForDLMS)
                {
                    AdhocRead adhocpRead = new AdhocRead();
                    adhocpRead.MdiParent = this;
                    adhocpRead.WindowState = FormWindowState.Maximized;
                    adhocpRead.On_StatusChanged += new IsStatusChanged(MainForm_OnStatusChanged);
                    adhocpRead.On_ConnectionDetailStatusChanged += new IsConnectionDetailStatusChanged(MainForm_OnStatusChangedLabel2);
                    adhocpRead.Show();
                }
                else if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                {
                    toolStripStatusLabel.Text = "Port Not Available.";
                }
                else
                {
                    toolStripStatusLabel.Text = "Sign-On failure.";
                }
            }
        }

        private void adHocReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
            //{
            //    MessageBox.Show("Please select file from explorer window.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
                        
            if (Application.OpenForms["AdhocReport"] != null)
            {
                Application.OpenForms["AdhocReport"].Close();
            }

            if (ActivateThisChild("LoadSurveyReportFileWise") == false)
            {

               AdhocReport adhocReport = new AdhocReport();                
                adhocReport.Show();
            }
            
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripBtnCMRICommunication_Click(object sender, EventArgs e)
        {

        }

        private void toolStripBtnSystemConfig_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void listViewExplorer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //public static string formatdata(byte[] buffer, bool bunsignflag)
        //{
        //    stringbuilder sb = new stringbuilder();

        //    bool bsignflag = false;
        //    int64 tempval = 0;
        //    for (int i = 0; i < buffer.length; i++)
        //    {

        //        if (buffer[0] > 127)
        //        {

        //            if (buffer.length > 1)
        //            {
        //                if (bunsignflag) bsignflag = true;

        //            }
        //        }
        //        sb.append(buffer[i].tostring("x2"));
        //    }

        //    if (bsignflag == true)
        //    {
        //        if (buffer.length == 4)
        //        {
        //            tempval = convert.toint64("ffffffff", 16) - (convert.toint64(sb.tostring(), 16) - 1);
        //            return "-" + tempval.tostring();
        //        }
        //        else if (buffer.length == 8)
        //        {
        //            tempval = convert.toint64("ffffffffffffffff", 16) - (convert.toint64(sb.tostring(), 16) - 1);
        //            return "-" + tempval.tostring();
        //        }
        //        else
        //        {
        //            tempval = convert.toint32("ffff", 16) - (convert.toint64(sb.tostring(), 16) - 1);
        //            return "-" + tempval.tostring();
        //        }

        //    }
        //    else
        //    {
        //        return convert.toint64(sb.tostring(), 16).tostring();
        //    }
        //}



    }
}
