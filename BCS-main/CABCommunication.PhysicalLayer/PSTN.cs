#region Namespaces
using System;
using System.Collections.Generic;
using System.IO.Ports;
using Hunt.EPIC.Logging;
using CAB.Framework;
using CAB.Serialization;
using CABCommunication.Common;
#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// This interface defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    public class PSTN : SerialPort, IPhysicalChannel
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        private string baudRate;
        private string parity;
        private string stopBits;
        private string dataBits;
        private long responseTimeout;
        private long intercharacterDelay;
        private byte noOfRetry;
        public ChannelType channelType;
        private byte[] receiveBuffer;
        //making it equal to the default value of serial port's readBufferSize
        private const int MaxRecieveLength = 250000;
        private bool readFlag = false;
        private bool dataRecievedFlag;
        private long elapsedMilliseconds;
        private long timeout;
        private DateTime timeStamp;
        private int bufferIndex = 0;
        static object syncRoot = new object();
        static Serializer serializer = null;
        static ModemConfig modemConfig = null;
        private string dataNumber = string.Empty;
        private const string DLMS = "DLMS";
        private const string NonDLMS = "NonDLMS";
        private ModemConfigProperties modemConfigProperties = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Serial).ToString());
        private ChannelInformation channelInformation = new ChannelInformation();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SIM number
        /// </summary>
        public string SimNumber { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// This operation will be invoked by User to assign channel properties.
        /// </summary>
        /// <param name="channelDetail"></param>
        public PSTN(ChannelInformation channelInfo)
        {
            channelInformation = channelInfo;
            this.SimNumber = channelInfo.ModemInfo;
            this.PortName = channelInfo.ComPort;
        }
        /// <summary>
        /// 
        /// </summary>
        static PSTN()
        {
            serializer = new Serializer();
            modemConfig = (ModemConfig)serializer.DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "Modem.xml", typeof(ModemConfig));
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public void SetBaud(byte baud)
        {

        }

        /// <summary>
        /// Opens the GSM session with remote meter assumes.
        /// </summary>
        /// <returns></returns>
        public bool OpenSession()
        {
            this.modemConfigProperties = GetModemConfigProperties(channelInformation.CommunicationMode + DLMS);
            dataNumber = SimNumber;
            Result response = new Result();
            response.ErrorCode = CommunicationErrorType.Nothing;
            this.Close();
            this.baudRate = modemConfigProperties.Portsettings[0].BitsPerSecond;
            this.stopBits = modemConfigProperties.Portsettings[0].Stopbits;
            this.parity = modemConfigProperties.Portsettings[0].Parity;
            this.dataBits = modemConfigProperties.Portsettings[0].Databits;
            this.responseTimeout = modemConfigProperties.Commandsettings[0].CommandTimeout;
            this.intercharacterDelay = modemConfigProperties.Commandsettings[0].InterCharacterTimeout;
            this.noOfRetry = (byte)modemConfigProperties.Commandsettings[0].DLMSRetries;
            this.ReadBufferSize = 4096;

            if (OpenSessionWithDelay())
            {

                //Assume that DLMS modem is present
                response = ConnectLocalModem(modemConfigProperties);
                if (response.ErrorCode == CommunicationErrorType.LocalModemConnected)
                {
                    //connect to remote modem
                    response = ConnectRemoteModem(modemConfigProperties);
                    //if remote modem is not connected than the remote modem could be of Non-DLMS
                    if (response.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (channelInformation.ProtocolType == "DLMS")
                        {
                            response.ErrorCode = CommunicationErrorType.SuccessForDLMS;
                        }
                        else
                        {
                            byte[] discData = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x53, 0x7B, 0x73, 0x7E };
                            response = this.Send(discData, 12);
                            if (response.RecieveDataLength > 11)
                            {
                                response.ErrorCode = CommunicationErrorType.SuccessForDLMS;
                            }
                        }
                    }
                    else
                    {
                        response.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                    }

                    if (channelInformation.ProtocolType == "Generic" && response.ErrorCode != CommunicationErrorType.SuccessForDLMS)
                    {
                        response.ErrorCode = ConnectIECModem();
                    }
                }
            }
            if (response.ErrorCode == CommunicationErrorType.SuccessForDLMS || response.ErrorCode == CommunicationErrorType.SuccessForIEC)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CommunicationErrorType ConnectIECModem()
        {
            Result response = new Result();
            response.ErrorCode = CommunicationErrorType.Nothing;
            this.modemConfigProperties = GetModemConfigProperties(channelInformation.CommunicationMode + NonDLMS);
            Close();
            //Try opening the port with Non-DLMS settings
            this.baudRate = modemConfigProperties.Portsettings[0].BitsPerSecond;
            this.stopBits = modemConfigProperties.Portsettings[0].Stopbits;
            this.parity = modemConfigProperties.Portsettings[0].Parity;
            this.dataBits = modemConfigProperties.Portsettings[0].Databits;
            this.ReadBufferSize = 10000;
            if (OpenSessionWithDelay())
            {
                response = ConnectLocalModem(modemConfigProperties);
                if (response.ErrorCode == CommunicationErrorType.LocalModemConnected)
                {
                    response = ConnectRemoteModem(modemConfigProperties);
                    if (response.ErrorCode == CommunicationErrorType.Success)
                    {
                        response.ErrorCode = CommunicationErrorType.ConnectedNonDLMS;
                    }
                }
            }
            return response.ErrorCode;
        }

        /// <summary>
        /// This operation will be invoked by the Adapter user to close session(s).
        /// This will also close the opened com/IP port.
        /// </summary>
        /// <returns></returns>
        public bool CloseSession()
        {
            bool success = false;
            try
            {
                CloseRemoteSession();
                if (this.IsOpen)
                {
                    this.DiscardInBuffer();
                    this.Close();
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                logger.Log(LOGLEVELS.Error, "Error while Closing Session", ex);
            }
            return success;
        }


        /// <summary>
        /// Sends data/command to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result Send(byte[] data, int dataLength)
        {
            this.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            Result result = null;
            try
            {
                result = SendCommand(data, dataLength);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            finally
            {
                this.DataReceived -= new SerialDataReceivedEventHandler(comPort_DataReceived);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandLength"></param>
        /// <param name="remoteModem"></param>
        /// <returns></returns>
        public Result Send(byte[] command, int commandLength, bool remoteModem)
        {
            Result result = null;
            if (remoteModem)
            {
                result = SendGSMRemoteCommand(command, commandLength);
            }
            else
            {
                result = SendGSMCommand(command, commandLength);
            }
            return result;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handle Data Received method of Com port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                dataRecievedFlag = true;
                timeout = this.intercharacterDelay;
                timeStamp = DateTime.Now;
                int nCount = this.BytesToRead;
                SerialPort sp = (SerialPort)sender;
                //For Normal
                sp.Read(receiveBuffer, bufferIndex, nCount);
                bufferIndex = bufferIndex + nCount;
                if (receiveBuffer[receiveBuffer[2] + 1] == 0x7E)
                    readFlag = true;

            }

            catch (Exception ex)
            {
                //   do nothing
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }

        }
        /// <summary>
        /// Handle Data Received method of Com port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comGSMPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                dataRecievedFlag = true;
                timeout = this.intercharacterDelay;
                timeStamp = DateTime.Now;
                int nCount = this.BytesToRead;
                SerialPort sp = (SerialPort)sender;
                //For Normal
                sp.Read(receiveBuffer, bufferIndex, nCount);
                bufferIndex = bufferIndex + nCount;
                //If the command is of AT type then check that at least 4 bytes are received.
                if (bufferIndex > 4)
                {
                    if ((receiveBuffer[0] == 0x0D) && (receiveBuffer[1] == 0x0A) && (receiveBuffer[bufferIndex - 2] == 0x0D) && (receiveBuffer[bufferIndex - 1] == 0x0A))
                    {
                        readFlag = true;
                    }
                }


            }

            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                //   do nothing
            }

        }
        #endregion

        #region Private Methods


        /// <summary>
        /// Sends data/command to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result SendGSMCommand(byte[] data, int dataLength)
        {
            this.DataReceived += new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
            Result result = null;
            try
            {
                result = SendCommand(data, dataLength);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            finally
            {
                this.DataReceived -= new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
            }
            return result;
        }

        /// <summary>
        /// Sends data/command to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result SendGSMRemoteCommand(byte[] data, int dataLength)
        {
            this.DataReceived += new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
            Result result = null;
            try
            {
                responseTimeout = 70000;
                intercharacterDelay = 65000;
                result = SendCommand(data, dataLength);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            finally
            {
                this.DataReceived -= new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
                responseTimeout = modemConfigProperties.Commandsettings[0].CommandTimeout;
                intercharacterDelay = modemConfigProperties.Commandsettings[0].InterCharacterTimeout;
            }
            return result;
        }

        /// <summary>
        /// Opens the session and gives a delay suitable for GSM communication in multithreaded environment as
        /// the actual serial port takes a while closing the real serial port.
        /// </summary>
        /// <returns></returns>
        private bool OpenSessionWithDelay()
        {
            lock (syncRoot)
            {

                if (IsOpen == true)
                {
                    DiscardInBuffer();
                    Close();
                    DelayExecution(2000);
                }
                this.BaudRate = int.Parse(baudRate);
                this.DataBits = int.Parse(dataBits);
                this.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.stopBits);
                this.Parity = (Parity)Enum.Parse(typeof(Parity), this.parity);
                //this.PortName = this.portName;
                this.Handshake = Handshake.None;


                this.RtsEnable = true;
                this.DtrEnable = true;
                try
                {
                    Open();
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                    return false;

                }
            }
        }

        /// <summary>
        /// /// Opens the session and gives a delay suitable for GSM communication in multithreaded environment as
        /// the actual serial port takes a while closing the real serial port.
        /// </summary>
        /// <returns></returns>
        private bool CloseSessionWithDelay()
        {
            try
            {
                lock (syncRoot)
                {

                    if (IsOpen)
                    {
                        DiscardInBuffer();
                        Close();
                        DelayExecution(2000);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                return false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CloseRemoteSession()
        {
            bool success = false;
            try
            {
                string[] resetCommands = GetResetATCommands();
                if (resetCommands != null && resetCommands.Length > 0)
                {
                    foreach (string command in resetCommands)
                    {
                        SendCommandToModem(command, false);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            finally
            {

            }
            return success;

        }

        /// <summary>
        /// Gets Various timeouts i.e Interframe , response etc.
        /// </summary>
        /// <returns></returns>
        private CommunicationErrorType Timeout()
        {
            CommunicationErrorType errorType = CommunicationErrorType.Nothing;
            long elapsedTime = DateTime.Now.Ticks - timeStamp.Ticks;

            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);

            elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);

            if (dataRecievedFlag == true)
            {
                if (elapsedMilliseconds > timeout)
                {
                    errorType = CommunicationErrorType.InterFrameTimeout;
                }
            }
            else
            {
                if (elapsedMilliseconds > timeout)
                {
                    errorType = CommunicationErrorType.ResponseTimeout;
                }

            }
            return errorType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private Result SendCommand(byte[] data, int dataLength)
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.ResponseTimeout;
            result.RecieveDataBuffer = new List<byte>();
            byte retries = 1;
            readFlag = false;
            bufferIndex = 0;

            //Intializing the receive buffer and data received flag before
            //data is written to serial port
            //Fix for TFS Bug : 186256
            dataRecievedFlag = false;
            receiveBuffer = new byte[MaxRecieveLength];
            if (this.IsOpen)
            {
                this.DiscardOutBuffer();
                this.DiscardInBuffer();
                this.Write(data, 0, dataLength);
            }

            //set the current timestamp for calculating timeout delay

            timeStamp = DateTime.Now;
            timeout = this.responseTimeout;


            //check untill response is recived within the timeout limit
            do
            {
                System.Threading.Thread.Sleep(10);
                CommunicationErrorType errorType = Timeout();
                if (errorType == CommunicationErrorType.ResponseTimeout)
                {
                    if (retries > this.noOfRetry)
                    {
                        break;
                    }
                    else
                    {

                        timeStamp = DateTime.Now;
                        timeout = this.responseTimeout;
                        dataRecievedFlag = false;
                        retries++;
                        bufferIndex = 0;
                        readFlag = false;
                        receiveBuffer = new byte[MaxRecieveLength];
                        this.DiscardOutBuffer();
                        this.DiscardInBuffer();
                        this.Write(data, 0, dataLength);
                    }
                }
                else if (errorType == CommunicationErrorType.InterFrameTimeout)
                {
                    if (readFlag)
                    {
                        result.ErrorCode = CommunicationErrorType.Success;
                        for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
                        {
                            result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
                        }
                        result.RecieveDataLength = bufferIndex;
                        bufferIndex = 0;
                        readFlag = false;
                    }
                    else
                    {
                        if (bufferIndex > 1)
                        {
                            for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
                            {
                                result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
                            }
                            result.RecieveDataLength = bufferIndex;
                            bufferIndex = 0;
                            readFlag = false;
                            result.ErrorCode = CommunicationErrorType.Success;
                        }
                        else
                        {
                            bufferIndex = 0;
                            result.ErrorCode = CommunicationErrorType.InterFrameTimeout;
                        }

                    }
                    break;
                }
                else
                {
                    if (readFlag == true)
                    {
                        result.ErrorCode = CommunicationErrorType.Success;
                        for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
                        {
                            result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
                        }
                        result.RecieveDataLength = bufferIndex;
                        bufferIndex = 0;
                        break;

                    }
                }
            } while (true);
            return result;
        }
        /// <summary>
        /// Delays the execution suitable for GSM communication
        /// </summary>
        /// <param name="millisecondsTime"></param>
        private void DelayExecution(int millisecondsTime)
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(millisecondsTime);
            while (DateTime.UtcNow < end)
            {
            }
        }
        /// <summary>
        /// Returns the modem config properties according to communication type.
        /// </summary>
        /// <param name="comType"></param>
        /// <returns></returns>
        private ModemConfigProperties GetModemConfigProperties(string comType)
        {
            ModemConfigProperties configProperites = null;
            foreach (ModemConfigProperties properties in modemConfig.Items)
            {
                if (properties.Name.ToLower() == comType.ToLower())
                {
                    configProperites = properties;
                    break;
                }
            }
            return configProperites;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private string[] GetInitATCommands(ModemConfigProperties modemConfigProperties)
        {
            string[] initCommands = null;

            if (modemConfigProperties != null)
            {
                //Read Command settings section
                if (modemConfigProperties.Commandsettings != null && modemConfigProperties.Commandsettings.Length > 0)
                {
                    initCommands = modemConfigProperties.Commandsettings[0].Initialize.Split('|');
                }
            }
            return initCommands;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private string GetDialString(ModemConfigProperties modemConfigProperties)
        {
            string dial = string.Empty;
            if (modemConfigProperties != null)
            {
                //Read Command settings section
                if (modemConfigProperties.Commandsettings != null && modemConfigProperties.Commandsettings.Length > 0)
                {
                    dial = modemConfigProperties.Commandsettings[0].Dial;
                }
            }
            return dial;

        }
        /// <summary>
        /// Get Reset AT commands from 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="comType"></param>
        /// <returns></returns>
        private string[] GetResetATCommands()
        {
            string[] initCommands = null;
            if (modemConfigProperties != null)
            {
                //Read Command settings section
                if (modemConfigProperties.Commandsettings != null && modemConfigProperties.Commandsettings.Length > 0)
                {
                    initCommands = modemConfigProperties.Commandsettings[0].Reset.Split('|');
                }
            }
            return initCommands;
        }
        /// <summary
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <returns></returns>
        private string SendCommandToModem(string command, bool remoteModem)
        {
            try
            {
                int modemIndex = 0;
                byte[] modemCommand = new byte[20];
                const string Discription = "+++";
                string CommandResult = "";
                modemIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    modemCommand[modemIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }
                //BhardwajG : If command is not equal to discription then add enter or 0D
                if (!command.Equals(Discription))
                {
                    modemCommand[modemIndex++] = Convert.ToByte('\r');
                }
                Result result = Send(modemCommand, modemIndex, remoteModem);
                if (result.ErrorCode != CommunicationErrorType.Success)
                {
                    return "Modem Time Out.";
                }
                else
                {
                    for (int i = 0; i < result.RecieveDataLength; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(result.RecieveDataBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                throw;

            }
        }
        /// <summary>
        /// Connect to local modem.
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private Result ConnectLocalModem(ModemConfigProperties modemConfigProperties)
        {

            Result response = new Result();
            string[] initCommands = GetInitATCommands(modemConfigProperties);
            string result = string.Empty;
            for (int counter = 0; counter < initCommands.Length; counter++)
            {
                // objSerialComm.bCommType = 1;
                result = SendCommandToModem(initCommands[counter], false);
                if (initCommands[counter].Equals("ATH") || initCommands[counter].Equals("ATE0") || initCommands[counter].Equals("AT"))
                {
                    if (result.ToUpper().Contains("OK"))
                    {
                        response.ErrorCode = CommunicationErrorType.LocalModemConnected;
                    }
                    else
                    {
                        response.ErrorCode = CommunicationErrorType.LocalModemNotConnected;
                        break;
                    }
                }
                else
                {
                    response.ErrorCode = CommunicationErrorType.LocalModemConnected;
                }
            }
            return response;
        }
        /// <summary>
        /// Connects to remote modem
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private Result ConnectRemoteModem(ModemConfigProperties modemConfigProperties)
        {

            string result = null;
            Result response = new Result();
            try
            {
                string dial = GetDialString(modemConfigProperties);
                dial = string.Concat(dial, dataNumber);
                if (!string.IsNullOrEmpty(dial))
                {
                    result = SendCommandToModem(dial, true);
                    if (result.ToUpper().Contains("CONNECT"))
                    {
                        response.ErrorCode = CommunicationErrorType.Success;
                    }
                    else
                    {
                        //this.StatusMessage = result;
                        response.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            return response;
        }

        #endregion


    }
}
