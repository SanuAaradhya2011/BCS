namespace CAB.UI
{
    partial class SystemInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemInfo));
            this.tabSystemInfo = new CAB.UI.Controls.PremiumTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOS = new System.Windows.Forms.Label();
            this.lbldomainname = new System.Windows.Forms.Label();
            this.lblsystemdirectory = new System.Windows.Forms.Label();
            this.lblcpu = new System.Windows.Forms.Label();
            this.lblmonitorsize = new System.Windows.Forms.Label();
            this.lblsystemname = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstinfo = new System.Windows.Forms.ListBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSystemInfo.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSystemInfo
            // 
            this.tabSystemInfo.Controls.Add(this.tabPage1);
            this.tabSystemInfo.Controls.Add(this.tabPage2);
            this.tabSystemInfo.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabSystemInfo.ItemSize = new System.Drawing.Size(120, 30);
            this.tabSystemInfo.Location = new System.Drawing.Point(1, 2);
            this.tabSystemInfo.Name = "tabSystemInfo";
            this.tabSystemInfo.SelectedIndex = 0;
            this.tabSystemInfo.Size = new System.Drawing.Size(453, 288);
            this.tabSystemInfo.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(445, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblOS);
            this.groupBox1.Controls.Add(this.lbldomainname);
            this.groupBox1.Controls.Add(this.lblsystemdirectory);
            this.groupBox1.Controls.Add(this.lblcpu);
            this.groupBox1.Controls.Add(this.lblmonitorsize);
            this.groupBox1.Controls.Add(this.lblsystemname);
            this.groupBox1.Location = new System.Drawing.Point(29, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 214);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Location = new System.Drawing.Point(15, 172);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(59, 25);
            this.lblOS.TabIndex = 12;
            this.lblOS.Text = "label1";
            // 
            // lbldomainname
            // 
            this.lbldomainname.AutoSize = true;
            this.lbldomainname.Location = new System.Drawing.Point(15, 57);
            this.lbldomainname.Name = "lbldomainname";
            this.lbldomainname.Size = new System.Drawing.Size(59, 25);
            this.lbldomainname.TabIndex = 11;
            this.lbldomainname.Text = "label7";
            // 
            // lblsystemdirectory
            // 
            this.lblsystemdirectory.AutoSize = true;
            this.lblsystemdirectory.Location = new System.Drawing.Point(15, 87);
            this.lblsystemdirectory.Name = "lblsystemdirectory";
            this.lblsystemdirectory.Size = new System.Drawing.Size(59, 25);
            this.lblsystemdirectory.TabIndex = 10;
            this.lblsystemdirectory.Text = "label6";
            // 
            // lblcpu
            // 
            this.lblcpu.AutoSize = true;
            this.lblcpu.Location = new System.Drawing.Point(15, 145);
            this.lblcpu.Name = "lblcpu";
            this.lblcpu.Size = new System.Drawing.Size(59, 25);
            this.lblcpu.TabIndex = 9;
            this.lblcpu.Text = "label4";
            // 
            // lblmonitorsize
            // 
            this.lblmonitorsize.AutoSize = true;
            this.lblmonitorsize.Location = new System.Drawing.Point(15, 116);
            this.lblmonitorsize.Name = "lblmonitorsize";
            this.lblmonitorsize.Size = new System.Drawing.Size(59, 25);
            this.lblmonitorsize.TabIndex = 8;
            this.lblmonitorsize.Text = "label2";
            // 
            // lblsystemname
            // 
            this.lblsystemname.AutoSize = true;
            this.lblsystemname.Location = new System.Drawing.Point(15, 27);
            this.lblsystemname.Name = "lblsystemname";
            this.lblsystemname.Size = new System.Drawing.Size(59, 25);
            this.lblsystemname.TabIndex = 7;
            this.lblsystemname.Text = "label1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstinfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(445, 250);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Process";
            // 
            // lstinfo
            // 
            this.lstinfo.FormattingEnabled = true;
            this.lstinfo.ItemHeight = 25;
            this.lstinfo.Location = new System.Drawing.Point(7, 6);
            this.lstinfo.Name = "lstinfo";
            this.lstinfo.Size = new System.Drawing.Size(435, 229);
            this.lstinfo.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Sno";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Process Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Process ID";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // SystemInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(461, 298);
            this.ControlBox = true;
            this.Controls.Add(this.tabSystemInfo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusMessage = "";
            this.Text = "System Info";
            this.Load += new System.EventHandler(this.SystemInfo_Load);
            this.tabSystemInfo.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabSystemInfo;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblsystemdirectory;
        private System.Windows.Forms.Label lblcpu;
        private System.Windows.Forms.Label lblmonitorsize;
        private System.Windows.Forms.Label lblsystemname;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ListBox lstinfo;
        private System.Windows.Forms.Label lbldomainname;
    }
}
