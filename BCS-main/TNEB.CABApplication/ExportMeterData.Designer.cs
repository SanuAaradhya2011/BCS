namespace CAB.UI
{
    partial class ExportMeterData
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
			this.groupBoxExportMeterData = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBoxSelectMeters = new System.Windows.Forms.GroupBox();
			this.dGVCABMeterDetails = new System.Windows.Forms.DataGridView();
			this.btnExport = new System.Windows.Forms.Button();
			this.rbtnNormal = new System.Windows.Forms.RadioButton();
			this.rbtnASCII = new System.Windows.Forms.RadioButton();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBoxExportMeterData.SuspendLayout();
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
			this.tableLayoutPanel1.Controls.Add(this.groupBoxExportMeterData, 1, 1);
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
			// groupBoxExportMeterData
			// 
			this.groupBoxExportMeterData.Controls.Add(this.rbtnASCII);
			this.groupBoxExportMeterData.Controls.Add(this.rbtnNormal);
			this.groupBoxExportMeterData.Controls.Add(this.btnCancel);
			this.groupBoxExportMeterData.Controls.Add(this.groupBoxSelectMeters);
			this.groupBoxExportMeterData.Controls.Add(this.btnExport);
			this.groupBoxExportMeterData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxExportMeterData.Location = new System.Drawing.Point(48, 33);
			this.groupBoxExportMeterData.Name = "groupBoxExportMeterData";
			this.groupBoxExportMeterData.Size = new System.Drawing.Size(498, 367);
			this.groupBoxExportMeterData.TabIndex = 6;
			this.groupBoxExportMeterData.TabStop = false;
			this.groupBoxExportMeterData.Text = "Export Meter Data";
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
			// rbtnNormal
			// 
			this.rbtnNormal.AutoSize = true;
			this.rbtnNormal.Checked = true;
			this.rbtnNormal.Location = new System.Drawing.Point(329, 237);
			this.rbtnNormal.Name = "rbtnNormal";
			this.rbtnNormal.Size = new System.Drawing.Size(58, 17);
			this.rbtnNormal.TabIndex = 22;
			this.rbtnNormal.TabStop = true;
			this.rbtnNormal.Text = "Normal";
			this.rbtnNormal.UseVisualStyleBackColor = true;
			// 
			// rbtnASCII
			// 
			this.rbtnASCII.AutoSize = true;
			this.rbtnASCII.Location = new System.Drawing.Point(407, 237);
			this.rbtnASCII.Name = "rbtnASCII";
			this.rbtnASCII.Size = new System.Drawing.Size(52, 17);
			this.rbtnASCII.TabIndex = 23;
			this.rbtnASCII.Text = "ASCII";
			this.rbtnASCII.UseVisualStyleBackColor = true;
			// 
			// ExportMeterData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(598, 437);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ExportMeterData";
			this.Text = "Export Meter Data";
			this.Load += new System.EventHandler(this.ExportStandardData_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBoxExportMeterData.ResumeLayout(false);
			this.groupBoxExportMeterData.PerformLayout();
			this.groupBoxSelectMeters.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dGVCABMeterDetails)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxExportMeterData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.GroupBox groupBoxSelectMeters;
        public System.Windows.Forms.DataGridView dGVCABMeterDetails;
		private System.Windows.Forms.RadioButton rbtnASCII;
		private System.Windows.Forms.RadioButton rbtnNormal;
    }
}