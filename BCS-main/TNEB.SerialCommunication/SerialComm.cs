///****************************************************************************
//'*
//'*  Projet       : DLMS With LTCT
//'*
//'*  Component    : MMP
//'*
//'*  Module       : Serial Communication
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 10/08/09 | Gopal Krishna Gupta : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Xml;
using System.Threading;
namespace SerialCommunication
{
    public class SerialComm
    {
        public delegate void OnClose(string portName);
        public event OnClose Close;
        //BhardwajG : Delegate for time out event
        public delegate void OnTimedOut();
        //BhardwajG : Event for time out
        public event OnTimedOut TimedOut;

        #region Declaration
        public byte[] ReceiveBuffer = new byte[255];
        public byte[] fastDownLoadBuffer = new byte[522000];
        public byte[] fastDownloadBufferCMRI = new byte[668628];
        public int bufferIndex = 0;
        public byte bCommType = 0;
        int nCount = 0;
        #endregion

        #region Manager Variables
        public SerialPort comPort;
        private string _baudRate;
        private string _parity;
        private string _stopBits;
        private string _dataBits;
        private string _portName;
        private string _command;
        private long _cmriWaitTimeout;
        private int _cmriPktTimeout;
        private int _commandTimeout;
        private int _intercharacterDelay;
        private int _intercommandDelay = 0;
        public int Pktflg = 0;
        public string strOutBuff = "";
        public int commandIndex = 0;
        public long commCount = 0;
        public bool flgReadFlag = false;
        public bool flgDataReceived;
        public long elapsedMilliseconds;
        public long timeout;
        public int pktCount = 0;
        public DateTime TimeStamp;
        private object syncRoot = new object();
        //BhardwajG : variable for holding retries
        private int retries = 4;
        #region FastDownLoading Properties
        //Property Added on 24 Feb 2012 for 
        private int fastDownLoadBufferSize;
        public int FastDownLoadBufferSize
        {
            get { return fastDownLoadBufferSize; }
            set { fastDownLoadBufferSize = value; }
        }
        public delegate void OnFDLStatusChanged(string statusMessage);
        public event OnFDLStatusChanged onfdlStatusChanged;
        #endregion

        #region Manager Constructors

       
        public SerialComm()
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }
        public SerialComm(bool flag, int configuredRetries)
        {
            //BhardwajG : API constructor
            this.retries = configuredRetries;
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }
        public SerialComm(string portName)
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            PortName = portName;
            comPort.PortName = portName;
        }
        public SerialComm(int bufferSize)
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            fastDownLoadBuffer = new byte[bufferSize];

            comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceivedFastDownLoad);
        }

        public SerialComm(int bufferSize, bool flag)
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _command = string.Empty;
            _cmriWaitTimeout = 0;
            _cmriPktTimeout = 0;
            _commandTimeout = 0;
            _intercharacterDelay = 0;
            _intercommandDelay = 0;
            flgDataReceived = false;
            fastDownloadBufferCMRI = new byte[bufferSize];

            comPort = new SerialPort();
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceivedFastDownLoadForCMRI);
        }

        #endregion

        #region Manager Properties
        /// <summary>
        /// BhardwajG : Property for holding dlms retries
        /// </summary>
        public int DLMSRetries
        {
            get
            {
                return retries;
            }
            set
            {
                retries = value;
            }
        }
        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        public string StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public string DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        public string PortName
        {
            get { return _portName; }

            set
            {
                lock (syncRoot)
                {
                    _portName = value;
                }
            }
        }

        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        public int InterchatracterDelay
        {
            get { return _intercharacterDelay; }
            set { _intercharacterDelay = value; }
        }

        public int IntercommandDelay
        {
            get { return _intercommandDelay; }
            set { _intercommandDelay = value; }
        }

        public long CMRIWaitTimeout
        {
            get { return _cmriWaitTimeout; }
            set { _cmriWaitTimeout = value; }
        }

        public int CMRIPktTimeout
        {
            get { return _cmriPktTimeout; }
            set { _cmriPktTimeout = value; }
        }

        #endregion

        #region WriteData

        internal void WriteData(string msg)
        {
            if (!(comPort.IsOpen == true)) comPort.Open();
            comPort.Write(msg);
        }

        internal void WriteData(byte[] msg, int offset, int count)
        {
            if (!(comPort.IsOpen == true))
            {
                comPort.Open();
            }
            comPort.Write(msg, offset, count);
        }
        #endregion
        /// <summary>
        /// The function raises the time out event if the command is not AT command
        /// </summary>
        private void OnTimeOut()
        {
            if (bCommType == 0x00)
            {
                if (TimedOut != null)
                {
                    TimedOut();
                }
            }
        }
        #region OpenPort
        public bool OpenPort()
        {
            lock (syncRoot)
            {

                if (comPort.IsOpen == true)
                {
                    comPort.DiscardInBuffer();
                    comPort.Close();

                }
                comPort.BaudRate = int.Parse(this.BaudRate);    //BaudRate
                comPort.DataBits = int.Parse(this.DataBits);          //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.StopBits);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), this.Parity);    //Parity
                comPort.PortName = "COM1";  //PortName
                //now open the port
                comPort.RtsEnable = true;
                comPort.DtrEnable = true;
                try
                {
                    comPort.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion
        public bool OpenPortWithDelay()
        {
            lock (syncRoot)
            {

                if (comPort.IsOpen == true)
                {
                    comPort.DiscardInBuffer();
                    comPort.Close();
                    DelayExecution(2000);
                }
                comPort.BaudRate = int.Parse(this.BaudRate);    //BaudRate
                comPort.DataBits = int.Parse(this.DataBits);          //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.StopBits);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), this.Parity);    //Parity
                comPort.PortName = this.PortName;  //PortName
                //now open the port
                comPort.RtsEnable = true;
                comPort.DtrEnable = true;
                try
                {
                    comPort.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #region ClosePort
        public bool ClosePort()
        {
            try
            {
                lock (syncRoot)
                {

                    if (comPort.IsOpen)
                    {
                        comPort.DiscardInBuffer();
                        comPort.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public bool ClosePortWithDelay()
        {
            try
            {
                lock (syncRoot)
                {

                    if (comPort.IsOpen)
                    {
                        comPort.DiscardInBuffer();
                        comPort.Close();
                        DelayExecution(2000);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region SetParity
        internal string[] SetParity()
        {
            return (Enum.GetNames(typeof(Parity)));
        }
        #endregion

        #region StopBit
        internal string[] StopBit()
        {
            return (Enum.GetNames(typeof(StopBits)));
        }
        #endregion

        #region GetAvailablePorts
        public string[] GetAvailablePorts()
        {
            return (SerialPort.GetPortNames());
        }
        #endregion

        #region comport_DataReceivedCMRI
        public void comPort_DataReceivedFastDownLoadForCMRI(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                flgDataReceived = true;
                timeout = this.InterchatracterDelay;
                TimeStamp = DateTime.Now;
                nCount = comPort.BytesToRead;
                //onfdlStatusChanged("Bytes Read : "+bufferIndex.ToString());
                comPort.Read(fastDownloadBufferCMRI, bufferIndex, nCount);
                bufferIndex = bufferIndex + nCount;
                if (bCommType == 0x00)
                {
                    if (fastDownloadBufferCMRI[fastDownloadBufferCMRI[2] + 1] == 0x7E)
                        flgReadFlag = true;
                }
                else
                {
                    if ((fastDownloadBufferCMRI[0] == 0x0D) && (fastDownLoadBuffer[1] == 0x0A) && (fastDownLoadBuffer[bufferIndex - 2] == 0x0D) && (fastDownLoadBuffer[bufferIndex - 1] == 0x0A))
                    {
                        flgReadFlag = true;
                        bCommType = 0x00;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region comPort_DataReceived
        public void comPort_DataReceivedFastDownLoad(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                flgDataReceived = true;
                timeout = this.InterchatracterDelay;
                TimeStamp = DateTime.Now;
                nCount = comPort.BytesToRead;
                //onfdlStatusChanged("Bytes Read : "+bufferIndex.ToString());
                comPort.Read(fastDownLoadBuffer, bufferIndex, nCount);
                bufferIndex = bufferIndex + nCount;
                if (bCommType == 0x00)
                {
                    if (fastDownLoadBuffer[fastDownLoadBuffer[2] + 1] == 0x7E)
                        flgReadFlag = true;
                }
                else
                {
                    if ((fastDownLoadBuffer[0] == 0x0D) && (fastDownLoadBuffer[1] == 0x0A) && (fastDownLoadBuffer[bufferIndex - 2] == 0x0D) && (fastDownLoadBuffer[bufferIndex - 1] == 0x0A))
                    {
                        flgReadFlag = true;
                        bCommType = 0x00;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region SendFastDownLoadCmdToPort
        //'******************************************************************************
        //'
        //'  NAME     : fSendDataToPort
        //'
        //'  INPUT    : Buffer and Length
        //'
        //'  OUTPUT   : True or False
        //'
        //'  PURPOSE  : To Send data on Opened Serial Port and Recieve data in RecieveBuffer 
        //'
        //'*******************************************************************************
        public bool SendFastDownLoadCmdToPort(byte[] Buffer, byte nLength, out bool communicationTimeOut)
        {
            byte nRetries = 1;
            //clear the buffer
            flgReadFlag = false;
            bufferIndex = 0;
            strOutBuff = "";
            communicationTimeOut = false;
            for (int i = 0; i < fastDownLoadBuffer.Length; i++)
                fastDownLoadBuffer[i] = 0x00;

            //Send the command to COM port
            comPort.Write(Buffer, 0, nLength);

            //set the current timestamp for calculating timeout delay

            TimeStamp = DateTime.Now;
            timeout = CommandTimeout;
            flgDataReceived = false;

            //check untill response is recived within the timeout limit
            do
            {
                if (Timeout() == 0)
                {
                    if (nRetries > 4)
                    {
                        communicationTimeOut = true;
                        return false;
                    }
                    else
                    {
                        TimeStamp = DateTime.Now;
                        timeout = CommandTimeout;
                        flgDataReceived = false;
                        nRetries++;
                        //clear the buffer
                        flgReadFlag = false;
                        bufferIndex = 0;
                        strOutBuff = "";
                        for (int i = 0; i < fastDownLoadBuffer.Length; i++)
                            fastDownLoadBuffer[i] = 0x00;
                        comPort.Write(Buffer, 0, nLength);
                    }
                    //break;
                }
                else if (Timeout() == 1)
                {
                    return false;
                }
                else
                {
                    if (flgReadFlag == true)
                    {
                        break;
                    }
                }
            } while (true);

            return true;

        }
        #endregion


        #region comPort_DataReceived
        public void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                flgDataReceived = true;
                timeout = this.InterchatracterDelay;
                TimeStamp = DateTime.Now;
                nCount = comPort.BytesToRead;
                comPort.Read(ReceiveBuffer, bufferIndex, nCount);
                bufferIndex = bufferIndex + nCount;
                if (bCommType == 0x00)
                {
                    if (ReceiveBuffer[ReceiveBuffer[2] + 1] == 0x7E)
                        flgReadFlag = true;
                }
                else
                {
                    //BhardwajG : If the command is of AT type then check that at least 4 bytes are received.
                    if (bufferIndex > 4)
                    {
                        if ((ReceiveBuffer[0] == 0x0D) && (ReceiveBuffer[1] == 0x0A) && (ReceiveBuffer[bufferIndex - 2] == 0x0D) && (ReceiveBuffer[bufferIndex - 1] == 0x0A))
                        {
                            flgReadFlag = true;
                            bCommType = 0x00;
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                //   do nothing
            }
            catch (InvalidOperationException ex)
            {
                //   do nothing

            }
            catch (ArgumentOutOfRangeException ex)
            {
                //   do nothing
            }
            catch (ArgumentException ex)
            {
                //   do nothing
            }
            catch (TimeoutException ex)
            {
                //   do nothing
            }
            catch (Exception ex)
            {
                //   do nothing
            }

            //bufferIndex = bufferIndex + nCount;   
        }

        #endregion
        public bool fSendDataToPortCMRI(byte[] Buffer, byte nLength, string cmd, out bool communicationTimeOut)
        {
            communicationTimeOut = false;
            byte nRetries = 1;
            //clear the buffer
            flgReadFlag = false;
            bufferIndex = 0;
            strOutBuff = "";
            byte[] lastTag = new byte[1] { 126 };
            string receiveFastDownload = string.Empty;
            for (int i = 0; i < fastDownloadBufferCMRI.Length; i++)
            {
                fastDownloadBufferCMRI[i] = 0x00;
            }
            try
            {
                // if port is not open


                //Send the command to COM port
                lock (syncRoot)
                {
                    if (!comPort.IsOpen)
                    {

                        // try opening it
                        if (!OpenPort())
                        {

                            //if not opened than return false
                            return false;
                        }
                    }
                    if (comPort.IsOpen)
                    {

                        comPort.Write(Buffer, 0, nLength);
                        comPort.Write(cmd);
                        comPort.Write(lastTag, 0, 1);
                    }

                }
                //set the current timestamp for calculating timeout delay

                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;

                flgDataReceived = false;

                //check untill response is recived within the timeout limit
                do
                {
                    if (Timeout() == 0)
                    {
                        if (nRetries > 4)
                        {
                            communicationTimeOut = true;
                            return false;
                        }
                        else
                        {
                            TimeStamp = DateTime.Now;
                            timeout = CommandTimeout;
                            flgDataReceived = false;
                            nRetries++;
                            //clear the buffer
                            flgReadFlag = false;
                            bufferIndex = 0;
                            strOutBuff = "";
                            for (int i = 0; i < fastDownloadBufferCMRI.Length; i++)
                            {
                                fastDownloadBufferCMRI[i] = 0x00;
                            }

                            comPort.Write(Buffer, 0, nLength);
                            comPort.Write(cmd);
                            comPort.Write(lastTag, 0, 1);
                        }
                        //break;
                    }
                    else if (Timeout() == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (flgReadFlag == true)
                        {
                            break;
                        }
                    }
                } while (true);

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        #region fSendDataToPort
        //'******************************************************************************
        //'
        //'  NAME     : fSendDataToPort
        //'
        //'  INPUT    : Buffer and Length
        //'
        //'  OUTPUT   : True or False
        //'
        //'  PURPOSE  : To Send data on Opened Serial Port and Recieve data in RecieveBuffer 
        //'
        //'*******************************************************************************
        public bool fSendDataToPort(byte[] Buffer, byte nLength)
        {

            byte nRetries = 1;
            //clear the buffer
            flgReadFlag = false;
            bufferIndex = 0;
            strOutBuff = "";

            for (int i = 0; i < 255; i++)
            {
                ReceiveBuffer[i] = 0x00;
            }

            try
            {
                // if port is not open


                //Send the command to COM port
                lock (syncRoot)
                {
                    if (!comPort.IsOpen)
                    {

                        // try opening it
                        if (!OpenPort())
                        {

                            //if not opened than return false
                            return false;
                        }
                    }
                    if (comPort.IsOpen)
                    {
                        comPort.DiscardOutBuffer();
                        comPort.DiscardInBuffer();
                        comPort.Write(Buffer, 0, nLength);
                    }

                }
                //set the current timestamp for calculating timeout delay

                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;

                flgDataReceived = false;

                //check untill response is recived within the timeout limit
                do
                {
                    if (Timeout() == 0)
                    {
                        if (nRetries > retries)
                        {
                            return false;
                        }
                        else
                        {
                            OnTimeOut();
                            TimeStamp = DateTime.Now;
                            timeout = CommandTimeout;
                            flgDataReceived = false;
                            nRetries++;
                            //clear the buffer
                            flgReadFlag = false;
                            bufferIndex = 0;
                            strOutBuff = "";
                            for (int i = 0; i < 255; i++)
                            {
                                ReceiveBuffer[i] = 0x00;
                            }
                            comPort.DiscardOutBuffer();
                            comPort.DiscardInBuffer();
                            comPort.Write(Buffer, 0, nLength);
                        }
                        //break;
                    }
                    else if (Timeout() == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (flgReadFlag == true)
                        {
                            break;
                        }
                    }
                } while (true);

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

             #endregion
        public bool fSendDataToPortOnTimeOut(byte[] Buffer, byte nLength)
        {

            byte nRetries = 1;
            //clear the buffer
            flgReadFlag = false;
            bufferIndex = 0;
            strOutBuff = "";

            for (int i = 0; i < 255; i++)
            {
                ReceiveBuffer[i] = 0x00;
            }

            try
            {
                // if port is not open


                //Send the command to COM port
                lock (syncRoot)
                {
                    if (!comPort.IsOpen)
                    {

                        // try opening it
                        if (!OpenPort())
                        {

                            //if not opened than return false
                            return false;
                        }
                    }
                    if (comPort.IsOpen)
                    {
                        comPort.DiscardOutBuffer();
                        comPort.DiscardInBuffer();
                        comPort.Write(Buffer, 0, nLength);
                    }

                }
                //set the current timestamp for calculating timeout delay

                TimeStamp = DateTime.Now;
                timeout = CommandTimeout;

                flgDataReceived = false;

                //check untill response is recived within the timeout limit
                do
                {
                    if (Timeout() == 0)
                    {
                        if (nRetries > 4)
                        {
                            return false;
                        }
                        else
                        {
                            TimeStamp = DateTime.Now;
                            timeout = CommandTimeout;
                            flgDataReceived = false;
                            nRetries++;
                            //clear the buffer
                            flgReadFlag = false;
                            bufferIndex = 0;
                            strOutBuff = "";
                            for (int i = 0; i < 255; i++)
                            {
                                ReceiveBuffer[i] = 0x00;
                            }
                            comPort.DiscardOutBuffer();
                            comPort.DiscardInBuffer();
                            comPort.Write(Buffer, 0, nLength);
                        }
                        //break;
                    }
                    else if (Timeout() == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (flgReadFlag == true)
                        {
                            break;
                        }
                    }
                } while (true);

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        # region SetserialPort
        public void SetSerialPortSettings(String Port, String baud, String parity, String dataBits, String nStopBits, int CmdTimeout, int intercharDelay)
        {
            //set COM port settings
            PortName = Port;
            BaudRate = baud;
            Parity = parity;
            DataBits = dataBits;
            StopBits = nStopBits;
            CommandTimeout = CmdTimeout;
            InterchatracterDelay = intercharDelay;
        }
        # endregion
        public void SetSerialPortSettings(String baud, String parity, String dataBits, String nStopBits, int CmdTimeout, int intercharDelay)
        {
            //set COM port settings
            BaudRate = baud;
            Parity = parity;
            DataBits = dataBits;
            StopBits = nStopBits;
            CommandTimeout = CmdTimeout;
            InterchatracterDelay = intercharDelay;
        }      

        # region Delay

        public void DelayExecution(int millisecondsTime)
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(millisecondsTime);
            while (DateTime.UtcNow < end)
            {
            }
        }

        # endregion

        # region Timeout

        public int Timeout()
        {

            long elapsedTime;

            elapsedTime = DateTime.Now.Ticks - TimeStamp.Ticks;

            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);

            elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);

            if (flgDataReceived == true)
            {
                if (elapsedMilliseconds > timeout)
                {
                   
                    flgReadFlag = true;//Interframe timeout
                    return 1;
                }
                else
                {
                    return 2;//Nothing
                }
            }
            else
            {
                if (elapsedMilliseconds > timeout)
                {
                    return 0;//Response Time out
                }
                else
                {
                    return 2;//Nothing
                }
            }
        }

        # endregion


    }
}
