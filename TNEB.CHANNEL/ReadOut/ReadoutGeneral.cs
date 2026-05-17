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
    public class ReadoutGeneral : ReadBase
    {
        public ReadoutGeneral()
        {
            command = Command.GetInstance();
        }

        public override string GetData()
        {
            IsSignOnFailure = false;
            isCorruptedData = false;
            string meterID = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    IsSignOnFailure = true;
                    communications.DelayExecution();
                    communications.ClosePort();
                    return string.Empty;
                }
                else
                {
                    communications.DelayExecution();
                    if (communications.ReadFlag)
                    {
                        communications.TotalReadBytes = 0;
                        string fileInput = string.Empty;
                        communications.ReadFlag = false;
                        communications.Command = command.ReadoutCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        if (!communications.SendCommand())
                            return "2";
                        else
                        {
                            Thread.Sleep(200);
                            if (communications.ResponseSignOn != string.Empty)
                            {
                                //if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                                //{
                                //    this.StatusMessage = MessageConstant.GetText("M000039");
                                //    Application.DoEvents();
                                //    isCorruptedData = true;
                                //    return "1"; 
                                //}
                            }

                            int index = communications.ResponseSignOn.IndexOf("/");
                            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                            Thread.Sleep(200);
                            do
                            {
                                if (!IsAborted)
                                {
                                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                                    Application.DoEvents();
                                }
                                else
                                {
                                    this.StatusMessage = "User Aborted.";
                                    Application.DoEvents();
                                    return string.Empty;
                                }
                                if (communications.Timeout())
                                {
                                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
                                    return "1";
                                }
                                else
                                {
                                    if (communications.ReadFlag)
                                    {
                                        Thread.Sleep(200);
                                        fileInput = communications.OutBuffer;
                                        bool chkreadbcc = ReadoutCommon.CalculateBcc(fileInput, fileInput.Length - 3, fileInput.Substring(fileInput.Length - 1, 1));
                                        if (!chkreadbcc)
                                            return "3";

                                        fileInput = string.Concat(Convert.ToChar(1), "RD", MeterID(communications.ResponseSignOn), "/", this.ReadingDateTime, "/", fileInput, Convert.ToChar(4));

                                        break;
                                    }
                                }
                            } while (true);
                            return fileInput;
                        }
                    }
                }
                return "2";
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
        }
    }
}
