using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.IECFramework;
using CAB.IECChannel;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using CAB.IECChannel.ReadOut;

namespace CAB.UI
{
    public partial class GSMStatus : MdiChildForm
    {
        private GSMCommunication communication = null;
        private ReadoutGSM readoutGSM = null;
        public GSMStatus()
        {
            readoutGSM = new ReadoutGSM();
            InitializeComponent();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }
        private void Connect()
        {
            string simNumber = txtSIMNumber.Text.Trim();
            long temNumber = 0;
            if (string.IsNullOrEmpty(simNumber))
            {
                this.StatusMessage = "SIM Number can't be empty.";
                Application.DoEvents();
                txtSIMNumber.Focus();
                return;
            }
            else if (simNumber.Length < 10 || simNumber.Length > 11 || !long.TryParse(simNumber, out temNumber))
            {
                this.StatusMessage = "Invalid SIM Number.";
                Application.DoEvents();
                txtSIMNumber.Focus();
                return;
            }
            else
            {
                ConfigInfo.SimNumber = txtSIMNumber.Text;
                this.StatusMessage = "Dialing.......";
                Application.DoEvents();
                ConfigInfo.ChannelType = ChannelType.GSM;
                communication = ChannelManager.GetChannel() as GSMCommunication;
                ConfigInfo.ChannelType = ChannelType.RS232;
                readoutGSM.Channel = communication;
                readoutGSM.SIMNumber = simNumber;
                GSMCommunicationStatus status = readoutGSM.ConnectToModem();
                
                if (status.Equals(GSMCommunicationStatus.ModemDisconnected))
                {
                    this.StatusMessage = "Modem Disconnected";
                    Application.DoEvents();

                }
                else if (status.Equals(GSMCommunicationStatus.NoCarrier))
                {
                    this.StatusMessage = "No Carrier found.";
                    Application.DoEvents();
                }
                else if (status.Equals(GSMCommunicationStatus.TimeOut))
                {
                    MessageBox.Show("Either modem is unavailable or already in connected state.\nCheck wire connection or click on Disconnect","BCS",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                }
                else if (status.Equals(GSMCommunicationStatus.TimeOut))
                {
                    this.StatusMessage = "Error in opening COM Port.";
                    Application.DoEvents();
                }
                else if (status.Equals(GSMCommunicationStatus.ModemNotResponding))
                {
                    this.StatusMessage = "Modem Not Responding.";
                    Application.DoEvents();
                }
                else if (status.Equals(GSMCommunicationStatus.SendCommandError))
                {
                    this.StatusMessage = "Send Command Error.";
                    Application.DoEvents();
                }
                else if (status.Equals(GSMCommunicationStatus.ErrorInOpeningPort))
                {
                    this.StatusMessage = "Error In Opening Port.";
                    Application.DoEvents();
                }
                else
                {
                    this.StatusMessage = "Connected";
                    Application.DoEvents();
                    btnDisconnect.Enabled = true;
                    btnConnect.Enabled = false;
                    ConfigSettings.ChangeNode("GSMConnected", "True");
                    ConfigInfo.ChannelType = ChannelType.GSM;
                    this.ConnectionTypeMessage = ChannelType.GSM.ToString();

                }
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }
        public bool Disconnect()
        {
            bool isDisconnected = false;
            string simNumber = txtSIMNumber.Text.Trim();
            long temNumber = 0;
            if (string.IsNullOrEmpty(simNumber))
            {
                this.StatusMessage = "SIM Number can't be empty.";
                Application.DoEvents();
                txtSIMNumber.Focus();
                isDisconnected = false;
            }
            else if (simNumber.Length < 10 || simNumber.Length > 11 || !long.TryParse(simNumber, out temNumber))
            {
                this.StatusMessage = "Invalid SIM Number.";
                Application.DoEvents();
                txtSIMNumber.Focus();
                isDisconnected = false;
            }
            else
            {
                try
                {
                    this.StatusMessage = "Dialing.......";
                    Application.DoEvents();
                    ConfigInfo.ChannelType = ChannelType.GSM;
                    communication = ChannelManager.GetChannel() as GSMCommunication;
                    readoutGSM.Channel = communication;
                    readoutGSM.SIMNumber = simNumber;
                    GSMCommunicationStatus status = readoutGSM.DisconnectModem();

                    if (status.Equals(GSMCommunicationStatus.ModemDisconnected))
                    {
                        isDisconnected = true;
                        this.StatusMessage = "Modem Disconnected";
                        Application.DoEvents();
                        btnDisconnect.Enabled = false;
                        btnConnect.Enabled = true;
                        ConfigSettings.ChangeNode("GSMConnected", "False");
                    }
                    else if (status.Equals(GSMCommunicationStatus.NoCarrier))
                    {
                        this.StatusMessage = "No Carrier found.";
                        Application.DoEvents();
                    }
                    else if (status.Equals(GSMCommunicationStatus.TimeOut))
                    {
                        this.StatusMessage = "Command Timeout.";
                        Application.DoEvents();
                    }
                    else if (status.Equals(GSMCommunicationStatus.PortOpenError))
                    {
                        this.StatusMessage = "Error in opening COM Port.";
                        Application.DoEvents();
                    }
                    else
                    {
                        this.StatusMessage = "";
                        Application.DoEvents();
                    }

                }
                catch (Exception)
                {
                    return isDisconnected;
                }
                finally
                {
                    ConfigSettings.ChangeNode("GSMConnected", "False");
                    this.ConnectionTypeMessage = "Physical";
                    ConfigInfo.ChannelType = ChannelType.RS232;
                }
            }
            return isDisconnected;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void GSMStatus_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void GSMStatus_Load(object sender, EventArgs e)
        {
            if (ConfigInfo.SimNumber != null)
            {
                txtSIMNumber.Text = ConfigInfo.SimNumber;
            }
        }
    }
}
