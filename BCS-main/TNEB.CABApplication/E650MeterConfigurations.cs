#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

using CAB.E650MeterConfiguration.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.WrapperLayer;
using CAB.E650MeterConfiguration;
using System.Collections.ObjectModel;
using CAB.Mapper;
using CABEntity;
using CAB.Parser.Entity;
using System.IO;
using CAB.EntityGenerator;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.BLL;
#endregion

namespace CAB.UI
{
    /// <summary>
    /// Provides for Meter Programming features
    /// </summary>
    public partial class E650MeterConfigurations : MdiChildForm
    {
        #region Constants and Variables
        private System.Resources.ResourceManager resourceMgr;
        private Communication communication;
        private List<byte> selectedPushParams = new List<byte>();
        private List<byte> selectedScrollParams = new List<byte>();
        private List<byte> selectedHighResParams = new List<byte>();
        private byte[] activeSeasonProfile;
        private byte[] activeWeekProfile;
        private byte[] activeDayProfile;
        private byte[] passiveSeasonProfile;
        private byte[] passiveWeekProfile;
        private byte[] passiveDayProfile;
        private byte[] passiveActivationDate;
        private DataSet displayParameterRepository;
        int rIndex = 0;
        int count = 0;
        int rcount = 0;
        int gIndex = 0;
        List<byte> touData;

        private const string ZONE = "Zone";
        private const string MONDAY = "Mon";
        private const string TUESDAY = "Tue";
        private const string WEDNESDAY = "Wed";
        private const string THURSDAY = "Thu";
        private const string FRIDAY = "Fri";
        private const string SATURDAY = "Sat";
        private const string SUNDAY = "Sun";
        private const string DAY = "Day";
        private const string Month = "Month";
        private const string TARIFF = "Tariff";
        private const string COLZONE = "colZone";
        private const string COLMONDAY = "colMon";
        private const string COLTUESDAY = "colTue";
        private const string COLWEDNESDAY = "colWed";
        private const string COLTHURSDAY = "colThu";
        private const string COLFRIDAY = "colFri";
        private const string COLSATURDAY = "colSat";
        private const string COLSUNDAY = "colSun";
        private const string COLDAY = "colDay";
        private const string COLMONTH = "colMonth";
        private const string COLSESSION = "colSeason";
        private const string WEEKPROFILE = "Week Profile";
        private const string COLTARIFF = "colTariff";
        private const string COLSTARTHOUR = "colStartHour";
        private const string STARTHOUR = "Start Hour";
        private const string COLSTARTMIN = "colStartMin";
        private const string STARTMIN = "Start Min";
        private const string WEEK = "Week";
        private const byte dayProfileCount = 1;
        private const byte weekProfileCount = 1;
        private const byte seasonProfileCount = 1;

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public E650MeterConfigurations()
        {

            InitializeComponent();
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.MeterConfigurations", System.Reflection.Assembly.GetExecutingAssembly());
            communication = new Communication(ConfigSettings.GetValue("PortName"),
                                              Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")),
                                              ConfigSettings.GetValue("ModePassword"));
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        /// <summary>
        /// Form Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterConfigurations_Load(object sender, EventArgs e)
        {
            timer_RTC.Start();
            BindTOUGrids();
            BindBillingTypeControls();
            BindDisplayParameters();
            if (tabRS232LockUnlock.TabPages.Contains(tabPageLSCapturePeriod))
            {
                tabRS232LockUnlock.TabPages.Remove(tabPageLSCapturePeriod);
            }
        }

        /// <summary>
        /// RTC Timer event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_RTC_Tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("en-GB");
            //System.Threading.Thread.CurrentThread.CurrentCulture = c;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = c;
            rtcCtrl.Controls[0].Controls["txtRTC"].Text = System.DateTime.Now.ToString(c);
        }

        /// <summary>
        /// Write Programming parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (CheckValidations("write"))
            {
                string validationMessage = ValidateConfiguration("write");
                if (validationMessage.Length == 0)
                {
                    List<ProfileId> selectedProfiles;
                    List<ProfileCommand> lstProfileCommands;
                    ProfileCommand selectedCommand;
                    int meterModelNumber = 0;
                    bool isConnected = false;
                    pnConfigOptions.Enabled = false;
                    this.StatusMessage = "";
                    txterrorLog.Text = "";
                    Application.DoEvents();
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        Result result = communication.OpenSession();
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            isConnected = true;
                            meterModelNumber = NamePlateConstants.PumaLTE650Value;
                            lstProfileCommands = GetProfileCommandEntity();
                            selectedProfiles = GetSelectedProfileId("write");
                            foreach (ProfileId selectedConfigId in selectedProfiles)
                            {

                                //Filter one command entity
                                List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                {
                                    return profileCommandEntity.TagNumber == (int)selectedConfigId
                                    && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                    profileCommandEntity.MeterModelNumber == 0);
                                });

                                //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                                if (selectedConfigId == ProfileId.RTC)
                                {
                                    ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                                    rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                                    profileCommand.Add(rtcCommand);
                                }

                                if (profileCommand.Count > 0)
                                {
                                    selectedCommand = profileCommand[0];
                                    selectedCommand.Action = ActionType.WRITE;

                                    //Fill WriteData buffer for corresponding programming parameter
                                    switch (selectedConfigId)
                                    {
                                        case ProfileId.RTC:
                                            profileCommand[0].WriteDataBuffer = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                            "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                            break;
                                        case ProfileId.DIP:
                                            if (cmbDemandType.Text == "Block Demand")
                                            {
                                                profileCommand[0].WriteDataBuffer = FillDIPData(false);
                                            }
                                            else
                                            {
                                                profileCommand[0].WriteDataBuffer = FillDIPData(true);
                                            }
                                            break;
                                        case ProfileId.SIP:
                                            profileCommand[0].WriteDataBuffer = FillSIPData();
                                            break;
                                        case ProfileId.BillingReset:
                                            //No need to send any data for MD reset
                                            profileCommand[0].Action = ActionType.RESET;
                                            break;
                                        case ProfileId.BillingType:
                                            profileCommand[0].WriteDataBuffer = FillBillingTypeData();
                                            break;
                                        case ProfileId.ResetLockOutDays:
                                            profileCommand[0].WriteDataBuffer = Convert.ToByte(cmbResetLockoutdays.Text);
                                            break;
                                        case ProfileId.KvahSelection:
                                            profileCommand[0].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                            break;
                                        case ProfileId.RS232LockUnlock:
                                            profileCommand[0].WriteDataBuffer = rdbRS232Lock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                            break;
                                        case ProfileId.AutoLock:
                                            profileCommand[0].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                            break;
                                        case ProfileId.PassiveSeasonProfile:
                                            profileCommand[0].WriteDataBuffer = GetSeasonProfileBuffer(meterModelNumber);
                                            break;
                                        case ProfileId.PassiveWeekProfile:
                                            profileCommand[0].WriteDataBuffer = GetWeekProfileBuffer(meterModelNumber);
                                            break;
                                        case ProfileId.PassiveDayProfile:
                                            profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(meterModelNumber);
                                            break;
                                        case ProfileId.ActivationDate:
                                            profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(meterModelNumber);
                                            break;
                                        case ProfileId.PushDisplayParameter:
                                            profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                                            break;
                                        case ProfileId.ScrollDisplyParameter:
                                            profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                                            break;
                                        case ProfileId.HighResolutionDisplayParameter:
                                            profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVHighResolution);
                                            break;
                                        case ProfileId.DisplayTimeoutParameter:
                                            profileCommand[0].WriteDataBuffer = GetDisplayTimeoutData();
                                            break;
                                        default:
                                            break;
                                    }

                                    result = communication.Send(profileCommand[0]);

                                    if (result.ErrorCode != CommunicationErrorType.Success)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                MessageBox.Show("Data written successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                this.StatusMessage = resourceMgr.GetString("FailureMsg_Write");
                                MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            this.StatusMessage = resourceMgr.GetString("FailureMsg_Write");
                            MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        this.StatusMessage = resourceMgr.GetString("FailureMsg_Write");

                    }
                    finally
                    {
                        if (isConnected)
                        {
                            communication.CloseSession();
                        }
                        pnConfigOptions.Enabled = true;
                        this.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show(validationMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Read programming parameters 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (CheckValidations("read"))
            {
                List<ProfileId> selectedProfiles;
                List<ProfileCommand> lstProfileCommands;
                ProfileCommand selectedCommand;
                int meterModelNumber = 0;
                bool isConnected = false;
                pnConfigOptions.Enabled = false;
                this.StatusMessage = "";
                txterrorLog.Text = "";
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                try
                { 
                    Result result = communication.OpenSession();
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        isConnected = true;
                        meterModelNumber = NamePlateConstants.PumaLTE650Value;
                        lstProfileCommands = GetProfileCommandEntity();
                        selectedProfiles = GetSelectedProfileId("read");
                        foreach (ProfileId selectedConfigId in selectedProfiles)
                        {
                            //Filter one command entity
                            List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                            {
                                return profileCommandEntity.TagNumber == (int)selectedConfigId
                                && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                profileCommandEntity.MeterModelNumber == 0);
                            });

                            //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                            if (selectedConfigId == ProfileId.RTC)
                            {
                                ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                                profileCommand.Add(rtcCommand);
                            }

                            if (profileCommand.Count > 0)
                            {
                                selectedCommand = profileCommand[0];
                                selectedCommand.Action = ActionType.READ;
                                this.StatusMessage = resourceMgr.GetString("Reading") + selectedConfigId.ToString() + " ...";
                                Application.DoEvents();
                                result = communication.Send(selectedCommand);   
                             
                                if (result.ErrorCode == CommunicationErrorType.Success || result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                                {
                                    if ((result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0) || selectedConfigId == ProfileId.BillingReset)
                                    {
                                        switch (selectedConfigId)
                                        {
                                            case ProfileId.RTC:
                                                DisplayMeterRTC(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.DIP:
                                                DisplayDIP(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.SIP:
                                                DisplayLSCapturePeriod(result.RecieveDataBuffer.ToArray());
                                                break;
                                            case ProfileId.BillingReset:
                                                //display Billig Reset
                                                break;
                                            case ProfileId.BillingType:
                                                DisplayBillingDateTime(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.ResetLockOutDays:
                                                DisplayBillingResetLockOutDays(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.KvahSelection:
                                                DisplayKVAhSelection(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.RS232LockUnlock:
                                                DisplayRS232LockUnlock(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.AutoLock:
                                                DisplayAutoLockUnlock(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            case ProfileId.PassiveSeasonProfile:
                                                passiveSeasonProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbFutureTOD.Checked)
                                                {
                                                    FillSeasonProfileParameters(passiveSeasonProfile);
                                                }
                                                break;
                                            case ProfileId.PassiveWeekProfile:
                                                passiveWeekProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbFutureTOD.Checked)
                                                {
                                                    FillWeekProfileParameters(passiveWeekProfile);
                                                }
                                                break;
                                            case ProfileId.PassiveDayProfile:
                                                passiveDayProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbFutureTOD.Checked)
                                                {
                                                    FillDayProfileParameters(passiveDayProfile, meterModelNumber);
                                                }
                                                break;
                                            case ProfileId.ActiveSeasonProfile:
                                                activeSeasonProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbCurrentTOD.Checked)
                                                {
                                                    FillSeasonProfileParameters(activeSeasonProfile);
                                                }
                                                break;
                                            case ProfileId.ActiveWeekProfile:
                                                activeWeekProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbCurrentTOD.Checked)
                                                {
                                                    FillWeekProfileParameters(activeWeekProfile);
                                                }
                                                break;
                                            case ProfileId.ActiveDayProfile:
                                                activeDayProfile = result.RecieveDataBuffer.ToArray();
                                                if (rdbCurrentTOD.Checked)
                                                {
                                                    FillDayProfileParameters(activeDayProfile, meterModelNumber);
                                                }
                                                break;
                                            case ProfileId.ActivationDate:
                                                passiveActivationDate = result.RecieveDataBuffer.ToArray();
                                                FillTOUActivationDate(passiveActivationDate);
                                                break;
                                            case ProfileId.PushDisplayParameter:
                                                ShowDispayParameters(result.RecieveDataBuffer.ToArray(), CABEntity.DisplayParameter.PushMode, selectedCommand);
                                                break;
                                            case ProfileId.ScrollDisplyParameter:
                                                ShowDispayParameters(result.RecieveDataBuffer.ToArray(), CABEntity.DisplayParameter.ScrollMode, selectedCommand);
                                                break;
                                            case ProfileId.HighResolutionDisplayParameter:
                                                ShowDispayParameters(result.RecieveDataBuffer.ToArray(), CABEntity.DisplayParameter.HighResolutionMode, selectedCommand);
                                                break;
                                            case ProfileId.DisplayTimeoutParameter:
                                                FillDisplayParametersTimeouts(result.RecieveDataBuffer.ToArray(), selectedCommand);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        result.ErrorCode = CommunicationErrorType.PasswordInavalid;
                                        this.StatusMessage = "Readout Failed.";
                                        Application.DoEvents();
                                        break;
                                    }
                                }
                                if (result.ErrorCode != CommunicationErrorType.Success)
                                {
                                    break;
                                }
                            }
                        }
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            this.StatusMessage = resourceMgr.GetString("Readoutcompleted");
                            Application.DoEvents();
                            btnCreateCfgFile_Click(sender, e);                           
                        }
                        else
                        {
                            this.StatusMessage = "Failure in Reading Configuration(s)";
                            MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        this.StatusMessage = "Failure in Reading Configuration(s)";
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {                    
                    this.StatusMessage = "Failure in Reading Configuration(s)";

                }
                finally
                {
                    if (isConnected)
                    {
                        communication.CloseSession();
                    }
                    pnConfigOptions.Enabled = true;
                    this.Cursor = Cursors.Default;
                }
            }

        }

        /// <summary>
        /// Billing Type Combobox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxBillingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbBoxBillingPeriod.SelectedIndex == 1)
            {
                for (int date = 1; date <= 28; date++)
                {
                    cmbBoxBillingDate.Items.Add(date);
                }

                for (int hour = 0; hour <= 23; hour++)
                {
                    cmbBoxBillingHour.Items.Add(hour);
                }

                for (int minute = 0; minute <= 59; minute++)
                {
                    cmbBoxBillingMinute.Items.Add(minute);
                }
                cmbBoxBillingHour.SelectedIndex = 0;
                cmbBoxBillingMinute.SelectedIndex = 0;
                cmbBoxBillingDate.SelectedIndex = 0;

                cmbBoxBillingHour.Enabled = true;
                cmbBoxBillingMinute.Enabled = true;
                cmbBoxBillingDate.Enabled = true;
                cmbResetLockoutdays.SelectedIndex = 0;
            }
            else
            {
                cmbBoxBillingHour.Items.Clear();
                cmbBoxBillingMinute.Items.Clear();
                cmbBoxBillingDate.Items.Clear();

                cmbBoxBillingHour.Enabled = false;
                cmbBoxBillingMinute.Enabled = false;
                cmbBoxBillingDate.Enabled = false;
                cmbResetLockoutdays.SelectedIndex = 0;
                Application.DoEvents();
            }

        }
       
        /// <summary>
        /// Selected index changed of display paremeter tab control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlDisplayParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageDisplayTimeOut", StringComparison.OrdinalIgnoreCase))
            {
                btnUpScroll.Visible = false;
                btnDownScroll.Visible = false;
                chkDisplayParamSelectAll.Visible = false;
            }
            else
            {
                btnUpScroll.Visible = true;
                btnDownScroll.Visible = true;
                chkDisplayParamSelectAll.Visible = true;
            }
           
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPagePushButton", StringComparison.OrdinalIgnoreCase))
            {               
                dGVPushDisplayParams.Columns["SNO"].Width = 80;
                dGVPushDisplayParams.Columns["ID"].Width = 80;
                dGVPushDisplayParams.Columns["Description"].Width = 200;
                dGVPushDisplayParams.Columns["colInclude"].Width = 85;               

            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageScrollButton", StringComparison.OrdinalIgnoreCase))
            {
               
                dGVScrollDisplayParams.Columns["SNO"].Width = 80;
                dGVScrollDisplayParams.Columns["ID"].Width = 80;
                dGVScrollDisplayParams.Columns["Description"].Width = 200;
                dGVScrollDisplayParams.Columns["colInclude"].Width = 85;
               
            }
            if (string.Equals(tabControlDisplayParams.SelectedTab.Name, "tabPageHighResolution", StringComparison.OrdinalIgnoreCase))
            {                
                dGVHighResolution.Columns["ID"].Width = 80;
                dGVHighResolution.Columns["Description"].Width = 200;
                dGVHighResolution.Columns["colInclude"].Width = 85;
                
            }

        }        
        /// <summary>
        /// Select all check box handler for disply parameters .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDisplayParamSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            DataGridView dgvTemp = null;
            bool status = chkDisplayParamSelectAll.Checked ? true : false;
            if (tabControlDisplayParams.SelectedIndex == 0)
            {
                dgvTemp =  dGVPushDisplayParams;
            }
            if (tabControlDisplayParams.SelectedIndex == 1)
            {
                dgvTemp = dGVScrollDisplayParams;
            }
            if (tabControlDisplayParams.SelectedIndex == 2)
            {
                dgvTemp = dGVHighResolution;
            }
            for (int i = 0; i < dgvTemp.Rows.Count; i++)
            {
                dgvTemp.Rows[i].Cells["colInclude"].Value = status;
            }          
        }
        /// <summary>
        /// Cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVScrollDisplayParams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVScrollDisplayParams, e);
        }
        /// <summary>
        /// Cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVHighResolution_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVHighResolution, e);
        }
        /// <summary>
        /// Cell Content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGVPushDisplayParams_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectAllCheckBoxForDisplayParameters(dGVPushDisplayParams, e);
        }


        #region CheckboxEventHandler
        /// <summary>
        /// select all event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkMDWithIP.Checked = true;
                chkKVARSelcetion.Checked = true;
                chkDisplayParam.Checked = true;
                chkTOD.Checked = true;
                chkRTC.Checked = true;
                chkBillingReset.Checked = true;
                chkBilingType.Checked = true;
                chkKVARSelcetion.Checked = true;
                chkLockRS232.Checked = true;
                chkAutoLock.Checked = true;
            }
            else
            {
                chkMDWithIP.Checked = false;
                chkKVARSelcetion.Checked = false;
                chkDisplayParam.Checked = false;
                chkTOD.Checked = false;
                chkRTC.Checked = false;
                chkBillingReset.Checked = false;
                chkBilingType.Checked = false;
                chkKVARSelcetion.Checked = false;
                chkLockRS232.Checked = false;
                chkAutoLock.Checked = false;
            }
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRTC_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkMDWithIP.Checked && chkKVARSelcetion.Checked && chkDisplayParam.Checked && chkTOD.Checked
                 && chkRTC.Checked && chkBillingReset.Checked && chkBilingType.Checked && chkKVARSelcetion.Checked && chkLockRS232.Checked)
            {
                chkSelectAll.Checked = true;
            }
            else
            {
                chkSelectAll.Checked = false;
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoLock_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkMDWithIP.Checked && chkKVARSelcetion.Checked && chkDisplayParam.Checked && chkTOD.Checked
                 && chkRTC.Checked && chkAutoLock.Checked && chkBillingReset.Checked && chkBilingType.Checked && chkKVARSelcetion.Checked && chkLockRS232.Checked)
            {
                chkSelectAll.Checked = true;
            }
            else
            {
                chkSelectAll.Checked = false;
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }

        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMDWithIP_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkKVARSelcetion_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDisplayParam_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTOD_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }

        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBilingType_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBillingReset_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLSCapturePeriod_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        /// <summary>
        /// Checkbox event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLockRS232_CheckedChanged(object sender, EventArgs e)
        {
            chkRTC_CheckedChanged(sender, e);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageHighResolution_Enter(object sender, EventArgs e)
        {
           CheckAndUpdateSelectAll(dGVHighResolution);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPagePushButton_Enter(object sender, EventArgs e)
        {
            CheckAndUpdateSelectAll(dGVPushDisplayParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageScrollButton_Enter(object sender, EventArgs e)
        {
            CheckAndUpdateSelectAll(dGVScrollDisplayParams);
        }       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validate cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvDayProfile.CurrentCell.IsInEditMode == true)
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
                                dgvDayProfile.Rows[e.RowIndex].Cells[2].Value = "00";
                                dgvDayProfile.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 1; colCount <= dgvWeekProfile.ColumnCount - 1; colCount++)
                                {
                                    dgvWeekProfile.Rows[0].Cells[colCount].Value = "01";
                                }
                                dgvSeasonProfile.Rows[0].Cells[0].Value = "01";
                                dgvSeasonProfile.Rows[0].Cells[1].Value = "01";
                                dgvSeasonProfile.Rows[0].Cells[2].Value = "01";

                            }
                        }
                        rcount = dgvDayProfile.CurrentCell.RowIndex;
                        if (dgvDayProfile.Rows[rcount].Cells[1].Value == null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null
                            || dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dgvDayProfile.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dgvDayProfile.Rows[rcount].Cells[1].Value != null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null &&
                            dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
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
                        if (e.RowIndex != 9 && dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < 10; count++)
                                    {
                                        dgvDayProfile.Rows[count].ReadOnly = true;
                                    }
                                }

                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "45")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < 10; count++)
                                        {
                                            dgvDayProfile.Rows[count].ReadOnly = true;
                                        }

                                    }

                                }
                            }
                        }
                        if (dgvDayProfile.Rows[rcount].Cells[1].Value != null &&
                            (dgvDayProfile.Rows[rcount].Cells[2].Value == null
                            || dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dgvDayProfile.Rows[rowIndex].ReadOnly = false;
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

                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 45)
                        {
                            e.Cancel = true;
                        }

                        if (e.RowIndex != 9 && dgvDayProfile.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dgvDayProfile.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                        {
                            if (dgvDayProfile.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dgvDayProfile.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                dgvDayProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                e.Cancel = true;
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeekProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvWeekProfile[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            string gridVal = e.FormattedValue.ToString();
                            if (gridVal == "")
                            {
                                dgvWeekProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                            }
                            else
                            {
                                if (Convert.ToInt16(gridVal) < 1 || Convert.ToInt16(gridVal) > 6)
                                {
                                    dgvWeekProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                    e.Cancel = true;
                                }
                                else
                                {
                                    dgvWeekProfile.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        if (e.RowIndex == 0 && e.ColumnIndex == 7)
                        {
                            dgvWeekProfile.Rows[e.RowIndex].Cells[0].Value = "01";
                            dgvWeekProfile.Rows[e.RowIndex].Cells[1].Value = "01";
                        }
                    }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSeasonProfile_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (dgvSeasonProfile[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }

                            if (e.FormattedValue.ToString() == "") { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 31))
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0 && e.FormattedValue.ToString() != ""
                                && e.FormattedValue.ToString() != null
                                && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value) > 29)
                            {
                                if (Convert.ToInt16(e.FormattedValue) == 2)
                                {
                                    dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (e.FormattedValue.ToString() == "")
                            { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 12))
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "INVALID";
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dgvSeasonProfile.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != 0 && e.FormattedValue != null && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value)
                                    <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }

                            if (e.RowIndex != seasonProfileCount - 1 && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value)
                                    >= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != seasonProfileCount - 1
                                && dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0
                                && e.RowIndex != seasonProfileCount - 1
                                && e.FormattedValue != null
                                && e.FormattedValue.ToString() != ""
                                && Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.FormattedValue != null && e.FormattedValue.ToString() != "" && Convert.ToInt16(e.FormattedValue) == 2)
                            {
                                if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex - 1, e.RowIndex].Value) == 29)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (e.RowIndex != 0 && e.ColumnIndex == 0)
                    {
                        if (e.RowIndex != 0 && e.FormattedValue != null
                            && e.FormattedValue.ToString() != ""
                            && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex, e.RowIndex - 1].Value))
                        {
                            if (Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex].Value)
                                <= Convert.ToInt16(dgvSeasonProfile[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    dgvSeasonProfile[1, count].ReadOnly = true;
                                    count++;
                                }
                                return;
                            }
                            else
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    dgvSeasonProfile[1, count].ReadOnly = false;
                                    count++;
                                }
                            }
                        }

                    }

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDayProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                rcount = dgvDayProfile.CurrentCell.RowIndex;
                if (rcount != 0 && dgvDayProfile.Rows[rcount - 1].Cells[1].Value != null)
                {
                    if (dgvDayProfile.Rows[rcount - 1].Cells[2].Value != null && dgvDayProfile.Rows[rcount - 1].Cells[3].Value != null)
                    {
                        if (dgvDayProfile.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dgvDayProfile.Rows[rcount - 1].Cells[3].Value.ToString() == "45")
                        {
                            for (count = rcount; count < 10; count++)
                            {
                                dgvDayProfile.Rows[count].ReadOnly = true;
                            }
                            return;
                        }
                    }
                }

                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (int rowCount = 0; rowCount < 9; rowCount++)
                    {
                        if ((dgvDayProfile.Rows[rowCount].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rowCount].Cells[3].Value != null)
                            && (dgvDayProfile.Rows[rowCount + 1].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rowCount + 1].Cells[3].Value != null))
                        {
                            if ((dgvDayProfile.Rows[rowCount].Cells[2].Value.ToString() == dgvDayProfile.Rows[rowCount + 1].Cells[2].Value.ToString())
                                && (Convert.ToInt16(dgvDayProfile.Rows[rowCount].Cells[3].Value) >= Convert.ToInt16(dgvDayProfile.Rows[rowCount + 1].Cells[3].Value)))
                            {
                                while (rowCount < 8)
                                {
                                    dgvDayProfile.Rows[rowCount + 2].ReadOnly = true;
                                    rowCount++;
                                }
                                return;
                            }
                        }
                    }
                }

                if (rcount != 0)
                {
                    if (dgvDayProfile.Rows[rcount - 1].Cells[3].Value == null)
                    {
                        int rowCount = rcount;
                        while (rowCount < 10)
                        {
                            dgvDayProfile.Rows[rowCount].ReadOnly = true;
                            rowCount++;
                        }
                        if (dgvDayProfile.Rows[rcount - 1].Cells[1].Value == null && dgvDayProfile.Rows[rcount - 1].Cells[2].Value == null)
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
                            dgvDayProfile.Rows[rcount].Cells[count].ReadOnly = false;
                        }
                        rIndex = rcount + 1;
                        while (rIndex < 10)
                        {
                            dgvDayProfile.Rows[rIndex].ReadOnly = false;
                            rIndex++;
                        }
                    }
                }

                if (dgvDayProfile.Rows[rcount].Cells[1].Value == null)
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dgvDayProfile.Rows[rcount].Cells[count].Value = null;
                        dgvDayProfile.Rows[rcount].Cells[count].ReadOnly = true;
                    }
                    count = 0;
                    while (dgvDayProfile.Name != dgvDayProfile.Name)
                    {
                        count++;
                    }

                    for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                    {
                        for (rIndex = 0; rIndex < 10; rIndex++)
                        {
                            if (dgvDayProfile.Rows[rIndex].Cells[1].Value != null
                                && (dgvDayProfile.Rows[rIndex].Cells[2].Value == null
                                || dgvDayProfile.Rows[rIndex].Cells[3].Value == null))
                            {
                                count = gIndex;
                                while (count < 3)
                                {
                                    dgvDayProfile.ReadOnly = true;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dgvDayProfile.ReadOnly = true;
                                        count--;
                                    }
                                }
                                dgvWeekProfile.ReadOnly = true;
                                dgvSeasonProfile.ReadOnly = true;
                                return;
                            }
                            else
                            {
                                count = gIndex;
                                while (count < 3)
                                {
                                    dgvDayProfile.ReadOnly = false;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dgvDayProfile.ReadOnly = false;
                                        count--;
                                    }
                                }
                                dgvWeekProfile.ReadOnly = false;
                                dgvSeasonProfile.ReadOnly = false;

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
                        dgvDayProfile.Rows[rcount].Cells[count].ReadOnly = false;
                    }
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dgvDayProfile.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }


                if (dgvDayProfile.Rows[rcount].Cells[1].Value != null
                    && dgvDayProfile.Rows[rcount].Cells[2].Value == null)
                {
                    dgvDayProfile.Rows[rcount].Cells[3].ReadOnly = true;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dgvDayProfile.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    dgvDayProfile.Rows[rcount].Cells[3].ReadOnly = false;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dgvDayProfile.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                if (dgvDayProfile.Rows[rcount].Cells[1].Value != null
                    && (dgvDayProfile.Rows[rcount].Cells[2].Value == null
                        && dgvDayProfile.Rows[rcount].Cells[3].Value == null))
                {
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dgvDayProfile.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dgvDayProfile.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rIndex = 0; rIndex < 10; rIndex++)
                    {
                        if (dgvDayProfile.Rows[rIndex].Cells[1].Value != null
                            && (dgvDayProfile.Rows[rIndex].Cells[2].Value == null
                           ))
                        {
                            count = gIndex;
                            while (count < 3)
                            {
                                dgvDayProfile.ReadOnly = true;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dgvDayProfile.ReadOnly = true;
                                    count--;
                                }
                            }
                            dgvWeekProfile.ReadOnly = true;
                            dgvSeasonProfile.ReadOnly = true;
                            return;
                        }
                        else
                        {
                            count = gIndex;
                            while (count < 3)
                            {
                                dgvDayProfile.ReadOnly = false;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dgvDayProfile.ReadOnly = false;
                                    count--;
                                }
                            }
                            dgvWeekProfile.ReadOnly = false;
                            dgvSeasonProfile.ReadOnly = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                dgvDayProfile.Rows[0].Cells[2].ReadOnly = true;
                dgvDayProfile.Rows[0].Cells[3].ReadOnly = true;
                dgvDayProfile.Columns[0].ReadOnly = true;
            }

        }
        /// <summary>
        /// week grid cell click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeekProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView weekProfileGrids = dgvWeekProfile;
            try
            {
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                if (rIndex != 0 && dgvSeasonProfile.Rows[rIndex - 1].Cells[1].Value == null)
                {
                    int rowIndex = rIndex;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.Rows[rIndex].ReadOnly = true;
                        rowIndex++;
                    }
                    return;
                }
                else
                {
                    int rowIndex = rIndex + 1;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.ReadOnly = false;
                        rowIndex++;
                    }
                }

                int colIndex = weekProfileGrids.CurrentCell.ColumnIndex;
                if (colIndex != 0 && (weekProfileGrids.Rows[rIndex].Cells[colIndex - 1].Value == null))
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].Value = null;
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = true;
                    return;
                }
                else
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = false;
                }

                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    if (dgvDayProfile.Rows[0].Cells[1].Value == null
                        && dgvDayProfile.Rows[0].Cells[2].Value == null
                        && dgvDayProfile.Rows[0].Cells[3].Value == null)
                    {
                        weekProfileGrids.ReadOnly = true;
                        break;
                    }
                    else
                    {
                        weekProfileGrids.ReadOnly = false;

                    }
                }
                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rcount = 0; rcount < 9; rcount++)
                    {
                        if ((dgvDayProfile.Rows[rcount].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rcount].Cells[3].Value != null)
                            && (dgvDayProfile.Rows[rcount + 1].Cells[2].Value != null)
                            && (dgvDayProfile.Rows[rcount + 1].Cells[3].Value != null))
                        {
                            if ((dgvDayProfile.Rows[rcount].Cells[2].Value.ToString() == dgvDayProfile.Rows[rcount + 1].Cells[2].Value.ToString())
                                && (Convert.ToInt16(dgvDayProfile.Rows[rcount].Cells[3].Value) >= Convert.ToInt16(dgvDayProfile.Rows[rcount + 1].Cells[3].Value)))
                            {
                                while (rcount < 8)
                                {
                                    dgvDayProfile.Rows[rcount + 2].ReadOnly = true;
                                    rcount++;
                                }
                                weekProfileGrids.ReadOnly = true;
                                dgvSeasonProfile.ReadOnly = true;
                                return;
                            }
                        }
                    }
                }
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                count = weekProfileGrids.CurrentCell.ColumnIndex;
                if (count >= 2)
                {
                    if (weekProfileGrids.Rows[rIndex].Cells[count].Value == null)
                    {
                        if (weekProfileGrids.Rows[rIndex].Cells[count - 1].Value == null)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = true;
                            dgvSeasonProfile.Rows[rIndex].ReadOnly = true;
                        }
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = true;
                            count++;
                        }
                        return;
                    }
                    else
                    {
                        weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = false;
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = false;
                            count++;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                weekProfileGrids.Columns[0].ReadOnly = true;
            }
        }
        /// <summary>
        /// Season grid cell click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSeasonProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                rIndex = dgvSeasonProfile.CurrentCell.RowIndex;
                if (dgvWeekProfile.Rows[rIndex].Cells[7].Value == null)
                {
                    dgvSeasonProfile.ReadOnly = true;
                }
                else
                {
                    dgvSeasonProfile.ReadOnly = false;
                    if (dgvSeasonProfile.Rows[rIndex].Cells[0].Value == null)
                    {
                        dgvSeasonProfile.Rows[rIndex].Cells[1].Value = null;
                        dgvSeasonProfile.Rows[rIndex].Cells[1].ReadOnly = true;
                    }
                    else
                    {
                        for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                        {
                            if (dgvDayProfile.Rows[0].Cells[1].Value == null
                                && dgvDayProfile.Rows[0].Cells[2].Value == null
                                && dgvDayProfile.Rows[0].Cells[3].Value == null)
                            {
                                dgvSeasonProfile.ReadOnly = true;
                                break;
                            }
                            else
                            {
                                dgvSeasonProfile.ReadOnly = false;

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
            }
        }
        /// <summary>
        /// Auto fill tou tables 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoFillTOUConfiguration_Click(object sender, EventArgs e)
        {
            if (dgvDayProfile.Rows[0].Cells[COLTARIFF].Value != null && dgvDayProfile.Rows[0].Cells[COLTARIFF].Value.ToString() != string.Empty)
            {
                for (int gridCount = 1; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rowCount = 0; rowCount < dgvDayProfile.Rows.Count; rowCount++)
                    {
                        dgvDayProfile.Rows[rowCount].Cells[COLTARIFF].Value = dgvDayProfile.Rows[rowCount].Cells[COLTARIFF].Value;
                        dgvDayProfile.Rows[rowCount].Cells[COLSTARTHOUR].Value = dgvDayProfile.Rows[rowCount].Cells[COLSTARTHOUR].Value;
                        dgvDayProfile.Rows[rowCount].Cells[COLSTARTMIN].Value = dgvDayProfile.Rows[rowCount].Cells[COLSTARTMIN].Value;
                    }
                }
                for (int rowCount = 0; rowCount < weekProfileCount; rowCount++)
                {
                    for (int colCount = 0; colCount <= dgvWeekProfile.ColumnCount - 1; colCount++)
                    {
                        if (colCount == 0)
                        {
                            dgvWeekProfile.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString();
                        }
                        else
                        {
                            dgvWeekProfile.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");
                        }
                    }
                }
                for (int rowCount = 0; rowCount < seasonProfileCount; rowCount++)
                {
                    for (int colCount = 0; colCount <= dgvSeasonProfile.ColumnCount - 1; colCount++)
                    {
                        dgvSeasonProfile.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");
                    }
                }
            }
        }
        /// <summary>
        /// Reset tou configuration data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetTOUConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rCount = 0; rCount < dgvDayProfile.RowCount; rCount++)
                    {
                        for (int cCount = 1; cCount < dgvDayProfile.ColumnCount; cCount++)
                        {
                            dgvDayProfile.Rows[rCount].Cells[cCount].Value = null;
                        }
                    }
                }
                for (int rCount = 0; rCount < weekProfileCount; rCount++)
                {
                    for (int cCount = 1; cCount < dgvWeekProfile.ColumnCount; cCount++)
                    {
                        dgvWeekProfile.Rows[rCount].Cells[cCount].Value = null;
                    }
                }

                for (int rCount = 0; rCount < seasonProfileCount; rCount++)
                {
                    for (int cCount = 0; cCount < dgvSeasonProfile.ColumnCount; cCount++)
                    {
                        dgvSeasonProfile.Rows[rCount].Cells[cCount].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            dtpFutureActivationDate.Value = DateTime.Now;
        }
        /// <summary>
        /// write tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteTOU_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// read current tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCurrentTOU_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Read future tou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFutureTOU_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Demand type change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDemandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDemandType.SelectedItem.ToString() == "Block Demand")
            {
                cmbDemandSubInterlavTime.SelectedIndex = -1;

            }
            else
            {

                if (cmbDemandInterval.Text == "15")
                {
                    cmbDemandSubInterlavTime.Text = "5";
                }
                else if (cmbDemandInterval.Text == "30")
                {
                    cmbDemandSubInterlavTime.SelectedItem = "10";
                }

            }
        }
        /// <summary>
        /// Interval type change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDemandInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDemandType.Text != "Block Demand")
            {
                if (cmbDemandInterval.Text == "15")
                {
                    cmbDemandSubInterlavTime.Text = "5";
                }
                else if (cmbDemandInterval.Text == "30")
                {
                    cmbDemandSubInterlavTime.SelectedItem = "10";
                }
            }
        }
        /// <summary>
        /// Create CFG File 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateCfgFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (CheckValidations("create configuration file"))
            {
                string validationMessage = ValidateConfiguration("Config Write");
                if (validationMessage.Length == 0)
                {
                    this.StatusMessage = string.Empty;
                    string fileLocation = GetFileName();
                    if (!string.IsNullOrEmpty(fileLocation))
                    {
                        FileStream file = new FileStream(fileLocation, FileMode.Create);
                        StreamWriter writer = new StreamWriter(file);
                        if (writer != null)
                        {
                            writer.WriteLine("DLMS");

                            List<ProfileId> selectedProfiles;
                            List<ProfileCommand> lstProfileCommands;
                            ProfileCommand selectedCommand;
                            int meterModelNumber = 0;
                            pnConfigOptions.Enabled = false;
                            this.StatusMessage = "";
                            txterrorLog.Text = "";
                            Application.DoEvents();
                            this.Cursor = Cursors.WaitCursor;
                            try
                            {
                                lstProfileCommands = GetProfileCommandEntity();
                                selectedProfiles = GetSelectedProfileId("write");
                                foreach (ProfileId selectedConfigId in selectedProfiles)
                                {

                                    //Filter one command entity
                                    List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                                    {
                                        return profileCommandEntity.TagNumber == (int)selectedConfigId
                                        && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                                        profileCommandEntity.MeterModelNumber == 0);
                                    });

                                    //This is an exception as we have RTC tag in xml file in instant profile so we can't put it one more time.
                                    if (selectedConfigId == ProfileId.RTC)
                                    {
                                        ProfileCommand rtcCommand = new ProfileCommand(8, "00.00.01.00.00.FF", 2);
                                        rtcCommand.ClassName = "CAB.E650MeterConfiguration.RTC,E650MeterConfiguration";
                                        profileCommand.Add(rtcCommand);
                                    }


                                    if (profileCommand.Count > 0)
                                    {
                                        selectedCommand = profileCommand[0];
                                        selectedCommand.Action = ActionType.WRITE;

                                        //Fill WriteData buffer for corresponding programming parameter
                                        switch (selectedConfigId)
                                        {

                                            case ProfileId.DIP:
                                                if (cmbDemandType.SelectedItem.ToString() == "Block Demand")
                                                {
                                                    profileCommand[0].WriteDataBuffer = FillDIPData(false);
                                                }
                                                else
                                                {
                                                    profileCommand[0].WriteDataBuffer = FillDIPData(true);
                                                }
                                                break;
                                            case ProfileId.SIP:
                                                profileCommand[0].WriteDataBuffer = Convert.ToInt32(cmbBoxLSCapturePeriod.Text);
                                                break;
                                            case ProfileId.RTC:
                                                profileCommand[0].WriteDataBuffer = DateTime.ParseExact(rtcCtrl.Controls[0].Controls["txtRTC"].Text,
                                                                                        "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                break;
                                            case ProfileId.BillingReset:
                                                //No need to send any data for MD reset
                                                profileCommand[0].Action = ActionType.RESET;
                                                break;
                                            case ProfileId.BillingType:
                                                profileCommand[0].WriteDataBuffer = FillBillingTypeData();
                                                break;
                                            case ProfileId.ResetLockOutDays:
                                                profileCommand[0].WriteDataBuffer = Convert.ToByte(cmbResetLockoutdays.Text);
                                                break;
                                            case ProfileId.KvahSelection:
                                                profileCommand[0].WriteDataBuffer = rdbKVAhLagOnly.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                                                break;
                                            case ProfileId.RS232LockUnlock:
                                                profileCommand[0].WriteDataBuffer = rdbRS232Lock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                break;
                                            case ProfileId.AutoLock:
                                                profileCommand[0].WriteDataBuffer = rdbAutoLock.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                                                break;
                                            case ProfileId.PassiveSeasonProfile:
                                                profileCommand[0].WriteDataBuffer = GetSeasonProfileBuffer(meterModelNumber);
                                                break;
                                            case ProfileId.PassiveWeekProfile:
                                                profileCommand[0].WriteDataBuffer = GetWeekProfileBuffer(meterModelNumber); break;
                                            case ProfileId.PassiveDayProfile:
                                                profileCommand[0].WriteDataBuffer = GetDayProfileBuffer(meterModelNumber);
                                                break;
                                            case ProfileId.ActivationDate:
                                                profileCommand[0].WriteDataBuffer = GetActivationDateBuffer(meterModelNumber);
                                                break;
                                            case ProfileId.PushDisplayParameter:
                                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVPushDisplayParams);
                                                break;
                                            case ProfileId.ScrollDisplyParameter:
                                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVScrollDisplayParams);
                                                break;
                                            case ProfileId.HighResolutionDisplayParameter:
                                                profileCommand[0].WriteDataBuffer = GetSelectedRowsinParameterGrid(dGVHighResolution);
                                                break;
                                            case ProfileId.DisplayTimeoutParameter:
                                                profileCommand[0].WriteDataBuffer = GetDisplayTimeoutData();
                                                break;
                                            default:
                                                break;
                                        }
                                        writer.Write(String.Format("{0:X2}", profileCommand[0].ClassId) + profileCommand[0].ObisCode.Replace(".", "") +
                                           String.Format("{0:X2}", profileCommand[0].Attribute));
                                        foreach (byte bufferByte in CommonConfig.GetDataBuffer(profileCommand[0].ClassName, profileCommand[0].WriteDataBuffer))
                                        {
                                            writer.Write(String.Format("{0:X2}", bufferByte));
                                        }
                                        writer.WriteLine();
                                    }

                                }
                                this.StatusMessage = "CFG File Created Successfully.";
                            }
                            catch (Exception)
                            {
                                this.StatusMessage = "Error occured while creating file CFG file";

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
        /// Upload CFG file ti fill UI controls .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";                    
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "Configuration File";
            openFile.InitialDirectory = ConfigInfo.GetLocation();
            openFile.Filter = "Configuration file(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            try
            {
                if (result == DialogResult.OK)
                {
                    if (DisplayConfigurationFromFile(openFile.FileName))
                    {
                        this.StatusMessage = resourceMgr.GetString("Upload");
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show(" Invalid File ", "BCS");

            }
        }
        /// <summary>
        /// Current TOD 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbCurrentTOD_CheckedChanged(object sender, EventArgs e)
        {
            //if(
            //FillSeasonProfileParameters(activeSeasonProfile);
            //FillWeekProfileParameters(activeWeekProfile);
            //FillDayProfileParameters(activeDayProfile,NamePlateConstants.PumaLTE650Value);
            //FillTOUActivationDate(passiveActivationDate);
        }
        /// <summary>
        /// Future TOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbFutureTOD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFutureTOD.Checked)
            {
                if (passiveSeasonProfile != null && passiveWeekProfile != null && passiveDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(passiveSeasonProfile);
                    FillWeekProfileParameters(passiveWeekProfile);
                    FillDayProfileParameters(passiveDayProfile, NamePlateConstants.PumaLTE650Value);
                    FillTOUActivationDate(passiveActivationDate);
                }
            }
            else
            {
                if (activeSeasonProfile != null && activeWeekProfile != null && activeDayProfile != null && passiveActivationDate != null)
                {
                    FillSeasonProfileParameters(activeSeasonProfile);
                    FillWeekProfileParameters(activeWeekProfile);
                    FillDayProfileParameters(activeDayProfile, NamePlateConstants.PumaLTE650Value);
                    FillTOUActivationDate(passiveActivationDate);
                }
            }
        }
        /// <summary>
        /// Refresh status message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRS232LockUnlock_MouseClick(object sender, MouseEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void E650MeterConfigurations_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
        }

        #endregion

        #region Private Methods
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
                    string[] profileWiseData = fileContent.Split('\n');                   
                    List<ProfileId> selectedProfiles;
                    ProfileCommand profileCommand ;
                    foreach (string profileData in profileWiseData)
                    {
                        string actualData = profileData.TrimEnd('\r');
                        if (Array.IndexOf(profileWiseData,profileData) == 0 || Array.IndexOf(profileWiseData,profileData) == profileWiseData.Length - 1)
                            continue;
                        DLMSCOMMAND dlmsCommand = new GenerateEntity().GetCommandFromCommandRepository(actualData.Substring(0, 16));
                        byte[] receivedData = SoapHexBinary.Parse(actualData.Substring(16)).Value;
                        profileCommand = new ProfileCommand();
                        profileCommand.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                        profileCommand.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                        profileCommand.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                        profileCommand.ObisCode = dlmsCommand.OBISCODE;
                        profileCommand.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                        profileCommand.ClassName = dlmsCommand.CLASSNAME;

                        switch (Convert.ToInt32(dlmsCommand.TAGNO))
                        {

                            case (int)ProfileId.RTC:
                                DisplayMeterRTC(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.DIP:
                                DisplayDIP(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.SIP:                               
                                break;
                            case (int)ProfileId.BillingReset:
                                chkMDReset.Checked = receivedData[1] == 1 ? true : false;
                                break;
                            case (int)ProfileId.BillingType:
                                DisplayBillingDateTime(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.ResetLockOutDays:
                                DisplayBillingResetLockOutDays(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.KvahSelection:
                                DisplayKVAhSelection(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.RS232LockUnlock:
                                DisplayRS232LockUnlock(receivedData,profileCommand);
                                break;
                            case (int)ProfileId.AutoLock:
                                DisplayAutoLockUnlock(receivedData, profileCommand);
                                break;
                            case (int)ProfileId.PassiveSeasonProfile:
                                FillSeasonProfileParameters(receivedData);
                                break;
                            case (int)ProfileId.PassiveWeekProfile:
                                FillWeekProfileParameters(receivedData);
                                 break;
                            case (int)ProfileId.PassiveDayProfile:
                                 FillDayProfileParameters(receivedData,NamePlateConstants.PumaLTE650Value);
                                break;
                            case (int)ProfileId.ActivationDate:
                                FillTOUActivationDate(receivedData);
                                break;
                            case (int)ProfileId.PushDisplayParameter:
                                ShowDispayParameters(receivedData, CABEntity.DisplayParameter.PushMode, profileCommand);
                                break;
                            case (int)ProfileId.ScrollDisplyParameter:
                                ShowDispayParameters(receivedData, CABEntity.DisplayParameter.ScrollMode, profileCommand);
                                break;
                            case (int)ProfileId.HighResolutionDisplayParameter:
                                ShowDispayParameters(receivedData, CABEntity.DisplayParameter.HighResolutionMode, profileCommand);
                                break;
                            case (int)ProfileId.DisplayTimeoutParameter:
                                FillDisplayParametersTimeouts(receivedData,profileCommand);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;                
            }
            return result;
        }
       
        /// <summary>
        /// This method is used for filling session profile details in TOU grids.
        /// </summary>
        private void FillSeasonProfileParameters(byte[] buffer)
        {
            try
            {
                int nIndex = 2;
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                    nIndex += 4;
                    dgvSeasonProfile.Rows[seasonCount].Cells[COLSESSION].Value = buffer[nIndex++].ToString("00");
                    nIndex += 4;
                    dgvSeasonProfile.Rows[seasonCount].Cells[COLMONTH].Value = buffer[nIndex++].ToString("00");
                    dgvSeasonProfile.Rows[seasonCount].Cells[COLDAY].Value = buffer[nIndex++].ToString("00");
                    nIndex += 11;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling weekly profile details in TOU grids.
        /// </summary>
        public void FillWeekProfileParameters(byte[] buffer)
        {
            int nIndex = 2;
            try
            {
                for (byte weekCount = 0; weekCount < weekProfileCount; weekCount++)
                {
                    nIndex += 5;
                    for (byte colCount = 1; colCount < 8; colCount++)
                    {
                        nIndex++;
                        dgvWeekProfile.Rows[weekCount].Cells[colCount].Value = buffer[nIndex++].ToString("00");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling day profile details in TOU grids.
        /// </summary>
        public void FillDayProfileParameters(byte[] buffer, int meterModel)
        {
            try
            {
                int nIndex = 2;
                for (byte dayCount = 0; dayCount < dayProfileCount; dayCount++)
                {
                    nIndex += 6;
                    for (byte rowCount = 0; rowCount < 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        if (tariff == 0)
                        {
                            dgvDayProfile.Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dgvDayProfile.Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dgvDayProfile.Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            dgvDayProfile.Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                            dgvDayProfile.Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                            dgvDayProfile.Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling TOU activation date.
        /// </summary>
        /// <param name="buffer"></param>        
        public void FillTOUActivationDate(byte[] buffer)
        {
            dtpFutureActivationDate.Refresh();
            dtpFutureActivationDate.CustomFormat = "dd/MM/yyyy";
            if (rdbFutureTOD.Checked)
            {
                int nIndex = 0x02;
                int activationYear = 0;
                try
                {
                    activationYear = (activationYear | (int)buffer[nIndex++]) << 8;
                    activationYear = (activationYear | (int)buffer[nIndex++]);
                    int activationMonth = buffer[nIndex++];
                    int activationDay = buffer[nIndex];
                    dtpFutureActivationDate.Value = Convert.ToDateTime(activationDay.ToString() + "/" + activationMonth.ToString() + "/"
                        + activationYear.ToString(), new CultureInfo("en-GB"));
                }
                catch (Exception ex)
                {
                    dtpFutureActivationDate.Value = DateTime.MinValue;
                    throw ex;
                }
            }
            else
            {                
                dtpFutureActivationDate.Value = DateTime.Now;                
            }
        }
        /// <summary>
        /// Validate display timeout tab
        /// </summary>
        /// <param name="scrollTime"></param>
        /// <param name="pushTimeOut"></param>
        /// <param name="autoScrollTime"></param>
        /// <returns></returns>
        private string ValidateDisplayTimeout(string scrollTime, string pushTimeOut, string autoScrollTime)
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

            if (!string.IsNullOrEmpty(scrollTime) && (ValidateRegEx(scrollTime, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid scroll time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(scrollTime) && (Convert.ToInt32(scrollTime) < 1 || Convert.ToInt32(scrollTime) > 300))
            {

                validationMessage += "Scroll time can contain value between 1 and 300." + Symbols.NEWLINE;
            }


            if (!string.IsNullOrEmpty(pushTimeOut) && (ValidateRegEx(pushTimeOut, @"^([0-9]{1,3})$") == false))
            {

                validationMessage += "Invalid push button timeout." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(pushTimeOut) && (Convert.ToInt32(pushTimeOut) < 1 || Convert.ToInt32(pushTimeOut) > 600))
            {

                validationMessage += "Push button timeout can contain value between 1 and 600." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (ValidateRegEx(autoScrollTime, @"^([0-9]{0,3})$") == false))
            {

                validationMessage += "Invalid auto scroll resume time." + Symbols.NEWLINE;
            }
            if (!string.IsNullOrEmpty(autoScrollTime) && (Convert.ToInt32(autoScrollTime) < 3 || Convert.ToInt32(autoScrollTime) > 300))
            {

                validationMessage += "Auto scroll resume time can contain value between 3 and 300." + Symbols.NEWLINE;

            }


            return validationMessage;
        }

        /// <summary>
        /// Validate a specified string against given regular expression 
        /// </summary>
        /// <param name="toValidate">String to be validated</param>
        /// <param name="regEx">Regular expression for validation</param>
        /// <returns>Returns true if validation is performed successfully otherwise false.</returns>
        private bool ValidateRegEx(string toValidate, string regEx)
        {
            if (Regex.Match(toValidate, regEx).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Tou grid binding
        /// </summary>
        private void BindTOUGrids()
        {
            BindDayProfileGrid();
            BindWeekProfileGrid();
            BindSeasonProfileGrid();
        }
        /// <summary>
        /// 
        /// </summary>
        private void BindDisplayParameters()
        {
            FillDisplayParameters(dGVPushDisplayParams, "PUSH");
            dGVPushDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVPushDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVPushDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedPushParams, dGVPushDisplayParams);
            dGVPushDisplayParams.Columns["SNO"].Width = 80;
            dGVPushDisplayParams.Columns["ID"].Width = 80;
            dGVPushDisplayParams.Columns["Description"].Width = 200;
            dGVPushDisplayParams.Columns["colInclude"].Width = 85;
            dGVPushDisplayParams.Refresh();

            FillDisplayParameters(dGVScrollDisplayParams, "SCROLL");
            dGVScrollDisplayParams.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVScrollDisplayParams.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVScrollDisplayParams.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedScrollParams, dGVScrollDisplayParams);

            dGVScrollDisplayParams.Columns["SNO"].Width = 80;
            dGVScrollDisplayParams.Columns["ID"].Width = 80;
            dGVScrollDisplayParams.Columns["Description"].Width = 200;
            dGVScrollDisplayParams.Columns["colInclude"].Width = 85;
            dGVScrollDisplayParams.Refresh();

            FillDisplayParameters(dGVHighResolution, "HIGHRESOLUTION");
            dGVHighResolution.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVHighResolution.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            dGVHighResolution.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;
            FillDisplayParameters(selectedHighResParams, dGVHighResolution);

            dGVHighResolution.Columns["SNO"].Width = 80;
            dGVHighResolution.Columns["ID"].Width = 80;
            dGVHighResolution.Columns["Description"].Width = 200;
            dGVHighResolution.Columns["colInclude"].Width = 85;

            dGVHighResolution.Refresh();

        }
        
        /// <summary>
        /// This method is used to day profile  grids on the view load method.
        /// </summary>
        private void BindDayProfileGrid()
        {
            try
            {
                dgvDayProfile.ColumnCount = 0;
                dgvDayProfile.RowHeadersVisible = false;
                dgvDayProfile.Columns.Add(GetDataGridView(10, COLZONE, ZONE, 35));
                dgvDayProfile.Columns.Add(GetDataGridView(8, COLTARIFF, TARIFF, 39));
                dgvDayProfile.Columns.Add(GetDataGridView(23, COLSTARTHOUR, STARTHOUR, 39));
                dgvDayProfile.Columns.Add(GetDataGridView(3, COLSTARTMIN, STARTMIN, 39));
                dgvDayProfile.RowCount = 10;
                for (int index = 0; index < dgvDayProfile.RowCount; index++)
                {
                    dgvDayProfile.Rows[index].Cells[0].Value = (index + 1).ToString();
                }
                dgvDayProfile.Columns[0].Width = 50;
                dgvDayProfile.Columns[1].Width = 80;
                dgvDayProfile.Columns[2].Width = 80;
                dgvDayProfile.Columns[3].Width = 80;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used to bind season profile grid on view load method
        /// </summary>        
        private void BindSeasonProfileGrid()
        {
            try
            {
                dgvSeasonProfile.Columns.Add(GetDataGridView(31, COLDAY, DAY, 40));
                dgvSeasonProfile.Columns.Add(GetDataGridView(12, COLMONTH, Month, 40));
                dgvSeasonProfile.Columns.Add(GetDataGridView(seasonProfileCount, COLSESSION, WEEKPROFILE, 50));
                dgvSeasonProfile.RowCount = seasonProfileCount;
                dgvSeasonProfile.RowHeadersVisible = false;
                dgvSeasonProfile.Rows[0].Cells[COLDAY].ReadOnly = true;
                dgvSeasonProfile.Rows[0].Cells[COLMONTH].ReadOnly = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to bind week details grid on the view load method.
        /// </summary>      
        private void BindWeekProfileGrid()
        {
            try
            {
                dgvWeekProfile.RowHeadersVisible = false;
                dgvWeekProfile.Columns.Add(GetDataGridView(1, COLZONE, WEEK, 50));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLMONDAY, MONDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLTUESDAY, TUESDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLWEDNESDAY, WEDNESDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLTHURSDAY, THURSDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLFRIDAY, FRIDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLSATURDAY, SATURDAY, 37));
                dgvWeekProfile.Columns.Add(GetDataGridView(dayProfileCount, COLSUNDAY, SUNDAY, 37));

                dgvWeekProfile.RowCount = weekProfileCount;
                dgvWeekProfile.Rows[0].Cells[0].Value = "1";

                dgvWeekProfile.Columns[COLZONE].ReadOnly = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Used to create columns for various profile data grids
        /// This method is called while adding columns to various data grids
        /// </summary>
        /// <param name="numberOfItems"></param>
        /// <param name="columnName"></param>
        /// <param name="headerText"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        DataGridViewComboBoxColumn GetDataGridView(int numberOfItems, string columnName, string headerText, int width)
        {
            int index = 1;
            DataGridViewComboBoxColumn gridViewComboBox = new DataGridViewComboBoxColumn();
            gridViewComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            gridViewComboBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewComboBox.Width = width;
            gridViewComboBox.Name = columnName;
            gridViewComboBox.HeaderText = headerText;
            if (headerText == STARTHOUR || headerText == STARTMIN)
            {
                index = 0;
            }
            for (; index <= numberOfItems; index++)
            {
                if (headerText == TARIFF)
                {
                    gridViewComboBox.Items.Add("T" + index.ToString());
                }
                else if (headerText == STARTMIN)
                {
                    gridViewComboBox.Items.Add((index * 15).ToString("00"));
                }
                else if (headerText == WEEK || headerText == ZONE)
                {
                    gridViewComboBox.Items.Add(index.ToString());
                }
                else
                {
                    gridViewComboBox.Items.Add(index.ToString("00"));
                }
            }
            return gridViewComboBox;

        }

        /// <summary>
        /// All check box of dgrid should be checked
        /// </summary>
        /// <param name="dgvTemp"></param>
        private void CheckAllTheElementsInGrid(DataGridView dgvTemp)
        {           
            if (chkDisplayParamSelectAll.Checked == true)
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
        /// <summary>
        /// Check uncheck according to display paremeter grid selected.
        /// </summary>
        /// <param name="dgvTemp"></param>
        private void CheckUncheckAll(DataGridView dgvTemp)
        {
            List<byte> selectedParemetrs = new List<byte>();
            if (chkDisplayParamSelectAll.Checked == true)
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = true;
                    selectedParemetrs.Add(Convert.ToByte(row.Cells["ID"].Value));
                }

            }
            else
            {
                foreach (DataGridViewRow row in dgvTemp.Rows)
                {
                    row.Cells["colInclude"].Value = false;
                }
            }
            if (dgvTemp.Name == "dGVPushDisplayParams")
            {
                selectedPushParams = selectedParemetrs;
            }
            else if (dgvTemp.Name == "dGVScrollDisplayParams")
            {
                selectedScrollParams = selectedParemetrs;
            }
            else if (dgvTemp.Name == "dGVHighResolution")
            {
                selectedHighResParams = selectedParemetrs;
            }
            dgvTemp.EndEdit();
        }
        /// <summary>
        /// Gets id's of selected rows in parameter grid.
        /// </summary>
        /// <param name="dgvDisplayParams"></param>
        /// <returns></returns>
        private List<byte> GetSelectedRowsinParameterGrid(DataGridView dgvDisplayParams)
        {
            List<byte> displayParams = new List<byte>();
            if (dgvDisplayParams != null)
            {
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                    {
                        displayParams.Add(Convert.ToByte(row.Cells["ID"].Value));
                    }
                }
            }          
            return displayParams;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dGVHighResolution"></param>
        private void CheckAndUpdateSelectAll(DataGridView dGVHighResolution)
        {
            bool isSelected = false;
            foreach (DataGridViewRow row in dGVHighResolution.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["colInclude"].Value))
                {
                    isSelected = false;
                    break;
                }
                else if (row.Cells["colInclude"].Value != null && Convert.ToBoolean(row.Cells["colInclude"].Value))
                {
                    isSelected = true;
                }
            }
            chkDisplayParamSelectAll.CheckedChanged -= new EventHandler(chkDisplayParamSelectAll_CheckedChanged);           
            chkDisplayParamSelectAll.Checked = isSelected;                                                   
            chkDisplayParamSelectAll.CheckedChanged += new EventHandler(chkDisplayParamSelectAll_CheckedChanged);
        }
        /// <summary>
        /// Fill selected disply parameters
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="dgvDisplayParams"></param>
        private void FillDisplayParameters(List<byte> receivedData, DataGridView dgvDisplayParams)
        {
            int parameterCount = receivedData.Count;
            byte displayParameters = 0;
            foreach (DataGridViewRow row in dgvDisplayParams.Rows)
            {
                row.Cells["colInclude"].Value = false;
            }
            Application.DoEvents();

            for (int paramCounter = 0; paramCounter < parameterCount; paramCounter++)
            {
                int rowCounter = 0;
                displayParameters = receivedData[paramCounter];
                foreach (DataGridViewRow row in dgvDisplayParams.Rows)
                {
                    rowCounter++;
                    if (Convert.ToInt32(row.Cells["ID"].Value) == displayParameters)
                    {
                        //row.Cells["colInclude"].Value = true;
                        for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                        {
                            MoveDisplayRow(tempRowCounter, dgvDisplayParams);
                        }

                    }
                }
            }

            dgvDisplayParams.ClearSelection();
            dgvDisplayParams.Rows[0].Cells[2].Selected = true;


            for (int displayParamCounter = 0; displayParamCounter < parameterCount; displayParamCounter++)
            {
                dgvDisplayParams.Rows[displayParamCounter].Cells["colInclude"].Value = true;
            }
            Application.DoEvents();
        }
        /// <summary>
        /// Show display paremeter after display readout.
        /// </summary>
        /// <param name="receviedData"></param>
        /// <param name="parameterType"></param>
        /// <param name="profileCommand"></param>
        private void ShowDispayParameters(byte[] receviedData, CABEntity.DisplayParameter parameterType, ProfileCommand profileCommand)
        {
            try
            {

                List<byte> selectedParameters = new List<byte>();
                DataGridView targetGridView = null;
                tabRS232LockUnlock.SelectedIndex = 3;
                if (parameterType == CABEntity.DisplayParameter.PushMode)
                {                    
                    targetGridView = dGVPushDisplayParams;                   
                }
                else if (parameterType == CABEntity.DisplayParameter.ScrollMode)
                {                   
                    targetGridView = dGVScrollDisplayParams;
                    tabControlDisplayParams.SelectedIndex = 1;
                }
                else if (parameterType == CABEntity.DisplayParameter.HighResolutionMode)
                {                    
                    targetGridView = dGVHighResolution;
                    tabControlDisplayParams.SelectedIndex = 2;
                }
                targetGridView.Columns["ID"].SortMode = DataGridViewColumnSortMode.NotSortable;
                targetGridView.Columns["SNO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                targetGridView.Columns["Description"].SortMode = DataGridViewColumnSortMode.NotSortable;


                ProfileData profileData = new CAB.E650MeterConfiguration.DisplayParameter(true).ParseData(receviedData, GetDLMSCommandFromProfileCommand(profileCommand));
                List<ProfileData> profileList = new List<ProfileData>();
                profileList.Add(profileData);
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new DisplayParameterAndTimeout().GetData(profileList, parameterType);
                    foreach (DataGridViewRow row in targetGridView.Rows)
                    {
                        row.Cells["colInclude"].Value = false;
                    }
                    Application.DoEvents();
                    int rowCounter = 0;
                    int selectedParameterId = -1;
                    for (int paramCounter = 0; paramCounter < collDisplayParamatersDBEntity.Count; paramCounter++)
                    {
                        rowCounter = 0;
                        selectedParameterId = GetParameterIdFromName(collDisplayParamatersDBEntity[paramCounter].paramaterName, parameterType);
                        selectedParameters.Add(Convert.ToByte(selectedParameterId));
                        foreach (DataGridViewRow row in targetGridView.Rows)
                        {
                            rowCounter++;
                            if (Convert.ToInt32(row.Cells["ID"].Value) == selectedParameterId)
                            {                                
                                for (int tempRowCounter = rowCounter; tempRowCounter > 1 + paramCounter; tempRowCounter--)
                                {
                                    MoveDisplayRow(tempRowCounter, targetGridView);
                                }

                            }
                        }
                    }

                    targetGridView.ClearSelection();
                    targetGridView.Rows[0].Cells[2].Selected = true;


                    for (int displayParamCounter = 0; displayParamCounter < collDisplayParamatersDBEntity.Count; displayParamCounter++)
                    {
                        targetGridView.Rows[displayParamCounter].Cells["colInclude"].Value = true;
                        
                    }                 

                   
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Error in showing dsiplay parameter !");
            }
            tabRS232LockUnlock.SelectedIndex = 0;
            tabControlDisplayParams.SelectedIndex = 0;
            this.StatusMessage = "Display Parameters " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Used to get parameter id from parameter name 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        private int GetParameterIdFromName(string parameterName, CABEntity.DisplayParameter parameterType)
        {
            DataTable masterTable = new DataTable();
            int parameterId = -1;
            if (parameterType == CABEntity.DisplayParameter.PushMode)
            {
                masterTable = displayParameterRepository.Tables["PushDisplayParams"];
            }
            else if (parameterType == CABEntity.DisplayParameter.ScrollMode)
            {
                masterTable = displayParameterRepository.Tables["ScrollDisplayParams"];
            }
            else if (parameterType == CABEntity.DisplayParameter.HighResolutionMode)
            {
                masterTable = displayParameterRepository.Tables["HighResolution"];
            }

            foreach (DataRow row in masterTable.Rows)
            {
                if (row["Description"].ToString() == parameterName)
                {
                    parameterId = Convert.ToInt32(row["ID"]);
                    break;
                }
            }
            return parameterId;
        }

        /// <summary>
        /// Move selected display row upward
        /// </summary>
        /// <param name="nRowIndex"></param>
        /// <param name="dgvDisplayParams"></param>
        private void MoveDisplayRow(int rowIndex, DataGridView dgvDisplayParams)
        {


            int selectedRow = rowIndex;// 
            if (selectedRow > 0)
            {

                String tempDispID, tempDispInfo;
                tempDispID = dgvDisplayParams.Rows[selectedRow - 1].Cells["ID"].Value.ToString();
                tempDispInfo = dgvDisplayParams.Rows[selectedRow - 1].Cells["Description"].Value.ToString();

                dgvDisplayParams.Rows[selectedRow - 1].Cells["ID"].Value = dgvDisplayParams.Rows[selectedRow - 2].Cells["ID"].Value;
                dgvDisplayParams.Rows[selectedRow - 1].Cells["Description"].Value = dgvDisplayParams.Rows[selectedRow - 2].Cells["Description"].Value;

                dgvDisplayParams.Rows[selectedRow - 2].Cells["ID"].Value = tempDispID;
                dgvDisplayParams.Rows[selectedRow - 2].Cells["Description"].Value = tempDispInfo;
                dgvDisplayParams.ClearSelection();


            }

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
                profileCommandEntity.ClassName = dlmsCommand.CLASSNAME;
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }

        /// <summary>
        /// Gets Selected profile Ids to be programmed in meter
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private List<ProfileId> GetSelectedProfileId(string action)
        {
            List<ProfileId> selectedElements = new List<ProfileId>();
            selectedElements.Clear();
            if (chkRTC.Checked)
            {
                selectedElements.Add(ProfileId.RTC);

            }
            if (chkBillingReset.Checked)
            {
                selectedElements.Add(ProfileId.BillingReset);
            }
            if (chkTOD.Checked)
            {
                if (action == "read")
                {
                    selectedElements.Add(ProfileId.PassiveSeasonProfile);
                    selectedElements.Add(ProfileId.PassiveWeekProfile);
                    selectedElements.Add(ProfileId.PassiveDayProfile);

                    selectedElements.Add(ProfileId.ActiveSeasonProfile);
                    selectedElements.Add(ProfileId.ActiveWeekProfile);
                    selectedElements.Add(ProfileId.ActiveDayProfile);
                }
                else
                {
                    if (rdbFutureTOD.Checked)
                    {
                        selectedElements.Add(ProfileId.PassiveSeasonProfile);
                        selectedElements.Add(ProfileId.PassiveWeekProfile);
                        selectedElements.Add(ProfileId.PassiveDayProfile);
                    }
                    else
                    {
                        selectedElements.Add(ProfileId.ActiveSeasonProfile);
                        selectedElements.Add(ProfileId.ActiveWeekProfile);
                        selectedElements.Add(ProfileId.ActiveDayProfile);
                    }
                }

                selectedElements.Add(ProfileId.ActivationDate);
            }
            if (chkDisplayParam.Checked)
            {
                selectedElements.Add(ProfileId.PushDisplayParameter);
                selectedElements.Add(ProfileId.ScrollDisplyParameter);
                selectedElements.Add(ProfileId.HighResolutionDisplayParameter);
                selectedElements.Add(ProfileId.DisplayTimeoutParameter);
            }
            if (chkBilingType.Checked)
            {
                selectedElements.Add(ProfileId.BillingType);
                selectedElements.Add(ProfileId.ResetLockOutDays);
            }            
            if (chkKVARSelcetion.Checked)
            {
                selectedElements.Add(ProfileId.KvahSelection);
            }
            if (chkLockRS232.Checked)
            {
                selectedElements.Add(ProfileId.RS232LockUnlock);
            }
            if (chkMDWithIP.Checked)
            {
                selectedElements.Add(ProfileId.DIP);
                selectedElements.Add(ProfileId.SIP);
            }
            if (chkAutoLock.Checked)
            {
                selectedElements.Add(ProfileId.AutoLock);
            }
            return selectedElements;
        }

        /// <summary>
        /// Used to fill Billing type entity from values of Billing type controls
        /// </summary>
        /// <returns></returns>
        private CAB.E650MeterConfiguration.Entity.BillingDateTime FillBillingTypeData()
        {
            CAB.E650MeterConfiguration.Entity.BillingDateTime billingTypeData = new CAB.E650MeterConfiguration.Entity.BillingDateTime();
            if (cmbBoxBillingPeriod.SelectedIndex == 1)
            {
                billingTypeData.Hour = Convert.ToByte(cmbBoxBillingHour.Text);
                billingTypeData.Date = Convert.ToByte(cmbBoxBillingDate.Text);
                billingTypeData.Minute = Convert.ToByte(cmbBoxBillingMinute.Text);
            }
            else
            {
                billingTypeData.Hour = 0xFF;
                billingTypeData.Date = 0xFE;
                billingTypeData.Minute = 0xFF;
            }
            return billingTypeData;
        }

        /// <summary>
        /// Used to fill Demand integration period
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int FillDIPData(bool isSlidingDemand)
        {
            int demandIPData;
            if (isSlidingDemand)
            {
                if (cmbDemandInterval.Text == "15")
                {
                    demandIPData = 4996;
                }
                else
                {
                    demandIPData = 9992;
                }
            }
            else
            {
                demandIPData = Convert.ToInt32(cmbDemandInterval.SelectedItem)*60;
            }
            return demandIPData;
        }

        /// <summary>
        ///  Used to fill LSIP
        /// </summary>
        /// <returns></returns>
        private int FillSIPData()
        {          
           int lodaSurveyCapturePeriod = Convert.ToInt16(cmbDemandInterval.SelectedItem) * 60;
           return lodaSurveyCapturePeriod;         
        }


        /// <summary>
        /// get display timeout data from UI  into entity
        /// </summary>
        /// <returns></returns>
        private DisplayTimeout GetDisplayTimeoutData()
        {
            DisplayTimeout displyTimeout = new DisplayTimeout();
            displyTimeout.PushTimeout = Convert.ToInt32(txtPushButtonTimeout.Text);
            displyTimeout.ScrollTime = Convert.ToInt32(txtScrollTime.Text);


            displyTimeout.AutoScrollTime = string.IsNullOrEmpty(txtScrollResumeTime.Text.Trim()) ? 0 : Convert.ToInt32(txtScrollResumeTime.Text);
            displyTimeout.AutoScrollModeSelected = (chkAutoScrollTime.Checked) ? 1 : 0;

            return displyTimeout;
        }

        /// <summary>
        /// This function checks whether user has made selctions before reading or writing meetr configurations
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True if proper selections have neen made, else False </returns>
        private bool CheckValidations(string action)
        {
            bool result = true;
            try
            {
                if (!chkAutoLock.Checked && !chkMDWithIP.Checked && !chkKVARSelcetion.Checked && !chkDisplayParam.Checked && !chkTOD.Checked && !chkRTC.Checked && !chkBilingType.Checked && !chkBillingReset.Checked && !chkLSCapturePeriod.Checked && !chkLockRS232.Checked)
                {
                    MessageBox.Show(resourceMgr.GetString("selectOption") + action, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    result = false;
                }
                if (chkBillingReset.Checked && action == "read")
                {
                    if (!chkMDWithIP.Checked && !chkKVARSelcetion.Checked && !chkDisplayParam.Checked && !chkTOD.Checked && !chkRTC.Checked && !chkBilingType.Checked  && !chkLockRS232.Checked)
                    {
                        MessageBox.Show("This option cannot be read", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        result = false;
                    }
                }
                
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Display meter RTC on  UI
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayMeterRTC(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new RTC(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    DataGridView dgvRTC = rtcCtrl.Controls[0].Controls["dGridRTC"] as DataGridView;
                    dgvRTC.Rows.Add();
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn1"].Value = dgvRTC.RowCount;
                    dgvRTC.Rows[dgvRTC.RowCount - 1].Cells["dataGridViewTextBoxColumn2"].Value =
                                                                    profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Dsiplaying RTC", "BCS");
            }
            this.StatusMessage = "RTC" + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Sets DIP on UI control
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayDIP(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                ProfileData profileData = new E650MeterConfiguration.DemandIntegrationPeriod(true).ParseData(receivedData,
                                                                                              GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string acutalData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    cmbDemandType.SelectedItem = Convert.ToInt32(acutalData.Substring(0, 1)) == 0 ? "Block Demand" : "Sliding Demand";
                    cmbDemandInterval.SelectedItem = (Int32.Parse(acutalData.Substring(1, 3), System.Globalization.NumberStyles.HexNumber) / 60).ToString();
                    cmbDemandSubInterlavTime.SelectedItem = (int.Parse(acutalData.Substring(0, 1), System.Globalization.NumberStyles.HexNumber) * 5).ToString();
                }
            }
            catch
            {

            }

            this.StatusMessage = "MDIP and LSIP" + resourceMgr.GetString("ReadSuccess");

        }
        /// <summary>
        ///  Converts ProfileCommand instance to DLMSCOMMAND instance .
        /// </summary>
        /// <param name="profileCommand"></param>
        /// <returns></returns>
        private DLMSCOMMAND GetDLMSCommandFromProfileCommand(ProfileCommand profileCommand)
        {
            DLMSCOMMAND dlmsCommand = new DLMSCOMMAND();
            dlmsCommand.ATTRIBUTE = profileCommand.Attribute.ToString();
            dlmsCommand.OBISCODE = profileCommand.ObisCode;
            dlmsCommand.CLASSNAME = profileCommand.ClassName;
            dlmsCommand.CLASS = profileCommand.ClassId.ToString();
            return dlmsCommand;

        }

        /// <summary>
        /// Sets Kvah selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayKVAhSelection(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new CAB.E650MeterConfiguration.KVAHSelection(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbKVAhLagOnly.Checked = true;
                    }
                    else
                    {
                        rdbKVAhLagLead.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in displaying KvahSelection!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.StatusMessage = "kVah" + resourceMgr.GetString("ReadSuccess");
           
        }

        /// <summary>
        /// Sets RS232 selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayRS232LockUnlock(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new RS232Lock(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbRS232Unlock.Checked = true;
                    }
                    else
                    {
                        rdbRS232Lock.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in displaying RS232LockUnlock !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.StatusMessage = "Lock Unlock RS232" + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Sets Auto Lock selection option to corresponding radio button
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayAutoLockUnlock(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new AutoLockUnlock(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string resultData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    if (resultData == "0")
                    {
                        rdbAutoUnlock.Checked = true;
                    }
                    else
                    {
                        rdbAutoLock.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in displaying Auto Lock !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
            this.StatusMessage = "Auto Lock Unlock" + resourceMgr.GetString("ReadSuccess");
        }
        /// <summary>
        /// Display Billing Type Data on UI
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayBillingDateTime(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                cmbBoxBillingPeriod.Items.Clear();
                cmbBoxBillingPeriod.Items.Add("End of Month");
                cmbBoxBillingPeriod.Items.Add("User Defined");
                ProfileData profileData = new BillingType(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string actualData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    string billingMode = actualData.Substring(0, 2);
                    if (billingMode == "00")
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
                        cmbBoxBillingDate.Text = Convert.ToInt32(actualData.Substring(0, 2)).ToString();
                        cmbBoxBillingHour.Text = actualData.Substring(2, 2);
                        cmbBoxBillingMinute.Text = actualData.Substring(4, 2);
                    }

                    //Lockout days 
                    //cmbResetLockoutdays.Text = Convert.ToString(Convert.ToInt32(actualData.Substring(10, 2), 16));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
            this.StatusMessage = "Billing Mode " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Display LS Capture period value on UI
        /// </summary>
        /// <param name="receivedData"></param>
        private void DisplayLSCapturePeriod(byte[] receivedData)
        {
            int compValue = 0;
            compValue = (compValue | (int)receivedData[01]) << 8;
            compValue = (compValue | (int)receivedData[02]);
            cmbBoxLSCapturePeriod.Text = Convert.ToString(compValue);
        }

        /// <summary>
        /// Display Billing reset lock out days
        /// </summary>
        /// <param name="receivedData"></param>
        /// <param name="profileCommand"></param>
        private void DisplayBillingResetLockOutDays(byte[] receivedData, ProfileCommand profileCommand)
        {
            try
            {
                ProfileData profileData = new ResetLockOutDays(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));
                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    string actualData = profileData.ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    cmbResetLockoutdays.Text = ((int.Parse(actualData, System.Globalization.NumberStyles.HexNumber)) / (24 * 4)).ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in dsiplaying Reset Lock out days !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            this.StatusMessage = "Reset Lockout Days  " + resourceMgr.GetString("ReadSuccess");
        }

        /// <summary>
        /// Fill Display parameters
        /// </summary>
        /// <param name="dGVPushDisplayParams"></param>
        /// <param name="paramType"></param>
        private void FillDisplayParameters(DataGridView dGVPushDisplayParams, string paramType)
        {

            //DataSet displayParameters = null;
            XmlDataDocument xmlDataDocument = null;
            try
            {
                xmlDataDocument = new XmlDataDocument();
                xmlDataDocument.DataSet.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"\" + "TNEBPUMADisplayParameters.xml");
                //displayParameters = new DataSet();
                displayParameterRepository = xmlDataDocument.DataSet;
                dGVPushDisplayParams.DataSource = displayParameterRepository.DefaultViewManager;

                //specify grdiview datamember
                if (paramType == "PUSH")
                {
                    dGVPushDisplayParams.DataMember = "PushDisplayParams";
                }
                else if (paramType == "SCROLL")
                {
                    dGVPushDisplayParams.DataMember = "ScrollDisplayParams";
                }
                else if (paramType == "HIGHRESOLUTION")
                {
                    dGVPushDisplayParams.DataMember = "HighResolution";
                }

                DataGridViewColumn gridViewColumn = new DataGridViewCheckBoxColumn();
                gridViewColumn.Name = "colInclude";
                gridViewColumn.HeaderText = "Include";

                if (!dGVPushDisplayParams.Columns.Contains("colInclude"))
                {
                    dGVPushDisplayParams.Columns.Insert(dGVPushDisplayParams.Columns.Count, gridViewColumn);
                }
                dGVPushDisplayParams.Columns[0].Width = 80;
                dGVPushDisplayParams.Columns[1].Width = 80;
                dGVPushDisplayParams.Columns[2].Width = 200;
                dGVPushDisplayParams.Columns[3].Width = 85;

                dGVPushDisplayParams.Columns["SNO"].ReadOnly = true;
                dGVPushDisplayParams.Columns["ID"].ReadOnly = true;
                dGVPushDisplayParams.Columns["Description"].ReadOnly = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //dispose and free memory occupied by objects
                displayParameterRepository.Dispose();
            }            
            
        }

        /// <summary>
        /// Used to get season profile data buffer.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetSeasonProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(TOUConstants.Array);
            touData.Add(seasonProfileCount);


            for (byte i = 0; i < seasonProfileCount; i++)
            {
                touData.Add(TOUConstants.Structure);
                touData.Add(0x03);

                touData.Add(0x09);
                touData.Add(0x01);
                if (Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLSESSION].Value) == 0x00)
                {
                    touData.Add(0x01);
                }
                else
                {
                    touData.Add(Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLSESSION].Value));
                }
                touData.Add(0x09);
                touData.Add(0x0C);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLMONTH].Value));
                touData.Add(Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLDAY].Value));
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0x80);
                touData.Add(0x00);
                touData.Add(0x00);
                touData.Add(0x09);
                touData.Add(0x01);
                if (Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLSESSION].Value) == 0x00)
                {
                    touData.Add(0x01);
                }
                else
                {
                    touData.Add(Convert.ToByte(dgvSeasonProfile.Rows[i].Cells[COLSESSION].Value));
                }
            }
            return touData.ToArray();
        }

        /// <summary>
        /// Used to get buffer data for week profile writing
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetWeekProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(TOUConstants.Array);
            touData.Add(weekProfileCount);


            for (byte i = 0; i < weekProfileCount; i++)
            {
                touData.Add(TOUConstants.Structure);
                touData.Add(0x08);

                touData.Add(0x09);
                touData.Add(0x01);
                touData.Add((byte)(i + 1));

                for (byte j = 1; j < 8; j++)
                {
                    touData.Add(0x11);
                    touData.Add(Convert.ToByte(dgvWeekProfile.Rows[i].Cells[j].Value) == 0x00 ?
                        (byte)0x01 : Convert.ToByte(dgvWeekProfile.Rows[i].Cells[j].Value));
                }
            }


            return touData.ToArray();
        }

        /// <summary>
        /// Used to get buffer for writing day profile data.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetDayProfileBuffer(int meterModel)
        {
            byte tempDayProfileCount = dayProfileCount;
            touData = new List<byte>();
            touData.Add(TOUConstants.Array);

            touData.Add(tempDayProfileCount);


            for (byte i = 0; i < tempDayProfileCount; i++)
            {
                touData.Add(0x02);
                touData.Add(0x02);
                touData.Add(0x11);
                touData.Add((byte)(i + 1));             //Day Id 
                touData.Add(0x01);
                touData.Add(0x0A);

                for (byte j = 0; j < 10; j++)
                {
                    touData.Add(0x02);
                    touData.Add(0x03);
                    touData.Add(0x09);
                    touData.Add(0x04);
                    touData.Add(Convert.ToByte(dgvDayProfile.Rows[j].Cells[COLSTARTHOUR].Value));      //   Slot Start Hour
                    touData.Add(Convert.ToByte(dgvDayProfile.Rows[j].Cells[COLSTARTMIN].Value));       //   Slot Start min
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x09);
                    touData.Add(0x06);
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x0A);
                    touData.Add(0x00);
                    touData.Add(0x64);
                    touData.Add(0xFF);
                    touData.Add(0x12);
                    touData.Add(0x00);
                    switch (Convert.ToString(dgvDayProfile.Rows[j].Cells[COLTARIFF].Value))
                    {
                        case "T1":
                            touData.Add(0x01);
                            break;
                        case "T2":
                            touData.Add(0x02);
                            break;
                        case "T3":
                            touData.Add(0x03);
                            break;
                        case "T4":
                            touData.Add(0x04);
                            break;
                        case "T5":
                            touData.Add(0x05);
                            break;
                        case "T6":
                            touData.Add(0x06);
                            break;
                        case "T7":
                            touData.Add(0x07);
                            break;
                        case "T8":
                            touData.Add(0x08);
                            break;
                        default:
                            touData.Add(0x00);
                            break;
                    }
                }
            }

            return touData.ToArray();
        }

        /// <summary>
        /// Used to get activation date buffer data 
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetActivationDateBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(0x09);
            touData.Add(0x0C);
            touData.Add(Convert.ToByte((dtpFutureActivationDate.Value.Year & 0xFF00) >> 8));
            touData.Add(Convert.ToByte(dtpFutureActivationDate.Value.Year & 0x00FF));
            touData.Add(Convert.ToByte(dtpFutureActivationDate.Value.Month)); //month
            touData.Add(Convert.ToByte(dtpFutureActivationDate.Value.Day));  //day of month   
            touData.Add(0xFF);  //day of week
            touData.Add(0xFF);  //hh
            touData.Add(0xFF);  //mm
            touData.Add(0xFF);  //ss
            touData.Add(0xFF);
            touData.Add(0x80);
            touData.Add(0x00);
            touData.Add(0x00);
            return touData.ToArray();

        }
        /// <summary>
        /// fill display timeout dat on ui
        /// </summary>
        /// <param name="receivedData"></param>
        private void FillDisplayParametersTimeouts(byte[] receivedData, ProfileCommand profileCommand)
        {

            try
            {
                ProfileData profileData = new DisplayTimeoutParameter(true).ParseData(receivedData, GetDLMSCommandFromProfileCommand(profileCommand));

                if (profileData != null && profileData.ListMeterDataPacket.Count > 0 && profileData.ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    foreach (DataElement dataElement in profileData.ListMeterDataPacket[0].ListDataElementValue)
                    {
                        if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 0)
                        {
                            txtPushButtonTimeout.Text = dataElement.Value;
                        }
                        else if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 1)
                        {
                            txtScrollTime.Text = dataElement.Value;
                        }
                        else if (profileData.ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 2)
                        {
                            txtScrollResumeTime.Text = dataElement.Value;
                            chkAutoScrollTime.Checked = true;
                        }

                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in showing Display Tiemout data !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Used to validate programming data before writting configuration to meter
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string ValidateConfiguration(string action)
        {
            StringBuilder errorMessage = new StringBuilder();
            if (chkMDWithIP.Checked)
            {
                if (cmbDemandType.Text == "")
                {
                    errorMessage.Append("Demand type can't be left blank." + Symbols.NEWLINE);

                }
                if (cmbDemandInterval.Text == "")
                {
                    errorMessage.Append("Demand interval can't be left blank." + Symbols.NEWLINE);

                }
                if (cmbDemandType.Text != "Block Demand" && cmbDemandInterval.SelectedIndex == 1 && cmbDemandSubInterlavTime.Text == "")
                {
                    errorMessage.Append("Demand sub interval can't be left blank." + Symbols.NEWLINE);

                }
            }
            if (chkBillingReset.Checked && action == "write")
            {
                if (!chkMDReset.Checked)
                {
                    errorMessage.Append("Please select billing reset checkbox." + Symbols.NEWLINE);
                }
            }
            if (chkBilingType.Checked)
            {
                if (cmbBoxBillingPeriod.Text == "")
                {
                    errorMessage.Append("Billing Period can't be left blank." + Symbols.NEWLINE);

                }
            }
            if (chkLSCapturePeriod.Checked)
            {
                if (cmbBoxLSCapturePeriod.Text == "")
                {
                    errorMessage.Append("Load survey capture period can't be left blank." + Symbols.NEWLINE);

                }
            }
            if (chkTOD.Checked)
            {
                if (rdbCurrentTOD.Checked && action == "write")
                {
                    errorMessage.Append("Please select future TOD." + Symbols.NEWLINE);
                }
                if (ValidateTOUGrids().Length > 0)
                {
                    errorMessage.Append(ValidateTOUGrids());
                }
            }
            if (chkLockRS232.Checked)
            {
                if (!rdbRS232Lock.Checked && !rdbRS232Unlock.Checked)
                {
                    errorMessage.Append("Please select Lock/Unlock RS232 option." + Symbols.NEWLINE);
                }
            }
            if (chkKVARSelcetion.Checked)
            {
                if (!rdbKVAhLagOnly.Checked && !rdbKVAhLagLead.Checked)
                {
                    errorMessage.Append("Please select kvah Selection mode." + Symbols.NEWLINE);

                }
            }
            if (chkAutoLock.Checked)
            {
                if (!rdbAutoLock.Checked && !rdbAutoUnlock.Checked)
                {
                    errorMessage.Append("Please select auto lock/unlock option." + Symbols.NEWLINE);

                }
            }
            if (chkDisplayParam.Checked)
            {
                errorMessage.Append(ValidateDisplayParameters());
            }

            return errorMessage.ToString();
        }
        /// <summary>
        /// Used to validate data in cells of all profile grids
        /// </summary>
        /// <returns></returns>
        public string ValidateTOUGrids()
        {
            string errorMessage = string.Empty;
            errorMessage = ValidateDayTOUGrids();
            errorMessage += ValidateWeekTOUGrids();
            errorMessage += ValidateSeasonTOUGrids();
            return errorMessage;

        }

        /// <summary>
        ///  used to validate data of dat profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateDayTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {

                    if (dgvDayProfile.Rows[0].Cells[COLTARIFF].Value == null)
                    {

                        errorMessage = "Please fill TOD Detail." + Symbols.NEWLINE;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorMessage;
        }


        /// <summary>
        ///  used to validate data of week profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateWeekTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int rowCount = 0; rowCount < dgvWeekProfile.RowCount; rowCount++)
                {
                    for (int colCount = 1; colCount < dgvWeekProfile.ColumnCount; colCount++)
                    {
                        if (dgvWeekProfile.Rows[rowCount].Cells[colCount].Value == null)
                        {
                            errorMessage = "Please fill TOU Week table." + Symbols.NEWLINE;
                            break;
                        }
                    }
                    if (errorMessage.Length > 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorMessage;
        }

        /// <summary>
        /// used to validate data of season profile grid
        /// </summary>
        /// <returns></returns>
        private string ValidateSeasonTOUGrids()
        {
            string errorMessage = string.Empty;
            try
            {
                for (int rowCount = 0; rowCount < dgvSeasonProfile.RowCount; rowCount++)
                {
                    if (dgvSeasonProfile.Rows[rowCount].Cells[COLMONTH].Value == null)
                    {
                        errorMessage = "Please fill TOU Season table." + Symbols.NEWLINE;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorMessage;
        }
        /// <summary>
        /// Validate display parameters 
        /// </summary>
        /// <returns></returns>
        private string ValidateDisplayParameters()
        {
            StringBuilder validationMessage = new StringBuilder();
            if (GetSelectedRowsinParameterGrid(dGVPushDisplayParams).Count == 0 && selectedPushParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 push button display parameter." + Symbols.NEWLINE);
            }
            if (GetSelectedRowsinParameterGrid(dGVScrollDisplayParams).Count == 0 && selectedScrollParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 scroll button display parameter." + Symbols.NEWLINE);
            }
            if (GetSelectedRowsinParameterGrid(dGVHighResolution).Count == 0 && selectedHighResParams.Count == 0)
            {
                validationMessage.Append("Please select at least 1 high resolution display parameter" + Symbols.NEWLINE);
            }

            validationMessage.Append(ValidateDisplayTimeout(txtScrollTime.Text, txtPushButtonTimeout.Text, txtScrollResumeTime.Text));

            return validationMessage.ToString();
        }
        /// <summary>
        /// Bind default data to billing type controls
        /// </summary>
        private void BindBillingTypeControls()
        {
            for (int i = 0; i <= 255; i++)
            {
                cmbResetLockoutdays.Items.Add(i);
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
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return fileLocation;
        }
        /// <summary>
        /// Update Select All check box on each cell check box click .
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void UpdateSelectAllCheckBoxForDisplayParameters(DataGridView dataGridView,DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {                
                if (e.ColumnIndex == 0)
                {
                    dataGridView.EndEdit();
                    chkDisplayParamSelectAll.CheckedChanged -= chkDisplayParamSelectAll_CheckedChanged;
                    if (!(bool)dataGridView.CurrentCell.Value)
                        chkDisplayParamSelectAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                            if (cell.Value == null || (bool)cell.Value == false)
                            { IfAllRowsSelected = false; break; }
                        }
                        chkDisplayParamSelectAll.Checked = IfAllRowsSelected;
                    }
                    this.chkDisplayParamSelectAll.CheckedChanged += chkDisplayParamSelectAll_CheckedChanged;
                }
            }
        }
        #endregion                                              
        
    }
}
