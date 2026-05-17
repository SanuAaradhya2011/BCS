using System.IO.Ports;
using CAB.Framework;
using System;
using System.Threading;
using System.Text;
using CAB.Framework.Utility;
using System.Data;

namespace CAB.Channel
{
    public class ReadBase
    {
        public ApplicationType AppType;
        public delegate void ChannelStatusChanged(string msg);
        public event ChannelStatusChanged OnChannelStatusChanged;
        protected ChannelBase communications;
        protected Command command;
        protected bool isCorruptedData;
        protected bool isAborted;
        public bool IsSignOnFailure { get; set; }
        public ReadBase()
        {
            //AppType = ConfigInfo.GetApplicationType();
            command = Command.GetInstance();
        }
        private string dateTime;
        public string ReadingDateTime
        {
            get
            {
                return dateTime;
            }
            set
            {
                dateTime = value;
            }
        }
        private string message;
        public string StatusMessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                if (OnChannelStatusChanged != null)
                {
                    OnChannelStatusChanged(message);
                }
            }
        }
        public ChannelBase Channel
        {
            get { return communications; }
            set { communications = value; }
        }
        public bool IsAborted
        {
            get { return isAborted; }
            set { isAborted = value; }
        }
        public bool IsCorruptedData
        {
            get { return isCorruptedData; }
            set { isCorruptedData = value; }
        }
        protected DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        protected DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        public string MeterPassword { get; set; }

        public virtual string GetData()
        {
            return string.Empty;
        }

        public virtual string GetInstantData()
        {
            return string.Empty;
        }

        public virtual bool GeDate()
        {
            return false;
        }

        public virtual string ReverseEnergy()
        {
            return string.Empty;
        }

        public virtual string GetDTMParameterData()
        {
            return string.Empty;
        }

        public virtual string GetFirmWareVersion()
        {
            return string.Empty;
        }

       
    }
}