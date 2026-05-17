using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.IECChannel;
using CAB.IECChannel.ReadOut;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using CAB.MeterData.Upload;
using CAB.Parser;
using CAB.UI;
using CAB.UI.Controls;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using IEC.CAB.CHANNEL.Programming;
using CAB.Contracts;

namespace CABApplication
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
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
        private int isMeterType = 0; // Story - 347720 - To set the variable based on meter type is single phase DLMS
        public List<System.Enum> enumData;
        List<System.Enum> selectedProfiles;
        private Communication communication;
        private delegate void IntMeterId(int simIndex, string meterID); // Story - 0427028 - Meter ID not getting updated in GSM grid

        //Story - 354414 - Thread Implementation - Declaration needed
        Thread readThread;
        private ProfileId currentProfile = 0;
        private delegate void SetCursorCallBack(Cursor cursor);
        private delegate void SetDataGridStyleCallback(System.Enum profileId, DataGridViewCellStyle style);
        private delegate void SetDataGridStatusCallback(System.Enum profileId, string style);
        private bool isMeterReading = false;
        // private CommunicationType commType;
        private MeterMasterBLL meterMasterBLL = null;
        private Serial objSerialComm = null;
        private static string simNumber = string.Empty;

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterDataReadoutForm).ToString());
        public MeterDataReadoutForm()
        {
            InitializeComponent();
            command = Command.GetInstance();

            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as IECLocalCommunication;
                GsmCommPanel.Visible = false;
            }
            else
            {
                //communications = ChannelManager.GetChannel() as GSMCommunication;
                communications = ChannelManager.GetChannel() as IECLocalCommunication; // Story - 365867 - same class is used for both type of communication
                FillMeterIdSerialNumber();
                GsmCommPanel.Visible = true;
            }

        }
        //ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
        private void FillMeterIdSerialNumber()
        {
            noMeterFoundStatus.Visible = false;
            meterMasterBLL = new MeterMasterBLL();
            DataSet dsMeterIdSimNumber = meterMasterBLL.GetMeterIdAndSimNumber(ConfigSettings.GetValue("ChannelType"));
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
                //if (commType == CommunicationType.GPRS)
                //{
                //    dgvSimNoColumn.HeaderText = "IMEI Number";
                //}
                //else
                //{
                dgvSimNoColumn.HeaderText = "Sim Number";
                //}
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
                    dgvMeterIdAndSim.Rows[count].Cells["Status"].Value = "Communication Not Started";
                    dgvMeterIdAndSim.Rows[count].Cells["Select"].Value = false;
                }


                //dgvMeterIdAndSim.Columns["Meter Id"].ReadOnly = true;
                //dgvMeterIdAndSim.Columns["Status"].ReadOnly = true;


                //dgvMeterIdAndSim.Columns["Meter Id"].Width = dgvMeterIdAndSim.Columns["Meter Id"].Width + 20;
                //dgvMeterIdAndSim.Columns["Select"].Width = dgvMeterIdAndSim.Columns["Select"].Width - 40;


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

        private void lngGridViewReadControl1_Load(object sender, EventArgs e)
        {
            lngGridViewReadControl1.SetDefaultCellStyle(true);
            enumData = new List<System.Enum>();
            //enumData.Add(ProfileId.GeneralBilling);
            enumData.Add(ProfileId.Tamper);
            enumData.Add(ProfileId.LoadSurvey);
            enumData.Add(ProfileId.Phasor);
            enumData.Add(ProfileId.FraudEnergy);
            enumData.Add(ProfileId.Midnight);
            if (isMeterType == 1 || isMeterType == 2) // Story - 349654 - Remove Phasor and Fraud enrgy for single phase non dlms baud rate 300 and 9600 meters
            {
                enumData.Remove(ProfileId.Phasor);
                enumData.Remove(ProfileId.FraudEnergy);
            }
            enumData.Add(ProfileId.MeterConfiguration);
            lngGridViewReadControl1.AddEnumList(enumData, true);
            //lngGridViewReadControl1.SetDefaultRedaoutProfile(new List<System.Enum>() { ProfileId.GeneralBilling });
            btnAbort.Enabled = false;
        }

        /// <summary>
        ///  Used to create profileId enums based on profiles
        /// that needs to be read(Selected by user through checkboxes)
        /// </summary>
        /// <returns></returns>
        private List<System.Enum> GetSelectedProfilesToRead(int isSinglePhaseNonDLMS)
        {
            List<System.Enum> selectedProfiles = new List<System.Enum>();
            selectedProfiles.Clear();
            selectedProfiles.Add(ProfileId.NamePlate);
            selectedProfiles.AddRange(lngGridViewReadControl1.GetSelectedProfilesList<System.Enum>(enumData));
            if (selectedProfiles.Contains(ProfileId.Tamper))
            {
                if (isSinglePhaseNonDLMS == 0 || isSinglePhaseNonDLMS == 3)
                    selectedProfiles.Add(ProfileId.Transaction);
            }
            return selectedProfiles;
        }

        private void ButtonStatus()
        {
            EnableReadControls(); //Story - 354414 - Thread Implementation - All these controls are accessible from cross thread operation
            //btnRead.Enabled = true;
            //btnCancel.Enabled = true;
            //btnAbort.Enabled = false;
            //this.Cursor = Cursors.Default;
            SetCursor(Cursors.Default);
            this.RightStatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
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
            if (readMtrConfig)
            {
                //this.StatusMessage = "User Aborted.";
                this.StatusMessageAsync = "User Aborted.";
                Application.DoEvents();
            }
            lngGridViewReadControl1.Enabled = flag;
            readMtrConfig = false;
            DataAcquisition.Enabled = true;
            Configuration.Enabled = true;

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

            //string readingDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {
                foreach (System.Enum profile in enumData)
                {
                    // Story - 354414 - Thread Implementation - All the above properties can be set in the comman method
                    SetGridRowAttributes(System.Drawing.Color.White, profile, "Readout Not Started...");
                }
                //if (!ConfigInfo.IsGSMConnected())
                //{
                communications = ChannelManager.GetChannel() as IECLocalCommunication;
                if (IsMeterType == 2)
                {
                    communications.Parity = System.IO.Ports.Parity.None;
                    communications.DataBits = 8;
                }
                else if (IsMeterType == 1 && IsMeterType == 3)
                {
                    communications.Parity = System.IO.Ports.Parity.Even;
                    communications.DataBits = 7;
                }

                //if ((ConfigSettings.GetValue("ChannelType") != CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
                //{
                //    if (oneToOneGSM.Checked)
                //    {
                //        if (!ValidateSimNumber())
                //        {
                //            return;
                //        }
                //        communications.SimNumber = "0" + txtBoxMeterSIM.Text.Trim();
                //    }
                //    else if (oneToManyGSM.Checked)
                //    {
                //        ReadOneToMany(strFileName);
                //    }
                //}
                // }
                // else
                // {
                //     communications = ChannelManager.GetChannel() as GSMCommunication;
                // }


                lngGridViewReadControl1.Enabled = false;
                setConnectionDetail(true);
                EnbaleControls(false);
                IsAborted = false;
                this.StatusMessage = string.Empty;
                this.StatusMessageAsync = string.Empty;
                Application.DoEvents();

                if (!versionFlag)
                {
                    this.StatusMessageAsync = "Invalid Firmware version.";
                    EnableStopTimer(); //Story - 354414 - Thread Implementation - All these controls are accessible from cross thread operation
                    setConnectionDetail(false);
                    Application.DoEvents();
                    EnableReadControls(); //Story - 354414 - Thread Implementation - All these controls are accessible from cross thread operation
                    return;
                }
                if (chkLoadSurvey.Checked)
                {
                    if (ConfigSettings.GetValue("LoadSurveyDays") == "") // Story - 349654 - Load Survey day value should come from settings page
                    {
                        //this.StatusMessage = "Please select the no. of days for Load Survey Readout";
                        this.StatusMessageAsync = "Please select the no. of days for Load Survey Readout";
                        ButtonStatus();
                        Application.DoEvents();
                        if (TotalCheck() == 1)
                        {
                            //EnbaleControls(true);
                            EnableReadControls(); //Story - 354414 - Thread Implementation - All these controls are accessible from cross thread operation
                            return;
                        }
                    }
                }
                IsAborted = false;
                btnAbort.Enabled = true;
                btnRead.Enabled = false;
                btnCancel.Enabled = false;
                DataAcquisition.Enabled = false;
                Configuration.Enabled = false;
                //Story - 354414 - Thread Implementation
                readThread = new Thread(ReadMeter);
                //making the thread background as it communication needs to be stoppped when BCS closes.
                readThread.IsBackground = true;
                readThread.Start(SynchronizationContext.Current);

            }
            catch (Exception ex)
            {
                this.RightStatusMessage = string.Empty;
                this.StatusMessage = string.Empty;
                this.RightStatusMessageAsync = string.Empty;
                this.StatusMessageAsync = string.Empty;
                Application.DoEvents();
                //this.Cursor = Cursors.Default;
                SetCursor(Cursors.Default);//Story - 354414 - Thread Implementation - All these controls are accessible from cross thread operation
                //EnbaleControls(true);
                EnableReadControls();
                //StopProgressBarTimer();
                EnableStopTimer();
                setConnectionDetail(false);
                logger.Log(LOGLEVELS.Error, "Error while Reading IEC Ruby Meter.", ex);
            }
            // Story - 349654 - To disable the progress bar after get any error or exception with in the inner functions
            finally
            {
                Application.DoEvents();

            }
        }

        /// <summary>
        /// Validate SIM Number 
        /// </summary>
        /// <returns></returns>
        private bool ValidateSimNumber()
        {
            bool flag = true;
            long simNumber = 0;
            if (txtBoxMeterSIM.Text.Trim().Length == 0)
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            else if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return false;
            }
            //if (commType == CommunicationType.GPRS)
            //{
            //    if (txtBoxMeterSIM.Text.Trim().Length != 15)
            //    {
            //        CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        txtBoxMeterSIM.Focus();
            //        return false;
            //    }
            //}
            else
            {
                if (txtBoxMeterSIM.Text.Trim().Length != 10)
                {
                    CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    return false;
                }
            }
            return flag;

        }
        /// <summary>
        /// Used to get Load su
        /// </summary>
        private void GetLoadSurveyDaysAndMDInterval()
        {
            if (!ConfigInfo.IsGSMConnected())
            {
                communications = ChannelManager.GetChannel() as IECLocalCommunication;
            }
            else
            {
                //communications = ChannelManager.GetChannel() as GSMCommunication;
                communications = ChannelManager.GetChannel() as IECLocalCommunication;// Story - 365867 - same class is used for both type of communication
            }
            if (communications == null)
            {
                //this.Cursor = Cursors.Default;
                SetCursor(Cursors.Default); // Story - 354414 - Thread Implementation
                return;
            }
            totalDay = 0;
            //Story - 354414 - Thread Implementation - Will work in thread, so would not be accessible directly
            //btnCancel.Enabled = false;
            DisableCancel();
            //this.Cursor = Cursors.WaitCursor;
            SetCursor(Cursors.WaitCursor);
            Application.DoEvents();
            ReadoutDTMLoadSurvey readoutDTMLoadSurvey = new ReadoutDTMLoadSurvey();
            readoutDTMLoadSurvey.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
            readoutDTMLoadSurvey.Channel = communications;
            string data = readoutDTMLoadSurvey.LoadDTMDay();
            if (readoutDTMLoadSurvey.IsSignOnFailure)
            {
                //this.StatusMessage = SIGNONFAILURE;
                this.StatusMessageAsync = SIGNONFAILURE;
                Application.DoEvents();
                ButtonStatus();
                //this.Cursor = Cursors.Default;
                SetCursor(Cursors.Default); //Story - 354414 - Thread Implementation
                return;
            }
            if (string.IsNullOrEmpty(data))
            {
                //this.StatusMessage = "Invalid response from meter.";
                this.StatusMessageAsync = "Invalid response from meter.";
                Application.DoEvents();
                //this.Cursor = Cursors.Default;
                SetCursor(Cursors.Default); //Story - 354414 - Thread Implementation
                return;
            }
            if (data.Length >= 5)
            {
                string noofdays = data.Substring(21, 2);
                totalDay = Convert.ToInt32(noofdays, 16);

            }
            else
                data = string.Empty;

        }

        private void ChangeStatus(string data, bool SignOn)
        {
            this.RightStatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
            if (!SignOn)
            {
                if (data.Trim() != string.Empty)
                {
                    //this.StatusMessage = MessageConstant.GetText("M000068");
                    this.StatusMessageAsync = MessageConstant.GetText("M000068");
                }
                else
                {
                    this.StatusMessage = string.Empty;
                    this.StatusMessageAsync = string.Empty;
                }
            }
            else
            {
                //this.StatusMessage = MessageConstant.GetText("M000083");
                this.StatusMessageAsync = MessageConstant.GetText("M000083");
            }
        }
        private void ChangeStatus(string data)
        {
            this.RightStatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;

            if (data.Trim() != string.Empty)
            {
                //this.StatusMessage = MessageConstant.GetText("M000068");
                this.StatusMessageAsync = MessageConstant.GetText("M000068");
            }
            else
            {
                this.StatusMessage = string.Empty;
                this.StatusMessageAsync = string.Empty;
            }
        }
        /// <summary>
        /// Originaly written 
        /// This method signature is changed, to pass the meter ID in this function
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="meterId"></param>
        public void SaveData(string fileText, string meterId)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
            // Story - 349654 - To get the meter id and append in the file name
            meterId = meterId.Substring(meterId.IndexOf("/") + 1);
            meterId = meterId.Substring(4, meterId.Length - 4); // Story - 349654 - In this case buffer for meter id is dynamic not fix 16 althaugh meter id can be upto 16 digits
            if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId.Trim() + "_" + fileName;
            bool Flag = false;
        ReConf:
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessageAsync = "File name can't be empty.";
                        this.RightStatusMessageAsync = "";
                        //this.StatusMessage = "File name can't be empty.";
                        //this.RightStatusMessage = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessageAsync = string.Empty;
                    this.StatusMessage = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");
                //this.StatusMessage = MessageConstant.GetText("M000047");
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
                //this.StatusMessage = MessageConstant.GetText("M000048");
                this.StatusMessageAsync = MessageConstant.GetText("M000048");
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, "BCS");
                logger.Log(LOGLEVELS.Error, "SaveData(string fileText, string meterId)", Ex);
            }

            bool IsUploaded = false;
            UploadFile uploadFile = new UploadFile();
            //this.StatusMessage = "Uploading readout file..";
            this.StatusMessageAsync = "Uploading readout file...";
            Application.DoEvents();
            //btnAbort.Enabled = false;
            DisableAbort(); //Story - 354414 - Thread Implementation
            string resultMessage = string.Empty;
            SetCursor(Cursors.WaitCursor);
            //this.Cursor = Cursors.WaitCursor;
            ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
            IsUploaded = uploadFile.UploadCABFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null);
            if (IsUploaded)
            {
                this.ListRefresh = true;
                //this.RightStatusMessage = String.Empty;
                //this.StatusMessage = "File Uploaded successfully.";
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = "File Uploaded successfully.";
            }
            else
            {
                //this.RightStatusMessage = String.Empty;
                //this.StatusMessage = resultMessage;
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = resultMessage;
            }
            //this.Cursor = Cursors.WaitCursor;
            SetCursor(Cursors.WaitCursor); //Story - 354414 - Thread Implementation

        }

        public void SaveDataForSPhaseIEC(string fileText, string meterId)
        {
            string fileName = ReadoutCommon.GetFileName().Trim();
            // Story - 349654 - To get the meter id and append in the file name
            meterId = meterId.Substring(meterId.IndexOf("/") + 1);
            meterId = meterId.Substring(13, 16);
            if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId.Trim() + "_" + fileName;
            bool Flag = false;
        ReConf:
            do
            {
            AMT:
                if (ReadoutCommon.ReadoutMessageBox(ref fileName, DialogType.Common) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        this.StatusMessageAsync = "File name can't be empty.";
                        this.RightStatusMessageAsync = "";
                        //this.StatusMessage = "File name can't be empty.";
                        //this.RightStatusMessage = "";
                        Application.DoEvents();
                        goto AMT;
                    }
                    if (ReadoutCommon.ValidFileName(fileName))
                        Flag = true;
                }
                else
                {
                    this.StatusMessageAsync = string.Empty;
                    this.StatusMessage = string.Empty;
                    return;
                }
            } while (!Flag);
            if (fileName.Trim().Equals(string.Empty) || Flag == false)
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");
                //this.StatusMessage = MessageConstant.GetText("M000047");
                return;
            }
            //tmpMtrID = readOutsList[fileNumber].Substring(readOutsList[fileNumber].IndexOf("/") + 1);
            //tmpMtrID = tmpMtrID.Substring(13, tmpMtrID.IndexOf("/") - 13);
            string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".SLG");
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
                //this.StatusMessage = MessageConstant.GetText("M000048");
                this.StatusMessageAsync = MessageConstant.GetText("M000048");
            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, "BCS");
                logger.Log(LOGLEVELS.Error, "SaveDataForSPhaseIEC(string fileText, string meterId)", Ex);
            }

            bool IsUploaded = false;
            UploadFile uploadFile = new UploadFile();
            //this.StatusMessage = "Uploading readout file..";
            this.StatusMessageAsync = "Uploading readout file...";
            Application.DoEvents();
            //btnAbort.Enabled = false;
            DisableAbort(); //Story - 354414 - Thread Implementation
            string resultMessage = string.Empty;
            //this.Cursor = Cursors.WaitCursor;
            SetCursor(Cursors.WaitCursor);
            ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
            IsUploaded = uploadFile.UploadSLGFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null);
            if (IsUploaded)
            {
                this.ListRefresh = true;
                //this.RightStatusMessage = String.Empty;
                //this.StatusMessage = "File Uploaded successfully.";
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = "File Uploaded successfully.";
            }
            else
            {
                this.RightStatusMessage = String.Empty;
                this.StatusMessage = resultMessage;
                this.RightStatusMessageAsync = String.Empty;
                this.StatusMessageAsync = resultMessage;
            }
            //this.Cursor = Cursors.WaitCursor;
            SetCursor(Cursors.WaitCursor);

        }

        public void SaveRemoteDataForSPhaseIEC(string fileText, string meterId)
        {
            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
            if (fileName.Trim().Equals(string.Empty))
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");

            }

            else
            {
                fileName = ReadoutCommon.GetFileName().Trim();
                // Story - 349654 - To get the meter id and append in the file name
                meterId = meterId.Substring(meterId.IndexOf("/") + 1);
                meterId = meterId.Substring(13, 16);
                //if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId.Trim() + "_" + fileName;

                if (fileName.Trim().Equals(string.Empty))
                {
                    this.StatusMessageAsync = MessageConstant.GetText("M000047");
                    return;
                }
                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".SLG");
                filePath = filePath.Replace("\\\\", "\\");

                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file);
                    fileText = ConfigInfo.EncryptFile(fileText);
                    wr1.Write(fileText);
                    wr1.Close();
                    file.Close();
                    //this.StatusMessage = MessageConstant.GetText("M000048");
                    this.StatusMessageAsync = MessageConstant.GetText("M000048");
                }
                catch (Exception Ex)    //Exception log for catch block
                {
                    MessageBox.Show(Ex.Message, "BCS");
                    logger.Log(LOGLEVELS.Error, "SaveRemoteDataForSPhaseIEC(string fileText, string meterId)", Ex);
                }

                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                //this.StatusMessage = "Uploading readout file..";
                this.StatusMessageAsync = "Uploading readout file...";
                Application.DoEvents();
                //btnAbort.Enabled = false;
                DisableAbort(); //Story - 354414 - Thread Implementation
                string resultMessage = string.Empty;
                //this.Cursor = Cursors.WaitCursor;
                SetCursor(Cursors.WaitCursor);
                ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
                IsUploaded = uploadFile.UploadSLGFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null);
                if (IsUploaded)
                {
                    this.ListRefresh = true;
                    //this.RightStatusMessage = String.Empty;
                    //this.StatusMessage = "File Uploaded successfully.";
                    this.RightStatusMessageAsync = String.Empty;
                    this.StatusMessageAsync = "File Uploaded successfully.";
                }
                else
                {
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = resultMessage;
                    this.RightStatusMessageAsync = String.Empty;
                    this.StatusMessageAsync = resultMessage;
                }
                //this.Cursor = Cursors.WaitCursor;
                SetCursor(Cursors.WaitCursor);
            }
        }
        public void SaveRemoteData(string fileText, string meterId)
        {
            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
            if (fileName.Trim().Equals(string.Empty))
            {
                this.StatusMessageAsync = MessageConstant.GetText("M000047");

            }
            else
            {
                fileName = ReadoutCommon.GetFileName().Trim();
                // Story - 349654 - To get the meter id and append in the file name
                // Story - 427028 - calculation of meter id for CAB file
                //meterId = meterId.Substring(meterId.IndexOf("/") + 1);
                //meterId = meterId.Substring(13, 16);
                meterId = meterId.Substring(meterId.IndexOf("/") + 1);
                meterId = meterId.Substring(4, meterId.Length - 4);
                //if (ConfigInfo.FileNamingConvention().Equals("Default+MeterID"))
                fileName = meterId.Trim() + "_" + fileName;

                if (fileName.Trim().Equals(string.Empty))
                {
                    this.StatusMessageAsync = MessageConstant.GetText("M000047");
                    return;
                }
                string filePath = string.Concat(ConfigInfo.CheckOrCreatePath(), "\\", fileName, ".CAB");
                filePath = filePath.Replace("\\\\", "\\");

                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    StreamWriter wr1 = new StreamWriter(file);
                    fileText = ConfigInfo.EncryptFile(fileText);
                    wr1.Write(fileText);
                    wr1.Close();
                    file.Close();
                    //this.StatusMessage = MessageConstant.GetText("M000048");
                    this.StatusMessageAsync = MessageConstant.GetText("M000048");
                }
                catch (Exception Ex)    //Exception log for catch block
                {
                    MessageBox.Show(Ex.Message, "BCS");
                    logger.Log(LOGLEVELS.Error, "SaveRemoteData(string fileText, string meterId)", Ex);
                }

                bool IsUploaded = false;
                UploadFile uploadFile = new UploadFile();
                //this.StatusMessage = "Uploading readout file..";
                this.StatusMessageAsync = "Uploading readout file...";
                Application.DoEvents();
                //btnAbort.Enabled = false;
                DisableAbort(); //Story - 354414 - Thread Implementation
                string resultMessage = string.Empty;
                //this.Cursor = Cursors.WaitCursor;
                SetCursor(Cursors.WaitCursor);
                ConfigSettings.ChangeNode("SourceOfFile", CAB.UI.Common.GetChannelType());
                IsUploaded = uploadFile.UploadCABFile(filePath, uploadFile.GetIECFileContent(filePath), true, out resultMessage, null);
                if (IsUploaded)
                {
                    this.ListRefresh = true;
                    //this.RightStatusMessage = String.Empty;
                    //this.StatusMessage = "File Uploaded successfully.";
                    this.RightStatusMessageAsync = String.Empty;
                    this.StatusMessageAsync = "File Uploaded successfully.";
                }
                else
                {
                    this.RightStatusMessage = String.Empty;
                    this.StatusMessage = resultMessage;
                    this.RightStatusMessageAsync = String.Empty;
                    this.StatusMessageAsync = resultMessage;
                }
                //this.Cursor = Cursors.WaitCursor;
                SetCursor(Cursors.WaitCursor);
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.rbtnAll_CheckedChanged(this, null);
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
            this.StatusMessageAsync = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
        }

        private void MeterDataReadoutForm_Load(object sender, EventArgs e)
        {
            BindLoadSurveyDays();
            //btnCancel.Enabled = true;
            EnableCancel();
            //this.rbtnAll_CheckedChanged(this, null);           
            this.Text = "Meter Readout";
            readOut = new ReadoutFirmwareVersion();
            readOut.Channel = communications;
            versionFlag = true;
            //btnAbort.Enabled = false;
            DisableAbort();
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];
        }
        /// <summary>
        /// to start the progress bar and overlap the position 
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StartProgressBarTimer()
        {
            statusStrip.Visible = true;
            statusStrip.Enabled = true;
            progressBarTimer.Enabled = true;
        }

        /// <summary>
        /// to stop progress bar , make it in-visible and make 
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StopProgressBarTimer()
        {
            statusStrip.Visible = false;
            progressBarTimer.Enabled = false;
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
        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void setConnectionDetail(bool isConnected)
        {
            string mode;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (isConnected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "IEC" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
            else
            {
                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
        }

        private void Channel_OnStatusChanged(string msg)
        {
            this.StatusMessage = msg;
            this.StatusMessageAsync = msg;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            try
            {
                //StopProgressBarTimer();
                EnableStopTimer();
                setConnectionDetail(false);
                //IsAborted = true;
                EnableAbort();
                if (readOut != null)
                    readOut.IsAborted = true;
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                readOut = null;
                this.RightStatusMessage = string.Empty;
                this.StatusMessage = string.Empty;
                this.RightStatusMessageAsync = string.Empty;
                this.StatusMessageAsync = string.Empty;
                Application.DoEvents();
                this.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnCancel_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            //Function changed on 29th feb 2012 as per the bug report
            //btnCancel.Enabled = false;
            DisableCancel();
            //btnAbort.Enabled = false;
            DisableAbort();
            if (readThread != null)
            {
                readThread.Abort();
            }
            Application.DoEvents();
            IsAborted = true;
            readOut.IsAborted = true;
            //this.StatusMessage = "Aborting...";
            this.StatusMessageAsync = "Aborting...";

            if (currentProfile != 0)
            {
                // Story - 354414 - Thread Implementation - All the above properties can be set in the comman method
                SetGridRowAttributes(System.Drawing.Color.Red, currentProfile, "User Aborted...");

            }
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            communications.ClosePort();
            this.RightStatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
            //StopProgressBarTimer();
            setConnectionDetail(false);
            Application.DoEvents();

            Thread.Sleep(500);
            if (!readMtrConfig)
            {
                //this.StatusMessage = "User Aborted.";
                this.StatusMessageAsync = "User Aborted.";
                Application.DoEvents();
            }
            //this.Cursor = Cursors.Default;
            SetCursor(Cursors.Default);
            EnableReadControls();
            //btnCancel.Enabled = true;
            //lngGridViewReadControl1.Enabled = true;
            EnableStopTimer();
            Application.DoEvents();
        }
        private void btnCancelPhasor_Click(object sender, EventArgs e)
        {
            //StopProgressBarTimer();
            EnableStopTimer();
            setConnectionDetail(false);
            this.Close();
            ChangeStatus(string.Empty);
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //StopProgressBarTimer();
            EnableStopTimer();
            setConnectionDetail(false);
            this.Close();
            ChangeStatus(string.Empty);
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
            this.StatusMessageAsync = string.Empty;
            Application.DoEvents();
            chkLoadSurvey.Checked = true;
            chkGeneral.Checked = true;
            chkTamper.Checked = true;
            chkTransaction.Checked = true;
            chkPhasor.Checked = true;
            chkFraudEnergy.Checked = true;
            chkDTMDaily.Checked = true;

            grpLoadSurvey.Enabled = true;


            if (chkMeterConfigurations.Visible)
            {
                chkMeterConfigurations.Checked = true;
            }

            btnAbort.Enabled = false;
            btnRead.Enabled = true;
            btnCancel.Enabled = false;

        }
        private void rbtnPartial_CheckedChanged(object sender, EventArgs e)
        {
            this.RightStatusMessage = string.Empty;
            this.StatusMessage = string.Empty;
            this.RightStatusMessageAsync = string.Empty;
            this.StatusMessageAsync = string.Empty;
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
                grpLoadSurvey.Enabled = false;
            }
        }

        private void chkGeneral_CheckedChanged(object sender, EventArgs e)
        {
            rbtnAll.CheckedChanged -= rbtnAll_CheckedChanged;
            rbtnPartial.CheckedChanged -= rbtnPartial_CheckedChanged;

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

            if (chkMeterConfigurations.Visible && !chkMeterConfigurations.Checked)
                return false;
            if (chkGeneral.Visible && !chkGeneral.Checked)
                return false;
            if (chkFraudEnergy.Visible && !chkFraudEnergy.Checked)
                return false;
            if (chkLoadSurvey.Visible && !chkLoadSurvey.Checked)
                return false;
            if (chkTamper.Visible && !chkTamper.Checked)
                return false;
            if (chkTransaction.Visible && !chkTransaction.Checked)
                return false;
            if (chkPhasor.Visible && !chkPhasor.Checked)
                return false;
            if (chkDTMDaily.Visible && !chkDTMDaily.Checked)
                return false;
            return true;
        }

        private void MeterDataReadoutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!grpReadoptions.Enabled)
                btnAbort_Click(sender, e);
            //this.StatusMessage = "";
            //StopProgressBarTimer();
            //setConnectionDetail(false);
            this.RightStatusMessage = "";
            this.RightStatusMessageAsync = "";
            //this.Cursor = Cursors.Default;
            SetCursor(Cursors.Default);

            //Story - 354414 - Thread Implementation
            if (isMeterReading)
            {
                e.Cancel = true;
            }
            else
            {
                MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;

                if (menuStrip.InvokeRequired)
                {
                    menuStrip.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    DataAcquisition.Enabled = true;
                    Configuration.Enabled = true;
                    setConnectionDetail(false);
                    EnableStopTimer();
                }
                this.StatusMessage = "";
                this.StatusMessageAsync = "";
            }
        }
        /// <summary>
        /// progress bar timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar.Value > toolStripProgressBar.Maximum - 1)
            {
                toolStripProgressBar.Value = 0;
            }
            else
            {
                toolStripProgressBar.Value = toolStripProgressBar.Value + 10;
            }
        }
        /// <summary>
        /// Read meter in a thread ,takes state as a parameter which will 
        /// contain information required for thread to process
        /// Story - 354414 - Thread Implementation - To Read the meter in a thread
        /// This method is having all the content was in btn_ReadClick to process the meter readout. This is containing filtered data
        /// In case need original, can go in the btn_ReadClick in the commented code section
        /// </summary>
        /// <param name="state"></param>
        private void ReadMeter(object state)
        {
            //string tamperData = string.Empty;
            //string generalData = string.Empty;
            //string loadSurveyData = string.Empty;
            //string transactionData = string.Empty;
            //string phasorData = string.Empty;
            //string fraudEnergyData = string.Empty;
            //string dTMDailyProfileData = string.Empty;
            //string headerInfoData = string.Empty;
            //string namePlateDetailData = string.Empty;
            //string meterConfigurationData = string.Empty;
            //string readingDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            //bool isEmptyData = false;

            this.StatusMessageAsync = string.Empty;
            Application.DoEvents();
            SetCursor(Cursors.WaitCursor);
            selectedProfiles = GetSelectedProfilesToRead(IsMeterType);
            isMeterReading = true;
            string fileText = string.Empty;

            EnableAbort();
            Application.DoEvents();

            #region Commented

            # region General Data
            //if (!IsAborted)
            //{
            //    generalData = string.Empty;
            //    Application.DoEvents();

            //    this.StatusMessageAsync = "Reading General/Instant/Billing data.....";
            //    currentProfile = ProfileId.GeneralBilling;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.GeneralBilling, "Reading Data...");
            //    Application.DoEvents();

            //    setConnectionDetail(true);
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutGeneralForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutGeneralForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutGeneral();
            //        readOut.OnChannelStatusChanged += new ReadoutGeneral.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    readOut.ReadingDateTime = readingDateTime;
            //    generalData = readOut.GetData();
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, SIGNONFAILURE);
            //        ButtonStatus();
            //        Application.DoEvents();
            //        EnableReadControls();
            //        SetCursor(Cursors.Default);
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();
            //        this.StatusMessageAsync = "User Aborted";
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, "User Aborted...");

            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (generalData == "1" || generalData == "2" || generalData == "3")
            //    {
            //        ButtonStatus();
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, this.StatusMessageAsync.ToString());

            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (generalData.Trim().Length == 1)
            //    {
            //        ButtonStatus();
            //        if (generalData.Length < 5)
            //            generalData = string.Empty;

            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //    {
            //        if (!isEmptyData)
            //        {
            //            if (generalData == string.Empty)
            //            {
            //                this.StatusMessageAsync = "General/Instant/Billing data not available in meter";

            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, "Data Not Available...");
            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "General/Instant/Billing data read successfully.";
            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.GeneralBilling, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //        isEmptyData = false;
            //    }
            //}

            # endregion

            #region Tamper Data
            //if (selectedProfiles.Contains(ProfileId.Tamper) && !IsAborted)
            //{
            //    this.StatusMessageAsync = "Reading Tamper data.....";
            //    currentProfile = ProfileId.Tamper;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Tamper, "Reading Data...");
            //    Application.DoEvents();
            //    setConnectionDetail(true);
            //    tamperData = string.Empty;
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutTamperForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutTamperForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutTamper();
            //        readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    readOut.ReadingDateTime = readingDateTime;
            //    tamperData = readOut.GetData();

            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        if (tamperData.Length >= 60)
            //            tamperData = Convert.ToChar(1) + "TM" + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + "/" + tamperData + Convert.ToChar(4);
            //    }
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, SIGNONFAILURE);
            //        ButtonStatus();

            //        Application.DoEvents();
            //        EnableReadControls();
            //        SetCursor(Cursors.Default);
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();
            //        this.StatusMessageAsync = "User Aborted";

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "User Aborted...");

            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (tamperData == "1" || tamperData == "2" || tamperData == "3" || string.IsNullOrEmpty(tamperData))
            //    {
            //        ButtonStatus();
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, this.StatusMessageAsync.ToString());
            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (tamperData.Trim().Length <= 1)
            //    {
            //        ButtonStatus();

            //        if (tamperData.Length < 5)
            //            tamperData = string.Empty;
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //    {
            //        if (!isEmptyData)
            //        {
            //            if (tamperData == string.Empty)
            //            {
            //                this.StatusMessageAsync = "Tamper data not available in meter";

            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "Data Not Available...");
            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "Tamper data read successfully.";

            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Tamper, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //        isEmptyData = false;
            //    }
            //}
            # endregion

            #region Transaction Data
            //if (selectedProfiles.Contains(ProfileId.Transaction) && !IsAborted)
            //{
            //    this.StatusMessageAsync = "Reading Transaction data.....";
            //    currentProfile = ProfileId.Tamper;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Tamper, "Reading Data...");

            //    Application.DoEvents();
            //    setConnectionDetail(true);
            //    transactionData = string.Empty;
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutTransactionForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutTransactionForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutTransaction();
            //        readOut.OnChannelStatusChanged += new ReadoutTransaction.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    readOut.ReadingDateTime = readingDateTime;
            //    transactionData = readOut.GetData();
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, SIGNONFAILURE);
            //        ButtonStatus();

            //        SetCursor(Cursors.Default);
            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();

            //        this.StatusMessageAsync = "User Aborted";

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "User Aborted...");
            //        SetCursor(Cursors.Default);
            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(transactionData))
            //    {
            //        ButtonStatus();

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, this.StatusMessageAsync.ToString());
            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    Thread.Sleep(200);
            //    if (communications.ResponseSignOn != string.Empty && transactionData.Length >= 313)
            //    {
            //        string response = readOut.MeterID(communications.ResponseSignOn);
            //        string strtemptr = Convert.ToChar(4).ToString() + Convert.ToChar(1).ToString() + ReadoutConstant.REGISTER + response + "/" + readingDateTime;
            //        transactionData = transactionData.Replace(ReadoutConstant.TRANSACTION, strtemptr);
            //        transactionData = Convert.ToChar(1) + ReadoutConstant.TAMPER + response + "/" + readingDateTime + transactionData + Convert.ToChar(4);
            //    }
            //    else
            //    {
            //        ButtonStatus();

            //        transactionData = string.Empty;
            //        this.StatusMessageAsync = "Invalid Response from meter.";
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            SetCursor(Cursors.Default);
            //            return;
            //        }
            //    }
            //    if (!isEmptyData)
            //    {

            //        if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //        {

            //            if (transactionData.Trim().Equals(string.Empty))
            //            {
            //                this.StatusMessageAsync = "Transaction data not available in meter";
            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "Data Not Available...");
            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "Transaction data read successfully.";
            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Tamper, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //    }
            //    isEmptyData = false;
            //}
            #endregion

            #region Phasor Data
            //if (selectedProfiles.Contains(ProfileId.Phasor) && !IsAborted)
            //{
            //    phasorData = string.Empty;
            //    this.StatusMessageAsync = "Reading Phasor data.....";
            //    currentProfile = ProfileId.Phasor;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Phasor, "Reading Data...");
            //    Application.DoEvents();
            //    setConnectionDetail(true);
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutPhasorForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutPhasorForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutPhasor();
            //        readOut.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    readOut.ReadingDateTime = readingDateTime;
            //    phasorData = readOut.GetData();
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, SIGNONFAILURE);
            //        SetCursor(Cursors.Default);
            //        ButtonStatus();

            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();
            //        this.StatusMessageAsync = "User Aborted";

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, "User Aborted...");
            //        SetCursor(Cursors.Default);
            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(phasorData))
            //    {
            //        ButtonStatus();
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, this.StatusMessageAsync.ToString());
            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (phasorData.Length >= 93)
            //        phasorData = Convert.ToChar(1) + ReadoutConstant.PHASOR + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + phasorData + Convert.ToChar(4);
            //    else
            //    {
            //        ButtonStatus();

            //        phasorData = string.Empty;
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    if (!isEmptyData)
            //    {
            //        if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //        {
            //            if (phasorData.Trim().Equals(string.Empty))
            //            {
            //                this.StatusMessageAsync = "Phasor data not available in meter";
            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, "Data Not Available...");
            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "Phasor data read successfully.";
            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Phasor, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //    }
            //    isEmptyData = false;
            //}
            #endregion

            #region Fraud & Reverse Energy data
            //if (selectedProfiles.Contains(ProfileId.FraudEnergy) && !IsAborted)
            //{
            //    fraudEnergyData = string.Empty;
            //    this.StatusMessageAsync = "Reading Fraud Energy data.....";
            //    currentProfile = ProfileId.FraudEnergy;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.FraudEnergy, "Reading Data...");
            //    Application.DoEvents();
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutFraudEnergyForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutFraudEnergyForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutFraudEnergy();
            //        readOut.OnChannelStatusChanged += new ReadoutFraudEnergy.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    readOut.ReadingDateTime = readingDateTime;
            //    setConnectionDetail(true);
            //    fraudEnergyData = readOut.GetData();
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, SIGNONFAILURE);
            //        SetCursor(Cursors.Default);
            //        ButtonStatus();

            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();

            //        this.StatusMessageAsync = "User Aborted";

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, "User Aborted...");
            //        SetCursor(Cursors.Default);
            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(fraudEnergyData))
            //    {
            //        ButtonStatus();

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, this.StatusMessageAsync.ToString());
            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (fraudEnergyData != string.Empty)
            //        fraudEnergyData = string.Concat(Convert.ToChar(1), ReadoutConstant.MAGNETICINFLUENCE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, fraudEnergyData);
            //    else
            //    {
            //        ButtonStatus();

            //        fraudEnergyData = string.Empty;
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    string reserveEnergy = readOut.ReverseEnergy();
            //    if (reserveEnergy != "")
            //        fraudEnergyData = string.Concat(fraudEnergyData, Convert.ToChar(1), reserveEnergy, Convert.ToChar(4));
            //    else
            //    {
            //        ButtonStatus();

            //        fraudEnergyData = string.Empty;
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    if (!isEmptyData)
            //    {
            //        if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //        {
            //            if (fraudEnergyData.Trim().Equals(string.Empty))
            //            {
            //                this.StatusMessageAsync = "Fraud Energy data not available in meter";
            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, "Data Not Available...");

            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "Fraud Energy data read successfully.";
            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.FraudEnergy, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //    }
            //    isEmptyData = false;
            //}
            #endregion

            #region Daily Profile Data
            //if (selectedProfiles.Contains(ProfileId.Midnight) && !IsAborted)
            //{
            //    this.StatusMessageAsync = "Reading Daily Profile data.....";
            //    currentProfile = ProfileId.Midnight;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Midnight, "Reading Data...");

            //    Application.DoEvents();
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new ReadoutDTMDailyProfileForSingllePhaseIEC(isTNEB);
            //        readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfileForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new ReadoutDTMDailyProfile(isTNEB);
            //        readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfile.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    readOut.Channel = communications;
            //    readOut.IsAborted = IsAborted;
            //    setConnectionDetail(true);
            //    string dtmParameterData = readOut.GetDTMParameterData();
            //    if (readOut.IsSignOnFailure)
            //    {
            //        this.StatusMessageAsync = SIGNONFAILURE;
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);

            //        SetCursor(Cursors.Default);
            //        ButtonStatus();

            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (readOut.IsAborted)
            //    {
            //        ButtonStatus();

            //        this.StatusMessageAsync = "User Aborted";
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "User Aborted...");
            //        SetCursor(Cursors.Default);
            //        Application.DoEvents();
            //        EnableReadControls();
            //        return;
            //    }
            //    if (dtmParameterData == string.Empty)
            //    {
            //        ButtonStatus();

            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);
            //        Application.DoEvents();
            //        SetCursor(Cursors.Default);
            //        EnableReadControls();
            //        return;
            //    }
            //    if (dtmParameterData.Trim() != string.Empty)
            //    {
            //        if (IsMeterType == 1 || IsMeterType == 2)
            //        {
            //            //chek for minimum data length is 24 for single data
            //            if (dtmParameterData.Length >= 24)
            //            {
            //                readOut.ReadingDateTime = readingDateTime;
            //                dTMDailyProfileData = string.Concat(Convert.ToChar(1), ReadoutConstant.DTMDAILYPROFILE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, "/", dtmParameterData, dTMDailyProfileData, Convert.ToChar(4));
            //            }
            //            else
            //            {
            //                ButtonStatus();
            //                dTMDailyProfileData = string.Empty;
            //                this.StatusMessageAsync = "Daily Profile data not available in meter";
            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
            //                Application.DoEvents();
            //                isEmptyData = true;
            //                if (TotalCheckCount() == 1)
            //                {
            //                    readOut.IsAborted = true;
            //                    communications.Command = command.BreakCommand;
            //                    communications.SendCommand();
            //                    communications.DelayExecution();
            //                    communications.ClosePort();
            //                    this.RightStatusMessage = string.Empty;
            //                    this.RightStatusMessageAsync = string.Empty;
            //                    Application.DoEvents();
            //                    Thread.Sleep(500);
            //                    EnableReadControls();
            //                    EnableAbort();
            //                    return;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (dtmParameterData.Length >= 27)
            //            {
            //                readOut.ReadingDateTime = readingDateTime;
            //                dTMDailyProfileData = readOut.GetData();
            //                if (dTMDailyProfileData.Trim() != string.Empty)
            //                {
            //                    if (dTMDailyProfileData.Length >= 15)
            //                        dTMDailyProfileData = string.Concat(Convert.ToChar(1), ReadoutConstant.DTMDAILYPROFILE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, dtmParameterData, dTMDailyProfileData, Convert.ToChar(4));
            //                    else
            //                    {
            //                        ButtonStatus();

            //                        dTMDailyProfileData = string.Empty;
            //                        this.StatusMessageAsync = "Daily Profile data not available in meter";
            //                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
            //                        Application.DoEvents();
            //                        isEmptyData = true;
            //                        if (TotalCheckCount() == 1)
            //                        {
            //                            readOut.IsAborted = true;
            //                            communications.Command = command.BreakCommand;
            //                            communications.SendCommand();
            //                            communications.DelayExecution();
            //                            communications.ClosePort();
            //                            this.RightStatusMessage = string.Empty;
            //                            this.RightStatusMessageAsync = string.Empty;
            //                            Application.DoEvents();
            //                            Thread.Sleep(500);
            //                            EnableReadControls();
            //                            EnableAbort();
            //                            return;
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    ButtonStatus();

            //                    SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);
            //                    Application.DoEvents();
            //                    SetCursor(Cursors.Default);
            //                    EnableReadControls();
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ButtonStatus();

            //        dtmParameterData = string.Empty;
            //        Application.DoEvents();
            //        isEmptyData = true;
            //        if (TotalCheck() == 1)
            //        {
            //            EnableReadControls();
            //            return;
            //        }
            //    }
            //    if (!isEmptyData)
            //    {
            //        if (!readOut.IsAborted && !readOut.IsCorruptedData)
            //        {
            //            if (dTMDailyProfileData.Trim().Equals(string.Empty))
            //            {
            //                this.StatusMessageAsync = "Daily Profile data not available in meter";
            //                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
            //                Application.DoEvents();
            //                Application.DoEvents();
            //            }
            //            else
            //            {
            //                this.StatusMessageAsync = "Daily Profile data read successfully.";
            //                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Midnight, "Readout Successful...");
            //                Application.DoEvents();
            //            }
            //        }
            //    }
            //    isEmptyData = false;
            //}
            #endregion

            #region Load Survey Data
            //if (selectedProfiles.Contains(ProfileId.LoadSurvey) && !IsAborted)
            //{
            //    this.StatusMessageAsync = "Reading Load Survey data.....";
            //    currentProfile = ProfileId.LoadSurvey;

            //    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.LoadSurvey, "Reading Data...");

            //    Application.DoEvents();
            //    setConnectionDetail(true);
            //    if (IsMeterType == 1 || IsMeterType == 2)
            //    {
            //        readOut = new CAB.IECChannel.ReadOut.ReadoutDTMLoadSurveyForSingllePhaseIEC();
            //        readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurveyForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }
            //    else
            //    {
            //        readOut = new CAB.IECChannel.ReadOut.ReadoutDTMLoadSurvey();
            //        readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
            //    }

            //    readOut.Channel = communications;
            //    readOut.ReadingDateTime = readingDateTime;

            //    if (!isEmptyData)
            //    {
            //        if (IsMeterType == 1 || IsMeterType == 2)
            //        {
            //            loadSurveyData = ((ReadoutDTMLoadSurveyForSingllePhaseIEC)readOut).GetData(ConfigSettings.GetValue("LoadSurveyDays"), 90);
            //            if (loadSurveyData.Length >= 15)
            //                loadSurveyData = Convert.ToChar(1) + ReadoutConstant.DTMPROFILE + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + "/" + loadSurveyData + Convert.ToChar(4); //responseForLoadSurvey
            //        }
            //        else
            //        {
            //            GetLoadSurveyDaysAndMDInterval();
            //            loadSurveyData = ((ReadoutDTMLoadSurvey)readOut).GetData(ConfigSettings.GetValue("LoadSurveyDays"), totalDay); // Story - 349654 - Load Survey day value should come from settings page
            //        }
            //        if (readOut.IsSignOnFailure)
            //        {
            //            this.StatusMessageAsync = SIGNONFAILURE;
            //            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, SIGNONFAILURE);
            //            SetCursor(Cursors.Default);
            //            ButtonStatus();
            //            Application.DoEvents();
            //            EnableReadControls();
            //            return;
            //        }
            //        else if (readOut.IsAborted)
            //        {
            //            ButtonStatus();
            //            this.StatusMessageAsync = "User Aborted";
            //            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "User Aborted");
            //            SetCursor(Cursors.Default);
            //            Application.DoEvents();
            //            EnableReadControls();
            //            return;
            //        }
            //        else if (string.IsNullOrEmpty(loadSurveyData))
            //        {
            //            ButtonStatus();
            //            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "Readout Failure");
            //            Application.DoEvents();
            //            SetCursor(Cursors.Default);
            //            EnableReadControls();
            //            return;
            //        }
            //        else
            //        {
            //            SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.LoadSurvey, "Readout Successful...");
            //        }
            //        ChangeStatus(loadSurveyData, readOut.IsSignOnFailure);
            //    }
            //    else
            //    {
            //        this.StatusMessageAsync = "Load Survey data not available in meter";
            //        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "Data Not Available...");
            //        Application.DoEvents();
            //    }
            //}
            #endregion

            //if (loadSurveyData.Length <= 5)
            //    loadSurveyData = string.Empty;


            //if (generalData.Length <= 1)
            //    generalData = string.Empty;


            //string fileText = string.Concat(headerInfoData, namePlateDetailData, generalData, tamperData, loadSurveyData, transactionData, phasorData, fraudEnergyData, dTMDailyProfileData, meterConfigurationData);
            # endregion

            try
            {
                communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                if ((ConfigSettings.GetValue("ChannelType") != CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
                {
                    communications.noOfRetry = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    if (oneToOneGSM.Checked)
                    {
                        ReadOneToOne();
                    }
                    else if (oneToManyGSM.Checked)
                    {
                        ReadOneToMany();
                    }
                }
                else
                {
                    SetCursor(Cursors.WaitCursor);
                    this.StatusMessageAsync = "Reading Meter Data...";
                    EnableStartTimer();
                    GetMeterData(false, 0);
                }
            }
            catch (Exception ex)
            {
                this.StatusMessageAsync = "Readout Failure";
                logger.Log(LOGLEVELS.Error, "Error while Reading PUMA Meter.", ex);
            }
            finally
            {
                EnableReadControls();
                isMeterReading = false;
                Application.DoEvents();
                SetCursor(Cursors.Default);
                EnableStopTimer();
                setConnectionDetail(false);
            }
        }
        private void ReadOneToOne()
        {
            try
            {
                if (!ValidateSimNumber())
                {
                    return;
                }
                SetCursor(Cursors.WaitCursor);
                this.StatusMessageAsync = "Reading Meter Data...";
                EnableStartTimer();
                communications.SimNumber = "0" + txtBoxMeterSIM.Text.Trim();
                GetMeterData(true, 0);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ReadOneToOne()", ex);
            }
        }
        private void ReadOneToMany()
        {
            try
            {
                bool isConnected = false;
                if (ValidateGrid())
                {
                    SetCursor(Cursors.WaitCursor);
                    this.StatusMessageAsync = "Reading Meter Data...";
                    EnableStartTimer();
                    ResetGrid();
                    byte totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                    for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                    {
                        for (int rowCount = 0; rowCount < dgvMeterIdAndSim.RowCount; rowCount++)
                        {
                            EnableAbort();
                            DataGridViewCheckBoxCell chk1 = dgvMeterIdAndSim.Rows[rowCount].Cells["Select"] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(chk1.Value))
                            {
                                simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..."
                                    : "Connecting " + simNumber + " ...";
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightYellow;
                                this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                                Application.DoEvents();

                                communications.SimNumber = simNumber;
                                isConnected = GetMeterData(true, rowCount);
                                DisableAbort();
                                if (isConnected)
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.LightGreen;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout completed.";
                                    this.StatusMessageAsync = "Readout completed.";
                                    dgvMeterIdAndSim.Rows[rowCount].Cells["Select"].Value = false;
                                }
                                else
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Style.BackColor = System.Drawing.Color.Red;
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, rowCount].Value = "Readout failed.";
                                    this.StatusMessageAsync = "Readout failed.";
                                }
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
        /// <summary>
        /// Change grid status while click on read button .
        /// </summary>
        private void ResetGrid()
        {
            for (int count = 0; count < dgvMeterIdAndSim.Rows.Count; count++)
            {
                DataGridViewCheckBoxCell selectedCheckBox = dgvMeterIdAndSim.Rows[count].Cells["Select"] as DataGridViewCheckBoxCell;
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
        private bool GetMeterData(bool isRemote, int simIndex)
        {
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
            string readingDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            bool isEmptyData = false;
            string fileText = string.Empty;
            bool flag = false;
            try
            {
                # region General Data
                if (!IsAborted)
                {
                    generalData = string.Empty;
                    EnableStartTimer();
                    Application.DoEvents();
                    this.StatusMessageAsync = "Reading General/Instant/Billing data.....";
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    currentProfile = ProfileId.GeneralBilling;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.GeneralBilling, "Reading Data...");
                    Application.DoEvents();

                    setConnectionDetail(true);
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutGeneralForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutGeneralForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutGeneral();
                        readOut.OnChannelStatusChanged += new ReadoutGeneral.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    generalData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        ButtonStatus();
                        Application.DoEvents();
                        EnableReadControls();
                        SetCursor(Cursors.Default);
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessageAsync = "User Aborted";
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, "User Aborted...");
                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (generalData == "1" || generalData == "2" || generalData == "3")
                    {
                        ButtonStatus();
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (generalData.Trim().Length == 1)
                    {
                        ButtonStatus();
                        if (generalData.Length < 5)
                            generalData = string.Empty;

                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!readOut.IsAborted && !readOut.IsCorruptedData)
                    {
                        if (!isEmptyData)
                        {
                            if (generalData == string.Empty)
                            {
                                this.StatusMessageAsync = "General/Instant/Billing data not available in meter";

                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.GeneralBilling, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = "Data Not Available...";
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "General/Instant/Billing data read successfully.";
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.GeneralBilling, "Readout Successful...");
                                Application.DoEvents();
                            }
                        }
                        isEmptyData = false;
                    }
                }

                # endregion

                #region Tamper Data
                if (selectedProfiles.Contains(ProfileId.Tamper) && !IsAborted)
                {
                    this.StatusMessageAsync = "Reading Tamper data.....";
                    currentProfile = ProfileId.Tamper;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Tamper, "Reading Data...");
                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    setConnectionDetail(true);
                    tamperData = string.Empty;
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutTamperForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutTamperForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutTamper();
                        readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    tamperData = readOut.GetData();

                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        if (tamperData.Length >= 60)
                            tamperData = Convert.ToChar(1) + "TM" + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + "/" + tamperData + Convert.ToChar(4);
                    }
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        ButtonStatus();

                        Application.DoEvents();
                        EnableReadControls();
                        SetCursor(Cursors.Default);
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessageAsync = "User Aborted";

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "User Aborted...");

                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (tamperData == "1" || tamperData == "2" || tamperData == "3" || string.IsNullOrEmpty(tamperData))
                    {
                        ButtonStatus();
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
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
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!readOut.IsAborted && !readOut.IsCorruptedData)
                    {
                        if (!isEmptyData)
                        {
                            if (tamperData == string.Empty)
                            {
                                this.StatusMessageAsync = "Tamper data not available in meter";

                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = "Data Not Available...";
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "Tamper data read successfully.";

                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Tamper, "Readout Successful...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                        }
                        isEmptyData = false;
                    }
                }
                # endregion

                #region Transaction Data
                if (selectedProfiles.Contains(ProfileId.Transaction) && !IsAborted)
                {
                    this.StatusMessageAsync = "Reading Transaction data.....";
                    currentProfile = ProfileId.Tamper;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Tamper, "Reading Data...");

                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    setConnectionDetail(true);
                    transactionData = string.Empty;
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutTransactionForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutTransactionForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutTransaction();
                        readOut.OnChannelStatusChanged += new ReadoutTransaction.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    transactionData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        ButtonStatus();

                        SetCursor(Cursors.Default);
                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();

                        this.StatusMessageAsync = "User Aborted";

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "User Aborted...");
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (string.IsNullOrEmpty(transactionData))
                    {
                        ButtonStatus();

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty && transactionData.Length >= 313)
                    {
                        string response = readOut.MeterID(communications.ResponseSignOn);
                        string strtemptr = Convert.ToChar(4).ToString() + Convert.ToChar(1).ToString() + ReadoutConstant.REGISTER + response + "/" + readingDateTime;
                        transactionData = transactionData.Replace(ReadoutConstant.TRANSACTION, strtemptr);
                        transactionData = Convert.ToChar(1) + ReadoutConstant.TAMPER + response + "/" + readingDateTime + transactionData + Convert.ToChar(4);
                    }
                    else
                    {
                        ButtonStatus();

                        transactionData = string.Empty;
                        this.StatusMessageAsync = "Invalid Response from meter.";
                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnableReadControls();
                            SetCursor(Cursors.Default);
                            return false;
                        }
                    }
                    if (!isEmptyData)
                    {

                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {

                            if (transactionData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessageAsync = "Transaction data not available in meter";
                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Tamper, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "Transaction data read successfully.";
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Tamper, "Readout Successful...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Phasor Data
                if (selectedProfiles.Contains(ProfileId.Phasor) && !IsAborted)
                {
                    phasorData = string.Empty;
                    this.StatusMessageAsync = "Reading Phasor data.....";
                    currentProfile = ProfileId.Phasor;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Phasor, "Reading Data...");
                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    setConnectionDetail(true);
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutPhasorForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutPhasorForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutPhasor();
                        readOut.OnChannelStatusChanged += new ReadoutPhasor.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    phasorData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        ButtonStatus();

                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessageAsync = "User Aborted";

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, "User Aborted...");
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (string.IsNullOrEmpty(phasorData))
                    {
                        ButtonStatus();
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
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
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (phasorData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessageAsync = "Phasor data not available in meter";
                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Phasor, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "Phasor data read successfully.";
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Phasor, "Readout Successful...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Fraud & Reverse Energy data
                if (selectedProfiles.Contains(ProfileId.FraudEnergy) && !IsAborted)
                {
                    fraudEnergyData = string.Empty;
                    this.StatusMessageAsync = "Reading Fraud Energy data.....";
                    currentProfile = ProfileId.FraudEnergy;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.FraudEnergy, "Reading Data...");
                    Application.DoEvents();
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutFraudEnergyForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutFraudEnergyForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutFraudEnergy();
                        readOut.OnChannelStatusChanged += new ReadoutFraudEnergy.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    setConnectionDetail(true);
                    fraudEnergyData = readOut.GetData();
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, SIGNONFAILURE);
                        SetCursor(Cursors.Default);
                        ButtonStatus();

                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();

                        this.StatusMessageAsync = "User Aborted";

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, "User Aborted...");
                        SetCursor(Cursors.Default);
                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (string.IsNullOrEmpty(fraudEnergyData))
                    {
                        ButtonStatus();

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
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
                            EnableReadControls();
                            return false;
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
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (fraudEnergyData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessageAsync = "Fraud Energy data not available in meter";
                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.FraudEnergy, "Data Not Available...");

                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessageAsync = "Fraud Energy data read successfully.";
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.FraudEnergy, "Readout Successful...");
                                Application.DoEvents();
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Daily Profile Data
                if (selectedProfiles.Contains(ProfileId.Midnight) && !IsAborted)
                {
                    this.StatusMessageAsync = "Reading Daily Profile data.....";
                    currentProfile = ProfileId.Midnight;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.Midnight, "Reading Data...");

                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new ReadoutDTMDailyProfileForSingllePhaseIEC(isTNEB);
                        readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfileForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new ReadoutDTMDailyProfile(isTNEB);
                        readOut.OnChannelStatusChanged += new ReadoutDTMDailyProfile.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    setConnectionDetail(true);
                    //string dtmParameterData = readOut.GetDTMParameterData();

                    string dtmParameterData = ((ReadoutDTMDailyProfileForSingllePhaseIEC)readOut).GetDataDS(ConfigSettings.GetValue("DailySurveyDays"), 90);;
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        ButtonStatus();

                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();

                        this.StatusMessageAsync = "User Aborted";
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "User Aborted...");
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        Application.DoEvents();
                        EnableReadControls();
                        return false;
                    }
                    if (dtmParameterData == string.Empty)
                    {
                        ButtonStatus();

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (dtmParameterData.Trim() != string.Empty)
                    {
                        if (IsMeterType == 1 || IsMeterType == 2)
                        {
                            //chek for minimum data length is 24 for single data
                            if (dtmParameterData.Length >= 24)
                            {
                                readOut.ReadingDateTime = readingDateTime;
                                dTMDailyProfileData = string.Concat(Convert.ToChar(1), ReadoutConstant.DTMDAILYPROFILE, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, "/", dtmParameterData, dTMDailyProfileData, Convert.ToChar(4));
                            }
                            else
                            {
                                ButtonStatus();
                                dTMDailyProfileData = string.Empty;
                                this.StatusMessageAsync = "Daily Profile data not available in meter";
                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                                isEmptyData = true;
                                if (TotalCheckCount() == 1)
                                {
                                    readOut.IsAborted = true;
                                    communications.Command = command.BreakCommand;
                                    communications.SendCommand();
                                    communications.DelayExecution();
                                    communications.ClosePort();
                                    this.RightStatusMessage = string.Empty;
                                    this.RightStatusMessageAsync = string.Empty;
                                    Application.DoEvents();
                                    Thread.Sleep(500);
                                    EnableReadControls();
                                    EnableAbort();
                                    return false;
                                }
                            }
                        }
                        else
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
                                        this.StatusMessageAsync = "Daily Profile data not available in meter";
                                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
                                        Application.DoEvents();
                                        isEmptyData = true;
                                        if (TotalCheckCount() == 1)
                                        {
                                            readOut.IsAborted = true;
                                            communications.Command = command.BreakCommand;
                                            communications.SendCommand();
                                            communications.DelayExecution();
                                            communications.ClosePort();
                                            this.RightStatusMessage = string.Empty;
                                            this.RightStatusMessageAsync = string.Empty;
                                            Application.DoEvents();
                                            Thread.Sleep(500);
                                            EnableReadControls();
                                            EnableAbort();
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    ButtonStatus();

                                    SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, SIGNONFAILURE);
                                    Application.DoEvents();
                                    SetCursor(Cursors.Default);
                                    EnableReadControls();
                                    return false;
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
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!isEmptyData)
                    {
                        if (!readOut.IsAborted && !readOut.IsCorruptedData)
                        {
                            if (dTMDailyProfileData.Trim().Equals(string.Empty))
                            {
                                this.StatusMessageAsync = "Daily Profile data not available in meter";
                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.Midnight, "Data Not Available...");
                                Application.DoEvents();
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "Daily Profile data read successfully.";
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.Midnight, "Readout Successful...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    isEmptyData = false;
                }
                #endregion

                #region Load Survey Data
                if (selectedProfiles.Contains(ProfileId.LoadSurvey) && !IsAborted)
                {
                    this.StatusMessageAsync = "Reading Load Survey data.....";
                    currentProfile = ProfileId.LoadSurvey;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.LoadSurvey, "Reading Data...");

                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    setConnectionDetail(true);
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new CAB.IECChannel.ReadOut.ReadoutDTMLoadSurveyForSingllePhaseIEC();
                        readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurveyForSingllePhaseIEC.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    else
                    {
                        readOut = new CAB.IECChannel.ReadOut.ReadoutDTMLoadSurvey();
                        readOut.OnChannelStatusChanged += new ReadoutDTMLoadSurvey.ChannelStatusChanged(Channel_OnStatusChanged);
                    }

                    readOut.Channel = communications;
                    readOut.ReadingDateTime = readingDateTime;

                    if (!isEmptyData)
                    {
                        if (IsMeterType == 1 || IsMeterType == 2)
                        {
                            loadSurveyData = ((ReadoutDTMLoadSurveyForSingllePhaseIEC)readOut).GetData(ConfigSettings.GetValue("LoadSurveyDays"), 90);
                            if (loadSurveyData.Length >= 15)
                                loadSurveyData = Convert.ToChar(1) + ReadoutConstant.DTMPROFILE + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + "/" + loadSurveyData + Convert.ToChar(4); //responseForLoadSurvey
                        }
                        else
                        {
                            GetLoadSurveyDaysAndMDInterval();
                            loadSurveyData = ((ReadoutDTMLoadSurvey)readOut).GetData(ConfigSettings.GetValue("LoadSurveyDays"), totalDay); // Story - 349654 - Load Survey day value should come from settings page
                        }
                        if (readOut.IsSignOnFailure)
                        {
                            this.StatusMessageAsync = SIGNONFAILURE;
                            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, SIGNONFAILURE);
                            if (isRemote && (oneToManyGSM.Checked == true))
                            {
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                Application.DoEvents();
                            }
                            SetCursor(Cursors.Default);
                            ButtonStatus();
                            Application.DoEvents();
                            EnableReadControls();
                            return false;
                        }
                        else if (readOut.IsAborted)
                        {
                            ButtonStatus();
                            this.StatusMessageAsync = "User Aborted";
                            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "User Aborted");
                            if (isRemote && (oneToManyGSM.Checked == true))
                            {
                                dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                Application.DoEvents();
                            }
                            SetCursor(Cursors.Default);
                            Application.DoEvents();
                            EnableReadControls();
                            return false;
                        }
                        else if (string.IsNullOrEmpty(loadSurveyData))
                        {
                            ButtonStatus();
                            SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "Readout Failure");
                            Application.DoEvents();
                            SetCursor(Cursors.Default);
                            EnableReadControls();
                            return false;
                        }
                        else
                        {
                            SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.LoadSurvey, "Readout Successful...");
                        }
                        ChangeStatus(loadSurveyData, readOut.IsSignOnFailure);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        this.StatusMessageAsync = "Load Survey data not available in meter";
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.LoadSurvey, "Data Not Available...");
                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                    }
                }
                #endregion

                #region Meter Configuration

                if (selectedProfiles.Contains(ProfileId.MeterConfiguration) && !IsAborted)
                {
                    this.StatusMessageAsync = "Reading Meter Configuration....";
                    currentProfile = ProfileId.MeterConfiguration;

                    SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.MeterConfiguration, "Reading Data...");
                    Application.DoEvents();
                    if (isRemote && (oneToManyGSM.Checked == true))
                    {
                        dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                        Application.DoEvents();
                    }
                    setConnectionDetail(true);
                    meterConfigurationData = string.Empty;
                    if (IsMeterType == 1 || IsMeterType == 2)
                    {
                        readOut = new CAB.IECChannel.ReadOut.ReadConfigurations();
                        readOut.OnChannelStatusChanged += new ReadConfigurations.ChannelStatusChanged(Channel_OnStatusChanged);
                    }
                    //else
                    //{
                    //    readOut = new ReadoutTamper();
                    //    readOut.OnChannelStatusChanged += new ReadoutTamper.ChannelStatusChanged(Channel_OnStatusChanged);
                    //}
                    readOut.Channel = communications;
                    readOut.IsAborted = IsAborted;
                    readOut.ReadingDateTime = readingDateTime;
                    MeterConfigurationConfigSection configSection = XMLLoader.GetConfigSection(ConfigurationParameter.TODSP);
                    string strlabel = "";
                    string strmeterconfiguration = ((ReadConfigurations)readOut).ReadMeterConfigurations(configSection, ref strlabel);

                    //if (IsMeterType == 1 || IsMeterType == 2)
                    //{
                    //    if (meterConfigurationData.Length >= 20)
                    //        meterConfigurationData = Convert.ToChar(1) + "TU" + readOut.MeterID(communications.ResponseSignOn) + "/" + readingDateTime + "/" + tamperData + Convert.ToChar(4);
                    //}
                    if (readOut.IsSignOnFailure)
                    {
                        this.StatusMessageAsync = SIGNONFAILURE;
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.MeterConfiguration, SIGNONFAILURE);
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        ButtonStatus();

                        Application.DoEvents();
                        EnableReadControls();
                        SetCursor(Cursors.Default);
                        return false;
                    }
                    if (readOut.IsAborted)
                    {
                        ButtonStatus();
                        this.StatusMessageAsync = "User Aborted";

                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.MeterConfiguration, "User Aborted...");

                        Application.DoEvents();
                        if (isRemote && (oneToManyGSM.Checked == true))
                        {
                            dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                            Application.DoEvents();
                        }
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (string.IsNullOrEmpty(strmeterconfiguration))
                    {
                        ButtonStatus();
                        SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.MeterConfiguration, this.StatusMessageAsync.ToString());
                        Application.DoEvents();
                        SetCursor(Cursors.Default);
                        EnableReadControls();
                        return false;
                    }
                    if (strmeterconfiguration.Trim().Length <= 1)
                    {
                        ButtonStatus();

                        if (strmeterconfiguration.Length < 5)
                            strmeterconfiguration = string.Empty;
                        Application.DoEvents();
                        isEmptyData = true;
                        if (TotalCheck() == 1)
                        {
                            EnableReadControls();
                            return false;
                        }
                    }
                    if (!readOut.IsAborted && !readOut.IsCorruptedData)
                    {
                        if (!isEmptyData)
                        {
                            if (strmeterconfiguration == string.Empty)
                            {
                                this.StatusMessageAsync = "Configuration data not available in meter";

                                SetGridRowAttributes(System.Drawing.Color.Red, ProfileId.MeterConfiguration, "Data Not Available...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = "Data Not Available...";
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.StatusMessageAsync = "Configuration data read successfully.";
                                meterConfigurationData = string.Concat(Convert.ToChar(1), ReadoutConstant.METERCONFIGURATION, readOut.MeterID(communications.ResponseSignOn), "/", readingDateTime, "/", strmeterconfiguration, meterConfigurationData, Convert.ToChar(4));
                                SetGridRowAttributes(System.Drawing.Color.LightGreen, ProfileId.MeterConfiguration, "Readout Successful...");
                                Application.DoEvents();
                                if (isRemote && (oneToManyGSM.Checked == true))
                                {
                                    dgvMeterIdAndSim[(int)dgvSimColumn.Status, simIndex].Value = this.StatusMessageAsync;
                                    Application.DoEvents();
                                }
                            }
                        }
                        isEmptyData = false;
                    }
                }

                #endregion

                if (loadSurveyData.Length <= 5)
                    loadSurveyData = string.Empty;


                if (generalData.Length <= 1)
                    generalData = string.Empty;

                fileText = string.Concat(headerInfoData, namePlateDetailData, generalData, tamperData, loadSurveyData, transactionData, phasorData, fraudEnergyData, dTMDailyProfileData, meterConfigurationData);

                if (!IsAborted && fileText.Trim() != string.Empty)
                {
                    string bcc = ReadoutCommon.CalculateFileBcc(fileText);
                    if (bcc != "")
                    {
                        fileText = string.Concat(fileText, bcc);
                        if (fileText != "")
                        {
                            if (isRemote)
                            {
                                this.RightStatusMessageAsync = String.Empty;
                                //this.StatusMessageAsync = "Readout Successful";
                                //EnableStopTimer();
                                //Application.DoEvents();
                                if (fileText.ToUpper().Contains("LGC"))
                                    SaveRemoteDataForSPhaseIEC(fileText, readOut.MeterID(communications.ResponseSignOn)); // Story - 349654 - Meter Id is passed as parameter to append in the file name
                                else
                                    SaveRemoteData(fileText, readOut.MeterID(communications.ResponseSignOn)); // Story - 349654 - Meter Id is passed as parameter to append in the file name

                                // Story - 427028 - MeterId was not getting update while reading thorugh GSM
                                if (oneToManyGSM.Checked == true)
                                {
                                    string meterId = string.Empty;
                                    meterId = readOut.MeterID(communications.ResponseSignOn).ToString().Substring(communications.ResponseSignOn.IndexOf("/") + 1);
                                    if (fileText.ToUpper().Contains("LGC"))
                                        meterId = meterId.ToString().Substring(13, 16).Trim();
                                    else
                                        meterId = meterId.Substring(4, meterId.Length - 4).Trim();
                                    MeterIdSet(simIndex, meterId);
                                    //dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, simIndex].Value = (readOut.MeterID(communications.ResponseSignOn)).Substring(13, 16);
                                    Application.DoEvents();
                                }
                            }
                            else
                            {
                                this.RightStatusMessageAsync = String.Empty;
                                //this.StatusMessageAsync = "Readout Successful";
                                EnableStopTimer();
                                Application.DoEvents();
                                if (fileText.ToUpper().Contains("LGC"))
                                    SaveDataForSPhaseIEC(fileText, readOut.MeterID(communications.ResponseSignOn)); // Story - 349654 - Meter Id is passed as parameter to append in the file name
                                else
                                    SaveData(fileText, readOut.MeterID(communications.ResponseSignOn)); // Story - 349654 - Meter Id is passed as parameter to append in the file name
                            }
                        }
                    }
                    else
                    {
                        this.StatusMessageAsync = "BCC not matched";
                        Application.DoEvents();
                    }
                }
                else
                {
                    if (IsAborted)
                    {
                        this.StatusMessageAsync = "User Aborted";
                        Application.DoEvents();
                        EnableReadControls();
                    }
                    else if (TotalCheckCount() > 0)
                    {
                        this.StatusMessageAsync = "Data not available in meter";
                        Application.DoEvents();
                    }
                }
                EnableReadControls();
                lngGridViewReadControl1.Enabled = true;
                this.RightStatusMessage = String.Empty;
                this.RightStatusMessageAsync = String.Empty;
                flag = true;
            }

            catch (Exception ex)
            {
                this.StatusMessageAsync = "Readout Failure";
                logger.Log(LOGLEVELS.Error, "Error while Reading PUMA Meter.", ex);
                flag = false;
            }
            finally
            {
                EnableReadControls();
                isMeterReading = false;
                Application.DoEvents();
                SetCursor(Cursors.Default);
                EnableStopTimer();
                setConnectionDetail(false);
            }
            return flag;
        }
        private void EnableStartTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStartTimer));
            }
            else
            {
                StartProgressBarTimer();
            }
        }
        /// <summary>
        /// enable stop timer for different thread
        /// </summary>
        private void EnableStopTimer()
        {
            if (statusStrip.InvokeRequired)
            {
                statusStrip.Invoke(new MethodInvoker(EnableStopTimer));
            }
            else
            {
                StopProgressBarTimer();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cursor"></param>
        private void SetCursor(Cursor cursor)
        {
            if (this.InvokeRequired)
            {
                SetCursorCallBack callBack = new SetCursorCallBack(SetCursor);
                Invoke(callBack, cursor);
            }
            else
            {
                this.Cursor = cursor;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void EnableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(EnableAbort));
            }
            else
            {
                btnAbort.Enabled = true;
            }
        }
        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableReadControls()
        {
            if (grpReadoptions.InvokeRequired)
            {
                grpReadoptions.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                grpReadoptions.Enabled = true;
            }
            if (btnRead.InvokeRequired)
            {
                btnRead.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnRead.Enabled = true;
            }
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnAbort.Enabled = false;
            }
            if (btnCancel.InvokeRequired)
            {
                btnCancel.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnCancel.Enabled = true;
            }
            if (lngGridViewReadControl1.InvokeRequired)
            {
                lngGridViewReadControl1.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                lngGridViewReadControl1.Enabled = true;
            }

            if (chkLoadSurvey.Checked)
            {
                if (grpLoadSurvey.InvokeRequired)
                {
                    grpLoadSurvey.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    grpLoadSurvey.Enabled = true;
                }

            }

            if (this.Parent != null)
            {
                MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;

                if (menuStrip.InvokeRequired)
                {
                    menuStrip.Invoke(new MethodInvoker(EnableReadControls));
                }
                else
                {
                    DataAcquisition.Enabled = true;
                    Configuration.Enabled = true;
                    setConnectionDetail(false);
                    EnableStopTimer();
                }
            }

        }
        /// <summary>
        /// Disable abort button .
        /// </summary>
        private void DisableAbort()
        {
            if (btnAbort.InvokeRequired)
            {
                btnAbort.Invoke(new MethodInvoker(DisableAbort));
            }
            else
            {
                btnAbort.Enabled = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="style"></param>
        private void SetDataGridStyle(System.Enum profileId, DataGridViewCellStyle style)
        {
            if (this.InvokeRequired)
            {
                SetDataGridStyleCallback callBack = new SetDataGridStyleCallback(SetDataGridStyle);
                Invoke(callBack, profileId, style);
            }
            else
            {
                lngGridViewReadControl1.SetColour(profileId, style);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="message"></param>
        private void SetDataGridStatus(System.Enum profileId, string message)
        {
            if (this.InvokeRequired)
            {
                SetDataGridStatusCallback callBack = new SetDataGridStatusCallback(SetDataGridStatus);
                Invoke(callBack, profileId, message);
            }
            else
            {
                lngGridViewReadControl1.SetStatus(profileId, message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="?"></param>
        /// <param name="message"></param>
        private void SetGridRowAttributes(Color color, System.Enum profile, string message)
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = color;
            SetDataGridStyle(profile, style);
            SetDataGridStatus(profile, message);
        }
        /// <summary>
        /// 
        /// </summary>
        private void EnableCancel()
        {
            if (btnCancel.InvokeRequired)
            {
                btnCancel.Invoke(new MethodInvoker(EnableCancel));
            }
            else
            {
                btnCancel.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisableCancel()
        {
            if (btnCancel.InvokeRequired)
            {
                btnCancel.Invoke(new MethodInvoker(DisableCancel));
            }
            else
            {
                btnCancel.Enabled = false;
            }
        }
        private void oneToManyGSM_CheckedChanged(object sender, EventArgs e)
        {
            dgvMeterIdAndSim.Enabled = true;
            selectAll.Enabled = true;
            grpSimNumber.Enabled = false;
        }

        private void oneToOneGSM_CheckedChanged(object sender, EventArgs e)
        {
            dgvMeterIdAndSim.Enabled = false;
            selectAll.Enabled = false;
            grpSimNumber.Enabled = true;
        }
        /// <summary>
        /// on click , selects all check boxes or unselects all check boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// This method is used to update GSM index in cross thread operation
        /// </summary>
        /// <param name="simIndex"></param>
        /// <param name="meterID"></param>
        private void MeterIdSet(int simIndex, string meterID)
        {
            if (dgvMeterIdAndSim.InvokeRequired)
            {
                dgvMeterIdAndSim.Invoke(new IntMeterId(MeterIdSet), new object[] { simIndex, meterID });
            }
            else
            {
                dgvMeterIdAndSim[(int)dgvSimColumn.MeterID, simIndex].Value = meterID;
            }
        }
    }

}
