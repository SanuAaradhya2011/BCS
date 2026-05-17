namespace CAB.UI
{
    partial class MidNightReportMeterIDWise
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
            this.radioBoxPanel = new System.Windows.Forms.Panel();
            this.meterIDLabel = new System.Windows.Forms.Label();
            this.groupLoadDate = new System.Windows.Forms.GroupBox();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnShow = new System.Windows.Forms.Button();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.chkListMidnightParameters = new System.Windows.Forms.CheckedListBox();
            this.lblNoDataFound = new System.Windows.Forms.Label();
            this.groupLoadDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioBoxPanel
            // 
            this.radioBoxPanel.AutoScroll = true;
            this.radioBoxPanel.BackColor = System.Drawing.SystemColors.Window;
            this.radioBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.radioBoxPanel.Location = new System.Drawing.Point(6, 21);
            this.radioBoxPanel.Name = "radioBoxPanel";
            this.radioBoxPanel.Size = new System.Drawing.Size(336, 230);
            this.radioBoxPanel.TabIndex = 13;
            // 
            // meterIDLabel
            // 
            this.meterIDLabel.AutoSize = true;
            this.meterIDLabel.Location = new System.Drawing.Point(12, 5);
            this.meterIDLabel.Name = "meterIDLabel";
            this.meterIDLabel.Size = new System.Drawing.Size(48, 13);
            this.meterIDLabel.TabIndex = 14;
            this.meterIDLabel.Text = "Meter ID";
            // 
            // groupLoadDate
            // 
            this.groupLoadDate.Controls.Add(this.dateTimeEnd);
            this.groupLoadDate.Controls.Add(this.dateTimeStart);
            this.groupLoadDate.Controls.Add(this.label2);
            this.groupLoadDate.Controls.Add(this.label1);
            this.groupLoadDate.Location = new System.Drawing.Point(12, 255);
            this.groupLoadDate.Name = "groupLoadDate";
            this.groupLoadDate.Size = new System.Drawing.Size(325, 72);
            this.groupLoadDate.TabIndex = 15;
            this.groupLoadDate.TabStop = false;
            this.groupLoadDate.Text = "Occurrence Date";
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.CustomFormat = "dd/MM/yyyy";
            this.dateTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeEnd.Location = new System.Drawing.Point(185, 36);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.Size = new System.Drawing.Size(81, 20);
            this.dateTimeEnd.TabIndex = 3;
            this.dateTimeEnd.DropDown += new System.EventHandler(this.dateTimeEnd_DropDown);
            this.dateTimeEnd.CloseUp += new System.EventHandler(this.dateTimeEnd_CloseUp);
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.CustomFormat = "dd/MM/yyyy";
            this.dateTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeStart.Location = new System.Drawing.Point(29, 36);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.Size = new System.Drawing.Size(88, 20);
            this.dateTimeStart.TabIndex = 2;
            this.dateTimeStart.DropDown += new System.EventHandler(this.dateTimeStart_DropDown);
            this.dateTimeStart.CloseUp += new System.EventHandler(this.dateTimeStart_CloseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Date";
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(181, 330);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 16;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(262, 330);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 17;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = false;
            this.SMD_btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.SMD_btnCancel.ForeColor = System.Drawing.Color.White;
            this.SMD_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SMD_btnCancel.FlatAppearance.BorderSize = 0;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click);
            // 
            // chkListMidnightParameters
            // 
            this.chkListMidnightParameters.CheckOnClick = true;
            this.chkListMidnightParameters.FormattingEnabled = true;
            this.chkListMidnightParameters.HorizontalScrollbar = true;
            this.chkListMidnightParameters.Location = new System.Drawing.Point(4, 358);
            this.chkListMidnightParameters.Name = "chkListMidnightParameters";
            this.chkListMidnightParameters.Size = new System.Drawing.Size(338, 229);
            this.chkListMidnightParameters.TabIndex = 18;
            // 
            // lblNoDataFound
            // 
            this.lblNoDataFound.AutoSize = true;
            this.lblNoDataFound.BackColor = System.Drawing.Color.White;
            this.lblNoDataFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataFound.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataFound.Location = new System.Drawing.Point(19, 361);
            this.lblNoDataFound.Name = "lblNoDataFound";
            this.lblNoDataFound.Size = new System.Drawing.Size(93, 13);
            this.lblNoDataFound.TabIndex = 10;
            this.lblNoDataFound.Text = "No Data Found";
            // 
            // MidNightReportMeterIDWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(347, 596);
            this.Controls.Add(this.lblNoDataFound);
            this.Controls.Add(this.chkListMidnightParameters);
            this.Controls.Add(this.SMD_btnCancel);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.groupLoadDate);
            this.Controls.Add(this.meterIDLabel);
            this.Controls.Add(this.radioBoxPanel);
            this.MaximumSize = new System.Drawing.Size(363, 634);
            this.MinimumSize = new System.Drawing.Size(363, 634);
            this.Name = "MidNightReportMeterIDWise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Midnight Report";
            this.Load += new System.EventHandler(this.MidNightReportMeterIDWise_Load);
            this.groupLoadDate.ResumeLayout(false);
            this.groupLoadDate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel radioBoxPanel;
        private System.Windows.Forms.Label meterIDLabel;
        private System.Windows.Forms.GroupBox groupLoadDate;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.CheckedListBox chkListMidnightParameters;
        private System.Windows.Forms.Label lblNoDataFound;
    }
}
