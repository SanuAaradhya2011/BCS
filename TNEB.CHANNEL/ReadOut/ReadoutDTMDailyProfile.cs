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
    public class ReadoutDTMDailyProfile : ReadBase
    {
        
        bool isTNEB = false;
        public ReadoutDTMDailyProfile(bool ISTNEB)
        {
            this.isTNEB = ISTNEB;
        }
        public override string GetDTMParameterData()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                    //Gaurav Bhardwaj
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
                {
                    IsSignOnFailure = true;
                    communications.DelayExecution();
                    if (communications.ComPort.IsOpen)
                        communications.ClosePort();
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

        /* GKG 04/03/2013 TANGEDCO ISSUE*/
        public override string GetFirmWareVersion()
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
                        if (communications.ReadFlag)
                        {
                            data = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                                data = string.Empty;
                            data = String.Format("{0:0.00}", Convert.ToDouble(Convert.ToInt32(data.Substring(data.Length - 6, 4), 16)) / 100);

                        }
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
        /* GKG 04/03/2013 TANGEDCO ISSUE*/

        public override string GetData()
        {
            /* GKG 04/03/2013 TANGEDCO ISSUE*/
            string fwVersion = string.Empty;

            if(isTNEB)
            {
                fwVersion = GetFirmWareVersion();
            }
            /* GKG 04/03/2013 TANGEDCO ISSUE*/

            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                    string commandPassword; 
                    if (isTNEB)
                    {
                        /* GKG 04/03/2013 TANGEDCO ISSUE*/
                        if (fwVersion == "0.27" || fwVersion == "0.70"|| fwVersion == "0.78" ||fwVersion == "1.29" ||  fwVersion == "1.56"  )
                        {
                            commandPassword = command.DTMDayPasswordCommand;
                        }
                        else
                        {
                            commandPassword = command.DTMNonTNEBDayPasswordCommand;
                        }
                        /* GKG 04/03/2013 TANGEDCO ISSUE*/
                    }
                    else
                    {
                        commandPassword = command.DTMNonTNEBDayPasswordCommand;
                    }
                   
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
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }
    }
}