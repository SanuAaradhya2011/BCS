namespace CABApplication
{
    partial class E650CMRICommunication
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
            this.lblMeterID = new System.Windows.Forms.Label();
            this.lblMeterIDVal = new System.Windows.Forms.Label();
            this.chkOtherCMRI = new System.Windows.Forms.CheckBox();
            this.chkNameplateCMRI = new System.Windows.Forms.CheckBox();
            this.chkTamperCMRI = new System.Windows.Forms.CheckBox();
            this.chkLoadSurveyCMRI = new System.Windows.Forms.CheckBox();
            this.chkBillingCMRI = new System.Windows.Forms.CheckBox();
            this.chkInstaCMRI = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.chkBoxAutoScrollTime = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btnReadDisplayParamsTimeout = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.txtBoxAutoScrollTime = new System.Windows.Forms.TextBox();
            this.txtBoxPushTimeout = new System.Windows.Forms.TextBox();
            this.txtBoxScrollTime = new System.Windows.Forms.TextBox();
            this.btnWriteDisplayParamsTimeout = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearCMRI = new CAB.UI.Controls.CABButton();
            this.lblTOUFile = new CAB.UI.Controls.CABLabel();
            this.btnBrowse = new CAB.UI.Controls.CABButton();
            this.btnCancelCMRI = new CAB.UI.Controls.CABButton();
            this.btnUpdateRTC = new CAB.UI.Controls.CABButton();
            this.btnReadData = new CAB.UI.Controls.CABButton();
            this.btnPrepareCMRI = new CAB.UI.Controls.CABButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.485785F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.87889F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.61938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.688581F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.61938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.07266F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.609F));
            this.tableLayoutPanel1.Controls.Add(this.lblMeterID, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblMeterID
            // 
            this.lblMeterID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMeterID.AutoSize = true;
            this.lblMeterID.Location = new System.Drawing.Point(4, 17);
            this.lblMeterID.Name = "lblMeterID";
            this.lblMeterID.Size = new System.Drawing.Size(10, 65);
            this.lblMeterID.TabIndex = 0;
            this.lblMeterID.Text = "Meter ID";
            // 
            // lblMeterIDVal
            // 
            this.lblMeterIDVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMeterIDVal.AutoSize = true;
            this.lblMeterIDVal.Location = new System.Drawing.Point(46, 11);
            this.lblMeterIDVal.Name = "lblMeterIDVal";
            this.lblMeterIDVal.Size = new System.Drawing.Size(49, 13);
            this.lblMeterIDVal.TabIndex = 1;
            this.lblMeterIDVal.Text = "1002001";
            // 
            // chkOtherCMRI
            // 
            this.chkOtherCMRI.AutoSize = true;
            this.chkOtherCMRI.Checked = true;
            this.chkOtherCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOtherCMRI.Location = new System.Drawing.Point(35, 250);
            this.chkOtherCMRI.Name = "chkOtherCMRI";
            this.chkOtherCMRI.Size = new System.Drawing.Size(70, 17);
            this.chkOtherCMRI.TabIndex = 37;
            this.chkOtherCMRI.Text = "Select All";
            this.chkOtherCMRI.UseVisualStyleBackColor = true;
            // 
            // chkNameplateCMRI
            // 
            this.chkNameplateCMRI.AutoSize = true;
            this.chkNameplateCMRI.Checked = true;
            this.chkNameplateCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNameplateCMRI.Enabled = false;
            this.chkNameplateCMRI.Location = new System.Drawing.Point(35, 204);
            this.chkNameplateCMRI.Name = "chkNameplateCMRI";
            this.chkNameplateCMRI.Size = new System.Drawing.Size(63, 17);
            this.chkNameplateCMRI.TabIndex = 36;
            this.chkNameplateCMRI.Text = "General";
            this.chkNameplateCMRI.UseVisualStyleBackColor = true;
            // 
            // chkTamperCMRI
            // 
            this.chkTamperCMRI.AutoSize = true;
            this.chkTamperCMRI.Checked = true;
            this.chkTamperCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTamperCMRI.Location = new System.Drawing.Point(35, 159);
            this.chkTamperCMRI.Name = "chkTamperCMRI";
            this.chkTamperCMRI.Size = new System.Drawing.Size(71, 17);
            this.chkTamperCMRI.TabIndex = 35;
            this.chkTamperCMRI.Text = "Event log";
            this.chkTamperCMRI.UseVisualStyleBackColor = true;
            // 
            // chkLoadSurveyCMRI
            // 
            this.chkLoadSurveyCMRI.AutoSize = true;
            this.chkLoadSurveyCMRI.Checked = true;
            this.chkLoadSurveyCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoadSurveyCMRI.Location = new System.Drawing.Point(35, 115);
            this.chkLoadSurveyCMRI.Name = "chkLoadSurveyCMRI";
            this.chkLoadSurveyCMRI.Size = new System.Drawing.Size(86, 17);
            this.chkLoadSurveyCMRI.TabIndex = 34;
            this.chkLoadSurveyCMRI.Text = "Load Survey";
            this.chkLoadSurveyCMRI.UseVisualStyleBackColor = true;
            // 
            // chkBillingCMRI
            // 
            this.chkBillingCMRI.AutoSize = true;
            this.chkBillingCMRI.Checked = true;
            this.chkBillingCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBillingCMRI.Location = new System.Drawing.Point(35, 71);
            this.chkBillingCMRI.Name = "chkBillingCMRI";
            this.chkBillingCMRI.Size = new System.Drawing.Size(88, 17);
            this.chkBillingCMRI.TabIndex = 33;
            this.chkBillingCMRI.Text = "Billing History";
            this.chkBillingCMRI.UseVisualStyleBackColor = true;
            // 
            // chkInstaCMRI
            // 
            this.chkInstaCMRI.AutoSize = true;
            this.chkInstaCMRI.Checked = true;
            this.chkInstaCMRI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInstaCMRI.Location = new System.Drawing.Point(35, 27);
            this.chkInstaCMRI.Name = "chkInstaCMRI";
            this.chkInstaCMRI.Size = new System.Drawing.Size(93, 17);
            this.chkInstaCMRI.TabIndex = 32;
            this.chkInstaCMRI.Text = "Instantaneous";
            this.chkInstaCMRI.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "S.No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Real Time Clock";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 175;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Port Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(326, 128);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(108, 13);
            this.label43.TabIndex = 9;
            this.label43.Text = "* Valid Range (1-300)";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(326, 92);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(108, 13);
            this.label42.TabIndex = 9;
            this.label42.Text = "* Valid Range (1-600)";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(326, 57);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(108, 13);
            this.label41.TabIndex = 9;
            this.label41.Text = "* Valid Range (1-300)";
            // 
            // chkBoxAutoScrollTime
            // 
            this.chkBoxAutoScrollTime.AutoSize = true;
            this.chkBoxAutoScrollTime.Location = new System.Drawing.Point(67, 127);
            this.chkBoxAutoScrollTime.Name = "chkBoxAutoScrollTime";
            this.chkBoxAutoScrollTime.Size = new System.Drawing.Size(145, 17);
            this.chkBoxAutoScrollTime.TabIndex = 8;
            this.chkBoxAutoScrollTime.Text = "Auto Scroll Resume Time";
            this.chkBoxAutoScrollTime.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(64, 92);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(106, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "Push Button Timeout";
            // 
            // btnReadDisplayParamsTimeout
            // 
            this.btnReadDisplayParamsTimeout.Enabled = false;
            this.btnReadDisplayParamsTimeout.Location = new System.Drawing.Point(326, 221);
            this.btnReadDisplayParamsTimeout.Name = "btnReadDisplayParamsTimeout";
            this.btnReadDisplayParamsTimeout.Size = new System.Drawing.Size(75, 26);
            this.btnReadDisplayParamsTimeout.TabIndex = 7;
            this.btnReadDisplayParamsTimeout.Text = "Read";
            this.btnReadDisplayParamsTimeout.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(64, 57);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Scroll Time Per Item";
            // 
            // txtBoxAutoScrollTime
            // 
            this.txtBoxAutoScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxAutoScrollTime.Enabled = false;
            this.txtBoxAutoScrollTime.Location = new System.Drawing.Point(245, 125);
            this.txtBoxAutoScrollTime.MaxLength = 3;
            this.txtBoxAutoScrollTime.Name = "txtBoxAutoScrollTime";
            this.txtBoxAutoScrollTime.Size = new System.Drawing.Size(75, 20);
            this.txtBoxAutoScrollTime.TabIndex = 5;
            // 
            // txtBoxPushTimeout
            // 
            this.txtBoxPushTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxPushTimeout.Location = new System.Drawing.Point(245, 89);
            this.txtBoxPushTimeout.MaxLength = 3;
            this.txtBoxPushTimeout.Name = "txtBoxPushTimeout";
            this.txtBoxPushTimeout.Size = new System.Drawing.Size(75, 20);
            this.txtBoxPushTimeout.TabIndex = 5;
            // 
            // txtBoxScrollTime
            // 
            this.txtBoxScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxScrollTime.Location = new System.Drawing.Point(245, 54);
            this.txtBoxScrollTime.MaxLength = 3;
            this.txtBoxScrollTime.Name = "txtBoxScrollTime";
            this.txtBoxScrollTime.Size = new System.Drawing.Size(75, 20);
            this.txtBoxScrollTime.TabIndex = 5;
            // 
            // btnWriteDisplayParamsTimeout
            // 
            this.btnWriteDisplayParamsTimeout.Enabled = false;
            this.btnWriteDisplayParamsTimeout.Location = new System.Drawing.Point(245, 221);
            this.btnWriteDisplayParamsTimeout.Name = "btnWriteDisplayParamsTimeout";
            this.btnWriteDisplayParamsTimeout.Size = new System.Drawing.Size(75, 26);
            this.btnWriteDisplayParamsTimeout.TabIndex = 6;
            this.btnWriteDisplayParamsTimeout.Text = "Write";
            this.btnWriteDisplayParamsTimeout.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClearCMRI);
            this.groupBox1.Controls.Add(this.lblTOUFile);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.btnCancelCMRI);
            this.groupBox1.Controls.Add(this.btnUpdateRTC);
            this.groupBox1.Controls.Add(this.btnReadData);
            this.groupBox1.Controls.Add(this.btnPrepareCMRI);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 303);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CMRI Communication";
            // 
            // btnClearCMRI
            // 
            this.btnClearCMRI.Location = new System.Drawing.Point(50, 194);
            this.btnClearCMRI.Name = "btnClearCMRI";
            this.btnClearCMRI.Size = new System.Drawing.Size(187, 34);
            this.btnClearCMRI.TabIndex = 11;
            this.btnClearCMRI.Text = "C&lear CMRI";
            this.btnClearCMRI.TranslationKey = "B000019";
            this.btnClearCMRI.UseVisualStyleBackColor = true;
            this.btnClearCMRI.Click += new System.EventHandler(this.btnClearCMRI_Click);
            // 
            // lblTOUFile
            // 
            this.lblTOUFile.AutoSize = true;
            this.lblTOUFile.Location = new System.Drawing.Point(183, 287);
            this.lblTOUFile.Name = "lblTOUFile";
            this.lblTOUFile.Size = new System.Drawing.Size(54, 13);
            this.lblTOUFile.TabIndex = 10;
            this.lblTOUFile.Text = "File Name";
            this.lblTOUFile.TranslationKey = null;
            this.lblTOUFile.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(243, 74);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(63, 34);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.TranslationKey = "B000011";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancelCMRI
            // 
            this.btnCancelCMRI.Location = new System.Drawing.Point(50, 234);
            this.btnCancelCMRI.Name = "btnCancelCMRI";
            this.btnCancelCMRI.Size = new System.Drawing.Size(187, 34);
            this.btnCancelCMRI.TabIndex = 8;
            this.btnCancelCMRI.Text = "&Cancel";
            this.btnCancelCMRI.TranslationKey = "B000009";
            this.btnCancelCMRI.UseVisualStyleBackColor = true;
            this.btnCancelCMRI.Click += new System.EventHandler(this.btnCancelCMRI_Click);
            // 
            // btnUpdateRTC
            // 
            this.btnUpdateRTC.Location = new System.Drawing.Point(50, 154);
            this.btnUpdateRTC.Name = "btnUpdateRTC";
            this.btnUpdateRTC.Size = new System.Drawing.Size(187, 34);
            this.btnUpdateRTC.TabIndex = 7;
            this.btnUpdateRTC.Text = "Update RTC";
            this.btnUpdateRTC.TranslationKey = "B000017";
            this.btnUpdateRTC.UseVisualStyleBackColor = true;
            this.btnUpdateRTC.Click += new System.EventHandler(this.btnUpdateRTC_Click);
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(50, 114);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(187, 34);
            this.btnReadData.TabIndex = 6;
            this.btnReadData.Text = "Read Data";
            this.btnReadData.TranslationKey = "B000016";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnPrepareCMRI
            // 
            this.btnPrepareCMRI.Location = new System.Drawing.Point(50, 74);
            this.btnPrepareCMRI.Name = "btnPrepareCMRI";
            this.btnPrepareCMRI.Size = new System.Drawing.Size(187, 34);
            this.btnPrepareCMRI.TabIndex = 5;
            this.btnPrepareCMRI.Text = "Prepare CMRI";
            this.btnPrepareCMRI.TranslationKey = "B000015";
            this.btnPrepareCMRI.UseVisualStyleBackColor = true;
            this.btnPrepareCMRI.Click += new System.EventHandler(this.btnPrepareCMRI_Click);
            // 
            // E650CMRICommunication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(380, 327);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "E650CMRICommunication";
            this.StatusMessage = "";
            this.Text = "CMRI Communication";
            this.Load += new System.EventHandler(this.E650CMRICommunication_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.E650CMRICommunication_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }











        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMeterID;
        public System.Windows.Forms.Label lblMeterIDVal;
        private System.Windows.Forms.CheckBox chkOtherCMRI;
        private System.Windows.Forms.CheckBox chkNameplateCMRI;
        private System.Windows.Forms.CheckBox chkTamperCMRI;
        private System.Windows.Forms.CheckBox chkLoadSurveyCMRI;
        private System.Windows.Forms.CheckBox chkBillingCMRI;
        private System.Windows.Forms.CheckBox chkInstaCMRI;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.CheckBox chkBoxAutoScrollTime;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnReadDisplayParamsTimeout;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtBoxAutoScrollTime;
        private System.Windows.Forms.TextBox txtBoxPushTimeout;
        private System.Windows.Forms.TextBox txtBoxScrollTime;
        private System.Windows.Forms.Button btnWriteDisplayParamsTimeout;
        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABButton btnClearCMRI;
        private CAB.UI.Controls.CABLabel lblTOUFile;
        private CAB.UI.Controls.CABButton btnBrowse;
        private CAB.UI.Controls.CABButton btnCancelCMRI;
        private CAB.UI.Controls.CABButton btnUpdateRTC;
        private CAB.UI.Controls.CABButton btnReadData;
        private CAB.UI.Controls.CABButton btnPrepareCMRI;
    }
}

