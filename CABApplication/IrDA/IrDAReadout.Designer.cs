namespace CAB.UI
{
    partial class IrDAReadout
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
            this.grp = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.slno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Param = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonRead = new System.Windows.Forms.Button();
            this.grp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // grp
            // 
            this.grp.Controls.Add(this.dataGridView1);
            this.grp.Controls.Add(this.buttonCancel);
            this.grp.Controls.Add(this.buttonExport);
            this.grp.Controls.Add(this.buttonRead);
            this.grp.Location = new System.Drawing.Point(4, 4);
            this.grp.Name = "grp";
            this.grp.Size = new System.Drawing.Size(734, 477);
            this.grp.TabIndex = 3;
            this.grp.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.slno,
            this.Param,
            this.value});
            this.dataGridView1.Location = new System.Drawing.Point(18, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(704, 401);
            this.dataGridView1.TabIndex = 42;
            // 
            // slno
            // 
            this.slno.HeaderText = "S.No";
            this.slno.Name = "slno";
            this.slno.Width = 50;
            // 
            // Param
            // 
            this.Param.HeaderText = "Parameter";
            this.Param.Name = "Param";
            this.Param.Width = 200;
            // 
            // value
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            this.value.Width = 200;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(602, 426);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(440, 426);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 0;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = false;
            this.buttonExport.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.buttonExport.ForeColor = System.Drawing.Color.White;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.FlatAppearance.BorderSize = 0;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonRead
            // 
            this.buttonRead.Location = new System.Drawing.Point(521, 426);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(75, 23);
            this.buttonRead.TabIndex = 0;
            this.buttonRead.Text = "Read Data";
            this.buttonRead.UseVisualStyleBackColor = false;
            this.buttonRead.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.buttonRead.ForeColor = System.Drawing.Color.White;
            this.buttonRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRead.FlatAppearance.BorderSize = 0;
            this.buttonRead.Click += new System.EventHandler(this.lblRead_Click);
            // 
            // IrDAReadout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(745, 493);
            this.Controls.Add(this.grp);
            this.Name = "IrDAReadout";
            this.StatusMessage = global::CABApplication.Properties.Resources.CCPrivateKey;
            this.Text = "IrDA Readout";
            this.Load += new System.EventHandler(this.frmIrDAReadouts_Load);
            this.grp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grp;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn slno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Param;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;

    }
}
