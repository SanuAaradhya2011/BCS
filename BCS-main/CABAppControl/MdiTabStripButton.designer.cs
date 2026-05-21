namespace CAB.UI.Controls
{
	partial class MdiTabStripButton
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
            this.mdiTabStrip1 = new CAB.UI.Controls.MdiTabStrip();
            // 
            // mdiTabStrip1
            // 
            this.mdiTabStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mdiTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.mdiTabStrip1.Name = "mdiTabStrip1";
            this.mdiTabStrip1.SelectedTab = null;
            this.mdiTabStrip1.ShowItemToolTips = false;
            this.mdiTabStrip1.Size = new System.Drawing.Size(100, 25);
            this.mdiTabStrip1.TabIndex = 0;
            this.mdiTabStrip1.Text = "mdiTabStrip1";

		}

		#endregion

        private MdiTabStrip mdiTabStrip1;
	}
}
