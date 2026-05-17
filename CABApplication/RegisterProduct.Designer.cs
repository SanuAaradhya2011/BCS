namespace CAB.UI
{
    partial class RegisterProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterProduct));
            this.btnProceed = new System.Windows.Forms.Button();
            this.rdBuyNow = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.grGroupBoxInitial = new System.Windows.Forms.GroupBox();
            this.lblDaysRemaining = new System.Windows.Forms.Label();
            this.grGroupBoxInitial.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProceed
            // 
            this.btnProceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.Location = new System.Drawing.Point(173, 42);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(92, 32);
            this.btnProceed.TabIndex = 2;
            this.btnProceed.Text = "Proceed >>";
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // rdBuyNow
            // 
            this.rdBuyNow.AutoSize = true;
            this.rdBuyNow.Checked = true;
            this.rdBuyNow.Location = new System.Drawing.Point(24, 19);
            this.rdBuyNow.Name = "rdBuyNow";
            this.rdBuyNow.Size = new System.Drawing.Size(74, 17);
            this.rdBuyNow.TabIndex = 0;
            this.rdBuyNow.TabStop = true;
            this.rdBuyNow.Text = "Buy Now !";
            this.rdBuyNow.UseVisualStyleBackColor = true;
            this.rdBuyNow.CheckedChanged += new System.EventHandler(this.rdBuyNow_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(111, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(156, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "Continue using Trial Version";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // grGroupBoxInitial
            // 
            this.grGroupBoxInitial.Controls.Add(this.lblDaysRemaining);
            this.grGroupBoxInitial.Controls.Add(this.btnProceed);
            this.grGroupBoxInitial.Controls.Add(this.radioButton1);
            this.grGroupBoxInitial.Controls.Add(this.rdBuyNow);
            this.grGroupBoxInitial.Location = new System.Drawing.Point(7, 1);
            this.grGroupBoxInitial.Name = "grGroupBoxInitial";
            this.grGroupBoxInitial.Size = new System.Drawing.Size(276, 94);
            this.grGroupBoxInitial.TabIndex = 13;
            this.grGroupBoxInitial.TabStop = false;
            // 
            // lblDaysRemaining
            // 
            this.lblDaysRemaining.AutoSize = true;
            this.lblDaysRemaining.ForeColor = System.Drawing.Color.Red;
            this.lblDaysRemaining.Location = new System.Drawing.Point(6, 75);
            this.lblDaysRemaining.Name = "lblDaysRemaining";
            this.lblDaysRemaining.Size = new System.Drawing.Size(0, 13);
            this.lblDaysRemaining.TabIndex = 3;
            // 
            // RegisterProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(290, 100);
            this.Controls.Add(this.grGroupBoxInitial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterProduct";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register Product";
            this.Load += new System.EventHandler(this.RegisterProduct_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegisterProduct_FormClosing);
            this.grGroupBoxInitial.ResumeLayout(false);
            this.grGroupBoxInitial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.RadioButton rdBuyNow;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox grGroupBoxInitial;
        private System.Windows.Forms.Label lblDaysRemaining;

    }
}

