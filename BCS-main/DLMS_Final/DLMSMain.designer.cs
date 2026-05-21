namespace DLMS_Final
{
    partial class DLMSMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMeterID = new System.Windows.Forms.Label();
            this.lblMeterIDVal = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLblSettings = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolstripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataDownloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageCompartment1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdFastDownload = new System.Windows.Forms.RadioButton();
            this.chkNormalDownload = new System.Windows.Forms.RadioButton();
            this.grpBoxDLMSRead = new System.Windows.Forms.GroupBox();
            this.chkPhasor = new System.Windows.Forms.CheckBox();
            this.chkMidnightData = new System.Windows.Forms.CheckBox();
            this.chkOther = new System.Windows.Forms.CheckBox();
            this.chkNameplate = new System.Windows.Forms.CheckBox();
            this.chkTamper = new System.Windows.Forms.CheckBox();
            this.chkLoadSurvey = new System.Windows.Forms.CheckBox();
            this.chkBilling = new System.Windows.Forms.CheckBox();
            this.chkInsta = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpBoxLS = new System.Windows.Forms.GroupBox();
            this.cmbLSDays = new System.Windows.Forms.ComboBox();
            this.rdBtnReadCompleteLS = new System.Windows.Forms.RadioButton();
            this.rdBtnReadBetweenLS = new System.Windows.Forms.RadioButton();
            this.dtPickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtPickerTo = new System.Windows.Forms.DateTimePicker();
            this.btnReadAll = new System.Windows.Forms.Button();
            this.grpBoxEventLog = new System.Windows.Forms.GroupBox();
            this.rdBtnReadBetweenEvent = new System.Windows.Forms.RadioButton();
            this.rdBtnReadLastEvent = new System.Windows.Forms.RadioButton();
            this.rdBtnReadCompleteEvent = new System.Windows.Forms.RadioButton();
            this.cmbBoxFromEvent = new System.Windows.Forms.ComboBox();
            this.cmbBoxLastFromEvent = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBoxToEvent = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpBoxBillingHistory = new System.Windows.Forms.GroupBox();
            this.rdBtnReadBetween = new System.Windows.Forms.RadioButton();
            this.rdBtnReadLast = new System.Windows.Forms.RadioButton();
            this.cmbBoxFrom = new System.Windows.Forms.ComboBox();
            this.cmbBoxLastFrom = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMonths = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBoxTo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rdBtnReadComplete = new System.Windows.Forms.RadioButton();
            this.tabPageCompartment2 = new System.Windows.Forms.TabPage();
            this.tabControlCMRI = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lstCMRIfile = new System.Windows.Forms.CheckedListBox();
            this.btnCMRICancel = new System.Windows.Forms.Button();
            this.btnReadAllCMRI = new System.Windows.Forms.Button();
            this.btnLoadList = new System.Windows.Forms.Button();
            this.chkSelectAllMeters = new System.Windows.Forms.CheckBox();
            this.grpPartialRead = new System.Windows.Forms.GroupBox();
            this.chkCMRIPhasor = new System.Windows.Forms.CheckBox();
            this.chkCMRISelectAll = new System.Windows.Forms.CheckBox();
            this.chkCMRIMidnightData = new System.Windows.Forms.CheckBox();
            this.chkCMRINameplate = new System.Windows.Forms.CheckBox();
            this.chkCMRITamper = new System.Windows.Forms.CheckBox();
            this.chkCMRILoadSurvey = new System.Windows.Forms.CheckBox();
            this.chkCMRIBilling = new System.Windows.Forms.CheckBox();
            this.chkCMRIInstant = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstFast = new System.Windows.Forms.CheckedListBox();
            this.chkFDSelectAll = new System.Windows.Forms.CheckBox();
            this.btnLoadMeterFD = new System.Windows.Forms.Button();
            this.btnCancelFD = new System.Windows.Forms.Button();
            this.btnFDRead = new System.Windows.Forms.Button();
            this.tabProgramming = new System.Windows.Forms.TabPage();
            this.tabCTPTRatio = new System.Windows.Forms.TabControl();
            this.tabPageRTC = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.dGVReadRTC = new System.Windows.Forms.DataGridView();
            this.colSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRTC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReadRTC = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBoxRTC = new System.Windows.Forms.TextBox();
            this.btnWriteRTC = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.cmbBoxBillingMinute = new System.Windows.Forms.ComboBox();
            this.cmbBoxBillingHour = new System.Windows.Forms.ComboBox();
            this.cmbBoxBillingDate = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbBoxBillingPeriod = new System.Windows.Forms.ComboBox();
            this.btnReadBillingDatetime = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.btnWriteBillingDatetime = new System.Windows.Forms.Button();
            this.tabPageTOUConfiguration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageSeason1 = new System.Windows.Forms.TabPage();
            this.grpDayTables = new System.Windows.Forms.GroupBox();
            this.gridTOUDay6 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay5 = new System.Windows.Forms.DataGridView();
            this.lblDayTable6 = new System.Windows.Forms.Label();
            this.lblDayTable5 = new System.Windows.Forms.Label();
            this.lblDayTable4 = new System.Windows.Forms.Label();
            this.lblDayTable3 = new System.Windows.Forms.Label();
            this.lblDayTable2 = new System.Windows.Forms.Label();
            this.lblDayTable1 = new System.Windows.Forms.Label();
            this.gridTOUDay1 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay2 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay3 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay4 = new System.Windows.Forms.DataGridView();
            this.tabPageSeason2 = new System.Windows.Forms.TabPage();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.gridTOUDay12 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay11 = new System.Windows.Forms.DataGridView();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.gridTOUDay7 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay8 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay9 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay10 = new System.Windows.Forms.DataGridView();
            this.tabPageSeason3 = new System.Windows.Forms.TabPage();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.gridTOUDay18 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay17 = new System.Windows.Forms.DataGridView();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.gridTOUDay13 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay14 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay15 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay16 = new System.Windows.Forms.DataGridView();
            this.tabPageSeason4 = new System.Windows.Forms.TabPage();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.gridTOUDay24 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay23 = new System.Windows.Forms.DataGridView();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.gridTOUDay19 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay20 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay21 = new System.Windows.Forms.DataGridView();
            this.gridTOUDay22 = new System.Windows.Forms.DataGridView();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.btnFillTOUConfiguration = new System.Windows.Forms.Button();
            this.dTPFutureActivationDate = new System.Windows.Forms.DateTimePicker();
            this.label191 = new System.Windows.Forms.Label();
            this.lblActivation = new System.Windows.Forms.Label();
            this.lblDayTable = new System.Windows.Forms.Label();
            this.gridActivationDate = new System.Windows.Forms.DataGridView();
            this.gridDayTables = new System.Windows.Forms.DataGridView();
            this.btnTOUWrite = new System.Windows.Forms.Button();
            this.btnReadFutureTOU = new System.Windows.Forms.Button();
            this.btnReadCurrentTOU = new System.Windows.Forms.Button();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.lblLoadSurveyCapturePeriod = new System.Windows.Forms.Label();
            this.cmbBoxLSCapturePeriod = new System.Windows.Forms.ComboBox();
            this.btnReadLSCapturePeriod = new System.Windows.Forms.Button();
            this.btnWriteLSCapturePeriod = new System.Windows.Forms.Button();
            this.tabPageIntegrationPeriod = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.lblDemandIPSeconds = new System.Windows.Forms.Label();
            this.lblDemandIntegrationPeriod = new System.Windows.Forms.Label();
            this.cmbBoxIntegrationPeriod = new System.Windows.Forms.ComboBox();
            this.btnReadIntegrationPeriod = new System.Windows.Forms.Button();
            this.btnWriteIntegrationPeriod = new System.Windows.Forms.Button();
            this.tbCTPTRatio = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblPTProgramNotSupported = new System.Windows.Forms.Label();
            this.btnReadPTRatio = new System.Windows.Forms.Button();
            this.lblPTRatio = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.nudPTRatio = new System.Windows.Forms.NumericUpDown();
            this.gbCTPTRatio = new System.Windows.Forms.GroupBox();
            this.lblCTRatioMessage = new System.Windows.Forms.Label();
            this.btnCTRatio = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.lblCTPTRatio = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCTPTWrite = new System.Windows.Forms.Button();
            this.nudCTRatio = new System.Windows.Forms.NumericUpDown();
            this.tabMDReset = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.btnMDReset = new System.Windows.Forms.Button();
            this.chkMDReset = new System.Windows.Forms.CheckBox();
            this.tbPDisplayParameters = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlDisplayParams = new System.Windows.Forms.TabControl();
            this.tabPagePushButton = new System.Windows.Forms.TabPage();
            this.dGVPushDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabPageScrollButton = new System.Windows.Forms.TabPage();
            this.dGVScrollDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabPageHighResolution = new System.Windows.Forms.TabPage();
            this.dGVHighResolution = new System.Windows.Forms.DataGridView();
            this.tabPageDisplayTimeOut = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.chkAutoScrollTime = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtScrollTime = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtPushButtonTimeout = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtScrollResumeTime = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUpScroll = new System.Windows.Forms.Button();
            this.btnWriteDisplayParams = new System.Windows.Forms.Button();
            this.btnReadDisplayParams = new System.Windows.Forms.Button();
            this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.btnDownScroll = new System.Windows.Forms.Button();
            this.tabKVAH = new System.Windows.Forms.TabPage();
            this.lblKvahNotSupported = new System.Windows.Forms.Label();
            this.btnReadKVAhSelection = new System.Windows.Forms.Button();
            this.btnWriteKVAhSelection = new System.Windows.Forms.Button();
            this.groupBox59 = new System.Windows.Forms.GroupBox();
            this.chkKVAhLagLead = new System.Windows.Forms.RadioButton();
            this.chkKVAhLagOnly = new System.Windows.Forms.RadioButton();
            this.tabRS232Lock = new System.Windows.Forms.TabPage();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.tabCMRI = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btnGenerateFile = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnWriteAll = new System.Windows.Forms.Button();
            this.tabGSM = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.textBoxGSM = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPCBA = new System.Windows.Forms.TabPage();
            this.grdPCBA = new System.Windows.Forms.DataGridView();
            this.lblPCBAMeterID = new System.Windows.Forms.Label();
            this.lblDisplayMeterId = new System.Windows.Forms.Label();
            this.btnPCBAExport = new System.Windows.Forms.Button();
            this.btnPCBARead = new System.Windows.Forms.Button();
            this.tabMeterAccuracyCheck = new System.Windows.Forms.TabPage();
            this.lblNoMeterAccuracyCheck = new System.Windows.Forms.Label();
            this.btnAccuracyCheckCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbMeterAccuracyCheck = new System.Windows.Forms.GroupBox();
            this.lblReactiveLeadUnit = new System.Windows.Forms.Label();
            this.lblReactiveLagUnit = new System.Windows.Forms.Label();
            this.lblApparentEnergyUnit = new System.Windows.Forms.Label();
            this.lblActiveEnergyUnit = new System.Windows.Forms.Label();
            this.lblduration = new System.Windows.Forms.Label();
            this.txtkvarhLagInitial = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadDelta = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadFinal = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadInitial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTestduration = new System.Windows.Forms.ComboBox();
            this.lblTestDuration = new System.Windows.Forms.Label();
            this.txtkvarhLagDelta = new System.Windows.Forms.TextBox();
            this.txtkVAhDelta = new System.Windows.Forms.TextBox();
            this.txtkWhDelta = new System.Windows.Forms.TextBox();
            this.txtkvarhLagFinal = new System.Windows.Forms.TextBox();
            this.txtkVAhFinal = new System.Windows.Forms.TextBox();
            this.txtkWhFinal = new System.Windows.Forms.TextBox();
            this.txtkVAhInitial = new System.Windows.Forms.TextBox();
            this.txtkWhInitial = new System.Windows.Forms.TextBox();
            this.lblDelta = new System.Windows.Forms.Label();
            this.lblFinalReading = new System.Windows.Forms.Label();
            this.lblInitialReading = new System.Windows.Forms.Label();
            this.lblkvarh = new System.Windows.Forms.Label();
            this.lblkVAh = new System.Windows.Forms.Label();
            this.lblkWh = new System.Windows.Forms.Label();
            this.tabPhasor = new System.Windows.Forms.TabPage();
            this.lblPhasorNotSupported = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.lblAngelYRValue = new System.Windows.Forms.Label();
            this.lblAngleBRValue = new System.Windows.Forms.Label();
            this.lblAngleBwTwoValue = new System.Windows.Forms.Label();
            this.lblTotalPWFactorValue = new System.Windows.Forms.Label();
            this.lblRLagLeadValue = new System.Windows.Forms.Label();
            this.lblYLagLeadValue = new System.Windows.Forms.Label();
            this.lblYChannelValue = new System.Windows.Forms.Label();
            this.lblBChannelValue = new System.Windows.Forms.Label();
            this.lblBLagLeadValue = new System.Windows.Forms.Label();
            this.lblBPhaseKWDirValue = new System.Windows.Forms.Label();
            this.lblRChannelValue = new System.Windows.Forms.Label();
            this.lblRPhaseKWDirVAlue = new System.Windows.Forms.Label();
            this.lblYPhaseKWDirValue = new System.Windows.Forms.Label();
            this.lblFrequencyValue = new System.Windows.Forms.Label();
            this.lblYPhasePFValue = new System.Windows.Forms.Label();
            this.lblBPhaesPFValue = new System.Windows.Forms.Label();
            this.lblRPhasePFValue = new System.Windows.Forms.Label();
            this.lblPhaseSeqValue = new System.Windows.Forms.Label();
            this.lblReactivePowerValue = new System.Windows.Forms.Label();
            this.lblApparentPowerValue = new System.Windows.Forms.Label();
            this.lblBCurrentValue = new System.Windows.Forms.Label();
            this.lblActivePowerValue = new System.Windows.Forms.Label();
            this.lblRCurrentValue = new System.Windows.Forms.Label();
            this.lblYCurrentValue = new System.Windows.Forms.Label();
            this.lblYVoltageValue = new System.Windows.Forms.Label();
            this.lblBVoltageValue = new System.Windows.Forms.Label();
            this.lblRVoltageValue = new System.Windows.Forms.Label();
            this.lblAngelYR = new System.Windows.Forms.Label();
            this.lblAngleBR = new System.Windows.Forms.Label();
            this.lblAngleBwTwo = new System.Windows.Forms.Label();
            this.lblTotalPWFactor = new System.Windows.Forms.Label();
            this.lblRLagLead = new System.Windows.Forms.Label();
            this.lblYLagLead = new System.Windows.Forms.Label();
            this.lblYChannel = new System.Windows.Forms.Label();
            this.lblBChannel = new System.Windows.Forms.Label();
            this.lblBLagLead = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.lblBPhaseKWDir = new System.Windows.Forms.Label();
            this.lblRChannel = new System.Windows.Forms.Label();
            this.lblYPhasePF = new System.Windows.Forms.Label();
            this.lblBPhasePF = new System.Windows.Forms.Label();
            this.lblRPhaseKWDir = new System.Windows.Forms.Label();
            this.lblYPhaseKWDir = new System.Windows.Forms.Label();
            this.lblRPhasePF = new System.Windows.Forms.Label();
            this.lblPhaseSeq = new System.Windows.Forms.Label();
            this.lblReactivePower = new System.Windows.Forms.Label();
            this.lblApparentPower = new System.Windows.Forms.Label();
            this.lblBCurrent = new System.Windows.Forms.Label();
            this.lblActivePower = new System.Windows.Forms.Label();
            this.lblRCurrent = new System.Windows.Forms.Label();
            this.lblYCurrent = new System.Windows.Forms.Label();
            this.lblYVoltage = new System.Windows.Forms.Label();
            this.lblBVoltage = new System.Windows.Forms.Label();
            this.lblRPhaseVoltage = new System.Windows.Forms.Label();
            this.lblPhasorData = new System.Windows.Forms.Label();
            this.btnCancelPhasor = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.btnReadPhasor = new System.Windows.Forms.Button();
            this.phasorDiagram1 = new CAB.UI.Controls.PhasorDiagram();
            this.tabPageCompartment4 = new System.Windows.Forms.TabPage();
            this.btnAutoConfigModem = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox35 = new System.Windows.Forms.GroupBox();
            this.gbPort = new System.Windows.Forms.GroupBox();
            this.panelCommunicationType = new System.Windows.Forms.Panel();
            this.rdGPRS = new System.Windows.Forms.RadioButton();
            this.rdPSTN = new System.Windows.Forms.RadioButton();
            this.rdGSM = new System.Windows.Forms.RadioButton();
            this.rdDirect = new System.Windows.Forms.RadioButton();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.dgvPortUsageAssociation = new System.Windows.Forms.DataGridView();
            this.colPortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortUsageTypeModem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPortUsageTypeCMRI = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.rdbMultiplePorts = new System.Windows.Forms.RadioButton();
            this.rdbSinglePort = new System.Windows.Forms.RadioButton();
            this.groupBox65 = new System.Windows.Forms.GroupBox();
            this.groupBox69 = new System.Windows.Forms.GroupBox();
            this.rdBtnRJPort = new System.Windows.Forms.RadioButton();
            this.rdBtnOpticalPort = new System.Windows.Forms.RadioButton();
            this.groupBox70 = new System.Windows.Forms.GroupBox();
            this.rdBtnModeE = new System.Windows.Forms.RadioButton();
            this.rdBtnDirectHDLC = new System.Windows.Forms.RadioButton();
            this.cmbAvailableSerialPort = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.cmbCMRIType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox67 = new System.Windows.Forms.GroupBox();
            this.label109 = new System.Windows.Forms.Label();
            this.label110 = new System.Windows.Forms.Label();
            this.label204 = new System.Windows.Forms.Label();
            this.txtGSMInterFrameTime = new System.Windows.Forms.TextBox();
            this.txtBoxInterFrameTime = new System.Windows.Forms.TextBox();
            this.txtResponseTimeOut = new System.Windows.Forms.TextBox();
            this.label205 = new System.Windows.Forms.Label();
            this.label206 = new System.Windows.Forms.Label();
            this.label207 = new System.Windows.Forms.Label();
            this.groupBox66 = new System.Windows.Forms.GroupBox();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.label108 = new System.Windows.Forms.Label();
            this.groupBox68 = new System.Windows.Forms.GroupBox();
            this.txtServerLowerMacAddress = new System.Windows.Forms.TextBox();
            this.txtServerSAP = new System.Windows.Forms.TextBox();
            this.label208 = new System.Windows.Forms.Label();
            this.label209 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtBoxScaleXML = new System.Windows.Forms.TextBox();
            this.label99 = new System.Windows.Forms.Label();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.txtHLSPwd = new System.Windows.Forms.TextBox();
            this.labelHLS = new System.Windows.Forms.Label();
            this.txtPWD = new System.Windows.Forms.TextBox();
            this.labelPwd = new System.Windows.Forms.Label();
            this.groupBox63 = new System.Windows.Forms.GroupBox();
            this.cmbSecurity = new System.Windows.Forms.ComboBox();
            this.cmbContext = new System.Windows.Forms.ComboBox();
            this.label106 = new System.Windows.Forms.Label();
            this.label107 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.timerRTC = new System.Windows.Forms.Timer(this.components);
            this.errpPortMapping = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkOtherCMRI = new System.Windows.Forms.CheckBox();
            this.chkNameplateCMRI = new System.Windows.Forms.CheckBox();
            this.chkTamperCMRI = new System.Windows.Forms.CheckBox();
            this.chkLoadSurveyCMRI = new System.Windows.Forms.CheckBox();
            this.chkBillingCMRI = new System.Windows.Forms.CheckBox();
            this.chkInstaCMRI = new System.Windows.Forms.CheckBox();
            this.Duration_Timer = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.chkBoxAutoScrollTime = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btnReadDisplayParamsTimeout = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.txtBoxAutoScrollTime = new System.Windows.Forms.TextBox();
            this.txtBoxPushTimeout = new System.Windows.Forms.TextBox();
            this.txtBoxScrollTime = new System.Windows.Forms.TextBox();
            this.btnWriteDisplayParamsTimeout = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageCompartment1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpBoxDLMSRead.SuspendLayout();
            this.grpBoxLS.SuspendLayout();
            this.grpBoxEventLog.SuspendLayout();
            this.grpBoxBillingHistory.SuspendLayout();
            this.tabPageCompartment2.SuspendLayout();
            this.tabControlCMRI.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grpPartialRead.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabProgramming.SuspendLayout();
            this.tabCTPTRatio.SuspendLayout();
            this.tabPageRTC.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVReadRTC)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.tabPageTOUConfiguration.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageSeason1.SuspendLayout();
            this.grpDayTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay4)).BeginInit();
            this.tabPageSeason2.SuspendLayout();
            this.groupBox26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay10)).BeginInit();
            this.tabPageSeason3.SuspendLayout();
            this.groupBox27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay16)).BeginInit();
            this.tabPageSeason4.SuspendLayout();
            this.groupBox28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay22)).BeginInit();
            this.groupBox25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridActivationDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDayTables)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.tabPageIntegrationPeriod.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.tbCTPTRatio.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPTRatio)).BeginInit();
            this.gbCTPTRatio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCTRatio)).BeginInit();
            this.tabMDReset.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.tbPDisplayParameters.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tabControlDisplayParams.SuspendLayout();
            this.tabPagePushButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVPushDisplayParams)).BeginInit();
            this.tabPageScrollButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVScrollDisplayParams)).BeginInit();
            this.tabPageHighResolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVHighResolution)).BeginInit();
            this.tabPageDisplayTimeOut.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabKVAH.SuspendLayout();
            this.groupBox59.SuspendLayout();
            this.tabRS232Lock.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabCMRI.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabGSM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPCBA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPCBA)).BeginInit();
            this.tabMeterAccuracyCheck.SuspendLayout();
            this.gbMeterAccuracyCheck.SuspendLayout();
            this.tabPhasor.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tabPageCompartment4.SuspendLayout();
            this.groupBox35.SuspendLayout();
            this.gbPort.SuspendLayout();
            this.panelCommunicationType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).BeginInit();
            this.groupBox65.SuspendLayout();
            this.groupBox69.SuspendLayout();
            this.groupBox70.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox67.SuspendLayout();
            this.groupBox66.SuspendLayout();
            this.groupBox68.SuspendLayout();
            this.groupBox36.SuspendLayout();
            this.groupBox63.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.485785F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.87889F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.61938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.688581F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.61938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.07266F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.609F));
            this.tableLayoutPanel1.Controls.Add(this.lblMeterID, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblMeterID
            // 
            this.lblMeterID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMeterID.AutoSize = true;
            this.lblMeterID.Location = new System.Drawing.Point(4, 1);
            this.lblMeterID.Name = "lblMeterID";
            this.lblMeterID.Size = new System.Drawing.Size(10, 98);
            this.lblMeterID.TabIndex = 0;
            this.lblMeterID.Text = "Meter ID";
            // 
            // lblMeterIDVal
            // 
            this.lblMeterIDVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMeterIDVal.AutoSize = true;
            this.lblMeterIDVal.Location = new System.Drawing.Point(46, 11);
            this.lblMeterIDVal.Name = "lblMeterIDVal";
            this.lblMeterIDVal.Size = new System.Drawing.Size(49, 13);
            this.lblMeterIDVal.TabIndex = 1;
            this.lblMeterIDVal.Text = "1002001";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLblSettings,
            this.toolStripProgressBar1,
            this.toolstripStatus,
            this.dataDownloadStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 862);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1231, 25);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(171, 20);
            this.toolStripStatusLabel1.Text = "Communication Settings";
            // 
            // toolStripStatusLblSettings
            // 
            this.toolStripStatusLblSettings.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLblSettings.Name = "toolStripStatusLblSettings";
            this.toolStripStatusLblSettings.Size = new System.Drawing.Size(180, 20);
            this.toolStripStatusLblSettings.Text = "toolStripStatusLblSettings";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 1;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(133, 20);
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolstripStatus
            // 
            this.toolstripStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolstripStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.toolstripStatus.Name = "toolstripStatus";
            this.toolstripStatus.Size = new System.Drawing.Size(0, 20);
            // 
            // dataDownloadStatus
            // 
            this.dataDownloadStatus.Name = "dataDownloadStatus";
            this.dataDownloadStatus.Size = new System.Drawing.Size(0, 20);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageCompartment1);
            this.tabControlMain.Controls.Add(this.tabPageCompartment2);
            this.tabControlMain.Controls.Add(this.tabProgramming);
            this.tabControlMain.Controls.Add(this.tabCMRI);
            this.tabControlMain.Controls.Add(this.tabGSM);
            this.tabControlMain.Controls.Add(this.tabPCBA);
            this.tabControlMain.Controls.Add(this.tabMeterAccuracyCheck);
            this.tabControlMain.Controls.Add(this.tabPhasor);
            this.tabControlMain.Controls.Add(this.tabPageCompartment4);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1231, 862);
            this.tabControlMain.TabIndex = 17;
            this.tabControlMain.TabIndexChanged += new System.EventHandler(this.tabControlMain_TabIndexChanged);
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageCompartment1
            // 
            this.tabPageCompartment1.AutoScroll = true;
            this.tabPageCompartment1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPageCompartment1.Controls.Add(this.groupBox4);
            this.tabPageCompartment1.Controls.Add(this.btnCancel);
            this.tabPageCompartment1.Controls.Add(this.grpBoxLS);
            this.tabPageCompartment1.Controls.Add(this.btnReadAll);
            this.tabPageCompartment1.Controls.Add(this.grpBoxEventLog);
            this.tabPageCompartment1.Controls.Add(this.grpBoxBillingHistory);
            this.tabPageCompartment1.Location = new System.Drawing.Point(4, 25);
            this.tabPageCompartment1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment1.Name = "tabPageCompartment1";
            this.tabPageCompartment1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment1.Size = new System.Drawing.Size(1223, 833);
            this.tabPageCompartment1.TabIndex = 0;
            this.tabPageCompartment1.Text = "Read Meter";
            this.tabPageCompartment1.UseVisualStyleBackColor = true;
            this.tabPageCompartment1.Click += new System.EventHandler(this.tabPageCompartment1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.grpBoxDLMSRead);
            this.groupBox4.Location = new System.Drawing.Point(73, 26);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(249, 569);
            this.groupBox4.TabIndex = 40;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdFastDownload);
            this.groupBox5.Controls.Add(this.chkNormalDownload);
            this.groupBox5.Location = new System.Drawing.Point(8, 22);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(233, 94);
            this.groupBox5.TabIndex = 33;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Download";
            // 
            // rdFastDownload
            // 
            this.rdFastDownload.AutoSize = true;
            this.rdFastDownload.Location = new System.Drawing.Point(37, 55);
            this.rdFastDownload.Margin = new System.Windows.Forms.Padding(4);
            this.rdFastDownload.Name = "rdFastDownload";
            this.rdFastDownload.Size = new System.Drawing.Size(56, 21);
            this.rdFastDownload.TabIndex = 1;
            this.rdFastDownload.Text = "Fast";
            this.rdFastDownload.UseVisualStyleBackColor = true;
            this.rdFastDownload.CheckedChanged += new System.EventHandler(this.rdFastDownload_CheckedChanged);
            // 
            // chkNormalDownload
            // 
            this.chkNormalDownload.AutoSize = true;
            this.chkNormalDownload.Checked = true;
            this.chkNormalDownload.Location = new System.Drawing.Point(37, 22);
            this.chkNormalDownload.Margin = new System.Windows.Forms.Padding(4);
            this.chkNormalDownload.Name = "chkNormalDownload";
            this.chkNormalDownload.Size = new System.Drawing.Size(74, 21);
            this.chkNormalDownload.TabIndex = 0;
            this.chkNormalDownload.TabStop = true;
            this.chkNormalDownload.Text = "Normal";
            this.chkNormalDownload.UseVisualStyleBackColor = true;
            this.chkNormalDownload.CheckedChanged += new System.EventHandler(this.chkNormalDownload_CheckedChanged);
            // 
            // grpBoxDLMSRead
            // 
            this.grpBoxDLMSRead.Controls.Add(this.chkPhasor);
            this.grpBoxDLMSRead.Controls.Add(this.chkMidnightData);
            this.grpBoxDLMSRead.Controls.Add(this.chkOther);
            this.grpBoxDLMSRead.Controls.Add(this.chkNameplate);
            this.grpBoxDLMSRead.Controls.Add(this.chkTamper);
            this.grpBoxDLMSRead.Controls.Add(this.chkLoadSurvey);
            this.grpBoxDLMSRead.Controls.Add(this.chkBilling);
            this.grpBoxDLMSRead.Controls.Add(this.chkInsta);
            this.grpBoxDLMSRead.Location = new System.Drawing.Point(8, 114);
            this.grpBoxDLMSRead.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoxDLMSRead.Name = "grpBoxDLMSRead";
            this.grpBoxDLMSRead.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoxDLMSRead.Size = new System.Drawing.Size(233, 447);
            this.grpBoxDLMSRead.TabIndex = 32;
            this.grpBoxDLMSRead.TabStop = false;
            // 
            // chkPhasor
            // 
            this.chkPhasor.AutoSize = true;
            this.chkPhasor.Location = new System.Drawing.Point(33, 415);
            this.chkPhasor.Margin = new System.Windows.Forms.Padding(4);
            this.chkPhasor.Name = "chkPhasor";
            this.chkPhasor.Size = new System.Drawing.Size(75, 21);
            this.chkPhasor.TabIndex = 40;
            this.chkPhasor.Text = "Phasor";
            this.chkPhasor.UseVisualStyleBackColor = true;
            this.chkPhasor.Visible = false;
            this.chkPhasor.CheckedChanged += new System.EventHandler(this.chkPhasor_CheckedChanged);
            // 
            // chkMidnightData
            // 
            this.chkMidnightData.AutoSize = true;
            this.chkMidnightData.Checked = true;
            this.chkMidnightData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMidnightData.Location = new System.Drawing.Point(33, 374);
            this.chkMidnightData.Margin = new System.Windows.Forms.Padding(4);
            this.chkMidnightData.Name = "chkMidnightData";
            this.chkMidnightData.Size = new System.Drawing.Size(117, 21);
            this.chkMidnightData.TabIndex = 39;
            this.chkMidnightData.Text = "Midnight Data";
            this.chkMidnightData.UseVisualStyleBackColor = true;
            this.chkMidnightData.CheckedChanged += new System.EventHandler(this.chkMidnightData_CheckedChanged);
            // 
            // chkOther
            // 
            this.chkOther.AutoSize = true;
            this.chkOther.Checked = true;
            this.chkOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOther.Location = new System.Drawing.Point(33, 44);
            this.chkOther.Margin = new System.Windows.Forms.Padding(4);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(98, 21);
            this.chkOther.TabIndex = 37;
            this.chkOther.Text = "Select All";
            this.chkOther.UseVisualStyleBackColor = true;
            this.chkOther.CheckedChanged += new System.EventHandler(this.chkOther_CheckedChanged);
            // 
            // chkNameplate
            // 
            this.chkNameplate.AutoSize = true;
            this.chkNameplate.Checked = true;
            this.chkNameplate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNameplate.Enabled = false;
            this.chkNameplate.Location = new System.Drawing.Point(33, 334);
            this.chkNameplate.Margin = new System.Windows.Forms.Padding(4);
            this.chkNameplate.Name = "chkNameplate";
            this.chkNameplate.Size = new System.Drawing.Size(81, 21);
            this.chkNameplate.TabIndex = 36;
            this.chkNameplate.Text = "General";
            this.chkNameplate.UseVisualStyleBackColor = true;
            // 
            // chkTamper
            // 
            this.chkTamper.AutoSize = true;
            this.chkTamper.Checked = true;
            this.chkTamper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTamper.Location = new System.Drawing.Point(32, 276);
            this.chkTamper.Margin = new System.Windows.Forms.Padding(4);
            this.chkTamper.Name = "chkTamper";
            this.chkTamper.Size = new System.Drawing.Size(89, 21);
            this.chkTamper.TabIndex = 35;
            this.chkTamper.Text = "Event log";
            this.chkTamper.UseVisualStyleBackColor = true;
            this.chkTamper.CheckedChanged += new System.EventHandler(this.chkTamper_CheckedChanged);
            // 
            // chkLoadSurvey
            // 
            this.chkLoadSurvey.AutoSize = true;
            this.chkLoadSurvey.Checked = true;
            this.chkLoadSurvey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoadSurvey.Location = new System.Drawing.Point(32, 218);
            this.chkLoadSurvey.Margin = new System.Windows.Forms.Padding(4);
            this.chkLoadSurvey.Name = "chkLoadSurvey";
            this.chkLoadSurvey.Size = new System.Drawing.Size(110, 21);
            this.chkLoadSurvey.TabIndex = 34;
            this.chkLoadSurvey.Text = "Load Survey";
            this.chkLoadSurvey.UseVisualStyleBackColor = true;
            this.chkLoadSurvey.CheckedChanged += new System.EventHandler(this.chkLoadSurvey_CheckedChanged);
            // 
            // chkBilling
            // 
            this.chkBilling.AutoSize = true;
            this.chkBilling.Checked = true;
            this.chkBilling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBilling.Location = new System.Drawing.Point(32, 160);
            this.chkBilling.Margin = new System.Windows.Forms.Padding(4);
            this.chkBilling.Name = "chkBilling";
            this.chkBilling.Size = new System.Drawing.Size(115, 21);
            this.chkBilling.TabIndex = 33;
            this.chkBilling.Text = "Billing History";
            this.chkBilling.UseVisualStyleBackColor = true;
            this.chkBilling.CheckedChanged += new System.EventHandler(this.chkBilling_CheckedChanged);
            // 
            // chkInsta
            // 
            this.chkInsta.AutoSize = true;
            this.chkInsta.Checked = true;
            this.chkInsta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInsta.Location = new System.Drawing.Point(32, 102);
            this.chkInsta.Margin = new System.Windows.Forms.Padding(4);
            this.chkInsta.Name = "chkInsta";
            this.chkInsta.Size = new System.Drawing.Size(119, 21);
            this.chkInsta.TabIndex = 32;
            this.chkInsta.Text = "Instantaneous";
            this.chkInsta.UseVisualStyleBackColor = true;
            this.chkInsta.CheckedChanged += new System.EventHandler(this.chkInsta_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1016, 526);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 32);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpBoxLS
            // 
            this.grpBoxLS.Controls.Add(this.cmbLSDays);
            this.grpBoxLS.Controls.Add(this.rdBtnReadCompleteLS);
            this.grpBoxLS.Controls.Add(this.rdBtnReadBetweenLS);
            this.grpBoxLS.Controls.Add(this.dtPickerFrom);
            this.grpBoxLS.Controls.Add(this.label5);
            this.grpBoxLS.Controls.Add(this.dtPickerTo);
            this.grpBoxLS.Location = new System.Drawing.Point(343, 183);
            this.grpBoxLS.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoxLS.Name = "grpBoxLS";
            this.grpBoxLS.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoxLS.Size = new System.Drawing.Size(792, 112);
            this.grpBoxLS.TabIndex = 36;
            this.grpBoxLS.TabStop = false;
            this.grpBoxLS.Text = "Load Survey Read Options";
            // 
            // cmbLSDays
            // 
            this.cmbLSDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLSDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLSDays.FormattingEnabled = true;
            this.cmbLSDays.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50"});
            this.cmbLSDays.Location = new System.Drawing.Point(200, 28);
            this.cmbLSDays.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLSDays.Name = "cmbLSDays";
            this.cmbLSDays.Size = new System.Drawing.Size(65, 24);
            this.cmbLSDays.TabIndex = 19;
            this.cmbLSDays.Visible = false;
            // 
            // rdBtnReadCompleteLS
            // 
            this.rdBtnReadCompleteLS.AutoSize = true;
            this.rdBtnReadCompleteLS.Checked = true;
            this.rdBtnReadCompleteLS.Location = new System.Drawing.Point(43, 30);
            this.rdBtnReadCompleteLS.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadCompleteLS.Name = "rdBtnReadCompleteLS";
            this.rdBtnReadCompleteLS.Size = new System.Drawing.Size(124, 21);
            this.rdBtnReadCompleteLS.TabIndex = 14;
            this.rdBtnReadCompleteLS.TabStop = true;
            this.rdBtnReadCompleteLS.Text = "Read complete";
            this.rdBtnReadCompleteLS.UseVisualStyleBackColor = true;
            this.rdBtnReadCompleteLS.CheckedChanged += new System.EventHandler(this.rdBtnReadCompleteLS_CheckedChanged);
            // 
            // rdBtnReadBetweenLS
            // 
            this.rdBtnReadBetweenLS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdBtnReadBetweenLS.AutoSize = true;
            this.rdBtnReadBetweenLS.Location = new System.Drawing.Point(43, 58);
            this.rdBtnReadBetweenLS.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadBetweenLS.Name = "rdBtnReadBetweenLS";
            this.rdBtnReadBetweenLS.Size = new System.Drawing.Size(138, 21);
            this.rdBtnReadBetweenLS.TabIndex = 16;
            this.rdBtnReadBetweenLS.Text = "Read profile from";
            this.rdBtnReadBetweenLS.UseVisualStyleBackColor = true;
            this.rdBtnReadBetweenLS.CheckedChanged += new System.EventHandler(this.rdBtnReadBetweenLS_CheckedChanged);
            // 
            // dtPickerFrom
            // 
            this.dtPickerFrom.Checked = false;
            this.dtPickerFrom.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtPickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerFrom.Location = new System.Drawing.Point(200, 58);
            this.dtPickerFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtPickerFrom.Name = "dtPickerFrom";
            this.dtPickerFrom.Size = new System.Drawing.Size(203, 22);
            this.dtPickerFrom.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(425, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "To";
            // 
            // dtPickerTo
            // 
            this.dtPickerTo.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtPickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerTo.Location = new System.Drawing.Point(464, 58);
            this.dtPickerTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtPickerTo.Name = "dtPickerTo";
            this.dtPickerTo.Size = new System.Drawing.Size(200, 22);
            this.dtPickerTo.TabIndex = 18;
            // 
            // btnReadAll
            // 
            this.btnReadAll.Enabled = false;
            this.btnReadAll.Location = new System.Drawing.Point(885, 526);
            this.btnReadAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadAll.Name = "btnReadAll";
            this.btnReadAll.Size = new System.Drawing.Size(123, 32);
            this.btnReadAll.TabIndex = 34;
            this.btnReadAll.Text = "Read";
            this.btnReadAll.UseVisualStyleBackColor = true;
            this.btnReadAll.Click += new System.EventHandler(this.btnReadAll_Click);
            // 
            // grpBoxEventLog
            // 
            this.grpBoxEventLog.Controls.Add(this.rdBtnReadBetweenEvent);
            this.grpBoxEventLog.Controls.Add(this.rdBtnReadLastEvent);
            this.grpBoxEventLog.Controls.Add(this.rdBtnReadCompleteEvent);
            this.grpBoxEventLog.Controls.Add(this.cmbBoxFromEvent);
            this.grpBoxEventLog.Controls.Add(this.cmbBoxLastFromEvent);
            this.grpBoxEventLog.Controls.Add(this.label6);
            this.grpBoxEventLog.Controls.Add(this.label7);
            this.grpBoxEventLog.Controls.Add(this.label8);
            this.grpBoxEventLog.Controls.Add(this.cmbBoxToEvent);
            this.grpBoxEventLog.Controls.Add(this.label9);
            this.grpBoxEventLog.Location = new System.Drawing.Point(343, 313);
            this.grpBoxEventLog.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoxEventLog.Name = "grpBoxEventLog";
            this.grpBoxEventLog.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoxEventLog.Size = new System.Drawing.Size(792, 151);
            this.grpBoxEventLog.TabIndex = 37;
            this.grpBoxEventLog.TabStop = false;
            this.grpBoxEventLog.Text = "Event log Read Options";
            // 
            // rdBtnReadBetweenEvent
            // 
            this.rdBtnReadBetweenEvent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdBtnReadBetweenEvent.AutoSize = true;
            this.rdBtnReadBetweenEvent.Location = new System.Drawing.Point(43, 106);
            this.rdBtnReadBetweenEvent.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadBetweenEvent.Name = "rdBtnReadBetweenEvent";
            this.rdBtnReadBetweenEvent.Size = new System.Drawing.Size(138, 21);
            this.rdBtnReadBetweenEvent.TabIndex = 13;
            this.rdBtnReadBetweenEvent.Text = "Read profile from";
            this.rdBtnReadBetweenEvent.UseVisualStyleBackColor = true;
            this.rdBtnReadBetweenEvent.CheckedChanged += new System.EventHandler(this.rdBtnReadBetweenEvent_CheckedChanged);
            // 
            // rdBtnReadLastEvent
            // 
            this.rdBtnReadLastEvent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdBtnReadLastEvent.AutoSize = true;
            this.rdBtnReadLastEvent.Location = new System.Drawing.Point(43, 66);
            this.rdBtnReadLastEvent.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadLastEvent.Name = "rdBtnReadLastEvent";
            this.rdBtnReadLastEvent.Size = new System.Drawing.Size(93, 21);
            this.rdBtnReadLastEvent.TabIndex = 9;
            this.rdBtnReadLastEvent.Text = "Read last ";
            this.rdBtnReadLastEvent.UseVisualStyleBackColor = true;
            this.rdBtnReadLastEvent.CheckedChanged += new System.EventHandler(this.rdBtnReadLastEvent_CheckedChanged);
            // 
            // rdBtnReadCompleteEvent
            // 
            this.rdBtnReadCompleteEvent.AutoSize = true;
            this.rdBtnReadCompleteEvent.Checked = true;
            this.rdBtnReadCompleteEvent.Location = new System.Drawing.Point(41, 33);
            this.rdBtnReadCompleteEvent.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadCompleteEvent.Name = "rdBtnReadCompleteEvent";
            this.rdBtnReadCompleteEvent.Size = new System.Drawing.Size(124, 21);
            this.rdBtnReadCompleteEvent.TabIndex = 8;
            this.rdBtnReadCompleteEvent.TabStop = true;
            this.rdBtnReadCompleteEvent.Text = "Read complete";
            this.rdBtnReadCompleteEvent.UseVisualStyleBackColor = true;
            this.rdBtnReadCompleteEvent.CheckedChanged += new System.EventHandler(this.rdBtnReadCompleteEvent_CheckedChanged);
            // 
            // cmbBoxFromEvent
            // 
            this.cmbBoxFromEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxFromEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxFromEvent.FormattingEnabled = true;
            this.cmbBoxFromEvent.Location = new System.Drawing.Point(200, 106);
            this.cmbBoxFromEvent.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxFromEvent.Name = "cmbBoxFromEvent";
            this.cmbBoxFromEvent.Size = new System.Drawing.Size(85, 24);
            this.cmbBoxFromEvent.TabIndex = 14;
            // 
            // cmbBoxLastFromEvent
            // 
            this.cmbBoxLastFromEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxLastFromEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLastFromEvent.FormattingEnabled = true;
            this.cmbBoxLastFromEvent.Location = new System.Drawing.Point(200, 63);
            this.cmbBoxLastFromEvent.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxLastFromEvent.Name = "cmbBoxLastFromEvent";
            this.cmbBoxLastFromEvent.Size = new System.Drawing.Size(85, 24);
            this.cmbBoxLastFromEvent.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(295, 111);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 16;
            this.label6.Text = "Event";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(295, 66);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Event(s)";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(436, 111);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "To";
            // 
            // cmbBoxToEvent
            // 
            this.cmbBoxToEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxToEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxToEvent.FormattingEnabled = true;
            this.cmbBoxToEvent.Location = new System.Drawing.Point(481, 106);
            this.cmbBoxToEvent.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxToEvent.Name = "cmbBoxToEvent";
            this.cmbBoxToEvent.Size = new System.Drawing.Size(99, 24);
            this.cmbBoxToEvent.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(589, 111);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "Event";
            // 
            // grpBoxBillingHistory
            // 
            this.grpBoxBillingHistory.Controls.Add(this.rdBtnReadBetween);
            this.grpBoxBillingHistory.Controls.Add(this.rdBtnReadLast);
            this.grpBoxBillingHistory.Controls.Add(this.cmbBoxFrom);
            this.grpBoxBillingHistory.Controls.Add(this.cmbBoxLastFrom);
            this.grpBoxBillingHistory.Controls.Add(this.label2);
            this.grpBoxBillingHistory.Controls.Add(this.lblMonths);
            this.grpBoxBillingHistory.Controls.Add(this.label4);
            this.grpBoxBillingHistory.Controls.Add(this.cmbBoxTo);
            this.grpBoxBillingHistory.Controls.Add(this.label3);
            this.grpBoxBillingHistory.Controls.Add(this.rdBtnReadComplete);
            this.grpBoxBillingHistory.Location = new System.Drawing.Point(343, 26);
            this.grpBoxBillingHistory.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoxBillingHistory.Name = "grpBoxBillingHistory";
            this.grpBoxBillingHistory.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoxBillingHistory.Size = new System.Drawing.Size(792, 149);
            this.grpBoxBillingHistory.TabIndex = 35;
            this.grpBoxBillingHistory.TabStop = false;
            this.grpBoxBillingHistory.Text = "Billing history Read Options";
            // 
            // rdBtnReadBetween
            // 
            this.rdBtnReadBetween.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdBtnReadBetween.AutoSize = true;
            this.rdBtnReadBetween.Location = new System.Drawing.Point(31, 89);
            this.rdBtnReadBetween.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadBetween.Name = "rdBtnReadBetween";
            this.rdBtnReadBetween.Size = new System.Drawing.Size(138, 21);
            this.rdBtnReadBetween.TabIndex = 12;
            this.rdBtnReadBetween.Text = "Read profile from";
            this.rdBtnReadBetween.UseVisualStyleBackColor = true;
            this.rdBtnReadBetween.Visible = false;
            // 
            // rdBtnReadLast
            // 
            this.rdBtnReadLast.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdBtnReadLast.AutoSize = true;
            this.rdBtnReadLast.Location = new System.Drawing.Point(31, 87);
            this.rdBtnReadLast.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadLast.Name = "rdBtnReadLast";
            this.rdBtnReadLast.Size = new System.Drawing.Size(93, 21);
            this.rdBtnReadLast.TabIndex = 9;
            this.rdBtnReadLast.Text = "Read last ";
            this.rdBtnReadLast.UseVisualStyleBackColor = true;
            this.rdBtnReadLast.CheckedChanged += new System.EventHandler(this.rdBtnReadLast_CheckedChanged);
            // 
            // cmbBoxFrom
            // 
            this.cmbBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxFrom.Enabled = false;
            this.cmbBoxFrom.FormattingEnabled = true;
            this.cmbBoxFrom.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbBoxFrom.Location = new System.Drawing.Point(187, 106);
            this.cmbBoxFrom.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxFrom.Name = "cmbBoxFrom";
            this.cmbBoxFrom.Size = new System.Drawing.Size(64, 24);
            this.cmbBoxFrom.TabIndex = 15;
            this.cmbBoxFrom.Visible = false;
            // 
            // cmbBoxLastFrom
            // 
            this.cmbBoxLastFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxLastFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLastFrom.FormattingEnabled = true;
            this.cmbBoxLastFrom.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbBoxLastFrom.Location = new System.Drawing.Point(187, 86);
            this.cmbBoxLastFrom.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxLastFrom.Name = "cmbBoxLastFrom";
            this.cmbBoxLastFrom.Size = new System.Drawing.Size(64, 24);
            this.cmbBoxLastFrom.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Month";
            this.label2.Visible = false;
            // 
            // lblMonths
            // 
            this.lblMonths.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMonths.AutoSize = true;
            this.lblMonths.Location = new System.Drawing.Point(281, 90);
            this.lblMonths.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonths.Name = "lblMonths";
            this.lblMonths.Size = new System.Drawing.Size(64, 17);
            this.lblMonths.TabIndex = 11;
            this.lblMonths.Text = "Month(s)";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(381, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "To";
            this.label4.Visible = false;
            // 
            // cmbBoxTo
            // 
            this.cmbBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBoxTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxTo.FormattingEnabled = true;
            this.cmbBoxTo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbBoxTo.Location = new System.Drawing.Point(429, 90);
            this.cmbBoxTo.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxTo.Name = "cmbBoxTo";
            this.cmbBoxTo.Size = new System.Drawing.Size(65, 24);
            this.cmbBoxTo.TabIndex = 16;
            this.cmbBoxTo.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(525, 94);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Month";
            this.label3.Visible = false;
            // 
            // rdBtnReadComplete
            // 
            this.rdBtnReadComplete.AutoSize = true;
            this.rdBtnReadComplete.Checked = true;
            this.rdBtnReadComplete.Location = new System.Drawing.Point(31, 44);
            this.rdBtnReadComplete.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnReadComplete.Name = "rdBtnReadComplete";
            this.rdBtnReadComplete.Size = new System.Drawing.Size(124, 21);
            this.rdBtnReadComplete.TabIndex = 1;
            this.rdBtnReadComplete.TabStop = true;
            this.rdBtnReadComplete.Text = "Read complete";
            this.rdBtnReadComplete.UseVisualStyleBackColor = true;
            this.rdBtnReadComplete.CheckedChanged += new System.EventHandler(this.rdBtnReadComplete_CheckedChanged);
            // 
            // tabPageCompartment2
            // 
            this.tabPageCompartment2.AutoScroll = true;
            this.tabPageCompartment2.Controls.Add(this.tabControlCMRI);
            this.tabPageCompartment2.Location = new System.Drawing.Point(4, 25);
            this.tabPageCompartment2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment2.Name = "tabPageCompartment2";
            this.tabPageCompartment2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment2.Size = new System.Drawing.Size(1223, 833);
            this.tabPageCompartment2.TabIndex = 1;
            this.tabPageCompartment2.Text = "Read CMRI";
            this.tabPageCompartment2.UseVisualStyleBackColor = true;
            // 
            // tabControlCMRI
            // 
            this.tabControlCMRI.Controls.Add(this.tabPage1);
            this.tabControlCMRI.Controls.Add(this.tabPage2);
            this.tabControlCMRI.Location = new System.Drawing.Point(11, 7);
            this.tabControlCMRI.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlCMRI.Name = "tabControlCMRI";
            this.tabControlCMRI.SelectedIndex = 0;
            this.tabControlCMRI.Size = new System.Drawing.Size(1067, 604);
            this.tabControlCMRI.TabIndex = 44;
            this.tabControlCMRI.Enter += new System.EventHandler(this.tabControlCMRI_Enter);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstCMRIfile);
            this.tabPage1.Controls.Add(this.btnCMRICancel);
            this.tabPage1.Controls.Add(this.btnReadAllCMRI);
            this.tabPage1.Controls.Add(this.btnLoadList);
            this.tabPage1.Controls.Add(this.chkSelectAllMeters);
            this.tabPage1.Controls.Add(this.grpPartialRead);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1059, 575);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Direct read";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // lstCMRIfile
            // 
            this.lstCMRIfile.FormattingEnabled = true;
            this.lstCMRIfile.Location = new System.Drawing.Point(25, 23);
            this.lstCMRIfile.Margin = new System.Windows.Forms.Padding(4);
            this.lstCMRIfile.Name = "lstCMRIfile";
            this.lstCMRIfile.ScrollAlwaysVisible = true;
            this.lstCMRIfile.Size = new System.Drawing.Size(273, 429);
            this.lstCMRIfile.TabIndex = 47;
            // 
            // btnCMRICancel
            // 
            this.btnCMRICancel.Location = new System.Drawing.Point(651, 482);
            this.btnCMRICancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCMRICancel.Name = "btnCMRICancel";
            this.btnCMRICancel.Size = new System.Drawing.Size(123, 32);
            this.btnCMRICancel.TabIndex = 48;
            this.btnCMRICancel.Text = "Cancel";
            this.btnCMRICancel.UseVisualStyleBackColor = true;
            this.btnCMRICancel.Click += new System.EventHandler(this.btnCMRICancel_Click);
            // 
            // btnReadAllCMRI
            // 
            this.btnReadAllCMRI.Enabled = false;
            this.btnReadAllCMRI.Location = new System.Drawing.Point(520, 482);
            this.btnReadAllCMRI.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadAllCMRI.Name = "btnReadAllCMRI";
            this.btnReadAllCMRI.Size = new System.Drawing.Size(123, 32);
            this.btnReadAllCMRI.TabIndex = 45;
            this.btnReadAllCMRI.Text = "Read";
            this.btnReadAllCMRI.UseVisualStyleBackColor = true;
            this.btnReadAllCMRI.Click += new System.EventHandler(this.btnReadAllCMRI_Click);
            // 
            // btnLoadList
            // 
            this.btnLoadList.Location = new System.Drawing.Point(389, 482);
            this.btnLoadList.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(123, 32);
            this.btnLoadList.TabIndex = 46;
            this.btnLoadList.Text = "Load Meter List";
            this.btnLoadList.UseVisualStyleBackColor = true;
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // chkSelectAllMeters
            // 
            this.chkSelectAllMeters.AutoSize = true;
            this.chkSelectAllMeters.Checked = true;
            this.chkSelectAllMeters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAllMeters.Location = new System.Drawing.Point(25, 494);
            this.chkSelectAllMeters.Margin = new System.Windows.Forms.Padding(4);
            this.chkSelectAllMeters.Name = "chkSelectAllMeters";
            this.chkSelectAllMeters.Size = new System.Drawing.Size(88, 21);
            this.chkSelectAllMeters.TabIndex = 49;
            this.chkSelectAllMeters.Text = "Select All";
            this.chkSelectAllMeters.UseVisualStyleBackColor = true;
            this.chkSelectAllMeters.CheckedChanged += new System.EventHandler(this.chkSelectAllMeters_CheckedChanged);
            // 
            // grpPartialRead
            // 
            this.grpPartialRead.Controls.Add(this.chkCMRIPhasor);
            this.grpPartialRead.Controls.Add(this.chkCMRISelectAll);
            this.grpPartialRead.Controls.Add(this.chkCMRIMidnightData);
            this.grpPartialRead.Controls.Add(this.chkCMRINameplate);
            this.grpPartialRead.Controls.Add(this.chkCMRITamper);
            this.grpPartialRead.Controls.Add(this.chkCMRILoadSurvey);
            this.grpPartialRead.Controls.Add(this.chkCMRIBilling);
            this.grpPartialRead.Controls.Add(this.chkCMRIInstant);
            this.grpPartialRead.Location = new System.Drawing.Point(389, 14);
            this.grpPartialRead.Margin = new System.Windows.Forms.Padding(4);
            this.grpPartialRead.Name = "grpPartialRead";
            this.grpPartialRead.Padding = new System.Windows.Forms.Padding(4);
            this.grpPartialRead.Size = new System.Drawing.Size(223, 436);
            this.grpPartialRead.TabIndex = 44;
            this.grpPartialRead.TabStop = false;
            // 
            // chkCMRIPhasor
            // 
            this.chkCMRIPhasor.AutoSize = true;
            this.chkCMRIPhasor.Checked = true;
            this.chkCMRIPhasor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRIPhasor.Location = new System.Drawing.Point(47, 348);
            this.chkCMRIPhasor.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRIPhasor.Name = "chkCMRIPhasor";
            this.chkCMRIPhasor.Size = new System.Drawing.Size(75, 21);
            this.chkCMRIPhasor.TabIndex = 40;
            this.chkCMRIPhasor.Text = "Phasor";
            this.chkCMRIPhasor.UseVisualStyleBackColor = true;
            this.chkCMRIPhasor.CheckedChanged += new System.EventHandler(this.chkCMRIPhasor_CheckedChanged);
            // 
            // chkCMRISelectAll
            // 
            this.chkCMRISelectAll.AutoSize = true;
            this.chkCMRISelectAll.Checked = true;
            this.chkCMRISelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRISelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCMRISelectAll.Location = new System.Drawing.Point(47, 394);
            this.chkCMRISelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRISelectAll.Name = "chkCMRISelectAll";
            this.chkCMRISelectAll.Size = new System.Drawing.Size(88, 21);
            this.chkCMRISelectAll.TabIndex = 39;
            this.chkCMRISelectAll.Text = "Select All";
            this.chkCMRISelectAll.UseVisualStyleBackColor = true;
            this.chkCMRISelectAll.CheckedChanged += new System.EventHandler(this.chkCMRISelectAll_CheckedChanged);
            // 
            // chkCMRIMidnightData
            // 
            this.chkCMRIMidnightData.AutoSize = true;
            this.chkCMRIMidnightData.Checked = true;
            this.chkCMRIMidnightData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRIMidnightData.Location = new System.Drawing.Point(47, 300);
            this.chkCMRIMidnightData.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRIMidnightData.Name = "chkCMRIMidnightData";
            this.chkCMRIMidnightData.Size = new System.Drawing.Size(132, 21);
            this.chkCMRIMidnightData.TabIndex = 38;
            this.chkCMRIMidnightData.Text = "Midnight Energy";
            this.chkCMRIMidnightData.UseVisualStyleBackColor = true;
            this.chkCMRIMidnightData.CheckedChanged += new System.EventHandler(this.chkCMRIMidnightData_CheckedChanged);
            // 
            // chkCMRINameplate
            // 
            this.chkCMRINameplate.AutoSize = true;
            this.chkCMRINameplate.Checked = true;
            this.chkCMRINameplate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRINameplate.Enabled = false;
            this.chkCMRINameplate.Location = new System.Drawing.Point(47, 251);
            this.chkCMRINameplate.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRINameplate.Name = "chkCMRINameplate";
            this.chkCMRINameplate.Size = new System.Drawing.Size(81, 21);
            this.chkCMRINameplate.TabIndex = 36;
            this.chkCMRINameplate.Text = "General";
            this.chkCMRINameplate.UseVisualStyleBackColor = true;
            this.chkCMRINameplate.CheckedChanged += new System.EventHandler(this.chkCMRINameplate_CheckedChanged);
            // 
            // chkCMRITamper
            // 
            this.chkCMRITamper.AutoSize = true;
            this.chkCMRITamper.Checked = true;
            this.chkCMRITamper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRITamper.Location = new System.Drawing.Point(47, 196);
            this.chkCMRITamper.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRITamper.Name = "chkCMRITamper";
            this.chkCMRITamper.Size = new System.Drawing.Size(89, 21);
            this.chkCMRITamper.TabIndex = 35;
            this.chkCMRITamper.Text = "Event log";
            this.chkCMRITamper.UseVisualStyleBackColor = true;
            this.chkCMRITamper.CheckedChanged += new System.EventHandler(this.chkCMRITamper_CheckedChanged);
            // 
            // chkCMRILoadSurvey
            // 
            this.chkCMRILoadSurvey.AutoSize = true;
            this.chkCMRILoadSurvey.Checked = true;
            this.chkCMRILoadSurvey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRILoadSurvey.Location = new System.Drawing.Point(47, 142);
            this.chkCMRILoadSurvey.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRILoadSurvey.Name = "chkCMRILoadSurvey";
            this.chkCMRILoadSurvey.Size = new System.Drawing.Size(110, 21);
            this.chkCMRILoadSurvey.TabIndex = 34;
            this.chkCMRILoadSurvey.Text = "Load Survey";
            this.chkCMRILoadSurvey.UseVisualStyleBackColor = true;
            this.chkCMRILoadSurvey.CheckedChanged += new System.EventHandler(this.chkCMRILoadSurvey_CheckedChanged);
            // 
            // chkCMRIBilling
            // 
            this.chkCMRIBilling.AutoSize = true;
            this.chkCMRIBilling.Checked = true;
            this.chkCMRIBilling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRIBilling.Location = new System.Drawing.Point(47, 87);
            this.chkCMRIBilling.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRIBilling.Name = "chkCMRIBilling";
            this.chkCMRIBilling.Size = new System.Drawing.Size(115, 21);
            this.chkCMRIBilling.TabIndex = 33;
            this.chkCMRIBilling.Text = "Billing History";
            this.chkCMRIBilling.UseVisualStyleBackColor = true;
            this.chkCMRIBilling.CheckedChanged += new System.EventHandler(this.chkCMRIBilling_CheckedChanged);
            // 
            // chkCMRIInstant
            // 
            this.chkCMRIInstant.AutoSize = true;
            this.chkCMRIInstant.Checked = true;
            this.chkCMRIInstant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCMRIInstant.Location = new System.Drawing.Point(47, 33);
            this.chkCMRIInstant.Margin = new System.Windows.Forms.Padding(4);
            this.chkCMRIInstant.Name = "chkCMRIInstant";
            this.chkCMRIInstant.Size = new System.Drawing.Size(119, 21);
            this.chkCMRIInstant.TabIndex = 32;
            this.chkCMRIInstant.Text = "Instantaneous";
            this.chkCMRIInstant.UseVisualStyleBackColor = true;
            this.chkCMRIInstant.CheckedChanged += new System.EventHandler(this.chkCMRIInstant_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstFast);
            this.tabPage2.Controls.Add(this.chkFDSelectAll);
            this.tabPage2.Controls.Add(this.btnLoadMeterFD);
            this.tabPage2.Controls.Add(this.btnCancelFD);
            this.tabPage2.Controls.Add(this.btnFDRead);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1059, 575);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "FDL Read";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstFast
            // 
            this.lstFast.FormattingEnabled = true;
            this.lstFast.Location = new System.Drawing.Point(28, 23);
            this.lstFast.Margin = new System.Windows.Forms.Padding(4);
            this.lstFast.Name = "lstFast";
            this.lstFast.ScrollAlwaysVisible = true;
            this.lstFast.Size = new System.Drawing.Size(311, 361);
            this.lstFast.TabIndex = 59;
            // 
            // chkFDSelectAll
            // 
            this.chkFDSelectAll.AutoSize = true;
            this.chkFDSelectAll.Checked = true;
            this.chkFDSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFDSelectAll.Location = new System.Drawing.Point(28, 425);
            this.chkFDSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkFDSelectAll.Name = "chkFDSelectAll";
            this.chkFDSelectAll.Size = new System.Drawing.Size(88, 21);
            this.chkFDSelectAll.TabIndex = 58;
            this.chkFDSelectAll.Text = "Select All";
            this.chkFDSelectAll.UseVisualStyleBackColor = true;
            this.chkFDSelectAll.CheckedChanged += new System.EventHandler(this.chkFDSelectAll_CheckedChanged);
            // 
            // btnLoadMeterFD
            // 
            this.btnLoadMeterFD.Location = new System.Drawing.Point(393, 414);
            this.btnLoadMeterFD.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadMeterFD.Name = "btnLoadMeterFD";
            this.btnLoadMeterFD.Size = new System.Drawing.Size(123, 32);
            this.btnLoadMeterFD.TabIndex = 56;
            this.btnLoadMeterFD.Text = "Load Meter List";
            this.btnLoadMeterFD.UseVisualStyleBackColor = true;
            this.btnLoadMeterFD.Visible = false;
            this.btnLoadMeterFD.Click += new System.EventHandler(this.btnLoadMeterFD_Click);
            // 
            // btnCancelFD
            // 
            this.btnCancelFD.Location = new System.Drawing.Point(269, 417);
            this.btnCancelFD.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelFD.Name = "btnCancelFD";
            this.btnCancelFD.Size = new System.Drawing.Size(123, 32);
            this.btnCancelFD.TabIndex = 57;
            this.btnCancelFD.Text = "Cancel";
            this.btnCancelFD.UseVisualStyleBackColor = true;
            this.btnCancelFD.Click += new System.EventHandler(this.btnCancelFD_Click);
            // 
            // btnFDRead
            // 
            this.btnFDRead.Enabled = false;
            this.btnFDRead.Location = new System.Drawing.Point(137, 417);
            this.btnFDRead.Margin = new System.Windows.Forms.Padding(4);
            this.btnFDRead.Name = "btnFDRead";
            this.btnFDRead.Size = new System.Drawing.Size(123, 32);
            this.btnFDRead.TabIndex = 55;
            this.btnFDRead.Text = "Read";
            this.btnFDRead.UseVisualStyleBackColor = true;
            this.btnFDRead.Click += new System.EventHandler(this.btnFDRead_Click);
            // 
            // tabProgramming
            // 
            this.tabProgramming.AutoScroll = true;
            this.tabProgramming.Controls.Add(this.tabCTPTRatio);
            this.tabProgramming.Location = new System.Drawing.Point(4, 25);
            this.tabProgramming.Margin = new System.Windows.Forms.Padding(4);
            this.tabProgramming.Name = "tabProgramming";
            this.tabProgramming.Size = new System.Drawing.Size(1223, 833);
            this.tabProgramming.TabIndex = 5;
            this.tabProgramming.Text = "Programing Parameters";
            this.tabProgramming.UseVisualStyleBackColor = true;
            // 
            // tabCTPTRatio
            // 
            this.tabCTPTRatio.Controls.Add(this.tabPageRTC);
            this.tabCTPTRatio.Controls.Add(this.tabPage4);
            this.tabCTPTRatio.Controls.Add(this.tabPageTOUConfiguration);
            this.tabCTPTRatio.Controls.Add(this.tabPage7);
            this.tabCTPTRatio.Controls.Add(this.tabPageIntegrationPeriod);
            this.tabCTPTRatio.Controls.Add(this.tbCTPTRatio);
            this.tabCTPTRatio.Controls.Add(this.tabMDReset);
            this.tabCTPTRatio.Controls.Add(this.tbPDisplayParameters);
            this.tabCTPTRatio.Controls.Add(this.tabKVAH);
            this.tabCTPTRatio.Controls.Add(this.tabRS232Lock);
            this.tabCTPTRatio.Location = new System.Drawing.Point(0, 0);
            this.tabCTPTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.tabCTPTRatio.Name = "tabCTPTRatio";
            this.tabCTPTRatio.SelectedIndex = 0;
            this.tabCTPTRatio.Size = new System.Drawing.Size(1220, 828);
            this.tabCTPTRatio.TabIndex = 1;
            this.tabCTPTRatio.SelectedIndexChanged += new System.EventHandler(this.tabCTPTRatio_SelectedIndexChanged);
            // 
            // tabPageRTC
            // 
            this.tabPageRTC.AutoScroll = true;
            this.tabPageRTC.Controls.Add(this.tableLayoutPanel3);
            this.tabPageRTC.Location = new System.Drawing.Point(4, 25);
            this.tabPageRTC.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageRTC.Name = "tabPageRTC";
            this.tabPageRTC.Size = new System.Drawing.Size(1212, 799);
            this.tabPageRTC.TabIndex = 1;
            this.tabPageRTC.Text = "RTC";
            this.tabPageRTC.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.583333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.41666F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox9, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.16854F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.83146F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(913, 487);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox9
            // 
            this.groupBox9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox9.Controls.Add(this.dGVReadRTC);
            this.groupBox9.Controls.Add(this.btnReadRTC);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.txtBoxRTC);
            this.groupBox9.Controls.Add(this.btnWriteRTC);
            this.groupBox9.Location = new System.Drawing.Point(45, 27);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox9.Size = new System.Drawing.Size(863, 432);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Update RTC";
            // 
            // dGVReadRTC
            // 
            this.dGVReadRTC.AllowUserToAddRows = false;
            this.dGVReadRTC.AllowUserToDeleteRows = false;
            this.dGVReadRTC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVReadRTC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSNo,
            this.colRTC});
            this.dGVReadRTC.Location = new System.Drawing.Point(391, 106);
            this.dGVReadRTC.Margin = new System.Windows.Forms.Padding(4);
            this.dGVReadRTC.Name = "dGVReadRTC";
            this.dGVReadRTC.ReadOnly = true;
            this.dGVReadRTC.Size = new System.Drawing.Size(401, 295);
            this.dGVReadRTC.TabIndex = 9;
            // 
            // colSNo
            // 
            this.colSNo.HeaderText = "S.No.";
            this.colSNo.Name = "colSNo";
            this.colSNo.ReadOnly = true;
            this.colSNo.Width = 80;
            // 
            // colRTC
            // 
            this.colRTC.HeaderText = "Real Time Clock";
            this.colRTC.Name = "colRTC";
            this.colRTC.ReadOnly = true;
            this.colRTC.Width = 175;
            // 
            // btnReadRTC
            // 
            this.btnReadRTC.Location = new System.Drawing.Point(264, 266);
            this.btnReadRTC.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadRTC.Name = "btnReadRTC";
            this.btnReadRTC.Size = new System.Drawing.Size(95, 32);
            this.btnReadRTC.TabIndex = 8;
            this.btnReadRTC.Text = "Read";
            this.btnReadRTC.UseVisualStyleBackColor = true;
            this.btnReadRTC.Click += new System.EventHandler(this.btnReadRTC_Click);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(27, 56);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 17);
            this.label12.TabIndex = 4;
            this.label12.Text = "Real Time Clock";
            // 
            // txtBoxRTC
            // 
            this.txtBoxRTC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxRTC.Location = new System.Drawing.Point(155, 53);
            this.txtBoxRTC.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxRTC.MaxLength = 8;
            this.txtBoxRTC.Name = "txtBoxRTC";
            this.txtBoxRTC.Size = new System.Drawing.Size(204, 22);
            this.txtBoxRTC.TabIndex = 5;
            // 
            // btnWriteRTC
            // 
            this.btnWriteRTC.Location = new System.Drawing.Point(156, 266);
            this.btnWriteRTC.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteRTC.Name = "btnWriteRTC";
            this.btnWriteRTC.Size = new System.Drawing.Size(95, 32);
            this.btnWriteRTC.TabIndex = 6;
            this.btnWriteRTC.Text = "Write";
            this.btnWriteRTC.UseVisualStyleBackColor = true;
            this.btnWriteRTC.Click += new System.EventHandler(this.btnWriteRTC_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.tableLayoutPanel9);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1212, 799);
            this.tabPage4.TabIndex = 8;
            this.tabPage4.Text = "Billing Date & Time";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.AutoScroll = true;
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.583333F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.41666F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 490F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 397F));
            this.tableLayoutPanel9.Controls.Add(this.groupBox17, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.16854F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1212, 799);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.cmbBoxBillingMinute);
            this.groupBox17.Controls.Add(this.cmbBoxBillingHour);
            this.groupBox17.Controls.Add(this.cmbBoxBillingDate);
            this.groupBox17.Controls.Add(this.label23);
            this.groupBox17.Controls.Add(this.label24);
            this.groupBox17.Controls.Add(this.label25);
            this.groupBox17.Controls.Add(this.cmbBoxBillingPeriod);
            this.groupBox17.Controls.Add(this.btnReadBillingDatetime);
            this.groupBox17.Controls.Add(this.label26);
            this.groupBox17.Controls.Add(this.btnWriteBillingDatetime);
            this.groupBox17.Location = new System.Drawing.Point(37, 4);
            this.groupBox17.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox17.Size = new System.Drawing.Size(599, 452);
            this.groupBox17.TabIndex = 1;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Billing Date && Time";
            // 
            // cmbBoxBillingMinute
            // 
            this.cmbBoxBillingMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingMinute.Enabled = false;
            this.cmbBoxBillingMinute.FormattingEnabled = true;
            this.cmbBoxBillingMinute.Location = new System.Drawing.Point(236, 233);
            this.cmbBoxBillingMinute.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxBillingMinute.Name = "cmbBoxBillingMinute";
            this.cmbBoxBillingMinute.Size = new System.Drawing.Size(160, 24);
            this.cmbBoxBillingMinute.TabIndex = 3;
            // 
            // cmbBoxBillingHour
            // 
            this.cmbBoxBillingHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingHour.Enabled = false;
            this.cmbBoxBillingHour.FormattingEnabled = true;
            this.cmbBoxBillingHour.Location = new System.Drawing.Point(236, 188);
            this.cmbBoxBillingHour.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxBillingHour.Name = "cmbBoxBillingHour";
            this.cmbBoxBillingHour.Size = new System.Drawing.Size(160, 24);
            this.cmbBoxBillingHour.TabIndex = 2;
            // 
            // cmbBoxBillingDate
            // 
            this.cmbBoxBillingDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingDate.Enabled = false;
            this.cmbBoxBillingDate.FormattingEnabled = true;
            this.cmbBoxBillingDate.Location = new System.Drawing.Point(236, 144);
            this.cmbBoxBillingDate.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxBillingDate.Name = "cmbBoxBillingDate";
            this.cmbBoxBillingDate.Size = new System.Drawing.Size(160, 24);
            this.cmbBoxBillingDate.TabIndex = 1;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(80, 236);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(93, 17);
            this.label23.TabIndex = 11;
            this.label23.Text = "Select Minute";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(80, 192);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(82, 17);
            this.label24.TabIndex = 9;
            this.label24.Text = "Select Hour";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(80, 148);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(81, 17);
            this.label25.TabIndex = 10;
            this.label25.Text = "Select Date";
            // 
            // cmbBoxBillingPeriod
            // 
            this.cmbBoxBillingPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingPeriod.FormattingEnabled = true;
            this.cmbBoxBillingPeriod.Items.AddRange(new object[] {
            "End of Month",
            "User Defined"});
            this.cmbBoxBillingPeriod.Location = new System.Drawing.Point(236, 102);
            this.cmbBoxBillingPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxBillingPeriod.Name = "cmbBoxBillingPeriod";
            this.cmbBoxBillingPeriod.Size = new System.Drawing.Size(160, 24);
            this.cmbBoxBillingPeriod.TabIndex = 0;
            this.cmbBoxBillingPeriod.SelectedValueChanged += new System.EventHandler(this.cmbBoxBillingPeriod_SelectedValueChanged);
            // 
            // btnReadBillingDatetime
            // 
            this.btnReadBillingDatetime.Location = new System.Drawing.Point(344, 289);
            this.btnReadBillingDatetime.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadBillingDatetime.Name = "btnReadBillingDatetime";
            this.btnReadBillingDatetime.Size = new System.Drawing.Size(100, 32);
            this.btnReadBillingDatetime.TabIndex = 5;
            this.btnReadBillingDatetime.Text = "Read";
            this.btnReadBillingDatetime.UseVisualStyleBackColor = true;
            this.btnReadBillingDatetime.Click += new System.EventHandler(this.btnReadBillingDatetime_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(80, 112);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(133, 17);
            this.label26.TabIndex = 4;
            this.label26.Text = "Select Billing Period";
            // 
            // btnWriteBillingDatetime
            // 
            this.btnWriteBillingDatetime.Location = new System.Drawing.Point(236, 289);
            this.btnWriteBillingDatetime.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteBillingDatetime.Name = "btnWriteBillingDatetime";
            this.btnWriteBillingDatetime.Size = new System.Drawing.Size(100, 32);
            this.btnWriteBillingDatetime.TabIndex = 4;
            this.btnWriteBillingDatetime.Text = "Write";
            this.btnWriteBillingDatetime.UseVisualStyleBackColor = true;
            this.btnWriteBillingDatetime.Click += new System.EventHandler(this.btnWriteBillingDatetime_Click);
            // 
            // tabPageTOUConfiguration
            // 
            this.tabPageTOUConfiguration.Controls.Add(this.tableLayoutPanel16);
            this.tabPageTOUConfiguration.Location = new System.Drawing.Point(4, 25);
            this.tabPageTOUConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageTOUConfiguration.Name = "tabPageTOUConfiguration";
            this.tabPageTOUConfiguration.Size = new System.Drawing.Size(1212, 799);
            this.tabPageTOUConfiguration.TabIndex = 16;
            this.tabPageTOUConfiguration.Text = "TOU Configuration";
            this.tabPageTOUConfiguration.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.AutoScroll = true;
            this.tableLayoutPanel16.AutoSize = true;
            this.tableLayoutPanel16.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel16.ColumnCount = 3;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 672F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 613F));
            this.tableLayoutPanel16.Controls.Add(this.tabControl2, 0, 1);
            this.tableLayoutPanel16.Controls.Add(this.groupBox25, 2, 1);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 3;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 553F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 239F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(1378, 820);
            this.tableLayoutPanel16.TabIndex = 1;
            // 
            // tabControl2
            // 
            this.tableLayoutPanel16.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.tabPageSeason1);
            this.tabControl2.Controls.Add(this.tabPageSeason2);
            this.tabControl2.Controls.Add(this.tabPageSeason3);
            this.tabControl2.Controls.Add(this.tabPageSeason4);
            this.tabControl2.Location = new System.Drawing.Point(4, 32);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(757, 545);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPageSeason1
            // 
            this.tabPageSeason1.Controls.Add(this.grpDayTables);
            this.tabPageSeason1.Location = new System.Drawing.Point(4, 25);
            this.tabPageSeason1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSeason1.Name = "tabPageSeason1";
            this.tabPageSeason1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSeason1.Size = new System.Drawing.Size(749, 516);
            this.tabPageSeason1.TabIndex = 0;
            this.tabPageSeason1.Text = "Season 1";
            this.tabPageSeason1.UseVisualStyleBackColor = true;
            // 
            // grpDayTables
            // 
            this.grpDayTables.Controls.Add(this.gridTOUDay6);
            this.grpDayTables.Controls.Add(this.gridTOUDay5);
            this.grpDayTables.Controls.Add(this.lblDayTable6);
            this.grpDayTables.Controls.Add(this.lblDayTable5);
            this.grpDayTables.Controls.Add(this.lblDayTable4);
            this.grpDayTables.Controls.Add(this.lblDayTable3);
            this.grpDayTables.Controls.Add(this.lblDayTable2);
            this.grpDayTables.Controls.Add(this.lblDayTable1);
            this.grpDayTables.Controls.Add(this.gridTOUDay1);
            this.grpDayTables.Controls.Add(this.gridTOUDay2);
            this.grpDayTables.Controls.Add(this.gridTOUDay3);
            this.grpDayTables.Controls.Add(this.gridTOUDay4);
            this.grpDayTables.ForeColor = System.Drawing.Color.Black;
            this.grpDayTables.Location = new System.Drawing.Point(12, 6);
            this.grpDayTables.Margin = new System.Windows.Forms.Padding(4);
            this.grpDayTables.Name = "grpDayTables";
            this.grpDayTables.Padding = new System.Windows.Forms.Padding(4);
            this.grpDayTables.Size = new System.Drawing.Size(723, 501);
            this.grpDayTables.TabIndex = 4;
            this.grpDayTables.TabStop = false;
            // 
            // gridTOUDay6
            // 
            this.gridTOUDay6.AllowUserToAddRows = false;
            this.gridTOUDay6.AllowUserToDeleteRows = false;
            this.gridTOUDay6.AllowUserToResizeColumns = false;
            this.gridTOUDay6.AllowUserToResizeRows = false;
            this.gridTOUDay6.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay6.Location = new System.Drawing.Point(483, 287);
            this.gridTOUDay6.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay6.Name = "gridTOUDay6";
            this.gridTOUDay6.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay6.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay6.TabIndex = 9;
            this.gridTOUDay6.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay6.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay6.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay5
            // 
            this.gridTOUDay5.AllowUserToAddRows = false;
            this.gridTOUDay5.AllowUserToDeleteRows = false;
            this.gridTOUDay5.AllowUserToResizeColumns = false;
            this.gridTOUDay5.AllowUserToResizeRows = false;
            this.gridTOUDay5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay5.Location = new System.Drawing.Point(245, 287);
            this.gridTOUDay5.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay5.Name = "gridTOUDay5";
            this.gridTOUDay5.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay5.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay5.TabIndex = 8;
            this.gridTOUDay5.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay5.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay5.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // lblDayTable6
            // 
            this.lblDayTable6.AutoSize = true;
            this.lblDayTable6.Location = new System.Drawing.Point(555, 267);
            this.lblDayTable6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable6.Name = "lblDayTable6";
            this.lblDayTable6.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable6.TabIndex = 7;
            this.lblDayTable6.Text = "Day Table 6";
            // 
            // lblDayTable5
            // 
            this.lblDayTable5.AutoSize = true;
            this.lblDayTable5.Location = new System.Drawing.Point(317, 267);
            this.lblDayTable5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable5.Name = "lblDayTable5";
            this.lblDayTable5.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable5.TabIndex = 7;
            this.lblDayTable5.Text = "Day Table 5";
            // 
            // lblDayTable4
            // 
            this.lblDayTable4.AutoSize = true;
            this.lblDayTable4.Location = new System.Drawing.Point(80, 267);
            this.lblDayTable4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable4.Name = "lblDayTable4";
            this.lblDayTable4.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable4.TabIndex = 7;
            this.lblDayTable4.Text = "Day Table 4";
            // 
            // lblDayTable3
            // 
            this.lblDayTable3.AutoSize = true;
            this.lblDayTable3.Location = new System.Drawing.Point(555, 18);
            this.lblDayTable3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable3.Name = "lblDayTable3";
            this.lblDayTable3.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable3.TabIndex = 6;
            this.lblDayTable3.Text = "Day Table 3";
            // 
            // lblDayTable2
            // 
            this.lblDayTable2.AutoSize = true;
            this.lblDayTable2.Location = new System.Drawing.Point(317, 18);
            this.lblDayTable2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable2.Name = "lblDayTable2";
            this.lblDayTable2.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable2.TabIndex = 5;
            this.lblDayTable2.Text = "Day Table 2";
            // 
            // lblDayTable1
            // 
            this.lblDayTable1.AutoSize = true;
            this.lblDayTable1.Location = new System.Drawing.Point(80, 18);
            this.lblDayTable1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable1.Name = "lblDayTable1";
            this.lblDayTable1.Size = new System.Drawing.Size(85, 17);
            this.lblDayTable1.TabIndex = 4;
            this.lblDayTable1.Text = "Day Table 1";
            // 
            // gridTOUDay1
            // 
            this.gridTOUDay1.AllowUserToAddRows = false;
            this.gridTOUDay1.AllowUserToDeleteRows = false;
            this.gridTOUDay1.AllowUserToResizeColumns = false;
            this.gridTOUDay1.AllowUserToResizeRows = false;
            this.gridTOUDay1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay1.Location = new System.Drawing.Point(8, 38);
            this.gridTOUDay1.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay1.Name = "gridTOUDay1";
            this.gridTOUDay1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay1.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay1.TabIndex = 3;
            this.gridTOUDay1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay2
            // 
            this.gridTOUDay2.AllowUserToAddRows = false;
            this.gridTOUDay2.AllowUserToDeleteRows = false;
            this.gridTOUDay2.AllowUserToResizeColumns = false;
            this.gridTOUDay2.AllowUserToResizeRows = false;
            this.gridTOUDay2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay2.Location = new System.Drawing.Point(245, 38);
            this.gridTOUDay2.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay2.Name = "gridTOUDay2";
            this.gridTOUDay2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay2.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay2.TabIndex = 2;
            this.gridTOUDay2.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay2.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay3
            // 
            this.gridTOUDay3.AllowUserToAddRows = false;
            this.gridTOUDay3.AllowUserToDeleteRows = false;
            this.gridTOUDay3.AllowUserToResizeColumns = false;
            this.gridTOUDay3.AllowUserToResizeRows = false;
            this.gridTOUDay3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay3.Location = new System.Drawing.Point(483, 38);
            this.gridTOUDay3.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay3.Name = "gridTOUDay3";
            this.gridTOUDay3.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay3.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay3.TabIndex = 1;
            this.gridTOUDay3.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay3.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay3.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay4
            // 
            this.gridTOUDay4.AllowUserToAddRows = false;
            this.gridTOUDay4.AllowUserToDeleteRows = false;
            this.gridTOUDay4.AllowUserToResizeColumns = false;
            this.gridTOUDay4.AllowUserToResizeRows = false;
            this.gridTOUDay4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay4.Location = new System.Drawing.Point(8, 287);
            this.gridTOUDay4.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay4.Name = "gridTOUDay4";
            this.gridTOUDay4.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay4.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay4.TabIndex = 0;
            this.gridTOUDay4.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay4.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay4.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // tabPageSeason2
            // 
            this.tabPageSeason2.Controls.Add(this.groupBox26);
            this.tabPageSeason2.Location = new System.Drawing.Point(4, 25);
            this.tabPageSeason2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSeason2.Name = "tabPageSeason2";
            this.tabPageSeason2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSeason2.Size = new System.Drawing.Size(749, 516);
            this.tabPageSeason2.TabIndex = 1;
            this.tabPageSeason2.Text = "Season 2";
            this.tabPageSeason2.UseVisualStyleBackColor = true;
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.gridTOUDay12);
            this.groupBox26.Controls.Add(this.gridTOUDay11);
            this.groupBox26.Controls.Add(this.label51);
            this.groupBox26.Controls.Add(this.label52);
            this.groupBox26.Controls.Add(this.label53);
            this.groupBox26.Controls.Add(this.label54);
            this.groupBox26.Controls.Add(this.label55);
            this.groupBox26.Controls.Add(this.label56);
            this.groupBox26.Controls.Add(this.gridTOUDay7);
            this.groupBox26.Controls.Add(this.gridTOUDay8);
            this.groupBox26.Controls.Add(this.gridTOUDay9);
            this.groupBox26.Controls.Add(this.gridTOUDay10);
            this.groupBox26.ForeColor = System.Drawing.Color.Black;
            this.groupBox26.Location = new System.Drawing.Point(12, 6);
            this.groupBox26.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox26.Size = new System.Drawing.Size(727, 501);
            this.groupBox26.TabIndex = 5;
            this.groupBox26.TabStop = false;
            // 
            // gridTOUDay12
            // 
            this.gridTOUDay12.AllowUserToAddRows = false;
            this.gridTOUDay12.AllowUserToDeleteRows = false;
            this.gridTOUDay12.AllowUserToResizeColumns = false;
            this.gridTOUDay12.AllowUserToResizeRows = false;
            this.gridTOUDay12.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay12.Location = new System.Drawing.Point(483, 287);
            this.gridTOUDay12.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay12.Name = "gridTOUDay12";
            this.gridTOUDay12.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay12.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay12.TabIndex = 9;
            this.gridTOUDay12.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay12.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay12.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay11
            // 
            this.gridTOUDay11.AllowUserToAddRows = false;
            this.gridTOUDay11.AllowUserToDeleteRows = false;
            this.gridTOUDay11.AllowUserToResizeColumns = false;
            this.gridTOUDay11.AllowUserToResizeRows = false;
            this.gridTOUDay11.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay11.Location = new System.Drawing.Point(245, 287);
            this.gridTOUDay11.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay11.Name = "gridTOUDay11";
            this.gridTOUDay11.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay11.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay11.TabIndex = 8;
            this.gridTOUDay11.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay11.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay11.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(555, 267);
            this.label51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(85, 17);
            this.label51.TabIndex = 7;
            this.label51.Text = "Day Table 6";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(317, 267);
            this.label52.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(85, 17);
            this.label52.TabIndex = 7;
            this.label52.Text = "Day Table 5";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(80, 267);
            this.label53.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(85, 17);
            this.label53.TabIndex = 7;
            this.label53.Text = "Day Table 4";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(555, 18);
            this.label54.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(85, 17);
            this.label54.TabIndex = 6;
            this.label54.Text = "Day Table 3";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(317, 18);
            this.label55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(85, 17);
            this.label55.TabIndex = 5;
            this.label55.Text = "Day Table 2";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(80, 18);
            this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(85, 17);
            this.label56.TabIndex = 4;
            this.label56.Text = "Day Table 1";
            // 
            // gridTOUDay7
            // 
            this.gridTOUDay7.AllowUserToAddRows = false;
            this.gridTOUDay7.AllowUserToDeleteRows = false;
            this.gridTOUDay7.AllowUserToResizeColumns = false;
            this.gridTOUDay7.AllowUserToResizeRows = false;
            this.gridTOUDay7.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay7.Location = new System.Drawing.Point(8, 38);
            this.gridTOUDay7.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay7.Name = "gridTOUDay7";
            this.gridTOUDay7.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay7.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay7.TabIndex = 3;
            this.gridTOUDay7.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay7.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay8
            // 
            this.gridTOUDay8.AllowUserToAddRows = false;
            this.gridTOUDay8.AllowUserToDeleteRows = false;
            this.gridTOUDay8.AllowUserToResizeColumns = false;
            this.gridTOUDay8.AllowUserToResizeRows = false;
            this.gridTOUDay8.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay8.Location = new System.Drawing.Point(245, 38);
            this.gridTOUDay8.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay8.Name = "gridTOUDay8";
            this.gridTOUDay8.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay8.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay8.TabIndex = 2;
            this.gridTOUDay8.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay8.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay8.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay9
            // 
            this.gridTOUDay9.AllowUserToAddRows = false;
            this.gridTOUDay9.AllowUserToDeleteRows = false;
            this.gridTOUDay9.AllowUserToResizeColumns = false;
            this.gridTOUDay9.AllowUserToResizeRows = false;
            this.gridTOUDay9.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay9.Location = new System.Drawing.Point(483, 38);
            this.gridTOUDay9.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay9.Name = "gridTOUDay9";
            this.gridTOUDay9.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay9.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay9.TabIndex = 1;
            this.gridTOUDay9.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay9.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay9.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay10
            // 
            this.gridTOUDay10.AllowUserToAddRows = false;
            this.gridTOUDay10.AllowUserToDeleteRows = false;
            this.gridTOUDay10.AllowUserToResizeColumns = false;
            this.gridTOUDay10.AllowUserToResizeRows = false;
            this.gridTOUDay10.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay10.Location = new System.Drawing.Point(8, 287);
            this.gridTOUDay10.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay10.Name = "gridTOUDay10";
            this.gridTOUDay10.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay10.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay10.TabIndex = 0;
            this.gridTOUDay10.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay10.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay10.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // tabPageSeason3
            // 
            this.tabPageSeason3.Controls.Add(this.groupBox27);
            this.tabPageSeason3.Location = new System.Drawing.Point(4, 25);
            this.tabPageSeason3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSeason3.Name = "tabPageSeason3";
            this.tabPageSeason3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSeason3.Size = new System.Drawing.Size(749, 516);
            this.tabPageSeason3.TabIndex = 2;
            this.tabPageSeason3.Text = "Season 3";
            this.tabPageSeason3.UseVisualStyleBackColor = true;
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.gridTOUDay18);
            this.groupBox27.Controls.Add(this.gridTOUDay17);
            this.groupBox27.Controls.Add(this.label57);
            this.groupBox27.Controls.Add(this.label58);
            this.groupBox27.Controls.Add(this.label59);
            this.groupBox27.Controls.Add(this.label60);
            this.groupBox27.Controls.Add(this.label61);
            this.groupBox27.Controls.Add(this.label62);
            this.groupBox27.Controls.Add(this.gridTOUDay13);
            this.groupBox27.Controls.Add(this.gridTOUDay14);
            this.groupBox27.Controls.Add(this.gridTOUDay15);
            this.groupBox27.Controls.Add(this.gridTOUDay16);
            this.groupBox27.ForeColor = System.Drawing.Color.Black;
            this.groupBox27.Location = new System.Drawing.Point(12, 6);
            this.groupBox27.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox27.Size = new System.Drawing.Size(731, 501);
            this.groupBox27.TabIndex = 5;
            this.groupBox27.TabStop = false;
            // 
            // gridTOUDay18
            // 
            this.gridTOUDay18.AllowUserToAddRows = false;
            this.gridTOUDay18.AllowUserToDeleteRows = false;
            this.gridTOUDay18.AllowUserToResizeColumns = false;
            this.gridTOUDay18.AllowUserToResizeRows = false;
            this.gridTOUDay18.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay18.Location = new System.Drawing.Point(483, 287);
            this.gridTOUDay18.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay18.Name = "gridTOUDay18";
            this.gridTOUDay18.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay18.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay18.TabIndex = 9;
            this.gridTOUDay18.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay18.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay18.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay17
            // 
            this.gridTOUDay17.AllowUserToAddRows = false;
            this.gridTOUDay17.AllowUserToDeleteRows = false;
            this.gridTOUDay17.AllowUserToResizeColumns = false;
            this.gridTOUDay17.AllowUserToResizeRows = false;
            this.gridTOUDay17.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay17.Location = new System.Drawing.Point(245, 287);
            this.gridTOUDay17.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay17.Name = "gridTOUDay17";
            this.gridTOUDay17.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay17.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay17.TabIndex = 8;
            this.gridTOUDay17.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay17.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay17.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(555, 267);
            this.label57.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(85, 17);
            this.label57.TabIndex = 7;
            this.label57.Text = "Day Table 6";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(317, 267);
            this.label58.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(85, 17);
            this.label58.TabIndex = 7;
            this.label58.Text = "Day Table 5";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(80, 267);
            this.label59.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(85, 17);
            this.label59.TabIndex = 7;
            this.label59.Text = "Day Table 4";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(555, 18);
            this.label60.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(85, 17);
            this.label60.TabIndex = 6;
            this.label60.Text = "Day Table 3";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(317, 18);
            this.label61.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(85, 17);
            this.label61.TabIndex = 5;
            this.label61.Text = "Day Table 2";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(80, 18);
            this.label62.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(85, 17);
            this.label62.TabIndex = 4;
            this.label62.Text = "Day Table 1";
            // 
            // gridTOUDay13
            // 
            this.gridTOUDay13.AllowUserToAddRows = false;
            this.gridTOUDay13.AllowUserToDeleteRows = false;
            this.gridTOUDay13.AllowUserToResizeColumns = false;
            this.gridTOUDay13.AllowUserToResizeRows = false;
            this.gridTOUDay13.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay13.Location = new System.Drawing.Point(8, 38);
            this.gridTOUDay13.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay13.Name = "gridTOUDay13";
            this.gridTOUDay13.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay13.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay13.TabIndex = 3;
            this.gridTOUDay13.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay13.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay13.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay14
            // 
            this.gridTOUDay14.AllowUserToAddRows = false;
            this.gridTOUDay14.AllowUserToDeleteRows = false;
            this.gridTOUDay14.AllowUserToResizeColumns = false;
            this.gridTOUDay14.AllowUserToResizeRows = false;
            this.gridTOUDay14.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay14.Location = new System.Drawing.Point(245, 38);
            this.gridTOUDay14.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay14.Name = "gridTOUDay14";
            this.gridTOUDay14.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay14.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay14.TabIndex = 2;
            this.gridTOUDay14.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay14.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay14.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay15
            // 
            this.gridTOUDay15.AllowUserToAddRows = false;
            this.gridTOUDay15.AllowUserToDeleteRows = false;
            this.gridTOUDay15.AllowUserToResizeColumns = false;
            this.gridTOUDay15.AllowUserToResizeRows = false;
            this.gridTOUDay15.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay15.Location = new System.Drawing.Point(483, 38);
            this.gridTOUDay15.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay15.Name = "gridTOUDay15";
            this.gridTOUDay15.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay15.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay15.StandardTab = true;
            this.gridTOUDay15.TabIndex = 1;
            this.gridTOUDay15.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay15.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay15.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay16
            // 
            this.gridTOUDay16.AllowUserToAddRows = false;
            this.gridTOUDay16.AllowUserToDeleteRows = false;
            this.gridTOUDay16.AllowUserToResizeColumns = false;
            this.gridTOUDay16.AllowUserToResizeRows = false;
            this.gridTOUDay16.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay16.Location = new System.Drawing.Point(8, 287);
            this.gridTOUDay16.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay16.Name = "gridTOUDay16";
            this.gridTOUDay16.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay16.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay16.TabIndex = 0;
            this.gridTOUDay16.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay16.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay16.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // tabPageSeason4
            // 
            this.tabPageSeason4.Controls.Add(this.groupBox28);
            this.tabPageSeason4.Location = new System.Drawing.Point(4, 25);
            this.tabPageSeason4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageSeason4.Name = "tabPageSeason4";
            this.tabPageSeason4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageSeason4.Size = new System.Drawing.Size(749, 516);
            this.tabPageSeason4.TabIndex = 3;
            this.tabPageSeason4.Text = "Season 4";
            this.tabPageSeason4.UseVisualStyleBackColor = true;
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.gridTOUDay24);
            this.groupBox28.Controls.Add(this.gridTOUDay23);
            this.groupBox28.Controls.Add(this.label63);
            this.groupBox28.Controls.Add(this.label64);
            this.groupBox28.Controls.Add(this.label65);
            this.groupBox28.Controls.Add(this.label66);
            this.groupBox28.Controls.Add(this.label67);
            this.groupBox28.Controls.Add(this.label68);
            this.groupBox28.Controls.Add(this.gridTOUDay19);
            this.groupBox28.Controls.Add(this.gridTOUDay20);
            this.groupBox28.Controls.Add(this.gridTOUDay21);
            this.groupBox28.Controls.Add(this.gridTOUDay22);
            this.groupBox28.ForeColor = System.Drawing.Color.Black;
            this.groupBox28.Location = new System.Drawing.Point(12, 6);
            this.groupBox28.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox28.Size = new System.Drawing.Size(727, 501);
            this.groupBox28.TabIndex = 5;
            this.groupBox28.TabStop = false;
            // 
            // gridTOUDay24
            // 
            this.gridTOUDay24.AllowUserToAddRows = false;
            this.gridTOUDay24.AllowUserToDeleteRows = false;
            this.gridTOUDay24.AllowUserToResizeColumns = false;
            this.gridTOUDay24.AllowUserToResizeRows = false;
            this.gridTOUDay24.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay24.Location = new System.Drawing.Point(483, 287);
            this.gridTOUDay24.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay24.Name = "gridTOUDay24";
            this.gridTOUDay24.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay24.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay24.TabIndex = 9;
            this.gridTOUDay24.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay24.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay24.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay23
            // 
            this.gridTOUDay23.AllowUserToAddRows = false;
            this.gridTOUDay23.AllowUserToDeleteRows = false;
            this.gridTOUDay23.AllowUserToResizeColumns = false;
            this.gridTOUDay23.AllowUserToResizeRows = false;
            this.gridTOUDay23.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay23.Location = new System.Drawing.Point(245, 287);
            this.gridTOUDay23.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay23.Name = "gridTOUDay23";
            this.gridTOUDay23.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay23.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay23.TabIndex = 8;
            this.gridTOUDay23.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay23.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay23.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(555, 267);
            this.label63.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(85, 17);
            this.label63.TabIndex = 7;
            this.label63.Text = "Day Table 6";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(317, 267);
            this.label64.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(85, 17);
            this.label64.TabIndex = 7;
            this.label64.Text = "Day Table 5";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(80, 267);
            this.label65.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(85, 17);
            this.label65.TabIndex = 7;
            this.label65.Text = "Day Table 4";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(555, 18);
            this.label66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(85, 17);
            this.label66.TabIndex = 6;
            this.label66.Text = "Day Table 3";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(317, 18);
            this.label67.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(85, 17);
            this.label67.TabIndex = 5;
            this.label67.Text = "Day Table 2";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(80, 18);
            this.label68.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(85, 17);
            this.label68.TabIndex = 4;
            this.label68.Text = "Day Table 1";
            // 
            // gridTOUDay19
            // 
            this.gridTOUDay19.AllowUserToAddRows = false;
            this.gridTOUDay19.AllowUserToDeleteRows = false;
            this.gridTOUDay19.AllowUserToResizeColumns = false;
            this.gridTOUDay19.AllowUserToResizeRows = false;
            this.gridTOUDay19.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay19.Location = new System.Drawing.Point(8, 38);
            this.gridTOUDay19.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay19.Name = "gridTOUDay19";
            this.gridTOUDay19.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay19.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay19.TabIndex = 3;
            this.gridTOUDay19.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay19.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay19.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay20
            // 
            this.gridTOUDay20.AllowUserToAddRows = false;
            this.gridTOUDay20.AllowUserToDeleteRows = false;
            this.gridTOUDay20.AllowUserToResizeColumns = false;
            this.gridTOUDay20.AllowUserToResizeRows = false;
            this.gridTOUDay20.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay20.Location = new System.Drawing.Point(245, 38);
            this.gridTOUDay20.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay20.Name = "gridTOUDay20";
            this.gridTOUDay20.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay20.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay20.TabIndex = 2;
            this.gridTOUDay20.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay20.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay20.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay21
            // 
            this.gridTOUDay21.AllowUserToAddRows = false;
            this.gridTOUDay21.AllowUserToDeleteRows = false;
            this.gridTOUDay21.AllowUserToResizeColumns = false;
            this.gridTOUDay21.AllowUserToResizeRows = false;
            this.gridTOUDay21.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay21.Location = new System.Drawing.Point(483, 38);
            this.gridTOUDay21.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay21.Name = "gridTOUDay21";
            this.gridTOUDay21.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay21.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay21.TabIndex = 1;
            this.gridTOUDay21.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay21.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay21.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // gridTOUDay22
            // 
            this.gridTOUDay22.AllowUserToAddRows = false;
            this.gridTOUDay22.AllowUserToDeleteRows = false;
            this.gridTOUDay22.AllowUserToResizeColumns = false;
            this.gridTOUDay22.AllowUserToResizeRows = false;
            this.gridTOUDay22.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTOUDay22.Location = new System.Drawing.Point(8, 287);
            this.gridTOUDay22.Margin = new System.Windows.Forms.Padding(4);
            this.gridTOUDay22.Name = "gridTOUDay22";
            this.gridTOUDay22.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridTOUDay22.Size = new System.Drawing.Size(229, 207);
            this.gridTOUDay22.TabIndex = 0;
            this.gridTOUDay22.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridTOUDay_CellValidating);
            this.gridTOUDay22.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTOUDay_CellClick);
            this.gridTOUDay22.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTOUDay1_DataError);
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.btnResetAll);
            this.groupBox25.Controls.Add(this.btnFillTOUConfiguration);
            this.groupBox25.Controls.Add(this.dTPFutureActivationDate);
            this.groupBox25.Controls.Add(this.label191);
            this.groupBox25.Controls.Add(this.lblActivation);
            this.groupBox25.Controls.Add(this.lblDayTable);
            this.groupBox25.Controls.Add(this.gridActivationDate);
            this.groupBox25.Controls.Add(this.gridDayTables);
            this.groupBox25.Controls.Add(this.btnTOUWrite);
            this.groupBox25.Controls.Add(this.btnReadFutureTOU);
            this.groupBox25.Controls.Add(this.btnReadCurrentTOU);
            this.groupBox25.Location = new System.Drawing.Point(769, 32);
            this.groupBox25.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox25.Size = new System.Drawing.Size(419, 545);
            this.groupBox25.TabIndex = 1;
            this.groupBox25.TabStop = false;
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(229, 353);
            this.btnResetAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(153, 33);
            this.btnResetAll.TabIndex = 50;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // btnFillTOUConfiguration
            // 
            this.btnFillTOUConfiguration.Location = new System.Drawing.Point(229, 389);
            this.btnFillTOUConfiguration.Margin = new System.Windows.Forms.Padding(4);
            this.btnFillTOUConfiguration.Name = "btnFillTOUConfiguration";
            this.btnFillTOUConfiguration.Size = new System.Drawing.Size(153, 33);
            this.btnFillTOUConfiguration.TabIndex = 49;
            this.btnFillTOUConfiguration.Text = "Auto Fill";
            this.btnFillTOUConfiguration.UseVisualStyleBackColor = true;
            this.btnFillTOUConfiguration.Click += new System.EventHandler(this.btnFillTOUConfiguration_Click);
            // 
            // dTPFutureActivationDate
            // 
            this.dTPFutureActivationDate.CustomFormat = "dd/MM/yyyy";
            this.dTPFutureActivationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTPFutureActivationDate.Location = new System.Drawing.Point(229, 465);
            this.dTPFutureActivationDate.Margin = new System.Windows.Forms.Padding(4);
            this.dTPFutureActivationDate.Name = "dTPFutureActivationDate";
            this.dTPFutureActivationDate.Size = new System.Drawing.Size(145, 22);
            this.dTPFutureActivationDate.TabIndex = 43;
            // 
            // label191
            // 
            this.label191.Location = new System.Drawing.Point(13, 468);
            this.label191.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label191.Name = "label191";
            this.label191.Size = new System.Drawing.Size(189, 22);
            this.label191.TabIndex = 42;
            this.label191.Text = "Future TOU Activation Date";
            // 
            // lblActivation
            // 
            this.lblActivation.AutoSize = true;
            this.lblActivation.Location = new System.Drawing.Point(59, 241);
            this.lblActivation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivation.Name = "lblActivation";
            this.lblActivation.Size = new System.Drawing.Size(100, 17);
            this.lblActivation.TabIndex = 42;
            this.lblActivation.Text = "Season Profile";
            // 
            // lblDayTable
            // 
            this.lblDayTable.AutoSize = true;
            this.lblDayTable.Location = new System.Drawing.Point(181, 18);
            this.lblDayTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDayTable.Name = "lblDayTable";
            this.lblDayTable.Size = new System.Drawing.Size(88, 17);
            this.lblDayTable.TabIndex = 40;
            this.lblDayTable.Text = "Week Profile";
            // 
            // gridActivationDate
            // 
            this.gridActivationDate.AllowDrop = true;
            this.gridActivationDate.AllowUserToAddRows = false;
            this.gridActivationDate.AllowUserToDeleteRows = false;
            this.gridActivationDate.AllowUserToResizeColumns = false;
            this.gridActivationDate.AllowUserToResizeRows = false;
            this.gridActivationDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridActivationDate.Location = new System.Drawing.Point(17, 267);
            this.gridActivationDate.Margin = new System.Windows.Forms.Padding(4);
            this.gridActivationDate.Name = "gridActivationDate";
            this.gridActivationDate.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.gridActivationDate.Size = new System.Drawing.Size(180, 155);
            this.gridActivationDate.TabIndex = 41;
            this.gridActivationDate.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridActivationDate_CellValidating);
            this.gridActivationDate.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridActivationDate_CellClick);
            this.gridActivationDate.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridActivationDate_DataError);
            // 
            // gridDayTables
            // 
            this.gridDayTables.AllowUserToAddRows = false;
            this.gridDayTables.AllowUserToDeleteRows = false;
            this.gridDayTables.AllowUserToResizeColumns = false;
            this.gridDayTables.AllowUserToResizeRows = false;
            this.gridDayTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDayTables.Location = new System.Drawing.Point(0, 38);
            this.gridDayTables.Margin = new System.Windows.Forms.Padding(4);
            this.gridDayTables.Name = "gridDayTables";
            this.gridDayTables.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridDayTables.Size = new System.Drawing.Size(417, 140);
            this.gridDayTables.TabIndex = 39;
            this.gridDayTables.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gridDayTables_CellValidating);
            this.gridDayTables.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDayTables_CellClick);
            this.gridDayTables.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridDayTables_DataError);
            // 
            // btnTOUWrite
            // 
            this.btnTOUWrite.Location = new System.Drawing.Point(229, 313);
            this.btnTOUWrite.Margin = new System.Windows.Forms.Padding(4);
            this.btnTOUWrite.Name = "btnTOUWrite";
            this.btnTOUWrite.Size = new System.Drawing.Size(153, 33);
            this.btnTOUWrite.TabIndex = 37;
            this.btnTOUWrite.Text = "Write TOU";
            this.btnTOUWrite.UseVisualStyleBackColor = true;
            this.btnTOUWrite.Click += new System.EventHandler(this.btnTOUWrite_Click);
            // 
            // btnReadFutureTOU
            // 
            this.btnReadFutureTOU.AccessibleName = "a";
            this.btnReadFutureTOU.Location = new System.Drawing.Point(229, 224);
            this.btnReadFutureTOU.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadFutureTOU.Name = "btnReadFutureTOU";
            this.btnReadFutureTOU.Size = new System.Drawing.Size(153, 33);
            this.btnReadFutureTOU.TabIndex = 36;
            this.btnReadFutureTOU.Text = "Read Future TOU";
            this.btnReadFutureTOU.UseVisualStyleBackColor = true;
            this.btnReadFutureTOU.Click += new System.EventHandler(this.btnReadFutureTOU_Click);
            // 
            // btnReadCurrentTOU
            // 
            this.btnReadCurrentTOU.Location = new System.Drawing.Point(229, 268);
            this.btnReadCurrentTOU.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadCurrentTOU.Name = "btnReadCurrentTOU";
            this.btnReadCurrentTOU.Size = new System.Drawing.Size(153, 33);
            this.btnReadCurrentTOU.TabIndex = 36;
            this.btnReadCurrentTOU.Text = "Read Current TOU";
            this.btnReadCurrentTOU.UseVisualStyleBackColor = true;
            this.btnReadCurrentTOU.Click += new System.EventHandler(this.btnReadCurrentTOU_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.AutoScroll = true;
            this.tabPage7.Controls.Add(this.tableLayoutPanel12);
            this.tabPage7.Location = new System.Drawing.Point(4, 25);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(1212, 799);
            this.tabPage7.TabIndex = 11;
            this.tabPage7.Text = "LS Capture Period";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.AutoScroll = true;
            this.tableLayoutPanel12.AutoSize = true;
            this.tableLayoutPanel12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.583333F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.41666F));
            this.tableLayoutPanel12.Controls.Add(this.groupBox18, 1, 1);
            this.tableLayoutPanel12.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 3;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.16854F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.83146F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(838, 415);
            this.tableLayoutPanel12.TabIndex = 3;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.lblSeconds);
            this.groupBox18.Controls.Add(this.lblLoadSurveyCapturePeriod);
            this.groupBox18.Controls.Add(this.cmbBoxLSCapturePeriod);
            this.groupBox18.Controls.Add(this.btnReadLSCapturePeriod);
            this.groupBox18.Controls.Add(this.btnWriteLSCapturePeriod);
            this.groupBox18.Location = new System.Drawing.Point(42, 24);
            this.groupBox18.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox18.Size = new System.Drawing.Size(792, 363);
            this.groupBox18.TabIndex = 1;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Load Survey Capture Period";
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(332, 60);
            this.lblSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(73, 17);
            this.lblSeconds.TabIndex = 4;
            this.lblSeconds.Text = "Second(s)";
            // 
            // lblLoadSurveyCapturePeriod
            // 
            this.lblLoadSurveyCapturePeriod.AutoSize = true;
            this.lblLoadSurveyCapturePeriod.Location = new System.Drawing.Point(27, 60);
            this.lblLoadSurveyCapturePeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoadSurveyCapturePeriod.Name = "lblLoadSurveyCapturePeriod";
            this.lblLoadSurveyCapturePeriod.Size = new System.Drawing.Size(191, 17);
            this.lblLoadSurveyCapturePeriod.TabIndex = 3;
            this.lblLoadSurveyCapturePeriod.Text = "Load Survey Capture Period ";
            // 
            // cmbBoxLSCapturePeriod
            // 
            this.cmbBoxLSCapturePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLSCapturePeriod.FormattingEnabled = true;
            this.cmbBoxLSCapturePeriod.Items.AddRange(new object[] {
            "900",
            "1800",
            "3600"});
            this.cmbBoxLSCapturePeriod.Location = new System.Drawing.Point(224, 57);
            this.cmbBoxLSCapturePeriod.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxLSCapturePeriod.Name = "cmbBoxLSCapturePeriod";
            this.cmbBoxLSCapturePeriod.Size = new System.Drawing.Size(99, 24);
            this.cmbBoxLSCapturePeriod.TabIndex = 0;
            // 
            // btnReadLSCapturePeriod
            // 
            this.btnReadLSCapturePeriod.Location = new System.Drawing.Point(289, 130);
            this.btnReadLSCapturePeriod.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadLSCapturePeriod.Name = "btnReadLSCapturePeriod";
            this.btnReadLSCapturePeriod.Size = new System.Drawing.Size(100, 32);
            this.btnReadLSCapturePeriod.TabIndex = 2;
            this.btnReadLSCapturePeriod.Text = "Read";
            this.btnReadLSCapturePeriod.UseVisualStyleBackColor = true;
            this.btnReadLSCapturePeriod.Click += new System.EventHandler(this.btnReadLSCapturePeriod_Click);
            // 
            // btnWriteLSCapturePeriod
            // 
            this.btnWriteLSCapturePeriod.AutoSize = true;
            this.btnWriteLSCapturePeriod.Location = new System.Drawing.Point(181, 130);
            this.btnWriteLSCapturePeriod.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteLSCapturePeriod.Name = "btnWriteLSCapturePeriod";
            this.btnWriteLSCapturePeriod.Size = new System.Drawing.Size(100, 33);
            this.btnWriteLSCapturePeriod.TabIndex = 1;
            this.btnWriteLSCapturePeriod.Text = "Write";
            this.btnWriteLSCapturePeriod.UseVisualStyleBackColor = true;
            this.btnWriteLSCapturePeriod.Click += new System.EventHandler(this.btnWriteLSCapturePeriod_Click);
            // 
            // tabPageIntegrationPeriod
            // 
            this.tabPageIntegrationPeriod.AutoScroll = true;
            this.tabPageIntegrationPeriod.Controls.Add(this.tableLayoutPanel6);
            this.tabPageIntegrationPeriod.Location = new System.Drawing.Point(4, 25);
            this.tabPageIntegrationPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageIntegrationPeriod.Name = "tabPageIntegrationPeriod";
            this.tabPageIntegrationPeriod.Size = new System.Drawing.Size(1212, 799);
            this.tabPageIntegrationPeriod.TabIndex = 4;
            this.tabPageIntegrationPeriod.Text = "Integration Period";
            this.tabPageIntegrationPeriod.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoScroll = true;
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.583333F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.41666F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 488F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox24, 1, 1);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.16854F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.83146F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(930, 237);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // groupBox24
            // 
            this.groupBox24.AutoSize = true;
            this.groupBox24.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox24.Controls.Add(this.lblDemandIPSeconds);
            this.groupBox24.Controls.Add(this.lblDemandIntegrationPeriod);
            this.groupBox24.Controls.Add(this.cmbBoxIntegrationPeriod);
            this.groupBox24.Controls.Add(this.btnReadIntegrationPeriod);
            this.groupBox24.Controls.Add(this.btnWriteIntegrationPeriod);
            this.groupBox24.Location = new System.Drawing.Point(24, 15);
            this.groupBox24.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox24.Size = new System.Drawing.Size(413, 194);
            this.groupBox24.TabIndex = 1;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Demand Integration Period";
            // 
            // lblDemandIPSeconds
            // 
            this.lblDemandIPSeconds.AutoSize = true;
            this.lblDemandIPSeconds.Location = new System.Drawing.Point(333, 73);
            this.lblDemandIPSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDemandIPSeconds.Name = "lblDemandIPSeconds";
            this.lblDemandIPSeconds.Size = new System.Drawing.Size(73, 17);
            this.lblDemandIPSeconds.TabIndex = 6;
            this.lblDemandIPSeconds.Text = "Second(s)";
            // 
            // lblDemandIntegrationPeriod
            // 
            this.lblDemandIntegrationPeriod.AutoSize = true;
            this.lblDemandIntegrationPeriod.Location = new System.Drawing.Point(27, 73);
            this.lblDemandIntegrationPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDemandIntegrationPeriod.Name = "lblDemandIntegrationPeriod";
            this.lblDemandIntegrationPeriod.Size = new System.Drawing.Size(177, 17);
            this.lblDemandIntegrationPeriod.TabIndex = 5;
            this.lblDemandIntegrationPeriod.Text = "Demand Integration Period";
            // 
            // cmbBoxIntegrationPeriod
            // 
            this.cmbBoxIntegrationPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxIntegrationPeriod.FormattingEnabled = true;
            this.cmbBoxIntegrationPeriod.Items.AddRange(new object[] {
            "900",
            "1800"});
            this.cmbBoxIntegrationPeriod.Location = new System.Drawing.Point(225, 69);
            this.cmbBoxIntegrationPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxIntegrationPeriod.Name = "cmbBoxIntegrationPeriod";
            this.cmbBoxIntegrationPeriod.Size = new System.Drawing.Size(99, 24);
            this.cmbBoxIntegrationPeriod.TabIndex = 0;
            // 
            // btnReadIntegrationPeriod
            // 
            this.btnReadIntegrationPeriod.Location = new System.Drawing.Point(304, 140);
            this.btnReadIntegrationPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadIntegrationPeriod.Name = "btnReadIntegrationPeriod";
            this.btnReadIntegrationPeriod.Size = new System.Drawing.Size(100, 32);
            this.btnReadIntegrationPeriod.TabIndex = 2;
            this.btnReadIntegrationPeriod.Text = "Read";
            this.btnReadIntegrationPeriod.UseVisualStyleBackColor = true;
            this.btnReadIntegrationPeriod.Click += new System.EventHandler(this.btnReadIntegrationPeriod_Click);
            // 
            // btnWriteIntegrationPeriod
            // 
            this.btnWriteIntegrationPeriod.Location = new System.Drawing.Point(196, 140);
            this.btnWriteIntegrationPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteIntegrationPeriod.Name = "btnWriteIntegrationPeriod";
            this.btnWriteIntegrationPeriod.Size = new System.Drawing.Size(100, 32);
            this.btnWriteIntegrationPeriod.TabIndex = 1;
            this.btnWriteIntegrationPeriod.Text = "Write";
            this.btnWriteIntegrationPeriod.UseVisualStyleBackColor = true;
            this.btnWriteIntegrationPeriod.Click += new System.EventHandler(this.btnWriteIntegrationPeriod_Click);
            // 
            // tbCTPTRatio
            // 
            this.tbCTPTRatio.Controls.Add(this.groupBox6);
            this.tbCTPTRatio.Controls.Add(this.gbCTPTRatio);
            this.tbCTPTRatio.Location = new System.Drawing.Point(4, 25);
            this.tbCTPTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.tbCTPTRatio.Name = "tbCTPTRatio";
            this.tbCTPTRatio.Padding = new System.Windows.Forms.Padding(4);
            this.tbCTPTRatio.Size = new System.Drawing.Size(1212, 799);
            this.tbCTPTRatio.TabIndex = 17;
            this.tbCTPTRatio.Text = "CT/PT Ratio";
            this.tbCTPTRatio.UseVisualStyleBackColor = true;
            this.tbCTPTRatio.Click += new System.EventHandler(this.tbCTPTRatio_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lblPTProgramNotSupported);
            this.groupBox6.Controls.Add(this.btnReadPTRatio);
            this.groupBox6.Controls.Add(this.lblPTRatio);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Controls.Add(this.button8);
            this.groupBox6.Controls.Add(this.nudPTRatio);
            this.groupBox6.Location = new System.Drawing.Point(9, 231);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(977, 305);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Configure PT Ratio";
            // 
            // lblPTProgramNotSupported
            // 
            this.lblPTProgramNotSupported.AutoSize = true;
            this.lblPTProgramNotSupported.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPTProgramNotSupported.ForeColor = System.Drawing.Color.Black;
            this.lblPTProgramNotSupported.Location = new System.Drawing.Point(27, 212);
            this.lblPTProgramNotSupported.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPTProgramNotSupported.Name = "lblPTProgramNotSupported";
            this.lblPTProgramNotSupported.Size = new System.Drawing.Size(61, 17);
            this.lblPTProgramNotSupported.TabIndex = 11;
            this.lblPTProgramNotSupported.Text = "PTRatio";
            this.lblPTProgramNotSupported.Visible = false;
            // 
            // btnReadPTRatio
            // 
            this.btnReadPTRatio.Location = new System.Drawing.Point(28, 149);
            this.btnReadPTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadPTRatio.Name = "btnReadPTRatio";
            this.btnReadPTRatio.Size = new System.Drawing.Size(100, 28);
            this.btnReadPTRatio.TabIndex = 7;
            this.btnReadPTRatio.Text = "Read";
            this.btnReadPTRatio.UseVisualStyleBackColor = true;
            this.btnReadPTRatio.Click += new System.EventHandler(this.btnReadPTRatio_Click_1);
            // 
            // lblPTRatio
            // 
            this.lblPTRatio.AutoSize = true;
            this.lblPTRatio.Location = new System.Drawing.Point(188, 92);
            this.lblPTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPTRatio.Name = "lblPTRatio";
            this.lblPTRatio.Size = new System.Drawing.Size(55, 17);
            this.lblPTRatio.TabIndex = 6;
            this.lblPTRatio.Text = "(1-100)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(27, 89);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 17);
            this.label17.TabIndex = 5;
            this.label17.Text = "PT Ratio : ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(244, 149);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(136, 149);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(100, 28);
            this.button8.TabIndex = 2;
            this.button8.Text = "Write";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click_2);
            // 
            // nudPTRatio
            // 
            this.nudPTRatio.Location = new System.Drawing.Point(107, 86);
            this.nudPTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.nudPTRatio.Name = "nudPTRatio";
            this.nudPTRatio.Size = new System.Drawing.Size(71, 22);
            this.nudPTRatio.TabIndex = 1;
            this.nudPTRatio.ValueChanged += new System.EventHandler(this.nudPTRatio_ValueChanged);
            this.nudPTRatio.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nudPTRatio_KeyUp);
            this.nudPTRatio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudPTRatio_KeyDown);
            // 
            // gbCTPTRatio
            // 
            this.gbCTPTRatio.Controls.Add(this.lblCTRatioMessage);
            this.gbCTPTRatio.Controls.Add(this.btnCTRatio);
            this.gbCTPTRatio.Controls.Add(this.label14);
            this.gbCTPTRatio.Controls.Add(this.lblCTPTRatio);
            this.gbCTPTRatio.Controls.Add(this.btnReset);
            this.gbCTPTRatio.Controls.Add(this.btnCTPTWrite);
            this.gbCTPTRatio.Controls.Add(this.nudCTRatio);
            this.gbCTPTRatio.Location = new System.Drawing.Point(9, 6);
            this.gbCTPTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.gbCTPTRatio.Name = "gbCTPTRatio";
            this.gbCTPTRatio.Padding = new System.Windows.Forms.Padding(4);
            this.gbCTPTRatio.Size = new System.Drawing.Size(979, 208);
            this.gbCTPTRatio.TabIndex = 4;
            this.gbCTPTRatio.TabStop = false;
            this.gbCTPTRatio.Text = "Configure CT Ratio";
            // 
            // lblCTRatioMessage
            // 
            this.lblCTRatioMessage.AutoSize = true;
            this.lblCTRatioMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCTRatioMessage.ForeColor = System.Drawing.Color.Black;
            this.lblCTRatioMessage.Location = new System.Drawing.Point(23, 177);
            this.lblCTRatioMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTRatioMessage.Name = "lblCTRatioMessage";
            this.lblCTRatioMessage.Size = new System.Drawing.Size(63, 17);
            this.lblCTRatioMessage.TabIndex = 10;
            this.lblCTRatioMessage.Text = "CTRatio";
            this.lblCTRatioMessage.Visible = false;
            // 
            // btnCTRatio
            // 
            this.btnCTRatio.Location = new System.Drawing.Point(25, 134);
            this.btnCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.btnCTRatio.Name = "btnCTRatio";
            this.btnCTRatio.Size = new System.Drawing.Size(100, 28);
            this.btnCTRatio.TabIndex = 8;
            this.btnCTRatio.Text = "Read";
            this.btnCTRatio.UseVisualStyleBackColor = true;
            this.btnCTRatio.Click += new System.EventHandler(this.btnCTRatio_Click_1);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(189, 57);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 17);
            this.label14.TabIndex = 7;
            this.label14.Text = "(1-320)";
            // 
            // lblCTPTRatio
            // 
            this.lblCTPTRatio.AutoSize = true;
            this.lblCTPTRatio.Location = new System.Drawing.Point(27, 57);
            this.lblCTPTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTPTRatio.Name = "lblCTPTRatio";
            this.lblCTPTRatio.Size = new System.Drawing.Size(75, 17);
            this.lblCTPTRatio.TabIndex = 4;
            this.lblCTPTRatio.Text = "CT Ratio : ";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(241, 134);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 28);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click_2);
            // 
            // btnCTPTWrite
            // 
            this.btnCTPTWrite.Location = new System.Drawing.Point(133, 134);
            this.btnCTPTWrite.Margin = new System.Windows.Forms.Padding(4);
            this.btnCTPTWrite.Name = "btnCTPTWrite";
            this.btnCTPTWrite.Size = new System.Drawing.Size(100, 28);
            this.btnCTPTWrite.TabIndex = 2;
            this.btnCTPTWrite.Text = "Write";
            this.btnCTPTWrite.UseVisualStyleBackColor = true;
            this.btnCTPTWrite.Click += new System.EventHandler(this.btnCTPTWrite_Click_2);
            // 
            // nudCTRatio
            // 
            this.nudCTRatio.Location = new System.Drawing.Point(108, 54);
            this.nudCTRatio.Margin = new System.Windows.Forms.Padding(4);
            this.nudCTRatio.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.nudCTRatio.Name = "nudCTRatio";
            this.nudCTRatio.Size = new System.Drawing.Size(71, 22);
            this.nudCTRatio.TabIndex = 0;
            this.nudCTRatio.ValueChanged += new System.EventHandler(this.nudCTRatio_ValueChanged);
            this.nudCTRatio.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nudCTRatio_KeyUp);
            // 
            // tabMDReset
            // 
            this.tabMDReset.Controls.Add(this.groupBox13);
            this.tabMDReset.Location = new System.Drawing.Point(4, 25);
            this.tabMDReset.Margin = new System.Windows.Forms.Padding(4);
            this.tabMDReset.Name = "tabMDReset";
            this.tabMDReset.Size = new System.Drawing.Size(1212, 799);
            this.tabMDReset.TabIndex = 18;
            this.tabMDReset.Text = "Billing Reset";
            this.tabMDReset.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.btnMDReset);
            this.groupBox13.Controls.Add(this.chkMDReset);
            this.groupBox13.Location = new System.Drawing.Point(12, 10);
            this.groupBox13.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox13.Size = new System.Drawing.Size(459, 194);
            this.groupBox13.TabIndex = 0;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Billing Reset";
            // 
            // btnMDReset
            // 
            this.btnMDReset.Location = new System.Drawing.Point(321, 146);
            this.btnMDReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnMDReset.Name = "btnMDReset";
            this.btnMDReset.Size = new System.Drawing.Size(96, 28);
            this.btnMDReset.TabIndex = 1;
            this.btnMDReset.Text = "Write";
            this.btnMDReset.UseVisualStyleBackColor = true;
            this.btnMDReset.Click += new System.EventHandler(this.btnMDReset_Click);
            // 
            // chkMDReset
            // 
            this.chkMDReset.AutoSize = true;
            this.chkMDReset.Location = new System.Drawing.Point(8, 43);
            this.chkMDReset.Margin = new System.Windows.Forms.Padding(4);
            this.chkMDReset.Name = "chkMDReset";
            this.chkMDReset.Size = new System.Drawing.Size(108, 21);
            this.chkMDReset.TabIndex = 0;
            this.chkMDReset.Text = "Billing Reset";
            this.chkMDReset.UseVisualStyleBackColor = true;
            this.chkMDReset.CheckedChanged += new System.EventHandler(this.chkMDReset_CheckedChanged);
            // 
            // tbPDisplayParameters
            // 
            this.tbPDisplayParameters.Controls.Add(this.tableLayoutPanel13);
            this.tbPDisplayParameters.Location = new System.Drawing.Point(4, 25);
            this.tbPDisplayParameters.Margin = new System.Windows.Forms.Padding(4);
            this.tbPDisplayParameters.Name = "tbPDisplayParameters";
            this.tbPDisplayParameters.Size = new System.Drawing.Size(1212, 799);
            this.tbPDisplayParameters.TabIndex = 19;
            this.tbPDisplayParameters.Text = "Display Parameters";
            this.tbPDisplayParameters.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 4;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 887F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 299F));
            this.tableLayoutPanel13.Controls.Add(this.tabControlDisplayParams, 1, 1);
            this.tableLayoutPanel13.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 4;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 486F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(1107, 796);
            this.tableLayoutPanel13.TabIndex = 1;
            this.tableLayoutPanel13.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel13_Paint);
            // 
            // tabControlDisplayParams
            // 
            this.tabControlDisplayParams.Controls.Add(this.tabPagePushButton);
            this.tabControlDisplayParams.Controls.Add(this.tabPageScrollButton);
            this.tabControlDisplayParams.Controls.Add(this.tabPageHighResolution);
            this.tabControlDisplayParams.Controls.Add(this.tabPageDisplayTimeOut);
            this.tabControlDisplayParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDisplayParams.Location = new System.Drawing.Point(27, 14);
            this.tabControlDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlDisplayParams.Name = "tabControlDisplayParams";
            this.tableLayoutPanel13.SetRowSpan(this.tabControlDisplayParams, 3);
            this.tabControlDisplayParams.SelectedIndex = 0;
            this.tabControlDisplayParams.Size = new System.Drawing.Size(887, 782);
            this.tabControlDisplayParams.TabIndex = 2;
            this.tabControlDisplayParams.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlDisplayParams_Selected);
            this.tabControlDisplayParams.SelectedIndexChanged += new System.EventHandler(this.tabControlDisplayParams_SelectedIndexChanged);
            // 
            // tabPagePushButton
            // 
            this.tabPagePushButton.Controls.Add(this.dGVPushDisplayParams);
            this.tabPagePushButton.Location = new System.Drawing.Point(4, 25);
            this.tabPagePushButton.Margin = new System.Windows.Forms.Padding(4);
            this.tabPagePushButton.Name = "tabPagePushButton";
            this.tabPagePushButton.Size = new System.Drawing.Size(879, 753);
            this.tabPagePushButton.TabIndex = 0;
            this.tabPagePushButton.Text = "Push Button";
            this.tabPagePushButton.UseVisualStyleBackColor = true;
            this.tabPagePushButton.Enter += new System.EventHandler(this.tabPagePushButton_Enter);
            // 
            // dGVPushDisplayParams
            // 
            this.dGVPushDisplayParams.AllowUserToAddRows = false;
            this.dGVPushDisplayParams.AllowUserToDeleteRows = false;
            this.dGVPushDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVPushDisplayParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVPushDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dGVPushDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dGVPushDisplayParams.MultiSelect = false;
            this.dGVPushDisplayParams.Name = "dGVPushDisplayParams";
            this.dGVPushDisplayParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dGVPushDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVPushDisplayParams.Size = new System.Drawing.Size(879, 753);
            this.dGVPushDisplayParams.TabIndex = 0;
            this.dGVPushDisplayParams.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVPushDisplayParams_CellValueChanged);
            this.dGVPushDisplayParams.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dGVPushDisplayParams_SortCompare);
            this.dGVPushDisplayParams.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dGVPushDisplayParams_CellFormatting);
            this.dGVPushDisplayParams.CurrentCellDirtyStateChanged += new System.EventHandler(this.dGVPushDisplayParams_CurrentCellDirtyStateChanged);
            this.dGVPushDisplayParams.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dGVPushDisplayParams_DataBindingComplete);
            // 
            // tabPageScrollButton
            // 
            this.tabPageScrollButton.Controls.Add(this.dGVScrollDisplayParams);
            this.tabPageScrollButton.Location = new System.Drawing.Point(4, 25);
            this.tabPageScrollButton.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageScrollButton.Name = "tabPageScrollButton";
            this.tabPageScrollButton.Size = new System.Drawing.Size(879, 753);
            this.tabPageScrollButton.TabIndex = 1;
            this.tabPageScrollButton.Text = "Scroll Button";
            this.tabPageScrollButton.UseVisualStyleBackColor = true;
            this.tabPageScrollButton.Enter += new System.EventHandler(this.tabPageScrollButton_Enter);
            // 
            // dGVScrollDisplayParams
            // 
            this.dGVScrollDisplayParams.AllowUserToAddRows = false;
            this.dGVScrollDisplayParams.AllowUserToDeleteRows = false;
            this.dGVScrollDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVScrollDisplayParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVScrollDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dGVScrollDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dGVScrollDisplayParams.MultiSelect = false;
            this.dGVScrollDisplayParams.Name = "dGVScrollDisplayParams";
            this.dGVScrollDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVScrollDisplayParams.Size = new System.Drawing.Size(879, 753);
            this.dGVScrollDisplayParams.TabIndex = 1;
            this.dGVScrollDisplayParams.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVScrollDisplayParams_CellValueChanged);
            this.dGVScrollDisplayParams.CurrentCellDirtyStateChanged += new System.EventHandler(this.dGVScrollDisplayParams_CurrentCellDirtyStateChanged);
            // 
            // tabPageHighResolution
            // 
            this.tabPageHighResolution.Controls.Add(this.dGVHighResolution);
            this.tabPageHighResolution.Location = new System.Drawing.Point(4, 25);
            this.tabPageHighResolution.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageHighResolution.Name = "tabPageHighResolution";
            this.tabPageHighResolution.Size = new System.Drawing.Size(879, 753);
            this.tabPageHighResolution.TabIndex = 2;
            this.tabPageHighResolution.Text = "High Resolution";
            this.tabPageHighResolution.UseVisualStyleBackColor = true;
            this.tabPageHighResolution.Enter += new System.EventHandler(this.tabPageHighResolution_Enter);
            // 
            // dGVHighResolution
            // 
            this.dGVHighResolution.AllowUserToAddRows = false;
            this.dGVHighResolution.AllowUserToDeleteRows = false;
            this.dGVHighResolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVHighResolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVHighResolution.Location = new System.Drawing.Point(0, 0);
            this.dGVHighResolution.Margin = new System.Windows.Forms.Padding(0);
            this.dGVHighResolution.MultiSelect = false;
            this.dGVHighResolution.Name = "dGVHighResolution";
            this.dGVHighResolution.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVHighResolution.Size = new System.Drawing.Size(879, 753);
            this.dGVHighResolution.TabIndex = 1;
            this.dGVHighResolution.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVHighResolution_CellValueChanged);
            this.dGVHighResolution.Enter += new System.EventHandler(this.tabPageHighResolution_Enter);
            this.dGVHighResolution.CurrentCellDirtyStateChanged += new System.EventHandler(this.dGVHighResolution_CurrentCellDirtyStateChanged);
            // 
            // tabPageDisplayTimeOut
            // 
            this.tabPageDisplayTimeOut.Controls.Add(this.groupBox15);
            this.tabPageDisplayTimeOut.Location = new System.Drawing.Point(4, 25);
            this.tabPageDisplayTimeOut.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageDisplayTimeOut.Name = "tabPageDisplayTimeOut";
            this.tabPageDisplayTimeOut.Size = new System.Drawing.Size(879, 753);
            this.tabPageDisplayTimeOut.TabIndex = 3;
            this.tabPageDisplayTimeOut.Text = "Display Timeout";
            this.tabPageDisplayTimeOut.UseVisualStyleBackColor = true;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.chkAutoScrollTime);
            this.groupBox15.Controls.Add(this.label13);
            this.groupBox15.Controls.Add(this.txtScrollTime);
            this.groupBox15.Controls.Add(this.label15);
            this.groupBox15.Controls.Add(this.txtPushButtonTimeout);
            this.groupBox15.Controls.Add(this.label18);
            this.groupBox15.Controls.Add(this.txtScrollResumeTime);
            this.groupBox15.Controls.Add(this.label20);
            this.groupBox15.Controls.Add(this.label19);
            this.groupBox15.Location = new System.Drawing.Point(4, 20);
            this.groupBox15.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox15.Size = new System.Drawing.Size(799, 346);
            this.groupBox15.TabIndex = 21;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Display Timeouts";
            // 
            // chkAutoScrollTime
            // 
            this.chkAutoScrollTime.AutoSize = true;
            this.chkAutoScrollTime.Location = new System.Drawing.Point(29, 128);
            this.chkAutoScrollTime.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoScrollTime.Name = "chkAutoScrollTime";
            this.chkAutoScrollTime.Size = new System.Drawing.Size(189, 21);
            this.chkAutoScrollTime.TabIndex = 22;
            this.chkAutoScrollTime.Text = "Auto Scroll Resume Time";
            this.chkAutoScrollTime.UseVisualStyleBackColor = true;
            this.chkAutoScrollTime.Click += new System.EventHandler(this.chkAutoScrollTime_Click);
            this.chkAutoScrollTime.CheckedChanged += new System.EventHandler(this.chkAutoScrollTime_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(385, 129);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 17);
            this.label13.TabIndex = 18;
            this.label13.Text = "* Valid Range (3-300)";
            // 
            // txtScrollTime
            // 
            this.txtScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtScrollTime.Location = new System.Drawing.Point(260, 33);
            this.txtScrollTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtScrollTime.MaxLength = 3;
            this.txtScrollTime.Name = "txtScrollTime";
            this.txtScrollTime.Size = new System.Drawing.Size(99, 22);
            this.txtScrollTime.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(385, 85);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(145, 17);
            this.label15.TabIndex = 19;
            this.label15.Text = "* Valid Range (1-600)";
            // 
            // txtPushButtonTimeout
            // 
            this.txtPushButtonTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPushButtonTimeout.Location = new System.Drawing.Point(260, 81);
            this.txtPushButtonTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.txtPushButtonTimeout.MaxLength = 3;
            this.txtPushButtonTimeout.Name = "txtPushButtonTimeout";
            this.txtPushButtonTimeout.Size = new System.Drawing.Size(99, 22);
            this.txtPushButtonTimeout.TabIndex = 14;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(385, 42);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(145, 17);
            this.label18.TabIndex = 20;
            this.label18.Text = "* Valid Range (1-300)";
            // 
            // txtScrollResumeTime
            // 
            this.txtScrollResumeTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtScrollResumeTime.Enabled = false;
            this.txtScrollResumeTime.Location = new System.Drawing.Point(260, 128);
            this.txtScrollResumeTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtScrollResumeTime.MaxLength = 3;
            this.txtScrollResumeTime.Name = "txtScrollResumeTime";
            this.txtScrollResumeTime.Size = new System.Drawing.Size(99, 22);
            this.txtScrollResumeTime.TabIndex = 12;
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(25, 46);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(134, 17);
            this.label20.TabIndex = 11;
            this.label20.Text = "Scroll Time Per Item";
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(25, 85);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(140, 17);
            this.label19.TabIndex = 10;
            this.label19.Text = "Push Button Timeout";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpScroll);
            this.panel2.Controls.Add(this.btnWriteDisplayParams);
            this.panel2.Controls.Add(this.btnReadDisplayParams);
            this.panel2.Controls.Add(this.chkBoxSelectAll);
            this.panel2.Controls.Add(this.btnDownScroll);
            this.panel2.Location = new System.Drawing.Point(918, 18);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(111, 354);
            this.panel2.TabIndex = 17;
            // 
            // btnUpScroll
            // 
            this.btnUpScroll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpScroll.Location = new System.Drawing.Point(17, 128);
            this.btnUpScroll.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpScroll.Name = "btnUpScroll";
            this.btnUpScroll.Size = new System.Drawing.Size(57, 50);
            this.btnUpScroll.TabIndex = 1;
            this.btnUpScroll.Text = "^";
            this.btnUpScroll.UseVisualStyleBackColor = true;
            this.btnUpScroll.Click += new System.EventHandler(this.btnUpScroll_Click);
            // 
            // btnWriteDisplayParams
            // 
            this.btnWriteDisplayParams.Location = new System.Drawing.Point(4, 322);
            this.btnWriteDisplayParams.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteDisplayParams.Name = "btnWriteDisplayParams";
            this.btnWriteDisplayParams.Size = new System.Drawing.Size(100, 28);
            this.btnWriteDisplayParams.TabIndex = 1;
            this.btnWriteDisplayParams.Text = "Write";
            this.btnWriteDisplayParams.UseVisualStyleBackColor = true;
            this.btnWriteDisplayParams.Click += new System.EventHandler(this.btnWriteDisplayParams_Click);
            // 
            // btnReadDisplayParams
            // 
            this.btnReadDisplayParams.Location = new System.Drawing.Point(4, 286);
            this.btnReadDisplayParams.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadDisplayParams.Name = "btnReadDisplayParams";
            this.btnReadDisplayParams.Size = new System.Drawing.Size(100, 28);
            this.btnReadDisplayParams.TabIndex = 1;
            this.btnReadDisplayParams.Text = "Read";
            this.btnReadDisplayParams.UseVisualStyleBackColor = true;
            this.btnReadDisplayParams.Click += new System.EventHandler(this.btnReadDisplayParams_Click);
            // 
            // chkBoxSelectAll
            // 
            this.chkBoxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSelectAll.AutoSize = true;
            this.chkBoxSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxSelectAll.Location = new System.Drawing.Point(8, 259);
            this.chkBoxSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkBoxSelectAll.Name = "chkBoxSelectAll";
            this.chkBoxSelectAll.Size = new System.Drawing.Size(98, 21);
            this.chkBoxSelectAll.TabIndex = 3;
            this.chkBoxSelectAll.Text = "Select All";
            this.chkBoxSelectAll.UseVisualStyleBackColor = true;
            this.chkBoxSelectAll.CheckedChanged += new System.EventHandler(this.chkBoxSelectAll_CheckedChanged);
            // 
            // btnDownScroll
            // 
            this.btnDownScroll.Location = new System.Drawing.Point(17, 182);
            this.btnDownScroll.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownScroll.Name = "btnDownScroll";
            this.btnDownScroll.Size = new System.Drawing.Size(57, 50);
            this.btnDownScroll.TabIndex = 0;
            this.btnDownScroll.Text = "v";
            this.btnDownScroll.UseVisualStyleBackColor = true;
            this.btnDownScroll.Click += new System.EventHandler(this.btnDownScroll_Click);
            // 
            // tabKVAH
            // 
            this.tabKVAH.Controls.Add(this.lblKvahNotSupported);
            this.tabKVAH.Controls.Add(this.btnReadKVAhSelection);
            this.tabKVAH.Controls.Add(this.btnWriteKVAhSelection);
            this.tabKVAH.Controls.Add(this.groupBox59);
            this.tabKVAH.Location = new System.Drawing.Point(4, 25);
            this.tabKVAH.Margin = new System.Windows.Forms.Padding(4);
            this.tabKVAH.Name = "tabKVAH";
            this.tabKVAH.Size = new System.Drawing.Size(1212, 799);
            this.tabKVAH.TabIndex = 19;
            this.tabKVAH.Text = "kvah Selection";
            this.tabKVAH.UseVisualStyleBackColor = true;
            // 
            // lblKvahNotSupported
            // 
            this.lblKvahNotSupported.AutoSize = true;
            this.lblKvahNotSupported.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKvahNotSupported.ForeColor = System.Drawing.Color.Black;
            this.lblKvahNotSupported.Location = new System.Drawing.Point(23, 272);
            this.lblKvahNotSupported.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKvahNotSupported.Name = "lblKvahNotSupported";
            this.lblKvahNotSupported.Size = new System.Drawing.Size(62, 17);
            this.lblKvahNotSupported.TabIndex = 20;
            this.lblKvahNotSupported.Text = "NoKvah";
            this.lblKvahNotSupported.Visible = false;
            // 
            // btnReadKVAhSelection
            // 
            this.btnReadKVAhSelection.Location = new System.Drawing.Point(215, 178);
            this.btnReadKVAhSelection.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadKVAhSelection.Name = "btnReadKVAhSelection";
            this.btnReadKVAhSelection.Size = new System.Drawing.Size(100, 32);
            this.btnReadKVAhSelection.TabIndex = 19;
            this.btnReadKVAhSelection.Text = "Read";
            this.btnReadKVAhSelection.UseVisualStyleBackColor = true;
            this.btnReadKVAhSelection.Click += new System.EventHandler(this.btnReadKVAhSelection_Click);
            // 
            // btnWriteKVAhSelection
            // 
            this.btnWriteKVAhSelection.Location = new System.Drawing.Point(88, 178);
            this.btnWriteKVAhSelection.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteKVAhSelection.Name = "btnWriteKVAhSelection";
            this.btnWriteKVAhSelection.Size = new System.Drawing.Size(100, 32);
            this.btnWriteKVAhSelection.TabIndex = 18;
            this.btnWriteKVAhSelection.Text = "Write";
            this.btnWriteKVAhSelection.UseVisualStyleBackColor = true;
            this.btnWriteKVAhSelection.Click += new System.EventHandler(this.btnWriteKVAhSelection_Click);
            // 
            // groupBox59
            // 
            this.groupBox59.Controls.Add(this.chkKVAhLagLead);
            this.groupBox59.Controls.Add(this.chkKVAhLagOnly);
            this.groupBox59.Location = new System.Drawing.Point(27, 26);
            this.groupBox59.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox59.Name = "groupBox59";
            this.groupBox59.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox59.Size = new System.Drawing.Size(413, 134);
            this.groupBox59.TabIndex = 17;
            this.groupBox59.TabStop = false;
            this.groupBox59.Text = "kvah Selection";
            // 
            // chkKVAhLagLead
            // 
            this.chkKVAhLagLead.AutoSize = true;
            this.chkKVAhLagLead.Location = new System.Drawing.Point(215, 57);
            this.chkKVAhLagLead.Margin = new System.Windows.Forms.Padding(4);
            this.chkKVAhLagLead.Name = "chkKVAhLagLead";
            this.chkKVAhLagLead.Size = new System.Drawing.Size(158, 21);
            this.chkKVAhLagLead.TabIndex = 1;
            this.chkKVAhLagLead.Text = "Lag + Lead (Unlock)";
            this.chkKVAhLagLead.UseVisualStyleBackColor = true;
            // 
            // chkKVAhLagOnly
            // 
            this.chkKVAhLagOnly.AutoSize = true;
            this.chkKVAhLagOnly.Location = new System.Drawing.Point(67, 57);
            this.chkKVAhLagOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkKVAhLagOnly.Name = "chkKVAhLagOnly";
            this.chkKVAhLagOnly.Size = new System.Drawing.Size(130, 21);
            this.chkKVAhLagOnly.TabIndex = 0;
            this.chkKVAhLagOnly.Text = "Lag Only (Lock)";
            this.chkKVAhLagOnly.UseVisualStyleBackColor = true;
            // 
            // tabRS232Lock
            // 
            this.tabRS232Lock.Controls.Add(this.button10);
            this.tabRS232Lock.Controls.Add(this.button11);
            this.tabRS232Lock.Controls.Add(this.groupBox14);
            this.tabRS232Lock.Location = new System.Drawing.Point(4, 25);
            this.tabRS232Lock.Margin = new System.Windows.Forms.Padding(4);
            this.tabRS232Lock.Name = "tabRS232Lock";
            this.tabRS232Lock.Size = new System.Drawing.Size(1212, 799);
            this.tabRS232Lock.TabIndex = 20;
            this.tabRS232Lock.Text = "RS232 Lock";
            this.tabRS232Lock.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(205, 183);
            this.button10.Margin = new System.Windows.Forms.Padding(4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(100, 32);
            this.button10.TabIndex = 22;
            this.button10.Text = "Read";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(79, 183);
            this.button11.Margin = new System.Windows.Forms.Padding(4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(100, 32);
            this.button11.TabIndex = 21;
            this.button11.Text = "Write";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.radioButton9);
            this.groupBox14.Controls.Add(this.radioButton10);
            this.groupBox14.Location = new System.Drawing.Point(17, 31);
            this.groupBox14.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox14.Size = new System.Drawing.Size(413, 134);
            this.groupBox14.TabIndex = 20;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "RS232 Lock/Unlock Selection";
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(215, 57);
            this.radioButton9.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(72, 21);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.Text = "Unlock";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(67, 57);
            this.radioButton10.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(59, 21);
            this.radioButton10.TabIndex = 0;
            this.radioButton10.Text = "Lock";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // tabCMRI
            // 
            this.tabCMRI.AutoScroll = true;
            this.tabCMRI.Controls.Add(this.groupBox7);
            this.tabCMRI.Location = new System.Drawing.Point(4, 25);
            this.tabCMRI.Margin = new System.Windows.Forms.Padding(4);
            this.tabCMRI.Name = "tabCMRI";
            this.tabCMRI.Padding = new System.Windows.Forms.Padding(4);
            this.tabCMRI.Size = new System.Drawing.Size(1223, 833);
            this.tabCMRI.TabIndex = 6;
            this.tabCMRI.Text = "CMRI Preparation";
            this.tabCMRI.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button5);
            this.groupBox7.Controls.Add(this.btnGenerateFile);
            this.groupBox7.Controls.Add(this.button4);
            this.groupBox7.Controls.Add(this.btnLoadFile);
            this.groupBox7.Controls.Add(this.btnWriteAll);
            this.groupBox7.Location = new System.Drawing.Point(31, 31);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(375, 348);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(115, 282);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(144, 32);
            this.button5.TabIndex = 18;
            this.button5.Text = "Cancel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.Location = new System.Drawing.Point(115, 97);
            this.btnGenerateFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(144, 32);
            this.btnGenerateFile.TabIndex = 14;
            this.btnGenerateFile.Text = "Generate CFC File";
            this.btnGenerateFile.UseVisualStyleBackColor = true;
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateFile_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(115, 159);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(144, 32);
            this.button4.TabIndex = 17;
            this.button4.Text = "Prepare CMRI";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(115, 36);
            this.btnLoadFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(144, 32);
            this.btnLoadFile.TabIndex = 15;
            this.btnLoadFile.Text = "Load CFC File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnWriteAll
            // 
            this.btnWriteAll.Location = new System.Drawing.Point(115, 220);
            this.btnWriteAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteAll.Name = "btnWriteAll";
            this.btnWriteAll.Size = new System.Drawing.Size(144, 32);
            this.btnWriteAll.TabIndex = 16;
            this.btnWriteAll.Text = "Update CMRI RTC";
            this.btnWriteAll.UseVisualStyleBackColor = true;
            this.btnWriteAll.Click += new System.EventHandler(this.btnWriteAll_Click);
            // 
            // tabGSM
            // 
            this.tabGSM.AutoScroll = true;
            this.tabGSM.Controls.Add(this.textBox1);
            this.tabGSM.Controls.Add(this.button9);
            this.tabGSM.Controls.Add(this.dataGridView1);
            this.tabGSM.Controls.Add(this.button3);
            this.tabGSM.Controls.Add(this.btnDisconnect);
            this.tabGSM.Controls.Add(this.btnConnect);
            this.tabGSM.Controls.Add(this.groupBox1);
            this.tabGSM.Location = new System.Drawing.Point(4, 25);
            this.tabGSM.Margin = new System.Windows.Forms.Padding(4);
            this.tabGSM.Name = "tabGSM";
            this.tabGSM.Padding = new System.Windows.Forms.Padding(4);
            this.tabGSM.Size = new System.Drawing.Size(1223, 833);
            this.tabGSM.TabIndex = 4;
            this.tabGSM.Text = "GSM";
            this.tabGSM.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(527, 50);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.MaxLength = 11;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 22);
            this.textBox1.TabIndex = 52;
            this.textBox1.Text = "2";
            this.textBox1.Visible = false;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(527, 82);
            this.button9.Margin = new System.Windows.Forms.Padding(4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(177, 162);
            this.button9.TabIndex = 51;
            this.button9.Text = "test";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.dataGridView1.Location = new System.Drawing.Point(751, 48);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(401, 535);
            this.dataGridView1.TabIndex = 50;
            this.dataGridView1.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "S.No.";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Real Time Clock";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 175;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(336, 213);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 32);
            this.button3.TabIndex = 45;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(208, 213);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(95, 32);
            this.btnDisconnect.TabIndex = 44;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(80, 213);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 32);
            this.btnConnect.TabIndex = 43;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.textBoxGSM);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(49, 48);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(400, 140);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GSM Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Location = new System.Drawing.Point(103, 357);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(345, 76);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication Port";
            this.groupBox2.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(37, 36);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(77, 21);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "RS-232";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(172, 36);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(103, 21);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.Text = "Optical Port";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // textBoxGSM
            // 
            this.textBoxGSM.Location = new System.Drawing.Point(180, 57);
            this.textBoxGSM.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGSM.MaxLength = 11;
            this.textBoxGSM.Name = "textBoxGSM";
            this.textBoxGSM.Size = new System.Drawing.Size(172, 22);
            this.textBoxGSM.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.radioButton4);
            this.groupBox3.Location = new System.Drawing.Point(119, 246);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(345, 78);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Communication Mode";
            this.groupBox3.Visible = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(40, 36);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(78, 21);
            this.radioButton3.TabIndex = 10;
            this.radioButton3.Text = "Mode-E";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(172, 36);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(108, 21);
            this.radioButton4.TabIndex = 9;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Direct-HDLC";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(71, 63);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "SIM Number";
            // 
            // tabPCBA
            // 
            this.tabPCBA.Controls.Add(this.grdPCBA);
            this.tabPCBA.Controls.Add(this.lblPCBAMeterID);
            this.tabPCBA.Controls.Add(this.lblDisplayMeterId);
            this.tabPCBA.Controls.Add(this.btnPCBAExport);
            this.tabPCBA.Controls.Add(this.btnPCBARead);
            this.tabPCBA.Location = new System.Drawing.Point(4, 25);
            this.tabPCBA.Margin = new System.Windows.Forms.Padding(4);
            this.tabPCBA.Name = "tabPCBA";
            this.tabPCBA.Size = new System.Drawing.Size(1223, 833);
            this.tabPCBA.TabIndex = 7;
            this.tabPCBA.Text = "PCBA Read";
            this.tabPCBA.UseVisualStyleBackColor = true;
            // 
            // grdPCBA
            // 
            this.grdPCBA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPCBA.Location = new System.Drawing.Point(12, 50);
            this.grdPCBA.Margin = new System.Windows.Forms.Padding(4);
            this.grdPCBA.Name = "grdPCBA";
            this.grdPCBA.Size = new System.Drawing.Size(337, 124);
            this.grdPCBA.TabIndex = 5;
            // 
            // lblPCBAMeterID
            // 
            this.lblPCBAMeterID.AutoSize = true;
            this.lblPCBAMeterID.Location = new System.Drawing.Point(93, 18);
            this.lblPCBAMeterID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPCBAMeterID.Name = "lblPCBAMeterID";
            this.lblPCBAMeterID.Size = new System.Drawing.Size(54, 17);
            this.lblPCBAMeterID.TabIndex = 4;
            this.lblPCBAMeterID.Text = "label13";
            // 
            // lblDisplayMeterId
            // 
            this.lblDisplayMeterId.AutoSize = true;
            this.lblDisplayMeterId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayMeterId.Location = new System.Drawing.Point(4, 18);
            this.lblDisplayMeterId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDisplayMeterId.Name = "lblDisplayMeterId";
            this.lblDisplayMeterId.Size = new System.Drawing.Size(69, 17);
            this.lblDisplayMeterId.TabIndex = 3;
            this.lblDisplayMeterId.Text = "Meter ID";
            // 
            // btnPCBAExport
            // 
            this.btnPCBAExport.Location = new System.Drawing.Point(136, 197);
            this.btnPCBAExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnPCBAExport.Name = "btnPCBAExport";
            this.btnPCBAExport.Size = new System.Drawing.Size(100, 28);
            this.btnPCBAExport.TabIndex = 1;
            this.btnPCBAExport.Text = "Export";
            this.btnPCBAExport.UseVisualStyleBackColor = true;
            this.btnPCBAExport.Click += new System.EventHandler(this.btnPCBAExport_Click);
            // 
            // btnPCBARead
            // 
            this.btnPCBARead.Location = new System.Drawing.Point(11, 197);
            this.btnPCBARead.Margin = new System.Windows.Forms.Padding(4);
            this.btnPCBARead.Name = "btnPCBARead";
            this.btnPCBARead.Size = new System.Drawing.Size(100, 28);
            this.btnPCBARead.TabIndex = 0;
            this.btnPCBARead.Text = "Read";
            this.btnPCBARead.UseVisualStyleBackColor = true;
            this.btnPCBARead.Click += new System.EventHandler(this.btnPCBARead_Click);
            // 
            // tabMeterAccuracyCheck
            // 
            this.tabMeterAccuracyCheck.AutoScroll = true;
            this.tabMeterAccuracyCheck.Controls.Add(this.lblNoMeterAccuracyCheck);
            this.tabMeterAccuracyCheck.Controls.Add(this.btnAccuracyCheckCancel);
            this.tabMeterAccuracyCheck.Controls.Add(this.btnStart);
            this.tabMeterAccuracyCheck.Controls.Add(this.gbMeterAccuracyCheck);
            this.tabMeterAccuracyCheck.Location = new System.Drawing.Point(4, 25);
            this.tabMeterAccuracyCheck.Margin = new System.Windows.Forms.Padding(4);
            this.tabMeterAccuracyCheck.Name = "tabMeterAccuracyCheck";
            this.tabMeterAccuracyCheck.Size = new System.Drawing.Size(1223, 833);
            this.tabMeterAccuracyCheck.TabIndex = 8;
            this.tabMeterAccuracyCheck.Text = "Meter Accuracy Check";
            this.tabMeterAccuracyCheck.UseVisualStyleBackColor = true;
            this.tabMeterAccuracyCheck.Click += new System.EventHandler(this.tabMeterAccuracyCheck_Click);
            // 
            // lblNoMeterAccuracyCheck
            // 
            this.lblNoMeterAccuracyCheck.AutoSize = true;
            this.lblNoMeterAccuracyCheck.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoMeterAccuracyCheck.ForeColor = System.Drawing.Color.Black;
            this.lblNoMeterAccuracyCheck.Location = new System.Drawing.Point(24, 441);
            this.lblNoMeterAccuracyCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoMeterAccuracyCheck.Name = "lblNoMeterAccuracyCheck";
            this.lblNoMeterAccuracyCheck.Size = new System.Drawing.Size(75, 17);
            this.lblNoMeterAccuracyCheck.TabIndex = 9;
            this.lblNoMeterAccuracyCheck.Text = "NoPhasor";
            this.lblNoMeterAccuracyCheck.Visible = false;
            // 
            // btnAccuracyCheckCancel
            // 
            this.btnAccuracyCheckCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccuracyCheckCancel.Location = new System.Drawing.Point(532, 399);
            this.btnAccuracyCheckCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnAccuracyCheckCancel.Name = "btnAccuracyCheckCancel";
            this.btnAccuracyCheckCancel.Size = new System.Drawing.Size(100, 28);
            this.btnAccuracyCheckCancel.TabIndex = 8;
            this.btnAccuracyCheckCancel.Text = "Cancel";
            this.btnAccuracyCheckCancel.UseVisualStyleBackColor = true;
            this.btnAccuracyCheckCancel.Click += new System.EventHandler(this.btnAccuracyCheckCancel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(408, 399);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 28);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbMeterAccuracyCheck
            // 
            this.gbMeterAccuracyCheck.Controls.Add(this.lblReactiveLeadUnit);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblReactiveLagUnit);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblApparentEnergyUnit);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblActiveEnergyUnit);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblduration);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLagInitial);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLeadDelta);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLeadFinal);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLeadInitial);
            this.gbMeterAccuracyCheck.Controls.Add(this.label1);
            this.gbMeterAccuracyCheck.Controls.Add(this.cmbTestduration);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblTestDuration);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLagDelta);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkVAhDelta);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkWhDelta);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkvarhLagFinal);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkVAhFinal);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkWhFinal);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkVAhInitial);
            this.gbMeterAccuracyCheck.Controls.Add(this.txtkWhInitial);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblDelta);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblFinalReading);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblInitialReading);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblkvarh);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblkVAh);
            this.gbMeterAccuracyCheck.Controls.Add(this.lblkWh);
            this.gbMeterAccuracyCheck.Location = new System.Drawing.Point(28, 30);
            this.gbMeterAccuracyCheck.Margin = new System.Windows.Forms.Padding(4);
            this.gbMeterAccuracyCheck.Name = "gbMeterAccuracyCheck";
            this.gbMeterAccuracyCheck.Padding = new System.Windows.Forms.Padding(4);
            this.gbMeterAccuracyCheck.Size = new System.Drawing.Size(728, 341);
            this.gbMeterAccuracyCheck.TabIndex = 6;
            this.gbMeterAccuracyCheck.TabStop = false;
            this.gbMeterAccuracyCheck.Text = "Meter Accuracy Check";
            // 
            // lblReactiveLeadUnit
            // 
            this.lblReactiveLeadUnit.AutoSize = true;
            this.lblReactiveLeadUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactiveLeadUnit.Location = new System.Drawing.Point(632, 219);
            this.lblReactiveLeadUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReactiveLeadUnit.Name = "lblReactiveLeadUnit";
            this.lblReactiveLeadUnit.Size = new System.Drawing.Size(46, 17);
            this.lblReactiveLeadUnit.TabIndex = 25;
            this.lblReactiveLeadUnit.Text = "kVArh";
            this.lblReactiveLeadUnit.Visible = false;
            // 
            // lblReactiveLagUnit
            // 
            this.lblReactiveLagUnit.AutoSize = true;
            this.lblReactiveLagUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactiveLagUnit.Location = new System.Drawing.Point(632, 170);
            this.lblReactiveLagUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReactiveLagUnit.Name = "lblReactiveLagUnit";
            this.lblReactiveLagUnit.Size = new System.Drawing.Size(46, 17);
            this.lblReactiveLagUnit.TabIndex = 24;
            this.lblReactiveLagUnit.Text = "kVArh";
            this.lblReactiveLagUnit.Visible = false;
            // 
            // lblApparentEnergyUnit
            // 
            this.lblApparentEnergyUnit.AutoSize = true;
            this.lblApparentEnergyUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApparentEnergyUnit.Location = new System.Drawing.Point(632, 123);
            this.lblApparentEnergyUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApparentEnergyUnit.Name = "lblApparentEnergyUnit";
            this.lblApparentEnergyUnit.Size = new System.Drawing.Size(41, 17);
            this.lblApparentEnergyUnit.TabIndex = 23;
            this.lblApparentEnergyUnit.Text = "kVAh";
            this.lblApparentEnergyUnit.Visible = false;
            // 
            // lblActiveEnergyUnit
            // 
            this.lblActiveEnergyUnit.AutoSize = true;
            this.lblActiveEnergyUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblActiveEnergyUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveEnergyUnit.Location = new System.Drawing.Point(632, 70);
            this.lblActiveEnergyUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActiveEnergyUnit.Name = "lblActiveEnergyUnit";
            this.lblActiveEnergyUnit.Size = new System.Drawing.Size(36, 17);
            this.lblActiveEnergyUnit.TabIndex = 22;
            this.lblActiveEnergyUnit.Text = "kWh";
            this.lblActiveEnergyUnit.Visible = false;
            // 
            // lblduration
            // 
            this.lblduration.AutoSize = true;
            this.lblduration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblduration.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblduration.Location = new System.Drawing.Point(55, 299);
            this.lblduration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblduration.Name = "lblduration";
            this.lblduration.Size = new System.Drawing.Size(0, 17);
            this.lblduration.TabIndex = 21;
            this.lblduration.Visible = false;
            // 
            // txtkvarhLagInitial
            // 
            this.txtkvarhLagInitial.Location = new System.Drawing.Point(176, 166);
            this.txtkvarhLagInitial.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLagInitial.Name = "txtkvarhLagInitial";
            this.txtkvarhLagInitial.ReadOnly = true;
            this.txtkvarhLagInitial.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLagInitial.TabIndex = 8;
            // 
            // txtkvarhLeadDelta
            // 
            this.txtkvarhLeadDelta.Location = new System.Drawing.Point(469, 215);
            this.txtkvarhLeadDelta.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLeadDelta.Name = "txtkvarhLeadDelta";
            this.txtkvarhLeadDelta.ReadOnly = true;
            this.txtkvarhLeadDelta.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLeadDelta.TabIndex = 20;
            // 
            // txtkvarhLeadFinal
            // 
            this.txtkvarhLeadFinal.Location = new System.Drawing.Point(323, 215);
            this.txtkvarhLeadFinal.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLeadFinal.Name = "txtkvarhLeadFinal";
            this.txtkvarhLeadFinal.ReadOnly = true;
            this.txtkvarhLeadFinal.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLeadFinal.TabIndex = 19;
            // 
            // txtkvarhLeadInitial
            // 
            this.txtkvarhLeadInitial.Location = new System.Drawing.Point(176, 215);
            this.txtkvarhLeadInitial.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLeadInitial.Name = "txtkvarhLeadInitial";
            this.txtkvarhLeadInitial.ReadOnly = true;
            this.txtkvarhLeadInitial.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLeadInitial.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 219);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Reactive Energy(Lead)";
            // 
            // cmbTestduration
            // 
            this.cmbTestduration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestduration.FormattingEnabled = true;
            this.cmbTestduration.Location = new System.Drawing.Point(249, 265);
            this.cmbTestduration.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTestduration.Name = "cmbTestduration";
            this.cmbTestduration.Size = new System.Drawing.Size(53, 24);
            this.cmbTestduration.TabIndex = 16;
            // 
            // lblTestDuration
            // 
            this.lblTestDuration.AutoSize = true;
            this.lblTestDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestDuration.Location = new System.Drawing.Point(55, 268);
            this.lblTestDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestDuration.Name = "lblTestDuration";
            this.lblTestDuration.Size = new System.Drawing.Size(157, 17);
            this.lblTestDuration.TabIndex = 15;
            this.lblTestDuration.Text = "Test Duration (Minutes)";
            // 
            // txtkvarhLagDelta
            // 
            this.txtkvarhLagDelta.Location = new System.Drawing.Point(469, 166);
            this.txtkvarhLagDelta.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLagDelta.Name = "txtkvarhLagDelta";
            this.txtkvarhLagDelta.ReadOnly = true;
            this.txtkvarhLagDelta.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLagDelta.TabIndex = 14;
            // 
            // txtkVAhDelta
            // 
            this.txtkVAhDelta.Location = new System.Drawing.Point(469, 114);
            this.txtkVAhDelta.Margin = new System.Windows.Forms.Padding(4);
            this.txtkVAhDelta.Name = "txtkVAhDelta";
            this.txtkVAhDelta.ReadOnly = true;
            this.txtkVAhDelta.Size = new System.Drawing.Size(137, 22);
            this.txtkVAhDelta.TabIndex = 13;
            // 
            // txtkWhDelta
            // 
            this.txtkWhDelta.Location = new System.Drawing.Point(469, 62);
            this.txtkWhDelta.Margin = new System.Windows.Forms.Padding(4);
            this.txtkWhDelta.Name = "txtkWhDelta";
            this.txtkWhDelta.ReadOnly = true;
            this.txtkWhDelta.Size = new System.Drawing.Size(137, 22);
            this.txtkWhDelta.TabIndex = 12;
            // 
            // txtkvarhLagFinal
            // 
            this.txtkvarhLagFinal.Location = new System.Drawing.Point(323, 166);
            this.txtkvarhLagFinal.Margin = new System.Windows.Forms.Padding(4);
            this.txtkvarhLagFinal.Name = "txtkvarhLagFinal";
            this.txtkvarhLagFinal.ReadOnly = true;
            this.txtkvarhLagFinal.Size = new System.Drawing.Size(137, 22);
            this.txtkvarhLagFinal.TabIndex = 11;
            // 
            // txtkVAhFinal
            // 
            this.txtkVAhFinal.Location = new System.Drawing.Point(323, 114);
            this.txtkVAhFinal.Margin = new System.Windows.Forms.Padding(4);
            this.txtkVAhFinal.Name = "txtkVAhFinal";
            this.txtkVAhFinal.ReadOnly = true;
            this.txtkVAhFinal.Size = new System.Drawing.Size(137, 22);
            this.txtkVAhFinal.TabIndex = 10;
            // 
            // txtkWhFinal
            // 
            this.txtkWhFinal.Location = new System.Drawing.Point(323, 62);
            this.txtkWhFinal.Margin = new System.Windows.Forms.Padding(4);
            this.txtkWhFinal.Name = "txtkWhFinal";
            this.txtkWhFinal.ReadOnly = true;
            this.txtkWhFinal.Size = new System.Drawing.Size(137, 22);
            this.txtkWhFinal.TabIndex = 9;
            // 
            // txtkVAhInitial
            // 
            this.txtkVAhInitial.Location = new System.Drawing.Point(176, 114);
            this.txtkVAhInitial.Margin = new System.Windows.Forms.Padding(4);
            this.txtkVAhInitial.Name = "txtkVAhInitial";
            this.txtkVAhInitial.ReadOnly = true;
            this.txtkVAhInitial.Size = new System.Drawing.Size(137, 22);
            this.txtkVAhInitial.TabIndex = 7;
            // 
            // txtkWhInitial
            // 
            this.txtkWhInitial.Location = new System.Drawing.Point(176, 62);
            this.txtkWhInitial.Margin = new System.Windows.Forms.Padding(4);
            this.txtkWhInitial.Name = "txtkWhInitial";
            this.txtkWhInitial.ReadOnly = true;
            this.txtkWhInitial.Size = new System.Drawing.Size(137, 22);
            this.txtkWhInitial.TabIndex = 6;
            // 
            // lblDelta
            // 
            this.lblDelta.AutoSize = true;
            this.lblDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelta.Location = new System.Drawing.Point(488, 33);
            this.lblDelta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(41, 17);
            this.lblDelta.TabIndex = 5;
            this.lblDelta.Text = "Delta";
            // 
            // lblFinalReading
            // 
            this.lblFinalReading.AutoSize = true;
            this.lblFinalReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalReading.Location = new System.Drawing.Point(335, 33);
            this.lblFinalReading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFinalReading.Name = "lblFinalReading";
            this.lblFinalReading.Size = new System.Drawing.Size(95, 17);
            this.lblFinalReading.TabIndex = 4;
            this.lblFinalReading.Text = "Final Reading";
            // 
            // lblInitialReading
            // 
            this.lblInitialReading.AutoSize = true;
            this.lblInitialReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialReading.Location = new System.Drawing.Point(185, 33);
            this.lblInitialReading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInitialReading.Name = "lblInitialReading";
            this.lblInitialReading.Size = new System.Drawing.Size(97, 17);
            this.lblInitialReading.TabIndex = 3;
            this.lblInitialReading.Text = "Initial Reading";
            // 
            // lblkvarh
            // 
            this.lblkvarh.AutoSize = true;
            this.lblkvarh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkvarh.Location = new System.Drawing.Point(15, 175);
            this.lblkvarh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblkvarh.Name = "lblkvarh";
            this.lblkvarh.Size = new System.Drawing.Size(146, 17);
            this.lblkvarh.TabIndex = 2;
            this.lblkvarh.Text = "Reactive Energy(Lag)";
            // 
            // lblkVAh
            // 
            this.lblkVAh.AutoSize = true;
            this.lblkVAh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkVAh.Location = new System.Drawing.Point(15, 123);
            this.lblkVAh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblkVAh.Name = "lblkVAh";
            this.lblkVAh.Size = new System.Drawing.Size(115, 17);
            this.lblkVAh.TabIndex = 1;
            this.lblkVAh.Text = "Apparent Energy";
            // 
            // lblkWh
            // 
            this.lblkWh.AutoSize = true;
            this.lblkWh.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblkWh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkWh.Location = new System.Drawing.Point(15, 70);
            this.lblkWh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblkWh.Name = "lblkWh";
            this.lblkWh.Size = new System.Drawing.Size(95, 17);
            this.lblkWh.TabIndex = 0;
            this.lblkWh.Text = "Active Energy";
            // 
            // tabPhasor
            // 
            this.tabPhasor.AutoScroll = true;
            this.tabPhasor.Controls.Add(this.lblPhasorNotSupported);
            this.tabPhasor.Controls.Add(this.groupBox12);
            this.tabPhasor.Controls.Add(this.lblPhasorData);
            this.tabPhasor.Controls.Add(this.btnCancelPhasor);
            this.tabPhasor.Controls.Add(this.btnHold);
            this.tabPhasor.Controls.Add(this.btnReadPhasor);
            this.tabPhasor.Controls.Add(this.phasorDiagram1);
            this.tabPhasor.Location = new System.Drawing.Point(4, 25);
            this.tabPhasor.Margin = new System.Windows.Forms.Padding(4);
            this.tabPhasor.Name = "tabPhasor";
            this.tabPhasor.Padding = new System.Windows.Forms.Padding(4);
            this.tabPhasor.Size = new System.Drawing.Size(1223, 833);
            this.tabPhasor.TabIndex = 9;
            this.tabPhasor.Text = "Dynamic Phasor";
            this.tabPhasor.UseVisualStyleBackColor = true;
            // 
            // lblPhasorNotSupported
            // 
            this.lblPhasorNotSupported.AutoSize = true;
            this.lblPhasorNotSupported.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhasorNotSupported.ForeColor = System.Drawing.Color.Black;
            this.lblPhasorNotSupported.Location = new System.Drawing.Point(36, 486);
            this.lblPhasorNotSupported.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhasorNotSupported.Name = "lblPhasorNotSupported";
            this.lblPhasorNotSupported.Size = new System.Drawing.Size(75, 17);
            this.lblPhasorNotSupported.TabIndex = 8;
            this.lblPhasorNotSupported.Text = "NoPhasor";
            this.lblPhasorNotSupported.Visible = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.lblAngelYRValue);
            this.groupBox12.Controls.Add(this.lblAngleBRValue);
            this.groupBox12.Controls.Add(this.lblAngleBwTwoValue);
            this.groupBox12.Controls.Add(this.lblTotalPWFactorValue);
            this.groupBox12.Controls.Add(this.lblRLagLeadValue);
            this.groupBox12.Controls.Add(this.lblYLagLeadValue);
            this.groupBox12.Controls.Add(this.lblYChannelValue);
            this.groupBox12.Controls.Add(this.lblBChannelValue);
            this.groupBox12.Controls.Add(this.lblBLagLeadValue);
            this.groupBox12.Controls.Add(this.lblBPhaseKWDirValue);
            this.groupBox12.Controls.Add(this.lblRChannelValue);
            this.groupBox12.Controls.Add(this.lblRPhaseKWDirVAlue);
            this.groupBox12.Controls.Add(this.lblYPhaseKWDirValue);
            this.groupBox12.Controls.Add(this.lblFrequencyValue);
            this.groupBox12.Controls.Add(this.lblYPhasePFValue);
            this.groupBox12.Controls.Add(this.lblBPhaesPFValue);
            this.groupBox12.Controls.Add(this.lblRPhasePFValue);
            this.groupBox12.Controls.Add(this.lblPhaseSeqValue);
            this.groupBox12.Controls.Add(this.lblReactivePowerValue);
            this.groupBox12.Controls.Add(this.lblApparentPowerValue);
            this.groupBox12.Controls.Add(this.lblBCurrentValue);
            this.groupBox12.Controls.Add(this.lblActivePowerValue);
            this.groupBox12.Controls.Add(this.lblRCurrentValue);
            this.groupBox12.Controls.Add(this.lblYCurrentValue);
            this.groupBox12.Controls.Add(this.lblYVoltageValue);
            this.groupBox12.Controls.Add(this.lblBVoltageValue);
            this.groupBox12.Controls.Add(this.lblRVoltageValue);
            this.groupBox12.Controls.Add(this.lblAngelYR);
            this.groupBox12.Controls.Add(this.lblAngleBR);
            this.groupBox12.Controls.Add(this.lblAngleBwTwo);
            this.groupBox12.Controls.Add(this.lblTotalPWFactor);
            this.groupBox12.Controls.Add(this.lblRLagLead);
            this.groupBox12.Controls.Add(this.lblYLagLead);
            this.groupBox12.Controls.Add(this.lblYChannel);
            this.groupBox12.Controls.Add(this.lblBChannel);
            this.groupBox12.Controls.Add(this.lblBLagLead);
            this.groupBox12.Controls.Add(this.lblFrequency);
            this.groupBox12.Controls.Add(this.lblBPhaseKWDir);
            this.groupBox12.Controls.Add(this.lblRChannel);
            this.groupBox12.Controls.Add(this.lblYPhasePF);
            this.groupBox12.Controls.Add(this.lblBPhasePF);
            this.groupBox12.Controls.Add(this.lblRPhaseKWDir);
            this.groupBox12.Controls.Add(this.lblYPhaseKWDir);
            this.groupBox12.Controls.Add(this.lblRPhasePF);
            this.groupBox12.Controls.Add(this.lblPhaseSeq);
            this.groupBox12.Controls.Add(this.lblReactivePower);
            this.groupBox12.Controls.Add(this.lblApparentPower);
            this.groupBox12.Controls.Add(this.lblBCurrent);
            this.groupBox12.Controls.Add(this.lblActivePower);
            this.groupBox12.Controls.Add(this.lblRCurrent);
            this.groupBox12.Controls.Add(this.lblYCurrent);
            this.groupBox12.Controls.Add(this.lblYVoltage);
            this.groupBox12.Controls.Add(this.lblBVoltage);
            this.groupBox12.Controls.Add(this.lblRPhaseVoltage);
            this.groupBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.Location = new System.Drawing.Point(635, 0);
            this.groupBox12.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox12.Size = new System.Drawing.Size(585, 466);
            this.groupBox12.TabIndex = 6;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Phasor Parameters";
            this.groupBox12.Enter += new System.EventHandler(this.groupBox12_Enter);
            // 
            // lblAngelYRValue
            // 
            this.lblAngelYRValue.AutoSize = true;
            this.lblAngelYRValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngelYRValue.Location = new System.Drawing.Point(500, 320);
            this.lblAngelYRValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngelYRValue.Name = "lblAngelYRValue";
            this.lblAngelYRValue.Size = new System.Drawing.Size(12, 17);
            this.lblAngelYRValue.TabIndex = 53;
            this.lblAngelYRValue.Text = " ";
            // 
            // lblAngleBRValue
            // 
            this.lblAngleBRValue.AutoSize = true;
            this.lblAngleBRValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleBRValue.Location = new System.Drawing.Point(500, 350);
            this.lblAngleBRValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngleBRValue.Name = "lblAngleBRValue";
            this.lblAngleBRValue.Size = new System.Drawing.Size(12, 17);
            this.lblAngleBRValue.TabIndex = 52;
            this.lblAngleBRValue.Text = " ";
            // 
            // lblAngleBwTwoValue
            // 
            this.lblAngleBwTwoValue.AutoSize = true;
            this.lblAngleBwTwoValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleBwTwoValue.Location = new System.Drawing.Point(500, 379);
            this.lblAngleBwTwoValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngleBwTwoValue.Name = "lblAngleBwTwoValue";
            this.lblAngleBwTwoValue.Size = new System.Drawing.Size(12, 17);
            this.lblAngleBwTwoValue.TabIndex = 51;
            this.lblAngleBwTwoValue.Text = " ";
            // 
            // lblTotalPWFactorValue
            // 
            this.lblTotalPWFactorValue.AutoSize = true;
            this.lblTotalPWFactorValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPWFactorValue.Location = new System.Drawing.Point(525, 410);
            this.lblTotalPWFactorValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalPWFactorValue.Name = "lblTotalPWFactorValue";
            this.lblTotalPWFactorValue.Size = new System.Drawing.Size(12, 17);
            this.lblTotalPWFactorValue.TabIndex = 50;
            this.lblTotalPWFactorValue.Text = " ";
            // 
            // lblRLagLeadValue
            // 
            this.lblRLagLeadValue.AutoSize = true;
            this.lblRLagLeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRLagLeadValue.Location = new System.Drawing.Point(500, 222);
            this.lblRLagLeadValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRLagLeadValue.Name = "lblRLagLeadValue";
            this.lblRLagLeadValue.Size = new System.Drawing.Size(12, 17);
            this.lblRLagLeadValue.TabIndex = 49;
            this.lblRLagLeadValue.Text = " ";
            // 
            // lblYLagLeadValue
            // 
            this.lblYLagLeadValue.AutoSize = true;
            this.lblYLagLeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYLagLeadValue.Location = new System.Drawing.Point(500, 258);
            this.lblYLagLeadValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYLagLeadValue.Name = "lblYLagLeadValue";
            this.lblYLagLeadValue.Size = new System.Drawing.Size(12, 17);
            this.lblYLagLeadValue.TabIndex = 48;
            this.lblYLagLeadValue.Text = " ";
            // 
            // lblYChannelValue
            // 
            this.lblYChannelValue.AutoSize = true;
            this.lblYChannelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYChannelValue.Location = new System.Drawing.Point(500, 155);
            this.lblYChannelValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYChannelValue.Name = "lblYChannelValue";
            this.lblYChannelValue.Size = new System.Drawing.Size(12, 17);
            this.lblYChannelValue.TabIndex = 47;
            this.lblYChannelValue.Text = " ";
            // 
            // lblBChannelValue
            // 
            this.lblBChannelValue.AutoSize = true;
            this.lblBChannelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBChannelValue.Location = new System.Drawing.Point(500, 187);
            this.lblBChannelValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBChannelValue.Name = "lblBChannelValue";
            this.lblBChannelValue.Size = new System.Drawing.Size(12, 17);
            this.lblBChannelValue.TabIndex = 46;
            this.lblBChannelValue.Text = " ";
            // 
            // lblBLagLeadValue
            // 
            this.lblBLagLeadValue.AutoSize = true;
            this.lblBLagLeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBLagLeadValue.Location = new System.Drawing.Point(500, 290);
            this.lblBLagLeadValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBLagLeadValue.Name = "lblBLagLeadValue";
            this.lblBLagLeadValue.Size = new System.Drawing.Size(12, 17);
            this.lblBLagLeadValue.TabIndex = 45;
            this.lblBLagLeadValue.Text = " ";
            // 
            // lblBPhaseKWDirValue
            // 
            this.lblBPhaseKWDirValue.AutoSize = true;
            this.lblBPhaseKWDirValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBPhaseKWDirValue.Location = new System.Drawing.Point(500, 92);
            this.lblBPhaseKWDirValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBPhaseKWDirValue.Name = "lblBPhaseKWDirValue";
            this.lblBPhaseKWDirValue.Size = new System.Drawing.Size(12, 17);
            this.lblBPhaseKWDirValue.TabIndex = 44;
            this.lblBPhaseKWDirValue.Text = " ";
            // 
            // lblRChannelValue
            // 
            this.lblRChannelValue.AutoSize = true;
            this.lblRChannelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRChannelValue.Location = new System.Drawing.Point(500, 123);
            this.lblRChannelValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRChannelValue.Name = "lblRChannelValue";
            this.lblRChannelValue.Size = new System.Drawing.Size(12, 17);
            this.lblRChannelValue.TabIndex = 43;
            this.lblRChannelValue.Text = " ";
            // 
            // lblRPhaseKWDirVAlue
            // 
            this.lblRPhaseKWDirVAlue.AutoSize = true;
            this.lblRPhaseKWDirVAlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPhaseKWDirVAlue.Location = new System.Drawing.Point(500, 26);
            this.lblRPhaseKWDirVAlue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRPhaseKWDirVAlue.Name = "lblRPhaseKWDirVAlue";
            this.lblRPhaseKWDirVAlue.Size = new System.Drawing.Size(12, 17);
            this.lblRPhaseKWDirVAlue.TabIndex = 42;
            this.lblRPhaseKWDirVAlue.Text = " ";
            // 
            // lblYPhaseKWDirValue
            // 
            this.lblYPhaseKWDirValue.AutoSize = true;
            this.lblYPhaseKWDirValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPhaseKWDirValue.Location = new System.Drawing.Point(500, 58);
            this.lblYPhaseKWDirValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYPhaseKWDirValue.Name = "lblYPhaseKWDirValue";
            this.lblYPhaseKWDirValue.Size = new System.Drawing.Size(12, 17);
            this.lblYPhaseKWDirValue.TabIndex = 41;
            this.lblYPhaseKWDirValue.Text = " ";
            // 
            // lblFrequencyValue
            // 
            this.lblFrequencyValue.AutoSize = true;
            this.lblFrequencyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrequencyValue.Location = new System.Drawing.Point(200, 411);
            this.lblFrequencyValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrequencyValue.Name = "lblFrequencyValue";
            this.lblFrequencyValue.Size = new System.Drawing.Size(12, 17);
            this.lblFrequencyValue.TabIndex = 40;
            this.lblFrequencyValue.Text = " ";
            // 
            // lblYPhasePFValue
            // 
            this.lblYPhasePFValue.AutoSize = true;
            this.lblYPhasePFValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPhasePFValue.Location = new System.Drawing.Point(201, 347);
            this.lblYPhasePFValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYPhasePFValue.Name = "lblYPhasePFValue";
            this.lblYPhasePFValue.Size = new System.Drawing.Size(12, 17);
            this.lblYPhasePFValue.TabIndex = 39;
            this.lblYPhasePFValue.Text = " ";
            // 
            // lblBPhaesPFValue
            // 
            this.lblBPhaesPFValue.AutoSize = true;
            this.lblBPhaesPFValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBPhaesPFValue.Location = new System.Drawing.Point(200, 379);
            this.lblBPhaesPFValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBPhaesPFValue.Name = "lblBPhaesPFValue";
            this.lblBPhaesPFValue.Size = new System.Drawing.Size(12, 17);
            this.lblBPhaesPFValue.TabIndex = 38;
            this.lblBPhaesPFValue.Text = " ";
            // 
            // lblRPhasePFValue
            // 
            this.lblRPhasePFValue.AutoSize = true;
            this.lblRPhasePFValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPhasePFValue.Location = new System.Drawing.Point(201, 315);
            this.lblRPhasePFValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRPhasePFValue.Name = "lblRPhasePFValue";
            this.lblRPhasePFValue.Size = new System.Drawing.Size(12, 17);
            this.lblRPhasePFValue.TabIndex = 37;
            this.lblRPhasePFValue.Text = " ";
            // 
            // lblPhaseSeqValue
            // 
            this.lblPhaseSeqValue.AutoSize = true;
            this.lblPhaseSeqValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhaseSeqValue.Location = new System.Drawing.Point(201, 443);
            this.lblPhaseSeqValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhaseSeqValue.Name = "lblPhaseSeqValue";
            this.lblPhaseSeqValue.Size = new System.Drawing.Size(12, 17);
            this.lblPhaseSeqValue.TabIndex = 36;
            this.lblPhaseSeqValue.Text = " ";
            // 
            // lblReactivePowerValue
            // 
            this.lblReactivePowerValue.AutoSize = true;
            this.lblReactivePowerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactivePowerValue.Location = new System.Drawing.Point(201, 251);
            this.lblReactivePowerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReactivePowerValue.Name = "lblReactivePowerValue";
            this.lblReactivePowerValue.Size = new System.Drawing.Size(12, 17);
            this.lblReactivePowerValue.TabIndex = 35;
            this.lblReactivePowerValue.Text = " ";
            // 
            // lblApparentPowerValue
            // 
            this.lblApparentPowerValue.AutoSize = true;
            this.lblApparentPowerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApparentPowerValue.Location = new System.Drawing.Point(201, 283);
            this.lblApparentPowerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApparentPowerValue.Name = "lblApparentPowerValue";
            this.lblApparentPowerValue.Size = new System.Drawing.Size(12, 17);
            this.lblApparentPowerValue.TabIndex = 34;
            this.lblApparentPowerValue.Text = " ";
            // 
            // lblBCurrentValue
            // 
            this.lblBCurrentValue.AutoSize = true;
            this.lblBCurrentValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCurrentValue.Location = new System.Drawing.Point(201, 187);
            this.lblBCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBCurrentValue.Name = "lblBCurrentValue";
            this.lblBCurrentValue.Size = new System.Drawing.Size(12, 17);
            this.lblBCurrentValue.TabIndex = 33;
            this.lblBCurrentValue.Text = " ";
            // 
            // lblActivePowerValue
            // 
            this.lblActivePowerValue.AutoSize = true;
            this.lblActivePowerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivePowerValue.Location = new System.Drawing.Point(200, 219);
            this.lblActivePowerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivePowerValue.Name = "lblActivePowerValue";
            this.lblActivePowerValue.Size = new System.Drawing.Size(12, 17);
            this.lblActivePowerValue.TabIndex = 32;
            this.lblActivePowerValue.Text = " ";
            // 
            // lblRCurrentValue
            // 
            this.lblRCurrentValue.AutoSize = true;
            this.lblRCurrentValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRCurrentValue.Location = new System.Drawing.Point(201, 123);
            this.lblRCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRCurrentValue.Name = "lblRCurrentValue";
            this.lblRCurrentValue.Size = new System.Drawing.Size(12, 17);
            this.lblRCurrentValue.TabIndex = 31;
            this.lblRCurrentValue.Text = " ";
            // 
            // lblYCurrentValue
            // 
            this.lblYCurrentValue.AutoSize = true;
            this.lblYCurrentValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYCurrentValue.Location = new System.Drawing.Point(200, 155);
            this.lblYCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYCurrentValue.Name = "lblYCurrentValue";
            this.lblYCurrentValue.Size = new System.Drawing.Size(12, 17);
            this.lblYCurrentValue.TabIndex = 30;
            this.lblYCurrentValue.Text = " ";
            // 
            // lblYVoltageValue
            // 
            this.lblYVoltageValue.AutoSize = true;
            this.lblYVoltageValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYVoltageValue.Location = new System.Drawing.Point(201, 59);
            this.lblYVoltageValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYVoltageValue.Name = "lblYVoltageValue";
            this.lblYVoltageValue.Size = new System.Drawing.Size(12, 17);
            this.lblYVoltageValue.TabIndex = 29;
            this.lblYVoltageValue.Text = " ";
            // 
            // lblBVoltageValue
            // 
            this.lblBVoltageValue.AutoSize = true;
            this.lblBVoltageValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBVoltageValue.Location = new System.Drawing.Point(201, 91);
            this.lblBVoltageValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBVoltageValue.Name = "lblBVoltageValue";
            this.lblBVoltageValue.Size = new System.Drawing.Size(12, 17);
            this.lblBVoltageValue.TabIndex = 28;
            this.lblBVoltageValue.Text = " ";
            // 
            // lblRVoltageValue
            // 
            this.lblRVoltageValue.AutoSize = true;
            this.lblRVoltageValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRVoltageValue.Location = new System.Drawing.Point(201, 26);
            this.lblRVoltageValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRVoltageValue.Name = "lblRVoltageValue";
            this.lblRVoltageValue.Size = new System.Drawing.Size(12, 17);
            this.lblRVoltageValue.TabIndex = 27;
            this.lblRVoltageValue.Text = " ";
            // 
            // lblAngelYR
            // 
            this.lblAngelYR.AutoSize = true;
            this.lblAngelYR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngelYR.Location = new System.Drawing.Point(307, 315);
            this.lblAngelYR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngelYR.Name = "lblAngelYR";
            this.lblAngelYR.Size = new System.Drawing.Size(86, 17);
            this.lblAngelYR.TabIndex = 26;
            this.lblAngelYR.Text = "Angle Y-R:";
            // 
            // lblAngleBR
            // 
            this.lblAngleBR.AutoSize = true;
            this.lblAngleBR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleBR.Location = new System.Drawing.Point(307, 347);
            this.lblAngleBR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngleBR.Name = "lblAngleBR";
            this.lblAngleBR.Size = new System.Drawing.Size(86, 17);
            this.lblAngleBR.TabIndex = 25;
            this.lblAngleBR.Text = "Angle B-R:";
            // 
            // lblAngleBwTwo
            // 
            this.lblAngleBwTwo.AutoSize = true;
            this.lblAngleBwTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleBwTwo.Location = new System.Drawing.Point(307, 379);
            this.lblAngleBwTwo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngleBwTwo.Name = "lblAngleBwTwo";
            this.lblAngleBwTwo.Size = new System.Drawing.Size(154, 17);
            this.lblAngleBwTwo.TabIndex = 24;
            this.lblAngleBwTwo.Text = "Angle Between Two:";
            this.lblAngleBwTwo.Click += new System.EventHandler(this.lblAngleBwTwo_Click);
            // 
            // lblTotalPWFactor
            // 
            this.lblTotalPWFactor.AutoSize = true;
            this.lblTotalPWFactor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPWFactor.Location = new System.Drawing.Point(307, 407);
            this.lblTotalPWFactor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalPWFactor.Name = "lblTotalPWFactor";
            this.lblTotalPWFactor.Size = new System.Drawing.Size(200, 17);
            this.lblTotalPWFactor.TabIndex = 23;
            this.lblTotalPWFactor.Text = "Total Phase Power Factor:";
            // 
            // lblRLagLead
            // 
            this.lblRLagLead.AutoSize = true;
            this.lblRLagLead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRLagLead.Location = new System.Drawing.Point(307, 219);
            this.lblRLagLead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRLagLead.Name = "lblRLagLead";
            this.lblRLagLead.Size = new System.Drawing.Size(147, 17);
            this.lblRLagLead.TabIndex = 22;
            this.lblRLagLead.Text = "R Phase Lag/Lead:";
            // 
            // lblYLagLead
            // 
            this.lblYLagLead.AutoSize = true;
            this.lblYLagLead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYLagLead.Location = new System.Drawing.Point(307, 251);
            this.lblYLagLead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYLagLead.Name = "lblYLagLead";
            this.lblYLagLead.Size = new System.Drawing.Size(146, 17);
            this.lblYLagLead.TabIndex = 21;
            this.lblYLagLead.Text = "Y Phase Lag/Lead:";
            // 
            // lblYChannel
            // 
            this.lblYChannel.AutoSize = true;
            this.lblYChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYChannel.Location = new System.Drawing.Point(307, 155);
            this.lblYChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYChannel.Name = "lblYChannel";
            this.lblYChannel.Size = new System.Drawing.Size(137, 17);
            this.lblYChannel.TabIndex = 20;
            this.lblYChannel.Text = "Y Phase Channel:";
            // 
            // lblBChannel
            // 
            this.lblBChannel.AutoSize = true;
            this.lblBChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBChannel.Location = new System.Drawing.Point(307, 187);
            this.lblBChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBChannel.Name = "lblBChannel";
            this.lblBChannel.Size = new System.Drawing.Size(137, 17);
            this.lblBChannel.TabIndex = 19;
            this.lblBChannel.Text = "B Phase Channel:";
            // 
            // lblBLagLead
            // 
            this.lblBLagLead.AutoSize = true;
            this.lblBLagLead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBLagLead.Location = new System.Drawing.Point(307, 283);
            this.lblBLagLead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBLagLead.Name = "lblBLagLead";
            this.lblBLagLead.Size = new System.Drawing.Size(146, 17);
            this.lblBLagLead.TabIndex = 18;
            this.lblBLagLead.Text = "B Phase Lag/Lead:";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrequency.Location = new System.Drawing.Point(16, 411);
            this.lblFrequency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(89, 17);
            this.lblFrequency.TabIndex = 17;
            this.lblFrequency.Text = "Frequency:";
            // 
            // lblBPhaseKWDir
            // 
            this.lblBPhaseKWDir.AutoSize = true;
            this.lblBPhaseKWDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBPhaseKWDir.Location = new System.Drawing.Point(307, 91);
            this.lblBPhaseKWDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBPhaseKWDir.Name = "lblBPhaseKWDir";
            this.lblBPhaseKWDir.Size = new System.Drawing.Size(170, 17);
            this.lblBPhaseKWDir.TabIndex = 16;
            this.lblBPhaseKWDir.Text = "B Phase kW Direction:";
            // 
            // lblRChannel
            // 
            this.lblRChannel.AutoSize = true;
            this.lblRChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRChannel.Location = new System.Drawing.Point(307, 123);
            this.lblRChannel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRChannel.Name = "lblRChannel";
            this.lblRChannel.Size = new System.Drawing.Size(138, 17);
            this.lblRChannel.TabIndex = 15;
            this.lblRChannel.Text = "R Phase Channel:";
            // 
            // lblYPhasePF
            // 
            this.lblYPhasePF.AutoSize = true;
            this.lblYPhasePF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPhasePF.Location = new System.Drawing.Point(16, 347);
            this.lblYPhasePF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYPhasePF.Name = "lblYPhasePF";
            this.lblYPhasePF.Size = new System.Drawing.Size(173, 17);
            this.lblYPhasePF.TabIndex = 14;
            this.lblYPhasePF.Text = "Y Phase Power Factor:";
            // 
            // lblBPhasePF
            // 
            this.lblBPhasePF.AutoSize = true;
            this.lblBPhasePF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBPhasePF.Location = new System.Drawing.Point(16, 379);
            this.lblBPhasePF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBPhasePF.Name = "lblBPhasePF";
            this.lblBPhasePF.Size = new System.Drawing.Size(173, 17);
            this.lblBPhasePF.TabIndex = 13;
            this.lblBPhasePF.Text = "B Phase Power Factor:";
            // 
            // lblRPhaseKWDir
            // 
            this.lblRPhaseKWDir.AutoSize = true;
            this.lblRPhaseKWDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPhaseKWDir.Location = new System.Drawing.Point(307, 27);
            this.lblRPhaseKWDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRPhaseKWDir.Name = "lblRPhaseKWDir";
            this.lblRPhaseKWDir.Size = new System.Drawing.Size(171, 17);
            this.lblRPhaseKWDir.TabIndex = 12;
            this.lblRPhaseKWDir.Text = "R Phase kW Direction:";
            // 
            // lblYPhaseKWDir
            // 
            this.lblYPhaseKWDir.AutoSize = true;
            this.lblYPhaseKWDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPhaseKWDir.Location = new System.Drawing.Point(307, 59);
            this.lblYPhaseKWDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYPhaseKWDir.Name = "lblYPhaseKWDir";
            this.lblYPhaseKWDir.Size = new System.Drawing.Size(170, 17);
            this.lblYPhaseKWDir.TabIndex = 11;
            this.lblYPhaseKWDir.Text = "Y Phase kW Direction:";
            // 
            // lblRPhasePF
            // 
            this.lblRPhasePF.AutoSize = true;
            this.lblRPhasePF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPhasePF.Location = new System.Drawing.Point(16, 315);
            this.lblRPhasePF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRPhasePF.Name = "lblRPhasePF";
            this.lblRPhasePF.Size = new System.Drawing.Size(174, 17);
            this.lblRPhasePF.TabIndex = 10;
            this.lblRPhasePF.Text = "R Phase Power Factor:";
            // 
            // lblPhaseSeq
            // 
            this.lblPhaseSeq.AutoSize = true;
            this.lblPhaseSeq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhaseSeq.Location = new System.Drawing.Point(16, 443);
            this.lblPhaseSeq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhaseSeq.Name = "lblPhaseSeq";
            this.lblPhaseSeq.Size = new System.Drawing.Size(135, 17);
            this.lblPhaseSeq.TabIndex = 9;
            this.lblPhaseSeq.Text = "Phase Sequence:";
            // 
            // lblReactivePower
            // 
            this.lblReactivePower.AutoSize = true;
            this.lblReactivePower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactivePower.Location = new System.Drawing.Point(16, 251);
            this.lblReactivePower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReactivePower.Name = "lblReactivePower";
            this.lblReactivePower.Size = new System.Drawing.Size(168, 17);
            this.lblReactivePower.TabIndex = 8;
            this.lblReactivePower.Text = "Reactive Power(kvar):";
            this.lblReactivePower.Click += new System.EventHandler(this.label28_Click);
            // 
            // lblApparentPower
            // 
            this.lblApparentPower.AutoSize = true;
            this.lblApparentPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApparentPower.Location = new System.Drawing.Point(16, 283);
            this.lblApparentPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApparentPower.Name = "lblApparentPower";
            this.lblApparentPower.Size = new System.Drawing.Size(168, 17);
            this.lblApparentPower.TabIndex = 7;
            this.lblApparentPower.Text = "Apparent Power(kVA):";
            // 
            // lblBCurrent
            // 
            this.lblBCurrent.AutoSize = true;
            this.lblBCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBCurrent.Location = new System.Drawing.Point(16, 187);
            this.lblBCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBCurrent.Name = "lblBCurrent";
            this.lblBCurrent.Size = new System.Drawing.Size(154, 17);
            this.lblBCurrent.TabIndex = 6;
            this.lblBCurrent.Text = "B Phase Current(A):";
            // 
            // lblActivePower
            // 
            this.lblActivePower.AutoSize = true;
            this.lblActivePower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivePower.Location = new System.Drawing.Point(16, 219);
            this.lblActivePower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActivePower.Name = "lblActivePower";
            this.lblActivePower.Size = new System.Drawing.Size(140, 17);
            this.lblActivePower.TabIndex = 5;
            this.lblActivePower.Text = "Active Power(kW):";
            // 
            // lblRCurrent
            // 
            this.lblRCurrent.AutoSize = true;
            this.lblRCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRCurrent.Location = new System.Drawing.Point(16, 123);
            this.lblRCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRCurrent.Name = "lblRCurrent";
            this.lblRCurrent.Size = new System.Drawing.Size(155, 17);
            this.lblRCurrent.TabIndex = 4;
            this.lblRCurrent.Text = "R Phase Current(A):";
            // 
            // lblYCurrent
            // 
            this.lblYCurrent.AutoSize = true;
            this.lblYCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYCurrent.Location = new System.Drawing.Point(16, 155);
            this.lblYCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYCurrent.Name = "lblYCurrent";
            this.lblYCurrent.Size = new System.Drawing.Size(154, 17);
            this.lblYCurrent.TabIndex = 3;
            this.lblYCurrent.Text = "Y Phase Current(A):";
            // 
            // lblYVoltage
            // 
            this.lblYVoltage.AutoSize = true;
            this.lblYVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYVoltage.Location = new System.Drawing.Point(16, 59);
            this.lblYVoltage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYVoltage.Name = "lblYVoltage";
            this.lblYVoltage.Size = new System.Drawing.Size(155, 17);
            this.lblYVoltage.TabIndex = 2;
            this.lblYVoltage.Text = "Y Phase Voltage(V):";
            // 
            // lblBVoltage
            // 
            this.lblBVoltage.AutoSize = true;
            this.lblBVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBVoltage.Location = new System.Drawing.Point(16, 91);
            this.lblBVoltage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBVoltage.Name = "lblBVoltage";
            this.lblBVoltage.Size = new System.Drawing.Size(155, 17);
            this.lblBVoltage.TabIndex = 1;
            this.lblBVoltage.Text = "B Phase Voltage(V):";
            // 
            // lblRPhaseVoltage
            // 
            this.lblRPhaseVoltage.AutoSize = true;
            this.lblRPhaseVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRPhaseVoltage.Location = new System.Drawing.Point(16, 27);
            this.lblRPhaseVoltage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRPhaseVoltage.Name = "lblRPhaseVoltage";
            this.lblRPhaseVoltage.Size = new System.Drawing.Size(161, 17);
            this.lblRPhaseVoltage.TabIndex = 0;
            this.lblRPhaseVoltage.Text = "R Phase Voltage(V) :";
            // 
            // lblPhasorData
            // 
            this.lblPhasorData.AutoSize = true;
            this.lblPhasorData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhasorData.ForeColor = System.Drawing.Color.Red;
            this.lblPhasorData.Location = new System.Drawing.Point(48, 26);
            this.lblPhasorData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhasorData.Name = "lblPhasorData";
            this.lblPhasorData.Size = new System.Drawing.Size(0, 18);
            this.lblPhasorData.TabIndex = 5;
            this.lblPhasorData.Visible = false;
            // 
            // btnCancelPhasor
            // 
            this.btnCancelPhasor.Location = new System.Drawing.Point(1011, 474);
            this.btnCancelPhasor.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelPhasor.Name = "btnCancelPhasor";
            this.btnCancelPhasor.Size = new System.Drawing.Size(100, 28);
            this.btnCancelPhasor.TabIndex = 4;
            this.btnCancelPhasor.Text = "Cancel";
            this.btnCancelPhasor.UseVisualStyleBackColor = true;
            this.btnCancelPhasor.Click += new System.EventHandler(this.btnCancelPhasor_Click);
            // 
            // btnHold
            // 
            this.btnHold.Enabled = false;
            this.btnHold.Location = new System.Drawing.Point(879, 474);
            this.btnHold.Margin = new System.Windows.Forms.Padding(4);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(100, 28);
            this.btnHold.TabIndex = 3;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // btnReadPhasor
            // 
            this.btnReadPhasor.Location = new System.Drawing.Point(751, 474);
            this.btnReadPhasor.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadPhasor.Name = "btnReadPhasor";
            this.btnReadPhasor.Size = new System.Drawing.Size(100, 28);
            this.btnReadPhasor.TabIndex = 2;
            this.btnReadPhasor.Text = "Read Data";
            this.btnReadPhasor.UseVisualStyleBackColor = true;
            this.btnReadPhasor.Click += new System.EventHandler(this.btnReadPhasor_Click);
            // 
            // phasorDiagram1
            // 
            this.phasorDiagram1.BackColor = System.Drawing.Color.Transparent;
            this.phasorDiagram1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.phasorDiagram1.Location = new System.Drawing.Point(4, 4);
            this.phasorDiagram1.Margin = new System.Windows.Forms.Padding(5);
            this.phasorDiagram1.Name = "phasorDiagram1";
            this.phasorDiagram1.PhasorData = null;
            this.phasorDiagram1.PhasorDataset = null;
            this.phasorDiagram1.Size = new System.Drawing.Size(641, 492);
            this.phasorDiagram1.TabIndex = 0;
            // 
            // tabPageCompartment4
            // 
            this.tabPageCompartment4.AutoScroll = true;
            this.tabPageCompartment4.Controls.Add(this.btnAutoConfigModem);
            this.tabPageCompartment4.Controls.Add(this.button2);
            this.tabPageCompartment4.Controls.Add(this.groupBox35);
            this.tabPageCompartment4.Controls.Add(this.btnOK);
            this.tabPageCompartment4.Location = new System.Drawing.Point(4, 25);
            this.tabPageCompartment4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment4.Name = "tabPageCompartment4";
            this.tabPageCompartment4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPageCompartment4.Size = new System.Drawing.Size(1223, 833);
            this.tabPageCompartment4.TabIndex = 3;
            this.tabPageCompartment4.Text = "Settings";
            this.tabPageCompartment4.UseVisualStyleBackColor = true;
            // 
            // btnAutoConfigModem
            // 
            this.btnAutoConfigModem.Location = new System.Drawing.Point(704, 532);
            this.btnAutoConfigModem.Margin = new System.Windows.Forms.Padding(4);
            this.btnAutoConfigModem.Name = "btnAutoConfigModem";
            this.btnAutoConfigModem.Size = new System.Drawing.Size(195, 32);
            this.btnAutoConfigModem.TabIndex = 41;
            this.btnAutoConfigModem.Text = "Auto Configure Modem";
            this.btnAutoConfigModem.UseVisualStyleBackColor = true;
            this.btnAutoConfigModem.Click += new System.EventHandler(this.btnAutoConfigModem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1028, 532);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 32);
            this.button2.TabIndex = 39;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox35
            // 
            this.groupBox35.Controls.Add(this.gbPort);
            this.groupBox35.Controls.Add(this.groupBox8);
            this.groupBox35.Controls.Add(this.groupBox67);
            this.groupBox35.Controls.Add(this.groupBox66);
            this.groupBox35.Controls.Add(this.groupBox68);
            this.groupBox35.Controls.Add(this.button6);
            this.groupBox35.Controls.Add(this.txtBoxScaleXML);
            this.groupBox35.Controls.Add(this.label99);
            this.groupBox35.Controls.Add(this.groupBox36);
            this.groupBox35.Controls.Add(this.groupBox63);
            this.groupBox35.Location = new System.Drawing.Point(125, 23);
            this.groupBox35.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox35.Name = "groupBox35";
            this.groupBox35.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox35.Size = new System.Drawing.Size(1001, 489);
            this.groupBox35.TabIndex = 22;
            this.groupBox35.TabStop = false;
            this.groupBox35.Text = "DLMS Settings";
            // 
            // gbPort
            // 
            this.gbPort.Controls.Add(this.panelCommunicationType);
            this.gbPort.Controls.Add(this.btnTestConnection);
            this.gbPort.Controls.Add(this.dgvPortUsageAssociation);
            this.gbPort.Controls.Add(this.rdbMultiplePorts);
            this.gbPort.Controls.Add(this.rdbSinglePort);
            this.gbPort.Controls.Add(this.groupBox65);
            this.gbPort.Location = new System.Drawing.Point(517, 121);
            this.gbPort.Margin = new System.Windows.Forms.Padding(4);
            this.gbPort.Name = "gbPort";
            this.gbPort.Padding = new System.Windows.Forms.Padding(4);
            this.gbPort.Size = new System.Drawing.Size(444, 358);
            this.gbPort.TabIndex = 40;
            this.gbPort.TabStop = false;
            this.gbPort.Text = "Port";
            // 
            // panelCommunicationType
            // 
            this.panelCommunicationType.Controls.Add(this.rdGPRS);
            this.panelCommunicationType.Controls.Add(this.rdPSTN);
            this.panelCommunicationType.Controls.Add(this.rdGSM);
            this.panelCommunicationType.Controls.Add(this.rdDirect);
            this.panelCommunicationType.Location = new System.Drawing.Point(29, 295);
            this.panelCommunicationType.Margin = new System.Windows.Forms.Padding(4);
            this.panelCommunicationType.Name = "panelCommunicationType";
            this.panelCommunicationType.Size = new System.Drawing.Size(407, 55);
            this.panelCommunicationType.TabIndex = 42;
            // 
            // rdGPRS
            // 
            this.rdGPRS.AutoSize = true;
            this.rdGPRS.Location = new System.Drawing.Point(301, 18);
            this.rdGPRS.Margin = new System.Windows.Forms.Padding(4);
            this.rdGPRS.Name = "rdGPRS";
            this.rdGPRS.Size = new System.Drawing.Size(68, 21);
            this.rdGPRS.TabIndex = 3;
            this.rdGPRS.Text = "GPRS";
            this.rdGPRS.UseVisualStyleBackColor = true;
            // 
            // rdPSTN
            // 
            this.rdPSTN.AutoSize = true;
            this.rdPSTN.Location = new System.Drawing.Point(192, 20);
            this.rdPSTN.Margin = new System.Windows.Forms.Padding(4);
            this.rdPSTN.Name = "rdPSTN";
            this.rdPSTN.Size = new System.Drawing.Size(66, 21);
            this.rdPSTN.TabIndex = 2;
            this.rdPSTN.Text = "PSTN";
            this.rdPSTN.UseVisualStyleBackColor = true;
            // 
            // rdGSM
            // 
            this.rdGSM.AutoSize = true;
            this.rdGSM.Location = new System.Drawing.Point(96, 18);
            this.rdGSM.Margin = new System.Windows.Forms.Padding(4);
            this.rdGSM.Name = "rdGSM";
            this.rdGSM.Size = new System.Drawing.Size(60, 21);
            this.rdGSM.TabIndex = 1;
            this.rdGSM.Text = "GSM";
            this.rdGSM.UseVisualStyleBackColor = true;
            // 
            // rdDirect
            // 
            this.rdDirect.AutoSize = true;
            this.rdDirect.Checked = true;
            this.rdDirect.Location = new System.Drawing.Point(8, 18);
            this.rdDirect.Margin = new System.Windows.Forms.Padding(4);
            this.rdDirect.Name = "rdDirect";
            this.rdDirect.Size = new System.Drawing.Size(66, 21);
            this.rdDirect.TabIndex = 0;
            this.rdDirect.TabStop = true;
            this.rdDirect.Text = "Direct";
            this.rdDirect.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(145, 261);
            this.btnTestConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(148, 28);
            this.btnTestConnection.TabIndex = 41;
            this.btnTestConnection.Text = "Test Connections";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Visible = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // dgvPortUsageAssociation
            // 
            this.dgvPortUsageAssociation.AllowUserToAddRows = false;
            this.dgvPortUsageAssociation.AllowUserToDeleteRows = false;
            this.dgvPortUsageAssociation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortUsageAssociation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPortName,
            this.colPortUsageTypeModem,
            this.colPortUsageTypeCMRI});
            this.dgvPortUsageAssociation.Location = new System.Drawing.Point(37, 54);
            this.dgvPortUsageAssociation.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPortUsageAssociation.Name = "dgvPortUsageAssociation";
            this.dgvPortUsageAssociation.RowHeadersWidth = 30;
            this.dgvPortUsageAssociation.Size = new System.Drawing.Size(371, 198);
            this.dgvPortUsageAssociation.TabIndex = 40;
            this.dgvPortUsageAssociation.Visible = false;
            this.dgvPortUsageAssociation.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvPortUsageAssociation_CellBeginEdit);
            this.dgvPortUsageAssociation.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortUsageAssociation_CellEndEdit);
            this.dgvPortUsageAssociation.MouseLeave += new System.EventHandler(this.dgvPortUsageAssociation_MouseLeave);
            // 
            // colPortName
            // 
            this.colPortName.HeaderText = "Port Name";
            this.colPortName.Name = "colPortName";
            this.colPortName.ReadOnly = true;
            this.colPortName.Width = 70;
            // 
            // colPortUsageTypeModem
            // 
            this.colPortUsageTypeModem.HeaderText = "Scheduling";
            this.colPortUsageTypeModem.Name = "colPortUsageTypeModem";
            this.colPortUsageTypeModem.Width = 70;
            // 
            // colPortUsageTypeCMRI
            // 
            this.colPortUsageTypeCMRI.HeaderText = "Direct";
            this.colPortUsageTypeCMRI.Name = "colPortUsageTypeCMRI";
            this.colPortUsageTypeCMRI.Width = 40;
            // 
            // rdbMultiplePorts
            // 
            this.rdbMultiplePorts.AutoSize = true;
            this.rdbMultiplePorts.Location = new System.Drawing.Point(255, 27);
            this.rdbMultiplePorts.Margin = new System.Windows.Forms.Padding(4);
            this.rdbMultiplePorts.Name = "rdbMultiplePorts";
            this.rdbMultiplePorts.Size = new System.Drawing.Size(114, 21);
            this.rdbMultiplePorts.TabIndex = 1;
            this.rdbMultiplePorts.Text = "Multiple Ports";
            this.rdbMultiplePorts.UseVisualStyleBackColor = true;
            // 
            // rdbSinglePort
            // 
            this.rdbSinglePort.AutoSize = true;
            this.rdbSinglePort.Checked = true;
            this.rdbSinglePort.Location = new System.Drawing.Point(37, 27);
            this.rdbSinglePort.Margin = new System.Windows.Forms.Padding(4);
            this.rdbSinglePort.Name = "rdbSinglePort";
            this.rdbSinglePort.Size = new System.Drawing.Size(98, 21);
            this.rdbSinglePort.TabIndex = 0;
            this.rdbSinglePort.TabStop = true;
            this.rdbSinglePort.Text = "Single Port";
            this.rdbSinglePort.UseVisualStyleBackColor = true;
            this.rdbSinglePort.CheckedChanged += new System.EventHandler(this.rdbSinglePort_CheckedChanged);
            // 
            // groupBox65
            // 
            this.groupBox65.Controls.Add(this.groupBox69);
            this.groupBox65.Controls.Add(this.groupBox70);
            this.groupBox65.Controls.Add(this.cmbAvailableSerialPort);
            this.groupBox65.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox65.Location = new System.Drawing.Point(72, 112);
            this.groupBox65.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox65.Name = "groupBox65";
            this.groupBox65.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox65.Size = new System.Drawing.Size(304, 114);
            this.groupBox65.TabIndex = 25;
            this.groupBox65.TabStop = false;
            this.groupBox65.Text = "COM Port Settings";
            // 
            // groupBox69
            // 
            this.groupBox69.Controls.Add(this.rdBtnRJPort);
            this.groupBox69.Controls.Add(this.rdBtnOpticalPort);
            this.groupBox69.Location = new System.Drawing.Point(103, 357);
            this.groupBox69.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox69.Name = "groupBox69";
            this.groupBox69.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox69.Size = new System.Drawing.Size(345, 76);
            this.groupBox69.TabIndex = 12;
            this.groupBox69.TabStop = false;
            this.groupBox69.Text = "Communication Port";
            this.groupBox69.Visible = false;
            // 
            // rdBtnRJPort
            // 
            this.rdBtnRJPort.AutoSize = true;
            this.rdBtnRJPort.Checked = true;
            this.rdBtnRJPort.Location = new System.Drawing.Point(37, 36);
            this.rdBtnRJPort.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnRJPort.Name = "rdBtnRJPort";
            this.rdBtnRJPort.Size = new System.Drawing.Size(77, 21);
            this.rdBtnRJPort.TabIndex = 6;
            this.rdBtnRJPort.TabStop = true;
            this.rdBtnRJPort.Text = "RS-232";
            this.rdBtnRJPort.UseVisualStyleBackColor = true;
            // 
            // rdBtnOpticalPort
            // 
            this.rdBtnOpticalPort.AutoSize = true;
            this.rdBtnOpticalPort.Location = new System.Drawing.Point(172, 36);
            this.rdBtnOpticalPort.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnOpticalPort.Name = "rdBtnOpticalPort";
            this.rdBtnOpticalPort.Size = new System.Drawing.Size(103, 21);
            this.rdBtnOpticalPort.TabIndex = 7;
            this.rdBtnOpticalPort.Text = "Optical Port";
            this.rdBtnOpticalPort.UseVisualStyleBackColor = true;
            // 
            // groupBox70
            // 
            this.groupBox70.Controls.Add(this.rdBtnModeE);
            this.groupBox70.Controls.Add(this.rdBtnDirectHDLC);
            this.groupBox70.Location = new System.Drawing.Point(119, 246);
            this.groupBox70.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox70.Name = "groupBox70";
            this.groupBox70.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox70.Size = new System.Drawing.Size(345, 78);
            this.groupBox70.TabIndex = 11;
            this.groupBox70.TabStop = false;
            this.groupBox70.Text = "Communication Mode";
            this.groupBox70.Visible = false;
            // 
            // rdBtnModeE
            // 
            this.rdBtnModeE.AutoSize = true;
            this.rdBtnModeE.Location = new System.Drawing.Point(40, 36);
            this.rdBtnModeE.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnModeE.Name = "rdBtnModeE";
            this.rdBtnModeE.Size = new System.Drawing.Size(78, 21);
            this.rdBtnModeE.TabIndex = 10;
            this.rdBtnModeE.Text = "Mode-E";
            this.rdBtnModeE.UseVisualStyleBackColor = true;
            // 
            // rdBtnDirectHDLC
            // 
            this.rdBtnDirectHDLC.AutoSize = true;
            this.rdBtnDirectHDLC.Checked = true;
            this.rdBtnDirectHDLC.Location = new System.Drawing.Point(172, 36);
            this.rdBtnDirectHDLC.Margin = new System.Windows.Forms.Padding(4);
            this.rdBtnDirectHDLC.Name = "rdBtnDirectHDLC";
            this.rdBtnDirectHDLC.Size = new System.Drawing.Size(108, 21);
            this.rdBtnDirectHDLC.TabIndex = 9;
            this.rdBtnDirectHDLC.TabStop = true;
            this.rdBtnDirectHDLC.Text = "Direct-HDLC";
            this.rdBtnDirectHDLC.UseVisualStyleBackColor = true;
            // 
            // cmbAvailableSerialPort
            // 
            this.cmbAvailableSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAvailableSerialPort.FormattingEnabled = true;
            this.cmbAvailableSerialPort.Location = new System.Drawing.Point(95, 48);
            this.cmbAvailableSerialPort.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAvailableSerialPort.Name = "cmbAvailableSerialPort";
            this.cmbAvailableSerialPort.Size = new System.Drawing.Size(172, 24);
            this.cmbAvailableSerialPort.TabIndex = 1;
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(13, 52);
            this.COMPortSet_lblCOMPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(74, 17);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "Serial Port";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.groupBox10);
            this.groupBox8.Controls.Add(this.groupBox11);
            this.groupBox8.Controls.Add(this.cmbCMRIType);
            this.groupBox8.Controls.Add(this.label11);
            this.groupBox8.Location = new System.Drawing.Point(517, 23);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox8.Size = new System.Drawing.Size(441, 87);
            this.groupBox8.TabIndex = 26;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "CMRI Selection";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.radioButton5);
            this.groupBox10.Controls.Add(this.radioButton6);
            this.groupBox10.Location = new System.Drawing.Point(103, 357);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox10.Size = new System.Drawing.Size(345, 76);
            this.groupBox10.TabIndex = 12;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Communication Port";
            this.groupBox10.Visible = false;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Checked = true;
            this.radioButton5.Location = new System.Drawing.Point(37, 36);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(77, 21);
            this.radioButton5.TabIndex = 6;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "RS-232";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(172, 36);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(103, 21);
            this.radioButton6.TabIndex = 7;
            this.radioButton6.Text = "Optical Port";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.radioButton7);
            this.groupBox11.Controls.Add(this.radioButton8);
            this.groupBox11.Location = new System.Drawing.Point(119, 246);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox11.Size = new System.Drawing.Size(345, 78);
            this.groupBox11.TabIndex = 11;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Communication Mode";
            this.groupBox11.Visible = false;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(40, 36);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(78, 21);
            this.radioButton7.TabIndex = 10;
            this.radioButton7.Text = "Mode-E";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.Location = new System.Drawing.Point(172, 36);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(108, 21);
            this.radioButton8.TabIndex = 9;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Direct-HDLC";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // cmbCMRIType
            // 
            this.cmbCMRIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCMRIType.FormattingEnabled = true;
            this.cmbCMRIType.Location = new System.Drawing.Point(204, 30);
            this.cmbCMRIType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCMRIType.Name = "cmbCMRIType";
            this.cmbCMRIType.Size = new System.Drawing.Size(167, 24);
            this.cmbCMRIType.TabIndex = 1;
            this.cmbCMRIType.SelectedIndexChanged += new System.EventHandler(this.cmbCMRIType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(57, 33);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 17);
            this.label11.TabIndex = 0;
            this.label11.Text = "CMRI Type";
            // 
            // groupBox67
            // 
            this.groupBox67.Controls.Add(this.label109);
            this.groupBox67.Controls.Add(this.label110);
            this.groupBox67.Controls.Add(this.label204);
            this.groupBox67.Controls.Add(this.txtGSMInterFrameTime);
            this.groupBox67.Controls.Add(this.txtBoxInterFrameTime);
            this.groupBox67.Controls.Add(this.txtResponseTimeOut);
            this.groupBox67.Controls.Add(this.label205);
            this.groupBox67.Controls.Add(this.label206);
            this.groupBox67.Controls.Add(this.label207);
            this.groupBox67.Location = new System.Drawing.Point(35, 121);
            this.groupBox67.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox67.Name = "groupBox67";
            this.groupBox67.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox67.Size = new System.Drawing.Size(445, 122);
            this.groupBox67.TabIndex = 1;
            this.groupBox67.TabStop = false;
            this.groupBox67.Text = "HDLC Time Outs";
            // 
            // label109
            // 
            this.label109.AutoSize = true;
            this.label109.Location = new System.Drawing.Point(357, 203);
            this.label109.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(26, 17);
            this.label109.TabIndex = 6;
            this.label109.Text = "ms";
            this.label109.Visible = false;
            // 
            // label110
            // 
            this.label110.AutoSize = true;
            this.label110.Location = new System.Drawing.Point(363, 76);
            this.label110.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(26, 17);
            this.label110.TabIndex = 6;
            this.label110.Text = "ms";
            // 
            // label204
            // 
            this.label204.AutoSize = true;
            this.label204.Location = new System.Drawing.Point(363, 37);
            this.label204.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label204.Name = "label204";
            this.label204.Size = new System.Drawing.Size(26, 17);
            this.label204.TabIndex = 6;
            this.label204.Text = "ms";
            // 
            // txtGSMInterFrameTime
            // 
            this.txtGSMInterFrameTime.Location = new System.Drawing.Point(235, 199);
            this.txtGSMInterFrameTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtGSMInterFrameTime.MaxLength = 120;
            this.txtGSMInterFrameTime.Name = "txtGSMInterFrameTime";
            this.txtGSMInterFrameTime.Size = new System.Drawing.Size(116, 22);
            this.txtGSMInterFrameTime.TabIndex = 5;
            this.txtGSMInterFrameTime.Visible = false;
            // 
            // txtBoxInterFrameTime
            // 
            this.txtBoxInterFrameTime.Location = new System.Drawing.Point(235, 73);
            this.txtBoxInterFrameTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxInterFrameTime.MaxLength = 9999;
            this.txtBoxInterFrameTime.Name = "txtBoxInterFrameTime";
            this.txtBoxInterFrameTime.Size = new System.Drawing.Size(116, 22);
            this.txtBoxInterFrameTime.TabIndex = 4;
            this.txtBoxInterFrameTime.Text = "300";
            // 
            // txtResponseTimeOut
            // 
            this.txtResponseTimeOut.Location = new System.Drawing.Point(235, 33);
            this.txtResponseTimeOut.Margin = new System.Windows.Forms.Padding(4);
            this.txtResponseTimeOut.MaxLength = 9999;
            this.txtResponseTimeOut.Name = "txtResponseTimeOut";
            this.txtResponseTimeOut.Size = new System.Drawing.Size(116, 22);
            this.txtResponseTimeOut.TabIndex = 3;
            this.txtResponseTimeOut.Text = "2200";
            // 
            // label205
            // 
            this.label205.AutoSize = true;
            this.label205.Location = new System.Drawing.Point(61, 203);
            this.label205.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label205.Name = "label205";
            this.label205.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label205.Size = new System.Drawing.Size(162, 17);
            this.label205.TabIndex = 2;
            this.label205.Text = "GSM Interframe Timeout";
            this.label205.Visible = false;
            // 
            // label206
            // 
            this.label206.AutoSize = true;
            this.label206.Location = new System.Drawing.Point(27, 76);
            this.label206.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label206.Name = "label206";
            this.label206.Size = new System.Drawing.Size(135, 17);
            this.label206.TabIndex = 1;
            this.label206.Text = "Inter Frame Timeout";
            // 
            // label207
            // 
            this.label207.AutoSize = true;
            this.label207.Location = new System.Drawing.Point(27, 33);
            this.label207.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label207.Name = "label207";
            this.label207.Size = new System.Drawing.Size(127, 17);
            this.label207.TabIndex = 0;
            this.label207.Text = "Response Timeout";
            // 
            // groupBox66
            // 
            this.groupBox66.Controls.Add(this.cmbMode);
            this.groupBox66.Controls.Add(this.label108);
            this.groupBox66.Location = new System.Drawing.Point(35, 23);
            this.groupBox66.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox66.Name = "groupBox66";
            this.groupBox66.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox66.Size = new System.Drawing.Size(441, 87);
            this.groupBox66.TabIndex = 5;
            this.groupBox66.TabStop = false;
            this.groupBox66.Text = "Mode";
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            " PC ",
            " MR ",
            " US ",
            " FS "});
            this.cmbMode.Location = new System.Drawing.Point(213, 26);
            this.cmbMode.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(163, 24);
            this.cmbMode.TabIndex = 3;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // label108
            // 
            this.label108.AutoSize = true;
            this.label108.Location = new System.Drawing.Point(36, 30);
            this.label108.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(86, 17);
            this.label108.TabIndex = 0;
            this.label108.Text = "Select Mode";
            // 
            // groupBox68
            // 
            this.groupBox68.Controls.Add(this.txtServerLowerMacAddress);
            this.groupBox68.Controls.Add(this.txtServerSAP);
            this.groupBox68.Controls.Add(this.label208);
            this.groupBox68.Controls.Add(this.label209);
            this.groupBox68.Location = new System.Drawing.Point(9, 532);
            this.groupBox68.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox68.Name = "groupBox68";
            this.groupBox68.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox68.Size = new System.Drawing.Size(441, 122);
            this.groupBox68.TabIndex = 0;
            this.groupBox68.TabStop = false;
            this.groupBox68.Text = "Destination Address";
            this.groupBox68.Visible = false;
            // 
            // txtServerLowerMacAddress
            // 
            this.txtServerLowerMacAddress.Enabled = false;
            this.txtServerLowerMacAddress.Location = new System.Drawing.Point(204, 80);
            this.txtServerLowerMacAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerLowerMacAddress.MaxLength = 4;
            this.txtServerLowerMacAddress.Name = "txtServerLowerMacAddress";
            this.txtServerLowerMacAddress.Size = new System.Drawing.Size(163, 22);
            this.txtServerLowerMacAddress.TabIndex = 5;
            this.txtServerLowerMacAddress.Text = "17";
            // 
            // txtServerSAP
            // 
            this.txtServerSAP.Enabled = false;
            this.txtServerSAP.Location = new System.Drawing.Point(204, 39);
            this.txtServerSAP.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerSAP.MaxLength = 1;
            this.txtServerSAP.Name = "txtServerSAP";
            this.txtServerSAP.Size = new System.Drawing.Size(163, 22);
            this.txtServerSAP.TabIndex = 4;
            this.txtServerSAP.Text = "1";
            // 
            // label208
            // 
            this.label208.AutoSize = true;
            this.label208.Location = new System.Drawing.Point(21, 84);
            this.label208.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label208.Name = "label208";
            this.label208.Size = new System.Drawing.Size(148, 17);
            this.label208.TabIndex = 3;
            this.label208.Text = "Server Lower Address";
            // 
            // label209
            // 
            this.label209.AutoSize = true;
            this.label209.Location = new System.Drawing.Point(21, 43);
            this.label209.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label209.Name = "label209";
            this.label209.Size = new System.Drawing.Size(149, 17);
            this.label209.TabIndex = 2;
            this.label209.Text = "Server Upper Address";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(933, 486);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(44, 30);
            this.button6.TabIndex = 23;
            this.button6.Text = "...";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtBoxScaleXML
            // 
            this.txtBoxScaleXML.Location = new System.Drawing.Point(183, 486);
            this.txtBoxScaleXML.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxScaleXML.Name = "txtBoxScaleXML";
            this.txtBoxScaleXML.ReadOnly = true;
            this.txtBoxScaleXML.Size = new System.Drawing.Size(741, 22);
            this.txtBoxScaleXML.TabIndex = 22;
            this.txtBoxScaleXML.Text = " ";
            this.txtBoxScaleXML.Visible = false;
            // 
            // label99
            // 
            this.label99.AutoSize = true;
            this.label99.Location = new System.Drawing.Point(39, 495);
            this.label99.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(92, 17);
            this.label99.TabIndex = 21;
            this.label99.Text = "Settings Path";
            this.label99.Visible = false;
            // 
            // groupBox36
            // 
            this.groupBox36.Controls.Add(this.button7);
            this.groupBox36.Controls.Add(this.txtHLSPwd);
            this.groupBox36.Controls.Add(this.labelHLS);
            this.groupBox36.Controls.Add(this.txtPWD);
            this.groupBox36.Controls.Add(this.labelPwd);
            this.groupBox36.Location = new System.Drawing.Point(35, 250);
            this.groupBox36.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox36.Size = new System.Drawing.Size(441, 112);
            this.groupBox36.TabIndex = 4;
            this.groupBox36.TabStop = false;
            this.groupBox36.Text = "Password Authentication";
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(372, 25);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(48, 34);
            this.button7.TabIndex = 4;
            this.button7.Text = "&Key";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // txtHLSPwd
            // 
            this.txtHLSPwd.Location = new System.Drawing.Point(133, 74);
            this.txtHLSPwd.Margin = new System.Windows.Forms.Padding(4);
            this.txtHLSPwd.MaxLength = 32;
            this.txtHLSPwd.Name = "txtHLSPwd";
            this.txtHLSPwd.Size = new System.Drawing.Size(285, 22);
            this.txtHLSPwd.TabIndex = 3;
            this.txtHLSPwd.Text = "93BC0FABF6C85E9E1C53D78885373DC7";
            this.txtHLSPwd.Visible = false;
            // 
            // labelHLS
            // 
            this.labelHLS.AutoSize = true;
            this.labelHLS.Location = new System.Drawing.Point(32, 34);
            this.labelHLS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHLS.Name = "labelHLS";
            this.labelHLS.Size = new System.Drawing.Size(63, 17);
            this.labelHLS.TabIndex = 2;
            this.labelHLS.Text = "HLS Key";
            this.labelHLS.Visible = false;
            this.labelHLS.Click += new System.EventHandler(this.labelHLS_Click);
            // 
            // txtPWD
            // 
            this.txtPWD.Location = new System.Drawing.Point(133, 31);
            this.txtPWD.Margin = new System.Windows.Forms.Padding(4);
            this.txtPWD.MaxLength = 8;
            this.txtPWD.Name = "txtPWD";
            this.txtPWD.PasswordChar = '*';
            this.txtPWD.Size = new System.Drawing.Size(163, 22);
            this.txtPWD.TabIndex = 1;
            this.txtPWD.Text = "12345678";
            // 
            // labelPwd
            // 
            this.labelPwd.AutoSize = true;
            this.labelPwd.Location = new System.Drawing.Point(24, 34);
            this.labelPwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(73, 17);
            this.labelPwd.TabIndex = 0;
            this.labelPwd.Text = " Password";
            this.labelPwd.Visible = false;
            // 
            // groupBox63
            // 
            this.groupBox63.Controls.Add(this.cmbSecurity);
            this.groupBox63.Controls.Add(this.cmbContext);
            this.groupBox63.Controls.Add(this.label106);
            this.groupBox63.Controls.Add(this.label107);
            this.groupBox63.Location = new System.Drawing.Point(35, 369);
            this.groupBox63.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox63.Name = "groupBox63";
            this.groupBox63.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox63.Size = new System.Drawing.Size(441, 110);
            this.groupBox63.TabIndex = 0;
            this.groupBox63.TabStop = false;
            this.groupBox63.Text = "Referencing Type && Security Level";
            // 
            // cmbSecurity
            // 
            this.cmbSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecurity.Enabled = false;
            this.cmbSecurity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbSecurity.FormattingEnabled = true;
            this.cmbSecurity.Items.AddRange(new object[] {
            "No Security",
            "Low-Level",
            "High-Level"});
            this.cmbSecurity.Location = new System.Drawing.Point(204, 62);
            this.cmbSecurity.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSecurity.Name = "cmbSecurity";
            this.cmbSecurity.Size = new System.Drawing.Size(163, 24);
            this.cmbSecurity.TabIndex = 3;
            // 
            // cmbContext
            // 
            this.cmbContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContext.Enabled = false;
            this.cmbContext.FormattingEnabled = true;
            this.cmbContext.Items.AddRange(new object[] {
            "Long Name [LN]"});
            this.cmbContext.Location = new System.Drawing.Point(204, 23);
            this.cmbContext.Margin = new System.Windows.Forms.Padding(4);
            this.cmbContext.Name = "cmbContext";
            this.cmbContext.Size = new System.Drawing.Size(163, 24);
            this.cmbContext.TabIndex = 2;
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(25, 62);
            this.label106.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(97, 17);
            this.label106.TabIndex = 1;
            this.label106.Text = "Security Level";
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.Location = new System.Drawing.Point(25, 27);
            this.label107.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(121, 17);
            this.label107.TabIndex = 0;
            this.label107.Text = "Referencing Type";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(920, 532);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 32);
            this.btnOK.TabIndex = 23;
            this.btnOK.Text = " Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // timerRTC
            // 
            this.timerRTC.Interval = 1000;
            this.timerRTC.Tick += new System.EventHandler(this.timerRTC_Tick);
            // 
            // errpPortMapping
            // 
            this.errpPortMapping.ContainerControl = this;
            // 
            // chkOtherCMRI
            // 
            this.chkOtherCMRI.AutoSize = true;
            this.chkOtherCMRI.Checked = true;
            this.chkOtherCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOtherCMRI.Location = new System.Drawing.Point(35, 250);
            this.chkOtherCMRI.Name = "chkOtherCMRI";
            this.chkOtherCMRI.Size = new System.Drawing.Size(70, 17);
            this.chkOtherCMRI.TabIndex = 37;
            this.chkOtherCMRI.Text = "Select All";
            this.chkOtherCMRI.UseVisualStyleBackColor = true;
            // 
            // chkNameplateCMRI
            // 
            this.chkNameplateCMRI.AutoSize = true;
            this.chkNameplateCMRI.Checked = true;
            this.chkNameplateCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNameplateCMRI.Enabled = false;
            this.chkNameplateCMRI.Location = new System.Drawing.Point(35, 204);
            this.chkNameplateCMRI.Name = "chkNameplateCMRI";
            this.chkNameplateCMRI.Size = new System.Drawing.Size(63, 17);
            this.chkNameplateCMRI.TabIndex = 36;
            this.chkNameplateCMRI.Text = "General";
            this.chkNameplateCMRI.UseVisualStyleBackColor = true;
            // 
            // chkTamperCMRI
            // 
            this.chkTamperCMRI.AutoSize = true;
            this.chkTamperCMRI.Checked = true;
            this.chkTamperCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTamperCMRI.Location = new System.Drawing.Point(35, 159);
            this.chkTamperCMRI.Name = "chkTamperCMRI";
            this.chkTamperCMRI.Size = new System.Drawing.Size(71, 17);
            this.chkTamperCMRI.TabIndex = 35;
            this.chkTamperCMRI.Text = "Event log";
            this.chkTamperCMRI.UseVisualStyleBackColor = true;
            // 
            // chkLoadSurveyCMRI
            // 
            this.chkLoadSurveyCMRI.AutoSize = true;
            this.chkLoadSurveyCMRI.Checked = true;
            this.chkLoadSurveyCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoadSurveyCMRI.Location = new System.Drawing.Point(35, 115);
            this.chkLoadSurveyCMRI.Name = "chkLoadSurveyCMRI";
            this.chkLoadSurveyCMRI.Size = new System.Drawing.Size(86, 17);
            this.chkLoadSurveyCMRI.TabIndex = 34;
            this.chkLoadSurveyCMRI.Text = "Load Survey";
            this.chkLoadSurveyCMRI.UseVisualStyleBackColor = true;
            // 
            // chkBillingCMRI
            // 
            this.chkBillingCMRI.AutoSize = true;
            this.chkBillingCMRI.Checked = true;
            this.chkBillingCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBillingCMRI.Location = new System.Drawing.Point(35, 71);
            this.chkBillingCMRI.Name = "chkBillingCMRI";
            this.chkBillingCMRI.Size = new System.Drawing.Size(88, 17);
            this.chkBillingCMRI.TabIndex = 33;
            this.chkBillingCMRI.Text = "Billing History";
            this.chkBillingCMRI.UseVisualStyleBackColor = true;
            // 
            // chkInstaCMRI
            // 
            this.chkInstaCMRI.AutoSize = true;
            this.chkInstaCMRI.Checked = true;
            this.chkInstaCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInstaCMRI.Location = new System.Drawing.Point(35, 27);
            this.chkInstaCMRI.Name = "chkInstaCMRI";
            this.chkInstaCMRI.Size = new System.Drawing.Size(93, 17);
            this.chkInstaCMRI.TabIndex = 32;
            this.chkInstaCMRI.Text = "Instantaneous";
            this.chkInstaCMRI.UseVisualStyleBackColor = true;
            // 
            // Duration_Timer
            // 
            this.Duration_Timer.Tick += new System.EventHandler(this.Duration_Timer_Tick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "S.No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Real Time Clock";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 175;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Port Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(326, 128);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(108, 13);
            this.label43.TabIndex = 9;
            this.label43.Text = "* Valid Range (1-300)";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(326, 92);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(108, 13);
            this.label42.TabIndex = 9;
            this.label42.Text = "* Valid Range (1-600)";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(326, 57);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(108, 13);
            this.label41.TabIndex = 9;
            this.label41.Text = "* Valid Range (1-300)";
            // 
            // chkBoxAutoScrollTime
            // 
            this.chkBoxAutoScrollTime.AutoSize = true;
            this.chkBoxAutoScrollTime.Location = new System.Drawing.Point(67, 127);
            this.chkBoxAutoScrollTime.Name = "chkBoxAutoScrollTime";
            this.chkBoxAutoScrollTime.Size = new System.Drawing.Size(145, 17);
            this.chkBoxAutoScrollTime.TabIndex = 8;
            this.chkBoxAutoScrollTime.Text = "Auto Scroll Resume Time";
            this.chkBoxAutoScrollTime.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(64, 92);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(106, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "Push Button Timeout";
            // 
            // btnReadDisplayParamsTimeout
            // 
            this.btnReadDisplayParamsTimeout.Enabled = false;
            this.btnReadDisplayParamsTimeout.Location = new System.Drawing.Point(326, 221);
            this.btnReadDisplayParamsTimeout.Name = "btnReadDisplayParamsTimeout";
            this.btnReadDisplayParamsTimeout.Size = new System.Drawing.Size(75, 26);
            this.btnReadDisplayParamsTimeout.TabIndex = 7;
            this.btnReadDisplayParamsTimeout.Text = "Read";
            this.btnReadDisplayParamsTimeout.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(64, 57);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Scroll Time Per Item";
            // 
            // txtBoxAutoScrollTime
            // 
            this.txtBoxAutoScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxAutoScrollTime.Enabled = false;
            this.txtBoxAutoScrollTime.Location = new System.Drawing.Point(245, 125);
            this.txtBoxAutoScrollTime.MaxLength = 3;
            this.txtBoxAutoScrollTime.Name = "txtBoxAutoScrollTime";
            this.txtBoxAutoScrollTime.Size = new System.Drawing.Size(75, 22);
            this.txtBoxAutoScrollTime.TabIndex = 5;
            // 
            // txtBoxPushTimeout
            // 
            this.txtBoxPushTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxPushTimeout.Location = new System.Drawing.Point(245, 89);
            this.txtBoxPushTimeout.MaxLength = 3;
            this.txtBoxPushTimeout.Name = "txtBoxPushTimeout";
            this.txtBoxPushTimeout.Size = new System.Drawing.Size(75, 22);
            this.txtBoxPushTimeout.TabIndex = 5;
            // 
            // txtBoxScrollTime
            // 
            this.txtBoxScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxScrollTime.Location = new System.Drawing.Point(245, 54);
            this.txtBoxScrollTime.MaxLength = 3;
            this.txtBoxScrollTime.Name = "txtBoxScrollTime";
            this.txtBoxScrollTime.Size = new System.Drawing.Size(75, 22);
            this.txtBoxScrollTime.TabIndex = 5;
            // 
            // btnWriteDisplayParamsTimeout
            // 
            this.btnWriteDisplayParamsTimeout.Enabled = false;
            this.btnWriteDisplayParamsTimeout.Location = new System.Drawing.Point(245, 221);
            this.btnWriteDisplayParamsTimeout.Name = "btnWriteDisplayParamsTimeout";
            this.btnWriteDisplayParamsTimeout.Size = new System.Drawing.Size(75, 26);
            this.btnWriteDisplayParamsTimeout.TabIndex = 6;
            this.btnWriteDisplayParamsTimeout.Text = "Write";
            this.btnWriteDisplayParamsTimeout.UseVisualStyleBackColor = true;
            // 
            // DLMSMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1231, 887);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "DLMSMain";
            this.StatusMessage = global::DLMS_Final.Utility.UtilityMessage.CT;
            this.Text = "DLMS COM MODULE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLMSMain_FormClosed);
            this.Load += new System.EventHandler(this.DLMSMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DLMSMain_FormClosing);
            this.Activated += new System.EventHandler(this.DLMSMain_Activated);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageCompartment1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grpBoxDLMSRead.ResumeLayout(false);
            this.grpBoxDLMSRead.PerformLayout();
            this.grpBoxLS.ResumeLayout(false);
            this.grpBoxLS.PerformLayout();
            this.grpBoxEventLog.ResumeLayout(false);
            this.grpBoxEventLog.PerformLayout();
            this.grpBoxBillingHistory.ResumeLayout(false);
            this.grpBoxBillingHistory.PerformLayout();
            this.tabPageCompartment2.ResumeLayout(false);
            this.tabControlCMRI.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.grpPartialRead.ResumeLayout(false);
            this.grpPartialRead.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabProgramming.ResumeLayout(false);
            this.tabCTPTRatio.ResumeLayout(false);
            this.tabPageRTC.ResumeLayout(false);
            this.tabPageRTC.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVReadRTC)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.tabPageTOUConfiguration.ResumeLayout(false);
            this.tabPageTOUConfiguration.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageSeason1.ResumeLayout(false);
            this.grpDayTables.ResumeLayout(false);
            this.grpDayTables.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay4)).EndInit();
            this.tabPageSeason2.ResumeLayout(false);
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay10)).EndInit();
            this.tabPageSeason3.ResumeLayout(false);
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay16)).EndInit();
            this.tabPageSeason4.ResumeLayout(false);
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTOUDay22)).EndInit();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridActivationDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDayTables)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.tabPageIntegrationPeriod.ResumeLayout(false);
            this.tabPageIntegrationPeriod.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.tbCTPTRatio.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPTRatio)).EndInit();
            this.gbCTPTRatio.ResumeLayout(false);
            this.gbCTPTRatio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCTRatio)).EndInit();
            this.tabMDReset.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.tbPDisplayParameters.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tabControlDisplayParams.ResumeLayout(false);
            this.tabPagePushButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVPushDisplayParams)).EndInit();
            this.tabPageScrollButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVScrollDisplayParams)).EndInit();
            this.tabPageHighResolution.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVHighResolution)).EndInit();
            this.tabPageDisplayTimeOut.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabKVAH.ResumeLayout(false);
            this.tabKVAH.PerformLayout();
            this.groupBox59.ResumeLayout(false);
            this.groupBox59.PerformLayout();
            this.tabRS232Lock.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.tabCMRI.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabGSM.ResumeLayout(false);
            this.tabGSM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPCBA.ResumeLayout(false);
            this.tabPCBA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPCBA)).EndInit();
            this.tabMeterAccuracyCheck.ResumeLayout(false);
            this.tabMeterAccuracyCheck.PerformLayout();
            this.gbMeterAccuracyCheck.ResumeLayout(false);
            this.gbMeterAccuracyCheck.PerformLayout();
            this.tabPhasor.ResumeLayout(false);
            this.tabPhasor.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tabPageCompartment4.ResumeLayout(false);
            this.groupBox35.ResumeLayout(false);
            this.groupBox35.PerformLayout();
            this.gbPort.ResumeLayout(false);
            this.gbPort.PerformLayout();
            this.panelCommunicationType.ResumeLayout(false);
            this.panelCommunicationType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).EndInit();
            this.groupBox65.ResumeLayout(false);
            this.groupBox65.PerformLayout();
            this.groupBox69.ResumeLayout(false);
            this.groupBox69.PerformLayout();
            this.groupBox70.ResumeLayout(false);
            this.groupBox70.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox67.ResumeLayout(false);
            this.groupBox67.PerformLayout();
            this.groupBox66.ResumeLayout(false);
            this.groupBox66.PerformLayout();
            this.groupBox68.ResumeLayout(false);
            this.groupBox68.PerformLayout();
            this.groupBox36.ResumeLayout(false);
            this.groupBox36.PerformLayout();
            this.groupBox63.ResumeLayout(false);
            this.groupBox63.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }











        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMeterID;
        public System.Windows.Forms.Label lblMeterIDVal;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLblSettings;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageCompartment1;
        private System.Windows.Forms.TabPage tabPageCompartment2;
        private System.Windows.Forms.TabPage tabPageCompartment4;
        private System.Windows.Forms.GroupBox grpBoxLS;
        private System.Windows.Forms.RadioButton rdBtnReadCompleteLS;
        private System.Windows.Forms.RadioButton rdBtnReadBetweenLS;
        private System.Windows.Forms.DateTimePicker dtPickerFrom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtPickerTo;
        private System.Windows.Forms.GroupBox grpBoxBillingHistory;
        private System.Windows.Forms.RadioButton rdBtnReadBetween;
        private System.Windows.Forms.RadioButton rdBtnReadLast;
        private System.Windows.Forms.ComboBox cmbBoxFrom;
        private System.Windows.Forms.ComboBox cmbBoxLastFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMonths;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBoxTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdBtnReadComplete;
        private System.Windows.Forms.Button btnReadAll;
        private System.Windows.Forms.GroupBox grpBoxEventLog;
        private System.Windows.Forms.RadioButton rdBtnReadBetweenEvent;
        private System.Windows.Forms.RadioButton rdBtnReadLastEvent;
        private System.Windows.Forms.RadioButton rdBtnReadCompleteEvent;
        private System.Windows.Forms.ComboBox cmbBoxFromEvent;
        private System.Windows.Forms.ComboBox cmbBoxLastFromEvent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbBoxToEvent;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grpBoxDLMSRead;
        private System.Windows.Forms.CheckBox chkOther;
        private System.Windows.Forms.CheckBox chkNameplate;
        private System.Windows.Forms.CheckBox chkTamper;
        private System.Windows.Forms.CheckBox chkLoadSurvey;
        private System.Windows.Forms.CheckBox chkBilling;
        private System.Windows.Forms.CheckBox chkInsta;
        private System.Windows.Forms.GroupBox groupBox35;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtBoxScaleXML;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.GroupBox groupBox36;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox txtHLSPwd;
        private System.Windows.Forms.Label labelHLS;
        private System.Windows.Forms.TextBox txtPWD;
        private System.Windows.Forms.Label labelPwd;
        private System.Windows.Forms.GroupBox groupBox63;
        private System.Windows.Forms.ComboBox cmbSecurity;
        private System.Windows.Forms.ComboBox cmbContext;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.GroupBox groupBox66;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.GroupBox groupBox67;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.Label label204;
        private System.Windows.Forms.TextBox txtGSMInterFrameTime;
        private System.Windows.Forms.TextBox txtBoxInterFrameTime;
        private System.Windows.Forms.TextBox txtResponseTimeOut;
        private System.Windows.Forms.Label label205;
        private System.Windows.Forms.Label label206;
        private System.Windows.Forms.Label label207;
        private System.Windows.Forms.GroupBox groupBox68;
        private System.Windows.Forms.TextBox txtServerLowerMacAddress;
        private System.Windows.Forms.TextBox txtServerSAP;
        private System.Windows.Forms.Label label208;
        private System.Windows.Forms.Label label209;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox65;
        private System.Windows.Forms.GroupBox groupBox69;
        private System.Windows.Forms.RadioButton rdBtnRJPort;
        private System.Windows.Forms.RadioButton rdBtnOpticalPort;
        private System.Windows.Forms.GroupBox groupBox70;
        private System.Windows.Forms.RadioButton rdBtnModeE;
        private System.Windows.Forms.RadioButton rdBtnDirectHDLC;
        internal System.Windows.Forms.ComboBox cmbAvailableSerialPort;
        private System.Windows.Forms.Label COMPortSet_lblCOMPort;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabGSM;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox textBoxGSM;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TabPage tabProgramming;
        private System.Windows.Forms.Timer timerRTC;
        private System.Windows.Forms.TabControl tabCTPTRatio;
        private System.Windows.Forms.TabPage tabPageRTC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.ComboBox cmbBoxBillingMinute;
        private System.Windows.Forms.ComboBox cmbBoxBillingHour;
        private System.Windows.Forms.ComboBox cmbBoxBillingDate;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cmbBoxBillingPeriod;
        private System.Windows.Forms.Button btnReadBillingDatetime;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnWriteBillingDatetime;
        private System.Windows.Forms.TabPage tabPageTOUConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.ComboBox cmbBoxLSCapturePeriod;
        private System.Windows.Forms.Button btnReadLSCapturePeriod;
        private System.Windows.Forms.Button btnWriteLSCapturePeriod;
        private System.Windows.Forms.TabPage tabPageIntegrationPeriod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.ComboBox cmbBoxIntegrationPeriod;
        private System.Windows.Forms.Button btnReadIntegrationPeriod;
        private System.Windows.Forms.Button btnWriteIntegrationPeriod;
        private System.Windows.Forms.TabPage tabCMRI;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnGenerateFile;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnWriteAll;
        private System.Windows.Forms.DataGridView dGVReadRTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRTC;
        private System.Windows.Forms.Button btnReadRTC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBoxRTC;
        private System.Windows.Forms.Button btnWriteRTC;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        internal System.Windows.Forms.ComboBox cmbCMRIType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gbPort;
        private System.Windows.Forms.RadioButton rdbMultiplePorts;
        private System.Windows.Forms.RadioButton rdbSinglePort;
        private System.Windows.Forms.Button btnAutoConfigModem;
        private System.Windows.Forms.DataGridView dgvPortUsageAssociation;
        private System.Windows.Forms.ErrorProvider errpPortMapping;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.ToolStripStatusLabel toolstripStatus;
        private System.Windows.Forms.ToolStripStatusLabel dataDownloadStatus;
        private System.Windows.Forms.Label lblLoadSurveyCapturePeriod;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.Label lblDemandIPSeconds;
        private System.Windows.Forms.Label lblDemandIntegrationPeriod;
        private System.Windows.Forms.TabControl tabControlCMRI;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkOtherCMRI;
        private System.Windows.Forms.CheckBox chkNameplateCMRI;
        private System.Windows.Forms.CheckBox chkTamperCMRI;
        private System.Windows.Forms.CheckBox chkLoadSurveyCMRI;
        private System.Windows.Forms.CheckBox chkBillingCMRI;
        private System.Windows.Forms.CheckBox chkInstaCMRI;
        private System.Windows.Forms.CheckedListBox lstCMRIfile;
        private System.Windows.Forms.Button btnCMRICancel;
        private System.Windows.Forms.Button btnReadAllCMRI;
        private System.Windows.Forms.Button btnLoadList;
        private System.Windows.Forms.CheckBox chkSelectAllMeters;
        private System.Windows.Forms.GroupBox grpPartialRead;
        private System.Windows.Forms.CheckBox chkCMRINameplate;
        private System.Windows.Forms.CheckBox chkCMRITamper;
        private System.Windows.Forms.CheckBox chkCMRILoadSurvey;
        private System.Windows.Forms.CheckBox chkCMRIBilling;
        private System.Windows.Forms.CheckBox chkCMRIInstant;
        private System.Windows.Forms.CheckedListBox lstFast;
        private System.Windows.Forms.CheckBox chkFDSelectAll;
        private System.Windows.Forms.Button btnLoadMeterFD;
        private System.Windows.Forms.Button btnCancelFD;
        private System.Windows.Forms.Button btnFDRead;
        private System.Windows.Forms.CheckBox chkMidnightData;
        private System.Windows.Forms.TabPage tabPCBA;
        private System.Windows.Forms.Label lblPCBAMeterID;
        private System.Windows.Forms.Label lblDisplayMeterId;
        private System.Windows.Forms.Button btnPCBAExport;
        private System.Windows.Forms.Button btnPCBARead;
        private System.Windows.Forms.DataGridView grdPCBA;
        private System.Windows.Forms.CheckBox chkCMRIMidnightData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rdFastDownload;
        private System.Windows.Forms.RadioButton chkNormalDownload;
        private System.Windows.Forms.TabPage tabMeterAccuracyCheck;
        private System.Windows.Forms.Button btnAccuracyCheckCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox gbMeterAccuracyCheck;
        private System.Windows.Forms.Label lblduration;
        private System.Windows.Forms.TextBox txtkvarhLagInitial;
        private System.Windows.Forms.TextBox txtkvarhLeadDelta;
        private System.Windows.Forms.TextBox txtkvarhLeadFinal;
        private System.Windows.Forms.TextBox txtkvarhLeadInitial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTestduration;
        private System.Windows.Forms.Label lblTestDuration;
        private System.Windows.Forms.TextBox txtkvarhLagDelta;
        private System.Windows.Forms.TextBox txtkVAhDelta;
        private System.Windows.Forms.TextBox txtkWhDelta;
        private System.Windows.Forms.TextBox txtkvarhLagFinal;
        private System.Windows.Forms.TextBox txtkVAhFinal;
        private System.Windows.Forms.TextBox txtkWhFinal;
        private System.Windows.Forms.TextBox txtkVAhInitial;
        private System.Windows.Forms.TextBox txtkWhInitial;
        private System.Windows.Forms.Label lblDelta;
        private System.Windows.Forms.Label lblFinalReading;
        private System.Windows.Forms.Label lblInitialReading;
        private System.Windows.Forms.Label lblkvarh;
        private System.Windows.Forms.Label lblkVAh;
        private System.Windows.Forms.Label lblkWh;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Timer Duration_Timer;
        private System.Windows.Forms.TabPage tbCTPTRatio;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnReadPTRatio;
        private System.Windows.Forms.Label lblPTRatio;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.NumericUpDown nudPTRatio;
        private System.Windows.Forms.GroupBox gbCTPTRatio;
        private System.Windows.Forms.Button btnCTRatio;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblCTPTRatio;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCTPTWrite;
        private System.Windows.Forms.NumericUpDown nudCTRatio;
        private System.Windows.Forms.CheckBox chkPhasor;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.TabPage tabPhasor;
        private CAB.UI.Controls.PhasorDiagram phasorDiagram1;
        private System.Windows.Forms.Button btnCancelPhasor;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnReadPhasor;
        private System.Windows.Forms.Label lblPhasorData;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label lblRLagLead;
        private System.Windows.Forms.Label lblYLagLead;
        private System.Windows.Forms.Label lblYChannel;
        private System.Windows.Forms.Label lblBChannel;
        private System.Windows.Forms.Label lblBLagLead;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblBPhaseKWDir;
        private System.Windows.Forms.Label lblRChannel;
        private System.Windows.Forms.Label lblYPhasePF;
        private System.Windows.Forms.Label lblBPhasePF;
        private System.Windows.Forms.Label lblRPhaseKWDir;
        private System.Windows.Forms.Label lblYPhaseKWDir;
        private System.Windows.Forms.Label lblRPhasePF;
        private System.Windows.Forms.Label lblPhaseSeq;
        private System.Windows.Forms.Label lblReactivePower;
        private System.Windows.Forms.Label lblApparentPower;
        private System.Windows.Forms.Label lblBCurrent;
        private System.Windows.Forms.Label lblActivePower;
        private System.Windows.Forms.Label lblRCurrent;
        private System.Windows.Forms.Label lblYCurrent;
        private System.Windows.Forms.Label lblYVoltage;
        private System.Windows.Forms.Label lblBVoltage;
        private System.Windows.Forms.Label lblRPhaseVoltage;
        private System.Windows.Forms.Label lblAngelYR;
        private System.Windows.Forms.Label lblAngleBR;
        private System.Windows.Forms.Label lblAngleBwTwo;
        private System.Windows.Forms.Label lblTotalPWFactor;
        private System.Windows.Forms.Label lblAngelYRValue;
        private System.Windows.Forms.Label lblAngleBRValue;
        private System.Windows.Forms.Label lblAngleBwTwoValue;
        private System.Windows.Forms.Label lblTotalPWFactorValue;
        private System.Windows.Forms.Label lblRLagLeadValue;
        private System.Windows.Forms.Label lblYLagLeadValue;
        private System.Windows.Forms.Label lblYChannelValue;
        private System.Windows.Forms.Label lblBChannelValue;
        private System.Windows.Forms.Label lblBLagLeadValue;
        private System.Windows.Forms.Label lblBPhaseKWDirValue;
        private System.Windows.Forms.Label lblRChannelValue;
        private System.Windows.Forms.Label lblRPhaseKWDirVAlue;
        private System.Windows.Forms.Label lblYPhaseKWDirValue;
        private System.Windows.Forms.Label lblFrequencyValue;
        private System.Windows.Forms.Label lblYPhasePFValue;
        private System.Windows.Forms.Label lblBPhaesPFValue;
        private System.Windows.Forms.Label lblRPhasePFValue;
        private System.Windows.Forms.Label lblPhaseSeqValue;
        private System.Windows.Forms.Label lblReactivePowerValue;
        private System.Windows.Forms.Label lblApparentPowerValue;
        private System.Windows.Forms.Label lblBCurrentValue;
        private System.Windows.Forms.Label lblActivePowerValue;
        private System.Windows.Forms.Label lblRCurrentValue;
        private System.Windows.Forms.Label lblYCurrentValue;
        private System.Windows.Forms.Label lblYVoltageValue;
        private System.Windows.Forms.Label lblBVoltageValue;
        private System.Windows.Forms.Label lblRVoltageValue;
        private System.Windows.Forms.TabPage tabMDReset;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.CheckBox chkMDReset;
        private System.Windows.Forms.Button btnMDReset;
        private System.Windows.Forms.TabPage tbPDisplayParameters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.TabControl tabControlDisplayParams;
        private System.Windows.Forms.TabPage tabPagePushButton;
        private System.Windows.Forms.DataGridView dGVPushDisplayParams;
        private System.Windows.Forms.TabPage tabPageScrollButton;
        private System.Windows.Forms.DataGridView dGVScrollDisplayParams;
        private System.Windows.Forms.TabPage tabPageHighResolution;
        private System.Windows.Forms.DataGridView dGVHighResolution;
        private System.Windows.Forms.Button btnWriteDisplayParams;
        private System.Windows.Forms.Button btnReadDisplayParams;
        private System.Windows.Forms.CheckBox chkBoxSelectAll;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnUpScroll;
        private System.Windows.Forms.Button btnDownScroll;
        private System.Windows.Forms.TabPage tabKVAH;
        private System.Windows.Forms.Button btnReadKVAhSelection;
        private System.Windows.Forms.Button btnWriteKVAhSelection;
        private System.Windows.Forms.GroupBox groupBox59;
        private System.Windows.Forms.RadioButton chkKVAhLagLead;
        private System.Windows.Forms.RadioButton chkKVAhLagOnly;
        private System.Windows.Forms.TabPage tabRS232Lock;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.TabPage tabPageDisplayTimeOut;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtScrollResumeTime;
        private System.Windows.Forms.TextBox txtPushButtonTimeout;
        private System.Windows.Forms.TextBox txtScrollTime;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.CheckBox chkBoxAutoScrollTime;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnReadDisplayParamsTimeout;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtBoxAutoScrollTime;
        private System.Windows.Forms.TextBox txtBoxPushTimeout;
        private System.Windows.Forms.TextBox txtBoxScrollTime;
        private System.Windows.Forms.Button btnWriteDisplayParamsTimeout;
        private System.Windows.Forms.CheckBox chkAutoScrollTime;
        private System.Windows.Forms.CheckBox chkCMRISelectAll;
        private System.Windows.Forms.Panel panelCommunicationType;
        private System.Windows.Forms.RadioButton rdPSTN;
        private System.Windows.Forms.RadioButton rdGSM;
        private System.Windows.Forms.RadioButton rdDirect;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageSeason1;
        private System.Windows.Forms.GroupBox grpDayTables;
        private System.Windows.Forms.DataGridView gridTOUDay6;
        private System.Windows.Forms.DataGridView gridTOUDay5;
        private System.Windows.Forms.Label lblDayTable6;
        private System.Windows.Forms.Label lblDayTable5;
        private System.Windows.Forms.Label lblDayTable4;
        private System.Windows.Forms.Label lblDayTable3;
        private System.Windows.Forms.Label lblDayTable2;
        private System.Windows.Forms.Label lblDayTable1;
        private System.Windows.Forms.DataGridView gridTOUDay1;
        private System.Windows.Forms.DataGridView gridTOUDay2;
        private System.Windows.Forms.DataGridView gridTOUDay3;
        private System.Windows.Forms.DataGridView gridTOUDay4;
        private System.Windows.Forms.TabPage tabPageSeason2;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.DataGridView gridTOUDay12;
        private System.Windows.Forms.DataGridView gridTOUDay11;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.DataGridView gridTOUDay7;
        private System.Windows.Forms.DataGridView gridTOUDay8;
        private System.Windows.Forms.DataGridView gridTOUDay9;
        private System.Windows.Forms.DataGridView gridTOUDay10;
        private System.Windows.Forms.TabPage tabPageSeason3;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.DataGridView gridTOUDay18;
        private System.Windows.Forms.DataGridView gridTOUDay17;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.DataGridView gridTOUDay13;
        private System.Windows.Forms.DataGridView gridTOUDay14;
        private System.Windows.Forms.DataGridView gridTOUDay15;
        private System.Windows.Forms.DataGridView gridTOUDay16;
        private System.Windows.Forms.TabPage tabPageSeason4;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.DataGridView gridTOUDay24;
        private System.Windows.Forms.DataGridView gridTOUDay23;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.DataGridView gridTOUDay19;
        private System.Windows.Forms.DataGridView gridTOUDay20;
        private System.Windows.Forms.DataGridView gridTOUDay21;
        private System.Windows.Forms.DataGridView gridTOUDay22;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Button btnFillTOUConfiguration;
        private System.Windows.Forms.DateTimePicker dTPFutureActivationDate;
        private System.Windows.Forms.Label label191;
        private System.Windows.Forms.Label lblActivation;
        private System.Windows.Forms.Label lblDayTable;
        private System.Windows.Forms.DataGridView gridActivationDate;
        private System.Windows.Forms.DataGridView gridDayTables;
        private System.Windows.Forms.Button btnTOUWrite;
        private System.Windows.Forms.Button btnReadFutureTOU;
        private System.Windows.Forms.Button btnReadCurrentTOU;
        private System.Windows.Forms.CheckBox chkCMRIPhasor;
        private System.Windows.Forms.RadioButton rdGPRS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPortUsageTypeModem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPortUsageTypeCMRI;
        private System.Windows.Forms.Label lblPhasorNotSupported;
        private System.Windows.Forms.Label lblKvahNotSupported;
        private System.Windows.Forms.ComboBox cmbLSDays;
        private System.Windows.Forms.Label lblReactiveLeadUnit;
        private System.Windows.Forms.Label lblReactiveLagUnit;
        private System.Windows.Forms.Label lblApparentEnergyUnit;
        private System.Windows.Forms.Label lblActiveEnergyUnit;
        private System.Windows.Forms.Label lblNoMeterAccuracyCheck;
        private System.Windows.Forms.Label lblCTRatioMessage;
        private System.Windows.Forms.Label lblPTProgramNotSupported;
    }
}

