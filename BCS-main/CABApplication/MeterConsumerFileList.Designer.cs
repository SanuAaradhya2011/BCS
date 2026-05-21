namespace CAB.UI
{
    partial class MeterConsumerFileList
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
            this.lngFileLists.Location = new System.Drawing.Point(18, 68);
            this.lngFileLists.Margin = new System.Windows.Forms.Padding(6);
            this.lngFileLists.Name = "lngFileLists";
            this.lngFileLists.SelectedIndex = 0;
            this.lngFileLists.SelectedRowId = "";
            this.lngFileLists.Size = new System.Drawing.Size(1394, 479);
            this.lngFileLists.TabIndex = 0;
            this.lngFileLists.ValueColumn = null;
            this.lngFileLists.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lngFileLists_OnGridMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.Controls.Add(this.lblText);
            this.panel1.Location = new System.Drawing.Point(20, 18);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1215, 46);
            this.panel1.TabIndex = 1;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.Red;
            this.lblText.Location = new System.Drawing.Point(6, 14);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(64, 22);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "label1";
            // 
            // MeterConsumerFileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1156, 862);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lngFileLists);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MeterConsumerFileList";
            this.StatusMessage = "";
            this.Text = "MeterFileList";
            this.Load += new System.EventHandler(this.MeterFileList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABGridControl lngFileLists;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblText;
    }
}
