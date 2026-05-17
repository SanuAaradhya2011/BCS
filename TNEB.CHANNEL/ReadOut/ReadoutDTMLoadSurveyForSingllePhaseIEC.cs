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
    public class ReadoutDTMLoadSurveyForSingllePhaseIEC : ReadBase
    {
        int commandreTry = 3;
        private static string responseForLoadSurvey;
        public ReadoutDTMLoadSurveyForSingllePhaseIEC()
        {
            command = Command.GetInstance();
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
                commandreTry = 3;
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
                communications.CurrentTime = DateTime.Now;
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading tamper for single phase
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

        public string GetData(string numberOfDays, int avaialbleDays)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            string parameterData = string.Empty; ;
            byte noOfLSAvailable = 90;

            numberOfDays = (Int32.Parse(numberOfDays) > 255) ? "255" : numberOfDays; //SarkarA code change 20180529 //limit ls days to FF as ceiling

            byte noOFDaysFromSettings = Convert.ToByte(numberOfDays);
            try
            {
                if (!ConnectToIECMeter()) return string.Empty;
                /*
                Thread.Sleep(1000);
                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOnForSPhaseIEC())
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
                        string passwordData = string.Empty;
                        communications.ReadFlag = false;
                        communications.Command = command.DTMDailySurveyCommand_A;
                        communications.CommandID = 2;
                        communications.OutBuffer = string.Empty;
                        if (!communications.SendCommand())
                            return "2";
                        else
                        {
                            //Thread.Sleep(200);
                            // Mohsin - 19-10-15
                            Thread.Sleep(200);
                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                            int index = communications.ResponseSignOn.IndexOf("/");
                            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                            Thread.Sleep(400);
                            Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                            //Application.DoEvents();
                            do
                            {
                                if (!IsAborted)
                                {
                                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                                    //Application.DoEvents();
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
                                    Application.DoEvents();
                                    return "1";
                                }
                                else
                                {
                                    if (communications.ReadFlag && communications.OutBuffer.Length >= 16)
                                    {
                                        Thread.Sleep(200);
                                        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                                        passwordData = communications.OutBuffer;
                                        break;
                                    }
                                }
                            } while (true);

                            if (passwordData != string.Empty)
                            {
                                string commandPassword = AlgorithemicSignOn(passwordData);
                                communications.DelayExecution();
                                communications.CurrentTime = DateTime.Now;
                                communications.ReadFlag = false;
                                communications.Command = commandPassword;
                                communications.CommandID = 2;
                                communications.OutBuffer = "";
                                communications.IsDataReceived = false;
                                communications.SendCommand();

                                Thread.Sleep(200);
                                Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                                do
                                {
                                    if (communications.Timeout())
                                    {
                                        break;
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
                                */
                #region ZCommand

                Thread.Sleep(200);
                communications.Command = command.LoadSurveyZCommand;
                // Get the meter Load survey data 
                if (!GetMeterBufferData()) return string.Empty;

                //communications.DelayExecution();
                //communications.CurrentTime = DateTime.Now;
                //communications.ReadFlag = false;
                //communications.Command = command.LoadSurveyZCommand;
                //communications.CommandID = 2;
                //communications.OutBuffer = "";
                //communications.IsDataReceived = false;
                //communications.SendCommand();
                //data = "";
                //Thread.Sleep(100);
                //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                //do
                //{
                //    if (communications.Timeout())
                //    {
                //        this.StatusMessage = MessageConstant.GetText("M000040");
                //        Application.DoEvents();
                //        break;
                //    }
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
                Thread.Sleep(200);
                //    if (IsAborted)
                //    {
                //        this.StatusMessage = "User Aborted.";
                //        Application.DoEvents();
                //        data = string.Empty;
                //    }
                //} while (true);

                #endregion

                #region LCommand
                if (parameterData.Length >= 12)
                {
                    noOfLSAvailable = Convert.ToByte(parameterData.Substring(8, 2), 16);

                    noOfLSAvailable = Convert.ToByte(GetMaxLSDays(parameterData, noOfLSAvailable));

                    if (noOFDaysFromSettings < noOfLSAvailable)
                    {
                        noOfLSAvailable = noOFDaysFromSettings;
                    }
                }
                // Handle Starlight meters
                else if (parameterData.Length >= 6)
                {
                    // Max Days 90 in case of Starlight TPDDL
                    noOfLSAvailable = 90;

                    noOfLSAvailable = Convert.ToByte(GetMaxLSDays(parameterData, noOfLSAvailable));

                    if (noOFDaysFromSettings < noOfLSAvailable)
                    {
                        noOfLSAvailable = noOFDaysFromSettings;
                    }
                }


                communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;
                string cmdLS = command.LoadSurveyLCommand;

                cmdLS = cmdLS.Replace("ND", ReadoutCommon.DTMStringToHex3(noOfLSAvailable.ToString("00")));
                cmdLS = cmdLS.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(cmdLS.Substring(2, cmdLS.Length - 5)));

                communications.Command = cmdLS;
                bool isDPLastEvent = false;
                while (!isDPLastEvent)
                {
                    if (!GetMeterBufferData()) return string.Empty;
                    string internalData = string.Empty;
                    // handeled ETX Issue when BCC coming as 03
                    //if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1))) isDPLastEvent = true;                   
                    if (communications.OutBuffer.Substring(0,communications.OutBuffer.Length-1).Contains("\x03")) isDPLastEvent = true;

                    //Bug 504948: Removing Last character BCC from the outbuffer data for correct file creation beacuse last character is not of use in file parsing
                    data += communications.OutBuffer.Substring(0,communications.OutBuffer.Length-1);

                    communications.Command = "06";
                    Application.DoEvents();
                    Application.DoEvents();
                }

                //communications.CommandID = 2;
                //communications.OutBuffer = "";
                //communications.IsDataReceived = false;
                //communications.SendCommand();
                ////data = "";
                //Thread.Sleep(100);
                //Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                //do
                //{
                //    if (communications.Timeout())
                //    {
                //        this.StatusMessage = MessageConstant.GetText("M000040");
                //        Application.DoEvents();
                //        break;
                //    }
                //    //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
                //    //{
                //    if (communications.OutBuffer.Contains("\x03") && (communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x03") + 1)))
                //    {
                //        if (communications.OutBuffer.Contains("\x04"))
                //        {

                //        }
                //        else
                //        {
                //           // this.StatusMessage = "Read Completed";
                //           // Application.DoEvents();
                //            data += communications.OutBuffer;
                //            break;
                //        }
                //    }
                //    //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x04"))
                //    //{
                //    if (communications.OutBuffer.Contains("\x04") && communications.OutBuffer.Length >= (communications.OutBuffer.IndexOf("\x04") + 1))
                //    {
                //        data = data + communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1);
                //        communications.DelayExecution();
                //        communications.CurrentTime = DateTime.Now;
                //        communications.ReadFlag = false;
                //        communications.Command = "06";
                //        communications.CommandID = 2;
                //        communications.OutBuffer = "";
                //        communications.IsDataReceived = false;
                //        communications.SendCommand();
                //        Thread.Sleep(100);
                //        Application.DoEvents();// Story - 354382 - Progress bar is not moving while reading Load Survey for single phase
                //    }
                //    if (IsAborted)
                //    {
                //        this.StatusMessage = "User Aborted.";
                //        Application.DoEvents();
                //        data = string.Empty;
                //    }
                //} while (true);

                #endregion
                //}
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

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        } 

        public static int GetMaxLSDays(string CmdResponse, int m_LoadSurvey)
        {
            string IP = "";
            int lsdays = m_LoadSurvey;
            if (CmdResponse.Length < 8)
            {
                IP = CmdResponse.Substring(CmdResponse.IndexOf('(') + 1, 2);
                IP = Convert.ToString(Convert.ToInt32(IP, 16), 2);
                while (IP.Length < 8) { IP = "0" + IP; }
                IP = IP.Substring(2, 2);
                //User story taskID: 490975
                if (IP == "00") lsdays = lsdays / 2;
                else if (IP == "11") lsdays = lsdays * 2;

            }
            else
            {
                IP = CmdResponse.Substring(CmdResponse.IndexOf('(') + 1, 2);
                IP = Convert.ToString(Convert.ToInt32(IP, 16), 2);
                string tempdays = CmdResponse.Substring(CmdResponse.IndexOf('(') + 7, 2);
                lsdays = Convert.ToInt16(tempdays, 16);
                string byte1 = "";
                string byte2 = "";
                byte1 = CmdResponse.Substring(CmdResponse.IndexOf('(') + 3, 2);
                byte1 = Convert.ToString(Convert.ToInt32(byte1, 16), 2);
                byte2 = CmdResponse.Substring(CmdResponse.IndexOf('(') + 5, 2);
                byte2 = Convert.ToString(Convert.ToInt32(byte2, 16), 2);
                byte1 = byte1.PadLeft(8, '0');
                byte2 = byte2.PadLeft(8, '0');
                byte1 = ReverseString(byte1);
                byte2 = ReverseString(byte2);
                IP = IP.PadLeft(8, '0');
                string lstabledata = byte1 + byte2;
                IP = IP.Substring(2, 2);

                //if (IP == "10") lsdays = lsdays;
                if (IP == "00") lsdays = lsdays / 2;
                else if (IP == "11") lsdays = lsdays * 2;
               
            }

            return lsdays;
        }

        //public static int GetMaxLSDays_Backup(int meterIP, int m_LoadSurvey)
        //{
        //    int maxIP = 0;
        //    try
        //    {
        //        maxIP = m_LoadSurvey;                    // 30 Min IP
        //        if (m_LoadSurvey <= 0) return 0;
        //        if (meterIP == 0) maxIP = maxIP / 2;      // 15 Min IP   
        //        else if (meterIP == 3) maxIP = maxIP * 2; // 60 min IP
        //        return maxIP;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

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
    }
}

