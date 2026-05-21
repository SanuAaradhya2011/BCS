namespace CABAppControl
{
	partial class GroupRadioBox
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
            this.radioGroupBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // radioGroupBox
            // 
            this.radioGroupBox.BackColor = System.Drawing.SystemColors.Window;
            this.radioGroupBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.radioGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioGroupBox.Location = new System.Drawing.Point(0, 0);
            this.radioGroupBox.Name = "radioGroupBox";
            this.radioGroupBox.Size = new System.Drawing.Size(325, 274);
            this.radioGroupBox.TabIndex = 0;
            this.radioGroupBox.TabStop = false;
            this.radioGroupBox.Text = "MeterID";
            // 
            // GroupRadioBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.radioGroupBox);
            this.Name = "GroupRadioBox";
            this.Size = new System.Drawing.Size(325, 277);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox radioGroupBox;
	}
}
