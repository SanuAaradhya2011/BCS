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
    public class ReadoutFirmwareVersion : ReadBase
    {    
        public ReadoutFirmwareVersion()
        {
            command = Command.GetInstance();
        }

        public override string GetFirmWareVersion()
        {
            string fileInput = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                    return fileInput;
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    communications.DelayExecution();
                    if (communications.ComPort.IsOpen)
                        communications.ClosePort();
                    return fileInput;
                }
                else
                {
                    communications.DelayExecution();
                    if (communications.ReadFlag)
                    {
                        communications.TotalReadBytes = 0;
                        communications.ReadFlag = false;
                        communications.Command = "063035360D0A";
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.SendCommand();
                        communications.DelayExecution();
                        communications.TotalReadBytes = 0;
                        communications.ReadFlag = false;
                        communications.Command = "3A30303030303030304824560D0A";
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.SendCommand();
                        do
                        {
                            fileInput = communications.OutBuffer;
                            if (fileInput.Length > 5)
                                break;
                        }
                        while (true);

                    }
                }
            }
            catch (Exception)
            {
                fileInput = "";
            }
            finally
            {
                communications.DelayExecution();
                communications.ClosePort();
            }
            return fileInput;
        }
       
    }
}
