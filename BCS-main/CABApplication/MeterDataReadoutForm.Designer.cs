namespace CABApplication
{
    partial class MeterDataReadoutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tpReadData = new System.Windows.Forms.TabPage();
            this.ReadOut_Grpread = new System.Windows.Forms.GroupBox();
            this.grpReadoptions = new System.Windows.Forms.GroupBox();
            this.panelPartialData = new System.Windows.Forms.Panel();
            this.chkMeterConfigurations = new System.Windows.Forms.CheckBox();
            this.chkDTMDaily = new System.Windows.Forms.CheckBox();
            this.chkFraudEnergy = new System.Windows.Forms.CheckBox();
            this.chkPhasor = new System.Windows.Forms.CheckBox();
            this.chkTransaction = new System.Windows.Forms.CheckBox();
            this.chkLoadSurvey = new System.Windows.Forms.CheckBox();
            this.chkTamper = new System.Windows.Forms.CheckBox();
            this.chkGeneral = new System.Windows.Forms.CheckBox();
            this.rbtnPartial = new System.Windows.Forms.RadioButton();
            this.rbtnAll = new System.Windows.Forms.RadioButton();
            this.grpLoadSurvey = new System.Windows.Forms.GroupBox();
            this.btn_Noofdays = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmoDTMdays = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new CAB.UI.Controls.PremiumTabControl();
            this.lngGridViewReadControl1 = new CAB.UI.Controls.CABGridViewReadControl();
            this.lblInfo = new System.Windows.Forms.Label();
            this.groupBoxReadoutContainer = new System.Windows.Forms.GroupBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.dgvMeterIdAndSim = new System.Windows.Forms.DataGridView();
            this.GsmCommPanel = new System.Windows.Forms.Panel();
            this.noMeterFoundStatus = new System.Windows.Forms.Label();
            this.selectAll = new System.Windows.Forms.CheckBox();
            this.grpSimNumber = new System.Windows.Forms.GroupBox();
            this.lngSimNumber = new CAB.UI.Controls.CABLabel();
            this.txtBoxMeterSIM = new System.Windows.Forms.TextBox();
            this.grpCommType = new System.Windows.Forms.GroupBox();
            this.oneToManyGSM = new System.Windows.Forms.RadioButton();
            this.oneToOneGSM = new System.Windows.Forms.RadioButton();
            this.tpReadData.SuspendLayout();
            this.ReadOut_Grpread.SuspendLayout();
            this.grpReadoptions.SuspendLayout();
            this.panelPartialData.SuspendLayout();
            this.grpLoadSurvey.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBoxReadoutContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).BeginInit();
            this.GsmCommPanel.SuspendLayout();
            this.grpSimNumber.SuspendLayout();
            this.grpCommType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpReadData
            // 
            this.tpReadData.AutoScroll = true;
            this.tpReadData.BackColor = System.Drawing.Color.Snow;
            this.tpReadData.Controls.Add(this.ReadOut_Grpread);
            this.tpReadData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.tpReadData.Location = new System.Drawing.Point(4, 34);
            this.tpReadData.Name = "tpReadData";
            this.tpReadData.Padding = new System.Windows.Forms.Padding(3);
            this.tpReadData.Size = new System.Drawing.Size(487, 244);
            this.tpReadData.TabIndex = 0;
            this.tpReadData.Text = "Read Data";
            // 
            // ReadOut_Grpread
            // 
            this.ReadOut_Grpread.Controls.Add(this.grpReadoptions);
            this.ReadOut_Grpread.Controls.Add(this.grpLoadSurvey);
            this.ReadOut_Grpread.Location = new System.Drawing.Point(6, 6);
            this.ReadOut_Grpread.Name = "ReadOut_Grpread";
            this.ReadOut_Grpread.Size = new System.Drawing.Size(537, 290);
            this.ReadOut_Grpread.TabIndex = 20;
            this.ReadOut_Grpread.TabStop = false;
            // 
            // grpReadoptions
            // 
            this.grpReadoptions.Controls.Add(this.panelPartialData);
            this.grpReadoptions.Controls.Add(this.rbtnPartial);
            this.grpReadoptions.Controls.Add(this.rbtnAll);
            this.grpReadoptions.Location = new System.Drawing.Point(10, 20);
            this.grpReadoptions.Name = "grpReadoptions";
            this.grpReadoptions.Size = new System.Drawing.Size(468, 245);
            this.grpReadoptions.TabIndex = 0;
            this.grpReadoptions.TabStop = false;
            this.grpReadoptions.Text = "Read Options";
            // 
            // panelPartialData
            // 
            this.panelPartialData.Controls.Add(this.chkMeterConfigurations);
            this.panelPartialData.Controls.Add(this.chkDTMDaily);
            this.panelPartialData.Controls.Add(this.chkFraudEnergy);
            this.panelPartialData.Controls.Add(this.chkPhasor);
            this.panelPartialData.Controls.Add(this.chkTransaction);
            this.panelPartialData.Controls.Add(this.chkLoadSurvey);
            this.panelPartialData.Controls.Add(this.chkTamper);
            this.panelPartialData.Controls.Add(this.chkGeneral);
            this.panelPartialData.Location = new System.Drawing.Point(-23, 42);
            this.panelPartialData.Name = "panelPartialData";
            this.panelPartialData.Size = new System.Drawing.Size(491, 197);
            this.panelPartialData.TabIndex = 2;
            // 
            // chkMeterConfigurations
            // 
            this.chkMeterConfigurations.AutoSize = true;
            this.chkMeterConfigurations.Location = new System.Drawing.Point(7, 176);
            this.chkMeterConfigurations.Name = "chkMeterConfigurations";
            this.chkMeterConfigurations.Size = new System.Drawing.Size(218, 29);
            this.chkMeterConfigurations.TabIndex = 21;
            this.chkMeterConfigurations.Text = "Meter Configurations";
            this.chkMeterConfigurations.UseVisualStyleBackColor = true;
            this.chkMeterConfigurations.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkDTMDaily
            // 
            this.chkDTMDaily.AutoSize = true;
            this.chkDTMDaily.Location = new System.Drawing.Point(7, 153);
            this.chkDTMDaily.Name = "chkDTMDaily";
            this.chkDTMDaily.Size = new System.Drawing.Size(186, 29);
            this.chkDTMDaily.TabIndex = 7;
            this.chkDTMDaily.Text = "Daily Load Profile";
            this.chkDTMDaily.UseVisualStyleBackColor = true;
            this.chkDTMDaily.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkFraudEnergy
            // 
            this.chkFraudEnergy.AutoSize = true;
            this.chkFraudEnergy.Location = new System.Drawing.Point(7, 129);
            this.chkFraudEnergy.Name = "chkFraudEnergy";
            this.chkFraudEnergy.Size = new System.Drawing.Size(154, 29);
            this.chkFraudEnergy.TabIndex = 5;
            this.chkFraudEnergy.Text = "Fraud Energy ";
            this.chkFraudEnergy.UseVisualStyleBackColor = true;
            this.chkFraudEnergy.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkPhasor
            // 
            this.chkPhasor.AutoSize = true;
            this.chkPhasor.Location = new System.Drawing.Point(7, 106);
            this.chkPhasor.Name = "chkPhasor";
            this.chkPhasor.Size = new System.Drawing.Size(96, 29);
            this.chkPhasor.TabIndex = 4;
            this.chkPhasor.Text = "Phasor";
            this.chkPhasor.UseVisualStyleBackColor = true;
            this.chkPhasor.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkTransaction
            // 
            this.chkTransaction.AutoSize = true;
            this.chkTransaction.Location = new System.Drawing.Point(7, 83);
            this.chkTransaction.Name = "chkTransaction";
            this.chkTransaction.Size = new System.Drawing.Size(134, 29);
            this.chkTransaction.TabIndex = 3;
            this.chkTransaction.Text = "Transaction";
            this.chkTransaction.UseVisualStyleBackColor = true;
            this.chkTransaction.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkLoadSurvey
            // 
            this.chkLoadSurvey.AutoSize = true;
            this.chkLoadSurvey.Location = new System.Drawing.Point(7, 58);
            this.chkLoadSurvey.Name = "chkLoadSurvey";
            this.chkLoadSurvey.Size = new System.Drawing.Size(138, 29);
            this.chkLoadSurvey.TabIndex = 2;
            this.chkLoadSurvey.Text = "Load survey";
            this.chkLoadSurvey.UseVisualStyleBackColor = true;
            this.chkLoadSurvey.CheckedChanged += new System.EventHandler(this.chkLoadSurvey_CheckedChanged);
            // 
            // chkTamper
            // 
            this.chkTamper.AutoSize = true;
            this.chkTamper.Location = new System.Drawing.Point(7, 35);
            this.chkTamper.Name = "chkTamper";
            this.chkTamper.Size = new System.Drawing.Size(100, 29);
            this.chkTamper.TabIndex = 1;
            this.chkTamper.Text = "Tamper";
            this.chkTamper.UseVisualStyleBackColor = true;
            this.chkTamper.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // chkGeneral
            // 
            this.chkGeneral.AutoSize = true;
            this.chkGeneral.Location = new System.Drawing.Point(7, 12);
            this.chkGeneral.Name = "chkGeneral";
            this.chkGeneral.Size = new System.Drawing.Size(241, 29);
            this.chkGeneral.TabIndex = 0;
            this.chkGeneral.Text = "General and Billing data";
            this.chkGeneral.UseVisualStyleBackColor = true;
            this.chkGeneral.CheckedChanged += new System.EventHandler(this.chkGeneral_CheckedChanged);
            // 
            // rbtnPartial
            // 
            this.rbtnPartial.AutoSize = true;
            this.rbtnPartial.Location = new System.Drawing.Point(85, 19);
            this.rbtnPartial.Name = "rbtnPartial";
            this.rbtnPartial.Size = new System.Drawing.Size(132, 29);
            this.rbtnPartial.TabIndex = 1;
            this.rbtnPartial.Text = "Partial data";
            this.rbtnPartial.UseVisualStyleBackColor = true;
            this.rbtnPartial.CheckedChanged += new System.EventHandler(this.rbtnPartial_CheckedChanged);
            // 
            // rbtnAll
            // 
            this.rbtnAll.AutoSize = true;
            this.rbtnAll.Checked = true;
            this.rbtnAll.Location = new System.Drawing.Point(18, 19);
            this.rbtnAll.Name = "rbtnAll";
            this.rbtnAll.Size = new System.Drawing.Size(101, 29);
            this.rbtnAll.TabIndex = 0;
            this.rbtnAll.TabStop = true;
            this.rbtnAll.Text = "All data";
            this.rbtnAll.UseVisualStyleBackColor = true;
            this.rbtnAll.CheckedChanged += new System.EventHandler(this.rbtnAll_CheckedChanged);
            // 
            // grpLoadSurvey
            // 
            this.grpLoadSurvey.Controls.Add(this.btn_Noofdays);
            this.grpLoadSurvey.Controls.Add(this.label5);
            this.grpLoadSurvey.Controls.Add(this.cmoDTMdays);
            this.grpLoadSurvey.Enabled = false;
            this.grpLoadSurvey.Location = new System.Drawing.Point(203, 19);
            this.grpLoadSurvey.Name = "grpLoadSurvey";
            this.grpLoadSurvey.Size = new System.Drawing.Size(319, 73);
            this.grpLoadSurvey.TabIndex = 2;
            this.grpLoadSurvey.TabStop = false;
            this.grpLoadSurvey.Text = "Load Survey";
            // 
            // btn_Noofdays
            // 
            this.btn_Noofdays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btn_Noofdays.FlatAppearance.BorderSize = 0;
            this.btn_Noofdays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Noofdays.ForeColor = System.Drawing.Color.White;
            this.btn_Noofdays.Location = new System.Drawing.Point(237, 24);
            this.btn_Noofdays.Name = "btn_Noofdays";
            this.btn_Noofdays.Size = new System.Drawing.Size(54, 28);
            this.btn_Noofdays.TabIndex = 32;
            this.btn_Noofdays.Text = "Select";
            this.btn_Noofdays.UseVisualStyleBackColor = false;
            this.btn_Noofdays.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(215, 25);
            this.label5.TabIndex = 31;
            this.label5.Text = "No. of Load Survey Days";
            // 
            // cmoDTMdays
            // 
            this.cmoDTMdays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmoDTMdays.FormatString = "N2";
            this.cmoDTMdays.FormattingEnabled = true;
            this.cmoDTMdays.Location = new System.Drawing.Point(183, 27);
            this.cmoDTMdays.MaxLength = 2;
            this.cmoDTMdays.Name = "cmoDTMdays";
            this.cmoDTMdays.Size = new System.Drawing.Size(47, 33);
            this.cmoDTMdays.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpReadData);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.ItemSize = new System.Drawing.Size(120, 30);
            this.tabControl1.Location = new System.Drawing.Point(15, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(495, 282);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Visible = false;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // lngGridViewReadControl1
            // 
            this.lngGridViewReadControl1.AutoScroll = true;
            this.lngGridViewReadControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lngGridViewReadControl1.Location = new System.Drawing.Point(2, 46);
            this.lngGridViewReadControl1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.lngGridViewReadControl1.Name = "lngGridViewReadControl1";
            this.lngGridViewReadControl1.Size = new System.Drawing.Size(553, 378);
            this.lngGridViewReadControl1.TabIndex = 1;
            this.lngGridViewReadControl1.Load += new System.EventHandler(this.lngGridViewReadControl1_Load);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.lblInfo.Location = new System.Drawing.Point(7, 506);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(411, 25);
            this.lblInfo.TabIndex = 55;
            this.lblInfo.Text = "* Name Plate and Instantaneous is default readout";
            // 
            // groupBoxReadoutContainer
            // 
            this.groupBoxReadoutContainer.Controls.Add(this.btnAbort);
            this.groupBoxReadoutContainer.Controls.Add(this.btnRead);
            this.groupBoxReadoutContainer.Controls.Add(this.btnCancel);
            this.groupBoxReadoutContainer.Location = new System.Drawing.Point(12, 453);
            this.groupBoxReadoutContainer.Name = "groupBoxReadoutContainer";
            this.groupBoxReadoutContainer.Size = new System.Drawing.Size(360, 50);
            this.groupBoxReadoutContainer.TabIndex = 56;
            this.groupBoxReadoutContainer.TabStop = false;
            // 
            // btnAbort
            // 
            this.btnAbort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnAbort.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbort.FlatAppearance.BorderSize = 0;
            this.btnAbort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(40)))), ((int)(((byte)(55)))));
            this.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbort.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAbort.ForeColor = System.Drawing.Color.White;
            this.btnAbort.Location = new System.Drawing.Point(120, 14);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(90, 30);
            this.btnAbort.TabIndex = 8;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = false;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRead.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRead.FlatAppearance.BorderSize = 0;
            this.btnRead.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(190)))));
            this.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRead.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnRead.ForeColor = System.Drawing.Color.White;
            this.btnRead.Location = new System.Drawing.Point(14, 14);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(100, 30);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "📊  Read Data";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(220, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(9, 523);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1082, 28);
            this.statusStrip.TabIndex = 57;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 20);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dgvMeterIdAndSim
            // 
            this.dgvMeterIdAndSim.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMeterIdAndSim.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvMeterIdAndSim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMeterIdAndSim.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMeterIdAndSim.Location = new System.Drawing.Point(0, 0);
            this.dgvMeterIdAndSim.Name = "dgvMeterIdAndSim";
            this.dgvMeterIdAndSim.RowHeadersWidth = 45;
            this.dgvMeterIdAndSim.Size = new System.Drawing.Size(562, 279);
            this.dgvMeterIdAndSim.TabIndex = 50;
            // 
            // GsmCommPanel
            // 
            this.GsmCommPanel.Controls.Add(this.noMeterFoundStatus);
            this.GsmCommPanel.Controls.Add(this.selectAll);
            this.GsmCommPanel.Controls.Add(this.grpSimNumber);
            this.GsmCommPanel.Controls.Add(this.grpCommType);
            this.GsmCommPanel.Controls.Add(this.dgvMeterIdAndSim);
            this.GsmCommPanel.Location = new System.Drawing.Point(540, 19);
            this.GsmCommPanel.Name = "GsmCommPanel";
            this.GsmCommPanel.Size = new System.Drawing.Size(553, 431);
            this.GsmCommPanel.TabIndex = 58;
            // 
            // noMeterFoundStatus
            // 
            this.noMeterFoundStatus.AutoSize = true;
            this.noMeterFoundStatus.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.noMeterFoundStatus.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noMeterFoundStatus.ForeColor = System.Drawing.Color.Black;
            this.noMeterFoundStatus.Location = new System.Drawing.Point(142, 13);
            this.noMeterFoundStatus.Name = "noMeterFoundStatus";
            this.noMeterFoundStatus.Size = new System.Drawing.Size(72, 22);
            this.noMeterFoundStatus.TabIndex = 56;
            this.noMeterFoundStatus.Text = "label8";
            this.noMeterFoundStatus.Visible = false;
            // 
            // selectAll
            // 
            this.selectAll.AutoSize = true;
            this.selectAll.Location = new System.Drawing.Point(12, 515);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(115, 29);
            this.selectAll.TabIndex = 54;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.CheckedChanged += new System.EventHandler(this.selectAll_CheckedChanged);
            // 
            // grpSimNumber
            // 
            this.grpSimNumber.Controls.Add(this.lngSimNumber);
            this.grpSimNumber.Controls.Add(this.txtBoxMeterSIM);
            this.grpSimNumber.Enabled = false;
            this.grpSimNumber.Location = new System.Drawing.Point(235, 305);
            this.grpSimNumber.Name = "grpSimNumber";
            this.grpSimNumber.Size = new System.Drawing.Size(298, 61);
            this.grpSimNumber.TabIndex = 55;
            this.grpSimNumber.TabStop = false;
            this.grpSimNumber.Text = "Sim Number";
            // 
            // lngSimNumber
            // 
            this.lngSimNumber.AutoSize = true;
            this.lngSimNumber.Location = new System.Drawing.Point(9, 24);
            this.lngSimNumber.Name = "lngSimNumber";
            this.lngSimNumber.Size = new System.Drawing.Size(196, 25);
            this.lngSimNumber.TabIndex = 56;
            this.lngSimNumber.Text = "Meter SIM No.      +91";
            this.lngSimNumber.TranslationKey = "";
            // 
            // txtBoxMeterSIM
            // 
            this.txtBoxMeterSIM.Location = new System.Drawing.Point(144, 21);
            this.txtBoxMeterSIM.MaxLength = 10;
            this.txtBoxMeterSIM.Name = "txtBoxMeterSIM";
            this.txtBoxMeterSIM.Size = new System.Drawing.Size(136, 33);
            this.txtBoxMeterSIM.TabIndex = 55;
            // 
            // grpCommType
            // 
            this.grpCommType.Controls.Add(this.oneToManyGSM);
            this.grpCommType.Controls.Add(this.oneToOneGSM);
            this.grpCommType.Location = new System.Drawing.Point(12, 305);
            this.grpCommType.Name = "grpCommType";
            this.grpCommType.Size = new System.Drawing.Size(219, 61);
            this.grpCommType.TabIndex = 49;
            this.grpCommType.TabStop = false;
            this.grpCommType.Text = "Communication Type";
            // 
            // oneToManyGSM
            // 
            this.oneToManyGSM.AutoSize = true;
            this.oneToManyGSM.Checked = true;
            this.oneToManyGSM.Location = new System.Drawing.Point(17, 24);
            this.oneToManyGSM.Name = "oneToManyGSM";
            this.oneToManyGSM.Size = new System.Drawing.Size(148, 29);
            this.oneToManyGSM.TabIndex = 49;
            this.oneToManyGSM.TabStop = true;
            this.oneToManyGSM.Text = "One To Many";
            this.oneToManyGSM.UseVisualStyleBackColor = true;
            this.oneToManyGSM.CheckedChanged += new System.EventHandler(this.oneToManyGSM_CheckedChanged);
            // 
            // oneToOneGSM
            // 
            this.oneToOneGSM.AutoSize = true;
            this.oneToOneGSM.Location = new System.Drawing.Point(113, 24);
            this.oneToOneGSM.Name = "oneToOneGSM";
            this.oneToOneGSM.Size = new System.Drawing.Size(136, 29);
            this.oneToOneGSM.TabIndex = 48;
            this.oneToOneGSM.Text = "One To One";
            this.oneToOneGSM.UseVisualStyleBackColor = true;
            this.oneToOneGSM.CheckedChanged += new System.EventHandler(this.oneToOneGSM_CheckedChanged);
            // 
            // MeterDataReadoutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1100, 560);
            this.Controls.Add(this.GsmCommPanel);
            this.Controls.Add(this.groupBoxReadoutContainer);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lngGridViewReadControl1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MeterDataReadoutForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusMessage = "";
            this.Text = "📟  Meter Readout";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MeterDataReadoutForm_FormClosing);
            this.Load += new System.EventHandler(this.MeterDataReadoutForm_Load);
            this.tpReadData.ResumeLayout(false);
            this.ReadOut_Grpread.ResumeLayout(false);
            this.grpReadoptions.ResumeLayout(false);
            this.grpReadoptions.PerformLayout();
            this.panelPartialData.ResumeLayout(false);
            this.panelPartialData.PerformLayout();
            this.grpLoadSurvey.ResumeLayout(false);
            this.grpLoadSurvey.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBoxReadoutContainer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).EndInit();
            this.GsmCommPanel.ResumeLayout(false);
            this.GsmCommPanel.PerformLayout();
            this.grpSimNumber.ResumeLayout(false);
            this.grpSimNumber.PerformLayout();
            this.grpCommType.ResumeLayout(false);
            this.grpCommType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tpReadData;
        private System.Windows.Forms.GroupBox ReadOut_Grpread;
        private System.Windows.Forms.GroupBox grpReadoptions;
        private System.Windows.Forms.Panel panelPartialData;
        private System.Windows.Forms.CheckBox chkMeterConfigurations;
        private System.Windows.Forms.CheckBox chkDTMDaily;
        private System.Windows.Forms.CheckBox chkFraudEnergy;
        private System.Windows.Forms.CheckBox chkPhasor;
        private System.Windows.Forms.CheckBox chkTransaction;
        private System.Windows.Forms.CheckBox chkLoadSurvey;
        private System.Windows.Forms.CheckBox chkTamper;
        private System.Windows.Forms.CheckBox chkGeneral;
        private System.Windows.Forms.RadioButton rbtnPartial;
        private System.Windows.Forms.RadioButton rbtnAll;
        private System.Windows.Forms.GroupBox grpLoadSurvey;
        private System.Windows.Forms.Button btn_Noofdays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmoDTMdays;
        private CAB.UI.Controls.PremiumTabControl tabControl1;
        private CAB.UI.Controls.CABGridViewReadControl lngGridViewReadControl1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox groupBoxReadoutContainer;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Timer progressBarTimer;
        private System.Windows.Forms.DataGridView dgvMeterIdAndSim;
        private System.Windows.Forms.Panel GsmCommPanel;
        private System.Windows.Forms.Label noMeterFoundStatus;
        private System.Windows.Forms.CheckBox selectAll;
        private System.Windows.Forms.GroupBox grpSimNumber;
        private CAB.UI.Controls.CABLabel lngSimNumber;
        private System.Windows.Forms.TextBox txtBoxMeterSIM;
        private System.Windows.Forms.GroupBox grpCommType;
        private System.Windows.Forms.RadioButton oneToManyGSM;
        private System.Windows.Forms.RadioButton oneToOneGSM;




    }
}




