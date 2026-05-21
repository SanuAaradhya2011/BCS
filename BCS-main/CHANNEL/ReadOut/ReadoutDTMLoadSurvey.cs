using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using CAB.UI.Controls;

namespace CAB.Channel.ReadOut
{
    public class ReadoutDTMLoadSurvey : ReadBase
    {
        private static string responseForLoadSurvey;
        public ReadoutDTMLoadSurvey()
        {
            command = Command.GetInstance();
        }

        public string LoadDTMDay()
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
                    return null;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.Command = command.ManufactureCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            IsSignOnFailure = true;
                            return null;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    this.StatusMessage = MessageConstant.GetText("M000067");
                    string commandPassword = "";
                    //if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                    //    commandPassword = command.LoadLPasswordCommand;
                    //else
                        commandPassword = command.DTMLoadLPasswordCommand;

                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = string.Empty;
                    communications.IsDataReceived = false;
                    communications.ReadFlag = false;
                    communications.SendCommand();
                    int num = 0;
                    if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                        num = 25;
                    else
                        num = 5;
                    do
                    {
                        if (communications.Timeout())
                        {
                            if (communications.OutBuffer.Length >= num)
                                break;
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            if (communications.OutBuffer.Length < 4)
                                break;
                        }
                        if (communications.ReadFlag)
                            break;
                    } while (true);

                    if (communications.OutBuffer.Length >= 5)
                    {
                        Thread.Sleep(200);
                        data = communications.OutBuffer;
                        if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                            responseForLoadSurvey = data.Substring(0, 21);
                        bool flag = ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1));
                        if (!flag)
                        {
                            data = string.Empty;
                            this.StatusMessage = MessageConstant.GetText("M000056");
                            Application.DoEvents();
                        }
                    }
                }
                else
                    IsSignOnFailure = true;
            }
            catch (Exception)
            {
                data = string.Empty;
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

        public string GetData(string numberOfDays)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (numberOfDays.Trim() == string.Empty)
                    return data;
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
                    return null;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.Command = command.DTMLoadManfCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            return null;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    this.StatusMessage = MessageConstant.GetText("M000066");
                    string commandPassword = command.DTMLoadPasswordCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                    commandPassword = commandPassword.Replace("ND", ReadoutCommon.DTMStringToHex(numberOfDays));
                    else
                    commandPassword = commandPassword.Replace("ND", ReadoutConstant.DTMPROFILENUMBER + ReadoutCommon.DTMStringToHex(numberOfDays));
                    commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = string.Empty;
                    communications.IsDataReceived = false;
                    communications.ReadFlag = false;
                    communications.SendCommand();

                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            break;
                        }
                        if (communications.ReadFlag)
                            break;
                        if (!IsAborted)
                           // this.StatusMessage = string.Concat(MessageConstant.GetText("M000043"), "  ", communications.TotalReadBytes);
                        if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                        {
                           // this.StatusMessage = string.Concat("Total Read Bytes : ", communications.TotalReadBytes);
                            Application.DoEvents();
                        }
                    } while (true);

                    if (communications.OutBuffer.Length >= 13)
                    {
                        Thread.Sleep(200);
                        data = communications.OutBuffer;
                        bool flag = ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1));
                        if (!flag)
                        {
                            data = string.Empty;
                            this.StatusMessage = MessageConstant.GetText("M000056");
                            Application.DoEvents();
                        }
                        else
                        {
                            if (data.Length >= 15)
                            {
                                if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                                    data = Convert.ToChar(1) + "L" + communications.ResponseSignOn.Replace(ReadoutConstant.CRETURNENTER, "") + "/" + this.ReadingDateTime + responseForLoadSurvey + data + Convert.ToChar(4);
                                else
                                    data = Convert.ToChar(1) + ReadoutConstant.DTMPROFILE + communications.ResponseSignOn.Replace(ReadoutConstant.CRETURNENTER, "") + "/" + this.ReadingDateTime + data + Convert.ToChar(4);
                            }
                        }
                    }
                }
                else
                    IsSignOnFailure = true;
            }
            catch (Exception)
            {
                data = string.Empty; ;
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

