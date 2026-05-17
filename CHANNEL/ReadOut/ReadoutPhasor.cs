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
    public class ReadoutPhasor : ReadBase
    { 
        private bool isPhasor;
        public ReadoutPhasor()
        {
            command = Command.GetInstance();
        } 
        public bool IsPhasor
        {
            get { return isPhasor; }
            set { isPhasor = value; }
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
                }
                else
                { 
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
                    {
                        communications.DelayExecution();
                        communications.Command = command.PhasorManfCommand;
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
                                return string.Empty;
                            }
                        }
                        int index = communications.ResponseSignOn.IndexOf("/");
                        communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                        Thread.Sleep(200);
                        communications.OutBuffer = string.Empty;
                        string phasorPasswordCommand = command.PhasorPasswordCommand;
                        phasorPasswordCommand = phasorPasswordCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        phasorPasswordCommand = phasorPasswordCommand.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(phasorPasswordCommand.Substring(0, phasorPasswordCommand.Length - 7)));
                        communications.Command = phasorPasswordCommand;
                        communications.CommandID = 2;
                        communications.CurrentTime = DateTime.Now;
                        communications.ReadFlag = false;
                        communications.IsDataReceived = false;
                        communications.SendCommand();
                        communications.DelayExecution();
                        do
                        {
                            if (communications.Timeout())
                            {
                                this.StatusMessage = MessageConstant.GetText("M000055");
                                Application.DoEvents();
                                return string.Empty;
                            }
                            if (IsAborted)
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
                            Thread.Sleep(100);
                            string tampalert = communications.OutBuffer;
                            bool Bccres = ReadoutCommon.CalculateBcc(tampalert.Substring(1), tampalert.Length - 3, tampalert.Substring(tampalert.Length - 1, 1));
                            if (Bccres == true)
                            {
                                data = communications.OutBuffer;
                                isPhasor = true;
                            }
                            else
                            {
                                this.StatusMessage = MessageConstant.GetText("M000056");
                                Application.DoEvents();
                                data = string.Empty;
                                isPhasor = false;
                            }
                        }
                    }
                    else
                    { 
                        IsSignOnFailure = true;
                    }
                }
            }
            catch (Exception)
            {
                isPhasor = false;
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