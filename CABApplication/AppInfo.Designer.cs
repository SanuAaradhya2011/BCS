namespace CAB.UI
{
    partial class AppInfo
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnThreePhaseLTCT = new System.Windows.Forms.RadioButton();
            this.rbtnThreePhaseWholeCurrent = new System.Windows.Forms.RadioButton();
            this.rbtnSinglePhase = new System.Windows.Forms.RadioButton();
            this.grpProtocolType = new System.Windows.Forms.GroupBox();
            this.rbtnDLMS = new System.Windows.Forms.RadioButton();
            this.rbtnIEC = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.grpProtocolType.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnThreePhaseLTCT);
            this.groupBox2.Controls.Add(this.rbtnThreePhaseWholeCurrent);
            this.groupBox2.Controls.Add(this.rbtnSinglePhase);
            this.groupBox2.Location = new System.Drawing.Point(12, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 127);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meter Type";
            // 
            // rbtnThreePhaseLTCT
            // 
            this.rbtnThreePhaseLTCT.AutoSize = true;
            this.rbtnThreePhaseLTCT.Location = new System.Drawing.Point(19, 94);
            this.rbtnThreePhaseLTCT.Name = "rbtnThreePhaseLTCT";
            this.rbtnThreePhaseLTCT.Size = new System.Drawing.Size(116, 17);
            this.rbtnThreePhaseLTCT.TabIndex = 8;
            this.rbtnThreePhaseLTCT.Text = "Three Phase LTCT";
            this.rbtnThreePhaseLTCT.UseVisualStyleBackColor = true;
            // 
            // rbtnThreePhaseWholeCurrent
            // 
            this.rbtnThreePhaseWholeCurrent.AutoSize = true;
            this.rbtnThreePhaseWholeCurrent.Location = new System.Drawing.Point(19, 59);
            this.rbtnThreePhaseWholeCurrent.Name = "rbtnThreePhaseWholeCurrent";
            this.rbtnThreePhaseWholeCurrent.Size = new System.Drawing.Size(157, 17);
            this.rbtnThreePhaseWholeCurrent.TabIndex = 7;
            this.rbtnThreePhaseWholeCurrent.Text = "Three Phase Whole Current";
            this.rbtnThreePhaseWholeCurrent.UseVisualStyleBackColor = true;
            // 
            // rbtnSinglePhase
            // 
            this.rbtnSinglePhase.AutoSize = true;
            this.rbtnSinglePhase.Checked = true;
            this.rbtnSinglePhase.Location = new System.Drawing.Point(20, 26);
            this.rbtnSinglePhase.Name = "rbtnSinglePhase";
            this.rbtnSinglePhase.Size = new System.Drawing.Size(87, 17);
            this.rbtnSinglePhase.TabIndex = 6;
            this.rbtnSinglePhase.TabStop = true;
            this.rbtnSinglePhase.Text = "Single Phase";
            this.rbtnSinglePhase.UseVisualStyleBackColor = true;
            // 
            // grpProtocolType
            // 
            this.grpProtocolType.Controls.Add(this.rbtnDLMS);
            this.grpProtocolType.Controls.Add(this.rbtnIEC);
            this.grpProtocolType.Location = new System.Drawing.Point(12, 3);
            this.grpProtocolType.Name = "grpProtocolType";
            this.grpProtocolType.Size = new System.Drawing.Size(187, 76);
            this.grpProtocolType.TabIndex = 9;
            this.grpProtocolType.TabStop = false;
            this.grpProtocolType.Text = "Protocol Type";
            // 
            // rbtnDLMS
            // 
            this.rbtnDLMS.AutoSize = true;
            this.rbtnDLMS.Location = new System.Drawing.Point(19, 53);
            this.rbtnDLMS.Name = "rbtnDLMS";
            this.rbtnDLMS.Size = new System.Drawing.Size(55, 17);
            this.rbtnDLMS.TabIndex = 7;
            this.rbtnDLMS.Text = "DLMS";
            this.rbtnDLMS.UseVisualStyleBackColor = true;
            // 
            // rbtnIEC
            // 
            this.rbtnIEC.AutoSize = true;
            this.rbtnIEC.Checked = true;
            this.rbtnIEC.Location = new System.Drawing.Point(19, 22);
            this.rbtnIEC.Name = "rbtnIEC";
            this.rbtnIEC.Size = new System.Drawing.Size(42, 17);
            this.rbtnIEC.TabIndex = 6;
            this.rbtnIEC.TabStop = true;
            this.rbtnIEC.Text = "IEC";
            this.rbtnIEC.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(139, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(61, 218);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(60, 23);
            this.btnConfigure.TabIndex = 8;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.UseVisualStyleBackColor = false;
            this.btnConfigure.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnConfigure.ForeColor = System.Drawing.Color.White;
            this.btnConfigure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigure.FlatAppearance.BorderSize = 0;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // AppInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(212, 245);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpProtocolType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfigure);
            this.Name = "AppInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusMessage = "";
            this.Text = "Meter Type";
            this.Load += new System.EventHandler(this.ApplicationType_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpProtocolType.ResumeLayout(false);
            this.grpProtocolType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnThreePhaseLTCT;
        private System.Windows.Forms.RadioButton rbtnThreePhaseWholeCurrent;
        private System.Windows.Forms.RadioButton rbtnSinglePhase;
        private System.Windows.Forms.GroupBox grpProtocolType;
        private System.Windows.Forms.RadioButton rbtnDLMS;
        private System.Windows.Forms.RadioButton rbtnIEC;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfigure;

    }
}

