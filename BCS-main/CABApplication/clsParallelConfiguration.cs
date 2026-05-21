using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Hunt.EPIC.Logging;
using CAB.BLL;
using CAB.Channel.ReadOut;
using CAB.EntityGenerator;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.MeterData.Upload;
using CAB.Parser;
using CAB.Serialization;
using CAB.UI;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using System.Drawing;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;

namespace CABApplication
{
    public class clsParallelConfiguration
    {                                     
        List<System.Enum> selectedElements = null;
        CommunicationType commType = CommunicationType.DIRECT;
        Communication communication = null;
        DataGridViewRow dgvr = null;
        CommunicationMode commMode = CommunicationMode.Normal;
        string Tcpport = string.Empty;
        string StatusMessageAsync = string.Empty;
        string StatusMessage = string.Empty;
        string ConnectionDetailStatusMessageAsync = string.Empty;
        private string DebugLogFileName = string.Empty;
        private string Staticip = string.Empty;
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        int _DIPData;
        int _SlideSubDIP;
        int _SIPData;
        byte _BillingMonthTypeData;
        byte _cmbResetLockoutdays;
        bool _rdbKVAhLagOnly;
        bool _rdbRS232Lock;
        bool _rdbAutoLock; 
        byte[] _GetSeasonProfileBuffer; 
        byte[] _GetWeekProfileBuffer; 
        byte[] _GetSplDayProfileBuffer; 
        byte[] _GetDayProfileBuffer; 
        byte[] _GetActivationDateBuffer; 
        List<byte> _GetSelectedRowsinParameterGrid; 
        List<byte> _GetSelectedRowsinParameterGridPush; 
        List<byte> _GetSelectedRowsinParameterGridScroll; 
        List<byte> _GetSelectedRowsinParameterGridHigh; 
        Int32 _nudCTRatio; 
        Int32 _nudPTRatio; 
        bool _rdbEnableManualBilling; 
        bool _rdbEnableSoftwareBilling; 
        List<byte> _WriteLoadControl; 
        List<byte> _WriteLoadControl1PSmartMeter; 
        List<byte> _WriteDisconnectControl; 
        List<byte> _WriteRS485;
        int _cmbDIPDemandTypeSelectedIndex;
        string _cmbDIPDemandTypeSelectedItem;
        bool _chkDisconnect;
        bool _chkconnect;
        DateTime _rtcCtrl;
        int RowCount;
        CAB.E650MeterConfiguration.Entity.BillingDateTime _BillingTypeData = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(clsParallelConfiguration).ToString());

        public clsParallelConfiguration(int rowCount,List<System.Enum> _selectedElements, CommunicationType _commType, DataGridViewRow DataGridRow, string Port, CommunicationMode CommMode, int FillDIPData, int FillSlideSubDIP, int FillSIPData, CAB.E650MeterConfiguration.Entity.BillingDateTime FillBillingTypeData, byte FillBillingMonthTypeData, byte cmbResetLockoutdays, bool rdbKVAhLagOnly, bool rdbRS232Lock, bool rdbAutoLock, byte[] GetSeasonProfileBuffer, byte[] GetWeekProfileBuffer, byte[] GetSplDayProfileBuffer, byte[] GetDayProfileBuffer, byte[] GetActivationDateBuffer, List<byte> GetSelectedRowsinParameterGridPush, List<byte> GetSelectedRowsinParameterGridScroll, List<byte> GetSelectedRowsinParameterGridHigh, Int32 nudCTRatio, Int32 nudPTRatio, bool rdbEnableManualBilling, bool rdbEnableSoftwareBilling, List<byte> WriteLoadControl, List<byte> WriteLoadControl1PSmartMeter, List<byte> WriteDisconnectControl, List<byte> WriteRS485, int cmbDIPDemandType, string cmbDIPDemandTypeSelectedItem,bool chkDisconnect,bool chkconnect, DateTime rtcCtrl)
    {
        try
        {
            RowCount = rowCount;
            DebugLogFileName = DataGridRow.Cells["SimNo"].Value.ToString().Replace('.', '_') + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            selectedElements = _selectedElements;
            commType = _commType;
            dgvr = DataGridRow;
            Tcpport = Port;
            commMode = CommMode;
            _DIPData = FillDIPData;
            _SlideSubDIP = FillSlideSubDIP;
            _SIPData = FillSIPData;
            _BillingTypeData = FillBillingTypeData;
            _BillingMonthTypeData = FillBillingMonthTypeData;
            _cmbResetLockoutdays = cmbResetLockoutdays;
            _rdbKVAhLagOnly = rdbKVAhLagOnly;
            _rdbRS232Lock = rdbRS232Lock;
            _rdbAutoLock = rdbAutoLock;
            _GetSeasonProfileBuffer = GetSeasonProfileBuffer;
            _GetWeekProfileBuffer = GetWeekProfileBuffer;
            _GetSplDayProfileBuffer = GetSplDayProfileBuffer;
            _GetDayProfileBuffer = GetDayProfileBuffer;
            _GetActivationDateBuffer = GetActivationDateBuffer;
            _GetSelectedRowsinParameterGridPush = GetSelectedRowsinParameterGridPush;
            _GetSelectedRowsinParameterGridScroll = GetSelectedRowsinParameterGridScroll;
            _GetSelectedRowsinParameterGridHigh = GetSelectedRowsinParameterGridHigh;
            _nudCTRatio = nudCTRatio;
            _nudPTRatio = nudPTRatio;
            _rdbEnableManualBilling = rdbEnableManualBilling;
            _rdbEnableSoftwareBilling = rdbEnableSoftwareBilling;
            _WriteLoadControl = WriteLoadControl;
            _WriteLoadControl1PSmartMeter = WriteLoadControl1PSmartMeter;
            _WriteDisconnectControl = WriteDisconnectControl;
            _WriteRS485 = WriteRS485;
            _cmbDIPDemandTypeSelectedIndex = cmbDIPDemandType;
            _cmbDIPDemandTypeSelectedItem = cmbDIPDemandTypeSelectedItem;
            _chkDisconnect = chkDisconnect;
            _chkconnect = chkconnect;
            _rtcCtrl = rtcCtrl;           
            WriteDebugLog("clsParallelConfiguration");

        }
        catch (Exception ex)    //Exception log for catch block
        {
            logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
        }
            
    }


        private void WriteDebugLog(string Message)
        {
            try
            {
                File.AppendAllText(DebugLogFileName, Message + "\n");
            }
            catch (Exception ex)    //Exception log for catch block
            {

                MessageBox.Show(ex.ToString());
                logger.Log(LOGLEVELS.Error, "WriteDebugLog(string Message)", ex);
            }
        }


        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="selectedProfile" Type= "System.Enum"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color, System.Enum selectedProfile, string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                {

                    dgvc.Style.BackColor = color;
                    dgvc.Value = CommonBLL.GetEnumDescription(selectedProfile) + Message;
                    //Application.DoEvents();
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="Message" Type= "string"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color, string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                {
                    dgvc.Style.BackColor = color;
                    dgvc.Value = Message;
                    //Application.DoEvents();
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set value for DataGridViewCell
        /// </summary>
        /// <param name="Message" Type= "string"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(string Message, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                {
                    dgvc.Value = Message;
                    //Application.DoEvents();
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set CheckValue for DataGridViewCheckBoxCell
        /// </summary>
        /// <param name="Value" Type= "bool"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(bool Value, DataGridViewCheckBoxCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                {
                    dgvc.Value = Value;
                    //Application.DoEvents();
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }
        }



        /// <summary>
        /// OverLoaded method to set backcolor for DataGridViewCell
        /// </summary>
        /// <param name="color" Type= "System.Drawing.Color"></param>
        /// <param name="dgvc" Type = "DataGridViewCell"></param>
        /// <returns></returns>
        private void SetGridRowAttributes(System.Drawing.Color color, DataGridViewCell dgvc)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)dgvc.OwningRow).DataGridView;
                Action a = () =>
                {
                    dgvc.Style.BackColor = color;
                    //Application.DoEvents();
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog("***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }
        }





        private void SetConnectionDetail(bool connected)
        {
            try
            {
                string channelType = ConfigSettings.GetValue("ChannelType");
                string mode;
                if (connected)
                {

                    mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                    this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
                    SetGridRowAttributes(Color.Green, this.ConnectionDetailStatusMessageAsync, dgvr.Cells["Status"]);
                    Application.DoEvents();
                }
                else
                {

                    mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                    this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;
                    SetGridRowAttributes(Color.Red, this.ConnectionDetailStatusMessageAsync, dgvr.Cells["Status"]);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
            }            
        }



        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            try
            {
                DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
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
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());                
            }          
            return lstProfileCommands;
        }




        /// <summary>
        /// Writes Meter Config Data
        /// </summary>
        /// <param name="selectedProfiles"></param>
        /// <param name="isRemote"></param>
        /// <param name="simIndex"></param>
        /// <returns></returns>
        private bool WriteMeterConfigData(List<System.Enum> selectedProfiles, bool isRemote)
        {
            bool isSuccess = false;
            try
            {
                //int meterModelNumber;// = NamePlateConstants.PumaLTE650Value;
                ProfileCommand selectedCommand;
                Result result = new Result();
                result.ErrorCode = CommunicationErrorType.Success;
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    List<ProfileCommand> lstProfileCommands;
                    lstProfileCommands = GetProfileCommandEntity();
                    //btnAbort.Enabled = true;
                    foreach (ProfileId selectedConfigId in selectedProfiles)
                    {
                        //Filter one command entity
                        List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                        {
                            return profileCommandEntity.TagNumber == (int)selectedConfigId
                            && (Convert.ToString(profileCommandEntity.MeterModelNumber) == ConfigInfo.MeterModel ||
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
                            // HTCT Specific Changes
                            if (selectedConfigId == ProfileId.KvahSelection && ConfigInfo.MeterModel == "10")
                            {
                                this.StatusMessage = "Writing Mvah Selection" + " ...";
                                SetGridRowAttributes(System.Drawing.Color.Green, this.StatusMessage, dgvr.Cells["Status"]);
                                Application.DoEvents();
                            }
                            else
                            {
                                this.StatusMessage = "Writing " + CommonBLL.GetEnumDescription(selectedConfigId) + " ...";
                                SetGridRowAttributes(System.Drawing.Color.Green, this.StatusMessage, dgvr.Cells["Status"]);
                                Application.DoEvents();
                            }
                            if (selectedConfigId == ProfileId.PassiveDayProfile || selectedConfigId == ProfileId.PassiveSeasonProfile
                                || selectedConfigId == ProfileId.PassiveWeekProfile || selectedConfigId == ProfileId.ActivationDate)
                            {
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.TOU, "Writing Data...", dgvr.Cells["Status"]);
                                Application.DoEvents();
                            }
                            //else if (selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter
                            //    || selectedConfigId == ProfileId.DisplayTimeoutParameter)
                            else if (selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter)
                            // Story - Hide Display Timeout Parameter
                            {
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, ProfileId.DisplayParameters, "Writing Data...", dgvr.Cells["Status"]);
                                Application.DoEvents();
                            }
                            else
                            {
                                SetGridRowAttributes(System.Drawing.Color.LightYellow, selectedConfigId, "Writing Data...", dgvr.Cells["Status"]);
                                Application.DoEvents();
                            }

                            Application.DoEvents();
                            selectedCommand = profileCommand[0];
                            selectedCommand.Action = ActionType.WRITE;        
                            //if (selectedConfigId == ProfileId.DIP && cmbDIPDemandType.SelectedItem.ToString() == "Sliding Demand")
                            //{
                            //    profileCommand[0].ClassId = 1;
                            //    profileCommand[0].ObisCode = "00.00.60.01.99.FF";
                            //    profileCommand[0].Attribute = 2;
                            //}
                            //Fill WriteData buffer for corresponding programming parameter
                            ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveSleep = 1200;
                            ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveTimeOut = 5000;   
                            switch (selectedConfigId)
                            {
                                case ProfileId.RTC:                                                                         
                                    profileCommand[0].WriteDataBuffer = _rtcCtrl;                                    
                                    break;
                                case ProfileId.DIP:
                                    {
                                        ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveSleep = 5000;
                                        ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveTimeOut = 10000;                                       
                                        profileCommand[0].WriteDataBuffer = _DIPData;
                                        profileCommand[1].Action = ActionType.WRITE;
                                        profileCommand[1].WriteDataBuffer = _SlideSubDIP;
                                    }
                                    break;
                                case ProfileId.SIP:
                                    {
                                        ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveSleep = 10000;
                                        ((TCP)(communication.PhysicalChannelDetail)).TCPReceiveTimeOut = 70000;                                      
                                        profileCommand[0].WriteDataBuffer = _SIPData;
                                    }
                                    break;
                                case ProfileId.BillingReset:
                                    //No need to send any data for MD reset
                                    profileCommand[0].Action = ActionType.RESET;
                                    profileCommand[0].MeterModelNumber = Convert.ToByte(ConfigInfo.MeterModel);
                                    break;
                                case ProfileId.BillingType:
                                    profileCommand[0].WriteDataBuffer = _BillingTypeData;
                                    break;
                                case ProfileId.BillingMonthType:
                                    profileCommand[0].WriteDataBuffer = _BillingMonthTypeData;
                                    break; // [BillingType_Month]
                                case ProfileId.ResetLockOutDays:
                                    profileCommand[0].WriteDataBuffer = _cmbResetLockoutdays;
                                    break;
                                case ProfileId.KvahSelection:
                                    profileCommand[0].WriteDataBuffer = _rdbKVAhLagOnly ? Convert.ToByte(0) : Convert.ToByte(1);
                                    break;
                                case ProfileId.RS232LockUnlock:
                                    profileCommand[0].WriteDataBuffer = _rdbRS232Lock ? Convert.ToByte(1) : Convert.ToByte(0);
                                    break;
                                case ProfileId.AutoLock:
                                    profileCommand[0].WriteDataBuffer = _rdbAutoLock ? Convert.ToByte(0) : Convert.ToByte(1);
                                    break;
                                case ProfileId.PassiveSeasonProfile:
                                    //******* Meter Model Change Required Here ***********//
                                    if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VBSPNoSeasonNoWeek || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VFSPNoSeasonNoWeek) continue;
                                    profileCommand[0].WriteDataBuffer = _GetSeasonProfileBuffer;
                                    break;
                                case ProfileId.PassiveWeekProfile:

                                    //******* Meter Model Change Required Here ***********//
                                    if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VBSPNoSeasonNoWeek || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.VFSPNoSeasonNoWeek) continue;
                                    profileCommand[0].WriteDataBuffer = _GetWeekProfileBuffer;
                                    break;
                                //**********smart meter special day profile************
                                case ProfileId.SpecialDayProfileSmartMeter:
                                    profileCommand[0].WriteDataBuffer = _GetSplDayProfileBuffer;
                                    break;
                                case ProfileId.PassiveDayProfile:
                                    profileCommand[0].WriteDataBuffer = _GetDayProfileBuffer;
                                    break;
                                case ProfileId.ActivationDate:
                                    profileCommand[0].WriteDataBuffer = _GetActivationDateBuffer;
                                    break;
                                case ProfileId.PushDisplayParameter:
                                    {
                                        profileCommand[0].WriteDataBuffer = _GetSelectedRowsinParameterGridPush;
                                        SetGridRowAttributes(System.Drawing.Color.Green, ProfileId.DisplayParameters, "Write Successful.", dgvr.Cells["Status"]);
                                        Application.DoEvents();
                                    }
                                    break;
                                case ProfileId.ScrollDisplyParameter:
                                    {
                                        profileCommand[0].WriteDataBuffer = _GetSelectedRowsinParameterGridScroll;
                                        SetGridRowAttributes(System.Drawing.Color.Green, ProfileId.DisplayParameters, "Write Successful.", dgvr.Cells["Status"]); Application.DoEvents();
                                    }
                                    break;
                                case ProfileId.HighResolutionDisplayParameter:
                                    {
                                        profileCommand[0].WriteDataBuffer = _GetSelectedRowsinParameterGridHigh;
                                        SetGridRowAttributes(System.Drawing.Color.Green, ProfileId.DisplayParameters, "Write Successful.", dgvr.Cells["Status"]); Application.DoEvents();
                                    }
                                    break;
                                // Story - Hide Display Timeout Parameter
                                //case ProfileId.DisplayTimeoutParameter:
                                //    profileCommand[0].WriteDataBuffer = GetDisplayTimeoutData();
                                //    break;
                                case ProfileId.CTRatio:
                                    profileCommand[0].WriteDataBuffer = _nudCTRatio;
                                    break;
                                case ProfileId.PTRatio:
                                    profileCommand[0].WriteDataBuffer = _nudPTRatio;
                                    break;
                                case ProfileId.ManualBilling:
                                    profileCommand[0].WriteDataBuffer = _rdbEnableManualBilling ? Convert.ToByte(1) : Convert.ToByte(0);
                                    break;
                                case ProfileId.SoftwareBilling:
                                    profileCommand[0].WriteDataBuffer = _rdbEnableSoftwareBilling ? Convert.ToByte(1) : Convert.ToByte(0);
                                    break;
                                //********* For smart meter load control and disconnect control*******
                                case ProfileId.LoadControl:
                                    profileCommand[0].WriteDataBuffer = _WriteLoadControl;
                                    break;
                                case ProfileId.LoadControl1PSmartMeter:
                                    profileCommand[0].WriteDataBuffer = _WriteLoadControl1PSmartMeter;
                                    break;

                                case ProfileId.DisconnectControl:
                                    if (_chkconnect)
                                    {
                                        profileCommand[0].Attribute = 0x02;
                                    }
                                    else if (_chkDisconnect)
                                    {
                                        profileCommand[0].Attribute = 0x01;
                                    }
                                    profileCommand[0].WriteDataBuffer = _WriteDisconnectControl;
                                    profileCommand[0].Action = ActionType.ACTIONREQUEST;
                                    break;
                                //********* For HTCT Meter*******
                                case ProfileId.RS485:
                                    profileCommand[0].WriteDataBuffer = _WriteRS485;
                                    break;
                                default:
                                    break;
                            }
                            if (selectedConfigId == ProfileId.DIP)
                            {
                                //******* Meter Model Change Required Here ***********//
                                if (ConfigInfo.SignatureInfo.ToUpperInvariant().Contains("VB") || ConfigInfo.SignatureInfo.Contains("VF") || ConfigInfo.SignatureInfo.Contains("FS") || ConfigInfo.SignatureInfo.Contains("SK") || ConfigInfo.SignatureInfo.Contains("SF"))
                                {
                                    //For this Meter Model send only first Profile commands
                                    result = communication.Send(profileCommand[_cmbDIPDemandTypeSelectedIndex]);
                                }
                                else
                                {
                                    // For rest Meter Model send both the command
                                    if (_cmbDIPDemandTypeSelectedItem == "Sliding Demand")
                                    {
                                        result = communication.Send(profileCommand[0]);
                                        if (result.ErrorCode == CommunicationErrorType.Success) result = communication.Send(profileCommand[1]);
                                    }
                                    else  //cmbDIPDemandType.SelectedItem.ToString() == "Block Demand")
                                    {
                                        result = communication.Send(profileCommand[1]);
                                        if (result.ErrorCode == CommunicationErrorType.Success) result = communication.Send(profileCommand[0]);
                                    }
                                }
                            }
                            else if ((selectedConfigId == ProfileId.PushDisplayParameter || selectedConfigId == ProfileId.ScrollDisplyParameter || selectedConfigId == ProfileId.HighResolutionDisplayParameter) && ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
                            {
                                result = communication.SendWriteBlock(profileCommand[0]);
                            }
                            else
                                result = communication.Send(profileCommand[0]);

                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {

                                // Story - Hide Display Timeout Parameter
                                //if (selectedConfigId == ProfileId.DisplayTimeoutParameter)
                                //{
                                //    lngGridViewReadControl1.UncheckCheckBox(ProfileId.DisplayParameters);
                                //    SetGrid(ProfileId.DisplayParameters, System.Drawing.Color.Green, "Write Successful.");
                                //}
                                if (selectedConfigId == ProfileId.ActivationDate)
                                {

                                    SetGridRowAttributes(System.Drawing.Color.Green, ProfileId.TOU, "Write Successful.", dgvr.Cells["Status"]);
                                    Application.DoEvents();
                                }
                                else
                                {
                                    SetGridRowAttributes(System.Drawing.Color.Green, selectedConfigId, "Write Successful.", dgvr.Cells["Status"]); Application.DoEvents();
                                    Application.DoEvents();
                                }
                                continue;
                            }
                            else
                            {
                                if (selectedConfigId == ProfileId.PushDisplayParameter ||
                                    selectedConfigId == ProfileId.HighResolutionDisplayParameter ||
                                    selectedConfigId == ProfileId.ScrollDisplyParameter) //|| selectedConfigId == ProfileId.DisplayTimeoutParameter)// Story - Hide Display Timeout Parameter
                                {

                                    SetGridRowAttributes(System.Drawing.Color.LightPink, ProfileId.DisplayParameters, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                                    Application.DoEvents();
                                }
                                else if (selectedConfigId == ProfileId.ActivationDate ||
                                    selectedConfigId == ProfileId.PassiveDayProfile || selectedConfigId == ProfileId.PassiveSeasonProfile ||
                                    selectedConfigId == ProfileId.PassiveWeekProfile)
                                {
                                    SetGridRowAttributes(System.Drawing.Color.LightPink, ProfileId.TOU, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                                    Application.DoEvents();
                                }
                                else if (selectedConfigId == ProfileId.BillingMonthType && result.ErrorCode == CommunicationErrorType.AccessDenied)
                                {
                                    //Billing Month Type 
                                    //If Access Denied comes from Meter then show Success message for Billing Type "Other" (if NAC(03) response come for Billing Cycle write command)  

                                    result.ErrorCode = CommunicationErrorType.Success;
                                    SetGridRowAttributes(System.Drawing.Color.LightPink, selectedConfigId, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                                    Application.DoEvents();
                                }
                                else
                                {
                                    SetGridRowAttributes(System.Drawing.Color.LightPink, selectedConfigId, CommonBLL.GetEnumDescription(result.ErrorCode), dgvr.Cells["Status"]);
                                    Application.DoEvents();

                                }
                                //result.ErrorCode = CommunicationErrorType.PasswordInavalid;
                                this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                                SetGridRowAttributes(System.Drawing.Color.LightPink, selectedConfigId, this.StatusMessage, dgvr.Cells["Status"]);
                                Application.DoEvents();
                                isSuccess = false;
                                break;
                                Application.DoEvents();
                                //break;
                            }
                            isSuccess = true;

                        }
                        //else if (result.ErrorCode == CommunicationErrorType.AccessDenied)
                        //{
                        //    SetGrid(selectedConfigId, System.Drawing.Color.Red, "Access Denied");
                        //    isSuccess = false;
                        //}
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        this.StatusMessage = "Data Written Successfully.";
                        SetGridRowAttributes(System.Drawing.Color.LightPink, this.StatusMessage, dgvr.Cells["Status"]);
                        Application.DoEvents();
                        isSuccess = true;
                    }
                    else
                    {
                        this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        SetGridRowAttributes(System.Drawing.Color.LightPink, this.StatusMessage, dgvr.Cells["Status"]);
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                    SetGridRowAttributes(System.Drawing.Color.LightPink, this.StatusMessage, dgvr.Cells["Status"]);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
                 this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
            }
            
            return isSuccess;
        }


        public void WriteThreadOne()
        {
            Result result = new Result();
            bool isConnected = false;
            try
            {
                List<System.Enum> selectedProfiles = selectedElements;
                
                byte totalRetries;
                
                totalRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
                

                for (byte retryNumber = 0; retryNumber < totalRetries; retryNumber++)
                {

                    DataGridViewCheckBoxCell chk1 = dgvr.Cells["Select"] as DataGridViewCheckBoxCell;
                    
                    if (Convert.ToBoolean(chk1.Value))
                    {
                        string simNumber = dgvr.Cells["SimNo"].Value.ToString();
                        //simNumber = dgvMeterIdAndSim[(int)dgvSimColumn.SimNo, rowCount].Value.ToString();
                        string Message = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";
                        this.StatusMessageAsync = retryNumber > 0 ? "Retrying To Connect " + simNumber + " ..." : "Connecting " + simNumber + " ...";

                        SetGridRowAttributes(System.Drawing.Color.LightYellow, Message, dgvr.Cells["Status"]);

                        
                        Application.DoEvents();
                        ChannelInformation channelInfo = new ChannelInformation();
                        channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
                        channelInfo.ComPort = ConfigSettings.GetValue("PortName");
                        channelInfo.ModemInfo = simNumber;
                        channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
                        channelInfo.Password = ConfigSettings.GetValue("ModePassword");
                        channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
                        channelInfo.NoOfRetries = totalRetries;
                        Staticip = simNumber;//txtBoxMeterSIM.Text;                        
                        if (commType == CommunicationType.TCP)
                        {
                            channelInfo.ModemInfo = Staticip;
                            channelInfo.TcpPort = Tcpport;
                        }
                        else
                        {
                            channelInfo.ModemInfo = simNumber;
                        }
                        communication = new Communication(channelInfo);                       

                        result = communication.OpenSession();
                        SetConnectionDetail(true);
                        if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS || result.ErrorCode == CommunicationErrorType.Success)
                        {
                            //isMeterConnected = true;
                            isConnected = WriteMeterConfigData(selectedProfiles, true);
                            if (isConnected)
                            {
                                SetGridRowAttributes(System.Drawing.Color.LightGreen,"Write completed.",dgvr.Cells["Status"]);              
                                this.StatusMessageAsync = "Write completed.";
                                SetGridRowAttributes(false, dgvr.Cells["Select"] as DataGridViewCheckBoxCell);                                                              Application.DoEvents();
                            }
                            else
                            {                                
                                SetGridRowAttributes(System.Drawing.Color.Red, "Write failed.", dgvr.Cells["Status"]); 
                                this.StatusMessageAsync = "Write failed.";
                                Application.DoEvents();
                            }

                        }
                        else
                        {   
                            this.StatusMessageAsync = CommonBLL.GetEnumDescription(result.ErrorCode);                            
                            SetGridRowAttributes(System.Drawing.Color.Red, "Connection " + simNumber + " failed.", dgvr.Cells["Status"]);
                            this.StatusMessageAsync = "Connection " + simNumber + " failed.";
                            Application.DoEvents();
                        }
                        communication.CloseSession();
                    }

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.WriteDebugLog( "***" + MethodInfo.GetCurrentMethod().Name + "***\n" + ex.ToString());
                logger.Log(LOGLEVELS.Error, "Reading Meter Data... clsParallelConfiguration Row = " + RowCount + ex.ToString(),ex);
            }

        }
    }
}
