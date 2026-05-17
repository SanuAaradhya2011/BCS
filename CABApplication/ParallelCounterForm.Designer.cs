namespace CABApplication
{
    partial class ParallelCounterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParallelCounterForm));
            this.rbSerial = new System.Windows.Forms.RadioButton();
            this.rbParallel = new System.Windows.Forms.RadioButton();
            this.lblThreadCount = new System.Windows.Forms.Label();
            this.txtThreadCount = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rbSerial
            // 
            this.rbSerial.AutoSize = true;
            this.rbSerial.Location = new System.Drawing.Point(17, 13);
            this.rbSerial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSerial.Name = "rbSerial";
            this.rbSerial.Size = new System.Drawing.Size(165, 29);
            this.rbSerial.TabIndex = 0;
            this.rbSerial.TabStop = true;
            this.rbSerial.Text = "Read Sequencial";
            this.rbSerial.UseVisualStyleBackColor = true;
            this.rbSerial.CheckedChanged += new System.EventHandler(this.rbSerial_CheckedChanged);
            // 
            // rbParallel
            // 
            this.rbParallel.AutoSize = true;
            this.rbParallel.Location = new System.Drawing.Point(17, 55);
            this.rbParallel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbParallel.Name = "rbParallel";
            this.rbParallel.Size = new System.Drawing.Size(135, 29);
            this.rbParallel.TabIndex = 1;
            this.rbParallel.TabStop = true;
            this.rbParallel.Text = "Read Parallel";
            this.rbParallel.UseVisualStyleBackColor = true;
            this.rbParallel.CheckedChanged += new System.EventHandler(this.rbParallel_CheckedChanged);
            // 
            // lblThreadCount
            // 
            this.lblThreadCount.AutoSize = true;
            this.lblThreadCount.Location = new System.Drawing.Point(17, 95);
            this.lblThreadCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThreadCount.Name = "lblThreadCount";
            this.lblThreadCount.Size = new System.Drawing.Size(201, 25);
            this.lblThreadCount.TabIndex = 2;
            this.lblThreadCount.Text = "Parallel Thread Pool Size";
            // 
            // txtThreadCount
            // 
            this.txtThreadCount.Location = new System.Drawing.Point(204, 88);
            this.txtThreadCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtThreadCount.Name = "txtThreadCount";
            this.txtThreadCount.Size = new System.Drawing.Size(141, 31);
            this.txtThreadCount.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(203, 187);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 38);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(17, 142);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(341, 25);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "TCP is applicable for DLMS meters only !!!";
            // 
            // ParallelCounterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(494, 230);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtThreadCount);
            this.Controls.Add(this.lblThreadCount);
            this.Controls.Add(this.rbParallel);
            this.Controls.Add(this.rbSerial);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ParallelCounterForm";
            this.Text = "ParallelCounterForm";
            this.Load += new System.EventHandler(this.ParallelCounterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbSerial;
        private System.Windows.Forms.RadioButton rbParallel;
        private System.Windows.Forms.Label lblThreadCount;
        private System.Windows.Forms.TextBox txtThreadCount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMessage;
    }
}

