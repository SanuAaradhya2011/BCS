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
    public class ReadoutDTMDailyProfileForSingllePhaseIEC : ReadBase
    {
        int commandreTry = 3;
        bool isTNEB = false;
        public ReadoutDTMDailyProfileForSingllePhaseIEC(bool ISTNEB)
        {
            this.isTNEB = ISTNEB;
        }
 

        #region ChangeWithCommand

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
                    commandreTry--;
                } while (commandreTry > 0);

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
                communications.CurrentTime = DateTime.Now;
                Thread.Sleep(100);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
                do
                {
                    if (communications.ReadFlag)
                    {
                        if (communications.OutBuffer.ToUpper().Contains("ER30")) return true;
                        else if (communications.OutBuffer.ToUpper().Contains("ER")) break;

                        Thread.Sleep(100); return true;
                    }
                    if (IsAborted) this.StatusMessage = "User Aborted.";
                    Application.DoEvents();
                } while (!communications.Timeout());
                Application.DoEvents();
            } while (commandreTry-- > 0);
            return false;
        }

        public override string GetDTMParameterData()
        {
            IsSignOnFailure = false;
            string data = string.Empty;
          
            try
            {
                if (!ConnectToIECMeter()) return string.Empty;
                Thread.Sleep(200);
                communications.Command = command.DTMDailySurveyCommand_B;
                bool isDPLastEvent = false;
                while (!isDPLastEvent)
                {                  
                    if (!GetMeterBufferData()) return string.Empty;
                    string internalData = string.Empty;
                    // handeled ETX Issue when BCC coming as 03
                    //if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1))) isDPLastEvent = true;
                    if (communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1).Contains("\x03")) isDPLastEvent = true;

                    //Bug 504948: Removing Last character BCC from the outbuffer data for correct file creation beacuse last character is not of use in file parsing
                    data += communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1);

                    //data += communications.OutBuffer;
                    communications.Command = "06";
                    Application.DoEvents();
                    Application.DoEvents();
                }

                Thread.Sleep(100);
                Application.DoEvents();
                //Thread.Sleep(1000);
                //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                // if (ConfigInfo.GetLocalMode().Equals("Optical"))
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
                //            // Mohsin 19-10-15
                //           //  Thread.Sleep(200);
                //           // int index = communications.ResponseSignOn.IndexOf("/");
                //           // communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //            Thread.Sleep(200);
                //            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                //            int index = communications.ResponseSignOn.IndexOf("/");
                //            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                //            Thread.Sleep(400);
                //            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase

                //            do
                //            {
                //                if (!IsAborted)
                //                {
                //                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                //                    //Application.DoEvents();
                //                }
                //                else
                //                {
                //                    this.StatusMessage = "User Aborted.";
                //                    Application.DoEvents();
                //                    return string.Empty;
                //                }
                //                if (communications.Timeout())
                //                {
                //                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
                //                    Application.DoEvents();
                //                    return "1";
                //                }
                //                else
                //                {
                //                    if (communications.ReadFlag)
                //                    {
                //                        Thread.Sleep(200);
                //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                //                        passwordData = communications.OutBuffer;
                //                        break;
                //                    }
                //                }
                //            } while (true);

                //            if (passwordData != string.Empty)
                //            {
                //                string commandPassword = AlgorithemicSignOn(passwordData);


                //                communications.DelayExecution();
                //                communications.CurrentTime = DateTime.Now;
                //                communications.ReadFlag = false;
                //                communications.Command = commandPassword;
                //                communications.CommandID = 2;
                //                communications.OutBuffer = "";
                //                communications.IsDataReceived = false;
                //                communications.SendCommand();

                //                Thread.Sleep(200);
                //                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                //                do
                //                {
                //                    if (communications.Timeout())
                //                    {
                //                        break;
                //                    }
                //                    if (communications.ReadFlag)
                //                        break;
                //                    if (IsAborted)
                //                    {
                //                        this.StatusMessage = "User Aborted.";
                //                        Application.DoEvents();
                //                        return string.Empty;
                //                    }
                //                } while (true);

                //                if (communications.OutBuffer.Length >= 0)
                //                {
                //                    if (communications.OutBuffer.Contains("\x06"))
                //                    {
                //                        communications.DelayExecution();
                //                        communications.CurrentTime = DateTime.Now;
                //                        communications.ReadFlag = false;

                //                        communications.Command = command.DTMDailySurveyCommand_B;
                //                        communications.CommandID = 2;
                //                        communications.OutBuffer = "";
                //                        communications.IsDataReceived = false;
                //                        communications.SendCommand();
                //                        data = "";
                //                        Thread.Sleep(100);
                //                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                //                        do
                //                        {
                //                            if (communications.Timeout())
                //                            {
                //                                this.StatusMessage = MessageConstant.GetText("M000040");
                //                                Application.DoEvents();
                //                                break;
                //                            }
                //                            //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
                //                            //{
                //                            //    if (communications.OutBuffer.Contains("\x04"))
                //                            //    {
                //                            //    }
                //                            //    else
                //                            //    {
                //                            //        this.StatusMessage = "Read Completed";
                //                            //        Application.DoEvents();
                //                            //        data += communications.OutBuffer;
                //                            //        break;
                //                            //    }
                //                            //}

                //                            if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                //                            {
                //                                if (communications.OutBuffer.Contains("\x04"))
                //                                {
                //                                }
                //                                else
                //                                {
                //                                   // this.StatusMessage = "Read Completed";
                //                                   // Application.DoEvents();
                //                                    data += communications.OutBuffer;
                //                                    break;
                //                                }
                //                            }
                //                            if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                //                            {
                //                                data = data + communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1);
                //                                communications.DelayExecution();
                //                                communications.CurrentTime = DateTime.Now;
                //                                communications.ReadFlag = false;
                //                                communications.Command = "06";
                //                                communications.CommandID = 2;
                //                                communications.OutBuffer = "";
                //                                communications.IsDataReceived = false;
                //                                communications.SendCommand();
                //                                Thread.Sleep(100);
                //                                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                //                            }
                //                            if (IsAborted)
                //                            {
                //                                this.StatusMessage = "User Aborted.";
                //                                Application.DoEvents();
                //                                data = string.Empty;
                //                                break;
                //                            }
                //                        } while (true);
                //                    }
                //                }
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
            return data;
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

        #endregion
        /* GKG 04/03/2013 TANGEDCO ISSUE*/
        public override string GetFirmWareVersion()
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
                }
                else
                {
                    communications.DelayExecution();
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    if (communications.SignOn())
                    {
                        communications.DelayExecution();
                        communications.Command = command.FraudEnergyManfCommand;
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
                        string headerInfoCommand = "3A31313131313131314D73040D0A";
                        headerInfoCommand = headerInfoCommand.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                        string CmdBcc = ReadoutCommon.ReturnBcc(headerInfoCommand.Substring(0, headerInfoCommand.Length - 7));
                        headerInfoCommand = headerInfoCommand.Replace(ReadoutConstant.BCC, CmdBcc);
                        communications.Command = headerInfoCommand;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        communications.IsDataReceived = false;
                        communications.ReadFlag = false;
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(200);
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
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

                        } while (communications.OutBuffer.Length < 25);
                        if (communications.ReadFlag)
                        {
                            data = communications.OutBuffer;
                            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                                data = string.Empty;
                            data = String.Format("{0:0.00}", Convert.ToDouble(Convert.ToInt32(data.Substring(data.Length - 6, 4), 16)) / 100);

                        }
                    }
                    else
                        IsSignOnFailure = true;
                }
            }
            catch (Exception)
            {
                data = string.Empty;
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }
        /* GKG 04/03/2013 TANGEDCO ISSUE*/

        public override string GetData()
        {
            /* GKG 04/03/2013 TANGEDCO ISSUE*/
            string fwVersion = string.Empty;

            if (isTNEB)
            {
                fwVersion = GetFirmWareVersion();
            }
            /* GKG 04/03/2013 TANGEDCO ISSUE*/

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
                    communications.DelayExecution();
                    communications.Command = command.DTMDayManfCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            Application.DoEvents();
                            isCorruptedData = true;
                            return data;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                    string commandPassword;
                    if (isTNEB)
                    {
                        /* GKG 04/03/2013 TANGEDCO ISSUE*/
                        if (fwVersion == "0.27" || fwVersion == "0.70" || fwVersion == "0.78" || fwVersion == "1.29" || fwVersion == "1.56")
                        {
                            commandPassword = command.DTMDayPasswordCommand;
                        }
                        else
                        {
                            commandPassword = command.DTMNonTNEBDayPasswordCommand;
                        }
                        /* GKG 04/03/2013 TANGEDCO ISSUE*/
                    }
                    else
                    {
                        commandPassword = command.DTMNonTNEBDayPasswordCommand;
                    }

                    commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ReadoutCommon.SetPassword(ReadoutConstant.METERPASSWORD));
                    commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(0, commandPassword.Length - 7)));
                    communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    communications.ReadFlag = false;
                    communications.Command = commandPassword;
                    communications.CommandID = 2;
                    communications.OutBuffer = "";
                    communications.IsDataReceived = false;
                    communications.SendCommand();
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            return data;
                        }
                        if (communications.ReadFlag)
                            break;
                        if (IsAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            Application.DoEvents();
                            return string.Empty;
                        }
                    } while (true);
                    if (communications.OutBuffer.Length >= 3)
                    {
                        Thread.Sleep(200);
                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Daily Profile for single phase
                        bool Bccres = ReadoutCommon.CalculateBcc(communications.OutBuffer.Substring(1), communications.OutBuffer.Length - 3, communications.OutBuffer.Substring(communications.OutBuffer.Length - 1, 1));
                        if (Bccres)
                        {
                            data = communications.OutBuffer;
                        }
                    }
                }
                else
                    IsSignOnFailure = true;
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

        public override string GetDataDS(string numberOfDays, int avaialbleDays)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            string parameterData = string.Empty; ;
            int noOfDSAvailable = 90;
           
            try
            {
                if (!ConnectToIECMeter()) return string.Empty;
              
                #region ZCommand

                Thread.Sleep(200);
                communications.Command = command.DailySurveyConfigurationCommand;
                Thread.Sleep(1200);
                // Get the meter Load survey data 
                if (!GetMeterBufferData()) return string.Empty;
           


                #endregion

                if (communications.OutBuffer.Length >= 13 && communications.OutBuffer.Contains("\x03"))
                {
                    parameterData = communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 2);
                    parameterData = parameterData + "\x04";

                }
                if (communications.OutBuffer.Length >= 7 && communications.OutBuffer.Contains("\x03"))
                {
                    parameterData = communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 2);
                    parameterData = parameterData + "\x04";

                }

                if (parameterData.Length >= 12)
                {
                    noOfDSAvailable = Convert.ToByte(parameterData.Substring(8, 2), 16);                   
                }               
                else if (parameterData.ToUpper().Contains("ER"))
                {
                    noOfDSAvailable = 90;
                   
                }
                else
                    parameterData = "(ER30)";
                
                communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;
                string cmdS = command.DTMDailySurveyCommand_B;
                if (noOfDSAvailable <= 0) 
                    return ReadoutNotSupported = "Readout Not Supported";
                else
                    noOfDSAvailable = noOfDSAvailable + 1;

                cmdS = cmdS.Replace("ND", ReadoutCommon.DTMStringToHex3(noOfDSAvailable.ToString("00")));
                cmdS = cmdS.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(cmdS.Substring(2, cmdS.Length - 5)));

                communications.Command = cmdS;
                bool isDPLastEvent = false;
                while (!isDPLastEvent)
                {
                    if (!GetMeterBufferData()) return string.Empty;
                    string internalData = string.Empty;
                    // handeled ETX Issue when BCC coming as 03
                    //if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1))) isDPLastEvent = true;                   
                    if (communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1).Contains("\x03")) isDPLastEvent = true;

                    //Bug 504948: Removing Last character BCC from the outbuffer data for correct file creation beacuse last character is not of use in file parsing
                    data += communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1);

                    communications.Command = "06";
                    Application.DoEvents();
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                //communications.DelayExecution();
                //communications.Command = command.BreakCommand;
                //communications.SendCommand();
                //communications.DelayExecution();
                //communications.Command = command.BreakCommand;
                //communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            //Thread.Sleep(200);
            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
            data = parameterData + data;
            return data;
        }
        public static int GetMaxLSDays(string CmdResponse, int m_dailysurvey)
        {           
            int DailySurveyDays = m_dailysurvey;
           
            if (CmdResponse.Length > 11)
            {
                string s = CmdResponse.Substring(CmdResponse.Length - 3, 2);
              DailySurveyDays =   int.Parse( s,System.Globalization.NumberStyles.HexNumber);
            }
            else if (CmdResponse.Length.ToString().Contains("Err"))
            {
                DailySurveyDays = 90;
            }          

            return DailySurveyDays;
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        } 
    }

}