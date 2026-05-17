using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using CAB.Entity;
using CAB.BLL;
using Hunt.EPIC.Logging;
using CAB.Framework.Utility;
using CAB.Framework.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;

using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Threading;


namespace CAB.UI
{
 
    public partial class PhasorDiagramForm : CABForm
    {
        public PhasorDiagramForm()
        {
            InitializeComponent();
            
        }
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(PhasorDiagramForm).ToString());
        private System.IO.Stream streamToPrint;
        string streamType;
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt
        (
            IntPtr hdcDest, // handle to destination DC
            int nXDest, // x-coord of destination upper-left corner
            int nYDest, // y-coord of destination upper-left corner
            int nWidth, // width of destination rectangle
            int nHeight, // height of destination rectangle
            IntPtr hdcSrc, // handle to source DC
            int nXSrc, // x-coordinate of source upper-left corner
            int nYSrc, // y-coordinate of source upper-left corner
            System.Int32 dwRop // raster operation code
        );

        private string meterDataID;
        private bool _showGrid = false;
        public string MeterDataID
        {
            get
            {
                return meterDataID;
            }
            set
            {
                meterDataID = value;
            }
        }
        private bool phasorDataAvailable=false;
        public bool PhasorDataAvailable
        {
            get
            {
                return phasorDataAvailable;
            }
            set
            {
                phasorDataAvailable = value;
            }
        }

        private PhasorEntity getPhaserEntity()
        {

            PhasorEntity phasor = null;
            try
            {
                phasor = new DLMS650PhasorBLL().GetPhasorDataEntity(Convert.ToInt32(meterDataID)) as PhasorEntity;
                if (phasor != null)
                    phasorDataAvailable = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "getPhaserEntity()", ex);
            }
            return phasor;
        }
        public bool ShowPhasorDataInGrid
        {
            get { return _showGrid; }
            set { _showGrid = value; }
        }

        private void PhasorDiagramForm_Load(object sender, EventArgs e)
        {     
            PhasorEntity phEntity = getPhaserEntity();
            if (phEntity == null)
            {
                return;
            }

            lngPhasorDiagram.PhasorData = phEntity;
            if (!ShowPhasorDataInGrid)
            {
                lngPhasorData.Visible = false;
            }

            if (string.IsNullOrEmpty(phEntity.PhaseSequence) || phEntity.PhaseSequence.ToUpper() == "INCORRECT")
            {
                lngPhasorDiagram.Visible = false;
                lblPhasorNotShown.Visible = true;
                lblPhasorNotShown.Text = "Phase Sequence is not correct. Phasor can not be shown";
            }
            else
            {
                lngPhasorDiagram.Visible = true;
                lblPhasorNotShown.Visible = false;
            }

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
             System.Drawing.Image image = System.Drawing.Image.FromStream(this.streamToPrint);
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            int width = image.Width;
            int height = image.Height;
            if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
            {
                width = e.MarginBounds.Width;
                height = image.Height * e.MarginBounds.Width / image.Width;
            }
            else
            {
                height = e.MarginBounds.Height;
                width = image.Width * e.MarginBounds.Height / image.Height;
            }
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
            e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        public void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Graphics g1 = this.CreateGraphics();
                Image MyImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, g1);
                Graphics g2 = Graphics.FromImage(MyImage);
                IntPtr dc1 = g1.GetHdc();
                IntPtr dc2 = g2.GetHdc();
                BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
                g1.ReleaseHdc(dc1);
                g2.ReleaseHdc(dc2);
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
                filePath=filePath + "PrintPage.jpg";

                if (System.IO.File.Exists(filePath))//@"d:\PrintPage.jpg"))
                {
                    System.IO.File.Delete(filePath);//@"d:\PrintPage.jpg");
                }
                MyImage.Save(filePath,ImageFormat.Jpeg);//(@"d:\PrintPage.jpg", ImageFormat.Jpeg);
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);//(@"d:\PrintPage.jpg", FileMode.Open, FileAccess.Read);
             //   StartPrint(fileStream, "Image");
                fileStream.Close();
                //if (System.IO.File.Exists(@"d:\PrintPage.jpg"))
                //{
                //    System.IO.File.Delete(@"d:\PrintPage.jpg");
                //}
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnPrint_Click(object sender, EventArgs e)", ex);
            }
        }
        public void StartPrint(Stream streamToPrint, string streamType)
        {
            this.printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            this.streamToPrint = streamToPrint;
            this.streamType = streamType;
            System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();
            PrintDialog1.AllowSomePages = true;
            PrintDialog1.ShowHelp = true;
            PrintDialog1.Document = printDocument1;
           
            DialogResult result = PrintDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument1.DefaultPageSettings.Landscape = true;
                printDocument1.Print();

            }
        }

        private void PhasorDiagramForm_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnPrint_Click(sender, e);
            timer1.Stop();
            this.Close();
        }

        private void lngPhasorDiagram_Load(object sender, EventArgs e)
        {

        }




    }
}
