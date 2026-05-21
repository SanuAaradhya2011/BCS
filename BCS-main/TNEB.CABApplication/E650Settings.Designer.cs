namespace CABApplication
{
    partial class E650Settings
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
            this.groupBoxModeSetting = new System.Windows.Forms.GroupBox();
            this.groupBoxPassword = new System.Windows.Forms.GroupBox();
            this.lbllHLS = new System.Windows.Forms.Label();
            this.txtPWD = new System.Windows.Forms.TextBox();
            this.groupBoxSelectMode = new System.Windows.Forms.GroupBox();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.lblSelectMode = new System.Windows.Forms.Label();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.btnSave = new CAB.UI.Controls.CABButton();
            this.groupBoxModeSetting.SuspendLayout();
            this.groupBoxPassword.SuspendLayout();
            this.groupBoxSelectMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxModeSetting
            // 
            this.groupBoxModeSetting.Controls.Add(this.groupBoxPassword);
            this.groupBoxModeSetting.Controls.Add(this.groupBoxSelectMode);
            this.groupBoxModeSetting.Controls.Add(this.btnCancel);
            this.groupBoxModeSetting.Controls.Add(this.btnSave);
            this.groupBoxModeSetting.Location = new System.Drawing.Point(8, 8);
            this.groupBoxModeSetting.Name = "groupBoxModeSetting";
            this.groupBoxModeSetting.Size = new System.Drawing.Size(330, 264);
            this.groupBoxModeSetting.TabIndex = 3;
            this.groupBoxModeSetting.TabStop = false;
            this.groupBoxModeSetting.Text = "Mode Settings";
            // 
            // groupBoxPassword
            // 
            this.groupBoxPassword.Controls.Add(this.lbllHLS);
            this.groupBoxPassword.Controls.Add(this.txtPWD);
            this.groupBoxPassword.Location = new System.Drawing.Point(15, 118);
            this.groupBoxPassword.Name = "groupBoxPassword";
            this.groupBoxPassword.Size = new System.Drawing.Size(297, 77);
            this.groupBoxPassword.TabIndex = 7;
            this.groupBoxPassword.TabStop = false;
            this.groupBoxPassword.Text = "Password Authentication";
            // 
            // lbllHLS
            // 
            this.lbllHLS.AutoSize = true;
            this.lbllHLS.Location = new System.Drawing.Point(69, 35);
            this.lbllHLS.Name = "lbllHLS";
            this.lbllHLS.Size = new System.Drawing.Size(49, 13);
            this.lbllHLS.TabIndex = 2;
            this.lbllHLS.Text = "HLS Key";
            // 
            // txtPWD
            // 
            this.txtPWD.Location = new System.Drawing.Point(157, 32);
            this.txtPWD.MaxLength = 8;
            this.txtPWD.Name = "txtPWD";
            this.txtPWD.PasswordChar = '*';
            this.txtPWD.Size = new System.Drawing.Size(123, 20);
            this.txtPWD.TabIndex = 1;
            this.txtPWD.Text = "12345678";
            // 
            // groupBoxSelectMode
            // 
            this.groupBoxSelectMode.Controls.Add(this.cmbMode);
            this.groupBoxSelectMode.Controls.Add(this.lblSelectMode);
            this.groupBoxSelectMode.Location = new System.Drawing.Point(15, 29);
            this.groupBoxSelectMode.Name = "groupBoxSelectMode";
            this.groupBoxSelectMode.Size = new System.Drawing.Size(297, 71);
            this.groupBoxSelectMode.TabIndex = 6;
            this.groupBoxSelectMode.TabStop = false;
            this.groupBoxSelectMode.Text = "Mode";
            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "Reader (MR)",
            "Master (US) "});
            this.cmbMode.Location = new System.Drawing.Point(157, 29);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(123, 21);
            this.cmbMode.TabIndex = 3;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // lblSelectMode
            // 
            this.lblSelectMode.AutoSize = true;
            this.lblSelectMode.Location = new System.Drawing.Point(69, 32);
            this.lblSelectMode.Name = "lblSelectMode";
            this.lblSelectMode.Size = new System.Drawing.Size(67, 13);
            this.lblSelectMode.TabIndex = 0;
            this.lblSelectMode.Text = "Select Mode";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(210, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TranslationKey = null;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(139, 216);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(55, 25);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.TranslationKey = null;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // E650Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(351, 279);
            this.Controls.Add(this.groupBoxModeSetting);
            this.Name = "E650Settings";
            this.StatusMessage = "";
            this.Text = "LT Settings";
            this.Load += new System.EventHandler(this.SystemSettings_Load);
            this.groupBoxModeSetting.ResumeLayout(false);
            this.groupBoxPassword.ResumeLayout(false);
            this.groupBoxPassword.PerformLayout();
            this.groupBoxSelectMode.ResumeLayout(false);
            this.groupBoxSelectMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxModeSetting;
        private CAB.UI.Controls.CABButton btnCancel;
        private CAB.UI.Controls.CABButton btnSave;
        private System.Windows.Forms.GroupBox groupBoxSelectMode;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Label lblSelectMode;
        private System.Windows.Forms.GroupBox groupBoxPassword;
        private System.Windows.Forms.Label lbllHLS;
        private System.Windows.Forms.TextBox txtPWD;
    }
}