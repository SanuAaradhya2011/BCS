namespace CAB.UI
{
    partial class GSMScheduleActivation
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.rbtnStop = new System.Windows.Forms.RadioButton();
            this.rbtnStart = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.rbtnStop);
            this.groupBox1.Controls.Add(this.rbtnStart);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 167);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activate GSM Schedule";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(325, 125);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 31);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(192, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 31);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Activate";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rbtnStop
            // 
            this.rbtnStop.AutoSize = true;
            this.rbtnStop.Location = new System.Drawing.Point(241, 57);
            this.rbtnStop.Name = "rbtnStop";
            this.rbtnStop.Size = new System.Drawing.Size(144, 21);
            this.rbtnStop.TabIndex = 1;
            this.rbtnStop.TabStop = true;
            this.rbtnStop.Text = "Stop GSM Service";
            this.rbtnStop.UseVisualStyleBackColor = true;
            this.rbtnStop.CheckedChanged += new System.EventHandler(this.rbtnStop_CheckedChanged);
            // 
            // rbtnStart
            // 
            this.rbtnStart.AutoSize = true;
            this.rbtnStart.Location = new System.Drawing.Point(53, 57);
            this.rbtnStart.Name = "rbtnStart";
            this.rbtnStart.Size = new System.Drawing.Size(145, 21);
            this.rbtnStart.TabIndex = 0;
            this.rbtnStart.TabStop = true;
            this.rbtnStart.Text = "Start GSM Service";
            this.rbtnStart.UseVisualStyleBackColor = true;
            this.rbtnStart.CheckedChanged += new System.EventHandler(this.rbtnStart_CheckedChanged);
            // 
            // GSMScheduleActivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 180);
            this.Controls.Add(this.groupBox1);
            this.Name = "GSMScheduleActivation";
            this.StatusMessage = "";
            this.Text = "GSMScheduleActivation";
            this.Load += new System.EventHandler(this.GSMScheduleActivation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnStop;
        private System.Windows.Forms.RadioButton rbtnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}