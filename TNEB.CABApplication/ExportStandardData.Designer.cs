namespace CAB.UI
{
    partial class ExportStandardData
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBoxExportStandardData = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBoxSelectMeters = new System.Windows.Forms.GroupBox();
			this.dGVCABMeterDetails = new System.Windows.Forms.DataGridView();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtBoxFileName = new System.Windows.Forms.TextBox();
			this.lblCABFileName = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBoxExportStandardData.SuspendLayout();
			this.groupBoxSelectMeters.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dGVCABMeterDetails)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 515F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
			this.tableLayoutPanel1.Controls.Add(this.groupBoxExportStandardData, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 388F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(598, 453);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// groupBoxExportStandardData
			// 
			this.groupBoxExportStandardData.Controls.Add(this.btnCancel);
			this.groupBoxExportStandardData.Controls.Add(this.groupBoxSelectMeters);
			this.groupBoxExportStandardData.Controls.Add(this.btnExport);
			this.groupBoxExportStandardData.Controls.Add(this.btnBrowse);
			this.groupBoxExportStandardData.Controls.Add(this.txtBoxFileName);
			this.groupBoxExportStandardData.Controls.Add(this.lblCABFileName);
			this.groupBoxExportStandardData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxExportStandardData.Location = new System.Drawing.Point(23, 33);
			this.groupBoxExportStandardData.Name = "groupBoxExportStandardData";
			this.groupBoxExportStandardData.Size = new System.Drawing.Size(509, 382);
			this.groupBoxExportStandardData.TabIndex = 6;
			this.groupBoxExportStandardData.TabStop = false;
			this.groupBoxExportStandardData.Text = "Export Standard Data";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(401, 334);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(68, 28);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupBoxSelectMeters
			// 
			this.groupBoxSelectMeters.Controls.Add(this.dGVCABMeterDetails);
			this.groupBoxSelectMeters.Location = new System.Drawing.Point(43, 110);
			this.groupBoxSelectMeters.Name = "groupBoxSelectMeters";
			this.groupBoxSelectMeters.Size = new System.Drawing.Size(426, 177);
			this.groupBoxSelectMeters.TabIndex = 21;
			this.groupBoxSelectMeters.TabStop = false;
			this.groupBoxSelectMeters.Text = "Select meters";
			// 
			// dGVCABMeterDetails
			// 
			this.dGVCABMeterDetails.AllowUserToAddRows = false;
			this.dGVCABMeterDetails.AllowUserToDeleteRows = false;
			this.dGVCABMeterDetails.AllowUserToResizeColumns = false;
			this.dGVCABMeterDetails.AllowUserToResizeRows = false;
			this.dGVCABMeterDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dGVCABMeterDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dGVCABMeterDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dGVCABMeterDetails.Location = new System.Drawing.Point(3, 16);
			this.dGVCABMeterDetails.MultiSelect = false;
			this.dGVCABMeterDetails.Name = "dGVCABMeterDetails";
			this.dGVCABMeterDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dGVCABMeterDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dGVCABMeterDetails.Size = new System.Drawing.Size(420, 158);
			this.dGVCABMeterDetails.TabIndex = 1;
			//this.dGVCABMeterDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVCABMeterDetails_CellContentClick);
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(327, 334);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(68, 28);
			this.btnExport.TabIndex = 0;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(352, 49);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(34, 23);
			this.btnBrowse.TabIndex = 20;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			//this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtBoxFileName
			// 
			this.txtBoxFileName.BackColor = System.Drawing.Color.White;
			this.txtBoxFileName.Location = new System.Drawing.Point(128, 50);
			this.txtBoxFileName.Name = "txtBoxFileName";
			this.txtBoxFileName.ReadOnly = true;
			this.txtBoxFileName.Size = new System.Drawing.Size(218, 20);
			this.txtBoxFileName.TabIndex = 19;
			// 
			// lblCABFileName
			// 
			this.lblCABFileName.AutoSize = true;
			this.lblCABFileName.Location = new System.Drawing.Point(40, 54);
			this.lblCABFileName.Name = "lblCABFileName";
			this.lblCABFileName.Size = new System.Drawing.Size(74, 13);
			this.lblCABFileName.TabIndex = 18;
			this.lblCABFileName.Text = "CAB file name";
			// 
			// ExportStandardData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(598, 453);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ExportStandardData";
			this.Text = "ExportStandardData";
			this.Load += new System.EventHandler(this.ExportStandardData_Load);
			//this.Activated += new System.EventHandler(this.ExportStandardData_Activated);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBoxExportStandardData.ResumeLayout(false);
			this.groupBoxExportStandardData.PerformLayout();
			this.groupBoxSelectMeters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dGVCABMeterDetails)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxExportStandardData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBoxSelectMeters;
        private System.Windows.Forms.Button btnBrowse;
        public System.Windows.Forms.TextBox txtBoxFileName;
        private System.Windows.Forms.Label lblCABFileName;
        public System.Windows.Forms.DataGridView dGVCABMeterDetails;
    }
}