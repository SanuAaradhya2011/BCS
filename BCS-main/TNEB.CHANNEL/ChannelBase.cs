using System.IO.Ports;
using CAB.IECFramework;
using System;
using System.Threading;
using System.Text;
using CAB.IECFramework.Utility;
using LTCTBLL;
using CAB.Entity;
using CABCommunication.PhysicalLayer;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


namespace CAB.IECChannel
{
    public abstract class IECChannelBase : IChannel
    {
        public SerialPort ComPort;
        public bool IsDataReceived;
        public int BaudRate;
        public Parity Parity;
        public StopBits StopBits;
        public int DataBits;
        public string PortName;
        public string Command;
        public int CommandTimeout;
        public int InterChatracterDelay;
        public int InterCommandDelay;
        public long CMRIWaitTimeout;
        public int CMRIPktTimeout;
        public int CommandID;
        public long TotalReadBytes;
        public string OutBuffer;
        public bool ReadFlag;
        public DateTime CurrentTime;
        public int PacketCount;
        public string ResponseSignOn;
        protected string ExistingBuffer;
        protected bool FlagBCC;
        private ModemConfigProperties modemConfigProperties = null;
        private ChannelInformation channelInformation = new ChannelInformation();
        private const string DLMS = "DLMS";
        private string dataNumber = string.Empty;
        private string baudRate;
        private string parity;
        private string stopBits;
        private string dataBits;
        private long responseTimeout;
        private long intercharacterDelay;
        public byte noOfRetry;
        static object syncRoot = new object();
        static ModemConfig modemConfig = null;
        private bool dataRecievedFlag;
        private bool readFlag = false;
        private int bufferIndex = 0;
        private byte[] receiveBuffer;
        private const int MaxRecieveLength = 250000;
        private long timeout;
        private DateTime timeStamp;
        public string SimNumber { get; set; }
        public string ChannelType { get; set; }


        //private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Serial).ToString());


        public IECChannelBase()
        {
            CommandID = 0;
            TotalReadBytes = 0;
            IsDataReceived = false;
            OutBuffer = string.Empty;
            ResponseSignOn = string.Empty;
            ExistingBuffer = string.Empty;
            ComPort = new SerialPort();
            //System.Diagnostics.Debug.Print( ComPort.BytesToRead.ToString());
            ComPort.ReadBufferSize = 10000;
            ComPort.DataReceived += new SerialDataReceivedEventHandler(OnPortDataReceived);
            this.PortName = ConfigInfo.GetPortName();
            this.BaudRate = Convert.ToInt32(ConfigInfo.GetBaudRate());
            this.DataBits = CAB.IECFramework.Command.GetInstance().DataBit;
            this.StopBits = CAB.IECFramework.Command.GetInstance().StopBit;
            this.Parity = CAB.IECFramework.Command.GetInstance().Parity;

            modemConfig = (ModemConfig)(DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "Modem.xml", typeof(ModemConfig)));
        }
        /// <summary>
        /// Deserialize an an object xml to object
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlClassType"></param>
        /// <returns></returns>
        public object DeserializeToObject(string xmlFilePath, Type xmlClassType)
        {
            string xmlString = string.Empty;
            StringReader strReader = null;
            XmlSerializer xmlSerializer = null;
            try
            {
                xmlSerializer = new XmlSerializer(xmlClassType);
                using (StreamReader reader = new StreamReader(xmlFilePath))
                {
                    xmlString = reader.ReadToEnd();
                }
                strReader = new StringReader(xmlString);
                return Convert.ChangeType(xmlSerializer.Deserialize(strReader), xmlClassType);
            }
            catch
            {
                throw;

            }
            finally
            {
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
        }
        /// <summary>
        /// Sets the Channel with outside serial port
        /// </summary>
        /// <param name="serial"></param>
        public void SetPort(Serial serial)
        {
            CommandID = 0;
            TotalReadBytes = 0;
            IsDataReceived = false;
            OutBuffer = string.Empty;
            ResponseSignOn = string.Empty;
            ExistingBuffer = string.Empty;
            this.ComPort = serial;
            //System.Diagnostics.Debug.Print( ComPort.BytesToRead.ToString());

            ComPort.DataReceived += new SerialDataReceivedEventHandler(OnPortDataReceived);
            this.PortName = serial.PortName;
            this.BaudRate = Convert.ToInt32(ConfigInfo.GetBaudRate());
            this.DataBits = CAB.IECFramework.Command.GetInstance().DataBit;
            this.StopBits = CAB.IECFramework.Command.GetInstance().StopBit;
            this.Parity = CAB.IECFramework.Command.GetInstance().Parity;
        }
        public virtual bool OpenPort()
        {
            if (ComPort.IsOpen)
            {
                return true;
                //ComPort.Close();
            }
                
            ComPort.BaudRate = BaudRate;
            ComPort.DataBits = DataBits;
            ComPort.StopBits = StopBits;
            ComPort.Parity = Parity;
            ComPort.PortName = PortName;
            ComPort.RtsEnable = true;
            ComPort.DtrEnable = true;
            bool flag = true;
            try
            {
                ComPort.Open();
                if ((this.ChannelType != CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
                {
                   // flag = OpenSession();
                }

            }
            catch (Exception ex)
            {
                //new CABException(ex);
            }
            return flag;
        }


        /// <summary>
        /// Opens the GSM session with remote meter assumes.
        /// </summary>
        /// <returns></returns>
        public bool OpenSession()
        {
            bool success = false;
            this.modemConfigProperties = GetModemConfigProperties(this.ChannelType + DLMS); //GetModemConfigProperties(ConfigSettings.GetValue("ChannelType") + DLMS);
            dataNumber = this.SimNumber;
            this.responseTimeout = modemConfigProperties.Commandsettings[0].CommandTimeout;
            this.intercharacterDelay = modemConfigProperties.Commandsettings[0].InterCharacterTimeout;

            success = ConnectLocalModem(modemConfigProperties);
            if (success)
            {
                success = ConnectRemoteModem(modemConfigProperties);
            }
            return success;
        }


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
                //logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            finally
            {

            }
            return success;

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
        /// Connect to local modem.
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private bool ConnectLocalModem(ModemConfigProperties modemConfigProperties)
        {
            bool response = false;
            string[] initCommands = GetInitATCommands(modemConfigProperties);
            string result = string.Empty;
            for (int counter = 0; counter < initCommands.Length; counter++)
            {
                // objSerialComm.bCommType = 1;
                response = SendCommandToModem(initCommands[counter], false);
                if (response == false)
                    break;
            }
            return response;
        }

        /// <summary
        /// This method is used for the sending command to modem.
        /// </summary>
        /// <param name="command">Please paas the command to execute on the modem.</param>
        /// <returns></returns>
        private bool SendCommandToModem(string command, bool remoteModem)
        {
            bool result = false;
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
                CommandResult = SendNewCommand(modemCommand, modemIndex, remoteModem);

                if (remoteModem)
                {
                    if (CommandResult.Contains("CONNECT"))
                    {
                        DelayExecution(5000);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {
                //logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
                throw;

            }
            return result;
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
        /// Connects to remote modem
        /// </summary>
        /// <param name="modemConfigProperties"></param>
        /// <returns></returns>
        private bool ConnectRemoteModem(ModemConfigProperties modemConfigProperties)
        {
            //string result = null;
            //Result response = new Result();
            bool response = false;
            try
            {
                this.CommandTimeout = 70000;
                this.InterChatracterDelay = 70000;

                string dial = GetDialString(modemConfigProperties);
                dial = string.Concat(dial, dataNumber);
                if (!string.IsNullOrEmpty(dial))
                {
                    response = SendCommandToModem(dial, true);
                }
                this.CommandTimeout = 10000;
                this.InterChatracterDelay = 10000;
            }
            catch (Exception ex)
            {
                //logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
            }
            return response;
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
        public virtual bool ClosePort()
        {
            bool flag = false;
            try
            {
                if (ComPort.IsOpen)
                {
                    if ((ConfigSettings.GetValue("ChannelType") != CABCommunication.PhysicalLayer.ChannelType.Direct.ToString()))
                    {
                        DelayExecution(5000);
                        CloseRemoteSession();
                    }
                    ComPort.Close();

                    flag = true;
                }
            }
            catch (Exception ex)
            {
                // new CABException(ex);
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private string SendNewCommand(byte[] data, int dataLength, bool remoteModem)
        {
            string response = string.Empty;
            bool Flag = false;
            int counter = noOfRetry;
            readFlag = false;
            bufferIndex = 0;
            dataRecievedFlag = false;
            receiveBuffer = new byte[MaxRecieveLength];
            byte lenBytes = 6;
            DelayExecution(1000);
            do
            {
                this.CommandID = 11;
                if (remoteModem)
                {
                    this.CommandID = 10;
                    lenBytes = 15;
                }
                this.OutBuffer = string.Empty;
                this.ReadFlag = false;

                if (!ComPort.IsOpen)
                {
                    Flag = false;
                    break;
                }
                else
                {
                    CurrentTime = DateTime.Now;
                    ComPort.DiscardOutBuffer();
                    ComPort.DiscardInBuffer();
                    ComPort.Write(data, 0, dataLength);
                    do
                    {
                        if (this.ReadFlag == true)
                        {
                            if (this.OutBuffer.Length >= lenBytes)
                            {
                                response = this.OutBuffer;
                                Flag = true;
                                break;
                            }
                        }
                        if (this.OutBuffer.ToUpperInvariant().IndexOf("NO CARRIER") >= 0 || this.OutBuffer.ToUpperInvariant().IndexOf("NO ANSWER") >= 0 || this.OutBuffer.ToUpperInvariant().IndexOf("ERROR") >= 0)
                        {
                            response = this.OutBuffer;
                            break;
                        }
                    } while (this.Timeout() != true);
                }
                this.CurrentTime = DateTime.Now;
                counter--;
                if (Flag)
                    break;
            } while (counter != 0);

            return response;
        }
        public virtual bool SendCommand()
        {
            int counter = 0;
            char CH;
            bool flag = false;
            FlagBCC = false;
            try
            {
                CurrentTime = DateTime.Now;
                while (counter < Command.Length)
                {
                    CH = Convert.ToChar(Convert.ToUInt16((Command.Substring(counter, 2)), 16));
                    if (!ComPort.IsOpen)
                        ComPort.Open();
                    ComPort.Write(Convert.ToString(CH));
                    Thread.Sleep(05); // Sleep is modified implemented for Eco-Star 1P IEC meter readout support.
                    counter += 2;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                //new CABException(ex);
            }
            return flag;
        }

        /// <summary>
        /// Modified by Vivek Agrawal
        /// </summary>
        public virtual void DelayExecution()
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(this.InterCommandDelay);
            while (DateTime.UtcNow < end)
            {
            }
            // if(UtilityDetails.UtilityName == UtilityEntity.TNEB)
            System.Threading.Thread.Sleep(1);
        }

        public virtual bool Timeout()
        {
            long elapsedTime;
            int timeout;
            elapsedTime = DateTime.Now.Ticks - CurrentTime.Ticks;
            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);
            long elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);
            if (!IsDataReceived)
                timeout = this.CommandTimeout;
            else
                timeout = this.InterChatracterDelay;
            if (elapsedMilliseconds > timeout)
                return true;
            else
                return false;
        }

        public virtual void ReadBytes()
        {
            CurrentTime = DateTime.Now;
            TotalReadBytes = TotalReadBytes + ComPort.BytesToRead;
            ExistingBuffer = ComPort.ReadExisting();
            OutBuffer = string.Concat(OutBuffer, ExistingBuffer);
            IsDataReceived = true;
        }


        public bool SignOn()
        {
            bool Flag = false;
            int counter = 3;

            try
            {
                this.Command = CAB.IECFramework.Command.GetInstance().SignOnCommand;
                do
                {
                    this.CommandID = 0;
                    this.OutBuffer = string.Empty;
                    this.ReadFlag = false;
                    ResponseSignOn = string.Empty;
                    if (!this.SendCommand())
                    {
                        Flag = false;
                        break;
                    }
                    else
                    {

                        do
                        {
                            if (this.ReadFlag == true)
                            {
                                if (this.OutBuffer.Length >= 14)
                                {
                                    ResponseSignOn = this.OutBuffer;
                                    Flag = true;
                                    break;
                                }
                            }
                        } while (this.Timeout() != true);
                    }
                    this.CurrentTime = DateTime.Now;
                    counter--;
                    if (Flag)
                        break;
                } while (counter != 0);
            }
            catch (Exception Ex)
            {
                Flag = false;
                // new CABException(Ex);
            }
            return Flag;
        }

        public bool SignOnForSPhaseIEC()
        {
            bool Flag = false;
            int counter = 3;

            try
            {
                this.Command = CAB.IECFramework.Command.GetInstance().SignOnCommand;
                do
                {
                    this.CommandID = 0;
                    this.OutBuffer = string.Empty;
                    this.ReadFlag = false;
                    ResponseSignOn = string.Empty;
                    if (!this.SendCommand())
                    {
                        Flag = false;
                        break;
                    }
                    else
                    {

                        do
                        {
                            if (this.ReadFlag == true)
                            {
                                if (this.OutBuffer.Length >= 30)
                                {
                                    //ResponseSignOn = this.OutBuffer;
                                    ResponseSignOn = this.OutBuffer.Replace("\x03", "");
                                    Flag = true;
                                    break;
                                }
                            }
                        } while (this.Timeout() != true);
                    }
                    this.CurrentTime = DateTime.Now;
                    counter--;
                    if (Flag)
                        break;
                } while (counter != 0);
            }
            catch (Exception Ex)
            {
                Flag = false;
                // new CABException(Ex);
            }
            return Flag;
        }

        public virtual void OnPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                IsDataReceived = true;
                ExistingBuffer = string.Empty;
                int bufferIndex;

                if (CommandID == 0)
                {

                    ReadBytes();
                    if (TotalReadBytes >= 27)
                    {
                        ReadFlag = true;
                        FlagBCC = false;
                        return;
                    }
                }
                if (CommandID == 2)
                {
                    CurrentTime = DateTime.Now;
                    TotalReadBytes = TotalReadBytes + ComPort.BytesToRead;
                    ExistingBuffer = ComPort.ReadExisting();
                    OutBuffer = string.Concat(OutBuffer, ExistingBuffer);
                    IsDataReceived = true;
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    Byte[] totalByte = encoding.GetBytes(ExistingBuffer);
                    int index = 0;
                    if (!FlagBCC)
                    {
                        foreach (byte bytes in totalByte)
                        {
                            //  4 Bytes support is added
                            if (bytes == 3 || bytes == 4)
                            {
                                if (totalByte.Length > index + 1)
                                {
                                    ReadFlag = true;
                                    FlagBCC = false;
                                    return;
                                }
                                else
                                {
                                    FlagBCC = true;
                                    return;
                                }
                            }
                            index++;
                        }
                    }
                    else
                    {
                        ReadFlag = true;
                        FlagBCC = false;
                        return;
                    }
                }
                if (CommandID == 3)
                {
                    ReadBytes();
                    if (TotalReadBytes >= 1 && OutBuffer.ToString().Equals(Convert.ToChar(6).ToString()))
                        this.ReadFlag = true;
                    TotalReadBytes = 0;
                }
                if (CommandID == 4)
                {
                    ReadBytes();
                    if (TotalReadBytes >= 80)
                        ReadFlag = true;
                }
                if (CommandID == 9)
                {
                    ReadBytes();
                    bufferIndex = ExistingBuffer.IndexOf("t");
                    if (bufferIndex > 0)
                        bufferIndex = ExistingBuffer.IndexOf("kt");
                    if (bufferIndex >= 0)
                    {
                        PacketCount = 1;
                        Command = "06";
                        SendCommand();
                    }
                    bufferIndex = ExistingBuffer.IndexOf("PktEnd");
                    if (bufferIndex >= 0) ReadFlag = true;
                }
                if (CommandID == 11)
                {
                    ReadBytes();
                    if (TotalReadBytes >= 6)
                        ReadFlag = true;
                }
                if (CommandID == 10)
                {
                    ReadBytes();
                    if (TotalReadBytes >= 15)
                        ReadFlag = true;
                }
            }
            catch (Exception)
            {
                //new CABException( ex);
            }

        }
        /// <summary>
        /// Detaches the port received event from serial port object
        /// </summary>
        public void DetachEvent()
        {
            this.ComPort.DataReceived -= new SerialDataReceivedEventHandler(OnPortDataReceived);
        }
    }
}
