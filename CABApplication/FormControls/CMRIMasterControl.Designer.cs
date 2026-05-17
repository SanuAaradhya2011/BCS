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
            this.groupBoxMRIDefinition.Controls.Add(this.lblMRINumber);
            this.groupBoxMRIDefinition.Controls.Add(this.lblDescription);
            this.groupBoxMRIDefinition.Controls.Add(this.txtBoxCMRINumber);
            this.groupBoxMRIDefinition.Controls.Add(this.txtBoxCMRIDescription);
            this.groupBoxMRIDefinition.Controls.Add(this.btnSave);
            this.groupBoxMRIDefinition.Controls.Add(this.btnCancel);
            this.groupBoxMRIDefinition.Location = new System.Drawing.Point(0, 0); // overridden at runtime by CenterGroupBox()
            this.groupBoxMRIDefinition.Name = "groupBoxMRIDefinition";
            this.groupBoxMRIDefinition.Size = new System.Drawing.Size(400, 240);
            this.groupBoxMRIDefinition.TabIndex = 1;
            this.groupBoxMRIDefinition.TabStop = false;
            this.groupBoxMRIDefinition.Text = "📟  Add New Device";
            this.groupBoxMRIDefinition.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.groupBoxMRIDefinition.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.groupBoxMRIDefinition.Padding = new System.Windows.Forms.Padding(10);
            // 
            // lblMRINumber
            // 
            this.lblMRINumber.AutoSize = true;
            this.lblMRINumber.Location = new System.Drawing.Point(25, 45);
            this.lblMRINumber.Name = "lblMRINumber";
            this.lblMRINumber.Size = new System.Drawing.Size(150, 17);
            this.lblMRINumber.TabIndex = 6;
            this.lblMRINumber.Text = "CMRI Serial Number:";
            this.lblMRINumber.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMRINumber.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(25, 105);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(150, 17);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Device Description:";
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // txtBoxCMRINumber
            // 
            this.txtBoxCMRINumber.Location = new System.Drawing.Point(25, 65);
            this.txtBoxCMRINumber.MaxLength = 16;
            this.txtBoxCMRINumber.Name = "txtBoxCMRINumber";
            this.txtBoxCMRINumber.Size = new System.Drawing.Size(350, 25);
            this.txtBoxCMRINumber.TabIndex = 0;
            this.txtBoxCMRINumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCMRINumber.BackColor = System.Drawing.Color.White;
            this.txtBoxCMRINumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // txtBoxCMRIDescription
            // 
            this.txtBoxCMRIDescription.Location = new System.Drawing.Point(25, 125);
            this.txtBoxCMRIDescription.MaxLength = 50;
            this.txtBoxCMRIDescription.Name = "txtBoxCMRIDescription";
            this.txtBoxCMRIDescription.Size = new System.Drawing.Size(350, 25);
            this.txtBoxCMRIDescription.TabIndex = 1;
            this.txtBoxCMRIDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxCMRIDescription.BackColor = System.Drawing.Color.White;
            this.txtBoxCMRIDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 185);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 32);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "💾  Save Device";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "✖️  Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CMRIMasterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.BackColor = System.Drawing.Color.FromArgb(235, 240, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMRIDefinition);
            this.Name = "CMRIMasterControl";
            this.Size = new System.Drawing.Size(420, 250);
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
    }
}

