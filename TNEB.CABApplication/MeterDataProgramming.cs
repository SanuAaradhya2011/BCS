using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using CAB.IECFramework;
using CAB.IECChannel;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;
using CAB.IECChannel.Programming;
using System.IO;
using System.Text.RegularExpressions;
using CAB.IECChannel.ReadOut;

namespace CAB.UI
{
    public partial class MeterDataProgramming : MdiChildForm
    {
        private IECChannelBase communications;
        private Command command;
        private string meterPswd;
        private int ctRatio;
        private bool isValidTOU = true;
        string touFileName = "";
        string statusMsg = "";
        public MeterDataProgramming()
        {
            InitializeComponent();
            command = Command.GetInstance();
            communications = ChannelManager.GetChannel() as LocalCommunication;
        }

        private void InitializeProgrammingValues()
        {
            this.StatusMessage = "";
            ctRatio = 0;
            meterPswd = string.Empty;
            tabPageTOU.Enabled = false;
            grpResets.Enabled = false;
            Application.DoEvents();
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
                if (statusMsg!="Invalid RTC." && meterRTCData.Length == 0)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                communications = ChannelManager.GetChannel() as LocalCommunication;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (chkBoxMinAvgCurrent.Checked == false && chkBoxMaxAvgCurrent.Checked == false && chkBoxMinAvgVoltage.Checked == false && chkBoxMaxAvgVoltage.Checked == false && chkBoxDailyMD1.Checked == false && chkBoxDailyMD2.Checked == false && chkBoxDailyMD3.Checked == false && chkBoxKwh.Checked == false && chkBoxKvarhLag.Checked == false && chkBoxKvarhLead.Checked == false && chkBoxKVAh.Checked == false && chkBoxCumFundkWh.Checked==false)
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
            if(string.IsNullOrEmpty(fileContent))
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
            DateTimePicker[] holidayActivationDates =  new DateTimePicker[]
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
                dtPickerFutureActivationDate.Value = ProgrammingCommon.GetDate(ProgrammingCommon.futureActivationDate,true);     
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
            dtPickerFutureActivationDate.Value = Convert.ToDateTime(ds.Tables[tableIndex].Rows[0]["Season Activation Date"].ToString(),new CultureInfo("en-GB"));
            
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                FinalizeProgrammingValues();
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSetTOU_Click(object sender, EventArgs e)
        {
            if (!isValidTOU)
            {
                this.StatusMessage = "Invalid Entry!";
                Application.DoEvents();
                return;
            }
            if (!ValidateTOUData())
            {
                this.Cursor = Cursors.Default;
                return;
            }
            List<string> touCommands;
            TOUInformation touInformation = new TOUInformation();
            touInformation.OnChannelStatusChanged += new RTCInformation.ChannelStatusChanged(Channel_OnStatusChanged);
            MeterPassword meterPassword = new MeterPassword(false);
            meterPassword.OnValues_Submission += new MeterPassword.GetSubmittedValues(meterPassword_OnValuesSubmission);
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

                touCommands = GetTOUCommands();
                if (touInformation.SetTOU(touCommands))
                    this.StatusMessage = "TOU set successfully!";
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                FinalizeProgrammingValues();
            }
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
                    touCommand +=ProgrammingCommon.GetASCIIValue(row.Cells["Start Hour"].Value.ToString());
                    touCommand +=ProgrammingCommon.GetASCIIValue(row.Cells["Start Minute"].Value.ToString());
                    if (row.Cells["Rate"].Value.ToString() != "00")
                        slots++;
                    touCommand += ProgrammingCommon.GetASCIIValue(row.Cells["Rate"].Value.ToString().Replace('T', '0'));
                }
                dayTable = ProgrammingCommon.GetASCIIValue((holidayIndex % 6 + 1 % 7).ToString("d2"));
                //holidayActivationDate = ProgrammingCommon.GetASCIIValue(dtPickerCollection[holidayIndex].Value.Date.ToShortDateString().Replace(ConfigInfo.DateFormat().Substring(2, 1), "")); 
                ////
                //holidayActivationDate = DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString();
                holidayActivationDate = ProgrammingCommon.GetASCIIValue(DateUtility.DateTimeToLong(dtPickerCollection[holidayIndex].Value).ToString().Substring(2,6));
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
                DateTime dateTime = ProgrammingCommon.GetDate(row.Cells["SeasonActivationDate"].Value.ToString(),true);
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

            //foreach (DataGridView gridTOU in gridHoliday)
            //    if (!CheckTOUSlots(gridTOU))
            //        return false;

            if (!CheckActivationDate())
            {
                this.StatusMessage = string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                return false;
            }

            return true;
        }

        private bool CheckTOUSlots(DataGridView gridTOU)
        {
            if (gridTOU.Rows[0].Cells[1].Value.ToString() == "00")
            {
                this.StatusMessage = "Season slots not complete!";
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                return false;
            }
            if (!isValidTOU)
            {
                this.StatusMessage = "Invalid Entry!";
                Application.DoEvents();
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
                            //do
                            //{
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[1].ReadOnly = true;
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[2].ReadOnly = true;
                            //    gridTOU.Rows[grdRowIndex + 1].Cells[3].ReadOnly = true;
                            //    grdRowIndex++;
                            //} while (grdRowIndex != 9);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckActivationDate()
        {
            DateTime prevDate = new DateTime();
            DateTime currentDate = new DateTime();
            DateTime futureActivationDate = DateTime.Now.AddDays(1).Date;
            TimeSpan ts = futureActivationDate.Date.Subtract(dtPickerFutureActivationDate.Value.Date);

            if (ts.Days > 0)
            {
                this.StatusMessage = string.Concat("Future TOU activation date should be greater than: ", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                return false;
            }

            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                //DateTime dt;
                if(row.Index ==0)
                {
                    prevDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(),false);
                }
                else
                {
                    currentDate = ProgrammingCommon.GetDate(row.Cells[0].Value.ToString(),true); 
                    if (prevDate.Day == currentDate.Day && prevDate.Month == currentDate.Month)
                    {
                        this.StatusMessage = "Season activation dates should be unique";
                        return false;
                    }
                    prevDate=currentDate.Date;
                }
                if (prevDate.Day == 29 && prevDate.Month == 2)
                {
                    this.StatusMessage = "  29 Feb cannot be selected as a Season Activation Date";
                    return false;
                }

                //if (prevDate < dtPickerFutureActivationDate.Value.Date)
                //{
                //    this.StatusMessage = "Season Activation date cannot be less than the Future TOU Activation Date";
                //    return false;
                //}
            }
            return true;
        }

        private void gridActivation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (!ValidateTOUData())
            {
                this.Cursor = Cursors.Default;
                return;
            }
            if (CreateTOUFile())
                MessageBox.Show("File created sucessfully at " + ConfigInfo.GetTOULocation() + @"\" + touFileName.Trim() +  ".TOU", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (Exception ex)
            {
                this.StatusMessage = ex.Message;
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
            fileContent += "<FSAD>" + CreateTOUString(touFileContent[touIndex]) + "</FSAD>" + "\r\n";
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
            string fileName = string.Empty;
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

        private void MeterDataProgramming_Load(object sender, EventArgs e)
        {
            rdAll.Checked = true;
            rdBtnAutoRTC.Checked = true;
            rdBtnManualRTC.Checked = false;
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
            dtPickerRTC.Format =  DateTimePickerFormat.Custom;
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
            foreach(DataGridView holidayGrid in holidayGrids)
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

                if (  Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "23" &&  Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "45")
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
                                    dataGrid.Rows[grdRowIndex+1].Cells[1].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex+1].Cells[2].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex+1].Cells[3].ReadOnly = true;
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
                    if ( Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if ( Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) ==  Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[3].Value))
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
                        string val=Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value);
                         string val1=Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[1].Value);
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
                            if (Convert.ToInt16( dataGrid.Rows[rIndex].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                            {
                               dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                               dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                    if (Convert.ToInt16( dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToInt16( dataGrid.Rows[rIndex + 1].Cells[2].Value) && (Convert.ToInt16( dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToInt16( dataGrid.Rows[rIndex + 1].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 0)
                {
                    if (Convert.ToInt16( dataGrid.Rows[rIndex - 1].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) && (Convert.ToInt16( dataGrid.Rows[rIndex - 1].Cells[3].Value) == Convert.ToInt16( dataGrid.Rows[rIndex].Cells[3].Value)))
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
            catch
            {
                dtView.Rows[e.RowIndex].ErrorText = "Invalid Value ";
                e.Cancel = true;
                isValidTOU = false;
                return false;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTOUFile();
        }

        private void SaveTOUFile()
        {
            string touFilePath = string.Empty;

            try
            {
                if (isValidTOU == true)
                {
                    string fileContent = CreateTOUCommand();
                    if (fileContent != "")
                    {
                        if (!ValidateTOUData())
                        {
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        touFilePath = ConfigInfo.GetTOULocation();
                        if (!Directory.Exists(touFilePath))
                            Directory.CreateDirectory(touFilePath);
                        FileStream fs = new FileStream(string.Concat(touFilePath, "/TouConfig.TOU"), FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(fileContent);
                        sw.Close();
                        fs.Close();
                        this.StatusMessage = "Data Saved Successfully";
                    }
                    else
                    {
                        this.StatusMessage = "No data available for saving ";
                    }
                }
                else
                {
                    foreach (DataGridView gridSeason in GetSeasonGridCollection())
                    {
                        if (gridSeason.Rows[0].Cells[1].Value.ToString() == "00")
                        {
                            this.StatusMessage = "Season slots not complete!";// "No data available for saving.";
                            return;
                        }
                    }

                    this.StatusMessage = "Invalid Entry!";// "No data available for saving.";
                    return;


                    //foreach (DataGridView gridHoliday in GetHolidayGridCollection())
                    //{
                    //    if (gridHoliday.Rows[0].Cells[1].Value.ToString() == "00")
                    //    {
                    //        this.StatusMessage = "No data available for saving.";
                    //        return;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                this.StatusMessage = ex.Message;
                return;
            }
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
            int rowCount=0;
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
            int rIndex=0;
            int startDay = 1;
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string sDay = startDay.ToString();
                if (sDay.Length < 2) { sDay = "0" + sDay; }
                DateTime dt = new DateTime(DateTime.Now.Year, 1, Int32.Parse( sDay));
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

        private void rtcTimer_Tick(object sender, EventArgs e)
        {
            dtPickerRTC.Value = DateTime.Now;
        }

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
        }


        /////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////
        //private void GridCellClick(DataGridView dataGrid)
        //{
        //    try
        //    {
        //        Application.DoEvents();
        //        Application.DoEvents();
        //        Application.DoEvents();
        //        int rIndex = 0;
        //        int count = 0;
        //        int rcount = 0;
        //        this.StatusMessage = "";
        //        int gIndex = 0;

        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount != 0 && dataGrid.Rows[rcount - 1].Cells[1].Value.ToString() != "00")
        //        {
        //            if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "45")
        //            {
        //                for (count = rcount; count <= 9; count++)
        //                {
        //                    dataGrid.Rows[count].ReadOnly = true;
        //                }
        //                this.StatusMessage = "No more enteries allowed as the day is complete";
        //                isValidTOU = true;
        //                return;
        //            }
        //        }

        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        for (rcount = 0; rcount <= 8; rcount++)
        //        {
        //            if ((dataGrid.Rows[rcount].Cells[2].Value.ToString() != "00") && (dataGrid.Rows[rcount].Cells[3].Value.ToString() != "00") && (dataGrid.Rows[rcount + 1].Cells[2].Value.ToString() != "00") && (dataGrid.Rows[rcount + 1].Cells[3].Value.ToString() != "00"))
        //            {
        //                if ((dataGrid.Rows[rcount].Cells[2].Value.ToString() == dataGrid.Rows[rcount + 1].Cells[2].Value.ToString()) && (Convert.ToInt16(dataGrid.Rows[rcount].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[rcount + 1].Cells[3].Value)))
        //                {
        //                    while (rcount < 8)
        //                    {
        //                        dataGrid.Rows[rcount + 2].ReadOnly = true;
        //                        rcount++;
        //                    }
        //                    isValidTOU = false;
        //                    return;
        //                }
        //            }
        //        }


        //        rcount = dataGrid.CurrentCell.RowIndex;  //////##########################
        //        if (rcount == 1)
        //        {
        //            if (dataGrid.Rows[rcount - 1].Cells[1].Value.ToString() == "00" && dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "00" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "00")
        //            {
        //                while (rcount <= 9)
        //                {
        //                    dataGrid.Rows[rcount].ReadOnly = true;
        //                    rcount++;
        //                }
        //                return;
        //            }
        //            else
        //            {
        //                while (rIndex <= 9)
        //                {
        //                    dataGrid.Rows[rIndex].ReadOnly = false;
        //                    rIndex++;
        //                }
        //            }
        //        }

        //        else if (rcount > 1)
        //        {
        //            if (dataGrid.Rows[rcount - 1].Cells[1].Value.ToString() == "00")
        //            {
        //                if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "00" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "00")
        //                {
        //                    while (rcount <= 9)
        //                    {
        //                        dataGrid.Rows[rcount].ReadOnly = true;
        //                        rcount++;
        //                    }
        //                    return;
        //                }
        //                else
        //                {
        //                    while (rIndex <= 9)
        //                    {
        //                        dataGrid.Rows[rIndex].ReadOnly = false;
        //                        rIndex++;
        //                    }
        //                }
        //            }
        //        }
        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount >= 3 && dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == dataGrid.Rows[rcount - 2].Cells[2].Value.ToString() && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == dataGrid.Rows[rcount - 2].Cells[3].Value.ToString())
        //        {
        //            while (rcount <= 9)
        //            {
        //                dataGrid.Rows[rcount].ReadOnly = true;
        //                rcount++;
        //            }
        //            return;
        //        }
        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount != 0 && dataGrid.Rows[rcount].Cells[1].Value.ToString() == "00")
        //        {
        //            for (count = 2; count <= 3; count++)
        //            {
        //                dataGrid.Rows[rcount].Cells[count].Value = "00";
        //                dataGrid.Rows[rcount].Cells[count].ReadOnly = true;
        //            }
        //            count = 0;
        //            //while (dataGrid.Name != dataGrid.Name)
        //            //{
        //            //    count++;
        //            //}
        //            //for (rIndex = 0; rIndex <= 9; rIndex++)
        //            //{
        //            //    if (dataGrid.Rows[rIndex].Cells[1].Value.ToString() != "00" && (dataGrid.Rows[rIndex].Cells[2].Value.ToString() == "00" || dataGrid.Rows[rIndex].Cells[3].Value.ToString() == "00"))
        //            //    {
        //            //        isValidTOU = false;
        //            //        return;
        //            //    }
        //            //    else
        //            //    {
        //            //        isValidTOU = true;
        //            //    }
        //            //}
        //            rIndex = rcount + 1;
        //            return;
        //        }
        //        else
        //        {
        //            for (count = 2; count <= 3; count++)
        //            {
        //                dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
        //            }
        //            rIndex = rcount + 1;
        //            while (rIndex <= 5)
        //            {
        //                dataGrid.Rows[rIndex].ReadOnly = false;
        //                rIndex++;
        //            }
        //        }

        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount != 0 && dataGrid.Rows[rcount].Cells[1].Value.ToString() != "00" && dataGrid.Rows[rcount].Cells[2].Value.ToString() == "00")
        //        {
        //            if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "00" && Convert.ToInt16(dataGrid.Rows[rcount - 1].Cells[3].Value) < 45)
        //            {
        //                dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
        //                rIndex = rcount + 1;
        //                while (rIndex <= 9)
        //                {
        //                    dataGrid.Rows[rIndex].ReadOnly = false;
        //                    rIndex++;
        //                }
        //                isValidTOU = true;
        //            }
        //            else
        //            {
        //                dataGrid.Rows[rcount].Cells[3].ReadOnly = true;
        //                rIndex = rcount + 1;
        //                while (rIndex <= 9)
        //                {
        //                    dataGrid.Rows[rIndex].ReadOnly = true;
        //                    rIndex++;
        //                }
        //                isValidTOU = false;
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
        //            rIndex = rcount + 1;
        //            while (rIndex <= 9)
        //            {
        //                dataGrid.Rows[rIndex].ReadOnly = false;
        //                rIndex++;
        //            }
        //            isValidTOU = true;
        //        }

        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount != 0 && dataGrid.Rows[rcount].Cells[1].Value.ToString() != "00" && (dataGrid.Rows[rcount].Cells[2].Value.ToString() == "00" && dataGrid.Rows[rcount].Cells[3].Value.ToString() == "00"))
        //        {
        //            rIndex = rcount + 1;
        //            while (rIndex <= 9)
        //            {
        //                dataGrid.Rows[rIndex].ReadOnly = true;
        //                rIndex++;
        //            }
        //            //isValidTOU = false;
        //            return;
        //        }
        //        else
        //        {
        //            rIndex = rcount + 1;
        //            while (rIndex <= 9)
        //            {
        //                dataGrid.Rows[rIndex].ReadOnly = false;
        //                rIndex++;
        //            }
        //        }
        //        rcount = dataGrid.CurrentCell.RowIndex;
        //        if (rcount != 0)
        //        {
        //            if (dataGrid.Rows[rcount].Cells[1].Value.ToString() == "00" && dataGrid.Rows[rcount].Cells[2].Value.ToString() == "00" && dataGrid.Rows[rcount].Cells[3].Value.ToString() == "00")
        //            {
        //                isValidTOU = true;
        //            }
        //        }
        //        for (rIndex = 1; rIndex <= 9; rIndex++)
        //        {
        //            if (dataGrid.Rows[rIndex].Cells[1].Value.ToString() != "00" && (dataGrid.Rows[rIndex].Cells[2].Value.ToString() == "00" || dataGrid.Rows[rIndex].Cells[3].Value.ToString() == "00"))
        //            {
        //                isValidTOU = false;
        //                return;
        //            }
        //            else
        //            {
        //                isValidTOU = true;
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        MessageBox.Show("BCS Error", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        isValidTOU = false;
        //        //ResetTOU();
        //    }
        //    finally
        //    {
        //        dataGrid.Rows[0].Cells[2].ReadOnly = true;
        //        dataGrid.Rows[0].Cells[3].ReadOnly = true;
        //        dataGrid.Columns[0].ReadOnly = true;
        //    }

        //}


        //private void InvalidData()
        //{
        //    isValidTOU = false;
        //    this.StatusMessage = "Invalid Entry!";
        //    Application.DoEvents();
        //}


        //private bool validateGridCell(DataGridView dtView, object sender, DataGridViewCellValidatingEventArgs e)
        //{  
        //    int count = 0;
        //    int rcount = 0;
        //    int rIndex = 0;
        //    try
        //    {
        //        if (dtView.CurrentCell.IsInEditMode == true)
        //        {

        //            if (e.ColumnIndex == 1)
        //            {
        //                if (e.FormattedValue == null && (e.FormattedValue.ToString() != "T1" || e.FormattedValue.ToString() != "T2" || e.FormattedValue.ToString() != "T3" || e.FormattedValue.ToString() != "T4" || e.FormattedValue.ToString() != "T5" || e.FormattedValue.ToString() != "T6" || e.FormattedValue.ToString() != "T7" || e.FormattedValue.ToString() != "T8"))
        //                {
        //                    e.Cancel = true;
        //                    InvalidData();
        //                    return false;
        //                }
        //                else { isValidTOU = true; }
        //                if (e.FormattedValue.ToString() != "")
        //                {
        //                    if (e.RowIndex == 0)
        //                    {
        //                        dtView.Rows[e.RowIndex].Cells[2].Value = "00";
        //                        dtView.Rows[e.RowIndex].Cells[3].Value = "00";
        //                    }
        //                }
        //                rcount = dtView.CurrentCell.RowIndex;
        //                if (rcount > 0)
        //                {
        //                    if (dtView.Rows[rcount].Cells[1].Value.ToString() == "00" && (dtView.Rows[rcount].Cells[2].Value.ToString() == "00" || dtView.Rows[rcount].Cells[3].Value.ToString() == "00"))
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = true;
        //                            rIndex++;
        //                        }
        //                        //return false;
        //                    }
        //                    else
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = false;
        //                            rIndex++;
        //                        }
        //                    }
        //                }

        //                rcount = dtView.CurrentCell.RowIndex;
        //                if (rcount == 1)
        //                {
        //                    if (dtView.Rows[rcount].Cells[1].Value.ToString() != "00" && (dtView.Rows[rcount].Cells[2].Value.ToString() == "00" && dtView.Rows[rcount].Cells[3].Value.ToString() == "00"))
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = true;
        //                            rIndex++;
        //                        }
        //                        isValidTOU = false;
        //                        return false;
        //                    }
        //                    else
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = false;
        //                            rIndex++;
        //                        }
        //                        isValidTOU = true;
        //                    }
        //                }
        //                else if (rcount > 1)
        //                {
        //                    if ((dtView.Rows[rcount].Cells[2].Value.ToString() == "00" && dtView.Rows[rcount].Cells[3].Value.ToString() == "00"))
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = true;
        //                            rIndex++;
        //                        }
        //                        isValidTOU = false;
        //                        return false;
        //                    }
        //                    else
        //                    {
        //                        rIndex = rcount + 1;
        //                        while (rIndex <= 9)
        //                        {
        //                            dtView.Rows[rIndex].ReadOnly = false;
        //                            rIndex++;
        //                        }
        //                        isValidTOU = true;
        //                    }
        //                }
        //            }
        //            if (e.ColumnIndex == 2)
        //            {
        //                if (e.RowIndex == 0)
        //                {
        //                    if (e.FormattedValue.ToString() != "00")
        //                    {
        //                        e.Cancel = true;
        //                        InvalidData();
        //                    }
        //                }
        //                else { isValidTOU = true; }
        //                if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 23)
        //                {
        //                    e.Cancel = true;
        //                    InvalidData();
        //                }
        //                else { isValidTOU = true; }
        //                if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString() != "00")
        //                {
        //                    if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
        //                    {
        //                        e.Cancel = true;
        //                        InvalidData();
        //                        return false;
        //                    }
        //                    else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
        //                    {
        //                        if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
        //                        {
        //                            for (count = e.RowIndex + 2; count <= 9; count++)
        //                            {
        //                                dtView.Rows[count].ReadOnly = true;
        //                            }
        //                            isValidTOU = false;
        //                        }
        //                        else { isValidTOU = true; }
        //                    }
        //                }
        //                if (e.RowIndex != 0 && e.RowIndex != 1)
        //                {
        //                    if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)
        //                    {
        //                        if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
        //                        {
        //                            e.Cancel = true;
        //                            InvalidData();
        //                            return false;
        //                        }

        //                        else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
        //                        {
        //                            if (Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "45")
        //                            {
        //                                e.Cancel = true;
        //                                InvalidData();
        //                                return false;
        //                            }
        //                            else if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
        //                            {
        //                                for (count = e.RowIndex + 1; count <= 9; count++)
        //                                {
        //                                    dtView.Rows[count].ReadOnly = true;
        //                                }
        //                                isValidTOU = false;
        //                                return false;
        //                            }
        //                            else { isValidTOU = true; }
        //                        }
        //                    }
        //                }
        //                rcount = dtView.CurrentCell.RowIndex;
        //                if (dtView.Rows[rcount].Cells[1].Value.ToString() != "00" && (dtView.Rows[rcount].Cells[2].Value.ToString() == "00" || dtView.Rows[rcount].Cells[3].Value.ToString() == "00"))
        //                {
        //                    rIndex = rcount + 1;
        //                    while (rIndex <= 9)
        //                    {
        //                        dtView.Rows[rIndex].ReadOnly = true;
        //                        rIndex++;
        //                    }
        //                    isValidTOU = false;
        //                    return false;
        //                }
        //                else
        //                {
        //                    rIndex = rcount + 1;
        //                    while (rIndex <= 9)
        //                    {
        //                        dtView.Rows[rIndex].ReadOnly = false;
        //                        rIndex++;
        //                    }
        //                }
        //            }
        //            if (e.ColumnIndex == 3)
        //            {
        //                if (e.RowIndex == 0)
        //                {
        //                    if (e.FormattedValue.ToString() != "00")
        //                    {
        //                        e.Cancel = true;
        //                        InvalidData();
        //                        return false;
        //                    }
        //                }
        //                else { isValidTOU = true; }
        //                if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 45)
        //                {
        //                    e.Cancel = true;
        //                    InvalidData();
        //                    return false;
        //                }
        //                else { isValidTOU = true; }
        //                if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[1].Value.ToString() != "00" && dtView.Rows[e.RowIndex + 1].Cells[2].Value.ToString() != "00")
        //                {
        //                    if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
        //                    {
        //                        if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
        //                        {
        //                            e.Cancel = true;
        //                            InvalidData();
        //                            return false;
        //                        }
        //                        else { isValidTOU = true; }
        //                    }
        //                }
        //                if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
        //                {
        //                    if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
        //                    {
        //                        e.Cancel = true;
        //                        InvalidData();
        //                        return false;
        //                    }
        //                    else { isValidTOU = true; }
        //                }
        //            }
        //        }

        //        return true;
        //    }

        //    catch
        //    {
        //        dtView.Rows[e.RowIndex].ErrorText = "Invalid Value ";
        //        e.Cancel = true;
        //        InvalidData();
        //        return false;
        //    }
        //}

    }
}