namespace CAB.UI
{
    partial class LoadSurveyReportFileWise
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
            this.groupBoxLoadSurvey = new System.Windows.Forms.GroupBox();
            this.SMD_rbtnLoadSurveyEnergy = new System.Windows.Forms.RadioButton();
            this.SMD_rbtnLoadSurveyDemand = new System.Windows.Forms.RadioButton();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupLoadSurvey = new System.Windows.Forms.GroupBox();
            this.lblNoDataFound = new System.Windows.Forms.Label();
            this.chkListLoadSurveyParameters = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbWithPadding = new System.Windows.Forms.RadioButton();
            this.rdbNoPadding = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxLoadSurvey.SuspendLayout();
            this.groupLoadSurvey.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLoadSurvey
            // 
            this.groupBoxLoadSurvey.Controls.Add(this.SMD_rbtnLoadSurveyEnergy);
            this.groupBoxLoadSurvey.Controls.Add(this.SMD_rbtnLoadSurveyDemand);
            this.groupBoxLoadSurvey.Location = new System.Drawing.Point(8, 78);
            this.groupBoxLoadSurvey.Name = "groupBoxLoadSurvey";
            this.groupBoxLoadSurvey.Size = new System.Drawing.Size(279, 34);
            this.groupBoxLoadSurvey.TabIndex = 2;
            this.groupBoxLoadSurvey.TabStop = false;
            // 
            // SMD_rbtnLoadSurveyEnergy
            // 
            this.SMD_rbtnLoadSurveyEnergy.AutoSize = true;
            this.SMD_rbtnLoadSurveyEnergy.Location = new System.Drawing.Point(156, 11);
            this.SMD_rbtnLoadSurveyEnergy.Name = "SMD_rbtnLoadSurveyEnergy";
            this.SMD_rbtnLoadSurveyEnergy.Size = new System.Drawing.Size(58, 17);
            this.SMD_rbtnLoadSurveyEnergy.TabIndex = 2;
            this.SMD_rbtnLoadSurveyEnergy.Text = "Energy";
            this.SMD_rbtnLoadSurveyEnergy.UseVisualStyleBackColor = true;
            this.SMD_rbtnLoadSurveyEnergy.CheckedChanged += new System.EventHandler(this.SMD_rbtnLoadSurveyEnergy_CheckedChanged);
            // 
            // SMD_rbtnLoadSurveyDemand
            // 
            this.SMD_rbtnLoadSurveyDemand.AutoSize = true;
            this.SMD_rbtnLoadSurveyDemand.Location = new System.Drawing.Point(17, 11);
            this.SMD_rbtnLoadSurveyDemand.Name = "SMD_rbtnLoadSurveyDemand";
            this.SMD_rbtnLoadSurveyDemand.Size = new System.Drawing.Size(65, 17);
            this.SMD_rbtnLoadSurveyDemand.TabIndex = 1;
            this.SMD_rbtnLoadSurveyDemand.Text = "Demand";
            this.SMD_rbtnLoadSurveyDemand.UseVisualStyleBackColor = true;
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(212, 367);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 7;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = false;
            this.SMD_btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.SMD_btnCancel.ForeColor = System.Drawing.Color.White;
            this.SMD_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SMD_btnCancel.FlatAppearance.BorderSize = 0;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(122, 367);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 6;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupLoadSurvey
            // 
            this.groupLoadSurvey.Controls.Add(this.lblNoDataFound);
            this.groupLoadSurvey.Controls.Add(this.chkListLoadSurveyParameters);
            this.groupLoadSurvey.Location = new System.Drawing.Point(9, 151);
            this.groupLoadSurvey.Name = "groupLoadSurvey";
            this.groupLoadSurvey.Size = new System.Drawing.Size(278, 210);
            this.groupLoadSurvey.TabIndex = 8;
            this.groupLoadSurvey.TabStop = false;
            this.groupLoadSurvey.Text = "Load Survey Parameters";
            // 
            // lblNoDataFound
            // 
            this.lblNoDataFound.AutoSize = true;
            this.lblNoDataFound.BackColor = System.Drawing.Color.White;
            this.lblNoDataFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataFound.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataFound.Location = new System.Drawing.Point(13, 21);
            this.lblNoDataFound.Name = "lblNoDataFound";
            this.lblNoDataFound.Size = new System.Drawing.Size(98, 13);
            this.lblNoDataFound.TabIndex = 1;
            this.lblNoDataFound.Text = "lblNoDataFound";
            // 
            // chkListLoadSurveyParameters
            // 
            this.chkListLoadSurveyParameters.BackColor = System.Drawing.Color.White;
            this.chkListLoadSurveyParameters.CheckOnClick = true;
            this.chkListLoadSurveyParameters.FormattingEnabled = true;
            this.chkListLoadSurveyParameters.Location = new System.Drawing.Point(7, 17);
            this.chkListLoadSurveyParameters.Name = "chkListLoadSurveyParameters";
            this.chkListLoadSurveyParameters.Size = new System.Drawing.Size(263, 184);
            this.chkListLoadSurveyParameters.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbWithPadding);
            this.groupBox1.Controls.Add(this.rdbNoPadding);
            this.groupBox1.Location = new System.Drawing.Point(8, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 34);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // rdbWithPadding
            // 
            this.rdbWithPadding.AutoSize = true;
            this.rdbWithPadding.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbWithPadding.Location = new System.Drawing.Point(156, 11);
            this.rdbWithPadding.Name = "rdbWithPadding";
            this.rdbWithPadding.Size = new System.Drawing.Size(89, 17);
            this.rdbWithPadding.TabIndex = 20;
            this.rdbWithPadding.Text = "With Padding";
            this.rdbWithPadding.UseVisualStyleBackColor = true;
            // 
            // rdbNoPadding
            // 
            this.rdbNoPadding.AutoSize = true;
            this.rdbNoPadding.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbNoPadding.Location = new System.Drawing.Point(17, 11);
            this.rdbNoPadding.Name = "rdbNoPadding";
            this.rdbNoPadding.Size = new System.Drawing.Size(104, 17);
            this.rdbNoPadding.TabIndex = 19;
            this.rdbNoPadding.Text = "Without Padding";
            this.rdbNoPadding.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePickerStart);
            this.groupBox2.Controls.Add(this.dateTimePickerEnd);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(9, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 60);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(16, 34);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(98, 20);
            this.dateTimePickerStart.TabIndex = 19;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(155, 34);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(99, 20);
            this.dateTimePickerEnd.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "End Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Start Date:";
            // 
            // LoadSurveyReportFileWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(297, 413);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupLoadSurvey);
            this.Controls.Add(this.SMD_btnCancel);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.groupBoxLoadSurvey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadSurveyReportFileWise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Survey";
            this.Load += new System.EventHandler(this.LoadSurveyReport_Load);
            this.groupBoxLoadSurvey.ResumeLayout(false);
            this.groupBoxLoadSurvey.PerformLayout();
            this.groupLoadSurvey.ResumeLayout(false);
            this.groupLoadSurvey.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLoadSurvey;
        private System.Windows.Forms.RadioButton SMD_rbtnLoadSurveyEnergy;
        private System.Windows.Forms.RadioButton SMD_rbtnLoadSurveyDemand;
        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupLoadSurvey;
        private System.Windows.Forms.CheckedListBox chkListLoadSurveyParameters;
        private System.Windows.Forms.Label lblNoDataFound;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbWithPadding;
        private System.Windows.Forms.RadioButton rdbNoPadding;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
    }
}

