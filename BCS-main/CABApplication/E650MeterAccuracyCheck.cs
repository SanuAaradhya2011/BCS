#region NameSpaces
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using CABEntity;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Hunt.EPIC.Logging;
#endregion
namespace CABApplication
{
    /// <summary>
    /// Meter Accuracy Check 
    /// </summary>
    public partial class E650MeterAccuracyCheck : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private System.Resources.ResourceManager resourceMgr;
        private const string START = "Start";
        private const string STOP = "Stop";
        DateTime startDatetime ;
        private List<byte> meterId = null;
        private MeterAccuracyCheckEntity meterAccuracyCheckEntity;
        private Communication communication;
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        string MeterID = string.Empty;
        int securitymachanism = 0;
        string MeterItem = string.Empty;
        int MeterModelNo = 0;
        long InitializationCounter = 0;
        List<ProfileCommand> SMAccuracyCommand = new List<ProfileCommand>() { 
                                new ProfileCommand(07,"01.00.63.80.81.FF", 03 ),
                                new ProfileCommand(07,"01.00.63.80.81.FF", 02),
                                new ProfileCommand(07,"01.00.5E.5B.81.FF", 03),
                                new ProfileCommand(07,"01.00.5E.5B.81.FF", 02)
                            };
        Result result = new Result();
        bool isConnected = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650MeterAccuracyCheck).ToString());
        DataTypeFactory dataTypeFactory;
        StructureInfoManager infoManager;
        int ctRatio = 1;
        int ptRatio = 1;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public E650MeterAccuracyCheck()
        {
            InitializeComponent();

            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);

            // To fill the duration combobox.
            for (int i = 1; i < 61; i++)
            {
                cmbTestduration.Items.Add(i);
            }
            cmbTestduration.SelectedIndex = 0;
            // To create resource file for messages display.
            resourceMgr = new System.Resources.ResourceManager("CABApplication.E650MeterAccuracyCheck", System.Reflection.Assembly.GetExecutingAssembly());

            infoManager = new StructureInfoManager(new Serializer());
            dataTypeFactory = new DataTypeFactory();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        private void btnAccuracyCheckCancel_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == resourceMgr.GetString("Stop"))
            {
                int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("TestRunning"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (msgres != 6) return;
            }
            this.StatusMessage = "";
            SetConnectionDetail(false);
            this.Close();

        }       
        /// <summary>
        /// Meter Accuracy check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            ValidateParameters();
        }
        /// <summary>
        /// Closing form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterAccuracyCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            SetConnectionDetail(false);
        }        
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is used for clear all the controls when accuracy check is started.
        /// </summary>
        private void ValidateParameters()
        {
            if (btnStart.Text.Equals(START))
            {
                btnStart.Enabled = false;
                this.StatusMessage = string.Empty;
                btnStart.Text = STOP;
                cmbTestduration.Enabled = false;
                Application.DoEvents();
                lblduration.Visible = true;
                lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                //lblduration.Location = new System.Drawing.Point(300, 220);
                txtkVAhDelta.Text = string.Empty;
                txtkvarhLagDelta.Text = string.Empty;
                txtkvarhLeadDelta.Text = string.Empty;
                txtkWhDelta.Text = string.Empty;
                txtkVAhFinal.Text = string.Empty;
                txtkVAhInitial.Text = string.Empty;
                txtkvarhLagFinal.Text = string.Empty;
                txtkvarhLagInitial.Text = string.Empty;
                txtkvarhLeadFinal.Text = string.Empty;
                txtkvarhLeadInitial.Text = string.Empty;
                txtkWhFinal.Text = string.Empty;
                txtkWhInitial.Text = string.Empty;

                txtkVAhExportDelta.Text = string.Empty;
                txtkvarhLagExportDelta.Text = string.Empty;
                txtkvarhLeadExportDelta.Text = string.Empty;
                txtkWhExportDelta.Text = string.Empty;
                txtkVAhExportFinal.Text = string.Empty;
                txtkVAhExportInitial.Text = string.Empty;
                txtkvarhLagExportFinal.Text = string.Empty;
                txtkvarhLagExportInitial.Text = string.Empty;
                txtkvarhLeadExportFinal.Text = string.Empty;
                txtkvarhLeadExportInitial.Text = string.Empty;
                txtkWhExportFinal.Text = string.Empty;
                txtkWhExportInitial.Text = string.Empty;

                ctRatio = 1;
                ptRatio = 1;
                lblCTRatio.Text = "CT Ratio - ";
                lblPTRatio.Text = "PT Ratio - ";
                groupBoxCTPTRatio.Enabled = false;
                //txtReversekVAhDelta.Text = string.Empty;
                //txtReversekVAhInitial.Text = string.Empty;
                //txtReversekVAhFinal.Text = string.Empty;

                //txtReversekvarhLagInitial.Text = string.Empty;
                //txtReversekvarhLagDelta.Text = string.Empty;
                //txtReversekvarhLagFinal.Text = string.Empty;

                //txtReversekWhDelta.Text = string.Empty;
                //txtReversekWhFinal.Text = string.Empty;
                //txtReversekWhInitial.Text = string.Empty;

                //txtReversekvarhLeadFinal.Text = string.Empty;
                //txtReversekvarhLeadInitial.Text = string.Empty;
                //txtReversekvarhLeadDelta.Text = string.Empty;
                Application.DoEvents();
                // To start reading parameters on start of accuracy check.
                StartMeterAccuracyCheck();
                btnStart.Enabled = true;
            }
            else
            {
                btnStart.Enabled = false;
                this.StatusMessage = string.Empty;
                Application.DoEvents();
                // To start reading parameters on stop of accuracy check.
                StopAccuracyCheck();
                cmbTestduration.Enabled = true;
                btnStart.Enabled = true;
                groupBoxCTPTRatio.Enabled = true;
            }

        }
        /// <summary>
        /// Added for calculating the elasped time after the timer is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Duration_Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan dtDuration = DateTime.Now - startDatetime;

            if (cmbTestduration.Text != "")
            {
                if (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) + 3 == Convert.ToInt32(cmbTestduration.Text) * 60)
                {
                    //Duration_Timer.Stop();
                    btnStart_Click(this, e);
                    Duration_Timer.Stop();
                    dtDuration = DateTime.Now - startDatetime;

                    if ((((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) > Convert.ToInt32(cmbTestduration.Text) * 60) || (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) < Convert.ToInt32(cmbTestduration.Text) * 60))
                    {
                        if (Convert.ToInt32(cmbTestduration.Text) < 60)
                        {
                            lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + Convert.ToInt32(cmbTestduration.Text).ToString(resourceMgr.GetString("Zero")) + ":" + resourceMgr.GetString("Zero");
                        }
                        else
                        {
                            lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("One") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                        }
                    }
                    else
                    {
                        //dtDuration = DateTime.Now - startDatetime;
                        lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
                    }
                }
                else
                {
                    dtDuration = DateTime.Now - startDatetime;
                    lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
                }
            }
        }
        /// <summary>
        /// Final reading
        /// </summary>
        private void StopAccuracyCheck()
        {
            Duration_Timer.Stop();
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            bool isConnected = false;
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            try
            {
                btnStart.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                SetConnectionDetail(true);
                SmartMeterCommuincation();
                //ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                //result = communication.OpenSession();
                //if (result.ErrorCode == CommunicationErrorType.Success)
                //{
                //    isConnected = true;
                //    SetConnectionDetail(true);
                //    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        //string idLength = result.RecieveDataBuffer[1].ToString("00");
                        //int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        //meterId = new List<byte>();
                        //meterId = result.RecieveDataBuffer.GetRange(2, index);
                        lstProfileCommands = GetProfileCommandEntity();
                        List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                        {
                            return profileCommandEntity.TagNumber == (byte)ProfileId.MeterAccuracyCheck
                            && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                            profileCommandEntity.MeterModelNumber == 0);
                        });

                       
                        try
                        {
                            for (int index = 0; index < profileReadCommands.Count; index++)
                            {
                                profileReadCommands[index].Action = ActionType.READ;
                                profileReadCommands[index].MeterID = meterId;
                                if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
                                    result = communication.Send(SMAccuracyCommand[index]);
                                else
                                result = communication.Send(profileReadCommands[index]);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    resultData.Append(String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                           + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                  + String.Format("{0:X2}", profileReadCommands[index].Attribute));
                                    for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                    {
                                        resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                    }
                                    resultData.AppendLine();
                                }
                            }
                                Common.EntityMapper.MeterAccuracyCheck mapperMeterAccuracy = new Common.EntityMapper.MeterAccuracyCheck();
                                //Append redundent one line in data to make sure that it will work like file data in entityGenerator.GetProfileWiseEntityList() method.
                                resultData.AppendLine(" ");
                                List<ProfileData> meterAccuracyData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), false);
                                List<MeterAccuracyCheckEntity> meterAccuracyCheckEntity = mapperMeterAccuracy.GetData(meterAccuracyData);
                                Application.DoEvents();
                                if (meterAccuracyCheckEntity.Count > 0)
                                {
                                    if (rdbApplyRatio.Checked) ApplyCTPTRatios(meterAccuracyCheckEntity[0]);

                                    DisplayFinalReading(meterAccuracyCheckEntity[0]);
                                    cmbTestduration.Enabled = true;
                                    DisplayDeltaValues();
                                    btnStart.Text = START;
                                    Duration_Timer.Stop();
                                    this.Cursor = Cursors.Default;
                                }
                            }

                        catch (Exception ex)    //Exception log for catch block
                        {
                            btnStart.Text = START;
                            cmbTestduration.Enabled = true;
                            this.Cursor = Cursors.Default;
                            Duration_Timer.Stop();
                            logger.Log(LOGLEVELS.Error, "StopAccuracyCheck()", ex);
                            //MessageBox.Show("Meter Accuracy not supported", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }

              //  }
                if (result.ErrorCode != CommunicationErrorType.Success)
                {                   
                  
                    btnStart.Text = START;
                    cmbTestduration.Enabled = true;
                    this.Cursor = Cursors.Default;
                    Duration_Timer.Stop();                    
                    if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                    {
                        this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        this.StatusMessage = resourceMgr.GetString("Failure");
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "StopAccuracyCheck()", ex);
                //MessageBox.Show("Meter Accuracy not supported", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                //if (isConnected)
                //{
                    communication.CloseSession();
                //}
                this.Cursor = Cursors.Default;
                btnStart.Enabled = true;
                Application.DoEvents();

            }
        }
        /// <summary>
        /// Meter Acccuracy check read
        /// </summary>
        private void StartMeterAccuracyCheck()
        {
           string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            try
            {
                //btnStart.Enabled = false;
                //this.Cursor = Cursors.WaitCursor;
                //ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                //startDatetime = DateTime.Now;
                //Duration_Timer.Start();
                //Result result = communication.OpenSession();
                // if (result.ErrorCode == CommunicationErrorType.Success)
                //{
                //    isConnected = true;
                    SetConnectionDetail(true);
                   SmartMeterCommuincation();
                   if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        
                        //string idLength = result.RecieveDataBuffer[1].ToString("00");
                       // int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                       //meterId = new List<byte>();
                        //meterId = result.RecieveDataBuffer.GetRange(2, index);
                        if (rdbApplyRatio.Checked) GetCTPTRatio();

                        lstProfileCommands = GetProfileCommandEntity();
                        List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                        {
                            return profileCommandEntity.TagNumber == (byte)ProfileId.MeterAccuracyCheck
                            && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                            profileCommandEntity.MeterModelNumber == 0);
                        });
                         
                        try
                        {
                            for (int index = 0; index < profileReadCommands.Count; index++)
                            {
                                profileReadCommands[index].Action = ActionType.READ;
                                profileReadCommands[index].MeterID = meterId;
                                if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
                                result = communication.Send(SMAccuracyCommand[index]);
                                else
                                result = communication.Send(profileReadCommands[index]);


                                if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer.Count > 0)
                                {
                                    resultData.Append(String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                           + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                  + String.Format("{0:X2}", profileReadCommands[index].Attribute));
                                    for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                    {
                                        resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                    }
                                    resultData.AppendLine();
                                }
                                else
                                {
                                    Duration_Timer.Stop();
                                    btnStart.Text = START;
                                    SetConnectionDetail(false);
                                    MessageBox.Show("Meter Accuracy not supported", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }

                                                  
                            Common.EntityMapper.MeterAccuracyCheck mapperMeterAccuracy = new Common.EntityMapper.MeterAccuracyCheck();
                            //Append redundent one line in data to make sure that it will work like file data in entityGenerator.GetProfileWiseEntityList() method.
                            resultData.AppendLine(" ");
                            List<ProfileData> meterAccuracyData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), false);
                            List<MeterAccuracyCheckEntity> meterAccuracyCheckEntity = mapperMeterAccuracy.GetData(meterAccuracyData);
                            Application.DoEvents();
                            if (meterAccuracyCheckEntity.Count > 0)
                            {
                                if (rdbApplyRatio.Checked) ApplyCTPTRatios(meterAccuracyCheckEntity[0]);
                               
                                DisplayInitialReading(meterAccuracyCheckEntity[0]);
                                // HTCT Specific
                                string signatureInfo = communication.GetSignatureData().ToUpper();
                                if (signatureInfo.ToUpper().Contains("HM") || signatureInfo.ToUpper().Contains("SM") || signatureInfo.ToUpper().Contains("sm"))
                                {
                                    DisplayUnit(true);
                                }
                               
                                else
                                {
                                    DisplayUnit(false);
                                }
                                // Timer starts after getting the first response from meter.                               
                                startDatetime = DateTime.Now;
                                Duration_Timer.Start();
                            }
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            Duration_Timer.Stop();
                            btnStart.Text = START;
                            SetConnectionDetail(false);
                            logger.Log(LOGLEVELS.Error, "StartMeterAccuracyCheck()", ex);
                           // MessageBox.Show("Meter Accuracy not supported", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                //}
                if (result.ErrorCode != CommunicationErrorType.Success)
                {
                    btnStart.Text = START;
                    Duration_Timer.Stop();                    
                    this.Cursor = Cursors.Default;
                    SetConnectionDetail(false);
                    if (result.ErrorCode == CommunicationErrorType.PortInvalid)
                    {
                        this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                    else
                    {
                        this.StatusMessage = resourceMgr.GetString("Failure");
                        MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Duration_Timer.Stop();
                btnStart.Text = START;
                this.StatusMessage = resourceMgr.GetString("Failure");
                SetConnectionDetail(false);
                MessageBox.Show("Meter Accuracy not supported", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Log(LOGLEVELS.Error, "StartMeterAccuracyCheck()", ex);
            }
            finally
            {
                //if (isConnected)
                //{
                    communication.CloseSession();
              //  }
                this.Cursor = Cursors.Default;                
                btnStart.Enabled = true;
                SetConnectionDetail(false);
               //communication.CloseSession();
                Application.DoEvents();
            }
        }
        
        /// <summary>
        /// Apply CT and PT ratios to energy values
        /// </summary>
        /// <param name="meterAccuracyCheckEntity"></param>
        private void ApplyCTPTRatios(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            decimal kvah = 0, kwh = 0, kvarhLag = 0, kvarhLead = 0, exportKvah = 0, exportKwh = 0, exportKvarhLag = 0, exportKvarhLead = 0;

            if (decimal.TryParse(meterAccuracyCheckEntity.KWh, out kwh))
                meterAccuracyCheckEntity.KWh = (kwh * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.KVAh, out kvah))
                meterAccuracyCheckEntity.KVAh = (kvah * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.KvarhLag, out kvarhLag))
                meterAccuracyCheckEntity.KvarhLag = (kvarhLag * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.KvarhLead, out kvarhLead))
                meterAccuracyCheckEntity.KvarhLead = (kvarhLead * ctRatio * ptRatio).ToString();

            if (decimal.TryParse(meterAccuracyCheckEntity.ExportKWh, out exportKwh))
                meterAccuracyCheckEntity.ExportKWh = (exportKwh * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.ExportKVAh, out exportKvah))
                meterAccuracyCheckEntity.ExportKVAh = (exportKvah * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.ExportKvarhLag, out exportKvarhLag))
                meterAccuracyCheckEntity.ExportKvarhLag = (exportKvarhLag * ctRatio * ptRatio).ToString();
            if (decimal.TryParse(meterAccuracyCheckEntity.ExportKvarhLead, out exportKvarhLead))
                meterAccuracyCheckEntity.ExportKvarhLead = (exportKvarhLead * ctRatio * ptRatio).ToString();
        }

        /// <summary>
        /// Gets the CT and PT Ratios of the meter in Ad-Hoc mode
        /// </summary>
        private void GetCTPTRatio()
        {
            try
            {
                result = communication.OpenSession();

                ProfileCommand profileCommand = new ProfileCommand(01, "01.00.00.04.02.FF", 02);
                result = communication.Send(profileCommand);
                if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength > 0)
                {
                    var reader = new ExtendedBinaryReader(new System.IO.MemoryStream(result.RecieveDataBuffer.ToArray()), true);
                    int info = Convert.ToInt32(reader.ReadByte());
                    DataType dataType = dataTypeFactory.GetDataType(infoManager.GetUnitInfo(info));
                    string value = dataType.GetValue(reader, new CAB.Parser.Entity.DataElementConfiguration());
                    if (!int.TryParse(value, out ctRatio))
                    {
                        ctRatio = 1;
                    }
                }
                else
                {
                    lblCTRatio.Text += "(default) ";
                }

                profileCommand = new ProfileCommand(01, "01.00.00.04.03.FF", 02);
                result = communication.Send(profileCommand);
                if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength > 0)
                {
                    var reader = new ExtendedBinaryReader(new System.IO.MemoryStream(result.RecieveDataBuffer.ToArray()), true);
                    int info = Convert.ToInt32(reader.ReadByte());
                    DataType dataType = dataTypeFactory.GetDataType(infoManager.GetUnitInfo(info));
                    string value = dataType.GetValue(reader, new CAB.Parser.Entity.DataElementConfiguration());
                    if (!int.TryParse(value, out ptRatio))
                    {
                        ptRatio = 1;
                    }
                }
                else
                {
                    lblPTRatio.Text += "(default) ";
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GetCTPTRatio", ex);
            }
            finally
            {
                lblCTRatio.Text += ctRatio.ToString();
                lblPTRatio.Text += ptRatio.ToString();
            }
        }

        public List<string> GetSecurityKeys(string meterid)
        {

            string source = string.Empty;
            string result = string.Empty;
            string errorcode = string.Empty;
            int rowindex = 0;
            XmlDocument doc = new XmlDocument();
            List<string> SecurityKeyDetails = new List<string>();
            //string fileNames = string.Concat(Path.GetDirectoryName(Application.ExecutablePath))+"\\"+"CABApplication.exe.config";
            try
            {
                //  string path = AppDomain.CurrentDomain.BaseDirectory + "\\XML\\EndDeviceSecurityResponse.xml";
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\EndDeviceSecurityResponse.xml";

                doc.Load(path);

                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name == "ResponseMessage")
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.Name == "Header")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Source")
                                    {
                                        source = node2.InnerText;

                                    }

                                }
                            }
                            if (node1.Name == "Reply")
                            {
                                foreach (XmlNode node2 in node1.ChildNodes)
                                {
                                    if (node2.Name == "Result")
                                    {
                                        result = node2.InnerText;

                                    }
                                    if (node2.Name == "Error")
                                    {
                                        foreach (XmlNode node3 in node2.ChildNodes)
                                        {
                                            if (node3.Name == "code")
                                            {

                                                errorcode = node2.InnerText;
                                            }

                                        }


                                    }

                                }
                            }
                        }
                }
                if (source == "command center" && result == "OK" && errorcode == "0.0")
                {
                    var doc2 = XDocument.Load(path);

                    var itemsList = (from c in doc2.Descendants("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity")
                                     select new
                                     {
                                         // item = query1.ElementAt(0),
                                         meterid = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}Names").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}name"),
                                         llsvalue = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}CustomAttributes").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}value"),
                                         secGlobalKey = doc2.Root.Elements("{http://iec.ch/TC57/2011/schema/message}Payload").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurityConfig").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}EndDeviceSecurity").Elements("{http://iec.ch/TC57/2011/EndDeviceSecurityConfig#}meterGlobalKey"),
                                     }).ToList();

                    int itemindex = 0;
                    foreach (var item in itemsList)
                    {
                        if (itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value == meterid)
                        {
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).meterid.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).llsvalue.ElementAt(itemindex).Value);
                            SecurityKeyDetails.Add(itemsList.ElementAt(itemindex).secGlobalKey.ElementAt(itemindex).Value);
                            itemindex++;
                            break;
                        }
                    }
                    return SecurityKeyDetails;


                }
                return null;

            }
            catch (UnauthorizedAccessException ex)    //Exception log for catch block
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
                logger.Log(LOGLEVELS.Error, "GetSecurityKeys(string meterid)", ex);
                return null;
            }

        }

        /// <summary>
        /// VBM - Used to Display unit for HTCT and LTCT meters
        /// </summary>
        /// <param name="isHTCT"></param>
        /// 
        public void SmartMeterCommuincation()
        {
            const int LLSKEYINDEX = 1;
            const int GLOBALKEYINDEX = 2;
            const int TEMPGLOBALKEYINDEX = 3;
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            if (ConfigSettings.GetValue("PortName").Contains(","))
                channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
            else
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
           // Result result = new Result();
            if (ConfigSettings.GetValue("ApplicationContext") == "03")
                channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter
            else
                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
            if (ConfigSettings.GetValue("ApplicationContext") == "03")
                if (channelInfo.SecurityMechanism == 0x00)
                {
                    result = communication.OpenSession();
                    // ************ Read meter ID Start
                    ProfileCommand profileCommand = new ProfileCommand(01, "0x00.0x00.0x60.0x01.0x00.0xFF", 02);
                    result = communication.Send(profileCommand);
                    communication.CloseSession();
                    if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        int index = 0;

                        index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        for (int i = 2; i < index + 2; i++)
                        {
                            MeterID += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                        }
                    }
                    // ************Read meter ID Close
                    result = communication.OpenSession();
                    // ************Read Invocation Counter start
                    profileCommand = new ProfileCommand(01, "0x00.0x00.0x2B.0x01.0x00.0xFF", 02);
                    if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (ConfigSettings.GetValue("SecurityMechanism") == "01")
                            profileCommand = new ProfileCommand(01, "00.00.2B.01.02.FF", 02);
                        else if (ConfigSettings.GetValue("SecurityMechanism") == "02")
                            profileCommand = new ProfileCommand(01, "00.00.2B.01.03.FF", 02);
                    }

                    result = communication.Send(profileCommand);
                    communication.CloseSession();
                    if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        int index = 0;
                        long InvoCountValue = 0;

                        byte[] incount = new byte[4];
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                            communication = new Communication(channelInfo);
                            securitymachanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                            string data = string.Empty;  //FormatData(result.RecieveDataBuffer.ToArray(), false);
                            int adbyteindex = 0;
                            for (int i = 1; i < index + 5; i++)
                            {
                                incount[adbyteindex++] = result.RecieveDataBuffer[i];

                            }
                            data = FormatData(incount, false);
                            if (data != null) InvoCountValue = Convert.ToInt64(data);
                            InitializationCounter = InvoCountValue + 1;

                            //List<string> SecurityKeyDetails = GetSecurityKeys(MeterID);
                            List<string> SecurityKeyDetails = Security_Key.SecurityKeyManager.GetSecurityKeys(MeterID, ConfigSettings.GetValue("PrivateKey"));
                            if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= GLOBALKEYINDEX)
                            {
                                ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[LLSKEYINDEX]);
                                ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[GLOBALKEYINDEX]);
                                ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[GLOBALKEYINDEX]);

                            }
                            
                            result = communication.OpenSessionCipher(InitializationCounter);

                            if ((result.ErrorCode != CommunicationErrorType.Success && result.ErrorCode != CommunicationErrorType.ConnectedDLMS) && SecurityKeyDetails != null && SecurityKeyDetails.Count > TEMPGLOBALKEYINDEX)
                            {
                                ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[LLSKEYINDEX]);
                                ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[TEMPGLOBALKEYINDEX]);
                                ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[TEMPGLOBALKEYINDEX]);
                                result = communication.OpenSessionCipher(InitializationCounter + 1);
                            }
                            
                            // Get smart meter model detail
                            profileCommand = new ProfileCommand(01, "00.00.60.80.08.FF", 02);
                            result = communication.Send(profileCommand);
                            if (result.RecieveDataBuffer != null && result.ErrorCode == CommunicationErrorType.Success)
                            {
                                int mindex = 0;

                                mindex = Convert.ToInt16(result.RecieveDataBuffer[1]);
                                for (int i = 2; i < mindex + 2; i++)
                                {
                                    MeterItem += Convert.ToChar(result.RecieveDataBuffer[i]).ToString();

                                }

                                if (MeterItem.Contains("SM0310"))
                                {
                                    MeterModelNo = NamePlateConstants.SmartM_Cipher_WCM; //Smart meter 3 ph WCM 35.

                                }
                                else if (MeterItem.Contains("SM0405"))
                                {
                                    MeterModelNo = NamePlateConstants.SmartM_Cipher_LTCT; //Smart meter 3 ph LTCT 34.

                                }
                                else if (MeterItem.Contains("SM0110"))
                                {

                                    MeterModelNo = NamePlateConstants.SmartM_Cipher_1PH; //Smart meter 1 ph 37.

                                }
                            }
                        }
                    }

                }
            // ************Read Invocation Counter Close
            if (ConfigSettings.GetValue("ApplicationContext") != "03")
                result = communication.OpenSession();
        
        
        }

        public static string FormatData(byte[] buffer, bool bUnsignFlag)
        {
            StringBuilder sb = new StringBuilder();

            bool bSignFlag = false;
            Int64 tempVal = 0;
            for (int i = 0; i < buffer.Length; i++)
            {

                if (buffer[0] > 127)
                {

                    if (buffer.Length > 1)
                    {
                        if (bUnsignFlag) bSignFlag = true;

                    }
                }
                sb.Append(buffer[i].ToString("X2"));
            }

            if (bSignFlag == true)
            {
                if (buffer.Length == 4)
                {
                    tempVal = Convert.ToInt64("FFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else if (buffer.Length == 8)
                {
                    tempVal = Convert.ToInt64("FFFFFFFFFFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }
                else
                {
                    tempVal = Convert.ToInt32("FFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                    return "-" + tempVal.ToString();
                }

            }
            else
            {
                return Convert.ToInt64(sb.ToString(), 16).ToString();
            }
        }   
      private void DisplayUnit(bool isHTCT)
        {
            lblActiveEnergyUnit.Visible = true;
            lblApparentEnergyUnit.Visible = true;
            lblReactiveLagUnit.Visible = true;
            lblReactiveLeadUnit.Visible = true;

            lblActiveEnergyExportUnit.Visible = true;
            lblApparentEnergyExportUnit.Visible = true;
            lblReactiveLagExportUnit.Visible = true;
            lblReactiveLeadExportUnit.Visible = true;

            if (isHTCT)
            {
               lblActiveEnergyUnit.Text = "MWh";
                lblApparentEnergyUnit.Text = "MVAh";
                lblReactiveLagUnit.Text = "MVArh";
                lblReactiveLeadUnit.Text = "MVArh";

                lblActiveEnergyExportUnit.Text = "MWh";
                lblApparentEnergyExportUnit.Text = "MVAh";
                lblReactiveLagExportUnit.Text = "MVArh";
                lblReactiveLeadExportUnit.Text = "MVArh";
            }
            if (MeterModelNo == NamePlateConstants.SmartM_Cipher_1PH || MeterModelNo == NamePlateConstants.SmartM_Cipher_HTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_LTCT || MeterModelNo == NamePlateConstants.SmartM_Cipher_WCM)
            {
                lblActiveEnergyUnit.Text = "KWh";
                lblApparentEnergyUnit.Text = "KVAh";
                lblReactiveLagUnit.Text = "KVArh";
                lblReactiveLeadUnit.Text = "KVArh";

                lblActiveEnergyExportUnit.Text = "KWh";
                lblApparentEnergyExportUnit.Text = "KVAh";
                lblReactiveLagExportUnit.Text = "KVArh";
                lblReactiveLeadExportUnit.Text = "KVArh";
            }
        }
        /// <summary>
        /// Filll initial Data to coiltrols 
        /// </summary>
        /// <param name="meterAccuracyCheckEntity"></param>
        private void DisplayInitialReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            //Normal Energies 
            txtkVAhInitial.Text = meterAccuracyCheckEntity.KVAh.ToString();
            txtkvarhLagInitial.Text = meterAccuracyCheckEntity.KvarhLag.ToString();
            txtkvarhLeadInitial.Text = meterAccuracyCheckEntity.KvarhLead.ToString();
            txtkWhInitial.Text = meterAccuracyCheckEntity.KWh.ToString();

            ////Reverse Energies 
            //txtReversekVAhInitial.Text = meterAccuracyCheckEntity.ReversekVAh.ToString();
            //txtReversekWhInitial.Text = meterAccuracyCheckEntity.ReversekWh.ToString();
            //txtReversekvarhLagInitial.Text = meterAccuracyCheckEntity.ReversekvarhLag.ToString();
            //txtReversekvarhLeadInitial.Text = meterAccuracyCheckEntity.ReversekvarhLead.ToString();

            //Export Energies 
            txtkVAhExportInitial.Text = meterAccuracyCheckEntity.ExportKVAh.ToString();
            txtkvarhLagExportInitial.Text = meterAccuracyCheckEntity.ExportKvarhLag.ToString();
            txtkvarhLeadExportInitial.Text = meterAccuracyCheckEntity.ExportKvarhLead.ToString();
            txtkWhExportInitial.Text = meterAccuracyCheckEntity.ExportKWh.ToString();

        }
        /// <summary>
        /// Filll final data to controls
        /// </summary>
        /// <param name="meterAccuracyCheckEntity"></param>
        private void DisplayFinalReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            //Normal Energies 
            txtkVAhFinal.Text = meterAccuracyCheckEntity.KVAh.ToString();
            txtkvarhLagFinal.Text = meterAccuracyCheckEntity.KvarhLag.ToString();
            txtkvarhLeadFinal.Text = meterAccuracyCheckEntity.KvarhLead.ToString();
            txtkWhFinal.Text = meterAccuracyCheckEntity.KWh.ToString();

            ////Reverse Energies 
            //txtReversekVAhFinal.Text = meterAccuracyCheckEntity.ReversekVAh.ToString();
            //txtReversekWhFinal.Text = meterAccuracyCheckEntity.ReversekWh.ToString();
            //txtReversekvarhLagFinal.Text = meterAccuracyCheckEntity.ReversekvarhLag.ToString();
            //txtReversekvarhLeadFinal.Text = meterAccuracyCheckEntity.ReversekvarhLead.ToString();

            //Export Energies 
            txtkVAhExportFinal.Text = meterAccuracyCheckEntity.ExportKVAh.ToString();
            txtkvarhLagExportFinal.Text = meterAccuracyCheckEntity.ExportKvarhLag.ToString();
            txtkvarhLeadExportFinal.Text = meterAccuracyCheckEntity.ExportKvarhLead.ToString();
            txtkWhExportFinal.Text = meterAccuracyCheckEntity.ExportKWh.ToString();
        }
        /// <summary>
        /// Fill Delta data to controls 
        /// </summary>
        private void DisplayDeltaValues()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtkVAhFinal.Text) && !String.IsNullOrEmpty(txtkVAhInitial.Text))
                {
                    txtkVAhDelta.Text = (Convert.ToDecimal(txtkVAhFinal.Text) - Convert.ToDecimal(txtkVAhInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLagFinal.Text) && !String.IsNullOrEmpty(txtkvarhLagInitial.Text))
                {
                    txtkvarhLagDelta.Text = (Convert.ToDecimal(txtkvarhLagFinal.Text) - Convert.ToDecimal(txtkvarhLagInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLeadFinal.Text) && !String.IsNullOrEmpty(txtkvarhLeadInitial.Text))
                {
                    txtkvarhLeadDelta.Text = (Convert.ToDecimal(txtkvarhLeadFinal.Text) - Convert.ToDecimal(txtkvarhLeadInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkWhFinal.Text) && !String.IsNullOrEmpty(txtkWhInitial.Text))
                {
                    txtkWhDelta.Text = (Convert.ToDecimal(txtkWhFinal.Text) - Convert.ToDecimal(txtkWhInitial.Text)).ToString();
                }


                //if (!String.IsNullOrEmpty(txtReversekVAhFinal.Text) && !String.IsNullOrEmpty(txtReversekVAhInitial.Text))
                //{
                //    txtReversekVAhDelta.Text = (Convert.ToDecimal(txtReversekVAhFinal.Text) - Convert.ToDecimal(txtReversekVAhInitial.Text)).ToString();
                //}
                //if (!String.IsNullOrEmpty(txtReversekvarhLagFinal.Text) && !String.IsNullOrEmpty(txtReversekvarhLagInitial.Text))
                //{
                //    txtReversekvarhLagDelta.Text = (Convert.ToDecimal(txtReversekvarhLagFinal.Text) - Convert.ToDecimal(txtReversekvarhLagInitial.Text)).ToString();
                //}
                //if (!String.IsNullOrEmpty(txtReversekvarhLeadFinal.Text) && !String.IsNullOrEmpty(txtReversekvarhLeadInitial.Text))
                //{
                //    txtReversekvarhLeadDelta.Text = (Convert.ToDecimal(txtReversekvarhLeadFinal.Text) - Convert.ToDecimal(txtReversekvarhLeadInitial.Text)).ToString();
                //}
                //if (!String.IsNullOrEmpty(txtReversekWhFinal.Text) && !String.IsNullOrEmpty(txtReversekWhInitial.Text))
                //{
                //    txtReversekWhDelta.Text = (Convert.ToDecimal(txtReversekWhFinal.Text) - Convert.ToDecimal(txtReversekWhInitial.Text)).ToString();
                //}

                if (!String.IsNullOrEmpty(txtkVAhExportFinal.Text) && !String.IsNullOrEmpty(txtkVAhExportInitial.Text))
                {
                    txtkVAhExportDelta.Text = (Convert.ToDecimal(txtkVAhExportFinal.Text) - Convert.ToDecimal(txtkVAhExportInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLagExportFinal.Text) && !String.IsNullOrEmpty(txtkvarhLagExportInitial.Text))
                {
                    txtkvarhLagExportDelta.Text = (Convert.ToDecimal(txtkvarhLagExportFinal.Text) - Convert.ToDecimal(txtkvarhLagExportInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLeadExportFinal.Text) && !String.IsNullOrEmpty(txtkvarhLeadExportInitial.Text))
                {
                    txtkvarhLeadExportDelta.Text = (Convert.ToDecimal(txtkvarhLeadExportFinal.Text) - Convert.ToDecimal(txtkvarhLeadExportInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkWhExportFinal.Text) && !String.IsNullOrEmpty(txtkWhExportInitial.Text))
                {
                    txtkWhExportDelta.Text = (Convert.ToDecimal(txtkWhExportFinal.Text) - Convert.ToDecimal(txtkWhExportInitial.Text)).ToString();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayDeltaValues()", ex);
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
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }
        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
        #endregion


    }
}
