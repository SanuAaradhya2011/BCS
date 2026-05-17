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
using Hunt.EPIC.Logging;
#endregion
namespace CAB.UI
{
    /// <summary>
    /// Used for setting com port baud rate and communication option
    /// </summary>
    public partial class PortSettingForm : MdiChildForm, IChannel
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
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(PortSettingForm).ToString());
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
        public PortSettingForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods

        //public bool CheckModemExistOrAvailable(string portName)
        //{
        //    SerialComm objSerialComm = null;
        //    try
        //    {
        //        objSerialComm = new SerialComm();
        //        objSerialComm.InterchatracterDelay = SerialPortSettings.Default.InterframeTimeout;
        //        objSerialComm.SetSerialPortSettings(portName, "9600", "None", "8", "1", SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
        //        if (objSerialComm.OpenPort())
        //        {
        //            objSerialComm.CommandTimeout = 6000;
        //            objSerialComm.bCommType = 1;
        //            objSerialComm.InterchatracterDelay = 5000;
        //            objSerialComm.timeout = 5500;
        //            string Result = SendCommandToModem("AT", objSerialComm);
        //            if (Result == "\r\nOK\r\n")
        //            {
        //                objSerialComm.ClosePort();
        //                return true;
        //            }
        //            else
        //            {
        //                objSerialComm.ClosePort();
        //                return false;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (objSerialComm != null)
        //        {
        //            objSerialComm.ClosePort();
        //        }
        //    }
        //    return false;
        //}
        public bool TestConnection(string portName)
        {
            bool isConnected = false;
            Serial objSerial = new Serial(portName);
            byte[] data = { 0x41, 0x54, 0x0D };
            if (objSerial.OpenSession())
            {
                Result result = objSerial.SendGSMCommand(data, data.Length);
                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;// GetMeterData(strFileName, false, 0);
                    objSerial.CloseSession();
                }
            }
            else
            {
                isConnected = false;
            }
            return isConnected;
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
        private void PortSettingForm_Load(object sender, EventArgs e)
        {
            BindPorts();
            BindDefaultSettings();
                rdGPRS.Visible = true;
            SettingsBLL settingsBLL = new SettingsBLL();
            this.cboPort.DataSource = settingsBLL.GetPort().Tables[0];
            this.cboPort.DisplayMember = "DisplayMember";
            this.cboPort.ValueMember = "ValueMember";

            this.cboBaudRate.DataSource = settingsBLL.GetBaudRate().Tables[0];
            this.cboBaudRate.DisplayMember = "DisplayMember";
            this.cboBaudRate.ValueMember = "ValueMember";

            this.cboCommMode.DataSource = settingsBLL.GetCommunicationMode().Tables[0];
            this.cboCommMode.DisplayMember = "DisplayMember";
            this.cboCommMode.ValueMember = "ValueMember";
            int counter = 0;
            string value = ConfigSettings.GetValue("PortName");
            for (counter = 0; counter < cboPort.Items.Count; counter++)
            {
                cboPort.SelectedIndex = counter;
                if ((((System.Data.DataRowView)(cboPort.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            value = ConfigSettings.GetValue("BaudRate");
            for (counter = 0; counter < cboBaudRate.Items.Count; counter++)
            {
                cboBaudRate.SelectedIndex = counter;
                if ((((System.Data.DataRowView)(cboBaudRate.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            value = ConfigSettings.GetValue("CommunicationMode");
            for (counter = 0; counter < cboCommMode.Items.Count; counter++)
            {
                cboCommMode.SelectedIndex = counter;
                if ((((System.Data.DataRowView)(cboCommMode.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                    break;
            }
            if (rdDirect.Checked == true)
                chkIsNonDLMS.Visible = false;
            else if ((rdGSM.Checked == true) || (rdPSTN.Checked == true))
            {
                string type = ConfigSettings.GetValue("GSMMeterType");
                if (type == "DLMS")
                    chkIsNonDLMS.Checked = false;
                else
                    chkIsNonDLMS.Checked = true;
            }


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
            //UtilityEntity utilityEntity = UtilityDetails.GetPrimaryUtilityDetails();
            //utilityEntity == UtilityEntity.JUSCO

            if (cboCommMode.SelectedIndex == 1)
            {
                // Added for PVVNL utility 
                //if (UtilityDetails.utilityName != UtilityEntity.PVVNL)
                //{
                cboBaudRate.Text = "300";
                cboBaudRate.Enabled = false;
                //}
                ////else
                ////{
                //    cboBaudRate.Text = "9600";
                //    cboBaudRate.Enabled = false;
                ////}
            }
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

        private void rdDirect_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDirect.Checked)
            {
                groupBox1.Visible = true;
                panelMultiple.Visible = false;
                chkIsNonDLMS.Visible = false;
            }
        }

        private void rdGSM_CheckedChanged(object sender, EventArgs e)
        {
            if (rdGSM.Checked && areMultiplePortsPresent)
            {
                panelMultiple.Enabled = true;
                panelMultiple.Visible = true;
                groupBox1.Visible = false;
            }
            if (rdGSM.Checked && !areMultiplePortsPresent)
            {
                groupBox1.Visible = true;
                dgvPortUsageAssociation.Visible = false;
            }
            chkIsNonDLMS.Visible = true;
        }

        private void rdPSTN_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPSTN.Checked && areMultiplePortsPresent)
            {
                panelMultiple.Enabled = true;
                panelMultiple.Visible = true;
                groupBox1.Visible = false;
            }
            if (rdPSTN.Checked && !areMultiplePortsPresent)
            {
                groupBox1.Visible = true;
                dgvPortUsageAssociation.Visible = false;
            }
            chkIsNonDLMS.Visible = true;
        }

        private void rdGPRS_CheckedChanged(object sender, EventArgs e)
        {            
            if (rdGPRS.Checked)
            {
                MessageBox.Show("GPRS is applicable for DLMS meters only !!!");
                chkIsNonDLMS.Visible = false;
                groupBox1.Visible = false;
                panelMultiple.Visible = true;
                panelMultiple.Enabled = false;
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

        private void dgvPortUsageAssociation_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
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
                for (int i = 0; i < dgvPortUsageAssociation.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeModem.Name].Value)) &&
                        (Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeCMRI.Name].Value)))
                    {
                        MessageBox.Show("Cannot select the same port for both Direct and Scheduling!", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
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
                                if (TestConnection(strPortName))
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
                    if (strCaption == "Test Connections - SUCCESS")
                    {
                        MessageBox.Show("Please click on Save button to save the settings.", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    isConnectionTested = false;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Please check your port mapping!", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                if (!string.IsNullOrEmpty(strPortName))
                {
                    strPortName = "[" + strPortName + "] ";
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error in Testing Connection: " + strPortName + ex.Message, "Test Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectionTested = false;
                logger.Log(LOGLEVELS.Error, "btnTestConnection_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                if (!isConnectionTested)
                {
                    btnTestConnection.Enabled = true;
                }
            }
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
                ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.Direct.ToString());
                if (cboPort.SelectedItem != null)
                {
                    ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
                    ConfigSettings.ChangeNode("BaudRate", ((System.Data.DataRowView)(cboBaudRate.Items[cboBaudRate.SelectedIndex])).Row.ItemArray[0].ToString());
                    ConfigSettings.ChangeNode("CommunicationMode", ((System.Data.DataRowView)(cboCommMode.Items[cboCommMode.SelectedIndex])).Row.ItemArray[0].ToString());
                    ConfigSettings.ChangeNode("GSMConnected", "False");
                    this.StatusMessage = "Settings Saved Successfully.";
                }
                else
                {
                    MessageBox.Show("Please select communication port.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rdGSM.Checked || rdPSTN.Checked)
            {
                if (areMultiplePortsPresent)
                {
                    SetMultiplePortValue();
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
                else
                {
                    if (cboPort.SelectedItem != null)
                    {
                        ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
                        if (chkIsNonDLMS.Checked == true)
                        {                           
                            ConfigSettings.ChangeNode("BaudRate", "9600");
                            ConfigSettings.ChangeNode("CommunicationMode", "RS 232");
                        }
                        else
                        {                           
                            ConfigSettings.ChangeNode("BaudRate", ((System.Data.DataRowView)(cboBaudRate.Items[cboBaudRate.SelectedIndex])).Row.ItemArray[0].ToString());
                            ConfigSettings.ChangeNode("CommunicationMode", ((System.Data.DataRowView)(cboCommMode.Items[cboCommMode.SelectedIndex])).Row.ItemArray[0].ToString());
                        }
                        this.StatusMessage = "Settings Saved Successfully.";
                    }
                    else
                    {
                        MessageBox.Show("Please select communication port.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (rdGSM.Checked)
                {
                    ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.GSM.ToString());
                }
                else
                {
                    ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString());
                }
                if (chkIsNonDLMS.Checked == true)
                {
                    ConfigSettings.ChangeNode("GSMMeterType", "NONDLMS");
                    ConfigSettings.ChangeNode("GSMConnected", "True");
                }
                else
                {
                    ConfigSettings.ChangeNode("GSMMeterType", "DLMS");
                    ConfigSettings.ChangeNode("GSMConnected", "True");
                }
                //else
                //    ConfigSettings.ChangeNode("GSMMeterType", "DLMS");
            }
            else
            {
                ConfigSettings.ChangeNode("ChannelType", CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString());
                this.StatusMessage = "Settings Saved Successfully.";
            }
        }
        #endregion

        private void COMPortSet_lblCOMPort_Click(object sender, EventArgs e)
        {

        }
        #region Private Methods

        /// <summary>
        /// Binds the default communication type settings
        /// </summary>
        private void BindDefaultSettings()
        {
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (channelType == CABCommunication.PhysicalLayer.ChannelType.GSM.ToString())
            {
                rdGSM.Checked = true;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString())
            {
                rdPSTN.Checked = true;
            }
            else if (channelType == CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString())
            {
                rdGPRS.Checked = true;
            }
            else
            {
                rdDirect.Checked = true;
            }
        }

        /// <summary>
        /// Binding COM ports to BCS/Scheduling ports
        /// </summary>
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
            areMultiplePortsPresent = false;
            if (PortNames.Length > 1)
            {
                areMultiplePortsPresent = true;
            }
            //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("CommunicationType")))
            //{
            //    if (ConfigurationManager.AppSettings.Get("CommunicationType").ToString().ToLower() == "otm")
            //    {
            //        isMTMType = false;
            //    }
            //    else if (ConfigurationManager.AppSettings.Get("CommunicationType").ToString().ToLower() == "mtm")
            //    {
            //        isMTMType = true;
            //    }
            //    else
            //    {
            //        isMTMType = false;
            //    }
            //}
            for (int i = 0; i < PortNames.Length; i++)
            {
                //cmbAvailableSerialPort.Items.Add(PortNames[i]);

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

            //rdbSinglePort.CheckedChanged -= rdbSinglePort_CheckedChanged;
            //if (!areMultiplePortsPresent)
            //{ 
            //rdbMultiplePorts.Enabled = false;
            //rdGSM.Enabled = false;
            //rdPSTN.Enabled = false;
            //ToolTip ttMultiplePorts = new ToolTip();
            //ttMultiplePorts.SetToolTip(gbPort, "Multiple Ports option disabled\nas this computer doesn't have\nmore than one COM Port.");
            //}
            if (IsMultiplePortSelected && areMultiplePortsPresent && isMTMType)
            {
                //rdbMultiplePorts.Checked = true;
                dgvPortUsageAssociation.Visible = true;
                groupBox1.Visible = false;
                btnTestConnection.Visible = true;
                btnTestConnection.Enabled = false;
                isConnectionTested = true;
            }
            //else
            //{
            //    //rdbSinglePort.Checked = true;
            //    dgvPortUsageAssociation.Visible = false;
            //    grpBoxCOMPort.Visible = true;
            //    btnTestConnection.Visible = false;
            //}
            //rdbSinglePort.CheckedChanged += rdbSinglePort_CheckedChanged;

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
            if (rdGSM.Checked || rdPSTN.Checked)
            {
                objSystemSettings.UpdateSetting(s, "1");
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
            bool flag = false;
            //if (isAnyValueSet &&
            //    !string.IsNullOrEmpty(strModemPorts) &&
            //    !string.IsNullOrEmpty(strCMRIPort))
            //{
            //    pModemPorts = strModemPorts;
            //    pCMRIPort = strCMRIPort;
            //    return true;
            //}
            if (isAnyValueSet &&
                !string.IsNullOrEmpty(strModemPorts))
            {
                pModemPorts = strModemPorts;
               // ConfigSettings.ChangeNode("PortName", strModemPorts);
                flag = true;
            }
            if (isAnyValueSet &&
            !string.IsNullOrEmpty(strCMRIPort))
            {
                pCMRIPort = strCMRIPort;
                //if (!(rdGSM.Checked == true))
                //    ConfigSettings.ChangeNode("PortName", strCMRIPort);
                flag = true;
            }
            else if (string.IsNullOrEmpty(strModemPorts) &&
                string.IsNullOrEmpty(strCMRIPort))
            {
                errpPortMapping.Clear();
                errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for CMRI connection");
                flag = false;
            }
            //else if (!string.IsNullOrEmpty(strModemPorts) &&
            //    string.IsNullOrEmpty(strCMRIPort))
            //{
            //    errpPortMapping.Clear();
            //    errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for CMRI connection");
            //    flag = false;
            //}
            //else if (!string.IsNullOrEmpty(strCMRIPort) &&
            //    string.IsNullOrEmpty(strModemPorts))
            //{
            //    errpPortMapping.Clear();
            //    errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for GSM Modem connection");
            //    flag = false;
            //}
            //else
            //{
            //    errpPortMapping.Clear();
            //    errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for MODEM and CMRI connections");
            //    flag = false;
            //}
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
    }
}
