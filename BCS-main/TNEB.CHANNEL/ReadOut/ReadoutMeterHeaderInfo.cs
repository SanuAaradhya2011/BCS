using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using CAB.UI.Controls;
using System.Windows.Forms;

namespace CAB.IECChannel.ReadOut
{
    public class ReadoutMeterHeaderInfo  : ReadBase
    {
        public ReadoutMeterHeaderInfo()
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
                }
                else
                {
                    communications.DelayExecution();
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
                    {
                        communications.DelayExecution();
                        communications.Command = command.FraudEnergyManfCommand;
                        communications.SendCommand();
                        communications.DelayExecution();
                        communications.DelayExecution();
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
                        string headerInfoCommand = "3A31313131313131314D73040D0A";
                        headerInfoCommand = headerInfoCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        string CmdBcc = ReadoutCommon.ReturnBcc(headerInfoCommand.Substring(0, headerInfoCommand.Length - 7));
                        headerInfoCommand = headerInfoCommand.Replace(ReadoutConstant.BCC, CmdBcc);
                        communications.Command = headerInfoCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        communications.IsDataReceived = false;
                        communications.ReadFlag = false;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        do
                        {
                            if (communications.Timeout())
                            {
                                this.StatusMessage = MessageConstant.GetText("M000041");
                                Application.DoEvents();
                                break;
                            }

                            if (IsAborted)
                            {
                                this.StatusMessage = "User Aborted.";
                                Application.DoEvents();
                                return string.Empty;
                            }
                          
                        } while (communications.OutBuffer.Length < 25);
                        if (communications.ReadFlag )
                        {
                            data = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                                data = string.Empty;
                            data = string.Concat(Convert.ToChar(1), "HD", communications.ResponseSignOn.Replace("\r\n", string.Empty), "/", this.ReadingDateTime, "/M00000000", data, Convert.ToChar(4));

                        }

                        /* GKG 28/01/2013 TANGEDCO ISSUE*/
                        string billingInfoCommand = "01523102303641352831290320";
                        billingInfoCommand = billingInfoCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        CmdBcc = ReadoutCommon.ReturnBcc(billingInfoCommand.Substring(0, headerInfoCommand.Length - 7));
                        billingInfoCommand = billingInfoCommand.Replace(ReadoutConstant.BCC, CmdBcc);
                        communications.Command = billingInfoCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        communications.IsDataReceived = false;
                        communications.ReadFlag = false;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        do
                        {
                            if (communications.Timeout())
                            {
                                this.StatusMessage = MessageConstant.GetText("M000041");
                                Application.DoEvents();
                                break;
                            }

                            if (IsAborted)
                            {
                                this.StatusMessage = "User Aborted.";
                                Application.DoEvents();
                                return string.Empty;
                            }

                        } while (communications.OutBuffer.Length < 7);
                        if (communications.ReadFlag)
                        {
                            string billingType = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(billingType.Substring(1), billingType.Length - 3, billingType.Substring(billingType.Length - 1, 1)))
                                billingType = string.Empty;
                            data = data.Insert(data.Length - 3, billingType.Substring(billingType.Length - 5, 2));
                        }
                        /* GKG 28/01/2013 TANGEDCO ISSUE*/
                    }
                    else
                        IsSignOnFailure = true;
                }
            }
            catch (Exception)
            {
                data = string.Empty;
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
    }
}
