namespace CAB.UI
{
	partial class ImportConsumerData
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
			this.txtFile = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnImport = new CAB.UI.Controls.CABButton();
			this.btnBrowse = new CAB.UI.Controls.CABButton();
			this.lngLabel1 = new CAB.UI.Controls.CABLabel();
			this.btnCancel = new CAB.UI.Controls.CABButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFile
			// 
			this.txtFile.Location = new System.Drawing.Point(77, 21);
			this.txtFile.Name = "txtFile";
			this.txtFile.Size = new System.Drawing.Size(145, 20);
			this.txtFile.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnImport);
			this.groupBox1.Controls.Add(this.btnBrowse);
			this.groupBox1.Controls.Add(this.lngLabel1);
			this.groupBox1.Controls.Add(this.btnCancel);
			this.groupBox1.Controls.Add(this.txtFile);
			this.groupBox1.Location = new System.Drawing.Point(12, 38);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(318, 106);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Import Consumer Data";
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(147, 63);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(75, 23);
			this.btnImport.TabIndex = 4;
			this.btnImport.Text = "&Import";
			this.btnImport.TranslationKey = "B000018";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(228, 20);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 3;
			this.btnBrowse.Text = "&Browse...";
			this.btnBrowse.TranslationKey = "B000011";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// lngLabel1
			// 
			this.lngLabel1.AutoSize = true;
			this.lngLabel1.Location = new System.Drawing.Point(15, 24);
			this.lngLabel1.Name = "lngLabel1";
			this.lngLabel1.Size = new System.Drawing.Size(56, 13);
			this.lngLabel1.TabIndex = 2;
			this.lngLabel1.Text = "Select File";
			this.lngLabel1.TranslationKey = "L000039";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(228, 63);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.TranslationKey = "B000009";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ImportConsumerData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 261);
			this.Controls.Add(this.groupBox1);
			this.Name = "ImportConsumerData";
			this.Text = "Import Consumer Data";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtFile;
		private CAB.UI.Controls.CABLabel lngLabel1;
		private CAB.UI.Controls.CABButton btnBrowse;
		private CAB.UI.Controls.CABButton btnImport;
		private CAB.UI.Controls.CABButton btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}