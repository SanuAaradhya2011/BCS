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
    public class ReadoutTamperForSingllePhaseIEC : ReadBase
    {
        int commandreTry = 3;
        public ReadoutTamperForSingllePhaseIEC()
        {
            command = Command.GetInstance();
        }

        private string CompartmentCommand()
        {
            string compartmentCommand = string.Empty;
            string hexData = ReadoutCommon.StrToHex("100");
            for (int i = 0; i < 6; i++)
                compartmentCommand = string.Concat(compartmentCommand, hexData);
            return compartmentCommand;
        }
        private bool SignOn()
        {
            bool validSignOn = true;
            if (!communications.SignOnForSPhaseIEC())
            {
                IsSignOnFailure = true;
                communications.DelayExecution();
                if (communications.ComPort.IsOpen)
                    communications.ClosePort();
                validSignOn = false;
            }
            else
            {
                communications.DelayExecution();
                communications.Command = command.ManufactureCommand;
                communications.SendCommand();
                communications.DelayExecution();
                Thread.Sleep(200);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = MessageConstant.GetText("M000039");
                        isCorruptedData = true;
                        validSignOn = false;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                Thread.Sleep(200);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                communications.CommandID = 2;


            }
            if (!validSignOn)
            {
                return false;
            }
            else
            {
                return communications.ReadFlag;
            }

        }

        /// <summary>
        /// This method is used to connect the IEC meter with the client 
        /// </summary>
        /// <returns></returns>
        private bool ConnectToIECMeter()
        {

            try
            {
                Thread.Sleep(1000);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return false;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOnForSPhaseIEC())
                {
                    IsSignOnFailure = true;
                    communications.DelayExecution();
                    communications.ClosePort();
                    return false;
                }
                Thread.Sleep(200);
                communications.TotalReadBytes = 0;
                string passwordData = string.Empty;
                communications.ReadFlag = false;
                communications.Command = command.DTMDailySurveyCommand_A;
                communications.CommandID = 2;
                communications.OutBuffer = string.Empty;
                if (!communications.SendCommand()) return false;

                Thread.Sleep(200);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //Thread.Sleep(400);
                communications.CurrentTime = DateTime.Now;
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase

                do
                {
                    Application.DoEvents();
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        return false;
                    }

                    if (communications.ReadFlag)
                    {
                        Thread.Sleep(200);
                        passwordData = communications.OutBuffer;
                        break;
                    }

                } while (!communications.Timeout());

                if (passwordData == string.Empty) return false;
                string commandPassword = AlgorithemicSignOn(passwordData);
                communications.Command = commandPassword;
                communications.CommandID = 3;
                communications.OutBuffer = "";
                communications.IsDataReceived = false;
                do
                {

                    // communications.DelayExecution();

                    communications.ReadFlag = false;

                    communications.SendCommand();
                    communications.CurrentTime = DateTime.Now;
                    // Thread.Sleep(200);
                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    do
                    {
                        if (communications.ReadFlag) break;
                        Application.DoEvents();
                    } while (!communications.Timeout());
                    if (communications.ReadFlag) break;
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        return false;
                    }
                    Application.DoEvents();
                } while (commandreTry-- > 0);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the meter profile specific data
        /// </summary>
        /// <returns>true/false</returns>
        private bool GetMeterBufferData()
        {
            commandreTry = 3;
            do
            {
                communications.ReadFlag = false;            
                communications.CommandID = 2;
                communications.OutBuffer = "";
                communications.IsDataReceived = false;
                communications.SendCommand();                
                Thread.Sleep(100);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.ReadFlag) 
                    {
                        if (communications.OutBuffer.ToUpper().Contains("ER")) break;
                        Thread.Sleep(100); return true;
                    }
                    if (IsAborted) this.StatusMessage = "User Aborted.";
                    Application.DoEvents();
                } while (!communications.Timeout());
              Application.DoEvents();              
            } while (commandreTry-- > 0);
            return false;
        }
        
        public override string GetData()
        {

            IsSignOnFailure = false;
            string data = string.Empty;
            char[] finalTamperConfigValue = new char[16];
            int length = 0;
            //int lastTamperIndex = 0;            
            string internalData = string.Empty;
            //byte noOfLSAvailable = 90;
            //byte noOFDaysFromSettings = Convert.ToByte(numberOfDays);
            List<string> tamperCommandlist = new List<string>();
            tamperCommandlist.Add(command.FTamperLCommand); // Magnet Tamper
            tamperCommandlist.Add(command.STamperLCommand); // Earth Tamper
            tamperCommandlist.Add(command.SixthTamperLCommand); // Reverse Tamper
            tamperCommandlist.Add(command.TTamperLCommand);  // Single wire Tamper
            tamperCommandlist.Add(command.FourthTamperLCommand); // Neutral Tamper
            tamperCommandlist.Add(command.FifthTamperLCommand); // ESD Tamper
            tamperCommandlist.Add(command.SeventhTamperLCommand); // Low PF Tamper
            tamperCommandlist.Add(command.EightTamperLCommand); // Low Voltage Tamper
            tamperCommandlist.Add(command.NinthTamperLCommand); // Over Load Tamper
            tamperCommandlist.Add(command.TenthTamperLCommand); // Meter Cover open Tamper

            List<string> tamperCommandDataTag = new List<string>();
            tamperCommandDataTag.Add("\x02(D1"); // Magnet Tamper
            tamperCommandDataTag.Add("\x02(E6"); // Earth Tamper
            tamperCommandDataTag.Add("\x02(C6"); // Reverse Tamper
            tamperCommandDataTag.Add("\x02(E7"); // Single wire Tamper
            tamperCommandDataTag.Add("\x02(D2"); // Neutral Tamper
            tamperCommandDataTag.Add("\x02(E8"); // ESD Tamper
            tamperCommandDataTag.Add("\x02(EC"); // Low PF Tamper
            tamperCommandDataTag.Add("\x02(ED"); // Low Voltage Tamper
            tamperCommandDataTag.Add("\x02(EB"); // Over Load Tamper 
            tamperCommandDataTag.Add("\x02(EE"); // Meter Cover open Tamper

            try
            {
                if (!ConnectToIECMeter())
                    return string.Empty;

                //Thread.Sleep(1000);
                //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                //if (ConfigInfo.GetLocalMode().Equals("Optical"))
                //    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                //if (!communications.OpenPort())
                //{
                //    this.StatusMessage = MessageConstant.GetText("M000038");
                //    Application.DoEvents();
                //    return string.Empty;
                //}
                //communications.CurrentTime = DateTime.Now;
                //if (!communications.SignOnForSPhaseIEC())
                //{
                //    IsSignOnFailure = true;
                //    communications.DelayExecution();
                //    communications.ClosePort();
                //    return string.Empty;
                //}
                //else
                //{
                //    communications.DelayExecution();
                //    if (communications.ReadFlag)
                //    {
                //        communications.TotalReadBytes = 0;
                //        string passwordData = string.Empty;
                //        communications.ReadFlag = false;
                //        communications.Command = command.DTMDailySurveyCommand_A;
                //        communications.CommandID = 2;
                //        communications.OutBuffer = string.Empty;
                //        if (!communications.SendCommand())
                //            return "2";
                //        else
                //        {
                //            //Thread.Sleep(200);
                //            //int index = communications.ResponseSignOn.IndexOf("/");
                //            //communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //            //Thread.Sleep(200);

                //            Thread.Sleep(200);
                //            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                //            int index = communications.ResponseSignOn.IndexOf("/");
                //            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //            Thread.Sleep(400);
                //            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase

                //            do
                //            {
                //                if (IsAborted)
                //                {
                //                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                //                    this.StatusMessage = "User Aborted.";
                //                    Application.DoEvents();
                //                    return string.Empty;
                //                }
                //                //else
                //                //{
                //                //    this.StatusMessage = "User Aborted.";
                //                //    Application.DoEvents();
                //                //    return string.Empty;
                //                //}
                //                if (communications.Timeout())
                //                {
                //                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
                //                    return "1";
                //                }

                //                    if (communications.ReadFlag)
                //                    {
                //                        Thread.Sleep(200);
                //                        passwordData = communications.OutBuffer;
                //                        break;
                //                    }

                //            } while (true);

                //            if (passwordData != string.Empty)
                //            {
                //                do
                //                {
                //                    string commandPassword = AlgorithemicSignOn(passwordData);
                //                    // communications.DelayExecution();
                //                    communications.CurrentTime = DateTime.Now;
                //                    communications.ReadFlag = false;
                //                    communications.Command = commandPassword;
                //                    communications.CommandID = 2;
                //                    communications.OutBuffer = "";
                //                    communications.IsDataReceived = false;
                //                    communications.SendCommand();

                //                    // Thread.Sleep(200);
                //                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                //                    do
                //                    {                                         
                //                        if (communications.ReadFlag) break;                                          

                //                    } while (!communications.Timeout());
                //                    if (communications.ReadFlag) break;
                //                    if (IsAborted)
                //                    {
                //                        this.StatusMessage = "User Aborted.";
                //                        Application.DoEvents();
                //                        return string.Empty;
                //                    }
                //                    Application.DoEvents();
                //                } while (commandreTry-- > 0);
                #region Tamper "T" Command

                // communications.DelayExecution();

                //communications.ReadFlag = false;
                //communications.Command = command.TamperTCommand;
                //communications.CommandID = 2;
                //communications.OutBuffer = "";
                //communications.IsDataReceived = false;
                //communications.SendCommand();
                //communications.CurrentTime = DateTime.Now;

                //data = "";
                //Thread.Sleep(100);
                //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                //do
                //{
                //    if (communications.Timeout())
                //    {
                //        this.StatusMessage = MessageConstant.GetText("M000040");
                //        Application.DoEvents();
                //        break;
                //    }
                //    Thread.Sleep(100);
                //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                //    length = communications.OutBuffer.Length;
                //    if (length >= 7)
                //    {
                //        if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                //        {
                Thread.Sleep(200);
                communications.Command = command.TamperTCommand;
                if (!GetMeterBufferData())
                    return string.Empty;
                internalData = string.Empty;
                int byteLength = 0;
                if (communications.OutBuffer.Contains('(') && communications.OutBuffer.Contains(')'))
                    byteLength = communications.OutBuffer.Split('(', ')')[1].Length;
                if (byteLength == 4)
                    internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                else
                    internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 5);

                string tamperConfig = internalData;
                tamperConfig = tamperConfig.PadRight(4, '0');
                finalTamperConfigValue = GetTamperConfigBitsList(tamperConfig);
                // break;
                //        }
                //    }
                //    if (IsAborted)
                //    {
                //        this.StatusMessage = "User Aborted.";
                //        Application.DoEvents();
                //        data = string.Empty;
                //    }
                //} while (true);

                #endregion

                if (finalTamperConfigValue != null)
                {
                    //lastTamperIndex = Array.LastIndexOf(finalTamperConfigValue, '1');

                    #region All Tamper readout

                    int supportedTamperCounts = 0;
                    while (supportedTamperCounts < finalTamperConfigValue.Length)
                    {
                        if (finalTamperConfigValue[supportedTamperCounts] == '0')
                        { 
                            supportedTamperCounts++; 
                            continue; 
                        }
                        bool isTamperLastEvent = false;
                        communications.Command = tamperCommandlist[supportedTamperCounts];

                        while (!isTamperLastEvent)
                        {
                            if (!GetMeterBufferData()) return string.Empty;

                            internalData = string.Empty;
                            // handeled EOT Issue when BCC coming as 04
                           // if (communications.OutBuffer.Contains("\x04") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1)))
                            if (communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1).Contains("\x04"))
                            {
                                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                                internalData = tamperCommandDataTag[supportedTamperCounts] + internalData;
                                data = data + internalData;
                               
                            }
                            else
                            {
                                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                                internalData = tamperCommandDataTag[supportedTamperCounts] + internalData + "\x04";
                                data = data + internalData;
                                isTamperLastEvent = true;
                            }
                            communications.Command = "06";
                            Application.DoEvents();
                            Application.DoEvents();
                        }
                        supportedTamperCounts++;
                        Thread.Sleep(100);
                        Application.DoEvents();
                             

                    }
                    //if (finalTamperConfigValue[0] == '1') // Magnet Tamper
                    //{
                    //    communications.Command = command.FTamperLCommand;
                    //    if (!GetMeterBufferData())
                    //        return string.Empty;

                        //communications.DelayExecution();
                        //communications.CurrentTime = DateTime.Now;
                        //communications.ReadFlag = false;
                        //communications.Command = command.FTamperLCommand;
                        //communications.CommandID = 2;
                        //communications.OutBuffer = "";
                        //communications.IsDataReceived = false;
                        //communications.SendCommand();
                        //Thread.Sleep(100);
                        //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                        //do
                        //{
                        //    if (communications.Timeout())
                        //    {
                        //        this.StatusMessage = MessageConstant.GetText("M000040");
                        //        Application.DoEvents();
                        //        break;
                        //    }
                        //    Thread.Sleep(100);
                        //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                        //    length = communications.OutBuffer.Length;
                        //    if (length >= 81)
                        //    {
                        //        //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                        //        if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                        //        {
                        //            if (communications.OutBuffer.Contains("\x04"))
                        //            {

                        //            }
                        //            else
                        //            {
                        //                internalData = string.Empty;
                        //                if (lastTamperIndex == 0)
                        //                {
                        //                    internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                        //                    internalData = "\x02(D1" + internalData;
                        //                    data = data + internalData;
                        //                }
                        //                else
                        //                {
                        //                    internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                        //                    internalData = "\x02(D1" + internalData + "\x04";
                        //                    data = data + internalData;
                        //                }
                        //                break;
                        //            }
                        //        }
                        //        //else if ( communications.OutBuffer.Contains("\x04"))
                        //        if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                        //        {
                        //            internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                        //            internalData = "\x02(D1" + internalData;
                        //            data = data + internalData;
                        //            communications.DelayExecution();
                        //            communications.CurrentTime = DateTime.Now;
                        //            communications.ReadFlag = false;
                        //            communications.Command = "06";
                        //            communications.CommandID = 2;
                        //            communications.OutBuffer = "";
                        //            communications.IsDataReceived = false;
                        //            communications.SendCommand();
                        //            Thread.Sleep(100);
                        //            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                        //        }
                        //    }
                        //    if (IsAborted)
                        //    {
                        //        this.StatusMessage = "User Aborted.";
                        //        Application.DoEvents();
                        //        data = string.Empty;
                        //        break; // Story - 354414 - Missing break for abort
                        //    }
                        //} while (true);
                    //}

                    #endregion

                    //#region Earth Command
                    //if (finalTamperConfigValue[1] == '1') // Earth Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.STamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 1)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(E6" + internalData;// +"\x03";
                    //                        data = data + internalData;//
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(E6" + internalData + "\x04";
                    //                        data = data + internalData;// 
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(E6" + internalData;
                    //                data = data + internalData;// 
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Single wire Command
                    //if (finalTamperConfigValue[3] == '1') // Single wire Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.TTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 3)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(E7" + internalData;// +"\x03";
                    //                        data = data + internalData;//
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(E7" + internalData + "\x04";
                    //                        data = data + internalData;// 
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(E7" + internalData;
                    //                data = data + internalData;// 
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Neutral Disturbance Command
                    //if (finalTamperConfigValue[4] == '1') // Neutral Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.FourthTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 4)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(D2" + internalData;// +"\x03";
                    //                        data = data + internalData;//
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(D2" + internalData + "\x04"; // +"\x03";
                    //                        data = data + internalData;// 
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(D2" + internalData;
                    //                data = data + internalData;
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region ESD Command
                    //if (finalTamperConfigValue[5] == '1') // ESD Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.FifthTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 5)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(E8" + internalData;// +"\x03";
                    //                        data = data + internalData;//
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(E8" + internalData + "\x04"; // +"\x03"; /*Added ESD Tamper "E8" code command  */
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(E8" + internalData; /*Added ESD Tamper "E8" command for Fifth command  */
                    //                data = data + internalData;
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Reverse Command
                    //if (finalTamperConfigValue[2] == '1') // Reverse Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.SixthTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 2)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(C6" + internalData;
                    //                        data = data + internalData;
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(C6" + internalData + "\x04";
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(C6" + internalData; /*Added Reverse Tamper "C6" command */
                    //                data = data + internalData;
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Low PF Command
                    //if (finalTamperConfigValue[6] == '1') // Low PF Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.SeventhTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 6)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(EC" + internalData;
                    //                        data = data + internalData;
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(EC" + internalData + "\x04";
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(EC" + internalData; /*Added Low PF Tamper "EC" command */
                    //                data = data + internalData;// 
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Low Voltage Command
                    //if (finalTamperConfigValue[7] == '1') // Low Voltage Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.EightTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 7)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(ED" + internalData;
                    //                        data = data + internalData;
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(ED" + internalData + "\x04";
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(ED" + internalData; /*Added Low Voltage Tamper "ED" command */
                    //                data = data + internalData;// 
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Over Load Command
                    //if (finalTamperConfigValue[8] == '1') // Over Load Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.NinthTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    //data = "";
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 8)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(EB" + internalData;
                    //                        data = data + internalData;
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(EB" + internalData + "\x04";
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(EB" + internalData; /*Added Over Load Tamper "EB" command */
                    //                data = data + internalData;
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion

                    //#region Meter Cover open Command
                    //if (finalTamperConfigValue[9] == '1') // Meter Cover open Tamper
                    //{
                    //    communications.DelayExecution();
                    //    communications.CurrentTime = DateTime.Now;
                    //    communications.ReadFlag = false;
                    //    communications.Command = command.TenthTamperLCommand;
                    //    communications.CommandID = 2;
                    //    communications.OutBuffer = "";
                    //    communications.IsDataReceived = false;
                    //    communications.SendCommand();
                    //    //data = "";
                    //    Thread.Sleep(100);
                    //    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //    do
                    //    {
                    //        if (communications.Timeout())
                    //        {
                    //            this.StatusMessage = MessageConstant.GetText("M000040");
                    //            Application.DoEvents();
                    //            break;
                    //        }
                    //        Thread.Sleep(100);
                    //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //        length = communications.OutBuffer.Length;
                    //        if (length >= 81)
                    //        {
                    //            //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                    //            {
                    //                if (communications.OutBuffer.Contains("\x04"))
                    //                {

                    //                }
                    //                else
                    //                {
                    //                    internalData = string.Empty;
                    //                    if (lastTamperIndex == 9)
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                        internalData = "\x02(EE" + internalData;
                    //                        data = data + internalData;
                    //                    }
                    //                    else
                    //                    {
                    //                        internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
                    //                        internalData = "\x02(EE" + internalData + "\x04";
                    //                        data = data + internalData;
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
                    //            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                    //            {
                    //                internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
                    //                internalData = "\x02(EE" + internalData; /*Added Cover Open Tamper "EE" command */
                    //                data = data + internalData;
                    //                communications.DelayExecution();
                    //                communications.CurrentTime = DateTime.Now;
                    //                communications.ReadFlag = false;
                    //                communications.Command = "06";
                    //                communications.CommandID = 2;
                    //                communications.OutBuffer = "";
                    //                communications.IsDataReceived = false;
                    //                communications.SendCommand();
                    //                Thread.Sleep(100);
                    //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                    //            }
                    //        }
                    //        if (IsAborted)
                    //        {
                    //            this.StatusMessage = "User Aborted.";
                    //            Application.DoEvents();
                    //            data = string.Empty;
                    //            break; // Story - 354414 - Missing break for abort
                    //        }
                    //    } while (true);
                    //}

                    //#endregion
                }
                else
                {
                    this.StatusMessage = "Tamper Not Supported.";
                    Application.DoEvents();
                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                //communications.Command = command.BreakCommand;
                //communications.SendCommand();
                //communications.DelayExecution();
                //communications.Command = command.BreakCommand;
                //communications.SendCommand();
                //communications.DelayExecution();
                communications.ClosePort();
            }
            return data + "\x03"; ;
        }


        //public override string GetData()
        //{

        //    IsSignOnFailure = false;
        //    string data = string.Empty;
        //    int length = 0;
        //    //byte noOfLSAvailable = 90;
        //    //byte noOFDaysFromSettings = Convert.ToByte(numberOfDays);
        //    try
        //    {
        //        Thread.Sleep(1000);
        //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //        if (ConfigInfo.GetLocalMode().Equals("Optical"))
        //            communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
        //        if (!communications.OpenPort())
        //        {
        //            this.StatusMessage = MessageConstant.GetText("M000038");
        //            Application.DoEvents();
        //            return string.Empty;
        //        }
        //        communications.CurrentTime = DateTime.Now;
        //        if (!communications.SignOnForSPhaseIEC())
        //        {
        //            IsSignOnFailure = true;
        //            communications.DelayExecution();
        //            communications.ClosePort();
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            communications.DelayExecution();
        //            if (communications.ReadFlag)
        //            {
        //                communications.TotalReadBytes = 0;
        //                string passwordData = string.Empty;
        //                communications.ReadFlag = false;
        //                communications.Command = command.DTMDailySurveyCommand_A;
        //                communications.CommandID = 2;
        //                communications.OutBuffer = string.Empty;
        //                if (!communications.SendCommand())
        //                    return "2";
        //                else
        //                {
        //                    //Thread.Sleep(200);
        //                    //int index = communications.ResponseSignOn.IndexOf("/");
        //                    //communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
        //                    //Thread.Sleep(200);

        //                    Thread.Sleep(200);
        //                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                    int index = communications.ResponseSignOn.IndexOf("/");
        //                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
        //                    Thread.Sleep(400);
        //                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase

        //                    do
        //                    {
        //                        if (!IsAborted)
        //                        {
        //                            // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
        //                            Application.DoEvents();
        //                        }
        //                        else
        //                        {
        //                            this.StatusMessage = "User Aborted.";
        //                            Application.DoEvents();
        //                            return string.Empty;
        //                        }
        //                        if (communications.Timeout())
        //                        {
        //                            this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
        //                            return "1";
        //                        }
        //                        else
        //                        {
        //                            if (communications.ReadFlag)
        //                            {
        //                                Thread.Sleep(200);
        //                                passwordData = communications.OutBuffer;
        //                                break;
        //                            }
        //                        }
        //                    } while (true);

        //                    if (passwordData != string.Empty)
        //                    {
        //                        string commandPassword = AlgorithemicSignOn(passwordData);
        //                        communications.DelayExecution();
        //                        communications.CurrentTime = DateTime.Now;
        //                        communications.ReadFlag = false;
        //                        communications.Command = commandPassword;
        //                        communications.CommandID = 2;
        //                        communications.OutBuffer = "";
        //                        communications.IsDataReceived = false;
        //                        communications.SendCommand();

        //                        Thread.Sleep(200);
        //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        do
        //                        {
        //                            if (communications.Timeout())
        //                            {
        //                                break;
        //                            }
        //                            if (communications.ReadFlag)
        //                                break;
        //                            if (IsAborted)
        //                            {
        //                                this.StatusMessage = "User Aborted.";
        //                                Application.DoEvents();
        //                                return string.Empty;
        //                            }
        //                        } while (true);


        //                        #region TCommand

        //                        //communications.DelayExecution();
        //                        //communications.CurrentTime = DateTime.Now;
        //                        //communications.ReadFlag = false;
        //                        //communications.Command = command.TamperTCommand;
        //                        //communications.CommandID = 2;
        //                        //communications.OutBuffer = "";
        //                        //communications.IsDataReceived = false;
        //                        //communications.SendCommand();
        //                        //data = "";
        //                        //Thread.Sleep(100);
        //                        //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        //do
        //                        //{
        //                        //    if (communications.Timeout())
        //                        //    {
        //                        //        this.StatusMessage = MessageConstant.GetText("M000040");
        //                        //        Application.DoEvents();
        //                        //        break;
        //                        //    }
        //                        //    //if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
        //                        //    if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
        //                        //    {
        //                        //        data = data + communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 2);
        //                        //        data = data + "\x04";
        //                        //        break;
        //                        //    }
        //                        //    if (IsAborted)
        //                        //    {
        //                        //        this.StatusMessage = "User Aborted.";
        //                        //        Application.DoEvents();
        //                        //        data = string.Empty;
        //                        //    }
        //                        //} while (true);

        //                        #endregion

        //                        #region FirstLCommand

        //                        communications.DelayExecution();
        //                        communications.CurrentTime = DateTime.Now;
        //                        communications.ReadFlag = false;
        //                        communications.Command = command.FTamperLCommand;
        //                        communications.CommandID = 2;
        //                        communications.OutBuffer = "";
        //                        communications.IsDataReceived = false;
        //                        communications.SendCommand();
        //                        //data = "";
        //                        Thread.Sleep(100);
        //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        do
        //                        {
        //                            if (communications.Timeout())
        //                            {
        //                                this.StatusMessage = MessageConstant.GetText("M000040");
        //                                Application.DoEvents();
        //                                break;
        //                            }
        //                            Thread.Sleep(100);
        //                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                            length = communications.OutBuffer.Length;
        //                            if (length >= 81)
        //                            {
        //                                //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
        //                                {
        //                                    if (communications.OutBuffer.Contains("\x04"))
        //                                    {

        //                                    }
        //                                    else
        //                                    {
        //                                        string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
        //                                        internalData = "\x02(D1" + internalData + "\x04";
        //                                        data = data + internalData;// 
        //                                        break;
        //                                    }
        //                                }
        //                                //else if ( communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
        //                                {
        //                                    string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
        //                                    internalData = "\x02(D1" + internalData;
        //                                    data = data + internalData;// 
        //                                    communications.DelayExecution();
        //                                    communications.CurrentTime = DateTime.Now;
        //                                    communications.ReadFlag = false;
        //                                    communications.Command = "06";
        //                                    communications.CommandID = 2;
        //                                    communications.OutBuffer = "";
        //                                    communications.IsDataReceived = false;
        //                                    communications.SendCommand();
        //                                    Thread.Sleep(100);
        //                                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                                }
        //                            }
        //                            if (IsAborted)
        //                            {
        //                                this.StatusMessage = "User Aborted.";
        //                                Application.DoEvents();
        //                                data = string.Empty;
        //                                break; // Story - 354414 - Missing break for abort
        //                            }
        //                        } while (true);

        //                        #endregion

        //                        #region SecondLCommand

        //                        communications.DelayExecution();
        //                        communications.CurrentTime = DateTime.Now;
        //                        communications.ReadFlag = false;
        //                        communications.Command = command.STamperLCommand;
        //                        communications.CommandID = 2;
        //                        communications.OutBuffer = "";
        //                        communications.IsDataReceived = false;
        //                        communications.SendCommand();
        //                        //data = "";
        //                        Thread.Sleep(100);
        //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        do
        //                        {
        //                            if (communications.Timeout())
        //                            {
        //                                this.StatusMessage = MessageConstant.GetText("M000040");
        //                                Application.DoEvents();
        //                                break;
        //                            }
        //                            Thread.Sleep(100);
        //                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                            length = communications.OutBuffer.Length;
        //                            if (length >= 81)
        //                            {
        //                                //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
        //                                {
        //                                    if (communications.OutBuffer.Contains("\x04"))
        //                                    {

        //                                    }
        //                                    else
        //                                    {
        //                                        string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
        //                                        internalData = "\x02(E6" + internalData + "\x04";
        //                                        data = data + internalData;// 
        //                                        break;
        //                                    }
        //                                }
        //                                //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
        //                                {
        //                                    string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
        //                                    internalData = "\x02(E6" + internalData;
        //                                    data = data + internalData;// 
        //                                    communications.DelayExecution();
        //                                    communications.CurrentTime = DateTime.Now;
        //                                    communications.ReadFlag = false;
        //                                    communications.Command = "06";
        //                                    communications.CommandID = 2;
        //                                    communications.OutBuffer = "";
        //                                    communications.IsDataReceived = false;
        //                                    communications.SendCommand();
        //                                    Thread.Sleep(100);
        //                                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                                }
        //                            }
        //                            if (IsAborted)
        //                            {
        //                                this.StatusMessage = "User Aborted.";
        //                                Application.DoEvents();
        //                                data = string.Empty;
        //                                break; // Story - 354414 - Missing break for abort
        //                            }
        //                        } while (true);

        //                        #endregion

        //                        #region ThirdLCommand

        //                        communications.DelayExecution();
        //                        communications.CurrentTime = DateTime.Now;
        //                        communications.ReadFlag = false;
        //                        communications.Command = command.TTamperLCommand;
        //                        communications.CommandID = 2;
        //                        communications.OutBuffer = "";
        //                        communications.IsDataReceived = false;
        //                        communications.SendCommand();
        //                        //data = "";
        //                        Thread.Sleep(100);
        //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        do
        //                        {
        //                            if (communications.Timeout())
        //                            {
        //                                this.StatusMessage = MessageConstant.GetText("M000040");
        //                                Application.DoEvents();
        //                                break;
        //                            }
        //                            Thread.Sleep(100);
        //                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                            length = communications.OutBuffer.Length;
        //                            if (length >= 81)
        //                            {
        //                                //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
        //                                {
        //                                    if (communications.OutBuffer.Contains("\x04"))
        //                                    {

        //                                    }
        //                                    else
        //                                    {
        //                                        string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 4);
        //                                        internalData = "\x02(E7" + internalData + "\x04";
        //                                        data = data + internalData;// 
        //                                        break;
        //                                    }
        //                                }
        //                                //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
        //                                {
        //                                    string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
        //                                    internalData = "\x02(E7" + internalData;
        //                                    data = data + internalData;// 
        //                                    communications.DelayExecution();
        //                                    communications.CurrentTime = DateTime.Now;
        //                                    communications.ReadFlag = false;
        //                                    communications.Command = "06";
        //                                    communications.CommandID = 2;
        //                                    communications.OutBuffer = "";
        //                                    communications.IsDataReceived = false;
        //                                    communications.SendCommand();
        //                                    Thread.Sleep(100);
        //                                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                                }
        //                            }
        //                            if (IsAborted)
        //                            {
        //                                this.StatusMessage = "User Aborted.";
        //                                Application.DoEvents();
        //                                data = string.Empty;
        //                                break; // Story - 354414 - Missing break for abort
        //                            }
        //                        } while (true);

        //                        #endregion

        //                        #region FourthLCommand

        //                        communications.DelayExecution();
        //                        communications.CurrentTime = DateTime.Now;
        //                        communications.ReadFlag = false;
        //                        communications.Command = command.FourthTamperLCommand;
        //                        communications.CommandID = 2;
        //                        communications.OutBuffer = "";
        //                        communications.IsDataReceived = false;
        //                        communications.SendCommand();
        //                        //data = "";
        //                        Thread.Sleep(100);
        //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                        do
        //                        {
        //                            if (communications.Timeout())
        //                            {
        //                                this.StatusMessage = MessageConstant.GetText("M000040");
        //                                Application.DoEvents();
        //                                break;
        //                            }
        //                            Thread.Sleep(100);
        //                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                            length = communications.OutBuffer.Length;
        //                            if (length >= 81)
        //                            {
        //                                //if (communications.OutBuffer.Contains("\x03") && !communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
        //                                {
        //                                    if (communications.OutBuffer.Contains("\x04"))
        //                                    {

        //                                    }
        //                                    else
        //                                    {
        //                                        string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
        //                                        internalData = "\x02(D2" + internalData;// +"\x03";
        //                                        data = data + internalData;// 
        //                                        break;
        //                                    }
        //                                }
        //                                //else if (length >= 80 && communications.OutBuffer.Contains("\x04"))
        //                                if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
        //                                {
        //                                    string internalData = communications.OutBuffer.Substring(2, communications.OutBuffer.Length - 3);
        //                                    internalData = "\x02(D2" + internalData;
        //                                    data = data + internalData;// 
        //                                    communications.DelayExecution();
        //                                    communications.CurrentTime = DateTime.Now;
        //                                    communications.ReadFlag = false;
        //                                    communications.Command = "06";
        //                                    communications.CommandID = 2;
        //                                    communications.OutBuffer = "";
        //                                    communications.IsDataReceived = false;
        //                                    communications.SendCommand();
        //                                    Thread.Sleep(100);
        //                                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
        //                                }
        //                            }
        //                            if (IsAborted)
        //                            {
        //                                this.StatusMessage = "User Aborted.";
        //                                Application.DoEvents();
        //                                data = string.Empty;
        //                                break; // Story - 354414 - Missing break for abort
        //                            }
        //                        } while (true);

        //                        #endregion
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.ClosePort();
        //    }
        //    return data;
        //}

        /// <summary>
        /// This method convert the Tamper command Hex value to its corresponding bit value
        /// </summary>
        /// <param name="TempTamperConfigurationBytes">Command string Hex value</param>
        /// <returns>Char aaray of Bit value</returns>
        public char[] GetTamperConfigBitsList(string TempTamperConfigurationBytes)
        {
            string binConfigByte1 = Convert.ToString(Convert.ToInt32(TempTamperConfigurationBytes.Substring(0, 2), 16), 2).PadLeft(8, '0');
            string binConfigByte2 = Convert.ToString(Convert.ToInt32(TempTamperConfigurationBytes.Substring(2, 2), 16), 2).PadLeft(8, '0');
            string bitConfig = binConfigByte2.Substring(4, 1) + binConfigByte2.Substring(5, 1) + binConfigByte2.Substring(6, 1) + binConfigByte2.Substring(7, 1) + binConfigByte1.Substring(0, 6); //----CaseOpen 3's + Overload 2's + LowVoltage 1's + Low PF 0's (1st Bit + 2nd Bit + 3rd bit + 4th bit of 2nd Byte) +  (6 bit of 1st Byte)
            char[] TamperBitsList = bitConfig.ToCharArray();
            Array.Reverse(TamperBitsList);
            return TamperBitsList;
        }


        public string AlgorithemicSignOn(string strSeed)
        {
            try
            {
                string[] strdata = strSeed.Split('(');
                string passdata = strdata[1].Substring(0, strdata[1].IndexOf(')'));
                string CalculatedSeed = TeaAlgorithm(passdata);
                uint ass = uint.Parse(CalculatedSeed);
                string Hexcmd = string.Format("{0:X}", ass);
                while (Hexcmd.Length < 8) Hexcmd = "0" + Hexcmd;
                Hexcmd = StrToHex(Hexcmd);
                passdata = "50320228" + Hexcmd + "2903";
                passdata += ReadoutCommon.ReturnBcc(passdata);
                passdata = "01" + passdata;
                return passdata;
            }
            catch (Exception)
            {
                return null;
            }
        }
        ///////////////////////////////// Mohsin Raza *******************************

        /******************************************************************************
    *  Function Name   : TeaAlgorithm()
    *  Description     : TeaAlgorithm is used for creating the algorithemic Password .  
    *                    Command based on command recevied from Meter               
    *******************************************************************************/
        public string TeaAlgorithm(string strseed)
        {
            uint y = 0;
            try
            {
                //strseed = "8C1FE0DA";
                /* this key is specific to the CCR meter */
                uint seed = Convert.ToUInt32(strseed, 16);//0xC903CA6C;// 
                uint[] Key = { 0x9c178e52, 0x1ec2a690, 0xfb34508d, 0x359e2697 };    //Algo Standard value
                uint Delta = 0x9e3779b9;
                uint z;
                uint Sum = 0;
                uint n = 32;
                //char n = Convert.ToChar(32);  /* a key schedule constant */
                uint[] EncryptData = new uint[2];
                y = seed;
                z = 100 - seed;

                while (n-- > 0)
                {
                    /* basic cycle start */
                    y += ((z << 4) ^ (z >> 5)) + z ^ Sum + Key[Sum & 3];
                    Sum += Delta;
                    z += ((y << 4) ^ (y >> 5)) + y ^ Sum + Key[(Sum >> 11) & 3];
                    /* end cycle */
                }
                EncryptData[0] = y;
                EncryptData[1] = z;

            }
            catch (Exception)
            {
                return "";

            }
            return y.ToString();
        }
        public string StrToHex(string GetStr)
        {
            string tempstr = "";
            try
            {
                int indecount = 0;
                char AsciiCh;
                int chrascii;
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                while (indecount < GetStr.Length)
                {
                    AsciiCh = Convert.ToChar(GetStr.Substring(indecount, 1));
                    if ((AsciiCh >= 48) && AsciiCh <= 57)
                    {
                        chrascii = (Convert.ToInt16(AsciiCh) - 48) + 30;
                        tempstr += chrascii.ToString();
                    }
                    else
                    {
                        if (AsciiCh != 32)
                        {
                            chrascii = Convert.ToInt16(AsciiCh);
                            AsciiCh = _hexChars[chrascii / 16];
                            tempstr += (_hexChars[chrascii / 16]).ToString() + (_hexChars[chrascii % 16]).ToString();
                        }
                        else
                        {
                            tempstr += "20";       //Space
                        }
                    }
                    indecount++;
                }
            }
            catch (Exception)
            {
                return "";
            }
            return tempstr;
        }
        public override string GetInstantData()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return data;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    communications.OutBuffer = string.Empty;
                    communications.Command = command.TamperStatusManfCommand;
                    communications.SendCommand();
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    //Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            isCorruptedData = true;
                            Application.DoEvents();
                            return data;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));

                    string commandPassword = command.TamperStatusCommand;
                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace("Bcc", ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = string.Empty;

                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    communications.CurrentTime = DateTime.Now;
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            break;
                        }
                    } while (communications.OutBuffer.Length < 9);

                    if (communications.OutBuffer.Length >= 9)
                    {
                        communications.DelayExecution();
                        data = communications.OutBuffer;
                    }
                }
                else
                {
                    IsSignOnFailure = true;
                    data = null;
                }
            }
            catch (Exception)
            {
                data = null;
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
