namespace CAB.UI
{
    partial class GsmReadReport
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
            this.pnlDate = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gpDateRange = new System.Windows.Forms.GroupBox();
            this.lstTaskList = new System.Windows.Forms.ListBox();
            this.btnTaskID = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnShow = new System.Windows.Forms.Button();
            this.pnlDate.SuspendLayout();
            this.gpDateRange.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.btnCancel);
            this.pnlDate.Controls.Add(this.gpDateRange);
            this.pnlDate.Controls.Add(this.btnShow);
            this.pnlDate.Location = new System.Drawing.Point(8, 10);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(394, 301);
            this.pnlDate.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(310, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gpDateRange
            // 
            this.gpDateRange.Controls.Add(this.lstTaskList);
            this.gpDateRange.Controls.Add(this.btnTaskID);
            this.gpDateRange.Controls.Add(this.label1);
            this.gpDateRange.Controls.Add(this.lblFrom);
            this.gpDateRange.Controls.Add(this.dtpFrom);
            this.gpDateRange.Controls.Add(this.lblTo);
            this.gpDateRange.Controls.Add(this.dtpTo);
            this.gpDateRange.Location = new System.Drawing.Point(9, 8);
            this.gpDateRange.Name = "gpDateRange";
            this.gpDateRange.Size = new System.Drawing.Size(378, 259);
            this.gpDateRange.TabIndex = 7;
            this.gpDateRange.TabStop = false;
            this.gpDateRange.Text = "Schedule Date Range";
            // 
            // lstTaskList
            // 
            this.lstTaskList.FormattingEnabled = true;
            this.lstTaskList.Location = new System.Drawing.Point(76, 74);
            this.lstTaskList.Name = "lstTaskList";
            this.lstTaskList.Size = new System.Drawing.Size(192, 147);
            this.lstTaskList.TabIndex = 7;
            // 
            // btnTaskID
            // 
            this.btnTaskID.Location = new System.Drawing.Point(129, 227);
            this.btnTaskID.Name = "btnTaskID";
            this.btnTaskID.Size = new System.Drawing.Size(71, 23);
            this.btnTaskID.TabIndex = 6;
            this.btnTaskID.Text = "Get TaskID";
            this.btnTaskID.UseVisualStyleBackColor = false;
            this.btnTaskID.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnTaskID.ForeColor = System.Drawing.Color.White;
            this.btnTaskID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaskID.FlatAppearance.BorderSize = 0;
            this.btnTaskID.Click += new System.EventHandler(this.btnTaskID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Task ID";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(17, 36);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(56, 13);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From Date";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(76, 33);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(97, 20);
            this.dtpFrom.TabIndex = 0;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(203, 37);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(46, 13);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "To Date";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(252, 34);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 20);
            this.dtpTo.TabIndex = 1;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(229, 270);
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
            // GsmReadReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 317);
            this.Controls.Add(this.pnlDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GsmReadReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GSM Read Report";
            this.pnlDate.ResumeLayout(false);
            this.gpDateRange.ResumeLayout(false);
            this.gpDateRange.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox gpDateRange;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTaskID;
        private System.Windows.Forms.ListBox lstTaskList;
    }
}
