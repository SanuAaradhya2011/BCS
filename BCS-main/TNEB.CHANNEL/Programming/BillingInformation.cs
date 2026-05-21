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

    public class BillingInformation : ReadBase
    {
        public BillingInformation()
        {
            command = Command.GetInstance();
        }

        public bool ResetBilling()
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            string rtcCommand = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port";
                    return false;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Signon failure.";
                    return false;
                }
                communications.DelayExecution();
                communications.Command = command.BillingEnergyResetManfCommand;
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();
                Thread.Sleep(200);
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Signon failure.";
                        return flag;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;

                communications.OutBuffer = string.Empty;
                rtcCommand = command.BillingEnergyResetPasswordCommand;
                rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));

                string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(0, rtcCommand.Length - 7));
                rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                communications.Command = rtcCommand;
                communications.CommandID = 3;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;
                communications.CurrentTime = DateTime.Now;
                communications.SendCommand();
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        return flag;
                    }
                } while (communications.OutBuffer.Length < 1);

                if (communications.OutBuffer.Length >= 1)
                {
                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    if (charACK == 6)
                    {
                        //this.StatusMessage = "Billing was reset successfully.";
                    }                    
                    else
                    {
                        this.StatusMessage = "Access Denied";
                        return flag;
                    }
                }


            }
            catch (Exception ex)
            {
                new CABException(ex);
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