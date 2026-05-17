namespace CAB.UI
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
            this.tpFraudEnergy = new System.Windows.Forms.TabPage();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_ReadReverseEnergy = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.grpFraudEnergy = new System.Windows.Forms.GroupBox();
            this.txtFraudApparent = new System.Windows.Forms.TextBox();
            this.lblFraudApparent = new System.Windows.Forms.Label();
            this.txtFraudReactiveLead = new System.Windows.Forms.TextBox();
            this.txtFraudReactiveLag = new System.Windows.Forms.TextBox();
            this.lblFraudReactiveLag = new System.Windows.Forms.Label();
            this.lblFraudReactiveLead = new System.Windows.Forms.Label();
            this.lblFraudActive = new System.Windows.Forms.Label();
            this.txtFraudActive = new System.Windows.Forms.TextBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_RevKvah = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_RevKwh = new System.Windows.Forms.TextBox();
            this.lbkNoDataFound = new System.Windows.Forms.Label();
            this.tpPhasor = new System.Windows.Forms.TabPage();
            this.lngPhasorDiagram = new CAB.UI.Controls.PhasorDiagram();
            this.lngPgrid = new CAB.UI.Controls.CABGridControl();
            this.btnStopPhasor = new System.Windows.Forms.Button();
            this.btnCancelPhasor = new System.Windows.Forms.Button();
            this.btnReadPhasor = new System.Windows.Forms.Button();
            this.tpTransaction = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lgcRTCUpdate = new System.Windows.Forms.TabControl();
            this.tpProgrammingUpdate = new System.Windows.Forms.TabPage();
            this.lblProgrammingCounter = new CAB.UI.Controls.CABLabel();
            this.lgcProgrammingUpdate = new CAB.UI.Controls.CABGridControl();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.tpRTCUpdate = new System.Windows.Forms.TabPage();
            this.lblRTCUpdate = new CAB.UI.Controls.CABLabel();
            this.lNGGRTCUpdate = new CAB.UI.Controls.CABGridControl();
            this.lngLabel3 = new CAB.UI.Controls.CABLabel();
            this.tpTamperStatus = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lgcTamper = new CAB.UI.Controls.CABGridControl();
            this.tpReadData = new System.Windows.Forms.TabPage();
            this.ReadOut_Grpread = new System.Windows.Forms.GroupBox();
            this.btnAbort = new System.Windows.Forms.Button();
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpLoadSurvey = new System.Windows.Forms.GroupBox();
            this.btn_Noofdays = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmoDTMdays = new System.Windows.Forms.ComboBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpFraudEnergy.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.grpFraudEnergy.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tpPhasor.SuspendLayout();
            this.tpTransaction.SuspendLayout();
            this.lgcRTCUpdate.SuspendLayout();
            this.tpProgrammingUpdate.SuspendLayout();
            this.tpRTCUpdate.SuspendLayout();
            this.tpTamperStatus.SuspendLayout();
            this.tpReadData.SuspendLayout();
            this.ReadOut_Grpread.SuspendLayout();
            this.grpReadoptions.SuspendLayout();
            this.panelPartialData.SuspendLayout();
            this.grpLoadSurvey.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpFraudEnergy
            // 
            this.tpFraudEnergy.AutoScroll = true;
            this.tpFraudEnergy.Controls.Add(this.btn_cancel);
            this.tpFraudEnergy.Controls.Add(this.btn_ReadReverseEnergy);
            this.tpFraudEnergy.Controls.Add(this.tabControl3);
            this.tpFraudEnergy.Location = new System.Drawing.Point(4, 22);
            this.tpFraudEnergy.Name = "tpFraudEnergy";
            this.tpFraudEnergy.Size = new System.Drawing.Size(890, 439);
            this.tpFraudEnergy.TabIndex = 4;
            this.tpFraudEnergy.Text = "Fraud Energy";
            this.tpFraudEnergy.UseVisualStyleBackColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(442, 352);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(83, 29);
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ReadReverseEnergy
            // 
            this.btn_ReadReverseEnergy.Location = new System.Drawing.Point(335, 352);
            this.btn_ReadReverseEnergy.Name = "btn_ReadReverseEnergy";
            this.btn_ReadReverseEnergy.Size = new System.Drawing.Size(83, 29);
            this.btn_ReadReverseEnergy.TabIndex = 7;
            this.btn_ReadReverseEnergy.Text = "Read Data";
            this.btn_ReadReverseEnergy.UseVisualStyleBackColor = true;
            this.btn_ReadReverseEnergy.Click += new System.EventHandler(this.btn_ReadReverseEnergy_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage9);
            this.tabControl3.Controls.Add(this.tabPage10);
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(522, 343);
            this.tabControl3.TabIndex = 6;
            // 
            // tabPage9
            // 
            this.tabPage9.AccessibleName = "rb_MgtInfluence";
            this.tabPage9.AutoScroll = true;
            this.tabPage9.Controls.Add(this.grpFraudEnergy);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(514, 317);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "Magnetic influence Energy";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // grpFraudEnergy
            // 
            this.grpFraudEnergy.Controls.Add(this.txtFraudApparent);
            this.grpFraudEnergy.Controls.Add(this.lblFraudApparent);
            this.grpFraudEnergy.Controls.Add(this.txtFraudReactiveLead);
            this.grpFraudEnergy.Controls.Add(this.txtFraudReactiveLag);
            this.grpFraudEnergy.Controls.Add(this.lblFraudReactiveLag);
            this.grpFraudEnergy.Controls.Add(this.lblFraudReactiveLead);
            this.grpFraudEnergy.Controls.Add(this.lblFraudActive);
            this.grpFraudEnergy.Controls.Add(this.txtFraudActive);
            this.grpFraudEnergy.ForeColor = System.Drawing.Color.Black;
            this.grpFraudEnergy.Location = new System.Drawing.Point(48, 37);
            this.grpFraudEnergy.Name = "grpFraudEnergy";
            this.grpFraudEnergy.Size = new System.Drawing.Size(415, 198);
            this.grpFraudEnergy.TabIndex = 2;
            this.grpFraudEnergy.TabStop = false;
            this.grpFraudEnergy.Text = "Magnetic Influence Energy";
            // 
            // txtFraudApparent
            // 
            this.txtFraudApparent.Location = new System.Drawing.Point(244, 137);
            this.txtFraudApparent.Name = "txtFraudApparent";
            this.txtFraudApparent.ReadOnly = true;
            this.txtFraudApparent.Size = new System.Drawing.Size(100, 20);
            this.txtFraudApparent.TabIndex = 9;
            // 
            // lblFraudApparent
            // 
            this.lblFraudApparent.AutoSize = true;
            this.lblFraudApparent.Location = new System.Drawing.Point(67, 142);
            this.lblFraudApparent.Name = "lblFraudApparent";
            this.lblFraudApparent.Size = new System.Drawing.Size(121, 13);
            this.lblFraudApparent.TabIndex = 8;
            this.lblFraudApparent.Text = "Apparent Energy (kVAh)";
            // 
            // txtFraudReactiveLead
            // 
            this.txtFraudReactiveLead.Location = new System.Drawing.Point(244, 104);
            this.txtFraudReactiveLead.Name = "txtFraudReactiveLead";
            this.txtFraudReactiveLead.ReadOnly = true;
            this.txtFraudReactiveLead.Size = new System.Drawing.Size(100, 20);
            this.txtFraudReactiveLead.TabIndex = 6;
            // 
            // txtFraudReactiveLag
            // 
            this.txtFraudReactiveLag.Location = new System.Drawing.Point(244, 70);
            this.txtFraudReactiveLag.Name = "txtFraudReactiveLag";
            this.txtFraudReactiveLag.ReadOnly = true;
            this.txtFraudReactiveLag.Size = new System.Drawing.Size(100, 20);
            this.txtFraudReactiveLag.TabIndex = 5;
            // 
            // lblFraudReactiveLag
            // 
            this.lblFraudReactiveLag.AutoSize = true;
            this.lblFraudReactiveLag.Location = new System.Drawing.Point(66, 75);
            this.lblFraudReactiveLag.Name = "lblFraudReactiveLag";
            this.lblFraudReactiveLag.Size = new System.Drawing.Size(114, 13);
            this.lblFraudReactiveLag.TabIndex = 3;
            this.lblFraudReactiveLag.Text = "Reactive Lag (kVARh)";
            // 
            // lblFraudReactiveLead
            // 
            this.lblFraudReactiveLead.AutoSize = true;
            this.lblFraudReactiveLead.Location = new System.Drawing.Point(66, 108);
            this.lblFraudReactiveLead.Name = "lblFraudReactiveLead";
            this.lblFraudReactiveLead.Size = new System.Drawing.Size(120, 13);
            this.lblFraudReactiveLead.TabIndex = 2;
            this.lblFraudReactiveLead.Text = "Reactive Lead (kVARh)";
            // 
            // lblFraudActive
            // 
            this.lblFraudActive.AutoSize = true;
            this.lblFraudActive.Location = new System.Drawing.Point(67, 42);
            this.lblFraudActive.Name = "lblFraudActive";
            this.lblFraudActive.Size = new System.Drawing.Size(105, 13);
            this.lblFraudActive.TabIndex = 1;
            this.lblFraudActive.Text = "Active Energy (kWh)";
            // 
            // txtFraudActive
            // 
            this.txtFraudActive.Location = new System.Drawing.Point(244, 37);
            this.txtFraudActive.Name = "txtFraudActive";
            this.txtFraudActive.ReadOnly = true;
            this.txtFraudActive.Size = new System.Drawing.Size(100, 20);
            this.txtFraudActive.TabIndex = 1;
            // 
            // tabPage10
            // 
            this.tabPage10.AccessibleName = "tb_ReverseEnergy";
            this.tabPage10.Controls.Add(this.groupBox2);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(514, 317);
            this.tabPage10.TabIndex = 1;
            this.tabPage10.Text = "Reverse Energy";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_RevKvah);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txt_RevKwh);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(20, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 123);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reverse Energy";
            // 
            // txt_RevKvah
            // 
            this.txt_RevKvah.Location = new System.Drawing.Point(244, 76);
            this.txt_RevKvah.Name = "txt_RevKvah";
            this.txt_RevKvah.ReadOnly = true;
            this.txt_RevKvah.Size = new System.Drawing.Size(100, 20);
            this.txt_RevKvah.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(248, 152);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(248, 126);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Reverse Cumulative (kVAh)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fraud Reactive Lag (kVARh)";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fraud Reactive Lead (kVARh)";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(99, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Reverse Cumulative (kWh)";
            // 
            // txt_RevKwh
            // 
            this.txt_RevKwh.Location = new System.Drawing.Point(244, 33);
            this.txt_RevKwh.Name = "txt_RevKwh";
            this.txt_RevKwh.ReadOnly = true;
            this.txt_RevKwh.Size = new System.Drawing.Size(100, 20);
            this.txt_RevKwh.TabIndex = 1;
            // 
            // lbkNoDataFound
            // 
            this.lbkNoDataFound.AutoSize = true;
            this.lbkNoDataFound.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbkNoDataFound.ForeColor = System.Drawing.Color.Red;
            this.lbkNoDataFound.Location = new System.Drawing.Point(36, 29);
            this.lbkNoDataFound.Name = "lbkNoDataFound";
            this.lbkNoDataFound.Size = new System.Drawing.Size(48, 14);
            this.lbkNoDataFound.TabIndex = 6;
            this.lbkNoDataFound.Text = "label8";
            this.lbkNoDataFound.Visible = false;
            // 
            // tpPhasor
            // 
            this.tpPhasor.AutoScroll = true;
            this.tpPhasor.Controls.Add(this.lngPhasorDiagram);
            this.tpPhasor.Controls.Add(this.lngPgrid);
            this.tpPhasor.Controls.Add(this.btnStopPhasor);
            this.tpPhasor.Controls.Add(this.btnCancelPhasor);
            this.tpPhasor.Controls.Add(this.btnReadPhasor);
            this.tpPhasor.Controls.Add(this.lbkNoDataFound);
            this.tpPhasor.Location = new System.Drawing.Point(4, 22);
            this.tpPhasor.Name = "tpPhasor";
            this.tpPhasor.Size = new System.Drawing.Size(890, 439);
            this.tpPhasor.TabIndex = 3;
            this.tpPhasor.Text = "Phasor";
            this.tpPhasor.UseVisualStyleBackColor = true;
            // 
            // lngPhasorDiagram
            // 
            this.lngPhasorDiagram.BackColor = System.Drawing.Color.Transparent;
            this.lngPhasorDiagram.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lngPhasorDiagram.Location = new System.Drawing.Point(6, 9);
            this.lngPhasorDiagram.Margin = new System.Windows.Forms.Padding(4);
            this.lngPhasorDiagram.Name = "lngPhasorDiagram";
            this.lngPhasorDiagram.PhasorData = null;
            this.lngPhasorDiagram.PhasorDataset = null;
            this.lngPhasorDiagram.Size = new System.Drawing.Size(468, 425);
            this.lngPhasorDiagram.TabIndex = 5;
            // 
            // lngPgrid
            // 
            this.lngPgrid.AutoScroll = true;
            this.lngPgrid.Data = null;
            this.lngPgrid.HiddenColumn = null;
            this.lngPgrid.HiddenColumn2 = null;
            this.lngPgrid.HiddenColumn3 = null;
            this.lngPgrid.HiddenColumn4 = null;
            this.lngPgrid.HiddenColumns = null;
            this.lngPgrid.IsEqual = true;
            this.lngPgrid.IsFullRow = false;
            this.lngPgrid.IsSorting = false;
            this.lngPgrid.Location = new System.Drawing.Point(532, 4);
            this.lngPgrid.Margin = new System.Windows.Forms.Padding(4);
            this.lngPgrid.Name = "lngPgrid";
            this.lngPgrid.SelectedIndex = 0;
            this.lngPgrid.Size = new System.Drawing.Size(326, 356);
            this.lngPgrid.TabIndex = 8;
            this.lngPgrid.ValueColumn = null;
            this.lngPgrid.ValueColumn2 = null;
            // 
            // btnStopPhasor
            // 
            this.btnStopPhasor.Location = new System.Drawing.Point(687, 367);
            this.btnStopPhasor.Name = "btnStopPhasor";
            this.btnStopPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnStopPhasor.TabIndex = 6;
            this.btnStopPhasor.Text = "Hold";
            this.btnStopPhasor.UseVisualStyleBackColor = true;
            this.btnStopPhasor.Click += new System.EventHandler(this.btnStopPhasor_Click);
            // 
            // btnCancelPhasor
            // 
            this.btnCancelPhasor.Location = new System.Drawing.Point(776, 367);
            this.btnCancelPhasor.Name = "btnCancelPhasor";
            this.btnCancelPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnCancelPhasor.TabIndex = 7;
            this.btnCancelPhasor.Text = "Cancel";
            this.btnCancelPhasor.UseVisualStyleBackColor = true;
            this.btnCancelPhasor.Click += new System.EventHandler(this.btnCancelPhasor_Click);
            // 
            // btnReadPhasor
            // 
            this.btnReadPhasor.Location = new System.Drawing.Point(598, 367);
            this.btnReadPhasor.Name = "btnReadPhasor";
            this.btnReadPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnReadPhasor.TabIndex = 5;
            this.btnReadPhasor.Text = "Read Data";
            this.btnReadPhasor.UseVisualStyleBackColor = true;
            this.btnReadPhasor.Click += new System.EventHandler(this.btnReadPhasor_Click);
            // 
            // tpTransaction
            // 
            this.tpTransaction.AutoScroll = true;
            this.tpTransaction.Controls.Add(this.button3);
            this.tpTransaction.Controls.Add(this.button4);
            this.tpTransaction.Controls.Add(this.lgcRTCUpdate);
            this.tpTransaction.Location = new System.Drawing.Point(4, 22);
            this.tpTransaction.Name = "tpTransaction";
            this.tpTransaction.Size = new System.Drawing.Size(890, 439);
            this.tpTransaction.TabIndex = 2;
            this.tpTransaction.Text = "Transactions";
            this.tpTransaction.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(782, 364);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(74, 29);
            this.button3.TabIndex = 28;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(692, 364);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(74, 29);
            this.button4.TabIndex = 27;
            this.button4.Text = "Read Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lgcRTCUpdate
            // 
            this.lgcRTCUpdate.Controls.Add(this.tpProgrammingUpdate);
            this.lgcRTCUpdate.Controls.Add(this.tpRTCUpdate);
            this.lgcRTCUpdate.Location = new System.Drawing.Point(3, 3);
            this.lgcRTCUpdate.Name = "lgcRTCUpdate";
            this.lgcRTCUpdate.SelectedIndex = 0;
            this.lgcRTCUpdate.Size = new System.Drawing.Size(857, 355);
            this.lgcRTCUpdate.TabIndex = 0;
            // 
            // tpProgrammingUpdate
            // 
            this.tpProgrammingUpdate.AutoScroll = true;
            this.tpProgrammingUpdate.AutoScrollMargin = new System.Drawing.Size(100, 0);
            this.tpProgrammingUpdate.Controls.Add(this.lblProgrammingCounter);
            this.tpProgrammingUpdate.Controls.Add(this.lgcProgrammingUpdate);
            this.tpProgrammingUpdate.Controls.Add(this.lngLabel1);
            this.tpProgrammingUpdate.Location = new System.Drawing.Point(4, 22);
            this.tpProgrammingUpdate.Name = "tpProgrammingUpdate";
            this.tpProgrammingUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tpProgrammingUpdate.Size = new System.Drawing.Size(849, 329);
            this.tpProgrammingUpdate.TabIndex = 0;
            this.tpProgrammingUpdate.Text = "Programming Update";
            this.tpProgrammingUpdate.UseVisualStyleBackColor = true;
            // 
            // lblProgrammingCounter
            // 
            this.lblProgrammingCounter.AutoSize = true;
            this.lblProgrammingCounter.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgrammingCounter.Location = new System.Drawing.Point(448, 18);
            this.lblProgrammingCounter.Name = "lblProgrammingCounter";
            this.lblProgrammingCounter.Size = new System.Drawing.Size(15, 16);
            this.lblProgrammingCounter.TabIndex = 3;
            this.lblProgrammingCounter.Text = "0";
            this.lblProgrammingCounter.TranslationKey = null;
            // 
            // lgcProgrammingUpdate
            // 
            this.lgcProgrammingUpdate.AutoScroll = true;
            this.lgcProgrammingUpdate.Data = null;
            this.lgcProgrammingUpdate.HiddenColumn = null;
            this.lgcProgrammingUpdate.HiddenColumn2 = null;
            this.lgcProgrammingUpdate.HiddenColumn3 = null;
            this.lgcProgrammingUpdate.HiddenColumn4 = null;
            this.lgcProgrammingUpdate.HiddenColumns = null;
            this.lgcProgrammingUpdate.IsEqual = true;
            this.lgcProgrammingUpdate.IsFullRow = false;
            this.lgcProgrammingUpdate.IsSorting = false;
            this.lgcProgrammingUpdate.Location = new System.Drawing.Point(6, 45);
            this.lgcProgrammingUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.lgcProgrammingUpdate.Name = "lgcProgrammingUpdate";
            this.lgcProgrammingUpdate.SelectedIndex = 0;
            this.lgcProgrammingUpdate.Size = new System.Drawing.Size(2240, 278);
            this.lgcProgrammingUpdate.TabIndex = 2;
            this.lgcProgrammingUpdate.ValueColumn = null;
            this.lgcProgrammingUpdate.ValueColumn2 = null;
            this.lgcProgrammingUpdate.Load += new System.EventHandler(this.lgcProgrammingUpdate_Load);
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel1.Location = new System.Drawing.Point(229, 17);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(213, 16);
            this.lngLabel1.TabIndex = 1;
            this.lngLabel1.Text = "Number of programming Update";
            this.lngLabel1.TranslationKey = null;
            // 
            // tpRTCUpdate
            // 
            this.tpRTCUpdate.AutoScroll = true;
            this.tpRTCUpdate.Controls.Add(this.lblRTCUpdate);
            this.tpRTCUpdate.Controls.Add(this.lNGGRTCUpdate);
            this.tpRTCUpdate.Controls.Add(this.lngLabel3);
            this.tpRTCUpdate.Location = new System.Drawing.Point(4, 22);
            this.tpRTCUpdate.Name = "tpRTCUpdate";
            this.tpRTCUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tpRTCUpdate.Size = new System.Drawing.Size(849, 329);
            this.tpRTCUpdate.TabIndex = 1;
            this.tpRTCUpdate.Text = "RTC Update";
            this.tpRTCUpdate.UseVisualStyleBackColor = true;
            // 
            // lblRTCUpdate
            // 
            this.lblRTCUpdate.AutoSize = true;
            this.lblRTCUpdate.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRTCUpdate.Location = new System.Drawing.Point(437, 11);
            this.lblRTCUpdate.Name = "lblRTCUpdate";
            this.lblRTCUpdate.Size = new System.Drawing.Size(15, 16);
            this.lblRTCUpdate.TabIndex = 6;
            this.lblRTCUpdate.Text = "0";
            this.lblRTCUpdate.TranslationKey = null;
            // 
            // lNGGRTCUpdate
            // 
            this.lNGGRTCUpdate.AutoScroll = true;
            this.lNGGRTCUpdate.Data = null;
            this.lNGGRTCUpdate.HiddenColumn = null;
            this.lNGGRTCUpdate.HiddenColumn2 = null;
            this.lNGGRTCUpdate.HiddenColumn3 = null;
            this.lNGGRTCUpdate.HiddenColumn4 = null;
            this.lNGGRTCUpdate.HiddenColumns = null;
            this.lNGGRTCUpdate.IsEqual = true;
            this.lNGGRTCUpdate.IsFullRow = false;
            this.lNGGRTCUpdate.IsSorting = false;
            this.lNGGRTCUpdate.Location = new System.Drawing.Point(6, 42);
            this.lNGGRTCUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.lNGGRTCUpdate.Name = "lNGGRTCUpdate";
            this.lNGGRTCUpdate.SelectedIndex = 0;
            this.lNGGRTCUpdate.Size = new System.Drawing.Size(837, 276);
            this.lNGGRTCUpdate.TabIndex = 5;
            this.lNGGRTCUpdate.ValueColumn = null;
            this.lNGGRTCUpdate.ValueColumn2 = null;
            // 
            // lngLabel3
            // 
            this.lngLabel3.AutoSize = true;
            this.lngLabel3.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel3.Location = new System.Drawing.Point(266, 11);
            this.lngLabel3.Name = "lngLabel3";
            this.lngLabel3.Size = new System.Drawing.Size(153, 16);
            this.lngLabel3.TabIndex = 4;
            this.lngLabel3.Text = "Number of RTC Update";
            this.lngLabel3.TranslationKey = null;
            // 
            // tpTamperStatus
            // 
            this.tpTamperStatus.AutoScroll = true;
            this.tpTamperStatus.Controls.Add(this.button1);
            this.tpTamperStatus.Controls.Add(this.button2);
            this.tpTamperStatus.Controls.Add(this.lgcTamper);
            this.tpTamperStatus.Location = new System.Drawing.Point(4, 22);
            this.tpTamperStatus.Name = "tpTamperStatus";
            this.tpTamperStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tpTamperStatus.Size = new System.Drawing.Size(890, 439);
            this.tpTamperStatus.TabIndex = 1;
            this.tpTamperStatus.Text = "Tamper Status";
            this.tpTamperStatus.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(492, 364);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 29);
            this.button1.TabIndex = 26;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(412, 364);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 29);
            this.button2.TabIndex = 25;
            this.button2.Text = "Read Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lgcTamper
            // 
            this.lgcTamper.AutoScroll = true;
            this.lgcTamper.Data = null;
            this.lgcTamper.HiddenColumn = null;
            this.lgcTamper.HiddenColumn2 = null;
            this.lgcTamper.HiddenColumn3 = null;
            this.lgcTamper.HiddenColumn4 = null;
            this.lgcTamper.HiddenColumns = null;
            this.lgcTamper.IsEqual = true;
            this.lgcTamper.IsFullRow = false;
            this.lgcTamper.IsSorting = false;
            this.lgcTamper.Location = new System.Drawing.Point(7, 7);
            this.lgcTamper.Margin = new System.Windows.Forms.Padding(4);
            this.lgcTamper.Name = "lgcTamper";
            this.lgcTamper.SelectedIndex = 0;
            this.lgcTamper.Size = new System.Drawing.Size(559, 353);
            this.lgcTamper.TabIndex = 0;
            this.lgcTamper.ValueColumn = null;
            this.lgcTamper.ValueColumn2 = null;
            // 
            // tpReadData
            // 
            this.tpReadData.AutoScroll = true;
            this.tpReadData.Controls.Add(this.ReadOut_Grpread);
            this.tpReadData.Location = new System.Drawing.Point(4, 22);
            this.tpReadData.Name = "tpReadData";
            this.tpReadData.Padding = new System.Windows.Forms.Padding(3);
            this.tpReadData.Size = new System.Drawing.Size(890, 439);
            this.tpReadData.TabIndex = 0;
            this.tpReadData.Text = "Read Data";
            this.tpReadData.UseVisualStyleBackColor = true;
            // 
            // ReadOut_Grpread
            // 
            this.ReadOut_Grpread.Controls.Add(this.btnAbort);
            this.ReadOut_Grpread.Controls.Add(this.grpReadoptions);
            this.ReadOut_Grpread.Controls.Add(this.btnCancel);
            this.ReadOut_Grpread.Controls.Add(this.grpLoadSurvey);
            this.ReadOut_Grpread.Controls.Add(this.btnRead);
            this.ReadOut_Grpread.Location = new System.Drawing.Point(6, 6);
            this.ReadOut_Grpread.Name = "ReadOut_Grpread";
            this.ReadOut_Grpread.Size = new System.Drawing.Size(537, 290);
            this.ReadOut_Grpread.TabIndex = 20;
            this.ReadOut_Grpread.TabStop = false;
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(368, 219);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(74, 29);
            this.btnAbort.TabIndex = 25;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // grpReadoptions
            // 
            this.grpReadoptions.Controls.Add(this.panelPartialData);
            this.grpReadoptions.Controls.Add(this.rbtnPartial);
            this.grpReadoptions.Controls.Add(this.rbtnAll);
            this.grpReadoptions.Location = new System.Drawing.Point(10, 20);
            this.grpReadoptions.Name = "grpReadoptions";
            this.grpReadoptions.Size = new System.Drawing.Size(176, 245);
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
            this.panelPartialData.Location = new System.Drawing.Point(12, 42);
            this.panelPartialData.Name = "panelPartialData";
            this.panelPartialData.Size = new System.Drawing.Size(152, 197);
            this.panelPartialData.TabIndex = 2;
            // 
            // chkMeterConfigurations
            // 
            this.chkMeterConfigurations.AutoSize = true;
            this.chkMeterConfigurations.Location = new System.Drawing.Point(7, 176);
            this.chkMeterConfigurations.Name = "chkMeterConfigurations";
            this.chkMeterConfigurations.Size = new System.Drawing.Size(123, 17);
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
            this.chkDTMDaily.Size = new System.Drawing.Size(108, 17);
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
            this.chkFraudEnergy.Size = new System.Drawing.Size(92, 17);
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
            this.chkPhasor.Size = new System.Drawing.Size(59, 17);
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
            this.chkTransaction.Size = new System.Drawing.Size(82, 17);
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
            this.chkLoadSurvey.Size = new System.Drawing.Size(84, 17);
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
            this.chkTamper.Size = new System.Drawing.Size(62, 17);
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
            this.chkGeneral.Size = new System.Drawing.Size(138, 17);
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
            this.rbtnPartial.Size = new System.Drawing.Size(78, 17);
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
            this.rbtnAll.Size = new System.Drawing.Size(60, 17);
            this.rbtnAll.TabIndex = 0;
            this.rbtnAll.TabStop = true;
            this.rbtnAll.Text = "All data";
            this.rbtnAll.UseVisualStyleBackColor = true;
            this.rbtnAll.CheckedChanged += new System.EventHandler(this.rbtnAll_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(446, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 29);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.btn_Noofdays.Location = new System.Drawing.Point(237, 24);
            this.btn_Noofdays.Name = "btn_Noofdays";
            this.btn_Noofdays.Size = new System.Drawing.Size(54, 28);
            this.btn_Noofdays.TabIndex = 32;
            this.btn_Noofdays.Text = "Select";
            this.btn_Noofdays.UseVisualStyleBackColor = true;
            this.btn_Noofdays.Visible = false;
            this.btn_Noofdays.Click += new System.EventHandler(this.btn_Noofdays_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
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
            this.cmoDTMdays.Size = new System.Drawing.Size(47, 21);
            this.cmoDTMdays.TabIndex = 8;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(291, 219);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(74, 29);
            this.btnRead.TabIndex = 23;
            this.btnRead.Text = "Read Data";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpReadData);
            this.tabControl1.Controls.Add(this.tpTamperStatus);
            this.tabControl1.Controls.Add(this.tpTransaction);
            this.tabControl1.Controls.Add(this.tpPhasor);
            this.tabControl1.Controls.Add(this.tpFraudEnergy);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(898, 465);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MeterDataReadoutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(910, 560);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MeterDataReadoutForm";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusMessage = "";
            this.Text = "MeterDataReadoutForm";
            this.Load += new System.EventHandler(this.MeterDataReadoutForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MeterDataReadoutForm_FormClosing);
            this.tpFraudEnergy.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.grpFraudEnergy.ResumeLayout(false);
            this.grpFraudEnergy.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tpPhasor.ResumeLayout(false);
            this.tpPhasor.PerformLayout();
            this.tpTransaction.ResumeLayout(false);
            this.lgcRTCUpdate.ResumeLayout(false);
            this.tpProgrammingUpdate.ResumeLayout(false);
            this.tpProgrammingUpdate.PerformLayout();
            this.tpRTCUpdate.ResumeLayout(false);
            this.tpRTCUpdate.PerformLayout();
            this.tpTamperStatus.ResumeLayout(false);
            this.tpReadData.ResumeLayout(false);
            this.ReadOut_Grpread.ResumeLayout(false);
            this.grpReadoptions.ResumeLayout(false);
            this.grpReadoptions.PerformLayout();
            this.panelPartialData.ResumeLayout(false);
            this.panelPartialData.PerformLayout();
            this.grpLoadSurvey.ResumeLayout(false);
            this.grpLoadSurvey.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tpFraudEnergy;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ReadReverseEnergy;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.GroupBox grpFraudEnergy;
        private System.Windows.Forms.TextBox txtFraudApparent;
        private System.Windows.Forms.Label lblFraudApparent;
        private System.Windows.Forms.TextBox txtFraudReactiveLead;
        private System.Windows.Forms.TextBox txtFraudReactiveLag;
        private System.Windows.Forms.Label lblFraudReactiveLag;
        private System.Windows.Forms.Label lblFraudReactiveLead;
        private System.Windows.Forms.Label lblFraudActive;
        private System.Windows.Forms.TextBox txtFraudActive;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_RevKvah;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_RevKwh;
        private System.Windows.Forms.TabPage tpPhasor;
        private CAB.UI.Controls.CABGridControl lngPgrid;
        private System.Windows.Forms.Button btnStopPhasor;
        private System.Windows.Forms.Button btnCancelPhasor;
        private System.Windows.Forms.Button btnReadPhasor;
        private System.Windows.Forms.TabPage tpTransaction;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabControl lgcRTCUpdate;
        private System.Windows.Forms.TabPage tpProgrammingUpdate;
        private CAB.UI.Controls.CABLabel lblProgrammingCounter;
        private CAB.UI.Controls.CABGridControl lgcProgrammingUpdate;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.TabPage tpRTCUpdate;
        private CAB.UI.Controls.CABLabel lblRTCUpdate;
        private CAB.UI.Controls.CABGridControl lNGGRTCUpdate;
        private CAB.UI.Controls.CABLabel lngLabel3;
        private System.Windows.Forms.TabPage tpTamperStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private CAB.UI.Controls.CABGridControl lgcTamper;
        private System.Windows.Forms.TabPage tpReadData;
        private System.Windows.Forms.GroupBox ReadOut_Grpread;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.GroupBox grpReadoptions;
        private System.Windows.Forms.Panel panelPartialData;
        private System.Windows.Forms.CheckBox chkDTMDaily;
        private System.Windows.Forms.CheckBox chkFraudEnergy;
        private System.Windows.Forms.CheckBox chkPhasor;
        private System.Windows.Forms.CheckBox chkTransaction;
        private System.Windows.Forms.CheckBox chkLoadSurvey;
        private System.Windows.Forms.CheckBox chkTamper;
        private System.Windows.Forms.CheckBox chkGeneral;
        private System.Windows.Forms.RadioButton rbtnPartial;
        private System.Windows.Forms.RadioButton rbtnAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpLoadSurvey;
        private System.Windows.Forms.Button btn_Noofdays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmoDTMdays;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox chkMeterConfigurations;
        private System.Windows.Forms.Label lbkNoDataFound;
        private CAB.UI.Controls.PhasorDiagram lngPhasorDiagram;



    }
}
