using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Channel;
using CAB.Channel.ReadOut;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CAB.UI;

namespace CABApplication
{
    public partial class IECPhasorReadout : MdiChildForm
    {
        private bool IsAborted = false;
        private ChannelBase communications;
        private Command command;
        private const string SIGNONFAILURE = "Sign-On failure";
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)"; 
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
         private ToolStripProgressBar toolStripBar;
        private StatusStrip statusBar;
        private ToolStripStatusLabel toolStripStatusLabel;
        private int isMeterType = 0; // Story - 347720 - To set the variable based on meter type is single phase DLMS

        public IECPhasorReadout()
        {
            InitializeComponent();
            command = Command.GetInstance();           
            if (!ConfigInfo.IsGSMConnected())
                communications = ChannelManager.GetChannel() as LocalCommunication;
            else
                communications = ChannelManager.GetChannel() as GSMCommunication;
           
        }
        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }

        public int IsMeterType  // 0 = 1P and 3P dlms ; 1 = SP NonDLMS ; 2 = SP NONDLMS 9600 ; 3 = 3P NON DLMS
        {
            get
            {
                return isMeterType;
            }
            set
            {
                isMeterType = value;
            }
        }
        /// <summary>
        /// Converts a datatable to phasor entity
        /// </summary>
        /// <returns></returns>
        private PhasorEntity GetPhasorEntity(DataTable table)
        {
            PhasorEntity phasorEntity;
            DataTable dtTable = new DataTable();
            foreach (DataRow dr in table.Rows) //Add columns
            {
                string coulmnName = dr[0].ToString();
                coulmnName = new string(coulmnName.ToList().Where(c => c != ' ').ToArray());
                coulmnName = new string(coulmnName.ToList().Where(c => c != '/').ToArray());
                if (coulmnName == "AngleBWAny2PhasePresent")
                {
                    coulmnName = "AngleBWAnyPhasePresent";
                }
                dtTable.Columns.Add(coulmnName, typeof(string));
                
            }
            foreach (DataRow dr in table.Rows) //Add columns
            {
                string coulmnName = dr[2].ToString();
                coulmnName = new string(coulmnName.ToList().Where(c => c != ' ').ToArray());
                coulmnName = new string(coulmnName.ToList().Where(c => c != '/').ToArray());
                if (coulmnName == "AngleBWAny2PhasePresent")
                {
                    coulmnName = "AngleBWAnyPhasePresent";
                }
                dtTable.Columns.Add(coulmnName, typeof(string));

            }
            DataRow drRow = dtTable.NewRow();
            int counter;
            for (counter=0; counter < table.Rows.Count; counter++) // Add column values
            {
                drRow[counter] = table.Rows[counter][1].ToString();

            }
            for (int i = 0; i < table.Rows.Count; i++) // Add column values
            {
                drRow[counter] = table.Rows[i][3].ToString();
                counter++;
            }
            dtTable.Rows.Add(drRow);    
            if (dtTable.Rows.Count > 0)
            {
                return phasorEntity = (PhasorEntity)new DLMS650PhasorBLL().GetPhasorDataEntity(dtTable);
            }
            return phasorEntity = null;
        }      
                
        private void ChangeStatus(string data)
        {
            this.RightStatusMessage = string.Empty;

            if (data.Trim() != string.Empty)
                this.StatusMessage = MessageConstant.GetText("M000068");
            else
                this.StatusMessage = string.Empty;
        }

        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool isConnected)
        {
            string mode;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (isConnected)
            {
                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "IEC" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

            }
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            SetConnectionDetail(false);
            StopProgressBarTimer();
            this.Close();
            ChangeStatus(string.Empty);
        }
        public static string PhasorFilterData(string data, int start, int end, int div, string format)
        {
            return (Convert.ToDouble((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString()) / div).ToString(format);
        }

        public static string PhasorFilterData(string data, int start, int end, double div)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber) * div).ToString());
        }

        public static string PhasorFilterData(string data, int start, int end)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString());
        }


        public static DataSet DisplayPhasor(string PhasorPara)
        {
            DataTable table = new DataTable();
            int col = 0;
            string[] phasorRow = PhasorRow();
            string[] phasorColumn = PhasorColumnValues();

            for (col = 0; col < phasorRow.Length; col++)
            {
                table.Columns.Add(new DataColumn(phasorRow[col], typeof(System.String)));
            }

            for (int counter = 0; counter < 15; counter++)
            {
                DataRow dataRow = table.NewRow();
                for (col = 0; col < phasorRow.Length; col++)
                {
                    if (col == 0)
                    {
                        dataRow[col] = phasorColumn[counter];
                    }
                    if (col == 2)
                    {
                        dataRow[col] = phasorColumn[counter + 15];
                    }

                }
                table.Rows.Add(dataRow);
            }


            //Voltage R y  b  Phase
            table.Rows[0][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 1, 4, 100, "0.00");
            table.Rows[1][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 5, 4, 100, "0.00");
            table.Rows[2][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 9, 4, 100, "0.00");

            /*Current R y  b  Phase*/
            table.Rows[3][1]  = ReadoutCommon.PhasorFilterData(PhasorPara, 13, 8, 1000, "0.000");
            table.Rows[4][1]  = ReadoutCommon.PhasorFilterData(PhasorPara, 21, 8, 1000, "0.000");
            table.Rows[5][1]  = ReadoutCommon.PhasorFilterData(PhasorPara, 29, 8, 1000, "0.000");

            /*Total Active, Inductive, Capacitive and Apparent Power*/
            table.Rows[6][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 38, 8, 100000, "0.000");
            table.Rows[7][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 46, 8, 100000, "0.000");
            table.Rows[8][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 54, 8, 100000, "0.000");
            table.Rows[9][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 62, 8, 100000, "0.000");


            /*PF R y  b  Phase*/
            table.Rows[10][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 70, 4, 10000, "0.00");
            table.Rows[11][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 74, 4, 10000, "0.00");
            table.Rows[12][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 78, 4, 10000, "0.00");

            table.Rows[13][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 82, 8, 10000, "0.00");
            //Frequency   
            table.Rows[14][1] = ReadoutCommon.PhasorFilterData(PhasorPara, 90, 4, 100, "0.00");


            table.Rows[0][3] = "CORRECT";

            /*Total */
            table.Rows[1][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 96, 2)) == 0) ? "Import" : "Export";

            //Iport/Export R y  b  Phase
            table.Rows[2][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 98, 2)) == 0) ? "Import" : "Export";
            table.Rows[3][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 100, 2)) == 0) ? "Import" : "Export";
            table.Rows[4][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 102, 2)) == 0) ? "Import" : "Export";

            //Chaneel Missing R y  b  Phase
             table.Rows[5][3]  = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 104, 2)) == 0) ? "Absent" : "Present";
             table.Rows[6][3]  = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 106, 2)) == 0) ? "Absent" : "Present";
             table.Rows[7][3]  = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 108, 2)) == 0) ? "Absent" : "Present";

            /*Lag/ Lead R y  b  Phase*/

              table.Rows[8][3]  = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 110, 2)) == 0) ? "Lag" : "Lead";
              table.Rows[9][3]  = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 112, 2)) == 0) ? "Lag" : "Lead";
              table.Rows[10][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 114, 2)) == 0) ? "Lag" : "Lead";


            /*Lag/ Lead Total*/
            table.Rows[11][3] = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 116, 2)) == 0) ? "Lag" : "Lead";

            /* Y B Phase Angle with respect to R Phase*/
            table.Rows[12][3]= ReadoutCommon.PhasorFilterData(PhasorPara, 118, 2, 7.2);
            table.Rows[13][3]= ReadoutCommon.PhasorFilterData(PhasorPara, 120, 2, 7.2);
            table.Rows[14][3] = ReadoutCommon.PhasorFilterData(PhasorPara, 122, 2, 7.2);

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string[] PhasorRow()
        {
            string[] array = new string[4];
            array[0] = "Parameter1";
            array[1] = "Value1";
            array[2] = "Parameter2";
            array[3] = "Value2";
            return array;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string[] PhasorColumnValues()
        {
            string[] array = new string[30];
            array[0] = "R Phase Voltage";
            array[1] = "Y Phase Voltage";
            array[2] = "B Phase Voltage";
            array[3] = "R Phase Current";
            array[4] = "Y Phase Current";
            array[5] = "B Phase Current";
            array[6] = "Total Active Power";
            array[7] = "Total Inductive Power";
            array[8] = "Total Capacitive Power";
            array[9] = "Total Apparent Power";
            array[10] = "R Phase PF";
            array[11] = "Y Phase PF";
            array[12] = "B Phase PF";
            array[13] = "Total Instantaneous PF";
            array[14] = "Frequency";
            array[15] = "Phase Sequence";
            array[16] = "Total kW Direction";
            array[17] = "R Phase kW Direction";
            array[18] = "Y Phase kW Direction";
            array[19] = "B Phase kW Direction";
            array[20] = "R Phase Channel";
            array[21] = "Y Phase Channel";
            array[22] = "B Phase Channel";
            array[23] = "R Phase Lag/Lead";
            array[24] = "Y Phase Lag/Lead";
            array[25] = "B Phase Lag/Lead";
            array[26] = "Total";
            array[27] = "Y Phase Angle With R Phase";
            array[28] = "B Phase Angle With R Phase";
            array[29] = "Angle B/W Any 2 Phase Present";
            return array;
        }

          /// <summary>
        /// to start the progress bar and overlap the position 
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StartProgressBarTimer()
        {
            statusStrip.Visible = true;
            progressBarTimer.Enabled = true;
        }

        /// <summary>
        /// to stop progress bar , make it in-visible and make visible.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StopProgressBarTimer()
        {
            statusStrip.Visible = false;
            progressBarTimer.Enabled = false;
        }


        private void btnReadPhasor_Click(object sender, EventArgs e)
        {
            try
            {
               
                StartProgressBarTimer();
                SetConnectionDetail(true);
                this.RightStatusMessage = "Reading Phasor Data";
                this.StatusMessage = "";
                Application.DoEvents();
                IsAborted = false;
                DataAcquisition.Enabled = false;
                Configuration.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                ReadoutPhasor readoutPhasor = new ReadoutPhasor();
                readoutPhasor.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
                readoutPhasor.Channel = communications;
                if (isMeterType == 1 || isMeterType == 2)
                {
                    this.StatusMessage = "Readout Not Supported."; 
                    return;
                }
                string phasoreadoutPhasorrData = readoutPhasor.GetData();
               
                if (readoutPhasor.IsSignOnFailure)
                {

                    SetConnectionDetail(false);
                    StopProgressBarTimer();
                    this.StatusMessage = "Signon Fail";
                    btnReadPhasor.Enabled = true;
                    btnStopPhasor.Enabled = false;
                    btnCancelPhasor.Enabled = true;
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                    return;
                }
                btnCancelPhasor.Enabled = false;
                btnStopPhasor.Enabled = true;
                btnReadPhasor.Enabled = false;
                if (!phasoreadoutPhasorrData.Trim().Equals(string.Empty))
                {
                    this.StatusMessage = "Reading Phasor data.....";
                    do
                    {
                        readoutPhasor.IsAborted = IsAborted;
                        phasoreadoutPhasorrData = readoutPhasor.GetData();
                        if (phasoreadoutPhasorrData != "")
                        {
                            DataSet phasorData = DisplayPhasor(phasoreadoutPhasorrData);
                            if (phasorData != null && phasorData.Tables.Count > 0)
                            {
                                if (phasorData.Tables[0].Rows[0][3].ToString() != "CORRECT")
                                {
                                    lblPhasorData.Visible = true;
                                    this.lblPhasorData.Text = "Phase Sequence is not correct. Phasor can not be shown";
                                    lngPhasorDiagram.Visible = false;
                                    lngPgrid.Data = null;
                                }
                                else
                                {
                                    PhasorEntity phasorEntity = GetPhasorEntity(phasorData.Tables[0]);
                                    lblPhasorData.Visible = false;
                                    lngPhasorDiagram.PhasorData = phasorEntity;
                                    lngPhasorDiagram.Refresh();
                                    lngPgrid.Visible = true;

                                    lngPgrid.Data = phasorData;
                                    this.Cursor = Cursors.Default;
                                    lngPgrid.SetWidth(0, 130);
                                    lngPgrid.SetWidth(1, 60);
                                    lngPgrid.SetWidth(2, 168);
                                    lngPgrid.SetWidth(3, 60);
                                    lngPgrid.SetHeaderText(0, "Parameters");
                                    lngPgrid.SetHeaderText(1, "Values");
                                    lngPgrid.SetHeaderText(2, "Parameters");
                                    lngPgrid.SetHeaderText(3, "Values");
                                    lngPgrid.ResizeColumn(false);
                                    lngPgrid.IsSorting = false;
                                }
                            }

                        }
                        Application.DoEvents();
                        if (!readoutPhasor.IsPhasor)
                            break;
                        if (IsAborted)
                            break;
                        if (phasoreadoutPhasorrData == string.Empty)
                        {
                            SetConnectionDetail(false);
                            StopProgressBarTimer();
                            this.StatusMessage = "Timeout!";
                            this.RightStatusMessage = string.Empty;
                            btnReadPhasor.Enabled = true;
                            btnStopPhasor.Enabled = false;
                            this.Cursor = Cursors.Default;
                            Application.DoEvents();
                            break;
                        }
                        Thread.Sleep(200);
                    } while (true);

                }
                else
                    lngPgrid.Data = null;
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
                DataAcquisition.Enabled = true;
                Configuration.Enabled = true;
                SetConnectionDetail(false);
                StopProgressBarTimer();
            }
        }

        private void btnCancelPhasor_Click(object sender, EventArgs e)
        {
            this.Close();
            ChangeStatus(string.Empty);
            SetConnectionDetail(false);
            StopProgressBarTimer();
        }

        private void btnStopPhasor_Click(object sender, EventArgs e)
        {
           
            IsAborted = true;
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            communications.ClosePort();
            this.StatusMessage = "User Aborted.";
            lblPhasorData.Visible = false;
            this.RightStatusMessage = string.Empty;
            SetConnectionDetail(false);
            StopProgressBarTimer();
            Application.DoEvents();
            Thread.Sleep(500);
            this.StatusMessage = "Phasor reading stopped.";
            Application.DoEvents();
            btnReadPhasor.Enabled = true;
            btnStopPhasor.Enabled = false;
            btnCancelPhasor.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void IECPhasorReadout_Load(object sender, EventArgs e)
        {
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];                   
        }
        /// <summary>
        /// progress bar timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBarTimer_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar.Value > toolStripProgressBar.Maximum - 1)
            {
                toolStripProgressBar.Value = 0;
            }
            else
            {
                toolStripProgressBar.Value = toolStripProgressBar.Value + 10;
            }
        }

       

       


   
    }
}

