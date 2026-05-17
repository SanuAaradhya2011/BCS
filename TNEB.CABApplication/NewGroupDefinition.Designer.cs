namespace CAB.UI
{
	partial class NewGroupDefinition
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
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.txtSubGroupName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.grpBoxDefinition = new System.Windows.Forms.GroupBox();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.btnSave = new CAB.UI.Controls.CABButton();
            this.grpBoxSelectMeters = new System.Windows.Forms.GroupBox();
            this.lngLabel2 = new CAB.UI.Controls.CABLabel();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.lngButton4 = new CAB.UI.Controls.CABButton();
            this.lngButton3 = new CAB.UI.Controls.CABButton();
            this.lngButton2 = new CAB.UI.Controls.CABButton();
            this.lngButton1 = new CAB.UI.Controls.CABButton();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.lblGroupName = new CAB.UI.Controls.CABLabel();
            this.lblDescription = new CAB.UI.Controls.CABLabel();
            this.lblSubGroupName = new CAB.UI.Controls.CABLabel();
            this.grpBoxDefinition.SuspendLayout();
            this.grpBoxSelectMeters.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(124, 21);
            this.txtGroupName.MaxLength = 35;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.ReadOnly = true;
            this.txtGroupName.Size = new System.Drawing.Size(159, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // txtSubGroupName
            // 
            this.txtSubGroupName.Location = new System.Drawing.Point(124, 59);
            this.txtSubGroupName.MaxLength = 35;
            this.txtSubGroupName.Name = "txtSubGroupName";
            this.txtSubGroupName.Size = new System.Drawing.Size(159, 20);
            this.txtSubGroupName.TabIndex = 3;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(365, 59);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(231, 20);
            this.txtDescription.TabIndex = 5;
            // 
            // grpBoxDefinition
            // 
            this.grpBoxDefinition.Controls.Add(this.btnCancel);
            this.grpBoxDefinition.Controls.Add(this.btnSave);
            this.grpBoxDefinition.Controls.Add(this.grpBoxSelectMeters);
            this.grpBoxDefinition.Controls.Add(this.txtDescription);
            this.grpBoxDefinition.Controls.Add(this.lblGroupName);
            this.grpBoxDefinition.Controls.Add(this.lblDescription);
            this.grpBoxDefinition.Controls.Add(this.txtGroupName);
            this.grpBoxDefinition.Controls.Add(this.txtSubGroupName);
            this.grpBoxDefinition.Controls.Add(this.lblSubGroupName);
            this.grpBoxDefinition.Location = new System.Drawing.Point(12, 12);
            this.grpBoxDefinition.Name = "grpBoxDefinition";
            this.grpBoxDefinition.Size = new System.Drawing.Size(625, 444);
            this.grpBoxDefinition.TabIndex = 6;
            this.grpBoxDefinition.TabStop = false;
            this.grpBoxDefinition.Text = "Group Definition";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(544, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TranslationKey = "B000009";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(463, 400);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.TranslationKey = "B000008";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpBoxSelectMeters
            // 
            this.grpBoxSelectMeters.Controls.Add(this.lngLabel2);
            this.grpBoxSelectMeters.Controls.Add(this.lngLabel1);
            this.grpBoxSelectMeters.Controls.Add(this.lngButton4);
            this.grpBoxSelectMeters.Controls.Add(this.lngButton3);
            this.grpBoxSelectMeters.Controls.Add(this.lngButton2);
            this.grpBoxSelectMeters.Controls.Add(this.lngButton1);
            this.grpBoxSelectMeters.Controls.Add(this.listBoxSelectedMeters);
            this.grpBoxSelectMeters.Controls.Add(this.listBoxAvailableMeters);
            this.grpBoxSelectMeters.Location = new System.Drawing.Point(6, 101);
            this.grpBoxSelectMeters.Name = "grpBoxSelectMeters";
            this.grpBoxSelectMeters.Size = new System.Drawing.Size(613, 293);
            this.grpBoxSelectMeters.TabIndex = 6;
            this.grpBoxSelectMeters.TabStop = false;
            this.grpBoxSelectMeters.Text = "Selected Meters";
            // 
            // lngLabel2
            // 
            this.lngLabel2.AutoSize = true;
            this.lngLabel2.Location = new System.Drawing.Point(356, 16);
            this.lngLabel2.Name = "lngLabel2";
            this.lngLabel2.Size = new System.Drawing.Size(79, 13);
            this.lngLabel2.TabIndex = 7;
            this.lngLabel2.Text = "Selected Meter";
            this.lngLabel2.TranslationKey = "L000048";
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(23, 16);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(80, 13);
            this.lngLabel1.TabIndex = 6;
            this.lngLabel1.Text = "Available Meter";
            this.lngLabel1.TranslationKey = "L000047";
            // 
            // lngButton4
            // 
            this.lngButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton4.Location = new System.Drawing.Point(281, 177);
            this.lngButton4.Name = "lngButton4";
            this.lngButton4.Size = new System.Drawing.Size(50, 28);
            this.lngButton4.TabIndex = 5;
            this.lngButton4.Text = "<<";
            this.lngButton4.TranslationKey = null;
            this.lngButton4.UseVisualStyleBackColor = true;
            this.lngButton4.Click += new System.EventHandler(this.lngButton4_Click);
            // 
            // lngButton3
            // 
            this.lngButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton3.Location = new System.Drawing.Point(281, 147);
            this.lngButton3.Name = "lngButton3";
            this.lngButton3.Size = new System.Drawing.Size(50, 28);
            this.lngButton3.TabIndex = 4;
            this.lngButton3.Text = "<";
            this.lngButton3.TranslationKey = null;
            this.lngButton3.UseVisualStyleBackColor = true;
            this.lngButton3.Click += new System.EventHandler(this.lngButton3_Click);
            // 
            // lngButton2
            // 
            this.lngButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton2.Location = new System.Drawing.Point(281, 117);
            this.lngButton2.Name = "lngButton2";
            this.lngButton2.Size = new System.Drawing.Size(50, 28);
            this.lngButton2.TabIndex = 3;
            this.lngButton2.Text = ">>";
            this.lngButton2.TranslationKey = null;
            this.lngButton2.UseVisualStyleBackColor = true;
            this.lngButton2.Click += new System.EventHandler(this.lngButton2_Click);
            // 
            // lngButton1
            // 
            this.lngButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton1.Location = new System.Drawing.Point(281, 87);
            this.lngButton1.Name = "lngButton1";
            this.lngButton1.Size = new System.Drawing.Size(50, 28);
            this.lngButton1.TabIndex = 2;
            this.lngButton1.Text = ">";
            this.lngButton1.TranslationKey = null;
            this.lngButton1.UseVisualStyleBackColor = true;
            this.lngButton1.Click += new System.EventHandler(this.lngButton1_Click);
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(359, 36);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(222, 251);
            this.listBoxSelectedMeters.TabIndex = 1;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(26, 36);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(222, 251);
            this.listBoxAvailableMeters.TabIndex = 0;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(29, 24);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(67, 13);
            this.lblGroupName.TabIndex = 0;
            this.lblGroupName.Text = "Group Name";
            this.lblGroupName.TranslationKey = "L000055";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(299, 62);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description";
            this.lblDescription.TranslationKey = null;
            // 
            // lblSubGroupName
            // 
            this.lblSubGroupName.AutoSize = true;
            this.lblSubGroupName.Location = new System.Drawing.Point(29, 62);
            this.lblSubGroupName.Name = "lblSubGroupName";
            this.lblSubGroupName.Size = new System.Drawing.Size(89, 13);
            this.lblSubGroupName.TabIndex = 2;
            this.lblSubGroupName.Text = "Sub-Group Name";
            this.lblSubGroupName.TranslationKey = "L000056";
            // 
            // NewGroupDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(649, 468);
            this.Controls.Add(this.grpBoxDefinition);
            this.Name = "NewGroupDefinition";
            this.StatusMessage = "";
            this.Text = "New Group Definition";
            this.Load += new System.EventHandler(this.NewGroupDefinition_Load);
            this.grpBoxDefinition.ResumeLayout(false);
            this.grpBoxDefinition.PerformLayout();
            this.grpBoxSelectMeters.ResumeLayout(false);
            this.grpBoxSelectMeters.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private CAB.UI.Controls.CABLabel lblGroupName;
		private System.Windows.Forms.TextBox txtGroupName;
		private System.Windows.Forms.TextBox txtSubGroupName;
		private CAB.UI.Controls.CABLabel lblSubGroupName;
		private System.Windows.Forms.TextBox txtDescription;
		private CAB.UI.Controls.CABLabel lblDescription;
		private System.Windows.Forms.GroupBox grpBoxDefinition;
		private System.Windows.Forms.GroupBox grpBoxSelectMeters;
		private CAB.UI.Controls.CABButton btnCancel;
		private System.Windows.Forms.ListBox listBoxSelectedMeters;
		private System.Windows.Forms.ListBox listBoxAvailableMeters;
		private CAB.UI.Controls.CABButton lngButton4;
		private CAB.UI.Controls.CABButton lngButton3;
		private CAB.UI.Controls.CABButton lngButton2;
		private CAB.UI.Controls.CABButton lngButton1;
		private CAB.UI.Controls.CABLabel lngLabel2;
		private CAB.UI.Controls.CABLabel lngLabel1;
		public CAB.UI.Controls.CABButton btnSave;

	}
}