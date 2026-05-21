namespace CAB.UI
{
    partial class DTMDailyProfileSelectForm
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
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkBoxListDailyProfile = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(113, 284);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(194, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkBoxListDailyProfile
            // 
            this.chkBoxListDailyProfile.CheckOnClick = true;
            this.chkBoxListDailyProfile.FormattingEnabled = true;
            this.chkBoxListDailyProfile.Items.AddRange(new object[] {
            "Cumulative kWh",
            "Cumulative kVArh Lag",
            "Cumulative kVArh Lead",
            "Cumulative kVAh",
            "Daily MD1 (kW)",
            "MD1 TimeStamp",
            "Daily MD2 (kVA)",
            "MD2 TimeStamp",
            "Power On Hours"});
            this.chkBoxListDailyProfile.Location = new System.Drawing.Point(12, 31);
            this.chkBoxListDailyProfile.Name = "chkBoxListDailyProfile";
            this.chkBoxListDailyProfile.Size = new System.Drawing.Size(257, 229);
            this.chkBoxListDailyProfile.TabIndex = 5;
            this.chkBoxListDailyProfile.SelectedIndexChanged += new System.EventHandler(this.chkBoxListDailyProfile_SelectedIndexChanged);
            this.chkBoxListDailyProfile.KeyUp += new System.Windows.Forms.KeyEventHandler(this.chkBoxListDailyProfile_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select Daily Profile Parameters : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Note : Maximum of 8 Parameters can be selected";
            // 
            // DTMDailyProfileSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(281, 313);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkBoxListDailyProfile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DTMDailyProfileSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daily Profile Parameters";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DTMDailyProfileSelectForm_FormClosed);
            this.Load += new System.EventHandler(this.DTMDailyProfileSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckedListBox chkBoxListDailyProfile;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}