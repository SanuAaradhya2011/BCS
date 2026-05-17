#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.Mapper;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.WrapperLayer;
using CAB.IECChannel.ReadOut;
using System.Threading;
using System.Drawing;
using System.Data;
using CAB.BLL;
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
        private bool  isPhasorRuning = false;
        private bool isPhasorStopped = false;
        private DataSet phasorDataForGrid = null;
        private PhasorEntity phasorDataForDiagram = null;
        private const string Splitter = "$";
        private volatile byte noOfDays = 0;
        private bool isMeterReading = false;
        private bool isAborted = false;
        private const string ReadoutFailure = "Readout Failure.";
        Thread readThread;
       
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
            //communication = new Communication();

            communication = new Communication(ConfigSettings.GetValue("PortName"),
                                              Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")),
                                              ConfigSettings.GetValue("ModePassword"));
            entityGenerator = new GenerateEntity();

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
            BindLoadSurveyDays();
            this.rdbAllData_CheckedChanged(this, null);                              
            
        }             

        /// <summary>
        /// Read selected profile data from meter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
           // progressBar.Maximum = GetSelectedProfilesToRead().Count;
           // progressBar.Visible = true;
           // progressBar.Step = 1;
            btnRead.Enabled = false;
            btnAbort.Enabled = true;
            btnCancel.Enabled = false;
            btn_ReadReverseEnergy.Enabled = false;
            btnReadPhasor.Enabled = false;
            if (chkLoadSurvey.Checked)
            {
                noOfDays = Convert.ToByte(cmbLoadSurveyDays.Text);
            }
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            menuStrip.Items["dataAcquisitionToolStripMenuItem"].Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            readThread = new Thread(ReadAsync);
            //making the thread background as it communication needs to be stoppped when BCS closes.
            readThread.IsBackground = true;
            readThread.Start(SynchronizationContext.Current);
            this.Cursor = Cursors.Default;
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
            //chkGeneral.Checked = false;
            chkTamper.Checked = false;           
            chkPhasor.Checked = false;
            chkFraudEnergy.Checked = false;
            chkMidnight.Checked = false;
            btnAbort.Enabled = false;
            btnRead.Enabled = false;
            btnCancel.Enabled = false;
            chkLoadSurvey.Checked = false;
            chkMeterConfigurations.Checked = false;
            grpLoadSurvey.Enabled = false;


            //cmbLoadSurveyDays.Items.Clear();
            cmbLoadSurveyDays.SelectedIndex = -1;
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
            chkLoadSurvey.Checked = true;
            chkGeneral.Checked = true;
            chkTamper.Checked = true;            
            chkPhasor.Checked = true;
            chkFraudEnergy.Checked = true;
            chkMidnight.Checked = true;
            chkMeterConfigurations.Checked = true;
            grpLoadSurvey.Enabled = true;


            btnAbort.Enabled = false;
            btnRead.Enabled = true;
            btnCancel.Enabled = false;
            cmbLoadSurveyDays.SelectedIndex = 29;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGeneral_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllData.CheckedChanged -= rdbAllData_CheckedChanged;
            rdbPartialData.CheckedChanged -= rdbPartialData_CheckedChanged;

            if (chkMeterConfigurations.Checked && chkGeneral.Checked && chkFraudEnergy.Checked && chkLoadSurvey.Checked
                && chkTamper.Checked  && chkPhasor.Checked && chkMidnight.Checked)
            {               
                rdbAllData.Checked = true;
            }
            else
            {
                rdbPartialData.Checked = true;
                
            }
            if ((chkMeterConfigurations.Checked || chkGeneral.Checked || chkFraudEnergy.Checked || chkLoadSurvey.Checked
                || chkTamper.Checked  || chkPhasor.Checked || chkMidnight.Checked))
            {
                btnRead.Enabled = true;
                btnCancel.Enabled = true;
            }
            else
            {
                btnRead.Enabled = false;
                btnCancel.Enabled = false;
            }

            if (chkLoadSurvey.Checked)
            {
                btnAbort.Enabled = btnCancel.Enabled = false;
                grpLoadSurvey.Enabled = true;
                cmbLoadSurveyDays.SelectedIndex = 29;
            }
            else
            {
                //btnAbort.Enabled = btnRead.Enabled = true;
                grpLoadSurvey.Enabled = false;
                cmbLoadSurveyDays.SelectedIndex = -1;
            }

            rdbAllData.CheckedChanged += rdbAllData_CheckedChanged;
            rdbPartialData.CheckedChanged += rdbPartialData_CheckedChanged;
        }

        /// <summary>
        /// Close Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }
        /// <summary>
        /// Red phasor data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadPhasor_Click(object sender, EventArgs e)
        {
            isPhasorStopped = false;
            isPhasorRuning = true;            
            btnReadPhasor.Enabled = false;
           // btnStopPhasor.Enabled = true;
            btnCancelPhasor.Enabled = false;
            btnCancelFraudEnergy.Enabled = false;
            btn_ReadReverseEnergy.Enabled = false;
            btnRead.Enabled = false;
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            menuStrip.Items["dataAcquisitionToolStripMenuItem"].Enabled = false;
            Thread readThread = new Thread(GeneratePhasor);
            readThread.Start(SynchronizationContext.Current);

        }
        /// <summary>
        /// HOld phasor data read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopPhasor_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "User Aborted.";                      
            btnStopPhasor.Enabled = false;
            btnCancelPhasor.Enabled = true;
            this.Cursor = Cursors.Default;
            isPhasorStopped = true;
            isPhasorRuning = false;
            Application.DoEvents();
            
        }
        /// <summary>
        /// Colse current window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelPhasor_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Read fraud energy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReadReverseEnergy_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            btnCancelFraudEnergy.Enabled = false;
            string downloadedData = string.Empty;
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            FraudEnergy mapperFraudEnergy = new FraudEnergy();
            try
            {
                isMeterReading = true;
                btn_ReadReverseEnergy.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                Result result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        this.StatusMessage = MessageConstant.GetText("M000063");
                        Application.DoEvents();
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            string idLength = result.RecieveDataBuffer[1].ToString("00");
                            int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);
                            lstProfileCommands = GetProfileCommandEntity();
                            List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                            {
                                return profileCommandEntity.TagNumber == (byte)ProfileId.FraudEnergy
                                && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                                profileCommandEntity.MeterModelNumber == 0);
                            });

                            profileReadCommands[0].Action = ActionType.READ;
                            profileReadCommands[0].MeterID = meterId;

                            try
                            {

                                result = communication.Send(profileReadCommands[0]);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    resultData.Append(String.Format("{0:X2}", profileReadCommands[0].ClassId)
                                           + profileReadCommands[0].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                  + String.Format("{0:X2}", profileReadCommands[0].Attribute));
                                    for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                    {
                                        resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                    }

                                    List<ProfileData> fraudEnergyData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), true);
                                    List<FraudEnergyEntity> farudenergyEntity = mapperFraudEnergy.GetData(fraudEnergyData);
                                    Application.DoEvents();
                                    FillFraudEnergyData(farudenergyEntity[0]);
                                    this.StatusMessage = MessageConstant.GetText("M000068");
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Error in Reading Fraud Energy.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {                            
                            this.StatusMessageAsync = ReadoutFailure;                            
                            Application.DoEvents();
                            //MessageBox.Show("Invalid Password", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        this.StatusMessageAsync = ReadoutFailure;
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.StatusMessageAsync = ReadoutFailure;
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode),"BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Reading Fraud Energy", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                btnCancelFraudEnergy.Enabled = true;
                isMeterReading = false;
                communication.CloseSession();
                this.Cursor = Cursors.Default;
                btn_ReadReverseEnergy.Enabled = true;
                Application.DoEvents();
            }

        }
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelFraudEnergy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Abort Redaout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAbort_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnAbort.Enabled = false;
            Application.DoEvents();
            isAborted = true;            
            this.StatusMessageAsync = "Aborting...";            
            Application.DoEvents();
            Thread.Sleep(1000);            
           // btnRead.Enabled = true;
            //this.StatusMessage = "User Aborted.";
            Application.DoEvents();            
            this.Cursor = Cursors.Default;
            btnCancel.Enabled = true;            
            
        }
        /// <summary>
        /// close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterDataReadout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMeterReading || isPhasorRuning)
            {
                e.Cancel=true;
            }
            else
            {
                MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;

                if (menuStrip.InvokeRequired)
                {
                    menuStrip.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    menuStrip.Items["dataAcquisitionToolStripMenuItem"].Enabled = true;
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
        #endregion

        #region Private Methods
        /// <summary>
        /// To make sure that file upload window will same as IEC.
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveData(string fileText)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
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
                this.StatusMessageAsync = MessageConstant.GetText("M000048");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "BCS");
            }

            bool IsUploaded = false;
            UploadFile uploadFile = new UploadFile();
            this.StatusMessageAsync = "Uploading readout file..";
           // btnAbort.Enabled = false;
            uploadFile.cmriID = "BCS";            
            IsUploaded = uploadFile.Upload2NGFile(filePath);               
            uploadFile.DeleteFile();
            if (IsUploaded)
            {
                //this.OnListRefresh += new IsListRefresh();

                this.ListRefreshAsync = true;
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = "File Uploaded successfully.";
                //Application.DoEvents();
            }
            else
            {
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = uploadFile.StatusMessageAsync;
            }
           
        }

        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableStopPhasorControl()
        {
            if (btnStopPhasor.InvokeRequired)
            {
                btnStopPhasor.Invoke(new MethodInvoker(EnableStopPhasorControl));
            }
            else
            {
                btnStopPhasor.Enabled = true;
            }
        }
        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableStartPhasorControl()
        {
            if (btnReadPhasor.InvokeRequired)
            {
                btnReadPhasor.Invoke(new MethodInvoker(EnableStartPhasorControl));
            }
            else
            {
                btnReadPhasor.Enabled = true;
            }
        }
        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableReadControls()
        {
            if (btnStopPhasor.InvokeRequired)
            {
                btnStopPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnStopPhasor.Enabled = false;
            }
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
            if (btnReadPhasor.InvokeRequired)
            {
                btnReadPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnReadPhasor.Enabled = true;
            }
            if (btn_ReadReverseEnergy.InvokeRequired)
            {
                btn_ReadReverseEnergy.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btn_ReadReverseEnergy.Enabled = true; 
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
                    menuStrip.Items["dataAcquisitionToolStripMenuItem"].Enabled = true;
                }
            }
            if (btnCancelFraudEnergy.InvokeRequired)
            {
                btnCancelFraudEnergy.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnCancelFraudEnergy.Enabled = false;
            }     

        }
        /// <summary>
        /// Clear phasor controls befor readout.
        /// </summary>
        private void ClearPhasorControls()
        {
            if (lngPgrid.InvokeRequired)
            {
                lngPgrid.Invoke(new MethodInvoker(ClearPhasorControls));
            }
            else
            {
                lngPgrid.Data = null;
            }
            if (lngPhasorDiagram.InvokeRequired)
            {
                lngPhasorDiagram.Invoke(new MethodInvoker(ClearPhasorControls));
            }
            else
            {
                lngPhasorDiagram.PhasorData = null;
                lngPhasorDiagram.Refresh();
                lngPhasorDiagram.Show();
            }   
        }
        /// <summary>
        /// Update phasor diagram in thread.
        /// </summary>
        private void UpdatePhasorDiagram()
        {
            
            if (lngPhasorDiagram.InvokeRequired)
            {
                lngPhasorDiagram.Invoke(new MethodInvoker(UpdatePhasorDiagram));
            }
            else
            {
                lngPhasorDiagram.PhasorData = phasorDataForDiagram;
                lngPhasorDiagram.Refresh();
                lngPhasorDiagram.Show();
            }   
        }
        /// <summary>
        /// Update phasor grid in thread.
        /// </summary>
        private void UpdatePhasorGrid()
        {

            if (lngPgrid.InvokeRequired)
            {
                lngPgrid.Invoke(new MethodInvoker(UpdatePhasorGrid));
            }
            else
            {
                lngPgrid.Data = phasorDataForGrid;
                lngPgrid.SetWidth("Parameter", 180);
                lngPgrid.SetWidth("Value", 85);
                lngPgrid.IsSorting = false;          
            }
        }
        /// <summary>
        /// Update label to show incorrect phase sequence .
        /// </summary>
        private void PhaseSequenceIncorrectMesage()
        {
            if (lblPhasorData.InvokeRequired)
            {
                lblPhasorData.Invoke(new MethodInvoker(PhaseSequenceIncorrectMesage));
            }
            else
            {
                lblPhasorData.Text = "Phase sequence is not correct. Phasor can not be drawn.";
                lblPhasorData.Visible = true;                
            }
            if (lngPhasorDiagram.InvokeRequired)
            {
                lngPhasorDiagram.Invoke(new MethodInvoker(PhaseSequenceIncorrectMesage));
            }
            else
            {                
                lngPhasorDiagram.Visible = false;
            }
        }
        ///// <summary>
        ///// Perform the step on progress bar on UI thread
        ///// </summary>
        //private void ProgressStep()
        //{
        //    if (progressBar.InvokeRequired)
        //    {
        //        progressBar.Invoke(new MethodInvoker(ProgressStep));
        //    }
        //    else
        //    {
        //        progressBar.PerformStep();
        //        Application.DoEvents();
        //    }
        //}
        ///// <summary>
        ///// Hides the progress bar on UI thread
        ///// </summary>
        //private void HideProgressBar()
        //{
        //    if (progressBar.InvokeRequired)
        //    {
        //        progressBar.Invoke(new MethodInvoker(HideProgressBar));
        //    }
        //    else
        //    {
        //        progressBar.Visible = false;
        //        Application.DoEvents();
        //    }
        //}
        /// <summary>
        /// Read meter in a thread ,takes state as a parameter which will 
        /// contain information required for thread to process
        /// </summary>
        /// <param name="uiContext"></param>
        private void ReadAsync(object state)
        {
            string data = string.Empty;
            string idLength = string.Empty;
            int index = 0;
            string lngFilename = string.Empty;
            String fileMeterData;
            string meterSerialNumber = string.Empty;
            string classIdOBISCodeAttribute = string.Empty;
            int meterModelNumber = 0;
            bool isConnected = false;
            List<ProfileId> selectedProfiles;
            List<ProfileCommand> lstProfileCommands;
            String strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");
            try
            {
                isMeterReading = true;
                this.StatusMessageAsync = "Reading Meter Data..";
                lstProfileCommands = GetProfileCommandEntity();
                selectedProfiles = GetSelectedProfilesToRead();              
                if (!Directory.Exists(strFileName))
                {
                    Directory.CreateDirectory(strFileName);
                }
                Result result = communication.OpenSession();

                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            result.ErrorCode = CommunicationErrorType.AccessDenied;
                            data = string.Empty;
                            idLength = result.RecieveDataBuffer[1].ToString("00");
                            index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);
                            for (int i = 2; i < index + 2; i++)
                            {
                                data += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                            }

                            #region CreateResultFile
                            meterSerialNumber = data;
                            Application.DoEvents();
                            //this..Text = "Meter Serial Number : " + meterSerialNumber;
                            strFileName = strFileName + data;
                            strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                            fileMeterData = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);

                            FileStream initialFileStream = new FileStream(strFileName, FileMode.Create);
                            StreamWriter writeToFile = new StreamWriter(initialFileStream);
                            #endregion
                            writeToFile.WriteLine(Splitter);
                            writeToFile.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            #region Getting Meter Time
                            Application.DoEvents();
                            DateTime meterTime = System.DateTime.Now;
                            try
                            {
                                profileCommand = new ProfileCommand(08, "00.00.01.00.00.FF", 02);
                                result = communication.Send(profileCommand);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    DateTime dateTime = DateTime.MinValue;
                                    int year = 0;
                                    int dataIndex = 2;
                                    year = year | (int)result.RecieveDataBuffer[dataIndex++] << 8;
                                    year = year | (int)result.RecieveDataBuffer[dataIndex++];

                                    int month = result.RecieveDataBuffer[dataIndex++];
                                    int day = result.RecieveDataBuffer[dataIndex++];
                                    dataIndex++;
                                    int hour = result.RecieveDataBuffer[dataIndex++];
                                    int min = result.RecieveDataBuffer[dataIndex++];
                                    int sec = result.RecieveDataBuffer[dataIndex++];

                                    dateTime = new DateTime(year, month, day, hour, min, sec);
                                    meterTime = dateTime;
                                }
                            }
                            catch
                            {

                            }
                            #endregion

                            #region ReadProfileAndWriteToFile

                            meterModelNumber = NamePlateConstants.PumaLTE650Value;

                            Application.DoEvents();
                            foreach (ProfileId selectedProfile in selectedProfiles)
                            {

                                this.StatusMessageAsync = "Reading " + selectedProfile.ToString() + " Data.....";
                                List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                {
                                    return profileCommandEntity.TagNumber == (int)selectedProfile
                                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                    profileCommandEntity.MeterModelNumber == 0);
                                });

                                if (selectedProfile == ProfileId.LoadSurvey)
                                {
                                    profileReadCommands[1].SelectiveAccess = true;
                                    profileReadCommands[1].SelectiveDays = noOfDays;
                                    profileReadCommands[1].StartTime = meterTime;
                                }
                                for (index = 0; index < profileReadCommands.Count; index++)
                                {

                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        profileReadCommands[index].Action = ActionType.READ;
                                        profileReadCommands[index].MeterID = meterId;
                                        result = communication.Send(profileReadCommands[index]);
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            if (!isAborted)
                                            {
                                                classIdOBISCodeAttribute = String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                                    + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                           + String.Format("{0:X2}", profileReadCommands[index].Attribute);
                                                if (result.RecieveDataLength <= 0)
                                                {

                                                }
                                                else
                                                {
                                                    writeToFile.Write(classIdOBISCodeAttribute);
                                                    for (int i = 0; i < result.RecieveDataLength; i++)
                                                    {
                                                        writeToFile.Write(String.Format("{0:X2}", result.RecieveDataBuffer[i]));
                                                    }
                                                    writeToFile.WriteLine("");
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                }
                                if (result.ErrorCode != CommunicationErrorType.Success || isAborted)
                                {
                                    if (isAborted)
                                    {
                                        if (btnRead.InvokeRequired)
                                        {
                                            btnRead.Invoke(new MethodInvoker(EnableReadControls));
                                        }
                                        else
                                        {
                                            btnRead.Enabled = true;
                                        }
                                        this.StatusMessageAsync = "User Aborted.";
                                        Application.DoEvents();
                                    }
                                    break;
                                }
                            }
                            #endregion

                            #region ResourceClosingAndCleanup
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                communication.CloseSession();
                                writeToFile.Close();
                                initialFileStream.Close();
                                if (isAborted)
                                {
                                    File.Delete(strFileName);
                                }
                                else
                                {
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
                                        // this.RightStatusMessage = String.Empty;
                                        this.StatusMessageAsync = "Readout Successful";
                                        SaveData(fileText);

                                    }
                                }
                            }
                            else
                            {
                                writeToFile.Close();
                                initialFileStream.Close();
                                File.Delete(strFileName);
                                this.StatusMessageAsync = ReadoutFailure;
                                MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            #endregion
                        }
                        else
                        {
                            this.StatusMessageAsync = ReadoutFailure;                           
                        }
                    }
                    else
                    {
                        this.StatusMessageAsync = ReadoutFailure;
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                else
                {
                    this.StatusMessageAsync = ReadoutFailure;
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), " BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            catch (Exception)
            {
                this.StatusMessageAsync = ReadoutFailure;
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                isAborted = false;
                isMeterReading = false;                              
                EnableReadControls();
            }
        }      
        /// <summary>
        ///  Used to create profileId enums based on profiles
        /// that needs to be read(Selected by user through checkboxes)
        /// </summary>
        /// <returns></returns>
        private List<ProfileId> GetSelectedProfilesToRead()
        {
            List<ProfileId> selectedProfiles = new List<ProfileId>();
            selectedProfiles.Clear();
            #region Mandatory Profiles/Configuration
            selectedProfiles.Add(ProfileId.NamePlate);
            selectedProfiles.Add(ProfileId.Phasor);
            selectedProfiles.Add(ProfileId.Instant);
            selectedProfiles.Add(ProfileId.Billing);
            selectedProfiles.Add(ProfileId.FraudEnergy);

            selectedProfiles.Add(ProfileId.BillingType);
            selectedProfiles.Add(ProfileId.KvahSelection);
            selectedProfiles.Add(ProfileId.DIP);
            selectedProfiles.Add(ProfileId.ResetLockOutDays);
            #endregion            
            if (chkLoadSurvey.Checked)
            {
                selectedProfiles.Add(ProfileId.LoadSurvey);
            }
            if (chkTamper.Checked)
            {
                selectedProfiles.Add(ProfileId.Tamper);
            }
            if (chkMidnight.Checked)
            {
                selectedProfiles.Add(ProfileId.Midnight);
            }                      
            if (chkFraudEnergy.Checked)
            {
               // selectedProfiles.Add(ProfileId.FraudEnergy);
            }
            if (chkMeterConfigurations.Checked)
            {               
                selectedProfiles.Add(ProfileId.RS232LockUnlock);               
                selectedProfiles.Add(ProfileId.PushDisplayParameter);
                selectedProfiles.Add(ProfileId.ScrollDisplyParameter);
                selectedProfiles.Add(ProfileId.HighResolutionDisplayParameter);
                selectedProfiles.Add(ProfileId.DisplayTimeoutParameter);
                selectedProfiles.Add(ProfileId.AutoLock);
                //TOU
                selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                selectedProfiles.Add(ProfileId.PassiveDayProfile);
                selectedProfiles.Add(ProfileId.ActiveSeasonProfile);
                selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                selectedProfiles.Add(ProfileId.ActiveDayProfile);
                selectedProfiles.Add(ProfileId.ActivationDate);
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
        private  string GetMD5ChecksumForFile(string fileName)
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
        /// <summary>
        /// Bind No of days in Load Survey Combo Box .
        /// </summary>
        private void BindLoadSurveyDays()
        {
            for (int index = 1; index <= 91; index++)
            {
                cmbLoadSurveyDays.Items.Add(index.ToString());              
                
            }
            cmbLoadSurveyDays.SelectedIndex = 29;
        }
        /// <summary>
        /// Generate phasor 
        /// </summary>
        /// <param name="state"></param>
        private void GeneratePhasor(object state)
        {
            this.StatusMessageAsync = "Reading Phasor data.....";
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;            
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            bool isResponseTimeout = false;
            Phasor mapperPhasor = new Phasor();
            bool isConnected = false;
            try
            {
                ClearPhasorControls();
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                Result result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    result = communication.Send(profileCommand);
                    if (isPhasorStopped)
                    {
                        isPhasorRuning = false;                       
                        EnableStartPhasorControl();
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            string idLength = result.RecieveDataBuffer[1].ToString("00");
                            int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);
                            lstProfileCommands = GetProfileCommandEntity();
                            List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                   {
                                       return profileCommandEntity.TagNumber == (byte)ProfileId.Phasor
                                       && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                                       profileCommandEntity.MeterModelNumber == 0);
                                   });

                            profileReadCommands[0].Action = ActionType.READ;
                            profileReadCommands[0].MeterID = meterId;
                            while (true)
                            {
                                try
                                {
                                    if (isPhasorStopped)
                                    {
                                        isPhasorRuning = false;
                                        break;
                                    }
                                    resultData = new StringBuilder();
                                    this.StatusMessageAsync = "Reading Phasor data.....";
                                    isResponseTimeout = false;
                                    result = communication.Send(profileReadCommands[0]);
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        resultData.Append(String.Format("{0:X2}", profileReadCommands[0].ClassId)
                                               + profileReadCommands[0].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                      + String.Format("{0:X2}", profileReadCommands[0].Attribute));
                                        for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                        {
                                            resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                        }

                                        List<ProfileData> phasorData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), true);
                                        List<PhasorEntity> phasorEntity = mapperPhasor.GetData(phasorData);

                                        Application.DoEvents();
                                        if (!isPhasorStopped)
                                        {
                                            UpdatePhasor(phasorEntity[0]);
                                            EnableStopPhasorControl();

                                        }
                                        else
                                        {
                                            this.StatusMessageAsync = "Phasor readout stopped.";
                                        }
                                    }
                                    else
                                    {
                                        isPhasorRuning = false;
                                        isResponseTimeout = true;
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    //MessageBox.Show(result.ErrorCode.ToString(), "BCS");
                                }
                                finally
                                {

                                }

                            }
                            if (!isPhasorRuning)
                            {
                                this.StatusMessage = string.Empty;
                                //btnReadPhasor.Enabled = true;
                                // btnStopPhasor.Enabled = false;
                                if (isResponseTimeout)
                                {
                                    isResponseTimeout = false;
                                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }
                        else
                        {

                            communication.CloseSession();
                            this.StatusMessageAsync = ReadoutFailure;
                        }
                    }
                    else
                    {
                        isPhasorRuning = false;
                        // btnReadPhasor.Enabled = true;
                        //btnStopPhasor.Enabled = false;
                        this.StatusMessage = string.Empty;
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    isPhasorRuning = false;                    
                    this.StatusMessage = string.Empty;
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Error in Reading Phasor","BCS",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                //btnReadPhasor.Enabled = true;
                //btnStopPhasor.Enabled = false;
                if (isConnected)
                {
                   communication.CloseSession();
                }
                isPhasorRuning = false;
                EnableReadControls();                
                //this.Cursor = Cursors.Default;
                //this.UseWaitCursor = false;                
            }

        }
        /// <summary>
        /// Enable phasor stop button when one reading cycle completed
        /// </summary>
        private void EnableStopPhasor()
        {

            if (btnStopPhasor.InvokeRequired)
            {
                btnStopPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnStopPhasor.Enabled = true;
            }
            if (btnReadPhasor.InvokeRequired)
            {
                btnReadPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnReadPhasor.Enabled = false;
            }            
            Application.DoEvents();
        
        }
        /// <summary>
        /// Used to clear phasor form previous data 
        /// </summary>
        private void ClearPhasor()
        {
            lngPhasorDiagram.PhasorData = null;
            lngPhasorDiagram.Refresh();
            lngPhasorDiagram.Show();

            lngPgrid.Data = null;

            //lblRVoltageValue.Text = string.Empty;
            //lblYVoltageValue.Text = string.Empty;
            //lblBVoltageValue.Text = string.Empty;
            //lblRCurrentValue.Text = string.Empty;
            //lblYCurrentValue.Text = string.Empty;
            //lblBCurrentValue.Text = string.Empty;
            //lblActivePowerValue.Text = string.Empty;
            ////lblReactivePowerValue.Text = phasorData;
            //lblApparentPowerValue.Text = string.Empty;
            //lblRPhasePFValue.Text = string.Empty;
            //lblYPhasePFValue.Text = string.Empty;
            //lblBPhaesPFValue.Text = string.Empty;
            //lblFrequencyValue.Text = string.Empty;
            //lblPhaseSeqValue.Text = string.Empty;
            //lblRPhaseKWDirVAlue.Text = string.Empty;
            //lblYPhaseKWDirValue.Text = string.Empty;
            //lblBPhaseKWDirValue.Text = string.Empty;
            //lblRChannelValue.Text = string.Empty;
            //lblYChannelValue.Text = string.Empty;
            //lblBChannelValue.Text = string.Empty;
            //lblRLagLeadValue.Text = string.Empty;
            //lblYLagLeadValue.Text = string.Empty;
            //lblBLagLeadValue.Text = string.Empty;
            //lblAngelYRValue.Text = string.Empty;
            //lblAngleBRValue.Text = string.Empty;
            //lblAngleBwTwoValue.Text = string.Empty;
            //lblTotalPWFactorValue.Text = string.Empty;



        }
        /// <summary>
        /// Update dynamic phasor 
        /// </summary>
        /// <param name="phasorData"></param>
        private void UpdatePhasor(PhasorEntity phasorData)
        {
            if (phasorData.PhaseSequence.ToUpper() == "CORRECT")
            {
                phasorDataForDiagram = phasorData;
                UpdatePhasorDiagram();
            }
            else
            {
                PhaseSequenceIncorrectMesage();
            }
            
            if (phasorData != null)
            {

                DataTable table = new DataTable();
                int col = 0;
                string[] phasorRow = PhasorRow();
                string[] phasorColumn = PhasorColumnValues();

                for (col = 0; col < phasorRow.Length; col++)
                    table.Columns.Add(new DataColumn(phasorRow[col], typeof(System.String)));
                for (int rowCount = 0; rowCount < phasorColumn.Length; rowCount++)
                {
                    DataRow dataRow = table.NewRow();
                    for (col = 0; col < 2; col++)
                    {
                        if (col == 0)
                            dataRow[col] = phasorColumn[rowCount];
                        else
                            dataRow[col] = string.Empty;
                    }
                    table.Rows.Add(dataRow);
                }
                /*Voltage R y  b  Phase*/
                table.Rows[0][1] = phasorData.RPhaseVoltage;
                table.Rows[1][1] = phasorData.YPhaseVoltage;
                table.Rows[2][1] = phasorData.BPhaseVoltage;

                /*Current R y  b  Phase*/
                table.Rows[3][1] = phasorData.RPhaseCurrent;
                table.Rows[4][1] = phasorData.YPhaseCurrent;
                table.Rows[5][1] = phasorData.BPhaseCurrent;

                ///*Resolution*/
                //table.Rows[6][1] = PhasorFilterData(PhasorPara, 37, 1);

                /*Total Active, Inductive, Capacitive and Apparent Power*/
                table.Rows[6][1] = phasorData.TotalActivePower;
                table.Rows[7][1] = phasorData.TotalInductivePower;
                table.Rows[8][1] = phasorData.TotalCapacitivePower;
                table.Rows[9][1] = phasorData.TotalApparentPower;


                /*PF R y  b  Phase*/
                table.Rows[10][1] = phasorData.RPhasePF;
                table.Rows[11][1] = phasorData.YPhasePF;
                table.Rows[12][1] = phasorData.BPhasePF;
                /*Net PF */
                table.Rows[13][1] = phasorData.TotalInstantaneousPF;
                /*Frequency */
                table.Rows[14][1] = phasorData.Frequency;

                table.Rows[15][1] = phasorData.PhaseSequence;              


                /*Total */
                table.Rows[16][1] = phasorData.TotalActivePower.Contains("-") ? "Export" : "Import";

                ///*Import/Export R y  b  Phase*/
                table.Rows[17][1] = phasorData.RPhasekWDirection;
                table.Rows[18][1] = phasorData.YPhasekWDirection;
                table.Rows[19][1] = phasorData.BPhasekWDirection;

                ///*Chaneel Missing R y  b  Phase*/
                table.Rows[20][1] = phasorData.RPhaseChannel;
                table.Rows[21][1] = phasorData.YPhaseChannel;
                table.Rows[22][1] = phasorData.BPhaseChannel;



                table.Rows[23][1] = phasorData.RPhaseLagLead;
                table.Rows[24][1] = phasorData.YPhaseLagLead;
                table.Rows[25][1] = phasorData.BPhaseLagLead;

                //*Lag/ Lead Total*/
                table.Rows[26][1] = phasorData.Total;
                

                /* Y B Phase Angle with respect to R Phase*/
                table.Rows[27][1] = phasorData.YPhaseAngleWithRPhase;
                table.Rows[28][1] = phasorData.BPhaseAngleWithRPhase;
                table.Rows[29][1] = phasorData.AngleBWAnyPhasePresent;
                phasorDataForGrid = new DataSet();
                phasorDataForGrid.Tables.Add(table);
                UpdatePhasorGrid();
            }
            //this.UseWaitCursor = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string[] PhasorRow()
        {
            string[] array = new string[2];
            array[0] = "Parameter";
            array[1] = "Value";
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
        /// Display FraudEnergy data to UI
        /// </summary>
        private void  FillFraudEnergyData(FraudEnergyEntity fraudEnergyData)
        {

            txtFraudActive.Text = fraudEnergyData.MagneticInfluenceKWh;
            txtFraudApparent.Text = fraudEnergyData.MagneticInflueneceKVAh;
            txtFraudReactiveLag.Text = fraudEnergyData.MagneticInflueneceKVARhLag;
            txtFraudReactiveLead.Text = fraudEnergyData.MagneticInflueneceKVARhLead;

            txtRevKvah.Text = fraudEnergyData.ReverseEnergyKVAh;
            txtRevKwh.Text = fraudEnergyData.ReverseEnergyKWh;
            txtReverseEnergyKvarhLag.Text = fraudEnergyData.ReverseEnergyKVARhLag;
            txtReverseEnergyKvarhLead.Text = fraudEnergyData.ReverseEnergyKVARhLead;
            txtTHDVoltageRPhase.Text = fraudEnergyData.THDVoltageRPhase;
            txtTHDVoltageYPhase.Text = fraudEnergyData.THDVoltageYPhase;
            txtTHDVoltageBPhase.Text = fraudEnergyData.THDVoltageBPhase;

            txtTHDRPhaseCurrent.Text = fraudEnergyData.THDCurrentRPhase;
            txtTHDYPhaseCurrent.Text = fraudEnergyData.THDCurrentYPhase;
            txtTHDBPhaseCurrent.Text = fraudEnergyData.THDCurrentBPhase;
        }
        #endregion                    
        
        
    }
}
