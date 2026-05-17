namespace CAB.UI
{
    partial class ApplicationLogDetails
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lngscApplicationLogDetails = new CAB.UI.Controls.CABSearchControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdActivityLogDetails = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityLogDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lngscApplicationLogDetails);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 417);
            this.panel1.TabIndex = 0;
            // 
            // lngscApplicationLogDetails
            // 
            this.lngscApplicationLogDetails.HideMainSearch = true;
            this.lngscApplicationLogDetails.Location = new System.Drawing.Point(3, 3);
            this.lngscApplicationLogDetails.Margin = new System.Windows.Forms.Padding(4);
            this.lngscApplicationLogDetails.Name = "lngscApplicationLogDetails";
            this.lngscApplicationLogDetails.PrimaryNonComboData = null;
            this.lngscApplicationLogDetails.PrimarySearchTypeComboData = "";
            this.lngscApplicationLogDetails.PrimarySearchTypeData = null;
            this.lngscApplicationLogDetails.SearchRequire = true;
            this.lngscApplicationLogDetails.SecondaryNonComboData = null;
            this.lngscApplicationLogDetails.SecondarySearchTypeComboData = "";
            this.lngscApplicationLogDetails.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.lngscApplicationLogDetails.SecondarySearchTypeTextData = "";
            this.lngscApplicationLogDetails.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.lngscApplicationLogDetails.Size = new System.Drawing.Size(749, 32);
            this.lngscApplicationLogDetails.TabIndex = 4;
            this.lngscApplicationLogDetails.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.lngscApplicationLogDetails_OnFindNowClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdActivityLogDetails);
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(740, 353);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activity Details";
            // 
            // grdActivityLogDetails
            // 
            this.grdActivityLogDetails.AllowUserToAddRows = false;
            this.grdActivityLogDetails.AllowUserToDeleteRows = false;
            this.grdActivityLogDetails.AllowUserToResizeColumns = false;
            this.grdActivityLogDetails.AllowUserToResizeRows = false;
            this.grdActivityLogDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdActivityLogDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdActivityLogDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActivityLogDetails.Location = new System.Drawing.Point(3, 16);
            this.grdActivityLogDetails.Margin = new System.Windows.Forms.Padding(0);
            this.grdActivityLogDetails.Name = "grdActivityLogDetails";
            this.grdActivityLogDetails.ReadOnly = true;
            this.grdActivityLogDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdActivityLogDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdActivityLogDetails.RowTemplate.Height = 24;
            this.grdActivityLogDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdActivityLogDetails.Size = new System.Drawing.Size(734, 334);
            this.grdActivityLogDetails.TabIndex = 0;
            // 
            // ApplicationLogDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(763, 417);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ApplicationLogDetails";
            this.StatusMessage = "";
            this.Text = "Application Log Details";
            this.Load += new System.EventHandler(this.ApplicationLogDetails_Load);
            this.Activated += new System.EventHandler(this.ApplicationLogDetails_Activated);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityLogDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridView grdActivityLogDetails;
		private CAB.UI.Controls.CABSearchControl lngscApplicationLogDetails;


	}
}