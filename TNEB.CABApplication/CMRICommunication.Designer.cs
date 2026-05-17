namespace CAB.UI
{
	partial class CMRICommunication
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
            this.btnClearCMRI = new CAB.UI.Controls.CABButton();
            this.lblTOUFile = new CAB.UI.Controls.CABLabel();
            this.btnBrowse = new CAB.UI.Controls.CABButton();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.btnUpdateRTC = new CAB.UI.Controls.CABButton();
            this.btnReadData = new CAB.UI.Controls.CABButton();
            this.btnPrepareCMRI = new CAB.UI.Controls.CABButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClearCMRI);
            this.groupBox1.Controls.Add(this.lblTOUFile);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnUpdateRTC);
            this.groupBox1.Controls.Add(this.btnReadData);
            this.groupBox1.Controls.Add(this.btnPrepareCMRI);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 303);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CMRI Communication";
            // 
            // btnClearCMRI
            // 
            this.btnClearCMRI.Location = new System.Drawing.Point(50, 194);
            this.btnClearCMRI.Name = "btnClearCMRI";
            this.btnClearCMRI.Size = new System.Drawing.Size(187, 34);
            this.btnClearCMRI.TabIndex = 11;
            this.btnClearCMRI.Text = "C&lear CMRI";
            this.btnClearCMRI.TranslationKey = "B000019";
            this.btnClearCMRI.UseVisualStyleBackColor = true;
            this.btnClearCMRI.Click += new System.EventHandler(this.btnClearCMRI_Click);
            // 
            // lblTOUFile
            // 
            this.lblTOUFile.AutoSize = true;
            this.lblTOUFile.Location = new System.Drawing.Point(183, 287);
            this.lblTOUFile.Name = "lblTOUFile";
            this.lblTOUFile.Size = new System.Drawing.Size(54, 13);
            this.lblTOUFile.TabIndex = 10;
            this.lblTOUFile.Text = "File Name";
            this.lblTOUFile.TranslationKey = null;
            this.lblTOUFile.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(243, 74);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(63, 34);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.TranslationKey = "B000011";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(50, 234);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(187, 34);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TranslationKey = "B000009";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdateRTC
            // 
            this.btnUpdateRTC.Location = new System.Drawing.Point(50, 154);
            this.btnUpdateRTC.Name = "btnUpdateRTC";
            this.btnUpdateRTC.Size = new System.Drawing.Size(187, 34);
            this.btnUpdateRTC.TabIndex = 7;
            this.btnUpdateRTC.Text = "Update RTC";
            this.btnUpdateRTC.TranslationKey = "B000017";
            this.btnUpdateRTC.UseVisualStyleBackColor = true;
            this.btnUpdateRTC.Click += new System.EventHandler(this.btnUpdateRTC_Click);
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(50, 114);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(187, 34);
            this.btnReadData.TabIndex = 6;
            this.btnReadData.Text = "Read Data";
            this.btnReadData.TranslationKey = "B000016";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnPrepareCMRI
            // 
            this.btnPrepareCMRI.Location = new System.Drawing.Point(50, 74);
            this.btnPrepareCMRI.Name = "btnPrepareCMRI";
            this.btnPrepareCMRI.Size = new System.Drawing.Size(187, 34);
            this.btnPrepareCMRI.TabIndex = 5;
            this.btnPrepareCMRI.Text = "Prepare CMRI";
            this.btnPrepareCMRI.TranslationKey = "B000015";
            this.btnPrepareCMRI.UseVisualStyleBackColor = true;
            this.btnPrepareCMRI.Click += new System.EventHandler(this.btnPrepareCMRI_Click);
            // 
            // CMRICommunication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(380, 327);
            this.Controls.Add(this.groupBox1);
            this.Name = "CMRICommunication";
            this.StatusMessage = "";
            this.Text = "CMRI Communication";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMRICommunication_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private CAB.UI.Controls.CABButton btnPrepareCMRI;
		private CAB.UI.Controls.CABButton btnBrowse;
		private CAB.UI.Controls.CABButton btnCancel;
		private CAB.UI.Controls.CABButton btnUpdateRTC;
		private CAB.UI.Controls.CABButton btnReadData;
		private CAB.UI.Controls.CABLabel lblTOUFile;
		private CAB.UI.Controls.CABButton btnClearCMRI;
	}
}