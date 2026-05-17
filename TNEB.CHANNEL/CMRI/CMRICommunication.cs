using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECChannel.ReadOut;

namespace CAB.IECChannel.CMRI
{
	public class ReadCMRI : ReadBase
	{
		private IECChannelBase communications;


		public ReadCMRI()
		{
			command = Command.GetInstance();
		}

		public IECChannelBase Channel
		{
			get { return communications; }
			set { communications = value; }
		}

		private string GetASCIISendCommand(string cmd)
		{
			StringBuilder sbcmd = new StringBuilder();
			foreach (char ch in cmd)
			{
				int value = Convert.ToInt32(ch);
				sbcmd.Append(String.Format("{0:X}", value));
			}
			return sbcmd.ToString();
		}

		public string GetData()
		{
            try
            {
                IsSignOnFailure = false;
                string strOutput = string.Empty;
                string totalBytesCommand = string.Empty;
                string sendcmd = "72646F7574"; //rdout
                string strinput = string.Empty;


                communications.BaudRate = 19200;
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                }
                else
                {
                    communications.CommandID = 0;
                    communications.OutBuffer = string.Empty;
                    communications.Command = sendcmd;
                    if (communications.SendCommand())
                    {
                        communications.IsDataReceived = false;
                        communications.CurrentTime = DateTime.Now;

                        do
                        {
                            if (communications.Timeout())
                            { 
                                this.StatusMessage = "Timeout !";
                                Application.DoEvents(); 
                                break;
                            }
                        } while (true);


                        if (communications.OutBuffer.Length >= 1)
                        {
                            if (communications.OutBuffer.Contains(""))
                            {
                                totalBytesCommand = communications.OutBuffer.Replace("", "");
                            }
                            totalBytesCommand = Convert.ToString(Convert.ToInt32(totalBytesCommand) / 512 + 1);
                            if (totalBytesCommand != "")
                            {
                                communications.OutBuffer = string.Empty;
                                communications.CommandID = 9;
                                communications.Command = GetASCIISendCommand(totalBytesCommand);
                                if (communications.SendCommand())
                                {
                                    communications.IsDataReceived = false;
                                    communications.CurrentTime = DateTime.Now;
                                    do
                                    {
                                        if (communications.Timeout())
                                        {
                                            Application.DoEvents();
                                            break;
                                        }
                                    } while (true);
                                }
                            }
                        }
                    }
                }
                strOutput = communications.OutBuffer.Replace("Pkt", "");
                strOutput = strOutput.Replace("End", "");
                return strOutput;
            }
            catch (CABException)
            {
                return null;
            }
            finally
            {
                communications.ClosePort();
            }
		}
	}
}

			
