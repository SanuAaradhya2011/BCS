using CAB.Framework;
using System;
namespace CAB.UI
{
    partial class frmGSMGrouping
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
            this.grpSelection = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lstAvailable = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlArea = new System.Windows.Forms.Panel();
            this.lblDivision = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.lblCircle = new System.Windows.Forms.Label();
            this.cmbDivision = new System.Windows.Forms.ComboBox();
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.cmbCircle = new System.Windows.Forms.ComboBox();
            this.grpGroupDetails = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.grpMeterDetails = new System.Windows.Forms.GroupBox();
            this.pnlMeter = new System.Windows.Forms.Panel();
            this.txtMeterNumber = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblCommType = new System.Windows.Forms.Label();
            this.comboBoxCommType = new System.Windows.Forms.ComboBox();
            this.rdSelectAll = new System.Windows.Forms.RadioButton();
            this.rdAreaWise = new System.Windows.Forms.RadioButton();
            this.rdMeterWise = new System.Windows.Forms.RadioButton();
            this.grpSelection.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlArea.SuspendLayout();
            this.grpGroupDetails.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grpMeterDetails.SuspendLayout();
            this.pnlMeter.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSelection
            // 
            this.grpSelection.Controls.Add(this.panel5);
            this.grpSelection.Location = new System.Drawing.Point(20, 310);
            this.grpSelection.Name = "grpSelection";
            this.grpSelection.Size = new System.Drawing.Size(800, 390);
            this.grpSelection.TabIndex = 1;
            this.grpSelection.TabStop = false;
            this.grpSelection.Text = "  ✅  Meter Confirmation  ";
            this.grpSelection.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.grpSelection.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.grpSelection.Padding = new System.Windows.Forms.Padding(10);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.button1);
            this.panel5.Controls.Add(this.button3);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Controls.Add(this.lstSelected);
            this.panel5.Controls.Add(this.button2);
            this.panel5.Controls.Add(this.lstAvailable);
            this.panel5.Controls.Add(this.btnSave);
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Location = new System.Drawing.Point(14, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(772, 350);
            this.panel5.TabIndex = 0;
            this.panel5.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.label5.Location = new System.Drawing.Point(430, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 17);
            this.label5.TabIndex = 50;
            this.label5.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 49;
            this.label3.Text = "📋  Available Meters";
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(440, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 48;
            this.label2.Text = "Selected Meters";
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(340, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 32);
            this.button1.TabIndex = 46;
            this.button1.Text = "◀◀  All";
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button1.UseVisualStyleBackColor = false;
            this.button1.BackColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(195, 60, 60);
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(340, 95);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 32);
            this.button3.TabIndex = 44;
            this.button3.Text = "▶▶  All";
            this.button3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button3.UseVisualStyleBackColor = false;
            this.button3.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(340, 55);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 32);
            this.button4.TabIndex = 43;
            this.button4.Text = "  ▶  Add";
            this.button4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button4.UseVisualStyleBackColor = false;
            this.button4.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lstSelected
            // 
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.HorizontalScrollbar = true;
            this.lstSelected.Location = new System.Drawing.Point(440, 30);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(300, 264);
            this.lstSelected.TabIndex = 47;
            this.lstSelected.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.lstSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSelected.BackColor = System.Drawing.Color.White;
            this.lstSelected.SelectedIndexChanged += new System.EventHandler(this.lstSelected_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(340, 135);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 32);
            this.button2.TabIndex = 45;
            this.button2.Text = "◀  Remove";
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button2.UseVisualStyleBackColor = false;
            this.button2.BackColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(195, 60, 60);
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lstAvailable
            // 
            this.lstAvailable.FormattingEnabled = true;
            this.lstAvailable.HorizontalScrollbar = true;
            this.lstAvailable.Location = new System.Drawing.Point(30, 30);
            this.lstAvailable.Name = "lstAvailable";
            this.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailable.Size = new System.Drawing.Size(280, 264);
            this.lstAvailable.TabIndex = 42;
            this.lstAvailable.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.lstAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstAvailable.BackColor = System.Drawing.Color.White;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(530, 305);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 34);
            this.btnSave.TabIndex = 41;
            this.btnSave.Text = "✔  Save Group";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(16, 137, 62);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(14, 115, 52);
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(660, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 34);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlArea);
            this.groupBox2.Location = new System.Drawing.Point(20, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 148);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "  🌐  Geographic Area  ";
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            // 
            // pnlArea
            // 
            this.pnlArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlArea.Controls.Add(this.lblDivision);
            this.pnlArea.Controls.Add(this.lblRegion);
            this.pnlArea.Controls.Add(this.lblCircle);
            this.pnlArea.Controls.Add(this.cmbDivision);
            this.pnlArea.Controls.Add(this.cmbRegion);
            this.pnlArea.Controls.Add(this.cmbCircle);
            this.pnlArea.Location = new System.Drawing.Point(14, 28);
            this.pnlArea.Name = "pnlArea";
            this.pnlArea.Size = new System.Drawing.Size(396, 110);
            this.pnlArea.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            this.pnlArea.TabIndex = 0;
            // 
            // lblDivision
            // 
            this.lblDivision.AutoSize = true;
            this.lblDivision.Location = new System.Drawing.Point(12, 76);
            this.lblDivision.Name = "lblDivision";
            this.lblDivision.Size = new System.Drawing.Size(60, 17);
            this.lblDivision.TabIndex = 3;
            this.lblDivision.Text = "Division :";
            this.lblDivision.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDivision.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(12, 14);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(54, 17);
            this.lblRegion.TabIndex = 3;
            this.lblRegion.Text = "Region :";
            this.lblRegion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblRegion.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // lblCircle
            // 
            this.lblCircle.AutoSize = true;
            this.lblCircle.Location = new System.Drawing.Point(12, 46);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(46, 17);
            this.lblCircle.TabIndex = 2;
            this.lblCircle.Text = "Circle :";
            this.lblCircle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCircle.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // cmbDivision
            // 
            this.cmbDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDivision.FormattingEnabled = true;
            this.cmbDivision.Location = new System.Drawing.Point(80, 73);
            this.cmbDivision.Name = "cmbDivision";
            this.cmbDivision.Size = new System.Drawing.Size(300, 25);
            this.cmbDivision.TabIndex = 0;
            this.cmbDivision.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbDivision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDivision.BackColor = System.Drawing.Color.White;
            this.cmbDivision.SelectedIndexChanged += new System.EventHandler(this.cmbDivision_SelectedIndexChanged);
            // 
            // cmbRegion
            // 
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Location = new System.Drawing.Point(80, 11);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(300, 25);
            this.cmbRegion.TabIndex = 1;
            this.cmbRegion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbRegion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRegion.BackColor = System.Drawing.Color.White;
            this.cmbRegion.SelectedIndexChanged += new System.EventHandler(this.cmbRegion_SelectedIndexChanged);
            // 
            // cmbCircle
            // 
            this.cmbCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCircle.FormattingEnabled = true;
            this.cmbCircle.Location = new System.Drawing.Point(80, 42);
            this.cmbCircle.Name = "cmbCircle";
            this.cmbCircle.Size = new System.Drawing.Size(300, 25);
            this.cmbCircle.TabIndex = 0;
            this.cmbCircle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbCircle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCircle.BackColor = System.Drawing.Color.White;
            this.cmbCircle.SelectedIndexChanged += new System.EventHandler(this.cmbCircle_SelectedIndexChanged);
            // 
            // grpGroupDetails
            // 
            this.grpGroupDetails.Controls.Add(this.panel3);
            this.grpGroupDetails.Location = new System.Drawing.Point(20, 16);
            this.grpGroupDetails.Name = "grpGroupDetails";
            this.grpGroupDetails.Size = new System.Drawing.Size(420, 130);
            this.grpGroupDetails.TabIndex = 3;
            this.grpGroupDetails.TabStop = false;
            this.grpGroupDetails.Text = "  🎯  Group Identity  ";
            this.grpGroupDetails.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.grpGroupDetails.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.grpGroupDetails.Padding = new System.Windows.Forms.Padding(10);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.lblGroupName);
            this.panel3.Controls.Add(this.txtGroupName);
            this.panel3.Location = new System.Drawing.Point(14, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(396, 92);
            this.panel3.TabIndex = 1;
            this.panel3.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(0, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "*";
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(12, 21);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(90, 17);
            this.lblGroupName.TabIndex = 0;
            this.lblGroupName.Text = "🏷️  Group Name :";
            this.lblGroupName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGroupName.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(110, 18);
            this.txtGroupName.MaxLength = 20;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(270, 25);
            this.txtGroupName.TabIndex = 0;
            this.txtGroupName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtGroupName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGroupName.BackColor = System.Drawing.Color.White;
            // 
            // grpMeterDetails
            // 
            this.grpMeterDetails.Controls.Add(this.pnlMeter);
            this.grpMeterDetails.Location = new System.Drawing.Point(450, 155);
            this.grpMeterDetails.Name = "grpMeterDetails";
            this.grpMeterDetails.Size = new System.Drawing.Size(370, 148);
            this.grpMeterDetails.TabIndex = 4;
            this.grpMeterDetails.TabStop = false;
            this.grpMeterDetails.Text = "  🔍  Add Meter By ID  ";
            this.grpMeterDetails.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpMeterDetails.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            // 
            // pnlMeter
            // 
            this.pnlMeter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlMeter.Controls.Add(this.txtMeterNumber);
            this.pnlMeter.Controls.Add(this.btnAdd);
            this.pnlMeter.Controls.Add(this.label1);
            this.pnlMeter.Location = new System.Drawing.Point(14, 28);
            this.pnlMeter.Name = "pnlMeter";
            this.pnlMeter.Size = new System.Drawing.Size(342, 110);
            this.pnlMeter.TabIndex = 0;
            this.pnlMeter.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // txtMeterNumber
            // 
            this.txtMeterNumber.Location = new System.Drawing.Point(110, 30);
            this.txtMeterNumber.Name = "txtMeterNumber";
            this.txtMeterNumber.Size = new System.Drawing.Size(210, 25);
            this.txtMeterNumber.TabIndex = 3;
            this.txtMeterNumber.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMeterNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMeterNumber.BackColor = System.Drawing.Color.White;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(110, 65);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "➕  Add Meter";
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "📏  Meter ID :";
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel6);
            this.groupBox3.Location = new System.Drawing.Point(450, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(370, 130);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "  ⚙️  Configuration  ";
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.groupBox3.Padding = new System.Windows.Forms.Padding(10);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panel6.Controls.Add(this.lblCommType);
            this.panel6.Controls.Add(this.comboBoxCommType);
            this.panel6.Controls.Add(this.rdSelectAll);
            this.panel6.Controls.Add(this.rdAreaWise);
            this.panel6.Controls.Add(this.rdMeterWise);
            this.panel6.Location = new System.Drawing.Point(14, 28);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(342, 92);
            this.panel6.TabIndex = 1;
            this.panel6.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // lblCommType
            // 
            this.lblCommType.AutoSize = true;
            this.lblCommType.Location = new System.Drawing.Point(8, 10);
            this.lblCommType.Name = "lblCommType";
            this.lblCommType.Size = new System.Drawing.Size(130, 17);
            this.lblCommType.TabIndex = 4;
            this.lblCommType.Text = "📡  Communication Type :";
            this.lblCommType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCommType.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // comboBoxCommType
            // 
            this.comboBoxCommType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommType.FormattingEnabled = true;
            this.comboBoxCommType.Items.AddRange(new object[] {
            "GSM",
            "PSTN",
            "GPRS",
            "TCP",
            "FTP"
            });
            this.comboBoxCommType.Location = new System.Drawing.Point(175, 7);
            this.comboBoxCommType.Name = "comboBoxCommType";
            this.comboBoxCommType.Size = new System.Drawing.Size(150, 25);
            this.comboBoxCommType.TabIndex = 5;
            this.comboBoxCommType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxCommType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxCommType.BackColor = System.Drawing.Color.White;
            this.comboBoxCommType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommType_SelectedIndexChanged);
            // 
            // rdSelectAll
            // 
            this.rdSelectAll.AutoSize = true;
            this.rdSelectAll.Enabled = false;
            this.rdSelectAll.Location = new System.Drawing.Point(230, 50);
            this.rdSelectAll.Name = "rdSelectAll";
            this.rdSelectAll.Size = new System.Drawing.Size(80, 20);
            this.rdSelectAll.TabIndex = 2;
            this.rdSelectAll.TabStop = true;
            this.rdSelectAll.Text = "Select All";
            this.rdSelectAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.rdSelectAll.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.rdSelectAll.UseVisualStyleBackColor = true;
            this.rdSelectAll.Visible = false;
            this.rdSelectAll.CheckedChanged += new System.EventHandler(this.rdSelectAll_CheckedChanged);
            // 
            // rdAreaWise
            // 
            this.rdAreaWise.AutoSize = true;
            this.rdAreaWise.Location = new System.Drawing.Point(120, 50);
            this.rdAreaWise.Name = "rdAreaWise";
            this.rdAreaWise.Size = new System.Drawing.Size(90, 20);
            this.rdAreaWise.TabIndex = 1;
            this.rdAreaWise.TabStop = true;
            this.rdAreaWise.Text = "🌐  Area Wise";
            this.rdAreaWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.rdAreaWise.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.rdAreaWise.UseVisualStyleBackColor = true;
            this.rdAreaWise.CheckedChanged += new System.EventHandler(this.rdAreaWise_CheckedChanged);
            // 
            // rdMeterWise
            // 
            this.rdMeterWise.AutoSize = true;
            this.rdMeterWise.Location = new System.Drawing.Point(8, 50);
            this.rdMeterWise.Name = "rdMeterWise";
            this.rdMeterWise.Size = new System.Drawing.Size(95, 20);
            this.rdMeterWise.TabIndex = 0;
            this.rdMeterWise.TabStop = true;
            this.rdMeterWise.Text = "📟  Meter Wise";
            this.rdMeterWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.rdMeterWise.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.rdMeterWise.UseVisualStyleBackColor = true;
            this.rdMeterWise.CheckedChanged += new System.EventHandler(this.rdMeterwise_CheckedChanged);
            // 
            // frmGSMGrouping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(235, 240, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(840, 720);
            this.Controls.Add(this.grpMeterDetails);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpGroupDetails);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpSelection);
            this.Name = "frmGSMGrouping";
            this.StatusMessage = "";
            this.Text = "Create Meter Group";
            this.Load += new System.EventHandler(this.frmGSMGrouping_Load);
            this.Activated += new System.EventHandler(this.frmGSMGrouping_Activated);
            this.grpSelection.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.pnlArea.ResumeLayout(false);
            this.pnlArea.PerformLayout();
            this.grpGroupDetails.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grpMeterDetails.ResumeLayout(false);
            this.pnlMeter.ResumeLayout(false);
            this.pnlMeter.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSelection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel pnlArea;
        private System.Windows.Forms.ComboBox cmbDivision;
        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.ComboBox cmbCircle;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.Label lblCircle;
        private System.Windows.Forms.Label lblDivision;
        private System.Windows.Forms.GroupBox grpGroupDetails;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.GroupBox grpMeterDetails;
        private System.Windows.Forms.Panel pnlMeter;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lstAvailable;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rdSelectAll;
        private System.Windows.Forms.RadioButton rdAreaWise;
        private System.Windows.Forms.RadioButton rdMeterWise;
        private System.Windows.Forms.TextBox txtMeterNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCommType;
        private System.Windows.Forms.ComboBox comboBoxCommType;
    }
}



