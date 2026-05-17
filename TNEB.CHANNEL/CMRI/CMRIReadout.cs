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
	public class CMRIReadout : ReadBase
	{ 
        public CMRIReadout()
		{
			command = Command.GetInstance();
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

        public override string GetData()
        {
            string retVal = string.Empty;
            try
            {

                IsSignOnFailure = false;
                string strOutput = string.Empty;
                string totalBytesCommand = string.Empty;
                long status;
                string sendcmd = "72646F7574"; //rdout
                string strinput = string.Empty;
                //this.StatusMessage = "";
                //Application.DoEvents();
                communications.BaudRate = 19200;
                if (!communications.OpenPort())
                {
                    //this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    retVal = MessageConstant.GetText("M000038");
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
                                break;
                            }
                        } while (true);

                        if (communications.OutBuffer.Length <= 0)
                        {
                            retVal = "Connection Timeout !";
                            //this.StatusMessage = "Timeout !";
                            Application.DoEvents();
                        }
                        else if (communications.OutBuffer.Length >= 1)
                        {
                            if (communications.OutBuffer.Length != 1)
                            {
                                totalBytesCommand = communications.OutBuffer.Substring(2);
                            }
                            //if (communications.OutBuffer.Contains(""))
                            //{
                            //    totalBytesCommand = communications.OutBuffer.Replace("", "");
                            //    totalBytesCommand = totalBytesCommand.Replace(" ", "");
                            //    totalBytesCommand = totalBytesCommand.Replace("", "");
                            //}
                            if (totalBytesCommand == "" || totalBytesCommand == "out")
                                retVal = string.Empty;

                            if (totalBytesCommand == string.Empty)
                            {
                                retVal = "No Data Available.";
                                return retVal;
                            }
                            //Piyush Singh Added for ALI Logic
                            //totalBytesCommand = Convert.ToString(Convert.ToInt32(totalBytesCommand) / 512 + 1);                          

                            if (totalBytesCommand != "")
                            {
                                if (Convert.ToInt32(totalBytesCommand) % 512 == 0)
                                    totalBytesCommand = Convert.ToString(Convert.ToInt32(totalBytesCommand) / 512);
                                else
                                    totalBytesCommand = Convert.ToString(Convert.ToInt32(totalBytesCommand) / 512 + 1);

                                communications.OutBuffer = string.Empty;
                                communications.CommandID = 9;
                                communications.Command = GetASCIISendCommand(totalBytesCommand);
                                if (communications.SendCommand())
                                {
                                    communications.TotalReadBytes = 0;
                                    communications.IsDataReceived = false;
                                    communications.CurrentTime = DateTime.Now;
                                    do
                                    {
                                        status = (communications.TotalReadBytes / 512);
                                        if (Convert.ToInt32(status.ToString()) <= Convert.ToInt32(totalBytesCommand))
                                        {
                                           // this.StatusMessage = status.ToString() + " / " + totalBytesCommand + " Packet Read";
                                        }
                                        Application.DoEvents();
                                        if (communications.Timeout())
                                        {
                                            Application.DoEvents();
                                            break;
                                        }
                                    } while (true);
                                    if (Convert.ToInt64(totalBytesCommand) != status)
                                    {
                                        this.StatusMessage = "Communication Fail.";
                                        Application.DoEvents();
                                    }
                                }
                            }
                            strOutput = communications.OutBuffer.Replace("Pkt", "");
                            if (!(strOutput.Contains("End")))
                            {
                                retVal = string.Empty;
                            }
                            strOutput = strOutput.Replace("End", "");
                            retVal = strOutput;
                        }
                    }
                }

            }
            catch (CABException)
            {
                //return string.Empty;
            }
            finally
            {
                communications.ClosePort();
            }
            return retVal;
        }

        //public bool UpdateCMRIRTC()
        //{
        //    IsSignOnFailure = false;
        //    string data = string.Empty;
        //    char charACK;
        //    string sendcmd = "7374727463"; //strtc
        //    string ETX = "03";
        //    string strinput = string.Empty;
        //    bool Flag = false;
        //    try
        //    {
        //        communications.BaudRate = 9600;
        //        if (!communications.OpenPort())
        //        {
        //            this.StatusMessage = MessageConstant.GetText("M000038");
        //        }
        //        else
        //        {
        //            communications.TotalReadBytes = 0;
        //            string fileInput = string.Empty;
        //            communications.ReadFlag = false;
        //            communications.Command = sendcmd;
        //            communications.CommandID = 0;
        //            communications.IsDataReceived = false;
        //            communications.OutBuffer = string.Empty;
        //            communications.CurrentTime = DateTime.Now;
        //            if (communications.SendCommand())
        //            {
        //                do
        //                {
        //                    if (communications.Timeout())
        //                    {
        //                        Flag = false;
        //                        break;
        //                    }
        //                } while (true);

        //                if (communications.OutBuffer.Length <= 0)//Equals(string.Empty))
        //                {
        //                    this.StatusMessage = "TimeOut!";
        //                    Application.DoEvents();
        //                    Flag = false;
        //                }
        //                if (communications.OutBuffer.Length >= 1)
        //                {
        //                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
        //                    if (charACK == 6)
        //                    {
        //                        communications.OutBuffer = string.Empty;
        //                        communications.Command = GetSystemRtc() + ETX;
        //                        if (communications.SendCommand())
        //                        {
        //                            communications.IsDataReceived = false;
        //                            communications.CurrentTime = DateTime.Now;
        //                            //Flag = true;
        //                            do
        //                            {
        //                                if (communications.Timeout())
        //                                {
        //                                    //Flag = true;
        //                                    Application.DoEvents();
        //                                    break;
        //                                }
        //                            } while (true);

        //                            if (communications.OutBuffer.Length <= 0)
        //                            {
        //                                Flag = false;
        //                                Application.DoEvents();
        //                            }
        //                            if (communications.OutBuffer.Length >= 1)
        //                            {
        //                                charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
        //                                if (charACK == 6)
        //                                {
        //                                    Flag = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Flag = false;
        //    }
        //    finally
        //    {
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.ClosePort();
        //        communications.ClosePort();
        //    }
        //    return Flag;
        //}
        public string UpdateCMRIRTC()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            char charACK;
            string sendcmd = "7374727463"; //strtc
            string ETX = "03";
            string strinput = string.Empty;
            string Flag = string.Empty;
            try
            {
                communications.BaudRate = 9600;
                if (!communications.OpenPort())
                {
                    Flag = MessageConstant.GetText("M000038");
                    this.StatusMessage = MessageConstant.GetText("M000038");
                }
                else
                {
                    communications.TotalReadBytes = 0;
                    string fileInput = string.Empty;
                    communications.ReadFlag = false;
                    communications.Command = sendcmd;
                    communications.CommandID = 0;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SendCommand())
                    {
                        do
                        {
                            if (communications.Timeout())
                            {
                                Flag = "Sign-On Failure!";
                                break;
                            }
                        } while (true);

                        if (communications.OutBuffer.Length <= 0)//Equals(string.Empty))
                        {
                            //this.StatusMessage = "TimeOut!";
                            Application.DoEvents();
                            Flag = "Sign-On Failure!";
                        }
                        if (communications.OutBuffer.Length >= 1)
                        {
                            charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                            if (charACK == 6)
                            {
                                communications.OutBuffer = string.Empty;
                                communications.Command = GetSystemRtc() + ETX;
                                if (communications.SendCommand())
                                {
                                    communications.IsDataReceived = false;
                                    communications.CurrentTime = DateTime.Now;
                                    //Flag = true;
                                    do
                                    {
                                        if (communications.Timeout())
                                        {
                                            //Flag = true;
                                            Application.DoEvents();
                                            break;
                                        }
                                    } while (true);

                                    if (communications.OutBuffer.Length <= 0)
                                    {
                                        Flag = "Timeout!";
                                        Application.DoEvents();
                                    }
                                    if (communications.OutBuffer.Length >= 1)
                                    {
                                        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                                        if (charACK == 6)
                                        {
                                            Flag = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Flag = false;
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                communications.ClosePort();
            }
            return Flag;
        }
        public string GetSystemRtc()
        {
            string sysrtc = string.Empty;
            string strrtc = string.Empty;
            /*Day*/
            strrtc = DateTime.Now.Day.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Month*/
            strrtc = DateTime.Now.Month.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Year*/
            strrtc = DateTime.Now.Year.ToString("0000");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(2, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(3, 1)) + 30).ToString());
            /*Hour*/
            strrtc = DateTime.Now.Hour.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Minut*/
            strrtc = DateTime.Now.Minute.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            return sysrtc;
        }
	}
}

			
