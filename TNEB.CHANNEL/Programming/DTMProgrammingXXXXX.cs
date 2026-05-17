using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Channel.ReadOut;

namespace CAB.Channel.Programming
{
    public class DTMProgramming : ReadBase 
    {
        public int HighLoadThreshold {get;set;}
        public int LowLoadThreshold { get; set; }
        public int TransformerRating { get; set; }
        public string DailyParamsValue { get; set; }

        public DTMProgramming()
        {
            command = Command.GetInstance();
        }

        public bool WriteLPRParameters()
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = 300;
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port.";
                    return flag;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Signon failure.";
                    return flag;
                }
                communications.DelayExecution();
                communications.Command = command.LPRParaManfCommand;
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
                        return flag;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout Error.";
                        return flag;
                    }
                } while (communications.OutBuffer.Length < 5);

                if (communications.OutBuffer.Length >= 5)
                {
                    communications.OutBuffer = string.Empty;
                    string rtcCommand = command.LPRParaPasswordCommand;
                    rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                    string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                    rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                    communications.Command = rtcCommand;
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.SendCommand();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout Error.";
                            return flag;
                        }
                    } while (communications.OutBuffer.Length < 1);

                    if (communications.OutBuffer.Length >= 1)
                    {
                        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                        if (charACK == 6)
                        {
                            communications.OutBuffer = string.Empty;
                            rtcCommand = command.LPRParaWriteCommand;
                            rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(string.Concat(HighLoadThreshold.ToString(), LowLoadThreshold.ToString(), TransformerRating.ToString())));
                            calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                            rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                            communications.Command = rtcCommand;
                            communications.CommandID = 3;
                            communications.ReadFlag = false;
                            communications.IsDataReceived = false;
                            communications.OutBuffer = string.Empty;
                            communications.CurrentTime = DateTime.Now;
                            communications.SendCommand();
                            do
                            {
                                if (communications.Timeout())
                                {
                                    this.StatusMessage = "Timeout Error.";
                                    return flag;
                                }
                            } while (communications.OutBuffer.Length < 1);

                            if (communications.OutBuffer.Length >= 1)
                            {
                                charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                                if (charACK == 6)
                                {
                                    flag = true;
                                    this.StatusMessage = "LPR Parameters Updated";//MessageConstant.GetText("Msg_RTCUpdated");
                                }
                                else if (charACK == 21)
                                {
                                    this.StatusMessage = "Access Denied.";
                                    return flag;
                                }
                            }
                        }
                        else
                        {
                            this.StatusMessage = "Access Denied";
                            return flag;
                        }
                    }
                    else
                    {
                        this.StatusMessage = "Access Denied.";
                        return flag;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CABException(ex);
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return flag;
        }

        public bool WriteDTMDailyLog()
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = 300;
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port.";
                    return flag;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Signon failure.";
                    return false;
                }
                communications.DelayExecution();
                communications.Command = command.DTMLogManfCommand;
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();
  
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Signon failure.";
                        return flag;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        return flag;
                    }
                } while (communications.OutBuffer.Length < 5);

                if (communications.OutBuffer.Length >= 5)
                {
                    communications.OutBuffer = string.Empty;
                    string rtcCommand = command.DTMLogPasswordCommand;
                    rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                    string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                    rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                    communications.Command = rtcCommand;
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.SendCommand();
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout!";
                            return flag;
                        }
                    } while (communications.OutBuffer.Length < 1);

                    if (communications.OutBuffer.Length >= 1)
                    {
                        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                        if (charACK == 6)
                        {
                            communications.OutBuffer = string.Empty;
                            rtcCommand = command.DTMLogWriteCommand;
                            rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(DailyParamsValue));
                            calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                            rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                            communications.Command = rtcCommand;
                            communications.CommandID = 3;
                            communications.ReadFlag = false;
                            communications.IsDataReceived = false;
                            communications.OutBuffer = string.Empty;
                            communications.CurrentTime = DateTime.Now;
                            communications.SendCommand();
                            communications.DelayExecution();
                            do
                            {
                                if (communications.Timeout())
                                {
                                    this.StatusMessage = "Timeout!";
                                    return flag;
                                }
                            } while (communications.OutBuffer.Length < 1);

                            if (communications.OutBuffer.Length >= 1)
                            {
                                charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                                if (charACK == 6)
                                {
                                    flag = true;
                                    this.StatusMessage = "Daily log was updated successfully.";//MessageConstant.GetText("Msg_RTCUpdated");
                                }
                                else if (charACK == 21)
                                {
                                    this.StatusMessage = "Access Denied.";
                                }
                            }
                        }
                        else
                        {
                            this.StatusMessage = "Access Denied.";
                            return flag;
                        }
                    }
                    else
                    {
                        this.StatusMessage = "Access Denied.";
                        return flag;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CABException(ex);
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return flag;
        }
    }
}

