namespace CAB.UI
{
    partial class CMRIID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMRIID));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbCMRI = new System.Windows.Forms.RadioButton();
            this.rdbDirect = new System.Windows.Forms.RadioButton();
            this.txtCMRIID = new System.Windows.Forms.TextBox();
            this.lblCMRIID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbCMRI);
            this.groupBox1.Controls.Add(this.rdbDirect);
            this.groupBox1.Controls.Add(this.txtCMRIID);
            this.groupBox1.Controls.Add(this.lblCMRIID);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Location = new System.Drawing.Point(11, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(424, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rdbCMRI
            // 
            this.rdbCMRI.AutoSize = true;
            this.rdbCMRI.Location = new System.Drawing.Point(117, 117);
            this.rdbCMRI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdbCMRI.Name = "rdbCMRI";
            this.rdbCMRI.Size = new System.Drawing.Size(182, 29);
            this.rdbCMRI.TabIndex = 22;
            this.rdbCMRI.TabStop = true;
            this.rdbCMRI.Text = "CMRI Readout File";
            this.rdbCMRI.UseVisualStyleBackColor = true;
            this.rdbCMRI.CheckedChanged += new System.EventHandler(this.rdbCMRI_CheckedChanged);
            // 
            // rdbDirect
            // 
            this.rdbDirect.AutoSize = true;
            this.rdbDirect.Location = new System.Drawing.Point(117, 78);
            this.rdbDirect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rdbDirect.Name = "rdbDirect";
            this.rdbDirect.Size = new System.Drawing.Size(185, 29);
            this.rdbDirect.TabIndex = 21;
            this.rdbDirect.TabStop = true;
            this.rdbDirect.Text = "Direct Readout File";
            this.rdbDirect.UseVisualStyleBackColor = true;
            this.rdbDirect.CheckedChanged += new System.EventHandler(this.rdbDirect_CheckedChanged);
            // 
            // txtCMRIID
            // 
            this.txtCMRIID.Location = new System.Drawing.Point(117, 33);
            this.txtCMRIID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCMRIID.MaxLength = 10;
            this.txtCMRIID.Name = "txtCMRIID";
            this.txtCMRIID.Size = new System.Drawing.Size(257, 31);
            this.txtCMRIID.TabIndex = 17;
            this.txtCMRIID.TextChanged += new System.EventHandler(this.txtCMRIID_TextChanged);
            this.txtCMRIID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCMRIID_KeyPress);
            // 
            // lblCMRIID
            // 
            this.lblCMRIID.AutoSize = true;
            this.lblCMRIID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCMRIID.Location = new System.Drawing.Point(33, 38);
            this.lblCMRIID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCMRIID.Name = "lblCMRIID";
            this.lblCMRIID.Size = new System.Drawing.Size(78, 25);
            this.lblCMRIID.TabIndex = 20;
            this.lblCMRIID.Text = "CMRI ID";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(284, 170);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 45);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(190, 170);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 45);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // CMRIID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(444, 237);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CMRIID";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter CMRI ID";
            this.Load += new System.EventHandler(this.CMRIID_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox txtCMRIID;
        private System.Windows.Forms.Label lblCMRIID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rdbCMRI;
        private System.Windows.Forms.RadioButton rdbDirect;


    }
}

