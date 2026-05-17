namespace CABAppControl
{
    partial class RTC
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dGVReadRTC = new System.Windows.Forms.DataGridView();
            this.colSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRTC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReadRTC = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxRTC = new System.Windows.Forms.TextBox();
            this.btnWriteRTC = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRTC = new System.Windows.Forms.TextBox();
            this.dGridRTC = new System.Windows.Forms.DataGridView();
            this.grpBoxRTC = new System.Windows.Forms.GroupBox();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVReadRTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGridRTC)).BeginInit();
            this.grpBoxRTC.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.583333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.41666F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dGVReadRTC);
            this.groupBox2.Controls.Add(this.btnReadRTC);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtBoxRTC);
            this.groupBox2.Controls.Add(this.btnWriteRTC);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(5, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(44, 118);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Update RTC";
            // 
            // dGVReadRTC
            // 
            this.dGVReadRTC.AllowUserToAddRows = false;
            this.dGVReadRTC.AllowUserToDeleteRows = false;
            this.dGVReadRTC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVReadRTC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSNo,
            this.colRTC});
            this.dGVReadRTC.Location = new System.Drawing.Point(428, 58);
            this.dGVReadRTC.Name = "dGVReadRTC";
            this.dGVReadRTC.ReadOnly = true;
            this.dGVReadRTC.Size = new System.Drawing.Size(301, 263);
            this.dGVReadRTC.TabIndex = 9;
            // 
            // colSNo
            // 
            this.colSNo.HeaderText = "S.No.";
            this.colSNo.Name = "colSNo";
            this.colSNo.ReadOnly = true;
            this.colSNo.Width = 80;
            // 
            // colRTC
            // 
            this.colRTC.HeaderText = "Real Time Clock";
            this.colRTC.Name = "colRTC";
            this.colRTC.ReadOnly = true;
            this.colRTC.Width = 175;
            // 
            // btnReadRTC
            // 
            this.btnReadRTC.Enabled = false;
            this.btnReadRTC.Location = new System.Drawing.Point(254, 98);
            this.btnReadRTC.Name = "btnReadRTC";
            this.btnReadRTC.Size = new System.Drawing.Size(75, 26);
            this.btnReadRTC.TabIndex = 8;
            this.btnReadRTC.Text = "Read";
            this.btnReadRTC.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, -91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Real Time Clock";
            // 
            // txtBoxRTC
            // 
            this.txtBoxRTC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxRTC.Location = new System.Drawing.Point(173, -94);
            this.txtBoxRTC.MaxLength = 8;
            this.txtBoxRTC.Name = "txtBoxRTC";
            this.txtBoxRTC.Size = new System.Drawing.Size(156, 20);
            this.txtBoxRTC.TabIndex = 5;
            // 
            // btnWriteRTC
            // 
            this.btnWriteRTC.Enabled = false;
            this.btnWriteRTC.Location = new System.Drawing.Point(173, 98);
            this.btnWriteRTC.Name = "btnWriteRTC";
            this.btnWriteRTC.Size = new System.Drawing.Size(75, 26);
            this.btnWriteRTC.TabIndex = 6;
            this.btnWriteRTC.Text = "Write";
            this.btnWriteRTC.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Real Time Clock";
            // 
            // txtRTC
            // 
            this.txtRTC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRTC.Enabled = false;
            this.txtRTC.Location = new System.Drawing.Point(219, 32);
            this.txtRTC.MaxLength = 8;
            this.txtRTC.Name = "txtRTC";
            this.txtRTC.Size = new System.Drawing.Size(175, 20);
            this.txtRTC.TabIndex = 11;
            // 
            // dGridRTC
            // 
            this.dGridRTC.AllowUserToAddRows = false;
            this.dGridRTC.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGridRTC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGridRTC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGridRTC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dGridRTC.Location = new System.Drawing.Point(100, 73);
            this.dGridRTC.Name = "dGridRTC";
            this.dGridRTC.ReadOnly = true;
            this.dGridRTC.Size = new System.Drawing.Size(294, 255);
            this.dGridRTC.TabIndex = 14;
            // 
            // grpBoxRTC
            // 
            this.grpBoxRTC.Controls.Add(this.label1);
            this.grpBoxRTC.Controls.Add(this.dGridRTC);
            this.grpBoxRTC.Controls.Add(this.txtRTC);
            this.grpBoxRTC.Location = new System.Drawing.Point(7, 3);
            this.grpBoxRTC.Name = "grpBoxRTC";
            this.grpBoxRTC.Size = new System.Drawing.Size(493, 372);
            this.grpBoxRTC.TabIndex = 15;
            this.grpBoxRTC.TabStop = false;
            this.grpBoxRTC.Text = "Update RTC";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "S.No.";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Real Time Clock";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 175;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Real Time Clock";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "S.No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // RTC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBoxRTC);
            this.Name = "RTC";
            this.Size = new System.Drawing.Size(500, 384);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVReadRTC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGridRTC)).EndInit();
            this.grpBoxRTC.ResumeLayout(false);
            this.grpBoxRTC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dGVReadRTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRTC;
        private System.Windows.Forms.Button btnReadRTC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxRTC;
        private System.Windows.Forms.Button btnWriteRTC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRTC;
        private System.Windows.Forms.DataGridView dGridRTC;
        private System.Windows.Forms.GroupBox grpBoxRTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}
