using CAB.UI.Controls;
namespace CAB.UI
{
    partial class UserInformations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInformations));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelPageSubtitle = new System.Windows.Forms.Label();
            this.labelPageTitle = new System.Windows.Forms.Label();
            this.panelSearchCard = new System.Windows.Forms.Panel();
            this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
            this.panelContentCard = new System.Windows.Forms.Panel();
            this.ucGridControl = new CAB.UI.Controls.CABGridControl();
            this.panelHeader.SuspendLayout();
            this.panelSearchCard.SuspendLayout();
            this.panelContentCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelHeader.Controls.Add(this.labelPageSubtitle);
            this.panelHeader.Controls.Add(this.labelPageTitle);
            this.panelHeader.Location = new System.Drawing.Point(26, 21);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1517, 107);
            this.panelHeader.TabIndex = 0;
            // 
            // labelPageSubtitle
            // 
            this.labelPageSubtitle.AutoSize = true;
            this.labelPageSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPageSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(108)))), ((int)(((byte)(132)))));
            this.labelPageSubtitle.Location = new System.Drawing.Point(3, 61);
            this.labelPageSubtitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPageSubtitle.Name = "labelPageSubtitle";
            this.labelPageSubtitle.Size = new System.Drawing.Size(595, 28);
            this.labelPageSubtitle.TabIndex = 1;
            this.labelPageSubtitle.Text = "Search, review and maintain user profiles from a cleaner workspace.";
            // 
            // labelPageTitle
            // 
            this.labelPageTitle.AutoSize = true;
            this.labelPageTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(45)))));
            this.labelPageTitle.Location = new System.Drawing.Point(0, 5);
            this.labelPageTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPageTitle.Name = "labelPageTitle";
            this.labelPageTitle.Size = new System.Drawing.Size(298, 48);
            this.labelPageTitle.TabIndex = 0;
            this.labelPageTitle.Text = "User Information";
            this.labelPageTitle.Click += new System.EventHandler(this.labelPageTitle_Click);
            // 
            // panelSearchCard
            // 
            this.panelSearchCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchCard.BackColor = System.Drawing.Color.White;
            this.panelSearchCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearchCard.Controls.Add(this.ucSearchControl);
            this.panelSearchCard.Location = new System.Drawing.Point(26, 141);
            this.panelSearchCard.Margin = new System.Windows.Forms.Padding(4);
            this.panelSearchCard.Name = "panelSearchCard";
            this.panelSearchCard.Padding = new System.Windows.Forms.Padding(23, 21, 23, 21);
            this.panelSearchCard.Size = new System.Drawing.Size(1517, 87);
            this.panelSearchCard.TabIndex = 1;
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.BackColor = System.Drawing.Color.White;
            this.ucSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl.HideMainSearch = true;
            this.ucSearchControl.Location = new System.Drawing.Point(23, 21);
            this.ucSearchControl.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = true;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(1469, 43);
            this.ucSearchControl.TabIndex = 0;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            this.ucSearchControl.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.ucSearchControl_OnFindNowClick);
            // 
            // panelContentCard
            // 
            this.panelContentCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContentCard.BackColor = System.Drawing.Color.White;
            this.panelContentCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelContentCard.Controls.Add(this.ucGridControl);
            this.panelContentCard.Location = new System.Drawing.Point(26, 253);
            this.panelContentCard.Margin = new System.Windows.Forms.Padding(4);
            this.panelContentCard.Name = "panelContentCard";
            this.panelContentCard.Padding = new System.Windows.Forms.Padding(23, 24, 23, 24);
            this.panelContentCard.Size = new System.Drawing.Size(1517, 733);
            this.panelContentCard.TabIndex = 2;
            // 
            // ucGridControl
            // 
            this.ucGridControl.Data = null;
            this.ucGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGridControl.HiddenColumn = null;
            this.ucGridControl.HiddenColumns = null;
            this.ucGridControl.IsEqual = false;
            this.ucGridControl.IsFullRow = false;
            this.ucGridControl.IsSorting = true;
            this.ucGridControl.Location = new System.Drawing.Point(23, 24);
            this.ucGridControl.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.ucGridControl.Name = "ucGridControl";
            this.ucGridControl.SelectedIndex = 0;
            this.ucGridControl.SelectedRowId = "";
            this.ucGridControl.Size = new System.Drawing.Size(1469, 683);
            this.ucGridControl.TabIndex = 0;
            this.ucGridControl.ValueColumn = "UserInformation_ID";
            // 
            // UserInformations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1569, 1013);
            this.Controls.Add(this.panelContentCard);
            this.Controls.Add(this.panelSearchCard);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "UserInformations";
            this.StatusMessage = "";
            this.Text = "UserInformations";
            this.Activated += new System.EventHandler(this.UserInformations_Activated);
            this.Load += new System.EventHandler(this.UserInformations_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSearchCard.ResumeLayout(false);
            this.panelContentCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelPageSubtitle;
        private System.Windows.Forms.Label labelPageTitle;
        private System.Windows.Forms.Panel panelSearchCard;
        private System.Windows.Forms.Panel panelContentCard;
        private  CABSearchControl ucSearchControl;
        //private UserInformation ucDetail;
        private CABGridControl ucGridControl;
    }
}
