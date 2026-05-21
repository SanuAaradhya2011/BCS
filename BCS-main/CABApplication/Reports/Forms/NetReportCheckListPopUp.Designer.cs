namespace CABApplication.Reports.Forms
{
    partial class NetReportCheckListPopUp
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
            this.chkSelectedList = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkSelectedList
            // 
            this.chkSelectedList.FormattingEnabled = true;
            this.chkSelectedList.Location = new System.Drawing.Point(12, 13);
            this.chkSelectedList.Name = "chkSelectedList";
            this.chkSelectedList.Size = new System.Drawing.Size(408, 259);
            this.chkSelectedList.TabIndex = 0;
            this.chkSelectedList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkSelectedList_MouseClick);
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(12, 289);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSelectAll.TabIndex = 2;
            this.checkBoxSelectAll.Text = "Select All";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.checkBoxSelectAll_CheckedChanged);
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(362, 285);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(53, 23);
            this.btnSaveChanges.TabIndex = 3;
            this.btnSaveChanges.Text = "Next > ";
            this.btnSaveChanges.UseVisualStyleBackColor = false;
            this.btnSaveChanges.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSaveChanges.ForeColor = System.Drawing.Color.White;
            this.btnSaveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveChanges.FlatAppearance.BorderSize = 0;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // NetReportCheckListPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 328);
            this.Controls.Add(this.btnSaveChanges);
            this.Controls.Add(this.checkBoxSelectAll);
            this.Controls.Add(this.chkSelectedList);
            this.MaximumSize = new System.Drawing.Size(448, 366);
            this.MinimumSize = new System.Drawing.Size(448, 366);
            this.Name = "NetReportCheckListPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Net Metering Check List ";
            this.Load += new System.EventHandler(this.NetReportCheckListPopUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkSelectedList;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private System.Windows.Forms.Button btnSaveChanges;
    }
}

