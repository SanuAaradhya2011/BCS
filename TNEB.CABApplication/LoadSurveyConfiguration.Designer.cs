namespace CAB.UI
{
    partial class LoadSurveyConfiguration
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
            this.grpLSConfiguartion = new System.Windows.Forms.GroupBox();
            this.btnLSConfigCancel = new System.Windows.Forms.Button();
            this.btnLSConfigSave = new System.Windows.Forms.Button();
            this.cmbLSConfiguration = new System.Windows.Forms.ComboBox();
            this.grpLSConfiguartion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLSConfiguartion
            // 
            this.grpLSConfiguartion.Controls.Add(this.btnLSConfigCancel);
            this.grpLSConfiguartion.Controls.Add(this.btnLSConfigSave);
            this.grpLSConfiguartion.Controls.Add(this.cmbLSConfiguration);
            this.grpLSConfiguartion.Location = new System.Drawing.Point(11, 8);
            this.grpLSConfiguartion.Name = "grpLSConfiguartion";
            this.grpLSConfiguartion.Size = new System.Drawing.Size(313, 68);
            this.grpLSConfiguartion.TabIndex = 0;
            this.grpLSConfiguartion.TabStop = false;
            this.grpLSConfiguartion.Text = "No. of days for which Load Suvey is to be read:";
            // 
            // btnLSConfigCancel
            // 
            this.btnLSConfigCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLSConfigCancel.Location = new System.Drawing.Point(239, 35);
            this.btnLSConfigCancel.Name = "btnLSConfigCancel";
            this.btnLSConfigCancel.Size = new System.Drawing.Size(56, 23);
            this.btnLSConfigCancel.TabIndex = 2;
            this.btnLSConfigCancel.Text = "Cancel";
            this.btnLSConfigCancel.UseVisualStyleBackColor = true;
            this.btnLSConfigCancel.Click += new System.EventHandler(this.btnLSConfigCancel_Click);
            // 
            // btnLSConfigSave
            // 
            this.btnLSConfigSave.Location = new System.Drawing.Point(177, 35);
            this.btnLSConfigSave.Name = "btnLSConfigSave";
            this.btnLSConfigSave.Size = new System.Drawing.Size(56, 23);
            this.btnLSConfigSave.TabIndex = 1;
            this.btnLSConfigSave.Text = "Save";
            this.btnLSConfigSave.UseVisualStyleBackColor = true;
            this.btnLSConfigSave.Click += new System.EventHandler(this.btnLSConfigSave_Click);
            // 
            // cmbLSConfiguration
            // 
            this.cmbLSConfiguration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLSConfiguration.FormattingEnabled = true;
            this.cmbLSConfiguration.Location = new System.Drawing.Point(18, 33);
            this.cmbLSConfiguration.Name = "cmbLSConfiguration";
            this.cmbLSConfiguration.Size = new System.Drawing.Size(74, 21);
            this.cmbLSConfiguration.TabIndex = 1;
            // 
            // LoadSurveyConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 88);
            this.Controls.Add(this.grpLSConfiguartion);
            this.Name = "LoadSurveyConfiguration";
            this.StatusMessage = "";
            this.Text = "LoadSurveyConfiguration";
            this.Load += new System.EventHandler(this.LoadSurveyConfiguration_Load);
            this.grpLSConfiguartion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpLSConfiguartion;
        private System.Windows.Forms.Button btnLSConfigSave;
        private System.Windows.Forms.Button btnLSConfigCancel;
        internal System.Windows.Forms.ComboBox cmbLSConfiguration;
    }
}