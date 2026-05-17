using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CAB.BLL;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using IEC.CAB.CHANNEL.Programming;
using CAB.Channel.ReadOut;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.IECChannel;
using CAB.IECChannel.Programming;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class IECMeterConfiguration : MdiChildForm
    {
        string validationMessage = "";
        DataGridViewCellStyle style;
        private CommunicationType commType;
        private IECChannelBase communications;
        private Communication communication;
        private Command command;
        private bool isMeterConnected = false;
        bool IsOffline = false;
        private string meterPswd="";
        private int ctRatio;
        private bool isValidTOU = true;
        string touFileName = "";
        string statusMsg = "";
        private Serializer serializer = null;
        public static MeterConfigSettings meterConfigSettings = null;
        public List<System.Enum> enumData;
        private List<System.Enum> listSelectedParams;
        private const string FOURTOU = "FOUR";
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private string gridTOUDay1_name = "gridTOUDay1";
        private string gridTOUDay2_name = "gridTOUDay2";
        private string gridTOUDay3_name = "gridTOUDay3";
        private string gridTOUDay4_name = "gridTOUDay4";
        private int isMeterType = 0; // Story - 347720 - To set the variable based on meter type is single phase DLMS
        private static object syncRoot = new object();
        private static string simNumber = string.Empty;
        private MeterMasterBLL meterMasterBLL = null;
        CABAppControl.DisplayParameterIEC objDisplayParameterIECConfig = null;
        bool Is10Zone8Slots = false;
        bool Is10Zone6Slot = false;
        int TOUZone = 0;
        int TOUSlot = 0;
        int rIndex = 0;
        int count = 0;
        int rcount = 0;
        int gIndex = 0;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(IECMeterConfiguration).ToString());
        #region Nested Types

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
        public IECMeterConfiguration(bool IsOnline)
        {
            InitializeComponent();
            LoadDisplayParameterTab();
            meterMasterBLL = new MeterMasterBLL();
            command = Command.GetInstance();
            communications = ChannelManager.GetChannel() as IECLocalCommunication;
            serializer = new Serializer();
            lock (syncRoot)
            {
                //if (meterConfigSettings == null)
                {
                    meterConfigSettings = (MeterConfigSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "MeterConfigSettings.xml"), typeof(MeterConfigSettings));
                }
            }
            // Code added by deep for GSM Configuration in IEC meter
            commType = GetCommuniactioType();
            if (commType == CommunicationType.DIRECT)
            {
                GsmCommPanel.Visible = false;
                tabControlMeterConfiguration.Width = 847;
                tabControlMeterConfiguration.Height = 426;
            }
            else
            {
            if (IsOnline)
            {
                    GsmCommPanel.Visible = true;
                    if (commType == CommunicationType.GPRS)
                    {
                        lngSimNumber.Text = "Modem IMEI Number: ";
                        grpSimNumber.Text = "IMEI Number";
                        txtBoxMeterSIM.MaxLength = 15;
                    }
                    FillMeterIdSerialNumber();
                }
                else
                {
                    GsmCommPanel.Visible = false;
                    tabControlMeterConfiguration.Width = 847;
                    tabControlMeterConfiguration.Height = 426;
                }

            }
            if (IsOnline)
            {
                //btnCancel.Location = btnCreateCfgFile.Location;
                btnCreateCfgFile.Visible = false;
                btnCancelMain.Location = btnCreateCfgFile.Location;
                //btnAbort.Location = btnUploadFile.Location;
                //btnUploadFile.Visible = false;
                 //IsOffline = false;
            }
            else
            {
                btnCancel.Location = btnUploadFile.Location;
                btnCreateCfgFile.Location = btnWrite.Location;
                btnUploadFile.Location = btnRead.Location;
                btnRead.Visible = false;
                btnWrite.Visible = false;
                //IsOffline = true;
            }
            //if (commType == CommunicationType.DIRECT)
            //{
            //    GsmCommPanel.Visible = false;
            //    tabControlMeterConfiguration.Width = 847;
            //    tabControlMeterConfiguration.Height = 426;
            //}
            
            //    if (IsOnline)
            //    {
            //        GsmCommPanel.Visible = true;
            //        btnCancelMain.Location = btnCreateCfgFile.Location;
            //        btnCreateCfgFile.Visible = false;
            //        FillMeterIdSerialNumber();

            //    }
            

            //else
            //{
            //    btnCreateCfgFile.Location = btnRead.Location;
            //    btnCancelMain.Location = btnWrite.Location;
            //    btnRead.Visible = false;
            //    btnWrite.Visible = false;
            //}
        }

        private void LoadDisplayParameterTab()
        {
            try
            {
                objDisplayParameterIECConfig = new CABAppControl.DisplayParameterIEC(); 
                objDisplayParameterIECConfig.FillDisplayParameters();
                objDisplayParameterIECConfig.Dock = DockStyle.Fill;
                tabPageDisplayParameter.Controls.Add(objDisplayParameterIECConfig);
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "LoadDisplayParameterTab()", ex);
            }
        }

        private void InitializeProgrammingValues()
        {
            //this.StatusMessage = "";
            ctRatio = 0;
            //meterPswd = string.Empty;          
            tabPageTOU.Enabled = false;
            grpResets.Enabled = false;
            Application.DoEvents();
        }
        // Story - 347720 - To set the variable based on meter type is single phase DLMS
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
            return commType;
        }
        private void FinalizeProgrammingValues()
        {            
            tabPageTOU.Enabled = true;
            grpResets.Enabled = true;
            Application.DoEvents();
        }

        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
        }


        private void btnUpdateRTC_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            RTCInformation rtcInformation = new RTCInformation();
            rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);

            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            string meterRTCData = string.Empty;
            DateTime rtc;
            try
            {
                InitializeProgrammingValues();
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                rtcInformation.RTCDateTime = dtPickerRTC.Value;
                rtcInformation.Channel = communications;

                meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                this.StatusMessage = statusMsg;
                if (statusMsg != "Invalid RTC." && meterRTCData.Length == 0)
                    return;
                meterRTCData = meterRTCData.Substring(meterRTCData.IndexOf("\n") + 3, 12);
                meterRTCData = meterRTCData.Substring(0, 2) + "/" + meterRTCData.Substring(2, 2) + "/" + meterRTCData.Substring(4, 2) + " " + meterRTCData.Substring(6, 2) + ":" + meterRTCData.Substring(8, 2) + ":" + meterRTCData.Substring(10, 2);

                rtc = ProgrammingCommon.GetDateWithTime(meterRTCData);
                if (statusMsg == "Invalid RTC.")
                {

                    rtcInformation.MeterPassword = meterPswd;
                    rtcInformation.SetRTC(meterPswd);
                }
                else if (rtcInformation.ValidateRTC(rtc))
                {
                    rtcInformation.MeterPassword = meterPswd;
                    rtcInformation.SetRTC(meterPswd);
                }
                else
                {
                    this.StatusMessage = "RTC backforce of more than 15 minutes is not allowed.";
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnUpdateRTC_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void btnSetCTRatio_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            CTRatioProgramming ctRatioProgramming = new CTRatioProgramming();
            ctRatioProgramming.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(true);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0 || ctRatio == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                ctRatioProgramming.CTRatio = ctRatio;
                ctRatioProgramming.MeterPassword = meterPswd;
                ctRatioProgramming.Channel = communications;
                ctRatioProgramming.SetCTRatio();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnSetCTRatio_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void meterPassword_OnValuesSubmission(string password, int CTRatio)
        {
            this.meterPswd = password;
            this.ctRatio = CTRatio;
        }

       

        /// <summary>
        /// 
        /// </summary>
        private void HideTabs()
        {
            //tabControlMeterConfiguration.TabPages.Remove(tabPageTOU);
            //tabControlMeterConfiguration.TabPages.Remove(tabPageDailyLog);
            tabControlMeterConfiguration.TabPages.Remove(tabPageDTMProgramming);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void HideTabs(List<System.Enum> list)
        {
            if (!list.Contains(ProfileId.FourTOU))
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageTOUSP);
                tabControlMeterConfiguration.TabPages.Remove(tabPageTOU);
            }
            if (!list.Contains(ProfileId.RTC))
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageRTC);
            }
            if (!list.Contains(ProfileId.DailyLog))
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageDailyLog);
            }
            if (!list.Contains(ProfileId.DTM))
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageDTMProgramming);
            }
            if (!list.Contains(ProfileId.MagneticTamperIcon))
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageMgtTamperIcon);
            }
            if (!list.Contains(ProfileId.BillingReset)) // Story - 354382 - To dispaly the tabs as according to profiles in the grid
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageBillingReset);
            }
            if (!list.Contains(ProfileId.DisplayParametersIEC)) 
            {
                tabControlMeterConfiguration.TabPages.Remove(tabPageDisplayParameter);
            }
        }

        private void LoadTabs()
        {
            foreach (ProfileId param in enumData)
            {
                switch (param)
                {

                    case ProfileId.FourTOU:
                        {
                            if (isMeterType == 1 || isMeterType == 2)
                            {
                                if (!tabControlMeterConfiguration.TabPages.Contains(tabPageTOUSP))
                                {
                                    tabControlMeterConfiguration.TabPages.Add(tabPageTOUSP);
                                }
                                if (tabControlMeterConfiguration.TabPages.Contains(tabPageTOU))
                                {
                                    tabControlMeterConfiguration.TabPages.Remove(tabPageTOU);
                                }
                            }
                            else
                            {
                                if (!tabControlMeterConfiguration.TabPages.Contains(tabPageTOU))
                                {
                                    tabControlMeterConfiguration.TabPages.Add(tabPageTOU);
                                }
                                if (tabControlMeterConfiguration.TabPages.Contains(tabPageTOUSP))
                                {
                                    tabControlMeterConfiguration.TabPages.Remove(tabPageTOUSP);
                                }
                            }
                            break;
                        }
                    case ProfileId.RTC:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageRTC))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageRTC);
                            }
                            break;
                        }
                    case ProfileId.BillingReset:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageBillingReset))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageBillingReset);
                            }
                            break;
                        }
                    case ProfileId.DailyLog:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageDailyLog))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageDailyLog);
                            }
                            break;
                        }
                    case ProfileId.DTM:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageDTMProgramming))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageDTMProgramming);
                            }
                            break;
                        }
                    case ProfileId.MagneticTamperIcon:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageMgtTamperIcon))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageMgtTamperIcon);
                            }
                            break;
                        }
                    case ProfileId.DIP:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabDIP))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabDIP);
                            }
                            break;
                        }
                    case ProfileId.BillingType:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabBillingType))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabBillingType);
                            }
                            break;
                        }
                    case ProfileId.DisplayParametersIEC:
                        {
                            if (!tabControlMeterConfiguration.TabPages.Contains(tabPageDisplayParameter))
                            {
                                tabControlMeterConfiguration.TabPages.Add(tabPageDisplayParameter);
                            }
                            break;
                        }
                    default: break;

                }
            }
        }

        private void btnReadRTC_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            RTCInformation rtcInformation = new RTCInformation();
            rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            string meterRTCData = string.Empty;
            DateTime rtc;
            try
            {
                InitializeProgrammingValues();
                rtcInformation.Channel = communications;
                this.Cursor = Cursors.WaitCursor;
                meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                if (meterRTCData.Length == 0)
                {
                    this.StatusMessage = statusMsg;
                    return;
                }
                meterRTCData = meterRTCData.Substring(meterRTCData.IndexOf("\n") + 3, 12);
                meterRTCData = meterRTCData.Substring(0, 2) + "/" + meterRTCData.Substring(2, 2) + "/" + meterRTCData.Substring(4, 2) + " " + meterRTCData.Substring(6, 2) + ":" + meterRTCData.Substring(8, 2) + ":" + meterRTCData.Substring(10, 2);
                if (!DateTime.TryParse(meterRTCData, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out rtc))
                    this.StatusMessage = "Invalid RTC";
                else
                {
                    rdBtnManualRTC.Checked = true;
                    dtPickerRTC.Value = rtc;
                    if (meterRTCData != "")
                        this.StatusMessage = "RTC read successfully.";
                    else
                        this.StatusMessage = statusMsg;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnReadRTC_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void btnResetBilling_Click(object sender, EventArgs e)
        {
            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as IECLocalCommunication;
            }
            else
            {
                communications = ChannelManager.GetChannel() as GSMCommunication;
            }

            this.StatusMessage = string.Empty;
            BillingInformation billingInformation = new BillingInformation();
            billingInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                billingInformation.MeterPassword = meterPswd;
                billingInformation.Channel = communications;
                billingInformation.ResetBilling();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnResetBilling_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void btnMagneticTamperReset_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            MagneticTamperInformation magneticTamperInformation = new MagneticTamperInformation();
            magneticTamperInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                magneticTamperInformation.MeterPassword = meterPswd;
                magneticTamperInformation.Channel = communications;
                magneticTamperInformation.ResetMagneticTamper();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnMagneticTamperReset_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void btnWriteLPRParameters_Click(object sender, EventArgs e)
        {
            DTMProgramming dtmProgramming = new DTMProgramming();
            dtmProgramming.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                if (ValidateLPRParameters() == false)
                {
                    return;
                }
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                dtmProgramming.MeterPassword = meterPswd;
                dtmProgramming.Channel = communications;
                dtmProgramming.HighLoadThreshold = Convert.ToInt32(txtBoxHighLoad.Text.Trim()).ToString("X");
                dtmProgramming.LowLoadThreshold = Convert.ToInt32(txtBoxLowLoad.Text.Trim()).ToString("X");
                dtmProgramming.TransformerRating = Convert.ToInt32(txtBoxTransformerRating.Text.Trim()).ToString("X");
                if (dtmProgramming.WriteLPRParameters())
                    this.StatusMessage = "DTM LPR parameters written successfully.";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnWriteLPRParameters_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private bool ValidateLPRParameters()
        {
            if (string.IsNullOrEmpty(txtBoxHighLoad.Text.Trim()))
            {
                this.StatusMessage = "High load value can not be left blank.";
                txtBoxHighLoad.Focus();
                return false;
            }
            else if (!ValidationProvider.ValidateData(txtBoxHighLoad.Text.Trim(), @"^(0{0,2}[1-9]|0?[1-9][0-9]|[1][0-9][0-9]|[2][0][0])$"))
            {
                this.StatusMessage = "Valid range for high load threshold is 1-200";
                txtBoxHighLoad.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtBoxLowLoad.Text.Trim()))
            {
                this.StatusMessage = "Low load value can not be left blank.";
                txtBoxLowLoad.Focus();
                return false;
            }
            else if (!ValidationProvider.ValidateData(txtBoxLowLoad.Text.Trim(), @"^(0{0,2}[0-9]|0?[1-9][0-9]|[1][0][0])$"))
            {
                this.StatusMessage = "Valid range for low load threshold is 0-100";
                txtBoxLowLoad.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtBoxTransformerRating.Text.Trim()))
            {
                this.StatusMessage = "Transformer rating can not be left blank.";
                txtBoxTransformerRating.Focus();
                return false;
            }
            else if (!ValidationProvider.ValidateData(txtBoxTransformerRating.Text.Trim(), @"^(0{0,2}[1-9]|0?[1-9][0-9]|[1-6][0-9][0-9]|[7][0][0])$"))
            {
                this.StatusMessage = "Valid range for transformer rating is 1-700";
                txtBoxTransformerRating.Focus();
                return false;
            }
            return true;
        }

        private void btnWriteDailyLog_Click(object sender, EventArgs e)
        {
            DTMProgramming dtmProgramming = new DTMProgramming();
            dtmProgramming.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                if (ValidateDailyParams() == false)
                    return;
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                dtmProgramming.MeterPassword = meterPswd;
                dtmProgramming.Channel = communications;
                dtmProgramming.DailyParamsValue = GetDailyLogCommand();
                this.Cursor = Cursors.WaitCursor;

                //2 march 2012: Command added to reset the Daily log data after configuring the parameters.
                //dtmProgramming.WriteDTMDailyLog();
                if (dtmProgramming.WriteDTMDailyLog())
                {
                    dtmProgramming.ResetDTMDailyLog();
                    this.statusMsg = dtmProgramming.StatusMessage;
                }
                else
                    this.statusMsg = dtmProgramming.StatusMessage;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "btnWriteDailyLog_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                //2 march 2012: ClosePort() added to close the port after communication
                communications.DelayExecution();
                communications.ClosePort();
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }

        }

        private bool ValidateDailyParams()
        {
            if (chkBoxMinAvgCurrent.Checked == false && chkBoxMaxAvgCurrent.Checked == false && chkBoxMinAvgVoltage.Checked == false && chkBoxMaxAvgVoltage.Checked == false && chkBoxDailyMD1.Checked == false && chkBoxDailyMD2.Checked == false && chkBoxDailyMD3.Checked == false && chkBoxKwh.Checked == false && chkBoxKvarhLag.Checked == false && chkBoxKvarhLead.Checked == false && chkBoxKVAh.Checked == false && chkBoxCumFundkWh.Checked == false)
            {
                this.StatusMessage = "Please select a parameter to configure.";
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GetDailyLogCommand()
        {
            //int dailyParamsVal = 0;
            //if (chkBoxKwh.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 0);
            //if (chkBoxKvarhLag.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 1);
            //if (chkBoxKvarhLead.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 2);
            //if (chkBoxKVAh.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 3);
            //if (chkBoxDailyMD1.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 4);
            //if (chkBoxDailyMD2.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 5);
            //if (chkBoxDailyMD3.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 6);
            //if (chkBoxMaxAvgVoltage.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 7);
            //dailyParamsVal = dailyParamsVal << 16;
            //if (chkBoxMinAvgVoltage.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 0);
            //if (chkBoxMaxAvgCurrent.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 1);
            //if (chkBoxMinAvgCurrent.Checked == true)
            //    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 2);
            ////if (chkBoxCumFundkWh.Checked == true)
            ////    dailyParamsVal = dailyParamsVal | (int)Math.Pow(2, 3);

            //return dailyParamsVal;


            string data1 = "", data2 = ""; ;
            if (chkBoxKwh.Checked == true)
                data1 = "1";
            else
                data1 = "0";
            if (chkBoxKvarhLag.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxKvarhLead.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxKVAh.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxDailyMD1.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxDailyMD2.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxDailyMD3.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;
            if (chkBoxMaxAvgVoltage.Checked == true)
                data1 = "1" + data1;
            else
                data1 = "0" + data1;

            while (data1.Length < 8) data1 = "0" + data1;
            data1 = (Convert.ToString(Convert.ToInt32(data1, 2), 16)).ToUpper();
            if (data1.Length < 2) data1 = "0" + data1;

            if (chkBoxMinAvgVoltage.Checked == true)
                data2 = "1";
            else
                data2 = "0";
            if (chkBoxMaxAvgCurrent.Checked == true)
                data2 = "1" + data2;
            else
                data2 = "0" + data2;
            if (chkBoxMinAvgCurrent.Checked == true)
                data2 = "1" + data2;
            else
                data2 = "0" + data2;
            if (chkBoxCumFundkWh.Checked == true)
                data2 = "1" + data2;
            else
                data2 = "0" + data2;

            while (data2.Length < 8) data2 = "0" + data2;
            data2 = (Convert.ToString(Convert.ToInt32(data2, 2), 16)).ToUpper();
            if (data2.Length < 2) data2 = "0" + data2;

            string result = data1 + data2;

            return result;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDailyLogCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLPRCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            //InitializeAllCheckboxes();
            if (chkAll.Checked == true)
            {
                //chkDefault.Checked = false;
                chkBoxKwh.Checked = true;
                chkBoxKvarhLag.Checked = true;
                chkBoxKvarhLead.Checked = true;
                chkBoxKVAh.Checked = true;
                chkBoxDailyMD1.Checked = true;
                chkBoxDailyMD2.Checked = true;
                //chkBoxDailyMD3.Checked = true;
                //chkBoxMaxAvgVoltage.Checked = true;
                //chkBoxMinAvgVoltage.Checked = true;
                //chkBoxMaxAvgCurrent.Checked = true;
                //chkBoxMinAvgCurrent.Checked = true;
            }
            else
            {
                chkBoxKwh.Checked = false;
                chkBoxKvarhLag.Checked = false;
                chkBoxKvarhLead.Checked = false;
                chkBoxKVAh.Checked = false;
                chkBoxDailyMD1.Checked = false;
                chkBoxDailyMD2.Checked = false;
            }
        }

        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefault.Checked == true)
            {
                chkAll.Checked = false;
                chkBoxKwh.Checked = true;
                chkBoxKVAh.Checked = true;
                chkBoxDailyMD1.Checked = true;
                chkBoxDailyMD2.Checked = true;
                chkBoxDailyMD3.Checked = true;
                chkBoxMaxAvgVoltage.Checked = true;
                chkBoxMaxAvgCurrent.Checked = true;
            }
            else
            {
                chkBoxKwh.Checked = false;
                chkBoxKVAh.Checked = false;
                chkBoxDailyMD1.Checked = false;
                chkBoxDailyMD2.Checked = false;
                chkBoxDailyMD3.Checked = false;
                chkBoxMaxAvgVoltage.Checked = false;
                chkBoxMaxAvgCurrent.Checked = false;
            }
        }

        private void InitializeAllCheckboxes()
        {
            //chkDefault.Checked = false;
            chkBoxKwh.Checked = false;
            chkBoxKvarhLag.Checked = false;
            chkBoxKvarhLead.Checked = false;
            chkBoxKVAh.Checked = false;
            chkBoxDailyMD1.Checked = false;
            chkBoxDailyMD2.Checked = false;
            chkBoxDailyMD3.Checked = false;
            chkBoxMaxAvgVoltage.Checked = false;
            chkBoxMinAvgVoltage.Checked = false;
            chkBoxMaxAvgCurrent.Checked = false;
            chkBoxMinAvgCurrent.Checked = false;
            chkBoxCumFundkWh.Checked = false;
        }

        private void MeterDataProgramming_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;

            if (ConfigInfo.IsGSMConnected())
            {
                btnUpdateRTC.Enabled = false;
                btnReadRTC.Enabled = false;
                btnMagneticTamperReset.Enabled = false;
            }
            else
            {
                btnUpdateRTC.Enabled = true;
                btnReadRTC.Enabled = true;
                btnMagneticTamperReset.Enabled = true;
            }
        }

        private void MeterDataProgramming_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StatusMessage = string.Empty;

        }

        private void btnLoadTOU_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTOUFile = new OpenFileDialog();
            openTOUFile.DefaultExt = "TOU";
            openTOUFile.InitialDirectory = ConfigInfo.GetTOULocation();
            //openTOUFile.InitialDirectory = (@"C:\CAB Config");
            openTOUFile.Filter = "TOU Configuration file(*.TOU)|*.TOU";
            DialogResult result = openTOUFile.ShowDialog();
            if (result == DialogResult.OK)
                DisplayTOUFromFile(openTOUFile.FileName);
        }



        private void DisplayTOUFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                this.StatusMessage = "File does not exists.";
            string fileContent = new TOUInformation().GetTOUFileContent(filePath);
            if (string.IsNullOrEmpty(fileContent))
                this.StatusMessage = "Empty file.";
            DisplayTOU(fileContent);
        }

        private DataGridView[] GetSeasonGridCollection()
        {
            DataGridView[] seasonGrids = new DataGridView[] 
            {
                gridS1Day1, gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6,
                gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, 
                gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6,
                gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6
            };
            return seasonGrids;
        }

        private DataGridView[] GetHolidayGridCollection()
        {
            DataGridView[] holidayGrids = new DataGridView[] 
            {
                gridHoliday1,gridHoliday2, gridHoliday3, gridHoliday4, gridHoliday5,
                gridHoliday6, gridHoliday7, gridHoliday8, gridHoliday9, gridHoliday10
            };
            return holidayGrids;
        }

        private DataGridView[] GetAssignmentGridCollection()
        {
            DataGridView[] dayAssignmentGrids = new DataGridView[] 
            {
                gridAssignmentS1, gridAssignmentS2, gridAssignmentS3, gridAssignmentS4
            };
            return dayAssignmentGrids;
        }

        private DateTimePicker[] GetActivationDateCollection()
        {
            DateTimePicker[] holidayActivationDates = new DateTimePicker[]
            {
                dtPicAcDate1, dtPicAcDate2, dtPicAcDate3, dtPicAcDate4, dtPicAcDate5,
                dtPicAcDate6, dtPicAcDate7, dtPicAcDate8, dtPicAcDate9, dtPicAcDate10 
            };
            return holidayActivationDates;
        }

        private void DisplayTOU(string touData)
        {
            int tableIndex = 0;
            int rowIndex = 0;
            DataSet ds = new DataSet();

            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();
            DataGridView[] dayAssignmentGrids = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();

            ds = ProgrammingCommon.DisplayTOUData(touData, "Current");//2 march 2012: tag changed from "Future" to "Current" to display current TOU 
            if (ds == null)
            {
                this.StatusMessage = "Invalid TOU";
                return;
            }

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Rows.Clear();
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    seasonGrid.Rows.Add();
                    seasonGrid.Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                tableIndex++;
            }

            for (int holidayIndex = 0; holidayIndex <= holidayGrids.GetUpperBound(0); holidayIndex++)
            {
                holidayGrids[holidayIndex].Rows.Clear();
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    holidayGrids[holidayIndex].Rows.Add();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                {
                    DateTime dt;
                    if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                        if (DateTime.TryParse(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString(), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                        {
                            dtPickerCollection[holidayIndex].Value = dt;
                            dtPickerCollection[holidayIndex].CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                        }
                }
                tableIndex++;
            }
            //for (int seasonIndex = 0; seasonIndex <= gridSeason.GetUpperBound(0); seasonIndex++)
            //{
            //    gridSeason[seasonIndex].DataSource = ds.Tables[tableIndex++];
            //}
            //for (int holidayIndex = 0; holidayIndex <= gridHoliday.GetUpperBound(0); holidayIndex++)
            //{
            //    gridHoliday[holidayIndex].DataSource = ds.Tables[tableIndex];
            //    gridHoliday[holidayIndex].Columns["Activation Date"].Visible = false;
            //    dtPickerCollection[holidayIndex].Value = Convert.ToDateTime(ds.Tables[tableIndex++].Rows[0]["Activation Date"].ToString());
            //    dtPickerCollection[holidayIndex].CustomFormat = ConfigInfo.DateFormat();
            //}
            if (!string.IsNullOrEmpty(ProgrammingCommon.futureActivationDate))
            {
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                dtPickerFutureActivationDate.Value = ProgrammingCommon.GetDate(ProgrammingCommon.futureActivationDate, true);
            }
            for (int dayAssignment = 0; dayAssignment <= dayAssignmentGrids.GetUpperBound(0); dayAssignment++)
            {
                rowIndex = 0;
                dayAssignmentGrids[dayAssignment].Rows.Clear();
                foreach (DataRow row in ds.Tables[tableIndex].Rows)
                {
                    dayAssignmentGrids[dayAssignment].Rows.Add();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[0].Value = row["Day"].ToString();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[1].Value = row["Day Table"].ToString();
                    rowIndex++;
                }
                tableIndex++;
            }

            rowIndex = 0;
            gridActivation.Rows.Clear();
            dtPickerFutureActivationDate.Value = Convert.ToDateTime(ds.Tables[tableIndex].Rows[0]["Season Activation Date"].ToString(), new CultureInfo("en-GB"));

            foreach (DataRow row in ds.Tables[tableIndex].Rows)
            {
                gridActivation.Rows.Add();
                gridActivation.Rows[rowIndex].Cells[0].Value = row["Season Activation Date"].ToString();
                gridActivation.Rows[rowIndex].Cells[1].Value = Convert.ToInt32(row["Season Number"].ToString()).ToString();
                rowIndex++;
            }

        }



        private void btnGetTOU_Click(object sender, EventArgs e)
        {
            TOUInformation touInformation = new TOUInformation();
            touInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            string touData = string.Empty;
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                touInformation.MeterPassword = meterPswd;
                touInformation.Channel = communications;
                touData = touInformation.GetTOU();
                if (touData != "")
                {
                    DisplayTOU(touData);
                    this.StatusMessage = "Readout successful!";
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "btnGetTOU_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSetTOU_Click(object sender, EventArgs e)
        {
            //if (!isValidTOU)
            //{
            //    this.StatusMessage = "Invalid Entry!";
            //    Application.DoEvents();
            //    return;
            //}
            //if (!ValidateTOUData())
            //{
            //    this.Cursor = Cursors.Default;
            //    return;
            //}
            //List<string> touCommands;
            //TOUInformation touInformation = new TOUInformation();
            //touInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            //try
            //{
            //    InitializeProgrammingValues();
            //    this.Cursor = Cursors.WaitCursor;
            //    meterPassword.ShowDialog();
            //    Application.DoEvents();
            //    if (meterPswd.Length == 0)
            //    {
            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //    touInformation.MeterPassword = meterPswd;
            //    touInformation.Channel = communications;

            //    touCommands = GetTOUCommands();
            //    if (touInformation.SetTOU(touCommands))
            //        this.StatusMessage = "TOU set successfully!";
            //    this.Cursor = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //finally
            //{
            //    FinalizeProgrammingValues();
            //}
        }


        private List<string> GetValuesFromDayTableGrid(string GridName)
        {
            List<string> lstTOUTable = new List<string>();
            DataGridView dgv = this.Controls.Find(GridName, true).FirstOrDefault() as DataGridView;
            if (dgv != null)
            {
                int indexRow = 0;
                foreach (DataGridViewRow itemRow in dgv.Rows)
                {
                    string Data = string.Empty;
                    string CellData = Convert.ToString(itemRow.Cells["Tariff"].Value).Trim();

                    if (CellData == string.Empty)
                    {
                        Data = "00000";
                    }
                    else
                    {
                        Data = CellData.Substring(1, 1) + Convert.ToString(itemRow.Cells["Hours"].Value).Trim() + Convert.ToString(itemRow.Cells["Minutes"].Value).Trim();
                    }
                    lstTOUTable.Add(Data);
                    indexRow++;
                }
            }
            return lstTOUTable;
        }

        private List<string> GetValuesFromWeekTableGrid()
        {
            List<string> lstTOUTable = new List<string>();
            int indexRow = 0;
            foreach (DataGridViewRow itemRow in gridDayTables.Rows)
            {
                string Data = string.Empty;
                if (Convert.ToString(itemRow.Cells["Mon"].Value) == string.Empty)
                {
                    Data = "00000000000";
                }
                else
                {
                    Data = Convert.ToString(itemRow.Cells["Mon"].Value) +
                            Convert.ToString(itemRow.Cells["Tue"].Value) +
                            Convert.ToString(itemRow.Cells["Wed"].Value) +
                            Convert.ToString(itemRow.Cells["Thu"].Value) +
                            Convert.ToString(itemRow.Cells["Fri"].Value) +
                            Convert.ToString(itemRow.Cells["Sat"].Value) +
                            Convert.ToString(itemRow.Cells["Sun"].Value) +
                            Convert.ToString(gridActivationDate.Rows[indexRow].Cells[0].Value) +
                            Convert.ToString(gridActivationDate.Rows[indexRow].Cells[1].Value);
                }
                lstTOUTable.Add(Data);
                indexRow++;
            }
            return lstTOUTable;
        }






        private List<string> GetTOUCommandsSP()
        {
            //
            /* 0x01   W   4   0x02  D     4    0   INDEX  (   DATA   )  {0x03 OR 0x04} BCC */
            //CHAR DISPLAY
            /* 0x01 0x57 0x34 0x02 0x44  0x34 0x30 INDEX 0x28 DATA 0x29 {0x03 OR 0x04} BCC */
            //HEX DISPLAY

            /* 0x01   W   4   0x02  (   DATA   )  {0x03 OR 0x04} BCC */
            //CHAR DISPLAY
            /* 0x01 0x57 0x34 0x02 0x28 DATA 0x29 {0x03 OR 0x04} BCC */
            //HEX DISPLAY

            List<string> touCommands = new List<string>();
            string HeaderText1 = "01573402";
            string HeaderText2 = "443430";
            string Header31 = "31";
            string Header32 = "32";
            string Header33 = "33";
            string Header34 = "34";
            string Header30 = "30";
            string HeaderStartBracket = "28";
            string HeaderEndBracket = "29";
            string HeaderResponseComplete = "03";
            string HeaderResponseRemaining = "04";



            //DayTable Entry
            for (int index = 1; index < 6; index++)
            {
                switch (index)
                {
                    case 1:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay1_name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header31;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;
                    case 2:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay2_name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header32;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 3:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay3_name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header33;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 4:
                        {
                            List<string> TempLstCommands = GetValuesFromDayTableGrid(gridTOUDay4_name);
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header34;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    case 5:
                        {
                            //Week Table Entry
                            List<string> TempLstCommands = GetValuesFromWeekTableGrid();
                            for (int i = 0; i < TempLstCommands.Count; i++)
                            {
                                string TempListCommand = string.Empty;
                                TempListCommand += HeaderText1;
                                if (i == 0)
                                {
                                    TempListCommand += HeaderText2 + Header30;
                                }
                                TempListCommand += HeaderStartBracket + ProgrammingCommon.GetASCIIValue(TempLstCommands[i]) + HeaderEndBracket;
                                if (i == (TempLstCommands.Count - 1))
                                {
                                    TempListCommand += HeaderResponseComplete;
                                }
                                else
                                {
                                    TempListCommand += HeaderResponseRemaining;
                                }
                                touCommands.Add(TempListCommand);
                            }
                        }
                        break;

                    default:
                        {

                        }
                        break;
                }
            }
            return touCommands;
        }

        private List<string> GetTOUCommands()
        {
            List<string> touCommands = new List<string>();
            string touAddress = string.Empty;
            string touCommand = string.Empty;
            int slots = 0, gridIndex = 0;
            string dayTable = string.Empty;
            string holidayActivationDate = string.Empty;

            int[] touMemoryAddress = new int[] 
            {
                784, 785, 786, 787, 788, 789, 800, 801, 802, 803, 804, 805, 816,
                817, 818, 819, 820, 821, 832, 833, 834, 835, 836, 837, 848, 849,    //TOU address in meter memory 
                850, 851, 852, 853, 854, 855, 856, 857, 790, 806, 822, 838, 864 
            };

            DataGridView[] gridSeason = GetSeasonGridCollection();
            DataGridView[] gridHoliday = GetHolidayGridCollection();
            DataGridView[] gridDayAssignment = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();


            for (int seasonIndex = 0; seasonIndex <= gridSeason.GetUpperBound(0); seasonIndex++)
            {
                touAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                touCommand = string.Empty;

                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridSeason[seasonIndex].Rows)
                {
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((seasonIndex % 6 + 1 % 7).ToString("d2"));
                touCommands.Add("01573102" + touAddress + "28" + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + touCommand + "29" + "03");
            }

            for (int holidayIndex = 0; holidayIndex <= gridHoliday.GetUpperBound(0); holidayIndex++)
            {
                touAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                touCommand = string.Empty;

                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridHoliday[holidayIndex].Rows)
                {
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((holidayIndex % 6 + 1 % 7).ToString("d2"));
                //holidayActivationDate = ProgrammingCommon.GetASCIIValue(dtPickerCollection[holidayIndex].Value.Date.ToShortDateString().Replace(ConfigInfo.DateFormat().Substring(2, 1), "")); 
                ////
                //holidayActivationDate = DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString();
                holidayActivationDate = ProgrammingCommon.GetASCIIValue(DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString().Substring(2, 6));
                /////
                touCommands.Add("01573102" + touAddress + "28" + holidayActivationDate + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + touCommand + "29" + "03");
            }

            for (int dayAssignment = 0; dayAssignment <= gridDayAssignment.GetUpperBound(0); dayAssignment++)
            {
                touCommand = string.Empty;
                touAddress = string.Empty;
                touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));

                foreach (DataGridViewRow row in gridDayAssignment[dayAssignment].Rows)
                {
                    string tempStr = row.Cells[1].Value.ToString();
                    touCommand += ProgrammingCommon.GetASCIIValue(tempStr.Replace("Day Table ", "0").Trim());
                }
                touCommands.Add("01573102" + touAddress + "28" + touCommand + "29" + "03");
            }

            touAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
            touCommand = string.Empty;
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Day.ToString("d2"));
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Month.ToString("d2"));
            touCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Year.ToString().Substring(2));

            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string tempCommand = "";
                DateTime dateTime = ProgrammingCommon.GetDate(row.Cells["SeasonActivationDate"].Value.ToString(), true);
                tempCommand = String.Format("{0:00}", dateTime.Day.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", dateTime.Month.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", Convert.ToInt16(row.Cells["SeasonNumber"].Value.ToString()));
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                touCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
            }
            touCommands.Add("01573102" + touAddress + "28" + touCommand + "29" + "03");
            return touCommands;

        }

        private void gridActivation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gridActivation[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gridActivation.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                        {
                            DateTime dt = new DateTime();
                            CalendarEditingControl ctl = gridActivation.EditingControl as CalendarEditingControl;
                            if (ctl != null)
                            {
                                bool isValid = DateTime.TryParse(ctl.Value.ToString(), out dt);
                                if (!isValid)
                                {
                                    gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = true;
                                    gridActivation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ctl.Value;
                                    gridActivation.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        else
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        if (Convert.ToInt16(e.FormattedValue.ToString()) < 1 || (Convert.ToInt16(e.FormattedValue.ToString()) > 4))
                        {
                            gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            isValidTOU = true;
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                        }
                    }
                }
            }
        }

        private void gridActivation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAcCellClick();
        }

        private void GridAcCellClick()
        {
            if (gridActivation.CurrentCell.Style.ForeColor == Color.Red)
            {
                gridActivation.CurrentCell.Style.ForeColor = Color.Black;
                if (gridActivation.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                    comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    comboCell.Items.Add("1");
                    comboCell.Items.Add("2");
                    comboCell.Items.Add("3");
                    comboCell.Items.Add("4");
                    int rIndex = gridActivation.CurrentCell.RowIndex;
                    gridActivation.Rows[rIndex].Cells[1] = comboCell;
                }
            }
        }

        private string ValidateTOUData()
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                DataGridView[] gridSeason = GetSeasonGridCollection();
                DataGridView[] gridHoliday = GetHolidayGridCollection();

                string activationMessage = string.Empty;
                foreach (DataGridView gridTOU in gridSeason)
                {
                    //errorMessage.Append(CheckTOUSlots(gridTOU));

                    if (errorMessage.ToString() != string.Empty)
                    {
                        errorMessage.Append(Symbols.NEWLINE);
                        break;
                    }
                }
                activationMessage = CheckActivationDate();
                if (activationMessage.Length > 0)
                {

                    errorMessage.Append(activationMessage);
                    errorMessage.Append(Symbols.NEWLINE);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateTOUData()", ex);
                throw;
            }


            return errorMessage.ToString();
        }

        private string CheckTOUSlots(DataGridView gridTOU)
        {
            if (gridTOU.Rows[0].Cells[1].Value.ToString() == "00")
            {

                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return "Season slots not complete!";
            }
            if (!isValidTOU)
            {
                //this.StatusMessage = "Invalid Entry!";
                Application.DoEvents();
                return "Invalid Entry!";
            }
            int grdRowIndex = 0;
            for (grdRowIndex = 1; grdRowIndex <= 9; grdRowIndex++)
            {
                if (Convert.ToString(gridTOU.Rows[grdRowIndex].Cells[1].Value) != "00")
                {
                    if (Convert.ToString(gridTOU.Rows[grdRowIndex].Cells[2].Value) == "00")
                    {
                        if ((Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) != "00") || (Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(gridTOU.Rows[grdRowIndex - 1].Cells[3].Value) == 45) || (Convert.ToString(gridTOU.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(gridTOU.Rows[grdRowIndex - 1].Cells[3].Value) >= Convert.ToInt16(gridTOU.Rows[grdRowIndex].Cells[3].Value)))
                        {
                            //do
                            //{
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[1].ReadOnly = true;
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[2].ReadOnly = true;
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[3].ReadOnly = true;
                            //    grdRowIndex++;
                            //} while (grdRowIndex != 9);
                            return "Invalid Entry!";
                        }
                    }
                }
            }
            return string.Empty;
        }

        private string CheckActivationDate()
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                DateTime prevDate = new DateTime();
                DateTime currentDate = new DateTime();
                DateTime futureActivationDate = DateTime.Now.AddDays(1).Date;
                TimeSpan ts = futureActivationDate.Date.Subtract(dtPickerFutureActivationDate.Value.Date);

                if (ts.Days > 0)
                {
                    errorMessage.Append(string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy")));
                    errorMessage.Append(Symbols.NEWLINE);

                }

                foreach (DataGridViewRow row in gridActivation.Rows)
                {
                    //DateTime dt;
                    if (row.Index == 0)
                    {
                        prevDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(), false);
                    }
                    else
                    {
                        currentDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(), true);
                        if (prevDate.Day == currentDate.Day && prevDate.Month == currentDate.Month)
                        {
                            errorMessage.Append("Season activation dates should be unique");
                            errorMessage.Append(Symbols.NEWLINE);
                        }
                        prevDate = currentDate.Date;
                    }
                    if (prevDate.Day == 29 && prevDate.Month == 2)
                    {
                        errorMessage.Append("29 Feb cannot be selected as a Season Activation Date");
                        errorMessage.Append(Symbols.NEWLINE);
                    }

                    //if (prevDate < dtPickerFutureActivationDate.Value.Date)
                    //{
                    //    this.StatusMessage = "Season Activation date cannot be less than the Future TOU Activation Date";
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckActivationDate()", ex);
                throw;
            }
            return errorMessage.ToString();
        }

        private void gridActivation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            //this.StatusMessage = "";
            //if (!ValidateTOUData())
            //{
            //    this.Cursor = Cursors.Default;
            //    return;
            //}
            //if (CreateTOUFile())
            //    MessageBox.Show("File created sucessfully at " + ConfigInfo.GetTOULocation() + @"\" + touFileName.Trim() +  ".TOU", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //this.StatusMessage = string.Concat("File created sucessfully at ", ConfigInfo.GetTOULocation());
        }

        private bool CreateTOUFile()
        {
            try
            {
                string touFilePath = string.Empty;
                string touContent = CreateTOUCommand();
                touFileName = GetTOUFileName();
                if (string.IsNullOrEmpty(touFileName))
                {
                    return false;
                }

                touFilePath = ConfigInfo.GetTOULocation();
                if (!Directory.Exists(touFilePath))
                    Directory.CreateDirectory(touFilePath);
                FileStream fs = new FileStream(string.Concat(touFilePath, touFileName.Trim(), ".TOU"), FileMode.Create);
                //FileStream fs = new FileStream(@"C:\CAB Config\" + fileName + ".TOU", FileMode.Create);
                StreamWriter sr = new StreamWriter(fs);
                sr.Write(touContent);
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = ex.Message;
                logger.Log(LOGLEVELS.Error, "CreateTOUFile()", ex);
                return false;
            }
            return true;
        }

        private string CreateTOUCommand()
        {
            int touIndex = 0, touSeason = 0, touSeasonDay = 0, touHoliday = 0, touDateActvation = 0;
            List<string> touFileContent = new List<string>();
            string fileContent = string.Empty;

            touFileContent = GetTOUCommands();
            if (touFileContent[0].Trim().Length == 0)
                return string.Empty;

            touSeason = 1;
            while (touSeason <= 4)  //Season Day Table
            {
                touSeasonDay = 1;
                while (touSeasonDay <= 6)
                {
                    fileContent += "<S" + touSeason.ToString() + "D" + touSeasonDay.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</S" + touSeason.ToString() + "D" + touSeasonDay.ToString() + ">" + "\r\n";
                    touSeasonDay++;
                    touIndex++;
                }
                touSeason++;
            }

            touHoliday = 1;
            while (touHoliday <= 10)    //Holiday
            {
                fileContent += "<HD" + touHoliday.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</HD" + touHoliday.ToString() + ">" + "\r\n";
                touHoliday++;
                touIndex++;

            }

            touDateActvation = 1;
            while (touDateActvation <= 4)   //Activation Date
            {
                fileContent += "<SD" + touDateActvation.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</SD" + touDateActvation.ToString() + ">" + "\r\n";
                touDateActvation++;
                touIndex++;

            }

            /*Future Activation date*/
            fileContent += "<FSAD>" + CreateTOUString(touFileContent[touIndex]) + "</FSAD>";
            return fileContent;
        }

        private string CreateTOUString(string cmdTou)
        {
            int cmdLength = 0;
            string strRes = string.Empty;
            while (cmdLength < cmdTou.Length)
            {
                if (cmdLength == cmdTou.Length - 2)
                    strRes += cmdTou.Substring(cmdLength, 2);
                else
                    strRes += cmdTou.Substring(cmdLength, 2) + "\x20";
                cmdLength += 2;
            }
            return strRes;
        }

        private string GetTOUFileName()
        {
            //string fileName = string.Empty;
            // Story - 349654 - While calling ReadoutMessageBox function, file name creation is commented over there and file name will be passed from here
            string fileName = ReadoutCommon.GetFileName().Trim();

            bool Flag = false;
            do
            {
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.TOU) == DialogResult.OK)
                {
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    fileName = string.Empty;
                    break;
                }
            } while (!Flag);

            return fileName;
        }

        /// <summary>
        /// RTC Timer event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtcTimer_Tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("en-GB");
            //System.Threading.Thread.CurrentThread.CurrentCulture = c;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = c;
            rtcCtrl.Controls[0].Controls["txtRTC"].Text = System.DateTime.Now.ToString(c);
        }

        private void MeterDataProgramming_Load(object sender, EventArgs e)
        {
            //HideTabs();
            HideTabs(enumData); // Story - 354382 - To dispaly the tabs as according to profiles in the grid
            LoadTabs();

             //Initialize TOU 1P NDLMS Grid
            if (CAB.IECFramework.Utility.ConfigInfo.SignatureInfo.Length > 8 && CAB.IECFramework.Utility.ConfigInfo.SignatureInfo[7] == 'A')
            {
                Is10Zone8Slots = true;
                Is10Zone6Slot = false;
                TOUZone = 10;
                TOUSlot = 8;
            }
            else if (CAB.IECFramework.Utility.ConfigInfo.SignatureInfo.Length > 8 && CAB.IECFramework.Utility.ConfigInfo.SignatureInfo[7] == 'B')
            {
                Is10Zone6Slot = true;
                Is10Zone8Slots = false;
                TOUZone = 10;
                TOUSlot = 6;
            }
            else
            {
                Is10Zone8Slots = false;
                Is10Zone6Slot = false;
                TOUZone = 6;
                TOUSlot = 6;
            }

            InitializeSinglePhaseTouGrid();
            Associate_SP_NDLMS_GridEvent();


            listSelectedParams = new List<System.Enum>();
            rdAll.Checked = true;
            rdBtnAutoRTC.Checked = true;
            rdBtnManualRTC.Checked = false;
            rtcTimer.Start();
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
            dtPickerRTC.Format = DateTimePickerFormat.Custom;
            dtPickerRTC.CustomFormat = string.Concat("dd/MM/yyyy HH:mm:ss"); //ConfigInfo.DateFormat();
            dtPickerRTC.Value = DateTime.Now;
            SetTOUGrids();
           

            if (File.Exists(string.Concat(ConfigInfo.GetTOULocation(), @"\TouConfig.TOU")))
                DisplayTOUFromFile(string.Concat(ConfigInfo.GetTOULocation(), @"\TouConfig.TOU"));
            else
                ResetAllGrids();
            chkBoxDailyMD3.Checked = false;
            chkBoxMaxAvgCurrent.Checked = false;
            chkBoxMaxAvgVoltage.Checked = false;
            chkBoxMinAvgCurrent.Checked = false;
            chkBoxMinAvgVoltage.Checked = false;
            chkBoxCumFundkWh.Checked = false;
            //No Need of this page So Removing It .
            tabControlMeterConfiguration.TabPages.Remove(tabPageResets);
            // Story - 354382 - Set the seconds with minutes in the Time Interval drop down
            if (tabControlMeterConfiguration.TabPages.Contains(tabDIP))
            {
                //for DIP default values
                cmbDIPDemandInterval.Items.Clear();
                cmbDIPDemandInterval.Items.Add("15 (900)");
                cmbDIPDemandInterval.Items.Add("30 (1800)");
            }
            //Initialize TOU 1P NDLMS Grid
            Initialize_TOU_SP_NDLMS_Grid_with_Zone_Slots();
            Associate_SP_NDLMS_GridEvent();
        }


        private void Initialize_TOU_SP_NDLMS_Grid_with_Zone_Slots()
        {
            if (CAB.IECFramework.Utility.ConfigInfo.SignatureInfo.Length > 8 && CAB.IECFramework.Utility.ConfigInfo.SignatureInfo[7] == 'A')
            {
                Is10Zone8Slots = true;
                Is10Zone6Slot = false;
                TOUZone = 10;
                TOUSlot = 8;
            }
            else if (CAB.IECFramework.Utility.ConfigInfo.SignatureInfo.Length > 8 && CAB.IECFramework.Utility.ConfigInfo.SignatureInfo[7] == 'B')
            {
                Is10Zone6Slot = true;
                Is10Zone8Slots = false;
                TOUZone = 10;
                TOUSlot = 6;
            }
            else
            {
                Is10Zone8Slots = false;
                Is10Zone6Slot = false;
                TOUZone = 6;
                TOUSlot = 6;
            }
            InitializeSinglePhaseTouGrid();           
        }



        private void Associate_SP_NDLMS_GridEvent()
        {
            this.gridTOUDay1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.SP_NDLMS_ValidateDayProfileCell);
            this.gridTOUDay1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SP_NDLMS_DayGridCellClick);

            this.gridTOUDay2.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.SP_NDLMS_ValidateDayProfileCell);
            this.gridTOUDay2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SP_NDLMS_DayGridCellClick);

            this.gridTOUDay3.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.SP_NDLMS_ValidateDayProfileCell);
            this.gridTOUDay3.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SP_NDLMS_DayGridCellClick);

            this.gridTOUDay4.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.SP_NDLMS_ValidateDayProfileCell);
            this.gridTOUDay4.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SP_NDLMS_DayGridCellClick);

        }

        private void InitializeSinglePhaseTouGrid()
        {
            try
            {                
                InitializeGrid(gridDayTables.Name);
                InitializeGrid(gridActivationDate.Name);
                InitializeGrid("gridTOUDay1");
                InitializeGrid("gridTOUDay2");
                InitializeGrid("gridTOUDay3");
                InitializeGrid("gridTOUDay4");
                ResetSinglePhaseNdlmsTouValues();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeSinglePhaseTouGrid()", ex);
            }
        }

        private DataGridViewComboBoxColumn GetSNo()
        {
            DataGridViewComboBoxColumn colSNo = new DataGridViewComboBoxColumn();
            colSNo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colSNo.Name = "SNo.";
            colSNo.HeaderText = "SNo.";
            colSNo.Items.Add("1");
            colSNo.Items.Add("2");
            colSNo.Items.Add("3");
            colSNo.Items.Add("4");
            colSNo.Items.Add("5");
            colSNo.Items.Add("6");
            colSNo.Items.Add("7");
            colSNo.Items.Add("8");
            colSNo.Items.Add("9");
            colSNo.Items.Add("10");
            return colSNo;
        }

        private DataGridViewComboBoxColumn GetRates()
        {
            DataGridViewComboBoxColumn colRate = new DataGridViewComboBoxColumn();
            colRate.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colRate.Name = "Rate";
            colRate.HeaderText = "Rate";
            colRate.Items.Add("T1");
            colRate.Items.Add("T2");
            colRate.Items.Add("T3");
            colRate.Items.Add("T4");
            colRate.Items.Add("T5");
            colRate.Items.Add("T6");
            colRate.Items.Add("T7");
            colRate.Items.Add("T8");
            colRate.Items.Add("00");
            return colRate;
        }

        private DataGridViewComboBoxColumn GetStartHour()
        {
            DataGridViewComboBoxColumn colStartHour = new DataGridViewComboBoxColumn();
            colStartHour.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartHour.Name = "Start Hour";
            colStartHour.HeaderText = "Start Hour";
            colStartHour.Items.Add("00");
            colStartHour.Items.Add("01");
            colStartHour.Items.Add("02");
            colStartHour.Items.Add("03");
            colStartHour.Items.Add("04");
            colStartHour.Items.Add("05");
            colStartHour.Items.Add("06");
            colStartHour.Items.Add("07");
            colStartHour.Items.Add("08");
            colStartHour.Items.Add("09");
            colStartHour.Items.Add("10");
            colStartHour.Items.Add("11");
            colStartHour.Items.Add("12");
            colStartHour.Items.Add("13");
            colStartHour.Items.Add("14");
            colStartHour.Items.Add("15");
            colStartHour.Items.Add("16");
            colStartHour.Items.Add("17");
            colStartHour.Items.Add("18");
            colStartHour.Items.Add("19");
            colStartHour.Items.Add("20");
            colStartHour.Items.Add("21");
            colStartHour.Items.Add("22");
            colStartHour.Items.Add("23");
            return colStartHour;
        }

        private DataGridViewComboBoxColumn GetStartMinute()
        {
            DataGridViewComboBoxColumn colStartMinute = new DataGridViewComboBoxColumn();
            colStartMinute.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartMinute.Name = "Start Minute";
            colStartMinute.HeaderText = "Start Minute";
            colStartMinute.Items.Add("00");
            colStartMinute.Items.Add("15");
            colStartMinute.Items.Add("30");
            colStartMinute.Items.Add("45");
            return colStartMinute;
        }

        private void SetTOUGrids()
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Columns.Add(GetSNo());
                seasonGrid.Columns.Add(GetRates());
                seasonGrid.Columns.Add(GetStartHour());
                seasonGrid.Columns.Add(GetStartMinute());
                seasonGrid.Columns[0].ReadOnly = true;
            }
            foreach (DataGridView holidayGrid in holidayGrids)
            {
                holidayGrid.Columns.Add(GetSNo());
                holidayGrid.Columns.Add(GetRates());
                holidayGrid.Columns.Add(GetStartHour());
                holidayGrid.Columns.Add(GetStartMinute());
                holidayGrid.Columns[0].ReadOnly = true;
            }
        }

        private void gridS1Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day1);
        }

        private void gridS1Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day2);
        }

        private void gridS1Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day3);
        }

        private void gridS1Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day4);
        }

        private void gridS1Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day5);
        }

        private void gridS1Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day6);
        }

        private void gridS2Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day1);
        }

        private void gridS2Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day2);
        }

        private void gridS2Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day3);
        }

        private void gridS2Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day4);
        }

        private void gridS2Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day5);
        }

        private void gridS2Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day6);
        }

        private void gridS3Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day1);
        }

        private void gridS3Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day2);
        }

        private void gridS3Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day3);
        }

        private void gridS3Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day4);
        }

        private void gridS3Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day5);
        }

        private void gridS3Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day6);
        }

        private void gridS4Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day1);
        }

        private void gridS4Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day2);
        }

        private void gridS4Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day3);
        }

        private void gridS4Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day4);
        }

        private void gridS4Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day5);
        }

        private void gridS4Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day6);
        }

        private void gridHoliday1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday1);
        }

        private void gridHoliday2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday2);
        }

        private void gridHoliday3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday3);
        }

        private void gridHoliday4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday4);
        }

        private void gridHoliday5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday5);
        }

        private void gridHoliday6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday6);
        }

        private void gridHoliday7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday7);
        }

        private void gridHoliday8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday8);
        }

        private void gridHoliday9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday9);
        }

        private void gridHoliday10_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday10);
        }


        private void gridS1Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day1, sender, e);
        }

        private void gridS1Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day2, sender, e);
        }

        private void gridS1Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day3, sender, e);
        }

        private void gridS1Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day4, sender, e);
        }

        private void gridS1Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day5, sender, e);
        }

        private void gridS1Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day6, sender, e);
        }

        private void gridS2Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day1, sender, e);
        }

        private void gridS2Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day2, sender, e);
        }

        private void gridS2Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day3, sender, e);
        }

        private void gridS2Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day4, sender, e);
        }

        private void gridS2Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day5, sender, e);
        }

        private void gridS2Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day6, sender, e);
        }

        private void gridS3Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day1, sender, e);
        }

        private void gridS3Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day2, sender, e);
        }

        private void gridS3Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day3, sender, e);
        }

        private void gridS3Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day4, sender, e);
        }

        private void gridS3Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day5, sender, e);
        }

        private void gridS3Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day6, sender, e);
        }

        private void gridS4Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day1, sender, e);
        }

        private void gridS4Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day2, sender, e);
        }

        private void gridS4Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day3, sender, e);
        }

        private void gridS4Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day4, sender, e);
        }

        private void gridS4Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day5, sender, e);
        }

        private void gridS4Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day6, sender, e);
        }

        private void gridHoliday1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday1, sender, e);
        }

        private void gridHoliday2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday2, sender, e);
        }

        private void gridHoliday3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday3, sender, e);
        }

        private void gridHoliday4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday4, sender, e);
        }

        private void gridHoliday5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday5, sender, e);
        }

        private void gridHoliday6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday6, sender, e);
        }

        private void gridHoliday7_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday7, sender, e);
        }

        private void gridHoliday8_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday8, sender, e);
        }

        private void gridHoliday9_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday9, sender, e);
        }

        private void gridHoliday10_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday10, sender, e);
        }


        /// <summary>
        /// This method is called while click on the season grid from the view and validating the grid SP NDLMS.
        /// </summary>
        private void SP_NDLMS_DayGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = (DataGridView)sender;
            try
            {               
                int dayProfCount = 4;
                DataGridView[] dayProfGrids = new DataGridView[] { gridTOUDay1, gridTOUDay2, gridTOUDay3, gridTOUDay4 };


                rcount = dataGrid.CurrentCell.RowIndex;
                if (rcount != 0 && dataGrid.Rows[rcount - 1].Cells[1].Value != null)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[2].Value != null && dataGrid.Rows[rcount - 1].Cells[3].Value != null)
                    {
                        if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "55")
                        {
                            for (count = rcount; count < TOUZone; count++)
                            {
                                dataGrid.Rows[count].ReadOnly = true;
                            }
                            return;
                        }
                    }
                }

                //for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                //{
                //    for (int rowCount = 0; rowCount < 9; rowCount++)
                //    {
                //        if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value != null)
                //            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value != null))
                //        {
                //            if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value.ToString())
                //                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value)))
                //            {
                //                while (rowCount < 8)
                //                {
                //                    dayProfileGrids[gIndex].Rows[rowCount + 2].ReadOnly = true;
                //                    rowCount++;
                //                }
                //                return;
                //            }
                //        }
                //    }
                //}

                if (rcount != 0)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[3].Value == null)
                    {
                        int rowCount = rcount;
                        while (rowCount < TOUZone)
                        {
                            dataGrid.Rows[rowCount].ReadOnly = true;
                            rowCount++;
                        }
                        if (dataGrid.Rows[rcount - 1].Cells[1].Value == null && dataGrid.Rows[rcount - 1].Cells[2].Value == null)
                        {
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        for (count = 1; count <= 3; count++)
                        {
                            dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                        }
                        rIndex = rcount + 1;
                        while (rIndex < TOUZone)
                        {
                            dataGrid.Rows[rIndex].ReadOnly = false;
                            rIndex++;
                        }
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value == null)
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].Value = null;
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = true;
                    }


                    count = 0;
                    while (dayProfGrids[count].Name != dataGrid.Name)
                    {
                        count++;
                    }




                    for (gIndex = 0; gIndex < dayProfCount; gIndex++)
                    {
                        for (rIndex = 0; rIndex < TOUZone; rIndex++)
                        {
                            if (dayProfGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                                && (dayProfGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                                || dayProfGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                            {
                                count = gIndex;
                                while (count < dayProfCount - 1)
                                {
                                    dayProfGrids[count + 1].ReadOnly = true;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfGrids[count - 1].ReadOnly = true;
                                        count--;
                                    }
                                }
                                gridDayTables.ReadOnly = true;
                                gridActivationDate.ReadOnly = true;
                                return;
                            }
                            else
                            {
                                count = gIndex;
                                while (count < dayProfCount - 1)
                                {
                                    dayProfGrids[count + 1].ReadOnly = false;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfGrids[count - 1].ReadOnly = false;
                                        count--;
                                    }
                                }
                                gridDayTables.ReadOnly = false;
                                gridActivationDate.ReadOnly = false;

                            }
                        }
                    }
                    rIndex = rcount + 1;
                    return;
                }
                else
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                    }
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }


                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && dataGrid.Rows[rcount].Cells[2].Value == null)
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = true;
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value != null
                    && (dataGrid.Rows[rcount].Cells[2].Value == null
                        && dataGrid.Rows[rcount].Cells[3].Value == null))
                {
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    rIndex = rcount + 1;
                    while (rIndex < TOUZone)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                for (gIndex = 0; gIndex < dayProfCount; gIndex++)
                {
                    for (rIndex = 0; rIndex < TOUZone; rIndex++)
                    {
                        if (dayProfGrids[gIndex].Rows[rIndex].Cells[1].Value != null
                            && (dayProfGrids[gIndex].Rows[rIndex].Cells[2].Value == null
                            || dayProfGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                        {
                            count = gIndex;
                            while (count < dayProfCount - 1)
                            {
                                dayProfGrids[count + 1].ReadOnly = true;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfGrids[count - 1].ReadOnly = true;
                                    count--;
                                }
                            }
                            gridDayTables.ReadOnly = true;
                            gridActivationDate.ReadOnly = true;
                            return;
                        }
                        else
                        {
                            count = gIndex;
                            while (count < dayProfCount - 1)
                            {
                                dayProfGrids[count + 1].ReadOnly = false;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfGrids[count - 1].ReadOnly = false;
                                    count--;
                                }
                            }
                            gridDayTables.ReadOnly = false;
                            gridActivationDate.ReadOnly = false;

                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_DayGridCellClick(object sender, DataGridViewCellEventArgs e)", ex);
            }
            finally
            {
                dataGrid.Rows[0].Cells[2].ReadOnly = true;
                dataGrid.Rows[0].Cells[3].ReadOnly = true;
                dataGrid.Columns[0].ReadOnly = true;
            }
        }



        private void SP_NDLMS_ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dtView = sender as DataGridView;
            try
            {
                if (dtView.CurrentCell.IsInEditMode == true)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (e.FormattedValue == null)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                        }
                        if (e.FormattedValue.ToString() != "")
                        {
                            if (e.RowIndex == 0)
                            {
                                dtView.Rows[e.RowIndex].Cells[2].Value = "00";
                                dtView.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 1; colCount <= gridDayTables.ColumnCount - 1; colCount++)
                                {
                                    gridDayTables.Rows[0].Cells[colCount].Value = "1";
                                }
                                gridActivationDate.Rows[0].Cells[0].Value = "01";
                                gridActivationDate.Rows[0].Cells[1].Value = "01";

                            }
                        }
                        rcount = dtView.CurrentCell.RowIndex;
                        if (dtView.Rows[rcount].Cells[1].Value == null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dtView.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < TOUZone)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < TOUZone)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null &&
                            dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                    }
                    if (e.ColumnIndex == 2)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                        }
                        if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            e.Cancel = true;
                        }
                        else if (Convert.ToInt16(e.FormattedValue) > 23)
                        {
                            e.Cancel = true;
                        }

                        else
                        {

                        }
                        if (e.RowIndex != (TOUZone - 1) && dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < TOUZone; count++)
                                    {
                                        dtView.Rows[count].ReadOnly = true;
                                    }
                                }

                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "55")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < TOUZone; count++)
                                        {
                                            dtView.Rows[count].ReadOnly = true;
                                        }

                                    }

                                }
                            }
                        }
                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < TOUZone)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }
                    }
                    if (e.ColumnIndex == 3)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }

                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 55)
                        {
                            e.Cancel = true;
                        }

                        if (e.RowIndex != (TOUZone - 1) && dtView.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                        {
                            if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)    //Exception log for catch block
            {
                dtView.Rows[e.RowIndex].ErrorText = "INVALID";
                e.Cancel = true;
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "SP_NDLMS_ValidateDayProfileCell(object sender, DataGridViewCellValidatingEventArgs e)", ex);
            }
        }

            
        private void GridCellClick(DataGridView dataGrid)
        {
            if (dataGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                int colIndex = dataGrid.CurrentCell.ColumnIndex;
                int rowIndex = dataGrid.CurrentCell.RowIndex;
                dataGrid.Rows[rowIndex].Cells[colIndex] = comboCell;

                if (dataGrid.CurrentCell.ColumnIndex == 1)
                {
                    comboCell.Items.Add("T1");
                    comboCell.Items.Add("T2");
                    comboCell.Items.Add("T3");
                    comboCell.Items.Add("T4");
                    comboCell.Items.Add("T5");
                    comboCell.Items.Add("T6");
                    comboCell.Items.Add("T7");
                    comboCell.Items.Add("T8");
                    comboCell.Items.Add("00");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 2)
                {
                    comboCell.Items.Add("00");
                    comboCell.Items.Add("01");
                    comboCell.Items.Add("02");
                    comboCell.Items.Add("03");
                    comboCell.Items.Add("04");
                    comboCell.Items.Add("05");
                    comboCell.Items.Add("06");
                    comboCell.Items.Add("07");
                    comboCell.Items.Add("08");
                    comboCell.Items.Add("09");
                    comboCell.Items.Add("10");
                    comboCell.Items.Add("11");
                    comboCell.Items.Add("12");
                    comboCell.Items.Add("13");
                    comboCell.Items.Add("14");
                    comboCell.Items.Add("15");
                    comboCell.Items.Add("16");
                    comboCell.Items.Add("17");
                    comboCell.Items.Add("18");
                    comboCell.Items.Add("19");
                    comboCell.Items.Add("20");
                    comboCell.Items.Add("21");
                    comboCell.Items.Add("22");
                    comboCell.Items.Add("23");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 3)
                {
                    comboCell.Items.Add("00");
                    comboCell.Items.Add("15");
                    comboCell.Items.Add("30");
                    comboCell.Items.Add("45");
                }
            }
            int rIndex = dataGrid.CurrentCell.RowIndex;
            int count = 0;
            if (rIndex != 0)
            {

                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "23" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "45")
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = true;
                        dataGrid.Rows[count].Cells[2].ReadOnly = true;
                        dataGrid.Rows[count].Cells[3].ReadOnly = true;
                    }
                    this.StatusMessage = "No more entries allowed as the day is complete";
                    isValidTOU = true;
                    return;
                }
                else
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = false;
                        dataGrid.Rows[count].Cells[2].ReadOnly = false;
                        dataGrid.Rows[count].Cells[3].ReadOnly = false;
                    }
                    this.StatusMessage = " ";
                    isValidTOU = true;
                }

                int grdRowIndex = 0;
                for (grdRowIndex = 1; grdRowIndex < 9; grdRowIndex++) //changed on 11/01/2011 grdRowIndex <= 9 changed to grdRowIndex < 9 
                {
                    if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[2].Value) == "00")
                        {
                            if ((Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) != "00") || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == 45) || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[grdRowIndex].Cells[3].Value)))
                            {
                                dataGrid.Rows[grdRowIndex].Cells[3].ReadOnly = true;
                                do
                                {
                                    dataGrid.Rows[grdRowIndex + 1].Cells[1].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[2].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[3].ReadOnly = true;
                                    grdRowIndex++;
                                } while (grdRowIndex != 9);
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 7; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true; // this line added on  11/01/2011
                            do
                            {
                                dataGrid.Rows[rIndex + 2].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 8);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 8; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex - 1].Cells[3].ReadOnly = true;
                            do
                            {
                                dataGrid.Rows[rIndex + 1].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 9);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex > 1)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                    {
                        string val = Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value);
                        string val1 = Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[1].Value);
                        if (val1 != "00" && val != "00")
                        {
                            do
                            {
                                dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 10);
                            isValidTOU = false;
                            return;
                        }
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    //isValidTOU = false;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    if (rIndex != 1 && (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "00"))
                    {
                        do
                        {
                            dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                            rIndex++;
                        } while (rIndex != 10);
                        isValidTOU = false;
                        return;
                    }
                    else
                    {
                        dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = true;
                    }
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    //isValidTOU = false;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00" && Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == "00")
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) != "00" || (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "45"))// && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == "00")//added on june 10
                    {
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = false;
                        return;
                    }
                }
                //else
                //{ 
                //   dataGrid.Rows[rIndex].Cells[3].ReadOnly = false ;
                //    isValidTOU = true;
                //}

                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 9)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value))
                        {
                            if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                            {
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                    if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 0)
                {
                    if (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }

            }
        }


        private bool validateGridCell(DataGridView dtView, object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "T1") && (e.FormattedValue.ToString() != "T2") && (e.FormattedValue.ToString() != "T3") && (e.FormattedValue.ToString() != "T4") && (e.FormattedValue.ToString() != "T5") && (e.FormattedValue.ToString() != "T6") && (e.FormattedValue.ToString() != "T7") && (e.FormattedValue.ToString() != "T8"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";

                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                            dtView.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                            dtView.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        }
                        //if (e.RowIndex != 0)
                        //{
                        //    if (e.FormattedValue.ToString() != "00" && (Convert.ToString(dtView.Rows[e.RowIndex].Cells[2].Value) == "00" && Convert.ToString(dtView.Rows[e.RowIndex].Cells[3].Value) == "00"))
                        //    {
                        //        e.Cancel = true;
                        //        isValidTOU = false;
                        //        return false;
                        //    }
                        //}
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else isValidTOU = true;
                        if (e.FormattedValue.ToString() != "" && (Convert.ToInt16(e.FormattedValue.ToString()) > 23))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }
                        if (e.RowIndex != 0)//added on june 10
                        {
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "45")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }

                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                        }
                        else isValidTOU = true;

                        //Shivangy included the condition on 26 May 2009
                        if (e.RowIndex != 0 && e.RowIndex != 9)
                        {

                            if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString() != "00")
                            {
                                if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;


                                if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value) || Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                        e.Cancel = true;
                                        isValidTOU = false;
                                    }
                                    else { isValidTOU = true; }
                                }
                            }

                            //*********

                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else isValidTOU = true;

                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() == "00" && dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "45")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = false;
                                    return false;
                                }
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    int currIndex = e.RowIndex;
                                    int rIndex = e.RowIndex + 1;
                                    if (rIndex < 10)
                                    {
                                        do
                                        {
                                            dtView.Rows[rIndex].Cells[1].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[2].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[3].ReadOnly = true;
                                            rIndex++;
                                        } while (rIndex != 10);
                                        isValidTOU = false;
                                        return false;
                                    }
                                }
                                else { isValidTOU = true; }
                            }
                        }
                        if (e.RowIndex == 9)
                        {
                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[2].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[2].Value) && Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[3].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[3].Value))
                            {
                                isValidTOU = false;
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "00") && (e.FormattedValue.ToString() != "15") && (e.FormattedValue.ToString() != "30") && (e.FormattedValue.ToString() != "45"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }

                        if (e.FormattedValue.ToString() != "" && e.RowIndex > 0)
                        {
                            int index = e.RowIndex;
                            if (index != 9)
                            {
                                while (index != 10)//Added on March 13
                                {
                                    if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 2].Value.ToString() != "00")
                                    {
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        else
                                        {
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                        }
                                    }
                                    index++;
                                }
                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                int rIndex = e.RowIndex + 1;
                                do
                                {
                                    dtView.Rows[rIndex].Cells[0].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[1].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[2].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[3].ReadOnly = false;
                                    rIndex++;
                                } while (rIndex != 10);
                                rIndex--;
                                this.StatusMessage = "";
                                isValidTOU = false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                dtView.Rows[e.RowIndex].ErrorText = "Invalid Value ";
                e.Cancel = true;
                isValidTOU = false;
                logger.Log(LOGLEVELS.Error, "validateGridCell(DataGridView dtView, object sender, DataGridViewCellValidatingEventArgs e)", ex);
                return false;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTOUFile();
        }

        private void SaveTOUFile()
        {
            //string touFilePath = string.Empty;

            //try
            //{
            //    if (isValidTOU == true)
            //    {
            //        string fileContent = CreateTOUCommand();
            //        if (fileContent != "")
            //        {
            //            if (!ValidateTOUData())
            //            {
            //                this.Cursor = Cursors.Default;
            //                return;
            //            }
            //            touFilePath = ConfigInfo.GetTOULocation();
            //            if (!Directory.Exists(touFilePath))
            //                Directory.CreateDirectory(touFilePath);
            //            FileStream fs = new FileStream(string.Concat(touFilePath, "/TouConfig.TOU"), FileMode.Create);
            //            StreamWriter sw = new StreamWriter(fs);
            //            sw.Write(fileContent);
            //            sw.Close();
            //            fs.Close();
            //            this.StatusMessage = "Data Saved Successfully";
            //        }
            //        else
            //        {
            //            this.StatusMessage = "No data available for saving ";
            //        }
            //    }
            //    else
            //    {
            //        foreach (DataGridView gridSeason in GetSeasonGridCollection())
            //        {
            //            if (gridSeason.Rows[0].Cells[1].Value.ToString() == "00")
            //            {
            //                this.StatusMessage = "Season slots not complete!";// "No data available for saving.";
            //                return;
            //            }
            //        }

            //        this.StatusMessage = "Invalid Entry!";// "No data available for saving.";
            //        return;


            //        //foreach (DataGridView gridHoliday in GetHolidayGridCollection())
            //        //{
            //        //    if (gridHoliday.Rows[0].Cells[1].Value.ToString() == "00")
            //        //    {
            //        //        this.StatusMessage = "No data available for saving.";
            //        //        return;
            //        //    }
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.StatusMessage = ex.Message;
            //    return;
            //}
        }


        private void btnResetAll_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            ResetAllGrids();
        }

        private void ResetAllGrids()
        {
            ResetSeasonGrids();
            ResetHolidayGrids();
            ResetDayAssignmentGrids();
            ResetFutureActivationGrid();
        }

        private void ResetSeasonGrids()
        {
            int rowCount = 0;
            foreach (DataGridView seasonGrid in GetSeasonGridCollection())
            {
                rowCount = 0;
                if (seasonGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        seasonGrid.Rows.Add();
                        seasonGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }

                foreach (DataGridViewRow row in seasonGrid.Rows)
                {
                    row.Cells["Start Hour"].Value = "00";
                    row.Cells["Start Minute"].Value = "00";
                    row.Cells["Rate"].Value = "00";
                }
            }
        }

        private void ResetHolidayGrids()
        {
            int rowCount = 0;
            foreach (DataGridView holidayGrid in GetHolidayGridCollection())
            {
                rowCount = 0;
                if (holidayGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        holidayGrid.Rows.Add();
                        holidayGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }
                foreach (DataGridViewRow row in holidayGrid.Rows)
                {
                    row.Cells["Start Hour"].Value = "00";
                    row.Cells["Start Minute"].Value = "00";
                    row.Cells["Rate"].Value = "00";
                }
            }

            foreach (DateTimePicker dtp in GetActivationDateCollection())
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                dtp.Value = System.DateTime.Now;
            }
        }

        private void ResetDayAssignmentGrids()
        {
            foreach (DataGridView assignmentGrid in GetAssignmentGridCollection())
            {
                if (assignmentGrid.Rows.Count == 0)
                {
                    assignmentGrid.Rows.Add(7);
                    assignmentGrid.Rows[0].Cells[0].Value = "Sunday";
                    assignmentGrid.Rows[1].Cells[0].Value = "Monday";
                    assignmentGrid.Rows[2].Cells[0].Value = "Tuesday";
                    assignmentGrid.Rows[3].Cells[0].Value = "Wednesday";
                    assignmentGrid.Rows[4].Cells[0].Value = "Thursday";
                    assignmentGrid.Rows[5].Cells[0].Value = "Friday";
                    assignmentGrid.Rows[6].Cells[0].Value = "Saturday";
                }
                foreach (DataGridViewRow row in assignmentGrid.Rows)
                {
                    row.Cells[1].Value = "Day Table 1";
                    row.Cells[1].Value = "Day Table 1";
                }
            }
        }

        private void ResetFutureActivationGrid()
        {
            double dayIndex = 0;
            dtPickerFutureActivationDate.Format = DateTimePickerFormat.Custom;
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
            dtPickerFutureActivationDate.Value = System.DateTime.Now.AddDays(++dayIndex);

            if (gridActivation.Rows.Count == 0)
                gridActivation.Rows.Add(4);
            int rIndex = 0;
            int startDay = 1;
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string sDay = startDay.ToString();
                if (sDay.Length < 2) { sDay = "0" + sDay; }
                DateTime dt = new DateTime(DateTime.Now.Year, 1, Int32.Parse(sDay));
                row.Cells[0].Value = dt;// Convert.ToDateTime(sDay + "/01/" + DateTime.Now.Year.ToString());
                startDay++;
                //row.Cells[0].Value = System.DateTime.Now.AddDays(++dayIndex);
                row.Cells[1].Value = "1";
            }
        }

        private void rdBtnAutoRTC_CheckedChanged(object sender, EventArgs e)
        {
            dtPickerRTC.Enabled = false;
            rtcTimer.Enabled = true;
        }

        private void rdBtnManualRTC_CheckedChanged(object sender, EventArgs e)
        {
            dtPickerRTC.Enabled = true;
            rtcTimer.Enabled = false;
        }

        //private void rtcTimer_Tick(object sender, EventArgs e)
        //{
        //    dtPickerRTC.Value = DateTime.Now;
        //}

        private void rdAll_CheckedChanged(object sender, EventArgs e)
        {
            InitializeAllCheckboxes();
            if (rdAll.Checked == true)
            {
                chkBoxKwh.Checked = true;
                chkBoxKvarhLag.Checked = true;
                chkBoxKvarhLead.Checked = true;
                chkBoxKVAh.Checked = true;
                chkBoxDailyMD1.Checked = true;
                chkBoxDailyMD2.Checked = true;
                chkBoxDailyMD3.Checked = true;
                chkBoxMaxAvgVoltage.Checked = true;
                chkBoxMinAvgVoltage.Checked = true;
                chkBoxMaxAvgCurrent.Checked = true;
                chkBoxMinAvgCurrent.Checked = true;
                chkBoxCumFundkWh.Checked = true;
            }
            else
            {
                InitializeAllCheckboxes();
            }
        }

        private void rdDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDefault.Checked == true)
            {
                chkBoxKwh.Checked = true;
                chkBoxKVAh.Checked = true;
                chkBoxDailyMD1.Checked = true;
                chkBoxDailyMD2.Checked = true;
                chkBoxDailyMD3.Checked = true;
                chkBoxMaxAvgVoltage.Checked = true;
                chkBoxMaxAvgCurrent.Checked = true;
            }
            else
            {
                chkBoxKwh.Checked = false;
                chkBoxKVAh.Checked = false;
                chkBoxDailyMD1.Checked = false;
                chkBoxDailyMD2.Checked = false;
                chkBoxDailyMD3.Checked = false;
                chkBoxMaxAvgVoltage.Checked = false;
                chkBoxMaxAvgCurrent.Checked = false;
                chkBoxCumFundkWh.Checked = false;
            }
        }


        private void GridAssignValidate(DataGridView AssignGrid, object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (AssignGrid[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string gridVal = e.FormattedValue.ToString();
                        if (gridVal == "")
                        {
                            AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            if (gridVal != "Day Table 1" && gridVal != "Day Table 2" && gridVal != "Day Table 3" && gridVal != "Day Table 4" && gridVal != "Day Table 5" && gridVal != "Day Table 6")
                            {
                                AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else
                            {
                                isValidTOU = true;
                                AssignGrid.Rows[e.RowIndex].ErrorText = "";
                            }
                        }
                    }
                }
            }
        }


        private void gridAssignmentS1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS1, sender, e);
        }

        private void gridAssignmentS2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS2, sender, e);
        }

        private void gridAssignmentS3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS3, sender, e);
        }

        private void gridAssignmentS4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS4, sender, e);
        }

        private void GridAssignClick(DataGridView AssignGrid)
        {
            if (AssignGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                if (AssignGrid.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell dtComboCell = new DataGridViewComboBoxCell();
                    dtComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    dtComboCell.Items.Add("Day Table 1");
                    dtComboCell.Items.Add("Day Table 2");
                    dtComboCell.Items.Add("Day Table 3");
                    dtComboCell.Items.Add("Day Table 4");
                    dtComboCell.Items.Add("Day Table 5");
                    dtComboCell.Items.Add("Day Table 6");
                    int rIndex = AssignGrid.CurrentCell.RowIndex;
                    int cIndex = AssignGrid.CurrentCell.ColumnIndex;
                    AssignGrid.Rows[rIndex].Cells[cIndex] = dtComboCell;
                }
            }
        }

        private void gridAssignmentS1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS1);
        }

        private void gridAssignmentS2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS2);
        }

        private void gridAssignmentS3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS3);
        }

        private void gridAssignmentS4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS4);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StatusMessage = "";
        }

        private void chkBoxKwh_CheckedChanged(object sender, EventArgs e)
        {
            chkAll.CheckedChanged -= chkAll_CheckedChanged;
            if (chkBoxDailyMD1.Checked == true && chkBoxDailyMD2.Checked == true && chkBoxKVAh.Checked == true &&
                chkBoxKvarhLag.Checked == true && chkBoxKvarhLead.Checked == true && chkBoxKwh.Checked == true)
                chkAll.Checked = true;
            else
                chkAll.Checked = false;
            chkAll.CheckedChanged += chkAll_CheckedChanged;
        }

        private void chkBoxKvarhLag_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxKwh_CheckedChanged(sender, e);
        }

        private void chkBoxKvarhLead_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxKwh_CheckedChanged(sender, e);
        }

        private void chkBoxKVAh_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxKwh_CheckedChanged(sender, e);
        }

        private void chkBoxDailyMD1_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxKwh_CheckedChanged(sender, e);
        }

        private void chkBoxDailyMD2_CheckedChanged(object sender, EventArgs e)
        {
            chkBoxKwh_CheckedChanged(sender, e);
        }

        private void MeterDataProgramming_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
            SetConnectionDetail(false);
        }

        /// <summary>
        /// gets meter config data from xml where meter model number and firmware version match.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="firmware"></param>
        /// <returns></returns>
        private MeterConfigSettingsMeterConfigElement GetMeterConfig(string meterModel, string firmware)
        {
            MeterConfigSettingsMeterConfigElement result = null;
            foreach (MeterConfigSettingsMeterConfigElement meterConfigSettingsElement in meterConfigSettings.Items)
            {
                if (meterConfigSettingsElement.MeterModel == meterModel.ToString() && meterConfigSettingsElement.Firmware == firmware.ToString())
                {
                    result = meterConfigSettingsElement;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lngMain_Load(object sender, EventArgs e)
        {
            enumData = new List<System.Enum>();
            MeterConfigSettingsMeterConfigElement element = GetMeterConfig(ConfigInfo.MeterModel, ConfigInfo.FirmwareVersion);

            if (element != null)
            {                
                if (Convert.ToBoolean(element.RTC))
                {
                    enumData.Add(ProfileId.RTC);
                }
                if (Convert.ToBoolean(element.BillingReset))
                {
                    enumData.Add(ProfileId.BillingReset);
                }
                if (Convert.ToBoolean(element.DailyLog))
                {
                    enumData.Add(ProfileId.DailyLog);
                }
                if (Convert.ToBoolean(element.MagneticTamperIcon))
                {
                    enumData.Add(ProfileId.MagneticTamperIcon);
                }
                if (Convert.ToBoolean(element.DTM))
                {
                    enumData.Add(ProfileId.DTM);
                }
                if (element.TOD.ToUpper() == FOURTOU)
                {
                    enumData.Add(ProfileId.FourTOU);
                }
                if (IsMeterType == 1 || IsMeterType == 2)
                {
                    //btnRead.Enabled = false;
                    if (Convert.ToBoolean(element.DIP))
                    {
                        enumData.Add(ProfileId.DIP);
                        cmbDIPDemandSubIntervalTime.Visible = false;
                        labelDIPSubDemandIntervalUnit.Visible = false;
                        labelDIPSubDemandInterval.Visible = false;
                        cmbDIPDemandType.SelectedIndex = 0;
                        cmbDIPDemandType.Enabled = false;
                    }
                    if (element.BillingType.ToString().ToUpper() == "OTHER")
                    {
                        enumData.Add(ProfileId.BillingType);
                        otherBillingType.Visible = true;
                        otherBillingType.Checked = true;
                        billingPeriodPanel.Visible = true;
                        label23.Visible = true;
                        label24.Visible = true;
                        cmbBoxBillingHour.Visible = true;
                        cmbBoxBillingMinute.Visible = true;
                        panelBillingMode.Enabled = false;
                    }
                    //if (element.BillingType.ToString().ToUpper() == "OTHER")
                    //{
                    //    enumData.Add(ProfileId.BillingType);
                    //    otherBillingType.Visible = true;
                    //    otherBillingType.Checked = true;
                    //    otherBillingType.Location = normalBillingType.Location;
                    //    billingPeriodPanel.Visible = true;
                    //    label23.Visible = false;
                    //    label24.Visible = false;
                    //    cmbBoxBillingHour.Visible = false;
                    //    cmbBoxBillingMinute.Visible = false;

                    //}
                    //enumData.Add(ProfileId.KvahSelection);
                }
                if (Convert.ToBoolean(element.DisplayParametersIEC))
                {
                    enumData.Add(ProfileId.DisplayParametersIEC);
                }
            }
            lngMain.AddEnumList(enumData, true);
            lngMain.DisableStatusColumn();
            lngMain.DeselectCheckBoxes();
        }
        /// <summary>
        /// Fill CFG File Data into controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            //LoadTabs();
            //listSelectedParams = new List<System.Enum>();
            //HideTabs(listSelectedParams);
            //lngMain.SelectUploadedParameters(listSelectedParams);
            this.StatusMessage = "";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "Configuration File";
            openFile.InitialDirectory = ConfigInfo.GetLocation();
            openFile.Filter = "Configuration file(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            lngMain.DeselectCheckBoxes();
            //timer_RTC.Start();
            // BindTOUGrids();
            //BindBillingTypeControls();
            LoadTabs();
            listSelectedParams = new List<System.Enum>();
            try
            {
                if (result == DialogResult.OK)
                {
                    if (DisplayConfigurationFromFile(openFile.FileName))
                    {
                        this.StatusMessage = "File uploaded successfully.";
                    }
                    HideTabs(listSelectedParams);
                }
            }

            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(" Invalid File ", "BCS");
                logger.Log(LOGLEVELS.Error, "btnUploadFile_Click(object sender, EventArgs e)", ex);
            }

            lngMain.SelectUploadedParameters(listSelectedParams);
        }

        /// <summary>
        /// Display configuration from file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool DisplayConfigurationFromFile(string fileName)
        {
            bool result = true;
            try
            {
                string fileContent = string.Empty;
                if (File.Exists(fileName))
                {
                    StreamReader streamReader = new StreamReader(fileName);
                    fileContent = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (!string.IsNullOrEmpty(fileContent))
                {

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayConfigurationFromFile(string fileName)", ex);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Write Configuration Data .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
            try
            {
                DisableControls();
                if (IsMeterType == 1)
                {
                    communications.Parity = System.IO.Ports.Parity.Even;
                    communications.DataBits = 7;
                }
                else if (IsMeterType == 2)
                {
                    communications.Parity = System.IO.Ports.Parity.None;
                    communications.DataBits = 8;
                }

                this.StatusMessage = "";
                string password = string.Empty;
                List<System.Enum> selectedProfiles;
                if (CheckValidations("write"))
                {
                   validationMessage = ValidateConfiguration("write");
                    if (validationMessage.Length == 0)
                    {
                        selectedProfiles = GetSelectedProfileId("write");
                        //Display warning message if user writes daily log .
                        if (selectedProfiles.Contains(ProfileId.DailyLog))
                        {
                       if (!MessageBox.Show("Writting daily log will delete all daily log data.Are you sure want to continue?","BCS", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                            {
                                selectedProfiles.Remove(ProfileId.DailyLog);
                            }

                        }
                        if (selectedProfiles.Count > 0)
                        {
                            if (commType == CommunicationType.GSM)
                            {
                                if (!ValidateSimNumber())
                                {
                                    return;
                                }
                                        else
                                {
                                    OpenPasswordDialog();
                                    if (meterPswd.Length == 0)
                                        return;
                                }
                            }
                            if (oneToOneGSM.Checked && commType == CommunicationType.GSM)
                            {
                                WriteOneToOne();
                            }
                            else if (oneToManyGSM.Checked && commType == CommunicationType.GSM)
                            {
                               // WriteOneToMany();
                        }


                            if (commType == CommunicationType.DIRECT)
                            {
                                OpenPasswordDialog();
                                if (meterPswd.Length == 0)
                                    return;
                                WriteMeterConfig_IEC(selectedProfiles, false, 0);
                              
                    }

                        }
                    else
                    {
                        MessageBox.Show(validationMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "Failure in Writting Configuration(s)";
                logger.Log(LOGLEVELS.Error, "btnWrite_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                //Cleanup
                EnableControls(mode);
                SetConnectionDetail(false);
            }
        }


        /// <summary>
        /// Get Meter password form password dialog .
        /// </summary>
        private void OpenPasswordDialog()
        {
            this.meterPswd = string.Empty;
            this.ctRatio = 0;
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            meterPassword.ShowDialog();
            Application.DoEvents();
        }
        /// <summary>
        /// Used to reset Magnetic tamper Icon  .
        /// </summary>
        private void ResetMagneticTamperIcon()
        {
            //this.StatusMessage = string.Empty;
            MagneticTamperInformation magneticTamperInformation = new MagneticTamperInformation();
            magneticTamperInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                magneticTamperInformation.MeterPassword = meterPswd;
                magneticTamperInformation.Channel = communications;
                magneticTamperInformation.ResetMagneticTamper();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ResetMagneticTamperIcon()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }
       private void ResetMagneticTamperIconSP()
        {
            //this.StatusMessage = string.Empty;
            //MagneticTamperInformationForSP magneticTamperInformation = new MagneticTamperInformationForSP();
            TOUInformationSP magneticTamperInformation = new TOUInformationSP();
            magneticTamperInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                magneticTamperInformation.MeterPassword = meterPswd;
                magneticTamperInformation.Channel = communications;
                magneticTamperInformation.ResetMagneticTamperSP();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ResetMagneticTamperIconSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void ResetDemandIntegrationPeriodSP()
        {
            //DIPForSP DIP = new DIPForSP();
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;
                string cmd = "";
                if (cmbDIPDemandInterval.Text.Contains("15"))
                {
                    cmd = "0C";
                }
                else if (cmbDIPDemandInterval.Text.Contains("30"))
                {
                    cmd = "2C";
                }
                else
                {

                }
                touInformationSP.ResetDIP(cmd);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ResetDemandIntegrationPeriodSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void UpdateBillingTypeSP()
        {
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;

                string data = GetWishListData();
                touInformationSP.UpdateBillingType(data);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "UpdateBillingTypeSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        public string GetWishListData()
        {
            try
            {
                string data1 = "";
                data1 = "00000";

                if (cmbBoxBillingPeriod.SelectedIndex == 0) { data1 = "0" + data1; }
                else if (cmbBoxBillingPeriod.SelectedIndex == 1) { data1 = "1" + data1; }

                if (evenMonthBilling.Checked) data1 = "00" + data1; //0
                else if (oddMonthBilling.Checked) data1 = "01" + data1; //1
                else if (monthlyBilling.Checked) data1 = "10" + data1; //2

                while (data1.Length < 8) data1 = "0" + data1;
                data1 = BinarytoHex(data1);

                string data2 = (Convert.ToInt32(cmbBoxBillingDate.Text)).ToString("X");
                if (data2.Length < 2) { data2 = "0" + data2; }
                //data2 = ReadoutCommon.DTMStringToHex(data2);

                string data3 = (Convert.ToInt32(cmbBoxBillingHour.Text)).ToString("X");
                if (data3.Length < 2) { data3 = "0" + data3; }
                //data3 = ReadoutCommon.StrToHexCmd(data3);

                string data4 = (Convert.ToInt32(cmbBoxBillingMinute.Text)).ToString("X");
                if (data4.Length < 2) { data4 = "0" + data4; }
                //data4 = ReadoutCommon.StrToHexCmd(data4);

                return data1 + data2 + data3 + data4;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetWishListData()", ex);
                return "";
            }
        }
        private string BinarytoHex(string binData)
        {
            try
            {
                string data = "";
                data = Convert.ToInt64(binData, 2).ToString("X");//binary to hex
                while (data.Length < 2) data = "0" + data;
                //data = ReadoutCommon.DTMStringToHex(data);
                return data;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BinarytoHex(string binData)", ex);
                return "";
            }
        }
        /// <summary>
        /// used to Update protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "IEC" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
        /// <summary>
        /// Used to write TOD
        /// </summary>
        private void WriteTOU()
        {
            List<string> touCommands;
            TOUInformation touInformation = new TOUInformation();
            touInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                touInformation.MeterPassword = meterPswd;
                touInformation.Channel = communications;

                touCommands = GetTOUCommands();
                touInformation.SetTOU(touCommands);
                //this.StatusMessage = "TOU set successfully!";
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteTOU()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
            }
        }
        /// <summary>
        /// Used to write Dialy log parameters .
        /// </summary>
        private void WriteDailyLog()
        {

            DTMProgramming dtmProgramming = new DTMProgramming();
            dtmProgramming.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                if (ValidateDailyParams() == false)
                    return;
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                dtmProgramming.MeterPassword = meterPswd;
                dtmProgramming.Channel = communications;
                dtmProgramming.DailyParamsValue = GetDailyLogCommand();
                this.Cursor = Cursors.WaitCursor;

                //2 march 2012: Command added to reset the Daily log data after configuring the parameters.
                //dtmProgramming.WriteDTMDailyLog();
                if (dtmProgramming.WriteDTMDailyLog())
                {
                    dtmProgramming.ResetDTMDailyLog();
                    this.statusMsg = dtmProgramming.StatusMessage;
                }
                else
                    this.statusMsg = dtmProgramming.StatusMessage;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "WriteDailyLog()", ex);
            }
            finally
            {
                //2 march 2012: ClosePort() added to close the port after communication
                communications.DelayExecution();
                communications.ClosePort();
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }

        }
        /// <summary>
        /// Used to write DTM LPR parameters .
        /// </summary>
        private void WriteDTM()
        {
            DTMProgramming dtmProgramming = new DTMProgramming();
            dtmProgramming.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                if (ValidateLPRParameters() == false)
                {
                    return;
                }
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                dtmProgramming.MeterPassword = meterPswd;
                dtmProgramming.Channel = communications;
                dtmProgramming.HighLoadThreshold = Convert.ToInt32(txtBoxHighLoad.Text.Trim()).ToString("X");
                dtmProgramming.LowLoadThreshold = Convert.ToInt32(txtBoxLowLoad.Text.Trim()).ToString("X");
                dtmProgramming.TransformerRating = Convert.ToInt32(txtBoxTransformerRating.Text.Trim()).ToString("X");
                dtmProgramming.WriteLPRParameters();
                // this.StatusMessage = "DTM LPR parameters written successfully.";
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteDTM()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }
        /// <summary>
        /// Used to update meter RTC.
        /// </summary>
        private void UpdateRTC()
        {
            // this.StatusMessage = string.Empty;
            RTCInformation rtcInformation = new RTCInformation();
            rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);

            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            string meterRTCData = string.Empty;
            DateTime rtc;
            try
            {
                InitializeProgrammingValues();
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                rtcInformation.RTCDateTime = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                            "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); ;
                rtcInformation.Channel = communications;

                meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                if (!string.IsNullOrEmpty(statusMsg))
                {
                    this.StatusMessage = statusMsg;
                }
                if (statusMsg != "Invalid RTC." && meterRTCData.Length == 0)
                    return;
                meterRTCData = meterRTCData.Substring(meterRTCData.IndexOf("\n") + 3, 12);
                meterRTCData = meterRTCData.Substring(0, 2) + "/" + meterRTCData.Substring(2, 2) + "/" + meterRTCData.Substring(4, 2) + " " + meterRTCData.Substring(6, 2) + ":" + meterRTCData.Substring(8, 2) + ":" + meterRTCData.Substring(10, 2);

                rtc = ProgrammingCommon.GetDateWithTime(meterRTCData);
                if (statusMsg == "Invalid RTC.")
                {

                    rtcInformation.MeterPassword = meterPswd;
                    rtcInformation.SetRTC(meterPswd);
                }
                else if (rtcInformation.ValidateRTC(rtc))
                {
                    rtcInformation.MeterPassword = meterPswd;
                    rtcInformation.SetRTC(meterPswd);
                }
                else
                {
                    this.StatusMessage = "RTC backforce of more than 15 minutes is not allowed.";
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "UpdateRTC()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }


        private void WriteTOUForSPhase()
        {
            List<string> touCommands;
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;

                touCommands = GetTOUCommandsSP();
                touInformationSP.SetTOU(touCommands);
                //this.StatusMessage = "TOU set successfully!";
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteTOUForSPhase()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
            }
        }


        /// <summary>
        /// Used to update meter RTC for Single Phase.
        /// </summary>
        private void UpdateRTCForSPhase()
        {
            //RTCInformationForSP rtcInformation = new RTCInformationForSP();
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new TOUInformationSP.ChannelStatusChanged(Channel_OnStatusChanged);

            string meterRTCData = string.Empty;
          //  DateTime rtc;
            try
            {
                InitializeProgrammingValues();
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                touInformationSP.RTCDateTime = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                            "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); ;
                touInformationSP.Channel = communications;
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.SetRTC(meterPswd);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "UpdateRTCForSPhase()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }
        /// <summary>
        ///Used to reset billing .
        /// </summary>
        private void WriteBillingReset()
        {
            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as IECLocalCommunication;
            }
            else
            {
                communications = ChannelManager.GetChannel() as GSMCommunication;
            }

            //this.StatusMessage = string.Empty;
            BillingInformation billingInformation = new BillingInformation();
            billingInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                billingInformation.MeterPassword = meterPswd;
                billingInformation.Channel = communications;
                billingInformation.ResetBilling();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteBillingReset()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        /// <summary>
        ///Used to reset billing For Single Phase.
        /// </summary>
        private void WriteBillingResetForSP()
        {
            //if (!ConfigInfo.IsGSMConnected())
            //{
            //    communications = ChannelManager.GetChannel() as IECLocalCommunication;
            //}
            //else
            //{
            //    communications = ChannelManager.GetChannel() as GSMCommunication;
            //}

            //this.StatusMessage = string.Empty;
           // BillingInformationForSP billingInformation = new BillingInformationForSP();
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new RTCInformationForSP.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                // meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;
                touInformationSP.ResetBilling();

            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteBillingResetForSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }
        /// <summary>
        /// Validate configuration data nefore writing.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string ValidateConfiguration(string action)
        {
            StringBuilder errorMessage = new StringBuilder();
            try
            {

                List<System.Enum> listSelected = new List<System.Enum>();
                listSelected.AddRange(lngMain.GetSelectedProfilesList<System.Enum>(enumData));

                if (listSelected.Contains(ProfileId.BillingReset))
                {
                    if (!chkMDReset.Checked)
                    {
                        errorMessage.Append("Please select billing reset checkbox." + Symbols.NEWLINE);
                        this.StatusMessage = "Please select billing reset checkbox.";
                    }
                }

                if (listSelected.Contains(ProfileId.DIP ))
                {
                    if (cmbDIPDemandInterval.SelectedIndex == -1)
                    {
                        errorMessage.Append("Please select Demand integration value." + Symbols.NEWLINE);
                        this.StatusMessage = "Please select Demand integration value.";
                    }
                }
                if (listSelected.Contains(ProfileId.BillingType))
                {
                    if (cmbBoxBillingPeriod.SelectedIndex == -1)
                    {
                        errorMessage.Append("Please select billing type value." + Symbols.NEWLINE);
                        this.StatusMessage = "Please select billing type value.";
                    }
                }

                if (listSelected.Contains(ProfileId.MagneticTamperIcon))
                {
                    if (!checkBoxMagneticTamperIcon.Checked)
                    {
                        errorMessage.Append("Please select Tamper Reset checkbox." + Symbols.NEWLINE);
                    }
                }
                if (listSelected.Contains(ProfileId.FourTOU))
                {

                    if (!isValidTOU)
                    {
                        errorMessage.Append("Invalid TOU Entry." + Symbols.NEWLINE);

                    }
                    string touMessage = ValidateTOUData();
                    if (touMessage != string.Empty)
                    {
                        errorMessage.Append(touMessage);
                    }
                }
                if (listSelected.Contains(ProfileId.DailyLog))
                {

                    if (chkBoxMinAvgCurrent.Checked == false && chkBoxMaxAvgCurrent.Checked == false && chkBoxMinAvgVoltage.Checked == false && chkBoxMaxAvgVoltage.Checked == false && chkBoxDailyMD1.Checked == false && chkBoxDailyMD2.Checked == false && chkBoxDailyMD3.Checked == false && chkBoxKwh.Checked == false && chkBoxKvarhLag.Checked == false && chkBoxKvarhLead.Checked == false && chkBoxKVAh.Checked == false && chkBoxCumFundkWh.Checked == false)
                    {
                        errorMessage.Append("Please select a daily log parameter to configure." + Symbols.NEWLINE);
                    }
                }

                if (listSelected.Contains(ProfileId.DTM))
                {

                    if (string.IsNullOrEmpty(txtBoxHighLoad.Text.Trim()))
                    {
                        errorMessage.Append("DTM Programming : High load value can not be left blank." + Symbols.NEWLINE);
                    }
                    else if (!ValidationProvider.ValidateData(txtBoxHighLoad.Text.Trim(), @"^(0{0,2}[1-9]|0?[1-9][0-9]|[1][0-9][0-9]|[2][0][0])$"))
                    {
                        errorMessage.Append("DTM Programming : Valid range for high load threshold is 1-200" + Symbols.NEWLINE);
                    }
                    if (string.IsNullOrEmpty(txtBoxLowLoad.Text.Trim()))
                    {
                        // this.StatusMessage = "Low load value can not be left blank.";
                        errorMessage.Append("DTM Programming : Low load value can not be left blank." + Symbols.NEWLINE);

                    }
                    else if (!ValidationProvider.ValidateData(txtBoxLowLoad.Text.Trim(), @"^(0{0,2}[0-9]|0?[1-9][0-9]|[1][0][0])$"))
                    {
                        //this.StatusMessage = "Valid range for low load threshold is 0-100";
                        errorMessage.Append("DTM Programming : Valid range for low load threshold is 0-100" + Symbols.NEWLINE);

                    }

                    if (string.IsNullOrEmpty(txtBoxTransformerRating.Text.Trim()))
                    {
                        // this.StatusMessage = "Transformer rating can not be left blank.";
                        errorMessage.Append("Transformer rating can not be left blank." + Symbols.NEWLINE);
                    }
                    else if (!ValidationProvider.ValidateData(txtBoxTransformerRating.Text.Trim(), @"^(0{0,2}[1-9]|0?[1-9][0-9]|[1-6][0-9][0-9]|[7][0][0])$"))
                    {
                        this.StatusMessage = "Valid range for transformer rating is 1-700";
                        errorMessage.Append("Valid range for transformer rating is 1-700" + Symbols.NEWLINE);

                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateConfiguration(string action)", ex);
                throw;
            }
            return errorMessage.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool CheckValidations(string action)
        {
            bool result = true;
            try
            {
                List<System.Enum> listSelected = new List<System.Enum>();
                listSelected.AddRange(lngMain.GetSelectedProfilesList<System.Enum>(enumData));
                if (listSelected.Count == 0)
                {
                    MessageBox.Show("Please select at least one option.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    result = false;
                }
                if ((listSelected.Contains(ProfileId.BillingReset) || listSelected.Contains(ProfileId.MagneticTamperIcon)
                    || listSelected.Contains(ProfileId.DailyLog) || listSelected.Contains(ProfileId.DIP) || listSelected.Contains(ProfileId.BillingType)) && action == "read")
                {
                    if (!listSelected.Contains(ProfileId.FourTOU) && !listSelected.Contains(ProfileId.DTM) && listSelected.Contains(ProfileId.RTC))
                    {
                        MessageBox.Show("Selected option(s) cannot be read", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result = false;
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                result = false;
                logger.Log(LOGLEVELS.Error, "CheckValidations(string action)", ex);
            }
            return result;
        }
        /// <summary>
        /// Read Configuration data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;

            try
            {

                DisableControls();

                if (IsMeterType == 1)
                {
                    communications.Parity = System.IO.Ports.Parity.Even;
                    communications.DataBits = 7;
                }
                else if (IsMeterType == 2)
                {
                    communications.Parity = System.IO.Ports.Parity.None;
                    communications.DataBits = 8;
                 
                }
                this.StatusMessage = "";
                Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
               // pnConfigOptions.Enabled = false;
                
                if (CheckValidations("read"))
                {
                    List<System.Enum> selectedProfiles; selectedProfiles = GetSelectedProfileId("read");
                    if (commType == CommunicationType.GSM)
                    {
                        if (!ValidateSimNumber())
                        {
                            return;
                                }
                                }

                    if (oneToOneGSM.Checked && commType == CommunicationType.GSM)
                                {
                        ReadOneToOne();
                                }
                    else if (oneToManyGSM.Checked && commType == CommunicationType.GSM)
                                {
                       // ReadOneToMany();
                                }

                     if (commType == CommunicationType.DIRECT)
                    {
                       GetMeterConfigData(selectedProfiles, false, 0);
                   
                    // btnCreateCfgFile_Click(sender, e);
                }
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "Read Configuration Fail.";
                Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                logger.Log(LOGLEVELS.Error, "btnRead_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                Application.DoEvents();
                pnConfigOptions.Enabled = true;
               EnableControls(mode);
                this.Cursor = Cursors.Default;
                SetConnectionDetail(false);
            }
       
        }

        private void ReadDemandIntegrationPeriodSP()
        {
            //this.StatusMessage = string.Empty; // Story - 354382 - Status message is not getting refreshed while reading parameters
            string dipData = string.Empty;

            try
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
                Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
                touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                touInformationSP.Channel = communications;
                dipData = touInformationSP.DemandIntegrationPeriod(ref statusMsg);

                //1 Phase


                //DIPForSP dipInfo = new DIPForSP();
                //dipInfo.OnChannelStatusChanged += new DIPForSP.ChannelStatusChanged(Channel_OnStatusChanged);
                //InitializeProgrammingValues();
                //dipInfo.Channel = communications;
                //this.Cursor = Cursors.WaitCursor;
                //dipData = dipInfo.DemandIntegrationPeriod(ref statusMsg);
                DisplayDIPSP(dipData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadDemandIntegrationPeriodSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void DisplaykVAhSelectionSP(string mdData)
        {
            try
            {
                mdData = mdData.Substring(mdData.IndexOf('(') + 1, mdData.IndexOf(')') - mdData.IndexOf('('));
                if (mdData.Length >= 12)
                {
                    string binaryval = Convert.ToString(Convert.ToInt32(mdData.Substring(2, 2), 16), 2).PadLeft(8, '0');//convert to binary

                    if ("1" == binaryval.Substring(7, 1)) rdbKVAhLagOnly.Checked = true;
                    else rdbKVAhLagLead.Checked = true;

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplaykVAhSelectionSP(string mdData)", ex);
            }
        }

        private void DisplayDIPSP(string mdData)
        {
            try
            {
                if (mdData.Length > 2)
                {
                    string binaryval = Convert.ToString(Convert.ToInt32(mdData.Substring(mdData.IndexOf("(") + 1, 2), 16), 2);//convert to binary
                    while (binaryval.Length < 8) { binaryval = "0" + binaryval; }
                    if (binaryval.Substring(1, 1) == "0")
                    {
                        //rdLSIP30.Checked = true;
                    }
                    else
                    {
                        //rdLSIP15.Checked = true;
                    }
                    if (binaryval.Substring(2, 2) == "00")
                    {
                        //rdInterval15.Checked = true;
                        if(cmbDIPDemandInterval.Items.Count > 0)
                        {
                            cmbDIPDemandInterval.SelectedIndex = 0;
                        }
                    }
                    else if (binaryval.Substring(2, 2) == "10")
                    {
                        //rdInterval30.Checked = true;
                        if (cmbDIPDemandInterval.Items.Count > 1)
                        {
                            cmbDIPDemandInterval.SelectedIndex = 1;
                        }
                    }
                    else if (binaryval.Substring(2, 2) == "11")
                    {
                        //rdInterval60.Checked = true;
                    }
                    string stVal = Convert.ToString(Convert.ToInt32(binaryval.Substring(4, 4), 2), 16);
                    //cmbStoredMDValues.Text = Convert.ToInt32(stVal, 16).ToString();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayDIPSP(string mdData)", ex);
            }        
        }

        private void ReadAndDisplayRTC()
        {
            //this.StatusMessage = string.Empty; // Story - 354382 - Status message is not getting refreshed while reading parameters
            string meterRTCData = string.Empty;

            if (IsMeterType == 1 || IsMeterType == 2)
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
                Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
                touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                touInformationSP.Channel = communications;
                meterRTCData = touInformationSP.GetRTC(ref statusMsg);
            }
            else
            {
                RTCInformation rtcInformation = new RTCInformation();
                rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
                InitializeProgrammingValues();
                rtcInformation.Channel = communications;
                this.Cursor = Cursors.WaitCursor;
                meterRTCData = rtcInformation.GetRTC(ref statusMsg);
            }
            DateTime rtc;
            try
            {
                if (meterRTCData.Length <= 10)
                {
                    this.StatusMessage = statusMsg;
                    return;
                }
                meterRTCData = meterRTCData.Substring(meterRTCData.IndexOf("\n") + 3, 12);
                if (IsMeterType == 1 || IsMeterType == 2)
                {
                    meterRTCData = meterRTCData.Substring(6, 2) + "/" + meterRTCData.Substring(8, 2) + "/" + meterRTCData.Substring(10, 2) + " " + meterRTCData.Substring(4, 2) + ":" + meterRTCData.Substring(2, 2) + ":" + meterRTCData.Substring(0, 2);
                }
                else
                {
                    meterRTCData = meterRTCData.Substring(0, 2) + "/" + meterRTCData.Substring(2, 2) + "/" + meterRTCData.Substring(4, 2) + " " + meterRTCData.Substring(6, 2) + ":" + meterRTCData.Substring(8, 2) + ":" + meterRTCData.Substring(10, 2);
                }
                if (!DateTime.TryParse(meterRTCData, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out rtc))
                    this.StatusMessage = "Invalid RTC";
                else
                {
                    DataGridView dgvRTC = rtcCtrl.Controls[0].Controls["dGridRTC"] as DataGridView;
                    dgvRTC.Rows.Add();
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn1"].Value = dgvRTC.RowCount;
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn2"].Value = meterRTCData;

                    if (!listSelectedParams.Contains(ProfileId.RTC))
                    {
                        listSelectedParams.Add(ProfileId.RTC);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadAndDisplayRTC()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }


        private void DisplayBillingTypeSP(string data)
        {
            try
            {
                if (data.Length > 2)
                {
                    //first byte 
                    string wishListData = "";
                    //wishListData = data.Substring(data.IndexOf("(") + 1, 32);
                    wishListData = data.Substring(data.IndexOf("(") + 1, 12); //User Story 478249. Billing Type generic 6 byte data 
                    string binaryval = Convert.ToString(Convert.ToInt32(wishListData.Substring(0, 2), 16), 2);
                    while (binaryval.Length < 8)
                    {
                        binaryval = "0" + binaryval;
                    }

                    // Scroll Mode, Reverse Mode

                    if (binaryval.Substring(5, 1) == "1")
                    {
                        //chkScrollMode.Checked = true; scr_en_cb.Enabled = true;
                    }
                    else
                    {
                        //chkScrollMode.Checked = false; scr_en_cb.Enabled = false;
                    }
                    if (binaryval.Substring(4, 1) == "1")
                    {
                        //chkReverseMode.Checked = true; cmb_RevEnabled.Enabled = true;
                    }
                    else
                    {
                        //chkReverseMode.Checked = false; cmb_RevEnabled.Enabled = false;
                    }
                    //
                    //Billing Mode
                    if (binaryval.Substring(3, 1) == "1")
                    {
                        //signon_cb.Checked = true;
                    }
                    else
                    {
                        //signon_cb.Checked = false;
                    }
                    if (binaryval.Substring(2, 1) == "1")
                    {
                        //rdProgBilling.Checked = true;
                        cmbBoxBillingPeriod.SelectedIndex = 1;
                    }
                    else
                    {
                        cmbBoxBillingPeriod.SelectedIndex = 0;
                    }

                    //bill Cycle
                    string billcycle = binaryval.Substring(0, 2);
                    billcycle = Convert.ToInt32(billcycle, 2).ToString();
                    if (billcycle == "0")
                    {
                        evenMonthBilling.Checked = true;
                    }
                    else if (billcycle == "1")
                    {
                        oddMonthBilling.Checked = true;
                    }
                    else
                    {
                        monthlyBilling.Checked = true;
                    }
                    //second byte

                    string billDate = (Convert.ToInt32(wishListData.Substring(2, 2), 16)).ToString();
                    while (billDate.Length < 2)
                    {
                        billDate = "0" + billDate;
                    }
                    cmbBoxBillingDate.Text = billDate;

                    //third byte
                    string billHour = (Convert.ToInt32(wishListData.Substring(4, 2), 16)).ToString();
                    while (billHour.Length < 2)
                    {
                        billHour = "0" + billHour;
                    }
                    cmbBoxBillingHour.Text = billHour;

                    //forth byte
                    string billMin = (Convert.ToInt32(wishListData.Substring(6, 2), 16)).ToString();
                    while (billMin.Length < 2)
                    {
                        billMin = "0" + billMin;
                    }
                    cmbBoxBillingMinute.Text = billMin;

                    //Fifth byte
                    string HREnabled = Convert.ToString(Convert.ToInt32(wishListData.Substring(8, 2), 16), 2);
                    while (HREnabled.Length < 8)
                    {
                        HREnabled = "0" + HREnabled;
                    }
                    //cmb_RevEnabled.Text = Convert.ToString(Convert.ToInt32(HREnabled.Substring(0, 4), 2));
                    //cmb_HREnabled.Text = Convert.ToString(Convert.ToInt32(HREnabled.Substring(4, 4), 2));
                    //sixth Byte
                    string HRTimeOut = Convert.ToString(Convert.ToInt32(wishListData.Substring(10, 2), 16), 2);
                    while (HRTimeOut.Length < 8)
                    {
                        HRTimeOut = "0" + HRTimeOut;
                    }
                    //scr_en_cb.Text = Convert.ToString(Convert.ToInt32(HRTimeOut.Substring(0, 3), 2) + 3);

                    //cmb_HRTimeout.Text = Convert.ToString(Convert.ToInt32(HRTimeOut.Substring(3, 5), 2));
                    //cmb_HRTimeout.Text = Convert.ToString(Convert.ToInt32(HRTimeOut, 2));


                   /* int decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(12, 2)));
                    etamper_occ_cb.Text = decValue.ToString();

                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(14, 2)));
                    etamper_res_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(16, 2)));
                    rev_occ_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(18, 2)));
                    rev_res_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(20, 2)));
                    mag_occ_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(22, 2)));
                    mag_res_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(24, 2)));
                    sw_occ_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(26, 2)));
                    sw_res_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(28, 2)));
                    ol_occ_cb.Text = decValue.ToString();
                    decValue = Convert.ToInt32(objSerialComm.HexToDecimalConversion(wishListData.Substring(30, 2)));
                    ol_res_cb.Text = decValue.ToString();*/


                    //rest of the bytes are unused
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayBillingTypeSP(string data)", ex);
            }
        }


        private void ReadAndDisplayBillingTypeSP()
        {
            //this.StatusMessage = string.Empty; // Story - 354382 - Status message is not getting refreshed while reading parameters
            string billingTypeData = string.Empty;

            try
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
                Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
                touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);

                
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                 
                Application.DoEvents();
                
                touInformationSP.Channel = communications;
                billingTypeData = touInformationSP.GetBillingType(ref statusMsg);
                
                
                DisplayBillingTypeSP(billingTypeData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadAndDisplayBillingTypeSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadAndDisplayTOU()
        {
            TOUInformation touInformation = new TOUInformation();
            touInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            string touData = string.Empty;
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                //meterPassword.ShowDialog();
                Application.DoEvents();
                //if (meterPswd.Length == 0)
                //{
                //    this.Cursor = Cursors.Default;
                //    return;
                //}
                //touInformation.MeterPassword = meterPswd;
                touInformation.Channel = communications;
                touData = touInformation.GetTOU();
                if (touData != "")
                {
                    DisplayTOU(touData);
                    // this.StatusMessage = "TOU Readout successful!";
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadAndDisplayTOU()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReadAndDisplayTOUSP()
        {
            TOUInformationSP touInformationSP = new TOUInformationSP();
            Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            string touData = string.Empty;
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                //meterPassword.ShowDialog();
                Application.DoEvents();
                //if (meterPswd.Length == 0)
                //{
                //    this.Cursor = Cursors.Default;
                //    return;
                //}
                //touInformation.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;

                touData = touInformationSP.GetTOUSP(dicTOUSP);
                if (touData != "")
                {
                    DisplayTOUSP(touData, dicTOUSP);
                    // this.StatusMessage = "TOU Readout successful!";
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadAndDisplayTOUSP()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
                this.Cursor = Cursors.Default;
            }
        }

        private void SetTOUSP_SeasonTable(Dictionary<int, string> dicChild)
        {
            int index = 0;
            gridDayTables.Rows.Clear();
            gridActivationDate.Rows.Clear();
            foreach (KeyValuePair<int, string> item in dicChild)
            {
                index++;
                string BufferValue = item.Value;
                int initPos = BufferValue.IndexOf("(");
                int finPos = BufferValue.IndexOf(")");
                string ResultValue = BufferValue.Substring((initPos + 1), (finPos - initPos));
                if (ResultValue.Substring(0, 1) != "0")
                {
                    gridDayTables.Rows.Add(index.ToString(),
                        ResultValue.Substring(0, 1),
                        ResultValue.Substring(1, 1),
                        ResultValue.Substring(2, 1),
                        ResultValue.Substring(3, 1),
                        ResultValue.Substring(4, 1),
                        ResultValue.Substring(5, 1),
                        ResultValue.Substring(6, 1)
                        );
                    gridActivationDate.Rows.Add(
                         ResultValue.Substring(7, 2),
                          ResultValue.Substring(9, 2));
                }
                else
                {
                    gridDayTables.Rows.Add(index.ToString(),
                        "",
                        "",
                        "",
                        "",
                        "",
                        "",
                        "");
                    gridActivationDate.Rows.Add(
                       "",
                        "");
                }
            }

        }

        private void SetTOUSP_DayTable(string GridName, Dictionary<int, string> dicChild)
        {
            DataGridView dgv = this.Controls.Find(GridName, true).FirstOrDefault() as DataGridView;
            if (dgv != null)
            {
                int index = 0;
                if (dgv.Rows != null)
                {
                    dgv.Rows.Clear();
                }
                foreach (KeyValuePair<int, string> item in dicChild)
                {
                    index++;
                    string BufferValue = item.Value;
                    int initPos = BufferValue.IndexOf("(");
                    int finPos = BufferValue.IndexOf(")");
                    string ResultValue = BufferValue.Substring((initPos + 1), (finPos - initPos));
                    string tamp = ResultValue.Substring(0, 1);
                    string hrs = ResultValue.Substring(1, 2);
                    string mins = ResultValue.Substring(3, 2);
                    if (Convert.ToInt16(tamp) != 0)
                    {
                        dgv.Rows.Add(index.ToString(), "T" + tamp, hrs, mins);
                    }
                    else
                    {
                        dgv.Rows.Add(index.ToString(), null, null, null);
                    }
                }              
            }
        }

        void SetValuesInGrids(Dictionary<string, Dictionary<int, string>> dicTOUSP)
        {
            const string R_D1 = "015234024434303128290317";
            const string R_D2 = "015234024434303228290314";
            const string R_D3 = "015234024434303328290315";
            const string R_D4 = "015234024434303428290312";
            const string R_D5 = "015234024434303028290316";

            foreach (KeyValuePair<string, Dictionary<int, string>> ParentPair in dicTOUSP)
            {
                switch (ParentPair.Key)
                {
                    case R_D1:
                        SetTOUSP_DayTable(gridTOUDay1_name, ParentPair.Value);
                        break;

                    case R_D2:
                        SetTOUSP_DayTable(gridTOUDay2_name, ParentPair.Value);
                        break;

                    case R_D3:
                        SetTOUSP_DayTable(gridTOUDay3_name, ParentPair.Value);
                        break;

                    case R_D4:
                        SetTOUSP_DayTable(gridTOUDay4_name, ParentPair.Value);
                        break;

                    case R_D5:
                        SetTOUSP_SeasonTable(ParentPair.Value);
                        break;

                    default:
                        break;
                }
            }

        }


        private void InitializeGrid(string GridName)
        {
            try
            {

                DataGridView dgv = this.Controls.Find(GridName, true).FirstOrDefault() as DataGridView;
                if (dgv != null)
                {
                    if (dgv.Rows != null)
                    {
                        dgv.Rows.Clear();
                    }
                    if (dgv.Columns != null)
                    {
                        dgv.Columns.Clear();
                    }

                    if (dgv.Name.Contains("TOU"))
                    {

                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 25;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Zone";
                        gridcombo.HeaderText = "Zone";
                        gridcombo.ReadOnly = true;
                        gridcombo.Items.Add("1");
                        gridcombo.Items.Add("2");
                        gridcombo.Items.Add("3");
                        gridcombo.Items.Add("4");
                        gridcombo.Items.Add("5");
                        gridcombo.Items.Add("6");
                        if (Is10Zone8Slots || Is10Zone6Slot)
                        {
                            gridcombo.Items.Add("7");
                            gridcombo.Items.Add("8");
                            gridcombo.Items.Add("9");
                            gridcombo.Items.Add("10");
                        }


                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 25;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Tariff";
                        gridcombo1.HeaderText = "Tariff";
                        gridcombo1.Items.Add("T1");
                        gridcombo1.Items.Add("T2");
                        gridcombo1.Items.Add("T3");
                        gridcombo1.Items.Add("T4");
                        gridcombo1.Items.Add("T5");
                        gridcombo1.Items.Add("T6");
                        if (Is10Zone8Slots)
                        {
                            gridcombo1.Items.Add("T7");
                            gridcombo1.Items.Add("T8");
                        }


                        DataGridViewComboBoxColumn gridcombo2 = new DataGridViewComboBoxColumn();
                        gridcombo2.Width = 25;
                        gridcombo2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo2.Name = "Hours";
                        gridcombo2.HeaderText = "Hours";
                        int index = 0;
                        for (index = 0; index <= 23; index++)
                        {
                            if (index < 10) { gridcombo2.Items.Add("0" + index.ToString()); }
                            else { gridcombo2.Items.Add(index.ToString()); }
                        }

                        DataGridViewComboBoxColumn gridcombo3 = new DataGridViewComboBoxColumn();
                        gridcombo3.Width = 25;
                        gridcombo3.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        gridcombo3.Name = "Minutes";
                        gridcombo3.HeaderText = "Minutes";

                        // Updated index to resolve index expection by mohsin
                        for (index = 0; index < 4; index++)
                        {
                            gridcombo3.Items.Add((index * 15).ToString("00"));
                        }


                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                        dgv.Columns.Add(gridcombo2);
                        dgv.Columns.Add(gridcombo3);
                    }
                    if (dgv.Name.Contains("Tables"))
                    {
                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 37;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Zone";
                        gridcombo.HeaderText = "Zone";
                        gridcombo.ReadOnly = true;
                        gridcombo.Items.Add("1");
                        gridcombo.Items.Add("2");
                        gridcombo.Items.Add("3");
                        gridcombo.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 37;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Mon";
                        gridcombo1.HeaderText = "Mon";
                        gridcombo1.Items.Add("1");
                        gridcombo1.Items.Add("2");
                        gridcombo1.Items.Add("3");
                        gridcombo1.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo2 = new DataGridViewComboBoxColumn();
                        gridcombo2.Width = 37;
                        gridcombo2.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo2.Name = "Tue";
                        gridcombo2.HeaderText = "Tue";
                        gridcombo2.Items.Add("1");
                        gridcombo2.Items.Add("2");
                        gridcombo2.Items.Add("3");
                        gridcombo2.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo3 = new DataGridViewComboBoxColumn();
                        gridcombo3.Width = 37;
                        gridcombo3.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo3.Name = "Wed";
                        gridcombo3.HeaderText = "Wed";
                        gridcombo3.Items.Add("1");
                        gridcombo3.Items.Add("2");
                        gridcombo3.Items.Add("3");
                        gridcombo3.Items.Add("4");

                        DataGridViewComboBoxColumn gridcombo4 = new DataGridViewComboBoxColumn();
                        gridcombo4.Width = 37;
                        gridcombo4.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo4.Name = "Thu";
                        gridcombo4.HeaderText = "Thu";
                        gridcombo4.Items.Add("1");
                        gridcombo4.Items.Add("2");
                        gridcombo4.Items.Add("3");
                        gridcombo4.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo5 = new DataGridViewComboBoxColumn();
                        gridcombo5.Width = 37;
                        gridcombo5.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo5.Name = "Fri";
                        gridcombo5.HeaderText = "Fri";
                        gridcombo5.Items.Add("1");
                        gridcombo5.Items.Add("2");
                        gridcombo5.Items.Add("3");
                        gridcombo5.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo6 = new DataGridViewComboBoxColumn();
                        gridcombo6.Width = 37;
                        gridcombo6.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo6.Name = "Sat";
                        gridcombo6.HeaderText = "Sat";
                        gridcombo6.Items.Add("1");
                        gridcombo6.Items.Add("2");
                        gridcombo6.Items.Add("3");
                        gridcombo6.Items.Add("4");


                        DataGridViewComboBoxColumn gridcombo7 = new DataGridViewComboBoxColumn();
                        gridcombo7.Width = 37;
                        gridcombo7.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo7.Name = "Sun";
                        gridcombo7.HeaderText = "Sun";
                        gridcombo7.Items.Add("1");
                        gridcombo7.Items.Add("2");
                        gridcombo7.Items.Add("3");
                        gridcombo7.Items.Add("4");




                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                        dgv.Columns.Add(gridcombo2);
                        dgv.Columns.Add(gridcombo3);
                        dgv.Columns.Add(gridcombo4);
                        dgv.Columns.Add(gridcombo5);
                        dgv.Columns.Add(gridcombo6);
                        dgv.Columns.Add(gridcombo7);
                    }
                    if (dgv.Name.Contains("Activation"))
                    {

                        DataGridViewComboBoxColumn gridcombo = new DataGridViewComboBoxColumn();
                        gridcombo.Width = 40;
                        gridcombo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo.Name = "Day";
                        gridcombo.HeaderText = "Day";
                        //gridcombo.ReadOnly = true;
                        int index = 0;
                        for (index = 1; index <= 31; index++)
                        {
                            if (index < 10) { gridcombo.Items.Add("0" + index.ToString()); }
                            else { gridcombo.Items.Add(index.ToString()); }
                        }

                        DataGridViewComboBoxColumn gridcombo1 = new DataGridViewComboBoxColumn();
                        gridcombo1.Width = 40;
                        gridcombo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        gridcombo1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gridcombo1.Name = "Month";
                        gridcombo1.HeaderText = "Month";
                        for (index = 1; index <= 12; index++)
                        {
                            if (index < 10) { gridcombo1.Items.Add("0" + index.ToString()); }
                            else { gridcombo1.Items.Add(index.ToString()); }
                        }

                        dgv.Columns.Add(gridcombo);
                        dgv.Columns.Add(gridcombo1);
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InitializeGrid(string GridName)", ex);

            }
        }
          // Added for GSM configuration in IEC meters
        private void oneToManyGSM_CheckedChanged(object sender, EventArgs e)
        {
            dgvMeterIdAndSim.Enabled = true;
            selectAll.Enabled = true;
            grpSimNumber.Enabled = false;
        }

        private void oneToOneGSM_CheckedChanged(object sender, EventArgs e)
        {
            ResetGrid(true);
            dgvMeterIdAndSim.Enabled = false;
            selectAll.Enabled = false;
            grpSimNumber.Enabled = true;
           
        }

        private void ResetGrid(bool resetToInitialMode)
        {
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {
                DataGridViewCheckBoxCell selectedCheckBox = dgvMeterIdAndSim.Rows[count].Cells["Select"] as DataGridViewCheckBoxCell;
                if (resetToInitialMode)
                {
                    selectedCheckBox.Value = false;
                    selectAll.Checked = false;
                }
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


        private void FillDefaultGridValues(string GridName)
        {
            try
            {
                DataGridView dgv = this.Controls.Find(GridName, true).FirstOrDefault() as DataGridView;
                int Count = 0;
                if (dgv != null)
                {
                    if (dgv.Rows != null)
                    {
                        dgv.Rows.Clear();
                    }

                    if (dgv.Name.Contains("Tables") || dgv.Name.Contains("Activation"))
                    {
                        Count = 4;
                    }
                    if (dgv.Name.Contains("TOU"))
                    {
                        Count = 6;
                        if (Is10Zone8Slots || Is10Zone6Slot)
                        {
                            Count = 10;
                        }
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        dgv.Rows.Add();
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (j == 0 && !dgv.Name.Contains("Activation"))
                            {
                                dgv.Rows[i].Cells[j].Value = Convert.ToString(i + 1);
                            }
                            else
                            {
                                dgv.Rows[i].Cells[j].Value = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillDefaultGridValues(string GridName)", ex);
            }
        }



        private void DisplayTOUSP(string touData, Dictionary<string, Dictionary<int, string>> dicTOUSP)
        {
            SetValuesInGrids(dicTOUSP);
            //throw new NotImplementedException();
        }

        private List<System.Enum> GetSelectedProfileId(string action)
        {
            List<System.Enum> selectedElements = new List<System.Enum>();
            selectedElements.Clear();
            selectedElements.AddRange(lngMain.GetSelectedProfilesList<System.Enum>(enumData));
            return selectedElements;
        }
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            communications.ClosePort(); 
            this.Close();
        }
        /// <summary>
        /// Create CFG File .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCfgFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            List<System.Enum> selectedProfiles;
            if (CheckValidations("create configuration file"))
            {
                string validationMessage = ValidateConfiguration("Config Write");
                if (validationMessage.Length == 0)
                {
                    this.StatusMessage = string.Empty;
                    string fileLocation = GetFileName();
                    if (!string.IsNullOrEmpty(fileLocation))
                    {
                        pnConfigOptions.Enabled = false;
                        FileStream file = new FileStream(fileLocation, FileMode.Create);
                        StreamWriter writer = new StreamWriter(file);
                        if (writer != null)
                        {
                            try
                            {
                                string authFlag = string.Empty;
                                this.Cursor = Cursors.WaitCursor;
                                selectedProfiles = GetSelectedProfileId("write");

                                writer.Write("(" + GetAuthenticationFlag(selectedProfiles) + ")");

                                //For CTratio
                                writer.Write("\r\n(00)");

                                if (selectedProfiles.Contains(ProfileId.DTM))
                                {
                                    writer.Write("\r\n(" + GetDTMProgrammingCmd() + ")");
                                }
                                else
                                {
                                    writer.Write("\r\n(00000000)");
                                }
                                if (selectedProfiles.Contains(ProfileId.DailyLog))
                                {
                                    writer.Write("\r\n(" + GetDailyLogCmd() + ")");
                                }
                                else
                                {
                                    writer.Write("\r\n(0000)");
                                }

                                if (selectedProfiles.Contains(ProfileId.FourTOU))
                                {
                                    writer.Write("\r\n" + CreateTOUCommand());
                                }
                                else
                                {
                                    writer.Write("\r\n(00)");
                                }

                                this.StatusMessage = "CFG File Created Successfully.";
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                this.StatusMessage = "Error occured while creating CFG file";
                                logger.Log(LOGLEVELS.Error, "btnCreateCfgFile_Click(object sender, EventArgs e)", ex);
                            }
                            finally
                            {
                                if (writer != null)
                                {
                                    writer.Close();
                                }
                                pnConfigOptions.Enabled = true;
                                this.Cursor = Cursors.Default;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(validationMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// Gets Flags for CFG file.
        /// </summary>
        /// <param name="selectedProfiles"></param>
        /// <returns></returns>
        private string GetAuthenticationFlag(List<System.Enum> selectedProfiles)
        {
            string authFlag = string.Empty;
            if (selectedProfiles.Contains(ProfileId.RTC))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.MagneticTamperIcon))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.FourTOU))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.BillingReset))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            //if (chk_CTRatio.Checked) authFlag += "1";
            //else authFlag += "0";
            authFlag += "0";
            if (selectedProfiles.Contains(ProfileId.DTM))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }
            if (selectedProfiles.Contains(ProfileId.DailyLog))
            {
                authFlag += "1";
            }
            else
            {
                authFlag += "0";
            }

            return authFlag;
        }
        /// <summary>
        /// Get DTM programming command for CFG file creation .
        /// </summary>
        /// <returns></returns>
        private string GetDTMProgrammingCmd()
        {
            string Tranrating = Convert.ToInt32(txtBoxTransformerRating.Text).ToString("X");
            while (Tranrating.Length < 4) Tranrating = "0" + Tranrating;
            string HL = Convert.ToInt32(txtBoxHighLoad.Text).ToString("X");
            while (HL.Length < 2) HL = "0" + HL;
            string LL = Convert.ToInt32(txtBoxLowLoad.Text).ToString("X");
            while (LL.Length < 2) LL = "0" + LL;
            return "(" + HL + LL + Tranrating + ")";
        }
        /// <summary>
        /// Get Daily command for CFG file creation .
        /// </summary>
        /// <returns></returns>
        private string GetDailyLogCmd()
        {
            try
            {
                string CmdResponse1 = "";
                if (chkBoxKwh.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxKvarhLag.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxKvarhLead.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxKVAh.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxDailyMD1.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxDailyMD2.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxDailyMD3.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;
                if (chkBoxMaxAvgVoltage.Checked == true) CmdResponse1 = "1" + CmdResponse1;
                else CmdResponse1 = "0" + CmdResponse1;

                string CmdResponse2 = "";
                if (chkBoxMinAvgVoltage.Checked == true) CmdResponse2 = "1" + CmdResponse2;
                else CmdResponse2 = "0" + CmdResponse2;
                if (chkBoxMaxAvgCurrent.Checked == true) CmdResponse2 = "1" + CmdResponse2;
                else CmdResponse2 = "0" + CmdResponse2;
                if (chkBoxMinAvgCurrent.Checked == true) CmdResponse2 = "1" + CmdResponse2;
                else CmdResponse2 = "0" + CmdResponse2;

                if (chkBoxCumFundkWh.Checked == true) CmdResponse2 = "1" + CmdResponse2;
                else CmdResponse2 = "0" + CmdResponse2;

                while (CmdResponse1.Length < 8) CmdResponse1 = "0" + CmdResponse1;
                while (CmdResponse2.Length < 8) CmdResponse2 = "0" + CmdResponse2;

                CmdResponse1 = Convert.ToInt32(CmdResponse1, 2).ToString();
                CmdResponse1 = Convert.ToInt32(CmdResponse1).ToString("X");

                CmdResponse2 = Convert.ToInt32(CmdResponse2, 2).ToString();
                CmdResponse2 = Convert.ToInt32(CmdResponse2).ToString("X");
                while (CmdResponse2.Length < 2) CmdResponse2 = "0" + CmdResponse2;
                while (CmdResponse1.Length < 2) CmdResponse1 = "0" + CmdResponse1;
                return CmdResponse1 + CmdResponse2;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDailyLogCmd()", ex);
                return "";
            }
        }


        /// <summary>
        /// This function returns the file name of cfg file chosen by user
        /// </summary>
        /// <param name="fileText"></param>
        private string GetFileName()
        {
            string fileLocation = string.Empty;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "cfg";
                saveFileDialog.Filter = "Export file (*.cfg)|*.cfg";
                saveFileDialog.AddExtension = true;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".cfg";
                saveFileDialog.Title = "Save As";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName == "")
                    {
                        CABMessageBox.ShowFilterMessage("File name cannot be blank", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    }
                    else
                    {
                        fileLocation = saveFileDialog.FileName.Trim();
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFileName()", ex);
            }
            finally
            {

            }
            return fileLocation;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoFill_Click(object sender, EventArgs e)
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            foreach (DataGridView sGrid in seasonGrids)
            {
                for (int rowIndex = 0; rowIndex < gridS1Day1.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < gridS1Day1.Columns.Count; colIndex++)
                    {
                        sGrid[colIndex, rowIndex].Value = gridS1Day1[colIndex, rowIndex].Value.ToString();
                    }
                }
            }
        }

        private void cmbBoxBillingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBoxBillingHour.Items.Clear();
            cmbBoxBillingMinute.Items.Clear();
            cmbBoxBillingDate.Items.Clear();
            if (cmbBoxBillingPeriod.SelectedIndex == 1)
            {
                for (int date = 1; date <= 28; date++)
                {
                    cmbBoxBillingDate.Items.Add(date.ToString().PadLeft(2, '0'));
                }

                for (int hour = 0; hour <= 23; hour++)
                {
                    cmbBoxBillingHour.Items.Add(hour.ToString().PadLeft(2, '0'));
                }

                for (int minute = 0; minute <= 59; minute++)
                {
                    cmbBoxBillingMinute.Items.Add(minute.ToString().PadLeft(2, '0'));
                }
                //cmbBoxBillingHour.SelectedIndex = 0;
                //cmbBoxBillingMinute.SelectedIndex = 0;
                //cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = true;
                cmbBoxBillingMinute.Enabled = true;
                cmbBoxBillingDate.Enabled = true;
                // cmbResetLockoutdays.SelectedIndex = 0;
            }
            else
            {
                cmbBoxBillingDate.Items.Add("01");
                cmbBoxBillingHour.Items.Add("00");
                cmbBoxBillingMinute.Items.Add("00");
                //cmbBoxBillingHour.SelectedIndex = 0;
                //cmbBoxBillingMinute.SelectedIndex = 0;
                //cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = false;
                cmbBoxBillingMinute.Enabled = false;
                cmbBoxBillingDate.Enabled = false;
                //cmbResetLockoutdays.SelectedIndex = 0;
                Application.DoEvents();
            }
            cmbBoxBillingHour.SelectedIndex = 0;
            cmbBoxBillingMinute.SelectedIndex = 0;
            cmbBoxBillingDate.SelectedIndex = 0;
            //cmbResetLockoutdays.SelectedIndex = 0;

        }

        private void ResetSinglePhaseNdlmsTouValues()
        {
            try
            {
                FillDefaultGridValues(gridTOUDay1_name);
                FillDefaultGridValues(gridTOUDay2_name);
                FillDefaultGridValues(gridTOUDay3_name);
                FillDefaultGridValues(gridTOUDay4_name);
                FillDefaultGridValues(gridDayTables.Name);
                FillDefaultGridValues(gridActivationDate.Name);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ResetSinglePhaseNdlmsTouValues()", ex);
            }
        }


        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSinglePhaseNdlmsTouValues();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnResetGrid_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnFillAuto_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridTOUDay1 != null && gridTOUDay2 != null && gridTOUDay3 != null && gridTOUDay4 != null && gridDayTables != null && gridActivationDate != null)
                {
                    FillDefaultGridValues(gridTOUDay2_name);
                    FillDefaultGridValues(gridTOUDay3_name);
                    FillDefaultGridValues(gridTOUDay4_name);
                    FillDefaultGridValues(gridDayTables.Name);
                    FillDefaultGridValues(gridActivationDate.Name);


                    if (Convert.ToString(gridTOUDay1.Rows[0].Cells[1].Value) != string.Empty)
                    {
                        int index = 0;
                        foreach (DataGridViewRow itemRow in gridTOUDay1.Rows)
                        {
                            int indexClm = 0;
                            foreach (DataGridViewColumn itemColumn in gridTOUDay1.Columns)
                            {
                                if (indexClm != 0)
                                {
                                    gridTOUDay2.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1.Rows[index].Cells[itemColumn.Name].Value;
                                    gridTOUDay3.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1.Rows[index].Cells[itemColumn.Name].Value;
                                    gridTOUDay4.Rows[index].Cells[itemColumn.Name].Value = gridTOUDay1.Rows[index].Cells[itemColumn.Name].Value;
                                }
                                indexClm++;
                            }

                            index++;
                        }

                        //gridDayTables.Rows.Add();
                        int indexColumn = 0;
                        foreach (DataGridViewColumn itemColumn in gridDayTables.Columns)
                        {
                            if (indexColumn != 0)
                            {
                                gridDayTables.Rows[0].Cells[itemColumn.Name].Value = "1";
                                gridDayTables.Rows[1].Cells[itemColumn.Name].Value = "2";
                                gridDayTables.Rows[2].Cells[itemColumn.Name].Value = "3";
                                gridDayTables.Rows[3].Cells[itemColumn.Name].Value = "4";
                            }
                            indexColumn++;
                        }


                        //gridActivationDate.Rows.Add();
                        foreach (DataGridViewColumn itemColumn in gridActivationDate.Columns)
                        {
                            gridActivationDate.Rows[0].Cells[itemColumn.Name].Value = "01";
                            gridActivationDate.Rows[1].Cells[itemColumn.Name].Value = "02";
                            gridActivationDate.Rows[2].Cells[itemColumn.Name].Value = "03";
                            gridActivationDate.Rows[3].Cells[itemColumn.Name].Value = "04";
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnFillAuto_Click(object sender, EventArgs e)", ex);
            }
        }     

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

        private void dgvMeterIdAndSim_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvMeterIdAndSim.IsCurrentCellDirty)
            {
                dgvMeterIdAndSim.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            }
        }

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
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started.";
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }

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

        private void DisableControls()
        {
            if (commType != CommunicationType.DIRECT)
            {
                dgvMeterIdAndSim.Enabled = false;
            }
            selectAll.Enabled = false;
            btnCancel.Enabled = false;
            btnWrite.Enabled = false;
            btnRead.Enabled = false;
            btnUploadFile.Enabled = false;
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnableControls(string mode)
        {
            btnCancel.Enabled = true;
            btnRead.Enabled = true;
            btnWrite.Enabled = true;
            btnUploadFile.Enabled = true;
            if (mode == ReaderMode)
            {
                btnWrite.Enabled = false;
            }
            else
            {
                btnWrite.Enabled = true;
            }
            selectAll.Enabled = true;
            if (commType != CommunicationType.DIRECT)
            {
                dgvMeterIdAndSim.Enabled = true;
            }
            this.Cursor = Cursors.Default;

        }

        private void ReadOneToOne()
        {
            try
            {
                simNumber = txtBoxMeterSIM.Text;
                if (commType != CommunicationType.DIRECT)
                {
                    this.StatusMessageAsync = "Connecting " + simNumber + " ...";
                }
                ChannelInformation channelInfo = new ChannelInformation();
                communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                communications.ComPort.PortName  = ConfigSettings.GetValue("PortName");
                communications.noOfRetry = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communications.SimNumber = simNumber;
                communications.ComPort.Open();
               if (communications.OpenSession() == true)
               {    
                    isMeterConnected = true;
                    SetConnectionDetail(true);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("read");
                  bool isConnected = GetMeterConfigData(selectedProfiles, false, 0);
                }
                else
                {
                    if (commType == CommunicationType.DIRECT)
                    {
                        //this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadOneToOne()", ex);
            }
            finally
            {
                //communications.CloseSession();
                isMeterConnected = false;
            }
        }

        private void ReadOneToMany()
        {
            try
            {
                Result result;
                if (ValidateGrid())
                {
                    ResetGrid(false);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("read");
                    byte totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                    {
                        for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                        {
                            DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(chk1.Value))
                            {
                                simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                    : "Connecting " + simNumber + " ...";
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                                this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                                Application.DoEvents();
                                ChannelInformation channelInfo = new ChannelInformation();
                                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                                channelInfo.ModemInfo = simNumber;
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                                channelInfo.ProtocolType = "IEC"; //UtilityDetails.PrimaryUtlityName;
                                channelInfo.NoOfRetries = totalRetries;
                                communication = new Communication(channelInfo);
                                result = communication.OpenSession();
                                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    isMeterConnected = true;
                                    SetConnectionDetail(true);
                                    //if (GetMeterConfigData(selectedProfiles, true, rowCount))
                                    //{
                                    //    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                    //    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout completed.";
                                    //    dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;
                                    //}
                                    //else
        //{
                                    //    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    //    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout failed.";
        //}

                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.";
                                    this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                }
                                communication.CloseSession();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast 1 SIM number to read!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadOneToMany()", ex);
            }
            finally
            {
                isMeterConnected = false;
            }
        }

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

       private bool ValidateSimNumber()
        {
            bool flag = true;
            long simNumber = 0;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                //CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.StatusMessage = "Please select atleast 1 sim number.";
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            else if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                flag = false;
            }
            if (commType == CommunicationType.GPRS)
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 15)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    flag = false;
                }
            }
            else
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 10)
                {
                   // CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.StatusMessage = "Please select atleast 1 sim number.";
                    txtBoxMeterSIM.Focus();
                    flag = false;
                }
            }
            return flag;

        }

        private bool GetMeterConfigData(List<System.Enum> selectedProfiles, bool isRemote, int simIndex)
        {
            bool isAnyReadSuccess = false;
            SetConnectionDetail(true);
            TOUInformationSP touInformationSP = new TOUInformationSP();
            this.StatusMessage = "Sign on...";
            Application.DoEvents();
            meterPswd = "";
            if (!touInformationSP.IECSignOn(communications,meterPswd))
            {
                this.StatusMessage = "Read Configuration Fail.";
                Application.DoEvents();
                communications.ClosePort(); 
                return false;
            }
             foreach (ProfileId selectedConfigId in selectedProfiles)
                    {
                        this.StatusMessage = "Reading Meter Configuration(s)...";
                        Application.DoEvents(); // Story - 354382 - Status message is not getting refreshed while reading parameters
                        SetConnectionDetail(true);
                        Application.DoEvents();
                        switch (selectedConfigId)
                        {
                            case ProfileId.FourTOU:
                                this.StatusMessage = "Reading TOU...";
                                Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                                if (IsMeterType == 1 || IsMeterType == 2)
                                {
                                    ReadAndDisplayTOUSP();
                                }
                                else
                                {
                                    ReadAndDisplayTOU();
                                }
                                break;
                            case ProfileId.RTC:
                                this.StatusMessage = "Reading RTC...";
                                Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                                ReadAndDisplayRTC();
                                break;
                            case ProfileId.BillingType:
                                this.StatusMessage = "Reading BillingType...";
                                Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                                if (IsMeterType == 1 || IsMeterType == 2)
                                {
                                    //1 Phase
                                    ReadAndDisplayBillingTypeSP();
                                }
                                else
                                {
                                    //3 Phase
                                }
                                break;

                            case ProfileId.DIP:
                                this.StatusMessage = "Reading DIP...";
                                Application.DoEvents();
                                if (IsMeterType == 1 || IsMeterType == 2)
                                {
                                    // 1 Phase
                                    ReadDemandIntegrationPeriodSP();
                                }
                                else
                                {
                                    //3 Phase
                                }
                                break;

                            case ProfileId.DisplayParametersIEC:
                                this.StatusMessage = "Reading Display Parameter...";
                                Application.DoEvents();
                                if (IsMeterType == 1 || IsMeterType == 2)
                                {
                                    // 1 Phase
                                    ReadDisplayParameterSP();
                                }
                                else
                                {
                                    //3 Phase
                                }
                                break;
                            case ProfileId.KvahSelection:
                                this.StatusMessage = "Reading kVAh Selection...";
                                Application.DoEvents();
                                if (IsMeterType == 1 || IsMeterType == 2)
                                {
                                    // 1 Phase
                                    ReadkVAhSelectionSP();
                                }
                                break;

                    default:
                                break;
                        }
                        Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                    }
                     communications.Command = command.BreakCommand;
                     communications.SendCommand();
                     communications.DelayExecution();
                    if (this.StatusMessage.Contains("Reading") && this.StatusMessage.Contains("..."))
                    {
                        this.StatusMessage = "Data read Successfully.";
                        Application.DoEvents();// Story - 354382 - Status message is not getting refreshed while reading parameters
                        communications.ClosePort();                     

                    }

                     

            return isAnyReadSuccess;
        
        
        }

        private void ReadkVAhSelectionSP()
        {
            //this.StatusMessage = string.Empty; // Story - 354382 - Status message is not getting refreshed while reading parameters
            string dipData = string.Empty;

            try
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
                Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
                touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                touInformationSP.Channel = communications;
                dipData = touInformationSP.KVAHSelection(ref statusMsg);

                DisplaykVAhSelectionSP(dipData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadkVAhSelectionSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void ReadDisplayParameterSP()
        {
            TOUInformationSP touInformationSP = new TOUInformationSP();           
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);             
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;                
                Application.DoEvents();
                touInformationSP.Channel = communications;

                Dictionary<string, string> dicDispPushSP = new Dictionary<string, string>();
                touInformationSP.GetPushDisplayParameterSP(dicDispPushSP, ref statusMsg);                
                Application.DoEvents();
                if (!statusMsg.Contains("Error"))
                {
                    objDisplayParameterIECConfig.SetPushButtonSelectedList(dicDispPushSP);
                }
                
                
                Dictionary<string, string> dicDispScrollSP = new Dictionary<string, string>();
                touInformationSP.GetScrollDisplayParameterSP(dicDispScrollSP, ref statusMsg);                
                Application.DoEvents();
                if (!statusMsg.Contains("Error"))
                {
                    objDisplayParameterIECConfig.SetScrollButtonSelectedList(dicDispScrollSP);
                }

                Dictionary<string, string> dicDispHighSP = new Dictionary<string, string>();
                touInformationSP.GetHighDisplayParameterSP(dicDispHighSP, ref statusMsg);                
                Application.DoEvents();
                if (!statusMsg.Contains("Error"))
                {
                    objDisplayParameterIECConfig.SetHighButtonSelectedList(dicDispHighSP);
                }        
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "ReadDisplayParameterSP()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
                this.Cursor = Cursors.Default;               
            }
        }

        private void WriteOneToOne()
        {
            try
            {
                simNumber = txtBoxMeterSIM.Text;
                if (commType != CommunicationType.DIRECT)
                {
                    this.StatusMessageAsync = "Connecting " + simNumber + " ...";
                }
                ChannelInformation channelInfo = new ChannelInformation();
                communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                communications.noOfRetry = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                communications.SimNumber = simNumber;
                communications.ComPort.Open();
                if (communications.OpenSession() == true)
                {
                    isMeterConnected = true;
                    SetConnectionDetail(true);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("write");
                    WriteMeterConfig_IEC(selectedProfiles, false, 0);
                            
                }
                else
                {
                    if (commType == CommunicationType.DIRECT)
                    {
                        //this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteOneToOne()", ex);
            }
            finally
            {
                communications.ClosePort();
                isMeterConnected = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteOneToMany()
        {
            try
            {
                Result result = new Result();
                bool isConnected;
                if (ValidateGrid())
                {
                    ResetGrid(false);
                    List<System.Enum> selectedProfiles = GetSelectedProfileId("write");
                    // To remove selected profile for NORMAL billing type [BillingType_Month]
                    if (normalBillingType.Checked == true)
                    {
                        selectedProfiles.Remove(ProfileId.BillingMonthType);
                    }
                    byte totalRetries;
                    if (commType == CommunicationType.DIRECT)
                    {
                        totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    }
                    else
                    {
                        // Fixing the no of retries to 1 in case of remote communication
                        totalRetries = 1;
                    }

                    for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                    {
                        for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                        {
                            DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                            style = new DataGridViewCellStyle();
                            if (Convert.ToBoolean(chk1.Value))
                            {
                                simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                    : "Connecting " + simNumber + " ...";
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                                this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                                Application.DoEvents();
                                ChannelInformation channelInfo = new ChannelInformation();
                                channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                                channelInfo.ModemInfo = simNumber;
                                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                                channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                                channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                                channelInfo.NoOfRetries = totalRetries;
                                communication = new Communication(channelInfo);

                                result = communication.OpenSession();
                                SetConnectionDetail(true);
                                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    isMeterConnected = true;
                                    isConnected = WriteMeterConfig_IEC(selectedProfiles, true, rowCount);
                                    if (isConnected)
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Write completed.";
                                        this.StatusMessageAsync = "Write completed.";
                                        dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;
                                    }
                                    else
                                    {
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Write failed.";
                                        this.StatusMessageAsync = "Write failed.";
                                    }

                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Connection " + simNumber + " failed.";
                                    this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                                }
                                communication.CloseSession();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select atleast 1 SIM number to write!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "WriteOneToMany()", ex);
            }
            finally
            {
                isMeterConnected = false;
            }
        }

        private bool WriteMeterConfig_IEC(List<System.Enum> selectedProfiles, bool isRemote, int simIndex)
        {

            bool isSuccess = false;
             try
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
               // touInformationSP.MeterPassword = meterPswd;
                this.StatusMessage = "Sign on...";
                Application.DoEvents();
                if (!touInformationSP.IECSignOn(communications, meterPswd))
                {
                    this.StatusMessage = "Data written Failed.";
                    Application.DoEvents();
                    communications.ClosePort();
                    return false;
                }

                foreach (ProfileId selectedConfigId in selectedProfiles)
                {

                    if (meterPswd == null || meterPswd.Length == 0 )
                    {
                        break;
                    }

                    this.StatusMessage = "Writting Meter Configuration(s)...";
                    SetConnectionDetail(true);
                    Application.DoEvents();
                    switch (selectedConfigId)
                    {
                        case ProfileId.RTC:
                            this.StatusMessage = "Writting RTC...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                UpdateRTCForSPhase();
                            else
                                UpdateRTC();
                            break;
                        case ProfileId.FourTOU:
                            this.StatusMessage = "Writting TOU...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                WriteTOUForSPhase();
                            else
                                WriteTOU();
                            break;
                        case ProfileId.BillingReset:
                            this.StatusMessage = "Writting Billing Reset...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                WriteBillingResetForSP();
                            else
                                WriteBillingReset();
                            break;
                        case ProfileId.DailyLog:
                            this.StatusMessage = "Writting Daily Log...";
                            WriteDailyLog();
                            break;
                        case ProfileId.DTM:
                            this.StatusMessage = "Writting Daily Profile...";
                            WriteDTM();
                            break;
                        case ProfileId.MagneticTamperIcon:
                            this.StatusMessage = "Writting Magnetic Tamper Icon...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                ResetMagneticTamperIconSP();
                            else
                                ResetMagneticTamperIcon();
                            break;
                        case ProfileId.DIP:
                            this.StatusMessage = "Writting Demand Integration Period...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                ResetDemandIntegrationPeriodSP();
                            break;
                        case ProfileId.KvahSelection:
                            this.StatusMessage = "Writting kVAh Selection...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                WriteKVAHSelectionSP();
                            break;
                        case ProfileId.BillingType:
                            this.StatusMessage = "Writting Billing Type...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                UpdateBillingTypeSP();
                            break;
                        case ProfileId.DisplayParametersIEC:
                            this.StatusMessage = "Writting Display Parameter...";
                            if (IsMeterType == 1 || IsMeterType == 2)
                                UpdateDisplayParameterSP();
                            break;
                        default:
                            break;
                    }
                    System.Threading.Thread.Sleep(200);
                }
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                if ((meterPswd != null ) && (meterPswd.Length > 0) && this.StatusMessage.Contains("Writting"))
                {
                    this.StatusMessage = "Data written successfully.";
                    isSuccess = true;
                    communications.ClosePort();
                    Application.DoEvents(); // Story - 354382 - Message was not getting refreshed during parameter written

                }
                else
                {
                    //MessageBox.Show(validationMessage, "BCS");
                    this.StatusMessage = validationMessage + "Data written Failed.";
                }
            }
             catch (Exception ex)    //Exception log for catch block
             {
                 MessageBox.Show(ex.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 logger.Log(LOGLEVELS.Error, "WriteMeterConfig_IEC(List<System.Enum> selectedProfiles, bool isRemote, int simIndex)", ex);
             }
             finally
             {
                 communications.ClosePort();
             } 
              return isSuccess;
        }

        private void WriteKVAHSelectionSP()
        {
            try
            {
                TOUInformationSP touInformationSP = new TOUInformationSP();
                Dictionary<string, Dictionary<int, string>> dicTOUSP = new Dictionary<string, Dictionary<int, string>>();
                touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                touInformationSP.Channel = communications;
                string tamperData = touInformationSP.KVAHSelection(ref statusMsg);

                tamperData = tamperData.Substring(tamperData.IndexOf('(') + 1, tamperData.IndexOf(')') - tamperData.IndexOf('(') - 1);

                if (tamperData.Length >= 12)
                {
                    string binaryval = Convert.ToString(Convert.ToInt32(tamperData.Substring(2, 2), 16), 2).PadLeft(8, '0');//convert to binary
                    string value = Convert.ToInt32(rdbKVAhLagOnly.Checked).ToString();

                    binaryval = binaryval.Remove(7);
                    binaryval += value;

                    string resultval = Convert.ToInt32(binaryval, 2).ToString("X2");

                    tamperData = tamperData.Remove(2, 2);
                    tamperData = tamperData.Insert(2, resultval);
                }

                Application.DoEvents();
                if (meterPswd.Length == 0)
                    return;
                this.Cursor = Cursors.WaitCursor;
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;
                
                touInformationSP.WriteKVAHSelection(tamperData);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteKVAHSelectionSP()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FinalizeProgrammingValues();
            }
        }

        private void UpdateDisplayParameterSP()
        {            
            List<string> SelectListPush;
            List<string> SelectListScroll;
            List<string> SelectListHigh;
            TOUInformationSP touInformationSP = new TOUInformationSP();
            touInformationSP.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            //MeterPassword meterPassword = new MeterPassword(false);
            //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
            try
            {
                InitializeProgrammingValues();
                this.Cursor = Cursors.WaitCursor;
                //meterPassword.ShowDialog();
                Application.DoEvents();
                if (meterPswd.Length == 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                touInformationSP.MeterPassword = meterPswd;
                touInformationSP.Channel = communications;

                SelectListPush = objDisplayParameterIECConfig.GetPushButtonSelectedList();
                SelectListScroll = objDisplayParameterIECConfig.GetScrollButtonSelectedList();
                SelectListHigh = objDisplayParameterIECConfig.GetHighButtonSelectedList();               


                touInformationSP.SetPushParameter(SelectListPush);
                touInformationSP.SetScrollParameter(SelectListScroll);
                touInformationSP.SetHighParameter(SelectListHigh);

                //this.StatusMessage = "TOU set successfully!";
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "UpdateDisplayParameterSP()", ex);
            }
            finally
            {
                FinalizeProgrammingValues();
            }
        }

    }
}