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
            this.panelContentCard = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdActivityLogDetails = new System.Windows.Forms.DataGridView();
            this.panelSearchCard = new System.Windows.Forms.Panel();
            this.lngscApplicationLogDetails = new CAB.UI.Controls.CABSearchControl();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityLogDetails)).BeginInit();
            this.panelSearchCard.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.panel1.Controls.Add(this.panelContentCard);
            this.panel1.Controls.Add(this.panelSearchCard);
            this.panel1.Controls.Add(this.panelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1337, 880);
            this.panel1.TabIndex = 0;
            // 
            // panelContentCard
            // 
            this.panelContentCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContentCard.BackColor = System.Drawing.Color.White;
            this.panelContentCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelContentCard.Controls.Add(this.groupBox1);
            this.panelContentCard.Location = new System.Drawing.Point(15, 181);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(4);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.panelContentCard.Size = new System.Drawing.Size(1306, 679);
            this.panelContentCard.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.grdActivityLogDetails);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(45)))));
            this.groupBox1.Location = new System.Drawing.Point(10, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10, 13, 10, 11);
            this.groupBox1.Size = new System.Drawing.Size(1284, 655);
            this.groupBox1.TabIndex = 0;
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
            this.grdActivityLogDetails.BackgroundColor = System.Drawing.Color.White;
            this.grdActivityLogDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdActivityLogDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdActivityLogDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdActivityLogDetails.ColumnHeadersHeight = 38;
            this.grdActivityLogDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdActivityLogDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActivityLogDetails.EnableHeadersVisualStyles = false;
            this.grdActivityLogDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.grdActivityLogDetails.Location = new System.Drawing.Point(10, 41);
            this.grdActivityLogDetails.Margin = new System.Windows.Forms.Padding(0);
            this.grdActivityLogDetails.Name = "grdActivityLogDetails";
            this.grdActivityLogDetails.ReadOnly = true;
            this.grdActivityLogDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdActivityLogDetails.RowHeadersVisible = false;
            this.grdActivityLogDetails.RowHeadersWidth = 62;
            this.grdActivityLogDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdActivityLogDetails.RowTemplate.Height = 32;
            this.grdActivityLogDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdActivityLogDetails.Size = new System.Drawing.Size(1264, 603);
            this.grdActivityLogDetails.TabIndex = 0;
            // 
            // panelSearchCard
            // 
            this.panelSearchCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.panelSearchCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearchCard.Controls.Add(this.lngscApplicationLogDetails);
            this.panelSearchCard.Location = new System.Drawing.Point(15, 98);
            this.panelSearchCard.Margin = new System.Windows.Forms.Padding(4);
            this.panelSearchCard.Name = "panelSearchCard";
            this.panelSearchCard.Padding = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.panelSearchCard.Size = new System.Drawing.Size(1306, 71);
            this.panelSearchCard.TabIndex = 1;
            // 
            // lngscApplicationLogDetails
            // 
            this.lngscApplicationLogDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.lngscApplicationLogDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lngscApplicationLogDetails.HideMainSearch = true;
            this.lngscApplicationLogDetails.Location = new System.Drawing.Point(15, 13);
            this.lngscApplicationLogDetails.Margin = new System.Windows.Forms.Padding(5);
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
            this.lngscApplicationLogDetails.Size = new System.Drawing.Size(1274, 43);
            this.lngscApplicationLogDetails.TabIndex = 0;
            this.lngscApplicationLogDetails.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.lngscApplicationLogDetails_OnFindNowClick);
            this.lngscApplicationLogDetails.Load += new System.EventHandler(this.lngscApplicationLogDetails_Load);
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelHeader.Controls.Add(this.labelSubtitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Location = new System.Drawing.Point(15, 13);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1306, 77);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.AutoSize = true;
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(108)))), ((int)(((byte)(132)))));
            this.labelSubtitle.Location = new System.Drawing.Point(3, 45);
            this.labelSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(540, 28);
            this.labelSubtitle.TabIndex = 1;
            this.labelSubtitle.Text = "Review application activity using the same search experience.";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(45)))));
            this.labelTitle.Location = new System.Drawing.Point(0, 5);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(400, 48);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Application Log Details";
            // 
            // ApplicationLogDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1337, 880);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ApplicationLogDetails";
            this.StatusMessage = "";
            this.Text = "Application Log Details";
            this.Activated += new System.EventHandler(this.ApplicationLogDetails_Activated);
            this.Load += new System.EventHandler(this.ApplicationLogDetails_Load);
            this.panel1.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdActivityLogDetails)).EndInit();
            this.panelSearchCard.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panelHeader;
		private System.Windows.Forms.Label labelSubtitle;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Panel panelSearchCard;
		private System.Windows.Forms.Panel panelContentCard;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridView grdActivityLogDetails;
		private CAB.UI.Controls.CABSearchControl lngscApplicationLogDetails;
    }
}
