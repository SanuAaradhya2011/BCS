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
    public partial class SchedularSettingForm : MdiChildForm, IChannel
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
        private string cmriPorts = string.Empty;
        private string gsmComPorts = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SchedularSettingForm).ToString());
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
        public SchedularSettingForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods

        public bool TestConnection(string portName)
        {
            bool isConnected = false;
            try
            {
                Serial objSerial = new Serial(portName);
                byte[] data = { 0x41, 0x54, 0x0D };
                if (objSerial.OpenSession())
                {
                    Result result = objSerial.SendGSMCommand(data, data.Length);
                    if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
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
                logger.Log(LOGLEVELS.Error, "TestConnection(string portName)", ex);
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
            lblMsg.Text = string.Empty;
            BindPorts();
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
        //private void cboCommMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cboBaudRate.Enabled = true;
        //    //UtilityEntity utilityEntity = UtilityDetails.GetPrimaryUtilityDetails();
        //    //utilityEntity == UtilityEntity.JUSCO

        //    if (cboCommMode.SelectedIndex == 1)
        //    {
        //        // Added for PVVNL utility 
        //        //if (UtilityDetails.utilityName != UtilityEntity.PVVNL)
        //        //{
        //        cboBaudRate.Text = "300";
        //        cboBaudRate.Enabled = false;
        //        //}
        //        ////else
        //        ////{
        //        //    cboBaudRate.Text = "9600";
        //        //    cboBaudRate.Enabled = false;
        //        ////}
        //    }
        //}

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

        //private void rdDirect_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdDirect.Checked)
        //    {
        //        groupBox1.Visible = true;
        //        panelMultiple.Visible = false;
        //        chkIsNonDLMS.Visible = false;
        //    }
        //}



        private void dgvPortUsageAssociation_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!isPortAssociationChanged &&
              (dgvPortUsageAssociation.Columns[e.ColumnIndex].Name.Equals(colPortUsage.Name)))
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

                isConnectionTested = false;
            }
        }

        //private void btnTestConnection_Click(object sender, EventArgs e)
        //{
        //    string strModemPorts, strCMRIPort;
        //    string strPortName = string.Empty;
        //    string[] arrModemPorts;
        //    List<string> lstFailedPorts = new List<string>(), lstPassedPorts = new List<string>();
        //    try
        //    {
        //        btnTestConnection.Enabled = false;
        //        isPortAssociationChanged = false;
        //        PreviousPortAssociationColIndex = PreviousPortAssociationRowIndex = -1;
        //        Cursor.Current = Cursors.WaitCursor;
        //        for (int i = 0; i < dgvPortUsageAssociation.Rows.Count; i++)
        //        {
        //            if ((Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeModem.Name].Value)) &&
        //                (Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsageTypeCMRI.Name].Value)))
        //            {
        //                MessageBox.Show("Cannot select the same port for both Direct and Scheduling!", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //        }
        //        if (ValidatePortMapping(out strModemPorts, out strCMRIPort))
        //        {
        //            string strMessage = string.Empty, strCaption = string.Empty;
        //            MessageBoxIcon mbIcon = MessageBoxIcon.None;
        //            if (!string.IsNullOrEmpty(strModemPorts))
        //            {
        //                foreach (CABSerialPort objSerialPort in CABSerialPorts.ListOfSerialPorts)
        //                {
        //                    if (objSerialPort.IsResponding &&
        //                        ("," + strModemPorts + ",").Contains("," + objSerialPort.PortName + ","))
        //                    {
        //                        lstPassedPorts.Add(objSerialPort.PortName);
        //                    }
        //                }
        //                arrModemPorts = strModemPorts.Split(',');
        //                for (int i = 0; i < arrModemPorts.Length; i++)
        //                {
        //                    strPortName = arrModemPorts[i];
        //                    if (!lstPassedPorts.Contains(strPortName))
        //                    {
        //                        if (TestConnection(strPortName))
        //                        {
        //                            lstPassedPorts.Add(strPortName);
        //                            CABSerialPorts.SetPortRespondingStatus(strPortName, true);
        //                        }
        //                        else
        //                        {
        //                            lstFailedPorts.Add(strPortName);
        //CABSerialPorts.SetPortRespondingStatus(strPortName, false);
        //                        }
        //                    }
        //                }
        //                if (lstFailedPorts.Count == 0 &&
        //                    lstPassedPorts.Count > 0)
        //                {
        //                    strMessage = "All ports selected for GSM Modem connection are responding properly.";
        //                    strCaption = "Test Connections - SUCCESS";
        //                    mbIcon = MessageBoxIcon.Information;
        //                    isConnectionTested = true;
        //                }
        //                else if (lstFailedPorts.Count > 0)
        //                {
        //                    if (lstPassedPorts.Count == 0)
        //                    {
        //                        strMessage = "None of the ports selected for GSM Modem connection are responding.\n\nPlease check the connections.";
        //                        strCaption = "Test Connections - FAILED";
        //                        mbIcon = MessageBoxIcon.Error;
        //                        isConnectionTested = false;
        //                    }
        //                    else
        //                    {
        //                        strMessage = "The GSM Modem(s) on Port(s): [";
        //                        foreach (string PassedPort in lstPassedPorts)
        //                        {
        //                            strMessage += PassedPort + ",";
        //                        }
        //                        strMessage = strMessage.TrimEnd(',');
        //                        strMessage += "] are responding properly.";
        //                        strMessage += "\n\nBut, the Port(s): [";
        //                        foreach (string FailedPort in lstFailedPorts)
        //                        {
        //                            strMessage += FailedPort + ",";
        //                        }
        //                        strMessage = strMessage.TrimEnd(',');
        //                        strMessage += "] failed to respond.\nPlease check the connections on these ports.";
        //                        strCaption = "Test Connections - WARNING";
        //                        mbIcon = MessageBoxIcon.Warning;
        //                        isConnectionTested = false;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                strMessage = "No port selected for Modem connection.";
        //                strCaption = "Test Connections - WARNING";
        //                mbIcon = MessageBoxIcon.Warning;
        //                isConnectionTested = false;
        //            }
        //            Cursor.Current = Cursors.Default;
        //            MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, mbIcon);
        //            if (strCaption == "Test Connections - SUCCESS")
        //            {
        //                MessageBox.Show("Please click on Save button to save the settings.", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //        else
        //        {
        //            isConnectionTested = false;
        //            Cursor.Current = Cursors.Default;
        //            MessageBox.Show("Please check your port mapping!", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!string.IsNullOrEmpty(strPortName))
        //        {
        //            strPortName = "[" + strPortName + "] ";
        //        }
        //        Cursor.Current = Cursors.Default;
        //        MessageBox.Show("Error in Testing Connection: " + strPortName + ex.Message, "Test Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        isConnectionTested = false;
        //    }
        //    finally
        //    {
        //        if (!isConnectionTested)
        //        {
        //            btnTestConnection.Enabled = true;
        //        }
        //    }
        //}

        /// <summary>
        /// Save com port details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngbSave_Click(object sender, EventArgs e)
        {

            //if (areMultiplePortsPresent)
            //{
            //SetMultiplePortValue();
            //if (!isConnectionTested)
            //{
            //    MessageBox.Show("Please ensure all GSM Modem connections are working properly using the Test Connections button", "Connections not Tested", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (!SavePortMapping())
            {
                MessageBox.Show("Unable to Save port mapping.", "Invalid Port Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                this.StatusMessage = "Settings Saved Successfully.";
            //}
            //else
            //{
            //    //if (cboPort.SelectedItem != null)
            //    //{
            //    //    ConfigSettings.ChangeNode("PortName", ((System.Data.DataRowView)(cboPort.Items[cboPort.SelectedIndex])).Row.ItemArray[0].ToString());
            //    //    if (chkIsNonDLMS.Checked == true)
            //    //    {                           
            //    //        ConfigSettings.ChangeNode("BaudRate", "9600");
            //    //        ConfigSettings.ChangeNode("CommunicationMode", "RS 232");
            //    //    }
            //    //    else
            //    //    {                           
            //    //        ConfigSettings.ChangeNode("BaudRate", ((System.Data.DataRowView)(cboBaudRate.Items[cboBaudRate.SelectedIndex])).Row.ItemArray[0].ToString());
            //    //        ConfigSettings.ChangeNode("CommunicationMode", ((System.Data.DataRowView)(cboCommMode.Items[cboCommMode.SelectedIndex])).Row.ItemArray[0].ToString());
            //    //    }
            //    //    this.StatusMessage = "Settings Saved Successfully.";
            //    //}
            //    //else
            //    //{
            //    //    MessageBox.Show("Please select communication port.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //}
            //}

        }

        private string CheckPortStatus()
        {
            bool isChecked = false;
            string strModemPorts = string.Empty;
            for (int i = 0; i < dgvPortUsageAssociation.Rows.Count; i++)
            {
                isChecked = Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsage.Name].Value);
                if (isChecked)
                {
                    strModemPorts += dgvPortUsageAssociation.Rows[i].Cells[colPortName.Name].Value.ToString() + ",";
                }
            }
            strModemPorts = strModemPorts.TrimEnd(',');

            if (strModemPorts != string.Empty)
            {
                string[] arrModemPorts = strModemPorts.Split(',');
                strModemPorts = string.Empty;
                for (int i = 0; i < arrModemPorts.Length; i++)
                {
                    if (!TestConnection(arrModemPorts[i]))
                    {
                        strModemPorts += arrModemPorts[i] + ",";
                    }
                }
                if (strModemPorts != string.Empty)
                    strModemPorts = strModemPorts.TrimEnd(',');
            }
            return strModemPorts;
        }


        #endregion
        #region Private Methods

        /// <summary>
        /// Binds the default communication type settings
        /// </summary>


        /// <summary>
        /// Binding COM ports to BCS/Scheduling ports
        /// </summary>
        private void BindPorts()
        {
            try
            {
                SerialComm objSerialComm = new SerialComm();
                string[] PortNames = objSerialComm.GetAvailablePorts();
                Array.Reverse(PortNames);
                List<string> lstAllPorts = null;
                this.StatusMessage = string.Empty;
                if (PortNames.Length > 0)
                {
                    lstAllPorts = new List<string>(PortNames);
                    lstAllPorts.Sort();
                }
                string message = string.Empty;
                int sNo = 0;
                for (int i = 0; i < lstAllPorts.Count; i++)
                {
                    this.StatusMessage = "Checking status of " + lstAllPorts[i] + " port.";
                    Application.DoEvents();
                    sNo = sNo + 1;
                    if (TestConnection(lstAllPorts[i]))
                    {
                        if (ConfigSettings.GetValue("PortName").ToUpper() == lstAllPorts[i].ToString().ToUpper())
                        {
                            cmriPorts = lstAllPorts[i];
                            lblMsg.Text = "* " + lstAllPorts[i] + " is already selected for direct/remote communication.";
                        }
                        dgvPortUsageAssociation.Rows.Add(sNo, lstAllPorts[i], true);
                    }
                    else
                    {
                        sNo = sNo - 1;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BindPorts()", ex);
            }
        }

        ///// <summary>
        ///// Gets the list of saved ports
        ///// </summary>
        ///// <returns></returns>
        //private IList<CABSerialPort> GetSavedSerialPorts()
        //{
        //    string strGSMModemPorts = objSystemSettings.GetSettingValue(SystemSettings.GSM_COM_PORTS);
        //    string strCMRIPort = objSystemSettings.GetSettingValue(SystemSettings.CMRI_COM_PORT);
        //    List<CABSerialPort> lstSerialPorts = new List<CABSerialPort>();
        //    if (!string.IsNullOrEmpty(strGSMModemPorts) ||
        //        !string.IsNullOrEmpty(strCMRIPort))
        //    {
        //        List<string> lstAllPorts = new List<string>(strGSMModemPorts.CommaSplit());
        //        lstAllPorts.Add(strCMRIPort);
        //        lstAllPorts.Sort();
        //        for (int i = 0; i < lstAllPorts.Count; i++)
        //        {
        //            CABSerialPort objCABSerialPort = new CABSerialPort();
        //            objCABSerialPort.PortName = lstAllPorts[i];
        //            objCABSerialPort.IsResponding = true;
        //            lstSerialPorts.Add(objCABSerialPort);
        //        }
        //    }
        //    return lstSerialPorts;
        //}

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
                }
                if (!string.IsNullOrEmpty(strCMRIPort))
                {
                    objSystemSettings.UpdateSetting(SystemSettings.CMRI_COM_PORT, strCMRIPort);
                    objSystemSettings.UpdateSetting(SystemSettings.COM_PORT, strCMRIPort);
                }
                retVal = true;
            }
            return retVal;
        }

        private bool ValidatePortMapping(out string pModemPorts, out string pCMRIPort)
        {
            errpPortMapping.Clear();
            bool isAnyValueSet = false;
            string strModemPorts = string.Empty;
            string strCMRIPort = string.Empty;
            pModemPorts = pCMRIPort = string.Empty;

            bool isChecked = false;
            string portName = string.Empty;
            for (int i = 0; i < dgvPortUsageAssociation.Rows.Count; i++)
            {
                isChecked = Convert.ToBoolean(dgvPortUsageAssociation.Rows[i].Cells[colPortUsage.Name].Value);
                portName = dgvPortUsageAssociation.Rows[i].Cells[colPortName.Name].Value.ToString();

                if (isChecked)
                {
                    isAnyValueSet = true;
                    if (cmriPorts != string.Empty && cmriPorts == portName)
                    {
                        strCMRIPort = portName;
                    }
                    else
                    {
                        strModemPorts += portName + ",";
                    }
                }
            }
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
                errpPortMapping.SetError(dgvPortUsageAssociation, "No port mapped for connection");
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
    }
}
