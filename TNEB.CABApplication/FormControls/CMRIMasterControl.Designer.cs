namespace CAB.UI
{
    partial class CMRIMasterControl
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
            this.groupBoxMRIDefinition = new System.Windows.Forms.GroupBox();
            this.lblCMRIType = new System.Windows.Forms.Label();
            this.rdSands = new System.Windows.Forms.RadioButton();
            this.rdAnalogs = new System.Windows.Forms.RadioButton();
            this.lblMRINumber = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtBoxCMRINumber = new System.Windows.Forms.TextBox();
            this.txtBoxCMRIDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxMRIDefinition.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMRIDefinition
            // 
            this.groupBoxMRIDefinition.Controls.Add(this.lblCMRIType);
            this.groupBoxMRIDefinition.Controls.Add(this.rdSands);
            this.groupBoxMRIDefinition.Controls.Add(this.rdAnalogs);
            this.groupBoxMRIDefinition.Controls.Add(this.lblMRINumber);
            this.groupBoxMRIDefinition.Controls.Add(this.lblDescription);
            this.groupBoxMRIDefinition.Controls.Add(this.txtBoxCMRINumber);
            this.groupBoxMRIDefinition.Controls.Add(this.txtBoxCMRIDescription);
            this.groupBoxMRIDefinition.Controls.Add(this.btnSave);
            this.groupBoxMRIDefinition.Controls.Add(this.btnCancel);
            this.groupBoxMRIDefinition.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMRIDefinition.Name = "groupBoxMRIDefinition";
            this.groupBoxMRIDefinition.Size = new System.Drawing.Size(403, 226);
            this.groupBoxMRIDefinition.TabIndex = 1;
            this.groupBoxMRIDefinition.TabStop = false;
            this.groupBoxMRIDefinition.Text = "CMRI Definition";
            // 
            // lblCMRIType
            // 
            this.lblCMRIType.Location = new System.Drawing.Point(46, 123);
            this.lblCMRIType.Name = "lblCMRIType";
            this.lblCMRIType.Size = new System.Drawing.Size(60, 13);
            this.lblCMRIType.TabIndex = 10;
            this.lblCMRIType.Text = "Type";
            // 
            // rdSands
            // 
            this.rdSands.AutoSize = true;
            this.rdSands.Location = new System.Drawing.Point(202, 119);
            this.rdSands.Name = "rdSands";
            this.rdSands.Size = new System.Drawing.Size(62, 17);
            this.rdSands.TabIndex = 9;
            this.rdSands.TabStop = true;
            this.rdSands.Text = "SANDS";
            this.rdSands.UseVisualStyleBackColor = true;
            // 
            // rdAnalogs
            // 
            this.rdAnalogs.AutoSize = true;
            this.rdAnalogs.Location = new System.Drawing.Point(136, 119);
            this.rdAnalogs.Name = "rdAnalogs";
            this.rdAnalogs.Size = new System.Drawing.Size(66, 17);
            this.rdAnalogs.TabIndex = 8;
            this.rdAnalogs.TabStop = true;
            this.rdAnalogs.Text = "Analogic";
            this.rdAnalogs.UseVisualStyleBackColor = true;
            // 
            // lblMRINumber
            // 
            this.lblMRINumber.AutoSize = true;
            this.lblMRINumber.Location = new System.Drawing.Point(46, 52);
            this.lblMRINumber.Name = "lblMRINumber";
            this.lblMRINumber.Size = new System.Drawing.Size(74, 13);
            this.lblMRINumber.TabIndex = 6;
            this.lblMRINumber.Text = "CMRI Number";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(46, 88);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description";
            // 
            // txtBoxCMRINumber
            // 
            this.txtBoxCMRINumber.Location = new System.Drawing.Point(136, 49);
            this.txtBoxCMRINumber.MaxLength = 10;
            this.txtBoxCMRINumber.Name = "txtBoxCMRINumber";
            this.txtBoxCMRINumber.Size = new System.Drawing.Size(121, 20);
            this.txtBoxCMRINumber.TabIndex = 0;
            // 
            // txtBoxCMRIDescription
            // 
            this.txtBoxCMRIDescription.Location = new System.Drawing.Point(136, 84);
            this.txtBoxCMRIDescription.MaxLength = 50;
            this.txtBoxCMRIDescription.Name = "txtBoxCMRIDescription";
            this.txtBoxCMRIDescription.Size = new System.Drawing.Size(121, 20);
            this.txtBoxCMRIDescription.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(136, 155);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(200, 155);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CMRIMasterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.groupBoxMRIDefinition);
            this.Name = "CMRIMasterControl";
            this.Size = new System.Drawing.Size(411, 236);
            this.Load += new System.EventHandler(this.CMRIMasterControl_Load);
            this.groupBoxMRIDefinition.ResumeLayout(false);
            this.groupBoxMRIDefinition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Label lblMRINumber;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtBoxCMRINumber;
        private System.Windows.Forms.TextBox txtBoxCMRIDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.GroupBox groupBoxMRIDefinition;
        private System.Windows.Forms.Label lblCMRIType;
        private System.Windows.Forms.RadioButton rdSands;
        private System.Windows.Forms.RadioButton rdAnalogs;
    }
}
