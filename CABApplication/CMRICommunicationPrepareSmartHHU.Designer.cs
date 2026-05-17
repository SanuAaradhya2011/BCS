namespace CAB.UI
{
    partial class CMRICommunicationPrepareSmartHHU
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
            this.bthSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbSelectionType = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bthSave
            // 
            this.bthSave.Location = new System.Drawing.Point(102, 71);
            this.bthSave.Name = "bthSave";
            this.bthSave.Size = new System.Drawing.Size(81, 38);
            this.bthSave.TabIndex = 2;
            this.bthSave.Text = "OK";
            this.bthSave.UseVisualStyleBackColor = true;
            this.bthSave.Click += new System.EventHandler(this.BthSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbSelectionType);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 53);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // cmbSelectionType
            // 
            this.cmbSelectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectionType.FormattingEnabled = true;
            this.cmbSelectionType.Items.AddRange(new object[] {
            "Devices List",
            "Security File"});
            this.cmbSelectionType.Location = new System.Drawing.Point(30, 19);
            this.cmbSelectionType.Name = "cmbSelectionType";
            this.cmbSelectionType.Size = new System.Drawing.Size(213, 21);
            this.cmbSelectionType.TabIndex = 4;
            // 
            // CMRICommunicationPrepareSmartHHU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 118);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bthSave);
            this.Name = "CMRICommunicationPrepareSmartHHU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prepare Smart HHU";
            this.Load += new System.EventHandler(this.CMRICommunicationPrepareSmartHHU_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button bthSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSelectionType;
    }
}