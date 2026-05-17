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
            string data=string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                    string commandPassword = command.LoadLPasswordCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
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
                            if (communications.OutBuffer.Length >= 5)
                                break;
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            if (communications.OutBuffer.Length < 4)
                                break;
                        }
                        if (communications.ReadFlag)
                            break;
                    } while (true);

                    if (communications.OutBuffer.Length >= 25)
                    {
                        Thread.Sleep(200);
                        data = communications.OutBuffer;
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
                {
                    IsSignOnFailure = true;
                    communications.DelayExecution();
                    if (communications.ComPort.IsOpen)
                        communications.ClosePort();
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
                    communications.ClosePort();
                }
            }
            return data;
        }

        public string GetData(string numberOfDays, int avaialbleDays)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (numberOfDays.Trim() == string.Empty)
                    return data;
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
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
                    //this.StatusMessage = MessageConstant.GetText("M000066");//this line was commented bcoz it said "Reading DTM LoadSurvey Data" 
                    this.StatusMessage = "Reading Load Survey Data";
                    string commandPassword = command.DTMLoadPasswordCommand; 
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    if (avaialbleDays >= 100)
                        commandPassword = commandPassword.Replace("ND", ReadoutCommon.DTMStringToHex2(numberOfDays));
                    else
                        commandPassword = commandPassword.Replace("ND", ReadoutCommon.DTMStringToHex(numberOfDays));
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
                        if (IsAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            System.Windows.Forms.Application.DoEvents();
                            return string.Empty;
                        }
                        if (communications.ReadFlag)
                            break;
                      //  this.StatusMessage = string.Concat( "Total Read Bytes : ", communications.TotalReadBytes);
                        Application.DoEvents();
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
                                data = Convert.ToChar(1) + "L" + MeterID(communications.ResponseSignOn) + "/" + this.ReadingDateTime + responseForLoadSurvey + data + Convert.ToChar(4); 
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
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }
    }
}

