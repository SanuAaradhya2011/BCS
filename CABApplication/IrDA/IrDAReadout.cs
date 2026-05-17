#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.BLL;
using CAB.Framework;
using CABCommunication.Common;
using CAB.Serialization;
using CAB.Parser;
using System.Threading;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using Hunt.EPIC.Logging;
using CABCommunication.DataLinkLayer;
using CABApplication.Reports.Forms;
#endregion

namespace CAB.UI
{
    /// <summary>
    /// This form is used for selecting readout/communication mode form BCS .
    /// </summary>
    public partial class IrDAReadout : MdiChildForm
    {
        public Communication communication;
        private FileReportDataSet reportXSD = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(IrDAReadout).ToString());
       
        public IrDAReadout()
        {
            InitializeComponent();
            reportXSD = new FileReportDataSet();
        }

        private void frmIrDAReadouts_Load(object sender, EventArgs e)
        {
            //TreeViewExplorer.Nodes.Add("Billing Data");
            //MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
        }

        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {
            string mode;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (connected)
            {
                mode = "IrDA";
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
            else
            {
                mode = "IrDA";
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
        }

        private void lblRead_Click(object sender, EventArgs e)
         {
            try
            {
                DisableControls();
                ChannelInformation channelInfo = new ChannelInformation();
                channelInfo.CommunicationMode = "IrDA";
                channelInfo.NoOfRetries = 1;
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                // channelInfo.ModemInfo = ConfigSettings.GetValue("PortName");
                channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communication = new Communication(channelInfo);

                byte[] resultArrayBytes;
                if (!IsValidSelection()) return;
                //txtIrDAData.Text = "";
                Application.DoEvents();
                //objIrDaLI.UpdatedLed += new IrDALayerInterface.UpdateHandler(AddressForm_PingLed);
                //this.Cursor = Cursors.WaitCursor;
                //txtIrDAData.AppendText ( "--> Connecting To Meter ...");
                this.StatusMessageAsync = "Connecting To Meter ...";
                if (!communication.OpenSessionIrDA())
                {
                    this.StatusMessageAsync = "Port Failed.";
                    return;
                }
                int DeviceID = 0xFFFFFF;
                int HHuID = 0xEEEEEE;
                string dataType = "Billing Data";
                switch (dataType) //TreeViewExplorer.SelectedNode.Text
                {
                    case "Billing Data":
                        //------------------------Initiation Command-------------------------------------
                        //txtIrDAData.AppendText ( "\r\n" + "--> Sending Initiation Command ...");
                        this.StatusMessageAsync = "Sending Initiation Command ...";
                       // txtIrDAData.Text += "\r\n";
                        Result result = communication.ReadIrDAByteFromMeter((byte)IrDALayer.IrDACommandType.InitiationCommand, DeviceID, HHuID, "");
                        if (result.ErrorCode!=CommunicationErrorType.Success)
                        {
                            this.StatusMessageAsync = "Communication Failed.";
                            return;
                        }
                       
                        resultArrayBytes = result.RecieveDataBuffer.ToArray();

                        //DisplayData(resultArrayBytes);
                        dataGridView1.Rows.Clear();
                        //display in Grid
                        DisplayGridData(resultArrayBytes, 11, 18, "init");

                        //-----------------Get Device ID from Response-----------------------------------
                        int compValue = 0;
                        compValue = (compValue | (int)resultArrayBytes[7]) << 8;
                        compValue = (compValue | (int)resultArrayBytes[8]) << 8;
                        compValue = (compValue | (int)resultArrayBytes[9]);
                        DeviceID = compValue;
                        //----------------------------Get Billing Profile Data Command--------------------
                       // txtIrDAData.Text += "\r\n";
                        Application.DoEvents();
                        Thread.Sleep(300);
                        //txtIrDAData.AppendText ( "\r\n" + "--> Sending Billing Profile Command ...");
                        this.StatusMessageAsync = "Sending Billing Profile Command ...";
                        //txtIrDAData.Text += "\r\n";
                        Application.DoEvents();
                        //txtIrDAData.Text += IrDALayer.IrDACommandType.BillingDataCommand.ToString();
                        result = communication.ReadIrDAByteFromMeter((byte)IrDALayer.IrDACommandType.BillingDataCommand, DeviceID, HHuID, "");
                        if (result.ErrorCode!=CommunicationErrorType.Success)
                        {
                            this.StatusMessageAsync = "Communication Failed.";
                            return;
                        }

                        resultArrayBytes = result.RecieveDataBuffer.ToArray();

                        //DisplayData(resultArrayBytes);

                        //display in Grid
                        DisplayGridData(resultArrayBytes, 10, 58, "Billing");
                 
                            
                        Application.DoEvents();
                        Thread.Sleep(300);
                        //txtIrDAData.AppendText ( "\r\n" + "--> Sending Closing Command ...");
                        this.StatusMessageAsync = "Sending Closing Command ...";
                        Application.DoEvents();
                        //txtIrDAData.Text += IrDALayer.IrDACommandType.ClosingCommand.ToString();
                       
                        //----------------------------Send Closing Command , No Response will come from Meter------------- 
                        result = communication.ReadIrDAByteFromMeter((byte)IrDALayer.IrDACommandType.ClosingCommand, DeviceID, HHuID, "");
                        if (result.ErrorCode!=CommunicationErrorType.Success)
                        {
                            return;
                        }
                        DisplayStatusMsg("Reading Completed !", false);

                        //txtIrDAData.AppendText ( "\r\n" + "---------Reading Completed !-----------");
                        this.StatusMessageAsync = "Reading Completed.";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                communication.CloseSessionIrDA();
                EnableControls();
                this.Cursor = Cursors.Default;
            }
        }

        private void EnableControls()
        {
            buttonRead.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void DisableControls()
        {
            buttonRead.Enabled = false;
            buttonCancel.Enabled = false;
        }

        private void DisplayStatusMsg(string p, bool p_2)
        {
            //throw new NotImplementedException();
        }

        private void DisplayGridData(byte[] receivedData, int start, int end, string cmd)
        {
          
            string[] paramdata = {"Meter ID", "Manufacturing DATE", "Version Control ID", "Meter Phase", "Meter Health Check", "MD Integration Period",
                                  "RTC-Time", "RTC-Date", "Instantaneous Voltage R", "Instantaneous Voltage Y", "Instantaneous Voltage B",
                                  "Instantaneous Current R", "Instantaneous Current Y", "Instantaneous Current B", "Cum. kWh", "Cum. kVAh", "LBP1kvAMD",
                                  "MD Time", "MD Date", "Make Code", "Phase", "Meter Multiplying factor", "MD Reset Count"};
         
            if (cmd == "init")
            {
                dataGridView1.Rows.Add(new object[] { "1", paramdata[0], IrDAParsor(receivedData, 11, 3) });
                dataGridView1.Rows.Add(new object[] { "2", paramdata[1], monthnme(IrDAParsor(receivedData, 14, 1)) });
                dataGridView1.Rows.Add(new object[] { "3", paramdata[2], IrDAParsor(receivedData, 15, 1) });
                dataGridView1.Rows.Add(new object[] { "4", paramdata[3], SelectedMterType(IrDAParsor(receivedData, 16, 1)) });
                dataGridView1.Rows.Add(new object[] { "5", paramdata[4], IrDAParsor(receivedData, 17, 1) });
                dataGridView1.Rows.Add(new object[] { "6", paramdata[5], IrDAParsor(receivedData, 18, 1) });               
            }
            else if (cmd == "Billing")
            {
                dataGridView1.Rows.Add(new object[] { "7", paramdata[6], receivedData[10].ToString("00") + ":" + receivedData[11].ToString("00") + ":" + receivedData[12].ToString("00") });
                dataGridView1.Rows.Add(new object[] { "8", paramdata[7], receivedData[13].ToString("00") + ":" + receivedData[14].ToString("00") + ":" + receivedData[15].ToString("00") });
                dataGridView1.Rows.Add(new object[] { "9", paramdata[8], (Convert.ToDecimal(IrDAParsor(receivedData, 16, 2)) / 10).ToString("0.0") });
                dataGridView1.Rows.Add(new object[] { "10", paramdata[9], (Convert.ToDecimal(IrDAParsor(receivedData, 18, 2)) / 10).ToString("0.0") });
                dataGridView1.Rows.Add(new object[] { "11", paramdata[10], (Convert.ToDecimal(IrDAParsor(receivedData, 20, 2)) / 10).ToString("0.0") });
                dataGridView1.Rows.Add(new object[] { "12", paramdata[11], (Convert.ToDecimal(IrDAParsor(receivedData, 22, 2)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "13", paramdata[12], (Convert.ToDecimal(IrDAParsor(receivedData, 24, 2)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "14", paramdata[13], (Convert.ToDecimal(IrDAParsor(receivedData, 26, 2)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "15", paramdata[14], (Convert.ToDecimal(IrDAParsor(receivedData, 32, 4)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "16", paramdata[15], (Convert.ToDecimal(IrDAParsor(receivedData, 44, 4)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "17", paramdata[16], (Convert.ToDecimal(IrDAParsor(receivedData, 48, 2)) / 100).ToString("0.00") });
                dataGridView1.Rows.Add(new object[] { "18", paramdata[17], receivedData[50].ToString("00") + ":" + receivedData[51].ToString("00") });
                dataGridView1.Rows.Add(new object[] { "19", paramdata[18], receivedData[52].ToString("00") + ":" + receivedData[53].ToString("00") + ":" + receivedData[54].ToString("00") });
                dataGridView1.Rows.Add(new object[] { "20", paramdata[19], "" + (char)receivedData[55] + (char)receivedData[56] + (char)receivedData[57] });
                dataGridView1.Rows.Add(new object[] { "21", paramdata[20], SelectedMterType(Convert.ToString(receivedData[58])) });
                dataGridView1.Rows.Add(new object[] { "22", paramdata[21], IrDAParsor(receivedData, 59, 2) });
                dataGridView1.Rows.Add(new object[] { "23", paramdata[22], IrDAParsor(receivedData, 77, 2) });
            }
            else { }
        }

        // Parse method
        private string IrDAParsor(byte[] receivedData, int start, int len)
        {
            string nonformateddata = "";
            string formateddata = "";

            for (int i = start; i < start + len; i++)
            {
                nonformateddata += receivedData[i].ToString("X2");        
            }

            formateddata = HexToDecimalConversion(nonformateddata);

            return formateddata;

        }

        public  string HexToDecimalConversion(string strHex)
        {
            try
            {
                int bytecnt = 0;
                bytecnt = strHex.Length;
                string strtemp = "";
                while (bytecnt > 0)
                {
                    strtemp += strHex.Substring(bytecnt - 2, 2);
                    bytecnt -= 2;
                }
                long aa = Int64.Parse(strtemp, System.Globalization.NumberStyles.HexNumber);
                return aa.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
      
        private void DisplayData(byte[] receivedData)
        {
            int countIndex = 0;
            //txtIrDAData.Text += "<--";
            while (countIndex < receivedData[5])
            {
                //txtIrDAData.Text += receivedData[countIndex].ToString("X2") + " ";
                countIndex++;
            }
            //txtIrDAData.Text += "\r\n";
            
        }


        string SelectedMterType(string phse)
        {
            if (phse == "0") return "Single Phase";
            else if (phse == "3") return "Three Phase";
            else return ""; 
        }

        private bool IsValidSelection()
        {
            //bool isSelected = false;
            //if (TreeViewExplorer.SelectedNode != null)
            //{
            //    switch (TreeViewExplorer.SelectedNode.Text)
            //    {
            //        case "Billing Data":
            //            isSelected = true;
            //            break;

            //    }
            //}
            //if (isSelected) return isSelected;
            //MessageBox.Show("Please Select atleast one option from List !", "DLMS-PT", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            //return isSelected;
            return true;
        }
       public string monthnme(string mnth)
        {
            string m_name;
            switch (mnth)
            {
                case "1": m_name = "January"; break;
                case "2": m_name = "February"; break;
                case "3": m_name = "March"; break;
                case "4": m_name = "April"; break;
                case "5": m_name = "May"; break;
                case "6": m_name = "June"; break;
                case "7": m_name = "July"; break;
                case "8": m_name = "August"; break;
                case "9": m_name = "September"; break;
                case "10": m_name = "October"; break;
                case "11": m_name = "November"; break;
                case "12": m_name = "December"; break;
                default: m_name = ""; break;
            }
            return m_name;

        }

        

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TreeViewExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to Export!", "IRDA", MessageBoxButtons.OK);
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DataRow reportRow;
                reportXSD.Tables["IRDABillingTable"].Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    reportRow = reportXSD.Tables["IRDABillingTable"].NewRow();
                    reportRow[0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    reportRow[1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    reportRow[2] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    reportXSD.Tables["IRDABillingTable"].Rows.Add(reportRow);
                }

                ReportForm objReportForm = new ReportForm();
                CABApplication.Reports.DLMS_Detailed_Reports.IRDABillingReport iRDABillingReport = new CABApplication.Reports.DLMS_Detailed_Reports.IRDABillingReport();

                //assinging data source to MidNightReport
                iRDABillingReport.SetDataSource(reportXSD);

                //assinging data source to ReportForm
                objReportForm.rptViewer.ReportSource = iRDABillingReport;


                objReportForm.rptViewer.Zoom(1);
                this.Hide();
                objReportForm.ShowDialog();
                reportXSD.Clear();
                this.Show();
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "buttonExport_Click", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

    }
}
