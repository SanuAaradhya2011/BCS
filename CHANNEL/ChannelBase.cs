using System.IO.Ports;
using CAB.Framework;
using System;
using System.Threading;
using System.Text;
using CAB.Framework.Utility;

namespace CAB.Channel
{
    public abstract class ChannelBase : IChannel
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
        public ChannelBase()
        {
            CommandID = 0;
            TotalReadBytes=0;
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
            this.DataBits = CAB.Framework.Command.GetInstance().DataBit;
            this.StopBits = CAB.Framework.Command.GetInstance().StopBit;
            this.Parity = CAB.Framework.Command.GetInstance().Parity;
        } 
        public virtual bool OpenPort()
        {
            if (ComPort.IsOpen)
                ComPort.Close();
            ComPort.BaudRate = BaudRate;
            ComPort.DataBits = DataBits;
            ComPort.StopBits = StopBits;
            ComPort.Parity = Parity;
            ComPort.PortName = PortName;
            ComPort.RtsEnable = true;
            ComPort.DtrEnable = true;
            bool flag = false;
            try
            {
                ComPort.Open();
                flag = true;
            }
            catch (Exception ex)
            { 
                  new CABException(ex);
            }
            return flag;
        } 
        public virtual bool ClosePort()
        {
            bool flag = false;
            try
            {
                if (ComPort.IsOpen) 
                    ComPort.Close();
                flag =  true;
            }
            catch (Exception ex)
            { 
                  new CABException(ex);
            }
            return flag;
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
                    counter += 2;
                }
                flag = true;
            }
            catch (Exception ex)
            { 
                  new CABException(ex);
            }
            return flag;
        }
        //public virtual void DelayExecution()
        //{
        //    DateTime end = DateTime.UtcNow.AddMilliseconds(this.InterCommandDelay);
        //    while (DateTime.UtcNow < end)
        //    {
        //    }
        //    //System.Threading.Thread.Sleep(3000);
        //}

        /// <summary>
        /// Modified by Vivek Agrawal
        /// </summary>
        public virtual void DelayExecution()
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(this.InterCommandDelay);
            while (DateTime.UtcNow < end)
            {
            }
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
            this.OutBuffer = string.Empty;
            this.ReadFlag = false;
            try
            {
                this.Command = CAB.Framework.Command.GetInstance().SignOnCommand;
                do
                {
                    this.CommandID = 0;
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
                                if (this.OutBuffer.Length >= 27)
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
                  new CABException(Ex);
            }
            return Flag;
        }

        public virtual void OnPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }
    }
}
