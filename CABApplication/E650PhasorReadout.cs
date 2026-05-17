#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common.EntityMapper;
using CAB.BLL;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Hunt.EPIC.Logging;
#endregion
namespace CABApplication
{
    /// <summary>
    /// Dyanamic phasor readout .
    /// </summary>
    partial class E650PhasorReadout : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private Communication communication;
        private GenerateEntity entityGenerator;
        private List<byte> meterId = null;
        private bool isPhasorRuning = false;
        private bool isPhasorStopped = false;
        private DataSet phasorDataForGrid = null;
        private PhasorEntity phasorDataForDiagram = null;
        private const string ReadoutFailure = "Readout Failure.";
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
        private CommunicationMode commMode = CommunicationMode.Normal;
        StringBuilder resultData = new StringBuilder();
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650PhasorReadout).ToString());

        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// This is constructer
        /// </summary>
        public E650PhasorReadout()
        {
            InitializeComponent();
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            // channelInfo.ModemInfo = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);          
            entityGenerator = new GenerateEntity();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers



        private void btnReadPhasor_Click(object sender, EventArgs e)
        {

            isPhasorStopped = false;
            isPhasorRuning = true;
            btnReadPhasor.Enabled = false;
            // btnStopPhasor.Enabled = true;
            btnCancelPhasor.Enabled = false;
            DataAcquisition.Enabled = false;
            Configuration.Enabled = false;
            Thread readThread = new Thread(GeneratePhasor);
            readThread.Start(SynchronizationContext.Current);

        }
        /// <summary>
        /// HOld phasor data read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopPhasor_Click_1(object sender, EventArgs e)
        {

            this.StatusMessage = "User Aborted.";
            btnStopPhasor.Enabled = false;
            btnCancelPhasor.Enabled = true;
            this.Cursor = Cursors.Default;
            isPhasorStopped = true;
            isPhasorRuning = false;
            Application.DoEvents();
            SetConnectionDetail(false);
            EnableStopTimer();
        }

        /// <summary>
        /// Close current window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelPhasor_Click_1(object sender, EventArgs e)
        {
            SetConnectionDetail(false);
            EnableStopTimer();
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650PhasorReadout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isPhasorRuning)
            {
                e.Cancel = true;
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

        #endregion

        #region Private Methods

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

            if (btnReadPhasor.InvokeRequired)
            {
                btnReadPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnReadPhasor.Enabled = true;
            }
            if (btnCancelPhasor.InvokeRequired)
            {
                btnCancelPhasor.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnCancelPhasor.Enabled = true;
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
                lngPgrid.SetWidth(0, 130);
                lngPgrid.SetWidth(1, 60);
                lngPgrid.SetWidth(2, 168);
                lngPgrid.SetWidth(3, 55);
                lngPgrid.SetHeaderText(0, "Parameters");
                lngPgrid.SetHeaderText(1, "Values");
                lngPgrid.SetHeaderText(2, "Parameters");
                lngPgrid.SetHeaderText(3, "Values");
                lngPgrid.ResizeColumn(false);
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

        /// <summary>
        /// Reads Phasor In Fast Download Mode
        /// </summary>
        private void ReadPhasorFastDownload(ProfileCommand resultFDData)
        {
            Result result;            
            Phasor mapperPhasor = new Phasor();
            ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.BC.FF", 02);
            string data = communication.GetSignatureData();              

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
                    resultFDData.Action = ActionType.READ;
                    resultFDData.MeterID = meterId;
                    result = communication.Send(resultFDData);
                    if ((result.RecieveDataLength <= 0) || (result.RecieveDataLength < 33))
                    {
                        this.StatusMessageAsync = "Readout Not Supported";
                        break;
                    }
                    else
                    {
                        #region WriteSignatureData
                        //Write signature data that will be used for getting meter model number .                        
                        resultData.AppendLine(GetSignatureDataInFileFormat(data));
                        #endregion

                        resultData.Append(String.Format("{0:X2}", resultFDData.ClassId)
                               + resultFDData.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                      + String.Format("{0:X2}", resultFDData.Attribute));
                        for (int counter = 0; counter < result.RecieveDataBuffer.Count; counter++)
                        {
                            resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                        }                      

                        List<ProfileData> phasorData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), true);
                        PhasorEntity phasorEntity = mapperPhasor.GetMappedEntity(phasorData);
                        Application.DoEvents();
                        if (!isPhasorStopped)
                        {
                            UpdatePhasor(phasorEntity);
                            EnableStopPhasorControl();

                        }
                        else
                        {
                            this.StatusMessageAsync = "Phasor readout stopped.";
                        }
                    }


                }

                catch (Exception ex)    //Exception log for catch block
                {
                    //MessageBox.Show(result.ErrorCode.ToString(), "BCS");
                    logger.Log(LOGLEVELS.Error, "ReadPhasorFastDownload(ProfileCommand resultFDData)", ex);
                }
                finally
                {

                }
            }

        }

        /// <summary>
        /// Gets the signature data in file format
        /// </summary>
        /// <param name="signatureInfo"></param>
        /// <returns></returns>
        private string GetSignatureDataInFileFormat(string signatureInfo)
        {
            string outputSignatureInfo = "0100006001BCFF020914";//322E34393234303031303036305743347253";
            byte[] dataInByteForm = System.Text.Encoding.ASCII.GetBytes(signatureInfo);

            for (int dataIndex = 0; dataIndex < signatureInfo.Length; dataIndex++)
            {
                outputSignatureInfo = outputSignatureInfo + String.Format("{0:X2}", dataInByteForm[dataIndex]);
            }
            return outputSignatureInfo;
        }


        /// <summary>
        /// Reads Phasor In Normal Mode
        /// </summary>
        private void ReadPhasorNormal(ProfileCommand captureDataObject, StringBuilder captureObject, StringBuilder scalarObject, StringBuilder scalarUnitAndDataObject)
        {
            Result result;
            Phasor mapperPhasor = new Phasor();
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
                    captureDataObject.Action = ActionType.READ;
                    captureDataObject.MeterID = meterId;
                    result = communication.Send(captureDataObject);

                    resultData.Append(captureObject);
                    resultData.AppendLine("");

                    resultData.Append(String.Format("{0:X2}", captureDataObject.ClassId)
                          + captureDataObject.ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                 + String.Format("{0:X2}", captureDataObject.Attribute));

                    for (int counter = 0; counter < result.RecieveDataBuffer.Count; counter++)
                    {
                        resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                    }
                    resultData.AppendLine("");

                    resultData.Append(scalarObject);
                    resultData.AppendLine("");

                    resultData.Append(scalarUnitAndDataObject);
                    resultData.AppendLine("");
                    resultData.AppendLine(" ");

                    List<ProfileData> phasorData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), false);
                    PhasorEntity phasorEntity = mapperPhasor.GetMappedEntity(phasorData);
                    Application.DoEvents();
                    if (!isPhasorStopped)
                    {
                        UpdatePhasor(phasorEntity);
                        EnableStopPhasorControl();

                    }
                    else
                    {
                        this.StatusMessageAsync = "Phasor readout stopped.";
                    }

                }

                catch (Exception ex)    //Exception log for catch block
                {
                    //MessageBox.Show(result.ErrorCode.ToString(), "BCS");
                    logger.Log(LOGLEVELS.Error, "ReadPhasorNormal(ProfileCommand captureDataObject, StringBuilder captureObject, StringBuilder scalarObject, StringBuilder scalarUnitAndDataObject)", ex);
                }
                finally
                {

                }
            }

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
            GenerateEntity entityGenerator = new GenerateEntity();
            bool isResponseTimeout = false;
            Phasor mapperPhasor = new Phasor();
            bool isConnected = false;
            Result captureObjects;
            Result scalarObjects;
            Result scalarUnitAndData;
            List<byte> captureObjectBuffer;
            List<byte> scalarObjectsBuffer;
            List<byte> scalarUnitAndDataBuffer;
            ProfileCommand captureDataObject;
            ProfileCommand fastDownloadObject;
            StringBuilder captureObjectString;
            StringBuilder scalarObjectString;
            StringBuilder scalarUnitAndDataString;
            string MeterID = string.Empty;
            string MeterItem = string.Empty;
            int MeterModelNo =0;
            int securitymachanism = 0;
            const int LLSKEYINDEX = 1;
            const int GLOBALKEYINDEX = 2;
            const int TEMPGLOBALKEYINDEX = 3;

            try
            {
                //EnableStartTimer();
                //commMode = GetCommuniactionMode();
                //ClearPhasorControls();
                //ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                //Result result = communication.OpenSession();
               //SetConnectionDetail(true);
                          
                ChannelInformation channelInfo = new ChannelInformation();
                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                if (ConfigSettings.GetValue("PortName").Contains(","))
                    channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
                else
                    channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                Result result = new Result();
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                    channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter
                else
                    channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communication = new Communication(channelInfo);
                if (ConfigSettings.GetValue("ApplicationContext") == "03")
                    if (channelInfo.SecurityMechanism == 0x00)
                    {

                        result = communication.OpenSession();
                        // ************ Read meter ID Start
                        ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x60.0x01.0x00.0xFF", 02);
                        result = communication.Send(profileCommand);
                        communication.CloseSession();
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
                        result = communication.OpenSession();
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
                                data = FormatData(incount, false);
                                if (data != null) InvoCountValue = Convert.ToInt64(data);
                                InitializationCounter = InvoCountValue + 1;

                                //List<string> SecurityKeyDetails = GetSecurityKeys(MeterID);
                                List<string> SecurityKeyDetails = Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                                if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= GLOBALKEYINDEX)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[LLSKEYINDEX]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[GLOBALKEYINDEX]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[GLOBALKEYINDEX]);
                                }
                                result = communication.OpenSessionCipher(InitializationCounter);

                                if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > TEMPGLOBALKEYINDEX)
                                {
                                    ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[LLSKEYINDEX]);
                                    ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[TEMPGLOBALKEYINDEX]);
                                    ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[TEMPGLOBALKEYINDEX]);
                                    result = communication.OpenSessionCipher(InitializationCounter + 1);
                                }
                                // Get smart meter model detail
                                profileCommand = new ProfileCommand(01, "00.00.60.80.08.FF", 02);
                                result = communication.Send(profileCommand);
                                if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    int mindex = 0;

                                    mindex = Convert.ToInt16(result.RecieveDataBuffer[1]);
                                    for (int i = 2; i < mindex + 2; i++)
                                    {
                                        MeterItem += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                                    }

                                    if (MeterItem.Contains("SM0310"))
                                    {
                                        MeterModelNo = NamePlateConstants.SmartM_Cipher_WCM; //Smart meter 3 ph WCM 35.

                                    }
                                    else if (MeterItem.Contains("SM0405"))
                                    {
                                        MeterModelNo = NamePlateConstants.SmartM_Cipher_LTCT; //Smart meter 3 ph LTCT 34.

                                    }
                                    else if (MeterItem.Contains("SM0110"))
                                    {

                                        MeterModelNo = NamePlateConstants.SmartM_Cipher_1PH; //Smart meter 1 ph 37.

                                    }
                                }
                            }
                        }

                    }
                // ************Read Invocation Counter Close
                if (ConfigSettings.GetValue("ApplicationContext") != "03")
                    result = communication.OpenSession();


                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                {
                    EnableStartTimer();
                    commMode = GetCommuniactionMode();
                    ClearPhasorControls();
                   ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.BC.FF", 02);
                    result = communication.Send(profileCommand);
                    SetConnectionDetail(true);
                    captureDataObject = new ProfileCommand();
                    isConnected = true;
                    //result = communication.Send(profileCommand);
                    if (isPhasorStopped)
                    {
                        isPhasorRuning = false;
                        EnableStartPhasorControl();
                    }
                 
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        //if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        //{

                            List<ProfileCommand> SMPhasorCommand = new List<ProfileCommand>() { 
                                new ProfileCommand(07,"01.00.63.80.80.FF", 03 ),
                                new ProfileCommand(07,"01.00.63.80.80.FF", 02),
                                new ProfileCommand(07,"01.00.5E.5B.80.FF", 03),
                                new ProfileCommand(07,"01.00.5E.5B.80.FF", 02)
                            };
                            captureObjectString = new StringBuilder();
                            scalarObjectString = new StringBuilder();
                            scalarUnitAndDataString = new StringBuilder();
                            captureObjectBuffer = new List<byte>();
                            scalarObjectsBuffer = new List<byte>();
                            scalarUnitAndDataBuffer = new List<byte>();
                            //string idLength = result.RecieveDataBuffer[1].ToString("00");
                            //int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            //meterId = new List<byte>();
                            //meterId = result.RecieveDataBuffer.GetRange(2, index);
                            lstProfileCommands = GetProfileCommandEntity();
                            List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, ProfileId.Phasor, NamePlateConstants.PumaLTE650Value);

                            if (profileReadCommands.Count > 1)
                            {
                                profileReadCommands[0].Action = ActionType.READ;
                                profileReadCommands[0].MeterID = meterId;
                                if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM) 
                                  captureObjects = communication.Send(SMPhasorCommand[0]);
                                else
                                 captureObjects = communication.Send(profileReadCommands[0]);
                               
                                captureObjectBuffer = captureObjects.RecieveDataBuffer;
                                if (captureObjects.ErrorCode.ToString() == "Success" && captureObjectBuffer.Count > 0)
                                {
                                    captureObjectString.Append(String.Format("{0:X2}", profileReadCommands[0].ClassId)
                                               + profileReadCommands[0].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                      + String.Format("{0:X2}", profileReadCommands[0].Attribute));

                                    for (int counter = 0; counter < captureObjectBuffer.Count; counter++)
                                    {
                                        captureObjectString.Append(String.Format("{0:X2}", captureObjectBuffer[counter]));
                                    }
                                    profileReadCommands[2].Action = ActionType.READ;
                                    profileReadCommands[2].MeterID = meterId;
                                    if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
                                        scalarObjects = communication.Send(SMPhasorCommand[2]);
                                    else
                                     scalarObjects = communication.Send(profileReadCommands[2]);
                                   
                                    profileReadCommands[3].Action = ActionType.READ;
                                    profileReadCommands[3].MeterID = meterId;
                                    if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
                                        scalarUnitAndData = communication.Send(SMPhasorCommand[3]);
                                    else
                                      scalarUnitAndData = communication.Send(profileReadCommands[3]);
                                    
                                    if (scalarObjects.ErrorCode == CommunicationErrorType.Success && scalarUnitAndData.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        if (scalarUnitAndData.RecieveDataBuffer.Count > 0)
                                        {
                                            scalarObjectsBuffer = scalarObjects.RecieveDataBuffer;
                                            scalarUnitAndDataBuffer = scalarUnitAndData.RecieveDataBuffer;
                                            scalarObjectString.Append(String.Format("{0:X2}", profileReadCommands[2].ClassId)
                                                   + profileReadCommands[2].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                          + String.Format("{0:X2}", profileReadCommands[2].Attribute));

                                            for (int counter = 0; counter < scalarObjectsBuffer.Count; counter++)
                                            {
                                                scalarObjectString.Append(String.Format("{0:X2}", scalarObjectsBuffer[counter]));
                                            }

                                            scalarUnitAndDataString.Append(String.Format("{0:X2}", profileReadCommands[3].ClassId)
                                                   + profileReadCommands[3].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                          + String.Format("{0:X2}", profileReadCommands[3].Attribute));

                                            for (int counter = 0; counter < scalarUnitAndDataBuffer.Count; counter++)
                                            {
                                                scalarUnitAndDataString.Append(String.Format("{0:X2}", scalarUnitAndDataBuffer[counter]));
                                            }
                                        }
                                        else
                                        {
                                            isPhasorRuning = false;
                                            isResponseTimeout = true;
                                            this.StatusMessageAsync = "Readout Not Supported";
                                            return;
                                        }
                                    }
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
                                         captureDataObject = SMPhasorCommand[1];
                                        else
                                        captureDataObject = profileReadCommands[1];
                                        
                                        ReadPhasorNormal(captureDataObject, captureObjectString, scalarObjectString, scalarUnitAndDataString);
                                    }
                                    else
                                    {
                                        isPhasorRuning = false;
                                        isResponseTimeout = true;
                                        this.StatusMessageAsync = "Readout Not Supported";
                                    }
                                }
                                else
                                {
                                    fastDownloadObject = new ProfileCommand();
                                    fastDownloadObject = profileReadCommands[4];
                                    fastDownloadObject.Action = ActionType.READ;
                                    fastDownloadObject.MeterID = meterId;
                                    if (result.ErrorCode == CommunicationErrorType.Success)
                                    {
                                        ReadPhasorFastDownload(fastDownloadObject);
                                    }
                                    else
                                    {
                                        isPhasorRuning = false;
                                        isResponseTimeout = true;
                                    }
                                }

                            }

                            if (!isPhasorRuning)
                            {
                                
                                this.StatusMessage = string.Empty;
                                if (isResponseTimeout)
                                {
                                    isResponseTimeout = false;
                                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                                }

                            }
                        //}
                        else
                        {

                            communication.CloseSession();
                            this.StatusMessageAsync = ReadoutFailure;
                        }
                    }
                    else
                    {
                        isPhasorRuning = false;
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }

                }
                else
                {
                    isPhasorRuning = false;
                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessageAsync = ReadoutFailure;
                logger.Log(LOGLEVELS.Error, "GeneratePhasor(object state)", ex);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                isPhasorRuning = false;
                EnableReadControls();
                SetConnectionDetail(false);
                EnableStopTimer();
            }

        }
        /// <summary>
        /// Enable phasor stop button when one reading cycle completed
        /// </summary>
        /// 

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
                if (source == "command center" && result == "OK" && errorcode == "0.0")
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
                            itemindex++;
                            break;
                        }
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

        public static string FormatData(byte[] buffer, bool bUnsignFlag)
        {
            StringBuilder sb = new StringBuilder();

            bool bSignFlag = false;
            Int64 tempVal = 0;
            for (int i = 0; i < buffer.Length; i++)
            {

                if (buffer[0] > 127)
                {

                    if (buffer.Length > 1)
                    {
                        if (bUnsignFlag) bSignFlag = true;

                    }
                }
                sb.Append(buffer[i].ToString("X2"));
            }

            if (bSignFlag == true)
            {
                if (buffer.Length == 4)
                {
                    tempVal = Convert.ToInt64("FFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else if (buffer.Length == 8)
                {
                    tempVal = Convert.ToInt64("FFFFFFFFFFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else
                {
                    tempVal = Convert.ToInt32("FFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }

            }
            else
            {
                return Convert.ToInt64(sb.ToString(), 16).ToString();
            }
        }
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
        /// 
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber)
        {
            List<ProfileCommand> profileReadCommands = null;
            
            //find normal commands
            profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
            {
                return profileCommandEntity.TagNumber == (int)selectedProfile
                && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                profileCommandEntity.MeterModelNumber == 0);
            });
           
            return profileReadCommands;
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
        /// Used to clear phasor form previous data 
        /// </summary>
        private void ClearPhasor()
        {
            lngPhasorDiagram.PhasorData = null;
            lngPhasorDiagram.Refresh();
            lngPhasorDiagram.Show();
            lngPgrid.Data = null;

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
            else if (phasorData.PhaseSequence.ToUpper() == "INVALID")
            {
                phasorData.PhaseSequence = "Incorrect";
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
                string signatureInfo = communication.GetSignatureData().ToUpper();

                #region HTCT Specific
                if (signatureInfo.ToUpper().Contains("HM"))
                {
                    for (int columnIndex = 0; columnIndex < phasorColumn.Length; columnIndex++)
                    {
                        if (phasorColumn[columnIndex].Contains("kW"))
                            phasorColumn[columnIndex] = phasorColumn[columnIndex].Replace("kW", "MW");
                        else if (phasorColumn[columnIndex].Contains("kVA"))
                            phasorColumn[columnIndex] = phasorColumn[columnIndex].Replace("kVA", "MVA");
                        else if (phasorColumn[columnIndex].Contains("KVA"))
                            phasorColumn[columnIndex] = phasorColumn[columnIndex].Replace("KVA", "MVA");
                        else if (phasorColumn[columnIndex].Contains("kvar"))
                            phasorColumn[columnIndex] = phasorColumn[columnIndex].Replace("kvar", "Mvar");
                    }
                }
                #endregion

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
                table.Rows[6][1] = phasorData.ActivePower;
                table.Rows[7][1] = phasorData.TotalInductivePower;
                table.Rows[8][1] = phasorData.TotalCapacitivePower;
                table.Rows[9][1] = phasorData.ApparentPower;


                /*PF R y  b  Phase*/
                table.Rows[10][1] = phasorData.RPhasePowerFactor;
                table.Rows[11][1] = phasorData.YPhasePowerFactor;
                table.Rows[12][1] = phasorData.BPhasePowerFactor;
                /*Net PF */
                table.Rows[13][1] = phasorData.TotalPhasePowerFactor;
                /*Frequency */
                table.Rows[14][1] = phasorData.Frequency;

                table.Rows[0][3] = phasorData.PhaseSequence;


                /*Total */
                table.Rows[1][3] = phasorData.ActivePower.Contains("-") ? "Export" : "Import";

                ///*Import/Export R y  b  Phase*/
                table.Rows[2][3] = phasorData.RPhaseNegativePowerFlag;
                table.Rows[3][3] = phasorData.YPhaseNegativePowerFlag;
                table.Rows[4][3] = phasorData.BPhaseNegativePowerFlag;

                ///*Chaneel Missing R y  b  Phase*/
                table.Rows[5][3] = phasorData.RPhaseChannel;
                table.Rows[6][3] = phasorData.YPhaseChannel;
                table.Rows[7][3] = phasorData.BPhaseChannel;



                table.Rows[8][3] = phasorData.RPhaseCapacitiveInductiveFlag;
                table.Rows[9][3] = phasorData.YPhaseCapacitiveInductiveFlag;
                table.Rows[10][3] = phasorData.BPhaseCapacitiveInductiveFlag;

                //*Lag/ Lead Total*/
                table.Rows[11][3] = phasorData.Total;


                /* Y B Phase Angle with respect to R Phase*/
                table.Rows[12][3] = phasorData.AngleYR;
                table.Rows[13][3] = phasorData.AngleBR;
                table.Rows[14][3] = phasorData.AngleBetweenTwo;
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
            array[26] = "Total Lag/Lead";
            array[27] = "Y Phase Angle With R Phase";
            array[28] = "B Phase Angle With R Phase";
            array[29] = "Angle B/W Any 2 Phase Present";
            return array;
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
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {
            string mode;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
            else
            {
                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

            }
        }

        
        /// <summary>
        /// to start the progress bar and overlap the position of 
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StartProgressBarTimer()
        {
            statusStrip.Visible = true;
            progressBarTimer.Enabled = true;
        }

        /// <summary>
        /// to stop progress bar , make it in-visible and make 
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
        #endregion

        private void E650PhasorReadout_Load(object sender, EventArgs e)
        {
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];
        }

       


    }
}
