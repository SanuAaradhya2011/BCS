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
    public class DTMProgramming : ReadBase 
    {
        public string HighLoadThreshold {get;set;}
        public string LowLoadThreshold { get; set; }
        public string TransformerRating { get; set; }
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
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                    communications.DelayExecution();
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
                            while (HighLoadThreshold.Length < 2) { HighLoadThreshold = "0" + HighLoadThreshold; }
                            while (LowLoadThreshold.Length < 2) { LowLoadThreshold = "0" + LowLoadThreshold; }
                            while (TransformerRating.Length < 4) { TransformerRating = "0" + TransformerRating; }
                            rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(string.Concat(HighLoadThreshold, LowLoadThreshold, TransformerRating)));
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
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                                    this.StatusMessage = "Daily log configured successfully.";//2 march 2012:Message modified
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

        public void ResetDTMDailyLog()
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            string rtcCommand = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port";
                    return ;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Signon failure.";
                    return ;
                }
                communications.DelayExecution();
                communications.Command = command.DTMParaManfCommand;
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();
                Thread.Sleep(200);
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Signon failure.";
                        return ;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;

                communications.OutBuffer = string.Empty;
                rtcCommand = command.DTMLogResetCommand;
                rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));

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
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        return ;
                    }
                } while (communications.OutBuffer.Length < 1);

                if (communications.OutBuffer.Length >= 1)
                {
                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    if (charACK == 6)
                    {
                        //this.StatusMessage = "Daily log configured successfully. Data reset.";
                    }
                    else
                    {
                        this.StatusMessage = "Access Denied.";
                        return;
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
        }

        /// <summary>
        /// This function clears the daily profile data present in the meter.
        /// </summary>
        //public void ResetDTMDailyLog()
        //{
        //    char charACK;
        //    IsSignOnFailure = false;
        //    string data = string.Empty;

        //    try
        //    {
        //        if (ConfigInfo.GetLocalMode().Equals("Optical"))
        //            communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
        //        if (!communications.OpenPort())
        //        {
        //            this.StatusMessage = "Error in opening port.";
        //            return;
        //        }
        //        communications.CurrentTime = DateTime.Now;
        //        if (!communications.SignOn())
        //        {
        //            this.StatusMessage = "Signon failure.";
        //            return;
        //        }
        //        communications.DelayExecution();
        //        communications.Command = command.DTMParaManfCommand;
        //        communications.OutBuffer = string.Empty;
        //        communications.SendCommand();
        //        communications.DelayExecution();

        //        if (communications.ResponseSignOn != string.Empty)
        //        {
        //            if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
        //            {
        //                this.StatusMessage = "Signon failure.";
        //                return;
        //            }
        //        }
        //        communications.OutBuffer = string.Empty;
        //        string dailyLogResetCommand = command.DTMLogResetCommand;
        //        if (dailyLogResetCommand == String.Empty)
        //        {
        //            this.StatusMessage = "Failure in daily log data reset";
        //            return;
        //        }
        //        dailyLogResetCommand = dailyLogResetCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
        //        string calculatedBcc = ReadoutCommon.ReturnBcc(dailyLogResetCommand.Substring(2, dailyLogResetCommand.Length - 5));
        //        dailyLogResetCommand = dailyLogResetCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
        //        communications.Command = dailyLogResetCommand;
        //        communications.CommandID = 2;
        //        communications.ReadFlag = false;
        //        communications.IsDataReceived = false;
        //        communications.OutBuffer = string.Empty;
        //        communications.CurrentTime = DateTime.Now;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        do
        //        {
        //            if (communications.Timeout())
        //            {
        //                this.StatusMessage = "Timeout!";
        //                return;
        //            }
        //        } while (communications.OutBuffer.Length < 1);
        //        if (communications.OutBuffer.Length >= 1)
        //        {
        //            charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
        //            if (charACK == 6)
        //                this.StatusMessage = "Daily log configured successfully. Data reset.";
        //            else if (charACK == 21)
        //                this.StatusMessage = "Access Denied.";
        //        }
        //        else
        //        {
        //            this.StatusMessage = "Access Denied.";
        //            return;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.ClosePort();
        //    }
        //    return;
        //}
    }
}

