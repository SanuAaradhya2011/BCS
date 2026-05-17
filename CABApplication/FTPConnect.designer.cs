namespace CABApplication
{
    partial class FTPConnect
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
            this.tabControl1 = new CAB.UI.Controls.PremiumTabControl();
            this.tabTreeView = new System.Windows.Forms.TabPage();
            this.menuToolTreeView = new System.Windows.Forms.ToolStrip();
            this.lblGetTreeList = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.lblDownloadFileList = new System.Windows.Forms.ToolStripLabel();
            this.lsttreeLiist = new System.Windows.Forms.ListBox();
            this.tabListView = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpFTP = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSelectedFile = new System.Windows.Forms.TextBox();
            this.lstfileLiist = new System.Windows.Forms.ListBox();
            this.menuTool = new System.Windows.Forms.ToolStrip();
            this.lblGetFileList = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDownload = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblDelete = new System.Windows.Forms.ToolStripLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stsmsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1.SuspendLayout();
            this.tabTreeView.SuspendLayout();
            this.menuToolTreeView.SuspendLayout();
            this.tabListView.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpFTP.SuspendLayout();
            this.menuTool.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTreeView);
            this.tabControl1.Controls.Add(this.tabListView);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(919, 607);
            this.tabControl1.TabIndex = 63;
            // 
            // tabTreeView
            // 
            this.tabTreeView.Controls.Add(this.menuToolTreeView);
            this.tabTreeView.Controls.Add(this.lsttreeLiist);
            this.tabTreeView.Location = new System.Drawing.Point(4, 22);
            this.tabTreeView.Name = "tabTreeView";
            this.tabTreeView.Padding = new System.Windows.Forms.Padding(3);
            this.tabTreeView.Size = new System.Drawing.Size(911, 581);
            this.tabTreeView.TabIndex = 0;
            this.tabTreeView.Text = "Tree View";
            // 
            // menuToolTreeView
            // 
            this.menuToolTreeView.BackColor = System.Drawing.SystemColors.Control;
            this.menuToolTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblGetTreeList,
            this.toolStripLabel6,
            this.toolStripSeparator3,
            this.lblDownloadFileList});
            this.menuToolTreeView.Location = new System.Drawing.Point(3, 3);
            this.menuToolTreeView.Name = "menuToolTreeView";
            this.menuToolTreeView.Size = new System.Drawing.Size(905, 25);
            this.menuToolTreeView.TabIndex = 64;
            this.menuToolTreeView.Text = "toolStrip1";
            // 
            // lblGetTreeList
            // 
            this.lblGetTreeList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGetTreeList.Name = "lblGetTreeList";
            this.lblGetTreeList.Size = new System.Drawing.Size(88, 22);
            this.lblGetTreeList.Text = "Get Tree View";
            this.lblGetTreeList.Click += new System.EventHandler(this.lblGetTreeList_Click);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(0, 22);
            // 
            // lblDownloadFileList
            // 
            this.lblDownloadFileList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDownloadFileList.Name = "lblDownloadFileList";
            this.lblDownloadFileList.Size = new System.Drawing.Size(90, 22);
            this.lblDownloadFileList.Text = "Download Files";
            this.lblDownloadFileList.Click += new System.EventHandler(this.lblDownloadFileList_Click);
            // 
            // lsttreeLiist
            // 
            this.lsttreeLiist.FormattingEnabled = true;
            this.lsttreeLiist.Location = new System.Drawing.Point(8, 34);
            this.lsttreeLiist.Name = "lsttreeLiist";
            this.lsttreeLiist.Size = new System.Drawing.Size(897, 320);
            this.lsttreeLiist.TabIndex = 2;
            // 
            // tabListView
            // 
            this.tabListView.Controls.Add(this.groupBox1);
            this.tabListView.Controls.Add(this.menuTool);
            this.tabListView.Location = new System.Drawing.Point(4, 22);
            this.tabListView.Name = "tabListView";
            this.tabListView.Padding = new System.Windows.Forms.Padding(3);
            this.tabListView.Size = new System.Drawing.Size(911, 581);
            this.tabListView.TabIndex = 1;
            this.tabListView.Text = "List View";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpFTP);
            this.groupBox1.Location = new System.Drawing.Point(10, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(890, 545);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            // 
            // grpFTP
            // 
            this.grpFTP.Controls.Add(this.label2);
            this.grpFTP.Controls.Add(this.txtSelectedFile);
            this.grpFTP.Controls.Add(this.lstfileLiist);
            this.grpFTP.Location = new System.Drawing.Point(15, 4);
            this.grpFTP.Name = "grpFTP";
            this.grpFTP.Size = new System.Drawing.Size(863, 520);
            this.grpFTP.TabIndex = 0;
            this.grpFTP.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Selected File";
            // 
            // txtSelectedFile
            // 
            this.txtSelectedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelectedFile.Location = new System.Drawing.Point(80, 13);
            this.txtSelectedFile.Name = "txtSelectedFile";
            this.txtSelectedFile.ReadOnly = true;
            this.txtSelectedFile.Size = new System.Drawing.Size(773, 20);
            this.txtSelectedFile.TabIndex = 4;
            // 
            // lstfileLiist
            // 
            this.lstfileLiist.FormattingEnabled = true;
            this.lstfileLiist.Location = new System.Drawing.Point(21, 39);
            this.lstfileLiist.Name = "lstfileLiist";
            this.lstfileLiist.Size = new System.Drawing.Size(833, 320);
            this.lstfileLiist.TabIndex = 1;
            this.lstfileLiist.SelectedIndexChanged += new System.EventHandler(this.lstfileLiist_Click);
            this.lstfileLiist.DoubleClick += new System.EventHandler(this.lstfileLiist_DoubleClick);
            // 
            // menuTool
            // 
            this.menuTool.BackColor = System.Drawing.SystemColors.Control;
            this.menuTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblGetFileList,
            this.toolStripSeparator2,
            this.lblDownload,
            this.toolStripSeparator1,
            this.lblDelete,
            this.lblStatus});
            this.menuTool.Location = new System.Drawing.Point(3, 3);
            this.menuTool.Name = "menuTool";
            this.menuTool.Size = new System.Drawing.Size(905, 25);
            this.menuTool.TabIndex = 63;
            this.menuTool.Text = "toolStrip1";
            // 
            // lblGetFileList
            // 
            this.lblGetFileList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGetFileList.Name = "lblGetFileList";
            this.lblGetFileList.Size = new System.Drawing.Size(72, 22);
            this.lblGetFileList.Text = "Get File List";
            this.lblGetFileList.Click += new System.EventHandler(this.lblGetFileList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblDownload
            // 
            this.lblDownload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(85, 22);
            this.lblDownload.Text = "Download File";
            this.lblDownload.Click += new System.EventHandler(this.lblDownload_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblDelete
            // 
            this.lblDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(45, 22);
            this.lblDelete.Text = "Delete";
            this.lblDelete.Click += new System.EventHandler(this.lblDelete_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 22);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsmsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 610);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(923, 22);
            this.statusStrip1.TabIndex = 64;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stsmsg
            // 
            this.stsmsg.BackColor = System.Drawing.SystemColors.Control;
            this.stsmsg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stsmsg.ForeColor = System.Drawing.Color.Green;
            this.stsmsg.Name = "stsmsg";
            this.stsmsg.Size = new System.Drawing.Size(130, 17);
            this.stsmsg.Text = "toolStripStatusLabel1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // FTPConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 632);
            this.ControlBox = false;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FTPConnect";
            this.ShowIcon = false;
            this.Text = "Download Readout Files";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FTPConnect_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FTPConnect_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabTreeView.ResumeLayout(false);
            this.tabTreeView.PerformLayout();
            this.menuToolTreeView.ResumeLayout(false);
            this.menuToolTreeView.PerformLayout();
            this.tabListView.ResumeLayout(false);
            this.tabListView.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.grpFTP.ResumeLayout(false);
            this.grpFTP.PerformLayout();
            this.menuTool.ResumeLayout(false);
            this.menuTool.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabControl1;
        private System.Windows.Forms.TabPage tabTreeView;
        private System.Windows.Forms.TabPage tabListView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpFTP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSelectedFile;
        private System.Windows.Forms.ListBox lstfileLiist;
        private System.Windows.Forms.ToolStrip menuTool;
        private System.Windows.Forms.ToolStripLabel lblGetFileList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblDownload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblDelete;
        private System.Windows.Forms.ToolStripLabel lblStatus;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stsmsg;
        private System.Windows.Forms.ListBox lsttreeLiist;
        private System.Windows.Forms.ToolStrip menuToolTreeView;
        private System.Windows.Forms.ToolStripLabel lblGetTreeList;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripLabel lblDownloadFileList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

    }
}
