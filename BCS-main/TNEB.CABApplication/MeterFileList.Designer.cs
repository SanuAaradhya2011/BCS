namespace CAB.UI
{
    partial class MeterFileList
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
            this.lngFileLists = new CAB.UI.Controls.CABGridControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lngFileLists
            // 
            this.lngFileLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lngFileLists.AutoScroll = true;
            this.lngFileLists.Data = null;
            this.lngFileLists.HiddenColumn = null;
            this.lngFileLists.HiddenColumns = null;
            this.lngFileLists.IsEqual = true;
            this.lngFileLists.IsFullRow = false;
            this.lngFileLists.IsSorting = false;
            this.lngFileLists.Location = new System.Drawing.Point(12, 44);
            this.lngFileLists.Margin = new System.Windows.Forms.Padding(4);
            this.lngFileLists.Name = "lngFileLists";
            this.lngFileLists.SelectedIndex = 0;
            this.lngFileLists.Size = new System.Drawing.Size(666, 396);
            this.lngFileLists.TabIndex = 0;
            this.lngFileLists.ValueColumn = null;
            this.lngFileLists.Load += new System.EventHandler(this.lngFileLists_Load);
            this.lngFileLists.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lngFileLists_OnGridRowChanged);
            this.lngFileLists.OnGridRowChanged1 += new CAB.UI.Controls.CABGridControl.GridRowChanged1(this.lngFileLists_OnGridRowChanged1);
            this.lngFileLists.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lngFileLists_OnGridMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblText);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 30);
            this.panel1.TabIndex = 2;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblText.Location = new System.Drawing.Point(4, 9);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(47, 15);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "label1";
            this.lblText.Click += new System.EventHandler(this.lblText_Click);
            // 
            // MeterFileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(835, 509);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lngFileLists);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MeterFileList";
            this.StatusMessage = "";
            this.Text = "MeterFileList";
            this.Load += new System.EventHandler(this.MeterFileList_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MeterFileList_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABGridControl lngFileLists;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lblText;
    }
}