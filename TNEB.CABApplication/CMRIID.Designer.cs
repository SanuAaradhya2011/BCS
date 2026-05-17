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
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 138);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // rdbCMRI
            // 
            this.rdbCMRI.AutoSize = true;
            this.rdbCMRI.Location = new System.Drawing.Point(82, 70);
            this.rdbCMRI.Name = "rdbCMRI";
            this.rdbCMRI.Size = new System.Drawing.Size(115, 17);
            this.rdbCMRI.TabIndex = 22;
            this.rdbCMRI.TabStop = true;
            this.rdbCMRI.Text = "CMRI Readout File";
            this.rdbCMRI.UseVisualStyleBackColor = true;
            this.rdbCMRI.CheckedChanged += new System.EventHandler(this.rdbCMRI_CheckedChanged);
            // 
            // rdbDirect
            // 
            this.rdbDirect.AutoSize = true;
            this.rdbDirect.Location = new System.Drawing.Point(82, 47);
            this.rdbDirect.Name = "rdbDirect";
            this.rdbDirect.Size = new System.Drawing.Size(116, 17);
            this.rdbDirect.TabIndex = 21;
            this.rdbDirect.TabStop = true;
            this.rdbDirect.Text = "Direct Readout File";
            this.rdbDirect.UseVisualStyleBackColor = true;
            this.rdbDirect.CheckedChanged += new System.EventHandler(this.rdbDirect_CheckedChanged);
            // 
            // txtCMRIID
            // 
            this.txtCMRIID.Location = new System.Drawing.Point(82, 20);
            this.txtCMRIID.MaxLength = 10;
            this.txtCMRIID.Name = "txtCMRIID";
            this.txtCMRIID.Size = new System.Drawing.Size(181, 20);
            this.txtCMRIID.TabIndex = 17;
            this.txtCMRIID.TextChanged += new System.EventHandler(this.txtCMRIID_TextChanged);
            this.txtCMRIID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCMRIID_KeyPress);
            // 
            // lblCMRIID
            // 
            this.lblCMRIID.AutoSize = true;
            this.lblCMRIID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCMRIID.Location = new System.Drawing.Point(23, 23);
            this.lblCMRIID.Name = "lblCMRIID";
            this.lblCMRIID.Size = new System.Drawing.Size(48, 13);
            this.lblCMRIID.TabIndex = 20;
            this.lblCMRIID.Text = "CMRI ID";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(199, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 27);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(133, 102);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 27);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // CMRIID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(311, 142);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
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