using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECChannel.ReadOut;

namespace CAB.IECChannel.Programming
{
    public class RTCInformation : ReadBase
    {
        private DateTime rtcDateTime;

        public RTCInformation()
        {
            command = Command.GetInstance();
        }

        public DateTime RTCDateTime
        {
            get { return rtcDateTime; }
            set { rtcDateTime = value; }
        }

        public string GetRTC(ref string statusMsg)
        {
            IsSignOnFailure = false;
            DateTime meterRTC = new DateTime();
            statusMsg = "";
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    statusMsg = "Error in opening port.";
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    statusMsg = "Sign-On failure";
                    return string.Empty;
                }
                communications.DelayExecution();
                communications.Command = command.ReadRTCManfCommand;
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();

                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        statusMsg = "Sign-On failure";
                        return string.Empty;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.DelayExecution();
                communications.OutBuffer = string.Empty;
                string rtcCommand = command.ReadRTCPasswordCommand;
                rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue("00000000"));//password mode set to hardware
                communications.Command = rtcCommand;
                communications.CommandID = 3;
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;
                communications.SendCommand();
                communications.DelayExecution();
                do
                {
                    if (communications.Timeout())
                    {
                        statusMsg = "Timeout!"; ;
                        //statusMsg = this.StatusMessage;
                        Application.DoEvents();
                        return string.Empty;
                    }
                } while (communications.OutBuffer.Length < 15);

                if (communications.OutBuffer.Length >= 15)
                {
                    communications.DelayExecution();
                    string tempData = communications.OutBuffer;
                    bool Bccres = ReadoutCommon.CalculateBcc(tempData.Substring(1), tempData.Length - 3, tempData.Substring(tempData.Length - 1, 1));
                    if (Bccres == true)
                    {
                        tempData = tempData.Substring(1, 2) + "/" + tempData.Substring(3, 2) + "/" + tempData.Substring(5, 2) + " " + tempData.Substring(7, 2) + ":" + tempData.Substring(9, 2) + ":" + tempData.Substring(11, 2);
                        if (!DateTime.TryParse(tempData, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out meterRTC))
                            statusMsg = "Invalid RTC.";
                        else
                            data = communications.ResponseSignOn + "|" + communications.OutBuffer;
                    }
                    else
                    {
                        statusMsg = "Access Denied.";
                        data = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                new CABException(ex);
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }

        public bool ValidateRTC(DateTime meterRTC)
        {
            TimeSpan timeDiff = meterRTC - RTCDateTime;
            TimeSpan ts = TimeSpan.FromTicks(timeDiff.Ticks);
            bool flag = false;
            if (ts.TotalMinutes >= 15)
                flag = false;
            else
                flag = true;
            return flag;
        }

        public bool SetRTC(string meterPassword)
        {
            try
            {
                char charACK;

                if (MeterPassword.Length == 0)
                {
                    return false;
                }
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port.";
                    return false;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Signon failure.";
                    return false;
                }
                communications.DelayExecution();
                communications.Command = command.RTCManfCommand;
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();
                communications.DelayExecution();
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Signon failure.";
                        return false;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.DelayExecution();
                communications.OutBuffer = string.Empty;
                string rtcCommand = command.RTCPasswordCommand;
                rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(meterPassword));
                rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(RTCDateTime));
                string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(0, rtcCommand.Length - 7));
                rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                communications.Command = rtcCommand;
                communications.CommandID = 3;
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;
                communications.SendCommand();
                communications.DelayExecution();
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        return false;
                    }
                } while (communications.OutBuffer.Length < 1);

                if (communications.OutBuffer.Length >= 1)
                {
                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    if (charACK == 6)
                    {
                        //this.StatusMessage = "RTC updated sucessfully.";
                    }
                    else
                    {
                        this.StatusMessage = "Access Denied.";
                        return false;
                    }
                }
                return true;
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
        }
    }
}