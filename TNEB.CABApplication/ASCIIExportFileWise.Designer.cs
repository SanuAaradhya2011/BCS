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
            this.label1 = new System.Windows.Forms.Label();
            this.rbtnMeterWise = new System.Windows.Forms.RadioButton();
            this.rbtnFileWise = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExportData = new System.Windows.Forms.Button();
            this.cboSettings = new System.Windows.Forms.ComboBox();
            this.lblFileFormat = new System.Windows.Forms.Label();
            this.dvgFileWiseExport = new System.Windows.Forms.DataGridView();
            this.dgvMeterWiseExport = new System.Windows.Forms.DataGridView();
            this.groupBoxExportASCIIData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgFileWiseExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterWiseExport)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxExportASCIIData
            // 
            this.groupBoxExportASCIIData.Controls.Add(this.chkAll);
            this.groupBoxExportASCIIData.Controls.Add(this.label1);
            this.groupBoxExportASCIIData.Controls.Add(this.rbtnMeterWise);
            this.groupBoxExportASCIIData.Controls.Add(this.rbtnFileWise);
            this.groupBoxExportASCIIData.Controls.Add(this.btnCancel);
            this.groupBoxExportASCIIData.Controls.Add(this.btnExportData);
            this.groupBoxExportASCIIData.Controls.Add(this.cboSettings);
            this.groupBoxExportASCIIData.Controls.Add(this.lblFileFormat);
            this.groupBoxExportASCIIData.Controls.Add(this.dvgFileWiseExport);
            this.groupBoxExportASCIIData.Controls.Add(this.dgvMeterWiseExport);
            this.groupBoxExportASCIIData.Location = new System.Drawing.Point(6, 6);
            this.groupBoxExportASCIIData.Name = "groupBoxExportASCIIData";
            this.groupBoxExportASCIIData.Size = new System.Drawing.Size(724, 431);
            this.groupBoxExportASCIIData.TabIndex = 1;
            this.groupBoxExportASCIIData.TabStop = false;
            this.groupBoxExportASCIIData.Text = "Export ASCII Data";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(6, 395);
            this.chkAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(70, 17);
            this.chkAll.TabIndex = 23;
            this.chkAll.Text = "Select All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Export Type";
            // 
            // rbtnMeterWise
            // 
            this.rbtnMeterWise.AutoSize = true;
            this.rbtnMeterWise.Location = new System.Drawing.Point(200, 52);
            this.rbtnMeterWise.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.rbtnFileWise.Location = new System.Drawing.Point(123, 50);
            this.rbtnFileWise.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.btnCancel.Location = new System.Drawing.Point(650, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 29);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(580, 392);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(64, 29);
            this.btnExportData.TabIndex = 18;
            this.btnExportData.Text = "Export";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // cboSettings
            // 
            this.cboSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSettings.FormattingEnabled = true;
            this.cboSettings.Location = new System.Drawing.Point(123, 19);
            this.cboSettings.Name = "cboSettings";
            this.cboSettings.Size = new System.Drawing.Size(218, 21);
            this.cboSettings.TabIndex = 15;
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.AutoSize = true;
            this.lblFileFormat.Location = new System.Drawing.Point(28, 22);
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
            this.dvgFileWiseExport.Location = new System.Drawing.Point(8, 74);
            this.dvgFileWiseExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dvgFileWiseExport.MultiSelect = false;
            this.dvgFileWiseExport.Name = "dvgFileWiseExport";
            this.dvgFileWiseExport.RowTemplate.Height = 24;
            this.dvgFileWiseExport.Size = new System.Drawing.Size(711, 313);
            this.dvgFileWiseExport.TabIndex = 24;
            // 
            // dgvMeterWiseExport
            // 
            this.dgvMeterWiseExport.AccessibleRole = System.Windows.Forms.AccessibleRole.RowHeader;
            this.dgvMeterWiseExport.AllowUserToAddRows = false;
            this.dgvMeterWiseExport.AllowUserToDeleteRows = false;
            this.dgvMeterWiseExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeterWiseExport.Location = new System.Drawing.Point(8, 74);
            this.dgvMeterWiseExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvMeterWiseExport.MultiSelect = false;
            this.dgvMeterWiseExport.Name = "dgvMeterWiseExport";
            this.dgvMeterWiseExport.RowTemplate.Height = 24;
            this.dgvMeterWiseExport.Size = new System.Drawing.Size(711, 313);
            this.dgvMeterWiseExport.TabIndex = 22;
            // 
            // ASCIIExportFileWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(740, 442);
            this.Controls.Add(this.groupBoxExportASCIIData);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ASCIIExportFileWise";
            this.StatusMessage = "";
            this.Text = "ASCIIExportFileWise";
            this.Load += new System.EventHandler(this.ASCIIExportFileWise_Load);
            this.groupBoxExportASCIIData.ResumeLayout(false);
            this.groupBoxExportASCIIData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgFileWiseExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterWiseExport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxExportASCIIData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.ComboBox cboSettings;
        private System.Windows.Forms.Label lblFileFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtnMeterWise;
        private System.Windows.Forms.RadioButton rbtnFileWise;
        private System.Windows.Forms.DataGridView dgvMeterWiseExport;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.DataGridView dvgFileWiseExport;
    }
}