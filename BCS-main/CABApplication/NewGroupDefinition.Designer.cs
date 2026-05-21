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
            this.txtSubGroupName.Location = new System.Drawing.Point(124, 54);
            this.txtSubGroupName.MaxLength = 35;
            this.txtSubGroupName.Name = "txtSubGroupName";
            this.txtSubGroupName.Size = new System.Drawing.Size(159, 20);
            this.txtSubGroupName.TabIndex = 3;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(365, 54);
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
            this.grpBoxDefinition.Size = new System.Drawing.Size(625, 396);
            this.grpBoxDefinition.TabIndex = 6;
            this.grpBoxDefinition.TabStop = false;
            this.grpBoxDefinition.Text = "Group Definition";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(544, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TranslationKey = "B000009";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(463, 359);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.TranslationKey = "B000008";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
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
            this.grpBoxSelectMeters.Location = new System.Drawing.Point(6, 87);
            this.grpBoxSelectMeters.Name = "grpBoxSelectMeters";
            this.grpBoxSelectMeters.Size = new System.Drawing.Size(613, 264);
            this.grpBoxSelectMeters.TabIndex = 6;
            this.grpBoxSelectMeters.TabStop = false;
            this.grpBoxSelectMeters.Text = "✅  Selected Meters";
            this.grpBoxSelectMeters.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpBoxSelectMeters.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
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
            this.lngLabel1.Text = "📋  Available Meters";
            this.lngLabel1.TranslationKey = "L000047";
            this.lngLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lngLabel1.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            // 
            // lngButton4
            // 
            this.lngButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton4.Location = new System.Drawing.Point(281, 169);
            this.lngButton4.Name = "lngButton4";
            this.lngButton4.Size = new System.Drawing.Size(50, 28);
            this.lngButton4.TabIndex = 5;
            this.lngButton4.Text = "<<";
            this.lngButton4.TranslationKey = null;
            this.lngButton4.UseVisualStyleBackColor = false;
            this.lngButton4.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngButton4.ForeColor = System.Drawing.Color.White;
            this.lngButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngButton4.FlatAppearance.BorderSize = 0;
            this.lngButton4.Click += new System.EventHandler(this.lngButton4_Click);
            // 
            // lngButton3
            // 
            this.lngButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton3.Location = new System.Drawing.Point(281, 139);
            this.lngButton3.Name = "lngButton3";
            this.lngButton3.Size = new System.Drawing.Size(50, 28);
            this.lngButton3.TabIndex = 4;
            this.lngButton3.Text = "<";
            this.lngButton3.TranslationKey = null;
            this.lngButton3.UseVisualStyleBackColor = false;
            this.lngButton3.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngButton3.ForeColor = System.Drawing.Color.White;
            this.lngButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngButton3.FlatAppearance.BorderSize = 0;
            this.lngButton3.Click += new System.EventHandler(this.lngButton3_Click);
            // 
            // lngButton2
            // 
            this.lngButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton2.Location = new System.Drawing.Point(281, 109);
            this.lngButton2.Name = "lngButton2";
            this.lngButton2.Size = new System.Drawing.Size(50, 28);
            this.lngButton2.TabIndex = 3;
            this.lngButton2.Text = "◀  Remove";
            this.lngButton2.Text = "<";
            this.lngButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lngButton2.TranslationKey = null;
            this.lngButton2.UseVisualStyleBackColor = false;
            this.lngButton2.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngButton2.ForeColor = System.Drawing.Color.White;
            this.lngButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngButton2.FlatAppearance.BorderSize = 0;
            this.lngButton2.Click += new System.EventHandler(this.lngButton2_Click);
            this.lngButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(195, 60, 60);
            this.lngButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // lngButton1
            // 
            this.lngButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngButton1.Location = new System.Drawing.Point(281, 79);
            this.lngButton1.Name = "lngButton1";
            this.lngButton1.Size = new System.Drawing.Size(50, 28);
            this.lngButton1.TabIndex = 2;
            this.lngButton1.Text = "  ▶  Add";
            this.lngButton1.Text = ">";
            this.lngButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lngButton1.TranslationKey = null;
            this.lngButton1.UseVisualStyleBackColor = false;
            this.lngButton1.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngButton1.ForeColor = System.Drawing.Color.White;
            this.lngButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngButton1.FlatAppearance.BorderSize = 0;
            this.lngButton1.Click += new System.EventHandler(this.lngButton1_Click);
            this.lngButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.lngButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(359, 36);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(222, 212);
            this.listBoxSelectedMeters.TabIndex = 1;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(26, 36);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(222, 212);
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
            this.lblDescription.Location = new System.Drawing.Point(299, 57);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description";
            this.lblDescription.TranslationKey = null;
            // 
            // lblSubGroupName
            // 
            this.lblSubGroupName.AutoSize = true;
            this.lblSubGroupName.Location = new System.Drawing.Point(29, 57);
            this.lblSubGroupName.Name = "lblSubGroupName";
            this.lblSubGroupName.Size = new System.Drawing.Size(89, 13);
            this.lblSubGroupName.TabIndex = 2;
            this.lblSubGroupName.Text = "Sub-Group Name";
            this.lblSubGroupName.TranslationKey = "L000056";
            // 
            // NewGroupDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 417);
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
