#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.Channel.ReadOut;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.MeterData.Upload;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Net;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.ServiceProcess;
using XMLCCConvertion; 
#endregion
namespace CABApplication
{
    /// <summary>
    /// Responsible for all meter readout activities
    /// </summary>
    partial class E650MeterDataReadout : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private Communication communication;
        private GenerateEntity entityGenerator;
        private List<byte> meterId = null;
        private const string Splitter = "$";
        private bool isMeterReading = false;
        private const string ReadoutFailure = "Readout Failure.";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650MeterDataReadout).ToString());
        Thread readThread;
        public List<System.Enum> enumData;

        private MeterMasterBLL meterMasterBLL = null;
        private static string simNumber = string.Empty;
        private static string Staticip = string.Empty;
        private static string Tcpport = string.Empty;
        private CommunicationType commType;

        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
        private delegate void SetCursorCallBack(Cursor cursor);
        private delegate void SetDataGridStyleCallback(System.Enum profileId, DataGridViewCellStyle style);
        private delegate void SetDataGridStatusCallback(System.Enum profileId, string style);
        private CommunicationMode commMode = CommunicationMode.Normal;
        private bool readSuccess = false;
        private string firmwareVersion = null;
        List<System.Enum> selectedMeterConfigProfile;
        private Serializer serializer = null;
        private static object syncRoot = new object();
        private static MeterConfigSettings meterConfigSettings = null;
        private ProfileId currentProfile = 0;
        private int countProfile;
        private List<Thread> lstThread = null;
        public int securitymachanism = 0;

        private static string MeterSerialNumber = string.Empty;
        string Taskname = DateTime.Now.ToString("yyyyMMddHHmmss");
        Thread ReadThread = null;
        DateTime readoutDateTime;
        public enum dgvSimColumn
        {
            /// <summary>
            /// serial Number
            /// </summary>
            SN = 0,
            /// <summary>
            /// Sim Number
            /// </summary>
            SimNo,
            /// <summary>
            /// Meter ID
            /// </summary>
            MeterID,


            /// <summary>
            /// Select
            /// </summary>
            Select,
            /// <summary>
            /// Status
            /// </summary>
            Status,


        }
        #endregion

        #region Properties

        #endregion

        #region Constructor
        /// <summary>
        /// This is constructer
        /// </summary>
        public E650MeterDataReadout()
        {
            InitializeComponent();
            entityGenerator = new GenerateEntity();
            commType = GetCommuniactioType();
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.ModemInfo = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
            meterMasterBLL = new MeterMasterBLL();
            selectedMeterConfigProfile = new List<System.Enum>();
            FillMeterIdSerialNumber();
            //to deserialize the xml file
            serializer = new Serializer();
            lock (syncRoot)
            {
                if (meterConfigSettings == null)
                {
                    meterConfigSettings = (MeterConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterConfigSettings.xml"), typeof(MeterConfigSettings));
                }
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        /// <summary>
        /// Form Load .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterDataReadout_Load(object sender, EventArgs e)
        {
            //BindLoadSurveyDays();
           // txtPort.Text = ConfigSettings.GetValue("TCPPORT");
            //Get CommunicationMode i.e Normal or FD
            commMode = GetCommuniactionMode();
            this.rdbAllData_CheckedChanged(this, null);
            btnCancel.Enabled = true;

            if (commType == CommunicationType.DIRECT)
            {
                GsmCommPanel.Visible = false;
            }
            else
            {
                GsmCommPanel.Visible = true;
                if (commType == CommunicationType.GPRS)
                {
                    lngSimNumber.Text = "Modem IMEI Number: ";
                    grpSimNumber.Text = "IMEI Number";
                    txtBoxMeterSIM.MaxLength = 15;
                }
                if (commType == CommunicationType.TCP)
                {
                    lngSimNumber.Text = "Static IP: ";
                    grpSimNumber.Text = "IP/Port";
                    txtBoxMeterSIM.MaxLength = 22;
                    lngPort.Visible = true;
                    txtPort.Visible = true;
                }
            }
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbPartialData_CheckedChanged(object sender, EventArgs e)
        {
            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            btnAbort.Enabled = false;
            btnRead.Enabled = false;
            btnCancel.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbAllData_CheckedChanged(object sender, EventArgs e)
        {

            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            btnAbort.Enabled = false;
            btnRead.Enabled = true;
            btnCancel.Enabled = false;
        }
        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterDataReadout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMeterReading)
            {
                e.Cancel = true;
            }
            else
            {
                AbortThread(lstThread);
                if (readThread != null && readThread.IsAlive)
                {
                    readThread.Abort();
                }
                MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;

                if (menuStrip.InvokeRequired)
                {
                    menuStrip.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    DataAcquisition.Enabled = true;
                    Configuration.Enabled = true;
                    SetConnectionDetail(false);
                    EnableStopTimer();
                }
                this.StatusMessage = "";
            }
        }
        /// <summary>
        /// Clear status messsage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlReadSata_MouseClick(object sender, MouseEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageReadData_Enter(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPagePhasor_Enter(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageFraudEnergy_Enter(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }


        private void lngGridViewReadControl1_Load(object sender, EventArgs e)
        {
            string comMode = ConfigSettings.GetValue("CommunicationMode");
            lngGridViewReadControl1.SetDefaultCellStyle(true);
            enumData = new List<System.Enum>();
            enumData.Add(ProfileId.Instant);
            enumData.Add(ProfileId.Phasor);
            enumData.Add(ProfileId.Billing);
            enumData.Add(ProfileId.Tamper);
            enumData.Add(ProfileId.Midnight);
            // Meter Configuration supported in FD as well
            enumData.Add(ProfileId.MeterConfiguration);
            enumData.Add(ProfileId.LoadSurvey);
            enumData.Add(ProfileId.LoadSwitch);
            lngGridViewReadControl1.AddEnumList(enumData, true);
            btnAbort.Enabled = false;
        }

        /// <summary>
        /// Read Meter data .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            DataGridViewCellStyle style;
            foreach (System.Enum profile in enumData)
            {
                style = new DataGridViewCellStyle();
                lngGridViewReadControl1.SetStatus(profile, "Readout Not Started...");
                style.BackColor = System.Drawing.Color.White;
                lngGridViewReadControl1.SetColour(profile, style);
            }
            if (oneToOneGSM.Checked && commType != CommunicationType.TCP)
            {
                if (!ValidateSimNumber())
                {
                    return;
                }
            }
            else if(oneToOneGSM.Checked && commType == CommunicationType.TCP)
            {
                if (!ValidateIP())
                {
                    return;
                }
            }
            lngGridViewReadControl1.Enabled = false;
            dgvMeterIdAndSim.Enabled = false;
            selectAll.Enabled = false;
            btnRead.Enabled = false;
            // btnAbort.Enabled = true;
            btnCancel.Enabled = false;
            DataAcquisition.Enabled = false;
            Configuration.Enabled = false;
            grpSimNumber.Enabled = false;
            grpCommType.Enabled = false;
            readThread = new Thread(ReadMeter);
            //making the thread background as it communication needs to be stoppped when BCS closes.
            readThread.IsBackground = true;
            readThread.Start(SynchronizationContext.Current);
           
        }
        private void CheckAndStartService()
        {
            try
            {
                ServiceController service = new ServiceController(CABApplication.Properties.Resources.SERVICENAME);
                if (service != null)
                {
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        service.Start();
                    }
                }
                else
                {
                    MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                    Application.Exit();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                logger.Log(LOGLEVELS.Error, "CheckAndStartService()", ex);
                Application.Exit();
            }

        }

        private void CheckAndStopService()
        {
            try
            {
                ServiceController service = new ServiceController(CABApplication.Properties.Resources.SERVICENAME);
                if (service != null)
                {
                    if (service.Status != ServiceControllerStatus.Stopped)
                    {
                        service.Stop();
                    }
                }
                else
                {
                    MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                    Application.Exit();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                logger.Log(LOGLEVELS.Error, "CheckAndStopService()", ex);
                Application.Exit();
            }

        }

        private bool ValidateIP()
        {
            bool flag = true;
            long portNumber = 0;
            IPAddress ipAddress = null;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!IPAddress.TryParse(txtBoxMeterSIM.Text, out ipAddress))
            {
                CABMessageBox.ShowFilterMessage("M000121", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!long.TryParse(txtPort.Text, out portNumber))
            {
                CABMessageBox.ShowFilterMessage("M000122", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else
            {

            }
            return flag;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            btnAbort.Enabled = false;
            AbortThread(lstThread);
            AbortReadThread(ReadThread);
            if (readThread != null && readThread.IsAlive)
            {
                readThread.Abort();
            }
            btnCancel.Enabled = false;
            communication.CloseSession();
            Application.DoEvents();
           this.StatusMessage = "User Aborted.";
            if (currentProfile != 0)
            {
                SetGridRowAttributes(Color.Red, currentProfile, "User Aborted");
            }
            if (oneToManyGSM.Checked && countProfile != -1)
            {
                for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
                {
                    string Rowstatus = dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Value.ToString();
                    if (Rowstatus != "Readout completed.")
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style.BackColor = System.Drawing.Color.Red;
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Value = "User Aborted";
                    }
                }
            }
            grpSimNumber.Enabled = true;
            grpCommType.Enabled = true;
            Application.DoEvents();
            this.Cursor = Cursors.Default;
            SetConnectionDetail(false);
            EnableStopTimer();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetConnectionDetail(false);
            this.StatusMessage = string.Empty;
            EnableStopTimer();
            this.Close();
        }

        private void oneToOneGSM_CheckedChanged(object sender, EventArgs e)
        {
            txtPort.Text = ConfigSettings.GetValue("TCPPORT");
            dgvMeterIdAndSim.Enabled = false;
            selectAll.Enabled = false;
            grpSimNumber.Enabled = true;
        }

        private void oneToManyGSM_CheckedChanged(object sender, EventArgs e)
        {
            txtPort.Text = "";
            dgvMeterIdAndSim.Enabled = true;
            selectAll.Enabled = true;
            grpSimNumber.Enabled = false;
        }
        /// <summary>
        /// on click , selects all check boxes or unselects all check boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAll.Checked)
            {
                for (int count = 0; count < dgvMeterIdAndSim.RowCount; count++)
                {
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = true;
                }
            }
            else
            {
                for (int count = 0; count < dgvMeterIdAndSim.RowCount; count++)
                {
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }
            }
        }
        /// <summary>
        /// to update select all button , when one check box is unchecked from all checked boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMeterIdAndSim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool flag = true;
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {

                if (!(bool)dgvMeterIdAndSim.Rows[count].Cells["Select"].Value)
                {
                    flag = false;
                    break;
                }
            }

            selectAll.CheckedChanged -= selectAll_CheckedChanged;
            if (flag)
            {
                selectAll.Checked = true;
            }
            else
            {
                selectAll.Checked = false;
            }
            selectAll.CheckedChanged += selectAll_CheckedChanged;
        }
        /// <summary>
        /// to enable select all button , be checked or unchecked , when a check box is clicked in column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvMeterIdAndSim_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvMeterIdAndSim.IsCurrentCellDirty)
            {
                dgvMeterIdAndSim.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            }
        }
        /// <summary>
        /// timer for progress bar
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

        #endregion

        #region Private Methods
        /// <summary>
        /// To make sure that file upload window will same as IEC.
        /// </summary>
        /// <param name="fileText"></param>
        private void SaveData(string fileText, string meterId)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
            if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId + "_" + fileName;
            bool Flag = false;
        ReConf:
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessageAsync = "File name can't be empty.";
                        this.RightStatusMessageAsync = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessageAsync = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");
                return;
            }
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
            filePath = filePath.Replace("\\\\", "\\");
            if (File.Exists(filePath))
            {
                DialogResult dr = CABMessageBox.ShowFilterMessage("M000046", "A000001", MessageBoxButtons.YesNo);
                if (dr.Equals(DialogResult.No))
                    goto ReConf;
            }
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                FileUplaod(filePath);
                // this.StatusMessageAsync = MessageConstant.GetText("M000048");
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Log(LOGLEVELS.Error, "SaveData(string fileText, string meterId)", Ex);
            }



        }
        /// <summary>
        /// To make sure that file upload window will same as IEC.
        /// </summary>
        /// <param name="fileText"></param>
        private void SaveRemoteData(string fileText, string meterId)
        {
            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
            if (fileName.Trim().Equals(string.Empty))
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");

            }
            else
            {
                // To make sure that both direct and remote communication read file naming convention will be same.
                if (fileName.Length >= 14)
                {
                    fileName = fileName.Substring(0, 8) + "_" + fileName.Substring(8, 6);
                }
                fileName = meterId + "_" + fileName;
                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".2NG");
                filePath = filePath.Replace("\\\\", "\\");

                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file);
                    wr1.Write(fileText);
                    wr1.Close();
                    file.Close();
                    this.StatusMessageAsync = MessageConstant.GetText("M000048");
                    FileUplaod(filePath);
                }
                catch (Exception Ex)    //Exception log for catch block
                {
                    MessageBox.Show(Ex.Message, "BCS");
                    logger.Log(LOGLEVELS.Error, "SaveRemoteData(string fileText, string meterId)", Ex);
                }

            }

        }

        /// <summary>
        /// To make sure that file upload window will same as IEC.
        /// </summary>
        /// <param name="fileText"></param>
        private void CreateXMLFileForCC(string fileText, string meterId, string metersignature)
        {
            try
            {
                ReadOutToXML _readdatatoxml = new ReadOutToXML(meterId, readoutDateTime, "", fileText);

                _readdatatoxml.GetReadoutData("");

                if (!_readdatatoxml.GenerateXMLFile("", "", metersignature))
                {
                    MessageBox.Show("Unable to generate xml data", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //logger.Log(LOGLEVELS.Error, "SaveData(string fileText, string meterId)", new Exception());
                }
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //logger.Log(LOGLEVELS.Error, "SaveData(string fileText, string meterId)", Ex);
            }



        }

        private void FileUplaod(string filePath)
        {
            bool IsUploaded = false;
            UploadFile uploadFile = new UploadFile();
            this.StatusMessageAsync = "Uploading readout file...";
            string resultMessage = string.Empty;
            ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
            //this.Cursor = Cursors.WaitCursor;
            IsUploaded = uploadFile.Upload2NGFile(filePath, uploadFile.GetContent(filePath), true, out resultMessage, null);

            if (IsUploaded)
            {
                this.ListRefreshAsync = true;
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = "File Uploaded successfully.";
                Application.DoEvents();

            }
            else
            {
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = resultMessage;
            }
            //this.Cursor = Cursors.Default;

        }

        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableReadControls()
        {
            try
            {
                if (btnRead.InvokeRequired)
                {
                    btnRead.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    btnRead.Enabled = true;
                }
                if (btnAbort.InvokeRequired)
                {
                    btnAbort.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    btnAbort.Enabled = false;
                }
                if (btnCancel.InvokeRequired)
                {
                    btnCancel.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    btnCancel.Enabled = true;
                }
                if (dgvMeterIdAndSim.InvokeRequired)
                {
                    dgvMeterIdAndSim.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    dgvMeterIdAndSim.Enabled = true;
                    selectAll.Enabled = true;
                }
                if (lngGridViewReadControl1.InvokeRequired)
                {
                    lngGridViewReadControl1.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    lngGridViewReadControl1.Enabled = true;
                }
                if (grpSimNumber.InvokeRequired)
                {
                    grpSimNumber.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    grpSimNumber.Enabled = true;
                }
                if (grpCommType.InvokeRequired)
                {
                    grpCommType.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    grpCommType.Enabled = true;
                }

                if (this.Parent != null)
                {
                    MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;

                    if (menuStrip.InvokeRequired)
                    {
                        menuStrip.Invoke(new MethodInvoker(EnableReadControls));
                    }
                    else
                    {
                        DataAcquisition.Enabled = true;
                        Configuration.Enabled = true;
                        SetConnectionDetail(false);
                        EnableStopTimer();
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "EnableReadControls()", ex);
            }
            finally
            {
                progressBarTimer.Enabled = false;
                progressBarTimer.Dispose();
                progressBarTimer.Stop();
            } 
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

        /// <summary>
        /// enable start timer for different thread
        /// </summary>
        private void EnableStartTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStartTimer));
            }
            else
            {
                StartProgressBarTimer();
            }
        }

        /// <summary>
        /// enable stop timer for different thread
        /// </summary>
        private void EnableStopTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStopTimer));
            }
            else
            {
                StopProgressBarTimer();
            }
        }

        private void SetDataGrid()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(DisableAbort));
            }
            else
            {
                btnAbort.Enabled = false;
            }
        }


        /// <summary>
        /// Disable abort button .
        /// </summary>
        private void DisableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(DisableAbort));
            }
            else
            {
                btnAbort.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the signature data in file format
        /// </summary>
        /// <param name="signatureInfo"></param>
        /// <returns></returns>
        private string GetSignatureDataInFileFormat(string signatureInfo)
        {
            //string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            string outputSignatureInfo = "0100006001BCFF02";
            // Combining the signature Data length and Octet string with the outputSignatureInfo //User Story 478245. Voltage Rating change to 63.5 V for HK meter
            if (ConfigInfo.SignatureDataLength != string.Empty)
            {
                outputSignatureInfo += ConfigInfo.SignatureDataLength;
            }
            else
            {
                outputSignatureInfo += "0914";
            }
            byte[] dataInByteForm = System.Text.Encoding.ASCII.GetBytes(signatureInfo);

            for (int dataIndex = 0; dataIndex < signatureInfo.Length; dataIndex++)
            {
                outputSignatureInfo = outputSignatureInfo + String.Format("{0:X2}", dataInByteForm[dataIndex]);
            }
            return outputSignatureInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CommunicationType GetCommuniactioType()
        {
            CommunicationType commType = CommunicationType.DIRECT;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (channelType == CABCommunication.PhysicalLayer.ChannelType.GSM.ToString())
            {
                commType = CommunicationType.GSM;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString())
            {
                commType = CommunicationType.PSTN;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString())
            {
                commType = CommunicationType.GPRS;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.TCP.ToString())
            {
                commType = CommunicationType.TCP;
            }
            return commType;
        }
        /// <summary>
        /// Gets the communication mode
        /// </summary>
        /// <returns></returns>
        private CommunicationMode GetCommuniactionMode()
        {
            string comMode = ConfigSettings.GetValue("CommunicationMode");
            if (comMode == CommunicationMode.FastDownload.ToString())
            {
                commMode = CommunicationMode.FastDownload;
            }
            else
            {
                commMode = CommunicationMode.Normal;
            }
            return commMode;
        }


      


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        private bool GetMeterData(string strFileName, bool isRemote, int simIndex)
        {
            FileStream initialFileStream = null;
            StreamWriter writeToFile = null;
            string data = string.Empty;
            string lngFilename = string.Empty;
            string classIdOBISCodeAttribute = string.Empty;
            string meterSerialNumber = string.Empty;
            string idLength = string.Empty;
            int index = 0;
            String fileMeterData;
            int meterModelNumber = 0;
            data = string.Empty;
            List<System.Enum> selectedProfiles;
            List<ProfileCommand> lstProfileCommands;
            lstProfileCommands = GetProfileCommandEntity();
            // selectedProfiles = GetSelectedProfilesToRead();
            bool isConnected = true;
            ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
            Result result = communication.Send(profileCommand);
            string signatureInfo = "";

            try
            {
                if (result != null && result.ErrorCode == CommunicationErrorType.Success)
                {
                    if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                    {
                        //result.RecieveDataBuffer[0] = 0x06;
                        //result.RecieveDataBuffer[1] = 0x02;
                        //result.RecieveDataBuffer[2] = 0x51;
                        //result.RecieveDataBuffer[3] = 0x02;
                        //result.RecieveDataBuffer[4] = 0x5E;
                        //result.RecieveDataBuffer[5] = 0x06;
                        if (result.RecieveDataBuffer[0] != 0x06)
                        {

                            idLength = result.RecieveDataBuffer[1].ToString("00");
                            index = Convert.ToInt16(result.RecieveDataBuffer[1]);

                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);
                            for (int i = 2; i < index + 2; i++)
                            {
                                data += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();
                            }
                        }

                        else // To handle hpl meter id datatype 0x06 --> 4 byte integer
                        {

                            int adbyteindex = 0;
                            byte[] incount = new byte[4];
                            for (int i = 1; i <= 4; i++)
                            {
                                incount[adbyteindex++] = result.RecieveDataBuffer[i];

                            }
                            data = Convertstring.FormatData(incount, false);
                        }
                        if (isRemote && commType == CommunicationType.GSM)
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, simIndex].Value = data;
                        }

                        #region CreateResultFile
                        try
                        {
                            meterSerialNumber = data.TrimEnd();
                            Application.DoEvents();
                            strFileName = strFileName + data;
                            readoutDateTime = DateTime.Now;
                            strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                            fileMeterData = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);

                            initialFileStream = new FileStream(strFileName, FileMode.Create);
                            writeToFile = new StreamWriter(initialFileStream);
                            writeToFile.WriteLine(Splitter);
                            writeToFile.WriteLine(index.ToString("00"));
                            writeToFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            result.ErrorCode = CommunicationErrorType.InvalidMeterIDName;
                            logger.Log(LOGLEVELS.Error, "GetMeterData(string strFileName, bool isRemote, int simIndex)", ex);
                        }
                        #endregion

                        Application.DoEvents();
                        DateTime meterTime;
                        
                        int meterType = 0;
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            meterTime = communication.GetMeterDateTime();
                            signatureInfo = communication.GetSignatureData();
                            if (signatureInfo == "")
                            {
                                this.StatusMessageAsync = "Meter Reading Failed";
                                return false;
                            }

                            if (signatureInfo.ToUpper() == "**2.21240010060WC4RSL")
                            {
                                meterType = communication.GetMeterType();
                                firmwareVersion = signatureInfo.Substring(0, 6).Trim('*');

                            }
                            else if (signatureInfo.ToUpper().IndexOf("FS") >= 0)
                            {
                                meterType = communication.GetMeterType();
                                firmwareVersion = signatureInfo.Substring(0, 7).Trim('#');  // for 1P smart meter
                            }
                            else
                            {
                                if (signatureInfo != "")
                                    firmwareVersion = signatureInfo.Substring(0, 6).Trim('*');
                            }

                            if (signatureInfo.ToUpper().Contains("WC"))
                            {
                                if (meterType == 5)
                                {
                                    meterModelNumber = NamePlateConstants.RubyE350Value;
                                }
                                else
                                {
                                    meterModelNumber = NamePlateConstants.RubyE250Value;
                                }
                            }
                            //*******Other manufacturer meter support  ***********//
                            else if (ConfigSettings.GetValue("OtherManufacture") == "TRUE" && signatureInfo != "Non-Landis+GyrMeter")
                            {
                                MessageBox.Show("Setting saved as Non Cabcon Meter " + "\n" + "Connected Meter is Cabcon." + "\n" + "Please change the settings or change the meter.");
                                this.StatusMessageAsync = "";
                                return isConnected = false;

                                
                            }
                            else if (signatureInfo.Contains("SM0310"))
                            {
                                meterModelNumber = NamePlateConstants.SmartM_Cipher_WCM;
                                firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                            }
                            else if (signatureInfo.Contains("SM0405"))
                            {
                                meterModelNumber = NamePlateConstants.SmartM_Cipher_LTCT;
                                firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                            }
                            else if (signatureInfo.Contains("SM0110"))
                            {
                                meterModelNumber = NamePlateConstants.SmartM_Cipher_1PH;
                                firmwareVersion = signatureInfo.Substring(signatureInfo.LastIndexOf('-') + 1, 10);
                            }
                            else if (signatureInfo.Contains("lt"))
                            {
                                meterModelNumber = NamePlateConstants.TwoTOUltModelValue;
                            }
                            else if ((signatureInfo.Contains("LT")))
                            {
                                meterModelNumber = NamePlateConstants.PumaLTE650Value;
                            }
                            else if (signatureInfo.Contains("ST"))
                            {
                                meterModelNumber = NamePlateConstants.SapphireLTCT;
                            }
                            else if (signatureInfo.Contains("st"))
                            {
                                meterModelNumber = NamePlateConstants.SapphireLTCT_st;
                            }
                            else if (signatureInfo.ToUpper().Contains("HT") || signatureInfo.ToUpper().Contains("HK"))
                            {
                                meterModelNumber = NamePlateConstants.PumaHTE650Value;
                            }
                            else if (signatureInfo.ToUpper().Contains("HK"))
                            {
                                meterModelNumber = NamePlateConstants.PumaHTE650Value;
                            }
                            else if (signatureInfo.ToUpper().Contains("LC"))
                            {
                                meterModelNumber = NamePlateConstants.LTCTCortexValue;
                            }
                            else if (signatureInfo.Contains("uk"))
                            {
                                meterModelNumber = NamePlateConstants.Ruby6ukModelValue;
                            }
                            else if (signatureInfo.Contains("UK"))
                            {
                                meterModelNumber = NamePlateConstants.Ruby6Value;
                            }
                            else if (signatureInfo.ToUpper().Contains("WB"))
                            {
                                meterModelNumber = NamePlateConstants.WBValue;
                            }
                            else if (signatureInfo.ToUpper().Contains("BW"))
                            {
                                meterModelNumber = NamePlateConstants.WBLTValue;
                            }
                            else if (signatureInfo.ToUpper().Contains("SK"))
                            {
                                meterModelNumber = NamePlateConstants.RubyE150Value;
                            }
                            else if (signatureInfo.ToUpper().Contains("SF"))
                            {
                                meterModelNumber = NamePlateConstants.SFSP;
                            }
                            else if (signatureInfo.ToUpper().Contains("HM"))
                            {
                                meterModelNumber = NamePlateConstants.PumaHTE650MWValue;
                            }
                            //---Rohit-----------03-March-2016------- for UPCL-----TwoSeason------
                            else if (signatureInfo.Contains("sc"))
                            {
                                meterModelNumber = NamePlateConstants.TwoTOUSapphireValue;
                            }
                            else if (signatureInfo.Contains("SC"))
                            {
                                meterModelNumber = NamePlateConstants.SapphireValue;
                            }
                            //---Rohit-----------21-March-2016------- for VB---1p DLMS--No Season-No Week-----
                            //SarkarA code change start 20180122 // Add valid meter model name for model 33 "Sc"
                            else if (signatureInfo.Contains("Sc"))
                            {
                                meterModelNumber = NamePlateConstants.ThreeTOUWCMValue;
                            }
                            //SarkarA code change end 20180122
                            else if (signatureInfo.Contains("VB"))
                            {
                                meterModelNumber = NamePlateConstants.VBSPNoSeasonNoWeek;
                            }
                            //******* Meter Model Change Required Here ***********//
                            else if (signatureInfo.ToUpper().Contains("VF"))
                            {
                                meterModelNumber = NamePlateConstants.VFSPNoSeasonNoWeek;
                            }
                            else if (signatureInfo.ToUpper().Contains("TN"))
                            {
                                meterModelNumber = NamePlateConstants.TNValue;
                            }
                            else if (signatureInfo.ToUpper().Contains("FS")) // single phase smart meter
                            {
                                meterModelNumber = NamePlateConstants.SM110value;
                            }
                            //******* Smart meter 3 phase WCM  ***********//
                            else if (signatureInfo.ToUpper().Contains("FU"))
                            {
                                meterModelNumber = NamePlateConstants.Smartmeter_WCM;
                            }
                            //******* Smart meter 3 phase LTCT Falcon ***********//
                            else if (signatureInfo.ToUpper().Contains("FL"))
                            {
                                meterModelNumber = NamePlateConstants.Smartmeter_LTCT;
                            }
                            //******* Smart meter 3 phase HTCT ***********//
                            else if (signatureInfo.ToUpper().Contains("FH"))
                            {
                                meterModelNumber = NamePlateConstants.Smartmeter_HTCT;
                            }
                            //*******Sapphire 3 phase HTCT ***********//
                            else if (signatureInfo.Contains("sm"))
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_sm;
                            }
                            //******* Sapphire 3 phase HTCT ***********//
                            else if (signatureInfo.Contains("SM"))
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_SM;
                            }

                            //*******Sapphire 3 phase HTCT ***********//
                            else if (signatureInfo.Contains("sh"))
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_sh;
                            }

                            //******* Sapphire 3 phase HTCT ***********//
                            else if (signatureInfo.Contains("SH"))
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_SH;
                            }
                            //******* Sapphire S2 Three phase Low cost meter ***********//
                            else if (signatureInfo.Contains("SPS201"))
                            {
                                meterModelNumber = NamePlateConstants.SapphireS2;
                            }
                            //******* Sapphire S2 Three phase Low cost meter ***********//
                            else if (signatureInfo.Contains("SPS202"))
                            {
                                byte[] Signaturebytes = Encoding.ASCII.GetBytes(signatureInfo);
                               // if (Signaturebytes[13]==0x01)//LGZ-SPS202-S01101-074.001.024C
                                    switch (Signaturebytes[20])
                                    {
                                        case 0x31: //LTCT
                                        case 0x32:
                                            if (Signaturebytes[13] == 0x31)
                                                meterModelNumber = NamePlateConstants.Sapphire_Netmeter_LTCT;
                                            else
                                                meterModelNumber = NamePlateConstants.SapphireS2;//S2 ltct need to be define
                                            break;
                                       case 0x33: //WCM
                                       case 0x34: //WCM
                                       case 0x35: //WCM
                                       case 0x39:
                                       case 0x41:
                                            if (Signaturebytes[13] == 0x31)
                                                meterModelNumber = NamePlateConstants.Sapphire_Netmeter_WCM;
                                            else
                                                meterModelNumber = NamePlateConstants.SapphireS2;                                            
                                            break;                                      
                                        default:
                                            break;
                                    }
                                if ( Signaturebytes[20] == 0x01 || Signaturebytes[20] == 0x02)
                                        //ltct sapphire s2
                                  if (Signaturebytes[20] == 0x03 || Signaturebytes[20] == 0x04)

                                            meterModelNumber = NamePlateConstants.Sapphire_Netmeter_WCM;
                                else
                                meterModelNumber = NamePlateConstants.SapphireS2;
                            }


                            //*******Vim series 2 meter ***********//
                            else if (signatureInfo.Contains("vb"))
                            {
                                meterModelNumber = NamePlateConstants.VIM_Series2;
                            }
                            else if (signatureInfo.Contains("BF"))
                            {
                                meterModelNumber = NamePlateConstants.BYPL_7Slot;
                            }
                            else if (signatureInfo.Contains("RF"))
                            {
                                meterModelNumber = NamePlateConstants.BRPL_7Slot;
                            }
                            else if (signatureInfo.Contains("CF"))
                            {
                                meterModelNumber = NamePlateConstants.BYPL_FD;
                            }
                            else if (signatureInfo.Contains("CB"))  //user story 1016689
                            {
                                meterModelNumber = NamePlateConstants.BRPL_CBSP;
                            }
                            else if (signatureInfo.Contains("Non-Landis+GyrMeter"))
                            {
                                meterModelNumber = NamePlateConstants.NonLandisMeter;
                            }
                            else if (signatureInfo.Contains("W0"))  //user story 1016689
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_Netmeter_WCM;
                            }
                            else if (signatureInfo.Contains("L0"))  //user story 1016689
                            {
                                meterModelNumber = NamePlateConstants.Sapphire_Netmeter_LTCT;
                            }

                            else
                            {
                                meterModelNumber = NamePlateConstants.InvalidModelValue;
                            }
                            if (meterModelNumber != NamePlateConstants.NonLandisMeter && meterModelNumber != 0)// Added for Non Landis gyr meters
                            {

                                selectedMeterConfigProfile = CheckMeterConfiguration(meterModelNumber.ToString(), firmwareVersion.TrimStart('0'));
                            }
                            else
                            {
                                SetGridRowAttributes(Color.LightGray, ProfileId.MeterConfiguration, "Readout Not Supported.");
                            }

                            selectedProfiles = GetSelectedProfilesToRead();

                            writeToFile.WriteLine(GetSignatureDataInFileFormat(signatureInfo));

                            // addition of sps2 meter for BD switch
                            if ((signatureInfo.ToUpper().Contains("SC") || signatureInfo.ToUpper().Contains("ST") ||
                                signatureInfo.ToUpper().Contains("st")) ||
                                (signatureInfo.ToUpper().Contains("SPS2") && signatureInfo.ToUpper().Contains("-T00101")) && (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1)
                                && (commType == CommunicationType.DIRECT))
                                communication.SetBaudRate(6);



                            #region ReadProfileAndWriteToFile
                            Application.DoEvents();
                            //NOt a good practice , but forced ro apply check as for NDPL Ruby meter with version 1.66
                            //a strange thing encountered that if BCS sends command for anomaly and then reads midnight data it returnes no data even if meter has midnight data.
                            if (firmwareVersion == "1.66")
                            {
                                selectedProfiles.Remove(ProfileId.Anomaly);
                            }
                            //MSEDCL Check for Daily Load Profile
                            if (firmwareVersion == "2.21" && meterModelNumber == NamePlateConstants.RubyE250Value)
                            {
                                if (selectedProfiles.Contains(ProfileId.Midnight))
                                {
                                    selectedProfiles.Remove(ProfileId.Midnight);
                                    SetGridRowAttributes(Color.LightGray, ProfileId.Midnight, "Readout Not Supported.");
                                }
                            }
                            if (meterModelNumber == NamePlateConstants.SM110value || meterModelNumber == NamePlateConstants.NonLandisMeter)// Added for non Landis gyr meters
                            {
                                if (selectedProfiles.Contains(ProfileId.Phasor))
                                {
                                    selectedProfiles.Remove(ProfileId.Phasor);
                                    SetGridRowAttributes(Color.LightGray, ProfileId.Phasor, "Readout Not Supported.");
                                }
                            }
                            if (selectedProfiles.Contains(ProfileId.LoadSwitch) && meterModelNumber == NamePlateConstants.NonLandisMeter)
                            {
                                selectedProfiles.Remove(ProfileId.LoadSwitch);
                                SetGridRowAttributes(Color.LightGray, ProfileId.LoadSwitch, "Readout Not Supported.");
                            }
                            //Fast Download check for PVVNL  
                            if (commMode == CommunicationMode.FastDownload && (firmwareVersion == "2.21" || firmwareVersion == "06.03"
                                || firmwareVersion == "06.09"))
                            {
                                this.StatusMessageAsync = "Fast Download mode not supported.";
                                return false;
                            }
                            foreach (ProfileId selectedProfile in selectedProfiles)
                            {
                                readSuccess = false;
                                currentProfile = selectedProfile;
                                // HTCT Specific Changes
                                if (selectedProfile == ProfileId.KvahSelection && meterModelNumber == 10)
                                {
                                    this.StatusMessageAsync = "Reading Mvah Selection";
                                }
                                else
                                {
                                    this.StatusMessageAsync = "Reading " + CommonBLL.GetEnumDescription(selectedProfile) + " Data...";
                                }


                                if (selectedMeterConfigProfile.Contains(selectedProfile))
                                {
                                    SetGridRowAttributes(Color.LightYellow, ProfileId.MeterConfiguration, "Reading Data...");
                                }
                                else if (selectedProfile == ProfileId.Anomaly)
                                {
                                    SetGridRowAttributes(Color.LightYellow, ProfileId.Instant, "Reading Data...");
                                }
                                else
                                {
                                    SetGridRowAttributes(Color.LightYellow, selectedProfile, "Reading Data...");
                                }

                                if (isRemote)
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    //Application.DoEvents();
                                }
                                //******* Meter Model Change Required Here ***********//
                                if (((meterModelNumber == NamePlateConstants.RubyE250Value) || (meterModelNumber == NamePlateConstants.RubyE150Value || meterModelNumber == NamePlateConstants.VBSPNoSeasonNoWeek || meterModelNumber == NamePlateConstants.SM110value || meterModelNumber == NamePlateConstants.SM110value) || meterModelNumber == NamePlateConstants.Smartmeter_LTCT || meterModelNumber == NamePlateConstants.Smartmeter_WCM || meterModelNumber == NamePlateConstants.SmartM_Cipher_WCM || meterModelNumber == NamePlateConstants.SmartM_Cipher_LTCT)
                                    && (commMode == CommunicationMode.FastDownload))
                                {
                                    this.StatusMessageAsync = "Fast download mode not supported.";
                                    result.ErrorCode = CommunicationErrorType.FastDownloadNotSupported;
                                    break;
                                }
                                else
                                {

                                    //Get profile commands according to communication mode saved in other settings
                                    List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, selectedProfile, meterModelNumber);

                                    if (selectedProfile == ProfileId.LoadSurvey)
                                    {
                                        int commandIndex = commMode == CommunicationMode.FastDownload ? 0 : 1;
                                        profileReadCommands[commandIndex].SelectiveAccess = true;
                                        profileReadCommands[commandIndex].ToTime = meterTime;
                                        profileReadCommands[commandIndex].FromTime = meterTime.AddDays(-(Convert.ToInt32(ConfigSettings.GetValue("LoadSurveyDays"))));
                                    }
                                    for (index = 0; index < profileReadCommands.Count; index++)
                                    {
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            profileReadCommands[index].Action = ActionType.READ;
                                            profileReadCommands[index].MeterID = meterId;
                                            result = communication.Send(profileReadCommands[index]);
                                            classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                                + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                       + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                            if (((result.RecieveDataLength <= 0) || (result.RecieveDataLength < 33)) && selectedProfile == ProfileId.Phasor && profileReadCommands[index].ClassId == 0xFE)
                                            {
                                                result.ErrorCode = CommunicationErrorType.Success;
                                            }
                                            else if ((result.RecieveDataLength <= 0) && selectedProfile == ProfileId.Tamper)
                                            {
                                                result.ErrorCode = CommunicationErrorType.Success;
                                                writeToFile.WriteLine(classIdOBISCodeAttribute + "0100");
                                                // *********** For smart meter Falcon 2 ******
                                                if (index == profileReadCommands.Count - 1)
                                                {
                                                    SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                }
                                            }
                                            else if ((result.RecieveDataLength <= 0) || (result.RecieveDataLength < 33 && selectedProfile == ProfileId.Phasor))
                                            {
                                                if (selectedProfile == ProfileId.NamePlate)
                                                {
                                                    readSuccess = true;
                                                }
                                                else
                                                {
                                                    if (readSuccess)
                                                    {
                                                        SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                        break;
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                readSuccess = true;
                                                writeToFile.Write(classIdOBISCodeAttribute);
                                                for (int i = 0; i < result.RecieveDataBuffer.Count; i++)
                                                {
                                                    writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                                }
                                                writeToFile.WriteLine("");
                                                if (index == profileReadCommands.Count - 1)
                                                {
                                                    if (selectedProfile == ProfileId.Anomaly)
                                                    {
                                                        SetGridRowAttributes(Color.LightGreen, ProfileId.Instant, "Readout Successful.");
                                                    }
                                                    else
                                                    {
                                                        SetGridRowAttributes(Color.LightGreen, selectedProfile, "Readout Successful.");
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            break;
                                        }


                                    }
                                    if (!readSuccess)
                                    {
                                        if (result.ErrorCode == CommunicationErrorType.ResponseTimeout)
                                        {
                                            SetGridRowAttributes(Color.Red, selectedProfile, "Sign On Failure/Timeout.");
                                        }
                                        else
                                        {
                                            SetGridRowAttributes(Color.LightGray, selectedProfile, "Readout Not Supported.");
                                        }
                                    }

                                    if (result.ErrorCode != CommunicationErrorType.Success)
                                    {
                                        if (result.ErrorCode == CommunicationErrorType.ResponseTimeout)
                                        {
                                            if (selectedProfile == ProfileId.Anomaly)
                                            {
                                                SetGridRowAttributes(Color.Red, ProfileId.Instant, "Sign On Failure/Timeout.");
                                            }
                                            else if (selectedMeterConfigProfile.Contains(selectedProfile))
                                            {
                                                SetGridRowAttributes(Color.Red, ProfileId.MeterConfiguration, "Sign On Failure/Timeout.");
                                            }
                                            else
                                            {
                                                SetGridRowAttributes(Color.Red, selectedProfile, "Sign On Failure/Timeout.");
                                            }
                                        }
                                        break;
                                    }
                                }
                                //Last element of meter config so make meter config row as successfull.
                                if (selectedMeterConfigProfile.Count > 0 && selectedProfile == (ProfileId)selectedMeterConfigProfile[selectedMeterConfigProfile.Count - 1])
                                {
                                    SetGridRowAttributes(Color.LightGreen, ProfileId.MeterConfiguration, "Readout Successful.");
                                }


                            }
                            #endregion

                            if ((signatureInfo.ToUpper().Contains("SC") ||
                                signatureInfo.ToUpper().Contains("ST") ||
                                signatureInfo.ToUpper().Contains("st")) &&
                                (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1) &&
                                (commType == CommunicationType.DIRECT))
                                communication.SetBaudRate(5);
                        }


                        #region ResourceClosingAndCleanup
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {

                            communication.CloseSession();
                            // Add model sps2 to set BD 9600 after disconnect
                            if ((signatureInfo.ToUpper().Contains("SPS2") && signatureInfo.ToUpper().Contains("-T00101")) &&
                               (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1) &&
                               (commType == CommunicationType.DIRECT))
                                communication.PhysicalChannelDetail.SetBaud(5);

                            writeToFile.Close();
                            initialFileStream.Close();

                            String strChecksum = GetMD5ChecksumForFile(strFileName);
                            FileStream fileStream = new FileStream(strFileName, FileMode.Append);
                            StreamWriter writeStream = new StreamWriter(fileStream);
                            writeStream.WriteLine(strChecksum);
                            writeStream.Close();
                            fileStream.Close();
                            StreamReader streamReader = new StreamReader(strFileName);
                            string fileText = streamReader.ReadToEnd();
                            streamReader.Close();
                            File.Delete(strFileName);
                            if (fileText != "")
                            {

                                if (commType == CommunicationType.DIRECT)
                                {
                                    this.StatusMessageAsync = "Readout Successful.";
                                    EnableStopTimer();
                                    DisableAbort();
                                    SaveData(fileText, data.TrimEnd());
                                    // Update for xml
                                    if (ConfigSettings.GetValue("ApplicationContext") == "03")
                                        CreateXMLFileForCC(fileText, data.TrimEnd(), signatureInfo);
                                }
                                else
                                {
                                    SaveRemoteData(fileText, data.TrimEnd());

                                }
                            }

                        }
                        else if (result.ErrorCode == CommunicationErrorType.InvalidMeterIDName)
                        {
                            communication.CloseSession();
                            this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);

                        }
                        else
                        {
                            isConnected = false;
                            writeToFile.Close();
                            initialFileStream.Close();
                            File.Delete(strFileName);
                            this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                        }
                        #endregion

                    }
                    else
                    {
                        isConnected = false;
                        this.StatusMessageAsync = ReadoutFailure;
                    }
                }
                else
                {
                    isConnected = false;
                    try
                    {
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    catch (Exception ex)    //Exception log for catch block 
                    {
                        logger.Log(LOGLEVELS.Error, "GetMeterData(string strFileName, bool isRemote, int simIndex)", ex);
                    }
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                communication.CloseSession();
                if ((signatureInfo.ToUpper().Contains("SPS2") && signatureInfo.ToUpper().Contains("-T00101")) &&
                   (Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) != 1) &&
                   (commType == CommunicationType.DIRECT))
                    communication.PhysicalChannelDetail.SetBaud(5);
            }

            return isConnected;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="?"></param>
        /// <param name="message"></param>
        private void SetGridRowAttributes(Color color, ProfileId profile, string message)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = color;
            SetDataGridStyle(profile, style);
            SetDataGridStatus(profile, message);

            if (style.BackColor == Color.Red)
            {
                style.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// gets meter config data from xml where meter model number and firmware version match.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="firmware"></param>
        /// <returns></returns>
        public MeterConfigSettingsMeterConfigElement GetMeterConfig(string meterModel, string firmware)
        {
            MeterConfigSettingsMeterConfigElement result = null;
            try
            {
             if (meterModel == "6" || meterModel == "9" || meterModel == "8" || meterModel == "10" || meterModel == "11"
                 || meterModel == "12" || meterModel == "13" || meterModel == "14" || meterModel == "18" || meterModel == "35" || meterModel == "34" || meterModel == "37" || ConfigInfo.MeterModel == ((int)NamePlateConstants.SFSP).ToString())
                {
                    firmware = "0.00";
                }
                //result = meterConfigSettings.Items[0];
                foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
                {
                    if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == firmware.ToString())
                    {
                        result = meterConfigSettingsElement;
                        break;
                    }
                }
                if (result == null)
                {
                    foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
                    {
                        if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == "0.00")
                        {
                            result = meterConfigSettingsElement;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterConfig(string meterModel, string firmware)", ex);
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateGrid()
        {
            bool isSuccess = false;

            if (dgvMeterIdAndSim != null)
            {
                for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                {
                    DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(chk1.Value) == true)
                    {
                        isSuccess = true;
                    }

                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        private void ReadOneToOne(string strFileName)
        {
            countProfile = -1;
            simNumber = txtBoxMeterSIM.Text;
            Staticip =  txtBoxMeterSIM.Text;
            Tcpport = txtPort.Text;
            SetCursor(Cursors.WaitCursor);
            string MeterID = string.Empty;
            this.StatusMessageAsync = "Reading Meter Data...";
            EnableStartTimer();
            if (commType != CommunicationType.DIRECT)
            {
                if (commType == CommunicationType.TCP)
                {
                    this.StatusMessageAsync = "Connecting TCP Modem  " + Staticip + " ...";
                }
                else
                {
                    this.StatusMessageAsync = "Connecting " + simNumber + " ...";
                }

                
            }
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            if (ConfigSettings.GetValue("PortName").Contains(","))
                channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
            else
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            if (commType == CommunicationType.TCP)
            {
                channelInfo.ModemInfo = Staticip;
                channelInfo.TcpPort = Tcpport;
            }
            else
            {
                channelInfo.ModemInfo = simNumber;
            }

            if (ConfigSettings.GetValue("ApplicationContext") == "03")
                channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter
            else
                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
             EnableAbort();
             Result result = new Result();
            if (ConfigSettings.GetValue("ApplicationContext") == "03")
                if (channelInfo.SecurityMechanism == 0x00)
                {
                    result = communication.OpenSession();
                    if (commType == CommunicationType.TCP && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        this.StatusMessageAsync = "Remote Modem Connected";
                    }
                    // ************ Read meter ID Start
                     ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x60.0x01.0x00.0xFF", 02);
                     result = communication.Send(profileCommand);
                    //communication.CloseSession();
                    if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        int index = 0;
                        
                        index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        for (int i = 2; i < index + 2; i++)
                        {
                            MeterID += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                        }
                     }
                    // ************Read meter ID Close
                   // result = communication.OpenSession();
                    // ************Read Invocation Counter start
                     profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
                    if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (ConfigSettings.GetValue("SecurityMechanism") == "01")
                            profileCommand = new ProfileCommand(01, "00.00.2B.01.02.FF", 02);
                        else if (ConfigSettings.GetValue("SecurityMechanism") == "02")
                            profileCommand = new ProfileCommand(01, "00.00.2B.01.03.FF", 02);
                    }

                    result = communication.Send(profileCommand);
                    communication.CloseSession();
                    if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        int index = 0;
                        long InvoCountValue = 0;
                        long InitializationCounter = 0;
                        byte[] incount = new byte[4];
                       
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                            communication = new Communication(channelInfo);
                            securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                            string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                            int adbyteindex = 0;
                            for (int i = 1; i < index + 5; i++)
                            {
                                incount[adbyteindex++] = result.RecieveDataBuffer[i];

                            }
                            data = Convertstring.FormatData(incount, false);
                            if (data != null) InvoCountValue = Convert.ToInt64(data);
                            InitializationCounter = InvoCountValue + 1;
                           
                            List<string> SecurityKeyDetails = Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                            //MohsinRaza : below code is added to handle dual key logic for smart meter
                        
                            if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                            {
                                ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);
                            }
                            else //-------------Set to Default if Meter ID not found in EndDeviceSecurtityResponse file---------------
                            {
                                logger.Log(LOGLEVELS.Error, "ReadOneToOne()--> Setting Security Keys To Default for meter ID :" + MeterID + " due to Null return from EndDeviceSecurityResponse");
                                ConfigSettings.ChangeNode("ModePassword", ConfigSettings.GetValue("DefaultModePassword"));
                                ConfigSettings.ChangeNode("GlobalEncryptionKey", ConfigSettings.GetValue("DefaultGlobalEncryptionKey"));
                                ConfigSettings.ChangeNode("AuthenticationKey", ConfigSettings.GetValue("DefaultAuthenticationKey"));
                            }
                       
                            result = communication.OpenSessionCipher(InitializationCounter);
                            //----------------CC Error Case handeling for retry with additional key received--------------------------
                            if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > 3 && SecurityKeyDetails[3].Trim().Length > 0)
                            {
                                ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);
                                ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[3]);
                                ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[3]);
                                result = communication.OpenSessionCipher(InitializationCounter+1);
                            }
                           
                        }
                    }

                }
            // ************Read Invocation Counter Close
            if (ConfigSettings.GetValue("ApplicationContext") != "03")

               result = communication.OpenSession();
           
            if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
            {
                SetConnectionDetail(true);
                bool isConnected = GetMeterData(strFileName, false, 0);
                if (isConnected == false)
                    communication.CloseSession();
            }
            else
            {
                if (commType == CommunicationType.DIRECT)
                {
                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
                else
                {
                    communication.CloseSession();
                    if (commType == CommunicationType.TCP)
                    {
                        this.StatusMessageAsync = "Connection " + Staticip + " failed.";
                    }
                    else
                    {
                        this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                    }

                    
                }
            }
            DisableAbort();
        }
        private bool IsChildActive(List<Thread> lstThread)
        {
            bool flag = false;
            try
            {
                foreach (Thread item in lstThread)
                {
                    if (item.IsAlive)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IsChildActive(List<Thread> lstThread)", ex);
                
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        private void ReadOneToManyParallel(string strFileName)
        {
            AbortThread(lstThread);
            lstThread = new List<Thread>();
            EnableAbort();
            SetCursor(Cursors.WaitCursor);
           string TCPPort = ConfigSettings.GetValue("TCPPORT");
            try
            {
                if (ValidateGrid())
                {
                    SetCursor(Cursors.WaitCursor);
                    this.StatusMessageAsync = " ";
                    //************************TCP/IP Log********************
                    logger.Log(LOGLEVELS.Debug, "Reading Meter Data...");
                    EnableStartTimer();
                    ResetGrid();
                    for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                    {                        
                        DataGridViewRow dgvr = dgvMeterIdAndSim.Rows[rowCount];
                        DataGridViewCheckBoxCell chk1 = dgvr.Cells["Select"] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk1.Value))
                        {
                            clsParallelReader objclsParallelReader = new clsParallelReader(rowCount, strFileName, dgvr, lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData), commMode, TCPPort, commType);
                            Thread th = new Thread(new ThreadStart(objclsParallelReader.ReadThreadOne));                            
                            th.Start();
                            
                            lstThread.Add(th);
                        }
                    }


                    //Wait MainThread till Child are executing
                    foreach (Thread th in lstThread)
                    {
                        th.Join();
                    }

                    //Wait MainThread till Child are executing
                    /*while (IsChildActive(lstThread))
                    {
                        Thread.Sleep(10000);
                    }*/

                }
                else
                {
                    if (commType == CommunicationType.GPRS)
                    {
                        MessageBox.Show("Select atleast 1 IMEI number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (commType == CommunicationType.TCP)
                    {
                        MessageBox.Show("Select atleast 1 IP to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Select atleast 1 SIM number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                //************************TCP/IP Log********************
                logger.Log(LOGLEVELS.Debug, "Reading Meter Data..." + ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadOneToManyParallel(string strFileName)", ex);
            }
            finally
            {
                //Abort thread that are still alive                
                AbortThread(lstThread);
            }
        }

        private void AbortThread(List<Thread> lstThread)
        {
            try
            {
                foreach (var item in lstThread)
                {
                    if (item != null && item.IsAlive)
                    {
                        try
                        {
                            item.Abort();
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "AbortThread(List<Thread> lstThread)", ex);
                            
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AbortThread(List<Thread> lstThread)", ex);           
            }
        }
        private void AbortReadThread(Thread th)
        {
            try
            {
                if (th != null && th.IsAlive)
                {
                    th.Abort();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "AbortThread(Thread th)", ex);
            }
        }       


        private void ReadOneToMany()
        {
            Result result = new Result();
            bool isConnected = false;
            String strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");
            countProfile = -1;
            if (ValidateGrid())
            {
                SetCursor(Cursors.WaitCursor);
                this.StatusMessageAsync = "Reading Meter Data...";
                EnableStartTimer();
                ResetGrid();
                byte totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                //************** Insert all GSM read meters first then update its status below ***************
                for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                {
                    DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                    bool DBStatus = false;
                    simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();

                    MeterSerialNumber = dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, rowCount].Value.ToString();
                    if (Convert.ToBoolean(chk1.Value))
                    {
                        DBStatus = new GSMConfigBLL().InsertReadData(Convert.ToInt32(MeterSerialNumber), simNumber, "Pending...", "Reading Not start", Taskname);
                    }
                }

                for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                {
                    for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                    {
                        countProfile = rowCount;
                        DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                        bool ReadStatus = false;
                        if (Convert.ToBoolean(chk1.Value))
                        {
                            simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                            MeterSerialNumber = dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, rowCount].Value.ToString();
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                : "Connecting " + simNumber + " ...";
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                            this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                            Application.DoEvents();

                            ChannelInformation channelInfo = new ChannelInformation();
                            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                            if (ConfigSettings.GetValue("PortName").Contains(","))
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
                            else
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                            channelInfo.ModemInfo = simNumber;
                            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                            channelInfo.NoOfRetries = totalRetries;
                            channelInfo.TcpPort = ConfigSettings.GetValue("TCPPORT");// TCP PORT
                            communication = new Communication(channelInfo);
                            EnableAbort();
                            result = communication.OpenSession();
                            SetConnectionDetail(true);
                            if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                            {
                                isConnected = GetMeterData(strFileName, true, rowCount);
                                DisableAbort();
                                if (isConnected)
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout completed.";
                                    this.StatusMessageAsync = "Readout completed.";
                                    dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;
                                    logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "" + simNumber + " Readout completed.");
                                    ReadStatus = new GSMConfigBLL().UpdateReadStatus(MeterSerialNumber, "Pass", "Readout completed", Taskname);
                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout failed.";
                                    this.StatusMessageAsync = "Readout failed.";
                                    logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "" + simNumber + " Readout failed.");
                                    ReadStatus = new GSMConfigBLL().UpdateReadStatus(MeterSerialNumber, "Fail", "Readout failed.", Taskname);

                                }
                                communication.CloseSession();
                            }
                            else
                            {
                                communication.CloseSession();
                                DisableAbort();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.";
                                this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                logger.Log(LOGLEVELS.Info, dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "" + simNumber + " Connection failed.");
                                ReadStatus = new GSMConfigBLL().UpdateReadStatus(MeterSerialNumber, "Fail", "Connection failed.", Taskname);
                            }

                        }
                    }
                }
            }
            else
            {
                if (commType == CommunicationType.GPRS)
                {
                    MessageBox.Show("Select atleast 1 IMEI number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Select atleast 1 SIM number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        /// <summary>
        /// Read meter in a thread ,takes state as a parameter which will 
        /// contain information required for thread to process
        /// </summary>
        /// <param name="state"></param>
        private void ReadMeter(object state)
        {
            try
            {
                isMeterReading = true;

                String strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");
                if (!Directory.Exists(strFileName))
                {
                    Directory.CreateDirectory(strFileName);
                }
                //SetCursor(Cursors.WaitCursor);
                //this.StatusMessageAsync = "Reading Meter Data...";
                //EnableStartTimer();
                if (oneToOneGSM.Checked || commType == CommunicationType.DIRECT)
                {
                    ReadOneToOne(strFileName);
                }
                else if (oneToManyGSM.Checked)
                {
                    if (commType == CommunicationType.TCP)
                    {
                        CheckAndStopService();
                        if (Convert.ToBoolean(ConfigSettings.GetValue("IsTCPOneToManyParallel")))
                        {
                            ReadOneToManyParallel(strFileName);
                        }
                        else
                        {
                           // ReadOneToMany(strFileName);
                            ReadOneToMany();
                        }
                    }
                    else
                    {
                       ReadThread = new Thread(ReadOneToMany);
                        ReadThread.Start();
                        ;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                this.StatusMessageAsync = ReadoutFailure;
                logger.Log(LOGLEVELS.Error, "Error while Reading PUMA Meter.", ex);
                logger.Log(LOGLEVELS.Error, "ReadMeter(object state)", ex);

            }
            finally
            {
                if (communication.Serial != null)
                {
                    communication.Serial.Close();
                }
                isMeterReading = false;
                EnableReadControls();
                SetConnectionDetail(false);
                SetCursor(Cursors.Default);
                CheckAndStartService();
                EnableStopTimer();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void EnableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(EnableAbort));
            }
            else
            {
                btnAbort.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        private void SetCursor(Cursor cursor)
        {
            if (this.InvokeRequired)
            {
                SetCursorCallBack callBack = new SetCursorCallBack(SetCursor);
                Invoke(callBack, cursor);
            }
            else
            {
                this.Cursor = cursor;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="style"></param>
        private void SetDataGridStyle(System.Enum profileId, DataGridViewCellStyle style)
        {
            if (this.InvokeRequired)
            {
                SetDataGridStyleCallback callBack = new SetDataGridStyleCallback(SetDataGridStyle);
                Invoke(callBack, profileId, style);
            }
            else
            {
                lngGridViewReadControl1.SetColour(profileId, style);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="message"></param>
        private void SetDataGridStatus(System.Enum profileId, string message)
        {
            if (this.InvokeRequired)
            {
                SetDataGridStatusCallback callBack = new SetDataGridStatusCallback(SetDataGridStatus);
                Invoke(callBack, profileId, message);
            }
            else
            {
                lngGridViewReadControl1.SetStatus(profileId, message);
            }
        }

        /// <summary>
        /// Sets the status of MeterID and SIM/IMEI grid
        /// </summary>
        private void SetStatus(DataGridViewCell dataCell, string msg)
        {
            foreach (DataGridViewRow gridViewRow in dgvMeterIdAndSim.Rows)
            {
                if (Convert.ToBoolean(gridViewRow.Cells["Select"].Value))
                {
                    gridViewRow.Cells["Status"].Value = msg;
                }
            }
        }

        /// <summary>
        /// Sets the style of MeterID and SIM/IMEI grid
        /// </summary>
        private void SetStyle(DataGridViewRow dataRow, DataGridViewCellStyle style)
        {
            foreach (DataGridViewRow gridViewRow in dgvMeterIdAndSim.Rows)
            {
                if (Convert.ToBoolean(gridViewRow.Cells["Select"].Value))
                {
                    gridViewRow.Cells["Status"].Style = style;
                }
            }
        }



        /// <summary>
        /// Validate SIM Number 
        /// </summary>
        /// <returns></returns>
        private bool ValidateSimNumber()
        {
            bool flag = true;
            long simNumber = 0;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            if (commType == CommunicationType.GPRS)
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 15)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    return false;
                }
            }
            else
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 10)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    return false;
                }
            }
            return flag;

        }

        /// <summary>
        ///  Used to create profileId enums based on profiles
        /// that needs to be read(Selected by user through checkboxes)
        /// </summary>
        /// <returns></returns>
        private List<System.Enum> GetSelectedProfilesToRead()
        {
            List<System.Enum> selectedProfiles = new List<System.Enum>();
            selectedProfiles.Clear();
            #region Mandatory Profiles/Configuration
            selectedProfiles.Add(ProfileId.NamePlate);
            selectedProfiles.Add(ProfileId.NamePlateProfile);
            selectedProfiles.AddRange(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData));
                       
            if (selectedProfiles.Contains(ProfileId.Instant))
            {
                selectedProfiles.Insert(selectedProfiles.IndexOf(ProfileId.NamePlate) + 1, ProfileId.Anomaly);
            }
            if (selectedProfiles.Contains(ProfileId.MeterConfiguration))
            {
                if (selectedMeterConfigProfile.Count != 0)
                {
                    // Keeping Load Survey as the last parameter to be read
                    if (selectedProfiles.Contains(ProfileId.LoadSurvey))
                    {
                        selectedProfiles.InsertRange(selectedProfiles.IndexOf(ProfileId.LoadSurvey) - 1, selectedMeterConfigProfile);
                    }
                    else
                    {
                        selectedProfiles.AddRange(selectedMeterConfigProfile);
                    }
                }
                selectedProfiles.Remove(ProfileId.MeterConfiguration);
            }
            return selectedProfiles;
        }
        /// <summary>
        /// checks what parameters to show for meter configuration
        /// </summary>
        /// <param name="meterModelNumber"></param>
        /// <param name="firmwareVersion"></param>
        /// <returns></returns>
        private List<System.Enum> CheckMeterConfiguration(string meterModelNumber, string firmwareVersion)
        {

            List<System.Enum> selectedProfiles = new List<System.Enum>();
            MeterConfigSettingsMeterConfigElement element = GetMeterConfig(meterModelNumber, firmwareVersion);
            if (element != null)
            {

                if (Convert.ToBoolean(element.DIP))
                {
                    selectedProfiles.Add(ProfileId.DIP);
                }
                if (Convert.ToBoolean(element.KvahSelection))
                {
                    selectedProfiles.Add(ProfileId.KvahSelection);
                }
                if (Convert.ToBoolean(element.DisplayParameters))
                {
                    selectedProfiles.Add(ProfileId.PushDisplayParameter);
                    selectedProfiles.Add(ProfileId.ScrollDisplyParameter);
                    selectedProfiles.Add(ProfileId.HighResolutionDisplayParameter);
                    //selectedProfiles.Add(ProfileId.DisplayTimeoutParameter); // Story - Hide Display Timeout Parameter
                }
                if (element.TOD.ToString().ToUpper() == "ONE"
                    || element.TOD.ToString().ToUpper() == "TWO"
                    || element.TOD.ToString().ToUpper() == "FOUR"
                    || element.TOD.ToString().ToUpper() == "FOURSP"
                    || element.TOD.ToString().ToUpper() == "THREE")     //SarkarA code change 20180223 // added 3TOU
                {
                    selectedProfiles.Add(ProfileId.ActiveDayProfile);
                    selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                    selectedProfiles.Add(ProfileId.ActiveSeasonProfile);

                    selectedProfiles.Add(ProfileId.PassiveDayProfile);
                    selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                    selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                    selectedProfiles.Add(ProfileId.ActivationDate);
                }

                if (element.TOD.ToString().ToUpper() == "FOURSP10Z8S")
                {
                    selectedProfiles.Add(ProfileId.ActiveDayProfile);
                    selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                    selectedProfiles.Add(ProfileId.ActiveSeasonProfile);

                    selectedProfiles.Add(ProfileId.PassiveDayProfile);
                    selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                    selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                    selectedProfiles.Add(ProfileId.ActivationDate);
                    selectedProfiles.Add(ProfileId.SpecialDayProfileSmartMeter);
                }


                if (Convert.ToBoolean(element.RTC))
                {
                    selectedProfiles.Add(ProfileId.RTC);
                }
                // [BillingType_Month]
                if (element.BillingType.ToString().ToUpper() == "NORMAL")
                {
                    selectedProfiles.Add(ProfileId.BillingType);
                }
                else if (element.BillingType.ToString().ToUpper() == "OTHER")
                {
                    selectedProfiles.Add(ProfileId.BillingType);
                    selectedProfiles.Add(ProfileId.BillingMonthType);
                }
                if (Convert.ToBoolean(element.AutoLock))
                {
                    selectedProfiles.Add(ProfileId.AutoLock);
                }
                if (Convert.ToBoolean(element.ManualBilling))
                {
                    selectedProfiles.Add(ProfileId.ManualBilling);
                }
                if (Convert.ToBoolean(element.SoftwareBilling))
                {
                    selectedProfiles.Add(ProfileId.SoftwareBilling);
                }
                if (Convert.ToBoolean(element.LockRS232))
                {
                    selectedProfiles.Add(ProfileId.RS232LockUnlock);
                }
                if (Convert.ToBoolean(element.SIP))
                {
                    selectedProfiles.Add(ProfileId.SIP);
                }
                if (Convert.ToBoolean(element.CTRatio))
                {
                    selectedProfiles.Add(ProfileId.CTRatio);
                }
                if (Convert.ToBoolean(element.PTRatio))
                {
                    selectedProfiles.Add(ProfileId.PTRatio);
                }
                if (Convert.ToBoolean(element.DIPWithSliding))
                {
                    selectedProfiles.Add(ProfileId.DIPWithSliding);
                }
                if (Convert.ToBoolean(element.DisconnectControl))
                {
                    selectedProfiles.Add(ProfileId.DisconnectControl);
                }
                if (Convert.ToBoolean(element.LoadControl))
                {
                    selectedProfiles.Add(ProfileId.LoadControl);
                }
                if (Convert.ToBoolean(element.LoadControl1PSmartMeter))
                {
                    selectedProfiles.Add(ProfileId.LoadControl1PSmartMeter);
                }
                if (Convert.ToBoolean(element.RS485))
                {
                    selectedProfiles.Add(ProfileId.RS485);
                }
                if (Convert.ToBoolean(element.PaymentMode))
                {
                    selectedProfiles.Add(ProfileId.Paymentmode);
                }
                if (Convert.ToBoolean(element.Meteringmode))
                {
                    selectedProfiles.Add(ProfileId.Meteringmode);
                }
                if (Convert.ToBoolean(element.LoadLimit))
                {
                    selectedProfiles.Add(ProfileId.LoadLimit);
                }
                if (Convert.ToBoolean(element.SlidingDemand))
                {
                    selectedProfiles.Add(ProfileId.Slidingdemand);
                }
                if (Convert.ToBoolean(element.OpticalLockUnlock))
                {
                    selectedProfiles.Add(ProfileId.OpticalLockUnlock);
                }
                if (Convert.ToBoolean(element.RJLockUnlock))
                {
                    selectedProfiles.Add(ProfileId.RJLockUnlock);
                }
                if (Convert.ToBoolean(element.PulseEnergy))
                {
                    selectedProfiles.Add(ProfileId.PulseEnergy);
                }
                if (Convert.ToBoolean(element.ManualButtonMDReset))
                {
                    selectedProfiles.Add(ProfileId.ManualButtonMDReset);
                }

            }
            if (selectedProfiles.Contains(ProfileId.Instant))
            {
                selectedProfiles.Add(ProfileId.Anomaly);               
            }         
            return selectedProfiles;
        }
        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }
        /// <summary>
        /// Creates and returns MD5 CheckSum
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetMD5ChecksumForFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(fileName))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", fileName));

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] hashValue = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                StringBuilder checkSum = new StringBuilder();
                foreach (byte hasByte in hashValue)
                {
                    checkSum.Append(hasByte.ToString("X2"));
                }
                return checkSum.ToString().ToUpper();
            }
        }

        private void FillMeterIdSerialNumber()
        {
            noMeterFoundStatus.Visible = false;
            DataSet dsMeterIdSimNumber = meterMasterBLL.GetMeterIdAndSimNumber(this.commType.ToString());
            if (dsMeterIdSimNumber != null && dsMeterIdSimNumber.Tables != null && dsMeterIdSimNumber.Tables.Count > 0)
            {
                dgvMeterIdAndSim.AutoGenerateColumns = true;
                //dgvMeterIdAndSim.DataSource = dsMeterIdSimNumber.Tables[0];

                DataGridViewColumn dgvSnoColumn = new DataGridViewTextBoxColumn();
                dgvSnoColumn.Name = "SNo";
                dgvSnoColumn.HeaderText = "S.No";
                dgvSnoColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("Sno"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvSnoColumn);
                }

                DataGridViewColumn dgvSimNoColumn = new DataGridViewTextBoxColumn();
                dgvSimNoColumn.Name = "SimNo";
                if (commType == CommunicationType.GPRS)
                {
                    dgvSimNoColumn.HeaderText = "IMEI Number";
                }
                else if (commType == CommunicationType.TCP)
                {
                    dgvSimNoColumn.HeaderText = "SIM IP Number";
                }
                else
                {
                    dgvSimNoColumn.HeaderText = "Sim Number";
                }
                dgvSimNoColumn.MinimumWidth = 100;
                dgvSimNoColumn.Width = 100;
                dgvSimNoColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("SimNo"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvSimNoColumn);
                }


                DataGridViewColumn dgvMeterIDColumn = new DataGridViewTextBoxColumn();
                dgvMeterIDColumn.Name = "MeterID";
                dgvMeterIDColumn.HeaderText = "Meter ID";
                dgvMeterIDColumn.ReadOnly = true;


                if (!dgvMeterIdAndSim.Columns.Contains("MeterID"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvMeterIDColumn);
                }


                DataGridViewColumn dgvColumn = new DataGridViewCheckBoxColumn();
                dgvColumn.Name = "Select";
                dgvColumn.HeaderText = "Select";

                if (!dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvColumn);
                }

                DataGridViewColumn dgvStatusColumn = new DataGridViewTextBoxColumn();
                dgvStatusColumn.Name = "Status";
                dgvStatusColumn.HeaderText = "Meter Read Status";
                dgvStatusColumn.MinimumWidth = 200;
                dgvStatusColumn.Width = 200;
                dgvStatusColumn.ReadOnly = true;

                if (!dgvMeterIdAndSim.Columns.Contains("Status"))
                {
                    dgvMeterIdAndSim.Columns.Insert(dgvMeterIdAndSim.Columns.Count, dgvStatusColumn);
                }


                for (int count = 0; count < dsMeterIdSimNumber.Tables[0].Rows.Count; count++)
                {
                    dgvMeterIdAndSim.Rows.Add();

                    dgvMeterIdAndSim.Rows[count].Cells["Sno"].Value = count + 1;
                    dgvMeterIdAndSim.Rows[count].Cells["SimNo"].Value = dsMeterIdSimNumber.Tables[0].Rows[count][1].ToString();
                    dgvMeterIdAndSim.Rows[count].Cells["MeterID"].Value = dsMeterIdSimNumber.Tables[0].Rows[count][0].ToString();
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started";
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }


                //dgvMeterIdAndSim.Columns["Meter Id"].ReadOnly = true;
                //dgvMeterIdAndSim.Columns["Status"].ReadOnly = true;


                //dgvMeterIdAndSim.Columns["Meter Id"].Width = dgvMeterIdAndSim.Columns["Meter Id"].Width + 20;
                //dgvMeterIdAndSim.Columns["Select"].Width = dgvMeterIdAndSim.Columns["Select"].Width - 40;


                dgvMeterIdAndSim.AllowUserToAddRows = false;
            }
            else
            {

                //dgvMeterIdAndSim.Visible = false;
                if (dgvMeterIdAndSim.Columns.Contains("Select"))
                {
                    dgvMeterIdAndSim.Columns.Remove("Select");
                }
                dgvMeterIdAndSim.DataSource = null;
                selectAll.Visible = false;
                noMeterFoundStatus.Visible = true;
                noMeterFoundStatus.Text = "No meter is assigned for " + ConfigSettings.GetValue("ChannelType") + " Connection.";
            }


        }
        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
        /// <summary>
        /// Change grid status while click on read button .
        /// </summary>
        private void ResetGrid()
        {


            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {
                DataGridViewCheckBoxCell selectedCheckBox = dgvMeterIdAndSim.Rows[count].Cells["Select"] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(selectedCheckBox.Value))
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = System.Drawing.Color.LightGray;
                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style = style;
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Enqueue";
                }
                else
                {
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = System.Drawing.Color.White;
                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, count].Style = style;
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber)
        {
            List<ProfileCommand> profileReadCommands = null;
            if (commMode == CommunicationMode.Normal)
            {
                //find normal commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    return profileCommandEntity.TagNumber == (int)selectedProfile
                    && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    profileCommandEntity.MeterModelNumber == 0);
                    if (profileCommandEntity.TagNumber == 147)
                    { int ui = 10; }
                });
            }
            else
            {
                //find fast download commands
                profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                {
                    if (profileCommandEntity.TagNumber == 3)
                    {
                        return (profileCommandEntity.TagNumber == (int)selectedProfile)
                         && (profileCommandEntity.ClassId == 0xFD)
                         && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                         profileCommandEntity.MeterModelNumber == 0);
                    }
                    //else if (profileCommandEntity.TagNumber == 4 && meterModelNumber == 2)
                    //{
                    //    return (profileCommandEntity.TagNumber == (int)selectedProfile)
                    //     && (profileCommandEntity.ClassId == 0xFD)
                    //     && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                    //     profileCommandEntity.MeterModelNumber == 0);
                    //}
                    else
                    {
                        return (profileCommandEntity.TagNumber == (int)selectedProfile)
                        && (profileCommandEntity.ClassId != 0xFF) && (profileCommandEntity.ClassId != 0xFD)
                        && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                        profileCommandEntity.MeterModelNumber == 0);
                    }
                });

            }
            return profileReadCommands;
        }
            #endregion

        public List<string> GetSecurityKeys(string meterid)
        {

            string source = string.Empty;
            string result = string.Empty;
            string errorcode = string.Empty;
            int rowindex = 0;
            XmlDocument doc = new XmlDocument();
            List<string> SecurityKeyDetails = new List<string>();
            //string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath))+"\\"+"CABApplication.exe.config";
            try
            {
                //  string path = AppDomain.CurrentDomain.BaseDirectory + "\\XML\\EndDeviceSecurityResponse.xml";
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\EndDeviceSecurityResponse.xml";

                doc.Load(path);

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "ResponseMessage")
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.Name == "Header")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Source")
                                    {
                                        source = node2.InnerText;

                                    }

                                }
                            }
                            if (node1.Name == "Reply")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Result")
                                    {
                                        result = node2.InnerText;

                                    }
                                    if (node2.Name == "Error")
                                    {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            if (node3.Name == "code")
                                            {

                                                errorcode = node2.InnerText;
                                            }

                                        }


                                    }

                                }
                            }
                        }
                }
                if (source == "command center" && result != "FAILED" && errorcode == "0.0")
                {
                    var doc2 = XDocument.Load(path);

                    var itemsList = (from c in doc2.Descendants("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity")
                                     select new
                                     {
                                         // item = query1.ElementAt(0),
                                         meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                         llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),
                                     }).ToList();

                    int itemindex = 0;
                    foreach (var item in itemsList)
                    {
                        if (itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value == meterid)
                        {
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                             break;
                        }

                        itemindex++;
                    }
                    return SecurityKeyDetails;


                }
                return null;

            }
            catch (UnauthorizedAccessException ex)    //Exception log for catch block
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
                logger.Log(LOGLEVELS.Error, "GetSecurityKeys(string meterid)", ex);
                return null;
            }

        }




        #endregion

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
