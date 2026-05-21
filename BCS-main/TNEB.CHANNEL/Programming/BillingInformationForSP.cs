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

    public class BillingInformationForSP : ReadBase
    {
        public BillingInformationForSP()
        {
            command = Command.GetInstance();
        }

        public bool ResetBilling()
        {

            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                Thread.Sleep(1000);
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    IsSignOnFailure = false;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOnForSPhaseIEC())
                {
                    communications.DelayExecution();
                    communications.ClosePort();
                    this.StatusMessage = "Signon failure.";
                    IsSignOnFailure = false;
                }
                else
                {
                    communications.DelayExecution();
                    if (communications.ReadFlag)
                    {
                        communications.TotalReadBytes = 0;
                        string passwordData = string.Empty;
                        communications.ReadFlag = false;
                        communications.Command = command.DTMDailySurveyCommand_A;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        if (!communications.SendCommand())
                            return false;// "2";
                        else
                        {
                            Thread.Sleep(200);
                            int index = communications.ResponseSignOn.IndexOf("/");
                            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                            Thread.Sleep(200);
                            do
                            {
                                if (!IsAborted)
                                {
                                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                                    Application.DoEvents();
                                }
                                else
                                {
                                    this.StatusMessage = "User Aborted.";
                                    Application.DoEvents();
                                    IsSignOnFailure = false;
                                    break; // Story - 354382 - During parameter written should break when user aborted
                                }
                                if (communications.Timeout())
                                {
                                    IsSignOnFailure = false;
                                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040");
                                    break; // Story - 354382 - During parameter written should break when timeout
                                    //return "1";
                                }
                                else
                                {
                                    if (communications.ReadFlag)
                                    {
                                        Thread.Sleep(200);
                                        passwordData = communications.OutBuffer;
                                        break;
                                    }
                                }
                            } while (true);

                            if (passwordData != string.Empty)
                            {
                                string commandPassword = command.PasswordCommandRTC_SP;
                                commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                                commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(2, commandPassword.Length - 5)));
                                communications.DelayExecution();
                                communications.CurrentTime = DateTime.Now;
                                communications.ReadFlag = false;
                                communications.Command = commandPassword;
                                communications.CommandID = 2;
                                communications.OutBuffer = "";
                                communications.IsDataReceived = false;
                                communications.SendCommand();

                                Thread.Sleep(200);
                                do
                                {
                                    if (communications.Timeout())
                                    {
                                        IsSignOnFailure = false;
                                        break;
                                    }
                                    if (communications.ReadFlag)
                                        break;
                                    if (IsAborted)
                                    {
                                        this.StatusMessage = "User Aborted.";
                                        Application.DoEvents();
                                        IsSignOnFailure = false;
                                        break; // Story - 354382 - During parameter written should break when user aborted
                                    }
                                } while (true);

                                if (communications.OutBuffer.Length >= 0)
                                {
                                    if (communications.OutBuffer.Contains("\x15"))
                                    {
                                        this.StatusMessage = "Access Denied.";
                                        Application.DoEvents();
                                        data = string.Empty;
                                        IsSignOnFailure = false;
                                    }
                                    if (communications.OutBuffer.Contains("\x06"))
                                    {
                                        communications.DelayExecution();
                                        communications.CurrentTime = DateTime.Now;
                                        communications.ReadFlag = false;

                                        string commandBillingReset = command.BillingEnergyReset_SP;
                                        commandBillingReset = commandBillingReset.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandBillingReset.Substring(2, commandBillingReset.Length - 5)));
                                        communications.Command = commandBillingReset;
                                        communications.CommandID = 2;
                                        communications.OutBuffer = "";
                                        communications.IsDataReceived = false;
                                        communications.SendCommand();
                                        data = "";
                                        Thread.Sleep(100);
                                        communications.CurrentTime = DateTime.Now;
                                        do
                                        {                                            
                                            //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
                                            //{
                                            //    if (communications.OutBuffer.Contains("\x04"))
                                            //    {
                                            //    }
                                            //    else
                                            //    {
                                            //        //this.StatusMessage = "Read Completed";
                                            //        Application.DoEvents();
                                            //        data += communications.OutBuffer;
                                            //        IsSignOnFailure = true;
                                            //        break;
                                            //    }
                                            //}
                                            if (communications.OutBuffer.Contains("\x15"))
                                            {
                                                this.StatusMessage = "Access Denied.";
                                                Application.DoEvents();
                                                data = string.Empty;
                                                IsSignOnFailure = false;
                                                break;
                                            }
                                            if (communications.OutBuffer.Contains("\x06"))
                                            {
                                                //this.StatusMessage = "Read Completed";
                                                Application.DoEvents();
                                                data += communications.OutBuffer;
                                                IsSignOnFailure = true;
                                                break;
                                            }
                                            if (IsAborted)
                                            {
                                                this.StatusMessage = "User Aborted.";
                                                Application.DoEvents();
                                                data = string.Empty;
                                                IsSignOnFailure = false;
                                                break; // Story - 354382 - During parameter written should break when user aborted
                                            }
                                            //Timeout check for break should be after check for response from Meter
                                            if (communications.Timeout())
                                            {
                                                //this.StatusMessage = MessageConstant.GetText("M000040");
                                                //Application.DoEvents();
                                                this.StatusMessage = "Timeout.";
                                                IsSignOnFailure = false;
                                                break;
                                            }
                                        } while (true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Thread.Sleep(200);
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                Thread.Sleep(200);
            }
            return IsSignOnFailure;

            /////////////////////////////////////////////////////////////





            //char charACK;
            //IsSignOnFailure = false;
            //bool flag = false;
            //string data = string.Empty;
            //string rtcCommand = string.Empty;

            //try
            //{
            //    if (ConfigInfo.GetLocalMode().Equals("Optical"))
            //        communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
            //    if (!communications.OpenPort())
            //    {
            //        this.StatusMessage = "Error in opening port";
            //        return false;
            //    }

            //    communications.CurrentTime = DateTime.Now;
            //    if (!communications.SignOn())
            //    {
            //        this.StatusMessage = "Signon failure.";
            //        return false;
            //    }
            //    communications.DelayExecution();
            //    communications.Command = command.BillingEnergyResetManfCommand;
            //    communications.OutBuffer = string.Empty;
            //    communications.CommandID = 2;
            //    communications.SendCommand();
            //    communications.DelayExecution();
            //    Thread.Sleep(200);
            //    if (communications.ResponseSignOn != string.Empty)
            //    {
            //        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
            //        {
            //            this.StatusMessage = "Signon failure.";
            //            return flag;
            //        }
            //    }
            //    int index = communications.ResponseSignOn.IndexOf("/");
            //    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
            //    communications.CurrentTime = DateTime.Now;

            //    communications.OutBuffer = string.Empty;
            //    rtcCommand = command.BillingEnergyResetPasswordCommand;
            //    rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));

            //    string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(0, rtcCommand.Length - 7));
            //    rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

            //    communications.Command = rtcCommand;
            //    communications.CommandID = 3;
            //    communications.ReadFlag = false;
            //    communications.IsDataReceived = false;
            //    communications.OutBuffer = string.Empty;
            //    communications.CurrentTime = DateTime.Now;
            //    communications.SendCommand();
            //    do
            //    {
            //        if (communications.Timeout())
            //        {
            //            this.StatusMessage = "Timeout!";
            //            Application.DoEvents();
            //            return flag;
            //        }
            //    } while (communications.OutBuffer.Length < 1);

            //    if (communications.OutBuffer.Length >= 1)
            //    {
            //        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
            //        if (charACK == 6)
            //        {
            //            //this.StatusMessage = "Billing was reset successfully.";
            //        }                    
            //        else
            //        {
            //            this.StatusMessage = "Access Denied";
            //            return flag;
            //        }
            //    }


            //}
            //catch (Exception ex)
            //{
            //    new CABException(ex);
            //}
            //finally
            //{
            //    communications.Command = command.BreakCommand;
            //    communications.SendCommand();
            //    communications.DelayExecution();
            //    communications.ClosePort();
            //}
            //return flag;
        }
    }
}