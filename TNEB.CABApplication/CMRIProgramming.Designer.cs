namespace CABApplication
{
	partial class CMRIProgramming
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
			this.btnCMRIReadout = new CAB.UI.Controls.CABButton();
			this.btnUpdateRTC = new CAB.UI.Controls.CABButton();
			this.btnPrepareCMRI = new CAB.UI.Controls.CABButton();
			this.btnCancel = new CAB.UI.Controls.CABButton();
			this.btnBrowse = new CAB.UI.Controls.CABButton();
			this.groupBoxCMRIProgramming = new System.Windows.Forms.GroupBox();
			this.groupBoxCMRIProgramming.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCMRIReadout
			// 
			this.btnCMRIReadout.Location = new System.Drawing.Point(63, 45);
			this.btnCMRIReadout.Name = "btnCMRIReadout";
			this.btnCMRIReadout.Size = new System.Drawing.Size(105, 29);
			this.btnCMRIReadout.TabIndex = 0;
			this.btnCMRIReadout.Text = "Read CMRI Data";
			this.btnCMRIReadout.TranslationKey = null;
			this.btnCMRIReadout.UseVisualStyleBackColor = true;
			this.btnCMRIReadout.Click += new System.EventHandler(this.btnCMRIReadout_Click);
			// 
			// btnUpdateRTC
			// 
			this.btnUpdateRTC.Location = new System.Drawing.Point(63, 80);
			this.btnUpdateRTC.Name = "btnUpdateRTC";
			this.btnUpdateRTC.Size = new System.Drawing.Size(105, 29);
			this.btnUpdateRTC.TabIndex = 1;
			this.btnUpdateRTC.Text = "Update RTC";
			this.btnUpdateRTC.TranslationKey = null;
			this.btnUpdateRTC.UseVisualStyleBackColor = true;
			// 
			// btnPrepareCMRI
			// 
			this.btnPrepareCMRI.Location = new System.Drawing.Point(63, 115);
			this.btnPrepareCMRI.Name = "btnPrepareCMRI";
			this.btnPrepareCMRI.Size = new System.Drawing.Size(105, 29);
			this.btnPrepareCMRI.TabIndex = 2;
			this.btnPrepareCMRI.Text = "Prepare CMRI";
			this.btnPrepareCMRI.TranslationKey = null;
			this.btnPrepareCMRI.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(63, 150);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(105, 29);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "cancel";
			this.btnCancel.TranslationKey = null;
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(174, 45);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(54, 29);
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.TranslationKey = null;
			this.btnBrowse.UseVisualStyleBackColor = true;
			// 
			// groupBoxCMRIProgramming
			// 
			this.groupBoxCMRIProgramming.Controls.Add(this.btnBrowse);
			this.groupBoxCMRIProgramming.Controls.Add(this.btnCMRIReadout);
			this.groupBoxCMRIProgramming.Controls.Add(this.btnCancel);
			this.groupBoxCMRIProgramming.Controls.Add(this.btnUpdateRTC);
			this.groupBoxCMRIProgramming.Controls.Add(this.btnPrepareCMRI);
			this.groupBoxCMRIProgramming.Location = new System.Drawing.Point(75, 50);
			this.groupBoxCMRIProgramming.Name = "groupBoxCMRIProgramming";
			this.groupBoxCMRIProgramming.Size = new System.Drawing.Size(290, 225);
			this.groupBoxCMRIProgramming.TabIndex = 5;
			this.groupBoxCMRIProgramming.TabStop = false;
			this.groupBoxCMRIProgramming.Text = "CMRI Programming";
			// 
			// CMRIProgramming
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(441, 324);
			this.ControlBox = false;
			this.Controls.Add(this.groupBoxCMRIProgramming);
			this.Name = "CMRIProgramming";
			this.Text = "CMRI Programming";
			this.groupBoxCMRIProgramming.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private CAB.UI.Controls.CABButton btnCMRIReadout;
		private CAB.UI.Controls.CABButton btnUpdateRTC;
		private CAB.UI.Controls.CABButton btnPrepareCMRI;
		private CAB.UI.Controls.CABButton btnCancel;
		private CAB.UI.Controls.CABButton btnBrowse;
		private System.Windows.Forms.GroupBox groupBoxCMRIProgramming;
	}
}