namespace CABApplication
{
    partial class ABCDecryption
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_EnterMID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRpt = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnexit = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgtamperstatus = new System.Windows.Forms.DataGridView();
            this.Tamper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.txtct = new System.Windows.Forms.TextBox();
            this.txtmd = new System.Windows.Forms.TextBox();
            this.txtkwh = new System.Windows.Forms.TextBox();
            this.txtmid = new System.Windows.Forms.TextBox();
            this.lblbmd = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblmid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblbkwh = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnselect = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgtamperstatus)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_EnterMID);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnRpt);
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.btnexit);
            this.groupBox3.Controls.Add(this.btnDecrypt);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btnselect);
            this.groupBox3.Controls.Add(this.txtCode);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(508, 493);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Authenticated Billing code Decryption";
            // 
            // txt_EnterMID
            // 
            this.txt_EnterMID.BackColor = System.Drawing.Color.White;
            this.txt_EnterMID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_EnterMID.Location = new System.Drawing.Point(162, 79);
            this.txt_EnterMID.MaxLength = 8;
            this.txt_EnterMID.Name = "txt_EnterMID";
            this.txt_EnterMID.Size = new System.Drawing.Size(165, 22);
            this.txt_EnterMID.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(80, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Enter Meter ID";
            // 
            // btnRpt
            // 
            this.btnRpt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRpt.Location = new System.Drawing.Point(219, 406);
            this.btnRpt.Name = "btnRpt";
            this.btnRpt.Size = new System.Drawing.Size(55, 35);
            this.btnRpt.TabIndex = 6;
            this.btnRpt.Text = "Export";
            this.btnRpt.UseVisualStyleBackColor = false;
            this.btnRpt.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnRpt.ForeColor = System.Drawing.Color.White;
            this.btnRpt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRpt.FlatAppearance.BorderSize = 0;
            this.btnRpt.Click += new System.EventHandler(this.btnRpt_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(276, 406);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(55, 35);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnexit
            // 
            this.btnexit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnexit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexit.Location = new System.Drawing.Point(333, 406);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(55, 35);
            this.btnexit.TabIndex = 8;
            this.btnexit.Text = "&Cancel";
            this.btnexit.UseVisualStyleBackColor = false;
            this.btnexit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnexit.ForeColor = System.Drawing.Color.White;
            this.btnexit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnexit.FlatAppearance.BorderSize = 0;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.Location = new System.Drawing.Point(161, 406);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(55, 35);
            this.btnDecrypt.TabIndex = 5;
            this.btnDecrypt.Text = "&Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = false;
            this.btnDecrypt.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnDecrypt.ForeColor = System.Drawing.Color.White;
            this.btnDecrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecrypt.FlatAppearance.BorderSize = 0;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgtamperstatus);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txtct);
            this.groupBox4.Controls.Add(this.txtmd);
            this.groupBox4.Controls.Add(this.txtkwh);
            this.groupBox4.Controls.Add(this.txtmid);
            this.groupBox4.Controls.Add(this.lblbmd);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.lblmid);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.lblbkwh);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(6, 114);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(481, 291);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // dgtamperstatus
            // 
            this.dgtamperstatus.AllowUserToAddRows = false;
            this.dgtamperstatus.AllowUserToDeleteRows = false;
            this.dgtamperstatus.AllowUserToResizeRows = false;
            this.dgtamperstatus.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgtamperstatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgtamperstatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Tamper,
            this.Status});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgtamperstatus.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgtamperstatus.Location = new System.Drawing.Point(25, 19);
            this.dgtamperstatus.Name = "dgtamperstatus";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgtamperstatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgtamperstatus.RowHeadersVisible = false;
            this.dgtamperstatus.Size = new System.Drawing.Size(355, 264);
            this.dgtamperstatus.TabIndex = 26;
            // 
            // Tamper
            // 
            this.Tamper.Frozen = true;
            this.Tamper.HeaderText = "Parameters";
            this.Tamper.Name = "Tamper";
            this.Tamper.ReadOnly = true;
            this.Tamper.Width = 200;
            // 
            // Status
            // 
            this.Status.Frozen = true;
            this.Status.HeaderText = "Value";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 150;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(54, -17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Meter ID";
            // 
            // txtct
            // 
            this.txtct.BackColor = System.Drawing.Color.White;
            this.txtct.Location = new System.Drawing.Point(433, 128);
            this.txtct.Name = "txtct";
            this.txtct.ReadOnly = true;
            this.txtct.Size = new System.Drawing.Size(100, 20);
            this.txtct.TabIndex = 24;
            this.txtct.Visible = false;
            this.txtct.WordWrap = false;
            // 
            // txtmd
            // 
            this.txtmd.BackColor = System.Drawing.Color.White;
            this.txtmd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmd.Location = new System.Drawing.Point(432, 188);
            this.txtmd.Name = "txtmd";
            this.txtmd.ReadOnly = true;
            this.txtmd.Size = new System.Drawing.Size(100, 20);
            this.txtmd.TabIndex = 25;
            this.txtmd.Visible = false;
            // 
            // txtkwh
            // 
            this.txtkwh.BackColor = System.Drawing.Color.White;
            this.txtkwh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtkwh.Location = new System.Drawing.Point(408, 221);
            this.txtkwh.Name = "txtkwh";
            this.txtkwh.ReadOnly = true;
            this.txtkwh.Size = new System.Drawing.Size(100, 20);
            this.txtkwh.TabIndex = 24;
            this.txtkwh.Visible = false;
            // 
            // txtmid
            // 
            this.txtmid.BackColor = System.Drawing.Color.White;
            this.txtmid.Location = new System.Drawing.Point(433, 154);
            this.txtmid.Name = "txtmid";
            this.txtmid.ReadOnly = true;
            this.txtmid.Size = new System.Drawing.Size(100, 20);
            this.txtmid.TabIndex = 23;
            this.txtmid.Visible = false;
            // 
            // lblbmd
            // 
            this.lblbmd.AutoSize = true;
            this.lblbmd.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblbmd.Location = new System.Drawing.Point(466, 237);
            this.lblbmd.Name = "lblbmd";
            this.lblbmd.Size = new System.Drawing.Size(28, 16);
            this.lblbmd.TabIndex = 5;
            this.lblbmd.Text = "MD";
            this.lblbmd.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(355, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Billing MD (kW)";
            this.label6.Visible = false;
            // 
            // lblmid
            // 
            this.lblmid.AutoSize = true;
            this.lblmid.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmid.Location = new System.Drawing.Point(500, 98);
            this.lblmid.Name = "lblmid";
            this.lblmid.Size = new System.Drawing.Size(33, 16);
            this.lblmid.TabIndex = 3;
            this.lblmid.Text = "MID";
            this.lblmid.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(446, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Meter ID";
            this.label4.Visible = false;
            // 
            // lblbkwh
            // 
            this.lblbkwh.AutoSize = true;
            this.lblbkwh.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblbkwh.Location = new System.Drawing.Point(500, 237);
            this.lblbkwh.Name = "lblbkwh";
            this.lblbkwh.Size = new System.Drawing.Size(36, 16);
            this.lblbkwh.TabIndex = 1;
            this.lblbkwh.Text = "kWh";
            this.lblbkwh.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(366, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Billing kWh";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter 20 Digit ABC Code";
            // 
            // btnselect
            // 
            this.btnselect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnselect.Location = new System.Drawing.Point(458, 41);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(56, 23);
            this.btnselect.TabIndex = 6;
            this.btnselect.Text = "&Select...";
            this.btnselect.UseVisualStyleBackColor = false;
            this.btnselect.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnselect.ForeColor = System.Drawing.Color.White;
            this.btnselect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnselect.FlatAppearance.BorderSize = 0;
            this.btnselect.Visible = false;
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(162, 41);
            this.txtCode.MaxLength = 20;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(165, 21);
            this.txtCode.TabIndex = 0;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCode_KeyPress);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 505);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // ABCDecryption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 505);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ABCDecryption";
            this.StatusMessage = "";
            this.Text = "ABC-1 Decryption";
            this.Load += new System.EventHandler(this.ABCDecryption_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgtamperstatus)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.TextBox txt_EnterMID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRpt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnexit;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtct;
        public System.Windows.Forms.TextBox txtmd;
        public System.Windows.Forms.TextBox txtkwh;
        public System.Windows.Forms.TextBox txtmid;
        private System.Windows.Forms.Label lblbmd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblmid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblbkwh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnselect;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgtamperstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tamper;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}
