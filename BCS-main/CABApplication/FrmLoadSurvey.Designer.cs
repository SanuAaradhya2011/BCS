namespace CAB.UI
{
	partial class FrmLoadSurvey
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
			this.graphFormControl1 = new CAB.UI.Graphs.GraphFormControl();
			this.SuspendLayout();
			// 
			// graphFormControl1
			// 
			this.graphFormControl1.AutoScroll = true;
			this.graphFormControl1.FileName = null;
			this.graphFormControl1.Location = new System.Drawing.Point(0, 0);
			this.graphFormControl1.Margin = new System.Windows.Forms.Padding(0);
			this.graphFormControl1.MeterDataID = null;
			//this.graphFormControl1.MeterID = null;
			this.graphFormControl1.Name = "graphFormControl1";
			this.graphFormControl1.Size = new System.Drawing.Size(1287, 712);
			this.graphFormControl1.TabIndex = 0;
			// 
			// FrmLoadSurvey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1291, 713);
			this.ControlBox = true;
			this.Controls.Add(this.graphFormControl1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "FrmLoadSurvey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Load Survey Graph";
			this.Load += new System.EventHandler(this.FrmLoadSurvey_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private CAB.UI.Graphs.GraphFormControl graphFormControl1;




	}
}