namespace CABApplication
{
    partial class DebugSecurityKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugSecurityKey));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCipherHLS = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCipherEncryption = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCipherLLS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPlainHLS = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlainEncryption = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPlainLLS = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCipherHLS);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCipherEncryption);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCipherLLS);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(17, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1047, 270);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cipher text";
            // 
            // txtCipherHLS
            // 
            this.txtCipherHLS.Location = new System.Drawing.Point(121, 183);
            this.txtCipherHLS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCipherHLS.Multiline = true;
            this.txtCipherHLS.Name = "txtCipherHLS";
            this.txtCipherHLS.Size = new System.Drawing.Size(913, 56);
            this.txtCipherHLS.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 205);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "HLS Key";
            // 
            // txtCipherEncryption
            // 
            this.txtCipherEncryption.Location = new System.Drawing.Point(121, 112);
            this.txtCipherEncryption.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCipherEncryption.Multiline = true;
            this.txtCipherEncryption.Name = "txtCipherEncryption";
            this.txtCipherEncryption.Size = new System.Drawing.Size(913, 56);
            this.txtCipherEncryption.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Encruption Key";
            // 
            // txtCipherLLS
            // 
            this.txtCipherLLS.Location = new System.Drawing.Point(121, 35);
            this.txtCipherLLS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCipherLLS.Multiline = true;
            this.txtCipherLLS.Name = "txtCipherLLS";
            this.txtCipherLLS.Size = new System.Drawing.Size(913, 56);
            this.txtCipherLLS.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "LLS Key";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPlainHLS);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtPlainEncryption);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtPlainLLS);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(17, 360);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1047, 180);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Plain text";
            // 
            // txtPlainHLS
            // 
            this.txtPlainHLS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlainHLS.Location = new System.Drawing.Point(121, 122);
            this.txtPlainHLS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPlainHLS.Name = "txtPlainHLS";
            this.txtPlainHLS.ReadOnly = true;
            this.txtPlainHLS.Size = new System.Drawing.Size(913, 28);
            this.txtPlainHLS.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 122);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "HLS Key";
            // 
            // txtPlainEncryption
            // 
            this.txtPlainEncryption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlainEncryption.Location = new System.Drawing.Point(121, 78);
            this.txtPlainEncryption.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPlainEncryption.Name = "txtPlainEncryption";
            this.txtPlainEncryption.ReadOnly = true;
            this.txtPlainEncryption.Size = new System.Drawing.Size(913, 28);
            this.txtPlainEncryption.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 78);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 25);
            this.label5.TabIndex = 2;
            this.label5.Text = "Encruption Key";
            // 
            // txtPlainLLS
            // 
            this.txtPlainLLS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlainLLS.Location = new System.Drawing.Point(121, 35);
            this.txtPlainLLS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPlainLLS.Name = "txtPlainLLS";
            this.txtPlainLLS.ReadOnly = true;
            this.txtPlainLLS.Size = new System.Drawing.Size(913, 28);
            this.txtPlainLLS.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 35);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "LLS Key";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnDecrypt.FlatAppearance.BorderSize = 0;
            this.btnDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.ForeColor = System.Drawing.Color.White;
            this.btnDecrypt.Location = new System.Drawing.Point(433, 300);
            this.btnDecrypt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(250, 65);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = false;
            this.btnDecrypt.Click += new System.EventHandler(this.BtnDecrypt_Click);
            // 
            // DebugSecurityKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1077, 560);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebugSecurityKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decrypt Security Keys";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCipherHLS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCipherEncryption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCipherLLS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPlainHLS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPlainEncryption;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPlainLLS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDecrypt;
    }
}
