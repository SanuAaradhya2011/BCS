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
            this.btnShow = new System.Windows.Forms.Button();
            this.lblTodate = new System.Windows.Forms.Label();
            this.lblFromdate = new System.Windows.Forms.Label();
            this.DtpickerTo = new System.Windows.Forms.DateTimePicker();
            this.DtpickerFrom = new System.Windows.Forms.DateTimePicker();
            this.lblText = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlAccent = new System.Windows.Forms.Panel();
            this.pnlGridCard = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // lngFileLists
            // 
            this.lngFileLists.AutoScroll = true;
            this.lngFileLists.BackColor = System.Drawing.Color.Linen;
            this.lngFileLists.BackgroundImage = global::CABApplication.Properties.Resources.Background;
            this.lngFileLists.Data = null;
            this.lngFileLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lngFileLists.HiddenColumn = null;
            this.lngFileLists.HiddenColumns = null;
            this.lngFileLists.IsEqual = true;
            this.lngFileLists.IsFullRow = false;
            this.lngFileLists.IsSorting = false;
            this.lngFileLists.Location = new System.Drawing.Point(1, 1);
            this.lngFileLists.Margin = new System.Windows.Forms.Padding(0);
            this.lngFileLists.Name = "lngFileLists";
            this.lngFileLists.SelectedIndex = 0;
            this.lngFileLists.SelectedRowId = "";
            this.lngFileLists.Size = new System.Drawing.Size(809, 380);
            this.lngFileLists.TabIndex = 0;
            this.lngFileLists.ValueColumn = null;
            this.lngFileLists.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lngFileLists_OnGridRowChanged);
            this.lngFileLists.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lngFileLists_OnGridMouseDoubleClick);
            this.lngFileLists.Load += new System.EventHandler(this.lngFileLists_Load);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.lblTodate);
            this.panel1.Controls.Add(this.lblFromdate);
            this.panel1.Controls.Add(this.DtpickerTo);
            this.panel1.Controls.Add(this.DtpickerFrom);
            this.panel1.Controls.Add(this.lblText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 59);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.panel1.Size = new System.Drawing.Size(835, 48);
            this.panel1.TabIndex = 2;
            // 
            // btnShow
            // 
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnShow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.Location = new System.Drawing.Point(700, 9);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(90, 30);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "â–¶  Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.Visible = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // lblTodate
            // 
            this.lblTodate.AutoSize = true;
            this.lblTodate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.lblTodate.Location = new System.Drawing.Point(545, 16);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(30, 25);
            this.lblTodate.TabIndex = 4;
            this.lblTodate.Text = "To";
            this.lblTodate.Visible = false;
            // 
            // lblFromdate
            // 
            this.lblFromdate.AutoSize = true;
            this.lblFromdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.lblFromdate.Location = new System.Drawing.Point(380, 16);
            this.lblFromdate.Name = "lblFromdate";
            this.lblFromdate.Size = new System.Drawing.Size(54, 25);
            this.lblFromdate.TabIndex = 3;
            this.lblFromdate.Text = "From";
            this.lblFromdate.Visible = false;
            // 
            // DtpickerTo
            // 
            this.DtpickerTo.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.DtpickerTo.CustomFormat = "dd-MMM-yyyy";
            this.DtpickerTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DtpickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpickerTo.Location = new System.Drawing.Point(570, 12);
            this.DtpickerTo.Name = "DtpickerTo";
            this.DtpickerTo.Size = new System.Drawing.Size(115, 31);
            this.DtpickerTo.TabIndex = 2;
            this.DtpickerTo.Visible = false;
            // 
            // DtpickerFrom
            // 
            this.DtpickerFrom.CalendarFont = new System.Drawing.Font("Segoe UI", 9F);
            this.DtpickerFrom.CustomFormat = "dd-MMM-yyyy";
            this.DtpickerFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DtpickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpickerFrom.Location = new System.Drawing.Point(420, 12);
            this.DtpickerFrom.Name = "DtpickerFrom";
            this.DtpickerFrom.Size = new System.Drawing.Size(115, 31);
            this.DtpickerFrom.TabIndex = 1;
            this.DtpickerFrom.Visible = false;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblText.Location = new System.Drawing.Point(16, 13);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(76, 30);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "label1";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.pnlHeader.Size = new System.Drawing.Size(835, 56);
            this.pnlHeader.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(218, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Meter File List";
            // 
            // pnlAccent
            // 
            this.pnlAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(160)))), ((int)(((byte)(190)))));
            this.pnlAccent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAccent.Location = new System.Drawing.Point(0, 56);
            this.pnlAccent.Name = "pnlAccent";
            this.pnlAccent.Size = new System.Drawing.Size(835, 3);
            this.pnlAccent.TabIndex = 11;
            // 
            // pnlGridCard
            // 
            this.pnlGridCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGridCard.BackColor = System.Drawing.Color.White;
            this.pnlGridCard.Controls.Add(this.lngFileLists);
            this.pnlGridCard.Location = new System.Drawing.Point(12, 115);
            this.pnlGridCard.Name = "pnlGridCard";
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(1);
            this.pnlGridCard.Size = new System.Drawing.Size(811, 382);
            this.pnlGridCard.TabIndex = 10;
            // 
            // MeterFileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(835, 509);
            this.Controls.Add(this.pnlGridCard);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlAccent);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MeterFileList";
            this.StatusMessage = "";
            this.Text = "Meter File List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MeterFileList_FormClosed);
            this.Load += new System.EventHandler(this.MeterFileList_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlGridCard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABGridControl lngFileLists;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblText;
        internal System.Windows.Forms.Label lblTodate;
        internal System.Windows.Forms.Label lblFromdate;
        internal System.Windows.Forms.DateTimePicker DtpickerFrom;
        internal System.Windows.Forms.Button btnShow;
        internal System.Windows.Forms.DateTimePicker DtpickerTo;

        // New layout controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlAccent;
        private System.Windows.Forms.Panel pnlGridCard;
    }
}