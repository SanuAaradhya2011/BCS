using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

using DLMSLIB;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using SerialCommunication;
using Utilities;
namespace DLMSGSMCommunication
{
    public delegate void MultiplePortsGSMLogEventHandler(object sender, GSMLogEventArgs e);
    internal enum ProgrammingCode
    {
        Success,
        Fail,
        AccessDenied,
        DataUnavailable,
        TimeOut,
        SignOnFailed,
        CosemConnectionFailed,
        MeterIDMismatch
    }
    public class MultiplePortsCommunication
    {

        #region Variable Declaration
        string commandResponse = string.Empty;
        byte[] HDLCCommand = new byte[200];
        byte[] MODEMCommand = new byte[20];
        GSMLogEventArgs gsmEventArgs = null;
        byte MODEMIndex = 0;
        byte HDLCIndex = 0;
        byte ShowIndex = 0;
        bool isCurrentCommandOfPTRatio = false;
        UploadFile upload = new UploadFile();
        public SerialComm objSerialComm = null;
        COSEMLIB objCOSEMLIB = null;
        HDLCLIB objHDLCLIB = null;
        public string comPortName = string.Empty;
        GSMLoggingBLL logBLL = null;
        string meterID = null;
        int logID = 0;
        object test = new object();
        int comRetries = 0;
        GSMLoggingEntity gsmLoggingEntity = null;
        #endregion
        #region Scheduler AT commands variables
        private ModemConfig modemConfig = null;
        private string[] initCommands = null;
        private string[] resetCommands = null;
        private string dial = string.Empty;
        private string baudRate = string.Empty;
        private string parity = string.Empty;
        private string stopBit = string.Empty;
        private string dataBit = string.Empty;
        private int configuredTimeOut = 5000;
        private int configuredIntercharacterDelay = 3500;
        #endregion
        #region Global functions declarations
        byte[] globalFunctionHDLCCommand = new byte[200];
        byte globalFunctionHDLCIndex = 0;
        private int dLMSRetryForMeterCommands = 0;
        private const int DLMSRetryForModemCommands = 0;
        //list will store general data.
        private List<string> generalList = null;
        //list will store instant data
        private List<string> instantList = null;
        //list will store billing data
        private List<string> billingList = null;
        //list will store tamper data
        private List<string> tamperList = null;
        //list will store file data 
        private List<string> fileList = null;
        //list will store tou file data
        private List<string> touList = null;
        // meterIDOfCurrentlyReadMeter holds the value of currently read meter. This may differ from what is scheduled from BCS.
        private string meterIDOfCurrentlyReadMeter = string.Empty;
        //holds the value of current file Name and date time
        private string FileMeterdata = string.Empty;
        //holds the value of file name
        private string strFileName = string.Empty;
        //holds the value of directory name
        private string directoryName = string.Empty;
        bool isPUMA = false;

        #endregion

        string simNumber = string.Empty;
        /// <summary>
        /// Property for holding dlms retries for meter commands
        /// </summary>
        public int DLMSRetryForMeterCommands
        {
            get
            {
                return dLMSRetryForMeterCommands;
            }
            set
            {
                dLMSRetryForMeterCommands = value;
            }
        }
        public string SimNumber
        {
            get
            {
                return simNumber;
            }
            set
            {
                simNumber = value;
            }
        }
        public string MeterID
        {
            get
            {
                return meterID;
            }
            set
            {
                meterID = value;
            }
        }
        public int LogID
        {
            get
            {
                return logID;
            }
            set
            {
                logID = value;
            }
        }
        //public event MultiplePortsGSMLogEventHandler GSMLogCreating;

        public MultiplePortsCommunication()
        {
            //TODO - Commented by Arun
            //objSerialComm = new SerialComm();
            //gsmEventArgs = new GSMLogEventArgs();
            //gsmEventArgs.IsGeneralCompleted = false;
            //gsmEventArgs.IsInstantCompleted = false;
            //gsmEventArgs.IsBillingCompleted = false;
            //gsmEventArgs.Retries = 1;

        }
        /// <summary>
        /// Get modem config name from communication Type
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        private ModemConfigProperties GetModemProperties(ModemConfig config, string communicationType)
        {
            ModemConfigProperties configProperites = null;
            foreach (ModemConfigProperties modemConfig in config.Items)
            {
                if (modemConfig.Name == communicationType)
                {
                    configProperites = modemConfig;
                    break;
                }
            }
            return configProperites;
        }
        /// <summary>
        /// Reads Modem config details into objects 
        /// </summary>
        public void FillModemConfigDetails(string modemCfgFilePath, string communicationType)
        {
            string reConnectCommand = string.Empty;
            string initCommand = string.Empty;
            string resetCommand = string.Empty;
            //Deserialize the AT command information from xml to object

            if (this.modemConfig != null)
            {
                //BhardwajG Get modem config name object
                ModemConfigProperties modemConfigProperties = GetModemProperties(modemConfig, communicationType);
                if (modemConfigProperties != null)
                {
                    //Read Command settings section
                    if (modemConfigProperties.Commandsettings != null && modemConfigProperties.Commandsettings.Length > 0)
                    {
                        initCommand = modemConfigProperties.Commandsettings[0].Initialize;
                        resetCommand = modemConfigProperties.Commandsettings[0].Reset;
                        reConnectCommand = modemConfigProperties.Commandsettings[0].ReConnect;
                        if (!string.IsNullOrEmpty(initCommand))
                        {
                            this.initCommands = initCommand.Split('|');
                        }
                        if (!string.IsNullOrEmpty(resetCommand))
                        {
                            this.resetCommands = resetCommand.Split('|');
                        }
                        //Dialing command 
                        this.dial = modemConfigProperties.Commandsettings[0].Dial;
                        this.configuredIntercharacterDelay = modemConfigProperties.Commandsettings[0].InterCharacterTimeout;
                        this.configuredTimeOut = modemConfigProperties.Commandsettings[0].CommandTimeout;
                        //BhardwajG Set the dlms retry for meter commands
                        this.dLMSRetryForMeterCommands = modemConfigProperties.Commandsettings[0].DLMSRetries;
                    }
                    //Read port settings section
                    if (modemConfigProperties.Portsettings != null && modemConfigProperties.Portsettings.Length > 0)
                    {
                        this.baudRate = modemConfigProperties.Portsettings[0].BitsPerSecond;
                        this.parity = modemConfigProperties.Portsettings[0].Parity;
                        this.stopBit = modemConfigProperties.Portsettings[0].Stopbits;
                        this.dataBit = modemConfigProperties.Portsettings[0].Databits;
                    }
                }
            }
        }
        /// <summary>
        /// Used to Connect Modem using its default config by reading modem.cfg      
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public bool SwitchModemConfigToDefaultConfig(string filePath, string communicationType)
        {
            bool success = false;
            string result = string.Empty;
            try
            {

                EventLogging.CallLogDetails(comPortName + ":" + "Initializing local modem.");
                FillModemConfigDetails(filePath, communicationType);
                objSerialComm.DLMSRetries = DLMSRetryForModemCommands;
                //BhardwajG : Send discription irrespective of the commands configured in the .xml
                objSerialComm.bCommType = 1;
                MultipleSerialPortSettings.Default.CommandTimeOut = 6000;
                //BhardwajG Intercharacter Delay 5500
                MultipleSerialPortSettings.Default.IntercharacterDelay = 5500;
                objSerialComm.SetSerialPortSettings(comPortName, this.baudRate, this.parity, this.dataBit, this.stopBit, MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                //BhardwajG : Set serial port             

                if (initCommands != null && initCommands.Length > 0)
                {
                    for (int counter = 0; counter < initCommands.Length; counter++)
                    {
                        objSerialComm.bCommType = 1;
                        result = SendCommandToModem(initCommands[counter]);
                        if (initCommands[counter].Equals("ATH") || initCommands[counter].Equals("ATE") || initCommands[counter].Equals("AT"))
                        {
                            if (result.ToUpper().Contains("OK"))
                            {
                                success = true;
                            }
                            else
                            {
                                success = false;
                                break;
                            }
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    objSerialComm.SetSerialPortSettings("9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                }
            }
            catch
            {
                success = false;
            }
            finally
            {
                MultipleSerialPortSettings.Default.CommandTimeOut = this.configuredTimeOut;
                MultipleSerialPortSettings.Default.IntercharacterDelay = this.configuredIntercharacterDelay;
                objSerialComm.bCommType = 0x00;
                objSerialComm.DLMSRetries = this.dLMSRetryForMeterCommands;

            }
            return success;

        }

        /// <summary>
        /// 
        /// </summary>
        public void LeaveModemToUtilityConfig()
        {
            try
            {
                objSerialComm.DLMSRetries = DLMSRetryForModemCommands;
                if (resetCommands != null && resetCommands.Length > 0)
                {
                    foreach (string command in resetCommands)
                    {
                        objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.InterframeTimeout;
                        objSerialComm.CommandTimeout = 6000;
                        objSerialComm.bCommType = 1;
                        objSerialComm.InterchatracterDelay = 5000;
                        objSerialComm.timeout = 5500;
                        SendCommandToModem(command);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                objSerialComm.DLMSRetries = DLMSRetryForMeterCommands;
            }
        }
        /// <summary>
        /// Reads Modem config details into objects 
        /// </summary>
        public void GetModemConfigDetail(string modemCfgFilePath)
        {
            CABSerializer apiConfiguration = new CABSerializer();
            string reConnectCommand = string.Empty;
            string initCommand = string.Empty;
            string resetCommand = string.Empty;
            string[] initCommandList = null;
            string[] resetCommandList = null;
            this.modemConfig = (ModemConfig)apiConfiguration.DeserializeToObject(modemCfgFilePath, typeof(ModemConfig));
            if (this.modemConfig != null)
            {
                ModemConfigModemNameCommandsettings[] modemConfigModemNameCommandsettings = modemConfig.Items[0].Commandsettings;
                ModemConfigModemNamePortsettings[] modemConfigModemNamePortsettings = modemConfig.Items[0].Portsettings;
                //Read Command settings section
                if (modemConfigModemNameCommandsettings != null && modemConfigModemNameCommandsettings.Length > 0)
                {
                    initCommand = modemConfigModemNameCommandsettings[0].Initialize;
                    resetCommand = modemConfigModemNameCommandsettings[0].Reset;
                    reConnectCommand = modemConfigModemNameCommandsettings[0].ReConnect;
                    if (!string.IsNullOrEmpty(initCommand))
                    {
                        initCommandList = initCommand.Split('|');
                        this.initCommands = initCommandList;

                    }
                    if (!string.IsNullOrEmpty(resetCommand))
                    {
                        resetCommandList = resetCommand.Split('|');
                        this.resetCommands = resetCommandList;
                    }
                    //Dialing command 
                    this.dial = modemConfigModemNameCommandsettings[0].Dial;
                }
                //Read port settings section
                if (modemConfigModemNamePortsettings != null && modemConfigModemNamePortsettings.Length > 0)
                {
                    this.baudRate = modemConfigModemNamePortsettings[0].BitsPerSecond;
                    this.parity = modemConfigModemNamePortsettings[0].Parity;
                    this.stopBit = modemConfigModemNamePortsettings[0].Stopbits;
                    this.dataBit = modemConfigModemNamePortsettings[0].Databits;

                }
            }
        }
        public MultiplePortsCommunication(string portName, string phoneNumber, string mpMeterID, int mpLogID, SerialComm lngSerialPort, ModemConfig modemConfiguration)
        {
            logID = mpLogID;
            meterID = mpMeterID;
            logBLL = new GSMLoggingBLL();
            simNumber = phoneNumber;
            comPortName = portName;
            objSerialComm = lngSerialPort;

            objHDLCLIB = new HDLCLIB();
            objCOSEMLIB = new COSEMLIB();
            gsmLoggingEntity = new GSMLoggingEntity();
            //objSerialComm.PortName = portName;
            upload.ComPortName = portName;
            gsmLoggingEntity.IsGeneralCompleted = false;
            //gsmEventArgs = new GSMLogEventArgs();
            gsmLoggingEntity.IsGeneralCompleted = false;
            gsmLoggingEntity.IsInstantCompleted = false;
            gsmLoggingEntity.IsBillingCompleted = false;
            gsmLoggingEntity.Log_ID = logID;
            //gsmLoggingEntity.Retries = 1;
            this.modemConfig = modemConfiguration;
            fileList = new List<string>();

        }

        void objSerialComm_Close(string portName)
        {

        }
        #region Global functions

        //'******************************************************************************
        //'
        //'  NAME     : SendSNRM
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send SNRM packet and Recieve and Check UA response
        //'
        //'*******************************************************************************
        public bool fSendSNRM(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        {

            try
            {
                globalFunctionHDLCIndex = 0;
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddHDLCFrameTag(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddServerSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nServerSAP, nServerLowerMacAddress);
                globalFunctionHDLCIndex = objHDLCLIB.fAddClientSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nClientSAP);
                objHDLCLIB.fSetSNRM();
                globalFunctionHDLCIndex = objHDLCLIB.fAddCmdByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.ffillLength(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, 9, 10);
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                if (objSerialComm.fSendDataToPort(globalFunctionHDLCCommand, globalFunctionHDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fSetUA();//Setting Response Command type
                    if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer, nClientSAP) == true)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckHDLCResponse
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        //'
        //'*******************************************************************************
        private bool fCheckHDLCResponse(byte[] Buffer, int nClientSAP)
        {

            if (objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                return false;
            }
            else
            {
                if (objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    return false;
                }
                else
                {
                    if (objHDLCLIB.fCheckServerSAP(Buffer, nClientSAP) == false)
                    {
                        return false;
                    }
                    else
                    {
                        if (objHDLCLIB.fCheckCommand(Buffer, objHDLCLIB.nCMDByte) == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSendAARQ
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send AARQ packet and Recieve and Check AARE response
        //'
        //'*******************************************************************************
        public bool fSendAARQ(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, byte nSecurityMechanism, string nPassword, string HLSKey)
        {
            try
            {
                //Change Needed
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = 0x00;
                cnfBlock[1] = 0x12;
                cnfBlock[2] = 0x1A;
                //Change Needed
                //7EA02E0002002321107ECBE6E600601DA109060760857405080101BE10040E01000000065F1F040000121AFFFFF4FF7E
                //7EA047000200234110974BE6E6006036A1090607608574050801018A0207808B0760857405080201AC0A80083132333435363738BE10040E01000000065F1F040000121AFFFFEEE07E

                globalFunctionHDLCIndex = 0;
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddHDLCFrameTag(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddServerSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nServerSAP, nServerLowerMacAddress);
                globalFunctionHDLCIndex = objHDLCLIB.fAddClientSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nClientSAP);
                objHDLCLIB.fSetInitialI();
                globalFunctionHDLCIndex = objHDLCLIB.fAddCmdByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objCOSEMLIB.fAddLLCByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                if (nSecurityMechanism == 0x01)
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddAARQTAG(globalFunctionHDLCCommand, globalFunctionHDLCIndex, 0x36);
                else
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddAARQTAG(globalFunctionHDLCCommand, globalFunctionHDLCIndex, 0x3E);
                byte nApplicationContext = 0x01;
                globalFunctionHDLCIndex = objCOSEMLIB.fAddContext(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nApplicationContext);
                if (nSecurityMechanism == 0x01)
                {
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddSecMechanism(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nSecurityMechanism);
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddPassword(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nPassword);
                }
                else if (nSecurityMechanism == 0x02)
                {
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddSecMechanism(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nSecurityMechanism);
                    globalFunctionHDLCIndex = objCOSEMLIB.fAddRandomKey(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nPassword);
                }
                globalFunctionHDLCIndex = objCOSEMLIB.fAddUserInf(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objCOSEMLIB.fAddCnfBlock(globalFunctionHDLCCommand, globalFunctionHDLCIndex, cnfBlock);
                int nPDUSize = 9999;
                globalFunctionHDLCIndex = objCOSEMLIB.fAddPDUSize(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nPDUSize);
                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.ffillLength(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, globalFunctionHDLCIndex - 3);
                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex - 2, globalFunctionHDLCIndex - 1);
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);

                if (objSerialComm.fSendDataToPort(globalFunctionHDLCCommand, globalFunctionHDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer, nClientSAP) == true)
                    {
                        if (objCOSEMLIB.fCheckAARQResponse(objSerialComm.ReceiveBuffer) == true)
                        {
                            if (nSecurityMechanism == 0x02)
                            {

                                globalFunctionHDLCIndex = 0;
                                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                globalFunctionHDLCIndex = objHDLCLIB.fAddHDLCFrameTag(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                globalFunctionHDLCIndex = objHDLCLIB.fAddServerSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nServerSAP, nServerLowerMacAddress);
                                globalFunctionHDLCIndex = objHDLCLIB.fAddClientSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nClientSAP);
                                objHDLCLIB.fIncSend();
                                globalFunctionHDLCIndex = objHDLCLIB.fAddCmdByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                globalFunctionHDLCIndex = objCOSEMLIB.fAddLLCByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                //C3 01 C1 00 0F 00 00 28 00 03 FF 01 00 09 10
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0xC3;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x01;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0xC1;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x00;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x0F;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x00;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x00;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x28;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x00;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x03;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0xFF;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x01;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x00;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x09;
                                globalFunctionHDLCCommand[globalFunctionHDLCIndex++] = 0x10;
                                globalFunctionHDLCIndex = objCOSEMLIB.fAddEncryptedKey(globalFunctionHDLCCommand, globalFunctionHDLCIndex, HLSKey);

                                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                objHDLCLIB.ffillLength(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, globalFunctionHDLCIndex - 3);
                                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex - 2, globalFunctionHDLCIndex - 1);
                                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);

                                if (objSerialComm.fSendDataToPort(globalFunctionHDLCCommand, globalFunctionHDLCIndex) == false)
                                {
                                    return false;
                                }
                                else
                                {
                                    ///check for response
                                    //////Application.DoEvents();
                                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                                    if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer, nClientSAP) == true)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                                return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSendDISC
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send DISC packet and Recieve and Check UA response
        //'
        //'*******************************************************************************
        public bool fSendDISC(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        {
            try
            {
                globalFunctionHDLCIndex = 0;
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddHDLCFrameTag(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddServerSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nServerSAP, nServerLowerMacAddress);
                globalFunctionHDLCIndex = objHDLCLIB.fAddClientSAP(globalFunctionHDLCCommand, globalFunctionHDLCIndex, nClientSAP);
                objHDLCLIB.fSetDISC();
                globalFunctionHDLCIndex = objHDLCLIB.fAddCmdByte(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                globalFunctionHDLCIndex = objHDLCLIB.fAddBlankFCS(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.ffillLength(globalFunctionHDLCCommand, globalFunctionHDLCIndex);
                objHDLCLIB.fGenerateFCS(globalFunctionHDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(globalFunctionHDLCCommand, 9, 10);
                globalFunctionHDLCIndex = objHDLCLIB.fAdd7E(globalFunctionHDLCCommand, globalFunctionHDLCIndex);

                if (objSerialComm.fSendDataToPort(globalFunctionHDLCCommand, globalFunctionHDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fSetUA();//Setting Response Command type
                    if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer, nClientSAP) == true)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        //public void OnGSMLogCreating(object sender, GSMLogEventArgs e)
        //{
        //    if (GSMLogCreating != null)
        //        GSMLogCreating(sender, e);
        //}

        #region Private Methods for Reading Meter data.
        /// <summary>
        /// This method is used for the checksum from the file which created during reading meter data.
        /// </summary>
        /// <param name="filename">Pass the filename for get checksum.</param>
        /// <returns></returns>
        private string GetMD5ChecksumForFile(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("The 'filename' parameter cannot be null.");

            if (!File.Exists(filename))
                throw new ArgumentException(string.Format("Filename '{0}' does not exist.", filename));

            using (FileStream fstream = new FileStream(filename, FileMode.Open))
            {
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fstream);

                // Convert the byte array to a printable string.
                StringBuilder sb = new StringBuilder(32);
                foreach (byte hex in hash)
                    sb.Append(hex.ToString("X2"));

                return sb.ToString().ToUpper();
            }
        }

        /// <summary>
        /// This method is used for disconnecting DLMS communication from serial com port.
        /// </summary>
        public void DLMSDisconnect()
        {
            try
            {

                if (fSendDISC(MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress, MultipleSerialPortSettings.Default.ClientSAP) == true)
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Disconnected with Meter Successfully.");
                    return;
                }
                else
                {
                    //objSerialComm.ClosePort();
                    EventLogging.CallLogDetails(comPortName + ":" + "HDLC Connection Failed..");
                    return;
                }
            }
            catch (Exception ex)
            {
                //objSerialComm.ClosePort();
                EventLogging.CallLogDetails(ex.Message);
                return;
            }
        }

        /// <summary>
        /// This method is used for initializing the meter id.
        /// </summary>
        /// <param name="iIndex">Pass the index number.</param>
        /// <returns></returns>
        private int InitializeReadMeterID(int iIndex)
        {
            try
            {
                //store value from xml data set
                DataSet OBISLIST = null;
                //define xml data document object
                XmlDataDocument xmlDatadoc = null;
                xmlDatadoc = new XmlDataDocument();
                //serialize the xml data 
                string path = AppDomain.CurrentDomain.BaseDirectory + "Name Plate Details.xml";//MultipleSerialPortSettings.Default.ReadOut;//AppDomain.CurrentDomain.BaseDirectory + "DLMSReadOutList.xml";

                xmlDatadoc.DataSet.ReadXml(path);

                //assign memory to dataset object and name it "alerts"
                OBISLIST = new DataSet("OBis List Dataset");
                //deserialize xml data
                OBISLIST = xmlDatadoc.DataSet;
                objCOSEMLIB.ObisQueryDSet = OBISLIST;
                //store value from xml data set


                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();


                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQuery(HDLCCommand, HDLCIndex, iIndex);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {

                    return 0x00;
                }
                else
                {

                    objHDLCLIB.fIncRecieve();//Setting Response Command type                 
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(objSerialComm.ReceiveBuffer);

                        // This is a workaround as LTCT,HTCT meters will not support PT Ratio.
                        // if access denied and current command is for PT ratio return success.
                        if (ret == 0x03 && isCurrentCommandOfPTRatio)
                        {
                            return 0x01;
                        }
                        if (ret == 0x01)
                        {
                            return 0x01; //Success
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Data unavailable.");
                            return 0x0E;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Access denied.");

                            return 0x03;
                        }
                        else
                        {
                            return 0x00; //Fail
                        }
                    }
                    else
                    {
                        return 0x00;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// This method is used for reading temper profile from meter.
        /// </summary>
        /// <param name="atb">Pass the attribute id.</param>
        /// <param name="tamperCompartment">Pass the temper compartment number.</param>
        /// <returns>Byte value.</returns>
        private byte ReadTamperProfile(byte atb, byte tamperCompartment)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fGetQueryTamperProfile(HDLCCommand, HDLCIndex, atb, tamperCompartment);

                //added by gopal for Selective Access By Entry
                if (atb == 0x02)
                {
                    //Commenting out code for selective access by entry.
                    ////if (rdBtnReadLastEvent.Checked == true)//TODO:Abhay
                    //{
                    //    HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(1));

                    //}
                    //// else if (rdBtnReadBetweenEvent.Checked == true)
                    //{

                    //    //HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFromEvent.Text), Convert.ToByte(cmbBoxToEvent.Text));
                    //}
                }
                //added by gopal for Selective Access
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }


            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is using for reading billing profile form the meter.
        /// </summary>
        /// <param name="atb">Pass the attribute id.</param>
        /// <returns>Byte value</returns>        
        private byte ReadBillingProfile(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fGetQueryBillingProfile(HDLCCommand, HDLCIndex, atb);

                //added by gopal for Selective Access By Entry
                //if (atb == 0x02)
                ////{
                ////    if (rdBtnReadLast.Checked == true)
                ////    {
                ////        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(cmbBoxLastFrom.Text));

                ////    }
                ////    else if (rdBtnReadBetween.Checked == true)
                ////    {
                ////        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(cmbBoxFrom.Text), Convert.ToByte(cmbBoxTo.Text));
                ////    }

                //}
                //added by gopal for Selective Access
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {

                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// reads the RTC of meter
        /// </summary>
        /// <returns></returns>
        private DateTime GetMeterDateTime()
        {
            DateTime meterDateTime = DateTime.MinValue;
            try
            {
                MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                //iIndex = 0;
                int ret = ReadRTC(2);
                if (ret == 0x00)
                {
                    String strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }

                    meterDateTime = DateUtility.ConvertHexStringToDateTime(strTemp);
                    //BhardwajG if meter date time is not parsed correctly
                    if (meterDateTime == DateTime.MinValue)
                    {
                        meterDateTime = DateTime.Now;
                    }
                }
                return meterDateTime;
            }
            catch (Exception ex)
            {
                meterDateTime = DateTime.Now;
                return meterDateTime;
            }
        }
        /// <summary>
        /// This method is used for reading load survey data from the meter.
        /// </summary>
        /// <param name="atb"></param>
        /// <returns></returns>
        private byte ReadLSProfile(byte atb, DateTime fromDate, DateTime toDate)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fGetQueryLoadSurveyProfile(HDLCCommand, HDLCIndex, atb);

                //added by dhirendra for Selective Access By Range
                if (atb == 0x02)
                {
                    // if (rdBtnReadBetweenLS.Checked == true)//TODO:Abhay
                    //{
                    EventLogging.CallLogDetails(comPortName + ": Load survey is read in duration from : " + fromDate.ToString() + toDate.ToString());
                    //compare from min values before making the partial query
                    if (fromDate > DateTime.MinValue && toDate > DateTime.MinValue)
                    {
                        HDLCIndex = objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, fromDate, toDate);
                    }

                    //}
                }
                //added by gopal for Selective Access
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is used for reading scalar profile from the meter.
        /// </summary>
        /// <param name="atb">Pass the atribute information to get sclar profile data.</param>
        /// <param name="nProfileindex">Pass the profile index number to get sclar profile data.</param>
        /// <returns>byte value</returns>
        private byte ReadScalarProfile(byte atb, byte nProfileindex)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (nProfileindex == 0)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 1)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 2)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 3)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 4)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 5)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, atb);
                }

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method is used for reading instantaneous data from meter.
        /// </summary>
        /// <param name="atb">pass the atribute value for getting instantaneous data.</param>
        /// <returns>bye value</returns>
        private byte ReadInastantaneous(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.GetQueryInstantProfile(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This mehod is used for checking HDLC connection response.
        /// </summary>
        /// <param name="Buffer">Pass buffer to check response.</param>
        /// <returns></returns>
        private bool CheckHDLCResponse(byte[] Buffer)
        {
            if (objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                //objSerialComm.ClosePort();
                EventLogging.CallLogDetails(comPortName + ":" + "Invalid Start or end Tag");
                return false;
            }
            else
            {
                if (objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    //objSerialComm.ClosePort();
                    EventLogging.CallLogDetails(comPortName + ":" + "Invalid HDLC FCS");
                    return false;
                }
                else
                {
                    if (objHDLCLIB.fCheckServerSAP(Buffer, MultipleSerialPortSettings.Default.ClientSAP) == false)
                    {
                        //objSerialComm.ClosePort();
                        EventLogging.CallLogDetails(comPortName + ":" + "Invalid Destination Address");
                        return false;
                    }
                    else
                    {
                        if (objHDLCLIB.fCheckCommand(Buffer, objHDLCLIB.nCMDByte) == false)
                        {
                            //objSerialComm.ClosePort();
                            EventLogging.CallLogDetails(comPortName + ":" + "Invalid Response Byte");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// This method is used for reading serial number.
        /// </summary>
        /// <returns></returns>
        private int ReadMeterSerialNumber()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.GetQueryReadMeterID(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 1;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                        return 1;
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
                return 1;
            }
        }

        /// <summary>
        /// This method is used for connecting to serial port using DLMS GSM communication.
        /// </summary>
        /// <returns>A boolean value true or false.</returns>
        public bool DLMSConnect()
        {
            try
            {
                //objSerialComm.SetSerialPortSettings(comPortName, "9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                objSerialComm.SetSerialPortSettings("9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                //if (!objSerialComm.OpenPort())
                //{
                //    EventLogging.CallLogDetails(comPortName + ": Error while connecting with modem. The port is opened by some other application.");
                //    return false;
                //}
                if (fSendSNRM(MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress, MultipleSerialPortSettings.Default.ClientSAP) == true)
                {

                    if (MultipleSerialPortSettings.Default.SecurityMechanism == 0)
                    {
                        MultipleSerialPortSettings.Default.SecurityMechanism = 1;
                    }
                    if (fSendAARQ(MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress, MultipleSerialPortSettings.Default.ClientSAP, MultipleSerialPortSettings.Default.SecurityMechanism, MultipleSerialPortSettings.Default.Password, MultipleSerialPortSettings.Default.HLSKey) == true)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Connected to Meter Successfully");
                        return true;
                    }
                    else
                    {

                        //objSerialComm.ClosePort();
                        EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed..Unable to connect to Meter");
                        return false;
                    }
                }
                else
                {

                    //objSerialComm.ClosePort();
                    EventLogging.CallLogDetails(comPortName + ":" + "HDLC Connection Failed..Unable to connect to Meter" + Thread.CurrentThread.Name);
                    return false;
                }
            }
            catch (Exception ex)
            {

                //objSerialComm.ClosePort();
                EventLogging.CallLogDetails(ex.Message);
                return false;
            }
        }
        public void RebootModem()
        {


            try
            {
                objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.InterframeTimeout;
                //objSerialComm.SetSerialPortSettings(comPortName, "9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                objSerialComm.SetSerialPortSettings("9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                //if (!objSerialComm.OpenPort())
                //{
                //    EventLogging.CallLogDetails(comPortName + " : Error while rebooting modem. The port is opened by some other application.");
                //    return;
                //}
                objSerialComm.CommandTimeout = 6000;
                objSerialComm.bCommType = 1;
                objSerialComm.InterchatracterDelay = 5000;
                objSerialComm.timeout = 5500;
                EventLogging.CallLogDetails(comPortName + ":" + "Rebooting modem..");
                SendCommandToModem("AT+cfun=1");
                EventLogging.CallLogDetails(comPortName + ":" + "Waiting for 10 seconds..");
                Thread.Sleep(10000);

            }
            catch (Exception ex)
            {
                //objSerialComm.ClosePort();
            }
            finally
            {
                objSerialComm.CommandTimeout = MultipleSerialPortSettings.Default.CommandTimeOut;
                objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.IntercharacterDelay;
                objSerialComm.bCommType = 0;
                //objSerialComm.ClosePort();
            }

        }
        public void LogInDatabase(GSMTaskEntity gsmTaskEntity, int retriesUsed, string message)
        {
            comRetries = retriesUsed;
            bool IsinstantaneousRequired = false;
            bool IsBillingRequired = false;
            bool IsGeneralRequired = false;
            IsGeneralRequired = gsmTaskEntity.isGeneralRequired;
            IsBillingRequired = gsmTaskEntity.isBillingRequired;
            gsmLoggingEntity.Task_ID = gsmTaskEntity.taskId;
            gsmLoggingEntity.Meter_ID = Thread.CurrentThread.Name.Split(':')[0].ToString();
            gsmLoggingEntity.Group_ID = gsmTaskEntity.groupId;
            gsmLoggingEntity.Retries = retriesUsed;
            gsmLoggingEntity.ErrorMessage = message;
            gsmLoggingEntity.Log_ID = logID;
            IsinstantaneousRequired = gsmTaskEntity.isInstantaneousRequired;
            if (IsGeneralRequired)
            {
                if (gsmLoggingEntity.IsGeneralCompleted == false)
                    gsmLoggingEntity.Status = "NC";
            }
            else if (IsBillingRequired)
            {
                if (gsmLoggingEntity.IsBillingCompleted == false)
                    gsmLoggingEntity.Status = "NC";
            }
            else if (IsinstantaneousRequired)
            {
                if (gsmLoggingEntity.IsInstantCompleted == false)
                    gsmLoggingEntity.Status = "NC";
            }
            else
            {
                gsmLoggingEntity.Status = "NS";
            }

            //gsmLoggingEntity.ErrorMessage = "Unable to connect to Modem" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
            //EventLogging.CallLogDetails("before event..");
            //raise a logging event here
            GSMLogCreating(gsmLoggingEntity);
        }
        public bool TryConnectingMeter(GSMTaskEntity gsmTaskEntity, int retriesUsed, string message)
        {
            comRetries = retriesUsed;
            bool IsinstantaneousRequired = false;
            bool IsBillingRequired = false;
            bool IsGeneralRequired = false;
            IsGeneralRequired = gsmTaskEntity.isGeneralRequired;
            IsBillingRequired = gsmTaskEntity.isBillingRequired;
            IsinstantaneousRequired = gsmTaskEntity.isInstantaneousRequired;
            gsmLoggingEntity.Task_ID = gsmTaskEntity.taskId;
            gsmLoggingEntity.Meter_ID = Thread.CurrentThread.Name.Split(':')[0].ToString();
            gsmLoggingEntity.Group_ID = gsmTaskEntity.groupId;
            gsmLoggingEntity.Retries = retriesUsed;
            gsmLoggingEntity.ErrorMessage = message;
            gsmLoggingEntity.Log_ID = logID;
            if (DLMSConnect() != true)
            {
                if (IsGeneralRequired)
                {
                    if (gsmLoggingEntity.IsGeneralCompleted == false)
                        gsmLoggingEntity.Status = "NC";
                }
                else if (IsBillingRequired)
                {
                    if (gsmLoggingEntity.IsBillingCompleted == false)
                        gsmLoggingEntity.Status = "NC";
                }
                else if (IsinstantaneousRequired)
                {
                    if (gsmLoggingEntity.IsInstantCompleted == false)
                        gsmLoggingEntity.Status = "NC";
                }
                else
                {
                    gsmLoggingEntity.Status = "NS";
                }

                gsmLoggingEntity.ErrorMessage = "Unable to connect to Meter" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                //EventLogging.CallLogDetails("before event..");
                //raise a logging event here
                GSMLogCreating(gsmLoggingEntity);
                return false;
            }
            return true;
            //EventLogging.CallLogDetails("after event..");
        }
        private byte fReadCumulativeKVA(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = objCOSEMLIB.GetCumulativeKVA(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        //else if (ret == 0x02)//next packet
                        //{
                        //    while (true)
                        //    {
                        //        fIncrementTimer();

                        //        //Send Block tarsfer Command
                        //        HDLCIndex = 0;
                        //        HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        //        HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);

                        //        objHDLCLIB.fIncSend();

                        //        HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                        //        objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        //        objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        //        objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                        //        objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);

                        //        HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        objHDLCLIB.fIncRecieve();//Setting Response Command type
                        //        //7EA014022321766E17E6E600C002C100000002CA8C7E
                        //        if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                        //        {
                        //            return 0x00;
                        //        }
                        //        else
                        //        {
                        //            if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                        //            {
                        //                ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        //                if (ret == 0x01)
                        //                    break;
                        //                else if (ret == 0x02)
                        //                    continue;
                        //            }
                        //            else
                        //            {
                        //                return 0x00;
                        //            }
                        //        }
                        //    }

                        //    return 0x01;
                        //}
                        //else if (ret == 0x05)
                        //{
                        //    return 0x05;
                        //}
                        //else if (ret == 0x07)
                        //{
                        //    return 0x07;
                        //}
                        //BhardwajG return 0x07 instead of 0x00
                        else
                        {
                            return 0x07;
                        }
                    }
                    //BhardwajG return 0x07 instead of 0x00
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //added PUMA
        private byte fReadCumulativeKW(byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = objCOSEMLIB.GetCumulativeKW(HDLCCommand, HDLCIndex, atb);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        // Fix by Swati. Change function  fCheckCOSEMResponse() to fCheckCOSEMResponseForGet() 
                        int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        //else if (ret == 0x02)//next packet
                        //{
                        //    while (true)
                        //    {
                        //        fIncrementTimer();

                        //        //Send Block tarsfer Command
                        //        HDLCIndex = 0;
                        //        HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                        //        HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);

                        //        objHDLCLIB.fIncSend();

                        //        HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                        //        HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                        //        objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                        //        objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                        //        objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                        //        objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                        //        objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);

                        //        HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                        //        objHDLCLIB.fIncRecieve();//Setting Response Command type
                        //        //7EA014022321766E17E6E600C002C100000002CA8C7E
                        //        if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                        //        {
                        //            return 0x00;
                        //        }
                        //        else
                        //        {
                        //            if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                        //            {
                        //                ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        //                if (ret == 0x01)
                        //                    break;
                        //                else if (ret == 0x02)
                        //                    continue;
                        //            }
                        //            else
                        //            {
                        //                return 0x00;
                        //            }
                        //        }
                        //    }

                        //    return 0x01;
                        //}
                        //else if (ret == 0x05)
                        //{
                        //    return 0x05;
                        //}
                        //else if (ret == 0x07)
                        //{
                        //    return 0x07;
                        //}
                        //BhardwajG return 0x07 instead of 0x00
                        else
                        {
                            return 0x07;
                        }
                    }
                    //BhardwajG return 0x07 instead of 0x00
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Read demand integration period
        /// </summary>
        /// <returns></returns>
        private int ReadIntegrationPeriod()
        {
            int status = 0;
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.GetQueryReadIntegrationPeriod(HDLCCommand, HDLCIndex, 2);
                //HDLCIndex = objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    status = (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(objSerialComm.ReceiveBuffer, MultipleSerialPortSettings.Default.ClientSAP) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponseForGet(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            status = (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            status = (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            status = (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            status = (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        status = (int)ProgrammingCode.CosemConnectionFailed;
                }
                return status;
            }
            catch (Exception ex)
            {
                status = (int)ProgrammingCode.CosemConnectionFailed;
                return status;
            }
        }
        /// <summary>
        /// Read Demand Integration Period
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        private bool ReadDemandIntegrationPeriod()
        {
            bool success = false;
            string strTemp = string.Empty;
            int compValue = 0;
            ////eventlogging.CallLogDetails(comPortName + ":" + "Now start reading Instantaneous data from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            int writeResponse = ReadIntegrationPeriod();
            //cmbBoxIntegrationPeriod.Items.AddRange(cmbItems);
            if (writeResponse == (int)ProgrammingCode.Success)
            {
                compValue = (compValue | (int)objSerialComm.ReceiveBuffer[19]) << 8;
                compValue = (compValue | (int)objSerialComm.ReceiveBuffer[20]);
                strTemp = "0A" + compValue.ToString();
                success = true;
            }
            else if (writeResponse == (int)ProgrammingCode.DataUnavailable)
            {
                //if data is unavailable then return false, as there will nothing to write in file
                success = false;
            }
            else
            {
                success = false;
            }
            if (success)
            {
                fileList.Add(strTemp);
            }
            return success;
        }
        /// <summary>
        /// Fills the load survey from and to dates according to the load survey type selected
        /// </summary>
        /// <param name="gsmTaskEntity"></param>
        private void FillLoadSurveyFromAndToDate(GSMTaskEntity gsmTaskEntity)
        {
            try
            {
                //Get the current RTC of the meter
                gsmTaskEntity.LoadSurveyToDate = GetMeterDateTime();
                gsmTaskEntity.LoadSurveyFromDate = gsmTaskEntity.LoadSurveyToDate.AddDays(-(MultipleSerialPortSettings.Default.MaxLoadSurveyDays));
                if (gsmTaskEntity.LoadSurveyJobType == JobType.LoadSurveyPartial)
                {
                    DLMS650LoadSurveyBLL dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                    //when load survey type is of load survey partial get last load survey date from db.
                    DateTime lastLoadSurveyDate = dlms650LoadSurveyBLL.GetLastLoadSurveyDataInDbForMeter(meterIDOfCurrentlyReadMeter);
                    // if last load survey date stored in db is greater than max load survey date then it can be a load survey from date.
                    if (lastLoadSurveyDate != null && lastLoadSurveyDate > DateTime.MinValue && lastLoadSurveyDate > gsmTaskEntity.LoadSurveyFromDate)
                    {
                        gsmTaskEntity.LoadSurveyFromDate = lastLoadSurveyDate;
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
        /// <summary>
        /// Writes the list to streamwriter
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="list"></param>
        private void WriteListToStream(StreamWriter streamWriter, List<string> list)
        {
            foreach (string item in list)
            {
                streamWriter.WriteLine(item);

            }
        }
        /// <summary>
        /// Read general profile, return status of read
        /// </summary>
        /// <returns></returns>
        private bool ReadGeneral()
        {
            bool bSuccess = true;
            generalList = new List<string>();
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading General data from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            int iIndex = 0;
            int nObjectCount = 0;
            iIndex = 0;
            ShowIndex = 1;
            int meterModelNo = 0;
            //nObjectCount = 7;//2;
            //BhardwajG : If show meter model no then send meter model no command as well. 
            if (UtilityDetails.ShowMeterModelNo)
            {
                nObjectCount = 8;//2;
            }
            else
            {
                nObjectCount = 7;
            }
            while (iIndex < nObjectCount)
            {
                if (iIndex == 6)
                    isCurrentCommandOfPTRatio = true;
                else
                    isCurrentCommandOfPTRatio = false;
                int retn = InitializeReadMeterID(iIndex);
                if (retn == 0x01)
                {
                    if (objHDLCLIB.fCheckFCS(objSerialComm.ReceiveBuffer) == false)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Invalid Cosem FCS. " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.Status = "NC";
                        bSuccess = false;
                        break;
                    }
                    else
                    {
                        //DisplayNamePlateDataInGrid(objSerialComm.ReceiveBuffer, iIndex);
                        int length = 0;
                        int startIndex = 0;
                        string strTemp = "";
                        if (objSerialComm.ReceiveBuffer[18] == 0x09 && objSerialComm.ReceiveBuffer[19] != 12)
                        {
                            length = objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x0A && objSerialComm.ReceiveBuffer[19] != 12)
                        {
                            length = objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x09 && objSerialComm.ReceiveBuffer[19] == 12)
                        {
                            length = objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x12)
                        {
                            length = 2;
                            startIndex = 19;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x11)
                        {
                            length = 1;
                            startIndex = 19;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x06 || objSerialComm.ReceiveBuffer[18] == 0x05)
                        {
                            length = 4;
                            startIndex = 19;
                        }
                        else if (objSerialComm.ReceiveBuffer[18] == 0x15)
                        {
                            length = 8;
                            startIndex = 19;
                        }
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + string.Format("{0:X2}", objSerialComm.ReceiveBuffer[i + startIndex]);
                        }
                        //BhardwajG : Get the meter model no if required.
                        if (UtilityDetails.ShowMeterModelNo)
                        {
                            if (iIndex == 7)
                            {
                                if (int.TryParse(strTemp, out meterModelNo))
                                {
                                    if (meterModelNo == NamePlateConstants.RubyE250Value)
                                    {
                                        isPUMA = false;
                                    }
                                    else
                                    {
                                        isPUMA = true;
                                    }
                                }
                            }
                        }
                        if (isCurrentCommandOfPTRatio && string.IsNullOrEmpty(strTemp))
                        {
                            // add in list and write on stream after completion of loop,
                            // so that there should be need of reading general profile in every retry.
                            generalList.Add("05" + strTemp + 0x00);
                        }
                        else
                        {
                            // add in list and write on stream after completion of loop,
                            // so that there should be need of reading general profile in every retry.
                            generalList.Add("05" + strTemp);
                        }
                    }
                }
                else if (retn == 0x00)
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
                iIndex++;
            }
            if (bSuccess)
            {
                fileList.AddRange(generalList);
            }
            return bSuccess;
        }
        /// <summary>
        /// Read Instant and cumulative demand kw and kva if the meter is PUMA
        /// </summary>
        /// <returns></returns>
        private bool ReadInstant()
        {
            byte ret = 0;
            bool bSuccess = true;
            //Instantiating the list
            instantList = new List<string>();
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            ret = ReadInastantaneous(3);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                //add to the list
                instantList.Add("01" + strTemp);
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed." + " : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadInastantaneous(2);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    //add to the list
                    instantList.Add("01" + strTemp);
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;

                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(3, 0);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    instantList.Add("01" + strTemp);
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(2, 0);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    instantList.Add("01" + strTemp);
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            if (isPUMA)
            {
                //added PUMA
                #region CU-MD-KW
                MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                byte retval1 = 0x00;
                if (bSuccess)
                {
                    //for getting Data
                    retval1 = fReadCumulativeKW(2);
                    if (retval1 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        // To solve DLMS_0074 
                        int startIndex = 0;
                        // Receive buffer[18] tells the datatype , 0x06 means long int.
                        if (objSerialComm.ReceiveBuffer[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                        }
                        else
                        {
                            // added if readout is not successful.
                            bSuccess = false;
                            EventLogging.CallLogDetails("Cosem Connection Failed");
                        }
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objSerialComm.ReceiveBuffer[i + startIndex]);
                        }
                        instantList.Add("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (retval1 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        instantList.Add("01" + "00000000");
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":Cosem Connection Failed.");
                        bSuccess = false;
                        gsmEventArgs.GSMLog.Status = "NC";
                    }
                }
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                if (bSuccess)
                {
                    //for getting scalar unit
                    retval1 = ReadScalarProfile(3, 4);
                    if (retval1 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                        }
                        instantList.Add("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    //BhardwajG : For by passing ruby
                    else if (retval1 == 0x07 || retval1 == 0x05)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        instantList.Add("01");
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":Cosem Connection Failed");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                    }
                }
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                #endregion

                //added PUMA
                #region CU-MD-KVA
                MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;

                //for getting Data
                byte retval2 = 0x00;
                if (bSuccess)
                {
                    retval2 = fReadCumulativeKVA(2);
                    if (retval2 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        ///00000041_11_06_10_06_26_12
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        // To solve DLMS_0074 
                        int startIndex = 0;
                        // Receive buffer[18] tells the datatype , 0x06 means long int.
                        if (objSerialComm.ReceiveBuffer[18] == 0x06)
                        {
                            length = 4;
                            startIndex = 19;
                        }
                        //else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01)
                        //{ 

                        //}
                        else
                        {
                            // added if readout is not successful.
                            EventLogging.CallLogDetails(comPortName + ":Cosem Connection Failed");
                            gsmEventArgs.GSMLog.Status = "NC";
                            bSuccess = false;
                        }
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objSerialComm.ReceiveBuffer[i + startIndex]);
                        }
                        instantList.Add("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    else if (retval2 == 0x07)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        instantList.Add("01" + "00000000");
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":Cosem Connection Failed");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                    }
                }
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                if (bSuccess)
                {
                    //for getting scalar unit
                    retval2 = ReadScalarProfile(3, 5);
                    if (retval2 == 0x01)
                    {
                        //DisplayDataInGrid(GlobalObjects.objCOSEMLIB.BlockBuffer);
                        //fApplyScalarUnit();
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                        }
                        instantList.Add("01" + strTemp);
                    }
                    //fix - Ashish 04/10/11
                    //BhardwajG : For by passing ruby
                    else if (retval2 == 0x07 || retval1 == 0x05)
                    {
                        //write an empty line so that parser can predict that nothing in this line should be read
                        instantList.Add("01");
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":Cosem Connection Failed");
                        gsmEventArgs.GSMLog.Status = "NC";
                        bSuccess = false;
                    }
                }
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                #endregion
            }
            //BhardwajG : For reading anamoly data
            if (bSuccess)
            {
                fileList.AddRange(instantList);
            }
            return bSuccess;

            //else
            //{
            //    for (byte x = 0; x < 4; x++)
            //        wr1.WriteLine("01");              //writing Line breaks for no data
            //}

        }
        /// <summary>
        /// Read billing profile 
        /// </summary>
        /// <returns></returns>
        private bool ReadBilling()
        {
            byte ret = 0x00;
            billingList = new List<string>();
            bool bSuccess = true;
            ret = ReadBillingProfile(3);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                billingList.Add("02" + strTemp);
            }
            else if (ret == 0x05)
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Access Denied " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            if (bSuccess)
            {
                ret = ReadBillingProfile(2);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    billingList.Add("02" + strTemp);
                }
                else if (ret == 0x05)
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Access Denied " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(3, 1);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    billingList.Add("02" + strTemp);
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(2, 1);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    billingList.Add("02" + strTemp);
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            if (bSuccess)
            {
                billingList.Add("020D");
                fileList.AddRange(billingList);
            }
            return bSuccess;

        }
        private bool ReadTamper()
        {
            bool bSuccess = true;
            tamperList = new List<string>();
            byte ret = 0x00;
            string strTamperScalecapture = string.Empty;
            string strTamperScalebuffer = string.Empty;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 1 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            ret = ReadTamperProfile(3, 0);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                tamperList.Add("04" + strTemp);
            }
            // in case of no data
            else if (ret == 0x07)
            {
                tamperList.Add("04");
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            ret = ReadTamperProfile(2, 0);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                tamperList.Add("04" + strTemp);
            }
            // in case of no data
            else if (ret == 0x07)
            {
                tamperList.Add("04");
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(3, 3);
                if (ret == 0x01)
                {

                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                    strTamperScalecapture = strTemp;
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadScalarProfile(2, 3);
                if (ret == 0x01)
                {

                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                    strTamperScalebuffer = strTemp;
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                    return false;
                }
            }
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 2 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadTamperProfile(3, 1);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadTamperProfile(2, 1);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            tamperList.Add("04" + strTamperScalecapture);
            tamperList.Add("04" + strTamperScalebuffer);


            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 3 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            if (bSuccess)
            {
                ret = ReadTamperProfile(3, 2);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadTamperProfile(2, 2);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            tamperList.Add("04" + strTamperScalecapture);
            tamperList.Add("04" + strTamperScalebuffer);

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 4 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            ret = ReadTamperProfile(3, 3);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                tamperList.Add("04" + strTemp);
            }
            else if (ret == 0x07)
            {
                tamperList.Add("04");
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            ret = ReadTamperProfile(2, 3);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                tamperList.Add("04" + strTemp);
            }
            // in case of no data
            else if (ret == 0x07)
            {
                tamperList.Add("04");
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }

            tamperList.Add("04" + strTamperScalecapture);
            tamperList.Add("04" + strTamperScalebuffer);

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 5 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            ret = ReadTamperProfile(3, 4);
            if (ret == 0x01)
            {
                string strTemp = "";
                int length = objCOSEMLIB.nBlockTotalByteCount;
                //length = nBlockIndex;
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                tamperList.Add("04" + strTemp);
            }
            // in case of no data
            else if (ret == 0x07)
            {
                tamperList.Add("04");
            }
            else
            {
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                gsmLoggingEntity.Status = "NC";
                bSuccess = false;
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadTamperProfile(2, 4);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            tamperList.Add("04" + strTamperScalecapture);
            tamperList.Add("04" + strTamperScalebuffer);

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Tamper data compartment 6 from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            if (bSuccess)
            {
                ret = ReadTamperProfile(3, 5);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            if (bSuccess)
            {
                ret = ReadTamperProfile(2, 5);
                if (ret == 0x01)
                {
                    string strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    tamperList.Add("04" + strTemp);
                }
                // in case of no data
                else if (ret == 0x07)
                {
                    tamperList.Add("04");
                }
                else
                {
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                    gsmLoggingEntity.Status = "NC";
                    bSuccess = false;
                }
            }
            tamperList.Add("04" + strTamperScalecapture);
            tamperList.Add("04" + strTamperScalebuffer);
            strTamperScalecapture = "";
            strTamperScalebuffer = "";
            if (bSuccess)
            {
                fileList.AddRange(tamperList);
            }
            return bSuccess;
        }
        /// <summary>
        /// Reading meter id
        /// </summary>
        /// <returns></returns>
        private bool ReadMeterID()
        {
            bool bSuccess = true;
            int writeResponse = ReadMeterSerialNumber();
            if (writeResponse == 0)
            {
                string data = string.Empty;

                int idLen = Convert.ToInt16(objSerialComm.ReceiveBuffer[19]);
                if (idLen < 7 || idLen > 16)
                {
                    bSuccess = false;
                    EventLogging.CallLogDetails(comPortName + ":" + "Meter data corrupt " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                }
                string idLength = Convert.ToString(objSerialComm.ReceiveBuffer[19]);
                while (idLength.Length < 2) idLength = "0" + idLength;
                int index = Convert.ToInt16(objSerialComm.ReceiveBuffer[19]);
                for (int i = 20; i <= 20 + (index - 1); i++)
                {
                    data += Convert.ToChar(objSerialComm.ReceiveBuffer[i]).ToString();
                }
                //set the meter id so that last load survey date stored in database can be fetched
                meterIDOfCurrentlyReadMeter = data;
                strFileName = strFileName + data;
                strFileName = strFileName + "_" + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Month) + "_" + String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + "_" + String.Format("{0:00}", DateTime.Now.Minute) + "_" + String.Format("{0:00}", DateTime.Now.Second) + ".2NG";
                FileMeterdata = idLength + data + String.Format("{0:0000}", DateTime.Now.Year) + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
            }
            else
            {
                bSuccess = false;
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            }
            if (bSuccess)
            {
                fileList.Add("00" + FileMeterdata);
            }
            return bSuccess;
        }
        private bool ReadLoadSurvey(GSMTaskEntity gsmTaskEntity)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            bool bSuccess = true;
            try
            {
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                fileStream = new FileStream(directoryName + strFileName, FileMode.Create);
                streamWriter = new StreamWriter(fileStream);
                WriteListToStream(streamWriter, fileList);
                byte ret = 0x00;
                EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Load Survey data from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                objCOSEMLIB.nBlockIndex = 0;
                objCOSEMLIB.nTotalPacketSize = 0;
                objCOSEMLIB.nBlockNumber = 0;
                objCOSEMLIB.nBlockTotalByteCount = 0;
                //File will be created only when meter id and general completed
                if (gsmLoggingEntity.IsMeterIDCompleted && gsmLoggingEntity.IsGeneralCompleted)
                {

                    ret = ReadLSProfile(3, DateTime.MinValue, DateTime.MinValue);
                    if (ret == 0x01)
                    {
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        streamWriter.Write("03");
                        for (int i = 0; i < length; i++)
                        {
                            streamWriter.Write(string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]));
                        }
                        streamWriter.WriteLine("");
                    }
                    else if (ret == 0x05)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Access Denied.");
                        gsmLoggingEntity.Status = "NC";
                        bSuccess = false;
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                        gsmLoggingEntity.Status = "NC";
                        bSuccess = false;
                    }
                    if (bSuccess)
                    {
                        // if the load survey type is loadsurveypartialfrom, from and to date are already filled. 
                        if (gsmTaskEntity.LoadSurveyJobType != JobType.LoadSurveyPartialFrom)
                        {
                            FillLoadSurveyFromAndToDate(gsmTaskEntity);
                        }
                        //meterMasterEntity.Meter_ID
                        ret = ReadLSProfile(2, gsmTaskEntity.LoadSurveyFromDate, gsmTaskEntity.LoadSurveyToDate);
                        if (ret == 0x01)
                        {
                            int length = objCOSEMLIB.nBlockTotalByteCount;
                            streamWriter.Write("03");
                            for (int i = 0; i < length; i++)
                            {
                                //strTemp = strTemp + string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                                streamWriter.Write(string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]));
                            }
                            //wr1.WriteLine("03" + strTemp);
                            streamWriter.WriteLine("");
                        }
                        else if (ret == 0x05)
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Access Denied.");
                            gsmLoggingEntity.Status = "NC";
                            bSuccess = false;
                        }
                        else if (ret == 0x07)
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "No load survey data found");

                        }
                        else
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                            gsmLoggingEntity.Status = "NC";
                            bSuccess = false;
                        }
                    }
                    objCOSEMLIB.nBlockIndex = 0;
                    objCOSEMLIB.nTotalPacketSize = 0;
                    objCOSEMLIB.nBlockNumber = 0;
                    objCOSEMLIB.nBlockTotalByteCount = 0;
                    if (bSuccess)
                    {
                        ret = ReadScalarProfile(3, 2);
                        if (ret == 0x01)
                        {
                            int length = objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            streamWriter.Write("03");
                            for (int i = 0; i < length; i++)
                            {
                                streamWriter.Write(string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]));
                            }
                            streamWriter.WriteLine("");
                        }
                        else
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                            gsmLoggingEntity.Status = "NC";
                            bSuccess = false;
                        }
                    }
                    objCOSEMLIB.nBlockIndex = 0;
                    objCOSEMLIB.nTotalPacketSize = 0;
                    objCOSEMLIB.nBlockNumber = 0;
                    objCOSEMLIB.nBlockTotalByteCount = 0;
                    if (bSuccess)
                    {
                        ret = ReadScalarProfile(2, 2);
                        if (ret == 0x01)
                        {
                            int length = objCOSEMLIB.nBlockTotalByteCount;
                            //length = nBlockIndex;
                            streamWriter.Write("03");
                            for (int i = 0; i < length; i++)
                            {
                                streamWriter.Write(string.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]));
                            }
                            streamWriter.WriteLine("");
                        }
                        else
                        {
                            EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed.");
                            gsmLoggingEntity.Status = "NC";
                            bSuccess = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bSuccess = false;
            }
            finally
            {
                streamWriter.Close();
                fileStream.Close();
                string strChecksum = GetMD5ChecksumForFile(directoryName + strFileName);
                FileStream file2 = new FileStream(directoryName + strFileName, FileMode.Append);
                StreamWriter wr2 = new StreamWriter(file2);
                wr2.WriteLine(strChecksum);
                wr2.Close();
                file2.Close();
                if (bSuccess)
                {
                    //if load survey read succeeded then remove data from list
                    fileList.Clear();

                }
                else
                {
                    //If file exists then delete the file.
                    if (File.Exists(directoryName + strFileName))
                    {
                        File.Delete(directoryName + strFileName);
                    }
                }
            }
            return bSuccess;
        }
        public void MakeFileFromList()
        {
            FileStream lngFile = null;
            StreamWriter lngWriter = null;
            FileStream checkSumFile = null;
            StreamWriter checkSumWriter = null;
            try
            {
                if (fileList != null && fileList.Count > 0)
                {
                    if (!Directory.Exists(directoryName.Trim()))
                    {
                        Directory.CreateDirectory(directoryName.Trim());
                    }
                    if (gsmLoggingEntity.IsMeterIDCompleted && gsmLoggingEntity.IsGeneralCompleted)
                    {
                        lngFile = new FileStream(directoryName + strFileName, FileMode.Append);
                        lngWriter = new StreamWriter(lngFile);
                        foreach (string line in fileList)
                        {
                            lngWriter.WriteLine(line);
                        }
                        lngWriter.Close();
                        lngFile.Close();
                        string strChecksum = GetMD5ChecksumForFile(directoryName + strFileName);
                        checkSumFile = new FileStream(directoryName + strFileName, FileMode.Append);
                        checkSumWriter = new StreamWriter(checkSumFile);
                        checkSumWriter.WriteLine(strChecksum);
                        checkSumWriter.Close();
                        checkSumFile.Close();
                        //save the file

                    }
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(comPortName + ":While making file " + ex.Message);
                strFileName = string.Empty;
            }
            finally
            {
                if (File.Exists(directoryName + strFileName))
                {
                    if (upload.SaveMeterData(directoryName + strFileName))
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Data saved into DB : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        //BhardwajG : Do not delete file as this is required. 
                        //File.Delete(strFileName);
                    }
                    else
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Error occured while saving data in DB : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    }
                }
                //Release the resources if left unclosed
                if (lngWriter != null)
                {
                    lngWriter.Close();
                }
                if (lngFile != null)
                {
                    lngFile.Close();
                }
                if (checkSumFile != null)
                {
                    checkSumFile.Close();
                }
                if (checkSumWriter != null)
                {
                    checkSumWriter.Close();
                }
            }


        }
        /// <summary>
        /// This method is being used for the reading meter data.
        /// </summary>
        /// <param name="IsinstantaneousRequired">Pass the boolean value for instantaneous data is required or not to read from meter.</param>
        /// <param name="IsBillingRequired">Pass the boolean value for billing data is required or not to read from meter.</param>
        /// <param name="IsloadSurveyRequired">Pass the boolean value for load survey data is required or not to read from meter.</param>
        /// <param name="IsTamperRequired">Pass the boolean value for tampering data is required or not to read from meter.</param>
        /// <param name="IsGeneralRequired">Pass the boolean value for general data is required or not to read from meter.</param>
        /// <returns>A boolean value true or false.</returns>
        public bool GetMeterData(MeterMasterEntity meterMasterEntity, GSMTaskEntity gsmTaskEntity, int retriesUsed, int retriesCount, string message, bool newMeter, AutoResetEvent manualResetEvent, ManualResetEvent allFinished, bool isModemConnected)
        {
            bool IsinstantaneousRequired = false;
            bool IsBillingRequired = false;
            bool IsloadSurveyRequired = false;
            bool IsTamperRequired = false;
            bool IsGeneralRequired = false;
            //BhardwajG : variable for holding meter model no
            //int meterModelNo = 0;
            comRetries = retriesUsed;
            simNumber = meterMasterEntity.Meter_Phone;
            gsmLoggingEntity.Task_ID = gsmTaskEntity.taskId;
            gsmLoggingEntity.Meter_ID = Thread.CurrentThread.Name.Split(':')[0].ToString();
            gsmLoggingEntity.Group_ID = gsmTaskEntity.groupId;
            IsGeneralRequired = gsmTaskEntity.isGeneralRequired;
            IsBillingRequired = gsmTaskEntity.isBillingRequired;
            IsinstantaneousRequired = gsmTaskEntity.isInstantaneousRequired;
            IsloadSurveyRequired = gsmTaskEntity.IsLoadSurveyRequired;
            IsTamperRequired = gsmTaskEntity.IsTamperRequired;
            //gsmLoggingEntity.IsGeneralCompleted = IsGeneralRequired;
            //gsmLoggingEntity.IsInstantCompleted = IsinstantaneousRequired;
            //gsmLoggingEntity.IsBillingCompleted = IsBillingRequired;
            gsmLoggingEntity.Status = "NS";
            gsmLoggingEntity.Retries = retriesUsed;
            gsmLoggingEntity.ErrorMessage = message;
            gsmLoggingEntity.Log_ID = logID;
            //string strTamperScalecapture;
            //string strTamperScalebuffer;
            //string strFileName;

            FileStream file1 = null;
            StreamWriter wr1 = null;
            bool bSuccess = true;

            directoryName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"DLMSCommunication\");

            if (UtilityEntity.PUMA == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            try
            {

                #region Reading meter ID
                if (bSuccess && !gsmLoggingEntity.IsMeterIDCompleted)
                {
                    meterIDOfCurrentlyReadMeter = string.Empty;
                    gsmLoggingEntity.IsMeterIDCompleted = ReadMeterID();
                    bSuccess = gsmLoggingEntity.IsMeterIDCompleted;
                }
                #endregion


                byte ret;


                //wr1.WriteLine("00" + FileMeterdata);
                #region Nameplate General
                //if general required and not completed
                if (IsGeneralRequired && !gsmLoggingEntity.IsGeneralCompleted)
                {

                    gsmLoggingEntity.IsGeneralCompleted = ReadGeneral();
                    bSuccess = gsmLoggingEntity.IsGeneralCompleted;
                    if (bSuccess)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "General Data read successfully..!! " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.ErrorMessage = "General data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        // It will always be true as instant profile is mandatory
                        if (IsinstantaneousRequired == true || IsBillingRequired == true || IsTamperRequired || IsloadSurveyRequired)
                        {
                            gsmLoggingEntity.Status = "IP";
                        }
                        else
                        {
                            gsmLoggingEntity.Status = "C";
                        }
                    }
                    GSMLogCreating(gsmLoggingEntity);
                    // WriteListToStream(wr1, generalList);
                }
                //else
                //{
                //    //Writes to file general data if read in previous retry, otherwise write empty data
                //    if (wr1 != null && generalList != null && generalList.Count > 0)
                //    {
                //        WriteListToStream(wr1, generalList);
                //    }
                //    else
                //    {
                //        //writing Line breaks for no data
                //        for (byte x = 0; x < 7; x++)
                //            wr1.WriteLine("05");
                //    }
                //}
                //Write to file now

                #endregion
                //raise event here
                //EventLogging.CallLogDetails(comPortName + ":" + "Before event" + Thread.CurrentThread.Name);


                //EventLogging.CallLogDetails(comPortName + ":" + "After event" + Thread.CurrentThread.Name);

                #region Instantaneous
                //Instant required and if not completed
                if (IsinstantaneousRequired)
                {
                    if (bSuccess && !gsmLoggingEntity.IsInstantCompleted)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Instantaneous data from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.IsInstantCompleted = ReadInstant();
                        bSuccess = gsmLoggingEntity.IsInstantCompleted;
                    }
                    if (bSuccess && UtilityDetails.ShowAnamolyParameters && !gsmLoggingEntity.IsAnomalyCompleted)
                    {
                        gsmLoggingEntity.IsAnomalyCompleted = ReadAnamolyParmaters();
                        bSuccess = gsmLoggingEntity.IsAnomalyCompleted;
                        if (bSuccess)
                        {
                            gsmLoggingEntity.ErrorMessage = "Instantaneous data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                            EventLogging.CallLogDetails(comPortName + ":" + "Instantaneous Data read successfully..!!" + " : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                            if (IsBillingRequired || IsloadSurveyRequired || IsTamperRequired)
                            {
                                gsmLoggingEntity.Status = "IP";
                            }
                            else
                            {
                                bSuccess = true;
                                gsmLoggingEntity.Status = "C";
                            }
                            GSMLogCreating(gsmLoggingEntity);
                        }
                    }

                }
                #endregion
                //raise event here


                #region Billing
                //Billing Required and if not completed
                if (IsBillingRequired)
                {
                    if (bSuccess && !gsmLoggingEntity.IsBillingCompleted)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Now start reading Billing data from Meter " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.IsBillingCompleted = ReadBilling();
                        bSuccess = gsmLoggingEntity.IsBillingCompleted;
                    }
                    //BhardwajG : Read TOU if tou is configured.
                    if (bSuccess && UtilityDetails.ShowTouConfiguration && !gsmLoggingEntity.IsTOUCompleted)
                    {
                        gsmLoggingEntity.IsTOUCompleted = ReadTOUandWriteInFile();
                        bSuccess = gsmLoggingEntity.IsTOUCompleted;
                        EventLogging.CallLogDetails(comPortName + ":" + "Billing Data read successfully..!! : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        if (IsloadSurveyRequired || IsTamperRequired)
                        {
                            gsmLoggingEntity.Status = "IP";
                        }
                        else
                        {
                            gsmLoggingEntity.Status = "C";
                            bSuccess = true;
                        }
                        gsmLoggingEntity.ErrorMessage = "Billing data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        GSMLogCreating(gsmLoggingEntity);
                    }
                    //else
                    //{
                    //    //writing Line breaks for no data
                    //    for (byte x = 0; x < 4; x++)
                    //        wr1.WriteLine("02");
                    //}
                }
                #endregion
                //raise event here
                //BhardwajG read demand integration period in service.
                if (UtilityDetails.ShowDIP)
                {
                    if (bSuccess && !gsmLoggingEntity.IsDIPCompleted)
                    {
                        gsmLoggingEntity.IsDIPCompleted = ReadDemandIntegrationPeriod();
                        bSuccess = gsmLoggingEntity.IsDIPCompleted;
                    }
                }
                #region EventLog Tampering
                //if tamper required and not completed
                if (bSuccess && IsTamperRequired && !gsmLoggingEntity.IsTamperCompleted)
                {
                    gsmLoggingEntity.IsTamperCompleted = ReadTamper();
                    bSuccess = gsmLoggingEntity.IsTamperCompleted;
                    if (bSuccess)
                    {
                        EventLogging.CallLogDetails(comPortName + ":" + "Tamper Data read successfully..!! : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        if (IsloadSurveyRequired)
                        {
                            gsmLoggingEntity.Status = "IP";
                        }
                        else
                        {
                            gsmLoggingEntity.Status = "C";
                        }
                        gsmLoggingEntity.ErrorMessage = "Tamper data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        GSMLogCreating(gsmLoggingEntity);
                    }
                }

                #endregion

                #region loadSurvey
                //If required and not completed
                if (bSuccess && IsloadSurveyRequired && !gsmLoggingEntity.IsLoadSurveyCompleted)
                {

                    gsmLoggingEntity.IsLoadSurveyCompleted = ReadLoadSurvey(gsmTaskEntity);
                    bSuccess = gsmLoggingEntity.IsLoadSurveyCompleted;
                    if (bSuccess)
                    {
                        gsmLoggingEntity.Status = "C";
                        EventLogging.CallLogDetails("Load Survey data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.ErrorMessage = "Load Survey data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        GSMLogCreating(gsmLoggingEntity);
                    }


                }
                //else
                //{
                //  for (byte x = 0; x < 4; x++)
                //    wr1.WriteLine("03");              //writing Line breaks for no data
                //}
                #endregion


                //moving file upload into finally so that file uploads in every retry.
                //if the retires are completed set log_id to zero to make way for new log entry.
                //if (retriesUsed >= retriesCount)
                //    gsmEventArgs.Log_ID = 0;



                //if (bSuccess == true)
                //{

                //}
                ////else
                //{
                //    File.Delete(strFileName);
                //}
            }
            catch (Exception ex)
            {
                bSuccess = false;
                EventLogging.CallLogDetails(ex.Message.ToString() + " : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
            }
            finally
            {               
                if (bSuccess)
                {
                    gsmLoggingEntity.Status = "C";
                    gsmLoggingEntity.ErrorMessage = "Data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                    GSMLogCreating(gsmLoggingEntity);
                    DLMSDisconnect();
                }
                else
                {
                    gsmLoggingEntity.Status = "NC";
                    gsmLoggingEntity.ErrorMessage = "Problem occurred while reading data. Most probable cause - weak signal strength. " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                    GSMLogCreating(gsmLoggingEntity);
                }

                //BhardwajG: Call leave modem to utility instead of disconnect,
                //leave modem to utility config will make sure that local modem's baud rate is changed t utility default baud rate.
                LeaveModemToUtilityConfig();
            }
            return bSuccess;
        }

        #endregion
        /// <summary>
        /// BhardwajG : Function for reading PCBA 
        /// </summary>
        /// <returns></returns>
        private int ReadGetPCBA()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.GetQueryReadPCBAStatus(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {

                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        /// <summary>
        /// BhardwajG : Read TOU and write data on stream
        /// </summary>
        /// <param name="wr1"></param>
        private bool ReadTOUandWriteInFile()
        {
            bool success = true;
            touList = new List<string>();
            try
            {
                MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                //iIndex = 0;
                int ret = ReadTOU(5);
                if (ret == 0x00)
                {
                    String strTemp = "";
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    //length = nBlockIndex;
                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    touList.Add("08" + strTemp);
                }
                //BhardwajG : Removing CMRI case for scheduling
                else
                {
                    MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                    success = false;
                    EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    gsmLoggingEntity.Status = "NC";
                }
                if (success)
                {
                    MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadTOU(4);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                        }
                        touList.Add("08" + strTemp);
                        success = true;
                    }
                    //BhardwajG : Removing CMRI case for scheduling
                    else
                    {

                        MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                        success = false;
                        EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.Status = "NC";
                    }
                }
                if (success)
                {
                    MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadTOU(3);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                        }
                        touList.Add("08" + strTemp);
                    }
                    //BhardwajG : Removing CMRI case for scheduling
                    else
                    {
                        MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                        success = false;
                        EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.Status = "NC";
                    }
                }
                if (success)
                {
                    MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                    //iIndex = 0;
                    ret = ReadRTC(2);
                    if (ret == 0x00)
                    {
                        String strTemp = "";
                        //BhardwajG : Do not use global objects
                        int length = objCOSEMLIB.nBlockTotalByteCount;
                        //length = nBlockIndex;
                        for (int i = 0; i < length; i++)
                        {
                            //BhardwajG : Do not use global objects
                            strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                        }
                        touList.Add("08" + strTemp);
                    }
                    //BhardwajG : Removing CMRI case for scheduling
                    else
                    {
                        MultipleSerialPortSettings.Default.ServerSAP = 0x01;
                        success = false;
                        EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        gsmLoggingEntity.Status = "NC";
                    }

                }

            }
            catch (Exception ex)
            {
                return success;
            }
            if (success)
            {
                fileList.AddRange(touList);
            }
            return success;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public int ReadRTC(byte attribute)
        {
            HDLCIndex = 0;
            HDLCCommand = new byte[200];
            objCOSEMLIB.nBlockIndex = 0x00;
            objCOSEMLIB.nBlockNumber = 0x00;
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
            objHDLCLIB.fIncSend();
            HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = objCOSEMLIB.GetQueryReadRTC(HDLCCommand, HDLCIndex, attribute);
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            try
            {

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed; ;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return (int)ProgrammingCode.CosemConnectionFailed;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                        {
                                            break;
                                        }
                                        else if (ret == 0x02)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        return (int)ProgrammingCode.CosemConnectionFailed;
                                    }
                                }
                            }

                            return (int)ProgrammingCode.Success;
                        }
                        //else if (ret == 0x05)
                        //{
                        //    return (int)CoreUtility.DLMSResultType.AccessDenied;
                        //}
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// BhardwajG : Function for reading anamoly parameters depending on the meter type
        /// </summary>
        /// <param name="wr1"></param>
        /// <returns></returns>
        private bool ReadAnamolyParmaters()
        {
            bool success = false;
            int response = 0;
            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;
            String strTemp = string.Empty;
            if (isPUMA)
            {
                response = ReadAnamoly();
            }
            else
            {
                response = ReadGetPCBA();
            }
            if (response == (int)ProgrammingCode.Success)
            {

                int length = objCOSEMLIB.nBlockTotalByteCount;
                strTemp = "07";
                for (int i = 0; i < length; i++)
                {
                    strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                }
                success = true;
                //EventLogging.CallLogDetails(comPortName + ":" + "Anamoly data read successfully : " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                //GSMLogCreating(gsmLoggingEntity);
            }
            else
            {
                strTemp = "07";
                EventLogging.CallLogDetails(comPortName + ":" + "Cosem Connection Failed " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                gsmLoggingEntity.Status = "NC";
                success = false;
            }
            if (success)
            {
                fileList.Add(strTemp);
            }
            return success;

        }
        ///BhardwajG
        /// <summary>
        /// This method is used for reading the TOU from the meter.
        /// </summary>
        /// <param name="attribute">Pleas pass the profile attribute which need to read from the meter.</param>
        /// <returns></returns>
        public int ReadTOU(byte attribute)
        {
            HDLCIndex = 0;
            HDLCCommand = new byte[200];
            objCOSEMLIB.nBlockIndex = 0x00;
            objCOSEMLIB.nBlockNumber = 0x00;
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
            objHDLCLIB.fIncSend();
            HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            HDLCIndex = objCOSEMLIB.GetQueryReadTOU(HDLCCommand, HDLCIndex, attribute);
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

            try
            {

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed; ;
                }
                else
                {
                    objHDLCLIB.fIncRecieve();
                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                HDLCIndex = 0;
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                                objHDLCLIB.fIncSend();
                                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                objHDLCLIB.fIncRecieve();
                                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return (int)ProgrammingCode.CosemConnectionFailed;
                                }
                                else
                                {
                                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                        {
                                            break;
                                        }
                                        else if (ret == 0x02)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        return (int)ProgrammingCode.CosemConnectionFailed;
                                    }
                                }
                            }

                            return (int)ProgrammingCode.Success;
                        }
                        //else if (ret == 0x05)
                        //{
                        //    return (int)CoreUtility.DLMSResultType.AccessDenied;
                        //}
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// BhardwajG : Function for reading anamoly command
        /// </summary>
        /// <returns></returns>
        private int ReadAnamoly()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ServerSAP, MultipleSerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, MultipleSerialPortSettings.Default.ClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.GetQueryReadAnamoly(HDLCCommand, HDLCIndex, 2);

                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)ProgrammingCode.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (CheckHDLCResponse(objSerialComm.ReceiveBuffer) == true)
                    {

                        int ret = objCOSEMLIB.fCheckCOSEMResponse(objSerialComm.ReceiveBuffer);
                        //int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)
                        {
                            return (int)ProgrammingCode.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)ProgrammingCode.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)ProgrammingCode.AccessDenied;
                        }
                        else
                        {
                            return (int)ProgrammingCode.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)ProgrammingCode.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)ProgrammingCode.CosemConnectionFailed;
            }
        }
        #region GSM Communication
        /// <summa
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <returns></returns>
        private string SendCommandToModem(string command)
        {
            try
            {
                const string Discription = "+++";
                string CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }
                //BhardwajG : If command is not equal to discription then add enter or 0D
                if (!command.Equals(Discription))
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');
                }
                if (objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    return "Modem Time Out.";
                }
                else
                {
                    for (int i = 0; i < objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(objSerialComm.ReceiveBuffer[i]);
                    }

                    return CommandResult;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <param name="Number">Please paas the sim number to dial proper modem.</param>
        /// <returns></returns>
        private string SendCommandToModem(string command, string Number)
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                for (int i = 0; i < Number.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToByte(Number.Substring(i, 1)) + 0x30);
                }
                //MODEMCommand[MODEMIndex++] = 0X3B; //for voice call

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');



                if (objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    for (int i = 0; i < objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(objSerialComm.ReceiveBuffer[i]);
                    }

                    return CommandResult;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// This method is used for sending description for disconnecting  to modem.
        /// </summary>
        /// <returns></returns>
        private string SendDiscription()
        {
            try
            {
                string CommandResult = "";
                MODEMIndex = 0;
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                if (objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    //////Application.DoEvents();
                    for (int i = 0; i < objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public bool CheckModemExistorAvailable(string portName)
        {
            objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.InterframeTimeout;
            objSerialComm.SetSerialPortSettings(portName, "9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
            objSerialComm.OpenPort();
            objSerialComm.CommandTimeout = 6000;
            objSerialComm.bCommType = 1;
            objSerialComm.InterchatracterDelay = 5000;
            objSerialComm.timeout = 5500;
            string Result = SendCommandToModem("AT");
            if (Result == "\r\nOK\r\n")
                return true;
            else
                return false;

        }
        /// <summary>
        ///  This method is used for connecting to a configured dlms modem.
        /// </summary>
        /// <param name="tempsimNumber">Please paas the sim number to be dialled for communication.</param>
        public bool Connect(string tempsimNumber, out string message)
        {
            message = string.Empty;
            bool matchResult = true;
            try
            {
                objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.InterframeTimeout;
                //objSerialComm.SetSerialPortSettings(comPortName, "9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                objSerialComm.SetSerialPortSettings("9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                //if (!objSerialComm.OpenPort())
                //{
                //    message = comPortName + " : Error while connecting with modem. The port is opened by some other application.";
                //    return false;
                //}
                objSerialComm.CommandTimeout = 6000;
                objSerialComm.bCommType = 1;
                objSerialComm.InterchatracterDelay = 5000;
                objSerialComm.timeout = 5500;
                string Result = SendCommandToModem("AT");
                //BhardwajG : Check that result contains Ok as PSTN response might not be equal to OK
                //BhardwajG : Match result always true as OK doesnt come in case of USB to serial converter.
                if (matchResult)
                {
                    objSerialComm.InterchatracterDelay = 70000;
                    objSerialComm.CommandTimeout = 75000;
                    objSerialComm.bCommType = 2;
                    //BhardwajG : Pick dial from .xml
                    Result = SendCommandToModem(this.dial, Thread.CurrentThread.Name.Split(':')[0].ToString());
                    //BhardwajG : check that result contains CONNECT as PSTN response might not be equal to connect 9600
                    if (Result.ToUpper().Contains("CONNECT"))
                    {
                        message = "Connected to remote modem successfully : " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        //EventLogging.CallLogDetails(comPortName + ":" + "Communication start successfully: " + Result + " " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                        return true;
                    }
                    else if (Result == "\r\nNO CARRIER\r\n" || Result == "\r\nBUSY\r\n" || Result == "\r\nNO ANSWER\r\n")
                    {
                        message = "Unable to connect to remote modem : " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        //EventLogging.CallLogDetails(comPortName + ":" + "Not able to connect to Sim number:" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString() + " " + Result);
                        return false;
                    }
                    else if (Result == "\r\nERROR\r\n")
                    {
                        message = "Error while connecting remote modem  : " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        //Disconnect();
                        //EventLogging.CallLogDetails("Rebooting local modem..");
                        //SendCommandToModem("AT+cfun=1");
                        //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                        //Thread.Sleep(10000);
                        return false;
                    }
                    else
                    {

                        message = "Unable to connect to Remote modem : " + Thread.CurrentThread.Name.Split(':')[0].ToString();
                        //objSerialComm.ClosePort();
                        EventLogging.CallLogDetails(comPortName + ":" + "Not able to connect to Sim number:" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString() + " " + Result);
                        return false;
                    }
                }
                else if (Result == "Modem Time Out.")
                {
                    message = "Local modem time out";
                    //Disconnect();
                    //EventLogging.CallLogDetails("Rebooting local modem..");
                    //RebootModem();
                    //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                    //Thread.Sleep(10000);
                    return false;
                }
                else if (Result == "\r\nERROR\r\n")
                {
                    message = "Error while connecting local modem";
                    //Disconnect();
                    //EventLogging.CallLogDetails("Rebooting local modem..");
                    //SendCommandToModem("AT+cfun=1");
                    //EventLogging.CallLogDetails("Waiting for 10 seconds..");
                    //Thread.Sleep(10000);
                    return false;
                }
                else
                {
                    message = "Local Modem not connected";
                    //objSerialComm.ClosePort();
                    //EventLogging.CallLogDetails(comPortName + ":" + "Not able to connect to Sim number:" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                //objSerialComm.ClosePort();
                EventLogging.CallLogDetails(comPortName + ":" + "Not able to connect to sim number:" + Thread.CurrentThread.Name.Split(':')[1].ToString() + " - " + Thread.CurrentThread.Name.Split(':')[0].ToString() + ex.Message.ToString());
                return false;
            }
            finally
            {
                objSerialComm.CommandTimeout = MultipleSerialPortSettings.Default.CommandTimeOut;
                objSerialComm.InterchatracterDelay = MultipleSerialPortSettings.Default.IntercharacterDelay;
                objSerialComm.bCommType = 0;
                //objSerialComm.ClosePort();
            }
        }
        #endregion
        public void GSMLogCreating(GSMLoggingEntity gsmLoggingEntity)
        {

            gsmLoggingEntity.Retries = comRetries;
            //EventLogging.CallLogDetails("Log ID" + logID + ", Task ID" + gsmLoggingEntity.Task_ID + " , Phone number " + Thread.CurrentThread.Name.Split(':')[1].ToString() + " Retries : " + comRetries + " , " + gsmLoggingEntity.ErrorMessage);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            gsmLoggingEntity.CreationDateTime = DateTime.Now;
            gsmLoggingEntity.Log_ID = logID;
            if (logID > 0)
            {
                logBLL.UpdateData(gsmLoggingEntity);
            }
            else
            {
                logBLL.InsertData(gsmLoggingEntity, false);
            }

            //this.LogID = gsmLoggingEntity.Log_ID;
        }
    }
}
