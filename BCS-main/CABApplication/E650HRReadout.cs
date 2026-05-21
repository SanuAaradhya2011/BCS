#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common.EntityMapper;
using CAB.BLL;
using CAB.Entity;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Hunt.EPIC.Logging;
#endregion

namespace CABApplication
{
    /// <summary>
    /// Snap Read is continious instantaneous readout directly from meter
    /// </summary>
    public partial class E650HRReadout : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private Communication communication;
        private GenerateEntity entityGenerator;
        private List<byte> meterId = null;
        private bool isSnapReadRuning = false;
        private bool isSnapReadStopped = false;
        private DataSet snapReadDataForGrid = null;
        private DataSet instantData = null;
        private const string ReadoutFailure = "Readout Failure.";
        private const string CumPowerFailureForInstant = "Cumulative Power-Failure Duration";
        private const string CUMPOTENTIALFAILRPHASE = "Cumulative Period Of Potential Fail - R Phase";
        private const string CUMPOTENTIALFAILYPHASE = "Cumulative Period Of Potential Fail - Y Phase";
        private const string CUMPOTENTIALFAILBPHASE = "Cumulative Period Of Potential Fail - B Phase";
        private const string CUMCURRENTFAILRPHASE = "Cumulative Period Of Current Fail - R Phase";
        private const string CUMCURRENTFAILYPHASE = "Cumulative Period Of Current Fail - Y Phase";
        private const string CUMCURRENTFAILBPHASE = "Cumulative Period Of Current Fail - B Phase";
        private const string DateFormatForInScumPowFail = "dd : hh : mm";
        private const string DateFormatForElapsedTime = "MM:SS";
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private const int DescriptionWidth = 230;
        private const int OBISCodeWidth = 100;
        private const int ClassIDWidth = 55;
        private const int AttributeWidth = 55;
        private const int ValueWidth = 130;
        private const int UnitWidth = 130;
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;
        private General mapperGeneral;
        private const string DateFormatForInScumPowFailDDDDHH = "dddd : hh";
        string MeterID = string.Empty;
        int securitymachanism = 0;
        string MeterItem = string.Empty;
        int MeterModelNo = 0;
        long InitializationCounter = 0;
        private static readonly Hunt.EPIC.Logging.IGeneralLog logger = Hunt.EPIC.Logging.LogFactory.CreateGeneralLogger(typeof(E650HRReadout).ToString());
        Result result = new Result();
        DataTypeFactory dataTypeFactory;
        StructureInfoManager infoManager;
        int ctRatio = 1;
        int ptRatio = 1;
       
        #endregion

        #region Properties
        #endregion

        #region Constructor
        /// <summary>
        /// This is constructer
        /// </summary>
        public E650HRReadout()
        {
            InitializeComponent();
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            // channelInfo.ModemInfo = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            communication = new Communication(channelInfo);
            entityGenerator = new GenerateEntity();
            mapperGeneral = new General();

            infoManager = new StructureInfoManager(new Serializer());
            dataTypeFactory = new DataTypeFactory();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        /// <summary>
        /// when the form loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SnapRead_Load(object sender, EventArgs e)
        {
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];
            lngInstant.SetLabelText = "";
        }
        /// <summary>
        /// on read button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadSnapRead_Click(object sender, EventArgs e)
        {
            isSnapReadStopped = false;
            isSnapReadRuning = true;
            btnReadSnap.Enabled = false;
            btnHoldSnap.Enabled = true;
            btnCancelSnap.Enabled = false;
            DataAcquisition.Enabled = false;
            Configuration.Enabled = false;
            label1.Visible = false;
            groupBoxCTPTRatio.Enabled = false;
            ctRatio = 1;
            ptRatio = 1;
            lblCTRatio.Text = "CT Ratio - ";
            lblPTRatio.Text = "PT Ratio - ";
            Thread readThread = new Thread(GenerateSnapRead);
            readThread.Start(SynchronizationContext.Current);
        }
        /// <summary>
        /// on hold button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHoldSnapRead_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "User Aborted.";
            btnHoldSnap.Enabled = false;
            btnCancelSnap.Enabled = true;
            this.Cursor = Cursors.Default;
            isSnapReadStopped = true;
            isSnapReadRuning = false;
            groupBoxCTPTRatio.Enabled = true;
            Application.DoEvents();
            SetConnectionDetail(false);
            EnableStopTimer();
        }
        /// <summary>
        /// on cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSnapRead_Click(object sender, EventArgs e)
        {
            SetConnectionDetail(false);
            EnableStopTimer();
            this.Close();
        }
        /// <summary>
        /// dont allow closing of form when reading is on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SnapRead_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSnapReadRuning)
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
                    SetConnectionDetail(false);
                    EnableStopTimer();
                }
                this.StatusMessage = "";
            }
        }

        /// <summary>
        /// progress bar timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBarTimer_Tick(object sender, EventArgs e)
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

        #endregion

        #region Private Methods

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
                    Invoke((MethodInvoker)delegate { lblCTRatio.Text += "(default) "; });
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
                    Invoke((MethodInvoker)delegate { lblPTRatio.Text += "(default) "; });
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "GetCTPTRatio", ex);
            }
            finally
            {
                Invoke((MethodInvoker)delegate 
                { 
                    lblCTRatio.Text += ctRatio.ToString();
                    lblPTRatio.Text += ptRatio.ToString();
                });
            }
        }

        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableStopSnapReadControl()
        {
            if (btnHoldSnap.InvokeRequired)
            {
                btnHoldSnap.Invoke(new MethodInvoker(EnableStopSnapReadControl));
            }
            else
            {
                btnHoldSnap.Enabled = true;
            }
        }
        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableStartSnapReadControl()
        {
            if (btnReadSnap.InvokeRequired)
            {
                btnReadSnap.Invoke(new MethodInvoker(EnableStartSnapReadControl));
            }
            else
            {
                btnReadSnap.Enabled = true;
            }
        }
        /// <summary>
        /// Enables the button on UI thread
        /// </summary>
        private void EnableReadControls()
        {
            if (btnHoldSnap.InvokeRequired)
            {
                btnHoldSnap.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnHoldSnap.Enabled = false;
            }

            if (btnReadSnap.InvokeRequired)
            {
                btnReadSnap.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnReadSnap.Enabled = true;
            }
            if (btnCancelSnap.InvokeRequired)
            {
                btnCancelSnap.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnCancelSnap.Enabled = true;
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
                    SetConnectionDetail(false);
                    EnableStopTimer();
                }
            }

        }
        /// <summary>
        /// Update snap Read grid in thread.
        /// </summary>
        private void UpdateSnapReadGrid()
        {

            if (lngInstant.InvokeRequired)
            {
                lngInstant.Invoke(new MethodInvoker(UpdateSnapReadGrid));
            }
            else
            {
                DataSet dataSet = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32("18"));
                if (dataSet != null)
                {
                    //  lngInstant.Data = dataSet;
                    lngInstant.Data = snapReadDataForGrid;

                    lngInstant.SetWidth("Descriptions", 230);
                    lngInstant.SetWidth("OBIS Code", 100);
                    lngInstant.SetWidth("Class ID", 55);
                    lngInstant.SetWidth("Attribute", 55);
                    lngInstant.SetWidth("Value", 130);
                    lngInstant.SetWidth("Unit", 130);
                }


            }
        }
        /// <summary>
        /// Generate snap Read 
        /// </summary>
        /// <param name="state"></param>
        private void GenerateSnapRead(object state)
        {
            // this.StatusMessageAsync = "Reading Snap Read data.....";
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            bool isResponseTimeout = false;
            Instantaneous mapperInstant = new Instantaneous();
            bool isConnected = false;
            try
            {
                EnableStartTimer();
                ClearSnapReadControls();
                SetConnectionDetail(true);
                SmartMeterCommuincation();

                lstProfileCommands = GetProfileCommandEntity();
                List<ProfileCommand> profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, ProfileId.HRProfile, NamePlateConstants.PumaLTE650Value);

                //SetConnectionDetail(true);
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    if (ConfigSettings.GetValue("ApplicationContext") != "03")
                    {
                        ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                        result = communication.OpenSession();
                        isConnected = true;
                        string signatureData = communication.GetSignatureData();
                        ConfigInfo.SignatureInfo = signatureData;

                        result = communication.Send(profileCommand);
                    }
                    if (isSnapReadStopped)
                    {
                        isSnapReadRuning = false;
                        EnableStartSnapReadControl();
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            string idLength = result.RecieveDataBuffer[1].ToString("00");
                            int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);

                            //if (rdbApplyRatio.Checked) 
                                GetCTPTRatio();

                            while (true)
                            {
                                resultData = new StringBuilder();
                                if (isSnapReadStopped)
                                {
                                    isSnapReadRuning = false;
                                    break;
                                }
                                try
                                {
                                    for (index = 0; index < profileReadCommands.Count; index++)
                                    {
                                        if (result.ErrorCode == CommunicationErrorType.Success && !isSnapReadStopped)
                                        {
                                            profileReadCommands[index].Action = ActionType.READ;
                                            profileReadCommands[index].MeterID = meterId;
                                            this.StatusMessageAsync = "Reading HR Profile data...";
                                            isResponseTimeout = false;                                            
                                            result = communication.Send(profileReadCommands[index]);
                                            if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                                            {
                                                resultData.Append(String.Format("{0:X2}", profileReadCommands[index].ClassId)
                                                       + profileReadCommands[index].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                                              + String.Format("{0:X2}", profileReadCommands[index].Attribute));
                                                for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                                {
                                                    resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                                }

                                                resultData.AppendLine("");
                                            }
                                            else if (result.ErrorCode == CommunicationErrorType.Success &&  result.RecieveDataBuffer.Count==0)
                                            {
                                                profileReadCommands = GetProfileCommandsToRead(lstProfileCommands, ProfileId.MeterAccuracyCheck, NamePlateConstants.PumaLTE650Value);
                                                index = 0;
                                                resultData = new StringBuilder();
                                            }
                                            else
	                                        {
                                                isSnapReadRuning = false;
                                                isResponseTimeout = true;
                                                break;
	                                        }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (!isSnapReadStopped)
                                    {
                                        //Append redundent one line in data to make sure that it will work like file data in entityGenerator.GetProfileWiseEntityList() method.
                                        resultData.AppendLine(" ");
                                        List<ProfileData> instantData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), false);
                                        List<DLMS650InstantaneousEntity> instantEntityList = mapperInstant.GetMappedEntity(instantData, null);
                                        Application.DoEvents();

                                        ConvertInstantEntityToDataSet(instantEntityList);
                                        UpdateInstantGrid();
                                        EnableStopSnapReadControl();
                                    }
                                    else
                                    {
                                        this.StatusMessageAsync = "HR Profile readout stopped.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //MessageBox.Show(result.ErrorCode.ToString(), "BCS");
                                }
                                finally
                                {

                                }
                            }
                            if (!isSnapReadRuning)
                            {
                                this.StatusMessage = string.Empty;
                                if (isResponseTimeout)
                                {
                                    isResponseTimeout = false;
                                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                                }

                            }
                        }
                        else
                        {
                            communication.CloseSession();
                            this.StatusMessageAsync = ReadoutFailure;
                        }
                    }
                    else
                    {
                        isSnapReadRuning = false;
                        this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                    }
                }
                else
                {
                    isSnapReadRuning = false;
                    this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);
                }
            }
            catch (Exception ex)
            {
                this.StatusMessageAsync = ReadoutFailure;
            }
            finally
            {
                //if (isConnected)
                //{
                    communication.CloseSession();
              //  }
                isSnapReadRuning = false;
                EnableReadControls();
                SetConnectionDetail(false);
                EnableStopTimer();
                groupBoxCTPTRatio.Enabled = true;
            }

        }
        /// <summary>
        /// Enable snap Read stop button when one reading cycle completed
        /// </summary>
        private void EnableStopSnapRead()
        {

            if (btnHoldSnap.InvokeRequired)
            {
                btnHoldSnap.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnHoldSnap.Enabled = true;
            }
            if (btnReadSnap.InvokeRequired)
            {
                btnReadSnap.Invoke(new MethodInvoker(EnableReadControls));
            }
            else
            {
                btnReadSnap.Enabled = false;
            }
            Application.DoEvents();
        }
        /// <summary>
        /// Clear snap Read controls befor readout.
        /// </summary>
        private void ClearSnapReadControls()
        {
            if (lngInstant.InvokeRequired)
            {
                lngInstant.Invoke(new MethodInvoker(ClearSnapReadControls));
            }
            else
            {
                lngInstant.Data = null;
            }

        }
        /// <summary>
        /// Convert Entity into DataSet
        /// </summary>
        /// <param name="instantEntityList"></param>
        private void ConvertInstantEntityToDataSet(List<DLMS650InstantaneousEntity> entityList)
        {

            instantData = new DataSet();
            DataTable instantTable = new DataTable();
            instantTable.Columns.Add(new DataColumn("Descriptions", typeof(System.String)));
            instantTable.Columns.Add(new DataColumn("OBIS Code", typeof(System.String)));
            instantTable.Columns.Add(new DataColumn("Class ID", typeof(System.String)));
            instantTable.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
            instantTable.Columns.Add(new DataColumn("Value", typeof(System.String)));
            instantTable.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            string dateValue = string.Empty;
            int counter = 0;
            string chkPowerOnOffDurationFormat = ConfigSettings.GetValue("ChkPowerOnOffDurationFormat");
            foreach (DLMS650InstantaneousEntity instantEntity in entityList)
            {
                counter = (int)instantEntity.InstantPowerDataIndex;
                DataRow instantRow = instantTable.NewRow();
                instantRow["Descriptions"] = instantEntity.InstantPowerColumnName;

                if (instantEntity.InstantPowerColumnName.ToUpper().Contains("DATE") || instantEntity.InstantPowerColumnName.ToUpper().Contains("TIME"))
                {
                    continue;
                }
                else
                {
                    string[] dataValue = instantEntity.InstantPowerColumnValue.Split('*');

                    if (rdbApplyRatio.Checked)
                        instantRow["Value"] = (Convert.ToDecimal(dataValue[0]) * ctRatio * ptRatio).ToString();
                    else
                        instantRow["Value"] = dataValue[0];

                    if (dataValue.Length > 1)
                    {
                        instantRow["Unit"] = dataValue[1];
                    }

                }
                instantRow["OBIS Code"] = instantEntity.InstantPowerObisCode;
                instantRow["Class ID"] = instantEntity.InstantPowerClassID;
                instantRow["Attribute"] = instantEntity.InstantPowerAttribute;

                if (instantRow["Value"].ToString().Contains("----")) continue;
                instantTable.Rows.Add(instantRow);
            }
            instantData.Tables.Add(instantTable);
            instantData.AcceptChanges();
        }

        /// <summary>
        /// Convert Minute value into MM:SS
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public string ConvertTimSpanToMMSS(TimeSpan timeSpan)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(timeSpan.Minutes.ToString("00"));
            strBuilder.Append(" : ");
            strBuilder.Append(timeSpan.Seconds.ToString("00"));
            return strBuilder.ToString();

        }

        /// <summary>
        /// Update instnat grid
        /// </summary>
        private void UpdateInstantGrid()
        {
            //Bind instant data set to datagrid.
            if (lngInstant.InvokeRequired)
            {
                lngInstant.Invoke(new MethodInvoker(UpdateInstantGrid));
            }
            else
            {
                
                    //string signatureInfo = communication.GetSignatureData().ToUpper();
                    //string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(MeterDataID);
                    //string chkValNumPowFail = ConfigSettings.GetValue("ChkNumPowFail");
                    //#region WB and UGVCL Specific
                    //if (chkValNumPowFail == "1")
                    //{
                    //    DataRow[] rowPowFailCount = instantData.Tables[0].Select("Descriptions = 'Number of Power-Failures'");
                    //    if (rowPowFailCount != null && rowPowFailCount.Length > 0)
                    //    {
                    //        instantData.Tables[0].Rows.Remove(rowPowFailCount[0]);
                    //        instantData.AcceptChanges();
                    //    }
                    //}
                   //#endregion

                    //#region TNEB Model specific check to remove Reverse KWH
                    //// [ReverseKWH_Remove]
                    //if (signatureInfo.ToUpper().Contains("TN"))
                    //{
                    //    DataRow[] reverseKWH = instantData.Tables[0].Select("Descriptions = 'Reverse KWH'");

                    //    if (reverseKWH != null && reverseKWH.Length > 0)
                    //    {
                    //        instantData.Tables[0].Rows.Remove(reverseKWH[0]);
                    //        instantData.AcceptChanges();
                    //    }
                    //}
                    //#endregion

                    //#region HTCT Specific
                    //if (signatureInfo.ToUpper().Contains("HM"))
                    //{
                    //    foreach (DataRow dataRow in instantData.Tables[0].Rows)
                    //    {
                    //        string rowDescription = dataRow["Descriptions"].ToString();

                    //        if (rowDescription.Contains("kW"))
                    //            dataRow["Descriptions"] = rowDescription.Replace("kW", "MW");
                    //        else if (rowDescription.Contains("kVA"))
                    //            dataRow["Descriptions"] = rowDescription.Replace("kVA", "MVA");
                    //        else if (rowDescription.Contains("KVA"))
                    //            dataRow["Descriptions"] = rowDescription.Replace("KVA", "MVA");
                    //        else if (rowDescription.Contains("kvar"))
                    //            dataRow["Descriptions"] = rowDescription.Replace("kvar", "Mvar");
                    //        instantData.AcceptChanges();
                    //    }
                    //}
                    //#endregion
                    lngInstant.Data = instantData;
                    lngInstant.Refresh();
                    lngInstant.Show();
                    lngInstant.SetWidth("Descriptions", DescriptionWidth);
                    lngInstant.SetWidth("OBIS Code", OBISCodeWidth);
                    lngInstant.SetWidth("Class ID", ClassIDWidth);
                    lngInstant.SetWidth("Attribute", AttributeWidth);
                    lngInstant.SetWidth("Value", ValueWidth);
                    lngInstant.SetWidth("Unit", UnitWidth);

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
            string mode;
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (connected)
            {
                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
            else
            {
                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstProfileCommands"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="meterModelNumber"></param>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandsToRead(List<ProfileCommand> lstProfileCommands, ProfileId selectedProfile, int meterModelNumber)
        {
            List<ProfileCommand> profileReadCommands = null;
            profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
            {
                return profileCommandEntity.TagNumber == (int)selectedProfile
                && (profileCommandEntity.ClassId != 0xFF)
                && (profileCommandEntity.MeterModelNumber == meterModelNumber ||
                profileCommandEntity.MeterModelNumber == 0);
            });
            return profileReadCommands;
        }

        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StartProgressBarTimer()
        {
            statusStrip.Visible = true;
            progressBarTimer.Enabled = true;
        }
        /// <param name="progressBar"></param>
        /// <param name="statusLabel"></param>
        public void StopProgressBarTimer()
        {
            statusStrip.Visible = false;
            progressBarTimer.Enabled = false;
        }

        /// <summary>
        /// enable start timer for different thread
        /// </summary>
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
        public void SmartMeterCommuincation()
        {
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

                            List<string> SecurityKeyDetails = GetSecurityKeys(MeterID);
                            if (SecurityKeyDetails != null && SecurityKeyDetails.Count >= 2)
                            {
                                ConfigSettings.ChangeNode("ModePassword", SecurityKeyDetails[1]);

                                ConfigSettings.ChangeNode("GlobalEncryptionKey", SecurityKeyDetails[2]);
                                ConfigSettings.ChangeNode("AuthenticationKey", SecurityKeyDetails[2]);

                            }


                            result = communication.OpenSessionCipher(InitializationCounter);
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
            catch (UnauthorizedAccessException ex)
            {
                //MessageBox.Show("Access permission issue. Please run as administrator.");
                return null;
            }

        }
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
        #endregion



    }
}
