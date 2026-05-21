using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CABAppControl
{
	public partial class PhasorControl : UserControl
	{

		Pen pen = new Pen(Color.Black, 2);
		Pen penRPhase = new Pen(Color.Red, 3);
		Pen penYPhase = new Pen(Color.Yellow, 3);
		Pen penBPhase = new Pen(Color.Blue, 3);
		
		int yPhaseX1 = 0;
		int yPhaseY1 = 0;
		int bPhaseX1 = 0;
		int bPhaseY1 = 0;

		public PhasorControl()
		{
			InitializeComponent();
		}

		public DataSet PhasorDataSet { get; set; }

		private void PhasorControl_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			graphics.DrawEllipse(pen, 50, 50, 200, 200);
			graphics.DrawLine(pen, 50, 150, 250, 150);
			graphics.DrawLine(pen, 150, 50, 150, 250);
			//Code for Voltage
			if ((Convert.ToString(PhasorDataSet.Tables[0].Rows[0][1]) != null) && (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[0][1])) != 0))
			{
				graphics.DrawLine(penRPhase, 150, 150, 250, 150);
			}

			double angleY = Math.Cos(Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[24][1])));
			if ((Convert.ToString(PhasorDataSet.Tables[0].Rows[1][1]) != null) && (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[1][1])) != 0))
			{
				yPhaseX1 = Convert.ToInt32(GetCircumX(100, 150, angleY));
				yPhaseY1 = Convert.ToInt32(GetCircumY(100, 150, angleY));
				graphics.DrawLine(penYPhase, 150, 150, yPhaseX1, yPhaseY1);
			}

			double angleB = Math.Cos(Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[25][1])));
			if ((Convert.ToString(PhasorDataSet.Tables[0].Rows[2][1]) != null) && (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[2][1])) != 0))
			{
				bPhaseX1 = Convert.ToInt32(GetCircumX(100, 150, angleB));
				bPhaseY1 = Convert.ToInt32(GetCircumY(100, 150, angleB));
				graphics.DrawLine(penBPhase, 150, 150, bPhaseX1, bPhaseY1);
			}

			//Code for Current
			penRPhase.DashStyle = DashStyle.Dash;
			if (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[3][1])) != 0)
			{
				double angleR = GetAngle(Convert.ToString(PhasorDataSet.Tables[0].Rows[19][1]), Convert.ToString(PhasorDataSet.Tables[0].Rows[12][1]), 6);
				yPhaseX1 = Convert.ToInt32(GetCircumX(100, 150, angleR));
				yPhaseY1 = Convert.ToInt32(GetCircumY(100, 150, angleR));
				graphics.DrawLine(penRPhase, 150, 150, yPhaseX1, yPhaseY1);
			}


			penYPhase.DashStyle = DashStyle.Dash;
			if (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[4][1])) != 0)
			{
				angleY = GetAngle(Convert.ToString(PhasorDataSet.Tables[0].Rows[20][1]), Convert.ToString(PhasorDataSet.Tables[0].Rows[13][1]), 7);
				yPhaseX1 = Convert.ToInt32(GetCircumX(100, 150, angleY));
				yPhaseY1 = Convert.ToInt32(GetCircumY(100, 150, angleY));
				graphics.DrawLine(penYPhase, 150, 150, yPhaseX1, yPhaseY1);
			}

			penBPhase.DashStyle = DashStyle.Dash;
			if (Convert.ToInt16(Convert.ToString(PhasorDataSet.Tables[0].Rows[5][1])) != 0)
			{
				angleB = GetAngle(Convert.ToString(PhasorDataSet.Tables[0].Rows[21][1]), Convert.ToString(PhasorDataSet.Tables[0].Rows[14][1]), 8);
				bPhaseX1 = Convert.ToInt32(GetCircumX(100, 150, angleB));
				bPhaseY1 = Convert.ToInt32(GetCircumY(100, 150, angleB));
				graphics.DrawLine(penBPhase, 150, 150, bPhaseX1, bPhaseY1);
			}
		}

		private double GetCircumX(double radius, int centre, double angle)
		{
			return (centre + (radius * Math.Cos(angle)));
		}

		private double GetCircumY(double radius, int centre, double angle)
		{
			return (centre + (radius * Math.Sin(angle)));
		}

		private void PhasorControl_Load(object sender, EventArgs e)
		{
			pictureBox1.Dock = DockStyle.Fill;
			pictureBox1.BackColor = Color.White;
			pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PhasorControl_Paint);
			this.Controls.Add(pictureBox1);
		}

		private double GetAngle(string lagLeadValue, string importExport, int rowNumber)
		{
			if ((lagLeadValue == "Lag") && (importExport == "Import"))
			{
				if (Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1])) != 0)
				{
					return Math.Cos(1 / Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1])));
				}
			}
			else if ((lagLeadValue == "Lead") && (importExport == "Import"))
			{
				if (Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1])) != 0)
				{
					return (360 - Math.Cos(1 / Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1]))));
				}
			}
			else if ((lagLeadValue == "Lag") && (importExport == "Export"))
			{
				if (Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1])) != 0)
				{
					return (360 - Math.Cos(1 / Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1]))));
				}
			}
			else if ((lagLeadValue == "Lead") && (importExport == "Export"))
			{
				if (Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1])) != 0)
				{
					return Math.Cos(1 / Convert.ToDouble(Convert.ToString(PhasorDataSet.Tables[0].Rows[rowNumber][1]))));
				}
			}
		}

		/*
		 * R Phase Voltage	0.00
Y Phase Voltage	0.00
B Phase Voltage	254.27
R Phase Current	1.920
Y Phase Current	17.520
B Phase Current	209.040
R Power Factor	0.00
Y Power Factor	0.00
B Power Factor	1.00
Total Power Factor	1.00
Frequency	51.06
Phase Sequence	0
R Import Export	Import
Y Import Export	Import
B Import Export	Export
Total Import Export	Export
R Phase Channel	Absent
Y Phase Channel	Absent
B Phase Channel	Present
R Lag Lead	Lag
Y Lag Lead	Lag
B Lag Lead	Lag
Total Lag Lead	Lag
Y Phase Angle	0
B Phase Angle	0
Phase Angle Difference	0
		 */
	}
}
