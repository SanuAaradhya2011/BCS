namespace CAB.UI
{
	partial class ExportConsumerData
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
			this.chkIncludeHeaders = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbtnASCII = new System.Windows.Forms.RadioButton();
			this.rbtnNormal = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbFileFormat = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkIncludeHeaders);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.btnCancel);
			this.groupBox1.Controls.Add(this.cmbFileFormat);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnExport);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(379, 201);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Export Consumer Data";
			// 
			// chkIncludeHeaders
			// 
			this.chkIncludeHeaders.AutoSize = true;
			this.chkIncludeHeaders.Location = new System.Drawing.Point(19, 149);
			this.chkIncludeHeaders.Name = "chkIncludeHeaders";
			this.chkIncludeHeaders.Size = new System.Drawing.Size(129, 17);
			this.chkIncludeHeaders.TabIndex = 33;
			this.chkIncludeHeaders.Text = "Include headers in file\r\n";
			this.chkIncludeHeaders.UseVisualStyleBackColor = true;
			this.chkIncludeHeaders.Visible = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbtnASCII);
			this.groupBox2.Controls.Add(this.rbtnNormal);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(19, 30);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(338, 81);
			this.groupBox2.TabIndex = 32;
			this.groupBox2.TabStop = false;
			// 
			// rbtnASCII
			// 
			this.rbtnASCII.AutoSize = true;
			this.rbtnASCII.Location = new System.Drawing.Point(220, 32);
			this.rbtnASCII.Name = "rbtnASCII";
			this.rbtnASCII.Size = new System.Drawing.Size(52, 17);
			this.rbtnASCII.TabIndex = 31;
			this.rbtnASCII.Text = "ASCII";
			this.rbtnASCII.UseVisualStyleBackColor = true;
			// 
			// rbtnNormal
			// 
			this.rbtnNormal.AutoSize = true;
			this.rbtnNormal.Checked = true;
			this.rbtnNormal.Location = new System.Drawing.Point(146, 32);
			this.rbtnNormal.Name = "rbtnNormal";
			this.rbtnNormal.Size = new System.Drawing.Size(58, 17);
			this.rbtnNormal.TabIndex = 30;
			this.rbtnNormal.TabStop = true;
			this.rbtnNormal.Text = "Normal";
			this.rbtnNormal.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(66, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 29;
			this.label2.Text = "File Format";
			// 
			// cmbFileFormat
			// 
			this.cmbFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFileFormat.FormattingEnabled = true;
			this.cmbFileFormat.Location = new System.Drawing.Point(101, 175);
			this.cmbFileFormat.Name = "cmbFileFormat";
			this.cmbFileFormat.Size = new System.Drawing.Size(171, 21);
			this.cmbFileFormat.TabIndex = 28;
			this.cmbFileFormat.Visible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 178);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 13);
			this.label1.TabIndex = 27;
			this.label1.Text = "Export Setting";
			this.label1.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(287, 131);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(70, 26);
			this.btnCancel.TabIndex = 20;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(213, 131);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(70, 26);
			this.btnExport.TabIndex = 18;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// ExportConsumerData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(379, 201);
			this.Controls.Add(this.groupBox1);
			this.Name = "ExportConsumerData";
			this.Text = "Consumer Export Data";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkIncludeHeaders;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbtnASCII;
		private System.Windows.Forms.RadioButton rbtnNormal;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbFileFormat;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnExport;
	}
}