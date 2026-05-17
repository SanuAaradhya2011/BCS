using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CAB.IECChannel.ReadOut;
using System.Threading;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Contracts;
using CABAppControl;
using CABEntity;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CAB.IECChannel.Programming;

namespace CAB.IECChannel.ReadOut
{
    public class ReadMeterAccuracyCheck : ReadBase
    {
        private IECLocalCommunication communications;
        public ReadMeterAccuracyCheck()
        {
            command = Command.GetInstance();
            communications = ChannelManager.GetChannel() as IECLocalCommunication;
        }

        public bool ReadCommandsMeterAccuracyCheck(string ReadCommand, ref string data, int IsMeterType)
        {
            string strReadCommand = string.Empty;
            try
            {
                communications.OutBuffer = string.Empty;
                strReadCommand = ReadCommand;
                // Story - 369686 - Accuracy check for single phase IEC meter

                string calculatedBcc = string.Empty;
                if (IsMeterType == 2)
                {
                    calculatedBcc= ReadoutCommon.ReturnBcc(strReadCommand.Substring(2, strReadCommand.Length-5));
                    communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                    communications.Parity = System.IO.Ports.Parity.None;
                    communications.DataBits = 8;
                }
                else if (IsMeterType == 1)
                {
                    calculatedBcc = ReadoutCommon.ReturnBcc(strReadCommand.Substring(2, strReadCommand.Length - 5));
                    communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                    communications.Parity = System.IO.Ports.Parity.Even;
                    communications.DataBits = 7;
                }
                else if (IsMeterType == 3)
                {
                    calculatedBcc = ReadoutCommon.ReturnBcc(strReadCommand.Substring(0, strReadCommand.IndexOf("Bcc")));
                }
                strReadCommand = strReadCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                communications.Command = strReadCommand;
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
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        break;

                    }
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        return false;
                    }
                } while (true);
                if (communications.ReadFlag)
                {
                    communications.DelayExecution();
                    data = communications.OutBuffer;
                    if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                        data = string.Empty;
                }
                return true;
            }
            catch (Exception)
            {
                this.StatusMessage = "Failure in Meter Accuracy Check Read";
                return true;
            }
        }


        public string HandshakeCommandsMeterAccuracyCheck(bool IsManufactureSpecificReadCmd, int IsMeterType)
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            string passwordCommand = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (IsMeterType == 2)
                {
                    communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                    communications.Parity = System.IO.Ports.Parity.None;
                    communications.DataBits = 8;
                }
                else if (IsMeterType == 1)
                {
                    communications.ChannelType = ConfigSettings.GetValue("ChannelType");
                    communications.Parity = System.IO.Ports.Parity.Even;
                    communications.DataBits = 7;
                }
                if (!communications.OpenPort())
                {

                    this.StatusMessage = "Error in opening port";
                    return null;
                }

                communications.CurrentTime = DateTime.Now;
                if (IsMeterType == 1 || IsMeterType==2)
                {
                    if (!communications.SignOnForSPhaseIEC())
                    {
                        this.StatusMessage = "Sign-On failure";
                        return null;
                    }
                }
                else
                {
                    if (!communications.SignOn())
                    {
                        this.StatusMessage = "Sign-On failure";
                        return null;
                    }
                }
                communications.DelayExecution();
                if (IsManufactureSpecificReadCmd)
                {
                    communications.Command = "063035360D0A";
                    communications.OutBuffer = string.Empty;
                    communications.CommandID = 2;
                    communications.SendCommand();
                    this.StatusMessage = "Sign On";
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (IsMeterType == 3 )
                        {
                            if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                            {
                                this.StatusMessage = "Sign-On failure/Not Supported.";
                                return null;
                            }
                        }

                    }
                    else
                    {
                        this.StatusMessage = "Sign-On failure/Not Supported.";
                        return null;
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    communications.CurrentTime = DateTime.Now;


                }
            }
            catch (Exception)
            {
                return null;
            }
            return communications.ResponseSignOn + "|" + data;
        }


        public void BreakCommunication()
        {
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            if (communications.ComPort.IsOpen)
                communications.ClosePort();
        }

    }
}
