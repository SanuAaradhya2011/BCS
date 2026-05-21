using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;

namespace CAB.IECChannel.ReadOut
{
    public class ReadoutTransaction : ReadBase
    {
        public ReadoutTransaction()
        {
            command = Command.GetInstance();
        }

        public override string GetData()
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
                if (communications.SignOn())
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
                    //Thread.Sleep(200);
                    communications.DelayExecution();
                    if (ReadoutConstant.METERPASSWORD.Length != 0)
                    {
                        string transactionCommand = command.MeterProgCommand;
                        transactionCommand = transactionCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        string CmdBcc = ReadoutCommon.ReturnBcc(transactionCommand.Substring(0, transactionCommand.Length - 7));
                        transactionCommand = transactionCommand.Replace(ReadoutConstant.BCC, CmdBcc);
                        communications.Command = transactionCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        if (!communications.SendCommand())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                        }
                        else
                        {
                            //this.StatusMessage = MessageConstant.GetText("M000052");
                            //Application.DoEvents();
                            communications.CurrentTime = DateTime.Now;
                            communications.ReadFlag = false;
                            communications.IsDataReceived = false;
                            do
                            {
                                if (communications.Timeout())
                                {
                                    this.StatusMessage = MessageConstant.GetText("M000040");
                                    Application.DoEvents();
                                    break;
                                }
                                if (communications.ReadFlag == true)
                                    break;
                                if (isAborted)
                                {
                                    this.StatusMessage = "User Aborted.";
                                    System.Windows.Forms.Application.DoEvents();
                                    return string.Empty;
                                }
                            } while (true);

                            if (communications.OutBuffer.Length >= 13)
                            {
                                Thread.Sleep(200);
                                data = communications.OutBuffer;
                                bool Bccres = ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1));
                                communications.OutBuffer = string.Empty;
                                communications.DelayExecution();
                                Thread.Sleep(200);
                                string rtcData = ReadRTCProgramm(ReadoutConstant.METERPASSWORD);
                                data = string.Concat(data, ReadoutConstant.TRANSACTION, rtcData);
                                // data = string.Concat(data, ReadoutConstant.TRANSACTION,"<RTC>", rtcData);
                                communications.DelayExecution();
                                communications.ClosePort();
                            }
                        }
                    }
                }
                else
                {
                    IsSignOnFailure = true;
                    data = string.Empty;
                }
            }
            catch (Exception)
            {
                data = string.Empty;
            }
            finally
            {
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }

        public string ReadRTCProgramm(string MeterPass)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {

                communications.CurrentTime = DateTime.Now;
                if (ConfigInfo.IsGSMConnected())
                {
                    if (ConfigInfo.GetLocalMode().Equals("Optical"))
                        communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                    if (!communications.OpenPort())
                    {
                        this.StatusMessage = MessageConstant.GetText("M000038");
                        return string.Empty;
                    }
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
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
                        //Thread.Sleep(200);
                        communications.DelayExecution();
                    }
                }
                string PassCmd = command.ReadRTCUpdateCommand;
                PassCmd = PassCmd.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(MeterPass));
                string CmdBcc = ReadoutCommon.ReturnBcc(PassCmd.Substring(0, PassCmd.Length - 7));
                PassCmd = PassCmd.Replace(ReadoutConstant.BCC, CmdBcc);
                communications.Command = PassCmd;
                communications.CommandID = 2;
                communications.OutBuffer = string.Empty;
                communications.ReadFlag = false;
                if (!communications.SendCommand())
                {
                    this.StatusMessage = MessageConstant.GetText("M000040");
                    Application.DoEvents();
                }
                else
                {
                    //this.StatusMessage = MessageConstant.GetText("M000050");
                    //Application.DoEvents();
                    communications.CurrentTime = DateTime.Now;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            break;
                        }
                        if (isAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            System.Windows.Forms.Application.DoEvents();
                            return string.Empty;
                        }
                        if (communications.ReadFlag)
                            break;
                    } while (true);

                    if (communications.OutBuffer.Length >= 13)
                    {
                        Thread.Sleep(200);
                        data = communications.OutBuffer;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }
    }
}
