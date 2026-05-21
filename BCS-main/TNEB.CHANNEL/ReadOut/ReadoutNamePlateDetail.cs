using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using CAB.UI.Controls;
using System.Windows.Forms;

namespace CAB.IECChannel.ReadOut
{
    public class ReadoutNamePlateDetail : ReadBase
    {
        public ReadoutNamePlateDetail()
        {
            command = Command.GetInstance();
        }

        public override string GetData()
        {
            IsSignOnFailure = false;
            isCorruptedData = false;
            string fileInput = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                }
                else
                {
                    communications.DelayExecution();
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
                    {
                        communications.DelayExecution();
                        communications.Command = "063035360D0A";
                        communications.SendCommand();
                        communications.DelayExecution();
                        communications.DelayExecution();
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
                        communications.Command = "3A31313131313131314D6E190D0A";
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        communications.IsDataReceived = false;
                        communications.ReadFlag = false;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        do
                        {
                            if (communications.Timeout())
                            {
                                this.StatusMessage = MessageConstant.GetText("M000041");
                                Application.DoEvents();
                                break;
                            }

                            if (IsAborted)
                            {
                                this.StatusMessage = "User Aborted.";
                                Application.DoEvents();
                                return string.Empty;
                            }

                        } while (communications.OutBuffer.Length < 28);
                        if (communications.ReadFlag)
                        {
                            fileInput = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(fileInput.Substring(1), fileInput.Length - 3, fileInput.Substring(fileInput.Length - 1, 1)))
                                fileInput = string.Empty;
                            fileInput = string.Concat(Convert.ToChar(1), "NP", communications.ResponseSignOn.Replace("\r\n", string.Empty), "/", this.ReadingDateTime, fileInput, Convert.ToChar(4));

                        }
                    }
                    else
                        IsSignOnFailure = true;
                }

                # region oldImplementation

                //if (ConfigInfo.GetLocalMode().Equals("Optical"))
                //    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                //if (!communications.OpenPort())
                //{
                //    this.StatusMessage = MessageConstant.GetText("M000038");
                //    Application.DoEvents();
                //    return string.Empty;
                //}

                //communications.Command = command.BreakCommand;
                //communications.SendCommand();
                //communications.DelayExecution();
                //communications.CurrentTime = DateTime.Now;
                //if (!communications.SignOn())
                //{
                //    IsSignOnFailure = true;
                //    communications.DelayExecution();
                //    if (communications.ComPort.IsOpen)
                //        communications.ClosePort();
                //    return string.Empty;
                //}
                //if (communications.ReadFlag)
                //{
                //    communications.DelayExecution();
                //    if (communications.ResponseSignOn != string.Empty)
                //    {
                //        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                //        {
                //            this.StatusMessage = MessageConstant.GetText("M000039");
                //            IsSignOnFailure = true;
                //            return null;
                //        }
                //    }
                //    int index = communications.ResponseSignOn.IndexOf("/");
                //    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //    Thread.Sleep(200);
                //    this.StatusMessage = "Reading Name Plate Details";

                //    communications.TotalReadBytes = 0;
                //    communications.ReadFlag = false;
                //    communications.Command = "063035360D0A";
                //    communications.OutBuffer = string.Empty;
                //    communications.CurrentTime = DateTime.Now;
                //    communications.IsDataReceived = false;
                //    communications.SendCommand();
                //    communications.DelayExecution();
                //    communications.DelayExecution();

                //    communications.TotalReadBytes = 0;
                //    communications.ReadFlag = false;
                //    communications.Command = "3A31313131313131314D6E190D0A";
                //    communications.CommandID = 2;
                //    communications.OutBuffer = string.Empty;
                //    communications.IsDataReceived = false;
                //    communications.SendCommand();
                //    communications.DelayExecution();
                //    communications.DelayExecution();
                //    communications.DelayExecution();
                //    communications.DelayExecution();
                //    communications.CurrentTime = DateTime.Now;
                //    do
                //    {
                //        if (!IsAborted)
                //        {
                //            //this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                //            //Application.DoEvents();
                //        }
                //        if (communications.Timeout())
                //        {
                //            this.StatusMessage = "Time Out!";
                //            Application.DoEvents();
                //            return "";
                //        }
                //    } while (communications.TotalReadBytes < 28);
                //    if (communications.ReadFlag)
                //    {
                //        fileInput = communications.OutBuffer;
                //        bool chkreadbcc = ReadoutCommon.CalculateBcc(fileInput.Substring(1), fileInput.Length - 3, fileInput.Substring(fileInput.Length - 1, 1));
                //        if (!chkreadbcc)
                //            return "";
                //        fileInput = string.Concat(Convert.ToChar(1), "NP", communications.ResponseSignOn.Replace("\r\n", string.Empty), "/", this.ReadingDateTime, fileInput, Convert.ToChar(4));

                //    }
                //}

                # endregion
            }
            catch (Exception Ex)
            {
                //this.StatusMessage = Ex.ToString();
                return "2";
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

