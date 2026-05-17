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
    public class ReadoutTamper : ReadBase
    {
        public ReadoutTamper()
        {
            command = Command.GetInstance();
        }

        private string CompartmentCommand()
        {
            string compartmentCommand = string.Empty;
            string hexData = ReadoutCommon.StrToHex("100");
            for (int i = 0; i < 6; i++)
                compartmentCommand = string.Concat(compartmentCommand, hexData);
            return compartmentCommand;
        }
        public override string GetData()
        {
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                return GetData250();
            else
                return GetData650();
        }
        public string GetData650()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = 300;
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    IsSignOnFailure = true;
                    return string.Empty;
                }
                else
                {
                    communications.DelayExecution();
                    communications.Command = command.ManufactureCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            isCorruptedData = true;
                            return string.Empty; 
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    command.TamperEvent = "30";
                    communications.CommandID = 2;
                    int CompartmentCounter = 0;
                    if (communications.ReadFlag)
                    {
                        do
                        {
                            communications.ReadFlag = false;
                            int TotalRetry = 3;
                            communications.CommandID = 2;
                            communications.TotalReadBytes = 0;
                            communications.OutBuffer = string.Empty;
                            communications.Command = command.VCommand;
                            while (TotalRetry-- > 0)
                            {
                                if (!communications.SendCommand())
                                {
                                    return "2";
                                }
                                else
                                {
                                    communications.CurrentTime = DateTime.Now;
                                    do
                                    {
                                        if (!IsAborted)
                                        {
                                          //  this.StatusMessage = string.Concat(MessageConstant.GetText("M000070"), (CompartmentCounter + 1).ToString(), "    ", MessageConstant.GetText("M000043"), " ", communications.TotalReadBytes);
                                            Application.DoEvents();
                                        }
                                         else
                                            {
                                                this.StatusMessage = "User Aborted.";
                                                System.Windows.Forms.Application.DoEvents();
                                                return string.Empty;
                                            }
                                        if (communications.Timeout())
                                        {
                                            if (TotalRetry > 0)
                                                break;
                                            this.StatusMessage = MessageConstant.GetText("M000040");
                                            if (IsAborted)
                                            {
                                                IsAborted = false;
                                            }
                                            else
                                                this.StatusMessage = MessageConstant.GetText("M000041");

                                            return "1";
                                        }
                                        if (communications.ReadFlag)
                                        {
                                            Thread.Sleep(150);
                                            bool chlbcc = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), (communications.OutBuffer.Length - 3), communications.OutBuffer.Substring((communications.OutBuffer.Length - 1), 1));
                                            if (chlbcc == false)
                                                return "3";
                                            data = string.Concat(data, communications.OutBuffer);
                                            break;
                                        }

                                    } while (true);
                                }
                                if (communications.ReadFlag)
                                    break;
                            }
                            if (communications.Timeout())
                                return "2";
                            if (CompartmentCounter < 6)
                                this.StatusMessage = string.Concat(MessageConstant.GetText("M000053"), " ", (CompartmentCounter + 1).ToString());
                            if (CompartmentCounter < 6)
                                command.TamperEvent = ((CompartmentCounter + 1) + 30).ToString() + this.CompartmentCommand().Substring(CompartmentCounter * 8, 8);
                            CompartmentCounter++;
                        } while (CompartmentCounter < 6);

                    }
                    data = Convert.ToChar(1).ToString() + "TM" + communications.ResponseSignOn.Replace("\r\n", string.Empty) + "/" + ReadingDateTime + data + Convert.ToChar(4);
                } 
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
        public string GetData250()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    IsSignOnFailure = true;
                    return string.Empty;
                }
                else
                {
                    communications.DelayExecution();
                    communications.Command = command.ManufactureCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            isCorruptedData = true;
                            return string.Empty;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    command.TamperEvent = "30";
                    communications.CommandID = 2;
                    int CompartmentCounter = 0;
                    if (communications.ReadFlag)
                    {
                        do
                        {
                            communications.ReadFlag = false;
                            int TotalRetry = 3;
                            communications.CommandID = 2;
                            communications.TotalReadBytes = 0;
                            communications.OutBuffer = string.Empty;
                            communications.Command = command.VCommand;
                            while (TotalRetry-- > 0)
                            {
                                if (!communications.SendCommand())
                                {
                                    return "2";
                                }
                                else
                                {
                                    communications.CurrentTime = DateTime.Now;
                                    do
                                    {
                                        if (CompartmentCounter > 0)
                                        {
                                            if (!IsAborted)
                                            {
                                              //  this.StatusMessage = string.Concat(MessageConstant.GetText("M000070"), (CompartmentCounter).ToString(), "    ", MessageConstant.GetText("M000043"), " ", communications.TotalReadBytes);
                                                Application.DoEvents();
                                            }
                                            else
                                            {
                                                this.StatusMessage = "User Aborted.";
                                                System.Windows.Forms.Application.DoEvents();
                                                return string.Empty;
                                            }
                                        }

                                        if (communications.Timeout())
                                        {
                                            if (TotalRetry > 0)
                                                break;
                                            this.StatusMessage = MessageConstant.GetText("M000040");
                                            if (IsAborted)
                                            {
                                                IsAborted = false;
                                            }
                                            else
                                                this.StatusMessage = MessageConstant.GetText("M000041");

                                            return "1";
                                        }
                                        if (communications.ReadFlag)
                                        {
                                            Thread.Sleep(150);
                                            bool chlbcc = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), (communications.OutBuffer.Length - 3), communications.OutBuffer.Substring((communications.OutBuffer.Length - 1), 1));
                                            if (chlbcc == false)
                                                return "3";
                                            data = string.Concat(data, communications.OutBuffer);
                                            break;
                                        }

                                    } while (true);
                                }
                                if (communications.ReadFlag)
                                    break;
                            }
                            if (communications.Timeout())
                                return "2";
                            if (CompartmentCounter > 0 && CompartmentCounter < 5)
                                this.StatusMessage = string.Concat(MessageConstant.GetText("M000053"), " ", (CompartmentCounter).ToString());

                            if (CompartmentCounter < 5)
                                command.TamperEvent = ((CompartmentCounter + 1) + 30).ToString() + this.CompartmentCommand().Substring(CompartmentCounter * 8, 8);
                            CompartmentCounter++;
                        } while (CompartmentCounter <= 5);

                    }
                    data = Convert.ToChar(1).ToString() + "TM" + communications.ResponseSignOn.Replace("\r\n", string.Empty) + "/" + ReadingDateTime + data + Convert.ToChar(4);
                }
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

        public override string GetInstantData()
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
                    communications.OutBuffer = string.Empty;
                    communications.Command = command.TamperStatusManfCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            isCorruptedData = true;
                            Application.DoEvents();
                            return data; 
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    string commandPassword = command.TamperStatusCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace("Bcc", ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    communications.DelayExecution();
                    int totCount = 0;
                      if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                          totCount=9;
                    else
                          totCount=13;
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            break;
                        }
                    } while (communications.OutBuffer.Length < totCount);
                    communications.DelayExecution();
                    if (communications.OutBuffer.Length >= totCount)
                    {
                        Thread.Sleep(200);
                        data=communications.OutBuffer;

                    }
                }
                else
                {
                    IsSignOnFailure = true;
                    data = null;
                }
            }
            catch (Exception)
            {
                data = null;
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
