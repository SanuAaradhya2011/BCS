#region Namespaces
using System;
using System.IO.Ports;
using CABCommunication.Common;
using System.Collections.Generic;
using CAB.Serialization;
using CAB.Framework;
using Hunt.EPIC.Logging;
#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// This interface defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    public class Serial : SerialPort,IPhysicalChannel
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        private string baudRate;
        private string initialBaudRate;
        private string parity;
        private string stopBits;
        private string dataBits;
        private string portName;
        private long responseTimeout;
        private long intercharacterDelay;
        public byte noOfRetry;
        public ChannelType channelType;
        private byte[] receiveBuffer;
        //making it equal to the default value of serial port's readBufferSize
        private const int MaxRecieveLength = 5000000;//Buffer increases to 50 Lakhs bytes from 20 Lakhs Bytes for CMRI 


        private bool readFlag = false;
        private bool dataRecievedFlag;
        private long elapsedMilliseconds;
        private long timeout;
        private DateTime timeStamp;
        private int bufferIndex = 0;
        /// <summary>
        /// Initializing syncroot for thread syn.
        /// </summary>
        static object syncRoot = new object();
        static Serializer serializer = null;
        static ModemConfig modemConfig = null;
        private string dataNumber = string.Empty;
        private const string DLMS = "DLMS";
        private const string NonDLMS = "NonDLMS";
        private ModemConfigProperties modemConfigProperties = null;
        private CommunicationErrorType connectionType = CommunicationErrorType.Nothing;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Serial).ToString());
        
        #endregion

        #region Properties
        /// <summary>
        /// The property will store the connection type made for current session
        /// </summary>
        private CommunicationErrorType ConnectionType
        {
            get
            {
                return connectionType;
            }
            set
            {
                connectionType = value;
            }
        }
        #endregion

        #region Constructor
        static Serial()
        {
            serializer = new Serializer();
            modemConfig = (ModemConfig)serializer.DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "Modem.xml", typeof(ModemConfig));
        }
        /// <summary>
        /// Initialize Serial Communication instance
        /// </summary>
        /// <param name="compPort"></param>
        public Serial(string compPort)
        {
            baudRate = "9600";
            initialBaudRate = "9600";
            parity = "None"; ;
            stopBits = "1";
            dataBits = "8";
            portName = compPort;
            responseTimeout = 9000;
            intercharacterDelay = 5000;
            noOfRetry = 2;
            channelType = ChannelType.Direct;

        }
        public Serial()
        {
            baudRate = "9600";
            initialBaudRate = "9600";
            parity = "None"; ;
            stopBits = "1";
            dataBits = "8";
            responseTimeout = 9000;
            intercharacterDelay = 5000;
            noOfRetry = 2;
            channelType = ChannelType.Direct;

        }
        public Serial(ChannelInformation channelInfo)
        {
            baudRate = "9600";
            initialBaudRate = "9600";
            parity = "None"; ;
            stopBits = "1";
            dataBits = "8";
            responseTimeout = 9000;
            intercharacterDelay = 5000;
            noOfRetry = 2;
            channelType = ChannelType.Direct;
            this.portName = channelInfo.ComPort;

        }

              
        /// <summary>
        /// This operation will be invoked by User to assign channel properties.
        /// </summary>
        /// <param name="channelDetail"></param>
        public Serial(ChannelDetail channelDetail)
        {
            baudRate = channelDetail.BaudRate;
            initialBaudRate = channelDetail.InitialBaudRate;
            parity = channelDetail.Parity;
            stopBits = channelDetail.StopBits;
            dataBits = channelDetail.DataBits;
            portName = channelDetail.ComPort;
            responseTimeout = channelDetail.ResponseTimeout;
            intercharacterDelay = channelDetail.InterCharacterDelay;
            noOfRetry = channelDetail.NumberOfRetry;
            channelType = channelDetail.ChannelType;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelDetail"></param>
        public void SetChannel(ChannelDetail channelDetail)
        {
            baudRate = channelDetail.BaudRate;
            initialBaudRate = channelDetail.InitialBaudRate;
            parity = channelDetail.Parity;
            stopBits = channelDetail.StopBits;
            dataBits = channelDetail.DataBits;
            portName = channelDetail.ComPort;
            responseTimeout = channelDetail.ResponseTimeout;
            intercharacterDelay = channelDetail.InterCharacterDelay;
            noOfRetry = channelDetail.NumberOfRetry;
            channelType = channelDetail.ChannelType;
            this.connectionType = CommunicationErrorType.Nothing;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public void SetBaud(byte baud)
        {          

            switch (baud)
            {
                case 5 :
                    {
                        this.baudRate = "9600";
                        this.BaudRate = 9600;
                    }
                    break;
                case 6:
                    {
                        this.baudRate = "19200";
                        this.BaudRate = 19200;
                    }
                    break;
                case 7:
                    {
                        this.baudRate = "38400";
                        this.BaudRate = 38400;
                    }
                    break;
                default:
                    {
                        this.baudRate = "9600";
                        this.BaudRate = 9600;
                    }
                    break;
            }
             
        }
        /// <summary>
        /// This operation will be invoked by the Adapter user to open session(s) .
        /// This will also open specified com/IP port.
        /// </summary>
        /// <returns></returns>
        public bool OpenSession()
        {

            bool success = false;
            try
            {
                if (this.IsOpen == true)
                {
                    this.DiscardInBuffer();
                    this.Close();
                }                
                this.BaudRate = int.Parse(baudRate);
                this.DataBits = int.Parse(dataBits);
                this.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.stopBits);
                this.Parity = (Parity)Enum.Parse(typeof(Parity), this.parity);
                this.PortName = this.portName;
                this.Handshake = Handshake.None;


                this.RtsEnable = true;
                this.DtrEnable = true;

                this.Open();
                success = true;

              

            }
            catch (Exception ex)
            {
                success = false;
                 logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            return success;
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
                if (this.IsOpen)
                {
                    this.DiscardInBuffer();
                    this.Close();
                }
                success = true;
            }
            catch(Exception ex)
            {
                success = false;
                 logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            return success;
        }
        /// <summary>
        /// Opens the GSM session with remote meter assumes.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Result OpenSession(string phoneNumber)
        {
            
            this.modemConfigProperties = GetModemConfigProperties(channelType.ToString() + DLMS);
            //this.modemConfigProperties = GetModemConfigProperties("GSM" + DLMS);
            dataNumber = phoneNumber;
            Result response = new Result();
            response.ErrorCode = CommunicationErrorType.Nothing;

            this.Close();
            this.baudRate = modemConfigProperties.Portsettings[0].BitsPerSecond;
            this.stopBits = modemConfigProperties.Portsettings[0].Stopbits;
            this.parity = modemConfigProperties.Portsettings[0].Parity;
            this.dataBits = modemConfigProperties.Portsettings[0].Databits;
            this.ReadBufferSize = 4096;
            //OpenSession();   
            OpenSessionWithDelay();

            //Assume that DLMS modem is present
            response = ConnectLocalModem(modemConfigProperties);
            if (response.ErrorCode == CommunicationErrorType.LocalModemConnected)
            {
                //connect to remote modem
                response = ConnectRemoteModem(modemConfigProperties);
                //if remote modem is not connected than the remote modem could be of Non-DLMS
                if (response.ErrorCode == CommunicationErrorType.Success)
                {
                    //byte[] data = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x93, 0x77, 0xB5, 0x7E };
                    //response = this.Send(data, 12);
                    //if (response.RecieveDataLength > 11)
                    //{
                        byte[] discData = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x53, 0x7B, 0x73, 0x7E };
                        response = this.Send(discData, 12);
                        if (response.RecieveDataLength > 11)
                        {
                            response.ErrorCode = CommunicationErrorType.ConnectedDLMS;

                        }
                        else
                        {
                            byte[] signOnIEC = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
                            response = this.Send(signOnIEC, 5);
                            if (response.RecieveDataLength > 11)
                            {
                                byte[] breakCmd = { 0x01, 0x42, 0x30, 0x03, 0x71 };
                                response = this.Send(breakCmd, 5);
                                response.ErrorCode = CommunicationErrorType.SuccessForIECSP;
                            }
                        }
                    //}
                }
                else
                {
                    response.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;

                }
            }
            if (response.ErrorCode != CommunicationErrorType.ConnectedDLMS && response.ErrorCode != CommunicationErrorType.SuccessForIECSP)
            {
                this.modemConfigProperties = GetModemConfigProperties(channelType.ToString() + NonDLMS);
                ConnectLocalModem(modemConfigProperties);
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
                            connectionType = CommunicationErrorType.ConnectedNonDLMS;
                        }
                        else
                        {
                            connectionType = CommunicationErrorType.RemoteModemNotConnected;
                        }
                    }
                }
            }
            return response;
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
            catch(Exception ex)
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
        /// <summary>
        /// Sends data/command to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result SendGSMCommand(byte[] data, int dataLength)
        {
            this.DataReceived += new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
            Result result = null;
            try
            {
                result = SendCommand(data, dataLength);
            }
            catch(Exception ex)
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
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CloseRemoteSession()
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
        /// Sends data/command to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result SendGSMRemoteCommand(byte[] data, int dataLength)
        {
            this.DataReceived += new SerialDataReceivedEventHandler(comGSMPort_DataReceived);
            Result result = null;
            try
            {
                responseTimeout = 70000;
                intercharacterDelay = 65000;
                result = SendCommand(data, dataLength);
            }
            catch(Exception ex)
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
        public bool OpenSessionWithDelay()
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
                this.PortName = this.portName;
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
        public bool CloseSessionWithDelay()
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
                /*--To Handle only DLMS meter Data -- In Case of CMRI Communication Continue reading--*/
                //05-01-2017
                //if (receiveBuffer[0] == 0x7E && receiveBuffer[(((receiveBuffer[1] & 0x0F) << 8) | (int)receiveBuffer[2]) + 1] == 0x7E)

                //Segmenatation user story no: 520412 "To Handle large block data and Segmentation data along with blockSegementation Data"
                int nHDLCPktLength = ((receiveBuffer[1] & 0x07) << 8) | (int)receiveBuffer[2];
                if (receiveBuffer[0] == 0x7E && receiveBuffer[nHDLCPktLength + 1] == 0x7E)
                {
                    readFlag = true;
                }
            }

            catch(Exception ex)
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

            catch(Exception ex)
            {
                 logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                //   do nothing
            }

        }
        #endregion

        #region Private Methods
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
            else
            {
                logger.Log(LOGLEVELS.Error, "Port Status is Closed, Can't Send Any Command");
                result.ErrorCode = CommunicationErrorType.PortInvalid;
                return result;
            }
            
            //set the current timestamp for calculating timeout delay

            timeStamp = DateTime.Now;
            responseTimeout = 60000;
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
        public void DelayExecution(int millisecondsTime)
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
