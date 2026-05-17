namespace CAB.UI.Graphs
{
	partial class EnergyGraph
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.graphChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.btnColor = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.graphChart)).BeginInit();
			this.SuspendLayout();
			// 
			// graphChart
			// 
			this.graphChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
			this.graphChart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
			this.graphChart.BackSecondaryColor = System.Drawing.Color.White;
			this.graphChart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
			this.graphChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			this.graphChart.BorderlineWidth = 2;
			chartArea1.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic;
			chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea1.AxisX.ScaleBreakStyle.BreakLineStyle = System.Windows.Forms.DataVisualization.Charting.BreakLineStyle.None;
			chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
			chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(165)))), ((int)(((byte)(191)))), ((int)(((byte)(228)))));
			chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
			chartArea1.BackSecondaryColor = System.Drawing.Color.White;
			chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			chartArea1.CursorX.IsUserEnabled = true;
			chartArea1.CursorX.IsUserSelectionEnabled = true;
			chartArea1.CursorY.IsUserEnabled = true;
			chartArea1.CursorY.IsUserSelectionEnabled = true;
			chartArea1.Name = "ChartArea1";
			chartArea1.ShadowColor = System.Drawing.Color.Transparent;
			this.graphChart.ChartAreas.Add(chartArea1);
			this.graphChart.Dock = System.Windows.Forms.DockStyle.Fill;
			legend1.Alignment = System.Drawing.StringAlignment.Center;
			legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
			legend1.InterlacedRowsColor = System.Drawing.Color.White;
			legend1.Name = "Legend1";
			this.graphChart.Legends.Add(legend1);
			this.graphChart.Location = new System.Drawing.Point(0, 0);
			this.graphChart.Name = "graphChart";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.MarkerColor = System.Drawing.Color.White;
			series1.Name = "Series1";
			this.graphChart.Series.Add(series1);
			this.graphChart.Size = new System.Drawing.Size(1072, 619);
			this.graphChart.TabIndex = 0;
			this.graphChart.Text = "chart1";
			title1.Name = "ChartTitle";
			this.graphChart.Titles.Add(title1);
			this.graphChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.graphChart_MouseUp);
			this.graphChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graphChart_MouseMove);
			this.graphChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.graphChart_MouseDown);
			// 
			// btnColor
			// 
			this.btnColor.Location = new System.Drawing.Point(982, 18);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(75, 23);
			this.btnColor.TabIndex = 1;
			this.btnColor.Text = "Color";
			this.btnColor.UseVisualStyleBackColor = true;
			this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
			// 
			// EnergyGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.btnColor);
			this.Controls.Add(this.graphChart);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "EnergyGraph";
			this.Size = new System.Drawing.Size(1072, 619); 
			((System.ComponentModel.ISupportInitialize)(this.graphChart)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataVisualization.Charting.Chart graphChart;
		private System.Windows.Forms.Button btnColor;

	}
}
