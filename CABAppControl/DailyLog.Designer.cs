namespace CABAppControl
{
    partial class DailyLog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkCumulativeKwh = new System.Windows.Forms.CheckBox();
            this.chkCumulativeKVARhLag = new System.Windows.Forms.CheckBox();
            this.chkCumulativeKVARhLead = new System.Windows.Forms.CheckBox();
            this.chkCumulativeKVAh = new System.Windows.Forms.CheckBox();
            this.chkDailyMD1 = new System.Windows.Forms.CheckBox();
            this.chkDailyMD2 = new System.Windows.Forms.CheckBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.gbDailyLog = new System.Windows.Forms.GroupBox();
            this.gbDailyLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkCumulativeKwh
            // 
            this.chkCumulativeKwh.AutoSize = true;
            this.chkCumulativeKwh.Location = new System.Drawing.Point(7, 19);
            this.chkCumulativeKwh.Name = "chkCumulativeKwh";
            this.chkCumulativeKwh.Size = new System.Drawing.Size(108, 17);
            this.chkCumulativeKwh.TabIndex = 0;
            this.chkCumulativeKwh.Text = "Cumulative KWh ";
            this.chkCumulativeKwh.UseVisualStyleBackColor = true;
            this.chkCumulativeKwh.CheckedChanged += new System.EventHandler(this.chkCumulativeKwh_CheckedChanged);
            // 
            // chkCumulativeKVARhLag
            // 
            this.chkCumulativeKVARhLag.AutoSize = true;
            this.chkCumulativeKVARhLag.Location = new System.Drawing.Point(7, 48);
            this.chkCumulativeKVARhLag.Name = "chkCumulativeKVARhLag";
            this.chkCumulativeKVARhLag.Size = new System.Drawing.Size(140, 17);
            this.chkCumulativeKVARhLag.TabIndex = 1;
            this.chkCumulativeKVARhLag.Text = "Cumulative KVARh(Lag)";
            this.chkCumulativeKVARhLag.UseVisualStyleBackColor = true;
            this.chkCumulativeKVARhLag.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // chkCumulativeKVARhLead
            // 
            this.chkCumulativeKVARhLead.AutoSize = true;
            this.chkCumulativeKVARhLead.Location = new System.Drawing.Point(7, 77);
            this.chkCumulativeKVARhLead.Name = "chkCumulativeKVARhLead";
            this.chkCumulativeKVARhLead.Size = new System.Drawing.Size(146, 17);
            this.chkCumulativeKVARhLead.TabIndex = 2;
            this.chkCumulativeKVARhLead.Text = "Cumulative KVARh(Lead)\r\n";
            this.chkCumulativeKVARhLead.UseVisualStyleBackColor = true;
            this.chkCumulativeKVARhLead.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // chkCumulativeKVAh
            // 
            this.chkCumulativeKVAh.AutoSize = true;
            this.chkCumulativeKVAh.Location = new System.Drawing.Point(7, 106);
            this.chkCumulativeKVAh.Name = "chkCumulativeKVAh";
            this.chkCumulativeKVAh.Size = new System.Drawing.Size(108, 17);
            this.chkCumulativeKVAh.TabIndex = 3;
            this.chkCumulativeKVAh.Text = "Cumulative KVAh";
            this.chkCumulativeKVAh.UseVisualStyleBackColor = true;
            this.chkCumulativeKVAh.CheckedChanged += new System.EventHandler(this.chkCumulativeKVAh_CheckedChanged);
            // 
            // chkDailyMD1
            // 
            this.chkDailyMD1.AutoSize = true;
            this.chkDailyMD1.Location = new System.Drawing.Point(7, 135);
            this.chkDailyMD1.Name = "chkDailyMD1";
            this.chkDailyMD1.Size = new System.Drawing.Size(75, 17);
            this.chkDailyMD1.TabIndex = 4;
            this.chkDailyMD1.Text = "Daily MD1";
            this.chkDailyMD1.UseVisualStyleBackColor = true;
            this.chkDailyMD1.CheckedChanged += new System.EventHandler(this.chkDailyMD1_CheckedChanged);
            // 
            // chkDailyMD2
            // 
            this.chkDailyMD2.AutoSize = true;
            this.chkDailyMD2.Location = new System.Drawing.Point(7, 164);
            this.chkDailyMD2.Name = "chkDailyMD2";
            this.chkDailyMD2.Size = new System.Drawing.Size(75, 17);
            this.chkDailyMD2.TabIndex = 5;
            this.chkDailyMD2.Text = "Daily MD2";
            this.chkDailyMD2.UseVisualStyleBackColor = true;
            this.chkDailyMD2.CheckedChanged += new System.EventHandler(this.chkDailyMD2_CheckedChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(7, 209);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(69, 17);
            this.chkSelectAll.TabIndex = 6;
            this.chkSelectAll.Text = "Select all";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // gbDailyLog
            // 
            this.gbDailyLog.Controls.Add(this.chkCumulativeKwh);
            this.gbDailyLog.Controls.Add(this.chkSelectAll);
            this.gbDailyLog.Controls.Add(this.chkCumulativeKVARhLag);
            this.gbDailyLog.Controls.Add(this.chkDailyMD2);
            this.gbDailyLog.Controls.Add(this.chkCumulativeKVARhLead);
            this.gbDailyLog.Controls.Add(this.chkDailyMD1);
            this.gbDailyLog.Controls.Add(this.chkCumulativeKVAh);
            this.gbDailyLog.Location = new System.Drawing.Point(19, 21);
            this.gbDailyLog.Name = "gbDailyLog";
            this.gbDailyLog.Size = new System.Drawing.Size(250, 233);
            this.gbDailyLog.TabIndex = 7;
            this.gbDailyLog.TabStop = false;
            // 
            // DailyLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDailyLog);
            this.Name = "DailyLog";
            this.Size = new System.Drawing.Size(311, 267);
            this.gbDailyLog.ResumeLayout(false);
            this.gbDailyLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCumulativeKwh;
        private System.Windows.Forms.CheckBox chkCumulativeKVARhLag;
        private System.Windows.Forms.CheckBox chkCumulativeKVARhLead;
        private System.Windows.Forms.CheckBox chkCumulativeKVAh;
        private System.Windows.Forms.CheckBox chkDailyMD1;
        private System.Windows.Forms.CheckBox chkDailyMD2;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.GroupBox gbDailyLog;
    }
}
