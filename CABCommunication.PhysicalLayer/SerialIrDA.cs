#region Namespaces
using System;
using System.IO.Ports;
using CABCommunication.Common;
using System.Collections.Generic;
using CAB.Serialization;
using CAB.Framework;
using Hunt.EPIC.Logging;
using CAB.Framework.Utility;
#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// This interface defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    public class SerialIrDA : SerialPort,IPhysicalChannel
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
        private string dataNumber = string.Empty;
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
        static SerialIrDA()
        {
            serializer = new Serializer();
        }
        /// <summary>
        /// Initialize Serial Communication instance
        /// </summary>
        /// <param name="compPort"></param>
        public SerialIrDA(string compPort)
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
        public SerialIrDA()
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
        public SerialIrDA(ChannelInformation channelInfo)
        {
            baudRate = ConfigSettings.GetValue("BaudRate");
            initialBaudRate = "9600";
            parity = "None"; ;
            stopBits = "1";
            dataBits = "8";
            responseTimeout = 1000;
            timeout = 1000;
            intercharacterDelay = 1000;
            noOfRetry = channelInfo.NoOfRetries;
            channelType = ChannelType.Direct;
            this.portName = channelInfo.ComPort;

        }

              
        /// <summary>
        /// This operation will be invoked by User to assign channel properties.
        /// </summary>
        /// <param name="channelDetail"></param>
        public SerialIrDA(ChannelDetail channelDetail)
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

        public Result SendCommand(byte[] Buffer, int nLength)
        {
            Result result = new Result()
            {
                ErrorCode=CommunicationErrorType.Nothing,
                RecieveDataBuffer=new List<byte>()
            };

            int nRetries = 1;
            int discCommandByte = 8;
            int lenDatatobeReceive = 0;
            while (nRetries-- > 0)
            {

                readFlag = false;
                bufferIndex = 0;
                receiveBuffer = new byte[MaxRecieveLength];
                dataRecievedFlag = false;
                this.Write(Buffer, 0, nLength);
                timeStamp = DateTime.Now;
                do
                {
                    //if (Buffer[discCommandByte] == 83) return true;
                    System.Threading.Thread.Sleep(10);
                    if (dataRecievedFlag)
                    {
                        //result.ErrorCode = CommunicationErrorType.Success;
                    }
                    CommunicationErrorType errorType = Timeout();
                    if (errorType == CommunicationErrorType.ResponseTimeout)
                    {
                        result.ErrorCode = errorType;
                        break;
                    }
                    if (bufferIndex >= 5) lenDatatobeReceive = receiveBuffer[5];
                    if (lenDatatobeReceive > 0 && bufferIndex >= lenDatatobeReceive)
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

                } while (true);
                if (Buffer[discCommandByte] == 83) break;
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
        public void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
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

                if (nCount < 0) return;

                if (nCount >= 5)
                {
                    dataRecievedFlag = true;
                }
                bufferIndex = bufferIndex + nCount;
            }
            catch (Exception ex)
            {
                //   do nothing
                logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
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
        private Result SendCommand_OLD(byte[] data, int dataLength)
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.ResponseTimeout;
            result.RecieveDataBuffer = new List<byte>();
            byte retries = 1;
            readFlag = false;
            bufferIndex = 0;
            int discCommandByte = 8;
            int lenDatatobeReceive = 0;
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
        public void DelayExecution(int millisecondsTime)
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(millisecondsTime);
            while (DateTime.UtcNow < end)
            {
            }
        }
        #endregion



    }
}
