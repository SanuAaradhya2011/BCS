namespace CAB.UI
{
    partial class HVDSExport
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
            this.grpUpload = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsmsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtBoxFileName = new System.Windows.Forms.TextBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.grpUpload.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // grpUpload
            // 
            this.grpUpload.Controls.Add(this.label2);
            this.grpUpload.Controls.Add(this.label1);
            this.grpUpload.Controls.Add(this.txtOutput);
            this.grpUpload.Controls.Add(this.chkSelectAll);
            this.grpUpload.Controls.Add(this.btnClose);
            this.grpUpload.Controls.Add(this.btnUpload);
            this.grpUpload.Controls.Add(this.btnAbort);
            this.grpUpload.Controls.Add(this.btnBrowse);
            this.grpUpload.Controls.Add(this.btnExport);
            this.grpUpload.Controls.Add(this.statusStrip1);
            this.grpUpload.Controls.Add(this.txtBoxFileName);
            this.grpUpload.Location = new System.Drawing.Point(0, 324);
            this.grpUpload.Name = "grpUpload";
            this.grpUpload.Size = new System.Drawing.Size(749, 136);
            this.grpUpload.TabIndex = 2;
            this.grpUpload.TabStop = false;
            this.grpUpload.Text = "Upload File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 75;
            this.label2.Text = "Source File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 74;
            this.label1.Text = "Destination";
            // 
            // txtOutput
            // 
            this.txtOutput.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOutput.Location = new System.Drawing.Point(71, 28);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(673, 20);
            this.txtOutput.TabIndex = 73;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(675, 9);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(67, 17);
            this.chkSelectAll.TabIndex = 71;
            this.chkSelectAll.Text = "SelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(667, 82);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 70;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(424, 82);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 69;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(586, 82);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 68;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = false;
            this.btnAbort.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAbort.ForeColor = System.Drawing.Color.White;
            this.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbort.FlatAppearance.BorderSize = 0;
            this.btnAbort.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(667, 52);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 67;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(505, 82);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 66;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsmsg});
            this.statusStrip1.Location = new System.Drawing.Point(3, 111);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(743, 22);
            this.statusStrip1.TabIndex = 65;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stsmsg
            // 
            this.stsmsg.BackColor = System.Drawing.SystemColors.Control;
            this.stsmsg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stsmsg.ForeColor = System.Drawing.Color.Green;
            this.stsmsg.Name = "stsmsg";
            this.stsmsg.Size = new System.Drawing.Size(0, 17);
            // 
            // txtBoxFileName
            // 
            this.txtBoxFileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBoxFileName.Location = new System.Drawing.Point(71, 53);
            this.txtBoxFileName.Name = "txtBoxFileName";
            this.txtBoxFileName.ReadOnly = true;
            this.txtBoxFileName.Size = new System.Drawing.Size(590, 20);
            this.txtBoxFileName.TabIndex = 5;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Name = "dgvList";
            this.dgvList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dgvList.Size = new System.Drawing.Size(752, 323);
            this.dgvList.TabIndex = 3;
            // 
            // HVDSExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 460);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.grpUpload);
            this.Name = "HVDSExport";
            this.StatusMessage = "";
            this.Text = "HVDS Export";
            this.Load += new System.EventHandler(this.HVDSExport_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HVDSExport_FormClosing);
            this.SizeChanged += new System.EventHandler(this.HVDSExport_SizeChanged);
            this.grpUpload.ResumeLayout(false);
            this.grpUpload.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpUpload;
        private System.Windows.Forms.TextBox txtBoxFileName;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsmsg;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

