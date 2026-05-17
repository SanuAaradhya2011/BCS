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
        
    public class CTRatioProgramming : ReadBase 
    {
        private int ctRatio;

        public CTRatioProgramming()
        {
            command = Command.GetInstance();
        }

        public int CTRatio 
        {
            get { return ctRatio; }
            set { ctRatio = value; }
        }

        public bool SetCTRatio()
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
                    this.StatusMessage = "Error in opening port.";
                else
                {
                    communications.CurrentTime = DateTime.Now;
                    if (!communications.SignOn())
                    {
                        this.StatusMessage = "Sign-On failed.";
                        return false;
                    }
                    communications.DelayExecution();
                    communications.Command = command.CTResetManfCommand;
                    communications.OutBuffer = string.Empty;
                    communications.CommandID = 2;
                    communications.SendCommand();
                    communications.DelayExecution();

                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = "Sign-On failed.";
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
                            Application.DoEvents();
                            return flag;
                        }
                    } while (communications.OutBuffer.Length < 5);

                    if (communications.OutBuffer.Length >= 5)
                    {
                        communications.OutBuffer = string.Empty;
                        string rtcCommand = command.CTResetPasswordCommand;
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
                                Application.DoEvents();
                                return flag;
                            }
                        } while (communications.OutBuffer.Length < 1);

                        if (communications.OutBuffer.Length >= 1)
                        {
                            charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                            if (charACK == 6)
                            {
                                communications.OutBuffer = string.Empty;
                                rtcCommand = command.CTWriteCommand;
                                rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(CTRatio.ToString("X2")));
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
                                        Application.DoEvents();
                                        return flag;
                                    }
                                } while (communications.OutBuffer.Length < 1);

                                if (communications.OutBuffer.Length >= 1)
                                {
                                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                                    if (charACK == 6)
                                        this.StatusMessage = "CT Ratio Updated";
                                    else if (charACK == 21)
                                        this.StatusMessage = "Access Denied. CT Ratio incompatible with configured resolution value.";
                                    
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
            }
            catch (Exception)
            {
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
    }
}