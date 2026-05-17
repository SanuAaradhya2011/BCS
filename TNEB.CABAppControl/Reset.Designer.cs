namespace CABAppControl
{
    partial class Reset
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
            this.chkBillingReset = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkBillingReset
            // 
            this.chkBillingReset.AutoSize = true;
            this.chkBillingReset.Location = new System.Drawing.Point(18, 26);
            this.chkBillingReset.Name = "chkBillingReset";
            this.chkBillingReset.Size = new System.Drawing.Size(84, 17);
            this.chkBillingReset.TabIndex = 0;
            this.chkBillingReset.Text = "Billing Reset";
            this.chkBillingReset.UseVisualStyleBackColor = true;
            // 
            // Reset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkBillingReset);
            this.Name = "Reset";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBillingReset;
    }
}
