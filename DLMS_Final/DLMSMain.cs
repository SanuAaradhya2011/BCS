using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CHANNEL.Formatter;
using FastDownloading;
using CAB.BCS.DLMS.Model;
using CAB.BCS.DLMS.Presenter;
using CAB.BCS.DLMS.Utility;
using CAB.BCS.DLMS.Views;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI;
using CAB.UI.Controls;
using CABEntity;
using LTCTBLL;
using SerialCommunication;
using Utilities;
using CAB.Channel.Formatter;
namespace DLMS_Final
{
    public delegate void RefreshPhasorDiagram(PhasorEntity phasor);
    public delegate void UpdateStatusMessage(string message);
    public partial class DLMSMain : MdiChildForm, ITOUDefinition, ICMRI
    {
        internal enum ProgrammingCode
        {
            Success,
            Fail,
            AccessDenied,
            DataUnavailable,
            TimeOut,
            SignOnFailed,
            CosemConnectionFailed,
            MeterIDMismatch
        }

        #region Declaration
        bool flgdataValid;
        TabPage tempPage;
        TabPage tempPageUS1;
        TabPage tempPageUS2;
        public string respTOURead;
        DataGridView[] touGridNames;
        string[] touCommands;
        bool tabpageExist = true;
        bool tabpageExistUS = true;
        string commandResponse = string.Empty;
        byte[] HDLCCommand = new byte[200];
        byte[] MODEMCommand = new byte[20];
        byte HDLCIndex = 0;
        byte MODEMIndex = 0;
        bool err20 = false;
        string errStr = "";
        DataSet ds = null;
        byte ShowIndex = 0;
        bool isCurrentCommandOfPTRatio = false;
        SystemSettingsBLL objSystemSettings = new SystemSettingsBLL();
        bool isConnectionTested = false;
        bool isPortAssociationChanged = false;
        int PreviousPortAssociationRowIndex = -1, PreviousPortAssociationColIndex = -1;
        bool PreviousPortAssociationValue = false;
        string DefaultPortName = string.Empty;
        private static IList<CABSerialPort> lstSavedSerialPorts = new List<CABSerialPort>();
        private const string NOFAILURESFOUND = "No Failures Found";
        private const string NOTHINGTOEXPORT = "No data available to export";
        private const string SAVEFILEEXTENSION = "Text files (*.txt)|*.txt|*.csv|*.csv";
        private const string SUCCESSEXPORT = "File Exported Successfully";
        private const string FAILUREOCCURRED = "Failure Occurred";
        private const string EXPORTNOTSUCCESSFUL = "Export not successful";
        private const string METERSERIALNUMBER = "MeterSerialNumber";
        private const string PARAMETERS = "Parameters";
        private const string STATUS = "Status";
        private const string CTRATIOVALMESAGE = "CT Ratio value should be between 1 and ";
        private const string PTRATIOVALMESAGE = "PT Ratio value should be between 1 and ";
        private const string KVAHSELECTION = "Please select kVAh selection parameters.";
        private const string PUSHBUTTONVALMESSAGE = "Please select at least 1 push button display parameter";
        private const string SCROLLBUTTONVALMESSAGE = "Please select at least 1 scroll button display parameter";
        private const string HIGHRESOLUTIONVALMESSAGE = "Please select at least 1 high resolution display parameter";
        private const string DISPLAYTIMEOUTMESSAGE = "Please enter correct numeric value for Scroll and Push Button timeout.";
        private const int WIDTH = 100;
        private const int NVMFAILUREINDEX = 21;
        private const int RTCBADINDEX = 28;
        private const int RTCSTOPINDEX = 29;
        private const int RTCTIMESTOPINDEX = 30;
        string utility = string.Empty;
        bool isPUMA = false;
        bool commAborted;
        bool isMVVNL = false;
        string connectionMessage = string.Empty;
        private const string START = "Start";
        private const string STOP = "Stop";
        private const string WRITESUCCESSMESSAGE = "Parameter written successfully";
        private const string READSUCCESSMESSAGE = "Parameter read successfully";
        private const string OPERATIONFAILED = "Operation Failed";
        private const string ACCESSDENIED = "Access Denied";
        private const string MaxEmfLimitMessage = "Multiplication of CT ratio and PT ratio should not be greater than 7500.Please check CT and PT values";
        private int meterModelNumber = 0;
        private Dictionary<string, int> meterLoadList = null;
        DateTime startDatetime;
        List<byte> selectedPushParams = new List<byte>();
        List<byte> selectedScrollParams = new List<byte>();
        List<byte> selectedHighResParams = new List<byte>();
        #region Modem config details variables
        private ModemConfig modemConfig = null;
        private string[] initCommands = null;
        private string[] resetCommands = null;
        private string dial = string.Empty;
        private string baudRate = string.Empty;
        private string parity = string.Empty;
        private string stopBit = string.Empty;
        private string dataBit = string.Empty;
        private int configuredTimeOut = 5000;
        private int configuredIntercharacterDelay = 3500;
        private int dLMSRetryForMeterCommands = 0;
        private const int DLMSRetryForModemCommands = 0;
        private CommunicationType comType;
        private const string MegaActiveEnergy = "MWH";
        private const string MegaApparentEnergy = "MVAh";
        private const string MegaReactiveEnergyLagLead = "MVArh";

        #endregion
        /// <summary>
        /// for storing DLMS Retries for meter commands
        /// </summary>
        public int DLMSRetryForMeterCommands
        {
            get
            {
                return dLMSRetryForMeterCommands;
            }
            set
            {
                dLMSRetryForMeterCommands = value;
            }
        }
        public static IList<CABSerialPort> ListOfSavedSerialPorts
        {
            get
            {
                return lstSavedSerialPorts;
            }
            set
            {
                lstSavedSerialPorts = value;
            }
        }
        string InitialModemBaudRate = string.Empty;
        string ModemConfigCOMPort = string.Empty;
        frmModemAutoConfigure objModemAutoConfigure = null;
        #endregion


        string[] CMRIIntercharacterDelay;
        string[] CMRICommandTimeOut;
        private System.Resources.ResourceManager resourceMgr;



        private bool STOPPHASOR = false;
        private const string CMRITYPE = "CMRIType";
        private const string CMRIINTERCHARACTERDELY = "CMRIIntercharacterDelay";
        private const string CMRICOMMANDTIMEOUT = "CMRICommandTimeOut";
        Thread thPhasor;

        private ApplicationType types;
        private int ctRatio = 0;
        private int ptRatio = 0;
        private bool PHASORRUNNING = false;
        private string READMODE = string.Empty;

        private string ReadOutMode
        {
            get
            {
                if (string.IsNullOrEmpty(READMODE))
                {
                    return cmbMode.Text.Trim();
                }
                else
                {
                    return READMODE;
                }
            }
            set
            {
                READMODE = value;
            }
        }
        public DLMSMain()
        {

            InitializeComponent();
            /*Enable Phasor Read in Normal Mode using FD command */
            if (UtilityDetails.ShowPhasorFastDownloadInNormalMode || UtilityDetails.ReadPhasorNormalMode)
            {
                chkPhasor.Visible = true;
                chkPhasor.Checked = true;
            }
            /*Enable Phasor Read in Normal Mode using FD command */
            chkMidnightData.Visible = true;
            commAborted = false;
            respTOURead = "";
            touGridNames = new DataGridView[] {gridTOUDay1,gridTOUDay2, gridTOUDay3, gridTOUDay4, gridTOUDay5, gridTOUDay6, gridTOUDay7, 
                                        gridTOUDay8, gridTOUDay9, gridTOUDay10, gridTOUDay11, gridTOUDay12, gridTOUDay13,
                                        gridTOUDay14, gridTOUDay15, gridTOUDay16, gridTOUDay17, gridTOUDay18, gridTOUDay19, 
                                        gridTOUDay20, gridTOUDay21, gridTOUDay22, gridTOUDay23, gridTOUDay24 };
            flgdataValid = true;
            touCommands = new string[30];
            errStr = "(ER20)";
            //Yatin 03-Jan-2012
            lstSavedSerialPorts = GetSavedSerialPorts();
            FillDurationComboBox();
            resourceMgr = new System.Resources.ResourceManager("DLMS_Final.Utility.UtilityMessage", System.Reflection.Assembly.GetExecutingAssembly());

            // Added 


            if (UtilityDetails.ShowGPRSCommunication)
            {
                rdGPRS.Visible = true;
            }
            else
            {
                rdGPRS.Visible = false;
            }
        }


        private void FillDurationComboBox()
        {
            // To fill the duration combobox.
            for (int i = 1; i < 61; i++)
            {
                cmbTestduration.Items.Add(i);
            }
            cmbTestduration.SelectedIndex = 0;
        }
        private void FillDisplayParameters(DataGridView dGVPushDisplayParams, string paramType)
        {
            //define dataset object
            DataSet ds = null;
            //define xml data document object
            XmlDataDocument xmlDatadoc = null;

            try
            {
                //assign memory to xml data document object
                xmlDatadoc = new XmlDataDocument();

                //serialize the xml data 

                xmlDatadoc.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + UtilityDetails.PrimaryUtlityName + "_DisplayParameters.xml");


                //assign memory to dataset object and name it "alerts"
                ds = new DataSet();

                //deserialize xml data
                ds = xmlDatadoc.DataSet;

                //ds.Tables[0].Columns.Add(new DataColumn("colInclude"));
                //set datasource of gridview
                dGVPushDisplayParams.DataSource = ds.DefaultViewManager;
                // dGVScrollDisplayParams.DataSource = ds.DefaultViewManager;
                // dGVHighResolution.DataSource = ds.DefaultViewManager;

                //specify grdiview datamember
                if (paramType == "PUSH")
                {
                    dGVPushDisplayParams.DataMember = "DisplayParams";
                }
                else if (paramType == "SCROLL")
                {
                    dGVPushDisplayParams.DataMember = "DisplayParams1";
                }
                else if (paramType == "HIGHRESOLUTION")
                {
                    dGVPushDisplayParams.DataMember = "HighResolution";
                }

                DataGridViewColumn col1 = new DataGridViewCheckBoxColumn();
                col1.Name = "colInclude";
                col1.HeaderText = "Include";

                DataGridViewColumn col2 = new DataGridViewCheckBoxColumn();
                col2.Name = "colInclude";
                col2.HeaderText = "Include";

                DataGridViewColumn col3 = new DataGridViewCheckBoxColumn();
                col3.Name = "colInclude";
                col3.HeaderText = "Include";

                if (!dGVPushDisplayParams.Columns.Contains("colInclude"))
                {
                    dGVPushDisplayParams.Columns.Insert(dGVPushDisplayParams.Columns.Count, col1);
                }
                //if (!dGVScrollDisplayParams.Columns.Contains("colInclude"))
                //{
                //  dGVScrollDisplayParams.Columns.Add(col2);
                //}
                //if (!dGVHighResolution.Columns.Contains("colInclude"))
                //{
                //    dGVHighResolution.Columns.Add(col3);
                //}


                dGVPushDisplayParams.Columns[0].Width = 80;
                dGVPushDisplayParams.Columns[1].Width = 130;
                dGVPushDisplayParams.Columns[2].Width = 300;
                dGVPushDisplayParams.Columns[3].Width = 85;

                //dGVScrollDisplayParams.Columns[0].Width = 80;
                //dGVScrollDisplayParams.Columns[1].Width = 130;
                //dGVScrollDisplayParams.Columns[2].Width = 300;
                //dGVScrollDisplayParams.Columns[3].Width = 85;

                //dGVHighResolution.Columns[0].Width = 80;
                //dGVHighResolution.Columns[1].Width = 130;
                //dGVHighResolution.Columns[2].Width = 300;
                //dGVHighResolution.Columns[3].Width = 95;

                dGVPushDisplayParams.Columns["SNO"].ReadOnly = true;
                dGVPushDisplayParams.Columns["ID"].ReadOnly = true;
                dGVPushDisplayParams.Columns["Description"].ReadOnly = true;


                //dGVScrollDisplayParams.Columns[0].ReadOnly = true;
                //dGVScrollDisplayParams.Columns[1].ReadOnly = true;
                //dGVScrollDisplayParams.Columns[2].ReadOnly = true;

                //dGVHighResolution.Columns[0].ReadOnly = true;
                //dGVHighResolution.Columns[1].ReadOnly = true;
                //dGVHighResolution.Columns[2].ReadOnly = true;
                //dGVPushDisplayParams.Show();

            }
            catch (Exception ex)
            {
                //exception catched...show a message
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //dispose and free memory occupied by objects
                ds.Dispose();
            }
        }

        /// <summary>
        /// Used to Connect Modem using its default config by reading modem.cfg      
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public bool SwitchModemConfigToDefaultConfig(CommunicationType communicationType)
        {
            bool success = false;
            string result = string.Empty;
            try
            {
                GlobalObjects.objSerialComm.DLMSRetries = DLMSRetryForModemCommands;
                //Piyush : Fill modem config details
                //Piyush : pick the At commands from the same file according to the modem type
                FillModemConfigDetails(AppDomain.CurrentDomain.BaseDirectory + "modem.xml", communicationType.ToString());
                SystemSettingsBLL objSystemSettingsBLL = new SystemSettingsBLL();
                if (objSystemSettingsBLL.GetSettingValue("USE_MULTIPLE_PORTS") == "1")
                    SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("CMRI_COM_PORT");
                else
                    SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("COM_PORT");
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                //Piyush : Set the serial port to the utility config so that serial port can communicate to the local modem
                SerialPortSettings.Default.CommandTimeOut = 6000;
                SerialPortSettings.Default.IntercharacterDelay = 6000;
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, this.baudRate, this.parity, this.dataBit, this.stopBit, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                //Piyush : If intialization commands are present in modem.xml
                toolstripStatus.Text = "Initializing local modem, ";
                Application.DoEvents();
                if (initCommands != null && initCommands.Length > 0)
                {
                    //Bring Modem to desired baud rate according to the .xml configured
                    for (int counter = 0; counter < initCommands.Length; counter++)
                    {
                        GlobalObjects.objSerialComm.bCommType = 1;
                        result = fSendModemCommand(initCommands[counter]);
                        if (initCommands[counter].Equals("ATH") || initCommands[counter].Equals("ATE") || initCommands[counter].Equals("AT"))
                        {
                            if (result.ToUpper().Contains("OK"))
                            {
                                success = true;
                            }
                            else
                            {
                                success = false;
                                toolstripStatus.Text = "Local modem not connected to PC or not configured properly.";
                                Application.DoEvents();
                                break;
                            }

                        }
                        else
                        {
                            success = true;
                        }
                    }
                    //right now it's fixed when responsae mapping will be implemented, then remove this line.

                    //Bring serial port to default baud rate
                    GlobalObjects.objSerialComm.SetSerialPortSettings("9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                }
                if (success)
                {
                    toolstripStatus.Text = "Connecting remote modem..";
                    Application.DoEvents();
                    if (!string.IsNullOrEmpty(this.dial))
                    {
                        GlobalObjects.objSerialComm.InterchatracterDelay = 70000;
                        GlobalObjects.objSerialComm.CommandTimeout = 75000;
                        GlobalObjects.objSerialComm.bCommType = 2;
                        result = fSendModemCommand(this.dial);
                        if (result.ToUpper().Contains("CONNECT"))
                        {
                            toolstripStatus.Text = "Remote modem connected.";
                            Application.DoEvents();
                            success = true;
                        }
                        else
                        {
                            this.StatusMessage = result;
                            success = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            finally
            {
                GlobalObjects.objSerialComm.DLMSRetries = DLMSRetryForMeterCommands;
                toolstripStatus.Text = string.Empty;
                Application.DoEvents();
                if (success)
                {
                    SerialPortSettings.Default.CommandTimeOut = this.configuredTimeOut;
                    SerialPortSettings.Default.IntercharacterDelay = this.configuredIntercharacterDelay;
                }
                else
                {
                    //GlobalObjects.objSerialComm.ClosePort();
                }
                GlobalObjects.objSerialComm.bCommType = 0;
            }
            return success;

        }
        /// <summary>
        /// 
        /// </summary>
        public void LeaveModemToUtilityConfig()
        {
            bool hasCommands = false;
            //Piyush set timeout for direct communication.
            SerialPortSettings.Default.CommandTimeOut = 6000;
            SerialPortSettings.Default.IntercharacterDelay = 5500;
            try
            {
                //Set dlms retries to modem commands
                GlobalObjects.objSerialComm.DLMSRetries = DLMSRetryForModemCommands;
                if (resetCommands != null && resetCommands.Length > 0)
                {
                    hasCommands = true;

                    foreach (string command in resetCommands)
                    {
                        GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                        GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                        GlobalObjects.objSerialComm.bCommType = 1;
                        GlobalObjects.objSerialComm.timeout = SerialPortSettings.Default.CommandTimeOut;
                        fSendModemCommand(command);
                    }
                    //Close port
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                GlobalObjects.objSerialComm.DLMSRetries = DLMSRetryForMeterCommands;
                if (hasCommands)
                {
                    GlobalObjects.objSerialComm.ClosePort();
                }
                GlobalObjects.objSerialComm.bCommType = 0x00;

            }
        }
        /// <summary>
        /// Get modem config name from communication Type
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        private ModemConfigProperties GetModemConfigProperties(ModemConfig config, string communicationType)
        {
            ModemConfigProperties configMName = null;
            foreach (ModemConfigProperties modemConfig in config.Items)
            {
                if (modemConfig.Name == communicationType)
                {
                    configMName = modemConfig;
                    break;
                }
            }
            return configMName;
        }
        /// <summary>
        /// Reads Modem config details into objects 
        /// </summary>
        public void FillModemConfigDetails(string modemCfgFilePath, string communicationType)
        {
            CABSerializer apiConfiguration = new CABSerializer();
            string reConnectCommand = string.Empty;
            string initCommand = string.Empty;
            string resetCommand = string.Empty;
            //Deserialize the AT command information from xml to object
            this.modemConfig = (ModemConfig)apiConfiguration.DeserializeToObject(modemCfgFilePath, typeof(ModemConfig));
            if (this.modemConfig != null)
            {
                //Piyush Get modem config name object
                ModemConfigProperties modemConfigProperties = GetModemConfigProperties(modemConfig, communicationType);
                if (modemConfigProperties != null)
                {
                    //Read Command settings section
                    if (modemConfigProperties.Commandsettings != null && modemConfigProperties.Commandsettings.Length > 0)
                    {
                        initCommand = modemConfigProperties.Commandsettings[0].Initialize;
                        resetCommand = modemConfigProperties.Commandsettings[0].Reset;
                        reConnectCommand = modemConfigProperties.Commandsettings[0].ReConnect;
                        if (!string.IsNullOrEmpty(initCommand))
                        {
                            this.initCommands = initCommand.Split('|');

                        }
                        if (!string.IsNullOrEmpty(resetCommand))
                        {
                            this.resetCommands = resetCommand.Split('|');
                        }
                        //Dialing command 
                        // this.dial = modemConfigModemNameCommandsettings[0].Dial;                    
                        this.dial = modemConfigProperties.Commandsettings[0].Dial + "0" + SimSelectForm.SimNumber;
                        this.configuredIntercharacterDelay = modemConfigProperties.Commandsettings[0].InterCharacterTimeout;
                        this.configuredTimeOut = modemConfigProperties.Commandsettings[0].CommandTimeout;
                        //Piyush : setting dlms retries to be used.
                        this.dLMSRetryForMeterCommands = modemConfigProperties.Commandsettings[0].DLMSRetries;
                    }
                    //Read port settings section
                    if (modemConfigProperties.Portsettings != null && modemConfigProperties.Portsettings.Length > 0)
                    {
                        this.baudRate = modemConfigProperties.Portsettings[0].BitsPerSecond;
                        this.parity = modemConfigProperties.Portsettings[0].Parity;
                        this.stopBit = modemConfigProperties.Portsettings[0].Stopbits;
                        this.dataBit = modemConfigProperties.Portsettings[0].Databits;
                    }
                }
            }
        }
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;
                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }
        public byte fGetQueryDailySurveyProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x63;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;// 0x97;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        private byte fReadDailyProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = fGetQueryDailySurveyProfile(HDLCCommand, HDLCIndex, atb);

                //added by dhirendra for Selective Access By Range
                if (atb == 0x02)
                {
                    if (rdBtnReadBetween.Checked == true)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, dtPickerFrom.Value, dtPickerTo.Value);
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //'******************************************************************************
        //'  NAME     : fCheckHDLCResponse
        //'  INPUT    : none
        //'  OUTPUT   : true or False
        //'  PURPOSE  : Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        //'*******************************************************************************
        private bool fCheckHDLCResponse(byte[] Buffer)
        {
            fIncrementTimer();

            if (GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.ClosePort();
                MessageBox.Show("Invalid Start or end Tag", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            else
            {
                if (GlobalObjects.objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    this.Cursor = Cursors.Default;
                    GlobalObjects.objSerialComm.ClosePort();
                    MessageBox.Show("Invalid HDLC FCS", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
                else
                {
                    if (GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, SerialPortSettings.Default.ClientSAP) == false)
                    {
                        this.Cursor = Cursors.Default;
                        GlobalObjects.objSerialComm.ClosePort();
                        MessageBox.Show("Invalid Destination Address ", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                    else
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte) == false)
                        {
                            this.Cursor = Cursors.Default;
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Invalid Response Byte ", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        //'******************************************************************************
        //'  NAME     : fSendAARQ
        //'  INPUT    : none
        //'  OUTPUT   : true or False
        //'  PURPOSE  : Send AARQ packet and Recieve and Check AARE response
        //'*******************************************************************************
        private bool fSendAARQ()
        {
            try
            {
                //Change Needed
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = 0x00;
                cnfBlock[1] = 0x12;
                cnfBlock[2] = 0x1A;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fSetInitialI();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                if (SerialPortSettings.Default.SecurityMechanism == 0x01)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x36);
                else
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x1D);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddContext(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ApplicationContext);
                if (SerialPortSettings.Default.SecurityMechanism == 0x01)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, SerialPortSettings.Default.SecurityMechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPassword(HDLCCommand, HDLCIndex, SerialPortSettings.Default.Password);
                }
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddUserInf(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddCnfBlock(HDLCCommand, HDLCIndex, cnfBlock);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPDUSize(HDLCCommand, HDLCIndex, SerialPortSettings.Default.PDUSize);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        if (GlobalObjects.objCOSEMLIB.fCheckAARQResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private String fSendModemCommand(String command)
        {
            try
            {
                const string Discription = "+++";
                String CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }
                //Piyush : If the command is not equal to Discription only then add 0D.
                if (!command.Equals(Discription))
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');
                }
                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    return "Modem Time Out.";
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private String fSendModemCommand(String command, String Number)
        {
            try
            {
                String CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                for (int i = 0; i < Number.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToByte(Number.Substring(i, 1)) + 0x30);
                }

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');

                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private String fSendDisc()
        {
            try
            {
                String CommandResult = "";
                MODEMIndex = 0;
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    //////Application.DoEvents();
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region SystemMenuFunction
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FillDLMSInterfaceGrid()
        {
            try
            {
                ds = new DataSet();
                XPathNavigator nav;
                XPathDocument docNav;
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\" + "DLMSInterfaceClass.xml";

                string tempStr = string.Empty;

                DataTable tabInterfaceClass = new DataTable("tabInterfaceClass");
                tabInterfaceClass.Columns.Add("classID");
                tabInterfaceClass.Columns.Add("Name");
                tabInterfaceClass.Columns.Add("AttributeCount");

                DataTable tabAttributeClass = new DataTable("tabAttributeClass");
                tabAttributeClass.Columns.Add("transID");
                tabAttributeClass.Columns.Add("classID");
                tabAttributeClass.Columns.Add("className");
                tabAttributeClass.Columns.Add("AttributeID");
                tabAttributeClass.Columns.Add("AttributeName");

                DataTable tabOBISClass = new DataTable("tabOBISClass");
                tabOBISClass.Columns.Add("transID");
                tabOBISClass.Columns.Add("classID");
                tabOBISClass.Columns.Add("className");
                tabOBISClass.Columns.Add("OBISParamCode");
                tabOBISClass.Columns.Add("OBISParamName");

                ds.Tables.Add(tabInterfaceClass);
                ds.Tables.Add(tabAttributeClass);
                ds.Tables.Add(tabOBISClass);

                //Open the XML
                docNav = new XPathDocument(path);

                nav = docNav.CreateNavigator();

                nav.MoveToRoot();
                nav.MoveToFirstChild();
                nav.MoveToFirstChild();

                do
                {
                    //Find the first element.
                    if (nav.NodeType == XPathNodeType.Element)
                    {
                        if (nav.Name == "LTCT_3Phase")
                        {
                            //Determine whether children exist.
                            if (nav.HasChildren == true)
                            {
                                //Move to the first child.
                                nav.MoveToFirstChild();

                                //Loop through all of the children.
                                do
                                {
                                    //Check for Class attributes.
                                    if (nav.Name == "Class" && nav.HasAttributes == true)
                                    {
                                        DataRow row = tabInterfaceClass.NewRow();

                                        row["classID"] = nav.GetAttribute("ID", nav.NamespaceURI);
                                        row["Name"] = nav.GetAttribute("Name", nav.NamespaceURI);
                                        row["AttributeCount"] = nav.GetAttribute("AttributeCount", nav.NamespaceURI);

                                        tabInterfaceClass.Rows.Add(row);

                                        //Check for Attribute node and OBIS node
                                        if (nav.HasChildren == true)
                                        {
                                            //Move to the first child.
                                            nav.MoveToFirstChild();
                                            do
                                            {
                                                if (nav.HasAttributes == true)
                                                {

                                                    if (nav.Name == "Attribute")
                                                    {
                                                        DataRow rowTemp = tabAttributeClass.NewRow();

                                                        rowTemp["transID"] = 1;
                                                        rowTemp["classID"] = row["classID"];
                                                        rowTemp["className"] = row["Name"];
                                                        rowTemp["AttributeID"] = nav.GetAttribute("ID", nav.NamespaceURI);
                                                        rowTemp["AttributeName"] = nav.GetAttribute("Description", nav.NamespaceURI);
                                                        tabAttributeClass.Rows.Add(rowTemp);
                                                    }
                                                    else if (nav.Name == "OBIS")
                                                    {
                                                        DataRow rowTemp = tabOBISClass.NewRow();

                                                        rowTemp["transID"] = 1;
                                                        rowTemp["classID"] = row["classID"];
                                                        rowTemp["className"] = row["Name"];
                                                        rowTemp["OBISParamCode"] = ServiceClass.ServiceInstance.ConvertObisCode(nav.GetAttribute("ParamCode", nav.NamespaceURI), 16);
                                                        rowTemp["OBISParamName"] = nav.GetAttribute("ParamName", nav.NamespaceURI);
                                                        tabOBISClass.Rows.Add(rowTemp);
                                                    }
                                                }
                                                //DataRow rowTemp = tabTamperCondition.NewRow();
                                            } while (nav.MoveToNext());
                                        }
                                        nav.MoveToParent();
                                    }
                                } while (nav.MoveToNext());
                            }
                        }//end of D5 if condition
                    }
                } while (nav.MoveToNext());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        #region Abhay Form Load
        TOUPresenter touPresenter;
        DLMSConnection dlmsConnection;
        /// <summary>
        /// This method is used to Bind all the grids on the form load.
        /// </summary>
        private void BindTOU()
        {
            if (gridTOUDay1.Rows.Count == 0 && gridDayTables.RowCount == 0)
            {
                touPresenter.BindTOUGridCoulmun();
            }
            //BhardwjG: If utility is puma and is not ndpl
            if (UtilityDetails.ShowOneTOU)
            {
                TableLayoutColumnStyleCollection styles;
                tabControl2.Controls.Remove(tabPageSeason2);
                tabControl2.Controls.Remove(tabPageSeason3);
                tabControl2.Controls.Remove(tabPageSeason4);

                for (int gridCount = 1; gridCount <= touGridNames.GetUpperBound(0); gridCount++)
                {
                    touGridNames[gridCount].Visible = false;
                }

                lblDayTable6.Visible = false;
                lblDayTable5.Visible = false;
                lblDayTable4.Visible = false;
                lblDayTable3.Visible = false;
                lblDayTable2.Visible = false;
                gridDayTables.Height = 50;
                gridDayTables.Enabled = false;
                gridActivationDate.Height = 60;
                gridActivationDate.Enabled = false;
                gridTOUDay1.Height = 300;
                gridTOUDay1.Width = 300;
                gridTOUDay1.Columns[0].Width = 50;
                gridTOUDay1.Columns[1].Width = 80;
                gridTOUDay1.Columns[2].Width = 80;
                gridTOUDay1.Columns[3].Width = 80;
                grpDayTables.Width = 320;
                tabControl2.Width = 350;
                tableLayoutPanel16.ColumnCount = 3;
                styles = this.tableLayoutPanel16.ColumnStyles;
                btnFillTOUConfiguration.Visible = false;

                foreach (ColumnStyle style in styles)
                {
                    style.SizeType = SizeType.AutoSize;
                }
            }
            else if (UtilityDetails.ShowTwoTOU)
            {
                tabControl2.Controls.Remove(tabPageSeason2);
                tabControl2.Controls.Remove(tabPageSeason3);
                tabControl2.Controls.Remove(tabPageSeason4);

                for (int gridCount = 2; gridCount <= touGridNames.GetUpperBound(0); gridCount++)
                {
                    touGridNames[gridCount].Visible = false;
                }

                lblDayTable6.Visible = false;
                lblDayTable5.Visible = false;
                lblDayTable4.Visible = false;
                lblDayTable3.Visible = false;

            }

        }

        private void BindCMRI()
        {
            string[] CMRITypes = ConfigurationManager.AppSettings.Get(CMRITYPE).Split('|');
            CMRIIntercharacterDelay = ConfigurationManager.AppSettings.Get(CMRIINTERCHARACTERDELY).Split('|');
            CMRICommandTimeOut = ConfigurationManager.AppSettings.Get(CMRICOMMANDTIMEOUT).Split('|');
            cmbCMRIType.Items.AddRange(CMRITypes);
            cmbCMRIType.SelectedIndex = 0;

            SerialPortSettings.Default.IntercharacterDelay = Convert.ToInt32(CMRIIntercharacterDelay[0]);
            SerialPortSettings.Default.CommandTimeOut = Convert.ToInt32(CMRICommandTimeOut[0]);
            SerialPortSettings.Default.Save();
        }
        private void BindPorts()
        {
            bool isMTMType = false;
            SerialComm objSerialComm = new SerialComm();
            string[] PortNames = objSerialComm.GetAvailablePorts();
            Array.Reverse(PortNames);
            List<string> lstAllPorts = null;
            if (PortNames.Length > 0)
            {
                lstAllPorts = new List<string>(PortNames);
                lstAllPorts.Sort();
            }

            string strGSMModemPorts = objSystemSettings.GetSettingValue(SystemSettings.GSM_COM_PORTS);
            string strCMRIPort = objSystemSettings.GetSettingValue(SystemSettings.CMRI_COM_PORT);
            string message = string.Empty;
            bool areMultiplePortsPresent = false;
            if (PortNames.Length > 1)
            {
                areMultiplePortsPresent = true;
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("CommunicationType")))
            {
                if (System.Configuration.ConfigurationManager.AppSettings.Get("CommunicationType").ToString().ToLower() == "otm")
                {
                    isMTMType = false;
                }
                else if (System.Configuration.ConfigurationManager.AppSettings.Get("CommunicationType").ToString().ToLower() == "mtm")
                {
                    isMTMType = true;
                }
                else
                {
                    isMTMType = false;
                }
            }
            for (int i = 0; i < PortNames.Length; i++)
            {
                cmbAvailableSerialPort.Items.Add(PortNames[i]);

                if (areMultiplePortsPresent)
                {
                    if (("," + strGSMModemPorts + ",").Contains("," + lstAllPorts[i] + ","))
                    {
                        dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], true, false);
                    }
                    else if (strCMRIPort.Equals(lstAllPorts[i]))
                    {
                        dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], false, true);
                    }
                    else
                    {
                        dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], false, false);
                    }
                }
            }

            rdbSinglePort.CheckedChanged -= rdbSinglePort_CheckedChanged;
            if (!areMultiplePortsPresent)
            {
                rdbMultiplePorts.Enabled = false;
                ToolTip ttMultiplePorts = new ToolTip();
                ttMultiplePorts.SetToolTip(gbPort, "Multiple Ports option disabled\nas this computer doesn't have\nmore than one COM Port.");
            }
            if (IsMultiplePortSelected && areMultiplePortsPresent && isMTMType)
            {
                rdbMultiplePorts.Checked = true;
                dgvPortUsageAssociation.Visible = true;
                groupBox65.Visible = false;
                btnTestConnection.Visible = true;
                btnTestConnection.Enabled = false;
                isConnectionTested = true;
            }
            else
            {
                rdbSinglePort.Checked = true;
                dgvPortUsageAssociation.Visible = false;
                groupBox65.Visible = true;
                btnTestConnection.Visible = false;
            }
            rdbSinglePort.CheckedChanged += rdbSinglePort_CheckedChanged;

            DefaultPortName = cmbAvailableSerialPort.Text = SerialPortSettings.Default.SerialPort;
        }
        private void BindSettings()
        {
            txtBoxInterFrameTime.Text = Convert.ToString(SerialPortSettings.Default.IntercharacterDelay);
            txtResponseTimeOut.Text = Convert.ToString(SerialPortSettings.Default.CommandTimeOut);
            txtServerSAP.Text = Convert.ToString(SerialPortSettings.Default.ServerSAP);
            txtServerLowerMacAddress.Text = Convert.ToString(SerialPortSettings.Default.ServerLowerMacAddress);
            txtPWD.Text = Convert.ToString(SerialPortSettings.Default.Password);

            txtGSMInterFrameTime.Text = Convert.ToString(SerialPortSettings.Default.InterframeTimeout);
            txtBoxScaleXML.Text = SerialPortSettings.Default.ScaleXMLPath;
            txtHLSPwd.Text = SerialPortSettings.Default.HLSKey;

            textBoxGSM.Text = SerialPortSettings.Default.ModemNumber;

            //Set Combo box settings based on serial port settings.
            SetCommandModeText();

            for (int i = 1; i <= 100; i++)
            {
                cmbBoxLastFromEvent.Items.Add(i.ToString());
                cmbBoxFromEvent.Items.Add(i.ToString());
                cmbBoxToEvent.Items.Add(i.ToString());
            }
            cmbBoxLastFromEvent.SelectedIndex = 0;
            cmbBoxFromEvent.SelectedIndex = 0;
            cmbBoxToEvent.SelectedIndex = 0;

            cmbBoxLastFromEvent.Enabled = false;
            cmbBoxFromEvent.Enabled = false;
            cmbBoxToEvent.Enabled = false;

            cmbBoxLastFrom.SelectedIndex = 0;
            cmbBoxFrom.SelectedIndex = 0;
            cmbBoxTo.SelectedIndex = 0;

            grpPartialRead.Enabled = false;
            btnLoadList.Enabled = true;
            btnLoadMeterFD.Enabled = true;
            btnReadAll.Enabled = false;
            btnFDRead.Enabled = false;

            cmbBoxFrom.Enabled = false;
            cmbBoxLastFrom.Enabled = false;
            dtPickerFrom.Enabled = false;
            dtPickerTo.Enabled = false;

            SetButtonMode(SerialPortSettings.Default.ClientSAP);



            if (cmbMode.Text == " MR ")
            {
                tempPageUS1 = tabProgramming;
                tabControlMain.TabPages.Remove(tabProgramming);


                tempPageUS2 = tabCMRI;
                tabControlMain.TabPages.Remove(tabCMRI);

                tabpageExistUS = false;
                SetMRModeSettings(cmbMode.Text);
            }
            //vr.017 PUMA 23/02/2012
            else if (cmbMode.Text == " US ")
            {
                byte count = 1;
                if (tabpageExist == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPage);
                    tabpageExist = true;
                }
                else
                {
                    count = 2;
                }
                if (tabpageExistUS == false)
                {
                    tabControlMain.TabPages.Insert(count++, tabProgramming);
                    tabControlMain.TabPages.Insert(count++, tempPageUS2);
                    tabpageExistUS = true;
                }

                SetUSModeSettings(cmbMode.Text);

            }
            //vr.017 PUMA 23/02/2012
            else if (cmbMode.Text == " US ")
            {
                byte count = 1;
                if (tabpageExist == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPage);
                    tabpageExist = true;
                }
                else
                {
                    count = 2;
                }
                if (tabpageExistUS == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPageUS1);
                    tabControlMain.TabPages.Insert(count++, tempPageUS2);
                    tabpageExistUS = true;
                }

                //For US mode MD Reset should not visible                
                tabCTPTRatio.TabPages.Remove(tabMDReset);

                //If Dynamic phasor is not available for utility the remove it.
                if (!UtilityDetails.ShowDynamicPhasorTab)
                {
                    tabControlMain.TabPages.Remove(tabPhasor);
                }

            }
            //Added for FS Mode. Functionality will be same as US Mode. Some additional tab will be added.
            else if (cmbMode.Text == " FS ")
            {
                byte count = 1;
                if (tabpageExist == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPage);
                    tabpageExist = true;
                }
                else
                {
                    count = 2;
                }
                if (tabpageExistUS == false)
                {
                    tabControlMain.TabPages.Insert(count++, tabProgramming);
                    tabControlMain.TabPages.Insert(count++, tempPageUS2);
                    tabpageExistUS = true;
                }
                SetFSModeSettings(cmbMode.Text);

            }

            else
            {
                tempPage = tabPage1;
                tabControlMain.TabPages.Remove(tabPage1);
                tabpageExist = false;

                tempPageUS1 = tabProgramming;
                tabControlMain.TabPages.Remove(tabProgramming);

                tempPageUS2 = tabCMRI;
                tabControlMain.TabPages.Remove(tabCMRI);

                tabpageExistUS = false;
                SetPCModeSettings(cmbMode.Text);

            }
            //Set If Mode is not FS then apply settings
            SetModeNotFSSettings(cmbMode.Text);
        }


        /// <summary>
        /// Set Combox box based on serial port settings.
        /// </summary>
        private void SetCommandModeText()
        {

            //Set Context combo
            if (SerialPortSettings.Default.ApplicationContext == 0x01)
            {
                cmbContext.Text = "Long Name [LN]";
            }

            if (SerialPortSettings.Default.SecurityMechanism == 0x00)
            {
                cmbSecurity.Text = "No Security";
            }
            else if (SerialPortSettings.Default.SecurityMechanism == 0x01)
            {
                cmbSecurity.Text = "Low-Level";
            }
            else
            {
                cmbSecurity.Text = "High-Level";
            }

            //Set Command Mode Combo box
            if (SerialPortSettings.Default.ClientSAP == 0x10)
            {
                cmbMode.Text = " PC ";
            }
            else if (SerialPortSettings.Default.ClientSAP == 0x20)
            {
                cmbMode.Text = " MR ";
            }
            else if (SerialPortSettings.Default.ClientSAP == 0x30)
            {
                cmbMode.Text = " US ";
            }
            else if (SerialPortSettings.Default.ClientSAP == 0x40)
            {
                cmbMode.Text = " FS ";
            }
            else
            {
                cmbMode.Text = " PC ";
            }
        }

        private void Bind()
        {
            /* VBM - Add Midnight data feature */
            if (CoreUtility.IsMVVNL)
            {
                //chkMidnightData.Visible = true;
                //chkMidnightData.Checked = true;
                //chkCMRIMidnightData.Visible = true;
                //chkCMRIMidnightData.Location = new System.Drawing.Point(35, 244);
                //chkCMRISelectAll.Location = new System.Drawing.Point(35, 284);
                //chkCMRIMidnightData.Checked = true;
                isMVVNL = true;
            }
            else if (CoreUtility.IsPUMA)
            {
                //chkMidnightData.Visible = true;
                //chkMidnightData.Checked = true;
                //chkCMRIMidnightData.Visible = true;
                //chkCMRIMidnightData.Location = new System.Drawing.Point(35, 244);
                //chkCMRISelectAll.Location = new System.Drawing.Point(35, 284);
                //chkCMRIMidnightData.Checked = true;
                //Piyush: Set this boolean if puma if the core utility is puma
                isPUMA = true;
            }
            if (UtilityDetails.ShowMidnight)
            {
                chkMidnightData.Visible = true;
                chkMidnightData.Checked = true;
                chkCMRIMidnightData.Visible = true;
                chkCMRIMidnightData.Location = new System.Drawing.Point(35, 244);
                //chkCMRISelectAll.Location = new System.Drawing.Point(35, 284);
                chkCMRIMidnightData.Checked = true;
            }
            else
            {
                chkMidnightData.Visible = false;
                chkCMRIMidnightData.Visible = false;
                chkMidnightData.Checked = false;
                chkCMRIMidnightData.Checked = false;
            }
            /* VBM - Add Midnight data feature */
            /* VBM - Add CMRI phasor  feature */
            if (UtilityDetails.ShowPhasorInCMRINormalMode)
            {
                chkCMRIPhasor.Checked = true;
                chkCMRIPhasor.Visible = true;
            }
            else
            {
                chkCMRIPhasor.Checked = false;
                chkCMRIPhasor.Visible = false;
            }
            /* VBM - Add CMRI phasor  feature */

            //VBM - Make fast download option configurable.
            if (UtilityDetails.ShowFastDownLoad)
            {
                /* GKG 13/02/2013 JDVVNL Utility Addition */
                //rdFastDownload.Visible = true;
                if (UtilityDetails.PrimaryUtlityName == "JDVVNL")
                {
                    rdFastDownload.Visible = false;
                }
                else
                {
                    rdFastDownload.Visible = true;
                }
                /* GKG 13/02/2013 JDVVNL Utility Addition */


            }
            else
            {
                //Piyush : Remove CMRI fast download from CMRI screen
                tabControlCMRI.TabPages.Remove(tabPage2);
                rdFastDownload.Visible = false;
                //tabControlMain.TabPages.Remove(tabMeterAccuracyCheck);
                tabControlMain.TabPages.Remove(tabPhasor);

            }
            //VBM - Meter Accuracy Check  feature wise 
            if (!UtilityDetails.ShowMeterAccuracyCheck)
            {
                tabControlMain.TabPages.Remove(tabMeterAccuracyCheck);
            }

            if (!CoreUtility.IsMPKWCL)
            {
                tabControlMain.TabPages.Remove(tabPCBA);
            }
            else
            {
                lblPCBAMeterID.Visible = false;
                lblDisplayMeterId.Visible = false;
            }
            this.toolStripStatusLabel1.Text = string.Empty;

            if (DLMSMain.RunningInstance() != null)
            {
                this.Close();
            }

            try
            {
                dtPickerFrom.Value = Convert.ToDateTime(dtPickerFrom.Value.ToShortDateString() + " 00:00:00");
                dtPickerTo.Value = Convert.ToDateTime(dtPickerTo.Value.ToShortDateString() + " 23:59:59");
                BindTOU();
                timerRTC.Start();
                BindCMRI();
                BindPorts();
                BindSettings();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Update UI with GSM configuration stored in config file
        /// </summary>
        private void UpdateGSMControls()
        {
            //Piyush : Remove GSM tab page
            if (tabControlMain.TabPages.Contains(tabGSM))
            {
                tabControlMain.TabPages.Remove(tabGSM);
            }
            if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
            {
                panelCommunicationType.Visible = true;
                CommunicationType communicationType = CommunicationTypeDetail.GetCommunicationType();



                if (communicationType == CommunicationType.DIRECT)
                {
                    rdDirect.Checked = true;
                }
                else if (communicationType == CommunicationType.GSM)
                {
                    rdGSM.Checked = true;
                }
                else if (communicationType == CommunicationType.PSTN)
                {
                    rdPSTN.Checked = true;
                }
                else if (communicationType == CommunicationType.GPRS)
                {
                    rdGPRS.Checked = true;
                    GlobalObjects.objSerialComm.SetSerialPortSettings(communicationType.ToString());
                }

                //If GSM is not enabled and only gprs is enabled then hide gsm and pstn radio buttons.
                if (!UtilityDetails.EnableGSMCommunication)
                {
                    rdGSM.Visible = false;
                    rdPSTN.Visible = false;
                }
            }
            else
            {
                panelCommunicationType.Visible = false;
            }
        }
        private void DLMSMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (SerialPortSettings.Default.CommunicationType == CommunicationType.GSM.ToString()
                || SerialPortSettings.Default.CommunicationType == CommunicationType.PSTN.ToString()
                    //Condition added for GPRS. IF mode is GPRS then also add LS capture days.
                || SerialPortSettings.Default.CommunicationType == CommunicationType.GPRS.ToString())
                {
                    cmbLSDays.Visible = true;
                    cmbLSDays.SelectedItem = SerialPortSettings.Default.LPReadDays.ToString();
                }
                else
                {
                    cmbLSDays.Visible = false;
                }
                //VBM - Add Message to use that ruby meter has no phasor .
                if (UtilityDetails.ShowMeterModelNo)
                {
                    string message = "* Phasor is not supported in " + NamePlateConstants.RubyE250 + " meter.";
                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.UPCONTRACTORS.ToString())
                    {
                        message = "* Phasor is not supported in " + NamePlateConstants.RubyE250 + "/Cortex" + " meter.";
                    }
                    lblPhasorNotSupported.Visible = true;
                    lblPhasorNotSupported.Text = message;
                }
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString())
                {
                    lblKvahNotSupported.Visible = true;
                    lblKvahNotSupported.Text = "* Kvah selection is not supported in " + NamePlateConstants.RubyE250 + " meter.";
                }

                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.UPCONTRACTORS.ToString())
                {
                    lblCTRatioMessage.Visible = true;
                    lblCTRatioMessage.Text = "* Programming CT ratio is not supported in " + NamePlateConstants.RubyE250 + "/Cortex" + " meter.";
                    lblPTProgramNotSupported.Visible = true;
                    lblPTProgramNotSupported.Text = "* Programming PT ratio is not supported in " + NamePlateConstants.RubyE250 + "/Cortex" + " meter.";
                }
                //TOU name changes for one and two tou
                if (UtilityDetails.ShowTwoTOU)
                {
                    tabPageSeason1.Text = "TOD Detail";
                    lblDayTable1.Text = "Season 1";
                    lblDayTable2.Text = "Season 2";

                }
                else if (UtilityDetails.ShowOneTOU)
                {
                    tabPageSeason1.Text = "TOD Detail";
                    lblDayTable1.Text = "Season";

                }

                lblNoMeterAccuracyCheck.Visible = true;
                lblNoMeterAccuracyCheck.Text = "* Meter accuracy check is not supported in " + NamePlateConstants.RubyE250 + " meter.";
                //Applying fix unit value for power incase of EXCEL POWER utility 
                //This is temprorary code and will be rmoved once standerd solution will be found .
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.EXCELPOWER.ToString())
                {
                    lblActivePower.Text = "Active Power(MW):";
                    lblReactivePower.Text = "Reactive Power(Mvar):";
                    lblApparentPower.Text = "Apparent Power(MVA):";
                    lblRPhaseKWDir.Text = "R Phase MW Direction:";
                    lblYPhaseKWDir.Text = "Y Phase MW Direction:";
                    lblBPhaseKWDir.Text = "B Phase MW Direction:";
                }

                //Piyush : Update constrols with GSM related functionality
                UpdateGSMControls();
                touPresenter = new TOUPresenter(this);
                cmriPresenter = new CMRIPresenter(this);
                dlmsConnection = new DLMSConnection();
                CoreUtility.ObjectToolStripProgressBar = toolStripProgressBar1;
                Bind();
                CoreUtility.ObjectToolStripProgressBar = this.toolStripProgressBar1;
                /* VBM - Make Midnight read feature configurable */
                if (UtilityDetails.ShowMidnight)
                {
                    if (chkNormalDownload.Checked)
                    {
                        chkMidnightData.Visible = true;
                        if (chkOther.Checked)
                        {
                            chkMidnightData.Checked = true;
                        }
                    }

                }
                /* VBM - Make Midnight read feature configurable */
                /*GKG 02/12/2013 PT RATIO CHANGES*/
                if (UtilityDetails.ShowTwoBytePTRatio)
                {
                    lblPTRatio.Text = "(1-320)";
                    nudPTRatio.Maximum = 320;
                }
                else
                {
                    lblPTRatio.Text = "(1-100)";
                    nudPTRatio.Maximum = 100;
                }
                /*GKG 02/12/2013 PT RATIO CHANGES*/
                //if (cmbMode.Text.ToLower().Contains("fs"))
                //{
                //    if (UtilityDetails.ShowDisplayParameters)
                //    {
                //        FillDisplayParameters();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        #endregion

        private void DLMSMain_Activated(object sender, EventArgs e)
        {
            UpdateToolStripStatus();
        }

        private void UpdateToolStripStatus()
        {
            toolStripStatusLblSettings.Text = SerialPortSettings.Default.SerialPort;
            if (SerialPortSettings.Default.ClientSAP == 0x10)
                toolStripStatusLblSettings.Text += ", " + " PC ";
            else if (SerialPortSettings.Default.ClientSAP == 0x20)
                toolStripStatusLblSettings.Text += ", " + " MR ";
            else if (SerialPortSettings.Default.ClientSAP == 0x30)
                toolStripStatusLblSettings.Text += ", " + " US ";
            else if (SerialPortSettings.Default.ClientSAP == 0x40)
                toolStripStatusLblSettings.Text += ", " + " FS ";
            if (UtilityDetails.EnableGSMCommunication)
            {
                toolStripStatusLblSettings.Text += " , " + SerialPortSettings.Default.CommunicationType;
            }
            this.toolstripStatus.Text = "";
        }

        private void tabPageHighResolution_Enter(object sender, EventArgs e)
        {
            try
            {
                CheckAndUpdateSelectAll(dGVHighResolution);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
        private void dGVHighResolution_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
        {
            if (dGVHighResolution.IsCurrentCellDirty)
            {
                dGVHighResolution.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dGVScrollDisplayParams_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
        {
            if (dGVScrollDisplayParams.IsCurrentCellDirty)
            {
                dGVScrollDisplayParams.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dGVPushDisplayParams_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
        {
            if (dGVPushDisplayParams.IsCurrentCellDirty)
            {
                dGVPushDisplayParams.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dGVPushDisplayParams_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            CheckAndUpdateSelectAll(dGVPushDisplayParams);
        }
        private void dGVScrollDisplayParams_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            CheckAndUpdateSelectAll(dGVScrollDisplayParams);
        }
        private void dGVHighResolution_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            CheckAndUpdateSelectAll(dGVHighResolution);
        }

        private void CheckAndUpdateSelectAll(DataGridView dGVHighResolution)
        {
            bool isSelected = false;
            foreach (DataGridViewRow row in dGVHighResolution.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colInclude"].Value) != true)
                {
                    isSelected = false;
                    break;
                }
                else if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value) == true)
                {
                    isSelected = true;
                }
            }
            chkBoxSelectAll.CheckedChanged -= new EventHandler(chkBoxSelectAll_CheckedChanged);
            if (isSelected == false)
            {
                chkBoxSelectAll.Checked = false;
            }
            else
            {
                chkBoxSelectAll.Checked = true;
            }
            chkBoxSelectAll.CheckedChanged += new EventHandler(chkBoxSelectAll_CheckedChanged);
        }

        private void tabPageScrollButton_Enter(object sender, EventArgs e)
        {
            try
            {
                CheckAndUpdateSelectAll(dGVScrollDisplayParams);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }


        private void tabPagePushButton_Enter(object sender, EventArgs e)
        {
            try
            {
                CheckAndUpdateSelectAll(dGVPushDisplayParams);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
        /// <summary>
        /// This method is used to read midnight data.
        /// </summary>
        /// <param name="wr1"></param>
        /// <returns></returns>
        private bool ReadMidnightData(StreamWriter wr1)
        {
            bool bSuccess = true;
            if (chkCMRIMidnightData.Checked)
            {
                #region Midnight Data
                SerialPortSettings.Default.ServerSAP = 0x01;
                byte ret = fReadMidnightDataProfile(3);
                if (ret == 0x01)
                {
                    string strTemp = string.Empty;
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    wr1.WriteLine("06" + strTemp);
                }
                else if (ret == 0x07)
                {
                    //write an empty line so that parser can predict that nothing in this line should be read
                    wr1.WriteLine("06");
                }
                else if (ret == 0x05)
                {
                    StopTimer();
                    bSuccess = false;
                    MessageBox.Show(COMMessages.ACCESSDENIED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    btnReadAllCMRI.Enabled = true;
                    btnCMRICancel.Enabled = true;
                    return bSuccess;
                }
                ret = fReadMidnightDataProfile(2);
                if (ret == 0x01)
                {
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    wr1.Write("06");
                    for (int i = 0; i < length; i++)
                    {
                        wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                    }
                    wr1.WriteLine("");
                }
                else if (ret == 0x07)
                {
                    wr1.WriteLine("06");
                }
                else if (ret == 0x05)
                {
                    StopTimer();
                    MessageBox.Show(COMMessages.ACCESSDENIED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    bSuccess = false;
                    btnReadAllCMRI.Enabled = true;
                    btnCMRICancel.Enabled = true;//cancel button change
                    return bSuccess;
                }

                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                ret = ReadScalarProfile(3, 6);
                if (ret == 0x01)
                {
                    String strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    wr1.WriteLine("06" + strTemp);
                }
                else if (ret == 0x07)
                {
                    wr1.WriteLine("06");
                }
                else
                {
                    StopTimer();

                    MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    bSuccess = false;
                    btnReadAllCMRI.Enabled = true;
                    btnCMRICancel.Enabled = true;
                    return bSuccess;
                }

                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                ret = ReadScalarProfile(2, 6);
                if (ret == 0x01)
                {
                    String strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    wr1.WriteLine("06" + strTemp);

                }
                else if (ret == 0x07)
                {
                    wr1.WriteLine("06");
                }
                else
                {
                    StopTimer();

                    MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    bSuccess = false;
                    btnReadAllCMRI.Enabled = true;
                    btnCMRICancel.Enabled = true;
                    return bSuccess;
                }
                #endregion
                chkCMRIMidnightData.Enabled = true;
            }
            else
            {
                for (byte x = 0; x < 4; x++)
                    wr1.WriteLine("06");
            }
            return bSuccess;
        }
        private void DisplaySAPListFastDownload(byte[] Blockdata)
        {
            DisplaySAPList(Blockdata);
            //string data = string.Empty;
            //int capture_object_definition;
            //int i, j = 0;
            //int nLength = 0;
            //int nByteIndex = 0;
            //string meterID = string.Empty;
            //nByteIndex++;       // Array 01
            //capture_object_definition = Blockdata[nByteIndex];
            //for (i = 0; i < capture_object_definition; i++)
            //{
            //    nByteIndex++;
            //    nByteIndex++;

            //    nByteIndex++;
            //    nByteIndex++;
            //    nByteIndex++;

            //    nByteIndex++;
            //    nByteIndex++;
            //    nLength = Blockdata[nByteIndex++];
            //    // length 06
            //    data = "";
            //    for (j = 0; j < nLength; j++)
            //    {
            //        data = data + Convert.ToChar(Blockdata[j + nByteIndex]);
            //    }
            //    nByteIndex = nByteIndex + (j - 1);
            //    //09 0C 07 DA 0B 1D FF 0B 30 13 FF 80 00 00

            //    nByteIndex++;
            //    nByteIndex++;
            //    nByteIndex++;

            //    int year = 0;// receivedData[21];
            //    year = (year | (int)Blockdata[nByteIndex++]) << 8;
            //    year = (year | (int)Blockdata[nByteIndex++]);
            //    int month = Blockdata[nByteIndex++];
            //    int date = Blockdata[nByteIndex++];
            //    int week = Blockdata[nByteIndex++];
            //    int hour = Blockdata[nByteIndex++];
            //    int minute = Blockdata[nByteIndex++];
            //    int second = Blockdata[nByteIndex++];
            //    if (Blockdata[nByteIndex] == 253)
            //    {
            //        data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + week.ToString("d2") + ":" + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2") + ".FD";
            //        lstFast.Items.Add(data, true);
            //    }
            //    nByteIndex++;
            //    nByteIndex++;
            //    nByteIndex++;
            //    //nByteIndex++;

            //}
        }

        private bool fReadCMRIInstant(string strFileName, string FileMeterdata, StreamWriter wr1, FileStream file1)
        {
            fIncrementTimer();
            try
            {
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                Application.DoEvents();
                #region Common Readout
                byte ret = fReadInastantaneous(3);
                fIncrementTimer();
                if (ret == 0x01) //// instantaneous scaler capture objecct
                {
                    //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    ///00000041_11_06_10_06_26_12
                    string strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strTemp = "01" + strTemp;
                    wr1.WriteLine(strTemp);
                }
                else if (ret == 0x07)
                {

                    wr1.WriteLine("010100");
                }
                else
                {
                    StopTimer();
                    MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    wr1.Close();
                    file1.Close();
                    System.IO.File.Delete(strFileName);
                    btnFDRead.Enabled = true;
                    DLMSMain.fDLMSDisconnect();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    return false;
                }
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                ret = fReadInastantaneous(2);
                if (ret == 0x01) //// instantaneous scaler capture objecct
                {
                    //FillDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    //fApplyScalarUnit();
                    String strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strTemp = "01" + strTemp;
                    wr1.WriteLine(strTemp);
                }
                else if (ret == 0x07)
                {

                    wr1.WriteLine("010100");
                }
                else
                {
                    StopTimer();
                    MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    wr1.Close();
                    file1.Close();
                    System.IO.File.Delete(strFileName);
                    btnFDRead.Enabled = true;
                    DLMSMain.fDLMSDisconnect();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    return false;
                }
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                ret = ReadScalarProfile(3, 0);
                if (ret == 0x01) //// instantaneous scaler capture objecct
                {
                    //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    //fApplyScalarUnit();
                    String strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strTemp = "01" + strTemp;
                    wr1.WriteLine(strTemp);
                }
                else if (ret == 0x07)
                {

                    wr1.WriteLine("010100");
                }
                else
                {
                    StopTimer();

                    MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    wr1.Close();
                    file1.Close();
                    System.IO.File.Delete(strFileName);
                    btnFDRead.Enabled = true;
                    DLMSMain.fDLMSDisconnect();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    return false;
                }
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                ret = ReadScalarProfile(2, 0);
                if (ret == 0x01) //// instantaneous scaler capture objecct
                {
                    //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    //fApplyScalarUnit();
                    String strTemp = "";
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strTemp = "01" + strTemp;
                    wr1.WriteLine(strTemp);
                }
                else if (ret == 0x07)
                {
                    wr1.WriteLine("010100");
                }
                else
                {
                    StopTimer();

                    MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    wr1.Close();
                    file1.Close();
                    System.IO.File.Delete(strFileName);
                    btnFDRead.Enabled = true;
                    DLMSMain.fDLMSDisconnect();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    return false;
                }
                #endregion
                #region Instant for PUMA
                if (isPUMA)
                {
                    //added PUMA
                    #region CU-MD-KW
                    btnFDRead.Enabled = false;
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    Application.DoEvents();

                    //for getting Data
                    byte retval1 = fReadCumulativeKW(2);
                    if (retval1 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        // To solve DLMS_0074 
                        int startIndex = 0;
                        // Receive buffer[18] tells the datatype , 0x06 means long int.
                        if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                        {
                            wr1.WriteLine("010100");
                        }
                        else
                        {
                            // added if readout is not successful.
                            StopTimer();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCancelFD.Enabled = true; //cancel button change
                            return false;
                        }
                        //length = nBlockIndex;

                    }
                    //fix - Ashish 04/10/11
                    else if (retval1 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01" + "00000000");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        btnCancelFD.Enabled = true; //cancel button change
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    //for getting scalar unit
                    retval1 = ReadScalarProfile(3, 4);
                    if (retval1 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (retval1 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        btnCancelFD.Enabled = true; //cancel button change
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    #endregion

                    //added PUMA
                    #region CU-MD-KVA
                    btnFDRead.Enabled = false;
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    Application.DoEvents();

                    //for getting Data
                    byte retval2 = fReadCumulativeKVA(2);
                    if (retval2 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        ///00000041_11_06_10_06_26_12
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        // To solve DLMS_0074 
                        int startIndex = 0;
                        // Receive buffer[18] tells the datatype , 0x06 means long int.
                        if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                        {
                            wr1.WriteLine("010100");
                        }
                        else
                        {
                            // added if readout is not successful.
                            StopTimer();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCancelFD.Enabled = true; //cancel button change
                            return false;
                        }
                        //length = nBlockIndex;

                    }
                    //fix - Ashish 04/10/11
                    else if (retval2 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01" + "00000000");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        btnCancelFD.Enabled = true; //cancel button change
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    //for getting scalar unit
                    retval2 = ReadScalarProfile(3, 5);
                    if (retval2 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (retval2 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        btnCancelFD.Enabled = true; //cancel button change
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    #endregion
                }
                #endregion
        #endregion


            }
            catch (Exception ex)
            {

            }

            return true;

        }

        private bool fReadCMRIGeneral(string strFileName, string FileMeterdata, StreamWriter wr1, FileStream file1)
        {

            try
            {
                #region Nameplate
                Application.DoEvents();

                int iIndex = 0;

                int nObjectCount = 0;
                iIndex = 0;
                // ShowIndex = 1;
                nObjectCount = 7;//2;
                //nObjectCount = dGVGeneralReadout.Rows.Count;
                while (iIndex < nObjectCount)
                {

                    int ret = Initialize_ReadMeterID(iIndex);
                    if (ret == 0x01)
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                        {
                            this.Cursor = Cursors.Default;

                            MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            wr1.Close();
                            file1.Close();
                            System.IO.File.Delete(strFileName);
                            btnReadAll.Enabled = true;
                            DLMSMain.fDLMSDisconnect();
                            SerialPortSettings.Default.ServerSAP = 0x01;
                            return false;
                        }
                        else
                        {

                            //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                            int length = 0;
                            int startIndex = 0;
                            String strTemp = "";
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                            {
                                length = 2;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                            {
                                length = 1;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                            {
                                length = 4;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                            {
                                length = 8;
                                startIndex = 19;

                            }
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            strTemp = "05" + strTemp;
                            wr1.WriteLine(strTemp);
                            //fDLMSConnect();
                        }
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("050100");
                    }
                    else if (ret == 0x00)
                    {
                        this.Cursor = Cursors.Default;

                        MessageBox.Show("CMRI Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnReadAll.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    else
                    {
                        //do not display message
                        this.Cursor = Cursors.Default;
                        //DLMSMain.fDLMSDisconnect();
                        //MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        break;
                    }
                    iIndex++;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
            return true;
        }
        /// <summary>
        /// This function creates the command for fastdownloading and call methods for read general,instant and fast downloading.
        /// </summary>
        /// <param name="ldn"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool fReadCMRIFileForFastdownload(int serverSAP, int ldn, out string message)
        {

            toolStripProgressBar1.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            SerialPortSettings.Default.ServerSAP = serverSAP;
            bool communicationTimeOut = false;
            message = string.Empty;
            // to call again the default constructor for dlms.
            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            if (DLMSMain.fDLMSConnect() != true)
            {
                StopTimer();
                return false;
            }
            //fIncrementTimer();
            //Application.DoEvents();
            String FileMeterdata;
            string lngFilename = string.Empty;
            string meterID = string.Empty;
            string commandCMRIRead = string.Empty;
            string fastdownloadCMRICommand = string.Empty;
            string fileName = string.Empty;
            fastdownloadCMRICommand = "FD";

            byte[] cmdFD = new byte[25];
            string strFileName = string.Empty;
            lstFast.SetSelected(ldn - 2, true);
            int meterIDIndex = lstFast.Text.Length - 26;
            if (meterIDIndex < 7 || meterIDIndex > 16)
                return false;

            string tempMID = string.Empty;
            String tempStr = lstFast.Text.Substring(0, meterIDIndex);
            strFileName = strFileName + tempStr;
            #region create command// Crating the command
            foreach (char c in tempStr)
            {
                int tmp = c;
                fastdownloadCMRICommand += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));

            }
            fastdownloadCMRICommand += ',';

            if (lstFast.Text.Contains(".FD"))
            {
                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + "_" + String.Format("{0:00}", DateTime.Now.Millisecond) + ".FDL";
            }
            int tyear = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 7, 4));
            string temp = string.Empty;
            fastdownloadCMRICommand += tyear.ToString("X4");

            int tmonth = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 4, 2));
            fastdownloadCMRICommand += tmonth.ToString("X2");

            int tdate = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 1, 2));
            fastdownloadCMRICommand += tdate.ToString("X2");

            int tweek = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 12, 2));
            fastdownloadCMRICommand += tweek.ToString("X2");

            int thour = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 15, 2));
            fastdownloadCMRICommand += thour.ToString("X2");

            int tmin = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 18, 2));
            fastdownloadCMRICommand += tmin.ToString("X2");

            int tsec = Convert.ToInt16(lstFast.Text.Substring(meterIDIndex + 21, 2));
            fastdownloadCMRICommand += tsec.ToString("X2");

            fastdownloadCMRICommand += "FD800000";
            string[] cmd = new string[3] { "7E", "A0", "2D" };

            byte[] fdCommand = new byte[cmd.Length];
            for (int j = 0; j < cmd.Length; j++)
                fdCommand[j] = Convert.ToByte(cmd[j], 16);



            #endregion

            DateTime dumpdate = new DateTime(tyear, tmonth, tdate, thour, tmin, tsec);
            string mID = Convert.ToString(meterIDIndex);
            while (mID.Length < 2) { mID = "0" + mID; }
            FileMeterdata = mID + tempStr + String.Format("{0:0000}", dumpdate.Year) + String.Format("{0:00}", dumpdate.Month) + String.Format("{0:00}", dumpdate.Day) + String.Format("{0:00}", dumpdate.Hour) + String.Format("{0:00}", dumpdate.Minute) + String.Format("{0:00}", dumpdate.Second);

            FileStream file1 = new FileStream(strFileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);
            #region Read
            try
            {
                btnCancelFD.Enabled = false;
                wr1.WriteLine("00" + FileMeterdata);
                //fIncrementTimer();
                if (strFileName.Contains(".FDL"))
                {
                    //fReadCMRIInstant(strFileName, FileMeterdata, wr1, file1)
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("02");              //writing Line breaks for no data
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("03");
                    for (byte x = 0; x < 24; x++)
                        wr1.WriteLine("04");
                    //fReadCMRIGeneral(strFileName, FileMeterdata, wr1, file1)

                    this.StatusMessage = "Read Out in progress";
                    //fIncrementTimer();
                    //Application.DoEvents();
                    DLMSMain.fDLMSDisconnect();
                    GlobalObjects.objSerialComm.ClosePort();
                    byte index = Convert.ToByte(fdCommand.Length);
                    /*GKG Meter ID dyanamic changes*/
                    fdCommand[2] = (byte)((fastdownloadCMRICommand.Length) + 2);
                    /*GKG Meter ID dyanamic changes*/
                    int res = fReadCMRIFD(fdCommand, index, fastdownloadCMRICommand);
                    fIncrementTimer();
                    Application.DoEvents();
                    if (res == 0)
                    {
                        toolStripProgressBar1.Visible = false;
                        wr1.Close();
                        file1.Close();

                        //Added to calculate checksum.
                        String strChecksum = GetMD5ChecksumForFile(strFileName);
                        FileStream file2 = new FileStream(strFileName, FileMode.Append);
                        StreamWriter wr2 = new StreamWriter(file2);
                        wr2.WriteLine(strChecksum);
                        wr2.Close();
                        file2.Close();
                        bool resultStatus = fGetdownloadedData(strFileName, out fileName, tempStr);
                        if (resultStatus)
                        {
                            return true;

                        }
                        else
                        {
                            File.Delete(strFileName);
                            btnFDRead.Enabled = true;
                            btnCancelFD.Enabled = true;
                            return false;
                        }
                    }
                    else
                    {
                        this.StatusMessage = string.Empty;
                        MessageBox.Show("Time Out", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnFDRead.Enabled = true;
                        return false;

                    }


                }

                wr1.Close();
                file1.Close();
                StopTimer();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                StopTimer();
                Application.DoEvents();
                toolStripProgressBar1.Visible = false;
                DLMSMain.fDLMSDisconnect();
                btnCancelFD.Enabled = true;
                GlobalObjects.objSerialComm.ClosePort();
                wr1.Close();
                file1.Close();
                this.Cursor = Cursors.Default;
            }
            #endregion
            return true;
        }
        /// <summary>
        /// This method is used to get the downloaded data from the buffer and create the file.
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="fileName"></param>
        /// <param name="meterID"></param>
        /// <returns></returns>
        public bool fGetdownloadedData(string strFileName, out string fileName, string meterID)
        {
            fIncrementTimer();
            Application.DoEvents();
            string downloadedData = string.Empty;
            fileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"FDLFILES\");
            StreamWriter streamWriter = null;
            FileStream fileStream = null;
            try
            {
                StreamReader streamCABFile = new StreamReader(strFileName);
                string lngFileData = streamCABFile.ReadToEnd();
                streamCABFile.Close();
                downloadedData = BitConverter.ToString(GlobalObjects.objSerialComm.fastDownloadBufferCMRI).Replace("-", "");
                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrEmpty(downloadedData))
                {
                    for (int i = 0; i < downloadedData.Length; i += 2)
                    {

                        String temp = downloadedData.Substring(i, 2);
                        builder.Append(System.Convert.ToChar(System.Convert.ToUInt32(temp, 16)).ToString());
                        fIncrementTimer();
                        Application.DoEvents();
                    }
                }
                String completeDownloadedData = builder.ToString();
                fIncrementTimer();
                Application.DoEvents();
                if (completeDownloadedData.Contains("ENDOFFILE"))
                {
                    completeDownloadedData = completeDownloadedData.Substring(0, (completeDownloadedData.IndexOf("ENDOFFILE") + 9) - 9);
                }
                else
                {
                    return false;
                }
                completeDownloadedData = lngFileData + completeDownloadedData;
                if (!Directory.Exists(fileName))
                    Directory.CreateDirectory(fileName);
                //Create file name.
                fileName = fileName + meterID + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".FDL";
                fileStream = new FileStream(fileName, FileMode.Create);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(completeDownloadedData);


            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (streamWriter != null && fileStream != null)
                {
                    streamWriter.Close();
                    fileStream.Close();
                }
                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }
                fIncrementTimer();
                Application.DoEvents();
            }
            return true;
        }


        public bool fReadCMRFile(int ldn, int itemToSelect)
        {
            StartTimer();
            SerialPortSettings.Default.ServerSAP = ldn;
            // Added to increase the time out in case of excessive data.
            SerialPortSettings.Default.CommandTimeOut = 9000;
            SerialPortSettings.Default.IntercharacterDelay = 7000;
            if (DLMSMain.fDLMSConnect() != true)
            {
                StopTimer();
                return false;
            }
            fIncrementTimer();

            String strTamperScalecapture = "";
            String strTamperScalebuffer = "";
            String strFileName;
            String FileMeterdata;
            string lngFilename = string.Empty;
            string meterID = string.Empty;
            string commandCMRIRead = string.Empty;
            strFileName = AppDomain.CurrentDomain.BaseDirectory;
            //strFileName = SerialPortSettings.Default.ScaleXMLPath + "\\";

            lstCMRIfile.SetSelected(itemToSelect - 2, true);

            int meterIDIndex = lstCMRIfile.Text.Length - 20;

            if (meterIDIndex < 7 || meterIDIndex > 16)
                return false;


            String tempStr = lstCMRIfile.Text.Substring(0, meterIDIndex);
            strFileName = strFileName + tempStr;

            strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + "_" + String.Format("{0:00}", DateTime.Now.Millisecond) + ".2NG";

            ///////////////////////////////////////////////////////////////////////

            int tyear = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 7, 4));
            int tmonth = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 4, 2));
            int tdate = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 1, 2));
            int thour = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 12, 2));
            int tmin = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 15, 2));
            int tsec = Convert.ToInt16(lstCMRIfile.Text.Substring(meterIDIndex + 18, 2));

            DateTime dumpdate = new DateTime(tyear, tmonth, tdate, thour, tmin, tsec);
            //VBM - Letgth needs to be reduced by 1, to make sure that both direct and cmri readout filed get uploaded in same way.
            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.SHYAMINDUS.ToString())
            {
                meterIDIndex = meterIDIndex - 1;
            }

            string mID = Convert.ToString(meterIDIndex);
            while (mID.Length < 2) { mID = "0" + mID; }
            FileMeterdata = mID + tempStr + String.Format("{0:0000}", dumpdate.Year) + String.Format("{0:00}", dumpdate.Month) + String.Format("{0:00}", dumpdate.Day) + String.Format("{0:00}", dumpdate.Hour) + String.Format("{0:00}", dumpdate.Minute) + String.Format("{0:00}", dumpdate.Second);

            FileStream file1 = new FileStream(strFileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);
            try
            {
                wr1.WriteLine("00" + FileMeterdata);
                fIncrementTimer();
                if (chkCMRIInstant.Checked == true)
                {


                    #region instantaneous
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    Application.DoEvents();

                    byte ret = fReadInastantaneous(3);
                    fIncrementTimer();
                    if (ret == 0x01) //// instantaneous scaler capture objecct
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        ///00000041_11_06_10_06_26_12
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("010100");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnReadAllCMRI.Enabled = true;
                        btnCMRICancel.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = fReadInastantaneous(2);
                    if (ret == 0x01) //// instantaneous scaler capture objecct
                    {
                        //FillDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("010100");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnReadAllCMRI.Enabled = true;
                        btnCMRICancel.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 0);
                    if (ret == 0x01) //// instantaneous scaler capture objecct
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("010100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 0);
                    if (ret == 0x01) //// instantaneous scaler capture objecct
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else if (ret == 0x07)
                    {
                        wr1.WriteLine("010100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    #endregion
                    if (UtilityDetails.ShowCumulativeMDKWKVA)
                    {
                        //added PUMA
                        #region CU-MD-KW
                        btnReadAll.Enabled = false;
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        Application.DoEvents();

                        //for getting Data
                        byte retval1 = fReadCumulativeKW(2);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }
                            // Added to check if no instant is read from cmri.
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                            {
                                wr1.WriteLine("010100");
                            }
                            else
                            {
                                // added if readout is not successful.
                                StopTimer();
                                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                btnCMRICancel.Enabled = true; //cancel button change
                                btnReadAllCMRI.Enabled = true;
                                return false;
                            }

                        }
                        //fix - Ashish 04/10/11
                        else if (retval1 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            StopTimer();
                            MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCMRICancel.Enabled = true; //cancel button change
                            btnReadAllCMRI.Enabled = true;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval1 = ReadScalarProfile(3, 4);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        //Added for NDPL Ruby to by-pass cumulative demand command
                        else if (retval1 == 0x07 || retval1 == 0x05)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCMRICancel.Enabled = true; //cancel button change
                            btnReadAllCMRI.Enabled = true;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion

                        //added PUMA
                        #region CU-MD-KVA
                        btnReadAll.Enabled = false;
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        Application.DoEvents();

                        //for getting Data
                        byte retval2 = fReadCumulativeKVA(2);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            ///00000041_11_06_10_06_26_12
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }

                                // Added to check if no instant is read from cmri.
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                            {
                                wr1.WriteLine("010100");
                            }
                            else
                            {
                                // added if readout is not successful.
                                StopTimer();
                                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                btnCMRICancel.Enabled = true;//cancel button change
                                btnReadAllCMRI.Enabled = true;
                                return false;
                            }
                        }
                        //fix - Ashish 04/10/11
                        else if (retval2 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCMRICancel.Enabled = true; //cancel button change
                            btnReadAllCMRI.Enabled = true;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval2 = ReadScalarProfile(3, 5);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        //Piyush : for NDPL ruby : By pass cumulative maximum demand
                        else if (retval2 == 0x07 || retval2 == 0x05)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            btnCMRICancel.Enabled = true; //cancel button change
                            btnReadAllCMRI.Enabled = true;
                            return false;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion
                    }
                    chkCMRIInstant.Enabled = true;
                    chkCMRIInstant.Enabled = true;

                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("01");              //writing Line breaks for no data
                }
                fIncrementTimer();
                if (chkCMRIBilling.Checked == true)
                {
                    #region Billing
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    byte ret = fReadBillingProfile(3);
                    fIncrementTimer();
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("020100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    //iIndex = 0;
                    ret = fReadBillingProfile(2);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("020100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("020100");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("020100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    #endregion
                    chkCMRIBilling.Enabled = true;

                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("02");              //writing Line breaks for no data
                }
                //VBM - Temp code - will be removed after CMRI imlementation
                if (chkCMRIBilling.Checked)
                {
                    wr1.WriteLine("02" + string.Format("{0:X2}", Convert.ToByte(13)));
                }

                if (chkCMRILoadSurvey.Checked == true)
                {
                    #region loadSurvey
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    byte ret = fReadLSProfile(3);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("030100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    //iIndex = 0;
                    ret = fReadLSProfile(2);
                    if (ret == 0x01)
                    {
                        //String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        wr1.Write("03");
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            //strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                        }
                        //wr1.WriteLine("03" + strTemp);
                        wr1.WriteLine("");

                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("030100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("030100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 2);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("030100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    #endregion
                    chkCMRILoadSurvey.Enabled = true;

                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("03");              //writing Line breaks for no data
                }
                fIncrementTimer();
                if (chkCMRITamper.Checked == true)
                {
                    #region EventLog
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    byte ret = fReadTamperProfile(3, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalecapture = strTemp;
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                        strTamperScalecapture = "040100";
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalebuffer = strTemp;
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                        strTamperScalebuffer = "040100";
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        System.IO.File.Delete(strFileName);
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);


                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 4);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 4);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 5);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 5);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else if (ret == 0x07)
                    {

                        wr1.WriteLine("040100");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    #endregion
                    chkCMRITamper.Enabled = true;

                }
                else
                {
                    for (byte x = 0; x < 24; x++)
                        wr1.WriteLine("04");              //writing Line breaks for no data
                }
                fIncrementTimer();
                if (chkCMRINameplate.Checked == true)
                {

                    #region Nameplate


                    SerialPortSettings.Default.ServerSAP = 0x01;
                    Application.DoEvents();

                    int iIndex = 0;

                    int nObjectCount = 0;
                    iIndex = 0;
                    // ShowIndex = 1;
                    if (UtilityDetails.ShowMeterModelNo && !UtilityDetails.ReadSignatureData)
                    {
                        nObjectCount = 8;//2;
                    }
                    else
                    {
                        nObjectCount = 7;
                    }
                    //nObjectCount = dGVGeneralReadout.Rows.Count;
                    while (iIndex < nObjectCount)
                    {

                        int ret = Initialize_ReadMeterID(iIndex);
                        if (ret == 0x01)
                        {
                            if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                            {
                                this.Cursor = Cursors.Default;

                                MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                wr1.Close();
                                file1.Close();
                                System.IO.File.Delete(strFileName);
                                btnCMRICancel.Enabled = true;
                                btnReadAllCMRI.Enabled = true;
                                DLMSMain.fDLMSDisconnect();
                                SerialPortSettings.Default.ServerSAP = 0x01;
                                return false;
                            }
                            else
                            {

                                //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                                int length = 0;
                                int startIndex = 0;
                                String strTemp = "";
                                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                                {
                                    length = 2;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                                {
                                    length = 1;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                                {
                                    length = 4;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                                {
                                    length = 8;
                                    startIndex = 19;

                                }
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("05" + strTemp);

                                //fDLMSConnect();
                            }
                        }
                        else if (ret == 0x07)
                        {

                            wr1.WriteLine("050100");
                        }
                        else if (ret == 0x00)
                        {
                            this.Cursor = Cursors.Default;

                            MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            wr1.Close();
                            file1.Close();
                            System.IO.File.Delete(strFileName);
                            btnCMRICancel.Enabled = true;
                            btnReadAllCMRI.Enabled = true;
                            DLMSMain.fDLMSDisconnect();
                            SerialPortSettings.Default.ServerSAP = 0x01;
                            return false;
                        }
                        else
                        {
                            //do not display message
                            this.Cursor = Cursors.Default;
                            //DLMSMain.fDLMSDisconnect();
                            //MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            break;
                        }
                        iIndex++;
                    }
                    #endregion
                    if (UtilityDetails.ShowMeterModelNo)
                    {
                        meterModelNumber = 0;
                        if (UtilityDetails.ReadSignatureData)
                        {

                            int writeResponse = ReadSignature();
                            if (writeResponse == 0x00)
                            {
                                string meterModel = Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[35]).ToString()
                                 + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[36]).ToString();
                                if (meterModel == "WC")
                                {
                                    meterModelNumber = NamePlateConstants.RubyE250Value;
                                }
                                else if (meterModel == "LT")
                                {
                                    meterModelNumber = NamePlateConstants.PumaLTE650Value;
                                }
                                else if (meterModel == "LC")
                                {
                                    meterModelNumber = NamePlateConstants.LTCTCortexValue;
                                }
                            }
                            wr1.WriteLine("05" + meterModelNumber.ToString("00"));
                        }
                    }
                    //chkCMRINameplate.Enabled = true;
                }
                else
                {
                    for (byte x = 0; x < 7; x++)
                        wr1.WriteLine("05");              //writing Line breaks for no data
                }
                //fAddFileName(strFileName.Substring(strFileName.Length - 32));

                if (UtilityDetails.ShowMidnight)
                {
                    // Make sure meter model number is not updated 
                    meterModelNumber = 0;
                    bool result = true;
                    if (chkCMRIMidnightData.Checked)
                    {
                        result = false;
                    }
                    if (isPUMA)
                    {
                        if (chkCMRIMidnightData.Checked)
                        {
                            ReadPUMAMidNightData(out result, wr1);
                        }
                        else
                        {
                            for (byte x = 0; x < 4; x++)
                                wr1.WriteLine("06");

                        }
                    }
                    else
                    {
                        result = ReadMidnightData(wr1);
                    }
                    if (!result)
                    {
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }
                }
                //Piyush : Read Anamoly for CMRI
                if (chkCMRIInstant.Checked && UtilityDetails.ShowAnamolyParameters && UtilityDetails.PrimaryUtlityName != "TNEB")
                {
                    if (UtilityDetails.ShowMeterModelNo) // For NDPL 
                    {
                        meterModelNumber = 0;
                        if (UtilityDetails.ShowMeterModelNo)
                        {
                            if (touModel.ReadMeterModelNumber() == 0x00)
                            {
                                meterModelNumber = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            }
                        }
                        ReadAnamolyParmaters(wr1, meterModelNumber);
                    }
                    else // For Remaining utility
                    {
                        ReadAnamolyParmaters(wr1);
                    }
                }

                /* GKG JVVNL Current TOU Read */
                if (chkCMRIBilling.Checked && UtilityDetails.ShowTouConfiguration)
                {
                    #region TOU
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    int ret = ReadTOU(5);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("08" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    /* VBM  "ret == 0x06" to fix CMRI issue when no TOU data in file */
                    else if (ret == 0x07 || ret == 0x06)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("08");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadTOU(4);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("08" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    /* VBM  "ret == 0x06" to fix CMRI issue when no TOU data in file */
                    else if (ret == 0x07 || ret == 0x06)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("08");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadTOU(3);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("08" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    /* VBM  "ret == 0x06" to fix CMRI issue when no TOU data in file */
                    else if (ret == 0x07 || ret == 0x06)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("08");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadRTC(2);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("08" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    /* VBM  "ret == 0x06" to fix CMRI issue when no TOU data in file */
                    else if (ret == 0x07 || ret == 0x06)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("08");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    #endregion
                }
                else
                {
                    wr1.WriteLine("08");              //writing Line breaks for no data
                }
                /* GKG JVVNL Current TOU Read */


                /* VBM  Read Phasor*/
                if (chkCMRIPhasor.Checked && UtilityDetails.ShowPhasorInCMRINormalMode)
                {
                    #region PHASOR
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    int ret = ReadPhasor(2);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("09" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    /* VBM  "ret == 0x06" to fix CMRI issue when no TOU data in file */
                    else if (ret == 0x07 || ret == 0x06)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("09");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show(COMMessages.CMRICONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        wr1.Close();
                        file1.Close();
                        System.IO.File.Delete(strFileName);
                        btnCMRICancel.Enabled = true;
                        btnReadAllCMRI.Enabled = true;
                        DLMSMain.fDLMSDisconnect();
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        return false;
                    }

                    #endregion
                }
                else
                {
                    wr1.WriteLine("09");              //writing Line breaks for no data
                }
                /* VBM  Read Phasor*/

                wr1.Close();
                file1.Close();
                btnReadAllCMRI.Enabled = true;
                btnCMRICancel.Enabled = true;
                btnLoadList.Enabled = true;
                btnReadAll.Enabled = true;
                DLMSMain.fDLMSDisconnect();



                ///calculating Check Sum 
                String strChecksum = GetMD5ChecksumForFile(strFileName);
                FileStream file2 = new FileStream(strFileName, FileMode.Append);
                StreamWriter wr2 = new StreamWriter(file2);
                wr2.WriteLine(strChecksum);
                wr2.Close();
                file2.Close();
                ///calculating Check Sum 
                ///
                StopTimer();
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                GlobalObjects.objSerialComm.ClosePort();
            }
        }

        public void SetButtonMode(int Mode)
        {
            if (Mode == 0x40)
            {
                btnReadAll.Enabled = true;
                btnLoadList.Enabled = false;
                btnLoadMeterFD.Enabled = false;
                btnFDRead.Enabled = true;
            }
            else if (Mode == 0x30)
            {
                btnReadAll.Enabled = true;
                btnLoadList.Enabled = false;
                btnLoadMeterFD.Enabled = false;
                btnFDRead.Enabled = false;
                btnStart.Enabled = true;
            }
            else if (Mode == 0x20)
            {
                btnReadAll.Enabled = true;
                btnLoadList.Enabled = true;
                btnLoadMeterFD.Enabled = true;
                btnFDRead.Enabled = false;
                btnStart.Enabled = true;
            }
            else if (Mode == 0x10)
            {
                btnReadAll.Enabled = false;
                btnLoadList.Enabled = false;
                btnLoadMeterFD.Enabled = false;
                btnFDRead.Enabled = false;
                btnStart.Enabled = false;
            }
            else
            {
                btnReadAll.Enabled = false;
                btnLoadList.Enabled = false;
                btnLoadMeterFD.Enabled = false;
                btnFDRead.Enabled = false;
            }
        }

        public static bool fDLMSConnectAccuracyCheck(out string connectionMessage)
        {
            connectionMessage = string.Empty;
            // Added to solve bug DLMS_0090
            SystemSettingsBLL objSystemSettingsBLL = new SystemSettingsBLL();
            if (objSystemSettingsBLL.GetSettingValue("USE_MULTIPLE_PORTS") == "1")
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("CMRI_COM_PORT");
            else
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("COM_PORT");

            try
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {

                    if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey) == true)
                    {
                        //MessageBox.Show("Connected.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return true;
                    }
                    else
                    {
                        connectionMessage = COMMessages.COSEMCONNECTIONFAILED;
                        GlobalObjects.objSerialComm.ClosePort();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
                else
                {

                    connectionMessage = COMMessages.HDLCCONNECTIONFAILED;
                    GlobalObjects.objSerialComm.ClosePort();
                    MessageBox.Show("HDLC Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();

                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public static bool fDLMSConnect()
        {

            // Added to solve bug DLMS_0090
            SystemSettingsBLL objSystemSettingsBLL = new SystemSettingsBLL();
            if (objSystemSettingsBLL.GetSettingValue("USE_MULTIPLE_PORTS") == "1")
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("CMRI_COM_PORT");
            else
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue("COM_PORT");

            try
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (CommunicationTypeDetail.GetCommunicationType() == CommunicationType.DIRECT)
                {
                    GlobalObjects.objSerialComm.OpenPort();
                }
                if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {

                    if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey) == true)
                    {
                        //MessageBox.Show("Connected.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return true;
                    }
                    else
                    {
                        fDLMSDisconnect();
                        GlobalObjects.objSerialComm.ClosePort();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    MessageBox.Show("HDLC Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                GlobalObjects.objSerialComm.ClosePort();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
        }

        public static void fDLMSDisconnect()
        {
            try
            {
                if (GlobalObjects.objGlobalFunctions.fSendDISC(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {
                    //MessageBox.Show("Disconnected.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    //MessageBox.Show("HDLC Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
                GlobalObjects.objSerialComm.ClosePort();
            }

            finally
            {

            }
        }

        private int fReadCMRIFD(byte[] HDLCCommand, byte HDLCIndex, string cmd)
        {
            fIncrementTimer();
            Application.DoEvents();
            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm(668628, true);
            GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", 6000, 5000);

            bool communicationTimeOut = false;
            GlobalObjects.objSerialComm.OpenPort();
            try
            {
                fIncrementTimer();
                Application.DoEvents();
                if (GlobalObjects.objSerialComm.fSendDataToPortCMRI(HDLCCommand, HDLCIndex, cmd, out communicationTimeOut) == false)
                {
                    if (communicationTimeOut)
                        return 1;
                }
                else
                {

                    return 0;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GlobalObjects.objSerialComm.ClosePort();

            }
            return 0;
        }

        private int fReadMeterSerialNumber()
        {
            try
            {
                fIncrementTimer();
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadMeterID(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 1;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                        return 1;
                }
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
        private byte fReadMeterAccuracyCheck(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Start and End tag for Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Start and End for HDLC
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Server Address
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Client Address
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryAccuracyCheckProfile(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /* GKG JVVNL Current TOU Read */
        /// <summary>
        /// This method is used for reading the TOU from the meter.
        /// </summary>
        /// <param name="attribute">Pleas pass the profile attribute which need to read from the meter.</param>
        /// <returns></returns>
        public int ReadTOU(byte attribute)
        {
            HDLCIndex = 0;
            HDLCCommand = new byte[200];
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadTOU(HDLCCommand, HDLCIndex, attribute);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

            try
            {

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed; ;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                        {
                                            break;
                                        }
                                        else if (ret == 0x02)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                    }
                                }
                            }

                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        //else if (ret == 0x05)
                        //{
                        //    return (int)CoreUtility.DLMSResultType.AccessDenied;
                        //}
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// VBM - Read phasor data 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public int ReadPhasor(byte attribute)
        {
            HDLCIndex = 0;
            HDLCCommand = new byte[200];
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPhasor(HDLCCommand, HDLCIndex, attribute);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

            try
            {

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed; ;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                        {
                                            break;
                                        }
                                        else if (ret == 0x02)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                    }
                                }
                            }

                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        //else if (ret == 0x05)
                        //{
                        //    return (int)CoreUtility.DLMSResultType.AccessDenied;
                        //}
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public int ReadRTC(byte attribute)
        {
            HDLCIndex = 0;
            HDLCCommand = new byte[200];
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0x00;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0x00;
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadRTC(HDLCCommand, HDLCIndex, attribute);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

            try
            {

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed; ;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                        {
                                            break;
                                        }
                                        else if (ret == 0x02)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                    }
                                }
                            }

                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        //else if (ret == 0x05)
                        //{
                        //    return (int)CoreUtility.DLMSResultType.AccessDenied;
                        //}
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* GKG JVVNL Current TOU Read */

        private byte fReadInastantaneous(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Start and End tag for Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Start and End for HDLC
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Server Address
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Client Address
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantProfile(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Read phasor normal 
        /// CESC 
        /// </summary>
        /// <param name="atb"></param>
        /// <returns></returns>
        private byte ReadPhasorNormalMode(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Start and End tag for Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Start and End for HDLC
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Server Address
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Client Address
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryPhasorProfile(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //added PUMA
        private byte fReadCumulativeKW(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKW(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        else
                        {
                            return 0x07;
                        }
                    }
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //added PUMA
        private byte fReadCumulativeKVA(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKVA(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        else
                        {
                            return 0x07;
                        }
                    }
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte ReadScalarProfile(byte atb, byte nProfileindex)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (nProfileindex == 0)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 1)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 2)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 3)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 4)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 5)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, atb);
                }
                //added for MVVNL
                else if (nProfileindex == 6)
                {
                    if (isPUMA)
                    {
                        if (meterModelNumber == NamePlateConstants.RubyE250Value)
                        {
                            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryMidnightDataScalarProfile(HDLCCommand, HDLCIndex, atb);
                        }
                        else
                        {
                            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryMidNightSacalarProfile(HDLCCommand, HDLCIndex, atb);
                        }
                    }
                    else
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryMidnightDataScalarProfile(HDLCCommand, HDLCIndex, atb);
                    }


                }
                //added for MVVNL
                // added for Accuracy Check
                else if (nProfileindex == 7)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryAccuracyCheckScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                //Added for CESC noraml phasor readout 
                else if (nProfileindex == 8)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryPhasorScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                // added for Accuracy Check
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte fReadBillingProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryBillingProfile(HDLCCommand, HDLCIndex, atb);

                //added by gopal for Selective Access By Entry
                if (atb == 0x02)
                {
                    if (rdBtnReadLast.Checked == true)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFrom.Text));

                    }
                    else if (rdBtnReadBetween.Checked == true)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFrom.Text), Convert.ToByte(cmbBoxTo.Text));
                    }

                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Converts hex to decimal
        /// </summary>
        /// <param name="dataInStringFormat"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public byte ConvertHexToDecimal(string dataInStringFormat, int dataIndex)
        {
            string data = dataInStringFormat.Substring(dataIndex, 2);
            return byte.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }


        /// <summary>
        /// This method converts the Date time values(Hexadecimal format) for a parameter into proper date time string
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string DateTimeValue)
        {
            int num = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Seconds = 0;
            DateTime dateTime;
            try
            {
                // Extracting the year value
                num += 4;
                string data = DateTimeValue.Substring(num, 4);
                Year = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
                num += 4;
                // Extracting the month value
                Month = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Day value
                Day = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 4;
                // Extracting the Hour value
                Hour = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Minutes value
                Minute = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Seconds value
                Seconds = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;

                dateTime = new DateTime(Year, Month, Day, Hour, Minute, Seconds);
            }
            catch
            {

                dateTime = System.DateTime.Now;
            }

            return dateTime;
        }
        private DateTime GetMeterDateTime()
        {
            DateTime meterDateTime = DateTime.Now;
            SerialPortSettings.Default.ServerSAP = 0x01;
            //iIndex = 0;
            int ret = ReadRTC(2);
            if (ret == 0x00)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                meterDateTime = GetDateTime(strTemp);

            }
            return meterDateTime;

        }
        private byte fReadLSProfile(byte atb)
        {
            try
            {
                DateTime meterDateTime = DateTime.Now;
                if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                {
                    meterDateTime = GetMeterDateTime();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                }
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryLoadSurveyProfile(HDLCCommand, HDLCIndex, atb);

                //added by dhirendra for Selective Access By Range
                if (atb == 0x02)
                {

                    if (rdBtnReadBetweenLS.Checked == true)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, dtPickerFrom.Value, dtPickerTo.Value);
                    }
                    else
                    {
                        if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                        {
                            int noOfDays = Convert.ToInt32(SerialPortSettings.Default.LPReadDays);
                            HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, meterDateTime.AddDays(-noOfDays), meterDateTime);
                        }
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //added for MVVNL
        private byte fReadMidnightDataProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryMidnightDataProfile(HDLCCommand, HDLCIndex, atb);


                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //added for MVVNL

        #region  SaveDataToFile
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 25 Feb 2012
        /// Purpose: Save the down loaded data in file.
        /// </summary>
        /// <param name="completeDownLoadedData"></param>
        private void SaveDataToFile(string completeDownLoadedData, string lngFileName, string meterID)
        {//Create file Name.(Directory +FileName).
            String fileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"FDLFILES\");
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                //Stream filelng = File.OpenRead(lngFileName);

                //  StreamExtensions.CopyTo(filelng, fileStream);
                //filelng.Close();

                StreamReader streamCABFile = new StreamReader(lngFileName);
                string lngFileData = streamCABFile.ReadToEnd();
                streamCABFile.Close();
                completeDownLoadedData = lngFileData + "FDLDATA\\" + completeDownLoadedData;
                //Encrypt the data.
                //completeDownLoadedData = ConfigInfo.EncryptFile(completeDownLoadedData);

                //If directory  not exists then create it.
                if (!Directory.Exists(fileName))
                    Directory.CreateDirectory(fileName);
                //Create file name.
                fileName = fileName + meterID + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".FDL";
                fileStream = new FileStream(fileName, FileMode.Create);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(completeDownLoadedData);

                //Show the name of the file in which downloaded data is stored.
                MessageBox.Show(resourceMgr.GetString("DataSaveIn") + fileName, " BCS");
            }
            catch (IOException)
            {//if there is any exception while storing the data in file the show the message.
                MessageBox.Show(resourceMgr.GetString("FileCreationError"));
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show(resourceMgr.GetString("Permissiondenied") + fileName);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
                completeDownLoadedData = string.Empty;
                File.Delete(lngFileName);
            }
        }
        #endregion

        #region ShowFastDownLoadValidationStatus
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 25 Feb 2012
        /// Purpose: Display the message according to status of operation.
        /// </summary>
        /// <param name="fastDownLoadStatus"></param>
        private void ShowFastDownLoadValidationStatus(FastDownLoadStatuses fastDownLoadStatus)
        {
            switch (fastDownLoadStatus)
            {
                //case FastDownLoadStatuses.BlankMeterID:
                //    MessageBox.Show(resourceMgr.GetString("BlankMeterID"), "BCS");
                //    txtMeterID.Focus();
                //   break;
                case FastDownLoadStatuses.ErrorInCommunication:
                    MessageBox.Show(resourceMgr.GetString("ErrorInCommunication"), "BCS");
                    break;
                //case FastDownLoadStatuses.IncorrectMeterID:
                //    MessageBox.Show(resourceMgr.GetString("IncorrectMeterID"), "BCS");
                //    txtMeterID.Focus();
                //    break;
                case FastDownLoadStatuses.NoOptionToDownLoad:
                    MessageBox.Show(resourceMgr.GetString("Selectanoption"), "BCS");
                    break;
            }
        }
        #endregion

        private void UpdateFastDownloadStatus(string statusMessage)
        {
            toolstripStatus.Text = statusMessage;// resourceMgr.GetString("DataReadoutStarted");
            Application.DoEvents();
        }
        //Thread demoThread;
        //delegate void SetTextCallback(string text);


        //private void SetDataDownloadStatus(string statusMessage)
        //{
        //    //toolstripStatus.Text = statusMessage;// resourceMgr.GetString("DataReadoutStarted");
        //    // this.StatusMessage = statusMessage;
        //    // SetStatus(statusMessage);
        //    this.demoThread =
        //        new Thread(new ParameterizedThreadStart(this.ThreadProcSafe));

        //    this.demoThread.Start(statusMessage);
        //    Application.DoEvents();

        //}
        ////// This method is executed on the worker thread and makes
        ////// a thread-safe call on the TextBox control.
        //private void ThreadProcSafe(object statusMessage)
        //{
        //    this.SetStatus(statusMessage.ToString());
        //}
        //private void SetStatus(string text)
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.textBox2.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetStatus);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        this.textBox2.Text = text;
        //        Application.DoEvents();
        //    }
        //}
        /// <summary>
        /// Used to Profiles from FD Mode . specially used where we nned to send FD command
        /// from normal mode for phasor and anomaly.
        /// </summary>
        /// <param name="meterID"></param>
        /// <param name="fdOptions"></param>
        /// <returns></returns>
        private String ReadMeterIDNormalForFD(string meterID, FastDownLoadOptions fdOptions)
        {
            string downloadedData = string.Empty;
            string FileMeterdata = string.Empty;
            try
            {
                Application.DoEvents();
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                fIncrementTimer();
                GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    btnReadAll.Enabled = true;
                }
                else
                {
                    fIncrementTimer();
                    GlobalObjects.objSerialComm.ClosePort();
                    Application.DoEvents();
                    FastDownLoadingBLL fastDownLoadingBLL = new FastDownLoadingBLL(meterID);
                    fastDownLoadingBLL.DownloadData(SerialPortSettings.Default.SerialPort, fdOptions, out downloadedData);

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, CABConstants.BAUDRATE, CABConstants.PARITY, CABConstants.DATABITS, CABConstants.STOPBITS, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
            }
            return downloadedData;
        }

        private bool ReadMeterID(out string meterID, out string strFileName, bool writeToFile)
        {
            bool success = false;
            strFileName = string.Empty;
            string FileMeterdata = string.Empty;
            meterID = string.Empty;
            // StartTimer();
            //bSuccess = true;
            try
            {
                Application.DoEvents();

                fIncrementTimer();
                GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    btnReadAll.Enabled = true;
                    //     StopTimer();
                    success = false;
                }
                else
                {
                    fIncrementTimer();

                    #region Reading meter ID
                    int writeResponse = fReadMeterSerialNumber();

                    if (writeResponse == 0)
                    {
                        string data = string.Empty;

                        int idLen = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                        if (idLen < 7 || idLen > 16)
                        {
                            MessageBox.Show("Meter data corrupt", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                            success = false;
                            ///     StopTimer();
                        }
                        else
                        {

                            string idLength = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                            while (idLength.Length < 2) idLength = "0" + idLength;
                            int index = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                            for (int i = 20; i <= 20 + (index - 1); i++)
                            {
                                data += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                            }
                            meterID = data;
                            //If value writeToFile is true THEN only create file and Write to it.
                            if (writeToFile)
                            {
                                strFileName = strFileName + data;
                                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                                FileMeterdata = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
                                FileStream fileStream = new FileStream(strFileName, FileMode.Create);
                                StreamWriter fileWriter = new StreamWriter(fileStream);
                                fileWriter.WriteLine("00" + FileMeterdata);
                            }
                            success = true;

                            //    StopTimer();
                        }
                    }
                    else
                    {
                        // Added close port.
                        //      StopTimer();
                        GlobalObjects.objSerialComm.ClosePort();
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        success = false;
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                return success;
            }
            finally
            {
                GlobalObjects.objSerialComm.ClosePort();
            }

        }
        private string ReadGeneralAndInstantaneousData(out string meterID, out bool bSuccess)
        {
            String strFileName = string.Empty;
            String FileMeterdata;
            meterID = "";
            StartTimer();
            bSuccess = true;

            Application.DoEvents();
            fIncrementTimer();
            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            if (DLMSMain.fDLMSConnect() != true)
            {
                btnReadAll.Enabled = true;
                StopTimer();
                return null;
            }
            fIncrementTimer();

            #region Reading meter ID
            int writeResponse = fReadMeterSerialNumber();

            if (writeResponse == 0)
            {
                string data = string.Empty;

                int idLen = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                if (idLen < 7 || idLen > 16)
                {
                    MessageBox.Show("Meter data corrupt", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                    return null;
                }

                string idLength = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                while (idLength.Length < 2) idLength = "0" + idLength;
                int index = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                for (int i = 20; i <= 20 + (index - 1); i++)
                {
                    data += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                }
                meterID = data;
                strFileName = strFileName + data;
                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                FileMeterdata = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
            }
            else
            {
                // Added close port.
                GlobalObjects.objSerialComm.ClosePort();
                MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            #endregion
            FileStream file1 = new FileStream(strFileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);

            try
            {

                wr1.WriteLine("00" + FileMeterdata);
                // Fix by Swati
                if (chkNameplate.Checked == true)
                {
                    #region instantaneous
                    btnReadAll.Enabled = false;

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    Application.DoEvents();

                    byte ret = fReadInastantaneous(3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        ///00000041_11_06_10_06_26_12
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (ret == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return null;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadInastantaneous(2);
                    if (ret == 0x01)
                    {
                        //FillDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (ret == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return null;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadScalarProfile(3, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (ret == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return null;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = ReadScalarProfile(2, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (ret == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        wr1.WriteLine("01");
                    }
                    else
                    {
                        StopTimer();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return null;
                    }
                    #endregion
                    if (isPUMA)
                    {
                        //added PUMA
                        #region CU-MD-KW
                        btnReadAll.Enabled = false;

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        Application.DoEvents();

                        //for getting Data
                        byte retval1 = fReadCumulativeKW(2);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }
                            else
                            {
                                // added if readout is not successful.
                                StopTimer();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                return null;
                            }
                            //length = nBlockIndex;

                        }
                        //fix - Ashish 04/10/11
                        else if (retval1 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            return null;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval1 = ReadScalarProfile(3, 4);
                        if (retval1 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval1 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            return null;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion

                        //added PUMA
                        #region CU-MD-KVA
                        btnReadAll.Enabled = false;

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        Application.DoEvents();

                        //for getting Data
                        byte retval2 = fReadCumulativeKVA(2);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            ///00000041_11_06_10_06_26_12
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            // To solve DLMS_0074 
                            int startIndex = 0;
                            // Receive buffer[18] tells the datatype , 0x06 means long int.
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                            {
                                length = 4;
                                startIndex = 19;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }
                            else
                            {
                                // added if readout is not successful.
                                StopTimer();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                return null;
                            }
                            //length = nBlockIndex;

                        }
                        //fix - Ashish 04/10/11
                        else if (retval2 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01" + "00000000");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            return null;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        //for getting scalar unit
                        retval2 = ReadScalarProfile(3, 5);
                        if (retval2 == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (retval2 == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            return null;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        #endregion
                    }
                    //chkInsta.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("01");              //writing Line breaks for no data
                }
                for (byte x = 0; x < 4; x++)
                    wr1.WriteLine("02");              //writing Line breaks for no data
                for (byte x = 0; x < 4; x++)
                    wr1.WriteLine("03");
                for (byte x = 0; x < 24; x++)
                    wr1.WriteLine("04");

                #region Nameplate

                int iIndex = 0;
                int nObjectCount = 0;

                iIndex = 0;
                ShowIndex = 1;
                nObjectCount = 7;//2;
                //nObjectCount = dGVGeneralReadout.Rows.Count;
                while (iIndex < nObjectCount)
                {
                    if (iIndex == 6)
                        isCurrentCommandOfPTRatio = true;
                    else
                        isCurrentCommandOfPTRatio = false;
                    int ret = Initialize_ReadMeterID(iIndex);
                    if (ret == 0x01)
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                        {
                            StopTimer();
                            MessageBox.Show("Invalid Cosem FCS", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            //DLMSMain.fDLMSDisconnect();
                            break;
                            bSuccess = false;
                        }
                        else
                        {

                            //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                            int length = 0;
                            int startIndex = 0;
                            String strTemp = "";
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                            {
                                length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                startIndex = 20;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                            {
                                length = 2;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                            {
                                length = 1;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                            {
                                length = 4;
                                startIndex = 19;
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                            {
                                length = 8;
                                startIndex = 19;

                            }
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                            }
                            if (isCurrentCommandOfPTRatio && String.IsNullOrEmpty(strTemp))
                            {
                                wr1.WriteLine("05" + strTemp + 0x00);
                            }
                            else
                            {
                                wr1.WriteLine("05" + strTemp);
                            }
                            //fDLMSConnect();
                        }
                    }
                    else if (ret == 0x00)
                    {
                        StopTimer();
                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        //DLMSMain.fDLMSDisconnect();
                        bSuccess = false;
                        break;
                    }
                    else
                    {
                        //do not display message
                        StopTimer();
                        //DLMSMain.fDLMSDisconnect();
                        //MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        break;
                    }
                    iIndex++;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                wr1.Close();
                file1.Close();

                btnReadAll.Enabled = true;
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                String strChecksum = GetMD5ChecksumForFile(strFileName);

                FileStream file2 = new FileStream(strFileName, FileMode.Append);
                StreamWriter wr2 = new StreamWriter(file2);

                wr2.WriteLine(strChecksum);

                wr2.Close();
                file2.Close();

                StopTimer();
                if (!bSuccess)
                    System.IO.File.Delete(strFileName);
            }
            return strFileName;
        }

        #region Fast DownLoading
        /// <summary>
        /// Created By : Vivek Agrawal
        /// Date : 25 Feb 2012
        /// Purpose: Fast DownLoading of Data 
        /// & store it in .FDL file
        /// Parameters: No Parameters
        /// </summary>
        private void FastDownLoading(string meterID, string lngFileName)
        {
            FDLOperations fastDownLoading = new FDLOperations(chkTamper.Checked, chkLoadSurvey.Checked, chkBilling.Checked, chkNameplate.Checked, chkInsta.Checked, chkPhasor.Checked, chkMidnightData.Checked, meterID);
            //Validate User Input for Fast DownLoading.
            FastDownLoadStatuses fastDownLoadStatus = FastDownLoadStatuses.None;
            //Variable to store the downloaded data.
            string downLoadedData = "";
            toolstripStatus.Text = resourceMgr.GetString("DataReadoutStarted");
            Application.DoEvents();
            //Down Load data.
            btnReadAll.Enabled = false;
            btnCancel.Enabled = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //  toolStripProgressBar1.Visible = true;
                //fIncrementTimer();
                fastDownLoading.OnfastDownloadingStatusChanged += new FDLOperations.OnFastDownloadingStatusChanged(UpdateFastDownloadStatus);
                //  fastDownLoading.onfdlStatusChanged += new FDLOperations.OnFDLStatusChanged(SetDataDownloadStatus);
                // To solve bug 89139, hard coding is removed for serial port selection. 
                fastDownLoadStatus = fastDownLoading.DownloadData(SerialPortSettings.Default.SerialPort, out downLoadedData);
                if (fastDownLoadStatus == FastDownLoadStatuses.ErrorInCommunication)
                {
                    toolstripStatus.Text = "";
                    Application.DoEvents();
                    MessageBox.Show(CoreUtility.GetMessageFromResourceFile("ErrorInCommunication"), "BCS");
                    //To solve bug 89140. Delete th file if error in communication is present. 
                    System.IO.File.Delete(lngFileName);
                    return;
                }
                else if (fastDownLoadStatus == FastDownLoadStatuses.BuffersizeNotSufficient)
                {
                    toolstripStatus.Text = "";
                    Application.DoEvents();
                    MessageBox.Show(resourceMgr.GetString("BufferSizeInsufficent"), "BCS");
                    System.IO.File.Delete(lngFileName);
                    return;
                }
                toolstripStatus.Text = resourceMgr.GetString("DataReadoutEnd");
            }
            catch (FileNotFoundException)
            {
                toolstripStatus.Text = "";
                Application.DoEvents();
                MessageBox.Show(resourceMgr.GetString("CommandFileNotFound"), "BCS");
                return;
            }
            catch (InvalidOperationException)
            {
                toolstripStatus.Text = "";
                Application.DoEvents();
                MessageBox.Show(resourceMgr.GetString("UnableToReadCommandFile"), "BCS");
                return;
            }
            finally
            {
                btnReadAll.Enabled = true;
                btnCancel.Enabled = true;
                //timerFDL.Stop();
                this.Cursor = Cursors.Default;
                //toolStripProgressBar1.Visible = false;
                fDLMSDisconnect();
            }
            //if no data is retrived then display the message.
            if (downLoadedData.Length == 0)
            {
                MessageBox.Show(resourceMgr.GetString("NoDataToSave"), "BCS");
                // Remove temporary file having general and instant data.
                System.IO.File.Delete(lngFileName);
                return;
            }
            toolstripStatus.Text = resourceMgr.GetString("SavingDataToFile");
            //if data is retrived then save the data on file system with appropiate file name.
            SaveDataToFile("\\VA\\METERID\\" + meterID + "\\DATETIME\\" + DateUtility.DateTimeToLong(DateTime.Now) + "\\" + downLoadedData, lngFileName, meterID);
            downLoadedData = null;
            toolstripStatus.Text = "";
            Application.DoEvents();
        }
        #endregion

        /// <summary>
        /// VBM - Checks whether channel is initialized for GSM/PSTN Communication
        /// </summary>
        /// <param name="commType"></param>
        /// <returns></returns>
        private bool CheckChannelInitialization(CommunicationType communicationType)
        {
            bool isChannelInitialized = false;
            SimSelectForm simSlectForm = new SimSelectForm(communicationType.ToString());
            simSlectForm.StartPosition = FormStartPosition.CenterParent;
            DialogResult dialogResult = simSlectForm.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                // GPRS communication support
                if (communicationType == CommunicationType.GPRS)
                {
                    isChannelInitialized = true;
                    GlobalObjects.objSerialComm.ImeiNumber = SimSelectForm.SimNumber;
                }
                else
                {
                    isChannelInitialized = SwitchModemConfigToDefaultConfig(communicationType);
                }
            }

            return isChannelInitialized;
        }

        /// <summary>
        /// Read Demand Integration Period
        /// </summary>
        private bool ReadDIP(StreamWriter writer)
        {
            bool bSuccess = false;
            int writeResponse = ReadIntegrationPeriod();
            int compValue = 0;
            if (writeResponse == (int)ProgrammingCode.Success)
            {
                compValue = (compValue | (int)GlobalObjects.objSerialComm.ReceiveBuffer[19]) << 8;
                compValue = (compValue | (int)GlobalObjects.objSerialComm.ReceiveBuffer[20]);
                bSuccess = true;
                writer.WriteLine("0A" + compValue.ToString());
                //txtBoxMeterID.Text = ReadMeterSerialNumber();
            }
            else if (writeResponse == (int)ProgrammingCode.AccessDenied)
            {
                MessageBox.Show("Access denied while reading Demand Integration Period", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
            }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
            {
                MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
            }
            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
            {
                MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
            }
            else
            {
                bSuccess = false;
            }
            return bSuccess;
        }


        private void btnReadAll_Click(object sender, EventArgs e)
        {
            commAborted = false;
            btnCancel.Enabled = false;
            SerialPortSettings.Default.ServerSAP = 0x01;
            string lngFilename = string.Empty;
            bool isChannelInitialized = true;
            //Empty the tool strip status
            this.toolstripStatus.Text = string.Empty;
            comType = CommunicationType.DIRECT;
            if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
            {
                comType = CommunicationTypeDetail.GetCommunicationType();
                if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                {
                    isChannelInitialized = CheckChannelInitialization(comType);
                    SerialPortSettings.Default.LPReadDays = cmbLSDays.SelectedItem.ToString();
                    SerialPortSettings.Default.Save();
                }
            }

            if (isChannelInitialized)
            {

                if (isPUMA)
                {
                    #region Fast DownLoading
                    /*
            Purpose:Fast DownLoading  & .FLD file creation
            Last Modified By: Vivek Agrawal
            Date            : 24 Feb 2012
            */

                    if (rdFastDownload.Checked)
                    {
                        string meterID = "";

                        //If some validation has failed then show the message else process for fast downloading.

                        if (!((chkBilling.Checked || chkLoadSurvey.Checked || chkTamper.Checked || chkNameplate.Checked || chkInsta.Checked)))
                        {
                            MessageBox.Show(resourceMgr.GetString("Selectanoption"), "BCS");
                            return;
                        }
                        toolstripStatus.Text = resourceMgr.GetString("ReadingGeneralInstantData");
                        if (ReadMeterID(out meterID, out lngFilename, true))
                        {
                            toolstripStatus.Text = resourceMgr.GetString("GeneralInstantaDataReadoutcompleted");
                            FastDownLoading(meterID, lngFilename);
                        }
                        toolstripStatus.Text = string.Empty;

                        GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                        btnCancel.Enabled = true; //cancel button change
                        return;
                    }
                }
                    #endregion

                if (chkInsta.Checked == false && chkBilling.Checked == false && chkLoadSurvey.Checked == false && chkTamper.Checked == false && chkNameplate.Checked == false)
                {
                    MessageBox.Show("Please select an option to read", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCancel.Enabled = true; //cancel button change
                    return;
                }

                if (chkLoadSurvey.Checked == true)
                {
                    /* VBM - Apply check  for wrong date range message  */
                    if (rdBtnReadBetweenLS.Checked)
                    {
                        if (System.DateTime.Compare(dtPickerFrom.Value, dtPickerTo.Value) == 1)
                        {
                            MessageBox.Show("Please select a valid date range", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                    }
                    /* VBM - Apply check  for wrong date range message  */
                }

                //fix Ashish - 14/10/2011
                if (chkTamper.Checked == true)
                {
                    if (rdBtnReadBetweenEvent.Checked && cmbBoxFromEvent.SelectedItem != null && cmbBoxToEvent.SelectedItem != null)
                        if (Convert.ToInt32(cmbBoxFromEvent.SelectedItem) > Convert.ToInt32(cmbBoxToEvent.SelectedItem))
                        {
                            MessageBox.Show("From event can not be greater than To event", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                }

                chkInsta.Enabled = true;
                chkBilling.Enabled = true;
                chkLoadSurvey.Enabled = true;
                chkTamper.Enabled = true;

                String strTamperScalecapture = string.Empty;
                String strTamperScalebuffer = string.Empty;
                String strFileName = string.Empty;
                String FileMeterdata;
                string meterSerialNumber = string.Empty;
                // Added file create in default folder - DLMSCommunication
                strFileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");

                if (!Directory.Exists(strFileName))
                {
                    Directory.CreateDirectory(strFileName);
                }
                StartTimer();
                Application.DoEvents();
                fIncrementTimer();
                //// Added to change the constructor when fastdownload read is changed to direct.
                //GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    btnCancel.Enabled = true; //cancel button change
                    btnReadAll.Enabled = true;
                    StopTimer();
                    return;
                }
                fIncrementTimer();

                #region Reading meter ID
                int writeResponse = fReadMeterSerialNumber();
                SerialPortSettings.Default.ServerSAP = 0x01;
                if (writeResponse == 0)
                {
                    string data = string.Empty;

                    int idLen = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                    if (idLen < 7 || idLen > 16)
                    {
                        MessageBox.Show("Meter data corrupt", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                        btnCancel.Enabled = true; //cancel button change
                        return;
                    }

                    string idLength = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                    while (idLength.Length < 2) idLength = "0" + idLength;
                    int index = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                    for (int i = 20; i <= 20 + (index - 1); i++)
                    {
                        data += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                    }
                    //VBM - Prefix additional 1 to serial number  
                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.SHYAMINDUS.ToString())
                    {
                        data = "1" + data;
                    }
                    meterSerialNumber = data;

                    //Display the meter serial number
                    this.toolstripStatus.Text = "Meter Serial Number : " + meterSerialNumber;
                    Application.DoEvents();
                    strFileName = strFileName + data;
                    strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                    FileMeterdata = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    StopTimer();
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    btnCancel.Enabled = true; //cancel button change
                    return;
                }
                #endregion

                meterModelNumber = 0;
                if (UtilityDetails.ShowMeterModelNo)
                {

                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.MVVNL.ToString())
                    {
                        writeResponse = WritePTRatio(1, true);
                        if (writeResponse == 2)
                        {
                            meterModelNumber = NamePlateConstants.RubyE250Value;
                        }
                        else if (writeResponse == 0x00)
                        {
                            meterModelNumber = NamePlateConstants.PumaLTE650Value;
                        }
                    }
                    else if (UtilityDetails.ReadSignatureData)
                    {
                        writeResponse = ReadSignature();
                        if (writeResponse == 0x00)
                        {
                            string meterModel = Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[35]).ToString()
                             + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[36]).ToString();
                            if (meterModel == "WC")
                            {
                                meterModelNumber = NamePlateConstants.RubyE250Value;
                            }
                            else if (meterModel == "LT")
                            {
                                meterModelNumber = NamePlateConstants.PumaLTE650Value;
                            }
                            else if (meterModel == "LC")
                            {
                                meterModelNumber = NamePlateConstants.LTCTCortexValue;
                            }
                        }
                    }
                    else
                    {
                        writeResponse = touModel.ReadMeterModelNumber();

                        if (writeResponse == 0x00)
                        {
                            meterModelNumber = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                        }
                    }
                }


                bool bSuccess = true;
                FileStream file1 = new FileStream(strFileName, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file1);

                try
                {   /*GKG 28/02/2013 137451 Initialisation before reading is required*/

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    /*GKG 28/02/2013 137451 Initialisation before reading is required*/
                    wr1.WriteLine("00" + FileMeterdata);

                    if (chkInsta.Checked == true)
                    {
                        #region instantaneous
                        btnReadAll.Enabled = false;
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        /*GKG 28/02/2013 137451 Initialisation before reading is required*/
                        //GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        //GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        //GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        //GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        /*GKG 28/02/2013 137451 Initialisation before reading is required*/
                        Application.DoEvents();

                        byte ret = fReadInastantaneous(3);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            ///00000041_11_06_10_06_26_12
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadInastantaneous(2);
                        if (ret == 0x01)
                        {
                            //FillDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = ReadScalarProfile(3, 0);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = ReadScalarProfile(2, 0);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("01" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("01");
                        }
                        else
                        {
                            StopTimer();
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        #endregion
                        //if (!(types.Equals(ApplicationType.DLMS_LTCT_650)) && !(types.Equals(ApplicationType.DLMS_RUBY_250)))
                        // Added to solve DLMS_0045.
                        if (UtilityDetails.ShowCumulativeMDKWKVA)
                        {
                            //added PUMA
                            #region CU-MD-KW
                            btnReadAll.Enabled = false;
                            SerialPortSettings.Default.ServerSAP = 0x01;
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            Application.DoEvents();

                            //for getting Data
                            byte retval1 = fReadCumulativeKW(2);
                            if (retval1 == 0x01)
                            {
                                //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                //length = nBlockIndex;
                                // To solve DLMS_0074 
                                int startIndex = 0;
                                // Receive buffer[18] tells the datatype , 0x06 means long int.
                                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                                {
                                    length = 4;
                                    startIndex = 19;
                                    for (int i = 0; i < length; i++)
                                    {
                                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                    }
                                    wr1.WriteLine("01" + strTemp);
                                }
                                else
                                {
                                    // added if readout is not successful.
                                    StopTimer();
                                    MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                    bSuccess = false;
                                    btnCancel.Enabled = true; //cancel button change
                                    return;
                                }
                                //length = nBlockIndex;

                            }
                            //fix - Ashish 04/10/11
                            else if (retval1 == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("01");
                            }
                            else
                            {
                                StopTimer();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            //for getting scalar unit
                            retval1 = ReadScalarProfile(3, 4);
                            if (retval1 == 0x01)
                            {
                                //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                                //fApplyScalarUnit();
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                //length = nBlockIndex;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }
                            //fix - Ashish 04/10/11
                            //Piyush : If the value is 5 then write an empty line and do not return
                            //This is required for ruby meter when read in PUMA login - NDPL
                            else if (retval1 == 0x07 || retval1 == 0x05)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("01");
                            }
                            else
                            {
                                StopTimer();

                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                            #endregion

                            //added PUMA
                            #region CU-MD-KVA
                            btnReadAll.Enabled = false;
                            SerialPortSettings.Default.ServerSAP = 0x01;
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            Application.DoEvents();

                            //for getting Data
                            byte retval2 = fReadCumulativeKVA(2);
                            if (retval2 == 0x01)
                            {
                                //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                                ///00000041_11_06_10_06_26_12
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                //length = nBlockIndex;
                                // To solve DLMS_0074 
                                int startIndex = 0;
                                // Receive buffer[18] tells the datatype , 0x06 means long int.
                                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                                {
                                    length = 4;
                                    startIndex = 19;
                                    for (int i = 0; i < length; i++)
                                    {
                                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                    }
                                    wr1.WriteLine("01" + strTemp);
                                }
                                else
                                {
                                    // added if readout is not successful.
                                    StopTimer();
                                    MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                    bSuccess = false;
                                    btnCancel.Enabled = true; //cancel button change
                                    return;
                                }
                                //length = nBlockIndex;

                            }
                            //fix - Ashish 04/10/11
                            else if (retval2 == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("01");
                            }
                            else
                            {
                                StopTimer();

                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            //for getting scalar unit
                            retval2 = ReadScalarProfile(3, 5);
                            if (retval2 == 0x01)
                            {
                                //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                                //fApplyScalarUnit();
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                //length = nBlockIndex;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("01" + strTemp);
                            }
                            //fix - Ashish 04/10/11
                            //Piyush : If the value is 5 then write an empty line and do not return
                            //This is required for ruby meter when read in PUMA login - NDPL
                            else if (retval2 == 0x07 || retval2 == 0x05)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("01");
                            }
                            else
                            {
                                StopTimer();

                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                            #endregion
                        }
                        chkInsta.Enabled = false;
                    }
                    else
                    {
                        for (byte x = 0; x < 4; x++)
                            wr1.WriteLine("01");              //writing Line breaks for no data
                    }
                    if (chkBilling.Checked == true)
                    {
                        #region Billing
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        byte ret = fReadBillingProfile(3);
                        if (ret == 0x01)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("02" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("02");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        ret = fReadBillingProfile(2);
                        if (ret == 0x01)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("02" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("02");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = ReadScalarProfile(3, 1);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("02" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("02");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        ret = ReadScalarProfile(2, 1);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("02" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("02");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        if (rdBtnReadComplete.Checked == true)
                        {
                            wr1.WriteLine("02" + string.Format("{0:X2}", Convert.ToByte(13)));
                        }
                        #endregion
                        chkBilling.Enabled = false;
                    }
                    else
                    {
                        for (byte x = 0; x < 4; x++)
                            wr1.WriteLine("02");              //writing Line breaks for no data
                    }
                    if (chkLoadSurvey.Checked == true)
                    {
                        #region loadSurvey
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        byte ret = fReadLSProfile(3);
                        if (ret == 0x01)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("03" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("03");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        //iIndex = 0;
                        ret = fReadLSProfile(2);
                        if (ret == 0x01)
                        {
                            //String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            wr1.Write("03");
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                //strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                            }
                            //wr1.WriteLine("03" + strTemp);
                            wr1.WriteLine("");
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("03");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        ret = ReadScalarProfile(3, 2);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("03" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("03");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = ReadScalarProfile(2, 2);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("03" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("03");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        #endregion
                        chkLoadSurvey.Enabled = false;
                    }
                    else
                    {
                        for (byte x = 0; x < 4; x++)
                            wr1.WriteLine("03");              //writing Line breaks for no data
                    }

                    if (chkTamper.Checked == true)
                    {
                        #region EventLog
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        byte ret = fReadTamperProfile(3, 0);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();

                            //StopTimer();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 0);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        ret = ReadScalarProfile(3, 3);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                            strTamperScalecapture = strTemp;
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        ret = ReadScalarProfile(2, 3);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            //fApplyScalarUnit();
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                            strTamperScalebuffer = strTemp;
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(3, 1);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 1);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        wr1.WriteLine("04" + strTamperScalecapture);
                        wr1.WriteLine("04" + strTamperScalebuffer);


                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        ret = fReadTamperProfile(3, 2);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 2);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        wr1.WriteLine("04" + strTamperScalecapture);
                        wr1.WriteLine("04" + strTamperScalebuffer);

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(3, 3);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 3);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        wr1.WriteLine("04" + strTamperScalecapture);
                        wr1.WriteLine("04" + strTamperScalebuffer);

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(3, 4);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 4);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        wr1.WriteLine("04" + strTamperScalecapture);
                        wr1.WriteLine("04" + strTamperScalebuffer);

                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(3, 5);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                        ret = fReadTamperProfile(2, 5);
                        if (ret == 0x01)
                        {
                            //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                            //MessageBox.Show("Billing");
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("04" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("04");
                        }
                        else
                        {
                            StopTimer();
                            //StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }
                        wr1.WriteLine("04" + strTamperScalecapture);
                        wr1.WriteLine("04" + strTamperScalebuffer);
                        strTamperScalecapture = "";
                        strTamperScalebuffer = "";
                        #endregion
                        chkTamper.Enabled = false;
                    }
                    else
                    {
                        for (byte x = 0; x < 24; x++)
                            wr1.WriteLine("04");              //writing Line breaks for no data
                    }

                    if (chkNameplate.Checked == true)
                    {
                        #region Nameplate
                        btnReadAll.Enabled = false;
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        Application.DoEvents();

                        int iIndex = 0;
                        int nObjectCount = 0;

                        iIndex = 0;
                        ShowIndex = 1;
                        if (UtilityDetails.ShowMeterModelNo)
                        {
                            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.MVVNL.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.DGVCL.ToString()
                                || UtilityDetails.ReadSignatureData)
                            {
                                nObjectCount = 7;
                            }
                            else
                            {
                                nObjectCount = 8;//2;
                            }
                        }
                        else
                        {
                            nObjectCount = 7;
                        }
                        //nObjectCount = dGVGeneralReadout.Rows.Count;
                        while (iIndex < nObjectCount)
                        {
                            if (iIndex == 6)
                                isCurrentCommandOfPTRatio = true;
                            else
                                isCurrentCommandOfPTRatio = false;
                            int ret = Initialize_ReadMeterID(iIndex);
                            if (ret == 0x01)
                            {
                                if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                                {
                                    StopTimer();
                                    MessageBox.Show("Invalid Cosem FCS", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                    //DLMSMain.fDLMSDisconnect();
                                    bSuccess = false;
                                    btnCancel.Enabled = true; //cancel button change
                                    break;
                                }
                                else
                                {

                                    //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                                    int length = 0;
                                    int startIndex = 0;
                                    String strTemp = "";
                                    if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                    {
                                        length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                        startIndex = 20;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                    {
                                        length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                        startIndex = 20;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                                    {
                                        length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                        startIndex = 20;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                                    {
                                        length = 2;
                                        startIndex = 19;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                                    {
                                        length = 1;
                                        startIndex = 19;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                                    {
                                        length = 4;
                                        startIndex = 19;
                                    }
                                    else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                                    {
                                        length = 8;
                                        startIndex = 19;

                                    }
                                    for (int i = 0; i < length; i++)
                                    {
                                        strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                    }
                                    if (isCurrentCommandOfPTRatio && String.IsNullOrEmpty(strTemp))
                                    {
                                        wr1.WriteLine("05" + strTemp + 0x00);
                                    }
                                    else
                                    {
                                        wr1.WriteLine("05" + strTemp);
                                    }
                                    //fDLMSConnect();
                                }

                            }
                            else if (ret == 0x00)
                            {
                                StopTimer();

                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                //DLMSMain.fDLMSDisconnect();
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                break;
                            }
                            else
                            {
                                //do not display message
                                StopTimer();
                                //DLMSMain.fDLMSDisconnect();
                                //MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                break;
                            }
                            iIndex++;
                        }
                        if (UtilityDetails.ShowMeterModelNo)
                        {
                            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.MVVNL.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.DGVCL.ToString()
                                || UtilityDetails.ReadSignatureData)
                            {
                                if (meterModelNumber == 0x01)
                                {
                                    wr1.WriteLine("0501");
                                }
                                else if (meterModelNumber == 0x02)
                                {
                                    wr1.WriteLine("0502");
                                }
                                else if (meterModelNumber == 0x03)
                                {
                                    wr1.WriteLine("0503");
                                }
                                else if (meterModelNumber == 0x04)
                                {
                                    wr1.WriteLine("0504");
                                }

                            }
                        }
                        #endregion
                        chkNameplate.Enabled = false;

                    }
                    else
                    {
                        for (byte x = 0; x < 7; x++)
                            wr1.WriteLine("05");              //writing Line breaks for no data
                    }
                    if (UtilityDetails.ShowMidnight)
                    {
                        if (chkMidnightData.Checked == true)
                        {
                            if (isPUMA)
                            {
                                if (meterModelNumber == NamePlateConstants.RubyE250Value)
                                {
                                    // For DGVCL utility no midnight in ruby meter , but its required in PUMA 
                                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.DGVCL.ToString())
                                    {
                                        for (byte x = 0; x < 4; x++)
                                        {
                                            wr1.WriteLine("06");
                                        }
                                    }
                                    else
                                    {
                                        ReadRubyMidnightData(out bSuccess, wr1);
                                    }
                                }
                                else if (meterModelNumber == NamePlateConstants.PumaLTE650Value || meterModelNumber == NamePlateConstants.PumaHTE650Value)
                                {
                                    ReadPUMAMidNightData(out bSuccess, wr1);
                                }

                            }
                            else
                            {
                                ReadRubyMidnightData(out bSuccess, wr1);
                            }
                            chkMidnightData.Enabled = false;
                        }
                        else
                        {
                            for (byte x = 0; x < 4; x++)
                                wr1.WriteLine("06");              //writing Line breaks for no data
                        }
                    }
                    else
                    {
                        for (byte x = 0; x < 4; x++)
                            wr1.WriteLine("06");              //writing Line breaks for no data
                    }
                    /* GKG JVVNL Current TOU Read */
                    if (chkBilling.Checked && UtilityDetails.ShowTouConfiguration)
                    {
                        #region TOU
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        int ret = ReadTOU(5);
                        if (ret == 0x00)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("08" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("08");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }



                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        ret = ReadTOU(4);
                        if (ret == 0x00)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("08" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("08");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        ret = ReadTOU(3);
                        if (ret == 0x00)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("08" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("08");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }


                        SerialPortSettings.Default.ServerSAP = 0x01;
                        //iIndex = 0;
                        ret = ReadRTC(2);
                        if (ret == 0x00)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            wr1.WriteLine("08" + strTemp);
                        }
                        //fix - Ashish 04/10/11
                        else if (ret == 0x07)
                        {
                            //write an empty line so that parser can predict that nothing in this line should be read
                            wr1.WriteLine("08");
                        }
                        else if (ret == 0x05)
                        {
                            StopTimer();
                            //StopTimer();
                            MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            btnCancel.Enabled = true; //cancel button change
                            return;
                        }

                        #endregion
                    }
                    else
                    {
                        wr1.WriteLine("08");              //writing Line breaks for no data
                    }

                    //Piyush for demand integration period.
                    if (UtilityDetails.ShowDIP)
                    {
                        if (bSuccess && !ReadDIP(wr1))
                        {
                            //Piyush commenting message is this is already in place in readdip function.
                            // MessageBox.Show("Cosem Connection Failed while Reading DIP.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;
                            return;
                        }
                    }
                    /* GKG JVVNL Current TOU Read */


                    /* VBM- Read Anomaly data using FD Command */
                    #region "Anamoly"
                    if (chkInsta.Checked && UtilityDetails.ShowAnamolyParameters)
                    {
                        btnReadAll.Enabled = false;
                        if (UtilityDetails.ShowAnomalyFastDownloadInNormalMode)
                        {
                            if (CommunicationTypeDetail.GetCommunicationType() == CommunicationType.DIRECT)
                            {

                                if (meterModelNumber == NamePlateConstants.RubyE250Value)
                                {
                                    wr1.WriteLine("07");
                                    DLMSMain.fDLMSDisconnect();
                                }
                                else
                                {
                                    wr1.WriteLine("07" + ReadMeterIDNormalForFD(meterSerialNumber, FastDownLoadOptions.Anomaly));
                                }
                            }
                            else
                            {
                                wr1.WriteLine("07");
                                DLMSMain.fDLMSDisconnect();
                            }
                        }
                        else if (UtilityDetails.ShowMeterModelNo)
                        {
                            if ((UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString()
                                || UtilityDetails.PrimaryUtlityName == UtilityEntity.MVVNL.ToString()
                                || UtilityDetails.PrimaryUtlityName == UtilityEntity.DGVCL.ToString())
                                && meterModelNumber == NamePlateConstants.RubyE250Value)
                            {
                                wr1.WriteLine("07");
                            }
                            else
                            {
                                ReadAnamolyParmaters(wr1, meterModelNumber);
                            }

                        }
                        else
                        {
                            //Read Anomaly as normal read.This is good.
                            ReadAnamolyParmaters(wr1);
                        }
                    }
                    else
                    {
                        //for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("07");              //writing Line breaks for no data
                    }
                    #endregion
                    /* VBM- Read anomaly data using FD Command */
                    if (CommunicationTypeDetail.GetCommunicationType() == CommunicationType.DIRECT)
                    {
                        /* VBM- Read Phasor data using FD Command */
                        #region Phasor
                        if (chkPhasor.Checked && UtilityDetails.ShowPhasorFastDownloadInNormalMode)
                        {
                            if (meterModelNumber == NamePlateConstants.RubyE250Value)
                            {
                                if (meterModelNumber == NamePlateConstants.RubyE250Value ||
                                   meterModelNumber == NamePlateConstants.LTCTCortexValue)
                                {
                                    DLMSMain.fDLMSDisconnect();
                                    wr1.WriteLine("09");
                                }
                                else
                                {
                                    wr1.WriteLine("09" + ReadMeterIDNormalForFD(meterSerialNumber, FastDownLoadOptions.Phasor));
                                }
                                chkPhasor.Enabled = false;
                            }
                            else
                            {
                                wr1.WriteLine("09" + ReadMeterIDNormalForFD(meterSerialNumber, FastDownLoadOptions.Phasor));
                            }
                            chkPhasor.Enabled = false;
                        }
                        #region PhasorNormal
                        //For CESC utility read phasor in normal mode like instant .
                        else if (chkPhasor.Checked && UtilityDetails.ReadPhasorNormalMode)
                        {
                            btnReadAll.Enabled = false;
                            SerialPortSettings.Default.ServerSAP = 0x01;
                            Application.DoEvents();
                            byte ret = ReadPhasorNormalMode(3);
                            if (ret == 0x01)
                            {
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("0B" + strTemp);
                            }
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("0B");
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            ret = ReadPhasorNormalMode(2);
                            if (ret == 0x01)
                            {
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("0B" + strTemp);
                            }
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("0B");
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            ret = ReadScalarProfile(3, 8);
                            if (ret == 0x01)
                            {

                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("0B" + strTemp);
                            }
                            //fix - Ashish 04/10/11
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("0B");
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                            ret = ReadScalarProfile(2, 8);
                            if (ret == 0x01)
                            {
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                wr1.WriteLine("0B" + strTemp);
                            }
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                wr1.WriteLine("0B");
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;
                                btnCancel.Enabled = true; //cancel button change
                                return;
                            }
                        }
                        #endregion
                        else
                        {
                            //CESC - Normal mode readout 
                            if (UtilityDetails.ReadPhasorNormalMode)
                            {
                                for (byte x = 0; x < 4; x++)
                                {
                                    wr1.WriteLine("0B");
                                }
                            }
                            else
                            {
                                wr1.WriteLine("09");              //writing Line breaks for no data
                            }

                        }
                        # endregion

                        /* VBM- Read Phasor data using FD Command */
                    }

                }

                catch (Exception ex)
                {
                    //following if condition added to refix bug 71614; 19th April 2012
                    if (GlobalObjects.objSerialComm.ClosePort())
                    { MessageBox.Show("The Port is Closed", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else
                    { MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                finally
                {
                    if ((chkPhasor.Checked && UtilityDetails.ShowPhasorFastDownloadInNormalMode) ||
                        (chkInsta.Checked && UtilityDetails.ShowAnomalyFastDownloadInNormalMode))
                    {
                    }
                    else
                    {
                        DLMSMain.fDLMSDisconnect();

                    }
                    if (CommunicationTypeDetail.GetCommunicationType() == CommunicationType.DIRECT)
                    {
                        GlobalObjects.objSerialComm.ClosePort();
                    }
                    wr1.Close();
                    file1.Close();
                    btnCancel.Enabled = true;
                    btnReadAll.Enabled = true;

                    if (commAborted == false)
                    {
                        String strChecksum = GetMD5ChecksumForFile(strFileName);

                        FileStream file2 = new FileStream(strFileName, FileMode.Append);
                        StreamWriter wr2 = new StreamWriter(file2);

                        wr2.WriteLine(strChecksum);

                        wr2.Close();
                        file2.Close();

                        if (UtilityDetails.EnableGSMCommunication)
                        {
                            if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                            {
                                toolstripStatus.Text = "Resetting modem..";
                                Application.DoEvents();
                                LeaveModemToUtilityConfig();
                                toolstripStatus.Text = string.Empty;
                                Application.DoEvents();
                            }
                        }

                        StopTimer();
                        if (bSuccess == true)
                            MessageBox.Show("Data is successfully saved in " + strFileName, " BCS");
                        else
                            System.IO.File.Delete(strFileName);
                    }

                }
            }
            else
            {
                this.toolstripStatus.Text = "Can not initialize local/remote modem";
            }
        }
        private void ReadAnamolyParmaters(StreamWriter wr1, int meterModelNumber)
        {

            int response = 0;
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            if (meterModelNumber == NamePlateConstants.RubyE250Value)
            {
                response = ReadGetPCBA();
            }
            else
            {
                response = ReadAnamoly();
            }

            if (response == (int)ProgrammingCode.Success)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("07" + strTemp);
            }
            else
            {
                wr1.WriteLine("07");
            }

        }


        private void ReadAnamolyParmaters(StreamWriter wr1)
        {
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            int response = ReadAnamoly();
            if (response == (int)ProgrammingCode.Success)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("07" + strTemp);
            }
            else
            {
                wr1.WriteLine("07");
            }

        }

        /// <summary>
        /// Used to read signature data for UP contarctors
        /// </summary>
        /// <returns></returns>
        private int ReadSignature()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadSignature(HDLCCommand, HDLCIndex, 2);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)CoreUtility.DLMSResultType.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)CoreUtility.DLMSResultType.AccessDenied;
                        }
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
            }
        }

        private int ReadGetPCBA()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPCBAStatus(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {

                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        private int ReadAnamoly()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadAnamoly(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {

                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void ReadRubyMidnightData(out bool bSuccess, StreamWriter wr1)
        {
            bSuccess = true;
            #region Midnight Data
            SerialPortSettings.Default.ServerSAP = 0x01;
            byte ret = fReadMidnightDataProfile(3);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                //write an empty line so that parser can predict that nothing in this line should be read
                wr1.WriteLine("06");
            }
            else if (ret == 0x05)
            {
                StopTimer();
                MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnCancel.Enabled = true; //cancel button change
                return;
            }
            ret = fReadMidnightDataProfile(2);
            if (ret == 0x01)
            {
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                wr1.Write("06");
                for (int i = 0; i < length; i++)
                {
                    wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                }
                wr1.WriteLine("");
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else if (ret == 0x05)
            {
                StopTimer();
                MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnCancel.Enabled = true; //cancel button change
                return;
            }

            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            ret = ReadScalarProfile(3, 6);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else
            {
                StopTimer();

                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnCancel.Enabled = true; //cancel button change
                return;
            }

            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

            ret = ReadScalarProfile(2, 6);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else
            {
                StopTimer();

                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnCancel.Enabled = true; //cancel button change
                return;
            }
            #endregion
        }
        private void ReadPUMAMidNightData(out bool bSuccess, StreamWriter wr1)
        {
            bSuccess = true;
            byte ret = fReadDailyProfile(3);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                //write an empty line so that parser can predict that nothing in this line should be read
                wr1.WriteLine("06");
            }
            else if (ret == 0x05)
            {
                StopTimer();
                MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnReadAllCMRI.Enabled = true;
                btnCMRICancel.Enabled = true;
                return;
            }
            ret = fReadDailyProfile(2);
            if (ret == 0x01)
            {
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                wr1.Write("06");
                for (int i = 0; i < length; i++)
                {
                    wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                }
                wr1.WriteLine("");
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else if (ret == 0x05)
            {
                StopTimer();
                MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnReadAllCMRI.Enabled = true;
                btnCMRICancel.Enabled = true;
                return;
            }

            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            ret = ReadScalarProfile(3, 6);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else
            {
                StopTimer();

                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnReadAllCMRI.Enabled = true;
                btnCMRICancel.Enabled = true;
                return;
            }

            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

            ret = ReadScalarProfile(2, 6);
            if (ret == 0x01)
            {
                String strTemp = "";
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                wr1.WriteLine("06" + strTemp);
            }
            else if (ret == 0x07)
            {
                wr1.WriteLine("06");
            }
            else
            {
                StopTimer();

                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                bSuccess = false;
                btnReadAllCMRI.Enabled = true;
                btnCMRICancel.Enabled = true;
                return;
            }


        }
        private int Initialize_ReadMeterID(int iIndex)
        {
            try
            {
                //store value from xml data set
                DataSet OBISLIST = null;
                //define xml data document object
                XmlDataDocument xmlDatadoc = null;
                xmlDatadoc = new XmlDataDocument();
                //serialize the xml data 
                string path = AppDomain.CurrentDomain.BaseDirectory + "Name Plate Details.xml";//SerialPortSettings.Default.ReadOut;//AppDomain.CurrentDomain.BaseDirectory + "DLMSReadOutList.xml";
                xmlDatadoc.DataSet.ReadXml(path);
                //assign memory to dataset object and name it "alerts"
                OBISLIST = new DataSet("OBis List Dataset");
                //deserialize xml data
                OBISLIST = xmlDatadoc.DataSet;
                GlobalObjects.objCOSEMLIB.ObisQueryDSet = OBISLIST;
                //store value from xml data set

                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQuery(HDLCCommand, HDLCIndex, iIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                fIncrementTimer();
                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        // This is a workaround as LTCT,HTCT meters will not support PT Ratio.
                        // if access denied and current command is for PT ratio return success.
                        if (ret == 0x03 && isCurrentCommandOfPTRatio)
                        {
                            return 0x01;
                        }
                        if (ret == 0x01)
                        {
                            return 0x01; //Success
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return 0x0E;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return 0x03;
                        }
                        else
                        {
                            return 0x00; //Fail
                        }
                    }
                    else
                    {
                        return 0x00;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte fReadTamperProfile(byte atb, byte tamperCompartment)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryTamperProfile(HDLCCommand, HDLCIndex, atb, tamperCompartment);

                //added by gopal for Selective Access By Entry
                if (atb == 0x02)
                {
                    if (rdBtnReadLastEvent.Checked == true)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFromEvent.Text));

                    }
                    else if (rdBtnReadBetweenEvent.Checked == true)
                    {

                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFromEvent.Text), Convert.ToByte(cmbBoxToEvent.Text));
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                fIncrementTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            float timeOut;
            SerialPortSettings.Default.SerialPort = rdbSinglePort.Checked ? cmbAvailableSerialPort.Text : DefaultPortName;
            if (txtPWD.Text.Length == 0)
            {
                if (cmbMode.Text != " PC ")
                {
                    MessageBox.Show("Please enter Password.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            if (txtPWD.Text.Length < 16 && cmbMode.Text == " US ")
            {
                MessageBox.Show("Please enter 16 digit random Key in Password Box.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            // Checking validation first. 2-May-2012
            if (txtPWD.Text.Length < 8 && cmbMode.Text == " MR ")
            {
                MessageBox.Show("Please enter 8 digit password in Password Box.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (cmbMode.Text == " FS ")
            {
                AccessPassword objAccessPassword = new AccessPassword();
                objAccessPassword.ShowDialog();

                if (objAccessPassword.Password != "lng123#")
                {
                    // MessageBox.Show("Please enter correct Password to save settings.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            if (rdbMultiplePorts.Checked)
            {
                if (!isConnectionTested)
                {
                    MessageBox.Show("Please ensure all GSM Modem connections are working properly using the Test Connections button", "Connections not Tested", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!SavePortMapping())
                {
                    MessageBox.Show("Unable to Save port mapping.", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lstSavedSerialPorts = GetSavedSerialPorts();
                MessageBox.Show("To apply changed settings, please re-start GSM service and BCS application.", "Restart GSM Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //iF SELECTED COMMUNICATION TYPE IS GPRS THEN DON'T SHOW GSM POP UP
            else if (!rdGPRS.Checked)
            {
                if (!string.IsNullOrEmpty(cmbAvailableSerialPort.Text.Trim()))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.COM_PORT, cmbAvailableSerialPort.Text);
                    MessageBox.Show("To apply changed settings, please close and open BCS application and restart GSM service.", "Restart BCS application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // To set the value for port setting.
            SetMultiplePortValue();


            if (txtBoxInterFrameTime.Text.Length == 0)
            {
                MessageBox.Show("Please enter InterFrame Timeout.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (txtResponseTimeOut.Text.Length == 0)
            {
                MessageBox.Show("Please enter Response Timeout.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            //added Ashish
            if (!float.TryParse(txtBoxInterFrameTime.Text, out timeOut))
            {
                MessageBox.Show("InterFrame Timeout should be numeric only.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!float.TryParse(txtResponseTimeOut.Text, out timeOut))
            {
                MessageBox.Show("Response Timeoutshould be numeric only.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (txtServerSAP.Text.Length == 0)
            {
                MessageBox.Show("Please enter Server Upper Address.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (txtServerLowerMacAddress.Text.Length == 0)
            {
                MessageBox.Show("Please enter Server Lower Address.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            SerialPortSettings.Default.IntercharacterDelay = Convert.ToInt32(txtBoxInterFrameTime.Text);
            SerialPortSettings.Default.CommandTimeOut = Convert.ToInt32(txtResponseTimeOut.Text);
            SerialPortSettings.Default.ServerSAP = Convert.ToInt32(txtServerSAP.Text);
            SerialPortSettings.Default.ServerLowerMacAddress = Convert.ToInt32(txtServerLowerMacAddress.Text);
            SerialPortSettings.Default.ScaleXMLPath = txtBoxScaleXML.Text;
            SerialPortSettings.Default.HLSKey = txtHLSPwd.Text;
            //SerialPortSettings.Default.InterframeTimeout = Convert.ToInt32(txtGSMInterFrameTime.Text);

            if (cmbContext.Text == "Long Name [LN]")
                SerialPortSettings.Default.ApplicationContext = 0x01;
            //else
            //    SerialPortSettings.Default.ApplicationContext = 0x02;
            if (cmbSecurity.Text == "No Security")
                SerialPortSettings.Default.SecurityMechanism = 0x00;
            else if (cmbSecurity.Text == "Low-Level")
                SerialPortSettings.Default.SecurityMechanism = 0x01;
            else
                SerialPortSettings.Default.SecurityMechanism = 0x02;

            SerialPortSettings.Default.PDUSize = 9999;
            SerialPortSettings.Default.Password = txtPWD.Text;

            if (cmbMode.Text == " PC ")
                SerialPortSettings.Default.ClientSAP = 0x10;
            else if (cmbMode.Text == " MR ")
                SerialPortSettings.Default.ClientSAP = 0x20;
            else if (cmbMode.Text == " US ")
            {
                SerialPortSettings.Default.ClientSAP = 0x30;
                SerialPortSettings.Default.Password = "1111111111111111";
                SerialPortSettings.Default.HLSKey = txtPWD.Text;
            }
            else if (cmbMode.Text == " FS ")
                SerialPortSettings.Default.ClientSAP = 0x40;
            //Piyush : If GSM communciation is enabled then save communication type also
            SerialPortSettings.Default.CommunicationType = "Direct";

            if (UtilityDetails.EnableGSMCommunication)
            {
                if (rdGSM.Checked)
                {
                    SerialPortSettings.Default.CommunicationType = "GSM";
                }
                else if (rdPSTN.Checked)
                {
                    SerialPortSettings.Default.CommunicationType = "PSTN";
                }

            }

            //SubhashM: If GPRS utility is enabled for GPRS
            if (UtilityDetails.ShowGPRSCommunication)
            {
                if (rdGPRS.Checked)
                {
                    SerialPortSettings.Default.CommunicationType = "GPRS";
                }
            }

            SerialPortSettings.Default.Save();

            //Set the read mode to property
            ReadOutMode = cmbMode.Text.Trim();


            SetModeNotFSSettings(cmbMode.Text);

            if (cmbMode.Text.Trim() == "PC")
            {
                if (tabpageExist == true)
                {
                    tabpageExist = false;
                    tempPage = tabPage1;
                }
                if (tabpageExistUS == true)
                {
                    tabpageExistUS = false;
                    tempPageUS1 = tabProgramming;
                    tabControlMain.TabPages.Remove(tabProgramming);
                    tempPageUS2 = tabCMRI;
                    tabControlMain.TabPages.Remove(tabCMRI);
                }
                SetPCModeSettings(cmbMode.Text);
            }
            else if (cmbMode.Text.Trim() == "MR")
            {
                if (tabpageExistUS == true)
                {
                    tabpageExistUS = false;
                    tempPageUS1 = tabProgramming;
                    tabControlMain.TabPages.Remove(tabProgramming);
                    tempPageUS2 = tabCMRI;
                    tabControlMain.TabPages.Remove(tabCMRI);
                }
                if (tabpageExist == false)
                {
                    tabpageExist = true;
                }
                SetMRModeSettings(cmbMode.Text);
            }
            else if (cmbMode.Text.Trim() == "US")
            {
                byte count = 1;
                if (tabpageExist == false)
                {
                    tabpageExist = true;
                    count = 2;
                }
                else
                {
                    count = 2;
                }
                if (tabpageExistUS == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPageUS1);
                    tabControlMain.TabPages.Insert(count++, tempPageUS2);
                    tabpageExistUS = true;
                }
                SetUSModeSettings(cmbMode.Text);
            }
            //Add condition for FS mode. Functionality will be same as US mode.
            //Additional tab will be added like MD Reset.
            else if (cmbMode.Text == " FS ")
            {
                SetFSModeSettings(cmbMode.Text);
            }

            //Calls method to set Communication mode. This will be used by GPRS Code
            SetCommunicationMode();

            // Spelling change. Bug 83827.
            MessageBox.Show("Settings saved successfully !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetButtonMode(SerialPortSettings.Default.ClientSAP);
            tabControlMain.SelectedIndex = 0;
            UpdateToolStripStatus();
        }

        /// <summary>
        /// Set the communication mode in Serial Communication class. So that it will accessable while sending the data to comport/GPRS tunnel
        /// </summary>
        private void SetCommunicationMode()
        {
            if (rdGPRS.Checked)
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(CommunicationType.GPRS.ToString());
            }
            else if (rdGSM.Checked)
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(CommunicationType.GSM.ToString());
            }
            else if (rdPSTN.Checked)
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(CommunicationType.PSTN.ToString());
            }
            else
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(CommunicationType.DIRECT.ToString());
            }

        }

        private void SetModeConfiguration(string selectedMode)
        {
            SetModeNotFSSettings(selectedMode);
            SetFSModeSettings(selectedMode);
            SetUSModeSettings(selectedMode);
            SetMRModeSettings(selectedMode);
            SetPCModeSettings(selectedMode);

        }

        /// <summary>
        /// Set Mode Settings if mode is not FS
        /// </summary>
        /// <param name="selectedMode"></param>
        private void SetModeNotFSSettings(string selectedMode)
        {
            //IF MODE IS NOT FS
            if (!string.Equals(selectedMode.Trim(), "FS", StringComparison.OrdinalIgnoreCase))
            {

                //IF TAB CONTAINS MD RESET TAB. THEN REMOVE IT. IT WILL BE SHOWN ONLY IN FS MODE.
                /* VBM Enable MD Rset in US Mode  for PGVCL*/
                if (tabCTPTRatio.Contains(tabMDReset) && !UtilityDetails.ShowMDResetInUSMode)
                {
                    tabCTPTRatio.TabPages.Remove(tabMDReset);
                }
                /* VBM Enable MD Rset in US Mode  for PGVCL*/
                //IF TAB CONTAINS KVAH Selection TAB. THEN REMOVE IT. IT WILL BE SHOWN ONLY IN FS MODE.
                //Enable kvah selection US Mode for specific utility.
                if (tabCTPTRatio.Contains(tabKVAH) && !UtilityDetails.ShowKVAHSelectionTabInUSMode)
                {
                    tabCTPTRatio.TabPages.Remove(tabKVAH);
                }

                //IF TAB CONTAINS KVAH Selection TAB. THEN REMOVE IT. IT WILL BE SHOWN ONLY IN FS MODE.
                if (tabCTPTRatio.Contains(tabRS232Lock))
                {
                    tabCTPTRatio.TabPages.Remove(tabRS232Lock);
                }


                if (tabCTPTRatio.TabPages.Contains(tbPDisplayParameters) && !UtilityDetails.ShowDisplayParametersInUSMode)
                {
                    tabCTPTRatio.TabPages.Remove(tbPDisplayParameters);
                }

            }
        }

        /// <summary>
        /// Set PC Mode settings
        /// </summary>
        /// <param name="selectedMode"></param>
        private void SetPCModeSettings(string selectedMode)
        {
            if (string.Equals(selectedMode.Trim(), "PC", StringComparison.OrdinalIgnoreCase))
            {
                //IF TAB CONTAINS PHASOR TAB THEN REMOVE IT. IT WILL BE SHOWN ONLY IN US MODE.
                if (tabControlMain.Contains(tabPhasor))
                {
                    tabControlMain.TabPages.Remove(tabPhasor);
                    //tabControlMain.Show();
                }
                if (tabControlMain.Contains(tabMeterAccuracyCheck))
                {
                    tabControlMain.TabPages.Remove(tabMeterAccuracyCheck);
                }
            }
        }

        /// <summary>
        /// Set MR Mode Settings
        /// </summary>
        /// <param name="selectedMode"></param>
        private void SetMRModeSettings(string selectedMode)
        {

            if (string.Equals(selectedMode.Trim(), "MR", StringComparison.OrdinalIgnoreCase))
            {

                //If Dynamic Phasor has to display for utility
                if (UtilityDetails.ShowDynamicPhasorTab)
                {
                    if (!tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Insert(tabControlMain.TabPages.Count - 1, tabPhasor);
                    }
                }
                else
                {
                    if (tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Remove(tabPhasor);
                    }
                }

                //Meter accuracy check 
                if (UtilityDetails.ShowMeterAccuracyCheck)
                {
                    if (!tabControlMain.TabPages.ContainsKey("tabMeterAccuracyCheck"))
                    {
                        tabControlMain.TabPages.Insert(tabControlMain.TabPages.Count - 2, tabMeterAccuracyCheck);
                    }
                }
                else if (tabControlMain.TabPages.ContainsKey("tabMeterAccuracyCheck"))
                {
                    tabControlMain.TabPages.Remove(tabMeterAccuracyCheck);
                }
            }
        }

        /// <summary>
        /// Set US Mode Settings
        /// </summary>
        /// <param name="selectedMode"></param>
        private void SetUSModeSettings(string selectedMode)
        {
            if (string.Equals(selectedMode.Trim(), "US", StringComparison.OrdinalIgnoreCase))
            {
                //If Dynamic phasor has to show for utility then add to tab else remove it
                if (UtilityDetails.ShowDynamicPhasorTab)
                {
                    if (!tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Insert(tabControlMain.TabPages.Count - 1, tabPhasor);
                    }
                }
                else
                {
                    if (tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Remove(tabPhasor);
                    }
                }

                //Change for BYPL login, Need to hide tab page 
                if (UtilityDetails.DisableProgrammingCTPTRatio)
                {
                    if (tabCTPTRatio.TabPages.ContainsKey("tbCTPTRatio"))
                    {
                        tabCTPTRatio.TabPages.Remove(tbCTPTRatio);
                    }
                }
                //Change for BYPL login, Need to hide tab page  
                if (UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                {

                    if (tabCTPTRatio.TabPages.ContainsKey("tabPageIntegrationPeriod"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabPageIntegrationPeriod);
                    }
                }
                //Change for BYPL login, Need to hide tab page 
                if (UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                {
                    if (tabCTPTRatio.TabPages.ContainsKey("tabPage7"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabPage7);
                    }
                }
                //VBM - Disable billing date and time programming
                if (UtilityDetails.DisableProgrammingBillingDateTime)
                {
                    if (tabCTPTRatio.TabPages.ContainsKey("tabPage4"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabPage4);
                    }
                }
                //Meter accuracy check 
                if (UtilityDetails.ShowMeterAccuracyCheck)
                {
                    if (!tabControlMain.TabPages.ContainsKey("tabMeterAccuracyCheck"))
                    {
                        tabControlMain.TabPages.Insert(tabControlMain.TabPages.Count - 2, tabMeterAccuracyCheck);
                    }
                }
                else if (tabControlMain.TabPages.ContainsKey("tabMeterAccuracyCheck"))
                {
                    tabControlMain.TabPages.Remove(tabMeterAccuracyCheck);
                }
                /* VBM - Add Display parameter in US Mode if utility has this property */
                if (UtilityDetails.ShowDisplayParametersInUSMode)
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tbPDisplayParameters"))
                    {
                        tabCTPTRatio.TabPages.Add(tbPDisplayParameters);
                        // FillDisplayParameters();
                    }
                }
                else
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tbPDisplayParameters"))
                    {
                        tabCTPTRatio.TabPages.Remove(tbPDisplayParameters);
                    }
                }
                /* VBM - Add Display parameter in US Mode if utility has this property */

            }

        }
        //private void tbPDisplayParameters_GotFocus(object sender, System.EventArgs e)
        // {
        //     if (cmbMode.Text.ToLower().Contains("fs"))
        //     {
        //         FillDisplayParameters();
        //     }
        // }
        /// <summary>
        /// Set FS Mode settings
        /// </summary>
        /// <param name="selectedMode"></param>
        private void SetFSModeSettings(string selectedMode)
        {

            if (string.Equals(selectedMode.Trim(), "FS", StringComparison.OrdinalIgnoreCase))
            {
                byte count = 1;
                if (tabpageExist == false)
                {
                    //tabControlMain.TabPages.Insert(count++, tempPage);
                    tabpageExist = true;
                    count = 2;
                }
                else
                {
                    count = 2;
                }
                if (tabpageExistUS == false)
                {
                    tabControlMain.TabPages.Insert(count++, tempPageUS1);
                    tabControlMain.TabPages.Insert(count++, tempPageUS2);
                    tabpageExistUS = true;
                }
                //If Mode is FS and MD reset has to display for utility. Then add MD Reset tab to the tab control
                if (UtilityDetails.ShowBillingResetTab)
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tabMDReset"))
                    {
                        tabCTPTRatio.TabPages.Add(tabMDReset);
                        tabCTPTRatio.Show();
                    }
                }
                else
                {

                    if (tabCTPTRatio.TabPages.ContainsKey("tabMDReset"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabMDReset);
                    }
                }

                //Piyush : 120086
                //Add or Remove Display Parameters tab and fill it, if it is applicable for current tender & Utility
                if (UtilityDetails.ShowDisplayParameters)
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tbPDisplayParameters"))
                    {
                        tabCTPTRatio.TabPages.Add(tbPDisplayParameters);
                        // FillDisplayParameters();
                    }
                }
                else
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tbPDisplayParameters"))
                    {
                        tabCTPTRatio.TabPages.Remove(tbPDisplayParameters);
                    }
                }



                //If Mode is FS and kvah tab has to show for utility. Then add KVAH Selection tab to the tab control
                if (UtilityDetails.ShowKVAHSelectionTabInFSMode)
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tabKVAH"))
                    {
                        tabCTPTRatio.TabPages.Add(tabKVAH);
                    }
                }
                else
                {
                    if (tabCTPTRatio.TabPages.ContainsKey("tabKVAH"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabKVAH);
                    }
                }

                //If Mode is FS and kvah tab has to show for utility. Then add KVAH Selection tab to the tab control
                if (UtilityDetails.ShowRS232Tab)
                {
                    if (!tabCTPTRatio.TabPages.ContainsKey("tabRS232Lock"))
                    {
                        tabCTPTRatio.TabPages.Add(tabRS232Lock);
                    }
                }
                else
                {
                    if (tabCTPTRatio.TabPages.ContainsKey("tabRS232Lock"))
                    {
                        tabCTPTRatio.TabPages.Remove(tabRS232Lock);
                    }
                }

                //Add Phasor Tab
                if (UtilityDetails.ShowDynamicPhasorTab)
                {
                    if (!tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Insert(tabControlMain.TabPages.Count - 1, tabPhasor);
                        tabCTPTRatio.Show();
                    }
                }
                else
                {
                    if (tabControlMain.TabPages.ContainsKey("tabPhasor"))
                    {
                        tabControlMain.TabPages.Remove(tabPhasor);
                        tabCTPTRatio.Show();
                    }
                }

            }

        }

        private bool ValidatePortMapping(out string pModemPorts, out string pCMRIPort)
        {
            errpPortMapping.Clear();
            bool isModem = false, isCMRI = false;
            bool isAnyValueSet = false;
            string strModemPorts = string.Empty;
            string strCMRIPort = string.Empty;
            pModemPorts = pCMRIPort = string.Empty;
            for (int i = 0; i < dgvPortUsageAssociation.Rows.Count; i++)
            {
                isModem = isCMRI = false;
                isModem = Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeModem.Name].Value);
                isCMRI = Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeCMRI.Name].Value);
                if (isModem || isCMRI)
                {
                    isAnyValueSet = true;
                    if (isModem && isCMRI)
                    {
                        errpPortMapping.SetError(dgvPortUsageAssociation, "A port cannot be mapped for both CMRI and MODEM connections!");
                        return false;
                    }
                    else if (isModem)
                    {
                        strModemPorts += dgvPortUsageAssociation.Rows[i].Cells[colPortName.Name].Value.ToString() + ",";
                    }
                    else if (string.IsNullOrEmpty(strCMRIPort))
                    {
                        strCMRIPort = dgvPortUsageAssociation.Rows[i].Cells[colPortName.Name].Value.ToString();
                    }
                    else
                    {
                        errpPortMapping.SetError(dgvPortUsageAssociation, "Only ONE port can be mapped for CMRI!");
                        return false;
                    }
                }
            }
            strModemPorts = strModemPorts.TrimEnd(',');
            if (isAnyValueSet &&
                !string.IsNullOrEmpty(strModemPorts) &&
                !string.IsNullOrEmpty(strCMRIPort))
            {
                pModemPorts = strModemPorts;
                pCMRIPort = strCMRIPort;
                return true;
            }
            else if (!string.IsNullOrEmpty(strModemPorts) &&
                string.IsNullOrEmpty(strCMRIPort))
            {
                errpPortMapping.Clear();
                errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for CMRI connection");
                return false;
            }
            else if (!string.IsNullOrEmpty(strCMRIPort) &&
                string.IsNullOrEmpty(strModemPorts))
            {
                errpPortMapping.Clear();
                errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for GSM Modem connection");
                return false;
            }
            else
            {
                errpPortMapping.Clear();
                errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for MODEM and CMRI connections");
                return false;
            }
        }
        private bool SavePortMapping()
        {
            bool retVal = false;
            string strModemPorts = string.Empty, strCMRIPort = string.Empty;
            if (ValidatePortMapping(out strModemPorts, out strCMRIPort))
            {
                if (!string.IsNullOrEmpty(strModemPorts))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.GSM_COM_PORTS, strModemPorts);
                }
                if (!string.IsNullOrEmpty(strCMRIPort))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.CMRI_COM_PORT, strCMRIPort);
                }
                retVal = true;
            }
            return retVal;
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMode.Text == " PC ")
            {
                txtPWD.Text = "";
                labelPwd.Visible = true;
                labelHLS.Visible = false;
                txtPWD.Enabled = false;
                txtHLSPwd.Enabled = false;
                cmbSecurity.SelectedIndex = 0;
                button7.Enabled = false;
            }
            if (cmbMode.Text == " MR ")
            {
                labelPwd.Visible = true;
                labelHLS.Visible = false;
                txtPWD.Enabled = true;
                txtPWD.Text = "";
                txtPWD.MaxLength = 8;
                txtHLSPwd.Enabled = false;
                cmbSecurity.SelectedIndex = 1;
                button7.Enabled = false;
            }

            if (cmbMode.Text == " US ")
            {
                txtPWD.Enabled = true;
                labelPwd.Visible = false;
                labelHLS.Visible = true;
                txtPWD.Text = "";
                txtPWD.MaxLength = 16;
                cmbSecurity.SelectedIndex = 2;
                txtHLSPwd.Visible = false;
                button7.Visible = false;
                button7.Enabled = false;
            }
            if (cmbMode.Text == " FS ")
            {
                labelPwd.Visible = true;
                labelHLS.Visible = false;
                txtPWD.Enabled = true;
                txtPWD.MaxLength = 8;
                txtPWD.Text = "00000000";
                txtPWD.Enabled = false;
                txtHLSPwd.Enabled = false;
                cmbSecurity.SelectedIndex = 1;
                button7.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog.
            folderBrowserDialog1.Description = "Select the path for Scale and Unit XML file storage.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            folderBrowserDialog1.ShowNewFolderButton = false;
            // Default to the My Documents folder.
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtBoxScaleXML.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            String fpath = string.Empty;
            if (isPUMA)
                fpath = Application.StartupPath + "\\sprash.exe";
            else
                fpath = Application.StartupPath + "\\sprash_old.exe";
            // temp code GKG : need to be removed after gaurav will add genric Encryption
            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.NDPL.ToString())
            {
                fpath = Application.StartupPath + "\\sprash_old.exe";
            }
            // temp code :  need to be removed after gaurav will add genric Encryption

            System.Diagnostics.Process.Start(fpath);
        }

        private void chkOther_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOther.Checked == true)
            {
                chkInsta.Checked = true;
                chkBilling.Checked = true;
                chkTamper.Checked = true;
                chkLoadSurvey.Checked = true;
                if (chkMidnightData.Visible)
                {
                    chkMidnightData.Checked = true;
                }
                chkPhasor.Checked = true;
                //VBM -though its checked but its refelcting while clicking two times ,So just checking it again.
                chkOther.Checked = true;

            }
            else
            {
                chkInsta.Checked = false;
                chkBilling.Checked = false;
                chkTamper.Checked = false;
                chkLoadSurvey.Checked = false;
                chkMidnightData.Checked = false;
                chkPhasor.Checked = false;

            }
            chkInsta.Enabled = true;
            chkBilling.Enabled = true;
            chkTamper.Enabled = true;
            chkLoadSurvey.Enabled = true;
            // To solve bug 89297.
            chkMidnightData.Enabled = true;
            chkPhasor.Enabled = true;
        }





        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            disconnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Connect()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;

                if (textBoxGSM.Text.Length < 10)
                {
                    MessageBox.Show("Enter 10 digit sim number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                {
                    SerialPortSettings.Default.ModemNumber = textBoxGSM.Text;
                    SerialPortSettings.Default.Save();
                }

                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = "AT";

                String Result = fSendModemCommand("AT");
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Result;
                Application.DoEvents();
                if (Result == "\r\nOK\r\n")
                {
                    Application.DoEvents();
                    GlobalObjects.objSerialComm.InterchatracterDelay = 35000;
                    GlobalObjects.objSerialComm.CommandTimeout = 40000;
                    GlobalObjects.objSerialComm.bCommType = 2;

                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = "ATD" + textBoxGSM.Text;
                    Application.DoEvents();

                    Application.DoEvents();
                    Result = fSendModemCommand("ATD", textBoxGSM.Text);
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Result;
                    Application.DoEvents();
                    if (Result == "\r\nCONNECT 9600\r\n")
                    {
                        MessageBox.Show(Result, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (Result == "\r\nNO CARRIER\r\n" || Result == "\r\nBUSY\r\n" || Result == "\r\nNO ANSWER\r\n")
                    {
                        MessageBox.Show(Result, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(Result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(Result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        private void disconnect()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                GlobalObjects.objSerialComm.InterchatracterDelay = 5500;
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = "+++";

                String Result = fSendDisc();

                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Result;

                if (Result == "\r\nOK\r\n")
                {
                    GlobalObjects.objSerialComm.InterchatracterDelay = 5500;
                    GlobalObjects.objSerialComm.CommandTimeout = 6000;
                    GlobalObjects.objSerialComm.bCommType = 1;

                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = "ATH";

                    Result = fSendModemCommand("ATH");
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = Result;

                    if (Result == "\r\nOK\r\n")
                        MessageBox.Show(Result, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    else
                    {
                        this.Cursor = Cursors.Default;
                        if (Result == string.Empty)
                            Result = "Error while Disconnecting";
                        MessageBox.Show(Result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    if (Result == string.Empty)
                        Result = "Error while Disconnecting";
                    MessageBox.Show(Result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.bCommType = 0;
                GlobalObjects.objSerialComm.flgReadFlag = false;
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        private void Readdata()
        {
            if (chkInsta.Checked == false && chkBilling.Checked == false && chkLoadSurvey.Checked == false && chkTamper.Checked == false && chkNameplate.Checked == false)
            {
                MessageBox.Show("Please select any option to read.", "BCS");
                return;
            }
            if (chkLoadSurvey.Checked == true)
            {
                if (System.DateTime.Compare(dtPickerFrom.Value, dtPickerTo.Value) == 1)
                {

                    MessageBox.Show("Please select valid load survey date.", "BCS");
                    return;
                }
            }

            chkInsta.Enabled = true;
            chkBilling.Enabled = true;
            chkLoadSurvey.Enabled = true;
            chkTamper.Enabled = true;

            String strTamperScalecapture;
            String strTamperScalebuffer;
            String strFileName;
            String FileMeterdata;
            //strFileName = SerialPortSettings.Default.ScaleXMLPath + "\\";
            strFileName = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(strFileName))
            {
                Directory.CreateDirectory(strFileName);
            }

            StartTimer();
            Application.DoEvents();
            fIncrementTimer();
            if (DLMSMain.fDLMSConnect() != true)
            {
                btnReadAll.Enabled = true;
                StopTimer();
                return;
            }
            fIncrementTimer();

            #region Reading meter ID
            int writeResponse = fReadMeterSerialNumber();

            if (writeResponse == 0)
            {
                string data = string.Empty;

                for (int i = 20; i <= 27; i++)
                {
                    data += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                }
                strFileName = strFileName + data;
                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                FileMeterdata = data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
            }
            else
            {

                MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return;
            }
            #endregion
            bool bSuccess = true;
            FileStream file1 = new FileStream(strFileName, FileMode.Create);
            StreamWriter wr1 = new StreamWriter(file1);

            try
            {
                wr1.WriteLine("00" + FileMeterdata);

                if (chkInsta.Checked == true)
                {
                    #region instantaneous
                    btnReadAll.Enabled = false;

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    Application.DoEvents();


                    byte ret = fReadInastantaneous(3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        ///00000041_11_06_10_06_26_12
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = fReadInastantaneous(2);
                    if (ret == 0x01)
                    {
                        //FillDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("01" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    #endregion
                    chkInsta.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("01");              //writing Line breaks for no data
                }
                if (chkBilling.Checked == true)
                {
                    #region Billing

                    //iIndex = 0;
                    byte ret = fReadBillingProfile(3);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        StopTimer();
                        //StopTimer();
                        GlobalObjects.objSerialComm.ClosePort();
                        MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    //iIndex = 0;
                    ret = fReadBillingProfile(2);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        StopTimer();
                        //StopTimer();
                        MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("02" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    #endregion
                    chkBilling.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("02");              //writing Line breaks for no data
                }
                if (chkLoadSurvey.Checked == true)
                {
                    #region loadSurvey

                    //iIndex = 0;
                    byte ret = fReadLSProfile(3);
                    if (ret == 0x01)
                    {
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else if (ret == 0x05)
                    {
                        StopTimer();
                        //StopTimer();
                        MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    //iIndex = 0;
                    ret = fReadLSProfile(2);
                    if (ret == 0x01)
                    {
                        //String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        wr1.Write("03");
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            //strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            wr1.Write(String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]));
                        }
                        //wr1.WriteLine("03" + strTemp);
                        wr1.WriteLine("");
                    }
                    else if (ret == 0x05)
                    {
                        StopTimer();
                        //StopTimer();
                        MessageBox.Show("Access Denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("03" + strTemp);
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    #endregion
                    chkLoadSurvey.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 4; x++)
                        wr1.WriteLine("03");              //writing Line breaks for no data
                }

                if (chkTamper.Checked == true)
                {
                    ////Connect();
                    ////disconnect();
                    #region EventLog
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    byte ret = fReadTamperProfile(3, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 0);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(3, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalecapture = strTemp;
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = ReadScalarProfile(2, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                        strTamperScalebuffer = strTemp;
                    }
                    else
                    {
                        StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 1);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);


                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    ret = fReadTamperProfile(3, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 2);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 3);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 4);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 4);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }

                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);

                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(3, 5);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                    ret = fReadTamperProfile(2, 5);
                    if (ret == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer, dgvTamper);
                        //MessageBox.Show("Billing");
                        String strTemp = "";
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        wr1.WriteLine("04" + strTemp);
                    }
                    else
                    {
                        StopTimer();
                        //StopTimer();

                        MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        bSuccess = false;
                        return;
                    }
                    wr1.WriteLine("04" + strTamperScalecapture);
                    wr1.WriteLine("04" + strTamperScalebuffer);
                    strTamperScalecapture = "";
                    strTamperScalebuffer = "";
                    #endregion
                    chkTamper.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 24; x++)
                        wr1.WriteLine("04");              //writing Line breaks for no data
                }

                if (chkNameplate.Checked == true)
                {

                    #region Nameplate
                    btnReadAll.Enabled = false;
                    Application.DoEvents();
                    int iIndex = 0;
                    int nObjectCount = 0;
                    iIndex = 0;
                    ShowIndex = 1;
                    nObjectCount = 6;
                    while (iIndex < nObjectCount)
                    {
                        int ret = Initialize_ReadMeterID(iIndex);
                        if (ret == 0x01)
                        {
                            if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == false)
                            {
                                StopTimer();
                                MessageBox.Show("Invalid Cosem FCS", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                //DLMSMain.fDLMSDisconnect();
                                break;
                                bSuccess = false;
                            }
                            else
                            {

                                //DisplayNamePlateDataInGrid(GlobalObjects.objSerialComm.ReceiveBuffer, iIndex);
                                int length = 0;
                                int startIndex = 0;
                                String strTemp = "";
                                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                                {
                                    length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                    startIndex = 20;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                                {
                                    length = 2;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                                {
                                    length = 1;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                                {
                                    length = 4;
                                    startIndex = 19;
                                }
                                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                                {
                                    length = 8;
                                    startIndex = 19;

                                }
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                                }
                                wr1.WriteLine("05" + strTemp);

                                //fDLMSConnect();
                            }
                        }
                        else if (ret == 0x00)
                        {
                            StopTimer();

                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            //DLMSMain.fDLMSDisconnect();
                            bSuccess = false;
                            break;
                        }
                        else
                        {
                            //do not display message
                            StopTimer();
                            //DLMSMain.fDLMSDisconnect();
                            //MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            break;
                        }
                        iIndex++;
                    }
                    #endregion
                    chkNameplate.Enabled = false;
                }
                else
                {
                    for (byte x = 0; x < 6; x++)
                        wr1.WriteLine("05");              //writing Line breaks for no data
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                wr1.Close();
                file1.Close();
                btnReadAll.Enabled = true;
                DLMSMain.fDLMSDisconnect();
                StopTimer();
                if (bSuccess == true)
                {
                    //dataGridView1.Rows.Add();
                    //dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = dataGridView1.RowCount;
                    //dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = strFileName;
                    //MessageBox.Show("Data is successfully saved in " + strFileName);
                }
                else
                    System.IO.File.Delete(strFileName);
            }
        }

        private void fIncrementTimer()
        {
            if (toolStripProgressBar1.Value > 99)
                toolStripProgressBar1.Value = 0;
            else
                toolStripProgressBar1.Value = toolStripProgressBar1.Value + 10;
        }
        private void StartTimer()
        {
            this.Cursor = Cursors.WaitCursor;
            toolStripProgressBar1.Visible = true;

        }
        private void StopTimer()
        {
            toolStripProgressBar1.Visible = false;
            this.Cursor = Cursors.Default;
        }

        private void btnWriteRTC_Click(object sender, EventArgs e)
        {
            EnableRTCControls(false);

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                               
                //Open meter selection window for GPRS Also.
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
            
                if (isChannelInitialized)
                {
                    //to do 

                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        EnableRTCControls(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    int writeResponse = WriteRTC();

                    if (writeResponse == (int)CoreUtility.DLMSResultType.Success)
                    {
                        MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)CoreUtility.DLMSResultType.AccessDenied)
                    {
                        MessageBox.Show("Access Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)CoreUtility.DLMSResultType.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                btnCancel.Enabled = true;
                btnWriteRTC.Enabled = true;
                EnableRTCControls(true);
            }
        }
        private int WriteRTC()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteRTC(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.ffillRTC(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;

                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void timerRTC_Tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentCulture = c;
            System.Threading.Thread.CurrentThread.CurrentUICulture = c;
            txtBoxRTC.Text = System.DateTime.Now.ToString();
        }

        private void btnReadRTC_Click(object sender, EventArgs e)
        {

            EnableRTCControls(false);
            //txtBoxRTC.Text = "";
            //timer1.Stop();
            this.Cursor = Cursors.WaitCursor;
            CommunicationType comType = CommunicationType.DIRECT;
            Application.DoEvents();
            bool isChannelInitialized = true;
            try
            {
                            
                //Enable meter selection window for GPRS also
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
               
                if (isChannelInitialized)
                {
                    //to do 

                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        EnableRTCControls(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    int writeResponse = ReadMeterRTC();

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        DisplayMeterRTC(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //txtBoxMeterID.Text = ReadMeterSerialNumber();
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                    {
                        MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteRTC.Enabled = true;
                btnReadRTC.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }

        private void EnableRTCControls(bool enable)
        {
            btnWriteRTC.Enabled = enable;
            btnReadRTC.Enabled = enable;
            btnCancel.Enabled = enable;
        }
        private int TryReadPCBAStatus()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPCBAStatus(HDLCCommand, HDLCIndex, 2);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }

        }
        private int ReadMeterRTC()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadRTC(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void DisplayMeterRTC(byte[] receivedData)
        {
            int year = 0;// receivedData[21];
            year = (year | (int)receivedData[20]) << 8;
            year = (year | (int)receivedData[21]);

            int month = receivedData[22];
            int date = receivedData[23];
            int hour = receivedData[25];
            int minute = receivedData[26];
            int second = receivedData[27];

            dGVReadRTC.Rows.Add();
            dGVReadRTC.Rows[dGVReadRTC.RowCount - 1].Cells["colSNo"].Value = dGVReadRTC.RowCount;
            dGVReadRTC.Rows[dGVReadRTC.RowCount - 1].Cells["colRTC"].Value = date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
        }

        private int WriteBillingDatetime(byte paramDate, byte paramHour, byte paramMinute)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteBillingDatetime(HDLCCommand, HDLCIndex, 4, paramDate, paramHour, paramMinute);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillBillingDatetime(HDLCCommand, HDLCIndex, date, hour, minute);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        private void btnWriteBillingDatetime_Click(object sender, EventArgs e)
        {
            if (cmbBoxBillingPeriod.Text == "")
            {
                MessageBox.Show("Billing Period can't be left blank.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }


            EnableBillingDateTimeControl(false);
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            bool isChannelInitialized = true;
            CommunicationType comType = CommunicationType.DIRECT;
            try
            {
               //Cabcon config commands if GSM is enabled      
                //If GPRS is enabled then Open imei number selector window for GPRS 
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {
                    //to do 


                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        EnableBillingDateTimeControl(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    byte hour, date, minute;

                    if (cmbBoxBillingPeriod.SelectedIndex == 1)
                    {
                        hour = Convert.ToByte(cmbBoxBillingHour.Text);
                        date = Convert.ToByte(cmbBoxBillingDate.Text);
                        minute = Convert.ToByte(cmbBoxBillingMinute.Text);
                    }
                    else
                    {
                        hour = 0xFF;
                        date = 0xFE;
                        minute = 0xFF;
                    }

                    //string meterID = txtBoxMeterID.Text.Trim();
                    int writeResponse = WriteBillingDatetime(date, hour, minute);

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        MessageBox.Show("Parameter written successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show(COMMessages.ACCESSDENIED, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteBillingDatetime.Enabled = true;
                btnReadBillingDatetime.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                btnCancel.Enabled = true;
            }
        }

        private void EnableBillingDateTimeControl(bool enable)
        {
            btnWriteBillingDatetime.Enabled = enable;
            btnReadBillingDatetime.Enabled = enable;
            btnCancel.Enabled = enable;
        }
        private int ReadBillingDatetime()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadBillingDatetime(HDLCCommand, HDLCIndex, 4);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void DisplayBillingDatetime(byte[] receivedData)
        {
            if (receivedData[0x21] == 0xFE && receivedData[0x18] == 0xFF && receivedData[0x19] == 0xFF)
            {
                cmbBoxBillingPeriod.SelectedIndex = 0;
                cmbBoxBillingDate.Text = "";
                cmbBoxBillingHour.Text = "";
                cmbBoxBillingMinute.Text = "";

                cmbBoxBillingDate.Enabled = false;
                cmbBoxBillingHour.Enabled = false;
                cmbBoxBillingMinute.Enabled = false;
            }
            else
            {
                cmbBoxBillingPeriod.SelectedIndex = 1;
                cmbBoxBillingDate.Text = receivedData[0x21].ToString();
                cmbBoxBillingHour.Text = receivedData[0x18].ToString();
                cmbBoxBillingMinute.Text = receivedData[0x19].ToString();

                cmbBoxBillingDate.Enabled = true;
                cmbBoxBillingHour.Enabled = true;
                cmbBoxBillingMinute.Enabled = true;
            }
        }

        private void btnReadBillingDatetime_Click(object sender, EventArgs e)
        {
            EnableBillingDateTimeControl(false);
            // cmbBoxBillingPeriod.Items.Clear();
            cmbBoxBillingDate.Items.Clear();
            cmbBoxBillingHour.Items.Clear();
            cmbBoxBillingMinute.Items.Clear();

            this.Cursor = Cursors.WaitCursor;

            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
               //Cabcon config commands if GSM is enabled      
                //Open sim selector if GPRS is enabled
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {
                    //to do 


                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        EnableBillingDateTimeControl(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    //string[] cmbItems = new string[cmbBoxBillingPeriod.Items.Count];
                    cmbBoxBillingPeriod.Items.Clear();
                    int writeResponse = ReadBillingDatetime();
                    cmbBoxBillingPeriod.Items.Add("End of Month");
                    cmbBoxBillingPeriod.Items.Add("User Defined");

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        DisplayBillingDatetime(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //txtBoxMeterID.Text = ReadMeterSerialNumber();
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                    {
                        MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteBillingDatetime.Enabled = true;
                btnReadBillingDatetime.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int fReadCTRatio()
        {
            int CTRatio = 1;
            HDLCIndex = 0;

            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadCTRatio(HDLCCommand, HDLCIndex, 2);

            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
            else
            {
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                {
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01)
                    {
                        /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */
                        byte[] ctRatioByte = new byte[2];
                        if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                        {
                            ctRatioByte[0] = 0;
                            ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                        {
                            ctRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                        }
                        CTRatio = int.Parse(String.Format("{0:X2}", ctRatioByte[0]) + String.Format("{0:X2}", ctRatioByte[1]), System.Globalization.NumberStyles.HexNumber);
                        /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */
                    }
                    else
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }

            }
            return CTRatio;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int fReadPTRatio()
        {
            int PTRatio = 1;
            HDLCIndex = 0;

            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPTRatio(HDLCCommand, HDLCIndex, 2);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
            else
            {
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                {
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    if (ret == 0x01)
                    {
                        byte[] ptRatioByte = new byte[2];
                        if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                        {
                            ptRatioByte[0] = 0;
                            ptRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                        {
                            ptRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            ptRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                        }
                        PTRatio = int.Parse(String.Format("{0:X2}", ptRatioByte[0]) + String.Format("{0:X2}", ptRatioByte[1]), System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }

            }
            return PTRatio;
        }

        private int WriteCTRatio(byte[] ctRatio, bool isRead)
        {
            HDLCIndex = 0;
            //7E            // start flag *
            //A0	
            // tag and legth
            //00	02	00	23	   // Server address *
            //21	               // Client adress *
            //54	               // Control Byte
            //AF	2E	           // HCS   
            //E6	E6	00	        //LC Bytes *
            //C1	01	            // tag for Writing *
            //C1	00	            // invoke id and prioroty *
            //01	                // Class  *
            //01	00	00	04	02	FF	    // Obis *
            //02	00	            // Attribute  *  
            //11	01	            // data type(byte) and Value *
            //7B	AC	            // fcs
            //7E                    // eND FALG *

            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            if (isRead)
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadCTRatio(HDLCCommand, HDLCIndex, 2);
            }
            else
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteCTRatio(HDLCCommand, HDLCIndex, 2);
            }
            if (!isRead)
            {
                HDLCIndex = GlobalObjects.objHDLCLIB.ffillCTRatio(HDLCCommand, HDLCIndex, ctRatio);
            }

            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
            else
            {
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                {
                    int ret;
                    if (isRead)
                    {
                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */
                            //byte[] ctRatioByte = new byte[2];
                            //ctRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            //ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                            //nudCTRatio.Value = int.Parse(String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[19]) + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[20]), System.Globalization.NumberStyles.HexNumber);

                            byte[] ctRatioByte = new byte[2];
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                            {
                                ctRatioByte[0] = 0;
                                ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                            {
                                ctRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                            }
                            nudCTRatio.Value = int.Parse(String.Format("{0:X2}", ctRatioByte[0]) + String.Format("{0:X2}", ctRatioByte[1]), System.Globalization.NumberStyles.HexNumber);
                            /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */
                        }

                    }
                    else
                    {
                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    }
                    if (ret == 0x01)
                    {
                        return (int)ProgrammingCode.Success;

                    }
                    else if (ret == 0x02)
                    {
                        return (int)ProgrammingCode.AccessDenied;
                    }
                    else
                    {
                        return (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                else
                    return (int)ProgrammingCode.CosemConnectionFailed;

            }
        }
        /*GKG 02/12/2013 PT RATIO CHANGES*/
        //private int WritePTRatio(byte ptRatio, bool isRead)
        private int WritePTRatio(int ptRatio, bool isRead)
        {
            HDLCIndex = 0;
            //7E            // start flag *
            //A0	
            // tag and legth
            //00	02	00	23	   // Server address *
            //21	               // Client adress *
            //54	               // Control Byte
            //AF	2E	           // HCS   
            //E6	E6	00	        //LC Bytes *
            //C1	01	            // tag for Writing *
            //C1	00	            // invoke id and prioroty *
            //01	                // Class  *
            //01	00	00	04	02	FF	    // Obis *
            //02	00	            // Attribute  *  
            //11	01	            // data type(byte) and Value *
            //7B	AC	            // fcs
            //7E                    // eND FALG *

            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            if (isRead)
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPTRatio(HDLCCommand, HDLCIndex, 2);
            }
            /*GKG 02/12/2013 PT RATIO CHANGES*/
            //else
            //{
            //    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWritePTRatio(HDLCCommand, HDLCIndex, 2);

                //}
            //if (!isRead)
            //{
            //    HDLCIndex = GlobalObjects.objHDLCLIB.ffillPTRatio(HDLCCommand, HDLCIndex, ptRatio);

                //}
            else
            {
                if (UtilityDetails.ShowTwoBytePTRatio)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWritePTRatio1(HDLCCommand, HDLCIndex, 2);
                    HDLCIndex = GlobalObjects.objHDLCLIB.ffillPTRatio(HDLCCommand, HDLCIndex, ptRatio);
                }
                else
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWritePTRatio(HDLCCommand, HDLCIndex, 2);
                    HDLCIndex = GlobalObjects.objHDLCLIB.ffillPTRatio(HDLCCommand, HDLCIndex, (byte)ptRatio);
                }
            }
            /*GKG 02/12/2013 PT RATIO CHANGES*/
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
            else
            {
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                {
                    int ret;
                    if (isRead)
                    {
                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    }
                    else
                    {
                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                    }
                    if (ret == 0x01)
                    {
                        if (isRead)
                        {
                            ///*GKG 02/12/2013 PT RATIO CHANGES*/
                            ////nudPTRatio.Value = int.Parse(String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[19]), System.Globalization.NumberStyles.HexNumber);
                            //if (UtilityDetails.PrimaryUtlityName == UtilityEntity.EXCELPOWER.ToString())
                            //{
                            //    nudPTRatio.Value = int.Parse(String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[19]) + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[20]), System.Globalization.NumberStyles.HexNumber);

                            //}
                            //else
                            //{
                            //    nudPTRatio.Value = int.Parse(String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[19]), System.Globalization.NumberStyles.HexNumber);
                            //}

                            ///*GKG 02/12/2013 PT RATIO CHANGES*/

                            /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */
                            //byte[] ctRatioByte = new byte[2];
                            //ctRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            //ctRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                            //nudCTRatio.Value = int.Parse(String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[19]) + String.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[20]), System.Globalization.NumberStyles.HexNumber);

                            byte[] ptRatioByte = new byte[2];
                            if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                            {
                                ptRatioByte[0] = 0;
                                ptRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            }
                            else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                            {
                                ptRatioByte[0] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[19];
                                ptRatioByte[1] = (byte)GlobalObjects.objSerialComm.ReceiveBuffer[20];
                            }
                            nudPTRatio.Value = int.Parse(String.Format("{0:X2}", ptRatioByte[0]) + String.Format("{0:X2}", ptRatioByte[1]), System.Globalization.NumberStyles.HexNumber);
                            /* GKG 23/04/2012 make Ct ratio generic for Ruby and PUma */

                        }

                        return (int)ProgrammingCode.Success;


                    }
                    /* GKG no need to use 0x02 but not changing as it is old code */
                    //else if (ret == 0x02 )
                    else if (ret == 0x02 || ret == 0x03)
                    {
                        return (int)ProgrammingCode.AccessDenied;
                    }

                    else
                    {
                        return (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                else
                    return (int)ProgrammingCode.CosemConnectionFailed;

            }
        }
        private int WriteLSCapturePeriod(int integrationPeriod)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteLSCapturePeriod(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.ffillLSCapturePeriod(HDLCCommand, HDLCIndex, integrationPeriod);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;

                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        private void btnWriteLSCapturePeriod_Click(object sender, EventArgs e)
        {
            if (cmbBoxLSCapturePeriod.Text == "")
            {
                MessageBox.Show("Load survey capture period can't be left blank.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            btnWriteLSCapturePeriod.Enabled = false;
            btnReadLSCapturePeriod.Enabled = false;
            btnCancel.Enabled = false;

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM or GPRS is enabled           
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {

                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        btnWriteLSCapturePeriod.Enabled = true;
                        btnReadLSCapturePeriod.Enabled = true;
                        btnCancel.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }


                    int writeResponse = WriteLSCapturePeriod(Convert.ToInt32(cmbBoxLSCapturePeriod.Text));

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        MessageBox.Show("Parameter written successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show(COMMessages.ACCESSDENIED, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteLSCapturePeriod.Enabled = true;
                btnReadLSCapturePeriod.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }
        private int ReadLSCapturePeriod()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadLSCapturePeriod(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        private void DisplayLSCapturePeriod(byte[] receivedData)
        {

            int compValue = 0;

            compValue = (compValue | (int)receivedData[19]) << 8;
            compValue = (compValue | (int)receivedData[20]);

            cmbBoxLSCapturePeriod.Text = Convert.ToString(compValue);
        }
        private void btnReadLSCapturePeriod_Click(object sender, EventArgs e)
        {
            btnWriteLSCapturePeriod.Enabled = false;
            btnReadLSCapturePeriod.Enabled = false;
            btnCancel.Enabled = false;
            cmbBoxLSCapturePeriod.Text = "";

            string[] cmbItems = new string[cmbBoxLSCapturePeriod.Items.Count];
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {

               //Cabcon config commands if GSM is enabled      
                //If GPRS or GSM is enabled for utility then do the channelInitialization.
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {

                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        btnWriteLSCapturePeriod.Enabled = true;
                        btnReadLSCapturePeriod.Enabled = true;
                        btnCancel.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    cmbBoxLSCapturePeriod.Items.CopyTo(cmbItems, 0);
                    cmbBoxLSCapturePeriod.Items.Clear();

                    int writeResponse = ReadLSCapturePeriod();
                    cmbBoxLSCapturePeriod.Items.AddRange(cmbItems);
                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        DisplayLSCapturePeriod(GlobalObjects.objSerialComm.ReceiveBuffer);
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                    {
                        MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteLSCapturePeriod.Enabled = true;
                btnReadLSCapturePeriod.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }
        private int WriteIntegrationPeriod(int integrationPeriod)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteIntegrationPeriod(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.ffillIntegrationPeriod(HDLCCommand, HDLCIndex, integrationPeriod);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;

                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        private void btnWriteIntegrationPeriod_Click(object sender, EventArgs e)
        {
            btnWriteIntegrationPeriod.Enabled = false;
            btnCancel.Enabled = false;

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                if (cmbBoxIntegrationPeriod.Text == string.Empty)
                {
                    MessageBox.Show("Please select an Integration Period", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    btnWriteIntegrationPeriod.Enabled = true;
                    btnCancel.Enabled = true;

                    this.Cursor = Cursors.Default;
                    return;
                }
               //Cabcon config commands if GSM is enabled      
                //If communication type is gprs then opend imei selector pop up.
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {

                    //Following condition added to resolve bug 74575; 18th April 2012
                    //if (cmbBoxIntegrationPeriod.Text == string.Empty)
                    //{
                    //    MessageBox.Show("Please select an Integration Period", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    //    btnWriteIntegrationPeriod.Enabled = true;
                    //    btnCancel.Enabled = true;

                    //    this.Cursor = Cursors.Default;
                    //    return;
                    //}



                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        btnWriteIntegrationPeriod.Enabled = true;
                        btnCancel.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }


                    int integrationPeriod = Convert.ToInt32(cmbBoxIntegrationPeriod.Text);

                    int writeResponse = WriteIntegrationPeriod(integrationPeriod);

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        MessageBox.Show("Parameter written successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show(COMMessages.ACCESSDENIED, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteIntegrationPeriod.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }

        private void btnReadIntegrationPeriod_Click(object sender, EventArgs e)
        {
            btnWriteIntegrationPeriod.Enabled = false;
            btnReadIntegrationPeriod.Enabled = false;
            btnCancel.Enabled = false;

            this.Cursor = Cursors.WaitCursor;

            Application.DoEvents();
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
               //Cabcon config commands if GSM is enabled      
                //if communication type is gprs then open imei popup screen
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {
                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        btnWriteIntegrationPeriod.Enabled = true;
                        btnReadIntegrationPeriod.Enabled = true;
                        btnCancel.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    string[] cmbItems = new string[cmbBoxIntegrationPeriod.Items.Count];
                    cmbBoxIntegrationPeriod.Items.CopyTo(cmbItems, 0);

                    cmbBoxIntegrationPeriod.Items.Clear();

                    int writeResponse = ReadIntegrationPeriod();

                    cmbBoxIntegrationPeriod.Items.AddRange(cmbItems);

                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        DisplayIntegrationPeriod(GlobalObjects.objSerialComm.ReceiveBuffer);
                        //txtBoxMeterID.Text = ReadMeterSerialNumber();
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                    {
                        MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteIntegrationPeriod.Enabled = true;
                btnReadIntegrationPeriod.Enabled = true;
                btnCancel.Enabled = true;
                if (isChannelInitialized)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }
        private int ReadIntegrationPeriod()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadIntegrationPeriod(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }


        private void DisplayIntegrationPeriod(byte[] receivedData)
        {

            int compValue = 0;

            compValue = (compValue | (int)receivedData[19]) << 8;
            compValue = (compValue | (int)receivedData[20]);

            cmbBoxIntegrationPeriod.Text = Convert.ToString(compValue);

        }

        private void cmbBoxBillingPeriod_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbBoxBillingPeriod.SelectedIndex == 1)
            {
                for (int i = 1; i <= 28; i++)
                {
                    cmbBoxBillingDate.Items.Add(i);
                }

                for (int i = 0; i <= 23; i++)
                {
                    cmbBoxBillingHour.Items.Add(i);
                }

                for (int i = 0; i <= 59; i++)
                {
                    cmbBoxBillingMinute.Items.Add(i);
                }
                cmbBoxBillingHour.SelectedIndex = 0;
                cmbBoxBillingMinute.SelectedIndex = 0;
                cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = true;
                cmbBoxBillingMinute.Enabled = true;
                cmbBoxBillingDate.Enabled = true;
            }
            else
            {
                cmbBoxBillingHour.Items.Clear();
                cmbBoxBillingMinute.Items.Clear();
                cmbBoxBillingDate.Items.Clear();

                cmbBoxBillingHour.Enabled = false;
                cmbBoxBillingMinute.Enabled = false;
                cmbBoxBillingDate.Enabled = false;
                Application.DoEvents();
            }
        }










        /// <summary>
        /// ******************************************************************************
        ///    Function Name    : InvalidData
        ///    Description      : This function indicates the user about an invalid entry.    
        /// ******************************************************************************* 
        /// </summary>

        private void InvalidData()
        {
            flgdataValid = false;
            //lblMonths.ForeColor = Color.Red;
            //lblMonths.Text = "Invalid Entry!";
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
        }

        private void btnGenerateFile_Click(object sender, EventArgs e)
        {
            String strFileName;
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            // Added to solve bug 94658
            string validationMessage = ValidateProgrammingData();
            if (string.IsNullOrEmpty(validationMessage))
            {
                SaveFileDialog1.Filter = "CFC Files (*.cfc)|*.cfc|All Files (*.*)|*.*";
                DialogResult result = SaveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    strFileName = SaveFileDialog1.FileName;
                }
                else
                    return;

                FileStream file1 = new FileStream(strFileName, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file1);

                if (fSaveParameters(file1, wr1))
                {
                    MessageBox.Show(resourceMgr.GetString("CFCCREATESUCCESS"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                wr1.Close();
                file1.Close();
            }
            else
            {

                MessageBox.Show(validationMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return;
            }
        }



        //The return type of the function changed to bool 18th April 2012
        private bool fSaveParameters(FileStream file1, StreamWriter wr1)
        {
            byte[] nDataBuffer = new byte[200];
            byte nDataIndex = 0;
            String strTemp = "";
            if (!UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
            {
                if (cmbBoxIntegrationPeriod.Text.Length == 0)
                {
                    MessageBox.Show("Please Select Integration Period.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
                int integrationPeriod = Convert.ToInt32(cmbBoxIntegrationPeriod.Text);

                wr1.Write("05");                 //identifier for integration Period
                nDataIndex = 0;
                nDataBuffer[nDataIndex++] = 0x01;//Convert.ToByte(ClassID);
                nDataBuffer[nDataIndex++] = 0x01; //00 00 60 01 00 255
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x08;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0x02;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x12;
                nDataBuffer[nDataIndex++] = Convert.ToByte((integrationPeriod & 0xFF00) >> 8);
                nDataBuffer[nDataIndex++] = Convert.ToByte(integrationPeriod & 0x00FF);




                for (int i = 0; i < nDataIndex; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", nDataBuffer[i]);
                }
                wr1.WriteLine(strTemp);
            }

            if (!UtilityDetails.DisableProgrammingBillingDateTime)
            {
                byte paramHour, paramDate, paramMinute;

                if (cmbBoxBillingPeriod.SelectedIndex == 1)
                {
                    paramHour = Convert.ToByte(cmbBoxBillingHour.Text);
                    paramDate = Convert.ToByte(cmbBoxBillingDate.Text);
                    paramMinute = Convert.ToByte(cmbBoxBillingMinute.Text);
                }
                else
                {
                    paramHour = 0xFF;
                    paramDate = 0xFE;
                    paramMinute = 0xFF;
                }
                wr1.Write("09");
                nDataIndex = 0;
                nDataBuffer[nDataIndex++] = 0x16;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x0F;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0x04;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x01;
                nDataBuffer[nDataIndex++] = 0x01;
                nDataBuffer[nDataIndex++] = 0x02;
                nDataBuffer[nDataIndex++] = 0x02;
                nDataBuffer[nDataIndex++] = 0x09;
                nDataBuffer[nDataIndex++] = 0x04;
                nDataBuffer[nDataIndex++] = paramHour;
                nDataBuffer[nDataIndex++] = paramMinute;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0x09;
                nDataBuffer[nDataIndex++] = 0x05;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = paramDate;
                nDataBuffer[nDataIndex++] = 0xFF;

                strTemp = "";
                for (int i = 0; i < nDataIndex; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", nDataBuffer[i]);
                }
                wr1.WriteLine(strTemp);
            }
            if (!UtilityDetails.DisableProgrammingSurveyCapturePeriod)
            {
                if (cmbBoxLSCapturePeriod.Text == "")
                {
                    MessageBox.Show("Load survey capture period can't be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
                int LSCapturePeriod = Convert.ToInt32(cmbBoxLSCapturePeriod.Text);
                wr1.Write("0C");                 //identifier for integration Period
                nDataIndex = 0;
                nDataBuffer[nDataIndex++] = 0x01;
                nDataBuffer[nDataIndex++] = 0x01;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x08;
                nDataBuffer[nDataIndex++] = 0x04;
                nDataBuffer[nDataIndex++] = 0xFF;
                nDataBuffer[nDataIndex++] = 0x02;
                nDataBuffer[nDataIndex++] = 0x00;
                nDataBuffer[nDataIndex++] = 0x12;
                nDataBuffer[nDataIndex++] = Convert.ToByte((LSCapturePeriod & 0xFF00) >> 8);
                nDataBuffer[nDataIndex++] = Convert.ToByte(LSCapturePeriod & 0x00FF);

                strTemp = "";
                for (int i = 0; i < nDataIndex; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", nDataBuffer[i]);
                }
                wr1.WriteLine(strTemp);
            }
            System.DateTime tdate;
            tdate = DateTime.Now.Date.AddDays(+1);

            DataGridView[] touGridNames = { gridTOUDay1, gridTOUDay2, gridTOUDay3, gridTOUDay4, gridTOUDay5, gridTOUDay6, gridTOUDay7, gridTOUDay8, gridTOUDay9, gridTOUDay10, gridTOUDay11, gridTOUDay12, gridTOUDay13, gridTOUDay14, gridTOUDay15, gridTOUDay16, gridTOUDay17, gridTOUDay18, gridTOUDay19, gridTOUDay20, gridTOUDay21, gridTOUDay22, gridTOUDay23, gridTOUDay24 };
            if (UtilityDetails.ShowOneTOU)
            {
                for (int rowCount = 0; rowCount < gridTOUDay1.RowCount; rowCount++)
                {
                    if (gridTOUDay1.Rows[rowCount].Cells["colTariff"].Value != null)
                    {
                        if (gridTOUDay1.Rows[rowCount].Cells[2].Value == null)
                        {
                            MessageBox.Show("TOU table not complete.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return false;
                        }
                        else
                        {
                            if (gridTOUDay1.Rows[rowCount].Cells[3].Value == null)
                            {
                                MessageBox.Show("TOU table not complete.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return false;
                            }
                        }
                    }
                }

            }
            else if (UtilityDetails.ShowTwoTOU)
            {
                for (int gridCount = 0; gridCount <= 1; gridCount++)
                {
                    if (touGridNames[gridCount].Rows[0].Cells["colTariff"].Value == null)
                    {
                        MessageBox.Show("TOU table can not be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
            }
            else// Else case added to resolve bug 74587; 18th April 2012
            {
                for (int gridCount = 0; gridCount <= touGridNames.GetUpperBound(0); gridCount++)
                {
                    if (touGridNames[gridCount].Rows[0].Cells["colTariff"].Value == null)
                    {
                        MessageBox.Show("TOU table can not be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
            }

            for (int rowCount = 0; rowCount < gridActivationDate.RowCount; rowCount++)
            {
                if (gridActivationDate.Rows[rowCount].Cells["colMonth"].Value == null)
                {
                    MessageBox.Show("TOU table can not be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }

            for (int rowCount = 0; rowCount < gridDayTables.RowCount; rowCount++)
            {
                //gridDayTables.Rows[rowCount].Cells["colZone"].Value = rowCount + 1;
                for (int colCount = 1; colCount < gridDayTables.ColumnCount; colCount++)
                {
                    if (gridDayTables.Rows[rowCount].Cells[colCount].Value == null)
                    {
                        MessageBox.Show("TOU table can not be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
            }

            wr1.Write("13");
            byte[] nTOUData = new byte[150000];
            int nIndex = 0;
            nTOUData[nIndex++] = 0x14;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x0D;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0xFF;
            nTOUData[nIndex++] = 0x07;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x01;
            nTOUData[nIndex++] = 0x04;

            for (byte i = 1; i < gridActivationDate.RowCount + 1; i++)// gridActivationDate.RowCount + 1 added instead of 5 to resolve bug 74587; 18th April 2012
            {
                nTOUData[nIndex++] = 0x02;
                nTOUData[nIndex++] = 0x03;
                nTOUData[nIndex++] = 0x09;
                nTOUData[nIndex++] = 0x01;
                // nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value);
                if (Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value) == 0x00)
                {
                    nTOUData[nIndex++] = 0x01;
                }
                else
                {
                    nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value);
                }
                nTOUData[nIndex++] = 0x09;
                nTOUData[nIndex++] = 0x0C;
                // added condition for PUMA

                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0xFF;


                nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colMonth"].Value);
                nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colDay"].Value);
                // added condition for PUMA

                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0x80;
                nTOUData[nIndex++] = 0x00;
                nTOUData[nIndex++] = 0x00;

                nTOUData[nIndex++] = 0x09;
                nTOUData[nIndex++] = 0x01;
                //nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value);         //Number of Week Profile
                if (Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value) == 0x00)
                {
                    nTOUData[nIndex++] = 0x01;
                }
                else
                {
                    nTOUData[nIndex++] = Convert.ToByte(gridActivationDate.Rows[i - 1].Cells["colSeason"].Value);
                }

            }
            strTemp = "";
            for (int i = 0; i < nIndex; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nTOUData[i]);
            }
            wr1.WriteLine(strTemp);


            wr1.Write("14");
            nIndex = 0;

            nTOUData[nIndex++] = 0x14;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x0D;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0xFF;
            nTOUData[nIndex++] = 0x08;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x01;
            nTOUData[nIndex++] = 0x04;          //Number of Week Profile
            for (byte i = 1; i < gridDayTables.RowCount + 1; i++) // gridDayTables.RowCount + 1 added instead of 5 to resolve bug 74587; 18th April 2012
            {
                nTOUData[nIndex++] = 0x02;
                nTOUData[nIndex++] = 0x08;
                nTOUData[nIndex++] = 0x09;
                nTOUData[nIndex++] = 0x01;
                nTOUData[nIndex++] = i;             //Number of Week profile 

                for (byte j = 1; j < 8; j++)
                {
                    nTOUData[nIndex++] = 0x11;
                    nTOUData[nIndex++] = Convert.ToByte(gridDayTables.Rows[i - 1].Cells[j].Value);      //day Id
                }
            }
            strTemp = "";
            for (int i = 0; i < nIndex; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nTOUData[i]);
            }
            wr1.WriteLine(strTemp);

            wr1.Write("15");
            nIndex = 0;

            nTOUData[nIndex++] = 0x14;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x0D;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0xFF;
            nTOUData[nIndex++] = 0x09;
            nTOUData[nIndex++] = 0x00;

            ///////////////////////writing of day profile /////////////////
            nTOUData[nIndex++] = 0x01;
            //Number of day Profile. 18 for non puma.
            int maxIndex = 0;
            if (UtilityDetails.ShowOneTOU)
            {
                nTOUData[nIndex++] = 0x01;
                maxIndex = 2;
            }
            else if (UtilityDetails.ShowTwoTOU)
            {
                nTOUData[nIndex++] = 0x02;
                maxIndex = 3;
            }
            else
            {
                nTOUData[nIndex++] = 0x18;
                maxIndex = 25;
            }


            for (byte i = 1; i < maxIndex; i++)
            {
                nTOUData[nIndex++] = 0x02;
                nTOUData[nIndex++] = 0x02;
                nTOUData[nIndex++] = 0x11;
                nTOUData[nIndex++] = i;             //Day Id 
                nTOUData[nIndex++] = 0x01;
                nTOUData[nIndex++] = 0x0A;             //Array Of 10

                for (byte j = 1; j < 11; j++)
                {
                    nTOUData[nIndex++] = 0x02;
                    nTOUData[nIndex++] = 0x03;
                    nTOUData[nIndex++] = 0x09;
                    nTOUData[nIndex++] = 0x04;
                    nTOUData[nIndex++] = Convert.ToByte(touGridNames[i - 1].Rows[j - 1].Cells["colStartHour"].Value);      //   Slot Start Hour
                    nTOUData[nIndex++] = Convert.ToByte(touGridNames[i - 1].Rows[j - 1].Cells["colStartMin"].Value);       //   Slot Start min
                    nTOUData[nIndex++] = 0x00;
                    nTOUData[nIndex++] = 0x00;
                    nTOUData[nIndex++] = 0x09;
                    nTOUData[nIndex++] = 0x06;
                    nTOUData[nIndex++] = 0x00;
                    nTOUData[nIndex++] = 0x00;
                    nTOUData[nIndex++] = 0x0A;
                    nTOUData[nIndex++] = 0x00;
                    nTOUData[nIndex++] = 0x64;
                    nTOUData[nIndex++] = 0xFF;
                    nTOUData[nIndex++] = 0x12;
                    nTOUData[nIndex++] = 0x00;

                    switch (Convert.ToString(touGridNames[i - 1].Rows[j - 1].Cells["colTariff"].Value))
                    {
                        case "T1":
                            nTOUData[nIndex++] = 1;
                            break;
                        case "T2":
                            nTOUData[nIndex++] = 2;
                            break;
                        case "T3":
                            nTOUData[nIndex++] = 3;
                            break;
                        case "T4":
                            nTOUData[nIndex++] = 4;
                            break;
                        case "T5":
                            nTOUData[nIndex++] = 5;
                            break;
                        case "T6":
                            nTOUData[nIndex++] = 6;
                            break;
                        case "T7":
                            nTOUData[nIndex++] = 7;
                            break;
                        case "T8":
                            nTOUData[nIndex++] = 8;
                            break;
                        default:
                            nTOUData[nIndex++] = 0;
                            break;
                    }
                }
            }

            strTemp = "";
            for (int i = 0; i < nIndex; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nTOUData[i]);
            }
            wr1.WriteLine(strTemp);
            wr1.Write("16");
            nIndex = 0;
            nTOUData[nIndex++] = 0x14;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x0D;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0xFF;
            nTOUData[nIndex++] = 0x0A;
            nTOUData[nIndex++] = 0x00;
            nTOUData[nIndex++] = 0x09;
            nTOUData[nIndex++] = 0x0C;
            nTOUData[nIndex++] = Convert.ToByte((dTPFutureActivationDate.Value.Year & 0xFF00) >> 8);
            nTOUData[nIndex++] = Convert.ToByte(dTPFutureActivationDate.Value.Year & 0x00FF);
            nTOUData[nIndex++] = Convert.ToByte(dTPFutureActivationDate.Value.Month); //month
            nTOUData[nIndex++] = Convert.ToByte(dTPFutureActivationDate.Value.Day);  //day of month
            // added condition for PUMA
            if (isPUMA)
            {
                nTOUData[nIndex++] = 0xFF;  //day of week
                nTOUData[nIndex++] = 0xFF;  //hh
                nTOUData[nIndex++] = 0xFF;  //mm
                nTOUData[nIndex++] = 0xFF;  //ss
                nTOUData[nIndex++] = 0xFF;
                nTOUData[nIndex++] = 0x80;
                nTOUData[nIndex++] = 0x00;
                nTOUData[nIndex++] = 0x00;
            }
            else
            {
                nTOUData[nIndex++] = 0x00;  //day of week
                nTOUData[nIndex++] = 0x00;  //hh
                nTOUData[nIndex++] = 0x00;  //mm
                nTOUData[nIndex++] = 0x00;  //ss
                nTOUData[nIndex++] = 0x00;
                nTOUData[nIndex++] = 0x00;
                nTOUData[nIndex++] = 0x00;
                nTOUData[nIndex++] = 0x00;
            }
            strTemp = "";
            for (int i = 0; i < nIndex; i++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nTOUData[i]);
            }
            wr1.WriteLine(strTemp);
            /*VBM - Added for MD Reset/display parameter feature enabled in US mode for both PUMA and RUBY*/
            if (UtilityDetails.ShowMDResetInUSMode ||
            (UtilityDetails.ShowFSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")))
            {
                WriteMDResetParameterToFile(wr1);
            }


            if ((UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs"))
                || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
            {
                WritePushButtonDisplayParameterinCFC(wr1);
                WriteScrollButtonDisplayParameterinCFC(wr1);
                WriteHighResolutionDisplayParameterinCFC(wr1);

                wr1.Write("24");
                strTemp = string.Empty;
                WriteDisplaySettingToFile(wr1);
            }
            if (UtilityDetails.ShowKVAHSelectionTabInFSMode || UtilityDetails.ShowKVAHSelectionTabInUSMode)
            {
                wr1.Write("20");
                WriteKVAHParameterToFile(wr1);
                strTemp = string.Empty;
            }
            /*VBM - Added for MD Reset/display parameter feature enabled in US mode for both PUMA and RUBY*/
            if (isPUMA)
            {
                wr1.Write("17");
                byte[] nCTRatio = new byte[50];
                byte[] nPTRatio = new byte[50];
                int nCTRatioDataIndex = 0;
                nCTRatio[nCTRatioDataIndex++] = 0x01;//Convert.ToByte(ClassID);
                nCTRatio[nCTRatioDataIndex++] = 0x01; //01 00 00 08 00 255
                nCTRatio[nCTRatioDataIndex++] = 0x00;
                nCTRatio[nCTRatioDataIndex++] = 0x00;
                nCTRatio[nCTRatioDataIndex++] = 0x04;
                nCTRatio[nCTRatioDataIndex++] = 0x02;
                nCTRatio[nCTRatioDataIndex++] = 0xFF;
                nCTRatio[nCTRatioDataIndex++] = 0x02;
                nCTRatio[nCTRatioDataIndex++] = 0x00;
                nCTRatio[nCTRatioDataIndex++] = 0x12;
                nCTRatio[nCTRatioDataIndex++] = Convert.ToByte((Convert.ToInt32(nudCTRatio.Value) & 0xFF00) >> 8);
                nCTRatio[nCTRatioDataIndex++] = Convert.ToByte(Convert.ToInt32(nudCTRatio.Value) & 0x00FF);
                strTemp = "";
                for (int dataLength = 0; dataLength < nCTRatioDataIndex; dataLength++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", nCTRatio[dataLength]);
                }
                wr1.WriteLine(strTemp);
                strTemp = "";
                nCTRatioDataIndex = 0;
                wr1.Write("18");
                nPTRatio[nCTRatioDataIndex++] = 0x01;//Convert.ToByte(ClassID);
                nPTRatio[nCTRatioDataIndex++] = 0x01; //01 00 00 08 00 255
                nPTRatio[nCTRatioDataIndex++] = 0x00;
                nPTRatio[nCTRatioDataIndex++] = 0x00;
                nPTRatio[nCTRatioDataIndex++] = 0x04;
                nPTRatio[nCTRatioDataIndex++] = 0x03;
                nPTRatio[nCTRatioDataIndex++] = 0xFF;
                nPTRatio[nCTRatioDataIndex++] = 0x02;
                nPTRatio[nCTRatioDataIndex++] = 0x00;
                /*GKG 02/12/2013 PT RATIO CHANGES*/
                //nPTRatio[nCTRatioDataIndex++] = 0x11;
                //nPTRatio[nCTRatioDataIndex++] = (byte)Convert.ToInt32(nudPTRatio.Value);
                if (UtilityDetails.ShowTwoBytePTRatio)
                {
                    nPTRatio[nCTRatioDataIndex++] = 0x12;
                    nPTRatio[nCTRatioDataIndex++] = Convert.ToByte((Convert.ToInt32(nudPTRatio.Value) & 0xFF00) >> 8);
                    nPTRatio[nCTRatioDataIndex++] = Convert.ToByte(Convert.ToInt32(nudPTRatio.Value) & 0x00FF);
                }
                else
                {
                    nPTRatio[nCTRatioDataIndex++] = 0x11;
                    nPTRatio[nCTRatioDataIndex++] = (byte)Convert.ToInt32(nudPTRatio.Value);
                }
                /*GKG 02/12/2013 PT RATIO CHANGES*/
                strTemp = "";
                for (int dataLength = 0; dataLength < nCTRatioDataIndex; dataLength++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", nPTRatio[dataLength]);
                }
                wr1.WriteLine(strTemp);

                /* VBM - Commented as this is handled feature wise above isPUMA check */
                // Tender PUMA TNEB TYPE
                //byte[] nMDReset = new byte[200];
                //int nMDResetDataIndex = 0;
                //wr1.Write("19");
                //nMDReset[nMDResetDataIndex++] = 0x09;//Convert.ToByte(ClassID);
                //nMDReset[nMDResetDataIndex++] = 0x00;
                //nMDReset[nMDResetDataIndex++] = 0x01;
                //nMDReset[nMDResetDataIndex++] = 0x0A;
                //nMDReset[nMDResetDataIndex++] = 0x09;
                //nMDReset[nMDResetDataIndex++] = 0x00;
                //nMDReset[nMDResetDataIndex++] = 0xFF;
                //nMDReset[nMDResetDataIndex++] = 0x01;// Convert.ToByte(AttributeID);
                //nMDReset[nMDResetDataIndex++] = 0x00;
                //nMDReset[nMDResetDataIndex++] = 0x09;
                //nMDReset[nMDResetDataIndex++] = (byte)Convert.ToInt32(chkMDReset.Checked);
                //strTemp = "";
                //for (int dataLength = 0; dataLength < nMDResetDataIndex; dataLength++)
                //{
                //    strTemp = strTemp + String.Format("{0:X2}", nMDReset[dataLength]);
                //}
                //wr1.WriteLine(strTemp);
                /* VBM - Commented as this is handled feature wise above isPUMA check */

                //Write KVAH Parameter to File
                wr1.Write("20");
                WriteKVAHParameterToFile(wr1);
                strTemp = string.Empty;
                // Write Push button
                /* VBM - Commented as this is handled feature wise above isPUMA check */
                //WritePushButtonDisplayParameterinCFC(wr1);
                //WriteScrollButtonDisplayParameterinCFC(wr1);
                //WriteHighResolutionDisplayParameterinCFC(wr1);
                ///* GKG 30/01/2012 TFS ID 134283 */
                ////if (UtilityDetails.ShowDisplayParameters)
                //if ((UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs"))
                //    || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                ///* GKG 30/01/2012 TFS ID 134283 */
                //{
                //    wr1.Write("24");
                //    strTemp = string.Empty;
                //    WriteDisplaySettingToFile(wr1);
                //}
                /* VBM - Commented as this is handled feature wise above isPUMA check */

            }
            return true;
        }

        private void WriteDisplaySettingToFile(StreamWriter wr1)
        {
            byte[] nDispTimeWrite = new byte[200];
            int nDisplayTimeoutIndex = 0;
            int pushTimeout, scrollTime, autoScrollModeSelected, autoScrollTime;
            ValidateDisplayParamsTimeout(txtScrollTime.Text.Trim(), txtPushButtonTimeout.Text.Trim(), txtScrollResumeTime.Text.Trim());

            pushTimeout = Convert.ToInt32(txtPushButtonTimeout.Text.Trim());
            scrollTime = Convert.ToInt32(txtScrollTime.Text.Trim());
            autoScrollModeSelected = (chkAutoScrollTime.Checked) ? 1 : 0;
            autoScrollTime = (autoScrollModeSelected == 0) ? 0 : Convert.ToInt32(txtScrollResumeTime.Text.Trim());

            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x01;//Convert.ToByte(ClassID);
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x00; //00 00 60 01 80 FF
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x00;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x60;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x01;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x80;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0xFF;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x02;// Convert.ToByte(AttributeID);
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x00;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x02;
            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x04;

            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x12;
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte((pushTimeout & 0xFF00) >> 8);
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte(pushTimeout & 0x00FF);

            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x12;
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte((scrollTime & 0xFF00) >> 8);
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte(scrollTime & 0x00FF);

            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x0F;
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte(autoScrollModeSelected);

            nDispTimeWrite[nDisplayTimeoutIndex++] = 0x12;
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte((autoScrollTime & 0xFF00) >> 8);
            nDispTimeWrite[nDisplayTimeoutIndex++] = Convert.ToByte(autoScrollTime & 0x00FF);


            string strTemp = string.Empty;
            for (int dataLength = 0; dataLength < nDisplayTimeoutIndex; dataLength++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nDispTimeWrite[dataLength]);
            }
            wr1.WriteLine(strTemp);

        }
        private void WritePushButtonDisplayParameterinCFC(StreamWriter streamWriter)
        {
            try
            {
                string strTemp = string.Empty;
                List<byte> selectedParams = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                if (selectedParams != null && selectedParams.Count > 0)
                {
                    streamWriter.Write("21");
                    streamWriter.Write(String.Format("{0:X2}", (byte)selectedParams.Count));
                    for (int dataLength = 0; dataLength < selectedParams.Count; dataLength++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", selectedParams[dataLength]);
                    }
                    streamWriter.WriteLine(strTemp);
                }
            }
            catch (Exception ex)
            { }

        }
        private void WriteScrollButtonDisplayParameterinCFC(StreamWriter streamWriter)
        {
            try
            {
                string strTemp = string.Empty;
                List<byte> selectedParams = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);

                if (selectedParams != null && selectedParams.Count > 0)
                {
                    streamWriter.Write("22");
                    streamWriter.Write(String.Format("{0:X2}", (byte)selectedParams.Count));
                    for (int dataLength = 0; dataLength < selectedParams.Count; dataLength++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", selectedParams[dataLength]);
                    }
                    streamWriter.WriteLine(strTemp);
                }
            }
            catch (Exception ex)
            { }

        }
        private void WriteHighResolutionDisplayParameterinCFC(StreamWriter streamWriter)
        {
            try
            {
                string strTemp = string.Empty;
                List<byte> selectedParams = GetSelectedRowsinParameterGrid(dGVHighResolution);

                if (selectedParams != null && selectedParams.Count > 0)
                {
                    streamWriter.Write("23");
                    streamWriter.Write(String.Format("{0:X2}", (byte)selectedParams.Count));
                    for (int dataLength = 0; dataLength < selectedParams.Count; dataLength++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", selectedParams[dataLength]);
                    }
                    streamWriter.WriteLine(strTemp);
                }
            }
            catch (Exception ex)
            { }

        }
        /// <summary>
        /// VBM - Write MDReset value in file .
        /// </summary>
        /// <param name="strWriter"></param>
        private void WriteMDResetParameterToFile(StreamWriter strWriter)
        {
            byte[] nMDReset = new byte[200];
            int nMDResetDataIndex = 0;
            strWriter.Write("19");
            nMDReset[nMDResetDataIndex++] = 0x09;//Convert.ToByte(ClassID);
            nMDReset[nMDResetDataIndex++] = 0x00;
            nMDReset[nMDResetDataIndex++] = 0x01;
            nMDReset[nMDResetDataIndex++] = 0x0A;
            nMDReset[nMDResetDataIndex++] = 0x09;
            nMDReset[nMDResetDataIndex++] = 0x00;
            nMDReset[nMDResetDataIndex++] = 0xFF;
            nMDReset[nMDResetDataIndex++] = 0x01;// Convert.ToByte(AttributeID);
            nMDReset[nMDResetDataIndex++] = 0x00;
            nMDReset[nMDResetDataIndex++] = 0x09;
            nMDReset[nMDResetDataIndex++] = (byte)Convert.ToInt32(chkMDReset.Checked);
            string strTemp = "";
            for (int dataLength = 0; dataLength < nMDResetDataIndex; dataLength++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nMDReset[dataLength]);
            }
            strWriter.WriteLine(strTemp);
        }
        /// <summary>
        /// Writes KVAH parameters to CFC File.
        /// </summary>
        /// <param name="wr1"></param>
        private void WriteKVAHParameterToFile(StreamWriter wr1)
        {
            byte[] nKVAHWrite = new byte[200];
            int nKVAHDataIndex = 0;
            nKVAHWrite[nKVAHDataIndex++] = 0x01;//Convert.ToByte(ClassID);
            nKVAHWrite[nKVAHDataIndex++] = 0x00; //00 00 60 01 8F FF
            nKVAHWrite[nKVAHDataIndex++] = 0x00;
            nKVAHWrite[nKVAHDataIndex++] = 0x60;
            nKVAHWrite[nKVAHDataIndex++] = 0x01;
            nKVAHWrite[nKVAHDataIndex++] = 0x8F;
            nKVAHWrite[nKVAHDataIndex++] = 0xFF;
            nKVAHWrite[nKVAHDataIndex++] = 0x02;// Convert.ToByte(AttributeID);
            nKVAHWrite[nKVAHDataIndex++] = 0x00;
            nKVAHWrite[nKVAHDataIndex++] = 0x09;
            nKVAHWrite[nKVAHDataIndex++] = 0x01;
            if (chkKVAhLagOnly.Checked)
            {
                nKVAHWrite[nKVAHDataIndex++] = 0x00;
            }
            else
            {
                nKVAHWrite[nKVAHDataIndex++] = 0x01;
            }

            string strTemp = string.Empty;
            for (int dataLength = 0; dataLength < nKVAHDataIndex; dataLength++)
            {
                strTemp = strTemp + String.Format("{0:X2}", nKVAHWrite[dataLength]);
            }
            wr1.WriteLine(strTemp);
        }

        private void fLoadParameters()
        {
            string validationMessage = string.Empty;
            String strFileName;
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "CFC Files (*.cfc)|*.cfc|All Files (*.*)|*.*";
            DialogResult result = OpenFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = OpenFileDialog1.FileName;
            }
            else
                return;

            FileStream file1 = new FileStream(strFileName, FileMode.Open);
            StreamReader read1 = new StreamReader(file1);
            byte[] nDataBuffer = new byte[20000];
            while (!read1.EndOfStream)
            {
                String data;
                data = read1.ReadLine();
                int nIndex = 0;
                for (int i = 0; i < data.Length; i = i + 2)
                {
                    nDataBuffer[nIndex++] = Convert.ToByte(data.Substring(i, 2), 16);
                }
                if (nDataBuffer[0] == 0x05)
                {
                    //cmbBoxIntegrationPeriod.Text = (nDataBuffer[11]).ToString();
                    int compValue = 0;

                    compValue = (compValue | (int)nDataBuffer[11]) << 8;
                    compValue = (compValue | (int)nDataBuffer[12]);

                    cmbBoxIntegrationPeriod.Text = Convert.ToString(compValue);
                }
                else if (nDataBuffer[0] == 0x17)
                {
                    if (!UtilityDetails.DisableProgrammingCTPTRatio)
                    {
                        nudCTRatio.Value = int.Parse(String.Format("{0:X2}", nDataBuffer[11]) + String.Format("{0:X2}", nDataBuffer[12]), System.Globalization.NumberStyles.HexNumber);

                    }

                }
                else if (nDataBuffer[0] == 0x18)
                {
                    if (!UtilityDetails.DisableProgrammingCTPTRatio)
                    {
                        /*GKG 02/12/2013 PT RATIO CHANGES*/
                        //nudPTRatio.Value = int.Parse(String.Format("{0:X2}", nDataBuffer[11]), System.Globalization.NumberStyles.HexNumber);
                        if (UtilityDetails.ShowTwoBytePTRatio)
                        {
                            nudPTRatio.Value = int.Parse(String.Format("{0:X2}", nDataBuffer[11]) + String.Format("{0:X2}", nDataBuffer[12]), System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            nudPTRatio.Value = int.Parse(String.Format("{0:X2}", nDataBuffer[11]), System.Globalization.NumberStyles.HexNumber);
                        }
                        /*GKG 02/12/2013 PT RATIO CHANGES*/
                    }
                }
                else if (nDataBuffer[0] == 0x19)
                {
                    if (UtilityDetails.ShowFSMode || UtilityDetails.ShowMDResetInUSMode)
                    {
                        chkMDReset.Checked = nDataBuffer[11] == 1 ? true : false;
                    }
                }
                //Check for KVAH Selection
                else if (nDataBuffer[0] == 0x20)
                {
                    //VBM : Enable kvah selection tab for Us mode.
                    if (UtilityDetails.ShowKVAHSelectionTabInFSMode || UtilityDetails.ShowKVAHSelectionTabInUSMode)
                    {
                        if (nDataBuffer[12] == 0)
                        {
                            chkKVAhLagOnly.Checked = true;
                        }
                        else
                        {
                            chkKVAhLagLead.Checked = true;
                        }
                    }
                }
                else if (nDataBuffer[0] == 0x21)
                {
                    if (UtilityDetails.ShowFSMode || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        selectedPushParams.Clear();
                        int length = (int)nDataBuffer[1];
                        for (int counter = 0; counter < length; counter++)
                        {
                            selectedPushParams.Add(nDataBuffer[counter + 2]);
                        }

                        /* GKG 30/01/2012 TFS ID 134432 */
                        if ((UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs"))
                            || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                        {
                            FillDisplayParameters(dGVPushDisplayParams, "PUSH");
                            dGVPushDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVPushDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVPushDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            FillDisplayParameters(selectedPushParams, dGVPushDisplayParams);
                            dGVPushDisplayParams.Refresh();
                        }
                        /* GKG 30/01/2012 TFS ID 134432 */



                    }
                }
                else if (nDataBuffer[0] == 0x22)
                {
                    if (UtilityDetails.ShowFSMode || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        selectedScrollParams.Clear();
                        int length = (int)nDataBuffer[1];
                        for (int counter = 0; counter < length; counter++)
                        {
                            selectedScrollParams.Add(nDataBuffer[counter + 2]);
                        }
                        /* GKG 30/01/2012 TFS ID 134432 */
                        if (UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")
                            || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                        {
                            FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
                            dGVScrollDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVScrollDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVScrollDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            FillDisplayParameters(selectedScrollParams, dGVScrollDisplayParams);
                            dGVScrollDisplayParams.Refresh();
                        }
                        /* GKG 30/01/2012 TFS ID 134432 */

                    }

                }
                else if (nDataBuffer[0] == 0x23)
                {
                    if (UtilityDetails.ShowFSMode || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        selectedHighResParams.Clear();
                        int length = (int)nDataBuffer[1];
                        for (int counter = 0; counter < length; counter++)
                        {
                            selectedHighResParams.Add(nDataBuffer[counter + 2]);
                        }
                        /* GKG 30/01/2012 TFS ID 134432 */
                        if (UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")
                            || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                        {
                            FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");
                            dGVHighResolution.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVHighResolution.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dGVHighResolution.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                            FillDisplayParameters(selectedHighResParams, dGVHighResolution);
                            dGVHighResolution.Refresh();
                        }
                        /* GKG 30/01/2012 TFS ID 134432 */
                    }


                }
                else if (nDataBuffer[0] == 0x24)
                {
                    if (UtilityDetails.ShowDisplayParameters || UtilityDetails.ShowDisplayParametersInUSMode)
                    {
                        txtPushButtonTimeout.Text = nDataBuffer[14].ToString();
                        txtScrollTime.Text = nDataBuffer[17].ToString();
                        if (Convert.ToInt32(nDataBuffer[22].ToString()) != 0)
                        {
                            txtScrollResumeTime.Text = nDataBuffer[22].ToString();
                        }
                    }


                }
                else if (nDataBuffer[0] == 0x09)
                {
                    if (nDataBuffer[25] == 0xFE && nDataBuffer[16] == 0xFF && nDataBuffer[17] == 0xFF)
                    {
                        cmbBoxBillingPeriod.SelectedIndex = 0;
                        cmbBoxBillingDate.Text = "";
                        cmbBoxBillingHour.Text = "";
                        cmbBoxBillingMinute.Text = "";

                        cmbBoxBillingDate.Enabled = false;
                        cmbBoxBillingHour.Enabled = false;
                        cmbBoxBillingMinute.Enabled = false;
                    }
                    else
                    {
                        cmbBoxBillingPeriod.SelectedIndex = 1;
                        cmbBoxBillingDate.Text = nDataBuffer[25].ToString();
                        cmbBoxBillingHour.Text = nDataBuffer[16].ToString();
                        cmbBoxBillingMinute.Text = nDataBuffer[17].ToString();

                        cmbBoxBillingDate.Enabled = true;
                        cmbBoxBillingHour.Enabled = true;
                        cmbBoxBillingMinute.Enabled = true;
                    }
                }
                else if (nDataBuffer[0] == 0x0C)
                {
                    int compValue = 0;

                    compValue = (compValue | (int)nDataBuffer[11]) << 8;
                    compValue = (compValue | (int)nDataBuffer[12]);

                    cmbBoxLSCapturePeriod.Text = Convert.ToString(compValue);
                }
                else if (nDataBuffer[0] == 0x13)
                {

                    byte[] touData = new byte[nDataBuffer.Length];
                    Array.Copy(nDataBuffer, 10, touData, 0, nDataBuffer.Length - 10);
                    touPresenter.FillSeasonProfileParameters(touData);
                }
                else if (nDataBuffer[0] == 0x14)
                {
                    byte[] touData = new byte[nDataBuffer.Length];
                    Array.Copy(nDataBuffer, 10, touData, 0, nDataBuffer.Length - 10);
                    touPresenter.FillWeekProfileParameters(touData);
                }
                else if (nDataBuffer[0] == 0x15)
                {
                    byte[] touData = new byte[nDataBuffer.Length];
                    Array.Copy(nDataBuffer, 10, touData, 0, nDataBuffer.Length - 10);
                    touPresenter.FillDayProfileParameters(touData, NamePlateConstants.PumaLTE650Value);

                }
                // Added to solve activation date not updating according to the cfc file.
                else if (nDataBuffer[0] == 0x16)
                {
                    byte[] touData = new byte[nDataBuffer.Length];
                    Array.Copy(nDataBuffer, 10, touData, 0, nDataBuffer.Length - 10);
                    touPresenter.FillTOUActivationDate(touData);
                }
            }
            read1.Close();
            file1.Close();
            validationMessage = ValidateProgrammingData();
            if (string.IsNullOrEmpty(validationMessage))
            {
                MessageBox.Show(resourceMgr.GetString("CFCUPLOADSUCCESS"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show(resourceMgr.GetString("CFCFILENOTCORRECT"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                fLoadParameters();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string ValidateProgrammingData()
        {
            string validationMessage = string.Empty;

            if (cmbBoxIntegrationPeriod.Text.Length == 0 && !UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
            {
                validationMessage += resourceMgr.GetString("SELECTINTEGRATIONPERIOD") + Symbols.NEWLINE;

            }
            if (cmbBoxLSCapturePeriod.Text == "" && !UtilityDetails.DisableProgrammingSurveyCapturePeriod)
            {
                validationMessage += resourceMgr.GetString("SELECTLSCAPTUREPERIOD") + Symbols.NEWLINE;

            }
            if (cmbBoxBillingPeriod.Text == "" && !UtilityDetails.DisableProgrammingBillingDateTime)
            {
                validationMessage += resourceMgr.GetString("SELECTBILLINGPARAMETERS") + Symbols.NEWLINE;

            }
            if (!touPresenter.ValidateTOUGrids())
            {
                validationMessage += CoreUtility.ExpMessage + Symbols.NEWLINE;
            }

            if (!UtilityDetails.DisableProgrammingCTPTRatio)
            {
                if (nudCTRatio.Value <= 0)
                {
                    validationMessage += CTRATIOVALMESAGE + nudCTRatio.Maximum + Symbols.NEWLINE;
                }
                if (nudPTRatio.Value <= 0)
                {
                    validationMessage += PTRATIOVALMESAGE + nudPTRatio.Maximum + Symbols.NEWLINE;
                }


            }
            //VBM - Enable kvah selection in US mode.
            if ((UtilityDetails.ShowKVAHSelectionTabInFSMode && (!chkKVAhLagOnly.Checked && !chkKVAhLagLead.Checked) && string.Equals(ReadOutMode, "FS", StringComparison.OrdinalIgnoreCase)) || (UtilityDetails.ShowKVAHSelectionTabInUSMode && (!chkKVAhLagOnly.Checked && !chkKVAhLagLead.Checked) && string.Equals(ReadOutMode, "US", StringComparison.OrdinalIgnoreCase)))
            {
                validationMessage += KVAHSELECTION + Symbols.NEWLINE;
            }
            if (UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")
                || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
            {
                List<byte> pushDisplayParams = null;
                List<byte> scrollDisplayParams = null;
                List<byte> highResDisplayParams = null;

                pushDisplayParams = GetSelectedRowsDisplayParameterGrid(0);
                scrollDisplayParams = GetSelectedRowsDisplayParameterGrid(1);
                highResDisplayParams = GetSelectedRowsDisplayParameterGrid(2);
                if (pushDisplayParams.Count == 0)
                {
                    validationMessage += PUSHBUTTONVALMESSAGE + Symbols.NEWLINE;
                }
                if (scrollDisplayParams.Count == 0)
                {
                    validationMessage += SCROLLBUTTONVALMESSAGE + Symbols.NEWLINE;
                }
                if (highResDisplayParams.Count == 0)
                {
                    validationMessage += HIGHRESOLUTIONVALMESSAGE + Symbols.NEWLINE;
                }

                string retMessage = ValidateDisplayTimeout(txtScrollTime.Text.Trim(), txtPushButtonTimeout.Text.Trim(), txtScrollResumeTime.Text.Trim());
                if (!string.IsNullOrEmpty(retMessage))
                {
                    validationMessage += retMessage + Symbols.NEWLINE;
                }
            }
            return validationMessage;

        }
        private void button4_Click(object sender, EventArgs e)
        {

            string validationMessage = ValidateProgrammingData();

            if (!string.IsNullOrEmpty(validationMessage))
            {
                MessageBox.Show(validationMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            try
            {
                ////////// Inter Frame    //////
                int writeResponse = 0;
                StartTimer();
                Application.DoEvents();
                fIncrementTimer();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    this.Cursor = Cursors.Default;
                    Application.DoEvents();
                    StopTimer();
                    return;
                }

                writeResponse = WriteRTC();

                if (writeResponse == (int)ProgrammingCode.Success)
                {

                    DLMSMain.fDLMSDisconnect();


                    StartTimer();
                    Application.DoEvents();
                    fIncrementTimer();
                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        StopTimer();
                        return;
                    }

                    ////////// Billing date and time//////\
                    //VBM - Write only when its enabled for utility
                    if (!UtilityDetails.DisableProgrammingBillingDateTime)
                    {
                        byte hour, date, minute;

                        if (cmbBoxBillingPeriod.SelectedIndex == 1)
                        {
                            hour = Convert.ToByte(cmbBoxBillingHour.Text);
                            date = Convert.ToByte(cmbBoxBillingDate.Text);
                            minute = Convert.ToByte(cmbBoxBillingMinute.Text);
                        }
                        else
                        {
                            hour = 0xFF;
                            date = 0xFE;
                            minute = 0xFF;
                        }

                        fIncrementTimer();

                        writeResponse = WriteBillingDatetime(date, hour, minute);
                        fIncrementTimer();
                        if (writeResponse == (int)ProgrammingCode.Success)
                        { }
                        else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                        {
                            MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    fIncrementTimer();


                    byte[] touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveSeasonProfile, NamePlateConstants.PumaLTE650Value);
                    writeResponse = WriteTOU(touData, touData.Length, (byte)ActivityCalender.PassiveSeasonProfile);
                    fIncrementTimer();
                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveWeekProfile, NamePlateConstants.PumaLTE650Value);
                        writeResponse = WriteTOU(touData, touData.Length, (byte)ActivityCalender.PassiveWeekProfile);
                        fIncrementTimer();
                        if (writeResponse == (int)ProgrammingCode.Success)
                        {
                            touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveDayProfile, NamePlateConstants.PumaLTE650Value);
                            writeResponse = WriteTOUBlock(touData, touData.Length, (byte)ActivityCalender.PassiveDayProfile);
                            fIncrementTimer();
                            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString() || UtilityDetails.PrimaryUtlityName == UtilityEntity.NDPL.ToString()
                                || UtilityDetails.PrimaryUtlityName == UtilityEntity.UPCONTRACTORS.ToString()
                                 || UtilityDetails.PrimaryUtlityName == UtilityEntity.DGVCL.ToString())
                            {
                                if (writeResponse == (int)ProgrammingCode.Success)
                                {
                                    touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveSeasonProfile, NamePlateConstants.RubyE250Value);
                                    writeResponse = WriteTOU(touData, touData.Length, (byte)ActivityCalender.PassiveSeasonProfile);
                                    fIncrementTimer();
                                    if (writeResponse == (int)ProgrammingCode.Success)
                                    {
                                        touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveWeekProfile, NamePlateConstants.RubyE250Value);
                                        writeResponse = WriteTOU(touData, touData.Length, (byte)ActivityCalender.PassiveWeekProfile);
                                        fIncrementTimer();
                                        if (writeResponse == (int)ProgrammingCode.Success)
                                        {
                                            touData = touPresenter.GetTOUBuffer(ActivityCalender.PassiveDayProfile, NamePlateConstants.RubyE250Value);
                                            writeResponse = WriteTOUBlock(touData, touData.Length, (byte)ActivityCalender.PassiveDayProfile);
                                        }
                                    }
                                }
                            }
                            fIncrementTimer();
                            if (writeResponse == (int)ProgrammingCode.Success)
                            {
                                touPresenter.ActivationDate = dTPFutureActivationDate.Value;
                                touData = touPresenter.GetTOUBuffer(ActivityCalender.ActivationDate, NamePlateConstants.PumaLTE650Value);
                                writeResponse = WriteTOU(touData, touData.Length, (byte)ActivityCalender.ActivationDate);

                                if (writeResponse == (int)ProgrammingCode.Success)
                                {
                                }
                                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                                {
                                    MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                    return;
                                }
                                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                                {
                                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                    return;
                                }
                            }
                            else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                            {
                                MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                            {
                                MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }
                        else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                        {
                            MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    fIncrementTimer();
                    if (!UtilityDetails.DisableProgrammingSurveyCapturePeriod)
                    {
                        //////// LS capture Period/////////
                        if (cmbBoxLSCapturePeriod.Text == "")
                        {
                            MessageBox.Show("Load survey capture period can't be left blank.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }

                        writeResponse = WriteLSCapturePeriod(Convert.ToInt32(cmbBoxLSCapturePeriod.Text));
                        fIncrementTimer();

                        if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                        {
                            MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                    fIncrementTimer();
                    ////////// integration period    //////
                    if (!UtilityDetails.DisableProgrammingDemandIntegrationPeriod)
                    {
                        int integrationPeriod = Convert.ToInt32(cmbBoxIntegrationPeriod.Text);
                        fIncrementTimer();
                        writeResponse = WriteIntegrationPeriod(integrationPeriod);
                    }
                    //Piyush : Try to write CT PT ratio only when utility supports it.
                    if (!UtilityDetails.DisableProgrammingCTPTRatio)
                    {
                        fIncrementTimer();
                        writeResponse = WriteCTRatio();
                        fIncrementTimer();
                        writeResponse = WritePTRatio();
                        fIncrementTimer();
                    }
                    /* GKG 04/02/2013 TFS ID 134922 */
                    //if (UtilityDetails.ShowFSMode)
                    if ((UtilityDetails.ShowFSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")) || (UtilityDetails.ShowMDResetInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    /* GKG 04/02/2013 TFS ID 134922 */
                    {
                        if (chkMDReset.Checked)
                        {
                            writeResponse = WriteMDResetCommand();
                        }

                        fIncrementTimer();
                        //Write KVAH Selection Command
                        // VBM : commneted this code here and moved it just below.
                        //if (chkKVAhLagOnly.Checked || chkKVAhLagLead.Checked)
                        //{
                        //    writeResponse = WriteKVAHParameterToCMRI();
                        //}
                    }
                    //VBM : enable kvah selection in US mode 
                    if ((UtilityDetails.ShowFSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")) || (UtilityDetails.ShowKVAHSelectionTabInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        if (chkKVAhLagOnly.Checked || chkKVAhLagLead.Checked)
                        {
                            writeResponse = WriteKVAHParameterToCMRI();
                        }
                    }

                    fIncrementTimer();
                    if (UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")
                        || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        //Push Params
                        writeResponse = WriteDisplayParameterToCMRI(0);

                        //Scroll Params
                        writeResponse = WriteDisplayParameterToCMRI(1);

                        //HighResolution params
                        writeResponse = WriteDisplayParameterToCMRI(2);
                    }
                    fIncrementTimer();
                    if (UtilityDetails.ShowDisplayParameters && cmbMode.SelectedItem.ToString().ToLower().Contains("fs")
                        || (UtilityDetails.ShowDisplayParametersInUSMode && cmbMode.SelectedItem.ToString().ToLower().Contains("us")))
                    {
                        int scrollResumeTime = string.IsNullOrEmpty(txtScrollResumeTime.Text.Trim()) ? 0 : Convert.ToInt32(txtScrollResumeTime.Text);
                        int scrollResumeSelected = (chkAutoScrollTime.Checked) ? 1 : 0;
                        writeResponse = WriteDisplayTimeouts(Convert.ToInt32(txtPushButtonTimeout.Text), Convert.ToInt32(txtScrollTime.Text), scrollResumeTime, scrollResumeSelected);
                    }
                    if (writeResponse == (int)ProgrammingCode.Success)
                    {
                        StopTimer();
                        MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                    {
                        MessageBox.Show(COMMessages.ACCESSDENIED, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    }
                    //MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                }


                DLMSMain.fDLMSDisconnect();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopTimer();
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                this.Cursor = Cursors.Default;
                btnCancel.Enabled = true;
            }
        }

        private int WriteTOU(byte[] nDataArray, int nLength, byte attribute)
        {
            int writeEnum = 0;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteTOU(HDLCCommand, HDLCIndex, attribute);

                for (int dataByteCount = 0; dataByteCount < nLength; dataByteCount++)
                {
                    HDLCCommand[HDLCIndex++] = nDataArray[dataByteCount];
                }

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            writeEnum = (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else if (ret == 0x03)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                return writeEnum;
            }
            catch (Exception ex)
            {
                writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                return writeEnum;
            }
        }

        /// <summary>
        /// This method is used to write TOU block into the meter.
        /// </summary>
        /// <param name="nDataArray">Pass data array.</param>
        /// <param name="nLength">Length of the array.</param>
        /// <param name="attribute">Attribute</param>
        /// <returns></returns>
        public int WriteTOUBlock(byte[] nDataArray, int nLength, byte attribute)
        {
            try
            {
                int nErrorCode = 0x00;
                bool nBlkTransfer = false;
                while (true)
                {
                    HDLCIndex = 0;
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                    GlobalObjects.objHDLCLIB.fIncSend();
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                    if (nBlkTransfer == false)
                    {
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = nLength;
                        HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteTOUBlock(HDLCCommand, HDLCIndex, attribute);
                    }
                    else
                    {
                        HDLCCommand[HDLCIndex++] = 0xC1;
                        HDLCCommand[HDLCIndex++] = 0x03;
                        HDLCCommand[HDLCIndex++] = 0xC1;
                    }

                    HDLCIndex = GlobalObjects.objCOSEMLIB.fSetBlockTransferPacket(HDLCCommand, HDLCIndex, nDataArray, nBlkTransfer);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                    GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                    GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                    HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                    if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                    {
                        nErrorCode = (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        break;
                    }
                    else
                    {
                        //////Application.DoEvents();
                        GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                        if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                        {
                            int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                            if (ret == 0x01)
                            {
                                nErrorCode = (int)CoreUtility.DLMSResultType.Success;
                                break;
                            }
                            else if (ret == 0x02)
                            {
                                nErrorCode = (int)CoreUtility.DLMSResultType.AccessDenied;
                                break;
                            }
                            else if (ret == 0x04)
                            {
                                nErrorCode = 0x4;
                                nBlkTransfer = true;
                            }
                            else
                            {
                                nErrorCode = (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                                break;
                            }
                        }
                        else
                        {
                            nErrorCode = (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                            break;
                        }
                    }
                }
                return nErrorCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Write KVAH Parameters to CMRI
        /// </summary>
        /// <returns></returns>
        private int WriteKVAHParameterToCMRI()
        {
            int writeResponse;
            if (chkKVAhLagOnly.Checked)
            {
                writeResponse = (int)fWriteKVAhSelection(0x00);
            }
            else
            {
                writeResponse = (int)fWriteKVAhSelection(0x01);
            }

            return writeResponse;
        }

        private void btnWriteAll_Click(object sender, EventArgs e)
        {
            btnWriteAll.Enabled = false;
            btnCancel.Enabled = false;

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            if (DLMSMain.fDLMSConnect() != true)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return;
            }
            try
            {
                int writeResponse = WriteRTC();

                if (writeResponse == (int)ProgrammingCode.Success)
                {
                    MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnWriteAll.Enabled = true;
                DLMSMain.fDLMSDisconnect();
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                this.Cursor = Cursors.Default;
                btnCancel.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private bool fCheckBCC(String strFileName)
        {
            string[] lines = File.ReadAllLines(strFileName);
            StringBuilder sb = new StringBuilder();
            int count = lines.Length - 1; // except last line 
            int i;
            for (i = 0; i < count; i++)
            {
                sb.AppendLine(lines[i]);
            }
            File.WriteAllText("output.txt", sb.ToString());
            String temp = lines[i];
            if (temp == GetMD5ChecksumForFile("output.txt"))
                return true;
            else
                return false;
        }

        private static string GetMD5ChecksumForFile(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(filename))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", filename));

            using (FileStream fstream = new FileStream(filename, FileMode.Open))
            {
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fstream);

                // Convert the byte array to a printable string.
                StringBuilder sb = new StringBuilder(32);
                foreach (byte hex in hash)
                    sb.Append(hex.ToString("X2"));

                return sb.ToString().ToUpper();
            }
        }

        private void rdBtnReadCompleteEvent_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnReadCompleteEvent.Checked == true)
            {
                cmbBoxLastFromEvent.Enabled = false;
                cmbBoxFromEvent.Enabled = false;
                cmbBoxToEvent.Enabled = false;
            }
        }

        private void rdBtnReadLastEvent_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnReadLastEvent.Checked == true)
                cmbBoxLastFromEvent.Enabled = true;
            else
                cmbBoxLastFromEvent.Enabled = false;
        }

        private void rdBtnReadBetweenEvent_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnReadBetweenEvent.Checked == true)
            {
                cmbBoxFromEvent.Enabled = true;
                cmbBoxToEvent.Enabled = true;
            }
            else
            {
                cmbBoxFromEvent.Enabled = false;
                cmbBoxToEvent.Enabled = false; ;
            }
        }

        private void chkTamper_CheckedChanged(object sender, EventArgs e)
        {
            chkInsta_CheckedChanged(sender, e);
            if (!rdFastDownload.Checked)
            {
                if (chkTamper.Checked == true)
                    grpBoxEventLog.Enabled = true;
                else
                    grpBoxEventLog.Enabled = false;
            }
        }

        private void chkLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            chkInsta_CheckedChanged(sender, e);
            if (!rdFastDownload.Checked)
            {
                if (chkLoadSurvey.Checked == true)
                    grpBoxLS.Enabled = true;
                else
                    grpBoxLS.Enabled = false;
            }
        }

        private void chkBilling_CheckedChanged(object sender, EventArgs e)
        {
            chkInsta_CheckedChanged(sender, e);
            if (!rdFastDownload.Checked)
            {
                if (chkBilling.Checked == true)
                    grpBoxBillingHistory.Enabled = true;
                else
                    grpBoxBillingHistory.Enabled = false;
            }
        }

        private void rdBtnReadCompleteLS_CheckedChanged(object sender, EventArgs e)
        {
            dtPickerFrom.Enabled = false;
            dtPickerTo.Enabled = false;
            cmbLSDays.Enabled = true;
        }

        private void rdBtnReadBetweenLS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnReadBetweenLS.Checked == true)
            {
                dtPickerFrom.Enabled = true;
                dtPickerTo.Enabled = true;
                cmbLSDays.Enabled = false;
            }
            else
            {
                dtPickerFrom.Enabled = false;
                dtPickerTo.Enabled = false;
                cmbLSDays.Enabled = true;
            }
        }

        private void rdBtnReadComplete_CheckedChanged(object sender, EventArgs e)
        {
            cmbBoxLastFrom.Enabled = false;
        }

        private void rdBtnReadLast_CheckedChanged(object sender, EventArgs e)
        {
            cmbBoxLastFrom.Enabled = true;
        }

        private void cmbCMRIType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialPortSettings.Default.IntercharacterDelay = Convert.ToInt32(CMRIIntercharacterDelay[cmbCMRIType.SelectedIndex]);
            SerialPortSettings.Default.CommandTimeOut = Convert.ToInt32(CMRICommandTimeOut[cmbCMRIType.SelectedIndex]);
        }

        // To set the System Settings value.
        private void SetMultiplePortValue()
        {

            string s = SystemSettings.USE_MULTIPLE_PORTS.ToString();
            if (rdbSinglePort.Checked)
            {
                objSystemSettings.UpdateSetting(s, "0");
            }
            if (rdbMultiplePorts.Checked)
            {
                objSystemSettings.UpdateSetting(s, "1");
            }
        }

        private bool IsMultiplePortSelected
        {
            get
            {
                string strTemp = objSystemSettings.GetSettingValue(SystemSettings.USE_MULTIPLE_PORTS);
                if (!string.IsNullOrEmpty(strTemp))
                {
                    if (strTemp.Equals("1"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private void rdbSinglePort_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSinglePort.Checked)
            {
                dgvPortUsageAssociation.Visible = false;
                groupBox65.Visible = true;
                btnTestConnection.Visible = false;
            }
            else
            {
                dgvPortUsageAssociation.Visible = true;
                groupBox65.Visible = false;
                btnTestConnection.Visible = true;
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string strModemPorts, strCMRIPort;
            string strPortName = string.Empty;
            string[] arrModemPorts;
            List<string> lstFailedPorts = new List<string>(), lstPassedPorts = new List<string>();
            try
            {
                btnTestConnection.Enabled = false;
                isPortAssociationChanged = false;
                PreviousPortAssociationColIndex = PreviousPortAssociationRowIndex = -1;
                Cursor.Current = Cursors.WaitCursor;
                if (ValidatePortMapping(out strModemPorts, out strCMRIPort))
                {
                    string strMessage = string.Empty, strCaption = string.Empty;
                    MessageBoxIcon mbIcon = MessageBoxIcon.None;
                    if (!string.IsNullOrEmpty(strModemPorts))
                    {
                        foreach (CABSerialPort objSerialPort in CABSerialPorts.ListOfSerialPorts)
                        {
                            if (objSerialPort.IsResponding &&
                                ("," + strModemPorts + ",").Contains("," + objSerialPort.PortName + ","))
                            {
                                lstPassedPorts.Add(objSerialPort.PortName);
                            }
                        }
                        arrModemPorts = strModemPorts.Split(',');
                        for (int i = 0; i < arrModemPorts.Length; i++)
                        {
                            strPortName = arrModemPorts[i];
                            if (!lstPassedPorts.Contains(strPortName))
                            {
                                if (CheckModemExistOrAvailable(strPortName))
                                {
                                    lstPassedPorts.Add(strPortName);
                                    CABSerialPorts.SetPortRespondingStatus(strPortName, true);
                                }
                                else
                                {
                                    lstFailedPorts.Add(strPortName);
                                    CABSerialPorts.SetPortRespondingStatus(strPortName, false);
                                }
                            }
                        }
                        if (lstFailedPorts.Count == 0 &&
                            lstPassedPorts.Count > 0)
                        {
                            strMessage = "All ports selected for GSM Modem connection are responding properly.";
                            strCaption = "Test Connections - SUCCESS";
                            mbIcon = MessageBoxIcon.Information;
                            isConnectionTested = true;
                        }
                        else if (lstFailedPorts.Count > 0)
                        {
                            if (lstPassedPorts.Count == 0)
                            {
                                strMessage = "None of the ports selected for GSM Modem connection are responding.\n\nPlease check the connections.";
                                strCaption = "Test Connections - FAILED";
                                mbIcon = MessageBoxIcon.Error;
                                isConnectionTested = false;
                            }
                            else
                            {
                                strMessage = "The GSM Modem(s) on Port(s): [";
                                foreach (string PassedPort in lstPassedPorts)
                                {
                                    strMessage += PassedPort + ",";
                                }
                                strMessage = strMessage.TrimEnd(',');
                                strMessage += "] are responding properly.";
                                strMessage += "\n\nBut, the Port(s): [";
                                foreach (string FailedPort in lstFailedPorts)
                                {
                                    strMessage += FailedPort + ",";
                                }
                                strMessage = strMessage.TrimEnd(',');
                                strMessage += "] failed to respond.\nPlease check the connections on these ports.";
                                strCaption = "Test Connections - WARNING";
                                mbIcon = MessageBoxIcon.Warning;
                                isConnectionTested = false;
                            }
                        }
                    }
                    else
                    {
                        strMessage = "No port selected for Modem connection.";
                        strCaption = "Test Connections - WARNING";
                        mbIcon = MessageBoxIcon.Warning;
                        isConnectionTested = false;
                    }
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, mbIcon);
                }
                else
                {
                    isConnectionTested = false;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Please check your port mapping!", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(strPortName))
                {
                    strPortName = "[" + strPortName + "] ";
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error in Testing Connection: " + strPortName + ex.Message, "Test Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectionTested = false;
            }
        }
        public bool CheckModemExistOrAvailable(string portName)
        {
            SerialComm objSerialComm = null;
            try
            {
                objSerialComm = new SerialComm();
                objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                objSerialComm.SetSerialPortSettings(portName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                if (objSerialComm.OpenPort())
                {
                    objSerialComm.CommandTimeout = 6000;
                    objSerialComm.bCommType = 1;
                    objSerialComm.InterchatracterDelay = 5000;
                    objSerialComm.timeout = 5500;
                    string Result = SendCommandToModem("AT", objSerialComm);
                    if (Result == "\r\nOK\r\n")
                    {
                        objSerialComm.ClosePort();
                        return true;
                    }
                    else
                    {
                        objSerialComm.ClosePort();
                        return false;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objSerialComm != null)
                {
                    objSerialComm.ClosePort();
                }
            }
            return false;
        }
        /// <summary>
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <returns></returns>
        private string SendCommandToModem(string command, SerialComm pSerialComm)
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');

                if (pSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    return "Modem Time Out.";
                }
                else
                {
                    for (int i = 0; i < pSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(pSerialComm.ReceiveBuffer[i]);
                    }

                    return CommandResult;
                }
            }
            catch
            {
                throw;
            }
        }

        private void dgvPortUsageAssociation_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!isPortAssociationChanged &&
                (dgvPortUsageAssociation.Columns[e.ColumnIndex].Name.Equals(colPortUsageTypeModem.Name) ||
                dgvPortUsageAssociation.Columns[e.ColumnIndex].Name.Equals(colPortUsageTypeCMRI.Name)))
            {
                PreviousPortAssociationColIndex = e.ColumnIndex;
                PreviousPortAssociationRowIndex = e.RowIndex;
                PreviousPortAssociationValue = Convert.ToBoolean(dgvPortUsageAssociation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
        }

        private void dgvPortUsageAssociation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!isPortAssociationChanged &&
                e.ColumnIndex == PreviousPortAssociationColIndex &&
                e.RowIndex == PreviousPortAssociationRowIndex &&
                PreviousPortAssociationValue == !Convert.ToBoolean(dgvPortUsageAssociation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
            {
                isPortAssociationChanged = true;
                btnTestConnection.Enabled = true;
                isConnectionTested = false;
            }
        }

        private void dgvPortUsageAssociation_MouseLeave(object sender, EventArgs e)
        {
            dgvPortUsageAssociation.EndEdit(DataGridViewDataErrorContexts.Commit);
            if (!isPortAssociationChanged &&
                PreviousPortAssociationColIndex >= 0 &&
                PreviousPortAssociationRowIndex >= 0 &&
                PreviousPortAssociationValue == !Convert.ToBoolean(dgvPortUsageAssociation.Rows[PreviousPortAssociationRowIndex].Cells[PreviousPortAssociationColIndex].Value))
            {
                isPortAssociationChanged = true;
                btnTestConnection.Enabled = true;
                isConnectionTested = false;
            }
        }
        private IList<CABSerialPort> GetSavedSerialPorts()
        {
            string strGSMModemPorts = objSystemSettings.GetSettingValue(SystemSettings.GSM_COM_PORTS);
            string strCMRIPort = objSystemSettings.GetSettingValue(SystemSettings.CMRI_COM_PORT);
            List<CABSerialPort> lstSerialPorts = new List<CABSerialPort>();
            if (!string.IsNullOrEmpty(strGSMModemPorts) ||
                !string.IsNullOrEmpty(strCMRIPort))
            {
                List<string> lstAllPorts = new List<string>(strGSMModemPorts.CommaSplit());
                lstAllPorts.Add(strCMRIPort);
                lstAllPorts.Sort();
                for (int i = 0; i < lstAllPorts.Count; i++)
                {
                    CABSerialPort objCABSerialPort = new CABSerialPort();
                    objCABSerialPort.PortName = lstAllPorts[i];
                    objCABSerialPort.IsResponding = true;
                    lstSerialPorts.Add(objCABSerialPort);
                }
            }
            return lstSerialPorts;
        }
        /// <summary>
        /// Yatin 19-Dec-2011
        /// </summary>
        public void AutoConfigureModem(RichTextBox pRTB)
        {
            string currentCommand = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
                GlobalObjects.objSerialComm.SetSerialPortSettings((string.IsNullOrEmpty(ModemConfigCOMPort) ? SerialPortSettings.Default.SerialPort : ModemConfigCOMPort), (string.IsNullOrEmpty(InitialModemBaudRate) ? "115200" : InitialModemBaudRate), "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                GlobalObjects.objSerialComm.CommandTimeout = 6000;
                GlobalObjects.objSerialComm.bCommType = 1;

                List<KeyValuePair<string, bool>> lstCommands = new List<KeyValuePair<string, bool>>();
                lstCommands.Add(new KeyValuePair<string, bool>("ATE0;&W", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+WOPEN=0", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+WOPEN=3", true));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+WOPEN=4", true));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+WIND=0", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+IFC=0,0", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT&C0", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT&D0", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT&S1", false));
                lstCommands.Add(new KeyValuePair<string, bool>("ATS0=2", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+ICF=3,4", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT&W", false));
                lstCommands.Add(new KeyValuePair<string, bool>("AT+IPR=9600;&W", false));


                string Result = string.Empty;
                Font fontUnderline = new Font(pRTB.Font, FontStyle.Underline);
                Font fontRegular = new Font(pRTB.Font, FontStyle.Regular);
                Font fontBold = new Font(pRTB.Font, FontStyle.Bold);
                pRTB.SelectionFont = fontUnderline;
                pRTB.AppendText("Trying to connect at " + GlobalObjects.objSerialComm.BaudRate + " bps on " + GlobalObjects.objSerialComm.PortName + "\n");
                pRTB.Refresh();
                foreach (KeyValuePair<string, bool> kvp in lstCommands)
                {
                    pRTB.SelectionFont = fontRegular;
                    pRTB.AppendText("\nCommand: ");
                    pRTB.SelectionFont = fontBold;
                    pRTB.AppendText(kvp.Key);
                    pRTB.Refresh();
                    currentCommand = kvp.Key;
                    if (CheckRunCommandStatus(kvp.Key, kvp.Value, out Result))
                    {
                        currentCommand = string.Empty;
                        pRTB.SelectionFont = fontRegular;
                        pRTB.AppendText("\nResult: ");
                        pRTB.SelectionFont = fontBold;
                        pRTB.AppendText(Result);
                        pRTB.Refresh();
                        GlobalObjects.objSerialComm.bCommType = 2;
                        if (kvp.Key == "AT")
                        {
                            GlobalObjects.objSerialComm.InterchatracterDelay = 35000;
                            GlobalObjects.objSerialComm.CommandTimeout = 40000;
                        }
                    }
                    else if (Result == "Modem Time Out.")
                    {
                        pRTB.SelectionFont = fontRegular;
                        pRTB.AppendText("\nResult: ");
                        pRTB.SelectionFont = fontBold;
                        pRTB.AppendText(Result);
                        pRTB.Refresh();
                        if (kvp.Key == "AT")
                        {
                            GlobalObjects.objSerialComm.ClosePort();
                            DialogResult dlgResult = MessageBox.Show("Unable to connect to Modem on " + GlobalObjects.objSerialComm.BaudRate + " bps.\n\nDo you want to try again with a different baud rate?", "Unable to Connect", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (objModemAutoConfigure != null)
                            {
                                objModemAutoConfigure.Dispose();
                            }
                            if (dlgResult == DialogResult.Yes)
                            {
                                btnAutoConfigModem_Click(null, null);
                            }
                            return;
                        }
                    }
                    else
                    {
                        pRTB.Text += "\nResult: " + Result;
                        this.Cursor = Cursors.Default;
                        MessageBox.Show(Result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Result = string.Empty;

                }
                GlobalObjects.objSerialComm.ClosePort();
                GlobalObjects.objSerialComm.SetSerialPortSettings(GlobalObjects.objSerialComm.PortName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                pRTB.SelectionFont = fontUnderline;
                pRTB.AppendText("\nTrying to connect at " + GlobalObjects.objSerialComm.BaudRate + " bps to check if modem has been configured.\n");
                pRTB.SelectionFont = fontRegular;
                pRTB.AppendText("\nCommand: ");
                pRTB.SelectionFont = fontBold;
                pRTB.AppendText("AT");
                pRTB.Refresh();
                if (CheckRunCommandStatus("AT", false, out Result))
                {
                    pRTB.SelectionFont = fontRegular;
                    pRTB.AppendText("\nResult: ");
                    pRTB.SelectionFont = fontBold;
                    pRTB.AppendText(Result);
                    pRTB.Refresh();
                    if (objModemAutoConfigure != null)
                    {
                        objModemAutoConfigure.Dispose();
                    }
                    MessageBox.Show("Modem Configured Successfully.");
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                if (currentCommand == "AT")
                {
                    DialogResult dlgResult = MessageBox.Show("Unable to connect to Modem on " + GlobalObjects.objSerialComm.BaudRate + " bps.\n\nDo you want to try again with a different baud rate?", "Unable to Connect", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (objModemAutoConfigure != null)
                    {
                        objModemAutoConfigure.Dispose();
                    }
                    if (dlgResult == DialogResult.Yes)
                    {
                        btnAutoConfigModem_Click(null, null);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                GlobalObjects.objSerialComm.CommandTimeout = SerialPortSettings.Default.CommandTimeOut;
                GlobalObjects.objSerialComm.InterchatracterDelay = SerialPortSettings.Default.IntercharacterDelay;
                GlobalObjects.objSerialComm.bCommType = 0;
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.ClosePort();
            }

        }

        /// <summary>
        /// Yatin 19-Dec-2011
        /// </summary>
        /// <param name="pCommand">Contains the text command to be sent to the port</param>
        /// <param name="pIsErrorStatusAllowed">If this is true, then ERROR response from the port will return true</param>
        /// <returns></returns>
        private bool CheckRunCommandStatus(string pCommand, bool pIsErrorStatusAllowed, out string pResponse)
        {
            String Result = fSendModemCommand(pCommand);
            Application.DoEvents();
            if (Result == "\r\nOK\r\n")
            {
                pResponse = Result;
                return true;

            }
            else if (pIsErrorStatusAllowed && Result == "\r\nERROR\r\n")
            {
                pResponse = Result;
                return true;

            }
            pResponse = Result;
            return false;
        }

        private void btnAutoConfigModem_Click(object sender, EventArgs e)
        {
            frmBaudRateSelector objBaudRateSelector = new frmBaudRateSelector();
            objBaudRateSelector.ShowDialog();
            if (objBaudRateSelector.DlgResult == DialogResult.OK)
            {
                InitialModemBaudRate = objBaudRateSelector.StrInitialBaudRate;
                ModemConfigCOMPort = objBaudRateSelector.IsMultipleCOMPorts ? objBaudRateSelector.SelectedCOMPort : SerialPortSettings.Default.SerialPort;
                objModemAutoConfigure = new frmModemAutoConfigure();
                objModemAutoConfigure.ConfigNow += new frmModemAutoConfigure.OnConfigure(AutoConfigureModem);
                objModemAutoConfigure.ShowDialog(this);
                InitialModemBaudRate = string.Empty;
                ModemConfigCOMPort = string.Empty;
            }
        }


        private void chkFastDownload_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkFastDownload.Checked)
            //    txtMeterID.Text = "";

            // Solved bug 72305. Assigned checked property for billing checkbox.
            //grpBoxFD.Visible = chkFastDownload.Checked;
            //grpBoxLS.Enabled = grpBoxBillingHistory.Enabled = grpBoxEventLog.Enabled = grpBoxDLMSRead.Enabled = btnCancel.Enabled =
            //        chkTamperFD.Checked = chkLoadSurveyFD.Checked = chkBillingFD.Checked = !chkFastDownload.Checked;

        }

        private void txtMeterID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.Handled = true;
        }

        private void tabPageCompartment4_Click(object sender, EventArgs e)
        {

        }

        #region TOU


        #region Variables
        string expMessage = string.Empty;
        private byte attribute;
        private object sender;
        private DataGridView currentClickedGrid;
        DataGridViewCellValidatingEventArgs eventArgs;
        private DateTime futureActivationDate;
        TOUModel touModel = new TOUModel();
        bool isConnect = false;
        #endregion

        #region Public properties
        public byte[] BlockBuffer
        {
            get
            {
                return GlobalObjects.objCOSEMLIB.BlockBuffer;
            }
        }
        public byte Attribute
        {
            get
            {
                return attribute;
            }
            set
            {
                attribute = value;
            }
        }
        public DataGridView[] TOUGridNames
        {
            get
            {
                return touGridNames;
            }
        }

        public DataGridView CurrentClickedGrid
        {
            get
            {
                return currentClickedGrid;
            }
            set
            {
                currentClickedGrid = value;
            }
        }
        public DataGridView GridActivationDate
        {
            get
            {
                return gridActivationDate;
            }

        }
        public DataGridView GridDayTables
        {
            get
            {
                return gridDayTables;
            }
        }
        public Label LblMonth
        {
            get
            {
                return lblMonths;
            }
            set
            {
                lblMonths = value;
            }
        }

        public DateTime FutureActivationDate
        {
            get
            {
                return dTPFutureActivationDate.Value;

            }
            set
            {
                dTPFutureActivationDate.Value = value;
            }
        }
        public object DataGridViewSenderObject
        {
            get
            {
                return sender;
            }
            set
            {
                sender = value;
            }
        }

        public DataGridViewCellValidatingEventArgs EventArgs
        {
            get
            {
                return eventArgs;
            }
            set
            {
                eventArgs = value;
            }
        }

        #endregion

        #region Private Methods
        private void ReadCurrentTOU()
        {
            int result;
            SetButtonsStatus(false);
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                touPresenter.ClearGrids();
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                SerialPortSettings.Default.ServerSAP = 0x01;
               //Cabcon config commands if GSM is enabled         
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {
                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        // VBM - fix defect - 150001
                        // MessageBox.Show(CoreUtility.GetMessageFromResourceFile("DLMSCONNECTIONNOTAVAILABLE"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        isConnect = true;
                    }
                    int modelNumber = 0;
                    if (UtilityDetails.ShowMeterModelNo)
                    {
                        modelNumber = GetModelNumber();
                    }
                    StartTimer();
                    result = ReadTOU((byte)ActivityCalender.ActiveSeasonProfile);
                    if (result == 0)
                    {
                        touPresenter.FillSeasonProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer);

                        result = ReadTOU((byte)ActivityCalender.ActiveWeekProfile);
                        if (result == 0)
                        {
                            touPresenter.FillWeekProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer);
                            result = ReadTOU((byte)ActivityCalender.ActiveDayProfile);
                            if (result == 0)
                            {
                                touPresenter.FillDayProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer, modelNumber);
                            }
                        }
                    }



                    if (result != 0)
                    {
                        MessageBox.Show(CoreUtility.GetMessageFromEnum(result).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }

            }
            catch (Exception ex)
            {
                if (CoreUtility.ExpMessage != null || !string.IsNullOrEmpty(CoreUtility.ExpMessage))
                {
                    MessageBox.Show(CoreUtility.GetMessageFromResourceFile("InvalidResponse"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            finally
            {
                StopTimer();
                if (isConnect)
                    DLMSMain.fDLMSDisconnect();
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                SetButtonsStatus(true);
            }
        }

        /// <summary>
        /// This method is used for reading future TOU from the presenter.
        /// </summary>
        private void ReadFutureTOU()
        {
            int result;
            SetButtonsStatus(false);
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                touPresenter.ClearGrids();
                Application.DoEvents();
                SerialPortSettings.Default.ServerSAP = 0x01;
               //Cabcon config commands if GSM is enabled         
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {
                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        //VBM - Resolve bug - 150001
                        //MessageBox.Show(CoreUtility.GetMessageFromResourceFile("DLMSCONNECTIONNOTAVAILABLE"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        isConnect = true;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    int modelNumber = 0;
                    if (UtilityDetails.ShowMeterModelNo)
                    {
                        modelNumber = GetModelNumber();
                    }
                    StartTimer();
                    result = ReadTOU((byte)ActivityCalender.PassiveSeasonProfile);
                    if (result == 0)
                    {
                        touPresenter.FillSeasonProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer);

                        result = ReadTOU((byte)ActivityCalender.PassiveWeekProfile);
                        if (result == 0)
                        {
                            touPresenter.FillWeekProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer);

                            result = ReadTOU((byte)ActivityCalender.PassiveDayProfile);
                            if (result == 0)
                            {
                                touPresenter.FillDayProfileParameters(GlobalObjects.objCOSEMLIB.BlockBuffer, modelNumber);
                                result = ReadTOU((byte)ActivityCalender.ActivationDate);
                                if (result == 0)
                                {
                                    touPresenter.FillTOUActivationDate(GlobalObjects.objCOSEMLIB.BlockBuffer);
                                }
                            }
                        }
                    }
                    if (result != 0)
                    {
                        MessageBox.Show(CoreUtility.GetMessageFromEnum(result).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            catch (Exception ex)
            {
                if (CoreUtility.ExpMessage != null || !string.IsNullOrEmpty(CoreUtility.ExpMessage))
                {
                    MessageBox.Show(CoreUtility.ExpMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            finally
            {
                StopTimer();
                if (isConnect)
                    DLMSMain.fDLMSDisconnect();
                SetButtonsStatus(true);
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// This method is used for setting TOU buttons enabled and disabled.
        /// </summary>
        /// <param name="status">Pass the status true or false for enabling and disabling buttons</param>
        private void SetButtonsStatus(bool status)
        {
            btnReadCurrentTOU.Enabled = status;
            btnTOUWrite.Enabled = status;
            btnReadFutureTOU.Enabled = status;
            btnFillTOUConfiguration.Enabled = status;
            btnResetAll.Enabled = status;
            btnCancel.Enabled = status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteTOUData()
        {

            DateTime tdate, ndate;
            tdate = DateTime.Now.Date.AddDays(+1);
            tdate = Convert.ToDateTime("29/02/2012");
            ndate = dTPFutureActivationDate.Value;
            if (ndate.Date == tdate.Date && ndate.Month == tdate.Month)
            {
                MessageBox.Show(CoreUtility.GetMessageFromResourceFile("FUTURETOUACTIVATEDATENOTBE29FEB"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            SetButtonsStatus(false);
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
                if (touPresenter.ValidateTOUGrids())
                {
                   //Cabcon config commands if GSM is enabled         
                    if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                    {
                        comType = CommunicationTypeDetail.GetCommunicationType();
                        if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                        {
                            isChannelInitialized = CheckChannelInitialization(comType);
                        }
                    }
                    //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                    if (isChannelInitialized)
                    {
                        Application.DoEvents();
                        StartTimer();
                        touPresenter.ActivationDate = dTPFutureActivationDate.Value;
                        WriteTOUProfiles();
                        this.Cursor = Cursors.WaitCursor;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                CoreUtility.ExpMessage = string.Empty;
            }
            finally
            {
                StopTimer();
                if (CoreUtility.ExpMessage != null && !string.IsNullOrEmpty(CoreUtility.ExpMessage))
                {
                    if (CoreUtility.ExpMessage.Equals("CosemConnectionFailed", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(CoreUtility.ExpMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        MessageBox.Show(CoreUtility.ExpMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                SetButtonMode(SerialPortSettings.Default.ClientSAP);
                SetButtonsStatus(true);
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }
                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;

            }
        }



        /// <summary>
        /// This method is used for writing the TOU profiles into the meter.
        /// </summary>
        public void WriteTOUProfiles()
        {
            if (DLMSMain.fDLMSConnect())
            {
                int response = 7;
                try
                {
                    int modelNumber = 0;
                    if (UtilityDetails.ShowMeterModelNo)
                    {
                        modelNumber = GetModelNumber();
                        //Cortex meter will follow ruby one season TOU logic so changing metermodel explicitly to ruby 
                        if (modelNumber == NamePlateConstants.LTCTCortexValue)
                        {
                            modelNumber = NamePlateConstants.RubyE250Value;
                        }

                    }
                    byte[] touBuffer;

                    touBuffer = touPresenter.GetTOUBuffer(ActivityCalender.PassiveSeasonProfile, modelNumber);
                    response = WriteTOU(touBuffer, touBuffer.Length, (byte)ActivityCalender.PassiveSeasonProfile);
                    if (response == (int)ProgrammingCode.Success)
                    {
                        touBuffer = touPresenter.GetTOUBuffer(ActivityCalender.PassiveWeekProfile, modelNumber);
                        response = WriteTOU(touBuffer, touBuffer.Length, (byte)ActivityCalender.PassiveWeekProfile);
                        if (response == (int)ProgrammingCode.Success)
                        {
                            touBuffer = touPresenter.GetTOUBuffer(ActivityCalender.PassiveDayProfile, modelNumber);
                            response = WriteTOUBlock(touBuffer, touBuffer.Length, (byte)ActivityCalender.PassiveDayProfile);
                            if (response == (int)ProgrammingCode.Success)
                            {
                                touBuffer = touPresenter.GetTOUBuffer(ActivityCalender.ActivationDate, modelNumber);
                                WriteTOU(touBuffer, touBuffer.Length, (byte)ActivityCalender.ActivationDate);
                            }
                        }
                    }


                }
                catch
                {

                }
                finally
                {
                    DLMSMain.fDLMSDisconnect();
                    if (response == (int)ProgrammingCode.Success)
                    {
                        CoreUtility.ExpMessage = "Parameters written successfully.";
                    }
                    else if (response == (int)ProgrammingCode.AccessDenied)
                    {
                        CoreUtility.ExpMessage = "Access Denied.";
                    }
                    else if (response == (int)ProgrammingCode.CosemConnectionFailed)
                    {
                        CoreUtility.ExpMessage = "Cosem Connection Failed.";

                    }
                }
            }
        }
        private int GetModelNumber()
        {
            int meterModelNo = 0;
            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.BESCOM.ToString())
            {
                int writeResponse = touModel.WritePTRatio();
                if (writeResponse == 2)
                {
                    meterModelNo = NamePlateConstants.RubyE250Value;
                }
                else if (writeResponse == 0x00)
                {
                    meterModelNo = NamePlateConstants.PumaLTE650Value;
                }
            }
            else if (UtilityDetails.ReadSignatureData)
            {
                int result = ReadSignature();
                if (result == 0x00)
                {
                    string meterModel = Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[35]).ToString()
                     + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[36]).ToString();
                    if (meterModel == "WC")
                    {
                        meterModelNo = NamePlateConstants.RubyE250Value;
                    }
                    else if (meterModel == "LT")
                    {
                        meterModelNo = NamePlateConstants.PumaLTE650Value;
                    }
                    else if (meterModel == "LC")
                    {
                        meterModelNo = NamePlateConstants.LTCTCortexValue;
                    }
                }
            }
            else
            {
                int writeResponse = touModel.ReadMeterModelNumber();

                if (writeResponse == 0x00)
                {
                    meterModelNo = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                }
            }


            return meterModelNo;
        }

        #endregion

        #region Events
        private void btnFillTOUConfiguration_Click(object sender, EventArgs e)
        {
            touPresenter.AutoFillTOU();
        }
        private void btnReadFutureTOU_Click(object sender, EventArgs e)
        {
            ReadFutureTOU();
        }
        private void btnReadCurrentTOU_Click(object sender, EventArgs e)
        {
            ReadCurrentTOU();
        }

        private void btnTOUWrite_Click(object sender, EventArgs e)
        {
            WriteTOUData();
        }

        private void gridTOUDay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.DataGridViewSenderObject = sender;
                touPresenter.DayGridCellClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridDayTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.DataGridViewSenderObject = sender;
                touPresenter.WeekGridCellClick();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridDayTables_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                this.EventArgs = e;
                this.DataGridViewSenderObject = sender;
                touPresenter.ValidateWeekProfileCell();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridActivationDate_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                this.EventArgs = e;
                this.DataGridViewSenderObject = sender;
                touPresenter.ValidateSeasonProfileCell();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridActivationDate_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.DataGridViewSenderObject = sender;
                touPresenter.SeasonGridCellClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridTOUDay_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            this.EventArgs = e;
            this.DataGridViewSenderObject = sender;
            touPresenter.ValidateDayProfileCell();
        }

        private void gridActivationDate_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            gridActivationDate.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "00";
        }

        private void gridDayTables_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            gridDayTables.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "00";
        }

        private void gridTOUDay1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "00";
        }
        #endregion

        #endregion


        #region CMRI
        CMRIPresenter cmriPresenter;

        #region Public Methods

        #endregion

        #region Public Properties
        public bool IsGeneral
        {
            get
            {
                return chkCMRINameplate.Checked;
            }

        }
        public bool IsInstantaneous
        {
            get
            {
                return chkCMRIInstant.Checked;
            }
        }
        public bool IsBilling
        {
            get
            {
                return chkCMRIBilling.Checked;
            }
        }
        public bool IsLoadSurvey
        {
            get
            {
                return chkCMRILoadSurvey.Checked;
            }
        }
        public bool IsSelectAll
        {
            get
            {
                return chkCMRISelectAll.Checked;
            }
        }
        public bool IsMidNightEnergies
        {
            get
            {
                return chkCMRIMidnightData.Checked;
            }
        }
        public bool IsEventLog
        {
            get
            {
                return chkCMRITamper.Checked;
            }
        }
        public CheckedListBox ListCMRI
        {
            get
            {
                return lstCMRIfile;
            }
            set
            {
                lstCMRIfile = value as CheckedListBox;
            }
        }
        public bool IsReadlast
        {
            get
            {
                return rdBtnReadComplete.Checked;
            }
        }
        public bool IsReadBetweenBilling
        {
            get
            {
                return rdBtnReadBetween.Checked;
            }
        }
        public string BillingToDate
        {
            get
            {
                return cmbBoxTo.Text;
            }
        }
        public string BillingFromDate
        {
            get
            {
                return cmbBoxFrom.Text;
            }
        }
        public string BillingLastFromDate
        {
            get
            {
                return cmbBoxLastFrom.Text;
            }
        }
        public bool IsReadBetweenLoadSurvey
        {
            get
            {
                return rdBtnReadBetweenLS.Checked;
            }
        }
        public DateTime LoadSurveyToDate
        {
            get
            {
                return dtPickerTo.Value;
            }
        }
        public DateTime LoadSurveyFromDate
        {
            get
            {
                return dtPickerFrom.Value;
            }
        }
        public bool IsReadBetweenEventLog
        {
            get
            {
                return rdBtnReadBetweenEvent.Checked;
            }
        }
        public bool IsReadLastEventLog
        {
            get
            {
                return rdBtnReadLastEvent.Checked;
            }
        }

        public string EventToDate
        {
            get
            {
                return cmbBoxToEvent.Text;
            }
        }
        public string EventFromDate
        {
            get
            {
                return cmbBoxFromEvent.Text;
            }
        }
        public string EventLastFromDate
        {
            get
            {
                return cmbBoxLastFromEvent.Text;
            }
        }
        public bool BtnReadAllCMRIEnabled
        {
            set
            {
                btnReadAllCMRI.Enabled = value;
            }
        }
        public bool BtnCMRICancelEnabled
        {
            set
            {
                btnCMRICancel.Enabled = value;
            }
        }
        public bool BtnLoadListEnabled
        {
            set
            {
                btnLoadList.Enabled = value;
            }
        }
        public bool BtnReadAllEnabled
        {
            set
            {
                btnReadAll.Enabled = value;
            }
        }
        public bool CheckCMRIBillingEnabled
        {
            set
            {
                chkCMRIBilling.Enabled = value;
            }
        }
        public bool CheckCMRILoadSurveyEnabled
        {
            set
            {
                chkCMRILoadSurvey.Enabled = value;
            }
        }
        public bool CheckCMRITamperEnabled
        {
            set
            {
                chkCMRITamper.Enabled = value;
            }
        }
        public bool CheckCMRINameplateEnabled
        {
            set
            {
                chkCMRINameplate.Enabled = value;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is reading different profils from the meter.
        /// </summary>
        private void GetCMRIProfiles()
        {
            btnCMRICancel.Enabled = false;
            bool result = false;
            if (!IsInstantaneous && !IsBilling && !IsLoadSurvey && !IsEventLog && !IsGeneral)
            {
                MessageBox.Show(CoreUtility.GetMessageFromResourceFile("SelectOption"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lstCMRIfile.CheckedItems.Count == 0)
            {
                MessageBox.Show(CoreUtility.GetMessageFromResourceFile("SelectFile"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int ldn = 2; ldn < lstCMRIfile.Items.Count + 2; ldn++)
            {
                if (lstCMRIfile.GetItemCheckState(ldn - 2) == CheckState.Checked)
                {
                    int index = 0;
                    meterLoadList.TryGetValue(lstCMRIfile.Items[ldn - 2].ToString(), out index);
                    result = fReadCMRFile(index + 2, ldn);
                    if (result == true)
                    {
                        lstCMRIfile.SetItemChecked(ldn - 2, false);
                    }
                    else
                    {
                        lstCMRIfile.Items.Clear();
                        btnLoadList.Enabled = true;
                        btnReadAllCMRI.Enabled = false;
                        return;
                    }
                }
                chkCMRIInstant.Enabled = true;
                chkCMRIBilling.Enabled = true;
                chkCMRILoadSurvey.Enabled = true;
                chkCMRITamper.Enabled = true;
                if (UtilityDetails.ShowMidnight)
                {
                    chkCMRIMidnightData.Enabled = true;
                }
                if (UtilityDetails.ShowPhasorInCMRINormalMode)
                {
                    chkCMRIPhasor.Enabled = true;
                }
            }
            if (result == true)
            {
                MessageBox.Show(CoreUtility.GetMessageFromResourceFile("CMRISucessMessage") + " " + AppDomain.CurrentDomain.BaseDirectory + ".");
            }
            else
            {
                MessageBox.Show(CoreUtility.GetMessageFromResourceFile("DataCorruptInCMRI"), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DisplaySAPList(byte[] Blockdata)
        {
            string data = string.Empty;
            int capture_object_definition;
            int i, j = 0;
            int nLength = 0;
            int nByteIndex = 0;

            nByteIndex++;       // Array 01
            capture_object_definition = Blockdata[nByteIndex];

            for (i = 0; i < capture_object_definition; i++)
            {
                nByteIndex++;
                nByteIndex++;

                nByteIndex++;
                nByteIndex++;
                nByteIndex++;

                nByteIndex++;
                nByteIndex++;
                nLength = Blockdata[nByteIndex++];
                // length 06
                data = "";
                for (j = 0; j < nLength; j++)
                {
                    data = data + Convert.ToChar(Blockdata[j + nByteIndex]);
                }
                nByteIndex = nByteIndex + (j - 1);
                //09 0C 07 DA 0B 1D FF 0B 30 13 FF 80 00 00

                nByteIndex++;
                nByteIndex++;
                nByteIndex++;

                int year = 0;// receivedData[21];
                year = (year | (int)Blockdata[nByteIndex++]) << 8;
                year = (year | (int)Blockdata[nByteIndex++]);
                int month = Blockdata[nByteIndex++];
                int date = Blockdata[nByteIndex++];
                int week = Blockdata[nByteIndex++];
                int hour = Blockdata[nByteIndex++];
                int minute = Blockdata[nByteIndex++];
                int second = Blockdata[nByteIndex++];
                if (Blockdata[nByteIndex] != 253)
                {
                    data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.SHYAMINDUS.ToString())
                    {
                        data = "1" + data;
                    }
                    lstCMRIfile.Items.Add(data, true);
                    meterLoadList.Add(data, i);
                    btnReadAllCMRI.Enabled = true;
                }
                else if (Blockdata[nByteIndex] == 253)
                {
                    data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + week.ToString("d2") + ":" + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2") + ".FD";
                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.SHYAMINDUS.ToString())
                    {
                        data = "1" + data;
                    }
                    lstFast.Items.Add(data, true);
                    meterLoadList.Add(data, i);
                    btnFDRead.Enabled = true;
                }
                nByteIndex++;
                nByteIndex++;
                nByteIndex++;
                //nByteIndex++;





            }
        }
        /// <summary>
        /// This method is used for loading available meter  list from the CMRI.
        /// </summary>
        private void LoadMeterListFromCMRI()
        {
            bool isConnected = false;
            SerialPortSettings.Default.ServerSAP = 0x01;
            lstCMRIfile.Items.Clear();
            StartTimer();
            //CoreUtility.GetIncrementedTimer();
            fIncrementTimer();
            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            try
            {
                if (dlmsConnection.Connect() != true)
                {
                    MessageBox.Show(CoreUtility.GetMessageFromResourceFile("HDLCConnectionFailed"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                isConnected = true;
                // CoreUtility.GetIncrementedTimer();
                fIncrementTimer();
                Application.DoEvents();
                if (cmriPresenter.ReadSAPlist() == true)
                {
                    meterLoadList = new Dictionary<string, int>();
                    meterLoadList.Clear();
                    lstCMRIfile.Items.Clear();
                    lstFast.Items.Clear();
                    // CoreUtility.GetIncrementedTimer();
                    DisplaySAPList(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    grpPartialRead.Enabled = true;
                    btnLoadList.Enabled = false;
                    btnReadAllCMRI.Enabled = true;
                }
                else
                {
                    MessageBox.Show(CoreUtility.GetMessageFromResourceFile("CosemConnectionFailed"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // CoreUtility.StopTimer();
                StopTimer();
                this.Cursor = Cursors.Default;
                if (isConnected)
                {
                    dlmsConnection.Disconnect();
                }
            }
        }

        #endregion

        #region Events
        private void btnReadAllCMRI_Click(object sender, EventArgs e)
        {
            GetCMRIProfiles();
        }
        private void btnCMRICancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSelectAllMeters_CheckedChanged(object sender, EventArgs e)
        {
            for (int counter = 0; counter < lstCMRIfile.Items.Count; counter++)
                lstCMRIfile.SetItemChecked(counter, chkSelectAllMeters.Checked);
        }

        private void btnLoadList_Click(object sender, EventArgs e)
        {
            LoadMeterListFromCMRI();
        }
        #endregion
        #endregion


        private void DLMSMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            commAborted = true;
            //fDLMSDisconnect();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void btnLoadMeterFD_Click(object sender, EventArgs e)
        {
            lstFast.Items.Clear();
            lstCMRIfile.Items.Clear();
            try
            {
                StartTimer();

                SerialPortSettings.Default.ServerSAP = 0x01;
                fIncrementTimer();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    StopTimer();
                    return;
                }
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                fIncrementTimer();
                Application.DoEvents();

                if (cmriPresenter.ReadSAPlist() == true)
                {
                    meterLoadList = new Dictionary<string, int>();
                    fIncrementTimer();
                    DisplaySAPListFastDownload(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    //DisplaySAPList(GlobalObjects.objCOSEMLIB.BlockBuffer);
                    btnLoadMeterFD.Enabled = false;
                }
                else
                {
                    StopTimer();
                    MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopTimer();
                DLMSMain.fDLMSDisconnect();
            }
        }

        private void btnFDRead_Click(object sender, EventArgs e)
        {
            bool res = false;
            string strFileName = string.Empty;
            string message = string.Empty;
            if (lstFast.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a file to read", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                toolstripStatus.Text = "Fast Download in progress...";
                // Application.DoEvents();
                for (int ldn = 2; ldn < lstFast.Items.Count + 2; ldn++)
                {
                    if (lstFast.GetItemCheckState(ldn - 2) == System.Windows.Forms.CheckState.Checked)
                    {
                        int index = 0;
                        meterLoadList.TryGetValue(lstFast.Items[ldn - 2].ToString(), out index);
                        res = fReadCMRIFileForFastdownload(index + 2, ldn, out message);
                        //res = true;
                        if (res == true)
                        {
                            lstFast.SetItemChecked(ldn - 2, false);
                        }
                        else
                        {

                            lstFast.Items.Clear();
                            btnLoadMeterFD.Enabled = true;
                            btnFDRead.Enabled = false;
                            return;
                        }
                    }
                }
                if (res == true)
                {
                    MessageBox.Show("Read Successfully completed for all meter IDs from CMRI and Saved in " + AppDomain.CurrentDomain.BaseDirectory + "FDLFILES");
                    Cursor.Current = Cursors.Default;
                }
                else
                    MessageBox.Show("Error occured while communicating with CMRI.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);

                toolstripStatus.Text = "Fast Download completed.";
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while communicating with CMRI.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                toolstripStatus.Text = "Error occured.";
                Application.DoEvents();
            }
            finally
            {

                GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            }
        }

        private void btnCancelFD_Click(object sender, EventArgs e)
        {
            this.Close();
            this.StatusMessage = string.Empty;
        }

        private void chkFDSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int counter = 0; counter < lstFast.Items.Count; counter++)
                lstFast.SetItemChecked(counter, chkFDSelectAll.Checked);
        }







        private void chkCMRISelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCMRISelectAll.Checked == true)
            {
                chkCMRIInstant.Checked = true;
                chkCMRIBilling.Checked = true;
                chkCMRITamper.Checked = true;
                chkCMRILoadSurvey.Checked = true;
                chkCMRIMidnightData.Checked = true;
                chkCMRIPhasor.Checked = true;
                //chkCMRINameplate.Checked = true;
            }
            else
            {
                chkCMRIInstant.Checked = false;
                chkCMRIBilling.Checked = false;
                chkCMRITamper.Checked = false;
                chkCMRILoadSurvey.Checked = false;
                chkCMRIMidnightData.Checked = false;
                chkCMRIPhasor.Checked = false;
                //chkCMRINameplate.Checked = false;
            }
            //chkCMRIInstant.Enabled = true;
            //chkCMRIBilling.Enabled = true;
            //chkCMRITamper.Enabled = true;
            //chkCMRILoadSurvey.Enabled = true;
            //chkCMRINameplate.Enabled = true;
        }

        //The following event was added to resolve bug 74559(Enhancement); 19th April 2012
        private void btnResetAll_Click(object sender, EventArgs e)
        {
            try
            {
                touPresenter.ResetAllTOU();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void chkInsta_CheckedChanged(object sender, EventArgs e)
        {
            chkOther.CheckedChanged -= chkOther_CheckedChanged;
            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? this.chkPhasor.Checked : true;
            //VBM - only check midnight check box when show midnight is true.
            if (chkInsta.Checked && chkLoadSurvey.Checked && chkBilling.Checked && chkTamper.Checked
                && chkPhasor && chkMidnnight)
                chkOther.Checked = true;
            else
                chkOther.Checked = false;

            chkOther.CheckedChanged += chkOther_CheckedChanged;
        }

        //added for MVVNL
        private void chkMidnightData_CheckedChanged(object sender, EventArgs e)
        {
            chkInsta_CheckedChanged(sender, e);
        }

        private void btnPCBARead_Click(object sender, EventArgs e)
        {
            PCBAReadStarting();
            string meterId = string.Empty;
            if (ReadPCBAStarted(out meterId))
            {
                if (!string.IsNullOrEmpty(meterId))
                {
                    ReadPCBACompleted(meterId);
                }
            }
            else
            {
                btnPCBARead.Enabled = true;
                btnPCBAExport.Enabled = true;
                this.Cursor = Cursors.Default;
            }

        }
        private DataTable FillPCBA(byte[] receivedBuffer)
        {
            DataTable pcbaTable = new DataTable();
            DataRow[] pcbaRow = new DataRow[2]; ;
            pcbaTable.Columns.Add(new DataColumn(PARAMETERS, typeof(System.String)));
            pcbaTable.Columns.Add(new DataColumn(STATUS, typeof(System.String)));
            pcbaRow[0] = pcbaTable.NewRow();
            pcbaRow[1] = pcbaTable.NewRow();
            if (receivedBuffer[NVMFAILUREINDEX] == 0x00)
            {
                pcbaRow[0][PARAMETERS] = EnumUtil.stringValueOf(PCBATypes.NVMFailure);
                pcbaRow[0][STATUS] = EnumUtil.stringValueOf(PCBAStatus.Fail);
                pcbaTable.Rows.Add(pcbaRow[0]);
            }
            else
            {

                pcbaRow[0][PARAMETERS] = EnumUtil.stringValueOf(PCBATypes.NVMFailure);
                pcbaRow[0][STATUS] = EnumUtil.stringValueOf(PCBAStatus.Ok);
                pcbaTable.Rows.Add(pcbaRow[0]);

            }
            if (receivedBuffer[RTCBADINDEX] == 0x00 || receivedBuffer[RTCSTOPINDEX] == 0x00 || receivedBuffer[RTCTIMESTOPINDEX] == 0x00)
            {

                pcbaRow[1][PARAMETERS] = EnumUtil.stringValueOf(PCBATypes.RTCFailure);
                pcbaRow[1][STATUS] = EnumUtil.stringValueOf(PCBAStatus.Fail);
                pcbaTable.Rows.Add(pcbaRow[1]);

            }
            else
            {

                pcbaRow[1][PARAMETERS] = EnumUtil.stringValueOf(PCBATypes.RTCFailure);
                pcbaRow[1][STATUS] = EnumUtil.stringValueOf(PCBAStatus.Ok);
                pcbaTable.Rows.Add(pcbaRow[1]);
            }
            return pcbaTable;



        }
        /// <summary>
        /// This method is used to display PCBA status on UI.
        /// </summary>
        /// <param name="receivedBuffer"></param>
        /// <param name="meterID"></param>
        private void DisplayPCBAStatus(byte[] receivedBuffer, string meterID)
        {

            lblPCBAMeterID.Visible = true;
            lblDisplayMeterId.Visible = true;
            lblPCBAMeterID.Text = meterID;
            DataSet pcbaDataSet = new DataSet();
            DataTable pcbaTable = new DataTable();
            pcbaTable = FillPCBA(receivedBuffer);
            pcbaDataSet.Tables.Add(pcbaTable);
            if (pcbaDataSet.FirstTableHasRows())
            {
                grdPCBA.DataSource = pcbaDataSet.Tables[0];
            }
            foreach (DataGridViewColumn column in grdPCBA.Columns)
            {
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

        }
        private void PCBAReadStarting()
        {
            btnPCBARead.Enabled = false;
            btnPCBAExport.Enabled = false;
            lblPCBAMeterID.Visible = false;
            lblDisplayMeterId.Visible = false;
            lblPCBAMeterID.Text = string.Empty;
            ClearGrid();

        }
        private void ClearGrid()
        {
            if (grdPCBA.Rows.Count > 0)
            {
                for (int rowCount = 0; rowCount < grdPCBA.RowCount; rowCount++)
                {
                    grdPCBA.Rows[rowCount].Cells[PARAMETERS].Value = null;
                    grdPCBA.Rows[rowCount].Cells[STATUS].Value = null;

                }
            }
        }
        /// <summary>
        /// This involves reading of meter serial number before reading pcba failures.
        /// </summary>
        /// <returns></returns>
        private bool ReadMeterNumberForPCBA(out string meterId)
        {

            int writeResponse = fReadMeterSerialNumber();
            SerialPortSettings.Default.ServerSAP = 0x01;
            meterId = string.Empty;
            int idLen, index;
            if (writeResponse == 0)
            {

                idLen = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                if (idLen < 7 || idLen > 16)
                {
                    MessageBox.Show("Meter data corrupt", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                    btnPCBAExport.Enabled = true;
                    btnPCBARead.Enabled = true;
                    this.Cursor = Cursors.Default;
                    return false;
                }

                string idLength = Convert.ToString(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                while (idLength.Length < 2)
                {
                    idLength = "0" + idLength;
                }
                index = Convert.ToInt16(GlobalObjects.objSerialComm.ReceiveBuffer[19]);
                for (int i = 20; i <= 20 + (index - 1); i++)
                {
                    meterId += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]).ToString();

                }

            }
            else
            {
                GlobalObjects.objSerialComm.ClosePort();
                MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                btnPCBAExport.Enabled = true;
                btnPCBARead.Enabled = true;
                this.Cursor = Cursors.Default;
                return false;
            }
            return true;
        }
        private bool TryReadMeterNumberForPCBA(out string meterId)
        {
            meterId = string.Empty;
            if (DLMSMain.fDLMSConnect() != true)
            {
                return false;
            }
            if (ReadMeterNumberForPCBA(out meterId))
            {
                return true;
            }
            return false;
        }
        private bool ReadPCBAStarted(out string meterId)
        {
            meterId = string.Empty;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            if (TryReadMeterNumberForPCBA(out meterId))
            {
                return true;
            }
            return false;

        }

        private void ReadPCBACompleted(string meterID)
        {
            int response;
            try
            {
                response = TryReadPCBAStatus();
                if (response == (int)ProgrammingCode.Success)
                {
                    DisplayPCBAStatus(GlobalObjects.objSerialComm.ReceiveBuffer, meterID);
                }
                else if (response == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (response == (int)ProgrammingCode.DataUnavailable)
                {
                    MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (response == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                btnPCBARead.Enabled = true;
                btnPCBAExport.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
        private void btnPCBAExport_Click(object sender, EventArgs e)
        {
            ExportPCBA();
        }
        /// <summary>
        /// This function is used to set the filename for export and to call exporttofile function.
        /// </summary>
        private void ExportPCBA()
        {
            SaveFileDialog saveFileDialogExport = new SaveFileDialog();
            try
            {

                if (grdPCBA.Rows.Count > 0)
                {
                    saveFileDialogExport.Filter = SAVEFILEEXTENSION;
                    saveFileDialogExport.RestoreDirectory = true;
                    if (saveFileDialogExport.ShowDialog() == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(saveFileDialogExport.FileName))
                        {
                            if (ExportToFile(saveFileDialogExport.FileName))
                            {
                                MessageBox.Show(SUCCESSEXPORT, BCSConstants.BCS);
                            }
                            else
                            {
                                File.Delete(saveFileDialogExport.FileName);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(NOTHINGTOEXPORT, BCSConstants.BCS);
                }

            }
            catch (Exception)
            {
                MessageBox.Show(EXPORTNOTSUCCESSFUL, BCSConstants.BCS);
            }

        }
        /// <summary>
        /// This function is used to write the pcba failures to a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ExportToFile(string fileName)
        {
            bool success = true;

            string dataToExport = string.Empty;
            string meterID = lblPCBAMeterID.Text;
            meterID = "MeterId:" + meterID;
            StreamWriter streamWriter = new StreamWriter(fileName);

            try
            {
                streamWriter.WriteLine(meterID);
                dataToExport += PARAMETERS + Symbols.COMMA + STATUS;
                streamWriter.WriteLine(dataToExport);

                for (int i = 0; i < grdPCBA.Rows.Count - 1; i++)
                {
                    dataToExport = string.Empty;
                    string value = Convert.ToString(grdPCBA.Rows[i].Cells[PARAMETERS].Value);
                    dataToExport += value + Symbols.COMMA;
                    value = Convert.ToString(grdPCBA.Rows[i].Cells[STATUS].Value);
                    dataToExport += value;
                    streamWriter.WriteLine(dataToExport);
                }
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;


        }

        private void tabPageCompartment1_Click(object sender, EventArgs e)
        {




        }



        private void chkNormalDownload_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxLS.Enabled = chkLoadSurvey.Checked;
            grpBoxEventLog.Enabled = chkTamper.Checked;
            grpBoxBillingHistory.Enabled = chkBilling.Checked;
            chkNameplate.Checked = true;
            chkNameplate.Enabled = false;
            /* VBM - Make Midnight data read configurable */
            if (UtilityDetails.ShowMidnight)
            {
                chkMidnightData.Visible = true;
            }
            //else if (CoreUtility.IsPUMA)
            //{
            //    chkMidnightData.Visible = true;

            //}
            else
            {
                chkMidnightData.Visible = false;
            }
            /* VBM - Make Midnight data read configurable */
            /* VBM - Read phasor using FD command in normal mode */
            if (UtilityDetails.ShowPhasorFastDownloadInNormalMode)
            {
                chkPhasor.Visible = true;
            }
            else
            {
                chkPhasor.Visible = false;
            }
            /* VBM - Read phasor using FD command in normal mode */
        }
        //added for MVVNL
        /// <summary>
        /// This method is used when Accuracy check is to be stopped.
        /// </summary>
        private void StopAccuracyCheck()
        {
            string strTemp = string.Empty;
            string completeData = string.Empty;
            bool flag = false;
            MeterAccuracyCheckEntity meterAccuracyCheckEntity = new MeterAccuracyCheckEntity();
            DLMS650FormatterMeterAccuracyCheck formatterMeterAccuracyCheck = new DLMS650FormatterMeterAccuracyCheck();
            try
            {
                if (ConnectToMeter())
                {
                    completeData += ReadParametersObjects(out flag) + Symbols.NEWLINE;
                    if (!flag)
                    {
                        completeData += ReadParameterData() + Symbols.NEWLINE;
                        if (!completeData.Contains(COMMessages.COSEMCONNECTIONFAILED))
                        {
                            completeData += ReadParametersScalarObjects() + Symbols.NEWLINE;
                            if (!completeData.Contains(COMMessages.COSEMCONNECTIONFAILED))
                                completeData += ReadParametersScalarUnits();
                            else
                                completeData = string.Empty;
                        }
                        else
                            completeData = string.Empty;
                        startDatetime = DateTime.Now;
                        if (!string.IsNullOrEmpty(completeData))
                        {
                            meterAccuracyCheckEntity = formatterMeterAccuracyCheck.GetDataForAccuracyCheck(completeData);
                        }
                        if (meterAccuracyCheckEntity.KVAh != null && meterAccuracyCheckEntity.KvarhLag != null && meterAccuracyCheckEntity.KvarhLead != null && meterAccuracyCheckEntity.KWh != null)
                        {
                            DisplayFinalReading(meterAccuracyCheckEntity);
                            cmbTestduration.Enabled = true;
                            DisplayDeltaValues();
                            btnStart.Text = START;
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
            }
            catch (Exception ex)
            {
                Duration_Timer.Stop();
                MessageBox.Show(resourceMgr.GetString("Failure"), BCSConstants.BCS);
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.ClosePort();
            }

        }
        /// <summary>
        /// This method is used to start reading parameters.
        /// </summary>
        private void StartAccuracyCheck()
        {
            string strTemp = string.Empty;
            string completeData = string.Empty;
            bool flag = false;
            MeterAccuracyCheckEntity meterAccuracyCheckEntity = new MeterAccuracyCheckEntity();
            DLMS650FormatterMeterAccuracyCheck formatterMeterAccuracyCheck = new DLMS650FormatterMeterAccuracyCheck();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (ConnectToMeter())
                {
                    completeData += ReadParametersObjects(out flag) + Symbols.NEWLINE;
                    if (!flag)
                    {
                        completeData += ReadParameterData() + Symbols.NEWLINE;
                        if (!completeData.Contains(COMMessages.COSEMCONNECTIONFAILED))
                        {
                            completeData += ReadParametersScalarObjects() + Symbols.NEWLINE;
                            if (!completeData.Contains(COMMessages.COSEMCONNECTIONFAILED))
                                completeData += ReadParametersScalarUnits();
                        }
                        startDatetime = DateTime.Now;
                        completeData += strTemp + Symbols.NEWLINE;

                        if (!string.IsNullOrEmpty(completeData))
                        {
                            meterAccuracyCheckEntity = formatterMeterAccuracyCheck.GetDataForAccuracyCheck(completeData);
                        }
                        if (meterAccuracyCheckEntity.KVAh != null && meterAccuracyCheckEntity.KvarhLag != null && meterAccuracyCheckEntity.KvarhLead != null && meterAccuracyCheckEntity.KWh != null)
                        {
                            Duration_Timer.Enabled = true;
                            DisplayInitialReading(meterAccuracyCheckEntity);
                            DisplayUnit(meterAccuracyCheckEntity.isHTCT);
                            lblduration.Visible = true;
                        }
                    }
                }
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
            }
            catch (Exception ex)
            {
                Duration_Timer.Stop();
                //MessageBox.Show(resourceMgr.GetString("Failure"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        /// <summary>
        /// VBM - Used to Display unit for HTCT and LTCT meters
        /// </summary>
        /// <param name="isHTCT"></param>
        private void DisplayUnit(bool isHTCT)
        {
            lblActiveEnergyUnit.Visible = true;
            lblApparentEnergyUnit.Visible = true;
            lblReactiveLagUnit.Visible = true;
            lblReactiveLeadUnit.Visible = true;

            if (isHTCT)
            {
                lblActiveEnergyUnit.Text = MegaActiveEnergy;
                lblApparentEnergyUnit.Text = MegaApparentEnergy;
                lblReactiveLagUnit.Text = MegaReactiveEnergyLagLead;
                lblReactiveLeadUnit.Text = MegaReactiveEnergyLagLead;
            }
        }
        /// <summary>
        /// To display the delta values - Difference of Final readings and Initial reading.
        /// </summary>
        private void DisplayDeltaValues()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtkVAhFinal.Text) && !String.IsNullOrEmpty(txtkVAhInitial.Text))
                {
                    txtkVAhDelta.Text = (Convert.ToDecimal(txtkVAhFinal.Text) - Convert.ToDecimal(txtkVAhInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLagFinal.Text) && !String.IsNullOrEmpty(txtkvarhLagInitial.Text))
                {
                    txtkvarhLagDelta.Text = (Convert.ToDecimal(txtkvarhLagFinal.Text) - Convert.ToDecimal(txtkvarhLagInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLeadFinal.Text) && !String.IsNullOrEmpty(txtkvarhLeadInitial.Text))
                {
                    txtkvarhLeadDelta.Text = (Convert.ToDecimal(txtkvarhLeadFinal.Text) - Convert.ToDecimal(txtkvarhLeadInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkWhFinal.Text) && !String.IsNullOrEmpty(txtkWhInitial.Text))
                {
                    // Solved bug 101884.
                    txtkWhDelta.Text = (Convert.ToDecimal(txtkWhFinal.Text) - Convert.ToDecimal(txtkWhInitial.Text)).ToString();
                }

                Duration_Timer.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        ///<summary>
        /// To display the final readings of energy parameters.
        /// </summary>
        private void DisplayFinalReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            txtkVAhFinal.Text = meterAccuracyCheckEntity.KVAh.ToString();
            txtkvarhLagFinal.Text = meterAccuracyCheckEntity.KvarhLag.ToString();
            txtkvarhLeadFinal.Text = meterAccuracyCheckEntity.KvarhLead.ToString();
            txtkWhFinal.Text = meterAccuracyCheckEntity.KWh.ToString();

        }
        ///<summary>
        /// To display the initial readings of energy parameters.
        /// </summary>
        private void DisplayInitialReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            txtkVAhInitial.Text = meterAccuracyCheckEntity.KVAh.ToString();
            txtkvarhLagInitial.Text = meterAccuracyCheckEntity.KvarhLag.ToString();
            txtkvarhLeadInitial.Text = meterAccuracyCheckEntity.KvarhLead.ToString();
            txtkWhInitial.Text = meterAccuracyCheckEntity.KWh.ToString();
        }
        /// <summary>
        /// This method is used to read scalar ojects
        /// </summary>
        /// <returns></returns>
        private string ReadParametersScalarObjects()
        {
            string strTemp = string.Empty;
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

            byte ret = ReadScalarProfile(3, 7);
            if (ret == 0x01)
            {
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }

            }
            else
            {
                GlobalObjects.objSerialComm.ClosePort();
                Duration_Timer.Stop();
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                strTemp = COMMessages.COSEMCONNECTIONFAILED;
                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
            return strTemp;
        }
        /// <summary>
        /// This method is used to read scalar unit data
        /// </summary>
        /// <returns></returns>
        private string ReadParametersScalarUnits()
        {
            string strTemp = string.Empty;

            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            byte ret = ReadScalarProfile(2, 7);
            if (ret == 0x01)
            {
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }

            }
            else
            {
                GlobalObjects.objSerialComm.ClosePort();
                Duration_Timer.Stop();
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                strTemp = COMMessages.COSEMCONNECTIONFAILED;
                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return strTemp;
        }
        /// <summary>
        /// This method is used to read parameters data
        /// </summary>
        /// <returns></returns>
        private string ReadParameterData()
        {
            string strTemp = string.Empty;
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            byte ret = fReadMeterAccuracyCheck(2);
            if (ret == 0x01)
            {

                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }

            }
            else
            {
                GlobalObjects.objSerialComm.ClosePort();
                Duration_Timer.Stop();
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                strTemp = COMMessages.COSEMCONNECTIONFAILED;
                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return strTemp;
        }
        private void rdFastDownload_CheckedChanged(object sender, EventArgs e)
        {
            //If Phasor command is available for Utility. Then show it.
            if (UtilityDetails.ShowPhasorReadFastDownload)
            {
                chkPhasor.Visible = true;
                chkPhasor.Checked = true;
            }

            //If midnight command is not available for utility then hide it.
            if (!UtilityDetails.ShowMidNightReadFastDownload)
            {
                chkMidnightData.Visible = false;
                chkMidnightData.Checked = false;
            }

            grpBoxBillingHistory.Enabled = false;
            grpBoxEventLog.Enabled = false;
            grpBoxLS.Enabled = false;
        }


        /// <summary>
        /// This method is used to read parameters objects.
        /// </summary>
        /// <returns></returns>
        private string ReadParametersObjects(out bool flag)
        {
            string strTemp = string.Empty;
            flag = false;
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            byte ret = fReadMeterAccuracyCheck(3);
            if (ret == 0x01)
            {
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }

            }
            else
            {
                GlobalObjects.objSerialComm.ClosePort();
                Duration_Timer.Stop();
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                flag = true;
                strTemp = COMMessages.COSEMCONNECTIONFAILED;
                MessageBox.Show(COMMessages.COSEMCONNECTIONFAILED, COMMessages.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return strTemp;
        }
        /// <summary>
        /// This method is used to used for sign on with meter.
        /// </summary>
        /// <returns></returns>
        private bool ConnectToMeter()
        {

            if (!DLMSMain.fDLMSConnectAccuracyCheck(out connectionMessage))
            {
                Duration_Timer.Stop();
                btnStart.Text = START;
                cmbTestduration.Enabled = true;
                this.Cursor = Cursors.Default;
                return false;
            }
            return true;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            ValidateParameters();
        }

        /// <summary>
        /// This method is used for clear all the controls when accuracy check is started.
        /// </summary>
        private void ValidateParameters()
        {
            if (btnStart.Text.Equals(START))
            {
                this.StatusMessage = string.Empty;
                btnStart.Text = STOP;
                cmbTestduration.Enabled = false;
                Application.DoEvents();
                lblduration.Visible = true;
                lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("DefaultDurationValue");
                txtkVAhDelta.Text = string.Empty;
                txtkvarhLagDelta.Text = string.Empty;
                txtkvarhLeadDelta.Text = string.Empty;
                txtkWhDelta.Text = string.Empty;
                txtkVAhFinal.Text = string.Empty;
                txtkVAhInitial.Text = string.Empty;
                txtkvarhLagFinal.Text = string.Empty;
                txtkvarhLagInitial.Text = string.Empty;
                txtkvarhLeadFinal.Text = string.Empty;
                txtkvarhLeadInitial.Text = string.Empty;
                txtkWhFinal.Text = string.Empty;
                txtkWhInitial.Text = string.Empty;
                Application.DoEvents();
                // To start reading parameters on start of accuracy check.
                StartAccuracyCheck();
            }
            else
            {
                this.StatusMessage = string.Empty;
                // To start reading parameters on stop of accuracy check.
                StopAccuracyCheck();
                cmbTestduration.Enabled = true;
            }

        }
        private void DisplayDuration(TimeSpan dtDuration)
        {
            if ((((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) > Convert.ToInt32(cmbTestduration.Text) * 60) || (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) < Convert.ToInt32(cmbTestduration.Text) * 60))
            {

                if (Convert.ToInt32(cmbTestduration.Text) < 60)
                {
                    lblduration.Text = resourceMgr.GetString("Duration")
                        + resourceMgr.GetString("Zero") + Symbols.COLON + Convert.ToInt32(cmbTestduration.Text).ToString(resourceMgr.GetString("Zero"))
                        + Symbols.COLON + resourceMgr.GetString("Zero");
                }
                else
                {
                    lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("One")
                        + Symbols.COLON + resourceMgr.GetString("Zero") + Symbols.COLON + resourceMgr.GetString("Zero");
                }
            }
            else
            {
                lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero"))
                    + Symbols.COLON + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero"))
                    + Symbols.COLON + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
            }
        }
        /// <summary>
        /// This method is used for refresh the timer duration and displaying it in the user screen.
        /// </summary>
        /// <param name="e"></param>
        private void RefreshDuration(EventArgs e)
        {
            TimeSpan dtDuration = DateTime.Now - startDatetime;
            if (cmbTestduration.Text != string.Empty)
            {
                if (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) == Convert.ToInt32(cmbTestduration.Text) * 60)
                {
                    // solved bug 100977.
                    if (connectionMessage.Equals(COMMessages.HDLCCONNECTIONFAILED) || connectionMessage.Equals(COMMessages.COSEMCONNECTIONFAILED))
                        Duration_Timer.Stop();
                    else
                    {
                        btnStart_Click(this, e);
                        dtDuration = DateTime.Now - startDatetime;
                        DisplayDuration(dtDuration);
                    }
                }
                else
                {

                    if (connectionMessage.Equals(COMMessages.HDLCCONNECTIONFAILED) || connectionMessage.Equals(COMMessages.COSEMCONNECTIONFAILED))
                        Duration_Timer.Stop();
                    else
                    {
                        dtDuration = DateTime.Now - startDatetime;
                        lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero"))
                            + Symbols.COLON + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero")) +
                            Symbols.COLON + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
                    }
                }

            }
        }
        private void btnAccuracyCheckCancel_Click(object sender, EventArgs e)
        {
            if (btnStart.Text.Equals(STOP))
            {
                int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("TestRunning"), BCSConstants.BCS, MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (msgres != 6) return;
            }
            this.StatusMessage = string.Empty;
            DLMSMain.fDLMSDisconnect();
            this.Close();
        }

        private void Duration_Timer_Tick(object sender, EventArgs e)
        {
            RefreshDuration(e);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            nudCTRatio.Value = 0;
            nudPTRatio.Value = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudPTRatio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnCTPTWrite_Click(object sender, EventArgs e)
        {
            WritePTRatio(false);
        }
        private int WritePTRatio()
        {
            try
            {

                SerialPortSettings.Default.ServerSAP = 0x01;

                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                fIncrementTimer();
                Application.DoEvents();
                /*GKG 02/12/2013 PT RATIO CHANGES*/
                //byte ptRatio = (byte)nudPTRatio.Value;
                //return WritePTRatio(ptRatio, false);
                return WritePTRatio((int)nudPTRatio.Value, false);
                /*GKG 02/12/2013 PT RATIO CHANGES*/
            }
            catch (Exception ex)
            {
                return 1;
            }

        }
        private int WritePTRatio(bool isRead)
        {
            bool isConnected = false;
            int retValue = 1;
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
               //Cabcon config commands if GSM is enabled      
                //If communication type is gprs then open imei selector window
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                        retValue = 10; //Explictly retorn 10 , Means User calcels SimSelectForm.
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {

                    StartTimer();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    fIncrementTimer();
                    if (DLMSMain.fDLMSConnect() != true)
                    {

                        StopTimer();
                        return 5;
                    }
                    isConnected = true;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    fIncrementTimer();
                    Application.DoEvents();
                    /*GKG 02/12/2013 PT RATIO CHANGES*/
                    //byte ptRatio = (byte)nudPTRatio.Value;
                    //return WritePTRatio(ptRatio, isRead);
                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.PDCHRY.ToString() && !isRead)
                    {
                        int meterPTratio = fReadCTRatio() * (int)nudPTRatio.Value;
                        if (meterPTratio > 7500)
                        {
                            MessageBox.Show(MaxEmfLimitMessage, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            retValue = 0x0B;
                        }
                        else
                        {
                            retValue = WritePTRatio((int)nudPTRatio.Value, isRead);
                        }
                    }
                    else
                    {
                        retValue = WritePTRatio((int)nudPTRatio.Value, isRead);
                    }


                    /*GKG 02/12/2013 PT RATIO CHANGES*/
                }
                return retValue;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            finally
            {
                StopTimer();
                if (isConnected)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }

                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        private int WriteCTRatio()
        {
            try
            {

                SerialPortSettings.Default.ServerSAP = 0x01;

                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                Application.DoEvents();
                Int16 intCT = (Int16)nudCTRatio.Value;
                /* GKG Why this code is written*/
                // byte ptRatio = (byte)nudPTRatio.Value;
                /* GKG Why this code is written*/
                byte[] ctRatio = BitConverter.GetBytes(intCT);
                return WriteCTRatio(ctRatio, false);
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {


            }
        }
        private int WriteCTRatio(bool isRead)
        {
            bool isConnected = false;
            int retValue = 1;
            CommunicationType comType = CommunicationType.DIRECT;
            bool isChannelInitialized = true;
            try
            {
               //Cabcon config commands if GSM is enabled      
                //If communication type is gprs then show imei selector window
                if (UtilityDetails.EnableGSMCommunication || UtilityDetails.ShowGPRSCommunication)
                {
                    comType = CommunicationTypeDetail.GetCommunicationType();
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN || comType == CommunicationType.GPRS)
                    {
                        isChannelInitialized = CheckChannelInitialization(comType);
                        retValue = 10; // Explicitly return 10 to identify that it comes from SimSelect Form.
                    }
                }
                //Piyush : //Cabcon config commands if GSM is enabled config commands if GSM is enabled
                if (isChannelInitialized)
                {

                    StartTimer();
                    SerialPortSettings.Default.ServerSAP = 0x01;
                    fIncrementTimer();
                    if (DLMSMain.fDLMSConnect() != true)
                    {
                        StopTimer();
                        return 5;
                    }
                    isConnected = true;
                    GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                    GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                    GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                    GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                    fIncrementTimer();
                    Application.DoEvents();
                    Int16 intCT = (Int16)nudCTRatio.Value;
                    /* GKG Why this code is written*/
                    // byte ptRatio = (byte)nudPTRatio.Value;
                    /* GKG Why this code is written*/
                    byte[] ctRatio = BitConverter.GetBytes(intCT);

                    if (UtilityDetails.PrimaryUtlityName == UtilityEntity.PDCHRY.ToString() && !isRead)
                    {
                        int meterPTratio = fReadPTRatio() * (int)nudCTRatio.Value;
                        if (meterPTratio > 7500)
                        {
                            MessageBox.Show(MaxEmfLimitMessage, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            retValue = 0x0B;
                        }
                        else
                        {
                            retValue = WriteCTRatio(ctRatio, isRead);
                        }
                    }
                    else
                    {
                        retValue = WriteCTRatio(ctRatio, isRead);
                    }


                }
                return retValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            finally
            {
                StopTimer();
                if (isConnected)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                if (UtilityDetails.EnableGSMCommunication)
                {
                    if (comType == CommunicationType.GSM || comType == CommunicationType.PSTN)
                    {
                        if (isChannelInitialized)
                        {
                            toolstripStatus.Text = "Resetting modem..";
                            Application.DoEvents();
                            LeaveModemToUtilityConfig();
                            toolstripStatus.Text = string.Empty;
                            Application.DoEvents();
                        }
                        else
                        {
                            this.toolstripStatus.Text = "Can not initialize local/remote modem";
                        }

                    }
                }
                GlobalObjects.objSerialComm.ClosePort();
            }
        }

        private void tbCTPTRatio_Click(object sender, EventArgs e)
        {

        }

        private void btnCTPTWrite_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            WritePTRatio(false);
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            nudCTRatio.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nudPTRatio.Value = 0;
        }

        private void btnCTRatio_Click(object sender, EventArgs e)
        {

        }

        private void btnReadPTRatio_Click(object sender, EventArgs e)
        {

        }

        private void btnCTRatio_Click_1(object sender, EventArgs e)
        {
            int writeStatus = 0;
            writeStatus = WriteCTRatio(true);
            // Return Value 10 means User calcels the SimSlect Form , So do nothing
            if (writeStatus == 10)
                return;
            if (writeStatus == 0)
            {
                MessageBox.Show(READSUCCESSMESSAGE, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 1)
            {
                MessageBox.Show(OPERATIONFAILED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 2)
            {
                MessageBox.Show(ACCESSDENIED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show(CoreUtility.GetMessageFromEnum(writeStatus).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }



        }

        private void btnCTPTWrite_Click_2(object sender, EventArgs e)
        {
            int writeStatus = 0;
            writeStatus = WriteCTRatio(false);
            // Return Value 10 means User calcels the SimSlect Form , So do nothing
            if (writeStatus == 10)
                return;
            if (writeStatus == 0)
            {
                MessageBox.Show(WRITESUCCESSMESSAGE, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 1)
            {
                MessageBox.Show(OPERATIONFAILED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 2)
            {
                MessageBox.Show(ACCESSDENIED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 0x0B)
            {
                // MessageBox.Show("CT Ratio not programmable." , BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show(CoreUtility.GetMessageFromEnum(writeStatus).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }




        }
        //Success,
        //   Fail,
        //   AccessDenied,
        //   DataUnavailable,
        //   TimeOut,
        //   SignOnFailed,
        //   CosemConnectionFailed,
        //   MeterIDMismatch
        private void btnReadPTRatio_Click_1(object sender, EventArgs e)
        {
            int writeStatus = 0;
            writeStatus = WritePTRatio(true);
            // Return Value 10 means User calcels the SimSlect Form , So do nothing
            if (writeStatus == 10)
                return;
            if (writeStatus == 0)
            {
                MessageBox.Show(READSUCCESSMESSAGE, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 1)
            {
                MessageBox.Show(OPERATIONFAILED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 2)
            {
                MessageBox.Show(ACCESSDENIED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show(CoreUtility.GetMessageFromEnum(writeStatus).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }


        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            if (nudPTRatio.Value > 320 && nudPTRatio.Value < 1)
            {
                MessageBox.Show(PTRATIOVALMESAGE);
            }

            int writeStatus = 0;
            writeStatus = WritePTRatio(false);
            // Return Value 10 means User calcels the SimSlect Form , So do nothing
            if (writeStatus == 10)
                return;
            if (writeStatus == 0)
            {
                MessageBox.Show(WRITESUCCESSMESSAGE, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 1)
            {
                MessageBox.Show(OPERATIONFAILED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 2)
            {
                MessageBox.Show(ACCESSDENIED, BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeStatus == 0x0B)
            {
                // MessageBox.Show("PT Ratio not programmable.", BCSConstants.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show(CoreUtility.GetMessageFromEnum(writeStatus).ToString(), CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }

        private void btnReset_Click_2(object sender, EventArgs e)
        {
            nudCTRatio.Value = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            nudPTRatio.Value = 0;
        }

        private void chkPhasor_CheckedChanged(object sender, EventArgs e)
        {
            chkInsta_CheckedChanged(sender, e);
        }

        private void nudPTRatio_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void nudPTRatio_KeyUp(object sender, KeyEventArgs e)
        {
            /*GKG 02/12/2013 PT RATIO CHANGES*/
            //if (nudPTRatio.Value > 100)
            //{
            //    nudPTRatio.Value = 100;
            //}
            int maxVal = 100;
            if (UtilityDetails.ShowTwoBytePTRatio)
            {
                maxVal = 320;
            }
            if (nudPTRatio.Value > maxVal)
            {
                nudPTRatio.Value = maxVal;
            }
            /*GKG 02/12/2013 PT RATIO CHANGES*/
        }

        private void nudCTRatio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabMeterAccuracyCheck_Click(object sender, EventArgs e)
        {

        }

        private void nudCTRatio_KeyUp(object sender, KeyEventArgs e)
        {
            if (nudCTRatio.Value > 320)
            {
                nudCTRatio.Value = 320;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            String strFileName;
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "CFH Files (*.cfh)|*.cfh|All Files (*.*)|*.*";
            DialogResult result = OpenFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = OpenFileDialog1.FileName;
            }
            else
                return;

            if (fCheckBCC(strFileName))
                MessageBox.Show("valid file");
            else
                MessageBox.Show("invalid file");
        }

        private void btnReadPhasor_Click(object sender, EventArgs e)
        {
            STOPPHASOR = false;

            PHASORRUNNING = true;
            //thPhasor = new Thread(new ThreadStart(GeneratePhasor));
            //thPhasor.Start();
            //thPhasor.IsBackground = true;

            btnHold.Enabled = true;
            btnReadPhasor.Enabled = false;
            //If the utility is of CESC read in normal mode
            if (UtilityDetails.GetUtilityDetails() == UtilityEntity.CESC)
            {
                GeneratePhasorInNormalMode();
            }
            else
            {
                GeneratePhasor();
            }
        }

        private void UpdatePhasor(PhasorEntity phasorData)
        {
            if (phasorData.PhaseSequence == "Correct")
            {
                phasorDiagram1.PhasorData = phasorData;
                phasorDiagram1.Refresh();
                phasorDiagram1.Show();
                lblPhasorData.Visible = false;
                phasorDiagram1.Visible = true;

            }
            else
            {
                lblPhasorData.Text = "Phase sequence is not correct. Phasor can not be drawn.";
                lblPhasorData.Visible = true;
                phasorDiagram1.Visible = false;
            }
            //lngPhasor.Data = getPhasorDataSet(phasorData);
            //lngPhasor.SetWidth(0, 150);
            //lngPhasor.SetWidth(1, 100);
            //lngPhasor.Refresh();
            //  DataSet ds = getPhasorDataSet(phasorData);
            if (phasorData != null)
            {
                lblRVoltageValue.Text = phasorData.RPhaseVoltage;
                lblYVoltageValue.Text = phasorData.YPhaseVoltage;
                lblBVoltageValue.Text = phasorData.BPhaseVoltage;
                lblRCurrentValue.Text = phasorData.RPhaseCurrent;
                lblYCurrentValue.Text = phasorData.YPhaseCurrent;
                lblBCurrentValue.Text = phasorData.BPhaseCurrent;
                lblActivePowerValue.Text = phasorData.ActivePower;
                lblReactivePowerValue.Text = phasorData.ReActivePower;
                lblApparentPowerValue.Text = phasorData.ApparentPower;
                lblRPhasePFValue.Text = phasorData.RPhasePowerFactor;
                lblYPhasePFValue.Text = phasorData.YPhasePowerFactor;
                lblBPhaesPFValue.Text = phasorData.BPhasePowerFactor;
                lblFrequencyValue.Text = phasorData.Frequency;
                lblPhaseSeqValue.Text = phasorData.PhaseSequence;
                lblRPhaseKWDirVAlue.Text = phasorData.RPhaseNegativePowerFlag;
                lblYPhaseKWDirValue.Text = phasorData.YPhaseNegativePowerFlag;
                lblBPhaseKWDirValue.Text = phasorData.BPhaseNegativePowerFlag;
                lblRChannelValue.Text = phasorData.RPhaseChannel;
                lblYChannelValue.Text = phasorData.YPhaseChannel;
                lblBChannelValue.Text = phasorData.BPhaseChannel;
                lblRLagLeadValue.Text = phasorData.RPhaseCapacitiveInductiveFlag;
                lblYLagLeadValue.Text = phasorData.YPhaseCapacitiveInductiveFlag;
                lblBLagLeadValue.Text = phasorData.BPhaseCapacitiveInductiveFlag;
                lblAngelYRValue.Text = phasorData.AngleYR;
                lblAngleBRValue.Text = phasorData.AngleBR;
                lblAngleBwTwoValue.Text = phasorData.AngleBetweenTwo;
                lblTotalPWFactorValue.Text = phasorData.TotalPhasePowerFactor;

            }
            this.UseWaitCursor = false;
        }
        /// <summary>
        /// Generate phasor in normal mode (DLMS)
        /// </summary>
        private void GeneratePhasorInNormalMode()
        {
            BillingGeneralNFDLMSEntity master = new BillingGeneralNFDLMSEntity();
            string[] data = new string[4];
            try
            {
                if (fDLMSConnect())
                {
                    while (true)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        SetStatusMessage("Reading Phasor data.....");
                        this.UseWaitCursor = true;

                        #region Read Phasor
                        string captureObjects = string.Empty;
                        string captureData = string.Empty;
                        string scalarObjects = string.Empty;
                        string scalarUnitAndData = string.Empty;
                        bool bSuccess = true;
                        SerialPortSettings.Default.ServerSAP = 0x01;
                        Application.DoEvents();
                        byte ret = ReadPhasorNormalMode(3);
                        if (ret == 0x01)
                        {
                            String strTemp = "";
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            for (int i = 0; i < length; i++)
                            {
                                strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            captureObjects = strTemp;
                        }
                        else if (ret == 0x07)
                        {
                            bSuccess = false;
                            //write an empty line so that parser can predict that nothing in this line should be read

                        }
                        else
                        {
                            StopTimer();
                            GlobalObjects.objSerialComm.ClosePort();
                            MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            bSuccess = false;


                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        if (bSuccess)
                        {
                            ret = ReadPhasorNormalMode(2);
                            if (ret == 0x01)
                            {
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                captureData = strTemp;
                            }
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                bSuccess = false;
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;

                            }
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        if (bSuccess)
                        {
                            ret = ReadScalarProfile(3, 8);
                            if (ret == 0x01)
                            {

                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                scalarObjects = strTemp;
                            }
                            //fix - Ashish 04/10/11
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                bSuccess = false;
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;

                            }
                        }
                        GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                        GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                        GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                        GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                        if (bSuccess)
                        {
                            ret = ReadScalarProfile(2, 8);
                            if (ret == 0x01)
                            {
                                String strTemp = "";
                                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                                for (int i = 0; i < length; i++)
                                {
                                    strTemp = strTemp + String.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                                }
                                scalarUnitAndData = strTemp;
                            }
                            else if (ret == 0x07)
                            {
                                //write an empty line so that parser can predict that nothing in this line should be read
                                bSuccess = false;
                            }
                            else
                            {
                                StopTimer();
                                GlobalObjects.objSerialComm.ClosePort();
                                MessageBox.Show("Cosem Connection Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                bSuccess = false;

                            }
                        }
                        #endregion

                        if (!bSuccess)
                        {
                            PHASORRUNNING = false;
                            btnReadPhasor.Enabled = true;
                            btnHold.Enabled = false;
                            MessageBox.Show("HDLC connection failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            SetStatusMessage(string.Empty);
                            break;
                        }
                        else
                        {
                            data[0] = captureObjects;
                            data[1] = captureData;
                            data[2] = scalarObjects;
                            data[3] = scalarUnitAndData;
                        }
                        SetStatusMessage("Updating Phasor data.....");
                        DLMS650FormatterPhasor formatterPhasor = new DLMS650FormatterPhasor();
                        formatterPhasor.LoadPhasorData(data, master); ;
                        Application.DoEvents();

                        if (STOPPHASOR)
                        {
                            PHASORRUNNING = false;
                            fDLMSDisconnect();
                            MessageBox.Show("Phasor readout stopped.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            toolstripStatus.Text = string.Empty;
                            break;
                        }

                        PhasorEntity phasorData = master.Phasor;

                        //Invoke(new RefreshPhasorDiagram(UpdatePhasor), phasorData);
                        UpdatePhasor(phasorData);


                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                SetStatusMessage(string.Empty);
                this.Cursor = Cursors.Default;
                this.UseWaitCursor = false;
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        private void GeneratePhasor()
        {

            SetStatusMessage("Reading Phasor data.....");

            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (ReadMeterID(out meterID, out lngFileName, false))
                {
                    Application.DoEvents();
                    FastDownLoadingBLL fastDownLoadingDAL = new FastDownLoadingBLL(meterID);

                    FastDownLoadStatuses status = fastDownLoadingDAL.DownloadData("COM1", FastDownLoadOptions.General, out downloadedData);
                    if (status != FastDownLoadStatuses.None)
                    {
                        fDLMSDisconnect();
                        PHASORRUNNING = false;
                        btnReadPhasor.Enabled = true;
                        btnHold.Enabled = false;
                        MessageBox.Show("HDLC connection failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetStatusMessage(string.Empty);
                        return;
                    }
                    ParseFDLGeneralData generalParser = new ParseFDLGeneralData(downloadedData, string.Empty, 0, 0);
                    DLMS650NamePlateDetailsEntity nameplateDetails = new DLMS650NamePlateDetailsEntity();
                    generalParser.Parse(out nameplateDetails);

                    while (true)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        try
                        {

                            SetStatusMessage("Reading Phasor data.....");
                            this.UseWaitCursor = true;
                            status = fastDownLoadingDAL.DownloadData("COM1", FastDownLoadOptions.Phasor, out downloadedData);
                            if (status != FastDownLoadStatuses.None)
                            {
                                fDLMSDisconnect();
                                PHASORRUNNING = false;
                                btnReadPhasor.Enabled = true;
                                btnHold.Enabled = false;
                                MessageBox.Show("HDLC connection failed.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                SetStatusMessage(string.Empty);
                                break;
                            }

                            ParseFDLPhasorData phasorParsor = new ParseFDLPhasorData(downloadedData, string.Empty, 0, 0);
                            Application.DoEvents();

                            if (STOPPHASOR)
                            {
                                PHASORRUNNING = false;
                                fDLMSDisconnect();
                                MessageBox.Show("Phasor readout stopped.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                toolstripStatus.Text = string.Empty;
                                break;
                            }

                            PhasorEntity phasorData = phasorParsor.getPhasorEntity();

                            //Invoke(new RefreshPhasorDiagram(UpdatePhasor), phasorData);
                            UpdatePhasor(phasorData);

                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                            this.UseWaitCursor = false;
                            GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    toolstripStatus.Text = string.Empty;
                    PHASORRUNNING = false;
                    Application.DoEvents();
                    btnReadPhasor.Enabled = true;
                    btnHold.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.UseWaitCursor = false;
                GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            }


        }

        private void SetStatusMessage(string msg)
        {
            toolstripStatus.Text = msg;
            Application.DoEvents();
        }

        private void btnHold_Click(object sender, EventArgs e)
        {

            STOPPHASOR = true;

            //  GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
            PHASORRUNNING = false;
            toolstripStatus.Text = "Phasor readout stopped.";
            btnReadPhasor.Enabled = true;
            btnHold.Enabled = false;
            this.Cursor = Cursors.Default;
            Application.DoEvents();
        }


        private DataSet getPhasorDataSet(PhasorEntity phasorData)
        {
            DataTable dtPhasor = new DataTable();
            DLMS650CommonBLL common = new DLMS650CommonBLL();
            Dictionary<string, string> dicPhasorParam = common.GetPhasorDisplayParameter();

            dtPhasor.Columns.Add(new DataColumn("Parameter"));
            dtPhasor.Columns.Add(new DataColumn("Value"));

            foreach (KeyValuePair<string, string> phasorParam in dicPhasorParam)
            {
                DataRow drRow = dtPhasor.NewRow();
                drRow["Parameter"] = phasorParam.Value;
                drRow["Value"] = getParamValue(phasorParam.Key, phasorData);
                dtPhasor.Rows.Add(drRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dtPhasor);
            return ds;
        }

        private string getParamValue(string phasorParam, PhasorEntity entity)
        {
            if (string.Equals(phasorParam, "RPhaseCurrent", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhaseCurrent;
            }
            else if (string.Equals(phasorParam, "YPhaseCurrent", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhaseCurrent;
            }
            else if (string.Equals(phasorParam, "BPhaseCurrent", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhaseCurrent;
            }
            else if (string.Equals(phasorParam, "RPhaseVoltage", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhaseVoltage;
            }
            else if (string.Equals(phasorParam, "YPhaseVoltage", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhaseVoltage;
            }
            else if (string.Equals(phasorParam, "BPhaseVoltage", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhaseVoltage;
            }
            else if (string.Equals(phasorParam, "RPhasePowerFactor", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhasePowerFactor;
            }
            else if (string.Equals(phasorParam, "YPhasePowerFactor", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhasePowerFactor;
            }
            else if (string.Equals(phasorParam, "BPhasePowerFactor", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhasePowerFactor;
            }
            else if (string.Equals(phasorParam, "TotalPhasePowerFactor", StringComparison.OrdinalIgnoreCase))
            {
                return entity.TotalPhasePowerFactor;
            }
            else if (string.Equals(phasorParam, "Frequency", StringComparison.OrdinalIgnoreCase))
            {
                return entity.Frequency;
            }
            else if (string.Equals(phasorParam, "ApparentPower", StringComparison.OrdinalIgnoreCase))
            {
                return entity.ApparentPower;
            }
            else if (string.Equals(phasorParam, "ActivePower", StringComparison.OrdinalIgnoreCase))
            {
                return entity.ActivePower;
            }
            else if (string.Equals(phasorParam, "ReactivePower", StringComparison.OrdinalIgnoreCase))
            {
                return entity.ReActivePower;
            }
            else if (string.Equals(phasorParam, "RPhaseNegativePowerFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhaseNegativePowerFlag;
            }
            else if (string.Equals(phasorParam, "YPhaseNegativePowerFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhaseNegativePowerFlag;
            }
            else if (string.Equals(phasorParam, "BPhaseNegativePowerFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhaseNegativePowerFlag;
            }
            else if (string.Equals(phasorParam, "RPhaseCapacitiveInductiveFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhaseCapacitiveInductiveFlag;
            }
            else if (string.Equals(phasorParam, "YPhaseCapacitiveInductiveFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhaseCapacitiveInductiveFlag;
            }
            else if (string.Equals(phasorParam, "BPhaseCapacitiveInductiveFlag", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhaseCapacitiveInductiveFlag;
            }
            else if (string.Equals(phasorParam, "AngleYR", StringComparison.OrdinalIgnoreCase))
            {
                return entity.AngleYR;
            }
            else if (string.Equals(phasorParam, "AngleBR", StringComparison.OrdinalIgnoreCase))
            {
                return entity.AngleBR;
            }
            else if (string.Equals(phasorParam, "AngleBetweenTwo", StringComparison.OrdinalIgnoreCase))
            {
                return entity.AngleBetweenTwo;

            }
            else if (string.Equals(phasorParam, "RPhaseChannel", StringComparison.OrdinalIgnoreCase))
            {
                return entity.RPhaseChannel;

            }
            else if (string.Equals(phasorParam, "BPhaseChannel", StringComparison.OrdinalIgnoreCase))
            {
                return entity.BPhaseChannel;

            }
            else if (string.Equals(phasorParam, "YPhaseChannel", StringComparison.OrdinalIgnoreCase))
            {
                return entity.YPhaseChannel;

            }
            else if (string.Equals(phasorParam, "PhaseSequence", StringComparison.OrdinalIgnoreCase))
            {
                return entity.PhaseSequence;

            }
            return string.Empty;
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Equals(tabControlMain.SelectedTab.Name, "tabPageCompartment1", StringComparison.OrdinalIgnoreCase))
            {
                if (SerialPortSettings.Default.CommunicationType == CommunicationType.GSM.ToString()
                    || SerialPortSettings.Default.CommunicationType == CommunicationType.PSTN.ToString()
                    || SerialPortSettings.Default.CommunicationType == CommunicationType.GPRS.ToString()
                    )
                {
                    cmbLSDays.Visible = true;
                    cmbLSDays.SelectedItem = SerialPortSettings.Default.LPReadDays.ToString();
                }
                else
                {
                    cmbLSDays.Visible = false;
                }
            }
            this.UseWaitCursor = false;
            if (!string.Equals(tabControlMain.SelectedTab.Name, "tabPhasor", StringComparison.OrdinalIgnoreCase))
            {


                // GlobalObjects.objSerialComm = new SerialCommunication.SerialComm();
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                STOPPHASOR = true;
                if (PHASORRUNNING)
                    toolstripStatus.Text = "Wait..while phasor communication is stopped";


            }
            else
            {
                btnReadPhasor.Enabled = true;
                btnHold.Enabled = false;
            }

            //If Setting tab clicked and FS mode is applicable then add FS option to combo box.
            if (string.Equals(tabControlMain.SelectedTab.Name, "tabPageCompartment4", StringComparison.OrdinalIgnoreCase))
            {
                if (!UtilityDetails.ShowFSMode && cmbMode.Items.Contains(" FS "))
                {
                    cmbMode.Items.Remove(" FS ");
                }
            }
            //If Setting tab clicked and FS mode is applicable then add FS option to combo box.
            if (string.Equals(tabControlMain.SelectedTab.Name, "tabProgramming", StringComparison.OrdinalIgnoreCase))
            {
                tabCTPTRatio.SelectedIndex = 0;
                tabCTPTRatio.Refresh();
            }

        }

        private void btnCancelPhasor_Click(object sender, EventArgs e)
        {
            //  fDLMSDisconnect();
            STOPPHASOR = true;
            PHASORRUNNING = false;
            this.UseWaitCursor = false;
            Application.DoEvents();
            this.Close();
        }

        private void tabControlMain_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }



        private void lblAngleBwTwo_Click(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void DLMSMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            STOPPHASOR = true;
            PHASORRUNNING = false;
        }

        private int WriteMDReset(out string messageText)
        {
            messageText = string.Empty;
            int writeResponse = WriteMDResetCommand();

            if (writeResponse == (int)CoreUtility.DLMSResultType.Success)
            {
                messageText = "Billing reset completed successfully.";
            }
            else if (writeResponse == (int)CoreUtility.DLMSResultType.AccessDenied)
            {
                messageText = "Access Denied.";
                //MessageBox.Show("Read/Write Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else if (writeResponse == (int)CoreUtility.DLMSResultType.CosemConnectionFailed)
            {
                messageText = "Cosem Connection Failed.";
                //MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return writeResponse;
        }
        private void btnMDReset_Click(object sender, EventArgs e)
        {
            string messageText = string.Empty;
            if (DialogResult.OK != MessageBox.Show("Are you sure to reset Billing?", CoreUtility.BCS, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            btnMDReset.Enabled = false;

            //Connect to meter via DLMS
            if (DLMSMain.fDLMSConnect() != true)
            {
                btnMDReset.Enabled = true;
                this.Cursor = Cursors.Default;
                return;
            }

            try
            {
                // the message will come in message text, empty if no message
                WriteMDReset(out messageText);
                if (!string.IsNullOrEmpty(messageText))
                {
                    MessageBox.Show(messageText, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                btnMDReset.Enabled = true;
            }
        }

        /// <summary>
        /// Write MD Reset command to meter.
        /// </summary>
        /// <returns></returns>
        private int WriteMDResetCommand()
        {
            ProgrammingCode returnCode = ProgrammingCode.CosemConnectionFailed;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryMDReset(HDLCCommand, HDLCIndex, 1);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    returnCode = ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForReset(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            returnCode = ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            returnCode = ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            returnCode = ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        returnCode = ProgrammingCode.CosemConnectionFailed;
                    }

                }
                return (int)returnCode;
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void tabCTPTRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCTPTRatio.SelectedTab.Name == "tabMDReset")
            {
                btnMDReset.Enabled = chkMDReset.Checked;

            }
            else if (tabCTPTRatio.SelectedTab.Name == "tbPDisplayParameters")
            {
                if ((cmbMode.Text.ToLower().Contains("fs") && UtilityDetails.ShowDisplayParameters)
                    || cmbMode.Text.ToLower().Contains("us") && UtilityDetails.ShowDisplayParametersInUSMode)
                {
                    //if (UtilityDetails.ShowDisplayParameters)
                    //{
                    if (tabControlDisplayParams.SelectedTab.Name == "tabPagePushButton")
                    {

                        FillDisplayParameters(dGVPushDisplayParams, "PUSH");
                        dGVPushDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVPushDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVPushDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        FillDisplayParameters(selectedPushParams, dGVPushDisplayParams);
                        dGVPushDisplayParams.Columns["SNO"].Width = 80;
                        dGVPushDisplayParams.Columns["ID"].Width = 130;
                        dGVPushDisplayParams.Columns["Description"].Width = 300;
                        dGVPushDisplayParams.Columns["colInclude"].Width = 85;
                        dGVPushDisplayParams.Refresh();
                    }
                    else if (tabControlDisplayParams.SelectedTab.Name == "tabPageScrollButton")
                    {
                        FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
                        dGVScrollDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVScrollDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVScrollDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        FillDisplayParameters(selectedScrollParams, dGVScrollDisplayParams);

                        dGVScrollDisplayParams.Columns["SNO"].Width = 80;
                        dGVScrollDisplayParams.Columns["ID"].Width = 130;
                        dGVScrollDisplayParams.Columns["Description"].Width = 300;
                        dGVScrollDisplayParams.Columns["colInclude"].Width = 85;
                        dGVScrollDisplayParams.Refresh();
                    }
                    else if (tabControlDisplayParams.SelectedTab.Name == "tabPageHighResolution")
                    {
                        FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");
                        dGVHighResolution.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVHighResolution.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dGVHighResolution.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        FillDisplayParameters(selectedHighResParams, dGVHighResolution);

                        dGVHighResolution.Columns["SNO"].Width = 80;
                        dGVHighResolution.Columns["ID"].Width = 130;
                        dGVHighResolution.Columns["Description"].Width = 300;
                        dGVHighResolution.Columns["colInclude"].Width = 85;

                        dGVHighResolution.Refresh();
                    }
                    //  }
                }
            }
        }

        private void chkMDReset_CheckedChanged(object sender, EventArgs e)
        {
            btnMDReset.Enabled = chkMDReset.Checked;
        }

        /// <summary>
        /// Enable/Disable the KVAH Selection parameters
        /// </summary>
        /// <param name="enable"></param>
        private void EnableKVAHSelectionParameters(bool enable)
        {
            btnReadKVAhSelection.Enabled = enable;
            btnWriteKVAhSelection.Enabled = enable;
        }
        private void btnWriteKVAhSelection_Click(object sender, EventArgs e)
        {
            if (!chkKVAhLagOnly.Checked && !chkKVAhLagLead.Checked)
            {
                MessageBox.Show("Please select kvah Selection mode.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            EnableKVAHSelectionParameters(false);

            //Connect to meter
            if (DLMSMain.fDLMSConnect() != true)
            {
                this.Cursor = Cursors.Default;
                EnableKVAHSelectionParameters(true);
                return;
            }

            try
            {
                byte KVAhSelection;

                if (chkKVAhLagOnly.Checked)
                {
                    KVAhSelection = 0x00;
                }
                else
                {
                    KVAhSelection = 0x01;
                }

                //Write selected kvah option to meter
                int writeResponse = fWriteKVAhSelection(KVAhSelection);

                if (writeResponse == (int)ProgrammingCode.Success)
                {
                    MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Read/Write Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                EnableKVAHSelectionParameters(true);
            }
        }

        /// <summary>
        /// Writes passed kvah selection mode to meter and returns result
        /// </summary>
        /// <param name="KVAhSelection"></param>
        /// <returns></returns>
        private int fWriteKVAhSelection(byte KVAhSelection)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteKVAhSelection(HDLCCommand, HDLCIndex, 2);

                HDLCCommand[HDLCIndex++] = KVAhSelection;

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        return (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void btnReadKVAhSelection_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EnableKVAHSelectionParameters(false);

            Application.DoEvents();
            if (DLMSMain.fDLMSConnect() != true)
            {
                EnableKVAHSelectionParameters(true);
                this.Cursor = Cursors.Default;
                return;
            }
            try
            {
                //Read meter to get KVAH selection details
                int writeResponse = fReadKVAhSelection();

                if (writeResponse == (int)ProgrammingCode.Success)
                {
                    //Display defined parameter to screen.
                    DisplayKVAhSelection(GlobalObjects.objSerialComm.ReceiveBuffer);
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access denied", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                {
                    MessageBox.Show("Data unavailable", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                EnableKVAHSelectionParameters(true);
                this.Cursor = Cursors.Default;
            }
        }

        private void btnUpScroll_Click(object sender, EventArgs e)
        {
            DataGridView dgvDisplayParams = null;
            if (tabControlDisplayParams.SelectedIndex == 0)
            {
                dgvDisplayParams = dGVPushDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 1)
            {
                dgvDisplayParams = dGVScrollDisplayParams;
            }
            else if (tabControlDisplayParams.SelectedIndex == 2)
            {
                dgvDisplayParams = dGVHighResolution;
            }

            int SelRow = dgvDisplayParams.CurrentRow.Index;// dGVPushDisplayParams.SelectedRows[0].Index;
            if (SelRow > 0)
            {

                String tempDispID, tempDispInfo;
                bool currentColInclude = false;
                tempDispID = dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value.ToString();
                tempDispInfo = dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value.ToString();
                if (dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value != null)
                {
                    currentColInclude = Convert.ToBoolean(dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value);
                }
                dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value = dgvDisplayParams.CurrentRow.Cells["ID"].Value;
                dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value = dgvDisplayParams.CurrentRow.Cells["Description"].Value;
                dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value = dgvDisplayParams.CurrentRow.Cells["colInclude"].Value;
                dgvDisplayParams.CurrentRow.Cells["colInclude"].Value = currentColInclude.ToString();
                dgvDisplayParams.CurrentRow.Cells["ID"].Value = tempDispID;
                dgvDisplayParams.CurrentRow.Cells["Description"].Value = tempDispInfo;

                dgvDisplayParams.ClearSelection();
                dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Selected = true;
                dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value = true;
            }
        }

        private void btnDownScroll_Click(object sender, EventArgs e)
        {

            String tempDispID, tempDispInfo;
            DataGridView dgvDisplayParams = null;
            bool colInclude = false;
            try
            {
                if (tabControlDisplayParams.SelectedIndex == 0)
                    dgvDisplayParams = dGVPushDisplayParams;
                else if (tabControlDisplayParams.SelectedIndex == 1)
                    dgvDisplayParams = dGVScrollDisplayParams;
                else if (tabControlDisplayParams.SelectedIndex == 2)
                    dgvDisplayParams = dGVHighResolution;

                int SelRow = dgvDisplayParams.CurrentRow.Index;// dGVPushDisplayParams.SelectedRows[0].Index;
                if (SelRow != dgvDisplayParams.Rows.Count - 1)
                {
                    if (SelRow < dgvDisplayParams.Rows.Count - 1)
                    {

                        tempDispID = dgvDisplayParams.Rows[SelRow + 1].Cells[2].Value.ToString();
                        tempDispInfo = dgvDisplayParams.Rows[SelRow + 1].Cells[3].Value.ToString();
                        if (dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value != null)
                        {
                            colInclude = Convert.ToBoolean(dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value);
                        }
                        dgvDisplayParams.Rows[SelRow + 1].Cells[2].Value = dgvDisplayParams.CurrentRow.Cells[2].Value;
                        dgvDisplayParams.Rows[SelRow + 1].Cells[3].Value = dgvDisplayParams.CurrentRow.Cells[3].Value;
                        dgvDisplayParams.Rows[SelRow + 1].Cells["colInclude"].Value = dgvDisplayParams.CurrentRow.Cells["colInclude"].Value;
                        dgvDisplayParams.CurrentRow.Cells[2].Value = tempDispID;
                        dgvDisplayParams.CurrentRow.Cells[3].Value = tempDispInfo;
                        dgvDisplayParams.CurrentRow.Cells["colInclude"].Value = colInclude;
                        dgvDisplayParams.ClearSelection();
                        dgvDisplayParams.Rows[SelRow + 1].Cells[2].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private DataGridView GetSelectedGrid()
        {
            DataGridView dgvDisplayParams = null;
            for (int counter = 0; counter < 3; counter++)
            {
                if (tabControlDisplayParams.SelectedIndex == counter)
                {
                    switch (counter)
                    {
                        case 0:
                            dgvDisplayParams = dGVPushDisplayParams;
                            break;
                        case 1:
                            dgvDisplayParams = dGVScrollDisplayParams;
                            break;
                        case 2:
                            dgvDisplayParams = dGVHighResolution;
                            break;
                    }
                    break;
                }
            }
            return dgvDisplayParams;
        }
        private DataGridView GetSelectedDisplayGrid(int counter)
        {
            DataGridView dgvDisplayParams = null;
            switch (counter)
            {
                case 0:
                    dgvDisplayParams = dGVPushDisplayParams;
                    break;
                case 1:
                    dgvDisplayParams = dGVScrollDisplayParams;
                    break;
                case 2:
                    dgvDisplayParams = dGVHighResolution;
                    break;
            }
            return dgvDisplayParams;
        }
        private void btnReadDisplayParams_Click(object sender, EventArgs e)
        {

            if (tabControlDisplayParams.SelectedIndex == 3)
            {
                ReadDisplayParameterSetting();
            }
            else
            {
                ReadDisplayParameter();
            }

        }
        private void SetEnableDisplayButton(bool isEnable)
        {
            btnWriteDisplayParams.Enabled = isEnable;
            btnReadDisplayParams.Enabled = isEnable;
            if (isEnable)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
            }
        }
        private void ReadDisplayParameterSetting()
        {
            try
            {
                SetEnableDisplayButton(false);
                btnCancel.Enabled = false;

                txtScrollTime.Text = "";
                txtPushButtonTimeout.Text = "";
                txtScrollResumeTime.Text = "";

                this.Cursor = Cursors.WaitCursor;

                Application.DoEvents();
                if (DLMSMain.fDLMSConnect() != true)
                {
                    SetEnableDisplayButton(true);
                    return;
                }

                int writeResponse = ReadDisplayParamsTimeouts();

                if (writeResponse == (int)ProgrammingCode.Success)
                {
                    FillDisplayParametersTimeouts(GlobalObjects.objSerialComm.ReceiveBuffer);
                    //GlobalObjects.objCOSEMLIB.BlockBuffer
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access denied", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                {
                    MessageBox.Show("Data unavailable", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                SetEnableDisplayButton(true);
            }
        }

        private void FillDisplayParametersTimeouts(byte[] receivedData)
        {
            int scrollTime = 0;
            int pushTimeout = 0;
            int autoScrollTime = 0;

            scrollTime = Convert.ToInt32((receivedData[0x15] << 8) | (receivedData[0x16]));

            pushTimeout = Convert.ToInt32((receivedData[0x18] << 8) | (receivedData[0x19]));

            if (receivedData[0x1B] == 0)
            {
                chkBoxAutoScrollTime.Checked = false;
            }
            else
            {
                chkBoxAutoScrollTime.Checked = true;
            }

            autoScrollTime = Convert.ToInt32(((receivedData[0x1D]) << 8) | (receivedData[0x1E]));
            txtScrollTime.Text = scrollTime.ToString();
            txtPushButtonTimeout.Text = pushTimeout.ToString();
            if (autoScrollTime != 0)
            {
                txtScrollResumeTime.Text = autoScrollTime.ToString();
            }
        }

        private int ReadDisplayParamsTimeouts()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPushDisplayParameterTimeouts(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void ReadDisplayParameter()
        {
            bool isMeterConnected = false;
            try
            {

                btnWriteDisplayParams.Enabled = false;
                btnReadDisplayParams.Enabled = false;
                btnCancel.Enabled = false;

                DataGridView dgvDisplayParams = GetSelectedGrid();
                if (dgvDisplayParams != null)
                {
                    foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                    {
                        row.Cells["colInclude"].Value = false;
                    }
                    this.Cursor = Cursors.WaitCursor;

                    Application.DoEvents();
                    if (DLMSMain.fDLMSConnect())
                    {
                        isMeterConnected = true;
                        int writeResponse = ReadDisplayParams(tabControlDisplayParams.SelectedIndex);
                        if (writeResponse == (int)ProgrammingCode.Success)
                        {
                            FillDisplayParameters(GlobalObjects.objSerialComm.ReceiveBuffer);
                            //GlobalObjects.objCOSEMLIB.BlockBuffer
                        }
                        else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show("Access denied", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        }
                        else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
                        {
                            MessageBox.Show("Data unavailable", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        }
                        else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                        {
                            MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (isMeterConnected)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                // fButtonMode(SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                btnWriteDisplayParams.Enabled = true;
                btnReadDisplayParams.Enabled = true;
                btnCancel.Enabled = false;

            }
        }
        private byte GetReadDisplayParameterQuery(byte[] HDLCommand, byte HDLCIndex, int displayParamIndex)
        {
            if (displayParamIndex == 0)
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPushDisplayParameter(HDLCCommand, HDLCIndex, 2);
            }
            else if (displayParamIndex == 1)
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadScrollDisplayParameter(HDLCCommand, HDLCIndex, 2);
            }
            else if (displayParamIndex == 2)
            {
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadHighResolutionDisplayParameter(HDLCCommand, HDLCIndex, 2);
            }
            return HDLCIndex;
        }
        private int ReadDisplayParams(int displayParamIndex)
        {
            int readEnum = 0;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GetReadDisplayParameterQuery(HDLCCommand, HDLCIndex, displayParamIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    readEnum = (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            readEnum = (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            readEnum = (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            readEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            readEnum = (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        readEnum = (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                return readEnum;
            }
            catch (Exception ex)
            {
                readEnum = (int)ProgrammingCode.CosemConnectionFailed;
                return readEnum;
            }
        }

        private void fMoveDisplayRow(int nRowIndex, DataGridView dgvDisplayParams)
        {


            int SelRow = nRowIndex;// dGVPushDisplayParams.SelectedRows[0].Index;
            if (SelRow > 0)
            {

                String tempDispID, tempDispInfo;
                tempDispID = dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value.ToString();
                tempDispInfo = dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value.ToString();

                dgvDisplayParams.Rows[SelRow - 1].Cells["ID"].Value = dgvDisplayParams.Rows[SelRow - 2].Cells["ID"].Value;
                dgvDisplayParams.Rows[SelRow - 1].Cells["Description"].Value = dgvDisplayParams.Rows[SelRow - 2].Cells["Description"].Value;

                dgvDisplayParams.Rows[SelRow - 2].Cells["ID"].Value = tempDispID;
                dgvDisplayParams.Rows[SelRow - 2].Cells["Description"].Value = tempDispInfo;
                dgvDisplayParams.ClearSelection();
                //dgvDisplayParams.Rows[SelRow - 1].Cells["colInclude"].Value = true;

            }

        }
        /// <summary>
        /// This function checks the checkbox in display parameter grid and move the checked rows into upper part of the grid
        /// </summary>
        /// <param name="receivedData"></param>
        private void FillDisplayParameters(byte[] receivedData)
        {
            int index = 0x14;
            DataGridView dgvDisplayParams = null;
            dgvDisplayParams = GetSelectedGrid();


            int nParamCount = receivedData[index - 1];
            byte nDispParam = 0;


            for (int paramCounter = 0; paramCounter < nParamCount; paramCounter++)
            {
                int rowCounter = 0;
                nDispParam = receivedData[paramCounter + index];
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    rowCounter++;
                    if (Convert.ToInt32(row.Cells["ID"].Value) == nDispParam)
                    {
                        //row.Cells["colInclude"].Value = true;
                        for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                        {
                            fMoveDisplayRow(tempRowCounter, dgvDisplayParams);
                        }

                    }
                }
            }
            dgvDisplayParams.ClearSelection();
            dgvDisplayParams.Rows[0].Cells[3].Selected = true;
            for (int displayParamCounter = 0; displayParamCounter < nParamCount; displayParamCounter++)
            {
                dgvDisplayParams.Rows[displayParamCounter].Cells["colInclude"].Value = true;
            }
        }
        private void FillDisplayParameters(List<byte> receivedData, DataGridView dgvDisplayParams)
        {
            int nParamCount = receivedData.Count;
            byte nDispParam = 0;
            foreach (DataGridViewRow row in dgvDisplayParams.Rows)
            {
                row.Cells["colInclude"].Value = false;
            }
            Application.DoEvents();

            for (int paramCounter = 0; paramCounter < nParamCount; paramCounter++)
            {
                int rowCounter = 0;
                nDispParam = receivedData[paramCounter];
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    rowCounter++;
                    if (Convert.ToInt32(row.Cells["ID"].Value) == nDispParam)
                    {
                        //row.Cells["colInclude"].Value = true;
                        for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                        {
                            fMoveDisplayRow(tempRowCounter, dgvDisplayParams);
                        }

                    }
                }
            }

            dgvDisplayParams.ClearSelection();
            dgvDisplayParams.Rows[0].Cells[2].Selected = true;


            for (int displayParamCounter = 0; displayParamCounter < nParamCount; displayParamCounter++)
            {
                dgvDisplayParams.Rows[displayParamCounter].Cells["colInclude"].Value = true;
            }
            Application.DoEvents();
        }
        private void chkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView dgvTemp = null;

            for (int counter = 0; counter < 3; counter++)
            {
                if (tabControlDisplayParams.SelectedIndex == counter)
                {
                    switch (counter)
                    {
                        case 0:
                            dgvTemp = dGVPushDisplayParams;
                            break;
                        case 1:
                            dgvTemp = dGVScrollDisplayParams;
                            break;
                        case 2:
                            dgvTemp = dGVHighResolution;
                            break;
                    }
                    CheckAllTheElementsInGrid(dgvTemp);
                    break;
                }
            }
        }
        private void CheckAllTheElementsInGrid(DataGridView dgvTemp)
        {
            if (chkBoxSelectAll.Checked == true)
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = true;
                }

            }
            else
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = false;
                }
            }
        }
        private List<byte> GetSelectedRowsinSelectedDisplayParameterGrid()
        {
            List<byte> displayParams = new List<byte>();
            DataGridView dgvDisplayParams = GetSelectedGrid();
            if (dgvDisplayParams != null)
            {
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                    {
                        //displayParams.Add(displayID);
                        /* GKG 04/02/2013 TFS ID 134921 */
                        //displayParams.Add(Convert.ToByte(row.Cells["colInclude"].Value));
                        displayParams.Add(Convert.ToByte(row.Cells["ID"].Value));
                        /* GKG 04/02/2013 TFS ID 134921 */
                    }
                }
            }
            return displayParams;
        }
        private List<byte> GetSelectedRowsDisplayParameterGrid(int gridId)
        {
            List<byte> displayParams = new List<byte>();
            DataGridView dgvDisplayParams = GetSelectedDisplayGrid(gridId);
            if (dgvDisplayParams != null)
            {
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                    {
                        //displayParams.Add(displayID);
                        /* GKG 04/02/2013 TFS ID 134921 */
                        //displayParams.Add(Convert.ToByte(row.Cells["colInclude"].Value));
                        displayParams.Add(Convert.ToByte(row.Cells["ID"].Value));
                        /* GKG 04/02/2013 TFS ID 134921 */
                    }
                }
            }
            return displayParams;
        }
        /// <summary>
        /// Get Selected row in parameter Grid
        /// </summary>
        /// <param name="dgvDisplayParams"></param>
        /// <returns></returns>
        private List<byte> GetSelectedRowsinParameterGrid(DataGridView dgvDisplayParams)
        {
            List<byte> displayParams = new List<byte>();
            //DataGridView dgvDisplayParams = GetSelectedGrid();
            if (dgvDisplayParams != null)
            {
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                    {
                        //displayParams.Add(displayID);
                        displayParams.Add(Convert.ToByte(row.Cells["ID"].Value));
                    }
                }
            }
            return displayParams;
        }
        /// <summary>
        /// Write Display parameters in the meter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteDisplayParams_Click(object sender, EventArgs e)
        {
            if (tabControlDisplayParams.SelectedIndex == 3)
            {
                WriteDisplayTimeoutSetting();
            }
            else
            {
                WriteDisplayParamSetting();
            }

        }
        private void WriteDisplayTimeoutSetting()
        {
            btnWriteDisplayParams.Enabled = false;
            btnReadDisplayParams.Enabled = false;

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            if (DLMSMain.fDLMSConnect() != true)
            {
                btnWriteDisplayParams.Enabled = true;
                btnReadDisplayParams.Enabled = true;
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return;
            }

            try
            {
                if (ValidateDisplayParamsTimeout(txtScrollTime.Text.Trim(), txtPushButtonTimeout.Text.Trim(), txtScrollResumeTime.Text.Trim()) == false)
                {
                    return;
                }
                int scrollResumeTime = string.IsNullOrEmpty(txtScrollResumeTime.Text.Trim()) ? 0 : Convert.ToInt32(txtScrollResumeTime.Text);
                int scrollResumeSelected = (chkAutoScrollTime.Checked) ? 1 : 0;
                int writeResponse = WriteDisplayTimeouts(Convert.ToInt32(txtPushButtonTimeout.Text), Convert.ToInt32(txtScrollTime.Text), scrollResumeTime, scrollResumeSelected);

                if (writeResponse == (int)ProgrammingCode.Success)
                {
                    MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                {
                    MessageBox.Show("Access Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
                else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                {
                    MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DLMSMain.fDLMSDisconnect();
                GlobalObjects.objSerialComm.ClosePort();
                btnWriteDisplayParams.Enabled = true;
                btnReadDisplayParams.Enabled = true;
                this.Cursor = Cursors.Default;
                btnCancel.Enabled = true;
            }
        }

        private int WriteDisplayTimeouts(int scrollTime, int pushTimeout, int autoScrollTime, int autoScrollModeSelected)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteDisplayParameterTimeouts(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.ffillDisplayParamsTimeouts(HDLCCommand, HDLCIndex, scrollTime, pushTimeout, autoScrollTime, autoScrollModeSelected);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else if (ret == 0x03)
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;

                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private bool ValidateDisplayParamsTimeout(string scrollTime, string pushTimeOut, string autoScrollTime)
        {
            string validationMessage = string.Empty;
            if (scrollTime == string.Empty)
            {

                validationMessage += "Scroll time can not be left blank." + Symbols.NEWLINE;
            }
            if (pushTimeOut == string.Empty)
            {

                validationMessage += "Push button timeout can not be left blank." + Symbols.NEWLINE;

            }
            if (chkAutoScrollTime.Checked && autoScrollTime == string.Empty)
            {

                validationMessage += "Auto scroll resume time can not be left blank." + Symbols.NEWLINE;

            }

            if (!string.IsNullOrEmpty(scrollTime) && (ServiceClass.ServiceInstance.ValidateRegEx(scrollTime, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid scroll time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(scrollTime) && (Convert.ToInt32(scrollTime) < 1 || Convert.ToInt32(scrollTime) > 300))
            {

                validationMessage += "Scroll time can contain value between 1 and 300." + Symbols.NEWLINE;
            }


            if (!string.IsNullOrEmpty(pushTimeOut) && (ServiceClass.ServiceInstance.ValidateRegEx(pushTimeOut, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid push button timeout." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(pushTimeOut) && (Convert.ToInt32(pushTimeOut) < 1 || Convert.ToInt32(pushTimeOut) > 600))
            {

                validationMessage += "Push button timeout can contain value between 1 and 600." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (ServiceClass.ServiceInstance.ValidateRegEx(autoScrollTime, @"^([0-9]{0,3})$") == false))
            {

                validationMessage += "Invalid auto scroll resume time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (Convert.ToInt32(autoScrollTime) < 3 || Convert.ToInt32(autoScrollTime) > 300))
            {

                validationMessage += "Auto scroll resume time can contain value between 3 and 300." + Symbols.NEWLINE;

            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                MessageBox.Show(validationMessage, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            else
            {
                return true;
            }

        }

        private string ValidateDisplayTimeout(string scrollTime, string pushTimeOut, string autoScrollTime)
        {
            //string message = string.Empty;
            //if (scrollTime == string.Empty)
            //{
            //    message = "Scroll time can not be left blank.";
            //}
            //else if (pushTimeOut == string.Empty)
            //{
            //    message = "Push button timeout can not be left blank.";
            //}

            //else if (!string.IsNullOrEmpty(scrollTime) && ServiceClass.ServiceInstance.ValidateRegEx(scrollTime, @"^([0-9]{1,3})$") == false)
            //{
            //    message = "Invalid scroll time.";
            //}
            //else if ( !string.IsNullOrEmpty(pushTimeOut) && ServiceClass.ServiceInstance.ValidateRegEx(pushTimeOut, @"^([0-9]{1,3})$") == false)
            //{
            //    message = "Invalid push button timeout.";
            //}
            //else if (ServiceClass.ServiceInstance.ValidateRegEx(autoScrollTime, @"^([0-9]{0,3})$") == false)
            //{
            //    message = "Invalid auto scroll resume time.";
            //}
            //else if (Convert.ToInt32(scrollTime) < 1 || Convert.ToInt32(scrollTime) > 300)
            //{
            //    message = "Scroll time can contain value between 1 and 300.";
            //}
            //else if (Convert.ToInt32(pushTimeOut) < 1 || Convert.ToInt32(pushTimeOut) > 600)
            //{
            //    message = "Push button timeout can contain value between 1 and 600.";
            //}
            //else if (!string.IsNullOrEmpty(autoScrollTime) && (Convert.ToInt32(autoScrollTime) < 1 || Convert.ToInt32(autoScrollTime) > 300))
            //{
            //    message = "Auto scroll resume time can contain value between 1 and 300.";
            //}
            string validationMessage = string.Empty;
            if (scrollTime == string.Empty)
            {

                validationMessage += "Scroll time can not be left blank." + Symbols.NEWLINE;
            }
            if (pushTimeOut == string.Empty)
            {

                validationMessage += "Push button timeout can not be left blank." + Symbols.NEWLINE;

            }
            if (chkAutoScrollTime.Checked && autoScrollTime == string.Empty)
            {

                validationMessage += "Auto scroll resume time can not be left blank." + Symbols.NEWLINE;

            }

            if (!string.IsNullOrEmpty(scrollTime) && (ServiceClass.ServiceInstance.ValidateRegEx(scrollTime, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid scroll time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(scrollTime) && (Convert.ToInt32(scrollTime) < 1 || Convert.ToInt32(scrollTime) > 300))
            {

                validationMessage += "Scroll time can contain value between 1 and 300." + Symbols.NEWLINE;
            }


            if (!string.IsNullOrEmpty(pushTimeOut) && (ServiceClass.ServiceInstance.ValidateRegEx(pushTimeOut, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid push button timeout." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(pushTimeOut) && (Convert.ToInt32(pushTimeOut) < 1 || Convert.ToInt32(pushTimeOut) > 600))
            {

                validationMessage += "Push button timeout can contain value between 1 and 600." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (ServiceClass.ServiceInstance.ValidateRegEx(autoScrollTime, @"^([0-9]{0,3})$") == false))
            {

                validationMessage += "Invalid auto scroll resume time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (Convert.ToInt32(autoScrollTime) < 3 || Convert.ToInt32(autoScrollTime) > 300))
            {

                validationMessage += "Auto scroll resume time can contain value between 3 and 300." + Symbols.NEWLINE;

            }


            return validationMessage;
        }
        private void WriteDisplayParamSetting()
        {
            btnWriteDisplayParams.Enabled = false;
            btnReadDisplayParams.Enabled = false;
            bool isConnected = false;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            List<byte> displayParams = null;


            try
            {
                displayParams = GetSelectedRowsinSelectedDisplayParameterGrid();
                // Get the selected rows in a list

                if (displayParams.Count >= 1)
                {
                    if (DLMSMain.fDLMSConnect())
                    {
                        isConnected = true;

                        int writeResponse = WriteDisplayParameter(displayParams, tabControlDisplayParams.SelectedIndex);

                        if (writeResponse == (int)ProgrammingCode.Success)
                        {
                            MessageBox.Show("Parameter written successfully.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show("Access Denied.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        }
                        else if (writeResponse == (int)ProgrammingCode.AccessDenied)
                        {
                            MessageBox.Show("Access Denied.Please change the mode.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
                        {
                            MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast one parameter from the display list.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (isConnected)
                {
                    DLMSMain.fDLMSDisconnect();
                }
                GlobalObjects.objSerialComm.ClosePort();
                this.Cursor = Cursors.Default;
                btnWriteDisplayParams.Enabled = true;
                btnReadDisplayParams.Enabled = true;
            }
        }


        //// this is new function
        //this.Cursor = Cursors.WaitCursor;
        //EnableKVAHSelectionParameters(false);

        //Application.DoEvents();
        //if (DLMSMain.fDLMSConnect() != true)
        //{
        //    EnableKVAHSelectionParameters(true);
        //    this.Cursor = Cursors.Default;
        //    return;
        //}
        //try
        //{
        //    //Read meter to get KVAH selection details
        //    int writeResponse = fReadKVAhSelection();

        //    if (writeResponse == (int)ProgrammingCode.Success)
        //    {
        //        //Display defined parameter to screen.
        //        DisplayKVAhSelection(GlobalObjects.objSerialComm.ReceiveBuffer);
        //    }
        //    else if (writeResponse == (int)ProgrammingCode.AccessDenied)
        //    {
        //        MessageBox.Show("Access denied", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        //        return;
        //    }
        //    else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
        //    {
        //        MessageBox.Show("Data unavailable", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        //        return;
        //    }
        //    else if (writeResponse == (int)ProgrammingCode.CosemConnectionFailed)
        //    {
        //        MessageBox.Show("Cosem Connection Failed.", CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

        //        return;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show(ex.Message, CoreUtility.BCS, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}
        //finally
        //{
        //    DLMSMain.fDLMSDisconnect();
        //    EnableKVAHSelectionParameters(true);
        //    this.Cursor = Cursors.Default;
        //}


        /// <summary>
        /// Sets the defined parameter to the correct option 
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayKVAhSelection(byte[] receivedData)
        {
            if (receivedData[20] == 0x00)
            {
                chkKVAhLagOnly.Checked = true;
            }
            else
            {
                chkKVAhLagLead.Checked = true;
            }


        }

        private int WriteDisplayParameter(List<byte> displayParams, int displayParamIndex)
        {
            int writeEnum = 0;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (displayParamIndex == 0)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWritePushDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }
                else if (displayParamIndex == 1)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteScrollDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }
                else if (displayParamIndex == 2)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteHighResolutionDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }

                HDLCIndex = GlobalObjects.objHDLCLIB.ffillDisplayParameters(HDLCCommand, HDLCIndex, displayParams);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            writeEnum = (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else if (ret == 0x03)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                return writeEnum;
            }
            catch (Exception ex)
            {
                writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                return writeEnum;
            }
        }

        private int WriteDisplayParameterToCMRI(int displayParamIndex)
        {
            int writeEnum = 0;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (displayParamIndex == 0)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWritePushDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }
                else if (displayParamIndex == 1)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteScrollDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }
                else if (displayParamIndex == 2)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteHighResolutionDisplayParameter(HDLCCommand, HDLCIndex, 2);
                }
                List<byte> paramList = GetSelectedRowsDisplayParameterGrid(displayParamIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.ffillDisplayParameters(HDLCCommand, HDLCIndex, paramList);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer))
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            writeEnum = (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else if (ret == 0x03)
                        {
                            writeEnum = (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                    {
                        writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                    }
                }
                return writeEnum;
            }
            catch (Exception ex)
            {
                writeEnum = (int)ProgrammingCode.CosemConnectionFailed;
                return writeEnum;
            }
        }
        /// <summary>
        /// Reads the KVAH selection defined in meter and returns the result.
        /// </summary>
        /// <returns></returns>
        private int fReadKVAhSelection()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadKVAhSelection(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }

        private void tabControlCMRI_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControlDisplayParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageDisplayTimeOut", StringComparison.OrdinalIgnoreCase))
            {
                btnUpScroll.Visible = false;
                btnDownScroll.Visible = false;
                chkBoxSelectAll.Visible = false;
            }
            else
            {
                btnUpScroll.Visible = true;
                btnDownScroll.Visible = true;
                chkBoxSelectAll.Visible = true;
            }

            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPagePushButton", StringComparison.OrdinalIgnoreCase))
            {
                List<byte> lstParam = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedScrollParams = lstParam;
                }

                lstParam = GetSelectedRowsinParameterGrid(dGVHighResolution);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedHighResParams = lstParam;
                }

                FillDisplayParameters(dGVPushDisplayParams, "PUSH");
                FillDisplayParameters(selectedPushParams, dGVPushDisplayParams);
                dGVPushDisplayParams.Columns["SNO"].Width = 80;
                dGVPushDisplayParams.Columns["ID"].Width = 130;
                dGVPushDisplayParams.Columns["Description"].Width = 300;
                dGVPushDisplayParams.Columns["colInclude"].Width = 85;
                dGVPushDisplayParams.Refresh();

            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageScrollButton", StringComparison.OrdinalIgnoreCase))
            {
                List<byte> lstParam = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedPushParams = lstParam;
                }

                lstParam = GetSelectedRowsinParameterGrid(dGVHighResolution);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedHighResParams = lstParam;
                }

                FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
                FillDisplayParameters(selectedScrollParams, dGVScrollDisplayParams);
                dGVScrollDisplayParams.Columns["SNO"].Width = 80;
                dGVScrollDisplayParams.Columns["ID"].Width = 130;
                dGVScrollDisplayParams.Columns["Description"].Width = 300;
                dGVScrollDisplayParams.Columns["colInclude"].Width = 85;
                dGVScrollDisplayParams.Refresh();

            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageHighResolution", StringComparison.OrdinalIgnoreCase))
            {
                List<byte> lstParam = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedPushParams = lstParam;
                }

                lstParam = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                if (lstParam != null && lstParam.Count > 0)
                {
                    selectedScrollParams = lstParam;
                }

                FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");
                FillDisplayParameters(selectedHighResParams, dGVHighResolution);
                dGVHighResolution.Columns["SNO"].Width = 80;
                dGVHighResolution.Columns["ID"].Width = 130;
                dGVHighResolution.Columns["Description"].Width = 300;
                dGVHighResolution.Columns["colInclude"].Width = 85;
                dGVHighResolution.Refresh();

            }

        }

        private void dGVPushDisplayParams_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dGVPushDisplayParams_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void tabControlDisplayParams_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void chkAutoScrollTime_Click(object sender, EventArgs e)
        {
            //if (chkBoxAutoScrollTime.Checked == true)
            //{
            //    txtScrollResumeTime.Enabled = true;
            //}
            //else
            //{
            //    txtScrollResumeTime.Text = "";
            //    txtScrollResumeTime.Enabled = false;
            //}

        }

        private void chkAutoScrollTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoScrollTime.Checked == true)
            {
                txtScrollResumeTime.Enabled = true;
            }
            else
            {
                txtScrollResumeTime.Text = "";
                txtScrollResumeTime.Enabled = false;
            }

        }

        private void dGVPushDisplayParams_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name != "Description")
            {
                int firstCellValue = (int)e.CellValue1;
                int secondCellValue = (int)e.CellValue2;
                e.SortResult = firstCellValue.CompareTo(secondCellValue);
                e.Handled = true;
            }
        }
        /*GKG 136251 : Check box issue */
        private void chkCMRIInstant_CheckedChanged(object sender, EventArgs e)
        {
            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;
            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked && chkCMRITamper.Checked
                && chkPhasor && chkMidnnight)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;
            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }

        private void chkCMRIBilling_CheckedChanged(object sender, EventArgs e)
        {
            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;
            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked && chkCMRITamper.Checked
                && chkPhasor && chkMidnnight)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;

            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }

        private void chkCMRILoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;
            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked && chkCMRITamper.Checked
                && chkMidnnight && chkPhasor)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;

            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }

        private void chkCMRITamper_CheckedChanged(object sender, EventArgs e)
        {
            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;
            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked && chkCMRITamper.Checked
                && chkPhasor && chkMidnnight)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;

            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }

        private void chkCMRINameplate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkCMRIMidnightData_CheckedChanged(object sender, EventArgs e)
        {

            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;

            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked
                && chkCMRITamper.Checked && chkMidnnight && chkPhasor)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;

            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }

        private void labelHLS_Click(object sender, EventArgs e)
        {

        }

        private void chkCMRIPhasor_CheckedChanged(object sender, EventArgs e)
        {
            chkCMRISelectAll.CheckedChanged -= chkCMRISelectAll_CheckedChanged;

            bool chkMidnnight = UtilityDetails.ShowMidnight ? chkCMRIMidnightData.Checked : true;
            bool chkPhasor = UtilityDetails.ShowPhasorInCMRINormalMode ? chkCMRIPhasor.Checked : true;

            if (chkCMRIInstant.Checked && chkCMRILoadSurvey.Checked && chkCMRIBilling.Checked && chkCMRITamper.Checked
                && chkMidnnight && chkPhasor)
                chkCMRISelectAll.Checked = true;
            else
                chkCMRISelectAll.Checked = false;

            chkCMRISelectAll.CheckedChanged += chkCMRISelectAll_CheckedChanged;
        }
        /*GKG 136251 : Check box issue */



    }

    public static class StreamExtensions
    {
        public static void CopyTo(this Stream input, Stream output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            } byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}

                    #endregion
