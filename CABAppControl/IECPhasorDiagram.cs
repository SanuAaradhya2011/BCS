using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CAB.Entity;

namespace CAB.UI.Controls
{
    public partial class IECPhasorDiagram : UserControl
    {
        private DataSet _phasorDataset;
        private PhasorEntity _phasorData;
        public IECPhasorDiagram()
        {
            InitializeComponent();
        }

        public DataSet PhasorDataset
        {
            get
            {
                return _phasorDataset;
            }
            set
            {
                _phasorDataset = value;
                pictureBox1.Refresh();
            }
        }

        public PhasorEntity PhasorData
        {
            get { return _phasorData; }
            set { _phasorData = value; }
        }

        private void PhasorControl_Paint(object sender, PaintEventArgs e)
        {
            AdjustableArrowCap arrowCap = new AdjustableArrowCap(5, 5, false);
            int yPhaseX1, bPhaseX1, yPhaseY1, bPhaseY1;
            float angleY, angleB;
            angleY = angleB = 0;
            Graphics graphics = e.Graphics;
            Font fontTitle = new Font("Arial", 7);

           
            Pen pen = new Pen(Color.Black, 2);
            graphics.DrawEllipse(pen, 0, 0, 300, 300);
            //graphics.DrawRectangle(pen,150, 0, 300, 300);
            //graphics.DrawArc(pen, new Rectangle(150, 0, 300, 150), -10, -330);
            graphics.DrawLine(pen, 150, 0, 150, 300);
            graphics.DrawLine(pen, 0, 150, 300, 150);
            //if (_phasorDataset == null)
            //    return;
            //if (_phasorDataset.Tables.Count == 0)
            //    return;
            //if (_phasorDataset.Tables[0].Rows.Count == 0)
            //    return;
            if (_phasorData == null) return;
            //Voltage R
            Pen penVoltageRPhase = new Pen(Color.Red, 2);
            penVoltageRPhase.CustomEndCap = arrowCap;

            long RVoltageValue = ((long)Convert.ToDecimal(_phasorData.RPhaseVoltage));
            long YVoltageValue = ((long)Convert.ToDecimal(_phasorData.YPhaseVoltage));
            long BVoltageValue = ((long)Convert.ToDecimal(_phasorData.BPhaseVoltage));
            if ((RVoltageValue != 0 && YVoltageValue != 0 && BVoltageValue == 0) || (RVoltageValue != 0 && YVoltageValue == 0 && BVoltageValue == 0))//If B Voltage==0 OR ONLY RPHASE PRESENT
            {
                float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBWAnyPhasePresent));
                if (RVoltageValue > 300)
                    RVoltageValue = 300;
                RVoltageValue = RVoltageValue / 2;
                RVoltageValue = RVoltageValue + 150;
                //if (RVoltageValue > 0)
                //    graphics.DrawLine(penVoltageRPhase, 150, 150, RVoltageValue, 150);

                if (RVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageRPhase, 150, 150, RVoltageValue, 150);
                    graphics.DrawString("V(R)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(RVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                }

                Pen penVoltageYPhase = new Pen(Color.Yellow, 2);
                penVoltageYPhase.CustomEndCap = arrowCap;
                angleY = 360 - commonAngle;

                if (YVoltageValue > 300)
                    YVoltageValue = 300;
                YVoltageValue = YVoltageValue / 2;
                yPhaseX1 = Convert.ToInt32(XCordinateValue(YVoltageValue, angleY));
                yPhaseY1 = Convert.ToInt32(YCordinateValue(YVoltageValue, angleY));
                if (yPhaseY1 > 300)
                    yPhaseY1 = yPhaseY1 / 2;
                if (YVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageYPhase, 150, 150, yPhaseX1, yPhaseY1);
                    graphics.DrawString("V(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(yPhaseX1, ItemType.Voltage), getTextCoOrdinate(yPhaseX1, ItemType.Voltage));
                }
            }
            
            if ((RVoltageValue != 0 && YVoltageValue == 0 && BVoltageValue != 0) || (RVoltageValue == 0 && YVoltageValue == 0 && BVoltageValue != 0))//If Y Voltage==0
            {
                //float commonAngle = ((long)Convert.ToDecimal(GetValue("Angle B/W Any 2 Phase Present")));
                float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBWAnyPhasePresent));
                if (RVoltageValue > 300)
                    RVoltageValue = 300;
                RVoltageValue = RVoltageValue / 2;
                RVoltageValue = RVoltageValue + 150;
                if (RVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageRPhase, 150, 150, RVoltageValue, 150);
                    graphics.DrawString("V(R)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(RVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                }

                //Voltage B
                Pen penVoltageBPhase = new Pen(Color.Blue, 2);
                penVoltageBPhase.CustomEndCap = arrowCap;
                angleB = 360 - commonAngle;

                if (BVoltageValue > 300)
                    BVoltageValue = 300;
                BVoltageValue = BVoltageValue / 2;
                bPhaseX1 = Convert.ToInt32(XCordinateValue(BVoltageValue, angleB));
                bPhaseY1 = Convert.ToInt32(YCordinateValue(BVoltageValue, angleB));
                if (bPhaseY1 > 300)
                    bPhaseY1 = bPhaseY1 / 2;
                if (BVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageBPhase, 150, 150, bPhaseX1, bPhaseY1);
                    graphics.DrawString("V(B)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(BVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                }
            }
            if ((RVoltageValue == 0 && YVoltageValue != 0 && BVoltageValue != 0) || (RVoltageValue == 0 && YVoltageValue != 0 && BVoltageValue == 0))//If R Voltage==0
            {
                float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBWAnyPhasePresent));
                Pen penVoltageYPhase = new Pen(Color.Yellow, 2);
                penVoltageYPhase.CustomEndCap = arrowCap;
                if (YVoltageValue > 300)
                    YVoltageValue = 300;
                YVoltageValue = YVoltageValue / 2;
                YVoltageValue = YVoltageValue + 150;
                if (YVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageYPhase, 150, 150, YVoltageValue, 150);
                    graphics.DrawString("V(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(YVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                }

                //Voltage B
                Pen penVoltageBPhase = new Pen(Color.Blue, 2);
                penVoltageBPhase.CustomEndCap = arrowCap;
                angleB = 360 - commonAngle;

                if (BVoltageValue > 300)
                    BVoltageValue = 300;
                BVoltageValue = BVoltageValue / 2;
                bPhaseX1 = Convert.ToInt32(XCordinateValue(BVoltageValue, angleB));
                bPhaseY1 = Convert.ToInt32(YCordinateValue(BVoltageValue, angleB));
                if (bPhaseY1 > 300)
                    bPhaseY1 = bPhaseY1 / 2;
                if (BVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageBPhase, 150, 150, bPhaseX1, bPhaseY1);
                    graphics.DrawString("V(B)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Voltage), getTextCoOrdinate(bPhaseY1, ItemType.Voltage));
                }
            }
            if (RVoltageValue != 0 && YVoltageValue != 0 && BVoltageValue != 0)//If all valtage available
            {
                if (RVoltageValue > 300)
                    RVoltageValue = 300;
                RVoltageValue = RVoltageValue / 2;
                RVoltageValue = RVoltageValue + 150;
                if (RVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageRPhase, 150, 150, RVoltageValue, 150);
                    graphics.DrawString("V(R)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(RVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                }

                //Voltage Y
                Pen penVoltageYPhase = new Pen(Color.Yellow, 2);
                penVoltageYPhase.CustomEndCap = arrowCap;
                //angleY = 360 - ((long)Convert.ToDecimal(GetValue("Y Phase Angle With R Phase")));
                angleY = 360 - ((long)Convert.ToDecimal(_phasorData.YPhaseAngleWithRPhase));

                if (YVoltageValue > 300)
                    YVoltageValue = 300;
                YVoltageValue = YVoltageValue / 2;
                yPhaseX1 = Convert.ToInt32(XCordinateValue(YVoltageValue, angleY));
                yPhaseY1 = Convert.ToInt32(YCordinateValue(YVoltageValue, angleY));
                if (yPhaseY1 > 300)
                    yPhaseY1 = yPhaseY1 / 2;
                if (YVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageYPhase, 150, 150, yPhaseX1, yPhaseY1);
                    graphics.DrawString("V(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(yPhaseX1, ItemType.Voltage), getTextCoOrdinate(yPhaseY1, ItemType.Voltage));
                }

                //Voltage B
                Pen penVoltageBPhase = new Pen(Color.Blue, 2);
                 penVoltageBPhase.CustomEndCap = arrowCap;
                //angleB = 360 - ((long)Convert.ToDecimal(GetValue("B Phase Angle With R Phase")));
                angleB = 360 - ((long)Convert.ToDecimal(_phasorData.BPhaseAngleWithRPhase));

                if (BVoltageValue > 300)
                    BVoltageValue = 300;
                BVoltageValue = BVoltageValue / 2;
                bPhaseX1 = Convert.ToInt32(XCordinateValue(BVoltageValue, angleB));
                bPhaseY1 = Convert.ToInt32(YCordinateValue(BVoltageValue, angleB));
                if (bPhaseY1 > 300)
                    bPhaseY1 = bPhaseY1 / 2;
                if (BVoltageValue > 0)
                {
                    graphics.DrawLine(penVoltageBPhase, 150, 150, bPhaseX1, bPhaseY1);
                    graphics.DrawString("V(B)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Voltage), getTextCoOrdinate(bPhaseY1, ItemType.Voltage));
                }

            }
            //Code for Current R Phase
            double currentRAngle;
            Pen penCurrentRPhase = new Pen(Color.Red, 2);
            penCurrentRPhase.CustomEndCap = arrowCap;
            penCurrentRPhase.DashStyle = DashStyle.Dash;
            //string rImportExportValue = Convert.ToString(GetValue("R Phase kW Direction"));//RImportExport
            string rImportExportValue = Convert.ToString(_phasorData.RPhasekWDirection);//RImportExport
            //string rLagLead = Convert.ToString(GetValue("R Phase Lag/Lead"));//RLagLead
            string rLagLead = Convert.ToString(_phasorData.RPhaseLagLead);//RLagLead
            //double rPhasePowerFactor = Convert.ToDouble(GetValue("R Phase PF"));//RPhasePowerFactor
            double rPhasePowerFactor = Convert.ToDouble(_phasorData.RPhasePF);//RPhasePowerFactor
            //double rCurrentValue = ((long)Convert.ToDecimal(GetValue("R Phase Current"))) * 10;//RPhaseCurrent
            double rCurrentValue = ((long)Convert.ToDecimal(_phasorData.RPhaseCurrent)) * 10;//RPhaseCurrent
            if (rCurrentValue > 100)
                rCurrentValue = 100;

           // MessageBox.Show("RPhase KW: " + _phasorData.RPhasekWDirection);
           // MessageBox.Show("R Phase Lag/Lead:" + _phasorData.RPhaseLagLead);
           // MessageBox.Show("rphase Power Factor" + _phasorData.RPhasePF);
            if (rImportExportValue.Equals("Import"))
            {
                if (rLagLead.Equals("Lag"))
                {
                    currentRAngle = Math.Acos(rPhasePowerFactor) * 57.2957795;
                    bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                }
                else
                {
                    currentRAngle = 360 - (Math.Acos(rPhasePowerFactor) * 57.2957795);
                    bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                }
            }
            else
            {
                if (rLagLead.Equals("Lag"))
                {
                    currentRAngle = Math.Acos((rPhasePowerFactor * -1)) * 57.2957795;
                    bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                }
                else
                {
                    currentRAngle = 360 - (Math.Acos((rPhasePowerFactor * -1)) * 57.2957795);
                    bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                }
            }
            if (bPhaseY1 > 300)
                bPhaseY1 = bPhaseY1 / 2;
            if (RVoltageValue > 0)
            {
                graphics.DrawLine(penCurrentRPhase, 150, 150, bPhaseX1, bPhaseY1);
                graphics.DrawString("I(R)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Current), getTextCoOrdinate(bPhaseY1, ItemType.Current));
            }


            //Code for Current Y Phase
            double currentYAngle;
            Pen penCurrentYPhase = new Pen(Color.Yellow, 2);
            penCurrentYPhase.CustomEndCap = arrowCap;
            penCurrentYPhase.DashStyle = DashStyle.Dash;
            //string yImportExportValue = Convert.ToString(GetValue("Y Phase kW Direction"));//YImportExport
            string yImportExportValue = Convert.ToString(_phasorData.YPhasekWDirection);//YImportExport
//            string yLagLead = Convert.ToString(GetValue("Y Phase Lag/Lead"));//YLagLead
            string yLagLead = Convert.ToString(_phasorData.YPhaseLagLead);//YLagLead
            //double yPhasePowerFactor = Convert.ToDouble(GetValue("Y Phase PF"));//YPhasePowerFactor
            double yPhasePowerFactor = Convert.ToDouble(_phasorData.YPhasePF);//YPhasePowerFactor
            //double yCurrentValue = ((long)Convert.ToDecimal(GetValue("Y Phase Current"))) * 10;//YPhaseCurrent
            double yCurrentValue = ((long)Convert.ToDecimal(_phasorData.YPhaseCurrent)) * 10;//YPhaseCurrent
            if (yCurrentValue > 100)
                yCurrentValue = 100;
            if (yImportExportValue.Equals("Import"))
            {
                if (yLagLead.Equals("Lag"))
                {
                    currentYAngle = Math.Acos(yPhasePowerFactor) * 57.2957795 + (360 - angleY);
                    bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                }
                else
                {
                    currentYAngle = 360 - (Math.Acos(yPhasePowerFactor) * 57.2957795) + (360 - angleY);
                    bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                }
            }
            else
            {
                if (yLagLead.Equals("Lag"))
                {
                    currentYAngle = (Math.Acos((yPhasePowerFactor * -1)) * 57.2957795) + (360 - angleY);
                    bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                }
                else
                {
                    currentYAngle = (360 - (Math.Acos((yPhasePowerFactor * -1))) * 57.2957795) + (360 - angleY);
                    bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                }
            }
            if (bPhaseY1 > 300)
                bPhaseY1 = bPhaseY1 / 2;
            if (YVoltageValue > 0)
            {
                graphics.DrawLine(penCurrentYPhase, 150, 150, bPhaseX1, bPhaseY1);
                graphics.DrawString("I(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Current), getTextCoOrdinate(bPhaseY1, ItemType.Current));
            }


            //Code for Current B Phase
            double currentBAngle;
            Pen penCurrentBPhase = new Pen(Color.Blue, 2);
            penCurrentBPhase.DashStyle = DashStyle.Dash;
            penCurrentBPhase.CustomEndCap = arrowCap;


            //string bImportExportValue = Convert.ToString(GetValue("B Phase kW Direction"));//BImportExport
            string bImportExportValue = Convert.ToString(_phasorData.BPhasekWDirection);//BImportExport
            //string bLagLead = Convert.ToString(GetValue("B Phase Lag/Lead"));//BLagLead
            string bLagLead = Convert.ToString(_phasorData.BPhaseLagLead);//BLagLead
            //double bPhasePowerFactor = Convert.ToDouble(GetValue("B Phase PF"));//BPhasePowerFactor
            double bPhasePowerFactor = Convert.ToDouble(_phasorData.BPhasePF);//BPhasePowerFactor
            //double bCurrentValue = ((long)Convert.ToDecimal(GetValue("B Phase Current"))) * 10;//BPhaseCurrent 
            double bCurrentValue = ((long)Convert.ToDecimal(_phasorData.BPhaseCurrent)) * 10;//BPhaseCurrent 
            if (bCurrentValue > 100)
                bCurrentValue = 100;
            if (bImportExportValue.Equals("Import"))
            {
                if (bLagLead.Equals("Lag"))
                {
                    currentBAngle = Math.Acos(bPhasePowerFactor) * 57.2957795 + (360 - angleB);
                    bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                }
                else
                {
                    currentBAngle = 360 - (Math.Acos(bPhasePowerFactor) * 57.2957795) + (360 - angleB);
                    bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                }
            }
            else
            {
                if (bLagLead.Equals("Lag"))
                {
                    double val = bPhasePowerFactor * -1;
                    val = Math.Acos(val);
                    currentBAngle = (val * 57.2957795) + (360 - angleB);
                    bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                }
                else
                {
                    currentBAngle = 360 - (Math.Acos((bPhasePowerFactor * -1))) * 57.2957795 + (360 - angleB);
                    bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                    bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                }
            }
            if (bPhaseY1 > 300)
                bPhaseY1 = bPhaseY1 / 2;
            if (BVoltageValue > 0)
            {
                graphics.DrawLine(penCurrentBPhase, 150, 150, bPhaseX1, bPhaseY1);
                graphics.DrawString("I(B)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Current), getTextCoOrdinate(bPhaseY1, ItemType.Current));
            }

        }

        private enum ItemType
        {
            Current,
            Voltage
        }
       

        private int getTextCoOrdinate(int axisValue, ItemType item)
        {
            int coordinate;
            int returnedAxisValue = axisValue;
            if (item == ItemType.Current)
            {
                coordinate = 1;
            }
            else
            {
                coordinate = 2;
            }
            if (axisValue >= 150)
            {
                returnedAxisValue = axisValue + coordinate;
            }
            else
            {
                returnedAxisValue = axisValue - coordinate;
            }
            return returnedAxisValue;
        }
        //private object GetValue(string columnName)
        //{
        //    foreach (DataRow dr in _phasorDataset.Tables[0].Rows)
        //    {
        //        if (Convert.ToString(dr[0]).Equals(columnName))
        //            return dr[1];
        //    }
        //    return null;
        //}
        private double XCordinateValue(float radius, float angle)
        {
            return (radius * Math.Cos(angle / 57.2957795)) + 150;
        }

        private double YCordinateValue(float radius, float angle)
        {
            return (radius * Math.Sin(angle / 57.2957795)) + 150;
        }

        private void PhasorControl_Load(object sender, EventArgs e)
        {
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackColor = Color.White;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PhasorControl_Paint);
            this.Controls.Add(pictureBox1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

    }
}
