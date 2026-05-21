namespace CABAppControl
{
    partial class kvarSelection
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
            this.rdbLagOnly = new System.Windows.Forms.RadioButton();
            this.rdbLagnLead = new System.Windows.Forms.RadioButton();
            this.grpkvarSelection = new System.Windows.Forms.GroupBox();
            this.grpkvarSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdbLagOnly
            // 
            this.rdbLagOnly.AutoSize = true;
            this.rdbLagOnly.Location = new System.Drawing.Point(78, 13);
            this.rdbLagOnly.Name = "rdbLagOnly";
            this.rdbLagOnly.Size = new System.Drawing.Size(98, 17);
            this.rdbLagOnly.TabIndex = 0;
            this.rdbLagOnly.TabStop = true;
            this.rdbLagOnly.Text = "Lag only (Lock)";
            this.rdbLagOnly.UseVisualStyleBackColor = true;
            // 
            // rdbLagnLead
            // 
            this.rdbLagnLead.AutoSize = true;
            this.rdbLagnLead.Location = new System.Drawing.Point(78, 54);
            this.rdbLagnLead.Name = "rdbLagnLead";
            this.rdbLagnLead.Size = new System.Drawing.Size(120, 17);
            this.rdbLagnLead.TabIndex = 1;
            this.rdbLagnLead.TabStop = true;
            this.rdbLagnLead.Text = "Lag + Lead (unlock)";
            this.rdbLagnLead.UseVisualStyleBackColor = true;
            // 
            // grpkvarSelection
            // 
            this.grpkvarSelection.Controls.Add(this.rdbLagnLead);
            this.grpkvarSelection.Controls.Add(this.rdbLagOnly);
            this.grpkvarSelection.Location = new System.Drawing.Point(3, -3);
            this.grpkvarSelection.Name = "grpkvarSelection";
            this.grpkvarSelection.Size = new System.Drawing.Size(574, 89);
            this.grpkvarSelection.TabIndex = 2;
            this.grpkvarSelection.TabStop = false;
            // 
            // kvarSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpkvarSelection);
            this.Name = "kvarSelection";
            this.Size = new System.Drawing.Size(580, 90);
            this.grpkvarSelection.ResumeLayout(false);
            this.grpkvarSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbLagOnly;
        private System.Windows.Forms.RadioButton rdbLagnLead;
        private System.Windows.Forms.GroupBox grpkvarSelection;
    }
}
