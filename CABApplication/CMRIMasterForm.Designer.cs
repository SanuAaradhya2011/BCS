namespace CAB.UI
{
    partial class CMRIMasterForm
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
            this.panelHeader = new CAB.UI.Controls.RoundedPanel();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelToolbarCard = new CAB.UI.Controls.RoundedPanel();
            this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
            this.panelContentCard = new CAB.UI.Controls.RoundedPanel();
            this.ucDetail = new CAB.UI.CMRIMasterControl();
            this.ucGridControl = new CAB.UI.Controls.CABGridControl();
            this.panelHeader.SuspendLayout();
            this.panelToolbarCard.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(253)))));
            this.panelHeader.BorderColor = System.Drawing.Color.Transparent;
            this.panelHeader.BorderRadius = 0;
            this.panelHeader.Controls.Add(this.labelSubtitle);
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelHeader.Location = new System.Drawing.Point(26, 24);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(5);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1993, 118);
            this.panelHeader.TabIndex = 0;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.AutoSize = true;
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(108)))), ((int)(((byte)(132)))));
            this.labelSubtitle.Location = new System.Drawing.Point(4, 71);
            this.labelSubtitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(614, 25);
            this.labelSubtitle.TabIndex = 1;
            this.labelSubtitle.Text = "Create, update and manage CMRI definitions from a cleaner workspace.";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(45)))));
            this.labelTitle.Location = new System.Drawing.Point(0, 6);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(349, 48);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "📟  CMRI Definition";
            // 
            // panelToolbarCard
            // 
            this.panelToolbarCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelToolbarCard.BackColor = System.Drawing.Color.White;
            this.panelToolbarCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panelToolbarCard.BorderThickness = 1;
            this.panelToolbarCard.Controls.Add(this.ucSearchControl);
            this.panelToolbarCard.Location = new System.Drawing.Point(26, 153);
            this.panelToolbarCard.Margin = new System.Windows.Forms.Padding(5);
            this.panelToolbarCard.Name = "panelToolbarCard";
            this.panelToolbarCard.Padding = new System.Windows.Forms.Padding(26, 24, 26, 24);
            this.panelToolbarCard.Size = new System.Drawing.Size(1993, 94);
            this.panelToolbarCard.TabIndex = 1;
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.BackColor = System.Drawing.Color.OldLace;
            this.ucSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl.HideMainSearch = false;
            this.ucSearchControl.Location = new System.Drawing.Point(26, 24);
            this.ucSearchControl.Margin = new System.Windows.Forms.Padding(6);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = false;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(1941, 46);
            this.ucSearchControl.TabIndex = 0;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // panelContentCard
            // 
            this.panelContentCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContentCard.BackColor = System.Drawing.Color.White;
            this.panelContentCard.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panelContentCard.BorderThickness = 1;
            this.panelContentCard.Controls.Add(this.ucDetail);
            this.panelContentCard.Controls.Add(this.ucGridControl);
            this.panelContentCard.Location = new System.Drawing.Point(26, 243);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(5);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(26, 24, 26, 24);
            this.panelContentCard.Size = new System.Drawing.Size(1623, 788);
            this.panelContentCard.TabIndex = 2;
            // 
            // ucDetail
            // 
            this.ucDetail.BackColor = System.Drawing.Color.FloralWhite;
            this.ucDetail.BackgroundImage = global::CABApplication.Properties.Resources.bakgroundmain;
            this.ucDetail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDetail.Enabled = true;
            this.ucDetail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucDetail.Location = new System.Drawing.Point(26, 24);
            this.ucDetail.Margin = new System.Windows.Forms.Padding(6);
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.Size = new System.Drawing.Size(1595, 740);
            this.ucDetail.StatusMessage = "";
            this.ucDetail.TabIndex = 1;
            this.ucDetail.OnControlStatusChanged += new CAB.UI.CMRIMasterControl.ControlStatusChanged(this.ucDetail_OnControlStatusChanged);
            this.ucDetail.OnCancelClick += new CAB.UI.CMRIMasterControl.CancelClickHandler(this.ucDetail_OnCancelClick);
            this.ucDetail.OnSaveClick += new CAB.UI.CMRIMasterControl.SaveClickHandler(this.ucDetail_OnSaveClick);
            this.ucDetail.Load += new System.EventHandler(this.ucDetail_Load);
            // 
            // ucGridControl
            // 
            this.ucGridControl.AutoScroll = true;
            this.ucGridControl.Data = null;
            this.ucGridControl.HiddenColumn = null;
            this.ucGridControl.HiddenColumns = null;
            this.ucGridControl.IsEqual = true;
            this.ucGridControl.IsFullRow = true;
            this.ucGridControl.IsSorting = true;
            this.ucGridControl.Location = new System.Drawing.Point(26, 24);
            this.ucGridControl.Margin = new System.Windows.Forms.Padding(6);
            this.ucGridControl.Name = "ucGridControl";
            this.ucGridControl.SelectedIndex = 0;
            this.ucGridControl.SelectedRowId = "";
            this.ucGridControl.Size = new System.Drawing.Size(1595, 770);
            this.ucGridControl.TabIndex = 0;
            this.ucGridControl.ValueColumn = null;
            // 
            // CMRIMasterForm  (AutoScroll removed — was causing panelContentCard to retain
            //                  design-time 1924×1170 virtual size in small MDI windows,
            //                  pushing the GroupBox below the visible area.)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1924, 1170);
            this.Controls.Add(this.panelContentCard);
            this.Controls.Add(this.panelToolbarCard);
            this.Controls.Add(this.panelHeader);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CMRIMasterForm";
            this.StatusMessage = "";
            this.Text = "CMRI Definition";
            this.Activated += new System.EventHandler(this.CMRIMasterForm_Activated);
            this.Load += new System.EventHandler(this.CMRIMasterForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelToolbarCard.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.RoundedPanel panelHeader;
        private System.Windows.Forms.Label labelSubtitle;
        private System.Windows.Forms.Label labelTitle;
        private CAB.UI.Controls.RoundedPanel panelToolbarCard;
        private CAB.UI.Controls.RoundedPanel panelContentCard;
        private CAB.UI.Controls.CABSearchControl ucSearchControl;
        private CAB.UI.Controls.CABGridControl ucGridControl;
        private CMRIMasterControl ucDetail;
    }
}
