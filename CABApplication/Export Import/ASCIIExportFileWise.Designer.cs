namespace CAB.UI
{
    partial class ASCIIExportFileWise
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
            this.groupBoxExportASCIIData = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.rbtnMeterWise = new System.Windows.Forms.RadioButton();
            this.rbtnFileWise = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExportData = new System.Windows.Forms.Button();
            this.cboSettings = new System.Windows.Forms.ComboBox();
            this.lblFileFormat = new System.Windows.Forms.Label();
            this.dvgFileWiseExport = new System.Windows.Forms.DataGridView();
            this.dgvMeterWiseExport = new System.Windows.Forms.DataGridView();
            this.rdHTCT = new System.Windows.Forms.RadioButton();
            this.rdLTCT = new System.Windows.Forms.RadioButton();
            this.gbExportType = new System.Windows.Forms.GroupBox();
            this.gbMeterType = new System.Windows.Forms.GroupBox();
            this.groupBoxExportASCIIData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgFileWiseExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterWiseExport)).BeginInit();
            this.gbExportType.SuspendLayout();
            this.gbMeterType.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxExportASCIIData
            // 
            this.groupBoxExportASCIIData.Controls.Add(this.gbMeterType);
            this.groupBoxExportASCIIData.Controls.Add(this.gbExportType);
            this.groupBoxExportASCIIData.Controls.Add(this.chkAll);
            this.groupBoxExportASCIIData.Controls.Add(this.btnCancel);
            this.groupBoxExportASCIIData.Controls.Add(this.btnExportData);
            this.groupBoxExportASCIIData.Controls.Add(this.cboSettings);
            this.groupBoxExportASCIIData.Controls.Add(this.lblFileFormat);
            this.groupBoxExportASCIIData.Controls.Add(this.dvgFileWiseExport);
            this.groupBoxExportASCIIData.Controls.Add(this.dgvMeterWiseExport);
            this.groupBoxExportASCIIData.Location = new System.Drawing.Point(6, 6);
            this.groupBoxExportASCIIData.Name = "groupBoxExportASCIIData";
            this.groupBoxExportASCIIData.Size = new System.Drawing.Size(724, 459);
            this.groupBoxExportASCIIData.TabIndex = 1;
            this.groupBoxExportASCIIData.TabStop = false;
            this.groupBoxExportASCIIData.Text = "Export ASCII Data";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(6, 426);
            this.chkAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(70, 17);
            this.chkAll.TabIndex = 23;
            this.chkAll.Text = "Select All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // rbtnMeterWise
            // 
            this.rbtnMeterWise.AutoSize = true;
            this.rbtnMeterWise.Location = new System.Drawing.Point(91, 17);
            this.rbtnMeterWise.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnMeterWise.Name = "rbtnMeterWise";
            this.rbtnMeterWise.Size = new System.Drawing.Size(79, 17);
            this.rbtnMeterWise.TabIndex = 20;
            this.rbtnMeterWise.Text = "Meter Wise";
            this.rbtnMeterWise.UseVisualStyleBackColor = true;
            this.rbtnMeterWise.CheckedChanged += new System.EventHandler(this.rbtnMeterWise_CheckedChanged);
            // 
            // rbtnFileWise
            // 
            this.rbtnFileWise.AutoSize = true;
            this.rbtnFileWise.Checked = true;
            this.rbtnFileWise.Location = new System.Drawing.Point(14, 16);
            this.rbtnFileWise.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnFileWise.Name = "rbtnFileWise";
            this.rbtnFileWise.Size = new System.Drawing.Size(68, 17);
            this.rbtnFileWise.TabIndex = 19;
            this.rbtnFileWise.TabStop = true;
            this.rbtnFileWise.Text = "File Wise";
            this.rbtnFileWise.UseVisualStyleBackColor = true;
            this.rbtnFileWise.CheckedChanged += new System.EventHandler(this.rbtnFileWise_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(650, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 29);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(580, 423);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(64, 29);
            this.btnExportData.TabIndex = 18;
            this.btnExportData.Text = "Export";
            this.btnExportData.UseVisualStyleBackColor = false;
            this.btnExportData.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnExportData.ForeColor = System.Drawing.Color.White;
            this.btnExportData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportData.FlatAppearance.BorderSize = 0;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // cboSettings
            // 
            this.cboSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSettings.FormattingEnabled = true;
            this.cboSettings.Location = new System.Drawing.Point(123, 18);
            this.cboSettings.Name = "cboSettings";
            this.cboSettings.Size = new System.Drawing.Size(218, 21);
            this.cboSettings.TabIndex = 15;
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.AutoSize = true;
            this.lblFileFormat.Location = new System.Drawing.Point(28, 21);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(54, 13);
            this.lblFileFormat.TabIndex = 13;
            this.lblFileFormat.Text = "File Name";
            // 
            // dvgFileWiseExport
            // 
            this.dvgFileWiseExport.AccessibleRole = System.Windows.Forms.AccessibleRole.RowHeader;
            this.dvgFileWiseExport.AllowUserToAddRows = false;
            this.dvgFileWiseExport.AllowUserToDeleteRows = false;
            this.dvgFileWiseExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgFileWiseExport.Location = new System.Drawing.Point(8, 86);
            this.dvgFileWiseExport.Margin = new System.Windows.Forms.Padding(2);
            this.dvgFileWiseExport.MultiSelect = false;
            this.dvgFileWiseExport.Name = "dvgFileWiseExport";
            this.dvgFileWiseExport.RowTemplate.Height = 24;
            this.dvgFileWiseExport.Size = new System.Drawing.Size(711, 332);
            this.dvgFileWiseExport.TabIndex = 24;
            this.dvgFileWiseExport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgFileWiseExport_CheckBoxChange);
            // 
            // dgvMeterWiseExport
            // 
            this.dgvMeterWiseExport.AccessibleRole = System.Windows.Forms.AccessibleRole.RowHeader;
            this.dgvMeterWiseExport.AllowUserToAddRows = false;
            this.dgvMeterWiseExport.AllowUserToDeleteRows = false;
            this.dgvMeterWiseExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeterWiseExport.Location = new System.Drawing.Point(8, 86);
            this.dgvMeterWiseExport.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMeterWiseExport.MultiSelect = false;
            this.dgvMeterWiseExport.Name = "dgvMeterWiseExport";
            this.dgvMeterWiseExport.RowTemplate.Height = 24;
            this.dgvMeterWiseExport.Size = new System.Drawing.Size(711, 301);
            this.dgvMeterWiseExport.TabIndex = 22;
            this.dgvMeterWiseExport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeterWiseExport_CheckBoxChange);
            // 
            // rdHTCT
            // 
            this.rdHTCT.AutoSize = true;
            this.rdHTCT.Location = new System.Drawing.Point(94, 17);
            this.rdHTCT.Margin = new System.Windows.Forms.Padding(2);
            this.rdHTCT.Name = "rdHTCT";
            this.rdHTCT.Size = new System.Drawing.Size(54, 17);
            this.rdHTCT.TabIndex = 26;
            this.rdHTCT.Text = "HTCT";
            this.rdHTCT.UseVisualStyleBackColor = true;
            this.rdHTCT.CheckedChanged += new System.EventHandler(this.rdHTCT_CheckedChanged);
            // 
            // rdLTCT
            // 
            this.rdLTCT.AutoSize = true;
            this.rdLTCT.Checked = true;
            this.rdLTCT.Location = new System.Drawing.Point(17, 15);
            this.rdLTCT.Margin = new System.Windows.Forms.Padding(2);
            this.rdLTCT.Name = "rdLTCT";
            this.rdLTCT.Size = new System.Drawing.Size(52, 17);
            this.rdLTCT.TabIndex = 25;
            this.rdLTCT.TabStop = true;
            this.rdLTCT.Text = "LTCT";
            this.rdLTCT.UseVisualStyleBackColor = true;
            this.rdLTCT.CheckedChanged += new System.EventHandler(this.rdLTCT_CheckedChanged);
            // 
            // gbExportType
            // 
            this.gbExportType.Controls.Add(this.rbtnFileWise);
            this.gbExportType.Controls.Add(this.rbtnMeterWise);
            this.gbExportType.Location = new System.Drawing.Point(122, 41);
            this.gbExportType.Name = "gbExportType";
            this.gbExportType.Size = new System.Drawing.Size(189, 40);
            this.gbExportType.TabIndex = 28;
            this.gbExportType.TabStop = false;
            this.gbExportType.Text = "Export Type";
            // 
            // gbMeterType
            // 
            this.gbMeterType.Controls.Add(this.rdLTCT);
            this.gbMeterType.Controls.Add(this.rdHTCT);
            this.gbMeterType.Location = new System.Drawing.Point(315, 41);
            this.gbMeterType.Name = "gbMeterType";
            this.gbMeterType.Size = new System.Drawing.Size(175, 40);
            this.gbMeterType.TabIndex = 29;
            this.gbMeterType.TabStop = false;
            this.gbMeterType.Text = "Meter Type";
            // 
            // ASCIIExportFileWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 470);
            this.Controls.Add(this.groupBoxExportASCIIData);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ASCIIExportFileWise";
            this.StatusMessage = "";
            this.Text = "ASCIIExportFileWise";
            this.Load += new System.EventHandler(this.ASCIIExportFileWise_Load);
            this.groupBoxExportASCIIData.ResumeLayout(false);
            this.groupBoxExportASCIIData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgFileWiseExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterWiseExport)).EndInit();
            this.gbExportType.ResumeLayout(false);
            this.gbExportType.PerformLayout();
            this.gbMeterType.ResumeLayout(false);
            this.gbMeterType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxExportASCIIData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.ComboBox cboSettings;
        private System.Windows.Forms.Label lblFileFormat;
        private System.Windows.Forms.RadioButton rbtnMeterWise;
        private System.Windows.Forms.RadioButton rbtnFileWise;
        private System.Windows.Forms.DataGridView dgvMeterWiseExport;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.DataGridView dvgFileWiseExport;
        private System.Windows.Forms.RadioButton rdHTCT;
        private System.Windows.Forms.RadioButton rdLTCT;
        private System.Windows.Forms.GroupBox gbMeterType;
        private System.Windows.Forms.GroupBox gbExportType;
    }
}

