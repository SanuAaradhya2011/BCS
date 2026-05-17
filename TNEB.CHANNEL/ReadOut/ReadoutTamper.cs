using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using CAB.UI.Controls;

namespace CAB.IECChannel.ReadOut
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
        private bool SignOn()
        {
            bool validSignOn = true;
            if (!communications.SignOn())
            {
                IsSignOnFailure = true;
                communications.DelayExecution();
                if (communications.ComPort.IsOpen)
                    communications.ClosePort();
                     validSignOn = false;
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
                        validSignOn = false;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                Thread.Sleep(200);
                communications.CommandID = 2;
              

            }
            if (!validSignOn)
            {
                return false;
            }
            else
            {
                return communications.ReadFlag;
            }

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
                if (!communications.SignOn())
                {
                    IsSignOnFailure = true;
                    communications.DelayExecution();
                    if (communications.ComPort.IsOpen)
                        communications.ClosePort();
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
                            if (CompartmentCounter > 0)
                            {
                                SignOn();
                            }
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
                                               // this.StatusMessage = string.Concat(MessageConstant.GetText("M000070"), (CompartmentCounter).ToString(), "    ", MessageConstant.GetText("M000043"), " ", communications.TotalReadBytes);
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
                            //if (CompartmentCounter > 0 && CompartmentCounter < 5)
                            //    this.StatusMessage = string.Concat(MessageConstant.GetText("M000053"), " ", (CompartmentCounter).ToString());

                            if (CompartmentCounter < 5)
                                command.TamperEvent = ((CompartmentCounter + 1) + 30).ToString() + this.CompartmentCommand().Substring(CompartmentCounter * 8, 8);
                            CompartmentCounter++;
                        } while (CompartmentCounter <= 5);

                    }
                    data = Convert.ToChar(1).ToString() + "TM" + MeterID(communications.ResponseSignOn) + "/" + ReadingDateTime + data + Convert.ToChar(4);
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

        public override string GetInstantData()
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
                    Application.DoEvents();
                    return data;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    communications.OutBuffer = string.Empty;
                    communications.Command = command.TamperStatusManfCommand;
                    communications.SendCommand();
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    //Thread.Sleep(200);
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
         
                    string commandPassword = command.TamperStatusCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace("Bcc", ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = string.Empty;
                   
                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            break;
                        }
                    }while (communications.OutBuffer.Length < 9);
                    
                    if (communications.OutBuffer.Length >= 9)
                    {
                        communications.DelayExecution();
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
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }


    }
}
