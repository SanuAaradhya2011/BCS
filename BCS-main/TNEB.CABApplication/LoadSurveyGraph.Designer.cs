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
			this.graphFormControl1.FileName = "15 May 2010 Load Survey.CAB";
			this.graphFormControl1.Location = new System.Drawing.Point(0, 1);
			this.graphFormControl1.MeterID = "00000022";
			this.graphFormControl1.Name = "graphFormControl1";
			this.graphFormControl1.Size = new System.Drawing.Size(1285, 731);
			this.graphFormControl1.TabIndex = 2;
			// 
			// FrmLoadSurvey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1283, 744);
			this.Controls.Add(this.graphFormControl1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmLoadSurvey";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Load Survey Graph";
			//this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLoadSurvey_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private CAB.UI.Graphs.GraphFormControl graphFormControl1;

	}
}