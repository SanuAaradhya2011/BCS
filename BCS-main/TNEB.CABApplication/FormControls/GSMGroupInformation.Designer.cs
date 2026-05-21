namespace CAB.UI
{
    partial class GSMGroupInformation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpHead = new System.Windows.Forms.GroupBox();
            this.cboSchedule = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstSelectedMeter = new System.Windows.Forms.ListBox();
            this.lstAllMeter = new System.Windows.Forms.ListBox();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.lngLabel3 = new CAB.UI.Controls.CABLabel();
            this.lngLabel4 = new CAB.UI.Controls.CABLabel();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.lngButton6 = new CAB.UI.Controls.CABButton();
            this.lngButton5 = new CAB.UI.Controls.CABButton();
            this.lngButton7 = new CAB.UI.Controls.CABButton();
            this.lngButton8 = new CAB.UI.Controls.CABButton();
            this.lngLabel5 = new CAB.UI.Controls.CABLabel();
            this.lngLabel7 = new CAB.UI.Controls.CABLabel();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.btnSave = new CAB.UI.Controls.CABButton();
            this.grpHead.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHead
            // 
            this.grpHead.Controls.Add(this.cboSchedule);
            this.grpHead.Controls.Add(this.dtpDate);
            this.grpHead.Controls.Add(this.panel1);
            this.grpHead.Controls.Add(this.panel2);
            this.grpHead.Controls.Add(this.lngLabel1);
            this.grpHead.Controls.Add(this.lngButton6);
            this.grpHead.Controls.Add(this.lngButton5);
            this.grpHead.Controls.Add(this.lngButton7);
            this.grpHead.Controls.Add(this.lngButton8);
            this.grpHead.Controls.Add(this.lstSelectedMeter);
            this.grpHead.Controls.Add(this.lstAllMeter);
            this.grpHead.Controls.Add(this.lngLabel5);
            this.grpHead.Controls.Add(this.txtGroupName);
            this.grpHead.Controls.Add(this.lngLabel7);
            this.grpHead.Location = new System.Drawing.Point(4, 4);
            this.grpHead.Name = "grpHead";
            this.grpHead.Size = new System.Drawing.Size(778, 476);
            this.grpHead.TabIndex = 0;
            this.grpHead.TabStop = false;
            // 
            // cboSchedule
            // 
            this.cboSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSchedule.FormattingEnabled = true;
            this.cboSchedule.Location = new System.Drawing.Point(142, 99);
            this.cboSchedule.Margin = new System.Windows.Forms.Padding(4);
            this.cboSchedule.Name = "cboSchedule";
            this.cboSchedule.Size = new System.Drawing.Size(175, 24);
            this.cboSchedule.TabIndex = 69;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(142, 64);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(110, 22);
            this.dtpDate.TabIndex = 68;
            this.dtpDate.Value = new System.DateTime(2010, 8, 7, 0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.lngLabel3);
            this.panel1.Location = new System.Drawing.Point(451, 131);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(318, 25);
            this.panel1.TabIndex = 67;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.lngLabel4);
            this.panel2.Location = new System.Drawing.Point(8, 131);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(309, 25);
            this.panel2.TabIndex = 66;
            // 
            // lstSelectedMeter
            // 
            this.lstSelectedMeter.FormattingEnabled = true;
            this.lstSelectedMeter.ItemHeight = 16;
            this.lstSelectedMeter.Location = new System.Drawing.Point(451, 157);
            this.lstSelectedMeter.Margin = new System.Windows.Forms.Padding(4);
            this.lstSelectedMeter.Name = "lstSelectedMeter";
            this.lstSelectedMeter.Size = new System.Drawing.Size(318, 308);
            this.lstSelectedMeter.TabIndex = 60;
            // 
            // lstAllMeter
            // 
            this.lstAllMeter.FormattingEnabled = true;
            this.lstAllMeter.ItemHeight = 16;
            this.lstAllMeter.Location = new System.Drawing.Point(8, 157);
            this.lstAllMeter.Margin = new System.Windows.Forms.Padding(4);
            this.lstAllMeter.Name = "lstAllMeter";
            this.lstAllMeter.Size = new System.Drawing.Size(309, 308);
            this.lstAllMeter.TabIndex = 59;
            this.lstAllMeter.SelectedIndexChanged += new System.EventHandler(this.lstAllMeter_SelectedIndexChanged);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(142, 30);
            this.txtGroupName.Margin = new System.Windows.Forms.Padding(4);
            this.txtGroupName.MaxLength = 35;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(175, 22);
            this.txtGroupName.TabIndex = 55;
            // 
            // lngLabel3
            // 
            this.lngLabel3.AutoSize = true;
            this.lngLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel3.Location = new System.Drawing.Point(2, 4);
            this.lngLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel3.Name = "lngLabel3";
            this.lngLabel3.Size = new System.Drawing.Size(117, 17);
            this.lngLabel3.TabIndex = 32;
            this.lngLabel3.Text = "Selected Meter";
            this.lngLabel3.TranslationKey = "";
            // 
            // lngLabel4
            // 
            this.lngLabel4.AutoSize = true;
            this.lngLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel4.Location = new System.Drawing.Point(3, 4);
            this.lngLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel4.Name = "lngLabel4";
            this.lngLabel4.Size = new System.Drawing.Size(120, 17);
            this.lngLabel4.TabIndex = 31;
            this.lngLabel4.Text = "Available Meter";
            this.lngLabel4.TranslationKey = "";
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(5, 99);
            this.lngLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(108, 17);
            this.lngLabel1.TabIndex = 65;
            this.lngLabel1.Text = "Schedule Name";
            this.lngLabel1.TranslationKey = "";
            // 
            // lngButton6
            // 
            this.lngButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton6.Location = new System.Drawing.Point(348, 286);
            this.lngButton6.Margin = new System.Windows.Forms.Padding(4);
            this.lngButton6.Name = "lngButton6";
            this.lngButton6.Size = new System.Drawing.Size(67, 34);
            this.lngButton6.TabIndex = 63;
            this.lngButton6.Text = "<";
            this.lngButton6.TranslationKey = null;
            this.lngButton6.UseVisualStyleBackColor = true;
            this.lngButton6.Click += new System.EventHandler(this.lngButton6_Click);
            // 
            // lngButton5
            // 
            this.lngButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton5.Location = new System.Drawing.Point(348, 323);
            this.lngButton5.Margin = new System.Windows.Forms.Padding(4);
            this.lngButton5.Name = "lngButton5";
            this.lngButton5.Size = new System.Drawing.Size(67, 34);
            this.lngButton5.TabIndex = 64;
            this.lngButton5.Text = "<<";
            this.lngButton5.TranslationKey = null;
            this.lngButton5.UseVisualStyleBackColor = true;
            this.lngButton5.Click += new System.EventHandler(this.lngButton5_Click);
            // 
            // lngButton7
            // 
            this.lngButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton7.Location = new System.Drawing.Point(348, 249);
            this.lngButton7.Margin = new System.Windows.Forms.Padding(4);
            this.lngButton7.Name = "lngButton7";
            this.lngButton7.Size = new System.Drawing.Size(67, 34);
            this.lngButton7.TabIndex = 62;
            this.lngButton7.Text = ">>";
            this.lngButton7.TranslationKey = null;
            this.lngButton7.UseVisualStyleBackColor = true;
            this.lngButton7.Click += new System.EventHandler(this.lngButton7_Click);
            // 
            // lngButton8
            // 
            this.lngButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton8.Location = new System.Drawing.Point(348, 212);
            this.lngButton8.Margin = new System.Windows.Forms.Padding(4);
            this.lngButton8.Name = "lngButton8";
            this.lngButton8.Size = new System.Drawing.Size(67, 34);
            this.lngButton8.TabIndex = 61;
            this.lngButton8.Text = ">";
            this.lngButton8.TranslationKey = null;
            this.lngButton8.UseVisualStyleBackColor = true;
            this.lngButton8.Click += new System.EventHandler(this.lngButton8_Click);
            // 
            // lngLabel5
            // 
            this.lngLabel5.AutoSize = true;
            this.lngLabel5.Location = new System.Drawing.Point(5, 33);
            this.lngLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel5.Name = "lngLabel5";
            this.lngLabel5.Size = new System.Drawing.Size(89, 17);
            this.lngLabel5.TabIndex = 54;
            this.lngLabel5.Text = "Group Name";
            this.lngLabel5.TranslationKey = "";
            // 
            // lngLabel7
            // 
            this.lngLabel7.AutoSize = true;
            this.lngLabel7.Location = new System.Drawing.Point(5, 64);
            this.lngLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel7.Name = "lngLabel7";
            this.lngLabel7.Size = new System.Drawing.Size(129, 17);
            this.lngLabel7.TabIndex = 56;
            this.lngLabel7.Text = "Reading Start Date";
            this.lngLabel7.TranslationKey = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(673, 487);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TranslationKey = "B000009";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(555, 487);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "&Save";
            this.btnSave.TranslationKey = "B000008";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // GSMGroupInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpHead);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "GSMGroupInformation";
            this.Size = new System.Drawing.Size(790, 531);
            this.Load += new System.EventHandler(this.GSMScheduleInfo_Load);
            this.grpHead.ResumeLayout(false);
            this.grpHead.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpHead;
        internal System.Windows.Forms.ComboBox cboSchedule;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Panel panel1;
        private CAB.UI.Controls.CABLabel lngLabel3;
        private System.Windows.Forms.Panel panel2;
        private CAB.UI.Controls.CABLabel lngLabel4;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private CAB.UI.Controls.CABButton lngButton6;
        private CAB.UI.Controls.CABButton lngButton5;
        private CAB.UI.Controls.CABButton lngButton7;
        private CAB.UI.Controls.CABButton lngButton8;
        private System.Windows.Forms.ListBox lstSelectedMeter;
        private System.Windows.Forms.ListBox lstAllMeter;
        private CAB.UI.Controls.CABButton btnCancel;
        private CAB.UI.Controls.CABLabel lngLabel5;
        public CAB.UI.Controls.CABButton btnSave;
        private System.Windows.Forms.TextBox txtGroupName;
        private CAB.UI.Controls.CABLabel lngLabel7;

    }
}
