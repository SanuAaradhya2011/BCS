using System;
using System.Reflection;
using CAB.UI.Controls;
using CAB.IECChannel;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using System.Drawing;
using CAB.IECChannel.ReadOut;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using CAB.Contracts;
using CAB.IECChannel.Formatter;
using CAB.Entity;
using CAB.IECChannel.Programming;
using LTCTBLL;
using CAB.BLL;
namespace CAB.UI
{
    partial class MeterDataReadoutForm : MdiChildForm
    {
        private bool IsAborted = false;
        private IECChannelBase communications;
        private Command command;
        private ReadBase readOut;
        bool versionFlag = false;
        bool readMtrConfig = false;
        int totalDay = 0;
        bool isTNEB = false;
        private const string SIGNONFAILURE = "Sign-On failure";
        public MeterDataReadoutForm()
        {
            InitializeComponent();
            command = Command.GetInstance();
            if (!ConfigInfo.IsGSMConnected())
                communications = ChannelManager.GetChannel() as LocalCommunication;
            else
                communications = ChannelManager.GetChannel() as GSMCommunication;

           // btnAbort.Enabled = false;
            if ((UtilityDetails.UtilityName == UtilityEntity.TNEB1) || UtilityDetails.UtilityName == UtilityEntity.TNEB)
            {
                isTNEB = true;
            }

        }

        private void ButtonStatus()
        {
            btnRead.Enabled = true;
            btnCancel.Enabled = true;
            btnAbort.Enabled = false;
            this.Cursor = Cursors.Default;
            this.RightStatusMessage = string.Empty;
        }
        private int TotalCheck()
        {
            return 1;
        }
        private void EnbaleControls(bool flag)
        {
            grpReadoptions.Enabled = flag;
            if (chkLoadSurvey.Checked)
                grpLoadSurvey.Enabled = flag;
            tpTamperStatus.Enabled = flag;
            lgcRTCUpdate.Enabled = flag;
            tpPhasor.Enabled = flag;
            tpTransaction.Enabled = flag;
            tpFraudEnergy.Enabled = flag;
            if (readMtrConfig)
            {
                this.StatusMessage = "User Aborted.";
                Application.DoEvents();
            }
            readMtrConfig = false;

        }
        private int TotalCheckCount()
        {
            int count = 0;
            if (chkGeneral.Checked) count++;
            if (chkTamper.Checked) count++;
            if (chkLoadSurvey.Checked) count++;
            if (chkTransaction.Checked) count++;
            if (chkPhasor.Checked) count++;
            if (chkFraudEnergy.Checked) count++;
            if (chkDTMDaily.Checked) count++;
            if (chkMeterConfigurations.Checked) count++;
            return count;
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as LocalCommunication;
            }
            else
            {
                communications = ChannelManager.GetChannel() as GSMCommunication;
            }

            string readingDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {
                EnbaleControls(false);
                IsAborted = false;
                this.StatusMessage = string.Empty;
                Application.DoEvents();

                if (!versionFlag)
                {
                    this.StatusMessage = "Invalid Firmware version.";
                    Application.DoEvents();
                    EnbaleControls(true);
                    return;
                }
                if (chkLoadSurvey.Checked)
                {
                    if (cmoDTMdays.Text == "")
                    {
                        this.StatusMessage = "Please select the no. of days for Load Survey Readout";
                        ButtonStatus();
                        Application.DoEvents();
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                }
                IsAborted = false;
                btnAbort.Enabled = true;
                btnRead.Enabled = false;
                btnCancel.Enabled = false;
                string tamperData = string.Empty;
                string generalData = string.Empty;
                string loadSurveyData = string.Empty;
                string transactionData = string.Empty;
                string phasorData = string.Empty;
                string fraudEnergyData = string.Empty;
                string dTMDailyProfileData = string.Empty;
                string headerInfoData = string.Empty;
                string namePlateDetailData = string.Empty;
                string meterConfigurationData = string.Empty;
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                bool isEmptyData = false;

                # region General Data
                if (chkGeneral.Checked && !IsAborted)
                {
                    generalData = string.Empty;
                    this.RightStatusMessage = "Reading general/billing data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    readOut = new ReadoutGeneral();
                    readOut.OnChannelStatusChanged += new ReadoutGeneral.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    generalData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        ButtonStatus();
                        Application.DoEvents();
                        EnbaleControls(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted";
                        Application.DoEvents();
                        this.Cursor = Cursors.Default;
                        EnbaleControls(true);
                        return;
                    }
                    if (generalData.Trim().Length == 1)
                    {
                        ButtonStatus();
                        if (generalData.Length < 5)
                            generalData = string.Empty;
                        //this.StatusMessage = "Invalid Response from meter.";
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    if (!readOut.IsAborted && !readOut.IsCorruptedData)
                    {
                        if (!isEmptyData)
                        {
                            if (generalData == string.Empty)
                            {
                                this.StatusMessage = "General/Billing data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "General/Billing data read successfully.";
                                Application.DoEvents();
                            }
                        }
                        isEmptyData = false;
                    }
                }
                # endregion

                #region Tamper Data
                if (chkTamper.Checked && !IsAborted)
                {
                    this.RightStatusMessage = "Reading Tamper data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    tamperData = string.Empty;
                    readOut = new ReadoutTamper();
                    readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    tamperData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        ButtonStatus();
                        Application.DoEvents();
                        EnbaleControls(true);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted";
                        Application.DoEvents();
                        this.Cursor = Cursors.Default;
                        EnbaleControls(true);
                        return;
                    }
                    if (tamperData.Trim().Length <= 1)
                    {
                        ButtonStatus();
                        if (tamperData.Length < 5)
                            tamperData = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    if (!readOut.IsAborted && !readOut.IsCorruptedData)
                    {
                        if (!isEmptyData)
                        {
                            if (tamperData == string.Empty)
                            {
                                this.StatusMessage = "Tamper data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Tamper data read successfully.";
                                Application.DoEvents();
                            }
                        }
                        isEmptyData = false;
                    }
                }
                # endregion

                #region Transaction Data
                if (chkTransaction.Checked && !IsAborted)
                {
                    this.RightStatusMessage = "Reading Transaction data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    transactionData = string.Empty;
                    readOut = new ReadoutTransaction();
                    readOut.OnChannelStatusChanged += new ReadoutTransaction.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    transactionData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        ButtonStatus();
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted";
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty && transactionData.Length >= 313)
                    {
                        string response = readOut.MeterID(communications.ResponseSignOn);
                        string strtemptr = Convert.ToChar(4).ToString() + Convert.ToChar(1).ToString() + ReadoutConstant.REGISTER + response + "/" + readingDateTime;
                        transactionData = transactionData.Replace(ReadoutConstant.TRANSACTION, strtemptr);
                        transactionData = Convert.ToChar(1) + ReadoutConstant.TAMPER + response + "/" + readingDateTime + transactionData + Convert.ToChar(4);
                        //transactionData = transactionData.Replace("<RTC>", "");
                    }
                    else
                    {
                        ButtonStatus();
                        transactionData = string.Empty;
                        this.StatusMessage = "Invalid Response from meter.";
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    if (!isEmptyData)
                    {

                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {

                            if (transactionData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessage = "Transaction data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Transaction data read successfully.";
                                Application.DoEvents();
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Phasor Data
                if (chkPhasor.Checked && !IsAborted)
                {
                    phasorData = string.Empty;
                    this.RightStatusMessage = "Reading Phasor data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    readOut = new ReadoutPhasor();
                    readOut.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    phasorData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        this.Cursor = Cursors.Default;
                        ButtonStatus();
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted";
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (phasorData.Length >= 93)
                        phasorData = Convert.ToChar(1) + ReadoutConstant.PHASOR + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + phasorData + Convert.ToChar(4);
                    else
                    {
                        ButtonStatus();
                        phasorData = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (phasorData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessage = "Phasor data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Phasor data read successfully.";
                                Application.DoEvents();
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Fraud & Reverse Energy data
                if (chkFraudEnergy.Checked && !IsAborted)
                {
                    fraudEnergyData = string.Empty;
                    this.RightStatusMessage = "Reading Fraud Energy data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    readOut = new ReadoutFraudEnergy();
                    readOut.OnChannelStatusChanged += new ReadoutFraudEnergy.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    fraudEnergyData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        this.Cursor = Cursors.Default;
                        ButtonStatus();
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted"; this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (fraudEnergyData != string.Empty)
                        fraudEnergyData = string.Concat(Convert.ToChar(1), ReadoutConstant.MAGNETICINFLUENCE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, fraudEnergyData);
                    else
                    {
                        ButtonStatus();
                        fraudEnergyData = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    string reserveEnergy = readOut.ReverseEnergy();
                    if (reserveEnergy != "")
                        fraudEnergyData = string.Concat(fraudEnergyData, Convert.ToChar(1), reserveEnergy, Convert.ToChar(4));
                    else
                    {
                        ButtonStatus();
                        fraudEnergyData = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (fraudEnergyData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessage = "Fraud Energy data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Fraud Energy data read successfully.";
                                Application.DoEvents();
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Daily Profile Data
                if (chkDTMDaily.Checked && !IsAborted)
                {
                    this.RightStatusMessage = "Reading Daily Profile data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    readOut = new ReadoutDTMDailyProfile(isTNEB);
                    readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfile.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    string dtmParameterData = readOut.GetDTMParameterData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessage = SIGNONFAILURE;
                        this.Cursor = Cursors.Default;
                        ButtonStatus();
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessage = "User Aborted";
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        EnbaleControls(true);
                        return;
                    }
                    if (dtmParameterData.Trim() != string.Empty)
                    {
                        if (dtmParameterData.Length >= 27)
                        {
                            readOut.ReadingDateTime = readingDateTime;
                            dTMDailyProfileData = readOut.GetData();
                            if (dTMDailyProfileData.Trim() != string.Empty)
                            {
                                if (dTMDailyProfileData.Length >= 15)
                                    dTMDailyProfileData = string.Concat(Convert.ToChar(1), ReadoutConstant.DTMDAILYPROFILE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, dtmParameterData, dTMDailyProfileData, Convert.ToChar(4));
                                else
                                {
                                    ButtonStatus();
                                    dTMDailyProfileData = string.Empty;
                                    this.StatusMessage = "Daily Profile data not available in meter";
                                    Application.DoEvents();
                                    isEmptyData = true;
                                    if (TotalCheckCount() == 1)
                                    {
                                        readOut.IsAborted = true;
                                        communications.Command = command.BreakCommand;
                                        communications.SendCommand();
                                        communications.DelayExecution();
                                        communications.ClosePort();
                                        //  this.StatusMessage = "User Aborted.";
                                        this.RightStatusMessage = string.Empty;
                                        Application.DoEvents();
                                        btnRead.Enabled = true;
                                        btnCancel.Enabled = true;
                                        btnAbort.Enabled = true;
                                        Thread.Sleep(500);

                                        EnbaleControls(true);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ButtonStatus();
                        dtmParameterData = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            return;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (dTMDailyProfileData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessage = "Daily Profile data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Daily Profile data read successfully.";
                                Application.DoEvents();
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Load Survey Data
                if (chkLoadSurvey.Checked && !IsAborted)
                {
                    this.RightStatusMessage = "Reading Load Survey data.....";
                    this.StatusMessage = string.Empty;
                    Application.DoEvents();
                    readOut = new ReadoutDTMLoadSurvey();
                    readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
                    readOut.Channel = communications;
                    readOut.ReadingDateTime = readingDateTime;
                    if (cmoDTMdays.Text == "")
                    {
                        this.StatusMessage = "Please check number of days is not selected";
                        ButtonStatus();
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnbaleControls(true);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    if (!isEmptyData)
                    {
                        loadSurveyData = ((ReadoutDTMLoadSurvey)readOut).GetData(cmoDTMdays.Text, totalDay);
                        if (readOut.IsSignOnFailure)
                        {
                            this.StatusMessage = SIGNONFAILURE;
                            this.Cursor = Cursors.Default;
                            ButtonStatus();
                            Application.DoEvents();
                            EnbaleControls(true);
                            return;
                        }
                        if (readOut.IsAborted)
                        {
                            ButtonStatus();
                            this.StatusMessage = "User Aborted";
                            this.Cursor = Cursors.Default;
                            Application.DoEvents();
                            EnbaleControls(true);
                            return;
                        }

                        ChangeStatus(loadSurveyData, readOut.IsSignOnFailure);
                    }
                    else
                    {
                        this.StatusMessage = "Load Survey data not available in meter";
                        Application.DoEvents();

                    }
                }
                #endregion

                # region Meter Configuration Read
                string tmpresult = string.Empty;
                if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                {
                    if (chkMeterConfigurations.Checked && !IsAborted)
                    {//string.Concat(Convert.ToChar(1), "RD", communications.ResponseSignOn.Replace("\r\n", string.Empty), "/", this.ReadingDateTime, "/", fileInput, Convert.ToChar(4));
                        //string.Concat(Convert.ToChar(1), "CR", communications.ResponseSignOn.Replace("\r\n", string.Empty), "/", this.ReadingDateTime, "/", fileInput, Convert.ToChar(4));
                        meterConfigurationData = string.Concat(Convert.ToChar(1), "CR");
                        // meterConfigurationData += "<ReadingDateTime>" + readingDateTime + "</ReadingDateTime>";
                        this.RightStatusMessage = "Reading Meter Configuration data.....";
                        this.StatusMessage = string.Empty;
                        Application.DoEvents();
                        MeterConfigurations meterConfigurations = new MeterConfigurations();
                        ReadConfigurations readConfig = new ReadConfigurations();

                        Collection<MeterConfigurationConfigSection> configSection = new Collection<MeterConfigurationConfigSection>();
                        MeterConfigurationConfigSection mtrConfigSection = new MeterConfigurationConfigSection();

                        btnRead.Enabled = false;
                        btnCancel.Enabled = false;
                        btnAbort.Enabled = true;
                        string statusMsg = "";
                        if (statusMsg == "Timeout!")
                            return;


                        #region Read RTC
                        configSection.Add(XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("RTC")));
                        try
                        {
                            //if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(true)))
                            //{//  -->/RTC//290911101242
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }
                            this.RightStatusMessage = "Reading RTC";
                            Application.DoEvents();

                            RTCInformation rtcInformation = new RTCInformation();
                            string meterRTCData = string.Empty;
                            // LocalCommunication communications;
                            communications = ChannelManager.GetChannel() as LocalCommunication;
                            rtcInformation.Channel = communications;
                            //this.Cursor = Cursors.WaitCursor;

                            meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                            if (statusMsg == "Invalid RTC.")
                            {
                                this.StatusMessage = "Invalid RTC.";
                                Application.DoEvents();
                                this.Cursor = Cursors.Default;
                                ButtonStatus();
                                EnbaleControls(true);
                                readConfig.BreakCommunication();
                                return;
                            }
                            if(meterRTCData!=string.Empty)
                            {
                                meterConfigurationData += ((string[])meterRTCData.Split('|'))[0] + "/" + readingDateTime;
                                meterConfigurationData += "/RTC/" + (((string[])meterRTCData.Split('|'))[1]).Replace("\n", string.Empty);

                                //   tmpresult = meterConfigurations.ReadManufactureSpecificCommands(configSection, readConfig, new System.Collections.Generic.List<ReadResult>()).Replace("\r\n", string.Empty);
                                //  meterConfigurationData += ((string[])tmpresult.Split('|'))[0] + "/" + readingDateTime;
                                //    meterConfigurationData += "/RTC/" + (((string[])tmpresult.Split('|'))[1]).Replace("\n", string.Empty);
                                this.RightStatusMessage = "RTC Read Successfully...";
                                Application.DoEvents();
                            }
                            //}

                            //else
                            //{
                            //    this.StatusMessage = readConfig.StatusMessage;
                            //    this.Cursor = Cursors.Default;
                            //    ButtonStatus();
                            //    Application.DoEvents();
                            //    EnbaleControls(true);
                            //    return;
                            //}
                        }
                        catch (Exception ex)
                        {
                            this.StatusMessage = "RTC," + ex.Message;
                            Application.DoEvents();
                            EnbaleControls(true);
                        }
                        finally
                        {
                            readConfig.BreakCommunication();
                        }
                        #endregion

                        if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }
                        if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                        {

                            string meterID = string.Empty;
                            configSection = new Collection<MeterConfigurationConfigSection>();
                            FormatterConfigurations formatterConfigurations = new FormatterConfigurations();
                            string Read_output = "";
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region Read MDWithIP
                            this.RightStatusMessage = "Reading MD With IP...";
                            Application.DoEvents();
                            //-->/MD/(010130000201300000000000)
                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("MDWithIP"));
                                    Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += "/MD/" + ((string[])Read_output.Split('|'))[1];
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "MD With IP," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region KVArSelection
                            //-->/KV/(01)
                            this.RightStatusMessage = "Reading kVAh Selection...";
                            Application.DoEvents();

                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("kvahSelection"));
                                    Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += "/KV/" + ((string[])Read_output.Split('|'))[1];
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "kVAh Selection," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region Display Parameters
                            //Read Display Paramaters.
                            //-->/DP/(777704)
                            //-->(6A6B6C6D7476776E88898A33868384858B8C8E8D870F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D)y
                            //-->(3F4144454647484A4C4F5253545556571A1B58595C5D5E5F606162636465666768690102030405070950517C5A7E7F8081820A0B0C0D0E)
                            //-->(01020304050A0B0C0D0E0F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D3F4144454647484A4C4F52)
                            //-->(53545556571A1B58595C5D5E5F6061626A6465666768696A6B6C6D7476776E88898A33868384858B8C8E8D877C817E7F80638207095051)
                            //-->(787A7B79)
                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    this.RightStatusMessage = "Reading Display Parameters...";
                                    Application.DoEvents();
                                    meterConfigurationData += "/DP/";
                                    string dispData=meterConfigurations.GetDisplayParamatersConfiguration(readConfig, ref meterID);
                                    if (dispData == "Aborted" || IsAborted)
                                        { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }
                                    else
                                        meterConfigurationData += dispData;
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "Display Parameters," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region Read DailyLog
                            this.RightStatusMessage = "Reading Daily Log...";
                            Application.DoEvents();
                            //-->/DL/(09)
                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("DailyLog"));
                                    Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += "/DL/" + ((string[])Read_output.Split('|'))[1];
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "Daily Log," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region Read BillingReset
                            this.RightStatusMessage = "Reading Billing type...";
                            Application.DoEvents();
                            //-->/BT/(01160000)
                            //-->/BM/(01)
                            //-->/LOD/(01)
                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("ModeOfBilling"));
                                    Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += "/BT/" + ((string[])Read_output.Split('|'))[1];

                                    if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                                    {
                                        mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("BillingPeriod"));
                                        Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                        meterConfigurationData += "/BM/" + ((string[])Read_output.Split('|'))[1];
                                    }

                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("ResetLockOutDays"));
                                    Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += "/LOD/" + ((string[])Read_output.Split('|'))[1];
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "Billing Reset," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            #region Read TOD.
                            try
                            {
                                if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                                {
                                    this.RightStatusMessage = "Reading TOD...";
                                    Application.DoEvents();
                                    mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("TOD"));
                                    tmpresult += readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                    meterConfigurationData += ((string[])tmpresult.Split('|'))[1];
                                    Application.DoEvents();
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    this.Cursor = Cursors.Default;
                                    ButtonStatus();
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                    readConfig.BreakCommunication();
                                    return;
                                }
                                readConfig.BreakCommunication();
                            }
                            catch (Exception ex)
                            {
                                readConfig.BreakCommunication();
                                this.StatusMessage = "TOD," + ex.Message;
                                Application.DoEvents();
                                EnbaleControls(true);
                            }
                            #endregion
                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }

                            # region ReadRS232LockUnlock
                            if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                            {
                                this.RightStatusMessage = "Reading RS232 Lock Unlock...";
                                Application.DoEvents();

                                try
                                {
                                    if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(true)))
                                    {
                                        mtrConfigSection = XMLLoader.GetConfigSection(meterConfigurations.GetConfigParameters("LockUnlockRS232"));
                                        Read_output = readConfig.ReadMeterConfigurations(mtrConfigSection, ref statusMsg);
                                        meterConfigurationData += "/RS232/" + ((string[])Read_output.Split('|'))[1];
                                    }
                                    else
                                    {
                                        this.StatusMessage = readConfig.StatusMessage;
                                        this.Cursor = Cursors.Default;
                                        ButtonStatus();
                                        Application.DoEvents();
                                        EnbaleControls(true);
                                        readConfig.BreakCommunication();
                                        return;
                                    }
                                    readConfig.BreakCommunication();
                                }
                                catch (Exception ex)
                                {
                                    readConfig.BreakCommunication();
                                    this.StatusMessage = "RS232 Lock Unlock," + ex.Message;
                                    Application.DoEvents();
                                    EnbaleControls(true);
                                }
                            }
                            # endregion

                            if (IsAborted) { readMtrConfig = true; EnbaleControls(true); btnRead.Enabled = true; btnCancel.Enabled = true; return; }
                        }
                        else
                        {
                            this.StatusMessage = readConfig.StatusMessage;
                            this.Cursor = Cursors.Default;
                            ButtonStatus();
                            Application.DoEvents();
                            EnbaleControls(true);
                            readConfig.BreakCommunication();
                            return;
                        }
                        // btnRead.Enabled = true;
                        btnAbort.Enabled = true;
                    }
                }
                # endregion

               

                if (loadSurveyData.Length <= 5)
                    loadSurveyData = string.Empty;
                if ((UtilityDetails.UtilityName == UtilityEntity.TNEB) || (UtilityDetails.UtilityName == UtilityEntity.TNEB1))
                {
                    if ((chkGeneral.Checked || chkTamper.Checked || chkLoadSurvey.Checked || chkFraudEnergy.Checked || chkDTMDaily.Checked || chkPhasor.Checked || chkTransaction.Checked || chkMeterConfigurations.Checked) && !IsAborted)
                    {
                        #region Header Info Data
                        if (UtilityDetails.UtilityName != UtilityEntity.UGVCL && UtilityDetails.UtilityName != UtilityEntity.PVVNL && UtilityDetails.UtilityName != UtilityEntity.JDVVNL)
                        {
                            this.RightStatusMessage = "Reading Header Info Data.....";
                            this.StatusMessage = string.Empty;
                            Application.DoEvents();
                            ReadoutMeterHeaderInfo readoutHeaderInfo = new ReadoutMeterHeaderInfo();
                            readoutHeaderInfo.OnChannelStatusChanged += new ReadBase.ChannelStatusChanged(Channel_OnStatusChanged);
                            readoutHeaderInfo.Channel = communications;
                            readoutHeaderInfo.ReadingDateTime = readingDateTime;
                            headerInfoData = readoutHeaderInfo.GetData();
                            if (readoutHeaderInfo.IsSignOnFailure)
                            {
                                this.StatusMessage = SIGNONFAILURE;
                                ButtonStatus();
                                Application.DoEvents();
                                EnbaleControls(true);
                                return;
                            }
                            if (readoutHeaderInfo.IsAborted)
                            {
                                ButtonStatus();
                                this.StatusMessage = "User Aborted";
                                Application.DoEvents();
                                EnbaleControls(true);
                                return;
                            }
                            if (headerInfoData == string.Empty)
                            {
                                this.StatusMessage = "Header data not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Header data read successfully.";
                                Application.DoEvents();
                            }
                        }
                        #endregion

                        #region Name Plate Detail

                        if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                        {
                            this.RightStatusMessage = "Reading Name Plate Details.....";
                            this.StatusMessage = string.Empty;
                            Application.DoEvents();
                            ReadoutNamePlateDetail readoutNamePlateDetail = new ReadoutNamePlateDetail();
                            readoutNamePlateDetail.OnChannelStatusChanged += new ReadBase.ChannelStatusChanged(Channel_OnStatusChanged);
                            readoutNamePlateDetail.Channel = communications;
                            readoutNamePlateDetail.ReadingDateTime = readingDateTime;
                            namePlateDetailData = readoutNamePlateDetail.GetData();
                            if (readoutNamePlateDetail.IsSignOnFailure)
                            {
                                this.StatusMessage = SIGNONFAILURE;
                                ButtonStatus();
                                Application.DoEvents();
                                EnbaleControls(true);
                                return;
                            }
                            if (readoutNamePlateDetail.IsAborted)
                            {
                                ButtonStatus();
                                this.StatusMessage = "User Aborted";
                                Application.DoEvents();
                                EnbaleControls(true);
                                return;
                            }
                            if (namePlateDetailData == string.Empty)
                            {
                                this.StatusMessage = "Name Plate Details not available in meter";
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Name Plate Details read successfully.";
                                Application.DoEvents();
                            }
                        }
                        #endregion
                    }
                }

                    if (generalData.Length <= 1)
                        generalData = string.Empty;

                    string fileText = string.Concat(headerInfoData, namePlateDetailData, generalData, tamperData, loadSurveyData, transactionData, phasorData, fraudEnergyData, dTMDailyProfileData, meterConfigurationData);
                
                if (!IsAborted && fileText.Trim() != string.Empty)
                {
                    string bcc = ReadoutCommon.CalculateFileBcc(fileText);
                    if (bcc != "")
                    {
                        fileText = string.Concat(fileText, bcc);
                        if (fileText != "")
                        {
                            this.RightStatusMessage = String.Empty;
                            this.StatusMessage = "Readout Successful";
                            Application.DoEvents();
                            SaveData(fileText);
                           
                        }
                    }
                    else
                    {
                        this.StatusMessage = "BCC not matched";
                        Application.DoEvents();
                    }
                }
                else
                {
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted";
                        Application.DoEvents();
                        EnbaleControls(true);
                    }
                    else if (TotalCheckCount() > 0)
                    {
                        this.StatusMessage = "Data not available in meter";
                        Application.DoEvents();
                    }
                }
                EnbaleControls(true);
                this.RightStatusMessage = String.Empty;
                //this.StatusMessage = String.Empty;
                Application.DoEvents();
                btnAbort.Enabled = false;
                btnRead.Enabled = true;
                btnCancel.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.RightStatusMessage = string.Empty;
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                this.Cursor = Cursors.Default;
                EnbaleControls(true);
            }
        }
        private void ChangeStatus(string data, bool SignOn)
        {
            this.RightStatusMessage = string.Empty;
            if (!SignOn)
            {
                if (data.Trim() != string.Empty)
                    this.StatusMessage = MessageConstant.GetText("M000068");
                else
                    this.StatusMessage = string.Empty;
            }
            else
                this.StatusMessage = MessageConstant.GetText("M000083");
        }
        private void ChangeStatus(string data)
        {
            this.RightStatusMessage = string.Empty;

            if (data.Trim() != string.Empty)
                this.StatusMessage = MessageConstant.GetText("M000068");
            else
                this.StatusMessage = string.Empty;
        }
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
                        this.StatusMessage = "File name can't be empty.";
                        this.RightStatusMessage = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessage = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessage = MessageConstant.GetText("M000047");
                return;
            }
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".CAB");
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
                fileText = ConfigInfo.EncryptFile(fileText);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                this.StatusMessage = MessageConstant.GetText("M000048");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "BCS");
            }

            bool IsUploaded = false;
            UploadFile uploadFile = new UploadFile();
            this.StatusMessage = "Uploading readout file..";
            btnAbort.Enabled = false;
            uploadFile.cmriID = "BCS";
            IsUploaded = uploadFile.Upload(filePath, uploadFile.GetContent(filePath), true);
            uploadFile.DeleteFile();  
            if (IsUploaded)
            {
                //this.OnListRefresh += new IsListRefresh();

                this.ListRefresh = true;
                this.RightStatusMessage = String.Empty;
                this.StatusMessage = "File Uploaded successfully.";
                //Application.DoEvents();
            }
            else
            {
                this.RightStatusMessage = String.Empty;
                this.StatusMessage = uploadFile.StatusMessage;
            }

        }
       
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.rbtnAll_CheckedChanged(this, null);
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
        }

        private void MeterDataReadoutForm_Load(object sender, EventArgs e)
        {
            BindLoadSurveyDays();
            this.rbtnAll_CheckedChanged(this, null);
            btnStopPhasor.Enabled = false;
            this.Text = "Meter Readout";
            readOut = new ReadoutFirmwareVersion();
            readOut.Channel = communications;
            //string versionData = readOut.GetFirmWareVersion();
            //double dataVer=0;
            //if (!string.IsNullOrEmpty(versionData))
            //{
            //    if (versionData.Substring(0, 1) == "/")
            //    {
            //        versionData = (Int32.Parse(versionData.Substring(1, 4), NumberStyles.HexNumber)).ToString();
            //        if (!string.IsNullOrEmpty(versionData))
            //            dataVer = double.Parse(versionData) / 100;
            //    }
            //}
            //if (dataVer >= 2.34 || dataVer >= 1.34 || dataVer==0)
            versionFlag = true;
            btnAbort.Enabled = false;

            chkMeterConfigurations.Visible = ShowMeterConfigurationReadOut();
            //chkMeterConfigurations.Checked = false;
           
        }

        /// <summary>
        /// Bind No of days in Load Survey Combo Box .
        /// </summary>
        private void BindLoadSurveyDays()
        {
            for (int index = 1; index <= 91; index++)
            {
                cmoDTMdays.Items.Add(index.ToString());

            }
            cmoDTMdays.SelectedIndex = 29;
        }

        private bool ShowMeterConfigurationReadOut()
        {
            switch (UtilityDetails.UtilityName)
            {
                case UtilityEntity.TNEB:
                    return true;
                case UtilityEntity.TNEB1:
                    return true;
                default :
                        return false;
            }
        }

        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                IsAborted = true;
                if (readOut != null)
                    readOut.IsAborted = true;
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                readOut = null;
                this.RightStatusMessage = string.Empty;
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                this.Close();
            }
            catch(Exception  ex)
            {
                
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            //Function changed on 29th feb 2012 as per the bug report
            btnCancel.Enabled = false;
            btnAbort.Enabled = false;
            Application.DoEvents();
            IsAborted = true;
            readOut.IsAborted = true;
            this.StatusMessage = "Aborting...";
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            communications.ClosePort();
            this.RightStatusMessage = string.Empty;
            Application.DoEvents();
           
            Thread.Sleep(500);
            if (!readMtrConfig)
            {
                btnRead.Enabled = true;
                
                this.StatusMessage = "User Aborted.";
                Application.DoEvents();
            }
            this.Cursor = Cursors.Default;
            btnCancel.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            try
            {
                button2.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                IsAborted = false;
                //lgcTamper.Data = dataSet;
                //lgcTamper.SetWidth("Tamper", 385);
                //lgcTamper.SetWidth("Status", 100);
                //lgcTamper.RefreshGrid();
                this.RightStatusMessage = "Reading Tamper data.....";
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                readOut = new ReadoutTamper();
                readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
                readOut.Channel = communications;
                readOut.IsAborted = IsAborted;
                string tamperData = readOut.GetInstantData();
                if (readOut.IsSignOnFailure)
                {
                    this.StatusMessage = SIGNONFAILURE;
                    button2.Enabled = true;
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                    return;
                }
                if (readOut.IsAborted)
                {
                    button2.Enabled = true;
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = string.Empty;
                    return;
                }
                if (tamperData.Length < 5)
                {
                    button2.Enabled = true;
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = string.Empty;
                    tamperData = string.Empty;
                    return;
                }

                if (!readOut.IsAborted && !readOut.IsCorruptedData)
                {
                    if (tamperData == string.Empty)
                    {
                        this.StatusMessage = "Tamper data not available in meter";
                        this.RightStatusMessage = string.Empty;
                        Application.DoEvents();
                    }
                    else
                    {
                        this.StatusMessage = "Tamper data read successfully.";
                        this.RightStatusMessage = string.Empty;
                        Application.DoEvents();
                    }
                }
                if (!string.IsNullOrEmpty(tamperData))
                {
                    DataTable table = ReadoutCommon.ConvertTamperData(tamperData);
                    if (table != null)
                    {
                        dataSet = new DataSet();
                        dataSet.Tables.Add(table);
                    }
                }
                lgcTamper.Data = dataSet;
                lgcTamper.SetWidth("Tamper", 385);
                lgcTamper.SetWidth("Status", 100);
                lgcTamper.RefreshGrid();
            }
            catch (Exception) { }
            finally
            {
                this.Cursor = Cursors.Default;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            ChangeStatus(string.Empty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.RightStatusMessage = MessageConstant.GetText("M000051");
                Application.DoEvents();
                ReadoutTransaction readoutTransaction = new ReadoutTransaction();
                readoutTransaction.OnChannelStatusChanged += new ReadoutTransaction.ChannelStatusChanged(Channel_OnStatusChanged);
                readoutTransaction.Channel = communications;
                readoutTransaction.IsAborted = IsAborted;
                string transactionData = readoutTransaction.GetData();
                if (readoutTransaction.IsSignOnFailure)
                {
                    lgcProgrammingUpdate.Data = null;
                    lblProgrammingCounter.Text = "0";
                    lblRTCUpdate.Text = "0";
                    lNGGRTCUpdate.Data = null;
                    lgcProgrammingUpdate.RefreshGrid();
                    lNGGRTCUpdate.RefreshGrid();
                    this.StatusMessage = SIGNONFAILURE;
                    button4.Enabled = true;
                    this.Cursor = Cursors.Default;
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                    return;
                }
                if (!string.IsNullOrEmpty(transactionData))
                {
                    lgcProgrammingUpdate.Data = ReadoutCommon.ProgrammingTimeStamp(transactionData);
                    lgcProgrammingUpdate.IsSorting = false;
                    lblProgrammingCounter.Text = Convert.ToInt32(transactionData.Substring(1, 2), 16).ToString();
                    int dataIndex = transactionData.IndexOf("Sep") + 4;
                    lblRTCUpdate.Text = Convert.ToInt32(transactionData.Substring(dataIndex, 2), 16).ToString();
                    lNGGRTCUpdate.Data = ReadoutCommon.RTCTimeStamp(transactionData);
                    lNGGRTCUpdate.IsSorting = false;
                    this.ChangeStatus(".");
                }
                else
                {
                    lgcProgrammingUpdate.Data = null;
                    lblProgrammingCounter.Text = "0";
                    lblRTCUpdate.Text = "0";
                    lNGGRTCUpdate.Data = null;
                    this.StatusMessage = "Data not available in meter.";
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                }
            }
            catch (Exception)
            {
                lgcProgrammingUpdate.Data = null;
                lblProgrammingCounter.Text = "0";
                lblRTCUpdate.Text = "0";
                lNGGRTCUpdate.Data = null;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                button4.Enabled = true;
            }
        }

        private void btnReadPhasor_Click(object sender, EventArgs e)
        {
            
            this.RightStatusMessage = "Reading Phasor Data";
            this.StatusMessage = "";
            Application.DoEvents();
            IsAborted = false;
            this.Cursor = Cursors.WaitCursor;
            ReadoutPhasor readoutPhasor = new ReadoutPhasor();
            readoutPhasor.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
            readoutPhasor.Channel = communications;
            string phasoreadoutPhasorrData = readoutPhasor.GetData();
            if (readoutPhasor.IsSignOnFailure)
            {
                this.StatusMessage = "Signon Fail";
                btnReadPhasor.Enabled = true;
                btnStopPhasor.Enabled = false;
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
                this.StatusMessage = "Data reading.....";
                do
                {
                    readoutPhasor.IsAborted = IsAborted;
                    phasoreadoutPhasorrData = readoutPhasor.GetData();
                    if (phasoreadoutPhasorrData != "")
                    {
                        if (ReadoutCommon.DisplayPhasor(phasoreadoutPhasorrData).Tables.Count > 0)
                        {
                            DataTable table = ReadoutCommon.DisplayPhasor(phasoreadoutPhasorrData).Tables[0];
                            PhasorEntity phasorEntity = GetPhasorEntity(table);
                            if (string.IsNullOrEmpty(phasorEntity.PhaseSequence) || phasorEntity.PhaseSequence.ToUpper() != "CORRECT")
                            {
                                lbkNoDataFound.Visible = true;
                                this.lbkNoDataFound.Text = "Phase Sequence is not correct. Phasor can not be shown";
                                lngPhasorDiagram.Visible = false;
                            }
                            else
                            {
                                lbkNoDataFound.Visible = false;
                                lngPhasorDiagram.Visible = true;
                                lngPhasorDiagram.PhasorData = phasorEntity;
                                lngPhasorDiagram.Refresh();
                                
                            }
                        }
                        lngPgrid.Data = ReadoutCommon.DisplayPhasor(phasoreadoutPhasorrData);
                        lngPgrid.SetWidth("Parameter", 180);
                        lngPgrid.SetWidth("Value", 85);
                        lngPgrid.IsSorting = false;
                    }
                    Application.DoEvents();
                    if (!readoutPhasor.IsPhasor)
                        break;
                    if (IsAborted)
                        break;
                    if (phasoreadoutPhasorrData == string.Empty)
                    {
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
            this.Cursor = Cursors.Default;
        }
        /// <summary>
        /// Converts a datatable to phasor entity
        /// </summary>
        /// <returns></returns>
        private PhasorEntity GetPhasorEntity(DataTable table)
        {
            PhasorEntity phasorEntity ;
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
            DataRow drRow = dtTable.NewRow();
            for (int i = 0; i < table.Rows.Count; i++) // Add column values
            {
                drRow[i] = table.Rows[i][1].ToString();

            }
            dtTable.Rows.Add(drRow);
            if (dtTable.Rows.Count > 0)
            {
                return phasorEntity = (PhasorEntity)new PhasorBLL().GetPhasorDataEntity(dtTable);               
            }
           return phasorEntity = null;
        }       

        private void btnCancelPhasor_Click(object sender, EventArgs e)
        {                      
            this.Close();
            ChangeStatus(string.Empty);
        }

        private void btnStopPhasor_Click(object sender, EventArgs e)
        {
            IsAborted = true;
            readOut.IsAborted = true;
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            communications.ClosePort();
            this.StatusMessage = "User Aborted.";
            lbkNoDataFound.Visible = false;
            this.RightStatusMessage = string.Empty;
            Application.DoEvents();
            Thread.Sleep(500);
            this.StatusMessage = "Phasor reading stopped.";
            Application.DoEvents();
            btnReadPhasor.Enabled = true;
            btnStopPhasor.Enabled = false;
            btnCancelPhasor.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            ChangeStatus(string.Empty);
        }

        private void btn_ReadReverseEnergy_Click(object sender, EventArgs e)
        {
            btn_ReadReverseEnergy.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.RightStatusMessage = MessageConstant.GetText("M000063");
                ReadoutFraudEnergy readoutFraudEnergy = new ReadoutFraudEnergy();
                readoutFraudEnergy.OnChannelStatusChanged += new ReadoutFraudEnergy.ChannelStatusChanged(Channel_OnStatusChanged);
                readoutFraudEnergy.Channel = communications;
                string reserveEnergy = readoutFraudEnergy.ReverseEnergy();
                if (readoutFraudEnergy.IsSignOnFailure)
                {
                    this.StatusMessage = "Signon Fail";
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                string fraudEnergyData = readoutFraudEnergy.GetData();
                if (readoutFraudEnergy.IsSignOnFailure)
                {
                    this.StatusMessage = "Signon Fail";
                    this.RightStatusMessage = string.Empty;
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (reserveEnergy.Trim() != string.Empty)
                {
                    //string val = string.Empty;
                    //int num = Convert.ToInt32(Convert.ToInt32(reserveEnergy.Substring(29, 2), 16));
                    //if (num == 1)
                    //    val = "0.0";
                    //else if (num == 2)
                    //    val = "0.00";
                    //else if (num == 3)
                    //    val = "0.000";
                    //else
                    //    val = "0";
                    //int index = 1;
                    //txt_RevKwh.Text = (Convert.ToDecimal(Convert.ToInt64(reserveEnergy.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    //txt_RevKvah.Text = (Convert.ToDecimal(Convert.ToInt64(reserveEnergy.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    const string regexFraud = @"(\(([\w\W]*?)\))";
                    MatchCollection matches = Regex.Matches(reserveEnergy, regexFraud, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                    string[] fraudEnergy = new string[matches.Count];
                    int count = 0;
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        fraudEnergy[count++] = groups["0"].Value;
                    }
                    if (Convert.ToDouble(fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1)) == 0)
                        txt_RevKwh.Text = "0";
                    else
                    {
                        txt_RevKwh.Text = fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0');
                        if (txt_RevKwh.Text.IndexOf('.') == 0) { txt_RevKwh.Text = "0" + txt_RevKwh.Text; }
                    }

                    if (Convert.ToDouble(fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1)) == 0)
                        txt_RevKvah.Text = "0";
                    else
                    {
                        txt_RevKvah.Text = fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0');
                        if (txt_RevKvah.Text.IndexOf('.') == 0) { txt_RevKvah.Text = "0" + txt_RevKvah.Text; }
                    }
                }
                if (fraudEnergyData.Trim() != string.Empty)
                {
                    //string val = string.Empty;
                    //int num = Convert.ToInt32(Convert.ToInt32(fraudEnergyData.Substring(57, 2), 16));
                    //if (num == 1)
                    //    val = "0.0";
                    //else if (num == 2)
                    //    val = "0.00";
                    //else if (num == 3)
                    //    val = "0.000";
                    //else
                    //    val = "0";
                    //int index = 1;
                    //txtFraudActive.Text = (Convert.ToDecimal(Convert.ToInt64(fraudEnergyData.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    //txtFraudReactiveLag.Text = (Convert.ToDecimal(Convert.ToInt64(fraudEnergyData.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    //txtFraudReactiveLead.Text = (Convert.ToDecimal(Convert.ToInt64(fraudEnergyData.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    //txtFraudApparent.Text = (Convert.ToDecimal(Convert.ToInt64(fraudEnergyData.Substring(index, 14), 16)) / 1000000).ToString(val);
                    //index += 14;
                    const string regexFraud = @"(\(([\w\W]*?)\))";
                    MatchCollection matches = Regex.Matches(fraudEnergyData, regexFraud, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                    string[] fraudEnergy = new string[matches.Count];
                    int count = 0;
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        fraudEnergy[count++] = groups["0"].Value;
                    }
                    if (Convert.ToDouble(fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1)) == 0)
                        txtFraudActive.Text = "0";
                    else
                    {
                        txtFraudActive.Text = fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0');
                        if (txtFraudActive.Text.IndexOf('.') == 0) { txtFraudActive.Text = "0" + txtFraudActive.Text; }
                    }

                    if (Convert.ToDouble(fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1)) == 0)
                        txtFraudReactiveLag.Text = "0";
                    else
                    {
                        txtFraudReactiveLag.Text = fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0');
                        if (txtFraudReactiveLag.Text.IndexOf('.') == 0) { txtFraudReactiveLag.Text = "0" + txtFraudReactiveLag.Text; }
                    }


                    if (Convert.ToDouble(fraudEnergy[2].Substring(1, fraudEnergy[2].IndexOf(')') - 1)) == 0)
                        txtFraudReactiveLead.Text = "0";
                    else
                    {
                        txtFraudReactiveLead.Text = fraudEnergy[2].Substring(1, fraudEnergy[2].IndexOf(')') - 1).TrimStart('0');
                        if (txtFraudReactiveLead.Text.IndexOf('.') == 0) { txtFraudReactiveLead.Text = "0" + txtFraudReactiveLead.Text; }
                    }

                    if (Convert.ToDouble(fraudEnergy[3].Substring(1, fraudEnergy[3].IndexOf(')') - 1)) == 0)
                        txtFraudApparent.Text = "0";
                    else
                    {
                        txtFraudApparent.Text = fraudEnergy[3].Substring(1, fraudEnergy[3].IndexOf(')') - 1).TrimStart('0');
                        if (txtFraudApparent.Text.IndexOf('.') == 0) { txtFraudApparent.Text = "0" + txtFraudApparent.Text; }
                    }

                }
                if (string.IsNullOrEmpty(reserveEnergy) && string.IsNullOrEmpty(fraudEnergyData))
                {
                    this.StatusMessage = "No data found";
                    this.RightStatusMessage = string.Empty;
                }
                else
                    this.ChangeStatus(".");
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btn_ReadReverseEnergy.Enabled = true;
            }
        }

      
        private void btn_Noofdays_Click(object sender, EventArgs e)
        {

            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as LocalCommunication;
            }
            else
            {
                communications = ChannelManager.GetChannel() as GSMCommunication;
            }
            if (communications == null)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            totalDay = 0;
            btnCancel.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            cmoDTMdays.Items.Clear();
            this.StatusMessage = "Reading Load Survey days";
            Application.DoEvents();
            ReadoutDTMLoadSurvey readoutDTMLoadSurvey = new ReadoutDTMLoadSurvey();
            readoutDTMLoadSurvey.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
            readoutDTMLoadSurvey.Channel = communications;
            string data = readoutDTMLoadSurvey.LoadDTMDay();
            if (readoutDTMLoadSurvey.IsSignOnFailure)
            {
                this.StatusMessage = SIGNONFAILURE;
                Application.DoEvents();
                ButtonStatus();
                this.Cursor = Cursors.Default;
                return;
            }
            if (string.IsNullOrEmpty(data))
            {
                this.StatusMessage = "Invalid response from meter.";
                Application.DoEvents();
                this.Cursor = Cursors.Default;
                return;
            }
            if (data.Length >= 5)
            {
                string noofdays = data.Substring(21, 2);
                totalDay = Convert.ToInt32(noofdays, 16);
                //int selectedConfig = Convert.ToInt32(ConfigInfo.GetLoadSurvey());
                //if (selectedConfig <= totalDay)
                //{
                //    for (int counter = 1; counter <= selectedConfig; counter++)
                //        cmoDTMdays.Items.Add(counter.ToString());
                //}
                //else
                //{
                for (int counter = 1; counter <= totalDay; counter++)
                    cmoDTMdays.Items.Add(counter.ToString());
                //}
            }
            else
                data = string.Empty;
            if (cmoDTMdays.Items.Count > 0)
            {
                cmoDTMdays.SelectedIndex = cmoDTMdays.Items.Count - 1;
                this.StatusMessage = "Days read successfully.";
                ButtonStatus();
                Application.DoEvents();
            }
            else
            {
                this.StatusMessage = "Load Survey data not available.";
                ButtonStatus();
                Application.DoEvents();
            }

            btnRead.Enabled = true;
            btnCancel.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            chkLoadSurvey.Checked = true;
            chkGeneral.Checked = true;
            chkTamper.Checked = true;
            chkTransaction.Checked = true;
            chkPhasor.Checked = true;
            chkFraudEnergy.Checked = true;
            chkDTMDaily.Checked = true;
            //if (btnRead.Enabled == false)
            //    btnAbort.Enabled = true;
            //else
            grpLoadSurvey.Enabled = true;

            //If Meter Configuration is visible then only make it checked.
            //This will not be visible other than TNEB
            if (chkMeterConfigurations.Visible)
            {
                chkMeterConfigurations.Checked = true;
            }

            btnAbort.Enabled = false;
            btnRead.Enabled = true;
            btnCancel.Enabled = false;
            //cmoDTMdays.Items.Clear();
        }
        private void rbtnPartial_CheckedChanged(object sender, EventArgs e)
        {
            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            chkGeneral.Checked = false;
            chkTamper.Checked = false;
            chkTransaction.Checked = false;
            chkPhasor.Checked = false;
            chkFraudEnergy.Checked = false;
            chkDTMDaily.Checked = false;
            btnAbort.Enabled = false;
            btnRead.Enabled = false;
            btnCancel.Enabled = false;
            chkLoadSurvey.Checked = false;
            // btnRead.Enabled = true;
            grpLoadSurvey.Enabled = false;
            chkMeterConfigurations.Checked = false;
            //cmoDTMdays.Items.Clear();
        }

        private void chkDTM_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPartial.Checked)
                rbtnPartial_CheckedChanged(this, null);
            if (rbtnAll.Checked)
                rbtnAll_CheckedChanged(this, null);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            ChangeStatus(string.Empty);
        }

        private void chkLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            chkGeneral_CheckedChanged(sender, e);
            if (chkLoadSurvey.Checked)
            {
                btnAbort.Enabled = btnCancel.Enabled = false;
                btnRead.Enabled = true;
                grpLoadSurvey.Enabled = true;
            }
            else
            {
                //btnAbort.Enabled = btnRead.Enabled = true;
                grpLoadSurvey.Enabled = false;
            }
        }

        private void chkGeneral_CheckedChanged(object sender, EventArgs e)
        {
            rbtnAll.CheckedChanged -= rbtnAll_CheckedChanged;
            rbtnPartial.CheckedChanged -= rbtnPartial_CheckedChanged;

            //If All visible check box are selected as true. Then make radio button all as selected.
            if (!isAllOptionSelected())
            {
                rbtnPartial.Checked = true;
                rbtnAll.Checked = false;
            }
            else
            {
                rbtnPartial.Checked = false;
                rbtnAll.Checked = true;
            }
            if ((chkMeterConfigurations.Checked || chkGeneral.Checked || chkFraudEnergy.Checked || chkLoadSurvey.Checked
                || chkTamper.Checked || chkTransaction.Checked || chkPhasor.Checked || chkDTMDaily.Checked))
            {
                btnRead.Enabled = true;
                btnCancel.Enabled = true;
            }
            else
            {
                btnRead.Enabled = false;
                btnCancel.Enabled = false;
            }

            rbtnAll.CheckedChanged += rbtnAll_CheckedChanged;
            rbtnPartial.CheckedChanged += rbtnPartial_CheckedChanged;

        }

        /// <summary>
        /// Returns true if all option selected. Else return false.
        /// </summary>
        /// <returns></returns>
        private bool isAllOptionSelected()
        {
            
           if(chkMeterConfigurations.Visible && !chkMeterConfigurations.Checked)
                    return false;
           if(chkGeneral.Visible && !chkGeneral.Checked )
                return false;
           if( chkFraudEnergy.Visible && !chkFraudEnergy.Checked )
                return false;
           if( chkLoadSurvey.Visible && !chkLoadSurvey.Checked)
                return false;
           if(chkTamper.Visible && !chkTamper.Checked )
                return false;
           if(chkTransaction.Visible && !chkTransaction.Checked )
                return false;
           if(chkPhasor.Visible && !chkPhasor.Checked )
                return false;
           if(chkDTMDaily.Visible && !chkDTMDaily.Checked)
                return false;
           return true;
        }

        private void lgcProgrammingUpdate_Load(object sender, EventArgs e)
        {

        }

        private void MeterDataReadoutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!grpReadoptions.Enabled)
            btnAbort_Click(sender, e); 
            this.StatusMessage = "";
            this.RightStatusMessage = "";
            this.Cursor = Cursors.Default;
        }
    }
}
