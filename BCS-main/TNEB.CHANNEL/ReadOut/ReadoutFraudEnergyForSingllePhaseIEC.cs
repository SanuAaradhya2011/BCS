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
    public class ReadoutFraudEnergyForSingllePhaseIEC : ReadBase
    {
        public ReadoutFraudEnergyForSingllePhaseIEC()
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
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                }
                else
                {
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOnForSPhaseIEC())
                    {
                        communications.DelayExecution();
                        communications.Command = command.FraudEnergyManfCommand;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
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
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
                        string fraudEnergyCommand = command.ReadFraudEnergyCommand;
                        fraudEnergyCommand = fraudEnergyCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        string CmdBcc = ReadoutCommon.ReturnBcc(fraudEnergyCommand.Substring(0, fraudEnergyCommand.Length - 7));
                        fraudEnergyCommand = fraudEnergyCommand.Replace(ReadoutConstant.BCC, CmdBcc);
                        communications.Command = fraudEnergyCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        communications.IsDataReceived = false;
                        communications.SendCommand();
                        communications.DelayExecution();
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
                                Application.DoEvents();
                                return string.Empty;
                            }
                        } while (communications.OutBuffer.Length < 42);
                        if (communications.OutBuffer.Length >= 40)
                        {

                            Thread.Sleep(300);
                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
                            data = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                                data = string.Empty;


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

        public override string ReverseEnergy()
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
                }
                else
                {
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
                    {
                        communications.DelayExecution();
                        communications.Command = command.ReverseEnergyManfCommand;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
                        if (communications.ResponseSignOn != string.Empty)
                        {
                            if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                            {
                                this.StatusMessage = MessageConstant.GetText("M000039");
                                return string.Empty;
                            }
                        }
                        int index = communications.ResponseSignOn.IndexOf("/");
                        communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                        Thread.Sleep(200);
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
                        communications.OutBuffer = string.Empty;
                        string phasorPasswordCommand = command.ReadReverseEnergyCommand;
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
                                this.StatusMessage = MessageConstant.GetText("M000065");
                                Application.DoEvents();
                                return string.Empty;
                            }
                            if (IsAborted)
                            {
                                this.StatusMessage = "User Aborted.";
                                Application.DoEvents();
                                return string.Empty;
                            }
                        } while (communications.OutBuffer.Length < 19);
                        if (communications.OutBuffer.Length >= 10)
                        {
                            Thread.Sleep(100);
                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Fraud Energy for single phase
                            bool Bccres = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), communications.OutBuffer.Length - 3, communications.OutBuffer.Substring(communications.OutBuffer.Length - 1, 1));
                            if (Bccres == true)
                                data = communications.OutBuffer;
                            else
                                data = string.Empty;
                        }
                    }
                    else
                        IsSignOnFailure = true;
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
