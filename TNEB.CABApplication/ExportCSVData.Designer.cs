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
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 504F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.Controls.Add(this.groupBoxExportStandardData, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 373F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(598, 437);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// groupBoxExportStandardData
			// 
			this.groupBoxExportStandardData.Controls.Add(this.btnCancel);
			this.groupBoxExportStandardData.Controls.Add(this.groupBoxSelectMeters);
			this.groupBoxExportStandardData.Controls.Add(this.btnExport);
			this.groupBoxExportStandardData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxExportStandardData.Location = new System.Drawing.Point(48, 33);
			this.groupBoxExportStandardData.Name = "groupBoxExportStandardData";
			this.groupBoxExportStandardData.Size = new System.Drawing.Size(498, 367);
			this.groupBoxExportStandardData.TabIndex = 6;
			this.groupBoxExportStandardData.TabStop = false;
			this.groupBoxExportStandardData.Text = "Export CSV Data";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(394, 281);
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
			this.groupBoxSelectMeters.Location = new System.Drawing.Point(36, 57);
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
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(320, 281);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(68, 28);
			this.btnExport.TabIndex = 0;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// ExportStandardData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(598, 437);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ExportStandardData";
			this.Text = "Export CSV Data";
			this.Load += new System.EventHandler(this.ExportStandardData_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBoxExportStandardData.ResumeLayout(false);
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
        public System.Windows.Forms.DataGridView dGVCABMeterDetails;
    }
}