
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CAB.IECChannel;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using CAB.IECChannel.Programming;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using CAB.IECChannel.ReadOut;
using CAB.IECChannel.Formatter;
using CABAppControl;
using CAB.Contracts;
using CAB.Entity;
using CABEntity;
using LTCTBLL;
using CABApplication;
using System.Globalization;
using System.Threading;


namespace CAB.UI
{
    public partial class MeterConfigurations : MdiChildForm
    {
        MDWithIP mdwithip = new MDWithIP();
        Control group = new Control();
        Control subGroup = new Control();
        string data;
        string fileText;
        string displayText;
        string configData = "";
        string touType;
        string currTODReadResult;
        string futureTODReadResult;
        string meterRTCData = string.Empty;
        bool? isAborted = null;
        Collection<Collection<String>> collSelectedDisplayParameters = new Collection<Collection<string>>();
        private System.Resources.ResourceManager resourceMgr;
        DateTime sysTime;
        Collection<Collection<string>> displayParametersToSelect = new Collection<Collection<string>>();
        Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
        //  StreamWriter swLog;
        string filePath;
        string errmsg = "";
        string meterPwd;
        private int ctRatio;
        bool readFromFile;
        string touFileUploadPath;
        LoginForm loginform = new LoginForm();
        private string UtilityName;
        Utility utility = new Utility();
        ReadConfigurations readConfig = new ReadConfigurations();
        private string billingModeEOM = "End of Month";
        private string billingModeUD = "User Defined";

        public MeterConfigurations()
        {
            data = String.Empty;
            fileText = String.Empty;
            displayText = String.Empty;
            touType = String.Empty;
            currTODReadResult = String.Empty;
            futureTODReadResult = String.Empty;
            meterPwd = String.Empty;
            ctRatio=0;
            readFromFile = false;
            touFileUploadPath = String.Empty;
            InitializeComponent();
            resourceMgr = new System.Resources.ResourceManager("CAB.UI.MeterConfigurations", System.Reflection.Assembly.GetExecutingAssembly());
            
            
            //    filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", "MeterReadWriteLog.txt");
            //   filePath = filePath.Replace("\\\\", "\\");
        }

        internal ConfigurationParameter GetConfigParameters(string name)
        {
            switch (name)
            {
                case "MDWithIP":
                    return ConfigurationParameter.MDWithIP;
                case "kvahSelection":
                    return ConfigurationParameter.KVAHSelection;
                case "DisplayParameters":
                    return ConfigurationParameter.DisplayParameters;
                case "TOD":
                    return ConfigurationParameter.TOD;
                case "RTC":
                    return ConfigurationParameter.RTC;
                case "BillingReset":
                    return ConfigurationParameter.BillingReset;
                case "Resets":
                    return ConfigurationParameter.Resets;
                case "DailyLog":
                    return ConfigurationParameter.DailyLog;
                // For Billing Reset Commands
                case "ModeOfBilling":
                    return ConfigurationParameter.ModeOfBilling;
                case "BillingPeriod":
                    return ConfigurationParameter.BillingPeriod;
                case "ResetLockOutDays":
                    return ConfigurationParameter.ResetLockOutDays;
                case "LockUnlockRS232":
                    return ConfigurationParameter.LockUnlockRS232;
                default:
                    return ConfigurationParameter.None;
            }
        }

        private void MeterConfigurations_Load(object sender, EventArgs e)
        {            
            //Write configuration permission given only to the super user
            isAborted = null;
            if (ConfigInfo.UserInformationID == 1)
                btnWrite.Enabled = true;
            else
                btnWrite.Enabled = false;
            ////Write configuration permission given only to the super user
            
            group = this.kvarSelection1.Controls["grpkvarSelection"];
            RadioButton rdb = (RadioButton)group.Controls["rdbLagnLead"];

            rdb.Checked = true;
            rdbFutureTOD.Checked = true;
            timer_RTC.Start();
            chkBilingReset.Enabled = true;
            // To get Utility Name
            if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            {
                chkLockRS232.Enabled = false;
            }
            if (UtilityDetails.UtilityName == UtilityEntity.TNEB1)
            {
                chkLockRS232.Enabled = false;
                tabRS232LockUnlock.TabPages.Remove(tabRS232);
            }
            group = this.billingReset1.Controls["gbAutoMode"];
            ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
            if (UtilityDetails.DisableEndOfMonthOptionBillingMode)
            {
                cmbModeofBilling.Items.Remove(billingModeEOM);
            }
        }

        private void MeterConfigurations_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAborted == false)
            {
                this.StatusMessage = "Aborting...";

                isAborted = true;
                readConfig.BreakCommunication();
                Application.DoEvents();
                Thread.Sleep(500);

                this.StatusMessage = "User Aborted.";
                
            }
            else
                this.StatusMessage = "";

            this.RightStatusMessage = string.Empty;
            Application.DoEvents();

            this.Cursor = Cursors.Default;
            //if(swLog !=null)
            //swLog.Close();
        }

        /// <summary>
        /// This function checks whether user has made selctions before reading or writing meetr configurations
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True if proper selections have neen made, else False </returns>
        private bool CheckValidations(string action)
        {
            try
            {
                if (!chkMDWithIP.Checked && !chkKVARSelcetion.Checked && !chkDisplayParam.Checked && !chkTOD.Checked && !chkRTC.Checked && !chkBilingReset.Checked && !chkReset.Checked && !chkDailyLog.Checked && !chkLockRS232.Checked)
                {
                    MessageBox.Show(resourceMgr.GetString("selectOption") + action, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// This function checks whether user has entered valid values for the parameters under MD with IP option
        /// </summary>
        /// <returns>True if proper values have neen entered, else False</returns>
        private bool CheckMDwithIP()
        {
            bool result = true;
            if (cmbDemandType.Text == "")
            {
                result = false;
            }
            if (cmbDemandInterval.Text == "")
            {

                result = false;
            }
            if (cmbDemandType.Text != "Block Demand" && cmbDemandInterval.SelectedIndex == 1 && cmbDemandSubInterlavTime.Text == "")
            {
                result = false;

            }

            return result;
        }

        // Billing Resets parameters validation
        private bool CheckBillingReset()
        {
            group = this.billingReset1.Controls["gbAutoMode"];
            ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
            if (cmbModeofBilling.Text == "")
            {
                return false;
            }

            else
                return true;

        }
        // Daily Log parameters validation
        private bool CheckDailyLog()
        {
            group = this.dailyLog1.Controls["gbDailyLog"];
            CheckBox chkCumulativeKWh = (CheckBox)group.Controls["chkCumulativeKWh"];
            CheckBox chkSelectAll = (CheckBox)group.Controls["chkSelectAll"];
            CheckBox chkCumulativeKVARhLag = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
            CheckBox chkCumulativeKVARhLead = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
            CheckBox chkCumulativeKVAh = (CheckBox)group.Controls["chkCumulativeKVAh"];
            CheckBox chkDailyMD1 = (CheckBox)group.Controls["chkDailyMD1"];
            CheckBox chkDailyMD2 = (CheckBox)group.Controls["chkDailyMD2"];
            if (chkCumulativeKVAh.Checked == false && chkCumulativeKVARhLag.Checked == false && chkCumulativeKVARhLead.Checked == false
                && chkSelectAll.Checked == false && chkCumulativeKWh.Checked == false && chkDailyMD1.Checked == false && chkDailyMD2.Checked == false)
                return false;
            else
                return true;

        }
        #region Function to Get command Data String by ConfigSection Name
        /// <summary>
        /// Code Region Added by Vivek on 10 August 2011 (TNEB Project).
        /// Purpose : This function returns the command Data String for a specific configuration section.
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <returns></returns>
        private string GetDataForCommand(string configSectionName)
        {
            if (configSectionName.ToLower() == ConfigurationParameter.BillingReset.ToString().ToLower())
                return null;
            if (configSectionName.ToLower() == ConfigurationParameter.DailyLog.ToString().ToLower())
                return GetDailyLog();
            if (configSectionName.ToLower() == ConfigurationParameter.KVAHSelection.ToString().ToLower())
                return GetkvarSelection();
            if (configSectionName.ToLower() == ConfigurationParameter.MDWithIP.ToString().ToLower())
                return GetMDWithIP();
            if (configSectionName.ToLower() == ConfigurationParameter.Resets.ToString().ToLower())
                return null;
            //If Config Section Name is RTC.
            if (configSectionName.ToLower() == ConfigurationParameter.RTC.ToString().ToLower())
            {//Get Write Command Data String.
                RTCBLL rtcBLL = new RTCBLL();
                return rtcBLL.GetWriteCommand(sysTime);
            }
            if (configSectionName.ToLower() == ConfigurationParameter.TOD.ToString().ToLower())
                return null;
            //If Config Section Name is Display Paramter.
            if (configSectionName.ToLower() == ConfigurationParameter.DisplayParameters.ToString().ToLower())
            {
                //Get Write Command Data String.
                DisplayParametersBLL displayParametersBLL = new DisplayParametersBLL();
                return displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters);
            }
            //If Config Section Name is Push Mode Display Paramter.
            //if (configSectionName.ToLower() == DisplayParameter.PushMode.ToString().ToLower())
            //{
            //    //Get Write Command Data String for selected Push Mode Display Parameters.
            //    DisplayParametersBLL displayParametersBLL = new PushModeParameterBLL();
            //    return displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[0]);
            //}
            ////If Config Section Name is ScrollMode Display Paramter.
            //if (configSectionName.ToLower() == DisplayParameter.ScrollMode.ToString().ToLower())
            //{
            //    //Get Write Command Data String for selected ScrollMode Display Parameters.
            //    DisplayParametersBLL displayParametersBLL = new ScrollModeParameterBLL();
            //    return displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[1]);
            //}
            //If Config Section Name is HighResolutionMode Display Paramter.
            if (configSectionName.ToLower() == DisplayParameter.HighResolutionMode.ToString().ToLower())
            {
                //Get Write Command Data String for selected HighResolutionMode Display Parameters.
                DisplayParametersBLL displayParametersBLL = new HighResolutionModeParameterBLL();
                return displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[2], 0, collSelectedDisplayParameters[2].Count - 1);
            }
            //If Config Section Name isDisplayTimeouts.
            if (configSectionName.ToLower() == DisplayParameter.DisplayTimeouts.ToString().ToLower())
            {
                return collSelectedDisplayParameters[3][0];
            }
            //For Billing Reset Tab
            // If Config Section Name is Mode of Billing
            if (configSectionName.ToLower() == ConfigurationParameter.ModeOfBilling.ToString().ToLower())
            {
                return GetModeOfBilling();
            }
            // If Config Section Name is Billing Period
            if (configSectionName.ToLower() == ConfigurationParameter.BillingPeriod.ToString().ToLower())
            {
                return GetBillingPeriod();
            }
            // If Config Section Name is Reset Lock Out Days
            if (configSectionName.ToLower() == ConfigurationParameter.ResetLockOutDays.ToString().ToLower())
            {
                return GetResetLockOutDays();
            }
            if (configSectionName.ToLower() == ConfigurationParameter.LockUnlockRS232.ToString().ToLower())
            {
                if (chkLockRS232Port.Checked)
                    return "3030";
                else
                    return "3031";
            }

            return null;
        }
        #endregion

        /// <summary>
        /// This function retrieves the data string for 'MD with IP' values from the UI
        /// </summary>
        /// <returns>The data string</returns>
        private string GetMDWithIP()
        {
            try
            {
                data = "";
                string subInterval = "";                
                if (cmbDemandType.Text == "Block Demand") data = "30313031";
                else data = "30313032";
                data += ProgrammingCommon.GetASCIIValue(Convert.ToInt16(cmbDemandInterval.Text).ToString());
                subInterval = cmbDemandSubInterlavTime.Text;
                while (subInterval.Length < 2) subInterval = "0" + subInterval;
                data += ProgrammingCommon.GetASCIIValue(subInterval);
                if (cmbDemandType.Text == "Block Demand") data += "30323031";
                else data += "30323032";
                data += ProgrammingCommon.GetASCIIValue(Convert.ToInt16(cmbDemandInterval.Text).ToString());
                subInterval = "";
                subInterval = cmbDemandSubInterlavTime.Text;
                while (subInterval.Length < 2) subInterval = "0" + subInterval;
                data += ProgrammingCommon.GetASCIIValue(subInterval);
                data += "30303330303046464646";
            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }

        /// <summary>
        /// This function retrieves the data string for 'kVAr Selection' values from the UI
        /// </summary>
        /// <returns>The data string</returns>
        private string GetkvarSelection()
        {
            try
            {
                data = "";
                group = this.kvarSelection1.Controls["grpkvarSelection"];
                RadioButton rdb = (RadioButton)group.Controls["rdbLagOnly"];
                if (rdb.Checked)
                    data += ProgrammingCommon.GetASCIIValue(EnumUtil.StringValue(kVArSelectionParameters.LagOnly));
                else
                    data += ProgrammingCommon.GetASCIIValue(EnumUtil.StringValue(kVArSelectionParameters.LagandLead));

            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }


        //To get Daily Log parameters values from checkboxes

        private string GetDailyLog()
        {
            try
            {
                char[] dailylog = new char[8];
                data = "";
                group = this.dailyLog1.Controls["gbDailyLog"];
                CheckBox chkCumulativeKwh = (CheckBox)group.Controls["chkCumulativeKwh"];
                dailylog[0] = '0';
                dailylog[1] = '0';
                if (chkCumulativeKwh.Checked)
                {
                    dailylog[7] = '1';
                }
                else
                {
                    dailylog[7] = '0';
                }
                CheckBox chkCumulativeKVARhLag = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
                if (chkCumulativeKVARhLag.Checked)
                {
                    dailylog[6] = '1';
                }
                else
                {
                    dailylog[6] = '0';
                }
                CheckBox chkCumulativeKVARhLead = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
                if (chkCumulativeKVARhLead.Checked)
                {
                    dailylog[5] = '1';
                }
                else
                {
                    dailylog[5] = '0';
                }
                CheckBox chkCumulativeKVAh = (CheckBox)group.Controls["chkCumulativeKVAh"];
                if (chkCumulativeKVAh.Checked)
                {
                    dailylog[4] = '1';
                }
                else
                {
                    dailylog[4] = '0';
                }

                CheckBox chkDailyMD1 = (CheckBox)group.Controls["chkDailyMD1"];
                if (chkDailyMD1.Checked)
                {
                    dailylog[3] = '1';
                }
                else
                {
                    dailylog[3] = '0';
                }
                CheckBox chkDailyMD2 = (CheckBox)group.Controls["chkDailyMD2"];
                if (chkDailyMD2.Checked)
                {
                    dailylog[2] = '1';
                }
                else
                {
                    dailylog[2] = '0';
                }

                string binaryDailyLog = "";
                for (int i = 0; i < dailylog.Length; i++)
                {
                    binaryDailyLog += dailylog[i];
                }
                int decDailyLog = Convert.ToInt32(binaryDailyLog, 2);
                string hexaDailyLog = Convert.ToString(decDailyLog, 16).ToUpper();

                if (hexaDailyLog.Length == 1)
                {
                    hexaDailyLog = Convert.ToString(hexaDailyLog).PadLeft(2, '0');
                }
                data += ProgrammingCommon.GetASCIIValue(hexaDailyLog.Substring(0, 1));
                data += ProgrammingCommon.GetASCIIValue(hexaDailyLog.Substring(1, 1));

            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }

        /// <summary>
        /// This function retrieves the data string for 'TOD' values from the UI 
        /// and creates the TOD write command.
        /// </summary>
        /// <returns>The TOD Write Command list</returns>
        private List<string> GetTODCommands()
        {
            List<string> todCommands = new List<string>();
            string todAddress = string.Empty;
            string todCommand = string.Empty;
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
                todAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                todCommand = string.Empty;

                todAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridSeason[seasonIndex].Rows)
                {
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((seasonIndex % 6 + 1 % 7).ToString("d2"));
                todCommands.Add("01573102" + todAddress + "28" + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + todCommand + "29" + "03");
            }

            for (int holidayIndex = 0; holidayIndex <= gridHoliday.GetUpperBound(0); holidayIndex++)
            {
                todAddress = string.Empty;
                slots = 0;
                dayTable = string.Empty;
                todCommand = string.Empty;

                todAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                foreach (DataGridViewRow row in gridHoliday[holidayIndex].Rows)
                {
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    todCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((holidayIndex % 10 + 1 % 11).ToString("d2"));
                holidayActivationDate = ProgrammingCommon.GetASCIIValue(DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString().Substring(2, 6));
                todCommands.Add("01573102" + todAddress + "28" + holidayActivationDate + dayTable + ProgrammingCommon.GetASCIIValue(slots.ToString("d2")) + todCommand + "29" + "03");
            }

            for (int dayAssignment = 0; dayAssignment <= gridDayAssignment.GetUpperBound(0); dayAssignment++)
            {
                todCommand = string.Empty;
                todAddress = string.Empty;
                todAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));

                foreach (DataGridViewRow row in gridDayAssignment[dayAssignment].Rows)
                {
                    string tempStr = row.Cells[1].Value.ToString();
                    todCommand += ProgrammingCommon.GetASCIIValue(tempStr.Replace("Day Table ", "0").Trim());
                }
                todCommands.Add("01573102" + todAddress + "28" + todCommand + "29" + "03");
            }
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];
            todAddress = ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
            todCommand = string.Empty;
            todCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Day.ToString("d2"));
            todCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Month.ToString("d2"));
            todCommand += ProgrammingCommon.GetASCIIValue(dtPickerFutureActivationDate.Value.Year.ToString().Substring(2));

            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];
            //gridActivation.Rows.Clear();
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string tempCommand = "";
                DateTime dateTime = ProgrammingCommon.GetDate(row.Cells["SeasonActivationDate"].Value.ToString(), true);
                tempCommand = String.Format("{0:00}", dateTime.Day.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", dateTime.Month.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", Convert.ToInt16(row.Cells["SeasonNumber"].Value.ToString()));
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todCommand += ProgrammingCommon.GetASCIIValue(tempCommand);
            }
            todCommands.Add("01573102" + todAddress + "28" + todCommand + "29" + "03");
            return todCommands;

        }

        // To get Mode of Billing 
        private string GetModeOfBilling()
        {
            try
            {
                data = "";
                group = this.billingReset1.Controls["gbAutoMode"];
                ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
                ComboBox cmbSelectDay = (ComboBox)group.Controls["cmbSelectDay"];
                ComboBox cmbSelectHour = (ComboBox)group.Controls["cmbSelectHour"];
                ComboBox cmbSelectMinutes = (ComboBox)group.Controls["cmbSelectMinutes"];
                if (cmbModeofBilling.SelectedItem == "End of Month")
                {
                    data += ProgrammingCommon.GetASCIIValue("00");
                    // The values are fixed for day,hour,minutes to complete data
                    data += EnumUtil.StringValue(BillingMode.EndofMonth); //"393039303930";
                }
                if (cmbModeofBilling.SelectedItem == "User Defined")
                {
                    data += ProgrammingCommon.GetASCIIValue("01");
                    if (cmbSelectDay.SelectedIndex.ToString().Length == 1)
                    {
                        data += "30";
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectDay.SelectedItem.ToString());
                    }
                    else
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectDay.SelectedItem.ToString());
                    if (cmbSelectHour.SelectedIndex.ToString().Length == 1)
                    {
                        data += "30";
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectHour.SelectedItem.ToString());
                    }
                    else
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectHour.SelectedItem.ToString());
                    if (cmbSelectMinutes.SelectedIndex.ToString().Length == 1)
                    {
                        data += "30";
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectMinutes.SelectedItem.ToString());
                    }
                    else
                        data += ProgrammingCommon.GetASCIIValue(cmbSelectMinutes.SelectedItem.ToString());
                }


            }
            catch (Exception)
            {
                data = null;
            }
            return data;

        }

        // To get the Billing Reset value from UI while creating the configuration file. 
        private string GetBillingResetStatus()
        {
            try
            {
                data = "";
                group = this.reset1;
                CheckBox chkBillingReset = (CheckBox)group.Controls["chkBillingReset"];
                if (chkBillingReset.Checked == true)
                {
                    data += ProgrammingCommon.GetASCIIValue("01");
                }
                else 
                {
                    data += ProgrammingCommon.GetASCIIValue("00");
                }

            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }


       // To get the Billing Period value from UI when performing write meter configurations. 
        private string GetBillingPeriod()
        {
            try
            {
                data = "";
                group = this.billingReset1.Controls["gbAutoMode"];
                RadioButton rdbBillingPeriod = (RadioButton)group.Controls["rbtnMonthly"];
                if (rdbBillingPeriod.Checked == true)
                {
                    data += ProgrammingCommon.GetASCIIValue("02");
                }
                rdbBillingPeriod = (RadioButton)group.Controls["rbtnOddMonth"];
                if (rdbBillingPeriod.Checked == true)
                {
                    data += ProgrammingCommon.GetASCIIValue("01");
                }
                rdbBillingPeriod = (RadioButton)group.Controls["rbtnEvenMonth"];
                if (rdbBillingPeriod.Checked == true)
                {
                    data += ProgrammingCommon.GetASCIIValue("00");
                }

            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }

        // To get the Reset Lock out days value from UI when performing write meter configurations. 
        private string GetResetLockOutDays()
        {
            try
            {
                data = "";

                group = this.billingReset1.Controls["gbManual"];
                ComboBox cmbResetLockOutDays = (ComboBox)group.Controls["cmbResetLockoutdays"];
                int[] ResetLockOutDays = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                for (int i = 0; i < ResetLockOutDays.Length; i++)
                {
                    if (cmbResetLockOutDays.SelectedIndex == ResetLockOutDays[i])
                    {
                        data += "30";
                        int ResetLockOutDaysDecimal = Convert.ToInt32(cmbResetLockOutDays.SelectedItem);
                        string ResetLockOutDaysHexaDecimal = Convert.ToString(ResetLockOutDaysDecimal, 16).ToUpper(); ;
                        data += ProgrammingCommon.GetASCIIValue(ResetLockOutDaysHexaDecimal);
                        return data;
                    }
                }

                int ResetLockOutDaysDec = Convert.ToInt32(cmbResetLockOutDays.SelectedItem);
                string ResetLockOutDaysHexa = Convert.ToString(ResetLockOutDaysDec, 16).ToUpper(); ;
                data += ProgrammingCommon.GetASCIIValue(ResetLockOutDaysHexa);

            }
            catch (Exception)
            {
                return data;
            }
            return data;
        }

        /// <summary>
        /// This function checks whether user has selected all the required values in the TOD window
        /// and the selections are complete and valid.
        /// </summary>
        /// <param name="gridTOU"></param>
        /// <returns>True if the selections are valid, else False</returns>
        private bool CheckTOUSlots(DataGridView gridTOU)
        {
            if (gridTOU.Rows.Count == 0 || gridTOU.Rows[0].Cells[1].Value.ToString() == "00")
            {
                this.StatusMessage = resourceMgr.GetString("ValidationMsgIncompleteSlots_TOD");
                // this.Cursor = Cursors.Default;
                Application.DoEvents();
                errmsg = errmsg + "\n" + resourceMgr.GetString("ValidationMsg_TOD") + " " + resourceMgr.GetString("ValidationMsg_TODSeasonSlots");
                return false;
            }
            if (!this.touOperation1.isValidTOU)
            {
                this.StatusMessage = resourceMgr.GetString("ValidationMsg_TOD")+ " " +  resourceMgr.GetString("ValidationMsg_TODInvalidEntry");
                Application.DoEvents();
                errmsg = errmsg + "\n" + resourceMgr.GetString("ValidationMsg_TOD") + " " + resourceMgr.GetString("ValidationMsg_TODInvalidEntry");
                return false;
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
                            this.StatusMessage = resourceMgr.GetString("ValidationMsg_TOD")+ " "+ resourceMgr.GetString("ValidationMsg_TODInvalidEntry");
                            Application.DoEvents();
                            errmsg = errmsg + "\n" + resourceMgr.GetString("ValidationMsg_TOD")+ " " + resourceMgr.GetString("ValidationMsg_TODInvalidEntry");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// This function checks whether the user has entered valid values for the future tod activation date 
        /// and the season activation dates.
        /// </summary>
        /// <returns>True if the selections are valid, else False</returns>
        private bool CheckActivationDate()
        {
            DateTime prevDate = new DateTime();
            DateTime currentDate = new DateTime();
            DateTime futureActivationDate = DateTime.Now.AddDays(1).Date;
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];

            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                if (row.Index == 0)
                {
                    prevDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(), false);
                }
                else
                {
                    currentDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(), true);
                    if (prevDate.Day == currentDate.Day && prevDate.Month == currentDate.Month)
                    {
                        this.StatusMessage = resourceMgr.GetString("ValidationMsg_UniqueActivationDate");
                        errmsg = errmsg + "\n" + resourceMgr.GetString("ValidationMsg_TOD") + " " + resourceMgr.GetString("ValidationMsg_UniqueActivationDate");
                        return false;
                    }
                    prevDate = currentDate.Date;
                }
                if (prevDate.Day == 29 && prevDate.Month == 2)
                {
                    this.StatusMessage = " " + resourceMgr.GetString("ValidationMsg_TODActivationDate");
                    errmsg = errmsg + "\n" + resourceMgr.GetString("ValidationMsg_TOD") + " " + resourceMgr.GetString("ValidationMsg_TODActivationDate");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This function promts the user to make valid selections while entering data in the TOD window.
        /// </summary>
        /// <returns>True if the selections are valid, else False</returns>
        private bool ValidateTOUData()
        {
            DataGridView[] gridSeason = GetSeasonGridCollection();
            DataGridView[] gridHoliday = GetHolidayGridCollection();

            foreach (DataGridView gridTOU in gridSeason)
                if (!CheckTOUSlots(gridTOU))
                {
                    //this.StatusMessage = "Invalid Entry!";
                    return false;
                }
            if (!CheckActivationDate())
            {
                //this.StatusMessage = string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                return false;
            }
            return true;
        }

        /// <summary>
        /// This function returens a DataGridViewComboBoxColumn containing the required serial numbers
        /// for the ten slots of all the TOD data grids.
        /// </summary>
        /// <returns>The resulting dataGridViewComboBoxColumn</returns>
        internal DataGridViewComboBoxColumn GetSNo()
        {
            DataGridViewComboBoxColumn colSNo = new DataGridViewComboBoxColumn();
            colSNo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colSNo.Name = "SNo.";
            colSNo.HeaderText = "SNo.";
            for (int sNoIndex = 1; sNoIndex <= 10; sNoIndex++)
            {
                colSNo.Items.Add(Convert.ToString(sNoIndex));
            }
            return colSNo;
        }

        /// <summary>
        /// This function returens a DataGridViewComboBoxColumn containing the required tariffs
        /// for all the TOD data grids.
        /// </summary>
        /// <returns>The resulting dataGridViewComboBoxColumn</returns>
        internal DataGridViewComboBoxColumn GetRates()
        {
            DataGridViewComboBoxColumn colRate = new DataGridViewComboBoxColumn();
            colRate.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colRate.Name = "Rate";
            colRate.HeaderText = "Rate";
            for (int rateIndex = 1; rateIndex <= 8; rateIndex++)
            {
                colRate.Items.Add( "T" + Convert.ToString(rateIndex));
            }
            colRate.Items.Add("00");
            return colRate;
        }

        /// <summary>
        /// This function returens a DataGridViewComboBoxColumn containing the required values
        /// for start hours of tariffs for all the TOD data grids.
        /// </summary>
        /// <returns>The resulting dataGridViewComboBoxColumn</returns>
        internal DataGridViewComboBoxColumn GetStartHour()
        {
            DataGridViewComboBoxColumn colStartHour = new DataGridViewComboBoxColumn();
            colStartHour.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartHour.Name = "Start Hour";
            colStartHour.HeaderText = "Start Hour";
            int hourIndex = 0;
            string hourString = "";
            for ( hourIndex = 0; hourIndex <= 23; hourIndex++)
            {

                if (hourIndex.ToString().Length < 2) { hourString = "0" + hourIndex.ToString(); }
                else { hourString = hourIndex.ToString(); }
                colStartHour.Items.Add(Convert.ToString(hourString));
            }
            return colStartHour;
        }

        /// <summary>
        /// This function returens a DataGridViewComboBoxColumn containing the required values
        /// for start minutes of tariffs for all the TOD data grids.
        /// </summary>
        /// <returns>The resulting dataGridViewComboBoxColumn</returns>
        internal DataGridViewComboBoxColumn GetStartMinute()
        {
            DataGridViewComboBoxColumn colStartMinute = new DataGridViewComboBoxColumn();
            colStartMinute.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartMinute.Name = "Start Minute";
            colStartMinute.HeaderText = "Start Minute";
            colStartMinute.Items.Add("00");
            for (int minIndex = 15; minIndex <= 45; minIndex+=15)
            {
                colStartMinute.Items.Add(minIndex.ToString());
            }
            return colStartMinute;
        }

        private void meterPassword_OnValuesSubmission(string password, int CTRatio)
        {
            this.meterPwd = password;
            this.ctRatio = CTRatio;
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
                    cmbDemandSubInterlavTime.Enabled = false;
                }
                else 
                {
                    cmbDemandSubInterlavTime.Enabled = true;
                    cmbDemandSubInterlavTime.SelectedIndex = -1;
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
                    cmbDemandSubInterlavTime.Enabled = false;
                }
                else 
                {
                    cmbDemandSubInterlavTime.SelectedIndex = -1;
                    cmbDemandSubInterlavTime.Enabled = true;
                }
            }
        }
        /// <summary>

        #region Write Event.
        private void btnWrite_Click(object sender, EventArgs e)
        {
            pnConfigOptions.Enabled = false;
            this.StatusMessage = "";
            txterrorLog.Text = "";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            WriteConfigurations writeConfig = new WriteConfigurations();
            //swLog = new StreamWriter(filePath, false);
            try
            {
                if (CheckValidations("write"))
                {  
                    ////Password made configurable by the user 
                    //MeterPassword meterPassword = new MeterPassword(false);
                    //meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
                    //meterPassword.ShowDialog();
                    //if (this.meterPwd.Length == 0)
                    //    return;
                    ////Password made configurable by the user 
                    this.Cursor = Cursors.WaitCursor;
                    string rtcMsg = "";
                    errmsg = "";
                    Collection<MeterConfigurationConfigSection> collectionConfigSection = new Collection<MeterConfigurationConfigSection>();
                    Application.DoEvents();
//                    ManufactureCommands_Write(ref rtcMsg);
                    

                    if (chkMDWithIP.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (CheckMDwithIP())
                        {
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("MDWithIP")));
                        }
                        else
                        {
                            //                                MessageBox.Show(resourceMgr.GetString("selectMDWithIPMsg"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            errmsg = errmsg + "\n" + "MD With IP : " + resourceMgr.GetString("selectMDWithIPMsg");
                            this.StatusMessage = resourceMgr.GetString("selectMDWithIPMsg");
                            return;
                        }
                    }
                    if (chkKVARSelcetion.Checked)
                    {
                        collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("kvahSelection")));
                    }

                    if (chkTOD.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (!ValidateTOUData())
                        {
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("TOD")));
                    }

                    #region Display Parameters
                    //Code Region Added by Vivek on 9 August 2011 for Display Parameters Write in meter(TNEB Project)
                    //If Display Parameters checkbox is checked and atleast a single parameter is  selected for each displayparamater type to write in meter.
                    Collection<Collection<String>> selectedDisplayParameters = new Collection<Collection<string>>();
                    if (chkDisplayParam.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        string displayParamValidationStatus = ValidateDisplayParameterInputs();
                        if (displayParamValidationStatus == string.Empty)
                        {
                            Collection<DisplayParamatersDBEntity> collectionDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
                            collSelectedDisplayParameters = new Collection<Collection<string>>();
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("DisplayParameters")));
                            //Get Configuration section object for Scroll Mode Parameter.
                            collectionConfigSection.Add(XMLLoader.GetConfigurationSection(DisplayParameter.ScrollMode));
                            collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.ScrollMode, collectionDisplayParamatersDBEntity));
                            // Get Configuration section object for Push Mode Parameter.
                            collectionConfigSection.Add(XMLLoader.GetConfigurationSection(DisplayParameter.PushMode));
                            collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.PushMode, collectionDisplayParamatersDBEntity));
                            //Get Configuration section object for HighResolution Mode Parameter.
                            collectionConfigSection.Add(XMLLoader.GetConfigurationSection(DisplayParameter.HighResolutionMode));
                            collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.HighResolutionMode, collectionDisplayParamatersDBEntity));
                            //Get Configuration Section for Display Timeouts.
                            collectionConfigSection.Add(XMLLoader.GetConfigurationSection(DisplayParameter.DisplayTimeouts));
                            collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.DisplayTimeouts, collectionDisplayParamatersDBEntity));
                            //DisplayParametersBLL displayParametersBLL = new DisplayParametersBLL();
                            //if (!displayParametersBLL.InsertData(collDisplayParamatersDBEntity))
                            //    this.StatusMessage = EnumUtil.StringValue(DisplayParamaterStatusMessage.DisplayParamaters_DBInsertionFailure);
                        }
                        else
                        {
                            this.StatusMessage = displayParamValidationStatus;
                            errmsg = errmsg + "\n" + "Display Parameters : " + displayParamValidationStatus;
                            return;
                        }
                    }
                    #endregion


                    if (chkDailyLog.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        //int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("Delete"), resourceMgr.GetString("BCS"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                        //if (msgres != 6)
                        //    return;
                        if (CheckDailyLog())
                        {
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("DailyLog")));
                        }
                        else
                        {
                            //MessageBox.Show(resourceMgr.GetString("ValidationMsg_DailyLog"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            this.StatusMessage = resourceMgr.GetString("ValidationMsg_DailyLog");
                            errmsg = errmsg + "\n" + "Daily Log : " + resourceMgr.GetString("ValidationMsg_DailyLog");
                            return;
                        }
                    }
                    // Billing Reset Write
                    if (chkBilingReset.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (CheckBillingReset())
                        {
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ModeOfBilling")));
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("BillingPeriod")));
                            collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ResetLockOutDays")));
                        }
                        else
                        {
                            //  MessageBox.Show(resourceMgr.GetString("ValidationMsg_BillingReset"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            this.StatusMessage = resourceMgr.GetString("ValidationMsg_BillingReset");
                            errmsg = errmsg + "\n" + "Billing Reset : " + resourceMgr.GetString("ValidationMsg_BillingReset");
                            return;
                        }
                    }
                    if (chkLockRS232.Checked)
                    {
                        collectionConfigSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("LockUnlockRS232")));
                    }

                    if (chkReset.Checked)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (((CheckBox)(tabReset.Controls[0].Controls["chkBillingReset"])).Checked == false)
                        {
                            this.StatusMessage = resourceMgr.GetString("ValidationMsg_Reset_BillingReset");// "Billing Reset option not checked in Reset Tab";
                            return ;
                        }
                      
                    }

                    //2 march 2012: This check shifted after the validation check
                    if (chkDailyLog.Checked)
                    {
                        int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("Delete"), resourceMgr.GetString("BCS"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                        if (msgres != 6)
                            return;
                    }

                    //2 march 2012: password window shifted after the validation check
                    MeterPassword meterPassword = new MeterPassword(false);
                    meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
                    meterPassword.ShowDialog();
                    if (this.meterPwd.Length == 0)
                        return;
                    //2 march 2012: password window shifted after the validation check
                    if (collectionConfigSection.Count > 0)
                    {
                        this.StatusMessage = resourceMgr.GetString("ProgressMsg_Write");
                        Application.DoEvents();
                        string res = "";
                        string msg = "";
                        foreach (MeterConfigurationConfigSection section in collectionConfigSection)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            if (!String.IsNullOrEmpty(writeConfig.HandshakeCommands(meterPwd, false)))
                            {
                                //res = "";
                                //msg = "";
                                if (section.Name.ToLower() == "TOD".ToLower())
                                    writeConfig.WriteTODConfigurations(GetTODCommands());
                                else if ((section.Name.ToLower() == DisplayParameter.PushMode.ToString().ToLower()) || section.Name.ToLower() == DisplayParameter.ScrollMode.ToString().ToLower())
                                {
                                    WriteDisplayParameters(section, writeConfig, ref msg);
                                }
                                else
                                    res = writeConfig.WriteMeterConfigurations(meterPwd, section, GetDataForCommand(section.Name), false, ref msg);
                                if (res == null)
                                {
                                    this.StatusMessage = msg;
                                    writeConfig.BreakCommunication();
                                    return;
                                }
                                if (!(chkRTC.Checked))//if(!(chkRTC.Checked || chkBilingReset.Checked))
                                this.StatusMessage = resourceMgr.GetString("SuccessMsg_Write");
                            }
                            else
                            {
                                this.StatusMessage = writeConfig.StatusMessage;
                            }
                            writeConfig.BreakCommunication();
                        }
                        writeConfig.BreakCommunication();

                        if (writeConfig.DailyLogFlag == true)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            if (!String.IsNullOrEmpty(writeConfig.HandshakeCommands(meterPwd, true)))
                            {
                                foreach (MeterConfigurationConfigSection section in collectionConfigSection)
                                {
                                    bool writeConfigResult;
                                    //Changed on 9th march 2012 as per validation report. The function now accepts a password string as second parameter
                                    writeConfigResult = writeConfig.WriteMeterCongigurationsDailyLogDelete(section,meterPwd);
                                }
                            }
                            writeConfig.BreakCommunication();
                        }
                    }

                    if (chkRTC.Checked || chkReset.Checked)
                        ManufactureCommands_Write(ref rtcMsg);
                    if (this.StatusMessage == "Timeout!" || this.StatusMessage=="Sign-On failure")
                        return;
                    //}
                    //else
                    //{
                    //        this.StatusMessage = rtcMsg;
                    //}
                }
            }
            catch (Exception)
            {
                this.StatusMessage = resourceMgr.GetString("FailureMsg_Write");

            }
            finally
            {
                this.Cursor = Cursors.Default;
                writeConfig.BreakCommunication();
                pnConfigOptions.Enabled = true;
                //swLog.Close();
            }
            if (errmsg.Trim().Length > 0)
            {
                txterrorLog.Text = errmsg.Insert(0, "FAILURES DURING EXECUTION\n---------------------------------------------------------\n");
            }
        }
        #endregion

        /// <summary>
        /// This function sends the write command for programming the selected display parameters into the meter.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="writeConfig"></param>
        /// <param name="msg"></param>
        private void WriteDisplayParameters(MeterConfigurationConfigSection section, WriteConfigurations writeConfig, ref string msg)
        {
            int z = section.Name.ToLower() == DisplayParameter.PushMode.ToString().ToLower() ? 1 : 0;
            DisplayParametersBLL DisplayParametersBLL;
            if (z == 0) DisplayParametersBLL = new PushModeParameterBLL(); else DisplayParametersBLL = new ScrollModeParameterBLL();
            string tmp = section.WriteCommand;
            string[] cmds = section.WriteCommand.Split('|');
            string cmdData = "";
            section.WriteCommand = cmds[0];
            if (collSelectedDisplayParameters[z].Count > 64)
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[z], 0, 63);
                writeConfig.WriteMeterConfigurations(meterPwd, section, cmdData, false, ref msg);
                section.WriteCommand = cmds[1];
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[z], 64, collSelectedDisplayParameters[z].Count - 1);
                writeConfig.WriteMeterConfigurations(meterPwd, section, cmdData, false, ref msg);
            }
            else
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[z], 0, collSelectedDisplayParameters[z].Count - 1);
                writeConfig.WriteMeterConfigurations(meterPwd, section, cmdData, false, ref msg);
            }
            section.WriteCommand = tmp;
        }

        #region ManufactureCommands_Write

        /// <summary>
        /// This function sends the command to wite the meter RTC and do billing reset from the meter.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>True if the write is successful, else False</returns>
        private bool ManufactureCommands_Write(ref string msg)
        {
            WriteConfigurations writeConfig = new WriteConfigurations();
            Collection<MeterConfigurationConfigSection> collectionConfigSection2 = new Collection<MeterConfigurationConfigSection>();
            this.StatusMessage = resourceMgr.GetString("ProgressMsg_Write");
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            #region Manufacture Specific Commands.
            //Code Region Added by Vivek on 26 August 2011 for RTC Parameter Write in meter(TNEBProject)
            //If RTC Paramater is checked
            if (chkRTC.Checked || chkReset.Checked)
            {
                if (chkRTC.Checked)
                {
                    sysTime = DateTime.Now;
                   
                    if (ValidateRTCTimeFrame(sysTime))
                    {
                        RTCInformation rtcInformation = new RTCInformation();
                        LocalCommunication communications;
                        communications = ChannelManager.GetChannel() as LocalCommunication;
                        rtcInformation.RTCDateTime = sysTime;
                        rtcInformation.Channel = communications;

                        rtcInformation.MeterPassword = "11111111";
                        this.StatusMessage = resourceMgr.GetString("ProgressMsg_Write");
                        Application.DoEvents();
                        rtcInformation.SetRTC(meterPwd);
                        
                        if (this.StatusMessage.Length > 0 && (!this.StatusMessage.Contains("Writ")))
                        errmsg = errmsg + "\n" + "RTC : " + this.StatusMessage;
                        if (rtcInformation.StatusMessage == "RTC updated sucessfully.")
                            this.StatusMessage = resourceMgr.GetString("SuccessMsg_Write");
                        else
                            this.StatusMessage = rtcInformation.StatusMessage;

                    }
                    else
                    {
                        if (this.StatusMessage == "Timeout!" || this.StatusMessage == "Sign-On failure")
                            return false;
                        msg = resourceMgr.GetString("ValidationMsg_RTC");
                        if (!errmsg.Contains("RTC"))
                        {
                            this.StatusMessage = msg;
                            errmsg = errmsg + "\n" + "RTC : " + msg;
                        }
                        return false;
                    }
                }
                if (chkReset.Checked)
                {
                    if (((CheckBox)(tabReset.Controls[0].Controls["chkBillingReset"])).Checked == true)
                        collectionConfigSection2.Add(XMLLoader.GetConfigSection(GetConfigParameters("BillingReset")));
                    else
                    {
                        msg = resourceMgr.GetString("ValidationMsg_Reset_BillingReset");// "Billing Reset option not checked in Reset Tab";
                        errmsg = errmsg + "\n" + "Reset : " + msg;
                        return false;
                    }
                }
                if (collectionConfigSection2.Count > 0 && (!String.IsNullOrEmpty(writeConfig.HandshakeCommands(meterPwd, true))))
                {//Condition added on 29 aug 2011 by Vivek. 
                    string res = string.Empty;
                    foreach (MeterConfigurationConfigSection section in collectionConfigSection2)
                    {
                        // writeConfig.WriteMeterConfigurations(section, "3A31313131313131314D410D0A", true);
                        if (section.Name.ToLower() == ConfigurationParameter.BillingReset.ToString().ToLower())
                        {
                            res = writeConfig.WriteMeterConfigurations(meterPwd, section, GetDataForCommand(section.Name), true, ref msg);
                            if (res == "Success")
                                this.StatusMessage = resourceMgr.GetString("SuccessMsg_Write");
                            else
                                this.StatusMessage = msg;
                        }
                        else
                        {//063035360D0A3A31313131313131314D41360D0A
                            this.StatusMessage = resourceMgr.GetString("RTC_Write");
                            Application.DoEvents();
                            writeConfig.ExecuteCommand("3A31313131313131314D410D0A");
                            res = writeConfig.WriteMeterConfigurations(meterPwd, section, GetDataForCommand(section.Name), true, ref msg);
                            if (res.Trim().Length == 0)
                            {
                                writeConfig.BreakCommunication();
                                return false;
                            }
                            else this.StatusMessage = resourceMgr.GetString("RTC_WriteSuccess");

                        }
                        //this.StatusMessage = resourceMgr.GetString("RTCUpdatedSuccess"); //"RTC updated successfully.";
                        if (res == null)
                        {
                            writeConfig.BreakCommunication();
                            return false;
                        }
                    }
                    // this.StatusMessage = resourceMgr.GetString("ExecutionSuccessMsg_ManufactureModeWrite");
                }
                else
                {
                    if (writeConfig.StatusMessage != null)
                    {
                        this.StatusMessage = writeConfig.StatusMessage;
                        errmsg = errmsg + "\n" + this.StatusMessage;
                    }
                }
                writeConfig.BreakCommunication();
            }
            #endregion

            return true;
        }
        #endregion

        #region RTC Read

        /// <summary>
        /// This command reads the meter RTC.
        /// </summary>
        /// <returns>The current Meter RTC</returns>
        private DateTime? ReadRTC()
        {
            ReadConfigurations readConfig = new ReadConfigurations();
            ReadResult readResult = new ReadResult();

            #region RTC Read
            RTCInformation rtcInformation = new RTCInformation();
            rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel1_OnStatusChanged);
            meterRTCData = string.Empty;
            DateTime rtc;
            try
            {
                //InitializeProgrammingValues();
                LocalCommunication communications;
                communications = ChannelManager.GetChannel() as LocalCommunication;
                rtcInformation.Channel = communications;
                //this.Cursor = Cursors.WaitCursor;
                this.StatusMessage = string.Empty;
                string statusMsg = "";
                meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                if (statusMsg == "Timeout!" || statusMsg == "Sign-On failure" || statusMsg=="Invalid RTC.")
                {
                    this.StatusMessage = statusMsg;
                    return null;
                }
                if (this.StatusMessage.Length > 0)
                    errmsg = errmsg + "\n" + this.StatusMessage;

                string tempData = meterRTCData.Substring(meterRTCData.IndexOf("|") + 2, meterRTCData.Length - meterRTCData.IndexOf("|") - 2);
                return DateTime.Parse(new FormatterConfigurations().ParseRTC(tempData), new System.Globalization.CultureInfo("en-GB"));
            }
            catch (Exception r)
            {
                errmsg = errmsg + "\n" + "RTC :Unable to parse value from meter" + ":-- " + meterRTCData.Replace("\n", "").Replace("\r", "");
            }
            return null;
            #endregion
        }
        #endregion

        #region ValidateRTC Write

        /// <summary>
        /// This function checks whehter the RTC read from the meter is in the correct format.
        /// </summary>
        /// <param name="sysTime"></param>
        /// <returns>True if the RTC value is valid, else False</returns>
        private bool ValidateRTCTimeFrame(DateTime sysTime)
        {
            RTCBLL rtcBLL = new RTCBLL();
            DateTime? rtcDateTime = ReadRTC();
            if (this.StatusMessage == "Invalid RTC.")
                return true;
            if (this.StatusMessage == "Timeout!")
                return false;
            if (rtcDateTime == null) return false;

            TimeSpan ts = sysTime.Subtract(rtcDateTime.Value);
            if (ts.TotalMinutes < 0 && Math.Abs(ts.TotalMinutes) <= ConfigInfo.GetRTCTimeFrame())
            {
                //   this.StatusMessage = resourceMgr.GetString("RTCValidationSuccess");
                //   Application.DoEvents();
                return true;
            }
            else if (ts.TotalMinutes >= 0.0)
            {
                //    this.StatusMessage = resourceMgr.GetString("RTCValidationSuccess");
                //  Application.DoEvents();
                return true;
            }
            else
                return false;
        }
        #endregion

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                chkMDWithIP.Checked = true;
                chkKVARSelcetion.Checked = true;
                chkDisplayParam.Checked = true;
                chkTOD.Checked = true;
                chkRTC.Checked = true;
                chkBilingReset.Checked = true;
                chkReset.Checked = true;
                chkDailyLog.Checked = true;
                chkLockRS232.Checked = true;
            }
            else
            {
                chkMDWithIP.Checked = false;
                chkKVARSelcetion.Checked = false;
                chkDisplayParam.Checked = false;
                chkTOD.Checked = false;
                chkRTC.Checked = false;
                chkBilingReset.Checked = false;
                chkReset.Checked = false;
                chkDailyLog.Checked = false;
                chkLockRS232.Checked = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This function retrives an array of the season datagridview controls from the TOD user control 
        /// so that they can be worked upon.
        /// </summary>
        /// <returns>The datagridview array.</returns>
        private DataGridView[] GetSeasonGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgS1"].Controls["tbSeason1"];
            DataGridView gridS1Day1 = (DataGridView)subGroup.Controls["tbPgS1D1"].Controls["gridS1Day1"];
            DataGridView gridS1Day2 = (DataGridView)subGroup.Controls["tbPgS1D2"].Controls["gridS1Day2"];
            DataGridView gridS1Day3 = (DataGridView)subGroup.Controls["tbPgS1D3"].Controls["gridS1Day3"];
            DataGridView gridS1Day4 = (DataGridView)subGroup.Controls["tbPgS1D4"].Controls["gridS1Day4"];
            DataGridView gridS1Day5 = (DataGridView)subGroup.Controls["tbPgS1D5"].Controls["gridS1Day5"];
            DataGridView gridS1Day6 = (DataGridView)subGroup.Controls["tbPgS1D6"].Controls["gridS1Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS2"].Controls["tbSeason2"];
            DataGridView gridS2Day1 = (DataGridView)subGroup.Controls["tbPgS2D1"].Controls["gridS2Day1"];
            DataGridView gridS2Day2 = (DataGridView)subGroup.Controls["tbPgS2D2"].Controls["gridS2Day2"];
            DataGridView gridS2Day3 = (DataGridView)subGroup.Controls["tbPgS2D3"].Controls["gridS2Day3"];
            DataGridView gridS2Day4 = (DataGridView)subGroup.Controls["tbPgS2D4"].Controls["gridS2Day4"];
            DataGridView gridS2Day5 = (DataGridView)subGroup.Controls["tbPgS2D5"].Controls["gridS2Day5"];
            DataGridView gridS2Day6 = (DataGridView)subGroup.Controls["tbPgS2D6"].Controls["gridS2Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS3"].Controls["tbSeason3"];
            DataGridView gridS3Day1 = (DataGridView)subGroup.Controls["tbPgS3D1"].Controls["gridS3Day1"];
            DataGridView gridS3Day2 = (DataGridView)subGroup.Controls["tbPgS3D2"].Controls["gridS3Day2"];
            DataGridView gridS3Day3 = (DataGridView)subGroup.Controls["tbPgS3D3"].Controls["gridS3Day3"];
            DataGridView gridS3Day4 = (DataGridView)subGroup.Controls["tbPgS3D4"].Controls["gridS3Day4"];
            DataGridView gridS3Day5 = (DataGridView)subGroup.Controls["tbPgS3D5"].Controls["gridS3Day5"];
            DataGridView gridS3Day6 = (DataGridView)subGroup.Controls["tbPgS3D6"].Controls["gridS3Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS4"].Controls["tbSeason4"];
            DataGridView gridS4Day1 = (DataGridView)subGroup.Controls["tbPgS4D1"].Controls["gridS4Day1"];
            DataGridView gridS4Day2 = (DataGridView)subGroup.Controls["tbPgS4D2"].Controls["gridS4Day2"];
            DataGridView gridS4Day3 = (DataGridView)subGroup.Controls["tbPgS4D3"].Controls["gridS4Day3"];
            DataGridView gridS4Day4 = (DataGridView)subGroup.Controls["tbPgS4D4"].Controls["gridS4Day4"];
            DataGridView gridS4Day5 = (DataGridView)subGroup.Controls["tbPgS4D5"].Controls["gridS4Day5"];
            DataGridView gridS4Day6 = (DataGridView)subGroup.Controls["tbPgS4D6"].Controls["gridS4Day6"];

            DataGridView[] seasonGrids = new DataGridView[] 
            {
                gridS1Day1, gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6,
                gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, 
                gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6,
                gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6
            };
            return seasonGrids;
        }

        /// <summary>
        /// This function retrives an array of the holiday datagridview controls from the TOD user control 
        /// so that they can be worked upon.
        /// </summary>
        /// <returns>The datagridview array</returns>
        private DataGridView[] GetHolidayGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgHoliday"].Controls["tbHoliday"];
            DataGridView gridHoliday1 = (DataGridView)subGroup.Controls["tbPgHDay1"].Controls["gridHoliday1"];
            DataGridView gridHoliday2 = (DataGridView)subGroup.Controls["tbPgHDay2"].Controls["gridHoliday2"];
            DataGridView gridHoliday3 = (DataGridView)subGroup.Controls["tbPgHDay3"].Controls["gridHoliday3"];
            DataGridView gridHoliday4 = (DataGridView)subGroup.Controls["tbPgHDay4"].Controls["gridHoliday4"];
            DataGridView gridHoliday5 = (DataGridView)subGroup.Controls["tbPgHDay5"].Controls["gridHoliday5"];
            DataGridView gridHoliday6 = (DataGridView)subGroup.Controls["tbPgHDay6"].Controls["gridHoliday6"];
            DataGridView gridHoliday7 = (DataGridView)subGroup.Controls["tbPgHDay7"].Controls["gridHoliday7"];
            DataGridView gridHoliday8 = (DataGridView)subGroup.Controls["tbPgHDay8"].Controls["gridHoliday8"];
            DataGridView gridHoliday9 = (DataGridView)subGroup.Controls["tbPgHDay9"].Controls["gridHoliday9"];
            DataGridView gridHoliday10 = (DataGridView)subGroup.Controls["tbPgHDay10"].Controls["gridHoliday10"];

            DataGridView[] holidayGrids = new DataGridView[] 
            {
                gridHoliday1,gridHoliday2, gridHoliday3, gridHoliday4, gridHoliday5,
                gridHoliday6, gridHoliday7, gridHoliday8, gridHoliday9, gridHoliday10
            };
            return holidayGrids;
        }

        /// <summary>
        /// This function retrives an array of the Day Assignment datagridview controls
        /// in the TOD control so that they can be worked upon.
        /// </summary>
        /// <returns>The datagridview array</returns>
        private DataGridView[] GetAssignmentGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgDayAssignment"].Controls["tbDayAssignment"];
            DataGridView gridAssignmentS1 = (DataGridView)subGroup.Controls["tbPgDayAssignS1"].Controls["gridAssignmentS1"];
            DataGridView gridAssignmentS2 = (DataGridView)subGroup.Controls["tbPgDayAssignS2"].Controls["gridAssignmentS2"];
            DataGridView gridAssignmentS3 = (DataGridView)subGroup.Controls["tbPgDayAssignS3"].Controls["gridAssignmentS3"];
            DataGridView gridAssignmentS4 = (DataGridView)subGroup.Controls["tbPgDayAssignS4"].Controls["gridAssignmentS4"];

            DataGridView[] dayAssignmentGrids = new DataGridView[] 
            {
                gridAssignmentS1, gridAssignmentS2, gridAssignmentS3, gridAssignmentS4
            };
            return dayAssignmentGrids;
        }

        /// <summary>
        /// This function retrives an array of the dateTimePicker controls in the Activation date tab  
        /// in the TOD control so that they can be worked upon.
        /// </summary>
        /// <returns>The dateTimePicker array</returns>
        private DateTimePicker[] GetActivationDateCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgHoliday"].Controls["tbHoliday"];
            DateTimePicker dtPicAcDate1 = (DateTimePicker)subGroup.Controls["tbPgHDay1"].Controls["dtPicAcDate1"];
            DateTimePicker dtPicAcDate2 = (DateTimePicker)subGroup.Controls["tbPgHDay2"].Controls["dtPicAcDate2"];
            DateTimePicker dtPicAcDate3 = (DateTimePicker)subGroup.Controls["tbPgHDay3"].Controls["dtPicAcDate3"];
            DateTimePicker dtPicAcDate4 = (DateTimePicker)subGroup.Controls["tbPgHDay4"].Controls["dtPicAcDate4"];
            DateTimePicker dtPicAcDate5 = (DateTimePicker)subGroup.Controls["tbPgHDay5"].Controls["dtPicAcDate5"];
            DateTimePicker dtPicAcDate6 = (DateTimePicker)subGroup.Controls["tbPgHDay6"].Controls["dtPicAcDate6"];
            DateTimePicker dtPicAcDate7 = (DateTimePicker)subGroup.Controls["tbPgHDay7"].Controls["dtPicAcDate7"];
            DateTimePicker dtPicAcDate8 = (DateTimePicker)subGroup.Controls["tbPgHDay8"].Controls["dtPicAcDate8"];
            DateTimePicker dtPicAcDate9 = (DateTimePicker)subGroup.Controls["tbPgHDay9"].Controls["dtPicAcDate9"];
            DateTimePicker dtPicAcDate10 = (DateTimePicker)subGroup.Controls["tbPgHDay10"].Controls["dtPicAcDate10"];

            DateTimePicker[] holidayActivationDates = new DateTimePicker[]
            {
                dtPicAcDate1, dtPicAcDate2, dtPicAcDate3, dtPicAcDate4, dtPicAcDate5,
                dtPicAcDate6, dtPicAcDate7, dtPicAcDate8, dtPicAcDate9, dtPicAcDate10 
            };
            return holidayActivationDates;
        }

        /// <summary>
        /// This function displayes the TOD data (whether the TOD type is 'Current or 'Future') on the UI 
        /// by extracting the values from the data string 'touData' passed as a parameter.
        /// </summary>
        /// <param name="touData"></param>
        /// <param name="touType"></param>
        private void DisplayTOU(string touData, string touType)
        {
            group = new Control();
            int tableIndex = 0;
            int rowIndex = 0;
            DataSet ds = new DataSet();

            this.touOperation1.buttonclicked = true;
            DataGridView[] seasonGrids = GetSeasonGridCollection();

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                if (seasonGrid.Columns.Count == 0)
                {
                    seasonGrid.Columns.Add(GetSNo());
                    seasonGrid.Columns.Add(GetRates());
                    seasonGrid.Columns.Add(GetStartHour());
                    seasonGrid.Columns.Add(GetStartMinute());
                }
            }

            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView holidayGrid in holidayGrids)
            {
                if (holidayGrid.Columns.Count == 0)
                {
                    holidayGrid.Columns.Add(GetSNo());
                    holidayGrid.Columns.Add(GetRates());
                    holidayGrid.Columns.Add(GetStartHour());
                    holidayGrid.Columns.Add(GetStartMinute());
                }
            }

            DataGridView[] dayAssignmentGrids = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();

            ds = ProgrammingCommon.DisplayTOUData(touData, touType);
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
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];
            dtPickerFutureActivationDate.Refresh();
            if (touType == EnumUtil.StringValue(TOUType.Future))
            {
                dtPickerFutureActivationDate.Refresh();
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                dtPickerFutureActivationDate.Value = ProgrammingCommon.GetDate(ProgrammingCommon.futureActivationDate, true);
            }
            else
            {
                dtPickerFutureActivationDate.Refresh();
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                dtPickerFutureActivationDate.Value = DateTime.Now;
            }

            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];
            gridActivation.Rows.Clear();
            foreach (DataRow row in ds.Tables[tableIndex].Rows)
            {
                gridActivation.Rows.Add();
                gridActivation.Rows[rowIndex].Cells[0].Value = row["Season Activation Date"].ToString();
                gridActivation.Rows[rowIndex].Cells[1].Value = Convert.ToInt32(row["Season Number"].ToString()).ToString();
                rowIndex++;
            }

        }


        #region ClearControls
        /// <summary>
        /// This function clears the controls for all the tabs of the MeterConfiguration window.
        /// </summary>
        private void ClearControls()
        {
            cmbDemandType.SelectedIndex = -1;
            cmbDemandInterval.SelectedIndex = -1;
            cmbDemandSubInterlavTime.SelectedIndex = -1;

            RadioButton rdb = (RadioButton)this.kvarSelection1.Controls["grpkvarSelection"].Controls["rdbLagnLead"];
            rdb.Checked = true;

            int index = 0;
            DataGridView dgridDisplayParams = new DataGridView();
            CheckBox chkBox;
            chkBox = (CheckBox)displayParameters.Controls[0].Controls[1].Controls["chkboxSelectAll"];
            for (index = 0; index <= 2; index++)
            {
                if (index == 0)
                {
                    dgridDisplayParams = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
                }
                if (index == 1)
                {
                    dgridDisplayParams = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
                }
                if (index == 2)
                {
                    dgridDisplayParams = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
                }
                for (int i = 0; i < dgridDisplayParams.Rows.Count; i++)
                {
                    dgridDisplayParams.Rows[i].Cells["colInclude"].Value = chkSelectAll.Checked;
                }
                dgridDisplayParams.EndEdit();
            }
            chkBox = (CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"]);
            chkBox.Checked = false;
            TextBox textBox = new TextBox();
            group = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0];
            textBox = (TextBox)group.Controls["txtScrollTime"]; textBox.Text = "";
            textBox = (TextBox)group.Controls["txtPushTimeout"]; textBox.Text = "";
            textBox = (TextBox)group.Controls["txtAutoScrollTime"]; textBox.Text = "";


            // For DailyLog
            group = this.dailyLog1.Controls["gbDailyLog"];
            CheckBox chk1 = (CheckBox)group.Controls["chkSelectAll"];
            chk1.Checked = false;
            if (chk1.Checked == false)
            {
                CheckBox chk = (CheckBox)group.Controls["chkCumulativeKwh"];
                chk.Checked = false;
                chk = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
                chk.Checked = false;
                chk = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
                chk.Checked = false;
                chk = (CheckBox)group.Controls["chkCumulativeKVAh"];
                chk.Checked = false;
                chk = (CheckBox)group.Controls["chkDailyMD1"];
                chk.Checked = false;
                chk = (CheckBox)group.Controls["chkDailyMD2"];
                chk.Checked = false;
            }
            // For Billing Reset 
            group = this.billingReset1.Controls["gbAutoMode"];
            ComboBox cmbModeOfBilling = (ComboBox)group.Controls["cmbModeofBilling"];
            cmbModeOfBilling.Items.Clear();
            //Fix defect #158836
            if (!UtilityDetails.DisableEndOfMonthOptionBillingMode)
            {
                cmbModeOfBilling.Items.Add("End of Month");
            }            
            cmbModeOfBilling.Items.Add("User Defined");
            
            ComboBox cmbSelectDay = (ComboBox)group.Controls["cmbSelectDay"];
            cmbSelectDay.Items.Clear();
            for (int i = 1; i < 29; i++)
            {
                cmbSelectDay.Items.Add(i);
            }
            ComboBox cmbSelectHour = (ComboBox)group.Controls["cmbSelectHour"];
            cmbSelectHour.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                cmbSelectHour.Items.Add(i);
            }
            ComboBox cmbSelectMinutes = (ComboBox)group.Controls["cmbSelectMinutes"];
            cmbSelectMinutes.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                cmbSelectMinutes.Items.Add(i);
            }
            RadioButton rdb1 = (RadioButton)this.billingReset1.Controls["gbAutoMode"].Controls["rbtnMonthly"];
            rdb1.Checked = true;
            group = this.billingReset1.Controls["gbManual"];
            ComboBox cmbResetLockOutDays = (ComboBox)group.Controls["cmbResetLockoutdays"];
            cmbResetLockOutDays.Items.Clear();
            for (int i = 0; i < 256; i++)
            {
                cmbResetLockOutDays.Items.Add(i);
            }
            ResetAllGrids();
            ResetRTC();
            //Billing Reset
            group = this.reset1;
            CheckBox chkReset = (CheckBox)group.Controls["chkBillingReset"];
            chkReset.Checked = false;
            //chkLockUnlockRs232
            chkLockRS232Port.Checked = false;
        }
        #endregion

        # region ResetRTCtab
        /// <summary>
        /// This function clears the 'RTC' tab in the MeterConfigurations window
        /// </summary>
        private void ResetRTC()
        {
            group = new Control();
            group = this.rtcCtrl.Controls["grpBoxRTC"];
            DataGridView gridRTC = (DataGridView)group.Controls["dGridRTC"];
            gridRTC.Rows.Clear();
        }
        # endregion

        # region ResetTODData
        /// <summary>
        /// This function clears the 'TOD' tab in the MeterConfigurations window
        /// </summary>
        private void SetTOUGrids()
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Columns.Clear();
                seasonGrid.Columns.Add(GetSNo());
                seasonGrid.Columns.Add(GetRates());
                seasonGrid.Columns.Add(GetStartHour());
                seasonGrid.Columns.Add(GetStartMinute());
                seasonGrid.Columns[0].ReadOnly = true;
            }
            foreach (DataGridView holidayGrid in holidayGrids)
            {
                holidayGrid.Columns.Clear();
                holidayGrid.Columns.Add(GetSNo());
                holidayGrid.Columns.Add(GetRates());
                holidayGrid.Columns.Add(GetStartHour());
                holidayGrid.Columns.Add(GetStartMinute());
                holidayGrid.Columns[0].ReadOnly = true;
            }
        }


        private void ResetAllGrids()
        {
            SetTOUGrids();
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
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)subGroup.Controls["dtPickerFutureActivationDate"];
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];

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
        # endregion

        private void Channel1_OnStatusChanged(string msg)
        {
            // this.StatusMessage = msg;
        }

        #region Read Meter Configuration(s)
        private void btnRead_Click(object sender, EventArgs e)
        {
            pnConfigOptions.Enabled = false;
            fileText = "";
            this.StatusMessage = "";
            currTODReadResult = "";
            futureTODReadResult = "";
            Application.DoEvents();
            //Password made configurable by the user 
            //Password made configurable by the user 
            this.Cursor = Cursors.WaitCursor;
            
            txterrorLog.Text = "";
            errmsg = "";
            try
            {
                if (CheckValidations("read"))
                {
                    ClearControls();
                    MeterConfigurationsNFEntity meterConfig = new MeterConfigurationsNFEntity();
                    Collection<MeterConfigurationConfigSection> configSection = new Collection<MeterConfigurationConfigSection>();
                    if (chkRTC.Checked)
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("RTC")));

                    List<ReadResult> readResult = new List<ReadResult>();
                    isAborted = false;
                    #region Read Manufacture Specific Commands.
                    if (chkRTC.Checked || chkReset.Checked)
                    {
                        if (configSection.Count > 0)
                        {
                            this.StatusMessage = resourceMgr.GetString("ReadingRTC");
                            Application.DoEvents();

                            if (chkRTC.Checked)
                            {
                                RTCInformation rtcInformation = new RTCInformation();
                                rtcInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel1_OnStatusChanged);

                                DateTime rtc;
                                try
                                {
                                    //InitializeProgrammingValues();
                                    LocalCommunication communications;
                                    communications = ChannelManager.GetChannel() as LocalCommunication;
                                    rtcInformation.Channel = communications;
                                    //this.Cursor = Cursors.WaitCursor;
                                    this.StatusMessage = resourceMgr.GetString("Reading") + "RTC" + " ...";
                                    string statusMsg = "";
                                    if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                                    meterRTCData = rtcInformation.GetRTC(ref statusMsg);
                                    
                                    if (statusMsg == "Timeout!" || statusMsg == "Sign-On failure" || statusMsg == "Error in opening port." || statusMsg == "Access Denied.")
                                    {
                                        this.StatusMessage = statusMsg;
                                        readConfig.BreakCommunication();
                                        return;
                                    }
                                    if (meterRTCData != "")
                                    {
                                        ReadResult result = new ReadResult();
                                        result.CommandName = "RTC";
                                        result.Result = meterRTCData;
                                        readResult.Add(result);
                                        this.StatusMessage = "RTC" + resourceMgr.GetString("ReadSuccess");
                                        Application.DoEvents();
                                        if (!(chkMDWithIP.Checked || chkKVARSelcetion.Checked || chkDisplayParam.Checked || chkTOD.Checked || chkBilingReset.Checked || chkDailyLog.Checked))
                                        {
                                            this.StatusMessage = resourceMgr.GetString("Readoutcompleted");
                                            Application.DoEvents();
                                        }
                                    }
                                    else if (statusMsg == "Invalid RTC.")
                                    {
                                        this.StatusMessage = "Invalid RTC.";
                                        Application.DoEvents();
                                    }
                                    else
                                    {
                                        this.StatusMessage = rtcInformation.StatusMessage;
                                        readConfig.BreakCommunication();
                                        errmsg = errmsg + "\n" + this.StatusMessage;
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errmsg = errmsg + "\n" + "RTC : " + "Unable to parse Meter Output :" + meterRTCData;
                                }
                                finally
                                {
                                    //  this.Cursor = Cursors.Default;
                                    // FinalizeProgrammingValues();
                                }
                            }

                            //if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(true)))
                            //{
                            //    ReadManufactureSpecificCommands(configSection, readConfig, readResult);
                            //    this.StatusMessage = resourceMgr.GetString("RTCReadSuccess");
                            //    configSection = new Collection<MeterConfigurationConfigSection>();
                            //    Application.DoEvents();
                            //}
                            //else { 
                            //    this.StatusMessage = readConfig.StatusMessage;
                            //    errmsg = errmsg + "\n" + this.StatusMessage;
                            //    }
                            //readConfig.BreakCommunication();
                        }
                    }
                    #endregion

                    if (chkMDWithIP.Checked)
                    {
                        fileText += "(1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("MDWithIP")));
                    }
                    else { fileText += "(0"; }
                    if (chkKVARSelcetion.Checked)
                    {
                        fileText += "1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("kvahSelection")));
                    }
                    else { fileText += "0"; }

                    if (chkDisplayParam.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    if (chkTOD.Checked)
                    {
                        fileText += "1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("TOD")));
                    }
                    else { fileText += "0"; }

                    if (chkRTC.Checked)
                    {
                        fileText += "1";
                        // configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("RTC")));
                    }
                    else { fileText += "0"; }

                    // For Billing Reset
                    if (chkBilingReset.Checked)
                    {
                        fileText += "1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ModeOfBilling")));
                        if (UtilityDetails.UtilityName != UtilityEntity.TNEB1)
                        {
                            configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("BillingPeriod")));
                        }
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ResetLockOutDays")));

                    }
                    else { fileText += "0"; }


                    //For Resets.
                    if (chkReset.Checked)
                    {
                        if (!chkMDWithIP.Checked && !chkKVARSelcetion.Checked && !chkDisplayParam.Checked && !chkTOD.Checked && !chkRTC.Checked && !chkBilingReset.Checked && !chkDailyLog.Checked &&  !chkLockRS232.Checked)
                        {
                            MessageBox.Show("This option cannot be read", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    fileText += "0";

                    //-------------------Added Daily log, but this in not in sequence--------------------------------//
                    if (chkDailyLog.Checked)
                    {
                        fileText += "1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("DailyLog")));
                    }
                    else { fileText += "0"; }
                   
                    
                    //Added RSLock232
                    if (chkLockRS232.Checked)
                    {
                        fileText += "1";
                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("LockUnlockRS232")));
                    }
                    else
                    {
                        fileText += "0";
                    }
                    //while (fileText.Length <= 8) { fileText += "0"; }
                    fileText += ")\r\n";
                    string statusMessage = "";
                    if (chkMDWithIP.Checked || chkKVARSelcetion.Checked || chkDisplayParam.Checked || chkTOD.Checked || chkBilingReset.Checked || chkDailyLog.Checked ||  chkLockRS232.Checked)
                    {
                        
                        foreach (MeterConfigurationConfigSection section in configSection)
                        {
                            ReadResult result = new ReadResult();
                            if (section.Name != "RTC" && section.Name != "LockUnlockRS232")
                            {
                                if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                                if ((!String.IsNullOrEmpty(readConfig.HandshakeCommands(false))))
                                {
                                    result.CommandName = section.Name;
                                    Enum sectionName = GetConfigParameters(section.Name);
                                    this.StatusMessage = resourceMgr.GetString("Reading") + EnumUtil.StringValue(sectionName) + " ...";
                                    Application.DoEvents();
                                    result.Result = readConfig.ReadMeterConfigurations(section, ref statusMessage);
                                    readConfig.BreakCommunication();
                                    if (statusMessage == "Timeout!")
                                    {
                                        this.StatusMessage = statusMessage;
                                        return;
                                    }
                                    result.section = section;
                                    this.StatusMessage = EnumUtil.StringValue(sectionName) + resourceMgr.GetString("ReadSuccess");
                                    Application.DoEvents();
                                    readResult.Add(result);
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    readConfig.BreakCommunication();
                                    errmsg = errmsg + "\n" + this.StatusMessage;
                                    return;
                                }
                            }
                            if (section.Name == "LockUnlockRS232")
                            {
                                if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                                if ((!String.IsNullOrEmpty(readConfig.HandshakeCommands(true))))
                                {

                                    result.CommandName = section.Name;
                                    Enum sectionName = GetConfigParameters(section.Name);
                                    this.StatusMessage = resourceMgr.GetString("Reading") + EnumUtil.StringValue(sectionName) + " ...";
                                    Application.DoEvents();
                                    result.Result = readConfig.ReadMeterConfigurations(section, ref statusMessage);
                                    readConfig.BreakCommunication();
                                    if (statusMessage == "Timeout!")
                                        return;
                                    result.section = section;
                                    this.StatusMessage = EnumUtil.StringValue(sectionName) + resourceMgr.GetString("ReadSuccess");
                                    Application.DoEvents();
                                    readResult.Add(result);
                                }
                                else
                                {
                                    this.StatusMessage = readConfig.StatusMessage;
                                    readConfig.BreakCommunication();
                                    errmsg = errmsg + "\n" + this.StatusMessage;
                                    return;
                                }
                            }
                        }
                        //Read Display Paramaters.
                        if (chkDisplayParam.Checked)
                        {
                            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                            if (!String.IsNullOrEmpty(readConfig.HandshakeCommands(false)))
                            {
                                ReadDisplayParamaters(readConfig);
                                if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                                if (this.StatusMessage == "Timeout!")
                                    return;
                                readConfig.BreakCommunication();
                            }
                            else { this.StatusMessage = readConfig.StatusMessage; readConfig.BreakCommunication(); errmsg = errmsg + "\n" + this.StatusMessage; return; }
                        }

                        this.StatusMessage = resourceMgr.GetString("Readoutcompleted");
                        Application.DoEvents();
                        readConfig.BreakCommunication();

                    }
                    if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                    FormatData(readResult,true);
                    if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                    int index = 0;
                    int count = 0;
                    string cmdResult = string.Empty;
                    try
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            if (fileText.Substring(i, 1) == "0") { fileText += "(00)\r\n"; }
                            else
                            {
                                if (index == 0)
                                {
                                    cmdResult = readResult.Find(delegate(ReadResult p) { return p.CommandName.ToLower() == "mdwithip"; }).Result;
                                    //continue;
                                }
                                if (index == 1)
                                {
                                    cmdResult = readResult.Find(delegate(ReadResult p) { return p.CommandName.ToLower() == "kvahselection"; }).Result;
                                    //continue;
                                }
                                if (index == 2 && displayText != "")
                                { fileText += displayText + "\r\n"; index++; continue; }

                                //added for TOD Readout
                                if (index == 3)
                                {
                                    if (rdbFutureTOD.Checked)
                                        fileText += CreateTOUCommand(futureTODReadResult) + "\r\n";
                                    else
                                        fileText += CreateTOUCommand(currTODReadResult) + "\r\n"; 
                                    index++;
                                    continue; 
                                }
                                //added for TOD Readout

                                if (index == 4)//RTC 
                                {
                                    cmdResult = readResult.Find(delegate(ReadResult p) { return p.CommandName.ToLower() == "rtc"; }).Result;
                                    string tempData = cmdResult.Substring(cmdResult.IndexOf("|") + 2, cmdResult.Length - cmdResult.IndexOf("|") - 2);
                                    char BccVal = Convert.ToChar(tempData.Substring(tempData.Length - 1));
                                    if (BccVal == '\n')
                                        tempData = tempData.Substring(0, tempData.Length - 1);
                                    fileText += "(" + tempData + ")\r\n"; index++;
                                    continue;
                                }
                                // For Billing Reset
                                if (index == 5)
                                {
                                       cmdResult = readResult.Find(delegate(ReadResult p) { return p.CommandName.ToLower() == "billingperiod"; }).Result;
                                        fileText += (cmdResult.Substring(cmdResult.IndexOf("("), cmdResult.IndexOf(")") - cmdResult.IndexOf("("))) + "|";
                                 
                                    cmdResult = readResult.Find(delegate(ReadResult p)
                                    {
                                        return

                                            p.CommandName.ToLower() == "modeofbilling";
                                    }).Result;
                                    fileText += (cmdResult.Substring(cmdResult.IndexOf("(") + 1,

                                    cmdResult.IndexOf(")") - cmdResult.IndexOf("(") - 1)) + "|";
                                    cmdResult = readResult.Find(delegate(ReadResult p)
                                    {
                                        return

                                            p.CommandName.ToLower() == "resetlockoutdays";
                                    }).Result;
                                    fileText += (cmdResult.Substring(cmdResult.IndexOf("(") + 1,

                                    cmdResult.IndexOf(")") - cmdResult.IndexOf("("))) + "\r\n";
                                    index++;
                                    continue;
                                }


                                if (index == 7)//Daily Log
                                {
                                    cmdResult = readResult.Find(delegate(ReadResult p)
                                    {
                                        return

                                            p.CommandName.ToLower() == "dailylog";
                                    }).Result;

                                }
                                 if (index == 8)//RS232
                                {
                                    cmdResult = readResult.Find(delegate(ReadResult p)
                                    {
                                        return p.CommandName.ToLower() == "lockunlockrs232";
                                    }).Result;
                                }
                                fileText += (cmdResult.Substring(cmdResult.IndexOf("("),

                                cmdResult.IndexOf(")") - cmdResult.IndexOf("(") + 1)) + "\r\n";
                                if (readResult.Count > count + 1) count++;
                            }
                            index++;
                        }
                    }
                    catch (Exception ee)
                    {
                        errmsg = errmsg + "\n" + "Error in creation of CFG File";
                    }

                    try
                    {
                        if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return; }
                        SaveData(fileText);
                    }
                    catch (Exception ee)
                    {
                        errmsg = errmsg + "\n" + "Error in saving CFG file";
                    }
                }
            }
            catch (Exception et)
            {
                this.StatusMessage = resourceMgr.GetString("FailureConfigurationReadout");
                readConfig.BreakCommunication();
            }
            finally
            {
                //this.StatusMessage = "Readout and Uploading Successful";
                //this.StatusMessage = "";
                this.Cursor = Cursors.Default;
                pnConfigOptions.Enabled = true;
                isAborted = null;
            }
            if (errmsg.Trim().Length > 0)
            {
                txterrorLog.Text = errmsg.Insert(0, "FAILURES DURING EXECUTION\n---------------------------------------------------------\n");
            }
        }
        #endregion

        #region ReadManufactureSpecificCommands

       
        internal string ReadManufactureSpecificCommands(Collection<MeterConfigurationConfigSection> configSection, ReadConfigurations readConfig, List<ReadResult> readResult)
        {
            //StringBuilder manufactureSpecificReadOutput = new StringBuilder();
            foreach (MeterConfigurationConfigSection section in configSection)
            {
                ReadResult result = new ReadResult();
                result.CommandName = section.Name;
                string statusMsgq = "";
                result.Result = readConfig.ReadMeterConfigurations(section, ref statusMsgq);
                result.section = section;
                readResult.Add(result);

                if (section.Name.ToLower() == ConfigurationParameter.RTC.ToString().ToLower())
                {
                    return result.Result;// manufactureSpecificReadOutput.Append("<RTC>" + result.Result + "</RTC>");
                }
            }
            return null;
        }
        #endregion

        # region test

        /// <summary>
        /// This function gets the demend type selected by the user.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The 'demand type' string</returns>
        private string GetDemandType(string str)
        {
            string demand = "";
            try
            {
                if (str == "01")
                    demand = EnumUtil.StringValue(DemandType.BlockDemand);
                else if (str == "02")
                    demand = EnumUtil.StringValue(DemandType.SlidingDemand);
                return demand;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// This function splits the 'MD with IP' data string and fiils the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitMDWithIPData(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            MDWithIPEntity mdWithIPEntity = new MDWithIPEntity();
            string mdData = configData.Substring(1, configData.Length-2);

                foreach (MeterConfigurationConfigSectionParameter configSectionParameter in configSection.Parameters)
                {
                    if (configSectionParameter.Name == "MD1DemandType")
                    {
                        mdWithIPEntity.KWDemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD1TimeInterval")
                    {
                        mdWithIPEntity.KWInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD1SubInterval")
                    {
                        mdWithIPEntity.KWSubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2DemandType")
                    {
                        mdWithIPEntity.KVADemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2TimeInterval")
                    {
                        mdWithIPEntity.KVAInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2SubInterval")
                    {
                        mdWithIPEntity.KVASubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex)-1, Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
          
                //mdWithIPEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.mdWithIPEntity = mdWithIPEntity;
            }
        }

        /// <summary>
        /// This function splits the 'kVAr Selection' data string and fiils the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitkvarSelectionData(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            kvarSelectionEntity kvarselectionEntity = new kvarSelectionEntity();
            string kvarData = configData;

            if (kvarData.Contains("FF"))
            {
                kvarData = "(00)";
            }
            if (Convert.ToInt16(kvarData.Substring(1, 2)) == 0)
            { kvarselectionEntity.LagOnly = "1"; kvarselectionEntity.LagandLead = "0"; }
            else if (Convert.ToInt16(kvarData.Substring(1, 2)) == 1)
            { kvarselectionEntity.LagOnly = "0"; kvarselectionEntity.LagandLead = "1"; }

            //varselectionEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
            meterConfigEntity.kvarselectionEntity = kvarselectionEntity;
        }

        /// <summary>
        /// This function splits the 'Billing Mode' data string and fiils the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitModeOfBilling(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            BillingResetEntity billingresetentity = new BillingResetEntity();
            string ModeOfBilling = configData.Substring(1, 2);
            if (ModeOfBilling == "00")
            {
                billingresetentity.ModeOfBilling = BillingMode.EndofMonth;
                billingresetentity.Day = "01";
                billingresetentity.Hours = "00";
                billingresetentity.Minutes = "00";
                billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.billingresetentity = billingresetentity;


            }
            else if (ModeOfBilling == "01")
            {
                billingresetentity.ModeOfBilling = BillingMode.UserDefined;
                ModeOfBilling = configData.Substring(3, 2);
                billingresetentity.Day = ModeOfBilling;
                ModeOfBilling = configData.Substring(5, 2);
                billingresetentity.Hours = ModeOfBilling;
                ModeOfBilling = configData.Substring(7, 2);
                billingresetentity.Minutes = ModeOfBilling;
                //billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.billingresetentity = billingresetentity;
            }

        }

        /// <summary>
        /// This function splits the 'Billing Perid' data string and fiils the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitBillingPeriod(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            BillingResetEntity billingresetentity = new BillingResetEntity();
            string BillingPeriod = configData;
            string s = BillingPeriod.Substring(1, 2);

            if (s == "00")
                billingresetentity.BillingPeriod = BillingPeriod1.EvenMonth;
            if (s == "01")
                billingresetentity.BillingPeriod = BillingPeriod1.OddMonth;
            if (s == "02")
                billingresetentity.BillingPeriod = BillingPeriod1.Monthly;
            meterConfigEntity.billingresetentity = billingresetentity;


        }

        /// <summary>
        /// This function splits the 'Billing Resetlockout Days' data string and fiils the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitResetLockOutDays(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            BillingResetEntity billingresetentity = new BillingResetEntity();
            string ResetLockOutDaysData = configData;
            string s = ResetLockOutDaysData.Substring(1, 2);
            string dec = Convert.ToString(Convert.ToInt32(s, 16), 10);
            billingresetentity.ResetLockOutDays = dec;
            //billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
            meterConfigEntity.billingresetentity = billingresetentity;

            
        }

        /// <summary>
        /// This function splits the 'Daily Log' data string and fills the corresponding data entity.
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="configSection"></param>
        /// <param name="meterConfigEntity"></param>
        public void SplitDailyLogData(string configData, MeterConfigurationConfigSection configSection, ref MeterConfigurationsNFEntity meterConfigEntity)
        {
            DailyLogEntity dailylogentity = new DailyLogEntity();
            string DailyLogData = configData;
            string s = DailyLogData.Substring(1, 2);
            string binary = Convert.ToString(Convert.ToInt32(s, 16), 2).PadLeft(8, '0');
            char[] c = new char[8];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = Convert.ToChar(binary.Substring(i, 1));
            }
            dailylogentity.CumulativeKWh = Convert.ToString(c[7]);
            dailylogentity.CumulativeKVARhLag = Convert.ToString(c[6]);
            dailylogentity.CumulativeKVARhLead = Convert.ToString(c[5]);
            dailylogentity.CumulativeKVAh = Convert.ToString(c[4]);
            dailylogentity.DailyMD1 = Convert.ToString(c[3]);
            dailylogentity.DailyMD2 = Convert.ToString(c[2]);
            //dailylogentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
            meterConfigEntity.dailylogentity = dailylogentity;
        }
        # endregion

        /// <summary>
        /// This function splits the 'RS232 Lock/Unlock' data string and fills the corresponding data entity.
        /// </summary>
        /// <param name="configdata"></param>
        /// <param name="configsection"></param>
        /// <param name="meterconfigentity"></param>
        public void SplitRS232Lock(string configdata, MeterConfigurationConfigSection configsection, ref MeterConfigurationsNFEntity meterconfigentity)
        {
            RS232LockEntity RS232Lockentity = new RS232LockEntity();
            string RS232Lock = configdata;

            string s = RS232Lock.Substring(1, 2);
            if (s == "01")
            {
                RS232Lockentity.LockStatus = "NotLocked";
                //RS232Lockentity.MeterID = configdata.Substring(5, configdata.IndexOf("\r") - 5);
            }
            if (s == "00")
            {
                RS232Lockentity.LockStatus = "Locked";
                //RS232Lockentity.MeterID = configdata.Substring(5, configdata.IndexOf("\r") - 5);
            }
            meterconfigentity.RS232Entity = RS232Lockentity;


            
        }
        #region FormatData
        /// <summary>
        /// this function accepts the data list containing data for each of the selected configuration parameters,
        /// parses the data strings one by one and displays the corresponding data in the UI.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="meterReadout"></param>
        private void FormatData(List<ReadResult> result, bool meterReadout)
        {
            MeterConfigurationsNFEntity meterConfig = new MeterConfigurationsNFEntity();
            FormatterConfigurations formatterConfigurations = new FormatterConfigurations();

            foreach (ReadResult res in result)
            {
                switch (res.CommandName)
                {
                    case "BillingReset":
                        try
                        {
                            if (!meterReadout)
                            {
                                DisplayBillingReset(res.Result);
                                chkReset.Checked = true;
                            }
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "Billing Reset : " + "Unable to parse the value entered from the UI";
                        }
                        //
                        break;
                        
                    case "DisplayParameters" :
                        if (!meterReadout)
                        {
                            string displayParamsCount = res.Result.Substring(0, 7);
                            int[] displayParmaterCountByType = formatterConfigurations.SplitDisplayParamaterCount(displayParamsCount);
                            Collection<DisplayParamatersDBEntity> temp = new Collection<DisplayParamatersDBEntity>();

                            int index = 7;
                            string scrollParams = "(" + res.Result.Substring(index, displayParmaterCountByType[0] * 2);
                            index += displayParmaterCountByType[0] * 2;
                            Collection<string> parametersToSelect = formatterConfigurations.ParseScrollModeParameters(scrollParams, displayParmaterCountByType[0]);

                            SelectRowsInDataGridByDisplayParamaterReadOutput(parametersToSelect, DisplayParameter.ScrollMode, temp);

                            string pushParams = "(" + res.Result.Substring(index, displayParmaterCountByType[1] * 2);
                            index += displayParmaterCountByType[1] * 2;
                            parametersToSelect = formatterConfigurations.ParsePushModeParameters(pushParams, displayParmaterCountByType[1]);
                            SelectRowsInDataGridByDisplayParamaterReadOutput(parametersToSelect, DisplayParameter.PushMode, temp);

                            string highResParams = "(" + res.Result.Substring(index, displayParmaterCountByType[2] * 2);
                            parametersToSelect = formatterConfigurations.ParseHighResolutionModeParameters(highResParams, displayParmaterCountByType[2]);
                            index += displayParmaterCountByType[2] * 2;
                            SelectRowsInDataGridByDisplayParamaterReadOutput(parametersToSelect, DisplayParameter.HighResolutionMode, temp);

                            string displayTimeOutText = "(" + res.Result.Substring(index);//, tmpResult.IndexOf(")") + 1);

                            int tmp = Convert.ToInt32(displayTimeOutText.Substring(1, 4), 16);
                            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text = tmp.ToString();


                            tmp = Convert.ToInt32(displayTimeOutText.Substring(5, 4), 16);
                            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text = tmp.ToString();

                            tmp = Convert.ToInt32(displayTimeOutText.Substring(9, 2), 16);
                            if (tmp != 0)
                            {
                                ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = true;
                                tmp = Convert.ToInt32(displayTimeOutText.Substring(11, 4), 16);
                                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = tmp.ToString();
                            }
                            else
                            {
                                ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = false;
                                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = "";
                                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
                            }
                            chkDisplayParam.Checked = true;
                        }
                        break;

                    case "MDWithIP":
                        MDWithIPEntity mdWithIPEntity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitMDWithIPData(res.Result, res.section, ref meterConfig);
                            else
                                SplitMDWithIPData(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "MD With IP : " + "Unable to parse Meter Output";
                        }
                        DisplayMDWithIP(meterConfig);
                        chkMDWithIP.Checked = true;
                        //this.StatusMessage = "Uploading MD With IP...";
                        //Application.DoEvents();

                        //insertion removed
                        //if (meterConfig.mdWithIPEntity != null)
                        //{
                        //    mdWithIPEntity = meterConfig.mdWithIPEntity as MDWithIPEntity;
                        //    MDWithIPBLL mdWithIPBLL = new MDWithIPBLL();
                        //    mdWithIPBLL.InsertData(mdWithIPEntity, -1, -1);
                        //}
                        //insertion removed

                        //fileText +=( res.Result.Substring(res.Result.IndexOf("(") + 1, res.Result.IndexOf(")") - res.Result.IndexOf("(") - 1)) + "\r\n";
                        break;
                    case "kvahSelection":
                        kvarSelectionEntity kvarselectionEntity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitkvarSelectionData(res.Result, res.section, ref meterConfig);
                            else
                                SplitkvarSelectionData(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "kVAH Selection : " + "Unable to parse Meter Output";
                        }
                        DisplaykvarSelection(meterConfig);
                        chkKVARSelcetion.Checked = true;
                        //this.StatusMessage = "Uploading kvar selection...";
                        //Application.DoEvents();
                        //insertion removed
                        //if (meterConfig.kvarselectionEntity != null)
                        //{
                        //    kvarselectionEntity = meterConfig.kvarselectionEntity as kvarSelectionEntity;
                        //    kvarSelectionBLL kvarselectionBLL = new kvarSelectionBLL();
                        //    kvarselectionBLL.InsertData(kvarselectionEntity, -1, -1);
                        //}
                        //insertion removed
                        //fileText += res.Result.Substring(res.Result.IndexOf("(") + 1, res.Result.IndexOf(")") - res.Result.IndexOf("(") - 1) + "\r\n";
                        break;
                    case "TOD":
                        string touType = "";
                        string todReadResult = "";
                        currTODReadResult = "";
                        futureTODReadResult = "";
                        try
                        {
                            if (meterReadout)
                            {
                                currTODReadResult = res.Result.Substring(res.Result.IndexOf("/TU//"), res.Result.IndexOf("/FU//") - res.Result.IndexOf("/TU//"));
                                futureTODReadResult = res.Result.Substring(res.Result.IndexOf("/FU//"));
                                if (rdbFutureTOD.Checked)
                                {
                                    touType = EnumUtil.StringValue(TOUType.Future);
                                    todReadResult = futureTODReadResult;
                                }
                                if (rdbCurrentTOD.Checked)
                                {
                                    touType = EnumUtil.StringValue(TOUType.Current);
                                    todReadResult = currTODReadResult;
                                }
                                
                            }

                            else
                            {
                                todReadResult = res.Result;
                                const string regexTOD = @"(\(([\w\W]*?)\))";
                                MatchCollection matches = Regex.Matches(todReadResult, regexTOD, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                                string[] tempTODData = new string[matches.Count];
                                int count = 0;
                                foreach (Match match in matches)
                                {
                                    GroupCollection groups = match.Groups;
                                    tempTODData[count++] = groups["0"].Value;
                                }
                                if (tempTODData[tempTODData.Length - 1].Length > 26)
                                    touType = EnumUtil.StringValue(TOUType.Future);
                                else
                                    touType = EnumUtil.StringValue(TOUType.Current);
                            }
                            tabRS232LockUnlock.SelectedIndex = 4; 
                            DisplayTOU(todReadResult, touType);
                            tabRS232LockUnlock.SelectedIndex = 0; 


                            chkTOD.Checked = true;
                            
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "TOU : " + "Unable to parse Meter Output";
                        }
                        break;
                    case "RTC":
                        try
                        {
                            string tempData = "";
                            if (meterReadout)
                                tempData = res.Result.Substring(res.Result.IndexOf("|") + 2, res.Result.Length - res.Result.IndexOf("|") - 2);
                            else
                                tempData = res.Result.Substring(1, res.Result.Length - 2);
                            string rtcvalue = new FormatterConfigurations().ParseRTC(tempData);
                            if (rtcvalue == null)
                                throw new Exception();
                            DisplayRTC(rtcvalue);// new RTCBLL().InsertData(meterRTCData, res.Result.Substring(5, res.Result.IndexOf("\r") - 5), -1);
                            chkRTC.Checked = true;
                            //insertion removed
                            //new RTCBLL().InsertData(meterRTCData, meterRTCData.Substring(5, meterRTCData.IndexOf("\r") - 5), -1, -1);
                            //insertion removed
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "RTC : " + "Unable to parse Meter Output :" + res.Result.Replace("\n", "").Replace("\r", "");
                        }
                        break;
                    case "DailyLog":
                        DailyLogEntity dailylogentity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitDailyLogData(res.Result, res.section, ref meterConfig);
                            else
                                SplitDailyLogData(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "Daily Log : " + "Unable to parse Meter Output";
                        }

                        DisplayDailyLog(meterConfig);
                        chkDailyLog.Checked = true;
                        //insertion removed
                        //if (meterConfig.dailylogentity != null)
                        //{
                        //    dailylogentity = meterConfig.dailylogentity as DailyLogEntity;
                        //    DailyLogBLL dailylogBLL = new DailyLogBLL();
                        //    dailylogBLL.Insertdata(dailylogentity, -1, -1);
                        //}
                        //insertion removed
                        break;
                    // For Billing Reset Tab
                    case "ModeOfBilling":
                        BillingResetEntity billingresetentity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitModeOfBilling(res.Result, res.section, ref meterConfig);
                            else
                                SplitModeOfBilling(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "Mode of Billing : " + "Unable to parse Meter Output";
                        }

                        DisplayModeOfbilling(meterConfig);
                        chkBilingReset.Checked = true;
                        break;
                    case "BillingPeriod":
                        billingresetentity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitBillingPeriod(res.Result, res.section, ref meterConfig);
                            else
                                SplitBillingPeriod(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "Billing Period : " + "Unable to parse Meter Output";
                        }

                        DisplayBillingPeriod(meterConfig);
                        break;
                    case "ResetLockOutDays":
                        billingresetentity = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitResetLockOutDays(res.Result, res.section, ref meterConfig);
                            else
                                SplitResetLockOutDays(res.Result, res.section, ref meterConfig);
                        }
                        catch (Exception ee)
                        {
                            errmsg = errmsg + "\n" + "Reset Lock Out Days : " + "Unable to parse Meter Output";
                        }

                        DisplayResetLockOutDays(meterConfig);
                        //insertion removed
                        //if (meterConfig.billingresetentity != null)
                        //{
                        //    billingresetentity = meterConfig.billingresetentity as BillingResetEntity;
                        //    BillingResetBLL billingresetBLL = new BillingResetBLL();
                        //    billingresetBLL.Insertdata(billingresetentity, -1, -1);
                        //}
                        //insertion removed
                        break;
                    case "LockUnlockRS232":
                        RS232LockEntity RS232Lock = null;
                        try
                        {
                            if (meterReadout)
                                formatterConfigurations.SplitRS232Lock(res.Result, res.section, ref meterConfig);
                            else
                                SplitRS232Lock(res.Result, res.section, ref meterConfig);
                        }
                        catch
                        {
                            errmsg = errmsg + "\n" + "RS232 Lock : " + "Unable to parse Meter Output";
                        }
                        DisplayRS232(meterConfig);
                        chkLockRS232.Checked = true;
                        //insertion removed
                        //if (meterConfig.RS232Entity != null)
                        //{
                        //    RS232Lock = meterConfig.RS232Entity as RS232LockEntity;
                        //    RS232BLL RS232bll = new RS232BLL();
                        //    RS232bll.Insertdata(RS232Lock, -1, -1);
                        //}
                        //insertion removed
                        break;
                }
            }
        }
        #endregion

        #region Display RTC
        /// <summary>
        /// This function displays the RTC data on the UI.
        /// </summary>
        /// <param name="rtc"></param>
        private void DisplayRTC(string rtc)
        {
            rtcCtrl.Controls[0].Controls["txtRTC"].Text = rtc;
            DataGridView dataGridView = (DataGridView)rtcCtrl.Controls[0].Controls["dGridRTC"];
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            DataGridViewCell srlNoCell = new DataGridViewTextBoxCell();
            srlNoCell.Value = (dataGridView.Rows.Count + 1).ToString();
            dataGridViewRow.Cells.Add(srlNoCell);
            DataGridViewCell rtcDatetimeCell = new DataGridViewTextBoxCell();
            rtcDatetimeCell.Value = rtc;
            dataGridViewRow.Cells.Add(rtcDatetimeCell);
            dataGridView.Rows.Add(dataGridViewRow);
        }
        #endregion

        #region DisplayDailyLog
        //To display the daily log parameters
        public void DisplayDailyLog(MeterConfigurationsNFEntity meterconfig)
        {
            group = this.dailyLog1.Controls["gbDailyLog"];
            CheckBox chk1 = (CheckBox)group.Controls["chkSelectAll"];
            CheckBox chk = (CheckBox)group.Controls["chkCumulativeKwh"];
            if (meterconfig.dailylogentity.CumulativeKWh == "1")
                chk.Checked = true;
            CheckBox chk2 = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
            if (meterconfig.dailylogentity.CumulativeKVARhLag == "1")
                chk2.Checked = true;
            CheckBox chk3 = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
            if (meterconfig.dailylogentity.CumulativeKVARhLead == "1")
                chk3.Checked = true;
            CheckBox chk4 = (CheckBox)group.Controls["chkCumulativeKVAh"];
            if (meterconfig.dailylogentity.CumulativeKVAh == "1")
                chk4.Checked = true;
            CheckBox chk5 = (CheckBox)group.Controls["chkDailyMD1"];
            if (meterconfig.dailylogentity.DailyMD1 == "1")
                chk5.Checked = true;
            CheckBox chk6 = (CheckBox)group.Controls["chkDailyMD2"];
            if (meterconfig.dailylogentity.DailyMD2 == "1")
                chk6.Checked = true;
            if (chk.Checked == true && chk2.Checked == true && chk3.Checked == true && chk4.Checked == true && chk5.Checked == true && chk6.Checked == true)
            {
                chk1.Checked = true;
            }


        }
        #endregion

        #region DisplaykvarSelection
        /// <summary>
        /// This function displays the 'kVAr Selection' data on the UI
        /// </summary>
        /// <param name="meterConfig"></param>
        private void DisplaykvarSelection(MeterConfigurationsNFEntity meterConfig)
        {
            group = this.kvarSelection1.Controls["grpkvarSelection"];
            RadioButton rdb = (RadioButton)group.Controls["rdbLagOnly"];
            if (meterConfig.kvarselectionEntity.LagOnly == EnumUtil.StringValue(kVArSelectionParameters.SelectedParameterValue))
                rdb.Checked = true;
            rdb = (RadioButton)group.Controls["rdbLagnLead"];
            if (meterConfig.kvarselectionEntity.LagandLead == EnumUtil.StringValue(kVArSelectionParameters.SelectedParameterValue))
                rdb.Checked = true;
        }
        #endregion

        #region BillingReset
        // To display reset lock out days
        private void DisplayResetLockOutDays(MeterConfigurationsNFEntity meterconfig)
        {
            group = this.billingReset1.Controls["gbManual"];
            ComboBox cmbResetLockoutDays = (ComboBox)group.Controls["cmbResetLockoutdays"];
            cmbResetLockoutDays.SelectedIndex = getSelectedIndex(cmbResetLockoutDays, Convert.ToString(meterconfig.billingresetentity.ResetLockOutDays));

        }

        // To dispay RS232Lock
        private void DisplayRS232(MeterConfigurationsNFEntity meterconfig)
        {
            if (meterconfig.RS232Entity.LockStatus == "Locked")
            {
                chkLockRS232Port.Checked = true;
            }
            if (meterconfig.RS232Entity.LockStatus == "NotLocked")
            {
                chkLockRS232Port.Checked = false;
            }
        }
        // To display Mode of billing
        private void DisplayModeOfbilling(MeterConfigurationsNFEntity meterconfig)
        {
            group = this.billingReset1.Controls["gbAutoMode"];
            ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
            cmbModeofBilling.SelectedItem = GetSelectedModeOfBilling(meterconfig.billingresetentity.ModeOfBilling);
            ComboBox cmbSelectDay = (ComboBox)group.Controls["cmbSelectDay"];
            cmbSelectDay.SelectedIndex = getSelectedIndexBillingReset(cmbSelectDay, Convert.ToString(meterconfig.billingresetentity.Day));
            ComboBox cmbSelectHour = (ComboBox)group.Controls["cmbSelectHour"];
            cmbSelectHour.SelectedIndex = getSelectedIndexBillingReset(cmbSelectHour, Convert.ToString(meterconfig.billingresetentity.Hours));
            ComboBox cmbSelectMinutes = (ComboBox)group.Controls["cmbSelectMinutes"];
            cmbSelectMinutes.SelectedIndex = getSelectedIndexBillingReset(cmbSelectMinutes, Convert.ToString(meterconfig.billingresetentity.Minutes));
            if (UtilityDetails.DisableEndOfMonthOptionBillingMode)
            {
                cmbModeofBilling.Items.Remove(billingModeEOM);
            }
            //RadioButton rdbMonthly = (RadioButton)group.Controls["rbtnMonthly"];
            //RadioButton rdbOddMonth = (RadioButton)group.Controls["rbtnOddMonth"];
            //RadioButton rdbEvenMonth = (RadioButton)group.Controls["rbtnEvenMonth"];
        }
        // For Billing Period

        private void DisplayBillingPeriod(MeterConfigurationsNFEntity meterconfig)
        {
            group = this.billingReset1.Controls["gbAutoMode"];
            RadioButton rdbMonthly = (RadioButton)group.Controls["rbtnMonthly"];
            RadioButton rdbOddMonth = (RadioButton)group.Controls["rbtnOddMonth"];
            RadioButton rdbEvenMonth = (RadioButton)group.Controls["rbtnEvenMonth"];
            if ((int)(meterconfig.billingresetentity.BillingPeriod) == 0)
                rdbMonthly.Checked = true;
            if ((int)(meterconfig.billingresetentity.BillingPeriod) == 1)
                rdbOddMonth.Checked = true;
            if ((int)(meterconfig.billingresetentity.BillingPeriod) == 2)
                rdbEvenMonth.Checked = true;

        }


        #endregion

        # region DisplayBillingReset
        /// <summary>
        /// This function displays the 'Billing Reset status on the UI
        /// </summary>
        /// <param name="ResetDataString"></param>
        private void DisplayBillingReset(string ResetDataString)
        {
            group = this.reset1;
            CheckBox chkReset = (CheckBox)group.Controls["chkBillingReset"];
            if (ResetDataString.Substring(1, 2) == "01")
            {
                chkReset.Checked = true;
            }
            else
                chkReset.Checked = false;
        }
        # endregion

        #region DisplayMDWithIP
        /// <summary>
        /// This function displays the 'MD with IP' data on the UI
        /// </summary>
        /// <param name="meterConfig"></param>
        private void DisplayMDWithIP(MeterConfigurationsNFEntity meterConfig)
        {


            cmbDemandType.SelectedIndex = getSelectedIndex(cmbDemandType, meterConfig.mdWithIPEntity.KWDemandType);
            cmbDemandInterval.SelectedIndex = getSelectedIndex(cmbDemandInterval, Convert.ToString(meterConfig.mdWithIPEntity.KWInterval));

            if (getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval)) != -1)
            {
                cmbDemandSubInterlavTime.SelectedIndex = getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval));
            }
            else
            {               
                cmbDemandSubInterlavTime.SelectedIndex = -1;
            }

            //cmbkVADemandType.SelectedIndex = getSelectedIndex(cmbkVADemandType, meterConfig.mdWithIPEntity.KVADemandType);
            //cmbkVADemandInt.SelectedIndex = getSelectedIndex(cmbkVADemandInt, Convert.ToString(meterConfig.mdWithIPEntity.KVAInterval));

            //if (getSelectedIndex(cmbkVADemandSubInt, Convert.ToString(meterConfig.mdWithIPEntity.KVASubInterval)) != -1)
            //{
            //    cmbkVADemandSubInt.SelectedIndex = getSelectedIndex(cmbkVADemandSubInt, Convert.ToString(meterConfig.mdWithIPEntity.KVASubInterval));
            //}
            //else
            //{
            //    string demand = ConfigSettings.GetValue("MDWithIPDefault");
            //    string[] demandDefault = demand.Split(',');
            //    int demandIndex = 0;
            //    cmbkVADemandSubInt.Items.Clear();
            //    cmbkVADemandSubInt.Items.Add(demandDefault[demandIndex++]);
            //    cmbkVADemandSubInt.Items.Add(demandDefault[demandIndex++]);
            //    cmbkVADemandSubInt.Items.Add(demandDefault[demandIndex++]);
            //}
        }
        #endregion

        internal int getSelectedIndex(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// This function decides the mode of Billing to be displayed on the UI in the 'MD with IP' window.
        /// </summary>
        /// <param name="ModeofBilling"></param>
        /// <returns></returns>
        private string GetSelectedModeOfBilling(BillingMode modeofBilling)
        {
            if (modeofBilling == BillingMode.EndofMonth)
                return billingModeEOM;
            if (modeofBilling == BillingMode.UserDefined)
                return billingModeUD;

            return "";
        }

        /// <summary>
        /// This function decides the Days, Hours and Minutes to be displayed on the UI in the 'MD with IP' window.
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private int getSelectedIndexBillingReset(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString().Length == 1)
                    {
                        string s = cmb.Items[i].ToString().PadLeft(2, '0');

                        if (s == strValue)
                        {
                            return i;
                        }
                    }
                    else if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }

                }
            }

            return -1;
        }

        #region SaveData
        /// <summary>
        /// This function writes the data string into a text file.
        /// </summary>
        /// <param name="fileText"></param>
        public void SaveData(string fileText)
        {
            string fileLocation = string.Empty;

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
                    return;
                }
                else
                {
                    fileLocation = saveFileDialog.FileName.Trim();
                }
                FileStream file = new FileStream(fileLocation, FileMode.Create);
                StreamWriter wr1 = new StreamWriter(file);
                wr1.Write(fileText);
                wr1.Close();
                file.Close();
                this.StatusMessage = MessageConstant.GetText("File created successfully");
            }
            else
            {
                return;
            }
        }
        #endregion

        #region ValidateDisplayParameterInputs
        /// <summary>
        /// Function Added by Vivek on 12 August 2011.
        /// Purpose : Validate inputs in Display Paramaters tab
        /// </summary>
        /// <returns></returns>
        private string ValidateDisplayParameterInputs()
        {//Validation to check if atleast a single paramater is selected in each display paramater type.
            if (!(IfAnyRowSelectedInDataGrid((DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"])))
                return (resourceMgr.GetString("SelectAtleastOneParamater") + EnumUtil.StringValue(DisplayParameter.PushMode));

            if (!(IfAnyRowSelectedInDataGrid((DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"])))
                return (resourceMgr.GetString("SelectAtleastOneParamater") + EnumUtil.StringValue(DisplayParameter.ScrollMode) );

            if (!(IfAnyRowSelectedInDataGrid((DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"])))
                return (resourceMgr.GetString("SelectAtleastOneParamater") + EnumUtil.StringValue(DisplayParameter.HighResolutionMode));

            //Validation Check : Scroll Time.
            string tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text.Trim();
            //Check for integer values only.
            foreach (char c in tmp)
                if (!char.IsDigit(c))
                { return resourceMgr.GetString("ValidationFailure_IntegersOnly") + EnumUtil.StringValue(DisplayParameter.ScrollMode) + resourceMgr.GetString("timeout"); }
            //Check for Range and blank value.
            if (tmp.Length == 0)
            { return resourceMgr.GetString("ValidationFailure_Blank") + EnumUtil.StringValue(DisplayParameter.ScrollMode) + resourceMgr.GetString("timeout"); }
            if (Convert.ToInt32(tmp) < 1 || Convert.ToInt32(tmp) > 300)
            { return resourceMgr.GetString("ValidationFailure_Range") + EnumUtil.StringValue(DisplayParameter.ScrollMode) + resourceMgr.GetString("timeout"); }

            //Validation Check : Push Time.
            tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text.Trim();
            //Check for integer values only.
            foreach (char c in tmp)
                if (!char.IsDigit(c))
                { return resourceMgr.GetString("ValidationFailure_IntegersOnly") + EnumUtil.StringValue(DisplayParameter.PushMode); }
            //Check for Range and blank value.
            if (tmp.Length == 0)
            { return resourceMgr.GetString("ValidationFailure_Blank") + EnumUtil.StringValue(DisplayParameter.PushMode) + resourceMgr.GetString("timeout"); }

            if (Convert.ToInt32(tmp) < 1 || Convert.ToInt32(tmp) > 600)
            { return resourceMgr.GetString("ValidationFailure_Range") + EnumUtil.StringValue(DisplayParameter.PushMode) + resourceMgr.GetString("timeout"); }

            //Validation Check : Auto Scroll Resume Time.
            if (((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked)
            {
                tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text.Trim();
                //Check for integer values only.
                foreach (char c in tmp)
                    if (!char.IsDigit(c))
                    { return resourceMgr.GetString("ValidationFailure_IntegersOnly") + " auto scroll timeout"; }
                //Check for Range and blank value.
                if (tmp.Length == 0)
                { return resourceMgr.GetString("ValidationFailure_Blank") + " auto scroll timeout"; }

                if (Convert.ToInt32(tmp) < 3 || Convert.ToInt32(tmp) > 300)
                { return resourceMgr.GetString("ValidationFailure_Range") + " auto scroll timeout"; }
            }
            return string.Empty;
        }
        #endregion

        #region Function to check If Any Row Selected InDataGrid
        /// <summary>
        /// Code Region Added by Vivek on 9 August 2011 for Display Parameters Write in meter(TNEB Project).
        /// Purpose : If any row is selected(checked) in a gridview then 
        /// this function returns True else false is returned
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        private bool IfAnyRowSelectedInDataGrid(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {//Get check box cell.
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                //Check if cell is checked.
                if (cell != null && cell.Value != null && (bool)cell.Value == true)
                    return true;
            }
            return false;
        }
        #endregion

        #region Function : Get Selected Paramaters names by displayParamter type.
        /// <summary>
        /// Code Region Added by Vivek on 10 August 2011 (TNEB Project).
        /// Purpose: This function returns the collection of selected parameters in a display parameter type.
        /// </summary>
        /// <param name="displayParameter"></param>
        /// <returns></returns>
        private Collection<String> GetSelectedParameters(DisplayParameter displayParameter, Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity)
        {
            DataGridView dataGridView = new DataGridView();
            Collection<String> selectedParameters = new Collection<string>();
            DisplayParamatersDBEntity displayParamatersDBEntity = new DisplayParamatersDBEntity();
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            else
            {//Add Display TimeOut Parameters to the collection.
                //selectedParameters.Add(DisplayTimeOutsParameters.PushButtonTimeOut.ToString());
                //selectedParameters.Add(DisplayTimeOutsParameters.ScrollTimePerItem.ToString());
                //if (((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked)
                //    selectedParameters.Add(DisplayTimeOutsParameters.AutoScrollResumeTime.ToString());

                string strWriteCommand = string.Empty;
                string tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text.Trim();
                //Fill DTO to write in Db.
                displayParamatersDBEntity = new DisplayParamatersDBEntity();
                displayParamatersDBEntity.displayParamaterType = displayParameter;
                displayParamatersDBEntity.paramaterName = DisplayTimeOutsParameters.ScrollTimePerItem.ToString();//"Scroll Time Out";
                displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                tmp = Convert.ToInt32(tmp).ToString("X2");//Get Hex Value of Decimal Value(input).
                while (tmp.Length < 4)
                { tmp = tmp.Insert(0, "0"); };
                //Get Ascii value of each Hex Character and then Get Hex Value of each Ascii character.
                strWriteCommand += ProgrammingCommon.GetASCIIValue(tmp);


                tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text.Trim();
                //Fill DTO to write in Db.
                displayParamatersDBEntity = new DisplayParamatersDBEntity();
                displayParamatersDBEntity.displayParamaterType = displayParameter;
                displayParamatersDBEntity.paramaterName = DisplayTimeOutsParameters.PushButtonTimeOut.ToString();// "Push Time Out";
                displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                tmp = Convert.ToInt32(tmp).ToString("X2");//Get Hex Value of Decimal Value(input).
                while (tmp.Length < 4)
                { tmp = tmp.Insert(0, "0"); };
                //Get Ascii value of each Hex Character and then Get Hex Value of each Ascii character.
                strWriteCommand += ProgrammingCommon.GetASCIIValue(tmp);


                if (((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked == true)
                {
                    tmp = displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text.Trim();
                    //Fill DTO to write in Db.
                    displayParamatersDBEntity = new DisplayParamatersDBEntity();
                    displayParamatersDBEntity.displayParamaterType = displayParameter;
                    displayParamatersDBEntity.paramaterName = DisplayTimeOutsParameters.AutoScrollResumeTime.ToString();// "Auto Scroll Time Out";
                    displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                    collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                    tmp = Convert.ToInt32(tmp).ToString("X2");//Get Hex Value of Decimal Value(input).
                    while (tmp.Length < 4)
                    { tmp = tmp.Insert(0, "0"); };
                    //AutoScroll Checkbox is true.
                    tmp = tmp.Insert(0, "01");
                    //Get Ascii value of each Hex Character and then Get Hex Value of each Ascii character.
                    strWriteCommand += ProgrammingCommon.GetASCIIValue(tmp);
                }
                else
                    strWriteCommand += "303030303030";
                selectedParameters.Add(strWriteCommand);
            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {   //If displayparameter type is any of the Pushmode,ScrollMode or High resolution  then get the collection of
                //selected(checkbox is checked) parameters from its datagridview.
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null && cell.Value != null && (bool)cell.Value == true)
                {
                    selectedParameters.Add((dataGridView[2, i].Value.ToString()));
                    //Fill DTO to write in Db.
                    displayParamatersDBEntity = new DisplayParamatersDBEntity();
                    displayParamatersDBEntity.displayParamaterType = displayParameter;
                    displayParamatersDBEntity.paramaterName = dataGridView[2, i].Value.ToString();
                    displayParamatersDBEntity.paramaterValue = 1;
                    collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
                }
            }
            //return the selected parameters.
            return selectedParameters;
        }

        #endregion

        #region ReadDisplayParamaters
        /// <summary>
        /// this function reads the display parameters configured in the meter.
        /// </summary>
        /// <param name="readConfig"></param>
        private void ReadDisplayParamaters(ReadConfigurations readConfig)
        {
            //Read Display Paramaters.
            
                //   fileText += "1";
                this.StatusMessage = resourceMgr.GetString("ReadingDisplayParamatersMeterConfigurations");
                Application.DoEvents();
                string meterID = string.Empty;
                displayParametersToSelect = new Collection<Collection<string>>();
                try
                {
                    if (GetDisplayParamatersConfiguration(readConfig, ref meterID) != "Aborted")
                    {
                        if (this.StatusMessage == "Timeout!")
                            return;
                    }
                    

                }
                catch (Exception)
                {
                    if (this.StatusMessage == "Timeout!")
                        return;
                    errmsg = errmsg + "\n" + "Display Parameters : " + "Error in parsing the meter output";
                }
                collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
                try
                {
                    SelectRowsInDataGridByDisplayParamaterReadOutput(displayParametersToSelect[0], DisplayParameter.ScrollMode, collDisplayParamatersDBEntity);
                    SelectRowsInDataGridByDisplayParamaterReadOutput(displayParametersToSelect[1], DisplayParameter.PushMode, collDisplayParamatersDBEntity);
                    SelectRowsInDataGridByDisplayParamaterReadOutput(displayParametersToSelect[2], DisplayParameter.HighResolutionMode, collDisplayParamatersDBEntity);
                    SetDisplayTimeOutsByDisplayParamaterReadOutput(displayParametersToSelect[3][0], collDisplayParamatersDBEntity);
                }
                catch (Exception rr)
                {
                    errmsg = errmsg + "\n" + "Display Parameters : " + "Parameters List mismatch";
                }
                DisplayParametersBLL displayParametersBLL = new DisplayParametersBLL();
                if (!displayParametersBLL.InsertData(collDisplayParamatersDBEntity, meterID, -1, -1))
                {
                    this.StatusMessage = resourceMgr.GetString("DisplayParamaters_DBInsertionFailure");
                    errmsg = errmsg + "\n" + this.StatusMessage;
                }
                this.StatusMessage = resourceMgr.GetString("DisplayParamatersReadSuccessfully");
                Application.DoEvents();

            //   else { fileText += "0"; }

        }
        #endregion

        #region GetDisplayParamatersConfiguration
        /// <summary>
        /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
        /// Purpose: This function returns the collection of selected parameters in a display parameter type.
        /// </summary>
        internal string GetDisplayParamatersConfiguration(ReadConfigurations readConfig, ref string meterID)
        {
            displayText = "";
            StringBuilder displayParameterReadOutput = new StringBuilder();
           // string pushOutput = string.Empty;
            //Code Region Added by Vivek on 12 August 2011 (TNEB Project).
            //-->/DP/(777704)
            //-->(6A6B6C6D7476776E88898A33868384858B8C8E8D870F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D)y
            //-->(3F4144454647484A4C4F5253545556571A1B58595C5D5E5F606162636465666768690102030405070950517C5A7E7F8081820A0B0C0D0E)
            //-->(01020304050A0B0C0D0E0F1011121314151617181C1D1F20211E22232425262728292A2B2C192D2E2F3031323436397D5B3A3B3C3D3F4144454647484A4C4F52)
            //-->(53545556571A1B58595C5D5E5F6061626A6465666768696A6B6C6D7476776E88898A33868384858B8C8E8D877C817E7F80638207095051)
            //-->(787A7B79)
            MeterConfigurationConfigSection configSection = XMLLoader.GetConfigSection(GetConfigParameters("DisplayParameters"));
            DisplayParametersBLL displayParametersBLL = new DisplayParametersBLL();
            Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            configSection.ReadCommand = configSection.ReadCommand.Replace(ReadoutConstant.DATA, displayParametersBLL.GetReadCommand());
            //ReadConfigurations readConfig = new ReadConfigurations();
            string output = string.Empty;
            string statusMsg = "";
            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return "Aborted"; }
            string responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
            if (statusMsg == "Timeout!")
            {
                readConfig.BreakCommunication();
                this.StatusMessage = "Timeout!";
                return null;
            }
            displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);

            meterID = responseOutput.Substring(5, responseOutput.IndexOf("\r") - 5);
            output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
            displayText += (output.Substring(output.IndexOf("("), output.IndexOf(")") - output.IndexOf("(")));

            FormatterConfigurations formatterConfigurations = new FormatterConfigurations();
            Collection<string> parametersToSelect = new Collection<string>();
            //Get the count of various display parameter types stored in meter.
            int[] displayParmaterCountByType = formatterConfigurations.SplitDisplayParamaterCount(output);

            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return "Aborted"; }
            #region Read Scroll Paramaters
            //Get Scroll Paramater Config Section.
            configSection = XMLLoader.GetConfigurationSection(DisplayParameter.ScrollMode);
            string temp2 = configSection.ReadCommand;
            string[] scrollReadCommands = configSection.ReadCommand.Split('|');
            //Get Scroll Parameter Read Command.
            if (displayParmaterCountByType[0] < 64)
            {
                configSection.ReadCommand = scrollReadCommands[0].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(displayParmaterCountByType[0].ToString()));
                //Read Scroll Parameters from meter.
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
                output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
                displayText += (output.Substring(output.IndexOf("(") + 1, output.IndexOf(")") - output.IndexOf("(") - 1));

            }
            else
            {
                configSection.ReadCommand = scrollReadCommands[0].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue("64"));
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
                output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('('));
               // displayText += output.Substring(output.IndexOf("(") + 1);

                configSection.ReadCommand = scrollReadCommands[1].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue((displayParmaterCountByType[0] - 64).ToString()));
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
                output += responseOutput.Substring(responseOutput.IndexOf('(') + 1, responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
                //displayText += (output.Substring(output.IndexOf("(") + 1));
                displayText += (output.Substring(output.IndexOf("(") + 1, output.Length - 3));
            }
            configSection.ReadCommand = temp2;
            parametersToSelect = formatterConfigurations.ParseScrollModeParameters(output, Convert.ToInt32(displayParmaterCountByType[0]));
            displayParametersToSelect.Add(parametersToSelect);


            #endregion

            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return "Aborted"; }

            #region Read Push Parameter
            //Get Push Parameter config section.
            configSection = XMLLoader.GetConfigurationSection(DisplayParameter.PushMode);
            //Get Push Parameter Read Command.
            string temp = configSection.ReadCommand;
            string[] pushReadCommands = configSection.ReadCommand.Split('|');
            if (displayParmaterCountByType[1] < 64)
            {
                //Get Push Parameter Read Command.
                configSection.ReadCommand = pushReadCommands[0].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(displayParmaterCountByType[1].ToString()));
                //Read Push Parameters from meter.
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
               // pushOutput = ((string[])responseOutput.Split('|'))[1];
                output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
                displayText += (output.Substring(output.IndexOf("(") + 1, output.IndexOf(")") - output.IndexOf("(") - 1));
            }
            else
            {
                configSection.ReadCommand = pushReadCommands[0].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue("64"));
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
              //  pushOutput = ((string[])responseOutput.Split('|'))[1];

                output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('('));
                //displayText += output.Substring(output.IndexOf("(") + 1);

                configSection.ReadCommand = pushReadCommands[1].Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue((displayParmaterCountByType[1] - 64).ToString()));
                responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
                if (statusMsg == "Timeout!")
                {
                    readConfig.BreakCommunication();
                    this.StatusMessage = "Timeout!";
                    return null;
                }
                displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
                //pushOutput = ((string[])responseOutput.Split('|'))[1];
                output += responseOutput.Substring(responseOutput.IndexOf('(') + 1, responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
                displayText += (output.Substring(output.IndexOf("(") + 1, output.IndexOf(")") - output.IndexOf("(") - 1));

            }
            configSection.ReadCommand = temp;
            parametersToSelect = formatterConfigurations.ParsePushModeParameters(output, displayParmaterCountByType[1]);
            displayParametersToSelect.Add(parametersToSelect);
            //displayParameterReadOutput = displayParameterReadOutput.Append("<Push>");
            //for (int j = 0; j < parametersToSelect.Count; j++)
            //    displayParameterReadOutput = displayParameterReadOutput.Append(parametersToSelect[j] + "#");
            //displayParameterReadOutput.Remove(displayParameterReadOutput.Length - 1, 1);
            //displayParameterReadOutput = displayParameterReadOutput.Append("</Push>");

            #endregion          

            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return "Aborted"; }
            #region Read High Resolution Paramaters
            //Get HighResolutionMode Paramater Config Section.
            configSection = XMLLoader.GetConfigurationSection(DisplayParameter.HighResolutionMode);
            //Get HighResolutionMode Parameter Read Command.
            configSection.ReadCommand = configSection.ReadCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(displayParmaterCountByType[2].ToString()));
            //Read HighResolutionMode Parameters from meter.
            responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
            if (statusMsg == "Timeout!")
            {
                readConfig.BreakCommunication();
                this.StatusMessage = "Timeout!";
                return null;
            }
            displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
            output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
            displayText += (output.Substring(output.IndexOf("(") + 1, output.IndexOf(")") - output.IndexOf("(") - 1));
            parametersToSelect = formatterConfigurations.ParseHighResolutionModeParameters(output, Convert.ToInt32(displayParmaterCountByType[2]));
            displayParametersToSelect.Add(parametersToSelect);

            //displayParameterReadOutput = displayParameterReadOutput.Append("<HighResolution>");
            //for (int j = 0; j < parametersToSelect.Count; j++)
            //    displayParameterReadOutput = displayParameterReadOutput.Append(parametersToSelect[j] + "#");
            //displayParameterReadOutput.Remove(displayParameterReadOutput.Length - 1, 1);
            //displayParameterReadOutput = displayParameterReadOutput.Append("</HighResolution>");

            #endregion

            if (isAborted == true) { this.StatusMessage = "User Aborted."; isAborted = null; Application.DoEvents(); this.Cursor = Cursors.Default; return "Aborted"; }
            #region Read Display Timeouts
            //Get DisplayTimeouts Paramater Config Section.
            configSection = XMLLoader.GetConfigurationSection(DisplayParameter.DisplayTimeouts);
            //Read DisplayTimeouts Parameters from meter.
            responseOutput = readConfig.ReadMeterConfigurations(configSection, ref statusMsg);
            if (statusMsg == "Timeout!")
            {
                readConfig.BreakCommunication();
                this.StatusMessage = "Timeout!";
                return null;
            }
            displayParameterReadOutput = displayParameterReadOutput.Append(((string[])responseOutput.Split('|'))[1]);
            output = responseOutput.Substring(responseOutput.IndexOf('('), responseOutput.IndexOf(')') - responseOutput.IndexOf('(') + 1);
            displayText += (output.Substring(output.IndexOf("(") + 1, output.IndexOf(")") - output.IndexOf("(")));
            parametersToSelect = new Collection<string>();
            parametersToSelect.Add(output);
            displayParametersToSelect.Add(parametersToSelect);

            //displayParameterReadOutput = displayParameterReadOutput.Append("<DisplayTimeOuts>");
            //displayParameterReadOutput = displayParameterReadOutput.Append(output + "</DisplayTimeOuts>");
            #endregion

            //MessageBox.Show("Paramaters Read Successfully ");
            return displayParameterReadOutput.ToString();
        }
        //else { fileText += "0"; }

        #endregion

        #region Set DisplayTimeOuts by Display ParamaterREad Output
        /// <summary>
        /// This function sets the display parameter timeouts.
        /// </summary>
        /// <param name="displayTimeOutParamOutputString"></param>
        /// <param name="collDisplayParamatersDBEntity"></param>
        private void SetDisplayTimeOutsByDisplayParamaterReadOutput(string displayTimeOutParamOutputString, Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity)
        {
            int tmp;
            FormatterConfigurations formatterConfigurations = new FormatterConfigurations();
            DisplayParamatersDBEntity displayParamatersDBEntity = new DisplayParamatersDBEntity();

            tmp = Convert.ToInt32(displayTimeOutParamOutputString.Substring(1, 4), 16);
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text = tmp.ToString();
            //Fill DTO to write in Db.
            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
            displayParamatersDBEntity.paramaterName = "Scroll Time Out";
            displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

            tmp = Convert.ToInt32(displayTimeOutParamOutputString.Substring(5, 4), 16);
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text = tmp.ToString();
            //Fill DTO to write in Db.
            displayParamatersDBEntity = new DisplayParamatersDBEntity();
            displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
            displayParamatersDBEntity.paramaterName = "Push Time Out";
            displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
            collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

            tmp = Convert.ToInt32(displayTimeOutParamOutputString.Substring(9, 2), 16);
            if (tmp != 0)
            {
                ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = true;
                tmp = Convert.ToInt32(displayTimeOutParamOutputString.Substring(11, 4), 16);
                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = tmp.ToString();
                //Fill DTO to write in Db.
                displayParamatersDBEntity = new DisplayParamatersDBEntity();
                displayParamatersDBEntity.displayParamaterType = DisplayParameter.DisplayTimeouts;
                displayParamatersDBEntity.paramaterName = "Auto Scroll Resume Time";
                displayParamatersDBEntity.paramaterValue = Convert.ToInt32(tmp);
                collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);
            }
            else
            {
                ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = false;
                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = "";
                displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
            }


        }
        #endregion

        #region SelectRowsInDataGridByDisplayParamaterReadOutput
        /// <summary>
        /// this function sets the Push Mode, Auto Scroll Mode and High Resolution parameters to the respective datagrids.
        /// </summary>
        /// <param name="paramatersToSelect"></param>
        /// <param name="displayParameter"></param>
        /// <param name="collDisplayParamatersDBEntity"></param>
        private void SelectRowsInDataGridByDisplayParamaterReadOutput(Collection<string> paramatersToSelect, DisplayParameter displayParameter, Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity)
        {
            DataGridView dataGridView = new DataGridView();
            DisplayParamatersDBEntity displayParamatersDBEntity = new DisplayParamatersDBEntity();
            tabRS232LockUnlock.SelectedIndex = 3;
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];

            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 1;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            }
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 2;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null)
                    ((DataGridViewCheckBoxCell)dataGridView[0, i]).Value = false;
            }
            int chkboxColIndex = 0, descColIndex = 0;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == "Description")
                    descColIndex = i;
                if (dataGridView.Columns[i].Name == "colInclude")
                    chkboxColIndex = i;

            }
            #region Code to bind paramaters in data grid by Priority.
            for (int i = 0; i < paramatersToSelect.Count; i++)
            {//Get paramater name in Current row.
                DataGridViewTextBoxCell txtcell = dataGridView[descColIndex, i] as DataGridViewTextBoxCell;
                if (txtcell != null && txtcell.Value != null)
                {//If paramater name in current row is same as the paramater to write then no shuffling is required.
                    //But if paramater name in current row is different to paramater to write
                    //then the paramater in the current row is shifted to the old position of paramater to be written in current row.
                    if (txtcell.Value.ToString() != paramatersToSelect[i])
                    {//if paramater name is not same as paramater name in current row then find the 
                        //row position of old occurence of paramater to be written in current row.
                        for (int j = i; j < dataGridView.Rows.Count; j++)
                        {
                            DataGridViewTextBoxCell txtcell2 = dataGridView[descColIndex, j] as DataGridViewTextBoxCell;
                            //if this condition is true then we get the old position of the paramater to be written in current row.
                            if (txtcell2 != null && txtcell2.Value != null && txtcell2.Value.ToString() == paramatersToSelect[i])
                            {//move the paramater in current row to this new position.
                                txtcell2.Value = txtcell.Value;
                                DataGridViewCheckBoxCell cell2 = dataGridView[chkboxColIndex, j] as DataGridViewCheckBoxCell;
                                if (cell2 != null)
                                    ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, j]).Value = false;
                            }
                        }
                    }//Set the paramater to be written to cell in the current row.
                    txtcell.Value = paramatersToSelect[i];
                    DataGridViewCheckBoxCell cell = dataGridView[chkboxColIndex, i] as DataGridViewCheckBoxCell;
                    if (cell != null)
                        ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, i]).Value = true;
                }

                //Fill DTO to write in Db.
                displayParamatersDBEntity = new DisplayParamatersDBEntity();
                displayParamatersDBEntity.displayParamaterType = displayParameter;
                displayParamatersDBEntity.paramaterName = paramatersToSelect[i];
                displayParamatersDBEntity.paramaterValue = 1;
                collDisplayParamatersDBEntity.Add(displayParamatersDBEntity);

            }
            //if (displayParameter == DisplayParameter.PushMode)
            //{
            //    displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"].Select();
            //    ((DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"]).DataSource = dataGridView;
            //}

            #endregion

            tabRS232LockUnlock.SelectedIndex = 0;
            ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 0;
        }
        #endregion

        private void timer_RTC_Tick(object sender, EventArgs e)
        {
            CultureInfo c = new CultureInfo("en-GB");
            //System.Threading.Thread.CurrentThread.CurrentCulture = c;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = c;
            rtcCtrl.Controls[0].Controls["txtRTC"].Text = System.DateTime.Now.ToString(c);
        }

        private void displayParameters_Load(object sender, EventArgs e)
        {
            DataGridView highResolutionGrid = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            DataViewManager dataView = highResolutionGrid.DataSource as DataViewManager;
            for (int rowCount = 0; rowCount < highResolutionGrid.Rows.Count; rowCount++)
            {
                if (highResolutionGrid.Rows[rowCount].Cells["Description"].Value.ToString() == "Cumulative Reverse Active Energy" || highResolutionGrid.Rows[rowCount].Cells["Description"].Value.ToString() == "Cumulative Reverse Lag Energy"
                    || highResolutionGrid.Rows[rowCount].Cells["Description"].Value.ToString() == "Cumulative Reverse Lead Energy" || highResolutionGrid.Rows[rowCount].Cells["Description"].Value.ToString() == "Cumulative Reverse Apparent Energy")
                {
                    highResolutionGrid.Rows.Remove(highResolutionGrid.Rows[rowCount]);
                    rowCount--;
                }
            }
           
        }

        private void rdbCurrentTOD_CheckedChanged(object sender, EventArgs e)
        {
            if (readFromFile == true)
            {
                if (touType == EnumUtil.StringValue(TOUType.Current))
                {
                    if (rdbCurrentTOD.Checked)
                    {
                        DisplaycfgFile(touFileUploadPath);
                        tabRS232LockUnlock.SelectedIndex = 4; 
                    }
                    else
                    {
                        group = this.touOperation1.Controls["grpTOU"].Controls["grpButtons"];
                        Button btnResetTODData = (Button)group.Controls["btnResetAll"];
                        btnResetTODData.PerformClick();
                    }
                }
            }
            else
            {
                if (currTODReadResult != "" && futureTODReadResult != "")
                {
                    if (rdbFutureTOD.Checked)
                        DisplayTOU(futureTODReadResult, "Future");
                    else
                        DisplayTOU(currTODReadResult, "Current");
                }
            }
        }

        private void rdbFutureTOD_CheckedChanged(object sender, EventArgs e)
        {
            if (readFromFile == true)
            {
                if (touType == EnumUtil.StringValue(TOUType.Future))
                {
                    if (rdbCurrentTOD.Checked)
                    {
                        group = this.touOperation1.Controls["grpTOU"].Controls["grpButtons"];
                        Button btnResetTODData = (Button)group.Controls["btnResetAll"];
                        btnResetTODData.PerformClick();
                    }
                    else
                    {
                        DisplaycfgFile(touFileUploadPath);
                        tabRS232LockUnlock.SelectedIndex = 4;
                    }
                }
            }
            else
            {
                if (currTODReadResult != "" && futureTODReadResult != "")
                {
                    if (rdbFutureTOD.Checked)
                        DisplayTOU(futureTODReadResult, "Future");
                    else
                        DisplayTOU(currTODReadResult, "Current");
                }
            }
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            this.StatusMessage = "";
        }

        /// <summary>
        /// This function converts the TOD data string to the hex format to be saved into the configuration file.
        /// </summary>
        /// <param name="cmdTou"></param>
        /// <returns></returns>
        private string CreateTOUString(string cmdTou)
        {
            int l = cmdTou.Length;
            int cmdLength = 0;
            string strRes = string.Empty;
            cmdTou = cmdTou.Substring(1, cmdTou.Length - 2);
            cmdTou = ProgrammingCommon.GetASCIIValue(cmdTou);
            while (cmdLength < cmdTou.Length)
            {
                if (cmdLength == cmdTou.Length - 2)
                    strRes += cmdTou.Substring(cmdLength, 2);
                else
                    strRes += cmdTou.Substring(cmdLength, 2) + "\x20";
                cmdLength += 2;
            }
            int s = strRes.Length;
            return strRes;
        }

        /// <summary>
        /// Thiss function accpts the TOD data string read from the meter and converts it into suitable format 
        /// to be written into the configuration file.
        /// </summary>
        /// <param name="readoutData"></param>
        /// <returns></returns>
        private string CreateTOUCommand(string readoutData)
        {
            ProgrammingCommon programmingCommon = new ProgrammingCommon();
            int touIndex = 0, touSeason = 0, touSeasonDay = 0, touHoliday = 0, touDateActvation = 0;
            List<string> touFileContent = new List<string>();
            string fileContent = string.Empty;
            string[] seasonActivationDates = new string[4];
            touFileContent = programmingCommon.GetTOUParameters(readoutData);
            if (touFileContent[0].Trim().Length == 0)
                return string.Empty;

            touSeason = 1;
            int seasonIndex = 0;
            while (touSeason <= 4)  //Season Day Table
            {
                
                touSeasonDay = 0;
                while (touSeasonDay <= 6)
                {

                    if (touSeasonDay == 6)
                    {
                        seasonActivationDates[seasonIndex] = "(<SD" + (touDateActvation + 1).ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</SD" + (touDateActvation + 1).ToString() + ">)\r\n";
                        seasonIndex++;
                        touDateActvation++;
                        touSeasonDay++;
                        touIndex++;
                    }
                    else
                    {
                        fileContent += "(<S" + touSeason.ToString() + "D" + (touSeasonDay + 1).ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</S" + touSeason.ToString() + "D" + (touSeasonDay + 1).ToString() + ">)\r\n";
                        touSeasonDay++;
                        touIndex++;
                    }

                }
                touSeason++;
            }

            touHoliday = 1;
            while (touHoliday <= 10)    //Holiday
            {
                fileContent += "(<HD" + touHoliday.ToString() + ">" + CreateTOUString(touFileContent[touIndex]) + "</HD" + touHoliday.ToString() + ">)\r\n";
                touHoliday++;
                touIndex++;

            }

            touDateActvation = 1;
            while (touDateActvation <= 4)   //Activation Date
            {
                fileContent += seasonActivationDates[touDateActvation-1];
                touDateActvation++;

            }

            /*Future Activation date*/
            fileContent += "(<FSAD>" + CreateTOUString(touFileContent[touIndex]) + "</FSAD>)";
            return fileContent;
        }

        private void chkMDWithIP_CheckedChanged(object sender, EventArgs e)
        {
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            if (chkMDWithIP.Checked && chkKVARSelcetion.Checked && chkDisplayParam.Checked && chkTOD.Checked
    && chkRTC.Checked && chkBilingReset.Checked && chkReset.Checked && chkDailyLog.Checked && chkLockRS232.Checked)
            {
                chkSelectAll.Checked = true;
            }
            else
            {
                chkSelectAll.Checked = false;
            }
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }

        private void chkKVARSelcetion_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkDisplayParam_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkTOD_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkRTC_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkBilingReset_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkReset_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkDailyLog_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        private void chkLockRS232_CheckedChanged(object sender, EventArgs e)
        {
            chkMDWithIP_CheckedChanged(sender, e);
        }

        /// <summary>
        /// This function extracts the 'TOD' data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetTODDataforFile()
        {
            List<string> todDataList = new List<string>();
            string todData = string.Empty;
            int slots = 0;
            string dayTable = string.Empty;
            string holidayActivationDate = string.Empty;

            ///////////Get Data from UI////////////////
            DataGridView[] gridSeason = GetSeasonGridCollection();
            DataGridView[] gridHoliday = GetHolidayGridCollection();
            DataGridView[] gridDayAssignment = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();

            string strTODdata = string.Empty;
            for (int seasonIndex = 0; seasonIndex <= gridSeason.GetUpperBound(0); seasonIndex++)
            {
                slots = 0;
                dayTable = string.Empty;
                todData = string.Empty;
                
                foreach (DataGridViewRow row in gridSeason[seasonIndex].Rows)
                {
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }

                
                for (int i = 0; i < todData.Length; i = i + 2)
                {
                   
                     strTODdata += todData.Substring(i, 2) + " ";
                     
                }
                if (strTODdata.EndsWith(" ")) strTODdata = strTODdata.Substring(0, strTODdata.Length - 1);
                todData = strTODdata;
                strTODdata = string.Empty;

                dayTable = ProgrammingCommon.GetASCIIValue((seasonIndex % 6 + 1 % 7).ToString("d2"));
                dayTable = dayTable.Substring(0, 2) + " " + dayTable.Substring(2, 2);

                string st = ProgrammingCommon.GetASCIIValue(slots.ToString("d2"));
                st = st.Substring(0, 2) + " " + st.Substring(2, 2);

                todDataList.Add(dayTable + " " + st + " " + todData );
            }

            for (int holidayIndex = 0; holidayIndex <= gridHoliday.GetUpperBound(0); holidayIndex++)
            {
                slots = 0;
                dayTable = string.Empty;
                todData = string.Empty;
                foreach (DataGridViewRow row in gridHoliday[holidayIndex].Rows)
                {
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    todData += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }

                
                for (int i = 0; i < todData.Length; i = i + 2)
                {
                    strTODdata += todData.Substring(i, 2) + " ";
                }
                if (strTODdata.EndsWith(" ")) strTODdata = strTODdata.Substring(0, strTODdata.Length - 1);
                todData = strTODdata;
                strTODdata = string.Empty;

                dayTable = ProgrammingCommon.GetASCIIValue((holidayIndex % 10 + 1 % 11).ToString("d2"));
                dayTable = dayTable.Substring(0, 2) + " " + dayTable.Substring(2, 2);
                if (rdbFutureTOD.Checked)
                {
                    holidayActivationDate = ProgrammingCommon.GetASCIIValue(DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString().Substring(2, 6));
                }
                else
                {
                    holidayActivationDate = ProgrammingCommon.GetASCIIValue("000000");
                }
                string str = string.Empty;
                for (int i = 0; i < holidayActivationDate.Length; i = i + 2)
                {
                    str += holidayActivationDate.Substring(i, 2) + " ";
                }
                if (str.EndsWith(" ")) str = str.Substring(0, str.Length - 1);

                string st = ProgrammingCommon.GetASCIIValue(slots.ToString("d2"));
                st = st.Substring(0, 2) + " " + st.Substring(2, 2);

                todDataList.Add(str + " " + dayTable + " " + st + " " + todData );
            }

            for (int dayAssignment = 0; dayAssignment <= gridDayAssignment.GetUpperBound(0); dayAssignment++)
            {
                todData = string.Empty;
                foreach (DataGridViewRow row in gridDayAssignment[dayAssignment].Rows)
                {
                    string tempStr = row.Cells[1].Value.ToString();
                    todData += ProgrammingCommon.GetASCIIValue(tempStr.Replace("Day Table ", "0").Trim());
                }

                for (int i = 0; i < todData.Length; i = i + 2)
                {
                    strTODdata += todData.Substring(i, 2) + " ";
                }
                if (strTODdata.EndsWith(" ")) strTODdata = strTODdata.Substring(0, strTODdata.Length - 1);
                todData = strTODdata;
                strTODdata = string.Empty;

                todDataList.Add(todData);
            }
            todData = string.Empty;
            if (rdbFutureTOD.Checked)
            {
                string fututeAcDate = "";
                group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
                DateTimePicker dtFutureAcDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];

                fututeAcDate += ProgrammingCommon.GetASCIIValue(dtFutureAcDate.Value.Day.ToString("d2"));
                fututeAcDate += ProgrammingCommon.GetASCIIValue(dtFutureAcDate.Value.Month.ToString("d2"));
                fututeAcDate += ProgrammingCommon.GetASCIIValue(dtFutureAcDate.Value.Year.ToString().Substring(2));
                todData += fututeAcDate;
            }

            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string tempCommand = "";
                DateTime dateTime = ProgrammingCommon.GetDate(row.Cells["SeasonActivationDate"].Value.ToString(), true);
                tempCommand = String.Format("{0:00}", dateTime.Day.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todData += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", dateTime.Month.ToString());
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todData += ProgrammingCommon.GetASCIIValue(tempCommand);
                tempCommand = String.Format("{0:00}", Convert.ToInt16(row.Cells["SeasonNumber"].Value.ToString()));
                if (tempCommand.Length < 2) { tempCommand = "0" + tempCommand; }
                todData += ProgrammingCommon.GetASCIIValue(tempCommand);
            }

            

            for (int i = 0; i < todData.Length; i = i + 2)
            {
                strTODdata += todData.Substring(i, 2) + " ";
            }
            if (strTODdata.EndsWith(" ")) strTODdata = strTODdata.Substring(0, strTODdata.Length - 1);
            todData = strTODdata;
            strTODdata = string.Empty;

            todDataList.Add(todData);

            //////////Add headers to the data and get it all into a single string named 'fileContent' ///////////////

            int touIndex = 0, touSeason = 0, touSeasonDay = 0, touHoliday = 0, touDateActvation = 0;
            List<string> touFileContent = new List<string>();
            string fileContent = string.Empty;
            string[] seasonActivationDates = new string[4];

            touSeason = 1;
            int sIndex = 0;
            while (touSeason <= 4)  //Season Day Table
            {

                touSeasonDay = 0;
                while (touSeasonDay < 6)
                {

                    if (touSeasonDay == 6)
                    {
                        seasonActivationDates[sIndex] = "(<SD" + (touDateActvation + 1).ToString() + ">" + todDataList[touIndex] + "</SD" + (touDateActvation + 1).ToString() + ">)\r\n";
                        sIndex++;
                        touDateActvation++;
                        touSeasonDay++;
                        touIndex++;
                    }
                    else
                    {
                        fileContent += "(<S" + touSeason.ToString() + "D" + (touSeasonDay + 1).ToString() + ">" + todDataList[touIndex] + "</S" + touSeason.ToString() + "D" + (touSeasonDay + 1).ToString() + ">)\r\n";
                        touSeasonDay++;
                        touIndex++;
                    }

                }
                touSeason++;
            }

            touHoliday = 1;
            while (touHoliday <= 10)    //Holiday
            {
                fileContent += "(<HD" + touHoliday.ToString() + ">" + todDataList[touIndex] + "</HD" + touHoliday.ToString() + ">)\r\n";
                touHoliday++;
                touIndex++;

            }

            touDateActvation = 1;
            while (touDateActvation <= 4)   //Activation Date
            {
                fileContent += "(<SD" + touDateActvation.ToString() + ">" + todDataList[touIndex] + "</SD" + touDateActvation.ToString() + ">)\r\n";
                touDateActvation++;
                touIndex++;
            }

            /*Future Activation date*/
            fileContent += "(<FSAD>" + todDataList[touIndex] + "</FSAD>)";
            return fileContent;
        }

        /// <summary>
        /// This function extracts the RTC data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetRTCDataforFile()
        {
            string rtcData = "";
            rtcData = System.DateTime.Now.Day.ToString("00") + System.DateTime.Now.Month.ToString("00") + System.DateTime.Now.Year.ToString("00").Substring(2);
            rtcData += System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00");
            rtcData += Convert.ToChar(3);         
            int bcc = 0;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(rtcData);
            int count = 0;
            
            foreach (byte b in bytes)
            {
                if (count <= bytes.Length) bcc = bcc ^ b;
                count++;
            }
            rtcData += Convert.ToChar(bcc);
            return rtcData;
        }

        /// <summary>
        /// This function extracts the Push Mode Display Parameters data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetPushModeParameters()
        {
            DisplayParametersBLL DisplayParametersBLL = new PushModeParameterBLL();
            string cmdData = "";
            if (collSelectedDisplayParameters[1].Count > 64)
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[1], 0, 63);
                cmdData += DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[1], 64, collSelectedDisplayParameters[1].Count - 1);

            }
            else
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[1], 0, collSelectedDisplayParameters[1].Count - 1);
            }
            return cmdData;
        }

        /// <summary>
        /// This function extracts the Scroll Mode Display Parameters data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetScrollModeParameters()
        {
            DisplayParametersBLL DisplayParametersBLL = new ScrollModeParameterBLL();
            string cmdData = "";
            if (collSelectedDisplayParameters[0].Count > 64)
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[0], 0, 63);
                cmdData += DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[0], 64, collSelectedDisplayParameters[0].Count - 1);

            }
            else
            {
                cmdData = DisplayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[0], 0, collSelectedDisplayParameters[0].Count - 1);
            }
            return cmdData;
        }

        /// <summary>
        /// This function extracts the High Resolution Display Parameters data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string GetHighReolutionParameters()
        {
            DisplayParametersBLL displayParametersBLL = new HighResolutionModeParameterBLL();
            string cmdData = "";
            cmdData= displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters[2], 0, collSelectedDisplayParameters[2].Count - 1);
            return cmdData;
        }
 
        /// <summary>
        /// This function extracts the 'Display Parameters' data from the UI to be written into the configuration file.
        /// </summary>
        /// <returns></returns>
        private string  GetDisplayParameters()
        {
            string displayData = "";

                collSelectedDisplayParameters = new Collection<Collection<string>>();
                Collection<DisplayParamatersDBEntity> displayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
                this.Cursor = Cursors.WaitCursor;
                string displayParamValidationStatus = ValidateDisplayParameterInputs();
                
                if (displayParamValidationStatus == string.Empty)
                {
                    
                    collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.ScrollMode, displayParamatersDBEntity));
                    collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.PushMode, displayParamatersDBEntity));
                    collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.HighResolutionMode, displayParamatersDBEntity));
                    collSelectedDisplayParameters.Add(GetSelectedParameters(DisplayParameter.DisplayTimeouts, displayParamatersDBEntity));
                }

                DisplayParametersBLL displayParametersBLL = new DisplayParametersBLL();
                displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters);
                string displayTimeouts= collSelectedDisplayParameters[3][0];

                string cnt = displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters);
                string scroll = GetScrollModeParameters();
                string push = GetPushModeParameters();
                string high = GetHighReolutionParameters();

                displayData = displayParametersBLL.GetWriteCommand(collSelectedDisplayParameters) + GetScrollModeParameters() +   GetPushModeParameters() +  GetHighReolutionParameters() + displayTimeouts;
            return displayData;
        }

        private void btnCreateCfgFile_Click(object sender, EventArgs e)
        {
            fileText = "";
            this.StatusMessage = "";
            currTODReadResult = "";
            futureTODReadResult = "";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            errmsg = "";
            try
            {
                if (CheckValidations("create configuration file"))
                {
                    if (chkMDWithIP.Checked)
                        fileText += "(1";
                    else { fileText += "(0"; }

                    if (chkKVARSelcetion.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    if (chkDisplayParam.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    if (chkTOD.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    if (chkRTC.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    // For Billing Reset
                    if (chkBilingReset.Checked)
                        fileText += "1";
                    else { fileText += "0"; }

                    //For Resets.
                    if (chkReset.Checked)
                        fileText += "1";
                    else
                        fileText += "0";

                    //-------------------Added Daily log, but this in not in sequence--------------------------------//
                    if (chkDailyLog.Checked)
                        fileText += "1";
                    else { fileText += "0"; }
                    //while (fileText.Length <= 8) { fileText += "0"; }
                    if (chkLockRS232.Checked)
                        fileText += "1";
                    else { fileText += "0"; }
                    fileText += ")\r\n";

                    int index = 0;
                    int count = 0;
                    string cmdResult = string.Empty;
                    try
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            if (fileText.Substring(i, 1) == "0") { fileText += "(00)\r\n"; }
                            else
                            {
                                //MD with IP
                                if (index == 0)
                                {
                                    if (!CheckMDwithIP())
                                    {
                                        this.StatusMessage = resourceMgr.GetString("selectMDWithIPMsg");
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else 
                                    fileText += "(" + ConvertHexToInt(GetMDWithIP()) +")\r\n"; index++; continue;
                                }

                                //kVAh Selection
                                if (index == 1)
                                {
                                    fileText += "(" + ConvertHexToInt(GetkvarSelection()) + ")\r\n"; index++; continue; 
                                }

                                //Display Parameters
                                if (index == 2)
                                {
                                    if (ValidateDisplayParameterInputs() != String.Empty)
                                    {
                                        this.StatusMessage = ValidateDisplayParameterInputs();
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                        fileText += "(" + ConvertHexToInt(GetDisplayParameters()) + ")\r\n"; index++; continue;
                                    
                                }

                                //TOD
                                if (index == 3)
                                {
                                    if (!ValidateTOUData())
                                    {
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                        fileText += GetTODDataforFile() + "\r\n"; index++; continue;
                                }

                                //RTC
                                if (index == 4)
                                {
                                    if (GetRTCDataforFile()==String.Empty)
                                    {
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                        fileText += "(" + GetRTCDataforFile() + ")\r\n"; index++; continue;
                                }

                                // Billing Type
                                if (index == 5)
                                {
                                    if (!CheckBillingReset())
                                    {
                                        this.StatusMessage = resourceMgr.GetString("ValidationMsg_BillingReset");
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                        fileText += "(" + ConvertHexToInt(GetBillingPeriod()) + "|" + ConvertHexToInt(GetModeOfBilling()) + "|" + ConvertHexToInt(GetResetLockOutDays()) + ")\r\n"; index++; continue;
                                }

                                //Billing Reset
                                if (index == 6)
                                {
                                    fileText += "(" + ConvertHexToInt(GetBillingResetStatus()) + ")\r\n"; index++; continue;
                                }

                                //Daily Log
                                if (index == 7)
                                {
                                    if (!CheckDailyLog())
                                    {
                                        this.StatusMessage = resourceMgr.GetString("ValidationMsg_DailyLog");
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                    else
                                        fileText += "(" + ConvertHexToInt(GetDailyLog()) + ")\r\n"; index++; continue;
                                }

                                //RS 232 Lock/ Unlock
                                if (index == 8)
                                {
                                    string portStatus="";
                                    if (chkLockRS232Port.Checked)
                                        portStatus = "3030";
                                    else
                                        portStatus = "3031";
                                    fileText += "(" + ConvertHexToInt(portStatus) + ")\r\n"; index++; continue;
                                }
                            }
                            index++;
                        }
                    }
                    catch (Exception)
                    {
                        errmsg = errmsg + "\n" + "Error in creation of CFG File";
                    }

                    try
                    {
                        SaveData(fileText);
                    }
                    catch (Exception ee)
                    {
                        errmsg = errmsg + "\n" + "Error in saving CFG file";
                    }
                }
            }
            catch (Exception et)
            {
                this.StatusMessage = et.Message;
               
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// This function converts the selected data string from the hex format to the ASCII format.
        /// </summary>
        /// <param name="hexvalue"></param>
        /// <returns></returns>
        public string ConvertHexToInt(string hexvalue)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexvalue.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexvalue.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        /// <summary>
        /// This function converts the TOD data from the hex format to the ASCII format 
        /// to split it and display the TOD records on the UI.
        /// </summary>
        /// <param name="hexvalue"></param>
        /// <returns></returns>
        public string ConvertTODHexToInt(string hexvalue)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexvalue.Length - 2; i += 3)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexvalue.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        /// <summary>
        /// This function displays the data from the selected configuration file onto the UI.
        /// </summary>
        /// <param name="filePath"></param>
        private void  DisplaycfgFile(string filePath)
        {
            string fileContent = string.Empty;
            long  index = 0;
            string strTemp = string.Empty;
            string[] readData = new string[25];
            string[] finalData = new string[40];

           
                StreamReader streamReader = File.OpenText(filePath);
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (fileStream.Length <= 0)
                    return;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    List<ReadResult> readResult = new List<ReadResult>();
                    Collection<MeterConfigurationConfigSection> configSection = new Collection<MeterConfigurationConfigSection>();

                    string s = reader.ReadToEnd();
                    string[] testarray = new string[1];
                    testarray[0] = "\r\n";
                    string[] sarray = s.Split(testarray, StringSplitOptions.RemoveEmptyEntries);
                    string[] arrTouData = new string[sarray.Length - 1];
                    int touDataIndex = 0;
                    string strTouData = String.Empty;
                    if (sarray[4] != "(00)")
                    {
                        for (int seasonIndex = 4; seasonIndex <= 9; seasonIndex++)
                        {
                            arrTouData[touDataIndex] = sarray[seasonIndex].Replace(sarray[seasonIndex].Substring(1, 6), "");
                            arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), "");
                            touDataIndex++;
                        }
                        arrTouData[touDataIndex] = sarray[38].Replace(sarray[38].Substring(1, 5), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 6), ""); touDataIndex++;

                        for (int seasonIndex = 10; seasonIndex <= 15; seasonIndex++)
                        {
                            arrTouData[touDataIndex] = sarray[seasonIndex].Replace(sarray[seasonIndex].Substring(1, 6), "");
                            arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), "");
                            touDataIndex++;
                        }
                        arrTouData[touDataIndex] = sarray[39].Replace(sarray[39].Substring(1, 5), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 6), ""); touDataIndex++;

                        for (int seasonIndex = 16; seasonIndex <= 21; seasonIndex++)
                        {
                            arrTouData[touDataIndex] = sarray[seasonIndex].Replace(sarray[seasonIndex].Substring(1, 6), "");
                            arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), "");
                            touDataIndex++;
                        }
                        arrTouData[touDataIndex] = sarray[40].Replace(sarray[40].Substring(1, 5), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 6), ""); touDataIndex++;

                        for (int seasonIndex = 22; seasonIndex <= 27; seasonIndex++)
                        {
                            arrTouData[touDataIndex] = sarray[seasonIndex].Replace(sarray[seasonIndex].Substring(1, 6), "");
                            arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), "");
                            touDataIndex++;
                        }
                        arrTouData[touDataIndex] = sarray[41].Replace(sarray[41].Substring(1, 5), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 6), ""); touDataIndex++;

                        for (int seasonIndex = 28; seasonIndex <= 36; seasonIndex++)
                        {
                            arrTouData[touDataIndex] = sarray[seasonIndex].Replace(sarray[seasonIndex].Substring(1, 5), "");
                            arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 6), "");
                            touDataIndex++;
                        }
                        arrTouData[touDataIndex] = sarray[37].Replace(sarray[37].Substring(1, 6), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), ""); touDataIndex++;

                        arrTouData[touDataIndex] = sarray[42].Replace(sarray[42].Substring(1, 6), "");
                        arrTouData[touDataIndex] = arrTouData[touDataIndex].Replace(arrTouData[touDataIndex].Substring(arrTouData[touDataIndex].IndexOf("/") - 1, 7), ""); touDataIndex++;
                        if (arrTouData[38].Substring(1, arrTouData[38].Length - 2).Length > 71)
                        {
                            rdbFutureTOD.Checked = true;
                            touType = EnumUtil.StringValue(TOUType.Future);
                        }
                        else
                        {
                            rdbCurrentTOD.Checked = true;
                            touType = EnumUtil.StringValue(TOUType.Current);
                        }

                        if (arrTouData[0] != null)
                        {
                            StringBuilder touData = new StringBuilder();
                            for (int touIndex = 0; touIndex <= 38; touIndex++)
                            {
                                touData.Append("(" + ConvertTODHexToInt(arrTouData[touIndex].Substring(1, arrTouData[touIndex].Length - 2)) + ")");
                            }
                            strTouData = touData.ToString();
                        }
                    }


                    else
                    {
                        strTouData = "(00)";
                    }


                    int intParametersLength = sarray[0].Length - 1;

                    string[] strNewArray = new string[intParametersLength + 1];
                    int j = 0;
                    int dataIndex = 0;
                    for (int i = 1; i < intParametersLength; i++)
                    {
                        if (i == 7)
                        {
                            if (sarray[0].Substring(sarray[0].Length - 4, 1) == "1")
                            {
                                configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("BillingReset")));
                                strNewArray[dataIndex++] = sarray[sarray.Length - 3];
                            }
                            j++;
                        }
                        if (i == 9)
                        {
                            if (sarray[0].Substring(sarray[0].Length - 2, 1) == "1")
                            {
                                configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("LockUnlockRS232")));
                                strNewArray[dataIndex++] = sarray[sarray.Length - 1];
                            }
                        }
                        if (j >= 40 && j <= 47)
                        {
                            if (sarray[j] == "(00)") { j++; continue; }
                        }
                        if (i == 2)
                        {
                            /*GKG 136021 : kvah selection not working fine*/

                            //configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("kvahSelection")));
                            //strNewArray[dataIndex++] = sarray[i];

                            if (sarray[0].Substring(2,1) == "1")
                            {
                                configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("kvahSelection")));
                                strNewArray[dataIndex++] = sarray[i];
                            }
                            /*GKG 136021 : kvah selection not working fine*/
                        }

                        if (sarray[i] != "(00)")
                        {
                            if (i < 4)
                            {
                                if (i == 1)
                                {
                                    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("MDWithIP")));
                                    strNewArray[dataIndex++] = sarray[i];
                                }
                                //else if (i == 2)
                                //    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("kvahSelection")));
                                else if (i == 3)
                                {
                                    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("DisplayParameters")));
                                    strNewArray[dataIndex++] = sarray[i];
                                }
                            }

                            if (i == 4)
                            {
                                configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("TOD")));
                                strNewArray[dataIndex++] = strTouData; j = i + 39;
                            }

                            if (i == 5)
                            {
                                configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("RTC")));
                                if (strTouData == "(00)")
                                { strNewArray[dataIndex++] = sarray[i]; }
                                else
                                    strNewArray[dataIndex++] = sarray[j++];
                            }

                            if (i == 6)
                            {
                                if (sarray[i] != "(00)")
                                {
                                    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("BillingPeriod")));
                                    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ModeOfBilling")));
                                    configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("ResetLockOutDays")));
                                    string[] strBilling = new string[3];
                                    if (strTouData == "(00)")
                                    { strBilling = sarray[i].Split('|'); }
                                    else
                                    { strBilling = sarray[j++].Split('|'); }
                                    strNewArray[dataIndex++] = strBilling[0] + ")";
                                    strNewArray[dataIndex++] = "(" + strBilling[1] + ")";
                                    strNewArray[dataIndex++] = "(" + strBilling[2];
                                }
                            }

                            if (i == 8)
                            {
                                if (sarray[i] != "(00)")
                                {
                                    if (i == 8)
                                        configSection.Add(XMLLoader.GetConfigSection(GetConfigParameters("DailyLog")));
                                    if (strTouData == "(00)")
                                    { strNewArray[dataIndex++] = sarray[i]; }
                                    else
                                    { strNewArray[dataIndex++] = sarray[j++]; }
                                }
                            }
                        }
                    }

                    index = 0;
                    foreach (MeterConfigurationConfigSection section in configSection)
                    {

                        ReadResult result = new ReadResult();
                        result.CommandName = section.Name;
                        Enum sectionName = GetConfigParameters(section.Name);
                        result.Result = strNewArray[index];
                        result.section = section;
                        readResult.Add(result);
                        index++;
                    }

                    FormatData(readResult, false);
                }
                fileStream.Close();
                streamReader.Close();
          
      
        }


        /// <summary>
        /// This function clears the controls on the 'Main' tab in the Meter Configurations window.
        /// </summary>
        private void ClearMainTab()
        {
            chkMDWithIP.Checked = false;
            chkKVARSelcetion.Checked = false;
            chkDisplayParam.Checked = false;
            chkTOD.Checked = false;
            chkBilingReset.Checked = false;
            chkReset.Checked = false;
            chkRTC.Checked = false;
            chkDailyLog.Checked = false;
            chkLockRS232.Checked = false;
        }

        /// <summary>
        /// This function created=s the configuration file from the UI.
        /// </summary>
        /// <param name="filePath"></param>
        private void DisplayConfigurationFromFile(string filePath)
        {
            readFromFile = true;
            DisplaycfgFile(filePath);
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            ClearMainTab();
            ClearControls();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "Configuration File";
            openFile.InitialDirectory = ConfigInfo.GetLocation();
            openFile.Filter = "Configuration file(*.cfg)|*.cfg";
            DialogResult result = openFile.ShowDialog();
            try
            {
                if (result == DialogResult.OK)
                {
                    touFileUploadPath = openFile.FileName;
                    DisplayConfigurationFromFile(openFile.FileName);
                    this.StatusMessage = resourceMgr.GetString("Upload");
                }
            }

            catch (Exception)
            {
                MessageBox.Show(" Invalid File ", "BCS");

            }
        }        

       
    }
}
