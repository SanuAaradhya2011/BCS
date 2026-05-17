namespace CAB.UI
{
    partial class CMRICommunication
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrepareCMRI = new CAB.UI.Controls.CABButton();
            this.btnBrowse = new CAB.UI.Controls.CABButton();
            this.btnReadData = new CAB.UI.Controls.CABButton();
            this.btnpreparesmarthhu = new CAB.UI.Controls.CABButton();
            this.btnUpdateRTC = new CAB.UI.Controls.CABButton();
            this.btnClearCMRI = new CAB.UI.Controls.CABButton();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.lblTOUFile = new CAB.UI.Controls.CABLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            // --- New layout panels ---
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFormSubtitle = new System.Windows.Forms.Label();
            this.pnlAccentBar = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlPrimaryActions = new System.Windows.Forms.Panel();
            this.lblSectionPrimary = new System.Windows.Forms.Label();
            this.pnlSecondaryActions = new System.Windows.Forms.Panel();
            this.lblSectionSecondary = new System.Windows.Forms.Label();
            this.pnlTouRow = new System.Windows.Forms.Panel();
            this.lblTouPath = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlPrimaryActions.SuspendLayout();
            this.pnlSecondaryActions.SuspendLayout();
            this.pnlTouRow.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // =====================================================================
            // HEADER PANEL  — deep blue branding bar
            // =====================================================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 90;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            this.pnlHeader.Controls.Add(this.lblFormSubtitle);
            this.pnlHeader.Controls.Add(this.lblFormTitle);

            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(24, 16);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Text = "CMRI Communication";

            this.lblFormSubtitle.AutoSize = true;
            this.lblFormSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormSubtitle.ForeColor = System.Drawing.Color.FromArgb(180, 220, 255);
            this.lblFormSubtitle.Location = new System.Drawing.Point(26, 52);
            this.lblFormSubtitle.Name = "lblFormSubtitle";
            this.lblFormSubtitle.Text = "Manage meter readout, configuration and RTC synchronization";

            // =====================================================================
            // ACCENT BAR — thin blue-to-teal separator line
            // =====================================================================
            this.pnlAccentBar.BackColor = System.Drawing.Color.FromArgb(0, 160, 190);
            this.pnlAccentBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAccentBar.Height = 3;
            this.pnlAccentBar.Name = "pnlAccentBar";

            // =====================================================================
            // MAIN CONTENT PANEL
            // =====================================================================
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(20, 18, 20, 12);
            this.pnlContent.Controls.Add(this.pnlFooter);
            this.pnlContent.Controls.Add(this.pnlTouRow);
            this.pnlContent.Controls.Add(this.pnlSecondaryActions);
            this.pnlContent.Controls.Add(this.lblSectionSecondary);
            this.pnlContent.Controls.Add(this.pnlPrimaryActions);
            this.pnlContent.Controls.Add(this.lblSectionPrimary);

            // =====================================================================
            // SECTION LABEL — Primary Actions
            // =====================================================================
            this.lblSectionPrimary.AutoSize = true;
            this.lblSectionPrimary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionPrimary.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblSectionPrimary.Location = new System.Drawing.Point(20, 20);
            this.lblSectionPrimary.Name = "lblSectionPrimary";
            this.lblSectionPrimary.Text = "PRIMARY OPERATIONS";

            // =====================================================================
            // PRIMARY ACTIONS CARD
            // =====================================================================
            this.pnlPrimaryActions.BackColor = System.Drawing.Color.White;
            this.pnlPrimaryActions.Location = new System.Drawing.Point(20, 42);
            this.pnlPrimaryActions.Name = "pnlPrimaryActions";
            this.pnlPrimaryActions.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlPrimaryActions.Size = new System.Drawing.Size(538, 122);
            this.pnlPrimaryActions.Controls.Add(this.btnPrepareCMRI);
            this.pnlPrimaryActions.Controls.Add(this.btnBrowse);
            this.pnlPrimaryActions.Controls.Add(this.btnReadData);

            // --- Prepare CMRI (primary CTA) ---
            this.btnPrepareCMRI.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPrepareCMRI.FlatAppearance.BorderSize = 0;
            this.btnPrepareCMRI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrepareCMRI.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrepareCMRI.ForeColor = System.Drawing.Color.White;
            this.btnPrepareCMRI.Location = new System.Drawing.Point(16, 14);
            this.btnPrepareCMRI.Name = "btnPrepareCMRI";
            this.btnPrepareCMRI.Size = new System.Drawing.Size(290, 44);
            this.btnPrepareCMRI.TabIndex = 0;
            this.btnPrepareCMRI.Text = "\u2B07  Prepare CMRI";
            this.btnPrepareCMRI.TranslationKey = null;
            this.btnPrepareCMRI.UseVisualStyleBackColor = false;
            this.btnPrepareCMRI.Click += new System.EventHandler(this.btnPrepareCMRI_Click);

            // --- Browse (secondary, beside Prepare) ---
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 215);
            this.btnBrowse.FlatAppearance.BorderSize = 1;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.btnBrowse.Location = new System.Drawing.Point(318, 14);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(204, 44);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "\U0001F4C1  Browse File...";
            this.btnBrowse.TranslationKey = null;
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // --- Read Data ---
            this.btnReadData.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnReadData.FlatAppearance.BorderSize = 0;
            this.btnReadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadData.ForeColor = System.Drawing.Color.White;
            this.btnReadData.Location = new System.Drawing.Point(16, 68);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(506, 40);
            this.btnReadData.TabIndex = 2;
            this.btnReadData.Text = "\u25B6  Read Data from Meter";
            this.btnReadData.TranslationKey = null;
            this.btnReadData.UseVisualStyleBackColor = false;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);

            // =====================================================================
            // SECTION LABEL — Secondary Actions
            // =====================================================================
            this.lblSectionSecondary.AutoSize = true;
            this.lblSectionSecondary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionSecondary.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblSectionSecondary.Location = new System.Drawing.Point(20, 180);
            this.lblSectionSecondary.Name = "lblSectionSecondary";
            this.lblSectionSecondary.Text = "DEVICE MANAGEMENT";

            // =====================================================================
            // SECONDARY ACTIONS CARD
            // =====================================================================
            this.pnlSecondaryActions.BackColor = System.Drawing.Color.White;
            this.pnlSecondaryActions.Location = new System.Drawing.Point(20, 202);
            this.pnlSecondaryActions.Name = "pnlSecondaryActions";
            this.pnlSecondaryActions.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlSecondaryActions.Size = new System.Drawing.Size(538, 172);
            this.pnlSecondaryActions.Controls.Add(this.btnpreparesmarthhu);
            this.pnlSecondaryActions.Controls.Add(this.btnUpdateRTC);
            this.pnlSecondaryActions.Controls.Add(this.btnClearCMRI);
            this.pnlSecondaryActions.Controls.Add(this.lblTOUFile);

            // --- Prepare Smart HHU ---
            this.btnpreparesmarthhu.BackColor = System.Drawing.Color.White;
            this.btnpreparesmarthhu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 215);
            this.btnpreparesmarthhu.FlatAppearance.BorderSize = 1;
            this.btnpreparesmarthhu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpreparesmarthhu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpreparesmarthhu.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.btnpreparesmarthhu.Location = new System.Drawing.Point(16, 14);
            this.btnpreparesmarthhu.Name = "btnpreparesmarthhu";
            this.btnpreparesmarthhu.Size = new System.Drawing.Size(506, 40);
            this.btnpreparesmarthhu.TabIndex = 3;
            this.btnpreparesmarthhu.Text = "\uD83D\uDCF1  Prepare Smart HHU";
            this.btnpreparesmarthhu.TranslationKey = null;
            this.btnpreparesmarthhu.UseVisualStyleBackColor = false;
            this.btnpreparesmarthhu.Click += new System.EventHandler(this.btnpreparesmarthhu_Click);

            // --- Update RTC ---
            this.btnUpdateRTC.BackColor = System.Drawing.Color.White;
            this.btnUpdateRTC.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 210, 215);
            this.btnUpdateRTC.FlatAppearance.BorderSize = 1;
            this.btnUpdateRTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateRTC.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateRTC.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.btnUpdateRTC.Location = new System.Drawing.Point(16, 64);
            this.btnUpdateRTC.Name = "btnUpdateRTC";
            this.btnUpdateRTC.Size = new System.Drawing.Size(506, 40);
            this.btnUpdateRTC.TabIndex = 4;
            this.btnUpdateRTC.Text = "\uD83D\uDD52  Synchronize RTC Clock";
            this.btnUpdateRTC.TranslationKey = null;
            this.btnUpdateRTC.UseVisualStyleBackColor = false;
            this.btnUpdateRTC.Click += new System.EventHandler(this.btnUpdateRTC_Click);

            // --- Clear CMRI (destructive — amber tint) ---
            this.btnClearCMRI.BackColor = System.Drawing.Color.White;
            this.btnClearCMRI.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(230, 180, 50);
            this.btnClearCMRI.FlatAppearance.BorderSize = 1;
            this.btnClearCMRI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCMRI.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearCMRI.ForeColor = System.Drawing.Color.FromArgb(140, 90, 0);
            this.btnClearCMRI.Location = new System.Drawing.Point(16, 114);
            this.btnClearCMRI.Name = "btnClearCMRI";
            this.btnClearCMRI.Size = new System.Drawing.Size(506, 40);
            this.btnClearCMRI.TabIndex = 5;
            this.btnClearCMRI.Text = "\u26A0  Clear CMRI Data";
            this.btnClearCMRI.TranslationKey = null;
            this.btnClearCMRI.UseVisualStyleBackColor = false;
            this.btnClearCMRI.Click += new System.EventHandler(this.btnClearCMRI_Click);

            // --- lblTOUFile (invisible, functional) ---
            this.lblTOUFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTOUFile.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblTOUFile.Location = new System.Drawing.Point(16, 154);
            this.lblTOUFile.Name = "lblTOUFile";
            this.lblTOUFile.Size = new System.Drawing.Size(400, 18);
            this.lblTOUFile.TabIndex = 7;
            this.lblTOUFile.TranslationKey = null;
            this.lblTOUFile.Visible = false;

            // =====================================================================
            // TOU PATH ROW (optional hint row)
            // =====================================================================
            this.pnlTouRow.BackColor = System.Drawing.Color.FromArgb(235, 245, 255);
            this.pnlTouRow.Location = new System.Drawing.Point(20, 390);
            this.pnlTouRow.Name = "pnlTouRow";
            this.pnlTouRow.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.pnlTouRow.Size = new System.Drawing.Size(538, 36);
            this.pnlTouRow.Visible = false;
            this.pnlTouRow.Controls.Add(this.lblTouPath);

            this.lblTouPath.AutoSize = true;
            this.lblTouPath.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTouPath.ForeColor = System.Drawing.Color.FromArgb(0, 90, 170);
            this.lblTouPath.Location = new System.Drawing.Point(14, 8);
            this.lblTouPath.Name = "lblTouPath";
            this.lblTouPath.Text = "Selected file: (none)";

            // =====================================================================
            // FOOTER PANEL — Cancel button
            // =====================================================================
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 62;
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlFooter.Controls.Add(this.btnCancel);

            // --- Cancel ---
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(240, 240, 242);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.btnCancel.Location = new System.Drawing.Point(398, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.TranslationKey = null;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // =====================================================================
            // groupBox1 — hidden compat wrapper (keeps code-behind references valid)
            // =====================================================================
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            // Note: child controls moved to pnl* panels above; groupBox1 kept for compat
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(0, 0);
            this.groupBox1.TabIndex = 99;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "";
            this.groupBox1.Visible = false;

            // =====================================================================
            // STATUS STRIP + PROGRESS
            // =====================================================================
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(0, 100, 180);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar });
            this.statusStrip.Location = new System.Drawing.Point(0, 548);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(578, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Visible = false;

            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 14);

            // =====================================================================
            // FORM — CMRICommunication
            // =====================================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.ClientSize = new System.Drawing.Size(578, 570);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlAccentBar);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CMRICommunication";
            this.StatusMessage = "";
            this.Text = "CMRI Communication";
            this.groupBox1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlPrimaryActions.ResumeLayout(false);
            this.pnlSecondaryActions.ResumeLayout(false);
            this.pnlTouRow.ResumeLayout(false);
            this.pnlTouRow.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABButton btnPrepareCMRI;
        private CAB.UI.Controls.CABButton btnBrowse;
        private CAB.UI.Controls.CABButton btnCancel;
        private CAB.UI.Controls.CABButton btnUpdateRTC;
        private CAB.UI.Controls.CABButton btnReadData;
        private CAB.UI.Controls.CABLabel lblTOUFile;
        private CAB.UI.Controls.CABButton btnClearCMRI;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Timer progressBarTimer;
        private CAB.UI.Controls.CABButton btnpreparesmarthhu;

        // New layout controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFormSubtitle;
        private System.Windows.Forms.Panel pnlAccentBar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlPrimaryActions;
        private System.Windows.Forms.Label lblSectionPrimary;
        private System.Windows.Forms.Panel pnlSecondaryActions;
        private System.Windows.Forms.Label lblSectionSecondary;
        private System.Windows.Forms.Panel pnlTouRow;
        private System.Windows.Forms.Label lblTouPath;
        private System.Windows.Forms.Panel pnlFooter;
    }
}