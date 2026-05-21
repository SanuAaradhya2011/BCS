namespace CAB.UI
{
    partial class SchedularSettingForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvPortUsageAssociation = new System.Windows.Forms.DataGridView();
            this.SNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortUsage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.errpPortMapping = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblMsg = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.dgvPortUsageAssociation);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 220);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Port Settings";
            // 
            // dgvPortUsageAssociation
            // 
            this.dgvPortUsageAssociation.AllowUserToAddRows = false;
            this.dgvPortUsageAssociation.AllowUserToDeleteRows = false;
            this.dgvPortUsageAssociation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortUsageAssociation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SNumber,
            this.colPortName,
            this.colPortUsage});
            this.dgvPortUsageAssociation.Location = new System.Drawing.Point(9, 20);
            this.dgvPortUsageAssociation.Name = "dgvPortUsageAssociation";
            this.dgvPortUsageAssociation.RowHeadersWidth = 30;
            this.dgvPortUsageAssociation.Size = new System.Drawing.Size(275, 185);
            this.dgvPortUsageAssociation.TabIndex = 52;
            // 
            // SNumber
            // 
            this.SNumber.HeaderText = "S. No.";
            this.SNumber.Name = "SNumber";
            this.SNumber.Width = 60;
            // 
            // colPortName
            // 
            this.colPortName.HeaderText = "Port Name";
            this.colPortName.Name = "colPortName";
            this.colPortName.ReadOnly = true;
            // 
            // colPortUsage
            // 
            this.colPortUsage.HeaderText = "Select";
            this.colPortUsage.Name = "colPortUsage";
            this.colPortUsage.Width = 60;
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(255, 275);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(55, 25);
            this.lngbCancel.TabIndex = 51;
            this.lngbCancel.Text = "Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = false;
            this.lngbCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbCancel.ForeColor = System.Drawing.Color.White;
            this.lngbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbCancel.FlatAppearance.BorderSize = 0;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(194, 275);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(55, 25);
            this.lngbSave.TabIndex = 50;
            this.lngbSave.Text = "Save";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = false;
            this.lngbSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbSave.ForeColor = System.Drawing.Color.White;
            this.lngbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSave.FlatAppearance.BorderSize = 0;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // errpPortMapping
            // 
            this.errpPortMapping.ContainerControl = this;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(12, 252);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 13);
            this.lblMsg.TabIndex = 53;
            // 
            // SchedularSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(319, 310);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.lngbCancel);
            this.Controls.Add(this.lngbSave);
            this.Controls.Add(this.groupBox2);
            this.Name = "SchedularSettingForm";
            this.StatusMessage = "";
            this.Text = "Schedular Settings";
            this.Load += new System.EventHandler(this.PortSettingForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortSettingForm_FormClosing);
            this.Activated += new System.EventHandler(this.PortSettingForm_Activated);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private System.Windows.Forms.ErrorProvider errpPortMapping;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.DataGridView dgvPortUsageAssociation;
        private System.Windows.Forms.DataGridViewTextBoxColumn SNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPortUsage;

    }
}
