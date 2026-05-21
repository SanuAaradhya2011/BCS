#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Common.EntityMapper;
using System.Collections;
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
using CAB.Framework.Entity;
using CAB.DALC.Data.DataServices;
using CAB.DALC.Data;
#endregion

namespace CABApplication
{
    /// <summary>
    /// Adhoc Read is directly from meter
    /// </summary>
    public partial class AdhocRead : MdiChildForm
    {      

        #region Constants and Variables
        private Communication communication;
       // private GenerateEntity entityGenerator;
        private List<byte> meterId = null;
        private bool isAdhocReadRuning = false;
        private bool isAdhocReadStopped = false;
        private DataSet AdhocReadDataForGrid = null;
        //private DataSet instantData = null;
        private const string ReadoutFailure = "Readout Failure.";       
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private const int DescriptionWidth = 200;
        private const int OBISCodeWidth = 100;
        private const int ClassIDWidth = 55;
        private const int AttributeWidth = 90;
        private const int ValueWidth = 130;
        private const int UnitWidth = 130;
        private ToolStripItem DataAcquisition;
        private ToolStripItem Configuration;         
        Result result = new Result();
        //private string Adhoc_ID = "Adhoc_ID";
        private string Descriptions = "Descriptions";       
        private string OBISCODE = "OBISCODE";
        private string CLASS = "CLASS";
        private string ATTRIBUTE = "ATTRIBUTE";
        private string Value = "Value";
        //private string Unit = "Unit";
        DataSet dsAdhoc = new DataSet();
        AdhocMasterEntity adhocMasterEntity = new AdhocMasterEntity();
        AdhocReadDAL adhocDAL = new AdhocReadDAL();
        DataSet dataSetXml = new DataSet();
        #endregion
        public AdhocRead()
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
           
        }
        #region Event Handlers

        private void SnapRead_Load(object sender, EventArgs e)
        {
            MenuStrip menuStrip = this.Parent.Parent.Controls.Find("menuStripMainForm", true)[0] as MenuStrip;
            DataAcquisition = menuStrip.Items["dataAcquisitionToolStripMenuItem"];
            Configuration = menuStrip.Items["configurationToolStripMenuItem"];
            dgvAdhoc.SetLabelText = "";
            Filladhocgrid();
        }
        private void btnReadAdhoc_Click(object sender, EventArgs e)
        {
            isAdhocReadStopped = false;
            isAdhocReadRuning = true;
            btnReadAdhoc.Enabled = false;           
            btnCancelSnap.Enabled = false;          
            Configuration.Enabled = false;                   
            GenerateAdhocRead();           
        }
       
        private void btnCancelSnapRead_Click(object sender, EventArgs e)
        {
            SetConnectionDetail(false);            
            this.Close();
        }
        private void Filladhocgrid()
        {            
            string xmlFilePath = "AdhocDynamicRead.xml";
            dataSetXml.ReadXml(xmlFilePath);
            dsAdhoc = dataSetXml;
            dgvAdhoc.Data = dataSetXml;
            dgvAdhoc.Refresh();
            dgvAdhoc.Show();
            dgvAdhoc.SetWidth("Descriptions", DescriptionWidth);
            dgvAdhoc.SetWidth("OBISCODE", OBISCodeWidth);
            dgvAdhoc.SetWidth("CLASS", ClassIDWidth);
            dgvAdhoc.SetWidth("ATTRIBUTE", AttributeWidth);
            dgvAdhoc.SetWidth("Value", ValueWidth);
            dgvAdhoc.SetWidth("Unit", UnitWidth);           

        }
       
         private void GenerateAdhocRead()
        {
            bool isResponseTimeout = false;
            bool isConnected = false;
            string[] datavalue = new string[2];
            string data = string.Empty;           
            StringBuilder resultData = new StringBuilder();           
            btnCancelSnap.Enabled =true;
            result.ErrorCode = CommunicationErrorType.Success;
            DataRow Adhocmasterrow = null;
            this.StatusMessageAsync = "";
            byte DLMSclass;
            string DLMSObis = String.Empty;
            byte DLMSAttribute;
            try
            {
                adhocDAL.DeleteData();
                dgvAdhoc.ClearData("Value");
                SetConnectionDetail(true);               
                //List<ProfileCommand> profileReadCommands = new List<ProfileCommand>() {                    
                // new ProfileCommand(01, "1.0.60.8D.0.FF", 02),new ProfileCommand(01, "1.0.0.3.0.FF", 02),
                // new ProfileCommand(01, "1.0.0.1.96.FF", 02),new ProfileCommand(08, "1.0.0.1.97.FF", 12),
                // new ProfileCommand(08, "1.0.0.1.98.FF", 02),new ProfileCommand(01, "1.0.0.1.99.FF", 02),
                // new ProfileCommand(08, "1.0.0.1.9B.FF", 12),new ProfileCommand(08, "1.0.0.1.9A.FF", 02), 
                // new ProfileCommand(01, "1.0.0.1.9C.FF", 02),new ProfileCommand(08, "1.0.0.1.9E.FF", 02),
                // new ProfileCommand(08, "1.0.0.1.9D.FF", 02) };
               
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
                    if (isAdhocReadStopped)
                    {
                        isAdhocReadRuning = false;
                        EnableStartAdhocReadControl();
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (result.RecieveDataBuffer != null && result.RecieveDataBuffer.Count > 0)
                        {
                            string idLength = result.RecieveDataBuffer[1].ToString("00");
                            int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                            meterId = new List<byte>();
                            meterId = result.RecieveDataBuffer.GetRange(2, index);                            
                                try
                                {
                                    //for (index = 0; index < profileReadCommands.Count; index++)
                                    for (index = 0; index < dataSetXml.Tables[0].Rows.Count; index++)
                                    {
                                        if (result.ErrorCode == CommunicationErrorType.Success && !isAdhocReadStopped)
                                        {
                                        DLMSclass =Convert.ToByte(dataSetXml.Tables[0].Rows[index]["CLASS"]);
                                        DLMSObis = dataSetXml.Tables[0].Rows[index]["OBISCODE"].ToString();
                                        DLMSAttribute = Convert.ToByte(dataSetXml.Tables[0].Rows[index]["ATTRIBUTE"]);
                                        ProfileCommand profileReadCommand = new ProfileCommand(DLMSclass, DLMSObis, DLMSAttribute);

                                        //profileReadCommands.[index].Action = ActionType.READ;
                                         //profileReadCommands[index].MeterID = meterId;
                                            this.StatusMessageAsync = "Ad hoc data Reading...";
                                            isResponseTimeout = false;
                                            result = communication.Send(profileReadCommand);
                                            if (result.ErrorCode == CommunicationErrorType.Success)
                                            {
                                           
                                            datavalue = GenericParser.DLMSDataFormator(result, 0, false);

                                            if (datavalue == null && result.RecieveDataLength == 0)
                                            {
                                                data = "Not Supported/Access denied";
                                                dgvAdhoc.Addrows(index, "Value", data);
                                                Adhocmasterrow = dsAdhoc.Tables[0].Rows[index];
                                                RowToEntity(Adhocmasterrow);
                                                adhocDAL.InsertData(adhocMasterEntity);
                                                continue;
                                            }
                                               
                                            data = datavalue[0];
                                            dgvAdhoc.Addrows(index, "Value", data);
                                            Adhocmasterrow= dsAdhoc.Tables[0].Rows[index];
                                            RowToEntity(Adhocmasterrow);                                            
                                            adhocDAL.InsertData(adhocMasterEntity);
                                        }
                                            else
                                            {
                                            dgvAdhoc.Addrows(index, "Value", "Not Supported/Access denied");
                                            Adhocmasterrow = dsAdhoc.Tables[0].Rows[index];
                                            RowToEntity(Adhocmasterrow);
                                            adhocDAL.InsertData(adhocMasterEntity);
                                            result.ErrorCode = CommunicationErrorType.Success;
                                            isAdhocReadRuning = false;
                                            isResponseTimeout = true;
                                            continue;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (!isAdhocReadStopped)
                                    {
                                   
                                    this.StatusMessageAsync = "Ad hoc readout complete.";
                                    btnReadAdhoc.Enabled = true;                                    
                                        Application.DoEvents();
                                   
                                    }
                                    else
                                    {
                                        this.StatusMessageAsync = "Ad hoc readout stopped.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                this.StatusMessageAsync = result.ErrorCode.ToString();
                                }
                                finally
                                {

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
                        isAdhocReadRuning = false;
                        this.StatusMessageAsync = ReadoutFailure;
                    }
                }
                else
                {
                    isAdhocReadRuning = false;
                    this.StatusMessageAsync = ReadoutFailure;
                }
            }
            catch (Exception)
            {
                this.StatusMessageAsync = ReadoutFailure;
            }
            finally
            {               
                communication.CloseSession();              
                isAdhocReadRuning = false;
                btnReadAdhoc.Enabled =true;
                SetConnectionDetail(false);              
            }

        }

        public IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;            
            if (NotNullAndNotDBNull(row, Descriptions)) adhocMasterEntity.Descriptions = Convert.ToString(row[Descriptions]);
            if (NotNullAndNotDBNull(row, OBISCODE)) adhocMasterEntity.OBISCODE = Convert.ToString(row[OBISCODE]);
            if (NotNullAndNotDBNull(row, CLASS)) adhocMasterEntity.CLASS = Convert.ToString(row[CLASS]);
            if (NotNullAndNotDBNull(row, ATTRIBUTE)) adhocMasterEntity.ATTRIBUTE = Convert.ToString(row[ATTRIBUTE]);
            if (NotNullAndNotDBNull(row, Value)) adhocMasterEntity.Value = Convert.ToString(row[Value]);
            return adhocMasterEntity;
        }
        public static string ParameterName(string parameterName)
        {
            return string.Concat(DatabaseFactory.GetPlaceholder(), parameterName);
        }

        public bool NotNullAndNotDBNull(DataRow row, string ColumnName)
        {
            object obj = row[ColumnName];
            if (obj == null)
                return false;
            if (string.IsNullOrEmpty(obj.ToString()))
                return false;
            else
                return true;
        }
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
        private void EnableStartAdhocReadControl()
        {
            if (btnReadAdhoc.InvokeRequired)
            {
                btnReadAdhoc.Invoke(new MethodInvoker(EnableStartAdhocReadControl));
            }
            else
            {
                btnReadAdhoc.Enabled = true;
            }
        }
        #endregion

        private void dgvAdhoc_Load(object sender, EventArgs e)
        {

        }
    }
}
