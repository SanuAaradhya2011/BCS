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
    public partial class PhasorDiagram : UserControl
    {
        private DataSet _phasorDataset;
        private PhasorEntity _phasorData;

        public PhasorDiagram()
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
            Font fontTitle = new Font("Arial", 7);
            const int CURRENTBASEVALUE = 150;
            double dbMaxCurrent = 0;
            double dbMaxVoltage = 0;


            try
            {
                int yPhaseX1, bPhaseX1, yPhaseY1, bPhaseY1;
                float angleY, angleB;
                angleY = angleB = 0;
                Graphics graphics = e.Graphics;

                Pen pen = new Pen(Color.Black, 2);
                graphics.DrawEllipse(pen, 0, 0, 300, 300);
                graphics.DrawLine(pen, 150, 0, 150, 300);
                graphics.DrawLine(pen, 0, 150, 300, 150);

                if (_phasorData == null) return;
                // Code added to remove "-" sign from current value
                String rPhaseCurrent = _phasorData.RPhaseCurrent;
                String yPhaseCurrent = _phasorData.YPhaseCurrent;
                String bPhaseCurrent = _phasorData.BPhaseCurrent;
                try
                {

                    if (rPhaseCurrent.Contains("-"))
                        rPhaseCurrent = rPhaseCurrent.Remove(0, 1);
                    if (yPhaseCurrent.Contains("-"))
                        yPhaseCurrent = yPhaseCurrent.Remove(0, 1);
                    if (bPhaseCurrent.Contains("-"))
                        bPhaseCurrent = bPhaseCurrent.Remove(0, 1);
                }
                catch
                {
                }
                // Code added to remove "-" sign from current value

                //Voltage R
                Pen penVoltageRPhase = new Pen(Color.Red, 2);
                penVoltageRPhase.CustomEndCap = arrowCap;


                long RVoltageValue = ((long)Convert.ToDecimal(_phasorData.RPhaseVoltage));
                long YVoltageValue = ((long)Convert.ToDecimal(_phasorData.YPhaseVoltage));
                long BVoltageValue = ((long)Convert.ToDecimal(_phasorData.BPhaseVoltage));
                // Calulating Scaling for Voltage as per requirement in BRPL
                dbMaxVoltage = Math.Max(RVoltageValue, YVoltageValue);
                dbMaxVoltage = Math.Max(dbMaxVoltage, BVoltageValue);
                if (dbMaxVoltage != 0)
                {
                    RVoltageValue = (RVoltageValue * 300) / (long)dbMaxVoltage;
                    YVoltageValue = (YVoltageValue * 300) / (long)dbMaxVoltage;
                    BVoltageValue = (BVoltageValue * 300) / (long)dbMaxVoltage;
                }


                if ((RVoltageValue != 0 && YVoltageValue != 0 && BVoltageValue == 0) || (RVoltageValue != 0 && YVoltageValue == 0 && BVoltageValue == 0))//If B Voltage==0
                {
                    //float commonAngle = ((long)Convert.ToDecimal(GetValue("Angle B/W Any 2 Phase Present")));
                    float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBetweenTwo));

                  

                    if (RVoltageValue > 300)
                        RVoltageValue = 300;
                    
                    RVoltageValue = RVoltageValue / 2;
                    RVoltageValue = RVoltageValue + 150;
                    if (RVoltageValue > 0)
                    {
                        graphics.DrawLine(penVoltageRPhase, 150, 150, RVoltageValue, 150);
                        graphics.DrawString("V(R)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(RVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                    }

                    Pen penVoltageYPhase = new Pen(Color.Yellow, 2);
                    penVoltageYPhase.CustomEndCap = arrowCap;
                    angleY = 360 - commonAngle;
                    // Try drawing y voltage only if it is greater than zero, needed for single phase draw
                    if (YVoltageValue > 0)
                    {
                        if (YVoltageValue > 300)
                            YVoltageValue = 300;
                        
                        YVoltageValue = YVoltageValue / 2;
                        yPhaseX1 = Convert.ToInt32(XCordinateValue(YVoltageValue, angleY));
                        yPhaseY1 = Convert.ToInt32(YCordinateValue(YVoltageValue, angleY));
                        if (yPhaseY1 > 300)
                        {
                            yPhaseY1 = yPhaseY1 / 2;
                        }
                        graphics.DrawLine(penVoltageYPhase, 150, 150, yPhaseX1, yPhaseY1);
                        graphics.DrawString("V(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(yPhaseX1, ItemType.Voltage), getTextCoOrdinate(yPhaseX1, ItemType.Voltage));
                    }
                }
                // if R and B phase present OR only B phase present 
                if ((RVoltageValue != 0 && YVoltageValue == 0 && BVoltageValue != 0) || (RVoltageValue == 0 && YVoltageValue == 0 && BVoltageValue != 0))//If Y Voltage==0
                {
                    // float commonAngle = ((long)Convert.ToDecimal(GetValue("Angle B/W Any 2 Phase Present")));
                    float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBetweenTwo));
                    if (RVoltageValue > 0)
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
                    }
                    //Voltage B
                    Pen penVoltageBPhase = new Pen(Color.Blue, 2);
                    penVoltageBPhase.CustomEndCap = arrowCap;
                    angleB = 360 - commonAngle;
                    if (BVoltageValue > 0)
                    {
                        if (BVoltageValue > 300)
                            BVoltageValue = 300;

                        BVoltageValue = BVoltageValue / 2;
                        bPhaseX1 = Convert.ToInt32(XCordinateValue(BVoltageValue, angleB));
                        bPhaseY1 = Convert.ToInt32(YCordinateValue(BVoltageValue, angleB));
                        
                        if (bPhaseY1 > 300)
                            bPhaseY1 = bPhaseY1 / 2;

                        if (RVoltageValue > 0)
                        {
                            graphics.DrawLine(penVoltageBPhase, 150, 150, bPhaseX1, bPhaseY1);
                            graphics.DrawString("V(B)", fontTitle, Brushes.Black, getTextCoOrdinate(bPhaseX1, ItemType.Voltage), getTextCoOrdinate(bPhaseY1, ItemType.Voltage));
                        }
                        else
                        {
                            BVoltageValue = BVoltageValue + 150;
                            graphics.DrawLine(penVoltageBPhase, 150, 150, BVoltageValue, 150);
                            graphics.DrawString("V(B)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(BVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                        }

                    }
                }
                //if Y and B voltage present OR only Y voltage is present
                if ((RVoltageValue == 0 && YVoltageValue != 0 && BVoltageValue != 0) || (RVoltageValue == 0 && YVoltageValue != 0 && BVoltageValue == 0))//If R Voltage==0
                {
                    //float commonAngle = ((long)Convert.ToDecimal(GetValue("Angle B/W Any 2 Phase Present")));
                    float commonAngle = ((long)Convert.ToDecimal(_phasorData.AngleBetweenTwo));
                    Pen penVoltageYPhase = new Pen(Color.Yellow, 2);
                    penVoltageYPhase.CustomEndCap = arrowCap;
                    if (YVoltageValue > 300)
                        YVoltageValue = 300;

                    YVoltageValue = YVoltageValue / 2;
                    YVoltageValue = YVoltageValue + 150;
                    // write y voltage anyways..
                    if (YVoltageValue > 0)
                    {
                        graphics.DrawLine(penVoltageYPhase, 150, 150, YVoltageValue, 150);
                        graphics.DrawString("V(Y)", fontTitle, Brushes.Black, getTextCoOrdinate(Convert.ToInt32(YVoltageValue), ItemType.Voltage), getTextCoOrdinate(150, ItemType.Voltage));
                    }


                    //Voltage B
                    // Do not write b voltage if only y voltage is present
                    if (BVoltageValue > 0)
                    {
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
                    // angleY = 360 - ((long)Convert.ToDecimal(GetValue("Y Phase Angle With R Phase")));
                    angleY = 360 - ((long)Convert.ToDecimal(_phasorData.AngleYR));

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
                    angleB = 360 - ((long)Convert.ToDecimal(_phasorData.AngleBR));

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

                string rImportExportValue = Convert.ToString(_phasorData.RPhaseNegativePowerFlag);//RImportExport
                string rLagLead = Convert.ToString(_phasorData.RPhaseCapacitiveInductiveFlag);//RLagLead
                double rPhasePowerFactor = Convert.ToDouble(_phasorData.RPhasePowerFactor);//RPhasePowerFactor
                //double rCurrentValue = ((long)Convert.ToDecimal(rPhaseCurrent)) * 10;//RPhaseCurrent
                // Updated Typecasting from long to double
                // Calulating Scaling for Voltage as per requirement in BRPL
                double rCurrentValue = (double)(Convert.ToDecimal(rPhaseCurrent)) ;//RPhaseCurrent
                double yCurrentValue = (double)(Convert.ToDecimal(yPhaseCurrent)) ;//RPhaseCurrent
                double bCurrentValue = (double)(Convert.ToDecimal(bPhaseCurrent)) ;//RPhaseCurrent
                dbMaxCurrent = Math.Max(rCurrentValue, yCurrentValue);
                dbMaxCurrent = Math.Max(dbMaxCurrent, bCurrentValue);

                if (dbMaxCurrent != 0)
                    rCurrentValue = rCurrentValue * CURRENTBASEVALUE / dbMaxCurrent;

                if (rCurrentValue > 100)
                    rCurrentValue = 150;

                if (rImportExportValue.Equals("Import", StringComparison.OrdinalIgnoreCase))
                {
                    if (rLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentRAngle = Math.Acos(rPhasePowerFactor) * 57.2957795;
                        bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                    }
                    else
                    {
                        currentRAngle = 360 - (Math.Acos(rPhasePowerFactor * -1) * 57.2957795);
                        bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                    }
                }
                else
                {
                    //changed for export lag added 180 degree
                    if (rLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentRAngle = 180 + Math.Acos((rPhasePowerFactor)) * 57.2957795;
                        bPhaseX1 = 150 + (int)((rCurrentValue * Math.Cos(currentRAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((rCurrentValue * Math.Sin(currentRAngle / 57.2957795)));
                    }
                    else
                    {
                        currentRAngle = 180 - (Math.Acos((rPhasePowerFactor * -1)) * 57.2957795);
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

                string yImportExportValue = Convert.ToString(_phasorData.YPhaseNegativePowerFlag);//YImportExport
                string yLagLead = Convert.ToString(_phasorData.YPhaseCapacitiveInductiveFlag);//YLagLead
                double yPhasePowerFactor = Convert.ToDouble(_phasorData.YPhasePowerFactor);//YPhasePowerFactor

                if (dbMaxCurrent != 0)
                    yCurrentValue = yCurrentValue * CURRENTBASEVALUE / dbMaxCurrent;

                if (yCurrentValue > 100)
                    yCurrentValue = 150;

                if (yImportExportValue.Equals("Import", StringComparison.OrdinalIgnoreCase))
                {
                    if (yLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentYAngle = Math.Acos(yPhasePowerFactor) * 57.2957795 + (360 - angleY);
                        bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                    }
                    else
                    {
                        currentYAngle = 360 - (Math.Acos(yPhasePowerFactor * -1) * 57.2957795) + (360 - angleY);
                        bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                    }
                }
                else
                {
                    if (yLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentYAngle = 180 + (Math.Acos((yPhasePowerFactor)) * 57.2957795) + (360 - angleY);
                        bPhaseX1 = 150 + (int)((yCurrentValue * Math.Cos(currentYAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((yCurrentValue * Math.Sin(currentYAngle / 57.2957795)));
                    }
                    else
                    {
                        currentYAngle = (180 - (Math.Acos((yPhasePowerFactor * -1))) * 57.2957795) + (360 - angleY);
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
                penCurrentBPhase.CustomEndCap = arrowCap;
                penCurrentBPhase.DashStyle = DashStyle.Dash;


                string bImportExportValue = Convert.ToString(_phasorData.BPhaseNegativePowerFlag);//BImportExport
                string bLagLead = Convert.ToString(_phasorData.BPhaseCapacitiveInductiveFlag);//BLagLead
                double bPhasePowerFactor = Convert.ToDouble(_phasorData.BPhasePowerFactor);//BPhasePowerFactor
                //double bCurrentValue = ((long)Convert.ToDecimal(bPhaseCurrent)) * 10;//BPhaseCurrent 
                // Updated typecasting from long to double
                //  double bCurrentValue = ((double)Convert.ToDecimal(bPhaseCurrent)) * 10;//BPhaseCurrent 

                if (dbMaxCurrent != 0)
                    bCurrentValue = bCurrentValue * CURRENTBASEVALUE / dbMaxCurrent;

                if (bCurrentValue > 100)
                    bCurrentValue = 150;

                if (bImportExportValue.Equals("Import", StringComparison.OrdinalIgnoreCase))
                {
                    if (bLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentBAngle = Math.Acos(bPhasePowerFactor) * 57.2957795 + (360 - angleB);
                        bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                    }
                    else
                    {
                        currentBAngle = 360 - (Math.Acos(bPhasePowerFactor * -1) * 57.2957795) + (360 - angleB);
                        bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                    }
                }
                else
                {
                    if (bLagLead.Equals("Lag", StringComparison.OrdinalIgnoreCase))
                    {
                        currentBAngle = 180 + (Math.Acos(bPhasePowerFactor) * 57.2957795) + (360 - angleB);
                        bPhaseX1 = 150 + (int)((bCurrentValue * Math.Cos(currentBAngle / 57.2957795)));
                        bPhaseY1 = 150 - (int)((bCurrentValue * Math.Sin(currentBAngle / 57.2957795)));
                    }
                    else
                    {
                        currentBAngle = 180 - (Math.Acos((bPhasePowerFactor * -1))) * 57.2957795 + (360 - angleB);
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
            catch (Exception ex)
            {
                throw ex;
            }
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

        private enum ItemType
        {
            Current,
            Voltage
        }


    }
}
