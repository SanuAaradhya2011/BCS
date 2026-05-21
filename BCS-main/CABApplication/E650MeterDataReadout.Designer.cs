namespace CABApplication
{
    partial class E650MeterDataReadout
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.lngGridViewReadControl1 = new CAB.UI.Controls.CABGridViewReadControl();
            this.grpCommType = new System.Windows.Forms.GroupBox();
            this.oneToManyGSM = new System.Windows.Forms.RadioButton();
            this.oneToOneGSM = new System.Windows.Forms.RadioButton();
            this.dgvMeterIdAndSim = new System.Windows.Forms.DataGridView();
            this.GsmCommPanel = new System.Windows.Forms.Panel();
            this.noMeterFoundStatus = new System.Windows.Forms.Label();
            this.selectAll = new System.Windows.Forms.CheckBox();
            this.grpSimNumber = new System.Windows.Forms.GroupBox();
            this.lngPort = new CAB.UI.Controls.CABLabel();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lngSimNumber = new CAB.UI.Controls.CABLabel();
            this.txtBoxMeterSIM = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.grpCommType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).BeginInit();
            this.GsmCommPanel.SuspendLayout();
            this.grpSimNumber.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox1.Location = new System.Drawing.Point(278, 109);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.label2.Location = new System.Drawing.Point(99, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 25);
            this.label2.TabIndex = 40;
            this.label2.Text = "THD Voltage B Phase";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.groupBox1.Location = new System.Drawing.Point(33, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(415, 251);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "THD Voltage/Current";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox2.Location = new System.Drawing.Point(278, 142);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 31);
            this.textBox2.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.label3.Location = new System.Drawing.Point(99, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(177, 25);
            this.label3.TabIndex = 38;
            this.label3.Text = "THD Current R Phase";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox3.Location = new System.Drawing.Point(278, 73);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 31);
            this.textBox3.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.label6.Location = new System.Drawing.Point(99, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(178, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "THD Voltage Y Phase";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.label7.Location = new System.Drawing.Point(99, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 25);
            this.label7.TabIndex = 1;
            this.label7.Text = "THD Voltage R Phase";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.White;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox4.Location = new System.Drawing.Point(278, 33);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 31);
            this.textBox4.TabIndex = 1;
            // 
            // lngGridViewReadControl1
            // 
            this.lngGridViewReadControl1.AutoScroll = true;
            this.lngGridViewReadControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lngGridViewReadControl1.Location = new System.Drawing.Point(2, 19);
            this.lngGridViewReadControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lngGridViewReadControl1.Name = "lngGridViewReadControl1";
            this.lngGridViewReadControl1.Size = new System.Drawing.Size(380, 221);
            this.lngGridViewReadControl1.TabIndex = 7;
            this.lngGridViewReadControl1.Load += new System.EventHandler(this.lngGridViewReadControl1_Load);
            // 
            // grpCommType
            // 
            this.grpCommType.BackColor = System.Drawing.Color.White;
            this.grpCommType.Controls.Add(this.oneToManyGSM);
            this.grpCommType.Controls.Add(this.oneToOneGSM);
            this.grpCommType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCommType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.grpCommType.Location = new System.Drawing.Point(10, 305);
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
            this.oneToManyGSM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.oneToManyGSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.oneToManyGSM.Location = new System.Drawing.Point(17, 24);
            this.oneToManyGSM.Name = "oneToManyGSM";
            this.oneToManyGSM.Size = new System.Drawing.Size(142, 29);
            this.oneToManyGSM.TabIndex = 49;
            this.oneToManyGSM.TabStop = true;
            this.oneToManyGSM.Text = "One To Many";
            this.oneToManyGSM.UseVisualStyleBackColor = true;
            this.oneToManyGSM.CheckedChanged += new System.EventHandler(this.oneToManyGSM_CheckedChanged);
            // 
            // oneToOneGSM
            // 
            this.oneToOneGSM.AutoSize = true;
            this.oneToOneGSM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.oneToOneGSM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.oneToOneGSM.Location = new System.Drawing.Point(113, 24);
            this.oneToOneGSM.Name = "oneToOneGSM";
            this.oneToOneGSM.Size = new System.Drawing.Size(131, 29);
            this.oneToOneGSM.TabIndex = 48;
            this.oneToOneGSM.Text = "One To One";
            this.oneToOneGSM.UseVisualStyleBackColor = true;
            this.oneToOneGSM.CheckedChanged += new System.EventHandler(this.oneToOneGSM_CheckedChanged);
            // 
            // dgvMeterIdAndSim
            // 
            this.dgvMeterIdAndSim.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMeterIdAndSim.BackgroundColor = System.Drawing.Color.White;
            this.dgvMeterIdAndSim.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMeterIdAndSim.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMeterIdAndSim.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvMeterIdAndSim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMeterIdAndSim.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgvMeterIdAndSim.EnableHeadersVisualStyles = false;
            this.dgvMeterIdAndSim.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.dgvMeterIdAndSim.Location = new System.Drawing.Point(0, 0);
            this.dgvMeterIdAndSim.Name = "dgvMeterIdAndSim";
            this.dgvMeterIdAndSim.RowHeadersWidth = 45;
            this.dgvMeterIdAndSim.Size = new System.Drawing.Size(579, 279);
            this.dgvMeterIdAndSim.TabIndex = 50;
            this.dgvMeterIdAndSim.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeterIdAndSim_CellContentClick);
            this.dgvMeterIdAndSim.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvMeterIdAndSim_CurrentCellDirtyStateChanged);
            // 
            // GsmCommPanel
            // 
            this.GsmCommPanel.BackColor = System.Drawing.Color.White;
            this.GsmCommPanel.Controls.Add(this.noMeterFoundStatus);
            this.GsmCommPanel.Controls.Add(this.selectAll);
            this.GsmCommPanel.Controls.Add(this.grpSimNumber);
            this.GsmCommPanel.Controls.Add(this.grpCommType);
            this.GsmCommPanel.Controls.Add(this.dgvMeterIdAndSim);
            this.GsmCommPanel.Location = new System.Drawing.Point(354, 19);
            this.GsmCommPanel.Name = "GsmCommPanel";
            this.GsmCommPanel.Size = new System.Drawing.Size(574, 431);
            this.GsmCommPanel.TabIndex = 52;
            // 
            // noMeterFoundStatus
            // 
            this.noMeterFoundStatus.AutoSize = true;
            this.noMeterFoundStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.noMeterFoundStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.noMeterFoundStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.noMeterFoundStatus.Location = new System.Drawing.Point(142, 13);
            this.noMeterFoundStatus.Name = "noMeterFoundStatus";
            this.noMeterFoundStatus.Size = new System.Drawing.Size(63, 25);
            this.noMeterFoundStatus.TabIndex = 56;
            this.noMeterFoundStatus.Text = "label8";
            this.noMeterFoundStatus.Visible = false;
            // 
            // selectAll
            // 
            this.selectAll.AutoSize = true;
            this.selectAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.selectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.selectAll.Location = new System.Drawing.Point(12, 287);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(109, 29);
            this.selectAll.TabIndex = 54;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.CheckedChanged += new System.EventHandler(this.selectAll_CheckedChanged);
            // 
            // grpSimNumber
            // 
            this.grpSimNumber.BackColor = System.Drawing.Color.White;
            this.grpSimNumber.Controls.Add(this.lngPort);
            this.grpSimNumber.Controls.Add(this.txtPort);
            this.grpSimNumber.Controls.Add(this.lngSimNumber);
            this.grpSimNumber.Controls.Add(this.txtBoxMeterSIM);
            this.grpSimNumber.Enabled = false;
            this.grpSimNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSimNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.grpSimNumber.Location = new System.Drawing.Point(235, 305);
            this.grpSimNumber.Name = "grpSimNumber";
            this.grpSimNumber.Size = new System.Drawing.Size(298, 73);
            this.grpSimNumber.TabIndex = 55;
            this.grpSimNumber.TabStop = false;
            this.grpSimNumber.Text = "Sim Number";
            // 
            // lngPort
            // 
            this.lngPort.AutoSize = true;
            this.lngPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lngPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.lngPort.Location = new System.Drawing.Point(10, 49);
            this.lngPort.Name = "lngPort";
            this.lngPort.Size = new System.Drawing.Size(44, 25);
            this.lngPort.TabIndex = 58;
            this.lngPort.Text = "Port";
            this.lngPort.TranslationKey = null;
            this.lngPort.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(145, 47);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(136, 31);
            this.txtPort.TabIndex = 57;
            this.txtPort.Visible = false;
            // 
            // lngSimNumber
            // 
            this.lngSimNumber.AutoSize = true;
            this.lngSimNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lngSimNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.lngSimNumber.Location = new System.Drawing.Point(9, 24);
            this.lngSimNumber.Name = "lngSimNumber";
            this.lngSimNumber.Size = new System.Drawing.Size(189, 25);
            this.lngSimNumber.TabIndex = 56;
            this.lngSimNumber.Text = "Meter SIM No.      +91";
            this.lngSimNumber.TranslationKey = null;
            // 
            // txtBoxMeterSIM
            // 
            this.txtBoxMeterSIM.Location = new System.Drawing.Point(144, 21);
            this.txtBoxMeterSIM.MaxLength = 10;
            this.txtBoxMeterSIM.Name = "txtBoxMeterSIM";
            this.txtBoxMeterSIM.Size = new System.Drawing.Size(136, 31);
            this.txtBoxMeterSIM.TabIndex = 55;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.groupBox2.Controls.Add(this.btnAbort);
            this.groupBox2.Controls.Add(this.btnRead);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox2.Location = new System.Drawing.Point(2, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 48);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnAbort
            // 
            this.btnAbort.BackColor = System.Drawing.Color.White;
            this.btnAbort.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(180)))), ((int)(((byte)(50)))));
            this.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAbort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(0)))));
            this.btnAbort.Location = new System.Drawing.Point(101, 14);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 28);
            this.btnAbort.TabIndex = 8;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = false;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRead.FlatAppearance.BorderSize = 0;
            this.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRead.ForeColor = System.Drawing.Color.White;
            this.btnRead.Location = new System.Drawing.Point(10, 14);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 28);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "Read Data";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(242)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.Location = new System.Drawing.Point(199, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.lblInfo.Location = new System.Drawing.Point(-2, 312);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(389, 23);
            this.lblInfo.TabIndex = 54;
            this.lblInfo.Text = "* Name Plate and Instantaneous is default readout ";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(9, 469);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(915, 28);
            this.statusStrip.TabIndex = 55;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 14);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Tick += new System.EventHandler(this.progressBarTimer_Tick);
            // 
            // E650MeterDataReadout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(933, 506);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.GsmCommPanel);
            this.Controls.Add(this.lngGridViewReadControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "E650MeterDataReadout";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusMessage = "";
            this.Text = "Meter Readout";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.E650MeterDataReadout_FormClosing);
            this.Load += new System.EventHandler(this.E650MeterDataReadout_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpCommType.ResumeLayout(false);
            this.grpCommType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).EndInit();
            this.GsmCommPanel.ResumeLayout(false);
            this.GsmCommPanel.PerformLayout();
            this.grpSimNumber.ResumeLayout(false);
            this.grpSimNumber.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4;
        private CAB.UI.Controls.CABGridViewReadControl lngGridViewReadControl1;
        private System.Windows.Forms.GroupBox grpCommType;
        private System.Windows.Forms.RadioButton oneToManyGSM;
        private System.Windows.Forms.RadioButton oneToOneGSM;
        private System.Windows.Forms.DataGridView dgvMeterIdAndSim;
        private System.Windows.Forms.Panel GsmCommPanel;
        private System.Windows.Forms.GroupBox grpSimNumber;
        private CAB.UI.Controls.CABLabel lngSimNumber;
        private System.Windows.Forms.TextBox txtBoxMeterSIM;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox selectAll;
        private System.Windows.Forms.Label noMeterFoundStatus;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Timer progressBarTimer;
        private CAB.UI.Controls.CABLabel lngPort;
        private System.Windows.Forms.TextBox txtPort;

    }
}


