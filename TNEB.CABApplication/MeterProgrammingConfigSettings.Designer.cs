namespace CAB.UI
{
    partial class MeterProgrammingConfigSettings
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
            this.lngButton1 = new CAB.UI.Controls.CABButton();
            this.lngButton2 = new CAB.UI.Controls.CABButton();
            this.lngButton3 = new CAB.UI.Controls.CABButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lngButton3);
            this.groupBox1.Controls.Add(this.lngButton2);
            this.groupBox1.Controls.Add(this.lngButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 193);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Meter Programming";
            // 
            // lngButton1
            // 
            this.lngButton1.Location = new System.Drawing.Point(79, 57);
            this.lngButton1.Name = "lngButton1";
            this.lngButton1.Size = new System.Drawing.Size(119, 23);
            this.lngButton1.TabIndex = 0;
            this.lngButton1.Text = "Get Configuration";
            this.lngButton1.TranslationKey = null;
            this.lngButton1.UseVisualStyleBackColor = true;
            // 
            // lngButton2
            // 
            this.lngButton2.Location = new System.Drawing.Point(79, 86);
            this.lngButton2.Name = "lngButton2";
            this.lngButton2.Size = new System.Drawing.Size(119, 23);
            this.lngButton2.TabIndex = 1;
            this.lngButton2.Text = "Configuration History";
            this.lngButton2.TranslationKey = null;
            this.lngButton2.UseVisualStyleBackColor = true;
            // 
            // lngButton3
            // 
            this.lngButton3.Location = new System.Drawing.Point(79, 115);
            this.lngButton3.Name = "lngButton3";
            this.lngButton3.Size = new System.Drawing.Size(119, 23);
            this.lngButton3.TabIndex = 2;
            this.lngButton3.Text = "Show Configuration";
            this.lngButton3.TranslationKey = null;
            this.lngButton3.UseVisualStyleBackColor = true;
            // 
            // MeterProgrammingConfigSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.groupBox1);
            this.Name = "MeterProgrammingConfigSettings";
            this.StatusMessage = "";
            this.Text = "MeterProgrammingConfigSettings";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABButton lngButton1;
        private CAB.UI.Controls.CABButton lngButton3;
        private CAB.UI.Controls.CABButton lngButton2;
    }
}