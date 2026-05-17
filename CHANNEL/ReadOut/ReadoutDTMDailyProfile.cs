using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;

namespace CAB.Channel.ReadOut
{
    public class ReadoutDTMDailyProfile : ReadBase
    {

        public override string GetDTMParameterData()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                {
                    if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                        communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                    else
                        communications.BaudRate = 300;
                }
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return data;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.Command = command.DTMParaManfCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            Application.DoEvents();
                            isCorruptedData = true;
                            return data;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    string commandPassword = command.DTMParaPasswordCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    communications.ReadFlag = false;
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = "";
                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            return data;
                        }
                        if (communications.ReadFlag)
                            break;
                        if (IsAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            Application.DoEvents();
                            return string.Empty;
                        }
                    } while (true);

                    if (communications.OutBuffer.Length >= 3)
                    {
                        Thread.Sleep(200);
                        bool Bccres = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), communications.OutBuffer.Length - 3, communications.OutBuffer.Substring(communications.OutBuffer.Length - 1, 1));
                        if (Bccres)
                        {
                            data = communications.OutBuffer;
                        }
                    }

                }
                else
                    IsSignOnFailure = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (communications != null)
                {
                    communications.DelayExecution();
                    communications.Command = command.BreakCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    communications.ClosePort();
                }
            }
            return data;
        }

        public override string GetData()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                {
                    if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                        communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                    else
                        communications.BaudRate = 300;
                }
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return data;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.Command = command.DTMDayManfCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            Application.DoEvents();
                            isCorruptedData = true;
                            return data;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    string commandPassword = command.DTMDayPasswordCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    communications.ReadFlag = false;
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = "";
                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            return data;
                        }
                        if (communications.ReadFlag)
                            break;
                        if (IsAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            Application.DoEvents();
                            return string.Empty;
                        }
                    } while (true);
                    if (communications.OutBuffer.Length >= 3)
                    {
                        Thread.Sleep(200);
                        bool Bccres = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), communications.OutBuffer.Length - 3, communications.OutBuffer.Substring(communications.OutBuffer.Length - 1, 1));
                        if (Bccres)
                        {
                            data = communications.OutBuffer;
                        }
                    }
                }
                else
                    IsSignOnFailure = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (communications != null)
                {
                    communications.DelayExecution();
                    communications.Command = command.BreakCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    communications.ClosePort();
                }
            }
            return data;
        }
    }
}