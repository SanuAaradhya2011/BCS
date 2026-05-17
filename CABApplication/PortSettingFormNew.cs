#region NameSpaces
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using SerialCommunication;
using System.Text;
using Hunt.EPIC.Logging;
//using CABApplication;
#endregion
namespace CAB.UI
{
    /// <summary>
    /// Used for setting com port baud rate and communication option
    /// </summary>
    public partial class PortSettingFormNew : MdiChildForm, IChannel
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        bool isConnectionTested = false;
        bool isPortAssociationChanged = false;
        int PreviousPortAssociationRowIndex = -1, PreviousPortAssociationColIndex = -1;
        bool PreviousPortAssociationValue = false;
        string DefaultPortName = string.Empty;
        byte[] MODEMCommand = new byte[20];
        byte MODEMIndex = 0;
        SystemSettingsBLL objSystemSettings = new SystemSettingsBLL();
        private static IList<CABSerialPort> lstSavedSerialPorts = new List<CABSerialPort>();
        bool areMultiplePortsPresent;
        bool isLoad = false;
        string[] serialportval;
        string signalstr = string.Empty;
        private ChannelDetail channelDetails = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(PortSettingFormNew).ToString());
        #endregion

        #region Properties

        public IPhysicalChannel PhysicalChannelDetail { get; set; }

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
        #endregion

        #region Constructer
        public PortSettingFormNew()
        {
            InitializeComponent();
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSettingFormNew_Load(object sender, EventArgs e)
        {
            isLoad = true;
            int counter = 0;
            BindDefaultSettings();
            BindPorts();
            string mode = ConfigSettings.GetValue("SelectionMode");

            SettingsBLL settingsBLL = new SettingsBLL();
            this.cboPort.DataSource = settingsBLL.GetPort().Tables[0];
            this.cboPort.DisplayMember = "DisplayMember";
            this.cboPort.ValueMember = "ValueMember";

            string value = ConfigSettings.GetValue("PortName");
            for (counter = 0; counter < cboPort.Items.Count; counter++)
            {
                cboPort.SelectedIndex = counter;
                if ((((System.Data.DataRowView)(cboPort.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }

            string FwType = ConfigSettings.GetValue("MeterFirmwareType");
            string channelType = ConfigSettings.GetValue("ChannelType");
            string baudRate = ConfigSettings.GetValue("BaudRate");
            string initialBaudRate = ConfigSettings.GetValue("InitialBaudRate");
            cboBaudRate.SelectedItem = baudRate;
            cboInitialbaudRate.SelectedItem = initialBaudRate;
            if (cboBaudRate.SelectedItem != null && cboBaudRate.SelectedItem.ToString() != "300")
                chk96DLMS.Text = string.Concat(cboBaudRate.SelectedItem, ", 8, N, 1 (3P/1P DLMS)");
            else
                chk96DLMS.Text = string.Concat("9600", ", 8, N, 1 (3P/1P DLMS)");
            chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");

            if (rdDirect.Checked)
            {
                panelDirect.Visible = true;
                panelRemote.Visible = false;
                btnModemConfig.Enabled = false;
                if (mode.ToUpper() == "AUTO")
                {
                    rbtAuto.Checked = true;
                    SetAutoDetails();
                }
                else if (mode.ToUpper() == "MANUAL")
                {
                    rbtManual.Checked = true;

                    if (FwType == "1")
                    {
                        chk96DLMS.Checked = true;
                        cboInitialbaudRate.Enabled = false;
                    }
                    if (FwType == "12")
                    {
                        chk96DLMS.Checked = true;
                        chk300IEC.Checked = true;
                        cboInitialbaudRate.Enabled = true;
                    }
                    if (FwType == "123")
                    {
                        chk96DLMS.Checked = true;
                        chk300IEC.Checked = true;
                        chk96IEC.Checked = true;
                        cboInitialbaudRate.Enabled = true;
                    }
                    if (FwType == "2")
                    {
                        chk300IEC.Checked = true;
                        chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");
                        chk96DLMS.Text = string.Concat("9600", ", 8, N, 1 (3P/1P DLMS)");
                        cboInitialbaudRate.Enabled = true;
                    }
                    if (FwType == "3")
                    {
                        chk96IEC.Checked = true;
                        chk96DLMS.Checked = false;
                        chk96DLMS.Text = string.Concat("9600", ", 8, N, 1 (3P/1P DLMS)");
                        cboInitialbaudRate.Enabled = false;
                    }
                    if (FwType == "13")
                    {
                        chk96DLMS.Checked = true;
                        chk96IEC.Checked = true;
                        cboInitialbaudRate.Enabled = false;
                    }
                    if (FwType == "23")
                    {
                        chk96DLMS.Checked = false;
                        chk96DLMS.Text = string.Concat("9600", ", 8, N, 1 (3P/1P DLMS)");
                        chk96IEC.Checked = true;
                        cboInitialbaudRate.Enabled = true;
                        chk300IEC.Checked = true;
                    }
                }
            }
            else if (rbtRemote.Checked)
            {
                btnModemConfig.Enabled = true;
                if (FwType == "1")
                {
                    cboInitialbaudRate.Enabled = false;
                    chk96DLMS.Checked = true;
                }
                if (FwType == "2")
                {
                    cboInitialbaudRate.Enabled = true;
                    chk300IEC.Checked = true;
                }
                if (FwType == "3")
                {
                    cboInitialbaudRate.Enabled = false;
                    chk96IEC.Checked = true;
                }
                chk96IEC.Enabled = chk300IEC.Enabled = chk96DLMS.Enabled = true;

                string CommunicationType = ConfigSettings.GetValue("ChannelType");
                if (CommunicationType == "GSM")
                    rbtGSM.Checked = true;
                if (CommunicationType == "PSTN")
                    rbtPSTN.Checked = true;
                if (CommunicationType == "TCP")
                    rbtTCP.Checked = true;
                if (CommunicationType == "GPRS")
                    rbtGPRS.Checked = true;
              }
            isLoad = false;
        }


        private void SetAutoDetails()
        {
            chk96DLMS.Checked = chk300IEC.Checked = chk96IEC.Checked = true;
            chk96DLMS.Enabled = chk300IEC.Enabled = chk96IEC.Enabled = cboBaudRate.Enabled = cboInitialbaudRate.Enabled = false;
            btnModemConfig.Enabled = false;
            btnModemInfo.Enabled = false;
        }
        private void SetGPRSDetails()
        {
            chk96DLMS.Checked = true;
            chk300IEC.Checked = chk96IEC.Checked = false;
            chk96DLMS.Enabled = chk300IEC.Enabled = chk96IEC.Enabled = cboBaudRate.Enabled = cboInitialbaudRate.Enabled = false;
            btnModemConfig.Enabled = false;
            btnModemInfo.Enabled = false;
        }

        private void SetManualDetails()
        {
            chk300IEC.Checked = chk96IEC.Checked = cboInitialbaudRate.Enabled = false;
            chk96DLMS.Checked = chk96DLMS.Enabled = chk300IEC.Enabled = chk96IEC.Enabled = cboBaudRate.Enabled = true;
            btnModemConfig.Enabled = true;
            btnModemInfo.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSettingForm_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCommMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboBaudRate.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }


        /// <summary>
        /// Close window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngbCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        /// <summary>
        /// Save com port details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngbSave_Click(object sender, EventArgs e)
        {
            if (rdDirect.Checked)
            {
                if (cboPort.SelectedItem != null)
                {
                    if (!chk96DLMS.Checked && !chk96IEC.Checked && !chk300IEC.Checked)
                    {
                        this.StatusMessage = "Please Select Meter Firmware Type.";
                        return;
                    }

                    ConfigSettings.ChangeNode("CommunicationType", "Direct");
                    ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.Direct.ToString());
                    ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
                    ConfigSettings.ChangeNode("BaudRate", cboBaudRate.Items[cboBaudRate.SelectedIndex].ToString());
                    ConfigSettings.ChangeNode("InitialBaudRate", cboInitialbaudRate.Items[cboInitialbaudRate.SelectedIndex].ToString());
                    if (rbtAuto.Checked)
                        ConfigSettings.ChangeNode("SelectionMode", "Auto");
                    if (rbtManual.Checked)
                        ConfigSettings.ChangeNode("SelectionMode", "Manual");
                    string firmwareType = string.Empty;
                    if (chk96DLMS.Checked)
                        firmwareType = ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "DLMS96008N13P1P"))).ToString();
                    if (chk300IEC.Checked)
                        firmwareType = string.Concat(firmwareType, ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "NONDLMS3007E13P1P"))).ToString());
                    if (chk96IEC.Checked)
                        firmwareType = string.Concat(firmwareType, ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "NONDLMS96008N11P"))).ToString());

                    ConfigSettings.ChangeNode("MeterFirmwareType", firmwareType.Trim());
                    ConfigSettings.ChangeNode("GSMConnected", "False");
                    this.StatusMessage = "Settings Saved Successfully.";
                }
                else
                {
                    MessageBox.Show("Please select communication port.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rbtRemote.Checked)
            {
                if (cboPort.SelectedItem != null)
                {
                    if (!chk96DLMS.Checked && !chk96IEC.Checked && !chk300IEC.Checked)
                    {
                        this.StatusMessage = "Please Select Meter Firmware Type.";
                        return;
                    }
                    ConfigSettings.ChangeNode("CommunicationType", "Remote");
                    if (rbtGSM.Checked)
                        ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.GSM.ToString());
                    if (rbtPSTN.Checked)
                        ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString());
                    if (rbtGPRS.Checked)
                        ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString());
                    if (rbtTCP.Checked)
                        ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.TCP.ToString());

                    ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
                    ConfigSettings.ChangeNode("BaudRate", cboBaudRate.Items[cboBaudRate.SelectedIndex].ToString());
                    ConfigSettings.ChangeNode("InitialBaudRate", cboInitialbaudRate.Items[cboInitialbaudRate.SelectedIndex].ToString());

                    string firmwareType = string.Empty;
                    if (chk96DLMS.Checked)
                        firmwareType = ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "DLMS96008N13P1P"))).ToString();
                    if (chk300IEC.Checked)
                        firmwareType = string.Concat(firmwareType, ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "NONDLMS3007E13P1P"))).ToString());
                    if (chk96IEC.Checked)
                        firmwareType = string.Concat(firmwareType, ((int)((MeterFirmwareType)Enum.Parse(typeof(MeterFirmwareType), "NONDLMS96008N11P"))).ToString());

                    ConfigSettings.ChangeNode("MeterFirmwareType", firmwareType.Trim());
                    ConfigSettings.ChangeNode("GSMConnected", "True");
                    this.StatusMessage = "Settings Saved Successfully.";
                }
                else
                {
                    MessageBox.Show("Please select communication port.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                ConfigSettings.ChangeNode("CommunicationType", "Remote");
                ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString());
                this.StatusMessage = "Settings Saved Successfully.";
            }
        }
        #endregion

        public enum MeterFirmwareType
        {
            //9600, 8, N, 1, (3P/1P DLMS)
            DLMS96008N13P1P = 1,
            //300, 7, E, 1 (3P/1P NONDLMS)
            NONDLMS3007E13P1P,
            //9600, 8, N, 1, (1P NONDLMS)
            NONDLMS96008N11P,
        }
        #region Private Methods

        /// <summary>
        /// Binds the default communication type settings
        /// </summary>
        private void BindDefaultSettings()
        {
            string channelType = ConfigSettings.GetValue("CommunicationType");
            if (channelType == CABCommunication.PhysicalLayer.ChannelType.Direct.ToString())
            {
                rdDirect.Checked = true;
            }
            else
            {
                rbtRemote.Checked = true;
            }
        }

        /// <summary>
        /// Binding COM ports to BCS/Scheduling ports
        /// </summary>
        private void BindPorts()
        {
            //bool isMTMType = false;
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
            areMultiplePortsPresent = false;
            if (PortNames.Length > 1)
            {
                areMultiplePortsPresent = true;
            }
            for (int i = 0; i < PortNames.Length; i++)
            {
                if (areMultiplePortsPresent)
                {
                    if (("," + strGSMModemPorts + ",").Contains("," + lstAllPorts[i] + ","))
                    {
                        // dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], true, false);
                    }
                    else if (strCMRIPort.Equals(lstAllPorts[i]))
                    {
                        //dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], false, true);
                    }
                    else
                    {
                        //dgvPortUsageAssociation.Rows.Add(lstAllPorts[i], false, false);
                    }
                }
            }
            DefaultPortName = cboPort.Text;// = SerialPortSettings.Default.SerialPort;
        }

        /// <summary>
        /// Setting Multiple Port Value
        /// </summary>
        private void SetMultiplePortValue()
        {
            string s = SystemSettings.USE_MULTIPLE_PORTS.ToString();
            if (rdDirect.Checked)
            {
                objSystemSettings.UpdateSetting(s, "0");
            }
        }

        /// <summary>
        /// Gets the list of saved ports
        /// </summary>
        /// <returns></returns>
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
        /// Saves ports
        /// </summary>
        /// <returns></returns>
        private bool SavePortMapping()
        {
            bool retVal = false;
            string strModemPorts = string.Empty, strCMRIPort = string.Empty;
            if (ValidatePortMapping(out strModemPorts, out strCMRIPort))
            {
                if (!string.IsNullOrEmpty(strModemPorts))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.GSM_COM_PORTS, strModemPorts);
                    ConfigSettings.ChangeNode("PortName", strModemPorts);
                }
                if (!string.IsNullOrEmpty(strCMRIPort))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.CMRI_COM_PORT, strCMRIPort);
                    if (rdDirect.Checked == true)
                        ConfigSettings.ChangeNode("PortName", strCMRIPort);
                }
                retVal = true;
            }
            return retVal;
        }

        private bool ValidatePortMapping(out string pModemPorts, out string pCMRIPort)
        {
            errpPortMapping.Clear();
            bool isModem = false, isCMRI = false;
            bool isAnyValueSet = false;
            string strModemPorts = string.Empty;
            string strCMRIPort = string.Empty;
            pModemPorts = pCMRIPort = string.Empty;

            strModemPorts = strModemPorts.TrimEnd(',');
            bool flag = false;

            if (isAnyValueSet &&
                !string.IsNullOrEmpty(strModemPorts))
            {
                pModemPorts = strModemPorts;
                flag = true;
            }
            if (isAnyValueSet &&
            !string.IsNullOrEmpty(strCMRIPort))
            {
                pCMRIPort = strCMRIPort;
                flag = true;
            }
            else if (string.IsNullOrEmpty(strModemPorts) &&
                string.IsNullOrEmpty(strCMRIPort))
            {
                errpPortMapping.Clear();
                flag = false;
            }
            return flag;
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
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SendCommandToModem(string command, SerialComm pSerialComm)", ex);
                throw;
            }
        }
        #endregion

        private void rbtRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDirect.Checked)
            {
                btnModemConfig.Enabled = false;
                rbtAuto.Checked = true;
                panelDirect.Visible = true;
                panelRemote.Visible = false;
            }
            if (rbtRemote.Checked)
            {
                btnModemConfig.Enabled = true;
                panelDirect.Visible = false;
                panelRemote.Visible = true;
                rbtAuto.Checked = false;
                cboBaudRate.SelectedIndex = 1;
                rbtGSM.Checked = true;
                if (isLoad == false)
                    SetManualDetails();
            }
        }

        private void rbtAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoad)
            {
                cboBaudRate.SelectedIndex = 1;
                cboInitialbaudRate.SelectedIndex = 0;
            }
            if (rbtAuto.Checked)
                SetAutoDetails();
            if (rbtManual.Checked)
            {
                SetManualDetails();
                chk96DLMS.Checked = true;
            }
            if (cboBaudRate.SelectedItem != null && cboBaudRate.SelectedIndex != 0)
                chk96DLMS.Text = string.Concat(cboBaudRate.SelectedItem, ", 8, N, 1 (3P/1P DLMS)");
            else
                chk96DLMS.Text = string.Concat("9600", ", 8, N, 1 (3P/1P DLMS)");
            chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");
        }

        private void rbtGSM_CheckedChanged(object sender, EventArgs e)
        {
            cboBaudRate.SelectedIndex = 1;
            SetManualDetails();
        }

        private void rbtPSTN_CheckedChanged(object sender, EventArgs e)
        {
            cboBaudRate.SelectedIndex = 1;
            SetManualDetails();
        }

        private void rbtGPRS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtGPRS.Checked)
            {
                MessageBox.Show("GPRS is applicable for DLMS meters only !!!");
                cboBaudRate.SelectedIndex = 1;
                cboInitialbaudRate.SelectedIndex = 0;
                //SetAutoDetails();
                SetGPRSDetails();
            }
        }

        private void cboBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtManual.Checked)
            {
                if (cboBaudRate.SelectedIndex != 0 && chk96DLMS.Checked)
                    chk96DLMS.Text = string.Concat(cboBaudRate.SelectedItem, ", 8, N, 1 (3P/1P DLMS)");
                chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");
            }
        }

        private void cboInitialbaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBaudRate.SelectedIndex != 0 && chk96DLMS.Checked)
                chk96DLMS.Text = string.Concat(cboBaudRate.SelectedItem, ", 8, N, 1 (3P/1P DLMS)");
            chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");
        }
        private void checkDetails()
        {
            if (chk96DLMS.Checked || chk96IEC.Checked)
            {
                cboBaudRate.Enabled = true;
            }
            else
            {
                cboBaudRate.Enabled = false;
            }
            if (chk300IEC.Checked)
            {
                cboInitialbaudRate.Enabled = true;
            }
            else
            {
                if (!isLoad)
                {
                    cboInitialbaudRate.SelectedIndex = 0;
                    chk300IEC.Text = string.Concat(cboInitialbaudRate.SelectedItem, ", 7, E, 1 (3P/1P NONDLMS)");
                    cboInitialbaudRate.Enabled = false;
                }
            }
        }
        private void SingleCheck()
        {
            if (rbtRemote.Checked)
            {
                if (chk300IEC.Checked)
                    chk96DLMS.Checked = chk96IEC.Checked = cboBaudRate.Enabled = false;
                if (chk96IEC.Checked)
                    chk96DLMS.Checked = chk300IEC.Checked = cboInitialbaudRate.Enabled = false;
                if (chk96DLMS.Checked)
                    chk300IEC.Checked = chk96IEC.Checked = cboInitialbaudRate.Enabled = false;
            }
        }
        private void chk96DLMS_CheckedChanged(object sender, EventArgs e)
        {
            if (chk96DLMS.Checked && cboBaudRate.SelectedIndex != 0)
                chk96DLMS.Text = string.Concat(cboBaudRate.SelectedItem, ", 8, N, 1 (3P/1P DLMS)");
            if (rbtRemote.Checked && isLoad == false && rbtGPRS.Checked == false)
                chk300IEC.Checked = chk96IEC.Checked = cboInitialbaudRate.Enabled = false;
            if (!chk96DLMS.Checked && !chk300IEC.Checked && !chk96IEC.Checked && isLoad == false)
                chk96DLMS.Checked = true;
            else
                checkDetails();
        }
        private void chk300IEC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtRemote.Checked && isLoad == false && rbtGPRS.Checked == false)
                chk96DLMS.Checked = chk96IEC.Checked = cboBaudRate.Enabled = false;
            if (!chk96DLMS.Checked && !chk300IEC.Checked && !chk96IEC.Checked && isLoad == false)
                chk300IEC.Checked = true;
            else
                checkDetails();
        }
        private void chk96IEC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtRemote.Checked && isLoad == false && rbtGPRS.Checked == false)
                chk96DLMS.Checked = chk300IEC.Checked = cboInitialbaudRate.Enabled = false;
            if (!chk96DLMS.Checked && !chk300IEC.Checked && !chk96IEC.Checked && isLoad == false)
                chk96IEC.Checked = true;
            else
                checkDetails();
        }
        private void lngbCancel_Click_1(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void btnModemConfig_Click(object sender, EventArgs e)
        {
            try
            {
                const int MAXGSMCOMSET = 4;
                bool flag = false;
                this.Enabled = false;

                //SettingForm.Enabled = false;
                //Application.DoEvents();
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                this.StatusMessage = "Modem configuration in Process... ";
                Application.DoEvents();
                for (int i = 1; i <= MAXGSMCOMSET; i++)
                {
                    flag = TestConnection(cboPort.Text, i);
                    if (flag)
                    {
                        if (chk96DLMS.Checked && i == 1)
                        {
                            this.StatusMessage = string.Empty;
                            Application.DoEvents();
                            this.StatusMessage = "Modem already configured.";
                            Application.DoEvents();
                            // MessageBox.Show("Modem already configured.");
                            flag = false;
                        }
                        else if (chk300IEC.Checked && i == 3)
                        {
                            this.StatusMessage = string.Empty;
                            Application.DoEvents();
                            this.StatusMessage = "Modem already configured.";
                            Application.DoEvents();
                            // MessageBox.Show("Modem already configured.");
                            flag = false;
                        }
                        else if (chk96IEC.Checked && i == 2)
                        {
                            this.StatusMessage = string.Empty;
                            Application.DoEvents();
                            this.StatusMessage = "Modem already configured.";
                            Application.DoEvents();
                            // MessageBox.Show("Modem already configured.");
                            flag = false;
                        }
                        break;
                    }
                }
                if (flag)
                {

                    SetModemConfiguration();

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnModemConfig_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Enabled = true;
            }

        }
        //***********Even & None Setting receive on chk box text ***************
        public String Even_None(string EvenNone)
        {

            if (EvenNone == "E" || EvenNone == "e")
            {
                EvenNone = "Even";
            }
            else if (EvenNone == "N" || EvenNone == "n")
            {
                EvenNone = "None";
            }
            return EvenNone;

        }


        private void SetModemConfiguration()
        {
            string strSendCmd = "AT+ICF=";
            string strSendCmdForBaudrate = "AT+IPR=";
            string sendCommandForSave = "AT&W\x0D";
            string Signalstrcommand = "AT+CSQ\x0D";
            string RecBuff = string.Empty;

            //int ibaudrate = 9600;
            int ibaudrate = 0;
            string chkboxval = string.Empty;
            //string[] serialportval;

            //***********set modem config on first check box click ***************
            try
            {
                if (chk96DLMS.Checked || chk300IEC.Checked || chk96IEC.Checked)
                {

                    chkboxval = chk96DLMS.Text;
                    serialportval = chkboxval.Split(',');

                    strSendCmd += "3,4\x0D";
                    strSendCmdForBaudrate += serialportval[0].Trim() + "\x0D";
                    ibaudrate = Convert.ToInt32(serialportval[0].Trim());
                    // channelDetails.DataBits = serialportval[1].Trim();
                    // channelDetails.Parity = Even_None(serialportval[2].Trim());

                }
                ////***********set modem config on Second check box click ***************
                //else if (chk300IEC.Checked)
                //{
                //    chkboxval = chk300IEC.Text;
                //    serialportval = chkboxval.Split(',');

                //    strSendCmd += "5,1\x0D";
                //    strSendCmdForBaudrate += serialportval[0].Trim() + "\x0D";
                //    ibaudrate = Convert.ToInt32(serialportval[0].Trim());
                //  //  channelDetails.DataBits = serialportval[1].Trim();
                //  //  channelDetails.Parity = Even_None(serialportval[2].Trim());
                //}
                ////***********set modem config on Third check box click ***************
                //else if (chk96IEC.Checked)
                //{
                //    chkboxval = chk96IEC.Text;
                //    serialportval = chkboxval.Split(',');

                //    strSendCmd += "3,4\x0D";
                //    strSendCmdForBaudrate += serialportval[0].Trim() + "\x0D";
                //    ibaudrate = Convert.ToInt32(serialportval[0].Trim());
                //   // channelDetails.DataBits = serialportval[1].Trim();
                //  //  channelDetails.Parity = Even_None(serialportval[2].Trim());
                //}
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SetModemConfiguration()", ex);
            }


            //if (chk96DLMS.Checked || chk96IEC.Checked)
            //{
            //    strSendCmd += "3,4\x0D";
            //    strSendCmdForBaudrate += "9600\x0D";
            //}
            //else
            //{
            //    strSendCmd += "5,1\x0D";
            //    strSendCmdForBaudrate += "300\x0D";
            //    ibaudrate = 300;
            //}
            // channelDetails.BaudRate = ibaudrate.ToString();
            // channelDetails.InitialBaudRate = ibaudrate.ToString(); ;
            Serial objSerial = new Serial(channelDetails);
            objSerial.OpenSession();

            byte[] data = Encoding.ASCII.GetBytes(strSendCmdForBaudrate);
            Result result = objSerial.SendGSMCommand(data, data.Length);
            objSerial.CloseSession();

            channelDetails.BaudRate = ibaudrate.ToString();
            channelDetails.InitialBaudRate = ibaudrate.ToString(); ;

            objSerial = new Serial(channelDetails);
            objSerial.OpenSession();
            data = Encoding.ASCII.GetBytes(strSendCmd);
            result = objSerial.SendGSMCommand(data, data.Length);
            objSerial.CloseSession();

            channelDetails.DataBits = serialportval[1].Trim();
            channelDetails.Parity = Even_None(serialportval[2].Trim());


            //if (ibaudrate == 300)
            //{
            //    channelDetails.DataBits = "7";
            //    channelDetails.Parity = "Even";
            //}
            //else
            //{
            //    channelDetails.DataBits = "8";
            //    channelDetails.Parity = "None";
            //}

            objSerial = new Serial(channelDetails);
            objSerial.OpenSession();
            data = Encoding.ASCII.GetBytes(sendCommandForSave);
            result = objSerial.SendGSMCommand(data, data.Length);
            objSerial.CloseSession();

            objSerial = new Serial(channelDetails);
            objSerial.OpenSession();
            data = Encoding.ASCII.GetBytes(Signalstrcommand);
            result = objSerial.SendGSMCommand(data, data.Length);
            objSerial.CloseSession();

            for (int buffIndex = 0; buffIndex < result.RecieveDataLength - 1; buffIndex++)
            {
                RecBuff = RecBuff + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
            }

            if (RecBuff != "")
            {
                signalstr = RecBuff.Substring(8, 2);

            }

            if (result == null)
            {
                MessageBox.Show("Modem configuration failed");
                return;
            }
            else
            {
                Application.DoEvents();
                this.StatusMessage = "Modem Configuration Successfully.";
                Application.DoEvents();
                MessageBox.Show("Current BaudRate : " + channelDetails.BaudRate + Environment.NewLine + "Parity Settings : " + channelDetails.DataBits + ", " + channelDetails.Parity + ", " + channelDetails.StopBits + " " + Environment.NewLine + "Signal Strength : " + signalstr + "  ");


            }


        }

        public bool TestConnection(string portName, int mFtype)
        {
            bool isConnected = false;
            try
            {
                GetChannelDetail(portName, mFtype);
                Serial objSerial = new Serial(channelDetails);
                byte[] data = { 0x41, 0x54, 0x0D };
                if (objSerial.OpenSession())
                {
                    Result result = objSerial.SendGSMCommand(data, data.Length);

                    if (result.RecieveDataBuffer.Contains((byte)'O') && result.RecieveDataBuffer.Contains((byte)'K'))
                    {
                        isConnected = true;// GetMeterData(strFileName, false, 0);                       
                    }
                    objSerial.CloseSession();
                }
                else
                {
                    isConnected = false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isConnected = false;
                logger.Log(LOGLEVELS.Error, "TestConnection(string portName, int mFtype)", ex);
            }
            return isConnected;
        }
        /// <summary>
        /// Gets the channel details
        /// </summary>
        /// <param name="isDLMS"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        /// 
        public bool TestModemReply(string portName, int mFtype)
        {
            bool isConnected = false;
            try
            {
                GetChannelDetail(portName, mFtype);
                Serial objSerial = new Serial(channelDetails);
                byte[] data = { 0x41, 0x54, 0x2B, 0x43, 0x53, 0x51, 0x0D };
                //   byte[] data = { 0x41, 0x54, 0x0D };
                string RecBuff = string.Empty;
                if (objSerial.OpenSession())
                {
                    Result result = objSerial.SendGSMCommand(data, data.Length);

                    for (int buffIndex = 0; buffIndex < result.RecieveDataLength - 1; buffIndex++)
                    {
                        RecBuff = RecBuff + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
                    }

                    if (RecBuff != "")
                    {
                        signalstr = RecBuff.Substring(8, 2);
                        isConnected = true;
                    }



                    objSerial.CloseSession();
                }
                else
                {
                    isConnected = false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isConnected = false;
                logger.Log(LOGLEVELS.Error, "TestModemReply(string portName, int mFtype)", ex);
            }
            return isConnected;
        }

        public ChannelDetail GetChannelDetail(string portName, int mFtype)
        {
            channelDetails = new ChannelDetail();
            channelDetails.ResponseTimeout = 6000;
            channelDetails.InterCharacterDelay = 5000;
            channelDetails.NumberOfRetry = 3;
            channelDetails.ChannelType = CABCommunication.PhysicalLayer.ChannelType.GSM;
            channelDetails.ComPort = portName;

            switch (mFtype)
            {
                case 1:
                    channelDetails.BaudRate = "9600";
                    channelDetails.InitialBaudRate = "9600";
                    channelDetails.Parity = "None"; ;
                    channelDetails.StopBits = "1";
                    channelDetails.DataBits = "8";
                    break;
                case 2:
                    channelDetails.BaudRate = "9600";
                    channelDetails.InitialBaudRate = "9600";
                    channelDetails.Parity = "Even"; ;
                    channelDetails.StopBits = "1";
                    channelDetails.DataBits = "7";
                    break;
                case 3:
                    channelDetails.BaudRate = "300";
                    channelDetails.InitialBaudRate = "300";
                    channelDetails.Parity = "Even"; ;
                    channelDetails.StopBits = "1";
                    channelDetails.DataBits = "7";
                    break;
                case 4:
                    channelDetails.BaudRate = "1200";
                    channelDetails.InitialBaudRate = "1200";
                    channelDetails.Parity = "Even";
                    channelDetails.StopBits = "1";
                    channelDetails.DataBits = "7";
                    break;
                default:
                    break;
            }

            return channelDetails;
        }

        private void btnModemInfo_Click(object sender, EventArgs e)
        {
            const int MAXGSMCOMSET = 4;
            bool flag = false;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.StatusMessage = "Getting modem information Please wait...";
            Application.DoEvents();
            this.Enabled = false;
            signalstr = "";

            try
            {
                for (int i = 1; i <= MAXGSMCOMSET; i++)
                {
                    flag = TestModemReply(cboPort.Text, i);

                    if (flag)
                    {
                        Application.DoEvents();
                        this.StatusMessage = string.Empty; //"Get Modem information Success.";
                        Application.DoEvents();
                        MessageBox.Show("Current BaudRate : " + channelDetails.BaudRate + Environment.NewLine + "Parity Settings : " + channelDetails.DataBits + ", " + channelDetails.Parity + ", " + channelDetails.StopBits + " " + Environment.NewLine + "Signal Strength : " + signalstr + "  ");


                        break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnModemInfo_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Enabled = true;
            }

        }

        private void rbtTCP_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTCP.Checked)
            {
                MessageBox.Show("TCP is applicable for DLMS meters only !!!");
                /*ParallelCounterForm pcf = new ParallelCounterForm();
                pcf.StartPosition = FormStartPosition.CenterParent;
                pcf.ShowDialog();*/
                cboBaudRate.SelectedIndex = 1;
                cboInitialbaudRate.SelectedIndex = 0;
                //SetAutoDetails();
                SetGPRSDetails();
            }
        }

    }
}
